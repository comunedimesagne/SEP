Imports ParsecAdmin
Imports ParsecUtility
Imports Telerik.Web.UI

Partial Class AttivaPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Private Class FiltroCorrente

        Public Property Id As Integer = 0
        Public Property Descrizione As String = String.Empty
        Public Property Tooltip As String = String.Empty
        Public Property Valore As Object = Nothing

    End Class


    Public Class FilterInfo
        Public Property Value As String
        Public Property ColumnName As String
        Public Property FunctionName As String
        Public Property SortOrder As String
    End Class

    Private Enum TipologiaFiltro
        Nessuno = -1
        Modulo = 0
        Documento = 1
        Stato = 2
        DataInizio = 3
        DataFine = 4

    End Enum

#Region "PROPRIETA'"


    Protected Function GetDescrizioneDocumento(ByVal s As String) As String
        ' <%# GetDescrizioneDocumento(DataBinder.Eval(Container.DataItem, "DescrizioneDocumento"))%>
        Return s
    End Function

    Protected Function GetDescrizioneDocumento(ByVal item As Object) As String
        '  <%# GetDescrizioneDocumento(Container.DataItem)%>

        'Dim task As ParsecWKF.TaskAttivo = CType(item, ParsecWKF.TaskAttivo)
        'Return task.DescrizioneDocumento

        Dim s = DataBinder.Eval(item, "DescrizioneDocumento").ToString()

        If Me.CaricaCollegamenti Then
            Dim task As ParsecWKF.TaskAttivo = CType(item, ParsecWKF.TaskAttivo)
            If task.IdModulo = ParsecAdmin.TipoModulo.PRO Then

                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim registrazione = registrazioni.Where(Function(c) c.Id = task.IdDocumento).FirstOrDefault
                If Not registrazione Is Nothing Then
                    Dim anno = registrazione.DataImmissione.Value.Year
                    Dim numero = registrazione.NumeroProtocollo
                    Dim tipologia = registrazione.TipoRegistrazione

                    Dim collegamenti As New ParsecWKF.CollegamentiViewRepository
                    Dim collegamentiDiretti = collegamenti.Where(Function(c) c.AnnoProtocollo1 = anno And c.NumeroProtocollo1 = numero And c.TipoRegistrazione1 = tipologia).ToList.Select(Function(c) c.NumeroProtocollo2.ToString & "/" & c.AnnoProtocollo2.ToString).ToList
                    Dim lista = String.Join(" - ", collegamentiDiretti)
                    If Not String.IsNullOrEmpty(lista) Then
                        s &= " - Rif. Prot. ( " & lista & " )"
                    End If
                    collegamenti.Dispose()
                End If

                registrazioni.Dispose()
            End If

        End If


        Return s
    End Function


    Public Property CaricaCollegamenti() As Boolean
        Get
            Return CType(Session("AttivaPage_CaricaCollegamenti"), Boolean)

        End Get
        Set(ByVal value As Boolean)
            Session("AttivaPage_CaricaCollegamenti") = value
        End Set
    End Property


    Public Property TaskCorrente() As ParsecWKF.TaskAttivo
        Get
            Return CType(Session("AttivaPage_Task"), ParsecWKF.TaskAttivo)
        End Get
        Set(ByVal value As ParsecWKF.TaskAttivo)
            Session("AttivaPage_Task") = value
        End Set
    End Property

    Public Property TaskAttivi() As List(Of ParsecWKF.TaskAttivo)
        Get
            Return CType(Session("AttivaPage_Tasks"), List(Of ParsecWKF.TaskAttivo))
        End Get
        Set(ByVal value As List(Of ParsecWKF.TaskAttivo))
            Session("AttivaPage_Tasks") = value
        End Set
    End Property

    Public Property Filtro() As ParsecWKF.TaskFiltro
        Get
            Return CType(Session("AttivaPage_Filtro"), ParsecWKF.TaskFiltro)
        End Get
        Set(ByVal value As ParsecWKF.TaskFiltro)
            Session("AttivaPage_Filtro") = value
        End Set
    End Property

    Private Property Filtri As List(Of FiltroCorrente)
        Get
            Return CType(Session("AttivaPage_FiltroCorrente"), List(Of FiltroCorrente))
        End Get
        Set(value As List(Of FiltroCorrente))
            Session("AttivaPage_FiltroCorrente") = value
        End Set
    End Property


    Public Property Filters() As List(Of FilterInfo)
        Get
            Return CType(Session("AttivaPage_Filters"), List(Of FilterInfo))
        End Get
        Set(value As List(Of FilterInfo))
            Session("AttivaPage_Filters") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        Me.ToolBarFiltri.DataSource = Me.Filtri
        Me.ToolBarFiltri.DataBind()
        FiltriLabel.Visible = Me.Filtri.Count > 0
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Scrivania"
        MainPage.DescrizioneProcedura = "> Scrivania ATTIVA"

        If Not Me.IsPostBack Then

            Me.Filters = New List(Of FilterInfo)

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName("VisualizzaProtocolliCollegatiGrigliaScrivania", ParsecAdmin.TipoModulo.PRO)
            parametri.Dispose()

            Me.CaricaCollegamenti = False

            If Not parametro Is Nothing Then
                Me.CaricaCollegamenti = (parametro.Valore = 1)
            End If


            Me.VisualizzaEmailControl.Visible = False
            Me.VisualizzaFatturaControl.Visible = False

            Me.Filtri = New List(Of FiltroCorrente)
            Me.CaricaStatiWorkflow()
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Me.CaricaDeleghe(utenteCorrente)
            Me.CaricaModelliWorkflow(CType(Nothing, Nullable(Of Integer)))
            Me.CaricaModuli()
            Me.TaskAttivi = Nothing
            Me.ImpostaFiltroPredefinito()
            Me.GetCookie()
            Me.TaskCorrente = Nothing

        End If

        '********************************************************************************************
        'Registro gli script necessari al funzionamento del componente ParsecComunicator
        '********************************************************************************************
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
            Me.RegistraParsecDigitalSign()
        End If

        '********************************************************************************************

        Me.FiltraImageButton.Attributes.Add("onclick", "ShowSearchPanel();hideSearch=false; return false;")
        Me.ApplicaFiltroButton.Attributes.Add("onclick", "HideSearchPanel();hideSearch = true;")


        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"


        End If

        Me.TaskGridView.Style.Add("width", widthStyle)




    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'If Not Me.TaskAttivi Is Nothing Then
        '    Me.ElencoTaskLabel.Text = "Attività&nbsp;&nbsp;" & If(Me.TaskAttivi.Count > 0, "( " & Me.TaskAttivi.Count.ToString & " )", "")
        'End If
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region

#Region "EVENTI TOOLBAR FILTRI"

    Protected Sub ToolBarFiltri_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles ToolBarFiltri.ItemCommand
        Dim id As Integer = CInt(e.CommandArgument)
        Dim filtro As FiltroCorrente = Filtri.Where(Function(c) c.Id = id).FirstOrDefault
        Me.Filtri.Remove(filtro)
        Select Case CType(id, TipologiaFiltro)
            Case TipologiaFiltro.Modulo
                Me.ModuliComboBox.SelectedIndex = 0
            Case TipologiaFiltro.Documento
                Me.TipologiaDocumentoComboBox.SelectedIndex = 0
            Case TipologiaFiltro.Stato
                Me.StatoComboBox.SelectedIndex = 0
            Case TipologiaFiltro.DataInizio
                Me.DataInizioIstanzaFiltroTextBox.SelectedDate = Nothing
            Case (TipologiaFiltro.DataFine)
                Me.DataFineIstanzaFiltroTextBox.SelectedDate = Nothing
        End Select
        Me.Search()
    End Sub

#End Region

#Region "EVENTI USER CONTROL EMAIL - FATTURA"

    Protected Sub VisualizzaEmailControl_OnCloseEvent() Handles VisualizzaEmailControl.OnCloseEvent
        Me.VisualizzaEmailControl.Visible = False
    End Sub

    Protected Sub VisualizzaFatturaControl_OnCloseEvent() Handles VisualizzaFatturaControl.OnCloseEvent
        Me.VisualizzaFatturaControl.Visible = False
    End Sub

#End Region

#Region "EVENTI GRIGLIA"



    '********************************************************************************************************************************
    'RESTITUISCE UNA LISTA DI TASK ATTIVI FILTRATA IN BASE AI FILTRI IMPOSTATI NELLA GRIGLIA ED ORDINATA IN BASE ALLA COLONNA
    '********************************************************************************************************************************
    Private Function FiltraTaskAttivi() As List(Of ParsecWKF.TaskAttivo)

        Dim res = Me.TaskAttivi.ToList


        If Not Me.Filters Is Nothing Then
            Dim key As String = String.Empty
            For Each f In Me.Filters
                key = f.Value
                If Not String.IsNullOrEmpty(key) Then
                    key = key.ToLower
                    Select Case f.FunctionName
                        Case "NoFilter"
                            Exit Select
                        Case "Contains"
                            Select Case f.ColumnName
                                Case "DescrizioneDocumento"
                                    res = res.Where(Function(c) c.DescrizioneDocumento.ToLower.Contains(key)).ToList

                                Case "Proponente"
                                    res = res.Where(Function(c) c.Proponente.ToLower.Contains(key)).ToList

                                Case "TaskCorrente"
                                    res = res.Where(Function(c) c.TaskCorrente.ToLower.Contains(key)).ToList
                               
                            End Select
                        Case "EqualTo"
                            Exit Select

                    End Select
                End If

                Dim order As String = String.Empty
                If Not String.IsNullOrEmpty(f.SortOrder) Then
                    order = f.SortOrder.ToLower
                End If


                Select Case f.ColumnName

                    Case "DescrizioneDocumento"
                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.DescrizioneDocumento).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.DescrizioneDocumento).ToList
                        End Select

                    Case "Proponente"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.Proponente).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.Proponente).ToList
                        End Select

                    Case "TaskCorrente"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.TaskCorrente).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.TaskCorrente).ToList
                        End Select

                End Select

            Next

        End If

        Return res
    End Function



    Protected Sub TaskGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TaskGridView.ItemCommand

        Select Case e.CommandName

            Case Telerik.Web.UI.RadGrid.FilterCommandName
                Dim filter = CType(e.CommandArgument, Pair)
                Dim nomeColonna As String = filter.Second.ToString
                Dim value = Me.TaskGridView.MasterTableView.Columns.FindByUniqueName(nomeColonna).CurrentFilterValue
                Dim functionName = filter.First.ToString()
                Dim info As New FilterInfo With {.Value = value, .ColumnName = nomeColonna, .FunctionName = functionName}
                Dim old = Me.Filters.Where(Function(c) c.ColumnName = nomeColonna).FirstOrDefault
                If Not old Is Nothing Then
                    Me.Filters.Remove(old)
                End If
                Me.Filters.Add(info)


            Case Telerik.Web.UI.RadGrid.SortCommandName

                Dim m = DirectCast(e, Telerik.Web.UI.GridSortCommandEventArgs).NewSortOrder.ToString
                Dim old = Me.Filters.Where(Function(c) c.ColumnName = e.CommandArgument).FirstOrDefault
                If old Is Nothing Then
                    Dim info As New FilterInfo With {.ColumnName = e.CommandArgument, .SortOrder = m}
                    For Each f In Me.Filters
                        f.SortOrder = ""
                    Next
                    Me.Filters.Add(info)
                Else
                    For Each f In Me.Filters
                        f.SortOrder = ""
                    Next
                    old.SortOrder = m
                End If

            Case Telerik.Web.UI.RadGrid.ExpandCollapseCommandName
                If Not e.Item.Expanded Then
                    Dim parentItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
                    Dim innerGrid As Telerik.Web.UI.RadGrid = CType(parentItem.ChildItem.FindControl("IterGridView"), Telerik.Web.UI.RadGrid)
                    innerGrid.Rebind()
                End If
            Case "Preview"
                Me.VisualizzaDocumento(e.Item)
            Case "Execute"
                Me.EseguiTask(e.Item)
            Case "PreviewEmail"

                Me.VisualizzaAllegatoPrimario(e.Item)

            Case "Attachment"
                Me.VisualizzaAllegati(e.Item)
        End Select
    End Sub

    Private Sub VisualizzaAllegati(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim taskAttivo As ParsecWKF.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = id).FirstOrDefault

        If CType(taskAttivo.IdModulo, ParsecAdmin.TipoModulo) = TipoModulo.PRO Then
            Dim parametriPagina As New Hashtable
            Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
            parametriPagina.Add("Filtro", taskAttivo.IdDocumento)
            parametriPagina.Add("VisualizzaAllegati", True)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 940, 510, Nothing, False)
        End If

    End Sub

    Private Sub VisualizzaAllegatoPrimario(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim taskAttivo As ParsecWKF.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = id).FirstOrDefault
        If CType(taskAttivo.IdModulo, ParsecAdmin.TipoModulo) = TipoModulo.PRO Then

            If taskAttivo.TipologiaAllegatoPrimario.HasValue Then
                Select Case CType(taskAttivo.TipologiaAllegatoPrimario, ParsecPro.TipologiaAllegatoPrimario)
                    Case ParsecPro.TipologiaAllegatoPrimario.Email
                        Me.VisualizzaEmail(taskAttivo)
                    Case ParsecPro.TipologiaAllegatoPrimario.Fattura
                        Me.VisualizzaFattura(taskAttivo)

                    Case ParsecPro.TipologiaAllegatoPrimario.Generico
                        Me.VisualizzaAllegatoGenerico(taskAttivo)
                End Select
            End If
        End If
    End Sub

    Private Sub VisualizzaAllegatoGenerico(ByVal taskAttivo As ParsecWKF.TaskAttivo)
        Dim allegati As New ParsecPro.AllegatiRepository
        Dim allegatoPrimario = allegati.GetAllegatiProtocollo(taskAttivo.IdDocumento).Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
        allegati.Dispose()

        If Not allegatoPrimario Is Nothing Then

            Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            If percorsoRoot.EndsWith("\") Then
                percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
            End If

            Dim pathDownload As String
            If String.IsNullOrEmpty(allegatoPrimario.NomeFileFirmato) Then
                pathDownload = percorsoRoot & allegatoPrimario.PercorsoRelativo & allegatoPrimario.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegatoPrimario.NomeFile
            Else
                pathDownload = percorsoRoot & allegatoPrimario.PercorsoRelativo & allegatoPrimario.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegatoPrimario.NomeFileFirmato
            End If

            Dim file As New IO.FileInfo(pathDownload)

            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        End If
    End Sub

    Public Sub VisualizzaFattura(ByVal taskAttivo As ParsecWKF.TaskAttivo)

        Dim fatture As New ParsecPro.FatturaElettronicaRepository
        Dim fattura = fatture.Where(Function(c) c.IdRegistrazione = taskAttivo.IdDocumento).FirstOrDefault

        If Not fattura Is Nothing Then
            Me.VisualizzaFatturaControl.CloseScript = "HideFatturaElettronicaPanel();hideFatturaPanel=true;HideControlPanel();return false;"
            Me.VisualizzaFatturaControl.ShowPanel()
            Me.VisualizzaFatturaControl.InitUI(fattura)
        End If
        fatture.Dispose()

    End Sub

    Private Sub VisualizzaEmail(ByVal taskAttivo As ParsecWKF.TaskAttivo)


        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim reg = registrazioni.Where(Function(c) c.Id = taskAttivo.IdDocumento).FirstOrDefault


        If reg Is Nothing Then
            ParsecUtility.Utility.MessageBox("Il protocollo associato al task corrente non esiste!", False)
            Exit Sub
        End If

        Dim numero = reg.NumeroProtocollo
        Dim anno = reg.DataImmissione.Value.Year
        Dim tipologia = reg.TipoRegistrazione
        Dim listaId = registrazioni.Where(Function(c) c.NumeroProtocollo = numero And c.DataImmissione.Value.Year = anno And c.TipoRegistrazione = tipologia).Select(Function(c) c.Id).ToList
        registrazioni.Dispose()


        Dim emails As New ParsecPro.EmailArrivoRepository
        'Dim mailMessage = emails.Where(Function(c) c.IdRegistrazione = taskAttivo.IdDocumento).FirstOrDefault
        Dim mailMessage = emails.Where(Function(c) listaId.Contains(c.IdRegistrazione)).FirstOrDefault
        emails.Dispose()


        If Not mailMessage Is Nothing Then
            Dim fullPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata") & mailMessage.PercorsoRelativo & mailMessage.NomeFileEml.Replace("Prot_", "")
            Dim file As New IO.FileInfo(fullPath)
            If file.Exists Then
                Try

                    registrazioni = New ParsecPro.RegistrazioniRepository
                    Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                    Dim registrazione = registrazioni.GetById(taskAttivo.IdDocumento)
                    Dim watermark As String = cliente.Descrizione & " - Cod. Amm. " & cliente.CodiceAmministrazione & " - Prot. n. " & registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & " del " & String.Format("{0:dd/MM/yyyy HH:mm}", registrazione.DataImmissione) & " - " & registrazioni.GetDescrizioneTipoRegistrazione(registrazione.TipoRegistrazione).ToUpper
                    registrazioni.Dispose()

                    Me.VisualizzaEmailControl.ShowPanel()
                    Me.VisualizzaEmailControl.InitUI(file.FullName, mailMessage.Id, watermark)
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox("Si è verificato un errore durante la lettura dell'email!" & vbCrLf & "Il file verrà aperto con l'applicazione associata.", False)
                    'IN CASO DI ERRORE VISUALIZZO IL FILE NON MODO CLASSICO
                    Session("AttachmentFullName") = file.FullName
                    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                    ParsecUtility.Utility.PageReload(pageUrl, False)
                End Try
            Else
                ParsecUtility.Utility.MessageBox("L'e-mail selezionata non esiste!", False)
            End If

        End If


    End Sub

    Private Sub VisualizzaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim taskAttivo As ParsecWKF.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = id).FirstOrDefault
        Dim parametriPagina As New Hashtable

        Select Case CType(taskAttivo.IdModulo, ParsecAdmin.TipoModulo)

            Case TipoModulo.PRO
                Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
                parametriPagina.Add("Filtro", taskAttivo.IdDocumento)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 560, Nothing, False)


            Case TipoModulo.ATT, TipoModulo.NAA

                Me.VisualizzaAtto(taskAttivo.IdDocumento)

            Case TipoModulo.PED
                Dim pageUrl As String = "~/UI/PraticheEdilizie/pages/search/VisualizzaPraticaPage.aspx"
                parametriPagina.Add("Filtro", taskAttivo.IdDocumento)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 610, Nothing, False)

            Case TipoModulo.IOL

                Dim pageUrl As String = "~/UI/Amministrazione/pages/search/VisualizzaIstanzaPraticaPage.aspx"
                parametriPagina.Add("Filtro", taskAttivo.IdDocumento)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina

                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 670, Nothing, False)

            Case TipoModulo.CSRA

                Dim idAtto As Integer = taskAttivo.IdDocumento
                Dim documenti As New ParsecAtt.DocumentoRepository
                Dim documento = documenti.Where(Function(c) c.Id = idAtto).FirstOrDefault
                documenti.Dispose()

                Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
                Dim scheda = schede.Where(Function(c) c.NumeroAtto = documento.ContatoreGenerale And c.DataAtto = documento.Data).FirstOrDefault
                schede.Dispose()

                Dim queryString As New Hashtable
                queryString.Add("IdSchedaControlloSuccessivoRegolaritaAmministrativa", scheda.Id.ToString)
                queryString.Add("Mode", "V")
                Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/SchedaControlloSuccessivoRegolaritaAmministrativaPage.aspx"


                ParsecUtility.Utility.ShowPopup(pageUrl, 910, 590, queryString, False)

            Case TipoModulo.WBT

                Dim idSegnalazione As Integer = taskAttivo.IdDocumento
                Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
                Dim segnalazione = segnalazioni.Where(Function(c) c.Id = idSegnalazione).FirstOrDefault
                segnalazioni.Dispose()

                If segnalazione.DataScadenza.AddHours(-1) <= Now Then
                    Me.TerminaTask(taskAttivo)
                    ParsecUtility.Utility.MessageBox("La segnalazione è scaduta!" & vbCrLf & "L'iter selezionato verrà chiuso", False)
                    Me.AggiornaGriglia()
                Else
                    Dim queryString As New Hashtable
                    queryString.Add("GuidSegnalazione", segnalazione.GuidSegnalazione)
                    queryString.Add("Mode", "V")
                    Dim pageUrl As String = "~/UI/Amministrazione/pages/search/SegnalazionePage.aspx"
                    ParsecUtility.Utility.ShowPopup(pageUrl, 910, 710, queryString, False)
                End If


            Case Else
                'todo altri moduli
        End Select
    End Sub


    Private Sub AggiornaGriglia()
        Dim script As New StringBuilder
        script.AppendLine("<script>")
        script.AppendLine("UpdateTask();")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
    End Sub


    Private Sub TerminaTask(ByVal taskAttivo As ParsecWKF.TaskAttivo)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'TERMINO IL TASK CORRENTE
        Dim statoTaskEseguito As Integer = 6
        Dim statoIstanzaCompletato As Integer = 3

        Dim tasks As New ParsecWKF.TaskRepository
        Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = taskAttivo.Id).FirstOrDefault
        task.IdStato = statoTaskEseguito
        task.DataEsecuzione = Now
        task.Destinatario = taskAttivo.IdMittente
        task.Operazione = "MODIFICA"
        task.Notificato = True
        tasks.SaveChanges()

        'Cambia lo stato del processo di workflow
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = taskAttivo.IdIstanza).FirstOrDefault
        istanza.IdStato = statoIstanzaCompletato
        istanza.DataEsecuzione = Now
        istanze.SaveChanges()

        Dim nuovoTask As New ParsecWKF.Task
        nuovoTask.IdIstanza = taskAttivo.IdIstanza
        nuovoTask.Nome = task.Corrente
        nuovoTask.Corrente = "FINE"
        nuovoTask.Successivo = String.Empty
        nuovoTask.Destinatario = taskAttivo.IdMittente
        nuovoTask.Mittente = taskAttivo.IdMittente
        nuovoTask.TaskPadre = task.Id
        nuovoTask.DataEsecuzione = Now
        nuovoTask.DataInizio = task.DataInizio
        nuovoTask.DataFine = task.DataFine
        nuovoTask.IdStato = statoTaskEseguito
        nuovoTask.Operazione = "FINE"
        nuovoTask.Notificato = True
        nuovoTask.Note = task.Note
        nuovoTask.IdUtenteOperazione = utenteCollegato.Id
        tasks.Add(nuovoTask)
        tasks.SaveChanges()

    End Sub



    Private Sub VisualizzaAtto(idDocumento As Integer)

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        If Not documento Is Nothing Then
            Dim queryString As New Hashtable
            queryString.Add("Tipo", documento.IdTipologiaDocumento)
            queryString.Add("Mode", "View")
            queryString.Add("Procedura", "10")
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AttoAmministrativoPage.aspx"
            Dim parametriPagina As New Hashtable
            parametriPagina.Add("IdDocumentoIter", idDocumento)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 650, queryString, False)

        End If
        documenti.Dispose()
        

    End Sub

    Private Sub EseguiTask(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim idAttoreCorrente As Integer = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Dim filtro As New ParsecWKF.TaskFiltro
        filtro.IdUtente = idAttoreCorrente
        Dim tasks As New ParsecWKF.TaskRepository

        Dim idModulo As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdModulo")
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

       

        If Me.TaskGridView.SelectedItems.Count > 1 Then



            'Dim idsSelezionati = (From it In Me.TaskGridView.SelectedItems.Cast(Of GridDataItem)()
            '                  Select New With {.IndiciTask = CInt(it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")), .IndiciModulo = CInt(it.OwnerTableView.DataKeyValues(it.ItemIndex)("IdModulo"))}).ToList

            'Dim idsSelezionati = (From it In Me.TaskGridView.SelectedItems.Cast(Of GridDataItem)()
            '                  Select New With {.IndiciTask = CInt(it.GetDataKeyValue("Id")), .IndiciModulo = CInt(it.GetDataKeyValue("IdModulo"))}).ToList


            'OPERAZIONE MASSIVA

            Dim taskIdList As New List(Of Integer)
            Dim listaModuli As New List(Of Integer)
            For Each selectedItem As GridDataItem In Me.TaskGridView.SelectedItems
                Dim idSelezionato As Integer = selectedItem.OwnerTableView.DataKeyValues(selectedItem.ItemIndex)("Id")
                taskIdList.Add(idSelezionato)

                Dim idModuloSelezionato As Integer = selectedItem.OwnerTableView.DataKeyValues(selectedItem.ItemIndex)("IdModulo")
                listaModuli.Add(idModuloSelezionato)

            Next

            If listaModuli.Distinct.Count > 1 Then
                'E' necessario selezionare solo task appartenenti allo stesso modulo
                ParsecUtility.Utility.MessageBox("L'operazione non è congruente con l'iter di alcuni task selezionati", False)
                Exit Sub
            End If





            filtro.Id = id
            filtro.IdModulo = idModulo

            'Dim task = tasks.GetView(filtro).Where(Function(c) c.Id = id).FirstOrDefault
            Dim task = tasks.GetView(filtro).FirstOrDefault

            Dim processDefinition As String = task.NomeFileIter
            Dim taskCorrente As String = task.TaskCorrente


            filtro.Id = Nothing

            'SE TUTTI I TASK SELEZIONATI ESISTONO.
            Dim taskSelezionati = tasks.GetView(filtro).Where(Function(c) taskIdList.Contains(c.Id) And c.NomeFileIter = processDefinition And c.TaskCorrente = taskCorrente).ToList
            If taskSelezionati.Count = taskIdList.Count Then

                'VERIFICO I TASK SELEZIONATI 

                Dim script As New StringBuilder
                script.AppendLine("<script>")
                script.AppendLine("currentPanel=0;ShowPanel(currentPanel);hide=false;panelIsVisible = false;enableUiHidden='" & Me.OperazioneControl.GetEnableUiHiddenControl & "'")
                script.AppendLine("</script>")

                ParsecUtility.Utility.RegisterScript(script, False)
                Me.OperazioneControl.InitUI(Me.DelegheScrivaniaComboBox.SelectedItem.Value, taskIdList, idModulo)
            Else
                ParsecUtility.Utility.MessageBox("L'operazione non è congruente con l'iter di alcuni task selezionati", False)
            End If
        Else



            'Dim watch = System.Diagnostics.Stopwatch.StartNew()

            filtro.Id = id
            filtro.IdModulo = idModulo
            Dim task = tasks.GetView(filtro).FirstOrDefault

            'watch.Stop()
            'Dim elapsedMs = watch.ElapsedMilliseconds

            If Not task Is Nothing Then
                If task.IdModulo = ParsecAdmin.TipoModulo.WBT Then

                    Dim idSegnalazione As Integer = task.IdDocumento
                    Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
                    Dim segnalazione = segnalazioni.Where(Function(c) c.Id = idSegnalazione).FirstOrDefault
                    segnalazioni.Dispose()

                    If segnalazione.DataScadenza.AddHours(-1) <= Now Then
                        Me.TerminaTask(task)
                        ParsecUtility.Utility.MessageBox("La segnalazione è scaduta!" & vbCrLf & "L'iter selezionato verrà chiuso", False)
                        Me.AggiornaGriglia()
                        Exit Sub
                    End If
                End If
            End If



            If Not task Is Nothing Then

                If task.IdModulo = ParsecAdmin.TipoModulo.IOL Then
                    Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository

                    Dim istanzaPratica = pratiche.GetFullById(task.IdDocumento)
                    If Not istanzaPratica Is Nothing Then
                        'IN ATTESA DI INTEGRAZIONE
                        If istanzaPratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.AttesaRichiestaIntegrazione Then
                            If istanzaPratica.DataRichiestaIntegrazione.HasValue Then
                                Dim giorni = Now.Subtract(istanzaPratica.DataRichiestaIntegrazione).Days

                                'VALORE DI DEFAULT
                                Dim giorniSospensione As Integer = 30
                                Dim ok As Boolean = False
                                If Not istanzaPratica.Procedimento Is Nothing Then
                                    If istanzaPratica.Procedimento.GiorniSospensione.HasValue Then
                                        giorniSospensione = istanzaPratica.Procedimento.GiorniSospensione
                                        ok = True
                                    End If
                                End If
                                'SE NON SONO STATI SPECIFICATI I GIORNI DI SOSPENSIONE
                                If Not ok Then
                                    Dim parametri As New ParsecAdmin.ParametriRepository
                                    Dim parametro = parametri.GetByName("GiorniAttesaIntegrazioneIstanzaOnLine")
                                    parametri.Dispose()

                                    If Not parametro Is Nothing Then
                                        Try
                                            giorniSospensione = CInt(parametro.Valore)
                                        Catch ex As Exception
                                        End Try
                                    End If
                                End If



                                If giorni <= giorniSospensione Then
                                    ParsecUtility.Utility.ConfirmDelete("L'istanza non può essere eseguita perchè è in attesa di integrazione!" & vbCrLf & "Premere OK per sbloccare l'operazione.", False, "Continua")
                                    Dim sb As New StringBuilder
                                    sb.AppendLine("<script>")
                                    sb.AppendLine("var res = ConfirmDeleteContinua();")
                                    sb.AppendLine("if (res){ document.getElementById('" & Me.continuaSenzaIntegrazioneButton.ClientID & "').click();}")

                                    sb.AppendLine("</script>")
                                    ParsecUtility.Utility.RegisterScript(sb, False)
                                    Me.TaskCorrente = task
                                    'ParsecUtility.Utility.MessageBox("L'operazione non può essere eseguita perchè l'istanza è in attesa dell'integrazione!", False)
                                    Exit Sub
                                End If
                            End If


                        End If
                    End If
                End If

                Dim script As New StringBuilder
                script.AppendLine("<script>")

                Select Case task.IdModulo

                    Case ParsecAdmin.TipoModulo.PRO, ParsecAdmin.TipoModulo.ATT, ParsecAdmin.TipoModulo.IOL, ParsecAdmin.TipoModulo.CSRA, ParsecAdmin.TipoModulo.WBT, ParsecAdmin.TipoModulo.NAA
                        script.AppendLine("currentPanel=0;ShowPanel(currentPanel);hide=false;panelIsVisible = false;enableUiHidden='" & Me.OperazioneControl.GetEnableUiHiddenControl & "'")
                        '' ''Case ParsecAdmin.TipoModulo.PED, ParsecAdmin.TipoModulo.SUAP
                        '' ''    script.AppendLine("currentPanel=1;ShowPanel(currentPanel);hide=false;panelIsVisible = false;enableUiHidden='" & Me.OperazioneControlSUAPE.GetEnableUiHiddenControl & "'")

                End Select

                script.AppendLine("</script>")

                ParsecUtility.Utility.RegisterScript(script, False)


                Select Case task.IdModulo
                    Case ParsecAdmin.TipoModulo.PRO, ParsecAdmin.TipoModulo.ATT, ParsecAdmin.TipoModulo.IOL, ParsecAdmin.TipoModulo.CSRA, ParsecAdmin.TipoModulo.WBT, ParsecAdmin.TipoModulo.NAA
                        '' ''Me.OperazioneControlSUAPE.Visible = False
                        Me.OperazioneControl.Visible = True

                        Me.OperazioneControl.InitUI(Me.DelegheScrivaniaComboBox.SelectedItem.Value, task)
                    Case ParsecAdmin.TipoModulo.PED, ParsecAdmin.TipoModulo.SUAP
                        Me.OperazioneControl.Visible = False
                        '' ''Me.OperazioneControlSUAPE.Visible = True
                        '' ''Me.OperazioneControlSUAPE.InitUI(Me.DelegheScrivaniaComboBox.SelectedItem.Value, task)
                End Select


            End If

        End If

        tasks.Dispose()

    End Sub

    Protected Sub TaskGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TaskGridView.ItemDataBound
        Dim btn As ImageButton = Nothing


        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pagerItem = CType(e.Item, GridPagerItem)
            Dim itemsCount = pagerItem.Paging.DataSourceCount
            Me.ElencoTaskLabel.Text = "Attività&nbsp;&nbsp;" & If(itemsCount > 0, "( " & itemsCount.ToString & " )", "")
        End If

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim task As ParsecWKF.TaskAttivo = CType(e.Item.DataItem, ParsecWKF.TaskAttivo)


            If TypeOf dataItem("Attachment").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Attachment").Controls(0), ImageButton)
                If task.NumeroAllegati.HasValue AndAlso task.NumeroAllegati > 0 Then
                    If task.NumeroAllegati = 1 Then
                        btn.ToolTip = "La registrazione di protocollo possiede n. " & task.NumeroAllegati.ToString & " allegato."
                    Else
                        btn.ToolTip = "La registrazione di protocollo possiede n. " & task.NumeroAllegati.ToString & " allegati."
                    End If
                Else
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.Attributes.Add("onclick", "return false")
                End If

            End If

            If TypeOf dataItem("PreviewEmail").Controls(0) Is ImageButton Then
                btn = CType(dataItem("PreviewEmail").Controls(0), ImageButton)



                If task.TipologiaAllegatoPrimario.HasValue Then
                    Select Case CType(task.TipologiaAllegatoPrimario, ParsecPro.TipologiaAllegatoPrimario)
                        Case ParsecPro.TipologiaAllegatoPrimario.Email
                            btn.ToolTip = "Visualizza Email..."
                            btn.ImageUrl = "~\images\email20.png"
                        Case ParsecPro.TipologiaAllegatoPrimario.Fattura
                            btn.ToolTip = "Visualizza Fattura..."
                            btn.ImageUrl = "~\images\xml_16.png"
                        Case ParsecPro.TipologiaAllegatoPrimario.Generico
                            btn.ToolTip = "Visualizza Allegato Primario..."
                            btn.ImageUrl = "~\images\text_preview.png"
                    End Select


                Else
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.Attributes.Add("onclick", "return false")
                End If

            End If


            'If TypeOf dataItem("ExecuteDisabled").Controls(0) Is ImageButton Then
            '    btn = CType(dataItem("ExecuteDisabled").Controls(0), ImageButton)
            '    btn.Attributes.Add("disabled", "disabled")
            '    '    btn.Attributes.Add("onclick", "ShowPanel();hide=false;")
            'End If





            'Dim parametro As ParsecWKF.ParametroProcesso = Nothing
            'Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
            'parametro = parametriProcesso.GetQuery.Where(Function(c) c.IdProcesso = task.IdIstanza And c.Nome = "Ammissibilita").FirstOrDefault
            'If Not parametro Is Nothing Then
            '    If TypeOf dataItem("Ammissibilita").Controls(0) Is ImageButton Then
            '        btn = CType(dataItem("Ammissibilita").Controls(0), ImageButton)
            '        btn.Attributes.Add("onclick", "return false")
            '        btn.Style.Add("cursor", "default")
            '        Select Case parametro.Valore
            '            Case "0"
            '                'Non ammissibile
            '                btn.ImageUrl = "~/images/pRosso16.png"
            '                btn.ToolTip = "Pratica Non Ammissibile"
            '            Case "1"
            '                'Ammissibile
            '                btn.ImageUrl = "~/images/pVerde16.png"
            '                btn.ToolTip = "Pratica Ammissibile"
            '            Case "2"
            '                'Parzialmente Ammissibile
            '                btn.ImageUrl = "~/images/pArancio16.png"
            '                btn.ToolTip = "Pratica Parzialmente Ammissibile"
            '        End Select
            '    End If
            'End If
        End If
    End Sub

    Protected Sub TaskGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles TaskGridView.NeedDataSource
        '*************************************
        'Filtro di default
        '*************************************
        If Me.TaskAttivi Is Nothing Then
            Dim tasks As New ParsecWKF.TaskRepository
            Me.TaskAttivi = tasks.GetView(Me.Filtro)
            tasks.Dispose()
        End If
        Me.TaskGridView.DataSource = Me.TaskAttivi
    End Sub

    Protected Sub TaskGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TaskGridView.ItemCreated

        '***************************************************************************************************************
        'GESTIONE SELEZIONE MULTIPLA
        '***************************************************************************************************************
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf TaskGridView_ItemPreRender
        End If
        '***************************************************************************************************************

        If TypeOf e.Item Is GridFilteringItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            'e.Item.Style.Add("background-color", "White")
        End If

        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If


        If TypeOf e.Item Is GridNestedViewItem Then
            AddHandler CType(e.Item.FindControl("IterGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.IterGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("IterGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.IterGridView_ItemCreated)
        End If

        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    '***************************************************************************************************************
    'GESTIONE SELEZIONE MULTIPLA
    '***************************************************************************************************************
    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In TaskGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        'Me.SaveSelectedItems()
    End Sub

    Protected Sub TaskGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub TaskGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles TaskGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.TaskGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.TaskGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.TaskGridView.SelectedItems.Count = Me.TaskGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.TaskGridView.Items.Count > 0
    End Sub

    '***************************************************************************************************************

#End Region

#Region "EVENTI GRIGLIA INTERNA"

    Protected Sub IterGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Dim parentItem As Telerik.Web.UI.GridDataItem = CType(CType(CType(sender, Telerik.Web.UI.RadGrid).NamingContainer, Telerik.Web.UI.GridNestedViewItem).ParentItem, Telerik.Web.UI.GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("IdIstanza")
        CType(sender, Telerik.Web.UI.RadGrid).DataSource = (New ParsecWKF.IstanzaRepository).GetTaskIstanze(id)
    End Sub

    Protected Sub IterGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub SbloccaTasks(idUtente As Integer)
        'Elimino tutti i task bloccati dall'utente corrente.
        Dim taskBloccati As New ParsecWKF.LockTaskRepository
        taskBloccati.DeleteAll(idUtente)
        taskBloccati.Dispose()
    End Sub

    Private Sub ImpostaFiltroPredefinito()
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim idUtente As Integer = utenteCorrente.Id
        If idUtente <> CInt(Me.DelegheScrivaniaComboBox.SelectedItem.Value) Then
            idUtente = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        End If
        Me.Filtro = New ParsecWKF.TaskFiltro With {.IdUtente = idUtente}
    End Sub

    Private Sub SaveCookie()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim cookie As HttpCookie = Request.Cookies("FilterPreferences_" & utenteCollegato.Id.ToString)
        If cookie Is Nothing Then
            cookie = New HttpCookie("FilterPreferences_" & utenteCollegato.Id.ToString)
        End If
        cookie("FilterModule") = Me.ModuliComboBox.SelectedValue
        cookie("FilterDocumentoType") = Me.TipologiaDocumentoComboBox.SelectedValue
        cookie("FilterStatoType") = Me.StatoComboBox.SelectedValue
        If Me.DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue Then
            cookie("FilterDateFrom") = Me.DataInizioIstanzaFiltroTextBox.SelectedDate
        Else
            cookie("FilterDateFrom") = Nothing
        End If


        If Me.DataFineIstanzaFiltroTextBox.SelectedDate.HasValue Then
            cookie("FilterDateTo") = Me.DataFineIstanzaFiltroTextBox.SelectedDate
        Else
            cookie("FilterDateTo") = Nothing
        End If

        cookie.Expires = DateTime.Now.AddYears(100)
        Response.Cookies.Add(cookie)
    End Sub

    Private Sub GetCookie()

        Me.Filtri.Clear()

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim cookie As HttpCookie = Request.Cookies("FilterPreferences_" & utenteCollegato.Id.ToString)
        If Not cookie Is Nothing Then

            Dim filtro As FiltroCorrente = Nothing
            If Not String.IsNullOrEmpty(cookie("FilterDateFrom")) Then
                Me.DataInizioIstanzaFiltroTextBox.SelectedDate = cookie("FilterDateFrom")
            End If

            If Not String.IsNullOrEmpty(cookie("FilterDateTo")) Then
                Me.DataFineIstanzaFiltroTextBox.SelectedDate = cookie("FilterDateTo")
            End If


            If Me.DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue Then

                filtro = New FiltroCorrente
                filtro.Id = TipologiaFiltro.DataInizio
                filtro.Valore = Me.DataInizioIstanzaFiltroTextBox.SelectedDate.Value
                filtro.Descrizione = "Da: " & Me.DataInizioIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
                filtro.Tooltip = "Data Inizio Da: " & Me.DataInizioIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
                Me.Filtri.Add(filtro)
            End If

            If Me.DataFineIstanzaFiltroTextBox.SelectedDate.HasValue Then

                filtro = New FiltroCorrente
                filtro.Id = TipologiaFiltro.DataFine
                filtro.Valore = Me.DataFineIstanzaFiltroTextBox.SelectedDate.Value
                filtro.Descrizione = "A: " & Me.DataFineIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
                filtro.Tooltip = "Data Inizio A: " & Me.DataFineIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
                Me.Filtri.Add(filtro)
            End If


            Dim moduloItem As RadComboBoxItem = Me.ModuliComboBox.Items.FindItemByValue(cookie("FilterModule"))
            Dim documentoItem As RadComboBoxItem = Me.TipologiaDocumentoComboBox.Items.FindItemByValue(cookie("FilterDocumentoType"))
            Dim statoItem As RadComboBoxItem = Me.StatoComboBox.Items.FindItemByValue(cookie("FilterStatoType"))

            If Not moduloItem Is Nothing AndAlso Not documentoItem Is Nothing AndAlso Not statoItem Is Nothing Then
                moduloItem.Selected = True
                documentoItem.Selected = True
                statoItem.Selected = True



                If moduloItem.Index > 0 Then
                    filtro = New FiltroCorrente
                    filtro.Id = TipologiaFiltro.Modulo
                    filtro.Valore = moduloItem.Text
                    filtro.Descrizione = moduloItem.Text
                    filtro.Tooltip = moduloItem.Text
                    Me.Filtri.Add(filtro)

                    'Filtro la tipologia di documenti
                    Me.CaricaModelliWorkflow(CInt(moduloItem.Value))

                    'Seleziono l'elemento
                    documentoItem = Me.TipologiaDocumentoComboBox.Items.FindItemByValue(cookie("FilterDocumentoType"))
                    If Not documentoItem Is Nothing Then
                        documentoItem.Selected = True
                    End If

                End If

                If documentoItem.Index > 0 Then
                    filtro = New FiltroCorrente
                    filtro.Id = TipologiaFiltro.Documento
                    filtro.Valore = documentoItem.Text
                    filtro.Descrizione = documentoItem.Text
                    filtro.Tooltip = documentoItem.Text
                    Me.Filtri.Add(filtro)
                End If

                If statoItem.Index > 0 Then
                    filtro = New FiltroCorrente
                    filtro.Id = TipologiaFiltro.Stato
                    filtro.Valore = statoItem.Text
                    filtro.Descrizione = statoItem.Text
                    filtro.Tooltip = statoItem.Text
                    Me.Filtri.Add(filtro)
                End If
            End If

            Me.Search()
        End If
    End Sub

    Private Sub Print()
        'Dim parametriStampa As New Hashtable
        'parametriStampa.Add("TipologiaStampa", "StampaQualificheOrganigramma")
        'parametriStampa.Add("DatiStampa", Me.Avvisi)
        'Session("ParametriStampa") = parametriStampa
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Search()
        Dim idUtente As Integer = Me.DelegheScrivaniaComboBox.SelectedItem.Value
        Dim tasks As New ParsecWKF.TaskRepository
        Dim filtro As New ParsecWKF.TaskFiltro
        filtro.IdUtente = idUtente
        filtro.RiferimentoDocumento = Me.RiferimentoDocumentoTextBox.Text
        If Me.StatoComboBox.SelectedIndex <> 0 Then
            filtro.Stato = Me.StatoComboBox.SelectedItem.Text
        End If
        If Me.TipologiaDocumentoComboBox.SelectedIndex <> 0 Then
            filtro.IdModello = Me.TipologiaDocumentoComboBox.SelectedItem.Value
        End If
        If Me.ModuliComboBox.SelectedIndex > 0 Then
            filtro.IdModulo = CInt(Me.ModuliComboBox.SelectedValue)
        End If

        If Me.DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue Then
            filtro.DataInizioInserimento = Me.DataInizioIstanzaFiltroTextBox.SelectedDate
        End If

        If Me.DataFineIstanzaFiltroTextBox.SelectedDate.HasValue Then
            filtro.DataFineInserimento = Me.DataFineIstanzaFiltroTextBox.SelectedDate
        End If


        Me.Filtro = filtro
        Me.TaskAttivi = tasks.GetView(Me.Filtro)
        tasks.Dispose()
        Me.TaskGridView.Rebind()


    End Sub

    Private Sub ResetSearch()

        For Each col As GridColumn In Me.TaskGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.TaskGridView.MasterTableView.FilterExpression = String.Empty

        Me.Filters = New List(Of FilterInfo)
        Me.Filtri.Clear()
        Me.RiferimentoDocumentoTextBox.Text = String.Empty
        Me.StatoComboBox.SelectedIndex = 0
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        Me.ModuliComboBox.SelectedIndex = 0
        Me.DataInizioIstanzaFiltroTextBox.SelectedDate = Nothing
        Me.DataFineIstanzaFiltroTextBox.SelectedDate = Nothing
        Me.TaskAttivi = Nothing
        Me.ImpostaFiltroPredefinito()
        Me.TaskGridView.Rebind()
    End Sub

    Private Sub CaricaModelliWorkflow(ByVal idModulo As Nullable(Of Integer))
        Dim modelli As New ParsecWKF.ModelliRepository
        Me.TipologiaDocumentoComboBox.DataSource = modelli.GetKeyValue(idModulo)
        Me.TipologiaDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologiaDocumentoComboBox.DataValueField = "Id"
        Me.TipologiaDocumentoComboBox.DataBind()
        Me.TipologiaDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Tutte -"))
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        modelli.Dispose()
    End Sub

    Private Sub CaricaDeleghe(ByVal utenteCorrente As ParsecAdmin.Utente)
        Dim tasks As New ParsecWKF.TaskRepository
        Me.DelegheScrivaniaComboBox.DataSource = tasks.GetUtentiDelegantiScrivania(utenteCorrente)
        Me.DelegheScrivaniaComboBox.DataTextField = "Descrizione"
        Me.DelegheScrivaniaComboBox.DataValueField = "Id"
        Me.DelegheScrivaniaComboBox.DataBind()
        Me.DelegheScrivaniaComboBox.FindItemByValue(utenteCorrente.Id).Selected = True
        tasks.Dispose()
    End Sub

    Private Sub CaricaStatiWorkflow()
        Dim tasks As New ParsecWKF.TaskRepository
        Me.StatoComboBox.DataSource = tasks.GetKeyValue.Where(Function(c) c.Descrizione <> String.Empty).ToList
        Me.StatoComboBox.DataTextField = "Descrizione"
        Me.StatoComboBox.DataValueField = "Id"
        Me.StatoComboBox.DataBind()
        Me.StatoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Tutti -"))
        Me.StatoComboBox.SelectedIndex = 0
        tasks.Dispose()
    End Sub

    Private Sub CaricaModuli()
        Dim moduli As New ParsecAdmin.ModuleRepository
        Me.ModuliComboBox.DataSource = moduli.GetQuery.Where(Function(c) c.Id = 2 Or c.Id = 3 Or c.Id = 5 Or c.Id = 17 Or c.Id = 1001 Or c.Id = 1002 Or c.Id = 1003).OrderBy(Function(c) c.Descrizione).ToList
        Me.ModuliComboBox.DataTextField = "Descrizione"
        Me.ModuliComboBox.DataValueField = "Id"
        Me.ModuliComboBox.DataBind()
        Me.ModuliComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Tutti -"))
        moduli.Dispose()
    End Sub


    Private Sub Esporta()
        Dim lista = Me.FiltraTaskAttivi

        If lista.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Non ci sono attività." & vbCrLf & "Impossibile eseguire l'esportazione!", False)
            Exit Sub
        End If

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("Task_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)

        Dim line As New StringBuilder

        Dim settori As String = String.Empty
        line.Append("DOCUMENTO" & vbTab)
        line.Append("PROPONENTE/MITTENTE" & vbTab)
        line.Append("STATO" & vbTab)
       

        swExport.WriteLine(line.ToString)
        line.Clear()



        For Each t As ParsecWKF.TaskAttivo In lista
            line.Append(If(Not String.IsNullOrEmpty(t.DescrizioneDocumento), t.DescrizioneDocumento.Replace(vbCrLf, " "), String.Empty) & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(t.Proponente), t.Proponente.Replace(vbCrLf, " "), String.Empty) & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(t.TaskCorrente), t.TaskCorrente.Replace(vbCrLf, " "), String.Empty) & vbTab)
            swExport.WriteLine(line.ToString)
            line.Clear()
        Next


        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

        Me.EsportaInExcelImageButton.Enabled = True
    End Sub

#End Region

#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

    Private Sub RegistraParsecDigitalSign()
        Dim script As String = ParsecAdmin.SignParameters.RegistraParsecDigitalSign
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub AggiornaTaskButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaTaskButton.Click
        'Me.TaskAttivi = Nothing
        'Me.TaskGridView.Rebind()
        Me.Search()

        Dim script As New StringBuilder
        script.AppendLine("<script>")
        script.AppendLine("HidePanel(currentPanel);hide = true;panelIsVisible=false;")
        script.AppendLine("</script>")

        ParsecUtility.Utility.RegisterScript(script, False)


    End Sub

    Protected Sub DelegheScrivaniaComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles DelegheScrivaniaComboBox.SelectedIndexChanged
        Me.Search()
    End Sub

    Protected Sub SalvaFiltroButton_Click(sender As Object, e As System.EventArgs) Handles SalvaFiltroButton.Click
        SaveCookie()
    End Sub

    Protected Sub continuaSenzaIntegrazioneButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles continuaSenzaIntegrazioneButton.Click
        Dim script As New StringBuilder
        script.AppendLine("<script>")
        script.AppendLine("currentPanel=0;ShowPanel(currentPanel);hide=false;panelIsVisible = false;enableUiHidden='" & Me.OperazioneControl.GetEnableUiHiddenControl & "'")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)

        '' ''Me.OperazioneControlSUAPE.Visible = False
        Me.OperazioneControl.Visible = True
        Me.OperazioneControl.InitUI(Me.DelegheScrivaniaComboBox.SelectedItem.Value, Me.TaskCorrente)
        Me.TaskCorrente = Nothing
    End Sub

    Protected Sub ApplicaFiltroButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ApplicaFiltroButton.Click

        Me.Filtri.Clear()
        Dim filtro As FiltroCorrente = Nothing

        If Me.ModuliComboBox.SelectedIndex > 0 Then
            filtro = New FiltroCorrente
            filtro.Id = TipologiaFiltro.Modulo
            filtro.Valore = Me.ModuliComboBox.Text
            filtro.Descrizione = Me.ModuliComboBox.Text
            filtro.Tooltip = Me.ModuliComboBox.Text
            Me.Filtri.Add(filtro)
        End If

        If Me.TipologiaDocumentoComboBox.SelectedIndex > 0 Then
            filtro = New FiltroCorrente
            filtro.Id = TipologiaFiltro.Documento
            filtro.Valore = Me.TipologiaDocumentoComboBox.Text
            filtro.Descrizione = Me.TipologiaDocumentoComboBox.Text
            filtro.Tooltip = Me.TipologiaDocumentoComboBox.Text
            Me.Filtri.Add(filtro)
        End If

        If Me.StatoComboBox.SelectedIndex > 0 Then
            filtro = New FiltroCorrente
            filtro.Id = TipologiaFiltro.Stato
            filtro.Valore = Me.StatoComboBox.Text
            filtro.Tooltip = Me.StatoComboBox.Text
            filtro.Descrizione = Me.StatoComboBox.Text
            Me.Filtri.Add(filtro)
        End If


        If Me.DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue Then

            filtro = New FiltroCorrente
            filtro.Id = TipologiaFiltro.DataInizio
            filtro.Valore = Me.DataInizioIstanzaFiltroTextBox.SelectedDate.Value
            filtro.Descrizione = "Da: " & Me.DataInizioIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
            filtro.Tooltip = "Data Inizio Da: " & Me.DataInizioIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
            Me.Filtri.Add(filtro)
        End If

        If Me.DataFineIstanzaFiltroTextBox.SelectedDate.HasValue Then

            filtro = New FiltroCorrente
            filtro.Id = TipologiaFiltro.DataFine
            filtro.Valore = Me.DataFineIstanzaFiltroTextBox.SelectedDate.Value
            filtro.Descrizione = "A: " & Me.DataFineIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
            filtro.Tooltip = "Data Inizio A: " & Me.DataFineIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
            Me.Filtri.Add(filtro)
        End If

        Me.Search()
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResetSearch()
    End Sub

    Protected Sub AnnullaFiltroButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaFiltroButton.Click

        Me.CaricaModelliWorkflow(CType(Nothing, Nullable(Of Integer)))
        Me.ModuliComboBox.SelectedIndex = 0
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        Me.StatoComboBox.SelectedIndex = 0
        Me.DataInizioIstanzaFiltroTextBox.SelectedDate = Nothing
        Me.DataFineIstanzaFiltroTextBox.SelectedDate = Nothing

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim cookie As HttpCookie = Request.Cookies("FilterPreferences_" & utenteCollegato.Id.ToString)
        If Not cookie Is Nothing Then
            Dim moduloItem As RadComboBoxItem = Me.ModuliComboBox.Items.FindItemByValue(cookie("FilterModule"))
            Dim documentoItem As RadComboBoxItem = Me.TipologiaDocumentoComboBox.Items.FindItemByValue(cookie("FilterDocumentoType"))
            Dim statoItem As RadComboBoxItem = Me.StatoComboBox.Items.FindItemByValue(cookie("FilterStatoType"))

            If Not moduloItem Is Nothing AndAlso Not documentoItem Is Nothing AndAlso Not statoItem Is Nothing Then
                moduloItem.Selected = True
                documentoItem.Selected = True
                statoItem.Selected = True
            End If

        End If
    End Sub

    Protected Sub ResettaFiltroButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ResettaFiltroButton.Click
        Me.ModuliComboBox.SelectedIndex = 0
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        Me.StatoComboBox.SelectedIndex = 0
    End Sub

    Protected Sub ModuliComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ModuliComboBox.SelectedIndexChanged
        Dim idModulo As Nullable(Of Integer) = Nothing
        If Me.ModuliComboBox.SelectedIndex > 0 Then
            idModulo = CInt(e.Value)
        End If
        Me.CaricaModelliWorkflow(idModulo)
    End Sub

    Protected Sub SbloccaTaskButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles SbloccaTaskButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.SbloccaTasks(utenteCollegato.Id)
        Me.Search()
    End Sub


    Protected Sub EsportaInExcelImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EsportaInExcelImageButton.Click
        Me.Esporta()
    End Sub

#End Region

  
End Class