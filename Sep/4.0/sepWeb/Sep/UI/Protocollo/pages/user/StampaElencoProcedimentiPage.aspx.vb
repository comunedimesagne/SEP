Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StampaElencoProcedimentiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    'Variabile di Sessione: lista dei Procedimenti associata alla griglia ProcedimentiGridView
    Public Property Procedimenti() As List(Of ParsecPro.Procedimento)
        Get
            Return CType(Session("StampaElencoProcedimentiPage_Procedimenti"), List(Of ParsecPro.Procedimento))
        End Get
        Set(ByVal value As List(Of ParsecPro.Procedimento))
            Session("StampaElencoProcedimentiPage_Procedimenti") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadListBox .rlbCheck,.RadListBox .rlbImage,.RadListBox .rlbText{vertical-align: middle;padding-left:5px;}"
        Me.Page.Header.Controls.Add(css)

        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> Stampa Elenco Procedimenti"

        If Not Me.Page.IsPostBack Then
            Me.Procedimenti = New List(Of ParsecPro.Procedimento)
            Me.CaricaStatoProcedimento()
            Me.ResettaFiltro()

            Me.CaricaUtenti()
            Me.CaricaProcedimenti()
            Me.SelezionaTuttiUtentiCheckBox.Enabled = (Me.UtentiListBox.Items.Count > 0)
            Me.SelezionaTuttiProcedimentiCheckBox.Enabled = (Me.ProcedimentiListBox.Items.Count > 0)

        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.ProcedimentiGridView.Style.Add("width", widthStyle)

    End Sub

    'Evento PreRender associato alla Pagina. Carica i titoli e abilita o meno alcuni campi.
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.ElencoUtentiLabel.Text = "Elenco Utenti&nbsp;&nbsp;&nbsp;" & If(Me.UtentiListBox.Items.Count > 0, "( " & Me.UtentiListBox.Items.Count.ToString & " )", "")
        Me.ElencoProcedimentiLabel.Text = "Elenco Procedimenti&nbsp;&nbsp;&nbsp;" & If(Me.ProcedimentiListBox.Items.Count > 0, "( " & Me.ProcedimentiListBox.Items.Count.ToString & " )", "")
        Me.TitoloLabel.Text = "Procedimenti&nbsp;&nbsp;&nbsp;" & If(Me.Procedimenti.Count > 0, "( " & Me.Procedimenti.Count.ToString & " )", "")
        Me.EsportaButton.Enabled = Me.Procedimenti.Count > 0
        Me.AnteprimaStampaButton.Enabled = Me.Procedimenti.Count > 0
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla griglia ProcedimentiGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione Procedimenti.
    Protected Sub ProcedimentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ProcedimentiGridView.NeedDataSource
        If Me.Procedimenti Is Nothing Then
            Dim procedimenti As New ParsecPro.ProcedimentoViewRepository
            Me.Procedimenti = procedimenti.GetView(Me.GetFiltro)
        End If
        Me.ProcedimentiGridView.DataSource = Me.Procedimenti
    End Sub

    'Evento ItemDataBound associato alla TipiDocumentoGridView. Setta alcuni tooltip in base al contenuto delle celle.
    Protected Sub ProcedimentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ProcedimentiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridHeaderItem Then
            Dim header As GridHeaderItem = CType(e.Item, GridHeaderItem)
            Dim lb As LinkButton = Nothing
            'DATO CHE HO IMPOSTATO LA PROPRIETA' HeaderTooltip CHE MI SOVRASCRIVE LA DESCRIZIONE DEL TOOLTIP DEL LINKBUTTON
            'LO REIMPOSTO IN MODO CORRETTO.
            If TypeOf header("Tempo").Controls(0) Is LinkButton Then
                lb = CType(header("Tempo").Controls(0), LinkButton)
                lb.ToolTip = "Cliccare qui per ordinare"
            End If
            If TypeOf header("Conteggio").Controls(0) Is LinkButton Then
                lb = CType(header("Conteggio").Controls(0), LinkButton)
                lb.ToolTip = "Cliccare qui per ordinare"
            End If
            If TypeOf header("Media").Controls(0) Is LinkButton Then
                lb = CType(header("Media").Controls(0), LinkButton)
                lb.ToolTip = "Cliccare qui per ordinare"
            End If
        End If

    End Sub

#End Region


#Region "EVENTI CONTROLLI"

    'Evento Click che lancia la stampa dei risultati della ricerca presenti in griglia
    Protected Sub AnteprimaStampaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnteprimaStampaButton.Click
        Try
            Me.Print()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message.ToString, False)
        End Try
    End Sub

    'Evento click che lancia la esportazione dei dati contenuti nella griglia
    Protected Sub EsportaButton_Click(sender As Object, e As System.EventArgs) Handles EsportaButton.Click
        Try
            Me.Esporta()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message.ToString, False)
        End Try
    End Sub

    'Evento Click che resetta i campi della maschera e riesegue il caricamento della griglia
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.ApplicaFiltro()
    End Sub

    'Esegue la ricerca applicando il filtro.
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

#End Region

#Region "METODI PRIVATI"

    'carica gli Utenti nella Lista UtentiListBox
    Private Sub CaricaUtenti()

        Dim utenti As New ParsecAdmin.UserRepository

        Dim res = (From utente In utenti.GetQuery
                  Where utente.LogTipoOperazione Is Nothing
                  Select utente).AsEnumerable.Select(Function(c) New With {
                                                         .Id = c.Id,
                                                         .Descrizione = (c.Username & " - " & c.Cognome & " " & c.Nome)
                                                         }).OrderBy(Function(c) c.Descrizione)



        Me.UtentiListBox.DataValueField = "Id"
        Me.UtentiListBox.DataTextField = "Descrizione"
        Me.UtentiListBox.DataSource = res
        Me.UtentiListBox.DataBind()

        utenti.Dispose()
    End Sub

    'Carica i Procedimenti nella Lista ProcedimentiListBox
    Private Sub CaricaProcedimenti()

        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository

        Dim res = From procedimento In procedimenti.GetQuery.OrderBy(Function(c) c.Nome)
           Select New With {.Id = procedimento.Id, .Descrizione = procedimento.Nome}

        Me.ProcedimentiListBox.DataValueField = "Id"
        Me.ProcedimentiListBox.DataTextField = "Descrizione"
        Me.ProcedimentiListBox.DataSource = res
        Me.ProcedimentiListBox.DataBind()

        procedimenti.Dispose()
    End Sub

    'Applica il filtro ricaricando la griglia ProcedimentiGridView.
    Private Sub ApplicaFiltro()
        Me.Procedimenti = Nothing
        Me.ProcedimentiGridView.Rebind()
    End Sub

    'Crea l'oggetto filtro e lo restituisce per la ricerca
    Private Function GetFiltro() As ParsecPro.FiltroProcedimento
        Dim filtro As New ParsecPro.FiltroProcedimento

        filtro.ElencoIdProcedimenti.AddRange(Me.ProcedimentiListBox.CheckedItems.Select(Function(c) CInt(c.Value)))
        filtro.ElencoIdUtenti.AddRange(Me.UtentiListBox.CheckedItems.Select(Function(c) CInt(c.Value)))
        filtro.Stato = CType(CInt(Me.StatoProcedimentoComboBox.SelectedValue), ParsecPro.StatoProcedimento)
        filtro.DataInizio = Me.DataProcedimentoInizioTextBox.SelectedDate
        filtro.DataFine = Me.DataProcedimentoFineTextBox.SelectedDate

        Return filtro
    End Function

    'Esportazione in excel dell'elenco delle informazioni contenuta nella griglia
    Private Sub Esporta()
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("StatisticheProcedimenti_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As String = String.Empty

        line &= "DESCRIZIONE;TERMINE (gg);FREQUENZA;MEDIA (gg)"
        swExport.WriteLine(line)
        line = ""
        For Each p As ParsecPro.Procedimento In Me.Procedimenti

            line &= p.Descrizione.Replace(";", ",") & ";" & _
                 If(p.Tempo.HasValue, p.Tempo.Value.ToString, "") & ";" & _
                 If(p.Conteggio.HasValue, p.Conteggio.Value.ToString, "") & ";" & _
                 If(p.Media.HasValue, p.Media.Value.ToString, "")

            swExport.WriteLine(line)
            line = ""
        Next
        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

    End Sub

    'Lancia il Report "StampaStatisticheProcedimenti"
    Private Sub Print()
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaStatisticheProcedimenti")
        parametriStampa.Add("DatiStampa", Me.Procedimenti)
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    'Convalida i parametri prima della Ricerca.
    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean

        If Me.UtentiListBox.CheckedItems.Count = 0 Then
            message.AppendLine("E' necessario selezionare almeno un utente!")
        End If

        If Me.ProcedimentiListBox.CheckedItems.Count = 0 Then
            message.AppendLine("E' necessario selezionare almeno un procedimento!")
        End If

        If Not Me.DataProcedimentoInizioTextBox.SelectedDate.HasValue Then
            message.AppendLine("E' necessario impostare il campo 'Data da'!")
        End If

        If Not Me.DataProcedimentoFineTextBox.SelectedDate.HasValue Then
            message.AppendLine("E' necessario impostare il campo 'Data a'!")
        End If

        'Se la data iniziale è maggiore di quella finale.
        If Me.DataProcedimentoInizioTextBox.SelectedDate.HasValue AndAlso Me.DataProcedimentoFineTextBox.SelectedDate.HasValue Then
            If Date.Compare(Me.DataProcedimentoInizioTextBox.SelectedDate, Me.DataProcedimentoFineTextBox.SelectedDate) > 0 Then
                message.AppendLine("Il campo 'Data da' deve essere antecedente o uguale al campo 'Data a'!")
            End If
        End If

        Return Not message.Length > 0
    End Function

    'Resetta i campi della Maschera..e quindi il filtro per la ricerca.
    Private Sub ResettaFiltro()
        Me.DataProcedimentoInizioTextBox.SelectedDate = New DateTime(Now.Year, 1, 1)
        Me.DataProcedimentoFineTextBox.SelectedDate = New DateTime(Now.Year, 12, 31)
        Me.StatoProcedimentoComboBox.SelectedIndex = 0
        Me.UtentiListBox.ClearChecked()
        Me.ProcedimentiListBox.ClearChecked()
        Me.SelezionaTuttiUtentiCheckBox.Checked = False
        Me.SelezionaTuttiProcedimentiCheckBox.Checked = False
    End Sub

    'carica gli stati del procedimento nella Combo StatoProcedimentoComboBox.
    Private Sub CaricaStatoProcedimento()
        Me.StatoProcedimentoComboBox.Items.Add(New RadComboBoxItem("In Corso", "1"))  'StatoProcedimento.Inizio
        Me.StatoProcedimentoComboBox.Items.Add(New RadComboBoxItem("Conclusi", "2"))  'StatoProcedimento.Fine
        Me.StatoProcedimentoComboBox.SelectedIndex = 0
    End Sub

#End Region

End Class