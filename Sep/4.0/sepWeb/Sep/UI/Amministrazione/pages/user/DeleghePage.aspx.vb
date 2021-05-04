Imports Telerik.Web.UI
Imports System.Web.Services

Partial Class DeleghePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Public Property Delegante() As ParsecAdmin.Delega
        Get
            Return CType(Session("DeleghePage_Delegante"), ParsecAdmin.Delega)
        End Get
        Set(ByVal value As ParsecAdmin.Delega)
            Session("DeleghePage_Delegante") = value
        End Set
    End Property

    Public Property Deleganti() As List(Of ParsecAdmin.Delega)
        Get
            Return CType(Session("DeleghePage_Deleganti"), List(Of ParsecAdmin.Delega))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Delega))
            Session("DeleghePage_Deleganti") = value
        End Set
    End Property

    Public Property Utenti() As List(Of ParsecAdmin.Utente)
        Get
            Return CType(Session("DeleghePage_Utenti"), List(Of ParsecAdmin.Utente))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Utente))
            Session("DeleghePage_Utenti") = value
        End Set
    End Property

    Public Property Delegati() As List(Of ParsecAdmin.Delegato)
        Get
            Return CType(Session("DeleghePage_Delegati"), List(Of ParsecAdmin.Delegato))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Delegato))
            Session("DeleghePage_Delegati") = value
        End Set
    End Property

    Public Property IdDelegatoSelezionati() As List(Of Integer)
        Get
            Return CType(Session("DeleghePage_IdDelegatoSelezionati"), List(Of Integer))
        End Get
        Set(ByVal value As List(Of Integer))
            Session("DeleghePage_IdDelegatoSelezionati") = value
        End Set
    End Property

    Public Property DelegatiSelezionati() As List(Of ParsecAdmin.Delegato)
        Get
            Return CType(Session("DeleghePage_DelegatiSelezionati"), List(Of ParsecAdmin.Delegato))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Delegato))
            Session("DeleghePage_DelegatiSelezionati") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Gestione Deleghe"
        If Not Me.Page.IsPostBack Then
            Me.Deleganti = Nothing
            Me.IdDelegatoSelezionati = New List(Of Integer)
            Me.DelegatiSelezionati = New List(Of ParsecAdmin.Delegato)
            Me.ResettaVista()

            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Delegante"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.DelegantiGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.DelegatiGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la delega selezionata?", False, Not Me.Delegante Is Nothing)
        If Not Me.Deleganti Is Nothing Then
            Me.TitoloElencoDelegantiLabel.Text = "Elenco Deleganti&nbsp;&nbsp;&nbsp;" & If(Me.Deleganti.Count > 0, "( " & Me.Deleganti.Count.ToString & " )", "")
        End If
        Me.ElencoDelegatiLabel.Text = "Delegati&nbsp;&nbsp;" & If(Me.DelegatiGridView.MasterTableView.Items.Count > 0, "(" & Me.DelegatiGridView.MasterTableView.Items.Count.ToString & ")", "")
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
                If Me.Delegante Is Nothing Then
                    ParsecUtility.Utility.MessageBox("E' necessario selezionare un delegante!", False)
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
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource


        Dim utenti As New ParsecAdmin.UserRepository

        Dim result = From utente In utenti.GetQuery
                     Where utente.LogTipoOperazione Is Nothing
                    Select utente

        If Not String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            result = result.Where(Function(c) c.Id <> CInt(Me.UtentiComboBox.SelectedValue))
        End If

        'If Not String.IsNullOrEmpty(Me.IdDeleganteTextBox.Text) Then
        '    result = result.Where(Function(c) c.Id <> CInt(Me.IdDeleganteTextBox.Text))
        'End If

        Dim view = result.AsEnumerable.Select(Function(c) New ParsecAdmin.Utente With {
                                                  .Id = c.Id,
                                                  .Nominativo = c.Cognome & " " & c.Nome
                                              }
                                          )

        view = view.Where(Function(c) Not Me.IdDelegatoSelezionati.Contains(c.Id))
        Me.Utenti = view.OrderBy(Function(c) c.Nominativo).ToList
        Me.UtentiGridView.DataSource = Me.Utenti

        utenti.Dispose()


    End Sub

#End Region

#Region "EVENTI GRIGLIA DELEGATI"

    Private Sub DelegatiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles DelegatiGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub DelegatiGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles DelegatiGridView.ItemDataBound


        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item


            Dim disabilitaAccessoScrivaniaCheckBox As CheckBox = CType(dataItem.FindControl("DisabilitaAccessoScrivaniaCheckBox"), CheckBox)
            'Dim id As Integer = dataItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
            'Dim delegato = Me.Deleganti.Where(Function(c) c.Id = id).FirstOrDefault
            Dim delegato = CType(dataItem.DataItem, ParsecAdmin.Delegato)

            disabilitaAccessoScrivaniaCheckBox.Checked = delegato.DisabilitaAccessoScrivania
        End If


    End Sub

    Protected Sub DelegatiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DelegatiGridView.NeedDataSource

        Dim utenti As New ParsecAdmin.UserRepository
        Dim deleghe As New ParsecAdmin.DelegheRepository(utenti.Context)



        Dim result = From delega In deleghe.GetQuery
                    Join utente In utenti.GetQuery
                    On delega.IdDelegato Equals utente.Id
                    Where utente.LogTipoOperazione Is Nothing
                    Select delega, utente




        If Not String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            result = result.Where(Function(c) c.delega.IdDelegante = CInt(Me.UtentiComboBox.SelectedValue))
        End If


        Dim view = result.AsEnumerable.Select(Function(c) New ParsecAdmin.Delegato With {
                                                  .Id = c.utente.Id,
                                                  .Nominativo = c.utente.Cognome & " " & c.utente.Nome,
                                                  .ValidaDa = c.delega.ValidaDa,
                                                  .ValidaA = c.delega.ValidaA,
                                                  .DisabilitaAccessoScrivania = c.delega.DisabilitaAccessoScrivania
                                              }
                                          )





        Dim result2 = From utente In utenti.GetQuery
                     Where utente.LogTipoOperazione Is Nothing
                    Select utente


        If Not String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            result2 = result2.Where(Function(c) c.Id <> CInt(Me.UtentiComboBox.SelectedValue))
        End If




        Dim view2 = result2.AsEnumerable.Select(Function(c) New ParsecAdmin.Utente With {
                                                  .Id = c.Id,
                                                  .Nominativo = c.Cognome & " " & c.Nome
                                              }
                                          )

        view2 = view2.Where(Function(c) Not Me.IdDelegatoSelezionati.Contains(c.Id))
        Dim elencoUtenti = view2.Select(Function(c) c.Id).ToList

        If Not String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            If Not elencoUtenti.Contains(CInt(Me.UtentiComboBox.SelectedValue)) Then
                elencoUtenti.Add(CInt(Me.UtentiComboBox.SelectedValue))
            End If

        End If

        Me.Delegati = view.Where(Function(c) Not elencoUtenti.Contains(c.Id)).GroupBy(Function(c) c.Id).Select(Function(c) c.FirstOrDefault).ToList

        For Each d In DelegatiSelezionati
            Dim idDelegato = d.Id
            Dim n = Me.Delegati.Where(Function(c) c.Id = idDelegato).FirstOrDefault()
            If Not n Is Nothing Then
                n.ValidaA = d.ValidaA
                n.ValidaDa = d.ValidaDa
            Else
                Me.Delegati.Add(d)
            End If
        Next

        Me.DelegatiGridView.DataSource = Me.Delegati.OrderBy(Function(c) c.Nominativo)

        utenti.Dispose()
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub DelegantiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DelegantiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
        End Select
    End Sub

    Protected Sub DelegantiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DelegantiGridView.NeedDataSource
        If Me.Deleganti Is Nothing Then
            Dim deleghe As New ParsecAdmin.DelegheRepository
            Me.Deleganti = deleghe.GetDeleganti(Nothing)
            deleghe.Dispose()
        End If
        Me.DelegantiGridView.DataSource = Me.Deleganti
    End Sub

    Protected Sub DelegantiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DelegantiGridView.ItemCreated
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
        Dim deleghe As New ParsecAdmin.DelegheRepository
        Dim filtro As ParsecAdmin.DelegaFiltro = Me.GetFiltro
        Me.Deleganti = deleghe.GetDeleganti(filtro)
        Me.DelegantiGridView.Rebind()
        deleghe.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.DelegaFiltro
        Dim filtro As New ParsecAdmin.DelegaFiltro
        'If Not String.IsNullOrEmpty(Me.IdDeleganteTextBox.Text) Then
        '    filtro.IdDelegante = CInt(Me.IdDeleganteTextBox.Text)
        'End If

        If Not String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            filtro.IdDelegante = CInt(Me.UtentiComboBox.SelectedValue)
        End If
        Return filtro
    End Function

    Private Sub Delete()
        Dim deleghe As New ParsecAdmin.DelegheRepository
        Try
            Dim filtro As New ParsecAdmin.DelegaFiltro With {.IdDelegante = Me.Delegante.IdDelegante}
            deleghe.DeleteAll(filtro)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            deleghe.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.DataValiditaInizioTextBox.SelectedDate = Now
        Me.DataValiditaFineTextBox.SelectedDate = Now.AddYears(5)
        Me.UtentiComboBox.Items.Clear()
        Me.UtentiComboBox.Text = String.Empty
        'Me.DeleganteTextBox.Text = String.Empty
        'Me.IdDeleganteTextBox.Text = String.Empty
        Me.Delegati = New List(Of ParsecAdmin.Delegato)
        Me.Utenti = New List(Of ParsecAdmin.Utente)
        Me.Delegante = Nothing
        Me.IdDelegatoSelezionati.Clear()
        Me.DelegatiSelezionati.Clear()
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub


    Private Sub AggiornaVista(ByVal delega As ParsecAdmin.Delega)
        Me.DelegatiSelezionati.Clear()

        'Me.DeleganteTextBox.Text = delega.Delegante
        'Me.IdDeleganteTextBox.Text = delega.IdDelegante
        'Me.DataValiditaInizioTextBox.SelectedDate = delega.ValidaDa
        'Me.DataValiditaFineTextBox.SelectedDate = delega.ValidaA

        Me.UtentiComboBox.Items.Clear()
        Dim item As New RadComboBoxItem(delega.Delegante, delega.IdDelegante.ToString)
        Me.UtentiComboBox.Items.Add(item)
        Me.UtentiComboBox.SelectedIndex = 0
        item.Selected = True
        Me.UtentiComboBox.Text = delega.Delegante

        Dim deleghe As New ParsecAdmin.DelegheRepository
        Me.IdDelegatoSelezionati = deleghe.GetQuery.Where(Function(c) c.IdDelegante = delega.IdDelegante).Select(Function(c) c.IdDelegato).ToList
        deleghe.Dispose()
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idDelega As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim deleghe As New ParsecAdmin.DelegheRepository
        Me.Delegante = deleghe.GetDeleganteById(idDelega)
        deleghe.Dispose()
        Me.AggiornaVista(Me.Delegante)
    End Sub

    Private Sub AggiornaGriglia()
        Me.Deleganti = Nothing
        Me.DelegantiGridView.Rebind()
    End Sub

    Private Sub Convalida()
        Dim message As New StringBuilder
        'If String.IsNullOrEmpty(Me.IdDeleganteTextBox.Text) Then
        If String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            message.AppendLine("E' necessario specificare il delegante!")
        Else
            'Dim idDelegante As Integer = CInt(Me.IdDeleganteTextBox.Text)
            Dim idDelegante As Integer = CInt(Me.UtentiComboBox.SelectedValue)
            Dim delegante As String = Me.UtentiComboBox.Text 'Me.DeleganteTextBox.Text
            Dim deleghe As New ParsecAdmin.DelegheRepository
            Dim exist As Boolean = False
            If Me.Delegante Is Nothing Then
                exist = Not deleghe.Where(Function(c) c.IdDelegante = idDelegante).FirstOrDefault Is Nothing
            End If
            If exist Then
                message.AppendLine("Il delegante " & delegante & " possiede già dei delegati!")
            End If
        End If

        If Me.Delegati.Count = 0 Then
            message.AppendLine("E' necessario specificare almeno un delegato!")
        End If

        'If Not Me.DataValiditaInizioTextBox.SelectedDate.HasValue Then
        '    message.AppendLine("E' necessario specificare la data di inizio validità!")
        'End If

        'If Not Me.DataValiditaFineTextBox.SelectedDate.HasValue Then
        '    message.AppendLine("E' necessario specificare la data di fine validità!")
        'End If

        If message.Length > 0 Then
            Throw New ApplicationException(message.ToString)
        End If
    End Sub

    Private Sub Save()

        Me.Convalida()

        Dim deleghe As New ParsecAdmin.DelegheRepository
        Try
            Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

            Dim nuovo As Boolean = Me.Delegante Is Nothing

            Me.Delegante = deleghe.CreateFromInstance(Me.Delegante)

            If Not nuovo Then
                Dim filtro As New ParsecAdmin.DelegaFiltro With {.IdDelegante = Me.Delegante.IdDelegante}
                deleghe.DeleteAll(filtro)
            Else
                Me.Delegante.ValidaDa = Now
                Me.Delegante.ValidaA = Now.AddYears(5)
            End If

            'Me.Delegante.IdDelegante = CInt(IdDeleganteTextBox.Text)
            Me.Delegante.IdDelegante = CInt(Me.UtentiComboBox.SelectedValue)
            Me.Delegante.IdUtenteIns = utenteCollegato.Id


            '************************************************************************************
            'AGGIORNO LA LISTA DEI DELEGATI
            '************************************************************************************
            Dim disabilitaAccessoScrivaniaCheckBox As CheckBox = Nothing
            Dim id As Integer = 0
            Dim delegato As ParsecAdmin.Delegato = Nothing

            For Each dataItem As GridDataItem In Me.DelegatiGridView.MasterTableView.Items

                id = dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("Id")
                delegato = Me.Delegati.Where(Function(c) c.Id = id).FirstOrDefault

                disabilitaAccessoScrivaniaCheckBox = CType(dataItem.FindControl("DisabilitaAccessoScrivaniaCheckBox"), CheckBox)
                delegato.DisabilitaAccessoScrivania = disabilitaAccessoScrivaniaCheckBox.Checked

            Next
            '************************************************************************************





            Me.Delegante.Delegati = Me.Delegati
            deleghe.SaveAll(Me.Delegante)

            '*******************************************************************
            'Aggiorno l'oggetto corrente.
            '*******************************************************************

            Me.Delegante = deleghe.GetDeleganti(New ParsecAdmin.DelegaFiltro With {.IdDelegante = Me.Delegante.IdDelegante}).FirstOrDefault
            Me.AggiornaVista(Me.Delegante)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            deleghe.Dispose()
        End Try
    End Sub


#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub TrovaDeleganteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaDeleganteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaDeleganteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim parametri As New Hashtable
        parametri("tipoSelezione") = 0 'singola
        Session("Parametri") = parametri
    End Sub

    Protected Sub AggiornaDeleganteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaDeleganteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim idDelegato As Integer = utentiSelezionati.First.Key
            ' Me.DeleganteTextBox.Text = utentiSelezionati.First.Value
            'Me.IdDeleganteTextBox.Text = idDelegato
            Me.UtentiGridView.Rebind()
            Me.DelegatiGridView.Rebind()
            Session.Remove("SelectedUsers")
        End If
    End Sub

    Protected Sub EliminaDeleganteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaDeleganteImageButton.Click
        ' Me.DeleganteTextBox.Text = String.Empty
        'Me.IdDeleganteTextBox.Text = String.Empty
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub

    Protected Sub AggiungiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiImageButton.Click
        If Me.UtentiGridView.SelectedIndexes.Count = 0 Then
            Exit Sub
        End If

        Dim message As New StringBuilder

        If Not Me.DataValiditaInizioTextBox.SelectedDate.HasValue Then
            message.AppendLine("E' necessario specificare la data di inizio validità!")
        End If

        If Not Me.DataValiditaFineTextBox.SelectedDate.HasValue Then
            message.AppendLine("E' necessario specificare la data di fine validità!")
        End If

        If message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(message.ToString, False)
            Exit Sub
        End If

        Dim item As GridDataItem = Nothing
        For i As Integer = 0 To Me.UtentiGridView.SelectedIndexes.Count - 1
            item = CType(Me.UtentiGridView.Items(Me.UtentiGridView.SelectedIndexes(i)), GridDataItem)
            Dim idUtente As Integer = CInt(item("Id").Text)
            Me.IdDelegatoSelezionati.Add(idUtente)

            Dim delegato As New ParsecAdmin.Delegato
            delegato.Nominativo = item("NominativoHidden").Text
            delegato.ValidaDa = Me.DataValiditaInizioTextBox.SelectedDate
            delegato.ValidaA = Me.DataValiditaFineTextBox.SelectedDate
            delegato.Id = idUtente


            Me.DelegatiSelezionati.Add(delegato)



        Next
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub

    Protected Sub EliminaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaImageButton.Click
        If Me.DelegatiGridView.SelectedIndexes.Count = 0 Then
            Exit Sub
        End If
        Dim item As GridDataItem = Nothing
        For i As Integer = 0 To Me.DelegatiGridView.SelectedIndexes.Count - 1
            item = CType(Me.DelegatiGridView.Items(Me.DelegatiGridView.SelectedIndexes(i)), GridDataItem)
            Dim idUtente As Integer = CInt(item("Id").Text)
            Me.IdDelegatoSelezionati.Remove(idUtente)
            Dim delegato = Me.DelegatiSelezionati.Where(Function(c) c.Id = idUtente).FirstOrDefault
            If Not delegato Is Nothing Then
                Me.DelegatiSelezionati.Remove(delegato)
            End If



        Next
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub

    Protected Sub AggiungiTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiTuttoImageButton.Click

        Dim message As New StringBuilder

        If Not Me.DataValiditaInizioTextBox.SelectedDate.HasValue Then
            message.AppendLine("E' necessario specificare la data di inizio validità!")
        End If

        If Not Me.DataValiditaFineTextBox.SelectedDate.HasValue Then
            message.AppendLine("E' necessario specificare la data di fine validità!")
        End If

        If message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(message.ToString, False)
            Exit Sub
        End If


        If Me.UtentiGridView.MasterTableView.Items.Count = 0 Then
            Exit Sub
        End If
        For Each Utente In Me.Utenti
            Me.IdDelegatoSelezionati.Add(Utente.Id)

            Dim delegato As New ParsecAdmin.Delegato
            delegato.Nominativo = Utente.Nominativo
            delegato.ValidaDa = Me.DataValiditaInizioTextBox.SelectedDate
            delegato.ValidaA = Me.DataValiditaFineTextBox.SelectedDate
            delegato.Id = Utente.Id


            Me.DelegatiSelezionati.Add(delegato)

        Next
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub

    Protected Sub EliminaTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaTuttoImageButton.Click
        If Me.DelegatiGridView.MasterTableView.Items.Count = 0 Then
            Exit Sub
        End If
        Me.IdDelegatoSelezionati.Clear()
        Me.DelegatiSelezionati.Clear()
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub

    Private Const ItemsPerRequest As Integer = 10

    <WebMethod()> _
    Public Shared Function GetUtenti(ByVal context As RadComboBoxContext) As RadComboBoxData
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim utenti As New ParsecAdmin.UserRepository

        Dim data As List(Of ParsecAdmin.Utente) = Nothing

        Dim view = From utente In utenti.GetQuery
                   Where utente.LogTipoOperazione Is Nothing
                   Order By (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))

        'If utenteCorrente.SuperUser Then
        '    view = view.Where(Function(c) c.SuperUser = False)
        'End If


        data = view.AsEnumerable.Select(Function(c) New ParsecAdmin.Utente With {.Id = c.Id, .Nominativo = If(c.Cognome = Nothing, "", c.Cognome) + " " + If(c.Nome = Nothing, "", c.Nome)}).ToList

        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            Dim item = data.ElementAt(i)
            itemData.Text = item.Nominativo
            itemData.Value = item.Id
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function

    Protected Sub UtentiComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles UtentiComboBox.SelectedIndexChanged
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub


#End Region


End Class