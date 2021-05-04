
Partial Class ExportExcelPage
    Inherits System.Web.UI.Page


    Private Sub DownloadFile()
        Dim fullPath As String = Session("AttachmentFullName")
        Session("AttachmentFullName") = Nothing
        Dim file As New IO.FileInfo(fullPath)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
        Response.AddHeader("Content-Length", file.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.TransmitFile(fullPath)
        'Response.WriteFile(fullPath)
        Response.End()
      
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("AttachmentFullName") Is Nothing Then
            Me.DownloadFile()
        End If


    End Sub

End Class
