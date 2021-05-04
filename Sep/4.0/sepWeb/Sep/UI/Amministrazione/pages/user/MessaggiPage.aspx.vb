Imports ParsecAdmin
Imports System.Transactions

Partial Class MessaggiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Messaggio() As ParsecAdmin.Messaggio
        Get
            Return CType(Session("MessaggiPage_Messaggio"), ParsecAdmin.Messaggio)
        End Get
        Set(ByVal value As ParsecAdmin.Messaggio)
            Session("MessaggiPage_Messaggio") = value
        End Set
    End Property

    Public Property Messaggi() As List(Of ParsecAdmin.Messaggio)
        Get
            Return CType(Session("MessaggiPage_Messaggi"), List(Of ParsecAdmin.Messaggio))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Messaggio))
            Session("MessaggiPage_Messaggi") = value
        End Set
    End Property

    Public Property Utenti() As List(Of ParsecAdmin.MessaggioUtente)
        Get
            Return CType(Session("MessaggiPage_Utenti"), List(Of ParsecAdmin.MessaggioUtente))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.MessaggioUtente))
            Session("MessaggiPage_Utenti") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Messaggi"
        Me.DataTextBox.Text = String.Format("{0:dd/MM/yyyy}", Now)
        If Not Me.Page.IsPostBack Then
            Me.Utenti = New List(Of ParsecAdmin.MessaggioUtente)
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.MessaggiGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il messaggio selezionato?", False, Not Me.Messaggio Is Nothing)
        Me.TitoloLabel.Text = "Elenco Messaggi&nbsp;&nbsp;&nbsp;" & If(Me.Messaggi.Count > 0, "( " & Me.Messaggi.Count.ToString & " )", "")

        Me.UtentiListBox.DataValueField = "IdUtente"
        Me.UtentiListBox.DataTextField = "Utente"

        Me.UtentiListBox.DataSource = Me.Utenti
        Me.UtentiListBox.DataBind()
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
                    If Not Me.Messaggio Is Nothing Then
                        Me.Delete()
                        Me.ResettaVista()
                        Me.AggiornaGriglia()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un messaggio!", False)
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

    Protected Sub MessaggiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles MessaggiGridView.NeedDataSource
        If Me.Messaggi Is Nothing Then
            Dim messaggi As New ParsecAdmin.MessaggioRepository
            Me.Messaggi = messaggi.GetView(Nothing)
            messaggi.Dispose()
        End If
        Me.MessaggiGridView.DataSource = Me.Messaggi
    End Sub

    Protected Sub MessaggiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles MessaggiGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub AggiornaGriglia()
        Me.Messaggi = Nothing
        Me.MessaggiGridView.Rebind()
    End Sub

    
    Private Sub Search()
        Dim messaggi As New ParsecAdmin.MessaggioRepository
        Dim filtro As ParsecAdmin.FiltroMessaggio = Me.GetFiltro
        Me.Messaggi = messaggi.GetView(filtro)
        Me.MessaggiGridView.Rebind()
        messaggi.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FiltroMessaggio
        Dim filtro As New ParsecAdmin.FiltroMessaggio
        filtro.Contenuto = Me.ContenutoTextBox.Text
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
        Dim messaggi As New ParsecAdmin.MessaggioRepository
        messaggi.Delete(Me.Messaggio)
        messaggi.Dispose()
    End Sub

    Private Sub Save()
        Dim messaggi As New ParsecAdmin.MessaggioRepository
        Dim messaggio As ParsecAdmin.Messaggio = messaggi.CreateFromInstance(Me.Messaggio)
        If Me.Messaggio Is Nothing Then
            messaggio.Data = Now
        End If
        messaggio.Contenuto = Me.ContenutoTextBox.Text
        messaggio.Utenti = Me.Utenti
        Try
            messaggi.Save(messaggio)
            Me.Messaggio = messaggi.Messaggio
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            messaggi.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.Messaggio = Nothing
        Me.ContenutoTextBox.Text = String.Empty
        Me.DataTextBox.Text = String.Format("{0:dd/MM/yyyy}", Now)
        Me.Utenti = New List(Of ParsecAdmin.MessaggioUtente)
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim messaggi As New ParsecAdmin.MessaggioRepository
        Me.Messaggio = messaggi.GetById(id)
        Me.DataTextBox.Text = Me.Messaggio.Data.ToShortDateString
        Me.ContenutoTextBox.Text = Me.Messaggio.Contenuto

        Dim utentiMessaggio As New ParsecAdmin.UtentiMessaggioRepository
        Me.Utenti = utentiMessaggio.GetView(Me.Messaggio)
        messaggi.Dispose()

        utentiMessaggio.Dispose()
    End Sub

    Private Sub DeleteSelectedItems(ByVal list As Telerik.Web.UI.RadListBox)
        For Each item As Telerik.Web.UI.RadListBoxItem In list.CheckedItems
            Dim idUtente As Integer = CInt(item.Value)
            Dim utente = Me.Utenti.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault
            If Not utente Is Nothing Then
                Me.Utenti.Remove(utente)
            End If
        Next
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'selezione multipla
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            For Each utente In utentiSelezionati
                Dim idUtente As Integer = CInt(utente.Key)
                Dim utenteCorrente = Me.Utenti.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault
                If utenteCorrente Is Nothing Then
                    Dim utenteMessaggio As New ParsecAdmin.MessaggioUtente With {.IdUtente = idUtente, .Utente = utente.Value, .Visibile = True}
                    Me.Utenti.Add(utenteMessaggio)
                End If
            Next
        End If
        Session("SelectedUsers") = Nothing
    End Sub

    Protected Sub EliminaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteImageButton.Click
        Me.DeleteSelectedItems(Me.UtentiListBox)
    End Sub

#End Region

End Class