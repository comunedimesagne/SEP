Imports Telerik.Web.UI

Partial Class RicercaCapitoloJSibacPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Private Property Capitoli() As List(Of ParsecPostgres.Capitolo)
        Get
            Return CType(Session("RicercaCapitoloJSibacPage_Capitoli"), List(Of ParsecPostgres.Capitolo))
        End Get
        Set(ByVal value As List(Of ParsecPostgres.Capitolo))
            Session("RicercaCapitoloJSibacPage_Capitoli") = value
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

            Dim capitoli As New ParsecPostgres.Capitoli

            Dim filtro As New ParsecPostgres.FiltroCapitolo
            If Not Request.QueryString("tipo") Is Nothing Then
                filtro.Tipologia = Request.QueryString("tipo")
            End If
            If Not Request.QueryString("anno") Is Nothing Then
                filtro.AnnoEsercizio = Request.QueryString("anno")
            End If

            capitoli.Filtra(filtro)


            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            If utenteCollegato.SuperUser = False Then

                Dim listaCapitoli = capitoli.ToList

                Dim utentiCentro As New ParsecAdmin.UtenteCentroResponsabilitaRepository
                Dim view = (From capitolo In listaCapitoli
                          Join centroResponsabilita In utentiCentro.GetQuery.Where(Function(c) c.IdUtente = utenteCollegato.Id).ToList
                          On capitolo.CodiceCentroResponsabilita Equals centroResponsabilita.CodiceCentroResponsabilita
                          Select capitolo).Distinct

                Me.Capitoli = view.ToList
            Else
                Me.Capitoli = capitoli.ToList
            End If



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
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim capitolo As ParsecPostgres.Capitolo = Me.Capitoli.Where(Function(c) c.Id = id).FirstOrDefault
        ParsecUtility.SessionManager.Capitolo = capitolo
        ParsecUtility.Utility.ClosePopup(True)
    End Sub




#End Region


End Class

