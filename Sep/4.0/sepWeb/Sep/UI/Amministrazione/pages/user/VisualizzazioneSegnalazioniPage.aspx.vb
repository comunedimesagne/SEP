Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class VisualizzazioneSegnalazioniPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Private Property Segnalazioni() As List(Of ParsecAdmin.Segnalazione)
        Get
            Return CType(Session("VisualizzazioneSegnalazioniPage_Segnalazioni"), List(Of ParsecAdmin.Segnalazione))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Segnalazione))
            Session("VisualizzazioneSegnalazioniPage_Segnalazioni") = value
        End Set
    End Property

    Private Property Segnalazione As ParsecWebServices.TipDetails
        Get
            Return CType(Session("VisualizzazioneSegnalazioniPage_Segnalazione"), ParsecWebServices.TipDetails)
        End Get
        Set(ByVal value As ParsecWebServices.TipDetails)
            Session("VisualizzazioneSegnalazioniPage_Segnalazione") = value
        End Set
    End Property

    Private Property Password As String
        Get
            Return CType(Session("VisualizzazioneSegnalazioniPage_Password"), String)
        End Get
        Set(ByVal value As String)
            Session("VisualizzazioneSegnalazioniPage_Password") = value
        End Set
    End Property

    Private Property UserName As String
        Get
            Return CType(Session("VisualizzazioneSegnalazioniPage_UserName"), String)
        End Get
        Set(ByVal value As String)
            Session("VisualizzazioneSegnalazioniPage_UserName") = value
        End Set
    End Property

    Private Property BaseUrl As String
        Get
            Return CType(Session("VisualizzazioneSegnalazioniPage_BaseUrl"), String)
        End Get
        Set(ByVal value As String)
            Session("VisualizzazioneSegnalazioniPage_BaseUrl") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Segnalazioni"
        Me.ScaricaSegnalazioniButton.Attributes.Add("onclick", "this.disabled=true;")
        If Not Me.Page.IsPostBack Then
            Me.Segnalazioni = Nothing
            Me.Segnalazione = Nothing

            Dim parametri As New ParsecAdmin.ParametriSegnalazioneRepository
            Dim parametro As ParsecAdmin.ParametriSegnalazione = parametri.GetQuery.FirstOrDefault
            parametri.Dispose()

            Me.UserName = ParsecCommon.CryptoUtil.Decrypt(parametro.NomeUtenteRicevente)
            Me.Password = ParsecCommon.CryptoUtil.Decrypt(parametro.PasswordRicevente)
            Me.BaseUrl = parametro.EndPointServizio

            Me.CommentiGridView.DataSource = New List(Of ParsecWebServices.Comment)
            Me.CommentiGridView.DataBind()

            Me.AllegatiSegnalatoreGridView.DataSource = New List(Of ParsecWebServices.Wbfile)
            Me.AllegatiSegnalatoreGridView.DataBind()

            Me.AllegatiRiceventeGridView.DataSource = New List(Of ParsecWebServices.Rfile)
            Me.AllegatiRiceventeGridView.DataBind()

        End If


        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.SegnalazioniGridView.Style.Add("width", widthStyle)

        Me.TabSegnalazioniPanel.Style.Add("width", widthStyle)

        Me.GrigliaSegnalazioniPanel.Style.Add("width", widthStyle)

        Me.RisposteQuestionarioPanel.Style.Add("width", widthStyle)
        Me.CommentiPanel.Style.Add("width", widthStyle)
        Me.AllegatiRiceventePanel.Style.Add("width", widthStyle)
        Me.AllegatiSegnalatorePanel.Style.Add("width", widthStyle)


        Me.CommentiGridView.Style.Add("width", widthStyle)
        Me.AllegatiSegnalatoreGridView.Style.Add("width", widthStyle)
        Me.AllegatiRiceventeGridView.Style.Add("width", widthStyle)


    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete

        If Not Me.Segnalazioni Is Nothing Then
            Me.SegnalazioniLabel.Text = "Segnalazioni&nbsp;&nbsp;" & If(Me.Segnalazioni.Count > 0, "( " & Me.Segnalazioni.Count.ToString & " )", "")
        End If


        If Not Me.Segnalazione Is Nothing Then
            Dim numeroCommenti As Integer = Me.Segnalazione.comments.Count
            Dim numeroAllegatiRicevente As Integer = Me.Segnalazione.rfiles.Count
            Dim numeroAllegatiSegnalante As Integer = Me.Segnalazione.wbfiles.Count

            Me.CommentiLabel.Text = "Commenti&nbsp;&nbsp;" & If(numeroCommenti > 0, "( " & numeroCommenti.ToString & " )", "")
            Me.AllegatiRiceventeLabel.Text = "Allegati Ricevente&nbsp;&nbsp;" & If(numeroAllegatiRicevente > 0, "( " & numeroAllegatiRicevente.ToString & " )", "")
            Me.AllegatiSegnalatoreLabel.Text = "Allegati Segnalante&nbsp;&nbsp;" & If(numeroAllegatiSegnalante > 0, "( " & numeroAllegatiSegnalante.ToString & " )", "")

            Me.SegnalazioneTabStrip.Tabs(1).Text = "Commenti" & If(numeroCommenti > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & numeroCommenti.ToString & ")</span>", "<span style='width:20px'></span>")
            Me.SegnalazioneTabStrip.Tabs(2).Text = "Allegati Ricevente" & If(numeroAllegatiRicevente > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & numeroAllegatiRicevente.ToString & ")</span>", "<span style='width:20px'></span>")
            Me.SegnalazioneTabStrip.Tabs(3).Text = "Allegati Segnalante" & If(numeroAllegatiSegnalante > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & numeroAllegatiSegnalante.ToString & ")</span>", "<span style='width:20px'></span>")

        End If

    End Sub


#End Region

#Region "EVENTI GRIGLIA"


    Protected Sub SegnalazioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles SegnalazioniGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub
   
    Protected Sub SegnalazioniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles SegnalazioniGridView.NeedDataSource
        If Me.Segnalazioni Is Nothing Then
            Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
            Dim data = Now.AddHours(1)
            Dim view = segnalazioni.Where(Function(c) c.DataScadenza >= data).ToList
            Me.Segnalazioni = view
            segnalazioni.Dispose()

        End If
        Me.SegnalazioniGridView.DataSource = Me.Segnalazioni
    End Sub

    Protected Sub SegnalazioniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles SegnalazioniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIE ALLEGATI"

    Protected Sub AllegatiSegnalatoreGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiSegnalatoreGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.DownloadAllegatoSegnalatore(e.Item)
        End If
    End Sub

    Protected Sub AllegatiRiceventeGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiRiceventeGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.DownloadAllegatoRicevente(e.Item)
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI"


    Protected Sub ScaricaSegnalazioniButton_Click(sender As Object, e As System.EventArgs) Handles ScaricaSegnalazioniButton.Click

        Try

            Dim parametri As New ParsecAdmin.ParametriSegnalazioneRepository
            Dim parametro As ParsecAdmin.ParametriSegnalazione = parametri.GetQuery.FirstOrDefault
            parametri.Dispose()


            Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
            service.Authenticate(Me.UserName, Me.Password)

            Dim tips As List(Of ParsecWebServices.Tip) = service.GetTips().OrderByDescending(Function(c) c.creation_date).ToList

            Dim idSegnalazione As String = String.Empty
            Dim nuovaSegnalazione As ParsecAdmin.Segnalazione = Nothing

            Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
            Dim i As Integer = 0
            For Each tip As ParsecWebServices.Tip In tips

                idSegnalazione = tip.internaltipid
                Dim segn = segnalazioni.Where(Function(c) c.GuidSegnalazione = idSegnalazione)
                If Not segn.Any Then
                    i += 1
                    nuovaSegnalazione = New ParsecAdmin.Segnalazione With {.GuidSegnalazione = idSegnalazione, .DataScadenza = tip.expiration_date, .Stato = "NUOVA", .DataCreazione = tip.creation_date, .NumeroSeriale = tip.sequence_number}
                    segnalazioni.Add(nuovaSegnalazione)
                    segnalazioni.SaveChanges()
                    service.SetLabel(tip.internaltipid, "NUOVA")

                    Me.CreaIstanzaSegnalazione(nuovaSegnalazione, parametro)

                End If
            Next

            segnalazioni.Dispose()

            If i = 0 Then
                ParsecUtility.Utility.MessageBox("Non ci sono nuove segnalazioni!", False)
            Else
                Me.Segnalazioni = Nothing
                Me.SegnalazioniGridView.Rebind()
            End If


            If Not Me.Segnalazione Is Nothing Then
                Me.CaricaDettaglioSegnalazione(Me.Segnalazione)
            End If




        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try

        Me.ScaricaSegnalazioniButton.Enabled = True

    End Sub

#End Region

#Region "METODI PRIVATI"

    Public Sub CreaIstanzaSegnalazione(ByVal segnalazione As ParsecAdmin.Segnalazione, ByVal parametri As ParsecAdmin.ParametriSegnalazione)




        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanzaPrecedente As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.IdDocumento = segnalazione.Id AndAlso c.IdModulo = ParsecAdmin.TipoModulo.WBT).FirstOrDefault


        'Se l'istanza del documento non è in iter
        If istanzaPrecedente Is Nothing Then

            Dim descrizioneDocumento As String = String.Format("WBT - {0} del {1}", segnalazione.NumeroSeriale, segnalazione.DataCreazione.ToShortDateString)

            Dim IdModelloIter As Integer = parametri.IdModelloIter

            Dim modelli As New ParsecWKF.ModelliRepository
            Dim modello As ParsecWKF.Modello = modelli.Where(Function(c) c.Id = IdModelloIter).FirstOrDefault
            modelli.Dispose()


            Dim adesso As DateTime = Now



            Dim statoIniziale As Integer = 1
            Dim statoDaEseguire As Integer = 5

            Dim idIstanza As Integer = 0

            'mittente del protocollo 
            Dim idMittente As Integer = parametri.IdDestinatarioIter


            Dim istanza As New ParsecWKF.Istanza
            istanza.Riferimento = descrizioneDocumento
            istanza.IdStato = statoIniziale
            istanza.DataInserimento = adesso
            istanza.DataScadenza = adesso '.AddDays(modelloIter.DurataIter)
            istanza.IdModello = modello.Id
            istanza.IdDocumento = segnalazione.Id

            istanza.Ufficio = idMittente
            'istanza.ContatoreGenerale = segnalazione.Id

            istanza.IdModulo = modello.RiferimentoModulo
            istanza.IdUtente = idMittente

            istanza.FileIter = modello.NomeFile

            Try

                istanze.Add(istanza)
                istanze.SaveChanges()
                istanze.Dispose()

                idIstanza = istanza.Id


                'MITTENTE E DESTINATARIO SOLO GLI STESSI

                '*******************************************************************************************************************************
                'Inserisco nei parametri del processo l'attore DESTINATARIO corrente.
                Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
                Dim processo As New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "DESTINATARIO", .Valore = idMittente.ToString}
                parametriProcesso.Add(processo)
                parametriProcesso.SaveChanges()



                'Inserisco nei parametri del processo l'attore MITTENTE se non esiste.
                Dim parametroProcessoExist As Boolean = Not parametriProcesso.GetQuery.Where(Function(c) c.IdProcesso = idIstanza And c.Nome = "MITTENTE").FirstOrDefault Is Nothing
                If Not parametroProcessoExist Then
                    processo = New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "MITTENTE", .Valore = idMittente.ToString}
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


                Dim corrente As String = modello.StatoIniziale()
                task.Corrente = corrente
                task.Successivo = modello.StatoSuccessivo(corrente)

                task.Mittente = idMittente
                'task.Destinatario = IdDestinatario
                task.IdStato = statoDaEseguire
                task.DataInizio = adesso
                task.DataEsecuzione = adesso
                task.DataFine = adesso '.AddDays(modelloIter.DurataTask)
                task.Operazione = "NUOVO"
                task.Cancellato = False
                task.IdUtenteOperazione = idMittente

                Try

                    tasks.Add(task)
                    tasks.SaveChanges()

                    tasks.Dispose()

                    'Aggiungo il nuovo task
                    Me.Procedi(task, istanza)


                    Me.AggiungiUtenteVisibilita(istanza.IdDocumento, istanza.IdModulo, idMittente)

                Catch ex As Exception
                    Throw New ApplicationException(ex.Message)
                End Try

            Catch ex As Exception
                Throw New ApplicationException(ex.Message)
            End Try

        End If

    End Sub

    '*************************************************************************************************************
    'INSERISCO IL TASK AUTOMATICO
    '*************************************************************************************************************
    Private Sub Procedi(ByVal taskAttivo As ParsecWKF.Task, ByVal istanzaAttiva As ParsecWKF.Istanza)

        Dim statoEseguito As Integer = 6
        Dim statoDaEseguire As Integer = 5
        Dim statoIstanzaCompletato As Integer = 3

        Dim nomeFileIter As String = istanzaAttiva.FileIter
        Dim idUtente As Integer = taskAttivo.Mittente

        Dim tasks As New ParsecWKF.TaskRepository


        tasks.Attach(taskAttivo)
        'Dim taskAttivo As ParsecWKF.Task = tasks.Where(Function(c) c.Id = taskAttivo.Id).FirstOrDefault


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



        '*************************************************************************************************************
        'AGGIORNO IL TASK PRECEDENTE
        '*************************************************************************************************************

        'If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
        '    task.Note = Me.NoteInterneTextBox.Text
        'End If


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

        Dim durata As Integer = 1

        If Not String.IsNullOrEmpty(statoSuccessivo) Then
            'durata = ModelloInfo.DurataTaskIter(statoSuccessivo, taskAttivo.NomeFileIter)
        End If


        'ANNULLO L'ISTANZA
        If azione.Type = "FINE" Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = istanzaAttiva.Id).FirstOrDefault
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

        'If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
        '    nuovotask.Note = Me.NoteInterneTextBox.Text
        'End If


        nuovotask.DataFine = Now.AddDays(durata)
        nuovotask.Cancellato = False
        nuovotask.Notificato = False
        nuovotask.IdUtenteOperazione = idUtente

        tasks.Add(nuovotask)
        tasks.SaveChanges()
        '*************************************************************************************************************

        tasks.Dispose()

    End Sub


    Private Sub AggiungiUtenteVisibilita(ByVal idDocumento As Integer, ByVal idModulo As Integer, ByVal idUtente As Integer)

        Dim visibilitaDocumento As New ParsecAdmin.VisibilitaDocumentoRepository

        Dim utenti As New ParsecAdmin.UserRepository
        Dim utente As ParsecAdmin.Utente = utenti.Where(Function(c) c.Id = idUtente).FirstOrDefault
        utenti.Dispose()

        Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
        utenteVisibilita.AbilitaCancellaEntita = False
        utenteVisibilita.Descrizione = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
        utenteVisibilita.TipoEntita = 2
        utenteVisibilita.IdEntita = utente.Id
        utenteVisibilita.IdModulo = idModulo
        utenteVisibilita.LogIdUtente = utente.Id
        utenteVisibilita.LogDataOperazione = Now
        utenteVisibilita.IdDocumento = idDocumento

        visibilitaDocumento.Add(utenteVisibilita)
        visibilitaDocumento.SaveChanges()
        visibilitaDocumento.Dispose()
    End Sub

    Private Sub CaricaDettaglioSegnalazione(ByVal segnalazione As ParsecWebServices.TipDetails)

        Me.CaricaQuestionari(segnalazione)
        Me.CaricaCommenti(segnalazione)
        Me.CaricaAllegatiSegnalatore(segnalazione)
        Me.CaricaAllegatiRicevente(segnalazione)

    End Sub

    Private Sub CaricaCommenti(ByVal segnalazione As ParsecWebServices.TipDetails)
        Dim commenti = segnalazione.comments.OrderByDescending(Function(c) c.creation_date).ToList
        Me.CommentiGridView.DataSource = commenti
        Me.CommentiGridView.DataBind()
    End Sub

    'ALLEGATI CHE IL RICEVENTE HA INVIATO AL SEGNALANTE
    Private Sub CaricaAllegatiSegnalatore(ByVal segnalazione As ParsecWebServices.TipDetails)
        Me.AllegatiSegnalatoreGridView.DataSource = segnalazione.wbfiles
        Me.AllegatiSegnalatoreGridView.DataBind()
    End Sub

    'ALLEGATI CHE IL SEGNALANTE HA INVIATO AL RICEVENTE
    Private Sub CaricaAllegatiRicevente(ByVal segnalazione As ParsecWebServices.TipDetails)
        Me.AllegatiRiceventeGridView.DataSource = segnalazione.rfiles
        Me.AllegatiRiceventeGridView.DataBind()
    End Sub

    Private Sub CaricaQuestionari(ByVal segnalazione As ParsecWebServices.TipDetails)

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.RisposteQuestionarioPanel.Controls.Clear()


        Dim divDomanda As String = "<div style='width:" & widthStyle & ";height:20px;padding-left:20px;'><span class='Etichetta' style='font-weight:bold;padding-top:2px;background-color:#f5f5f5;width:" & widthStyle & ";border: 1px solid #ddd;display: block'>{0}</span></div>"

        Dim divRisposta As String = "<div style='width:" & widthStyle & ";height:20px;padding-left:20px;'><span class='Etichetta' style='padding-top:2px;width:" & widthStyle & ";'>{0}</span></div>"
        Dim divRispostaSimbolo As String = "<div style='width:" & widthStyle & ";height:20px;padding-left:20px;'><span class='Etichetta' style='padding-top:2px;width:" & widthStyle & ";font-family:wingdings;font-size:12pt'>{0}</span></div>"

        Dim divStep As String = "<div style='width:" & widthStyle & ";height:20px;'><span class='Etichetta' style='font-weight:bold;padding-top:2px;width:" & widthStyle & ";'>{0}</span></div>"


        If Not segnalazione.answers Is Nothing Then
            Dim domande = CType(segnalazione.answers, Dictionary(Of String, Object)).Where(Function(c) Not c.Value Is Nothing)

            If domande.Count > 0 Then

                Dim i As Integer = 1

                For Each q In segnalazione.questionnaire
                    Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divStep, "Step " & i.ToString & ": " & q.label)))

                    i += 1

                    For Each child In q.children

                        Dim idChild = child.id
                        Dim domanda = domande.Where(Function(c) c.Key = idChild).FirstOrDefault

                        'If domanda.Value(0).Count <= 0 Then
                        '    Continue For
                        'End If

                        Dim tipo = child.type


                        Dim sss = domanda.Value(0)
                        Dim de = CType(sss, Dictionary(Of String, Object))
                        Dim risposta As String = CStr(de.FirstOrDefault.Value)

                        Select Case tipo.ToLower
                            Case "fileupload"
                                'NIENTE
                            Case "selectbox"

                                If child.id = domanda.Key Then
                                    Dim opt = child.options.Where(Function(c) c.id = risposta).FirstOrDefault


                                    Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divDomanda, child.label)))

                                    If Not opt Is Nothing Then
                                        Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divRisposta, opt.label)))
                                    End If

                                End If

                            Case "checkbox"


                                If child.id = domanda.Key Then

                                    Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divDomanda, child.label)))

                                    For Each kpv As KeyValuePair(Of String, Object) In de
                                        Dim key As String = CStr(kpv.Key)
                                        Dim value As Boolean = CBool(kpv.Value)
                                        If value Then
                                            Dim opt = child.options.Where(Function(c) c.id = key).FirstOrDefault
                                            Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divRisposta, If(opt Is Nothing, "", opt.label))))
                                        End If
                                    Next

                                End If

                            Case Else

                                'If child.id = domanda.Key Then

                                Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divDomanda, child.label)))

                                If Not String.IsNullOrEmpty(risposta) Then

                                    If tipo = "tos" Then

                                        If CBool(risposta) Then
                                            Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divRispostaSimbolo, "&#252;")))
                                        End If

                                    Else
                                        Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divRisposta, risposta)))
                                    End If

                                End If

                                ' End If


                        End Select

                        Me.RisposteQuestionarioPanel.Controls.Add(New LiteralControl(String.Format(divRisposta, "")))

                    Next

                Next
            End If


        End If

    End Sub

    Private Sub DownloadAllegatoSegnalatore(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("id")
        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("name")

        Try
            Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
            service.Authenticate(Me.UserName, Me.Password)
            Dim fileBuffer = service.GetWhistleBlowerFile(id)

            Dim ht As New Hashtable
            Dim est As String = IO.Path.GetExtension(filename)
            ht.Add("Content", fileBuffer)
            ht.Add("Extension", est)
            ht.Add("Filename", filename)
            Session("AttachmentFullName") = ht
            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
            ParsecUtility.Utility.PageReload(pageUrl, False)

            'Dim segnalazione = service.GetDetailsTip(Me.Segnalazione.internaltip_id)
            Me.CaricaDettaglioSegnalazione(Me.Segnalazione)

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    Private Sub DownloadAllegatoRicevente(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("id")
        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("name")

        Try
            Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
            service.Authenticate(Me.UserName, Me.Password)
            Dim fileBuffer = service.GetReceiverFile(id)

            Dim ht As New Hashtable
            Dim est As String = IO.Path.GetExtension(filename)
            ht.Add("Content", fileBuffer)
            ht.Add("Extension", est)
            ht.Add("Filename", filename)
            Session("AttachmentFullName") = ht
            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
            ParsecUtility.Utility.PageReload(pageUrl, False)

            ' Dim segnalazione = service.GetDetailsTip(Me.Segnalazione.internaltip_id)
            Me.CaricaDettaglioSegnalazione(Me.Segnalazione)

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Try
            'Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("GuidSegnalazione")
            Dim guid As String = item.GetDataKeyValue("GuidSegnalazione")
            Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
            service.Authenticate(Me.UserName, Me.Password)
            Dim tip = service.GetDetailsTip(guid)
            Me.Segnalazione = tip
            Me.CaricaDettaglioSegnalazione(tip)
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

#End Region

End Class