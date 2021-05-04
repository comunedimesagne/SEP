Imports ParsecAdmin
Imports Telerik.Web.UI

Partial Class RicercaProfiliPage
    Inherits System.Web.UI.Page

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim profileRepository As New ProfileRepository
        Dim filtro As New ParsecAdmin.ProfiloFiltro With {.Utente = New ParsecAdmin.Utente With {.Id = 1, .SuperUser = True}}
        Me.ProfiliGridView.DataSource = profileRepository.GetProfiliUtente(filtro)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Profili&nbsp;&nbsp;" & If(Me.ProfiliGridView.MasterTableView.Items.Count > 0, "( " & Me.ProfiliGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Private Sub ProfiliGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ProfiliGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf ProcedureGridView_ItemPreRender
        End If
    End Sub

    Protected Sub ProcedureGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub ProfiliGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles ProfiliGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.ProfiliGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.ProfiliGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.ProfiliGridView.SelectedItems.Count = Me.ProfiliGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.ProfiliGridView.Items.Count > 0
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In ProfiliGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Dim profili As New SortedList(Of Integer, String)
        Dim profileRepository As New ProfileRepository
        For Each item As GridDataItem In ProfiliGridView.Items
            Dim SelectCheckBox As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)
            If SelectCheckBox.Checked Then
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim profilo As ParsecAdmin.Profilo = profileRepository.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
                If Not profilo Is Nothing Then
                    profili.Add(profilo.Id, profilo.Descrizione)
                End If

            End If
        Next
        Session("SelectedProfiles") = profili

        ParsecUtility.Utility.ClosePopup(True)
        profileRepository.Dispose()
    End Sub

    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region
 
End Class