Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class ListaTrasmissioneAttiPage
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
        Me.MainPage.DescrizioneProcedura = "> Lista Trasmissione Atti"
        If Not Me.Page.IsPostBack Then
            Me.Documenti = New List(Of ParsecAtt.Documento)
            Me.CaricaTipologieRegistro()
            Me.ResettaFiltro()
            Me.SelectAllCheckBox.Enabled = False
            Me.stampaButton.Enabled = False
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If


        Me.ContatoreGeneraleInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.ContatoreGeneraleInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.ContatoreGeneraleFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

        Me.ContatoreSettoreInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.ContatoreSettoreInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.ContatoreSettoreFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.PannelloRisultatiLabel.Text = "Elenco Proposte Determina&nbsp;&nbsp;&nbsp;" & If(Me.AttiListBox.Items.Count > 0, "( " & Me.AttiListBox.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "PROPRIETA'"

    Public Property Documenti() As List(Of ParsecAtt.Documento)
        Get
            Return CType(Session("ListaTrasmissioneAttiPage_Documenti"), List(Of ParsecAtt.Documento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Documento))
            Session("ListaTrasmissioneAttiPage_Documenti") = value
        End Set
    End Property

#End Region


#Region "EVENTI CONTROLLI"


    Protected Sub StampaButton_Click(sender As Object, e As System.EventArgs) Handles StampaButton.Click
        Try
            Me.GeneraDataSourceListaTrasmissione()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message.ToString, False)
        End Try
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Try
                Me.ApplicaFiltro()
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    Protected Sub TrovaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaSettoreImageButton.Click
        Me.TrovaSettore()
    End Sub

    Protected Sub AggiornaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSettoreImageButton.Click
        Me.AggiornaSettore()
    End Sub

    Protected Sub EliminaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaSettoreImageButton.Click
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
    End Sub

    Protected Sub TrovaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUfficioImageButton.Click
        Me.TrovaUfficio()
    End Sub

    Protected Sub AggiornaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUfficioImageButton.Click
        Me.AggiornaUfficio()
    End Sub

    Protected Sub EliminaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUfficioImageButton.Click
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
    End Sub

    Protected Sub notificaOperazioneButton_Click(sender As Object, e As System.EventArgs) Handles notificaOperazioneButton.Click
        Me.NotificaOperazione()
    End Sub

    Private Sub NotificaOperazione()
        Me.infoOperazioneHidden.Value = "Stampa conclusa con successo!"
    End Sub

#End Region


#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

#Region "METODI PRIVATI"


    Private Sub ApplicaFiltro()

        Dim filtro As ParsecAtt.FiltroDocumento = Me.GetFiltro
        Dim documenti As New ParsecAtt.DocumentoRepository
        Me.Documenti = documenti.GetView(filtro)

        If Me.Documenti.Count = 0 Then
            Me.AttiListBox.Items.Clear()
            Me.stampaButton.Enabled = False
            Me.SelectAllCheckBox.Checked = False
            Me.SelectAllCheckBox.Enabled = False
            Throw New ApplicationException("Nessuna proposta di determina trovata con i criteri di filtro impostati!")
        End If

        Me.AttiListBox.DataSource = Me.Documenti
        Me.AttiListBox.DataValueField = "Id"
        Me.AttiListBox.DataTextField = "Descrizione"
        Me.AttiListBox.DataBind()

        documenti.Dispose()

        Me.SelectAllCheckBox.Checked = True
        Me.SelectAllCheckBox.Enabled = True
        Me.StampaButton.Enabled = True
        Dim checked As Boolean = Me.SelectAllCheckBox.Checked
        For i As Integer = 0 To Me.AttiListBox.Items.Count - 1
            Me.AttiListBox.Items(i).Checked = checked
        Next



    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroDocumento
        Dim filtro As New ParsecAtt.FiltroDocumento

        If Me.ContatoreGeneraleFineTextBox.Value.HasValue Then
            filtro.ContatoreGeneraleFine = CInt(Me.ContatoreGeneraleFineTextBox.Value)
        End If

        If Me.ContatoreSettoreInizioTextBox.Value.HasValue Then
            filtro.ContatoreSettoreInizio = CInt(Me.ContatoreSettoreInizioTextBox.Value)
        End If

        filtro.DataDocumentoInizio = Me.DataDocumentoInizioTextBox.SelectedDate
        filtro.DataDocumentoFine = Me.DataDocumentoFineTextBox.SelectedDate


        If Me.ContatoreGeneraleInizioTextBox.Value.HasValue Then
            filtro.ContatoreGeneraleInizio = CInt(Me.ContatoreGeneraleInizioTextBox.Value)
        End If


        If Me.ContatoreSettoreFineTextBox.Value.HasValue Then
            filtro.ContatoreSettoreFine = CInt(Me.ContatoreSettoreFineTextBox.Value)
        End If

        'If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
        '    filtro.IdSettore = CInt(Me.IdSettoreTextBox.Text)
        'Else
        filtro.Settore = Me.SettoreTextBox.Text.Trim
        'End If

        'If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
        '    filtro.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        'Else
        filtro.Ufficio = Me.UfficioTextBox.Text.Trim
        'End If

        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            filtro.Oggetto = Me.OggettoTextBox.Text
        End If

        If Me.TipologieRegistroComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaRegistro = CInt(Me.TipologieRegistroComboBox.SelectedValue)
        End If

        filtro.IdTipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina
        filtro.Adottato = False

        filtro.ApplicaAbilitazione = True

        Return filtro
    End Function

    Private Sub GeneraDataSourceListaTrasmissione()
        Dim template As String = "templateTrasmissioneAtti.odt"

        Dim localPathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")
        Dim input As String = localPathTemplate & template

        If Not IO.File.Exists(input) Then
            Throw New ApplicationException(String.Format("Il file '{0}' non esiste!", template))
        End If

        Dim listaId As List(Of Integer) = Me.AttiListBox.CheckedItems.Select(Function(c) CInt(c.Value)).ToList

        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters

        Dim remotePathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates")
        Dim remotePathReport As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocumentiTemp")



        Dim datiInput As New ParsecAdmin.DatiInput With {
             .SrcRemotePath = remotePathTemplate & template,
             .DestRemotePath = remotePathReport & "Report.odt",
             .ShowWindow = True,
             .Enabled = True,
             .FunctionName = "ProcessDocument"}

        Dim documentiSelezionati = Me.Documenti.Where(Function(c) listaId.Contains(c.Id))

        If documentiSelezionati.Count > 0 Then
            Dim dt As New DataTable("TabellaDocumenti")
            Dim row As DataRow = Nothing

            dt.Columns.Add(New DataColumn("docDataProposta", GetType(System.String)))
            dt.Columns.Add(New DataColumn("docContatoreGenerale", GetType(System.String)))
            dt.Columns.Add(New DataColumn("docOggetto", GetType(System.String)))
            dt.Columns.Add(New DataColumn("settore", GetType(System.String)))

            For Each documento In documentiSelezionati
                row = dt.NewRow
                row("docDataProposta") = documento.DataProposta.Value.ToShortDateString
                row("docContatoreGenerale") = documento.ContatoreGenerale
                row("docOggetto") = documento.Oggetto
                row("settore") = documento.DescrizioneSettore
                dt.Rows.Add(row)
            Next

            Dim data As String = openofficeParameters.CreateDataSource(datiInput, Nothing, dt)



            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            'UTILIZZO L'APPLET
            If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(data, Me.notificaOperazioneButton.ClientID, True, False)
            Else
                'UTILIZZO IL SOCKET
                ParsecUtility.Utility.EseguiServerComunicatorService(data, True, AddressOf Me.NotificaOperazione)
            End If


        End If

    End Sub


    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean


        If Me.ContatoreGeneraleFineTextBox.Value.HasValue AndAlso Me.ContatoreGeneraleInizioTextBox.Value.HasValue Then
            If Me.ContatoreGeneraleInizioTextBox.Value > Me.ContatoreGeneraleFineTextBox.Value Then
                message.AppendLine("Il campo 'Registro Generale da' deve essere inferiore o uguale al campo 'Registro Generale a'!")
            End If
        End If

        If Me.ContatoreSettoreFineTextBox.Value.HasValue AndAlso Me.ContatoreSettoreInizioTextBox.Value.HasValue Then
            If Me.ContatoreSettoreInizioTextBox.Value > Me.ContatoreSettoreFineTextBox.Value Then
                message.AppendLine("Il campo 'Registro Settore da' deve essere inferiore o uguale al campo 'Registro Settore a'!")
            End If
        End If

        'Se la data iniziale è maggiore di quella finale.
        If Me.DataDocumentoInizioTextBox.SelectedDate.HasValue AndAlso Me.DataDocumentoFineTextBox.SelectedDate.HasValue Then
            If Date.Compare(Me.DataDocumentoInizioTextBox.SelectedDate, Me.DataDocumentoFineTextBox.SelectedDate) > 0 Then
                message.AppendLine("Il campo 'Data Documento da' deve essere antecedente o uguale al campo 'Data Documento a'!")
            End If
        End If

        Return Not message.Length > 0
        Return True
    End Function

    Private Sub ResettaFiltro()
        Me.DataDocumentoInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataDocumentoFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)

        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty

        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty

        Me.ContatoreGeneraleInizioTextBox.Text = String.Empty
        Me.ContatoreGeneraleFineTextBox.Text = String.Empty

        Me.ContatoreSettoreInizioTextBox.Text = String.Empty
        Me.ContatoreSettoreFineTextBox.Text = String.Empty

        Me.OggettoTextBox.Text = String.Empty


        Me.TipologieRegistroComboBox.SelectedIndex = 0

        Me.AttiListBox.Items.Clear()

        Me.SelectAllCheckBox.Enabled = False
        Me.SelectAllCheckBox.Checked = False
        Me.stampaButton.Enabled = False

    End Sub


    Private Sub CaricaTipologieRegistro(idTipologiaDocumento As Nullable(Of Integer))
        Dim tipologie As New ParsecAtt.TipologieRegistroRepository
        Me.TipologieRegistroComboBox.DataValueField = "Id"
        Me.TipologieRegistroComboBox.DataTextField = "Descrizione"
        Me.TipologieRegistroComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaRegistro With {.Disattivato = False, .IdTipologiaDocumento = idTipologiaDocumento})
        Me.TipologieRegistroComboBox.DataBind()
        Me.TipologieRegistroComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieRegistroComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub CaricaTipologieRegistro()
        Me.CaricaTipologieRegistro(ParsecAtt.TipoDocumento.PropostaDetermina)
    End Sub

    Private Sub AggiornaSettore()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim idSettore As Integer = struttureSelezionate.First.Id
            'Aggiorno il settore
            Me.SettoreTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdSettoreTextBox.Text = idSettore.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Private Sub TrovaSettore()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaSettoreImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 910, 670, queryString, False)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100")
        parametriPagina.Add("ultimoLivelloStruttura", "100")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    End Sub

    Private Sub AggiornaUfficio()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim idUfficio As Integer = struttureSelezionate.First.Id
            'Aggiorno l'ufficio
            Me.UfficioTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdUfficioTextBox.Text = idUfficio.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Private Sub TrovaUfficio()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUfficioImageButton.ClientID)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "200")
        parametriPagina.Add("ultimoLivelloStruttura", "200")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

#End Region


End Class