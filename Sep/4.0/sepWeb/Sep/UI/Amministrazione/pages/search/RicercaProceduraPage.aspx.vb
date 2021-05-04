Imports ParsecAdmin
Imports Telerik.Web.UI

Partial Class RicercaProceduraPage
    Inherits System.Web.UI.Page

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim prodedure As New ProcedureRepository
        Dim idModulo As Integer = CInt(Request.QueryString("search"))
        Me.ProcedureGridView.DataSource = prodedure.GetQuery.Where(Function(c) c.IdModulo = idModulo).OrderBy(Function(c) c.Descrizione)
        'prodedure.Dispose()
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Procedure&nbsp;&nbsp;&nbsp;" & If(Me.ProcedureGridView.MasterTableView.Items.Count > 0, "( " & Me.ProcedureGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ProcedureGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles ProcedureGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.ProcedureGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.ProcedureGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.ProcedureGridView.SelectedItems.Count = Me.ProcedureGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.ProcedureGridView.Items.Count > 0
    End Sub

    Private Sub ProcedureGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ProcedureGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf ProcedureGridView_ItemPreRender
        End If
    End Sub

    Protected Sub ProcedureGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In ProcedureGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Dim procedure As New SortedList(Of Integer, String)

        Dim procedureRepository As New ProcedureRepository

        For Each item As GridDataItem In ProcedureGridView.Items
            Dim SelectCheckBox As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)
            If SelectCheckBox.Checked Then
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim procedura As ParsecAdmin.Procedura = procedureRepository.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
                If Not procedura Is Nothing Then
                    procedure.Add(procedura.Id, procedura.Descrizione)
                End If

            End If
        Next
        Session("SelectedProcedures") = procedure

        ParsecUtility.Utility.ClosePopup(True)
        procedureRepository.Dispose()
    End Sub

    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region

End Class