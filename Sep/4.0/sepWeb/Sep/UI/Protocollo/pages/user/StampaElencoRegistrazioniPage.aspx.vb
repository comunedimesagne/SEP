Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StampaElencoRegistrazioniPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "EVENTI PAGINA"

    'Evento PreInit associato alla Pagina. Impostazione modo apertura MasterPage
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BlankPage.master"
        End If
    End Sub

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.EsportaButton.Attributes.Add("onclick", "this.disabled=true;")

        Dim tipoRegistrazionePredefinita As Integer = CInt(Request.QueryString("Tipo"))

        If Me.Request.QueryString("Mode") Is Nothing Then
            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Protocollo"

            Select Case tipoRegistrazionePredefinita
                Case 0 'Stampa Elenco Registrazioni
                    Me.MainPage.DescrizioneProcedura = "> Stampa Elenco Registrazioni"
                Case 1 'Stampa Registro Protocollo Generale
                    Me.MainPage.DescrizioneProcedura = "> Stampa Registro Generale"

            End Select

        Else
            Select Case tipoRegistrazionePredefinita
                Case 0 'Stampa Elenco Registrazioni
                    Me.TitoloLabel.Text = "Stampa Elenco Registrazioni"

                Case 1 'Stampa Registro Protocollo Generale
                    Me.TitoloLabel.Text = "Stampa Registro Generale"
            End Select

        End If

        Me.ResettaFiltro()

        Dim tipiRicezione As New ParsecPro.TipiRicezioneInvioRepository
        Me.TipoRicezioneInvioComboBox.DataSource = tipiRicezione.GetView(Nothing)
        Me.TipoRicezioneInvioComboBox.DataTextField = "Descrizione"
        Me.TipoRicezioneInvioComboBox.DataValueField = "Id"
        Me.TipoRicezioneInvioComboBox.DataBind()
        Me.TipoRicezioneInvioComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.TipoRicezioneInvioComboBox.SelectedIndex = 0
        tipiRicezione.Dispose()

        Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
        Me.TipologiaDocumentoComboBox.DataSource = tipiDocumento.GetView(Nothing)
        Me.TipologiaDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologiaDocumentoComboBox.DataValueField = "Id"
        Me.TipologiaDocumentoComboBox.DataBind()
        Me.TipologiaDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        tipiDocumento.Dispose()

        Me.FiltroRegistrazioniAnnullateComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("Escludi annullate", "0"))
        Me.FiltroRegistrazioniAnnullateComboBox.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem("Visualizza solo annullate", "1"))
        Me.FiltroRegistrazioniAnnullateComboBox.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem("Includi annullate", "2"))


        Dim script As New StringBuilder
        script.AppendLine("var value =  $find('" & Me.NumeroProtocolloInizioTextBox.ClientID & "').get_displayValue(); var textbox =  $find('" & Me.NumeroProtocolloFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")
        Me.NumeroProtocolloInizioTextBox.Attributes.Add("onblur", script.ToString)

    End Sub


#End Region

#Region "METODI PRIVATI"

    'Metodo che costruisce il filtro per la Ricerca e lo restituisce
    Private Function GetFiltroRegistrazioni() As ParsecPro.RegistrazioneFiltro

        Dim idUtenteInserimento As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Me.IdUtenteInserimentoTextBox.Text) Then
            idUtenteInserimento = Convert.ToInt32(Me.IdUtenteInserimentoTextBox.Text)
        End If

        'FILTRO PER STRUTTURA UTILIZZANDO IL CAMPO DESCRIZIONE PER POTER TROVARE ANCHE I PROTOCOLLI CON STRUTTURE MODIFICATE
        Dim idUfficio As Nullable(Of Int32) = Nothing
        Dim ufficio As String = String.Empty
        ufficio = Me.UfficioTextBox.Text.Trim

        Dim idClassificazione As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            idClassificazione = Convert.ToInt32(Me.IdClassificazioneTextBox.Text)
        End If

        Dim numeroRiscontro As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Trim(Me.NumeroRiscontroTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroRiscontroTextBox.Text) Then
                numeroRiscontro = CInt(Me.NumeroRiscontroTextBox.Text)
            End If
        End If

        Dim annoRiscontro As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Trim(Me.AnnoRiscontroTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.AnnoRiscontroTextBox.Text) Then
                annoRiscontro = CInt(Me.AnnoRiscontroTextBox.Text)
            End If
        End If


        Dim IdTipoDocumento As Nullable(Of Int32) = Nothing
        If Me.TipologiaDocumentoComboBox.SelectedIndex <> 0 Then
            IdTipoDocumento = Me.TipologiaDocumentoComboBox.SelectedValue
        End If

        Dim IdTipoRicezioneInvio As Nullable(Of Int32) = Nothing
        If Me.TipoRicezioneInvioComboBox.SelectedIndex <> 0 Then
            IdTipoRicezioneInvio = Me.TipoRicezioneInvioComboBox.SelectedValue
        End If

        Dim registrazioni As New ParsecPro.RegistrazioniRepository

        Dim annullata As Nullable(Of Boolean) = Nothing
        If Me.FiltroRegistrazioniAnnullateComboBox.SelectedIndex <> 2 Then
            annullata = CBool(Me.FiltroRegistrazioniAnnullateComboBox.SelectedIndex)
        End If

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim numeroProtocolloInizio As Nullable(Of Int32)
        Dim numeroProtocolloFine As Nullable(Of Int32)

        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloInizioTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloInizioTextBox.Text) Then
                numeroProtocolloInizio = CInt(Me.NumeroProtocolloInizioTextBox.Text)
            End If
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloFineTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloFineTextBox.Text) Then
                numeroProtocolloFine = CInt(Me.NumeroProtocolloFineTextBox.Text)
            End If
        End If

        Dim filtro As New ParsecPro.RegistrazioneFiltro With
            {
                .Arrivo = Me.RegistrazioneArrivoCheckBox.Checked,
                .Partenza = Me.RegistrazionePartenzaCheckBox.Checked,
                .Interna = Me.RegistrazioneInternaCheckBox.Checked,
                .DataProtocolloInizio = Me.DataProtocolloInizioTextBox.SelectedDate,
                .DataProtocolloFine = Me.DataProtocolloFineTextBox.SelectedDate,
                .Annullata = annullata,
                .IdUtenteCollegato = utenteCorrente.Id,
                .NumeroProtocolloInizio = numeroProtocolloInizio,
                .NumeroProtocolloFine = numeroProtocolloFine,
                .ReferenteEsternoDenominazione = Me.DenominazioneCognomeTextBox.Text,
                .ReferenteEsternoNome = Me.NomeTextBox.Text,
                .ReferenteEsternoCitta = Me.CittaTextBox.Text,
                .ReferenteEsternoEmail = Me.EmailTextBox.Text,
                .IdUtenteInserimento = idUtenteInserimento,
                .IdStruttura = idUfficio,
                .Struttura = ufficio,
                .StrutturaCompleta = Me.IncludiStruttureDipendentiCheckBox.Checked,
                .IdClassificazione = idClassificazione,
                .ClassificazioneCompleta = Me.IncludiClassificheDipendentiCheckBox.Checked,
                .Oggetto = Me.FiltroOggettoTextBox.Text,
                .ProtocolloMittente = Me.ProtocolloMittenteTextBox.Text,
                .Note = Me.NoteTextBox.Text,
                .NoteInterne = Me.NoteInterneTextBox.Text,
                .IdTipoDocumento = IdTipoDocumento,
                .IdTipoRicezioneInvio = IdTipoRicezioneInvio,
                .DataDocumentoInizio = Me.DataDocumentoInizioTextBox.SelectedDate,
                .DataDocumentoFine = Me.DataDocumentoFineTextBox.SelectedDate,
                .DataRicezioneInvioInizio = Me.DataRicezioneInvioInizioTextBox.SelectedDate,
                .DataRicezioneInvioFine = Me.DataRicezioneInvioFineTextBox.SelectedDate,
                .Stampa = True
             }

        registrazioni.Dispose()
        Return filtro

    End Function

    'Metodo Print: effettura la ricerca delle registrazioni e genera il pdf
    Private Sub Print()

        Dim found As Boolean = False

        Dim protocolli As New ParsecPro.RegistrazioniRepository

        Dim registrazioni = protocolli.GetViewStampa(Me.GetFiltroRegistrazioni)

        For Each p In registrazioni
            found = True
            Exit For
        Next

        If Not found Then
            ParsecUtility.Utility.MessageBox("Nessuna registrazione trovata con i criteri di filtro impostati!", False)
            Me.EsportaButton.Enabled = True
            Exit Sub
        End If

        Me.GeneraReportPdf(registrazioni)

    End Sub

    'Ritorna le informazioni associate all'Ente configurato
    Private Function GetCliente() As ParsecAdmin.Cliente
        Dim clientRepository As New ParsecAdmin.ClientRepository
        Dim cliente As ParsecAdmin.Cliente = clientRepository.GetQuery.FirstOrDefault
        clientRepository.Dispose()
        Return cliente
    End Function

    'Resetta i campi della pagina e quindi i campi del filtro per la ricerca
    Private Sub ResettaFiltro()
        Me.RegistrazioneArrivoCheckBox.Checked = True
        Me.RegistrazionePartenzaCheckBox.Checked = True
        Me.RegistrazioneInternaCheckBox.Checked = True
        Me.FiltroRegistrazioniAnnullateComboBox.SelectedIndex = 0
        Me.NumeroProtocolloInizioTextBox.Text = String.Empty
        Me.NumeroProtocolloFineTextBox.Text = String.Empty

        Dim tipoRegistrazionePredefinita As Integer = CInt(Request.QueryString("Tipo"))
        Select Case tipoRegistrazionePredefinita
            Case 0 'Stampa Elenco Registrazioni
                Me.DataProtocolloInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
                Me.DataProtocolloFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
            Case 1 'Stampa Registro Protocollo Generale
                Me.DataProtocolloInizioTextBox.SelectedDate = Now.AddDays(-1)
                Me.DataProtocolloFineTextBox.SelectedDate = Now
        End Select

        Me.DenominazioneCognomeTextBox.Text = String.Empty
        Me.NomeTextBox.Text = String.Empty
        Me.CittaTextBox.Text = String.Empty
        Me.EmailTextBox.Text = String.Empty
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
        Me.FiltroOggettoTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.ProtocolloMittenteTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty
        Me.NoteInterneTextBox.Text = String.Empty
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        Me.TipoRicezioneInvioComboBox.SelectedIndex = 0
        Me.DataDocumentoInizioTextBox.SelectedDate = Nothing
        Me.DataDocumentoFineTextBox.SelectedDate = Nothing
        Me.DataRicezioneInvioInizioTextBox.SelectedDate = Nothing
        Me.DataRicezioneInvioFineTextBox.SelectedDate = Nothing
        Me.UtenteInserimentoTextBox.Text = String.Empty
        Me.IdUtenteInserimentoTextBox.Text = String.Empty
    End Sub

    'Convalida i parametri oppure no per i pulsanti Cerca ed Esporta
    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean
        If Not Me.RegistrazioneArrivoCheckBox.Checked AndAlso Not Me.RegistrazionePartenzaCheckBox.Checked AndAlso Not Me.RegistrazioneInternaCheckBox.Checked Then
            message.AppendLine("E' necessario selezionare almeno un tipo di registrazione!")
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloInizioTextBox.Text)) Then
            If ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloInizioTextBox.Text) Then
                message.AppendLine("Se specificato, il campo 'Numero protocollo da' deve essere un numero!")
            End If
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloFineTextBox.Text)) Then
            If ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloFineTextBox.Text) Then
                message.AppendLine("Se specificato, il campo 'Numero protocollo a' deve essere un numero!")
            End If
        End If

        Return Not message.Length > 0
    End Function

#End Region

#Region "EVENTI CONTROLLI PAGINA"

    'fa partire la ricerca in base ai filtri impostati
    Protected Sub CercaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CercaButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Me.Print()
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    'Resetta i campi della maschera
    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

#End Region

#Region "GESTIONE RICERCA UFFICIO"

    'Fa partire la ricerca delle Strutture tramite la Pagina RicercaOrganigrammaPage.aspx
    Protected Sub TrovaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaStrutturaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaStrutturaImageButton.ClientID)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("ultimoLivelloStruttura", "400")
        parametriPagina.Add("livelliSelezionabili", "100,200,300,400")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)

    End Sub

    'Aggiorna le informazioni della Struttura selezionata nell'evento TrovaStrutturaImageButton_Click
    Protected Sub AggiornaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaStrutturaImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Me.UfficioTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdUfficioTextBox.Text = struttureSelezionate.First.Id
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    'Evento che svuota i campi UfficioTextBox e IdUfficioTextBox
    Protected Sub EliminaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaStrutturaImageButton.Click
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
    End Sub

#End Region

#Region "GESTIONE RICERCA CLASSIFICAZIONE"

    'Fa partire la ricerca della Classificazione tramite la Pagina RicercaClassificazionePage.aspx
    Protected Sub TrovaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    End Sub

    'Aggiorna le informazioni della Classificazione selezionata nell'evento TrovaClassificazioneImageButton_Click
    Protected Sub AggiornaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim classificazioneCompleta As String = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione2(idClassificazione, 1) & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneTextBox.Text = idClassificazione.ToString
            Session("ClassificazioniSelezionate") = Nothing
        End If
    End Sub

    'Svuota i campi ClassificazioneTextBox e IdClassificazioneTextBox
    Protected Sub EliminaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaClassificazioneImageButton.Click
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
    End Sub

#End Region

#Region "GESTIONE RICERCA UTENTE"

    'Fa partire la ricerca degli Utenti tramite la Pagina RicercaUtentePage.aspx
    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 0 'singola
        Session("Parametri") = ht
    End Sub

    'Aggiorna le informazioni dell'utente selezionata nell'evento TrovaUtenteImageButton_Click
    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Me.UtenteInserimentoTextBox.Text = utentiSelezionati.First.Value
            Me.IdUtenteInserimentoTextBox.Text = utentiSelezionati.First.Key
            Session("SelectedUsers") = Nothing
        End If
    End Sub

    'Svuota i campi UtenteInserimentoTextBox e IdUtenteInserimentoTextBox
    Protected Sub EliminaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteImageButton.Click
        Me.UtenteInserimentoTextBox.Text = String.Empty
        Me.IdUtenteInserimentoTextBox.Text = String.Empty
    End Sub


#End Region

    'Fa partire la esportazione della lista delle Registrazioni in excel
    Protected Sub EsportaButton_Click(sender As Object, e As System.EventArgs) Handles EsportaButton.Click

        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Me.Esporta()
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If

    End Sub

    'Si occupa della esportazione delle Registrazioni. Prima le cerca in base ai filtri e poi le esporta.
    Private Sub Esporta()
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("Registrazioni_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As String = String.Empty
        Dim protocolli As New ParsecPro.RegistrazioniRepository

        Dim registrazioni = protocolli.GetView(Me.GetFiltroRegistrazioni)

        Dim found As Boolean = False
        line &= "N. Prot.;Data Prot.;Tipologia;Oggetto;Mittenti/Destinatari;Uffici"
        swExport.WriteLine(line)
        line = ""
        For Each p In registrazioni
            Dim m = p.ElencoReferentiEsterni
            line &= p.NumeroProtocollo.ToString.PadLeft(7, "0") & ";" & String.Format("{0:dd-MM-yyyy}", p.DataImmissione) & ";" & p.DescrizioneTipologiaRegistristrazione & ";" & p.Oggetto.ToString.Replace(vbCrLf, "") & ";" & p.ElencoReferentiEsterni & ";" & p.ElencoReferentiInterni
            swExport.WriteLine(line)
            line = ""
            found = True
        Next
        swExport.Close()

        If Not found Then
            ParsecUtility.Utility.MessageBox("Nessuna registrazione trovata con i criteri di filtro impostati!", False)
            Me.EsportaButton.Enabled = True
            Exit Sub
        End If

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

        Me.EsportaButton.Enabled = True
    End Sub

    'Genera il pdf a partire dai dati delle Registrazioni trovate
    Private Sub GeneraReportPdf(ByVal registrazioni As IEnumerable)
        Dim stream As New IO.MemoryStream
        Using document As New Document(pageSize.A4.Rotate, 18.5, 18.5, 36, 36)

            Dim canvas As PdfContentByte
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, stream)

            Dim pageEventHelper = New PageEventHelper()
            writer.PageEvent = pageEventHelper

            document.Open()

            canvas = writer.DirectContent
            Dim pageSize As Rectangle = document.PageSize

            document.Add(Me.CreateHeaderTable)

            Dim headerTable As New PdfPTable(5)

            headerTable.TotalWidth = 805
            headerTable.LockedWidth = True

            Dim widths As Single() = New Single() {130.0F, 196.0F, 196.0F, 196.0F, 87.0F}

            headerTable.SetWidths(widths)

            Dim columnNames As New List(Of String) From {"REGISTRO PROTOCOLLO", "OGGETTO", "METTENTE/DESTINATARI", "UFFICI", "PROT. MITTENTE"}

            For i As Integer = 0 To columnNames.Count - 1
                headerTable.AddCell(Me.CreateHeaderCell(columnNames(i)))
            Next

            document.Add(headerTable)

            Dim detailTable As PdfPTable
            Dim innerTable As PdfPTable
            Dim innerTable2 As PdfPTable
            Dim innerAllegatiTable As PdfPTable
            Dim detailCell As PdfPCell

            Dim evt As DottedCell = Nothing

            For Each p In registrazioni

                evt = New DottedCell(PdfPCell.BOTTOM_BORDER, False)

                detailTable = New PdfPTable(1)
                detailTable.TotalWidth = 805
                detailTable.LockedWidth = True
                detailTable.DefaultCell.Border = 0


                innerTable = New PdfPTable(5)
                innerTable.TotalWidth = 805
                innerTable.LockedWidth = True
                innerTable.DefaultCell.Border = PdfPCell.BOTTOM_BORDER
                innerTable.SetWidths(widths)


                innerTable.AddCell(Me.CreateDottedCell(p.NumeroProtocollo.ToString.PadLeft(7, "0") & " " & String.Format("{0:dd-MM-yyyy}", p.DataImmissione) & " " & p.DescrizioneTipologiaRegistristrazione, Element.ALIGN_LEFT, evt))
                innerTable.AddCell(Me.CreateDottedCell(p.Oggetto, Element.ALIGN_LEFT, evt))
                innerTable.AddCell(Me.CreateDottedCell(p.ElencoReferentiEsterni, Element.ALIGN_LEFT, evt))
                innerTable.AddCell(Me.CreateDottedCell(p.ElencoReferentiInterni, Element.ALIGN_LEFT, evt))
                innerTable.AddCell(Me.CreateDottedCell(p.regProtocolloMittente & " " & p.dataProtocolloMittente, Element.ALIGN_LEFT, evt))


                innerTable2 = New PdfPTable(6)
                innerTable2.TotalWidth = 805
                innerTable2.LockedWidth = True
                innerTable2.SetWidths(New Single() {111.0F, 113.0F, 176.0F, 127.0F, 178.0F, 100.0F})
                innerTable2.AddCell(Me.CreateDottedCell(p.DescrizioneTipologiaDocumento, Element.ALIGN_LEFT, evt))
                innerTable2.AddCell(Me.CreateDottedCell(p.DescrizioneTipoRicezioneInvio, Element.ALIGN_LEFT, evt))
                innerTable2.AddCell(Me.CreateDottedCell(p.Note, Element.ALIGN_LEFT, evt))
                innerTable2.AddCell(Me.CreateDottedCell(p.Annullata, Element.ALIGN_LEFT, evt))
                innerTable2.AddCell(Me.CreateDottedCell(p.Classificazione, Element.ALIGN_LEFT, evt))
                innerTable2.AddCell(Me.CreateDottedCell(p.UtenteUsername, Element.ALIGN_LEFT, evt))


                innerAllegatiTable = New PdfPTable(1)
                innerAllegatiTable.TotalWidth = 805
                innerAllegatiTable.LockedWidth = True

                evt = New DottedCell(PdfPCell.BOTTOM_BORDER, True)

                innerAllegatiTable.AddCell(Me.CreateDottedCell(p.listaAllegati, Element.ALIGN_LEFT, evt))

                detailCell = New PdfPCell
                detailCell.Border = 0

                detailCell.AddElement(innerTable)
                detailCell.AddElement(innerTable2)
                detailCell.AddElement(innerAllegatiTable)

                detailTable.AddCell(detailCell)

                detailTable.KeepTogether = True


                document.Add(detailTable)

            Next

            document.NewPage()

        End Using

        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaElencoRegistrazioniDiretta")
        parametriStampa.Add("FullPath", stream.GetBuffer)
        Session("ParametriStampaPro") = parametriStampa
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)

    End Sub

    'Crea il Footer della Tabella
    Private Function CreateFooterTable() As PdfPTable
        Dim footerFont As Font = FontFactory.GetFont("Arial", 7)
        Dim oggi As String = DateTime.Now.ToString("dd/MM/yyyy")

        Dim footerTable As New PdfPTable(2)
        footerTable.TotalWidth = 115 * 7
        footerTable.LockedWidth = True

        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        Dim cell As New PdfPCell(New Paragraph(utenteCollegato.Cognome & " " & utenteCollegato.Nome, footerFont))
        cell.HorizontalAlignment = Element.ALIGN_LEFT
        cell.Border = 0
        footerTable.AddCell(cell)

        cell = New PdfPCell(New Paragraph(oggi, footerFont))
        cell.HorizontalAlignment = Element.ALIGN_RIGHT
        cell.Border = 0
        footerTable.AddCell(cell)
        Return footerTable
    End Function

    'Crea l'Header della tabella nel PDF
    Private Function CreateHeaderTable() As PdfPTable

        Dim font As Font = FontFactory.GetFont("Arial", 14)

        Dim headerTable As New PdfPTable(2)
        headerTable.TotalWidth = 115 * 7
        headerTable.LockedWidth = True
        headerTable.SetWidths(New Single() {500.0F, 305.0F})
        Dim headerCell As New PdfPCell(New Paragraph("Registro Protocollo", font))

        headerCell.HorizontalAlignment = Element.ALIGN_LEFT
        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
        headerCell.FixedHeight = 30
        headerCell.Border = Rectangle.NO_BORDER
        headerTable.AddCell(headerCell)


        headerCell = New PdfPCell(New Paragraph(GetCliente.Descrizione, font))

        headerCell.HorizontalAlignment = Element.ALIGN_RIGHT
        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
        headerCell.FixedHeight = 30
        headerCell.Border = Rectangle.NO_BORDER
        headerTable.AddCell(headerCell)


        Return headerTable
    End Function

    'Crea l'Header delle celle nel PDF
    Private Function CreateHeaderCell(ByVal text As String) As PdfPCell
        Dim p As New Paragraph(text, FontFactory.GetFont("Arial", 9, Font.BOLD))
        p.Alignment = Element.ALIGN_CENTER

        Dim cell As New PdfPCell(p)
        cell.HorizontalAlignment = Element.ALIGN_CENTER
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE

        cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER
        cell.BackgroundColor = New iTextSharp.text.BaseColor(224, 224, 224)
        cell.PaddingBottom = 4
        Return cell
    End Function

    'Crea una cella
    Private Function CreateCell(ByVal text As String, ByVal align As Integer) As PdfPCell
        Dim p As New Paragraph(text, FontFactory.GetFont("Arial", 9, Font.NORMAL))
        p.Alignment = align

        Dim cell As New PdfPCell(p)
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 8

        cell.Border = 0
        Return cell
    End Function

    'Applica gli stili alle celle
    Private Function CreateDottedCell(ByVal text As String, ByVal align As Integer, ByVal cellEvent As IPdfPCellEvent) As PdfPCell
        Dim p As New Paragraph(text, FontFactory.GetFont("Arial", 9, Font.NORMAL))
        p.Alignment = align
        Dim cell As New PdfPCell(p)
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 8

        cell.Border = 0
        cell.CellEvent = cellEvent
        Return cell
    End Function

    'Classe che implementa la interfaccia IPdfPCellEvent
    Public Class DottedCell
        Implements IPdfPCellEvent

        Private border As Integer = 0
        Private solid As Boolean = False

        'Costruttore
        Public Sub New(ByVal border As Integer, ByVal solid As Boolean)
            MyBase.New()
            Me.border = border
            Me.solid = solid
        End Sub

        'Setta il layout alla cella
        Public Sub CellLayout(ByVal cell As PdfPCell, ByVal position As Rectangle, ByVal canvases As PdfContentByte()) Implements IPdfPCellEvent.CellLayout

            Dim cb As PdfContentByte = canvases(PdfPTable.LINECANVAS)
            cb.SaveState()

            If Not solid Then
                cb.SetLineDash(3.0F, 3.0F)
            End If

            If ((border And PdfPCell.TOP_BORDER) = PdfPCell.TOP_BORDER) Then
                cb.MoveTo(position.Right, position.Top)
                cb.LineTo(position.Left, position.Top)
            End If

            If ((border And PdfPCell.BOTTOM_BORDER) = PdfPCell.BOTTOM_BORDER) Then
                cb.MoveTo(position.Left, position.Bottom)
                cb.LineTo(position.Right, position.Bottom)
            End If


            If ((border And PdfPCell.RIGHT_BORDER) = PdfPCell.RIGHT_BORDER) Then
                cb.MoveTo(position.Right, position.Top)
                cb.LineTo(position.Right, position.Bottom)
            End If

            If ((border And PdfPCell.LEFT_BORDER) = PdfPCell.LEFT_BORDER) Then
                cb.MoveTo(position.Left, position.Top)
                cb.LineTo(position.Left, position.Bottom)
            End If

            cb.Stroke()
            cb.RestoreState()
        End Sub

    End Class

    'Classe che gestisce gli eventi sul PDF
    Public Class PageEventHelper
        Inherits PdfPageEventHelper

        Private cb As PdfContentByte
        Private template As PdfTemplate
        Private font As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, True)

        'Metodo OnOpenDocument
        Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
            MyBase.OnOpenDocument(writer, document)
            Me.cb = writer.DirectContent
            Me.template = Me.cb.CreateTemplate(50, 50)
        End Sub

        'Metodo onEndPAge
        Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
            MyBase.OnEndPage(writer, document)
            Dim pageN As Integer = writer.PageNumber
            Dim text As String = "Pagina " & pageN.ToString() & " di "


            Dim len As Single = Me.font.GetWidthPoint(text, 10)
            Dim adjust = font.GetWidthPoint("0", 10)

            Dim pageSize As iTextSharp.text.Rectangle = document.PageSize

            Me.cb.BeginText()
            Me.cb.SetFontAndSize(Me.font, 10)
            Me.cb.SetTextMatrix(document.Right - len - adjust, pageSize.GetBottom(document.BottomMargin) - 20)
            Me.cb.ShowText(text)
            Me.cb.EndText()


            Me.cb.AddTemplate(Me.template, document.Right - adjust, pageSize.GetBottom(document.BottomMargin) - 20)
        End Sub

        'Metodo OnCloseDocument
        Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
            MyBase.OnCloseDocument(writer, document)
            Me.template.BeginText()
            Me.template.SetFontAndSize(Me.font, 10)
            Me.template.SetTextMatrix(0, 0)
            Me.template.ShowText("" & (writer.PageNumber - 1))
            Me.template.EndText()
        End Sub

    End Class


End Class