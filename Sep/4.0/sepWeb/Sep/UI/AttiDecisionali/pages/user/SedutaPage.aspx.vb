Imports Telerik.Web.UI

Partial Class SedutaPage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    Public Property Seduta As ParsecAtt.Seduta
        Get
            Return CType(Session("SedutaPage_Seduta"), ParsecAtt.Seduta)
        End Get
        Set(ByVal value As ParsecAtt.Seduta)
            Session("SedutaPage_Seduta") = value
        End Set
    End Property

    Public Property Presenze() As List(Of ParsecAtt.PresenzaSeduta)
        Get
            Return CType(Session("SedutaPage_Presenze"), List(Of ParsecAtt.PresenzaSeduta))
        End Get
        Set(ByVal value As List(Of ParsecAtt.PresenzaSeduta))
            Session("SedutaPage_Presenze") = value
        End Set
    End Property

    Public Property DataSelezionata As Date
        Get
            Return CDate(Session("SedutaPage_DataSelezionata")).Date
        End Get
        Set(ByVal value As Date)
            Session("SedutaPage_DataSelezionata") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"


    Private Function GetNuovaGestionePresenze() As Boolean
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("GestionePresenzeSeduta", ParsecAdmin.TipoModulo.ATT)
        parametri.Dispose()

        Dim visualizzaPulsante As Boolean = False
        If Not parametro Is Nothing Then
            If parametro.Valore = "1" Then
                Return True
            End If
        End If
        Return False
    End Function

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.CaricaTipologieConvocazioni()
        Me.CaricaTipologieSedute()
        If Not Me.Page.IsPostBack Then

            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

            Dim nuovaGestionePresenze As Boolean = Me.GetNuovaGestionePresenze()
         
            Me.AggiornaPresenzeImageButton.Visible = utenteCorrente.SuperUser And nuovaGestionePresenze

            Me.Seduta = Nothing
            Me.Presenze = New List(Of ParsecAtt.PresenzaSeduta)

            If Not Request.QueryString("IdSeduta") Is Nothing Then
                Dim idSeduta = CInt(Request.QueryString("IdSeduta"))
                If idSeduta > 0 Then
                    Dim sedute As New ParsecAtt.SedutaRepository
                    Me.Seduta = sedute.GetById(idSeduta)
                    Me.AggiornaVista(Me.Seduta)
                    sedute.Dispose()
                    Me.TitleLabel.Text = "Seduta di: " & Me.Seduta.DescrizioneTipologiaSeduta
                    Me.DataConvocazioneLabel.Text = "Data di Convocazione: " & String.Format("{0:dd/MM/yyyy}", Me.Seduta.DataConvocazione)
                    Me.DataSelezionata = Me.Seduta.DataPrimaConvocazione.Value
                Else
                    If Not String.IsNullOrEmpty(CStr(Request.QueryString("DataSelezionata"))) Then
                        Me.DataSelezionata = CDate(Request.QueryString("DataSelezionata"))
                    End If
                    Me.ResettaVista()
                End If
            End If
        End If

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.AggiornaGrigliaPresenze()
        Me.ElencoPresenzeLabel.Text = "Elenco Presenze&nbsp;&nbsp;&nbsp;" & If(Me.Presenze.Count > 0, "( " & Me.Presenze.Count.ToString & " )", "")
    End Sub

#End Region

#Region "MEDOTI PRIVATI"

    Private Sub ResettaVista()
        Me.TipologieConvocazioneComboBox.SelectedIndex = 0
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        Me.DataPrimaConvocazioneTextBox.SelectedDate = Me.DataSelezionata
        Me.OrarioPrimaConvocazioneTextBox.SelectedDate = Now
        Me.DataSecondaConvocazioneTextBox.SelectedDate = Me.DataSelezionata
        Me.OrarioSecondaConvocazioneTextBox.SelectedDate = Now
        Me.DataTrasmissioneCapigruppoTextBox.SelectedDate = Nothing
        Me.DataTrasmissioneRevisoriTextBox.SelectedDate = Nothing
        ' Me.DataPrimaConvocazioneTextBox.DateInput.ReadOnly = True
        'Me.DataPrimaConvocazioneTextBox.DatePopupButton.Visible = False

        Me.Presenze = New List(Of ParsecAtt.PresenzaSeduta)
    End Sub

    Private Sub AggiornaVista(ByVal seduta As ParsecAtt.Seduta)
        If seduta.IdTipologiaConvocazione.HasValue AndAlso seduta.IdTipologiaConvocazione <> 0 Then
            Me.TipologieConvocazioneComboBox.FindItemByValue(seduta.IdTipologiaConvocazione.Value.ToString).Selected = True
        End If
        Me.TipologieSedutaComboBox.Enabled = False
        If seduta.IdTipologiaSeduta.HasValue Then
            Me.TipologieSedutaComboBox.FindItemByValue(seduta.IdTipologiaSeduta.Value.ToString).Selected = True
        End If

        Me.DataPrimaConvocazioneTextBox.SelectedDate = seduta.DataPrimaConvocazione
        Me.OrarioPrimaConvocazioneTextBox.SelectedDate = seduta.DataPrimaConvocazione

        Me.DataSecondaConvocazioneTextBox.SelectedDate = seduta.DataSecondaConvocazione
        Me.OrarioSecondaConvocazioneTextBox.SelectedDate = seduta.DataSecondaConvocazione

        Me.DataTrasmissioneCapigruppoTextBox.SelectedDate = seduta.DataTrasmissioneCapigruppo
        Me.DataTrasmissioneRevisoriTextBox.SelectedDate = seduta.DataTrasmissioneRevisori

        'Me.DataPrimaConvocazioneTextBox.DateInput.ReadOnly = True
        'Me.DataPrimaConvocazioneTextBox.DatePopupButton.Visible = False

        Me.DataPrimaConvocazioneRadioButton.Checked = seduta.PrimaConvocazione
        Me.DataSecondaConvocazioneRadioButton.Checked = Not seduta.PrimaConvocazione

        Me.CaricaPresenzeSeduta(seduta)


    End Sub

    Private Sub CaricaPresenzeSeduta(ByVal seduta As ParsecAtt.Seduta)
        Dim presenze As New ParsecAtt.PresenzaSedutaRepository

        Dim nuovaGestionePresenze As Boolean = Me.GetNuovaGestionePresenze()
        If nuovaGestionePresenze Then
            Me.Presenze = presenze.GetView(New ParsecAtt.FiltroPresenzaSeduta With {.IdSeduta = seduta.Id, .IncludiAnnullate = True})
        Else
            Me.Presenze = presenze.GetView(New ParsecAtt.FiltroPresenzaSeduta With {.IdSeduta = seduta.Id})
        End If

        presenze.Dispose()
    End Sub

    '***************************************************************************************************************
    'RECUPERO LE PRESENZE DALL'ORGANIGRAMMA
    '***************************************************************************************************************
    Private Sub CaricaPresenzePredefinite(ByVal tipologiaSeduta As ParsecAtt.TipologiaOrganoDeliberante)
        Dim valore As String = ""
        Select Case tipologiaSeduta
            Case ParsecAtt.TipologiaOrganoDeliberante.GiuntaComunale
                valore = "codStrutturaRuoloAssessore"
            Case ParsecAtt.TipologiaOrganoDeliberante.ConsiglioComunale
                valore = "codStrutturaRuoloConsigliere"
        End Select

        If Not String.IsNullOrEmpty(valore) Then
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName(valore, ParsecAdmin.TipoModulo.SEP)
            parametri.Dispose()

            Dim presenze As New ParsecAtt.PresenzaSedutaRepository
            Dim strutturaPadre As ParsecAdmin.Struttura = (New ParsecAdmin.StructureRepository).GetQuery.Where(Function(c) c.Codice = CInt(parametro.Valore) And c.LogStato Is Nothing).FirstOrDefault
            Me.Presenze = presenze.GetDefaultView(New ParsecAtt.FiltroPresenzaSeduta With {.IdPadre = strutturaPadre.Id})
            presenze.Dispose()
        Else
            Me.Presenze = New List(Of ParsecAtt.PresenzaSeduta)
        End If
    End Sub

    Private Sub CaricaTipologieConvocazioni()
        Dim tipologie As New ParsecAtt.TipologiaConvocazioneRepository
        Me.TipologieConvocazioneComboBox.DataValueField = "Id"
        Me.TipologieConvocazioneComboBox.DataTextField = "Descrizione"
        Me.TipologieConvocazioneComboBox.DataSource = tipologie.GetKeyValue(Nothing)
        Me.TipologieConvocazioneComboBox.DataBind()
        Me.TipologieConvocazioneComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieConvocazioneComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub CaricaTipologieSedute()
        Dim tipologie As New ParsecAtt.TipologiaSedutaRepository
        Me.TipologieSedutaComboBox.DataValueField = "Id"
        Me.TipologieSedutaComboBox.DataTextField = "Descrizione"
        Me.TipologieSedutaComboBox.DataSource = tipologie.GetKeyValue(Nothing)
        Me.TipologieSedutaComboBox.DataBind()
        Me.TipologieSedutaComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub


    Private Sub Save()
        Dim sedute As New ParsecAtt.SedutaRepository
        Try
            Dim seduta As ParsecAtt.Seduta = sedute.CreateFromInstance(Me.Seduta)
            If Me.TipologieSedutaComboBox.SelectedIndex <> 0 Then
                seduta.IdTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
            End If

            seduta.DataPrimaConvocazione = Me.DataPrimaConvocazioneTextBox.SelectedDate.Value.Date.AddHours(Me.OrarioPrimaConvocazioneTextBox.SelectedDate.Value.Hour).AddMinutes(Me.OrarioPrimaConvocazioneTextBox.SelectedDate.Value.Minute)
            If Me.TipologieConvocazioneComboBox.SelectedIndex <> 0 Then
                seduta.IdTipologiaConvocazione = CInt(Me.TipologieConvocazioneComboBox.SelectedValue)
            End If
            If Me.OrarioPrimaConvocazioneTextBox.SelectedDate.HasValue Then
                seduta.OrarioPrimaConvocazione = Me.OrarioPrimaConvocazioneTextBox.SelectedDate
            End If
            If Me.DataSecondaConvocazioneTextBox.SelectedDate.HasValue Then
                If Me.OrarioSecondaConvocazioneTextBox.SelectedDate.HasValue Then
                    seduta.DataSecondaConvocazione = Me.DataSecondaConvocazioneTextBox.SelectedDate.Value.Date.AddHours(Me.OrarioSecondaConvocazioneTextBox.SelectedDate.Value.Hour).AddMinutes(Me.OrarioSecondaConvocazioneTextBox.SelectedDate.Value.Minute)
                Else
                    seduta.DataSecondaConvocazione = Me.DataSecondaConvocazioneTextBox.SelectedDate.Value.Date
                End If
            End If
            If Me.DataTrasmissioneCapigruppoTextBox.SelectedDate.HasValue Then
                seduta.DataTrasmissioneCapigruppo = Me.DataTrasmissioneCapigruppoTextBox.SelectedDate
            End If
            If Me.DataTrasmissioneRevisoriTextBox.SelectedDate.HasValue Then
                seduta.DataTrasmissioneRevisori = Me.DataTrasmissioneRevisoriTextBox.SelectedDate
            End If

            If Me.DataPrimaConvocazioneRadioButton.Checked Then
                seduta.PrimaConvocazione = True
            Else
                seduta.PrimaConvocazione = False
            End If



            seduta.Presenze = Me.Presenze

            sedute.Save(seduta)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            sedute.Dispose()
        End Try
    End Sub


    Private Sub AggiornaGrigliaPresenze()
        Me.PresenzeGridView.DataSource = Me.Presenze
        Me.PresenzeGridView.DataBind()
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub AggiornaPresenzeImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaPresenzeImageButton.Click

        Dim nuovePresenze As List(Of ParsecAtt.PresenzaSeduta) = Nothing
        Dim presenzeAggiornate As New List(Of ParsecAtt.PresenzaSeduta)
        Dim tipologiaSeduta = CType(CInt(Me.TipologieSedutaComboBox.SelectedValue), ParsecAtt.TipologiaOrganoDeliberante)

        Dim valore As String = ""
        Select Case tipologiaSeduta
            Case ParsecAtt.TipologiaOrganoDeliberante.GiuntaComunale
                valore = "codStrutturaRuoloAssessore"
            Case ParsecAtt.TipologiaOrganoDeliberante.ConsiglioComunale
                valore = "codStrutturaRuoloConsigliere"
        End Select

        If Not String.IsNullOrEmpty(valore) Then
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName(valore, ParsecAdmin.TipoModulo.SEP)
            parametri.Dispose()

            Dim presenze As New ParsecAtt.PresenzaSedutaRepository
            Dim strutturaPadre As ParsecAdmin.Struttura = (New ParsecAdmin.StructureRepository).GetQuery.Where(Function(c) c.Codice = CInt(parametro.Valore) And c.LogStato Is Nothing).FirstOrDefault
            nuovePresenze = presenze.GetDefaultView(New ParsecAtt.FiltroPresenzaSeduta With {.IdPadre = strutturaPadre.Id})
            presenze.Dispose()
        Else
            nuovePresenze = New List(Of ParsecAtt.PresenzaSeduta)
        End If


        For Each presenza In Me.Presenze
            Dim codiceStruttura As Integer = presenza.CodiceStruttura
            Dim p = nuovePresenze.Where(Function(c) c.CodiceStruttura = codiceStruttura)
            If p.Any Then
                presenzeAggiornate.Add(presenza)
            End If
        Next

        For Each presenza In nuovePresenze
            Dim codiceStruttura As Integer = presenza.CodiceStruttura
            Dim p = presenzeAggiornate.Where(Function(c) c.CodiceStruttura = codiceStruttura)
            If Not p.Any Then
                presenzeAggiornate.Add(presenza)
            End If
        Next
        Me.Presenze = presenzeAggiornate
    End Sub

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Try
            Me.Save()
            ParsecUtility.Utility.CloseRadWindowAndUpadateParent(False)
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        If Me.Seduta Is Nothing Then
            Me.ResettaVista()
        Else
            Me.AggiornaVista(Me.Seduta)
        End If
    End Sub

    Protected Sub TipologieSedutaComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieSedutaComboBox.SelectedIndexChanged
        If Me.TipologieSedutaComboBox.SelectedIndex <> 0 Then
            Me.CaricaPresenzePredefinite(CType(CInt(Me.TipologieSedutaComboBox.SelectedValue), ParsecAtt.TipologiaOrganoDeliberante))
        Else
            Me.Presenze = New List(Of ParsecAtt.PresenzaSeduta)
        End If
        If CType(CInt(Me.TipologieSedutaComboBox.SelectedValue), ParsecAtt.TipologiaOrganoDeliberante) <> ParsecAtt.TipologiaOrganoDeliberante.ConsiglioComunale Then
            Me.TipologieConvocazioneComboBox.SelectedIndex = 0
        End If
        Me.TipologieConvocazioneComboBox.Enabled = CType(CInt(Me.TipologieSedutaComboBox.SelectedValue), ParsecAtt.TipologiaOrganoDeliberante) = ParsecAtt.TipologiaOrganoDeliberante.ConsiglioComunale


    End Sub

#End Region

#Region "GESTIONE PRESENZE"

    Protected Sub PresenzeGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles PresenzeGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim presenza As ParsecAtt.PresenzaSeduta = CType(e.Item.DataItem, ParsecAtt.PresenzaSeduta)
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("PresenteCheckBox"), CheckBox)
            chk.Checked = presenza.Presente
        End If
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        Dim item = CType(CType(sender, CheckBox).NamingContainer, GridItem)
        Dim dataItem As GridDataItem = CType(item, GridDataItem)
        Dim codice As Integer = CInt(dataItem("CodiceStruttura").Text)
        Me.Presenze.Where(Function(c) c.CodiceStruttura = codice).FirstOrDefault.Presente = CType(sender, CheckBox).Checked
    End Sub


#End Region



  
  
End Class
