Imports ParsecAtt
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneOrganoPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


#Region "PROPRIETA'"

    Public Property TipologiaSeduta() As ParsecAtt.TipologiaSeduta
        Get
            Return CType(Session("GestioneOrganoPage_TipologiaSeduta"), ParsecAtt.TipologiaSeduta)
        End Get
        Set(ByVal value As ParsecAtt.TipologiaSeduta)
            Session("GestioneOrganoPage_TipologiaSeduta") = value
        End Set
    End Property

    Public Property TipologieSeduta() As List(Of ParsecAtt.TipologiaSeduta)
        Get
            Return CType(Session("GestioneOrganoPage_TipologieSeduta"), List(Of ParsecAtt.TipologiaSeduta))
        End Get
        Set(ByVal value As List(Of ParsecAtt.TipologiaSeduta))
            Session("GestioneOrganoPage_TipologieSeduta") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Gestione Tipologia Organo Decisionale"

        If Not Me.Page.IsPostBack Then
            Me.TipologieSeduta = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.TipologieSedutaGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
            Me.CaricaTipologiaDocumento()
         
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.TipologieSedutaGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        'ParsecUtility.Utility.Confirm("Eliminare la tipologia di seduta selezionata?", False, Not Me.TipologiaSeduta Is Nothing)
        If Not Me.TipologieSeduta Is Nothing Then
            Me.TitoloElencoLabel.Text = "Elenco Tipologie di Seduta&nbsp;&nbsp;&nbsp;" & If(Me.TipologieSeduta.Count > 0, "( " & Me.TipologieSeduta.Count.ToString & " )", "")
        End If

        Me.RadToolBar.Items.FindItemByText("Elimina").Enabled = False
        Me.RadToolBar.Items.FindItemByText("Elimina").Style.Add("onclick", "return false")

        'SELEZIONO LA RIGA
        If Not Me.TipologiaSeduta Is Nothing Then
            Dim item As GridDataItem = Me.TipologieSedutaGridView.MasterTableView.FindItemByKeyValue("Id", Me.TipologiaSeduta.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If
    End Sub

#End Region


#Region "EVENTI TOOLBAR"


    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                ' Me.Print()
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
                    If Not Me.TipologiaSeduta Is Nothing Then
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.AggiornaGriglia()
                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)
                        End Try
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una tipologia di seduta!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            ' e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region


#Region "EVENTI GRIGLIA"

    Protected Sub TipologieSedutaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TipologieSedutaGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
        End Select
    End Sub


    Protected Sub TipologieSedutaGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles TipologieSedutaGridView.NeedDataSource
        If Me.TipologieSeduta Is Nothing Then
            Dim tipologie As New ParsecAtt.TipologiaSedutaRepository
            Me.TipologieSeduta = tipologie.GetView(Nothing)
            tipologie.Dispose()
        End If
        Me.TipologieSedutaGridView.DataSource = Me.TipologieSeduta
    End Sub

    Protected Sub TipologieSedutaGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TipologieSedutaGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region



#Region "METODI PRIVATI"

    Private Sub CaricaTipologiaDocumento()
        'luca 02/07/2020
        '' ''Dim tipiDocumento As New ParsecMES.TipiDocumentoRepository
        '' ''Me.TipoDocumentoComboBox.DataSource = tipiDocumento.GetView(Nothing).Where(Function(c) c.Cancellato = False)
        Me.TipoDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipoDocumentoComboBox.DataValueField = "Codice"
        Me.TipoDocumentoComboBox.DataBind()
        Me.TipoDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.TipoDocumentoComboBox.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem("Determina", "2"))
        Me.TipoDocumentoComboBox.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem("Delibera di Ginuta", "4"))
        Me.TipoDocumentoComboBox.Items.Insert(3, New Telerik.Web.UI.RadComboBoxItem("Direttiva", "6"))
        Me.TipoDocumentoComboBox.Items.Insert(4, New Telerik.Web.UI.RadComboBoxItem("Ordine del Giorno", "8"))
        Me.TipoDocumentoComboBox.Items.Insert(5, New Telerik.Web.UI.RadComboBoxItem("Lettera di Trasmissione Capigruppo", "9"))
        Me.TipoDocumentoComboBox.Items.Insert(6, New Telerik.Web.UI.RadComboBoxItem("Lettera di Trasmissione Revisori", "10"))
        Me.TipoDocumentoComboBox.Items.Insert(7, New Telerik.Web.UI.RadComboBoxItem("Avviso di Convocazione", "11"))
        Me.TipoDocumentoComboBox.Items.Insert(8, New Telerik.Web.UI.RadComboBoxItem("Ordinanza", "12"))
        Me.TipoDocumentoComboBox.Items.Insert(9, New Telerik.Web.UI.RadComboBoxItem("Decreto", "13"))
        Me.TipoDocumentoComboBox.Items.Insert(10, New Telerik.Web.UI.RadComboBoxItem("Atto generico", "16"))
        Me.TipoDocumentoComboBox.Items.Insert(11, New Telerik.Web.UI.RadComboBoxItem("Avviso Bando di Concorso", "17"))
        Me.TipoDocumentoComboBox.Items.Insert(12, New Telerik.Web.UI.RadComboBoxItem("Avviso generico", "18"))
        Me.TipoDocumentoComboBox.Items.Insert(13, New Telerik.Web.UI.RadComboBoxItem("Pubblicazione di Matrimonio", "19"))
        Me.TipoDocumentoComboBox.Items.Insert(14, New Telerik.Web.UI.RadComboBoxItem("Bilancio Comunale e Rendiconto di gestione", "20"))
        Me.TipoDocumentoComboBox.Items.Insert(15, New Telerik.Web.UI.RadComboBoxItem("Pubblicazioni e Autorizzazioni SUAP", "21"))
        Me.TipoDocumentoComboBox.Items.Insert(16, New Telerik.Web.UI.RadComboBoxItem("Concorsi e selezioni", "22"))
        Me.TipoDocumentoComboBox.Items.Insert(17, New Telerik.Web.UI.RadComboBoxItem("Concessioni edilizie", "23"))
        Me.TipoDocumentoComboBox.Items.Insert(18, New Telerik.Web.UI.RadComboBoxItem("Regolamenti Comunali", "24"))
        Me.TipoDocumentoComboBox.Items.Insert(19, New Telerik.Web.UI.RadComboBoxItem("Statuto comunale", "25"))
        Me.TipoDocumentoComboBox.Items.Insert(20, New Telerik.Web.UI.RadComboBoxItem("Elenchi consulenze ed incarichi", "26"))
        Me.TipoDocumentoComboBox.Items.Insert(21, New Telerik.Web.UI.RadComboBoxItem("Modulistica", "27"))
        Me.TipoDocumentoComboBox.Items.Insert(22, New Telerik.Web.UI.RadComboBoxItem("Piano Regolatore Generale e cartografia", "28"))
        Me.TipoDocumentoComboBox.Items.Insert(23, New Telerik.Web.UI.RadComboBoxItem("Piano Strategico territoriale", "29"))

        Me.TipoDocumentoComboBox.Items.Insert(24, New Telerik.Web.UI.RadComboBoxItem("Delibera di Consiglio", "5"))
        Me.TipoDocumentoComboBox.Items.Insert(25, New Telerik.Web.UI.RadComboBoxItem("Delibera di Commissario Straordinario", "7"))
        Me.TipoDocumentoComboBox.Items.Insert(26, New Telerik.Web.UI.RadComboBoxItem("Delibera di Sub-commissario Prefettizio", "14"))
        Me.TipoDocumentoComboBox.Items.Insert(27, New Telerik.Web.UI.RadComboBoxItem("Cambio nome-cognome", "30"))
        Me.TipoDocumentoComboBox.Items.Insert(28, New Telerik.Web.UI.RadComboBoxItem("Pubblicazione Bando di Gara", "31"))
        Me.TipoDocumentoComboBox.Items.Insert(29, New Telerik.Web.UI.RadComboBoxItem("Ordinanza Dirigenziale", "32"))
        Me.TipoDocumentoComboBox.Items.Insert(30, New Telerik.Web.UI.RadComboBoxItem("Ordinanza Sindacale", "33"))

        Me.TipoDocumentoComboBox.Items.Insert(31, New Telerik.Web.UI.RadComboBoxItem("Delibera della Commissione", "35"))
        Me.TipoDocumentoComboBox.Items.Insert(32, New Telerik.Web.UI.RadComboBoxItem("Atti del servizio elettorale", "36"))
        Me.TipoDocumentoComboBox.Items.Insert(33, New Telerik.Web.UI.RadComboBoxItem("Atti dell'Anagrafe e Stato Civile", "37"))
        Me.TipoDocumentoComboBox.Items.Insert(34, New Telerik.Web.UI.RadComboBoxItem("Autorizzazioni Amministrative", "38"))
        Me.TipoDocumentoComboBox.Items.Insert(35, New Telerik.Web.UI.RadComboBoxItem("Autorizzazioni Paesaggistiche", "39"))
        Me.TipoDocumentoComboBox.Items.Insert(36, New Telerik.Web.UI.RadComboBoxItem("Segnalazione Certificata Inizio Attivita' Edil.", "40"))

        Me.TipoDocumentoComboBox.Items.Insert(37, New Telerik.Web.UI.RadComboBoxItem("Tutela Ambientale", "41"))
        Me.TipoDocumentoComboBox.Items.Insert(38, New Telerik.Web.UI.RadComboBoxItem("Deliberazioni Assemblea A.R.O. 11/LE", "42"))
        Me.TipoDocumentoComboBox.Items.Insert(39, New Telerik.Web.UI.RadComboBoxItem("Deliberazioni del Coordinamento Istituz.", "43"))
        Me.TipoDocumentoComboBox.Items.Insert(40, New Telerik.Web.UI.RadComboBoxItem("Determina Ufficio di Piano", "44"))
        Me.TipoDocumentoComboBox.Items.Insert(41, New Telerik.Web.UI.RadComboBoxItem("Avvisi Allerta Meteo", "45"))
        Me.TipoDocumentoComboBox.Items.Insert(42, New Telerik.Web.UI.RadComboBoxItem("Decreto Sindacale", "46"))
        Me.TipoDocumentoComboBox.Items.Insert(43, New Telerik.Web.UI.RadComboBoxItem("Delibera GIUNTA STRAORDINARIA", "47"))


        Me.TipoDocumentoComboBox.SelectedIndex = 0
        '' ''tipiDocumento.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.TipologieSeduta = Nothing
        Me.TipologieSedutaGridView.Rebind()
    End Sub


    Private Sub Search()
        Dim tipologie As New ParsecAtt.TipologiaSedutaRepository
        Dim filtro As ParsecAtt.FiltroTipologiaSeduta = Me.GetFiltro
        Me.TipologieSeduta = tipologie.GetView(filtro)
        tipologie.Dispose()
        Me.TipologieSedutaGridView.Rebind()
    End Sub


    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim tipologieSeduta As New ParsecAtt.TipologiaSedutaRepository
        Dim tipologiaSeduta As ParsecAtt.TipologiaSeduta = tipologieSeduta.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        Me.AggiornaVista(tipologiaSeduta)
        tipologieSeduta.Dispose()
    End Sub


    Private Sub AggiornaVista(ByVal tipologiaSeduta As ParsecAtt.TipologiaSeduta)
        Me.ResettaVista()
        Me.DescrizioneTextBox.Text = tipologiaSeduta.Descrizione
        Me.DisabilitataCheckBox.Checked = tipologiaSeduta.Disabilitato
        If tipologiaSeduta.Data.HasValue Then
            Me.DataTextBox.Text = tipologiaSeduta.Data.Value.ToShortDateString
        End If

        Dim item = Me.TipoDocumentoComboBox.Items.FindItemByValue(tipologiaSeduta.CodiceTipologiaDocumento)
        If Not item Is Nothing Then
            item.Selected = True
        End If
       
        Me.TipologiaSeduta = tipologiaSeduta
    End Sub

    Private Sub Delete()
        'Niente
    End Sub

    Private Sub Save()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim tipologieSeduta As New ParsecAtt.TipologiaSedutaRepository
        Dim tipologiaSeduta As ParsecAtt.TipologiaSeduta = tipologieSeduta.CreateFromInstance(Me.TipologiaSeduta)

        'Aggiorno il modello
        tipologiaSeduta.Descrizione = Me.DescrizioneTextBox.Text.Trim
        tipologiaSeduta.Disabilitato = Me.DisabilitataCheckBox.Checked
        tipologiaSeduta.Data = CDate(Me.DataTextBox.Text)
        tipologiaSeduta.IdUtente = utenteCollegato.Id

        tipologiaSeduta.CodiceTipologiaDocumento = Nothing
        If Me.TipoDocumentoComboBox.SelectedIndex > 0 Then
            tipologiaSeduta.CodiceTipologiaDocumento = CInt(Me.TipoDocumentoComboBox.SelectedValue)
        End If

        Try
            tipologieSeduta.Save(tipologiaSeduta)
            Me.AggiornaVista(tipologieSeduta.TipologiaSeduta)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            tipologieSeduta.Dispose()
        End Try

    End Sub


    Private Sub ResettaVista()
        Me.DescrizioneTextBox.Text = String.Empty
        Me.DisabilitataCheckBox.Checked = False
        Me.DataTextBox.Text = String.Format("{0:dd/MM/yyyy}", Now)
        Me.TipologiaSeduta = Nothing
        Me.TipoDocumentoComboBox.SelectedIndex = 0
    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroTipologiaSeduta
        Dim filtro As New ParsecAtt.FiltroTipologiaSeduta
        filtro.Descrizione = Me.DescrizioneTextBox.Text.Trim
        Return filtro
    End Function



#End Region

End Class