Imports ParsecAdmin
Imports Telerik.Web.UI

Partial Class RicercaGruppoPage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    Public Property TipoSelezione As Integer
        Get
            Return ViewState("TipoSelezione")
        End Get
        Set(ByVal value As Integer)
            ViewState("TipoSelezione") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Gruppi " & If(Me.GruppiGridView.MasterTableView.Items.Count > 0, "( " & Me.GruppiGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim gruppi As New ParsecAdmin.GruppoRepository
        Me.GruppiGridView.DataSource = gruppi.GetView(New ParsecAdmin.FiltroGruppo With {.Abilitato = True})
        gruppi.Dispose()

        If Not Me.IsPostBack Then
            Me.GetParametri()
            If TipoSelezione = 0 Then
                Me.GruppiGridView.Columns(0).Visible = False
                Me.ConfermaButton.Visible = False
            End If
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Private Sub GruppiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles GruppiGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf GruppiGridView_ItemPreRender
        End If
    End Sub

    Protected Sub GruppiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles GruppiGridView.ItemCommand
        If e.CommandName = "Select" Then
            If Me.TipoSelezione = 0 Then
                Me.SelezionaGruppo(e.Item)
            End If
        End If
    End Sub

    Protected Sub GruppiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles GruppiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim gruppo As ParsecAdmin.Gruppo = CType(e.Item.DataItem, ParsecAdmin.Gruppo)
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)

            If gruppo.Abilitato Then
                chk.Enabled = True
                If gruppo.DataInizioValidita.Value.Date > Now.Date Then
                    chk.Enabled = False
                ElseIf gruppo.DataFineValidita.HasValue Then
                    If gruppo.DataFineValidita.Value.Date < Now.Date Then
                        chk.Enabled = False
                    End If
                End If
            Else
                chk.Enabled = False
            End If


            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona gruppo"
            End If
        End If
    End Sub

    Protected Sub GruppiGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub GruppiGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles GruppiGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.GruppiGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.GruppiGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.GruppiGridView.SelectedItems.Count = Me.GruppiGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.GruppiGridView.Items.Count > 0
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In GruppiGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub GetParametri()
        Me.TipoSelezione = 1
        Dim ht As Hashtable = Session("Parametri")

        If Not ht Is Nothing Then
            If ht.ContainsKey("tipoSelezione") Then
                Me.TipoSelezione = ht("tipoSelezione")
            End If
        End If
        Session("Parametri") = Nothing
    End Sub

    Private Sub SelezionaGruppo(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim gruppiSelezionati As New SortedList(Of Integer, String)
        Dim gruppi As New ParsecAdmin.GruppoRepository

        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim gruppo As ParsecAdmin.Gruppo = gruppi.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        If Not gruppo Is Nothing Then
            gruppiSelezionati.Add(gruppo.Id, gruppo.Descrizione)
        End If
        Session("GruppiSelezionati") = gruppiSelezionati
        ParsecUtility.Utility.ClosePopup(True)
        gruppi.Dispose()
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Dim gruppiSelezionati As New SortedList(Of Integer, String)
        Dim gruppi As New ParsecAdmin.GruppoRepository
        For Each item As GridDataItem In Me.GruppiGridView.Items
            Dim selectCheckBox As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)
            If selectCheckBox.Checked Then
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim gruppo As ParsecAdmin.Gruppo = gruppi.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
                If Not gruppo Is Nothing Then
                    gruppiSelezionati.Add(gruppo.Id, gruppo.Descrizione)
                End If
            End If
        Next
        Session("GruppiSelezionati") = gruppiSelezionati
        gruppi.Dispose()
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region

End Class