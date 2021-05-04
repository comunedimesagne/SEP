
Partial Class UploadPage
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click


        If Me.AllegatoUpload.UploadedFiles.Count > 0 Then

            Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles(0)
            If file.FileName <> "" Then
                Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento
                Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDownload") & Session.SessionID & "_" & file.FileName
                file.SaveAs(pathDownload)
                documentoFascicolo.IdDocumento = -1
                documentoFascicolo.NomeDocumento = file.FileName
                documentoFascicolo.TipoDocumento = 1  'Documento Generico
                documentoFascicolo.DescrizioneTipoDocumento = Me.GetImagePath(file.FileName)
                documentoFascicolo.DataDocumento = Now

                Session("DocumentoFascicoloSelezionato") = documentoFascicolo
            End If
        End If
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

    Private Function GetImagePath(ByVal filename As String) As String
        Dim estensione As String = IO.Path.GetExtension(filename)
        Select Case estensione
            Case "pdf"
                Return "~/sep/images/ext/pdf.gif"
            Case "xls"
                Return "~/sep/images/ext/xls.png"
            Case "doc"
                Return "~/sep/images/ext/doc.jpg"
            Case "ppt"
                Return "~/sep/images/ext/ppoint.png"
            Case "txt"
                Return "~/sep/images/ext/txt.jpg"
            Case Else
                Return "~/sep/images/ext/docum.png"
        End Select
    End Function

End Class
