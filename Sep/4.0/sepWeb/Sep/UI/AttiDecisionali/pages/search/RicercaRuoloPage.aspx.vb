Imports ParsecAdmin
Imports Telerik.Web.UI

Partial Class RicercaRuoloPage
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim ruoli As New ParsecWKF.RuoloRepository
        Me.RuoliGridView.DataSource = ruoli.GetView(Nothing)
        ruoli.Dispose()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.GetParametri()
            If TipoSelezione = 0 Then
                Me.RuoliGridView.Columns(0).Visible = False
                Me.ConfermaButton.Visible = False
            End If
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Ruoli " & If(Me.RuoliGridView.MasterTableView.Items.Count > 0, "( " & Me.RuoliGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Private Sub RuoliGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles RuoliGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf RuoliGridView_ItemPreRender
        End If
    End Sub

    Protected Sub RuoliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RuoliGridView.ItemCommand
        If e.CommandName = "Select" Then
            If Me.TipoSelezione = 0 Then
                Me.SelezionaRuolo(e.Item)
            End If
        End If
    End Sub

    Protected Sub RuoliGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RuoliGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona ruolo"
            End If
        End If
    End Sub

    Protected Sub RuoliGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub RuoliGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles RuoliGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.RuoliGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.RuoliGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.RuoliGridView.SelectedItems.Count = Me.RuoliGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.RuoliGridView.Items.Count > 0
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.RuoliGridView.MasterTableView.Items
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

    Private Sub SelezionaRuolo(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim ruoli As New ParsecWKF.RuoloRepository
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim ruolo As ParsecWKF.Ruolo = ruoli.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        If Not ruolo Is Nothing Then
            ParsecUtility.SessionManager.Ruolo = ruolo
        End If
        ParsecUtility.Utility.ClosePopup(True)
        ruoli.Dispose()
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Dim ruoliSelezionati As New List(Of ParsecWKF.Ruolo)
        Dim ruoli As New ParsecWKF.RuoloRepository

        For Each item As GridDataItem In Me.RuoliGridView.Items
            Dim selectCheckBox As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)
            If selectCheckBox.Checked Then
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim ruolo As ParsecWKF.Ruolo = ruoli.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
                If Not ruolo Is Nothing Then
                    ruoliSelezionati.Add(ruolo)
                End If

            End If
        Next
        ParsecUtility.SessionManager.Ruolo = ruoliSelezionati
        ParsecUtility.Utility.ClosePopup(True)
        ruoli.Dispose()

    End Sub

    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region

End Class