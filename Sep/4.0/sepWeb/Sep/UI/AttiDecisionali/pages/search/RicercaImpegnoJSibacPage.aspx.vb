Imports Telerik.Web.UI

Partial Class RicercaImpegnoJSibacPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Private Property Impegni() As List(Of ParsecPostgres.ImpegnoSpesa)
        Get
            Return CType(Session("RicercaImpegnoJSibacPage_Impegni"), List(Of ParsecPostgres.ImpegnoSpesa))
        End Get
        Set(ByVal value As List(Of ParsecPostgres.ImpegnoSpesa))
            Session("RicercaImpegnoJSibacPage_Impegni") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.Impegni = Nothing
            Me.CaricaAnni()

            Dim annoEsercizio As Integer = Now.Year
            If Not Request.QueryString("AnnoEsercizio") Is Nothing Then
                annoEsercizio = CInt(Request.QueryString("AnnoEsercizio"))
            End If

            Me.AnniComboBox.Items.FindItemByValue(annoEsercizio).Selected = True

        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ImpegniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ImpegniGridView.NeedDataSource
        If Me.Impegni Is Nothing Then

            Dim impegni As New ParsecPostgres.ImpegniSpesa
            Dim filtro As ParsecPostgres.FiltroImpegno = Me.GetFiltro
            impegni.FiltraByCapitolo(filtro)

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            If utenteCollegato.SuperUser = False Then

                Dim listaImpegni = impegni.ToList

                Dim utentiCentro As New ParsecAdmin.UtenteCentroResponsabilitaRepository
                Dim view = (From impegno In listaImpegni
                          Join centroResponsabilita In utentiCentro.GetQuery.Where(Function(c) c.IdUtente = utenteCollegato.Id).ToList
                          On impegno.CodiceCentroResponsabilita Equals centroResponsabilita.CodiceCentroResponsabilita
                          Select impegno) '.Distinct

                Me.Impegni = view.ToList

            Else
                Me.Impegni = impegni.ToList
            End If

        End If

        Me.ImpegniGridView.DataSource = Me.Impegni
    End Sub

    Protected Sub ImpegniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ImpegniGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaImpegno(e.Item)
        End If
    End Sub

    Protected Sub ImpegniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ImpegniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub ImpegniGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles ImpegniGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloLabel.Text = "Elenco Impegni Spesa&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub AnniComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles AnniComboBox.SelectedIndexChanged
        Me.Impegni = Nothing
        Me.ImpegniGridView.Rebind()
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Function GetFiltro() As ParsecPostgres.FiltroImpegno
        Dim filtro As New ParsecPostgres.FiltroImpegno

        If Not Request.QueryString("Tipologia") Is Nothing Then
            filtro.Tipologia = Request.QueryString("Tipologia")
        End If

        If Not Request.QueryString("AnnoEsercizio") Is Nothing Then
            filtro.AnnoEsercizio = Request.QueryString("AnnoEsercizio")
        End If

        If Not Request.QueryString("AnnoImpegno") Is Nothing Then
            filtro.AnnoImpegno = Request.QueryString("AnnoImpegno")
        End If

        If Not Request.QueryString("NumeroCapitolo") Is Nothing Then
            filtro.NumeroCapitolo = Request.QueryString("NumeroCapitolo")
        End If

        If Not Request.QueryString("Articolo") Is Nothing Then
            filtro.Articolo = Request.QueryString("Articolo")
        End If

        filtro.AnnoEsercizioImpegno = CInt(Me.AnniComboBox.SelectedValue)

        Return filtro
    End Function

    Private Sub SelezionaImpegno(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim impegno As ParsecPostgres.ImpegnoSpesa = Me.Impegni.Where(Function(c) c.Id = id).FirstOrDefault
        ParsecUtility.SessionManager.ImpegnoSpesa = impegno
        ParsecUtility.Utility.ClosePopup(True)
        Me.Impegni = Nothing
    End Sub

    Private Sub CaricaAnni()
        Dim anni As New ParsecAtt.AnnoRepository
        Dim elencoAnni = anni.GetQuery.OrderBy(Function(c) c.Valore).Select(Function(c) New With {.Valore = c.Valore})
        Me.AnniComboBox.DataValueField = "Valore"
        Me.AnniComboBox.DataTextField = "Valore"
        Me.AnniComboBox.DataSource = elencoAnni
        Me.AnniComboBox.DataBind()
        anni.Dispose()
    End Sub

#End Region

End Class

