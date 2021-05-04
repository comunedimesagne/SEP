Imports ParsecAdmin
Imports ParsecPro
Imports Telerik.Web.UI
Imports System.IO
Imports System.Web.Mail
Imports System.Data
Imports iTextSharp.text.pdf
Imports iTextSharp.text

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class AcquisizioneAutomaticaAllegatPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage
    Private message As New StringBuilder


#Region "PROPRIETA'"

    'Variabile di Sessione: rappresenta la lista degli allegati scansionati.
    Public Property AllegatiScansionati() As List(Of ParsecPro.AllegatoScansionato)
        Get
            Return CType(Session("AcquisizioneAutomaticaAllegatPage_AllegatiScansionati"), List(Of ParsecPro.AllegatoScansionato))
        End Get
        Set(ByVal value As List(Of ParsecPro.AllegatoScansionato))
            Session("AcquisizioneAutomaticaAllegatPage_AllegatiScansionati") = value
        End Set
    End Property

    'Variabile di Sessione: rappresentanta gli elementi selezioanti nella griglia.
    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("AcquisizioneAutomaticaAllegatPage_SelectedItems") Is Nothing Then
                Session("AcquisizioneAutomaticaAllegatPage_SelectedItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("AcquisizioneAutomaticaAllegatPage_SelectedItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("AcquisizioneAutomaticaAllegatPage_SelectedItems") = value
        End Set
    End Property

    'Variabile di Sessione: elenco degli Allegati che non sono stati riconosciuti.
    Public Property AllegatiScansionatiNonRiconosciuti() As List(Of ParsecPro.AllegatoScansionato)
        Get
            Return CType(Session("AcquisizioneAutomaticaAllegatPage_AllegatiScansionatiNonRiconosciuti"), List(Of ParsecPro.AllegatoScansionato))
        End Get
        Set(ByVal value As List(Of ParsecPro.AllegatoScansionato))
            Session("AcquisizioneAutomaticaAllegatPage_AllegatiScansionatiNonRiconosciuti") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> Allega Documenti Scansionati"
        If Not Me.IsPostBack Then

            Me.AllegatiScansionatiNonRiconosciuti = New List(Of ParsecPro.AllegatoScansionato)

            Me.SelectedItems = Nothing

            Dim anno As Integer = Me.GetAnnoEsercizio()

            Me.AnnoProtocolloTextBox.Text = anno
            Me.AnnoProtocolloErroreTextBox.Text = anno

            Me.DataAcquisizioneInizioTextBox.SelectedDate = New Date(anno, 1, 1)
            Me.DataAcquisizioneFineTextBox.SelectedDate = New Date(anno, 12, 31)
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraScansione()
        End If

        Me.ScansionaCodiceBarreRadioButton.Attributes.Add("onclick", Me.GetClientScript)

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.ErroriPanel.Style.Add("width", widthStyle)
        Me.GeneralePanel.Style.Add("width", widthStyle)
        Me.AllegatiScansionatiGridView.Style.Add("width", widthStyle)
        Me.AllegatiScansionatiErroreGridView.Style.Add("width", widthStyle)
    
    End Sub

    'Evento LoadComplete della Pagina: dopo che la pagina è stata caricata abilita o disabilita alcuni eventi.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ScansioniLabel.Text = "Scansioni associate&nbsp;&nbsp;&nbsp;" & If(Me.AllegatiScansionatiGridView.MasterTableView.Items.Count > 0, "( " & Me.AllegatiScansionatiGridView.MasterTableView.Items.Count.ToString & " )", "")
        Me.ScansioniErroreLabel.Text = "Scansioni non associate&nbsp;&nbsp;&nbsp;" & If(Me.AllegatiScansionatiNonRiconosciuti.Count > 0, "( " & Me.AllegatiScansionatiNonRiconosciuti.Count.ToString & " )", "")

        Me.SalvaAllegatiSelezionatiImageButton.Attributes.Remove("onclick")
        Me.EliminaAllegatiSelezionatiImageButton.Attributes.Remove("onclick")
        Dim message As String = "Eliminare tutti gli elementi selezionati?"

        If Me.AllegatiScansionatiGridView.SelectedItems.Count > 0 Then
            Me.EliminaAllegatiSelezionatiImageButton.Attributes.Add("onclick", "return confirm(""" & message & """)")
        Else
            Dim messageNoSelezione As String = "E' necessario selezionare almeno un allegato!"
            Me.SalvaAllegatiSelezionatiImageButton.Attributes.Add("onclick", "alert(""" & messageNoSelezione & """); return false;")
            Me.EliminaAllegatiSelezionatiImageButton.Attributes.Add("onclick", "alert(""" & messageNoSelezione & """); return false;")
        End If

        If Me.AllegatiScansionatiNonRiconosciuti.Count > 0 Then
            Me.EliminaAllegatiSelezionatiErroreImageButton.Attributes.Add("onclick", "return ConfirmDeleteScansioni(""" & message & """)")
        End If

        If Not ViewState("invalidControlId") Is Nothing Then
            Me.FindControlRecursive(Me.Page, ViewState("invalidControlId")).Focus()
            ViewState("invalidControlId") = Nothing
        End If
    End Sub

    'Evento PreRender della Pagina: prima che la pagina venga renderizzata disabilita lo scroll.
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanelErrore, Me.scrollPosErroreHidden, False)
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Recupera dal DB gli Allegati e riempie la variabile di Sessione
    'Richiamato da AllegatiScansionatiGridView.NeedDataSource
    Private Function GetAllegatiScansionati() As List(Of ParsecPro.AllegatoScansionato)
        Dim allegati As New ParsecPro.AllegatiScansionatiRepository
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        Dim filtro As New ParsecPro.AllegatoScansionatoFiltro With
          {
              .UtenteCollegato = utente,
              .DataAcquisizioneInizio = Me.DataAcquisizioneInizioTextBox.SelectedDate,
              .DataAcquisizioneFine = Me.DataAcquisizioneFineTextBox.SelectedDate
          }

        Dim res As List(Of ParsecPro.AllegatoScansionato) = allegati.GetView(filtro)
        allegati.Dispose()
        Return res
    End Function

    'Evento ItemCommand associato alla griglia AllegatiScansionatiGridView. Fa partire l'esecuzione dei comandi definiti nella griglia.
    Protected Sub AllegatiScansionatiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiScansionatiGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaAllegatoScansionato(e.Item)
        End If
        If e.CommandName = "Sort" Then
            Me.scrollPosHidden.Value = "0"
        End If
        If e.CommandName = "Preview" Then
            Me.DownloadFile(e.Item)
        End If
        If e.CommandName = "Save" Then
            Me.SalvaAllegatoScansionato(e.Item)
        End If
    End Sub

    'Evento NeedDataSource associato alla griglia AllegatiScansionatiGridView. Aggancia il datasource della griglia al DB e aggiorna la lista variabile di sessione AllegatiScansionati.
    Protected Sub AllegatiScansionatiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles AllegatiScansionatiGridView.NeedDataSource
        If Me.AllegatiScansionati Is Nothing Then
            Me.AllegatiScansionati = Me.GetAllegatiScansionati
        End If
        Me.AllegatiScansionatiGridView.DataSource = Me.AllegatiScansionati
    End Sub

    'Elimina l'Allegato e aggiorna la griglia. Istanziato da AllegatiScansionatiGridView.ItemCommand
    Private Sub EliminaAllegatoScansionato(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim allegati As New ParsecPro.AllegatiScansionatiRepository
        Dim allegato As ParsecPro.AllegatoScansionato = allegati.GetById(id)
        If Not allegato Is Nothing Then
            Dim message As String = "L'allegato selezionato è stato cancellato con successo!"
            Try

                allegati.Delete(allegato.Id)

                Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.Nomefile

                If IO.File.Exists(pathDownload) Then
                    IO.File.Delete(pathDownload)
                End If

                '*********************************************************
                'Aggiorno la griglia
                '*********************************************************

                Me.AllegatiScansionati = Nothing
                Me.AllegatiScansionatiGridView.Rebind()
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Impossibile cancellare l'allegato selezionato, per il seguente errore:" & vbCrLf & ex.Message, False)
            Finally
                ParsecUtility.Utility.MessageBox(message, False)
            End Try
        End If
        allegati.Dispose()
    End Sub


    '*********************************************************************************************************
    'GESTIONE TIMBRATURA
    '*********************************************************************************************************
    'Aggiunge la timbratura sul PDF.
    Private Sub AddWatermarkToPdf(ByVal watermark As String, ByVal allegato As AllegatoScansionato)

        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.Nomefile
        Dim temp As String = "temp" & allegato.Nomefile
        Dim pathDownloadTemp As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & temp

        'SE IL FILE PDF E' PROTETTO O SE IL FORMATO NON VALIDO NON AGGIUNGO IL WATERMARK
        Try
            Dim reader As New PdfReader(pathDownload)

            Dim af As AcroFields = reader.AcroFields
            Dim names = af.GetSignatureNames()

            'Se non ci sono firme
            If names.Count = 0 Then
                Dim fs As New IO.FileStream(pathDownloadTemp, FileMode.Create, FileAccess.Write)
                Dim pdfStamper As New PdfStamper(reader, fs)

                For i As Integer = 1 To reader.NumberOfPages
                    Dim size As iTextSharp.text.Rectangle = reader.GetPageSizeWithRotation(i)
                    Dim cb As PdfContentByte = pdfStamper.GetOverContent(i)
                    cb.SaveState()
                    Dim bf As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
                    cb.SetColorFill(BaseColor.BLACK)
                    cb.SetFontAndSize(bf, 12)
                    cb.BeginText()
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermark, size.Width - 15, size.Height / 2, -90)
                    cb.EndText()
                    cb.RestoreState()
                Next

                pdfStamper.Close()
                fs.Close()
                reader.Close()

                IO.File.Copy(pathDownloadTemp, pathDownload, True)
                IO.File.Delete(pathDownloadTemp)
            End If
        Catch ex As BadPasswordException
        Catch ex As BadPdfFormatException
        End Try
    End Sub

    'Classe di Appoggio contenente delle informazioni sugli Allegati
    Private Class AllegatoInfo
        Public Property Allegato As ParsecPro.Allegato
        Public Property IdRegistrazione As Integer
        Public Property NumeroAllegati As Integer

    End Class

    'Evento Click di SalvaAllegatiSelezionatiImageButton. Salva gli allegati
    Protected Sub SalvaAllegatiSelezionatiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SalvaAllegatiSelezionatiImageButton.Click

        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazioniBloccate As New ParsecPro.LockRegistrazioneRepository
        Dim allegatiScansionati As New ParsecPro.AllegatiScansionatiRepository


        Dim protocolli As New System.Collections.Generic.Dictionary(Of Integer, ParsecPro.Registrazione)

        Dim allegatiInfos As New List(Of AllegatoInfo)

      
        Dim messages As New List(Of String)

        Dim allegatiScansionatiToDelete As New System.Collections.Generic.Dictionary(Of Integer, Integer)

        Dim pathDownload As String = String.Empty

        Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
        Dim watermark As String = String.Empty

        Dim id As Integer = 0
        Dim allegatoScansionato As ParsecPro.AllegatoScansionato = Nothing
        Dim registrazione As ParsecPro.Registrazione = Nothing
        Dim idRegistrazione As Integer = 0
        Dim allegato As ParsecPro.Allegato = Nothing
        Dim utenteLock As ParsecAdmin.Utente = Nothing
        Dim cnt As Integer = 0

        For Each item As GridDataItem In Me.AllegatiScansionatiGridView.SelectedItems

            id = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            allegatoScansionato = allegatiScansionati.GetById(id)

            If Not allegatoScansionato Is Nothing Then

                If allegatoScansionato.NumeroProtocollo.HasValue AndAlso allegatoScansionato.AnnoProtocollo.HasValue Then

                    registrazione = registrazioni.GetRegistrazione(allegatoScansionato.NumeroProtocollo, allegatoScansionato.AnnoProtocollo, allegatoScansionato.TipoRegistrazione)

                    If Not registrazione Is Nothing Then


                        idRegistrazione = registrazione.Id

                        If registrazione.IdDocumentoWS.HasValue Then
                            messages.Add(String.Format("La registrazione n. {0}/{1} non si può modificare perchè è stata inviata al modulo Archivio!", allegatoScansionato.NumeroProtocollo, allegatoScansionato.AnnoProtocollo))
                            Continue For
                        End If

                        pathDownload = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegatoScansionato.Nomefile
                        If Not IO.File.Exists(pathDownload) Then
                            messages.Add(String.Format("File '{0}' non esiste", allegatoScansionato.Nomefile))
                            Continue For
                        End If

                        Dim registrazioneBloccata As ParsecPro.LockRegistrazione = registrazioniBloccate.GetLockRegistrazione(registrazione)
                        If Not registrazioneBloccata Is Nothing Then
                            'Se la registrazione non è bloccata dall'utente collegato.
                            If registrazioneBloccata.IdUtente <> utenteCollegato.Id Then

                                Dim utenti As New ParsecAdmin.UserRepository
                                utenteLock = utenti.Where(Function(c) c.Id = registrazioneBloccata.IdUtente).FirstOrDefault
                                utenti.Dispose()
                                messages.Add(String.Format("La registrazione n. {0}/{1} non si può modificare perchè è BLOCCATA da {2}!", allegatoScansionato.NumeroProtocollo, allegatoScansionato.AnnoProtocollo, utenteLock.Username))
                                Continue For
                            Else
                                'Sblocco la registrazione.
                                registrazioniBloccate.Delete(registrazioneBloccata.Id)
                            End If
                        End If


                        If Not protocolli.ContainsKey(idRegistrazione) Then
                            protocolli.Add(idRegistrazione, registrazione)
                        End If


                        If Not allegatiScansionatiToDelete.ContainsKey(allegatoScansionato.Id) Then
                            allegatiScansionatiToDelete.Add(allegatoScansionato.Id, idRegistrazione)
                        End If


                        '**************************************************************************************************************************************
                        'GESTIONE TIMBRATURA
                        '**************************************************************************************************************************************

                        watermark = cliente.Descrizione & " - Cod. Amm. " & cliente.CodiceAmministrazione & " - Prot. n. " & registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy HH:mm}", registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(registrazione.TipoRegistrazione).ToUpper

                        Me.AddWatermarkToPdf(watermark, allegatoScansionato)
                        '**************************************************************************************************************************************


                        allegato = New ParsecPro.Allegato
                        cnt = allegatiInfos.Where(Function(c) c.IdRegistrazione = idRegistrazione).Count + registrazione.Allegati.Count
                        allegato.Oggetto = "Allegato n. " & (cnt + 1).ToString
                        allegato.IdTipologiaDocumento = 1 'Primario
                        If cnt > 0 Then
                            allegato.IdTipologiaDocumento = 0 'Secondario
                        End If
                        allegato.NomeFile = allegatoScansionato.Nomefile
                        allegato.NomeFileTemp = allegatoScansionato.Nomefile
                        allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                        allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
                        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")


                        '*********************************************************
                        'Aggiungo l'allegato alla registrazione corrispondente.
                        '*********************************************************

                        allegatiInfos.Add(New AllegatoInfo With {.IdRegistrazione = idRegistrazione, .Allegato = allegato})

                    Else
                        messages.Add(String.Format("La registrazione n. {0}/{1} non esiste!", allegatoScansionato.NumeroProtocollo, allegatoScansionato.AnnoProtocollo))
                    End If
                End If
            End If
        Next


        Dim idProtocollo As Integer = 0
        Dim allegati As List(Of ParsecPro.Allegato) = Nothing
        For Each registrazione In protocolli.Values
            Try
                idProtocollo = registrazione.Id
                allegati = allegatiInfos.Where(Function(c) c.IdRegistrazione = idProtocollo).Select(Function(c) c.Allegato).ToList

                registrazione.Allegati.AddRange(allegati)
                '*********************************************************
                'Salvo gli allegati nella registrazione corrispondente.
                '*********************************************************
                registrazioni.SaveAllegati(registrazione)


                Dim reg = registrazioni.Where(Function(c) c.Id = idProtocollo).FirstOrDefault
                reg.NumeroAllegati = registrazione.Allegati.Count

                If Not reg.TipologiaAllegatoPrimario.HasValue Then
                    reg.TipologiaAllegatoPrimario = ParsecPro.TipologiaAllegatoPrimario.Generico
                End If

                registrazioni.SaveChanges()



            Catch ex As Exception
                If ex.InnerException Is Nothing Then
                    messages.Add(String.Format("Impossibile salvare gli allegati della registrazione n. {0}/{1}, per il seguente errore:{2}", registrazione.NumeroProtocollo, Year(registrazione.DataImmissione), vbCrLf & ex.Message))
                Else
                    messages.Add(String.Format("Impossibile salvare gli allegati della registrazione n. {0}/{1}, per il seguente errore:{2}", registrazione.NumeroProtocollo, Year(registrazione.DataImmissione), vbCrLf & ex.InnerException.Message))
                End If

            Finally

                '*********************************************************
                'Elimino gli allegati temporanei.
                '*********************************************************
                For Each item As KeyValuePair(Of Integer, Integer) In allegatiScansionatiToDelete
                    If item.Value = registrazione.Id Then
                        allegatiScansionati.Delete(item.Key)
                    End If
                Next
            End Try
        Next


        '*********************************************************
        'Libero le risorse.
        '*********************************************************
        registrazioni.Dispose()
        allegatiScansionati.Dispose()
        registrazioniBloccate.Dispose()

        '*********************************************************
        'Visualizzo il messaggio di errore.
        '*********************************************************

        If messages.Count > 0 Then
            messages = messages.Distinct.ToList
            Me.message.Append("Problemi riscontrati:" & vbCrLf & vbCrLf)
            For Each mess In messages
                Me.message.Append(mess)
            Next

            ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
            Me.message.Clear()
        Else
            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
        End If
       
        '*********************************************************
        'Aggiorno la griglia.
        '*********************************************************
        Me.AllegatiScansionati = Nothing
        Me.AllegatiScansionatiGridView.Rebind()


    End Sub

    'Salva l'Allegato Scansionato. Richiamato da AllegatiScansionatiGridView.ItemCommand.
    Private Sub SalvaAllegatoScansionato(ByVal item As Telerik.Web.UI.GridDataItem)
        Try

            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim allegatiScansionati As New ParsecPro.AllegatiScansionatiRepository

            Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim allegatoScansionato As ParsecPro.AllegatoScansionato = allegatiScansionati.GetById(id)

            If Not allegatoScansionato Is Nothing Then

                If allegatoScansionato.NumeroProtocollo.HasValue AndAlso allegatoScansionato.AnnoProtocollo.HasValue Then

                    Dim registrazione As ParsecPro.Registrazione = registrazioni.GetRegistrazione(allegatoScansionato.NumeroProtocollo, allegatoScansionato.AnnoProtocollo, allegatoScansionato.TipoRegistrazione)

                    If Not registrazione Is Nothing Then

                        If registrazione.IdDocumentoWS.HasValue Then
                            ParsecUtility.Utility.MessageBox(String.Format("La registrazione n. {0}/{1} non si può modificare perchè è stata inviata al modulo Archivio!", allegatoScansionato.NumeroProtocollo, allegatoScansionato.AnnoProtocollo), False)
                            Exit Sub
                        End If

                        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegatoScansionato.Nomefile


                        If IO.File.Exists(pathDownload) Then
                            Dim registrazioniBloccate As New ParsecPro.LockRegistrazioneRepository

                            Dim registrazioneBloccata As ParsecPro.LockRegistrazione = registrazioniBloccate.GetLockRegistrazione(registrazione)
                            If Not registrazioneBloccata Is Nothing Then
                                'Se la registrazione non è bloccata dall'utente collegato.
                                If registrazioneBloccata.IdUtente <> utenteCollegato.Id Then
                                    Dim utenteLock As ParsecAdmin.Utente = (New ParsecAdmin.UserRepository).GetQuery.Where(Function(c) c.Id = registrazioneBloccata.IdUtente).FirstOrDefault
                                    ParsecUtility.Utility.MessageBox(String.Format("La registrazione n. {0}/{1} non si può modificare perchè è BLOCCATA da {2}!", registrazione.NumeroProtocollo, registrazione.DataImmissione.Value.Year, utenteLock.Username), False)
                                    Exit Sub
                                Else
                                    'Sblocco la registrazione.
                                    registrazioniBloccate.Delete(registrazioneBloccata.Id)
                                End If
                            End If

                            registrazioniBloccate.Dispose()

                            '**************************************************************************************************************************************
                            'GESTIONE TIMBRATURA
                            '**************************************************************************************************************************************

                            Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                            Dim watermark As String = cliente.Descrizione & " - Cod. Amm. " & cliente.CodiceAmministrazione & " - Prot. n. " & registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy HH:mm}", registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(registrazione.TipoRegistrazione).ToUpper


                            Me.AddWatermarkToPdf(watermark, allegatoScansionato)
                            '**************************************************************************************************************************************

                            Dim allegato As New ParsecPro.Allegato
                            allegato.Oggetto = "Allegato n. " & (registrazione.Allegati.Count + 1).ToString
                            allegato.IdTipologiaDocumento = 1 'Primario
                            If registrazione.Allegati.Count > 0 Then
                                allegato.IdTipologiaDocumento = 0 'Secondario
                            End If
                            allegato.NomeFile = allegatoScansionato.Nomefile
                            allegato.NomeFileTemp = allegatoScansionato.Nomefile
                            allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                            allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
                            allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                            allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

                            '*********************************************************
                            'Aggiungo l'allegato alla registrazione corrispondente.
                            '*********************************************************
                            registrazione.Allegati.Add(allegato)

                            Try

                                '*********************************************************
                                'Salvo l'allegato nella registrazione corrispondente.
                                '*********************************************************
                                registrazioni.SaveAllegati(registrazione)

                                Dim reg = registrazioni.Where(Function(c) c.Id = registrazione.Id).FirstOrDefault

                                reg.NumeroAllegati = registrazione.Allegati.Count
                                If Not reg.TipologiaAllegatoPrimario.HasValue Then
                                    reg.TipologiaAllegatoPrimario = ParsecPro.TipologiaAllegatoPrimario.Generico
                                End If

                                registrazioni.SaveChanges()

                            Catch ex As Exception
                                If ex.InnerException Is Nothing Then
                                    Me.message.AppendLine(ex.Message)
                                Else
                                    Me.message.AppendLine(ex.InnerException.Message)
                                End If

                            Finally

                                '*********************************************************
                                'Elimino l'allegato temporaneo.
                                '*********************************************************
                                allegatiScansionati.Delete(allegatoScansionato.Id)


                                '*********************************************************
                                'Aggiorno la griglia.
                                '*********************************************************
                                Me.AllegatiScansionati = Nothing
                                Me.AllegatiScansionatiGridView.Rebind()


                                Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"

                            End Try

                        Else
                            Me.message.AppendLine("File '" & allegatoScansionato.Nomefile & "' non trovato!")
                        End If

                        '*********************************************************
                        'Visualizzo il messaggio di errore.
                        '*********************************************************
                        If Me.message.Length > 0 Then
                            Me.message.Insert(0, String.Format("Problemi riscontrati durante il salvataggio della registrazione n. {0}/{1}:", registrazione.NumeroProtocollo, Year(registrazione.DataImmissione)) & vbCrLf & vbCrLf)
                            ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
                            Me.message.Clear()
                        End If


                    Else
                        ParsecUtility.Utility.MessageBox(String.Format("La registrazione n. {0}/{1} non esiste!", allegatoScansionato.NumeroProtocollo, allegatoScansionato.AnnoProtocollo), False)
                    End If
                End If
            End If

            '*********************************************************
            'Libero le risorse.
            '*********************************************************
            registrazioni.Dispose()
            allegatiScansionati.Dispose()

        Catch ex As Exception
            If ex.InnerException Is Nothing Then
                ParsecUtility.Utility.MessageBox("Impossibile salvare l'allegato selezionato, per il seguente errore:" & vbCrLf & ex.Message, False)
            Else
                ParsecUtility.Utility.MessageBox("Impossibile salvare l'allegato selezionato, per il seguente errore:" & vbCrLf & ex.InnerException.Message, False)
            End If
        End Try

    End Sub

    'Effettua il download del file. Richiamato da AllegatiScansionatiGridView.ItemCommand 
    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim allegati As New ParsecPro.AllegatiScansionatiRepository
        Dim allegato As ParsecPro.AllegatoScansionato = allegati.GetById(id)
        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.Nomefile
        Dim file As New IO.FileInfo(pathDownload)
        If Not allegato Is Nothing Then
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        End If

        allegati.Dispose()
    End Sub

    'Evento associato alla griglia AllegatiScansionatiGridView
    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedItems()
    End Sub

    'Evento associato alla griglia AllegatiScansionatiGridView per la gestione dei Check
    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In AllegatiScansionatiGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
    End Sub

    'Evento ItemCreated associato alla grilia AllegatiScansionatiGridView. Setta lo stile se è di tipo GridHeaderItem e definisce il PreRender se è di tipo GridDataItem.
    Private Sub AllegatiScansionatiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles AllegatiScansionatiGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf AllegatiScansionatiGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop - 1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    'Crea una lista degli items selezionati nell griglia AllegatiScansionatiGridView. Questa lista contiene gli id degli allegati selezionati.
    Private Sub SaveSelectedItems()
        For Each item As GridItem In Me.AllegatiScansionatiGridView.Items
            If TypeOf item Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(item, GridDataItem)
                Dim id As Integer = CInt(dataItem("Id").Text)
                Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        If Not Me.SelectedItems.ContainsKey(id) Then
                            Me.SelectedItems.Add(id, True)
                        End If
                    Else
                        If Me.SelectedItems.ContainsKey(id) Then
                            Me.SelectedItems.Remove(id)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    'Evento ItemPreRender associato alla griglia AllegatiScansionatiGridView. Evento che scatta prima del Render. Presetta i check.
    Protected Sub AllegatiScansionatiGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    'Evento ItemDataBound associato alla griglia AllegatiScansionatiGridView. Defisce i tooltip, gli eventi in base al contenuto di ogni record.
    Protected Sub AllegatiScansionatiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AllegatiScansionatiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim allegato As ParsecPro.AllegatoScansionato = CType(e.Item.DataItem, ParsecPro.AllegatoScansionato)

            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina allegato scansionato"
                Dim message As String = "Eliminare l'elemento selezionato?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If

            If TypeOf dataItem("Save").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Save").Controls(0), ImageButton)
                btn.ToolTip = "Salva allegato scansionato"
            End If
            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Preview").Controls(0), ImageButton)
                btn.ToolTip = "Visualizza allegato scansionato"
            End If

            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)

            Dim id As String = allegato.Id

            If SelectedItems.ContainsKey(id) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(SelectedItems(id).ToString())
                dataItem.Selected = True
            End If

        End If
    End Sub


#End Region

#Region "AZIONI PANNELLO FILTRO"

    'Resetta i dati per la ricerca. Lanciato da AnnullaFiltroImageButton.Click
    Private Sub ResettaFiltro()
       Dim anno As Integer = Me.GetAnnoEsercizio()
       Me.DataAcquisizioneInizioTextBox.SelectedDate = New Date(anno, 1, 1)
        Me.DataAcquisizioneFineTextBox.SelectedDate = New Date(anno, 12, 31)
        Me.AllegatiScansionati = Nothing
        Me.AllegatiScansionatiGridView.Rebind()
    End Sub

    'Ricerca gli allegati. Metodo lanciato da FiltraImageButton.Click
    Private Sub FiltraAllegatiScansionati()
        Me.AllegatiScansionati = Me.GetAllegatiScansionati
        Me.AllegatiScansionatiGridView.Rebind()
    End Sub

    'Evento Click di FiltraImageButton. Effettua una ricerca
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FiltraImageButton.Click
        Me.FiltraAllegatiScansionati()
    End Sub

    'Resetta i campi della maschera. Resetta quindi il filtro.
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

    'Elimina gli allegati selezionati. 
    Protected Sub EliminaAllegatiSelezionatiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaAllegatiSelezionatiImageButton.Click

        Dim allegatiScansionati As New ParsecPro.AllegatiScansionatiRepository
        For Each item As GridDataItem In Me.AllegatiScansionatiGridView.SelectedItems
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim allegato As ParsecPro.AllegatoScansionato = allegatiScansionati.GetById(id)
            If Not allegato Is Nothing Then
                Dim message As String = "L'allegato '" & allegato.Nomefile & "' è stato cancellato con successo!"
                Try
                    allegatiScansionati.Delete(allegato.Id)
                    Me.message.AppendLine(message)

                    Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.Nomefile
                    If IO.File.Exists(pathDownload) Then
                        IO.File.Delete(pathDownload)
                    End If

                Catch ex As Exception
                    Me.message.AppendLine("Impossibile cancellare l'allegato '" & allegato.Nomefile & "', per il seguente errore:" & vbCrLf & ex.Message)
                End Try

            End If
        Next

        If Me.message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
            Me.message.Clear()
        End If

        '*********************************************************
        'Aggiorno la griglia
        '*********************************************************
        Me.AllegatiScansionati = Nothing
        Me.AllegatiScansionatiGridView.Rebind()

        allegatiScansionati.Dispose()
    End Sub


#End Region

#Region "METODI PRIVATI"

    'Metodo che ricerca ricorsivamente i controlli. Richiamto dal LoadComplete.
    Private Function FindControlRecursive(ByVal control As Control, ByVal id As String) As Control
        If control Is Nothing Then
            Return Nothing
        End If
        Dim ctrl As Control = control.FindControl(id)
        If ctrl Is Nothing Then
            For Each child As Control In control.Controls
                ctrl = Me.FindControlRecursive(child, id)
                If Not ctrl Is Nothing Then
                    Exit For
                End If
            Next
        End If
        Return ctrl
    End Function

    'Ritorna l'anno di esercio corrente.
    Private Function GetAnnoEsercizio() As Integer
        Dim esercizi As New ParsecPro.EsercizioRepository
        Dim esercizio As ParsecPro.Esercizio = esercizi.GetEsercizioCorrente
        esercizi.Dispose()
        Dim anno As Integer = Now.Year
        If Not esercizio Is Nothing Then
            anno = esercizio.Anno
        End If
        Return anno
    End Function

    'abilita o disabilita NumeroProtocolloTextBox e AnnoProtocolloTextBox
    Private Function GetClientScript() As String
        Dim script As New System.Text.StringBuilder
        script.Append("var txt1= $find('" & Me.NumeroProtocolloTextBox.ClientID & "');")
        script.Append("var txt2= $find('" & Me.AnnoProtocolloTextBox.ClientID & "');")
        script.Append("var chk= document.getElementById('" & Me.RegistrazioneInternaCheckBox.ClientID & "');")

        script.Append("if (this.checked) {")
        script.Append("     txt1.disable();")
        script.Append("     txt2.disable();")
        script.Append("     chk.disabled=true;")
        script.Append("}else{")
        script.Append("     txt1.enable();")
        script.Append("     txt2.enable();")
        script.Append("     chk.disabled=false;")
        script.Append("}")

        Return script.ToString
    End Function

    'Genera un nuovo id tempoaraneo per gli allegati temporanei aggiunti nella griglia
    Private Function GetNuovoIdAllegatoTemporaneo() As Integer
        Dim nuovoId As Integer = -1
        If Me.AllegatiScansionatiNonRiconosciuti.Count > 0 Then
            Dim allId = Me.AllegatiScansionatiNonRiconosciuti.Min(Function(c) c.Id) - 1
            If allId < 0 Then
                nuovoId = allId
            End If
        End If
        Return nuovoId
    End Function

    'Verifica che il protocollo con anno e numero di protocollo esista.
    Private Function VerificaProtocollo(ByVal numero As RadNumericTextBox, ByVal anno As RadNumericTextBox, ByVal tipo As CheckBox) As Boolean

        Dim numeroProtocollo As Nullable(Of Integer) = Me.VerificaCampo(numero, "Numero Protocollo")
        Dim annoProtocollo As Nullable(Of Integer) = Me.VerificaCampo(anno, "Anno Protocollo")
        If numeroProtocollo.HasValue AndAlso annoProtocollo.HasValue Then
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim tipologia As ParsecPro.TipoRegistrazione = TipoRegistrazione.Nessuna

            If tipo.Checked Then
                tipologia = TipoRegistrazione.Interna
            End If

            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetRegistrazione(numeroProtocollo.Value, annoProtocollo.Value, tipologia)

            'Se la registrazione esiste.
            If Not registrazione Is Nothing Then
                Return True
            Else
                Me.message.AppendLine(String.Format("La registrazione n. {0}/{1} non esiste!", numeroProtocollo.Value, annoProtocollo.Value))
            End If
        End If
        Return False
    End Function

    'Effettua una verifica sulla registrazione  con anno e numero di protocollo passati come argomenti.
    'Da il via libera eventuale sulla modificabilità o meno della registrazione
    Private Function VerificaRegistrazione(ByVal numero As RadNumericTextBox, ByVal anno As RadNumericTextBox, ByVal tipo As CheckBox) As ParsecPro.Registrazione
        Dim registrazione As ParsecPro.Registrazione = Nothing

        Dim numeroProtocollo As Nullable(Of Integer) = Me.VerificaCampo(numero, "Numero Protocollo")
        Dim annoProtocollo As Nullable(Of Integer) = Me.VerificaCampo(anno, "Anno Protocollo")

        If numeroProtocollo.HasValue AndAlso annoProtocollo.HasValue Then
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim tipologia As ParsecPro.TipoRegistrazione = TipoRegistrazione.Nessuna

            If tipo.Checked Then
                tipologia = TipoRegistrazione.Interna
            End If

            registrazione = registrazioni.GetRegistrazione(numeroProtocollo.Value, annoProtocollo.Value, tipologia)

            'Se la registrazione esiste.
            If registrazione Is Nothing Then
                Me.message.AppendLine(String.Format("La registrazione n. {0}/{1} non esiste!", numeroProtocollo.Value, annoProtocollo.Value))
            Else
                If registrazione.IdDocumentoWS.HasValue Then
                    Me.message.AppendLine(String.Format("La registrazione n. {0}/{1} non si può modificare perchè è stata inviata al modulo Archivio!", registrazione.NumeroProtocollo, registrazione.DataImmissione.Value.Year))
                    registrazione = Nothing
                Else
                    Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
                    Dim registrazioniBloccate As New ParsecPro.LockRegistrazioneRepository
                    Dim registrazioneBloccata As ParsecPro.LockRegistrazione = registrazioniBloccate.GetLockRegistrazione(registrazione)
                    If Not registrazioneBloccata Is Nothing Then
                        'Se la registrazione non è bloccata dall'utente collegato.
                        If registrazioneBloccata.IdUtente <> utenteCollegato.Id Then
                            Dim utenti As New ParsecAdmin.UserRepository
                            Dim utenteLock As ParsecAdmin.Utente = utenti.Where(Function(c) c.Id = registrazioneBloccata.IdUtente).FirstOrDefault
                            Me.message.AppendLine(String.Format("La registrazione n. {0}/{1} non si può modificare perchè è BLOCCATA da {2}!", registrazione.NumeroProtocollo, registrazione.DataImmissione.Value.Year, utenteLock.Username))
                            registrazione = Nothing
                            utenti.Dispose()
                        Else
                            'Sblocco la registrazione.
                            registrazioniBloccate.Delete(registrazioneBloccata.Id)
                        End If
                    End If
                    registrazioniBloccate.Dispose()
                End If
            End If
        End If
        Return registrazione
    End Function

    'Verifica che il campo passato come argomento sia valido
    Private Function VerificaCampo(ByVal campo As RadNumericTextBox, ByVal nomeCampo As String) As Nullable(Of Integer)
        Dim res As Nullable(Of Integer) = Nothing
        If Not campo.Value.HasValue Then
            Me.message.AppendLine(String.Format("Il campo " & "'{0}'" & " è obbligatorio!", nomeCampo))
            If ViewState("invalidControlId") Is Nothing Then
                ViewState("invalidControlId") = campo.ID
            End If
        Else
            res = CInt(campo.Text)
        End If
        Return res
    End Function

#End Region

#Region "AZIONI PANNELLO GRIGLIA"

    'Avvia la scansione del Documento.
    Protected Sub ScansionaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ScansionaImageButton.Click

        If Me.ScansionaCodiceBarreRadioButton.Checked Then
            Me.AvviaScansione()
            Exit Sub
        End If

        If Me.VerificaProtocollo(Me.NumeroProtocolloTextBox, Me.AnnoProtocolloTextBox, Me.RegistrazioneInternaCheckBox) Then
            Me.AvviaScansione()
        Else
            If Me.message.Length > 0 Then
                ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
                Me.message.Clear()
            End If
        End If
    End Sub

    'Evento click associato al controllo nascosto ScanUploadButton. Avvia la Notifica la scansione
    Protected Sub ScanUploadButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadButton.Click
        Me.NotificaScansione()
    End Sub

    'Registra lo Scanner
    Private Sub RegistraScansione()
        If TypeOf Me.MainPage Is MainPage Then
            Me.MainPage.RegisterComponent(ParsecAdmin.ScannerParameters.RegistraScansione)
        End If
    End Sub

    'Avvia la scansione. Lanciato da ScansionaImageButton.Click
    Private Sub AvviaScansione()
        Dim scanParameters As New ParsecAdmin.ScannerParameters

        Dim data As String = scanParameters.CreaDataSource(New ParsecAdmin.DatiCredenziali, New ParsecAdmin.DatiScansione With {.Duplex = Me.FronteRetroCheckBox.Checked, .DoBarCodeScan = Me.ScansionaCodiceBarreRadioButton.Checked, .DoOcrScan = Me.ScansionaCodiceBarreRadioButton.Checked, .UseUi = False})

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiScansione(data, Me.ScanUploadButton.ClientID, Me.infoScansioneHidden.ClientID, True, False)
        Else
            'UTILIZZO IL SOCKET  
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, AddressOf Me.NotificaInfoScansione, AddressOf Me.NotificaScansione)
        End If

    End Sub

    'Riempie il value di infoScansioneHidden
    Private Sub NotificaInfoScansione(ByVal data As String)
        Me.infoScansioneHidden.Value = data
    End Sub

    'Metodo lanciato da AvviaScansione(). Effettua la scansione
    Private Sub NotificaScansione()

        Dim sb As New StringBuilder

        '*********************************************************************************************
        'Se l'allegato è il risultato di una scansione ed è stato eseguito l'ocr.
        '*********************************************************************************************
        If Not String.IsNullOrEmpty(Me.infoScansioneHidden.Value) Then


            Dim allegati As New ParsecPro.AllegatiScansionatiRepository
            Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

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

            Dim anno As Nullable(Of Integer)
            Dim numero As Nullable(Of Integer)
            Dim tipo As ParsecPro.TipoRegistrazione = ParsecPro.TipoRegistrazione.Nessuna

            If Not Me.ScansionaCodiceBarreRadioButton.Checked Then
                anno = CInt(Me.AnnoProtocolloTextBox.Text)
                numero = CInt(Me.NumeroProtocolloTextBox.Text)
                If Me.RegistrazioneInternaCheckBox.Checked Then
                    tipo = ParsecPro.TipoRegistrazione.Interna
                End If
            End If

            Dim allegatiScansionati As List(Of ParsecPro.AllegatoScansionato) = allegati.GetViewFromDataSet(ds, utente, Me.ScansionaCodiceBarreRadioButton.Checked)

            Dim i As Integer = 0
            For Each allegatoScansionato As ParsecPro.AllegatoScansionato In allegatiScansionati
                i += 1
                If Not Me.ScansionaCodiceBarreRadioButton.Checked Then
                    allegatoScansionato.NumeroProtocollo = numero
                    allegatoScansionato.AnnoProtocollo = anno
                    allegatoScansionato.TipoRegistrazione = tipo
                Else
                    numero = allegatoScansionato.NumeroProtocollo
                    anno = allegatoScansionato.AnnoProtocollo
                    tipo = allegatoScansionato.TipoRegistrazione
                End If

                'SE IL CODICE A BARRE NON E' STATO RICONOSCIUTO
                If Not numero.HasValue OrElse Not anno.HasValue OrElse tipo = TipoRegistrazione.Nessuna Then

                    allegatoScansionato.Id = Me.GetNuovoIdAllegatoTemporaneo()
                    allegatoScansionato.Utente = utente.Username
                    Me.AllegatiScansionatiNonRiconosciuti.Add(allegatoScansionato)

                    'MEMORIZZO L'ERRORE E CONTINUO
                    sb.AppendLine(String.Format("Scansione {0} di {1} non riconosciuta", i.ToString, allegatiScansionati.Count))
                    Continue For
                Else
                    'SE NON ESISTE NESSUNA REGISTRAZIONE
                    'MEMORIZZARE L'ERRORE
                    Dim registrazioni As New ParsecPro.RegistrazioniRepository

                    Dim registrazione As ParsecPro.Registrazione = registrazioni.Where(Function(c) c.Modificato = False And c.NumeroProtocollo = numero.Value And Year(c.DataImmissione.Value) = anno.Value).FirstOrDefault

                    registrazioni.Dispose()
                    If registrazione Is Nothing Then
                        Dim tipologia As String = "A"
                        If tipo = TipoRegistrazione.Interna Then
                            tipologia = "I"
                        End If

                        allegatoScansionato.Id = Me.GetNuovoIdAllegatoTemporaneo()
                        allegatoScansionato.Utente = utente.Username
                        Me.AllegatiScansionatiNonRiconosciuti.Add(allegatoScansionato)

                        sb.AppendLine(String.Format("Scansione {0} di {1} - Impossibile trovare la registrazione {2}/{3} - {4}", i.ToString, allegatiScansionati.Count, numero.Value, anno.Value, tipologia))
                        Continue For
                    End If

                End If

                allegati.Save(allegatoScansionato)

                Me.SelectedItems.Add(allegatoScansionato.Id, True)
            Next

            If Me.DataAcquisizioneInizioTextBox.SelectedDate > Today Then
                Me.DataAcquisizioneInizioTextBox.SelectedDate = Now
            End If
            If Me.DataAcquisizioneFineTextBox.SelectedDate < Today Then
                Me.DataAcquisizioneFineTextBox.SelectedDate = Now
            End If

            Me.AllegatiScansionati = Nothing
            Me.AllegatiScansionatiGridView.Rebind()

            Me.infoScansioneHidden.Value = String.Empty
            ms.Close()
            allegati.Dispose()
        End If
        If sb.Length > 0 Then
            ParsecUtility.Utility.MessageBox(sb.ToString, False)
        End If
        Me.AllegatiScansionatiErroreGridView.Rebind()
    End Sub

#End Region

#Region "EVENTI GRIGLIA SCANSIONI NON ASSOCIATE"

    'Evento NeedDataSource associato alla Griglia AllegatiScansionatiErroreGridView. Associa il datasorce alla griglia.
    Protected Sub AllegatiScansionatiErroreGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles AllegatiScansionatiErroreGridView.NeedDataSource
        Me.AllegatiScansionatiErroreGridView.DataSource = Me.AllegatiScansionatiNonRiconosciuti
        Me.rowSelectedHidden.Value = "0"
    End Sub

    'Evento PreRender associato alla Griglia AllegatiScansionatiErroreGridView. Abilita o disabilita il pulsante di seleizone dui tutti i record della griglia in base al count.
    Protected Sub AllegatiScansionatiErroreGridView_PreRender(sender As Object, e As System.EventArgs) Handles AllegatiScansionatiErroreGridView.PreRender
        Dim gridHeaderItems = Me.AllegatiScansionatiErroreGridView.MasterTableView.GetItems(GridItemType.Header)
        Dim selectAllCheckBox As CheckBox = CType(CType(gridHeaderItems(0), GridHeaderItem)("SelectCheckBox").Controls(0), CheckBox)
        'DISABILITO LA CHECKBOX SELEZIONA TUTTO SE NON C'E' NIENTE DA SELEZIONARE
        If Me.AllegatiScansionatiErroreGridView.Items.Count = 0 Then
            selectAllCheckBox.Enabled = False
        End If
    End Sub

    'Evento ItemCommand associato alla Griglia AllegatiScansionatiErroreGridView. Permette l'esecuzione dei vari comandi attivabili dalla griglia AllegatiScansionatiErroreGridView.
    Protected Sub AllegatiScansionatiErroreGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiScansionatiErroreGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaAllegatoScansionatoErrore(e.Item)
        End If
        If e.CommandName = "Sort" Then
            Me.scrollPosErroreHidden.Value = "0"
        End If
        If e.CommandName = "Preview" Then
            Me.DownloadFileErrore(e.Item)
        End If
        If e.CommandName = "Save" Then
            Me.SalvaAllegatoScansionatoErrore(e.Item)
        End If

    End Sub

    'Evento ItemDataBound associato alla Griglia AllegatiScansionatiErroreGridView. Personalizza i tootip in base al contenuto delle celle.
    Protected Sub AllegatiScansionatiErroreGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AllegatiScansionatiErroreGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina allegato scansionato"
                Dim message As String = "Eliminare l'elemento selezionato?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If
            If TypeOf dataItem("Save").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Save").Controls(0), ImageButton)
                btn.ToolTip = "Salva allegato scansionato"
            End If
            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Preview").Controls(0), ImageButton)
                btn.ToolTip = "Visualizza allegato scansionato"
            End If
        End If
    End Sub

    'Evento ItemCreated associato alla Griglia AllegatiScansionatiErroreGridView. Setta lo style nel caso di item di tipo GridHeaderItem.
    Private Sub AllegatiScansionatiErroreGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles AllegatiScansionatiErroreGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    'Scarica il file associato alla griglia AllegatiScansionatiErroreGridView. Lanciato da AllegatiScansionatiErroreGridView.ItemCommand
    Private Sub DownloadFileErrore(ByVal item As Telerik.Web.UI.GridDataItem)
        Try
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim allegato As ParsecPro.AllegatoScansionato = Me.AllegatiScansionatiNonRiconosciuti.Where(Function(c) c.Id = id).FirstOrDefault
            Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegato.Nomefile
            Dim file As New IO.FileInfo(pathDownload)
            If Not allegato Is Nothing Then
                If file.Exists Then
                    Session("AttachmentFullName") = file.FullName
                    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                    ParsecUtility.Utility.PageReload(pageUrl, False)
                Else
                    ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
                End If
            Else
                'Allegato non trovato
            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox("Impossibile aprire l'allegato selezionato, per il seguente errore:" & vbCrLf & ex.Message, False)
        End Try
    End Sub

    'Elimina un Allegato associato alla griglia AllegatiScansionatiErroreGridView. Lanciato da AllegatiScansionatiErroreGridView.ItemCommand.
    Private Sub EliminaAllegatoScansionatoErrore(ByVal item As Telerik.Web.UI.GridDataItem)
        Try
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

            Dim allegato As ParsecPro.AllegatoScansionato = Me.AllegatiScansionatiNonRiconosciuti.Where(Function(c) c.Id = id).FirstOrDefault
            If Not allegato Is Nothing Then
                Dim message As String = "L'allegato selezionato è stato cancellato con successo!"
                Try

                    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegato.Nomefile
                    If IO.File.Exists(pathDownload) Then
                        IO.File.Delete(pathDownload)
                    End If

                    '*********************************************************
                    'Aggiorno la griglia
                    '*********************************************************
                    Me.AllegatiScansionatiNonRiconosciuti.Remove(allegato)
                    Me.AllegatiScansionatiErroreGridView.Rebind()

                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox("Impossibile cancellare l'allegato selezionato, per il seguente errore:" & vbCrLf & ex.Message, False)
                Finally
                    ParsecUtility.Utility.MessageBox(message, False)
                End Try
            Else
                'Allegato non trovato
            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox("Impossibile cancellare l'allegato selezionato, per il seguente errore:" & vbCrLf & ex.Message, False)
        End Try
    End Sub

    'Salva l'allegato. Lanciato da AllegatiScansionatiErroreGridView.ItemCommand,
    Private Sub SalvaAllegatoScansionatoErrore(ByVal item As Telerik.Web.UI.GridDataItem)

        Try
            'SE NON SONO STATI SPECIFICATI ANNO E NUMERO 
            'O SE IL PROTOCOLLO NON VIENE TROVATO O E' BLOCCATO 
            'O SE E' STATA INVIATA AL MODULO ARCHIVIO
            'RESTITUISCO NOTHING E MEMORIZZO IL MESSAGGIO DI ERRORE
            Dim registrazione As ParsecPro.Registrazione = Me.VerificaRegistrazione(Me.NumeroProtocolloErroreTextBox, Me.AnnoProtocolloErroreTextBox, Me.RegistrazioneInternaErroreCheckBox)

            If Not registrazione Is Nothing Then

               
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim allegatoScansionato As ParsecPro.AllegatoScansionato = Me.AllegatiScansionatiNonRiconosciuti.Where(Function(c) c.Id = id).FirstOrDefault

                If Not allegatoScansionato Is Nothing Then

                    Dim registrazioni As New ParsecPro.RegistrazioniRepository

                    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegatoScansionato.Nomefile


                    If IO.File.Exists(pathDownload) Then

                        '**************************************************************************************************************************************
                        'GESTIONE TIMBRATURA
                        '**************************************************************************************************************************************

                        Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                        Dim watermark As String = cliente.Descrizione & " - Cod. Amm. " & cliente.CodiceAmministrazione & " - Prot. n. " & registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy HH:mm}", registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(registrazione.TipoRegistrazione).ToUpper


                        Me.AddWatermarkToPdf(watermark, allegatoScansionato)
                        '**************************************************************************************************************************************

                        Dim allegato As New ParsecPro.Allegato
                        allegato.Oggetto = "Allegato n. " & (registrazione.Allegati.Count + 1).ToString
                        allegato.IdTipologiaDocumento = 1 'Primario
                        If registrazione.Allegati.Count > 0 Then
                            allegato.IdTipologiaDocumento = 0 'Secondario
                        End If
                        allegato.NomeFile = allegatoScansionato.Nomefile
                        allegato.NomeFileTemp = allegatoScansionato.Nomefile
                        allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                        allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
                        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")


                        '*********************************************************
                        'Aggiungo l'allegato alla registrazione corrispondente.
                        '*********************************************************
                        registrazione.Allegati.Add(allegato)


                        Try

                            '*********************************************************
                            'Salvo l'allegato nella registrazione corrispondente.
                            '*********************************************************
                            registrazioni.SaveAllegati(registrazione)

                            Dim reg = registrazioni.Where(Function(c) c.Id = registrazione.Id).FirstOrDefault

                            reg.NumeroAllegati = registrazione.Allegati.Count

                            If Not reg.TipologiaAllegatoPrimario.HasValue Then
                                reg.TipologiaAllegatoPrimario = ParsecPro.TipologiaAllegatoPrimario.Generico
                            End If

                            registrazioni.SaveChanges()

                        Catch ex As Exception
                            If ex.InnerException Is Nothing Then
                                Me.message.AppendLine(ex.Message)
                            Else
                                Me.message.AppendLine(ex.InnerException.Message)
                            End If


                        Finally

                            '*********************************************************
                            'Elimino l'allegato temporaneo.
                            '*********************************************************

                            Me.AllegatiScansionatiNonRiconosciuti.Remove(allegatoScansionato)

                            '*********************************************************
                            'Aggiorno la griglia.
                            '*********************************************************
                            Me.AllegatiScansionatiErroreGridView.Rebind()

                            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"

                        End Try

                        '*********************************************************
                        'Libero le risorse.
                        '*********************************************************
                        registrazioni.Dispose()

                    Else
                        Me.message.AppendLine("File '" & allegatoScansionato.Nomefile & "' non trovato!")
                    End If

                    
                Else
                    'ALLEGATO NON TROVATO
                End If

               
                '*********************************************************
                'Visualizzo il messaggio di errore.
                '*********************************************************
                If Me.message.Length > 0 Then
                    Me.message.Insert(0, String.Format("Problemi riscontrati durante il salvataggio della registrazione n. {0}/{1}:", registrazione.NumeroProtocollo, Year(registrazione.DataImmissione)) & vbCrLf & vbCrLf)
                    ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
                    Me.message.Clear()
                End If

                

            Else
                If Me.message.Length > 0 Then
                    ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
                    Me.message.Clear()
                End If
            End If

        Catch ex As Exception
            If ex.InnerException Is Nothing Then
                ParsecUtility.Utility.MessageBox("Impossibile salvare l'allegato selezionato, per il seguente errore:" & vbCrLf & ex.Message, False)
            Else
                ParsecUtility.Utility.MessageBox("Impossibile salvare l'allegato selezionato, per il seguente errore:" & vbCrLf & ex.InnerException.Message, False)
            End If
        End Try


    End Sub

    'Evento click di SalvaAllegatiSelezionatiErroreImageButton. Salva gli allegati.
    Protected Sub SalvaAllegatiSelezionatiErroreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles SalvaAllegatiSelezionatiErroreImageButton.Click

        If Me.AllegatiScansionatiErroreGridView.SelectedIndexes.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno un allegato!", False)
            Exit Sub
        End If

        'SE NON SONO STATI SPECIFICATI ANNO E NUMERO 
        'O SE IL PROTOCOLLO NON VIENE TROVATO O E' BLOCCATO 
        'O SE E' STATA INVIATA AL MODULO ARCHIVIO
        'RESTITUISCO NOTHING E MEMORIZZO IL MESSAGGIO DI ERRORE
        Dim registrazione As ParsecPro.Registrazione = Me.VerificaRegistrazione(Me.NumeroProtocolloErroreTextBox, Me.AnnoProtocolloErroreTextBox, Me.RegistrazioneInternaErroreCheckBox)


        If Not registrazione Is Nothing Then

            Dim registrazioni As New ParsecPro.RegistrazioniRepository

            Dim protocolli As New System.Collections.Generic.Dictionary(Of Integer, ParsecPro.Registrazione)
            Dim allegatiInfos As New List(Of AllegatoInfo)
            Dim allegatiScansionatiToDelete As New System.Collections.Generic.Dictionary(Of Integer, Integer)


            Dim selectedItem As GridDataItem = Nothing
            Dim idSelezionato As Integer = 0
            Dim allegatoScansionato As ParsecPro.AllegatoScansionato = Nothing
            Dim pathDownload As String = String.Empty
            Dim idRegistrazione As Integer = 0

            Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
            Dim watermark As String = cliente.Descrizione & " - Cod. Amm. " & cliente.CodiceAmministrazione & " - Prot. n. " & registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy HH:mm}", registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(registrazione.TipoRegistrazione).ToUpper

            Dim allegato As ParsecPro.Allegato = Nothing
            Dim cnt As Integer = 0

            For j As Integer = 0 To Me.AllegatiScansionatiErroreGridView.SelectedIndexes.Count - 1

                selectedItem = CType(Me.AllegatiScansionatiErroreGridView.Items(Me.AllegatiScansionatiErroreGridView.SelectedIndexes(j)), GridDataItem)
                idSelezionato = selectedItem.OwnerTableView.DataKeyValues(selectedItem.ItemIndex)("Id")
                allegatoScansionato = Me.AllegatiScansionatiNonRiconosciuti.Where(Function(c) c.Id = idSelezionato).FirstOrDefault

                If Not allegatoScansionato Is Nothing Then

                    pathDownload = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegatoScansionato.Nomefile

                    If IO.File.Exists(pathDownload) Then

                        idRegistrazione = registrazione.Id


                        If Not protocolli.ContainsKey(idRegistrazione) Then
                            protocolli.Add(idRegistrazione, registrazione)
                        End If


                        If Not allegatiScansionatiToDelete.ContainsKey(allegatoScansionato.Id) Then
                            allegatiScansionatiToDelete.Add(allegatoScansionato.Id, idRegistrazione)
                        End If


                        '**************************************************************************************************************************************
                        'GESTIONE TIMBRATURA
                        '**************************************************************************************************************************************

                        Me.AddWatermarkToPdf(watermark, allegatoScansionato)
                        '**************************************************************************************************************************************


                        allegato = New ParsecPro.Allegato
                        cnt = allegatiInfos.Where(Function(c) c.IdRegistrazione = idRegistrazione).Count + registrazione.Allegati.Count
                        allegato.Oggetto = "Allegato n. " & (cnt + 1).ToString
                        allegato.IdTipologiaDocumento = 1 'Primario
                        If cnt > 0 Then
                            allegato.IdTipologiaDocumento = 0 'Secondario
                        End If
                        allegato.NomeFile = allegatoScansionato.Nomefile
                        allegato.NomeFileTemp = allegatoScansionato.Nomefile
                        allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                        allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
                        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")


                        '*********************************************************
                        'Aggiungo l'allegato alla registrazione corrispondente.
                        '*********************************************************

                        allegatiInfos.Add(New AllegatoInfo With {.IdRegistrazione = idRegistrazione, .Allegato = allegato})

                    Else
                        Me.message.AppendLine("File '" & allegatoScansionato.Nomefile & "' non trovato!")
                    End If

                End If

            Next

            For Each protocollo As ParsecPro.Registrazione In protocolli.Values
                Try
                    Dim idProtocollo As Integer = protocollo.Id
                    Dim allegati As List(Of ParsecPro.Allegato) = allegatiInfos.Where(Function(c) c.IdRegistrazione = idProtocollo).Select(Function(c) c.Allegato).ToList


                    protocollo.Allegati.AddRange(allegati)
                    '*********************************************************
                    'Salvo gli allegati nella registrazione corrispondente.
                    '*********************************************************
                    registrazioni.SaveAllegati(protocollo)

                    Dim reg = registrazioni.Where(Function(c) c.Id = registrazione.Id).FirstOrDefault

                    reg.NumeroAllegati = allegati.Count

                    If Not reg.TipologiaAllegatoPrimario.HasValue Then
                        reg.TipologiaAllegatoPrimario = ParsecPro.TipologiaAllegatoPrimario.Generico
                    End If
                    registrazioni.SaveChanges()

                Catch ex As Exception
                    If ex.InnerException Is Nothing Then
                        Me.message.AppendLine(ex.Message)
                    Else
                        Me.message.AppendLine(ex.InnerException.Message)
                    End If

                Finally

                    '*********************************************************
                    'Elimino gli allegati temporanei.
                    '*********************************************************
                    Dim allegatoScansionatoErrore As ParsecPro.AllegatoScansionato = Nothing
                    Dim idAllegato As Integer = 0
                    For Each item As KeyValuePair(Of Integer, Integer) In allegatiScansionatiToDelete
                        If item.Value = protocollo.Id Then
                            idAllegato = item.Key
                            allegatoScansionatoErrore = Me.AllegatiScansionatiNonRiconosciuti.Where(Function(c) c.Id = idAllegato).FirstOrDefault
                            Me.AllegatiScansionatiNonRiconosciuti.Remove(allegatoScansionatoErrore)
                        End If
                    Next
                End Try
            Next


            '*********************************************************
            'Libero le risorse.
            '*********************************************************
            registrazioni.Dispose()


            '*********************************************************
            'Visualizzo il messaggio di errore.
            '*********************************************************
            If Me.message.Length > 0 Then
                Me.message.Insert(0, String.Format("Problemi riscontrati durante il salvataggio della registrazione n. {0}/{1}:", registrazione.NumeroProtocollo, Year(registrazione.DataImmissione)) & vbCrLf & vbCrLf)
                ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
                Me.message.Clear()
            Else
                Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
            End If

            '*********************************************************
            'Aggiorno la griglia.
            '*********************************************************

            Me.AllegatiScansionatiErroreGridView.Rebind()

        Else

            If Me.message.Length > 0 Then
                ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
                Me.message.Clear()
            End If

        End If

    End Sub

    'Evento click di EliminaAllegatiSelezionatiErroreImageButton. Elimina gli allegati.
    Protected Sub EliminaAllegatiSelezionatiErroreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaAllegatiSelezionatiErroreImageButton.Click

        If Me.AllegatiScansionatiErroreGridView.SelectedIndexes.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno un allegato!", False)
            Exit Sub
        End If

        Dim selectedItem As GridDataItem = Nothing
        Dim idSelezionato As Integer = 0

        For j As Integer = 0 To Me.AllegatiScansionatiErroreGridView.SelectedIndexes.Count - 1
            selectedItem = CType(Me.AllegatiScansionatiErroreGridView.Items(Me.AllegatiScansionatiErroreGridView.SelectedIndexes(j)), GridDataItem)
            idSelezionato = selectedItem.OwnerTableView.DataKeyValues(selectedItem.ItemIndex)("Id")
            Dim allegato As ParsecPro.AllegatoScansionato = Me.AllegatiScansionatiNonRiconosciuti.Where(Function(c) c.Id = idSelezionato).FirstOrDefault
            If Not allegato Is Nothing Then
                Dim message As String = "L'allegato '" & allegato.Nomefile & "' è stato cancellato con successo!"
                Try
                    Me.AllegatiScansionatiNonRiconosciuti.Remove(allegato)
                    Me.message.AppendLine(message)

                    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegato.Nomefile
                    If IO.File.Exists(pathDownload) Then
                        IO.File.Delete(pathDownload)
                    End If
                Catch ex As Exception
                    Me.message.AppendLine("Impossibile cancellare l'allegato '" & allegato.Nomefile & "', per il seguente errore:" & vbCrLf & ex.Message)
                End Try
            Else
                'Allegato non trovato
            End If
        Next


        If Me.message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
            Me.message.Clear()
        End If

        '*********************************************************
        'Aggiorno la griglia
        '*********************************************************
        Me.AllegatiScansionatiErroreGridView.Rebind()


    End Sub

#End Region

  
End Class
