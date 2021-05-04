Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class LetteraTrasmissionePage
    Inherits System.Web.UI.Page

    Public Enum LetteraTrasmissione
        CapiGruppo = 9
        Revisori = 10
    End Enum


    Private WithEvents MainPage As MainPage

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadListBox .rlbCheck,.RadListBox .rlbImage,.RadListBox .rlbText{vertical-align: middle;padding-left:5px;}"
        Me.Page.Header.Controls.Add(css)


        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Stampa Lettera Trasmissione"
        If Not Me.Page.IsPostBack Then
            Me.Documenti = New List(Of ParsecAtt.Documento)
            Me.CaricaTipologieSeduta()
            Me.CaricaPublicazioniWeb()
            Me.CaricaPublicazioniAlbo()
            Me.CaricaModelli()
            Me.ResettaFiltro()
            Me.SelectAllCheckBox.Enabled = False
            Me.StampaLetteraRevisoriButton.Enabled = False
            Me.StampaLetteraCapigruppoButton.Enabled = False
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If

        Me.ContatoreGeneraleInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.ContatoreGeneraleInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.ContatoreGeneraleFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.PannelloRisultatiLabel.Text = "Elenco Delibere&nbsp;&nbsp;&nbsp;" & If(Me.AttiListBox.Items.Count > 0, "( " & Me.AttiListBox.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "PROPRIETA'"

    Public Property Documenti() As List(Of ParsecAtt.Documento)
        Get
            Return CType(Session("LetteraTrasmissionePage_Documenti"), List(Of ParsecAtt.Documento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Documento))
            Session("LetteraTrasmissionePage_Documenti") = value
        End Set
    End Property

#End Region


#Region "EVENTI CONTROLLI"

    Protected Sub StampaLetteraCapigruppoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaLetteraCapigruppoButton.Click
        Try
            Me.GeneraDataSourceLetteraTrasmissione(LetteraTrasmissione.CapiGruppo)
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message.ToString, False)
        End Try
    End Sub

    Protected Sub StampaLetteraRevisoriButton_Click(sender As Object, e As System.EventArgs) Handles StampaLetteraRevisoriButton.Click
        Try
            Me.GeneraDataSourceLetteraTrasmissione(LetteraTrasmissione.Revisori)
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message.ToString, False)
        End Try
    End Sub

    Protected Sub PubblicazioneAlboComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles PubblicazioneAlboComboBox.SelectedIndexChanged
        If e.Value = "0" Then
            Me.DataPubblicazioneInizioTextBox.Enabled = False
            Me.DataPubblicazioneFineTextBox.Enabled = False
            Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
            Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing
        Else
            Me.DataPubblicazioneInizioTextBox.Enabled = True
            Me.DataPubblicazioneFineTextBox.Enabled = True
        End If
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Try
                Me.ApplicaFiltro()
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    Protected Sub TrovaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaSettoreImageButton.Click
        Me.TrovaSettore()
    End Sub

    Protected Sub AggiornaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSettoreImageButton.Click
        Me.AggiornaSettore()
    End Sub

    Protected Sub EliminaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaSettoreImageButton.Click
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
    End Sub

    Protected Sub TrovaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUfficioImageButton.Click
        Me.TrovaUfficio()
    End Sub

    Protected Sub AggiornaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUfficioImageButton.Click
        Me.AggiornaUfficio()
    End Sub

    Protected Sub EliminaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUfficioImageButton.Click
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
    End Sub

    Protected Sub TipologieSedutaComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieSedutaComboBox.SelectedIndexChanged
        Me.CaricaModelli()
    End Sub

    Protected Sub notificaOperazioneButton_Click(sender As Object, e As System.EventArgs) Handles notificaOperazioneButton.Click
        Me.NotificaOperazione()
    End Sub

    Private Sub NotificaOperazione()
        Me.infoOperazioneHidden.Value = "Esportazione conclusa con successo!"
    End Sub


#End Region


#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

#Region "METODI PRIVATI"




    Private Sub ApplicaFiltro()

        Dim filtro As ParsecAtt.FiltroDocumento = Me.GetFiltro
        Dim documenti As New ParsecAtt.DocumentoRepository
        Me.Documenti = documenti.GetView(filtro)



        If Me.Documenti.Count = 0 Then
            Me.AttiListBox.Items.Clear()
            Me.StampaLetteraCapigruppoButton.Enabled = False
            Me.StampaLetteraRevisoriButton.Enabled = False
            Me.SelectAllCheckBox.Checked = False
            Me.SelectAllCheckBox.Enabled = False
            Throw New ApplicationException("Nessuna delibera trovata con i criteri di filtro impostati!")
        End If

        Me.AttiListBox.DataSource = Me.Documenti
        Me.AttiListBox.DataValueField = "Id"
        Me.AttiListBox.DataTextField = "Descrizione"
        Me.AttiListBox.DataBind()

        documenti.Dispose()

        Me.SelectAllCheckBox.Checked = True
        Dim checked As Boolean = Me.SelectAllCheckBox.Checked
        For i As Integer = 0 To Me.AttiListBox.Items.Count - 1
            Me.AttiListBox.Items(i).Checked = checked
        Next

        Me.SelectAllCheckBox.Enabled = True
        Me.StampaLetteraRevisoriButton.Enabled = True
        Me.StampaLetteraCapigruppoButton.Enabled = True


    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroDocumento
        Dim filtro As New ParsecAtt.FiltroDocumento

        filtro.DataDocumentoInizio = Me.DataDocumentoInizioTextBox.SelectedDate
        filtro.DataDocumentoFine = Me.DataDocumentoFineTextBox.SelectedDate

        filtro.DataPubblicazioneInizio = Me.DataPubblicazioneInizioTextBox.SelectedDate
        filtro.DataPubblicazioneFine = Me.DataPubblicazioneFineTextBox.SelectedDate

        filtro.DataSedutaInizio = Me.DataSedutaInizioTextBox.SelectedDate
        filtro.DataSedutaFine = Me.DataSedutaFineTextBox.SelectedDate

        If Me.ContatoreGeneraleInizioTextBox.Value.HasValue Then
            filtro.ContatoreGeneraleInizio = CInt(Me.ContatoreGeneraleInizioTextBox.Value)
        End If

        If Me.ContatoreGeneraleFineTextBox.Value.HasValue Then
            filtro.ContatoreGeneraleFine = CInt(Me.ContatoreGeneraleFineTextBox.Value)
        End If

        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            filtro.Oggetto = Me.OggettoTextBox.Text
        End If

        filtro.IdTipologiaDocumento = ParsecAtt.TipoDocumento.Delibera


        'TIPOLOGIA SEDUTA
        If Me.TipologieSedutaComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
        End If

        'UFFICIO
        'If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
        '    filtro.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        'Else
        filtro.Ufficio = Me.UfficioTextBox.Text.Trim
        'End If

        'SETTORE
        'If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
        '    filtro.IdSettore = CInt(Me.IdSettoreTextBox.Text)
        'Else
        filtro.Settore = Me.SettoreTextBox.Text.Trim
        'End If

        'PUBBLICATO MODULO MESSI
        If Me.PubblicazioneAlboComboBox.SelectedIndex > 0 Then
            filtro.PubblicazioneAlbo = CBool(Me.PubblicazioneAlboComboBox.SelectedValue)
        End If

        'PUBBLICABILE
        If Me.PubblicazioneWebComboBox.SelectedIndex > 0 Then
            filtro.PubblicazioneWeb = CBool(Me.PubblicazioneWebComboBox.SelectedValue)
        End If

        'MODELLO
        If Me.ModelliComboBox.SelectedIndex > 0 Then
            filtro.IdModello = CInt(Me.ModelliComboBox.SelectedValue)
        End If

        filtro.ApplicaAbilitazione = True

        Return filtro
    End Function

    Public Sub GeneraDataSourceLetteraTrasmissione(ByVal tipo As LetteraTrasmissione)

        Dim modelli As New ParsecAtt.ModelliRepository
        Dim idTipologia As Integer = CInt(tipo)
        Dim modello = modelli.GetQuery.Where(Function(c) c.IdTipologiaDocumento = idTipologia).FirstOrDefault
        If modello Is Nothing Then
            Throw New ApplicationException("Il template della lettera di trasmissione non è configurato!")
        End If

        Dim template As String = modello.FileName

        Dim localPathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")
        Dim input As String = localPathTemplate & template

        If Not IO.File.Exists(input) Then
            Throw New ApplicationException(String.Format("Il file '{0}' non esiste!", template))
        End If

        Dim listaId As List(Of Integer) = Me.AttiListBox.CheckedItems.Select(Function(c) CInt(c.Value)).ToList

        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters

        Dim remotePathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates")
        Dim remotePathReport As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocumentiTemp")



        Dim datiInput As New ParsecAdmin.DatiInput With {
             .SrcRemotePath = remotePathTemplate & template,
             .DestRemotePath = remotePathReport & "Report.odt",
             .ShowWindow = True,
             .Enabled = True,
             .FunctionName = "ProcessDocument"}

        Dim documentiSelezionati = Me.Documenti.Where(Function(c) listaId.Contains(c.Id))

        If documentiSelezionati.Count > 0 Then
            Dim dt As New DataTable("TabellaDocumenti")
            Dim row As DataRow = Nothing

            dt.Columns.Add(New DataColumn("ordDate", GetType(System.String)))
            dt.Columns.Add(New DataColumn("docContatoreGenerale", GetType(System.String)))
            dt.Columns.Add(New DataColumn("docOggetto", GetType(System.String)))
            dt.Columns.Add(New DataColumn("contatore", GetType(System.String)))

            Dim i As Integer = 1

            Dim pattern As String = "#[\u0000-\u00FF]+#"
            Dim rgx As New Regex(pattern)

            Dim parametri As New ParsecAdmin.ParametriRepository

            Dim abilitaOmissis As Boolean = False
            Dim omissis As String = "...OMISSIS..."
            If tipo = LetteraTrasmissione.CapiGruppo Then
                Dim parametro = parametri.GetByName("AbilitaOmissisCapiGruppo")
                If Not parametro Is Nothing Then
                    abilitaOmissis = CBool(parametro.Valore)
                End If
            Else
                Dim parametro = parametri.GetByName("AbilitaOmissisCapiGruppoRevisori")
                If Not parametro Is Nothing Then
                    abilitaOmissis = CBool(parametro.Valore)
                End If
            End If

            If abilitaOmissis Then
                omissis = parametri.GetByName("Omissis").Valore
            End If

            parametri.Dispose()

            For Each documento In documentiSelezionati
                row = dt.NewRow
                row("ordDate") = documento.DataConvocazione.Value.ToShortDateString
                row("docContatoreGenerale") = documento.ContatoreGenerale

                If abilitaOmissis Then
                    row("docOggetto") = rgx.Replace(documento.Oggetto, omissis)
                Else
                    row("docOggetto") = documento.Oggetto
                End If

                row("contatore") = i
                i += 1
                dt.Rows.Add(row)
            Next

            Dim data As String = openofficeParameters.CreateDataSource(datiInput, Nothing, dt)

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            'UTILIZZO L'APPLET
            If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(data, Me.notificaOperazioneButton.ClientID, True, False)
            Else
                'UTILIZZO IL SOCKET
                ParsecUtility.Utility.EseguiServerComunicatorService(data, True, AddressOf Me.NotificaOperazione)
            End If

        End If

    End Sub



    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean

        If Me.ContatoreGeneraleFineTextBox.Value.HasValue AndAlso Me.ContatoreGeneraleInizioTextBox.Value.HasValue Then
            If Me.ContatoreGeneraleInizioTextBox.Value > Me.ContatoreGeneraleFineTextBox.Value Then
                message.AppendLine("Il campo 'Registro Generale da' deve essere inferiore o uguale al campo 'Registro Generale a'!")
            End If
        End If

        'Se la data iniziale è maggiore di quella finale.
        If Me.DataDocumentoInizioTextBox.SelectedDate.HasValue AndAlso Me.DataDocumentoFineTextBox.SelectedDate.HasValue Then
            If Date.Compare(Me.DataDocumentoInizioTextBox.SelectedDate, Me.DataDocumentoFineTextBox.SelectedDate) > 0 Then
                message.AppendLine("Il campo 'Data Documento da' deve essere antecedente o uguale al campo 'Data Documento a'!")
            End If
        End If

        If Me.DataPubblicazioneInizioTextBox.SelectedDate.HasValue AndAlso Me.DataPubblicazioneFineTextBox.SelectedDate.HasValue Then
            If Date.Compare(Me.DataPubblicazioneInizioTextBox.SelectedDate, Me.DataPubblicazioneFineTextBox.SelectedDate) > 0 Then
                message.AppendLine("Il campo 'Data Pubblicazione da' deve essere antecedente o uguale al campo 'Data Pubblicazione a'!")
            End If
        End If


        Return Not message.Length > 0
        Return True
    End Function

    Private Sub ResettaFiltro()
        Me.DataDocumentoInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataDocumentoFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)

        Me.DataSedutaInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataSedutaFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)

        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

        Me.ContatoreGeneraleInizioTextBox.Text = String.Empty
        Me.ContatoreGeneraleFineTextBox.Text = String.Empty

        Me.OggettoTextBox.Text = String.Empty
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty

        Me.PubblicazioneAlboComboBox.SelectedIndex = 0

        Me.TipologieSedutaComboBox.SelectedIndex = 0
        Me.PubblicazioneWebComboBox.SelectedIndex = 0
        Me.ModelliComboBox.SelectedIndex = 0

        Me.AttiListBox.Items.Clear()

        Me.SelectAllCheckBox.Enabled = False
        Me.SelectAllCheckBox.Checked = False
        Me.StampaLetteraRevisoriButton.Enabled = False
        Me.StampaLetteraCapigruppoButton.Enabled = False
    End Sub


    Private Sub CaricaPublicazioniAlbo()
        Me.PubblicazioneAlboComboBox.Items.Add(New RadComboBoxItem("Pubblicate", "1"))
        Me.PubblicazioneAlboComboBox.Items.Add(New RadComboBoxItem("Non pubblicate", "0"))
        Me.PubblicazioneAlboComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0
    End Sub


    Private Sub CaricaPublicazioniWeb()
        Me.PubblicazioneWebComboBox.Items.Add(New RadComboBoxItem("Pubblicabili", "1"))
        Me.PubblicazioneWebComboBox.Items.Add(New RadComboBoxItem("Non pubblicabili", "0"))
        Me.PubblicazioneWebComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

    End Sub

    Private Sub CaricaModelli()
        Dim idTipologiaDocumento As Nullable(Of Integer) = Nothing
        Dim idTipologiaSeduta As Nullable(Of Integer) = Nothing
        idTipologiaDocumento = ParsecAtt.TipoDocumento.Delibera
        If Me.TipologieSedutaComboBox.SelectedIndex > 0 Then
            idTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
        End If
        If idTipologiaSeduta.HasValue OrElse idTipologiaDocumento.HasValue Then
            Me.CaricaModelli(idTipologiaDocumento, idTipologiaSeduta)
        End If
    End Sub

    Private Sub CaricaModelli(idTipologiaDocumento As Nullable(Of Integer), idTipologiaSeduta As Nullable(Of Integer))
        Dim modelli As New ParsecAtt.ModelliRepository
        Me.ModelliComboBox.DataValueField = "Id"
        Me.ModelliComboBox.DataTextField = "Descrizione"
        Me.ModelliComboBox.DataSource = modelli.GetKeyValue(New ParsecAtt.FiltroModello With {.TipologiaDocumento = idTipologiaDocumento, .Disabilitato = False, .IdTipologiaSeduta = idTipologiaSeduta})
        Me.ModelliComboBox.DataBind()
        Me.ModelliComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.ModelliComboBox.SelectedIndex = 0
        modelli.Dispose()
    End Sub

    Private Sub CaricaTipologieSeduta()
        Dim tipologieSedute As New ParsecAtt.TipologiaSedutaRepository
        Me.TipologieSedutaComboBox.DataValueField = "Id"
        Me.TipologieSedutaComboBox.DataTextField = "Descrizione"
        Me.TipologieSedutaComboBox.DataSource = tipologieSedute.GetKeyValue(Nothing)
        Me.TipologieSedutaComboBox.DataBind()
        Me.TipologieSedutaComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        tipologieSedute.Dispose()
    End Sub

    Private Sub AggiornaSettore()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim idSettore As Integer = struttureSelezionate.First.Id
            'Aggiorno il settore
            Me.SettoreTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdSettoreTextBox.Text = idSettore.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Private Sub TrovaSettore()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaSettoreImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 910, 670, queryString, False)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100")
        parametriPagina.Add("ultimoLivelloStruttura", "100")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    End Sub

    Private Sub AggiornaUfficio()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim idUfficio As Integer = struttureSelezionate.First.Id
            'Aggiorno l'ufficio
            Me.UfficioTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdUfficioTextBox.Text = idUfficio.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Private Sub TrovaUfficio()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUfficioImageButton.ClientID)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "200")
        parametriPagina.Add("ultimoLivelloStruttura", "200")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

#End Region


End Class