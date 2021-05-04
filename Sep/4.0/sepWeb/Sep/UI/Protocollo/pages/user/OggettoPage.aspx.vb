Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class OggettoPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Public Property Filtro As String = String.Empty

    'Variabile di Sessione: oggetto corrente
    Public Property Oggetto() As ParsecPro.Oggetto
        Get
            Return CType(Session("OggettoPage_Oggetto"), ParsecPro.Oggetto)
        End Get
        Set(ByVal value As ParsecPro.Oggetto)
            Session("OggettoPage_Oggetto") = value
        End Set
    End Property

    'Variabile di Sessione: lista di Oggetti associata alla griglia OggettiGridView.
    Public Property Oggetti() As List(Of ParsecPro.Oggetto)
        Get
            Return CType(Session("OggettoPage_Oggetti"), List(Of ParsecPro.Oggetto))
        End Get
        Set(ByVal value As List(Of ParsecPro.Oggetto))
            Session("OggettoPage_Oggetti") = value
        End Set
    End Property

    'Variabile di Sessione: lista di Strutture associata alla griglia StruttureGridView
    Public Property Strutture() As List(Of ParsecPro.StrutturaOggetto)
        Get
            Return CType(Session("OggettoPage_Strutture"), List(Of ParsecPro.StrutturaOggetto))
        End Get
        Set(ByVal value As List(Of ParsecPro.StrutturaOggetto))
            Session("OggettoPage_Strutture") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.Request("Mode") Is Nothing Then
            MainPage = CType(Me.Master, MainPage)
            MainPage.NomeModulo = "Protocollo"
            MainPage.DescrizioneProcedura = "> Gestione Oggetti Standard per le Registrazioni"
            Me.OggettiGridView.MasterTableView.GetColumn("ConfirmSelectAndClose").Display = False
        Else
            Me.MainPage = CType(Me.Master, BlankPage)
        End If
    End Sub

    'Evento LoadComplete della Pagina.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.Request("Mode") Is Nothing Then
            If Not Me.Page.IsPostBack Then
                Me.GetParametri()
                Me.OggettoTextBox.Text = Me.Filtro
                Me.Search()
            End If
        End If
    End Sub

    'Evento LoadComplete associato alla Pagina
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'oggetto selezionato?", False, Not Me.Oggetto Is Nothing)
        If Not IsPostBack AndAlso Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
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

    'Evento PreInit associato alla Pagina
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BlankPage.master"
        End If
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    'Evento Cick associato alla RadToolBar: lancia i vari comandi dalla Toolbar
    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
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
                    If Not Me.Oggetto Is Nothing Then
                        Me.Delete()
                        Me.ResettaVista()
                        Me.AggiornaGriglia()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un'oggetto!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

    'Evento ItemCreated associato alla RadToolBar. Aggiunge l'evento onclick al pulsante Elimiina
    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemCommand associato alla Griglia OggettiGridView. Fa partire i vari comandi associati alla griglia
    Protected Sub OggettiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles OggettiGridView.ItemCommand
       Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
            Case "ConfirmSelectAndClose"
                Me.SelezionaOggetto(e.Item)
        End Select
    End Sub

    'Evento ItemDataBound associato alla Griglia OggettiGridView. Setta i tooltip e i text in base alle informazioni contenute nelle celle.
    Protected Sub OggettiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles OggettiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona oggetto"
            End If

            Dim oggettoCorrente As ParsecPro.Oggetto = CType(dataItem.DataItem, ParsecPro.Oggetto)

            If Not String.IsNullOrEmpty(oggettoCorrente.Contenuto) Then
                If oggettoCorrente.Contenuto.Length > 83 Then
                    dataItem("Contenuto").Text = oggettoCorrente.Contenuto.Substring(0, 80) + "..."
                End If
                dataItem("Contenuto").ToolTip = oggettoCorrente.Contenuto
            End If

            If Not String.IsNullOrEmpty(oggettoCorrente.Classificazione) Then
                If oggettoCorrente.Classificazione.Length > 55 Then
                    dataItem("Classificazione").Text = oggettoCorrente.Classificazione.Substring(0, 52) + "..."
                End If
                dataItem("Classificazione").ToolTip = oggettoCorrente.Classificazione
            End If
        End If
    End Sub

    'Evento NeedDataSource associato alla griglia OggettiGridView. Aggancia il datasource della griglia al DB. Carica la variabile di sessione Oggetti.
    Protected Sub OggettiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles OggettiGridView.NeedDataSource
        If Me.Oggetti Is Nothing Then
            Dim oggetti As New ParsecPro.OggettiRepository
            Me.Oggetti = oggetti.GetView(Nothing)
            oggetti.Dispose()
        End If
        Me.OggettiGridView.DataSource = Me.Oggetti
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

    'Evento Click che svuola l'oggetto.
    Protected Sub EliminaOggettoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaOggettoImageButton.Click
        Me.OggettoTextBox.Text = String.Empty
    End Sub

#End Region

#Region "METODI PRIVATI"

    'Metodo che fa partire l'aggiornamento della griglia
    Private Sub AggiornaGriglia()
        Me.Oggetti = Nothing
        Me.OggettiGridView.Rebind()
    End Sub

    'Imposta il filtro per la Ricerca
    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("Filtro") Then
                Me.Filtro = parametriPagina("Filtro")
            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    'Seleziona un Oggetto: richiamato da OggettiGridView.ItemCommand (evento associato alla griglia).
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

    'Metodo che riempie i vari campi quando un Oggetto viene selezionato dalla Griglia
    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim oggetti As New ParsecPro.OggettiRepository
        Me.Oggetto = oggetti.GetById(id)

        Me.OggettoTextBox.Text = Me.Oggetto.Contenuto

        If Me.Oggetto.IdProcedimento.HasValue Then
            Me.IdProcedimentoTextBox.Text = Me.Oggetto.IdProcedimento.Value.ToString
            Me.ProcedimentoTextBox.Text = Me.Oggetto.DescrizioneProcedimento
        End If

        Me.ClassificazioneTextBox.Text = Me.Oggetto.Classificazione
        If Me.Oggetto.IdClassificazione.HasValue Then
            Me.IdClassificazioneTextBox.Text = Me.Oggetto.IdClassificazione
        End If

        Me.Strutture = Me.Oggetto.Strutture

        oggetti.Dispose()

    End Sub

    'Metodo che ricerca e che popola la griglia. Richiamato da Me.Load()
    Private Sub Search()
        Dim oggetti As New ParsecPro.OggettiRepository
        Dim searchTemplate As New ParsecPro.Oggetto
        searchTemplate.Contenuto = Me.OggettoTextBox.Text
        Me.Oggetti = oggetti.GetView(searchTemplate)
        Me.OggettiGridView.Rebind()
        oggetti.Dispose()
    End Sub


    'Metodo Print
    Private Sub Print()
        'TODO
    End Sub

    'Cancella glo Oggetti dal DB. Caso "Elimina" della Toolbar.
    Private Sub Delete()
        Dim oggetti As New ParsecPro.OggettiRepository
        oggetti.Delete(Me.Oggetto)
        oggetti.Dispose()
    End Sub

    'Salva gli Oggetti su DB.
    Private Sub Save()
        Dim oggetti As New ParsecPro.OggettiRepository
        Dim oggetto As ParsecPro.Oggetto = oggetti.CreateFromInstance(Me.Oggetto)

        oggetto.Contenuto = Trim(Me.OggettoTextBox.Text.ToUpper)

        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            oggetto.IdClassificazione = CInt(Me.IdClassificazioneTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.IdProcedimentoTextBox.Text) Then
            oggetto.IdProcedimento = CInt(Me.IdProcedimentoTextBox.Text)
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

    'Resetta tutti i campi della interfaccia grafica.
    Private Sub ResettaVista()
        Me.Oggetto = Nothing
        Me.OggettoTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.ProcedimentoTextBox.Text = String.Empty
        Me.IdProcedimentoTextBox.Text = String.Empty
        Me.Strutture = New List(Of ParsecPro.StrutturaOggetto)
        Me.StruttureGridView.DataSource = Me.Strutture
        Me.StruttureGridView.DataBind()
    End Sub


#End Region

#Region "GESTIONE STRUTTURE"

    'Evento ItemDataBound associato alla Griglia StruttureGridView. Setta i tooltip in base alle informazioni contenute nelle celle.
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

    'Evento ItemCommand associato alla Griglia StruttureGridView. Permette l'esecuzione dei vari comandi attivabili dalla griglia StruttureGridView.
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

    'Evento Click di Trova TrovaStrutturaImageButton. Fa partire la maschera dell'organigramma per cercare e associare una Struttura
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

    'Metodo che assegna le Strutture selezionate in TrovaStrutturaImageButton_Click.
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
            Me.Strutture = strutture
            Session("SelectedStructures") = Nothing
        End If
    End Sub

#End Region

#Region "GESTIONE CLASSIFICAZIONE"

    'Evento Click di Trova TrovaClassificazioneImageButton. Fa partire la maschera RicercaClassificazionePage per cercare e associare una Classificazione
    Protected Sub TrovaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    End Sub

    'Metodo che assegna le Classificazioni selezionate in TrovaClassificazioneImageButton_Click
    Protected Sub AggiornaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim classificazioneCompleta As String = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione2(idClassificazione, 1) & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneTextBox.Text = idClassificazione.ToString
            Session("ClassificazioniSelezionate") = Nothing
        End If
    End Sub

    'Evento Click di EliminaClassificazioneImageButton. Svuota ClassificazioneTextBox e IdClassificazioneTextBox
    Protected Sub EliminaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaClassificazioneImageButton.Click
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
    End Sub

#End Region

#Region "GESTIONE PROCEDIMENTO"

    'Evento Click di Trova TrovaProcedimentoImageButton. Fa partire la maschera RicercaProcedimentoPage per cercare e associare un Procedimento
    Protected Sub TrovaProcedimentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaProcedimentoImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaProcedimentoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaProcedimentoImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 920, 585, queryString, False)
    End Sub

    'Metodo che assegna i Procedimenti selezionati in TrovaProcedimentoImageButton_Click
    Protected Sub AggiornaProcedimentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaProcedimentoImageButton.Click
        If Not ParsecUtility.SessionManager.Procedimento Is Nothing Then
            Dim procedimento As ParsecAdmin.Procedimento = ParsecUtility.SessionManager.Procedimento
            Me.ProcedimentoTextBox.Text = procedimento.Nome
            Me.IdProcedimentoTextBox.Text = procedimento.Id.ToString
            ParsecUtility.SessionManager.Procedimento = Nothing
        End If
    End Sub

    'Evento Click di EliminaProcedimentoImageButton. Svuota ProcedimentoTextBox e IdProcedimentoTextBox
    Protected Sub EliminaProcedimentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaProcedimentoImageButton.Click
        Me.ProcedimentoTextBox.Text = String.Empty
        Me.IdProcedimentoTextBox.Text = String.Empty
    End Sub

#End Region

End Class