Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class QualifichePage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property QualificaOrganigramma() As ParsecAdmin.QualificaOrganigramma
        Get
            Return CType(Session("QualifichePage_QualificaOrganigramma"), ParsecAdmin.QualificaOrganigramma)
        End Get
        Set(ByVal value As ParsecAdmin.QualificaOrganigramma)
            Session("QualifichePage_QualificaOrganigramma") = value
        End Set
    End Property

    Public Property QualificheOrganigramma() As List(Of ParsecAdmin.QualificaOrganigramma)
        Get
            Return CType(Session("QualifichePage_QualificheOrganigramma"), List(Of ParsecAdmin.QualificaOrganigramma))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.QualificaOrganigramma))
            Session("QualifichePage_QualificheOrganigramma") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Qualifiche Organigramma"

        If Not Me.Page.IsPostBack Then
            Me.QualificheOrganigramma = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Nome"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.QualificheGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

       Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.QualificheGridView.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la qualifica selezionata?", False, Not Me.QualificaOrganigramma Is Nothing)
        Me.TitoloElencoLabel.Text = "Elenco Qualifiche Organigramma&nbsp;&nbsp;&nbsp;" & If(Me.QualificheOrganigramma.Count > 0, "( " & Me.QualificheOrganigramma.Count.ToString & " )", "")

        'SELEZIONO LA RIGA
        If Not Me.QualificaOrganigramma Is Nothing Then
            Dim item As GridDataItem = Me.QualificheGridView.MasterTableView.FindItemByKeyValue("Id", Me.QualificaOrganigramma.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If
        'Try
        '    If Not String.IsNullOrEmpty(Me.selectedRowHidden.Value) Then
        '        Me.QualificheGridView.MasterTableView.Items(CInt(Me.selectedRowHidden.Value)).Selected = True
        '    End If
        'Catch ex As Exception
        'End Try
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
                    If Not Me.QualificaOrganigramma Is Nothing Then
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
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una qualifica!", False)
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

    Protected Sub QualificheGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles QualificheGridView.ItemCommand
        If e.CommandName = "Select" Then
            AggiornaVista(e.Item)
        End If
    End Sub

    Protected Sub QualificheGridView_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles QualificheGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub QualificheGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles QualificheGridView.NeedDataSource
        If Me.QualificheOrganigramma Is Nothing Then
            Dim qualifiche As New ParsecAdmin.QualificaOrganigrammaRepository
            Me.QualificheOrganigramma = qualifiche.GetQuery.ToList
            qualifiche.Dispose()
        End If
        Me.QualificheGridView.DataSource = Me.QualificheOrganigramma
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub Search()
        Dim qualifiche As New ParsecAdmin.QualificaOrganigrammaRepository
        Me.QualificheOrganigramma = qualifiche.GetQuery.Where(Function(c) c.Nome.Contains(Me.DescrizioneTextBox.Text)).ToList
        Me.QualificheGridView.Rebind()
        qualifiche.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.QualificheOrganigramma = Nothing
        Me.QualificheGridView.Rebind()
    End Sub

    Private Sub Print()
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaQualificheOrganigramma")
        parametriStampa.Add("DatiStampa", Me.QualificheOrganigramma)
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Dim qualifiche As New ParsecAdmin.QualificaOrganigrammaRepository
        qualifiche.Delete(Me.QualificaOrganigramma.Id)
        qualifiche.Dispose()
    End Sub

    Private Sub Save()
        Dim qualifiche As New ParsecAdmin.QualificaOrganigrammaRepository
        Dim qualifica As ParsecAdmin.QualificaOrganigramma = qualifiche.CreateFromInstance(Me.QualificaOrganigramma)
        qualifica.Nome = Me.DescrizioneTextBox.Text
        If Me.OrdinaleTextBox.Text <> "" Then
            qualifica.Ordinale = CInt(Me.OrdinaleTextBox.Text)
        End If
        qualifica.Admin = 0
        Try
            qualifiche.Save(qualifica)
            Me.AggiornaVista(qualifiche.QualificaOrganigramma)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            qualifiche.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.QualificaOrganigramma = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
        Me.OrdinaleTextBox.Text = String.Empty
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim qualifiche As New ParsecAdmin.QualificaOrganigrammaRepository
        Dim qualifica = qualifiche.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        Me.AggiornaVista(qualifica)
        qualifiche.Dispose()
    End Sub

    Private Sub AggiornaVista(ByVal qualifica As ParsecAdmin.QualificaOrganigramma)
        Me.ResettaVista()
        Me.DescrizioneTextBox.Text = qualifica.Nome
        If qualifica.Ordinale.HasValue Then
            Me.OrdinaleTextBox.Text = qualifica.Ordinale
        End If
        Me.QualificaOrganigramma = qualifica
    End Sub

#End Region

End Class