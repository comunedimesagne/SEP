Imports System.Data.SqlClient
Imports System.Data

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StampaTrasmissioniAttiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    'Variabile di Sessione
    Public Property Protocolli() As List(Of ParsecPro.Registrazione)
        Get
            Return CType(Session("StampaTrasmissioniAttiPage_Protocolli"), List(Of ParsecPro.Registrazione))
        End Get
        Set(ByVal value As List(Of ParsecPro.Registrazione))
            Session("StampaTrasmissioniAttiPage_Protocolli") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> Stampa Lista Trasmissione Atti"

        If Not Me.Page.IsPostBack Then
            Me.ResettaFiltro()
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If

        Dim script As New StringBuilder
        script.AppendLine("var value =  $find('" & Me.NumeroProtocolloInizioTextBox.ClientID & "').get_displayValue(); var textbox =  $find('" & Me.NumeroProtocolloFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")
        Me.NumeroProtocolloInizioTextBox.Attributes.Add("onblur", script.ToString)

    End Sub

    'Evento PreRender associato alla Pagina. Setta il titolo della griglia
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
       Me.PannelloRisultatiLabel.Text = "Elenco Registrazioni&nbsp;&nbsp;&nbsp;" & If(Me.RegistrazioniListBox.Items.Count > 0, "( " & Me.RegistrazioniListBox.Items.Count.ToString & " )", "")
    End Sub


#End Region

#Region "METODI PRIVATI"

    'Costruisce il Filtro per la ricerca e lo restituisce.
    Private Function GetFiltro() As ParsecPro.RegistrazioneFiltro

        Dim annullata As Nullable(Of Boolean) = Nothing

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

        Dim idStruttura As Nullable(Of Int32) = Nothing
        If Not String.IsNullOrEmpty(Me.IdReferenteTextBox.Text) Then
            idStruttura = Convert.ToInt32(Me.IdReferenteTextBox.Text)
        End If

        Dim nullInt As Nullable(Of Int32) = Nothing

        Dim nullData As Nullable(Of DateTime) = Nothing

        Dim filtro As New ParsecPro.RegistrazioneFiltro With
           {
               .Arrivo = Me.TipoRegistrazioneRadioList.SelectedValue = 0,
               .Partenza = Me.TipoRegistrazioneRadioList.SelectedValue = 1,
               .Interna = Me.TipoRegistrazioneRadioList.SelectedValue = 2,
               .Annullata = annullata,
               .NumeroProtocolloInizio = numeroProtocolloInizio,
               .NumeroProtocolloFine = numeroProtocolloFine,
               .DataProtocolloInizio = Me.DataProtocolloInizioTextBox.SelectedDate,
               .DataProtocolloFine = Me.DataProtocolloFineTextBox.SelectedDate,
               .ReferenteEsternoDenominazione = String.Empty,
               .ReferenteEsternoNome = String.Empty,
               .ReferenteEsternoCitta = String.Empty,
               .ReferenteEsternoEmail = String.Empty,
               .IdStruttura = idStruttura,
               .StrutturaCompleta = Me.StrutturaCompletaCheckBox.Checked,
               .Oggetto = String.Empty,
               .IdClassificazione = nullInt,
               .ClassificazioneCompleta = False,
               .NumeroRiscontro = nullInt,
               .AnnoRiscontro = nullInt,
               .ProtocolloMittente = String.Empty,
               .Note = String.Empty,
               .NoteInterne = String.Empty,
               .IdTipoDocumento = nullInt,
               .IdTipoRicezioneInvio = nullInt,
               .DataDocumentoInizio = nullData,
               .DataDocumentoFine = nullData,
               .DataRicezioneInvioInizio = nullData,
               .DataRicezioneInvioFine = nullData,
               .IdUtenteCollegato = utenteCorrente.Id,
               .IdUtenteInserimento = nullInt
           }

        Return filtro
    End Function

    'Conversione della lista in DataTable
    Private Function ConvertToDataTable(Of TSource)(ByVal source As IEnumerable(Of TSource)) As DataTable
        Dim props = GetType(TSource).GetProperties()
        Dim dt = New DataTable()
        dt.Columns.AddRange(props.Select(Function(p) New DataColumn(p.Name, p.PropertyType)).ToArray())
        source.ToList().ForEach(Function(i) dt.Rows.Add(props.Select(Function(p) p.GetValue(i, Nothing)).ToArray()))
        Return dt
    End Function

    'Convalida i parametri impostati nella Pagina. Richiamto da AnteprimaStampaButton.Click e da FiltraImageButton_Click
    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean

        If String.IsNullOrEmpty(Trim(Me.IdReferenteTextBox.Text)) Then
            message.AppendLine("E' necessario specificare un referente!")
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
        'Se la data iniziale è maggiore di quella finale.
        If Date.Compare(Me.DataProtocolloInizioTextBox.SelectedDate, Me.DataProtocolloFineTextBox.SelectedDate) > 0 Then
            message.AppendLine("La data di protocollo 'da' deve essere antecedente alla data di protocollo 'a'!")
        End If
        Return Not message.Length > 0
    End Function

    'Resetta i campi della Pagina.
    Private Sub ResettaFiltro()
        Me.Protocolli = New List(Of ParsecPro.Registrazione)
        'Tutte le registrazioni di oggi
        Me.DataProtocolloInizioTextBox.SelectedDate = Now
        Me.DataProtocolloFineTextBox.SelectedDate = Now
        Me.NumeroProtocolloInizioTextBox.Text = String.Empty
        Me.NumeroProtocolloFineTextBox.Text = String.Empty
        Me.SelectAllCheckBox.Checked = False
        Me.ReferenteTextBox.Text = String.Empty
        Me.IdReferenteTextBox.Text = String.Empty
        Me.TipoRegistrazioneRadioList.SelectedValue = 0
        Me.StrutturaCompletaCheckBox.Checked = False


        Me.RegistrazioniListBox.DataSource = Me.Protocolli
        Me.RegistrazioniListBox.DataValueField = "Id"
        Me.RegistrazioniListBox.DataTextField = "NumeroFormattato"
        Me.RegistrazioniListBox.DataBind()
    End Sub

    'Applica il filtro nella Ricerca
    Private Sub ApplicaFiltro()
        Me.Protocolli.Clear()

        Dim filtro As ParsecPro.RegistrazioneFiltro = Me.GetFiltro
        Dim protocolli As New ParsecPro.RegistrazioniRepository

        Dim res = protocolli.GetView(filtro)
        Dim descrizione As String = String.Empty
        Dim tipologie As New ParsecPro.TipiRicezioneInvioRepository

        For Each p In res
            descrizione = String.Empty
            If p.IdTipoRicezione.HasValue Then
                Dim idTipologia As Integer = CInt(p.IdTipoRicezione)
                descrizione = tipologie.Where(Function(c) c.Id = idTipologia).Select(Function(c) c.Descrizione).FirstOrDefault
            End If

            p.Note = "NOTE: " & p.Note
            p.DescrizioneTipoRicezioneInvio = descrizione
            p.NumeroFormattato = p.NumeroProtocollo.ToString.PadLeft(7, "0") & " " & String.Format("{0:dd-MM-yyyy}", p.DataImmissione)

            Me.Protocolli.Add(p)
        Next

        If Me.Protocolli.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Nessuna registrazione trovata con i criteri di filtro impostati!", False)
        End If

        Me.RegistrazioniListBox.DataSource = Me.Protocolli
        Me.RegistrazioniListBox.DataValueField = "Id"
        Me.RegistrazioniListBox.DataTextField = "NumeroFormattato"
        Me.RegistrazioniListBox.DataBind()

        protocolli.Dispose()

        Me.SelectAllCheckBox.Checked = True
        Dim checked As Boolean = Me.SelectAllCheckBox.Checked
        For i As Integer = 0 To Me.RegistrazioniListBox.Items.Count - 1
            Me.RegistrazioniListBox.Items(i).Checked = checked
        Next

    End Sub

#End Region

#Region "EVENTI CONTROLLI PAGINA"

    'Evento click per l'anteprima di stampa
    Protected Sub AnteprimaStampaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnteprimaStampaButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Me.Print()
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    'Seleziona o Deseleziona tutti i check contenuti nella lista RegistrazioniListBox
    Protected Sub SelectAllCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectAllCheckBox.CheckedChanged
        Dim checked As Boolean = Me.SelectAllCheckBox.Checked
        For i As Integer = 0 To Me.RegistrazioniListBox.Items.Count - 1
            'Me.RegistrazioniListBox.Items(i).Selected = checked
            Me.RegistrazioniListBox.Items(i).Checked = checked
        Next
    End Sub

    'Resetta i campi della Maschera e quindi, come conseguenza, i filtri di ricerca
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

    'Effettua la ricerca in base ai Filtri.
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Me.ApplicaFiltro()
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

#End Region

#Region "GESTIONE REFERENTE"

    'Fa partire la Ricerca del Referente tramite la maschera RicercaOrganigrammaPage.aspx
    Protected Sub TrovaReferenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaReferenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaReferenteImageButton.ClientID)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    'Aggiorna il Responsabile selezionato. Metodo che scatta al ritorno della procedura fatta partire da TrovaReferenteImageButton_Click
    Protected Sub AggiornaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaReferenteImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Me.ReferenteTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdReferenteTextBox.Text = struttureSelezionate.First.Id.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    'Resetta i campi ReferenteTextBox e IdReferenteTextBox
    Protected Sub EliminaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaReferenteImageButton.Click
        Me.ReferenteTextBox.Text = String.Empty
        Me.IdReferenteTextBox.Text = String.Empty
    End Sub

#End Region

#Region "SCRIPT PARSECOPENOFFICE"

    'Registra l'OpenOffice nella pagina per poterlo utilizzare
    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

    'Stampa: utilizza il Template "templateTrasmissioneProtocolli.odt"
    Private Sub Print()

        Dim template As String = "templateTrasmissioneProtocolli.odt"
        Dim localPathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")
        Dim input As String = localPathTemplate & template

        If Not IO.File.Exists(input) Then
            Throw New ApplicationException(String.Format("Il file '{0}' non esiste!", template))
        End If

        Dim annoEsercizio As Integer = Now.Year
        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")
        Dim pathCartellaAnno As String = String.Format("{0}{1}\", localPath, annoEsercizio)

        If Not IO.Directory.Exists(localPath) Then
            IO.Directory.CreateDirectory(localPath)
        End If

        If Not IO.Directory.Exists(pathCartellaAnno) Then
            IO.Directory.CreateDirectory(pathCartellaAnno)
        End If


        Dim listaId As New List(Of Integer)
        Dim protocolliSelezionati As New List(Of ParsecPro.Protocollo)
        For Each it As Telerik.Web.UI.RadListBoxItem In Me.RegistrazioniListBox.CheckedItems
            listaId.Add(CInt(it.Value))
        Next

        If listaId.Count > 0 Then

            Dim remotePath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti") & annoEsercizio.ToString & "/"
            Dim remotePathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates")

            Dim openOfficeParameters As New ParsecAdmin.OpenOfficeParameters


            Dim c As Integer = 0
            Dim res = From p In Me.Protocolli
                 Where listaId.Contains(p.Id)
                 Select New With {
                     .Progressivo = System.Math.Max(System.Threading.Interlocked.Increment(c), c - 1),
                     .NumeroProtocollo = p.NumeroProtocollo.ToString.PadLeft(7, "0"),
                     .DataProtocollo = p.DataImmissione.Value.ToShortDateString,
                     .MittDest = p.ElencoReferentiEsterni,
                     .Oggetto = p.Oggetto,
                     .uff = p.ElencoReferentiInterni,
                     .TipologiaRicezioneInvio = p.DescrizioneTipoRicezioneInvio
                     }


            Dim table = ConvertToDataTable(res)
            table.TableName = "TabellaProtocolli"

            Dim parametri As New Hashtable
            parametri.Add("Destinatario", Me.ReferenteTextBox.Text)
            parametri.Add("Struttura", ParsecAdmin.WebConfigSettings.GetKey("DescrizioneTrasmissioneAtti"))
            parametri.Add("DalGiorno", String.Format("{0:dd/MM/yyyy}", Me.DataProtocolloInizioTextBox.SelectedDate))
            parametri.Add("AlGiorno", String.Format("{0:dd/MM/yyyy}", Me.DataProtocolloFineTextBox.SelectedDate))

            Dim datiInput As New ParsecAdmin.DatiInput With {
                .SrcRemotePath = remotePathTemplate & template,
                .DestRemotePath = remotePath & "Report.odt",
                .ShowWindow = True,
                .Enabled = True,
                .FunctionName = "ProcessDocument"}

            Dim data As String = openOfficeParameters.CreateDataSource(datiInput, New ParsecAdmin.DatiCampiUtente(parametri), table)

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            'UTILIZZO L'APPLET
            If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then

                ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(data, "", True, False)
            Else
                'UTILIZZO IL SOCKET
                ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Nothing)
            End If

        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno una registrazione!", False)
        End If
    End Sub

#End Region

End Class