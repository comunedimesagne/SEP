Imports ParsecAdmin
Imports Telerik.Web.UI
Imports ParsecPro
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

'* SPDX-License-Identifier: GPL-3.0-only

'Classe di Appoggio
Public Class Allegato
    Public Property Id As String
    Public Property Nomefile As String
    Public Property Content As Byte()
End Class


Partial Class VisualizzaRegistrazionePage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    'Variabile di Sessione: filtro di ricerca
    Public Property Filtro As String
        Set(ByVal value As String)
            Session("VisualizzaRegistrazionePage_Filtro") = value
        End Set
        Get
            Return Session("VisualizzaRegistrazionePage_Filtro")
        End Get
    End Property

    'Variabile di Sessione: oggetto Registrazione corrente
    Public Property Registrazione() As ParsecPro.Registrazione
        Get
            Return CType(Session("VisualizzaRegistrazionePage_Registrazione"), ParsecPro.Registrazione)
        End Get
        Set(ByVal value As ParsecPro.Registrazione)
            Session("VisualizzaRegistrazionePage_Registrazione") = value
        End Set
    End Property

    'Variabile di Sessione: lista degli Allegati associati alla Registrazione
    Public Property AllegatiEmail As List(Of Allegato)
        Get
            Return CType(Session("VisualizzaRegistrazionePage_AllegatiEmail"), List(Of Allegato))
        End Get
        Set(ByVal value As List(Of Allegato))
            Session("VisualizzaRegistrazionePage_AllegatiEmail") = value
        End Set
    End Property

    'Variabile di Sessione: oggetto Email 
    Public Property Email As Rebex.Mail.MailMessage
        Get
            Return CType(Session("VisualizzaRegistrazionePage_Email"), Rebex.Mail.MailMessage)
        End Get
        Set(ByVal value As Rebex.Mail.MailMessage)
            Session("VisualizzaRegistrazionePage_Email") = value
        End Set
    End Property

  
#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then

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

            Me.CaricaPosizioneTimbro()

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

        Me.ChiudiAnteprimaEmailButton.Attributes.Add("onclick", "HidePanel();hide=true;return false;")

    End Sub

    'Evento Load della Pagina: preleva i parametri
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Me.GetParametri()
        End If
    End Sub

    'Evento LoadComplete: gestisce le impostazioni generali.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")

        Me.AggiornaVista(CInt(Filtro))
        Me.ImpostaUiPagina(Me.Registrazione.TipologiaRegistrazione)
        Me.ImpostaAbilitazioneUi()

    End Sub

#End Region

#Region "METODI PRIVATI"

    'Gestisce la visibilità degli oggi della interfaccia grafica
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
        Me.DocumentiGridView.MasterTableView.Columns.FindByUniqueName("Preview").Visible = True
        Me.DocumentiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.DescrizioneDocumentoTextBox.ReadOnly = True
        Me.NumeroDocumentiTextBox.ReadOnly = True
        Me.DocumentoPrimarioRadioButton.Enabled = False
        Me.DocumentoAllegatoRadioButton.Enabled = False
        Me.AllegatoUpload.Enabled = False


        '******************************************************************************************

        '******************************************************************************************
        'Scheda Avanzate
        '******************************************************************************************

        Me.EliminaNoteImageButton.Visible = False
        Me.NoteTextBox.ReadOnly = True
        Me.EliminaNoteInterneImageButton.Visible = False
        Me.NoteInterneTextBox.ReadOnly = True
        Me.ProtocolloMittenteTextBox.ReadOnly = True

        Me.FiltroCategoriaTextBox.Visible = False
        Me.FiltroClasseTextBox.Visible = False
        Me.FiltroSottoClasseTextBox.Visible = False
        Me.FiltraClassificazioneImageButton.Visible = False

        Me.FiltroDescrizioneClassificazioneTextBox.Visible = False
        Me.TrovaClassificazioneImageButton.Visible = False
        Me.ClassificazioneTextBox.ReadOnly = True

        Me.EliminaClassificazioneImageButton.Visible = False

        '******************************************************************************************

        '******************************************************************************************
        'Scheda Generale
        '******************************************************************************************


        Me.FiltroDenominazioneTextBox.Visible = False
        Me.TrovaReferenteEsternoImageButton.Visible = False
        Me.TrovaPrimoReferenteInternoImageButton.Visible = False
        Me.AggiungiNuovoReferenteEsternoImageButton.Visible = False
        Me.FiltroSecondoReferenteInternoTextBox.Visible = False
        Me.TrovaSecondoReferenteInternoImageButton.Visible = False
        Me.TrovaOggettoImageButton.Visible = False


        Me.OggettoTextBox.ReadOnly = True
        Me.TipologiaIterComboBox.Enabled = False

        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("Modifica").Visible = False
        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione <> ParsecPro.TipoRegistrazione.Arrivo
        Me.ReferentiEsterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxIterTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione <> ParsecPro.TipoRegistrazione.Arrivo

        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo
        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxIterTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo
        Me.SecondoReferentiInterniGridView.MasterTableView.Columns.FindByUniqueName("CheckBoxInviaEmailTemplateColumn").Visible = Me.Registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo
        '******************************************************************************************

        Me.VisibilitaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False

        Me.TrovaGruppoVisibilitaImageButton.Visible = False
        Me.TrovaUtenteVisibilitaImageButton.Visible = False


    End Sub

    'Preleva i èarametri da ParsecUtility.SessionManager.ParametriPagina e li imposta sulla pagina
    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("Filtro") Then
                Me.Filtro = parametriPagina("Filtro")
            End If
            If parametriPagina.ContainsKey("VisualizzaAllegati") Then
                If parametriPagina.ContainsKey("VisualizzaAllegati") = True Then
                    Me.DatiProtocolloTabStrip.MultiPage.PageViews(2).Selected = True
                    Me.DatiProtocolloTabStrip.Tabs(2).Selected = True
                End If
            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    'Richiamato da LoadComplete: imposta e text e i tooltip di alcuni oggetti grafici in base alla tipologia ti registrazione (se Arrivo o se Partenza/interna)
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

    'Carica le informazioni della Registrazione di Protocollo il cui Id è passato come parametro
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


        '********************************************************************************************
        'SELEZIONO L'ITER
        '********************************************************************************************
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza = istanze.Where(Function(c) c.IdDocumento = Registrazione.Id And c.IdModulo = ParsecAdmin.TipoModulo.PRO).FirstOrDefault
        If Not istanza Is Nothing Then
            Me.TipologiaIterComboBox.Items.FindItemByValue(istanza.IdModello).Selected = True
        End If
        istanze.Dispose()
        '********************************************************************************************

        '*********************************************************************************
        'GESTIONE TOOLTIP INFO OPERAZIONI
        '*********************************************************************************

        Dim filtro As New ParsecPro.RegistrazioneFiltro With
           {
             .NumeroProtocolloInizio = Registrazione.NumeroProtocollo,
             .DataProtocolloInizio = Registrazione.DataImmissione,
             .Partenza = (Registrazione.TipoRegistrazione = TipoRegistrazione.Partenza),
             .Arrivo = (Registrazione.TipoRegistrazione = TipoRegistrazione.Arrivo),
             .Interna = (Registrazione.TipoRegistrazione = TipoRegistrazione.Interna)
          }

        Dim nl As Integer = 1
        Dim messaggio As String = String.Empty

        Dim elencoRegistrazioni = registrazioni.GetInfoRegistrazioni(filtro)

        Dim cnt = elencoRegistrazioni.Cast(Of Object).Count()

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


    End Sub

    'Carica le informazioni avanzate della Registrazione di Protocollo
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
                    Me.TipoRicezioneInvioComboBox.Items.Add(New RadComboBoxItem(tipo.Descrizione.ToUpper, -2))
                    Me.TipoRicezioneInvioComboBox.FindItemByValue(-2).Selected = True
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

    'Aggiorna il conteggio del numero di allegati
    Private Sub AggiornaPannelloDocumenti(ByVal registrazione As ParsecPro.Registrazione)
        If Not registrazione.NumeroAllegati Is Nothing Then
            Me.NumeroDocumentiTextBox.Text = registrazione.NumeroAllegati
        End If
    End Sub

    'Visualizza il P7M
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

    'Effettua il downlaod del file
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


                If file.Extension.ToLower = ".eml" Then

                    Try
                        Me.AnteprimaEmail(file.FullName)
                        Session("EmailAttachmentFullName") = file.FullName
                        Dim script As New Text.StringBuilder
                        script.AppendLine("<script language='javascript'>")
                        script.AppendLine("ShowPanel();hide=false;")
                        script.AppendLine("</script>")
                        ParsecUtility.Utility.RegisterScript(script, False)
                    Catch ex As Exception

                        ParsecUtility.Utility.MessageBox("Si è verificato un errore durante la lettura dell'email!" & vbCrLf & "Il file verrà aperto con l'applicazione associata.", False)
                        'IN CASO DI ERRORE VISUALIZZO IL FILE NON MODO CLASSICO
                        Session("AttachmentFullName") = file.FullName
                        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                        ParsecUtility.Utility.PageReload(pageUrl, False)

                    End Try

                Else
                    Session("AttachmentFullName") = file.FullName
                    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                    ParsecUtility.Utility.PageReload(pageUrl, False)
                End If

            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        End If
    End Sub

    'Carica la combo della posizone del timbro
    Private Sub CaricaPosizioneTimbro()
        Dim dati As New Dictionary(Of String, String)
        dati.Add("1", "Sopra")
        dati.Add("2", "Sotto")
        dati.Add("3", "Destra")
        dati.Add("4", "Sinistra")
        Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})
        Me.PosizioneTimbroComboBox.DataSource = ds
        Me.PosizioneTimbroComboBox.DataTextField = "Descrizione"
        Me.PosizioneTimbroComboBox.DataValueField = "Id"
        Me.PosizioneTimbroComboBox.DataBind()
        Me.PosizioneTimbroComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.PosizioneTimbroComboBox.SelectedIndex = 3

    End Sub

    'Visaulizza l'anteprima della mail
    Private Sub AnteprimaEmail(ByVal fullPathEmail As String)

        Me.Email = Nothing
        Me.OggettoEmailLabel.Text = String.Empty
        Me.DataEmailLabel.Text = String.Empty
        Me.MittenteEmailLabel.Text = String.Empty
        Me.DestinatarioEmailLabel.Text = String.Empty
        Me.contenutoEmail.Text = String.Empty

        Dim innerMessage As Rebex.Mail.MailMessage = Nothing
        Dim ms As IO.MemoryStream = Nothing
        Dim lista As New List(Of Allegato)
        Dim message As New Rebex.Mail.MailMessage
        message.Load(fullPathEmail)


        Dim attEmail = message.Attachments.Where(Function(c) c.FileName.ToLower.EndsWith(".eml")).FirstOrDefault

        If Not attEmail Is Nothing Then
            innerMessage = New Rebex.Mail.MailMessage
            ms = New IO.MemoryStream
            attEmail.Save(ms)
            ms.Position = 0
            innerMessage.Load(ms)
            Me.Email = innerMessage
        Else
            Me.Email = message
        End If


        Me.OggettoEmailLabel.Text = "<span><b>Oggetto:</b></span>&nbsp" & Me.Email.Subject
        Try
            Me.DataEmailLabel.Text = "<span><b>Inviata:</b></span>&nbsp" & Me.Email.Date.OriginalTime.ToString("dddd dd MMMM yyyy")
        Catch ex As Exception
            Try
                Me.DataEmailLabel.Text = "<span><b>Inviata:</b></span>&nbsp" & message.Date.OriginalTime.ToString("dddd dd MMMM yyyy")
            Catch ex2 As Exception

            End Try
        End Try

        Me.MittenteEmailLabel.Text = "<span><b>Da:</b></span>&nbsp" & Me.Email.From.ToString
        Me.DestinatarioEmailLabel.Text = "<span><b>A:</b></span>&nbsp" & Me.Email.To.ToString

        Dim i As Integer = 0
        If Me.Email.HasBodyHtml Then
            Me.contenutoEmail.Text = Me.Email.BodyHtml
        Else
            Me.contenutoEmail.Text = Me.Email.BodyText.Replace(Chr(10), "</br>")
        End If

        Dim content As Byte() = Nothing
        Dim estensione As String = String.Empty
        Dim fatturaTrovata As Boolean = False

        For Each innerAtt In Me.Email.Attachments
            i += 1
            ms = New IO.MemoryStream
            innerAtt.Save(ms)
            ms.Position = 0
            content = ms.ToArray
            lista.Add(New Allegato With {.Id = i, .Nomefile = innerAtt.FileName, .Content = content})

            estensione = IO.Path.GetExtension(innerAtt.FileName)
            Select Case estensione.ToLower
                Case ".p7m"
                    'estrarre il documento interno se è un pdf 
                    Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
                    Try
                        content = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(content))
                    Catch ex As Exception
                        'NIENTE
                    End Try

                    Try
                        signedCms.Decode(content)
                        content = signedCms.ContentInfo.Content
                        estensione = "." & innerAtt.FileName.Split(".")(1)
                    Catch ex As Exception
                        content = System.Text.ASCIIEncoding.Default.GetBytes("Il file p7m non è valido per il seguente motivo:" & vbCrLf & ex.Message)
                        estensione = ".txt"
                    End Try

            End Select

            If Not fatturaTrovata Then
                If estensione.ToLower = ".xml" Then
                    Dim msxml As IO.MemoryStream = ParsecUtility.Utility.FixVersioneXml(content)
                    Try
                        Dim element = XElement.Load(msxml)
                        If Not element Is Nothing Then
                            Dim header = element.Element("FatturaElettronicaHeader")
                            If Not header Is Nothing Then
                                fatturaTrovata = True
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                End If
            End If
        Next

        Me.VisualizzaFatturaButton.Visible = fatturaTrovata

        Me.AllegatiEmail = lista
        Me.AllegatiEmailGridView.DataSource = lista
        Me.AllegatiEmailGridView.DataBind()

        AllegatiEmailLabel.Text = "Allegati" & If(lista.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & lista.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        Me.PosizioneTimbroComboBox.SelectedIndex = 3

    End Sub

    'Lancia i comandi dalla griglia AllegatiEmailGridView: scarica gli allegati
    Protected Sub AllegatiEmailGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiEmailGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.DownloadAllegatoEmail(e.Item)
        End If
    End Sub

    'Visualizza la fattura elettroncia
    Private Sub VisualizzaFattura()
        If Not Me.Registrazione Is Nothing Then
            Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
            Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = Me.Registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = Me.Registrazione.NumeroProtocollo).FirstOrDefault
            If Not fattura Is Nothing Then
                Dim queryString As New Hashtable
                Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaFatturaPage.aspx"
                queryString.Add("IdFatturaElettronica", fattura.Id.ToString)
                ParsecUtility.Utility.ShowPopup(pageUrl, 900, 600, queryString, False)
            End If
        End If

    End Sub

    'lancia la visualizazzione della fattura
    Protected Sub VisualizzaFatturaButton_Click(sender As Object, e As System.EventArgs) Handles VisualizzaFatturaButton.Click
        Me.VisualizzaFattura()
    End Sub

    'Scarica l' allegato della mail
    Private Sub DownloadAllegatoEmail(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim posizione = Me.PosizioneTimbroComboBox.SelectedItem.Index

        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As Allegato = Me.AllegatiEmail.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then

            Dim estensione As String = IO.Path.GetExtension(filename)
            Dim content As Byte() = allegato.Content

            Select Case estensione.ToLower
                Case ".pdf"

                    If posizione <> 0 Then
                        If Not Me.Registrazione Is Nothing Then
                            Dim registrazioni As New ParsecPro.RegistrazioniRepository
                            Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                            Dim watermark As String = cliente.Descrizione & " - Cod. Amm. " & cliente.CodiceAmministrazione & " - Prot. n. " & Me.Registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy HH:mm}", Me.Registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(Me.Registrazione.TipoRegistrazione).ToUpper
                            content = AddWatermarkToPdf(watermark, posizione, content)
                        End If
                    End If

                Case ".p7m"
                    'todo
                Case Else
                    'todo
            End Select

            Dim ht As New Hashtable

            Dim est As String = IO.Path.GetExtension(filename)
            Select Case est.ToLower
                Case ".p7m"

                    ht.Add("Content", allegato.Content)
                    ht.Add("Extension", ".p7m")

                Case Else
                    ht.Add("Content", content)
                    ht.Add("Extension", estensione)
            End Select

            ht.Add("Filename", filename)
            Session("AttachmentFullName") = ht
            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"

            If estensione.ToLower = ".pdf" Then
                ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
            Else
                ParsecUtility.Utility.PageReload(pageUrl, False)
            End If

        End If
    End Sub

    'Stampa la mail
    Protected Sub StampaEmailButton_Click(sender As Object, e As System.EventArgs) Handles StampaEmailButton.Click

        Dim posizione As Nullable(Of Integer) = Nothing
        If Me.PosizioneTimbroComboBox.Visible Then
            posizione = Me.PosizioneTimbroComboBox.SelectedItem.Index
        End If

        Dim bytes As Byte() = Nothing
        Dim pdfDocument As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4)
        Using memoryStream As New MemoryStream()
            Dim writer As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocument, memoryStream)
            pdfDocument.Open()

            Dim mainTable As New iTextSharp.text.pdf.PdfPTable(1) With {.WidthPercentage = 100}

            Dim table As New iTextSharp.text.pdf.PdfPTable(2) With {.WidthPercentage = 100}
            table.SetWidths(New Single() {20.0F, 80.0F})

            Dim fontBold As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)
            Dim fontNormal As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL)

            Dim cell As New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("Oggetto: ", fontBold))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(Me.Email.Subject, fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)


            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("Inviata: ", fontBold))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(Me.Email.Date.OriginalTime.ToString("dddd dd MMMM yyyy HH:mm"), fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("Da: ", fontBold))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(Me.Email.From.ToString, fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("A: ", fontBold))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(Me.Email.To.ToString, fontNormal))
            cell.Border = 0
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            If Me.Email.Attachments.Count > 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("Allegati: ", fontBold))
                cell.Border = iTextSharp.text.Rectangle.NO_BORDER
                cell.Padding = 0
                cell.UseDescender = True
                table.AddCell(cell)

                Dim allegati As String = Me.Email.Attachments.Select(Function(c) c.DisplayName).Aggregate(Function(c, n) c & " - " & n)


                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(allegati, fontNormal))
                cell.Border = 0
                cell.Padding = 0
                cell.UseDescender = True
                table.AddCell(cell)
            End If

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", fontNormal))

            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER
            cell.Padding = 0
            cell.FixedHeight = 10
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", fontNormal))
            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER
            cell.FixedHeight = 10
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)


            Dim bodyTable As New iTextSharp.text.pdf.PdfPTable(1) With {.WidthPercentage = 100}
            bodyTable.SetWidths(New Single() {100.0F})

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(vbCrLf, fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            bodyTable.AddCell(cell)

            Dim corpo As String = Me.ExtractTextFromHtml(Me.contenutoEmail.Text)
            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(corpo, fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            bodyTable.AddCell(cell)


            Dim mainCell As New iTextSharp.text.pdf.PdfPCell
            mainCell.Border = iTextSharp.text.Rectangle.NO_BORDER

            mainCell.AddElement(table)
            mainCell.AddElement(bodyTable)


            mainTable.AddCell(mainCell)


            pdfDocument.Add(mainTable)


            pdfDocument.Close()
            bytes = memoryStream.ToArray()
            memoryStream.Close()

        End Using

        If posizione.HasValue Then
            If posizione <> 0 Then
                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente

                Dim watermark As String = cliente.Descrizione & " - Cod. Amm. " & cliente.CodiceAmministrazione & " - Prot. n. " & Registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy HH:mm}", Registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(Registrazione.TipoRegistrazione).ToUpper
                bytes = AddWatermarkToPdf(watermark, posizione, bytes)

            End If
        End If

        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaFattura")
        parametriStampa.Add("FullPath", bytes)
        Session("ParametriStampaPro") = parametriStampa
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)

    End Sub

    'Estrae il testo dall'Html
    Private Function ExtractTextFromHtml(s As String) As String
        Dim pattern As String = "<.*?>"

        Dim reg As Regex = New Regex("<[^>]+>", RegexOptions.IgnoreCase)
        Dim regexCss = New Regex("(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
        s = regexCss.Replace(s, String.Empty)

        s = reg.Replace(s, String.Empty)

        s = s.Replace("&nbsp;", String.Empty)
        s = HttpUtility.HtmlDecode(s).Trim
        Return s
    End Function


    'Visualizza la mail
    Protected Sub VisualizzaEmailButton_Click(sender As Object, e As System.EventArgs) Handles VisualizzaEmailButton.Click
        Session("AttachmentFullName") = Session("EmailAttachmentFullName")
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
    End Sub

    'Aggiunge la Segnatura al PDF
    Private Function AddWatermarkToPdf(ByVal watermark As String, ByVal posizione As Integer, ByVal bytes As Byte()) As Byte()
        Dim ms As New MemoryStream()
        Dim reader As New PdfReader(bytes)
        Dim pdfStamper As New PdfStamper(reader, ms)

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

                    'DESTRA
                Case 3

                    x = size.Width - 15
                    y = size.Height / 2
                    rot = -90
                    'SINISTRA
                Case 4

                    x = 15
                    y = (size.Top - size.Bottom) / 2
                    rot = 90

            End Select

            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermark, x, y, rot)

            cb.EndText()
            cb.RestoreState()
        Next
        pdfStamper.Close()
        reader.Close()
        bytes = ms.ToArray()
        ms.Close()
        Return bytes
    End Function

#End Region

#Region "EVENTI GRIGLIE"

    'Evento ItemDataBound associato alla DocumentiGridView. Setta i tooltip in base al contenuto delle celle.. 
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

    'Evento ItemCommand associato alla ProtocolliGridView. Fa partire i comandi associati alla griglia degli Allegati.
    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Preview"
                Me.DownloadFile(e.Item)
            Case "SignedPreview"
                Me.VisualizzaDocumentoP7M(e.Item)
        End Select
    End Sub

    'Evento ItemDataBound associato alla CollegamentiDirettiGridView. Setta i tooltip e definisce gli stili del "Delete" e del "Detail" . 
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

#End Region


End Class
