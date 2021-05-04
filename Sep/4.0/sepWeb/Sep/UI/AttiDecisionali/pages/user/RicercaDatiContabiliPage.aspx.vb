Partial Class RicercaDatiContabiliPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Public Enum TipologiaDatoContabile
        Impegno = 0
        Liquidazione = 1
        Accertamento = 2
    End Enum

    Public Class DettaglioDetermina
        Public Property Id As Integer
        Public Property IdModello As Integer
        Public Property ContatoreGenerale As Integer
        Public Property DescrizioneTipologia As String
        Public Property DataDocumento As DateTime
        Public Property Oggetto As String
        Public Property DescrizioneUfficio As String
        Public Property DescrizioneSettore As String
        Public Property TipologiaDatoContabile As TipologiaDatoContabile
    End Class

    Public Class FilterInfo
        Public Property Value As String
        Public Property ColumnName As String
        Public Property FunctionName As String
        Public Property SortOrder As String
    End Class

#Region "PROPRIETA'"

    Public Property Documenti() As List(Of DettaglioDetermina)
        Get
            Return CType(Session("RicercaDatiContabiliPage_Documenti"), List(Of DettaglioDetermina))
        End Get
        Set(ByVal value As List(Of DettaglioDetermina))
            Session("RicercaDatiContabiliPage_Documenti") = value
        End Set
    End Property

    Public Property Filters() As List(Of FilterInfo)
        Get
            Return CType(Session("RicercaDatiContabiliPage_Filters"), List(Of FilterInfo))
        End Get
        Set(value As List(Of FilterInfo))
            Session("RicercaDatiContabiliPage_Filters") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Dim referentiEsterni As New ParsecPro.ReferentiEsterniRepository
        'Dim rubrica As New ParsecPro.StrutturaEsternaViewRepository(referentiEsterni.Context)
        'Dim view = From referente In referentiEsterni.GetQuery
        '           Join contatto In rubrica.GetQuery On referente.Id Equals contatto.Id
        '           Select referente.IdRegistrazione, contatto.Nome

        'Dim view2 = view.AsEnumerable.GroupBy(Function(c) c.IdRegistrazione).Select(Function(c) New With {.Elenco = c.Select(Function(v) v.Nome)})
        'Dim rr = view2.Select(Function(c) String.Join(", ", c.Elenco)).ToList

        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Ricerca Determine per Dati Contabili"
        If Not Me.Page.IsPostBack Then
            Me.Documenti = Nothing
            Me.CaricaAnni()
            Me.ResettaFiltro()
        End If

        Me.DocumentiGridView.GroupingSettings.CaseSensitive = False

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.DocumentiGridView.Style.Add("width", widthStyle)

    End Sub

#End Region


#Region "EVENTI CONTROLLI"

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FiltraImageButton.Click
        If Not Me.AccertamentoCheckBox.Checked AndAlso Not Me.ImpegnoSpesaCheckBox.Checked AndAlso Not Me.LiquidazioneCheckBox.Checked Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una tipologia di dato contabile!", False)
            Exit Sub
        End If
        Me.AggiornaGriglia()
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.AggiornaGriglia()
    End Sub

    Protected Sub EsportaInExcelImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EsportaInExcelImageButton.Click
        Me.EsportaInExcel()
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
        If Me.Documenti Is Nothing Then
            Me.Documenti = Me.GetDetermineByDatiContabili(Me.GetFiltro)
        End If
        Me.DocumentiGridView.DataSource = Me.Documenti
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
            Me.TitoloElencoLabel.Text = "Elenco Determine&nbsp;&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim btn As ImageButton = CType(dataItem.FindControl("IdStato"), ImageButton)
            btn.Attributes.Add("onclick", "return false")

            Dim dettaglio As DettaglioDetermina = CType(e.Item.DataItem, DettaglioDetermina)

            Select Case dettaglio.TipologiaDatoContabile
                Case TipologiaDatoContabile.Impegno
                    btn.ImageUrl = "~/images/pArancio16.png"
                    btn.ToolTip = "Impegno"
                Case TipologiaDatoContabile.Liquidazione
                    btn.ImageUrl = "~/images/pRosso16.png"
                    btn.ToolTip = "Liquidazione"
                Case TipologiaDatoContabile.Accertamento
                    btn.ImageUrl = "~/images/pVerde16.png"
                    btn.ToolTip = "Accertamento"
                Case Else
                    btn.ImageUrl = "~/images/vuoto.png"
                    btn.ToolTip = "Dati contabili assenti"
            End Select

        End If
    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub EsportaInExcel()

        Dim res = Me.FiltraDocumenti

        If res.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Non ci sono determine." & vbCrLf & "Impossibile eseguire l'esportazione!", False)
            Exit Sub
        End If

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("Determine_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As New StringBuilder

        line.Append("DATO CONTABILE" & vbTab)
        line.Append("NUMERO" & vbTab)
        line.Append("TIPOLOGIA" & vbTab)
        line.Append("DATA" & vbTab)
        line.Append("OGGETTO" & vbTab)
        line.Append("UFFICIO" & vbTab)
        line.Append("SETTORE" & vbTab)

        swExport.WriteLine(line.ToString)
        line.Clear()
        For Each item As DettaglioDetermina In res

            line.Append(item.TipologiaDatoContabile.ToString & vbTab)

            line.Append(item.ContatoreGenerale.ToString & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(item.DescrizioneTipologia), System.Text.RegularExpressions.Regex.Replace(item.DescrizioneTipologia.Trim, "[^\u0020-\u00FF]", " "), " ") & vbTab)
            line.Append(item.DataDocumento.ToShortDateString & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(item.Oggetto), System.Text.RegularExpressions.Regex.Replace(item.Oggetto.Trim, "[^\u0020-\u00FF]", " "), " ") & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(item.DescrizioneUfficio), System.Text.RegularExpressions.Regex.Replace(item.DescrizioneUfficio.Trim, "[^\u0020-\u00FF]", " "), " ") & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(item.DescrizioneSettore), System.Text.RegularExpressions.Regex.Replace(item.DescrizioneSettore.Trim, "[^\u0020-\u00FF]", " "), " ") & vbTab)

            swExport.WriteLine(line.ToString)
            line.Clear()
        Next
        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

    End Sub

    '********************************************************************************************************************************
    'RESTITUISCE UNA LISTA DI DETERMINE FILTRATA IN BASE AI FILTRI IMPOSTATI NELLA GRIGLIA ED ORDINATA IN BASE ALLA COLONNA
    '********************************************************************************************************************************
    Private Function FiltraDocumenti() As List(Of DettaglioDetermina)
        Dim res = Me.Documenti

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
                                Case "DescrizioneTipologia"
                                    res = res.Where(Function(c) c.DescrizioneTipologia.ToLower.Contains(key)).ToList
                                Case "Oggetto"
                                    res = res.Where(Function(c) c.Oggetto.ToLower.Contains(key)).ToList
                                Case "DescrizioneUfficio"
                                    res = res.Where(Function(c) c.DescrizioneUfficio.ToLower.Contains(key)).ToList
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

                    Case "DescrizioneTipologia"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.DescrizioneTipologia).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.DescrizioneTipologia).ToList
                        End Select

                    Case "Oggetto"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.Oggetto).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.Oggetto).ToList
                        End Select

                    Case "DescrizioneUfficio"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.DescrizioneUfficio).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.DescrizioneUfficio).ToList
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

    Private Function GetDetermineByDatiContabili(ByVal filtro As ParsecAtt.FiltroDocumento) As List(Of DettaglioDetermina)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim funzioni As New ParsecAdmin.FunzioniUtenteRepository
        Dim abilitatoVisibilita As Boolean = Not funzioni.GetQuery.Where(Function(c) c.IdFunzione = 45 And c.IdUtente = utenteCollegato.Id).FirstOrDefault Is Nothing
        funzioni.Dispose()

        Dim determineImpegniSpese As List(Of DettaglioDetermina) = Nothing
        Dim determineLiquidazione As List(Of DettaglioDetermina) = Nothing
        Dim determineAccertamenti As List(Of DettaglioDetermina) = Nothing
        Dim res As List(Of DettaglioDetermina) = Nothing

        Try
            If Me.ImpegnoSpesaCheckBox.Checked Then
                determineImpegniSpese = Me.GetDetermineByImpegniSpesa(filtro, abilitatoVisibilita, utenteCollegato.Id)
            Else
                determineImpegniSpese = New List(Of DettaglioDetermina)
            End If
            If Me.LiquidazioneCheckBox.Checked Then
                determineLiquidazione = Me.GetDetermineByLiquidazioni(filtro, abilitatoVisibilita, utenteCollegato.Id)
            Else
                determineLiquidazione = New List(Of DettaglioDetermina)
            End If
            If Me.AccertamentoCheckBox.Checked Then
                determineAccertamenti = Me.GetDetermineByAccertamenti(filtro, abilitatoVisibilita, utenteCollegato.Id)
            Else
                determineAccertamenti = New List(Of DettaglioDetermina)
            End If

            res = determineImpegniSpese.Union(determineLiquidazione).Union(determineAccertamenti).OrderBy(Function(c) c.ContatoreGenerale).ThenBy(Function(c) c.DataDocumento).ToList

        Catch ex As Exception
            Dim messaggio As String = String.Empty
            If Not ex.InnerException Is Nothing Then
                messaggio = ex.InnerException.Message
            Else
                messaggio = ex.Message
            End If
            ParsecUtility.Utility.MessageBox(messaggio, False)
            Return New List(Of DettaglioDetermina)
        End Try

        Return res
    End Function

    Private Function GetDetermineByImpegniSpesa(ByVal filtro As ParsecAtt.FiltroDocumento, ByVal abilitatoVisibilita As Boolean, ByVal idUtente As Integer) As List(Of DettaglioDetermina)

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim strutture As New ParsecAtt.StrutturaViewRepository(documenti.Context)
        Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository(documenti.Context)
        Dim impegniSpesa As New ParsecAtt.ImpegnoSpesaRepository(documenti.Context)

        'DETERMINE CORRENTI
        Dim queryBase = documenti.Where(Function(c) c.LogStato Is Nothing And c.IdTipologiaDocumento = 2).Select(Function(c) New With {c.Id, c.Data, c.IdUfficio, c.IdStruttura, c.IdTipologiaRegistro, c.IdModello, c.ContatoreGenerale, c.Oggetto})

        If Not filtro Is Nothing Then
            If Not abilitatoVisibilita Then
                If filtro.ApplicaAbilitazione.HasValue Then
                    If filtro.ApplicaAbilitazione.Value Then

                        Dim visibilita As New ParsecAtt.VisibilitaDocumentoViewRepository(documenti.Context)
                        Dim gruppi As New ParsecAtt.GruppoViewRepository(documenti.Context)
                        Dim utentiGruppo As New ParsecAtt.GruppoUtenteViewRepository(documenti.Context)

                        Dim queryBaseVisibilita = visibilita.Where(Function(c) c.IdModulo = ParsecAdmin.TipoModulo.ATT).Select(Function(c) New With {c.Id, c.IdDocumento, c.IdEntita, c.TipoEntita})
                        Dim queryBaseGruppo = gruppi.GetQuery.Select(Function(c) New With {c.Id})
                        Dim queryBaseUtenteGruppo = utentiGruppo.Where(Function(c) c.IdUtente = idUtente)

                        queryBase = (From documento In queryBase
                                     Join entita In queryBaseVisibilita
                                     On documento.Id Equals entita.IdDocumento
                                     Join gruppo In queryBaseGruppo
                                     On entita.IdEntita Equals gruppo.Id
                                     Join ug In queryBaseUtenteGruppo
                                     On ug.IdGruppo Equals gruppo.Id
                                     Where entita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
                                     Select documento).Union(
                                     From documento In queryBase
                                     Join entita In queryBaseVisibilita
                                     On documento.Id Equals entita.IdDocumento
                                     Where entita.IdEntita = idUtente And entita.TipoEntita = ParsecAdmin.TipoEntita.Utente
                                     Select documento)

                        'Dim s = CType(queryBase, Data.Objects.ObjectQuery).ToTraceString
                    End If
                End If
            End If

        End If

        Dim view = From documento In queryBase
                   Group Join ufficio In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On ufficio.Id Equals documento.IdUfficio
                   Into elencoUffici = Group
                   From ufficio In elencoUffici.DefaultIfEmpty
                   Group Join settore In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On settore.Id Equals documento.IdStruttura
                   Into elencoSettori = Group
                   From settore In elencoSettori.DefaultIfEmpty
                   Join tipoRegistro In tipologieRegistro.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On documento.IdTipologiaRegistro Equals tipoRegistro.Id
                   Join impegno In impegniSpesa.GetQuery.Select(Function(c) New With {c.IdDocumento, c.AnnoEsercizio, c.Capitolo, c.Articolo, c.NumeroImpegno, c.NumeroSubImpegno})
                   On impegno.IdDocumento Equals documento.Id
                   Select New With {documento, .DescrizioneUfficio = ufficio.Descrizione, .DescrizioneSettore = settore.Descrizione, impegno, .DescrizioneTipologia = tipoRegistro.Descrizione}

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

            If filtro.AnnoImpegno.HasValue Then
                view = view.Where(Function(c) c.impegno.AnnoEsercizio = filtro.AnnoImpegno)
            End If

            If filtro.CapitoloImpegno.HasValue Then
                view = view.Where(Function(c) c.impegno.Capitolo = filtro.CapitoloImpegno)
            End If

            If filtro.ArticoloImpegno.HasValue Then
                view = view.Where(Function(c) c.impegno.Articolo = filtro.ArticoloImpegno)
            End If

            If Not String.IsNullOrEmpty(filtro.NumImpegno) Then

                view = view.Where(Function(c) c.impegno.NumeroImpegno = filtro.NumImpegno)
            End If

            If Not String.IsNullOrEmpty(filtro.SubImpegno) Then
                view = view.Where(Function(c) c.impegno.NumeroSubImpegno = filtro.SubImpegno)
            End If

        End If


        'Dim distinctView = view.Select(Function(c) New With {c.documento.Id, c.documento.IdModello, c.documento.ContatoreGenerale, c.documento.Data, c.documento.Oggetto, c.DescrizioneTipologia, c.DescrizioneUfficio, c.DescrizioneSettore}).Distinct
        Dim distinctView = view.GroupBy(Function(c) New With {Key .Id = c.documento.Id, Key .IdModello = c.documento.IdModello, Key .ContatoreGenerale = c.documento.ContatoreGenerale, Key .Data = c.documento.Data, Key .DescrizioneSettore = c.DescrizioneSettore, Key .DescrizioneTipologia = c.DescrizioneTipologia, Key .DescrizioneUfficio = c.DescrizioneUfficio, Key .Oggetto = c.documento.Oggetto}).Select(Function(c) New With {c.Key.Id, c.Key.IdModello, c.Key.ContatoreGenerale, c.Key.Data, c.Key.Oggetto, c.Key.DescrizioneTipologia, c.Key.DescrizioneUfficio, c.Key.DescrizioneSettore})

        'Dim querySql = CType(distinctView, Data.Objects.ObjectQuery).ToTraceString

        Dim res = distinctView.AsEnumerable.Select(Function(c) New DettaglioDetermina With
                                          {
                                           .Id = c.Id,
                                           .IdModello = c.IdModello,
                                           .ContatoreGenerale = c.ContatoreGenerale,
                                           .DescrizioneTipologia = c.DescrizioneTipologia,
                                           .DataDocumento = c.Data,
                                           .Oggetto = c.Oggetto,
                                           .DescrizioneUfficio = c.DescrizioneUfficio,
                                           .DescrizioneSettore = c.DescrizioneSettore,
                                           .TipologiaDatoContabile = TipologiaDatoContabile.Impegno
                                        }).ToList

        Return res
    End Function

    Private Function GetDetermineByLiquidazioni(ByVal filtro As ParsecAtt.FiltroDocumento, ByVal abilitatoVisibilita As Boolean, ByVal idUtente As Integer) As List(Of DettaglioDetermina)

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim strutture As New ParsecAtt.StrutturaViewRepository(documenti.Context)
        Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository(documenti.Context)
        Dim liquidazioni As New ParsecAtt.LiquidazioneRepository(documenti.Context)

        'DETERMINE CORRENTI
        Dim queryBase = documenti.Where(Function(c) c.LogStato Is Nothing And c.IdTipologiaDocumento = 2).Select(Function(c) New With {c.Id, c.Data, c.IdUfficio, c.IdStruttura, c.IdTipologiaRegistro, c.IdModello, c.ContatoreGenerale, c.Oggetto})

        If Not filtro Is Nothing Then
            If Not abilitatoVisibilita Then
                If filtro.ApplicaAbilitazione.HasValue Then
                    If filtro.ApplicaAbilitazione.Value Then

                        Dim visibilita As New ParsecAtt.VisibilitaDocumentoViewRepository(documenti.Context)
                        Dim gruppi As New ParsecAtt.GruppoViewRepository(documenti.Context)
                        Dim utentiGruppo As New ParsecAtt.GruppoUtenteViewRepository(documenti.Context)

                        Dim queryBaseVisibilita = visibilita.Where(Function(c) c.IdModulo = ParsecAdmin.TipoModulo.ATT).Select(Function(c) New With {c.Id, c.IdDocumento, c.IdEntita, c.TipoEntita})
                        Dim queryBaseGruppo = gruppi.GetQuery.Select(Function(c) New With {c.Id})
                        Dim queryBaseUtenteGruppo = utentiGruppo.Where(Function(c) c.IdUtente = idUtente)

                        queryBase = (From documento In queryBase
                                     Join entita In queryBaseVisibilita
                                     On documento.Id Equals entita.IdDocumento
                                     Join gruppo In queryBaseGruppo
                                     On entita.IdEntita Equals gruppo.Id
                                     Join ug In queryBaseUtenteGruppo
                                     On ug.IdGruppo Equals gruppo.Id
                                     Where entita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
                                     Select documento).Union(
                                     From documento In queryBase
                                     Join entita In queryBaseVisibilita
                                     On documento.Id Equals entita.IdDocumento
                                     Where entita.IdEntita = idUtente And entita.TipoEntita = ParsecAdmin.TipoEntita.Utente
                                     Select documento)

                        'Dim s = CType(queryBase, Data.Objects.ObjectQuery).ToTraceString
                    End If
                End If
            End If

        End If

        Dim view = From documento In queryBase
                   Group Join ufficio In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On ufficio.Id Equals documento.IdUfficio
                   Into elencoUffici = Group
                   From ufficio In elencoUffici.DefaultIfEmpty
                   Group Join settore In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On settore.Id Equals documento.IdStruttura
                   Into elencoSettori = Group
                   From settore In elencoSettori.DefaultIfEmpty
                   Join tipoRegistro In tipologieRegistro.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On documento.IdTipologiaRegistro Equals tipoRegistro.Id
                   Join liquidazione In liquidazioni.GetQuery.Select(Function(c) New With {c.IdDocumento, c.AnnoEsercizio, c.Numero, c.Mandato, c.Nominativo, c.CFPIVA})
                   On liquidazione.IdDocumento Equals documento.Id
                   Select New With {documento, .DescrizioneUfficio = ufficio.Descrizione, .DescrizioneSettore = settore.Descrizione, liquidazione, .DescrizioneTipologia = tipoRegistro.Descrizione}

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

            If filtro.AnnoLiquidazione.HasValue Then
                view = view.Where(Function(c) c.liquidazione.AnnoEsercizio = filtro.AnnoLiquidazione)
            End If

            If filtro.NumLiquidazione.HasValue Then
                view = view.Where(Function(c) c.liquidazione.Numero = filtro.NumLiquidazione)
            End If

            If Not String.IsNullOrEmpty(filtro.Mandato) Then
                view = view.Where(Function(c) c.liquidazione.Mandato.ToLower.Trim.Contains(filtro.Mandato.ToLower.Trim))
            End If

            If Not String.IsNullOrEmpty(filtro.Beneficiario) Then
                view = view.Where(Function(c) c.liquidazione.Nominativo.ToLower.Trim.Contains(filtro.Beneficiario.ToLower.Trim))
            End If

            If Not String.IsNullOrEmpty(filtro.DatoFiscale) Then
                view = view.Where(Function(c) c.liquidazione.CFPIVA.ToLower.Trim.Contains(filtro.DatoFiscale.ToLower.Trim))
            End If

        End If

        'Dim distinctView = view.Select(Function(c) New With {c.documento.Id, c.documento.IdModello, c.documento.ContatoreGenerale, c.documento.Data, c.documento.Oggetto, c.DescrizioneTipologia, c.DescrizioneUfficio, c.DescrizioneSettore}).Distinct
        Dim distinctView = view.GroupBy(Function(c) New With {Key .Id = c.documento.Id, Key .IdModello = c.documento.IdModello, Key .ContatoreGenerale = c.documento.ContatoreGenerale, Key .Data = c.documento.Data, Key .DescrizioneSettore = c.DescrizioneSettore, Key .DescrizioneTipologia = c.DescrizioneTipologia, Key .DescrizioneUfficio = c.DescrizioneUfficio, Key .Oggetto = c.documento.Oggetto}).Select(Function(c) New With {c.Key.Id, c.Key.IdModello, c.Key.ContatoreGenerale, c.Key.Data, c.Key.Oggetto, c.Key.DescrizioneTipologia, c.Key.DescrizioneUfficio, c.Key.DescrizioneSettore})

        'Dim querySql = CType(distinctView, Data.Objects.ObjectQuery).ToTraceString

        Dim res = distinctView.AsEnumerable.Select(Function(c) New DettaglioDetermina With
                                          {
                                           .Id = c.Id,
                                           .IdModello = c.IdModello,
                                           .ContatoreGenerale = c.ContatoreGenerale,
                                           .DescrizioneTipologia = c.DescrizioneTipologia,
                                           .DataDocumento = c.Data,
                                           .Oggetto = c.Oggetto,
                                           .DescrizioneUfficio = c.DescrizioneUfficio,
                                           .DescrizioneSettore = c.DescrizioneSettore,
                                           .TipologiaDatoContabile = TipologiaDatoContabile.Liquidazione
                                        }).ToList

        Return res
    End Function

    Private Function GetDetermineByAccertamenti(ByVal filtro As ParsecAtt.FiltroDocumento, ByVal abilitatoVisibilita As Boolean, ByVal idUtente As Integer) As List(Of DettaglioDetermina)

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim strutture As New ParsecAtt.StrutturaViewRepository(documenti.Context)
        Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository(documenti.Context)
        Dim accertamenti As New ParsecAtt.AccertamentoRepository(documenti.Context)

        'DETERMINE CORRENTI
        Dim queryBase = documenti.Where(Function(c) c.LogStato Is Nothing And c.IdTipologiaDocumento = 2).Select(Function(c) New With {c.Id, c.Data, c.IdUfficio, c.IdStruttura, c.IdTipologiaRegistro, c.IdModello, c.ContatoreGenerale, c.Oggetto})


        If Not filtro Is Nothing Then
            If Not abilitatoVisibilita Then
                If filtro.ApplicaAbilitazione.HasValue Then
                    If filtro.ApplicaAbilitazione.Value Then

                        Dim visibilita As New ParsecAtt.VisibilitaDocumentoViewRepository(documenti.Context)
                        Dim gruppi As New ParsecAtt.GruppoViewRepository(documenti.Context)
                        Dim utentiGruppo As New ParsecAtt.GruppoUtenteViewRepository(documenti.Context)

                        Dim queryBaseVisibilita = visibilita.Where(Function(c) c.IdModulo = ParsecAdmin.TipoModulo.ATT).Select(Function(c) New With {c.Id, c.IdDocumento, c.IdEntita, c.TipoEntita})
                        Dim queryBaseGruppo = gruppi.GetQuery.Select(Function(c) New With {c.Id})
                        Dim queryBaseUtenteGruppo = utentiGruppo.Where(Function(c) c.IdUtente = idUtente)

                        queryBase = (From documento In queryBase
                                     Join entita In queryBaseVisibilita
                                     On documento.Id Equals entita.IdDocumento
                                     Join gruppo In queryBaseGruppo
                                     On entita.IdEntita Equals gruppo.Id
                                     Join ug In queryBaseUtenteGruppo
                                     On ug.IdGruppo Equals gruppo.Id
                                     Where entita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
                                     Select documento).Union(
                                     From documento In queryBase
                                     Join entita In queryBaseVisibilita
                                     On documento.Id Equals entita.IdDocumento
                                     Where entita.IdEntita = idUtente And entita.TipoEntita = ParsecAdmin.TipoEntita.Utente
                                     Select documento)

                        'Dim s = CType(queryBase, Data.Objects.ObjectQuery).ToTraceString
                    End If
                End If
            End If

        End If

        Dim view = From documento In queryBase
                   Group Join ufficio In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On ufficio.Id Equals documento.IdUfficio
                   Into elencoUffici = Group
                   From ufficio In elencoUffici.DefaultIfEmpty
                   Group Join settore In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On settore.Id Equals documento.IdStruttura
                   Into elencoSettori = Group
                   From settore In elencoSettori.DefaultIfEmpty
                   Join tipoRegistro In tipologieRegistro.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On documento.IdTipologiaRegistro Equals tipoRegistro.Id
                   Join accertamento In accertamenti.GetQuery.Select(Function(c) New With {c.IdDocumento, c.AnnoEsercizio, c.Capitolo, c.Articolo, c.NumeroAccertamento})
                   On accertamento.IdDocumento Equals documento.Id
                   Select New With {documento, .DescrizioneUfficio = ufficio.Descrizione, .DescrizioneSettore = settore.Descrizione, accertamento, .DescrizioneTipologia = tipoRegistro.Descrizione}

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

            If filtro.AnnoAccertamento.HasValue Then
                view = view.Where(Function(c) c.accertamento.AnnoEsercizio = filtro.AnnoAccertamento)
            End If

            If filtro.CapitoloAcc.HasValue Then
                view = view.Where(Function(c) c.accertamento.Capitolo = filtro.CapitoloAcc)
            End If

            If filtro.ArticoloAcc.HasValue Then
                view = view.Where(Function(c) c.accertamento.Articolo = filtro.ArticoloAcc)
            End If

            If filtro.NumAccertamento.HasValue Then
                view = view.Where(Function(c) c.accertamento.NumeroAccertamento = filtro.NumAccertamento)
            End If

        End If


        'Dim distinctView = view.Select(Function(c) New With {c.documento.Id, c.documento.IdModello, c.documento.ContatoreGenerale, c.documento.Data, c.documento.Oggetto, c.DescrizioneTipologia, c.DescrizioneUfficio, c.DescrizioneSettore}).Distinct
        Dim distinctView = view.GroupBy(Function(c) New With {Key .Id = c.documento.Id, Key .IdModello = c.documento.IdModello, Key .ContatoreGenerale = c.documento.ContatoreGenerale, Key .Data = c.documento.Data, Key .DescrizioneSettore = c.DescrizioneSettore, Key .DescrizioneTipologia = c.DescrizioneTipologia, Key .DescrizioneUfficio = c.DescrizioneUfficio, Key .Oggetto = c.documento.Oggetto}).Select(Function(c) New With {c.Key.Id, c.Key.IdModello, c.Key.ContatoreGenerale, c.Key.Data, c.Key.Oggetto, c.Key.DescrizioneTipologia, c.Key.DescrizioneUfficio, c.Key.DescrizioneSettore})

        'Dim querySql = CType(distinctView, Data.Objects.ObjectQuery).ToTraceString

        Dim res = distinctView.AsEnumerable.Select(Function(c) New DettaglioDetermina With
                                         {
                                          .Id = c.Id,
                                          .IdModello = c.IdModello,
                                          .ContatoreGenerale = c.ContatoreGenerale,
                                          .DescrizioneTipologia = c.DescrizioneTipologia,
                                          .DataDocumento = c.Data,
                                          .Oggetto = c.Oggetto,
                                          .DescrizioneUfficio = c.DescrizioneUfficio,
                                          .DescrizioneSettore = c.DescrizioneSettore,
                                          .TipologiaDatoContabile = TipologiaDatoContabile.Accertamento
                                       }).ToList

        Return res
    End Function

    Private Function GetDetermineByLiquidazioni2(ByVal filtro As ParsecAtt.FiltroDocumento, ByVal abilitatoVisibilita As Boolean, ByVal idUtente As Integer) As List(Of DettaglioDetermina)

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim strutture As New ParsecAtt.StrutturaViewRepository(documenti.Context)
        Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository(documenti.Context)
        Dim liquidazioni As New ParsecAtt.LiquidazioneRepository(documenti.Context)

        'DETERMINE CORRENTI
        Dim queryBase = documenti.Where(Function(c) c.LogStato Is Nothing And c.IdTipologiaDocumento = 2).Select(Function(c) New With {c.Id, c.Data, c.IdUfficio, c.IdStruttura, c.IdTipologiaRegistro, c.IdModello, c.ContatoreGenerale, c.Oggetto})

        If Not filtro Is Nothing Then
            If Not abilitatoVisibilita Then
                If filtro.ApplicaAbilitazione.HasValue Then
                    If filtro.ApplicaAbilitazione.Value Then

                        Dim visibilita As New ParsecAtt.VisibilitaDocumentoViewRepository(documenti.Context)
                        Dim gruppi As New ParsecAtt.GruppoViewRepository(documenti.Context)
                        Dim utentiGruppo As New ParsecAtt.GruppoUtenteViewRepository(documenti.Context)

                        Dim queryBaseVisibilita = visibilita.Where(Function(c) c.IdModulo = ParsecAdmin.TipoModulo.ATT).Select(Function(c) New With {c.Id, c.IdDocumento, c.IdEntita, c.TipoEntita})
                        Dim queryBaseGruppo = gruppi.GetQuery.Select(Function(c) New With {c.Id})
                        Dim queryBaseUtenteGruppo = utentiGruppo.Where(Function(c) c.IdUtente = idUtente)

                        queryBase = (From documento In queryBase
                                     Join entita In queryBaseVisibilita
                                     On documento.Id Equals entita.IdDocumento
                                     Join gruppo In queryBaseGruppo
                                     On entita.IdEntita Equals gruppo.Id
                                     Join ug In queryBaseUtenteGruppo
                                     On ug.IdGruppo Equals gruppo.Id
                                     Where entita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
                                     Select documento).Union(
                                     From documento In queryBase
                                     Join entita In queryBaseVisibilita
                                     On documento.Id Equals entita.IdDocumento
                                     Where entita.IdEntita = idUtente And entita.TipoEntita = ParsecAdmin.TipoEntita.Utente
                                     Select documento)

                        'Dim s = CType(queryBase, Data.Objects.ObjectQuery).ToTraceString
                    End If
                End If
            End If

        End If


        Dim liquidazioniView = liquidazioni.GetQuery.Select(Function(c) New With {c.IdDocumento, c.AnnoEsercizio, c.Numero, c.Mandato, c.Nominativo, c.CFPIVA})

        Dim distinctLiquidazioniView = (From liquidazione In liquidazioniView
                          Join documento In documenti.GetQuery.Select(Function(c) New With {c.Id})
                          On liquidazione.IdDocumento Equals documento.Id
                          Select liquidazione.IdDocumento).Distinct

        Dim view = From documento In queryBase
                   Group Join ufficio In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On ufficio.Id Equals documento.IdUfficio
                   Into elencoUffici = Group
                   From ufficio In elencoUffici.DefaultIfEmpty
                   Group Join settore In strutture.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On settore.Id Equals documento.IdStruttura
                   Into elencoSettori = Group
                   From settore In elencoSettori.DefaultIfEmpty
                   Join tipoRegistro In tipologieRegistro.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                   On documento.IdTipologiaRegistro Equals tipoRegistro.Id
                   Join liquidazione In distinctLiquidazioniView
                   On liquidazione Equals documento.Id
                   Select New With {.Id = documento.Id, .ContatoreGenerale = documento.ContatoreGenerale, .IdModello = documento.IdModello, .Data = documento.Data, .Oggetto = documento.Oggetto, .DescrizioneUfficio = ufficio.Descrizione, .DescrizioneSettore = settore.Descrizione, liquidazione, .DescrizioneTipologia = tipoRegistro.Descrizione}


        If Not filtro Is Nothing Then
            If filtro.AnnoLiquidazione.HasValue Then
                liquidazioniView = liquidazioniView.Where(Function(c) c.AnnoEsercizio = filtro.AnnoLiquidazione)
            End If

            If filtro.NumLiquidazione.HasValue Then
                liquidazioniView = liquidazioniView.Where(Function(c) c.Numero = filtro.NumLiquidazione)
            End If

            If Not String.IsNullOrEmpty(filtro.Mandato) Then
                liquidazioniView = liquidazioniView.Where(Function(c) c.Mandato.ToLower.Trim.Contains(filtro.Mandato.ToLower.Trim))
            End If

            If Not String.IsNullOrEmpty(filtro.Beneficiario) Then
                liquidazioniView = liquidazioniView.Where(Function(c) c.Nominativo.ToLower.Trim.Contains(filtro.Beneficiario.ToLower.Trim))
            End If

            If Not String.IsNullOrEmpty(filtro.DatoFiscale) Then
                liquidazioniView = liquidazioniView.Where(Function(c) c.CFPIVA.ToLower.Trim.Contains(filtro.DatoFiscale.ToLower.Trim))
            End If

            If filtro.DataDocumentoInizio.HasValue Then
                Dim d As Date = filtro.DataDocumentoInizio.Value
                Dim startDate As Date = New Date(d.Year, d.Month, d.Day, 0, 0, 0)
                queryBase = queryBase.Where(Function(c) c.Data >= startDate)
            End If

            If filtro.DataDocumentoFine.HasValue Then
                Dim d As Date = filtro.DataDocumentoFine.Value
                Dim endDate As Date = New Date(d.Year, d.Month, d.Day, 23, 59, 59)
                queryBase = queryBase.Where(Function(c) c.Data <= endDate)
            End If

        End If




        'Dim querySql = CType(view, Data.Objects.ObjectQuery).ToTraceString

        Dim res = view.AsEnumerable.Select(Function(c) New DettaglioDetermina With
                                          {
                                           .Id = c.Id,
                                           .IdModello = c.IdModello,
                                           .ContatoreGenerale = c.ContatoreGenerale,
                                           .DescrizioneTipologia = c.DescrizioneTipologia,
                                           .DataDocumento = c.Data,
                                           .Oggetto = c.Oggetto,
                                           .DescrizioneUfficio = c.DescrizioneUfficio,
                                           .DescrizioneSettore = c.DescrizioneSettore,
                                           .TipologiaDatoContabile = TipologiaDatoContabile.Liquidazione
                                        }).ToList

        Return res
    End Function

    Private Sub Anteprima(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
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

    Private Sub CaricaAnni()
        Dim anni As New ParsecAtt.AnnoRepository
        Dim lista = anni.GetQuery.OrderBy(Function(c) c.Valore).Select(Function(c) New With {.Valore = c.Valore}).ToList

        Me.AnnoDeterminaComboBox.DataValueField = "Valore"
        Me.AnnoDeterminaComboBox.DataTextField = "Valore"
        Me.AnnoDeterminaComboBox.DataSource = lista
        Me.AnnoDeterminaComboBox.DataBind()
        Me.AnnoDeterminaComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.AnnoDeterminaComboBox.SelectedIndex = 0

        Me.AnnoImpegnoSpesaComboBox.DataValueField = "Valore"
        Me.AnnoImpegnoSpesaComboBox.DataTextField = "Valore"
        Me.AnnoImpegnoSpesaComboBox.DataSource = lista
        Me.AnnoImpegnoSpesaComboBox.DataBind()
        Me.AnnoImpegnoSpesaComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.AnnoImpegnoSpesaComboBox.SelectedIndex = 0

        Me.AnnoLiquidazioneComboBox.DataValueField = "Valore"
        Me.AnnoLiquidazioneComboBox.DataTextField = "Valore"
        Me.AnnoLiquidazioneComboBox.DataSource = lista
        Me.AnnoLiquidazioneComboBox.DataBind()
        Me.AnnoLiquidazioneComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.AnnoLiquidazioneComboBox.SelectedIndex = 0

        Me.AnnoAccertamentoComboBox.DataValueField = "Valore"
        Me.AnnoAccertamentoComboBox.DataTextField = "Valore"
        Me.AnnoAccertamentoComboBox.DataSource = lista
        Me.AnnoAccertamentoComboBox.DataBind()
        Me.AnnoAccertamentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.AnnoAccertamentoComboBox.SelectedIndex = 0

        anni.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.Documenti = Nothing
        Me.DocumentiGridView.Rebind()
    End Sub

    Private Sub ResettaFiltro()

        Me.Filters = New List(Of FilterInfo)

        For Each col As Telerik.Web.UI.GridColumn In Me.DocumentiGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.DocumentiGridView.MasterTableView.FilterExpression = String.Empty

        '******************************************************************************************
        'DETERMINA
        '******************************************************************************************
        Me.AnnoDeterminaComboBox.SelectedIndex = 0
        '******************************************************************************************


        '******************************************************************************************
        'IMPEGNO SPESA
        '******************************************************************************************
        Me.ImpegnoSpesaCheckBox.Checked = True
        Me.AnnoImpegnoSpesaComboBox.SelectedIndex = 0
        Me.ArticoloImpegnoSpesaTextBox.Text = String.Empty
        Me.NumeroImpegnoSpesaTextBox.Text = String.Empty
        Me.NumeroSubImpegnoSpesaTextBox.Text = String.Empty
        Me.CapitoloImpegnoSpesaTextBox.Text = String.Empty
        '******************************************************************************************

        '******************************************************************************************
        'LIQUIDAZIONE
        '******************************************************************************************
        Me.LiquidazioneCheckBox.Checked = True
        Me.AnnoLiquidazioneComboBox.SelectedIndex = 0
        Me.NumeroLiquidazioneTextBox.Text = String.Empty
        Me.MandatoLiquidazioneTextBox.Text = String.Empty
        Me.BeneficiarioLiquidazioneTextBox.Text = String.Empty
        Me.DatoFiscaleLiquidazioneTextBox.Text = String.Empty
        '******************************************************************************************

        '******************************************************************************************
        'ACCERTAMENTO
        '******************************************************************************************
        Me.AccertamentoCheckBox.Checked = True
        Me.AnnoAccertamentoComboBox.SelectedIndex = 0
        Me.CapitoloAccertamentoTextBox.Text = String.Empty
        Me.ArticoloAccertamentoTextBox.Text = String.Empty
        Me.NumeroAccertamentoTextBox.Text = String.Empty
        '******************************************************************************************
    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroDocumento
        Dim filtro As New ParsecAtt.FiltroDocumento

        filtro.ApplicaAbilitazione = True

        '******************************************************************************************
        'DETERMINA
        '******************************************************************************************
        If Me.AnnoDeterminaComboBox.SelectedValue <> "-1" Then
            filtro.DataDocumentoInizio = "01/01/" & Me.AnnoDeterminaComboBox.SelectedValue
            filtro.DataDocumentoFine = "31/12/" & Me.AnnoDeterminaComboBox.SelectedValue
        End If
        '******************************************************************************************

        '******************************************************************************************
        'IMPEGNO SPESA
        '******************************************************************************************
        If Me.ImpegnoSpesaCheckBox.Checked Then

            If Me.AnnoImpegnoSpesaComboBox.SelectedValue <> "-1" Then
                filtro.AnnoImpegno = Me.AnnoImpegnoSpesaComboBox.SelectedValue
            End If

            If Me.CapitoloImpegnoSpesaTextBox.Value.HasValue Then
                filtro.CapitoloImpegno = Me.CapitoloImpegnoSpesaTextBox.Value
            End If

            If Me.ArticoloImpegnoSpesaTextBox.Value.HasValue > 0 Then
                filtro.ArticoloImpegno = Me.ArticoloImpegnoSpesaTextBox.Value
            End If

            If Not String.IsNullOrEmpty(Me.NumeroImpegnoSpesaTextBox.Text.Trim) Then
                filtro.NumImpegno = Me.NumeroImpegnoSpesaTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.NumeroSubImpegnoSpesaTextBox.Text.Trim) Then
                filtro.SubImpegno = Me.NumeroSubImpegnoSpesaTextBox.Text
            End If

        End If
        '******************************************************************************************


        '******************************************************************************************
        'LIQUIDAZIONE
        '******************************************************************************************
        If Me.LiquidazioneCheckBox.Checked Then

            If Me.AnnoLiquidazioneComboBox.SelectedValue <> "-1" Then
                filtro.AnnoLiquidazione = Me.AnnoLiquidazioneComboBox.SelectedValue
            End If

            If Me.NumeroLiquidazioneTextBox.Value.HasValue Then
                filtro.NumLiquidazione = Me.NumeroLiquidazioneTextBox.Value
            End If

            If Not String.IsNullOrEmpty(Me.MandatoLiquidazioneTextBox.Text.Trim) Then
                filtro.Mandato = Me.MandatoLiquidazioneTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.BeneficiarioLiquidazioneTextBox.Text.Trim) Then
                filtro.Beneficiario = Me.BeneficiarioLiquidazioneTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.DatoFiscaleLiquidazioneTextBox.Text.Trim) Then
                filtro.DatoFiscale = Me.DatoFiscaleLiquidazioneTextBox.Text
            End If

        End If
        '******************************************************************************************

        '******************************************************************************************
        'ACCERTAMENTO
        '******************************************************************************************
        If Me.AccertamentoCheckBox.Checked Then

            If Me.AnnoAccertamentoComboBox.SelectedValue <> "-1" Then
                filtro.AnnoAccertamento = Me.AnnoAccertamentoComboBox.SelectedValue
            End If

            If Me.CapitoloAccertamentoTextBox.Value.HasValue Then
                filtro.CapitoloAcc = Me.CapitoloAccertamentoTextBox.Value
            End If

            If Me.ArticoloAccertamentoTextBox.Value.HasValue Then
                filtro.ArticoloAcc = Me.ArticoloAccertamentoTextBox.Value
            End If

            If Me.NumeroAccertamentoTextBox.Value.HasValue Then
                filtro.NumAccertamento = Me.NumeroAccertamentoTextBox.Value
            End If

        End If
        '******************************************************************************************

        Return filtro

    End Function

#End Region

End Class
