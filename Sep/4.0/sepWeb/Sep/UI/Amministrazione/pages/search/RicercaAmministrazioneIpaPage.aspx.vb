Imports Telerik.Web.UI

Partial Class RicercaAmministrazioneIpaPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Public Property Amministrazioni() As List(Of ParsecPro.AmministrazioneIpa)
        Get
            Return CType(Session("RicercaAmministrazioneIpaPage_Amministrazioni"), List(Of ParsecPro.AmministrazioneIpa))
        End Get
        Set(ByVal value As List(Of ParsecPro.AmministrazioneIpa))
            Session("RicercaAmministrazioneIpaPage_Amministrazioni") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.Amministrazioni = Nothing
        End If
    End Sub




    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Amministrazioni&nbsp;&nbsp;&nbsp;" & If(Me.Amministrazioni.Count > 0, "( " & Me.Amministrazioni.Count.ToString & " )", "")
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"




    Protected Sub AmministrazioniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles AmministrazioniGridView.NeedDataSource
        If Me.Amministrazioni Is Nothing Then

            Dim ipaSearch As ParsecPro.IpaSearch = Nothing
            Try
                Dim categoria As String = String.Empty
                Dim chiaveRicerca As String = String.Empty
                If Not Request.QueryString("cat") Is Nothing Then
                    categoria = Request.QueryString("cat")
                End If
                If Not Request.QueryString("key") Is Nothing Then
                    chiaveRicerca = Request.QueryString("key")
                End If
                ipaSearch = New ParsecPro.IpaSearch


                Me.Amministrazioni = ipaSearch.DoSearchAmministrazioni(categoria, chiaveRicerca)
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            Finally
                If Not ipaSearch Is Nothing Then
                    ipaSearch.Dispose()
                End If
            End Try
        End If
        Me.AmministrazioniGridView.DataSource = Me.Amministrazioni
    End Sub


    Protected Sub AmministrazioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AmministrazioniGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaAmministrazione(e.Item)
        End If
    End Sub

    Protected Sub AmministrazioniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AmministrazioniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SelezionaAmministrazione(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim codice As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Codice")
        Dim amministrazione As ParsecPro.AmministrazioneIpa = Me.Amministrazioni.Where(Function(c) c.Codice = codice).FirstOrDefault
        ParsecUtility.SessionManager.AmministrazioneIpa = amministrazione
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region


End Class
