Imports ParsecAdmin
Imports ParsecPro

Partial Class UtentiConnessiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Private clienteCorrente As ParsecAdmin.Cliente

    Private Function GetUtentiConnessi() As List(Of ParsecAdmin.Utente)
        Dim utenti As New ParsecAdmin.UserRepository
        Dim utentiConnessi As New List(Of ParsecAdmin.Utente)
        For Each item As KeyValuePair(Of Integer, String) In ParsecUtility.UtentiConnessi.Items
            Dim utente As ParsecAdmin.Utente = utenti.GetUserById(item.Key).FirstOrDefault
            utente.Nome = utente.Cognome & " " & utente.Nome
            utentiConnessi.Add(utente)
        Next
        utenti.Dispose()
        Return utentiConnessi
    End Function


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Verifica Utenti Connessi"
    End Sub

    Protected Sub UtentiConessiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiConessiGridView.NeedDataSource
        Dim utenti = Me.GetUtentiConnessi
        Me.UtentiConessiGridView.DataSource = utenti

        Me.TitoloLabel.Text = "Utenti Connessi&nbsp;&nbsp;&nbsp;" & If(utenti.Count > 0, "( " & utenti.Count.ToString & " )", "")
    End Sub

    Protected Sub UtentiConessiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiConessiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Disconnnetti utente"
                btn.Attributes.Add("onclick", "return Confirm();")
                Dim utente As ParsecAdmin.Utente = CType(e.Item.DataItem, ParsecAdmin.Utente)
                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                If utente.Id = utenteCollegato.Id Then
                    btn.Visible = False
                    dataItem("Delete").Controls.Add(New LiteralControl("&nbsp;"))
                End If
            End If
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Disconnettere l'utente selezionato?", False, True)
    End Sub

    Protected Sub UtentiConessiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UtentiConessiGridView.ItemCommand
        If e.CommandName = "Delete" Then
            ParsecUtility.UtentiConnessi.Delete(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"))
            UtentiConessiGridView.Rebind()
        End If
    End Sub

    Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        UtentiConessiGridView.Rebind()
        TimeLabel.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Now)
    End Sub

End Class