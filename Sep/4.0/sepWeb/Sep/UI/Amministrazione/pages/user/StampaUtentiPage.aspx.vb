
Partial Class StampaUtentiPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Stampa Utenti"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Print()
    End Sub


    Private Sub Print()
        Dim utenti As New ParsecAdmin.UserRepository
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaUtenti")
        parametriStampa.Add("DatiStampa", utenti.GetUtentiPerStampa)
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
        utenti.Dispose()
    End Sub

End Class
