Imports ParsecAtt
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneRegistriPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property TipologiaRegistro() As ParsecAtt.TipologiaRegistro
        Get
            Return CType(Session("GestioneRegistriPage_TipologiaRegistro"), ParsecAtt.TipologiaRegistro)
        End Get
        Set(ByVal value As ParsecAtt.TipologiaRegistro)
            Session("GestioneRegistriPage_TipologiaRegistro") = value
        End Set
    End Property

    Public Property TipologieRegistro() As List(Of ParsecAtt.TipologiaRegistro)
        Get
            Return CType(Session("GestioneRegistriPage_TipologieRegistro"), List(Of ParsecAtt.TipologiaRegistro))
        End Get
        Set(ByVal value As List(Of ParsecAtt.TipologiaRegistro))
            Session("GestioneRegistriPage_TipologieRegistro") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Gestione Registri"

        Me.CaricaTipologieDocumento()

     

        If Not Me.Page.IsPostBack Then
            Me.TipologieRegistro = Nothing
            Me.ResettaVista()

            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.RegistriGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

        End If
        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.RegistriGridView.Style.Add("width", widthStyle)


    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la tipologia di registro selezionata?", False, Not Me.TipologiaRegistro Is Nothing)

        If Not Me.TipologieRegistro Is Nothing Then
            Me.TitoloElencoRegistriLabel.Text = "Elenco Tipologie di Registro&nbsp;&nbsp;&nbsp;" & If(Me.TipologieRegistro.Count > 0, "( " & Me.TipologieRegistro.Count.ToString & " )", "")
        End If

      
        Me.RadToolBar.Items.FindItemByText("Elimina").Enabled = False

    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName

            Case "Stampa"
                ' Me.Print()
            Case "Salva"
                Dim message As String = String.Empty
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
                    If Not Me.TipologiaRegistro Is Nothing Then
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.AggiornaGriglia()
                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)
                        End Try
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una tipologia di registro!", False)
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

    Protected Sub RegistriGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RegistriGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
         End Select
    End Sub

    Protected Sub RegistriGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RegistriGridView.NeedDataSource
        If Me.TipologieRegistro Is Nothing Then
            Dim tipologie As New ParsecAtt.TipologieRegistroRepository
            Me.TipologieRegistro = tipologie.GetView(Nothing)
            tipologie.Dispose()
        End If
        Me.RegistriGridView.DataSource = Me.TipologieRegistro
    End Sub

    Protected Sub RegistriGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RegistriGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder

        If Me.TipologieDocumentoComboBox.SelectedIndex = 0 Then
            message.AppendLine("La tipologia di documento a cui fa riferimento il registro.")
        End If

        If String.IsNullOrEmpty(Me.DescrizioneTextBox.Text) Then
            message.AppendLine("La descrizione del registro.")
        End If

        If Not Me.GeneraleCheckBox.Checked AndAlso Not Me.SettoreCheckBox.Checked Then
            message.AppendLine("La tipologia di contatore.")
        End If

        If message.Length > 0 Then
            message.Insert(0, "E' necessario specificare:" & vbCrLf)
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If

        Return message.Length = 0

    End Function

    Private Sub CaricaTipologieDocumento()
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Me.TipologieDocumentoComboBox.DataValueField = "Id"
        Me.TipologieDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologieDocumentoComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaDocumento With {.Modellizzabile = True})
        Me.TipologieDocumentoComboBox.DataBind()
        Me.TipologieDocumentoComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieDocumentoComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.TipologieRegistro = Nothing
        Me.RegistriGridView.Rebind()
    End Sub

    Private Sub Search()
        Dim tipologie As New ParsecAtt.TipologieRegistroRepository
        Dim filtro As ParsecAtt.FiltroTipologiaRegistro = Me.GetFiltro
        Me.TipologieRegistro = tipologie.GetView(filtro)
        tipologie.Dispose()
        Me.RegistriGridView.Rebind()
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()

        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository

        Dim tipologiaRegistro As ParsecAtt.TipologiaRegistro = tipologieRegistro.GetById(id)

        Me.DescrizioneTextBox.Text = tipologiaRegistro.Descrizione

        If tipologiaRegistro.IdTipologiaRegistro.HasValue Then
            Select Case CType(tipologiaRegistro.IdTipologiaRegistro, ParsecAtt.TipoRegistro)
                Case ParsecAtt.TipoRegistro.Settore
                    Me.SettoreCheckBox.Checked = True
                Case ParsecAtt.TipoRegistro.Generale
                    Me.GeneraleCheckBox.Checked = True
                Case ParsecAtt.TipoRegistro.GeneraleSettore
                    Me.SettoreCheckBox.Checked = True
                    Me.GeneraleCheckBox.Checked = True
            End Select
        End If

        If tipologiaRegistro.VisibileInIter.HasValue Then
            Me.VisibileIterCheckBox.Checked = tipologiaRegistro.VisibileInIter
        End If
        If tipologiaRegistro.Disattivato.HasValue Then
            Me.DisattivatoCheckBox.Checked = tipologiaRegistro.Disattivato
        End If

        If tipologiaRegistro.IdTipologiaDocumento.HasValue Then
            Me.TipologieDocumentoComboBox.FindItemByValue(tipologiaRegistro.IdTipologiaDocumento).Selected = True
        End If

        Me.TipologiaRegistro = tipologiaRegistro

        tipologieRegistro.Dispose()
    End Sub

    Private Sub Delete()
        'Dim firme As New ParsecAtt.FirmeRepository
        'Try
        '    firme.Delete(Me.Firma)
        'Catch ex As Exception
        '    Throw New ApplicationException(ex.Message)
        'Finally
        '    firme.Dispose()
        'End Try
    End Sub

    Private Sub Save()

        Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository
        Dim tipologiaRegistro As ParsecAtt.TipologiaRegistro = tipologieRegistro.CreateFromInstance(Me.TipologiaRegistro)

        'Aggiorno il modello
        tipologiaRegistro.Descrizione = Me.DescrizioneTextBox.Text.Trim
        tipologiaRegistro.VisibileInIter = Me.VisibileIterCheckBox.Checked
        tipologiaRegistro.Disattivato = Me.DisattivatoCheckBox.Checked

        If Me.TipologieDocumentoComboBox.SelectedIndex <> 0 Then
            tipologiaRegistro.IdTipologiaDocumento = CInt(Me.TipologieDocumentoComboBox.SelectedValue)
        End If

        If Me.SettoreCheckBox.Checked And Me.GeneraleCheckBox.Checked = True Then
            tipologiaRegistro.IdTipologiaRegistro = CInt(ParsecAtt.TipoRegistro.GeneraleSettore)
        Else
            If Me.SettoreCheckBox.Checked Then
                tipologiaRegistro.IdTipologiaRegistro = CInt(ParsecAtt.TipoRegistro.Settore)
            ElseIf Me.GeneraleCheckBox.Checked Then
                tipologiaRegistro.IdTipologiaRegistro = CInt(ParsecAtt.TipoRegistro.Generale)
            End If
        End If


        Try
            tipologieRegistro.Save(tipologiaRegistro)
            Me.TipologiaRegistro = tipologieRegistro.TipologiaRegistro
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            tipologieRegistro.Dispose()
        End Try

    End Sub

    Private Sub ResettaVista()
        Me.DescrizioneTextBox.Text = String.Empty
        Me.VisibileIterCheckBox.Checked = False
        Me.DisattivatoCheckBox.Checked = False
        Me.GeneraleCheckBox.Checked = False
        Me.SettoreCheckBox.Checked = False
        Me.TipologieDocumentoComboBox.SelectedIndex = 0
        Me.TipologiaRegistro = Nothing
    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroTipologiaRegistro
        Dim filtro As New ParsecAtt.FiltroTipologiaRegistro
        filtro.Descrizione = Me.DescrizioneTextBox.Text
        Return filtro
    End Function

#End Region

End Class