Imports ParsecAdmin
Imports System.Threading
Imports Telerik.Web.UI

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class TipoRicezioneInvioPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    'proprietà Shard privata
    Private Shared SessionName As String = "TipoRicezioneInvioPage_TipoRicezioneInvio"

    'Variabile di Sessione: oggetto TipoRicezioneInvio corrente.
    Public Property TipoRicezioneInvio() As ParsecPro.TipoRicezioneInvio
        Get
            Return CType(Session(SessionName), ParsecPro.TipoRicezioneInvio)
        End Get
        Set(ByVal value As ParsecPro.TipoRicezioneInvio)
            Session(SessionName) = value
        End Set
    End Property

    'Variabile di Sessione: lista di oggetti TipiRicezioneInvio associata alla griglia TipiRicezioneInvioGridView
    Public Property TipiRicezioneInvio() As List(Of ParsecPro.TipoRicezioneInvio)
        Get
            Return CType(Session("TipoRicezioneInvioPage_TipiRicezioneInvio"), List(Of ParsecPro.TipoRicezioneInvio))
        End Get
        Set(ByVal value As List(Of ParsecPro.TipoRicezioneInvio))
            Session("TipoRicezioneInvioPage_TipiRicezioneInvio") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Gestione Tipi di Ricezione/Invio"
        If Not Me.Page.IsPostBack Then
            Me.TipiRicezioneInvio = Nothing
            Me.ResettaVista()
        End If
    End Sub

    'Evento LoadComplete associato alla Pagina. Setta i titoli della Griglia
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'elemento selezionato?", False, Not Me.TipoRicezioneInvio Is Nothing)
        Me.TitoloElencoTipiRicezioneInvioLabel.Text = "Elenco tipi ricezione/invio&nbsp;&nbsp;&nbsp;" & If(Me.TipiRicezioneInvio.Count > 0, "( " & Me.TipiRicezioneInvio.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    'Evento Cick associato alla RadToolBar. Lancia i comandi della Toolbar.
    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    Me.TipiRicezioneInvio = Nothing
                    Me.TipiRicezioneInvioGridView.Rebind()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try

                ParsecUtility.Utility.MessageBox(message, False)

            Case "Nuovo"
                Me.ResettaVista()
                Me.TipiRicezioneInvio = Nothing
                Me.TipiRicezioneInvioGridView.Rebind()

              Case "Annulla"
                Me.ResettaVista()
                Me.TipiRicezioneInvio = Nothing
                Me.TipiRicezioneInvioGridView.Rebind()

              Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.TipoRicezioneInvio Is Nothing Then
                        Me.Delete()
                        Me.TipiRicezioneInvio = Nothing
                        Me.ResettaVista()
                        Me.TipiRicezioneInvioGridView.Rebind()

                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un tipo di documento!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"

            Case "Trova"
                Me.Search()
        End Select
    End Sub

    'Evento ItemCreated associato alla RadToolBar. Aggiunge l'evento click al pulsante "Elimina"
    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemCommand associato alla TipiRicezioneInvioGridView. Fa partire i comandi associati alla griglia.
    Protected Sub TipiRicezioneInvioGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TipiRicezioneInvioGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

    'Evento ItemDataBound associato alla TipiRicezioneInvioGridView. Setta alcuni tooltip.
    Protected Sub TipiRicezioneInvioGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TipiRicezioneInvioGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona tipo ricezione/invio"
            End If
        End If
    End Sub

    'Evento NeedDataSource associato alla griglia TipiRicezioneInvioGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione TipiRicezioneInvio.
    Protected Sub TipiRicezioneInvioGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles TipiRicezioneInvioGridView.NeedDataSource
        If Me.TipiRicezioneInvio Is Nothing Then
            Dim tipiRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository
            Me.TipiRicezioneInvio = tipiRicezioneInvio.GetView(Nothing)
            tipiRicezioneInvio.Dispose()
        End If
        Me.TipiRicezioneInvioGridView.DataSource = Me.TipiRicezioneInvio
    End Sub

    'Evento ItemCreated associato alla Griglia TipiRicezioneInvioGridView. Gestisce la navigazione tra pagine della griglia.
    Private Sub TipiRicezioneInvioGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles TipiRicezioneInvioGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    'Metodo Print: non implementato
    Private Sub Print()

    End Sub

    'Cancellazione Logica di un TipoRicezioneInvio. Invocato dalla Toolbar nel caso "Elimina"
    Private Sub Delete()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim tipiRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository

        Dim tipoRicezioneInvio As ParsecPro.TipoRicezioneInvio = tipiRicezioneInvio.Where(Function(c) c.Id = Me.TipoRicezioneInvio.Id).FirstOrDefault
        tipoRicezioneInvio.LogStato = "A"

        tipiRicezioneInvio.SaveChanges()

        tipiRicezioneInvio.Dispose()
    End Sub

    'Metodo search per la Ricerca
    Private Sub Search()
        Dim tipiRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository
        Dim searchTemplate As New ParsecPro.TipoRicezioneInvio With
            {
                .Descrizione = Me.DescrizioneTextBox.Text
            }

        Me.TipiRicezioneInvio = tipiRicezioneInvio.GetView(searchTemplate)
        Me.TipiRicezioneInvioGridView.Rebind()
        tipiRicezioneInvio.Dispose()
    End Sub

    'Metodo save per il salvataggio su DB. Invocato dalla Toolbar
    Private Sub Save()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim tipiRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository
        Dim tipoRicezioneInvio As New ParsecPro.TipoRicezioneInvio
        tipoRicezioneInvio.Descrizione = Trim(Me.DescrizioneTextBox.Text.ToUpper)
        tipoRicezioneInvio.LogIdUtente = utenteCollegato.Id
        tipoRicezioneInvio.LogUtente = utenteCollegato.Username
        tipoRicezioneInvio.LogDataRegistrazione = Now
        tipoRicezioneInvio.Codice = tipiRicezioneInvio.GetNuovoCodice
        Try

            '*******************************************************************
            'Gestione storico.
            '*******************************************************************
            tipiRicezioneInvio.TipoRicezioneInvio = Me.TipoRicezioneInvio
            tipiRicezioneInvio.Save(tipoRicezioneInvio)

            '*******************************************************************
            'Aggiorno l'oggetto corrente
            '*******************************************************************
            Me.TipoRicezioneInvio = tipiRicezioneInvio.TipoRicezioneInvio
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            tipiRicezioneInvio.Dispose()
        End Try
    End Sub

    'Resetta la View
    Private Sub ResettaVista()
        Me.TipoRicezioneInvio = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
    End Sub

    'Aggiorna la view quando si seleziona un elemento dalla griglia
    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim tipiRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository
        Me.TipoRicezioneInvio = tipiRicezioneInvio.GetById(id)
        Me.DescrizioneTextBox.Text = Me.TipoRicezioneInvio.Descrizione
        tipiRicezioneInvio.Dispose()
    End Sub

#End Region

  

End Class