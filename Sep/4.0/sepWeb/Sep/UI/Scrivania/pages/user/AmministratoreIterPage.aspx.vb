#Region "Namespaces importati"

Imports ParsecAdmin
Imports ParsecCommon
Imports ParsecUtility
Imports ParsecWKF
Imports System.IO
Imports Telerik.Web.UI

#End Region

Partial Class AmministratoreIterPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property FiltroModello() As ParsecWKF.FiltroModello
        Get
            Return CType(Session("AmministratoreIterPage_FiltroModello"), ParsecWKF.FiltroModello)
        End Get
        Set(ByVal value As ParsecWKF.FiltroModello)
            Session("AmministratoreIterPage_FiltroModello") = value
        End Set
    End Property

    Public Property Modello() As ParsecWKF.Modello
        Get
            Return CType(Session("AmministratoreIterPage_Modello"), ParsecWKF.Modello)
        End Get
        Set(ByVal value As ParsecWKF.Modello)
            Session("AmministratoreIterPage_Modello") = value
        End Set
    End Property

    Public Property Modelli() As List(Of ParsecWKF.Modelli)
        Get
            Return CType(Session("AmministratoreIterPage_Modelli"), List(Of ParsecWKF.Modelli))
        End Get
        Set(ByVal value As List(Of ParsecWKF.Modelli))
            Session("AmministratoreIterPage_Modelli") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Scrivania"
        MainPage.DescrizioneProcedura = "> Amministrazione Scrivania"
        Me.CaricaModuli()
        If Not Me.IsPostBack Then
            Session.Remove("IdModelloWkf")
            Me.DataTB.Text = Format(Today, "dd/MM/yyyy")
            Me.DescrizioneTextBox.Focus()
            Me.Modelli = Nothing
        End If            
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Me.Modelli Is Nothing Then Me.ElencoModelliLabel.Text = "Elenco Modelli " & If(Me.Modelli.Count > 0, "( " & Me.Modelli.Count.ToString & " )", "")
        Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub Abilita(ByVal val As Boolean)
        Me.NuovoModelloIB.Enabled = val
        Me.ModuloCB.Enabled = val
        Me.ModelliCB.Enabled = val
        Me.ModelloUpload.Enabled = val
    End Sub

    Private Sub AggiornaGriglia()
        Me.Modelli = Nothing
        Me.ModelliGridView.Rebind()
    End Sub

    Private Sub AggiornaVista(ByVal e As GridCommandEventArgs)
        Session("IdModelloWkf") = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ID")        
        Me.Carica(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ID"))       
    End Sub

    Private Sub Cancella(ByVal item As GridDataItem)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("ID")
        Dim mods As New ModelliRepository
        Dim modello As Modello = mods.GetById(id)
        If Not modello Is Nothing Then
            Dim message As String = "Il modello ' " & modello.Descrizione & " '  del  ' " & String.Format("{0:dd/MM/yyyy}", modello.DataInserimento) & " '  è stato cancellato con successo!"
            Try
                mods.Delete(modello, utenteCorrente)
                Me.Modelli = Nothing
                Me.ModelliGridView.Rebind()
            Catch ex As Exception
                Utility.MessageBox("Impossibile cancellare il modello selezionato, per il seguente errore:" & vbCrLf & ex.Message, False)
            Finally
                Utility.MessageBox(message, False)
                mods.Dispose()
            End Try
        End If
    End Sub

    Private Sub Carica(ByVal id As Integer)
        Try
            Dim modelli As New ParsecWKF.ModelliRepository
            Dim model As ParsecWKF.Modello = modelli.GetById(id)
            Me.Modello = model
            modelli.Dispose()
            Me.ResettaVista()
            With model
                Me.DescrizioneTextBox.Text = .Descrizione
                Me.ModuloCB.FindItemByValue(.RiferimentoModulo).Selected = True
                CaricaModelli(.RiferimentoModulo)
                Me.ModelliCB.FindItemByValue(.RiferimentoModello).Selected = True
                Me.DataTB.Text = String.Format("{0:dd/MM/yyyy}", .DataInserimento)
                Me.ModelloLnkB.Text = .NomeFile
                Me.NuovoModelloIB.Enabled = False
                Me.modelloUpload1.Visible = False
                Me.modelloUpload2.Visible = True
                Me.DescrizioneTextBox.Focus()
            End With
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    Private Sub CaricaModelli(ByVal mode As Short)
        Me.ModelliCB.Items.Clear()
        Select Case mode
            Case "2"
                Me.ModelliCB.Items.Insert(0, New RadComboBoxItem("Notifica Protocollo", 999))
            Case "3"
                Dim modelli As New ParsecAtt.ModelliRepository
                With Me.ModelliCB
                    .DataValueField = "Id"
                    .DataTextField = "Descrizione"
                    .DataSource = modelli.GetQuery.Where(Function(m) m.Disabilitato = False).OrderBy(Function(m) m.Descrizione).ToList
                    .DataBind()
                End With
                modelli.Dispose()
        End Select
    End Sub

    Private Sub CaricaModuli()
        Dim moduli As New ParsecAdmin.ModuleRepository
        With Me.ModuloCB
            .DataValueField = "Id"
            .DataTextField = "Descrizione"
            .DataSource = moduli.GetQuery.Where(Function(m) m.Abilitato = True).ToList
            .DataBind()
        End With
        moduli.Dispose()
    End Sub

#Region "Esportazione in excel"

    Private Sub DetailModel(ByRef sw As StreamWriter, ByVal ric As Modelli)
        Dim line As String = ""
        With ric
            line &= IIf(.IdModulo = 2, "Protocollo", "Atti amministrativi") & vbTab & .Descrizione & vbTab & Format(.DataInserimento, "dd/MM/yyyy") & vbTab & .Nomefile & vbTab & IIf(.Cancellato, "SI", "  ") & vbTab & .DataCancellazione
        End With
        sw.WriteLine(line)
    End Sub

    Private Sub HeaderFile(ByRef sw As StreamWriter, ByVal Valore As Boolean)
        Dim line As String = String.Empty
        line &= "Modulo SEP" & vbTab & "Nome modello" & vbTab & "Data" & vbTab & "Nome file modello" & vbTab & "Cancellato" & vbTab & "Data Cancellazione"
        sw.WriteLine(line)
    End Sub

    Private Sub SaveStreamWriter(ByVal path As String, ByVal nf As String, ByRef sw As StreamWriter)
        sw.Close()
        Session("AttachmentFullName") = path
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        Utility.PageReload(pageUrl, False)
        Dim esportazioniExcel As New ExportExcelRepository
        Dim exportExcel As ExportExcel = esportazioniExcel.CreateFromInstance(Nothing)
        With exportExcel
            .NomeFile = nf
            .Oggetto = "Elenco Modelli Workflow"
            .Utente = Applicazione.UtenteCorrente.Username
            .Data = Now
        End With
        esportazioniExcel.Save(exportExcel)
    End Sub

    Private Sub WriteDetail(ByRef sw As StreamWriter, ByVal idt As Integer, ByVal idmod As Short)
        Dim line As String = ""
        Select Case idmod
            Case "2"
                line &= "M O D E L L O  C O L L E G A T O" & vbTab & "Nome Modello" & vbTab & "Tipologia Documento" & vbTab & "Data Inizio" & vbTab & "Data Fine" & vbTab & "Disabilitato"
                sw.WriteLine(line)                
                line = ""
                Dim c As Integer = 1
                line &= c & vbTab & "Notifica Protocollo" & vbTab & "Protocollo"
                sw.WriteLine(line)
            Case "3"
                Dim mc As List(Of ParsecWKF.ModelliCollegati) = (New ParsecWKF.ModelliRepository).GetModelliCollegatiModello(idt)
                If mc.Count > 0 Then
                    line &= "M O D E L L O  C O L L E G A T O" & vbTab & "Nome Modello" & vbTab & "Tipologia Documento" & vbTab & "Data Inizio" & vbTab & "Data Fine" & vbTab & "Disabilitato"
                    sw.WriteLine(line)
                End If
                line = ""
                Dim c As Integer = 1
                For Each mx In mc
                    With mx
                        line &= c & vbTab & .Descrizione & vbTab & .Tipo & vbTab & .DataInizio & vbTab & .DataFine & vbTab & .Disabilitato
                    End With
                    sw.WriteLine(line)
                    line = ""
                    c += 1
                Next
        End Select
       
    End Sub

#End Region

    Private Function GetFiltro() As ParsecWKF.FiltroModello
        Dim filtro As New ParsecWKF.FiltroModello
        filtro.Descrizione = Me.DescrizioneTextBox.Text
        Return filtro
    End Function

    Private Sub Preview(ByVal item As GridDataItem)
        ' Da modificare quando sarà richiamabile il componente Designer del WorkFlow Engine
        Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("ModelloWorkflow")
        Dim id As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("ID")
        Dim modelli As New ModelliRepository
        Dim modello = modelli.GetById(id)
        modelli.Dispose()
        If Not modello Is Nothing Then
            Dim pathDownload As String = percorsoRoot & "\" & modello.NomeFile
            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("Il file del modello selezionato NON ESISTE!", False)
            End If
        End If
    End Sub

    Private Sub Recupera(ByVal item As GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("ID")
        Dim mods As New ModelliRepository
        Dim modello As Modello = mods.GetById(id)
        If Not modello Is Nothing Then
            Dim message As String = "Il modello ' " & modello.Descrizione & " '  del  ' " & String.Format("{0:dd/MM/yyyy}", modello.DataInserimento) & " '  è stato recuperato con successo!"
            Try
                mods.Recovery(modello)
                Me.Modelli = Nothing
                Me.ModelliGridView.Rebind()
            Catch ex As Exception
                Utility.MessageBox("Impossibile recuperare il modello selezionato, per il seguente errore:" & vbCrLf & ex.Message, False)
            Finally
                Utility.MessageBox(message, False)
                mods.Dispose()
            End Try
        End If
    End Sub

    Private Sub ResettaVista()
        Me.DescrizioneTextBox.Text = String.Empty
        Me.DataTB.Text = String.Format("{0:dd/MM/yyyy}", Today)
        Me.ModuloCB.ClearSelection()
        Me.ModelliCB.ClearSelection()
        Me.modelloUpload1.Visible = True
        Me.modelloUpload2.Visible = False
    End Sub

    Private Sub Save()
        Dim nuovo As Boolean = Me.Modello Is Nothing
        Dim utenteCollegato As ParsecAdmin.Utente = CType(Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim modelli As New ParsecWKF.ModelliRepository
        Dim modello As ParsecWKF.Modello = modelli.CreateFromInstance(Me.Modello)
        With modello
            .Descrizione = Me.DescrizioneTextBox.Text
            .RiferimentoModulo = CShort(Me.ModuloCB.SelectedValue)
            .RiferimentoModello = CShort(Me.ModelliCB.SelectedValue)
            .DataInserimento = CDate(Me.DataTB.Text)
            If Me.modelloUpload1.Visible Then
                .NomeFile = Me.ModelloUpload.UploadedFiles(0).FileName
            ElseIf Me.modelloUpload2.Visible Then
                .NomeFile = Me.ModelloLnkB.Text
            End If
            .UtenteInserimento = utenteCollegato.Id
        End With
        Try
            modelli.Modello = Me.Modello
            modelli.Save(modello)
            Me.Modello = modelli.Modello
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            modelli.Dispose()
        End Try
    End Sub

    Private Sub Search()
        Dim modelli As New ParsecWKF.ModelliRepository
        Dim filtro As ParsecWKF.FiltroModello = Me.GetFiltro
        Me.Modelli = modelli.GetView1(filtro)
        Me.FiltroModello = filtro
        modelli.Dispose()
        Me.ModelliGridView.Rebind()
    End Sub

    Private Function VerificaDati() As Boolean
        Dim msg As New StringBuilder
        If Me.DescrizioneTextBox.Text.Length = 0 Then
            msg.AppendLine("La descrizione è OBBLIGATORIA!")
        Else
            Dim mods As New ParsecWKF.ModelliRepository
            If Not IsNothing(Session("IdModelloWkf")) Then
                If mods.Esiste(Me.DescrizioneTextBox.Text, Session("IdModelloWkf")) Then msg.AppendLine("E' già presente un modello con la DESCRIZIONE specificata!")
            Else
                If mods.Esiste(Me.DescrizioneTextBox.Text) Then msg.AppendLine("E' già presente un modello con la DESCRIZIONE specificata!")
            End If
            mods.Dispose()
        End If
        If Me.ModuloCB.SelectedValue = "" Then msg.AppendLine("Il modulo è OBBLIGATORIO!")
        If Me.ModelliCB.SelectedValue = "" Then msg.AppendLine("Il modello è OBBLIGATORIO!")
        If Me.modelloUpload1.Visible Then
            If Me.ModelloUpload.UploadedFiles.Count = 0 Then msg.AppendLine("Il file del modello è OBBLIGATORIO")
        ElseIf Me.modelloUpload2.Visible Then
            If Me.ModelloLnkB.Text.Split(".")(1).ToLower <> "xml" Then msg.AppendLine("Tipo di file non GESTIBILE!")
        End If
        If msg.Length > 0 Then
            msg.Insert(0, "E' necessario specificare:" & vbCrLf)
            Utility.MessageBox(msg.ToString, False)
        End If
        Return msg.Length = 0
    End Function

#End Region

#Region "EVENTI GRIGLIA ModelliGridView"

    Protected Sub ModelliGridView_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ModelliGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        Dim btnr As ImageButton = Nothing
        Dim btnc As ImageButton = Nothing
        Dim btns As ImageButton = Nothing
        If TypeOf e.Item Is GridDataItem Then
            Dim rigaModello As ParsecWKF.Modelli = CType(e.Item.DataItem, ParsecWKF.Modelli)
            Dim dataItem As GridDataItem = e.Item
            btnr = CType(dataItem("Recovery").Controls(0), ImageButton)
            If rigaModello.Cancellato Then
                dataItem.ForeColor = Drawing.Color.Red
                If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                    btn = CType(dataItem("Delete").Controls(0), ImageButton)
                    With btn
                        .ImageUrl = "~\images\vuoto.png"
                        .ToolTip = "Modello cancellato da ' " & rigaModello.UtenteCancellazione & " ' il ' " & rigaModello.DataCancellazione & " '"
                        .Attributes.Add("onclick", "return false;")
                    End With
                End If
                btnr.ToolTip &= " ... " & vbCrLf & "Cancellato da ' " & rigaModello.UtenteCancellazione & " - " & rigaModello.DataCancellazione & " '"
                If TypeOf dataItem("Copy").Controls(0) Is ImageButton Then
                    btnc = CType(dataItem("Copy").Controls(0), ImageButton)
                    With btnc
                        .ImageUrl = "~\images\vuoto.png"
                        .Attributes.Add("onclick", "return false;")
                    End With
                End If
                If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                    btns = CType(dataItem("Select").Controls(0), ImageButton)
                    With btns
                        .ImageUrl = "~\images\vuoto.png"
                        .Attributes.Add("onclick", "return false;")
                    End With
                End If
            Else
                btnr.Visible = False
            End If
        End If
    End Sub

    Protected Sub ModelliGridView_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles ModelliGridView.ItemCommand
        Select Case e.CommandName
            Case RadGrid.ExpandCollapseCommandName
                If Not e.Item.Expanded Then
                    Dim parentItem As GridDataItem = CType(e.Item, GridDataItem)
                    Dim innerGrid As RadGrid = CType(parentItem.ChildItem.FindControl("ModelliWkfGridView"), RadGrid)
                    innerGrid.Rebind()
                End If
            Case "Select"
                Me.Abilita(True)
                Me.AggiornaVista(e)
            Case "Copy"
                Me.Abilita(True)
                Me.AggiornaVista(e)
                Me.DataTB.Text = String.Format("{0:dd/MM/yyyy}", Today)
                Me.Modello = Nothing
            Case "Delete"
                Me.Cancella(e.Item)
            Case "Preview"
                Me.Preview(e.Item)
            Case "Recovery"
                Me.Recupera(e.Item)
        End Select
    End Sub

    Protected Sub ModelliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ModelliGridView.NeedDataSource
        Try
            If Me.Modelli Is Nothing Then
                Dim modelr As New ParsecWKF.ModelliRepository
                Me.Modelli = modelr.GetView1(Me.FiltroModello)
                modelr.Dispose()
            End If
            ModelliGridView.DataSource = Me.Modelli
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ModelliGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ModelliGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            With e.Item.Style
                .Add("position", "relative")
                .Add("top", "expression(this.offsetParent.scrollTop-1)")
                .Add("z-index", "99")
                .Add("background-color", "White")
            End With           
        ElseIf TypeOf e.Item Is GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        ElseIf TypeOf e.Item Is GridNestedViewItem Then
            AddHandler CType(e.Item.FindControl("ModelliWkfGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.ModelliWkfGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("ModelliWkfGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.ModelliWkfGridView_ItemCreated)
        End If
    End Sub

#Region "EVENTI GRIGLIA ModelliWkfGridView"

    Protected Sub ModelliWkfGridView_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs)
        Dim parentItem As GridDataItem = CType(CType(CType(sender, RadGrid).NamingContainer, GridNestedViewItem).ParentItem, GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("ID")
        CType(sender, RadGrid).DataSource = (New ParsecWKF.ModelliRepository).GetModelliCollegatiModello(id)
    End Sub

    Protected Sub ModelliWkfGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs)
        If TypeOf e.Item Is GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#End Region

#Region "EVENTI CONTROLLI PAGINA"

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, RadToolBarButton).CommandName
            Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    If Me.VerificaDati Then
                        Me.Save()
                        Me.AggiornaGriglia()
                    Else
                        Exit Sub
                    End If
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)
            Case "Nuovo"
                Me.ResettaVista()
                Me.Abilita(True)
                Me.AggiornaGriglia()
            Case "Annulla"
                Me.FiltroModello = Nothing
                Me.ResettaVista()
                Me.Abilita(False)
                Me.AggiornaGriglia()
            Case "Trova"
                Me.Search()
        End Select
    End Sub

#End Region

    Protected Sub AggiungiModelloIB_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiModelloIB.Click
        If Me.ModelloUpload.UploadedFiles.Count = 0 Then
            Utility.MessageBox("Per collegare un modello, è necessario specificarne il file relativo!", False)
        Else
            If Me.ModelloUpload.UploadedFiles.Count > 0 Then
                Dim file As UploadedFile = Me.ModelloUpload.UploadedFiles(0)
                If file.GetExtension.ToLower = ".xml" Then
                    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("ModelloWorkflow") & "\" & file.FileName
                    file.SaveAs(pathDownload)
                    Me.ModelloLnkB.Text = file.FileName
                    Me.NomeFileModelloLbl.Text = file.FileName
                    Me.modelloUpload1.Visible = False
                    Me.modelloUpload2.Visible = True
                Else
                    Utility.MessageBox("Il file collegato non ha estensione compatibile (.XML)!", False)
                End If
            End If
        End If
    End Sub

    Protected Sub EspModXls_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EspModXls.Click
        If Me.Modelli Is Nothing Then
            Utility.MessageBox("Fase dell'esportazione dei modelli non possibile!", False)
        Else
            If Me.Modelli.Count > 0 Then
                Dim pathExport As String = ConfigurationManager.AppSettings("PathExport")
                If pathExport Is Nothing Then
                    Utility.MessageBox("Cartella dell'export non configurata, contattare gli amministratori del sistema!", False)
                    Exit Sub
                Else
                    If pathExport.Length = 0 Then
                        Utility.MessageBox("Cartella dell'export non configurata, contattare gli amministratori del sistema!", False)
                        Exit Sub
                    Else
                        If Not Directory.Exists(pathExport) Then Directory.CreateDirectory(pathExport)
                    End If
                End If
                Dim exportFilename As String = "ModelliWorkflow_" & Applicazione.UtenteCorrente.Id.ToString & "_" & String.Format("{0:dd_MM_yyyy_HHmmss}", Now) & ".xls"
                Dim fullPathExport As String = pathExport & exportFilename
                Dim swExport As New StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
                HeaderFile(swExport, True)
                For Each mode As Modelli In Me.Modelli
                    DetailModel(swExport, mode)
                    WriteDetail(swExport, mode.Id, mode.IdModulo)
                Next
                SaveStreamWriter(fullPathExport, exportFilename, swExport)
            Else
                Utility.MessageBox("Non ci sono modelli, non si può esportare nulla!", False)
            End If
        End If
    End Sub

    Protected Sub ModelloLnkB_Click(sender As Object, e As System.EventArgs) Handles ModelloLnkB.Click
        ' TODO Applet Designer Workflow
        Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("ModelloWorkflow")
        Dim filename As String = ModelloLnkB.Text
        Dim filenameTemp As String = NomeFileModelloLbl.Text
        If Not filename Is Nothing Then
            Dim file As New IO.FileInfo(percorsoRoot & "\" & filename)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                Utility.PageReload(pageUrl, False)
            Else
                Utility.MessageBox("Il file modello non esiste!", False)
            End If
        End If
    End Sub

    Protected Sub ModuloCB_SIC(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ModuloCB.SelectedIndexChanged
        CaricaModelli(Me.ModuloCB.SelectedValue)
    End Sub

    Protected Sub RimuoviModelloIB_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles RimuoviModelloIB.Click
        Me.ModelloLnkB.Text = String.Empty
        Me.NomeFileModelloLbl.Text = String.Empty
        Me.modelloUpload1.Visible = True
        Me.NuovoModelloIB.Enabled = True
        Me.ModelloUpload.Enabled = True
        Me.modelloUpload2.Visible = False
    End Sub

#End Region

End Class