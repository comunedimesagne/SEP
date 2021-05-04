
Imports Telerik.Web.UI

Partial Class GestioneControlloSuccessivoRegolaritaAmministrativaPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

    Private Enum CriterioSceltaEstrazione
        AttoDefinitivoConservato = 0
        AttoDefinitivoIterConcluso = 1
        AttoDefinitivoConservatoIterConcluso = 2
    End Enum

    Private Class SchedaConformita
        Public Property IdAtto As Integer
        Public Property ContatoreGenerale As Integer
        Public Property DataDocumento As DateTime
        Public Property Oggetto As String
        Public Property DescrizioneSettore As String
        Public Property CodiceSettore As Integer
        Public Property DescrizioneTipologiaRegistro As String

        Public Property DescrizioneTipologiaDocumento As String
        Public Property NomeModulo As String

    End Class

    Private Class DettaglioControlloAmmistrativoSuccessivo
        Public Property IdControlloAmmistrativoSuccessivo As Integer
        Public Property DescrizioneSettore As String
        Public Property CodiceSettore As Integer
        Public Property NumeroAtti As Integer
        Public Property Percentuale As Double
        Public Property NumeroAttiEstratti As Integer
    End Class

    Private Class FilterInfo
        Public Property Value As String
        Public Property ColumnName As String
        Public Property FunctionName As String
        Public Property SortOrder As String
    End Class

#Region "PROPRIETA'"

    Private Property SchedeConformita() As List(Of SchedaConformita)
        Get
            Return CType(Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_SchedeConformita"), List(Of SchedaConformita))
        End Get
        Set(ByVal value As List(Of SchedaConformita))
            Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_SchedeConformita") = value
        End Set
    End Property

    Private Property DettagliControlloAmmistrativoSuccessivo() As List(Of DettaglioControlloAmmistrativoSuccessivo)
        Get
            Return CType(Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_DettagliControlloAmmistrativoSuccessivo"), List(Of DettaglioControlloAmmistrativoSuccessivo))
        End Get
        Set(ByVal value As List(Of DettaglioControlloAmmistrativoSuccessivo))
            Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_DettagliControlloAmmistrativoSuccessivo") = value
        End Set
    End Property

    Private Property Filters() As List(Of FilterInfo)
        Get
            Return CType(Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_SchedeConformita_Filters"), List(Of FilterInfo))
        End Get
        Set(value As List(Of FilterInfo))
            Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_SchedeConformita_Filters") = value
        End Set
    End Property

    Private Property ControlloSuccessivoRegolaritaAmministrativa() As ParsecAtt.ControlloSuccessivoRegolaritaAmministrativa
        Get
            Return CType(Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_ControlloSuccessivoRegolaritaAmministrativa"), ParsecAtt.ControlloSuccessivoRegolaritaAmministrativa)
        End Get
        Set(ByVal value As ParsecAtt.ControlloSuccessivoRegolaritaAmministrativa)
            Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_ControlloSuccessivoRegolaritaAmministrativa") = value
        End Set
    End Property


    Private Property TipologieAttoSoggettaControlloSuccessivoRegolaritaAmministrativa() As List(Of ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa)
        Get
            Return CType(Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa"), List(Of ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa))
        End Get
        Set(ByVal value As List(Of ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa))
            Session("GestioneControlloSuccessivoRegolaritaAmministrativaPage_TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Controllo Successivo di Regolarita Amministrativa"
        If Not Me.Page.IsPostBack Then

            Me.CaricaTipologieDocumento()

            Me.Filters = New List(Of FilterInfo)

            Me.SchedeConformita = New List(Of SchedaConformita)
            Me.DettagliControlloAmmistrativoSuccessivo = New List(Of DettaglioControlloAmmistrativoSuccessivo)


            Dim controlliSuccessivi As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativaRepository
            Dim controllo = controlliSuccessivi.Where(Function(c) c.Eseguito = False).FirstOrDefault
            controlliSuccessivi.Dispose()

            If controllo Is Nothing Then

                Me.EseguiCampionamentoImageButton.Enabled = False
                Me.SalvaImageButton.Enabled = False
                Me.PeriodoRiferimentoLabel.Text = "Nessun controllo disponibile"
                Me.ModificaParametriControlloSuccessivo.Enabled = False
                Me.SalvaButton.Enabled = False
                Exit Sub
            End If

            Me.ControlloSuccessivoRegolaritaAmministrativa = controllo


            Dim ruoli As New ParsecWKF.RuoloRepository
            Dim viewRuoli = ruoli.Where(Function(c) c.Tipologia Is Nothing).Select(Function(c) New With {.Id = c.Id, .Descrizione = c.Descrizione & " (" & c.Id & ")"})
            Me.RuoloComboBox.DataSource = viewRuoli
            Me.RuoloComboBox.DataTextField = "Descrizione"
            Me.RuoloComboBox.DataValueField = "Id"
            Me.RuoloComboBox.DataBind()
            Me.RuoloComboBox.Enabled = False

            ruoli.Dispose()


            Dim dati As New Dictionary(Of String, String)
            dati.Add(CriterioSceltaEstrazione.AttoDefinitivoConservato, "Conservato")
            dati.Add(CriterioSceltaEstrazione.AttoDefinitivoIterConcluso, "Iter Terminato")
            dati.Add(CriterioSceltaEstrazione.AttoDefinitivoConservatoIterConcluso, "Conservato e/o Iter Terminato")
            Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})
            Me.CriterioSceltaComboBox.DataSource = ds
            Me.CriterioSceltaComboBox.DataTextField = "Descrizione"
            Me.CriterioSceltaComboBox.DataValueField = "Id"
            Me.CriterioSceltaComboBox.DataBind()
            Me.CriterioSceltaComboBox.SelectedIndex = 0


            Dim utenti As New ParsecAdmin.UserRepository
            Dim viewUtenti = utenti.Where(Function(c) c.LogTipoOperazione Is Nothing).AsEnumerable.Select(Function(c) New With {.Id = c.Id, .Descrizione = c.Cognome & " " & c.Nome & " (" & c.Id & ")"})
            Me.UtentiComboBox.DataSource = viewUtenti
            Me.UtentiComboBox.DataTextField = "Descrizione"
            Me.UtentiComboBox.DataValueField = "Id"
            Me.UtentiComboBox.DataBind()
            utenti.Dispose()

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            If Not utenteCollegato.SuperUser Then
                Me.NomeFileTemplateTextBox.Enabled = False
                Me.NomeFileTemplateTextBox.ToolTip = "Per modificare questo campo è necessario avere i permessi di SuperUser"
                Me.UtentiComboBox.Enabled = False
                Me.UtentiComboBox.ToolTip = "Per modificare questo campo è necessario avere i permessi di SuperUser"
            End If

            Me.PeriodoRiferimentoLabel.Text = "Periodo di riferimento dal " & controllo.DataEstrazione.Subtract(TimeSpan.FromDays(controllo.Periodicita - 1)).ToShortDateString & " al " & controllo.DataEstrazione.ToShortDateString

            If Now.Date < controllo.DataEstrazione.Date Then
                Me.EseguiCampionamentoImageButton.Enabled = False
                Me.SalvaImageButton.Enabled = False
                Me.PeriodoRiferimentoLabel.Text = "Prossimo controllo il " & controllo.DataEstrazione.ToShortDateString
            End If

        End If

        Me.DocumentiGridView.GroupingSettings.CaseSensitive = False

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.DocumentiGridView.Style.Add("width", widthStyle)
        Me.ControlloSuccessivoMultiPage.Style.Add("width", widthStyle)

        Me.TipologiaAttoPanel.Style.Add("width", widthStyle)
        Me.TipologiaAttoGridView.Style.Add("width", widthStyle)


        Me.ModificaParametriControlloSuccessivo.Attributes.Add("onclick", "ShowPanel();hide=false;SelectTabByIndex(0);")
        'Me.SalvaButton.Attributes.Add("onclick", "if (document.getElementById('" & Me.HideWindow.ClientID & "').value == '') {hide=false;} else {hide=true;}")
        Me.ChiudiButton.Attributes.Add("onclick", "HidePanel();hide=true;return false;")
        Me.ChiudiButton.AutoPostBack = False

    End Sub

#End Region


#Region "EVENTI CONTROLLI"

    Protected Sub EseguiCampionamentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EseguiCampionamentoImageButton.Click


        Dim controlliSuccessivi As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativaRepository
        Dim controllo = controlliSuccessivi.Where(Function(c) c.Eseguito = False).FirstOrDefault
        controlliSuccessivi.Dispose()

        Dim filtro As New ParsecAtt.FiltroDocumento
        filtro.DataDocumentoInizio = controllo.DataEstrazione.Subtract(TimeSpan.FromDays(controllo.Periodicita - 1))
        filtro.DataDocumentoFine = controllo.DataEstrazione


        Me.SchedeConformita = Me.GetSchedeConformita(filtro, controllo.Percentuale, CType(controllo.CriterioScelta, CriterioSceltaEstrazione))

        If Me.SchedeConformita.Count > 0 Then
            Me.DocumentiGridView.Rebind()
        Else
            ParsecUtility.Utility.MessageBox("Nessun atto estratto per il periodo corrente!", False)
        End If


    End Sub


    'https://github.com/hackerb9/passportpix/blob/master/passport.py
    Protected Sub SalvaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles SalvaImageButton.Click

        If Me.SchedeConformita.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario eseguire il campionamento", False)
            Exit Sub
        End If

        '***********************************************************************************************************************
        '1) AGGIORNO IL PRECEDENTE CONTROLLO AMMINISTRATIVO E SALVO IL NUOVO
        '***********************************************************************************************************************
        Dim controlliSuccessivi As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativaRepository
        Dim controllo = controlliSuccessivi.Where(Function(c) c.Eseguito = False).FirstOrDefault
        controllo.Eseguito = True

        Dim nuovoControllo As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativa
        nuovoControllo.DataEstrazione = controllo.DataEstrazione.AddDays(controllo.Periodicita)
        nuovoControllo.Eseguito = False
        nuovoControllo.Periodicita = controllo.Periodicita
        nuovoControllo.Percentuale = controllo.Percentuale
        nuovoControllo.NomefileIter = controllo.NomefileIter
        nuovoControllo.IdModelloIter = controllo.IdModelloIter
        nuovoControllo.IdRuolo = controllo.IdRuolo
        nuovoControllo.IdDestinatarioIter = controllo.IdDestinatarioIter

        nuovoControllo.Controllore = controllo.Controllore
        nuovoControllo.NomeFileTemplate = controllo.NomeFileTemplate


        controlliSuccessivi.Add(nuovoControllo)
        controlliSuccessivi.SaveChanges()
        controlliSuccessivi.Dispose()
        '***********************************************************************************************************************

        '***********************************************************************************************************************
        '2) SALVO I NUOVI DETTAGLI DEL CONTROLLO AMMINISTRATIVO
        '***********************************************************************************************************************
        Dim dettagli As New ParsecAtt.DettaglioControlloSuccessivoRegolaritaAmministrativaRepository

        For Each dettaglioControllo In Me.DettagliControlloAmmistrativoSuccessivo
            Dim nuovoDettaglioControllo As New ParsecAtt.DettaglioControlloSuccessivoRegolaritaAmministrativa
            nuovoDettaglioControllo.CodiceSettore = dettaglioControllo.CodiceSettore
            nuovoDettaglioControllo.DescrizioneSettore = dettaglioControllo.DescrizioneSettore
            nuovoDettaglioControllo.NumeroAtti = dettaglioControllo.NumeroAtti
            nuovoDettaglioControllo.NumeroAttiEstratti = dettaglioControllo.NumeroAttiEstratti
            nuovoDettaglioControllo.Percentuale = dettaglioControllo.Percentuale
            nuovoDettaglioControllo.IdControlloSuccessivoRegolaritaAmministrativa = controllo.Id

            dettagli.Add(nuovoDettaglioControllo)
        Next
        dettagli.SaveChanges()
        dettagli.Dispose()
        '***********************************************************************************************************************


        '***********************************************************************************************************************
        '3) SALVO LE SCHEDE ED AVVIO L'ITER
        '***********************************************************************************************************************

        Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
        Dim dettagliScheda As New ParsecAtt.DettaglioSchedaControlloSuccessivoRegolaritaAmministrativaRepository
        Dim indicatori As New ParsecAtt.IndicatoreControlloSuccessivoRegolaritaAmministrativaRepository

        Dim indicatoriConformitaAbilitati = indicatori.Where(Function(c) c.Abilitato = True).ToList

        Dim nuovaScheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa = Nothing

        Dim nuoviDettagliScheda As ParsecAtt.DettaglioSchedaControlloSuccessivoRegolaritaAmministrativa = Nothing

        Dim idModelloIter As Integer = controllo.IdModelloIter
        Dim idDestinatarioIter As Integer = controllo.IdDestinatarioIter

        Dim idRuoloDirigente As Integer = controllo.IdRuolo

        Dim modelliIter As New ParsecWKF.ModelliRepository
        Dim modelloIter As ParsecWKF.Modello = modelliIter.GetById(idModelloIter)
        modelliIter.Dispose()

        'PER OGNI ATTO ESTRATTO
        For Each schedaConformita In Me.SchedeConformita
            nuovaScheda = New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa

            'DA IMPOSTARE IN FASE DI VALUTAZIONE
            nuovaScheda.Osservazioni = Nothing
            nuovaScheda.DataControllo = Nothing
            nuovaScheda.Nomefile = Nothing

            nuovaScheda.Esito = ParsecAtt.EsitoControlloSuccessivoRegolaritaAmministrativa.Positivo

            nuovaScheda.IdControlloSuccessivoRegolaritaAmministrativa = controllo.Id
            nuovaScheda.DescrizioneSettore = schedaConformita.DescrizioneSettore
            nuovaScheda.DescrizioneTipologiaAtto = schedaConformita.DescrizioneTipologiaDocumento '"Determina"
            nuovaScheda.DataAtto = schedaConformita.DataDocumento
            nuovaScheda.OggettoAtto = schedaConformita.Oggetto
            nuovaScheda.NumeroAtto = schedaConformita.ContatoreGenerale

            nuovaScheda.Controllore = controllo.Controllore

            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim firmaDirigente = documenti.GetFirme(schedaConformita.IdAtto).Where(Function(c) c.IdRuolo = idRuoloDirigente).FirstOrDefault
            documenti.Dispose()


            Dim utenti As New ParsecAdmin.UserRepository
            Dim utente = utenti.Where(Function(c) c.Id = firmaDirigente.IdUtente).FirstOrDefault
            utenti.Dispose()
            If Not utente Is Nothing Then
                nuovaScheda.Responsabile = utente.Cognome & " " & utente.Nome
            End If

            nuovaScheda.DataEstrazione = Now


            'DA IMPOSTARE IN FASE DI FIRMA
            nuovaScheda.NomefileFirmato = Nothing


            schede.Add(nuovaScheda)

            schede.SaveChanges()

            For Each indicatore In indicatoriConformitaAbilitati
                nuoviDettagliScheda = New ParsecAtt.DettaglioSchedaControlloSuccessivoRegolaritaAmministrativa
                nuoviDettagliScheda.IdScheda = nuovaScheda.Id
                nuoviDettagliScheda.IdIndicatore = indicatore.Id
                dettagliScheda.Add(nuoviDettagliScheda)
                dettagliScheda.SaveChanges()
            Next


            'AVVIO L'ITER
            Me.CreaIstanzaScheda(schedaConformita, modelloIter, idDestinatarioIter, firmaDirigente.IdUtente)


            'TODO AGGIORNARE IL  DOCUMENTO docIdSchedaControlloSuccessivo

        Next



        schede.Dispose()
        dettagliScheda.Dispose()
        indicatori.Dispose()

        '***********************************************************************************************************************



        '***********************************************************************************************************************
        'RESETTO
        '***********************************************************************************************************************
        Me.SchedeConformita = New List(Of SchedaConformita)
        Me.DocumentiGridView.Rebind()
        Me.DettagliControlloAmmistrativoSuccessivo = New List(Of DettaglioControlloAmmistrativoSuccessivo)
        '***********************************************************************************************************************

        Me.PeriodoRiferimentoLabel.Text = "Periodo di riferimento dal " & nuovoControllo.DataEstrazione.Subtract(TimeSpan.FromDays(nuovoControllo.Periodicita - 1)).ToShortDateString & " al " & nuovoControllo.DataEstrazione.ToShortDateString

        If Now.Date < nuovoControllo.DataEstrazione.Date Then
            Me.EseguiCampionamentoImageButton.Enabled = False
            Me.SalvaImageButton.Enabled = False
            Me.PeriodoRiferimentoLabel.Text = "Prossimo controllo il " & nuovoControllo.DataEstrazione.ToShortDateString
        End If

    End Sub

    Protected Sub ModificaParametriControlloSuccessivo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ModificaParametriControlloSuccessivo.Click
        Dim controllo = Me.ControlloSuccessivoRegolaritaAmministrativa
        If Not controllo Is Nothing Then
            Me.CaricaParametriControlloSuccessivoRegolaritaAmministrativa(controllo)
        End If
    End Sub

    Protected Sub SalvaButton_Click(sender As Object, e As System.EventArgs) Handles SalvaButton.Click


        Dim messaggio As New System.Text.StringBuilder
        Dim insert As Boolean = False

        If String.IsNullOrEmpty(Me.ControlloreTextBox.Text) Then
            messaggio.AppendLine("Il controllore.")
            insert = True
        End If

        If String.IsNullOrEmpty(Me.NomeFileTemplateTextBox.Text) Then
            messaggio.AppendLine("Il template.")
            insert = True
        End If



        If Not Me.PercentualeTextBox.Value.HasValue Then
            messaggio.AppendLine("La percentuale.")
            insert = True
        End If

        If Not Me.PeriodicitaTextBox.Value.HasValue Then
            messaggio.AppendLine("La periodicità.")
            insert = True
        End If

        Dim localPathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")
        Dim input As String = localPathTemplate & Me.NomeFileTemplateTextBox.Text

        If Not IO.File.Exists(input) Then
            If insert Then
                messaggio.AppendLine("------------------------------------------------------")
            End If

            messaggio.AppendLine(String.Format("Il file '{0}' non esiste!", Me.NomeFileTemplateTextBox.Text))
        End If

        If insert Then
            messaggio.Insert(0, "E' necessario specificare:" & vbCrLf)
        End If

        If messaggio.Length > 0 Then

            ParsecUtility.Utility.MessageBox(messaggio.ToString, False)
            Me.HideWindow.Value = "NO"
            Exit Sub
        End If


        Dim id = Me.ControlloSuccessivoRegolaritaAmministrativa.Id

        Dim controlliSuccessivi As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativaRepository
        Dim controllo = controlliSuccessivi.Where(Function(c) c.Id = id).FirstOrDefault


        If Not controllo Is Nothing Then

            controllo.Periodicita = Me.PeriodicitaTextBox.Value
            controllo.Percentuale = Me.PercentualeTextBox.Value
            controllo.Controllore = Me.ControlloreTextBox.Text
            controllo.NomeFileTemplate = Me.NomeFileTemplateTextBox.Text
            controllo.IdDestinatarioIter = Me.UtentiComboBox.SelectedItem.Value
            controllo.CriterioScelta = Me.CriterioSceltaComboBox.SelectedItem.Value
            controlliSuccessivi.SaveChanges()

            Dim tipologieAtto As New ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativaRepository

            For Each tipologiaAtto In Me.TipologieAttoSoggettaControlloSuccessivoRegolaritaAmministrativa
                Dim tipologiaAttoDaAggiornare = tipologieAtto.Where(Function(c) c.Id = tipologiaAtto.Id).FirstOrDefault
                If Not tipologiaAttoDaAggiornare Is Nothing Then
                    tipologiaAttoDaAggiornare.SoggettoControllo = tipologiaAtto.SoggettoControllo
                End If
            Next
            tipologieAtto.SaveChanges()
            tipologieAtto.Dispose()


            Me.ControlloSuccessivoRegolaritaAmministrativa = controllo


            Me.PeriodoRiferimentoLabel.Text = "Periodo di riferimento dal " & controllo.DataEstrazione.Subtract(TimeSpan.FromDays(controllo.Periodicita - 1)).ToShortDateString & " al " & controllo.DataEstrazione.ToShortDateString

            If Now.Date < controllo.DataEstrazione.Date Then
                Me.EseguiCampionamentoImageButton.Enabled = False
                Me.SalvaImageButton.Enabled = False
                Me.PeriodoRiferimentoLabel.Text = "Prossimo controllo il " & controllo.DataEstrazione.ToShortDateString
            End If



        End If
        controlliSuccessivi.Dispose()
        Me.HideWindow.Value = "SI"


    End Sub

#End Region

#Region "EVENTI GRIGIA"

    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand

        Select Case e.CommandName
            Case Telerik.Web.UI.RadGrid.FilterCommandName

                Dim filter As Pair = CType(e.CommandArgument, Pair)
                Dim nomeColonna As String = filter.Second.ToString
                Dim value As String = Me.DocumentiGridView.MasterTableView.Columns.FindByUniqueName(nomeColonna).CurrentFilterValue
                Dim functionName As String = filter.First.ToString()


                Dim oldInfo As FilterInfo = Me.Filters.Where(Function(c) c.ColumnName = nomeColonna).FirstOrDefault

                If Not oldInfo Is Nothing Then
                    oldInfo.Value = value
                    oldInfo.FunctionName = functionName
                Else
                    Dim newInfo As New FilterInfo With {.Value = value, .ColumnName = nomeColonna, .FunctionName = functionName}
                    Me.Filters.Add(newInfo)
                End If


            Case Telerik.Web.UI.RadGrid.SortCommandName

                Dim sortOrder As String = DirectCast(e, Telerik.Web.UI.GridSortCommandEventArgs).NewSortOrder.ToString
                Dim oldInfo As FilterInfo = Me.Filters.Where(Function(c) c.ColumnName = e.CommandArgument).FirstOrDefault
                If oldInfo Is Nothing Then
                    Dim newInfo As New FilterInfo With {.ColumnName = e.CommandArgument, .SortOrder = sortOrder}

                    'For Each info As FilterInfo In Me.Filters
                    '    info.SortOrder = String.Empty
                    'Next
                    Me.Filters.ForEach(Sub(c) c.SortOrder = String.Empty)

                    Me.Filters.Add(newInfo)
                Else
                    'For Each info As FilterInfo In Me.Filters
                    '    info.SortOrder = String.Empty
                    'Next
                    Me.Filters.ForEach(Sub(c) c.SortOrder = String.Empty)

                    oldInfo.SortOrder = sortOrder
                End If

            Case "Anteprima"
                Me.Anteprima(e.Item)
        End Select

    End Sub

    Protected Sub DocumentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DocumentiGridView.NeedDataSource
        Me.DocumentiGridView.DataSource = Me.SchedeConformita
    End Sub

    Protected Sub DocumentiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As Telerik.Web.UI.RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), Telerik.Web.UI.RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub DocumentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, Telerik.Web.UI.GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.TitoloElencoLabel.Text = "Elenco Atti Soggetti a Controllo&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If
    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub CaricaTipologieDocumento()


        Dim tipologieControllo As New ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativaRepository
        Dim moduli As New ParsecAdmin.ModuleRepository

        Dim res = From tipologia In tipologieControllo.GetQuery.ToList
                  Join modulo In moduli.GetQuery.ToList
                      On tipologia.IdModulo Equals modulo.Id
                  Where modulo.Abilitato = True
                  Select New ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa With {
                      .Id = tipologia.IdTipologia,
                      .Descrizione = tipologia.Descrizione,
                      .NomeModulo = modulo.Descrizione,
                      .SoggettoControllo = tipologia.SoggettoControllo
                   }


        Me.TipologieDocumentoComboBox.DataSource = res.ToList
        Me.TipologieDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologieDocumentoComboBox.DataValueField = "Id"
        Me.TipologieDocumentoComboBox.DataBind()
        Me.TipologieDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.TipologieDocumentoComboBox.SelectedIndex = 0


    End Sub

    Private Sub CaricaParametriControlloSuccessivoRegolaritaAmministrativa(ByVal controllo As ParsecAtt.ControlloSuccessivoRegolaritaAmministrativa)


        Me.PercentualeTextBox.Value = controllo.Percentuale
        Me.PeriodicitaTextBox.Value = controllo.Periodicita
        Me.ControlloreTextBox.Text = controllo.Controllore
        Me.NomeFileIterLabelTextBox.Text = controllo.NomefileIter
        Me.NomeFileTemplateTextBox.Text = controllo.NomeFileTemplate
        Me.NomeFileIterLabelTextBox.Enabled = False


        Dim item = Me.RuoloComboBox.FindItemByValue(controllo.IdRuolo)
        If Not item Is Nothing Then
            item.Selected = True
        End If

        item = Me.UtentiComboBox.FindItemByValue(controllo.IdDestinatarioIter)
        If Not item Is Nothing Then
            item.Selected = True
        End If

        item = Me.CriterioSceltaComboBox.FindItemByValue(controllo.CriterioScelta)
        If Not item Is Nothing Then
            item.Selected = True
        End If



        'Me.ControlloSuccessivoTabStrip.Tabs(0).Selected = True
        'Me.ControlloSuccessivoMultiPage.PageViews(0).Selected = True

        Dim tipologieControllo As New ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativaRepository


        Dim moduli = New ParsecAdmin.ModuleRepository

        Dim res = From tipologia In tipologieControllo.GetQuery.ToList
                  Join modulo In moduli.GetQuery.ToList
                      On tipologia.IdModulo Equals modulo.Id
                  Where modulo.Abilitato = True
                  Select New ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa With {
                      .Id = tipologia.Id,
                      .Descrizione = tipologia.Descrizione,
                      .NomeModulo = modulo.Descrizione,
                      .SoggettoControllo = tipologia.SoggettoControllo
                   }


        Me.TipologieAttoSoggettaControlloSuccessivoRegolaritaAmministrativa = res.ToList



        Me.TipologiaAttoGridView.Rebind()

        tipologieControllo.Dispose()

    End Sub

    '*************************************************************************************************************
    'INSERISCO IL TASK AUTOMATICO
    '*************************************************************************************************************
    Private Sub Procedi(ByVal taskAttivo As ParsecWKF.Task, ByVal idIstanza As Integer, ByVal idUtente As Integer, ByVal idDestinatario As Integer, ByVal nomeFileIter As String)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim statoEseguito As Integer = 6
        Dim statoDaEseguire As Integer = 5
        Dim statoIstanzaCompletato As Integer = 3

        Dim tasks As New ParsecWKF.TaskRepository
        tasks.Attach(taskAttivo)

        'Dim taskAttivo As ParsecWKF.TaskAttivo = tasks.GetView(New ParsecWKF.TaskFiltro With {.IdUtente = idUtente, .IdModulo = 1001}).Where(Function(c) c.IdIstanza = idIstanza).FirstOrDefault

        Dim actions = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Corrente, nomeFileIter)

        Dim taskSuccessivoAutomatico As ParsecWKF.ParametroProcesso = actions(0).Parameters.Where(Function(c) c.Nome = "TaskSuccessivoAutomatico").FirstOrDefault

        Dim azione As ParsecWKF.Action = Nothing

        If Not taskSuccessivoAutomatico Is Nothing Then
            azione = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Successivo, nomeFileIter).Where(Function(c) c.Name = taskSuccessivoAutomatico.Valore).FirstOrDefault
        Else
            azione = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Successivo, nomeFileIter)(0)
        End If

        Dim operazione As String = "INIZIO"
        Dim procediAction = actions.Where(Function(c) c.Type = "INIZIO").FirstOrDefault
        If Not procediAction Is Nothing Then
            If Not String.IsNullOrEmpty(procediAction.Description) Then
                operazione = procediAction.Description.ToUpper
            End If
        End If


        'Recupero il nome del ruolo (To) associato all'azione.
        Dim roleToName = azione.ToActor
        Dim roleFromName = azione.FromActor
        If roleToName <> roleFromName Then
            idUtente = idDestinatario
        End If


        '*************************************************************************************************************
        'AGGIORNO IL TASK PRECEDENTE
        '*************************************************************************************************************
        'Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = taskAttivo.Id).FirstOrDefault

        'If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
        '    task.Note = Me.NoteInterneTextBox.Text
        'End If


        taskAttivo.IdStato = statoEseguito
        taskAttivo.DataEsecuzione = Now
        taskAttivo.Operazione = operazione
        taskAttivo.Destinatario = idUtente
        taskAttivo.Notificato = True
        tasks.SaveChanges()
        '*************************************************************************************************************

        '*************************************************************************************************************
        'INSERISCO IL NUOVO TASK
        '*************************************************************************************************************
        Dim statoSuccessivo As String = ParsecWKF.ModelloInfo.StatoSuccessivoAction(taskAttivo.Successivo, azione.Name, nomeFileIter)
        Dim durata As Integer = 0

        If Not String.IsNullOrEmpty(statoSuccessivo) Then
            durata = ParsecWKF.ModelloInfo.DurataTaskIter(statoSuccessivo, nomeFileIter)
        End If


        'ANNULLO L'ISTANZA
        If azione.Type = "FINE" Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = idIstanza).FirstOrDefault
            istanza.IdStato = statoIstanzaCompletato
            istanza.DataEsecuzione = Now
            istanze.SaveChanges()
            istanze.Dispose()
        End If

        Dim nuovotask As New ParsecWKF.Task
        nuovotask.IdIstanza = taskAttivo.IdIstanza
        nuovotask.TaskPadre = taskAttivo.Id

        nuovotask.Nome = taskAttivo.Corrente

        nuovotask.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo, nomeFileIter)

        nuovotask.Mittente = idUtente

        If azione.Type = "FINE" Then
            nuovotask.IdStato = statoEseguito
            nuovotask.Corrente = "FINE"
            nuovotask.Destinatario = idUtente
            nuovotask.Operazione = "FINE"
            nuovotask.DataEsecuzione = Now
        Else
            nuovotask.IdStato = statoDaEseguire
            nuovotask.Corrente = statoSuccessivo
        End If

        nuovotask.DataInizio = Now

        'If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
        '    nuovotask.Note = Me.NoteInterneTextBox.Text
        'End If


        nuovotask.DataFine = Now.AddDays(durata)
        nuovotask.Cancellato = False
        nuovotask.Notificato = False
        nuovotask.IdUtenteOperazione = utenteCollegato.Id

        tasks.Add(nuovotask)
        tasks.SaveChanges()
        '*************************************************************************************************************

        tasks.Dispose()
    End Sub

    Private Sub CreaIstanzaScheda(ByVal scheda As SchedaConformita, ByVal modelloIter As ParsecWKF.Modello, ByVal IdDestinatario As Integer, ByVal idUtenteDirigente As Integer)

        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanzaPrecedente As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.ContatoreGenerale = scheda.ContatoreGenerale AndAlso c.IdModulo = 1001 AndAlso Year(c.DataInserimento) = Year(scheda.DataDocumento)).FirstOrDefault

        'Se l'istanza del documento non è in iter
        If istanzaPrecedente Is Nothing Then

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

            Dim statoIniziale As Integer = 1
            Dim statoDaEseguire As Integer = 5

            Dim idIstanza As Integer = 0

            'mittente del protocollo 
            Dim idMittente As Integer = utenteCollegato.Id

            Dim descrizioneDocumento As String = String.Format("{0} n. {1}/{2} - Oggetto : {3}", scheda.DescrizioneTipologiaRegistro, scheda.ContatoreGenerale.ToString, scheda.DataDocumento.Year.ToString, scheda.Oggetto)

            Dim istanza As New ParsecWKF.Istanza
            istanza.Riferimento = "C.S.R.A. -" & descrizioneDocumento
            istanza.IdStato = statoIniziale
            istanza.DataInserimento = Now
            istanza.DataScadenza = Now.AddDays(modelloIter.DurataIter)
            istanza.IdModello = modelloIter.Id
            istanza.IdDocumento = scheda.IdAtto
            'istanza.Ufficio = 0
            istanza.Ufficio = IdDestinatario
            istanza.ContatoreGenerale = scheda.ContatoreGenerale
            istanza.IdModulo = modelloIter.RiferimentoModulo '1004 CONTROLLO AMMINISTRATIVO
            istanza.IdUtente = idMittente
            istanza.FileIter = modelloIter.NomeFile

            Try
                istanze.Save(istanza)
                idIstanza = istanze.Istanza.Id
                istanze.Dispose()

                '*******************************************************************************************************************************
                'Inserisco nei parametri del processo l'attore DESTINATARIO corrente.
                Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
                Dim processo As New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "SEGRETARIO", .Valore = IdDestinatario.ToString}
                parametriProcesso.Add(processo)
                parametriProcesso.SaveChanges()


                processo = New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "DIRIGENTE", .Valore = idUtenteDirigente.ToString}
                parametriProcesso.Add(processo)
                parametriProcesso.SaveChanges()



                'Inserisco nei parametri del processo l'attore MITTENTE se non esiste.
                Dim parametroProcessoExist As Boolean = Not parametriProcesso.GetQuery.Where(Function(c) c.IdProcesso = idIstanza And c.Nome = "MITTENTE").FirstOrDefault Is Nothing
                If Not parametroProcessoExist Then
                    processo = New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "MITTENTE", .Valore = idMittente.ToString}
                    parametriProcesso.Add(processo)
                    parametriProcesso.SaveChanges()
                End If

                parametriProcesso.Dispose()
                '*******************************************************************************************************************************

                'Inserisco il task dell'istanza appena inserita
                Dim tasks As New ParsecWKF.TaskRepository

                Dim task As New ParsecWKF.Task
                task.IdIstanza = idIstanza
                task.Nome = ""

                Dim corrente = modelloIter.StatoIniziale
                task.Corrente = corrente
                task.Successivo = modelloIter.StatoSuccessivo(corrente)

                task.Mittente = idMittente
                'task.Destinatario = IdDestinatario
                task.IdStato = statoDaEseguire
                task.DataInizio = Now
                task.DataEsecuzione = Now
                task.DataFine = Now.AddDays(modelloIter.DurataTask)
                task.Operazione = "NUOVO"
                task.Cancellato = False
                task.IdUtenteOperazione = utenteCollegato.Id


                'Dim list As List(Of ParsecWKF.Action) = ParsecWKF.ModelloInfo.ReadActionInfo(task.Corrente, modelloIter.NomeFile)
                ''Recupero il nome del ruolo (To) associato all'azione.
                'Dim roleToName = list(0).ToActor

                'Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleToName).FirstOrDefault
                'Dim idRuolo As Integer
                'If Not role Is Nothing Then
                '    idRuolo = role.Id
                '    Dim ruoloUtente As ParsecWKF.RuoloRelUtente = (New ParsecWKF.RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault
                '    If Not ruoloUtente Is Nothing Then
                '        idMittente = ruoloUtente.IdUtente
                '    End If
                'End If

                Try

                    tasks.Save(task)
                    tasks.Dispose()

                    'istanza = istanze.GetById(istanza.Id)
                    'Dim nomeFileIter As String = modelloIter.NomeFile
                    'istanza.FileIter = nomeFileIter
                    'istanze.SaveChanges()


                    'Aggiungo il nuovo task
                    Me.Procedi(task, istanza.Id, idMittente, IdDestinatario, istanza.FileIter)


                Catch ex As Exception
                    Throw New ApplicationException(ex.Message)
                End Try

            Catch ex As Exception
                Throw New ApplicationException(ex.Message)
            End Try

        End If

    End Sub

    '********************************************************************************************************************************
    'RESTITUISCE UNA LISTA DI DETERMINE FILTRATA IN BASE AI FILTRI IMPOSTATI NELLA GRIGLIA ED ORDINATA IN BASE ALLA COLONNA
    '********************************************************************************************************************************
    Private Function FiltraDocumenti() As List(Of SchedaConformita)
        Dim res = Me.SchedeConformita

        If Not Me.Filters Is Nothing Then
            Dim key As String = String.Empty
            For Each f As FilterInfo In Me.Filters
                key = f.Value
                If Not String.IsNullOrEmpty(key) Then
                    key = key.ToLower
                    Select Case f.FunctionName
                        Case "NoFilter"
                            Exit Select
                        Case "Contains"
                            Select Case f.ColumnName
                                Case "Oggetto"
                                    res = res.Where(Function(c) c.Oggetto.ToLower.Contains(key)).ToList
                                Case "DescrizioneSettore"
                                    res = res.Where(Function(c) c.DescrizioneSettore.ToLower.Contains(key)).ToList
                            End Select
                        Case "EqualTo"
                            Select Case f.ColumnName
                                Case "ContatoreGenerale"
                                    res = res.Where(Function(c) c.ContatoreGenerale = CInt(key)).ToList
                            End Select
                    End Select
                End If

                Dim order As String = String.Empty
                If Not String.IsNullOrEmpty(f.SortOrder) Then
                    order = f.SortOrder.ToLower
                End If

                Select Case f.ColumnName

                    Case "ContatoreGenerale"
                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.ContatoreGenerale).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.ContatoreGenerale).ToList
                        End Select



                    Case "Oggetto"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.Oggetto).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.Oggetto).ToList
                        End Select



                    Case "DescrizioneSettore"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.DescrizioneSettore).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.DescrizioneSettore).ToList
                        End Select

                    Case "DataDocumento"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.DataDocumento).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.DataDocumento).ToList
                        End Select

                End Select

            Next

        End If

        Return res

    End Function



    Private Function GetSchedeConformitaATT(ByVal filtro As ParsecAtt.FiltroDocumento, ByVal percentualeEstratta As Integer, ByVal criterioScelta As CriterioSceltaEstrazione, ByVal tipologia As ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa) As List(Of SchedaConformita)



        Dim estratte As New List(Of SchedaConformita)


        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim strutture As New ParsecAtt.StrutturaViewRepository(documenti.Context)
        Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository(documenti.Context)


        'DETERMINE E ORDINANZE CORRENTI CONSERVATE O IL CUI ITER E' COMPLETATO (IN BASE ALLA CONFIGURAZIONE)
        Dim queryBase = documenti.Where(Function(c) c.LogStato Is Nothing And (c.IdTipologiaDocumento = CType(tipologia.IdTipologia, ParsecAtt.TipoDocumento))).Select(Function(c) New With {c.Id, c.Data, c.IdStruttura, c.ContatoreGenerale, c.Oggetto, c.IdTipologiaRegistro, c.IdDocumentoWS, c.IdTipologiaDocumento})

        Dim statoIstanzaCompletato As Integer = 3

        Dim view = From documento In queryBase
                   Group Join settore In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione, c.Codice})
                   On settore.Id Equals documento.IdStruttura
                   Into elencoSettori = Group
                   From settore In elencoSettori.DefaultIfEmpty
                   Join tipoRegistro In tipologieRegistro.GetQuery
                   On documento.IdTipologiaRegistro Equals tipoRegistro.Id
                   Order By settore.Descrizione
                   Select New With {documento, .DescrizioneSettore = settore.Descrizione, .CodiceSettore = settore.Codice, .DescrizioneTipologiaRegistro = tipoRegistro.Descrizione}

        Select Case criterioScelta

            Case CriterioSceltaEstrazione.AttoDefinitivoConservato
                view = view.Where(Function(c) c.documento.IdDocumentoWS.HasValue)

            Case CriterioSceltaEstrazione.AttoDefinitivoIterConcluso

                Dim istanze As New ParsecCommon.RepositoryBase(Of ParsecAtt.IstanzaWKF)(documenti.Context)

                view = From item In view
                       Join istanza In istanze.GetQuery
                       On item.documento.Id Equals istanza.IdDocumento
                       Where istanza.IdStato = statoIstanzaCompletato
                       Order By item.DescrizioneSettore
                       Select item
            Case CriterioSceltaEstrazione.AttoDefinitivoConservatoIterConcluso

                Dim attiConservati = view.Where(Function(c) c.documento.IdDocumentoWS.HasValue)

                Dim istanze As New ParsecCommon.RepositoryBase(Of ParsecAtt.IstanzaWKF)(documenti.Context)

                Dim attiConclusi = From item In view
                                   Join istanza In istanze.GetQuery
                                   On item.documento.Id Equals istanza.IdDocumento
                                   Where istanza.IdStato = statoIstanzaCompletato
                                   Order By item.DescrizioneSettore
                                   Select item

                view = attiConservati.Union(attiConclusi)

        End Select


        If Not filtro Is Nothing Then

            If filtro.DataDocumentoInizio.HasValue Then
                Dim d As Date = filtro.DataDocumentoInizio.Value
                Dim startDate As Date = New Date(d.Year, d.Month, d.Day, 0, 0, 0)
                view = view.Where(Function(c) c.documento.Data >= startDate)
            End If

            If filtro.DataDocumentoFine.HasValue Then
                Dim d As Date = filtro.DataDocumentoFine.Value
                Dim endDate As Date = New Date(d.Year, d.Month, d.Day, 23, 59, 59)
                view = view.Where(Function(c) c.documento.Data <= endDate)
            End If
        End If


        'Dim querySql = CType(distinctView, Data.Objects.ObjectQuery).ToTraceString



        Dim res = view.AsEnumerable.Select(Function(c) New SchedaConformita With
                                          {
                                           .IdAtto = c.documento.Id,
                                           .ContatoreGenerale = c.documento.ContatoreGenerale,
                                           .DataDocumento = c.documento.Data,
                                           .Oggetto = c.documento.Oggetto,
                                           .DescrizioneSettore = c.DescrizioneSettore,
                                           .CodiceSettore = c.CodiceSettore,
                                           .DescrizioneTipologiaRegistro = c.DescrizioneTipologiaRegistro,
                                           .DescrizioneTipologiaDocumento = CType(c.documento.IdTipologiaDocumento, ParsecAtt.TipoDocumento).ToString,
                                           .NomeModulo = "ATT"
                                       }).ToList




        Dim listaCodici = res.Select(Function(c) c.CodiceSettore).Distinct

        Dim numeroAttiPerSettore As Integer = 0
        Dim cs As Integer = 0
        Dim numeroAttiPerSettoreEstratti As Integer = 0
        Dim percentuale As Double = 0.0
        Dim attiPerSettore As List(Of SchedaConformita)

        Dim rnd As Random = Nothing

        Dim dettaglio As DettaglioControlloAmmistrativoSuccessivo = Nothing

        For Each codice In listaCodici

            rnd = New Random
            cs = codice

            attiPerSettore = res.Where(Function(c) c.CodiceSettore = cs).ToList
            numeroAttiPerSettore = attiPerSettore.Count
            percentuale = numeroAttiPerSettore * percentualeEstratta / 100
            numeroAttiPerSettoreEstratti = CInt(Math.Ceiling(percentuale))



            attiPerSettore = attiPerSettore.OrderBy(Function(x) rnd.Next).Take(numeroAttiPerSettoreEstratti).ToList
            estratte.AddRange(attiPerSettore)

            'determinePerSettore = Me.Randomize(determinePerSettore)
            'estratte.AddRange(determinePerSettore.Take(numeroDeterminePerSettoreEstratte))

            dettaglio = New DettaglioControlloAmmistrativoSuccessivo
            dettaglio.CodiceSettore = codice
            dettaglio.DescrizioneSettore = attiPerSettore.Select(Function(c) c.DescrizioneSettore).FirstOrDefault

            dettaglio.NumeroAtti = numeroAttiPerSettore
            dettaglio.NumeroAttiEstratti = numeroAttiPerSettoreEstratti

            dettaglio.Percentuale = percentuale
            'dettaglio.IdControlloAmmistrativoSuccessivo = 0

            Me.DettagliControlloAmmistrativoSuccessivo.Add(dettaglio)

        Next

        Return estratte
    End Function



    Private Function GetSchedeConformita(ByVal filtro As ParsecAtt.FiltroDocumento, ByVal percentualeEstratta As Integer, ByVal criterioScelta As CriterioSceltaEstrazione) As List(Of SchedaConformita)


        Me.DettagliControlloAmmistrativoSuccessivo.Clear()

        Dim tipologieAttiSoggetteControllo As New ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativaRepository
        Dim tipologie = tipologieAttiSoggetteControllo.Where(Function(c) c.SoggettoControllo = True).ToList
        tipologieAttiSoggetteControllo.Dispose()

        Dim estratte As New List(Of SchedaConformita)

        If Me.TipologieDocumentoComboBox.SelectedIndex = 0 Then
            For Each tipologia As ParsecAtt.TipologiaAttoSoggettaControlloSuccessivoRegolaritaAmministrativa In tipologie
                Select Case tipologia.IdModulo
                    Case ParsecAdmin.TipoModulo.ATT

                        Dim estratteATT = Me.GetSchedeConformitaATT(filtro, percentualeEstratta, criterioScelta, tipologia)
                        estratte.AddRange(estratteATT)
                        'TODO ALTRI MODULI

                End Select

            Next
        Else
            'TODO ALTRI MODULI
            Dim tipologia = tipologie.Where(Function(c) c.IdTipologia = Me.TipologieDocumentoComboBox.SelectedValue And c.IdModulo = ParsecAdmin.TipoModulo.ATT).FirstOrDefault
            If Not tipologia Is Nothing Then
                Dim estratteATT = Me.GetSchedeConformitaATT(filtro, percentualeEstratta, criterioScelta, tipologia)
                estratte.AddRange(estratteATT)
            End If

        End If

        Return estratte
    End Function

    Private Function Randomize(Of T)(list As List(Of T)) As List(Of T)
        Dim randomizedList As New List(Of T)
        Dim rnd As New Random
        While list.Count > 0
            'Recupero un elemento random dalla lista principale
            Dim index As Integer = rnd.Next(0, list.Count)
            'Lo inserisco alla fine della lista casualizzata
            randomizedList.Add(list(index))
            'Rimuovo l'elemento dalla lista principale
            list.RemoveAt(index)
        End While
        Return randomizedList
    End Function

    'Fisher-Yates algoritmo
    Private Sub Shuffle(Of T)(list As IList(Of T))
        Dim rnd As New Random
        Dim n As Integer = list.Count
        While n > 1
            n -= 1
            Dim k As Integer = rnd.Next(n + 1)
            Dim value As T = list(k)
            list(k) = list(n)
            list(n) = value
        End While
    End Sub

    Private Sub Anteprima(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdAtto")
        Dim queryString As New Hashtable
        queryString.Add("Tipo", 2)
        queryString.Add("Mode", "View")
        queryString.Add("Procedura", "10")
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AttoAmministrativoPage.aspx"
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdDocumentoIter", id)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 650, queryString, False)
    End Sub

    Private Sub TipologiaAttoGridView_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles TipologiaAttoGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item


            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then


                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                If dataItem.DataItem.SoggettoControllo Then
                    btn.ImageUrl = "~\images\checkbox_checked.png"
                    btn.ToolTip = "Disabilita"
                Else
                    btn.ImageUrl = "~\images\checkbox_unchecked.png"
                    btn.ToolTip = "Abilita"
                End If


            End If
        End If


    End Sub

    Private Sub TipologiaAttoGridView_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles TipologiaAttoGridView.NeedDataSource
        Me.TipologiaAttoGridView.DataSource = Me.TipologieAttoSoggettaControlloSuccessivoRegolaritaAmministrativa
    End Sub


    Protected Sub TipologiaAttoGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TipologiaAttoGridView.ItemCommand
        If e.CommandName = "Select" Then
            Dim id As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"))
            Dim tipologia = Me.TipologieAttoSoggettaControlloSuccessivoRegolaritaAmministrativa.Where(Function(c) c.Id = id).FirstOrDefault
            If Not tipologia Is Nothing Then
                tipologia.SoggettoControllo = Not tipologia.SoggettoControllo
                TipologiaAttoGridView.Rebind()
            End If
        End If
    End Sub

    Protected Sub TipologiaAttoGridView_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles TipologiaAttoGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub



#End Region

End Class