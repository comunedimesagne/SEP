Imports Telerik.Web.UI

Partial Class RicercaCapitoloDedaGroupPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Private Property Capitoli() As List(Of ParsecWebServices.CapitoloModel)
        Get
            Return CType(Session("RicercaCapitoloDedaGroupPage_Capitoli"), List(Of ParsecWebServices.CapitoloModel))
        End Get
        Set(ByVal value As List(Of ParsecWebServices.CapitoloModel))
            Session("RicercaCapitoloDedaGroupPage_Capitoli") = value
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
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"


    Protected Sub CapitoliGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles CapitoliGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloLabel.Text = "Elenco Capitoli Spesa&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If
    End Sub


    Protected Sub CapitoliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles CapitoliGridView.NeedDataSource


        If Me.Capitoli Is Nothing Then
            Try
                Dim postParameters As New Dictionary(Of String, Object)

                postParameters.Add("EntrataUscita", ParsecWebServices.Direzione.Uscita)

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)



                If Not Request.QueryString("anno") Is Nothing Then
                    postParameters.Add("Anno", Request.QueryString("anno"))
                End If

                Dim endPoint As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupBaseUrlRagioneria")
                Dim scope As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupScopeRagioneria")
                Dim accessTokenUri As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupAccessTokenUriRagioneria")
                Dim clientId As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientIdRagioneria")
                Dim clientSecret As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientSecretRagioneria")

                Dim parameter As New ParsecWebServices.OAuth2Parameter With {.AccessTokenUri = accessTokenUri, .ClientId = clientId, .ClientSecret = clientSecret, .Scope = scope}
                Dim service As New ParsecWebServices.DedaGroupService(String.Format(endPoint, "CercaCapitoli"), parameter)

                Me.Capitoli = service.QueryCapitoli(postParameters)


                If utenteCollegato.SuperUser = False Then

                    Dim utentiCentro As New ParsecAdmin.UtenteCentroResponsabilitaRepository

                    Dim view = (From capitolo In Me.Capitoli
                                Join centroResponsabilita In utentiCentro.GetQuery.Where(Function(c) c.IdUtente = utenteCollegato.Id).ToList
                              On capitolo.CodiceCdr Equals centroResponsabilita.CodiceCentroResponsabilita
                                Select capitolo).Distinct

                    Me.Capitoli = view.ToList

                    utentiCentro.Dispose()

                End If
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
                Me.Capitoli = New List(Of ParsecWebServices.CapitoloModel)
            End Try

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
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdCapitolo")
        Dim capitolo As ParsecWebServices.CapitoloModel = Me.Capitoli.Where(Function(c) c.IdCapitolo = id).FirstOrDefault
        ParsecUtility.SessionManager.Capitolo = capitolo
        ParsecUtility.Utility.ClosePopup(True)
    End Sub


#End Region

End Class