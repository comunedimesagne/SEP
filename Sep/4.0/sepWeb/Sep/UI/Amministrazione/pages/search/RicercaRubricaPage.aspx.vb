Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI


Partial Class RicercaRubricaPage
    Inherits System.Web.UI.Page


    Public Property ParametroFiltro As String = String.Empty
    Public Property FiltroTipologiaReferente As Nullable(Of Int32)
    Public Property FiltroTipologiaSoggetto As Nullable(Of Int32)

#Region "PROPRIETA'"


    Public Property StrutturaEsterna() As ParsecAdmin.StrutturaEsternaInfo
        Get
            Return CType(Session("RicercaRubricaPage_StrutturaEsterna"), ParsecAdmin.StrutturaEsternaInfo)
        End Get
        Set(ByVal value As ParsecAdmin.StrutturaEsternaInfo)
            Session("RicercaRubricaPage_StrutturaEsterna") = value
        End Set
    End Property

    Public Property Rubrica() As List(Of ParsecAdmin.StrutturaEsternaInfo)
        Get
            Return CType(Session("RicercaRubricaPage_Rubrica"), List(Of ParsecAdmin.StrutturaEsternaInfo))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.StrutturaEsternaInfo))
            Session("RicercaRubricaPage_Rubrica") = value
        End Set
    End Property

    Public Property Filtro() As ParsecAdmin.FiltroStrutturaEsternaInfo
        Get
            Return CType(Session("RicercaRubricaPage_Filtro"), FiltroStrutturaEsternaInfo)
        End Get
        Set(ByVal value As FiltroStrutturaEsternaInfo)
            Session("RicercaRubricaPage_Filtro") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        Me.TipoReferenteListBox.DataValueField = "Id"
        Me.TipoReferenteListBox.DataTextField = "Descrizione"
        Me.TipoReferenteListBox.DataSource = (New TipologieReferenteRepository).GetView(Nothing)
        Me.TipoReferenteListBox.DataBind()

        Me.SessoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("-", "0"))
        Me.SessoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("M", "1"))
        Me.SessoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("F", "2"))
        Me.SessoComboBox.SelectedIndex = 0


        Me.AlboProfessionaleComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Seleziona albo", "0"))
        Me.AlboProfessionaleComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Agronomo", "1"))
        Me.AlboProfessionaleComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Architetto", "2"))
        Me.AlboProfessionaleComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Geometra", "3"))
        Me.AlboProfessionaleComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Ingegnere", "4"))
        Me.AlboProfessionaleComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Perito", "5"))
        Me.AlboProfessionaleComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Altro", "6"))
        Me.AlboProfessionaleComboBox.SelectedIndex = 0


        Me.TipoPersonaComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.TipoPersonaComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Fisica", "0"))
        Me.TipoPersonaComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Giuridica", "1"))
        Me.TipoPersonaComboBox.SelectedIndex = 0

        Me.CaricaTipologieDocumentoIdentita()

        Me.DisabilitaPulsantePredefinito.Attributes.Add("onclick", "return false;")
        ParsecUtility.Utility.RegisterDefaultButton(Me.ComuneResidenzaTextBox, Me.TrovaComuneResidenzaImageButton)
        ParsecUtility.Utility.RegisterDefaultButton(Me.ComuneUfficioTextBox, Me.TrovaComuneUfficioImageButton)
        ParsecUtility.Utility.RegisterDefaultButton(Me.ComuneNascitaTextBox, Me.TrovaComuneNascitaImageButton)


        If Not Me.Page.IsPostBack Then
            Me.ResettaVista()
            Me.DatiRubricaMultiPage.PageViews(0).Selected = True
            Me.Rubrica = Nothing
            Me.Filtro = Nothing

            'Imposto l'ordinamento predefinito.
            Dim sortExpr As New Telerik.Web.UI.GridSortExpression()
            sortExpr.FieldName = "Denominazione"
            sortExpr.SortOrder = Telerik.Web.UI.GridSortOrder.Ascending
            Me.RubricaGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Me.GetParametri()
            Me.DenominazioneTextBox.Text = Me.ParametroFiltro
            Me.Filtro = Me.GetFiltro
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il referente selezionato?", False, Not Me.StrutturaEsterna Is Nothing)
        If Not Me.Rubrica Is Nothing Then
            Me.TitoloElencoReferentiLabel.Text = "Elenco Referenti&nbsp;&nbsp;&nbsp;" & If(Me.Rubrica.Count > 0, "( " & Me.Rubrica.Count.ToString & " )", "")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub RubricaGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RubricaGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub RubricaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RubricaGridView.ItemCommand

        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e)

            Case "Copy"
                Me.AggiornaVista(e)
                Me.StrutturaEsterna = Nothing
            Case "ConfirmSelectAndClose"
                Me.SelezionaStrutturaEsterna(e)
        End Select

    End Sub

    Protected Sub RubricaGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RubricaGridView.NeedDataSource
        If Me.Rubrica Is Nothing Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            If Me.Filtro Is Nothing Then
                Me.Rubrica = rubrica.GetView(Nothing)
            Else
                Me.Rubrica = rubrica.GetView(Me.Filtro)
            End If
            rubrica.Dispose()
        End If
        Me.RubricaGridView.DataSource = Me.Rubrica
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "SalvaChiudi"
                Dim success As Boolean = True
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    ParsecUtility.SessionManager.Rubrica = Me.StrutturaEsterna

                Catch ex As Exception
                    success = False
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)
                If success Then
                    ParsecUtility.Utility.ClosePopup(False)
                End If

            Case "Elimina"

                If Me.hflVerificaElimina.Value = "1" Then
                    If Not Me.StrutturaEsterna Is Nothing Then
                        Me.Delete()
                        Me.ResettaVista()
                        Me.AggiornaGriglia()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una tipologia di referente!", False)
                    End If
                End If
                Me.hflVerificaElimina.Value = "0"

            Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    Me.AggiornaGriglia()
                Catch ex As Exception
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)
            Case "Nuovo"
                Me.Filtro = Nothing
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Annulla"
                Me.Filtro = Nothing
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Trova"
                Me.Filtro = Me.GetFiltro
                Me.AggiornaGriglia()

        End Select
    End Sub

#End Region

#Region "EVENTI CONTROLLI PAGINA"


    Protected Sub TrovaComuneResidenzaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaComuneResidenzaImageButton.Click
        Dim search As Boolean = True
        If Not String.IsNullOrEmpty(Me.ComuneResidenzaTextBox.Text) Then
            Dim comuni As New ParsecAdmin.ComuniUrbaniRepository
            Dim res = comuni.GetQuery.Where(Function(c) c.Descrizione.ToLower = Me.ComuneResidenzaTextBox.Text.ToLower).ToList
            If res.Count = 1 Then
                Dim comune As ParsecAdmin.ComuniUrbani = res.FirstOrDefault
                Me.ComuneResidenzaTextBox.Text = comune.Descrizione
                Me.CapResidenzaTextBox.Text = comune.CAP
                Me.ProvinciaResidenzaTextBox.Text = comune.Provincia
                comune = Nothing
                search = False
            End If
            comuni.Dispose()
        End If
        If search Then
            Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaComuniPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaComuneResidenzaImageButton.ClientID)
            queryString.Add("mode", CInt(Rnd()))
            queryString.Add("filtro", Me.ComuneResidenzaTextBox.Text)
            ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        End If
    End Sub

    Protected Sub AggiornaComuneResidenzaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaComuneResidenzaImageButton.Click
        If Not ParsecUtility.SessionManager.ComuneUrbano Is Nothing Then
            Dim comune As ParsecAdmin.ComuniUrbani = ParsecUtility.SessionManager.ComuneUrbano
            Me.ComuneResidenzaTextBox.Text = comune.Descrizione
            Me.CapResidenzaTextBox.Text = comune.CAP
            Me.ProvinciaResidenzaTextBox.Text = comune.Provincia
            ParsecUtility.SessionManager.ComuneUrbano = Nothing
            comune = Nothing
        End If
    End Sub

    Protected Sub EliminaComuneResidenzaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaComuneResidenzaImageButton.Click
        Me.ComuneResidenzaTextBox.Text = String.Empty
        Me.CapResidenzaTextBox.Text = String.Empty
        Me.ProvinciaResidenzaTextBox.Text = String.Empty
    End Sub

    Protected Sub EliminaComuneNascitaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaComuneNascitaImageButton.Click
        Me.ComuneNascitaTextBox.Text = String.Empty
        Me.CapNascitaTextBox.Text = String.Empty
        Me.ProvinciaNascitaTextBox.Text = String.Empty
        Me.CodiceIstatTextBox.Text = String.Empty
    End Sub



    Protected Sub TrovaComuneNascitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaComuneNascitaImageButton.Click
        Dim search As Boolean = True
        If Not String.IsNullOrEmpty(Me.ComuneNascitaTextBox.Text) Then
            Dim comuni As New ParsecAdmin.ComuneRepository
            Dim res = comuni.GetQuery.Where(Function(c) c.Descrizione.ToLower = Me.ComuneNascitaTextBox.Text.ToLower).ToList
            If res.Count = 1 Then
                Dim comune As ParsecAdmin.Comune = res.FirstOrDefault
                Me.ComuneNascitaTextBox.Text = comune.Descrizione
                Me.CapNascitaTextBox.Text = comune.CAP
                Me.ProvinciaNascitaTextBox.Text = comune.Provincia
                Me.CodiceIstatTextBox.Text = comune.CodiceIstat
                comune = Nothing
                search = False
            End If
            comuni.Dispose()
        End If
        If search Then
            Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaComunePage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaComuneNascitaImageButton.ClientID)
            queryString.Add("mode", CInt(Rnd()))
            queryString.Add("filtro", Me.ComuneNascitaTextBox.Text)
            ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        End If
    End Sub

    Protected Sub AggiornaComuneNascitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaComuneNascitaImageButton.Click
        If Not ParsecUtility.SessionManager.ComuneUrbano Is Nothing Then
            Dim comune As ParsecAdmin.Comune = ParsecUtility.SessionManager.ComuneUrbano
            Me.ComuneNascitaTextBox.Text = comune.Descrizione
            Me.CapNascitaTextBox.Text = comune.CAP
            Me.ProvinciaNascitaTextBox.Text = comune.Provincia
            Me.CodiceIstatTextBox.Text = comune.CodiceIstat
            ParsecUtility.SessionManager.ComuneUrbano = Nothing
            comune = Nothing
        End If
    End Sub
   

    Protected Sub TrovaComuneUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaComuneUfficioImageButton.Click
        Dim search As Boolean = True
        If Not String.IsNullOrEmpty(Me.ComuneUfficioTextBox.Text) Then
            Dim comuni As New ParsecAdmin.ComuniUrbaniRepository
            Dim res = comuni.GetQuery.Where(Function(c) c.Descrizione.ToLower = Me.ComuneUfficioTextBox.Text.ToLower).ToList
            If res.Count = 1 Then
                Dim comune As ParsecAdmin.ComuniUrbani = res.FirstOrDefault
                Me.ComuneUfficioTextBox.Text = comune.Descrizione
                Me.CapUfficioTextBox.Text = comune.CAP
                Me.ProvinciaUfficioTextBox.Text = comune.Provincia
                comune = Nothing
                search = False
            End If
            comuni.Dispose()
        End If
        If search Then
            Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaComuniPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaComuneUfficioImageButton.ClientID)
            queryString.Add("mode", CInt(Rnd()))
            queryString.Add("filtro", Me.ComuneUfficioTextBox.Text)
            ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        End If
    End Sub

    Protected Sub AggiornaComuneUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaComuneUfficioImageButton.Click
        If Not ParsecUtility.SessionManager.ComuneUrbano Is Nothing Then
            Dim comune As ParsecAdmin.ComuniUrbani = ParsecUtility.SessionManager.ComuneUrbano
            Me.ComuneUfficioTextBox.Text = comune.Descrizione
            Me.CapUfficioTextBox.Text = comune.CAP
            Me.ProvinciaUfficioTextBox.Text = comune.Provincia
            ParsecUtility.SessionManager.ComuneUrbano = Nothing
            comune = Nothing
        End If
    End Sub

    Protected Sub EliminaComuneUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaComuneUfficioImageButton.Click
        Me.ComuneUfficioTextBox.Text = String.Empty
        Me.CapUfficioTextBox.Text = String.Empty
        Me.ProvinciaUfficioTextBox.Text = String.Empty
    End Sub

    Protected Sub CalcolaCodiceFiscaleButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalcolaCodiceFiscaleButton.Click
        If Me.TipoPersonaComboBox.SelectedIndex > 0 Then
            If Me.TipoPersonaComboBox.SelectedIndex = 1 Then
                If String.IsNullOrEmpty(Me.NomeTextBox.Text) Then
                    ParsecUtility.Utility.MessageBox("E' necessario specificare il nome del referente!", False)
                    Return
                End If
                If String.IsNullOrEmpty(Me.DenominazioneTextBox.Text) Then
                    ParsecUtility.Utility.MessageBox("E' necessario specificare il cognome del referente!", False)
                    Return
                End If
                If Me.DataNascitaTextBox.SelectedDate Is Nothing Then
                    ParsecUtility.Utility.MessageBox("E' necessario specificare la data di nascita del referente!", False)
                    Return
                End If
                If Me.SessoComboBox.SelectedIndex = 0 Then
                    ParsecUtility.Utility.MessageBox("E' necessario specificare il sesso del referente!", False)
                    Return
                End If

                If String.IsNullOrEmpty(Me.ComuneNascitaTextBox.Text) Then
                    ParsecUtility.Utility.MessageBox("E' necessario specificare il comune di nascita del referente!", False)
                    Return
                End If

                Me.CodiceFiscaleTextBox.Text = ParsecUtility.Utility.CalcolaCodiceFiscale(Me.DenominazioneTextBox.Text, Me.NomeTextBox.Text, String.Format("{0:dd/MM/yyyy}", Me.DataNascitaTextBox.SelectedDate), Me.CodiceIstatTextBox.Text, Me.SessoComboBox.SelectedItem.Text)
            Else
                ParsecUtility.Utility.MessageBox("Non è possibile calcolare il codice fiscale se il referente è una persona giuridica!", False)
            End If
        Else
            ParsecUtility.Utility.MessageBox("E' possibile calcolare il codice fiscale solo se il referente è una persona fisica!", False)
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

   
    Private Sub CaricaTipologieDocumentoIdentita()
        Dim tipiDocumentoIdentita As New ParsecAdmin.TipiDocumentoRepository
        Me.TipoDocumentoIdentitaComboBox.DataValueField = "Id"
        Me.TipoDocumentoIdentitaComboBox.DataTextField = "Descrizione"
        Me.TipoDocumentoIdentitaComboBox.DataSource = tipiDocumentoIdentita.GetQuery.ToList
        Me.TipoDocumentoIdentitaComboBox.DataBind()
        Me.TipoDocumentoIdentitaComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.TipoDocumentoIdentitaComboBox.SelectedIndex = 0
        tipiDocumentoIdentita.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FiltroStrutturaEsternaInfo
        Dim filtro As New ParsecAdmin.FiltroStrutturaEsternaInfo
        If Not String.IsNullOrEmpty(Me.DenominazioneTextBox.Text) Then
            filtro.Denominazione = Trim(Me.DenominazioneTextBox.Text)
        End If
        If Not String.IsNullOrEmpty(Me.NomeTextBox.Text) Then
            filtro.Nome = Trim(Me.NomeTextBox.Text)
        End If
        If Not String.IsNullOrEmpty(Me.IndirizzoResidenzaTextBox.Text) Then
            filtro.Indirizzo = Trim(Me.IndirizzoResidenzaTextBox.Text)
        End If
        If Not String.IsNullOrEmpty(Me.ComuneResidenzaTextBox.Text) Then
            filtro.Comune = Trim(Me.ComuneResidenzaTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.TelefonoTextBox.Text) Then
            filtro.Telefono = Trim(Me.TelefonoTextBox.Text)
        End If


       
        filtro.TipologieReferente.Clear()

        If Me.FiltroTipologiaSoggetto.HasValue Then
            filtro.TipologiaSoggetto = Me.FiltroTipologiaSoggetto
        End If

        If Me.TipoPersonaComboBox.SelectedIndex > 0 Then
            filtro.TipologiaSoggetto = CInt(Me.TipoPersonaComboBox.SelectedValue)
        End If


        If Me.FiltroTipologiaReferente.HasValue Then
            filtro.TipologieReferente.Add(Me.FiltroTipologiaReferente)
            Me.FiltroTipologiaReferente = Nothing
        End If

        For i As Integer = 0 To Me.TipoReferenteListBox.CheckedItems.Count - 1
            If Not filtro.TipologieReferente.Contains(TipoReferenteListBox.CheckedItems(i).Value) Then
                filtro.TipologieReferente.Add(TipoReferenteListBox.CheckedItems(i).Value)
            End If
        Next

        Return filtro
    End Function

    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("Filtro") Then
                Me.ParametroFiltro = parametriPagina("Filtro")
            End If
            If parametriPagina.ContainsKey("FiltroTipologiaReferente") Then
                Me.FiltroTipologiaReferente = parametriPagina("FiltroTipologiaReferente")
            End If
            If parametriPagina.ContainsKey("FiltroTipologiaSoggetto") Then
                Me.FiltroTipologiaSoggetto = parametriPagina("FiltroTipologiaSoggetto")
                Me.TipoPersonaComboBox.FindItemByValue(Me.FiltroTipologiaSoggetto).Selected = True
            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    Private Sub AggiornaGriglia()
        Me.Rubrica = Nothing
        Me.RubricaGridView.Rebind()
    End Sub

    Private Sub Delete()
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim rubrica As New ParsecAdmin.RubricaRepository
        rubrica.Delete(Me.StrutturaEsterna, utenteCorrente)
        rubrica.Dispose()
    End Sub

    Private Sub Save()
        Dim rubrica As New ParsecAdmin.RubricaRepository
        Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = Nothing

        '******************************************************************************************
        'Inserisco sempre un nuovo referente.
        '******************************************************************************************
        strutturaEsterna = New ParsecAdmin.StrutturaEsternaInfo
        If Me.StrutturaEsterna Is Nothing Then
            strutturaEsterna.Codice = rubrica.GetNuovoCodice
        Else
            strutturaEsterna.Codice = rubrica.GetById(Me.StrutturaEsterna.Id).Codice
        End If

        strutturaEsterna.Denominazione = Trim(Me.DenominazioneTextBox.Text)
        strutturaEsterna.Azienda = Trim(Me.AziendaTextBox.Text)
        strutturaEsterna.Nome = Trim(Me.NomeTextBox.Text)
        strutturaEsterna.Email = Trim(Me.EmailTextBox.Text)
        strutturaEsterna.PartitaIVA = Trim(Me.PartitaIvaTextBox.Text)
        strutturaEsterna.Cellulare = Trim(Me.CellulareTextBox.Text)
        strutturaEsterna.Titolo = Trim(Me.TitoloTextBox.Text)

        strutturaEsterna.Indirizzo = Trim(Me.IndirizzoResidenzaTextBox.Text)
        strutturaEsterna.Comune = Trim(Me.ComuneResidenzaTextBox.Text)
        strutturaEsterna.CAP = Trim(Me.CapResidenzaTextBox.Text)
        strutturaEsterna.Provincia = Trim(Me.ProvinciaResidenzaTextBox.Text)

        strutturaEsterna.ComuneUfficio = Trim(Me.ComuneUfficioTextBox.Text)
        strutturaEsterna.ProvinciaUfficio = Trim(Me.ProvinciaUfficioTextBox.Text)
        strutturaEsterna.CAPUfficio = Trim(Me.CapUfficioTextBox.Text)
        strutturaEsterna.IndirizzoUfficio = Trim(Me.IndirizzoUfficioTextBox.Text)

        strutturaEsterna.ComuneNascita = Trim(Me.ComuneNascitaTextBox.Text)
        strutturaEsterna.ProvinciaNascita = Trim(Me.ProvinciaNascitaTextBox.Text)
        strutturaEsterna.CAPNascita = Trim(Me.CapNascitaTextBox.Text)

        If Me.TipoPersonaComboBox.SelectedIndex > 0 Then
            strutturaEsterna.Tipologia = CInt(Me.TipoPersonaComboBox.SelectedValue)
        End If



        strutturaEsterna.DataNascita = Me.DataNascitaTextBox.SelectedDate
        strutturaEsterna.NumeroIscrizioneAlbo = Trim(Me.NumeroIscrizioneAlboTextBox.Text)

        If Me.SessoComboBox.SelectedIndex <> 0 Then
            strutturaEsterna.Sesso = Me.SessoComboBox.SelectedItem.Text
        End If

        If Me.AlboProfessionaleComboBox.SelectedIndex <> 0 Then
            strutturaEsterna.AlboProfessionale = CInt(Me.AlboProfessionaleComboBox.SelectedValue)
        End If
        strutturaEsterna.ProvinciaAlbo = Trim(Me.ProvinciaAlboTextBox.Text)


        strutturaEsterna.SitoWeb = Trim(Me.SitoWebTextBox.Text)


        strutturaEsterna.CodiceFiscale = Trim(Me.CodiceFiscaleTextBox.Text).ToUpper

        strutturaEsterna.InRubrica = True

        If Me.TipoDocumentoIdentitaComboBox.SelectedIndex <> 0 Then
            strutturaEsterna.TipoDocumentoIdentita = CInt(Me.TipoDocumentoIdentitaComboBox.SelectedValue)
        End If

        strutturaEsterna.NumeroDocumentoIdentita = Me.NumeroDocumentoIdentitaTextBox.Text.Trim
        strutturaEsterna.DocumentoIdentitaEnteRilascio = Me.DocumentoIdentitaEnteRilascioTextBox.Text.Trim


        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        strutturaEsterna.LogIdUtente = utenteCorrente.Id
        strutturaEsterna.LogUtente = utenteCorrente.Username
        strutturaEsterna.LogDataRegistrazione = Now


        strutturaEsterna.Telefono = Trim(Me.TelefonoTextBox.Text)
        'strutturaEsterna.DataDecesso = Me.DataDecessoTextBox.SelectedDate
        strutturaEsterna.Fax = Trim(Me.FaxTextBox.Text)
        strutturaEsterna.IBAN = Trim(Me.IbanTextBox.Text).ToUpper

        Try


            Dim tipologieReferenteAssociati As New List(Of StrutturaEsternaInfoTipologiaReferente)
            For i As Integer = 0 To Me.TipoReferenteListBox.CheckedItems.Count - 1
                Dim idTipoReferente As Integer = CInt(Me.TipoReferenteListBox.CheckedItems(i).Value)
                Dim tipologia As New StrutturaEsternaInfoTipologiaReferente
                tipologia.IdTipologiaReferente = idTipoReferente
                tipologieReferenteAssociati.Add(tipologia)
            Next

            'Gestione storico
            rubrica.StrutturaEsterna = Me.StrutturaEsterna

            rubrica.Save(strutturaEsterna, tipologieReferenteAssociati)
            'Aggiorno l'oggetto corrente
            Me.StrutturaEsterna = rubrica.StrutturaEsterna

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            rubrica.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.StrutturaEsterna = Nothing

        Me.DenominazioneTextBox.Text = String.Empty
        Me.AziendaTextBox.Text = String.Empty
        Me.NomeTextBox.Text = String.Empty
        Me.EmailTextBox.Text = String.Empty
        Me.IndirizzoResidenzaTextBox.Text = String.Empty
        Me.ComuneUfficioTextBox.Text = String.Empty
        Me.ComuneResidenzaTextBox.Text = String.Empty
        Me.ProvinciaUfficioTextBox.Text = String.Empty
        Me.CapResidenzaTextBox.Text = String.Empty
        Me.CapUfficioTextBox.Text = String.Empty
        Me.ProvinciaResidenzaTextBox.Text = String.Empty
        Me.IndirizzoUfficioTextBox.Text = String.Empty
        Me.ComuneNascitaTextBox.Text = String.Empty
        Me.PartitaIvaTextBox.Text = String.Empty
        Me.ProvinciaNascitaTextBox.Text = String.Empty
        Me.CellulareTextBox.Text = String.Empty
        Me.CapNascitaTextBox.Text = String.Empty
        Me.TitoloTextBox.Text = String.Empty
        Me.DataNascitaTextBox.SelectedDate = Nothing
        Me.NumeroIscrizioneAlboTextBox.Text = String.Empty
        Me.SessoComboBox.SelectedIndex = 0
        Me.SitoWebTextBox.Text = String.Empty
        Me.CodiceFiscaleTextBox.Text = String.Empty

        Me.IbanTextBox.Text = String.Empty

        Me.TipoReferenteListBox.ClearSelection()
        Me.TipoReferenteListBox.ClearChecked()


        Me.TipoPersonaComboBox.SelectedIndex = 0

        Me.TelefonoTextBox.Text = String.Empty
        'Me.DataDecessoTextBox.SelectedDate = Nothing
        Me.FaxTextBox.Text = String.Empty
        Me.ProvinciaAlboTextBox.Text = String.Empty
        Me.AlboProfessionaleComboBox.SelectedIndex = 0

        'Dim item = Me.TipoReferenteListBox.FindItemByValue(11)
        'If Not item Is Nothing Then
        '    item.Checked = True
        '    item.Selected = True
        'End If

    End Sub

    Private Sub SelezionaStrutturaEsterna(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Dim rubrica As New ParsecAdmin.RubricaRepository
        ParsecUtility.SessionManager.Rubrica = rubrica.GetById(id)
        rubrica.Dispose()
        ParsecUtility.Utility.ClosePopup(False)
    End Sub

    Private Sub AggiornaVista(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        Me.ResettaVista()
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Dim rubrica As New ParsecAdmin.RubricaRepository
        Me.StrutturaEsterna = rubrica.GetById(id)


        'Cosa caricare sul copia???
        Select Case e.CommandName
            Case "Select"
            Case "Copy"
        End Select

        Me.DenominazioneTextBox.Text = Me.StrutturaEsterna.Denominazione
        Me.AziendaTextBox.Text = Me.StrutturaEsterna.Azienda
        Me.NomeTextBox.Text = Me.StrutturaEsterna.Nome
        Me.EmailTextBox.Text = Me.StrutturaEsterna.Email
        Me.IndirizzoResidenzaTextBox.Text = Me.StrutturaEsterna.Indirizzo
        Me.ComuneUfficioTextBox.Text = Me.StrutturaEsterna.ComuneUfficio
        Me.ComuneResidenzaTextBox.Text = Me.StrutturaEsterna.Comune
        Me.ProvinciaUfficioTextBox.Text = Me.StrutturaEsterna.ProvinciaUfficio
        Me.CapResidenzaTextBox.Text = Me.StrutturaEsterna.CAP
        Me.CapUfficioTextBox.Text = Me.StrutturaEsterna.CAPUfficio
        Me.ProvinciaResidenzaTextBox.Text = Me.StrutturaEsterna.Provincia
        Me.IndirizzoUfficioTextBox.Text = Me.StrutturaEsterna.IndirizzoUfficio
        Me.ComuneNascitaTextBox.Text = Me.StrutturaEsterna.ComuneNascita
        Me.PartitaIvaTextBox.Text = Me.StrutturaEsterna.PartitaIVA
        Me.ProvinciaNascitaTextBox.Text = Me.StrutturaEsterna.ProvinciaNascita
        Me.CellulareTextBox.Text = Me.StrutturaEsterna.Cellulare
        Me.CapNascitaTextBox.Text = Me.StrutturaEsterna.CAPNascita
        Me.TitoloTextBox.Text = Me.StrutturaEsterna.Titolo
        Me.IbanTextBox.Text = Me.StrutturaEsterna.IBAN

        If Me.StrutturaEsterna.TipoDocumentoIdentita.HasValue Then
            Me.TipoDocumentoIdentitaComboBox.FindItemByValue(Me.StrutturaEsterna.TipoDocumentoIdentita).Selected = True
        End If

        Me.DocumentoIdentitaEnteRilascioTextBox.Text = Me.StrutturaEsterna.DocumentoIdentitaEnteRilascio
        Me.NumeroDocumentoIdentitaTextBox.Text = Me.StrutturaEsterna.NumeroDocumentoIdentita

        If Not Me.StrutturaEsterna.DataNascita Is Nothing Then
            Me.DataNascitaTextBox.SelectedDate = CDate(String.Format("{0:dd/MM/yyyy}", Me.StrutturaEsterna.DataNascita))
        End If


        Me.NumeroIscrizioneAlboTextBox.Text = Me.StrutturaEsterna.NumeroIscrizioneAlbo

        If Not String.IsNullOrEmpty(Me.StrutturaEsterna.Sesso) Then
            Me.SessoComboBox.FindItemByText(Me.StrutturaEsterna.Sesso).Selected = True
        Else
            Me.SessoComboBox.SelectedIndex = 0
        End If

        Me.SitoWebTextBox.Text = Me.StrutturaEsterna.SitoWeb
        Me.CodiceFiscaleTextBox.Text = Me.StrutturaEsterna.CodiceFiscale

        Me.TelefonoTextBox.Text = Me.StrutturaEsterna.Telefono

        If Not Me.StrutturaEsterna.DataNascita Is Nothing Then
            'Me.DataDecessoTextBox.SelectedDate = CDate(String.Format("{0:dd/MM/yyyy}", Me.StrutturaEsterna.DataDecesso))
        End If
        Me.FaxTextBox.Text = Me.StrutturaEsterna.Fax

        'Seleziono il tipo di persona fisica o giuridica
        If Me.StrutturaEsterna.Tipologia.HasValue Then
            Me.TipoPersonaComboBox.Items.FindItemByValue(Me.StrutturaEsterna.Tipologia).Selected = True
        End If

        'Seleziono il tipo di albo pretorio
        If Me.StrutturaEsterna.AlboProfessionale.HasValue Then
            Me.AlboProfessionaleComboBox.Items.FindItemByValue(Me.StrutturaEsterna.AlboProfessionale).Selected = True
        End If

        Me.ProvinciaAlboTextBox.Text = Me.StrutturaEsterna.ProvinciaAlbo


        'Seleziono tutte le tipologie di referente associate al referente corrente.
        Dim idTipologieReferenteAssociati As List(Of Integer) = (New StrutturaEsternaInfoTipologiaReferenteRepository).GetTipologieReferenteId(id)
        For i As Integer = 0 To Me.TipoReferenteListBox.Items.Count - 1
            Dim idTipoReferente As Integer = CInt(Me.TipoReferenteListBox.Items(i).Value)
            If idTipologieReferenteAssociati.Contains(idTipoReferente) Then
                Me.TipoReferenteListBox.Items(i).Checked = True
            End If
        Next



        rubrica.Dispose()
    End Sub

#End Region

End Class