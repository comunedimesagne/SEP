Imports Telerik.Web.UI

Partial Class RicercaImpegnoApkPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Private Property Impegni() As List(Of ParsecWebServices.ImpegnoApkModel)
        Get
            Return CType(Session("RicercaImpegnoApkPage_Impegni"), List(Of ParsecWebServices.ImpegnoApkModel))
        End Get
        Set(ByVal value As List(Of ParsecWebServices.ImpegnoApkModel))
            Session("RicercaImpegnoApkPage_Impegni") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.Impegni = Nothing
            Me.CaricaAnni()

            Dim annoEsercizio As Integer = Now.Year
            If Not Request.QueryString("AnnoEsercizio") Is Nothing Then
                annoEsercizio = CInt(Request.QueryString("AnnoEsercizio"))
            End If

            Me.AnniComboBox.Items.FindItemByValue(annoEsercizio).Selected = True

        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ImpegniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ImpegniGridView.NeedDataSource
        If Me.Impegni Is Nothing Then

            Dim req As New ParsecWebServices.ImpegniApkRequest

            If Not Request.QueryString("AnnoEsercizio") Is Nothing Then
                req.Anno = Request.QueryString("AnnoEsercizio")
                req.Esercizio = CInt(Me.AnniComboBox.SelectedValue)
            End If

            Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsApk")
            Dim service As New ParsecWebServices.ImpegniApkService(baseUrl, req)
            Me.Impegni = service.GetView

        End If

        Me.ImpegniGridView.DataSource = Me.Impegni
    End Sub

    Protected Sub ImpegniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ImpegniGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaImpegno(e.Item)
        End If
    End Sub

    Protected Sub ImpegniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ImpegniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub ImpegniGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles ImpegniGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloLabel.Text = "Elenco Impegni Spesa&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub AnniComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles AnniComboBox.SelectedIndexChanged
        Me.Impegni = Nothing
        Me.ImpegniGridView.Rebind()
    End Sub

#End Region

#Region "METODI PRIVATI"

    'Private Function GetFiltro() As ParsecPostgres.FiltroImpegno
    '    Dim filtro As New ParsecPostgres.FiltroImpegno

    '    If Not Request.QueryString("Tipologia") Is Nothing Then
    '        filtro.Tipologia = Request.QueryString("Tipologia")
    '    End If

    '    If Not Request.QueryString("AnnoEsercizio") Is Nothing Then
    '        filtro.AnnoEsercizio = Request.QueryString("AnnoEsercizio")
    '    End If

    '    If Not Request.QueryString("AnnoImpegno") Is Nothing Then
    '        filtro.AnnoImpegno = Request.QueryString("AnnoImpegno")
    '    End If

    '    If Not Request.QueryString("NumeroCapitolo") Is Nothing Then
    '        filtro.NumeroCapitolo = Request.QueryString("NumeroCapitolo")
    '    End If

    '    If Not Request.QueryString("Articolo") Is Nothing Then
    '        filtro.Articolo = Request.QueryString("Articolo")
    '    End If

    '    filtro.AnnoEsercizioImpegno = CInt(Me.AnniComboBox.SelectedValue)

    '    Return filtro
    'End Function

    Private Sub SelezionaImpegno(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim impegno As ParsecWebServices.ImpegnoApkModel = Me.Impegni.Where(Function(c) c.Id = id).FirstOrDefault
        ParsecUtility.SessionManager.ImpegnoSpesa = impegno
        ParsecUtility.Utility.ClosePopup(True)
        Me.Impegni = Nothing
    End Sub

    Private Sub CaricaAnni()
        Dim anni As New ParsecAtt.AnnoRepository
        Dim elencoAnni = anni.GetQuery.OrderBy(Function(c) c.Valore).Select(Function(c) New With {.Valore = c.Valore})
        Me.AnniComboBox.DataValueField = "Valore"
        Me.AnniComboBox.DataTextField = "Valore"
        Me.AnniComboBox.DataSource = elencoAnni
        Me.AnniComboBox.DataBind()
        anni.Dispose()
    End Sub

#End Region

End Class

