Imports ParsecAdmin
Imports ParsecMES
Imports Telerik.Web.UI


Partial Class ViePage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As Object


#Region "PROPRIETA'"

    Public Property Via() As ParsecAdmin.Via
        Get
            Return CType(Session("ViePage_Via"), ParsecAdmin.Via)
        End Get
        Set(ByVal value As ParsecAdmin.Via)
            Session("ViePage_Via") = value
        End Set
    End Property

    Public Property Vie() As List(Of ParsecAdmin.Via)
        Get
            Return CType(Session("ViePage_Vie"), List(Of ParsecAdmin.Via))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Via))
            Session("ViePage_Vie") = value
        End Set
    End Property

    Public Property Filtro() As ParsecAdmin.FiltroVia
        Get
            Return CType(Session("ViePage_Filtro"), ParsecAdmin.FiltroVia)
        End Get
        Set(ByVal value As ParsecAdmin.FiltroVia)
            Session("ViePage_Filtro") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Page.Request("Mode") Is Nothing Then
            MainPage = CType(Me.Master, MainPage)
            MainPage.NomeModulo = "Amministrazione"
            MainPage.DescrizioneProcedura = "> Gestione Vie"
            Me.VieGridView.MasterTableView.GetColumn("ConfirmSelectAndClose").Display = False
            Me.PannelloPulsanti.Visible = False
        Else
            Me.MainPage = CType(Me.Master, BlankPage)
            Me.ChiudiButton.Visible = True
            Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
        End If

     
        If Not Me.Page.IsPostBack Then
            Me.Vie = Nothing
            Me.ResettaVista()
            Me.Filtro = Nothing
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Nome"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.VieGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

            If Not Page.Request("Mode") Is Nothing Then
                Me.GetParametri()
            End If

        End If


        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.PannelloPulsanti.Style.Add("width", widthStyle)
        Me.VieGridView.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BlankPage.master"
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'elemento selezionato?", False, Not Me.Via Is Nothing)
        Me.TitoloLabel.Text = "Elenco Vie&nbsp;&nbsp;&nbsp;" & If(Me.Vie.Count > 0, "( " & Me.Vie.Count.ToString & " )", "")


        'SELEZIONO LA RIGA
        If Not Me.Via Is Nothing Then
            Dim item As GridDataItem = Me.VieGridView.MasterTableView.FindItemByKeyValue("Id", Me.Via.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If
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
                    If Not Me.Via Is Nothing Then
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
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una via!", False)
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

    Protected Sub VieGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles VieGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
            Case "ConfirmSelectAndClose"
                Me.SelezionaVia(e.Item)
        End Select
    End Sub

    Protected Sub VieGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles VieGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub VieGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles VieGridView.NeedDataSource
        If Me.Vie Is Nothing Then
            Dim vie As New ParsecAdmin.VieRepository
            Me.Vie = vie.GetView(Me.Filtro)
            vie.Dispose()
        End If
        Me.VieGridView.DataSource = Me.Vie
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Function GetFiltro() As ParsecAdmin.FiltroVia
        Dim filtro As New ParsecAdmin.FiltroVia
        If Not String.IsNullOrEmpty(Me.DescrizioneTextBox.Text) Then
            filtro.Nome = Me.DescrizioneTextBox.Text
        End If
        Return filtro
    End Function

    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("Filtro") Then
                Me.DescrizioneTextBox.Text = parametriPagina("Filtro")
                Me.Filtro = Me.GetFiltro
            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    Private Sub SelezionaVia(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim vie As New ParsecAdmin.VieRepository
        ParsecUtility.SessionManager.Via = vie.GetById(id)
        vie.Dispose()
        ParsecUtility.Utility.ClosePopup(False)
    End Sub

    Private Sub AggiornaGriglia()
        Me.Vie = Nothing
        Me.VieGridView.Rebind()
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
        Dim vie As New ParsecAdmin.VieRepository
        vie.Delete(Me.Via)
        vie.Dispose()
    End Sub

    Private Sub Search()
        Dim vie As New ParsecAdmin.VieRepository
        Dim filtro = Me.GetFiltro
        Me.Vie = vie.GetView(filtro)
        Me.VieGridView.Rebind()
        vie.Dispose()
    End Sub

    Private Sub Save()
        Dim vie As New ParsecAdmin.VieRepository
        Dim via As ParsecAdmin.Via = vie.CreateFromInstance(Me.Via)
        via.Nome = Trim(Me.DescrizioneTextBox.Text)
        via.Note = Me.NoteTextBox.Text
        Try
            vie.Via = Me.Via
            vie.Save(via)
            Me.Via = vie.Via
            Me.AggiornaVista(Me.Via)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            vie.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.Via = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim vie As New ParsecAdmin.VieRepository
        Dim via = vie.GetById(id)
        Me.AggiornaVista(via)
        vie.Dispose()
    End Sub

    Private Sub AggiornaVista(ByVal via As ParsecAdmin.Via)
        Me.ResettaVista()
        Me.DescrizioneTextBox.Text = via.Nome
        Me.NoteTextBox.Text = via.Note
        Me.Via = via
    End Sub

#End Region

End Class