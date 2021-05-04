Imports Telerik.Web.UI

Partial Class RicercaCapitoloPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Public Property Capitoli() As List(Of ParsecAtt.Capitolo)
        Get
            Return CType(Session("RicercaCapitoloPage_Capitoli"), List(Of ParsecAtt.Capitolo))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Capitolo))
            Session("RicercaCapitoloPage_Capitoli") = value
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
        'Me.TitoloLabel.Text = "Elenco Capitoli&nbsp;&nbsp;&nbsp;" & If(Me.Capitoli.Count > 0, "( " & Me.Capitoli.Count.ToString & " )", "")
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"


    Protected Sub CapitoliGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles CapitoliGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloLabel.Text = "Elenco Capitoli&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If
    End Sub


    Protected Sub CapitoliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles CapitoliGridView.NeedDataSource
        If Me.Capitoli Is Nothing Then
            Dim capitoli As New ParsecAtt.CapitoloRepository
            Dim filtro As New ParsecAtt.FiltroCapitolo
            If Not Request.QueryString("tipo") Is Nothing Then
                filtro.IdTipologia = Request.QueryString("tipo")
            End If

            Me.Capitoli = capitoli.GetView(filtro)
            capitoli.Dispose()
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
        Dim capitoli As New ParsecAtt.CapitoloRepository
        Dim capitolo As ParsecAtt.Capitolo = capitoli.GetById(id)
        capitoli.Dispose()
        ParsecUtility.SessionManager.Capitolo = capitolo
        ParsecUtility.Utility.ClosePopup(True)
    End Sub


  

#End Region


End Class
