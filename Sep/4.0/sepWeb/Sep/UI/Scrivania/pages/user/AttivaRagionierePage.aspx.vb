#Region "Namesopaces importati"

Imports ParsecAdmin
Imports ParsecUtility.Utility
Imports ParsecUtility.Applicazione
Imports ParsecWKF
Imports System.Configuration
Imports System.IO
Imports Telerik.Web.UI

#End Region

Partial Class AttivaRagionierePage
    Inherits System.Web.UI.Page

#Region "Dichiarazioni"

    Private WithEvents MainPage As MainPage
    Public si As Integer
    Public sin As Integer
    Public sa As Integer
    Public san As Integer
    Public sl As Integer
    Public sln As Integer

    Public Enum Direction
        Forward = 0
        Backward = 1
        Sorting = 2
    End Enum

#End Region

#Region "PROPRIETA'"

    Public Property TaskAttivi() As List(Of TaskAttivo)
        Get
            Return CType(Session("AttivaRagionierePage_Tasks"), List(Of TaskAttivo))
        End Get
        Set(ByVal value As List(Of TaskAttivo))
            Session("AttivaRagionierePage_Tasks") = value
        End Set
    End Property

    Public Property Filtro() As TaskFiltro
        Get
            Return CType(Session("AttivaRagionierePage_Filtro"), TaskFiltro)
        End Get
        Set(ByVal value As TaskFiltro)
            Session("AttivaRagionierePage_Filtro") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Scrivania"
        MainPage.DescrizioneProcedura = "> Scrivania RAGIONIERE"
        If Not Me.IsPostBack Then
            Me.PopolaMenu()
            Me.CaricaDeleghe()
            Me.CaricaStatiWorkflow()
            Me.CaricaModelliWorkflow()
            Me.TaskAttivi = Nothing
            Me.ImpostaFiltroPredefinito()
        End If
    End Sub

    Private Sub ImpostaFiltroPredefinito()
        Dim idUtente As Integer = utenteCorrente.Id
        If idUtente <> CInt(Me.DelegheScrivaniaComboBox.SelectedItem.Value) Then idUtente = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Me.Filtro = New TaskFiltro With {.IdUtente = idUtente}
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Me.TaskAttivi Is Nothing Then
            Me.ElTask.Text = "Attività " & If(Me.TaskAttivi.Count > 0, "( " & Me.TaskAttivi.Count.ToString & " )", "")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub TaskGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TaskGridView.ItemCommand
        If e.CommandName = Telerik.Web.UI.RadGrid.ExpandCollapseCommandName AndAlso Not e.Item.Expanded Then
            Dim parentItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
            Dim innerGridi As Telerik.Web.UI.RadGrid = CType(parentItem.ChildItem.FindControl("ImpGridView"), Telerik.Web.UI.RadGrid)
            innerGridi.Rebind()
            Dim innerGridl As Telerik.Web.UI.RadGrid = CType(parentItem.ChildItem.FindControl("LiqGridView"), Telerik.Web.UI.RadGrid)
            innerGridl.Rebind()
            Dim innerGrida As Telerik.Web.UI.RadGrid = CType(parentItem.ChildItem.FindControl("AccGridView"), Telerik.Web.UI.RadGrid)
            innerGrida.Rebind()
        ElseIf e.CommandName = "Preview" Then
            Me.VisualizzaDocumento(e.Item)
        ElseIf e.CommandName = "Execute" Then
            Me.EseguiTask(e.Item)
        End If
    End Sub

    Protected Sub AggiornaTaskButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaTaskButton.Click
        Me.Search()
    End Sub

    Protected Sub TaskGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TaskGridView.ItemDataBound
        Dim img As Image = Nothing
        Dim lbl As Label = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            img = CType(e.Item.FindControl("Col_Imp"), Image)
            lbl = CType(e.Item.FindControl("LblTipo"), Label)
            Dim task As TaskAttivo = CType(e.Item.DataItem, TaskAttivo)
            With img
                If task.TotaleImporto = 0 Then
                    .ImageUrl = "~/images/pRosso16.png"
                    .ToolTip = "Dati contabili non indicati"
                    e.Item.Cells(10).ForeColor = Drawing.Color.Red
                Else
                    .ImageUrl = "~/images/pVerde16.png"
                    .ToolTip = "Dati contabili indicati"
                    e.Item.Cells(10).ForeColor = Drawing.Color.Green
                    e.Item.Cells(10).Font.Bold = True
                End If
            End With
            With lbl
                If task.Accertamento Then
                    .Text = "Acc"
                    .ToolTip = "Atto (ID=" & task.Id & ") di accertamento"
                    .ForeColor = Drawing.Color.Green
                ElseIf task.Impegno And Not task.Liquidazione Then
                    .Text = "Imp"
                    .ToolTip = "Atto (ID=" & task.Id & ") con impegno di spesa"
                    .ForeColor = Drawing.Color.Magenta
                    .Font.Bold = True
                ElseIf Not task.Impegno And task.Liquidazione Then
                    .Text = "Liq"
                    .ToolTip = "Atto (ID=" & task.Id & ") di liquidazione"
                    .ForeColor = Drawing.Color.Red
                    .Font.Bold = True
                ElseIf task.Impegno And task.Liquidazione Then
                    .Text = "Imp - Liq"
                    .ToolTip = "Atto (ID=" & task.Id & ") con impegno di spesa e liquidazione"
                    .ForeColor = Drawing.Color.Red
                    .Font.Bold = True
                End If
            End With
            If Not String.IsNullOrEmpty(task.Note) Then
                e.Item.Cells(2).BackColor = Drawing.Color.Yellow
                e.Item.Cells(2).ToolTip = "Note dell'attività : " & vbCrLf & task.Note
            End If
        ElseIf TypeOf e.Item Is Telerik.Web.UI.GridNestedViewItem Then
            Dim task As TaskAttivo = CType(e.Item.DataItem, TaskAttivo)
            Dim t1 As HtmlTable = CType(e.Item.FindControl("AccTable"), HtmlTable)
            t1.Visible = task.Accertamento
            Dim t2 As HtmlTable = CType(e.Item.FindControl("LiqTable"), HtmlTable)
            t2.Visible = task.Liquidazione
            Dim t3 As HtmlTable = CType(e.Item.FindControl("ImpTable"), HtmlTable)
            t3.Visible = task.Impegno
        End If
    End Sub

    Protected Sub TaskGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles TaskGridView.NeedDataSource
        If Me.TaskAttivi Is Nothing Then
            Dim tasks As New TaskRepository
            Me.TaskAttivi = tasks.GetViewR(Me.Filtro)
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

        If TypeOf e.Item Is GridFilteringItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            'e.Item.Style.Add("background-color", "White")
        End If

        If TypeOf e.Item Is GridNestedViewItem Then
            AddHandler CType(e.Item.FindControl("ImpGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.ImpGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("ImpGridView"), RadGrid).ItemDataBound, New GridItemEventHandler(AddressOf Me.ImpGridView_ItemDataBound)
            AddHandler CType(e.Item.FindControl("ImpGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.ImpGridView_ItemCreated)
            AddHandler CType(e.Item.FindControl("LiqGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.LiqGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("LiqGridView"), RadGrid).ItemDataBound, New GridItemEventHandler(AddressOf Me.LiqGridView_ItemDataBound)
            AddHandler CType(e.Item.FindControl("LiqGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.LiqGridView_ItemCreated)
            AddHandler CType(e.Item.FindControl("AccGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.AccGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("AccGridView"), RadGrid).ItemDataBound, New GridItemEventHandler(AddressOf Me.AccGridView_ItemDataBound)
            AddHandler CType(e.Item.FindControl("AccGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.AccGridView_ItemCreated)
        ElseIf TypeOf e.Item Is GridHeaderItem Then
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
        Me.VisualizzaAtto(taskAttivo.IdDocumento)
    End Sub

    Private Sub VisualizzaAtto(idDocumento As Integer)
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        If Not documento Is Nothing Then
            Dim queryString As New Hashtable
            queryString.Add("Tipo", documento.IdTipologiaDocumento)
            queryString.Add("Mode", "View")
            queryString.Add("Procedura", "10")
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AttoAmministrativoPage.aspx"
            Dim parametriPagina As New Hashtable
            parametriPagina.Add("IdDocumentoIter", idDocumento)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 600, queryString, False)
        End If
        documenti.Dispose()
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

#Region "EVENTI GRIGLIE INTERNE"

    Protected Sub ImpGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Dim parentItem As Telerik.Web.UI.GridDataItem = CType(CType(CType(sender, Telerik.Web.UI.RadGrid).NamingContainer, Telerik.Web.UI.GridNestedViewItem).ParentItem, Telerik.Web.UI.GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("IdDocumento")
        CType(sender, Telerik.Web.UI.RadGrid).DataSource = (New ParsecAtt.DocumentoRepository).GetImpegniSpesa(id)
    End Sub

    Protected Sub ImpGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim imp As ParsecAtt.ImpegnoSpesa = CType(e.Item.DataItem, ParsecAtt.ImpegnoSpesa)
            If IsNothing(imp.NumeroImpegno) Then
                e.Item.Cells(8).ForeColor = Drawing.Color.Red
            Else
                If imp.NumeroImpegno > 0 Then
                    e.Item.Cells(8).ForeColor = Drawing.Color.Green
                    e.Item.Cells(8).Font.Bold = True
                    e.Item.Cells(8).Font.Bold = True
                    sin += imp.Importo
                Else
                    e.Item.Cells(8).ForeColor = Drawing.Color.Red
                End If
            End If
            si += imp.Importo
        ElseIf TypeOf e.Item Is Telerik.Web.UI.GridFooterItem Then
            Dim fI As GridFooterItem = CType(e.Item, GridFooterItem)
            With fI("Importo")
                If si <> sin Then
                    .Text = String.Format("{0:N2}", si) & "  ( " & String.Format("{0:N2}", sin) & " ) "
                    .ToolTip = "Importo Totale Impegni ( numero impegno specificato )"
                    .Font.Size = 8
                Else
                    .Text = String.Format("{0:N2}", si)
                    .ToolTip = "Importo Totale Impegni"
                End If
                .Font.Bold = True
            End With
            si = 0
            sin = 0
        End If
    End Sub

    Protected Sub ImpGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub LiqGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Dim parentItem As Telerik.Web.UI.GridDataItem = CType(CType(CType(sender, Telerik.Web.UI.RadGrid).NamingContainer, Telerik.Web.UI.GridNestedViewItem).ParentItem, Telerik.Web.UI.GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("IdDocumento")
        CType(sender, Telerik.Web.UI.RadGrid).DataSource = (New ParsecAtt.DocumentoRepository).GetLiquidazioni(id)
    End Sub

    Protected Sub LiqGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim liq As ParsecAtt.Liquidazione = CType(e.Item.DataItem, ParsecAtt.Liquidazione)
            If IsNothing(liq.Numero) Then
                e.Item.Cells(11).ForeColor = Drawing.Color.Red
            Else
                If liq.Numero > 0 Then
                    e.Item.Cells(11).ForeColor = Drawing.Color.Green
                    e.Item.Cells(11).Font.Bold = True
                    e.Item.Cells(11).Font.Bold = True
                    sln += liq.ImportoLiquidato
                Else
                    e.Item.Cells(11).ForeColor = Drawing.Color.Red
                End If
            End If
            sl += liq.ImportoLiquidato
        ElseIf TypeOf e.Item Is Telerik.Web.UI.GridFooterItem Then
            Dim fI As GridFooterItem = CType(e.Item, GridFooterItem)
            With fI("ImportoLiquidato")
                If sl <> sln Then
                    .Text = String.Format("{0:N2}", sl) & "  ( " & String.Format("{0:N2}", sln) & " ) "
                    .ToolTip = "Importo Totale Liquidazioni ( numero liquidazione specificato )"
                    .Font.Size = 8
                Else
                    .Text = String.Format("{0:N2}", sl)
                    .ToolTip = "Importo Totale Liquidazioni"
                End If
                .Font.Bold = True
            End With
            sl = 0
            sln = 0
        End If
    End Sub

    Protected Sub LiqGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub AccGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Dim parentItem As Telerik.Web.UI.GridDataItem = CType(CType(CType(sender, Telerik.Web.UI.RadGrid).NamingContainer, Telerik.Web.UI.GridNestedViewItem).ParentItem, Telerik.Web.UI.GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("IdDocumento")
        CType(sender, Telerik.Web.UI.RadGrid).DataSource = (New ParsecAtt.DocumentoRepository).GetAccertamenti(id)
    End Sub

    Protected Sub AccGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim acc As ParsecAtt.Accertamento = CType(e.Item.DataItem, ParsecAtt.Accertamento)
            If IsNothing(acc.NumeroAccertamento) Then
                e.Item.Cells(8).ForeColor = Drawing.Color.Red
            Else
                If acc.NumeroAccertamento > 0 Then
                    e.Item.Cells(8).ForeColor = Drawing.Color.Green
                    e.Item.Cells(8).Font.Bold = True
                    san += acc.Importo
                Else
                    e.Item.Cells(8).ForeColor = Drawing.Color.Red
                End If
            End If
            sa += acc.Importo
        ElseIf TypeOf e.Item Is Telerik.Web.UI.GridFooterItem Then
            Dim fI As GridFooterItem = CType(e.Item, GridFooterItem)
            With fI("Importo")
                If sa <> san Then
                    .Text = String.Format("{0:N2}", sa) & "  ( " & String.Format("{0:N2}", san) & " ) "
                    .ToolTip = "Importo Totale Accertamento ( numero accertamento specificato )"
                    .Font.Size = 8
                Else
                    .Text = String.Format("{0:N2}", sa)
                    .ToolTip = "Importo Totale Accertamento"
                End If
                .Font.Bold = True
            End With
            sa = 0
            san = 0
        End If
    End Sub

    Protected Sub AccGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
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

#Region "EVENTI OGGETTI DELLA PAGINA"

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
            MessageBox("Non ci sono atti, non si può esportare nulla!", False)
        Else
            If Me.TaskAttivi.Count > 0 Then
                Dim exportFilename As String = "Ragioniere_" & UtenteCorrente.Id.ToString & "_AL_" & String.Format("{0:dd_MM_yyyy_HHmmss}", Now) & ".xls"
                ' Dim pathExport As String = ConfigurationManager.AppSettings("PathExport")
                Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")

                Dim fullPathExport As String = pathExport & exportFilename
                Dim swExport As New StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
                HeaderFile(swExport)
                For Each task As TaskAttivo In Me.TaskAttivi
                    DetailFile(swExport, task)
                    If task.Accertamento Then WriteAcc(swExport, task.IdDocumento)
                    If task.Impegno Then WriteImp(swExport, task.IdDocumento)
                    If task.Liquidazione Then WriteLiq(swExport, task.IdDocumento)
                Next
                SaveStreamWriter(fullPathExport, exportFilename, swExport)
            Else
                MessageBox("Non ci sono atti, non si può esportare nulla", False)
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

    ' INVIA AVANTI
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

    ' RITORNA INDIETRO
    Protected Sub Btn_Indietro_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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

    ' TODO FIRMA (Me.Firma)
    Protected Sub Btn_Firma_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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
                'msg &= Me.Firma(id, stato, nomefile, idistanza, Direction.Sorting)
            End If
            If msg.Length > 0 Then msgerr &= msg & vbCrLf
        Next
        If Not selezionato Then
            RadMessageBox("L''Inoltra Addetto' è un'operazione massiva, va CHECKATA almeno UN'attività!", 500, 120, False)
        Else
            If msgerr.Length > 0 Then
                RadMessageBox("L'operazione non è congruente con l'iter di alcuni atti selezionati!" & vbCrLf & msgerr, 500, 120, False)
            Else
                RadMessageBox("L'operazione si è conclusa con successo!" & vbCrLf & msgerr, 250, 120, False)
            End If
        End If
        Me.Search()
    End Sub

#End Region

#End Region

#Region "METODI"

    Private Sub CaricaDeleghe()
        Dim tasks As New TaskRepository
        With Me.DelegheScrivaniaComboBox
            .DataSource = tasks.GetUtentiDelegantiScrivania(UtenteCorrente)
            .DataTextField = "Descrizione"
            .DataValueField = "Id"
            .DataBind()
            .FindItemByValue(UtenteCorrente.Id).Selected = True
        End With
        tasks.Dispose()
    End Sub

#Region "Filtro"

    Private Sub CaricaModelliWorkflow()
        Dim modelli As New ModelliRepository
        With Me.TipologiaDocumentoComboBox
            .DataSource = modelli.GetQuery.ToList
            .DataTextField = "Descrizione"
            .DataValueField = "Id"
            .DataBind()
            .Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Tutte -"))
            .SelectedIndex = 0
        End With
        modelli.Dispose()
    End Sub

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
            If Me.TipologiaDocumentoComboBox.SelectedIndex <> 0 Then .IdModello = Me.TipologiaDocumentoComboBox.SelectedItem.Value
        End With
        Me.Filtro = filtro
        Me.TaskAttivi = tasks.GetViewR(Me.Filtro)
        tasks.Dispose()
        Me.TaskGridView.Rebind()
    End Sub

#End Region

#Region "Esportazione"

    Private Sub DetailFile(ByRef sw As StreamWriter, ByVal Task As TaskAttivo)
        Dim a As String = If(Task.Accertamento, "Si", "No")
        Dim i As String = If(Task.Impegno, "Si", "No")
        Dim l As String = If(Task.Liquidazione, "Si", "No")
        Dim line As String = ""
        line &= Task.Documento & vbTab & Task.Proponente & vbTab & Task.TaskCorrente & vbTab & Task.TotaleImporto & vbTab & Task.TotaleImportoI & vbTab & a & vbTab & i & vbTab & l
        sw.WriteLine(line)
    End Sub

    Private Sub HeaderFile(ByRef sw As StreamWriter)
        Dim line As String = String.Empty
        line &= "Documento" & vbTab & "Ufficio Proponente" & vbTab & "Stato" & vbTab & "Totale Importo" & vbTab & "(Totale Importo Specificato)" & vbTab & _
                "Accertamento" & vbTab & "Impegno" & vbTab & "Liquidazione"
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
            .Oggetto = "Elenco Atti Contabili Scrivania Ragioniere"
            .Utente = UtenteCorrente.Username
            .Data = Now
        End With
        esportazioniExcel.Save(exportExcel)
    End Sub

    Private Sub WriteAcc(ByRef sw As StreamWriter, ByVal idt As Integer)
        Dim line As String = ""
        line &= "A C C E R T A M E N T O" & vbTab & "Anno" & vbTab & "Capitolo" & vbTab & "Articolo" & vbTab & "Numero" & vbTab & "Sub-Accertamento" & vbTab & "Importo" & _
                vbTab & "Descrizione"
        sw.WriteLine(line)
        Dim ac As List(Of ParsecAtt.Accertamento) = (New ParsecAtt.DocumentoRepository).GetAccertamenti(idt)
        line = ""
        Dim c As Integer = 1
        For Each ax In ac
            line &= c & vbTab & ax.AnnoEsercizio & vbTab & ax.Capitolo & vbTab & ax.Articolo & vbTab & ax.NumeroAccertamento & vbTab & ax.NumeroSubAccertamento & vbTab & _
                    ax.Importo & vbTab & ax.Note
            sw.WriteLine(line)
            line = ""
            c += 1
        Next
    End Sub

    Private Sub WriteImp(ByRef sw As StreamWriter, ByVal idt As Integer)
        Dim line As String = ""
        line &= "I M P E G N O" & vbTab & "Anno" & vbTab & "Capitolo" & vbTab & "Articolo" & vbTab & "Numero" & vbTab & "Sub-Impegno" & vbTab & "Importo" & vbTab & _
                "Descrizione"
        sw.WriteLine(line)
        Dim im As List(Of ParsecAtt.ImpegnoSpesa) = (New ParsecAtt.DocumentoRepository).GetImpegniSpesa(idt)
        line = ""
        Dim c As Integer = 1
        For Each ix In im
            line &= c & vbTab & ix.AnnoEsercizio & vbTab & ix.Capitolo & vbTab & ix.Articolo & vbTab & ix.NumeroImpegno & vbTab & ix.NumeroSubImpegno & vbTab & _
                    ix.Importo & vbTab & ix.Note
            sw.WriteLine(line)
            line = ""
            c += 1
        Next
    End Sub

    Private Sub WriteLiq(ByRef sw As StreamWriter, ByVal idt As Integer)
        Dim line As String = ""
        line &= "L I Q U I D A Z I O N E" & vbTab & "Anno Impegno" & vbTab & "Capitolo Impegno" & vbTab & "Articolo Impegno" & vbTab & "Numero Impegno" & vbTab & _
                "Numero Sub-Impegno" & vbTab & "Anno Liquidazione" & vbTab & "Numero Liquidazione" & vbTab & "Mandato" & vbTab & "Importo Liquidato" & vbTab & _
                "Descrizione" & vbTab & "Nominativo"
        sw.WriteLine(line)
        Dim li As List(Of ParsecAtt.Liquidazione) = (New ParsecAtt.DocumentoRepository).GetLiquidazioni(idt)
        line = ""
        Dim c As Integer = 1
        For Each lx In li
            line &= c & vbTab & lx.AnnoImpegno & vbTab & lx.Capitolo & vbTab & lx.Articolo & vbTab & lx.NumeroImpegno & vbTab & lx.NumeroSubImpegno & vbTab & _
                    lx.AnnoEsercizio & vbTab & lx.Numero & vbTab & lx.Mandato & vbTab & lx.ImportoLiquidato & vbTab & lx.Note & vbTab & lx.Nominativo
            sw.WriteLine(line)
            line = ""
            c += 1
        Next
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