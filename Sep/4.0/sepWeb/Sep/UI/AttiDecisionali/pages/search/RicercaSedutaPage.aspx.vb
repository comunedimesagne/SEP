Imports Telerik.Web.UI

Partial Class RicercaSedutaPage
    Inherits System.Web.UI.Page



#Region "PROPRIETA'"

    Public Property Sedute() As List(Of ParsecAtt.Seduta)
        Get
            Return CType(Session("RicercaSedutaPage_Sedute"), List(Of ParsecAtt.Seduta))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Seduta))
            Session("RicercaSedutaPage_Sedute") = value
        End Set
    End Property

    Public Property Filtro As String
        Get
            Return ViewState("Filtro")
        End Get
        Set(ByVal value As String)
            ViewState("Filtro") = value
        End Set
    End Property

    Public Property FiltraDataSeduta As String
        Get
            Return ViewState("FiltraDataSeduta")
        End Get
        Set(ByVal value As String)
            ViewState("FiltraDataSeduta") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.Sedute = Nothing
            If Not String.IsNullOrEmpty(Request.QueryString("filtro")) Then
                Me.Filtro = Request.QueryString("filtro")
            End If

            If Not String.IsNullOrEmpty(Request.QueryString("filtroDataSeduta")) Then
                Me.FiltraDataSeduta = Request.QueryString("filtroDataSeduta")
            End If

        End If

        ParsecUtility.Utility.CloseWindow(False)
    End Sub


    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Sedute&nbsp;&nbsp;&nbsp;" & If(Me.Sedute.Count > 0, "( " & Me.Sedute.Count.ToString & " )", "")

    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub SeduteGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles SeduteGridView.NeedDataSource
        If Me.Sedute Is Nothing Then
            Dim sedute As New ParsecAtt.SedutaRepository
            Me.Sedute = sedute.GetView(New ParsecAtt.FiltroSeduta With {.IdTipologiaSeduta = Me.Filtro})
            If Not String.IsNullOrEmpty(Me.FiltraDataSeduta) Then

                If Me.FiltraDataSeduta = "1" Then

                    'Dim parametri As New ParsecAdmin.ParametriRepository
                    'Dim parametroAnnoCorrente = parametri.GetByName("AnnoCorrente", ParsecAdmin.TipoModulo.ATT)
                    'Dim annoCorrente = Now.Year
                    'If Not parametroAnnoCorrente Is Nothing Then
                    '    Integer.TryParse(parametroAnnoCorrente.Valore, annoCorrente)
                    'End If
                    'If Now.Year <> annoCorrente Then
                    '    Dim ultimaDataValida As String = "31/12/"
                    '    Dim parametroUltimaDataValida As ParsecAdmin.Parametri = parametri.GetByName("UltimaDataValida", ParsecAdmin.TipoModulo.ATT)
                    '    If Not parametroUltimaDataValida Is Nothing Then
                    '        ultimaDataValida = parametroUltimaDataValida.Valore
                    '    End If
                    '    Dim data As DateTime = DateTime.Parse(ultimaDataValida & annoCorrente.ToString)
                    '    Me.Sedute = Me.Sedute.Where(Function(c) c.DataConvocazione.Date = data.Date).ToList
                    'Else

                    'Me.Sedute = Me.Sedute.Where(Function(c) c.DataConvocazione.Date >= Now.Date).ToList
                    'End If
                    'parametri.Dispose()


                End If

            End If

            sedute.Dispose()
        End If
        Me.SeduteGridView.DataSource = Me.Sedute
    End Sub

    Protected Sub SeduteGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles SeduteGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaSeduta(e.Item)
        End If
    End Sub

    Protected Sub SeduteGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles SeduteGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SelezionaSeduta(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        'Dim sedute As New ParsecAtt.SedutaRepository
        'Dim seduta As ParsecAtt.Seduta = sedute.GetById(id)
        'sedute.Dispose()
        Dim seduta As ParsecAtt.Seduta = Me.Sedute.Where(Function(c) c.Id = id).FirstOrDefault
        ParsecUtility.SessionManager.Seduta = seduta
        'ParsecUtility.Utility.ClosePopup(True)
        ParsecUtility.Utility.DoWindowClose(True)
    End Sub

#End Region


End Class
