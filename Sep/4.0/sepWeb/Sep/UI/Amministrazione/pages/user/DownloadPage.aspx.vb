Imports System.Linq

Partial Class DownloadPage
    Inherits System.Web.UI.Page


    Private Sub DownloadFile()
        Dim fullPath As String = String.Empty

        If Not Page.Request("doc") Is Nothing Then
            fullPath = Page.Request("doc")
            Response.Redirect(fullPath)
        Else

            If Not Session("AttachmentFullName") Is Nothing Then

                If TypeOf Session("AttachmentFullName") Is Hashtable Then

                    Dim ht As Hashtable = CType(Session("AttachmentFullName"), Hashtable)
                    Dim contentType As String = "application/octet-stream"
                    Dim extension As String = ht("Extension")
                    Dim data = CType(ht("Content"), Byte())

                    If extension = ".pdf" Then

                        Response.Clear()

                        'Response.ClearContent()
                        'Response.ClearHeaders()
                        'Response.AddHeader("Content-Disposition", "attachment; filename=" & Uri.EscapeDataString(ht("Filename")))
                        'Response.AddHeader("Content-Length", data.Length.ToString())
                        'Response.AddHeader("Content-Type", contentType)

                        Dim browser = Request.Browser.Browser
                        If browser.ToLower.Contains("ie") Then
                            Response.ContentType = "application/octet-stream"


                        Else
                            Response.ContentType = "application/pdf"
                            'Response.ContentType = "application/octet-stream"


                        End If

                        Response.Cache.SetCacheability(HttpCacheability.Private)
                        Response.Cache.SetCacheability(HttpCacheability.NoCache)
                        Response.Expires = -1
                        Response.Buffer = True

                        'Response.BufferOutput = False

                        'Dim ms As New IO.MemoryStream(data)
                        'Dim buffer = New Byte(1024 - 1) {}
                        'Dim bytesRead As Integer = 0
                        'bytesRead = ms.Read(buffer, 0, buffer.Length)
                        'While bytesRead > 0
                        '    Response.BinaryWrite(buffer)
                        '    Response.Flush()
                        '    bytesRead = ms.Read(buffer, 0, buffer.Length)
                        'End While

                        Response.BinaryWrite(data)

                        Response.End()


                    Else

                        Dim filename As String = "File"
                        If ht.ContainsKey("Filename") Then
                            'filename = ht("Filename").ToString.Split(".")(0)
                            filename = ht("Filename").ToString
                        End If



                        Response.Clear()
                        Response.ClearContent()
                        Response.ClearHeaders()
                        'Response.AddHeader("Content-Disposition", "attachment; filename=" & filename & extension)
                        Response.AddHeader("Content-Disposition", "attachment; filename=" & Uri.EscapeDataString(filename))

                        Response.AddHeader("Content-Length", data.Length.ToString())
                        If extension = ".xml" Then
                            contentType = "text/xml"
                        End If

                        Response.AddHeader("Content-Type", contentType)
                        Response.ContentType = contentType
                        Response.Cache.SetCacheability(HttpCacheability.Private)
                        Response.Expires = -1
                        Response.Buffer = True
                        Response.BinaryWrite(data)

                        Response.End()

                    End If

                    Session("AttachmentFullName") = Nothing
                    Exit Sub

                Else
                    fullPath = Session("AttachmentFullName")
                    Session("AttachmentFullName") = Nothing

                End If


            End If


            If Not String.IsNullOrEmpty(fullPath) Then

                Me.ViewFile(fullPath)

            End If

        End If


    End Sub


    Private Sub BinaryWrite(ByVal fullpath As String)

        Dim data = IO.File.ReadAllBytes(fullpath)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" & Uri.EscapeDataString(IO.Path.GetFileName(fullpath)))
        Response.AddHeader("Content-Length", data.Length.ToString())
        Response.ContentType = "application/octet-stream"

        Response.Cache.SetCacheability(HttpCacheability.Private)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Expires = -1


        Response.Buffer = True
        Response.OutputStream.Write(data, 0, data.Length)
        Response.Flush()
        Response.End()
        Response.Close()
    End Sub


    Private Sub TransmitFile(ByVal fullpath As String)
        Dim file As New IO.FileInfo(fullpath)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" & Uri.EscapeDataString(file.Name))
        Response.AddHeader("Content-Length", file.Length.ToString())
        Response.ContentType = "application/octet-stream"

        Response.TransmitFile(fullpath)
        Response.Flush()
        Response.End()
    End Sub


    Private Sub ViewFile(ByVal fullpath As String)
        Response.Clear()

        Response.AddHeader("Content-Disposition", "attachment; filename=" & Uri.EscapeDataString(IO.Path.GetFileName(fullpath)))

        Response.ContentType = "application/octet-stream"

        Dim chunkSize As Integer = 1024
        Dim buffer As Byte() = New Byte(chunkSize - 1) {}
        Dim offset As Integer = 0
        Dim read As Integer = 0

        Using fs As IO.FileStream = IO.File.Open(fullpath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Response.AddHeader("Content-Length", fs.Length.ToString())
            read = fs.Read(buffer, offset, chunkSize)
            While read > 0
                Response.OutputStream.Write(buffer, 0, read)
                'Response.BinaryWrite(buffer)
                Response.Flush()
                read = fs.Read(buffer, offset, chunkSize)
            End While
        End Using

        Response.Close()
    End Sub

    





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.DownloadFile()
    End Sub

End Class
