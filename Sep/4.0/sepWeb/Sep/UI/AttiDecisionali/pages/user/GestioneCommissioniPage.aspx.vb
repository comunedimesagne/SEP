Imports ParsecAtt
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneCommissioniPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Public Property Commissione() As ParsecAtt.Commissione
        Get
            Return CType(Session("GestioneCommissioniPage_Commissione"), ParsecAtt.Commissione)
        End Get
        Set(ByVal value As ParsecAtt.Commissione)
            Session("GestioneCommissioniPage_Commissione") = value
        End Set
    End Property

    Public Property Commissioni() As List(Of ParsecAtt.Commissione)
        Get
            Return CType(Session("GestioneCommissioniPage_Commissioni"), List(Of ParsecAtt.Commissione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Commissione))
            Session("GestioneCommissioniPage_Commissioni") = value
        End Set
    End Property

    Public Property Consiglieri() As List(Of ParsecAtt.ConsigliereCommissione)
        Get
            Return CType(Session("GestioneCommissioniPage_Consiglieri"), List(Of ParsecAtt.ConsigliereCommissione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.ConsigliereCommissione))
            Session("GestioneCommissioniPage_Consiglieri") = value
        End Set
    End Property

    Public Property Utenti() As List(Of ParsecAtt.UtenteCommissione)
        Get
            Return CType(Session("GestioneCommissioniPage_Utenti"), List(Of ParsecAtt.UtenteCommissione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.UtenteCommissione))
            Session("GestioneCommissioniPage_Utenti") = value
        End Set
    End Property

    Public Property SelectedConsiglieriItems As Dictionary(Of String, Boolean)
        Get
            If Session("GestioneCommissioniPage_SelectedConsiglieriItems") Is Nothing Then
                Session("GestioneCommissioniPage_SelectedConsiglieriItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("GestioneCommissioniPage_SelectedConsiglieriItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("GestioneCommissioniPage_SelectedConsiglieriItems") = value
        End Set
    End Property

    Public Property SelectedUtentiItems As Dictionary(Of String, Boolean)
        Get
            If Session("GestioneCommissioniPage_SelectedUtentiItems") Is Nothing Then
                Session("GestioneCommissioniPage_SelectedUtentiItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("GestioneCommissioniPage_SelectedUtentiItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("GestioneCommissioniPage_SelectedUtentiItems") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Me.Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BasePage.master"
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Me.Page.Request("Mode") Is Nothing Then
            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Atti Decisionali"
            Me.MainPage.DescrizioneProcedura = "> Gestione Commissioni"
        Else
            Me.MainPage = CType(Me.Master, PopupPage)
        End If
        If Not Me.Page.IsPostBack Then
            Me.Commissioni = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.CommissioniGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.ConsiglieriGridView.Style.Add("width", widthStyle)
        Me.CommissioniGridView.Style.Add("width", widthStyle)

    
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la commissione selezionata?", False, Not Me.Commissione Is Nothing)
        If Not Me.Commissioni Is Nothing Then
            Me.TitoloElencoCommissioniLabel.Text = "Elenco Commissioni&nbsp;&nbsp;&nbsp;" & If(Me.Commissioni.Count > 0, "( " & Me.Commissioni.Count.ToString & " )", "")
        End If
        Me.TitoloElencoConsiglieriLabel.Text = "Elenco Consiglieri&nbsp;&nbsp;&nbsp;" & If(Me.Consiglieri.Count > 0, "( " & Me.Consiglieri.Count.ToString & " )", "")
        Me.TitoloElencoUtentiLabel.Text = "Elenco Utenti&nbsp;&nbsp;&nbsp;" & If(Me.Utenti.Count > 0, "( " & Me.Utenti.Count.ToString & " )", "")


        If Me.CommissioniGridView.SelectedItems.Count > 0 Then
            Dim message As String = "Eliminare tutti gli elementi selezionati?"
            Me.EliminaConsiglieriSelezionatiImageButton.Attributes.Add("onclick", "return confirm(""" & message & """)")
        Else
            Dim message As String = "E' necessario selezionare almeno un consigliere!"
            Me.EliminaConsiglieriSelezionatiImageButton.Attributes.Add("onclick", "alert(""" & message & """); return false;")
        End If

        If Me.UtentiGridView.SelectedItems.Count > 0 Then
            Dim message As String = "Eliminare tutti gli elementi selezionati?"
            Me.EliminaUtentiSelezionatiImageButton.Attributes.Add("onclick", "return confirm(""" & message & """)")
        Else
            Dim message As String = "E' necessario selezionare almeno un utente!"
            Me.EliminaUtentiSelezionatiImageButton.Attributes.Add("onclick", "alert(""" & message & """); return false;")
        End If

        'SELEZIONO LA RIGA
        If Not Me.Commissione Is Nothing Then
            Dim item As GridDataItem = Me.CommissioniGridView.MasterTableView.FindItemByKeyValue("Id", Me.Commissione.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollConsiglieriPanel, Me.scrollPosConsiglieriHidden, False)
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollUtentiPanel, Me.scrollPosUtentiHidden, False)
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

            Case "Annulla"
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Commissione Is Nothing Then
                        Dim message As String = String.Empty
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
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una commissione!", False)
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

    Protected Sub CommissioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles CommissioniGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.ResettaVista()
                Me.AggiornaVista(e.Item)
        End Select
    End Sub


    Protected Sub CommissioniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles CommissioniGridView.NeedDataSource
        If Me.Commissioni Is Nothing Then
            Dim commissioni As New ParsecAtt.CommissioniRepository
            Me.Commissioni = commissioni.GetView(Nothing)
            commissioni.Dispose()
        End If
        Me.CommissioniGridView.DataSource = Me.Commissioni
    End Sub

    Protected Sub CommissioniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles CommissioniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "GESTIONE PRESIDENTE E VICE"

    Protected Sub TrovaPresidenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaPresidenteImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/GestioneConsiglieriPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaPresidenteImageButton.ClientID)
        queryString.Add("Mode", "SelezioneSingola")
        ParsecUtility.Utility.ShowPopup(pageUrl, 720, 685, queryString, False)
    End Sub

    Protected Sub AggiornaPresidenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaPresidenteImageButton.Click
        If Not ParsecUtility.SessionManager.Consigliere Is Nothing Then
            Dim consigliere As ParsecAtt.Consigliere = ParsecUtility.SessionManager.Consigliere
            Me.PresidenteTextBox.Text = consigliere.Nominativo
            Me.IdPresidenteTextBox.Text = consigliere.Id.ToString
            ParsecUtility.SessionManager.Consigliere = Nothing
        End If
    End Sub


    Protected Sub EliminaPresidenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaPresidenteImageButton.Click
        Me.PresidenteTextBox.Text = String.Empty
        Me.IdPresidenteTextBox.Text = String.Empty
    End Sub

    Protected Sub TrovaViceImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaViceImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/GestioneConsiglieriPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaViceImageButton.ClientID)
        queryString.Add("Mode", "SelezioneSingola")
        ParsecUtility.Utility.ShowPopup(pageUrl, 720, 685, queryString, False)
    End Sub

    Protected Sub AggiornaViceImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaViceImageButton.Click
        If Not ParsecUtility.SessionManager.Consigliere Is Nothing Then
            Dim consigliere As ParsecAtt.Consigliere = ParsecUtility.SessionManager.Consigliere
            Me.ViceTextBox.Text = consigliere.Nominativo
            Me.IdViceTextBox.Text = consigliere.Id.ToString
            ParsecUtility.SessionManager.Consigliere = Nothing
        End If
    End Sub

    Protected Sub EliminaViceImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaViceImageButton.Click
        Me.ViceTextBox.Text = String.Empty
        Me.IdViceTextBox.Text = String.Empty
    End Sub


#End Region

#Region "GESTIONE CONSIGLIERI E UTENTI COLLEGATI"


    Protected Sub TrovaConsigliereImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaConsigliereImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/GestioneConsiglieriPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaConsigliereImageButton.ClientID)
        queryString.Add("Mode", "SelezioneMultipla")
        ParsecUtility.Utility.ShowPopup(pageUrl, 720, 685, queryString, False)
    End Sub

    Protected Sub EliminaConsiglieriSelezionatiImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaConsiglieriSelezionatiImageButton.Click
        For Each item As GridDataItem In Me.ConsiglieriGridView.SelectedItems
            Dim idConsigliere As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdConsigliere")
            Dim consigliere As ParsecAtt.ConsigliereCommissione = Me.Consiglieri.Where(Function(c) c.IdConsigliere = idConsigliere).FirstOrDefault
            Me.Consiglieri.Remove(consigliere)
        Next
        Me.ConsiglieriGridView.Rebind()
    End Sub

    Protected Sub AggiornaConsigliereImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaConsigliereImageButton.Click
        If Not ParsecUtility.SessionManager.Consigliere Is Nothing Then
            Dim consiglieri As List(Of ParsecAtt.Consigliere) = ParsecUtility.SessionManager.Consigliere

            Dim consiglieriSelezionati = (From c In consiglieri
                       Select New ParsecAtt.ConsigliereCommissione With {
                           .IdConsigliere = c.Id,
                           .Nominativo = c.Nominativo
                       }).ToList


            Me.Consiglieri = consiglieriSelezionati.Union(Me.Consiglieri).GroupBy(Function(c) c.IdConsigliere).Select(Function(c) c.First).ToList
            ParsecUtility.SessionManager.Consigliere = Nothing

            Me.ConsiglieriGridView.Rebind()

        End If
    End Sub



    Protected Sub TrovaUtenteImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'multipla
        Session("Parametri") = ht
    End Sub

    Protected Sub EliminaUtentiSelezionatiImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtentiSelezionatiImageButton.Click
        For Each item As GridDataItem In Me.UtentiGridView.SelectedItems
            Dim IdUtente As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdUtente")
            Dim utente As ParsecAtt.UtenteCommissione = Me.Utenti.Where(Function(c) c.IdUtente = IdUtente).FirstOrDefault
            Me.Utenti.Remove(utente)
        Next
        Me.UtentiGridView.Rebind()
    End Sub

    Protected Sub AggiornaUtenteImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utenti As SortedList(Of Integer, String) = Session("SelectedUsers")
            Session.Remove("SelectedUsers")

            Dim utentiSelezionati = (From c In utenti
                      Select New ParsecAtt.UtenteCommissione With {
                          .IdUtente = c.Key,
                          .Nominativo = c.Value
                      }).ToList


            Me.Utenti = utentiSelezionati.Union(Me.Utenti).GroupBy(Function(c) c.IdUtente).Select(Function(c) c.First).ToList
            Me.UtentiGridView.Rebind()
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SaveSelectedConsiglieriItems()
        For Each item As GridItem In Me.ConsiglieriGridView.Items
            If TypeOf item Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(item, GridDataItem)
                Dim id As Integer = CInt(dataItem("IdConsigliere").Text)
                Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        If Not Me.SelectedConsiglieriItems.ContainsKey(id) Then
                            Me.SelectedConsiglieriItems.Add(id, True)
                        End If
                    Else
                        If Me.SelectedConsiglieriItems.ContainsKey(id) Then
                            Me.SelectedConsiglieriItems.Remove(id)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub SaveSelectedUtentiItems()
        For Each item As GridItem In Me.UtentiGridView.Items
            If TypeOf item Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(item, GridDataItem)
                Dim id As Integer = CInt(dataItem("IdUtente").Text)
                Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        If Not Me.SelectedUtentiItems.ContainsKey(id) Then
                            Me.SelectedUtentiItems.Add(id, True)
                        End If
                    Else
                        If Me.SelectedUtentiItems.ContainsKey(id) Then
                            Me.SelectedUtentiItems.Remove(id)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub AggiornaGriglia()
        Me.Commissioni = Nothing
        Me.CommissioniGridView.Rebind()
    End Sub


    Private Sub AggiornaGrigliaUtenti()
        Me.UtentiGridView.DataSource = Me.Utenti
        Me.UtentiGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaConsiglieri()
        Me.ConsiglieriGridView.DataSource = Me.Consiglieri
        Me.ConsiglieriGridView.DataBind()
    End Sub

    Private Sub Search()
        Dim commissioni As New ParsecAtt.CommissioniRepository
        Dim filtro As ParsecAtt.FiltroCommissione = Me.GetFiltro
        Me.Commissioni = commissioni.GetView(filtro)
        commissioni.Dispose()
        Me.CommissioniGridView.Rebind()
    End Sub


    Private Sub ResettaVista()
        Me.DescrizioneTextBox.Text = String.Empty
        Me.PresidenteTextBox.Text = String.Empty
        Me.IdPresidenteTextBox.Text = String.Empty
        Me.ViceTextBox.Text = String.Empty
        Me.IdViceTextBox.Text = String.Empty
        Me.Commissione = Nothing
        Me.Consiglieri = New List(Of ParsecAtt.ConsigliereCommissione)
        Me.Utenti = New List(Of ParsecAtt.UtenteCommissione)

        Me.AggiornaGrigliaUtenti()
        Me.AggiornaGrigliaConsiglieri()

        'Me.UtentiGridView.Rebind()
        'Me.CommissioniGridView.Rebind()

        Me.SelectedConsiglieriItems = Nothing
        Me.SelectedUtentiItems = Nothing

    End Sub


    Private Sub AggiornaVista(commissione As ParsecAtt.Commissione)
        Me.DescrizioneTextBox.Text = commissione.Descrizione


        If commissione.IdPresidente.HasValue Then
            Me.IdPresidenteTextBox.Text = commissione.IdPresidente
        End If
        Me.PresidenteTextBox.Text = commissione.Presidente


        If commissione.IdVicePresidente.HasValue Then
            Me.IdViceTextBox.Text = commissione.IdVicePresidente
        End If
        Me.ViceTextBox.Text = commissione.VicePresidente
        Me.Commissione = commissione
        Me.Consiglieri = commissione.Consiglieri
        Me.Utenti = commissione.Utenti


        Me.ConsiglieriGridView.DataSource = Me.Consiglieri
        Me.UtentiGridView.DataSource = Me.Utenti

        Me.UtentiGridView.Rebind()
        Me.ConsiglieriGridView.Rebind()

    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim commissioni As New ParsecAtt.CommissioniRepository
        Dim commissione As ParsecAtt.Commissione = commissioni.GetById(id)
        commissioni.Dispose()
        Me.AggiornaVista(commissione)
    End Sub

    Private Sub Delete()
        Dim commissioni As New ParsecAtt.CommissioniRepository
        Try
            'Eseguo la cancellazione logica della commissione corrente.
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim commissione As ParsecAtt.Commissione = commissioni.GetQuery.Where(Function(c) c.Id = Me.Commissione.Id).FirstOrDefault
            If Not commissione Is Nothing Then
                commissione.DataOperazione = Now
                commissione.Stato = "A"
                commissione.IdUtente = utenteCollegato.Id
                commissioni.SaveChanges()
            End If

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            commissioni.Dispose()
        End Try
    End Sub


    Private Function AggiornaModello(ByVal commissione As ParsecAtt.Commissione) As ParsecAtt.Commissione
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)


        commissione.Utenti = Me.Utenti
        commissione.Consiglieri = Me.Consiglieri

        commissione.Descrizione = Me.DescrizioneTextBox.Text.Trim

        If Not String.IsNullOrEmpty(Me.IdPresidenteTextBox.Text) Then
            commissione.IdPresidente = CInt(Me.IdPresidenteTextBox.Text)
            commissione.Presidente = Me.PresidenteTextBox.Text.Trim
        End If
        If Not String.IsNullOrEmpty(Me.IdViceTextBox.Text) Then
            commissione.IdVicePresidente = CInt(Me.IdViceTextBox.Text)
            commissione.VicePresidente = Me.ViceTextBox.Text
        End If
        commissione.IdUtente = utenteCollegato.Id
        commissione.DataOperazione = Now

        Return commissione
    End Function


    Private Sub Save()

        Dim commissioni As New ParsecAtt.CommissioniRepository
        Dim commissione As ParsecAtt.Commissione = commissioni.CreateFromInstance(Me.Commissione)

        'Aggiorno il modello.
        commissione = Me.AggiornaModello(commissione)


        Try
            commissioni.Save(commissione)
            commissione = commissioni.GetById(commissioni.Commissione.Id)
            Me.ResettaVista()
            Me.AggiornaVista(commissione)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            commissioni.Dispose()
        End Try
    End Sub



    Private Function GetFiltro() As ParsecAtt.FiltroCommissione
        Dim filtro As New ParsecAtt.FiltroCommissione
        If Not String.IsNullOrEmpty(Me.DescrizioneTextBox.Text) Then
            filtro.Descrizione = Me.DescrizioneTextBox.Text
        End If
        Return filtro
    End Function

    Private Sub EliminaUtente(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idUtente As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdUtente")
        Dim utente As ParsecAtt.UtenteCommissione = Me.Utenti.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault
        Me.Utenti.Remove(utente)
        Me.UtentiGridView.Rebind()
    End Sub

    Private Sub EliminaConsigliere(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idConsigliere As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdConsigliere")
        Dim consigliere As ParsecAtt.ConsigliereCommissione = Me.Consiglieri.Where(Function(c) c.IdConsigliere = idConsigliere).FirstOrDefault
        Me.Consiglieri.Remove(consigliere)
        Me.ConsiglieriGridView.Rebind()
    End Sub


#End Region

#Region "GESTIONE GRIGLIA CONSIGLIERI"

    Protected Sub ConsiglieriGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ConsiglieriGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaConsigliere(e.Item)
        End If
    End Sub

    Protected Sub ConsiglieriGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ConsiglieriGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim consigliere As ParsecAtt.ConsigliereCommissione = CType(e.Item.DataItem, ParsecAtt.ConsigliereCommissione)
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina consigliere"
                Dim message As String = "Eliminare l'elemento selezionato?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim idConsigliere As String = consigliere.IdConsigliere
            If Me.SelectedConsiglieriItems.ContainsKey(idConsigliere) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(Me.SelectedConsiglieriItems(idConsigliere).ToString())
                dataItem.Selected = True
            End If
        End If
    End Sub

    Protected Sub ConsiglieriGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ConsiglieriGridView.NeedDataSource
        Me.ConsiglieriGridView.DataSource = Me.Consiglieri
    End Sub

    Protected Sub ConsiglieriToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedConsiglieriItems()
    End Sub

    Protected Sub ConsiglieriToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.ConsiglieriGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

    Private Sub ConsiglieriGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ConsiglieriGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf ConsiglieriGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub ConsiglieriGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub ConsiglieriGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles ConsiglieriGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.ConsiglieriGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.ConsiglieriGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.ConsiglieriGridView.SelectedItems.Count = Me.ConsiglieriGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.ConsiglieriGridView.Items.Count > 0
    End Sub

#End Region

#Region "GESTIONE GRIGLIA UTENTI"

    Protected Sub UtentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim utente As ParsecAtt.UtenteCommissione = CType(e.Item.DataItem, ParsecAtt.UtenteCommissione)
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina utente"
                Dim message As String = "Eliminare l'elemento selezionato?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If

            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim idUtente As String = utente.IdUtente
            If Me.SelectedUtentiItems.ContainsKey(idUtente) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(Me.SelectedUtentiItems(idUtente).ToString())
                dataItem.Selected = True
            End If

        End If
    End Sub

    Protected Sub UtentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UtentiGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaUtente(e.Item)
        End If
    End Sub

    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource
        Me.UtentiGridView.DataSource = Me.Utenti
    End Sub

    Protected Sub UtentiToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedUtentiItems()
    End Sub

    Protected Sub UtentiToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.UtentiGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

    Private Sub UtentiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf UtentiGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
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


#End Region


End Class