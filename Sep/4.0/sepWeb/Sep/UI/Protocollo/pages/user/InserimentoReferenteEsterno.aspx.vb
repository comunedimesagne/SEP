Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class InserimentoReferenteEsterno
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    'Variabile di Sessione: rappresenta il Referente corrente.
    Private Property Referente() As ParsecPro.IReferente
        Get
            Return CType(Session("InserimentoReferenteEsterno_Referente"), ParsecPro.IReferente)
        End Get
        Set(ByVal value As ParsecPro.IReferente)
            Session("InserimentoReferenteEsterno_Referente") = value
        End Set
    End Property

    'Variabile di Sessione: rappresenta il Referente memorizzato.
    Private Property ReferenteMemorizzato() As ParsecPro.IReferente
        Get
            Return CType(Session("InserimentoReferenteEsterno_ReferenteMemorizzato"), ParsecPro.IReferente)
        End Get
        Set(ByVal value As ParsecPro.IReferente)
            Session("InserimentoReferenteEsterno_ReferenteMemorizzato") = value
        End Set
    End Property

    'Variabile di Sessione: rappresenta la lista a cui si aggancia la griglia
    Private Property Referenti() As List(Of ParsecPro.IReferente)
        Get
            Return CType(Session("InserimentoReferenteEsterno_Referenti"), List(Of ParsecPro.IReferente))
        End Get
        Set(ByVal value As List(Of ParsecPro.IReferente))
            Session("InserimentoReferenteEsterno_Referenti") = value
        End Set
    End Property

    'Variabile di Sessione: rappresenta il Referente già esistente.
    Private Property ReferenteTrovato() As ParsecPro.IReferente
        Get
            Return CType(Session("InserimentoReferenteEsterno_ReferenteTrovato"), ParsecPro.IReferente)
        End Get
        Set(ByVal value As ParsecPro.IReferente)
            Session("InserimentoReferenteEsterno_ReferenteTrovato") = value
        End Set
    End Property



#End Region

#Region "EVENTI PAGINA"

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        ParsecUtility.Utility.RegisterDefaultButton(Me.CittaTextBox, Me.TrovaComuneImageButton)
        If Not Me.Page.IsPostBack Then
            Me.DenominazioneTextBox.Focus()
        End If
    End Sub

    'Evento LoadComplete della Pagina: dopo che la pagina è stata caricata carica la lista della Tiplogia di Soggetto.
    'In funzione del contentuo di parametriPagina("ListaReferenti") o parametriPagina("Referente") inizializza gli oggetti opportuni.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        If Not Me.Page.IsPostBack Then


            Me.TipoPersonaComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Fisica", "0"))
            Me.TipoPersonaComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Giuridica", "1"))
            Me.TipoPersonaComboBox.SelectedIndex = 0

            Me.Referente = Nothing
            Me.ReferenteMemorizzato = Nothing
            Me.Referenti = Nothing
            Me.ReferenteTrovato = Nothing

            If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
                Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina

                If parametriPagina.ContainsKey("ListaReferenti") Then
                    Me.Referenti = parametriPagina("ListaReferenti")
                End If

                If parametriPagina.ContainsKey("Referente") Then

                    Me.Referente = parametriPagina("Referente")
                    Me.ReferenteMemorizzato = Me.GetReferenteMemorizzato
                    Me.AggiornaVista(Me.Referente)
                    Me.Title = "Modifica Referente Esterno"

                Else
                    Me.Title = "Nuovo Referente Esterno"
                End If

                ParsecUtility.SessionManager.ParametriPagina = Nothing
            Else
                Me.Title = "Nuovo Referente Esterno"
            End If
        End If

    End Sub

#End Region


#Region "EVENTI CONTROLLI"

    'Evento click di SostituisciReferenteButton che effettua la sostituzione del referente.
    Private Sub SostituisciReferenteButton_Click(sender As Object, e As EventArgs) Handles SostituisciReferenteButton.Click

        If Not Me.Referente Is Nothing Then
            Session("IdReferenteDaSostituire") = Me.Referente.Id
        End If

        Session("ReferenteInserito") = Me.ReferenteTrovato

        Me.ReferenteTrovato = Nothing
        Me.Referente = Nothing
        Me.Referenti = Nothing
        Me.ReferenteMemorizzato = Nothing

        ParsecUtility.Utility.CloseRadWindowAndUpadateParent(True)

    End Sub

    'Pulsante che conferma l'inserimento del referente
    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click

        Dim tipoReferente As String = Me.Request.QueryString("tipoReferente")
        Dim referente As ParsecPro.IReferente = Nothing

        If Me.Referente Is Nothing Then
            If tipoReferente = "Mitt" Then
                referente = New ParsecPro.Mittente
            Else
                referente = New ParsecPro.Destinatario
            End If
        Else
            referente = Me.GetDeepCopyReferente(Me.Referente)
        End If

        'Protocollo Partenza e Interno  è un Destinatario Esterno

        Dim denominazione As String = If(Not Me.DenominazioneTextBox.Text Is Nothing, Me.DenominazioneTextBox.Text.Trim, Me.DenominazioneTextBox.Text)

        If Not String.IsNullOrEmpty(denominazione) Then

            If Me.Referente Is Nothing Then
                referente.Id = -1

                If Not Me.Referenti Is Nothing AndAlso Me.Referenti.Count > 0 Then
                    Dim nuovoId = Me.Referenti.Min(Function(c) c.Id) - 1
                    If nuovoId < 0 Then
                        referente.Id = nuovoId
                    End If
                End If

                referente.Codice = -1

            End If

            referente.Esistente = False
            referente.Interno = False
            referente.PerConoscenza = False
            referente.Cognome = Me.DenominazioneTextBox.Text


            referente.Nome = Nothing
            referente.Indirizzo = Nothing
            referente.Cap = Nothing
            referente.Citta = Nothing
            referente.Provincia = Nothing
            referente.Email = Nothing
            referente.LocalitaEstera = Nothing

            If Me.TipoPersonaComboBox.SelectedIndex > 0 Then
                referente.Tipologia = CInt(Me.TipoPersonaComboBox.SelectedValue)
            End If

            Dim nome As String = If(Not Me.NomeTextBox.Text Is Nothing, Me.NomeTextBox.Text.Trim, Me.NomeTextBox.Text)
            If Not String.IsNullOrEmpty(nome) Then
                referente.Nome = nome
            End If

            Dim indirizzo As String = If(Not Me.IndirizzoTextBox.Text Is Nothing, Me.IndirizzoTextBox.Text.Trim, Me.IndirizzoTextBox.Text)
            If Not String.IsNullOrEmpty(indirizzo) Then
                referente.Indirizzo = indirizzo
            End If

            Dim cap As String = If(Not Me.CapTextBox.Text Is Nothing, Me.CapTextBox.Text.Trim, Me.CapTextBox.Text)
            If Not String.IsNullOrEmpty(cap) Then
                referente.Cap = cap
            End If


            Dim citta As String = If(Not Me.CittaTextBox.Text Is Nothing, Me.CittaTextBox.Text.Trim, Me.CittaTextBox.Text)
            If Not String.IsNullOrEmpty(citta) Then
                referente.Citta = citta
            End If

            Dim provincia As String = If(Not Me.ProvinciaTextBox.Text Is Nothing, Me.ProvinciaTextBox.Text.Trim, Me.ProvinciaTextBox.Text)
            If Not String.IsNullOrEmpty(provincia) Then
                referente.Provincia = provincia
            End If

            Dim email As String = If(Not Me.EmailTextBox.Text Is Nothing, Me.EmailTextBox.Text.Trim, Me.EmailTextBox.Text)
            If Not String.IsNullOrEmpty(email) Then
                referente.Email = email
            End If

            Dim localita As String = If(Not Me.LocalitaEsteraTextBox.Text Is Nothing, Me.LocalitaEsteraTextBox.Text.Trim, Me.LocalitaEsteraTextBox.Text)
            If Not String.IsNullOrEmpty(localita) Then
                referente.LocalitaEstera = localita
            End If

            referente.Rubrica = Me.RubricaCheckBox.Checked

            Dim uguali As Boolean = False

            '************************************************************************************************************************
            'VERIFICO CHE NELLA LISTA DEI REFERENTI ASSOCIATI ALLA REGISTRAZIONE DI PROTOCOLLO NON SIANO PRESENTI DUPLICATI
            'SE IL REFERENTE CORRENTE E' NUOVO O E' DIVERSO DA QUELLO SELEZIONATO ALL'INIZIO (MEMORIZZATO DB)
            '************************************************************************************************************************
            If Not Me.Referenti Is Nothing Then

                If referente.Id >= 0 Then
                    uguali = Me.IsReferentiUguali(referente, Me.ReferenteMemorizzato)
                End If

                If referente.Id < 0 OrElse Not uguali Then
                    Dim mitt = Me.Referenti.Where(Function(c) c.Descrizione.Trim.ToLower = referente.Descrizione.Trim.ToLower And c.Id <> referente.Id).FirstOrDefault
                    If Not mitt Is Nothing Then
                        ParsecUtility.Utility.MessageBox("Il referente esterno è gia presente nella lista!", False)
                        Exit Sub
                    End If
                End If

            End If
            '************************************************************************************************************************


            '******************************************************************************************************************************
            'SE IL REFERENTE CORRENTE E' NUOVO O E' DIVERSO DA QUELLO SELEZIONATO ALL'INIZIO (MEMORIZZATO DB)
            '******************************************************************************************************************************

            If Not uguali Then
                '******************************************************************************************************************************
                'SE CORRISPONDE AD UN REFERENTE GIA' IN ARCHIVIO RECUPERO QUEST'ULTIMO
                '******************************************************************************************************************************
                Me.ReferenteTrovato = Me.GetReferente(referente, Me.ReferenteMemorizzato)

                If Not Me.ReferenteTrovato Is Nothing Then
                    Me.ExecuteConfirm("Il referente esterno corrente corrisponde ad un referente già presente in archivio." & vbCrLf & "Lo vuoi sostituire con quest'ultimo?", Me.SostituisciReferenteButton.ClientID)
                    Exit Sub
                End If
            End If
            '******************************************************************************************************************************

            If uguali Then

                Dim modificato = Me.IsReferenteModificato(referente, Me.ReferenteMemorizzato)
                If Not modificato Then

                    Me.Referente.Cognome = Me.ReferenteMemorizzato.Cognome
                    Me.Referente.Nome = Me.ReferenteMemorizzato.Nome
                    Me.Referente.Tipologia = Me.ReferenteMemorizzato.Tipologia
                    Me.Referente.Rubrica = Me.ReferenteMemorizzato.Rubrica
                    Me.Referente.Email = Me.ReferenteMemorizzato.Email
                    Me.Referente.Cap = Me.ReferenteMemorizzato.Cap
                    Me.Referente.Provincia = Me.ReferenteMemorizzato.Provincia
                    Me.Referente.Citta = Me.ReferenteMemorizzato.Citta
                    Me.Referente.LocalitaEstera = Me.ReferenteMemorizzato.LocalitaEstera
                    Me.Referente.Indirizzo = Me.ReferenteMemorizzato.Indirizzo

                    Session("ReferenteInserito") = Me.Referente
                Else
                    Session("ReferenteInserito") = referente
                End If
            Else
                Session("ReferenteInserito") = referente
            End If

            Me.ReferenteTrovato = Nothing
            Me.Referente = Nothing
            Me.Referenti = Nothing
            Me.ReferenteMemorizzato = Nothing

            ParsecUtility.Utility.CloseRadWindowAndUpadateParent(True)

        Else

            ParsecUtility.Utility.MessageBox("E' necessario specificare il campo Denom./Cognome!", False)

        End If

    End Sub

    'Chiude il Pannello
    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        ParsecUtility.Utility.CloseRadWindow(True)
    End Sub

    'Evento che fa scattare la ricerca del Comune. Una volta trovato e se confermato scatta il metodo AggiornaComuneImageButton che aggiorna.
    Protected Sub TrovaComuneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaComuneImageButton.Click
        Dim search As Boolean = True
        If Not String.IsNullOrEmpty(Me.CittaTextBox.Text.Trim) Then
            Dim comuni As New ParsecAdmin.ComuniUrbaniRepository
            Dim res = comuni.GetQuery.Where(Function(c) c.Descrizione.Trim.ToLower = Me.CittaTextBox.Text.Trim.ToLower).ToList
            If res.Count = 1 Then
                Dim comune As ParsecAdmin.ComuniUrbani = res.FirstOrDefault
                Me.CittaTextBox.Text = comune.Descrizione
                Me.CapTextBox.Text = comune.CAP
                Me.ProvinciaTextBox.Text = comune.Provincia
                comune = Nothing
                search = False
            End If
            comuni.Dispose()
        End If
        If search Then
            Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaComuniPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaComuneImageButton.ClientID)
            queryString.Add("mode", CInt(Rnd()))
            queryString.Add("filtro", Me.CittaTextBox.Text.Trim)
            ParsecUtility.Utility.ShowPopup(pageUrl, 800, 450, queryString, False)
        End If
    End Sub

    'Svuola i campi relativi a Citta, Cap e Provincia
    Protected Sub EliminaComuneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaComuneImageButton.Click
        Me.CittaTextBox.Text = String.Empty
        Me.CapTextBox.Text = String.Empty
        Me.ProvinciaTextBox.Text = String.Empty
    End Sub

    'Evento che aggiorna il Comune selezionata dallo maschera di Ricerca. Invocato da TrovaComuneImageButton.Click
    Protected Sub AggiornaComuneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaComuneImageButton.Click
        If Not ParsecUtility.SessionManager.ComuneUrbano Is Nothing Then
            Dim comune As ParsecAdmin.ComuniUrbani = ParsecUtility.SessionManager.ComuneUrbano
            Me.CittaTextBox.Text = comune.Descrizione
            Me.CapTextBox.Text = comune.CAP
            Me.ProvinciaTextBox.Text = comune.Provincia
            ParsecUtility.SessionManager.ComuneUrbano = Nothing
            comune = Nothing
        End If
    End Sub

#End Region


#Region "METODI PRIVATI"

    'Fa uscire il messaggio di conferma nel caso in cui il referente esterno corrente corrisponda ad un referente già presente in archivio.
    Private Sub ExecuteConfirm(ByVal message As String, ByVal controlId As String)
        Dim page As Page = CType(HttpContext.Current.Handler, Page)
        Dim script As New Text.StringBuilder
        message = Replace(message, vbCrLf, "\n")
        message = Replace(message, "'", "\'")
        message = Replace(message, """", " \""")
        script.AppendLine("<script language='javascript'>")

        script.AppendLine(";(function() {")
        script.AppendLine("     if (window.confirm(""" & message & """)){")
        script.AppendLine("         document.getElementById('" & controlId & "').click();")
        script.AppendLine("     }")
        script.AppendLine("})()")

        script.AppendLine("</script>")

        page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "ConfirmReplace", script.ToString)

    End Sub

    ''Metodo di conferma dell'operazione.
    'Private Sub Confirm44(ByVal message As String, ByVal controlId As String)
    '    Dim page As Page = CType(HttpContext.Current.Handler, Page)
    '    Dim script As New Text.StringBuilder
    '    message = Replace(message, vbCrLf, "\n")
    '    message = Replace(message, "'", "\'")
    '    message = Replace(message, """", " \""")
    '    script.AppendLine("<script language='javascript'>")

    '    script.AppendLine("function Confirm() {")
    '    script.AppendLine("     if (window.confirm(""" & message & """)){")
    '    script.AppendLine("         document.getElementById('" & controlId & "').click();")
    '    script.AppendLine("     }")
    '    script.AppendLine("}")
    '    script.AppendLine("Confirm();")

    '    script.AppendLine("</script>")

    '    page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "Confirm", script.ToString)

    'End Sub

    'Effettua una copia del Refrente e la restituisce.
    Private Function GetDeepCopyReferente(ByVal referente As ParsecPro.IReferente) As ParsecPro.IReferente
        Dim copy As ParsecPro.IReferente = Nothing

        If TypeOf referente Is ParsecPro.Mittente Then
            copy = New ParsecPro.Mittente
        Else
            copy = New ParsecPro.Destinatario
        End If

        copy.Cap = referente.Cap
        copy.Citta = referente.Citta
        copy.Codice = referente.Codice
        copy.CodiceFiscalePartitaIva = referente.CodiceFiscalePartitaIva
        copy.CodiceIPA = referente.CodiceIPA
        copy.Cognome = referente.Cognome
        'copy.Descrizione = referente.Descrizione
        copy.Email = referente.Email
        copy.Esistente = referente.Esistente
        copy.Fax = referente.Fax
        copy.Id = referente.Id
        copy.Indirizzo = referente.Indirizzo
        copy.Interno = referente.Interno
        copy.InviaEmail = referente.InviaEmail
        copy.Iter = referente.Iter
        copy.LivelloGerarchiaIPA = referente.LivelloGerarchiaIPA
        copy.LocalitaEstera = referente.LocalitaEstera
        copy.NodoIpaXml = referente.NodoIpaXml
        copy.Nome = referente.Nome
        copy.PerConoscenza = referente.PerConoscenza
        copy.Provincia = referente.Provincia
        copy.Rubrica = referente.Rubrica
        copy.Telefono = referente.Telefono
        copy.UfficioBilancio = referente.UfficioBilancio
        copy.Tipologia = referente.Tipologia

        Return copy
    End Function

    'Verifica se il referente corrente è stato modificato rispetto a quello memorizzato.
    Private Function IsReferenteModificato(ByVal referenteCorrente As ParsecPro.IReferente, ByVal referenteMemorizzato As ParsecPro.IReferente) As Boolean

        If referenteCorrente.Tipologia <> referenteMemorizzato.Tipologia Then
            Return True
        End If
        If referenteCorrente.Rubrica <> referenteMemorizzato.Rubrica Then
            Return True
        End If
        If referenteCorrente.Cognome <> referenteMemorizzato.Cognome Then
            Return True
        End If
        If referenteCorrente.Nome <> referenteMemorizzato.Nome Then
            Return True
        End If
        If referenteCorrente.Email <> referenteMemorizzato.Email Then
            Return True
        End If
        If referenteCorrente.Provincia <> referenteMemorizzato.Provincia Then
            Return True
        End If
        If referenteCorrente.Citta <> referenteMemorizzato.Citta Then
            Return True
        End If
        If referenteCorrente.Cap <> referenteMemorizzato.Cap Then
            Return True
        End If
        If referenteCorrente.Indirizzo <> referenteMemorizzato.Indirizzo Then
            Return True
        End If
        If referenteCorrente.LocalitaEstera <> referenteMemorizzato.LocalitaEstera Then
            Return True
        End If

        Return False

    End Function

    'Metodo che controlla se il referente corrente è uguale oppure no al referente memorizzato.
    Private Function IsReferentiUguali(ByVal referenteCorrente As ParsecPro.IReferente, ByVal referenteMemorizzato As ParsecPro.IReferente) As Boolean

        If referenteCorrente.Id >= 0 AndAlso Not referenteMemorizzato Is Nothing Then

            If Not String.Equals(If(referenteCorrente.Cognome, String.Empty), If(referenteMemorizzato.Cognome, String.Empty), StringComparison.InvariantCultureIgnoreCase) Then
                Return False
            End If

            If Not String.Equals(If(referenteCorrente.Nome, String.Empty), If(referenteMemorizzato.Nome, String.Empty), StringComparison.InvariantCultureIgnoreCase) Then
                Return False
            End If

            If Not String.Equals(If(referenteCorrente.Email, String.Empty), If(referenteMemorizzato.Email, String.Empty), StringComparison.InvariantCultureIgnoreCase) Then
                Return False
            End If

            If Not String.Equals(If(referenteCorrente.Citta, String.Empty), If(referenteMemorizzato.Citta, String.Empty), StringComparison.InvariantCultureIgnoreCase) Then
                Return False
            End If

            If Not String.Equals(If(referenteCorrente.Indirizzo, String.Empty), If(referenteMemorizzato.Indirizzo, String.Empty), StringComparison.InvariantCultureIgnoreCase) Then
                Return False
            End If

            If Not String.Equals(If(referenteCorrente.Provincia, String.Empty), If(referenteMemorizzato.Provincia, String.Empty), StringComparison.InvariantCultureIgnoreCase) Then
                Return False
            End If

            If Not String.Equals(If(referenteCorrente.LocalitaEstera, String.Empty), If(referenteMemorizzato.LocalitaEstera, String.Empty), StringComparison.InvariantCultureIgnoreCase) Then
                Return False
            End If

            If Not String.Equals(If(referenteCorrente.Cap, String.Empty), If(referenteMemorizzato.Cap, String.Empty), StringComparison.InvariantCultureIgnoreCase) Then
                Return False
            End If
        Else
            Return False
        End If

        Return True
    End Function

    'Cerca io referente memorizzato su DB in base all'oggetto referenteCorrente passato come parametro.
    Private Function GetReferente(ByVal referenteCorrente As ParsecPro.IReferente, ByVal referenteMemorizzato As ParsecPro.IReferente) As ParsecPro.IReferente
        'LA RICERCA IGNORA LA DIFFERENZA TRA MAIUSCOLE E MINUSCOLE, NON TIENE CONTO DEGLI SPAZI INIZIALI E FINALI, IGNORA LA DIFFERENZA TRA VALORI NULLI E VUOTI

        Dim referente As ParsecPro.IReferente = Nothing
        Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = Nothing
        Dim res As Boolean = False

        Dim cognome As String = Nothing
        If Not String.IsNullOrEmpty(referenteCorrente.Cognome) Then
            cognome = referenteCorrente.Cognome.Trim.ToLower
        End If

        Dim nome As String = Nothing
        If Not String.IsNullOrEmpty(referenteCorrente.Nome) Then
            nome = referenteCorrente.Nome.Trim.ToLower
        End If

        Dim email As String = Nothing
        If Not String.IsNullOrEmpty(referenteCorrente.Email) Then
            email = referenteCorrente.Email.Trim.ToLower
        End If

        Dim citta As String = Nothing
        If Not String.IsNullOrEmpty(referenteCorrente.Citta) Then
            citta = referenteCorrente.Citta.Trim.ToLower
        End If

        Dim indirizzo As String = Nothing
        If Not String.IsNullOrEmpty(referenteCorrente.Indirizzo) Then
            indirizzo = referenteCorrente.Indirizzo.Trim.ToLower
        End If

        Dim provincia As String = Nothing
        If Not String.IsNullOrEmpty(referenteCorrente.Provincia) Then
            provincia = referenteCorrente.Provincia.Trim.ToLower
        End If

        Dim localitaEstera As String = Nothing
        If Not String.IsNullOrEmpty(referenteCorrente.LocalitaEstera) Then
            localitaEstera = referenteCorrente.LocalitaEstera.Trim.ToLower
        End If

        Dim cap As String = Nothing
        If Not String.IsNullOrEmpty(referenteCorrente.Cap) Then
            cap = referenteCorrente.Cap.Trim.ToLower
        End If

        Dim rubrica As New ParsecAdmin.RubricaRepository


        Dim view = rubrica.Where(Function(c) c.LogStato Is Nothing)

        If referenteCorrente.Id > 0 Then
            view = view.Where(Function(c) c.Id <> referenteCorrente.Id)
        End If

        view = view.Where(Function(c) If(Not String.IsNullOrEmpty(c.Denominazione), c.Denominazione.Trim.ToLower = cognome, c.Denominazione = cognome))

        If nome Is Nothing Then
            view = view.Where(Function(c) c.Nome Is Nothing OrElse c.Nome = String.Empty)
        Else
            view = view.Where(Function(c) If(Not String.IsNullOrEmpty(c.Nome), c.Nome.Trim.ToLower = nome, c.Nome = nome))
        End If

        If email Is Nothing Then
            view = view.Where(Function(c) c.Email Is Nothing OrElse c.Email = String.Empty)
        Else
            view = view.Where(Function(c) If(Not String.IsNullOrEmpty(c.Email), c.Email.Trim.ToLower = email, c.Email = email))
        End If

        If indirizzo Is Nothing Then
            view = view.Where(Function(c) c.Indirizzo Is Nothing OrElse c.Indirizzo = String.Empty)
        Else
            view = view.Where(Function(c) If(Not String.IsNullOrEmpty(c.Indirizzo), c.Indirizzo.Trim.ToLower = indirizzo, c.Indirizzo = indirizzo))
        End If

        If cap Is Nothing Then
            view = view.Where(Function(c) c.CAP Is Nothing OrElse c.CAP = String.Empty)
        Else
            view = view.Where(Function(c) If(Not String.IsNullOrEmpty(c.CAP), c.CAP.Trim.ToLower = cap, c.CAP = cap))
        End If

        If citta Is Nothing Then
            view = view.Where(Function(c) c.Comune Is Nothing OrElse c.Comune = String.Empty)
        Else
            view = view.Where(Function(c) If(Not String.IsNullOrEmpty(c.Comune), c.Comune.Trim.ToLower = citta, c.Comune = citta))
        End If

        If provincia Is Nothing Then
            view = view.Where(Function(c) c.Provincia Is Nothing OrElse c.Provincia = String.Empty)
        Else
            view = view.Where(Function(c) If(Not String.IsNullOrEmpty(c.Provincia), c.Provincia.Trim.ToLower = provincia, c.Provincia = provincia))
        End If


        If localitaEstera Is Nothing Then
            view = view.Where(Function(c) c.LocalitaEstera Is Nothing OrElse c.LocalitaEstera = String.Empty)
        Else
            view = view.Where(Function(c) If(Not String.IsNullOrEmpty(c.LocalitaEstera), c.LocalitaEstera.Trim.ToLower = localitaEstera, c.LocalitaEstera = localitaEstera))
        End If


        res = view.Count > 0

        If res Then


            Dim struttureEsterneInRubrica = view.Where(Function(c) c.InRubrica = True)
            Dim n = struttureEsterneInRubrica.Count
            'SE I REFERENTI DUPLICATI NON SONO IN RUBRICA 
            If n = 0 Then
                strutturaEsterna = view.Where(Function(c) c.InRubrica = False).OrderByDescending(Function(c) c.Id).FirstOrDefault
            Else
                strutturaEsterna = view.Where(Function(c) c.InRubrica = True).OrderByDescending(Function(c) c.Id).FirstOrDefault
            End If


            If Not strutturaEsterna Is Nothing Then
                If TypeOf referenteCorrente Is ParsecPro.Mittente Then
                    referente = New ParsecPro.Mittente
                Else
                    referente = New ParsecPro.Destinatario
                End If

                referente.Id = strutturaEsterna.Id

                referente.Cap = strutturaEsterna.CAP
                referente.Citta = strutturaEsterna.Comune
                referente.Codice = strutturaEsterna.Codice
                referente.CodiceFiscalePartitaIva = strutturaEsterna.CodiceFiscale
                referente.CodiceIPA = strutturaEsterna.CodiceIPA
                referente.Cognome = strutturaEsterna.Denominazione
                referente.Email = strutturaEsterna.Email
                referente.Esistente = True
                referente.Fax = strutturaEsterna.Fax
                referente.Indirizzo = strutturaEsterna.Indirizzo
                referente.Interno = False
                referente.LivelloGerarchiaIPA = strutturaEsterna.LivelloGerarchiaIPA
                referente.LocalitaEstera = strutturaEsterna.LocalitaEstera
                referente.NodoIpaXml = strutturaEsterna.InfoXML
                referente.Nome = strutturaEsterna.Nome
                referente.PerConoscenza = False
                referente.Provincia = strutturaEsterna.Provincia
                referente.Rubrica = strutturaEsterna.InRubrica
                referente.Telefono = strutturaEsterna.Telefono
                referente.Tipologia = strutturaEsterna.Tipologia

            End If

        End If

        rubrica.Dispose()

        Return referente
    End Function

    'Riempie i campi della maschera in base al referente passato come parametro
    Private Sub AggiornaVista(ByVal referente As ParsecPro.IReferente)
        Me.DenominazioneTextBox.Text = referente.Cognome
        Me.NomeTextBox.Text = referente.Nome
        Me.IndirizzoTextBox.Text = referente.Indirizzo
        Me.CapTextBox.Text = referente.Cap
        Me.CittaTextBox.Text = referente.Citta
        Me.ProvinciaTextBox.Text = referente.Provincia
        Me.RubricaCheckBox.Checked = referente.Rubrica
        Me.EmailTextBox.Text = referente.Email
        Me.RubricaCheckBox.Checked = referente.Rubrica
        Me.LocalitaEsteraTextBox.Text = referente.LocalitaEstera

        Me.TipoPersonaComboBox.Items.FindItemByValue(referente.Tipologia).Selected = True

    End Sub

    'Restituisce il referete memorizzato in base alla variabile di Sessione Me.Referente
    Private Function GetReferenteMemorizzato() As ParsecPro.IReferente

        Dim referenteMemorizzato As ParsecPro.IReferente = Nothing

        If Not Me.Referente Is Nothing Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim referenteCorrente = rubrica.Where(Function(c) c.Id = Me.Referente.Id).FirstOrDefault
            rubrica.Dispose()


            If Not referenteCorrente Is Nothing Then

                If TypeOf Me.Referente Is ParsecPro.Mittente Then
                    referenteMemorizzato = New ParsecPro.Mittente
                Else
                    referenteMemorizzato = New ParsecPro.Destinatario
                End If

                referenteMemorizzato.Cognome = referenteCorrente.Denominazione
                referenteMemorizzato.Nome = referenteCorrente.Nome
                referenteMemorizzato.Email = referenteCorrente.Email
                referenteMemorizzato.Indirizzo = referenteCorrente.Indirizzo
                referenteMemorizzato.Cap = referenteCorrente.CAP
                referenteMemorizzato.Provincia = referenteCorrente.Provincia
                referenteMemorizzato.Citta = referenteCorrente.Comune
                referenteMemorizzato.LocalitaEstera = referenteCorrente.LocalitaEstera
                referenteMemorizzato.Id = referenteCorrente.Id
                referenteMemorizzato.Tipologia = referenteCorrente.Tipologia
                referenteMemorizzato.Rubrica = referenteCorrente.InRubrica
            End If

        End If

        Return referenteMemorizzato

    End Function



#End Region

End Class