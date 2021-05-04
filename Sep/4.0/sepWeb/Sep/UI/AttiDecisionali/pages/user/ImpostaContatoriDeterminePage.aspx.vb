Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class ImpostaContatoriDeterminePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Private annoCorrente As Integer = 0




    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Imposta Contatori Determine"

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AnnoCorrente", ParsecAdmin.TipoModulo.ATT)

        Me.annoCorrente = -1
        If Not parametro Is Nothing Then
            Integer.TryParse(parametro.Valore, Me.annoCorrente)
        End If

        parametri.Dispose()

        If Me.annoCorrente <> -1 Then
            Me.AnnoCorrenteLabel.Text = "Anno corrente: " & Me.annoCorrente.ToString
        Else
            Me.MessaggioLabel.Text = "Contattare l'amministratore, non è definito l'anno di esercizio per il modulo Atti Decisionali."
            Me.SalvaButton.Enabled = False
        End If

        Dim contatoriGenerale As New ParsecAtt.ContatoreGeneraleRepository
        Dim tipologie As New ParsecAtt.TipologieRegistroRepository(contatoriGenerale.Context)

        Dim view = From contatore In contatoriGenerale.GetQuery
                  Group Join tipologia In tipologie.GetQuery
                  On tipologia.Id Equals contatore.IdTipologiaRegistro
                  Into elencoTipologie = Group
                  From tipologia In elencoTipologie.DefaultIfEmpty
                  Where tipologia.Descrizione.Contains("determina") And contatore.Anno = annoCorrente And contatore.IdTipologiaRegistro = 4
                 Select contatore


        Dim contatoreGenerale As ParsecAtt.ContatoreGenerale = view.FirstOrDefault

        If Not contatoreGenerale Is Nothing Then
            Me.IdContatoreGeneraleTextBox.Text = contatoreGenerale.Id.ToString
            Me.NuovoContatoreGeneraleTextBox.Text = contatoreGenerale.Valore
            Me.ContatoreGeneraleTextBox.Text = contatoreGenerale.Valore
        Else
            'Me.MessaggioLabel.Text = "Contattare l'amministratore, non è definito l'anno di esercizio per il modulo Atti Decisionali."
            'Me.SalvaButton.Enabled = False
        End If

        contatoriGenerale.Dispose()

        Me.SalvaButton.Attributes.Add("onclick", "this.disabled=true;")
    End Sub

    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder
        Dim contatore As Integer = -1
        If Not String.IsNullOrEmpty(Me.NuovoContatoreGeneraleTextBox.Text) Then
            Integer.TryParse(Me.NuovoContatoreGeneraleTextBox.Text, contatore)
            If contatore < 1 Then
                message.AppendLine("E' necessario valorizzare i contatori con valori numerici maggiori di zero!")
            End If
        End If

        If message.Length = 0 Then
            For Each it As Telerik.Web.UI.GridDataItem In Me.ContatoriSettoreGridView.Items
                Dim NuovoContatoreSettoreTextBox As Telerik.Web.UI.RadNumericTextBox = CType(it.FindControl("NuovoContatoreSettoreTextBox"), Telerik.Web.UI.RadNumericTextBox)
                contatore = -1
                If Not String.IsNullOrEmpty(NuovoContatoreSettoreTextBox.Text) Then
                    Integer.TryParse(NuovoContatoreSettoreTextBox.Text, contatore)
                    If contatore < 1 Then
                        message.AppendLine("E' necessario valorizzare i contatori con valori numerici maggiori di zero!")
                        Exit For
                    End If
                End If
            Next

        End If

        If message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If


        Return message.Length = 0
    End Function


    Private Sub Save()

        Dim contatoriGenerale As New ParsecAtt.ContatoreGeneraleRepository
        Dim contatoriSettore As New ParsecAtt.ContatoreSettoreRepository
        Dim contatore As Integer = 0
        Try

            If Not String.IsNullOrEmpty(Me.NuovoContatoreGeneraleTextBox.Text) Then
                Dim contatoreGenerale As ParsecAtt.ContatoreGenerale = contatoriGenerale.GetQuery.Where(Function(c) c.Id = CInt(Me.IdContatoreGeneraleTextBox.Text)).FirstOrDefault
                If contatoreGenerale Is Nothing Then
                    contatoreGenerale = New ParsecAtt.ContatoreGenerale
                End If
                Integer.TryParse(Me.NuovoContatoreGeneraleTextBox.Text, contatore)
                contatoreGenerale.Valore = contatore
                contatoriGenerale.Save(contatoreGenerale)
                Me.ContatoreGeneraleTextBox.Text = contatoreGenerale.Valore
            End If

            For Each it As Telerik.Web.UI.GridDataItem In Me.ContatoriSettoreGridView.Items
                Dim NuovoContatoreSettoreTextBox As Telerik.Web.UI.RadNumericTextBox = CType(it.FindControl("NuovoContatoreSettoreTextBox"), Telerik.Web.UI.RadNumericTextBox)
                'Dim s = it.Cells(2).Text
                Dim idContatoreSettore As Integer = CInt(it("Id").Text)
                If Not String.IsNullOrEmpty(NuovoContatoreSettoreTextBox.Text) Then
                    Integer.TryParse(NuovoContatoreSettoreTextBox.Text, contatore)
                    Dim contatoreSettore As ParsecAtt.ContatoreSettore = contatoriSettore.GetQuery.Where(Function(c) c.Id = idContatoreSettore).FirstOrDefault
                    contatoreSettore.Valore = contatore
                    contatoriSettore.Save(contatoreSettore)
                End If
            Next
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            contatoriGenerale.Dispose()
            contatoriSettore.Dispose()
        End Try
    End Sub


    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Me.SalvaButton.Enabled = False
        Dim message As String = "Operazione conclusa con successo!"
        Try
            ' If Me.VerificaDati Then
            Me.Save()
            ParsecUtility.Utility.MessageBox(message, False)
            'Me.AnnoCorrenteLabel.Text = "Anno corrente: " & Me.annoCorrente.ToString
            Me.MessaggioLabel.Text = "I contatori, per l'anno " & annoCorrente.ToString & " sono stati aggiornati!"
            ' End If
        Catch ex As Exception
            message = ex.Message
            'Me.SalvaButton.Enabled = True
            ParsecUtility.Utility.MessageBox(message, False)
        End Try
        Me.SalvaButton.Enabled = True
    End Sub

    Protected Sub ContatoriSettoreGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ContatoriSettoreGridView.NeedDataSource
        Dim contatoriSettore As New ParsecAtt.ContatoreSettoreRepository
        Me.ContatoriSettoreGridView.DataSource = contatoriSettore.GetView
        contatoriSettore.Dispose()
    End Sub



    Protected Sub ContatoriSettoreGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ContatoriSettoreGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

End Class