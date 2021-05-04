Imports ParsecPro
Imports Telerik.Web.UI

'* SPDX-License-Identifier: GPL-3.0-only

'Permette la ricerca degli Oggetti da poter selezionare ed utilizzare nelle registrazioni di Protocollo
Partial Class RicercaOggettoPage
    Inherits System.Web.UI.Page


    Public Property Filtro As String = String.Empty

    'Variabile di Sessione: Oggetto di Protocollo corrente
    Public Property Oggetto() As ParsecPro.Oggetto
        Get
            Return CType(Session("RicercaOggettoPage_Oggetto"), ParsecPro.Oggetto)
        End Get
        Set(ByVal value As ParsecPro.Oggetto)
            Session("RicercaOggettoPage_Oggetto") = value
        End Set
    End Property

    'Variabile di Sessione: Elenco degli Oggetti associati alla griglia
    Public Property Oggetti() As List(Of ParsecPro.Oggetto)
        Get
            Return CType(Session("RicercaOggettoPage_Oggetti"), List(Of ParsecPro.Oggetto))
        End Get
        Set(ByVal value As List(Of ParsecPro.Oggetto))
            Session("RicercaOggettoPage_Oggetti") = value
        End Set
    End Property

    'Variabile di Sessione: Elenco delle Strutture dell'Organigramma asscoiate alla griglia
    Public Property Strutture() As List(Of ParsecPro.StrutturaOggetto)
        Get
            Return CType(Session("RicercaOggettoPage_Strutture"), List(Of ParsecPro.StrutturaOggetto))
        End Get
        Set(ByVal value As List(Of ParsecPro.StrutturaOggetto))
            Session("RicercaOggettoPage_Strutture") = value
        End Set
    End Property

    'Evento Load: inzializza la pagina
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Me.GetParametri()
            Me.OggettoTextBox.Text = Me.Filtro
            Me.Find()
        End If
    End Sub

    'Evento LoadComplete: carica le s.trutture e setta i titoli delle griglie.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Not IsPostBack AndAlso Not UI.ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
            Me.Strutture = New List(Of ParsecPro.StrutturaOggetto)
            Me.StruttureGridView.DataSource = Me.Strutture
            Me.StruttureGridView.DataBind()
        Else
            Me.StruttureGridView.DataSource = Me.Strutture
            Me.StruttureGridView.DataBind()
        End If
        Me.StrutturaLabel.Text = "Settore/Ufficio&nbsp;&nbsp;&nbsp;" & If(Me.StruttureGridView.MasterTableView.Items.Count > 0, "( " & Me.StruttureGridView.MasterTableView.Items.Count.ToString & " )", "")
        Me.TitoloLabel.Text = "Elenco Oggetti&nbsp;&nbsp;&nbsp;" & If(Me.Oggetti.Count > 0, "( " & Me.Oggetti.Count.ToString & " )", "")
    End Sub

    'Evento Click della Toolbar: permette di eseguire il Salvataggio e la Ricerca
    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
          Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    Me.Oggetti = Nothing
                    Me.OggettiGridView.Rebind()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)
            Case "Nuovo"
                Me.ResettaVista()
                Me.Oggetti = Nothing
                Me.OggettiGridView.Rebind()
            Case "Annulla"
                Me.ResettaVista()
                Me.Oggetti = Nothing
                Me.OggettiGridView.Rebind()
            Case "Trova"
                Me.Find()
        End Select
    End Sub

    'Metodo di Ricerca: richiamato dal "Trova" della Toolbar
    Private Sub Find()
        Dim oggetti As New ParsecPro.OggettiRepository
        Dim oggetto As New ParsecPro.Oggetto
        oggetto.Contenuto = Me.OggettoTextBox.Text
        Me.Oggetti = oggetti.GetView(oggetto)
        Me.OggettiGridView.Rebind()
        oggetti.Dispose()
    End Sub

    'Metodo di Salvataggio: richiamato dal "Salva" della Toolbar
    Private Sub Save()
        Dim oggetti As New ParsecPro.OggettiRepository
        Dim oggetto As ParsecPro.Oggetto = oggetti.CreateFromInstance(Me.Oggetto)

        oggetto.Contenuto = Trim(Me.OggettoTextBox.Text.ToUpper)

        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            oggetto.IdClassificazione = CInt(Me.IdClassificazioneTextBox.Text)
        End If
        oggetto.Strutture = Me.Strutture

        Try
            '*******************************************************************
            'Gestione storico
            '*******************************************************************
            oggetti.Oggetto = Me.Oggetto

            oggetti.Save(oggetto)

            '*******************************************************************
            'Aggiorno l'oggetto corrente
            '*******************************************************************
            Me.Oggetto = oggetti.Oggetto
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            oggetti.Dispose()
        End Try
    End Sub

    'Resetta i camopi della pagina e riallinea le griglie
    Private Sub ResettaVista()
        Me.Oggetto = Nothing
        Me.OggettoTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.Strutture = New List(Of ParsecPro.StrutturaOggetto)
        Me.StruttureGridView.DataSource = Me.Strutture
        Me.StruttureGridView.DataBind()
    End Sub

    'Una volta selezionato dalla griglia un elemento, riempie la maschera con i dati dell'elemento selezionato.
    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim oggetti As New ParsecPro.OggettiRepository
        Me.Oggetto = oggetti.GetById(id)

        Me.OggettoTextBox.Text = Me.Oggetto.Contenuto

        Me.ClassificazioneTextBox.Text = Me.Oggetto.Classificazione
        If Me.Oggetto.IdClassificazione.HasValue Then
            Me.IdClassificazioneTextBox.Text = Me.Oggetto.IdClassificazione
        End If

        Me.Strutture = Me.Oggetto.Strutture

        oggetti.Dispose()

    End Sub

    'Evento ItemCreated associato alla Griglia OggettiGridView. Gestisce la navigazione tra pagine della griglia.
    Protected Sub OggettiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles OggettiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    'Evento ItemCommand associato alla OggettiGridView. Fa partire i comandi associati alla griglia degli Oggetti (comandi di selezione e di selezione e conferma dell'oggetto per poter essere associato alla registrazione di protocollo).
    Protected Sub OggettiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles OggettiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
            Case "ConfirmSelectAndClose"
                Me.SelezionaOggetto(e.Item)
        End Select
    End Sub

    'Evento ItemDataBound associato alla OggettiGridView. Setta alcune proprietà sull'ImageButton "Select" situato nella griglia.
    Protected Sub OggettiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles OggettiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona oggetto"
            End If
        End If
    End Sub

    'Evento ItemDataBound associato alla StruttureGridView. Setta alcune proprietà sull'ImageButton "Delete" situato nella griglia.
    Protected Sub StruttureGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles StruttureGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina struttura"
            End If
        End If
    End Sub

    'Evento ItemCommand associato alla StruttureGridView. Fa partire i comandi associati alla griglia delle Strutture (comando "Delete").
    Protected Sub StruttureGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles StruttureGridView.ItemCommand
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Select Case e.CommandName
            Case "Delete"
                Dim struttura As ParsecPro.StrutturaOggetto = Me.Strutture.Where(Function(c) c.Id = id).FirstOrDefault
                Me.Strutture.Remove(struttura)
                Me.StruttureGridView.DataSource = Me.Strutture
                Me.StruttureGridView.DataBind()
        End Select
    End Sub

    'Evento NeedDataSource associato alla griglia OggettiGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione Oggetti.
    Protected Sub OggettiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles OggettiGridView.NeedDataSource
        If Me.Oggetti Is Nothing Then
            Dim oggetti As New ParsecPro.OggettiRepository
            Me.Oggetti = oggetti.GetView(Nothing)
            oggetti.Dispose()
        End If
        Me.OggettiGridView.DataSource = Me.Oggetti
    End Sub

    'Evento per la ricerca delle Strutture dell'Organigramma dell'Ente. Fa partire la maschera RicercaOrganigrammaPage.aspx con possibilità di selezionare una o più Strutture ed associarle all'Oggetto.
    Protected Sub TrovaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaStrutturaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaStrutturaImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 1) 'multipla
        parametriPagina.Add("ultimoLivelloStruttura", "300")
        parametriPagina.Add("livelliSelezionabili", "100,200,300")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    'Viene fatta partire da TrovaStrutturaImageButton.Click.
    'Se sono state selezionate una o più Strutture in RicercaOrganigrammaPage.aspx esse vengono associate alla griglia lista Strutture.
    Protected Sub AggiornaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaStrutturaImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim strutture As List(Of ParsecPro.StrutturaOggetto) = Me.Strutture
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            For Each s In struttureSelezionate
                Dim struttura As ParsecPro.StrutturaOggetto = Nothing
                Dim id As Integer = s.Id
                If strutture.Where(Function(c) c.Id = id).FirstOrDefault Is Nothing Then
                    struttura = New ParsecPro.StrutturaOggetto
                    struttura.Id = s.Id
                    struttura.CodiceStruttura = s.Codice
                    struttura.Descrizione = s.Descrizione
                    strutture.Add(struttura)
                End If
            Next

            Me.Strutture = Strutture
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    'Svuota il campo Oggetto
    Protected Sub EliminaOggettoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaOggettoImageButton.Click
        Me.OggettoTextBox.Text = String.Empty
    End Sub

    'Evento per la ricerca della Classificazione. Fa partire la maschera RicercaClassificazionePage.aspx con possibilità di selezionare una Classificazione ed associarla all'Oggetto.
    Protected Sub TrovaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    End Sub

    'Parte in conseguenza dell'evento TrovaClassificazioneImageButton.Click
    'Se viene selezionata una classificazione in RicercaClassificazionePage.aspx viene associata all'Oggetto.
    Protected Sub AggiornaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim classificazioneCompleta As String = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione(idClassificazione, 1) & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneTextBox.Text = idClassificazione.ToString
            Session("ClassificazioniSelezionate") = Nothing
        End If
    End Sub

    'Svuota i campe relativi alla classicazione: ClassificazioneTextBox e IdClassificazioneTextBox.
    Protected Sub EliminaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaClassificazioneImageButton.Click
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
    End Sub

    'Preleva da ParametriPagina l'Oggeto Filtro per la ricerca
    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("Filtro") Then
                Me.Filtro = parametriPagina("Filtro")
            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    'Richiamato da OggettiGridView.ItemCommand: mette in Sessione l'Oggetto selezionato per poterlo poi recuparare nella maschera richiedente.
    Private Sub SelezionaOggetto(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim oggetti As New ParsecPro.OggettiRepository
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim oggettoSelezionato As ParsecPro.Oggetto = oggetti.GetById(id)
        If Not oggettoSelezionato Is Nothing Then
            Session("OggettoSelezionato") = oggettoSelezionato
        End If
        ParsecUtility.Utility.ClosePopup(False)
        oggetti.Dispose()
    End Sub

End Class
