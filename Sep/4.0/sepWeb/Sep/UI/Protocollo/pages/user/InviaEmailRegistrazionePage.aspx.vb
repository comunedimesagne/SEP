#Region "IMPORTS"

Imports ParsecAdmin
Imports Telerik.Web.UI
Imports System.Data
Imports System.Web.Mail
Imports ParsecPro
Imports System.IO
Imports System.Reflection

Imports Rebex.Mail
Imports Rebex.Net
Imports System.Globalization

#End Region

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class InviaEmailRegistrazionePage
    Inherits System.Web.UI.Page

#Region "DICHIARAZIONI"

    Private message As StringBuilder

    Private totdimf As Decimal

    Private chiudiFinestra As Boolean = False

#End Region

#Region "PROPRIETA'"

    'Variabile di Sessione: oggetto Registrazione corrente
    Public Property Registrazione() As ParsecPro.Registrazione
        Get
            Return CType(Session("InviaEmailRegistrazionePage_Registrazione"), ParsecPro.Registrazione)
        End Get
        Set(ByVal value As ParsecPro.Registrazione)
            Session("InviaEmailRegistrazionePage_Registrazione") = value
        End Set
    End Property

    'Variabile di Sessione: Id del Modulo (Atti, Protocollo,..)
    Public Property Modulo As Integer
        Get
            Return CType(Session("InviaEmailRegistrazionePage_Modulo"), Integer)
        End Get
        Set(value As Integer)
            Session("InviaEmailRegistrazionePage_Modulo") = value
        End Set
    End Property

    'Variabile di Sessione.
    Public Property InvioDiretto As Boolean
        Get
            Return CType(Session("InviaEmailRegistrazionePage_InvioDiretto"), Boolean)
        End Get
        Set(value As Boolean)
            Session("InviaEmailRegistrazionePage_InvioDiretto") = value
        End Set
    End Property



#End Region

#Region "METODI PRIVATI"

    'Configura l'SMTP per l'invio della mail.
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

    'Invia la mail
    Private Sub InviaEmail()


        Me.chiudiFinestra = True

        Dim successo As Boolean = True
        Me.message = New StringBuilder

        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")

        Dim idCasella As Integer = CInt(Me.CaselleEmailComboBox.SelectedValue)

        Dim mail As New Rebex.Mail.MailMessage

        Dim destinatari As New List(Of String)

        Dim ids = Me.DestinatariListBox.CheckedItems.Select(Function(c) CInt(c.Value))

        For Each dest As ParsecPro.Destinatario In Me.Registrazione.Destinatari
            If Not String.IsNullOrEmpty(dest.Email) Then
                If ids.Contains(dest.Id) Then
                    If Me.CheckEmail(dest.Email) Then
                        destinatari.Add(dest.Email)
                    End If
                End If
            End If
        Next

        If Not String.IsNullOrEmpty(Me.AltriDestinatariTB.Text) Then
            Dim listaEmail = Me.AltriDestinatariTB.Text.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
            For Each s In listaEmail
                If Me.CheckEmail(s) Then
                    destinatari.Add(s)
                End If
            Next
        End If

        Dim casellePec As New ParsecAdmin.ParametriPecRepository
        Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetByIdAssociatoUtente(idCasella)
        Dim client = Me.ConfigureSmtp(casellaPec)

        Dim mailAttach As Rebex.Mail.Attachment

        '**************************************************************************************************************
        'Per l'invio email è necessario che ci sia almeno un allegato ('ObbligoAllegatiInvioEmail') - tipo invio PEC  - tipo documento ELETTRONICO
        '**************************************************************************************************************

        Dim allegatoEmail As ParsecPro.AllegatoEmail

        mail.From = casellaPec.Email
        Dim elencoDestinatari = String.Join(";", destinatari)
        For Each destinatario In destinatari
            mail.To.Add(destinatario)
        Next

        Dim destinatariCc As New List(Of String)

        If Not String.IsNullOrEmpty(Me.DestinatariCcTextBox.Text) Then
            Dim listaDestinatariCc = Me.DestinatariCcTextBox.Text.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
            For Each s In listaDestinatariCc
                If Me.CheckEmail(s) Then
                    destinatariCc.Add(s)
                End If
            Next
        End If

        Dim elencoDestinatariCc = String.Join(";", destinatariCc)

        For Each destinatarioCc In destinatariCc
            mail.CC.Add(destinatarioCc)
        Next



        Dim oggetto As String = New String(OggettoTextBox.Text.Where(Function(c) Not Char.IsControl(c)).ToArray())


        If Me.RiferimentoProtocolloChkBox.Checked Then
            mail.Subject = OggettoLinkButton.Text & " " & Replace(OggettoTextBox.Text, Constants.vbCrLf, " ")
        Else
            mail.Subject = Replace(OggettoTextBox.Text, Constants.vbCrLf, " ")
        End If

        mail.BodyText = Me.CorpoTextBox.Text

        Dim allegati As New List(Of ParsecPro.AllegatoEmail)
        percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)

        For Each item As GridDataItem In Me.DocumentiGridView.SelectedItems
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim allegato As ParsecPro.Allegato = Me.Registrazione.Allegati.Where(Function(c) c.Id = id).FirstOrDefault
            If Not allegato Is Nothing Then

                Dim fullPath As String = String.Empty
                Select Case CType(Me.Modulo, ParsecAdmin.TipoModulo)
                    Case TipoModulo.PRO
                        fullPath = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
                    Case TipoModulo.PED, TipoModulo.SUAP
                        fullPath = percorsoRoot & allegato.PercorsoRelativo & allegato.NomeFile
                End Select


                If IO.File.Exists(fullPath) Then

                    '*******************************************
                    'Allego all'e-mail i documenti selezionati. 
                    '*******************************************

                    mailAttach = New Rebex.Mail.Attachment(fullPath)
                    mailAttach.FileName = allegato.NomeFile

                    mail.Attachments.Add(mailAttach)

                    allegatoEmail = New ParsecPro.AllegatoEmail
                    allegatoEmail.IdAllegato = allegato.Id
                    allegati.Add(allegatoEmail)



                End If
            End If
        Next

        Dim parametri As New ParametriRepository
        Dim parametro As Parametri = parametri.GetByName("ObbligoAllegatiInvioEmail", ParsecAdmin.TipoModulo.PRO)
        parametri.Dispose()

        If Not parametro Is Nothing Then
            If Not CBool(parametro.Valore) Then
                If allegati.Count = 0 Then
                    Me.message.AppendLine("Per inviare l'e-mail è necessario allegare almeno un documento!")
                    Exit Sub
                End If
            End If
        Else
            If allegati.Count = 0 Then
                Me.message.AppendLine("Per inviare l'e-mail è necessario allegare almeno un documento!")
                Exit Sub
            End If
        End If

        If Not String.IsNullOrEmpty(Me.DimensioneCasellaHidden.Value) Then
            If CInt(Me.DimensioneCasellaHidden.Value) > 0 Then
                Dim dimensioneAllegatiSelezionati As Single = (CInt(Me.DimensioneAllegatiSelezionatiHidden.Value) / 1024) / 1024
                Dim ms As New IO.MemoryStream
                mail.Save(ms, Rebex.Mail.MailFormat.Mime)

                Dim dimensioneEmail As Single = (ms.Length / 1024) / 1024

                ms.Close()
                ms.Dispose()
                ms = Nothing

                Dim dimensioneCasella As Single = CSng(Me.DimensioneCasellaHidden.Value)

                If dimensioneEmail > dimensioneCasella Then
                    Me.chiudiFinestra = False
                    Me.message.AppendLine("La dimensione massima che può raggiungere un'email in uscita (compresi gli allegati) è di " & dimensioneCasella.ToString & " megabyte !" & vbCrLf & "La dimensione dell'email che si sta inviando è di " & String.Format("{0:N3}", dimensioneEmail) & "MB (di cui " & String.Format("{0:N3}", dimensioneAllegatiSelezionati) & " MB allegati)")
                    Exit Sub

                End If
            End If

        End If

        Try
            client.Timeout = 0
            client.Send(mail)
            client.Disconnect()

        Catch ex As System.Web.HttpException
            Me.message.AppendLine("E-mail non inviata! " & vbCrLf & ex.Message)
            successo = False
        Catch ex As Exception
            If TypeOf ex Is SmtpException Then
                Dim smtpEx As SmtpException = CType(ex, SmtpException)
                If Not smtpEx Is Nothing Then
                    If Not smtpEx.Response Is Nothing Then
                        Me.message.AppendLine("E-mail non inviata! " & vbCrLf & ex.Message & " " & smtpEx.Response.Description)
                    End If
                End If
            Else
                Me.message.AppendLine("E-mail non inviata! " & vbCrLf & ex.Message)
            End If

            successo = False
        Finally

            If successo Then


                For Each dest As Rebex.Mime.Headers.MailAddress In mail.To
                    Me.message.AppendLine("L'e-mail indirizzata a '" & dest.Address & "' è stata inviata correttamente!")
                Next

                For Each dest As Rebex.Mime.Headers.MailAddress In mail.CC
                    Me.message.AppendLine("L'e-mail indirizzata a '" & dest.Address & "' è stata inviata correttamente!")
                Next


                Try

                    If Not String.IsNullOrEmpty(elencoDestinatari) Then
                        elencoDestinatari = "To: " & elencoDestinatari
                    End If

                    If Not String.IsNullOrEmpty(elencoDestinatariCc) Then
                        elencoDestinatari &= " - Cc: " & elencoDestinatariCc
                    End If

                    SalvaEmailInDb(idCasella, elencoDestinatari, mail, allegati)

                Catch ex As Exception
                    Me.message.AppendLine(ex.Message)
                End Try

            End If
        End Try
    End Sub

    'Prende i vari parametri dall'oggetto parametriPagina e predispone la maschera.
    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("Modulo") Then Modulo = parametriPagina("Modulo")
            If parametriPagina.ContainsKey("Invio") Then
                If parametriPagina("Invio") = 1 Then
                    InvioDiretto = True
                End If

            End If

            If parametriPagina.ContainsKey("IdCasella") Then
                Try
                    Dim idCasella As Nullable(Of Integer) = parametriPagina("IdCasella")
                    Me.CaselleEmailComboBox.Items.FindItemByValue(idCasella.ToString).Selected = True
                Catch ex As Exception

                End Try

            End If

            If parametriPagina.ContainsKey("RegistrazioneDaInviare") Then
                If Modulo <> 5 And Modulo <> 8 Then
                    Modulo = ParsecAdmin.TipoModulo.PRO
                End If
                Me.Registrazione = parametriPagina("RegistrazioneDaInviare")
                Me.OggettoTextBox.Text = Me.Registrazione.Oggetto
                Dim numeroProtocollo As String = Me.Registrazione.NumeroProtocollo.Value.ToString.PadLeft(7, "0")
                Dim dataImmissione As String = String.Format("{0:dd/MM/yyyy}", Me.Registrazione.DataImmissione)
                Dim parametri As New ParsecAdmin.ParametriRepository
                Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("PEC_OggettoEmail", ParsecAdmin.TipoModulo.PRO)
                If Not parametro Is Nothing Then
                    Dim oggetto As String = String.Format(parametro.Valore, numeroProtocollo, dataImmissione)
                    Me.OggettoLinkButton.Text = oggetto
                Else
                    Me.OggettoLinkButton.Text = "Prot. N. " & numeroProtocollo & " del " & dataImmissione
                End If

                parametro = parametri.GetByName("PEC_CorpoEmail", ParsecAdmin.TipoModulo.PRO)
                If Not parametro Is Nothing Then
                    Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                    Me.CorpoTextBox.Text = String.Format(parametro.Valore, cliente.Descrizione, numeroProtocollo, dataImmissione)

                    If parametriPagina.ContainsKey("CorpoEmail") Then
                        Me.CorpoTextBox.Text &= vbCrLf & parametriPagina("CorpoEmail")
                    End If
                    If parametriPagina.ContainsKey("OggettoSUAPE") Then
                        Me.CorpoTextBox.Text = parametriPagina("OggettoSUAPE")
                    End If
                End If
                parametri.Dispose()

                Dim destinatari = From d In Me.Registrazione.Destinatari Select New With {.Id = d.Id, .DisplayText = String.Format("{0} ({1})", d.Cognome, If(String.IsNullOrEmpty(d.Email), "Nessun indirizzo di posta elettronica", d.Email))}
                Me.DestinatariListBox.DataValueField = "Id"
                Me.DestinatariListBox.DataTextField = "DisplayText"
                Me.DestinatariListBox.DataSource = destinatari
                Me.DestinatariListBox.DataBind()

                For Each dest As RadListBoxItem In Me.DestinatariListBox.Items
                    If dest.Text.Contains("Nessun indirizzo di posta elettronica") Then
                        dest.Checked = False
                        dest.Checkable = False
                    End If
                Next

                'TODO GESTIRE SE PRESENTI IL CARICAMENTO DEGLI ALLEGATI FIRMATI

                For Each allegato In Me.Registrazione.Allegati
                    If Not String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
                        allegato.NomeFile = allegato.NomeFileFirmato
                    End If
                Next

                Me.DocumentiGridView.DataSource = Me.Registrazione.Allegati
                Me.DocumentiGridView.DataBind()
                AggiornaGrigliaAllegati()

                For Each headerItem As GridHeaderItem In Me.DocumentiGridView.MasterTableView.GetItems(GridItemType.Header)
                    Dim chk As CheckBox = CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox)
                    chk.Checked = True
                Next


                If parametriPagina.Contains("DeselezionaAllegati") Then
                    If parametriPagina("DeselezionaAllegati") = 1 Then
                        For Each headerItem As GridHeaderItem In Me.DocumentiGridView.MasterTableView.GetItems(GridItemType.Header)
                            Dim chk As CheckBox = CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox)
                            chk.Checked = False
                        Next
                        For Each Item As GridItem In Me.DocumentiGridView.Items
                            Dim dataItem As GridDataItem = CType(Item, GridDataItem)
                            Dim chiave1 As Integer = CInt(dataItem("Id").Text)
                            If chiave1 > 2 Then
                                Item.Selected = False
                            End If
                        Next

                    End If
                End If

            End If

            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    'Salva la mail su file system
    Private Sub SaveEmail(message As Net.Mail.MailMessage, fullPath As String)
        Dim assembly As Assembly = GetType(Net.Mail.SmtpClient).Assembly
        Dim mailWriterType As Type = assembly.GetType("System.Net.Mail.MailWriter")
        Using ms As New IO.MemoryStream
            Dim mailWriterContructor As ConstructorInfo = mailWriterType.GetConstructor(BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Type() {GetType(Stream)}, Nothing)
            Dim mailWriter As Object = mailWriterContructor.Invoke(New Object() {ms})
            Dim sendMethod As MethodInfo = GetType(Net.Mail.MailMessage).GetMethod("Send", BindingFlags.Instance Or BindingFlags.NonPublic)
            sendMethod.Invoke(message, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Object() {mailWriter, True}, Nothing)
            ms.Position = 0
            Dim bytes As Byte() = ms.ToArray
            IO.File.WriteAllBytes(fullPath, bytes)
            Dim closeMethod As MethodInfo = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance Or BindingFlags.NonPublic)
            closeMethod.Invoke(mailWriter, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Object() {}, Nothing)
        End Using
    End Sub

    'Verifica che l'indirizzo mail sia valido
    Private Function CheckEmail(ByVal Indirizzo As String) As Boolean
        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
        Dim m = emailRegex.Match(Indirizzo)
        Return m.Success
    End Function

    'Aggiorna la grigliadegli allegati
    Private Sub AggiornaGrigliaAllegati()
        For Each item As GridDataItem In Me.DocumentiGridView.Items
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim allegato As ParsecPro.Allegato = Me.Registrazione.Allegati.Where(Function(c) c.Id = id).FirstOrDefault
            Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
            Dim i2 As Integer = 0

            If Not allegato Is Nothing Then
                Dim fullPath As String = String.Empty
                Select Case CType(Me.Modulo, ParsecAdmin.TipoModulo)
                    Case TipoModulo.PRO
                        fullPath = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
                    Case TipoModulo.PED, TipoModulo.SUAP
                        fullPath = percorsoRoot & Me.Registrazione.Allegati(i2).PercorsoRelativo & Me.Registrazione.Allegati(i2).NomeFile
                        i2 += 1
                End Select

                Dim chk As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)
                chk.Checked = True
                chk.Enabled = True
                item.Selected = True
                item.ToolTip = "Seleziona l'Allegato per l'invio"

            End If

        Next

    End Sub

    'Salva la mail su DB
    Private Sub SalvaEmailInDb(ByVal idCasella As Integer, ByVal elencoDestinatari As String, ByVal mail As Rebex.Mail.MailMessage, ByVal allegati As List(Of ParsecPro.AllegatoEmail))

        '************************************
        'Inserisco l'e-mail nel db
        '************************************

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim emails As New EmailRepository
        Dim allegatiEmail As New AllegatoEmailRepository
        Dim email As ParsecPro.Email = emails.CreateFromInstance(Nothing)
        email.IdCasella = idCasella
        email.Inviata = True
        email.IdUtente = utente.Id
        email.DataInvio = Now
        email.Corpo = mail.BodyText
        email.Oggetto = mail.Subject
        email.Destinatari = elencoDestinatari
        email.PercorsoRelativo = String.Format("\{0}\", Now.Year)
        email.NomeFileEml = Guid.NewGuid.ToString & ".eml"

        email.NumeroProtocollo = Me.Registrazione.NumeroProtocollo.Value
        email.AnnoProtocollo = Me.Registrazione.DataImmissione.Value.Year
        email.TipoProtocollo = Me.Registrazione.TipoRegistrazione

        email.MessaggioId = mail.MessageId.Id

        Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & email.PercorsoRelativo & email.NomeFileEml

        If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata")) Then
            IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata"))
        End If

        If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & email.PercorsoRelativo) Then
            IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & email.PercorsoRelativo)
        End If

        Try
            mail.Save(mailBoxPath, Rebex.Mail.MailFormat.Mime)

            Try
                emails.Save(email)

                '*******************************************
                'Aggiungo gli allegati all'e-mail del db
                '*******************************************

                allegatiEmail.SaveAll(email.Id, allegati)

                If ParsecUtility.SessionManager.ElencoIdEmail Is Nothing Then
                    ParsecUtility.SessionManager.ElencoIdEmail = New List(Of Integer)
                End If

                ParsecUtility.SessionManager.ElencoIdEmail.Add(email.Id)
            Catch ex As Exception
                Me.message.AppendLine("Il salvataggio, lato DB, dell'e-mail non si è potuto fare perchè: " & vbCrLf & ex.Message)
            Finally
                emails.Dispose()
                allegatiEmail.Dispose()
            End Try
        Catch ex As Exception
            Me.message.AppendLine("Il salvataggio del file .eml dell'e-mail non si è potuto fare perchè: " & vbCrLf & ex.Message)
        Finally
            emails.Dispose()
            allegatiEmail.Dispose()
        End Try
    End Sub

#End Region

#Region "METODI PROTETTI"

    'Metodo associato alla grigliaDocumentiGridView per la gestione dei checkbox
    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    'Metodo associato alla grigliaDocumentiGridView per la gestione dei checkbox
    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.DocumentiGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
    End Sub

    'Carica la caselle memorizzate su DB e riempie la comboboxCaselleEmailComboBox
    Private Sub CaricaCaselle()
        Dim caselle As New ParsecAdmin.ParametriPecRepository
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim caselleEmail = caselle.GetViewAssociatoUtente(utente.Id)
        Me.CaselleEmailComboBox.DataSource = caselleEmail
        Me.CaselleEmailComboBox.DataTextField = "Email"
        Me.CaselleEmailComboBox.DataValueField = "Id"
        Me.CaselleEmailComboBox.DataBind()

        Me.CaselleEmailComboBox.Items.Insert(0, New RadComboBoxItem("", -1))

        'SE C'E' UNA SOLA CASELLA LA SELEZIONO
        If caselleEmail.Count = 1 Then
            Me.CaselleEmailComboBox.SelectedIndex = 1
        Else
            Me.CaselleEmailComboBox.SelectedIndex = 0
        End If

    End Sub

#End Region

#Region "EVENTI PAGINA"

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then
            Me.CaricaCaselle()
            Me.DimensioneCasellaHidden.Value = String.Empty
            Me.DimensioneAllegatiSelezionatiHidden.Value = String.Empty
        End If

        Me.GetParametri()

        If Not Me.Page.IsPostBack Then
            For i As Integer = 0 To Me.DestinatariListBox.Items.Count - 1
                Me.DestinatariListBox.Items(i).Checked = True
            Next
            If Me.CaselleEmailComboBox.Items.Count > 1 Then MittenteLabel.Text = "Mittente (" & Me.CaselleEmailComboBox.Items.Count - 1 & ")"
            If Me.DestinatariListBox.Items.Count > 0 Then Me.DestinatariLabel.Text = "Destinatari (" & Me.DestinatariListBox.Items.Count & ")"
            Me.CaselleEmailComboBox.Focus()
            Me.RiferimentoProtocolloChkBox.Checked = True
        End If

        '**********************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '**********************************************************
        ParsecUtility.Utility.CloseWindow(False)
        '**********************************************************

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.DocumentiGridView.Style.Add("width", widthStyle)
        Me.scrollPanel.Style.Add("width", widthStyle)
    End Sub

    'Evento LoadComplete della Pagina: dopo che la pagina è stata caricata setta i titoli nelle griglie calcolando la dimensione totale degli allegati.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
        Dim totalBytes As Long

        For Each item As GridDataItem In Me.DocumentiGridView.SelectedItems
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim allegato As ParsecPro.Allegato = Me.Registrazione.Allegati.Where(Function(c) c.Id = id).FirstOrDefault
            If Not allegato Is Nothing Then
                Dim fullPath As String = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
                Dim fi As New IO.FileInfo(fullPath)
                If fi.Exists Then
                    totalBytes += fi.Length
                End If
            End If
        Next

        If totalBytes > 0 Then
            Me.DimensioneAllegatiSelezionatiHidden.Value = totalBytes.ToString
        End If

        Me.AllegatiLabel.Text = "Elenco Allegati&nbsp;&nbsp;" & If(Me.DocumentiGridView.MasterTableView.Items.Count > 0, "( " & Me.DocumentiGridView.MasterTableView.Items.Count.ToString & " )", "")


        Dim mb As Single = (totalBytes / 1024) / 1024


        If Not String.IsNullOrEmpty(Me.DimensioneCasellaHidden.Value) Then

            Dim dimensioneMassimaCasella As Long = CLng(Me.DimensioneCasellaHidden.Value)

            If mb > dimensioneMassimaCasella AndAlso dimensioneMassimaCasella > 0 Then
                Me.DimensioneAllegtiLabel.Style.Remove("color")
                Me.DimensioneAllegtiLabel.Style.Add("color", "#FF0000")
            Else
                Me.DimensioneAllegtiLabel.Style.Remove("color")
                Me.DimensioneAllegtiLabel.Style.Add("color", "#00156E")
            End If

        End If

        Dim kb As Single = (totalBytes / 1024)

        If String.Format("{0:N3}", mb) = "0,000" Then
            Me.DimensioneAllegtiLabel.Text = "Dimensione allegati selezionati:&nbsp;&nbsp;" & String.Format("{0:N3}", kb) & "&nbsp;&nbsp;KB"
        Else
            Me.DimensioneAllegtiLabel.Text = "Dimensione allegati selezionati:&nbsp;&nbsp;" & String.Format("{0:N3}", kb) & "&nbsp;&nbsp;KB&nbsp;&nbsp;&nbsp;(" & String.Format("{0:N3}", mb) & "&nbsp;&nbsp;&nbsp;MB)"
        End If

    End Sub

    'Evento PreRender della Pagina: dopo che la pagina venga renderizzata setta lo scroll scrollPanel ed alcuni tooltip..
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)

        If Me.AltriDestinatariTB.Text.Length > 0 Then
            If Not CheckEmail(Me.AltriDestinatariTB.Text) Then
                Me.AltriDestinatariTB.BorderColor = Drawing.Color.Red
                Me.AltriDestinatariTB.ToolTip = "Indirizzo/i specificato/i non corretti sintatticamente!"
            Else
                Me.AltriDestinatariTB.BorderColor = Drawing.Color.Black
                Me.AltriDestinatariTB.ToolTip = "Eventuali ed ulteriori indirizzi e-mail a cui indirizzare l'e-mail in preparazione"
            End If
        End If

    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemDataBound associato alla Griglia DocumentiGridView. Setta i tooltip in base alle informazioni contenute nelle celle.
    Protected Sub DocumentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim lbl As Label = CType(e.Item.FindControl("NumeratoreLabel"), Label)
            lbl.Text = (e.Item.ItemIndex + 1).ToString
            Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
            Dim allegato As ParsecPro.Allegato = Me.Registrazione.Allegati.Where(Function(c) c.Id = id).FirstOrDefault
            Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")

            If Not allegato Is Nothing Then

                Dim fullPath As String = String.Empty
                Select Case CType(Me.Modulo, ParsecAdmin.TipoModulo)
                    Case TipoModulo.PRO
                        fullPath = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
                    Case TipoModulo.PED, TipoModulo.SUAP
                        fullPath = percorsoRoot & allegato.PercorsoRelativo & allegato.NomeFile
                End Select

                Dim fi As New IO.FileInfo(fullPath)

                If fi.Exists Then

                    Dim mb As Single = (fi.Length / 1024) / 1024
                    Dim dimensioneLabel As Label = CType(e.Item.FindControl("DimensioneLabel"), Label)
                    dimensioneLabel.Text = String.Format("{0:N3}", mb)

                    totdimf += mb

                End If

            End If
        ElseIf TypeOf e.Item Is GridFooterItem Then

            Dim fI As GridFooterItem = CType(e.Item, GridFooterItem)
            With fI.Cells(8)
                If totdimf > 0 Then
                    .Text = String.Format("{0:N3}", totdimf)
                    .ToolTip = "Totale dimensione (in MB) dei file/s allegati"
                    totdimf = 0
                End If
            End With

        End If
    End Sub

    'Evento ItemCreated della DocumentiGridView. Setta lo stile ai GridHeaderItem e aggiunge l'handler per il metodo DocumentiGridView_ItemPreRender per la griglia DocumentiGridView
    Private Sub DocumentiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles DocumentiGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf DocumentiGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    'Evento ItemPreRender della DocumentiGridView. Gestisce il check dei checkbox
    Protected Sub DocumentiGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    'Evento SelectedIndexChanged sulla combo CaselleEmailComboBox. Quando cambia casella postale ricarica la dimensione massima.
    Protected Sub CaselleEmailComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles CaselleEmailComboBox.SelectedIndexChanged
        If Me.CaselleEmailComboBox.SelectedIndex > 0 Then
            Dim casellaPec As ParametriPec = (New ParametriPecRepository).GetById(CInt(Me.CaselleEmailComboBox.SelectedValue))
            Me.DimensioneCasellaHidden.Value = casellaPec.DimensioneMassima.ToString
        Else
            Me.DimensioneCasellaHidden.Value = String.Empty
        End If
    End Sub

    'Evento CheckedChanged definito su RiferimentoProtocolloChkBox. Fa uscire un messaggiosi avviso.
    Protected Sub RiferimentoProtocolloChkBox_CheckedChanged(sender As Object, e As System.EventArgs) Handles RiferimentoProtocolloChkBox.CheckedChanged
        If Not RiferimentoProtocolloChkBox.Checked Then
            ParsecUtility.Utility.MessageBox("Attenzione decheckando non sarà possibile collegare automaticamente le ricevute (accettazione/consegna) al protocollo!", False)
        End If
    End Sub

    'Effettua la chiusura della finestra
    Protected Sub ChiudiButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiButton.Click
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub

    'Evento click che invia la mail.
    Protected Sub InviaEmailButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles InviaEmailButton.Click

        Dim message As New StringBuilder
        Dim cnt As Integer = 0
        Dim ids = Me.DestinatariListBox.CheckedItems.Select(Function(c) CInt(c.Value))


        For Each dest As ParsecPro.Destinatario In Me.Registrazione.Destinatari
            If Not String.IsNullOrEmpty(dest.Email) Then
                If ids.Contains(dest.Id) Then
                    If Me.CheckEmail(dest.Email) Then
                        cnt += 1
                        Exit For
                    End If
                End If
            End If
        Next

        If cnt = 0 Then
            If Not String.IsNullOrEmpty(Me.AltriDestinatariTB.Text.Trim) Then
                Dim listaEmail = Me.AltriDestinatariTB.Text.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
                For Each s In listaEmail
                    If Me.CheckEmail(s) Then
                        cnt += 1
                    End If
                Next
            End If
        End If

        If cnt = 0 Then
            message.AppendLine("Per inviare l'e-mail è necessario specificare almeno un destinatario con un indirizzo di posta corretto!")
        End If


        If Me.CaselleEmailComboBox.SelectedIndex <> 0 Then

            If message.Length = 0 Then
                Me.InviaEmail()
                If Me.message.Length > 0 Then
                    ParsecUtility.SessionManager.EmailInviata = 1
                    ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
                    Me.message.Clear()
                    If Me.chiudiFinestra Then
                        ParsecUtility.Utility.DoWindowClose(False)
                    End If

                End If
            End If

        Else
            message.AppendLine.AppendLine("E' necessario selezionare il mittente!")
        End If

        If message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If

    End Sub

#End Region

End Class