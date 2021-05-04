#Region "IMPORTS"
Imports Telerik.Web.UI
Imports ParsecAdmin


#End Region

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class RicercaProtocolliPage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    'Variabile di Sessione per la registrazione degli script di chiusura
    Public Property RegistraScriptChiusura As Boolean
        Set(ByVal value As Boolean)
            Session("RicercaProtocolliPage_RegistraScriptChiusura") = value
        End Set
        Get
            Return CType(Session("RicercaProtocolliPage_RegistraScriptChiusura"), Boolean)
        End Get
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim tipiRicezione As New ParsecPro.TipiRicezioneInvioRepository
        Dim listaTipiRicezioneInvio As List(Of ParsecPro.TipoRicezioneInvio) = tipiRicezione.GetView(Nothing)

        Me.TipoRicezioneInvioComboBox.DataSource = listaTipiRicezioneInvio
        Me.TipoRicezioneInvioComboBox.DataTextField = "Descrizione"
        Me.TipoRicezioneInvioComboBox.DataValueField = "Id"
        Me.TipoRicezioneInvioComboBox.DataBind()
        Me.TipoRicezioneInvioComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Seleziona tipo ricezione -", "-1"))
        Me.TipoRicezioneInvioComboBox.SelectedIndex = 0

        tipiRicezione.Dispose()

        Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository

        Dim listaTipiDocumento As List(Of ParsecPro.TipoDocumento) = tipiDocumento.GetView(Nothing)
        Me.TipologiaDocumentoComboBox.DataSource = listaTipiDocumento
        Me.TipologiaDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologiaDocumentoComboBox.DataValueField = "Id"
        Me.TipologiaDocumentoComboBox.DataBind()
        Me.TipologiaDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Seleziona tipo documento -", "-1"))
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0

        tipiDocumento.Dispose()

        Me.FiltroRegistrazioniAnnullateComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("Escludi annullate", "0"))
        Me.FiltroRegistrazioniAnnullateComboBox.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem("Visualizza solo annullate", "1"))
        Me.FiltroRegistrazioniAnnullateComboBox.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem("Includi annullate", "2"))

        Me.DataProtocolloInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataProtocolloFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)

        If Not Me.Page.IsPostBack Then
            Me.RegistraScriptChiusura = True
        End If

        Dim script As New StringBuilder
        script.AppendLine("var value =  $find('" & Me.NumeroProtocolloInizioTextBox.ClientID & "').get_displayValue(); var textbox =  $find('" & Me.NumeroProtocolloFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")
        Me.NumeroProtocolloInizioTextBox.Attributes.Add("onblur", script.ToString)

    End Sub

    'Evento LoadComplete: gestisce la chiusura della pagina
    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        If Me.RegistraScriptChiusura Then
            ParsecUtility.Utility.CloseWindow(False)
        End If
        '***************************************************************************
    End Sub

#End Region

#Region "AZIONI PANNELLO FILTRO"

    'Resetta i campi utilizzati per la ricerca
    Private Sub ResettaFiltro()
        Me.RegistrazioneArrivoCheckBox.Checked = True
        Me.RegistrazionePartenzaCheckBox.Checked = True
        Me.RegistrazioneInternaCheckBox.Checked = True
        Me.FiltroRegistrazioniAnnullateComboBox.SelectedIndex = 0
        Me.NumeroProtocolloInizioTextBox.Text = String.Empty
        Me.NumeroProtocolloFineTextBox.Text = String.Empty
        Me.DataProtocolloInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataProtocolloFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        Me.DenominazioneCognomeTextBox.Text = String.Empty
        Me.NomeTextBox.Text = String.Empty
        Me.CittaTextBox.Text = String.Empty
        Me.EmailTextBox.Text = String.Empty
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
        Me.FiltroOggettoTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.ProtocolloMittenteTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty
        Me.NoteInterneTextBox.Text = String.Empty
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        Me.TipoRicezioneInvioComboBox.SelectedIndex = 0
        Me.DataDocumentoInizioTextBox.SelectedDate = Nothing
        Me.DataDocumentoFineTextBox.SelectedDate = Nothing
        Me.DataRicezioneInvioInizioTextBox.SelectedDate = Nothing
        Me.DataRicezioneInvioFineTextBox.SelectedDate = Nothing
        Me.UtenteInserimentoTextBox.Text = String.Empty
        Me.IdUtenteInserimentoTextBox.Text = String.Empty

    End Sub

    'Resetta i campi di ricerca
    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

    'Evento che, dopo aver convalidato i campi, costruisce il filtro di ricerca e lo pone in ParsecUtility.SessionManager per poter essere utilizzato
    Protected Sub CercaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CercaButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            ParsecUtility.SessionManager.FiltroRegistrazione = Me.GetFiltroRegistrazioni

            Me.RegistraScriptChiusura = False
            ParsecUtility.Utility.ClosePopup(True)

        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    'Metodo che controlla e valida i dati immessi per la ricerca.
    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean
        If Not Me.RegistrazioneArrivoCheckBox.Checked AndAlso Not Me.RegistrazionePartenzaCheckBox.Checked AndAlso Not Me.RegistrazioneInternaCheckBox.Checked Then
            message.AppendLine("E' necessario selezionare almeno un tipo di registrazione!")
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloInizioTextBox.Text)) Then
            If ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloInizioTextBox.Text) Then
                message.AppendLine("Se specificato, il campo 'Numero protocollo da' deve essere un numero!")
            End If
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloFineTextBox.Text)) Then
            If ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloFineTextBox.Text) Then
                message.AppendLine("Se specificato, il campo 'Numero protocollo a' deve essere un numero!")
            End If
        End If

        Return Not message.Length > 0
    End Function

    'Metodo invocato da CercaButton.Click. Costruisce e restituisce il filtro per la ricerca
    Private Function GetFiltroRegistrazioni() As ParsecPro.RegistrazioneFiltro

        Dim idUtenteInserimento As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Me.IdUtenteInserimentoTextBox.Text) Then
            idUtenteInserimento = Convert.ToInt32(Me.IdUtenteInserimentoTextBox.Text)
        End If

        'FILTRO PER STRUTTURA UTILIZZANDO IL CAMPO DESCRIZIONE PER POTER TROVARE ANCHE I PROTOCOLLI CON STRUTTURE MODIFICATE
        Dim idUfficio As Nullable(Of Int32) = Nothing
        Dim ufficio As String = String.Empty
        ufficio = Me.UfficioTextBox.Text.Trim

        Dim idClassificazione As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            idClassificazione = Convert.ToInt32(Me.IdClassificazioneTextBox.Text)
        End If

        Dim numeroRiscontro As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Trim(Me.NumeroRiscontroTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroRiscontroTextBox.Text) Then
                numeroRiscontro = CInt(Me.NumeroRiscontroTextBox.Text)
            End If
        End If

        Dim annoRiscontro As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Trim(Me.AnnoRiscontroTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.AnnoRiscontroTextBox.Text) Then
                annoRiscontro = CInt(Me.AnnoRiscontroTextBox.Text)
            End If
        End If

        Dim IdTipoDocumento As Nullable(Of Int32) = Nothing
        If Me.TipologiaDocumentoComboBox.SelectedIndex <> 0 Then
            IdTipoDocumento = Me.TipologiaDocumentoComboBox.SelectedValue
        End If

        Dim IdTipoRicezioneInvio As Nullable(Of Int32) = Nothing
        If Me.TipoRicezioneInvioComboBox.SelectedIndex <> 0 Then
            IdTipoRicezioneInvio = Me.TipoRicezioneInvioComboBox.SelectedValue
        End If

        Dim registrazioni As New ParsecPro.RegistrazioniRepository

        Dim annullata As Nullable(Of Boolean) = Nothing
        If Me.FiltroRegistrazioniAnnullateComboBox.SelectedIndex <> 2 Then
            annullata = CBool(Me.FiltroRegistrazioniAnnullateComboBox.SelectedIndex)
        End If

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim numeroProtocolloInizio As Nullable(Of Int32)
        Dim numeroProtocolloFine As Nullable(Of Int32)

        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloInizioTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloInizioTextBox.Text) Then
                numeroProtocolloInizio = CInt(Me.NumeroProtocolloInizioTextBox.Text)
            End If
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloFineTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloFineTextBox.Text) Then
                numeroProtocolloFine = CInt(Me.NumeroProtocolloFineTextBox.Text)
            End If
        End If

        Dim filtro As New ParsecPro.RegistrazioneFiltro With
            {
                .Arrivo = Me.RegistrazioneArrivoCheckBox.Checked,
                .Partenza = Me.RegistrazionePartenzaCheckBox.Checked,
                .Interna = Me.RegistrazioneInternaCheckBox.Checked,
                .DataProtocolloInizio = Me.DataProtocolloInizioTextBox.SelectedDate,
                .DataProtocolloFine = Me.DataProtocolloFineTextBox.SelectedDate,
                .Annullata = annullata,
                .IdUtenteCollegato = utenteCorrente.Id,
                .NumeroProtocolloInizio = numeroProtocolloInizio,
                .NumeroProtocolloFine = numeroProtocolloFine,
                .ReferenteEsternoDenominazione = Me.DenominazioneCognomeTextBox.Text,
                .ReferenteEsternoNome = Me.NomeTextBox.Text,
                .ReferenteEsternoCitta = Me.CittaTextBox.Text,
                .ReferenteEsternoEmail = Me.EmailTextBox.Text,
                .IdUtenteInserimento = idUtenteInserimento,
                .IdStruttura = idUfficio,
                .Struttura = ufficio,
                .StrutturaCompleta = Me.IncludiStruttureDipendentiCheckBox.Checked,
                .IdClassificazione = idClassificazione,
                .ClassificazioneCompleta = Me.IncludiClassificheDipendentiCheckBox.Checked,
                .Oggetto = Me.FiltroOggettoTextBox.Text,
                .ProtocolloMittente = Me.ProtocolloMittenteTextBox.Text,
                .Note = Me.NoteTextBox.Text,
                .NoteInterne = Me.NoteInterneTextBox.Text,
                .IdTipoDocumento = IdTipoDocumento,
                .IdTipoRicezioneInvio = IdTipoRicezioneInvio,
                .DataDocumentoInizio = Me.DataDocumentoInizioTextBox.SelectedDate,
                .DataDocumentoFine = Me.DataDocumentoFineTextBox.SelectedDate,
                .DataRicezioneInvioInizio = Me.DataRicezioneInvioInizioTextBox.SelectedDate,
                .DataRicezioneInvioFine = Me.DataRicezioneInvioFineTextBox.SelectedDate,
                .ApplicaAbilitazione = True
             }

        registrazioni.Dispose()
        Return filtro

    End Function

#End Region

#Region "GESTIONE RICERCA UFFICIO"

    'Meotdo di ricerca e associazione delle Strutture dell'Ente. Apre la pagina ad albero RicercaOrganigrammaPage.aspx per la associazione della Struttura.
    Protected Sub TrovaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaStrutturaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaStrutturaImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100,200,300,400")
        parametriPagina.Add("ultimoLivelloStruttura", "400") 'PERSONA
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    'Metodo che viene invocato da TrovaStrutturaImageButton.Click. Se è stata selezionata una Struttura nel metodo TrovaStrutturaImageButton.Click allora vengono popolati i campi della maschera.
    Protected Sub AggiornaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaStrutturaImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Me.UfficioTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdUfficioTextBox.Text = struttureSelezionate.First.Id
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    'Resetta i campi relativi all'ufficio.
    Protected Sub EliminaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaStrutturaImageButton.Click
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
    End Sub

#End Region

#Region "GESTIONE RICERCA UTENTE"

    'Metodo che apre la pagina di ricerca degli Utenti RicercaUtentePage.aspx. 
    'Se trovato l'utente di interesse può essere selezionato ed associato nella pagina di ricerca del protocollo come campo di ricerca.
    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 0 'singola
        Session("Parametri") = ht
    End Sub

    'Metodo che viene invocato dall'evento TrovaUtenteImageButton.Click. Popola i campi di ricerca relativi all'Utente di inserimento 
    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Me.UtenteInserimentoTextBox.Text = utentiSelezionati.First.Value
            Me.IdUtenteInserimentoTextBox.Text = utentiSelezionati.First.Key
            Session("SelectedUsers") = Nothing
        End If
    End Sub

    'Svuota i campi di ricerca relativi all'utente di inserimento della registrazione.
    Protected Sub EliminaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteImageButton.Click
        Me.UtenteInserimentoTextBox.Text = String.Empty
        Me.IdUtenteInserimentoTextBox.Text = String.Empty
    End Sub


#End Region


End Class