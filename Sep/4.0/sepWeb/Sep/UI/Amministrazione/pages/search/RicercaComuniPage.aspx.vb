Imports System.Web.UI.WebControls
Imports Telerik.Web.UI
Imports ParsecAdmin
Partial Class RicercaComuniPage
    Inherits System.Web.UI.Page

    Dim ComuniR As New ComuniUrbaniRepository


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        rgrdComuni.GroupingSettings.CaseSensitive = False
        'rgrdComuni.Width = 700
        If Not IsPostBack Then
            If Request.QueryString("mode") Is Nothing Then
                Session("ParentPage") = Request.Params.Item(0)
            End If
        End If
    End Sub

    Private Sub SelezionaComune(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim comuni As New ParsecAdmin.ComuniUrbaniRepository
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Codice")
        Dim comune As ParsecAdmin.ComuniUrbani = comuni.GetQuery.Where(Function(c) c.Codice = id).FirstOrDefault
        If Not comune Is Nothing Then
            ParsecUtility.SessionManager.ComuneUrbano = comune
        End If
        comuni.Dispose()
        ParsecUtility.Utility.ClosePopup(True)
    End Sub


    Protected Sub rgrdComuni_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgrdComuni.ItemCommand
        If e.CommandName = "Select" Then
            If Request.QueryString("mode") Is Nothing Then
                Try
                    Dim script As String = "<script language=""javascript"">" & vbCrLf
                    script += "{" & vbCrLf
                    script += "if(window.opener && !window.opener.closed)" & vbCrLf
                    script += "{" & vbCrLf
                    Select Case Session("ParentPage").ToString()
                        Case "CommittentePage1"
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtComuneResidenza_text').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(3).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtComuneResidenza').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(3).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtProvinciaResidenza_text').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(4).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtProvinciaResidenza').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(4).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtCAP_text').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(12).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtCAP').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(12).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                        Case "CommittentePage2"
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtComunePostale_text').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(3).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtComunePostale').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(3).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtProvinciaPostale_text').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(4).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtProvinciaPostale').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(4).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtCAPPostale_text').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(12).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtcmtCAPPostale').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(12).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                        Case "TerreniPage"
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtterComune_text').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(3).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtterComune').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(3).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtterCap_text').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(12).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                            script += "window.opener.document.getElementById('ctl00_MainContent_rtxtterCap').value= '" & rgrdComuni.Items(e.Item.ItemIndex).Cells(12).Text.Replace("&nbsp;", "").Replace("'", "`") & "';" & vbCrLf
                    End Select
                    script += "self.close();" & vbCrLf
                    script += "}" & vbCrLf
                    script += "}" & vbCrLf
                    script += "</" & "script>"

                    ClientScript.RegisterClientScriptBlock(Me.GetType(), "SetComune", script)

                Catch ex As Exception

                End Try
            Else
                SelezionaComune(e.Item)
            End If


        End If
    End Sub

    Protected Sub rgrdComuni_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgrdComuni.NeedDataSource
        rgrdComuni.DataSource = ComuniR.Fetch()

        If Not Me.IsPostBack Then
            If Not Request.QueryString("filtro") Is Nothing Then
                Dim filtro As String = Request.QueryString("filtro")
                If Not String.IsNullOrEmpty(filtro) Then
                    rgrdComuni.MasterTableView.FilterExpression = String.Format("(iif(Descrizione == null, """", Descrizione).ToString().ToUpper().Contains(""{0}"".ToUpper()))", filtro)
                    Dim column As GridColumn = rgrdComuni.MasterTableView.GetColumnSafe("Descrizione")
                    column.CurrentFilterValue = filtro
                   End If
            End If
        End If
    End Sub

    
End Class
