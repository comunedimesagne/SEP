Imports Telerik.Web.UI

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class RicercaFatturaElettronicaPage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    'Variabile di Sessione: lista delle fatture elettroniche.
    Private Property FattureElettroniche() As List(Of ParsecPro.FatturaElettronica)
        Get
            Return CType(Session("RicercaFatturaElettronicaPage_FattureElettroniche"), List(Of ParsecPro.FatturaElettronica))
        End Get
        Set(ByVal value As List(Of ParsecPro.FatturaElettronica))
            Session("RicercaFatturaElettronicaPage_FattureElettroniche") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.FattureElettroniche = Nothing
            Me.ResettaFiltro()
        End If

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.FattureElettronicheGridView.GroupingSettings.CaseSensitive = False
        Me.FattureElettronicheGridView.Style.Add("width", widthStyle)
    End Sub

    'Evento LoadComplete: gestisce il "chiudi"
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla griglia FattureElettronicheGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione FattureElettroniche.
    Protected Sub FattureElettronicheGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FattureElettronicheGridView.NeedDataSource
        If Me.FattureElettroniche Is Nothing Then
            Me.FattureElettroniche = Me.GetFatture(Me.GetFiltro)
        End If
        Me.FattureElettronicheGridView.DataSource = Me.FattureElettroniche
    End Sub

    'Evento ItemCommand associato alla FattureElettronicheGridView. Fa partire i comandi associati alla griglia delle fatture.
    Protected Sub FattureElettronicheGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FattureElettronicheGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.SelezionaFatturaElettronica(e.Item)
        End If
    End Sub

    'Evento ItemCreated associato alla Griglia FattureElettronicheGridView. Gestisce la navigazione tra pagine della griglia.
    Protected Sub FattureElettronicheGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FattureElettronicheGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    'Evento ItemDataBound associato alla FattureElettronicheGridView. Setta i tooltip e il titolo della griglia. 
    Protected Sub FattureElettronicheGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles FattureElettronicheGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloLabel.Text = "Elenco Fatture Elettroniche&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim stato = CType(DataBinder.Eval(dataItem.DataItem, "IdStato"), ParsecPro.StatoFattura)
            Dim btn As ImageButton = CType(dataItem.FindControl("IdStato"), ImageButton)

            Me.ImpostaStato(btn, stato)

        End If

    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    'Evento click del pulsante FiltraImageButton: permette la ricerca delle fatture in base ai filtri.
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FiltraImageButton.Click
        If Me.ProtocollataCheckBox.Checked = False AndAlso Me.AccettataCheckBox.Checked = False AndAlso Me.RifiutataCheckBox.Checked = False AndAlso Me.ConservataCheckBox.Checked = False AndAlso Me.ContabilizzataCheckBox.Checked = False Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno uno stato della fattura!", False)
        End If
        Me.AggiornaGriglia()
    End Sub

    'Evento click del pulsante AnnullaFiltroImageButton: permette il reset dei filtri e della griglia.
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.AggiornaGriglia()
    End Sub

#End Region

#Region "METODI PRIVATI"

    'Imposta i tooltip e le icone della griglia. Richiamato da FattureElettronicheGridView.ItemDataBound
    Private Sub ImpostaStato(ByVal btn As ImageButton, ByVal stato As ParsecPro.StatoFattura)
        btn.Attributes.Add("onclick", "return false")
        Select Case stato
            Case ParsecPro.StatoFattura.Protocollata
                btn.ImageUrl = "~/images/pArancio16.png"
                btn.ToolTip = "Protocollata"
            Case ParsecPro.StatoFattura.Accettata
                btn.ImageUrl = "~/images/pVerde16.png"
                btn.ToolTip = "Accettata"
            Case ParsecPro.StatoFattura.Rifiutata
                btn.ImageUrl = "~/images/pRosso16.png"
                btn.ToolTip = "Rifiutata"
            Case ParsecPro.StatoFattura.Convervata
                btn.ImageUrl = "~/images/pBlue16.png"
                btn.ToolTip = "Conservata"
            Case ParsecPro.StatoFattura.Contabilizzata
                btn.ImageUrl = "~/images/pFucsia16.png"
                btn.ToolTip = "Contabilizzata"
        End Select
    End Sub

    'Ricerca le fatture sul DB e le restituisce in una lista
    Private Function GetFatture(ByVal filtro As ParsecPro.FiltroFatturaElettronica) As List(Of ParsecPro.FatturaElettronica)

        Dim funzioni As New ParsecAdmin.FunzioniUtenteRepository
        Dim abilitatoVisibilitaProtocolli As Boolean = Not funzioni.GetQuery.Where(Function(c) c.IdFunzione = 45 And c.IdUtente = filtro.IdUtenteCollegato).FirstOrDefault Is Nothing
        Dim abilitatoVisibilitaFatture As Boolean = Not funzioni.GetQuery.Where(Function(c) c.IdFunzione = 49 And c.IdUtente = filtro.IdUtenteCollegato).FirstOrDefault Is Nothing
        funzioni.Dispose()


        If abilitatoVisibilitaFatture = False Then
            abilitatoVisibilitaFatture = abilitatoVisibilitaProtocolli
        End If

        Dim fatture As New ParsecPro.FatturaElettronicaRepository
        Dim res = fatture.GetQuery

        If Not abilitatoVisibilitaFatture Then

            Dim visibilita As New ParsecPro.VisibilitaDocumentoViewRepository(fatture.Context)
            Dim gruppi As New ParsecPro.GruppoViewRepository(fatture.Context)
            Dim utentiGruppo As New ParsecPro.GruppoUtenteViewRepository(fatture.Context)

            res = (From fattura In res
                       Join entita In visibilita.GetQuery
                       On fattura.IdRegistrazione Equals entita.IdDocumento
                       Join gruppo In gruppi.GetQuery
                       On entita.IdEntita Equals gruppo.Id
                       Join ug In utentiGruppo.GetQuery
                       On ug.IdGruppo Equals gruppo.Id
                       Where entita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo And entita.IdModulo = ParsecAdmin.TipoModulo.PRO And ug.IdUtente = filtro.IdUtenteCollegato
                       Select fattura).Union(
                       From fattura In res
                       Join entita In visibilita.GetQuery
                       On fattura.IdRegistrazione Equals entita.IdDocumento
                       Where entita.IdEntita = filtro.IdUtenteCollegato And entita.IdModulo = ParsecAdmin.TipoModulo.PRO And entita.TipoEntita = ParsecAdmin.TipoEntita.Utente
                       Order By fattura.MessaggioSdI.DataRicezioneInvio
                       Select fattura
                       )

        End If

        'ESCLUDO LE FATTURE RICEVUTE
        res = res.Where(Function(c) c.IdStato <> ParsecPro.StatoFattura.Ricevuta)

        If Not filtro Is Nothing Then


            If filtro.ElencoStati.Count > 0 Then
                res = res.Where(Function(c) filtro.ElencoStati.Contains(c.IdStato))
            Else
                res = res.Where(Function(c) c.IdStato = ParsecPro.StatoFattura.Nessuno)
            End If

            If filtro.DataInizioRicezioneInvio.HasValue Then
                Dim dataInizio = filtro.DataInizioRicezioneInvio.Value
                Dim startDate As Date = New Date(dataInizio.Year, dataInizio.Month, dataInizio.Day, 0, 0, 0)
                res = res.Where(Function(c) c.MessaggioSdI.DataRicezioneInvio >= startDate)
            End If
            If filtro.DataFineRicezioneInvio.HasValue Then
                Dim dataFine = filtro.DataFineRicezioneInvio.Value
                Dim endDate As Date = New Date(dataFine.Year, dataFine.Month, dataFine.Day, 23, 59, 59)
                res = res.Where(Function(c) c.MessaggioSdI.DataRicezioneInvio <= endDate)
            End If
            If Not String.IsNullOrEmpty(filtro.Nomefile) Then
                res = res.Where(Function(c) c.MessaggioSdI.Nomefile.Contains(filtro.Nomefile))
            End If
            If Not String.IsNullOrEmpty(filtro.Fornitore) Then
                res = res.Where(Function(c) c.DenominazioneFornitore.Contains(filtro.Fornitore))
            End If
        End If

        Dim viewFatture = res.AsEnumerable.Select(Function(c) New ParsecPro.FatturaElettronica With {
                                                                                            .AnnoProtocollo = c.AnnoProtocollo,
                                                                                            .CodiceFornitore = c.CodiceFornitore,
                                                                                            .ComuneFornitore = c.ComuneFornitore,
                                                                                            .DenominazioneFornitore = c.DenominazioneFornitore,
                                                                                            .Id = c.Id,
                                                                                            .IdentificativoSdI = c.IdentificativoSdI,
                                                                                            .IdMessaggioSdI = c.IdMessaggioSdI,
                                                                                            .IdRegistrazione = c.IdRegistrazione,
                                                                                            .IdStato = c.IdStato,
                                                                                            .NomeFileMetadati = c.NomeFileMetadati,
                                                                                            .NumeroFatture = c.NumeroFatture,
                                                                                            .NumeroProtocollo = c.NumeroProtocollo,
                                                                                            .Oggetto = c.Oggetto,
                                                                                            .PartitaIvaFornitore = c.PartitaIvaFornitore,
                                                                                            .StatoFatturaElettronica = c.StatoFatturaElettronica,
                                                                                            .VersioneFattura = c.VersioneFattura,
                                                                                            .CodiceIpaDestinatario = c.CodiceIpaDestinatario,
                                                                                            .DenominazioneDestinatario = c.DenominazioneDestinatario,
                                                                                            .ElencoCig = c.ElencoCig,
                                                                                            .CognomeFornitore = c.CognomeFornitore,
                                                                                            .IndirizzoFornitore = c.IndirizzoFornitore,
                                                                                            .NumeroCivicoFornitore = c.NumeroCivicoFornitore,
                                                                                            .NomeFornitore = c.NomeFornitore,
                                                                                            .CapFornitore = c.CapFornitore,
                                                                                            .EmailFornitore = c.EmailFornitore
                                                                                            }).ToList



        Return viewFatture

    End Function

    'Imposta e restituisce il filtro per la ricerca delle fatture.
    Private Function GetFiltro() As ParsecPro.FiltroFatturaElettronica
        Dim filtro As New ParsecPro.FiltroFatturaElettronica

        filtro.DataInizioRicezioneInvio = Me.DataInvioInizioTextBox.SelectedDate
        filtro.DataFineRicezioneInvio = Me.DataInvioFineTextBox.SelectedDate



        If Me.ProtocollataCheckBox.Checked Then
            filtro.ElencoStati.Add(ParsecPro.StatoFattura.Protocollata)
        End If

        If Me.AccettataCheckBox.Checked Then
            filtro.ElencoStati.Add(ParsecPro.StatoFattura.Accettata)
        End If


        If Me.RifiutataCheckBox.Checked Then
            filtro.ElencoStati.Add(ParsecPro.StatoFattura.Rifiutata)
        End If

        If Me.ConservataCheckBox.Checked Then
            filtro.ElencoStati.Add(ParsecPro.StatoFattura.Convervata)
        End If

        If Me.ContabilizzataCheckBox.Checked Then
            filtro.ElencoStati.Add(ParsecPro.StatoFattura.Contabilizzata)
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        filtro.IdUtenteCollegato = utenteCollegato.Id

        Return filtro
    End Function

    'Richiamamto da FattureElettronicheGridView.ItemCommand: permette di selezionare la fattura nella griglia.
    Private Sub SelezionaFatturaElettronica(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim fattura As ParsecPro.FatturaElettronica = Me.FattureElettroniche.Where(Function(c) c.Id = id).FirstOrDefault
        ParsecUtility.SessionManager.FatturaElettronica = fattura
        ParsecUtility.Utility.ClosePopup(False)
        Me.FattureElettroniche = Nothing
    End Sub

    'Aggiorna la griglia delle fatture
    Private Sub AggiornaGriglia()
        Me.FattureElettroniche = Nothing
        Me.FattureElettronicheGridView.Rebind()
    End Sub

    'Resetta i campi di ricerca della maschera. 
    Private Sub ResettaFiltro()
        For Each col As GridColumn In Me.FattureElettronicheGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.FattureElettronicheGridView.MasterTableView.FilterExpression = String.Empty
        Me.DataInvioInizioTextBox.SelectedDate = Nothing
        Me.DataInvioFineTextBox.SelectedDate = Nothing
        Me.ProtocollataCheckBox.Checked = True
        Me.AccettataCheckBox.Checked = True
        Me.RifiutataCheckBox.Checked = True
        Me.ConservataCheckBox.Checked = True
        Me.ContabilizzataCheckBox.Checked = True
    End Sub

#End Region

End Class