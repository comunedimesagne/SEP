Imports ParsecWKF
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class RuoliPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Private Property Ruoli() As List(Of Ruolo)
        Get
            Return CType(Session("RuoliPage_Ruoli"), List(Of Ruolo))
        End Get
        Set(ByVal value As List(Of Ruolo))
            Session("RuoliPage_Ruoli") = value
        End Set
    End Property

    Private Property Ruolo() As Ruolo
        Get
            Return CType(Session("RuoliPage_Ruolo"), Ruolo)
        End Get
        Set(ByVal value As Ruolo)
            Session("RuoliPage_Ruolo") = value
        End Set
    End Property

    Private Property UtentiRuolo() As List(Of ParsecWKF.UtentiRuolo)
        Get
            Return CType(Session("RuoliPage_UtentiRuolo"), List(Of ParsecWKF.UtentiRuolo))
        End Get
        Set(ByVal value As List(Of ParsecWKF.UtentiRuolo))
            Session("RuoliPage_UtentiRuolo") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Scrivania"
        MainPage.DescrizioneProcedura = "> Gestione Ruoli"

        If Not IsPostBack AndAlso Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
            Me.ResettaVista()
            Me.Ruoli = Nothing
            Me.UtentiRuolo = New List(Of ParsecWKF.UtentiRuolo)

            RadToolBar.FindItemByText("Nuovo").Enabled = utenteCollegato.SuperUser
            RadToolBar.FindItemByText("Salva").Enabled = utenteCollegato.SuperUser
            RadToolBar.FindItemByText("Elimina").Enabled = utenteCollegato.SuperUser

            Me.CaricaTipologie()

        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGrigliaUtenti.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.UtentiRuoloGridView.Style.Add("width", widthStyle)
        Me.RuoliGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Not Me.UtentiRuolo Is Nothing Then
            Me.UtentiRuoloLabel.Text = "Elenco Utenti " & If(Me.UtentiRuolo.Count > 0, "( " & Me.UtentiRuolo.Count.ToString & " )", "")
        End If
        If Not Me.Ruoli Is Nothing Then
            Me.RuoliLabel.Text = "Elenco Ruoli " & If(Me.Ruoli.Count > 0, "( " & Me.Ruoli.Count.ToString & " )", "")
        End If
        ParsecUtility.Utility.Confirm("Eliminare l'elemento selezionato?", False, Not Me.Ruolo Is Nothing)

        'SELEZIONO LA RIGA
        If Not Me.Ruolo Is Nothing Then
            Dim item As GridDataItem = Me.RuoliGridView.MasterTableView.FindItemByKeyValue("Id", Me.Ruolo.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub RuoliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RuoliGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

    Protected Sub RuoliGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RuoliGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona Ruolo"
            End If
        End If
    End Sub

    Protected Sub RuoliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RuoliGridView.NeedDataSource
        If Me.Ruoli Is Nothing Then
            Dim ruoli As New ParsecWKF.RuoloRepository
            Me.Ruoli = ruoli.GetView(Nothing)
            ruoli.Dispose()
        End If
        Me.RuoliGridView.DataSource = Me.Ruoli
    End Sub

    Protected Sub RuoliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RuoliGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA UTENTI"

    Protected Sub UtentiRuoloGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiRuoloGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim roleusr As UtentiRuolo = CType(e.Item.DataItem, UtentiRuolo)
            Dim utCorr As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Cancellazione dell'utente dal ruolo"
                Dim message As String = "Eliminare l'utente selezionato dal ruolo?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If
            btn.Enabled = utCorr.SuperUser
            If Not utCorr.SuperUser Then
                btn.ToolTip = "DISABILITATO ..."
            End If
        End If
    End Sub

    Protected Sub UtentiRuoloGV_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiRuoloGridView.NeedDataSource
        Me.UtentiRuoloGridView.DataSource = Me.UtentiRuolo
    End Sub

    Private Sub UtentiRuoloGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiRuoloGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub UtentiRuoloGV_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UtentiRuoloGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.CancellaUtente(e.Item)
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
        Dim rr As New RuoloRepository
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Ruolo Is Nothing Then
                        Dim message As String = String.Empty
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.UtentiRuolo = New List(Of ParsecWKF.UtentiRuolo)
                            Me.UtentiRuoloGridView.Rebind()
                            'Leggo i ruoli dal database.
                            Me.AggiornaGriglia()
                        Catch ex As Exception
                            message = ex.Message
                        End Try
                        Me.VisualizzaMessaggio(message)
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un ruolo!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"

            Case "Stampa"
                Me.Print()
            Case "Nuovo"
                Me.UtentiRuolo = New List(Of ParsecWKF.UtentiRuolo)
                Me.UtentiRuoloGridView.Rebind()
                Me.ResettaVista()
                'Leggo i ruoli dal database.
                Me.AggiornaGriglia()

            Case "Annulla"
                Me.UtentiRuolo = New List(Of ParsecWKF.UtentiRuolo)
                Me.UtentiRuoloGridView.Rebind()
                Me.ResettaVista()
                'Leggo i ruoli dal database.
                Me.AggiornaGriglia()

            Case "Salva"
                Dim message As String = String.Empty
                Try
                    Me.Salva()
                    'Leggo i ruoli dal database.
                    Me.AggiornaGriglia()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                Me.VisualizzaMessaggio(message)
              
            Case "Trova"
                Me.Search()

        End Select
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub CaricaTipologie()
        Dim dati As New Dictionary(Of String, String)
        dati.Add(-1, "")
        dati.Add(1, "Dinamica")
        Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})
        Me.TipologiaRuoloComboBox.DataSource = ds
        Me.TipologiaRuoloComboBox.DataTextField = "Descrizione"
        Me.TipologiaRuoloComboBox.DataValueField = "Id"
        Me.TipologiaRuoloComboBox.DataBind()
        'Me.TipologiaRuoloComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", -1))
        Me.TipologiaRuoloComboBox.SelectedIndex = 0
    End Sub

    Private Sub VisualizzaMessaggio(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
        Else
            ParsecUtility.Utility.MessageBox(message, False)
        End If
    End Sub

    Private Sub AggiornaGriglia()
        Me.Ruoli = Nothing
        Me.RuoliGridView.Rebind()
    End Sub

    Private Sub Search()
        Dim ruoli As New ParsecWKF.RuoloRepository
        Dim filtro = Me.GetFiltro
        Me.Ruoli = ruoli.GetView(filtro)
        ruoli.Dispose()
        Me.RuoliGridView.Rebind()
    End Sub

    Private Function GetFiltro() As ParsecWKF.RuoloFiltro
        Dim filtro As New ParsecWKF.RuoloFiltro
        If Not String.IsNullOrEmpty(Me.RuoloTextBox.Text) Then
            filtro.Descrizione = Me.RuoloTextBox.Text
        End If
        Return filtro
    End Function

    Private Sub Print()
        'Dim parametriStampa As New Hashtable
        'parametriStampa.Add("TipologiaStampa", "StampaRuoli")
        'parametriStampa.Add("DatiStampa", Me.Ruoli)
        'Session("ParametriStampa") = parametriStampa
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
        ParsecUtility.Utility.MessageBox("Funzionalità non abilitata!", False)
    End Sub

    Private Sub ResettaVista()
        Me.Ruolo = Nothing
        Me.RuoloTextBox.Text = String.Empty
        Me.TipologiaRuoloComboBox.SelectedIndex = 0
    End Sub

    Private Sub ResettaVistaRuoloUtenti()
        Me.RuoloTextBox.Text = String.Empty
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim ruoli As New RuoloRepository
        Me.Ruolo = ruoli.GetById(id)
        Me.RuoloTextBox.Text = Me.Ruolo.Descrizione
        If Me.Ruolo.Tipologia.HasValue Then
            Me.TipologiaRuoloComboBox.SelectedIndex = Me.Ruolo.Tipologia
        End If

        Me.UtentiRuolo = Me.Ruolo.Utenti
        ruoli.Dispose()
        Me.UtentiRuoloGridView.Rebind()
    End Sub

    Private Sub Salva()
        Dim ruoli As New RuoloRepository
        Dim ruolo As Ruolo = ruoli.CreateFromInstance(Me.Ruolo)
        ruolo.Descrizione = Me.RuoloTextBox.Text

        ruolo.Tipologia = Nothing
        If Me.TipologiaRuoloComboBox.SelectedIndex <> 0 Then
            ruolo.Tipologia = Me.TipologiaRuoloComboBox.SelectedValue
        End If

        Try
            ruoli.Save(ruolo, Me.UtentiRuolo)
            Me.Ruolo = ruoli.Ruolo
        Catch ex As Exception
            If ex.InnerException Is Nothing Then
                Throw New ApplicationException(ex.Message)
            Else
                Throw New ApplicationException(ex.InnerException.Message)
            End If
        Finally
            ruoli.Dispose()
        End Try
    End Sub

    Private Sub CancellaUtente(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idUtente As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdUtente")

        'Dim ids As New List(Of Integer)
        'ids.Add(idUtente)
        'Me.UtentiRuolo = (From utente In Me.UtentiRuolo
        '                  Where Not ids.Contains(utente.IdUtente)
        '                  Select utente).ToList


        ' Me.UtentiRuolo.RemoveAll(Function(c) c.IdUtente = idUtente)

        Dim utenteSelezionato = Me.UtentiRuolo.Where(Function(c) c.IdUtente = idUtente)
        If utenteSelezionato.Any Then
            Me.UtentiRuolo.Remove(utenteSelezionato.FirstOrDefault)
        End If


        'If id = 0 Then
        '    Me.UtentiRuolo.RemoveAt(item.ItemIndex)
        'Else
        '    Dim ids As New List(Of Integer)
        '    ids.Add(id)
        '    Me.UtentiRuolo = (From utente In Me.UtentiRuolo
        '                      Where Not ids.Contains(utente.Id)
        '                      Select utente).ToList
        'End If

        Me.UtentiRuoloGridView.Rebind()

    End Sub

    Private Sub Delete()
        Dim firme As New ParsecAtt.FirmeRepository
        Dim id As Integer = Me.Ruolo.Id
        Dim ruoloAssociato As Boolean = firme.Where(Function(c) c.IdRuolo = id).Any
        If Not ruoloAssociato Then
            Dim ruoli As New RuoloRepository
            Dim utentiRuolo As New RuoloRelUtenteRepository
            ruoli.Delete(id)
            utentiRuolo.DeleteAll(id)
            ruoli.Dispose()
            utentiRuolo.Dispose()
        Else
            Throw New ApplicationException("Impossibile eliminare il ruolo selezionato!" & vbCrLf & "Il ruolo è associato ad una o più firme")
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
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


            Dim utenti = (From c In utentiSelezionati
                      Select New ParsecWKF.UtentiRuolo With {
                          .IdUtente = c.Key,
                          .Utente = c.Value
                      }).ToList


            Me.UtentiRuolo = utenti.Union(Me.UtentiRuolo).GroupBy(Function(c) c.IdUtente).Select(Function(c) c.First).ToList
            Session("SelectedUsers") = Nothing

            Me.UtentiRuoloGridView.Rebind()
        End If
    End Sub

#End Region


End Class