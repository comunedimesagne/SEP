Imports Telerik.Web.UI

Partial Class RicercaUtenteWindowsPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Private Property WindowsUsers() As List(Of ParsecAdmin.WindowsUser)
        Get
            Return CType(Session("RicercaUtenteWindowsPage_WindowsUsers"), List(Of ParsecAdmin.WindowsUser))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.WindowsUser))
            Session("RicercaUtenteWindowsPage_WindowsUsers") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Try
                Dim ldapPath = ParsecAdmin.WebConfigSettings.GetKey("LdapPath")
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
            Me.WindowsUsers = Nothing
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"


    Protected Sub WindowsUsersGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles WindowsUsersGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloLabel.Text = "Elenco Utenti Windows&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If
    End Sub


    Protected Sub WindowsUsersGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles WindowsUsersGridView.NeedDataSource
        If Me.WindowsUsers Is Nothing Then
            Try
                Dim ldapPath = ParsecAdmin.WebConfigSettings.GetKey("LdapPath")
                Dim utenti As New ParsecAdmin.LdapWindowsUser(ldapPath)
                Me.WindowsUsers = utenti.GetAllUsers
                utenti.Dispose()
            Catch ex As Exception
                Me.WindowsUsers = New List(Of ParsecAdmin.WindowsUser)
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        End If
        Me.WindowsUsersGridView.DataSource = Me.WindowsUsers
    End Sub


    Protected Sub WindowsUsersGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles WindowsUsersGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaWindowsUser(e.Item)
        End If
    End Sub

    Protected Sub WindowsUsersGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles WindowsUsersGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SelezionaWindowsUser(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim objectGuid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("ObjectGuid")
        Dim windowsUser As ParsecAdmin.WindowsUser = Me.WindowsUsers.Where(Function(c) c.ObjectGuid = objectGuid).FirstOrDefault
        ParsecUtility.SessionManager.WindowsUser = windowsUser
        ParsecUtility.Utility.ClosePopup(True)
    End Sub




#End Region


End Class

