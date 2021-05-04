Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class ComuniPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Comune() As ParsecAdmin.Comune
        Get
            Return CType(Session("ComuniPage_Comune"), ParsecAdmin.Comune)
        End Get
        Set(ByVal value As ParsecAdmin.Comune)
            Session("ComuniPage_Comune") = value
        End Set
    End Property

    Public Property Comuni() As List(Of ParsecAdmin.Comune)
        Get
            Return CType(Session("ComuniPage_Comuni"), List(Of ParsecAdmin.Comune))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Comune))
            Session("ComuniPage_Comuni") = value
        End Set
    End Property
   
#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Comuni"

        If Not Me.Page.IsPostBack Then
            Me.Comuni = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.ComuniGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.ComuniGridView.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il comune selezionato?", False, Not Me.Comune Is Nothing)
        Me.TitoloElencoLabel.Text = "Elenco Comuni&nbsp;&nbsp;&nbsp;" & If(Me.Comuni.Count > 0, "( " & Me.Comuni.Count.ToString & " )", "")

        'SELEZIONO LA RIGA

        If Not Me.Comune Is Nothing Then
            Dim item As GridDataItem = Me.ComuniGridView.MasterTableView.FindItemByKeyValue("Id", Me.Comune.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
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
                    If Not Me.Comune Is Nothing Then
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
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un comune!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

#End Region


#Region "EVENTI GRIGLIA"

    Protected Sub ComuniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ComuniGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

    Protected Sub ComuniGridView_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles ComuniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub ComuniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ComuniGridView.NeedDataSource
        If Me.Comuni Is Nothing Then
            Dim comuni As New ParsecAdmin.ComuneRepository
            Me.Comuni = comuni.GetView(Nothing)
            comuni.Dispose()
        End If
        Me.ComuniGridView.DataSource = Me.Comuni
    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub Search()
        Dim comuni As New ParsecAdmin.ComuneRepository
        Dim filtro As New ParsecAdmin.FiltroComune
        filtro.Descrizione = Me.DescrizioneTextBox.Text
        Me.Comuni = comuni.GetView(filtro)
        Me.ComuniGridView.Rebind()
        comuni.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.Comuni = Nothing
        Me.ComuniGridView.Rebind()
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
        Dim comuni As New ParsecAdmin.ComuneRepository
        comuni.Delete(Me.Comune.Id)
        comuni.Dispose()
    End Sub

    Private Sub Save()
        Dim comuni As New ParsecAdmin.ComuneRepository
        Dim comune As ParsecAdmin.Comune = comuni.CreateFromInstance(Me.Comune)
        comune.Descrizione = Me.DescrizioneTextBox.Text
        comune.Provincia = Me.ProvinciaTextBox.Text
        comune.Cap = Me.CapTextBox.Text
        comune.CodiceIstat = Me.CodiceIstatTextBox.Text
        Try
            comuni.Save(comune)
            AggiornaVista(comuni.Comune)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            comuni.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.Comune = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
        Me.ProvinciaTextBox.Text = String.Empty
        Me.CapTextBox.Text = String.Empty
        Me.CodiceIstatTextBox.Text = String.Empty
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim comuni As New ParsecAdmin.ComuneRepository
        Dim comune = comuni.GetById(id)
        Me.AggiornaVista(comune)
        comuni.Dispose()
    End Sub

    Private Sub AggiornaVista(ByVal comune As ParsecAdmin.Comune)
        Me.ResettaVista()
        Me.DescrizioneTextBox.Text = comune.Descrizione
        Me.ProvinciaTextBox.Text = comune.Provincia
        Me.CapTextBox.Text = comune.Cap
        Me.CodiceIstatTextBox.Text = comune.CodiceIstat
        Me.Comune = comune
    End Sub


#End Region

End Class