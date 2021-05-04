Imports Telerik.Web.UI



Partial Class DelegatiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Public Property Delegato() As ParsecAdmin.Delega
        Get
            Return CType(Session("DelegatiPage_Delegato"), ParsecAdmin.Delega)
        End Get
        Set(ByVal value As ParsecAdmin.Delega)
            Session("DelegatiPage_Delegato") = value
        End Set
    End Property

    Public Property Delegati() As List(Of ParsecAdmin.Delega)
        Get
            Return CType(Session("DelegatiPage_Delegati"), List(Of ParsecAdmin.Delega))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Delega))
            Session("DelegatiPage_Delegati") = value
        End Set
    End Property


    Public Property Utenti() As List(Of ParsecAdmin.Utente)
        Get
            Return CType(Session("DelegatiPage_Utenti"), List(Of ParsecAdmin.Utente))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Utente))
            Session("DelegatiPage_Utenti") = value
        End Set
    End Property

    Public Property Deleganti() As List(Of ParsecAdmin.Delegante)
        Get
            Return CType(Session("DelegatiPage_Deleganti"), List(Of ParsecAdmin.Delegante))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Delegante))
            Session("DelegatiPage_Deleganti") = value
        End Set
    End Property


    Public Property IdDeleganteSelezionati() As List(Of Integer)
        Get
            Return CType(Session("DelegatiPage_IdDeleganteSelezionati"), List(Of Integer))
        End Get
        Set(ByVal value As List(Of Integer))
            Session("DelegatiPage_IdDeleganteSelezionati") = value
        End Set
    End Property

    Public Property DelegantiSelezionati() As List(Of ParsecAdmin.Delegante)
        Get
            Return CType(Session("DelegatiPage_DelegantiSelezionati"), List(Of ParsecAdmin.Delegante))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Delegante))
            Session("DelegatiPage_DelegantiSelezionati") = value
        End Set
    End Property

   

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Gestione Delegati"
        If Not Me.Page.IsPostBack Then
            Me.Delegati = Nothing
            Me.IdDeleganteSelezionati = New List(Of Integer)
            Me.DelegantiSelezionati = New List(Of ParsecAdmin.Delegante)
            Me.ResettaVista()

            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Delegato"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.DelegatiGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.DelegantiGridView.Style.Add("width", widthStyle)

     
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la delega selezionata?", False, Not Me.Delegato Is Nothing)
        If Not Me.Delegati Is Nothing Then
            Me.TitoloElencoDelegatiLabel.Text = "Elenco Delegati&nbsp;&nbsp;&nbsp;" & If(Me.Delegati.Count > 0, "( " & Me.Delegati.Count.ToString & " )", "")
        End If
        Me.ElencoDelegantiLabel.Text = "Deleganti&nbsp;&nbsp;" & If(Me.DelegantiGridView.MasterTableView.Items.Count > 0, "(" & Me.DelegantiGridView.MasterTableView.Items.Count.ToString & ")", "")
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
                If Me.Delegato Is Nothing Then
                    ParsecUtility.Utility.MessageBox("E' necessario selezionare un delegato!", False)
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


        If Not String.IsNullOrEmpty(Me.IdDelegatoTextBox.Text) Then
            result = result.Where(Function(c) c.Id <> CInt(Me.IdDelegatoTextBox.Text))
        End If

        Dim view = result.AsEnumerable.Select(Function(c) New ParsecAdmin.Utente With {
                                                  .Id = c.Id,
                                                  .Nominativo = c.Cognome & " " & c.Nome
                                              }
                                          )

        view = view.Where(Function(c) Not Me.IdDeleganteSelezionati.Contains(c.Id))
        Me.Utenti = view.OrderBy(Function(c) c.Nominativo).ToList
        Me.UtentiGridView.DataSource = Me.Utenti

        utenti.Dispose()


    End Sub



#End Region

#Region "EVENTI GRIGLIA DELEGANTI"

    Protected Sub UtentiComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles UtentiComboBox.SelectedIndexChanged
        Me.UtentiGridView.Rebind()
        Me.DelegatiGridView.Rebind()
    End Sub

    Private Sub DelegantiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles DelegantiGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub DelegantiGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles DelegantiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item


            Dim disabilitaAccessoScrivaniaCheckBox As CheckBox = CType(dataItem.FindControl("DisabilitaAccessoScrivaniaCheckBox"), CheckBox)
            'Dim id As Integer = dataItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
            'Dim delegante = Me.Deleganti.Where(Function(c) c.Id = id).FirstOrDefault
            Dim delegante = CType(dataItem.DataItem, ParsecAdmin.Delegante)

            disabilitaAccessoScrivaniaCheckBox.Checked = delegante.DisabilitaAccessoScrivania
        End If

    End Sub

    Protected Sub DelegantiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DelegantiGridView.NeedDataSource


        Dim utenti As New ParsecAdmin.UserRepository
        Dim deleghe As New ParsecAdmin.DelegheRepository(utenti.Context)

       

        Dim result = From delega In deleghe.GetQuery
                  Join utente In utenti.GetQuery
                  On delega.IdDelegante Equals utente.Id
                  Where utente.LogTipoOperazione Is Nothing
                  Select delega, utente


        If Not String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            result = result.Where(Function(c) c.delega.IdDelegato = CInt(Me.UtentiComboBox.SelectedValue))
        End If



        If Not String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            result = result.Where(Function(c) c.delega.IdDelegante <> CInt(Me.UtentiComboBox.SelectedValue))
        End If


        Dim view = result.AsEnumerable.Select(Function(c) New ParsecAdmin.Delegante With {
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

        view2 = view2.Where(Function(c) Not Me.IdDeleganteSelezionati.Contains(c.Id))
        Dim elencoUtenti = view2.Select(Function(c) c.Id).ToList

        If Not String.IsNullOrEmpty(Me.IdDelegatoTextBox.Text) Then
            If Not elencoUtenti.Contains(CInt(Me.IdDelegatoTextBox.Text)) Then
                elencoUtenti.Add(CInt(Me.IdDelegatoTextBox.Text))
            End If

        End If



        Me.Deleganti = view.Where(Function(c) Not elencoUtenti.Contains(c.Id)).GroupBy(Function(c) c.Id).Select(Function(c) c.FirstOrDefault).OrderBy(Function(c) c.Nominativo).ToList


        For Each d In DelegantiSelezionati
            Dim idDelegato = d.Id
            Dim n = Me.Deleganti.Where(Function(c) c.Id = idDelegato).FirstOrDefault()
            If Not n Is Nothing Then
                n.ValidaA = d.ValidaA
                n.ValidaDa = d.ValidaDa
            Else
                Me.Deleganti.Add(d)
            End If

        Next
        Me.DelegantiGridView.DataSource = Me.Deleganti

        utenti.Dispose()
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub DelegatiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DelegatiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
        End Select
    End Sub

    Protected Sub DelegatiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DelegatiGridView.NeedDataSource
        If Me.Delegati Is Nothing Then
            Dim deleghe As New ParsecAdmin.DelegheRepository
            Me.Delegati = deleghe.GetDelegati(Nothing)
            deleghe.Dispose()
        End If
        Me.DelegatiGridView.DataSource = Me.Delegati
    End Sub

    Protected Sub DelegatiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DelegatiGridView.ItemCreated
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
        Me.Delegati = deleghe.GetDelegati(filtro)
        Me.DelegatiGridView.Rebind()
        deleghe.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.DelegaFiltro
        Dim filtro As New ParsecAdmin.DelegaFiltro
        If Not String.IsNullOrEmpty(Me.IdDelegatoTextBox.Text) Then
            filtro.IdDelegato = CInt(Me.IdDelegatoTextBox.Text)
        End If
        Return filtro
    End Function

    Private Sub Delete()
        Dim deleghe As New ParsecAdmin.DelegheRepository
        Try
            Dim filtro As New ParsecAdmin.DelegaFiltro With {.IdDelegato = Me.Delegato.IdDelegato}
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
        'Me.DelegatoTextBox.Text = String.Empty
        'Me.IdDelegatoTextBox.Text = String.Empty
        Me.Deleganti = New List(Of ParsecAdmin.Delegante)
        Me.Utenti = New List(Of ParsecAdmin.Utente)
        Me.Delegato = Nothing
        Me.IdDeleganteSelezionati.Clear()
        Me.DelegantiSelezionati.Clear()
        Me.UtentiGridView.Rebind()
        Me.DelegantiGridView.Rebind()

    End Sub




    Private Sub AggiornaVista(ByVal delega As ParsecAdmin.Delega)
        Me.DelegantiSelezionati.Clear()
        'Me.DelegatoTextBox.Text = delega.Delegato
        'Me.IdDelegatoTextBox.Text = delega.IdDelegato
        Me.UtentiComboBox.Items.Clear()

        Dim item As New RadComboBoxItem(delega.Delegato, delega.IdDelegato.ToString)
        Me.UtentiComboBox.Items.Add(item)
        Me.UtentiComboBox.SelectedIndex = 0
        item.Selected = True
        Me.UtentiComboBox.Text = delega.Delegante

        Dim deleghe As New ParsecAdmin.DelegheRepository
        Me.IdDeleganteSelezionati = deleghe.GetQuery.Where(Function(c) c.IdDelegato = delega.IdDelegato).Select(Function(c) c.IdDelegante).ToList
        deleghe.Dispose()
        Me.UtentiGridView.Rebind()
        Me.DelegantiGridView.Rebind()
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idDelega As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim deleghe As New ParsecAdmin.DelegheRepository
        Dim filtro As New ParsecAdmin.DelegaFiltro
        Me.Delegato = deleghe.GetDelegatoById(idDelega)
        deleghe.Dispose()
        Me.AggiornaVista(Me.Delegato)
    End Sub

    Private Sub AggiornaGriglia()
        Me.Delegati = Nothing
        Me.DelegatiGridView.Rebind()
    End Sub

    Private Sub Convalida()
        Dim message As New StringBuilder
        If String.IsNullOrEmpty(Me.UtentiComboBox.SelectedValue) Then
            message.AppendLine("E' necessario specificare il delegato!")
        Else


            Dim idDelegato As Integer = CInt(Me.UtentiComboBox.SelectedValue)
            Dim delegato As String = Me.UtentiComboBox.Text 'Me.DelegatoTextBox.Text

            Dim deleghe As New ParsecAdmin.DelegheRepository
            Dim exist As Boolean = False
            If Me.Delegato Is Nothing Then
                exist = Not deleghe.Where(Function(c) c.IdDelegato = idDelegato).FirstOrDefault Is Nothing
            End If
            If exist Then
                message.AppendLine("Il delegato " & delegato & " possiede già dei deleganti!")
            End If
        End If

        If Me.Deleganti.Count = 0 Then
            message.AppendLine("E' necessario specificare almeno un delegante!")
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

            Dim nuovo As Boolean = Me.Delegato Is Nothing

            Me.Delegato = deleghe.CreateFromInstance(Me.Delegato)


            If Not nuovo Then
                Dim filtro As New ParsecAdmin.DelegaFiltro With {.IdDelegato = Me.Delegato.IdDelegato}
                deleghe.DeleteAll(filtro)
            End If


            Me.Delegato.IdDelegato = CInt(Me.UtentiComboBox.SelectedValue)
            Me.Delegato.IdUtenteIns = utenteCollegato.Id


            '************************************************************************************
            'AGGIORNO LA LISTA DEI DELEGANTI
            '************************************************************************************
            Dim disabilitaAccessoScrivaniaCheckBox As CheckBox = Nothing
            Dim id As Integer = 0
            Dim delegante As ParsecAdmin.Delegante = Nothing

            For Each dataItem As GridDataItem In Me.DelegantiGridView.MasterTableView.Items

                id = dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("Id")
                delegante = Me.Deleganti.Where(Function(c) c.Id = id).FirstOrDefault

                disabilitaAccessoScrivaniaCheckBox = CType(dataItem.FindControl("DisabilitaAccessoScrivaniaCheckBox"), CheckBox)
                delegante.DisabilitaAccessoScrivania = disabilitaAccessoScrivaniaCheckBox.Checked
              
            Next
            '************************************************************************************


            Me.Delegato.Deleganti = Me.Deleganti


            deleghe.SaveAll(Me.Delegato)

            '*******************************************************************
            'Aggiorno l'oggetto corrente.
            '*******************************************************************
            Me.Delegato = deleghe.GetDelegati(New ParsecAdmin.DelegaFiltro With {.IdDelegato = Me.Delegato.IdDelegato}).FirstOrDefault
            Me.AggiornaVista(Me.Delegato)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            deleghe.Dispose()
        End Try
    End Sub


#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub TrovaDelegatoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaDelegatoImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaDelegatoImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim parametri As New Hashtable
        parametri("tipoSelezione") = 0 'singola
        Session("Parametri") = parametri
    End Sub

    Protected Sub AggiornaDelegatoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaDelegatoImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim idDelegato As Integer = utentiSelezionati.First.Key
            'Me.DelegatoTextBox.Text = utentiSelezionati.First.Value
            'Me.IdDelegatoTextBox.Text = idDelegato
            Me.UtentiGridView.Rebind()
            Me.DelegantiGridView.Rebind()
            Session.Remove("SelectedUsers")
        End If
    End Sub

    Protected Sub EliminaDelegatoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaDelegatoImageButton.Click
        'Me.DelegatoTextBox.Text = String.Empty
        'Me.IdDelegatoTextBox.Text = String.Empty
        Me.UtentiGridView.Rebind()
        Me.DelegantiGridView.Rebind()
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
            Me.IdDeleganteSelezionati.Add(idUtente)



            Dim delegante As New ParsecAdmin.Delegante
            delegante.Nominativo = item("NominativoHidden").Text
            delegante.ValidaDa = Me.DataValiditaInizioTextBox.SelectedDate
            delegante.ValidaA = Me.DataValiditaFineTextBox.SelectedDate
            delegante.Id = idUtente


            Me.DelegantiSelezionati.Add(delegante)



        Next
        Me.UtentiGridView.Rebind()
        Me.DelegantiGridView.Rebind()
    End Sub

    Protected Sub EliminaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaImageButton.Click
        If Me.DelegantiGridView.SelectedIndexes.Count = 0 Then
            Exit Sub
        End If
        Dim item As GridDataItem = Nothing
        For i As Integer = 0 To Me.DelegantiGridView.SelectedIndexes.Count - 1
            item = CType(Me.DelegantiGridView.Items(Me.DelegantiGridView.SelectedIndexes(i)), GridDataItem)
            Dim idUtente As Integer = CInt(item("Id").Text)
            Me.IdDeleganteSelezionati.Remove(idUtente)
            Dim d = Me.DelegantiSelezionati.Where(Function(c) c.Id = idUtente).FirstOrDefault
            Me.DelegantiSelezionati.Remove(d)

        Next


        Me.UtentiGridView.Rebind()
        Me.DelegantiGridView.Rebind()
    End Sub

    Protected Sub AggiungiTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiTuttoImageButton.Click
        If Me.UtentiGridView.MasterTableView.Items.Count = 0 Then
            Exit Sub
        End If
        For Each Utente In Me.Utenti
            Me.IdDeleganteSelezionati.Add(Utente.Id)

            Dim delegante As New ParsecAdmin.Delegante
            delegante.Nominativo = Utente.Nominativo
            delegante.ValidaDa = Me.DataValiditaInizioTextBox.SelectedDate
            delegante.ValidaA = Me.DataValiditaFineTextBox.SelectedDate
            delegante.Id = Utente.Id


            Me.DelegantiSelezionati.Add(delegante)
        Next
        Me.UtentiGridView.Rebind()
        Me.DelegantiGridView.Rebind()
    End Sub

    Protected Sub EliminaTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaTuttoImageButton.Click
        If Me.DelegantiGridView.MasterTableView.Items.Count = 0 Then
            Exit Sub
        End If
        Me.IdDeleganteSelezionati.Clear()
        Me.DelegantiSelezionati.Clear()

        Me.UtentiGridView.Rebind()
        Me.DelegantiGridView.Rebind()
        Me.DelegantiSelezionati.Clear()
    End Sub

#End Region

End Class