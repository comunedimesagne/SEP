#Region "Namesopaces importati"

Imports ParsecAdmin
Imports ParsecUtility.Utility
Imports ParsecUtility.Applicazione
Imports ParsecWKF
Imports System.Configuration
Imports System.IO
Imports Telerik.Web.UI

#End Region

Partial Class AttivaProtocolliPage
    Inherits System.Web.UI.Page

#Region "Dichiarazioni"

    Private WithEvents MainPage As MainPage

    Public Enum Direction
        Forward = 0
        Backward = 1
        Sorting = 2
    End Enum

#End Region

#Region "PROPRIETA'"

    Public Property TaskAttivi() As List(Of TaskAttivo)
        Get
            Return CType(Session("AttivaProtocolliPage_Tasks"), List(Of TaskAttivo))
        End Get
        Set(ByVal value As List(Of TaskAttivo))
            Session("AttivaProtocolliPage_Tasks") = value
        End Set
    End Property

    Public Property Filtro() As TaskFiltro
        Get
            Return CType(Session("AttivaProtocolliPage_Filtro"), TaskFiltro)
        End Get
        Set(ByVal value As TaskFiltro)
            Session("AttivaProtocolliPage_Filtro") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Scrivania"
        MainPage.DescrizioneProcedura = "> Scrivania PROTOCOLLI"
        If Not Me.IsPostBack Then
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Me.PopolaMenu()
            Me.CaricaDeleghe(utenteCorrente)
            Me.CaricaStatiWorkflow()
            Me.TaskAttivi = Nothing
            Me.ImpostaFiltroPredefinito()
        End If
    End Sub

    Private Sub ImpostaFiltroPredefinito()
        Dim idUtente As Integer = UtenteCorrente.Id
        If idUtente <> CInt(Me.DelegheScrivaniaComboBox.SelectedItem.Value) Then idUtente = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Me.Filtro = New TaskFiltro With {.IdUtente = idUtente}
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Me.TaskAttivi Is Nothing Then
            Me.ElTask.Text = "Attività " & If(Me.TaskAttivi.Count > 0, "( " & Me.TaskAttivi.Count.ToString & " )", "")
            If Me.TaskAttivi.Count = 0 Then
                Me.EsportaXlsBtn.Enabled = False
                Me.EsportaXlsBtn.ToolTip = "Scrivania Protocolli vuota - non si può esportare nulla!"
            End If
        Else
            Me.EsportaXlsBtn.Enabled = False
            Me.EsportaXlsBtn.ToolTip = "Scrivania Protocolli vuota - non si può esportare nulla!"
        End If
    End Sub

#End Region

#Region "GRIGLIA"

    Protected Sub TaskGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TaskGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.VisualizzaDocumento(e.Item)
        ElseIf e.CommandName = "Execute" Then
            Me.EseguiTask(e.Item)
        End If
    End Sub

    Protected Sub AggiornaTaskButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaTaskButton.Click
        Me.Search()
    End Sub

    Protected Sub TaskGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TaskGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim task As TaskAttivo = CType(e.Item.DataItem, TaskAttivo)
            If task.Note.Length > 0 Then
                e.Item.Cells(2).BackColor = Drawing.Color.Yellow
                e.Item.Cells(2).ToolTip = "Note dell'attività : " & vbCrLf & task.Note
            End If
        End If
    End Sub

    Protected Sub TaskGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles TaskGridView.NeedDataSource
        If Me.TaskAttivi Is Nothing Then
            Dim tasks As New TaskRepository
            Me.TaskAttivi = tasks.GetViewP(Me.Filtro)
            tasks.Dispose()
        End If
        Me.TaskGridView.DataSource = Me.TaskAttivi
    End Sub

    Protected Sub TaskGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles TaskGridView.PreRender
        If TaskGridView.MasterTableView.GetItems(GridItemType.Item).Length > 0 Then
            Dim headerItem As GridHeaderItem = CType(TaskGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = TaskGridView.SelectedItems.Count = TaskGridView.Items.Count
        End If
    End Sub

    Protected Sub TaskGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TaskGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        ElseIf TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#Region "Metodi"

    Private Sub VisualizzaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim taskAttivo As TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = id).FirstOrDefault
        Me.VisualizzaProtocollo(taskAttivo.IdDocumento)
    End Sub

    Private Sub VisualizzaProtocollo(idDocumento As Integer)
        If idDocumento > 0 Then
            Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
            Dim parametriPagina As New Hashtable
            parametriPagina.Add("Filtro", idDocumento)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 940, 510, Nothing, False)
        End If
    End Sub

    Private Sub EseguiTask(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim pageUrl As String = "~/UI/Scrivania/pages/user/OperazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaTaskButton.ClientID)
        queryString.Add("IdTask", id.ToString)
        queryString.Add("IdAttoreCorrente", Me.DelegheScrivaniaComboBox.SelectedItem.Value)
        ParsecUtility.Utility.ShowRadWindow(pageUrl, "EseguiOperazionieRadWindow", queryString, False)
    End Sub

#End Region

#Region "Metodi della selezione check"

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.TaskGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

#End Region

#End Region

#Region "EVENTI OGGETTI"

    Protected Sub DelegheScrivaniaComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles DelegheScrivaniaComboBox.SelectedIndexChanged
        Me.Search()
    End Sub

#Region "Filtro"

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Me.Search()
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResetSearch()
    End Sub

#End Region

#Region "Esportazione"

    Protected Sub EsportaXlsBtn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EsportaXlsBtn.Click
        If Me.TaskAttivi Is Nothing Then
            MessageBox("Non ci sono protocolli, non si può esportare nulla!", False)
        Else
            If Me.TaskAttivi.Count > 0 Then
                Dim exportFilename As String = "Protocolli_" & UtenteCorrente.Id.ToString & "_AL_" & String.Format("{0:dd_MM_yyyy_HHmmss}", Now) & ".xls"
                Dim pathExport As String = ConfigurationManager.AppSettings("PathExport")
                Dim fullPathExport As String = pathExport & exportFilename
                Dim swExport As New StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
                HeaderFile(swExport)
                For Each task As TaskAttivo In Me.TaskAttivi
                    DetailFile(swExport, task)
                Next
                SaveStreamWriter(fullPathExport, exportFilename, swExport)
            Else
                MessageBox("Non ci sono protocolli, non si può esportare nulla", False)
            End If
        End If
    End Sub

#End Region

#Region "Addetti allo smistamento"

    Protected Sub OnCtx_ItemClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadMenuEventArgs)
        Dim menuItem As Telerik.Web.UI.RadMenuItem = e.Item
        Dim userId As String = menuItem.Attributes("UserId")
        Smista(userId)
    End Sub

#End Region

#Region "Operazioni massive"

    ' INVIA
    Protected Sub Btn_Invia_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selezionato As Boolean = False
        Dim msgerr As String = String.Empty
        Dim msg As String = String.Empty
        For Each it As GridDataItem In Me.TaskGridView.MasterTableView.Items
            Dim sChk As CheckBox = CType(it.FindControl("SelectCheckBox"), CheckBox)
            If sChk.Checked Then
                selezionato = True
                Dim id As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
                Dim stato As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("TaskCorrente")
                Dim nomefile As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("NomeFileIter")
                Dim idistanza As Long = it.OwnerTableView.DataKeyValues(it.ItemIndex)("IdIstanza")
                msg &= Me.Procedi(id, stato, nomefile, idistanza, Direction.Forward)
            End If
            If msg.Length > 0 Then
                msgerr &= msg & vbCrLf
                msg = ""
            End If
        Next
        If Not selezionato Then
            MessageBox("L'Inoltra Avanti' è un'operazione massiva, va CHECKATA almeno UN'attività!", False)
        Else
            If msgerr.Length > 0 Then
                RadMessageBox("L'operazione NON E' CONGRUENTE con l'iter di alcuni atti selezionati:" & vbCrLf & msgerr, 500, 120, False)
            Else
                MessageBox("L'operazione si è conclusa con successo!", False)
            End If
        End If
        Me.Search()
    End Sub

    ' PRESA IN CARICO
    Protected Sub Btn_InCarico_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selezionato As Boolean = False
        Dim msgerr As String = String.Empty
        Dim msg As String = String.Empty
        For Each it As Telerik.Web.UI.GridDataItem In Me.TaskGridView.Items
            Dim sChk As CheckBox = CType(it.FindControl("SelectCheckBox"), CheckBox)
            If sChk.Checked Then
                selezionato = True
                Dim id As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
                Dim stato As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("TaskCorrente")
                Dim nomefile As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("NomeFileIter")
                Dim idistanza As Long = it.OwnerTableView.DataKeyValues(it.ItemIndex)("IdIstanza")
                msg &= Me.Procedi(id, stato, nomefile, idistanza, Direction.Backward)
            End If
            If msg.Length > 0 Then
                msgerr &= msg & vbCrLf
                msg = ""
            End If
        Next
        If Not selezionato Then
            MessageBox("Il 'Ritorna Indietro' è un'operazione massiva, va CHECKATA almeno UN'attività!", False)
        Else
            If msgerr.Length > 0 Then
                RadMessageBox("L'operazione NON E' CONGRUENTE con l'iter di alcuni atti selezionati:" & vbCrLf & msgerr, 500, 120, False)
            Else
                MessageBox("L'operazione si è conclusa con successo!", False)
            End If
        End If
        Me.Search()
    End Sub

    ' SMISTA ADDETTO
    Private Sub Smista(ByVal UserId As Integer)
        Dim selezionato As Boolean = False
        Dim msgerr As String = String.Empty
        Dim msg As String = String.Empty
        For Each it As Telerik.Web.UI.GridDataItem In Me.TaskGridView.Items
            Dim sChk As CheckBox = CType(it.FindControl("SelectCheckBox"), CheckBox)
            If sChk.Checked Then
                selezionato = True
                Dim id As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
                Dim stato As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("TaskCorrente")
                Dim nomefile As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("NomeFileIter")
                Dim idistanza As Long = it.OwnerTableView.DataKeyValues(it.ItemIndex)("IdIstanza")
                msg &= Me.Procedi(id, stato, nomefile, idistanza, Direction.Sorting, UserId)
            End If
            If msg.Length > 0 Then
                msgerr &= msg & vbCrLf
                msg = ""
            End If
        Next
        If Not selezionato Then
            MessageBox("La 'Smista Addetto' è un'operazione massiva, va CHECKATA almeno UN'attività!", False)
        Else
            If msgerr.Length > 0 Then
                RadMessageBox("L'operazione NON E' CONGRUENTE con l'iter di alcuni atti selezionati:" & vbCrLf & msgerr, 500, 120, False)
            Else
                MessageBox("L'operazione si è conclusa con successo!", False)
            End If
        End If
        Me.Search()
    End Sub

    ' TODO
    Protected Sub Btn_Notifica_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim selezionato As Boolean = False
        'Dim msgerr As String = String.Empty
        'Dim msg As String = String.Empty
        'For Each it As Telerik.Web.UI.GridDataItem In Me.TaskGridView.Items
        '    Dim sChk As CheckBox = CType(it.FindControl("SelectCheckBox"), CheckBox)
        '    If sChk.Checked Then
        '        selezionato = True
        '        Dim id As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
        '        Dim stato As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("TaskCorrente")
        '        Dim nomefile As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("NomeFileIter")
        '        Dim idistanza As Long = it.OwnerTableView.DataKeyValues(it.ItemIndex)("IdIstanza")
        '        'msg &= Me.Firma(id, stato, nomefile, idistanza, Direction.Sorting)
        '    End If
        '    If msg.Length > 0 Then msgerr &= msg & vbCrLf
        'Next
        'If Not selezionato Then
        '    RadMessageBox("L''Inoltra Addetto' è un'operazione massiva, va CHECKATA almeno UN'attività!", 500, 120, False)
        'Else
        '    If msgerr.Length > 0 Then
        '        RadMessageBox("L'operazione non è congruente con l'iter di alcuni atti selezionati!" & vbCrLf & msgerr, 500, 120, False)
        '    Else
        '        RadMessageBox("L'operazione si è conclusa con successo!" & vbCrLf & msgerr, 250, 120, False)
        '    End If
        'End If
        Me.Search()
    End Sub

    ' TODO
    Protected Sub Btn_NoDestinatario_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim selezionato As Boolean = False
        'Dim msgerr As String = String.Empty
        'Dim msg As String = String.Empty
        'For Each it As Telerik.Web.UI.GridDataItem In Me.TaskGridView.Items
        '    Dim sChk As CheckBox = CType(it.FindControl("SelectCheckBox"), CheckBox)
        '    If sChk.Checked Then
        '        selezionato = True
        '        Dim id As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
        '        Dim stato As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("TaskCorrente")
        '        Dim nomefile As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("NomeFileIter")
        '        Dim idistanza As Long = it.OwnerTableView.DataKeyValues(it.ItemIndex)("IdIstanza")
        '        'msg &= Me.Firma(id, stato, nomefile, idistanza, Direction.Sorting)
        '    End If
        '    If msg.Length > 0 Then msgerr &= msg & vbCrLf
        'Next
        'If Not selezionato Then
        '    RadMessageBox("L''Inoltra Addetto' è un'operazione massiva, va CHECKATA almeno UN'attività!", 500, 120, False)
        'Else
        '    If msgerr.Length > 0 Then
        '        RadMessageBox("L'operazione non è congruente con l'iter di alcuni atti selezionati!" & vbCrLf & msgerr, 500, 120, False)
        '    Else
        '        RadMessageBox("L'operazione si è conclusa con successo!" & vbCrLf & msgerr, 250, 120, False)
        '    End If
        'End If
        Me.Search()
    End Sub

    ' TODO
    Protected Sub Btn_Fine_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim selezionato As Boolean = False
        'Dim msgerr As String = String.Empty
        'Dim msg As String = String.Empty
        'For Each it As Telerik.Web.UI.GridDataItem In Me.TaskGridView.Items
        '    Dim sChk As CheckBox = CType(it.FindControl("SelectCheckBox"), CheckBox)
        '    If sChk.Checked Then
        '        selezionato = True
        '        Dim id As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
        '        Dim stato As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("TaskCorrente")
        '        Dim nomefile As String = it.OwnerTableView.DataKeyValues(it.ItemIndex)("NomeFileIter")
        '        Dim idistanza As Long = it.OwnerTableView.DataKeyValues(it.ItemIndex)("IdIstanza")
        '        'msg &= Me.Firma(id, stato, nomefile, idistanza, Direction.Sorting)
        '    End If
        '    If msg.Length > 0 Then msgerr &= msg & vbCrLf
        'Next
        'If Not selezionato Then
        '    RadMessageBox("L''Inoltra Addetto' è un'operazione massiva, va CHECKATA almeno UN'attività!", 500, 120, False)
        'Else
        '    If msgerr.Length > 0 Then
        '        RadMessageBox("L'operazione non è congruente con l'iter di alcuni atti selezionati!" & vbCrLf & msgerr, 500, 120, False)
        '    Else
        '        RadMessageBox("L'operazione si è conclusa con successo!" & vbCrLf & msgerr, 250, 120, False)
        '    End If
        'End If
        Me.Search()
    End Sub

#End Region

#End Region

#Region "METODI"

    Private Sub CaricaDeleghe(ByVal utenteCorrente As ParsecAdmin.Utente)
        Dim tasks As New TaskRepository
        With Me.DelegheScrivaniaComboBox
            .DataSource = tasks.GetUtentiDelegantiScrivania(utenteCorrente)
            .DataTextField = "Descrizione"
            .DataValueField = "Id"
            .DataBind()
            .FindItemByValue(UtenteCorrente.Id).Selected = True
        End With
        tasks.Dispose()
    End Sub

#Region "Filtro"

    Private Sub CaricaStatiWorkflow()
        Dim tasks As New TaskRepository
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

    Private Sub ResetSearch()
        Me.RiferimentoDocumentoTextBox.Text = String.Empty
        Me.StatoComboBox.ClearSelection()
        Me.TaskAttivi = Nothing
        Me.ImpostaFiltroPredefinito()
        Me.TaskGridView.Rebind()
    End Sub

    Private Sub Search()
        Dim idUtente As Integer = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Dim tasks As New TaskRepository
        Dim filtro As New TaskFiltro
        With filtro
            .IdUtente = idUtente
            .RiferimentoDocumento = Me.RiferimentoDocumentoTextBox.Text
            If Me.StatoComboBox.SelectedIndex <> 0 Then .Stato = Me.StatoComboBox.SelectedItem.Text
        End With
        Me.Filtro = filtro
        Me.TaskAttivi = tasks.GetViewP(Me.Filtro)
        tasks.Dispose()
        Me.TaskGridView.Rebind()
    End Sub

#End Region

#Region "Esportazione"

    Private Sub DetailFile(ByRef sw As StreamWriter, ByVal Task As TaskAttivo)
        Dim line As String = ""
        line &= Task.Documento & vbTab & Task.Proponente & vbTab & Task.TaskCorrente & vbTab & Task.DataScadenza
        sw.WriteLine(line)
    End Sub

    Private Sub HeaderFile(ByRef sw As StreamWriter)
        Dim line As String = String.Empty
        line &= "Protocollo" & vbTab & "Destinato a" & vbTab & "Stato" & vbTab & "Scade"
        sw.WriteLine(line)
    End Sub

    Private Sub SaveStreamWriter(ByVal path As String, ByVal nf As String, ByRef sw As StreamWriter)
        sw.Close()
        Session("AttachmentFullName") = path
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        PageReload(pageUrl, False)
        Dim esportazioniExcel As New ExportExcelRepository
        Dim exportExcel As ExportExcel = esportazioniExcel.CreateFromInstance(Nothing)
        With exportExcel
            .NomeFile = nf
            .Oggetto = "Elenco Protocolli"
            .Utente = UtenteCorrente.Username
            .Data = Now
        End With
        esportazioniExcel.Save(exportExcel)
    End Sub

#End Region

#Region "Addetti allo smistamento"

    Private Sub PopolaMenu()
        Dim utCorr As ParsecAdmin.Utente = CType(UtenteCorrente, ParsecAdmin.Utente)
        Dim ctx As RadContextMenu = Me.ExecuteContextMenu
        Dim btn As RadButton = Me.ExecuteTaskButton
        For Each item In (New TaskRepository).GetAttoriScrivania(utCorr)
            Dim menuItem As New RadMenuItem(item.Descrizione)
            menuItem.Attributes.Add("UserId", item.Id.ToString)
            ctx.Items.Add(menuItem)
        Next
        If ctx.Items.Count > 10 Then
            ctx.DefaultGroupSettings.Height = Unit.Pixel(200)
            ctx.Height = Unit.Pixel(300)
        End If
        btn.Attributes.Add("menu", ctx.ClientID)
    End Sub

#End Region

#Region "Operazioni massive"

    Private Sub AggiungiUtenteVisibilita(ByVal idUtente As Integer, ByVal idistanza As Long)
        Dim idDocumento As Integer = 0
        Dim istanze As New IstanzaRepository
        Dim istanza As Istanza = istanze.GetQuery.Where(Function(c) c.Id = idistanza).FirstOrDefault
        If Not istanza Is Nothing Then idDocumento = istanza.IdDocumento
        istanze.Dispose()
        Dim visibilitaDocumento As New VisibilitaDocumentoRepository
        Dim visibilita = visibilitaDocumento.GetQuery.Where(Function(c) c.IdDocumento = idDocumento And c.IdModulo = 3).ToList
        Dim esiste As Boolean = Not visibilita.Where(Function(c) c.IdEntita = idUtente And c.TipoEntita = TipoEntita.Utente).FirstOrDefault Is Nothing
        If Not esiste Then
            Dim utenti As New UserRepository
            Dim utente As ParsecAdmin.Utente = utenti.GetUserById(idUtente).FirstOrDefault
            utenti.Dispose()
            If Not utente Is Nothing Then
                Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetUtenteVisibilita(utente, 3)
                utenteVisibilita.IdDocumento = idDocumento
                visibilitaDocumento.Add(utenteVisibilita)
                visibilitaDocumento.SaveChanges()
            End If
        End If
        visibilitaDocumento.Dispose()
    End Sub

    Private Function GetIdDestinatario(ByVal nf As String, ByVal stato As String, ByVal idistanza As Long, ByVal act As Action) As Integer
        Dim idDestinatario As Integer = 0
        Dim roleToName = ModelloInfo.ReadActorInfo(nf, stato, act.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Receiver).FirstOrDefault.Name
        Dim processi As New ParametriProcessoRepository
        Dim parametroProcesso As ParametroProcesso = processi.GetQuery.Where(Function(c) c.IdProcesso = idistanza And c.Nome = roleToName).FirstOrDefault
        processi.Dispose()
        If Not parametroProcesso Is Nothing Then
            idDestinatario = CInt(parametroProcesso.Valore)
        Else
            Dim role = (New RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleToName).FirstOrDefault
            Dim idRuolo As Integer
            'Se il ruolo non è presente
            If Not role Is Nothing Then
                idRuolo = role.Id
                idDestinatario = (New RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault.IdUtente
            End If
        End If
        Return idDestinatario
    End Function

    Private Function GetUtenteVisibilita(ByVal utente As ParsecAdmin.Utente, ByVal idModulo As Integer) As ParsecAdmin.VisibilitaDocumento
        Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
        With utenteVisibilita
            .AbilitaCancellaEntita = False
            .Descrizione = utente.Username + " - " + utente.Cognome + " " + utente.Nome
            .TipoEntita = ParsecAdmin.TipoEntita.Utente
            .IdEntita = utente.Id
            .IdModulo = idModulo
            .LogIdUtente = UtenteCorrente.Id
            .LogDataOperazione = Now
        End With
        Return utenteVisibilita
    End Function

    Private Function Procedi(ByVal id As Integer, ByVal stato As String, ByVal nf As String, ByVal idist As Long, ByVal direc As Direction, _
                             Optional ByVal userid As Integer = 0) As String
        Dim act As Action = Nothing
        If direc = Direction.Forward Then
            act = ModelloInfo.ReadActionInfo(stato, nf).Where(Function(c) c.Type = "INVIA AVANTI").FirstOrDefault
        ElseIf direc = Direction.Backward Then
            act = ModelloInfo.ReadActionInfo(stato, nf).Where(Function(c) c.Type = "INVIA INDIETRO").FirstOrDefault
        ElseIf direc = Direction.Sorting Then
            act = ModelloInfo.ReadActionInfo(stato, nf).Where(Function(c) c.Type = "SMISTAMENTO").FirstOrDefault
        End If
        If Not IsNothing(act) Then
            Dim idDestinatario As Integer
            If direc <> Direction.Sorting Then
                idDestinatario = GetIdDestinatario(nf, stato, idist, act)
            Else
                idDestinatario = userid
            End If
            If idDestinatario > 0 Then
                Dim operazione As String = String.Empty
                Select Case direc
                    Case Direction.Backward
                        operazione = "RITORNATO"
                    Case Direction.Forward
                        operazione = "INVIATO"
                    Case Direction.Sorting
                        operazione = "SMISTATO"
                End Select
                Return Me.WriteTask(id, stato, nf, idist, act, idDestinatario, operazione, False)
            Else
                Return "Incogruenza sull' IDTASK=" & id
            End If
        Else
            Return "Incogruenza sull' IDTASK=" & id
        End If
    End Function

    Private Function WriteTask(ByVal id As Integer, ByVal stato As String, ByVal nf As String, ByVal idistanza As Long, ByVal act As Action, _
                               ByVal idDestinatario As Integer, ByVal operazione As String, ByVal notificato As Boolean) As String
        If Not IsNothing(act) Then
            Me.AggiungiUtenteVisibilita(idDestinatario, idistanza)
            Dim tasks As New TaskRepository
            Dim task As Task = tasks.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
            UpdateTaskCorrente(idDestinatario, operazione, notificato, task, tasks)
            Dim statoSuccessivo As String = ModelloInfo.StatoSuccessivoAction(stato, act.Name, nf)
            InsertTaskSuccessivo(idDestinatario, stato, statoSuccessivo, nf, task, tasks)
            Return String.Empty
        Else
            Return "Incogruenza sull' IDTASK=" & id
        End If
    End Function

    Private Sub UpdateTaskCorrente(ByVal iddest As Integer, ByVal op As String, ByVal notif As Boolean, ByRef task As Task, ByRef tasks As TaskRepository)
        Dim statoEseguito As Integer = 6
        With task
            .Note = "Operazione massiva"
            .IdUtenteOperazione = UtenteCorrente.Id
            .IdStato = statoEseguito
            .DataEsecuzione = Now
            .Destinatario = iddest
            .Operazione = op.ToUpper
            .Notificato = notif
        End With
        tasks.SaveChanges()
    End Sub

    Private Sub InsertTaskSuccessivo(ByVal iddest As Integer, ByVal stato As String, ByVal statos As String, ByVal nf As String, ByRef t As Task, ByRef ts As TaskRepository)
        Dim statoDaEseguire As Integer = 5
        Dim nuovotask As New ParsecWKF.Task
        With nuovotask
            .IdIstanza = t.IdIstanza
            .TaskPadre = t.Id
            .Nome = stato
            .Corrente = statos
            .Successivo = ModelloInfo.StatoSuccessivoIter(statos, nf)
            .Mittente = iddest
            .IdStato = statoDaEseguire
            .DataInizio = Now
            .Note = "Operazione massiva"
            .DataFine = Now.AddDays(ModelloInfo.DurataTaskIter(statos, nf))
            .Cancellato = False
        End With
        ts.Add(nuovotask)
        ts.SaveChanges()
        ts.Dispose()
    End Sub

#End Region

#End Region

End Class