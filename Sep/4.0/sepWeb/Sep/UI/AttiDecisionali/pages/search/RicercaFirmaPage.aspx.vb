Imports ParsecAdmin
Imports Telerik.Web.UI

Partial Class RicercaFirmaPage
    Inherits System.Web.UI.Page

#Region "EVENTI PAGINA"

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Firme&nbsp;&nbsp;&nbsp;" & If(Me.FirmeGridView.MasterTableView.Items.Count > 0, "( " & Me.FirmeGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.FirmeGridView.Rebind()
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Private Sub FirmeGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles FirmeGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf FirmeGridView_ItemPreRender
        End If
    End Sub

    Protected Sub FirmeGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub FirmeGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles FirmeGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.FirmeGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.FirmeGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.FirmeGridView.SelectedItems.Count = Me.FirmeGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.FirmeGridView.Items.Count > 0
    End Sub

    Protected Sub FirmeGridView_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FirmeGridView.NeedDataSource
        Dim firme As New ParsecAtt.FirmeRepository
        Me.FirmeGridView.DataSource = firme.GetView(Nothing)
        firme.Dispose()

        If Not Me.Page.IsPostBack Then
            If Not Request.QueryString("filtro") Is Nothing Then
                Dim filtro As String = Request.QueryString("filtro")
                If Not String.IsNullOrEmpty(filtro) Then
                    'Me.FirmeGridView.MasterTableView.FilterExpression = String.Format("(Descrizione = {0})", Now.Year)
                    Me.FirmeGridView.MasterTableView.FilterExpression = String.Format("(iif(Descrizione == null, """", Descrizione).ToString().ToUpper().Contains(""{0}"".ToUpper()))", filtro)
                    Dim column As GridColumn = FirmeGridView.MasterTableView.GetColumnSafe("Descrizione")
                    column.CurrentFilterValue = filtro
                End If

            End If
        End If
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In FirmeGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click

        Dim firmeSelezionate As New List(Of ParsecAtt.ModelloFirma)

        Dim firme As New ParsecAtt.FirmeRepository

        For Each item As GridDataItem In FirmeGridView.Items
            Dim SelectCheckBox As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)

            If SelectCheckBox.Checked Then
                Dim ordinaleTextBox As RadTextBox = CType(item.FindControl("OrdinaleTextBox"), RadTextBox)
                Dim iterCheckBox As CheckBox = CType(item.FindControl("IterCheckBox"), CheckBox)

                Dim ordinale As Integer = 0
                Integer.TryParse(ordinaleTextBox.Text, ordinale)

                If ordinale <> 0 Then
                    Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                    Dim firma As ParsecAtt.Firma = firme.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
                    If Not firma Is Nothing Then

                        Dim modellofirma As New ParsecAtt.ModelloFirma
                        modellofirma.Ordinale = ordinale
                        modellofirma.Iter = iterCheckBox.Checked
                        modellofirma.Descrizione = firma.Descrizione
                        modellofirma.IdFirma = firma.Id
                        firmeSelezionate.Add(modellofirma)
                    End If
                End If


            End If
        Next
        Session("FirmeSelezionate") = firmeSelezionate

        ParsecUtility.Utility.ClosePopup(True)
        firme.Dispose()
    End Sub

    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region

End Class
