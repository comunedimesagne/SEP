#Region "IMPORTS"

Imports ParsecAdmin
Imports Telerik.Web.UI
Imports System.Data

#End Region

'* SPDX-License-Identifier: GPL-3.0-only

'Permette la ricerca e la selezione di Referenti dall'indiceIPA
Partial Class RicercaIndiceIpaPage
    Inherits System.Web.UI.Page


#Region "ENUMERAZIONI"

    Public Enum TipoPannello
        Filtro = 0
        Risultati = 1
        Dettaglio = 2
    End Enum

#End Region

#Region "GESTIONI PANNELLI"

    'Gestisce la visualizzazione del pannello
    Private Sub VisualizzaPannello(ByVal tipo As TipoPannello)
        Me.FiltroPanel.Visible = False
        Me.RisultatiPanel.Visible = False
        Me.IndietroRisultatiButton.Visible = False
        Select Case tipo
            Case TipoPannello.Filtro
                Me.FiltroPanel.Visible = True
            Case TipoPannello.Risultati
                Me.RisultatiPanel.Visible = True
                Me.IndietroRisultatiButton.Visible = True
            Case TipoPannello.Dettaglio
                '  Me.DettaglioPanel.Visible = True
        End Select

    End Sub

#End Region

#Region "PROPRIETA'"

    'Variabile di Sessione: Elenco dei referenti selezionati nella griglia
    Public Property SelectedItems() As List(Of ParsecPro.IReferente)
        Get
            Return CType(Session("RicercaIndiceIpaPage_SelectedItems"), List(Of ParsecPro.IReferente))
        End Get
        Set(ByVal value As List(Of ParsecPro.IReferente))
            Session("RicercaIndiceIpaPage_SelectedItems") = value
        End Set
    End Property

    'Variabile di Sessione: Elenco delle Amministrazioni
    Public Property Amministrazioni() As List(Of ParsecPro.AmministrazioneIpa)
        Get
            Return CType(Session("RicercaIndiceIpaPage_Amministrazioni"), List(Of ParsecPro.AmministrazioneIpa))
        End Get
        Set(ByVal value As List(Of ParsecPro.AmministrazioneIpa))
            Session("RicercaIndiceIpaPage_Amministrazioni") = value
        End Set
    End Property

    'Variabile di Sessione: Elenco degli Uffici
    Public Property Uffici() As List(Of ParsecPro.UfficioIpa)
        Get
            Return CType(Session("RicercaIndiceIpaPage_Uffici"), List(Of ParsecPro.UfficioIpa))
        End Get
        Set(ByVal value As List(Of ParsecPro.UfficioIpa))
            Session("RicercaIndiceIpaPage_Uffici") = value
        End Set
    End Property

    'Variabile di Sessione: Elenco delle Aree Organizzative Omogenee
    Public Property AreeOrganizzativeOmogenee() As List(Of ParsecPro.AreaOrganizzativaOmogeneaIpa)
        Get
            Return CType(Session("RicercaIndiceIpaPage_AreeOrganizzativeOmogenee"), List(Of ParsecPro.AreaOrganizzativaOmogeneaIpa))
        End Get
        Set(ByVal value As List(Of ParsecPro.AreaOrganizzativaOmogeneaIpa))
            Session("RicercaIndiceIpaPage_AreeOrganizzativeOmogenee") = value
        End Set
    End Property

    'Variabile di Sessione: non utilizzata. Per scopi futuri
    Private Property CodiceAmministrazione As String
        Get
            Return ViewState("CodiceAmministrazione")
        End Get
        Set(ByVal value As String)
            ViewState("CodiceAmministrazione") = value
        End Set
    End Property

    'Variabile di Sessione: Amminsitrazione per Interoperabilità
    Public Property AmmistrazioneInterop() As ParsecPro.Interoperabilita.Amministrazione
        Get
            Return CType(Session("RicercaIndiceIpaPage_AmmistrazioneInterop"), ParsecPro.Interoperabilita.Amministrazione)
        End Get
        Set(ByVal value As ParsecPro.Interoperabilita.Amministrazione)
            Session("RicercaIndiceIpaPage_AmmistrazioneInterop") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim categorie As New ParsecPro.CategorieIpaRepository
        Me.CategorieComboBox.DataSource = ParsecPro.CategorieIpaRepository.CreateCategories 'categorie.GetView()
        Me.CategorieComboBox.DataTextField = "Descrizione"
        Me.CategorieComboBox.DataValueField = "Valore"
        Me.CategorieComboBox.DataBind()
        Me.CategorieComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Seleziona categoria -", "-1"))
        Me.CategorieComboBox.SelectedIndex = 0
        categorie.Dispose()
        If Not Me.IsPostBack Then
            Me.Amministrazioni = New List(Of ParsecPro.AmministrazioneIpa)
            Me.SelectedItems = New List(Of ParsecPro.IReferente)
            Me.VisualizzaPannello(TipoPannello.Filtro)
        End If
    End Sub

    'Evento LoadComplete: gestisce i titoli sulle griglie
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ElencoAmministrazioniLabel.Text = "Elenco Amministrazioni " & If(Me.AmministrazioniGridView.MasterTableView.Items.Count > 0, "( " & Me.AmministrazioniGridView.MasterTableView.Items.Count.ToString & " )", "")
        Me.ElencoReferentiSelezionatiLabel.Text = "Elenco Referenti Selezionati " & If(Me.ElementiSelezionatiGridView.MasterTableView.Items.Count > 0, "( " & Me.ElementiSelezionatiGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla griglia AmministrazioniGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione Amministrazioni.
    Protected Sub AmministrazioniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles AmministrazioniGridView.NeedDataSource
        If Me.Amministrazioni Is Nothing Then
            Me.Amministrazioni = New List(Of ParsecPro.AmministrazioneIpa)
        End If
        Me.AmministrazioniGridView.DataSource = Me.Amministrazioni
    End Sub

    'Evento ItemCreated associato alla Griglia AmministrazioniGridView. Gestisce la navigazione tra pagine della griglia.
    Private Sub AmministrazioniGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles AmministrazioniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    'Evento ItemDataBound associato alla AmministrazioniGridView. Setta alcune proprietà sull'ImageButton "Select" situato nella griglia.
    Protected Sub AmministrazioniGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AmministrazioniGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                If TypeOf dataItem.DataItem Is ParsecPro.AmministrazioneIpa Then
                    btn.ToolTip = "Seleziona amministrazione"
                    btn.CommandArgument = "amministrazione"
                End If
                btn.Style.Add("cursor", "hand")
            End If
        End If
    End Sub

    'Evento ItemCommand associato alla AmministrazioniGridView. Fa partire i comandi associati alla griglia delle Amministrazioni (comandi di selezione).
    Protected Sub AmministrazioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AmministrazioniGridView.ItemCommand
        Select Case e.CommandName
            Case "SelectUffici"
                Me.SelezionaUffici(e.Item, True)
            Case "SelectAoo"
                Me.SelezionaUffici(e.Item, False)
            Case "Select"
                Me.SelezionaReferenteIpa(e.Item, e.CommandArgument)
        End Select
    End Sub

    'Evento ItemCommand associato alla UfficiGridView. Fa partire i comandi associati alla griglia degli Uffici (comandi di selezione).
    Protected Sub UfficiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UfficiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.SelezionaReferenteIpa(e.Item, e.CommandArgument)
        End Select
    End Sub

    'Evento ItemDataBound associato alla UfficiGridView. Setta i tooltip e il "CommandArgument". 
    Protected Sub UfficiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UfficiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                If TypeOf dataItem.DataItem Is ParsecPro.UfficioIpa Then
                    btn.ToolTip = "Seleziona ufficio"
                    btn.CommandArgument = "ufficio"
                Else
                    btn.ToolTip = "Seleziona area organizzativa omogenea"
                    btn.CommandArgument = "aoo"
                End If
                btn.Style.Add("cursor", "hand")
            End If
        End If
    End Sub

    'Evento NeedDataSource associato alla griglia UfficiGridView. Aggancia il datasource della griglia al DB.
    Protected Sub UfficiGridView_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UfficiGridView.NeedDataSource
        If Not Me.Uffici Is Nothing Then
            Me.UfficiGridView.DataSource = Me.Uffici
        Else
            Me.UfficiGridView.DataSource = Me.AreeOrganizzativeOmogenee
        End If
    End Sub

    'Evento ItemCreated associato alla Griglia UfficiGridView. Gestisce la navigazione tra pagine della griglia.
    Private Sub UfficiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UfficiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    'Evento ItemCreated associato alla Griglia ElementiSelezionatiGridView. Setta lo stile.
    Private Sub ElementiSelezionatiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ElementiSelezionatiGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    'Evento ItemDataBound associato alla ElementiSelezionatiGridView. Setta il tooltip e il 'CommandArgument' della griglia del pulsante "Delete". 
    Protected Sub ElementiSelezionatiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ElementiSelezionatiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim referente As ParsecPro.IReferente = dataItem.DataItem
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina referente IPA"
                btn.Style.Add("cursor", "hand")
                btn.CommandArgument = referente.Descrizione
            End If
        End If
    End Sub

    'Evento ItemCommand associato alla ElementiSelezionatiGridView. Fa partire i comandi associati alla griglia delle fatture (comando Delete).
    Protected Sub ElementiSelezionatiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ElementiSelezionatiGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Dim referente As ParsecPro.IReferente = Me.SelectedItems.Where(Function(c) c.Descrizione = e.CommandArgument).FirstOrDefault
                If Not referente Is Nothing Then
                    Me.SelectedItems.Remove(referente)
                End If
        End Select
    End Sub

    'Evento NeedDataSource associato alla griglia ElementiSelezionatiGridView. Aggancia il datasource alla lista SelectedItems.
    Protected Sub ElementiSelezionatiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ElementiSelezionatiGridView.NeedDataSource
        Me.ElementiSelezionatiGridView.DataSource = Me.SelectedItems
    End Sub

    'Permette la selezione di un referente (Ufficio, AOO oppure Amministrazione). Costruisce il Referente e lo aggiunge nella lista 'SelectedItems'. Richiamato da AmministrazioniGridView.ItemCommand
    Private Sub SelezionaReferenteIpa(ByVal item As Telerik.Web.UI.GridDataItem, ByVal tipologia As String)

        Dim codice As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Codice")
        Dim entity As ParsecPro.IEntityIpa = Nothing
        Select Case tipologia.ToLower
            Case "ufficio"
                entity = Me.Uffici.Where(Function(c) c.Codice = codice).FirstOrDefault
            Case "aoo"
                entity = Me.AreeOrganizzativeOmogenee.Where(Function(c) c.Codice = codice).FirstOrDefault
            Case "amministrazione"
                entity = Me.Amministrazioni.Where(Function(c) c.Codice = codice).FirstOrDefault
        End Select

        If Not entity Is Nothing Then

            Dim exist As Boolean
            exist = Not Me.SelectedItems.Where(Function(c) c.Cognome = entity.Nome).FirstOrDefault Is Nothing
            If Not exist Then
                Dim tipoReferente As String = Me.Request.QueryString("tipoReferente")
                Dim referente As ParsecPro.IReferente = Nothing
                If tipoReferente = "Mitt" Then
                    referente = New ParsecPro.Mittente
                Else
                    referente = New ParsecPro.Destinatario
                End If

                '****************************************************************************************************
                'Protocollo Partenza e Interno  è un Destinatario Esterno
                '****************************************************************************************************

                referente.Id = -1
                referente.Codice = -1
                referente.Interno = False
                referente.PerConoscenza = False
                referente.Cognome = entity.Nome
                referente.Nome = Nothing
                referente.Indirizzo = entity.Indirizzo
                referente.Cap = entity.Cap
                referente.Citta = entity.Citta
                referente.Provincia = entity.Provincia
                referente.Rubrica = False
                referente.Email = entity.Email
                referente.Tipologia = 1 'PERSONA GIURIDICA

                '****************************************************************************************************
                'VERIFICO SE IL REFERENTE E' STATO GIA' AGGIUNTO IN RUBRICA
                'CERCO L'ULTIMO
                Dim rubrica As New ParsecAdmin.RubricaRepository
                Dim elementoRubrica = rubrica.Where(Function(c) c.CodiceIPA = entity.Codice And c.LogStato Is Nothing).OrderByDescending(Function(c) c.Id).FirstOrDefault
                If Not elementoRubrica Is Nothing Then
                    referente.Esistente = True
                    referente.Id = elementoRubrica.Id
                Else
                    referente.Esistente = False
                End If

                rubrica.Dispose()

                '****************************************************************************************************

                referente.CodiceIPA = entity.Codice
                referente.Cognome = entity.Nome

                Select Case tipologia.ToLower
                    Case "ufficio"
                        Dim uo As New ParsecPro.Interoperabilita.UnitaOrganizzativa
                        uo.Denominazione = entity.Nome
                        uo.Identificativo = entity.Codice
                        Me.AmmistrazioneInterop.Items = {uo}
                        referente.LivelloGerarchiaIPA = 2

                        'SERIALIZZO IL NODO IPA 
                        referente.NodoIpaXml = Me.SerializzaNodoIpa(Me.AmmistrazioneInterop)

                    Case "aoo"
                        referente.LivelloGerarchiaIPA = 3
                    Case "amministrazione"
                        referente.LivelloGerarchiaIPA = 1
                End Select

                Me.SelectedItems.Add(referente)

                Me.ElementiSelezionatiGridView.Rebind()

            End If

        End If
    End Sub

    'Serializza il Nodo
    Private Function SerializzaNodoIpa(ammistrazione As ParsecPro.Interoperabilita.Amministrazione) As String
        Dim res As String = String.Empty
        Dim serializer = New System.Xml.Serialization.XmlSerializer(GetType(ParsecPro.Interoperabilita.Amministrazione))
        Using stream = New IO.MemoryStream
            serializer.Serialize(stream, ammistrazione)
            res = System.Text.ASCIIEncoding.Default.GetString(stream.ToArray)
        End Using
        Return res
    End Function

    'Permette la selezione di un Ufficio e di popolare le variabili di sessione Ufficio o AreeOrganizzativeOmogenee. Richiamato da AmministrazioniGridView.ItemCommand
    Private Sub SelezionaUffici(ByVal item As Telerik.Web.UI.GridDataItem, ByVal uffici As Boolean)
        Dim ipaSearch As ParsecPro.IpaSearch = Nothing
        Try
            ipaSearch = New ParsecPro.IpaSearch
            Dim codice As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Codice")

            Dim amm = Me.Amministrazioni.Where(Function(c) c.Codice = codice).FirstOrDefault

            Me.AmmistrazioneInterop = New ParsecPro.Interoperabilita.Amministrazione
            Me.AmmistrazioneInterop.CodiceAmministrazione = amm.Codice
            Me.AmmistrazioneInterop.Denominazione = amm.Nome

            If uffici Then
                Me.Uffici = ipaSearch.DoSearchUffici(codice)
                Me.AreeOrganizzativeOmogenee = Nothing
                Me.ElencoUfficiLabel.Text = "Elenco Uffici " & If(Me.Uffici.Count > 0, "( " & Me.Uffici.Count.ToString & " )", "") & " - " & Me.Amministrazioni.Where(Function(c) c.Codice = codice).FirstOrDefault.Nome
            Else
                Me.AreeOrganizzativeOmogenee = ipaSearch.DoSearchAoo(codice)
                Me.Uffici = Nothing
                Me.ElencoUfficiLabel.Text = "Elenco AOO " & If(Me.AreeOrganizzativeOmogenee.Count > 0, "( " & Me.AreeOrganizzativeOmogenee.Count.ToString & " )", "") & " - " & Me.Amministrazioni.Where(Function(c) c.Codice = codice).FirstOrDefault.Nome
            End If

            Me.UfficiGridView.Rebind()
            Me.VisualizzaPannello(TipoPannello.Risultati)

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        Finally
            If Not ipaSearch Is Nothing Then
                ipaSearch.Dispose()
            End If
        End Try
    End Sub


    'Svuota la lista degli Elementi selezionati
    Protected Sub EliminaTuttiReferentiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaTuttiReferentiImageButton.Click
        Me.SelectedItems.Clear()
        Me.ElementiSelezionatiGridView.Rebind()
    End Sub

#End Region

#Region "AZIONI PANNELLO FILTRO"

    'Resetta i campi ChiaveRicercaTextBox e CategorieComboBox utilizzati per la ricerca. Riallinea la griglia AmministrazioniGridView.
    Private Sub ResettaFiltro()
        Me.ChiaveRicercaTextBox.Text = String.Empty
        Me.CategorieComboBox.SelectedIndex = 0
        Me.Amministrazioni = Nothing
        Me.AmministrazioniGridView.Rebind()
    End Sub

    'Richiama il Reset dei campi di ricerca.
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

    'Fa partire la ricerca presso l'indiceIPA
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FiltraImageButton.Click
        Dim ipaSearch As ParsecPro.IpaSearch = Nothing
        Try
            ipaSearch = New ParsecPro.IpaSearch
            Dim categoria As String = String.Empty
            Dim chiaveRicerca As String = Me.ChiaveRicercaTextBox.Text
            If Me.CategorieComboBox.SelectedIndex <> 0 Then
                categoria = Me.CategorieComboBox.SelectedItem.Value
            End If
            Me.Amministrazioni = ipaSearch.DoSearchAmministrazioni(categoria, chiaveRicerca)
            Me.AmministrazioniGridView.Rebind()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        Finally
            If Not ipaSearch Is Nothing Then
                ipaSearch.Dispose()
            End If
        End Try
    End Sub

#End Region

#Region "AZIONI PANNELLO FOOTER"

    'Permette di ritornare ai risultati della ricerca.
    Protected Sub IndietroRisultatiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles IndietroRisultatiButton.Click
        Me.VisualizzaPannello(TipoPannello.Filtro)
        'Deseleziono la riga
        Me.AmministrazioniGridView.SelectedIndexes.Clear()
    End Sub

    'Conferma gli elementi selezionati
    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Session("ReferentiInseriti") = Me.SelectedItems
        ParsecUtility.Utility.ClosePopup(False)
    End Sub

#End Region

   
End Class
