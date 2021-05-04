Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneStruttureAbilitateUtentePage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage


#Region "PROPRIETA'"

    Public Property Strutture() As List(Of ParsecAdmin.Struttura)
        Get
            Return CType(Session("GestioneStruttureAbilitateUtentePage_Strutture"), List(Of ParsecAdmin.Struttura))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Struttura))
            Session("GestioneStruttureAbilitateUtentePage_Strutture") = value
        End Set
    End Property

    Public Property Utenti() As List(Of ParsecAdmin.AbilitazioneUtenteStruttura)
        Get
            Return CType(Session("GestioneStruttureAbilitateUtentePage_Utenti"), List(Of ParsecAdmin.AbilitazioneUtenteStruttura))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.AbilitazioneUtenteStruttura))
            Session("GestioneStruttureAbilitateUtentePage_Utenti") = value
        End Set
    End Property

    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("GestioneStruttureAbilitateUtentePage_SelectedItems") Is Nothing Then
                Session("GestioneStruttureAbilitateUtentePage_SelectedItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("GestioneStruttureAbilitateUtentePage_SelectedItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("GestioneStruttureAbilitateUtentePage_SelectedItems") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Gestione Strutture Abilitate"
        If Not Me.Page.IsPostBack Then
            Me.Strutture = Nothing
            Me.SelectedItems = Nothing
            Me.ResettaVista()
            Me.CaricaModuli()
           
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
        Me.StruttureGridView.Style.Add("width", widthStyle)

        Dim message As String = "Eliminare tutti gli elementi selezionati?"
        EliminaUtentiSelezionatiImageButton.Attributes.Add("onclick", "return confirm(""" & message & """)")
        Me.StruttureGridView.GroupingSettings.CaseSensitive = False
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Not Me.Strutture Is Nothing Then
            Me.TitoloElencoStruttureLabel.Text = "Elenco Strutture&nbsp;&nbsp;&nbsp;" & If(Me.Strutture.Count > 0, "( " & Me.Strutture.Count.ToString & " )", "")
        End If
        Me.TitoloElencoUtentiLabel.Text = "Elenco Utenti&nbsp;&nbsp;&nbsp;" & If(Me.Utenti.Count > 0, "( " & Me.Utenti.Count.ToString & " )", "")
        Me.EliminaUtentiSelezionatiImageButton.Enabled = Me.Utenti.Count > 0
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub


#End Region

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
          
            Case "Salva"
                Dim message As String = String.Empty
                Try
                    Me.Save()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                If String.IsNullOrEmpty(message) Then
                    Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                Else
                    ParsecUtility.Utility.MessageBox(message, False)
                End If
            Case "Annulla"
                Me.ResettaVista()
                'DESELEZIONO LA RIGA
                Me.StruttureGridView.SelectedIndexes.Clear()
                Me.UtentiGridView.Rebind()

                'RESETTO I FILTRI
                For Each column As GridColumn In Me.StruttureGridView.MasterTableView.RenderColumns
                    If column.SupportsFiltering Then
                        column.CurrentFilterValue = String.Empty
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter
                    End If
                Next
                Me.StruttureGridView.MasterTableView.FilterExpression = String.Empty
                Me.StruttureGridView.Rebind()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub StruttureGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles StruttureGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
        End Select
    End Sub

    Protected Sub StruttureGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles StruttureGridView.NeedDataSource
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim livelli As New ParsecAdmin.StrutturaLivelloRepository(strutture.Context)
        Dim gruppi As New ParsecAdmin.GruppoRepository(strutture.Context)
        If Me.Strutture Is Nothing Then

            Dim res = From struttura In strutture.GetQuery
                      Join livello In livelli.GetQuery
                      On struttura.IdGerarchia Equals livello.Gerarchia
                      Join gruppo In gruppi.GetQuery
                      On gruppo.Id Equals struttura.IdGruppo
                      Where struttura.LogStato Is Nothing And (struttura.IdGerarchia = 100 Or struttura.IdGerarchia = 200)
                      Order By struttura.IdGerarchia, struttura.Descrizione
                      Select struttura, livello, gruppo

            Me.Strutture = res.AsEnumerable.Select(Function(c) New ParsecAdmin.Struttura With {
                                                       .Id = c.struttura.Id,
                                                       .Codice = c.struttura.Codice,
                                                       .Descrizione = c.struttura.Descrizione,
                                                       .DescrizioneLivello = c.livello.Descrizione,
                                                       .IdGruppo = c.struttura.IdGruppo,
                                                       .DescrizioneGruppo = c.gruppo.Descrizione
                                                       }).ToList
            strutture.Dispose()
        End If
        Me.StruttureGridView.DataSource = Me.Strutture
    End Sub

    Protected Sub StruttureGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles StruttureGridView.ItemCreated
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

    Private Sub CaricaModuli()
        Dim moduli As New ModuleRepository
        Me.ModuloComboBox.DataSource = moduli.GetAbilitazioniModuli
        Me.ModuloComboBox.DataTextField = "Descrizione"
        Me.ModuloComboBox.DataValueField = "Id"
        Me.ModuloComboBox.DataBind()
        Me.ModuloComboBox.SelectedIndex = 0
        moduli.Dispose()
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

    Private Sub AggiornaVista(ByVal codice As Integer)

        Me.ResettaVista()
        Me.CodiceStrutturaHidden.Value = codice.ToString

        Dim utentiStruttura As New ParsecAdmin.AbilitazioneUtenteStrutturaRepository
        Dim utenti As New ParsecAdmin.UserRepository(utentiStruttura.Context)
        Dim moduli As New ParsecAdmin.ModuleRepository(utentiStruttura.Context)

        Dim res = From utente In utenti.GetQuery
                  Join utenteStruttura In utentiStruttura.GetQuery
                  On utente.Id Equals utenteStruttura.IdUtente
                   Join modulo In moduli.GetQuery
                  On utenteStruttura.IdModulo Equals modulo.Id
                  Where utenteStruttura.CodiceStruttura = codice And utenteStruttura.LogTipoOperazione Is Nothing
                 Select utente, utenteStruttura, DescrizioneModulo = modulo.Descrizione

        Dim view = res.AsEnumerable.Select(Function(c) New ParsecAdmin.AbilitazioneUtenteStruttura With {
                                                                                                         .Id = c.utenteStruttura.Id,
                                                                                                         .IdModulo = c.utenteStruttura.IdModulo,
                                                                                                         .IdUtente = c.utenteStruttura.IdUtente,
                                                                                                         .DescrizioneUtente = (If(c.utente.Cognome = Nothing, "", c.utente.Cognome) + " " + If(c.utente.Nome = Nothing, "", c.utente.Nome)),
                                                                                                         .DescrizioneModulo = c.DescrizioneModulo,
                                                                                                         .CodiceStruttura = c.utenteStruttura.CodiceStruttura
                                                                                                         })



        Me.Utenti = view.OrderBy(Function(c) c.DescrizioneUtente).ThenBy(Function(c) c.DescrizioneModulo).ToList

        utentiStruttura.Dispose()

        Me.UtentiGridView.Rebind()

    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim codice As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Codice")
        Me.AggiornaVista(codice)
    End Sub

  
    Private Sub Save()
        If Me.StruttureGridView.SelectedItems.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una struttura!", False)
            Exit Sub
        End If
        Dim utentiStruttura As New ParsecAdmin.AbilitazioneUtenteStrutturaRepository
        Dim gruppi As New ParsecAdmin.GruppoUtenteRepository
        Try
            Dim codice = Me.CodiceStrutturaHidden.Value
            utentiStruttura.DeleteAll(codice)

            Dim gruppo As ParsecAdmin.GruppoUtente = Nothing

            Dim strutturaSelezionata = Me.Strutture.Where(Function(c) c.Codice = codice).FirstOrDefault

            For Each utente In Me.Utenti
                utentiStruttura.Add(utente)
            Next

            'SE SI DESIDERA ASSOCIARE IL GRUPPO DI VISIBILITA' DELLA STRUTTURA SELEZIONATA ALL'UTENTE CORRENTE
            If Me.AggiornaGruppoVisibilitaCheckBox.Checked Then
                Dim idUtente As Integer = 0
                Dim utentiDistinti = Me.Utenti.GroupBy(Function(c) c.IdUtente).Select(Function(c) c.FirstOrDefault).ToList

                For Each utente In utentiDistinti
                    idUtente = utente.IdUtente
                    Dim view = gruppi.Where(Function(c) c.IdUtente = idUtente And c.IdGruppo = strutturaSelezionata.IdGruppo)
                    'SE IL GRUPPO DI VISIBILITA' NON E' ASSOCIATO ALL'UTENTE CORRENTE
                    If Not view.Any Then
                        'SE LA STRUTTURA POSSIEDE UN GRUPPO DI VISIBILITA'
                        If strutturaSelezionata.IdGruppo.HasValue Then
                            'ASSOCIO IL GRUPPO DI VISIBILITA' DELLA STRUTTURA SELEZIONATA ALL'UTENTE CORRENTE
                            gruppo = New ParsecAdmin.GruppoUtente
                            gruppo.IdUtente = utente.IdUtente
                            gruppo.IdGruppo = strutturaSelezionata.IdGruppo
                            gruppi.Add(gruppo)
                        End If

                    End If
                Next
            End If



            gruppi.SaveChanges()
            utentiStruttura.SaveChanges()

            Me.AggiornaVista(codice)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            utentiStruttura.Dispose()
            gruppi.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.CodiceStrutturaHidden.Value = String.Empty
        Me.Utenti = New List(Of ParsecAdmin.AbilitazioneUtenteStruttura)
        Me.ModuloComboBox.SelectedIndex = 0
    End Sub

#End Region



#Region "GESTIONE UTENTI"

    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        If Me.StruttureGridView.SelectedItems.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una struttura!", False)
            Exit Sub
        End If
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
            Dim utente As ParsecAdmin.AbilitazioneUtenteStruttura = Nothing
            Dim idUtente As Integer = 0
            Dim idModulo As Integer = CInt(Me.ModuloComboBox.SelectedItem.Value)
            For Each utenteSelezionato In utentiSelezionati
                idUtente = utenteSelezionato.Key
                Dim esiste As Boolean = Not Me.Utenti.Where(Function(c) c.IdUtente = idUtente And c.IdModulo = idModulo).FirstOrDefault Is Nothing
                If Not esiste Then
                    utente = New ParsecAdmin.AbilitazioneUtenteStruttura
                    utente.IdUtente = idUtente
                    utente.IdModulo = idModulo
                    utente.DescrizioneUtente = utenteSelezionato.Value
                    utente.DescrizioneModulo = Me.ModuloComboBox.SelectedItem.Text
                    utente.Id = -1
                    utente.CodiceStruttura = CInt(Me.CodiceStrutturaHidden.Value)
                    If Me.Utenti.Count > 0 Then
                        'ASSEGNO AGLI UTENTI NON SALVATI UN ID NEGATIVO
                        Dim min = Me.Utenti.Min(Function(c) c.Id)
                        If min < 0 Then
                            utente.Id = min - 1
                        End If
                    End If
                    Me.Utenti.Add(utente)
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
        Dim Id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim utente As ParsecAdmin.AbilitazioneUtenteStruttura = Me.Utenti.Where(Function(c) c.Id = Id).FirstOrDefault
        If Not utente Is Nothing Then
            Me.Utenti.Remove(utente)
        End If
        Me.UtentiGridView.Rebind()
    End Sub

    Protected Sub UtentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim utente As ParsecAdmin.AbilitazioneUtenteStruttura = CType(e.Item.DataItem, ParsecAdmin.AbilitazioneUtenteStruttura)
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina utente"
                Dim message As String = "Eliminare l'elemento selezionato?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim id As String = utente.Id
            If Me.SelectedItems.ContainsKey(id) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(Me.SelectedItems(id).ToString())
                dataItem.Selected = True
            End If
        End If
    End Sub

    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource
        Me.UtentiGridView.DataSource = Me.Utenti
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
        If Me.Utenti.Count > 0 Then
            If Me.UtentiGridView.SelectedItems.Count = 0 Then
                ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno un elemento!", False)
                Exit Sub
            End If
            For Each item As GridDataItem In Me.UtentiGridView.SelectedItems
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim utente As ParsecAdmin.AbilitazioneUtenteStruttura = Me.Utenti.Where(Function(c) c.Id = id).FirstOrDefault
                Me.Utenti.Remove(utente)
            Next
            Me.UtentiGridView.Rebind()
        End If

    End Sub

#End Region

End Class