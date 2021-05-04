Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class AvvisiPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Avviso() As ParsecAdmin.Avviso
        Get
            Return CType(Session(CStr(ViewState("Avviso_Ticks"))), ParsecAdmin.Avviso)
        End Get
        Set(ByVal value As ParsecAdmin.Avviso)
            If ViewState("Avviso_Ticks") Is Nothing Then
                ViewState("Avviso_Ticks") = "Avviso_" & Now.Ticks.ToString
            End If
            Session(CStr(ViewState("Avviso_Ticks"))) = value
        End Set
    End Property

    Public Property Avvisi() As List(Of ParsecAdmin.Avviso)
        Get
            Return CType(Session(CStr(ViewState("Avvisi_Ticks"))), Object)
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Avviso))
            If ViewState("Avvisi_Ticks") Is Nothing Then
                ViewState("Avvisi_Ticks") = "Avvisi_" & Now.Ticks.ToString
            End If
            Session(CStr(ViewState("Avvisi_Ticks"))) = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Avvisi"
        If Not Page.IsPostBack Then
            Me.ResettaVista()
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.AvvisiGridView.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'avviso selezionato?", False, Not Me.Avviso Is Nothing)
        If Not Me.Avvisi Is Nothing Then
            Me.TitoloElencoAvvisiLabel.Text = "Elenco Avvisi&nbsp;&nbsp;&nbsp;" & If(Me.Avvisi.Count > 0, "( " & Me.Avvisi.Count.ToString & " )", "")
        End If
    End Sub

#End Region


#Region "EVENTI TOOLBAR"

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

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
                    If Not Me.Avviso Is Nothing Then
                        Dim message As String = String.Empty
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.AggiornaGriglia()
                        Catch ex As Exception
                            message = ex.Message
                        End Try
                        If String.IsNullOrEmpty(message) Then
                            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                        Else
                            ParsecUtility.Utility.MessageBox(message, False)
                        End If
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un'avviso!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub AvvisiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AvvisiGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.DownloadFile(e.Item)
        End If
        If e.CommandName = "Show" Then
            Me.ImpostaVisualizzaAvviso(e.Item, True)
        End If
        If e.CommandName = "Hide" Then
            Me.ImpostaVisualizzaAvviso(e.Item, False)
        End If

    End Sub

    Protected Sub AvvisiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles AvvisiGridView.NeedDataSource
        If Me.Avvisi Is Nothing Then
            Dim avvisi As New ParsecAdmin.AvvisiRepository
            Me.Avvisi = avvisi.GetView(Nothing)
            avvisi.Dispose()
        End If
        Me.AvvisiGridView.DataSource = Me.Avvisi
    End Sub

    Protected Sub AvvisiGridView_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles AvvisiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub AvvisiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AvvisiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona avviso"
            End If
            Dim avviso As ParsecAdmin.Avviso = CType(e.Item.DataItem, ParsecAdmin.Avviso)
            If String.IsNullOrEmpty(avviso.NomeFile) Then
                dataItem("Preview").Controls(0).Visible = False
            Else
                dataItem("Preview").Controls(0).Visible = True
                If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                    btn = CType(dataItem("Preview").Controls(0), ImageButton)
                    btn.ToolTip = "Apri/Salva allegato"
                End If
            End If

            btn = CType(dataItem("Show").Controls(0), ImageButton)
            If avviso.Visibile.HasValue Then
                If avviso.Visibile Then
                    btn.ImageUrl = "~\images\EliminaAvviso16.png"
                    btn.ToolTip = "Nascondi avviso"
                    btn.CommandName = "Hide"
                Else
                    btn.ImageUrl = "~\images\AggiungiAvviso16.png"
                    btn.ToolTip = "Visualizza avviso"
                    btn.CommandName = "Show"
                End If
            Else
                btn.ImageUrl = "~\images\EliminaAvviso16.png"
                btn.ToolTip = "Nascondi avviso"
                btn.CommandName = "Hide"
            End If
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub Search()
        Dim avvisi As New ParsecAdmin.AvvisiRepository
        Dim filtro As New ParsecAdmin.FiltroAvviso
        avviso.Contenuto = Me.ContenutoTextBox.Text
        Me.Avvisi = avvisi.GetView(filtro)
        Me.AvvisiGridView.Rebind()
        avvisi.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.Avvisi = Nothing
        Me.AvvisiGridView.Rebind()
    End Sub

    Private Sub Print()
        'Dim parametriStampa As New Hashtable
        'parametriStampa.Add("TipologiaStampa", "StampaQualificheOrganigramma")
        'parametriStampa.Add("DatiStampa", Me.Avvisi)
        'Session("ParametriStampa") = parametriStampa
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Dim avvisi As New ParsecAdmin.AvvisiRepository
        avvisi.Delete(Me.Avviso.Id)
        avvisi.Dispose()

        Dim avvisiLetti As New ParsecAdmin.AvvisiIndirizziIPRepository
        avvisiLetti.DeleteAll(Me.Avviso.Id)
        avvisiLetti.Dispose()

    End Sub

    Private Sub Save()
        Dim avvisi As New ParsecAdmin.AvvisiRepository
        Dim avviso As ParsecAdmin.Avviso = avvisi.CreateFromInstance(Me.Avviso)
        avviso.Contenuto = Me.ContenutoTextBox.Text
        If Me.DataAvvisoDatePicker.SelectedDate.HasValue Then
            avviso.Data = Me.DataAvvisoDatePicker.SelectedDate
        End If
        If Me.DataScadenzaDatePicker.SelectedDate.HasValue Then
            avviso.DataScadenza = Me.DataScadenzaDatePicker.SelectedDate
        End If
        avviso.Visibile = True
        If Me.AllegatoUpload.UploadedFiles.Count > 0 Then
            Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles(0)
            If file.FileName <> "" Then
                Dim pathAvviso As String = System.Configuration.ConfigurationManager.AppSettings("PathAvviso")
                file.SaveAs(pathAvviso & file.FileName)
                avviso.NomeFile = file.FileName
            End If
        End If
        Try
            avvisi.Save(avviso)
            Me.Avviso = avvisi.Avviso
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            avvisi.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.Avviso = Nothing
        Me.ContenutoTextBox.Text = ""
        Me.DataAvvisoDatePicker.SelectedDate = Now
        Me.DataScadenzaDatePicker.SelectedDate = Now.AddDays(31)
        Me.AllegatoTextBox.Text = String.Empty
    End Sub

    Private Sub ImpostaVisualizzaAvviso(ByVal item As Telerik.Web.UI.GridDataItem, ByVal visibile As Boolean)
        Dim message As String = "La pubblicazione dell'avviso è stata aggiunta con successo!"
        Try
            If Not visibile Then
                message = "La pubblicazione dell'avviso è stata eliminata con successo!"
            End If
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim avvisi As New ParsecAdmin.AvvisiRepository
            Dim avviso As ParsecAdmin.Avviso = avvisi.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
            avviso.Visibile = visibile
            avvisi.SaveChanges()
            avvisi.Dispose()
            Me.Avvisi = Nothing
            Me.AvvisiGridView.Rebind()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        Finally
            ParsecUtility.Utility.MessageBox(message, False)
        End Try

    End Sub

    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim avvisi As New ParsecAdmin.AvvisiRepository
        Dim avviso As ParsecAdmin.Avviso = avvisi.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        Dim pathAvviso As String = System.Configuration.ConfigurationManager.AppSettings("PathAvviso")
        Dim file As New IO.FileInfo(pathAvviso & avviso.NomeFile)
        If file.Exists Then
            Session("AttachmentFullName") = file.FullName
            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
            ParsecUtility.Utility.PageReload(pageUrl, False)
        Else
            ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
        End If
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim avvisi As New ParsecAdmin.AvvisiRepository
        Me.Avviso = avvisi.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        Me.ContenutoTextBox.Text = Me.Avviso.Contenuto
        Me.AllegatoTextBox.Text = Me.Avviso.NomeFile
        If Not Me.Avviso.Data.Equals(Nothing) Then
            Me.DataAvvisoDatePicker.SelectedDate = CDate(String.Format("{0:dd/MM/yyyy}", Me.Avviso.Data))
        End If
        avvisi.Dispose()
    End Sub

#End Region


End Class