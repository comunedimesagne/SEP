Imports Telerik.Web.UI

Partial Class AccertamentoPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"


    Public Property Accertamento() As ParsecAtt.Accertamento
        Get
            Return CType(Session("AccertamentoPage_Accertamento"), ParsecAtt.Accertamento)
        End Get
        Set(ByVal value As ParsecAtt.Accertamento)
            Session("AccertamentoPage_Accertamento") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then
            For i As Integer = 0 To 6
                If i = 0 Then
                    Me.TitoliComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
                    Me.CategorieComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
                Else
                    Me.TitoliComboBox.Items.Insert(i, New Telerik.Web.UI.RadComboBoxItem(i.ToString, i.ToString))
                    Me.CategorieComboBox.Items.Insert(i, New Telerik.Web.UI.RadComboBoxItem("0" & i.ToString, "0" & i.ToString))
                End If
            Next

            Me.CaricaAnni()

            If ParsecUtility.SessionManager.Accertamento Is Nothing Then
                Me.ResettaVista()
                Me.TitleLabel.Text = "Nuovo Accertamento"
            Else
                Me.Accertamento = ParsecUtility.SessionManager.Accertamento
                Me.AggiornaVista()
                If Not Me.Request.QueryString("copia") Is Nothing Then
                    Me.TitleLabel.Text = "Nuovo Accertamento"
                Else
                    Me.TitleLabel.Text = "Modifica Accertamento"
                End If

            End If
        End If
    End Sub

#End Region


#Region "METODI PRIVATI"


    Private Sub CaricaAnni()
        Dim anni As New ParsecAtt.AnnoRepository
        Me.AnniComboBox.DataValueField = "Valore"
        Me.AnniComboBox.DataTextField = "Valore"
        Me.AnniComboBox.DataSource = anni.GetQuery.ToList
        Me.AnniComboBox.DataBind()
        anni.Dispose()
    End Sub

    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder

        If String.IsNullOrEmpty(Me.ImportoTextBox.Text) Then
            message.AppendLine("L'importo accertato.")
        End If
        If String.IsNullOrEmpty(Me.DescrizioneTextBox.Text) Then
            message.AppendLine("La descrizione dell'accertamento.")
        End If
        If String.IsNullOrEmpty(Me.CapitoloTextBox.Text) Then
            message.AppendLine("Il capitolo dell'accertamento.")
        End If

        If message.Length > 0 Then
            message.Insert(0, "E' necessario specificare:" & vbCrLf)
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
        Return message.Length = 0
    End Function

    Private Sub ResettaVista()
        Me.ImportoTextBox.Text = String.Empty
        Me.DescrizioneTextBox.Text = String.Empty
        Me.AnniComboBox.Items.FindItemByValue(Now.Year).Selected = True
        Me.CapitoloTextBox.Text = String.Empty
        Me.ArticoloTextBox.Text = String.Empty
        Me.AccertamentoTextBox.Text = String.Empty
        Me.SubAccertamentoTextBox.Text = String.Empty
        Me.TitoliComboBox.SelectedIndex = 0
        Me.CategorieComboBox.SelectedIndex = 0
        Me.RisorsaTextBox.Text = String.Empty

        Me.NumeroAttoAssunzioneTextBox.Text = String.Empty
        Me.DisponibilitaResidualeTextBox.Text = String.Empty
        Me.StanziamentoAssestatoTextBox.Text = String.Empty
        Me.StanziamentoInizialeTextBox.Text = String.Empty


        Me.Accertamento = Nothing
    End Sub

    Private Sub AggiornaVista()
        If Me.Accertamento.Importo.HasValue Then
            Me.ImportoTextBox.Value = Me.Accertamento.Importo
        End If

        Me.DescrizioneTextBox.Text = Me.Accertamento.Note

        If Me.Accertamento.AnnoEsercizio.HasValue Then
            Me.AnniComboBox.Items.FindItemByValue(Me.Accertamento.AnnoEsercizio).Selected = True
        End If


        If Me.Accertamento.Capitolo.HasValue Then
            Me.CapitoloTextBox.Text = Me.Accertamento.Capitolo.ToString
        End If


        If Me.Accertamento.Articolo.HasValue Then
            Me.ArticoloTextBox.Text = Me.Accertamento.Articolo.ToString
        End If


        If Me.Accertamento.NumeroAccertamento.HasValue Then
            Me.AccertamentoTextBox.Text = Me.Accertamento.NumeroAccertamento.ToString
        End If


        Me.SubAccertamentoTextBox.Text = Me.Accertamento.NumeroSubAccertamento

        If Not String.IsNullOrEmpty(Me.Accertamento.Titolo) Then
            Me.TitoliComboBox.Items.FindItemByValue(Me.Accertamento.Titolo).Selected = True
        End If

        If Not String.IsNullOrEmpty(Me.Accertamento.Categoria) Then
            Me.CategorieComboBox.Items.FindItemByValue(Me.Accertamento.Categoria).Selected = True
        End If



        If Me.Accertamento.Risorsa.HasValue Then
            Me.RisorsaTextBox.Text = Me.Accertamento.Risorsa.ToString
        End If

        If Not String.IsNullOrEmpty(Me.Accertamento.NumeroAttoAssunzioneOriginale) Then
            Me.NumeroAttoAssunzioneTextBox.Text = Me.Accertamento.NumeroAttoAssunzioneOriginale
        End If

        If Me.Accertamento.DisponibilitaResiduale.HasValue Then
            Me.DisponibilitaResidualeTextBox.Value = Me.Accertamento.DisponibilitaResiduale
        End If

        If Me.Accertamento.StanziamentoIniziale.HasValue Then
            Me.StanziamentoInizialeTextBox.Value = Me.Accertamento.StanziamentoIniziale
        End If

        If Me.Accertamento.StanziamentoAssestato.HasValue Then
            Me.StanziamentoAssestatoTextBox.Value = Me.Accertamento.StanziamentoAssestato
        End If


        ParsecUtility.SessionManager.Accertamento = Nothing
    End Sub

#End Region

#Region "EVENTI CONTROLLI"


    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        If Me.VerificaDati() Then

            Dim accertamento As ParsecAtt.Accertamento = Nothing
            If Me.Accertamento Is Nothing Then
                accertamento = New ParsecAtt.Accertamento
                accertamento.Guid = Guid.NewGuid.ToString
            Else
                If Not Me.Request.QueryString("copia") Is Nothing Then
                    accertamento = New ParsecAtt.Accertamento
                    accertamento.Guid = Guid.NewGuid.ToString
                Else
                    accertamento = Me.Accertamento
                End If

            End If

            accertamento.Importo = Me.ImportoTextBox.Value
            accertamento.Note = Me.DescrizioneTextBox.Text.Trim
            accertamento.Capitolo = CLng(Me.CapitoloTextBox.Text)

            accertamento.AnnoEsercizio = CInt(Me.AnniComboBox.SelectedItem.Text)


            accertamento.Articolo = Me.ArticoloTextBox.Value
            accertamento.NumeroAccertamento = Me.AccertamentoTextBox.Value
            accertamento.NumeroSubAccertamento = Me.SubAccertamentoTextBox.Text


            If Me.TitoliComboBox.SelectedIndex <> 0 Then
                accertamento.Titolo = Me.TitoliComboBox.SelectedItem.Text
            Else
                accertamento.Titolo = Nothing
            End If

            If Me.CategorieComboBox.SelectedIndex <> 0 Then
                accertamento.Categoria = Me.CategorieComboBox.SelectedItem.Text
            Else
                accertamento.Categoria = Nothing
            End If


            accertamento.Risorsa = Me.RisorsaTextBox.Value



            accertamento.NumeroAttoAssunzioneOriginale = Me.NumeroAttoAssunzioneTextBox.Text
            accertamento.DisponibilitaResiduale = Me.DisponibilitaResidualeTextBox.Value
            accertamento.StanziamentoIniziale = Me.StanziamentoInizialeTextBox.Value
            accertamento.StanziamentoAssestato = Me.StanziamentoAssestatoTextBox.Value



            ParsecUtility.SessionManager.Accertamento = accertamento
            ParsecUtility.Utility.ClosePopup(True)
        End If

    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaVista()
    End Sub

    Protected Sub TrovaCapitoloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaCapitoloImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/RicercaCapitoloPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaCapitoloImageButton.ClientID)
        queryString.Add("tipo", 1) ' 1=ENTRATA 2= SPESA
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 450, queryString, False)
    End Sub

    Protected Sub AggiornaCapitoloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaCapitoloImageButton.Click
        If Not ParsecUtility.SessionManager.Capitolo Is Nothing Then
            Dim capitolo As ParsecAtt.Capitolo = ParsecUtility.SessionManager.Capitolo
            ParsecUtility.SessionManager.Capitolo = Nothing
            Me.CapitoloTextBox.Text = capitolo.NumeroCapitolo
            'Me.TitoliComboBox.FindItemByValue(capitolo.Titolo).Selected = True
            Me.ArticoloTextBox.Text = capitolo.Articolo.ToString
            Me.DescrizioneTextBox.Text = capitolo.Descrizione
        End If
    End Sub

#End Region






   
End Class
