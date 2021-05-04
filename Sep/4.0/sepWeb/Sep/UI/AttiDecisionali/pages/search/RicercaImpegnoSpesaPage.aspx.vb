Imports Telerik.Web.UI

Partial Class RicercaImpegnoSpesaPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Public Property ImpegniSpesa() As List(Of ParsecAtt.ImpegnoSpesa)
        Get
            Return CType(Session("RicercaImpegnoSpesaPage_ImpegniSpesa"), List(Of ParsecAtt.ImpegnoSpesa))
        End Get
        Set(ByVal value As List(Of ParsecAtt.ImpegnoSpesa))
            Session("RicercaImpegnoSpesaPage_ImpegniSpesa") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.ImpegniSpesa = Nothing
        End If
    End Sub


    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Impegni Spesa&nbsp;&nbsp;&nbsp;" & If(Me.ImpegniSpesa.Count > 0, "( " & Me.ImpegniSpesa.Count.ToString & " )", "")
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ImpegniSpesaGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ImpegniSpesaGridView.NeedDataSource
        If Me.ImpegniSpesa Is Nothing Then
            Dim impegni As New ParsecAtt.ImpegnoSpesaRepository
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            ' Dim filtro As New ParsecAtt.FiltroImpegno With {.Anno = Now.Year}
            Me.ImpegniSpesa = impegni.GetViewDetermine(Nothing)
            impegni.Dispose()
        End If
        Me.ImpegniSpesaGridView.DataSource = Me.ImpegniSpesa

        If Not Me.Page.IsPostBack Then
            Me.ImpegniSpesaGridView.MasterTableView.FilterExpression = String.Format("(AnnoEsercizio = {0})", Now.Year)
            Dim column As GridColumn = ImpegniSpesaGridView.MasterTableView.GetColumnSafe("AnnoEsercizio")
            column.CurrentFilterValue = Now.Year.ToString
        End If

    End Sub


    Protected Sub ImpegniSpesaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ImpegniSpesaGridView.ItemCommand

        Select Case e.CommandName
            Case "Select"
                Me.SelezionaImpegnoSpesa(e.Item)
            Case "Preview"
                Me.VisualizzaDocumento(e.Item)
        End Select
      
    End Sub

    Private Sub VisualizzaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idDocumento As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdDocumento")
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        If Not documento Is Nothing Then
            Dim queryString As New Hashtable
            queryString.Add("Tipo", documento.IdTipologiaDocumento)
            queryString.Add("Mode", "View")
            queryString.Add("Procedura", "10")
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AttoAmministrativoPage.aspx"
            Dim parametriPagina As New Hashtable
            parametriPagina.Add("IdDocumentoIter", idDocumento)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 650, queryString, False)
        End If
        documenti.Dispose()
    End Sub

   

    Protected Sub ImpegniSpesaGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ImpegniSpesaGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SelezionaImpegnoSpesa(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim impegni As New ParsecAtt.ImpegnoSpesaRepository
        Dim filtro As New ParsecAtt.FiltroImpegno With {.Id = id}
        Dim impegno As ParsecAtt.ImpegnoSpesa = impegni.GetViewDetermine(filtro).FirstOrDefault
        impegni.Dispose()
        If Not impegno Is Nothing Then
            ParsecUtility.SessionManager.ImpegnoSpesa = impegno
            ParsecUtility.Utility.ClosePopup(True)
        End If
    End Sub

#End Region


End Class
