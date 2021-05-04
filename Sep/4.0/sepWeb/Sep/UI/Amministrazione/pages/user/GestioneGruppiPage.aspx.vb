Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneGruppiPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage


#Region "PROPRIETA'"

    Public Property Gruppo() As ParsecAdmin.Gruppo
        Get
            Return CType(Session("GestioneGruppiPage_Gruppo"), ParsecAdmin.Gruppo)
        End Get
        Set(ByVal value As ParsecAdmin.Gruppo)
            Session("GestioneGruppiPage_Gruppo") = value
        End Set
    End Property

    Public Property Gruppi() As List(Of ParsecAdmin.Gruppo)
        Get
            Return CType(Session("GestioneGruppiPage_Gruppi"), List(Of ParsecAdmin.Gruppo))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Gruppo))
            Session("GestioneGruppiPage_Gruppi") = value
        End Set
    End Property

    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("GestioneGruppiPage_SelectedItems") Is Nothing Then
                Session("GestioneGruppiPage_SelectedItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("GestioneGruppiPage_SelectedItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("GestioneGruppiPage_SelectedItems") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Gestione Gruppi"
        If Not Me.Page.IsPostBack Then
            Me.Gruppi = Nothing
            Me.SelectedItems = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.GruppiGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

        '***********************************************************************************************************
        'FIX PER I CONTROLLI CHECKBOX E RADIOBUTTON IN CHROME QUANDO VIENE APPLICATO LO SKIN DELLA TELERIK
        '***********************************************************************************************************
        'Dim css As New HtmlGenericControl
        'css.TagName = "style"
        'css.Attributes.Add("type", "text/css")


        'css.InnerHtml = ".rfdCheckbox input[type='checkbox']"
        'css.InnerHtml &= "{-webkit-appearance: none;}"


        'css.InnerHtml &= ".rfdRadio input[type='radio]"
        'css.InnerHtml &= "{-webkit-appearance: radio;}"


        'Me.Page.Header.Controls.Add(css)
        '***********************************************************************************************************

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGrigliaUtenti.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.GruppiGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il gruppo selezionato?", False, Me.Gruppo.Id <> 0)
        If Not Me.Gruppi Is Nothing Then
            Me.TitoloElencoGruppiLabel.Text = "Elenco Gruppi&nbsp;&nbsp;&nbsp;" & If(Me.Gruppi.Count > 0, "( " & Me.Gruppi.Count.ToString & " )", "")
        End If
        Me.TitoloElencoUtentiLabel.Text = "Elenco Utenti&nbsp;&nbsp;&nbsp;" & If(Me.Gruppo.Utenti.Count > 0, "( " & Me.Gruppo.Utenti.Count.ToString & " )", "")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub


#End Region

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                ' Me.Print()
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
                Me.UtentiGridView.Rebind()

            Case "Annulla"
                Me.ResettaVista()
                Me.AggiornaGriglia()
                Me.UtentiGridView.Rebind()
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Gruppo Is Nothing Then
                        Dim message As String = String.Empty
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.AggiornaGriglia()
                            Me.UtentiGridView.Rebind()
                        Catch ex As Exception
                            message = ex.Message
                        End Try
                        If String.IsNullOrEmpty(message) Then
                            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                        Else
                            ParsecUtility.Utility.MessageBox(message, False)
                        End If
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un gruppo!", False)
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

#Region "EVENTI GRIGLIA"

    Protected Sub GruppiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles GruppiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
        End Select
    End Sub

    Protected Sub GruppiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles GruppiGridView.NeedDataSource
        If Me.Gruppi Is Nothing Then
            Dim gruppi As New ParsecAdmin.GruppoRepository
            Me.Gruppi = gruppi.GetView(Nothing)
            gruppi.Dispose()
        End If
        Me.GruppiGridView.DataSource = Me.Gruppi
    End Sub

    Protected Sub GruppiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles GruppiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In UtentiGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedItems()
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub AggiornaGriglia()
        Me.Gruppi = Nothing
        Me.GruppiGridView.Rebind()
    End Sub

    Private Sub SaveSelectedItems()
        For Each item As GridItem In Me.UtentiGridView.Items
            If TypeOf item Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(item, GridDataItem)
                Dim id As Integer = CInt(dataItem("IdUtente").Text)
                Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        If Not Me.SelectedItems.ContainsKey(id) Then
                            Me.SelectedItems.Add(id, True)
                        End If
                    Else
                        If Me.SelectedItems.ContainsKey(id) Then
                            Me.SelectedItems.Remove(id)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub Search()
        Dim gruppi As New ParsecAdmin.GruppoRepository
        Dim filtro As ParsecAdmin.FiltroGruppo = Me.GetFiltro
        Me.Gruppi = gruppi.GetView(filtro)
        gruppi.Dispose()
        Me.GruppiGridView.Rebind()
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim gruppi As New ParsecAdmin.GruppoRepository
        Dim gruppo As ParsecAdmin.Gruppo = gruppi.GetById(id)
        gruppi.Dispose()
        Me.AggiornaVista(gruppo)
    End Sub

    Private Sub AggiornaVista(ByVal gruppo As ParsecAdmin.Gruppo)
        Me.ResettaVista()
        Me.DescrizioneTextBox.Text = gruppo.Descrizione
        Me.AbilitatoCheckBox.Checked = gruppo.Abilitato
        Me.DataValiditaInizioTextBox.SelectedDate = gruppo.DataInizioValidita
        Me.DataValiditaFineTextBox.SelectedDate = gruppo.DataFineValidita
        Me.Gruppo = gruppo
        Me.UtentiGridView.Rebind()
    End Sub

    Private Sub Delete()
        'Niente
    End Sub

    Private Sub AggiornaModello(gruppo As ParsecAdmin.Gruppo)
        If Not String.IsNullOrEmpty(Me.DescrizioneTextBox.Text) Then
            gruppo.Descrizione = Me.DescrizioneTextBox.Text
        End If
        gruppo.Abilitato = Me.AbilitatoCheckBox.Checked
        gruppo.DataInizioValidita = Me.DataValiditaInizioTextBox.SelectedDate
        gruppo.DataFineValidita = Me.DataValiditaFineTextBox.SelectedDate
        gruppo.Utenti = Me.Gruppo.Utenti
    End Sub

    Private Sub Save()

        Dim gruppi As New ParsecAdmin.GruppoRepository
        Dim gruppo As ParsecAdmin.Gruppo = Nothing

        If Me.Gruppo.Id = 0 Then
            gruppo = gruppi.CreateFromInstance(Nothing)
        Else
            gruppo = gruppi.CreateFromInstance(Me.Gruppo)
        End If

        'Aggiorno il modello.
        Me.AggiornaModello(gruppo)

        Try

            gruppi.Save(gruppo)
            Me.ResettaVista()

            Me.Gruppo = gruppi.GetById(gruppo.Id)
            Me.AggiornaVista(Me.Gruppo)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            gruppi.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.DescrizioneTextBox.Text = String.Empty
        Me.AbilitatoCheckBox.Checked = True
        Me.DataValiditaInizioTextBox.SelectedDate = Now
        Me.DataValiditaFineTextBox.SelectedDate = Nothing
        Me.Gruppo = New ParsecAdmin.Gruppo
        Me.Gruppo.Utenti = New List(Of ParsecAdmin.GruppoUtente)
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FiltroGruppo
        Dim filtro As New ParsecAdmin.FiltroGruppo
        filtro.Descrizione = Me.DescrizioneTextBox.Text.Trim
        Return filtro
    End Function

#End Region



#Region "GESTIONE UTENTI"

    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim utente As ParsecAdmin.GruppoUtente = Nothing
            Dim idUtente As Integer = 0
            For Each utenteSelezionato In utentiSelezionati
                idUtente = utenteSelezionato.Key
                Dim esiste As Boolean = Not Me.Gruppo.Utenti.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault Is Nothing
                If Not esiste Then
                    utente = New ParsecAdmin.GruppoUtente
                    utente.IdUtente = idUtente
                    utente.DescrizioneUtente = utenteSelezionato.Value
                    Me.Gruppo.Utenti.Add(utente)
                End If
            Next
            Me.UtentiGridView.Rebind()
            Session("SelectedUsers") = Nothing
        End If
    End Sub

    Protected Sub UtentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UtentiGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaUtente(e.Item)
        End If
    End Sub

    Private Sub EliminaUtente(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim IdUtente As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdUtente")
        Dim utente As ParsecAdmin.GruppoUtente = Me.Gruppo.Utenti.Where(Function(c) c.IdUtente = IdUtente).FirstOrDefault
        If Not utente Is Nothing Then
            Me.Gruppo.Utenti.Remove(utente)
        End If
        Me.UtentiGridView.Rebind()
    End Sub

    Protected Sub UtentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim utente As ParsecAdmin.GruppoUtente = CType(e.Item.DataItem, ParsecAdmin.GruppoUtente)
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina utente"
                Dim message As String = "Eliminare l'elemento selezionato?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim id As String = utente.IdUtente
            If Me.SelectedItems.ContainsKey(id) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(Me.SelectedItems(id).ToString())
                dataItem.Selected = True
            End If
        End If
    End Sub

    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource
        Me.UtentiGridView.DataSource = Me.Gruppo.Utenti
    End Sub

    Private Sub UtentiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf UtentiGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If

    End Sub

    Protected Sub UtentiGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub UtentiGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles UtentiGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.UtentiGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.UtentiGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.UtentiGridView.SelectedItems.Count = Me.UtentiGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.UtentiGridView.Items.Count > 0
    End Sub

    Protected Sub EliminaUtentiSelezionatiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtentiSelezionatiImageButton.Click

        If Me.UtentiGridView.SelectedItems.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno un elemento!", False)
            Exit Sub
        End If

        For Each item As GridDataItem In Me.UtentiGridView.SelectedItems
            Dim idUtente As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdUtente")
            Dim utente As ParsecAdmin.GruppoUtente = Me.Gruppo.Utenti.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault
            Me.Gruppo.Utenti.Remove(utente)
        Next
        Me.UtentiGridView.Rebind()
    End Sub

#End Region

End Class