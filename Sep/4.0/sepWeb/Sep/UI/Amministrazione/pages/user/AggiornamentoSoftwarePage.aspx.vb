Imports ParsecAdmin


Partial Class AggiornamentoSoftwarePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Aggiornamento Software"
        Dim script As String = RegistraParsecUpdater()
        Me.RegisterComponent(script)
    End Sub

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click

        Dim password = WebConfigSettings.GetKey("DocumentServerPwd")
        password = ParsecCommon.CryptoUtil.Decrypt(password)
        Dim user = WebConfigSettings.GetKey("DocumentServerUser")
        Dim port = WebConfigSettings.GetKey("DocumentServerPort")
        Dim host = WebConfigSettings.GetKey("DocumentServerUrl").Split(New Char() {"//"}, StringSplitOptions.RemoveEmptyEntries)(1)
        Dim destination = WebConfigSettings.GetKey("PathParsecClient").Replace("\", "/")

        Dim parsecScanPlusZip = WebConfigSettings.GetKey("PathRelativeParseScanPlus")
        Dim parsecOpenOfficeZip = WebConfigSettings.GetKey("PathRelativeParsecOpenOffice")
        Dim parsecDigitalSignZip = WebConfigSettings.GetKey("PathRelativeParsecDigiltalSign")
        Dim parsecPrintingZip = WebConfigSettings.GetKey("PathRelativeParsecPrinting")

        Dim data As String = "<information>"
        data &= "<port>" & port & "</port>"
        data &= "<user>" & user & "</user>"
        data &= "<password>" & password & "</password>"
        data &= "<host>" & host & "</host>"
        data &= "<destination>" & destination & "</destination>"

        data &= "<detail>"
        data &= "<url>" & parsecScanPlusZip & "</url>"
        data &= "</detail>"

        data &= "<detail>"
        data &= "<url>" & parsecOpenOfficeZip & "</url>"
        data &= "</detail>"

        data &= "<detail>"
        data &= "<url>" & parsecDigitalSignZip & "</url>"
        data &= "</detail>"



        data &= "<detail>"
        data &= "<url>" & parsecPrintingZip & "</url>"
        data &= "</detail>"

        data &= "</information>"

        RegistraTimerEseguiVerificaAggiornamento(data, NotificaAggiornamentoButton.ClientID, False)

    End Sub

    Public Shared ComponentName As String = "Updater"


    Public Shared Function RegistraParsecUpdater() As String
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim versioneJava As String = ParsecAdmin.WebConfigSettings.GetKey("DefaultVersioneJava")
        If Not String.IsNullOrEmpty(utenteCollegato.VersioneJava) Then
            versioneJava = utenteCollegato.VersioneJava
        End If
        versioneJava = versioneJava.Replace(".", "").Trim
        Dim script As String = "<applet id='" & ComponentName & "' name='" & ComponentName & "'  code='ParsecUpdater.class'  archive='" & String.Format(ParsecAdmin.WebConfigSettings.GetKey("PathParsecUpdater"), versioneJava) & "' width=20 height=20 mayscript  style='display:block'><param name = 'Image' value = 'Download.png'><param name = 'initial_focus' value = 'false'><param name = 'BackColor' value = 'BFDBFF'></applet>"
        Return script
    End Function


    Public Shared Sub RegistraTimerEseguiVerificaAggiornamento(ByVal data As String, ByVal buttonToClick As String, ByVal sincrono As Boolean)
        Dim script As New System.Text.StringBuilder
        script.AppendLine("<script language='Javascript'>")

        script.AppendLine("var id = '';")
        script.AppendLine("function CheckForUpdates(uploadButtonId) {")

        script.AppendLine("try")
        script.AppendLine("{")

        script.AppendLine("     if ((document.Updater.OutputData() != null) && (document.Updater.OutputData() != '')) {")
        script.AppendLine("             window.clearInterval(id);")
        script.AppendLine("             id = '';")
        ' script.AppendLine("             alert('FATTO');")
        script.AppendLine("             document.Updater.Reset();")

        If Not String.IsNullOrEmpty(buttonToClick) Then
            script.AppendLine("             document.getElementById(uploadButtonId).click();")
        End If

        script.AppendLine("     }")

        script.AppendLine("}")
        script.AppendLine("catch(e){}")
        script.AppendLine("finally{}")


        script.AppendLine("}")


        script.AppendLine("try")
        script.AppendLine("{")
        script.AppendLine("document.Updater.Update(" & Chr(34) & data & Chr(34) & ");")
        script.AppendLine("var id=window.setInterval(""CheckForUpdates('" & buttonToClick & "') "",100);")
        script.AppendLine("}")
        script.AppendLine("catch(e)")
        script.AppendLine("{")
        script.AppendLine("alert(e.message);")
        script.AppendLine("}")
        script.AppendLine("finally")
        script.AppendLine("{")
        script.AppendLine("}")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, sincrono)
    End Sub


    Public Sub RegisterComponent(ByVal script As String)
        Dim cell As New TableCell
        cell.Width = New Unit(30)
        cell.Controls.Add(New LiteralControl(script))
        Me.componentPlaceHolder.Rows(0).Cells.Add(cell)
    End Sub


    Protected Sub NotificaAggiornamentoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NotificaAggiornamentoButton.Click
        Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
    End Sub

End Class