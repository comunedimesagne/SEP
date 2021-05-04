Imports ParsecAdmin
Imports System.Transactions

Partial Class ManualiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Manuale() As ParsecAdmin.Manuale
        Get
            Return CType(Session("ManualiPage_Manuale"), ParsecAdmin.Manuale)
        End Get
        Set(ByVal value As ParsecAdmin.Manuale)
            Session("ManualiPage_Manuale") = value
        End Set
    End Property

    Public Property Manuali() As List(Of ParsecAdmin.Manuale)
        Get
            Return CType(Session("ManualiPage_Manuali"), List(Of ParsecAdmin.Manuale))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Manuale))
            Session("ManualiPage_Manuali") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Manualistica"

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.ManualiGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il manuale selezionato?", False, Not Me.Manuale Is Nothing)
        Me.TitoloLabel.Text = "Elenco Manuali&nbsp;&nbsp;&nbsp;" & If(Me.Manuali.Count > 0, "( " & Me.Manuali.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim message As String = String.Empty
                Try
                    Me.Save()
                    Me.AggiornaGriglia()
                   
                Catch ex As Exception
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
                    If Not Me.Manuale Is Nothing Then
                        Me.Delete()
                        Me.ResettaVista()
                        Me.AggiornaGriglia()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un manuale!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ManualiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ManualiGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.DownloadFile(e.Item)
        End If
    End Sub

    Protected Sub ManualiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ManualiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona Manuale"
            End If
            Dim manuale As ParsecAdmin.Manuale = CType(e.Item.DataItem, ParsecAdmin.Manuale)
            If String.IsNullOrEmpty(manuale.NomeFile) Then
                dataItem("Preview").Controls(0).Visible = False
            Else
                dataItem("Preview").Controls(0).Visible = True
                If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                    btn = CType(dataItem("Preview").Controls(0), ImageButton)
                    btn.ToolTip = "Apri/Salva Allegato"
                End If
            End If
        End If
    End Sub

    Protected Sub ManualiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ManualiGridView.NeedDataSource
        If Me.Manuali Is Nothing Then
            Dim manuali As New ParsecAdmin.ManualiRepository
            Me.Manuali = manuali.GetView(Nothing)
            manuali.Dispose()
        End If
        Me.ManualiGridView.DataSource = Me.Manuali
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub AggiornaGriglia()
        Me.Manuali = Nothing
        Me.ManualiGridView.Rebind()
    End Sub

    Private Sub Search()
        Dim manuali As New ParsecAdmin.ManualiRepository
        Dim filtro As ParsecAdmin.FiltroManuale = Me.GetFiltro
        Me.Manuali = manuali.GetView(filtro)
        Me.ManualiGridView.Rebind()
        manuali.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FiltroManuale
        Dim filtro As New ParsecAdmin.FiltroManuale
        filtro.Descrizione = Me.DescrizioneTextBox.Text
        Return filtro
    End Function

    Private Sub Print()
        'Dim parametriStampa As New Hashtable
        'parametriStampa.Add("TipologiaStampa", "StampaQualificheOrganigramma")
        'parametriStampa.Add("DatiStampa", Me.Avvisi)
        'Session("ParametriStampa") = parametriStampa
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Dim manuali As New ParsecAdmin.ManualiRepository
        manuali.Delete(Me.Manuale.Id)
        manuali.Dispose()
        'Potrebbe essere condiviso
        Dim pathManuale As String = System.Configuration.ConfigurationManager.AppSettings("PathManuali") & Me.Manuale.NomeFile
        If IO.File.Exists(pathManuale) Then
            IO.File.Delete(pathManuale)
        End If
    End Sub

    Private Sub Save()
        Dim manuali As New ParsecAdmin.ManualiRepository
        Dim manuale As ParsecAdmin.Manuale = manuali.CreateFromInstance(Me.Manuale)

       

        manuale.Descrizione = Me.DescrizioneTextBox.Text
        manuale.Data = Now
        manuale.NomeFile = If(Me.AllegatoUpload.UploadedFiles.Count > 0, Me.AllegatoUpload.UploadedFiles(0).FileName, manuale.NomeFile)

        Try
            manuali.Save(manuale)
            If Me.AllegatoUpload.UploadedFiles.Count > 0 Then
                Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles(0)
                If file.FileName <> "" Then
                    Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathManuali")
                    If (Not IO.Directory.Exists(pathDownload)) Then
                        IO.Directory.CreateDirectory(pathDownload)
                    End If
                    
                    file.SaveAs(pathDownload & file.FileName)
                    manuale.NomeFile = file.FileName
                End If
            End If

            Me.Manuale = manuali.Manuale
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            manuali.Dispose()
        End Try

    End Sub

    Private Sub ResettaVista()
        Me.Manuale = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
        Me.AllegatoTextBox.Text = String.Empty
    End Sub

    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim manuali As New ParsecAdmin.ManualiRepository
        Dim manuale As ParsecAdmin.Manuale = manuali.GetById(id)
        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathManuali")
        Dim file As New IO.FileInfo(pathDownload & manuale.NomeFile)
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
        Dim manuali As New ParsecAdmin.ManualiRepository
        Me.Manuale = manuali.GetById(id)
        Me.DescrizioneTextBox.Text = Me.Manuale.Descrizione
        Me.AllegatoTextBox.Text = Me.Manuale.NomeFile
        manuali.Dispose()
    End Sub

#End Region

End Class