
Partial Class VerificaPasswordPage
    Inherits System.Web.UI.Page

    Protected Sub rbtnInserisciPass_Click(sender As Object, e As System.EventArgs) Handles rbtnInserisciPass.Click
        Dim esito As String = String.Empty
        esito = VerificaPassword(rtxtPassword.Text)
        If Not CBool(esito.Length) Then
            'Dim codicePratica As String = Request.QueryString("CodicePratica")
            ''Chiudo la finestra
            'ScriptHolder.Controls.Clear()
            'ScriptHolder.Controls.Add(New LiteralControl("<script type='text/javascript'>Close(" & codicePratica & ")</" + "script>"))
            ParsecUtility.Utility.CloseRadWindowAndUpadateParent(False)
        Else
            ParsecUtility.Utility.MessageBox(esito, False)
        End If

    End Sub

    Private Function VerificaPassword(ByVal password As String) As String
        Dim userRepository As New ParsecAdmin.UserRepository
        Try
            Dim userName As String = GetUsernameSblocco()
            Dim utente As ParsecAdmin.Utente = userRepository.Authenticate(userName, password)
            If Not utente Is Nothing Then
                Return String.Empty
            Else
                Throw New ApplicationException("La Password inserita non è corretta. Si prega di riprovare.")
            End If

        Catch ex As Exception
            Return ex.Message
        Finally
            userRepository.Dispose()
        End Try

    End Function

    Private Function GetUsernameSblocco() As String
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("UtentePwdSblocco", ParsecAdmin.TipoModulo.SEP)
        parametri.Dispose()
        Return If(parametro Is Nothing, String.Empty, parametro.Valore)
    End Function
End Class
