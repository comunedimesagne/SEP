Imports ParsecPro
Imports Telerik.Web.UI
Imports System.Data
Imports ParsecWKF
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.Web.Services
Imports System.Data.Objects
Imports Rebex.Net
Imports System.Net


'* SPDX-License-Identifier: GPL-3.0-only

Partial Class RegistrazioneArrivoPage
    Inherits System.Web.UI.Page

    '***********************************************************
    'NUOVO - GESTIONE MOVIMENTAZIONE
    '***********************************************************
    Public Enum TipoProcedura As Integer
        Nessuna = -1
        Ricerca = 0
        Nuovo = 1
        Modifica = 2
        Movimentazione = 3
        Fascicolazione = 4
        GestioneAllegati = 5
        Annullamento = 6
        Consultazione = 7

    End Enum
    '***********************************************************

    'Enum rappresentante la Modalità di utilizzo della Maschera
    Public Enum AbilitazioneUtente
        None = 0
        ModificaAmministrativa = 1
        Modifica = 2
        Movimentazione = 3
    End Enum

    'Enuma rappresentante la modalità di apertura della Pagina
    Public Enum ModalitaPopup
        None = 0
        Insert = 1    'Nuovo protocollo
        Update = 2    'Modifica protocollo
        Attachment = 3 'Protocollazione allegati
    End Enum

    'classe rappresentante informazioni aggiuntive sul protocollo
    Public Class ProtocolloInfo
        Public Property DescrizioneRegistrazione As String = String.Empty
        Public Property IdMittente As Nullable(Of Integer)
        Public Property IdDestinatario As Nullable(Of Integer)
    End Class

    Private WithEvents MainPage As Object
    Private Const ItemsPerRequest As Integer = 10

#Region "PROPRIETA'"

    'Variabile di sessione
    Public Property TipologiaProceduraApertura As TipoProcedura
        Set(ByVal value As TipoProcedura)
            Session("RegistrazionePage_TipologiaProceduraApertura") = value
        End Set
        Get
            Return CType(Session("RegistrazionePage_TipologiaProceduraApertura"), TipoProcedura)
        End Get
    End Property

    'Restituisice la data del protocollo.
    Protected Function DataImmissione(ByVal container As GridItem) As System.Nullable(Of DateTime)
        If container.OwnerTableView.GetColumn("DataImmissione").CurrentFilterValue = String.Empty Then
            Return New System.Nullable(Of DateTime)()
        Else
            Try
                'Siccome la funzione è Between prendo la prima data
                Return DateTime.Parse(container.OwnerTableView.GetColumn("DataImmissione").CurrentFilterValue.Split(" ")(0))
            Catch ex As Exception
                Return New System.Nullable(Of DateTime)()
            End Try

        End If
    End Function

    'Restituisce la Descrizione della tipologia di Registrazione.
    Protected Function DescrizioneTipologiaSelezionata(ByVal container As GridItem) As String
        If container.OwnerTableView.GetColumn("DescrizioneTipologiaRegistristrazione").CurrentFilterValue = String.Empty Then
            Return ""
        Else
            Try
                'Siccome la funzione è Between prendo la prima data
                Return container.OwnerTableView.GetColumn("DescrizioneTipologiaRegistristrazione").CurrentFilterValue
            Catch ex As Exception
                Return ""
            End Try

        End If
    End Function

    'Variabile di sessione: rappresenta il filtroper la ricerca.
    Public Property RegistrazioneFiltro() As ParsecPro.RegistrazioneFiltro
        Get
            Return CType(Session("RegistrazionePage_RegistrazioneFiltro"), ParsecPro.RegistrazioneFiltro)
        End Get
        Set(ByVal value As ParsecPro.RegistrazioneFiltro)
            Session("RegistrazionePage_RegistrazioneFiltro") = value
        End Set
    End Property

    'Variabile di sessione: Registrazione di Protocollo corrente.
    Public Property Registrazione() As ParsecPro.Registrazione
        Get
            Return CType(Session("RegistrazionePage_Registrazione"), ParsecPro.Registrazione)
        End Get
        Set(ByVal value As ParsecPro.Registrazione)
            Session("RegistrazionePage_Registrazione") = value
        End Set
    End Property

    'Variabile di sessione: elenco delle Registrazioni di protocollo associate alla griglia principale.
    Public Property Registrazioni() As List(Of ParsecPro.Registrazione)
        Get
            Return CType(Session("RegistrazionePage_Registrazioni"), List(Of ParsecPro.Registrazione))
        End Get
        Set(ByVal value As List(Of ParsecPro.Registrazione))
            Session("RegistrazionePage_Registrazioni") = value
        End Set
    End Property

    'Variabile di sessione: elenco dei Destinatari associati alla registrazione di Protocollo
    Public Property Destinatari() As List(Of ParsecPro.Destinatario)
        Get
            Return CType(Session("RegistrazionePage_Destinatari"), List(Of ParsecPro.Destinatario))
        End Get
        Set(ByVal value As List(Of ParsecPro.Destinatario))
            Session("RegistrazionePage_Destinatari") = value
        End Set
    End Property

    'Variabile di sessione:  elenco dei Mittenti associati alla registrazione di Protocollo
    Public Property Mittenti() As List(Of ParsecPro.Mittente)
        Get
            Return CType(Session("RegistrazionePage_Mittenti"), List(Of ParsecPro.Mittente))
        End Get
        Set(ByVal value As List(Of ParsecPro.Mittente))
            Session("RegistrazionePage_Mittenti") = value
        End Set
    End Property

    'Variabile si sessione:  elenco degli Allegati associati  della registrazione di Protocollo
    Public Property Allegati() As List(Of ParsecPro.Allegato)
        Get
            Return CType(Session("RegistrazionePage_Allegati"), List(Of ParsecPro.Allegato))
        End Get
        Set(ByVal value As List(Of ParsecPro.Allegato))
            Session("RegistrazionePage_Allegati") = value
        End Set
    End Property

    'Variabile di sessione: elenco dei collegamento associati alla Registrazione.
    Public Property Collegamenti() As List(Of ParsecPro.Collegamento)
        Get
            Return CType(Session("RegistrazionePage_Collegamenti"), List(Of ParsecPro.Collegamento))
        End Get
        Set(ByVal value As List(Of ParsecPro.Collegamento))
            Session("RegistrazionePage_Collegamenti") = value
        End Set
    End Property

    'Variabile di sessione: elenco dei Fascicoli associati alla Registrazione.
    Public Property Fascicoli() As List(Of ParsecAdmin.Fascicolo)
        Get
            Return Session("RegistrazionePage_Fascicoli")
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Fascicolo))
            Session("RegistrazionePage_Fascicoli") = value
        End Set
    End Property

    'Elenco di gruppi o di utenti abilitati a visualizzare una registrazione di protocollo.
    Public Property Visibilita() As List(Of ParsecAdmin.VisibilitaDocumento)
        Get
            Return Session("RegistrazionePage_Visibilita")
        End Get
        Set(ByVal value As List(Of ParsecAdmin.VisibilitaDocumento))
            Session("RegistrazionePage_Visibilita") = value
        End Set
    End Property

    'Variabile di sessione: tipo logia di registrazione di protocollo
    Public Property TipologiaRegistrazione As ParsecPro.TipoRegistrazione
        Get
            Return CType(ViewState("RegistrazionePage_TipologiaRegistrazione"), ParsecPro.TipoRegistrazione)
        End Get
        Set(ByVal value As ParsecPro.TipoRegistrazione)
            ViewState("RegistrazionePage_TipologiaRegistrazione") = value
        End Set
    End Property

    'Variabile di sessione: modalità della Registrazione del protocollo (Ordinaria, Emergenza)
    Public Property ModalitaRegistrazione As ParsecPro.ModalitaRegistrazione
        Get
            Return CType(ViewState("RegistrazionePage_ModalitaRegistrazione"), ParsecPro.ModalitaRegistrazione)
        End Get
        Set(ByVal value As ParsecPro.ModalitaRegistrazione)
            ViewState("RegistrazionePage_ModalitaRegistrazione") = value
        End Set
    End Property

    'Variabile di sessione: modalità di apertura del popup
    Public Property ModalitaAperturaPopup As ModalitaPopup
        Get
            Return CType(ViewState("RegistrazionePage_ModalitaAperturaPopup"), ModalitaPopup)
        End Get
        Set(ByVal value As ModalitaPopup)
            ViewState("RegistrazionePage_ModalitaAperturaPopup") = value
        End Set
    End Property


    'Variabile indicante se l'iter è attivato oppure no
    Public ReadOnly Property IterAttivato As Boolean
        Get
            Dim res As Boolean = False
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AttivaGestioneScrivaniePro", ParsecAdmin.TipoModulo.SEP)
            If Not parametro Is Nothing Then
                res = CBool(parametro.Valore)
            End If
            parametri.Dispose()

            If Not Page.Request("AvviaIter") Is Nothing Then
                If Page.Request("AvviaIter") = "0" Then
                    res = False
                End If
            End If

            Return res
        End Get
    End Property

    'variabile indicante se visuliazzare la colonna uffici
    Public ReadOnly Property VisualizzaColonnaUffici As Boolean
        Get
            Dim res As Boolean = True
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("VisualizzaColonnaUffici", ParsecAdmin.TipoModulo.PRO)
            If Not parametro Is Nothing Then
                res = CBool(parametro.Valore)
            End If
            parametri.Dispose()
            Return res
        End Get
    End Property

    'Variabile di sessione inerente la abilitazione dell'Utente (modifica, modificaamministrativa, ecc)
    Public Property AbilitazioneUtenteCorrente As AbilitazioneUtente
        Get
            Return CType(ViewState("RegistrazionePage_AbilitazioneUtenteCorrente"), AbilitazioneUtente)
        End Get
        Set(ByVal value As AbilitazioneUtente)
            ViewState("RegistrazionePage_AbilitazioneUtenteCorrente") = value
        End Set
    End Property

    'Variabile di sessione rappresentante la Fattura Elettronica
    Public Property FatturaElettronica As ParsecPro.FatturaElettronica
        Get
            Return CType(ViewState("RegistrazionePage_FatturaElettronica"), ParsecPro.FatturaElettronica)
        End Get
        Set(ByVal value As ParsecPro.FatturaElettronica)
            ViewState("RegistrazionePage_FatturaElettronica") = value
        End Set
    End Property


    'Variabile di sessione indicante il TaskAttivo dell'iter.
    Public Property TaskAttivo() As ParsecWKF.TaskAttivo
        Get
            Return Session("RegistrazionePage_TaskAttivo")
        End Get
        Set(ByVal value As ParsecWKF.TaskAttivo)
            Session("RegistrazionePage_TaskAttivo") = value
        End Set
    End Property

    'Variabile di sessione: lista dei destinatari bloccati.
    Public Property DestinatariBloccati() As List(Of ParsecPro.Destinatario)
        Get
            Return CType(Session("RegistrazionePage_DestinatariBloccati"), List(Of ParsecPro.Destinatario))
        End Get
        Set(ByVal value As List(Of ParsecPro.Destinatario))
            Session("RegistrazionePage_DestinatariBloccati") = value
        End Set
    End Property

    'Variabile di sessione: destinatario del Task corrente dell'Iter.
    Public Property DestinatarioTaskCorrente() As ParsecPro.Destinatario
        Get
            Return CType(Session("RegistrazionePage_DestinatarioTaskCorrente"), ParsecPro.Destinatario)
        End Get
        Set(ByVal value As ParsecPro.Destinatario)
            Session("RegistrazionePage_DestinatarioTaskCorrente") = value
        End Set
    End Property

    'Variabile di sessione: modulo del Sep
    Public Property Modulo As Integer
        Get
            Return CType(Session("RegistrazioneArrivoPage_Modulo"), Integer)
        End Get
        Set(value As Integer)
            Session("RegistrazioneArrivoPage_Modulo") = value
        End Set
    End Property

    'Variabile di sessione: disabilitazione  dell'iter.
    Public Property DisabilitaIter As Boolean
        Get
            Return CType(Session("RegistrazioneArrivoPage_DisabilitaIter"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Session("RegistrazioneArrivoPage_DisabilitaIter") = value
        End Set
    End Property

    'Variabile di sessione indcante l'id della mail
    Public Property IdEmail As Nullable(Of Integer)
        Get
            Return CType(Session("RegistrazioneArrivoPage_IdEmail"), Nullable(Of Integer))
        End Get
        Set(value As Nullable(Of Integer))
            Session("RegistrazioneArrivoPage_IdEmail") = value
        End Set
    End Property

    'Variabile di sessione indicante se visulizzare a poieno schermo o meno la pagina.
    Private Property FullSize As Boolean
        Set(value As Boolean)
            ViewState("FullSize") = value
        End Set
        Get
            Return ViewState("FullSize")
        End Get
    End Property


#End Region

#Region "EVENTI PAGINA"

    'Carica le Tiplogie di Ricezione
    Private Sub CaricaTipologieRicezioneInvio()
        Dim tipiRicezione As New ParsecPro.TipiRicezioneInvioRepository
        Me.TipoRicezioneInvioComboBox.DataSource = tipiRicezione.GetView(Nothing)
        Me.TipoRicezioneInvioComboBox.DataTextField = "Descrizione"
        Me.TipoRicezioneInvioComboBox.DataValueField = "Id"
        Me.TipoRicezioneInvioComboBox.DataBind()
        Me.TipoRicezioneInvioComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipoRicezioneInvioComboBox.SelectedIndex = 0
        tipiRicezione.Dispose()
    End Sub

    'disabilita alcuni controlli della UI
    Private Sub DisabilitaUI(ByVal tipo As TipoProcedura)

        'DISABILITO I PUSANTI
        Me.RadToolBar.FindItemByText("Nuovo").Enabled = False
        Me.RadToolBar.FindItemByText("Nuovo").ToolTip = ""

        If tipo <> TipoProcedura.Annullamento Then
            Me.RadToolBar.FindItemByText("Elimina").Enabled = False
            Me.RadToolBar.FindItemByText("Elimina").ToolTip = ""
        Else
            Me.RadToolBar.FindItemByText("Salva").Enabled = False
            Me.RadToolBar.FindItemByText("Salva").ToolTip = ""
            Me.RadToolBar.FindItemByText("Elimina").Enabled = True
        End If

        If tipo = TipoProcedura.Consultazione Then
            Me.RadToolBar.FindItemByText("Salva").Enabled = False
            Me.RadToolBar.FindItemByText("Salva").ToolTip = ""
            Me.ProtocolliGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False


            Me.RadToolBar.FindItemByText("Invia E-mail").Enabled = False
            Me.RadToolBar.FindItemByText("Invia E-mail").ToolTip = ""
            Me.VisualizzaStoricoRegistrazioneImageButton.Visible = False

            Try

                Me.DatiProtocolloTabStrip.Tabs(2).Enabled = False
                Me.DatiProtocolloTabStrip.Tabs(3).Enabled = False
                Me.DatiProtocolloTabStrip.Tabs(4).Enabled = False
                Me.DatiProtocolloTabStrip.Tabs(5).Enabled = False
                Me.DatiProtocolloTabStrip.Tabs(6).Enabled = False

            Catch ex As Exception
                'NIENTE
            End Try

        End If


        Me.RadToolBar.FindItemByText("Annulla").Enabled = False
        Me.RadToolBar.FindItemByText("Annulla").ToolTip = ""

        ImpostaAbilitazioneUi(False)

        Select Case tipo
            Case TipoProcedura.Movimentazione
                'AbilitaMovimento
                Dim sezionato = Not Me.Registrazione Is Nothing
                Me.FiltroSecondoReferenteInternoTextBox.Visible = sezionato
                Me.TrovaSecondoReferenteInternoImageButton.Visible = sezionato
                Me.SecondoReferentiInterniGridView.Enabled = sezionato
                Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = sezionato

                Me.ProtocolliGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False

                Me.NoteInterneTextBox.Enabled = sezionato

                If Not sezionato Then
                    Me.SecondoReferentiInterniGridView.DataSource = New List(Of ParsecPro.Mittente)
                    Me.SecondoReferentiInterniGridView.DataBind()
                End If
            Case TipoProcedura.Fascicolazione
                '******************************************************************************************
                'Scheda Fascicoli
                '******************************************************************************************
                Me.NuovoFascicoloImageButton.Visible = True
                Me.TrovaFascicoloImageButton.Visible = True
                Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = True
                Me.FascicoliGridView.Enabled = True
                Me.FaseDocumentoFascicoloComboBox.Visible = True
                Me.FaseDocumentoFascicoloLabel.Visible = True
                Me.ProtocolliGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False

            Case TipoProcedura.GestioneAllegati

                Dim selezionato = Not Me.Registrazione Is Nothing
                '******************************************************************************************
                'Scheda Documenti
                '******************************************************************************************

                Me.ScansionaImageButton.Visible = selezionato
                Me.AggiungiDocumentoImageButton.Visible = selezionato
                Me.DocumentiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = selezionato
                Me.DescrizioneDocumentoTextBox.Enabled = selezionato
                Me.NumeroDocumentiTextBox.Enabled = selezionato
                Me.DocumentoPrimarioRadioButton.Enabled = selezionato
                Me.DocumentoAllegatoRadioButton.Enabled = selezionato
                Me.AllegatoUpload.Enabled = selezionato
                Me.FronteRetroCheckBox.Enabled = selezionato
                Me.VisualizzaUICheckBox.Enabled = selezionato
                Me.ProtocolliGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False

            Case TipoProcedura.Annullamento
                Me.NoteTextBox.Enabled = True

        End Select

    End Sub


    'Metodo Init della Page: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.chiudiImageButton.Attributes.Add("onclick", "window.close();return false;")
        Me.AggiornaFirmaDigitaleImageButton.Attributes.Add("onclick", "showUI=true;")

        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False

        Me.OggettoTextBox.Attributes.Add("onblur", "this.style.borderColor=''")
        Me.OggettoTextBox.Attributes.Add("onmouseout", "this.style.borderColor=''")
        Me.OggettoTextBox.Attributes.Add("onfocus", "this.style.borderColor='#305090'")
        Me.OggettoTextBox.Attributes.Add("onmouseover", "this.style.borderColor='#305090'")

        Me.ModalitaAperturaPopup = ModalitaPopup.None
        If Page.Request("Mode") Is Nothing Then
            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Protocollo"
            Me.MainPage.DescrizioneProcedura = "> Registrazione"
        Else
            Me.MainPage = CType(Me.Master, BasePage)
            CType(Me.Master, BasePage).DescrizioneProcedura = "Dettaglio Protocollo"
            CType(Me.Master, BasePage).ImpostaLarghezzaHeader(900)
        End If

        If Not Me.Page.IsPostBack Then

            Me.VisualizzaEmailControl.Visible = False

            '****************************************************************************************************
            'NUOVO - GESTIONE MOVIMENTAZIONE
            '*********************************************************************************
            Me.TipologiaProceduraApertura = TipoProcedura.Nuovo
            If Not Request.QueryString("Procedura") Is Nothing Then
                Me.TipologiaProceduraApertura = CInt(Request.QueryString("Procedura"))
            End If
            '****************************************************************************************************

            CacheRubrica = Nothing

            Me.Registrazione = Nothing
            Me.Registrazioni = Nothing
            Me.RegistrazioneFiltro = Nothing
            Me.RipristinaFiltroInizialeImageButton.Enabled = False
            Me.AbilitazioneUtenteCorrente = AbilitazioneUtente.None

            Me.CaricaFasi()

            Me.CaricaTipologieRicezioneInvio()

            Me.DestinatariBloccati = New List(Of ParsecPro.Destinatario)
            Me.TaskAttivo = Nothing
            Me.DestinatarioTaskCorrente = Nothing

            Me.DisabilitaIter = False
            Me.IdEmail = Nothing
            Me.FullSize = False

        End If

        If Not Page.Request("E") Is Nothing Then
            Me.ModalitaRegistrazione = ParsecPro.ModalitaRegistrazione.Emergenza

            Dim sessioni As New ParsecPro.SessioniEmergenzaRepository
            Me.SessioniEmergenzaComboBox.DataSource = sessioni.GetView(Nothing)
            Me.SessioniEmergenzaComboBox.DataTextField = "Nome"
            Me.SessioniEmergenzaComboBox.DataValueField = "Id"
            Me.SessioniEmergenzaComboBox.DataBind()
            sessioni.Dispose()
            Me.SessioniEmergenzaComboBox.SelectedIndex = 0

        Else
            Me.ModalitaRegistrazione = ParsecPro.ModalitaRegistrazione.Ordinaria

            Dim sessioni As New ParsecPro.SessioniEmergenzaRepository
            Me.SessioniEmergenzaComboBox.DataSource = sessioni.GetSessioni
            Me.SessioniEmergenzaComboBox.DataTextField = "Nome"
            Me.SessioniEmergenzaComboBox.DataValueField = "Id"
            Me.SessioniEmergenzaComboBox.DataBind()
            sessioni.Dispose()
            Me.SessioniEmergenzaComboBox.SelectedIndex = 0

        End If

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

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraScansione()
            Me.RegistraParsecDigitalSign()
        End If

        If Me.IterAttivato Then
            If Not Me.Page.IsPostBack Then
                Dim modelli As New ParsecWKF.ModelliRepository
                Me.TipologiaIterComboBox.Visible = True
                Me.TipologiaIterLabel.Visible = True
                Me.TipologiaIterComboBox.DataSource = modelli.GetView(Nothing)
                Me.TipologiaIterComboBox.DataTextField = "Descrizione"
                Me.TipologiaIterComboBox.DataValueField = "Id"
                Me.TipologiaIterComboBox.DataBind()

                Me.TipologiaIterComboBox.SelectedIndex = 0
                modelli.Dispose()

            End If
        Else
            Me.TipologiaIterComboBox.Visible = False
            Me.TipologiaIterLabel.Visible = False
        End If

        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        ParsecUtility.Utility.CloseWindow(False)

        '***************************************************************************

        Me.DisabilitaPulsantePredefinito.Attributes.Add("onclick", "return false;")

        Me.FiltroSecondoReferenteInternoTextBox.Attributes.Add("onkeydown", "javascript:if (event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" & Me.TrovaSecondoReferenteInternoImageButton.ClientID & "').click();}};")


        Me.ProtocolliGridView.MasterTableView.Columns.FindByUniqueName("ElencoReferentiInterni").Visible = Me.VisualizzaColonnaUffici
        If Not Me.VisualizzaColonnaUffici Then
            Me.ProtocolliGridView.MasterTableView.Columns.FindByUniqueName("Oggetto").ItemStyle.Width = New Unit(360)
            Me.ProtocolliGridView.MasterTableView.Columns.FindByUniqueName("Oggetto").HeaderStyle.Width = New Unit(360)

        End If

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.NotePanel.Style.Add("width", widthStyle)
        Me.ClassificazionePanel.Style.Add("width", widthStyle)
        Me.AllegatiPanel.Style.Add("width", widthStyle)
        Me.GrigliaAllegatiPanel.Style.Add("width", widthStyle)
        Me.DocumentiGridView.Style.Add("width", widthStyle)

        Me.CollegamentiPanel.Style.Add("width", widthStyle)
        Me.GrigliaCollegamentiPanel.Style.Add("width", widthStyle)

        Me.VisibilitaPanel.Style.Add("width", widthStyle)
        Me.DatiDocumentoPanel.Style.Add("width", widthStyle)
        Me.DatiProtocolloMultiPage.Style.Add("width", widthStyle)

        Me.CollegamentiDirettiGridView.Style.Add("width", widthStyle)
        Me.CollegamentiIndirettiGridView.Style.Add("width", widthStyle)

        Me.CollegamentiDirettiPanel.Style.Add("width", widthStyle)
        Me.CollegamentiIndirettiPanel.Style.Add("width", widthStyle)

        Me.VisibilitaGridView.Style.Add("width", widthStyle)
        Me.FascicoliGridView.Style.Add("width", widthStyle)
        Me.ProtocolliGridView.Style.Add("width", widthStyle)
        Me.ReferentiEsterniGridView.Style.Add("width", widthStyle)
        Me.SecondoReferentiInterniGridView.Style.Add("width", widthStyle)

        Me.GrigliaFascicoliPanel.Style.Add("width", widthStyle)
        Me.FascicoliPanel.Style.Add("width", widthStyle)

        Me.FullSizeProtocolliGridView.Style.Add("width", widthStyle)

    End Sub

    'Metodo PreInit della Page: associa la master page in base alla modalità con la quale viene aperta.
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BasePage.master"
        End If
    End Sub

    'Evento click per attivare la visualizzazione dello storico
    Protected Sub VisualizzaStoricoRegistrazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaStoricoRegistrazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Protocollo/pages/search/StoricoRegistrazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("r", Me.Registrazione.Id)
        ParsecUtility.Utility.ShowPopup(pageUrl, 950, 600, queryString, False)
    End Sub

    'Metodo che consente la selezione di un Modello tramite il parametro configurato nel sistema.
    Private Function SelezionaModelloDaParametro() As ParsecWKF.Modello
        '***************************************************************************************************************
        'SE STO PROTOCOLLANDO UNA FATTURA ELETTRONICA
        '***************************************************************************************************************
        Dim nomeParametro As String = String.Empty

        If Not Page.Request("Fattura") Is Nothing Then
            nomeParametro = "IdModelloWorkflowFatturaElettronica"
        End If

        If Page.Request("Fattura") Is Nothing AndAlso Page.Request("IstanzaOnline") Is Nothing AndAlso Page.Request("IstanzaImpresaInUnGiorno") Is Nothing Then
            Select Case Me.TipologiaRegistrazione
                Case TipoRegistrazione.Arrivo
                    nomeParametro = "IdModelloWorkflowNotifica"
                Case TipoRegistrazione.Partenza
                    nomeParametro = "IdModelloWorkflowNotificaInterna"
                Case TipoRegistrazione.Interna
                    nomeParametro = "IdModelloWorkflowNotificaInterna"
            End Select
        End If

        If Not String.IsNullOrEmpty(nomeParametro) Then
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName(nomeParametro, ParsecAdmin.TipoModulo.PRO)
            parametri.Dispose()
            If Not parametro Is Nothing Then
                Dim modelli As New ParsecWKF.ModelliRepository
                Dim modello = modelli.Where(Function(c) c.Id = CInt(parametro.Valore)).FirstOrDefault
                modelli.Dispose()
                Return modello
            End If
        Else
            Dim modelli As New ParsecWKF.ModelliRepository
            Dim modello As ParsecWKF.Modello = Nothing

            'SE ESEGUO UN DOPPIO SALVATAGGIO GENERA UN'ECCEZIONE

            If Not Me.Request.QueryString("IterProcedimento") Is Nothing Then
                Dim iterProcedimento As String = Me.Request.QueryString("IterProcedimento")
                modello = modelli.Where(Function(c) c.NomeFile = iterProcedimento).FirstOrDefault
            End If

            modelli.Dispose()
            Return modello
        End If

        Return Nothing
    End Function

    'Metodo che seleziona la tipologia di Iter
    Private Sub SelezionaModelloIter()
        If Me.IterAttivato Then
            Dim modello = Me.SelezionaModelloDaParametro
            If Not modello Is Nothing Then
                Me.TipologiaIterComboBox.Items.FindItemByValue(modello.Id).Selected = True
            Else
                Me.TipologiaIterComboBox.SelectedIndex = 0
            End If
        End If
    End Sub

    'Metodo che seleziona il Modello di Iter per le Fatture Elettroniche (legge ilparametro "IdModelloWorkflowFatturaElettronica")
    Private Sub SelezionaModelloIterFattura()
        If Not Page.Request("Fattura") Is Nothing Then
            Dim nomeParametro As String = "IdModelloWorkflowFatturaElettronica"
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName(nomeParametro, ParsecAdmin.TipoModulo.PRO)
            parametri.Dispose()
            If Not parametro Is Nothing Then
                Dim modelli As New ParsecWKF.ModelliRepository
                Dim modello = modelli.Where(Function(c) c.Id = CInt(parametro.Valore)).FirstOrDefault
                modelli.Dispose()
                If Not modello Is Nothing Then
                    Me.TipologiaIterComboBox.Items.FindItemByValue(modello.Id).Selected = True
                End If
            End If
        End If
    End Sub

    'Metodo LoadComplete della Page: gestisce i Messaggi di Eliminazione, i titoli delle griglie e abilitione/disabilitazione di vari campi. Nel caso di nuova regsitrazine aggiunge l'utente collegato agli autenti che hanno visibilità alla registrazione in lavorazione.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        '********************************************************************************************************************
        'Gestione abilitazione interfaccia
        '********************************************************************************************************************
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Page.Request("Mode") Is Nothing Then
            Dim eliminaAbilitato = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AnnullamentoRegistrazione)).FirstOrDefault Is Nothing
            Me.RadToolBar.Items.FindItemByText("Elimina").Enabled = eliminaAbilitato AndAlso If(Not Me.Registrazione Is Nothing, Not Me.Registrazione.Annullato, True)
            ParsecUtility.Utility.Confirm("Annullare la registrazione selezionata?", False, Not Me.Registrazione Is Nothing AndAlso eliminaAbilitato AndAlso Not Me.Registrazione.Annullato)
        Else
            If Me.TipologiaProceduraApertura = TipoProcedura.Annullamento Then
                Dim eliminaAbilitato = True
                ParsecUtility.Utility.Confirm("Annullare la registrazione selezionata?", False, Not Me.Registrazione Is Nothing AndAlso eliminaAbilitato AndAlso Not Me.Registrazione.Annullato)
            End If
        End If

        Dim abilitaStampigliatrice As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AbilitaStampigliatrice)).FirstOrDefault Is Nothing

        If abilitaStampigliatrice Then
            'UTILIZZO L'APPLET
            If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                Me.RegistraParsecPrinting()
            End If
        End If

        Dim arrivoAbilitato As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.RegistrazioneArrivo)).FirstOrDefault Is Nothing
        Dim partenzaAbilitato As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.RegistrazionePartenza)).FirstOrDefault Is Nothing
        Dim internoAbilitato As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.RegistrazioneInterna)).FirstOrDefault Is Nothing

        Dim salvaAbilitato = True

        '********************************************************************************************************************

        Dim tipoRegistrazionePredefinita As Integer = CInt(Request.QueryString("Tipo"))

        'Abilito solo la tipologia passata 
        If Not Page.Request("Mode") Is Nothing Then
            For i As Integer = 0 To 2
                If tipoRegistrazionePredefinita <> i Then
                    Me.TipoRegistrazioneRadioList.Items(i).Enabled = False
                End If
            Next
        End If

        If Not IsPostBack AndAlso Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then

            Me.TipoRegistrazioneRadioList.SelectedIndex = tipoRegistrazionePredefinita

            Me.TipologiaRegistrazione = If(Me.TipoRegistrazioneRadioList.Items(tipoRegistrazionePredefinita).Enabled, CType(tipoRegistrazionePredefinita, ParsecPro.TipoRegistrazione), ParsecPro.TipoRegistrazione.Nessuna)

            If Not Me.TipoRegistrazioneRadioList.Items(tipoRegistrazionePredefinita).Enabled Then
                For i As Integer = 0 To 2
                    If Me.TipoRegistrazioneRadioList.Items(i).Enabled Then
                        Me.TipologiaRegistrazione = CType(i, ParsecPro.TipoRegistrazione)
                        Me.TipoRegistrazioneRadioList.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            Me.PannelloSessioneEmergenza.Visible = (Me.ModalitaRegistrazione = ParsecPro.ModalitaRegistrazione.Emergenza)

            Me.RadToolBar.Items.FindItemByText("Invia E-mail").Enabled = False

          
            Me.Allegati = New List(Of ParsecPro.Allegato)
            Me.Mittenti = New ParsecPro.Mittenti
            Me.Destinatari = New ParsecPro.Destinatari
            Me.Collegamenti = New List(Of ParsecPro.Collegamento)
            Me.Fascicoli = New List(Of ParsecAdmin.Fascicolo)
            Me.Visibilita = New List(Of ParsecAdmin.VisibilitaDocumento)
           
            Me.GetParametri()

            '************************************************************************************************************************
            'AGGIUNGO L'UTENTE CORRENTE ALLA VISIBILITA
            '************************************************************************************************************************
            Me.AggiungiUtenteCollegatoVisibilita(utenteCollegato)
            '************************************************************************************************************************

            'SE NON E' UNA FATTURA E NON E' UNA SEGNALAZIONE DI ILLECITO
            If Page.Request("Fattura") Is Nothing AndAlso Page.Request("Segnalazione") Is Nothing Then
                'SE E' UNA NUOVA REGISTRAZIONE
                If Me.Registrazione Is Nothing Then
                    Me.CaricaReferenteDefault(Me.TipologiaRegistrazione)
                End If
            End If


        End If

        If Me.Registrazione Is Nothing Then
            Select Case CType(Me.TipoRegistrazioneRadioList.SelectedIndex, ParsecPro.TipoRegistrazione)
                Case TipoRegistrazione.Arrivo
                    salvaAbilitato = arrivoAbilitato
                Case TipoRegistrazione.Partenza
                    salvaAbilitato = partenzaAbilitato
                Case TipoRegistrazione.Interna
                    salvaAbilitato = internoAbilitato
            End Select

        End If

        'SE QUESTA PAGINA NON E' RICHIAMATA DA UN POPUP ????? DECIDERE SE IN ITER CONTANO LE ABILITAZIONI O NO
        'If Page.Request("Mode") Is Nothing Then
        If Me.Registrazione Is Nothing Then
            Me.RadToolBar.Items.FindItemByText("Salva").Enabled = salvaAbilitato
        End If
        'End If


        '***************************************************************************************************************
        'SE STO PROTOCOLLANDO UNA FATTURA ELETTRONICA
        '***************************************************************************************************************
        If Not Page.Request("Fattura") Is Nothing Then
            Me.SelezionaModelloIterFattura()
        End If

        If Not Me.Page.IsPostBack Then
            If Page.Request("Fattura") Is Nothing AndAlso Page.Request("IstanzaOnline") Is Nothing Then
                Me.SelezionaModelloIter()
            End If
        End If

        'Carico i referenti associati al protocollo selezionato
        Select Case Me.TipologiaRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                Me.ReferentiEsterniGridView.DataSource = Mittenti.ToList
                Me.SecondoReferentiInterniGridView.DataSource = Destinatari.Where(Function(c) c.Interno = True).ToList '.Interni
            Case ParsecPro.TipoRegistrazione.Partenza
                Me.ReferentiEsterniGridView.DataSource = Destinatari.ToList
                Me.SecondoReferentiInterniGridView.DataSource = Mittenti.Where(Function(c) c.Interno = True).ToList '.Interni
            Case TipoRegistrazione.Interna
                Me.ReferentiEsterniGridView.DataSource = Destinatari.ToList  'interni
                Me.SecondoReferentiInterniGridView.DataSource = Mittenti.Where(Function(c) c.Interno = True).ToList '.Interni
        End Select

        Me.ReferentiEsterniGridView.DataBind()
        Me.SecondoReferentiInterniGridView.DataBind()

        Me.DocumentiGridView.DataSource = Me.Allegati
        Me.DocumentiGridView.DataBind()


        Me.CollegamentiDirettiGridView.DataSource = Me.Collegamenti.Where(Function(c) c.Diretto = True).ToList
        Me.CollegamentiDirettiGridView.DataBind()

        Me.CollegamentiIndirettiGridView.DataSource = Me.Collegamenti.Where(Function(c) c.Diretto = False).ToList
        Me.CollegamentiIndirettiGridView.DataBind()

        Me.FascicoliGridView.DataSource = Me.Fascicoli
        Me.FascicoliGridView.DataBind()


        Me.VisibilitaGridView.DataSource = Me.Visibilita
        Me.VisibilitaGridView.DataBind()

        Me.ImpostaUiPagina(Me.TipologiaRegistrazione)
        Me.TipologiaIterComboBox.Enabled = (Me.TipologiaRegistrazione = TipoRegistrazione.Arrivo And Me.Registrazione Is Nothing) AndAlso Not Page.Request("Mode") Is Nothing


        If Not Page.Request("Mode") Is Nothing Then
            Me.TipologiaIterComboBox.Enabled = False
        End If
        Dim allegato As ParsecPro.Allegato = Me.Allegati.Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
        Me.DocumentoAllegatoRadioButton.Checked = Not allegato Is Nothing
        Me.DocumentoPrimarioRadioButton.Checked = allegato Is Nothing
        Me.DocumentoPrimarioRadioButton.Enabled = allegato Is Nothing

        If Me.TipoRegistrazioneRadioList.Items(0).Enabled Then
            Me.TipoRegistrazioneRadioList.Items(0).Enabled = arrivoAbilitato
        End If
        If Me.TipoRegistrazioneRadioList.Items(1).Enabled Then
            Me.TipoRegistrazioneRadioList.Items(1).Enabled = partenzaAbilitato
        End If
        If Me.TipoRegistrazioneRadioList.Items(2).Enabled Then
            Me.TipoRegistrazioneRadioList.Items(2).Enabled = internoAbilitato
        End If


        If Me.TipologiaProceduraApertura <> TipoProcedura.Consultazione Then
            Me.DatiProtocolloTabStrip.Tabs(2).Text = "Allegati" & If(Me.Allegati.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Allegati.Count.ToString & ")</span>", "<span style='width:20px'></span>")
            Me.DatiProtocolloTabStrip.Tabs(3).Text = "Collegamenti" & If(Me.Collegamenti.Where(Function(c) c.Diretto = True).Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Collegamenti.Where(Function(c) c.Diretto = True).Count.ToString & ")</span>", "<span style='width:20px'></span>")
            Me.DatiProtocolloTabStrip.Tabs(4).Text = "Fascicoli" & If(Me.Fascicoli.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Fascicoli.Count.ToString & ")</span>", "<span style='width:20px'></span>")
            Me.DatiProtocolloTabStrip.Tabs(5).Text = "Visibilità" & If(Me.Visibilita.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Visibilita.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        Else
            Me.DatiProtocolloTabStrip.Tabs(2).Text = "Allegati"
            Me.DatiProtocolloTabStrip.Tabs(3).Text = "Collegamenti"
            Me.DatiProtocolloTabStrip.Tabs(4).Text = "Fascicoli"
            Me.DatiProtocolloTabStrip.Tabs(5).Text = "Visibilità"
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

        If Me.Registrazione Is Nothing Then
            Me.AreaInfoLabel.Text = "<font color='#00156E'>Nuovo Protocollo - " & "</font><font color='" & color & "'>" & descrizione & "</font>"
        Else
            Me.AreaInfoLabel.Text = "<font color='#00156E'>Protocollo N° " & Registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy}", Registrazione.DataImmissione) & " - " & "</font><font color='" & color & "'>" & descrizione & "</font>"
        End If

        '********************************************************************************
        'NUOVO - GESTIONE MOVIMENTAZIONE
        '********************************************************************************
        Select Case Me.TipologiaProceduraApertura
            Case TipoProcedura.Movimentazione, TipoProcedura.Fascicolazione, TipoProcedura.GestioneAllegati, TipoProcedura.Annullamento, TipoProcedura.Consultazione
                Me.DisabilitaUI(Me.TipologiaProceduraApertura)
            Case Else
                Dim interna As Boolean = (Me.TipologiaRegistrazione = TipoRegistrazione.Interna)
                Me.RubricaComboBox.Visible = Not interna
                Me.AggiungiReferenteEsternoImageButton.Visible = Not interna
                Me.TrovaReferenteEsternoImageButton.Visible = Not interna
                Me.AggiungiNuovoReferenteEsternoImageButton.Visible = Not interna
                Me.TrovaReferenteEsternoIpaImageButton.Visible = Not interna
        End Select
        '********************************************************************************

        Me.RadToolBar.Items.FindItemByText("Elimina").Attributes.Add("onclick", "return Confirm();")

    End Sub

#End Region

#Region "GESTIONE APERTURA POPUP"

    'Aggiorna la Visibilità dalla Registrazione del Protocollo
    Private Sub AggiornaVisibilitaDaRegistrazione(registrazione As ParsecPro.Registrazione)

        Dim strutture As New ParsecPro.StrutturaViewRepository

        For Each mittenteInterno As ParsecPro.Mittente In registrazione.Mittenti.Where(Function(c) c.Interno = True).ToList

            Dim mitt = mittenteInterno
            Dim referente = strutture.Where(Function(c) c.Id = mitt.Id).FirstOrDefault

            If Not referente Is Nothing Then
                'AGGIUNGO L'UTENTE O IL GRUPPO DI VISIBILITA'
                If referente.IdGerarchia = 400 Then  'PERSONA
                    If referente.IdUtente.HasValue Then
                        Dim utenti As New ParsecAdmin.UserRepository
                        Dim utente As ParsecAdmin.Utente = utenti.GetUserById(referente.IdUtente).FirstOrDefault
                        If Not utente Is Nothing Then
                            Me.AggiungiUtenteVisibilita(utente)
                        End If
                    End If
                Else
                    Me.AggiungiGruppoDefault(referente.Id)
                End If
                '**************************************************************************************************
            End If
        Next

        For Each destinatarioInterno As ParsecPro.Destinatario In registrazione.Destinatari.Where(Function(c) c.Interno = True).ToList
            Dim dest = destinatarioInterno
            Dim referente = strutture.Where(Function(c) c.Id = dest.Id).FirstOrDefault
            If Not referente Is Nothing Then
                'AGGIUNGO L'UTENTE O IL GRUPPO DI VISIBILITA'
                If referente.IdGerarchia = 400 Then  'PERSONA
                    If referente.IdUtente.HasValue Then
                        Dim utenti As New ParsecAdmin.UserRepository
                        Dim utente As ParsecAdmin.Utente = utenti.GetUserById(referente.IdUtente).FirstOrDefault
                        If Not utente Is Nothing Then
                            Me.AggiungiUtenteVisibilita(utente)
                        End If
                    End If
                Else
                    Me.AggiungiGruppoDefault(referente.Id)
                End If
                '**************************************************************************************************
            End If
        Next

    End Sub

    'Recupero i parametri in base al contenuto di ParsecUtility.SessionManager.ParametriPagina
    Private Sub GetParametri()
        Dim registrazione As ParsecPro.Registrazione = Nothing

        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("RegistrazioneEmail") Then
                registrazione = parametriPagina("RegistrazioneEmail")
            End If


            If parametriPagina.ContainsKey("RegistrazioneInIter") Then
                registrazione = parametriPagina("RegistrazioneInIter")
                If parametriPagina.ContainsKey("Modulo") Then Modulo = parametriPagina("Modulo")
                'todo verificare 01/09
                If Modulo = 5 Then
                    If registrazione.Fascicoli.Count > 0 Then
                        Me.Fascicoli = registrazione.Fascicoli
                        Me.AggiornaGrigliaFascicoli()
                    End If
                End If

                Me.TaskAttivo = parametriPagina("TaskAttivo")

                Dim istanze As New ParsecWKF.IstanzaRepository
                Dim idDestinatarioTaskCorrente As Integer = 0

                If Not Me.TaskAttivo Is Nothing Then
                    idDestinatarioTaskCorrente = istanze.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault.Ufficio
                End If

                istanze.Dispose()

                Dim destinatariInterniIter = registrazione.Destinatari.Where(Function(c) c.Interno = True And c.Iter = True).ToList
                For Each dest In destinatariInterniIter

                    Dim idReferenteInterno As Integer = dest.Id

                    Dim strutture As New ParsecAdmin.StructureRepository
                    Dim struttura = strutture.GetQuery.Where(Function(c) c.Id = idReferenteInterno).FirstOrDefault
                    strutture.Dispose()

                    Dim IdDestinatario As Nullable(Of Integer) = Nothing

                    ' SETTORE 
                    If Not struttura.IDUtente.HasValue Then
                        IdDestinatario = Me.ResponsabileArea(idReferenteInterno, True)
                        'SERVIZIO
                        If Not IdDestinatario.HasValue Then
                            IdDestinatario = Me.ResponsabileArea(idReferenteInterno, False)
                        End If
                    Else
                        If struttura.IDUtente < 1 Then
                            IdDestinatario = Me.ResponsabileArea(idReferenteInterno, True)
                            'SERVIZIO
                            If Not IdDestinatario.HasValue Then
                                IdDestinatario = Me.ResponsabileArea(idReferenteInterno, False)
                            End If
                        Else
                            ' PERSONA
                            IdDestinatario = struttura.IDUtente
                        End If
                    End If

                    If Not Me.TaskAttivo Is Nothing Then
                        If IdDestinatario <> idDestinatarioTaskCorrente Then
                            Me.DestinatariBloccati.Add(dest)
                        Else
                            Me.DestinatarioTaskCorrente = dest
                        End If
                    End If

                Next

            End If

            If parametriPagina.ContainsKey("FatturaElettronica") Then
                Me.FatturaElettronica = parametriPagina("FatturaElettronica")
            End If

            'GESTIONE REGISTRAZIONE INTERNA (PROTOCOLLAZIONE DOCUMENTO DI RISPOSTA DA ITER) 

            If parametriPagina.ContainsKey("DisabilitaIter") Then
                Me.DisabilitaIter = parametriPagina("DisabilitaIter")
            End If

            If Not registrazione Is Nothing Then


                'Se sto aprendo la finestra come popup
                If Not Page.Request("Mode") Is Nothing Then

                    'Nascondo la lista dei protocolli.
                    Me.ProtocolliPanel.Style.Add("display", "none")
                    Me.ProtocolliGridView.Visible = False

                    'Disabilito tutti i pulsanti della toolbar
                    For i As Integer = 0 To Me.RadToolBar.Items.Count - 1
                        Me.RadToolBar.Items(i).Enabled = False

                    Next

                    Me.RadToolBar.Items.FindItemByText("Stampa").Enabled = True

                    If Me.TipologiaProceduraApertura <> TipoProcedura.Annullamento Then
                        Me.RadToolBar.Items.FindItemByText("Elimina").Attributes.Remove("onclick")
                    End If

                    'Abilito solo il pulsante salva e salva e chiudi
                    Me.RadToolBar.Items.FindItemByText("Salva").Enabled = True

                    Me.ChiudiButton.Visible = True

                    Select Case Me.Page.Request("Mode")

                        Case "Update"
                            Me.AggiornaVistaDaModello(registrazione, True)
                            Me.ModalitaAperturaPopup = ModalitaPopup.Update
                            Me.IdEmail = registrazione.IdEmail

                        Case "Insert"
                            Me.ModalitaAperturaPopup = ModalitaPopup.Insert
                            Me.OggettoTextBox.Text = registrazione.Oggetto

                            If registrazione.TipologiaRegistrazione <> TipoRegistrazione.Partenza Then
                                Me.DataRicezioneInvioTextBox.SelectedDate = registrazione.DataOraRicezioneInvio
                                Me.OrarioRicezioneInvioTextBox.SelectedDate = registrazione.DataOraRicezioneInvio
                            End If


                            If registrazione.IdTipoRicezione.HasValue Then
                                Me.TipoRicezioneInvioComboBox.FindItemByValue(registrazione.IdTipoRicezione).Selected = True
                            End If

                            If registrazione.IdTipoDocumento.HasValue Then
                                Me.TipologiaDocumentoComboBox.FindItemByValue(registrazione.IdTipoDocumento).Selected = True
                            End If

                            Me.NoteInterneTextBox.Text = registrazione.NoteInterne
                            Me.NoteTextBox.Text = registrazione.Note

                            If Not String.IsNullOrEmpty(registrazione.ProtocolloMittente) Then
                                Me.ProtocolloMittenteTextBox.Text = registrazione.ProtocolloMittente
                            End If


                            If parametriPagina.ContainsKey("RegistrazioneEmail") Then

                                Dim elettronicoItem = Me.TipologiaDocumentoComboBox.FindItemByText("ELETTRONICO")
                                Dim mailItem As RadComboBoxItem = Me.TipoRicezioneInvioComboBox.FindItemByText("PEC")

                                If parametriPagina.ContainsKey("IsPec") Then
                                    Dim isPec = CBool(parametriPagina("IsPec"))
                                    If Not isPec Then
                                       mailItem = Me.TipoRicezioneInvioComboBox.FindItemByText("E-MAIL")
                                    End If
                                 End If

                                If Not mailItem Is Nothing Then
                                    mailItem.Selected = True
                                End If

                                If Not elettronicoItem Is Nothing Then
                                    elettronicoItem.Selected = True
                                End If

                            End If

                            Me.Allegati = registrazione.Allegati
                            Me.Mittenti = registrazione.Mittenti
                            Me.Destinatari = registrazione.Destinatari

                            Me.TipologiaRegistrazione = registrazione.TipologiaRegistrazione

                            Me.Visibilita = registrazione.Visibilita

                            Me.Collegamenti = registrazione.Collegamenti

                            'SE NON E' UNA SEGNALAZIONE DI ILLECITO
                            If Me.Page.Request("Segnalazione") Is Nothing Then
                                Me.AggiornaVisibilitaDaRegistrazione(registrazione)
                            End If

                            If registrazione.Riservato.HasValue Then
                                Me.RiservatoCheckBox.Checked = registrazione.Riservato
                            End If


                            Me.IdEmail = registrazione.IdEmail


                            If registrazione.IdClassificazione.HasValue Then
                                Dim titolari As New ParsecAdmin.TitolarioClassificazioneRepository
                                Dim titolario As ParsecAdmin.TitolarioClassificazione = titolari.Where(Function(c) c.Id = registrazione.IdClassificazione).FirstOrDefault
                                If Not titolario Is Nothing Then
                                    Dim classificazioneCompleta As String = titolari.GetCodiciClassificazione2(registrazione.IdClassificazione, 1) & " " & titolario.Descrizione
                                    Me.ClassificazioneTextBox.Text = classificazioneCompleta
                                    Me.IdClassificazioneTextBox.Text = registrazione.IdClassificazione
                                End If
                                titolari.Dispose()
                            End If

                        Case "Attachment"

                            If parametriPagina.ContainsKey("DescrizioneAllegato") Then
                                Me.DescrizioneDocumentoTextBox.Text = parametriPagina("DescrizioneAllegato")
                            End If

                            Me.ModalitaAperturaPopup = ModalitaPopup.Attachment
                            Me.DataRicezioneInvioTextBox.SelectedDate = registrazione.DataOraRicezioneInvio
                            Me.OrarioRicezioneInvioTextBox.SelectedDate = registrazione.DataOraRicezioneInvio
                            Me.OggettoTextBox.Text = registrazione.Oggetto

                            Me.Allegati = registrazione.Allegati
                            Me.DocumentiGridView.DataSource = Me.Allegati
                            Me.DocumentiGridView.DataBind()

                            Me.Mittenti = registrazione.Mittenti
                            Me.Destinatari = registrazione.Destinatari

                            Me.TipologiaRegistrazione = registrazione.TipologiaRegistrazione 'ParsecPro.TipoRegistrazione.Arrivo
                            Me.ReferentiEsterniGridView.DataSource = Me.Mittenti.ToList
                            Me.SecondoReferentiInterniGridView.DataSource = Me.Destinatari.Where(Function(c) c.Interno = True).ToList 'Interni

                            Me.ReferentiEsterniGridView.DataBind()
                            Me.SecondoReferentiInterniGridView.DataBind()

                            Me.TipologiaIterLabel.Style.Add("display", "none")
                            Me.TipologiaIterComboBox.Style.Add("display", "none")


                            Me.DatiProtocolloTabStrip.FindTabByText("Collegamenti").Style.Add("display", "none")
                            Me.DatiProtocolloTabStrip.FindTabByText("Avanzate").Style.Add("display", "none")
                            Me.DatiProtocolloTabStrip.FindTabByText("Fascicoli").Style.Add("display", "none")

                    End Select

                End If
            End If

            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If

    End Sub

#End Region

    'Imposta il filtro per le Ricerche delle Registrazioni e lo restituisce per le ricerche.
    Private Function GetFiltroRegistrazioni() As ParsecPro.RegistrazioneFiltro

        Dim esercizi As New ParsecPro.EsercizioRepository
        Dim anno As Integer = esercizi.GetQuery.Where(Function(c) c.Corrente = True).FirstOrDefault.Anno
        esercizi.Dispose()

        Dim nullInteger As Nullable(Of Int32) = Nothing
        Dim nullBoolean As Nullable(Of Boolean) = Nothing
        Dim nullDate As Nullable(Of DateTime) = Nothing

        Dim startDate As New DateTime(anno, 1, 1)
        Dim endDate As New DateTime(anno, 12, 31)

        Dim registrazioni As New ParsecPro.RegistrazioniRepository

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim filtro As New ParsecPro.RegistrazioneFiltro With
            {
                .IdUtenteCollegato = utenteCorrente.Id,
                .Arrivo = True,
                .Partenza = True,
                .Interna = True,
                .DataProtocolloInizio = startDate,
                .DataProtocolloFine = endDate,
                .Annullata = False,
                .NumeroProtocolloInizio = nullInteger,
                .NumeroProtocolloFine = nullInteger,
                .ReferenteEsternoDenominazione = String.Empty,
                .ReferenteEsternoNome = String.Empty,
                .ReferenteEsternoCitta = String.Empty,
                .ReferenteEsternoEmail = String.Empty,
                .IdUtenteInserimento = nullInteger,
                .IdStruttura = nullInteger,
                .StrutturaCompleta = False,
                .IdClassificazione = nullInteger,
                .ClassificazioneCompleta = False,
                .Oggetto = String.Empty,
                .ProtocolloMittente = String.Empty,
                .Note = String.Empty,
                .NoteInterne = String.Empty,
                .IdTipoDocumento = nullInteger,
                .IdTipoRicezioneInvio = nullInteger,
                .DataDocumentoInizio = nullDate,
                .DataDocumentoFine = nullDate,
                .DataRicezioneInvioInizio = nullDate,
                .DataRicezioneInvioFine = nullDate
             }

        filtro.ApplicaAbilitazione = Not Me.TipologiaProceduraApertura = TipoProcedura.Consultazione

        registrazioni.Dispose()
        Return filtro

    End Function

    'Imposta il filtro per le Ricerche delle Ricevute e lo restituisce.
    Private Function GetFiltroRicevuta() As ParsecPro.RegistrazioneFiltro

        Dim nullInteger As Nullable(Of Int32) = Nothing
        Dim nullBoolean As Nullable(Of Boolean) = Nothing
        Dim nullDate As Nullable(Of DateTime) = Nothing

        Dim registrazioni As New ParsecPro.RegistrazioniRepository

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim filtro As New ParsecPro.RegistrazioneFiltro With
            {
                .IdUtenteCollegato = utenteCorrente.Id,
                .Arrivo = True,
                .Partenza = True,
                .Interna = True,
                .DataProtocolloInizio = Me.Registrazione.DataImmissione,
                .DataProtocolloFine = Me.Registrazione.DataImmissione,
                .Annullata = nullBoolean,
                .NumeroProtocolloInizio = Me.Registrazione.NumeroProtocollo,
                .NumeroProtocolloFine = Me.Registrazione.NumeroProtocollo,
                .ReferenteEsternoDenominazione = String.Empty,
                .ReferenteEsternoNome = String.Empty,
                .ReferenteEsternoCitta = String.Empty,
                .ReferenteEsternoEmail = String.Empty,
                .IdUtenteInserimento = nullInteger,
                .IdStruttura = nullInteger,
                .StrutturaCompleta = False,
                .IdClassificazione = nullInteger,
                .ClassificazioneCompleta = False,
                .Oggetto = String.Empty,
                .ProtocolloMittente = String.Empty,
                .Note = String.Empty,
                .NoteInterne = String.Empty,
                .IdTipoDocumento = nullInteger,
                .IdTipoRicezioneInvio = nullInteger,
                .DataDocumentoInizio = nullDate,
                .DataDocumentoFine = nullDate,
                .DataRicezioneInvioInizio = nullDate,
                .DataRicezioneInvioFine = nullDate
             }

        registrazioni.Dispose()
        Return filtro

    End Function

    'Metodo NeedDataSource della Griglia ProtocolliGridView: aggancia il datasource delle griglia ProtocolliGridView alla lista Registrazioni (variabile di sessione)
    Protected Sub ProtocolliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ProtocolliGridView.NeedDataSource

        'SE LA GRIGLIA NON E' VISIBILE NON CARICO I PROTOCOLLI

        'Se sto aprendo la finestra come popup
        If Not Page.Request("Mode") Is Nothing Then
            Exit Sub
        End If

        If Not Me.ProtocolliGridView.Visible Then
            Exit Sub
        End If
        '**********************************************************************************************

        If Me.Registrazioni Is Nothing Then
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

            If Me.RegistrazioneFiltro Is Nothing Then
                Dim filtro As ParsecPro.RegistrazioneFiltro = Me.GetFiltroRegistrazioni
                filtro.UltimeCinque = True
                Me.Registrazioni = registrazioni.GetView(filtro)
            Else
                Me.Registrazioni = registrazioni.GetView(Me.RegistrazioneFiltro)
            End If

        End If

        Me.ProtocolliGridView.DataSource = Me.Registrazioni

    End Sub

    'Metodo ItemCommand della Griglia ProtocolliGridView: fa partire i comandi della griglia.
    Protected Sub ProtocolliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ProtocolliGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                AggiornaVista(e)
            Case "Copy"
                Me.AggiornaVista(e)
                Me.Registrazione = Nothing
            Case "Stamp"
                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"))
                registrazioni.Dispose()
                Me.RegistraEseguiStampa(registrazione)
        End Select
    End Sub

    'Metodo che lancia la stampa etichetta
    Private Sub StampaEtichietta()
        Me.RegistraEseguiStampa(Me.Registrazione)
    End Sub

    'Metodo ItemDataBound della Griglia ProtocolliGridView: gestisce i tooltip della griglia e alcuni eventi in base al contentuo delle celle.
    Protected Sub ProtocolliGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ProtocolliGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona registrazione"
            End If
            If TypeOf dataItem("Copy").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Copy").Controls(0), ImageButton)
                btn.ToolTip = "Copia registrazione"
            End If

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim abilitaStampigliatrice As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AbilitaStampigliatrice)).FirstOrDefault Is Nothing


            If Not Me.VisualizzaColonnaUffici Then
                Dim divOggetto = CType(dataItem("Oggetto").FindControl("Oggetto"), HtmlGenericControl)
                divOggetto.Style.Add("width", "360px")
            End If



            If TypeOf dataItem("Stamp").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Stamp").Controls(0), ImageButton)

                If abilitaStampigliatrice Then
                    btn.ToolTip = "Stampa etichetta registrazione"
                Else
                    btn.ToolTip = "L'utente non è abilitato alla stampa delle etichette"
                    btn.Attributes.Add("onclick", "return false;")
                End If

            End If

        End If
    End Sub

    'Metodo ItemCreated della Griglia ProtocolliGridView: gestisce la navigazione tra pagine della griglia
    Protected Sub ProtocolliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ProtocolliGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    'Metodo che richiama il reset della View
    Private Sub ResettaVista()
        Me.ResettaVista(True)
    End Sub

    'Metodo che effettua il reset della View
    Private Sub ResettaVista(ByVal nuovo As Boolean)

        Me.RadToolBar.Items.FindItemByText("Salva").ToolTip = ""
        Me.RadToolBar.Items.FindItemByText("Elimina").ToolTip = ""

        Me.RubricaComboBox.Text = String.Empty
        Me.FiltroDenominazioneTextBox.Text = String.Empty
        Me.FiltroSecondoReferenteInternoTextBox.Text = String.Empty

        Me.VisualizzaStoricoRegistrazioneImageButton.Visible = False

        Me.PannelloSessioneEmergenza.Visible = False
        Me.RadToolBar.Items.FindItemByText("Invia E-mail").Enabled = False

        Me.AreaInfoLabel.Text = String.Empty

        Me.OggettoTextBox.Text = String.Empty

        Me.DataRicezioneInvioTextBox.SelectedDate = Nothing
        Me.OrarioRicezioneInvioTextBox.SelectedDate = Nothing
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        Me.NoteTextBox.Text = String.Empty
        Me.NoteInterneTextBox.Text = String.Empty
        Me.RiservatoCheckBox.Checked = False

        Me.ProtocolloMittenteTextBox.Text = String.Empty
        Me.DataDocumentoTextBox.SelectedDate = Nothing
        Me.AnticipatoViaFaxCheckBox.Checked = False
        Me.TipoRicezioneInvioComboBox.SelectedIndex = 0
        Me.StatoDocumentoComboBox.SelectedIndex = 0

        Me.FaseDocumentoFascicoloComboBox.SelectedIndex = 0

        Me.RiscontroNumeroProtocolloTextBox.Text = String.Empty
        Me.RiscontroDataImmissioneProtocolloTextBox.Text = String.Empty

        Me.DescrizioneDocumentoTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty

        Me.NumeroDocumentiTextBox.Text = String.Empty

        Me.FiltroCategoriaTextBox.Text = String.Empty
        Me.FiltroClasseTextBox.Text = String.Empty
        Me.FiltroSottoClasseTextBox.Text = String.Empty
        Me.FiltroDescrizioneClassificazioneTextBox.Text = String.Empty

        Me.Allegati = New List(Of ParsecPro.Allegato)
        Me.Mittenti = New ParsecPro.Mittenti
        Me.Destinatari = New ParsecPro.Destinatari
        Me.Collegamenti = New List(Of ParsecPro.Collegamento)
        Me.Fascicoli = New List(Of ParsecAdmin.Fascicolo)
        Me.Visibilita = New List(Of ParsecAdmin.VisibilitaDocumento)

        'AGGIUNGO L'UTENTE CORRENTE ALLA VISIBILITA
        '************************************************************************************************************************
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.AggiungiUtenteCollegatoVisibilita(utenteCollegato)
        '************************************************************************************************************************

        'Sessione emergenza
        Me.IdUtenteEmergenzaTextBox.Text = String.Empty
        Me.SessioniEmergenzaComboBox.SelectedIndex = 0
        Me.DataDocumentoTextBox.SelectedDate = Nothing
        Me.NumeroEmergenzaTextBox.Text = String.Empty
        Me.UtenteEmergenzaTextBox.Text = String.Empty

        If nuovo Then
            If Me.ModalitaAperturaPopup = ModalitaPopup.None Then
                Me.CaricaReferenteDefault(Me.TipologiaRegistrazione)
            End If
        End If

        Dim gestStorico As Telerik.Web.UI.RadComboBoxItem = Me.TipoRicezioneInvioComboBox.FindItemByValue(-2)
        If Not gestStorico Is Nothing Then
            gestStorico.Remove()
        End If

        Me.SelezionaModelloIter()

        Me.Registrazione = Nothing

    End Sub

    'Metodo che aggiorna il Pannello delle informazioni Avanzate
    Private Sub AggiornaPannelloAvanzate(ByVal registrazione As ParsecPro.Registrazione)
        Me.DataRicezioneInvioTextBox.SelectedDate = registrazione.DataOraRicezioneInvio
        Me.OrarioRicezioneInvioTextBox.SelectedDate = registrazione.DataOraRicezioneInvio
        If Not registrazione.IdTipoDocumento Is Nothing Then
            Me.TipologiaDocumentoComboBox.FindItemByValue(registrazione.IdTipoDocumento).Selected = True
        End If

        If registrazione.IdTipoRicezione.HasValue Then
            Try
                Me.TipoRicezioneInvioComboBox.FindItemByValue(registrazione.IdTipoRicezione).Selected = True
            Catch ex As Exception

                Dim gestStorico As Telerik.Web.UI.RadComboBoxItem = Me.TipoRicezioneInvioComboBox.FindItemByValue(-2)
                If gestStorico Is Nothing Then
                    Dim tipi As New ParsecPro.TipiRicezioneInvioRepository
                    Dim tipo = tipi.GetQuery.Where(Function(c) c.Id = registrazione.IdTipoRicezione).FirstOrDefault
                    If Not tipo Is Nothing Then
                        Me.TipoRicezioneInvioComboBox.Items.Add(New RadComboBoxItem(tipo.Descrizione.ToUpper, -2))
                        Me.TipoRicezioneInvioComboBox.FindItemByValue(-2).Selected = True
                    End If

                    tipi.Dispose()
                End If

            End Try

        End If
        Me.ProtocolloMittenteTextBox.Text = registrazione.ProtocolloMittente
        Me.AnticipatoViaFaxCheckBox.Checked = registrazione.AnticipatoViaFax

        If registrazione.DataDocumento.HasValue Then
            Me.DataDocumentoTextBox.SelectedDate = registrazione.DataDocumento
        End If

        Me.NoteTextBox.Text = registrazione.Note
        Me.NoteInterneTextBox.Text = registrazione.NoteInterne
    End Sub

    'Metodo che aggiorna il Pannello dei DOcumenti: aggiorna NumeroDocumentiTextBox.Text 
    Private Sub AggiornaPannelloDocumenti(ByVal registrazione As ParsecPro.Registrazione)
        If Not registrazione.NumeroAllegati Is Nothing Then
            Me.NumeroDocumentiTextBox.Text = registrazione.NumeroAllegati
        End If
    End Sub

    'Evento EliminaNoteImageButton_Click (svuota le NoteTextBox)
    Protected Sub EliminaNoteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaNoteImageButton.Click
        Me.NoteTextBox.Text = String.Empty
    End Sub

    'Evento EliminaNoteInterneImageButton_Click (svuota le NoteInterneTextBox)
    Protected Sub EliminaNoteInterneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaNoteInterneImageButton.Click
        Me.NoteInterneTextBox.Text = String.Empty
    End Sub

    'Metodo che affettua l'aggioramento delle informazioni riferite alla Sessione Emergenza.
    Private Sub AggiornaSessioneEmergenza(ByVal registrazione As ParsecPro.Registrazione)

        Dim utenti As New ParsecAdmin.UserRepository
        Dim utente As ParsecAdmin.Utente = utenti.GetQuery.Where(Function(c) c.Id = registrazione.IdUtenteEmergenza).FirstOrDefault
        Dim descrizioneUtente As String = String.Empty
        If Not utente Is Nothing Then
            descrizioneUtente = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
        End If

        utenti.Dispose()

        Dim sessioni As New ParsecPro.SessioniEmergenzaRepository
        Me.SessioniEmergenzaComboBox.DataSource = sessioni.GetSessioni
        Me.SessioniEmergenzaComboBox.DataTextField = "Nome"
        Me.SessioniEmergenzaComboBox.DataValueField = "Id"
        Me.SessioniEmergenzaComboBox.DataBind()
        sessioni.Dispose()

        Me.IdUtenteEmergenzaTextBox.Text = registrazione.IdUtenteEmergenza
        Me.SessioniEmergenzaComboBox.FindItemByValue(CStr(registrazione.IdSessioneEmergenza)).Selected = True
        Me.DataImmissioneTextBox.SelectedDate = registrazione.DataImmissione
        Me.NumeroEmergenzaTextBox.Text = registrazione.NumeroEmergenza
        Me.UtenteEmergenzaTextBox.Text = descrizioneUtente

    End Sub

    'Metodo che abilita/disabilita le info relative  alla sessione di emergenza
    Private Sub AbilitaSessioneEmergenza(ByVal abilita As Boolean)
        Me.SessioniEmergenzaComboBox.Enabled = abilita
        Me.NumeroEmergenzaTextBox.Enabled = abilita
        Me.DataImmissioneTextBox.Enabled = abilita
        Me.TrovaUtenteImageButton.Visible = abilita
        Me.EliminaUtenteImageButton.Visible = abilita
    End Sub

    'Rende visibile o meno il filtro ed il pulsante trova riferito al secondo referente
    Private Sub AbilitaMovimento(ByVal abilita As Boolean)
        Me.FiltroSecondoReferenteInternoTextBox.Visible = abilita
        Me.TrovaSecondoReferenteInternoImageButton.Visible = abilita
        Me.NoteInterneTextBox.Enabled = abilita
    End Sub

    'Imposta Visibilita o Meno dei Controlli della UI
    Private Sub ImpostaAbilitazioneUi(ByVal abilita As Boolean)

        'Scheda Fascicoli
        Me.NuovoFascicoloImageButton.Visible = abilita
        Me.TrovaFascicoloImageButton.Visible = abilita
        'Me.EliminaFascicoliImageButton.Visible = abilita
        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = abilita

        Me.FascicoliGridView.Enabled = abilita
        Me.FaseDocumentoFascicoloComboBox.Visible = abilita
        Me.FaseDocumentoFascicoloLabel.Visible = abilita

        'Scheda Collegamenti
        Me.TrovaCollegamentoImageButton.Visible = abilita
        Me.CollegamentiIndirettiGridView.MasterTableView.Columns.FindByUniqueName("Detail").Visible = abilita
        Me.CollegamentiDirettiGridView.MasterTableView.Columns.FindByUniqueName("Detail").Visible = abilita
        Me.CollegamentiDirettiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = abilita
        Me.CollegamentiDirettiGridView.Enabled = abilita
        Me.CollegamentiIndirettiGridView.Enabled = abilita

        'Scheda Documenti
        Me.ScansionaImageButton.Visible = abilita
        Me.AggiungiDocumentoImageButton.Visible = abilita
        Me.DocumentiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = abilita
        Me.DescrizioneDocumentoTextBox.Enabled = abilita
        Me.NumeroDocumentiTextBox.Enabled = abilita
        Me.DocumentoPrimarioRadioButton.Enabled = abilita
        Me.DocumentoAllegatoRadioButton.Enabled = abilita
        Me.AllegatoUpload.Enabled = abilita

        'Scheda Avanzate
        Me.NoteTextBox.Enabled = abilita
        Me.NoteInterneTextBox.Enabled = abilita
        Me.DataRicezioneInvioTextBox.Enabled = abilita
        Me.OrarioRicezioneInvioTextBox.Enabled = abilita
        Me.TipologiaDocumentoComboBox.Enabled = abilita
        Me.StatoDocumentoComboBox.Enabled = abilita
        Me.EliminaNoteImageButton.Visible = abilita
        Me.EliminaNoteInterneImageButton.Visible = abilita
        Me.ProtocolloMittenteTextBox.Enabled = abilita
        Me.DataDocumentoTextBox.Enabled = abilita
        Me.AnticipatoViaFaxCheckBox.Enabled = abilita
        Me.TipoRicezioneInvioComboBox.Enabled = abilita

        'Classificazione
        Me.FiltroCategoriaTextBox.Visible = abilita
        Me.FiltroClasseTextBox.Visible = abilita
        Me.FiltroSottoClasseTextBox.Visible = abilita
        Me.FiltraClassificazioneImageButton.Visible = abilita
        Me.FiltroDescrizioneClassificazioneTextBox.Visible = abilita
        Me.TrovaClassificazioneImageButton.Visible = abilita
        Me.ClassificazioneTextBox.Enabled = abilita
        Me.EliminaClassificazioneImageButton.Visible = abilita
        Me.FiltroClassificazionePanel.Visible = abilita
        Me.NoteInterneTextBox.Enabled = abilita

        'Scheda Generale
        Me.ReferentiEsterniGridView.Enabled = abilita
        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = abilita
        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("Modifica").Visible = abilita
        Me.FiltroDenominazioneTextBox.Visible = abilita
        Me.TrovaReferenteEsternoIpaImageButton.Visible = abilita
        Me.TrovaReferenteEsternoImageButton.Visible = abilita
        Me.TrovaPrimoReferenteInternoImageButton.Visible = abilita
        Me.AggiungiNuovoReferenteEsternoImageButton.Visible = abilita
        Me.RubricaComboBox.Visible = abilita
        Me.AggiungiReferenteEsternoImageButton.Visible = abilita


        Me.SecondoReferentiInterniGridView.Enabled = abilita
        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = abilita
        Me.TrovaSecondoReferenteInternoImageButton.Visible = abilita
        Me.FiltroSecondoReferenteInternoTextBox.Visible = abilita


        Me.TrovaOggettoImageButton.Visible = abilita
        Me.OggettoTextBox.Enabled = abilita
        Me.TipoRegistrazioneRadioList.Enabled = abilita
        Me.AggiungiOggettoImageButton.Visible = abilita
        Me.OggettiComboBox.Enabled = abilita

        'Scheda Visibilita
        Me.VisibilitaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = abilita
        Me.TrovaUtenteVisibilitaImageButton.Visible = abilita
        Me.TrovaGruppoVisibilitaImageButton.Visible = abilita

    End Sub

    'Aggiorna i campi della vista a partire dal Modello selezionato
    Private Sub AggiornaVistaDaModello(ByVal registrazione As ParsecPro.Registrazione, ByVal selezione As Boolean)
        Dim annullata As Boolean = registrazione.Annullato
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim registrazioneInUso As Boolean = False
        Me.ResettaVista(False)

        Me.Registrazione = registrazione

        'NON COPIO LA TIPOLOGIA DI ITER
        If selezione Then
            If Me.IterAttivato Then
                Dim istanze As New ParsecWKF.IstanzaRepository
                Dim istanza = istanze.Where(Function(c) c.IdDocumento = registrazione.Id And c.IdModulo = ParsecAdmin.TipoModulo.PRO).FirstOrDefault
                If Not istanza Is Nothing Then
                    Me.TipologiaIterComboBox.Items.FindItemByValue(istanza.IdModello).Selected = True
                End If
                istanze.Dispose()
            End If
        End If

        'Carico le informazioni a prescindere dall'operazione

        Me.OggettoTextBox.Text = registrazione.Oggetto

        If registrazione.IdClassificazione.HasValue Then
            Me.ClassificazioneTextBox.Text = registrazione.ClassificazioneCompleta
            Me.IdClassificazioneTextBox.Text = registrazione.IdClassificazione.ToString
        End If

        'Carico i referenti associati al protocollo selezionato
        Me.Mittenti = registrazione.Mittenti
        Me.Destinatari = registrazione.Destinatari

        'ELIMINO I DESTINATARI INTERNI MODIFICATI OD ANNULLATI
        If Not selezione Then
            Dim destinatari As List(Of ParsecPro.Destinatario) = registrazione.Destinatari
            Dim idDest As Integer = 0
            Dim strutture As New ParsecAdmin.StructureRepository
            Dim struttura As ParsecAdmin.Struttura = Nothing
            Dim destToRemove As ParsecPro.Destinatario = Nothing
            For Each dest In Me.Destinatari.Where(Function(c) c.Interno = True).ToList
                idDest = dest.Id
                struttura = strutture.Where(Function(c) c.Id = idDest And Not c.LogStato Is Nothing).FirstOrDefault
                If Not struttura Is Nothing Then
                    destToRemove = destinatari.Where(Function(c) c.Id = idDest And c.Interno = True).FirstOrDefault
                    If Not destToRemove Is Nothing Then
                        destinatari.Remove(destToRemove)
                    End If
                End If
            Next

            Me.Destinatari = destinatari
            strutture.Dispose()
        End If

        Me.TipologiaRegistrazione = registrazione.TipoRegistrazione

        'Seleziono il tipo di registrazione
        Me.TipoRegistrazioneRadioList.SelectedIndex = registrazione.TipoRegistrazione

        If Not registrazione.IdTipoDocumento Is Nothing Then
            Me.TipologiaDocumentoComboBox.FindItemByValue(registrazione.IdTipoDocumento).Selected = True
        End If

        If registrazione.IdTipoRicezione.HasValue Then
            Try
                Me.TipoRicezioneInvioComboBox.FindItemByValue(registrazione.IdTipoRicezione).Selected = True
            Catch ex As Exception

                Dim tipi As New ParsecPro.TipiRicezioneInvioRepository
                Dim tipo = tipi.GetQuery.Where(Function(c) c.Id = registrazione.IdTipoRicezione).FirstOrDefault
                If Not tipo Is Nothing Then
                    Me.TipoRicezioneInvioComboBox.Items.Add(New RadComboBoxItem(tipo.Descrizione.ToUpper, -2))
                    Me.TipoRicezioneInvioComboBox.FindItemByValue(-2).Selected = True
                End If

                tipi.Dispose()
            End Try

        End If

        'Carico i gruppi e gli utenti associati al protocollo selezionato
        Me.Visibilita = registrazione.Visibilita

        'SE STO COPIANDO IL PROTOCOLLO AGGIUNGO L'UTENTE CORRENTE ALLA VISIBILITA
        If Not selezione Then
            Me.AggiungiUtenteCollegatoVisibilita(utenteCollegato)

            'SONO TUTTI CANCELLABILI TRANNE L'UTENTE COLLEGATO
            For Each v In Me.Visibilita
                If v.IdEntita <> utenteCollegato.Id Then
                    v.AbilitaCancellaEntita = True
                End If
            Next

        End If

        If selezione Then

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("numeroCifreProtocollo", ParsecAdmin.TipoModulo.PRO)
            Dim numeroCifre As Integer = 7
            If Not parametro Is Nothing Then
                numeroCifre = CInt(parametro.Valore)
            End If

            Dim color As String = ""
            Select Case registrazione.TipologiaRegistrazione
                Case TipoRegistrazione.Arrivo
                    color = "#FF0000"
                Case TipoRegistrazione.Partenza
                    color = "#00AA00"
                Case TipoRegistrazione.Interna
                    color = "#FF8000"
            End Select

            Me.AreaInfoLabel.Text = "<font color='#00156E'>Protocollo N° " & registrazione.NumeroProtocollo.ToString.PadLeft(numeroCifre, "0") & " del " & String.Format("{0:dd/MM/yyyy}", registrazione.DataImmissione) & " - " & "</font><font color='" & color & "'>" & registrazioni.GetDescrizioneTipoRegistrazione(registrazione.TipoRegistrazione).ToUpper & "</font>"

            parametri.Dispose()

            Me.RiservatoCheckBox.Checked = registrazione.Riservato

            Me.AggiornaPannelloAvanzate(registrazione)
            Me.AggiornaPannelloDocumenti(registrazione)

            'Carico i collegamenti associati al protocollo selezionato
            Me.Collegamenti = registrazione.Collegamenti

            'Carico gli allegati associati al protocollo selezionato
            Me.Allegati = registrazione.Allegati

            'Carico i fascicoli associati al protocollo selezionato
            Me.Fascicoli = registrazione.Fascicoli

            'Gestione blocco registrazione
            Dim registrazioniBloccate As New ParsecPro.LockRegistrazioneRepository
            Dim registrazioneBloccata As ParsecPro.LockRegistrazione = registrazioniBloccate.GetLockRegistrazione(registrazione)
            If Not registrazioneBloccata Is Nothing Then
                'Se la registrazione non è bloccata dall'utente collegato.
                If registrazioneBloccata.IdUtente <> utenteCollegato.Id Then
                    Dim utenteLock As ParsecAdmin.Utente = (New ParsecAdmin.UserRepository).GetQuery.Where(Function(c) c.Id = registrazioneBloccata.IdUtente).FirstOrDefault
                    ParsecUtility.Utility.MessageBox("La registrazione selezionata è bloccata da " & utenteLock.Username & "!", False)
                    registrazioneInUso = True
                End If
            End If

            Dim funzioneModifica As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaRegistrazione)).FirstOrDefault Is Nothing
            Dim funzioneModificaAmmistrativa As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaAmministrativaRegistrazione)).FirstOrDefault Is Nothing
            Dim funzioneMovimento As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.MovimentoRegistrazione)).FirstOrDefault Is Nothing
            Dim funzioneGestioneAllegati As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaAllegatiRegistrazione)).FirstOrDefault Is Nothing

            Dim abilita As Boolean = False


            If Not annullata Then
                If funzioneModificaAmmistrativa Then
                    abilita = True
                    Me.AbilitazioneUtenteCorrente = AbilitazioneUtente.ModificaAmministrativa
                Else
                    If Not registrazioneInUso Then
                        abilita = funzioneModifica
                        Me.AbilitazioneUtenteCorrente = AbilitazioneUtente.Modifica

                    End If
                End If

                If Not registrazioneInUso Then
                    If Not abilita Then
                        If Me.TipologiaProceduraApertura = TipoProcedura.Movimentazione Then
                            abilita = funzioneMovimento
                            If abilita Then
                                Me.AbilitazioneUtenteCorrente = AbilitazioneUtente.Movimentazione
                            End If
                        End If

                        If Me.TipologiaProceduraApertura = TipoProcedura.GestioneAllegati Then
                            abilita = funzioneGestioneAllegati
                        End If

                    End If
                End If

            End If

            If Page.Request("Mode") Is Nothing Then
                Me.RadToolBar.Items.FindItemByText("Salva").Enabled = abilita
            End If
            Me.RadToolBar.Items.FindItemByText("Elimina").Enabled = Not annullata AndAlso Page.Request("Mode") Is Nothing

            'Se posso modificare il protocollo blocco
            If Me.RadToolBar.Items.FindItemByText("Salva").Enabled Then
                If Not registrazioneInUso Then

                    'Blocco la registrazione corrente.
                    registrazioneBloccata = New ParsecPro.LockRegistrazione With
                                                        {
                                                            .Id = registrazioniBloccate.GetNuovoId,
                                                            .IdUtente = utenteCollegato.Id,
                                                            .AnnoProtocollo = Year(registrazione.DataImmissione),
                                                            .NumeroProtocollo = registrazione.NumeroProtocollo,
                                                            .TipoProtocollo = registrazione.TipoRegistrazione
                                                         }

                    registrazioniBloccate.Save(registrazioneBloccata)
                    ParsecUtility.SessionManager.LockRegistrazione = registrazioneBloccata
                End If
            End If

            If funzioneModificaAmmistrativa Then
                Me.ImpostaAbilitazioneUi(Me.RadToolBar.Items.FindItemByText("Salva").Enabled)
            Else
                If funzioneModifica Then
                    Me.ImpostaAbilitazioneUi(Me.RadToolBar.Items.FindItemByText("Salva").Enabled)

                    Me.OggettoTextBox.Enabled = False
                    Me.TrovaOggettoImageButton.Visible = False
                    Me.AggiungiOggettoImageButton.Visible = False
                    Me.OggettiComboBox.Enabled = False

                    Me.TipologiaIterComboBox.Enabled = False

                    Me.TrovaReferenteEsternoImageButton.Visible = False
                    Me.AggiungiNuovoReferenteEsternoImageButton.Visible = False
                    Me.RubricaComboBox.Visible = False
                    Me.AggiungiReferenteEsternoImageButton.Visible = False

                Else
                    If funzioneMovimento Then
                        Me.ImpostaAbilitazioneUi(False)
                        Me.AbilitaMovimento(True)
                    End If
                End If
            End If

        Else
            Me.ImpostaAbilitazioneUi(True)

        End If

        'Gestione abilitazione invio e-mail
        If Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza AndAlso registrazione.Allegati.Count > 0 Then
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("INTabilitaInteroperabilitaXML", ParsecAdmin.TipoModulo.PRO)
            parametri.Dispose()
            Dim interoperabilitaAbilitata As Boolean = False

            Dim abilitaPEC As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AbilitaPec)).FirstOrDefault Is Nothing

            If Not parametro Is Nothing Then
                interoperabilitaAbilitata = (parametro.Valore = "1" AndAlso abilitaPEC)
            End If

            If interoperabilitaAbilitata AndAlso (registrazione.DescrizioneTipoRicezioneInvio.ToLower = "pec" OrElse registrazione.DescrizioneTipoRicezioneInvio.ToLower = "e-mail") Then
                Me.RadToolBar.Items.FindItemByText("Invia E-mail").Enabled = interoperabilitaAbilitata AndAlso Not annullata ' invioEmailAbilitato
            End If

        End If

        'Gestione abilitazione invio e-mail di notifica ai destinatari
        If selezione Then
            If Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna Then
                Me.RadToolBar.Items.FindItemByText("Invia E-mail").Enabled = Not annullata
            End If
        End If

        'Sessione emergenza
        If registrazione.IdSessioneEmergenza.HasValue Then
            Me.PannelloSessioneEmergenza.Visible = True
            Me.AggiornaSessioneEmergenza(registrazione)
            Me.AbilitaSessioneEmergenza(False)
        End If

        'GESTIONE TOOLTIP INFO OPERAZIONI

        If selezione Then

            Dim filtro As New ParsecPro.RegistrazioneFiltro With
               {
                 .NumeroProtocolloInizio = registrazione.NumeroProtocollo,
                 .DataProtocolloInizio = registrazione.DataImmissione,
                 .Partenza = (registrazione.TipoRegistrazione = TipoRegistrazione.Partenza),
                 .Arrivo = (registrazione.TipoRegistrazione = TipoRegistrazione.Arrivo),
                 .Interna = (registrazione.TipoRegistrazione = TipoRegistrazione.Interna)
              }

            Dim nl As Integer = 1
            Dim messaggio As String = String.Empty

            Dim elencoRegistrazioni = registrazioni.GetInfoRegistrazioni(filtro)

            Dim cnt = elencoRegistrazioni.Cast(Of Object).Count()

            Me.VisualizzaStoricoRegistrazioneImageButton.Visible = (cnt > 1)


            For Each r In elencoRegistrazioni
                If nl = 1 Then
                    messaggio = "<span style=""width:80px;text-align:right"">Inserita da</span> <b>" & Replace(r.DescrizioneUtente.ToUpper, "'", "&acute;") & "</b> il <b>" & r.DataImmissione & "</b>"
                Else
                    If CType(r.DataOraAnnullamento, Nullable(Of DateTime)).HasValue Then
                        Dim utenti As New ParsecAdmin.UserRepository
                        Dim descrizioneUtenteAnnullamento As String = String.Empty
                        Dim idUtenteAnnullamento As Integer = r.IdUtenteAnnullamento
                        Dim utenteAnnullamento = utenti.Where(Function(c) c.Id = idUtenteAnnullamento).FirstOrDefault
                        If Not utenteAnnullamento Is Nothing Then
                            descrizioneUtenteAnnullamento = utenteAnnullamento.Username & If(Not String.IsNullOrEmpty(utenteAnnullamento.Cognome), " - " & utenteAnnullamento.Cognome, "") & If(Not String.IsNullOrEmpty(utenteAnnullamento.Nome), " " & utenteAnnullamento.Nome, "")
                        End If
                        utenti.Dispose()
                        messaggio &= "<br><span style=""width:80px;text-align:right"">Annullata da</span> <b>" & Replace(descrizioneUtenteAnnullamento.ToUpper, "'", "&acute;") & "</b> il <b>" & r.DataOraAnnullamento & "</b>"
                    Else
                        messaggio &= "<br><span style=""width:80px;text-align:right"">Modificata da</span> <b>" & Replace(r.DescrizioneUtente.ToUpper, "'", "&acute;") & "</b> il <b>" & r.DataOraRegistrazione & "</b>"
                    End If
                End If
                nl += 1
            Next

            Dim width As Integer = 450
            Dim height As Integer = nl * 16 + 5

            InfoUtenteImageButton.Attributes.Add("onclick", "ShowTooltip(this,'" & messaggio & "'," & width & "," & height & ");")
            InfoUtenteImageButton.Attributes.Add("onmouseout", "HideTooltip();")

            '*********************************************************************************

            registrazioni.Dispose()

        End If

        'NUOVO - GESTIONE MOVIMENTAZIONE
        Select Case Me.TipologiaProceduraApertura
            Case TipoProcedura.Movimentazione
                If selezione Then
                    Select Case Me.TipologiaRegistrazione
                        Case TipoRegistrazione.Arrivo, TipoRegistrazione.Interna
                            Me.DestinatariBloccati.Clear()
                            Dim destinatariInterni = Me.Destinatari.Where(Function(c) c.Interno = True).ToList
                            Me.DestinatariBloccati.AddRange(destinatariInterni)
                    End Select
                End If
        End Select

    End Sub

    'Aggiorna i campi della View. Richiama il metodo AggiornaVistaDaModello.
    Private Sub AggiornaVista(ByVal id As Integer, ByVal selezione As Boolean)
        Try
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(id)
            registrazioni.Dispose()
            Me.AggiornaVistaDaModello(registrazione, selezione)
        Catch ex As Exception
            If ex.InnerException Is Nothing Then
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            Else
                ParsecUtility.Utility.MessageBox(ex.InnerException.Message, False)
            End If

        End Try
    End Sub

    'Richiama il metodo AggiornaVista per aggiornare il contenuto della view.
    Private Sub AggiornaVista(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Me.AggiornaVista(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"), e.CommandName = "Select")
    End Sub

    'Invia una Mail
    Private Sub InviaEmail()

        'VERIFICO SE GLI ALLEGATI SONO STATI MODIFICATI
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim reg = registrazioni.GetById(Me.Registrazione.Id)
        registrazioni.Dispose()

        Dim allegatiCorrenti = Me.Allegati
        Dim allegatiMemorizzati = reg.Allegati
        Dim necessarioSalvare = Me.ListAreNotEquals(Of ParsecPro.Allegato)(allegatiCorrenti, allegatiMemorizzati, "Id")

        If necessarioSalvare Then
            Throw New ApplicationException("E' necessario salvare la registrazione o annullare le modifiche apportate!")
            Exit Sub
        End If

        If Not Me.Registrazione Is Nothing Then
            If Me.TipologiaRegistrazione = TipoRegistrazione.Partenza Then
                Dim pageUrl As String = "~/UI/Protocollo/pages/user/InviaEmailRegistrazionePage.aspx"
                Dim queryString As New Hashtable
                queryString.Add("obj", "")
                Dim parametriPagina As New Hashtable
                parametriPagina.Add("RegistrazioneDaInviare", Me.Registrazione)
                parametriPagina.Add("Modulo", ParsecAdmin.TipoModulo.PRO)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 600, queryString, False)
            End If

            If Me.TipologiaRegistrazione = TipoRegistrazione.Arrivo OrElse Me.TipologiaRegistrazione = TipoRegistrazione.Interna Then
                Try
                    Me.InviaEmailDestinatari()
                    Me.infoOperazioneHidden.Value = "Invio e-mail conclusa con successo!"
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                End Try

            End If

        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una registrazione!", False)
        End If
    End Sub

    'Configura l'Smtp per l'invio Mail
    Private Function ConfigureSmtp(ByVal casellaPec As ParsecAdmin.ParametriPec) As Rebex.Net.Smtp
        Dim client As Rebex.Net.Smtp = Nothing
        Try
            If Not casellaPec Is Nothing Then
                client = New Rebex.Net.Smtp
                client.Settings.SslAcceptAllCertificates = True
                client.Settings.SslAllowedSuites = client.Settings.SslAllowedSuites And TlsCipherSuite.DH_anon_EXPORT_WITH_RC4_40_MD5 Or TlsCipherSuite.DH_anon_WITH_RC4_128_MD5 Or TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA Or TlsCipherSuite.DHE_DSS_WITH_RC4_128_SHA Or TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA Or TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 Or TlsCipherSuite.RSA_WITH_RC4_128_SHA Or TlsCipherSuite.RSA_WITH_RC4_128_MD5 Or TlsCipherSuite.RSA_WITH_3DES_EDE_CBC_SHA Or TlsCipherSuite.RSA_WITH_AES_128_CBC_SHA
                Dim mode As Rebex.Net.SslMode = SslMode.None
                Select Case casellaPec.SmtpPorta.Value
                    Case 465
                        mode = SslMode.Implicit
                    Case 25, 587
                        If casellaPec.SmtpIsSSL Then
                            mode = SslMode.Explicit
                        End If
                End Select
                Dim password As String = ParsecCommon.CryptoUtil.Decrypt(casellaPec.Password)
                client.Connect(casellaPec.SmtpServer, casellaPec.SmtpPorta.Value, mode)
                client.Login(casellaPec.UserId, password)
            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox("Si è verificato il seguente errore: " & ex.Message, False)
        End Try
        Return client
    End Function

    'Ritorna il valore Minimo
    Public Function GetMinValue(Of T)(ByVal list As List(Of T), ByVal propName As String) As Integer

        Dim res As Integer = -1
        If list.Count > 0 Then
            Dim parameters = Linq.Expressions.Expression.Parameter(GetType(T), "c")
            Dim body = Linq.Expressions.Expression.Convert(Linq.Expressions.Expression.Property(parameters, propName), GetType(Object))
            Dim lambda = Linq.Expressions.Expression.Lambda(Of Func(Of T, Object))(body, parameters)
            Dim i = list.AsQueryable().Min(lambda) - 1
            If i < 0 Then
                res = i
            End If
        End If
        Return res
    End Function

    'Confronta due liste e restituisce false o true se sono uguali o meno.
    Public Function ListAreNotEquals(Of T)(ByVal listA As List(Of T), ByVal listB As List(Of T), ByVal propName As String) As Boolean
        If listA.Count <> listB.Count Then
            Return True
        Else
            Dim tipo As Type = GetType(T)
            Dim objectB As T = Nothing

            For Each objectA In listA

                Dim propValue = tipo.GetProperty(propName).GetValue(objectA, Nothing)

                Dim parameters = Linq.Expressions.Expression.Parameter(GetType(T), "p")
                Dim body = Linq.Expressions.Expression.Equal(Linq.Expressions.Expression.PropertyOrField(parameters, propName), Linq.Expressions.Expression.Constant(propValue))

                Dim lambda = Linq.Expressions.Expression.Lambda(Of Func(Of T, Boolean))(body, parameters)

                objectB = listB.Where(lambda.Compile).FirstOrDefault

                If objectB Is Nothing Then
                    Return True
                Else

                    For Each pi In tipo.GetProperties
                        Try
                            Dim propValueA = pi.GetValue(objectA, Nothing)
                            Dim propValueB = pi.GetValue(objectB, Nothing)

                            If propValueA Is Nothing AndAlso propValueB Is Nothing Then
                                Continue For
                            End If

                            If propValueA Is Nothing AndAlso Not propValueB Is Nothing Then
                                Return True
                                Exit For
                            End If

                            If Not propValueA Is Nothing AndAlso propValueB Is Nothing Then
                                Return True
                                Exit For
                            End If

                            If Not TypeOf propValueA Is System.Data.Objects.DataClasses.EntityReference Then
                                If Not propValueA.Equals(propValueB) Then
                                    Return True
                                    Exit For
                                End If
                            End If

                        Catch ex As Exception
                            Continue For
                        End Try


                    Next
                End If
            Next
        End If
        Return False
    End Function

    'Evento click per inviare Mail
    Protected Sub InviaEmailButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles InviaEmailButton.Click

        If Me.TipologiaRegistrazione = TipoRegistrazione.Arrivo Then
            Try
                Me.InviaEmailDestinatari()
                Me.infoOperazioneHidden.Value = "Invio e-mail conclusa con successo!"
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        End If

    End Sub

    'Invia mail ai Destinatari. Richiamato da InviaEmailButton.Click
    Private Sub InviaEmailDestinatari()
        Dim necessarioSalvare As Boolean = False

        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim reg = registrazioni.GetById(Me.Registrazione.Id)
        registrazioni.Dispose()


        'VERIFICO SE I DESTINATARI SONO STATI MODIFICATI E CHE ALMENO UNO SIA CONTRASSEGNATO PER L'INVIO DELL'EMAIL
        Dim destinatariInterniCorrenti = Me.Destinatari.Where(Function(c) c.Interno).ToList

        If destinatariInterniCorrenti.Where(Function(c) c.InviaEmail = True).Count = 0 Then
            Throw New ApplicationException("E' necessario che almeno un destinatario interno sia contrassegnato per l'invio dell'email!")
            Exit Sub
        End If

        Dim destinatariInterniMemorizzati = reg.Destinatari.Where(Function(c) c.Interno).ToList

        necessarioSalvare = Me.ListAreNotEquals(Of ParsecPro.Destinatario)(destinatariInterniCorrenti, destinatariInterniMemorizzati, "Id")

        If necessarioSalvare Then
            Throw New ApplicationException("E' necessario salvare la registrazione o annullare le modifiche apportate!")
            Exit Sub
        End If

        'VERIFICO SE I MITTENTI SONO STATI MODIFICATI
        Dim mittentiCorrenti = Me.Mittenti
        Dim mittentiMemorizzati = reg.Mittenti
        necessarioSalvare = Me.ListAreNotEquals(Of ParsecPro.Mittente)(mittentiCorrenti, mittentiMemorizzati, "Descrizione")
        If necessarioSalvare Then
            Throw New ApplicationException("E' necessario salvare la registrazione o annullare le modifiche apportate!")
            Exit Sub
        End If

        'VERIFICO SE GLI ALLEGATI SONO STATI MODIFICATI
        Dim allegatiCorrenti = Me.Allegati
        Dim allegatiMemorizzati = reg.Allegati
        necessarioSalvare = Me.ListAreNotEquals(Of ParsecPro.Allegato)(allegatiCorrenti, allegatiMemorizzati, "Id")

        If necessarioSalvare Then
            Throw New ApplicationException("E' necessario salvare la registrazione o annullare le modifiche apportate!")
            Exit Sub
        End If

        destinatariInterniCorrenti = destinatariInterniCorrenti.Where(Function(c) c.InviaEmail = True And Not String.IsNullOrEmpty(c.Email)).ToList

        Dim destinatari As List(Of String) = destinatariInterniCorrenti.Select(Function(c) c.Email).ToList
        Dim elencoDestinatari = String.Join(";", destinatari)

        Dim casellaPec As ParsecAdmin.ParametriPec = Nothing
        Dim client As Rebex.Net.Smtp = Nothing

        Try
            'RECUPERO IL MITTENTE PREDEFINITO DELL'EMAIL E CONFIGURO IL CLIENT SMTP
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName("MittenteEmailInvioProtocollo", ParsecAdmin.TipoModulo.PRO)
            parametri.Dispose()

            If parametro Is Nothing Then
                Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "Il parametro 'MittenteEmailInvioProtocollo' non è presente.")
                Exit Sub
            End If

            Dim casellePec As New ParsecAdmin.ParametriPecRepository
            casellaPec = casellePec.GetQuery.Where(Function(c) c.Email = parametro.Valore).FirstOrDefault
            casellePec.Dispose()

            If casellaPec Is Nothing Then
                Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "La casella di posta '" & parametro.Valore & "' non è presente.")
                Exit Sub
            End If

            client = Me.ConfigureSmtp(casellaPec)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
            Exit Sub
        End Try

        'CONFIGURO IL MESSAGGIO EMAIL
        Dim mail As New Rebex.Mail.MailMessage
        Dim mailAttach As Rebex.Mail.Attachment = Nothing

        Dim clienti As New ParsecAdmin.ClientRepository
        Dim cliente = clienti.GetQuery.FirstOrDefault
        clienti.Dispose()

        Dim sb As New StringBuilder

        sb.AppendLine(cliente.Descrizione)
        sb.AppendLine("")

        Dim descrizioneProtocollo As String = "Prot. N. " & Me.Registrazione.NumeroProtocollo.Value.ToString & "/" & Me.Registrazione.DescrizioneTipologiaRegistristrazione.Chars(0) & " del " & Me.Registrazione.DataImmissione.Value.ToShortDateString

        sb.AppendLine(descrizioneProtocollo)
        sb.AppendLine("")

        sb.AppendLine("Mittenti:")
        For Each mittente In Me.Registrazione.Mittenti
            sb.AppendLine("- " & mittente.Descrizione)
        Next
        sb.AppendLine("")

        sb.AppendLine("Destinatari:")
        For Each destinatario In Me.Registrazione.Destinatari
            sb.AppendLine("- " & destinatario.Descrizione)
        Next

        sb.AppendLine("")
        sb.AppendLine("Oggetto:")
        sb.AppendLine(Me.Registrazione.Oggetto)

        mail.From = casellaPec.Email
        mail.Subject = cliente.Descrizione & " - " & descrizioneProtocollo
        mail.BodyText = sb.ToString

        For Each destinatario In destinatari
            mail.To.Add(destinatario)
        Next

        Dim allegati As New List(Of ParsecPro.AllegatoEmail)

        Dim allegatoPrimario As ParsecPro.Allegato = Me.Registrazione.Allegati.Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
        If Not allegatoPrimario Is Nothing Then
            Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)

            Dim fullPath As String = percorsoRoot & allegatoPrimario.PercorsoRelativo & allegatoPrimario.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegatoPrimario.NomeFile

            If IO.File.Exists(fullPath) Then
                mailAttach = New Rebex.Mail.Attachment(fullPath)
                mailAttach.FileName = allegatoPrimario.NomeFile
                mail.Attachments.Add(mailAttach)

                Dim allegatoEmail As New ParsecPro.AllegatoEmail
                allegatoEmail.IdAllegato = allegatoPrimario.Id
                allegati.Add(allegatoEmail)

            End If
        End If

        'INVIO L'EMAIL
        Try
            client.Timeout = 0
            client.Send(mail)
            client.Disconnect()
        Catch ex As Exception
            Throw New ApplicationException("Impossibile inviare l'email per il seguente motivo:" & vbCrLf & ex.Message)
            Exit Sub
        End Try

        'SALVO L'EMAIL SU DISCO
        Dim percorsoRelativo As String = String.Format("\{0}\", Now.Year)
        Dim nomeEmail As String = Guid.NewGuid.ToString & ".eml"
        Try

            Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & percorsoRelativo & nomeEmail
            If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata")) Then
                IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata"))
            End If
            If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & percorsoRelativo) Then
                IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & percorsoRelativo)
            End If
            mail.Save(mailBoxPath, Rebex.Mail.MailFormat.Mime)
        Catch ex As Exception
            Throw New ApplicationException("Impossibile salvare l'email su disco per il seguente motivo:" & vbCrLf & ex.Message)
            Exit Sub
        End Try

        'INSERISCO L'EMAIL NEL DB
        Try
            Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
            Dim emails As New EmailRepository
            Dim email As ParsecPro.Email = emails.CreateFromInstance(Nothing)
            email.IdCasella = casellaPec.Id
            email.Inviata = True
            email.IdUtente = utente.Id
            email.DataInvio = Now
            email.Corpo = mail.BodyText
            email.Oggetto = mail.Subject
            email.Destinatari = elencoDestinatari
            email.PercorsoRelativo = percorsoRelativo
            email.NomeFileEml = nomeEmail
            email.NumeroProtocollo = Me.Registrazione.NumeroProtocollo.Value
            email.AnnoProtocollo = Me.Registrazione.DataImmissione.Value.Year
            email.MessaggioId = mail.MessageId.Id

            email.TipoProtocollo = reg.TipoRegistrazione

            emails.Save(email)
            emails.Dispose()

            If allegati.Count > 0 Then
                Dim allegatiEmail As New AllegatoEmailRepository
                allegatiEmail.SaveAll(email.Id, allegati)
                allegatiEmail.Dispose()
            End If

        Catch ex As Exception
            Dim msg As String = String.Empty
            If Not ex.InnerException Is Nothing Then
                msg = ex.InnerException.Message
            Else
                msg = ex.Message
            End If
            Throw New ApplicationException("Impossibile salvare i dati dell'email nel database per il seguente motivo:" & vbCrLf & msg)
            Exit Sub
        End Try

    End Sub

    'Carica il referente di Default
    Private Sub CaricaReferenteDefault(ByVal tipo As ParsecPro.TipoRegistrazione)

        Select Case tipo
            Case TipoRegistrazione.Arrivo

                Dim refentiDefault As New ParsecPro.ReferenteInternoDefaultRepository

                Dim destinatariInterni = refentiDefault.Where(Function(c) c.Arrivo = True And c.Mittente = False).ToList
                Dim destinatarioInterno As ParsecPro.IReferente = Nothing
                Dim strutture As New ParsecPro.StrutturaViewRepository
                Dim struttura As ParsecPro.Struttura = Nothing

                For Each referente In destinatariInterni
                    Dim codice = referente.CodiceStruttura
                    struttura = strutture.GetQuery.Where(Function(c) c.Codice = codice And c.LogStato Is Nothing).FirstOrDefault
                    If Not struttura Is Nothing Then
                        destinatarioInterno = New ParsecPro.Destinatario(struttura.Id, True)

                        'Se non esiste
                        If Me.Destinatari.Where(Function(c) c.Id = destinatarioInterno.Id).FirstOrDefault Is Nothing Then

                            If Me.CheckEmail(struttura.Email) Then
                                destinatarioInterno.InviaEmail = referente.InviaEmail
                            End If

                            Dim avviabile = Me.IterAvviabile(struttura.Id)
                            If avviabile Then
                                destinatarioInterno.Iter = referente.Iter
                            End If

                            Me.Destinatari.Add(destinatarioInterno)

                            'AGGIUNGO L'UTENTE O IL GRUPPO DI VISIBILITA'
                            If struttura.IdGerarchia = 400 Then  'PERSONA
                                If struttura.IdUtente.HasValue Then
                                    Dim utenti As New ParsecAdmin.UserRepository
                                    Dim utente As ParsecAdmin.Utente = utenti.GetUserById(struttura.IdUtente).FirstOrDefault
                                    If Not utente Is Nothing Then
                                        Me.AggiungiUtenteVisibilita(utente)
                                    End If

                                End If
                            Else
                                Me.AggiungiGruppoDefault(struttura.Id)
                            End If

                        End If
                    End If

                Next

                strutture.Dispose()
                refentiDefault.Dispose()
        End Select

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        If utenteCollegato.CodiceStutturaDefault.HasValue Then
            Dim codice As Integer = utenteCollegato.CodiceStutturaDefault
            Dim strutture As New ParsecPro.StrutturaViewRepository
            Dim struttura = strutture.GetQuery.Where(Function(c) c.Codice = codice And c.LogStato Is Nothing).FirstOrDefault

            If Not struttura Is Nothing Then
                Select Case tipo
                    Case ParsecPro.TipoRegistrazione.Arrivo

                        'NIENTE
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        Dim mittenteInterno As ParsecPro.IReferente = Nothing
                        mittenteInterno = New ParsecPro.Mittente(struttura.Id, True) 'AOO CO.RE.COM. 
                        'Se non esiste
                        If Me.Mittenti.Where(Function(c) c.Id = mittenteInterno.Id).FirstOrDefault Is Nothing Then
                            Me.Mittenti.Add(mittenteInterno)
                        End If

                        Me.AggiungiGruppoDefault(struttura.Id)

                End Select


            End If
        End If
    End Sub

    'Lancia la Stampa il Registro generale
    Private Sub StampaRegistroGenerale()
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaElencoRegistrazioniPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "1")
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 550, queryString, False)
    End Sub

    'Lancia la Stampa Elenco Registrazioni
    Private Sub StampaElencoRegistrazioni()
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaElencoRegistrazioniPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "0")
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 550, queryString, False)
    End Sub

    'Evento Click associato alla Toolbar:lancia i vari comandi dalla Toolbar.
    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName

            Case "InviaEmail"
                Try
                    Me.InviaEmail()
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                End Try

            Case "Stampa"
                Me.Print()

            Case "StampaRicevuta"
                If Me.Registrazione Is Nothing Then
                    ParsecUtility.Utility.MessageBox("E' necessario selezionare una registrazione!", False)
                Else
                    Me.StampaRicevuta()
                End If

            Case "StampaEtichetta"
                If Me.Registrazione Is Nothing Then
                    ParsecUtility.Utility.MessageBox("E' necessario selezionare una registrazione!", False)
                Else
                    Me.StampaEtichietta()
                End If

            Case "StampaRegistroGenerale"
                Me.StampaRegistroGenerale()


            Case "StampaElencoRegistrazioni"
                Me.StampaElencoRegistrazioni()

            Case "Salva"

                Dim success As Boolean = True
                Dim message As String = "Operazione conclusa con successo!"
                Dim nuovo As Boolean = Me.Registrazione Is Nothing
                Try
                    Me.Save()
                    Me.Registrazioni = Nothing
                    Me.ProtocolliGridView.Rebind()
                    'Sblocco tutte le registrazioni dell'utente collegato.
                    Me.EliminaLockRegistrazione()
                Catch ex As ApplicationException
                    message = ex.Message
                    success = False
                End Try

                If Not success Then
                    ParsecUtility.Utility.MessageBox(message, False)
                Else
                    Me.infoOperazioneHidden.Value = message
                End If

                If success Then

                    If Not Me.Registrazione Is Nothing Then
                        Dim parametri As New ParsecAdmin.ParametriRepository
                        Dim parametro = parametri.GetByName("AbilitaSmistamentoEmailSuSalva", ParsecAdmin.TipoModulo.PRO)
                        parametri.Dispose()

                        If Not parametro Is Nothing Then
                            If parametro.Valore = "1" Then


                                Dim destinatariInterniCorrenti = Me.Destinatari.Where(Function(c) c.Interno).ToList
                                If destinatariInterniCorrenti.Where(Function(c) c.InviaEmail = True).Count > 0 Then
                                    ParsecUtility.Utility.ButtonClick(Me.InviaEmailButton.ClientID, False)
                                End If

                            End If
                        End If
                    End If

                    If Not Page.Request("Mode") Is Nothing Then

                        If nuovo Then
                            'SE NON HO PROTOCOLLATO UNA FATTURA
                            If Page.Request("Fattura") Is Nothing Then
                                'todo inviare una email al mittente della pec
                            End If
                        End If

                        ParsecUtility.SessionManager.Registrazione = Me.Registrazione
                    End If

                End If

            Case "Nuovo"

                Me.RadToolBar.Items.FindItemByText("Salva").Enabled = True
                'Abilito tutta l'interfaccia utente
                Me.ImpostaAbilitazioneUi(True)

                Me.ResettaVista()

                Me.Registrazioni = Nothing
                Me.ProtocolliGridView.Rebind()
                Me.PannelloSessioneEmergenza.Visible = (Me.ModalitaRegistrazione = ParsecPro.ModalitaRegistrazione.Emergenza)

                Me.AbilitaSessioneEmergenza(True)

            Case "Annulla"

                Me.RadToolBar.Items.FindItemByText("Salva").Enabled = True
                'Abilito tutta l'interfaccia utente
                Me.ImpostaAbilitazioneUi(True)

                Me.ResettaVista()
                Me.Registrazioni = Nothing
                Me.ProtocolliGridView.Rebind()

                Me.PannelloSessioneEmergenza.Visible = (Me.ModalitaRegistrazione = ParsecPro.ModalitaRegistrazione.Emergenza)
                Me.AbilitaSessioneEmergenza(True)

                'Sblocco tutte le registrazioni dell'utente collegato.
                Me.EliminaLockRegistrazione()

            Case "Elimina"

                Dim message As String = "Operazione conclusa con successo!"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Registrazione Is Nothing Then
                        Try
                            Me.Delete()
                            'Sblocco tutte le registrazioni dell'utente collegato.
                            Me.EliminaLockRegistrazione()

                            If Not Page.Request("Mode") Is Nothing Then
                                ParsecUtility.SessionManager.Registrazione = Me.Registrazione
                                Me.infoOperazioneHidden.Value = "Cancellazione conclusa con successo!"

                                Me.ResettaVista()

                                Exit Sub
                            End If

                            Me.ResettaVista()
                            Me.ProtocolliGridView.Rebind()

                            Me.infoOperazioneHidden.Value = message

                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)
                        End Try

                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una registrazione!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
            Case "RicercaAvanzata"
                Me.AdvancedSearch()

        End Select
    End Sub

    'Metodo Print
    Private Sub Print()
        'TODO
    End Sub

    'Ritorna l'Id del Responsabile di Area
    Private Function ResponsabileArea(ByVal IdReferenteInterno As Integer, ByVal settore As Boolean) As Nullable(Of Integer)
        Dim strutture As New ParsecAdmin.StructureRepository

        Dim IdResponsabile As Nullable(Of Integer) = Nothing

        'SETTORE
        If settore Then

            IdResponsabile = (From struttura In strutture.GetQuery
                            Where struttura.IdPadre = IdReferenteInterno And struttura.LogStato Is Nothing And struttura.IdGerarchia = 400 And struttura.Responsabile = True
                            Select struttura.IDUtente).FirstOrDefault
        Else
            'SERVIZIO
            Dim innerQuery = From s In strutture.GetQuery Where s.Id = IdReferenteInterno And s.IdGerarchia = 200 Select s.IdPadre
            IdResponsabile = (From struttura In strutture.GetQuery
                           Where struttura.IdPadre = innerQuery.FirstOrDefault And struttura.LogStato Is Nothing And struttura.IdGerarchia = 400 And struttura.Responsabile = True
                           Select struttura.IDUtente).FirstOrDefault
        End If

        strutture.Dispose()

        Return IdResponsabile

    End Function

    'Ritorna le informazioni sul Protocollo
    Private Function GetInfoProtocollo(ByVal idProtocollo As Integer, ByVal idReferenteInterno As Integer) As ProtocolloInfo

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("numeroCifreProtocollo", ParsecAdmin.TipoModulo.PRO)
        Dim numeroCifre As Integer = 7
        If Not parametro Is Nothing Then
            numeroCifre = CInt(parametro.Valore)
        End If
        parametri.Dispose()

        Dim protocolli As New ParsecPro.RegistrazioniRepository
        Dim referentiInterni As New ParsecPro.ReferentiInterniRepository
        Dim strutture As New ParsecAdmin.StructureRepository


        Dim vista = (From protocollo In protocolli.GetQuery.AsEnumerable
                    Join referenteEsterno In referentiInterni.GetQuery.AsEnumerable
                    On protocollo.Id Equals referenteEsterno.IdRegistrazione
                    Join struttura In strutture.GetQuery.AsEnumerable
                    On referenteEsterno.Id Equals struttura.Id
                    Where protocollo.Id = idProtocollo And struttura.Id = idReferenteInterno
                    Select New ProtocolloInfo With {
                        .DescrizioneRegistrazione = "Protocollo N° " & protocollo.NumeroProtocollo.ToString.PadLeft(numeroCifre, "0") & "/" & protocolli.GetDescrizioneTipoRegistrazione(protocollo.TipoRegistrazione).ToUpper.Chars(0) & " del " & String.Format("{0:dd/MM/yyyy}", protocollo.DataImmissione) & " Oggetto: " & protocollo.Oggetto,
                        .IdMittente = protocollo.IdUtente,
                        .IdDestinatario = struttura.IDUtente
                        }).FirstOrDefault

        protocolli.Dispose()
        referentiInterni.Dispose()
        strutture.Dispose()

        Return vista

    End Function

    ' verifico se l'istanza del documento è in iter opure no
    Public Function DocumentoInIter(ByVal protocollo As ParsecPro.Registrazione, ByVal idReferenteInterno As Integer) As Boolean
        Dim res As Boolean = False

        'leggo le informazioni del protocollo che carico nel workflow
        Dim protocolloInfo As ProtocolloInfo = Me.GetInfoProtocollo(protocollo.Id, idReferenteInterno)

        Dim IdDestinatario As Nullable(Of Integer) = Nothing

        ' SETTORE 
        If protocolloInfo.IdDestinatario.HasValue OrElse protocolloInfo.IdDestinatario.Value <> -1 Then
            IdDestinatario = Me.ResponsabileArea(idReferenteInterno, True)
            'SERVIZIO
            If Not IdDestinatario.HasValue Then
                IdDestinatario = Me.ResponsabileArea(idReferenteInterno, False)
            End If
        Else
            ' PERSONA
            IdDestinatario = protocolloInfo.IdDestinatario
        End If

        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.ContatoreGenerale = protocollo.NumeroProtocollo AndAlso c.IdModulo = 2 AndAlso c.Ufficio = IdDestinatario AndAlso Year(c.DataInserimento) = Year(protocollo.DataOraRegistrazione)).FirstOrDefault

        res = Not istanza Is Nothing

        Return res
    End Function

    'Ritorna il Destinatario dell'iter
    Private Function GetDestinatarioIter(ByVal idReferenteInterno As Integer) As Integer
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim struttura = strutture.GetQuery.Where(Function(c) c.Id = idReferenteInterno).FirstOrDefault

        Dim idDestinatario As Nullable(Of Integer) = struttura.IDUtente

        If Not idDestinatario.HasValue OrElse idDestinatario.Value < 1 Then


            idDestinatario = (From s In strutture.GetQuery
                              Where s.IdPadre = idReferenteInterno And s.LogStato Is Nothing And s.IdGerarchia = 400 And s.Responsabile = True
                              Select s.IDUtente).FirstOrDefault

            If Not idDestinatario.HasValue Then

                Dim innerQuery = From s In strutture.GetQuery Where s.Id = idReferenteInterno And s.IdGerarchia = 200 Select s.IdPadre
                idDestinatario = (From s In strutture.GetQuery
                               Where s.IdPadre = innerQuery.FirstOrDefault And s.LogStato Is Nothing And s.IdGerarchia = 400 And s.Responsabile = True
                               Select s.IDUtente).FirstOrDefault

            End If
        End If


        strutture.Dispose()

        Return idDestinatario

    End Function

    ' Carico nel Workflow il protocollo individuato per quell'utente
    Public Sub CreaIstanzaPro(ByVal protocollo As ParsecPro.Registrazione, ByVal modello As ParsecWKF.Modello)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim destinatariInterniInIter As List(Of ParsecPro.Destinatario) = Nothing

        destinatariInterniInIter = protocollo.Destinatari.Where(Function(c) c.Interno = True And c.Iter = True).ToList

        For i As Integer = 0 To destinatariInterniInIter.Count - 1

            Dim statoIniziale As Integer = 1
            Dim idIstanza As Integer = 0
            Dim idTask As Integer = 0

            Dim idReferenteInterno As Integer = destinatariInterniInIter(i).Id

            Dim strutture As New ParsecAdmin.StructureRepository
            Dim struttura = strutture.GetQuery.Where(Function(c) c.Id = idReferenteInterno).FirstOrDefault
            strutture.Dispose()

            Dim IdDestinatario As Nullable(Of Integer) = Nothing

            ' SETTORE 
            If Not struttura.IDUtente.HasValue Then
                IdDestinatario = Me.ResponsabileArea(idReferenteInterno, True)
                'SERVIZIO
                If Not IdDestinatario.HasValue Then
                    IdDestinatario = Me.ResponsabileArea(idReferenteInterno, False)
                End If
            Else
                If struttura.IDUtente < 1 Then
                    IdDestinatario = Me.ResponsabileArea(idReferenteInterno, True)
                    'SERVIZIO
                    If Not IdDestinatario.HasValue Then
                        IdDestinatario = Me.ResponsabileArea(idReferenteInterno, False)
                    End If
                Else
                    ' PERSONA
                    IdDestinatario = struttura.IDUtente
                End If
            End If

            Dim siglaIstanza As String = String.Empty


            Select Case Me.TipologiaIterComboBox.SelectedItem.Text
                Case "Pratica UG"
                    siglaIstanza = "UG"
                Case "Pratica GU5"
                    siglaIstanza = "GU5"
                Case "Pratica GU14"
                    siglaIstanza = "GU14"
            End Select

            Dim descrizioneRegistrazione As String = String.Empty
            Select Case Me.TipologiaIterComboBox.SelectedItem.Text
                Case "Pratica UG", "Pratica GU5", "Pratica GU14"
                    descrizioneRegistrazione = "Istanza " & siglaIstanza & " - " & protocollo.Oggetto & " - Prot. n. " & protocollo.NumeroProtocollo.ToString.PadLeft(7, "0") & "/" & protocollo.DataImmissione.Value.Year.ToString
                Case Else
                    descrizioneRegistrazione = String.Format("Prot. n. {0}/{1} - Oggetto : {2}", protocollo.NumeroProtocollo.ToString.PadLeft(7, "0"), protocollo.DataImmissione.Value.Year.ToString, protocollo.Oggetto)
            End Select

            'mittente del protocollo 
            Dim idMittente As Integer = utenteCollegato.Id

            Dim numeroProtocollo As Integer = protocollo.NumeroProtocollo
            Dim istanze As New ParsecWKF.IstanzaRepository

            Dim idModello As Integer = modello.Id

            Dim istanzaPrecedente As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.ContatoreGenerale = protocollo.NumeroProtocollo AndAlso c.IdModulo = 2 AndAlso c.Ufficio = IdDestinatario AndAlso Year(c.DataInserimento) = Year(protocollo.DataImmissione) And c.IdModello = idModello).FirstOrDefault


            'Se l'istanza del documento non è in iter

            If istanzaPrecedente Is Nothing Then

                Dim istanza As New ParsecWKF.Istanza
                istanza.Riferimento = descrizioneRegistrazione
                istanza.IdStato = statoIniziale
                istanza.DataInserimento = Now
                istanza.DataScadenza = Now.AddDays(modello.DurataIter)
                istanza.IdModello = modello.Id
                istanza.IdDocumento = protocollo.Id
                istanza.Ufficio = IdDestinatario
                istanza.ContatoreGenerale = numeroProtocollo
                istanza.IdModulo = 2
                istanza.IdUtente = utenteCollegato.Id

                istanza.FileIter = modello.NomeFile

                Try
                    istanze.Save(istanza)
                    idIstanza = istanze.Istanza.Id
                    istanze.Dispose()

                    'Inserisco nei parametri del processo l'attore DESTINATARIO corrente.
                    Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
                    Dim processo As New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "DESTINATARIO", .Valore = IdDestinatario.ToString}
                    parametriProcesso.Add(processo)
                    parametriProcesso.SaveChanges()

                    'Inserisco nei parametri del processo l'attore MITTENTE se non esiste.
                    Dim parametroProcessoExist As Boolean = Not parametriProcesso.GetQuery.Where(Function(c) c.IdProcesso = idIstanza And c.Nome = "MITTENTE").FirstOrDefault Is Nothing
                    If Not parametroProcessoExist Then
                        processo = New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "MITTENTE", .Valore = utenteCollegato.Id.ToString}
                        parametriProcesso.Add(processo)
                        parametriProcesso.SaveChanges()
                    End If

                    parametriProcesso.Dispose()

                    ' inserisco il task dell'istanza appena inserita
                    Dim tasks As New ParsecWKF.TaskRepository
                    Dim task As New ParsecWKF.Task
                    task.IdIstanza = idIstanza
                    task.Nome = ""
                    Dim corrente As String = modello.StatoIniziale
                    task.Corrente = corrente
                    task.Successivo = modello.StatoSuccessivo(corrente)

                    Dim list As List(Of ParsecWKF.Action) = ParsecWKF.ModelloInfo.ReadActionInfo(task.Corrente, modello.NomeFile)
                    'Recupero il nome del ruolo (To) associato all'azione.
                    Dim roleToName = list(0).ToActor

                    Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleToName).FirstOrDefault
                    Dim idRuolo As Integer
                    If Not role Is Nothing Then
                        idRuolo = role.Id
                        Dim ruoloUtente As ParsecWKF.RuoloRelUtente = (New ParsecWKF.RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault
                        If Not ruoloUtente Is Nothing Then
                            idMittente = ruoloUtente.IdUtente
                        End If
                    End If

                    task.Mittente = idMittente
                    task.IdStato = 5
                    task.DataInizio = Now
                    task.DataEsecuzione = Now
                    task.DataFine = Now.AddDays(modello.DurataTask)
                    task.Operazione = "NUOVO"
                    task.Cancellato = False
                    task.IdUtenteOperazione = utenteCollegato.Id

                    Try

                        tasks.Save(task)
                        tasks.Dispose()

                        'Aggiungo il nuovo task
                        Me.Procedi(task, istanza, idMittente, IdDestinatario)

                    Catch ex As Exception
                        Throw New ApplicationException(ex.Message)
                    End Try

                Catch ex As Exception
                    Throw New ApplicationException(ex.Message)
                End Try

            End If

        Next

    End Sub

    'INSERISCO IL TASK AUTOMATICO
    Private Sub Procedi(ByVal taskAttivo As ParsecWKF.Task, ByVal istanzaAttiva As ParsecWKF.Istanza, ByVal idUtente As Integer, ByVal idDestinatario As Integer)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim statoEseguito As Integer = 6
        Dim statoDaEseguire As Integer = 5
        Dim statoIstanzaCompletato As Integer = 3

        Dim nomeFileIter As String = istanzaAttiva.FileIter
        Dim idIstanza = istanzaAttiva.Id

        Dim tasks As New ParsecWKF.TaskRepository
        tasks.Attach(taskAttivo)

        Dim actions = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Corrente, nomeFileIter)

        Dim taskSuccessivoAutomatico As ParsecWKF.ParametroProcesso = actions(0).Parameters.Where(Function(c) c.Nome = "TaskSuccessivoAutomatico").FirstOrDefault

        Dim azione As ParsecWKF.Action = Nothing

        If Not taskSuccessivoAutomatico Is Nothing Then
            azione = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Successivo, nomeFileIter).Where(Function(c) c.Name = taskSuccessivoAutomatico.Valore).FirstOrDefault
        Else
            azione = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Successivo, nomeFileIter)(0)
        End If

        Dim operazione As String = "INIZIO"
        Dim procediAction = actions.Where(Function(c) c.Type = "INIZIO").FirstOrDefault
        If Not procediAction Is Nothing Then
            If Not String.IsNullOrEmpty(procediAction.Description) Then
                operazione = procediAction.Description.ToUpper
            End If
        End If

        'Recupero il nome del ruolo (To) associato all'azione.
        Dim roleToName = azione.ToActor
        Dim roleFromName = azione.FromActor
        If roleToName <> roleFromName Then
            idUtente = idDestinatario
        End If

        'AGGIORNO IL TASK PRECEDENTE
        If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
            taskAttivo.Note = Me.NoteInterneTextBox.Text
        End If

        taskAttivo.IdStato = statoEseguito
        taskAttivo.DataEsecuzione = Now
        taskAttivo.Operazione = operazione
        taskAttivo.Destinatario = idUtente
        taskAttivo.Notificato = True
        tasks.SaveChanges()

        'INSERISCO IL NUOVO TASK
        Dim statoSuccessivo As String = ParsecWKF.ModelloInfo.StatoSuccessivoAction(taskAttivo.Successivo, azione.Name, nomeFileIter)
        Dim durata As Integer = 0

        If Not String.IsNullOrEmpty(statoSuccessivo) Then
            durata = ParsecWKF.ModelloInfo.DurataTaskIter(statoSuccessivo, nomeFileIter)
        End If

        'ANNULLO L'ISTANZA
        If azione.Type = "FINE" Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = idIstanza).FirstOrDefault
            istanza.IdStato = statoIstanzaCompletato
            istanza.DataEsecuzione = Now
            istanze.SaveChanges()
            istanze.Dispose()
        End If

        Dim nuovotask As New ParsecWKF.Task
        nuovotask.IdIstanza = taskAttivo.IdIstanza
        nuovotask.TaskPadre = taskAttivo.Id

        nuovotask.Nome = taskAttivo.Corrente

        nuovotask.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo, nomeFileIter)

        nuovotask.Mittente = idUtente

        If azione.Type = "FINE" Then
            nuovotask.IdStato = statoEseguito
            nuovotask.Corrente = "FINE"
            nuovotask.Destinatario = idUtente
            nuovotask.Operazione = "FINE"
            nuovotask.DataEsecuzione = Now
        Else
            nuovotask.IdStato = statoDaEseguire
            nuovotask.Corrente = statoSuccessivo
        End If

        nuovotask.DataInizio = Now

        If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
            nuovotask.Note = Me.NoteInterneTextBox.Text
        End If


        nuovotask.DataFine = Now.AddDays(durata)
        nuovotask.Cancellato = False
        nuovotask.Notificato = False
        nuovotask.IdUtenteOperazione = utenteCollegato.Id

        tasks.Add(nuovotask)
        tasks.SaveChanges()

        tasks.Dispose()
    End Sub

    'Aggiorno la Istanza del Workflow
    Public Sub AggiornaIstanzaWorkflow(ByVal idDocumento As Integer, ByVal protocollo As ParsecPro.Registrazione, ByVal idModulo As Integer)
        Dim descrizioneRegistrazione As String = String.Format("Prot. n. {0}/{1} - Oggetto : {2}", protocollo.NumeroProtocollo.ToString.PadLeft(7, "0"), protocollo.DataImmissione.Value.Year.ToString, protocollo.Oggetto)
        Dim istanze As New IstanzaRepository
        Dim lista = istanze.GetQuery.Where(Function(c) c.IdDocumento = idDocumento AndAlso c.IdModulo = idModulo).ToList
        For Each istanza In lista
            istanza.IdDocumento = protocollo.Id
            istanza.Riferimento = descrizioneRegistrazione
            istanza.ContatoreGenerale = protocollo.NumeroProtocollo
        Next

        istanze.SaveChanges()
        istanze.Dispose()
    End Sub

    'Verifico se è settato il parametro IdModelloWorkflowFatturaElettronica: se lo è setto TipologiaIterComboBox.SelectedValue = parametro.Valore
    Private Function VerificaIterFattura() As Boolean
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("IdModelloWorkflowFatturaElettronica", ParsecAdmin.TipoModulo.PRO)
        parametri.Dispose()
        If Not parametro Is Nothing Then
            If TipologiaIterComboBox.SelectedValue = parametro.Valore Then
                Return True
            End If
        End If
        Return False
    End Function

    'Movimento il Destinatario
    Private Sub MovimentaDestinatario(idReferenteInterno As Integer)

        Dim strutture As New ParsecAdmin.StructureRepository
        Dim struttura = strutture.GetQuery.Where(Function(c) c.Id = idReferenteInterno).FirstOrDefault
        strutture.Dispose()

        Dim idDestinatario As Nullable(Of Integer) = Nothing

        ' SETTORE 
        If Not struttura.IDUtente.HasValue Then
            idDestinatario = Me.ResponsabileArea(idReferenteInterno, True)
            'SERVIZIO
            If Not idDestinatario.HasValue Then
                idDestinatario = Me.ResponsabileArea(idReferenteInterno, False)
            End If
        Else
            If struttura.IDUtente < 1 Then
                idDestinatario = Me.ResponsabileArea(idReferenteInterno, True)
                'SERVIZIO
                If Not idDestinatario.HasValue Then
                    idDestinatario = Me.ResponsabileArea(idReferenteInterno, False)
                End If
            Else
                ' PERSONA
                idDestinatario = struttura.IDUtente
            End If
        End If

        Dim istanze As New IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.IdDocumento = Me.Registrazione.Id AndAlso c.IdModulo = 2).FirstOrDefault
        istanza.Ufficio = idDestinatario
        istanze.SaveChanges()
        istanze.Dispose()
        Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
        Dim parametroProcesso = parametriProcesso.Where(Function(c) c.IdProcesso = istanza.Id And c.Nome = "DESTINATARIO").FirstOrDefault
        parametroProcesso.Valore = idDestinatario.ToString
        parametriProcesso.SaveChanges()
        parametriProcesso.Dispose()
    End Sub

    'Termino il Task
    Private Sub TerminaTask()
        If Not Me.TaskAttivo Is Nothing Then
            Dim lista = Me.Destinatari.Where(Function(c) c.Interno = True And c.Iter = True).Select(Function(c) c.Id).ToList()
            'SE I DESTINATARI INTERNI IN ITER NON CONTENGONO IL DESTINATARIO ASSOCIATO AL TASK CORRENTE
            Dim idReferenteInterno As Integer = 0

            Dim processa As Boolean = True
            If Me.Registrazione.TipologiaRegistrazione = TipoRegistrazione.Arrivo OrElse Me.Registrazione.TipologiaRegistrazione = TipoRegistrazione.Interna Then

                '# SE IL DESTINATARIO (ATTORE CORRENTE) NON E' TRA I DESTINATARI (PERCHE' IL TASK E' STATO SMISTATO - MODIFICA DEL RESPONSABILE DOPO L'AVVIO DELL'ITER - ALTRO)
                If Not Me.DestinatarioTaskCorrente Is Nothing Then
                    processa = Not lista.Contains(Me.DestinatarioTaskCorrente.Id)
                Else
                    processa = False
                End If

            End If

            If processa Then
                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)


                If Me.Registrazione.TipologiaRegistrazione = TipoRegistrazione.Arrivo OrElse Me.Registrazione.TipologiaRegistrazione = TipoRegistrazione.Interna Then
                    idReferenteInterno = Me.DestinatarioTaskCorrente.Id
                End If

                'TERMINO IL TASK CORRENTE
                Dim statoTaskEseguito As Integer = 6
                Dim statoIstanzaCompletato As Integer = 3

                Dim tasks As New ParsecWKF.TaskRepository
                Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.Id).FirstOrDefault
                task.IdStato = statoTaskEseguito
                task.DataEsecuzione = Now
                task.Destinatario = Me.TaskAttivo.IdMittente
                task.Operazione = "MODIFICA"
                task.Notificato = True
                tasks.SaveChanges()

                'Cambia lo stato del processo di workflow
                Dim istanze As New ParsecWKF.IstanzaRepository
                Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
                istanza.IdStato = statoIstanzaCompletato
                istanza.DataEsecuzione = Now
                istanze.SaveChanges()

                Dim nuovoTask As New ParsecWKF.Task
                nuovoTask.IdIstanza = Me.TaskAttivo.IdIstanza
                nuovoTask.Nome = task.Corrente
                nuovoTask.Corrente = "FINE"
                nuovoTask.Successivo = String.Empty
                nuovoTask.Destinatario = Me.TaskAttivo.IdMittente
                nuovoTask.Mittente = Me.TaskAttivo.IdMittente
                nuovoTask.TaskPadre = task.Id
                nuovoTask.DataEsecuzione = Now
                nuovoTask.DataInizio = task.DataInizio
                nuovoTask.DataFine = task.DataFine
                nuovoTask.IdStato = statoTaskEseguito
                nuovoTask.Operazione = "FINE"
                nuovoTask.Notificato = True
                nuovoTask.Note = task.Note
                nuovoTask.IdUtenteOperazione = utenteCollegato.Id
                tasks.Add(nuovoTask)
                tasks.SaveChanges()

            End If

        End If
    End Sub

    'Scrivo su file il log
    Private Sub WriteLog(ex As Exception)
        Dim logFilePath As String = ParsecAdmin.WebConfigSettings.GetKey("LogFilePath")
        Dim w As IO.StreamWriter = IO.File.AppendText(logFilePath & "LogInvio.txt")
        w.WriteLine(New String("*", 20))
        w.WriteLine("Eccezione del {0} alle ore {1}", DateTime.Now.ToLongDateString, DateTime.Now.ToLongTimeString)
        w.WriteLine(ex.Message)
        If Not ex.InnerException Is Nothing Then
            w.WriteLine("Eccezione Interna")
            w.WriteLine(ex.InnerException.Message)
        End If
        w.WriteLine(New String("*", 20))
        w.Flush()
        w.Close()
    End Sub


    'Metodo che si occupa del Salvataggio
    Private Sub Save()
        Dim successo As Boolean = True

        Select Case Me.TipologiaProceduraApertura
            Case TipoProcedura.Movimentazione, TipoProcedura.Fascicolazione, TipoProcedura.GestioneAllegati
                If Me.Registrazione Is Nothing Then
                    Throw New ApplicationException("E' necessario selezionare una registrazione di protocollo!")
                    Exit Sub
                End If
        End Select

        'SE E' UN ITER DI FATTURA

        If VerificaIterFattura() Then
            Dim destCount = Me.Destinatari.Where(Function(c) c.Interno = True And c.Iter = True).Count
            If destCount > 1 Then
                Throw New ApplicationException("Per l'iter selezionato è possibile contrassegnare come partecipante un solo destinatario!")
                Exit Sub
            End If
            If destCount = 0 Then
                Throw New ApplicationException("Per l'iter selezionato è necessario contrassegnare come partecipante un destinatario!")
                Exit Sub
            End If

        End If

        If Me.ModalitaAperturaPopup = ModalitaPopup.Attachment Then
            If Me.Allegati.Count = 0 Then
                Throw New ApplicationException("E' necessario inserire almeno un allegato!")
                Exit Sub
            End If
        End If

        Dim idRegistrazione As Integer = 0
        Dim nuovo As Boolean = Me.Registrazione Is Nothing

        If Not nuovo Then
            idRegistrazione = Me.Registrazione.Id
            'TODO
            Dim verificaInIter As Boolean = True

            Select Case Me.TipologiaProceduraApertura
                Case TipoProcedura.Movimentazione, TipoProcedura.Fascicolazione, TipoProcedura.GestioneAllegati, TipoProcedura.Annullamento
                    verificaInIter = False
            End Select

            If verificaInIter Then
                If Me.AbilitazioneUtenteCorrente <> AbilitazioneUtente.ModificaAmministrativa Then
                    If Me.ModalitaAperturaPopup = ModalitaPopup.None Then
                        Dim istanze As New ParsecWKF.IstanzaRepository
                        If istanze.ProtocolloInIter(idRegistrazione) Then
                            istanze.Dispose()
                            Throw New ApplicationException("Il protocollo selezionato non può essere modificato " & vbCrLf & " perchè fa parte di un iter!")
                            Exit Sub
                        End If
                    End If
                End If
            End If

        End If

        Dim registrazioni As New ParsecPro.RegistrazioniRepository

        'Inserisco sempre una nuova registrazione.
        Dim registrazione As ParsecPro.Registrazione = New ParsecPro.Registrazione

        registrazione.ModalitaRegistrazione = Me.ModalitaRegistrazione

        registrazione.DataOraRegistrazione = Now
        registrazione.DataProtocollo = Now
        registrazione.Annullato = False
        registrazione.Modificato = False
        registrazione.Riservato = Me.RiservatoCheckBox.Checked
        registrazione.AnticipatoViaFax = Me.AnticipatoViaFaxCheckBox.Checked
        registrazione.ProtocolloMittente = Me.ProtocolloMittenteTextBox.Text

        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            registrazione.IdClassificazione = Me.IdClassificazioneTextBox.Text
        End If

        If Me.TipoRicezioneInvioComboBox.SelectedIndex <> 0 Then
            registrazione.IdTipoRicezione = Me.TipoRicezioneInvioComboBox.SelectedItem.Value

            If registrazione.IdTipoRicezione = -2 Then
                If Not Me.Registrazione Is Nothing Then
                    registrazione.IdTipoRicezione = Me.Registrazione.IdTipoRicezione
                Else
                    registrazione.IdTipoRicezione = Nothing
                End If
            End If
            registrazione.DescrizioneTipoRicezioneInvio = Me.TipoRicezioneInvioComboBox.SelectedItem.Text
        End If

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        registrazione.IdUtente = utente.Id
        registrazione.UtenteUsername = utente.Username

        If nuovo Then

            registrazione.PresentiModifiche = False

            If Me.ModalitaRegistrazione = ParsecPro.ModalitaRegistrazione.Emergenza Then

                If Not String.IsNullOrEmpty(Me.IdUtenteEmergenzaTextBox.Text) Then
                    Dim idUtenteEmergenza As Integer = CInt(Me.IdUtenteEmergenzaTextBox.Text)
                    Dim utenti As New ParsecAdmin.UserRepository
                    Dim utenteEmergenza As ParsecAdmin.Utente = utenti.GetQuery.Where(Function(c) c.Id = idUtenteEmergenza).FirstOrDefault
                    utenti.Dispose()
                    If Not utenteEmergenza Is Nothing Then
                        registrazione.IdUtenteEmergenza = idUtenteEmergenza
                        registrazione.UtenteEmergenzaUsername = utenteEmergenza.Username
                    End If
                End If

                If Not String.IsNullOrEmpty(Me.NumeroEmergenzaTextBox.Text) Then
                    If Not ParsecUtility.Utility.CheckNumber(Me.NumeroEmergenzaTextBox.Text) Then
                        registrazione.NumeroEmergenza = CInt(Me.NumeroEmergenzaTextBox.Text)
                    End If
                End If

                If Not Me.SessioniEmergenzaComboBox.SelectedItem Is Nothing Then
                    registrazione.IdSessioneEmergenza = Me.SessioniEmergenzaComboBox.SelectedValue
                End If

                registrazione.DataImmissione = Me.DataImmissioneTextBox.SelectedDate

            Else
                registrazione.DataImmissione = registrazione.DataOraRegistrazione
            End If

        Else

            registrazione.NumeroProtocollo = Me.Registrazione.NumeroProtocollo
            registrazione.PresentiModifiche = True
            registrazione.DataImmissione = Me.Registrazione.DataImmissione
        End If


        registrazione.TipoRegistro = ParsecPro.TipoRegistro.Generale
        registrazione.TipologiaRegistrazione = Me.TipologiaRegistrazione
        registrazione.TipoRegistrazione = CInt(Me.TipologiaRegistrazione)

        If Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna Then
            registrazione.TipoRegistro = ParsecPro.TipoRegistro.Interno
        End If

        'Scheda avanzate

        Dim dataRicezioneInvio As Nullable(Of Date) = Me.DataRicezioneInvioTextBox.SelectedDate
        Dim oraRicezioneInvio As Nullable(Of Date) = Me.OrarioRicezioneInvioTextBox.SelectedDate

        If oraRicezioneInvio.HasValue AndAlso dataRicezioneInvio.HasValue Then
            registrazione.DataOraRicezioneInvio = dataRicezioneInvio.Value.AddHours(oraRicezioneInvio.Value.Hour).AddMinutes(oraRicezioneInvio.Value.Minute)
        Else
            registrazione.DataOraRicezioneInvio = Me.DataRicezioneInvioTextBox.SelectedDate
        End If

        If Me.TipologiaDocumentoComboBox.SelectedIndex <> 0 Then
            registrazione.IdTipoDocumento = Me.TipologiaDocumentoComboBox.SelectedItem.Value
        End If

        registrazione.Note = Me.NoteTextBox.Text
        registrazione.NoteInterne = Me.NoteInterneTextBox.Text

        registrazione.DataDocumento = Me.DataDocumentoTextBox.SelectedDate
        registrazione.Oggetto = Trim(Me.OggettoTextBox.Text)

        If Not String.IsNullOrEmpty(Me.NumeroDocumentiTextBox.Text) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroDocumentiTextBox.Text) Then
                registrazione.NumeroAllegati = CInt(Me.NumeroDocumentiTextBox.Text)
            End If
        End If

        If Not String.IsNullOrEmpty(Me.RiscontroNumeroProtocolloTextBox.Text) Then
            registrazione.NumeroProtocolloRiscontro = CInt(Me.RiscontroNumeroProtocolloTextBox.Text)
            registrazione.DataImmissioneRiscontro = CDate(Me.RiscontroDataImmissioneProtocolloTextBox.Text)
        End If

        registrazione.Destinatari = Me.Destinatari

        registrazione.Mittenti = Me.Mittenti
        registrazione.Allegati = Me.Allegati
        registrazione.Collegamenti = Me.Collegamenti
        registrazione.Fascicoli = Me.Fascicoli
        registrazione.Visibilita = Me.Visibilita

        Try

            'Gestione storico
            registrazioni.Registrazione = Me.Registrazione

            registrazione.IdEmail = Me.IdEmail

            If Not Me.FatturaElettronica Is Nothing Then
                registrazione.IdFatturaElettronica = Me.FatturaElettronica.Id
            End If

            registrazioni.Save(registrazione)

            'GESTIONE TIMBRATURA

            Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente

            Dim watermark As String = cliente.Descrizione & " - Cod. Amm. " & cliente.CodiceAmministrazione & " - Prot. n. " & registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy HH:mm}", registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(registrazione.TipoRegistrazione).ToUpper

            Dim posizione As Integer = 4   'SINISTRA - PROTOCOLLO PARTENZA/INTERNO
            If Me.TipologiaRegistrazione = TipoRegistrazione.Arrivo Then
                posizione = 3  'DESTRA
            End If

            For Each all As ParsecPro.Allegato In Me.Allegati
                If all.Scansionato Then
                    Me.AddWatermarkToPdf(watermark, posizione, all)
                End If
            Next

            'Aggiorno l'oggetto corrente
            Dim nuovoIdRegistrazione As Integer = registrazioni.Registrazione.Id
            registrazioni.Dispose()

            registrazioni = New ParsecPro.RegistrazioniRepository

            Me.Registrazione = registrazioni.GetById(nuovoIdRegistrazione)

            'Gestione iter

            If Me.IterAttivato Then

                If Me.ModalitaAperturaPopup <> ModalitaPopup.Attachment Then

                    If Not nuovo Then
                        Me.AggiornaIstanzaWorkflow(idRegistrazione, Me.Registrazione, 2)
                    End If

                    'SE IL MODELLO DI ITER E' FATTURA
                    'SE IN MODIFICA

                    If Not nuovo AndAlso Me.VerificaIterFattura() Then

                        'L'ITER DI FATTURA GESTISCE SOLO UN DESTINATARIO
                        Dim destinatarioInternoCorrente = Me.Registrazione.Destinatari.Where(Function(c) c.Interno = True And c.Iter = True).FirstOrDefault
                        Dim idReferenteInterno As Integer = destinatarioInternoCorrente.Id
                        Me.MovimentaDestinatario(idReferenteInterno)

                    Else

                        If Not Me.DisabilitaIter Then
                            Me.TerminaTask()

                            Dim modelli As New ParsecWKF.ModelliRepository
                            Dim idModello As Integer = Me.TipologiaIterComboBox.SelectedValue
                            Dim modello As ParsecWKF.Modello = modelli.GetById(idModello)
                            modelli.Dispose()

                            Me.CreaIstanzaPro(Me.Registrazione, modello)
                        End If

                    End If

                End If

            End If

            Me.AggiornaVistaDaModello(Me.Registrazione, True)

        Catch ex As Exception

            If ex.InnerException Is Nothing Then
                Throw New ApplicationException(ex.Message)
            Else
                Throw New ApplicationException(ex.InnerException.Message)
            End If
            successo = False

        Finally
            registrazioni.Dispose()
        End Try

        If successo Then

            Me.IdEmail = Nothing

            Me.FatturaElettronica = Nothing

        End If

    End Sub

    'Elimina il Blocco della registrazione
    Private Sub EliminaLockRegistrazione()
        Dim lockRegistrazione As ParsecPro.LockRegistrazione = CType(ParsecUtility.SessionManager.LockRegistrazione, ParsecPro.LockRegistrazione)
        If Not lockRegistrazione Is Nothing Then
            Dim registrazioniBloccate As New ParsecPro.LockRegistrazioneRepository
            registrazioniBloccate.Delete(lockRegistrazione.Id)
            registrazioniBloccate.Dispose()
            ParsecUtility.SessionManager.LockRegistrazione = Nothing
        End If
    End Sub


    'Metodo Delete che Annulla una Registrazione di protocollo
    Private Sub Delete()

        If String.IsNullOrEmpty(Me.NoteTextBox.Text.Trim) Then
            Throw New ApplicationException("E' necessario compilare il campo Note con la motivazione dell'annullamento!")
            Exit Sub
        End If

        If Me.ModalitaAperturaPopup = ModalitaPopup.None Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            If istanze.ProtocolloInIter(Me.Registrazione.Id) Then
                istanze.Dispose()
                Throw New ApplicationException("Il protocollo selezionato non può essere annullato " & vbCrLf & " perchè fa parte di un iter!")
                Exit Sub
            End If

        Else
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim protocolloPiuIter = istanze.Where(Function(c) c.IdDocumento = Me.Registrazione.Id And c.IdModulo = ParsecAdmin.TipoModulo.PRO And c.IdStato = 1).Count > 1
            istanze.Dispose()
            If protocolloPiuIter Then
                Throw New ApplicationException("Il protocollo selezionato non può essere annullato " & vbCrLf & " perchè fa parte di un iter destinato a più assegnatari!")
                Exit Sub
            End If
        End If

        Dim idRegistrazione As Integer = 0
        Dim success As Boolean = True

        Try
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetQuery.Where(Function(c) c.Id = Me.Registrazione.Id).FirstOrDefault
            idRegistrazione = registrazione.Id
            registrazione.Note = Me.NoteTextBox.Text
            registrazione.NoteInterne = Me.NoteInterneTextBox.Text
            registrazione.DataOraAnnullamento = Now
            registrazione.IdUtenteAnnullamento = utenteCollegato.Id
            registrazione.UtenteUsernameAnnullamento = utenteCollegato.Username
            registrazione.Annullato = True
            registrazioni.SaveChanges()
            registrazioni.Dispose()

        Catch ex As Exception
            success = False
            Throw New ApplicationException("Impossibile annullare la registrazione per il seguente motivo: " & ex.Message & "!")
        End Try

        If success Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            istanze.AnnullaIter(idRegistrazione, ParsecAdmin.TipoModulo.PRO)
        End If
    End Sub


#Region "GESTIONE MITTENTI DESTINATARI"

    'metodo che ritorna true o false in base se un Iter è avviabile oppure no
    Private Function IterAvviabile(idStruttura As Integer) As Boolean
        Dim res As Boolean = False
        Dim strutture As New ParsecAdmin.StructureRepository

        Dim struttura As ParsecAdmin.Struttura = strutture.GetQuery.Where(Function(c) c.Id = idStruttura).FirstOrDefault
        If Not struttura Is Nothing Then
            Select Case struttura.IdGerarchia
                Case "400" 'PERSONA
                    If struttura.IDUtente.HasValue Then
                        If struttura.IDUtente > 0 Then
                            res = True
                        End If
                    End If
                Case "100", "300" 'SETTORE E RUOLO

                    res = Not strutture.GetQuery.Where(Function(c) c.IdPadre = idStruttura And c.LogStato Is Nothing And c.Responsabile = True And c.IdGerarchia = 400 And c.IDUtente > 0).FirstOrDefault Is Nothing

                Case "200"  'UFFICIO
                    res = Not strutture.GetQuery.Where(Function(c) c.IdPadre = idStruttura And c.LogStato Is Nothing And c.Responsabile = True And c.IdGerarchia = 400 And c.IDUtente > 0).FirstOrDefault Is Nothing

                    If Not res Then
                        res = Not strutture.GetQuery.Where(Function(c) c.IdPadre = struttura.IdPadre And c.LogStato Is Nothing And c.Responsabile = True And c.IdGerarchia = 400 And c.IDUtente > 0).FirstOrDefault Is Nothing
                    End If

            End Select
        End If
        strutture.Dispose()
        Return res
    End Function

    'Metodo ItemDataBound della Griglia ReferentiEsterniGridView: setta tooltip ed associa eventi in base al contenuto delle celle.
    Protected Sub ReferentiEsterniGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ReferentiEsterniGridView.ItemDataBound
        Dim referente As ParsecPro.IReferente = Nothing
        Dim iterCheckBoxEnabled As Boolean = True
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim ref = CType(e.Item.DataItem, ParsecPro.IReferente)

            Dim iterCheckBox As CheckBox = CType(e.Item.FindControl("IterCheckBox"), CheckBox)

            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.Style.Add("cursor", "hand")
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        btn.ToolTip = String.Format("Elimina mittente {0}", If(ref.Interno, "interno", "esterno"))
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        btn.ToolTip = String.Format("Elimina destinatario {0}", If(ref.Interno, "interno", "esterno"))
                End Select
            End If

            If TypeOf dataItem("Modifica").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Modifica").Controls(0), ImageButton)

                If ref.Interno Then
                    btn.ToolTip = ""
                    btn.ImageUrl = "~\images\blank.gif"
                    btn.Attributes.Add("onclick", "return false;")


                Else
                    btn.Style.Add("cursor", "hand")
                    Select Case Me.TipologiaRegistrazione
                        Case ParsecPro.TipoRegistrazione.Arrivo
                            btn.ToolTip = "Modifica mittente esterno"
                        Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                            btn.ToolTip = "Modifica destinatario esterno"
                    End Select
                End If

            End If

            Select Case Me.TipologiaRegistrazione
                Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna

                    Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
                    Dim perConoscenzaCheckBox As CheckBox = CType(e.Item.FindControl("PerConoscenzaCheckBox"), CheckBox)
                    perConoscenzaCheckBox.Attributes.Add("onclick", "document.getElementById('" & Me.IdReferenteInternoTextBox.ClientID & "').value=" & id & ";document.getElementById('" & Me.secondoReferenteInternoChechedButton.ClientID & "').click();")
                    referente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
                    perConoscenzaCheckBox.Checked = referente.PerConoscenza

                    If Me.IterAttivato Then

                        If Me.DestinatariBloccati.Select(Function(c) c.Id).Contains(referente.Id) Then
                            Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                            btn.ImageUrl = "~\images\vuoto.png"
                            btn.ToolTip = ""
                            btn.Enabled = False
                            iterCheckBox.Enabled = False
                            iterCheckBoxEnabled = False
                        Else
                            If Not Me.DestinatarioTaskCorrente Is Nothing Then
                                If Me.DestinatarioTaskCorrente.Id = referente.Id Then

                                    iterCheckBox.Enabled = False
                                    iterCheckBoxEnabled = False
                                End If
                            End If
                        End If

                        iterCheckBox.Attributes.Add("onclick", "document.getElementById('" & Me.IdReferenteInternoTextBox.ClientID & "').value=" & id & ";document.getElementById('" & Me.secondoReferenteInternoIterChechedButton.ClientID & "').click();")
                        iterCheckBox.Checked = referente.Iter

                        Dim avviabileIter As Boolean = Me.IterAvviabile(referente.Id)
                        If iterCheckBoxEnabled Then
                            iterCheckBox.Enabled = avviabileIter
                        End If

                        Dim descrizione As String = String.Empty
                        If referente.Descrizione.StartsWith("p.c.: ") OrElse referente.Descrizione.StartsWith("c.a.: ") Then
                            descrizione = referente.Descrizione.Substring(6, referente.Descrizione.Length - 6)
                        End If

                        If iterCheckBoxEnabled Then
                            iterCheckBox.ToolTip = If(avviabileIter, "", "Per il destinatario " & descrizione & " l'iter non è avviabile")
                        Else
                            iterCheckBox.ToolTip = "Lo stato dell'iter non è modificabile."
                        End If

                    End If

                Case ParsecPro.TipoRegistrazione.Arrivo
                    'Niente
            End Select

            Select Case Me.TipologiaRegistrazione
                Case TipoRegistrazione.Arrivo, TipoRegistrazione.Interna
                    iterCheckBox.Style.Add("display", If(ref.Interno, "block", "none"))
                Case TipoRegistrazione.Partenza

            End Select

            Select Case Me.TipologiaRegistrazione
                Case TipoRegistrazione.Interna

                    Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")

                    Dim inviaEmailEnabled As Boolean = Me.CheckEmail(referente.Email)

                    Dim descrizione As String = String.Empty
                    If referente.Descrizione.StartsWith("p.c.: ") OrElse referente.Descrizione.StartsWith("c.a.: ") Then
                        descrizione = referente.Descrizione.Substring(6, referente.Descrizione.Length - 6)
                    End If


                    Dim inviaEmailCheckBox As CheckBox = CType(e.Item.FindControl("InviaEmailCheckBox"), CheckBox)
                    If inviaEmailEnabled Then
                        inviaEmailCheckBox.Attributes.Add("onclick", "document.getElementById('" & Me.IdReferenteInternoTextBox.ClientID & "').value=" & id & ";document.getElementById('" & Me.secondoReferenteInternoInviaEmailChechedButton.ClientID & "').click();")
                        inviaEmailCheckBox.ToolTip = "Invia email a " & referente.Email
                    Else
                        inviaEmailCheckBox.ToolTip = "Al destinatario " & descrizione & " non è possibile inviare l'email. Necessario indirizzo valido"
                    End If
                    inviaEmailCheckBox.Checked = referente.InviaEmail
                    inviaEmailCheckBox.Enabled = inviaEmailEnabled

            End Select

        End If

    End Sub

    'Metodo ItemCommand della griglia ReferentiEsterniGridView: lancia i comandi associati alla griglia dei referenti
    Protected Sub ReferentiEsterniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ReferentiEsterniGridView.ItemCommand
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Dim referente As ParsecPro.IReferente = Nothing
        Select Case e.CommandName
            Case "Delete"
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        referente = Me.Mittenti.Where(Function(c) c.Id = id).FirstOrDefault
                        Me.Mittenti.Remove(referente)
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        referente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
                        Me.Destinatari.Remove(referente)
                End Select
            Case "Modifica"

                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        referente = Me.Mittenti.Where(Function(c) c.Id = id).FirstOrDefault
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        referente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
                End Select

                Dim pageUrl As String = "InserimentoReferenteEsterno.aspx"
                Dim queryString As New Hashtable
                queryString.Add("obj", Me.AggiornaNuovoReferenteEsternoImageButton.ClientID)

                Dim parametriPagina As New Hashtable
                parametriPagina.Add("Referente", referente)

                Dim lista As New List(Of ParsecPro.IReferente)
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        For Each mitt In Me.Mittenti
                            lista.Add(mitt)
                        Next
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        For Each dest In Me.Destinatari
                            lista.Add(dest)
                        Next
                End Select

                parametriPagina.Add("ListaReferenti", lista)

                ParsecUtility.SessionManager.ParametriPagina = parametriPagina

                ParsecUtility.Utility.ShowRadWindow(pageUrl, "AggiungiReferenteEsternoRadWindow", queryString, False)

        End Select
    End Sub

    'Metodo ItemCreated della griglia ReferentiEsterniGridView: gestisce lo stile dei GridHeaderItem
    Private Sub ReferentiEsterniGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ReferentiEsterniGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "999")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    'Metodo PreRender della griglia ReferentiEsterniGridView: setta la visibilità dei check associati alla griglia dei referenti.
    Protected Sub ReferentiEsterniGridView_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReferentiEsterniGridView.PreRender
        For Each col As GridColumn In ReferentiEsterniGridView.MasterTableView.Columns

            If col.UniqueName = "CheckBoxTemplateColumn" Then
                col.Visible = (Me.TipologiaRegistrazione <> ParsecPro.TipoRegistrazione.Arrivo)
            End If

            If col.UniqueName = "CheckBoxIterTemplateColumn" Then
                col.Visible = (Me.TipologiaRegistrazione <> ParsecPro.TipoRegistrazione.Arrivo AndAlso Me.IterAttivato)
            End If

            If col.UniqueName = "CheckBoxInviaEmailTemplateColumn" Then
                col.Visible = (Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna)
            End If

        Next
    End Sub

    'Metodo ItemDataBound della Griglia SecondoReferentiInterniGridView: setta tooltip ed associa eventi in base al contenuto delle celle.
    Protected Sub SecondoReferentiInterniGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles SecondoReferentiInterniGridView.ItemDataBound
        Dim referente As ParsecPro.IReferente = Nothing
        Dim iterCheckBoxEnabled As Boolean = True
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.Style.Add("cursor", "hand")
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        btn.ToolTip = "Elimina destinatario interno"
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        btn.ToolTip = "Elimina mittente interno"
                End Select
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        If Me.IterAttivato Then
                            Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
                            referente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
                            If Me.DestinatariBloccati.Select(Function(c) c.Id).Contains(referente.Id) Then

                                btn.ImageUrl = "~\images\vuoto.png"
                                btn.ToolTip = "Non cancellabile"
                                btn.Enabled = False

                                Dim iterCheckBox As CheckBox = CType(e.Item.FindControl("IterCheckBox"), CheckBox)
                                iterCheckBox.Enabled = False
                                iterCheckBoxEnabled = False
                            Else
                                If Not Me.DestinatarioTaskCorrente Is Nothing Then
                                    If Me.DestinatarioTaskCorrente.Id = referente.Id Then
                                        Dim iterCheckBox As CheckBox = CType(e.Item.FindControl("IterCheckBox"), CheckBox)
                                        iterCheckBox.Enabled = False
                                        iterCheckBoxEnabled = False
                                    End If
                                End If

                            End If
                        End If

                End Select

            End If
            Select Case Me.TipologiaRegistrazione
                Case ParsecPro.TipoRegistrazione.Arrivo
                    Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
                    Dim perConoscenzaCheckBox As CheckBox = CType(e.Item.FindControl("PerConoscenzaCheckBox"), CheckBox)
                    perConoscenzaCheckBox.Attributes.Add("onclick", "document.getElementById('" & Me.IdReferenteInternoTextBox.ClientID & "').value=" & id & ";document.getElementById('" & Me.secondoReferenteInternoChechedButton.ClientID & "').click();")
                    referente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
                    perConoscenzaCheckBox.Checked = referente.PerConoscenza

                    Dim descrizione As String = String.Empty
                    If referente.Descrizione.StartsWith("p.c.: ") OrElse referente.Descrizione.StartsWith("c.a.: ") Then
                        descrizione = referente.Descrizione.Substring(6, referente.Descrizione.Length - 6)
                    End If

                    If Me.IterAttivato Then
                        Dim iterCheckBox As CheckBox = CType(e.Item.FindControl("IterCheckBox"), CheckBox)
                        iterCheckBox.Attributes.Add("onclick", "document.getElementById('" & Me.IdReferenteInternoTextBox.ClientID & "').value=" & id & ";document.getElementById('" & Me.secondoReferenteInternoIterChechedButton.ClientID & "').click();")

                        Dim avviabileIter As Boolean = Me.IterAvviabile(referente.Id)
                        If iterCheckBoxEnabled Then
                            iterCheckBox.Enabled = avviabileIter
                        End If


                        If iterCheckBoxEnabled Then
                            iterCheckBox.ToolTip = If(avviabileIter, "", "Per il destinatario " & descrizione & " l'iter non è avviabile")
                        Else
                            iterCheckBox.ToolTip = "Lo stato dell'iter non è modificabile."
                        End If

                        Dim tipoRicezionePec As Integer = 18

                        If Me.TipoRicezioneInvioComboBox.SelectedItem.Value <> 0 Then
                            If CInt(Me.TipoRicezioneInvioComboBox.SelectedItem.Value) = tipoRicezionePec Then
                                iterCheckBox.Checked = referente.Iter
                            Else
                                iterCheckBox.Checked = referente.Iter
                            End If
                        Else
                            iterCheckBox.Checked = referente.Iter
                        End If

                    End If

                    Dim inviaEmailEnabled As Boolean = Me.CheckEmail(referente.Email)

                    Dim inviaEmailCheckBox As CheckBox = CType(e.Item.FindControl("InviaEmailCheckBox"), CheckBox)
                    If inviaEmailEnabled Then
                        inviaEmailCheckBox.Attributes.Add("onclick", "document.getElementById('" & Me.IdReferenteInternoTextBox.ClientID & "').value=" & id & ";document.getElementById('" & Me.secondoReferenteInternoInviaEmailChechedButton.ClientID & "').click();")
                    Else
                        inviaEmailCheckBox.ToolTip = "Al destinatario " & descrizione & " non è possibile inviare l'email. Necessario indirizzo valido"
                    End If
                    inviaEmailCheckBox.Checked = referente.InviaEmail
                    inviaEmailCheckBox.Enabled = inviaEmailEnabled

                Case ParsecPro.TipoRegistrazione.Interna
                    'Niente
                Case ParsecPro.TipoRegistrazione.Partenza
                    'Niente
            End Select
        End If
    End Sub

    'verifica la correttezza di un indirizzo mail
    Private Function CheckEmail(ByVal Indirizzo As String) As Boolean
        If String.IsNullOrEmpty(Indirizzo) Then
            Return False
        End If
        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
        Dim m = emailRegex.Match(Indirizzo)
        Return m.Success
    End Function

    'Metodo PreRender della griglia SecondoReferentiInterniGridView: setta la visibilità dei check associati alla griglia dei secondi referenti.
    Protected Sub SecondoReferentiInterniGridView_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles SecondoReferentiInterniGridView.PreRender
        For Each col As GridColumn In SecondoReferentiInterniGridView.MasterTableView.Columns
            If col.UniqueName = "CheckBoxTemplateColumn" Then
                col.Visible = (Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo)
            End If
            If col.UniqueName = "CheckBoxIterTemplateColumn" Then
                col.Visible = (Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo AndAlso Me.IterAttivato)
            End If

            If col.UniqueName = "CheckBoxInviaEmailTemplateColumn" Then
                col.Visible = (Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo)
            End If
        Next
    End Sub

    'Metodo click del check relativo al secondoReferenteInternoChechedButton: setta nei destinatari coloro devono essere informati per conoscenza
    Protected Sub secondoReferenteInternoChechedButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles secondoReferenteInternoChechedButton.Click
        If Not String.IsNullOrEmpty(Me.IdReferenteInternoTextBox.Text) Then
            Dim id As Integer = CInt(Me.IdReferenteInternoTextBox.Text)
            Dim referente As ParsecPro.IReferente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
            referente.PerConoscenza = Not referente.PerConoscenza
        End If
    End Sub

    'Metodo click del check relativo al secondoReferenteInternoIterChechedButton:  setta nei destinatari coloro che fanno parte dell'iter
    Protected Sub secondoReferenteInternoIterChechedButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles secondoReferenteInternoIterChechedButton.Click
        If Not String.IsNullOrEmpty(Me.IdReferenteInternoTextBox.Text) Then
            Dim id As Integer = CInt(Me.IdReferenteInternoTextBox.Text)
            Dim referente As ParsecPro.IReferente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
            referente.Iter = Not referente.Iter
        End If
    End Sub

    'Metodo click del check relativo al secondoReferenteInternoInviaEmailChechedButton: setta nei destinatari coloro a cui mandare una mail.
    Protected Sub secondoReferenteInternoInviaEmailChechedButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles secondoReferenteInternoInviaEmailChechedButton.Click
        If Not String.IsNullOrEmpty(Me.IdReferenteInternoTextBox.Text) Then
            Dim id As Integer = CInt(Me.IdReferenteInternoTextBox.Text)
            Dim referente As ParsecPro.IReferente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
            referente.InviaEmail = Not referente.InviaEmail
        End If
    End Sub


    'Metodo ItemCommand relativo alla griglia SecondoReferentiInterniGridView: esegue i vari comandi attivabili dalla griglia.
    Protected Sub SecondoReferentiInterniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles SecondoReferentiInterniGridView.ItemCommand
        Dim referente As ParsecPro.IReferente = Nothing
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Select Case e.CommandName
            Case "Delete"
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        referente = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
                        Me.Destinatari.Remove(referente)
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        referente = Me.Mittenti.Where(Function(c) c.Id = id).FirstOrDefault
                        Me.Mittenti.Remove(referente)
                End Select

                'RIMUOVO L'UTENTE O IL GRUPPO DI VISIBILITA'
                Me.EliminaGruppoUtenteVisibilita(referente.Id)

        End Select
    End Sub

    'Metodo che elimina un elemento (Utente o Gruppo) dall'elenco della Visibilità
    Private Sub EliminaGruppoUtenteVisibilita(ByVal idReferente As Integer)
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim struttura As ParsecAdmin.Struttura = strutture.GetQuery.Where(Function(c) c.Id = idReferente).FirstOrDefault
        If Not struttura Is Nothing Then
            Dim entita As ParsecAdmin.VisibilitaDocumento = Nothing
            If struttura.IdGerarchia = 400 Then 'PERSONA
                If struttura.IDUtente.HasValue Then
                    entita = Me.Visibilita.Where(Function(c) c.IdEntita = struttura.IDUtente And c.TipoEntita = ParsecAdmin.TipoEntita.Utente).FirstOrDefault
                End If
            Else

                Dim idStruttura = struttura.Id
                Dim gruppi As New ParsecAdmin.GruppoRepository(strutture.Context)

                Dim gruppo = (From s In strutture.GetQuery
                        Join g In gruppi.GetQuery
                        On s.IdGruppo Equals g.Id
                        Where s.Id = idStruttura
                        Select g).FirstOrDefault
                If Not gruppo Is Nothing Then
                    entita = Me.Visibilita.Where(Function(c) c.IdEntita = gruppo.Id And c.TipoEntita = ParsecAdmin.TipoEntita.Gruppo).FirstOrDefault
                End If
            End If
            If Not entita Is Nothing Then
                Me.Visibilita.Remove(entita)
            End If
        End If
        strutture.Dispose()
    End Sub

    'Metodo Click per trovare un elemento da IndiceIPA e associarlo nella registrazione di protocollo. Lancia la maschera di ricerca RicercaIndiceIpaPage.aspx
    Protected Sub TrovaReferenteEsternoIpaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaReferenteEsternoIpaImageButton.Click
        Dim pageUrl As String = "~/UI/Protocollo/pages/search/RicercaIndiceIpaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaNuovoReferenteEsternoImageButton.ClientID)
        Select Case Me.TipologiaRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                queryString.Add("tipoReferente", "Mitt")
            Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                queryString.Add("tipoReferente", "Dest")
        End Select
        ParsecUtility.Utility.ShowPopup(pageUrl, 850, 650, queryString, False)
    End Sub

    'Metodo Click relativo a TrovaReferenteEsternoImageButton per trovare un elmento dalla Rubrica e associarlo nella registrazione di protocollo.
    Protected Sub TrovaReferenteEsternoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaReferenteEsternoImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaReferenteEsternoImageButton.ClientID)
        queryString.Add("mode", "search")

        Dim parametriPagina As New Hashtable

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        If String.IsNullOrEmpty(Me.RubricaComboBox.SelectedValue) Then
            parametriPagina.Add("Filtro", Me.RubricaComboBox.Text)
            If Not String.IsNullOrEmpty(Me.RubricaComboBox.Text) Then
                Dim rubrica As New ParsecAdmin.RubricaRepository
                Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.RubricaComboBox.Text) And c.LogStato Is Nothing And c.InRubrica = True).ToList
                If struttureEsterne.Count = 1 Then
                    Me.AggiornaReferenteEsterno(struttureEsterne(0))
                    Me.RubricaComboBox.Text = String.Empty
                    ParsecUtility.SessionManager.ParametriPagina = Nothing
                Else
                    ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
                End If
                rubrica.Dispose()
            Else
                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
            End If
        Else
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim struttureEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.RubricaComboBox.SelectedValue)).FirstOrDefault
            rubrica.Dispose()
            If Not struttureEsterna Is Nothing Then
                parametriPagina.Add("Filtro", struttureEsterna.Denominazione)
            End If
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
        End If


    End Sub

    'Aggiorna le informazioni del Referente Esterno. Lanciato da TrovaReferenteEsternoImageButton.Click
    Private Sub AggiornaReferenteEsterno(ByVal strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo)
        Dim referenteEsterno As ParsecPro.IReferente = Nothing
        Select Case Me.TipologiaRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                referenteEsterno = New ParsecPro.Mittente
            Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                referenteEsterno = New ParsecPro.Destinatario
        End Select
        referenteEsterno.Interno = False
        referenteEsterno.Rubrica = strutturaEsterna.InRubrica
        referenteEsterno.PerConoscenza = False
        referenteEsterno.Esistente = True
        referenteEsterno.Id = strutturaEsterna.Id
        referenteEsterno.Codice = strutturaEsterna.Codice
        referenteEsterno.Cognome = strutturaEsterna.Denominazione
        referenteEsterno.Nome = strutturaEsterna.Nome
        referenteEsterno.Indirizzo = strutturaEsterna.Indirizzo
        referenteEsterno.Cap = strutturaEsterna.CAP
        referenteEsterno.Citta = strutturaEsterna.Comune
        referenteEsterno.Provincia = strutturaEsterna.Provincia
        referenteEsterno.CodiceFiscalePartitaIva = strutturaEsterna.CodiceFiscale
        referenteEsterno.Telefono = strutturaEsterna.Telefono
        referenteEsterno.Fax = strutturaEsterna.Fax
        referenteEsterno.Email = strutturaEsterna.Email
        referenteEsterno.CodiceIPA = strutturaEsterna.CodiceIPA
        referenteEsterno.LivelloGerarchiaIPA = strutturaEsterna.LivelloGerarchiaIPA
        referenteEsterno.NodoIpaXml = strutturaEsterna.InfoXML
        referenteEsterno.Tipologia = strutturaEsterna.Tipologia
        Dim descrizione As String = referenteEsterno.Descrizione
        Dim id As Integer = referenteEsterno.Id
        Dim exist As Boolean
        Select Case Me.TipologiaRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                exist = Not Me.Mittenti.Where(Function(c) c.Descrizione = descrizione OrElse c.Id = id).FirstOrDefault Is Nothing
            Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                exist = Not Me.Destinatari.Where(Function(c) c.Descrizione = descrizione OrElse c.Id = id).FirstOrDefault Is Nothing
        End Select
        If Not exist Then
            Select Case Me.TipologiaRegistrazione
                Case ParsecPro.TipoRegistrazione.Arrivo
                    Me.Mittenti.Add(referenteEsterno)
                Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                    Me.Destinatari.Add(referenteEsterno)
            End Select
        Else
            ParsecUtility.Utility.MessageBox("Il referente selezionato è già presente", False)
        End If
    End Sub

    'Metodo Click relativo a AggiungiReferenteEsternoImageButton per aggiungere un referente esterno alla registrazione di protocollo.
    Protected Sub AggiungiReferenteEsternoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiReferenteEsternoImageButton.Click
        If Not String.IsNullOrEmpty(Me.RubricaComboBox.SelectedValue) Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim strutturaEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.RubricaComboBox.SelectedValue)).FirstOrDefault
            If Not strutturaEsterna Is Nothing Then
                Me.AggiornaReferenteEsterno(strutturaEsterna)
            End If
            Me.RubricaComboBox.Text = String.Empty
            Me.RubricaComboBox.SelectedValue = String.Empty
        End If
    End Sub

    'Metodo Click relativo a AggiornaReferenteEsternoImageButton per aggiornare un referente esterno per aggiungere un referente esterno alla registrazione di protocollo.
    Protected Sub AggiornaReferenteEsternoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaReferenteEsternoImageButton.Click
        If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
            Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
            Me.AggiornaReferenteEsterno(strutturaEsterna)
            Me.RubricaComboBox.Text = String.Empty
            ParsecUtility.SessionManager.Rubrica = Nothing
            If Me.ModalitaAperturaPopup = ModalitaPopup.None Then

            End If
        End If
    End Sub

    'Metodo Click relativo a TrovaPrimoReferenteInternoImageButton per trovare il referente dall'organigramma ed aggiungerlo alla registrazione del Protocollo nei Mittenti.
    Protected Sub TrovaPrimoReferenteInternoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaPrimoReferenteInternoImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaPrimoReferenteInternoImageButton.ClientID)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("idModulo", 2)
        parametriPagina.Add("idUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 1) 'multipla
        parametriPagina.Add("livelliSelezionabili", "100,200,300,400")
        parametriPagina.Add("ultimoLivelloStruttura", "400")
        parametriPagina.Add("Filtro", Me.FiltroDenominazioneTextBox.Text)
        parametriPagina.Add("ApplicaAbilitazioni", False)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)

    End Sub

    'Metodo Click relativo a AggiornaPrimoReferenteInternoImageButton per aggiornare il primo referente. Richiamato da TrovaPrimoReferenteInternoImageButton.Click
    Protected Sub AggiornaPrimoReferenteInternoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaPrimoReferenteInternoImageButton.Click

        Dim messaggio As New System.Text.StringBuilder

        If Not Session("SelectedStructures") Is Nothing Then
            Dim referenteInterno As ParsecPro.IReferente = Nothing
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            For Each referente As ParsecAdmin.StrutturaAbilitata In struttureSelezionate
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        referenteInterno = New ParsecPro.Mittente(referente.Id, True)
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        referenteInterno = New ParsecPro.Destinatario(referente.Id, True)
                End Select
                Dim descrizione As String = referenteInterno.Descrizione
                Dim id As String = referenteInterno.Id
                Dim exist As Boolean

                exist = Not Me.Mittenti.Where(Function(c) c.Id = id And c.Interno = True).FirstOrDefault Is Nothing
                exist = exist Or Not Me.Destinatari.Where(Function(c) c.Id = id And c.Interno = True).FirstOrDefault Is Nothing
                If Not exist Then
                    Select Case Me.TipologiaRegistrazione
                        Case ParsecPro.TipoRegistrazione.Arrivo
                            Me.Mittenti.Add(referenteInterno)
                        Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna

                            If Me.IterAttivato Then
                                Dim avviabile = Me.IterAvviabile(referente.Id)

                                Dim parametri As New ParsecAdmin.ParametriRepository
                                Dim iterAutomatico As ParsecAdmin.Parametri = parametri.GetByName("IterAutomatico", ParsecAdmin.TipoModulo.PRO)
                                Dim messaggioIter As ParsecAdmin.Parametri = parametri.GetByName("MessaggioIter", ParsecAdmin.TipoModulo.PRO)
                                parametri.Dispose()

                                If Not iterAutomatico Is Nothing Then
                                    referenteInterno.Iter = avviabile And CBool(iterAutomatico.Valore) ' And Me.TipoRicezioneInvioComboBox.SelectedItem.Text.ToLower = "pec"
                                End If
                                If Not Me.DestinatarioTaskCorrente Is Nothing Then
                                    If Me.DestinatarioTaskCorrente.Id = referenteInterno.Id Then
                                        referenteInterno.Iter = True
                                    End If
                                End If

                                If Me.AbilitazioneUtenteCorrente <> AbilitazioneUtente.Movimentazione Then
                                    If Not messaggioIter Is Nothing Then
                                        If CBool(messaggioIter.Valore) Then
                                            If Not avviabile Then
                                                messaggio.AppendLine(String.Format("Per il destinatario {0} non è avviabile l'iter!", referente.Descrizione))
                                            ElseIf Not referenteInterno.Iter Then
                                                messaggio.AppendLine(String.Format("Per il destinatario {0} non è stato avviato l'iter!", referente.Descrizione))
                                            End If
                                        End If
                                    End If
                                End If

                            End If

                            Me.Destinatari.Add(referenteInterno)
                    End Select

                    'AGGIUNGO L'UTENTE O IL GRUPPO DI VISIBILITA'
                    If referente.IdGerarchia = 400 Then  'PERSONA
                        If referente.IdUtente.HasValue Then
                            Dim utenti As New ParsecAdmin.UserRepository
                            Dim utente As ParsecAdmin.Utente = utenti.GetUserById(referente.IdUtente).FirstOrDefault
                            If Not utente Is Nothing Then
                                Me.AggiungiUtenteVisibilita(utente)
                            End If

                        End If
                    Else
                        Me.AggiungiGruppoDefault(referente.Id)
                    End If

                Else
                    ParsecUtility.Utility.MessageBox("Il referente selezionato è già presente", False)
                End If
            Next
            Session("SelectedStructures") = Nothing


            If messaggio.Length > 0 Then
                ParsecUtility.Utility.MessageBox(messaggio.ToString, False)
            End If

        End If
    End Sub

    'Metodo Click relativo a TrovaSecondoReferenteInternoImageButton per trovare il referente interno dall'organigramma e lo aggiunge alla lista dei destinatari
    Protected Sub TrovaSecondoReferenteInternoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaSecondoReferenteInternoImageButton.Click

        Dim tipoSelezione As Integer = 1 'multipla

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaSecondoReferenteInternoImageButton.ClientID)

        Dim parametriPagina As New Hashtable

        parametriPagina.Add("idModulo", 2)
        parametriPagina.Add("idUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", tipoSelezione) 'multipla
        parametriPagina.Add("livelliSelezionabili", "100,200,300,400")
        parametriPagina.Add("ultimoLivelloStruttura", "400")
        parametriPagina.Add("Filtro", Me.FiltroSecondoReferenteInternoTextBox.Text)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    'Metodo Click relativo a AggiornaSecondoReferenteInternoImageButton per aggiornare il referente interno associandolo alla griglia dei destinatari.
    Protected Sub AggiornaSecondoReferenteInternoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSecondoReferenteInternoImageButton.Click

        Dim messaggio As New System.Text.StringBuilder

        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim referenteInterno As ParsecPro.IReferente = Nothing
            For Each referente As ParsecAdmin.StrutturaAbilitata In struttureSelezionate
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        referenteInterno = New ParsecPro.Destinatario(referente.Id, True)
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        referenteInterno = New ParsecPro.Mittente(referente.Id, True)
                End Select
                Dim descrizione As String = referenteInterno.Descrizione
                Dim id As String = referenteInterno.Id
                Dim exist As Boolean

                exist = Not Me.Mittenti.Where(Function(c) c.Id = id And c.Interno = True).FirstOrDefault Is Nothing
                exist = exist Or Not Me.Destinatari.Where(Function(c) c.Id = id And c.Interno = True).FirstOrDefault Is Nothing

                If Not exist Then
                    Select Case Me.TipologiaRegistrazione
                        Case ParsecPro.TipoRegistrazione.Arrivo
                            If Me.IterAttivato Then

                                'SE STO PROTOCOLLANDO UNA FATTURA L'ITER E' SEMPRE ATTIVO
                                If VerificaIterFattura() Then
                                    Dim avviabile = Me.IterAvviabile(referente.Id)
                                    referenteInterno.Iter = avviabile
                                    If Not avviabile Then
                                        messaggio.AppendLine(String.Format("Per il destinatario {0} non è stato avviato l'iter!", referente.Descrizione))
                                    End If

                                Else
                                    Dim avviabile = Me.IterAvviabile(referente.Id)

                                    Dim parametri As New ParsecAdmin.ParametriRepository
                                    Dim iterAutomatico As ParsecAdmin.Parametri = parametri.GetByName("IterAutomatico", ParsecAdmin.TipoModulo.PRO)
                                    Dim messaggioIter As ParsecAdmin.Parametri = parametri.GetByName("MessaggioIter", ParsecAdmin.TipoModulo.PRO)
                                    parametri.Dispose()

                                    If Not iterAutomatico Is Nothing Then
                                        Select Case iterAutomatico.Valore
                                            Case "0"
                                                referenteInterno.Iter = False
                                            Case "1"
                                                referenteInterno.Iter = avviabile And (Me.TipoRicezioneInvioComboBox.SelectedItem.Text.ToLower = "pec" Or Me.TipoRicezioneInvioComboBox.SelectedItem.Text.ToLower = "e-mail")
                                            Case "2"
                                                referenteInterno.Iter = avviabile
                                        End Select
                                    End If

                                    If Not Me.DestinatarioTaskCorrente Is Nothing Then
                                        If Me.DestinatarioTaskCorrente.Id = referenteInterno.Id Then
                                            referenteInterno.Iter = True
                                        End If
                                    End If

                                    If Me.AbilitazioneUtenteCorrente <> AbilitazioneUtente.Movimentazione Then
                                        If Not messaggioIter Is Nothing Then
                                            If CBool(messaggioIter.Valore) Then
                                                If Not avviabile Then
                                                    messaggio.AppendLine(String.Format("Per il destinatario {0} non è avviabile l'iter!", referente.Descrizione))
                                                ElseIf Not referenteInterno.Iter Then
                                                    messaggio.AppendLine(String.Format("Per il destinatario {0} non è stato avviato l'iter!", referente.Descrizione))
                                                End If
                                            End If
                                        End If
                                    End If
                                End If

                            End If


                            If Not String.IsNullOrEmpty(referenteInterno.Email.Trim) Then
                                Dim parametri As New ParsecAdmin.ParametriRepository
                                Dim parametro = parametri.GetByName("SelezionaAutomaticamenteInvioEmailDestinatariInterni", ParsecAdmin.TipoModulo.PRO)
                                parametri.Dispose()
                                If Not parametro Is Nothing Then
                                    'MARCA IL DESTINATARIO INTERNO PER LA RICEZIONE DI UNA EMAIL DI AVVENUTA PROTOCOLLAZIONE
                                    If parametro.Valore = "1" Then
                                        referenteInterno.InviaEmail = True
                                    End If
                                End If
                            End If


                            Me.Destinatari.Add(referenteInterno)
                        Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                            Me.Mittenti.Add(referenteInterno)
                    End Select

                    'AGGIUNGO L'UTENTE O IL GRUPPO DI VISIBILITA'
                    If referente.IdGerarchia = 400 Then  'PERSONA
                        If referente.IdUtente.HasValue Then
                            Dim utenti As New ParsecAdmin.UserRepository
                            Dim utente As ParsecAdmin.Utente = utenti.GetUserById(referente.IdUtente).FirstOrDefault
                            If Not utente Is Nothing Then
                                Me.AggiungiUtenteVisibilita(utente)
                            End If

                        End If
                    Else
                        Me.AggiungiGruppoDefault(referente.Id)
                    End If

                Else
                    ParsecUtility.Utility.MessageBox("Il referente selezionato è già presente", False)
                End If
            Next
            Session("SelectedStructures") = Nothing
        End If

        If messaggio.Length > 0 Then
            ParsecUtility.Utility.MessageBox(messaggio.ToString, False)
        End If

    End Sub

#End Region

#Region "GESTIONE OGGETTO"

    'Metodo Click relativo a TrovaOggettoImageButton per trovare l'Oggetto ed associarlo alla registrazione di protocollo. fa partire la ricerca tramite la pagina OggettoPage.aspx.
    Protected Sub TrovaOggettoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaOggettoImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/OggettoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaOggettoImageButton.ClientID)
        queryString.Add("Mode", "1")
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("Filtro", Me.OggettoTextBox.Text)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 915, 630, queryString, False)
    End Sub

    'Metodo WebMethod per la selezione degli oggetti dalla Combo degli oggetti
    <WebMethod()> _
    Public Shared Function GetOggetti(ByVal context As RadComboBoxContext) As RadComboBoxData
        Dim oggetti As New ParsecPro.OggettiRepository

        Dim data = oggetti.Where(Function(c) c.Contenuto.Contains(context.Text) And c.Contenuto <> "").OrderBy(Function(c) c.Contenuto).ToList
        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            Dim item = data.ElementAt(i)
            itemData.Text = item.Contenuto
            itemData.Value = item.Id
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function

    'Metodo aggiorna l'Oggetto: richiamato da AggiungiOggettoImageButton.Click. Aggiorna eventualmente i riferimenti alla classificazione  e l'elenco dei referenti interni se l'oggetto selezioanato è relazionato a delle strutture.
    Private Sub AggiornaOggetto(ByVal oggettoSelezionato As ParsecPro.Oggetto)

        'AGGIUNGO L'OGGETTO
        Me.OggettoTextBox.Text = oggettoSelezionato.Contenuto

        'AGGIUNGO LA CLASSIFICAZIONE
        If oggettoSelezionato.IdClassificazione.HasValue Then
            Me.IdClassificazioneTextBox.Text = oggettoSelezionato.IdClassificazione.Value.ToString
            Dim classificazioni As New ParsecAdmin.TitolarioClassificazioneRepository
            Me.ClassificazioneTextBox.Text = oggettoSelezionato.Classificazione
            classificazioni.Dispose()
        End If

        Dim elencoStruttura As New List(Of ParsecAdmin.Struttura)
        Dim strutture As New ParsecAdmin.StructureRepository
        For Each struttura In oggettoSelezionato.Strutture
            Dim idStruttura = struttura.IdStruttura
            Dim strutturaCorrente = strutture.Where(Function(c) c.Id = idStruttura).FirstOrDefault
            If Not strutturaCorrente Is Nothing Then
                elencoStruttura.Add(strutturaCorrente)
            End If
        Next
        strutture.Dispose()

        'AGGIUNGO I REFERENTI INTERNI (UFFICIO SETTORE) E LA VISIBILITA'
        Me.AggiornaReferentiInterni(elencoStruttura)

    End Sub

    'Metodo che aggiorna la lista dei referenti interni: richiamato da AggiornaOggetto
    Private Sub AggiornaReferentiInterni(ByVal strutture As List(Of ParsecAdmin.Struttura))
        Dim messaggio As New System.Text.StringBuilder

        Dim referenteInterno As ParsecPro.IReferente = Nothing
        For Each referente In strutture
            Select Case Me.TipologiaRegistrazione
                Case ParsecPro.TipoRegistrazione.Arrivo
                    referenteInterno = New ParsecPro.Destinatario(referente.Id, True)
                Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                    referenteInterno = New ParsecPro.Mittente(referente.Id, True)
            End Select
            Dim descrizione As String = referenteInterno.Descrizione
            Dim id As String = referenteInterno.Id

            Dim exist As Boolean = Not Me.Mittenti.Where(Function(c) c.Id = id And c.Interno = True).FirstOrDefault Is Nothing
            exist = exist Or Not Me.Destinatari.Where(Function(c) c.Id = id And c.Interno = True).FirstOrDefault Is Nothing

            If Not exist Then
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        If Me.IterAttivato Then

                            'SE STO PROTOCOLLANDO UNA FATTURA L'ITER E' SEMPRE ATTIVO
                            If VerificaIterFattura() Then
                                Dim avviabile = Me.IterAvviabile(referente.Id)
                                referenteInterno.Iter = avviabile
                                If Not avviabile Then
                                    messaggio.AppendLine(String.Format("Per il destinatario {0} non è stato avviato l'iter!", referente.Descrizione))
                                End If

                            Else
                                Dim avviabile = Me.IterAvviabile(referente.Id)

                                Dim parametri As New ParsecAdmin.ParametriRepository
                                Dim iterAutomatico As ParsecAdmin.Parametri = parametri.GetByName("IterAutomatico", ParsecAdmin.TipoModulo.PRO)
                                Dim messaggioIter As ParsecAdmin.Parametri = parametri.GetByName("MessaggioIter", ParsecAdmin.TipoModulo.PRO)
                                parametri.Dispose()

                                If Not iterAutomatico Is Nothing Then
                                    Select Case iterAutomatico.Valore
                                        Case "0"
                                            referenteInterno.Iter = False
                                        Case "1"
                                            referenteInterno.Iter = avviabile And Me.TipoRicezioneInvioComboBox.SelectedItem.Text.ToLower = "pec"
                                        Case "2"
                                            referenteInterno.Iter = avviabile
                                    End Select

                                End If
                                If Not Me.DestinatarioTaskCorrente Is Nothing Then
                                    If Me.DestinatarioTaskCorrente.Id = referenteInterno.Id Then
                                        referenteInterno.Iter = True
                                    End If
                                End If

                                If Me.AbilitazioneUtenteCorrente <> AbilitazioneUtente.Movimentazione Then
                                    If Not messaggioIter Is Nothing Then
                                        If CBool(messaggioIter.Valore) Then
                                            If Not avviabile Then
                                                messaggio.AppendLine(String.Format("Per il destinatario {0} non è avviabile l'iter!", referente.Descrizione))
                                            ElseIf Not referenteInterno.Iter Then
                                                messaggio.AppendLine(String.Format("Per il destinatario {0} non è stato avviato l'iter!", referente.Descrizione))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        Me.Destinatari.Add(referenteInterno)
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        Me.Mittenti.Add(referenteInterno)
                End Select

                '**************************************************************************************************
                'AGGIUNGO L'UTENTE O IL GRUPPO DI VISIBILITA'
                If referente.IdGerarchia = 400 Then  'PERSONA
                    If referente.IDUtente.HasValue Then
                        Dim utenti As New ParsecAdmin.UserRepository
                        Dim utente As ParsecAdmin.Utente = utenti.GetUserById(referente.IDUtente).FirstOrDefault
                        If Not utente Is Nothing Then
                            Me.AggiungiUtenteVisibilita(utente)
                        End If

                    End If
                Else
                    Me.AggiungiGruppoDefault(referente.Id)
                End If
                '**************************************************************************************************

            Else
                ParsecUtility.Utility.MessageBox("Il referente selezionato è già presente", False)
            End If
        Next

        If messaggio.Length > 0 Then
            ParsecUtility.Utility.MessageBox(messaggio.ToString, False)
        End If
    End Sub

    'Metodo Click relativo a AggiungiOggettoImageButton per aggiungere l'Oggetto. Richiama il metodo AggiornaOggetto
    Protected Sub AggiungiOggettoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiOggettoImageButton.Click

        If Not String.IsNullOrEmpty(Me.OggettiComboBox.Text) Then
            Try
                Dim oggetti As New ParsecPro.OggettiRepository
                Dim oggetto = oggetti.GetById(CInt(Me.OggettiComboBox.SelectedValue))
                If Not oggetto Is Nothing Then
                    Me.AggiornaOggetto(oggetto)
                End If
            Catch ex As Exception
            End Try
            Me.OggettiComboBox.Text = String.Empty
        End If
    End Sub

    'Metodo Click relativo a AggiornaOggettoImageButton per aggiornare l'Oggetto. Richiama il metodo AggiornaOggetto
    Protected Sub AggiornaOggettoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaOggettoImageButton.Click
        If Not Session("OggettoSelezionato") Is Nothing Then
            Dim oggettoSelezionato As ParsecPro.Oggetto = Session("OggettoSelezionato")
            Session("OggettoSelezionato") = Nothing
            Me.AggiornaOggetto(oggettoSelezionato)
        End If
    End Sub

#End Region

#Region "GESTIONE INSERIMENTO VELOCE NUOVO REFERENTE"

    'Metodo Click relativo a AggiornaNuovoReferenteEsternoImageButton per Aggiornare un nuovo Mittente
    Protected Sub AggiornaNuovoReferenteEsternoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaNuovoReferenteEsternoImageButton.Click

        If Not Session("ReferenteInserito") Is Nothing Then
            Dim referente As ParsecPro.IReferente = Session("ReferenteInserito")
            Select Case Me.TipologiaRegistrazione
                Case ParsecPro.TipoRegistrazione.Arrivo

                    Dim mitt = Me.Mittenti.Where(Function(c) c.Id = referente.Id OrElse c.Descrizione = referente.Descrizione).FirstOrDefault
                    If Not mitt Is Nothing Then

                        Dim index = Me.Mittenti.IndexOf(mitt)
                        Me.Mittenti.Remove(mitt)

                        Me.Mittenti.Insert(index, referente)

                    Else

                        If Session("IdReferenteDaSostituire") Is Nothing Then
                            Me.Mittenti.Add(referente)
                        Else
                            Dim mittDaSostituire = Me.Mittenti.Where(Function(c) c.Id = Session("IdReferenteDaSostituire")).FirstOrDefault
                            If Not mittDaSostituire Is Nothing Then
                                Dim index = Me.Mittenti.IndexOf(mittDaSostituire)
                                Me.Mittenti.Remove(mittDaSostituire)
                                Me.Mittenti.Insert(index, referente)
                            Else
                                Me.Mittenti.Add(referente)
                            End If
                            Session("IdReferenteDaSostituire") = Nothing
                        End If

                    End If

                Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna

                    Dim dest = Me.Destinatari.Where(Function(c) c.Id = referente.Id OrElse c.Descrizione = referente.Descrizione).FirstOrDefault
                    If Not dest Is Nothing Then

                        Dim index = Me.Destinatari.IndexOf(dest)
                        Me.Destinatari.Remove(dest)
                        Me.Destinatari.Insert(index, referente)

                    Else

                        If Session("IdReferenteDaSostituire") Is Nothing Then
                            Me.Destinatari.Add(referente)
                        Else
                            Dim mittDaSostituire = Me.Destinatari.Where(Function(c) c.Id = Session("IdReferenteDaSostituire")).FirstOrDefault
                            If Not mittDaSostituire Is Nothing Then
                                Dim index = Me.Destinatari.IndexOf(mittDaSostituire)
                                Me.Destinatari.Remove(mittDaSostituire)
                                Me.Destinatari.Insert(index, referente)
                            Else
                                Me.Destinatari.Add(referente)
                            End If
                            Session("IdReferenteDaSostituire") = Nothing
                        End If


                    End If
            End Select
            Session("ReferenteInserito") = Nothing
        End If

        If Not Session("ReferentiInseriti") Is Nothing Then
            Dim referenti As List(Of ParsecPro.IReferente) = Session("ReferentiInseriti")

            For Each referente In referenti
                Dim descrizione As String = referente.Descrizione
                Select Case Me.TipologiaRegistrazione
                    Case ParsecPro.TipoRegistrazione.Arrivo
                        If Me.Mittenti.Where(Function(c) c.Descrizione = descrizione).FirstOrDefault Is Nothing Then
                            Me.Mittenti.Add(referente)
                        End If
                    Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                        If Me.Destinatari.Where(Function(c) c.Descrizione = descrizione).FirstOrDefault Is Nothing Then
                            Me.Destinatari.Add(referente)
                        End If
                End Select
            Next
            Session("ReferentiInseriti") = Nothing
        End If

    End Sub

    'Metodo Click relativo a AggiornaNuovoReferenteEsternoImageButton per Aggiornare un nuovo mittente esterno. Inserimento veloce.
    Protected Sub AggiungiNuovoReferenteEsternoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiNuovoReferenteEsternoImageButton.Click
        Dim pageUrl As String = "InserimentoReferenteEsterno.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaNuovoReferenteEsternoImageButton.ClientID)
        Select Case Me.TipologiaRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                queryString.Add("tipoReferente", "Mitt")
            Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                queryString.Add("tipoReferente", "Dest")
        End Select

        Dim parametriPagina As New Hashtable

        Dim lista As New List(Of ParsecPro.IReferente)
        Select Case Me.TipologiaRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                For Each mitt In Me.Mittenti
                    lista.Add(mitt)
                Next
            Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                For Each dest In Me.Destinatari
                    lista.Add(dest)
                Next
        End Select

        parametriPagina.Add("ListaReferenti", lista)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

        ParsecUtility.Utility.ShowRadWindow(pageUrl, "AggiungiReferenteEsternoRadWindow", queryString, False)
    End Sub

#End Region

#Region "GESTIONE ALLEGATI"

    'Metodo Click relativo a AggiungiDocumentoImageButton per Aggiungere un nuovo documento nella griglia degli allegati.
    Protected Sub AggiungiDocumentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoImageButton.Click

        If Me.AllegatoUpload.UploadedFiles.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Per inserire un allegato, è necessario specificarne il file relativo!", False)
            Exit Sub
        End If

        Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles.Cast(Of Telerik.Web.UI.UploadedFile).FirstOrDefault

        If String.IsNullOrEmpty(Me.DescrizioneDocumentoTextBox.Text.Trim) Then
            Me.DescrizioneDocumentoTextBox.Text = file.FileName
        End If


        If file.FileName <> "" Then

            Dim nomefile As String = Guid.NewGuid.ToString

            Dim filenameTemp As String = nomefile & "_" & file.FileName
            Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

            If Not IO.Directory.Exists(pathRootTemp) Then
                IO.Directory.CreateDirectory(pathRootTemp)
            End If

            Dim pathDownload As String = pathRootTemp & filenameTemp
            Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            Dim filename As String = file.FileName

            file.SaveAs(pathDownload)

            Dim allegato As New ParsecPro.Allegato
            allegato.NomeFile = filename
            allegato.NomeFileTemp = filenameTemp
            If Me.DocumentoPrimarioRadioButton.Checked Then
                allegato.IdTipologiaDocumento = 1
                allegato.DescrizioneTipologiaDocumento = "Primario"
            Else
                allegato.IdTipologiaDocumento = 0
                allegato.DescrizioneTipologiaDocumento = "Allegato"
            End If
            allegato.Oggetto = Me.DescrizioneDocumentoTextBox.Text
            allegato.PercorsoRoot = pathRoot
            allegato.PercorsoRootTemp = pathRootTemp
            allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
            allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

            If filename.ToLower.EndsWith(".pdf") Then
                allegato.Scansionato = True
            End If

            If Me.Allegati.Where(Function(c) c.ImprontaEsadecimale = allegato.ImprontaEsadecimale).FirstOrDefault Is Nothing Then
                Me.Allegati.Add(allegato)
            Else
                ParsecUtility.Utility.MessageBox("Il file selezionato è stato già allegato!", False)
            End If

            Me.DescrizioneDocumentoTextBox.Text = String.Empty
        End If

    End Sub

    'Metodo che effettua una copia della Lista degli Allegati restituendola.
    Private Function CopiaAllegati(ByVal reg As ParsecPro.Registrazione) As List(Of ParsecPro.Allegato)

        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

        If Not IO.Directory.Exists(pathRootTemp) Then
            IO.Directory.CreateDirectory(pathRootTemp)
        End If

        Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        pathRoot = pathRoot.Remove(pathRoot.Length - 1, 1)

        Dim pathDownload As String
        Dim filenameTemp As String
        Dim allegato As ParsecPro.Allegato = Nothing

        Dim allegati As New List(Of ParsecPro.Allegato)

        For Each all As ParsecPro.Allegato In reg.Allegati


            pathDownload = pathRoot & all.PercorsoRelativo & all.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & all.NomeFile

            If IO.File.Exists(pathDownload) Then


                filenameTemp = Guid.NewGuid.ToString & "_" & all.NomeFile

                allegato = New ParsecPro.Allegato

                allegato.NomeFile = all.NomeFile
                allegato.NomeFileTemp = filenameTemp

                'COPIO IL FILE NELLA CARTELLA TEMPORANEA
                IO.File.Copy(pathDownload, pathRootTemp & filenameTemp, True)


                If all.IdTipologiaDocumento = 1 Then
                    allegato.IdTipologiaDocumento = 1
                    allegato.DescrizioneTipologiaDocumento = "Primario"
                Else
                    allegato.IdTipologiaDocumento = 0
                    allegato.DescrizioneTipologiaDocumento = "Allegato"
                End If

                allegato.Oggetto = all.Oggetto

                allegato.PercorsoRoot = pathRoot
                allegato.PercorsoRootTemp = pathRootTemp
                allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

                If all.NomeFile.ToLower.EndsWith(".pdf") Then
                    allegato.Scansionato = True
                End If
                allegati.Add(allegato)

            End If


        Next
        Return allegati
    End Function

    'Metodo che cancella un documento sia dalla griglia. Se è un allegato temporaneo lo cancella anche dalla cartella Upload.
    Private Sub DeleteDocument(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

        Dim allegato As ParsecPro.Allegato = Me.Allegati.Where(Function(c) c.NomeFile = filename).FirstOrDefault
        If Not allegato Is Nothing Then
            Me.Allegati.Remove(allegato)
            'Se è un allegato temporaneo.
            If allegato.Id = 0 Then
                Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.NomeFileTemp
                If IO.File.Exists(pathDownload) Then
                    IO.File.Delete(pathDownload)
                End If

            End If
        End If
    End Sub

    'Metodo che firma digitalmente un documento.
    Private Sub FirmaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Not utenteCollegato.IdProviderFirma.HasValue Then
            Dim message As New StringBuilder
            message.AppendLine("L'utente corrente non è configurato correttamente!")
            message.AppendLine("E' necessario specificare il provider di firma.")
            ParsecUtility.Utility.MessageBox(message.ToString, False)
            Exit Sub
        End If

        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As ParsecPro.Allegato = Me.Allegati.Where(Function(c) c.NomeFile = filename).FirstOrDefault

        If Not allegato Is Nothing Then

            Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            Dim percorsoRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

            Dim pathRelativeDocument As String = ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocument")
            Dim pathRelativeDocumentiTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocumentiTemp")

            Dim pathFileToSign As String = String.Empty
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If allegato.Id = 0 Then

                Dim fileNameToSign As String = String.Empty
                If String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
                    fileNameToSign = allegato.NomeFileTemp
                Else
                    fileNameToSign = allegato.NomeFileTemp & ".p7m"
                End If


                pathDownload = percorsoRootTemp & fileNameToSign ' allegato.NomeFileTemp
                pathFileToSign = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & pathRelativeDocumentiTemp & fileNameToSign 'allegato.NomeFileTemp
            Else
                Dim fileNameToSign As String = String.Empty
                If String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
                    fileNameToSign = allegato.NomeFile
                Else
                    fileNameToSign = allegato.NomeFileFirmato
                End If

                Dim fileNameOnDisk As String = allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & fileNameToSign
                If percorsoRoot.EndsWith("\") Then
                    percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                End If

                pathDownload = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & fileNameToSign
                pathFileToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), pathRelativeDocument, allegato.PercorsoRelativo.Replace("\", ""), fileNameOnDisk)
            End If

            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then

                Dim certificateId As String = utenteCollegato.IdCertificato
                Dim provider As ParsecAdmin.ProviderType = CType(utenteCollegato.IdProviderFirma, ParsecAdmin.ProviderType)

                Dim signParameters As New ParsecAdmin.SignParameters

                Dim functionName As String = "SignDocument"

                If file.Extension.ToLower = ".p7m" Then
                    functionName = "CoSignDocument"
                End If

                Dim datiInput As New ParsecAdmin.DatiFirma With {
                    .RemotePathToSign = pathFileToSign,
                    .Provider = provider,
                    .FunctionName = functionName,
                    .CertificateId = certificateId
                }

                Dim data As String = signParameters.CreaDataSource(datiInput)


                Dim parametriPagina As New Hashtable

                If file.Extension.ToLower = ".p7m" Then
                    parametriPagina.Add("PathNomeFileFirmato", pathDownload)
                Else
                    If IO.Path.GetExtension(pathDownload).ToLower = ".odt" Then
                        pathDownload = pathDownload.Remove(pathDownload.Length - 4, 4) & ".pdf"
                    End If
                    parametriPagina.Add("PathNomeFileFirmato", String.Format("{0}.p7m", pathDownload))
                End If

                parametriPagina.Add("NomeFile", allegato.NomeFile)

                ParsecUtility.SessionManager.ParametriPagina = parametriPagina

                'UTILIZZO L'APPLET
                If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                    ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaFirmaDigitaleImageButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
                Else
                    'UTILIZZO IL SOCKET  
                    ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.AggiornaFirmaDigitale)
                End If
            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        Else
            'NIENTE
        End If

    End Sub

    'Metodo che scatta dopo che la firma digitale si è conclusa. E' richiamato da FirmaDocumento e da FirmaDigitaleImageButton.Click.
    Private Sub AggiornaFirmaDigitale()

        'SE HO FIRMATO
        Dim successo As Boolean = (Me.signerOutputHidden.Value = "OK")
        If successo Then
            Dim pathNomeFileFirmato As String = ParsecUtility.SessionManager.ParametriPagina("PathNomeFileFirmato")
            Dim filename As String = ParsecUtility.SessionManager.ParametriPagina("NomeFile")

            If IO.File.Exists(pathNomeFileFirmato) Then
                Dim allegato = Me.Allegati.Where(Function(c) c.NomeFile = filename).FirstOrDefault
                If Not allegato Is Nothing Then

                    If IO.Path.GetExtension(allegato.NomeFile).ToLower = ".odt" Then
                        filename = filename.Remove(filename.Length - 4, 4) & ".pdf"
                    End If

                    allegato.NomeFileFirmato = String.Format("{0}.p7m", filename)

                End If
                Me.infoOperazioneHidden.Value = "Firma eseguita correttamente!"
            End If
        Else
            'NASCONDO IL PANNELLO MODALE
            Dim sb As New StringBuilder
            sb.AppendLine("<script>")
            sb.AppendLine("showUI = true;")
            sb.AppendLine("</script>")
            ParsecUtility.Utility.RegisterScript(sb, False)
        End If

        Me.signerOutputHidden.Value = String.Empty
        ParsecUtility.SessionManager.ParametriPagina = Nothing
    End Sub

    'Metodo click che fa scattare l'aggiornamento post Firma
    Protected Sub AggiornaFirmaDigitaleImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaFirmaDigitaleImageButton.Click
        Me.AggiornaFirmaDigitale()
    End Sub

    'Metodo che visualizza un file firmato digitalmente.
    Private Sub VisualizzaDocumentoP7M(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        Dim percorsoRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim nomeFile As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As ParsecPro.Allegato = Me.Allegati.Where(Function(c) c.NomeFile = nomeFile).FirstOrDefault

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

    'Metodo che permette il download del documento allegato. Richiamato da DocumentiGridView.ItemCommand
    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

        Dim allegato As ParsecPro.Allegato = Me.Allegati.Where(Function(c) c.NomeFile = filename).FirstOrDefault
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


                If file.Extension.ToLower = ".eml" Then
                    Try
                        Dim idEmail As Nullable(Of Integer) = Nothing
                        Dim watermark As String = Nothing
                        If Not Me.Registrazione Is Nothing Then

                            Dim registrazioni As New ParsecPro.RegistrazioniRepository
                            Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                            watermark = cliente.Descrizione & " - Prot. n. " & Me.Registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy}", Me.Registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(Me.Registrazione.TipoRegistrazione).ToUpper
                            registrazioni.Dispose()

                            Dim numero As Integer = Me.Registrazione.NumeroProtocollo
                            Dim anno As Integer = Me.Registrazione.DataImmissione.Value.Year

                            Me.VisualizzaEmailControl.ShowPanel()
                            Me.VisualizzaEmailControl.InitUI(file.FullName, numero, anno, watermark)

                        Else

                            idEmail = Me.IdEmail
                            Me.VisualizzaEmailControl.ShowPanel()
                            Me.VisualizzaEmailControl.InitUI(file.FullName, idEmail, watermark)

                        End If

                    Catch ex As Exception
                        ParsecUtility.Utility.MessageBox("Si è verificato un errore durante la lettura dell'email!" & vbCrLf & "Il file verrà aperto con l'applicazione associata.", False)
                        'IN CASO DI ERRORE VISUALIZZO IL FILE IN MODO CLASSICO
                        Session("AttachmentFullName") = file.FullName
                        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                        ParsecUtility.Utility.PageReload(pageUrl, False)
                    End Try

                Else

                    If Me.Request.Browser.Browser.ToLower.Contains("safari") Then

                        Dim pageUrl As String = String.Empty
                        If allegato.Id = 0 Then
                            pageUrl = ParsecAdmin.WebConfigSettings.GetKey("PathHTTPDocumentTemp") & allegato.NomeFileTemp & "?rnd=" & Now.Millisecond.ToString
                        Else
                            pageUrl = ParsecAdmin.WebConfigSettings.GetKey("PathHTTPDocument") & allegato.PercorsoRelativo.Replace("\", "/") & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile & "?rnd=" & Now.Millisecond.ToString
                        End If

                        ParsecUtility.Utility.ShowPopup(pageUrl.Replace("//", "/").Replace("'", "\'"), 900, 700, Nothing, False)

                    Else
                        Session("AttachmentFullName") = file.FullName
                        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                        ParsecUtility.Utility.PageReload(pageUrl, False)
                    End If


                End If

            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        End If
    End Sub

    'Metodo ItemDataBound della griglia DocumentiGridView: setta tooltip ed eventi in base al contenuto delle celle.
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
            If Me.Allegati.Count > 0 Then
                Dim lbl As Label = CType(e.Item.FindControl("NumeratoreLabel"), Label)
                lbl.Text = (e.Item.ItemIndex + 1).ToString
            End If

            If TypeOf dataItem("Firma").Controls(0) Is ImageButton Then

                Dim btnFirma As ImageButton = CType(dataItem("Firma").Controls(0), ImageButton)

                btnFirma.ToolTip = "Apponi firma digitale al documento."
                btnFirma.Attributes.Add("onclick", "showUI=false;")

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
        End If
    End Sub

    'Metodo ItemDataBound della griglia ItemCommand: lancia i comandi dalla griglia degli allegati.
    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Me.DeleteDocument(e.Item)
            Case "Preview"
                Me.DownloadFile(e.Item)
            Case "Firma"
                Me.FirmaDocumento(e.Item)
            Case "SignedPreview"
                Me.VisualizzaDocumentoP7M(e.Item)
        End Select
    End Sub

#End Region

#Region "GESTIONE SCANSIONE"

    'Metodo che lancia la "Registrazione" dei componenti per la Scansione
    Private Sub RegistraScansione()
        Dim script As String = ParsecAdmin.ScannerParameters.RegistraScansione
        Me.MainPage.RegisterComponent(script)
    End Sub

    'Metodo che avvia la scansione
    Private Sub AvviaScansione()

        Dim scanner As New ParsecAdmin.ScannerParameters
        Dim data As String = scanner.CreaDataSource(New ParsecAdmin.DatiCredenziali, New ParsecAdmin.DatiScansione With {.Duplex = Me.FronteRetroCheckBox.Checked, .UseUi = Me.VisualizzaUICheckBox.Checked})

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiScansione(data, Me.ScanUploadButton.ClientID, Me.infoScansioneHidden.ClientID, True, False)
        Else
            'UTILIZZO IL SOCKET  
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, AddressOf Me.NotificaInfoScansione, AddressOf Me.NotificaScansione)
        End If
    End Sub

    'Metodo Click che permette l'avvio della Scansione
    Protected Sub ScansionaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ScansionaImageButton.Click
        Me.AvviaScansione()
    End Sub

    'Notifica le informazioni provenienti dalla scansione. Le associa ad un campo nascosto. Richiamato da AvviaScansione()
    Private Sub NotificaInfoScansione(ByVal data As String)
        Me.infoScansioneHidden.Value = data
    End Sub

    'Notifica la scansione: richiamato da ScanUploadButton.Click()
    Private Sub NotificaScansione()
        If Not String.IsNullOrEmpty(Me.infoScansioneHidden.Value) Then
            Dim ds As New DataSet
            Dim ms As IO.MemoryStream = Nothing


            Dim search As String = "DATIBASE64:"
            Dim pos = Me.infoScansioneHidden.Value.IndexOf(search)
            Dim infoScansione As String = String.Empty
            Try
                If pos <> -1 Then
                    infoScansione = Me.infoScansioneHidden.Value.Substring(pos + search.Length, Me.infoScansioneHidden.Value.Length - pos - search.Length)
                Else
                    infoScansione = Me.infoScansioneHidden.Value
                End If

                'LO SCANNER OLIVETTI IN CASO DI ERRORE SCRIVE NELL'OUTPUT DEL COMPONENTE
                If String.IsNullOrEmpty(infoScansione.Trim) Then
                    Exit Sub
                End If

                ms = New IO.MemoryStream(System.Convert.FromBase64String(infoScansione))
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Si è verificato un errore durante la scansione:" & vbCrLf & infoScansione, False)
                Exit Sub
            End Try

            ds.ReadXml(ms)

            Dim nomeFileDigitalizzato As String = IO.Path.GetFileName(CStr(ds.Tables(0).Rows(0)("NomeFile")))


            Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
            If Not IO.Directory.Exists(pathRootTemp) Then
                IO.Directory.CreateDirectory(pathRootTemp)
            End If
            Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            Dim pathDownload As String = pathRootTemp & nomeFileDigitalizzato

            Dim allegato As New ParsecPro.Allegato


            allegato.NomeFile = nomeFileDigitalizzato
            allegato.NomeFileTemp = nomeFileDigitalizzato

            If Me.DocumentoPrimarioRadioButton.Checked Then
                allegato.IdTipologiaDocumento = 1
                allegato.DescrizioneTipologiaDocumento = "Primario"
            Else
                allegato.IdTipologiaDocumento = 0
                allegato.DescrizioneTipologiaDocumento = "Allegato"
            End If

            If String.IsNullOrEmpty(Me.DescrizioneDocumentoTextBox.Text) Then
                allegato.Oggetto = "Allegato scansionato"
            Else
                allegato.Oggetto = Me.DescrizioneDocumentoTextBox.Text
            End If


            allegato.PercorsoRoot = pathRoot
            allegato.PercorsoRootTemp = pathRootTemp
            allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
            allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

            'Se il tipo di file scansionato è PDF
            If nomeFileDigitalizzato.ToLower.EndsWith(".pdf") Then
                allegato.Scansionato = True
            End If

            Me.Allegati.Add(allegato)
            Me.DescrizioneDocumentoTextBox.Text = String.Empty
        End If
    End Sub

    'Metodo Click su ScanUploadButton che fa partire la Notifica della scansione
    Protected Sub ScanUploadButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadButton.Click
        Me.NotificaScansione()
    End Sub


#End Region

#Region "GESTIONE FIRMA DIGITALE"

    'Metodo che lancia la "Registrazione" dei componenti per la Firma
    Private Sub RegistraParsecDigitalSign()
        Dim script As String = ParsecAdmin.SignParameters.RegistraParsecDigitalSign
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

#Region "GESTIONE CLASSIFICAZIONE"

    'Metodo Click del Pulsante TrovaClassificazioneImageButton per la Ricerca ed associazione della Voci per la Classificazione del protocollo
    Protected Sub TrovaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("filtro", Me.FiltroDescrizioneClassificazioneTextBox.Text)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    End Sub

    'Metodo Click del Pulsante AggiornaClassificazioneImageButton per l'Aggiornamento della Voci per la Classificazione. Richiamato da TrovaClassificazioneImageButton.Click
    Protected Sub AggiornaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim classificazioneCompleta As String = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione2(idClassificazione, 1) & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneTextBox.Text = idClassificazione.ToString
            Session("ClassificazioniSelezionate") = Nothing
            Me.FiltroDescrizioneClassificazioneTextBox.Text = String.Empty

        End If
    End Sub

    'Metodo Click su EliminaClassificazioneImageButton che elimina una voce diclassificazione dal Protocollo
    Protected Sub EliminaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaClassificazioneImageButton.Click
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        'SE NON E' UNA FATTURA
        If Page.Request("Fattura") Is Nothing Then
            Me.SelezionaModelloIter()
        End If
    End Sub

    'Metodo Click su FiltraClassificazioneImageButton. Filtra le voci per la ricerca della Voci di Classificazione e imposta ClassificazioneTextBox.Text e IdClassificazioneTextBox.Text 
    Protected Sub FiltraClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraClassificazioneImageButton.Click
        If Not String.IsNullOrEmpty(Me.FiltroCategoriaTextBox.Text) Then
            Dim classificazioni As New ParsecAdmin.TitolarioClassificazioneRepository
            Dim classificazione As ParsecAdmin.TitolarioClassificazione = classificazioni.GetClassificazione(Me.FiltroCategoriaTextBox.Text, Me.FiltroClasseTextBox.Text, Me.FiltroSottoClasseTextBox.Text)
            If Not classificazione Is Nothing Then
                Dim classificazioneCompleta As String = classificazioni.GetCodiciClassificazione2(classificazione.Id, 1) & " " & classificazione.Descrizione
                Me.ClassificazioneTextBox.Text = classificazioneCompleta
                Me.IdClassificazioneTextBox.Text = classificazione.Id.ToString
                Me.FiltroCategoriaTextBox.Text = String.Empty
                Me.FiltroClasseTextBox.Text = String.Empty
                Me.FiltroSottoClasseTextBox.Text = String.Empty
                Dim idClassificazione As Integer = classificazione.Id
            Else
                ParsecUtility.Utility.MessageBox("Classificazione non trovata!", False)
            End If
            classificazioni.Dispose()
        Else
            ParsecUtility.Utility.MessageBox("E' necessario specificare almeno la categoria!", False)
        End If
    End Sub

#End Region

#Region "GESTIONE COLLEGAMENTI"

    'Metodo Click su TrovaCollegamentoImageButton che ricerca un protocollo per collegarlo al protocollo selezionato
    Protected Sub TrovaCollegamentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaCollegamentoImageButton.Click
        Dim pageUrl As String = "~/UI/Protocollo/pages/search/RicercaRegistrazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaCollegamentoImageButton.ClientID)
        queryString.Add("modalita", "ricerca")
        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 600, queryString, False)
    End Sub

    'Metodo Click su AggiornaCollegamentoImageButton che aggiorna il riferimento alla registrazione di Protocollo. Richiamamto da TrovaCollegamentoImageButton.Click
    Protected Sub AggiornaCollegamentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaCollegamentoImageButton.Click
        If Not Session("RicercaRegistrazione_IdRegistrazioneSelezionata") Is Nothing Then
            Dim registrazioni As New ParsecPro.RegistrazioniRepository

            Dim protocolloSelezionato As ParsecPro.Registrazione = registrazioni.GetById(Session("RicercaRegistrazione_IdRegistrazioneSelezionata"))
            registrazioni.Dispose()
            Session("RicercaRegistrazione_IdRegistrazioneSelezionata") = Nothing

            If Not Me.Registrazione Is Nothing Then
                If protocolloSelezionato.Id = Me.Registrazione.Id Then
                    Exit Sub
                End If
            End If

            If Not protocolloSelezionato Is Nothing Then

                Dim aggiorna As Boolean = False

                '********************************************************************
                Dim parametri As New ParsecAdmin.ParametriRepository
                Dim parametro = parametri.GetByName("CaricaDatiDaProtocolloCollegato")
                parametri.Dispose()

                If Not parametro Is Nothing Then
                    aggiorna = parametro.Valore = "1"
                End If

                If aggiorna Then
                    Me.AggiornaVisibilitaDaRegistrazione(protocolloSelezionato)
                End If

                Dim collegamento As New ParsecPro.Collegamento
                collegamento.AnnoProtocollo2 = Year(protocolloSelezionato.DataImmissione)
                collegamento.NumeroProtocollo2 = protocolloSelezionato.NumeroProtocollo
                collegamento.TipoRegistrazione2 = protocolloSelezionato.TipoRegistrazione
                collegamento.Diretto = True
                collegamento.Oggetto = protocolloSelezionato.Oggetto
                collegamento.NumeroProtocollo = collegamento.GetNumeroProtocollo(protocolloSelezionato.NumeroProtocollo, protocolloSelezionato.TipoRegistrazione)
                collegamento.AnnoProtocollo = Year(protocolloSelezionato.DataImmissione)

                Dim referenti As New StringBuilder
                Dim uffici As New StringBuilder

                For Each m As ParsecPro.Mittente In protocolloSelezionato.Mittenti
                    If m.Interno Then
                        uffici.Append(m.Descrizione)
                    Else
                        referenti.Append(m.Descrizione)
                    End If
                Next

                For Each d As ParsecPro.Destinatario In protocolloSelezionato.Destinatari
                    If d.Interno Then
                        uffici.Append(d.Descrizione)
                    Else
                        referenti.Append(d.Descrizione)
                    End If
                Next

                collegamento.Referenti = referenti.ToString
                collegamento.Uffici = uffici.ToString

                If Me.Registrazione Is Nothing Then
                    Dim collegamentiDiretti As List(Of ParsecPro.Collegamento) = Me.Collegamenti.Where(Function(c) c.Diretto = True).ToList
                    If collegamentiDiretti.Count = 0 Then
                        'Il primo collegamento viene utilizzalo per caricare il nuovo protocollo

                        If aggiorna Then
                            Me.GetRegistrazioneDaCollegamento(protocolloSelezionato)
                        End If

                        Me.RiscontroNumeroProtocolloTextBox.Text = protocolloSelezionato.NumeroProtocollo
                        Me.RiscontroDataImmissioneProtocolloTextBox.Text = String.Format("{0:dd/MM/yyyy}", protocolloSelezionato.DataImmissione)

                    End If
                End If

                Dim esiste = Not Me.Collegamenti.Where(Function(c) c.NumeroProtocollo2 = collegamento.NumeroProtocollo2).FirstOrDefault Is Nothing
                If Not esiste Then
                    Me.Collegamenti.Add(collegamento)
                End If

            End If
        End If
    End Sub

    'Metodo che restituisce la Registrazione di Protocollo a partire da un Protocollo collegato
    Private Sub GetRegistrazioneDaCollegamento(ByVal protocolloSelezionato As ParsecPro.Registrazione)

        If Me.TipologiaRegistrazione = TipoRegistrazione.Interna Then
            Me.Mittenti = protocolloSelezionato.Mittenti.Where(Function(c) c.Interno = True).ToList
            Me.Destinatari = protocolloSelezionato.Destinatari.Where(Function(c) c.Interno = True).ToList
        Else
            Me.Mittenti = protocolloSelezionato.Mittenti
            Me.Destinatari = protocolloSelezionato.Destinatari
        End If


        Me.OggettoTextBox.Text = protocolloSelezionato.Oggetto

        If protocolloSelezionato.IdClassificazione.HasValue Then
            Me.ClassificazioneTextBox.Text = protocolloSelezionato.ClassificazioneCompleta
            Me.IdClassificazioneTextBox.Text = protocolloSelezionato.IdClassificazione.ToString
        End If

        'Se il tipo di registrazione di origine è diverso da quello di destinazione.
        If protocolloSelezionato.TipologiaRegistrazione <> Me.TipologiaRegistrazione Then
            If Me.TipologiaRegistrazione = TipoRegistrazione.Interna Then
                If protocolloSelezionato.TipologiaRegistrazione = TipoRegistrazione.Arrivo Then
                    Me.ScambiaMittentiDestinatari()
                End If
            Else
                Me.ScambiaMittentiDestinatari()
            End If

        End If
    End Sub

    'Metodo ItemDataBound della griglia CollegamentiDirettiGridView. Setta tooltip e stili in base al contenuto delle celle.
    Protected Sub CollegamentiDirettiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles CollegamentiDirettiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.Style.Add("cursor", "hand")
                btn.ToolTip = "Elimina collegamento"
            End If
            If TypeOf dataItem("Detail").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Detail").Controls(0), ImageButton)
                btn.Style.Add("cursor", "hand")
                btn.ToolTip = "Visualizza collegamento"
            End If
        End If
    End Sub

    'Metodo ItemCommand della griglia CollegamentiDirettiGridView. Fa partire i vari comandi eseguibili dalla griglia CollegamentiDirettiGridView.
    Protected Sub CollegamentiDirettiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles CollegamentiDirettiGridView.ItemCommand

        If e.CommandName = "Delete" Then
            Dim numeroProtocollo As String = CStr(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("NumeroProtocollo"))
            Dim collegamento As ParsecPro.Collegamento = Me.Collegamenti.Where(Function(c) c.NumeroProtocollo = numeroProtocollo).FirstOrDefault
            If Not collegamento Is Nothing Then
                Me.Collegamenti.Remove(collegamento)
            End If
        End If
        If e.CommandName = "Detail" Then

            Dim anno As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("AnnoProtocollo")
            Dim numeroTipo() As String = CStr(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("NumeroProtocollo")).Split("/")
            Dim numeroProtocollo As Integer = CInt(numeroTipo(0).Trim)

            Dim tipo As Integer = 0
            Select Case numeroTipo(1).Trim
                Case "P"
                    tipo = 1
                Case "I"
                    tipo = 2
            End Select

            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetQuery.Where(Function(c) c.NumeroProtocollo = numeroProtocollo And c.TipoRegistrazione = tipo And Year(c.DataImmissione) = anno And c.Modificato = False).FirstOrDefault
            registrazioni.Dispose()

            Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
            Dim queryString As New Hashtable

            Dim parametriPagina As New Hashtable
            parametriPagina.Add("Filtro", registrazione.Id)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina

            ParsecUtility.Utility.ShowPopup(pageUrl, 940, 510, queryString, False)

        End If
    End Sub


#End Region

    'Evento SelectedIndexChanged del Controllo TipoRegistrazioneRadioList. In base alla tipologia selezionata di abilitano/disabilitano pulsanti della toolbar.
    'Inoltre se siinverte la tiplogia ad una registrazione già salvata, scambia i mittenti e destinatari.
    Protected Sub TipoRegistrazioneRadioList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TipoRegistrazioneRadioList.SelectedIndexChanged
        Dim tipologiaRegistrazione As ParsecPro.TipoRegistrazione = ParsecPro.TipoRegistrazione.Nessuna
        Dim scambio As Boolean = False

        Select Case TipoRegistrazioneRadioList.SelectedIndex
            Case 0  'Arrivo
                tipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo
            Case 1  'Partenza
                tipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza
            Case 2  'Interna
                tipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna
        End Select

        If Me.TipologiaRegistrazione <> tipologiaRegistrazione Then

            If Not Me.Registrazione Is Nothing Then
                If tipologiaRegistrazione <> Me.Registrazione.TipologiaRegistrazione Then
                    Me.RadToolBar.Items.FindItemByText("Invia E-mail").Enabled = False
                Else
                    Me.RadToolBar.Items.FindItemByText("Invia E-mail").Enabled = True
                End If
            End If

            If tipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo AndAlso (Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza OrElse Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna) Then
                scambio = True
            ElseIf (tipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza OrElse Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna) AndAlso (Me.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo) Then
                scambio = True
            End If

            If scambio Then
                Me.ScambiaMittentiDestinatari()
            End If

            Me.TipologiaRegistrazione = tipologiaRegistrazione

            'ESCLUDO I DESTINATARI ESTERNI
            If tipologiaRegistrazione = TipoRegistrazione.Interna Then
                Me.Destinatari = Me.Destinatari.Where(Function(c) c.Interno = True).ToList
            End If

            If Me.Registrazione Is Nothing Then
                If Page.Request("Mode") Is Nothing Then

                    Me.CaricaReferenteDefault(Me.TipologiaRegistrazione)
                End If
            End If

        End If

        Me.SelezionaModelloIter()

    End Sub

    'Scambia Mittenti con Destinatari. Richiamato da TipoRegistrazioneRadioList.SelectedIndexChanged
    Private Sub ScambiaMittentiDestinatari()
        Dim mittenti As List(Of Mittente) = Me.Mittenti
        Dim destinatari As List(Of Destinatario) = Me.Destinatari
        'Rendo effettive le modifiche di inversione dei due gruppi di referenti
        Me.Mittenti = ParsecPro.Destinatari.ConvertiInMittenti(destinatari)
        Me.Destinatari = ParsecPro.Mittenti.ConvertiInDestinatari(mittenti)
    End Sub

    'Imposta alcune voci precaricate nella maschera in base alla tipologia di registrazione impostata.
    Private Sub ImpostaUiPagina(ByVal tipoRegistrazione As ParsecPro.TipoRegistrazione)
        Select Case tipoRegistrazione
            Case ParsecPro.TipoRegistrazione.Arrivo
                Me.ReferenteEsternoLabel.Text = "Mittenti " & If(Me.ReferentiEsterniGridView.MasterTableView.Items.Count > 0, "( " & Me.ReferentiEsterniGridView.MasterTableView.Items.Count.ToString & " )", "")
                Me.SecondoReferenteInternoLabel.Text = "Destinatari " & If(Me.SecondoReferentiInterniGridView.MasterTableView.Items.Count > 0, "( " & Me.SecondoReferentiInterniGridView.MasterTableView.Items.Count.ToString & " )", "")
                Me.ReferentiEsterniGridView.ToolTip = "Elenco mittenti associati al protocollo"
                Me.SecondoReferentiInterniGridView.ToolTip = "Elenco destinatari interni associati al protocollo"
                Me.DataOraRicezioneInvioLabel.Text = "Data/Ora ricezione"
                Me.TipoRicezioneInvioLabel.Text = "Tipo ricezione"
            Case ParsecPro.TipoRegistrazione.Partenza, ParsecPro.TipoRegistrazione.Interna
                Me.ReferenteEsternoLabel.Text = "Destinatari " & If(Me.ReferentiEsterniGridView.MasterTableView.Items.Count > 0, "( " & Me.ReferentiEsterniGridView.MasterTableView.Items.Count.ToString & " )", "")
                Me.SecondoReferenteInternoLabel.Text = "Mittenti " & If(Me.SecondoReferentiInterniGridView.MasterTableView.Items.Count > 0, "( " & Me.SecondoReferentiInterniGridView.MasterTableView.Items.Count.ToString & " )", "")
                Me.ReferentiEsterniGridView.ToolTip = "Elenco destinatari associati al protocollo"
                Me.SecondoReferentiInterniGridView.ToolTip = "Elenco mittenti interni associati al protocollo"
                Me.DataOraRicezioneInvioLabel.Text = "Data/Ora invio"
                Me.TipoRicezioneInvioLabel.Text = "Tipo invio"
        End Select
        Me.CollegamentiDirettiLabel.Text = "Collegamenti diretti " & If(Me.CollegamentiDirettiGridView.MasterTableView.Items.Count > 0, "( " & Me.CollegamentiDirettiGridView.MasterTableView.Items.Count.ToString & " )", "")
        Me.CollegamentiIndirettiLabel.Text = "Collegamenti indiretti " & If(Me.CollegamentiIndirettiGridView.MasterTableView.Items.Count > 0, "( " & Me.CollegamentiIndirettiGridView.MasterTableView.Items.Count.ToString & " )", "")

        Me.DocumentiLabel.Text = "Documenti " & If(Me.DocumentiGridView.MasterTableView.Items.Count > 0, "( " & Me.DocumentiGridView.MasterTableView.Items.Count.ToString & " )", "")

        Me.VisibilitaLabel.Text = "Elenco Visibilità&nbsp;&nbsp;" & If(Me.VisibilitaGridView.MasterTableView.Items.Count > 0, "( " & Me.VisibilitaGridView.MasterTableView.Items.Count.ToString & " )", "")

    End Sub


#Region "GESTIONE FASCICOLI"

    'Metodo ItemCommand della Griglia FascicoliGridView. Fa partire i comandi dalla griglia dei fascicoli.
    Protected Sub FascicoliGridView_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles FascicoliGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
            Dim fascicolo As ParsecAdmin.Fascicolo = Me.Fascicoli.Where(Function(c) c.Id = id).FirstOrDefault
            If Not fascicolo Is Nothing Then
                Me.Fascicoli.Remove(fascicolo)
            End If

        ElseIf e.CommandName = "Select" Then
            ModificaFascicolo(e.Item)
        End If
    End Sub

    'Richiama in Modifica la Pagina dei Fascicoli. Richiamato da FascicoliGridView.ItemCommand. Apre la maschera FascicoliPage.aspx
    Private Sub ModificaFascicolo(ByVal item As GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/FascicoliPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.ModificaFascicoloImageButton.ClientID)
        queryString.Add("mode", "update")
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdFascicolo", id)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 600, queryString, False)
    End Sub

    'Richiama in Inserimento la Pagina dei Fascicoli.  Apre la maschera FascicoliPage.aspx
    Protected Sub NuovoFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NuovoFascicoloImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/FascicoliPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.InserisciFascicoloImageButton.ClientID)
        queryString.Add("mode", "insert")
        Dim parametriPagina As New Hashtable
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 600, queryString, False)
    End Sub

    'Richiama in Ricerca l'Elenco dei Fascicoli. Richiama la pagina ElencoFascicoliPage.aspx con possibilità di selezionare un fascicolo per inserirlo nella griglia dei fascicoli collegati al prototocollo.
    Protected Sub TrovaFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaFascicoloImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/ElencoFascicoliPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("modalita", "ricerca")
        queryString.Add("obj", Me.InserisciFascicoloImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 650, queryString, False)
    End Sub

    'Inserisce un Fascicolo nella griglia. Richiamato dopo la esecuzione di TrovaFascicoloImageButton.Click
    Protected Sub InserisciFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles InserisciFascicoloImageButton.Click
        If Not ParsecUtility.SessionManager.Fascicolo Is Nothing Then
            Dim fascicolo As ParsecAdmin.Fascicolo = ParsecUtility.SessionManager.Fascicolo
            If Me.FaseDocumentoFascicoloComboBox.SelectedIndex > 0 Then
                fascicolo.Fase = Me.FaseDocumentoFascicoloComboBox.SelectedValue
            Else
                fascicolo.Fase = Nothing
            End If

            Dim exist As Boolean = Not Me.Fascicoli.Where(Function(c) c.Codice = fascicolo.Codice).FirstOrDefault Is Nothing
            If Not exist Then
                Me.Fascicoli.Add(fascicolo)
            End If
            ParsecUtility.SessionManager.Fascicolo = Nothing
        End If
    End Sub

    'Aggiorna la Griglia dei Fascicoli. Richiamato da GetParametri().
    Private Sub AggiornaGrigliaFascicoli()
        Me.FascicoliGridView.DataSource = Me.Fascicoli
        Me.FascicoliGridView.DataBind()
    End Sub

    'Metodo che scatta dopo il metodo ModificaFascicolo(). Aggiorna la lista dei fascicoli.
    Protected Sub ModificaFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ModificaFascicoloImageButton.Click
        If Not ParsecUtility.SessionManager.Fascicolo Is Nothing Then
            Dim fascicolo As ParsecAdmin.Fascicolo = ParsecUtility.SessionManager.Fascicolo

            Dim fascicoloPrecedente = Me.Fascicoli.Where(Function(c) c.Codice = fascicolo.Codice).FirstOrDefault
            If Not fascicoloPrecedente Is Nothing Then
                fascicolo.Fase = fascicoloPrecedente.Fase
                Dim index = Me.Fascicoli.FindIndex(Function(c) c.Codice = fascicolo.Codice)
                Me.Fascicoli.Remove(fascicoloPrecedente)
                Me.Fascicoli.Insert(index, fascicolo)
            End If
            ParsecUtility.SessionManager.Fascicolo = Nothing

        End If
    End Sub

    'Carica l'elenco delle Fasi dei nei fascicoli.
    Private Sub CaricaFasi()
        Dim dati As New Dictionary(Of String, String)
        dati.Add(ParsecAdmin.FaseDocumentoEnumeration.INIZIALE, "Iniziale")
        dati.Add(ParsecAdmin.FaseDocumentoEnumeration.FINALE, "Finale")
        Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})
        Me.FaseDocumentoFascicoloComboBox.DataSource = ds
        Me.FaseDocumentoFascicoloComboBox.DataTextField = "Descrizione"
        Me.FaseDocumentoFascicoloComboBox.DataValueField = "Id"
        Me.FaseDocumentoFascicoloComboBox.DataBind()
        Me.FaseDocumentoFascicoloComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.FaseDocumentoFascicoloComboBox.SelectedIndex = 0
    End Sub

#End Region

    'Metodo Click di FiltraImageButton che fa partire la Ricerca Avanzata delle Registrazioni di Protocollo.
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Me.AdvancedSearch()
    End Sub

    'Fa partire la Ricerca avanzata dei Protocolli. Richiamato da FiltraImageButton.Click
    Private Sub AdvancedSearch()
        Dim pageUrl As String = "~/UI/Protocollo/pages/search/RicercaProtocolliPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("modalita", "filtro")
        queryString.Add("obj", Me.AggiornaProtocolliImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 870, 550, queryString, False)
    End Sub

    'Metodo che parte dopo che si è concluso AdvancedSearch() per l'aggiornamento della griglia in base ai risultati della ricerca.
    Protected Sub AggiornaProtocolliImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaProtocolliImageButton.Click
        If Not ParsecUtility.SessionManager.FiltroRegistrazione Is Nothing Then
            Me.Registrazioni = Nothing
            Me.RegistrazioneFiltro = ParsecUtility.SessionManager.FiltroRegistrazione
            ParsecUtility.SessionManager.FiltroRegistrazione = Nothing
            Me.RipristinaFiltroInizialeImageButton.Enabled = True
            Me.ProtocolliGridView.Rebind()
        End If
    End Sub

    'Metodo click di RipristinaFiltroInizialeImageButton che ripristina le condizioni di Ricerca Iniziali della maschera.
    Protected Sub RipristinaFiltroInizialeImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles RipristinaFiltroInizialeImageButton.Click
        Me.RegistrazioneFiltro = Nothing
        Me.RipristinaFiltroInizialeImageButton.Enabled = False
        Me.RadToolBar.Items.FindItemByText("Salva").Enabled = True
        'Abilito tutta l'interfaccia utente
        Me.ImpostaAbilitazioneUi(True)
        Me.ResettaVista()
        Me.Registrazioni = Nothing
        Me.ProtocolliGridView.Rebind()
        Me.PannelloSessioneEmergenza.Visible = (Me.ModalitaRegistrazione = ParsecPro.ModalitaRegistrazione.Emergenza)

        Me.AbilitaSessioneEmergenza(True)
    End Sub


#Region "GESTIONE STAMPA ETICHETTE E TIMBRATRICE"

    'Metodo che lancia la "Registrazione" dei componenti per la stampa
    Private Sub RegistraParsecPrinting()
        Dim script As String = ParsecAdmin.PrintingParameters.RegistraParsecPrinting
        Me.MainPage.RegisterComponent(script)
    End Sub

    'Esegue la Stampa della Registrazione di protocollo.
    Private Sub RegistraEseguiStampa(ByVal registrazione As ParsecPro.Registrazione)
        Dim parameters As New ParsecAdmin.PrintingParameters
        Dim input As New ParsecAdmin.DatiStampa
        Dim datiEtichetta As New ParsecAdmin.DatiInputEtichetta
        Dim numeroProtocollo As String = registrazione.NumeroProtocollo.ToString.PadLeft(7, "0")
        Dim numeroProtocollo2 As String = registrazione.NumeroProtocollo.ToString.PadLeft(7, "0")
        Dim dataProtocollo As String = CDate(registrazione.DataImmissione).ToString("dd/MM/yyyy HH:mm")
        Dim annoProtocollo As String = CDate(registrazione.DataImmissione).Year.ToString

        datiEtichetta.Descrizione = numeroProtocollo
        datiEtichetta.NumeroProtocollo = numeroProtocollo2 & annoProtocollo & registrazione.TipoRegistrazione.ToString
        datiEtichetta.Classificazione = registrazione.ClassificazioneCompleta
        datiEtichetta.AnnoProtocollo = annoProtocollo
        datiEtichetta.DataRegistrazione = dataProtocollo
        Select Case registrazione.TipologiaRegistrazione
            Case TipoRegistrazione.Arrivo
                datiEtichetta.Tipologia = "A"
            Case TipoRegistrazione.Interna
                datiEtichetta.Tipologia = "I"
            Case TipoRegistrazione.Partenza
                datiEtichetta.Tipologia = "P"
        End Select


        Dim data As String = parameters.CreaDataSource(datiEtichetta, input)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Dim sb As New System.Text.StringBuilder

            sb.AppendLine("<script language='Javascript'>" & vbCrLf)
            sb.AppendLine("function Print(){ " & vbCrLf)
            sb.AppendLine("try" & vbCrLf)
            sb.AppendLine("{" & vbCrLf)


            Dim modal As Boolean = False
            sb.AppendLine("document." & ParsecAdmin.PrintingParameters.ComponentName & ".Execute(" & modal.ToString.ToLower & "," & Chr(34) & data & Chr(34) & ");")

            sb.AppendLine("}" & vbCrLf)
            sb.AppendLine("catch(e){alert(e.message);}" & vbCrLf)
            sb.AppendLine("finally {}" & vbCrLf)
            sb.AppendLine("}" & vbCrLf)
            sb.AppendLine("Print();" & vbCrLf)
            sb.AppendLine("</script>" & vbCrLf)

            ParsecUtility.Utility.RegisterScript(sb, False)

        Else

            'UTILIZZO IL SOCKET  
            Dim ip As String = Request.UserHostAddress.ToString
            Dim tcp As New ParsecUtility.TCPClient
            tcp.ConnectToServer(ip)
            tcp.SendData(data)
            tcp.ReceiveData()
            While String.IsNullOrEmpty(tcp.Data)
            End While
            tcp.CloseConnection()

        End If

    End Sub

#End Region

#Region "GESTIONE UTENTE EMERGENZA"

    'Metodo click associato a EliminaUtenteImageButton che resetta le informazioni circa l'Utente di Emergenza
    Protected Sub EliminaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteImageButton.Click
        Me.UtenteEmergenzaTextBox.Text = String.Empty
        Me.IdUtenteEmergenzaTextBox.Text = String.Empty
    End Sub

    'Metodo click associato a AggiornaUtenteImageButton che Aggiorna le informazioni circa l'Utente di Emergenza. Scatta dopo TrovaUtenteImageButton_Click per aggiornare la maschera
    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Me.UtenteEmergenzaTextBox.Text = utentiSelezionati.First.Value
            Me.IdUtenteEmergenzaTextBox.Text = utentiSelezionati.First.Key
            Session("SelectedUsers") = Nothing
        End If
    End Sub

    'Metodo click associato a TrovaUtenteImageButton che Aggiorna le informazioni circa l'Utente di Emergenza. Richiama la maschera RicercaUtentePage.aspx
    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 0 'singola
        Session("Parametri") = ht
    End Sub

#End Region

#Region "GESTIONE CHIUSURA FINESTRA POPUP"
    'Effettua la chiusura della finestra PopUp
    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub

#End Region

#Region "GESTIONE TIMBRATURA"

    'GESTIONE TIMBRATURA
    'Aggiunge la Segnatura al PDF
    Private Sub AddWatermarkToPdf(ByVal watermark As String, ByVal posizione As Integer, ByVal allegato As ParsecPro.Allegato)
        Dim esercizi As New ParsecPro.EsercizioRepository
        Dim annoCorrente As Integer = esercizi.GetAnnoEsercizioCorrente
        If annoCorrente = -1 Then
            annoCorrente = Now.Year
        End If
        esercizi.Dispose()
        Dim relativePath As String = "\" & annoCorrente.ToString & "\"
        Dim storedPath As String = allegato.PercorsoRoot & relativePath & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
        Dim temp As String = "temp" & allegato.NomeFile
        Dim pathDownloadTemp As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & temp

        'SE IL FILE PDF E' PROTETTO O SE IL FORMATO NON VALIDO NON AGGIUNGO IL WATERMARK
        Try
            Dim reader As New PdfReader(storedPath)

            Dim fs As New IO.FileStream(pathDownloadTemp, FileMode.Create, FileAccess.Write)
            Dim pdfStamper As New PdfStamper(reader, fs, ControlChars.NullChar, True)

            Dim x As Integer = 0
            Dim y As Integer = 0
            Dim rot As Single = 0.0F


            For i As Integer = 1 To reader.NumberOfPages
                Dim size As iTextSharp.text.Rectangle = reader.GetPageSizeWithRotation(i)
                Dim cb As PdfContentByte = pdfStamper.GetOverContent(i)
                cb.SaveState()
                Dim bf As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
                cb.SetColorFill(BaseColor.BLACK)
                cb.SetFontAndSize(bf, 12)
                cb.BeginText()


                Select Case posizione
                    'SOPRA
                    Case 1
                        x = (size.Right + size.Left) / 2
                        y = size.Top - 15
                        rot = 0

                        'SOTTO
                    Case 2

                        x = (size.Right + size.Left) / 2
                        y = 5
                        rot = 0

                        'DESTRA DEFAULT PROTOCOLLO ARRIVO
                    Case 3

                        x = size.Width - 15
                        y = size.Height / 2
                        rot = -90
                        'SINISTRA    DEFAULT PROTOCOLLO PARTENZA
                    Case 4

                        x = 15
                        y = (size.Top - size.Bottom) / 2
                        rot = 90

                End Select


                cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermark, x, y, rot)

                cb.EndText()
                cb.RestoreState()
            Next

            pdfStamper.Close()
            fs.Close()
            reader.Close()
            IO.File.Copy(pathDownloadTemp, storedPath, True)
            IO.File.Delete(pathDownloadTemp)
            'End If
        Catch ex As BadPasswordException
        Catch ex As BadPdfFormatException
        Catch ex As DocumentException
            'Aggiungere
        Catch ex As Exception
        End Try


    End Sub


#End Region

#Region "GESTIONE STAMPA RICEVUTA"

    'Stampa la Rivevuta della Registrazione del Protocollo
    Private Sub StampaRicevuta()
        Dim protocolli As New ParsecPro.RegistrazioniRepository
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaRicevutaRegistrazione")
        Dim reg = protocolli.GetView(Me.GetFiltroRicevuta())
        parametriStampa.Add("DatiStampa", reg)

        Session("ParametriStampaPro") = parametriStampa
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

#End Region

#Region "GESTIONE VISIBILITA'"

    'Evento Click di TrovaGruppoVisibilitaImageButton per ricercare ed associare un Gruppo di Utenti da aggiungere alla Visibilità della registrazione di Protocollo. Apre la maschera RicercaGruppoPage.aspx
    Protected Sub TrovaGruppoVisibilitaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaGruppoVisibilitaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaGruppoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaGruppoVisibilitaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    'Metodo che scatta dopo TrovaGruppoVisibilitaImageButton_Click e che aggiorna la lista della Visibilità.
    Protected Sub AggiornaGruppoVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaGruppoVisibilitaImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        If Not Session("GruppiSelezionati") Is Nothing Then
            Dim gruppiSelezionati As SortedList(Of Integer, String) = Session("GruppiSelezionati")
            Dim idGruppo As Integer = 0
            Dim gruppo As ParsecAdmin.VisibilitaDocumento = Nothing
            For Each gruppoSelezionato In gruppiSelezionati
                idGruppo = gruppoSelezionato.Key
                Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = idGruppo And c.TipoEntita = ParsecAdmin.TipoEntita.Gruppo).FirstOrDefault Is Nothing
                If Not esiste Then
                    gruppo = New ParsecAdmin.VisibilitaDocumento
                    gruppo.IdEntita = idGruppo
                    gruppo.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
                    gruppo.IdModulo = ParsecAdmin.TipoModulo.PRO
                    gruppo.Descrizione = gruppoSelezionato.Value
                    gruppo.LogIdUtente = utenteCollegato.Id
                    gruppo.LogDataOperazione = Now
                    gruppo.AbilitaCancellaEntita = True
                    Me.AggiungiGruppoUtenteVisibilita(gruppo)
                End If
            Next
            Session("GruppiSelezionati") = Nothing
        End If
    End Sub

    'Evento Click di TrovaGruppoVisibilitaImageButton per ricercare ed associare un Utente da aggiungere alla Visibilità della registrazione di Protocollo. Apre la maschera RicercaUtentePage.aspx
    Protected Sub TrovaUtenteVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteVisibilitaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteVisibilitaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    'Metodo che scatta dopo TrovaUtenteVisibilitaImageButton_Click e che aggiorna la lista della Visibilità.
    Protected Sub AggiornaUtenteVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteVisibilitaImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Nothing
            Dim idUtente As Integer = 0
            For Each utenteSelezionato In utentiSelezionati
                idUtente = utenteSelezionato.Key
                Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = idUtente And c.TipoEntita = ParsecAdmin.TipoEntita.Utente).FirstOrDefault Is Nothing
                If Not esiste Then
                    Dim utenti As New ParsecAdmin.UserRepository
                    Dim utente As ParsecAdmin.Utente = utenti.GetUserById(utenteSelezionato.Key).FirstOrDefault
                    If Not utente Is Nothing Then
                        Me.AggiungiUtenteVisibilita(utente)
                    End If
                End If
            Next
            Session("SelectedUsers") = Nothing
        End If
    End Sub

    'Evento ItemCommand della griglia VisibilitaGridView. Permette la cancellazione di un Gruppo o di un Utente
    Protected Sub VisibilitaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles VisibilitaGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                EliminaGruppoUtente(e)
        End Select
    End Sub

    'Evento ItemDataBound della griglia VisibilitaGridView. Setta tooltip e abilita gli onclick in base al contenuto delle celle.
    Protected Sub VisibilitaGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles VisibilitaGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim gruppoUtente As ParsecAdmin.VisibilitaDocumento = CType(e.Item.DataItem, ParsecAdmin.VisibilitaDocumento)
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina elemento selezionato"
                Dim message As String = "Eliminare l'elemento selezionato?"
                If Not gruppoUtente.AbilitaCancellaEntita Then
                    message = "L'elemento selezionato non può essere cancellato!"
                    btn.Attributes.Add("onclick", "alert(""" & message & """);return false;")
                    btn.ToolTip = "Elemento non cancellabile"
                Else
                    btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
                End If
            End If
        End If
    End Sub

    'Metodo che Elimina un Gruppo o un Utente dalla Lista della Visibilità. Lanciato da VisibilitaGridView.ItemCommand
    Private Sub EliminaGruppoUtente(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim idEntita As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("IdEntita"))
        Dim tipoEntita As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("TipoEntita"))
        Dim gruppoUtente As ParsecAdmin.VisibilitaDocumento = Me.Visibilita.Where(Function(c) c.IdEntita = idEntita And c.TipoEntita = tipoEntita).FirstOrDefault
        If Not gruppoUtente Is Nothing Then
            Me.Visibilita.Remove(gruppoUtente)
        End If
    End Sub

    'Metodo che Aggiunge un Gruppo o un Utente alla Lista della Visibilità. Richiamato da AggiungiUtenteVisibilita e AggiungiGruppoDefault.
    Private Sub AggiungiGruppoUtenteVisibilita(gruppoUtente As ParsecAdmin.VisibilitaDocumento)
        Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = gruppoUtente.IdEntita And c.TipoEntita = gruppoUtente.TipoEntita).FirstOrDefault Is Nothing
        If Not esiste Then
            Me.Visibilita.Add(gruppoUtente)
        End If
    End Sub

    'Metodo che aggiunge il Gruppo di default alla Lista della Visibilità
    Private Sub AggiungiGruppoDefault(ByVal idStruttura As Integer)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim startDate As Date = New Date(Now.Year, Now.Month, Now.Day, 0, 0, 0)
        Dim endDate As Date = New Date(Now.Year, Now.Month, Now.Day, 23, 59, 59)

        Dim gruppi As New ParsecAdmin.GruppoRepository(strutture.Context)
        Dim gruppo = (From s In strutture.GetQuery
                Join g In gruppi.GetQuery
                On s.IdGruppo Equals g.Id
                Where g.Abilitato = True And s.Id = idStruttura And g.DataInizioValidita <= startDate And (g.DataFineValidita >= Now Or Not g.DataFineValidita.HasValue)
                Select g).FirstOrDefault

        If Not gruppo Is Nothing Then
            Dim gruppoDefaut As New ParsecAdmin.VisibilitaDocumento
            gruppoDefaut.AbilitaCancellaEntita = False
            gruppoDefaut.Descrizione = gruppo.Descrizione
            gruppoDefaut.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
            gruppoDefaut.IdEntita = gruppo.Id
            gruppoDefaut.IdModulo = ParsecAdmin.TipoModulo.PRO
            gruppoDefaut.LogIdUtente = utenteCollegato.Id
            gruppoDefaut.LogDataOperazione = Now
            Me.AggiungiGruppoUtenteVisibilita(gruppoDefaut)

        End If

    End Sub

    'Metodo che Aggiunge un Utente alla lista della Visibilità. Richiamato da AggiungiUtenteVisibilita e AggiungiUtenteCollegatoVisibilita
    Private Sub AggiungiUtenteVisibilita(ByVal utente As ParsecAdmin.Utente, ByVal cancellabile As Boolean)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
        utenteVisibilita.AbilitaCancellaEntita = cancellabile
        utenteVisibilita.Descrizione = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
        utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
        utenteVisibilita.IdEntita = utente.Id
        utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.PRO
        utenteVisibilita.LogIdUtente = utenteCollegato.Id
        utenteVisibilita.LogDataOperazione = Now
        Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)
    End Sub

    'Richiama AggiungiUtenteVisibilita() per aggiungere un Utente alla lista della Visibilità.
    Private Sub AggiungiUtenteVisibilita(ByVal utente As ParsecAdmin.Utente)
        Me.AggiungiUtenteVisibilita(utente, True)
    End Sub

    'Richiama AggiungiUtenteVisibilita() per aggiungere un Utente alla lista della Visibilità.
    Private Sub AggiungiUtenteCollegatoVisibilita(ByVal utente As ParsecAdmin.Utente)
        Me.AggiungiUtenteVisibilita(utente, False)
    End Sub

#End Region


    Shared CacheRubrica As List(Of ParsecAdmin.StrutturaEsternaInfo) = Nothing

    'WebMethod che riempie la lista degli Elementi della Rubrica
    <WebMethod()> _
    Public Shared Function GetElementiRubrica(ByVal context As RadComboBoxContext) As RadComboBoxData
        If CacheRubrica Is Nothing Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            CacheRubrica = rubrica.Where(Function(c) c.Denominazione <> "" And c.LogStato Is Nothing And c.InRubrica = True).OrderBy(Function(c) c.Denominazione).ThenBy(Function(c) c.Nome).ToList
            rubrica.Dispose()
        End If
        
        Dim data = CacheRubrica.Where(Function(c) c.Denominazione.ToUpper.Contains(context.Text.ToUpper))
        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData) '(endOffset - itemOffset)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()

            Dim item = data.ElementAt(i)

            itemData.Text = item.Denominazione & " " & If(Not String.IsNullOrEmpty(item.Nome), item.Nome & ", ", ", ") & If(Not String.IsNullOrEmpty(item.Email), item.Email & ", ", "") & If(Not String.IsNullOrEmpty(item.Indirizzo), item.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(item.Comune), item.Comune & " ", "") & If(Not String.IsNullOrEmpty(item.CAP), item.CAP & " ", "") & If(Not String.IsNullOrEmpty(item.Provincia), "(" & item.Provincia & ")", "")

            itemData.Value = item.Id
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function

#Region "EVENTI GRIGLIA FULL SIZE"

    'Evento Click su VisualizzaSchermoInteroImageButton che abilita la visualizzazione a schermo intero
    Protected Sub VisualizzaSchermoInteroImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaSchermoInteroImageButton.Click
        Me.FullSize = True

        'RESETTO I FILTRI
        For Each column As GridColumn In Me.FullSizeProtocolliGridView.MasterTableView.RenderColumns
            If column.SupportsFiltering Then
                column.CurrentFilterValue = String.Empty
                column.CurrentFilterFunction = GridKnownFunction.NoFilter
            End If
        Next
        Me.FullSizeProtocolliGridView.MasterTableView.FilterExpression = String.Empty
        Me.FullSizeProtocolliGridView.CurrentPageIndex = 0
        Me.FullSizeProtocolliGridView.Rebind()

        Dim script As New StringBuilder
        script.AppendLine("<script>")
        script.AppendLine("ShowFullSizeGridPanel();")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
    End Sub

    'Metodo NeedDataSource della Griglia FullSizeProtocolliGridView. Aggancia il datasource della griglia alla lista delle registrazioni di protocolloò
    Protected Sub FullSizeParametriGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FullSizeProtocolliGridView.NeedDataSource
        If Me.FullSize Then
            Me.FullSizeProtocolliGridView.DataSource = Me.Registrazioni
        Else
            Me.FullSizeProtocolliGridView.DataSource = New List(Of ParsecPro.Registrazione)
        End If
    End Sub

    'Metodo ItemCommand della Griglia FullSizeProtocolliGridView. Permette la selezione di una registrazione dalla griglia e visualizzarne il contenuto.
    Protected Sub FullSizeProtocolliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FullSizeProtocolliGridView.ItemCommand

        If e.CommandName = "Select" Then
            Dim aggiorna As Boolean = False
            Dim selezionato As Boolean = Not Me.Registrazione Is Nothing
            Dim id = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"))

            If selezionato Then
                If Me.Registrazione.Id <> id Then
                    aggiorna = True
                End If
            End If

            If aggiorna OrElse Not selezionato Then

                Me.AggiornaVista(e)

                'DESELEZIONO LA RIGA
                Me.ProtocolliGridView.SelectedIndexes.Clear()

                'SELEZIONO LA NUOVA RIGA
                If Not Me.Registrazione Is Nothing Then
                    Dim item As GridDataItem = Me.ProtocolliGridView.MasterTableView.FindItemByKeyValue("Id", id)
                    If Not item Is Nothing Then
                        item.Selected = True
                    End If
                End If
            End If

            Dim script As New StringBuilder
            script.AppendLine("<script>")
            script.AppendLine("HideFullSizeGridPanel();")
            script.AppendLine("</script>")
            ParsecUtility.Utility.RegisterScript(script, False)
            Me.FullSize = False

        End If


    End Sub

    'Metodo ItemCreated della Griglia FullSizeProtocolliGridView. Setta gli stili.
    Protected Sub FullSizeProtocolliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FullSizeProtocolliGridView.ItemCreated

        Dim browser = Request.Browser.Browser

        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False

            If browser.ToLower.Contains("ie") Then
                e.Item.Style.Add("position", "relative")
                e.Item.Style.Add("bottom", "expression(this.offsetParent.scrollHeight - this.offsetParent.scrollTop - this.offsetParent.clientHeight-1)")
                e.Item.Style.Add("z-index", "99")
            End If

        End If

        If TypeOf e.Item Is GridFilteringItem Then

            If browser.ToLower.Contains("ie") Then
                e.Item.Style.Add("position", "relative")
                e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
                e.Item.Style.Add("z-index", "99")
            End If

        End If

        If TypeOf e.Item Is GridHeaderItem Then

            If browser.ToLower.Contains("ie") Then
                e.Item.Style.Add("position", "relative")
                e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
                e.Item.Style.Add("z-index", "99")
                e.Item.Style.Add("background-color", "White")

            End If

        End If

    End Sub

    'Evento Click di NoPaging. Permette la paginazione o meno della griglia ProtocolliGridView
    Protected Sub NoPaging_Click(sender As Object, e As System.EventArgs) Handles NoPaging.Click
        Me.ProtocolliGridView.AllowPaging = Not Me.ProtocolliGridView.AllowPaging
        If Me.ProtocolliGridView.AllowPaging Then
            Me.NoPaging.Text = "Non Paginare"
            Me.NoPaging.Icon.PrimaryIconUrl = "~/images/Next.png"
        Else
            Me.NoPaging.Text = "Paginare"
            Me.NoPaging.Icon.PrimaryIconUrl = "~/images/Previous.png"
        End If
        Me.ProtocolliGridView.Rebind()
    End Sub

    'Evento Click di FullSizeNoPaging. Permette la paginazione o meno della griglia FullSizeProtocolliGridView
    Protected Sub FullSizeNoPaging_Click(sender As Object, e As System.EventArgs) Handles FullSizeNoPaging.Click
        Me.FullSizeProtocolliGridView.AllowPaging = Not Me.FullSizeProtocolliGridView.AllowPaging
        If Me.FullSizeProtocolliGridView.AllowPaging Then
            Me.FullSizeNoPaging.Text = "Non Paginare"
            Me.FullSizeNoPaging.Icon.PrimaryIconUrl = "~/images/Next.png"
        Else
            Me.FullSizeNoPaging.Text = "Paginare"
            Me.FullSizeNoPaging.Icon.PrimaryIconUrl = "~/images/Previous.png"
        End If
        Me.FullSizeProtocolliGridView.Rebind()
    End Sub

    'Ripulisce una stringa dai caratteri problematici.
    Private Function Escape(ByVal s As String) As String
        Dim res As String = String.Empty

        If Not String.IsNullOrEmpty(s) Then
            s = s.Replace(vbCrLf, " ")
            s = s.Replace(vbTab, "")
            s = s.Replace("""", """""")
            Dim formatString As String = """{0}"""
            res = String.Format(formatString, s)
        End If

        Return res
    End Function


    'esegue la espostazione in formato xls della griglia delle Registrazioni di Protocollo
    Protected Sub EsportaInExcelImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EsportaInExcelImageButton.Click

        Dim trovata As Boolean = False
        For Each p In Me.Registrazioni
            trovata = True
            Exit For
        Next
        If Not trovata Then
            ParsecUtility.Utility.MessageBox("Non ci sono registrazioni di protocollo." & vbCrLf & "Impossibile eseguire l'esportazione!", False)
            Exit Sub
        End If

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("Registrazioni_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As New StringBuilder

        line.Append("N. PROT." & vbTab)
        line.Append("DATA PROT." & vbTab)
        line.Append("TIPOLOGIA" & vbTab)
        line.Append("OGGETTO" & vbTab)
        line.Append("MITTENTI/DESTINATARI" & vbTab)
        line.Append("UFFICI" & vbTab)

        swExport.WriteLine(line.ToString)
        line.Clear()

        For Each p In Me.Registrazioni

            line.Append("'" & p.NumeroProtocollo.ToString.PadLeft(7, "0") & vbTab)
            line.Append(String.Format("{0:dd-MM-yyyy}", p.DataImmissione) & vbTab)
            line.Append(p.DescrizioneTipologiaRegistristrazione & vbTab)

            'SOSTITUISCO TUTTO TRANNE I CARATTERI SPECIFICATI DAL RANGE 
            line.Append(System.Text.RegularExpressions.Regex.Replace(p.Oggetto, "[^\u0020-\u00FF]", " ") & vbTab)
            line.Append(System.Text.RegularExpressions.Regex.Replace(p.ElencoReferentiEsterni, "[^\u0020-\u00FF]", " ") & vbTab)
            line.Append(System.Text.RegularExpressions.Regex.Replace(p.ElencoReferentiInterni, "[^\u0020-\u00FF]", " ") & vbTab)

            swExport.WriteLine(line.ToString)
            line.Clear()

        Next
        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)


    End Sub

#End Region

#Region "EVENTI USER CONTROL EMAIL"

    'Evento OnCloseEvent su VisualizzaEmailControl. Nasconde il controllo VisualizzaEmailControl.
    Protected Sub VisualizzaEmailControl_OnCloseEvent() Handles VisualizzaEmailControl.OnCloseEvent
        Me.VisualizzaEmailControl.Visible = False
    End Sub

#End Region

End Class