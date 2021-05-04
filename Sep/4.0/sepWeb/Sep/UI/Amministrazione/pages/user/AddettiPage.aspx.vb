Imports Telerik.Web.UI

Partial Class AddettiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Public Property Responsabile() As ParsecAdmin.Addetto
        Get
            Return CType(Session("AddettiPage_Responsabile"), ParsecAdmin.Addetto)
        End Get
        Set(ByVal value As ParsecAdmin.Addetto)
            Session("AddettiPage_Responsabile") = value
        End Set
    End Property

    Public Property Responsabili() As List(Of ParsecAdmin.Addetto)
        Get
            Return CType(Session("AddettiPage_Responsabili"), List(Of ParsecAdmin.Addetto))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Addetto))
            Session("AddettiPage_Responsabili") = value
        End Set
    End Property

    Public Property Addetti() As List(Of ParsecAdmin.Addetto)
        Get
            Return CType(Session("AddettiPage_Addetti"), List(Of ParsecAdmin.Addetto))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Addetto))
            Session("AddettiPage_Addetti") = value
        End Set
    End Property

    Public Property Utenti() As List(Of ParsecAdmin.Utente)
        Get
            Return CType(Session("AddettiPage_Utenti"), List(Of ParsecAdmin.Utente))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Utente))
            Session("AddettiPage_Utenti") = value
        End Set
    End Property

    Public Property IdAddettiSelezionati() As List(Of Integer)
        Get
            Return CType(Session("AddettiPage_IdAddettiSelezionati"), List(Of Integer))
        End Get
        Set(ByVal value As List(Of Integer))
            Session("AddettiPage_IdAddettiSelezionati") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Gestione Addetti"
        If Not Me.Page.IsPostBack Then
            Me.Responsabili = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Responsabile"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.ResponsabiliGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If
        'Me.scrollPanelUtenti.Attributes.Add("onscroll", "OnScrollUtenti(this)")

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("height", "332px")

        Me.PannelloContenitoreGriglia.Style.Add("width", widthStyle)
        Me.PannelloContenitoreGriglia.Style.Add("height", "300px")

        Me.AddettiGridView.Style.Add("width", widthStyle)
        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.ResponsabiliGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'addetto selezionato?", False, Not Me.Responsabile Is Nothing)
        If Not Me.Responsabili Is Nothing Then
            Me.TitoloElencoResponsabiliLabel.Text = "Elenco Responsabili&nbsp;&nbsp;&nbsp;" & If(Me.Responsabili.Count > 0, "( " & Me.Responsabili.Count.ToString & " )", "")
        End If
        Me.ElencoAddettiLabel.Text = "Addetti&nbsp;&nbsp;" & If(Me.AddettiGridView.MasterTableView.Items.Count > 0, "(" & Me.AddettiGridView.MasterTableView.Items.Count.ToString & ")", "")
        Me.ElencoUtentiLabel.Text = "Utenti&nbsp;&nbsp;" & If(Me.UtentiGridView.MasterTableView.Items.Count > 0, "(" & Me.UtentiGridView.MasterTableView.Items.Count.ToString & ")", "")
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                '  Me.Print()

            Case "Salva"
                Dim message As String = String.Empty
                Try
                    Me.Save()
                    Me.AggiornaGriglia()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                If String.IsNullOrEmpty(message) Then
                    Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                Else
                    ParsecUtility.Utility.MessageBox(message, False)
                End If

            Case "Nuovo"
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Annulla"
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Elimina"
                Dim message As String = String.Empty
                If Me.Responsabile Is Nothing Then
                    ParsecUtility.Utility.MessageBox("E' necessario selezionare un addetto!", False)
                    Exit Sub
                End If

                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    Try
                        Me.Delete()
                        Me.ResettaVista()
                        Me.AggiornaGriglia()
                    Catch ex As Exception
                        message = ex.Message
                    End Try

                    If String.IsNullOrEmpty(message) Then
                        Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                    Else
                        ParsecUtility.Utility.MessageBox(message, False)
                    End If

                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"

            Case "Trova"
                Me.Search()

        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA UTENTI"

    Private Sub UtentiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then

            Dim browser = Request.Browser.Browser
            If Not browser.ToLower.Contains("ie") Then
            Else
                e.Item.Style.Add("position", "relative")
                e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
                e.Item.Style.Add("z-index", "99")
                e.Item.Style.Add("background-color", "White")
            End If


        End If
    End Sub

    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource


        Dim utenti As New ParsecAdmin.UserRepository

        Dim result = From utente In utenti.GetQuery
                     Where utente.LogTipoOperazione Is Nothing
                    Select utente


        If Not String.IsNullOrEmpty(Me.IdUtenteTextBox.Text) Then
            result = result.Where(Function(c) c.Id <> CInt(Me.IdUtenteTextBox.Text))
        End If

        Dim view = result.AsEnumerable.Select(Function(c) New ParsecAdmin.Utente With {
                                                  .Id = c.Id,
                                                  .Nominativo = c.Cognome & " " & c.Nome
                                              }
                                          )

        view = view.Where(Function(c) Not Me.IdAddettiSelezionati.Contains(c.Id))
        Me.Utenti = view.OrderBy(Function(c) c.Nominativo).ToList
        Me.UtentiGridView.DataSource = Me.Utenti

        utenti.Dispose()


    End Sub

#End Region

#Region "EVENTI GRIGLIA ADDETTI"

    Private Sub AddettiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles AddettiGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub AddettiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles AddettiGridView.NeedDataSource
        Dim utenti As New ParsecAdmin.UserRepository

        Dim result = From utente In utenti.GetQuery
                     Where utente.LogTipoOperazione Is Nothing
                    Select utente


        If Not String.IsNullOrEmpty(Me.IdUtenteTextBox.Text) Then
            result = result.Where(Function(c) c.Id <> CInt(Me.IdUtenteTextBox.Text))
        End If

        Dim view = result.AsEnumerable.Select(Function(c) New ParsecAdmin.Addetto With {
                                                  .IdAddetto = c.Id,
                                                  .Addetto = c.Cognome & " " & c.Nome
                                              }
                                          )

        view = view.Where(Function(c) Me.IdAddettiSelezionati.Contains(c.IdAddetto))

        Me.Addetti = view.OrderBy(Function(c) c.Addetto).ToList
        Me.AddettiGridView.DataSource = Me.Addetti

        utenti.Dispose()
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ResponsabiliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ResponsabiliGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
        End Select
    End Sub


    Protected Sub ResponsabiliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ResponsabiliGridView.NeedDataSource
        If Me.Responsabili Is Nothing Then
            Dim addetti As New ParsecAdmin.AddettiRepository
            Me.Responsabili = addetti.GetView(Nothing)
            addetti.Dispose()
        End If
        Me.ResponsabiliGridView.DataSource = Me.Responsabili
    End Sub

    Protected Sub ResponsabiliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ResponsabiliGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub Search()
        Dim addetti As New ParsecAdmin.AddettiRepository
        Dim filtro As ParsecAdmin.FiltroAddetto = Me.GetFiltro
        Me.Responsabili = addetti.GetView(filtro)
        Me.ResponsabiliGridView.Rebind()
        addetti.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FiltroAddetto
        Dim filtro As New ParsecAdmin.FiltroAddetto
        If Not String.IsNullOrEmpty(Me.IdUtenteTextBox.Text) Then
            filtro.IdResponsabile = CInt(Me.IdUtenteTextBox.Text)
        End If
        Return filtro
    End Function


    Private Sub Delete()
        Dim addetti As New ParsecAdmin.AddettiRepository
        Try
            addetti.DeleteAll(Me.Responsabile.IdResponsabile)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            addetti.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.UtenteTextBox.Text = String.Empty
        Me.IdUtenteTextBox.Text = String.Empty
        Me.Addetti = New List(Of ParsecAdmin.Addetto)
        Me.Utenti = New List(Of ParsecAdmin.Utente)
        Me.Responsabile = Nothing
        'Me.IdAddettiSelezionati.Clear()
        Me.IdAddettiSelezionati = New List(Of Integer)
        Me.UtentiGridView.Rebind()
        Me.AddettiGridView.Rebind()
    End Sub


    Private Sub AggiornaVista(ByVal responsabile As ParsecAdmin.Addetto)
        Me.UtenteTextBox.Text = responsabile.Responsabile
        Me.IdUtenteTextBox.Text = responsabile.IdResponsabile
        Dim addetti As New ParsecAdmin.AddettiRepository
        Me.IdAddettiSelezionati = addetti.GetQuery.Where(Function(c) c.IdResponsabile = responsabile.IdResponsabile).Select(Function(c) c.IdAddetto).ToList
        addetti.Dispose()
        Me.UtentiGridView.Rebind()
        Me.AddettiGridView.Rebind()
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim addetti As New ParsecAdmin.AddettiRepository
        Me.Responsabile = addetti.GetById(id)
        addetti.Dispose()
        Me.AggiornaVista(Me.Responsabile)
    End Sub

    Private Sub AggiornaGriglia()
        Me.Responsabili = Nothing
        Me.ResponsabiliGridView.Rebind()
    End Sub

    Private Sub Save()
        Dim nuovo As Boolean = Me.Responsabile Is Nothing

        Dim addetti As New ParsecAdmin.AddettiRepository

        If Not String.IsNullOrEmpty(Me.IdUtenteTextBox.Text) Then
            For Each addetto As ParsecAdmin.Addetto In Me.Addetti
                addetto.IdResponsabile = CInt(Me.IdUtenteTextBox.Text)
            Next
        End If

        Try
            Me.Responsabile = addetti.CreateFromInstance(Me.Responsabile)

            If Not String.IsNullOrEmpty(Me.IdUtenteTextBox.Text) Then
                Me.Responsabile.IdResponsabile = CInt(Me.IdUtenteTextBox.Text)
            End If

            addetti.Responsabile = Me.Responsabile
            addetti.Save(Me.Addetti)

            '*******************************************************************
            'Aggiorno l'oggetto corrente.
            '*******************************************************************
            Me.Responsabile = addetti.GetView(New ParsecAdmin.FiltroAddetto With {.IdResponsabile = Me.Responsabile.IdResponsabile}).FirstOrDefault
            Me.AggiornaVista(Me.Responsabile)

        Catch ex As Exception
            If nuovo Then
                Me.Responsabile = Nothing
            End If
            Throw New ApplicationException(ex.Message)
        Finally
            addetti.Dispose()
        End Try
    End Sub


#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub TrovaDeleganteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim parametri As New Hashtable
        parametri("tipoSelezione") = 0 'singola
        Session("Parametri") = parametri
    End Sub

    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim idDelegato As Integer = utentiSelezionati.First.Key
            Me.UtenteTextBox.Text = utentiSelezionati.First.Value
            Me.IdUtenteTextBox.Text = idDelegato
            Me.UtentiGridView.Rebind()
            Me.AddettiGridView.Rebind()
            Session.Remove("SelectedUsers")
        End If
    End Sub

    Protected Sub EliminaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteImageButton.Click
        Me.UtenteTextBox.Text = String.Empty
        Me.IdUtenteTextBox.Text = String.Empty
        Me.UtentiGridView.Rebind()
        Me.AddettiGridView.Rebind()
    End Sub

    Protected Sub AggiungiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiImageButton.Click
        If Me.UtentiGridView.SelectedIndexes.Count = 0 Then
            Exit Sub
        End If
        Dim item As GridDataItem = Nothing
        For i As Integer = 0 To Me.UtentiGridView.SelectedIndexes.Count - 1
            item = CType(Me.UtentiGridView.Items(Me.UtentiGridView.SelectedIndexes(i)), GridDataItem)
            Dim idUtente As Integer = CInt(item("Id").Text)
            Me.IdAddettiSelezionati.Add(idUtente)
        Next
        Me.UtentiGridView.Rebind()
        Me.AddettiGridView.Rebind()
    End Sub

    Protected Sub EliminaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaImageButton.Click
        If Me.AddettiGridView.SelectedIndexes.Count = 0 Then
            Exit Sub
        End If
        Dim item As GridDataItem = Nothing
        For i As Integer = 0 To Me.AddettiGridView.SelectedIndexes.Count - 1
            item = CType(Me.AddettiGridView.Items(Me.AddettiGridView.SelectedIndexes(i)), GridDataItem)
            'Aggiungere un GridBoundColumn con UniqueName IdAddetto
            'Dim idUtente As Integer = CInt(item("IdAddetto").Text)
            'Oppure
            Dim idUtente = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdAddetto")
            Me.IdAddettiSelezionati.Remove(idUtente)
        Next
        Me.UtentiGridView.Rebind()
        Me.AddettiGridView.Rebind()
    End Sub

    Protected Sub AggiungiTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiTuttoImageButton.Click
        If Me.UtentiGridView.MasterTableView.Items.Count = 0 Then
            Exit Sub
        End If
        For Each Utente In Me.Utenti
            Me.IdAddettiSelezionati.Add(Utente.Id)
        Next
        Me.UtentiGridView.Rebind()
        Me.AddettiGridView.Rebind()
    End Sub

    Protected Sub EliminaTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaTuttoImageButton.Click
        If Me.AddettiGridView.MasterTableView.Items.Count = 0 Then
            Exit Sub
        End If
        Me.IdAddettiSelezionati.Clear()
        Me.UtentiGridView.Rebind()
        Me.AddettiGridView.Rebind()
    End Sub

#End Region

End Class