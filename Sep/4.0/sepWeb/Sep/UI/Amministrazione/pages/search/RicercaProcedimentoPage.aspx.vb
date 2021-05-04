Imports Telerik.Web.UI

Partial Class RicercaProcedimentoPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Public Property Procedimenti() As List(Of ParsecAdmin.Procedimento)
        Get
            Return CType(Session("RicercaProcedimentoPage_Procedimenti"), List(Of ParsecAdmin.Procedimento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Procedimento))
            Session("RicercaProcedimentoPage_Procedimenti") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.Procedimenti = Nothing
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Procedimenti&nbsp;&nbsp;&nbsp;" & If(Me.Procedimenti.Count > 0, "( " & Me.Procedimenti.Count.ToString & " )", "")
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ProcedimentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ProcedimentiGridView.NeedDataSource
        If Me.Procedimenti Is Nothing Then
            Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
            Me.Procedimenti = procedimenti.GetView(Nothing)
            procedimenti.Dispose()
        End If
        Me.ProcedimentiGridView.DataSource = Me.Procedimenti

        'If Not Me.Page.IsPostBack Then
        '    Me.ProcedimentiGridView.MasterTableView.FilterExpression = String.Format("(AnnoEsercizio = {0})", Now.Year)
        '    Dim column As GridColumn = ProcedimentiGridView.MasterTableView.GetColumnSafe("AnnoEsercizio")
        '    column.CurrentFilterValue = Now.Year.ToString
        'End If

    End Sub


    Protected Sub ProcedimentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ProcedimentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.SelezionaProcedimento(e.Item)
        End Select
    End Sub

 
    Protected Sub ProcedimentiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ProcedimentiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SelezionaProcedimento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
        Dim procedimento As ParsecAdmin.Procedimento = procedimenti.GetById(id)
        procedimenti.Dispose()
        ParsecUtility.SessionManager.Procedimento = procedimento
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region


End Class
