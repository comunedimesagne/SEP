Imports ParsecAdmin
Imports Telerik.Web.UI
Imports System.Web.Services


Partial Class RicercaFascicoloPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Public Property Fascicolo() As ParsecAdmin.Fascicolo
        Get
            Return CType(Session("RicercaFascicoloPage_Fascicolo"), ParsecAdmin.Fascicolo)
        End Get
        Set(ByVal value As ParsecAdmin.Fascicolo)
            Session("RicercaFascicoloPage_Fascicolo") = value
        End Set
    End Property

    Public Property Fascicoli() As Object
        Get
            Return CType(Session("RicercaFascicoloPage_Fascicoli"), Object)
        End Get
        Set(ByVal value As Object)
            Session("RicercaFascicoloPage_Fascicoli") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then
            Me.Fascicoli = Nothing
            Me.CaricaStati()
            Me.CaricaProcedimenti()
            Me.CaricaTipologiaFascicoli()
        End If
        ParsecUtility.Utility.CloseWindow(False)

        Me.NumeroRegistroInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.NumeroRegistroInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.NumeroRegistroFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub CaricaTipologiaFascicoli()

        Dim dati As New List(Of ParsecAdmin.KeyValue)
        dati.Add(New ParsecAdmin.KeyValue With {.Id = TipologiaFascicolo.ProcedimentoAmministrativo, .Descrizione = "Procedimento"})
        dati.Add(New ParsecAdmin.KeyValue With {.Id = TipologiaFascicolo.Affare, .Descrizione = "Affare"})
        Me.TipologiaFascicoloComboBox.DataSource = dati
        Me.TipologiaFascicoloComboBox.DataTextField = "Descrizione"
        Me.TipologiaFascicoloComboBox.DataValueField = "Id"
        Me.TipologiaFascicoloComboBox.DataBind()
        Me.TipologiaFascicoloComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.TipologiaFascicoloComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaStati()
        Me.StatoFascicoloComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Tutti", "0"))
        Me.StatoFascicoloComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Solo Aperti", "1"))
        Me.StatoFascicoloComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Solo Chiusi", "2"))
        Me.StatoFascicoloComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaProcedimenti()
        Dim procedimentoRepository As New ParsecAdmin.ProcedimentoRepository
        Me.ProcedimentoComboBox.DataSource = procedimentoRepository.GetView(Nothing)
        Me.ProcedimentoComboBox.DataTextField = "Nome"
        Me.ProcedimentoComboBox.DataValueField = "id"
        Me.ProcedimentoComboBox.DataBind()
        Me.ProcedimentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.ProcedimentoComboBox.SelectedIndex = 0
        procedimentoRepository.Dispose()
    End Sub

    Private Sub SelezionaFascicolo(ByVal item As Telerik.Web.UI.GridDataItem)
        Session("IdFascicoloSelezionato") = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        ParsecUtility.Utility.ClosePopup(True)
    End Sub


    Private Sub ResettaFiltro()
        Me.NumeroRegistroInizioTextBox.Text = String.Empty
        Me.NumeroRegistroFineTextBox.Text = String.Empty
        Me.OggettoTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.ResponsabileTextBox.Text = String.Empty
        Me.IdResponsabileTextBox.Text = String.Empty
        Me.CodiceFascicoloCompletoTextBox.Text = String.Empty


        Me.DataInizioAperturaTextBox.SelectedDate = Nothing
        Me.DataFineAperturaTextBox.SelectedDate = Nothing
        Me.DataInizioChiusuraTextBox.SelectedDate = Nothing
        Me.DataFineChiusuraTextBox.SelectedDate = Nothing

        Me.StatoFascicoloComboBox.SelectedIndex = 0
        Me.ProcedimentoComboBox.SelectedIndex = 0
        Me.TitololariComboBox.SelectedIndex = 0
        Me.TipologiaFascicoloComboBox.SelectedIndex = 0

        Me.Fascicoli = Nothing
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FascicoloFiltro
        Dim filtro As New ParsecAdmin.FascicoloFiltro

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            filtro.Oggetto = Me.OggettoTextBox.Text.Trim
        End If

        filtro.NumeroRegistroInizio = Me.NumeroRegistroInizioTextBox.Value
        filtro.NumeroRegistroFine = Me.NumeroRegistroFineTextBox.Value



        If (Me.TitololariComboBox.SelectedValue <> "") Then
            Dim titolare = getElementoRubrica(Me.TitololariComboBox.SelectedValue)
            Dim codiceElemento = titolare.Codice
            Dim elementoStrutturaFiltro As New ParsecAdmin.FiltroStrutturaEsternaInfo
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim listaElementiStrutturabyCodice = rubrica.GetQuery().Where(Function(w) w.Codice = codiceElemento).ToList
            For Each elemento In listaElementiStrutturabyCodice
                filtro.listaIdTitolariToFind.Add(elemento.Id)
            Next
        End If

        If Not String.IsNullOrEmpty(Me.CodiceFascicoloCompletoTextBox.Text) Then
            filtro.CodiceFascicoloCompleto = Me.CodiceFascicoloCompletoTextBox.Text
        End If

        If (Me.ProcedimentoComboBox.SelectedValue <> "0") Then
            filtro.IdProcedimento = Me.ProcedimentoComboBox.SelectedValue
        End If

        If Not String.IsNullOrEmpty(Me.IdResponsabileTextBox.Text) Then
            filtro.IdResponsabile = CInt(Me.IdResponsabileTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            filtro.IdClassificazione = CInt(Me.IdClassificazioneTextBox.Text)
        End If

        filtro.StatoFascicolo = Me.StatoFascicoloComboBox.SelectedValue

        filtro.IdUtenteCollegato = utenteCollegato.Id


        If Me.DataInizioAperturaTextBox.SelectedDate.HasValue Then
            filtro.DataAperturaInizio = Me.DataInizioAperturaTextBox.SelectedDate
        End If

        If Me.DataFineAperturaTextBox.SelectedDate.HasValue Then
            filtro.DataAperturaFine = Me.DataFineAperturaTextBox.SelectedDate
        End If

        If Me.DataInizioChiusuraTextBox.SelectedDate.HasValue Then
            filtro.DataChiusuraInizio = Me.DataInizioChiusuraTextBox.SelectedDate
        End If

        If Me.DataFineChiusuraTextBox.SelectedDate.HasValue Then
            filtro.DataChiusuraFine = Me.DataFineChiusuraTextBox.SelectedDate
        End If

        If Me.TipologiaFascicoloComboBox.SelectedIndex <> 0 Then
            filtro.IdTipologiaFascicolo = CInt(Me.TipologiaFascicoloComboBox.SelectedValue)
        End If


        Return filtro

    End Function

#End Region

#Region "EVENTI CONTROLLI - RICERCA"

    Protected Sub CercaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CercaButton.Click
        ParsecUtility.SessionManager.FiltroFascicolo = Me.GetFiltro
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub

   

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

#End Region

#Region "EVENTI CONTROLLI - GESTIONE CLASSIFICAZIONE"

    Protected Sub TrovaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
    End Sub

    Protected Sub AggiornaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim classificazioneCompleta As String = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione2(idClassificazione, 1) & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneTextBox.Text = idClassificazione.ToString
            Session("ClassificazioniSelezionate") = Nothing
        End If
    End Sub

    Protected Sub EliminaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaClassificazioneImageButton.Click
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
    End Sub

#End Region

#Region "EVENTI CONTROLLI - GESTIONE RESPONSABILE"

    Protected Sub TrovaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaResponsabileImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaResponsabileImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "400")
        parametriPagina.Add("ultimoLivelloStruttura", "400")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub


    Protected Sub AggiornaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaResponsabileImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Me.ResponsabileTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdResponsabileTextBox.Text = struttureSelezionate.First.Id.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Protected Sub EliminaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaResponsabileImageButton.Click
        Me.ResponsabileTextBox.Text = String.Empty
        Me.IdResponsabileTextBox.Text = String.Empty
    End Sub

#End Region


    Private Const ItemsPerRequest As Integer = 10
#Region "TITOLARE"

    <WebMethod()> _
    Public Shared Function GetElementiRubrica(ByVal context As RadComboBoxContext) As RadComboBoxData
        Dim rubrica As New ParsecAdmin.RubricaRepository
        Dim data = rubrica.GetQuery.Where(Function(c) c.Denominazione.StartsWith(context.Text) And c.Denominazione <> "" And c.LogStato Is Nothing).OrderBy(Function(c) c.Denominazione).ToList
        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData) '(endOffset - itemOffset)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            Dim item = data.ElementAt(i)
            itemData.Text = item.Denominazione & " " & If(Not String.IsNullOrEmpty(item.Nome), item.Nome & ", ", "") & If(Not String.IsNullOrEmpty(item.Indirizzo), item.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(item.Comune), item.Comune & " ", "") & If(Not String.IsNullOrEmpty(item.CAP), item.CAP & " ", "") & If(Not String.IsNullOrEmpty(item.Provincia), "(" & item.Provincia & ")", "")
            itemData.Value = item.Id
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function

    Public Function getElementoRubrica(ByVal idElemento As Long) As ParsecAdmin.StrutturaEsternaInfo
        Dim rubrica As New ParsecAdmin.RubricaRepository
        Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Id = idElemento And c.LogStato Is Nothing).ToList
        Dim elemento As ParsecAdmin.StrutturaEsternaInfo = Nothing
        If (Not struttureEsterne Is Nothing And struttureEsterne.Count > 0) Then
            elemento = struttureEsterne(0)
        End If
        Return elemento
    End Function

    Protected Sub TrovaBeneficiarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaBeneficiarioImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaBeneficiarioImageButton.ClientID)
        queryString.Add("mode", "search")

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("Filtro", Me.TitololariComboBox.Text)
        parametriPagina.Add("FiltroTipologiaSoggetto", 1)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

        If Not String.IsNullOrEmpty(Me.TitololariComboBox.Text) Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) (c.Denominazione.Contains(Me.TitololariComboBox.Text)) And c.LogStato Is Nothing).ToList
            If struttureEsterne.Count = 1 Then
                Me.TitololariComboBox.Text = struttureEsterne(0).Denominazione
                Me.TitololariComboBox.SelectedValue = struttureEsterne(0).Id
                ParsecUtility.SessionManager.ParametriPagina = Nothing
            Else
                ParsecUtility.Utility.ShowPopup(pageUrl, 910, 720, queryString, False)
            End If
            rubrica.Dispose()
        Else
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 720, queryString, False)
        End If
    End Sub

    Protected Sub AggiornaBeneficiarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBeneficiarioImageButton.Click
        If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
            Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
            'Dim strutturaEsterne As ParsecContratti.StrutturaEsternaInfo = (New ParsecContratti.ViewRubricaRepository().GetQuery.Where(Function(o) o.iseCodice = strutturaEsterna.Codice And o.iseLog_Stato Is Nothing)).FirstOrDefault
            Me.TitololariComboBox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "") 'strutturaEsterna.Denominazione
            Me.TitololariComboBox.SelectedValue = strutturaEsterna.Id
            ParsecUtility.SessionManager.Rubrica = Nothing
        End If
    End Sub

#End Region

End Class
