Imports ParsecAdmin
Imports ParsecPro
Imports Telerik.Web.UI
Imports System.IO
Imports System.Web.Mail
Imports Rebex.Net

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class GestioneEmailInviatePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Private message As New StringBuilder

#Region "PROPRIETA'"

    'Variabile di Sessione. lista delle mail agganciata alla Griglia.
    Public Property Emails() As List(Of ParsecPro.Email)
        Get
            Return CType(Session("GestioneEmailInviatePage_Emails"), List(Of ParsecPro.Email))
        End Get
        Set(ByVal value As List(Of ParsecPro.Email))
            Session("GestioneEmailInviatePage_Emails") = value
        End Set
    End Property

    'Variabile di Sessione: lista degli id delle mail selezionate nella griglia.
    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("GestioneEmailInviatePage_SelectedItems") Is Nothing Then
                Session("GestioneEmailInviatePage_SelectedItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("GestioneEmailInviatePage_SelectedItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("GestioneEmailInviatePage_SelectedItems") = value
        End Set
    End Property

#End Region


#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> E-mail inviate"

        Me.EmailInviateComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Seleziona", "-1"))
        Me.EmailInviateComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Si", "1"))
        Me.EmailInviateComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("No", "0"))
        Me.EmailInviateComboBox.SelectedIndex = 0

        If Not Me.IsPostBack Then
            Me.Emails = Nothing
            Me.SelectedItems = Nothing
            Me.DataInvioInizioTextBox.SelectedDate = New Date(Now.Year, Now.Month, 1)
            Me.DataInvioFineTextBox.SelectedDate = New Date(Now.Year, Now.Month, 1).AddMonths(1).AddDays(-1)
        End If
        Me.EmailGridView.GroupingSettings.CaseSensitive = False

    End Sub

    'Evento LoadComplete associato alla Pagina: gestisce i titoli delle griglie e i messaggi di cancellazione.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloElencoMailLabel.Text = "Elenco E-mail Inviate&nbsp;&nbsp;&nbsp;" & If(Me.Emails.Count > 0, "( " & Me.Emails.Count.ToString & " )", "")

        If Me.EmailGridView.SelectedItems.Count = 0 Then
            Dim message As String = "E' necessario selezionare almeno un e-mail!"
            Me.InvioMassivoImageButton.Attributes.Add("onclick", "alert(""" & message & """); return false;")
        End If
        Me.SeparatoreImageButton.Attributes.Add("onclick", "return false;")

        If Me.Emails.Count > 0 Then
            Me.VerificaInvioMassivo()
        End If

        Me.ImpostaUI(Me.Emails.Count > 0)
    End Sub


#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla Griglia EmailGridView. Associa al datasource della griglia EmailGridView la ista delle mail (Emails). Aggiorna la lista Emails.
    Protected Sub EmailGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles EmailGridView.NeedDataSource
        If Me.Emails Is Nothing Then
            Me.Emails = Me.GetEmail()
        End If
        Me.EmailGridView.DataSource = Me.Emails
    End Sub

    'Evento ItemCommand associato alla Griglia EmailGridView. Lancia i vari comandi.
    Protected Sub EmailGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles EmailGridView.ItemCommand
        If e.CommandName = Telerik.Web.UI.RadGrid.ExpandCollapseCommandName AndAlso Not e.Item.Expanded Then
            Dim parentItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
            Dim innerGrid As Telerik.Web.UI.RadGrid = CType(parentItem.ChildItem.FindControl("AllegatiGridView"), Telerik.Web.UI.RadGrid)
            innerGrid.Rebind()
        End If
        If e.CommandName = "SendMail" Then
            Me.SendEmail(e.Item)
        End If
        If e.CommandName = "Delete" Then
            Me.EliminaEmail(e.Item)
        End If
        If e.CommandName = "Sort" Then
            Me.scrollPosHidden.Value = "0"
        End If

        If e.CommandName = "Preview" Then
            Me.DownloadEmail(e.Item)
        End If

    End Sub

    'Evento ItemPreRender associato alla Griglia EmailGridView. Gestisce i checkbox di selezione multipla.
    Protected Sub EmailGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    'Evento ItemDataBound associato alla Griglia EmailGridView. Setta i vari tooltip, le icone e la abilitazione o meno di alcuni campi in base al contenuto delle celle della griglia..
    Protected Sub EmailGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles EmailGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim mail As ParsecPro.Email = CType(e.Item.DataItem, ParsecPro.Email)
            Dim visibile As Boolean = False

            If TypeOf dataItem("SendMail").Controls(0) Is ImageButton Then
                btn = CType(dataItem("SendMail").Controls(0), ImageButton)
                btn.ToolTip = "Invia e-mail"
                visibile = Not mail.Inviata
                If Not visibile Then
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If

            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina e-mail"
                visibile = Not mail.Inviata AndAlso utente.SuperUser
                If Not visibile Then
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If

            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim id As String = mail.Id
            If mail.Inviata Then
                chk.Enabled = False
            End If

            If SelectedItems.ContainsKey(id) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(SelectedItems(id).ToString())
                dataItem.Selected = True
            End If

        End If
    End Sub

    'Evento ItemCreated associato alla Griglia EmailGridView. Definisce la NestedTabl degli Allegati. Gstisce la navigazione nella griglia.
    Private Sub EmailGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles EmailGridView.ItemCreated
        If TypeOf e.Item Is GridNestedViewItem Then
            AddHandler CType(e.Item.FindControl("AllegatiGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.AllegatiGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("AllegatiGridView"), RadGrid).ItemDataBound, New GridItemEventHandler(AddressOf Me.AllegatiGridView_ItemDataBound)
            AddHandler CType(e.Item.FindControl("AllegatiGridView"), RadGrid).ItemCommand, New GridCommandEventHandler(AddressOf Me.AllegatiGridView_ItemCommand)
        End If

        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf EmailGridView_ItemPreRender
        End If

        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If

    End Sub

    'Evento NeedDataSource associato alla Griglia AllegatiGridView. Effettua la mappatura tra AllegatiGridView e la relativa lista degli allegati.
    Protected Sub AllegatiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Dim parentItem As Telerik.Web.UI.GridDataItem = CType(CType(CType(sender, Telerik.Web.UI.RadGrid).NamingContainer, Telerik.Web.UI.GridNestedViewItem).ParentItem, Telerik.Web.UI.GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("Id")
        Dim allegatiEmail As New ParsecPro.AllegatoEmailRepository
        CType(sender, Telerik.Web.UI.RadGrid).DataSource = allegatiEmail.GetView(id)
        allegatiEmail.Dispose()
    End Sub

    'Evento ItemCommand associato alla Griglia AllegatiGridView. Lancia i comandi associati alla Griglia AllegatiGridView.
    Protected Sub AllegatiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        If e.CommandName = "Preview" Then
            Me.DownloadFile(e.Item)
        End If
    End Sub

    'Evento ItemDataBound associato alla Griglia AllegatiGridView. Setta i tootip e la visibilità di alcuni campi in vase al contenuto delle celle della Griglia AllegatiGridView. 
    Protected Sub AllegatiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim allegato As ParsecPro.AllegatoEmail = CType(e.Item.DataItem, ParsecPro.AllegatoEmail)
            If String.IsNullOrEmpty(allegato.Nomefile) Then
                dataItem("Preview").Controls(0).Visible = False
            Else
                dataItem("Preview").Controls(0).Visible = True
                If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                    btn = CType(dataItem("Preview").Controls(0), ImageButton)
                    btn.ToolTip = "Apri/Salva allegato"
                End If
            End If
        End If
    End Sub

    'Effettuta il Download delle Email. Richiamato da EmailGridView.ItemCommand
    Private Sub DownloadEmail(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idEmail As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim emails As New ParsecPro.EmailRepository
        Dim email As ParsecPro.Email = emails.GetQuery.Where(Function(c) c.Id = idEmail).FirstOrDefault
        If Not email Is Nothing Then
            Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & email.PercorsoRelativo & email.NomeFileEml
            Dim file As New IO.FileInfo(mailBoxPath)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("L'email selezionata non esiste!", False)
            End If
        End If
        emails.Dispose()
    End Sub

    'Effettua il Download del File. Richiamato da AllegatiGridView_ItemCommand
    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idAllegato As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdAllegato")
        Dim allegati As New ParsecAdmin.DocumentiRepository
        Dim allegato As ParsecAdmin.Documento = allegati.GetQuery.Where(Function(c) c.Id = idAllegato).FirstOrDefault
        If Not allegato Is Nothing Then
            Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti") & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFileOriginale
            Dim file As New IO.FileInfo(pathDownload)
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

    'Effettua una ricerca su DB delle Email in base alla DataInvioInizio, alla data DataInvioFine e all'Utente
    Private Function GetEmail() As List(Of ParsecPro.Email)
        Dim emails As New ParsecPro.EmailRepository
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim emailFiltro As New ParsecPro.EmailFiltro
        emailFiltro.DataInvioInizio = Me.DataInvioInizioTextBox.SelectedDate
        emailFiltro.DataInvioFine = Me.DataInvioFineTextBox.SelectedDate
        emailFiltro.UtenteCollegato = utente
        Dim res As List(Of ParsecPro.Email) = emails.GetView(emailFiltro)
        emails.Dispose()
        Return res
    End Function

    'Evento associato alla Griglia EmailGridView. Costruisce la lista degli Id delle mail selezionate.
    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedItems()
    End Sub

    'Evento associato alla Griglia EmailGridView. Gestisce i checkbox.
    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In EmailGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
    End Sub

    'Spedisce una mail. Richiamato da EmailGridView.ItemCommand
    Private Sub SendEmail(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim emails As New ParsecPro.EmailRepository
        Dim email As ParsecPro.Email = emails.GetById(id)
        If Not email Is Nothing Then
            Me.InviaEmail(email, emails)
            If Me.message.Length > 0 Then
                ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
                Me.message.Clear()
            End If
        End If
        emails.Dispose()
    End Sub

    'Configura l'SMTP per l'invio delle Email.
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
            Throw New ApplicationException("Si è verificato il seguente errore: " & ex.Message)
        End Try
        Return client
    End Function

    'Verifica se un indirizzo mail è valido.
    Private Function CheckEmail(ByVal Indirizzo As String) As Boolean
        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
        Dim m = emailRegex.Match(Indirizzo)
        Return m.Success
    End Function

    'Invia una mail. Richiamato da SendEmail.
    Private Sub InviaEmail(ByVal email As ParsecPro.Email, ByVal emails As ParsecPro.EmailRepository)
        Dim inviata As Boolean = False
        Try

            Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")

            Dim casellePec As New ParsecAdmin.ParametriPecRepository
            Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetByIdAssociatoUtente(email.IdCasella)
            casellePec.Dispose()

            If Not casellaPec Is Nothing Then

                Dim client As Rebex.Net.Smtp = Me.ConfigureSmtp(casellaPec)
                Dim mail As New Rebex.Mail.MailMessage
                Dim mailAttach As Rebex.Mail.Attachment = Nothing

                mail.From = casellaPec.Email

                Dim listaEmail = email.Destinatari.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)

                For Each s In listaEmail
                    If Me.CheckEmail(s) Then
                        If Not mail.To.Contains(s) Then
                            mail.To.Add(s)
                        End If
                    End If
                Next

                mail.Subject = email.Oggetto
                mail.BodyText = email.Corpo
                mail.Priority = Rebex.Mail.MailPriority.High

                Dim allegatiEmail As New ParsecPro.AllegatoEmailRepository
                Dim allegati As List(Of ParsecPro.AllegatoEmail) = allegatiEmail.GetView(email.Id)
                allegatiEmail.Dispose()

                For Each allegato As ParsecPro.AllegatoEmail In allegati
                    Dim fullPath As String = percorsoRoot & allegato.Nomefile
                    If IO.File.Exists(fullPath) Then
                        mailAttach = New Rebex.Mail.Attachment(fullPath)
                        mailAttach.FileName = IO.Path.GetFileName(fullPath)
                        mail.Attachments.Add(mailAttach)
                    End If
                Next

                client.Timeout = 0
                client.Send(mail)
                inviata = True

                client.Disconnect()

            End If

        Catch ex As Exception
            Me.message.AppendLine(ex.Message)
        Finally
            If inviata Then
                Me.message.AppendLine("L'e-mail indirizzata a '" & email.Destinatari & "' è stata inviata correttamente!")
                Try
                    '************************************
                    'Aggiorno l'e-mail
                    '************************************
                    email.Inviata = True
                    email.DataInvio = Now
                    emails.Save(email)

                    '************************************
                    'Aggiorno la griglia.
                    '************************************
                    Me.Emails = Nothing
                    Me.EmailGridView.Rebind()

                Catch ex As Exception
                    Me.message.AppendLine(ex.Message)
                End Try
            End If
        End Try

    End Sub

    'Elimina una mail. Richiamato da EmailGridView.ItemCommand.
    Private Sub EliminaEmail(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        Dim emails As New ParsecPro.EmailRepository
        Dim mail As ParsecPro.Email = emails.GetById(id)

        If Not mail Is Nothing Then
            Dim message As String = "L'e-mail indirizzata a " & mail.Destinatari & " del " & String.Format("{0:dd/MM/yyyy}", mail.DataInvio) & " è stata cancellata con successo!"
            Try

                emails.Delete(mail, utente)
                Me.Emails = Nothing
                Me.EmailGridView.Rebind()


            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Impossibile cancellare l'e-mail selezionata, per il seguente errore:" & vbCrLf & ex.Message, False)
            Finally
                ParsecUtility.Utility.MessageBox(message, False)
                emails.Dispose()
            End Try

        End If
    End Sub

    'Crea una lista di Id selezionati nella griglia EmailGridView.
    Private Sub SaveSelectedItems()
        For Each item As GridItem In Me.EmailGridView.Items
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


#End Region

#Region "AZIONI PANNELLO FILTRO"

    'Resetta il filtro per la ricerca. Richiamto da AnnullaFiltroImageButton.Click
    Private Sub ResettaFiltro()
        Me.EmailInviateComboBox.SelectedIndex = 0
        For Each col As GridColumn In Me.EmailGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.EmailGridView.MasterTableView.FilterExpression = String.Empty

        Me.DataInvioInizioTextBox.SelectedDate = New Date(Now.Year, Now.Month, 1)
        Me.DataInvioFineTextBox.SelectedDate = New Date(Now.Year, Now.Month, 1).AddMonths(1).AddDays(-1)

        Me.Emails = Nothing
        Me.EmailGridView.Rebind()
    End Sub

    'Filtra le Email. Effettua la ricerca. Richiaamto da FiltraImageButton.Click
    Private Sub FiltraEmails()
        Dim emails As New ParsecPro.EmailRepository
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim emailFiltro As New ParsecPro.EmailFiltro
        If Me.EmailInviateComboBox.SelectedIndex <> 0 Then
            emailFiltro.Inviata = CBool(Me.EmailInviateComboBox.SelectedValue)
        End If
        emailFiltro.DataInvioInizio = Me.DataInvioInizioTextBox.SelectedDate
        emailFiltro.DataInvioFine = Me.DataInvioFineTextBox.SelectedDate
        emailFiltro.UtenteCollegato = utente
        Me.Emails = emails.GetView(emailFiltro)
        emails.Dispose()
        Me.EmailGridView.Rebind()
    End Sub

    'Evento click che lancia il filtraggio delle mail.
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FiltraImageButton.Click
        Me.FiltraEmails()
    End Sub

    'Evento click che resetta il Filtro per la ricerca.
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

#End Region

    'Abilita o meno il pulsante di invio massivo delle mail
    Private Sub VerificaInvioMassivo()
        Dim emailNonInviate As List(Of ParsecPro.Email) = Me.Emails.Where(Function(c) c.Inviata = False).ToList
        Me.InvioMassivoImageButton.Enabled = emailNonInviate.Count > 0
    End Sub

    'Abilita o disabilita i pulsanti di invio massivo delle mail e di esportazione in excel.
    Private Sub ImpostaUI(ByVal abilita As Boolean)
        Me.InvioMassivoImageButton.Enabled = abilita
        Me.EsportaExcelImageButton.Enabled = abilita
    End Sub

    'Evento click che fa partire l'invio massivo.
    Protected Sub InvioMassivoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles InvioMassivoImageButton.Click
        Me.EseguiInvioEmailMassivo()
    End Sub

    'Lancia l'invio massivo delle mail. Richiamato da InvioMassivoImageButton.Click
    Private Sub EseguiInvioEmailMassivo()
        Dim emails As New ParsecPro.EmailRepository
        If Me.EmailGridView.SelectedItems.Count = 0 Then
            Me.message.AppendLine("E' necessario selezionare almeno un e-mail!")
        Else
            For Each item As GridDataItem In Me.EmailGridView.SelectedItems

                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim email As ParsecPro.Email = emails.GetById(id)

                If Not email Is Nothing Then
                    If Not email.Inviata Then
                        Me.InviaEmail(email, emails)
                    End If
                End If
            Next
        End If
        If Me.message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
            Me.message.Clear()
        End If
        emails.Dispose()
    End Sub

    'Evento click che esegue la esportazione . excel della griglia.
    Protected Sub EsportaExcelImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EsportaExcelImageButton.Click
        Me.EseguiEsportazioneExcel()
    End Sub

    'Esegue la esportazione in excel della griglia. Richiamato da EsportaExcelImageButton.Click
    Private Sub EseguiEsportazioneExcel()
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = "Email_UT_" & utente.Id.ToString & "_AL_" & String.Format("{0:dd_MM_yyyy_HHmmss}", Now) & ".xls"
        Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As String = String.Empty
        line &= "Mittente;Destinatario;Oggetto;Corpo;Data invio;Inviata"
        swExport.WriteLine(line)
        line = ""
        For Each email As ParsecPro.Email In Me.Emails
            Dim data As String = String.Format("{0:dd/MM/yyyy}", email.DataInvio)
            Dim inviata As String = If(email.Inviata, "Si", "No")
            line &= email.EmailMittente & ";" & email.Destinatari & ";" & email.Oggetto & ";" & email.Corpo & ";" & data & ";" & inviata
            swExport.WriteLine(line)
            line = ""
        Next
        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

        Dim esportazioniExcel As New ParsecAdmin.ExportExcelRepository
        Dim exportExcel As ParsecAdmin.ExportExcel = esportazioniExcel.CreateFromInstance(Nothing)
        exportExcel.NomeFile = exportFilename
        exportExcel.Oggetto = "Elenco e-mails"
        exportExcel.Utente = utente.Username
        exportExcel.Data = Now
        esportazioniExcel.Save(exportExcel)
    End Sub


End Class