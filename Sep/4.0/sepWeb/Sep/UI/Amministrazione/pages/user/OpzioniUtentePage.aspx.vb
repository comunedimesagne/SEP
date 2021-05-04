Imports ParsecAdmin


Partial Class OpzioniUtentePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage



    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("pswexp") Is Nothing Then
            Me.MasterPageFile = "~/BasePage.master"
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.Request("pswexp") Is Nothing Then
            MainPage = CType(Me.Master, MainPage)
            MainPage.NomeModulo = "Amministrazione"
            MainPage.DescrizioneProcedura = "> Impostazioni Utente"
        Else
            Me.MessaggioLabel.Visible = True
        End If

        Dim moduleRepository As New ParsecAdmin.ModuleRepository
        Dim moduli As List(Of ParsecAdmin.Modulo) = moduleRepository.GetAbilitazioniModuli()
        Me.ModuloComboBox.DataTextField = "Descrizione"
        Me.ModuloComboBox.DataValueField = "Id"
        Me.ModuloComboBox.DataSource = moduli
        Me.ModuloComboBox.DataBind()
        moduleRepository.Dispose()

        Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Me.ModuloComboBox.FindItemByValue(utenteCorrente.IdModuloDefault).Selected = True

     
    End Sub


    Private Function ControllaPassword(ByVal passwordUtente As Byte()) As Boolean
        Dim res As Boolean = False
        Dim password As Byte() = ParsecUtility.Utility.CalcolaHash(Me.PasswordCorrenteTextBox.Text)
        If passwordUtente.SequenceEqual(password) Then
            res = True
        End If
        Return res
    End Function



    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Dim message As String = String.Empty
        Try
            Me.Convalida()
            Dim utente As ParsecAdmin.Utente = Nothing
            Try
                Dim utenti As New ParsecAdmin.UserRepository
                Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
                utente = utenti.GetUserById(utenteCorrente.Id).FirstOrDefault
                utente.PswNonSettata = False
                utente.DataUltimoSettaggioPsw = Now
                utente.IdModuloDefault = CInt(Me.ModuloComboBox.SelectedItem.Value)
                utente.PasswordHash = ParsecUtility.Utility.CalcolaHash(Me.NuovaPasswordTextBox.Text)
                utenti.SaveChanges()
                utenti.Dispose()
            Catch ex As Exception
                message = ex.Message
            End Try
        Catch ex As Exception
            message = ex.Message
        End Try

        If String.IsNullOrEmpty(message) Then
            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo! " & vbCrLf & " Riaccedere al Sistema con la nuova password!."
            ParsecUtility.Utility.ButtonClick(RedirectImageButton.ClientID, False)
        Else
            ' Me.infoOperazioneHidden.Value = message
            ParsecUtility.Utility.MessageBox(message, False)
        End If
    End Sub

    Private Sub Convalida()
        Dim message As String = ""
        Dim lunghezzaMinima As Integer = 5
        Dim parametriRepository As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametriRepository.GetByName("numCaratteriPswUtente")
        If Not parametro Is Nothing Then
            lunghezzaMinima = CInt(parametro.Valore)
        End If
        parametriRepository.Dispose()

        If String.IsNullOrEmpty(Me.NuovaPasswordTextBox.Text) OrElse String.IsNullOrEmpty(Me.ConfermaPasswordTextBox.Text) Then
            message = "E' necessario inserire la password in entrambi i campi!"
            Throw New ApplicationException(message)
        Else
            If Me.NuovaPasswordTextBox.Text <> Me.ConfermaPasswordTextBox.Text Then
                message = "E' necessario inserire lo stesso valore per la password in entrambi i campi!"
                Throw New ApplicationException(message)
            End If
        End If
        If ParsecUtility.Utility.CheckPassword(Me.NuovaPasswordTextBox.Text) Then
            message = "La password inserita non è valida!"
            Throw New ApplicationException(message)
        End If
        If Me.NuovaPasswordTextBox.Text.Length < lunghezzaMinima Then
            message = "La password non può essere inferiore a " & CStr(lunghezzaMinima) & " caratteri!"
            Throw New ApplicationException(message)
        End If
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        If Not utenteCollegato Is Nothing Then
            If utenteCollegato.Username = Me.NuovaPasswordTextBox.Text Then
                message = "La password specificata non può essere uguale allo username dell'utente!"
                Throw New ApplicationException(message)
            End If
            If Not ControllaPassword(utenteCollegato.PasswordHash) Then
                message = "Nel campo 'Password corrente' è necessario digitare la password di accesso attualmente in uso!"
                Throw New ApplicationException(message)
            End If
        End If
        Dim userRepository As New ParsecAdmin.UserRepository
        If userRepository.PasswordGiaUsata(utenteCollegato.Id, Me.NuovaPasswordTextBox.Text) Then
            message = "La password specificata è stata già utilizzata dall'utente!"
            Throw New ApplicationException(message)
        End If
        userRepository.Dispose()


    End Sub


    Protected Sub RedirectImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RedirectImageButton.Click
        Response.Redirect("~/Login.aspx")
    End Sub

End Class