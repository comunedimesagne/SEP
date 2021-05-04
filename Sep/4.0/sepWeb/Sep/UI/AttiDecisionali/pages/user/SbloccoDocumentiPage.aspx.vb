Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class SbloccoDocumentiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadListBox .rlbCheck,.RadListBox .rlbImage,.RadListBox .rlbText{vertical-align: middle;padding-left:5px;}"
        Me.Page.Header.Controls.Add(css)

        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Sblocco Documenti"
       
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        Me.ApplicaFiltro()
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.PannelloRisultatiLabel.Text = "Elenco Atti&nbsp;&nbsp;&nbsp;" & If(Me.AttiListBox.Items.Count > 0, "( " & Me.AttiListBox.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

   Protected Sub SbloccaButton_Click(sender As Object, e As System.EventArgs) Handles SbloccaButton.Click
        Try
            Me.SbloccaDocumentiSelezionati()
            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SbloccaDocumentiSelezionati()
        Dim documentiBloccati As New ParsecAtt.LockDocumentoRepository
        Dim listaIdDocumentiBloccati As List(Of Integer) = Me.AttiListBox.CheckedItems.Select(Function(c) CInt(c.Value)).ToList

        For Each idDocumentoBloccato In listaIdDocumentiBloccati
            documentiBloccati.Delete(idDocumentoBloccato)
        Next
        documentiBloccati.Dispose()
    End Sub

    Private Sub ApplicaFiltro()

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim utenti As New ParsecAtt.UtenteViewRepository(documenti.Context)
        Dim documentiBloccati As New ParsecAtt.LockDocumentoRepository(documenti.Context)
        Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository(documenti.Context)

        Dim res = From documento In documenti.GetQuery
                   Join documentoSbloccato In documentiBloccati.GetQuery
                   On documento.Codice Equals documentoSbloccato.CodiceDocumento
                   Join utente In utenti.GetQuery
                   On utente.Id Equals documentoSbloccato.IdUtente
                   Join tipologiaRegestro In tipologieRegistro.GetQuery
                   On tipologiaRegestro.Id Equals documento.IdTipologiaRegistro
                   Where utente.LogTipoOperazione Is Nothing And documento.LogStato Is Nothing
                   Select documento, documentoSbloccato, utente, tipologiaRegestro
                      


        Dim view = res.AsEnumerable.Select(Function(c) New ParsecAtt.KeyValue With {
                                         .Id = c.documentoSbloccato.Id,
                                         .Descrizione = String.Format("{0} n. {1} del {2} - OGGETTO : {3} [Bloccato da: {4} ]", c.tipologiaRegestro.Descrizione, c.documento.ContatoreGenerale, If(c.documento.Data Is Nothing, c.documento.DataProposta.Value.ToShortDateString, c.documento.Data.Value.ToShortDateString), c.documento.Oggetto & Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"), c.utente.Username & " - " & c.utente.Nome & " " & c.utente.Cognome)
                                    }).ToList

        If view.Count = 0 Then
            Me.AttiListBox.Items.Clear()
            Me.SbloccaButton.Enabled = False
            Me.SelectAllCheckBox.Checked = False
            Me.SelectAllCheckBox.Enabled = False
        Else
            Me.AttiListBox.DataSource = view
            Me.AttiListBox.DataValueField = "Id"
            Me.AttiListBox.DataTextField = "Descrizione"
            Me.AttiListBox.DataBind()

            Me.SelectAllCheckBox.Checked = True
            Me.SelectAllCheckBox.Enabled = True
            Me.SbloccaButton.Enabled = True

            Dim checked As Boolean = Me.SelectAllCheckBox.Checked
            For i As Integer = 0 To Me.AttiListBox.Items.Count - 1
                Me.AttiListBox.Items(i).Checked = checked
            Next
        End If

        documenti.Dispose()

    End Sub

  
#End Region

End Class