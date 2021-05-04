#Region "Namespaces importati"

Imports ParsecAdmin
Imports ParsecUtility
Imports System.Configuration
Imports System.IO
Imports System.Web.UI
Imports Telerik.Web.UI

#End Region

Partial Class AttivaModularePage
    Inherits Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property TaskAttiviAtti() As List(Of ParsecWKF.TaskAttivo)
        Get
            Return CType(Session("AttivaModularePage_TaskAttiviAtti"), List(Of ParsecWKF.TaskAttivo))
        End Get
        Set(ByVal value As List(Of ParsecWKF.TaskAttivo))
            Session("AttivaModularePage_TaskAttiviAtti") = value
        End Set
    End Property

    Public Property TaskAttiviProtocolli() As List(Of ParsecWKF.TaskAttivo)
        Get
            Return CType(Session("AttivaModularePage_TaskAttiviProtocolli"), List(Of ParsecWKF.TaskAttivo))
        End Get
        Set(ByVal value As List(Of ParsecWKF.TaskAttivo))
            Session("AttivaModularePage_TaskAttiviProtocolli") = value
        End Set
    End Property

    Public Property Filtro() As ParsecWKF.TaskFiltro
        Get
            Return CType(Session("AttivaModularePage_Filtro"), ParsecWKF.TaskFiltro)
        End Get
        Set(ByVal value As ParsecWKF.TaskFiltro)
            Session("AttivaModularePage_Filtro") = value
        End Set
    End Property

    Public Property FiltroP() As ParsecWKF.TaskFiltro
        Get
            Return CType(Session("AttivaModularePage_FiltroP"), ParsecWKF.TaskFiltro)
        End Get
        Set(ByVal value As ParsecWKF.TaskFiltro)
            Session("AttivaModularePage_FiltroP") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Scrivania"
        MainPage.DescrizioneProcedura = "> Scrivania ATTIVA Modulare"
        If Not Me.IsPostBack Then
            Dim utenteCorrente As ParsecAdmin.Utente = CType(Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Me.CaricaDeleghe(utenteCorrente)
            Me.CaricaStatiWorkflow()
            Me.CaricaModelliWorkflow()
            Me.TaskAttiviAtti = Nothing
            Me.ImpostaFiltroPredefinito()
            Me.TaskAttiviProtocolli = Nothing
            Me.ImpostaFiltroPredefinitoP()
        End If
        Me.RegistraParsecOpenOffice()
        Me.RegistraParsecDigitalSign()

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.scrollPanelAtti.Style.Add("width", widthStyle)
        Me.scrollPanelProtocolli.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Me.TaskAttiviAtti Is Nothing Or Not Me.TaskAttiviProtocolli Is Nothing Then
            Me.ElencoTaskLabel.Text = "Elenco attività per moduli " & If(Me.TaskAttiviAtti.Count + Me.TaskAttiviProtocolli.Count > 0, "( " & (Me.TaskAttiviAtti.Count + Me.TaskAttiviProtocolli.Count).ToString & " )", "")
            If Me.TaskAttiviAtti.Count = 0 And Me.TaskAttiviProtocolli.Count = 0 Then
                Me.EsportaXlsBtn.Enabled = False
                Me.EsportaXlsBtn.ToolTip = "Scrivania Modulare vuota - non si può esportare nulla!"
            End If

            Me.ScrivaniaTabStrip.Tabs(0).Text = "Atti Decisionali" & If(Me.TaskAttiviAtti.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.TaskAttiviAtti.Count.ToString & ")</span>", "<span style='width:20px'></span>")
            Me.ScrivaniaTabStrip.Tabs(1).Text = "Protocolli" & If(Me.TaskAttiviProtocolli.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.TaskAttiviProtocolli.Count.ToString & ")</span>", "<span style='width:20px'></span>")

   
        End If
        Utility.SaveScrollPosition(Me.scrollPanelAtti, Me.scrollPosHiddenAtti, False)
        Utility.SaveScrollPosition(Me.scrollPanelProtocolli, Me.scrollPosHiddenProtocolli, False)
    End Sub

#End Region

#Region "EVENTI GRIGLIA TaskAttiGridView - Atti"

    Protected Sub TaskAttiGridView_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles TaskAttiGridView.ItemCommand
        Select Case e.CommandName
            Case RadGrid.ExpandCollapseCommandName
                If Not e.Item.Expanded Then
                    Dim parentItem As GridDataItem = CType(e.Item, GridDataItem)
                    Dim innerGrid As RadGrid = CType(parentItem.ChildItem.FindControl("IterAttiGridView"), RadGrid)
                    innerGrid.Rebind()
                End If
            Case "Preview"
                Me.VisualizzaAtto(e.Item)
            Case "Execute"
                Me.EseguiTask(e.Item)
        End Select
    End Sub

    Protected Sub TaskAttiGridView_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles TaskAttiGridView.NeedDataSource
        If Me.TaskAttiviAtti Is Nothing Then
            Dim tasks As New ParsecWKF.TaskRepository
            Me.TaskAttiviAtti = tasks.GetIstanzeAttiveAtti(Me.Filtro)
            tasks.Dispose()
        End If
        Me.TaskAttiGridView.DataSource = Me.TaskAttiviAtti
    End Sub

    Protected Sub TaskAttiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles TaskAttiGridView.ItemCreated
        If TypeOf e.Item Is GridFilteringItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
        ElseIf TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        ElseIf TypeOf e.Item Is GridNestedViewItem Then
            AddHandler CType(e.Item.FindControl("IterAttiGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.IterAttiGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("IterAttiGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.IterAttiGridView_ItemCreated)
        ElseIf TypeOf e.Item Is GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA TaskProcolliGridView - Protocollo"

    Protected Sub TaskProcolliGridView_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles TaskProcolliGridView.ItemCommand
        Select Case e.CommandName
            Case RadGrid.ExpandCollapseCommandName
                If Not e.Item.Expanded Then
                    Dim parentItem As GridDataItem = CType(e.Item, GridDataItem)
                    Dim innerGrid As RadGrid = CType(parentItem.ChildItem.FindControl("IterProtocolliGridView"), RadGrid)
                    innerGrid.Rebind()
                End If
            Case "Preview"
                Me.VisualizzaProtocollo(e.Item)
            Case "Execute"
                Me.EseguiTask(e.Item)
        End Select
    End Sub

    Protected Sub TaskProcolliGridView_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles TaskProcolliGridView.NeedDataSource
        If Me.TaskAttiviProtocolli Is Nothing Then
            Dim tasks As New ParsecWKF.TaskRepository
            Me.TaskAttiviProtocolli = tasks.GetIstanzeAttiveProtocollo(Me.FiltroP)
            tasks.Dispose()
        End If
        Me.TaskProcolliGridView.DataSource = Me.TaskAttiviProtocolli
    End Sub

    Protected Sub TaskProcolliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TaskProcolliGridView.ItemCreated
        If TypeOf e.Item Is GridFilteringItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
        ElseIf TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        ElseIf TypeOf e.Item Is GridNestedViewItem Then
            AddHandler CType(e.Item.FindControl("IterProtocolliGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.IterProtocolliGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("IterProtocolliGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.IterProtocolliGridView_ItemCreated)
        ElseIf TypeOf e.Item Is GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "EVENTI COMUNI ALLE GRIGLIE TaskAttiGridView - TaskProcolliGridView"

    Protected Sub AggiornaTaskButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaTaskButton.Click
        Me.Search()
        Dim script As New StringBuilder
        script.AppendLine("<script>HidePanel();hide = true;panelIsVisible=false;</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
    End Sub

    Protected Sub DelegheScrivaniaComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles DelegheScrivaniaComboBox.SelectedIndexChanged
        Me.Search()
    End Sub

    Private Sub EseguiTask(ByVal item As GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim idAttoreCorrente As Integer = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Dim tasks As New ParsecWKF.TaskRepository
        Dim filtro As New ParsecWKF.TaskFiltro
        filtro.IdUtente = idAttoreCorrente
        If Not tasks.GetView(filtro).Where(Function(c) c.Id = id).FirstOrDefault Is Nothing Then
            Dim pageUrl As String = "~/UI/Scrivania/pages/user/OperazionePage.aspx"
            Dim queryString As New Hashtable
            With queryString
                .Add("obj", Me.AggiornaTaskButton.ClientID)
                .Add("IdTask", id.ToString)
                .Add("IdAttoreCorrente", idAttoreCorrente)
            End With
            Dim script As New StringBuilder
            script.AppendLine("<script>ShowPanel();hide=false;panelIsVisible = false</script>")
            Utility.RegisterScript(script, False)
            'Me.OperazioneControl.InitUI(Me.DelegheScrivaniaComboBox.SelectedItem.Value, id)
        End If
        tasks.Dispose()
    End Sub

    Protected Sub SbloccaTaskButton_Click(sender As Object, e As ImageClickEventArgs) Handles SbloccaTaskButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(Applicazione.UtenteCorrente, Utente)
        Me.SbloccaTasks(utenteCollegato.Id)
        Me.Search()
    End Sub


    Private Sub VisualizzaAtto(ByVal item As GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim taskAttivo As ParsecWKF.TaskAttivo = Me.TaskAttiviAtti.Where(Function(c) c.Id = id).FirstOrDefault
        If Not taskAttivo Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = taskAttivo.IdDocumento).FirstOrDefault
            documenti.Dispose()
            If Not documento Is Nothing Then
                Dim queryString As New Hashtable
                queryString.Add("Tipo", documento.IdTipologiaDocumento)
                queryString.Add("Mode", "View")
                queryString.Add("Procedura", "10")
                Dim parametriPagina As New Hashtable
                parametriPagina.Add("IdDocumentoIter", documento.Id)
                Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AttoAmministrativoPage.aspx"
                SessionManager.ParametriPagina = parametriPagina
                Utility.ShowPopup(pageUrl, 930, 650, queryString, False)
            End If
        End If
    End Sub

   

    Private Sub VisualizzaProtocollo(ByVal item As GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim taskAttivo As ParsecWKF.TaskAttivo = Me.TaskAttiviProtocolli.Where(Function(c) c.Id = id).FirstOrDefault
        If Not taskAttivo Is Nothing Then
            Dim parametriPagina As New Hashtable
            Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
            parametriPagina.Add("Filtro", taskAttivo.IdDocumento)
            SessionManager.ParametriPagina = parametriPagina
            Utility.ShowPopup(pageUrl, 940, 510, Nothing, False)
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA INTERNA IterAttiGridView - IterProtocolliGridView"

    Protected Sub IterAttiGridView_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs)
        Dim parentItem As GridDataItem = CType(CType(CType(sender, RadGrid).NamingContainer, GridNestedViewItem).ParentItem, GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("IdIstanza")
        CType(sender, RadGrid).DataSource = (New ParsecWKF.IstanzaRepository).GetTaskIstanze(id)
    End Sub

    Protected Sub IterAttiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub IterProtocolliGridView_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs)
        Dim parentItem As GridDataItem = CType(CType(CType(sender, RadGrid).NamingContainer, GridNestedViewItem).ParentItem, GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("IdIstanza")
        CType(sender, RadGrid).DataSource = (New ParsecWKF.IstanzaRepository).GetTaskIstanze(id)
    End Sub

    Protected Sub IterProtocolliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub CaricaModelliWorkflow()
        Dim modelli As New ParsecWKF.ModelliRepository
        With Me.TipologiaDocumentoComboBox
            .DataSource = modelli.GetKeyValue()
            .DataTextField = "Descrizione"
            .DataValueField = "Id"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem("- Tutte -"))
            .SelectedIndex = 0
        End With
        modelli.Dispose()
    End Sub

    Private Sub CaricaDeleghe(ByVal utenteCorrente As ParsecAdmin.Utente)
        Dim tasks As New ParsecWKF.TaskRepository
        With Me.DelegheScrivaniaComboBox
            .DataSource = tasks.GetUtentiDelegantiScrivania(utenteCorrente)
            .DataTextField = "Descrizione"
            .DataValueField = "Id"
            .DataBind()
            .FindItemByValue(utenteCorrente.Id).Selected = True
        End With
        tasks.Dispose()
    End Sub

    Private Sub CaricaStatiWorkflow()
        Dim tasks As New ParsecWKF.TaskRepository
        With Me.StatoComboBox
            .DataSource = tasks.GetView()
            .DataTextField = "Corrente"
            .DataValueField = "Id"
            .DataBind()
            .Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Tutti -"))
            .SelectedIndex = 0
        End With
        tasks.Dispose()
    End Sub

    Private Sub ExportXls(ByVal Titolo As String)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim exportFilename As String = Titolo & "_" & utenteCorrente.Id.ToString & "_AL_" & String.Format("{0:dd_MM_yyyy_HHmmss}", Now) & ".xls"
        Dim pathExport As String = ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename
        Dim swExport As New StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        HeaderFile(swExport, Titolo)
        If Titolo = "ModulareAtti" Then
           For Each task As ParsecWKF.TaskAttivo In Me.TaskAttiviAtti
                DetailFile(swExport, task)
            Next
        ElseIf Titolo = "ModulareProtocolli" Then
            For Each task As ParsecWKF.TaskAttivo In Me.TaskAttiviProtocolli
                DetailFile(swExport, task)
            Next
        End If
        SaveStreamWriter(fullPathExport, exportFilename, swExport, Titolo, utenteCorrente)
    End Sub

    Private Sub DetailFile(ByRef sw As StreamWriter, ByVal Task As ParsecWKF.TaskAttivo)
        Dim line As String = ""
        line &= Task.Documento & vbTab & Task.Proponente
        sw.WriteLine(line)
    End Sub

    Private Sub HeaderFile(ByRef sw As StreamWriter, ByVal tit As String)
        Dim line As String = String.Empty
        If tit = "ModulareAtti" Then
            line &= "Riferimento Atto" & vbTab & "Settore Proponente"
        ElseIf tit = "ModulareProtocolli" Then
            line &= "Riferimento Protocollo" & vbTab & "Destinatario"
        End If
        sw.WriteLine(line)
    End Sub

    Private Sub SaveStreamWriter(ByVal path As String, ByVal nf As String, ByRef sw As StreamWriter, ByVal tit As String, ByVal uc As ParsecAdmin.Utente)
        sw.Close()
        Session("AttachmentFullName") = path
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        Utility.PageReload(pageUrl, False)
        Dim esportazioniExcel As New ExportExcelRepository
        Dim exportExcel As ExportExcel = esportazioniExcel.CreateFromInstance(Nothing)
        With exportExcel
            .NomeFile = nf
            If tit = "ModulareAtti" Then
                .Oggetto = "Elenco Atti Scrivania Modulare"
            ElseIf tit = "ModulareProtocolli" Then
                .Oggetto = "Elenco Protocolli Scrivania Modulare"
            End If
            .Utente = uc.Username
            .Data = Now
        End With
        esportazioniExcel.Save(exportExcel)
    End Sub

    Private Sub ImpostaFiltroPredefinito()
        Dim utenteCorrente As Utente = CType(Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim idUtente As Integer = utenteCorrente.Id
        If idUtente <> CInt(Me.DelegheScrivaniaComboBox.SelectedItem.Value) Then idUtente = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Me.Filtro = New ParsecWKF.TaskFiltro With {.IdUtente = idUtente}
    End Sub

    Private Sub ImpostaFiltroPredefinitoP()
        Dim utenteCorrente As Utente = CType(Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim idUtente As Integer = utenteCorrente.Id
        If idUtente <> CInt(Me.DelegheScrivaniaComboBox.SelectedItem.Value) Then idUtente = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Me.FiltroP = New ParsecWKF.TaskFiltro With {.IdUtente = idUtente}
    End Sub

    Private Sub ResetSearch()
        Me.RiferimentoDocumentoTextBox.Text = String.Empty
        Me.StatoComboBox.SelectedIndex = 0
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        Me.TaskAttiviAtti = Nothing
        Me.ImpostaFiltroPredefinito()
        Me.TaskAttiGridView.Rebind()
    End Sub

    Private Sub SbloccaTasks(idUtente As Integer)
        Dim taskBloccati As New ParsecWKF.LockTaskRepository
        taskBloccati.DeleteAll(idUtente)
        taskBloccati.Dispose()
    End Sub

    Private Sub Search()
        Dim idUtente As Integer = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Dim filtro As New ParsecWKF.TaskFiltro
        With filtro
            .IdUtente = idUtente
            .RiferimentoDocumento = Me.RiferimentoDocumentoTextBox.Text
            If Me.StatoComboBox.SelectedIndex <> 0 Then .Stato = Me.StatoComboBox.SelectedItem.Text
            If Me.TipologiaDocumentoComboBox.SelectedIndex <> 0 Then .IdModello = Me.TipologiaDocumentoComboBox.SelectedItem.Value
        End With
        Me.Filtro = filtro
        Me.FiltroP = filtro

        Me.AggiornaGriglie()
    End Sub

    Private Sub AggiornaGriglie()
        Me.TaskAttiviAtti = Nothing
        Me.TaskAttiviProtocolli = Nothing
        Me.TaskAttiGridView.Rebind()
        Me.TaskProcolliGridView.Rebind()
    End Sub

#End Region

#Region "EVENTI OGGETTI PAGINA"

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResetSearch()
    End Sub

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles FiltraImageButton.Click
        Me.Search()
    End Sub

    Protected Sub EsportaXlsBtn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EsportaXlsBtn.Click
        If Me.ScrivaniaTabStrip.Tabs(0).Selected Then
            ExportXls("ModulareAtti")
        ElseIf Me.ScrivaniaTabStrip.Tabs(1).Selected Then
            ExportXls("ModulareProtocolli")
        End If
    End Sub

#End Region

#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

    Private Sub RegistraParsecDigitalSign()
        Dim script As String = SignParameters.RegistraParsecDigitalSign
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

End Class