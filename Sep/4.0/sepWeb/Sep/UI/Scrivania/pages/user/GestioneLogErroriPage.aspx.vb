Imports ParsecAdmin
Imports ParsecPro
Imports Telerik.Web.UI
Imports System.IO
Imports System.Net

Partial Class GestioneLogErroriPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


#Region "PROPRIETA'"

    Public Property LogErrori() As List(Of ParsecAdmin.LogErrore)
        Get
            Return CType(Session("GestioneLogErroriPage_LogErrori"), List(Of ParsecAdmin.LogErrore))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.LogErrore))
            Session("GestioneLogErroriPage_LogErrori") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Gestione Log Errori"

        If Not Me.Page.IsPostBack Then

            Me.LogErrori = Nothing
            'Imposto l'ordinamento predefinito.
            Dim sortExpr As New Telerik.Web.UI.GridSortExpression()
            sortExpr.FieldName = "Data"
            sortExpr.SortOrder = Telerik.Web.UI.GridSortOrder.Descending
            Me.LogErroriGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

            Me.ResettaFiltro()


        End If


        Me.LogErroriGridView.GroupingSettings.CaseSensitive = False

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.ZoneID2.Style.Add("width", widthStyle)
        Me.LogErroriGridView.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloElencoLogLabel.Text = "Elenco Log Errori&nbsp;&nbsp;" & If(Me.LogErroriGridView.Items.Count > 0, "( " & Me.LogErroriGridView.Items.Count.ToString & " )", "")
        'Dim view = Me.FattureElettronicheGridView.MasterTableView.Items.OfType(Of GridDataItem).Where(Function(c) CType(c("SelectCheckBox").Controls(0), CheckBox).Checked = False).Select(Function(c) c.GetDataKeyValue("Id")).ToList
    End Sub

#End Region

#Region "EVENTI GRIGLIA"


    Protected Sub LogErroriGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles LogErroriGridView.NeedDataSource
        If Me.LogErrori Is Nothing Then
            Dim errori As New ParsecAdmin.LogErroriRepository
            Me.LogErrori = errori.GetView(Me.GetFiltro)
            errori.Dispose()
        End If
        Me.LogErroriGridView.DataSource = Me.LogErrori
    End Sub

    Protected Sub LogErroriGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles LogErroriGridView.ItemCommand
        If e.CommandName = "Anteprima" Then
            'Me.VisualizzaFattura(e.Item)
        End If
    End Sub

    Protected Sub LogErroriGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles LogErroriGridView.ItemCreated

        If TypeOf e.Item Is GridFilteringItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop)")
            e.Item.Style.Add("z-index", "99")
            'e.Item.Style.Add("background-color", "White")
        End If

        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If

    End Sub


#End Region

#Region "AZIONI PANNELLO FILTRO"

    Private Sub ResettaFiltro()
        For Each col As GridColumn In Me.LogErroriGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.LogErroriGridView.MasterTableView.FilterExpression = String.Empty
        Me.DataInizioTextBox.SelectedDate = New DateTime(Now.Year, 1, 1)
        Me.DataFineTextBox.SelectedDate = Now
    End Sub


    Private Function GetFiltro() As ParsecAdmin.FiltroLogErrore
        Dim filtro As New ParsecAdmin.FiltroLogErrore
        filtro.DataInizio = Me.DataInizioTextBox.SelectedDate
        filtro.DataFine = Me.DataFineTextBox.SelectedDate
        Return filtro
    End Function


    Private Sub AggiornaGriglia()
        Me.LogErrori = Nothing
        Me.LogErroriGridView.Rebind()
    End Sub

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FiltraImageButton.Click
        Me.AggiornaGriglia()
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.AggiornaGriglia()
    End Sub


#End Region

    Protected Sub ElimnaLogSelezionatiImageButton_Click(sender As Object, e As ImageClickEventArgs) Handles ElimnaLogSelezionatiImageButton.Click
        If Me.LogErroriGridView.SelectedIndexes.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno un log errore!", False)
            Exit Sub
        End If

        Dim sb As New StringBuilder

        Dim errori As New ParsecAdmin.LogErroriRepository

        For i As Integer = 0 To Me.LogErroriGridView.SelectedIndexes.Count - 1

            Dim selectedItem = CType(Me.LogErroriGridView.Items(Me.LogErroriGridView.SelectedIndexes(i)), GridDataItem)
            Try
                Dim idSelezionato As Integer = selectedItem.OwnerTableView.DataKeyValues(selectedItem.ItemIndex)("Id")
                errori.Delete(idSelezionato)
            Catch ex As Exception
                If sb.Length = 0 Then
                    sb.AppendLine("Si sono verificati i seguenti errori durante la cancellazione del log errore")
                End If
                sb.AppendLine(ex.Message)
            End Try
        Next

        errori.Dispose()

        ResettaFiltro()

        If sb.Length = 0 Then
            Me.infoOperazioneHidden.Value = "Eliminazione conclusa con successo!"
        Else
            ParsecUtility.Utility.MessageBox(sb.ToString, False)
        End If

        Me.AggiornaGriglia()

    End Sub
    Protected Sub EsportaInExcelImageButton_Click(sender As Object, e As ImageClickEventArgs) Handles EsportaInExcelImageButton.Click
        Try
            Me.EsportaXls()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub


    Private Sub EsportaXls()

        Dim pathExport As String = String.Empty

        Try
            pathExport = ParsecAdmin.WebConfigSettings.GetKey("PathExport")
        Catch ex As Exception
            Throw New ApplicationException(ex.Message & vbCrLf & "Cartella dell'export non configurata, contattare gli amministratori del sistema!")
        End Try

        Try
            If Not System.IO.Directory.Exists(pathExport) Then
                System.IO.Directory.CreateDirectory(pathExport)
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.Message.Replace("\", "\\"))
        End Try

        If Me.LogErrori Is Nothing Then
            Throw New ApplicationException("Non ci sono log." & vbCrLf & "Impossibile eseguire l'esportazione!")
        End If

        If Me.LogErrori.Count = 0 Then
            Throw New ApplicationException("Non ci sono log." & vbCrLf & "Impossibile eseguire l'esportazione!")
        End If

        '***************************************************************************************
        'CREO IL FILE
        '***************************************************************************************
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("LogErrori_{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy"))
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As New StringBuilder

        line.Append("DESCRIZIONE" & vbTab)
        line.Append("UTENTE" & vbTab)
        line.Append("MODULO" & vbTab)
        line.Append("DATA" & vbTab)
        swExport.WriteLine(line.ToString)
        line.Clear()

        For Each errore As ParsecAdmin.LogErrore In Me.LogErrori

            line.Append(Me.Escape(errore.Descrizione) & vbTab)
            line.Append(Me.Escape(errore.Utente) & vbTab)
            line.Append(Me.Escape(errore.NomeModulo) & vbTab)
            line.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", errore.Data) & vbTab)

            swExport.WriteLine(line.ToString)
            line.Clear()
        Next
        swExport.Close()

        '***************************************************************************************

        '***************************************************************************************
        'VISUALIZZO IL FILE
        '***************************************************************************************
        Session("AttachmentFullName") = fullPathExport
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
        '***************************************************************************************

        '***************************************************************************************
        'SALVO L'ESPORTAZIONE NEL DB
        '***************************************************************************************
        'Dim esportazioniExcel As New ParsecAdmin.ExportExcelRepository
        'Dim exportExcel As ParsecAdmin.ExportExcel = esportazioniExcel.CreateFromInstance(Nothing)
        'exportExcel.NomeFile = exportFilename
        'exportExcel.Oggetto = "Elenco Atti Decisionali"
        'exportExcel.Utente = utente.Username
        'exportExcel.Data = Now
        'esportazioniExcel.Save(exportExcel)
        'esportazioniExcel.Dispose()
        '***************************************************************************************
    End Sub



    'https://www.ietf.org/rfc/rfc4180.txt

    Private Function Escape(ByVal s As String) As String
        Dim res As String = String.Empty

        If Not String.IsNullOrEmpty(s) Then
            s = s.Replace(vbCrLf, " ")
            s = s.Replace(vbTab, "")
            s = s.Replace("""", """""")
            Dim formatString As String = """{0}"""
            res = String.Format(formatString, s)
        End If

        Return res
    End Function

End Class