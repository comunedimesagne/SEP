
Partial Class EsitiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Esiti"
    End Sub

End Class
