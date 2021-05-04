Imports ParsecAdmin

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class GestioneEmergenzaPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    'Variabile di Sessione: oggetto SessioneEmergenza corrente.
    Public Property SessioneEmergenza() As ParsecPro.SessioneEmergenza
        Get
            Return CType(Session("GestioneEmergenzaPage_SessioneEmergenza"), ParsecPro.SessioneEmergenza)
        End Get
        Set(ByVal value As ParsecPro.SessioneEmergenza)
            Session("GestioneEmergenzaPage_SessioneEmergenza") = value
        End Set
    End Property

    'Variabile di Sessione: lista delle Sessioi di Emergenza associata alla griglia
    Public Property SessioniEmergenza() As List(Of ParsecPro.SessioneEmergenza)
        Get
            Return CType(Session("GestioneEmergenzaPage_SessioniEmergenza"), List(Of ParsecPro.SessioneEmergenza))
        End Get
        Set(ByVal value As List(Of ParsecPro.SessioneEmergenza))
            Session("GestioneEmergenzaPage_SessioniEmergenza") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Gestione Sessioni di Emergenza"

        If Not Me.Page.IsPostBack Then
            Me.ResettaVista()
        End If

        Me.RadToolBar.Items.FindItemByText("Elimina").Enabled = False
        Me.RadToolBar.Items.FindItemByText("Stampa").Enabled = False
     
    End Sub

    'Evento LoadComplete associato alla Pagina: setta i Titoli sulle griglie e i messaggi di cancellazione.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'elemento selezionato?", False, Not Me.SessioneEmergenza Is Nothing)
        Me.TitoloElencoSessioniEmergenzaLabel.Text = ""
        Me.TitoloElencoSessioniEmergenzaLabel.Text = "Elenco sessioni emergenza " & If(Me.SessioniEmergenzaGridView.MasterTableView.Items.Count > 0, "( " & Me.SessioniEmergenzaGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    'Evento Click della RadToolBar: lancia i vari comandi della Toolbar.
    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    Me.SessioniEmergenza = Nothing
                    Me.SessioniEmergenzaGridView.Rebind()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try

                ParsecUtility.Utility.MessageBox(message, False)

            Case "Nuovo"
                Me.ResettaVista()
                Me.SessioniEmergenza = Nothing
                Me.SessioniEmergenzaGridView.Rebind()
            Case "Annulla"
                Me.ResettaVista()
                Me.SessioniEmergenza = Nothing
                Me.SessioniEmergenzaGridView.Rebind()
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.SessioneEmergenza Is Nothing Then
                        Me.Delete()
                        Me.SessioniEmergenza = Nothing
                        Me.ResettaVista()
                        Me.SessioniEmergenzaGridView.Rebind()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una sessione di emergenza!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

    'Evento ItemCreated associato alla Pagina: associa l'inclick sul pulsante "Elimina" della toolbar
    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemCommand associato alla griglia SessioniEmergenzaGridView. Lancia i comandi associati alla griglia SessioniEmergenzaGridView.
    Protected Sub SessioniEmergenzaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles SessioniEmergenzaGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
            Me.RadToolBar.Items.FindItemByText("Salva").Enabled = False
        End If
    End Sub

    'Evento ItemDataBound associato alla griglia SessioniEmergenzaGridView. Setta i tooltip sul button "select" della griglia.
    Protected Sub SessioniEmergenzaGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles SessioniEmergenzaGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona sessione emergenza"
            End If
        End If
    End Sub

    'Evento NeedDataSource associato alla griglia SessioniEmergenzaGridView. Effettua la mappatura tra il datasource della SessioniEmergenzaGridView e la lsita delle sessioni di emergenza.
    Protected Sub SessioniEmergenzaGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles SessioniEmergenzaGridView.NeedDataSource
        If Me.SessioniEmergenza Is Nothing Then
            Dim sessioni As New ParsecPro.SessioniEmergenzaRepository
            Me.SessioniEmergenza = sessioni.GetView(Nothing)
            sessioni.Dispose()
        End If
        Me.SessioniEmergenzaGridView.DataSource = Me.SessioniEmergenza
    End Sub


#End Region

#Region "METODI PRIVATI"

    'Metodo Print
    Private Sub Print()
        'TODO
    End Sub

    'Metodo Delete
    Private Sub Delete()
        'TODO
    End Sub

    'Metodo che effettua una ricerca su DB
    Private Sub Search()
        Dim sessioni As New ParsecPro.SessioniEmergenzaRepository
        Dim searchTemplate As New ParsecPro.SessioneEmergenza With
            {
                .Descrizione = Me.DescrizioneTextBox.Text
            }

        Me.SessioniEmergenza = sessioni.GetView(searchTemplate)
        Me.SessioniEmergenzaGridView.Rebind()
        sessioni.Dispose()
    End Sub

    'Metodo che si occupa del salvataggio su DB
    Private Sub Save()
        Dim sessioni As New ParsecPro.SessioniEmergenzaRepository
        Dim sessione As ParsecPro.SessioneEmergenza = New ParsecPro.SessioneEmergenza 'sessioni.CreateFromInstance(Me.SessioneEmergenza)

        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        Try

            If Me.SessioneEmergenza Is Nothing Then
                sessione.Codice = sessioni.GetNuovoCodice
            Else
                sessione.Codice = sessioni.GetById(Me.SessioneEmergenza.Id).Codice
            End If

            sessione.Descrizione = Me.DescrizioneTextBox.Text.Trim

            Dim dataInizio As Nullable(Of Date) = Me.DataEmergenzaInizioTextBox.SelectedDate
            Dim orarioInizio As Nullable(Of Date) = Me.OrarioEmergenzaInizioTextBox.SelectedDate

            If dataInizio.HasValue AndAlso orarioInizio.HasValue Then
                sessione.DataOraInizio = dataInizio.Value.AddHours(orarioInizio.Value.Hour).AddMinutes(orarioInizio.Value.Minute)
            Else
                sessione.DataOraInizio = Me.DataEmergenzaInizioTextBox.SelectedDate
            End If

            Dim dataFine As Nullable(Of Date) = Me.DataEmergenzaFineTextBox.SelectedDate
            Dim orarioFine As Nullable(Of Date) = Me.OrarioEmergenzaFineTextBox.SelectedDate

            If dataInizio.HasValue AndAlso orarioInizio.HasValue Then
                sessione.DataOraFine = dataFine.Value.AddHours(orarioFine.Value.Hour).AddMinutes(orarioFine.Value.Minute)
            Else
                sessione.DataOraFine = Me.DataEmergenzaFineTextBox.SelectedDate
            End If

            If Not String.IsNullOrEmpty(Me.NumeroRegistrazioniTextBox.Text) Then
                If Not ParsecUtility.Utility.CheckNumber(Me.NumeroRegistrazioniTextBox.Text) Then
                    sessione.NumeroRegistrazioni = CInt(Me.NumeroRegistrazioniTextBox.Text)
                End If
            End If

            sessione.Causa = Me.CausaTextBox.Text.Trim

            sessione.LogIdUtente = utenteCollegato.Id
            sessione.LogDataOperazione = Now
            sessione.SbloccaSistema = Me.SbloccaSistemaCheckBox.Checked


            '*******************************************************************
            'Gestione storico.
            '*******************************************************************
            sessioni.SessioneEmergenza = Me.SessioneEmergenza

            sessioni.Save(sessione)

            '*******************************************************************
            'Aggiorno l'oggetto corrente
            '*******************************************************************
            Me.SessioneEmergenza = sessioni.SessioneEmergenza
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            sessioni.Dispose()
        End Try
    End Sub

    'Resetta i campi del Form.
    Private Sub ResettaVista()

        Me.SessioneEmergenza = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
        Me.NumeroRegistrazioniTextBox.Text = String.Empty
        Me.CausaTextBox.Text = String.Empty
        Me.SbloccaSistemaCheckBox.Checked = False

        Me.DataEmergenzaInizioTextBox.SelectedDate = Now
        Me.DataEmergenzaFineTextBox.SelectedDate = Now

        Me.OrarioEmergenzaInizioTextBox.SelectedDate = Now.AddHours(-2)
        Me.OrarioEmergenzaFineTextBox.SelectedDate = Now.AddHours(-1)

    End Sub

    'Metodo che riempie i campi della maschera dopo che un elemento è stato selezionato dalla griglia
    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim sessioniEmergenza As New ParsecPro.SessioniEmergenzaRepository

        Dim sessioneCorrente As ParsecPro.SessioneEmergenza = sessioniEmergenza.GetById(id)

        Me.DescrizioneTextBox.Text = sessioneCorrente.Descrizione
        Me.NumeroRegistrazioniTextBox.Text = sessioneCorrente.NumeroRegistrazioni
        Me.CausaTextBox.Text = sessioneCorrente.Causa

        Me.DataEmergenzaInizioTextBox.SelectedDate = sessioneCorrente.DataOraInizio
        Me.DataEmergenzaFineTextBox.SelectedDate = sessioneCorrente.DataOraFine

        Me.OrarioEmergenzaInizioTextBox.SelectedDate = sessioneCorrente.DataOraInizio
        Me.OrarioEmergenzaFineTextBox.SelectedDate = sessioneCorrente.DataOraFine

        Me.SessioneEmergenza = sessioneCorrente

        sessioniEmergenza.Dispose()
    End Sub

#End Region

End Class