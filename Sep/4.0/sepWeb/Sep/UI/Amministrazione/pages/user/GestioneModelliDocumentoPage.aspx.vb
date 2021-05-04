Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class GestioneModelliDocumentoPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property ModelloDocumento() As ParsecAdmin.ModelloDocumento
        Get
            Return CType(Session("GestioneModelliDocumentoPage_ModelloDocumento"), ParsecAdmin.ModelloDocumento)
        End Get
        Set(ByVal value As ParsecAdmin.ModelloDocumento)
            Session("GestioneModelliDocumentoPage_ModelloDocumento") = value
        End Set
    End Property

    Public Property ModelliDocumento() As List(Of ParsecAdmin.ModelloDocumento)
        Get
            Return CType(Session("GestioneModelliDocumentoPage_ModelliDocumento"), List(Of ParsecAdmin.ModelloDocumento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.ModelloDocumento))
            Session("GestioneModelliDocumentoPage_ModelliDocumento") = value
        End Set
    End Property


    Public Property Filtro() As ParsecAdmin.ModelloDocumentoFiltro
        Get
            Return CType(Session("GestioneModelliDocumentoPage_Filtro"), ParsecAdmin.ModelloDocumentoFiltro)
        End Get
        Set(ByVal value As ParsecAdmin.ModelloDocumentoFiltro)
            Session("GestioneModelliDocumentoPage_Filtro") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Modelli dei Documenti Generici"

        If Not Me.Page.IsPostBack Then
            Me.ModelliDocumento = Nothing
            Me.ResettaVista()

            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Nome"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.ModelliDocumentoGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

        Me.RegistraParsecOpenOffice()

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)

        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("height", "332px")

        Me.PannelloContenitoreGriglia.Style.Add("width", widthStyle)
        Me.PannelloContenitoreGriglia.Style.Add("height", "300px")
        Me.ModelliDocumentoGridView.Style.Add("width", widthStyle)


    End Sub


    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'elemento selezionato?", False, Not Me.ModelloDocumento Is Nothing)
        If Not Me.ModelliDocumento Is Nothing Then
            Me.TitoloElencoModelliDocumentoLabel.Text = "Elenco Modelli Documento&nbsp;&nbsp;" & If(Me.ModelliDocumento.Count > 0, "( " & Me.ModelliDocumento.Count.ToString & " )", "")
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
                Me.Filtro = Nothing
                Me.ResettaVista()
                Me.AggiornaGriglia()
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.ModelloDocumento Is Nothing Then
                        Dim message As String = String.Empty
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.AggiornaGriglia()
                        Catch ex As ApplicationException
                            message = ex.Message
                        End Try
                        If String.IsNullOrEmpty(message) Then
                            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                        Else
                            ParsecUtility.Utility.MessageBox(message, False)
                        End If

                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un modello di documento!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.ImpostaFiltro()
                Me.AggiornaGriglia()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ModelliDocumentoGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ModelliDocumentoGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.Anteprima(e.Item)
        End If
    End Sub


    Private Sub Anteprima(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
        Dim modello As ParsecAdmin.ModelloDocumento = modelli.GetById(id)
        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathModelliDocumento") & modello.NomeFile
        If Not IO.File.Exists(localPath) Then
            ParsecUtility.Utility.MessageBox("Il file del modello non esiste!", False)
        Else
            Me.VisualizzaDocumento(modello.NomeFile, True)
        End If
    End Sub

    Protected Sub ModelliDocumentoGridView_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles ModelliDocumentoGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub


    Protected Sub ModelliDocumentoGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ModelliDocumentoGridView.NeedDataSource
        If Me.ModelliDocumento Is Nothing Then
            Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
            If Me.Filtro Is Nothing Then
                Dim searchTemplate As ParsecAdmin.ModelloDocumentoFiltro = Me.GetFiltroDefault
                Me.ModelliDocumento = modelli.GetView(searchTemplate)
            Else
                Me.ModelliDocumento = modelli.GetView(Me.Filtro)
                Me.Filtro = Nothing
            End If
            modelli.Dispose()
        End If
        Me.ModelliDocumentoGridView.DataSource = Me.ModelliDocumento
    End Sub


#End Region

#Region "METODI PRIVATI"

    Private Sub Print()
        'Dim parametriStampa As New Hashtable
        'parametriStampa.Add("TipologiaStampa", "StampaQualificheOrganigramma")
        'parametriStampa.Add("DatiStampa", Me.Avvisi)
        'Session("ParametriStampa") = parametriStampa
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
        Try
            modelli.Delete(Me.ModelloDocumento.Id)
            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathModelliDocumento") & Me.ModelloDocumento.NomeFile
            If IO.File.Exists(localPath) Then
                IO.File.Delete(localPath)
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            modelli.Dispose()
        End Try
    End Sub



    'TODO COMPLETARE
    Private Function GetFiltroDefault() As ParsecAdmin.ModelloDocumentoFiltro
        Dim filtro As New ParsecAdmin.ModelloDocumentoFiltro
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        If Not utenteCollegato.SuperUser Then
            filtro.IdUtente = utenteCollegato.Id
        End If
        'If Not String.IsNullOrEmpty(Me.NomeTextBox.Text) Then
        '    filtro.Nome = Me.NomeTextBox.Text.Trim
        'End If
        filtro.Validi = False  ' in base alla modalità di apertura
        Return filtro

    End Function

    Private Sub ImpostaFiltro()
        Me.Filtro = Me.GetFiltroDefault
        If Not String.IsNullOrEmpty(Me.NomeTextBox.Text) Then
            Me.Filtro.Nome = Me.NomeTextBox.Text.Trim
        End If
    End Sub


    Private Sub AggiornaGriglia()
        Me.ModelliDocumento = Nothing
        Me.ModelliDocumentoGridView.Rebind()
    End Sub

    Private Sub Save()
        Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
        Dim modello As ParsecAdmin.ModelloDocumento = modelli.CreateFromInstance(Me.ModelloDocumento)

        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        modello.Nome = Me.NomeTextBox.Text.Trim
        modello.IdUtente = utenteCollegato.Id
        modello.DataInizioValidita = Me.DataInizioValiditaTextBox.SelectedDate
        modello.DataFineValidita = Me.DataFineValiditaTextBox.SelectedDate
        modello.Abilitato = Me.AbilitatoCheckBox.Checked
        modello.VisibilitaPubblica = Me.VisibilitaPubblicaCheckBox.Checked


        Try
            '*******************************************************************
            'Gestione storico non utilizzata.
            '*******************************************************************
            'modelli.ModelloDocumento = Me.ModelloDocumento
            modelli.Save(modello)

            '*******************************************************************
            'Aggiorno l'oggetto corrente
            '*******************************************************************
            AggiornaVista(modelli.ModelloDocumento)

            'Me.ModelloDocumento = modelli.ModelloDocumento

            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathModelliDocumento")
            Dim srcLocalPath As String = localPath & "Empty.odt"
            Dim destLocalPath As String = localPath & Me.ModelloDocumento.NomeFile
            Me.CopiaDocumento(srcLocalPath, destLocalPath)


        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            modelli.Dispose()
        End Try
    End Sub

    Private Sub CopiaDocumento(ByVal input As String, ByVal output As String)
        Try
            If Not IO.File.Exists(output) Then
                IO.File.Copy(input, output)
                If (IO.File.GetAttributes(output) And IO.FileAttributes.ReadOnly) = IO.FileAttributes.ReadOnly Then
                    IO.File.SetAttributes(output, IO.FileAttributes.Normal)
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.ModelloDocumento = Nothing
        Me.NomeTextBox.Text = String.Empty
        Me.DataInizioValiditaTextBox.SelectedDate = Now
        Me.DataFineValiditaTextBox.SelectedDate = Nothing
        Me.VisibilitaPubblicaCheckBox.Checked = False
        Me.AbilitatoCheckBox.Checked = True
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
        Dim modello As ParsecAdmin.ModelloDocumento = modelli.GetById(id)
        modelli.Dispose()
        Me.AggiornaVista(modello)
    End Sub

    Private Sub AggiornaVista(ByVal modello As ParsecAdmin.ModelloDocumento)
        Me.ResettaVista()
        Me.NomeTextBox.Text = modello.Nome
        Me.DataInizioValiditaTextBox.SelectedDate = modello.DataInizioValidita
        Me.DataFineValiditaTextBox.SelectedDate = modello.DataFineValidita
        Me.AbilitatoCheckBox.Checked = modello.Abilitato
        Me.VisibilitaPubblicaCheckBox.Checked = modello.VisibilitaPubblica
        Me.ModelloDocumento = modello
    End Sub

#End Region

#Region "SCRIPT COMPONENTE PARSECOPENOFFICE"


    Private Sub VisualizzaDocumento(ByVal nomeFile As String, ByVal enabled As Boolean)
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeModelliDocumento") & nomeFile
        Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = "ViewDocument"}
        Dim data As String = openofficeParameters.CreateOpenParameter(datiInput)
        ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)
    End Sub



    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region



End Class