Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class TipologieReferentePage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property TipologiaReferente() As ParsecAdmin.TipologiaReferente
        Get
            Return CType(Session("TipologieReferentePage_TipologiaReferente"), ParsecAdmin.TipologiaReferente)
        End Get
        Set(ByVal value As ParsecAdmin.TipologiaReferente)
            Session("TipologieReferentePage_TipologiaReferente") = value
        End Set
    End Property

    Public Property TipologieReferente() As List(Of ParsecAdmin.TipologiaReferente)
        Get
            Return CType(Session("TipologieReferentePage_TipologieReferente"), List(Of ParsecAdmin.TipologiaReferente))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.TipologiaReferente))
            Session("TipologieReferentePage_TipologieReferente") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Tipologie di Referente"

        If Not Me.Page.IsPostBack Then
            Me.TipologieReferente = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.TipologieReferenteGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.TipologieReferenteGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la tipologia di referente selezionata?", False, Not Me.TipologiaReferente Is Nothing)
        Me.TitoloElencoLabel.Text = "Elenco Tipologie Referente&nbsp;&nbsp;&nbsp;" & If(Me.TipologieReferente.Count > 0, "( " & Me.TipologieReferente.Count.ToString & " )", "")

        'SELEZIONO LA RIGA
        If Not Me.TipologiaReferente Is Nothing Then
            Dim item As GridDataItem = Me.TipologieReferenteGridView.MasterTableView.FindItemByKeyValue("Id", Me.TipologiaReferente.Id)
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
                    If Not Me.TipologiaReferente Is Nothing Then
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
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una tipologia di referente!", False)
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

    Protected Sub TipologieReferenteGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TipologieReferenteGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

    Protected Sub TipologieReferenteGridView_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles TipologieReferenteGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub TipologieReferenteGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles TipologieReferenteGridView.NeedDataSource
        If Me.TipologieReferente Is Nothing Then
            Dim tipologieReferente As New ParsecAdmin.TipologieReferenteRepository
            Me.TipologieReferente = tipologieReferente.GetView(Nothing)
            tipologieReferente.Dispose()
        End If
        Me.TipologieReferenteGridView.DataSource = Me.TipologieReferente
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub Search()
        Dim tipologieReferente As New ParsecAdmin.TipologieReferenteRepository
        Dim filtro As New ParsecAdmin.FiltroTipologiaReferente
        filtro.Descrizione = Me.DescrizioneTextBox.Text
        Me.TipologieReferente = tipologieReferente.GetView(filtro)
        Me.TipologieReferenteGridView.Rebind()
        tipologieReferente.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.TipologieReferente = Nothing
        Me.TipologieReferenteGridView.Rebind()
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
        Dim tipologieReferente As New ParsecAdmin.TipologieReferenteRepository
        tipologieReferente.Delete(Me.TipologiaReferente.Id)
        tipologieReferente.Dispose()
    End Sub

    Private Sub Save()
        Dim tipologieReferente As New ParsecAdmin.TipologieReferenteRepository
        Dim tipoReferente As ParsecAdmin.TipologiaReferente = tipologieReferente.CreateFromInstance(Me.TipologiaReferente)
        tipoReferente.Descrizione = Me.DescrizioneTextBox.Text
        Try
            tipologieReferente.Save(tipoReferente)
            Me.AggiornaVista(tipologieReferente.TipologiaReferente)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            tipologieReferente.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.TipologiaReferente = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim tipologieReferente As New ParsecAdmin.TipologieReferenteRepository
        Dim tipologiaReferente = tipologieReferente.GetById(id)
        Me.AggiornaVista(tipologiaReferente)
        tipologieReferente.Dispose()
    End Sub

    Private Sub AggiornaVista(ByVal tipologiaReferente As ParsecAdmin.TipologiaReferente)
        Me.ResettaVista()
        Me.DescrizioneTextBox.Text = tipologiaReferente.Descrizione
        Me.TipologiaReferente = tipologiaReferente
    End Sub

#End Region

End Class