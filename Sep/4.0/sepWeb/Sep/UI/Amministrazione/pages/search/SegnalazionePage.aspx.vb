Imports System.Data
Imports System.Drawing

Partial Class SegnalazionePage
    Inherits System.Web.UI.Page


#Region "PROPRIETA' PAGINA"

    Public Property Segnalazione As ParsecWebServices.TipDetails
        Get
            Return CType(Session("SegnalazionePage_Segnalazione"), ParsecWebServices.TipDetails)
        End Get
        Set(ByVal value As ParsecWebServices.TipDetails)
            Session("SegnalazionePage_Segnalazione") = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return CType(Session("SegnalazionePage_Password"), String)
        End Get
        Set(ByVal value As String)
            Session("SegnalazionePage_Password") = value
        End Set
    End Property

    Public Property UserName As String
        Get
            Return CType(Session("SegnalazionePage_UserName"), String)
        End Get
        Set(ByVal value As String)
            Session("SegnalazionePage_UserName") = value
        End Set
    End Property

    Public Property BaseUrl As String
        Get
            Return CType(Session("SegnalazionePage_BaseUrl"), String)
        End Get
        Set(ByVal value As String)
            Session("SegnalazionePage_BaseUrl") = value
        End Set
    End Property

  
#End Region

#Region "EVENTI PAGINA"


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.SegnalazioneTabStrip.Tabs(1).Visible = False
        Me.MessaggiPageView.Visible = False

        If Not Me.Page.IsPostBack Then

            If Not Me.Page.Request.QueryString("GuidSegnalazione") Is Nothing Then
                Dim guidSegnalazione = CStr(Me.Page.Request.QueryString("GuidSegnalazione"))


                Try
                    Dim parametri As New ParsecAdmin.ParametriSegnalazioneRepository
                    Dim parametro = parametri.GetQuery.FirstOrDefault
                    parametri.Dispose()


                    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                    Dim credenziali As New ParsecAdmin.CredenzialiRiceventeRepository
                    Dim cred = credenziali.Where(Function(c) c.IdUtente = utenteCollegato.Id).FirstOrDefault
                    credenziali.Dispose()

                    If Not cred Is Nothing Then
                        Me.UserName = ParsecCommon.CryptoUtil.Decrypt(cred.NomeUtenteRicevente)
                        Me.Password = ParsecCommon.CryptoUtil.Decrypt(cred.PasswordRicevente)
                        Me.BaseUrl = parametro.EndPointServizio


                        Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
                        service.Authenticate(Me.UserName, Me.Password)
                        Dim segnalazione = service.GetDetailsTip(guidSegnalazione)


                        Me.CaricaDettaglioSegnalazione(segnalazione)
                    


                    Else

                        ParsecUtility.Utility.MessageBox("L'utente corrente non è configurato per accedere alle segnalazioni!", False)
                    End If



                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)

                    Me.MessaggiGridView.DataSource = New List(Of ParsecWebServices.Comment)
                    Me.MessaggiGridView.DataBind()
                    Me.CommentiGridView.DataSource = New List(Of ParsecWebServices.Comment)
                    Me.CommentiGridView.DataBind()
                    Me.AllegatiSegnalatoreGridView.DataSource = New List(Of ParsecWebServices.Wbfile)
                    Me.AllegatiSegnalatoreGridView.DataBind()
                    Me.AllegatiRiceventeGridView.DataSource = New List(Of ParsecWebServices.Rfile)
                    Me.AllegatiRiceventeGridView.DataBind()

                    Me.Disabilita()

                End Try



            End If

            If Not Me.Page.Request.QueryString("Mode") Is Nothing Then
                If Me.Page.Request.QueryString("Mode") = "V" Then

                    Me.Disabilita()

                End If
            End If

        End If



        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        ParsecUtility.Utility.CloseWindow(False)
        '***************************************************************************

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.GrigliaDocumentiPanel.Style.Add("width", widthStyle)



        Me.RisposteQuestionarioPanel.Style.Add("width", widthStyle)

        Me.MessaggiPanel.Style.Add("width", widthStyle)
        Me.CommentiPanel.Style.Add("width", widthStyle)
        Me.AllegatiRiceventePanel.Style.Add("width", widthStyle)
        Me.AllegatiSegnalatorePanel.Style.Add("width", widthStyle)

        Me.MessaggiGridView.Style.Add("width", widthStyle)
        Me.CommentiGridView.Style.Add("width", widthStyle)
        Me.AllegatiSegnalatoreGridView.Style.Add("width", widthStyle)
        Me.AllegatiRiceventeGridView.Style.Add("width", widthStyle)




    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete

        If Not Me.Segnalazione Is Nothing Then
            Dim numeroCommenti As Integer = Me.Segnalazione.comments.Count
            Dim numeroAllegatiRicevente As Integer = Me.Segnalazione.rfiles.Count
            Dim numeroAllegatiSegnalante As Integer = Me.Segnalazione.wbfiles.Count

            Me.CommentiLabel.Text = "Commenti&nbsp;&nbsp;" & If(numeroCommenti > 0, "( " & numeroCommenti.ToString & " )", "")
            Me.AllegatiRiceventeLabel.Text = "Allegati Ricevente&nbsp;&nbsp;" & If(numeroAllegatiRicevente > 0, "( " & numeroAllegatiRicevente.ToString & " )", "")
            Me.AllegatiSegnalatoreLabel.Text = "Allegati Segnalante&nbsp;&nbsp;" & If(numeroAllegatiSegnalante > 0, "( " & numeroAllegatiSegnalante.ToString & " )", "")

            Me.SegnalazioneTabStrip.Tabs(2).Text = "Commenti" & If(numeroCommenti > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & numeroCommenti.ToString & ")</span>", "<span style='width:20px'></span>")
            Me.SegnalazioneTabStrip.Tabs(3).Text = "Allegati Ricevente" & If(numeroAllegatiRicevente > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & numeroAllegatiRicevente.ToString & ")</span>", "<span style='width:20px'></span>")
            Me.SegnalazioneTabStrip.Tabs(4).Text = "Allegati Segnalante" & If(numeroAllegatiSegnalante > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & numeroAllegatiSegnalante.ToString & ")</span>", "<span style='width:20px'></span>")
        End If

    End Sub


#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ChiudiFinestraButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiFinestraButton.Click
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub



    Protected Sub InviaMessaggioButton_Click(sender As Object, e As System.EventArgs) Handles InviaMessaggioButton.Click

        Try
            If Not String.IsNullOrEmpty(Me.MessaggioTextBox.Text.Trim) Then
                Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
                service.Authenticate(Me.UserName, Me.Password)
                service.SendMessage(Me.Segnalazione.id, Me.MessaggioTextBox.Text.Trim)

                Dim segnalazione = service.GetDetailsTip(Me.Segnalazione.internaltip_id)

                Me.CaricaDettaglioSegnalazione(segnalazione)

                Me.MessaggioTextBox.Text = String.Empty

                ParsecUtility.SessionManager.SegnalazioneIllecito = Me.Segnalazione
            Else

                ParsecUtility.Utility.MessageBox("E' necessario specificare il messaggio!", False)

            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try


    End Sub

    Protected Sub InviaCommentoButton_Click(sender As Object, e As System.EventArgs) Handles InviaCommentoButton.Click
        Try
            If Not String.IsNullOrEmpty(Me.CommentoTextBox.Text.Trim) Then
                Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
                service.Authenticate(Me.UserName, Me.Password)
                service.SendComment(Me.Segnalazione.internaltip_id, Me.CommentoTextBox.Text.Trim)

                Dim segnalazione = service.GetDetailsTip(Me.Segnalazione.internaltip_id)

                Me.CaricaDettaglioSegnalazione(segnalazione)

                Me.CommentoTextBox.Text = String.Empty

                ParsecUtility.SessionManager.SegnalazioneIllecito = Me.Segnalazione

            Else

                ParsecUtility.Utility.MessageBox("E' necessario specificare il commento!", False)

            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try

    End Sub

    Protected Sub AggiungiAllegatoSegnalatoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiAllegatoSegnalatoreImageButton.Click

        If Me.AllegatoSegnalatoreUpload.UploadedFiles.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Per inserire il file, è necessario specificarne il file relativo!", False)
            Exit Sub
        End If

        Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoSegnalatoreUpload.UploadedFiles(0)

        Dim descrizione As String = String.Empty

        If String.IsNullOrEmpty(Me.DescrizioneAllegatoSegnalatoreTextBox.Text.Trim) Then
            descrizione = file.FileName
        Else
            descrizione = Me.DescrizioneAllegatoSegnalatoreTextBox.Text.Trim
        End If

        Try
            Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
            service.Authenticate(Me.UserName, Me.Password)

            ' Dim ms As New IO.MemoryStream
            'file.InputStream.CopyTo(ms)

            Dim bytes As Byte() = New Byte(file.ContentLength - 1) {}
            file.InputStream.Read(bytes, 0, bytes.Length)

            service.UploadReceiverFile(descrizione, Me.Segnalazione.internaltip_id, file.FileName, bytes)

            Dim segnalazione = service.GetDetailsTip(Me.Segnalazione.internaltip_id)

            ParsecUtility.SessionManager.SegnalazioneIllecito = Me.Segnalazione

            Me.CaricaDettaglioSegnalazione(segnalazione)

            Me.DescrizioneAllegatoSegnalatoreTextBox.Text = String.Empty

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

#End Region

#Region "PRIVATI"

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

    Private Sub CaricaMessaggi(ByVal segnalazione As ParsecWebServices.TipDetails)
        Dim messaggi = segnalazione.messages.OrderByDescending(Function(c) c.creation_date).ToList
        Me.MessaggiGridView.DataSource = messaggi
        Me.MessaggiGridView.DataBind()
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


    Private Sub CaricaDettaglioSegnalazione(ByVal segnalazione As ParsecWebServices.TipDetails)

        Me.NumeroSequenzaTextBox.Text = segnalazione.sequence_number
        Me.DataCreazioneTextBox.Text = String.Format("{0:dd/MM/yyyy HH:mm}", segnalazione.creation_date)

        Me.StatoTextBox.Text = "STATO: " & segnalazione.label.ToUpper
        Me.DataScadenzaTextBox.Text = String.Format("{0:dd/MM/yyyy HH:mm}", segnalazione.expiration_date)


        Me.CaricaQuestionari(segnalazione)

        If Me.SegnalazioneTabStrip.Tabs(1).Visible Then
            Me.CaricaMessaggi(segnalazione)
        End If


        Me.CaricaCommenti(segnalazione)
        Me.CaricaAllegatiSegnalatore(segnalazione)
        Me.CaricaAllegatiRicevente(segnalazione)

        Me.Segnalazione = segnalazione

       
    End Sub

    Private Sub EliminaAllegatoSegnalatore(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("id")

        Try
            Dim service As New ParsecWebServices.WhistleBlowingService(Me.BaseUrl)
            service.Authenticate(Me.UserName, Me.Password)
            service.DeleteWhistleBlowerFile(id)
            Dim segnalazione = service.GetDetailsTip(Me.Segnalazione.internaltip_id)
            Me.CaricaDettaglioSegnalazione(segnalazione)
            ParsecUtility.SessionManager.SegnalazioneIllecito = Me.Segnalazione
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
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

            Dim segnalazione = service.GetDetailsTip(Me.Segnalazione.internaltip_id)
            Me.CaricaDettaglioSegnalazione(segnalazione)
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

            Dim segnalazione = service.GetDetailsTip(Me.Segnalazione.internaltip_id)
            Me.CaricaDettaglioSegnalazione(segnalazione)
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    Private Sub Disabilita()
        Me.InviaMessaggioButton.Enabled = False
        Me.InviaCommentoButton.Enabled = False
        Me.AllegatoSegnalatoreUpload.Enabled = False
        Me.MessaggioTextBox.Enabled = False
        Me.CommentoTextBox.Enabled = False
        Me.DescrizioneAllegatoSegnalatoreTextBox.Enabled = False
        Me.AllegatiSegnalatoreGridView.MasterTableView.GetColumnSafe("Delete").Visible = False
    End Sub

#End Region


#Region "EVENTI GRIGLIA"

    Protected Sub AllegatiSegnalatoreGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiSegnalatoreGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaAllegatoSegnalatore(e.Item)
        End If

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


End Class