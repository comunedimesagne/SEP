Imports Telerik.Web.UI

Partial Class RicercaCapitoloApkPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Private Property Capitoli() As List(Of ParsecWebServices.CapitoloApkModel)
        Get
            Return CType(Session("RicercaCapitoloApkPage_Capitoli"), List(Of ParsecWebServices.CapitoloApkModel))
        End Get
        Set(ByVal value As List(Of ParsecWebServices.CapitoloApkModel))
            Session("RicercaCapitoloApkPage_Capitoli") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.Capitoli = Nothing
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"


    Protected Sub CapitoliGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles CapitoliGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloLabel.Text = "Elenco Capitoli Spesa&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If
    End Sub


    Protected Sub CapitoliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles CapitoliGridView.NeedDataSource

        If Me.Capitoli Is Nothing Then

            Dim req As New ParsecWebServices.CapitoliApkRequest
            If Not request.QueryString("tipo") Is Nothing Then
                req.Tipologia = "S" 'request.QueryString("tipo")
            End If
            If Not Request.QueryString("anno") Is Nothing Then
                req.Esercizio = Request.QueryString("anno")
                req.Anno = Request.QueryString("anno")
            End If

            Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsApk")

            Dim service As New ParsecWebServices.CapitoliApkService(baseUrl, req)

            Me.Capitoli = service.GetView

        End If

        Me.CapitoliGridView.DataSource = Me.Capitoli
    End Sub


    Protected Sub CapitoliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles CapitoliGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaCapitolo(e.Item)
        End If
    End Sub

    Protected Sub CapitoliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles CapitoliGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SelezionaCapitolo(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim capitolo As ParsecWebServices.CapitoloApkModel = Me.Capitoli.Where(Function(c) c.Id = id).FirstOrDefault
        ParsecUtility.SessionManager.Capitolo = capitolo
        ParsecUtility.Utility.ClosePopup(True)
    End Sub




#End Region


End Class

