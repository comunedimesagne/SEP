Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class RicercaStoricoDocumentiFascicoloPage
    Inherits System.Web.UI.Page



    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        '    Me.TitoloLabel.Text = "Elenco Procedure " & If(Me.ProcedureGridView.MasterTableView.Items.Count > 0, "( " & Me.ProcedureGridView.MasterTableView.Items.Count.ToString & " )", "")
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim documentiFascicolo As New ParsecAdmin.FascicoloDocumentoRepository
        Dim idFascicolo As Integer = CInt(Request.QueryString("id"))
        Me.StoricoDocumentiFascicoloGridView.DataSource = documentiFascicolo.GetQuery.Where(Function(c) c.IdFascicolo = idFascicolo)
        ' documentiFascicolo.Dispose()

    End Sub


    'Protected Sub StoricoDocumentiFascicoloGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles StoricoDocumentiFascicoloGridView.ItemDataBound
    '    Dim btn As Image = Nothing
    '    If (e.Item.ItemType = GridItemType.Item OrElse e.Item.ItemType = GridItemType.AlternatingItem OrElse e.Item.ItemType = GridItemType.SelectedItem) Then
    '        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
    '            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
    '            If TypeOf dataItem("DocumentType").Controls(0) Is Image Then
    '                Dim fasc As ParsecAdmin.FascicoloDocumento = CType(e.Item.DataItem, ParsecAdmin.FascicoloDocumento)
    '                'Dim filename As String = fasc.Immagine
    '                btn = CType(dataItem("DocumentType").Controls(0), Image)
    '                Dim utenti As New ParsecAdmin.UserRepository
    '                Dim utente As ParsecAdmin.Utente = (New ParsecAdmin.UserRepository).GetQuery.Where(Function(c) c.Id = fasc.IdutenteCancellazione).FirstOrDefault
    '                btn.ToolTip = "Inserito -" & String.Format("{0:dd/MM/yyyy}", fasc.DataDocumento) & "- Cancellato -" & String.Format("{0:dd/MM/yyyy}", fasc.DataOraCancellazione) & "- Utente '" & utente.Username & "'"
    '                'btn.ImageUrl = filename
    '            End If
    '        End If
    '    End If

    'End Sub


End Class
