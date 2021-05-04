Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI




Partial Class GestioneCentriResponsabilitaPage
    Inherits System.Web.UI.Page


    Public Class CentroResponsabilita
        Public Property Codice As String
        Public Property Descrizione As String
        Public Property Responsabile As String

    End Class


    Private WithEvents MainPage As MainPage


#Region "PROPRIETA'"

    Private Property CentriResponsabilita() As List(Of CentroResponsabilita)
        Get
            Return CType(Session("GestioneCentriResponsabilitaPage_CentriResponsabilita"), List(Of CentroResponsabilita))
        End Get
        Set(ByVal value As List(Of CentroResponsabilita))
            Session("GestioneCentriResponsabilitaPage_CentriResponsabilita") = value
        End Set
    End Property


    Private Property Utenti() As List(Of ParsecAdmin.UtenteCentroResponsabilita)
        Get
            Return CType(Session("GestioneCentriResponsabilitaPage_Utenti"), List(Of ParsecAdmin.UtenteCentroResponsabilita))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.UtenteCentroResponsabilita))
            Session("GestioneCentriResponsabilitaPage_Utenti") = value
        End Set
    End Property



    Private Property Filtered As Boolean
        Get
            Return CBool(ViewState("Filtered"))
        End Get
        Set(value As Boolean)
            ViewState("Filtered") = value
        End Set
    End Property


    Public ReadOnly Property TipologiaGestioneContabilita As ParsecAtt.TipologiaGestioneContabilita
        Get
            If Session("GestioneCentriResponsabilitaPag_TipologiaGestioneContabilita") Is Nothing Then
                Dim parametri As New ParsecAdmin.ParametriRepository
                Dim parametro = parametri.GetByName("TipologiaGestioneContabilita")
                parametri.Dispose()

                Dim tipologia As ParsecAtt.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.Classica
                If Not parametro Is Nothing Then
                    Try
                        tipologia = CType(parametro.Valore, ParsecAtt.TipologiaGestioneContabilita)
                    Catch ex As Exception
                    End Try
                End If
                Session("GestioneCentriResponsabilitaPag_TipologiaGestioneContabilita") = tipologia
            End If
            Return Session("GestioneCentriResponsabilitaPag_TipologiaGestioneContabilita")
        End Get
    End Property



#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Gestione Centri Responsabilità"
        If Not Me.Page.IsPostBack Then


            Me.CentriResponsabilita = Nothing

            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.CentriResponsabilitaGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
            Me.Filtered = False
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        'Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGrigliaUtenti.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.CentriResponsabilitaGridView.Style.Add("width", widthStyle)


        Me.CentriResponsabilitaGridView.GroupingSettings.CaseSensitive = False

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        ''SELEZIONO LA RIGA
        If Not String.IsNullOrEmpty(Me.CodiceStrutturaHidden.Value) Then
            Dim item As GridDataItem = Me.CentriResponsabilitaGridView.MasterTableView.FindItemByKeyValue("Codice", Me.CodiceStrutturaHidden.Value)
            If Not item Is Nothing Then
                item.Selected = True
                If Me.Filtered Then
                    Me.AggiornaVista(CInt(Me.CodiceStrutturaHidden.Value))
                    Me.Filtered = False
                End If

            Else
                Me.Utenti = New List(Of ParsecAdmin.UtenteCentroResponsabilita)
                Me.UtentiGridView.Rebind()
            End If
        End If

       
        Me.TitoloElencoUtentiLabel.Text = "Elenco Utenti&nbsp;&nbsp;&nbsp;" & If(Me.Utenti.Count > 0, "( " & Me.Utenti.Count.ToString & " )", "")

        If Me.Utenti.Count > 0 Then
            Dim message As String = "Eliminare tutti gli elementi selezionati?"
            Me.EliminaUtentiSelezionatiImageButton.Attributes.Add("onclick", "return ConfirmDeleteUtenti(""" & message & """)")
        End If

        'Dim checkedRowCount = Me.UtentiGridView.MasterTableView.Items.OfType(Of GridDataItem).Where(Function(c) CType(c("SelectCheckBox").Controls(0), CheckBox).Checked = True).Count
        'Me.EliminaUtentiSelezionatiImageButton.Enabled = checkedRowCount > 0




    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
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
                'Me.ResettaVista()
                'Me.AggiornaGriglia()
                'Me.UtentiGridView.Rebind()

            Case "Annulla"
                Me.ResettaVista()

                'DESELEZIONO LA RIGA
                Me.CentriResponsabilitaGridView.SelectedIndexes.Clear()

                'RESETTO I FILTRI
                For Each column As GridColumn In Me.CentriResponsabilitaGridView.MasterTableView.RenderColumns
                    If column.SupportsFiltering Then
                        column.CurrentFilterValue = String.Empty
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter
                    End If
                Next
                Me.CentriResponsabilitaGridView.MasterTableView.FilterExpression = String.Empty

                Me.AggiornaGriglia()

                Me.UtentiGridView.Rebind()
            Case "Elimina"
                'If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                '    If Not Me.CentroResponsabilita Is Nothing Then
                '        Dim message As String = String.Empty
                '        Try
                '            Me.Delete()
                '            Me.ResettaVista()
                '            Me.AggiornaGriglia()
                '            Me.UtentiGridView.Rebind()
                '        Catch ex As Exception
                '            message = ex.Message
                '        End Try
                '        If String.IsNullOrEmpty(message) Then
                '            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                '        Else
                '            ParsecUtility.Utility.MessageBox(message, False)
                '        End If
                '    Else
                '        ParsecUtility.Utility.MessageBox("E' necessario selezionare un gruppo!", False)
                '    End If
                'End If
                'CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                'Me.Search()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        'If e.Item.Text = "Elimina" Then
        '    e.Item.Attributes.Add("onclick", "return Confirm();")
        'End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub CentriResponsabilitaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles CentriResponsabilitaGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
            Case RadGrid.FilterCommandName
                Me.Filtered = True
        End Select
    End Sub

    Protected Sub CentriResponsabilitaGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles CentriResponsabilitaGridView.NeedDataSource


        If Me.CentriResponsabilita Is Nothing Then
            Dim anno As Integer = Me.GetAnnoEsercizio
            If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then

                Try
                    Dim endPoint As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupBaseUrlRagioneria")
                    Dim scope As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupScopeRagioneria")
                    Dim accessTokenUri As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupAccessTokenUriRagioneria")
                    Dim clientId As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientIdRagioneria")
                    Dim clientSecret As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientSecretRagioneria")
                    Dim parameter As New ParsecWebServices.OAuth2Parameter With {.AccessTokenUri = accessTokenUri, .ClientId = clientId, .ClientSecret = clientSecret, .Scope = scope}
                    Dim service As New ParsecWebServices.DedaGroupService(String.Format(endPoint, "ElencoCentriResponsabilita"), parameter)

                    Dim postParameters As New Dictionary(Of String, Object)
                    postParameters.Add("Anno", anno)

                    Dim centri = service.QueryCentriResponsabilita(postParameters)

                    Dim view = From centro In centri
                               Select New CentroResponsabilita With {
                                   .Codice = centro.Codice,
                                   .Descrizione = centro.Descrizione
                                   }


                    Me.CentriResponsabilita = view.ToList


                Catch ex As Exception


                End Try


            Else
                Dim centri As New ParsecPostgres.CentriResponsabilita(anno)

                Dim view = From centro In centri.ToList
                           Select New CentroResponsabilita With {
                                   .Codice = centro.Codice,
                                   .Descrizione = centro.Descrizione,
                                   .Responsabile = centro.Responsabile
                                   }

                Me.CentriResponsabilita = view.ToList
            End If

        End If
        Me.CentriResponsabilitaGridView.DataSource = Me.CentriResponsabilita
    End Sub

    Protected Sub CentriResponsabilitaGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles CentriResponsabilitaGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub CentriResponsabilitaGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles CentriResponsabilitaGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloCentriResponsabilitaLabel.Text = "Elenco Centri Responsabilità&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If
    End Sub

    
#End Region

#Region "METODI PRIVATI"

    Private Function GetAnnoEsercizio() As Integer
        Dim annoEsercizio As Integer = Now.Year
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AnnoCorrente", ParsecAdmin.TipoModulo.ATT)
        If Not parametro Is Nothing Then
            annoEsercizio = CInt(parametro.Valore)
        End If
        parametri.Dispose()
        Return annoEsercizio
    End Function

    Private Sub AggiornaGriglia()
        Me.CentriResponsabilita = Nothing
        Me.CentriResponsabilitaGridView.Rebind()
    End Sub


    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim codice As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Codice")
        Me.AggiornaVista(codice)
    End Sub

    Private Sub AggiornaVista(ByVal codice As String)
        Me.ResettaVista()

        Me.CodiceStrutturaHidden.Value = codice

        '************************************************************************************************
        'CARICO GLI UTENTI ASSOCIATI AL CENTRO DI RESPONSABILITA'
        '************************************************************************************************

        Dim utentiCentroResponsabilita As New UtenteCentroResponsabilitaRepository
        Me.Utenti = utentiCentroResponsabilita.GetView(codice)
        utentiCentroResponsabilita.Dispose()
        Me.UtentiGridView.Rebind()
        '************************************************************************************************

    End Sub

    Private Sub Delete()
        'Niente
    End Sub



    Private Sub Save()

        If Me.CentriResponsabilitaGridView.SelectedItems.Count = 0 Then
            Throw New ApplicationException("E' necessario selezionare un centro di responsabilità!")
        End If


        If Me.Utenti.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario aggiungere almeno un utente al centro di responsabilità!", False)
            Exit Sub
        End If

        Dim utentiCentro As New ParsecAdmin.UtenteCentroResponsabilitaRepository
        Try
            'CANCELLO TUTTI GLI UTENTI ASSOCIATI AL CENTRO DI RESPONSABILITA' CORRENTE

            Dim codice As String = Me.CodiceStrutturaHidden.Value

            utentiCentro.DeleteAll(codice)

            'ASSOCIO TUTTI GLI UTENTI CORRENTI AL CENTRO DI RESPONSABILITA'

            utentiCentro.SaveAll(codice, Me.Utenti)


        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            utentiCentro.Dispose()
        End Try

    End Sub

    Private Sub ResettaVista()
        Me.CodiceStrutturaHidden.Value = String.Empty
        Me.Utenti = New List(Of ParsecAdmin.UtenteCentroResponsabilita)
        Me.rowSelectedHidden.Value = "0"
    End Sub



#End Region



#Region "GESTIONE UTENTI"

    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click

        If Me.CentriResponsabilitaGridView.SelectedItems.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un centro di responsabilità!", False)
            Exit Sub
        End If

        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim utente As ParsecAdmin.UtenteCentroResponsabilita = Nothing
            Dim idUtente As Integer = 0
            For Each utenteSelezionato In utentiSelezionati
                idUtente = utenteSelezionato.Key
                Dim esiste As Boolean = Not Me.Utenti.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault Is Nothing
                If Not esiste Then
                    utente = New ParsecAdmin.UtenteCentroResponsabilita
                    utente.IdUtente = idUtente
                    utente.DescrizioneUtente = utenteSelezionato.Value
                    Me.Utenti.Add(utente)
                End If
            Next
            Me.UtentiGridView.Rebind()
            Session("SelectedUsers") = Nothing
        End If
    End Sub

    Protected Sub UtentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UtentiGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaUtente(e.Item)
        End If
    End Sub

    Private Sub EliminaUtente(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim IdUtente As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdUtente")
        Dim utente As ParsecAdmin.UtenteCentroResponsabilita = Me.Utenti.Where(Function(c) c.IdUtente = IdUtente).FirstOrDefault
        If Not utente Is Nothing Then
            Me.Utenti.Remove(utente)
        End If
        Me.UtentiGridView.Rebind()
    End Sub


    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource
        Me.UtentiGridView.DataSource = Me.Utenti
    End Sub

    Private Sub UtentiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiGridView.ItemCreated
         If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub UtentiGridView_PreRender(sender As Object, e As System.EventArgs) Handles UtentiGridView.PreRender
        Dim gridHeaderItems = Me.UtentiGridView.MasterTableView.GetItems(GridItemType.Header)
        Dim selectAllCheckBox As CheckBox = CType(CType(gridHeaderItems(0), GridHeaderItem)("SelectCheckBox").Controls(0), CheckBox)
        'DISABILITO LA CHECKBOX SELEZIONA TUTTO SE NON C'E' NIENTE DA SELEZIONARE
        If Me.UtentiGridView.Items.Count = 0 Then
            selectAllCheckBox.Enabled = False
        End If
        'selectAllCheckBox.Attributes.Add("onchange", "IsChecked(this);")
    End Sub

   
    Protected Sub EliminaUtentiSelezionatiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtentiSelezionatiImageButton.Click

        If Me.CentriResponsabilitaGridView.SelectedItems.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un centro di responsabilità!", False)
            Exit Sub
        End If


        If Me.UtentiGridView.SelectedIndexes.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno un elemento!", False)
            Exit Sub
        End If

        Dim selectedItem As GridDataItem = Nothing
        For i As Integer = 0 To Me.UtentiGridView.SelectedIndexes.Count - 1
            selectedItem = CType(Me.UtentiGridView.Items(Me.UtentiGridView.SelectedIndexes(i)), GridDataItem)
            Dim idUtente As Integer = selectedItem.OwnerTableView.DataKeyValues(selectedItem.ItemIndex)("IdUtente")

            Dim utente As ParsecAdmin.UtenteCentroResponsabilita = Me.Utenti.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault
            Me.Utenti.Remove(utente)
        Next
      
        Me.UtentiGridView.Rebind()

    End Sub

#End Region

  
End Class