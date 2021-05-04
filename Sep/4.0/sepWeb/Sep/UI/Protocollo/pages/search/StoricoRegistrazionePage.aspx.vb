#Region "IMPORTS"

Imports ParsecAdmin
Imports Telerik.Web.UI
Imports ParsecPro

#End Region

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StoricoRegistrazionePage
    Inherits System.Web.UI.Page


#Region "ENUMERAZIONI"

    'Enumerazione dei tipi di pannello
    Public Enum TipoPannello
        Risultati = 0
        Dettaglio = 1
    End Enum

#End Region

#Region "GESTIONI PANNELLI"

    'Gestisce la visibilità dei vari pannelli gestiti nella maschera (Risultati e Dettaglio)
    Private Sub VisualizzaPannello(ByVal tipo As TipoPannello)
        Me.RisultatiPanel.Visible = False
        Me.DettaglioPanel.Visible = False
        Select Case tipo
            Case TipoPannello.Risultati
                Me.RisultatiPanel.Visible = True
            Case TipoPannello.Dettaglio
                Me.DettaglioPanel.Visible = True
        End Select

    End Sub

#End Region

#Region "PROPRIETA'"

    'Variabile di sessione: posizione corrente nella lista della Registrazione
    Public Property CurrentPosition As Integer
        Get
            Return Session("StoricoRegistrazionePage_CurrentPosition")
        End Get
        Set(ByVal value As Integer)
            Session("StoricoRegistrazionePage_CurrentPosition") = value
        End Set
    End Property

    'Variabile di sessione: Registrazione di Protocollo corrente
    Public Property Registrazione() As ParsecPro.Registrazione
        Get
            Return CType(Session("StoricoRegistrazionePage_Registrazione"), ParsecPro.Registrazione)
        End Get
        Set(ByVal value As ParsecPro.Registrazione)
            Session("StoricoRegistrazionePage_Registrazione") = value
        End Set
    End Property

    'Variabile di sesione: elenco delle registrazioni di protocollo
    Public Property Registrazioni() As List(Of ParsecPro.Registrazione)
        Get
            Return CType(Session("StoricoRegistrazionePage_Registrazioni"), Object)
        End Get
        Set(ByVal value As List(Of ParsecPro.Registrazione))
            Session("StoricoRegistrazionePage_Registrazioni") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.IsPostBack Then
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Me.Registrazioni = registrazioni.GetStoricoRegistrazioni(Me.GetFiltroRegistrazioni)
            registrazioni.Dispose()
        End If
        Me.VisualizzaPannello(TipoPannello.Risultati)
        Me.SetButtonImage()

        If Not Me.IsPostBack Then
            Dim tipiRicezione As New ParsecPro.TipiRicezioneInvioRepository
            Me.TipoRicezioneInvioComboBox.DataSource = tipiRicezione.GetView(Nothing)
            Me.TipoRicezioneInvioComboBox.DataTextField = "Descrizione"
            Me.TipoRicezioneInvioComboBox.DataValueField = "Id"
            Me.TipoRicezioneInvioComboBox.DataBind()
            Me.TipoRicezioneInvioComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Seleziona tipologia -", "-1"))
            Me.TipoRicezioneInvioComboBox.SelectedIndex = 0
            tipiRicezione.Dispose()

            Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
            Me.TipologiaDocumentoComboBox.DataSource = tipiDocumento.GetView(Nothing)
            Me.TipologiaDocumentoComboBox.DataTextField = "Descrizione"
            Me.TipologiaDocumentoComboBox.DataValueField = "Id"
            Me.TipologiaDocumentoComboBox.DataBind()
            Me.TipologiaDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Seleziona tipologia -"))
            Me.TipologiaDocumentoComboBox.SelectedIndex = 0
            tipiDocumento.Dispose()

            'Per usi futuri
            Me.StatoDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Seleziona stato documento -"))
            Me.StatoDocumentoComboBox.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem("Archiviato"))
            Me.StatoDocumentoComboBox.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem("Assegnato"))
            Me.StatoDocumentoComboBox.Items.Insert(3, New Telerik.Web.UI.RadComboBoxItem("Eliminato"))
            Me.StatoDocumentoComboBox.Items.Insert(4, New Telerik.Web.UI.RadComboBoxItem("In Elaborazione"))
            Me.StatoDocumentoComboBox.Items.Insert(5, New Telerik.Web.UI.RadComboBoxItem("Inviato"))
            Me.StatoDocumentoComboBox.Items.Insert(6, New Telerik.Web.UI.RadComboBoxItem("Urgente"))

            Dim modelli As New ParsecWKF.ModelliRepository
            Me.TipologiaIterComboBox.DataSource = modelli.GetView(Nothing)
            Me.TipologiaIterComboBox.DataTextField = "Descrizione"
            Me.TipologiaIterComboBox.DataValueField = "Id"
            Me.TipologiaIterComboBox.DataBind()
            Me.TipologiaIterComboBox.SelectedIndex = 0
            modelli.Dispose()
        End If


        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.DocumentiGridView.Style.Add("width", widthStyle)

        Me.CollegamentiDirettiGridView.Style.Add("width", widthStyle)
        Me.CollegamentiIndirettiGridView.Style.Add("width", widthStyle)

        Me.VisibilitaGridView.Style.Add("width", widthStyle)
        Me.FascicoliGridView.Style.Add("width", widthStyle)

        Me.ReferentiEsterniGridView.Style.Add("width", widthStyle)
        Me.SecondoReferentiInterniGridView.Style.Add("width", widthStyle)

        Me.GrigliaFascicoliPanel.Style.Add("width", widthStyle)
        Me.FascicoliPanel.Style.Add("width", widthStyle)

    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla griglia ProtocolliGridView. Aggancia il datasource della griglia alla variabile di sessione Registrazioni.
    Protected Sub ProtocolliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ProtocolliGridView.NeedDataSource
        Me.ProtocolliGridView.DataSource = Me.Registrazioni
    End Sub

    'Evento ItemCommand associato alla ProtocolliGridView. Fa partire i comandi associati alla griglia delle registrazioni di Protocollo ("Select").
    Protected Sub ProtocolliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ProtocolliGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                SelezionaRegistrazione(e.Item)
        End Select
    End Sub

    'Evento ItemDataBound associato alla ProtocolliGridView. Setta il tooltip all icona "Select". 
    Protected Sub ProtocolliGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ProtocolliGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona registrazione"
            End If
        End If
    End Sub

#End Region

#Region "AZIONI PANNELLO FILTRO"

    'Costruisce e restituisce il Filtro utilizzato per la ricerca
    Private Function GetFiltroRegistrazioni() As ParsecPro.RegistrazioneFiltro
        Dim idRegistrazione As Integer = Me.Page.Request.QueryString("r")
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetQuery.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault
        registrazioni.Dispose()
        Dim filtro As New ParsecPro.RegistrazioneFiltro With
            {
              .NumeroProtocolloInizio = registrazione.NumeroProtocollo,
              .DataProtocolloInizio = registrazione.DataImmissione
               }
        Return filtro
    End Function

#End Region

#Region "AZIONI PANNELLO RISULTATI"

    'Seleziona la registraizone dalla Griglia: setta la variabile di sessione IdRegistrazioneSelezionata
    Private Sub SelezionaRegistrazione(ByVal item As Telerik.Web.UI.GridDataItem)
        Session("RicercaRegistrazione_IdRegistrazioneSelezionata") = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    End Sub

    'Apre il pannello di dettaglio caricando i dati della registrazione selezionata
    Protected Sub DettaglioImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DettaglioImageButton.Click
        If Not Session("RicercaRegistrazione_IdRegistrazioneSelezionata") Is Nothing Then

            'Trovo la posizione corrente nella lista
            Dim list As List(Of ParsecPro.Registrazione) = Me.Registrazioni
            Me.CurrentPosition = list.FindIndex(Function(c) c.Id = Session("RicercaRegistrazione_IdRegistrazioneSelezionata"))

            Me.CountItemLabel.Text = String.Format("di {0}", list.Count)
            Me.PositionItemTextBox.Text = (Me.CurrentPosition + 1).ToString

            Me.SetButtonState()

            Me.VisualizzaPannello(TipoPannello.Dettaglio)
            Me.AggiornaVista(Session("RicercaRegistrazione_IdRegistrazioneSelezionata"))
            Me.ImpostaUiPagina(Me.Registrazione.TipologiaRegistrazione)
            Me.ImpostaAbilitazioneUi()

        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una registrazione!", False)
        End If
    End Sub

#End Region

#Region "AZIONI PANNELLO DETTAGLIO"

    'Consente di passare dal dettaglio della registrazione alla lista dei movimenti storici della registrazione
    Protected Sub IndietroRisultatiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles IndietroRisultatiButton.Click
        Me.VisualizzaPannello(TipoPannello.Risultati)
        'Deseleziono la riga
        Me.ProtocolliGridView.SelectedIndexes.Clear()
        Me.Registrazione = Nothing
        Session("RicercaRegistrazione_IdRegistrazioneSelezionata") = Nothing
    End Sub

#End Region

#Region "GESTIONE VISUALIZZAZIONE REGISTRAZIONE"

    'Si va nel dettaglio della Registrazione selezionata nella griglia
    Private Sub AggiornaVista(ByVal id As Integer)

        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Me.Registrazione = registrazioni.GetById(id)

        '********************************************************************************************
        'Carico le informazioni a prescindere dall'operazione
        '********************************************************************************************

        Me.OggettoTextBox.Text = Registrazione.Oggetto

        If Registrazione.IdClassificazione.HasValue Then
            Me.ClassificazioneTextBox.Text = Registrazione.ClassificazioneCompleta
            Me.IdClassificazioneTextBox.Text = Registrazione.IdClassificazione.ToString
        End If

        Select Case Registrazione.TipologiaRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                Me.ReferentiEsterniGridView.DataSource = Registrazione.Mittenti.ToList
                Me.SecondoReferentiInterniGridView.DataSource = Registrazione.Destinatari.Where(Function(c) c.Interno = True).ToList 'Interni
            Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                Me.ReferentiEsterniGridView.DataSource = Registrazione.Destinatari.ToList
                Me.SecondoReferentiInterniGridView.DataSource = Registrazione.Mittenti.Where(Function(c) c.Interno = True).ToList 'Interni
        End Select


        Me.ReferentiEsterniGridView.DataBind()
        Me.SecondoReferentiInterniGridView.DataBind()
        '********************************************************************************************

        'Seleziono il tipo di registrazione
        Me.TipoRegistrazioneRadioList.SelectedIndex = Registrazione.TipoRegistrazione

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("numeroCifreProtocollo", ParsecAdmin.TipoModulo.PRO)
        Dim numeroCifre As Integer = 7
        If Not parametro Is Nothing Then
            numeroCifre = CInt(parametro.Valore)
        End If

        Dim color As String = String.Empty
        Dim descrizione As String = String.Empty
        Select Case CType(Me.TipoRegistrazioneRadioList.SelectedIndex, ParsecPro.TipoRegistrazione)
            Case TipoRegistrazione.Arrivo
                color = "#FF0000"
                descrizione = "ARRIVO"
            Case TipoRegistrazione.Partenza
                color = "#00AA00"
                descrizione = "PARTENZA"
            Case TipoRegistrazione.Interna
                color = "#FF8000"
                descrizione = "INTERNO"
        End Select

        Me.AreaInfoLabel.Text = "<font color='#00156E'>Protocollo N° " & Registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy}", Registrazione.DataImmissione) & " - " & "</font><font color='" & color & "'>" & descrizione & "</font>"
        parametri.Dispose()

        Me.RiservatoCheckBox.Checked = Registrazione.Riservato

        Me.AggiornaPannelloAvanzate(Registrazione)
        Me.AggiornaPannelloDocumenti(Registrazione)


        '********************************************************************************************
        'Carico i collegamenti associati al protocollo selezionato
        '********************************************************************************************

        Me.CollegamentiDirettiGridView.DataSource = Registrazione.Collegamenti.Where(Function(c) c.Diretto = True).ToList
        Me.CollegamentiDirettiGridView.DataBind()
        Me.CollegamentiIndirettiGridView.DataSource = Registrazione.Collegamenti.Where(Function(c) c.Diretto = False).ToList
        Me.CollegamentiIndirettiGridView.DataBind()

        '********************************************************************************************

        '********************************************************************************************
        'Carico gli allegati associati al protocollo selezionato
        '********************************************************************************************

        Me.DocumentiGridView.DataSource = Registrazione.Allegati
        Me.DocumentiGridView.DataBind()

        Me.DatiProtocolloTabStrip.Tabs(2).Text = "Allegati" & If(Registrazione.Allegati.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Registrazione.Allegati.Count.ToString & ")</span>", "<span style='width:20px'></span>")

        '********************************************************************************************
        'Carico i fascicoli associati al protocollo selezionato
        '********************************************************************************************

        Me.FascicoliGridView.DataSource = Registrazione.Fascicoli
        Me.FascicoliGridView.DataBind()

        '********************************************************************************************

        Me.VisibilitaGridView.DataSource = Me.Registrazione.Visibilita
        Me.VisibilitaGridView.DataBind()

        Me.DatiProtocolloTabStrip.Tabs(4).Text = "Fascicoli" & If(Me.Registrazione.Fascicoli.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Registrazione.Fascicoli.Count.ToString & ")</span>", "<span style='width:20px'></span>")

        Me.DatiProtocolloTabStrip.Tabs(5).Text = "Visibilità" & If(Me.Registrazione.Visibilita.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Registrazione.Visibilita.Count.ToString & ")</span>", "<span style='width:20px'></span>")

        Dim registrazioneCorrente = registrazioni.Where(Function(c) c.NumeroProtocollo = Registrazione.NumeroProtocollo And c.Modificato = False And c.Annullato = False And c.TipoRegistrazione = Registrazione.TipoRegistrazione And Year(c.DataImmissione) = Year(Registrazione.DataImmissione)).FirstOrDefault

        registrazioni.Dispose()


        '********************************************************************************************
        'SELEZIONO L'ITER
        '********************************************************************************************
        If Not registrazioneCorrente Is Nothing Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim istanza = istanze.Where(Function(c) c.IdDocumento = registrazioneCorrente.Id And c.IdModulo = ParsecAdmin.TipoModulo.PRO).FirstOrDefault
            If Not istanza Is Nothing Then
                Me.TipologiaIterComboBox.Items.FindItemByValue(istanza.IdModello).Selected = True
            End If
            istanze.Dispose()
        End If


        '********************************************************************************************


    End Sub

    'Aggiorna le informazioni del Pannello delle informazioni avanzate associate alla registrazione.
    Private Sub AggiornaPannelloAvanzate(ByVal registrazione As ParsecPro.Registrazione)
        Me.DataRicezioneInvioTextBox.SelectedDate = registrazione.DataOraRicezioneInvio
        Me.OrarioRicezioneInvioTextBox.SelectedDate = registrazione.DataOraRicezioneInvio
        If Not registrazione.IdTipoDocumento Is Nothing Then
            Me.TipologiaDocumentoComboBox.FindItemByValue(registrazione.IdTipoDocumento).Selected = True
        End If

        If registrazione.IdTipoRicezione.HasValue Then
            Me.TipoRicezioneInvioComboBox.FindItemByValue(registrazione.IdTipoRicezione).Selected = True
        End If
        Me.ProtocolloMittenteTextBox.Text = registrazione.ProtocolloMittente

        If registrazione.DataDocumento.HasValue Then
            Me.DataDocumentoTextBox.SelectedDate = registrazione.DataDocumento
        End If


        Me.NoteTextBox.Text = registrazione.Note
        Me.NoteInterneTextBox.Text = registrazione.NoteInterne
    End Sub

    'Aggiorna il numero dei documenti allegati alla registrazione
    Private Sub AggiornaPannelloDocumenti(ByVal registrazione As ParsecPro.Registrazione)
        If Not registrazione.NumeroAllegati Is Nothing Then
            Me.NumeroDocumentiTextBox.Text = registrazione.NumeroAllegati
        End If
    End Sub

    'Imposta i vari titoli e tooltip della interfaccia grafica
    Private Sub ImpostaUiPagina(ByVal tipoRegistrazione As ParsecPro.TipoRegistrazione)
        Select Case tipoRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                Me.ReferenteEsternoLabel.Text = "Mittenti&nbsp;&nbsp;" & If(Me.ReferentiEsterniGridView.MasterTableView.Items.Count > 0, "( " & Me.ReferentiEsterniGridView.MasterTableView.Items.Count.ToString & " )", "")
                Me.SecondoReferenteInternoLabel.Text = "Destinatari&nbsp;&nbsp;" & If(Me.SecondoReferentiInterniGridView.MasterTableView.Items.Count > 0, "( " & Me.SecondoReferentiInterniGridView.MasterTableView.Items.Count.ToString & " )", "")
                Me.ReferentiEsterniGridView.ToolTip = "Elenco mittenti associati al protocollo"
                Me.SecondoReferentiInterniGridView.ToolTip = "Elenco destinatari interni associati al protocollo"
                Me.DataOraRicezioneInvioLabel.Text = "Data/Ora ricezione"
                Me.TipoRicezioneInvioLabel.Text = "Tipo ricezione"
            Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                Me.ReferenteEsternoLabel.Text = "Destinatari&nbsp;&nbsp;" & If(Me.ReferentiEsterniGridView.MasterTableView.Items.Count > 0, "( " & Me.ReferentiEsterniGridView.MasterTableView.Items.Count.ToString & " )", "")
                Me.SecondoReferenteInternoLabel.Text = "Mittenti&nbsp;&nbsp;" & If(Me.SecondoReferentiInterniGridView.MasterTableView.Items.Count > 0, "( " & Me.SecondoReferentiInterniGridView.MasterTableView.Items.Count.ToString & " )", "")
                Me.ReferentiEsterniGridView.ToolTip = "Elenco destinatari associati al protocollo"
                Me.SecondoReferentiInterniGridView.ToolTip = "Elenco mittenti interni associati al protocollo"
                Me.DataOraRicezioneInvioLabel.Text = "Data/Ora invio"
                Me.TipoRicezioneInvioLabel.Text = "Tipo invio"
        End Select
        Me.CollegamentiDirettiLabel.Text = "Collegamenti diretti&nbsp;&nbsp;" & If(Me.CollegamentiDirettiGridView.MasterTableView.Items.Count > 0, "( " & Me.CollegamentiDirettiGridView.MasterTableView.Items.Count.ToString & " )", "")
        Me.CollegamentiIndirettiLabel.Text = "Collegamenti indiretti&nbsp;&nbsp;" & If(Me.CollegamentiIndirettiGridView.MasterTableView.Items.Count > 0, "( " & Me.CollegamentiIndirettiGridView.MasterTableView.Items.Count.ToString & " )", "")
        Me.VisibilitaLabel.Text = "Elenco Visibilità&nbsp;&nbsp;" & If(Me.VisibilitaGridView.MasterTableView.Items.Count > 0, "( " & Me.VisibilitaGridView.MasterTableView.Items.Count.ToString & " )", "")

    End Sub

    'Imposta le abilitazioni di visibilita ed enabled della Interfaccia Grafica 
    Private Sub ImpostaAbilitazioneUi()
        '******************************************************************************************
        'Scheda Fascicoli
        '******************************************************************************************
        
        Me.TrovaFascicoloImageButton.Visible = False
        Me.NuovoFascicoloImageButton.Visible = False
        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
        Me.FaseDocumentoFascicoloLabel.Visible = False
        Me.FaseDocumentoFascicoloComboBox.Visible = False

        '******************************************************************************************

        '******************************************************************************************
        'Scheda Collegamenti
        '******************************************************************************************
       
        Me.TrovaCollegamentoImageButton.Visible = False
        Me.CollegamentiIndirettiGridView.MasterTableView.Columns.FindByUniqueName("Detail").Visible = False
        Me.CollegamentiDirettiGridView.MasterTableView.Columns.FindByUniqueName("Detail").Visible = False
        Me.CollegamentiDirettiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False

        '******************************************************************************************

        '******************************************************************************************
        'Scheda Documenti
        '******************************************************************************************

        Me.ScansionaImageButton.Visible = False
        Me.AggiungiDocumentoImageButton.Visible = False
        Me.DocumentiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.DescrizioneDocumentoTextBox.Enabled = False
        Me.NumeroDocumentiTextBox.Enabled = False
        Me.DocumentoPrimarioRadioButton.Enabled = False
        Me.DocumentoAllegatoRadioButton.Enabled = False
        Me.AllegatoUpload.Enabled = False

        '******************************************************************************************

        '******************************************************************************************
        'Scheda Avanzate
        '******************************************************************************************

        Me.DataRicezioneInvioTextBox.Enabled = False
        Me.TipologiaDocumentoComboBox.Enabled = False
        Me.StatoDocumentoComboBox.Enabled = False
        Me.EliminaNoteImageButton.Visible = False
        Me.NoteTextBox.Enabled = False
        Me.EliminaNoteInterneImageButton.Visible = False
        Me.NoteInterneTextBox.Enabled = False
        Me.ProtocolloMittenteTextBox.Enabled = False
        Me.DataDocumentoTextBox.Enabled = False
        Me.AnticipatoViaFaxCheckBox.Enabled = False
        Me.TipoRicezioneInvioComboBox.Enabled = False
        Me.FiltroCategoriaTextBox.Visible = False
        Me.FiltroClasseTextBox.Visible = False
        Me.FiltroSottoClasseTextBox.Visible = False
        Me.FiltraClassificazioneImageButton.Visible = False

        Me.FiltroDescrizioneClassificazioneTextBox.Visible = False
        Me.TrovaClassificazioneImageButton.Visible = False
        Me.ClassificazioneTextBox.Enabled = False

        Me.EliminaClassificazioneImageButton.Visible = False

        '******************************************************************************************

        '******************************************************************************************
        'Scheda Generale
        '******************************************************************************************

        Me.TipoRegistrazioneRadioList.Enabled = False
        Me.FiltroDenominazioneTextBox.Visible = False
        Me.TrovaReferenteEsternoImageButton.Visible = False

        Me.TrovaPrimoReferenteInternoImageButton.Visible = False
        Me.AggiungiNuovoReferenteEsternoImageButton.Visible = False
        Me.FiltroSecondoReferenteInternoTextBox.Visible = False
        Me.TrovaSecondoReferenteInternoImageButton.Visible = False
        Me.TrovaOggettoImageButton.Visible = False
        Me.OggettoTextBox.Enabled = False

        Me.ReferentiEsterniGridView.Enabled = False
        Me.SecondoReferentiInterniGridView.Enabled = False

        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("Modifica").Visible = False
        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione <> ParsecPro.TipoRegistrazione.Arrivo
        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxIterTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione <> ParsecPro.TipoRegistrazione.Arrivo

        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo
        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxIterTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo
        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxInviaEmailTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo

        '******************************************************************************************

        Me.PannelloSessioneEmergenza.Visible = False
        Me.TipologiaIterComboBox.Enabled = False
        Me.TrovaReferenteEsternoIpaImageButton.Visible = False
        Me.OrarioRicezioneInvioTextBox.Enabled = False

        Me.VisibilitaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.TrovaGruppoVisibilitaImageButton.Visible = False
        Me.TrovaUtenteVisibilitaImageButton.Visible = False

    End Sub

#End Region

#Region "GESTIONE NAVIGAZIONE"

    'Consente di navigare i Risultati del pannello di dettaglio
    Protected Sub VaiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles VaiImageButton.Click
        If Not String.IsNullOrEmpty(Me.PositionItemTextBox.Text) Then
            Dim position As Integer
            If UInt32.TryParse(Me.PositionItemTextBox.Text, position) Then
                If position <= Me.Registrazioni.Count Then
                    Me.CurrentPosition = position - 1
                    Me.ScorriRegistrazioni(Me.CurrentPosition)
                    Me.SetButtonState()
                End If
            End If
        End If
        Me.PositionItemTextBox.Text = (Me.CurrentPosition + 1).ToString
    End Sub

    'Consente di navigare i Risultati del pannello di dettaglio
    Protected Sub PrimoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PrimoImageButton.Click
        Me.CurrentPosition = 0
        Me.ScorriRegistrazioni(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    'Consente di navigare i Risultati del pannello di dettaglio
    Protected Sub PrecedenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PrecedenteImageButton.Click
        Me.CurrentPosition -= 1
        Me.ScorriRegistrazioni(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    'Consente di navigare i Risultati del pannello di dettaglio
    Protected Sub UltimoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles UltimoImageButton.Click
        Me.CurrentPosition = Me.Registrazioni.Count - 1
        Me.ScorriRegistrazioni(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    'Consente di navigare i Risultati del pannello di dettaglio
    Protected Sub SuccessivoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SuccessivoImageButton.Click
        Me.CurrentPosition += 1
        Me.ScorriRegistrazioni(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    'Consente di navigare i Risultati del pannello di dettaglio
    Private Sub ScorriRegistrazioni(ByVal position As Integer)
        Me.PositionItemTextBox.Text = (position + 1).ToString
        Dim id As Integer = Me.Registrazioni(Me.CurrentPosition).Id
        Session("RicercaRegistrazione_IdRegistrazioneSelezionata") = id
        Me.AggiornaVista(id)
        Me.ImpostaUiPagina(Me.Registrazione.TipologiaRegistrazione)
        Me.ImpostaAbilitazioneUi()
    End Sub

    'Setta nella barra di navigazione del dettaglio le varie icone. Richiamato daimetodi di navigazione
    Private Sub SetButtonState()
        Dim enableForward As Boolean = (Me.CurrentPosition < Me.Registrazioni.Count - 1)
        Dim enableBack As Boolean = Me.CurrentPosition > 0
        Dim enableGoto As Boolean = Me.Registrazioni.Count > 1

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
        Me.VaiImageButton.Enabled = enableGoto
    End Sub

    'Setta nella barra di navigazione del dettaglio i vari eventi onMouseOver e onMouseOut. Richiamato dal Page.Init
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

#Region "EVENTI GRIGLIA  ALLEGATI"

    'Evento ItemDataBound associato alla DocumentiGridView. Setta tooltip, icone ed eventi in base al contentuo delle celle.
    Protected Sub DocumentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina documento"
            End If
            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Preview").Controls(0), ImageButton)
                btn.ToolTip = "Visualizza documento"
            End If

            If TypeOf dataItem("SignedPreview").Controls(0) Is ImageButton Then

                Dim btnSignedPreview As ImageButton = CType(dataItem("SignedPreview").Controls(0), ImageButton)

                Dim nomeFileFirmato As String = dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("NomeFileFirmato")
                If Not String.IsNullOrEmpty(nomeFileFirmato) Then
                    btnSignedPreview.ImageUrl = "~\images\signedDocument16.png"
                    btnSignedPreview.ToolTip = "Visualizza documento firmato."
                Else
                    btnSignedPreview.ImageUrl = "~\images\vuoto.png"
                    btnSignedPreview.Attributes.Add("onclick", "return false;")
                    btnSignedPreview.ToolTip = ""
                End If

            End If

            Dim lbl As Label = CType(e.Item.FindControl("NumeratoreLabel"), Label)
            lbl.Text = (e.Item.ItemIndex + 1).ToString

        End If
    End Sub

    'Evento ItemCommand associato alla DocumentiGridView. Esegue i comandi previsti nella griglia: Preview (download del documento) e "SignedPreview" (visualizzazione del p7m)
    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Preview"
                Me.DownloadFile(e.Item)

            Case "SignedPreview"
                Me.VisualizzaDocumentoP7M(e.Item)
        End Select
    End Sub

    'Visualizza il file p7m (se è un file firmato)
    Private Sub VisualizzaDocumentoP7M(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        Dim percorsoRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim nomeFile As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As ParsecPro.Allegato = Me.Registrazione.Allegati.Where(Function(c) c.NomeFile = nomeFile).FirstOrDefault

        If Not allegato Is Nothing Then

            Dim pathDownload As String = String.Empty

            If allegato.Id = 0 Then

                Dim temp = allegato.NomeFileTemp
                If IO.Path.GetExtension(allegato.NomeFile).ToLower = ".odt" Then
                    temp = temp.Remove(temp.Length - 4, 4) & ".pdf"
                End If

                pathDownload = percorsoRootTemp & temp & ".p7m"

            Else

                Dim filename As String = allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFileFirmato
                If percorsoRoot.EndsWith("\") Then
                    percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                End If
                pathDownload = percorsoRoot & allegato.PercorsoRelativo & filename

            End If

            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then

                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)

            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        Else
            'NIENTE
        End If
    End Sub

    'Effettua il download del file
    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

        Dim allegato As ParsecPro.Allegato = Me.Registrazione.Allegati.Where(Function(c) c.NomeFile = filename).FirstOrDefault
        If Not allegato Is Nothing Then
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If allegato.Id = 0 Then
                pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.NomeFileTemp
            Else
                percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                pathDownload = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
            End If
            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        End If
    End Sub

#End Region


    '**************************************************************************
    'DESTINATARIO INTERNO IN ARRIVO MITTENTE INTERNO IN USCITA - INTERNA
    '**************************************************************************
    Protected Sub SecondoReferentiInterniGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles SecondoReferentiInterniGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim referente As ParsecPro.IReferente = Nothing
            Select Case Me.Registrazione.TipologiaRegistrazione

                Case ParsecPro.TipoRegistrazione.Arrivo
                    Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
                    referente = Me.Registrazione.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault

                    Dim perConoscenzaCheckBox As CheckBox = CType(e.Item.FindControl("PerConoscenzaCheckBox"), CheckBox)
                    perConoscenzaCheckBox.Checked = referente.PerConoscenza


                    Dim iterCheckBox As CheckBox = CType(e.Item.FindControl("IterCheckBox"), CheckBox)
                    iterCheckBox.Checked = referente.Iter

                    Dim inviaEmailCheckBox As CheckBox = CType(e.Item.FindControl("InviaEmailCheckBox"), CheckBox)
                    inviaEmailCheckBox.Checked = referente.InviaEmail


                Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                    'Niente
            End Select
        End If
    End Sub


    '***************************************************************************************
    'MITTENTE ESTERNO-INTERNO IN ARRIVO  DESTINATARIO ESTERNO-INTERNO IN PARTENZA-INTERNA
    '***************************************************************************************
    Protected Sub ReferentiEsterniGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ReferentiEsterniGridView.ItemDataBound
        Dim referente As ParsecPro.IReferente = Nothing

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            Dim tipologiaRegistrazione = Me.Registrazione.TipologiaRegistrazione
            Select Case tipologiaRegistrazione
                Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna

                    Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
                    referente = Me.Registrazione.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault

                    Dim perConoscenzaCheckBox As CheckBox = CType(e.Item.FindControl("PerConoscenzaCheckBox"), CheckBox)
                    perConoscenzaCheckBox.Checked = referente.PerConoscenza

                    Dim iterCheckBox As CheckBox = CType(e.Item.FindControl("IterCheckBox"), CheckBox)
                    iterCheckBox.Checked = referente.Iter

                    Select Case tipologiaRegistrazione
                        Case TipoRegistrazione.Arrivo, TipoRegistrazione.Interna
                            iterCheckBox.Style.Add("display", If(referente.Interno, "block", "none"))
                        Case TipoRegistrazione.Partenza
                            iterCheckBox.Style.Add("display", "none")
                    End Select

                Case ParsecPro.TipoRegistrazione.Arrivo
                    'Niente
            End Select

        End If

    End Sub

End Class
