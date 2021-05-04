Imports Telerik.Web.UI

Partial Class RicercaBeneficiarioPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Public Property Liquidazioni() As List(Of ParsecAtt.Liquidazione)
        Get
            Return CType(Session("RicercaBeneficiarioPage_Liquidazioni"), List(Of ParsecAtt.Liquidazione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Liquidazione))
            Session("RicercaBeneficiarioPage_Liquidazioni") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"




    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Beneficiari&nbsp;&nbsp;&nbsp;" & If(Me.Liquidazioni.Count > 0, "( " & Me.Liquidazioni.Count.ToString & " )", "")
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub BeneficiariGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles BeneficiariGridView.NeedDataSource
        If Me.Liquidazioni Is Nothing Then
            Dim liquidazioni As New ParsecAtt.LiquidazioneRepository
            Dim filtro As New ParsecAtt.FiltroLiquidazione

            Dim res = (From l In liquidazioni.GetQuery
                      Select l).GroupBy(Function(c) c.Nominativo).Select(Function(c) c.FirstOrDefault).ToList
            '    filtro.IdTipologia = Request.QueryString("tipo")
            'End If

            Me.Liquidazioni = res
            liquidazioni.Dispose()
        End If
        Me.BeneficiariGridView.DataSource = Me.Liquidazioni
    End Sub


    Protected Sub CapitoliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles BeneficiariGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaBeneficiario(e.Item)
        End If
    End Sub

    Protected Sub BeneficiariGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles BeneficiariGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SelezionaBeneficiario(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim liquidazioni As New ParsecAtt.LiquidazioneRepository
        Dim liquidazione As ParsecAtt.Liquidazione = liquidazioni.GetById(id)
        liquidazioni.Dispose()
        ParsecUtility.SessionManager.Liquidazione = liquidazione
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region


End Class
