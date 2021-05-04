Imports ParsecAdmin
Imports ParsecPro
Imports Telerik.Web.UI
Imports System.IO
Imports System.Net

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class GestioneProtocollazioneMassivaFatturePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    'Variabile di Sessione: fattura elettronica corrente.
    Public Property FatturaElettronica() As ParsecPro.FatturaElettronica
        Get
            Return CType(Session("GestioneProtocollazioneMassivaFatturePage_FatturaElettronica"), ParsecPro.FatturaElettronica)
        End Get
        Set(ByVal value As ParsecPro.FatturaElettronica)
            Session("GestioneProtocollazioneMassivaFatturePage_FatturaElettronica") = value
        End Set
    End Property

    'Variabile di Sessione: lista delle fatture elettroniche associate alla Griglia delle Fatture.
    Public Property FattureElettroniche() As List(Of ParsecPro.FatturaElettronica)
        Get
            Return CType(Session("GestioneProtocollazioneMassivaFatturePage_FattureElettroniche"), List(Of ParsecPro.FatturaElettronica))
        End Get
        Set(ByVal value As List(Of ParsecPro.FatturaElettronica))
            Session("GestioneProtocollazioneMassivaFatturePage_FattureElettroniche") = value
        End Set
    End Property

    'Restiuisce il valore del Parametro AttivaGestioneScrivaniePro.
    Public ReadOnly Property IterAttivato As Boolean
        Get
            Dim res As Boolean = False
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AttivaGestioneScrivaniePro", ParsecAdmin.TipoModulo.SEP)
            If Not parametro Is Nothing Then
                res = CBool(parametro.Valore)
            End If
            parametri.Dispose()
            Return res
        End Get
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> Protocollazione Massiva Fatture Elettroniche"

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = "div.RadUploadProgressArea_Office2007 .ruProgress { background-image: none;}" & vbCrLf
        css.InnerHtml += ".RadUploadProgressArea { width: 320px !important;}" & vbCrLf
        css.InnerHtml += "div.RadUploadProgressArea li.ruProgressHeader{ margin: 10px 18px 0px; }" & vbCrLf
        css.InnerHtml += "table.CkeckListCss tr td label {margin-right:10px;padding-right:10px;}" & vbCrLf
        Me.Page.Header.Controls.Add(css)

        If Not Me.Page.IsPostBack Then

            Me.VisualizzaFatturaControl.Visible = False

            Me.FattureElettroniche = Nothing
            'Imposto l'ordinamento predefinito.
            Dim sortExpr As New Telerik.Web.UI.GridSortExpression()
            sortExpr.FieldName = "MessaggioSdI.DataRicezioneInvio"
            sortExpr.SortOrder = Telerik.Web.UI.GridSortOrder.Ascending
            Me.FattureElettronicheGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

        End If

        Me.ChiudiErrorPanelButton.Attributes.Add("onclick", "hideErrorPanel=true;return false;")

        Me.FattureElettronicheGridView.GroupingSettings.CaseSensitive = False

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.FattureElettronicheGridView.Style.Add("width", widthStyle)
    End Sub

    'Evento LoadComplete della Pagina: dopo che la pagina è stata caricata setta il titolo della Griglia.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloElencoFattureLabel.Text = "Elenco Fatture Elettroniche&nbsp;&nbsp;" & If(Me.FattureElettronicheGridView.Items.Count > 0, "( " & Me.FattureElettronicheGridView.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla griglia CaselleEmailGridView. Aggancia il datasource della griglia alla lista FattureElettroniche. Carica la variabile di sessione FattureElettroniche.
    Protected Sub FattureElettronicheGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FattureElettronicheGridView.NeedDataSource
        If Me.FattureElettroniche Is Nothing Then
            Dim fatture As New ParsecPro.FatturaElettronicaRepository
            Me.FattureElettroniche = fatture.GetView(Me.GetFiltro).OrderBy(Function(c) c.MessaggioSdI.DataRicezioneInvio).ToList
            fatture.Dispose()
        End If
        Me.FattureElettronicheGridView.DataSource = Me.FattureElettroniche
    End Sub

    'Evento ItemCommand associato alla Griglia FattureElettronicheGridView. Permette l'esecuzione dei vari comandi attivabili dalla griglia FattureElettronicheGridView.
    Protected Sub FattureElettronicheGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles FattureElettronicheGridView.ItemCommand
        If e.CommandName = "Anteprima" Then
            Me.VisualizzaFattura(e.Item)
        End If
    End Sub

    'Evento ItemCreated associato alla Griglia FattureElettronicheGridView. Gestisce  gli stili degliitem di tipo GridFilteringItem e GridHeaderItem.
    Protected Sub FattureElettronicheGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FattureElettronicheGridView.ItemCreated

        If TypeOf e.Item Is GridFilteringItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop)")
            e.Item.Style.Add("z-index", "99")
        End If

        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If

    End Sub

    'Visualizza la Fattura elettronica nelpannellino
    Private Sub VisualizzaFattura(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim fatture As New ParsecPro.FatturaElettronicaRepository
        Dim fattura = fatture.Where(Function(c) c.Id = id).FirstOrDefault

        If Not fattura Is Nothing Then
            Me.VisualizzaFatturaControl.ShowPanel()
            Me.VisualizzaFatturaControl.InitUI(fattura)
        End If
        fatture.Dispose()
    End Sub

#End Region

#Region "AZIONI PANNELLO FILTRO"

    'Resetta il filtro sulla griglia e le date di inzio e fine invio
    Private Sub ResettaFiltro()
        For Each col As GridColumn In Me.FattureElettronicheGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.FattureElettronicheGridView.MasterTableView.FilterExpression = String.Empty
        Me.DataInvioInizioTextBox.SelectedDate = Nothing
        Me.DataInvioFineTextBox.SelectedDate = Nothing
    End Sub

    'Costruisce e resituisce il filtro per la ricerca.
    Private Function GetFiltro() As ParsecPro.FiltroFatturaElettronica
        Dim filtro As New ParsecPro.FiltroFatturaElettronica

        filtro.DataInizioRicezioneInvio = Me.DataInvioInizioTextBox.SelectedDate
        filtro.DataFineRicezioneInvio = Me.DataInvioFineTextBox.SelectedDate
        filtro.ElencoStati.Add(StatoFattura.Ricevuta)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        filtro.IdUtenteCollegato = utenteCollegato.Id

        Return filtro
    End Function

    'Aggiorna la griglia FattureElettronicheGridView
    Private Sub AggiornaGriglia()
        Me.FattureElettroniche = Nothing
        Me.FattureElettronicheGridView.Rebind()
    End Sub

    'Avvia la Ricerca delle Fatture
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FiltraImageButton.Click
        Me.AggiornaGriglia()
    End Sub

    'Resetta il filtro e riaggiorna la griglia.
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.AggiornaGriglia()
    End Sub

    'Ricerca il Destinatario tramite la maschera RicercaOrganigrammaPage.aspx. Quando si chiude la maschera in questione scatta il metodo AggiornaDestinatarioImageButton
    Protected Sub TrovaDestinatarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaDestinatarioImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaDestinatarioImageButton.ClientID)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("idModulo", 2)
        parametriPagina.Add("idUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100,200,300,400")
        parametriPagina.Add("ultimoLivelloStruttura", "400")

        parametriPagina.Add("ApplicaAbilitazioni", False)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    'Evento che parte alla chiusura della maschera di ricerca RicercaOrganigrammaPage.aspx (evento TrovaDestinatarioImageButton.Click) 
    Protected Sub AggiornaDestinatarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaDestinatarioImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim struttura = struttureSelezionate.First
            Me.DestinatarioTextBox.Text = struttura.Descrizione
            Me.IdDestinatarioTextBox.Text = struttura.Id.ToString
            Me.CodiceDestinatarioTextBox.Text = struttura.Codice.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    'Svuota i campi DestinatarioTextBox, IdDestinatarioTextBox e CodiceDestinatarioTextBox
    Protected Sub EliminaDestinatarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaDestinatarioImageButton.Click
        Me.DestinatarioTextBox.Text = String.Empty
        Me.IdDestinatarioTextBox.Text = String.Empty
        Me.CodiceDestinatarioTextBox.Text = String.Empty
    End Sub


#End Region

#Region "PROTOCOLLAZIONE MASSIVA"

    'Legge il file della fattura elettronica (viene passato come parametro come XElement)
    Private Function LeggiFattura(fattura As XElement) As ParsecPro.Registrazione
        Dim registrazione As New Registrazione

        Dim header = fattura.Element("FatturaElettronicaHeader")

        'Destinatario Fattura
        Dim codiceIpaDestinatario = header.Element("DatiTrasmissione").Element("CodiceDestinatario").Value

        'Mittente fattura
        Dim cedentePrestatore = header.Element("CedentePrestatore")
        Dim datiAnagrafici = cedentePrestatore.Element("DatiAnagrafici")
        Dim anagrafica = datiAnagrafici.Element("Anagrafica")

        Dim denominazione As String = String.Empty
        Dim nome As String = String.Empty
        Dim cognome As String = String.Empty
        Dim codicePartitaIva As String = String.Empty
        Dim codiceFiscale As String = String.Empty
        Dim numeroCivico As String = String.Empty
        Dim provincia As String = String.Empty
        Dim email As String = String.Empty

        If Not anagrafica.Element("Denominazione") Is Nothing Then
            denominazione = anagrafica.Element("Denominazione").Value
        End If

        If Not anagrafica.Element("Nome") Is Nothing Then
            nome = anagrafica.Element("Nome").Value
        End If
        If Not anagrafica.Element("Cognome") Is Nothing Then
            cognome = anagrafica.Element("Cognome").Value
        End If

        'Il numero di identificazione fiscale ai fini IVA
        Dim fiscalePartitaIva = datiAnagrafici.Element("IdFiscaleIVA")
        If Not fiscalePartitaIva.Element("IdCodice") Is Nothing Then
            codicePartitaIva = fiscalePartitaIva.Element("IdCodice").Value
        End If

        Dim contatti = cedentePrestatore.Element("Contatti")
        If Not contatti Is Nothing Then
            If Not contatti.Element("Email") Is Nothing Then
                email = contatti.Element("Email").Value
            End If
        End If

        Dim sede = cedentePrestatore.Element("Sede")

        Dim indirizzo = sede.Element("Indirizzo").Value
        If Not sede.Element("NumeroCivico") Is Nothing Then
            numeroCivico = sede.Element("NumeroCivico").Value
        End If

        Dim cap = sede.Element("CAP").Value
        Dim comune = sede.Element("Comune").Value
        If Not sede.Element("Provincia") Is Nothing Then
            provincia = sede.Element("Provincia").Value
        End If

        Dim oggetto As String = String.Empty

        Dim body = fattura.Elements("FatturaElettronicaBody")
        For Each datiGenerali In body
            Dim datiGeneraliDocumento = datiGenerali.Element("DatiGenerali").Element("DatiGeneraliDocumento")

            Dim numerofattura = datiGeneraliDocumento.Element("Numero").Value

            Dim descrizioneTipoDocumento As String = "Fattura"
            If Not datiGeneraliDocumento.Element("TipoDocumento") Is Nothing Then
                Dim tipoDocumento = datiGeneraliDocumento.Element("TipoDocumento").Value
                Select Case tipoDocumento
                    Case "TD01"
                        descrizioneTipoDocumento = "Fattura"
                    Case "TD02"
                        descrizioneTipoDocumento = "Acconto/anticipo su fattura"
                    Case "TD03"
                        descrizioneTipoDocumento = "Acconto/anticipo su parcella"
                    Case "TD04"
                        descrizioneTipoDocumento = "Nota di credito"
                    Case "TD05"
                        descrizioneTipoDocumento = "Nota di debito"
                    Case "TD06"
                        descrizioneTipoDocumento = "Parcella"
                End Select
            End If

            oggetto &= descrizioneTipoDocumento & " n. " & numerofattura.ToString
            Dim datafattura = Date.Parse(datiGeneraliDocumento.Element("Data").Value).ToShortDateString

            oggetto &= " del " & datafattura & vbCrLf


            If Not datiGeneraliDocumento.Element("Causale") Is Nothing Then
                Dim causalefattura = datiGeneraliDocumento.Element("Causale").Value
            End If

        Next

        If Not String.IsNullOrEmpty(Me.IdDestinatarioTextBox.Text) Then
            Dim dest = New ParsecPro.Destinatario(CInt(Me.IdDestinatarioTextBox.Text), True)
            dest.Iter = True
            registrazione.Destinatari.Add(dest)
        Else
            Dim destinatarioTrovato As Boolean = False

            '*******************************************************************************************************************
            'RICERCO IL DESTINATARIO PER CIG
            'SE IL PARAMETRO CHE ABILITA QUESTA FUNZIONALITA' ESISTE ED E' IMPOSTATO SU TRUE
            '*******************************************************************************************************************
            Dim cig As String = String.Empty

            Dim codiceCIG = fattura.Descendants("CodiceCIG").GroupBy(Function(c) c).Select(Function(c) c.FirstOrDefault).ToList

            If codiceCIG.Count = 1 Then
                cig = codiceCIG.FirstOrDefault

                Dim impegni As New ParsecAtt.ImpegnoSpesaRepository

                Dim atti As New ParsecAtt.DocumentoRepository(impegni.Context)

                Dim impegniConCig = (From impegno In impegni.GetQuery
                           Join atto In atti.GetQuery
                           On impegno.IdDocumento Equals atto.Id
                           Where atto.LogStato Is Nothing And impegno.CIG = cig And atto.IdTipologiaDocumento = 2
                           Select impegno).ToList

                impegni.Dispose()

                If impegniConCig.Count = 1 Then

                    Dim documenti As New ParsecAtt.DocumentoRepository
                    Dim idDocumento = impegniConCig.FirstOrDefault.IdDocumento
                    Dim documento = documenti.Where(Function(c) c.Id = idDocumento).FirstOrDefault
                    documenti.Dispose()

                    If Not documento Is Nothing Then
                        Dim strutture As New ParsecAtt.StrutturaViewRepository
                        Dim struttura = strutture.Where(Function(c) c.Id = documento.IdStruttura And (c.LogStato Is Nothing Or c.LogStato = "M")).FirstOrDefault
                        If Not struttura Is Nothing Then
                            If struttura.LogStato = "M" Then
                                struttura = strutture.Where(Function(c) c.Codice = struttura.Codice And c.LogStato Is Nothing).FirstOrDefault
                                If Not struttura Is Nothing Then
                                    Dim dest = New ParsecPro.Destinatario(struttura.Id, True)
                                    dest.Iter = True
                                    registrazione.Destinatari.Add(dest)
                                    destinatarioTrovato = True
                                End If
                            Else
                                Dim dest = New ParsecPro.Destinatario(struttura.Id, True)
                                dest.Iter = True
                                registrazione.Destinatari.Add(dest)
                                destinatarioTrovato = True
                            End If
                        End If
                        strutture.Dispose()
                    End If
                End If
            End If

            '*******************************************************************************************************************

            '*******************************************************************************************************************
            'RICERCO IL DESTINATARIO PER CODICDE IPA
            '*******************************************************************************************************************
            If Not destinatarioTrovato Then
                Dim referenti As New ParsecAdmin.StructureRepository
                Dim referenteInterno = referenti.Where(Function(c) c.CodiceIPA = codiceIpaDestinatario And c.LogStato Is Nothing).FirstOrDefault
                referenti.Dispose()

                If Not referenteInterno Is Nothing Then
                    Dim dest = New ParsecPro.Destinatario(referenteInterno.Id, True)
                    dest.Iter = True
                    registrazione.Destinatari.Add(dest)
                End If
            End If
            '*******************************************************************************************************************
        End If

        '*******************************************************************************************************************
        'RICERCO IL MITTENTE
        '*******************************************************************************************************************
        Dim mittente As ParsecPro.Mittente = Nothing

        'CERCO L'ULTIMO
        Dim rubrica As New RubricaRepository
        Dim strutturaEsterna As StrutturaEsternaInfo = rubrica.GetQuery.Where(Function(c) c.CodiceFiscale = codicePartitaIva And c.LogStato Is Nothing).OrderByDescending(Function(c) c.Id).FirstOrDefault
        rubrica.Dispose()

      
        Dim mittenteTrovato As Boolean = False
        If Not strutturaEsterna Is Nothing Then
            If (strutturaEsterna.Denominazione = denominazione OrElse strutturaEsterna.Denominazione = cognome) AndAlso strutturaEsterna.Nome = nome AndAlso strutturaEsterna.Comune = comune AndAlso strutturaEsterna.CAP = cap Then
                mittente = New Mittente(strutturaEsterna.Id, False)
                mittenteTrovato = True
            End If
        End If
        If Not mittenteTrovato Then
            mittente = New Mittente
            mittente.Rubrica = False
            mittente.Indirizzo = indirizzo & If(Not String.IsNullOrEmpty(numeroCivico), ", " & numeroCivico, "")
            mittente.Cap = cap
            mittente.Provincia = provincia
            mittente.Citta = comune
            mittente.Email = email
            mittente.CodiceFiscalePartitaIva = codicePartitaIva
        End If

       
        If String.IsNullOrEmpty(denominazione) Then
            mittente.Cognome = cognome
            mittente.Nome = nome
            mittente.Descrizione = cognome & " " & nome
        Else
            mittente.Cognome = denominazione
            mittente.Descrizione = denominazione
        End If

        registrazione.Mittenti.Add(mittente)
        '*******************************************************************************************************************

        registrazione.Oggetto = oggetto

        Return registrazione
    End Function

    'Ritorna l'Allegato della email
    Private Function GetAllegatoEmail(ByVal mailMessage As ParsecPro.EmailArrivo) As ParsecPro.Allegato

        Dim nomefile As String = Guid.NewGuid.ToString & ".eml"
        Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
        Dim fullPathEmail As String = mailBoxPath & mailMessage.PercorsoRelativo & mailMessage.NomeFileEml
        Dim tipoEmail As ParsecPro.Pop3.Header.TipologiaEmail = CType(mailMessage.Tipo, ParsecPro.Pop3.Header.TipologiaEmail)

        'Copio l'allegato nella cartella temporanea.
        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
        IO.File.Copy(fullPathEmail, pathDownload)
        Dim allegato As New ParsecPro.Allegato
        allegato.NomeFile = nomefile
        allegato.NomeFileTemp = nomefile
        allegato.IdTipologiaDocumento = 1 'Primario
        allegato.DescrizioneTipologiaDocumento = "Primario"

        Select Case tipoEmail
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_Normale
                allegato.Oggetto = "Messaggio e-mail"
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC
                Dim parametri As New ParsecAdmin.ParametriRepository
                Dim parametro = parametri.GetByName("PEC_DescrizioneMailIn", ParsecAdmin.TipoModulo.PRO)
                parametri.Dispose()
                If Not parametro Is Nothing Then
                    allegato.Oggetto = parametro.Valore
                Else
                    allegato.Oggetto = "Messaggio originale da PEC"
                End If
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC_Anomalia
                allegato.Oggetto = "Messaggio PEC con anomalia"
        End Select
        allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
        Return allegato
    End Function

    'Ritorna se, per la struttura passata come parametro, è attivabile l'Iter oppure no
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

    'Evento click che protocolla le fatture selezionate nella griglia.
    Protected Sub ProtocollaFattureSelezionateImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ProtocollaFattureSelezionateImageButton.Click

        If Me.FattureElettronicheGridView.SelectedIndexes.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno una fattura!", False)
            Exit Sub
        End If

        If Not Me.IterAttivato Then
            ParsecUtility.Utility.MessageBox("E' necessario abilitare l'iter del protocollo!", False)
            Exit Sub
        End If

        If Not String.IsNullOrEmpty(Me.IdDestinatarioTextBox.Text) Then
            Dim avviabile = Me.IterAvviabile(Me.IdDestinatarioTextBox.Text)
            If Not avviabile Then
                ParsecUtility.Utility.MessageBox(String.Format("Per il destinatario {0} non è avviabile l'iter!", Me.DestinatarioTextBox.Text), False)
                Exit Sub
            End If
        End If

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim fatture As New ParsecPro.FatturaElettronicaRepository

        Dim sb As New StringBuilder

        Dim count = Me.FattureElettronicheGridView.SelectedItems.Count
        Dim context = RadProgressContext.Current
        context.PrimaryTotal = count.ToString
        Dim i As Integer = 0


        Dim percentCompleted As Integer = 0

        Dim fattura As ParsecPro.FatturaElettronica = Nothing

        Dim selectedItem As GridDataItem = Nothing
        For j As Integer = 0 To Me.FattureElettronicheGridView.SelectedIndexes.Count - 1

            i += 1
            context.PrimaryValue = i.ToString
            percentCompleted = CInt((0.5F + ((100.0F * i) / count)))
            context.PrimaryPercent = percentCompleted.ToString

            If Not Response.IsClientConnected Then
                Exit For
            End If
            selectedItem = CType(Me.FattureElettronicheGridView.Items(Me.FattureElettronicheGridView.SelectedIndexes(j)), GridDataItem)

            Try
                Dim idSelezionato As Integer = selectedItem.OwnerTableView.DataKeyValues(selectedItem.ItemIndex)("Id")
                fattura = fatture.Where(Function(c) c.Id = idSelezionato).FirstOrDefault
                Me.ProtocollaFattura(fattura, utenteCorrente)
            Catch ex As Exception
                If sb.Length = 0 Then
                    sb.AppendLine("<center><span style=""text-align:center;""><b>ELENCO FATTURE NON PROTOCOLLATE</b></span></center></br></br>")
                End If
                sb.AppendLine("&nbsp" & fattura.Oggetto & "</br>&nbspMOTIVO: " & ex.Message & "</br></br>")

            End Try
        Next

        fatture.Dispose()

        context.OperationComplete = True

        For Each col As GridColumn In Me.FattureElettronicheGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.FattureElettronicheGridView.MasterTableView.FilterExpression = String.Empty
        Me.DataInvioInizioTextBox.SelectedDate = Nothing
        Me.DataInvioFineTextBox.SelectedDate = Nothing

        If sb.Length = 0 Then
            Me.infoOperazioneHidden.Value = "Elaborazione conclusa con successo!"
        Else

            Me.VisualizzaErrorMessageBox(sb.ToString)

        End If

        Me.AggiornaGriglia()

    End Sub

    'Visualizza il messaggio di errore
    Private Sub VisualizzaErrorMessageBox(ByVal contenuto As String)
        Me.contenutoMessaggio.Text = contenuto
        Dim sb As New StringBuilder
        sb.AppendLine("<script>")
        sb.AppendLine("hideErrorPanel=false;")
        sb.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(sb, False)
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

    'OTTENGO IL TASK ATTIVO (DA ESEGUIRE) ASSOCIATO ALL'ISTANZA SPECIFICATA DAL PARAMETRO IDISTANZA
    Private Function GetTaskAttivo(ByVal idIstanza As Integer) As ParsecWKF.TaskAttivo
        Dim statoDaEseguire As Integer = 5

        Dim istanze As New ParsecWKF.IstanzaRepository()
        Dim tasks As New ParsecWKF.TaskRepository(istanze.Context)
        Dim taskAttivo As ParsecWKF.TaskAttivo = Nothing

        Dim res = From istanza In istanze.GetQuery.Select(Function(c) New With {.Id = c.Id, .FileIter = c.FileIter, .IdModulo = c.IdModulo, .Cancellato = c.Cancellato})
                  Join task In tasks.GetQuery.Select(Function(c) New With {.Id = c.Id, .IdIstanza = c.IdIstanza, .IdStato = c.IdStato, .Corrente = c.Corrente, .Successivo = c.Successivo})
                  On istanza.Id Equals task.IdIstanza
                  Where task.IdIstanza = idIstanza And task.IdStato = statoDaEseguire And istanza.IdModulo = ParsecAdmin.TipoModulo.PRO And istanza.Cancellato = False
                  Select New ParsecWKF.TaskAttivo With {
                      .Id = task.Id,
                      .IdIstanza = task.IdIstanza,
                      .NomeFileIter = istanza.FileIter,
                      .TaskCorrente = task.Corrente,
                      .TaskSuccessivo = task.Successivo
                      }

        taskAttivo = res.FirstOrDefault
        tasks.Dispose()

        Return taskAttivo

    End Function

    'INSERISCO IL TASK AUTOMATICO
    Private Sub Procedi(ByVal taskAttivo As ParsecWKF.Task, ByVal istanzaAttiva As ParsecWKF.Istanza, ByVal idUtente As Integer, ByVal idDestinatario As Integer)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim statoEseguito As Integer = 6
        Dim statoDaEseguire As Integer = 5
        Dim statoIstanzaCompletato As Integer = 3

        Dim idIstanza As Integer = istanzaAttiva.Id
        Dim nomeFileIter As String = istanzaAttiva.FileIter

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


        taskAttivo.IdStato = statoEseguito
        taskAttivo.DataEsecuzione = Now
        taskAttivo.Operazione = operazione
        taskAttivo.Destinatario = idUtente
        taskAttivo.Notificato = True
        tasks.SaveChanges()
        '*************************************************************************************************************

        '*************************************************************************************************************
        'INSERISCO IL NUOVO TASK
        '*************************************************************************************************************
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

        nuovotask.DataFine = Now.AddDays(durata)
        nuovotask.Cancellato = False
        nuovotask.Notificato = False
        nuovotask.IdUtenteOperazione = utenteCollegato.Id

        tasks.Add(nuovotask)
        tasks.SaveChanges()
        '*************************************************************************************************************

        tasks.Dispose()
    End Sub

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

            Dim descrizioneRegistrazione As String = String.Empty
            descrizioneRegistrazione = String.Format("Prot. n. {0}/{1} - Oggetto : {2}", protocollo.NumeroProtocollo.ToString.PadLeft(7, "0"), protocollo.DataImmissione.Value.Year.ToString, protocollo.Oggetto)


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
                istanza.IdModulo = 2 'modello.RiferimentoModulo
                istanza.IdUtente = utenteCollegato.Id
                istanza.FileIter = modello.NomeFile

                Try
                    istanze.Save(istanza)
                    idIstanza = istanze.Istanza.Id
                    istanze.Dispose()

                    '*******************************************************************************************************************************
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
                    '*******************************************************************************************************************************

                    ' inserisco il task dell'istanza appena inserita
                    Dim tasks As New ParsecWKF.TaskRepository
                    Dim task As New ParsecWKF.Task
                    task.IdIstanza = idIstanza
                    task.Nome = ""
                    task.Corrente = modello.StatoIniziale
                    task.Successivo = modello.StatoSuccessivo(modello.StatoIniziale)



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

    'Aggiunge un Utente o un Gruppo alla griglia relativa alla Visibilità
    Private Sub AggiungiGruppoUtenteVisibilita(ByVal registrazione As ParsecPro.Registrazione, ByVal gruppoUtente As ParsecAdmin.VisibilitaDocumento)
        Dim esiste As Boolean = Not registrazione.Visibilita.Where(Function(c) c.IdEntita = gruppoUtente.IdEntita And c.TipoEntita = gruppoUtente.TipoEntita).FirstOrDefault Is Nothing
        If Not esiste Then
            registrazione.Visibilita.Add(gruppoUtente)
        End If
    End Sub

    'Aggiunge un Utente alla lista relativa alla visibilità (richiamato da AggiornaVisibilitaDaRegistrazione)
    Private Sub AggiungiUtenteVisibilita(ByVal registrazione As ParsecPro.Registrazione, ByVal utente As ParsecAdmin.Utente)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
        utenteVisibilita.AbilitaCancellaEntita = True
        utenteVisibilita.Descrizione = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
        utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
        utenteVisibilita.IdEntita = utente.Id
        utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.PRO
        utenteVisibilita.LogIdUtente = utenteCollegato.Id
        utenteVisibilita.LogDataOperazione = Now
        Me.AggiungiGruppoUtenteVisibilita(registrazione, utenteVisibilita)
    End Sub

    'Aggiunge il gruppo di default alla lista relativa alla visibilità 
    Private Function AggiungiGruppoDefault(ByVal idStruttura As Integer, ByVal idUtente As Integer) As ParsecAdmin.VisibilitaDocumento
        Dim gruppoDefaut As ParsecAdmin.VisibilitaDocumento = Nothing
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
            gruppoDefaut = New ParsecAdmin.VisibilitaDocumento
            gruppoDefaut.AbilitaCancellaEntita = False
            gruppoDefaut.Descrizione = gruppo.Descrizione
            gruppoDefaut.TipoEntita = 1
            gruppoDefaut.IdEntita = gruppo.Id
            gruppoDefaut.IdModulo = 2
            gruppoDefaut.LogIdUtente = idUtente
            gruppoDefaut.LogDataOperazione = Now

        End If

        Return gruppoDefaut
    End Function

    'Aggiunge il gruppo di default alla lista relativa alla visibilità 
    Private Sub AggiungiGruppoDefault(ByVal registrazione As ParsecPro.Registrazione, ByVal idStruttura As Integer)
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
            Me.AggiungiGruppoUtenteVisibilita(registrazione, gruppoDefaut)
        End If
    End Sub

    'Aggiorna la lista della visibilità (richiamato da Protocollafattura)
    Private Sub AggiornaVisibilitaDaRegistrazione(ByVal registrazione As ParsecPro.Registrazione)

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
                            Me.AggiungiUtenteVisibilita(registrazione, utente)
                        End If
                    End If
                Else
                    Me.AggiungiGruppoDefault(registrazione, referente.Id)
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
                            Me.AggiungiUtenteVisibilita(registrazione, utente)
                        End If
                    End If
                Else
                    Me.AggiungiGruppoDefault(registrazione, referente.Id)
                End If
                '**************************************************************************************************
            End If
        Next

    End Sub

    'Costruisce la Registrazione di Protocollo a partire dalla Fattura Elettronica passata come parametro 
    Private Function GetRegistrazioneDaFattura(ByVal fattura As ParsecPro.FatturaElettronica) As ParsecPro.Registrazione
        Dim registrazione As ParsecPro.Registrazione = Nothing

        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim email = emails.Where(Function(c) c.Id = fattura.MessaggioSdI.IdEmail).FirstOrDefault
        emails.Dispose()

        If Not email Is Nothing Then

            Dim denominazioneFornitore As String = fattura.DenominazioneFornitore

            Dim indirizzoFornitore As String = String.Empty
            Dim comuneFornitore As String = fattura.ComuneFornitore

            Dim capFornitore As String = fattura.CapFornitore
            Dim emailFornitore As String = fattura.EmailFornitore
            Dim provinciaFornitore As String = fattura.ProvinciaFornitore
            Dim numeroCivicoFornitore As String = fattura.NumeroCivicoFornitore
            Dim nomeFornitore As String = fattura.NomeFornitore
            Dim cognomeFornitore As String = fattura.CognomeFornitore

            registrazione = New ParsecPro.Registrazione
            registrazione.Oggetto = fattura.Oggetto

            '*******************************************************************************************************************
            'RICERCO IL MITTENTE
            '*******************************************************************************************************************
            Dim mittente As ParsecPro.Mittente = Nothing
            Dim codicePartitaIva = fattura.PartitaIvaFornitore

            'CERCO L'ULTIMO
            Dim rubrica As New RubricaRepository
            Dim strutturaEsterna As StrutturaEsternaInfo = rubrica.GetQuery.Where(Function(c) c.CodiceFiscale = codicePartitaIva And c.LogStato Is Nothing).OrderByDescending(Function(c) c.Id).FirstOrDefault
            rubrica.Dispose()

            
            Dim mittenteTrovato As Boolean = False
            If Not strutturaEsterna Is Nothing Then
                If (strutturaEsterna.Denominazione = denominazioneFornitore OrElse strutturaEsterna.Denominazione = cognomeFornitore) AndAlso strutturaEsterna.Nome = nomeFornitore AndAlso strutturaEsterna.Comune = comuneFornitore AndAlso strutturaEsterna.CAP = capFornitore Then
                    mittente = New Mittente(strutturaEsterna.Id, False)
                    mittenteTrovato = True
                End If
            End If
            If Not mittenteTrovato Then
                mittente = New Mittente
                mittente.Rubrica = False
                mittente.Indirizzo = indirizzoFornitore & If(Not String.IsNullOrEmpty(numeroCivicoFornitore), ", " & numeroCivicoFornitore, "")
                mittente.Cap = capFornitore
                mittente.Provincia = provinciaFornitore
                mittente.Citta = comuneFornitore
                mittente.Email = emailFornitore
                mittente.CodiceFiscalePartitaIva = codicePartitaIva
            End If

            If String.IsNullOrEmpty(denominazioneFornitore) Then
                mittente.Cognome = cognomeFornitore
                mittente.Nome = nomeFornitore
                mittente.Descrizione = cognomeFornitore & " " & nomeFornitore
            Else
                mittente.Cognome = denominazioneFornitore
                mittente.Descrizione = denominazioneFornitore
            End If


            registrazione.Mittenti.Add(mittente)

            '*******************************************************************************************************************

            If Not String.IsNullOrEmpty(Me.IdDestinatarioTextBox.Text) Then
                Dim dest = New ParsecPro.Destinatario(CInt(Me.IdDestinatarioTextBox.Text), True)
                dest.Iter = True
                registrazione.Destinatari.Add(dest)
            Else

                '*******************************************************************************************************************
                'RICERCO IL DESTINATARIO PER CIG
                '*******************************************************************************************************************
                Dim codiceIpaDestinatario = fattura.CodiceIpaDestinatario
                Dim destinatarioTrovato As Boolean = False

                If Not String.IsNullOrEmpty(fattura.ElencoCig) Then
                    Dim elencoCig = fattura.ElencoCig.Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries).ToList
                    If elencoCig.Count = 1 Then
                        Dim cig = elencoCig.FirstOrDefault

                        Dim impegni As New ParsecAtt.ImpegnoSpesaRepository
                        Dim atti As New ParsecAtt.DocumentoRepository(impegni.Context)

                        Dim impegniConCig = (From impegno In impegni.GetQuery
                                   Join atto In atti.GetQuery
                                   On impegno.IdDocumento Equals atto.Id
                                   Where atto.LogStato Is Nothing And impegno.CIG = cig And atto.IdTipologiaDocumento = 2
                                   Select impegno).ToList

                        impegni.Dispose()

                        If impegniConCig.Count = 1 Then

                            Dim documenti As New ParsecAtt.DocumentoRepository
                            Dim idDocumento = impegniConCig.FirstOrDefault.IdDocumento
                            Dim documento = documenti.Where(Function(c) c.Id = idDocumento).FirstOrDefault
                            documenti.Dispose()

                            If Not documento Is Nothing Then
                                Dim strutture As New ParsecAtt.StrutturaViewRepository
                                Dim struttura = strutture.Where(Function(c) c.Id = documento.IdStruttura And (c.LogStato Is Nothing Or c.LogStato = "M")).FirstOrDefault
                                If Not struttura Is Nothing Then
                                    If struttura.LogStato = "M" Then
                                        struttura = strutture.Where(Function(c) c.Codice = struttura.Codice And c.LogStato Is Nothing).FirstOrDefault
                                        If Not struttura Is Nothing Then
                                            Dim dest = New ParsecPro.Destinatario(struttura.Id, True)
                                            dest.Iter = True
                                            registrazione.Destinatari.Add(dest)
                                            destinatarioTrovato = True
                                        End If
                                    Else
                                        Dim dest = New ParsecPro.Destinatario(struttura.Id, True)
                                        dest.Iter = True
                                        registrazione.Destinatari.Add(dest)
                                        destinatarioTrovato = True
                                    End If
                                End If
                                strutture.Dispose()
                            End If
                        End If
                    End If
                End If
                '*******************************************************************************************************************

                '*******************************************************************************************************************
                'RICERCO IL DESTINATARIO PER CODICDE IPA
                '*******************************************************************************************************************
                If Not destinatarioTrovato Then
                    Dim referenti As New ParsecAdmin.StructureRepository
                    Dim referenteInterno = referenti.Where(Function(c) c.CodiceIPA = codiceIpaDestinatario And c.LogStato Is Nothing).FirstOrDefault
                    referenti.Dispose()

                    If Not referenteInterno Is Nothing Then
                        Dim dest = New ParsecPro.Destinatario(referenteInterno.Id, True)
                        dest.Iter = True
                        registrazione.Destinatari.Add(dest)
                    End If
                End If
                '*******************************************************************************************************************

            End If


            Dim allegatoEmail = Me.GetAllegatoEmail(email)
            registrazione.Allegati.Add(allegatoEmail)
            registrazione.TipologiaRegistrazione = TipoRegistrazione.Arrivo
            registrazione.TipoRegistrazione = 0  'ARRIVO
        End If
        Return registrazione
    End Function
   
    'Costruisce la registrazione di Protocollo a partire dal'XML della Fattura Elettronica.
    Private Function GetRegistrazioneDaFatturaXML(ByVal fattura As ParsecPro.FatturaElettronica) As ParsecPro.Registrazione

        Dim registrazione As ParsecPro.Registrazione = Nothing

        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim email = emails.Where(Function(c) c.Id = fattura.MessaggioSdI.IdEmail).FirstOrDefault

        If Not email Is Nothing Then

            Dim message As New Rebex.Mail.MailMessage
            message.Settings.IgnoreInvalidTnefMessages = True

            Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
            Dim fullPathEmail As String = mailBoxPath & email.PercorsoRelativo & email.NomeFileEml

            message.Load(fullPathEmail)

            Dim ms As IO.MemoryStream = Nothing
            Dim innerMessage As Rebex.Mail.MailMessage = Nothing
            Dim IdentificativoSdI As String = String.Empty
            Dim nomeFileFattura As String = String.Empty
            Dim formato As String = String.Empty
            Dim fatturaElement As XElement = Nothing
            Dim element As XElement = Nothing
            Dim fatturaTrovata As Boolean = False
            Dim metaDatitrovati As Boolean = False

            For Each att In message.Attachments
                If att.FileName.ToLower.EndsWith(".eml") Then
                    innerMessage = New Rebex.Mail.MailMessage
                    innerMessage.Settings.IgnoreInvalidTnefMessages = True

                    ms = New IO.MemoryStream
                    att.Save(ms)
                    ms.Position = 0
                    innerMessage.Load(ms)

                    For Each s In innerMessage.Attachments
                        '******************************************************************************************************
                        'SE L'ALLEGATO DELL'EMAIL E' UN FILE XML O UN FILE P7M
                        '******************************************************************************************************
                        If s.FileName.ToLower.EndsWith(".xml") OrElse s.FileName.ToLower.EndsWith(".p7m") Then
                            ms = New IO.MemoryStream
                            s.Save(ms)
                            ms.Position = 0

                            If s.FileName.ToLower.EndsWith(".p7m") Then
                                Dim buffer As Byte() = ms.ToArray
                                Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms

                                'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
                                Try
                                    buffer = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(buffer))
                                Catch ex As Exception
                                    'NIENTE
                                End Try

                                signedCms.Decode(buffer)

                                ms = ParsecUtility.Utility.FixVersioneXml(signedCms.ContentInfo.Content)

                            Else
                                ms = ParsecUtility.Utility.FixVersioneXml(ms.ToArray)

                            End If

                            element = XElement.Load(ms)



                            If Not element Is Nothing Then
                                Dim header = element.Element("FatturaElettronicaHeader")

                                Dim identificativoSdIElement = element.Element("IdentificativoSdI")
                                Dim formatoElement = element.Element("Formato")
                                '******************************************************************************************************
                                'SE E' UNA FATTURA ELETTRONICA
                                '******************************************************************************************************
                                If Not header Is Nothing Then
                                    fatturaTrovata = True
                                    fatturaElement = element
                                End If

                                If Not identificativoSdIElement Is Nothing Then
                                    metaDatitrovati = True
                                    IdentificativoSdI = identificativoSdIElement.Value
                                    formato = formatoElement.Value
                                End If

                            End If
                        End If

                        If fatturaTrovata And metaDatitrovati Then
                            Exit For
                        End If
                    Next
                End If
                If fatturaTrovata And metaDatitrovati Then
                    Exit For
                End If
            Next

            If fatturaTrovata And metaDatitrovati Then

                registrazione = Me.LeggiFattura(fatturaElement)
                Dim allegatoEmail = Me.GetAllegatoEmail(email)
                registrazione.Allegati.Add(allegatoEmail)
                registrazione.TipologiaRegistrazione = TipoRegistrazione.Arrivo
                registrazione.TipoRegistrazione = 0  'ARRIVO

            End If

        End If

        Return registrazione

    End Function

    'Protocolla la Fattura Elettronica
    Private Sub ProtocollaFattura(ByVal fattura As ParsecPro.FatturaElettronica, ByVal utenteCorrente As ParsecAdmin.Utente)

        If fattura.NumeroProtocollo.HasValue Then
            Throw New ApplicationException("Già protocollata!")
        End If

        Dim registrazione As ParsecPro.Registrazione = Nothing

        If String.IsNullOrEmpty(fattura.IndirizzoFornitore) Then
            registrazione = Me.GetRegistrazioneDaFatturaXML(fattura)
        Else
            registrazione = Me.GetRegistrazioneDaFattura(fattura)
        End If


        If Not registrazione Is Nothing Then

            Dim dataOdierna = Now
            registrazione.DataOraRegistrazione = dataOdierna
            registrazione.IdUtente = utenteCorrente.Id
            registrazione.UtenteUsername = utenteCorrente.Username
            registrazione.DataImmissione = dataOdierna
            registrazione.Riservato = False
            registrazione.Modificato = False
            registrazione.Annullato = False
            registrazione.PresentiModifiche = False
            registrazione.DataOraAnnullamento = Nothing
            registrazione.IdUtenteAnnullamento = Nothing
            registrazione.UtenteUsernameAnnullamento = Nothing
            registrazione.DataOraRicezioneInvio = fattura.MessaggioSdI.DataRicezioneInvio
            registrazione.IdTipoDocumento = Nothing
            registrazione.IdTipoRicezione = Nothing
            registrazione.ProtocolloMittente = String.Empty

            registrazione.NumeroAllegati = registrazione.Allegati.Count
            registrazione.IdClassificazione = Nothing
            registrazione.NumeroProtocolloRiscontro = Nothing
            registrazione.DataImmissioneRiscontro = Nothing
            registrazione.IdSessioneEmergenza = Nothing
            registrazione.NumeroEmergenza = Nothing
            registrazione.IdUtenteEmergenza = Nothing
            registrazione.UtenteEmergenzaUsername = Nothing
            registrazione.Note = String.Empty
            registrazione.NoteInterne = String.Empty
            registrazione.Spid = Nothing
            registrazione.AnticipatoViaFax = False
            registrazione.TipologiaAllegatoPrimario = ParsecPro.TipologiaAllegatoPrimario.Fattura

            registrazione.DataDocumento = dataOdierna
            registrazione.DataProtocollo = dataOdierna


            Dim tipiRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository
            Dim tipoInvio = tipiRicezioneInvio.Where(Function(c) c.Descrizione.ToLower = "pec" And c.LogStato = Nothing).FirstOrDefault
            If Not tipoInvio Is Nothing Then
                registrazione.IdTipoRicezione = tipoInvio.Id
            End If
            Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
            Dim tipoDocumento = tipiDocumento.Where(Function(c) c.Descrizione.ToLower = "elettronico").FirstOrDefault
            If Not tipoDocumento Is Nothing Then
                registrazione.IdTipoDocumento = tipoDocumento.Id
            End If
            tipiDocumento.Dispose()


            '***********************************************************************************************************************************
            'AGGIUNGO VISIBILTA' UTENTE CORRENTE
            '***********************************************************************************************************************************
            Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
            utenteVisibilita.IdEntita = utenteCorrente.Id
            utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
            utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.PRO
            utenteVisibilita.Descrizione = (If(utenteCorrente.Username = Nothing, "", utenteCorrente.Username) + " - " + If(utenteCorrente.Cognome = Nothing, "", utenteCorrente.Cognome) + " " + If(utenteCorrente.Nome = Nothing, "", utenteCorrente.Nome))
            utenteVisibilita.LogIdUtente = utenteCorrente.Id
            utenteVisibilita.LogDataOperazione = Now
            utenteVisibilita.AbilitaCancellaEntita = False
            Me.AggiungiGruppoUtenteVisibilita(registrazione, utenteVisibilita)
            '***********************************************************************************************************************************

            '***********************************************************************************************************************************
            'CARICO LA VISIBILITA
            '***********************************************************************************************************************************
            If utenteCorrente.CodiceStutturaDefault.HasValue Then
                Dim strutture As New ParsecAdmin.StructureRepository
                Dim struttura = strutture.GetQuery.Where(Function(c) c.Codice = utenteCorrente.CodiceStutturaDefault And c.LogStato Is Nothing).FirstOrDefault
                If Not struttura Is Nothing Then
                    Dim gruppoDefault = Me.AggiungiGruppoDefault(struttura.Id, utenteCorrente.Id)
                    If Not gruppoDefault Is Nothing Then
                        Me.AggiungiGruppoUtenteVisibilita(registrazione, gruppoDefault)
                    End If

                End If
            End If
            '***********************************************************************************************************************************

            Me.AggiornaVisibilitaDaRegistrazione(registrazione)


            registrazione.IdEmail = fattura.MessaggioSdI.IdEmail
            registrazione.IdFatturaElettronica = fattura.Id

            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            registrazioni.Save(registrazione)
            registrazioni.Dispose()

            '***********************************************************************************************************************************
            'AVVIO L'ITER
            '***********************************************************************************************************************************
            Dim nomeParametro As String = "IdModelloWorkflowFatturaElettronica"
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName(nomeParametro, ParsecAdmin.TipoModulo.PRO)
            parametri.Dispose()
            If Not parametro Is Nothing Then
                Dim modelli As New ParsecWKF.ModelliRepository
                Dim modello = modelli.Where(Function(c) c.Id = CInt(parametro.Valore)).FirstOrDefault
                modelli.Dispose()
                If Not modello Is Nothing Then
                    Me.CreaIstanzaPro(registrazione, modello)
                End If
            End If
            '***********************************************************************************************************************************

        End If
    End Sub


#End Region

#Region "EVENTI CONTROLLO VISUALIZZA FATTURA "

    'Chiude ilpannello di visualizzazione della Fattura Elettronica
    Protected Sub VisualizzaFatturaControl_OnCloseEvent() Handles VisualizzaFatturaControl.OnCloseEvent
        Me.VisualizzaFatturaControl.Visible = False
    End Sub

#End Region

End Class
