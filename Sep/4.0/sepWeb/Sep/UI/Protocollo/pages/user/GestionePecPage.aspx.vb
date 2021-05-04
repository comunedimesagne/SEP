Imports ParsecAdmin
Imports Telerik.Web.UI

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class GestionePecPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    'Variabile di Sessione: oggetto CasellaPec corrente.
    Public Property CasellaPec() As ParsecAdmin.ParametriPec
        Get
            Return CType(Session("GestionePecPage_CasellaPec"), ParsecAdmin.ParametriPec)
        End Get
        Set(ByVal value As ParsecAdmin.ParametriPec)
            Session("GestionePecPage_CasellaPec") = value
        End Set
    End Property

    'Variabile di Sessione: lista delle Caselle associate alla relativa Griglia.
    Public Property CasellePec() As List(Of ParsecAdmin.ParametriPec)
        Get
            Return CType(Session("GestionePecPage_CasellePec"), List(Of ParsecAdmin.ParametriPec))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.ParametriPec))
            Session("GestionePecPage_CasellePec") = value
        End Set
    End Property

    'Variabile di Sessione: lista degli Utenti associati alla relativa Griglia.
    Public Property Utenti() As List(Of ParsecAdmin.Utente)
        Get
            Return CType(Session("GestionePecPage_Utenti"), List(Of ParsecAdmin.Utente))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Utente))
            Session("GestionePecPage_Utenti") = value
        End Set
    End Property

    'Variabile di Sessione: lista degli Utenti selezionati.
    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("GestionePecPage_SelectedItems") Is Nothing Then
                Session("GestionePecPage_SelectedItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("GestionePecPage_SelectedItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("GestionePecPage_SelectedItems") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Gestione Caselle PEC"
        If Not IsPostBack AndAlso Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
            Me.ResettaVista()
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGrigliaUtenti.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.CaselleEmailGridView.Style.Add("width", widthStyle)
    End Sub

    'Evento LoadComplete della Pagina: dopo che la pagina è stata caricata setta il titolo della Griglia. Inoltre gestisce il messaggio di cancellazione della PEC.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'elemento selezionato?", False, Not Me.CasellaPec Is Nothing)

        ParsecUtility.Utility.ConfirmDelete("Eliminare dalla casella l'utente selezionato?", False, "Casella")
        Me.UtentiLabel.Text = "Utenti&nbsp;&nbsp;&nbsp;" & If(Me.Utenti.Count > 0, "( " & Me.Utenti.Count.ToString & " )", "")
        Me.TitoloElencoCaselleEmailLabel.Text = "Elenco caselle e-mail&nbsp;&nbsp;&nbsp;" & If(Me.CasellePec.Count > 0, "( " & Me.CasellePec.Count.ToString & " )", "")

        'SELEZIONO LA RIGA
        If Not Me.CasellaPec Is Nothing Then
            Dim item As GridDataItem = Me.CaselleEmailGridView.MasterTableView.FindItemByKeyValue("Id", Me.CasellaPec.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If
    End Sub

    'Evento PreRender della Pagina: dopo che la pagina venga renderizzata setta lo scroll scrollPanel
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub


#End Region

#Region "EVENTI TOOLBAR"

    'Evento click della Toolbar. Esegue i vari comandi della Toolbar.
    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim success As Boolean = True
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    Me.CasellePec = Nothing
                    Me.CaselleEmailGridView.Rebind()

                Catch ex As ApplicationException
                    message = ex.Message
                    success = False
                End Try

                If Not success Then
                    ParsecUtility.Utility.MessageBox(message, False)
                Else
                    Me.infoOperazioneHidden.Value = message
                End If


            Case "Nuovo"
                Me.ResettaVista()
                Me.CasellePec = Nothing
                Me.CaselleEmailGridView.Rebind()
                Me.UtentiGridView.Rebind()
            Case "Annulla"
                Me.ResettaVista()
                Me.CasellePec = Nothing
                Me.CaselleEmailGridView.Rebind()
                Me.UtentiGridView.Rebind()
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.CasellaPec Is Nothing Then
                        Me.Delete()
                        Me.CasellePec = Nothing
                        Me.ResettaVista()
                        Me.CaselleEmailGridView.Rebind()
                        Me.UtentiGridView.Rebind()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una casella PEC!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

    'Evento ItemCreated della Toolbar. Aggiunge l'evento onclick sul pulsante Elimina.
    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemCommand associato alla Griglia CaselleEmailGridView. Permette l'esecuzione dei vari comandi attivabili dalla griglia CaselleEmailGridView.
    Protected Sub CaselleEmailGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles CaselleEmailGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

    'Evento ItemDataBound associato alla Griglia CaselleEmailGridView. Setta i tooltip in base alle informazioni contenute nelle celle.
    Protected Sub CaselleEmailGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles CaselleEmailGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona casella PEC"
            End If
        End If
    End Sub

    'Evento NeedDataSource associato alla griglia CaselleEmailGridView. Aggancia il datasource della griglia al DB. Carica la variabile di sessione CasellePec.
    Protected Sub CaselleEmailGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles CaselleEmailGridView.NeedDataSource
        If Me.CasellePec Is Nothing Then
            Dim parametriPec As New ParsecAdmin.ParametriPecRepository
            Me.CasellePec = parametriPec.GetView(Nothing)
            parametriPec.Dispose()
        End If
        Me.CaselleEmailGridView.DataSource = Me.CasellePec
    End Sub

    'Evento ItemCreated associato alla Griglia CaselleEmailGridView. Gestisce  il cambio di pagina.
    Private Sub CaselleEmailGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles CaselleEmailGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    'Metodo print
    Private Sub Print()

    End Sub

    'Cancella una casella dal DB
    Private Sub Delete()
        Dim casellePec As New ParsecAdmin.ParametriPecRepository
        casellePec.Delete(Me.CasellaPec)
        casellePec.Dispose()
    End Sub

    'Effettua la ricerca di una casella PEC
    Private Sub Search()
        Dim parametri As New ParsecAdmin.ParametriPecRepository
        Dim searchTemplate As New ParsecAdmin.ParametriPec With
            {
                .Email = Me.NomeCasellaTextBox.Text
            }

        Me.CasellePec = parametri.GetView(searchTemplate)
        Me.CaselleEmailGridView.Rebind()
        parametri.Dispose()
    End Sub

    'Effettua il salvataggio su DB
    Private Sub Save()
        Dim casellePec As New ParsecAdmin.ParametriPecRepository

        Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.CreateFromInstance(Me.CasellaPec)

        casellaPec.Email = Trim(Me.NomeCasellaTextBox.Text)
        casellaPec.UserId = Trim(Me.UserIdTextBox.Text)

        casellaPec.DimensioneMassima = 0
        If Me.DimensioneMassimaTextBox.Value.HasValue Then
            casellaPec.DimensioneMassima = CInt(Me.DimensioneMassimaTextBox.Value)
        End If


        If Not String.IsNullOrEmpty(Me.PasswordTextBox.Text) Then
            casellaPec.Password = ParsecCommon.CryptoUtil.Encrypt(Trim(Me.PasswordTextBox.Text))
        End If
       

        casellaPec.Pop3Server = Trim(Me.Pop3ServerTextBox.Text)

        Dim porta As Integer = 0


        If Not String.IsNullOrEmpty(Me.Pop3PortaTextBox.Text) Then
            If Int32.TryParse(Trim(Me.Pop3PortaTextBox.Text), porta) Then
                casellaPec.Pop3Porta = porta
            End If
        Else
            'Se non ho specificato la porta del server di posta in entrata assegno -1
            casellaPec.Pop3Porta = -1
        End If


        casellaPec.MantieniCopiaServer = Me.MantieniCopiaSulServerCheckBox.Checked

        casellaPec.SmtpServer = Trim(Me.SmtpServerTextBox.Text)


        If Not String.IsNullOrEmpty(Me.SmtpPortaTextBox.Text) Then
            If Int32.TryParse(Trim(Me.SmtpPortaTextBox.Text), porta) Then
                casellaPec.SmtpPorta = porta
            End If
        Else
            'Se non ho specificato la porta del server di posta in uscita assegno -1
            casellaPec.SmtpPorta = -1
        End If


        casellaPec.Pop3IsSSL = Me.ServerPop3UtilizzaSslCheckBox.Checked
        casellaPec.SmtpIsSSL = Me.ServerSmtpUtilizzaSslCheckBox.Checked
        casellaPec.SmtpAutentication = Me.ServerSmtpRichiedeAutenticazioneCheckBox.Checked

        Try
            '*******************************************************************
            'Gestione storico non utilizzata.
            '*******************************************************************
            casellePec.CasellaPec = Me.CasellaPec

            casellePec.Save(casellaPec, Me.Utenti)

            '*******************************************************************
            'Aggiorno l'oggetto corrente
            '*******************************************************************
            Me.CasellaPec = casellePec.CasellaPec

            'Aggiorno la gridView degli utenti
            Me.Utenti = Me.CasellaPec.Utenti

            Me.UtentiGridView.Rebind()

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            casellePec.Dispose()
        End Try
    End Sub

    'Resetta i campi della maschera e le relative liste
    Private Sub ResettaVista()
        Me.CasellaPec = Nothing

        Me.NomeCasellaTextBox.Text = String.Empty
        Me.UserIdTextBox.Text = String.Empty
        Me.PasswordTextBox.Text = String.Empty

        Me.Pop3ServerTextBox.Text = String.Empty
        Me.Pop3PortaTextBox.Text = String.Empty
        Me.ServerPop3UtilizzaSslCheckBox.Checked = False

        Me.SmtpServerTextBox.Text = String.Empty
        Me.SmtpPortaTextBox.Text = String.Empty
        Me.ServerSmtpUtilizzaSslCheckBox.Checked = False
        Me.ServerSmtpRichiedeAutenticazioneCheckBox.Checked = False
        Me.MantieniCopiaSulServerCheckBox.Checked = False

        Me.Utenti = New List(Of ParsecAdmin.Utente)

        Me.DimensioneMassimaTextBox.Value = 0

        Me.SelectedItems = Nothing
    End Sub

    'Riempie i campi della maschera in base alla Casella selezioanata nella griglia
    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim parametriPec As New ParsecAdmin.ParametriPecRepository
        Dim account As ParsecAdmin.ParametriPec = parametriPec.GetById(id)
        Me.CasellaPec = account


        Me.NomeCasellaTextBox.Text = account.Email
        Me.UserIdTextBox.Text = account.UserId
        Me.PasswordTextBox.Text = account.Password

        Me.DimensioneMassimaTextBox.Value = account.DimensioneMassima


        Me.Pop3ServerTextBox.Text = account.Pop3Server
        Me.Pop3PortaTextBox.Text = account.Pop3Porta
        Me.ServerPop3UtilizzaSslCheckBox.Checked = account.Pop3IsSSL

        Me.SmtpServerTextBox.Text = account.SmtpServer
        Me.SmtpPortaTextBox.Text = account.SmtpPorta
        Me.ServerSmtpUtilizzaSslCheckBox.Checked = account.SmtpIsSSL
        Me.ServerSmtpRichiedeAutenticazioneCheckBox.Checked = account.SmtpAutentication
        If Not account.MantieniCopiaServer Is Nothing Then
            Me.MantieniCopiaSulServerCheckBox.Checked = account.MantieniCopiaServer
        End If

        Me.Utenti = account.Utenti

        parametriPec.Dispose()

        Me.UtentiGridView.Rebind()

    End Sub

#End Region


#Region "GESTIONE UTENTI"

    'Evento che elimina un Utemte dalla relativa griglia
    Protected Sub EliminaUtentiSelezionatiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtentiSelezionatiImageButton.Click
        If Me.UtentiGridView.SelectedItems.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno un elemento!", False)
            Exit Sub
        End If
        For Each item As GridDataItem In Me.UtentiGridView.SelectedItems
            Dim idUtente As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim utente As ParsecAdmin.Utente = Me.Utenti.Where(Function(c) c.Id = idUtente).FirstOrDefault
            Me.Utenti.Remove(utente)
        Next
        Me.UtentiGridView.Rebind()
    End Sub

    'Evento che lanciala ricerca tramite la pagina RicercaUtentePage.aspx.
    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)


        If Not String.IsNullOrEmpty(Me.FiltroUtenteTextBox.Text) Then
            Dim users As New ParsecAdmin.UserRepository

            Dim utenti As List(Of ParsecAdmin.Utente) = users.GetQuery.Where(Function(c) c.LogTipoOperazione Is Nothing).ToList
            utenti = utenti.Where(Function(c) c.Username.ToLower.Contains(Me.FiltroUtenteTextBox.Text.ToLower) Or c.Cognome.ToLower.Contains(Me.FiltroUtenteTextBox.Text.ToLower) Or c.Nome.ToLower.Contains(Me.FiltroUtenteTextBox.Text.ToLower)).ToList
            If utenti.Count = 1 Then

                'Aggiungo gli utenti selezionati nella cache ed escludo i duplicati
                Dim res = (Me.Utenti.Union(utenti).GroupBy(Function(u) u.Id).Select(Function(ut) ut.FirstOrDefault.Id)).ToList

                Dim dataSource = (From s In users.GetQuery.ToList
                   Where res.Contains(s.Id)
                    Select New Utente With
                       {
                           .Id = s.Id,
                           .Descrizione = (If(s.Username = Nothing, "", s.Username) + " - " + If(s.Cognome = Nothing, "", s.Cognome) + " " + If(s.Nome = Nothing, "", s.Nome))
                       }).ToList

                Me.Utenti = dataSource.ToList
                users.Dispose()

            Else
                ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
            End If
        Else
            ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        End If

        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'multipla
        Session("Parametri") = ht
        Me.FiltroUtenteTextBox.Text = String.Empty
    End Sub

    'Associa un utente precedentemente riercato dalla procedura TrovaUtenteImageButton_Click (evento TrovaUtenteImageButton.Click)
    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click

        Dim utentiCasella As List(Of ParsecAdmin.Utente) = Me.Utenti
        Dim utenti As New List(Of ParsecAdmin.Utente)

        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")

            Dim users As New ParsecAdmin.UserRepository
            For Each utente In utentiSelezionati
                utenti.Add(users.GetUserById(utente.Key).FirstOrDefault)
            Next

            'Aggiungo gli utenti selezionati nella cache ed escludo i duplicati
            Dim res = (utentiCasella.Union(utenti).GroupBy(Function(u) u.Id).Select(Function(ut) ut.FirstOrDefault.Id)).ToList

            Dim dataSource = (From s In users.GetQuery.ToList
                    Where res.Contains(s.Id)
                     Select New Utente With
                        {
                            .Id = s.Id,
                            .Descrizione = (If(s.Username = Nothing, "", s.Username) + " - " + If(s.Cognome = Nothing, "", s.Cognome) + " " + If(s.Nome = Nothing, "", s.Nome))
                        }).ToList

            Me.Utenti = dataSource
            users.Dispose()

        End If
        Session("SelectedUsers") = Nothing

        Me.UtentiGridView.Rebind()
    End Sub

    'Evento ItemCommand associato alla Griglia UtentiGridView. Permette l'esecuzione dei vari comandi attivabili dalla griglia UtentiGridView (in questo caso il Delete).
    Protected Sub UtentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UtentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Me.EliminaUtente(e)
        End Select
    End Sub

    'Evento ItemCreated associato alla Griglia UtentiGridView. Definisce l' ItemPreRender e definisce lo stile per gli Item di tipo GridHeaderItem.
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

    'Evento ItemPreRender associato alla Griglia UtentiGridView. Gestisce il Selected dei checkbox.
    Protected Sub UtentiGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    'Evento PreRender associato alla Griglia UtentiGridView. Gestisce il Selected dei checkbox.
    Protected Sub UtentiGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles UtentiGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.UtentiGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.UtentiGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.UtentiGridView.SelectedItems.Count = Me.UtentiGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.UtentiGridView.Items.Count > 0
    End Sub

    'Elimina un Utente dalla Griglia UtentiGridView
    Private Sub EliminaUtente(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim idUtente As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")

        'Recupero la cache degli utenti associati alla casella corrente.
        Dim utentiCasella As List(Of ParsecAdmin.Utente) = Me.Utenti

        'Elimino l'utente selezionato dalla cache
        Dim utente As ParsecAdmin.Utente = utentiCasella.Where(Function(c) c.Id = idUtente).FirstOrDefault

        utentiCasella.Remove(utente)

        Me.Utenti = utentiCasella
        Me.UtentiGridView.Rebind()

    End Sub

    'Evento NeedDataSource associato alla griglia UtentiGridView. Aggancia il datasource della griglia alla lista  Utenti (variabile di sessione).
    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource
        Me.UtentiGridView.DataSource = Me.Utenti
    End Sub

    'Evento ItemDataBound associato alla Griglia UtentiGridView. Setta i tooltip e gestisce alcuni controlli della griglia in base alle informazioni contenute nelle celle.
    Protected Sub UtentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim utente As ParsecAdmin.Utente = CType(e.Item.DataItem, ParsecAdmin.Utente)
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina utente"
                btn.Attributes.Add("onclick", "return ConfirmDeleteCasella();")
            End If
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim id As String = Utente.Id
            If Me.SelectedItems.ContainsKey(id) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(Me.SelectedItems(id).ToString())
                dataItem.Selected = True
            End If
        End If
    End Sub

    'Evento associato alla griglia UtentiGridView per gestire i checkbox
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

    'Evento associato alla griglia UtentiGridView per gestire i checkbox
    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedItems()
    End Sub

    'Salva nella variabile di Sessione SelectedItems gli id degli Utenti selezionati nella griglia UtentiGridView.
    Private Sub SaveSelectedItems()
        For Each item As GridItem In Me.UtentiGridView.Items
            If TypeOf item Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(item, GridDataItem)
                Dim id As Integer = CInt(dataItem("Id").Text)
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

#End Region

End Class