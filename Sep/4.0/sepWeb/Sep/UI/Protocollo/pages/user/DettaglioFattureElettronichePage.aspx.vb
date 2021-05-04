Imports ParsecAdmin
Imports ParsecPro
Imports Telerik.Web.UI
Imports System.IO
Imports System.Net
Imports System.Xml

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class DettaglioFattureElettronichePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    'Variabile di Sessione: Fattura Elettronica
    Public Property FatturaElettronica() As ParsecPro.FatturaElettronica
        Get
            Return CType(Session("DettaglioFattureElettronichePage_FatturaElettronica"), ParsecPro.FatturaElettronica)
        End Get
        Set(ByVal value As ParsecPro.FatturaElettronica)
            Session("DettaglioFattureElettronichePage_FatturaElettronica") = value
        End Set
    End Property

    'Variabile di Sessione: lista delle Fatture Elettroniche associata alla grigila
    Public Property FattureElettroniche() As List(Of ParsecPro.FatturaElettronica)
        Get
            Return CType(Session("DettaglioFattureElettronichePage_FattureElettroniche"), List(Of ParsecPro.FatturaElettronica))
        End Get
        Set(ByVal value As List(Of ParsecPro.FatturaElettronica))
            Session("DettaglioFattureElettronichePage_FattureElettroniche") = value
        End Set
    End Property

    'Variabile di Sessione: lista degli oggetti (Utenti e/o Gruppi) per la visibilità.
    Public Property VisibilitaRicevute() As Boolean
        Get
            Return CType(Session("DettaglioFattureElettronichePage_VisibilitaRicevute"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Session("DettaglioFattureElettronichePage_VisibilitaRicevute") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> Dettaglio Fatture Elettroniche"

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

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

            Dim funzioni As New ParsecAdmin.FunzioniUtenteRepository
            VisibilitaRicevute = Not funzioni.GetQuery.Where(Function(c) c.IdFunzione = 46 _
                                                                     And c.IdUtente = utenteCollegato.Id).FirstOrDefault Is Nothing
            funzioni.Dispose()

            chkRicevuta.Checked = VisibilitaRicevute
            chkRicevuta.Enabled = VisibilitaRicevute

            'Imposto l'ordinamento predefinito.
            Dim sortExpr As New Telerik.Web.UI.GridSortExpression()
            sortExpr.FieldName = "AnnoProtocollo"
            sortExpr.SortOrder = Telerik.Web.UI.GridSortOrder.Ascending
            Me.FattureElettronicheGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

        End If

        Me.FattureElettronicheGridView.GroupingSettings.CaseSensitive = False
       
    End Sub

    'Evento LoadComplete della Pagina: dopo che la pagina è stata caricata setta il titolo della Griglia.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloElencoFattureLabel.Text = "Elenco Dettaglio Fatture Elettroniche " & If(Me.FattureElettroniche.Count > 0, "( " & Me.FattureElettroniche.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla griglia FattureElettronicheGridView. Aggancia il datasource della griglia al DB e aggiorna la lista variabile di sessione FattureElettroniche.
    Protected Sub FattureElettronicheGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FattureElettronicheGridView.NeedDataSource
        If Me.FattureElettroniche Is Nothing Then
            Dim fatture As New ParsecPro.FatturaElettronicaRepository
            Me.FattureElettroniche = fatture.GetView(Me.GetFiltro).OrderBy(Function(c) c.MessaggioSdI.DataRicezioneInvio).ToList
            fatture.Dispose()
        End If
        Me.FattureElettronicheGridView.DataSource = Me.FattureElettroniche
    End Sub

    'Evento ItemDataBound associato alla griglia FattureElettronicheGridView. Definisce i Tooltip e le icone in base al contenuto delle celle della griglia.
    Protected Sub FattureElettronicheGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles FattureElettronicheGridView.ItemDataBound

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim btn As ImageButton = Nothing

            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim fattura As ParsecPro.FatturaElettronica = CType(e.Item.DataItem, ParsecPro.FatturaElettronica)

            Dim stato As ParsecPro.StatoFattura = CType(fattura.IdStato, ParsecPro.StatoFattura)

            btn = CType(dataItem.FindControl("IdStato"), ImageButton)
            btn.Attributes.Add("onclick", "return false")

            Select Case stato

                Case ParsecPro.StatoFattura.Ricevuta

                    btn.ImageUrl = "~/images/pGiallo16.png"
                    btn.ToolTip = "Ricevuta"

                Case ParsecPro.StatoFattura.Protocollata

                    btn.ImageUrl = "~/images/pArancio16.png"
                    btn.ToolTip = "Protocollata"

                Case ParsecPro.StatoFattura.Accettata
                    btn.ImageUrl = "~/images/pVerde16.png"
                    btn.ToolTip = "Accettata"

                Case ParsecPro.StatoFattura.Contabilizzata

                    btn.ImageUrl = "~/images/pFucsia16.png"
                    btn.ToolTip = "Contabilizzata"

                Case ParsecPro.StatoFattura.Convervata

                    btn.ImageUrl = "~/images/pBlue16.png"
                    btn.ToolTip = "Conservata"

                Case ParsecPro.StatoFattura.Rifiutata


                    btn.ImageUrl = "~/images/pRosso16.png"
                    btn.ToolTip = "Rifiutata"

            End Select

        End If
    End Sub

    'Evento ItemCommand associato alla griglia FattureElettronicheGridView. Fa partire l'esecuzione dei comandi definiti nella griglia.
    Protected Sub FattureElettronicheGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles FattureElettronicheGridView.ItemCommand
        If e.CommandName = Telerik.Web.UI.RadGrid.ExpandCollapseCommandName AndAlso Not e.Item.Expanded Then
            Dim parentItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
            Dim innerGrid As Telerik.Web.UI.RadGrid = CType(parentItem.ChildItem.FindControl("NotificheGridView"), Telerik.Web.UI.RadGrid)
            innerGrid.Rebind()
        ElseIf e.CommandName = "Anteprima" Then
            Me.VisualizzaFattura(e.Item)
        End If
    End Sub

    'Metodo che apre il pannello per visualizare la Fattura Elettronica.
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

    'Evento ItemCreated associato alla griglia FattureElettronicheGridView. Definisce gli stili di GridFilteringItem, GridHeaderItem e GridPagerItem.
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

        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False

        End If

    End Sub

#End Region

#Region "EVENTI CONTROLLO VISUALIZZA FATTURA "

    'Chiude il Pannello della Fattura.
    Protected Sub VisualizzaFatturaControl_OnCloseEvent() Handles VisualizzaFatturaControl.OnCloseEvent
        Me.VisualizzaFatturaControl.Visible = False
    End Sub

#End Region

#Region "AZIONI PANNELLO FILTRO"

    'Resetta i campi che fungono da Filtro per la ricerca.
    Private Sub ResettaFiltro()
        For Each col As GridColumn In Me.FattureElettronicheGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.FattureElettronicheGridView.MasterTableView.FilterExpression = String.Empty
        Me.DataInvioInizioTextBox.SelectedDate = Nothing
        Me.DataInvioFineTextBox.SelectedDate = Nothing
        chkRicevuta.Checked = VisibilitaRicevute
        chkProtocollata.Checked = True
        chkAccettata.Checked = False
        chkRifiutata.Checked = False
        chkConservata.Checked = False
        chkContabilizzata.Checked = False
    End Sub

    'Costruisce il Filtro per la ricerca e lo restituisce.
    Private Function GetFiltro() As ParsecPro.FiltroFatturaElettronica
        Dim filtro As New ParsecPro.FiltroFatturaElettronica

        filtro.DataInizioRicezioneInvio = Me.DataInvioInizioTextBox.SelectedDate
        filtro.DataFineRicezioneInvio = Me.DataInvioFineTextBox.SelectedDate

        If chkRicevuta.Checked Then
            filtro.ElencoStati.Add(StatoFattura.Ricevuta)
        End If

        If chkProtocollata.Checked Then
            filtro.ElencoStati.Add(StatoFattura.Protocollata)
        End If

        If chkAccettata.Checked Then
            filtro.ElencoStati.Add(StatoFattura.Accettata)
        End If


        If chkRifiutata.Checked Then
            filtro.ElencoStati.Add(StatoFattura.Rifiutata)
        End If

        If chkConservata.Checked Then
            filtro.ElencoStati.Add(StatoFattura.Convervata)
        End If

        If Me.chkContabilizzata.Checked Then
            filtro.ElencoStati.Add(StatoFattura.Contabilizzata)
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        filtro.IdUtenteCollegato = utenteCollegato.Id

        Return filtro
    End Function

    'Aggiorna la grigla. Refresh.
    Private Sub AggiornaGriglia()
        Me.FattureElettroniche = Nothing
        Me.FattureElettronicheGridView.Rebind()
    End Sub

    'Fa partire la ricerca e quindi l' aggiornamento della griglia.
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FiltraImageButton.Click
        Me.AggiornaGriglia()
    End Sub

    'Fa partire il resetta dei campi della pagina e l'aggiornamento della griglia.
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.AggiornaGriglia()
    End Sub

#End Region

#Region "METODI PRIVATI"

    'Metodo che fa partire l'aggiornamento su DB delle Fatture Scadute.
    Public Sub AggiornaScadute()
        Dim InvoiceR As New ParsecPro.FatturaElettronicaRepository
        Dim FatturaElettronicaFiltro As New ParsecPro.FiltroFatturaElettronica
        FatturaElettronicaFiltro.IdStato = 1

        Dim ListaFatture As New List(Of FatturaElettronica)

        ListaFatture = InvoiceR.GetView(FatturaElettronicaFiltro).ToList

        For Each f In ListaFatture
            Dim giorni As Integer = (Now - f.MessaggioSdI.DataRicezioneInvio).Days
            If giorni < 12 Then
                f.IdStato = 4
                InvoiceR.SaveChanges()
            End If
        Next

        InvoiceR.Dispose()
    End Sub

#End Region

#Region "ESPORTAZIONE EXCEL"

    'Evento che fa partire la esportazione in Excel della Griglia delle Fatture.
    Protected Sub EsportaInExcelImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EsportaInExcelImageButton.Click

        If Me.FattureElettroniche.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Non ci sono fatture elettroniche." & vbCrLf & "Impossibile eseguire l'esportazione!", False)
            Exit Sub
        End If

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("FattureElettroniche_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As New StringBuilder

        line.Append("N. PROT." & vbTab)
        line.Append("ANNO PROT." & vbTab)

        line.Append("FORNITORE" & vbTab)
        line.Append("DESTINATARIO" & vbTab)

        line.Append("ESTREMI FATTURA" & vbTab)
        line.Append("PARTITA IVA" & vbTab)

        line.Append("CIG" & vbTab)
        line.Append("COLLOCAZIONE UTENZA" & vbTab)
        line.Append("TOTALE IMPONIBILE" & vbTab)
        line.Append("IVA" & vbTab)
        line.Append("TOTALE FATTURA" & vbTab)

        swExport.WriteLine(line.ToString)
        line.Clear()

        Dim pageCount As Integer = FattureElettronicheGridView.MasterTableView.PageCount
        Dim i As Integer = 0
        While i < pageCount
            FattureElettronicheGridView.CurrentPageIndex = i
            FattureElettronicheGridView.Rebind()

            For Each f As GridDataItem In FattureElettronicheGridView.MasterTableView.Items

                Dim idFattura As String = f.OwnerTableView.DataKeyValues(f.ItemIndex)("Id")
                Dim fattura = Me.FattureElettroniche.Where(Function(w) w.Id = idFattura).FirstOrDefault

                line.Append(If(fattura.NumeroProtocollo.HasValue, fattura.NumeroProtocollo.ToString, "") & vbTab)
                line.Append(If(fattura.AnnoProtocollo.HasValue, fattura.AnnoProtocollo.ToString, "") & vbTab)

                line.Append(If(Not String.IsNullOrEmpty(fattura.DenominazioneFornitore), fattura.DenominazioneFornitore, "") & vbTab)
                line.Append(If(Not String.IsNullOrEmpty(fattura.DenominazioneDestinatario), fattura.DenominazioneDestinatario, "") & vbTab)
                line.Append(If(Not String.IsNullOrEmpty(fattura.Oggetto), fattura.Oggetto, "") & vbTab)
                line.Append(If(Not String.IsNullOrEmpty(fattura.PartitaIvaFornitore), fattura.PartitaIvaFornitore, "") & vbTab)

                line.Append(If(Not String.IsNullOrEmpty(fattura.CIG), fattura.CIG, "") & vbTab)
                line.Append(If(Not String.IsNullOrEmpty(fattura.CollocazioneUtenza), fattura.CollocazioneUtenza, "") & vbTab)
                line.Append(If(Not String.IsNullOrEmpty(fattura.TotaleImponibile), fattura.TotaleImponibile.ToString, "") & vbTab)
                line.Append(If(Not String.IsNullOrEmpty(fattura.AliquotaIVA), fattura.AliquotaIVA.ToString, "") & vbTab)
                line.Append(If(Not String.IsNullOrEmpty(fattura.TotaleFattura), fattura.TotaleFattura.ToString, "") & vbTab)

                swExport.WriteLine(line.ToString)
                line.Clear()

            Next
            i += 1
        End While

        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

    End Sub

    'Effettua la sincronizzazione delle informazioni della Griglia in base contenuto del file p7m della fattura scaricato dal Protocollo.
    Protected Sub sincronizzazioneFattureButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles sincronizzazioneFattureButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim fatturaElettronicaRepository As New ParsecPro.FatturaElettronicaRepository
        Dim fatturaFiltro As New ParsecPro.FiltroFatturaElettronica
        fatturaFiltro.IdUtenteCollegato = utenteCollegato.Id
        Dim listaFattureElettroniche = fatturaElettronicaRepository.GetView(fatturaFiltro).OrderBy(Function(c) c.MessaggioSdI.DataRicezioneInvio).ToList

        Dim dettaglioFattureRepository As New ParsecPro.DettaglioFatturaElettronicaRepository
        Dim listaIdDettaglioFatture = dettaglioFattureRepository.GetView(Nothing).Select(Function(s) s.fkFatturaElettronica).ToList


        Dim listaFattureDaSincronizzare As List(Of ParsecPro.FatturaElettronica)
        If (listaIdDettaglioFatture.Count > 0) Then
            listaFattureDaSincronizzare = listaFattureElettroniche.Where(Function(w) Not listaIdDettaglioFatture.Contains(w.Id)).ToList
        Else
            listaFattureDaSincronizzare = listaFattureElettroniche
        End If

        Dim count = listaFattureDaSincronizzare.Count
        Dim context = RadProgressContext.Current
        context.PrimaryTotal = count.ToString
        Dim i As Integer = 0
        For Each fattura In listaFattureDaSincronizzare

            i += 1
            context.PrimaryValue = i.ToString

            Dim percentCompleted = CInt((0.5F + ((100.0F * i) / count)))

            context.PrimaryPercent = percentCompleted.ToString

            If Not Response.IsClientConnected Then
                Exit For
            End If

            Dim nomeFile = fattura.MessaggioSdI.Nomefile

            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomeFile

            If IO.File.Exists(localPath) Then

                Dim file = IO.File.ReadAllBytes(localPath)

                Dim memStreamFatturaForInterscambio As System.IO.MemoryStream = Nothing
                memStreamFatturaForInterscambio = New System.IO.MemoryStream(FixVersioneXml(file).ToArray)
                Dim documentoFattura As New XmlDocument
                Try
                    'è un xml forse
                    documentoFattura.Load(memStreamFatturaForInterscambio)
                Catch ex As Exception
                    'è un p7m forse
                    Try
                        Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
                        'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
                        Try
                            file = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(file))
                        Catch ex2 As Exception
                            'NIENTE
                        End Try
                        signedCms.Decode(file)
                        memStreamFatturaForInterscambio = New MemoryStream(FixVersioneXml(signedCms.ContentInfo.Content).ToArray)
                        documentoFattura.Load(memStreamFatturaForInterscambio)
                    Catch ex2 As Exception
                        'non è un formato valido
                        documentoFattura = Nothing
                    End Try
                End Try

                If (Not documentoFattura Is Nothing) Then

                    Dim CessionarioCommittente As XmlNodeList = documentoFattura.GetElementsByTagName("CessionarioCommittente")
                    'sezione indirizzo
                    Dim indirizzo As String = ""
                    Dim cap As String = ""
                    Dim comune As String = ""
                    Dim civico As String = ""
                    Try
                        indirizzo = CessionarioCommittente(0).Item("Sede").Item("Indirizzo").InnerText
                        cap = CessionarioCommittente(0).Item("Sede").Item("CAP").InnerText
                        comune = CessionarioCommittente(0).Item("Sede").Item("Comune").InnerText
                    Catch ex As Exception

                    End Try
                    Try
                        civico = ", n. " & CessionarioCommittente(0).Item("Sede").Item("NumeroCivico").InnerText
                    Catch ex As Exception

                    End Try
                    Dim indirizzoCompleto = indirizzo & civico & " -  CAP " & cap & " (" & comune & ")"
                    If (indirizzoCompleto.Length > 150) Then
                        indirizzoCompleto = indirizzoCompleto.Substring(0, 149) & "..."
                    End If
                    'fine sezione indirizzo

                    'DatiOrdineAcquisto
                    Dim DatiOrdineAcquisto As XmlNodeList = documentoFattura.GetElementsByTagName("DatiOrdineAcquisto")
                    Dim cig = ""
                    Try
                        cig = DatiOrdineAcquisto(0).Item("CodiceCIG").InnerText
                    Catch ex As Exception
                        cig = ""
                    End Try
                    Dim cup = ""
                    Try
                        cup = DatiOrdineAcquisto(0).Item("CodiceCUP").InnerText
                    Catch ex As Exception
                        cup = ""
                    End Try
                    'fine DatiOrdineAcquisto

                    'DatiOrdineAcquisto
                    Dim datiGeneraliDocumento As XmlNodeList = documentoFattura.GetElementsByTagName("DatiGeneraliDocumento")

                    Dim datiRiepilogo As XmlNodeList = documentoFattura.GetElementsByTagName("DatiRiepilogo")
                    Dim imponibile As Double = 0
                    Dim imposta As Double = 0 'AliquotaIVA
                    Dim totaleFattura As Double = 0

                    For contacicli As Integer = 0 To datiRiepilogo.Count - 1
                        Try
                            Dim imponibileTemp = Math.Round(CDbl(datiRiepilogo(contacicli).Item("ImponibileImporto").InnerText.Replace(".", ",")), 2, MidpointRounding.AwayFromZero)
                            Dim impostaTemp = Math.Round(CDbl(datiRiepilogo(contacicli).Item("Imposta").InnerText.Replace(".", ",")), 2, MidpointRounding.AwayFromZero)

                            Dim totaleFatturaTemp = Math.Round(imponibileTemp + impostaTemp, 2, MidpointRounding.AwayFromZero)

                            imponibile = Math.Round(imponibile + imponibileTemp, 2, MidpointRounding.AwayFromZero)
                            imposta = Math.Round(imposta + impostaTemp, 2, MidpointRounding.AwayFromZero)
                            totaleFattura = Math.Round(totaleFattura + totaleFatturaTemp, 2, MidpointRounding.AwayFromZero)

                        Catch ex As Exception

                        End Try

                    Next

                    'fine DatiOrdineAcquisto
                    Dim dettaglioFattura As New ParsecPro.DettaglioFattura
                    dettaglioFattura.fkFatturaElettronica = fattura.Id
                    dettaglioFattura.cig = cig
                    dettaglioFattura.cup = cup
                    dettaglioFattura.collocazioneUtenza = indirizzoCompleto
                    dettaglioFattura.IVA = imposta
                    dettaglioFattura.totaleImponibile = imponibile
                    dettaglioFattura.totale = totaleFattura
                    dettaglioFattureRepository.Save(dettaglioFattura)
                    'dettaglioFattura.Dispose()

                End If

            End If

        Next

        context.OperationComplete = True

        fatturaElettronicaRepository.Dispose()
        dettaglioFattureRepository.Dispose()

        Me.AggiornaGriglia()


    End Sub

    'Metodo che serve per la lettura del file della Fattura Elettronica.
    Private Shared Function FixVersioneXml(ByVal input As Byte()) As IO.MemoryStream
        Dim enc As New System.Text.UTF8Encoding
        Dim content As String = enc.GetString(input)
        Dim startPos As Integer = content.IndexOf("<?xml", StringComparison.OrdinalIgnoreCase) + 5
        Dim endPos As Integer = content.IndexOf("?>", startPos)
        If endPos <> -1 Then
            Dim header As String = content.Substring(startPos, endPos - startPos)

            Dim versione As String = "1.1"
            Dim v = header.IndexOf("version=")
            If v <> -1 Then
                versione = header.Substring(v + 9, 3)
            End If
            header = header.Replace("version=" & Chr(34) & versione & Chr(34) & "", "version=""1.0""")
            content = String.Format("{0}{1}{2}", content.Substring(0, startPos), header, content.Substring(endPos))
        End If
        Dim buffer As Byte() = enc.GetBytes(content)
        Dim ms As New IO.MemoryStream(buffer)
        Return ms
    End Function

#End Region


End Class
