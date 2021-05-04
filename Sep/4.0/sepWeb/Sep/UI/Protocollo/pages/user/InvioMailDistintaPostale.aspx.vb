Imports Telerik.Web.UI
Imports System.Web.Mail

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class InvioMailDistintaPostale
    Inherits System.Web.UI.Page

    'Classe di Appoggio riportante le informazioni necessarie della Distinta Postale
    Public Class ElementoDistintaPostale

        Property IdRegistrazione As String
        Property NumeroProtocollo As String
        Property DataProtocollo As Date
        Property Oggetto As String

        Property dimensioneAllegati As Decimal

    End Class

    'Classe di Apppggio rappresentante la mail da inviare.
    Public Class MailToSend

        Property mail As MailMessage
        Property dimensioneAllegati As Decimal
        Property listaRegistrazioniProtocollo As String

    End Class

    'Messaggio
    Private message As New StringBuilder

    'Dimensione massima degli Allegati (in MByte)
    Private sogliaAllegati As Decimal = 20

#Region "PROPRIETA'"

    'Variabile di Sessione: lista delle Distinte Postali su cui è mappata la griglia
    Private Property ListaElementiDistintaPostale() As List(Of ElementoDistintaPostale)
        Get
            Return CType(Session("InvioMailDistintaPostale_ListaProtocolli"), List(Of ElementoDistintaPostale))
        End Get
        Set(ByVal value As List(Of ElementoDistintaPostale))
            Session("InvioMailDistintaPostale_ListaProtocolli") = value
        End Set
    End Property

    'Variabile di Sessione: lista delle Mail da inviare.
    Private Property ListaMailToSend() As List(Of MailToSend)
        Get
            Return CType(Session("InvioMailDistintaPostale_ListaMailToSend"), List(Of MailToSend))
        End Get
        Set(ByVal value As List(Of MailToSend))
            Session("InvioMailDistintaPostale_ListaMailToSend") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.TitleLabel.Text = "Invia Mail Distinta Postale"

        If Not Me.Page.IsPostBack Then
            Me.CaricaMittenti()
            Me.chkMailDistinte.Checked = True

            Me.ListaElementiDistintaPostale = Me.GetDistintePostaliPerMail(ParsecUtility.SessionManager.FiltroRegistrazioneProtocollo)

            Me.grigliaProtocolli.DataSource = Me.ListaElementiDistintaPostale
            Me.grigliaProtocolli.DataBind()

            Me.ListaMailToSend = New List(Of MailToSend)
        End If

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = Nothing
        parametro = parametri.GetByName("LimiteDimensioneAllegatiPEC")
        sogliaAllegati = 20
        If (Not parametro Is Nothing) Then
            sogliaAllegati = parametro.Valore
        End If

    End Sub

#End Region

#Region "GRIGLIA"

    'Evento associato alla griglia grigliaProtocolli per gestire i checkbox
    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    'Evento associato alla griglia grigliaProtocolli per gestire i checkbox
    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In grigliaProtocolli.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

    'Evento ItemCreated associato alla Griglia UtentiGridView. Definisce l' ItemPreRender e definisce lo stile per gli Item di tipo GridHeaderItem.
    Protected Sub grigliaProtocolli_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grigliaProtocolli.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    'Evento NeedDataSource associato alla griglia grigliaProtocolli. Aggancia il datasource della griglia alla lista ListaElementiDistintaPostale (variabile di sessione).
    Protected Sub grigliaProtocolli_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grigliaProtocolli.NeedDataSource
        Me.grigliaProtocolli.DataSource = Me.ListaElementiDistintaPostale
    End Sub

    'Evento ItemDataBound associato alla Griglia grigliaProtocolli. Calcola la dimensione totale degli allegati di uogno registrazione e la setta nella cella "dimensioneAllegati".
    Protected Sub grigliaProtocolli_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grigliaProtocolli.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim elemento As ElementoDistintaPostale = CType(e.Item.DataItem, ElementoDistintaPostale)

            Dim registrazioniRepository As New ParsecPro.RegistrazioniRepository

            Dim registrazioneProtocollo = Me.ListaElementiDistintaPostale.Where(Function(w) w.IdRegistrazione = elemento.IdRegistrazione).FirstOrDefault
            Dim infoReader As System.IO.FileInfo
            Dim dimensioneAllegatiTotali As Decimal = 0.0

            If (Not registrazioneProtocollo Is Nothing) Then
                Dim allegati = registrazioniRepository.GetById(elemento.IdRegistrazione).Allegati
                Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")

                If allegati.Count() > 0 Then
                    'trovo gli allegati e li allego nella mail
                    For Each allegato As ParsecPro.Allegato In allegati
                        Dim filename As String = allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
                        Dim fullPath As String = percorsoRoot & allegato.PercorsoRelativo & filename
                        If IO.File.Exists(fullPath) Then
                            infoReader = My.Computer.FileSystem.GetFileInfo(fullPath)
                            dimensioneAllegatiTotali = dimensioneAllegatiTotali + ((infoReader.Length / 1024)/1024)

                        End If
                    Next
                End If
            End If

            Me.ListaElementiDistintaPostale.Where(Function(w) w.IdRegistrazione = elemento.IdRegistrazione).FirstOrDefault().dimensioneAllegati = Math.Round(dimensioneAllegatiTotali, 2)
            CType(dataItem("dimensioneAllegati"), TableCell).Text = Math.Round(dimensioneAllegatiTotali, 2).ToString

            registrazioniRepository.Dispose()

        End If

    End Sub

#End Region

#Region "METODI PRIVATI"

    'Ricerca e Restituisce una lista di Distinte Postali
    Private Function GetDistintePostaliPerMail(ByVal filtro As ParsecPro.RegistrazioneFiltro) As List(Of ElementoDistintaPostale)

        Dim registrazioneRepository As New ParsecPro.RegistrazioniRepository

        Dim view = From registrazione In registrazioneRepository.GetQuery
                   Order By registrazione.NumeroProtocollo
                   Select registrazione


        view = view.Where(Function(c) c.Modificato = False And c.TipoRegistrazione = 1)

        '***************************************************************************************************
        'Filtro Numero Protocollo
        '***************************************************************************************************
        If filtro.NumeroProtocolloInizio.HasValue Then
            view = view.Where(Function(c) c.NumeroProtocollo >= filtro.NumeroProtocolloInizio)
        End If
        If filtro.NumeroProtocolloFine.HasValue Then
            view = view.Where(Function(c) c.NumeroProtocollo <= filtro.NumeroProtocolloFine)
        End If
        '***************************************************************************************************

        '***************************************************************************************************
        'Filtro Tipologia Invio
        '***************************************************************************************************
        If filtro.ElencoId.Count > 0 Then
            view = view.Where(Function(c) If(Not c.IdTipoRicezione Is Nothing, filtro.ElencoId.Contains(c.IdTipoRicezione), True))
        End If

        '***************************************************************************************************

        '***************************************************************************************************
        'Filtro Data Protocollo
        '***************************************************************************************************
        If filtro.DataProtocolloInizio.HasValue Then
            Dim dataInizio = filtro.DataProtocolloInizio.Value
            Dim startDate As Date = New Date(dataInizio.Year, dataInizio.Month, dataInizio.Day, 0, 0, 0)

            view = view.Where(Function(c) c.DataImmissione >= startDate)
        End If
        If filtro.DataProtocolloFine.HasValue Then
            Dim dataFine = filtro.DataProtocolloFine.Value
            Dim endDate As Date = New Date(dataFine.Year, dataFine.Month, dataFine.Day, 23, 59, 59)
            view = view.Where(Function(c) c.DataImmissione <= endDate)
        End If


        Dim listaToReturn = From elemento In view.AsEnumerable
                            Select New ElementoDistintaPostale With {
                                .IdRegistrazione = elemento.Id,
                                .NumeroProtocollo = elemento.NumeroProtocollo,
                                .DataProtocollo = elemento.DataImmissione,
                                .Oggetto = elemento.Oggetto
                            }

        Return listaToReturn.ToList

    End Function

    'Carica i mittenti nella combobox MittenteComboBox.
    Private Sub CaricaMittenti()
        Dim parametriPec As New ParsecAdmin.ParametriPecRepository
        Dim pec = parametriPec.GetQuery
        Me.MittenteComboBox.DataSource = pec
        Me.MittenteComboBox.DataTextField = "Email"
        Me.MittenteComboBox.DataValueField = "id"
        Me.MittenteComboBox.DataBind()
        Me.MittenteComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Selezionare Il Mittente -", "0"))
        Me.MittenteComboBox.SelectedIndex = 0
    End Sub

    'Verifica la validità dei dati inseriti.
    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder

        If (Me.MittenteComboBox.SelectedIndex = 0) Then
            message.AppendLine("Occorre Selezionare il Mittente!")
        End If

        If (String.IsNullOrEmpty(Me.txtDestinatario.Text.Trim)) Then
            message.AppendLine("Occorre Selezionare il Destinatario!")
        End If

        If message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If

        Return message.Length = 0
    End Function

    'Resetta i campi oggetto, destinatario e Mittente
    Private Sub ResettaVista()

        Me.OggettoTextBox.Text = String.Empty
        Me.txtDestinatario.Text = String.Empty
        Me.MittenteComboBox.SelectedIndex = 0

    End Sub

    'Verifica i dati e se tutto ok invia la mail
    Protected Sub InviaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles InviaButton.Click

        If (Me.VerificaDati()) Then
            Me.InviaEmail()
        End If

    End Sub

    'Invia la mail
    Private Sub InviaEmail()
        Me.ListaMailToSend = New List(Of MailToSend)

        Dim registrazioniRepository As New ParsecPro.RegistrazioniRepository

        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")

        Dim dimensioneAllegatiTotali As Decimal = 0.0
        Dim listaRegistrazioniIncluse As String = ""

        Dim mail As MailMessage
        mail = Me.ConfiguraCdoEmail()
        mail.To = Me.txtDestinatario.Text
        mail.Subject = If(Me.OggettoTextBox.Text.Trim = String.Empty, "INVIO DISTINTE POSTALI", Me.OggettoTextBox.Text)
        mail.Body = ""

        For Each item As GridDataItem In Me.grigliaProtocolli.Items
            Dim selectCheckBox As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)
            If selectCheckBox.Checked Then

                If (Me.chkMailDistinte.Checked) Then
                    'devo ricreare la mail se ne devo mandare n distinte
                    mail = Me.ConfiguraCdoEmail()
                    mail.To = Me.txtDestinatario.Text
                    mail.Subject = If(Me.OggettoTextBox.Text.Trim = String.Empty, "INVIO DISTINTE POSTALI", Me.OggettoTextBox.Text)
                    mail.Body = ""
                End If

                Dim idRegistrazione As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdRegistrazione")

                Dim registrazioneProtocollo = Me.ListaElementiDistintaPostale.Where(Function(w) w.IdRegistrazione = idRegistrazione).FirstOrDefault

                If (Not registrazioneProtocollo Is Nothing) Then
                    Dim allegati = registrazioniRepository.GetById(idRegistrazione).Allegati

                    mail.Body = mail.Body & vbCrLf & "Protocollo N. " & registrazioneProtocollo.NumeroProtocollo & " del " & registrazioneProtocollo.DataProtocollo.ToShortDateString & " - Oggetto: " & registrazioneProtocollo.Oggetto & " " & vbCrLf '& " Destinatario " & registrazioneProtocollo.Destinatario & " " & vbCrLf

                    If allegati.Count() > 0 Then
                        'trovo gli allegati e li allego nella mail
                        Dim mailAttach As MailAttachment
                        For Each allegato As ParsecPro.Allegato In allegati
                            Dim filename As String = allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
                            Dim fullPath As String = percorsoRoot & allegato.PercorsoRelativo & filename
                            If IO.File.Exists(fullPath) Then
                                mailAttach = New MailAttachment(fullPath)
                                mail.Attachments.Add(mailAttach)
                                mail.Body = mail.Body & "           Allegato " & allegato.NomeFile & " " & vbCrLf

                            End If
                        Next
                        dimensioneAllegatiTotali = registrazioneProtocollo.dimensioneAllegati
                    End If
                End If

                If (Me.chkMailDistinte.Checked) Then

                    Dim mailToSend As New MailToSend
                    mailToSend.mail = Mail
                    mailToSend.dimensioneAllegati = dimensioneAllegatiTotali
                    mailToSend.listaRegistrazioniProtocollo = registrazioneProtocollo.NumeroProtocollo & " del " & registrazioneProtocollo.DataProtocollo.ToShortDateString

                    Me.ListaMailToSend.Add(mailToSend)

                    dimensioneAllegatiTotali = 0.0
                Else
                    listaRegistrazioniIncluse = listaRegistrazioniIncluse & registrazioneProtocollo.NumeroProtocollo & " del " & registrazioneProtocollo.DataProtocollo.ToShortDateString & " - "
                End If


            End If
        Next

        If Not chkMailDistinte.Checked Then
            Dim mailToSend As New MailToSend
            mailToSend.mail = mail
            mailToSend.listaRegistrazioniProtocollo = listaRegistrazioniIncluse
            mailToSend.dimensioneAllegati = Math.Round(dimensioneAllegatiTotali, 2)
            Me.ListaMailToSend.Add(mailToSend)
        End If

        If ListaMailToSend.Count > 0 Then
            For Each m In ListaMailToSend
                invioEffettivo(m, m.dimensioneAllegati)
            Next

            If (Me.message.ToString = "") Then
                ParsecUtility.Utility.MessageBox("L'E-mail sono state inviate correttamente!", False)
            Else
                ParsecUtility.Utility.MessageBox("Registrazioni non inviate i cui allegati, in totale, superano la soglia: " & vbCrLf & Me.message.ToString, False)
            End If

        Else
            ParsecUtility.Utility.MessageBox("Non sono state selezionate Registrazioni di Protocollo!", False)
        End If
        
    End Sub

    'Invio effettivo e reale della mail
    Private Sub invioEffettivo(ByVal mailToSend As MailToSend, ByVal dimensioneAllegatiTotali As Decimal)
        If (dimensioneAllegatiTotali <= sogliaAllegati) Then
            Try
                SmtpMail.Send(mailToSend.mail)
            Catch ex As System.Web.HttpException
                Me.message.AppendLine(ex.Message)
            Catch ex As Exception
                Me.message.AppendLine(ex.Message)
            Finally

            End Try
        Else
            Me.message.AppendLine(mailToSend.listaRegistrazioniProtocollo & " [" & Math.Round(mailToSend.dimensioneAllegati, 2) & " MBytes ]")
        End If
    End Sub

    'Configurazione della mail. Richiamato da InviaEmail()
    Private Function ConfiguraCdoEmail() As MailMessage
        Const cdoBasic As Integer = 1
        Const cdoSendUsingPort As Integer = 2
        Dim mail As MailMessage = Nothing
        Dim casellePec As New ParsecAdmin.ParametriPecRepository
        Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetByIdAssociatoUtente(Me.MittenteComboBox.SelectedValue)
        If Not casellaPec Is Nothing Then
            mail = New MailMessage
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", casellaPec.SmtpServer)
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", casellaPec.SmtpPorta)
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort)
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic)
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", casellaPec.UserId)
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", casellaPec.Password)
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", casellaPec.SmtpIsSSL)
            mail.From = casellaPec.Email
            mail.BodyFormat = MailFormat.Text
            mail.Priority = MailPriority.High
            SmtpMail.SmtpServer = casellaPec.SmtpServer & ":" & casellaPec.SmtpPorta.ToString
        End If
        Return mail
    End Function


#End Region

End Class
