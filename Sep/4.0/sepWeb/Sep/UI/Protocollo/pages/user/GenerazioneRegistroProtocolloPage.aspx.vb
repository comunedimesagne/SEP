Imports Telerik.Web.UI
Imports System.Xml
Imports System.IO
Imports System.Data.Objects
Imports System.Web.Services.Protocols
Imports System.ServiceModel

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class GenerazioneRegistroProtocolloPage
    Inherits System.Web.UI.Page


    'Classe di Appoggio: rappresenta una Registrazione di Protocollo
    Private Class Registrazione
        Public Property IdRegistrazione As Integer
        Public Property Oggetto As String
        Public Property NumeroProtocollo As Nullable(Of Integer)
        Public Property DataImmissione As DateTime
        Public Property DataImmissioneTroncata As Date
        Public Property DataRegistrazioneTroncata As Date
        Public Property DescrizioneTipologiaRegistristrazione As String
        Public Property ElencoReferentiInterni As String
        Public Property ElencoReferentiEsterni As String
        Public Property UtenteUsername As String
        Public Property Note As String
        Public Property DescrizioneTipoRicezioneInvio As String
        Public Property DescrizioneTipologiaDocumento As String
        Public Property dataProtocolloMittente As Date?
        Public Property regProtocolloMittente As String
        Public Property listaAllegati As String
        Public Property Classificazione As String
        Public Property Annullata As String
        Public Property DataAnnullamentoTroncata As Date
    End Class

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA"

    'Variabile di Sessione: lista delle voci del Registro di Protocollo associata alla Griglia.
    Private Property ListaRegistriProtocollo() As List(Of ParsecPro.RegistroProtocollo)
        Get
            Return CType(Session("GenerazioneRegistroProtocollo_ListaRegistriProtocollo"), List(Of ParsecPro.RegistroProtocollo))
        End Get
        Set(ByVal value As List(Of ParsecPro.RegistroProtocollo))
            Session("GenerazioneRegistroProtocollo_ListaRegistriProtocollo") = value
        End Set
    End Property

    'Variabile di Sessione: id associato al Registro Firmato.
    Public Property IdRegistroFirmato() As String
        Get
            Return CType(Session("GenerazioneRegistroProtocollo_IdRegistroFirmato"), String)
        End Get
        Set(ByVal value As String)
            Session("GenerazioneRegistroProtocollo_IdRegistroFirmato") = value
        End Set
    End Property

    'Variabile di Sessione: nome del File selezionato.
    Public Property FileNameSelezionato() As String
        Get
            Return CType(Session("GenerazioneRegistroProtocollo_FileNameSelezionato"), String)
        End Get
        Set(ByVal value As String)
            Session("GenerazioneRegistroProtocollo_FileNameSelezionato") = value
        End Set
    End Property

    'Variabile di Sessione: informazioni sull'Ente proprietario dell'Applicativo.
    Public Property Cliente() As ParsecAdmin.Cliente
        Get
            Return CType(Session("GenerazioneRegistroProtocollo_Cliente"), ParsecAdmin.Cliente)
        End Get
        Set(ByVal value As ParsecAdmin.Cliente)
            Session("GenerazioneRegistroProtocollo_Cliente") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = "div.RadUploadProgressArea_Office2007 .ruProgress { background-image: none;}" & vbCrLf
        css.InnerHtml += ".RadUploadProgressArea { width: 320px !important;}" & vbCrLf
        css.InnerHtml += "div.RadUploadProgressArea li.ruProgressHeader{ margin: 10px 18px 0px; }" & vbCrLf
        css.InnerHtml += "table.CkeckListCss tr td label {margin-right:10px;padding-right:10px;}" & vbCrLf

        Me.Page.Header.Controls.Add(css)

        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"

        Me.MainPage.DescrizioneProcedura = "> Generazione Registro Giornaliero"

        Try
            Dim pathProtocollo = ParsecAdmin.WebConfigSettings.GetKey("PathProtocollo")
            Dim pathRelativeProtocollo = ParsecAdmin.WebConfigSettings.GetKey("PathRelativeProtocollo")
            If (pathProtocollo = "" Or pathRelativeProtocollo = "") Then
                ParsecUtility.Utility.MessageBox("Il File Web.config non è stato configurato correttamente: mancano le chiavi del protocollo.", False)
                Exit Sub
            End If

            Me.comboTipologiaRegistro.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Tutti", "-1"))
            Me.comboTipologiaRegistro.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Protocolli", "0"))
            Me.comboTipologiaRegistro.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Protocolli Annullati", "1"))
            Me.comboTipologiaRegistro.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Protocolli Modificati", "2"))

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("obbligatorietaFirmaRegistroProtocollo")
            If parametro Is Nothing Then
                ParsecUtility.Utility.MessageBox("Il parametro obbligatorietaFirmaRegistroProtocollo non è stato trovato.", False)
                'Exit Sub
            End If
            parametri.Dispose()

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox("Il File Web.config non è stato configurato correttamente: manca la chiave PathProtocollo oppure PathRelativeProtocollo", False)
            Exit Sub
        End Try

        If (Not Me.Page.IsPostBack) Then

            Dim clienteRepository As New ParsecAdmin.ClientRepository
            Dim cliente = clienteRepository.GetAll().FirstOrDefault
            Me.Cliente = cliente
            clienteRepository.Dispose()

            settaCampiIniziali()

            Me.ListaRegistriProtocollo = New List(Of ParsecPro.RegistroProtocollo)

        End If

        Dim annoEsercizioRepository As New ParsecPro.EsercizioRepository
        Me.RegistraParsecDigitalSign()

    End Sub

    'effettua la registrazione della firma digitale a livello di pagina
    Private Sub RegistraParsecDigitalSign()
        Dim script As String = ParsecAdmin.SignParameters.RegistraParsecDigitalSign
        Me.MainPage.RegisterComponent(script)
    End Sub

    'Evento LoadComplete della Pagina: dopo che la pagina è stata caricata setta il titolo della Griglia. Inoltre gestisce il messaggio di generazione del registro.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Sei sicuro di voler generare il Registro?", False, True)
        If Not Me.ListaRegistriProtocollo Is Nothing Then
            Me.lblRisultatiRicerca.Text = "Elenco dei Registri &nbsp;&nbsp;" & If(Me.ListaRegistriProtocollo.Count > 0, "( " & Me.ListaRegistriProtocollo.Count.ToString & " )", "")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla griglia grigliaRegistroProtocollo. Aggancia il datasource della griglia al DB.
    Protected Sub grigliaRegistroProtocollo_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grigliaRegistroProtocollo.NeedDataSource
        Me.grigliaRegistroProtocollo.DataSource = Me.ListaRegistriProtocollo
    End Sub

    'Evento ItemCommand associato alla Griglia grigliaRegistroProtocollo. Permette l'esecuzione dei vari comandi attivabili dalla griglia grigliaRegistroProtocollo.
    Protected Sub grigliaRegistroProtocollo_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grigliaRegistroProtocollo.ItemCommand
        Select Case e.CommandName
            Case "PreviewDocumento"
                Dim idDocumento As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("id")
                Dim nomeFile As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("FileNameOriginale")
                Dim nomeFileFirmato As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("FileNameFirmato")
                Dim path As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("path")
                Dim pathRootProtocollo As String = ParsecAdmin.WebConfigSettings.GetKey("PathProtocollo") & path & "\"
                Dim file As New IO.FileInfo(pathRootProtocollo & nomeFile)
                If file.Exists Then
                    Dim maxKiloBytesLength As Integer = CInt(ParsecAdmin.WebConfigSettings.GetKey("MaxKiloBytesLengthAllegatoContratto"))
                    Dim dimensioneFile As Double = file.Length / 1024
                    If (dimensioneFile <= maxKiloBytesLength) Then
                        Session("AttachmentFullName") = file.FullName
                        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                        ParsecUtility.Utility.PageReload(pageUrl, False)
                    Else
                        ParsecUtility.Utility.MessageBox("La dimensione del File supera i " & (maxKiloBytesLength / 1024) & " Mb: contattare l'Amministratore del Sistema!", False)
                    End If
                Else
                    ParsecUtility.Utility.MessageBox("Il File selezionato non esiste!", False)
                End If
            Case "FirmaRegistro"

                Dim nomeFileFirmato As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("FileNameFirmato")
                Dim path As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("path")
                Dim pathRootProtocollo As String = ParsecAdmin.WebConfigSettings.GetKey("PathProtocollo") & Path & "\"
                If (nomeFileFirmato <> "") Then
                    Dim file As New IO.FileInfo(pathRootProtocollo & nomeFileFirmato)
                    If File.Exists Then
                        Dim maxKiloBytesLength As Integer = CInt(ParsecAdmin.WebConfigSettings.GetKey("MaxKiloBytesLengthAllegatoContratto"))
                        Dim dimensioneFile As Double = File.Length / 1024
                        If (dimensioneFile <= maxKiloBytesLength) Then
                            Session("AttachmentFullName") = File.FullName
                            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                            ParsecUtility.Utility.PageReload(pageUrl, False)
                        Else
                            ParsecUtility.Utility.MessageBox("La dimensione del File supera i " & (maxKiloBytesLength / 1024) & " Mb: contattare l'Amministratore del Sistema!", False)
                        End If
                    Else
                        ParsecUtility.Utility.MessageBox("Il File selezionato non esiste!", False)
                    End If
                Else
                    FirmaRegistro(e.Item)
                End If

            Case "Delete"
                Try
                    Dim idRegistro As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("id")
                    Dim nomeFile As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("FileNameOriginale")
                    Dim path As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("path")
                    Dim conservato As Boolean = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("conservato")
                    Dim idDocumentoWS As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("idPacchettoWs")

                    
                    Dim registroProtocolloRepository As New ParsecPro.RegistroProtocolloRepository
                    Dim registrazioneGiornaliera = registroProtocolloRepository.GetById(idRegistro)
                    registroProtocolloRepository.Delete(idRegistro)
                    Me.ListaRegistriProtocollo = registroProtocolloRepository.GetView(getFiltroRegistro)
                    Me.grigliaRegistroProtocollo.Rebind()
                    registroProtocolloRepository.Dispose()

                    Dim pathRootProtocollo As String = ParsecAdmin.WebConfigSettings.GetKey("PathProtocollo") & path & "\"
                    Dim file As New IO.FileInfo(pathRootProtocollo & nomeFile)
                    If file.Exists Then
                        IO.File.Delete(pathRootProtocollo & nomeFile)
                    End If
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                End Try
        End Select

    End Sub

    'Evento ItemCreated associato alla Griglia grigliaRegistroProtocollo. Gestisce  il cambio di pagina.
    Protected Sub grigliaRegistroProtocollo_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grigliaRegistroProtocollo.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    'Evento ItemDataBound associato alla Griglia grigliaRegistroProtocollo. Setta i tooltip e le icone della griglia in base alle informazioni contenute nelle celle.
    Protected Sub grigliaRegistroProtocollo_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grigliaRegistroProtocollo.ItemDataBound
        Dim btnConservazione As ImageButton = Nothing
        Dim btnFirma As ImageButton = Nothing
        Dim btnVisualizzaFile As ImageButton = Nothing

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim registroGiornaliero As ParsecPro.RegistroProtocollo = CType(e.Item.DataItem, ParsecPro.RegistroProtocollo)

            If TypeOf dataItem("FirmaRegistro").Controls(0) Is ImageButton Then
                btnFirma = CType(dataItem("FirmaRegistro").Controls(0), ImageButton)
                If (registroGiornaliero.fileNameFirmato Is Nothing) Then
                    btnFirma.ImageUrl = "~\images\firmaDocumento16.png"
                    btnFirma.ToolTip = "Apponi firma digitale al registro."
                Else
                    btnFirma.ImageUrl = "~\images\signedDocument.png"
                    btnFirma.ToolTip = "Visualizza Registro firmato."
                End If

            End If

        End If
    End Sub

#End Region

#Region "PRIVATI"

    'Aggiorna la Griglia in base ai risultati della ricerca.
    Private Sub AggiornaGriglia()
        Dim registroRepository = New ParsecPro.RegistroProtocolloRepository
        Me.ListaRegistriProtocollo = registroRepository.GetView(getFiltroRegistro())
        Me.grigliaRegistroProtocollo.Rebind()
        registroRepository.Dispose()
    End Sub

    'Legge il parametro del numero massimo di pagine per eliminare la Paginazione della griglia.
    Private Function GetlimiteNumeroPagineRicerca() As Integer
        Dim limiteNumeroPAgine As Integer = -1
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("limiteNumeroPaginazioneConservazione", ParsecAdmin.TipoModulo.CSZ)
        If Not parametro Is Nothing Then
            limiteNumeroPAgine = CInt(parametro.Valore)
        End If
        parametri.Dispose()
        Return limiteNumeroPAgine
    End Function

    'Evento click per eliminare/attivare la paginazione
    Protected Sub NoPaging_Click(sender As Object, e As System.EventArgs) Handles NoPaging.Click
        Dim limitePagine = GetlimiteNumeroPagineRicerca()
        If (Me.grigliaRegistroProtocollo.PageCount > limitePagine) Then
            ParsecUtility.Utility.MessageBox("Il numero di record risultante dalla ricerca è eccessivo: si prega di raffinare la ricerca!", False)
            Exit Sub
        End If

        Me.grigliaRegistroProtocollo.AllowPaging = Not Me.grigliaRegistroProtocollo.AllowPaging
        If Me.grigliaRegistroProtocollo.AllowPaging Then
            Me.NoPaging.Text = "Non Paginare"
            Me.NoPaging.Icon.PrimaryIconUrl = "~/images/Next.png"
        Else
            Me.NoPaging.Text = "Paginare"
            Me.NoPaging.Icon.PrimaryIconUrl = "~/images/Previous.png"
        End If
        Me.grigliaRegistroProtocollo.Rebind()
    End Sub

    'Impostazioni iniziali dei campi della pagina.
    Private Sub settaCampiIniziali()

        Dim dataRiferimento As Date = Now.AddDays(-1)

        Me.DataProtocolloInizioTextBox.SelectedDate = New Date(dataRiferimento.Year, dataRiferimento.Month, dataRiferimento.Day)
        Me.DataProtocolloFineTextBox.SelectedDate = New Date(dataRiferimento.Year, dataRiferimento.Month, dataRiferimento.Day)
        Me.comboTipologiaRegistro.SelectedValue = "-1"

    End Sub

    'Costruisce e resutuisce il fitro per la Ricerca dei registri
    Private Function getFiltroRegistro() As ParsecPro.RegistroProtocolloFiltro

        Dim filtroRegistroProtocollo As New ParsecPro.RegistroProtocolloFiltro

        If (Me.DataProtocolloInizioTextBox.SelectedDate.HasValue) Then
            Dim dataIniziale As Date = New Date(DataProtocolloInizioTextBox.SelectedDate.Value.Year, DataProtocolloInizioTextBox.SelectedDate.Value.Month, DataProtocolloInizioTextBox.SelectedDate.Value.Day, 0, 0, 0)
            filtroRegistroProtocollo.DataInizio = dataIniziale
        End If

        If (Me.DataProtocolloFineTextBox.SelectedDate.HasValue) Then
            Dim dataFinale As Date = New Date(DataProtocolloFineTextBox.SelectedDate.Value.Year, DataProtocolloFineTextBox.SelectedDate.Value.Month, DataProtocolloFineTextBox.SelectedDate.Value.Day, 23, 59, 59)
            filtroRegistroProtocollo.DataFine = dataFinale
        End If

        If (Me.comboTipologiaRegistro.SelectedValue <> "-1") Then
            filtroRegistroProtocollo.tipologia = Me.comboTipologiaRegistro.SelectedValue
        End If

        Return filtroRegistroProtocollo

    End Function

    'Lacia la firma del Regsitro
    Private Sub FirmaRegistro(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idDocumento As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("id")
        Dim nomeFile As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("FileNameOriginale")
        Dim nomeFileFirmato As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("FileNameFirmato")
        Dim path As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("path")

        Try
            If (nomeFileFirmato = "") Then

                Me.EffettuaFirmaDocumento(nomeFile, path.Substring(1))
                Me.IdRegistroFirmato = idDocumento
                Me.FileNameSelezionato = nomeFile

            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try

    End Sub

    'Effettua la firma del Registro
    Private Sub EffettuaFirmaDocumento(ByVal nomeFile As String, ByVal anno As String)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato
        Dim provider As ParsecAdmin.ProviderType = CType(utenteCorrente.IdProviderFirma, ParsecAdmin.ProviderType)

        Dim pathFileToSign As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeProtocollo") & anno & "/" & nomeFile

        Dim signParameters As New ParsecAdmin.SignParameters

        Dim datiInput As New ParsecAdmin.DatiFirma With {
            .RemotePathToSign = pathFileToSign,
            .Provider = provider,
            .FunctionName = "SignDocument",
            .CertificateId = certificateId
        }

        Dim data As String = signParameters.CreaDataSource(datiInput)

        ParsecUtility.Utility.RegistraTimerEseguiFirma(data, AggiornaRegistroFirmato.ClientID, False, False)

    End Sub

    'Metodo che scatta dopo la Firma del Registro.
    Protected Sub AggiornaRegistroFirmato_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaRegistroFirmato.Click

        Try
            Dim registroProtocolloRepository As New ParsecPro.RegistroProtocolloRepository
            Dim registroProtocollo = registroProtocolloRepository.GetById(Me.IdRegistroFirmato)

            If (Not registroProtocollo Is Nothing) Then

                If (IO.File.Exists(ParsecAdmin.WebConfigSettings.GetKey("PathProtocollo") & registroProtocollo.path & "\" & FileNameSelezionato & ".p7m")) Then
                    registroProtocollo.fileNameFirmato = FileNameSelezionato & ".p7m"
                    registroProtocolloRepository.Save(registroProtocollo)
                    Me.IdRegistroFirmato = Nothing
                    Me.FileNameSelezionato = String.Empty
                    ParsecUtility.Utility.MessageBox("Registro Giornaliero firmato correttamente!", False)
                    Me.ListaRegistriProtocollo = registroProtocolloRepository.GetView(getFiltroRegistro())
                    Me.grigliaRegistroProtocollo.Rebind()
                Else
                    ParsecUtility.Utility.MessageBox("Registro non firmato.", False)
                End If

            End If
            registroProtocolloRepository.Dispose()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try

    End Sub

    'Evento che lancia la generazione dei registri di protocollo
    Protected Sub cmbGeneraRegistroGiornaliero_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGeneraRegistroGiornaliero.Click

        Try
            If (Not Me.DataProtocolloInizioTextBox.SelectedDate.HasValue Or Not Me.DataProtocolloFineTextBox.SelectedDate.HasValue) Then
                ParsecUtility.Utility.MessageBox("Impossibile generare i Registri di Protocollo giornalieri: occorre specificare il range di date!", False)
            ElseIf (Me.DataProtocolloFineTextBox.SelectedDate < Me.DataProtocolloInizioTextBox.SelectedDate) Then
                ParsecUtility.Utility.MessageBox("Impossibile generare i Registri di Protocollo giornalieri: la data inziale deve essere minore di quella finale!", False)
            Else

                Dim registriGiornalieriGenerati As Boolean = False

                Dim registroProtocolloRep As New ParsecPro.RegistroProtocolloRepository
                Dim listaDateSenzaRegistro = registroProtocolloRep.getListaDateProtocolloSenzaRegistro(Me.DataProtocolloInizioTextBox.SelectedDate, Me.DataProtocolloFineTextBox.SelectedDate).Where(Function(w) w < Now).ToList

                If (listaDateSenzaRegistro Is Nothing Or listaDateSenzaRegistro.Count = 0) Then
                    registriGiornalieriGenerati = False
                Else
                    registriGiornalieriGenerati = True
                    For Each item In listaDateSenzaRegistro
                        generazioneMassiva(item)
                    Next
                    registroProtocolloRep = New ParsecPro.RegistroProtocolloRepository

                End If


                Dim trovatiAnnullati As String = lanciaReportAnnullati()

                Dim trovatiModificati As String = lanciaReportModificati()

                Me.ListaRegistriProtocollo = registroProtocolloRep.GetView(getFiltroRegistro)
                Me.grigliaRegistroProtocollo.Rebind()
                registroProtocolloRep.Dispose()

                Dim messaggioFinale As String = ""

                If (registriGiornalieriGenerati) Then
                    messaggioFinale = "I registri di protocollo giornalieri sono stati generati correttamente." & vbCrLf
                End If
                If (trovatiModificati <> "") Then
                    messaggioFinale = messaggioFinale & "Sono stati generati i Registri di protocolli modificati relativi a registrazioni effettuate nelle seguenti date:" + vbCrLf + trovatiModificati
                End If
                If (trovatiAnnullati <> "") Then
                    messaggioFinale = messaggioFinale & "Sono stati generati i Registri di protocolli annullati relativi a registrazioni annullate nelle seguenti date: " + vbCrLf + trovatiAnnullati
                End If

                If (messaggioFinale <> "") Then
                    ParsecUtility.Utility.MessageBox(messaggioFinale, False)
                Else
                    ParsecUtility.Utility.MessageBox("Non sono state trovate registrazioni di protocollo nell'arco temporale indicato.", False)
                End If


            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try

    End Sub

    'Genera massivamente i Registri di Protocollo
    Private Sub generazioneMassiva(ByVal dataRegistro As Date)
        Try

            If (IsDate(dataRegistro)) Then

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                Dim startDate As Date = New Date(dataRegistro.Year, dataRegistro.Month, dataRegistro.Day, 0, 0, 0)
                Dim endDate As Date = New Date(dataRegistro.Year, dataRegistro.Month, dataRegistro.Day, 23, 59, 59)

                Dim registrazioneProtocollo As New ParsecPro.RegistrazioniRepository()
                Dim numeroProtocolloMinimo = registrazioneProtocollo.GetQuery().Where(Function(w) w.DataImmissione >= startDate And w.DataImmissione <= endDate).Min(Function(m) m.NumeroProtocollo)
                Dim numeroProtocolloMassimo = registrazioneProtocollo.GetQuery().Where(Function(w) w.DataImmissione >= startDate And w.DataImmissione <= endDate).Max(Function(m) m.NumeroProtocollo)
                Dim dataProtocolloMinimo As DateTime = registrazioneProtocollo.GetQuery().Where(Function(w) w.DataImmissione >= startDate And w.DataImmissione <= endDate).Min(Function(m) m.DataImmissione)
                Dim dataProtocolloMassimo As DateTime = registrazioneProtocollo.GetQuery().Where(Function(w) w.DataImmissione >= startDate And w.DataImmissione <= endDate).Max(Function(m) m.DataImmissione)

                'GIORNALIERA
                Dim r As ParsecReporting.StampaElencoRegistrazioniReport = ParsecReporting.ReportManager.CreaStampaElencoRegistrazioni(queryGiornaliero(dataRegistro))
                Dim fileName As String = ""
                Dim result As Telerik.Reporting.Processing.RenderingResult = Nothing

                r.Titolo = "Registro Protocollo del " & dataRegistro.ToShortDateString
                r.RagioneSociale = Me.Cliente.Descrizione
                Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
                result = reportProcessor.RenderReport("PDF", r, Nothing)

                Dim mese As String = IIf(dataRegistro.Month > 9, dataRegistro.Month, "0" & dataRegistro.Month)
                Dim giorno As String = IIf(dataRegistro.Day > 9, dataRegistro.Day, "0" & dataRegistro.Day)
                fileName = "RegistroProtocollo_" + dataRegistro.Year.ToString + mese + giorno + ".pdf"

                Dim pathRootProtocollo As String = ParsecAdmin.WebConfigSettings.GetKey("PathProtocollo") & dataRegistro.Year.ToString + "\"

                If (Not IO.Directory.Exists(pathRootProtocollo)) Then
                    IO.Directory.CreateDirectory(pathRootProtocollo)
                End If

                IO.File.WriteAllBytes(pathRootProtocollo & fileName, result.DocumentBytes)

                Dim registroGiornalieroRep As New ParsecPro.RegistroProtocolloRepository

                Dim registroGiornaliero As New ParsecPro.RegistroProtocollo
                registroGiornaliero.path = "\" & dataRegistro.Year
                registroGiornaliero.tipologia = 0
                registroGiornaliero.dataRegistro = dataRegistro
                registroGiornaliero.daNumero = numeroProtocolloMinimo
                registroGiornaliero.aNumero = numeroProtocolloMassimo
                registroGiornaliero.daData = dataProtocolloMinimo
                registroGiornaliero.aData = dataProtocolloMassimo
                registroGiornaliero.idUtente = utenteCollegato.Id
                registroGiornaliero.dataCreazioneRegistro = Now

                registroGiornaliero.fileNameOriginale = fileName
                registroGiornaliero.conservato = False
                registroGiornalieroRep.Save(registroGiornaliero)

                registroGiornalieroRep.Dispose()

            Else
                ParsecUtility.Utility.MessageBox("Impossibile generare il Registro!", False)
            End If

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    'Query per trovare le regstrazioni per la generazione del Registro
    Private Function queryGiornaliero(ByVal dataRegistro As Date) As IEnumerable

        Dim startDate As Date = New Date(dataRegistro.Year, dataRegistro.Month, dataRegistro.Day, 0, 0, 0)
        Dim endDate As Date = New Date(dataRegistro.Year, dataRegistro.Month, dataRegistro.Day, 23, 59, 59)

        Dim registrazioneProtocollo As New ParsecPro.RegistrazioniRepository()
        Dim registro = registrazioneProtocollo.GetQuery()
        'la seguente nn può essere null sul db
        Dim tipologieRegistrazione = (New ParsecPro.TipologiaRegistrazioneRepository(registrazioneProtocollo.Context)).GetQuery
        'le seguenti pososno essere null sul db
        Dim tipoRicezione = (New ParsecPro.TipiRicezioneInvioRepository(registrazioneProtocollo.Context)).GetQuery
        Dim tipoDocumento = (New ParsecPro.TipiDocumentoRepository(registrazioneProtocollo.Context)).GetQuery

        Dim res = (From registrazione In registro
                  Join tipoReg In tipologieRegistrazione On registrazione.TipoRegistrazione Equals tipoReg.Id
                  Group Join tpDoc In tipoDocumento On tpDoc.Id Equals registrazione.IdTipoDocumento Into elenco1 = Group From tpDoc In elenco1.DefaultIfEmpty()
                  Group Join tpRic In tipoRicezione On tpRic.Id Equals registrazione.IdTipoRicezione Into elenco2 = Group From tpRic In elenco2.DefaultIfEmpty()
                  Order By registrazione.DataImmissione
                  Where (registrazione.DataImmissione >= startDate And registrazione.DataImmissione <= endDate And registrazione.Modificato = False)
                  Select New With {registrazione, tipoReg, tpDoc, tpRic})

        Dim a = (From r In res.ToList Select New With {
                                .Id = r.registrazione.Id,
                                .Oggetto = r.registrazione.Oggetto,
                                .NumeroProtocollo = r.registrazione.NumeroProtocollo,
                                .DataImmissione = r.registrazione.DataImmissione,
                                .DescrizioneTipologiaRegistristrazione = r.tipoReg.Descrizione,
                                .ElencoReferentiInterni = r.registrazione.ElencoReferentiInterni,
                                .ElencoReferentiEsterni = r.registrazione.ElencoReferentiEsterni,
                                .UtenteUsername = "UTENTE: " & vbCrLf & r.registrazione.UtenteUsername,
                                .Note = "NOTE: " & vbCrLf & r.registrazione.Note,
                                .DescrizioneTipoRicezioneInvio = If(r.tpRic Is Nothing, "TIPO RICEZIONE:", "TIPO RICEZIONE:" & vbCrLf & r.tpRic.Descrizione),
                                .DescrizioneTipologiaDocumento = If(r.tpDoc Is Nothing, "TIPO DOCUMENTO:", "TIPO DOCUMENTO:" & vbCrLf & r.tpDoc.Descrizione),
                                .dataProtocolloMittente = r.registrazione.DataDocumento,
                                .regProtocolloMittente = r.registrazione.ProtocolloMittente,
                                .listaAllegati = GetElencoAllegati(r.registrazione.Id),
                                .annullata = IIf(r.registrazione.DataOraAnnullamento.HasValue, "ANNULLATA IL:" & vbCrLf & r.registrazione.DataOraAnnullamento.ToString, ""),
                                 .Classificazione = If(r.registrazione.IdClassificazione Is Nothing, "CLASSIFICAZIONE:", "CLASSIFICAZIONE:" & vbCrLf & GetClassificazione(r.registrazione.IdClassificazione))
                            })

        Return a

    End Function

    'Trova le date delle registrazioni Annullate e ritorna una stringa contenente le date inerenti
    Private Function lanciaReportAnnullati() As String

        Dim elencoDateTrovate = ""

        Dim dataIniziale As Date = New Date(Me.DataProtocolloInizioTextBox.SelectedDate.Value.Year, Me.DataProtocolloInizioTextBox.SelectedDate.Value.Month, Me.DataProtocolloInizioTextBox.SelectedDate.Value.Day, 0, 0, 0)
        Dim dataFinale As Date = New Date(Me.DataProtocolloFineTextBox.SelectedDate.Value.Year, Me.DataProtocolloFineTextBox.SelectedDate.Value.Month, Me.DataProtocolloFineTextBox.SelectedDate.Value.Day, 23, 59, 59)

        Dim numeroGiorni = DateDiff(DateInterval.Day, dataIniziale, dataFinale)

        Dim recordAnnullati = queryAnnullate().Where(Function(w) w.DataAnnullamentoTroncata <> w.DataImmissioneTroncata)

        Dim listaIdRegistrazioniAnnullate = getListaIdRegistrazioniPerTipologia(1)

        Dim listaAnnullateRagguppate = recordAnnullati.Where(Function(w) Not listaIdRegistrazioniAnnullate.Contains(w.IdRegistrazione)).GroupBy(Function(g) g.DataImmissioneTroncata)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        For Each elemento In listaAnnullateRagguppate

            Dim registroGiornalieroRep As New ParsecPro.RegistroProtocolloRepository

            Dim dataCorrente = elemento.FirstOrDefault.DataImmissioneTroncata

            elencoDateTrovate = elencoDateTrovate + dataCorrente.ToShortDateString + vbCrLf

            Dim r As ParsecReporting.StampaElencoRegistrazioniReport = ParsecReporting.ReportManager.CreaStampaElencoRegistrazioni(elemento.ToList)

            Dim fileName As String = ""
            Dim result As Telerik.Reporting.Processing.RenderingResult = Nothing

            r.Titolo = "Registro dei Protocolli Annullati del " & dataCorrente.ToShortDateString
            r.RagioneSociale = Me.Cliente.Descrizione
            Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
            result = reportProcessor.RenderReport("PDF", r, Nothing)

            'vedo se sono stati generati altri registri per quella data
            Dim filtroRegistro As New ParsecPro.RegistroProtocolloFiltro
            filtroRegistro.tipologia = 1
            filtroRegistro.DataInizio = dataCorrente
            filtroRegistro.DataFine = dataCorrente

            Dim ultimoRegistroAnnullatoGenerato = registroGiornalieroRep.GetView(filtroRegistro).OrderByDescending(Function(o) o.id).FirstOrDefault
            Dim indiceRegistroAnnullatoGenerato As Integer = 1
            If (Not ultimoRegistroAnnullatoGenerato Is Nothing) Then
                Dim nomeFile = IO.Path.GetFileNameWithoutExtension(ultimoRegistroAnnullatoGenerato.fileNameOriginale)
                indiceRegistroAnnullatoGenerato = CInt(nomeFile.Substring(nomeFile.LastIndexOf("_") + 1)) + 1
            End If
            Dim mese As String = IIf(dataCorrente.Month > 9, dataCorrente.Month, "0" & dataCorrente.Month)
            Dim giorno As String = IIf(dataCorrente.Day > 9, dataCorrente.Day, "0" & dataCorrente.Day)
            fileName = "RegistroProtocolloAnnullati_" + dataCorrente.Year.ToString + mese + giorno + "_V_" + indiceRegistroAnnullatoGenerato.ToString + ".pdf"

            Dim pathRootProtocollo As String = ParsecAdmin.WebConfigSettings.GetKey("PathProtocollo") & dataFinale.Year.ToString + "\"

            If (Not IO.Directory.Exists(pathRootProtocollo)) Then
                IO.Directory.CreateDirectory(pathRootProtocollo)
            End If

            IO.File.WriteAllBytes(pathRootProtocollo & fileName, result.DocumentBytes)

            Dim registroGiornaliero As New ParsecPro.RegistroProtocollo
            registroGiornaliero.path = "\" & dataFinale.Year
            registroGiornaliero.tipologia = 1
            registroGiornaliero.dataRegistro = dataCorrente
            registroGiornaliero.fileNameOriginale = fileName
            registroGiornaliero.conservato = False
            registroGiornaliero.idUtente = utenteCollegato.Id
            registroGiornaliero.dataCreazioneRegistro = Now
            registroGiornalieroRep.Save(registroGiornaliero)
            registroGiornalieroRep.Dispose()

            'dettaglio protocolli annullati
            Dim dataAnnIniziale As Date = New Date(dataCorrente.Year, dataCorrente.Month, dataCorrente.Day, 0, 0, 0)
            Dim dataAnnFinale As Date = New Date(dataCorrente.Year, dataCorrente.Month, dataCorrente.Day, 23, 59, 59)
            Dim listaAnnullatiCorrenti = recordAnnullati.Where(Function(w) w.DataImmissione >= dataAnnIniziale And w.DataImmissione <= dataAnnFinale).ToList
            If (listaAnnullatiCorrenti.Count > 0) Then
                For Each protocolloAnnullato In listaAnnullatiCorrenti
                    Dim dettaglioRegistroProtocollorepository As New ParsecPro.DettaglioRegistroProtocolloRepository
                    Dim dettaglioRegistroProtocollo As New ParsecPro.DettaglioRegistroProtocollo
                    dettaglioRegistroProtocollo.IdRegistroProtocollo = registroGiornaliero.id
                    dettaglioRegistroProtocollo.IdRegistrazione = protocolloAnnullato.IdRegistrazione
                    dettaglioRegistroProtocollorepository.Add(dettaglioRegistroProtocollo)
                    dettaglioRegistroProtocollorepository.SaveChanges()
                Next
            End If
            'fine dettaglio
        Next

        Return elencoDateTrovate


    End Function

    'Lanciata da lanciaReportAnnullati(). Restiruisce le registrazioni annullate.
    Private Function queryAnnullate() As List(Of Registrazione)

        Dim dataInizioAnnullata = New Date(Me.DataProtocolloInizioTextBox.SelectedDate.Value.Year, Me.DataProtocolloInizioTextBox.SelectedDate.Value.Month, Me.DataProtocolloInizioTextBox.SelectedDate.Value.Day, 0, 0, 0)
        Dim dataFineAnnullata = New Date(Me.DataProtocolloFineTextBox.SelectedDate.Value.Year, Me.DataProtocolloFineTextBox.SelectedDate.Value.Month, Me.DataProtocolloFineTextBox.SelectedDate.Value.Day, 23, 59, 59)

        Dim registrazioneProtocollo As New ParsecPro.RegistrazioniRepository()
        Dim registro = registrazioneProtocollo.GetQuery()
        Dim tipologieRegistrazione = (New ParsecPro.TipologiaRegistrazioneRepository(registrazioneProtocollo.Context)).GetQuery
        Dim tipoRicezione = (New ParsecPro.TipiRicezioneInvioRepository(registrazioneProtocollo.Context)).GetQuery
        Dim tipoDocumento = (New ParsecPro.TipiDocumentoRepository(registrazioneProtocollo.Context)).GetQuery

        Dim res = (From registrazione In registro
                  Join tipoReg In tipologieRegistrazione On registrazione.TipoRegistrazione Equals tipoReg.Id
                  Group Join tpDoc In tipoDocumento On tpDoc.Id Equals registrazione.IdTipoDocumento Into elenco1 = Group From tpDoc In elenco1.DefaultIfEmpty()
                  Group Join tpRic In tipoRicezione On tpRic.Id Equals registrazione.IdTipoRicezione Into elenco2 = Group From tpRic In elenco2.DefaultIfEmpty()
                  Where (registrazione.Annullato = True And registrazione.DataOraAnnullamento >= dataInizioAnnullata And registrazione.DataOraAnnullamento <= dataFineAnnullata)
                  Order By registrazione.DataImmissione
                  Select New With {registrazione, tipoReg, tpDoc, tpRic})



        Dim a = (From r In res.AsEnumerable Select New Registrazione With {
                                .IdRegistrazione = r.registrazione.Id,
                                .Oggetto = r.registrazione.Oggetto,
                                .NumeroProtocollo = r.registrazione.NumeroProtocollo,
                                .DataImmissione = r.registrazione.DataImmissione,
                                .DataImmissioneTroncata = r.registrazione.DataImmissione.Value.Date,
                                .DataAnnullamentoTroncata = If(r.registrazione.DataOraAnnullamento.HasValue, r.registrazione.DataOraAnnullamento.Value.Date, Nothing),
                                .DescrizioneTipologiaRegistristrazione = r.tipoReg.Descrizione,
                                .ElencoReferentiInterni = r.registrazione.ElencoReferentiInterni,
                                .ElencoReferentiEsterni = r.registrazione.ElencoReferentiEsterni,
                                .UtenteUsername = "UTENTE: " & vbCrLf & r.registrazione.UtenteUsername,
                                .Note = "NOTE: " & vbCrLf & r.registrazione.Note,
                                .DescrizioneTipoRicezioneInvio = If(r.tpRic Is Nothing, "TIPO RICEZIONE:", "TIPO RICEZIONE:" & vbCrLf & r.tpRic.Descrizione),
                                .DescrizioneTipologiaDocumento = If(r.tpDoc Is Nothing, "TIPO DOCUMENTO:", "TIPO DOCUMENTO:" & vbCrLf & r.tpDoc.Descrizione),
                                .dataProtocolloMittente = r.registrazione.DataDocumento,
                                .regProtocolloMittente = r.registrazione.ProtocolloMittente,
                                .listaAllegati = GetElencoAllegati(r.registrazione.Id),
                                .Annullata = IIf(r.registrazione.DataOraAnnullamento.HasValue, "ANNULLATA IL:" & vbCrLf & r.registrazione.DataOraAnnullamento.ToString, ""),
                                .Classificazione = If(r.registrazione.IdClassificazione Is Nothing, "CLASSIFICAZIONE:", "CLASSIFICAZIONE:" & vbCrLf & GetClassificazione(r.registrazione.IdClassificazione))
                            })

        Return a.ToList

    End Function

    'Trova le date delle registrazioni Nodificate e ritorna una stringa contenente le date inerenti
    Private Function lanciaReportModificati() As String

        Dim elencoDateTrovate = ""

        Dim dataIniziale As Date = New Date(Me.DataProtocolloInizioTextBox.SelectedDate.Value.Year, Me.DataProtocolloInizioTextBox.SelectedDate.Value.Month, Me.DataProtocolloInizioTextBox.SelectedDate.Value.Day, 0, 0, 0)
        Dim dataFinale As Date = New Date(Me.DataProtocolloFineTextBox.SelectedDate.Value.Year, Me.DataProtocolloFineTextBox.SelectedDate.Value.Month, Me.DataProtocolloFineTextBox.SelectedDate.Value.Day, 23, 59, 59)

        Dim numeroGiorni = DateDiff(DateInterval.Day, dataIniziale, dataFinale)

        Dim recordModificati = queryModificate().Where(Function(w) w.DataRegistrazioneTroncata <> w.DataImmissioneTroncata)


        Dim listaIdRegistrazioniModificate = getListaIdRegistrazioniPerTipologia(2)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim listaModificateRagguppate = recordModificati.Where(Function(w) Not listaIdRegistrazioniModificate.Contains(w.IdRegistrazione)).GroupBy(Function(g) g.DataImmissioneTroncata)

        For Each elemento In listaModificateRagguppate

            Dim registroGiornalieroRep As New ParsecPro.RegistroProtocolloRepository

            Dim dataCorrente = elemento.FirstOrDefault.DataImmissioneTroncata

            elencoDateTrovate = elencoDateTrovate + dataCorrente.ToShortDateString + vbCrLf

            Dim r As ParsecReporting.StampaElencoRegistrazioniReport = ParsecReporting.ReportManager.CreaStampaElencoRegistrazioni(elemento.ToList)

            Dim fileName As String = ""
            Dim result As Telerik.Reporting.Processing.RenderingResult = Nothing

            r.Titolo = "Registro dei Protocolli Modificati del " & dataCorrente.ToShortDateString
            r.RagioneSociale = Me.Cliente.Descrizione
            Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
            result = reportProcessor.RenderReport("PDF", r, Nothing)

            'vedo se sono stati generati altri registri per quella data
            Dim filtroRegistro As New ParsecPro.RegistroProtocolloFiltro
            filtroRegistro.tipologia = 2
            filtroRegistro.DataInizio = dataCorrente
            filtroRegistro.DataFine = dataCorrente


            Dim ultimoRegistroAnnullatoGenerato = registroGiornalieroRep.GetView(filtroRegistro).OrderByDescending(Function(o) o.id).FirstOrDefault
            Dim indiceRegistroAnnullatoGenerato As Integer = 1
            If (Not ultimoRegistroAnnullatoGenerato Is Nothing) Then
                Dim nomeFile = IO.Path.GetFileNameWithoutExtension(ultimoRegistroAnnullatoGenerato.fileNameOriginale)
                indiceRegistroAnnullatoGenerato = CInt(nomeFile.Substring(nomeFile.LastIndexOf("_") + 1)) + 1
            End If
            Dim mese As String = IIf(dataCorrente.Month > 9, dataCorrente.Month, "0" & dataCorrente.Month)
            Dim giorno As String = IIf(dataCorrente.Day > 9, dataCorrente.Day, "0" & dataCorrente.Day)
            fileName = "RegistroProtocolloModificati_" + dataCorrente.Year.ToString + mese + giorno + "_V_" + indiceRegistroAnnullatoGenerato.ToString + ".pdf"

            Dim pathRootProtocollo As String = ParsecAdmin.WebConfigSettings.GetKey("PathProtocollo") & dataFinale.Year.ToString + "\"

            If (Not IO.Directory.Exists(pathRootProtocollo)) Then
                IO.Directory.CreateDirectory(pathRootProtocollo)
            End If

            IO.File.WriteAllBytes(pathRootProtocollo & fileName, result.DocumentBytes)

            Dim registroGiornaliero As New ParsecPro.RegistroProtocollo
            registroGiornaliero.path = "\" & dataFinale.Year
            registroGiornaliero.tipologia = 2
            registroGiornaliero.dataRegistro = dataCorrente
            registroGiornaliero.fileNameOriginale = fileName
            registroGiornaliero.conservato = False
            registroGiornaliero.idUtente = utenteCollegato.Id
            registroGiornaliero.dataCreazioneRegistro = Now
            registroGiornalieroRep.Save(registroGiornaliero)

            registroGiornalieroRep.Dispose()

            'dettaglio protocolli annullati
            Dim dataAnnIniziale As Date = New Date(dataCorrente.Year, dataCorrente.Month, dataCorrente.Day, 0, 0, 0)
            Dim dataAnnFinale As Date = New Date(dataCorrente.Year, dataCorrente.Month, dataCorrente.Day, 23, 59, 59)
            Dim listaAnnullatiCorrenti = recordModificati.Where(Function(w) w.DataImmissione >= dataAnnIniziale And w.DataImmissione <= dataAnnFinale).ToList
            If (listaAnnullatiCorrenti.Count > 0) Then
                For Each protocolloModificato In listaAnnullatiCorrenti
                    Dim dettaglioRegistroProtocollorepository As New ParsecPro.DettaglioRegistroProtocolloRepository
                    Dim dettaglioRegistroProtocollo As New ParsecPro.DettaglioRegistroProtocollo
                    dettaglioRegistroProtocollo.IdRegistroProtocollo = registroGiornaliero.id
                    dettaglioRegistroProtocollo.IdRegistrazione = protocolloModificato.IdRegistrazione
                    dettaglioRegistroProtocollorepository.Add(dettaglioRegistroProtocollo)
                    dettaglioRegistroProtocollorepository.SaveChanges()
                Next
            End If
            'fine dettaglio
        Next


        Return elencoDateTrovate

    End Function

    'lanciata da lanciaReportModificati(). Restiruisce le registrazioni modfiicate.
    Private Function queryModificate() As List(Of Registrazione)

        Dim dataInizioModificata = New Date(Me.DataProtocolloInizioTextBox.SelectedDate.Value.Year, Me.DataProtocolloInizioTextBox.SelectedDate.Value.Month, Me.DataProtocolloInizioTextBox.SelectedDate.Value.Day, 0, 0, 0)
        Dim dataFineModificata = New Date(Me.DataProtocolloFineTextBox.SelectedDate.Value.Year, Me.DataProtocolloFineTextBox.SelectedDate.Value.Month, Me.DataProtocolloFineTextBox.SelectedDate.Value.Day, 23, 59, 59)

        Dim registrazioneProtocollo As New ParsecPro.RegistrazioniRepository()
        Dim registro = registrazioneProtocollo.GetQuery()

        Dim tipologieRegistrazione = (New ParsecPro.TipologiaRegistrazioneRepository(registrazioneProtocollo.Context)).GetQuery

        Dim tipoRicezione = (New ParsecPro.TipiRicezioneInvioRepository(registrazioneProtocollo.Context)).GetQuery
        Dim tipoDocumento = (New ParsecPro.TipiDocumentoRepository(registrazioneProtocollo.Context)).GetQuery

        Dim res = (From registrazione In registro
                  Join tipoReg In tipologieRegistrazione On registrazione.TipoRegistrazione Equals tipoReg.Id
                  Group Join tpDoc In tipoDocumento On tpDoc.Id Equals registrazione.IdTipoDocumento Into elenco1 = Group From tpDoc In elenco1.DefaultIfEmpty()
                  Group Join tpRic In tipoRicezione On tpRic.Id Equals registrazione.IdTipoRicezione Into elenco2 = Group From tpRic In elenco2.DefaultIfEmpty()
                  Where (registrazione.Modificato = False And registrazione.DataOraRegistrazione >= dataInizioModificata And registrazione.DataOraRegistrazione <= dataFineModificata)
                  Order By registrazione.DataImmissione
                  Select New With {registrazione, tipoReg, tpDoc, tpRic})

        Dim a = (From r In res.AsEnumerable Select New Registrazione With {
                                .IdRegistrazione = r.registrazione.Id,
                                .Oggetto = r.registrazione.Oggetto,
                                .NumeroProtocollo = r.registrazione.NumeroProtocollo,
                                .DataImmissione = r.registrazione.DataImmissione,
                                .DataImmissioneTroncata = r.registrazione.DataImmissione.Value.Date,
                                .DataAnnullamentoTroncata = If(r.registrazione.DataOraAnnullamento.HasValue, r.registrazione.DataOraAnnullamento.Value.Date, Nothing),
                                .DataRegistrazioneTroncata = r.registrazione.DataOraRegistrazione.Value.Date,
                                .DescrizioneTipologiaRegistristrazione = r.tipoReg.Descrizione,
                                .ElencoReferentiInterni = r.registrazione.ElencoReferentiInterni,
                                .ElencoReferentiEsterni = r.registrazione.ElencoReferentiEsterni,
                                .UtenteUsername = "UTENTE: " & vbCrLf & r.registrazione.UtenteUsername,
                                .Note = "NOTE: " & vbCrLf & r.registrazione.Note,
                                .DescrizioneTipoRicezioneInvio = If(r.tpRic Is Nothing, "TIPO RICEZIONE:", "TIPO RICEZIONE:" & vbCrLf & r.tpRic.Descrizione),
                                .DescrizioneTipologiaDocumento = If(r.tpDoc Is Nothing, "TIPO DOCUMENTO:", "TIPO DOCUMENTO:" & vbCrLf & r.tpDoc.Descrizione),
                                .dataProtocolloMittente = r.registrazione.DataDocumento,
                                .regProtocolloMittente = r.registrazione.ProtocolloMittente,
                                .Annullata = "",
                                .listaAllegati = GetElencoAllegati(r.registrazione.Id),
        .Classificazione = If(r.registrazione.IdClassificazione Is Nothing, "CLASSIFICAZIONE:", "CLASSIFICAZIONE:" & vbCrLf & GetClassificazione(r.registrazione.IdClassificazione))
                            })

        Return a.ToList

    End Function

    'Restituisce la lista degli Id delle registrazioni in base al parametro Tipologia (1=Annullate; 2= Modificate)
    Public Function getListaIdRegistrazioniPerTipologia(ByVal tipologia As Integer) As IQueryable(Of Integer)
        Dim dettaglioProto As New ParsecPro.DettaglioRegistroProtocolloRepository
        Dim registroProtocollo = (New ParsecPro.RegistroProtocolloRepository(dettaglioProto.Context)).GetQuery

        Dim listaIdRegistrazioniAnnullate = (From dettaglio In dettaglioProto.GetQuery
                  Join registro In registroProtocollo On dettaglio.IdRegistroProtocollo Equals registro.id
                  Where (registro.tipologia = tipologia)
                  Select dettaglio.IdRegistrazione)

        Return listaIdRegistrazioniAnnullate
    End Function

    'Trova gli allegati per la registrazione passata come parametro e costruisce una stringa contenente progressivo - Nomefile - Impronta Esadecimale
    Private Function GetElencoAllegati(ByVal idRregistrazione As Integer) As String
        Dim sb As New StringBuilder
        Dim allegati = (New ParsecPro.AllegatiRepository).GetAllegatiProtocollo(idRregistrazione)
        If (allegati.Count > 0) Then
            Dim contaAllegato As Integer = 1
            sb.Append("ALLEGATI:" & vbCrLf)
            For Each item In allegati
                sb.Append(contaAllegato.ToString & ") " & item.NomeFile & " (" & item.ImprontaEsadecimale & ") " & vbCrLf)
                contaAllegato = contaAllegato + 1
            Next
            Dim s = sb.ToString
            Return s
        Else
            Return ""
        End If

    End Function

    'Restituisce la descrizione della Voce di Classificazione (la prende dal Titolario)
    Private Function GetClassificazione(idClassificazione As Nullable(Of Integer)) As String
        Dim classificazioneCompleta As String = ""
        If idClassificazione.HasValue Then
            Dim classificazioni As New ParsecAdmin.TitolarioClassificazioneRepository
            Dim classificazione As ParsecAdmin.TitolarioClassificazione = classificazioni.GetQuery.Where(Function(c) c.Id = idClassificazione).FirstOrDefault
            classificazioneCompleta = classificazioni.GetCodiciClassificazione2(classificazione.Id, 1) & " " & classificazione.Descrizione
            classificazioni.Dispose()
        End If
        Return classificazioneCompleta
    End Function

    'Effettua la Ricerca: costruisce prima il filtro in base ai dati della maschera.
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Dim filtro As ParsecPro.RegistroProtocolloFiltro = getFiltroRegistro()
        Dim registroProtocolloRep As New ParsecPro.RegistroProtocolloRepository
        Me.ListaRegistriProtocollo = registroProtocolloRep.GetView(filtro)
        registroProtocolloRep.Dispose()
        Me.grigliaRegistroProtocollo.Rebind()
    End Sub

    'Riroistina le condizioni iniziali della maschera. Resetta i campi.
    Protected Sub RipristinaFiltroInizialeImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles RipristinaFiltroInizialeImageButton.Click
        Dim registroProtocolloRep As New ParsecPro.RegistroProtocolloRepository
        Me.ListaRegistriProtocollo = New List(Of ParsecPro.RegistroProtocollo)
        Me.grigliaRegistroProtocollo.Rebind()
        registroProtocolloRep.Dispose()
        Me.settaCampiIniziali()
    End Sub

#End Region

End Class