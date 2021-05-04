#Region "Namespaces importati"

Imports Telerik.Web.UI
Imports ParsecWKF

#End Region

Partial Class VisualizzaUtentiIstanzaPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.IsPostBack Then
            Dim vis As New VisualizzazioneRepository
            Me.Visualizzazione = vis.GetUtentiIstanza(CInt(Request.QueryString("ID")))
            vis.Dispose()
            Me.UtentiIstanzaGridView.DataSource = Me.Visualizzazione
            Me.Lbl.Text = Request.QueryString("RIF")
        End If
    End Sub

    Public Property Visualizzazione() As List(Of VisualizzazioneStorico)
        Get
            Return CType(Session("StoricoPage_Visualizzazione"), List(Of VisualizzazioneStorico))
        End Get
        Set(ByVal value As List(Of VisualizzazioneStorico))
            Session("StoricoPage_Visualizzazione") = value
        End Set
    End Property

    Protected Sub UtentiIstanzaGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiIstanzaGridView.NeedDataSource
        Dim vis As New VisualizzazioneRepository
        Me.Visualizzazione = vis.GetUtentiIstanza(CInt(Request.QueryString("ID")))
        vis.Dispose()
        Me.UtentiIstanzaGridView.DataSource = Me.Visualizzazione
    End Sub

    Protected Sub imbEsportaPDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbEsportaPDF.Click
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "VisualizzaIstanzaStorico")
        parametriStampa.Add("DatiStampa", Me.Visualizzazione)
        parametriStampa.Add("Riferimento", Request.QueryString("RIF"))
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

End Class