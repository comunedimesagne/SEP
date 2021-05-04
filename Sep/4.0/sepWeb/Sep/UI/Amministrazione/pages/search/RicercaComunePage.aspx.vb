Imports System.Web.UI.WebControls
Imports Telerik.Web.UI
Imports ParsecAdmin

Partial Class RicercaComunePage
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        rgrdComune.GroupingSettings.CaseSensitive = False
    End Sub

    Private Sub SelezionaComune(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim comuni As New ComuneRepository
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim comune As ParsecAdmin.Comune = comuni.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        If Not comune Is Nothing Then
            Session("ComuneSelezionato") = comune
            ParsecUtility.SessionManager.ComuneUrbano = comune
        End If
        comuni.Dispose()
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

    Protected Sub rgrdComune_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgrdComune.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaComune(e.Item)
        End If
    End Sub

    Protected Sub rgrdComune_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgrdComune.NeedDataSource
        rgrdComune.DataSource = (New ComuneRepository).GetQuery.ToList

        If Not Me.IsPostBack Then
            If Not Request.QueryString("filtro") Is Nothing Then
                Dim filtro As String = Request.QueryString("filtro")
                If Not String.IsNullOrEmpty(filtro) Then
                    rgrdComune.MasterTableView.FilterExpression = String.Format("(iif(Descrizione == null, """", Descrizione).ToString().ToUpper().Contains(""{0}"".ToUpper()))", filtro)
                    Dim column As GridColumn = rgrdComune.MasterTableView.GetColumnSafe("Descrizione")
                    column.CurrentFilterValue = filtro
                End If
            End If
        End If
    End Sub
End Class