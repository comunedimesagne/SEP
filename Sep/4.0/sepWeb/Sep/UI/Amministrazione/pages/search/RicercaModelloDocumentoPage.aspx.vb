#Region "IMPORTS"

Imports ParsecAdmin
Imports Telerik.Web.UI
Imports System.Data

#End Region

Partial Class RicercaModelloDocumentoPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

   
    Public Property ModelliDocumento() As List(Of ParsecAdmin.ModelloDocumento)
        Get
            Return CType(Session("RicercaModelloDocumentoPage_ModelliDocumento"), List(Of ParsecAdmin.ModelloDocumento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.ModelloDocumento))
            Session("RicercaModelloDocumentoPage_ModelliDocumento") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.ModelliDocumento = Nothing
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ElencoModelliDocumentoLabel.Text = "Elenco Modelli Documento &nbsp;&nbsp;&nbsp;" & If(Me.ModelliDocumentoGridView.MasterTableView.Items.Count > 0, "( " & Me.ModelliDocumentoGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ModelliDocumentoGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ModelliDocumentoGridView.NeedDataSource
        If Me.ModelliDocumento Is Nothing Then
            Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
            Dim searchTemplate As ParsecAdmin.ModelloDocumentoFiltro = Me.GetFiltro()
            Me.ModelliDocumento = modelli.GetView(searchTemplate)
            modelli.Dispose()
        End If
        Me.ModelliDocumentoGridView.DataSource = Me.ModelliDocumento
    End Sub


    Protected Sub ModelliDocumentoGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ModelliDocumentoGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaModelloDocumento(e.Item)
        End If
    End Sub

    Protected Sub ModelliDocumentoGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ModelliDocumentoGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona modello di documento"
            End If
        End If
    End Sub



#End Region

#Region "METODI PRIVATI"


    Private Function GetFiltro() As ParsecAdmin.ModelloDocumentoFiltro
        Dim filtro As New ParsecAdmin.ModelloDocumentoFiltro
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        If Not utenteCollegato.SuperUser Then
            filtro.IdUtente = utenteCollegato.Id
        End If
        filtro.Validi = True
        Return filtro
    End Function


    Private Sub SelezionaModelloDocumento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
        Dim modello As ParsecAdmin.ModelloDocumento = modelli.GetById(id)
        modelli.Dispose()
        ParsecUtility.SessionManager.ModelloDocumento = modello
        ParsecUtility.Utility.ClosePopup(True)
    End Sub


#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region



 

End Class
