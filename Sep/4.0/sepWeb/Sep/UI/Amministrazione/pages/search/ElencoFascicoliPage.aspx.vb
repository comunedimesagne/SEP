Imports Telerik.Web.UI
Imports ParsecAdmin


Partial Class ElencoFascicoliPage
    Inherits System.Web.UI.Page


#Region "ENUMERAZIONI"

    Public Enum TipoPannello
        Filtro = 0
        Risultati = 1
        Dettaglio = 2
    End Enum

    Public Enum ModalitaPagina
        Filtro = 0
        Ricerca = 1
    End Enum

#End Region

#Region "GESTIONI PANNELLI"

    Private Sub VisualizzaPannello(ByVal tipo As TipoPannello)
        Me.FiltroPanel.Visible = False
        Me.RisultatiPanel.Visible = False
        Me.DettaglioPanel.Visible = False
        Select Case tipo
            Case TipoPannello.Filtro
                Me.FiltroPanel.Visible = True
            Case TipoPannello.Risultati
                Me.RisultatiPanel.Visible = True
            Case TipoPannello.Dettaglio
                Me.DettaglioPanel.Visible = True
        End Select

    End Sub

#End Region

#Region "PROPRIETA'"

    Public Property PageMode As ModalitaPagina
        Get
            Return Session("ElencoFascicoliPage_PageMode")
        End Get
        Set(ByVal value As ModalitaPagina)
            Session("ElencoFascicoliPage_PageMode") = value
        End Set
    End Property

    Public Property CurrentPosition As Integer
        Get
            Return Session("ElencoFascicoliPage_CurrentPosition")
        End Get
        Set(ByVal value As Integer)
            Session("ElencoFascicoliPage_CurrentPosition") = value
        End Set
    End Property

    Public Property Fascicolo() As ParsecAdmin.Fascicolo
        Get
            Return CType(Session("ElencoFascicoliPage_Fascicolo"), ParsecAdmin.Fascicolo)
        End Get
        Set(ByVal value As ParsecAdmin.Fascicolo)
            Session("ElencoFascicoliPage_Fascicolo") = value
        End Set
    End Property

    Public Property Fascicoli() As List(Of ParsecAdmin.Fascicolo)
        Get
            Return CType(Session("ElencoFascicoliPage_Fascicoli"), List(Of ParsecAdmin.Fascicolo))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Fascicolo))
            Session("ElencoFascicoliPage_Fascicoli") = value
        End Set
    End Property

    Public Property Documenti() As List(Of ParsecAdmin.FascicoloDocumento)
        Get
            Return CType(Session("ElencoFascicoliPage_Documenti"), List(Of ParsecAdmin.FascicoloDocumento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.FascicoloDocumento))
            Session("ElencoFascicoliPage_Documenti") = value
        End Set
    End Property

    Public Property Titolari() As List(Of ParsecAdmin.TitolareFascicolo)
        Get
            Return CType(Session("ElencoFascicoliPage_Titolari"), List(Of ParsecAdmin.TitolareFascicolo))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.TitolareFascicolo))
            Session("ElencoFascicoliPage_Titolari") = value
        End Set
    End Property

    Public Property Visibilita() As List(Of ParsecAdmin.VisibilitaDocumento)
        Get
            Return Session("ElencoFascicoliPage_Visibilita")
        End Get
        Set(ByVal value As List(Of ParsecAdmin.VisibilitaDocumento))
            Session("ElencoFascicoliPage_Visibilita") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.IsPostBack Then
            Me.Fascicoli = Nothing
            Me.CaricaStati()
            Me.CaricaProcedimenti()
            Me.CaricaTipologiaFascicoli()
        End If

        Me.VisualizzaPannello(TipoPannello.Filtro)


        If Request.QueryString("modalita").ToLower = "filtro" Then
            Me.PageMode = ModalitaPagina.Filtro
            Me.AvantiImageButton.Visible = False
        ElseIf Request.QueryString("modalita").ToLower = "ricerca" Then
            Me.PageMode = ModalitaPagina.Ricerca
            Me.CercaButton.Visible = False
        Else
            '
        End If

        Me.SetButtonImage()

        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        ParsecUtility.Utility.CloseWindow(False)
        '***************************************************************************
        Me.NumeroRegistroInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.NumeroRegistroInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.NumeroRegistroFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.PannelloDettaglio.Style.Add("width", widthStyle)


        Me.DocumentiPanel.Style.Add("width", widthStyle)
        Me.DocumentiGridView.Style.Add("width", widthStyle)

        Me.TitolariPanel.Style.Add("width", widthStyle)
        Me.TitolariGridView.Style.Add("width", widthStyle)


        Me.VisibilitaPanel.Style.Add("width", widthStyle)
        Me.VisibilitaGridView.Style.Add("width", widthStyle)


    End Sub

#End Region

    Private Sub CaricaTipologiaFascicoli()

        Dim dati As New List(Of ParsecAdmin.KeyValue)
        dati.Add(New ParsecAdmin.KeyValue With {.Id = ParsecAdmin.TipologiaFascicolo.ProcedimentoAmministrativo, .Descrizione = "Procedimento"})
        dati.Add(New ParsecAdmin.KeyValue With {.Id = ParsecAdmin.TipologiaFascicolo.Affare, .Descrizione = "Affare"})
        Me.TipologiaFascicoloFiltroComboBox.DataSource = dati
        Me.TipologiaFascicoloFiltroComboBox.DataTextField = "Descrizione"
        Me.TipologiaFascicoloFiltroComboBox.DataValueField = "Id"
        Me.TipologiaFascicoloFiltroComboBox.DataBind()
        Me.TipologiaFascicoloFiltroComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.TipologiaFascicoloFiltroComboBox.SelectedIndex = 0

        Me.TipologiaFascicoloComboBox.DataSource = dati
        Me.TipologiaFascicoloComboBox.DataTextField = "Descrizione"
        Me.TipologiaFascicoloComboBox.DataValueField = "Id"
        Me.TipologiaFascicoloComboBox.DataBind()
        Me.TipologiaFascicoloComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.TipologiaFascicoloComboBox.SelectedIndex = 0



    End Sub

    Private Sub CaricaStati()
        Me.StatoFascicoloFiltroComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Tutti", "0"))
        Me.StatoFascicoloFiltroComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Solo Aperti", "1"))
        Me.StatoFascicoloFiltroComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Solo Chiusi", "2"))
        Me.StatoFascicoloFiltroComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaProcedimenti()
        Dim procedimentoRepository As New ParsecAdmin.ProcedimentoRepository
        Me.ProcedimentoFiltroComboBox.DataSource = procedimentoRepository.GetView(Nothing)
        Me.ProcedimentoFiltroComboBox.DataTextField = "Descrizione"
        Me.ProcedimentoFiltroComboBox.DataValueField = "id"
        Me.ProcedimentoFiltroComboBox.DataBind()
        Me.ProcedimentoFiltroComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.ProcedimentoFiltroComboBox.SelectedIndex = 0
        procedimentoRepository.Dispose()
    End Sub


#Region "GESTIONE TITOLARE"

    Protected Sub TrovaBeneficiarioFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaBeneficiarioFiltroImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaBeneficiarioFiltroImageButton.ClientID)
        queryString.Add("mode", "search")

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("Filtro", Me.TitololariFiltroComboBox.Text)
        parametriPagina.Add("FiltroTipologiaSoggetto", 1)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

        If Not String.IsNullOrEmpty(Me.TitololariFiltroComboBox.Text) Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) (c.Denominazione.Contains(Me.TitololariFiltroComboBox.Text)) And c.LogStato Is Nothing).ToList
            If struttureEsterne.Count = 1 Then
                Me.TitololariFiltroComboBox.Text = struttureEsterne(0).Denominazione
                Me.TitololariFiltroComboBox.SelectedValue = struttureEsterne(0).Id
                ParsecUtility.SessionManager.ParametriPagina = Nothing
            Else
                ParsecUtility.Utility.ShowPopup(pageUrl, 910, 720, queryString, False)
            End If
            rubrica.Dispose()
        Else
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 720, queryString, False)
        End If
    End Sub

    Protected Sub AggiornaBeneficiarioFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBeneficiarioFiltroImageButton.Click
        If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
            Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
            Me.TitololariFiltroComboBox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "") 'strutturaEsterna.Denominazione
            Me.TitololariFiltroComboBox.SelectedValue = strutturaEsterna.Id
            ParsecUtility.SessionManager.Rubrica = Nothing
        End If
    End Sub

#End Region

#Region "GESTIONE CLASSIFICAZIONE"

    Protected Sub TrovaClassificazioneFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneFiltroImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneFiltroImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
    End Sub

    Protected Sub AggiornaClassificazioneFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneFiltroImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim classificazioneCompleta As String = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione2(idClassificazione, 1) & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneFiltroTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneFiltroTextBox.Text = idClassificazione.ToString
            Session("ClassificazioniSelezionate") = Nothing
        End If
    End Sub

    Protected Sub EliminaClassificazioneFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaClassificazioneFiltroImageButton.Click
        Me.ClassificazioneFiltroTextBox.Text = String.Empty
        Me.IdClassificazioneFiltroTextBox.Text = String.Empty
    End Sub

#End Region

#Region "GESTIONE RESPONSABILE"

    Protected Sub TrovaResponsabileFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaResponsabileFiltroImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaResponsabileFiltroImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "400")
        parametriPagina.Add("ultimoLivelloStruttura", "400")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 910, 670, queryString, False)
    End Sub


    Protected Sub AggiornaResponsabileFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaResponsabileFiltroImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Me.ResponsabileFiltroTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdResponsabileFiltroTextBox.Text = struttureSelezionate.First.Id.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Protected Sub EliminaResponsabileFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaResponsabileFiltroImageButton.Click
        Me.ResponsabileFiltroTextBox.Text = String.Empty
        Me.IdResponsabileFiltroTextBox.Text = String.Empty
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ProtocolliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FascicoliGridView.NeedDataSource
        Me.FascicoliGridView.DataSource = Me.Fascicoli
    End Sub

    Protected Sub FascicoliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FascicoliGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                SelezionaFascicolo(e.Item)
        End Select
    End Sub

    Protected Sub FascicoliGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FascicoliGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona Fascicolo"
            End If
        End If
    End Sub

    Protected Sub FascicoliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FascicoliGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub



#End Region

#Region "AZIONI PANNELLO FILTRO"

    Private Sub ResettaFiltro()
        Me.NumeroRegistroInizioTextBox.Text = String.Empty
        Me.NumeroRegistroFineTextBox.Text = String.Empty
        Me.OggettoFiltroTextBox.Text = String.Empty
        Me.ClassificazioneFiltroTextBox.Text = String.Empty
        Me.IdClassificazioneFiltroTextBox.Text = String.Empty
        Me.ResponsabileFiltroTextBox.Text = String.Empty
        Me.IdResponsabileFiltroTextBox.Text = String.Empty
        Me.CodiceFascicoloCompletoTextBox.Text = String.Empty
        Me.DataInizioAperturaTextBox.SelectedDate = Nothing
        Me.DataFineAperturaTextBox.SelectedDate = Nothing
        Me.DataInizioChiusuraTextBox.SelectedDate = Nothing
        Me.DataFineChiusuraTextBox.SelectedDate = Nothing
        Me.StatoFascicoloFiltroComboBox.SelectedIndex = 0
        Me.ProcedimentoFiltroComboBox.SelectedIndex = 0
        Me.TitololariFiltroComboBox.SelectedIndex = 0
        Me.TipologiaFascicoloFiltroComboBox.SelectedIndex = 0
        Me.Fascicoli = Nothing
        Me.FascicoliGridView.Rebind()
    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub




    Private Function GetFiltroFascicoli() As ParsecAdmin.FascicoloFiltro

        Dim filtro As New ParsecAdmin.FascicoloFiltro

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Not String.IsNullOrEmpty(Me.OggettoFiltroTextBox.Text) Then
            filtro.Oggetto = Me.OggettoFiltroTextBox.Text.Trim
        End If

        filtro.NumeroRegistroInizio = Me.NumeroRegistroInizioTextBox.Value
        filtro.NumeroRegistroFine = Me.NumeroRegistroFineTextBox.Value


        If Not String.IsNullOrEmpty(Me.TitololariFiltroComboBox.SelectedValue) Then

            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim titolare = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.TitololariFiltroComboBox.SelectedValue) And c.LogStato Is Nothing).FirstOrDefault
            If Not titolare Is Nothing Then

                Dim listaIdContatti = rubrica.GetQuery().Where(Function(w) w.Codice = titolare.Codice).Select(Function(c) c.Id).ToList
                For Each contattoId In listaIdContatti
                    filtro.listaIdTitolariToFind.Add(contattoId)
                Next
            End If
            rubrica.Dispose()
        End If

        If Not String.IsNullOrEmpty(Me.CodiceFascicoloCompletoTextBox.Text) Then
            filtro.CodiceFascicoloCompleto = Me.CodiceFascicoloCompletoTextBox.Text
        End If

        If Me.ProcedimentoFiltroComboBox.SelectedValue <> "0" Then
            filtro.IdProcedimento = Me.ProcedimentoFiltroComboBox.SelectedValue
        End If

        If Not String.IsNullOrEmpty(Me.IdResponsabileFiltroTextBox.Text) Then
            filtro.IdResponsabile = CInt(Me.IdResponsabileFiltroTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.IdClassificazioneFiltroTextBox.Text) Then
            filtro.IdClassificazione = CInt(Me.IdClassificazioneFiltroTextBox.Text)
        End If

        filtro.StatoFascicolo = Me.StatoFascicoloFiltroComboBox.SelectedValue

        filtro.IdUtenteCollegato = utenteCollegato.Id


        If Me.DataInizioAperturaTextBox.SelectedDate.HasValue Then
            filtro.DataAperturaInizio = Me.DataInizioAperturaTextBox.SelectedDate
        End If

        If Me.DataFineAperturaTextBox.SelectedDate.HasValue Then
            filtro.DataAperturaFine = Me.DataFineAperturaTextBox.SelectedDate
        End If

        If Me.DataInizioChiusuraTextBox.SelectedDate.HasValue Then
            filtro.DataChiusuraInizio = Me.DataInizioChiusuraTextBox.SelectedDate
        End If

        If Me.DataFineChiusuraTextBox.SelectedDate.HasValue Then
            filtro.DataChiusuraFine = Me.DataFineChiusuraTextBox.SelectedDate
        End If

        If Me.TipologiaFascicoloFiltroComboBox.SelectedIndex <> 0 Then
            filtro.IdTipologiaFascicolo = CInt(Me.TipologiaFascicoloFiltroComboBox.SelectedValue)
        End If


        Return filtro

    End Function

    Private Sub FiltraFascicoli()
        Dim fascicoli As New ParsecAdmin.FascicoliRepository
        Me.Fascicoli = fascicoli.GetView(Me.GetFiltroFascicoli)
        fascicoli.Dispose()
    End Sub

    Protected Sub CercaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CercaButton.Click
        'Dim message As New StringBuilder
        'If Me.ConvalidaParametri(message) Then
        '    ParsecUtility.SessionManager.FiltroRegistrazione = Me.GetFiltroFascicoli
        '    ParsecUtility.Utility.ClosePopup(False)
        'Else
        '    ParsecUtility.Utility.MessageBox(message.ToString, False)
        'End If
    End Sub

    Protected Sub AvantiImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AvantiImageButton.Click
        Me.FiltraFascicoli()
        Me.FascicoliGridView.Rebind()
        Me.VisualizzaPannello(TipoPannello.Risultati)
    End Sub

#End Region

#Region "AZIONI PANNELLO RISULTATI"

    Private Sub SelezionaFascicolo(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id"))
        Me.GetFascicolo(id)
    End Sub

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Me.Conferma()
    End Sub


    Protected Sub ConfermaButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton2.Click
        Me.Conferma()
    End Sub

    Private Sub Conferma()
        If Not Me.Fascicolo Is Nothing Then
            ParsecUtility.SessionManager.Fascicolo = Me.Fascicolo
            ParsecUtility.Utility.DoWindowClose(False)
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un fascicolo!", False)
        End If
    End Sub


    Protected Sub DettaglioImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DettaglioImageButton.Click
        If Not Me.Fascicolo Is Nothing Then

            'Trovo la posizione corrente nella lista
            Dim list As List(Of ParsecAdmin.Fascicolo) = Me.Fascicoli
            Me.CurrentPosition = list.FindIndex(Function(c) c.Id = Me.Fascicolo.Id)

            Me.CountItemLabel.Text = String.Format("di {0}", list.Count)
            Me.PositionItemTextBox.Text = (Me.CurrentPosition + 1).ToString

            Me.SetButtonState()

            Me.VisualizzaPannello(TipoPannello.Dettaglio)
            Me.AggiornaVista(Me.Fascicolo)

            Me.ImpostaUiPagina()
            Me.ImpostaAbilitazioneUi()

        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un fascicolo!", False)
        End If
    End Sub

    Protected Sub IndietroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles IndietroImageButton.Click
        Me.VisualizzaPannello(TipoPannello.Filtro)
        Me.Fascicolo = Nothing
    End Sub

#End Region

#Region "AZIONI PANNELLO DETTAGLIO"

    Protected Sub IndietroRisultatiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles IndietroRisultatiButton.Click
        Me.VisualizzaPannello(TipoPannello.Risultati)
        'Deseleziono la riga
        Me.FascicoliGridView.SelectedIndexes.Clear()
        Me.Fascicolo = Nothing

    End Sub

#End Region


#Region "GESTIONE VISUALIZZAZIONE FASCICOLO"

    Private Sub ResettaVista()
        Me.Fascicolo = Nothing

        Me.TipologiaFascicoloComboBox.SelectedIndex = 0

        Me.CodiceFascicoloUtenteTextBox.Text = String.Empty
        Me.DataTextBox.SelectedDate = Now

        Me.DataChiusuraTextBox.SelectedDate = Nothing
        Me.OggettoTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty


        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.CodiceFascicoloSistemaTextBox.Text = String.Empty
        Me.CodiceClassificazioneTextBox.Text = String.Empty

        Me.IdResponsabileTextBox.Text = String.Empty
        Me.ResponsabileTextBox.Text = String.Empty

        Me.ProcedimentoComboBox.SelectedValue = String.Empty
        Me.ProcedimentoComboBox.Text = String.Empty

        Me.numeroProvvedimentoChiusuraTextbox.Text = String.Empty



        Me.BeneficiarioComboBox.SelectedValue = String.Empty
        Me.BeneficiarioComboBox.Text = String.Empty

        Me.LegaleComboBox.SelectedValue = String.Empty
        Me.LegaleComboBox.Text = String.Empty
        Me.TipoDocumentoComboBox.SelectedIndex = 0

        Me.Titolari = New List(Of ParsecAdmin.TitolareFascicolo)
        Me.Documenti = New List(Of ParsecAdmin.FascicoloDocumento)
        Me.Visibilita = New List(Of ParsecAdmin.VisibilitaDocumento)


    End Sub

    Private Sub AggiornaVista(ByVal fascicolo As ParsecAdmin.Fascicolo)
        Dim fascicoli As New ParsecAdmin.FascicoliRepository
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.ResettaVista()
        Me.Fascicolo = fascicolo
        Me.TipologiaFascicoloComboBox.FindItemByValue(Me.Fascicolo.IdTipologiaFascicolo).Selected = True

        'Me.NumeroRegistroTextBox.Text = IIf(Me.Fascicolo.NumeroRegistro Is Nothing, "", Me.Fascicolo.NumeroRegistro)
        Me.CodiceFascicoloUtenteTextBox.Text = Me.Fascicolo.CodiceFascicoloUtente
        Me.CodiceFascicoloSistemaTextBox.Text = Me.Fascicolo.CodiceFascicoloSistema
        Me.DataTextBox.SelectedDate = Me.Fascicolo.DataApertura

        Me.DataChiusuraTextBox.SelectedDate = Me.Fascicolo.DataChiusura

        Me.OggettoTextBox.Text = Me.Fascicolo.Oggetto
        Me.NoteTextBox.Text = Me.Fascicolo.Note
        Me.numeroProvvedimentoChiusuraTextbox.Text = Me.Fascicolo.NumeroProvvedimentoChiusura


        If Me.Fascicolo.CodClassificaAppartenenza.HasValue Then
            Dim titolario As ParsecAdmin.TitolarioClassificazione = (New ParsecAdmin.TitolarioClassificazioneRepository).GetQuery.Where(Function(c) c.Id = Me.Fascicolo.CodClassificaAppartenenza).FirstOrDefault
            Dim codici = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione(CInt(Me.Fascicolo.CodClassificaAppartenenza.Value.ToString), 1)
            Dim classificazioneCompleta As String = codici & " " & titolario.Descrizione
            Me.IdClassificazioneTextBox.Text = Me.Fascicolo.CodClassificaAppartenenza.Value.ToString
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.CodiceClassificazioneTextBox.Text = codici
        End If


        Me.IdResponsabileTextBox.Text = Me.Fascicolo.IdStrutturaUtenteResponsabile.ToString
        Me.ResponsabileTextBox.Text = Me.Fascicolo.StrutturaUtenteResponsabile




        If Me.Fascicolo.idProcedimento.HasValue Then
            Me.ProcedimentoComboBox.SelectedValue = Me.Fascicolo.idProcedimento
        Else
            Me.ProcedimentoComboBox.SelectedValue = String.Empty
        End If


        Me.Titolari = (New ParsecAdmin.TitolariFascicoloRepository).getTitolari(Me.Fascicolo.Id)
        Me.Documenti = (New ParsecAdmin.FascicoloDocumentoRepository).GetDocumenti(Me.Fascicolo.Id)

        '********************************************************************************************
        'SCHEDA VISIBILITA (Carico i gruppi e gli utenti associati al protocollo selezionato)
        '********************************************************************************************
        Me.Visibilita = fascicoli.GetVisibilita(Me.Fascicolo.Id)


        fascicoli.Dispose()


        Me.AggiornaGrigliaVisibilita()
        Me.AggiornaGrigliaDocumenti()
        Me.AggiornaGrigliaTitolari()

    End Sub

    Private Sub AggiornaGrigliaVisibilita()
        Me.VisibilitaGridView.DataSource = Me.Visibilita
        Me.VisibilitaGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaDocumenti()
        Me.DocumentiGridView.DataSource = Me.Documenti
        Me.DocumentiGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaTitolari()
        Me.TitolariGridView.DataSource = Me.Titolari
        Me.TitolariGridView.DataBind()
    End Sub



    Private Sub ImpostaUiPagina()
       Me.DatiFascicoloStrip.Tabs(0).Text = "Titolari" & If(Me.Titolari.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Titolari.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        Me.DatiFascicoloStrip.Tabs(1).Text = "Documenti" & If(Me.Documenti.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Documenti.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        Me.DatiFascicoloStrip.Tabs(2).Text = "Visibilità" & If(Me.Visibilita.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Visibilita.Count.ToString & ")</span>", "<span style='width:20px'></span>")
    End Sub

    Private Sub ImpostaAbilitazioneUi()
        Me.TipologiaFascicoloComboBox.Enabled = False
        Me.CodiceFascicoloUtenteTextBox.Enabled = False
        Me.DataTextBox.Enabled = False
        Me.DataChiusuraTextBox.Enabled = False
        Me.TrovaClassificazioneImageButton.Visible = False
        Me.EliminaClassificazioneImageButton.Visible = False
        Me.ProcedimentoComboBox.Enabled = False
        Me.ResponsabileTextBox.Enabled = False
        Me.TrovaResponsabileImageButton.Visible = False
        Me.EliminaResponsabileImageButton.Visible = False
        Me.OggettoTextBox.Enabled = False
        Me.NoteTextBox.Enabled = False
        Me.numeroProvvedimentoChiusuraTextbox.Enabled = False


        Me.TitolariGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.DocumentiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False


        Me.VisibilitaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.TrovaGruppoVisibilitaImageButton.Visible = False
        Me.TrovaUtenteVisibilitaImageButton.Visible = False
    End Sub



#End Region

#Region "GESTIONE NAVIGAZIONE"

    Protected Sub VaiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles VaiImageButton.Click
        If Not String.IsNullOrEmpty(Me.PositionItemTextBox.Text) Then
            Dim position As Integer
            If UInt32.TryParse(Me.PositionItemTextBox.Text, position) Then
                If position <= Me.Fascicoli.Count Then
                    Me.CurrentPosition = position - 1
                    Me.Scorri(Me.CurrentPosition)
                    Me.SetButtonState()
                End If
            End If
        End If
        Me.PositionItemTextBox.Text = (Me.CurrentPosition + 1).ToString
    End Sub

    Protected Sub PrimoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PrimoImageButton.Click
        Me.CurrentPosition = 0
        Me.Scorri(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    Protected Sub PrecedenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PrecedenteImageButton.Click
        Me.CurrentPosition -= 1
        Me.Scorri(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    Protected Sub UltimoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles UltimoImageButton.Click
        Me.CurrentPosition = Me.Fascicoli.Count - 1
        Me.Scorri(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    Protected Sub SuccessivoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SuccessivoImageButton.Click
        Me.CurrentPosition += 1
        Me.Scorri(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    Private Sub Scorri(ByVal position As Integer)
        Me.PositionItemTextBox.Text = (position + 1).ToString
        Dim id As Integer = Me.Fascicoli(Me.CurrentPosition).Id
        Me.GetFascicolo(id)
        Me.AggiornaVista(Me.Fascicolo)
        Me.ImpostaUiPagina()
        Me.ImpostaAbilitazioneUi()
    End Sub


    Private Sub GetFascicolo(ByVal id As Integer)
        Dim fascicoli As New ParsecAdmin.FascicoliRepository
        Me.Fascicolo = fascicoli.GetById(id)
        fascicoli.Dispose()
    End Sub

    Private Sub SetButtonState()
        Dim enableForward As Boolean = (Me.CurrentPosition < Me.Fascicoli.Count - 1)
        Dim enableBack As Boolean = Me.CurrentPosition > 0
        Dim enableGoto As Boolean = Me.Fascicoli.Count > 1

        Me.UltimoImageButton.Enabled = enableForward
        Me.SuccessivoImageButton.Enabled = enableForward
        Me.PrimoImageButton.Enabled = enableBack
        Me.PrecedenteImageButton.Enabled = enableBack
        Me.UltimoImageButton.ImageUrl = "~\images\" & If(enableForward, "Last", "LastDisabled") & ".png"
        Me.SuccessivoImageButton.ImageUrl = "~\images\" & If(enableForward, "Next", "NextDisabled") & ".png"
        Me.PrecedenteImageButton.ImageUrl = "~\images\" & If(enableBack, "Previous", "PreviousDisabled") & ".png"
        Me.PrimoImageButton.ImageUrl = "~\images\" & If(enableBack, "First", "FirstDisabled") & ".png"

        Me.PositionItemTextBox.Enabled = enableGoto
        Me.VaiImageButton.ImageUrl = "~\images\" & If(enableGoto, "Goto", "GotoDisabled") & ".png"

    End Sub

    Private Sub SetButtonImage()
        Me.PrimoImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\FirstSelected.png") & "'")
        Me.PrimoImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\First.png") & "'")
        Me.PrecedenteImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\PreviousSelected.png") & "'")
        Me.PrecedenteImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\Previous.png") & "'")
        Me.SuccessivoImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\NextSelected.png") & "'")
        Me.SuccessivoImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\Next.png") & "'")
        Me.UltimoImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\LastSelected.png") & "'")
        Me.UltimoImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\Last.png") & "'")
        Me.VaiImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\GotoSelected.png") & "'")
        Me.VaiImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\Goto.png") & "'")
    End Sub

#End Region

    
End Class