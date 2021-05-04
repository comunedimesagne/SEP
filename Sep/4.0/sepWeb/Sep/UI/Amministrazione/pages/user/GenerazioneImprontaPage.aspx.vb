Imports ParsecAdmin


Partial Class GenerazioneImprontaPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

  
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Generazione Impronta"

        Me.TipologiaHashComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("SHA-1", "0"))
        Me.TipologiaHashComboBox.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem("SHA-256", "1"))
        'Me.TipologiaHashComboBox.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem("ALTRE", "2"))
    End Sub


    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        If Me.AllegatoUpload.UploadedFiles.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un file!", False)
            Exit Sub
        Else
            Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles(0)
            If Not String.IsNullOrEmpty(file.FileName) Then


                Dim attachmentBytes As Byte() = New Byte(file.InputStream.Length - 1) {}
                file.InputStream.Read(attachmentBytes, 0, attachmentBytes.Length)


                Dim impronta As Byte() = Nothing
                If Me.TipologiaHashComboBox.SelectedIndex = 0 Then
                    impronta = ParsecUtility.Utility.CalcolaHash(attachmentBytes)
                Else
                    impronta = ParsecUtility.Utility.CalcolaHash256(attachmentBytes)
                End If
                Me.ImprontaTextBox.Text = BitConverter.ToString(impronta).Replace("-", "")

            End If
        End If
    End Sub

End Class