Imports ParsecAdmin
Imports System.Transactions
Imports System.Web.Services
Imports Telerik.Web.UI
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class FascicoliPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Private Sub PrintElencoDocumenti()
        If Me.Fascicolo Is Nothing Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un fascicolo!", False)
            Exit Sub
        End If
        Me.GeneraReportPdf()
    End Sub

    Private Function GetCliente() As ParsecAdmin.Cliente
        Dim clientRepository As New ParsecAdmin.ClientRepository
        Dim cliente As ParsecAdmin.Cliente = clientRepository.GetQuery.FirstOrDefault
        clientRepository.Dispose()
        Return cliente
    End Function
    Private Function CreateHeaderTable() As PdfPTable

        Dim font As Font = FontFactory.GetFont("Arial", 18, Font.BOLD)

        Dim headerTable As New PdfPTable(2)
        headerTable.TotalWidth = 805
        headerTable.LockedWidth = True
        headerTable.SetWidths(New Single() {500.0F, 305.0F})
        Dim headerCell As New PdfPCell(New Paragraph("Stampa Fascicolo", font))

        headerCell.HorizontalAlignment = Element.ALIGN_LEFT
        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
        headerCell.FixedHeight = 30
        headerCell.Border = Rectangle.NO_BORDER
        headerTable.AddCell(headerCell)


        headerCell = New PdfPCell(New Paragraph(GetCliente.Descrizione, font))

        headerCell.HorizontalAlignment = Element.ALIGN_RIGHT
        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
        headerCell.FixedHeight = 30
        headerCell.Border = Rectangle.NO_BORDER
        headerTable.AddCell(headerCell)

        headerTable.SpacingAfter = 10

        Return headerTable
    End Function

    Private Function CreateHeaderCell(ByVal text As String) As PdfPCell
        Dim p As New Paragraph(text, FontFactory.GetFont("Arial", 9, Font.BOLD))
        p.Alignment = Element.ALIGN_CENTER

        Dim cell As New PdfPCell(p)
        'cell.HorizontalAlignment = Element.ALIGN_CENTER
        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE

        cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER
        cell.BackgroundColor = New iTextSharp.text.BaseColor(224, 224, 224)
        cell.PaddingBottom = 4
        Return cell
    End Function

    Private Function CreateCell(ByVal text As String, ByVal align As Integer, ByVal fontSize As Integer) As PdfPCell
        text = text.Replace(vbCrLf, " ")
        text = text.Replace(vbCr, " ")
        text = text.Replace(vbLf, " ")
        Dim p As New Paragraph(text, FontFactory.GetFont("Arial", fontSize, Font.NORMAL))
        p.Alignment = align

        Dim cell As New PdfPCell(p)
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 8
        cell.PaddingRight = 3
        cell.Border = 0
        Return cell
    End Function

    Private Function CreateCellBold(ByVal text As String, ByVal align As Integer, ByVal fontSize As Integer) As PdfPCell
        text = text.Replace(vbCrLf, " ")
        text = text.Replace(vbCr, " ")
        text = text.Replace(vbLf, " ")
        Dim p As New Paragraph(text, FontFactory.GetFont("Arial", fontSize, Font.BOLD))
        p.Alignment = align

        Dim cell As New PdfPCell(p)
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 8


        cell.Border = 0
        Return cell
    End Function

    Private Function CreateDetailTable() As PdfPTable
        Dim cell As PdfPCell = Nothing

        Dim detailTable = New PdfPTable(1)
        detailTable.TotalWidth = 805
        detailTable.LockedWidth = True

        Dim detailCell As New PdfPCell
        detailCell.Border = 0

        Dim titleTable As PdfPTable

        titleTable = New PdfPTable(1)
        titleTable.TotalWidth = 805
        titleTable.LockedWidth = True


        cell = Me.CreateCellBold("Dettaglio Fascicolo", Element.ALIGN_LEFT, 14)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        titleTable.AddCell(cell)

        detailCell.AddElement(titleTable)
        detailTable.AddCell(detailCell)

        detailCell = New PdfPCell
        detailCell.Border = 0


        Dim innerTable As PdfPTable

        innerTable = New PdfPTable(8)
        innerTable.TotalWidth = 805
        innerTable.SetWidths(New Single() {80.0F, 121.25F, 80.0F, 121.25F, 80.0F, 121.25F, 80.0F, 121.25F})
        innerTable.LockedWidth = True


        cell = Me.CreateCellBold("Tipologia:", Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCell(Me.TipologiaFascicoloComboBox.Text, Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCellBold("Codice:", Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCell(Me.CodiceFascicoloSistemaTextBox.Text & Me.CodiceFascicoloUtenteTextBox.Text, Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCellBold("Data Apertura:", Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        Dim dataApertura As String = String.Empty
        If Me.DataTextBox.SelectedDate.HasValue Then
            dataApertura = Me.DataTextBox.SelectedDate.Value.ToShortDateString
        End If

        cell = Me.CreateCell(dataApertura, Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCellBold("Data Chiusura:", Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        Dim dataChiusura As String = String.Empty
        If Me.DataChiusuraTextBox.SelectedDate.HasValue Then
            dataChiusura = Me.DataChiusuraTextBox.SelectedDate.Value.ToShortDateString
        End If

        cell = Me.CreateCell(dataChiusura, Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCellBold("Classificazione:", Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCell(Me.ClassificazioneTextBox.Text, Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCellBold("Procedimento:", Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCell(Me.ProcedimentoComboBox.Text, Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCellBold("Responsabile:", Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCell(Me.ResponsabileTextBox.Text, Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCellBold("Oggetto:", Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        cell = Me.CreateCell(Me.OggettoTextBox.Text, Element.ALIGN_LEFT, 9)
        cell.Border = PdfPCell.BOTTOM_BORDER
        cell.BorderWidth = 1
        cell.BorderColor = New iTextSharp.text.BaseColor(221, 221, 221)
        cell.MinimumHeight = 20
        innerTable.AddCell(cell)

        detailCell.AddElement(innerTable)
        detailTable.AddCell(detailCell)

        detailTable.SpacingAfter = 10

        Return detailTable

    End Function


    Private Sub GeneraReportPdf()

        Dim stream As New IO.MemoryStream
        Using document As New Document(PageSize.A4.Rotate, 18.5, 18.5, 36, 36)

            Dim canvas As PdfContentByte
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, stream)

            'Dim pageEventHelper = New PageEventHelper()
            'writer.PageEvent = pageEventHelper

            document.Open()

            canvas = writer.DirectContent
            Dim pageSize As Rectangle = document.PageSize

            document.Add(Me.CreateHeaderTable)

            document.Add(Me.CreateDetailTable)


            Dim pg As New iTextSharp.text.Paragraph("Elenco Documenti", FontFactory.GetFont("Arial", 14, Font.BOLD))
            pg.SpacingAfter = 10
            document.Add(pg)

            Dim headerTable As New PdfPTable(5)

            headerTable.TotalWidth = 805
            headerTable.LockedWidth = True
            Dim widths As Single() = New Single() {100.0F, 100.0F, 100.0F, 405.0F, 100.0F}


            headerTable.SetWidths(widths)


            Dim columnNames As New List(Of String) From {"TIPO", "FASE", "CATEGORIA", "DOCUMENTO", "INSERITO IL"}

            For i As Integer = 0 To columnNames.Count - 1
                headerTable.AddCell(Me.CreateHeaderCell(columnNames(i)))
            Next

            document.Add(headerTable)

            Dim detailTable As PdfPTable
            Dim innerTable As PdfPTable


            Dim detailCell As PdfPCell


            For Each p In Me.Documenti


                detailTable = New PdfPTable(1)
                detailTable.TotalWidth = 805
                detailTable.LockedWidth = True
                detailTable.DefaultCell.Border = 0


                innerTable = New PdfPTable(5)
                innerTable.TotalWidth = 805
                innerTable.LockedWidth = True
                innerTable.DefaultCell.Border = PdfPCell.BOTTOM_BORDER
                innerTable.SetWidths(widths)


                innerTable.AddCell(Me.CreateCell(p.DescrizioneTipoDocumento, Element.ALIGN_LEFT, 9))
                innerTable.AddCell(Me.CreateCell(p.DescrizioneFase, Element.ALIGN_LEFT, 9))
                innerTable.AddCell(Me.CreateCell(p.DescrizioneCategoria, Element.ALIGN_LEFT, 9))
                innerTable.AddCell(Me.CreateCell(p.NomeDocumentoOriginale, Element.ALIGN_LEFT, 9))
                innerTable.AddCell(Me.CreateCell(String.Format("{0:dd-MM-yyyy}", p.DataDocumento), Element.ALIGN_LEFT, 9))


                ' Dim n = iTextSharp.text.Utilities.MillimetersToPoints(33.0F)


                detailCell = New PdfPCell
                detailCell.Border = 0

                detailCell.AddElement(innerTable)


                detailTable.AddCell(detailCell)


                detailTable.KeepTogether = True


                document.Add(detailTable)

            Next


            document.NewPage()

        End Using

        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaElencoDocumentiFascicolo")
        parametriStampa.Add("FullPath", stream.GetBuffer)
        Session("ParametriStampaPro") = parametriStampa
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)

    End Sub


    Protected Function DataDocumentoSenzaOrario(ByVal container As GridItem) As System.Nullable(Of DateTime)
        If container.OwnerTableView.GetColumn("DataDocumentoSenzaOrario").CurrentFilterValue = String.Empty Then
            Return New System.Nullable(Of DateTime)()
        Else
            Try
                Return DateTime.Parse(container.OwnerTableView.GetColumn("DataDocumentoSenzaOrario").CurrentFilterValue)
            Catch ex As Exception
                Return New System.Nullable(Of DateTime)()
            End Try

        End If
    End Function


    Public Function DescrizioneDocumentoTooltip(dataItem As Object) As String
        Dim res As String = String.Empty

        Dim idDocumento As Integer = DataBinder.Eval(dataItem, "IdDocumento").ToString()

        Dim descrizioneTipo As String = DataBinder.Eval(dataItem, "DescrizioneTipoDocumento").ToString()

        Select Case descrizioneTipo
            Case "PRO"
                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim registrazione = registrazioni.Where(Function(c) c.Id = idDocumento).FirstOrDefault
                If Not registrazione Is Nothing Then
                    res = registrazione.Oggetto
                End If
                registrazioni.Dispose()
            Case "ATT"

                Dim attiAmministrativi As New ParsecAtt.DocumentoRepository
                Dim attoAmministrativo = attiAmministrativi.Where(Function(c) c.Id = idDocumento).FirstOrDefault
                If Not attoAmministrativo Is Nothing Then
                    res = attoAmministrativo.Oggetto
                End If
                attiAmministrativi.Dispose()

            Case Else
                res = DataBinder.Eval(dataItem, "NomeDocumentoOriginale").ToString()
        End Select

        Return res

    End Function



    Public Property RegistraScriptChiusura As Boolean
        Set(ByVal value As Boolean)
            Session("FascicoloPage_RegistraScriptChiusura") = value
        End Set
        Get
            Return CType(Session("FascicoloPage_RegistraScriptChiusura"), Boolean)
        End Get
    End Property

    'Elenco di gruppi o di utenti abilitati a visualizzare un protcollo.
    Public Property Visibilita() As List(Of ParsecAdmin.VisibilitaDocumento)
        Get
            Return Session("FascicoloPage_Visibilita")
        End Get
        Set(ByVal value As List(Of ParsecAdmin.VisibilitaDocumento))
            Session("FascicoloPage_Visibilita") = value
        End Set
    End Property

    Public Property Fascicolo() As ParsecAdmin.Fascicolo
        Get
            Return CType(Session("FascicoliPage_Fascicolo"), Object)
        End Get
        Set(ByVal value As ParsecAdmin.Fascicolo)
            Session("FascicoliPage_Fascicolo") = value
        End Set
    End Property

    Public Property Fascicoli() As List(Of ParsecAdmin.Fascicolo)
        Get
            Return CType(Session("FascicoliPage_Fascicoli"), List(Of ParsecAdmin.Fascicolo))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Fascicolo))
            Session("FascicoliPage_Fascicoli") = value
        End Set
    End Property

    Public Property Documenti() As List(Of ParsecAdmin.FascicoloDocumento)
        Get
            Return CType(Session("FascicoliPage_Documenti"), List(Of ParsecAdmin.FascicoloDocumento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.FascicoloDocumento))
            Session("FascicoliPage_Documenti") = value
        End Set
    End Property

    Public Property Titolari() As List(Of ParsecAdmin.TitolareFascicolo)
        Get
            Return CType(Session("FascicoliPage_Titolari"), List(Of ParsecAdmin.TitolareFascicolo))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.TitolareFascicolo))
            Session("FascicoliPage_Titolari") = value
        End Set
    End Property


    Public Property Amministrazioni() As List(Of ParsecAdmin.FascicoloAmministrazionePartecipante)
        Get
            Return CType(Session("FascicoliPage_Amministrazioni"), List(Of ParsecAdmin.FascicoloAmministrazionePartecipante))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.FascicoloAmministrazionePartecipante))
            Session("FascicoliPage_Amministrazioni") = value
        End Set
    End Property

    Public Property Filtro() As ParsecAdmin.FascicoloFiltro
        Get
            Return CType(Session("FascicoliPage_Filtro"), ParsecAdmin.FascicoloFiltro)
        End Get
        Set(ByVal value As ParsecAdmin.FascicoloFiltro)
            Session("FascicoliPage_Filtro") = value
        End Set
    End Property

    Public Property Salvato As Boolean
        Get
            Return ViewState("Salvato")
        End Get
        Set(value As Boolean)
            ViewState("Salvato") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BasePage.master"
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".CustomFooter .RadGrid .rgFooter { background-image: none;background-color:#DFE8F6;}"
        Me.Page.Header.Controls.Add(css)

        If Page.Request("Mode") Is Nothing Then
            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Amministrazione"
            Me.MainPage.DescrizioneProcedura = "> Fascicolo"
        ElseIf Page.Request("Mode") = "update" Then
            Me.MainPage = CType(Me.Master, BasePage)
            CType(Me.Master, BasePage).ImpostaLarghezzaHeader(900)
            CType(Me.Master, BasePage).DescrizioneProcedura = "Modifica Fascicolo"
        ElseIf Page.Request("Mode") = "insert" Then
            Me.MainPage = CType(Me.Master, BasePage)
            CType(Me.Master, BasePage).ImpostaLarghezzaHeader(900)
            CType(Me.Master, BasePage).DescrizioneProcedura = "Nuovo Fascicolo"
        ElseIf Page.Request("Mode") = "read" Then
            Me.MainPage = CType(Me.Master, BasePage)
            CType(Me.Master, BasePage).ImpostaLarghezzaHeader(900)
            CType(Me.Master, BasePage).DescrizioneProcedura = "Visualizza Fascicolo"
        End If


        If Not IsPostBack Then
            Me.Salvato = False
            Me.Fascicoli = Nothing

            'Me.ResettaGridView()
            Me.ResettaVista()

            Me.DataTextBox.SelectedDate = String.Format("{0:dd/MM/yyyy}", Now)


            'Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            'Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
            'utenteVisibilita.IdEntita = utenteCollegato.Id
            'utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
            'utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.SEP
            'utenteVisibilita.Descrizione = (If(utenteCollegato.Username = Nothing, "", utenteCollegato.Username) + " - " + If(utenteCollegato.Cognome = Nothing, "", utenteCollegato.Cognome) + " " + If(utenteCollegato.Nome = Nothing, "", utenteCollegato.Nome))
            'utenteVisibilita.LogIdUtente = utenteCollegato.Id
            'utenteVisibilita.LogDataOperazione = Now
            'utenteVisibilita.AbilitaCancellaEntita = False
            'Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)

            'Stampa elenco procedimenti

            Me.CaricaTipologiaDocumento()
            Me.CaricaFasi()

            Me.CaricaElencoProcedimentiStampa()
            CaricaTipologiaFascicoliFiltro()

            Me.CaricaTipologiaFascicoli()
            Me.CaricaCataloghiDocumentoFascicolo()

            Me.ResettaFiltroStampa()



            'Me.Filtro = Nothing
            Me.Filtro = New ParsecAdmin.FascicoloFiltro
            Me.Filtro.UltimeCinque = True

            Me.RegistraScriptChiusura = True

        End If

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'Modificare come c.. si vuole
        'Me.TrovaDocumentoStoricoImageButton.Visible = (utenteCorrente.Username.ToString.ToLower = "a")

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)

        Me.DocumentiPanel.Style.Add("width", widthStyle)
        Me.DocumentiGridView.Style.Add("width", widthStyle)

        'Me.TitolariPanel.Style.Add("width", widthStyle)
        'Me.TitolariGridView.Style.Add("width", widthStyle)

        Me.AmministrazioniPanel.Style.Add("width", widthStyle)
        Me.AmministrazioniGridView.Style.Add("width", widthStyle)


        Me.VisibilitaPanel.Style.Add("width", widthStyle)
        Me.VisibilitaGridView.Style.Add("width", widthStyle)

        If Not IsPostBack Then
            'riempio la lista
            Dim procedimentoRepository As New ParsecAdmin.ProcedimentoRepository
            Me.ProcedimentoComboBox.DataSource = procedimentoRepository.GetView(Nothing)
            Me.ProcedimentoComboBox.DataTextField = "Nome"
            Me.ProcedimentoComboBox.DataValueField = "id"
            Me.ProcedimentoComboBox.DataBind()
            Me.ProcedimentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
            Me.ProcedimentoComboBox.SelectedIndex = 0
            procedimentoRepository.Dispose()
        End If



        Me.ChiudiStampaImageButton.Attributes.Add("onclick", "HidePanel();hide = true; return false;")
        'Me.StampaImageButton.Attributes.Add("onclick", "HidePanel();hide = true; return false;")


        Me.ChiudiUploadButton.Attributes.Add("onclick", "HideUploadPanel();hideUpload = true; return false;")
        Me.ConfermaUploadButton.Attributes.Add("onclick", "HideUploadPanel();hideUpload = true;")


        Me.UploadDaScannerRadioButton.Attributes.Add("onclick", Me.GetClientScript(True))
        Me.UploadDaFileRadioButton.Attributes.Add("onclick", Me.GetClientScript(False))

        Me.ScansionaImageButton.Attributes.Add("onclick", Me.GetClientScript(True))

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraScansione()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadListBox .rlbCheck,.RadListBox .rlbImage,.RadListBox .rlbText{vertical-align: middle;padding-left:5px;}"
        Me.Page.Header.Controls.Add(css)

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.DatiMultiPage.Style.Add("width", widthStyle)
        'Me.TitolariPageView.Style.Add("width", widthStyle)
        Me.DocumentiPageView.Style.Add("width", widthStyle)
        Me.VisibilitaPageView.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        If Me.RegistraScriptChiusura Then
            ParsecUtility.Utility.CloseWindow(False)
        End If
        '***************************************************************************

        If Not Me.Page.IsPostBack Then
            If Not Page.Request("Mode") Is Nothing Then
                Me.GetParametri()

            End If
        End If

        ParsecUtility.Utility.Confirm("Eliminare il Fascicolo selezionato?", False, Not Me.Fascicolo Is Nothing)
        ParsecUtility.Utility.ConfirmDelete("Eliminare dal fascicolo il documento selezionato?", False, "Documento")
        ParsecUtility.Utility.ConfirmDelete("Eliminare tutti i documenti dal fascicolo selezionato?", False, "FascicoloTuttiDocumenti")

        Me.EliminaDocumentiImageButton.Attributes.Remove("onclick")
        Me.EliminaDocumentiImageButton.Attributes.Add("onclick", "return ConfirmDeleteFascicoloTuttiDocumenti();")
        Me.TitoloElencoFascicoliLabel.Text = "Elenco Fascicoli&nbsp;&nbsp;" & If(Me.Fascicoli.Count > 0, "( " & Me.Fascicoli.Count.ToString & " )", "")

        If Not IsPostBack AndAlso Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
            utenteVisibilita.IdEntita = utenteCollegato.Id
            utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
            utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.SEP
            utenteVisibilita.Descrizione = (If(utenteCollegato.Username = Nothing, "", utenteCollegato.Username) + " - " + If(utenteCollegato.Cognome = Nothing, "", utenteCollegato.Cognome) + " " + If(utenteCollegato.Nome = Nothing, "", utenteCollegato.Nome))
            utenteVisibilita.LogIdUtente = utenteCollegato.Id
            utenteVisibilita.LogDataOperazione = Now
            utenteVisibilita.AbilitaCancellaEntita = False
            Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)
        End If

        Me.DatiFascicoloStrip.Tabs(0).Text = "Amministrazioni" & If(Me.Amministrazioni.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Amministrazioni.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        Me.DatiFascicoloStrip.Tabs(1).Text = "Documenti" & If(Me.Documenti.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Documenti.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        Me.DatiFascicoloStrip.Tabs(2).Text = "Visibilità" & If(Me.Visibilita.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Visibilita.Count.ToString & ")</span>", "<span style='width:20px'></span>")

        Me.AggiornaGrigliaVisibilita()
        Me.AggiornaGrigliaDocumenti()
        'Me.AggiornaGrigliaTitolari()
        Me.AggiornaGrigliaAmministrazioni()

        Me.ProcedimentoComboBox.Enabled = (Me.TipologiaFascicoloComboBox.SelectedValue <> TipologiaFascicolo.Affare)


    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "StampaElencoDocumenti"
                Me.PrintElencoDocumenti()
            Case "Salva"

                Dim message As String = String.Empty
                Try
                    Me.Save()
                    Me.AggiornaGriglia()
                    'Me.GeneraRegistroCheckBox.Checked = False
                Catch ex As Exception
                    message = ex.Message
                End Try

                If String.IsNullOrEmpty(message) Then
                    Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                    Me.Salvato = True
                Else
                    ParsecUtility.Utility.MessageBox(message, False)
                End If


            Case "Nuovo"


                Me.Filtro = Nothing
                Me.ResettaVista()
                Me.AggiornaGriglia()


            Case "Annulla"
                'Me.Filtro = Nothing
                Me.Filtro = New ParsecAdmin.FascicoloFiltro
                Filtro.UltimeCinque = True
                Me.ResettaVista()
                Me.AggiornaGriglia()
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Fascicolo Is Nothing Then
                        Dim message As String = String.Empty
                        Try
                            Me.Delete()
                        Catch ex As Exception
                            message = ex.Message
                        End Try
                        If String.IsNullOrEmpty(message) Then
                            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                        Else
                            ParsecUtility.Utility.MessageBox(message, False)
                        End If

                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un fascicolo!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"

            Case "Trova"
                Me.Search()
                Me.ResettaGridView()

            Case "RicercaAvanzata"
                Me.AdvancedSearch()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
        If e.Item.Text = "Stampa registro..." Then
            e.Item.Attributes.Add("onclick", "ShowPanel();hide=false;")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub FascicoliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FascicoliGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub FascicoliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FascicoliGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e)
            Case "ConfirmSelectAndClose"
                ' Me.SelezionaFascicolo(e)
        End Select
    End Sub

    Protected Sub FascicoliGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FascicoliGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        'codiceStrutturaEsternaTitolare
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona fascicolo"
            End If
        End If
    End Sub

    Protected Sub FascicoliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FascicoliGridView.NeedDataSource
        If Me.Fascicoli Is Nothing Then
            Dim fascicoli As New ParsecAdmin.FascicoliRepository
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            If Me.Filtro Is Nothing Then
                Me.Filtro = New ParsecAdmin.FascicoloFiltro
                Me.Filtro.UltimeCinque = True

                Filtro.IdUtenteCollegato = utenteCollegato.Id
                Me.Fascicoli = fascicoli.GetView(Me.Filtro)
            Else

                Filtro.IdUtenteCollegato = utenteCollegato.Id
                Me.Fascicoli = fascicoli.GetView(Me.Filtro)

            End If
            fascicoli.Dispose()
        End If
        Me.FascicoliGridView.DataSource = Me.Fascicoli
    End Sub


#End Region

    '#Region "EVENTI GRIGLIA TITOLARI"

    '    Protected Sub TitolariGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TitolariGridView.ItemCreated
    '        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
    '            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
    '            pageSizeComboBox.Visible = False
    '            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
    '            changePageSizelbl.Visible = False
    '        End If
    '    End Sub

    '    Protected Sub TitolariGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TitolariGridView.ItemCommand
    '        If (e.Item.ItemIndex <> -1) Then
    '            Dim idFornitoreContraente As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("idTitolareFascicolo")
    '            Dim contraenteContratto As ParsecContratti.FornitoreContraente = Nothing
    '            Select Case e.CommandName
    '                Case "Delete"
    '                    Me.EliminaTitolare(e)
    '            End Select
    '        End If
    '    End Sub

    '#End Region

#Region "EVENTI GRIGLIA DOCUMENTI"

    Protected Sub DocumentiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub DocumentiGridView_ItemDataBound(ByVal sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina documento"
                btn.Attributes.Add("onclick", "return ConfirmDeleteDocumento();")
            End If
            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Preview").Controls(0), ImageButton)
                btn.ToolTip = "Visualizza documento"
            End If

            If TypeOf dataItem("Stato").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Stato").Controls(0), ImageButton)
                btn.Attributes.Add("onclick", "return false")
                btn.Style.Add("cursor", "default")
                Dim fascicolo As ParsecAdmin.FascicoloDocumento = CType(e.Item.DataItem, ParsecAdmin.FascicoloDocumento)
                If (fascicolo.Definitivo) Then
                    btn.ImageUrl = "~/images/pVerde16.png"
                    btn.ToolTip = "Stato = Definitivo"
                Else
                    btn.ImageUrl = "~/images/pArancio16.png"
                    btn.ToolTip = "Stato = Provvisorio"
                End If
            End If
        End If
    End Sub

    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Me.EliminaDocumento(e)
            Case "Preview"
                Me.VisualizzaDocumento(e.Item)
        End Select
    End Sub


#End Region

#Region "METODI PRIVATI"

    Private Sub AggiornaGrigliaVisibilita()
        Me.VisibilitaGridView.DataSource = Me.Visibilita
        Me.VisibilitaGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaDocumenti()
        Me.DocumentiGridView.DataSource = Me.Documenti
        Me.DocumentiGridView.DataBind()
    End Sub

    'Private Sub AggiornaGrigliaTitolari()
    '    Me.TitolariGridView.DataSource = Me.Titolari
    '    Me.TitolariGridView.DataBind()
    'End Sub

    Private Sub AggiornaGrigliaAmministrazioni()
        Me.AmministrazioniGridView.DataSource = Me.Amministrazioni
        Me.AmministrazioniGridView.DataBind()
    End Sub

    Private Function GetNuovoIdDocumentoTemporaneo() As Integer
        Dim nuovoId As Integer = -1
        If Me.Documenti.Count > 0 Then
            Dim allId = Me.Documenti.Min(Function(c) c.Id) - 1
            If allId < 0 Then
                nuovoId = allId
            End If
        End If
        Return nuovoId
    End Function



    Private Sub EliminaTitolare(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim idTitolare As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("idTitolareFascicolo")

        'Recupero la cache i documenti associati al fascicolo corrente.
        Dim titolariFascicolo As List(Of ParsecAdmin.TitolareFascicolo) = Me.Titolari

        'Elimino il documento selezionato dalla cache
        Dim titolare As ParsecAdmin.TitolareFascicolo = titolariFascicolo.Where(Function(c) c.idTitolareFascicolo = idTitolare).FirstOrDefault
        If (Not titolare Is Nothing) Then
            titolariFascicolo.Remove(titolare)
        End If

        'Aggiorno la cache che verrà bindata nel Page_Init
        Me.Titolari = titolariFascicolo

    End Sub

    Private Function ricavaIdMinimoAssegnatoListaContraenti(ByVal listaTitolariFascicolo As List(Of ParsecAdmin.TitolareFascicolo)) As Integer
        Dim minimoIndice As Integer = 9999
        For Each voce In listaTitolariFascicolo
            If (voce.idTitolareFascicolo < 0) Then
                If (voce.idTitolareFascicolo <= minimoIndice) Then
                    minimoIndice = voce.idTitolareFascicolo
                End If
            End If
        Next

        If (minimoIndice = 9999) Then
            Return -1
        Else
            Return (minimoIndice - 1)
        End If
    End Function

    Private Sub AdvancedSearch()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaFascicoloPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("modalita", "filtro")
        queryString.Add("obj", Me.AggiornaFascicoloImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 740, 400, queryString, False)
    End Sub

    Private Sub Search()
        Dim fascicoli As New ParsecAdmin.FascicoliRepository
        Dim filtro As ParsecAdmin.FascicoloFiltro = Me.GetFiltro
        filtro.UltimeCinque = False

        Me.Filtro = filtro
        If (Me.ProcedimentoComboBox.SelectedValue <> "") Then
            filtro.IdProcedimento = Me.ProcedimentoComboBox.SelectedValue
        End If
        If (Me.IdResponsabileTextBox.Text <> "") Then
            filtro.IdResponsabile = Me.IdResponsabileTextBox.Text
        End If
        'If (Me.BeneficiarioComboBox.SelectedValue <> "") Then
        '    Dim titolare = getElementoRubrica(Me.BeneficiarioComboBox.SelectedValue)
        '    Dim codiceElemento = titolare.Codice
        '    Dim elementoStrutturaFiltro As New ParsecAdmin.FiltroStrutturaEsternaInfo
        '    Dim rubrica As New ParsecAdmin.RubricaRepository
        '    Dim listaElementiStrutturabyCodice = rubrica.GetQuery().Where(Function(w) w.Codice = codiceElemento).ToList
        '    For Each elemento In listaElementiStrutturabyCodice
        '        filtro.listaIdTitolariToFind.Add(elemento.Id)
        '    Next
        'End If
        Me.Fascicoli = fascicoli.GetView(filtro)
        Me.FascicoliGridView.Rebind()
        fascicoli.Dispose()

    End Sub

    Private Function GetFiltro() As ParsecAdmin.FascicoloFiltro
        Dim filtro As New ParsecAdmin.FascicoloFiltro

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        filtro.IdUtenteCollegato = utenteCollegato.Id



        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            filtro.Oggetto = Me.OggettoTextBox.Text.Trim
        End If

        'If Not String.IsNullOrEmpty(Me.NumeroRegistroTextBox.Text) Then
        '    filtro.NumeroRegistro = CInt(Me.NumeroRegistroTextBox.Text.Trim)
        'End If

        If Not String.IsNullOrEmpty(Me.IdResponsabileTextBox.Text) Then
            filtro.IdResponsabile = CInt(Me.IdResponsabileTextBox.Text.Trim)
        End If

        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            filtro.IdClassificazione = CInt(Me.IdClassificazioneTextBox.Text.Trim)
        End If



        If Not String.IsNullOrEmpty(Me.CodiceFascicoloUtenteTextBox.Text) Then
            filtro.CodiceFascicoloUtente = Me.CodiceFascicoloUtenteTextBox.Text.Trim
        End If

        'If Not String.IsNullOrEmpty(Me.CodiceFascicoloSistemaTextBox.Text) Then
        '    filtro.CodiceFascicoloSistema = Me.CodiceFascicoloSistemaTextBox.Text.Trim
        'End If

        If Me.DataTextBox.SelectedDate.HasValue Then
            filtro.DataApertura = Me.DataTextBox.SelectedDate
        End If

        If Me.DataChiusuraTextBox.SelectedDate.HasValue Then
            filtro.DataChiusura = Me.DataChiusuraTextBox.SelectedDate
        End If
        If Me.TipologiaFascicoloComboBox.SelectedIndex <> 0 Then
            filtro.IdTipologiaFascicolo = CInt(Me.TipologiaFascicoloComboBox.SelectedValue)
        End If



        Return filtro
    End Function

    Private Sub ApplicaFiltro()
        If Not ParsecUtility.SessionManager.FiltroFascicolo Is Nothing Then
            Dim filtro As ParsecAdmin.FascicoloFiltro = ParsecUtility.SessionManager.FiltroFascicolo
            Dim fascicoliRep As New ParsecAdmin.FascicoliRepository
            Me.Fascicoli = fascicoliRep.GetView(filtro)
            fascicoliRep.Dispose()
            Me.FascicoliGridView.Rebind()
            ParsecUtility.SessionManager.FiltroFascicolo = Nothing
            Me.ResettaVista()
        End If
    End Sub

    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina


            'Disabilito tutti i pulsanti della toolbar
            For i As Integer = 0 To Me.RadToolBar.Items.Count - 1
                Me.RadToolBar.Items(i).Enabled = False
            Next
            Me.RadToolBar.Items.FindItemByText("Salva").Enabled = True
            Me.RadToolBar.FindItemByText("Elimina").Attributes.Remove("onclick")
            Me.RadToolBar.FindItemByText("Stampa").Attributes.Remove("onclick")

            Me.PannelloGriglia.Visible = False
            Me.ChiudiButton.Visible = True
            Me.PannelloChiusura.Visible = True


            Select Case Me.Page.Request("Mode")
                Case "update"
                    If parametriPagina.ContainsKey("IdFascicolo") Then
                        Dim idfascicolo As Integer = parametriPagina("IdFascicolo")
                        Me.AggiornaVista(idfascicolo)
                    End If

                Case "read"
                    If parametriPagina.ContainsKey("IdFascicolo") Then
                        Dim idfascicolo As Integer = parametriPagina("IdFascicolo")
                        Me.AggiornaVista(idfascicolo)
                        Me.RadToolBar.Items.FindItemByText("Salva").Enabled = False
                        Me.RadToolBar.FindItemByText("Salva").Attributes.Remove("onclick")
                    End If


                Case "insert"
            End Select



            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    Private Sub AggiornaGriglia()
        Me.Fascicoli = Nothing
        Me.FascicoliGridView.Rebind()
    End Sub

    Private Sub ResettaGridView()
        Me.Titolari = New List(Of ParsecAdmin.TitolareFascicolo)
        Me.Documenti = New List(Of ParsecAdmin.FascicoloDocumento)
        Me.Amministrazioni = New List(Of ParsecAdmin.FascicoloAmministrazionePartecipante)
    End Sub

    Private Sub Delete()
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim fascicolo As New ParsecAdmin.FascicoliRepository
        Try
            fascicolo.Delete(Me.Fascicolo, utenteCorrente)
            Me.ResettaVista()
            Me.AggiornaGriglia()
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try
        fascicolo.Dispose()
    End Sub

    Private Sub Save()
        Dim fascicoli As New ParsecAdmin.FascicoliRepository

        Try

            Dim fascicolo As ParsecAdmin.Fascicolo = Me.AggiornaModello()

            fascicoli.Fascicolo = Me.Fascicolo

            fascicoli.Save(fascicolo)
            Me.AggiornaVista(fascicoli.Fascicolo.Id)

            'Se l'operazione va a buon fine copio i documenti nella relativa cartella.
            'Me.CopiaDocumentoTemporaneo()

        Catch ex As Exception
            If ex.InnerException Is Nothing Then
                Throw New ApplicationException(ex.Message)
            Else
                Throw New ApplicationException(ex.InnerException.Message)
            End If

        Finally
            fascicoli.Dispose()
        End Try
    End Sub

    Private Function AggiornaModello() As ParsecAdmin.Fascicolo
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim fascicolo As New ParsecAdmin.Fascicolo

        fascicolo.DataCreazioneFascicolo = Now
        fascicolo.IdUtente = utenteCorrente.Id
        fascicolo.IdTipologiaFascicolo = CInt(Me.TipologiaFascicoloComboBox.SelectedValue)

        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            fascicolo.CodClassificaAppartenenza = CInt(Me.IdClassificazioneTextBox.Text)
            fascicolo.Classificazione = Me.ClassificazioneTextBox.Text
        End If

        If Not String.IsNullOrEmpty(Me.IdResponsabileTextBox.Text) Then
            fascicolo.IdStrutturaUtenteResponsabile = CInt(Me.IdResponsabileTextBox.Text)
            fascicolo.StrutturaUtenteResponsabile = Trim(Me.ResponsabileTextBox.Text)
        End If

        fascicolo.DataApertura = Me.DataTextBox.SelectedDate
        fascicolo.DataChiusura = Me.DataChiusuraTextBox.SelectedDate
        fascicolo.CodiceFascicoloUtente = Me.CodiceFascicoloUtenteTextBox.Text
        fascicolo.Oggetto = Trim(Me.OggettoTextBox.Text)
        fascicolo.Note = Trim(Me.NoteTextBox.Text)

        If Me.ProcedimentoComboBox.SelectedIndex > 0 Then
            fascicolo.IdProcedimento = Me.ProcedimentoComboBox.SelectedValue
        End If

        fascicolo.NumeroProvvedimentoChiusura = Me.numeroProvvedimentoChiusuraTextbox.Text


        fascicolo.Visibilita = Me.Visibilita
        fascicolo.Documenti = Me.Documenti
        fascicolo.Titolari = Me.Titolari
        fascicolo.Amministrazioni = Me.Amministrazioni


        Return fascicolo

    End Function


    Private Sub CopiaDocumentoTemporaneo()
        For Each documento As ParsecAdmin.FascicoloDocumento In Me.Documenti
            'Se è un documento generico
            If documento.IdDocumento = -1 Then
                'Creo la cartella dei documenti generici (file system) associati al fascicolo corrente.
                Dim pathFascicolo As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti") & documento.path
                If Not IO.Directory.Exists(pathFascicolo) Then
                    IO.Directory.CreateDirectory(pathFascicolo)
                End If
                'Sposto i documenti generici dalla cartella temporanea a quella relativa al fascicolo corrente.
                Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDownload") & Session.SessionID & "_" & documento.NomeDocumentoOriginale
                Dim destPath As String = pathFascicolo & documento.NomeDocumento
                If IO.File.Exists(pathDownload) Then
                    If Not IO.File.Exists(destPath) Then
                        IO.File.Copy(pathDownload, destPath)
                    End If
                    'Elimino il file dalla cartella temporanea.
                    IO.File.Delete(pathDownload)
                End If
            End If
        Next
    End Sub

    Private Sub ResettaVista()

        If Not Me.Fascicolo Is Nothing Then
            If Me.Fascicolo.IdProcedimento.HasValue Then
                Dim item As Telerik.Web.UI.RadComboBoxItem = Me.ProcedimentoComboBox.FindItemByValue(Me.Fascicolo.IdProcedimento)
                If Not item Is Nothing Then
                    item.Remove()
                End If
            End If
        End If


        Me.Fascicolo = Nothing
        'Me.NumeroRegistroTextBox.Text = String.Empty
        Me.TipologiaFascicoloComboBox.SelectedIndex = 0

        Me.CodiceFascicoloUtenteTextBox.Text = String.Empty
        Me.DataTextBox.SelectedDate = Now

        Me.DataChiusuraTextBox.SelectedDate = Nothing
        Me.OggettoTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty


        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.CodiceFascicoloSistemaTextBox.Text = String.Empty
        Me.CodiceClassificazioneTextBox.Text = String.Empty

        Me.IdResponsabileTextBox.Text = String.Empty
        Me.ResponsabileTextBox.Text = String.Empty


        Me.ProcedimentoComboBox.SelectedValue = String.Empty
        Me.ProcedimentoComboBox.Text = String.Empty

        Me.numeroProvvedimentoChiusuraTextbox.Text = String.Empty

        'Me.GeneraRegistroCheckBox.Checked = False

        'Me.BeneficiarioComboBox.SelectedValue = String.Empty
        'Me.BeneficiarioComboBox.Text = String.Empty

        'Me.LegaleComboBox.SelectedValue = String.Empty
        'Me.LegaleComboBox.Text = String.Empty
        Me.ResettaGridView()
        Me.Visibilita = New List(Of ParsecAdmin.VisibilitaDocumento)
        Me.TipoDocumentoComboBox.SelectedIndex = 0


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
        utenteVisibilita.IdEntita = utenteCollegato.Id
        utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
        utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.SEP
        utenteVisibilita.Descrizione = (If(utenteCollegato.Username = Nothing, "", utenteCollegato.Username) + " - " + If(utenteCollegato.Cognome = Nothing, "", utenteCollegato.Cognome) + " " + If(utenteCollegato.Nome = Nothing, "", utenteCollegato.Nome))
        utenteVisibilita.LogIdUtente = utenteCollegato.Id
        utenteVisibilita.LogDataOperazione = Now
        utenteVisibilita.AbilitaCancellaEntita = False
        Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)

    End Sub

    Private Sub VisualizzaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idDocumento As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        'Dim documentiFascicolo As New ParsecAdmin.FascicoloDocumentoRepository
        Dim documento As ParsecAdmin.FascicoloDocumento = Me.Documenti.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        Select Case documento.TipoDocumento
            Case 1 'Documento generico
                Me.DownloadFile(documento)
            Case ParsecAdmin.TipoModulo.PRO
                Me.VisualizzaRegistrazione(documento)
            Case ParsecAdmin.TipoModulo.ATT
                Me.VisualizzaAtto(documento.IdDocumento)
            Case ParsecAdmin.TipoModulo.IOL
                Me.VisualizzaIstanzaPratica(documento.IdDocumento)
        End Select
    End Sub


    Private Sub VisualizzaIstanzaPratica(ByVal idDocumento As String)
        Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
        Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        istanzePraticaOnline.Dispose()
        If Not istanzePratica Is Nothing Then
            Dim parametriPagina As New Hashtable
            Dim pageUrl As String = "~/UI/Amministrazione/pages/search/VisualizzaIstanzaPraticaPage.aspx"
            parametriPagina.Add("Filtro", istanzePratica.Id)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 940, 670, Nothing, False)
        Else
            ParsecUtility.Utility.MessageBox("L'istanza selezionata non esiste!", False)
        End If

    End Sub

    Private Sub VisualizzaRegistrazione(ByVal documento As ParsecAdmin.FascicoloDocumento)
        If Not documento Is Nothing Then
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetQuery.Where(Function(c) c.Id = documento.IdDocumento).FirstOrDefault
            registrazioni.Dispose()
            If Not registrazione Is Nothing Then
                Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
                Dim queryString As New Hashtable
                queryString.Add("filtro", registrazione.Id)
                Dim parametriPagina As New Hashtable
                parametriPagina.Add("Filtro", registrazione.Id)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 510, queryString, False)
            Else
                ParsecUtility.Utility.MessageBox("La registrazione selezionata non esiste!", False)
            End If
        End If
    End Sub

    Private Sub DownloadFile(ByVal documento As ParsecAdmin.FascicoloDocumento)
        If Not documento Is Nothing Then
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If documento.Id < 0 Then
                pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDownload") & documento.NomeFileTemp
            Else
                pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti") & documento.path & documento.NomeDocumento
            End If
            Dim file As New IO.FileInfo(pathDownload)

            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("Il documento selezionato non esiste!", False)
            End If
        End If
    End Sub

    Private Sub VisualizzaAtto(ByVal idDocumento As Integer)
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        documenti.Dispose()
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
        Else
            ParsecUtility.Utility.MessageBox("Il documento selezionato non esiste!", False)
        End If

    End Sub

    Private Sub EliminaDocumento(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim idDocumento As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")


        'Recupero la cache i documenti associati al fascicolo corrente.
        Dim documentiFascicolo As List(Of ParsecAdmin.FascicoloDocumento) = Me.Documenti

        'Elimino il documento selezionato dalla cache
        Dim documento As ParsecAdmin.FascicoloDocumento = documentiFascicolo.Where(Function(c) c.Id = idDocumento).FirstOrDefault

        If (documento.Fase = FaseDocumentoEnumeration.INIZIALE) Then
            ' Me.DataTextBox.SelectedDate = Nothing
        End If

        If (documento.Fase = FaseDocumentoEnumeration.FINALE) Then
            Me.DataChiusuraTextBox.SelectedDate = Nothing
            Me.numeroProvvedimentoChiusuraTextbox.Text = String.Empty
        End If

        documentiFascicolo.Remove(documento)

        'Aggiorno la cache che verrà bindata nel Page_Init
        Me.Documenti = documentiFascicolo


    End Sub

    Private Sub AggiornaVista(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        AggiornaVista(id)
    End Sub

    Private Sub AggiornaVista(ByVal id As Integer)
        Dim fascicoli As New ParsecAdmin.FascicoliRepository
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Me.ResettaVista()
        Me.Fascicolo = fascicoli.GetById(id)

        Me.TipologiaFascicoloComboBox.FindItemByValue(Me.Fascicolo.IdTipologiaFascicolo).Selected = True

        'Me.NumeroRegistroTextBox.Text = IIf(Me.Fascicolo.NumeroRegistro Is Nothing, "", Me.Fascicolo.NumeroRegistro)
        Me.CodiceFascicoloUtenteTextBox.Text = Me.Fascicolo.CodiceFascicoloUtente
        Me.CodiceFascicoloSistemaTextBox.Text = Me.Fascicolo.CodiceFascicoloSistema
        Me.DataTextBox.SelectedDate = Me.Fascicolo.DataApertura

        Me.DataChiusuraTextBox.SelectedDate = Me.Fascicolo.DataChiusura

        Me.OggettoTextBox.Text = Me.Fascicolo.Oggetto
        Me.NoteTextBox.Text = Me.Fascicolo.Note
        Me.numeroProvvedimentoChiusuraTextbox.Text = Me.Fascicolo.NumeroProvvedimentoChiusura


        If Me.Fascicolo.CodClassificaAppartenenza.HasValue Then

            Dim titolario As ParsecAdmin.TitolarioClassificazione = (New ParsecAdmin.TitolarioClassificazioneRepository).GetQuery.Where(Function(c) c.Id = Me.Fascicolo.CodClassificaAppartenenza).FirstOrDefault
            Dim codici = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione(CInt(Me.Fascicolo.CodClassificaAppartenenza.Value.ToString), 1)
            Dim classificazioneCompleta As String = codici & " " & titolario.Descrizione
            Me.IdClassificazioneTextBox.Text = Me.Fascicolo.CodClassificaAppartenenza.Value.ToString
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.CodiceClassificazioneTextBox.Text = codici
        End If


        Me.IdResponsabileTextBox.Text = Me.Fascicolo.IdStrutturaUtenteResponsabile.ToString
        Me.ResponsabileTextBox.Text = Me.Fascicolo.StrutturaUtenteResponsabile

        'Aggiorno l'elenco di utenti associati al fascicolo
        Me.Titolari = (New ParsecAdmin.TitolariFascicoloRepository).getTitolari(Me.Fascicolo.Id)

        'Aggiorno l'elenco di documenti associati al fascicolo
        Me.Documenti = (New ParsecAdmin.FascicoloDocumentoRepository).GetDocumenti(Me.Fascicolo.Id)

        Me.Amministrazioni = Me.Fascicolo.Amministrazioni

        If Me.Fascicolo.IdProcedimento.HasValue Then

            Dim item As RadComboBoxItem = Me.ProcedimentoComboBox.Items.FindItemByValue(Me.Fascicolo.IdProcedimento)
            If Not item Is Nothing Then
                item.Selected = True
            Else
                'SE IL PROCEDIMENTO NON E' VISIBILE (AD ESEMPIO L'UTENTE CORRENTE NON PUO' PIU' CREARE FASCICOLI CON QUESTO TIPO DI PROCEDIMENTO)
                Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
                Dim procedimento = procedimenti.Where(Function(c) c.Id = Me.Fascicolo.IdProcedimento).FirstOrDefault
                procedimenti.Dispose()
                If Not procedimento Is Nothing Then
                    item = New RadComboBoxItem(procedimento.Nome, procedimento.Id.ToString)
                    Me.ProcedimentoComboBox.Items.Add(item)
                    item.Selected = True
                    'Me.ProcedimentoComboBox.SelectedValue = Me.Fascicolo.IdProcedimento
                End If
            End If

        Else
            Me.ProcedimentoComboBox.SelectedValue = ""
        End If

        '********************************************************************************************
        'SCHEDA VISIBILITA (Carico i gruppi e gli utenti associati al protocollo selezionato)
        '********************************************************************************************
        Me.Visibilita = fascicoli.GetVisibilita(Me.Fascicolo.Id)


        fascicoli.Dispose()
    End Sub


    Private Sub CaricaTipologiaDocumento()

        Dim dati As New Dictionary(Of String, String)
        dati.Add(ParsecAdmin.TipoModulo.SEP.ToString, "Documento Generico")
        dati.Add(ParsecAdmin.TipoModulo.PRO.ToString, "Protocollo")
        dati.Add(ParsecAdmin.TipoModulo.ATT.ToString, "Atto Amministrativo")
        '' ''dati.Add(ParsecAdmin.TipoModulo.CNT.ToString, "Contratto")
        'dati.Add("4", "Pratica Edilizia")

        Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})

        Me.TipoDocumentoComboBox.DataSource = ds
        Me.TipoDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipoDocumentoComboBox.DataValueField = "Id"
        Me.TipoDocumentoComboBox.DataBind()
        Me.TipoDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.TipoDocumentoComboBox.SelectedIndex = 0


    End Sub

    Private Sub CaricaFasi()
        Dim dati As New Dictionary(Of String, String)
        dati.Add(ParsecAdmin.FaseDocumentoEnumeration.INIZIALE, "Iniziale")
        dati.Add(ParsecAdmin.FaseDocumentoEnumeration.FINALE, "Finale")
        Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})
        Me.FaseDocumentoComboBox.DataSource = ds
        Me.FaseDocumentoComboBox.DataTextField = "Descrizione"
        Me.FaseDocumentoComboBox.DataValueField = "Id"
        Me.FaseDocumentoComboBox.DataBind()
        Me.FaseDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.FaseDocumentoComboBox.SelectedIndex = 0
    End Sub


    Private Sub CaricaTipologiaFascicoli()
        Dim dati As New List(Of ParsecAdmin.KeyValue)
        dati.Add(New ParsecAdmin.KeyValue With {.Id = TipologiaFascicolo.ProcedimentoAmministrativo, .Descrizione = "Procedimento"})
        dati.Add(New ParsecAdmin.KeyValue With {.Id = TipologiaFascicolo.Affare, .Descrizione = "Affare"})

        Me.TipologiaFascicoloComboBox.DataSource = dati
        Me.TipologiaFascicoloComboBox.DataTextField = "Descrizione"
        Me.TipologiaFascicoloComboBox.DataValueField = "Id"
        Me.TipologiaFascicoloComboBox.DataBind()
        'Commentare in caso di combo con le check
        Me.TipologiaFascicoloComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.TipologiaFascicoloComboBox.SelectedIndex = 0

        'In caso di combo con le check
        ' Me.TipologiaFascicoloComboBox.SelectedIndex = -1

    End Sub

    Private Sub CaricaCataloghiDocumentoFascicolo()

        Dim categorie As New ParsecAdmin.CategorieDocumentoFascicoloRepository


        Me.CategoriaComboBox.DataSource = categorie.Where(Function(c) c.Abilitata = True).ToList
        Me.CategoriaComboBox.DataTextField = "Descrizione"
        Me.CategoriaComboBox.DataValueField = "Id"
        Me.CategoriaComboBox.DataBind()

        Me.CategoriaComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.CategoriaComboBox.SelectedIndex = 0
        categorie.Dispose()


    End Sub




#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub TipologiaFascicoloComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologiaFascicoloComboBox.SelectedIndexChanged
        If e.Value = TipologiaFascicolo.Affare Then
            Me.ProcedimentoComboBox.Text = String.Empty
            Me.ProcedimentoComboBox.SelectedValue = String.Empty
            Me.ProcedimentoComboBox.Enabled = False
        Else
            Me.ProcedimentoComboBox.Enabled = True
        End If
    End Sub

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Me.AdvancedSearch()
    End Sub

    Protected Sub RipristinaFiltroInizialeImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles RipristinaFiltroInizialeImageButton.Click
        ' Me.RipristinaFiltroInizialeImageButton.Enabled = False

        Me.Fascicoli = Nothing
        Me.FascicoliGridView.Rebind()

    End Sub

    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        If Me.Salvato Then
            ParsecUtility.SessionManager.Fascicolo = Me.Fascicolo
        End If
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub

    Protected Sub AggiornaFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaFascicoloImageButton.Click
        Me.ApplicaFiltro()
    End Sub

    'Protected Sub addTitolariCommandButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles addTitolariCommandButton.Click

    '    If (Me.BeneficiarioComboBox.SelectedValue = "") Then
    '        ParsecUtility.Utility.MessageBox("Nessun Titolare selezionato: impossibile proseguire!", False)
    '        Exit Sub
    '    End If

    '    If (Me.Titolari Is Nothing) Then
    '        Me.Titolari = New List(Of ParsecAdmin.TitolareFascicolo)
    '    End If

    '    If (Not Me.Titolari Is Nothing) Then
    '        Dim elementiInLista = Me.Titolari.Where(Function(w) w.idStrutturaEsternaTitolare = Me.BeneficiarioComboBox.SelectedValue)
    '        If (Not elementiInLista Is Nothing And elementiInLista.Count > 0) Then
    '            ParsecUtility.Utility.MessageBox("Titolare già presente: impossibile aggiungerlo alla lista!", False)
    '            Exit Sub
    '        End If
    '    End If

    '    Dim titolareFascicolo As New ParsecAdmin.TitolareFascicolo
    '    Dim titolare = getElementoRubrica(Me.BeneficiarioComboBox.SelectedValue)
    '    titolareFascicolo.idStrutturaEsternaTitolare = titolare.Id
    '    titolareFascicolo.denominazioneTitolare = titolare.Denominazione
    '    'titolareFascicolo.Titolare = titolare
    '    Dim legale As ParsecAdmin.StrutturaEsternaInfo = Nothing
    '    If (Me.LegaleComboBox.SelectedValue <> "") Then
    '        legale = getElementoRubrica(Me.LegaleComboBox.SelectedValue)
    '        titolareFascicolo.idStrutturaRappresentanteLegale = legale.Id
    '        'titolareFascicolo.RappresentanteLegale = legale
    '        titolareFascicolo.denominazioneRappresentanteLegale = legale.Denominazione
    '    End If
    '    titolareFascicolo.idTitolareFascicolo = ricavaIdMinimoAssegnatoListaContraenti(Me.Titolari)

    '    Me.Titolari.Add(titolareFascicolo)

    '    Me.BeneficiarioComboBox.SelectedValue = ""
    '    Me.BeneficiarioComboBox.Text = ""
    '    Me.LegaleComboBox.SelectedValue = ""
    '    Me.LegaleComboBox.Text = ""

    'End Sub

    Protected Sub TrovaDocumentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaDocumentoImageButton.Click

        Me.DescrizioneAllegatoTextBox.Text = String.Empty

        If Me.TipoDocumentoComboBox.SelectedIndex = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare la tipologia di documento!", False)
            Exit Sub
        End If


        Select Case Me.TipoDocumentoComboBox.SelectedIndex
            Case 1 'Documento Generico

                'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/UploadPage.aspx"
                'Dim queryString As New Hashtable
                'queryString.Add("obj", Me.AggiornaDocumentoImageButton.ClientID)
                'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)

                Me.AllegatoUpload.Enabled = True
                Me.NomeFileDocumentoScansionatoTextbox.Text = String.Empty
                Session("DocumentoFascicoloSelezionato") = Nothing

                Dim script As New Text.StringBuilder


                script.AppendLine("<script language='javascript'>")
                script.AppendLine("ShowUploadPanel();hideUpload=false;")


                script.Append("var chk1= document.getElementById('" & Me.FronteRetroCheckBox.ClientID & "');")
                script.Append("var chk2= document.getElementById('" & Me.VisualizzaUICheckBox.ClientID & "');")
                script.Append("var btn= document.getElementById('" & Me.ScansionaImageButton.ClientID & "');")

                script.Append("var opt1= document.getElementById('" & Me.UploadDaFileRadioButton.ClientID & "');")
                script.Append("var opt2= document.getElementById('" & Me.UploadDaScannerRadioButton.ClientID & "');")

                script.Append("chk1.disabled=true;")
                script.Append("chk2.disabled=true;")
                script.Append("btn.disabled=true;")

                script.Append("opt1.checked = true;")
                script.Append("opt2.checked = false;")


                script.AppendLine("</script>")
                ParsecUtility.Utility.RegisterScript(script, False)



            Case 2 'Registrazione Protocollo
                Dim pageUrl As String = "~/UI/Protocollo/pages/search/RicercaRegistrazionePage.aspx"
                Dim queryString As New Hashtable
                queryString.Add("obj", Me.AggiornaDocumentoImageButton.ClientID)
                queryString.Add("modalita", "ricerca")
                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 600, queryString, False)

            Case 3 'Atto Amministrativo
                Dim parametriPagina As New Hashtable
                Dim filtroTipoDoc As New ParsecAtt.FiltroTipologiaDocumento
                filtroTipoDoc.Modellizzabile = True
                filtroTipoDoc.TipoDefinitivo = True
                parametriPagina.Add("filtroTipoDoc", filtroTipoDoc)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/SelezionaAttoPage.aspx"
                Dim queryString As New Hashtable
                queryString.Add("obj", Me.AggiornaDocumentoImageButton.ClientID)
                queryString.Add("modalita", "ricerca")
                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 600, queryString, False)

            Case 4 'Contratto
                Dim parametriPagina As New Hashtable
                Dim filtroTipoDoc As New ParsecAtt.FiltroTipologiaDocumento
                filtroTipoDoc.Modellizzabile = True
                filtroTipoDoc.TipoDefinitivo = True
                parametriPagina.Add("filtroTipoDoc", filtroTipoDoc)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                Dim pageUrl As String = "~/UI/Contratti/pages/search/RicercaContrattiPage.aspx"
                Dim queryString As New Hashtable
                queryString.Add("obj", Me.AggiornaDocumentoImageButton.ClientID)
                queryString.Add("modalita", "ricerca")
                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 400, queryString, False)

            Case 5
        End Select

    End Sub

    Private Sub AggiornaDocumento()
        'Documento Generico

        If Not Session("DocumentoFascicoloSelezionato") Is Nothing Then
            Dim documentoSelezionato As ParsecAdmin.FascicoloDocumento = Session("DocumentoFascicoloSelezionato")

            If Not String.IsNullOrEmpty(Me.FaseDocumentoComboBox.SelectedValue) Then

                For Each item In Me.Documenti
                    If item.Fase = Me.FaseDocumentoComboBox.SelectedValue Then
                        item.Fase = Nothing
                        item.DescrizioneFase = ""
                    End If
                Next
                If Me.FaseDocumentoComboBox.SelectedIndex > 0 Then
                    documentoSelezionato.Fase = Me.FaseDocumentoComboBox.SelectedValue
                    documentoSelezionato.DescrizioneFase = Me.FaseDocumentoComboBox.Text
                End If


            End If

            If Me.CategoriaComboBox.SelectedIndex <> 0 Then
                documentoSelezionato.IdCategoriaDocumentoFascicolo = Me.CategoriaComboBox.SelectedValue
            End If




            documentoSelezionato.DescrizioneTipoDocumento = "GEN"

            'documentoSelezionato.NomeDocumentoOriginale = documentoSelezionato.NomeDocumento

            documentoSelezionato.Id = Me.GetNuovoIdDocumentoTemporaneo
            documentoSelezionato.Definitivo = True


            Dim esiste = Not Me.Documenti.Select(Function(c) c.NomeDocumento.Split("_").Last).Where(Function(c) c = documentoSelezionato.NomeDocumento).FirstOrDefault Is Nothing

            If Not esiste Then
                Me.Documenti.Add(documentoSelezionato)
            End If


            'Aggiungo i documenti selezionati nella cache ed escludo i duplicati
            'Me.Documenti = (Me.Documenti.GroupBy(Function(u) u.NomeDocumentoOriginale).Select(Function(ut) ut.FirstOrDefault)).ToList


            Session("DocumentoFascicoloSelezionato") = Nothing
        End If



        'Registrazione Protocollo
        If Not Session("RicercaRegistrazione_IdRegistrazioneSelezionata") Is Nothing Then

            Dim idProtocollo As Integer = Session("RicercaRegistrazione_IdRegistrazioneSelezionata")

            If (Not Me.Documenti Is Nothing) Then
                Dim elementiInLista = Me.Documenti.Where(Function(w) w.IdDocumento = idProtocollo And w.TipoDocumento = ParsecAdmin.TipoModulo.PRO)
                If (Not elementiInLista Is Nothing And elementiInLista.Count > 0) Then
                    ParsecUtility.Utility.MessageBox("Documento già presente: impossibile aggiungerlo alla lista!", False)
                    Session("RicercaRegistrazione_IdRegistrazioneSelezionata") = Nothing
                    Exit Sub
                End If
            End If


            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim protocolloSelezionato As ParsecPro.Registrazione = registrazioni.GetById(idProtocollo)
            registrazioni.Dispose()



            Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento

            documentoFascicolo.IdDocumento = protocolloSelezionato.Id

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("numeroCifreProtocollo")
            Dim numeroCifre As Integer = 7
            If Not parametro Is Nothing Then
                numeroCifre = CInt(parametro.Valore)
            End If

            documentoFascicolo.NomeDocumento = "Prot. N° " & protocolloSelezionato.NumeroProtocollo.ToString.PadLeft(numeroCifre, "0") & "/" & registrazioni.GetDescrizioneTipoRegistrazione(protocolloSelezionato.TipoRegistrazione).ToUpper.Chars(0) & " del " & String.Format("{0:dd/MM/yyyy}", protocolloSelezionato.DataImmissione)
            documentoFascicolo.NomeDocumentoOriginale = documentoFascicolo.NomeDocumento
            If (Me.FaseDocumentoComboBox.SelectedValue = ParsecAdmin.FaseDocumentoEnumeration.FINALE) Then
                Me.numeroProvvedimentoChiusuraTextbox.Text = documentoFascicolo.NomeDocumento
            End If
            parametri.Dispose()

            documentoFascicolo.TipoDocumento = ParsecAdmin.TipoModulo.PRO  'Protocollo
            documentoFascicolo.DescrizioneTipoDocumento = "PRO"
            documentoFascicolo.DataDocumento = protocolloSelezionato.DataImmissione
            documentoFascicolo.NumeroDocumento = protocolloSelezionato.NumeroProtocollo
            documentoFascicolo.Definitivo = True

            If Me.CategoriaComboBox.SelectedIndex <> 0 Then
                documentoFascicolo.IdCategoriaDocumentoFascicolo = Me.CategoriaComboBox.SelectedValue
            End If



            If Not String.IsNullOrEmpty(Me.FaseDocumentoComboBox.SelectedValue) Then
                For Each item In Me.Documenti
                    If item.Fase = Me.FaseDocumentoComboBox.SelectedValue Then
                        item.Fase = Nothing
                        item.DescrizioneFase = ""
                    End If
                Next
                If Me.FaseDocumentoComboBox.SelectedIndex > 0 Then
                    documentoFascicolo.Fase = Me.FaseDocumentoComboBox.SelectedValue
                    documentoFascicolo.DescrizioneFase = Me.FaseDocumentoComboBox.Text
                End If
            End If



            documentoFascicolo.Id = Me.GetNuovoIdDocumentoTemporaneo

            Me.Documenti.Add(documentoFascicolo)

            Me.Documenti = (Me.Documenti.GroupBy(Function(u) u.NomeDocumento).Select(Function(ut) ut.FirstOrDefault)).ToList


            Session("RicercaRegistrazione_IdRegistrazioneSelezionata") = Nothing
        End If


        'Atto Amministrativo
        If Not Session("SelezionaAtto_IdDocumentoSelezionato") Is Nothing Then

            Dim idDocumento As Integer = Session("SelezionaAtto_IdDocumentoSelezionato")

            If Not Me.Documenti Is Nothing Then
                Dim elementiInLista = Me.Documenti.Where(Function(w) w.IdDocumento = idDocumento And w.TipoDocumento = ParsecAdmin.TipoModulo.ATT)
                If (Not elementiInLista Is Nothing And elementiInLista.Count > 0) Then
                    ParsecUtility.Utility.MessageBox("Documento già presente: impossibile aggiungerlo alla lista!", False)
                    Session("SelezionaAtto_IdDocumentoSelezionato") = Nothing
                    Exit Sub
                End If
            End If

            Dim documentoR As New ParsecAtt.DocumentoRepository
            Dim tipoRegR As New ParsecAtt.TipologieRegistroRepository
            Dim documentoSelezionato As ParsecAtt.Documento = documentoR.GetById(idDocumento)
            documentoR.Dispose()





            Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento

            documentoFascicolo.IdDocumento = documentoSelezionato.Id
            documentoFascicolo.Definitivo = documentoSelezionato.Data.HasValue

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("numeroCifreProtocollo")
            Dim numeroCifre As Integer = 7
            If Not parametro Is Nothing Then
                numeroCifre = CInt(parametro.Valore)
            End If

            documentoFascicolo.NomeDocumento = tipoRegR.GetById(documentoSelezionato.IdTipologiaRegistro).Descrizione & " N° " & documentoSelezionato.ContatoreGenerale.ToString.PadLeft(numeroCifre, "0") & " del " & String.Format("{0:dd/MM/yyyy}", documentoSelezionato.DataDocumento)
            documentoFascicolo.NomeDocumentoOriginale = documentoFascicolo.NomeDocumento

            If (Me.FaseDocumentoComboBox.SelectedValue = ParsecAdmin.FaseDocumentoEnumeration.FINALE) Then
                Me.numeroProvvedimentoChiusuraTextbox.Text = documentoFascicolo.NomeDocumento
            End If
            parametri.Dispose()

            documentoFascicolo.TipoDocumento = TipoModulo.ATT
            documentoFascicolo.DescrizioneTipoDocumento = "ATT"
            documentoFascicolo.DataDocumento = documentoSelezionato.DataDocumento
            documentoFascicolo.NumeroDocumento = documentoSelezionato.ContatoreGenerale

            If Me.CategoriaComboBox.SelectedIndex <> 0 Then
                documentoFascicolo.IdCategoriaDocumentoFascicolo = Me.CategoriaComboBox.SelectedValue
            End If

            If Not String.IsNullOrEmpty(Me.FaseDocumentoComboBox.SelectedValue) Then
                For Each item In Me.Documenti
                    If item.Fase = Me.FaseDocumentoComboBox.SelectedValue Then
                        item.Fase = Nothing
                        item.DescrizioneFase = ""
                    End If
                Next
                If Me.FaseDocumentoComboBox.SelectedIndex > 0 Then
                    documentoFascicolo.Fase = Me.FaseDocumentoComboBox.SelectedValue
                    documentoFascicolo.DescrizioneFase = Me.FaseDocumentoComboBox.Text
                End If

            End If

            documentoFascicolo.Id = Me.GetNuovoIdDocumentoTemporaneo

            Me.Documenti.Add(documentoFascicolo)

            Me.Documenti = (Me.Documenti.GroupBy(Function(u) u.NomeDocumento).Select(Function(ut) ut.FirstOrDefault)).ToList


            Session("SelezionaAtto_IdDocumentoSelezionato") = Nothing
        End If

        Me.CategoriaComboBox.SelectedIndex = 0



        If (Me.FaseDocumentoComboBox.SelectedValue = FaseDocumentoEnumeration.INIZIALE) Then

        ElseIf (Me.FaseDocumentoComboBox.SelectedValue = FaseDocumentoEnumeration.FINALE) Then
            Me.DataChiusuraTextBox.SelectedDate = Now
        End If

        Me.FaseDocumentoComboBox.SelectedValue = ""
        Me.FaseDocumentoComboBox.Text = ""
        Me.TipoDocumentoComboBox.SelectedValue = ""
        Me.TipoDocumentoComboBox.Text = ""
    End Sub

    Protected Sub AggiornaDocumentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaDocumentoImageButton.Click
        Me.AggiornaDocumento()
    End Sub

    Protected Sub EliminaDocumentiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaDocumentiImageButton.Click
        If Me.Documenti.Count > 0 Then
            Me.Documenti.Clear()
        End If
    End Sub

    Protected Sub TrovaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaResponsabileImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/search/Test.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaResponsabileImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola

        parametriPagina.Add("idModulo", ParsecAdmin.ModuliEnumeration.Amministrazione)
        'parametriPagina.Add("idUtente", utenteCollegato.Id)
        parametriPagina.Add("livelliSelezionabili", "400")
        parametriPagina.Add("ultimoLivelloStruttura", "400")
        'parametriPagina.Add("ApplicaAbilitazioni", True)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Protected Sub AggiornaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaResponsabileImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Me.ResponsabileTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdResponsabileTextBox.Text = struttureSelezionate.First.Id.ToString

            Dim id As Integer = struttureSelezionate.First.Id

            Dim strutture As New ParsecAtt.StrutturaViewRepository
            Dim strutturaSelezionata = strutture.Where(Function(c) c.Id = id).FirstOrDefault
            If Not strutturaSelezionata Is Nothing Then

                If strutturaSelezionata.Responsabile Then
                    If strutturaSelezionata.IdUtente.HasValue Then
                        Me.AggiungiUtenteVisibilita(strutturaSelezionata.IdUtente, False)
                    End If
                Else
                    Dim strutturaPadre = strutture.Where(Function(c) c.Id = strutturaSelezionata.IdPadre).FirstOrDefault
                    If Not strutturaPadre Is Nothing Then
                        Dim responsabileSettore = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 400 And c.Responsabile And c.IdPadre = strutturaPadre.Id).FirstOrDefault
                        If Not responsabileSettore Is Nothing Then
                            If responsabileSettore.IdUtente.HasValue Then
                                Me.AggiungiUtenteVisibilita(responsabileSettore.IdUtente, False)
                            End If
                        End If
                    End If
                End If
            End If


            'Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            'If utenteCollegato.CodiceStutturaDefault.HasValue Then
            '    Dim codice As Integer = utenteCollegato.CodiceStutturaDefault

            '    Dim struttura = strutture.GetQuery.Where(Function(c) c.Codice = codice And c.LogStato Is Nothing).FirstOrDefault
            '    Dim idPadre As Integer = 0
            '    If Not struttura Is Nothing Then

            '        If struttura.IdGerarchia = 200 Then  'UFFICIO
            '            'Me.IdUfficioTextBox.Text = struttura.Id
            '            'Me.UfficioTextBox.Text = struttura.Descrizione
            '            '<add key="parsec_LivelloStrutturaRegistroDetermine" value="100" />
            '            'Aggiorno il settore (parametrizzare il livello)
            '            'Dim settore = Me.GetIdStruttura(struttura.Id, 100)
            '            'Me.IdSettoreTextBox.Text = settore.Id.ToString
            '            'Me.SettoreTextBox.Text = settore.Descrizione
            '            'idPadre = struttura.IdPadre
            '        Else
            '            'Me.IdSettoreTextBox.Text = struttura.Id
            '            'Me.SettoreTextBox.Text = struttura.Descrizione
            '            idPadre = struttura.Id
            '        End If

            '        Dim responsabileSettore = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 400 And c.Responsabile And c.IdPadre = idPadre).FirstOrDefault
            '        If Not responsabileSettore Is Nothing Then
            '            If responsabileSettore.IdUtente.HasValue Then
            '                Me.AggiungiUtenteVisibilita(responsabileSettore.IdUtente, False)
            '            End If
            '        End If

            '    End If

            'End If

            strutture.Dispose()
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Protected Sub EliminaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaResponsabileImageButton.Click

        If Not String.IsNullOrEmpty(Me.IdResponsabileTextBox.Text) Then
            Dim strutture As New ParsecAtt.StrutturaViewRepository

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            If utenteCollegato.CodiceStutturaDefault.HasValue Then
                Dim codice As Integer = utenteCollegato.CodiceStutturaDefault

                Dim struttura = strutture.GetQuery.Where(Function(c) c.Codice = codice And c.LogStato Is Nothing).FirstOrDefault

                Dim idPadre As Integer = struttura.Id
                Dim responsabileSettore = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 400 And c.Responsabile And c.IdPadre = idPadre).FirstOrDefault

                If Not responsabileSettore Is Nothing Then
                    If responsabileSettore.IdUtente.HasValue Then
                        Dim entita = Me.Visibilita.Where(Function(c) c.IdEntita = responsabileSettore.IdUtente And c.TipoEntita = ParsecAdmin.TipoEntita.Utente).FirstOrDefault
                        If Not entita Is Nothing Then
                            Me.Visibilita.Remove(entita)
                        End If
                    End If
                End If

            End If

            strutture.Dispose()


        End If


        Me.ResponsabileTextBox.Text = String.Empty
        Me.IdResponsabileTextBox.Text = String.Empty

    End Sub



    Protected Sub TrovaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneImageButton.Click

        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "2,3")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    End Sub

    Protected Sub AggiornaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim codici = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione(idClassificazione, 1)
            Dim classificazioneCompleta As String = codici & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneTextBox.Text = idClassificazione.ToString
            Me.CodiceClassificazioneTextBox.Text = codici
            Session("ClassificazioniSelezionate") = Nothing
            If Me.DataTextBox.SelectedDate.HasValue Then
                Me.CodiceFascicoloSistemaTextBox.Text = Me.DataTextBox.SelectedDate.Value.Year.ToString & "-" & codici & "/"
            Else
                Me.CodiceFascicoloSistemaTextBox.Text = "{anno}-" & codici & "/"
            End If


        End If
    End Sub

    Protected Sub EliminaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaClassificazioneImageButton.Click
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.CodiceFascicoloSistemaTextBox.Text = String.Empty
        Me.CodiceClassificazioneTextBox.Text = String.Empty
    End Sub

    'Protected Sub TrovaDocumentoStoricoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaDocumentoStoricoImageButton.Click
    '    If Not Me.Fascicolo Is Nothing Then
    '        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaStoricoDocumentiFascicoloPage.aspx"
    '        Dim queryString As New Hashtable
    '        queryString.Add("id", Me.Fascicolo.Id)
    '        ParsecUtility.Utility.ShowPopup(pageUrl, 600, 400, queryString, False)
    '    Else
    '        ParsecUtility.Utility.MessageBox("E' necessario selezionare un fascicolo!", False)
    '    End If
    'End Sub

    Private Const ItemsPerRequest As Integer = 10

    <WebMethod()>
    Public Shared Function GetElementiRubrica(ByVal context As RadComboBoxContext) As RadComboBoxData
        Dim rubrica As New ParsecAdmin.RubricaRepository
        Dim data = rubrica.GetQuery.Where(Function(c) c.Denominazione.StartsWith(context.Text) And c.Denominazione <> "" And c.LogStato Is Nothing And c.InRubrica = True).OrderBy(Function(c) c.Denominazione).ToList
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
        Dim strutturaEsterna = rubrica.GetQuery.Where(Function(c) c.Id = idElemento).FirstOrDefault
        Return strutturaEsterna
    End Function

    'Protected Sub TrovaBeneficiarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaBeneficiarioImageButton.Click
    '    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
    '    Dim queryString As New Hashtable
    '    queryString.Add("obj", Me.AggiornaBeneficiarioImageButton.ClientID)
    '    queryString.Add("mode", "search")

    '    Dim parametriPagina As New Hashtable

    '    'parametriPagina.Add("FiltroTipologiaSoggetto", 1)
    '    ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    '    If String.IsNullOrEmpty(Me.BeneficiarioComboBox.SelectedValue) Then
    '        parametriPagina.Add("Filtro", Me.BeneficiarioComboBox.Text)
    '        If Not String.IsNullOrEmpty(Me.BeneficiarioComboBox.Text) Then
    '            Dim rubrica As New ParsecAdmin.RubricaRepository
    '            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) (c.Denominazione.Contains(Me.BeneficiarioComboBox.Text)) And c.LogStato Is Nothing).ToList
    '            If struttureEsterne.Count = 1 Then
    '                Me.BeneficiarioComboBox.Text = struttureEsterne(0).Denominazione
    '                Me.BeneficiarioComboBox.SelectedValue = struttureEsterne(0).Id
    '                ParsecUtility.SessionManager.ParametriPagina = Nothing
    '            Else
    '                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '            End If
    '            rubrica.Dispose()
    '        Else
    '            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '        End If
    '    Else
    '        Dim rubrica As New ParsecAdmin.RubricaRepository
    '        Dim struttureEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.BeneficiarioComboBox.SelectedValue)).FirstOrDefault
    '        rubrica.Dispose()
    '        If Not struttureEsterna Is Nothing Then
    '            parametriPagina.Add("Filtro", struttureEsterna.Denominazione)
    '        End If
    '        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '    End If


    'End Sub

    'Protected Sub AggiornaBeneficiarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBeneficiarioImageButton.Click
    '    If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
    '        Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
    '        'Dim strutturaEsterne As ParsecContratti.StrutturaEsternaInfo = (New ParsecContratti.ViewRubricaRepository().GetQuery.Where(Function(o) o.iseCodice = strutturaEsterna.Codice And o.iseLog_Stato Is Nothing)).FirstOrDefault
    '        Me.BeneficiarioComboBox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "") 'strutturaEsterna.Denominazione
    '        Me.BeneficiarioComboBox.SelectedValue = strutturaEsterna.Id
    '        ParsecUtility.SessionManager.Rubrica = Nothing
    '    End If
    'End Sub

    'Protected Sub TrovaLegaleImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaLegaleImageButton.Click
    '    'Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaRubricaPage.aspx"
    '    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
    '    Dim queryString As New Hashtable
    '    queryString.Add("obj", Me.AggiornaLegaleImageButton.ClientID)
    '    queryString.Add("mode", "search")

    '    Dim parametriPagina As New Hashtable

    '    'parametriPagina.Add("FiltroTipologiaSoggetto", 1)
    '    ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    '    If String.IsNullOrEmpty(Me.LegaleComboBox.SelectedValue) Then
    '        parametriPagina.Add("Filtro", Me.LegaleComboBox.Text)
    '        If Not String.IsNullOrEmpty(Me.LegaleComboBox.Text) Then
    '            Dim rubrica As New ParsecAdmin.RubricaRepository
    '            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) (c.Denominazione.Contains(Me.LegaleComboBox.Text)) And c.LogStato Is Nothing).ToList
    '            If struttureEsterne.Count = 1 Then
    '                Me.LegaleComboBox.Text = struttureEsterne(0).Denominazione
    '                Me.LegaleComboBox.SelectedValue = struttureEsterne(0).Id
    '                ParsecUtility.SessionManager.ParametriPagina = Nothing
    '            Else
    '                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '            End If
    '            rubrica.Dispose()
    '        Else
    '            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '        End If
    '    Else
    '        Dim rubrica As New ParsecAdmin.RubricaRepository
    '        Dim struttureEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.LegaleComboBox.SelectedValue)).FirstOrDefault
    '        rubrica.Dispose()
    '        If Not struttureEsterna Is Nothing Then
    '            parametriPagina.Add("Filtro", struttureEsterna.Denominazione)
    '        End If
    '        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)

    '    End If

    'End Sub

    'Protected Sub AggiornaLegaleImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaLegaleImageButton.Click
    '    If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
    '        Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
    '        'Dim strutturaEsterne As ParsecContratti.StrutturaEsternaInfo = (New ParsecContratti.ViewRubricaRepository().GetQuery.Where(Function(o) o.iseCodice = strutturaEsterna.Codice And o.iseLog_Stato Is Nothing)).FirstOrDefault
    '        Me.LegaleComboBox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "") 'strutturaEsterna.Denominazione
    '        Me.LegaleComboBox.SelectedValue = strutturaEsterna.Id
    '        ParsecUtility.SessionManager.Rubrica = Nothing
    '    End If
    'End Sub

#End Region

#Region "GESTIONE VISIBILITA'"

    Protected Sub TrovaGruppoVisibilitaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaGruppoVisibilitaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaGruppoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaGruppoVisibilitaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaGruppoVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaGruppoVisibilitaImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        If Not Session("GruppiSelezionati") Is Nothing Then
            Dim gruppiSelezionati As SortedList(Of Integer, String) = Session("GruppiSelezionati")
            Dim idGruppo As Integer = 0
            For Each gruppoSelezionato In gruppiSelezionati
                idGruppo = gruppoSelezionato.Key
                Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = idGruppo And c.TipoEntita = ParsecAdmin.TipoEntita.Gruppo).FirstOrDefault Is Nothing
                If Not esiste Then
                    Dim gruppoVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetGruppoVisibilita(gruppoSelezionato)
                    Me.AggiungiGruppoUtenteVisibilita(gruppoVisibilita)
                End If
            Next
            Session("GruppiSelezionati") = Nothing
        End If
    End Sub

    Protected Sub TrovaUtenteVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteVisibilitaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteVisibilitaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaUtenteVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteVisibilitaImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim idUtente As Integer = 0
            For Each utenteSelezionato In utentiSelezionati
                idUtente = utenteSelezionato.Key
                Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = idUtente And c.TipoEntita = ParsecAdmin.TipoEntita.Utente).FirstOrDefault Is Nothing
                If Not esiste Then
                    Dim utenti As New ParsecAdmin.UserRepository
                    Dim utente As ParsecAdmin.Utente = utenti.GetUserById(utenteSelezionato.Key).FirstOrDefault
                    If Not utente Is Nothing Then
                        Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetUtenteVisibilita(utente, True)
                        Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)
                    End If
                End If
            Next
            Session("SelectedUsers") = Nothing
        End If
    End Sub


    Protected Sub VisibilitaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles VisibilitaGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                EliminaGruppoUtente(e)
        End Select
    End Sub

    Protected Sub VisibilitaGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles VisibilitaGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim gruppoUtente As ParsecAdmin.VisibilitaDocumento = CType(e.Item.DataItem, ParsecAdmin.VisibilitaDocumento)
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                Dim message As String = "Eliminare l'elemento selezionato?"
                If Not gruppoUtente.AbilitaCancellaEntita Then
                    message = "L'elemento selezionato non può essere cancellato!"
                    btn.Attributes.Add("onclick", "alert(""" & message & """);return false;")
                Else
                    btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
                End If
            End If
        End If
    End Sub

    Private Sub EliminaGruppoUtente(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim idEntita As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("IdEntita"))
        Dim tipoEntita As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("TipoEntita"))
        Dim gruppoUtente As ParsecAdmin.VisibilitaDocumento = Me.Visibilita.Where(Function(c) c.IdEntita = idEntita And c.TipoEntita = tipoEntita).FirstOrDefault
        If Not gruppoUtente Is Nothing Then
            Me.Visibilita.Remove(gruppoUtente)
        End If
    End Sub

    Private Sub AggiungiGruppoUtenteVisibilita(gruppoUtente As ParsecAdmin.VisibilitaDocumento)
        Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = gruppoUtente.IdEntita And c.TipoEntita = gruppoUtente.TipoEntita).FirstOrDefault Is Nothing
        If Not esiste Then
            Me.Visibilita.Add(gruppoUtente)
        End If
    End Sub

    Private Sub AggiungiGruppoVisibilitaTuttiUtenti()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim gruppi As New ParsecAdmin.GruppoRepository
        Dim gruppoTuttiUtenti As ParsecAdmin.Gruppo = gruppi.GetQuery.Where(Function(c) c.Id = 1).FirstOrDefault
        If Not gruppoTuttiUtenti Is Nothing Then
            Dim gruppoVisibilita As New ParsecAdmin.VisibilitaDocumento
            gruppoVisibilita.IdEntita = gruppoTuttiUtenti.Id
            gruppoVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
            gruppoVisibilita.IdModulo = ParsecAdmin.TipoModulo.SEP
            gruppoVisibilita.Descrizione = gruppoTuttiUtenti.Descrizione
            gruppoVisibilita.LogIdUtente = utenteCollegato.Id
            gruppoVisibilita.LogDataOperazione = Now
            gruppoVisibilita.AbilitaCancellaEntita = False
            Me.AggiungiGruppoUtenteVisibilita(gruppoVisibilita)
        End If
    End Sub

    Private Sub AggiungiUtenteVisibilita(ByVal idUtente As Integer, ByVal abilitaCancellazione As Boolean)
        Dim utenti As New ParsecAdmin.UserRepository
        Dim utente As ParsecAdmin.Utente = utenti.GetQuery.Where(Function(c) c.Id = idUtente).FirstOrDefault
        If Not utente Is Nothing Then
            Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetUtenteVisibilita(utente, abilitaCancellazione)
            Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)
        End If
        utenti.Dispose()
    End Sub

    Private Function GetUtenteVisibilita(ByVal utente As ParsecAdmin.Utente, ByVal abilitaCancellazione As Boolean) As ParsecAdmin.VisibilitaDocumento
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
        utenteVisibilita.AbilitaCancellaEntita = abilitaCancellazione
        utenteVisibilita.Descrizione = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
        utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
        utenteVisibilita.IdEntita = utente.Id
        utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.SEP
        utenteVisibilita.LogIdUtente = utenteCollegato.Id
        utenteVisibilita.LogDataOperazione = Now
        Return utenteVisibilita
    End Function


    Private Function GetGruppoVisibilita(ByVal gruppoSelezionato As KeyValuePair(Of Integer, String)) As ParsecAdmin.VisibilitaDocumento
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim gruppoVisibilita As New ParsecAdmin.VisibilitaDocumento
        gruppoVisibilita.IdEntita = gruppoSelezionato.Key
        gruppoVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
        gruppoVisibilita.IdModulo = ParsecAdmin.TipoModulo.SEP
        gruppoVisibilita.Descrizione = gruppoSelezionato.Value
        gruppoVisibilita.LogIdUtente = utenteCollegato.Id
        gruppoVisibilita.LogDataOperazione = Now
        gruppoVisibilita.AbilitaCancellaEntita = True
        Return gruppoVisibilita
    End Function

#End Region

#Region "STAMPA PROVVEDIMENTI"




    Private Sub CaricaTipologiaFascicoliFiltro()

        Dim dati As New List(Of ParsecAdmin.KeyValue)
        dati.Add(New ParsecAdmin.KeyValue With {.Id = TipologiaFascicolo.ProcedimentoAmministrativo, .Descrizione = "Procedimento"})
        dati.Add(New ParsecAdmin.KeyValue With {.Id = TipologiaFascicolo.Affare, .Descrizione = "Affare"})
        Me.TipologiaFascicoloFiltroComboBox.DataSource = dati
        Me.TipologiaFascicoloFiltroComboBox.DataTextField = "Descrizione"
        Me.TipologiaFascicoloFiltroComboBox.DataValueField = "Id"
        Me.TipologiaFascicoloFiltroComboBox.DataBind()
        Me.TipologiaFascicoloFiltroComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.TipologiaFascicoloFiltroComboBox.SelectedIndex = 0
    End Sub



    Public Sub CaricaElencoProcedimentiStampa()
        'riempio la lista della natura contratto
        Dim procedimentoRepository As New ParsecAdmin.ProcedimentoRepository
        Me.ProcedimentoFiltroComboBox.DataSource = procedimentoRepository.GetView(Nothing)
        Me.ProcedimentoFiltroComboBox.DataTextField = "Nome"
        Me.ProcedimentoFiltroComboBox.DataValueField = "id"
        Me.ProcedimentoFiltroComboBox.DataBind()
        Me.ProcedimentoFiltroComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.ProcedimentoFiltroComboBox.SelectedIndex = 0
        procedimentoRepository.Dispose()
    End Sub

    Public Sub ResettaFiltroStampa()
        Me.ContatoreGeneraleInizioTextBox.Text = String.Empty
        Me.ContatoreGeneraleFineTextBox.Text = String.Empty
        Me.AnnoInizioFiltroTextBox.Text = Now.Year
        Me.AnnoFineFiltroTextBox.Text = Now.Year
        'Me.BeneficiarioFiltroComboBox.SelectedValue = ""
        Me.ProcedimentoFiltroComboBox.SelectedIndex = 0
        Me.TipologiaFascicoloFiltroComboBox.SelectedIndex = 0
    End Sub

    Private Sub Esporta(ByVal registri As List(Of ParsecAdmin.RigaRegistro))
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("RegistroProcedimento_" & StrConv(Me.ProcedimentoFiltroComboBox.SelectedItem.Text, VbStrConv.ProperCase).Replace(" ", "") & "_UT{0}_AL_{1}.csv", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As String = String.Empty



        line &= "DATA DOMANDA;DATA RILASCIO;NUMERO PROVVEDIMENTO;SOGGETTO FISICO/GIURIDICO"
        swExport.WriteLine(line)
        line = ""

        Dim tit As String = String.Empty

        For Each p As ParsecAdmin.RigaRegistro In registri

            tit = String.Empty

            If p.ListaTitolari.Count > 0 Then
                tit = If(Not String.IsNullOrEmpty(p.ListaTitolari(0)), p.ListaTitolari(0).Substring(0, p.ListaTitolari(0).Length - 2).Replace(";", "").Trim, "")
            End If

            line &= If(p.DataDomanda.HasValue, p.DataDomanda.Value.ToShortDateString, "") & ";" &
                 If(p.DataRilascio.HasValue, p.DataRilascio.Value.ToShortDateString, "") & ";" &
                 If(Not String.IsNullOrEmpty(p.NumeroProvvedimento), p.NumeroProvvedimento.ToString, "") & ";" &
                 tit

            swExport.WriteLine(line)
            line = ""

            For i As Integer = 1 To p.ListaTitolari.Count - 1
                line &= If(p.DataDomanda.HasValue, "", "") & ";" &
                 If(p.DataRilascio.HasValue, "", "") & ";" &
                 If(Not String.IsNullOrEmpty(p.NumeroProvvedimento), "", "") & ";" &
                 If(Not String.IsNullOrEmpty(p.ListaTitolari(i)), p.ListaTitolari(i).Substring(0, p.ListaTitolari(i).Length - 2).Replace(";", "").Trim, "")
            Next

            swExport.WriteLine(line)
            line = ""

            line &= If(p.DataDomanda.HasValue, "", "") & ";" &
                If(p.DataRilascio.HasValue, "", "") & ";" &
                If(Not String.IsNullOrEmpty(p.NumeroProvvedimento), "", "") & ";" &
                If(Not String.IsNullOrEmpty(p.NumeroProvvedimento), "", "")

            swExport.WriteLine(line)
            line = ""
        Next
        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)


    End Sub

    Private Sub Print(ByVal registri As List(Of ParsecAdmin.RigaRegistro), ByVal tipoFascicolo As ParsecAdmin.TipologiaFascicolo)
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaRegistri")
        parametriStampa.Add("DatiStampa", registri)
        If tipoFascicolo = TipologiaFascicolo.ProcedimentoAmministrativo Then
            parametriStampa.Add("Titolo", "Registro Per Procedimento - " & Me.ProcedimentoFiltroComboBox.SelectedItem.Text)
        Else
            parametriStampa.Add("Titolo", "Registro Per Affare ")
        End If

        Session("parametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Function GetFiltroStampa() As ParsecAdmin.FascicoloFiltro
        Dim filtro As New ParsecAdmin.FascicoloFiltro

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        filtro.IdUtenteCollegato = utenteCollegato.Id
        If Not String.IsNullOrEmpty(Me.ContatoreGeneraleInizioTextBox.Text) Then
            filtro.NumeroRegistroInizio = CInt(Me.ContatoreGeneraleInizioTextBox.Text)
        End If
        If Not String.IsNullOrEmpty(Me.ContatoreGeneraleFineTextBox.Text) Then
            filtro.NumeroRegistroFine = CInt(Me.ContatoreGeneraleFineTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.AnnoInizioFiltroTextBox.Text) Then
            filtro.AnnoInizio = CInt(Me.AnnoInizioFiltroTextBox.Text)
        End If
        If Not String.IsNullOrEmpty(Me.AnnoFineFiltroTextBox.Text) Then
            filtro.AnnoFine = CInt(Me.AnnoFineFiltroTextBox.Text)
        End If

        'If Me.BeneficiarioFiltroComboBox.SelectedIndex > 0 Then
        '    filtro.CodiceTitolare = CInt(Me.BeneficiarioFiltroComboBox.SelectedValue)
        'End If

        If Me.ProcedimentoFiltroComboBox.SelectedIndex > 0 Then
            filtro.IdProcedimento = CInt(Me.ProcedimentoFiltroComboBox.SelectedValue)
        End If

        If Me.TipologiaFascicoloFiltroComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaFascicolo = CInt(Me.TipologiaFascicoloFiltroComboBox.SelectedValue)
        End If

        Return filtro

    End Function

    Protected Sub StampaImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaImageButton.Click

        Dim tipoFascicolo = CType(CInt(Me.TipologiaFascicoloFiltroComboBox.SelectedValue), ParsecAdmin.TipologiaFascicolo)

        Select Case tipoFascicolo
            Case TipologiaFascicolo.Nessuno
                ParsecUtility.Utility.MessageBox("E' necessario specificare la tipologia di fascicolo", False)
                Exit Sub
            Case TipologiaFascicolo.ProcedimentoAmministrativo
                If Me.ProcedimentoFiltroComboBox.SelectedIndex <= 0 Then
                    ParsecUtility.Utility.MessageBox("E' necessario specificare la tipologia di procedimento", False)
                    Exit Sub
                End If
            Case TipologiaFascicolo.Affare
        End Select

        Dim filtro As ParsecAdmin.FascicoloFiltro = Me.GetFiltroStampa
        Dim fascicoli As New ParsecAdmin.FascicoliRepository
        Dim view As List(Of ParsecAdmin.RigaRegistro) = fascicoli.getRigheRegistro(filtro)

        If view.Count > 0 Then
            Me.Print(view, tipoFascicolo)
        Else
            ParsecUtility.Utility.MessageBox("Nessun fascicolo trovato con i criteri di filtro impostati!", False)
        End If

        fascicoli.Dispose()
    End Sub

    Protected Sub EsportaImageButton_Click(sender As Object, e As System.EventArgs) Handles EsportaImageButton.Click

        Dim tipoFascicolo = CType(CInt(Me.TipologiaFascicoloFiltroComboBox.SelectedValue), ParsecAdmin.TipologiaFascicolo)

        Select Case tipoFascicolo
            Case TipologiaFascicolo.Nessuno
                ParsecUtility.Utility.MessageBox("E' necessario specificare la tipologia di fascicolo", False)
                Exit Sub
            Case TipologiaFascicolo.ProcedimentoAmministrativo
                If Me.ProcedimentoFiltroComboBox.SelectedIndex <= 0 Then
                    ParsecUtility.Utility.MessageBox("E' necessario specificare la tipologia di procedimento", False)
                    Exit Sub
                End If
            Case TipologiaFascicolo.Affare
        End Select

        Dim filtro As ParsecAdmin.FascicoloFiltro = Me.GetFiltroStampa

        Dim fascicoli As New ParsecAdmin.FascicoliRepository
        Dim view As List(Of ParsecAdmin.RigaRegistro) = fascicoli.getRigheRegistro(filtro)
        If view.Count > 0 Then
            Me.Esporta(view)
        Else
            ParsecUtility.Utility.MessageBox("Nessun fascicolo trovato con i criteri di filtro impostati!", False)
        End If

        fascicoli.Dispose()
    End Sub

    Protected Sub ChiudiStampaImageButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiStampaImageButton.Click
        Me.ResettaFiltroStampa()
    End Sub

#End Region


    Protected Sub ConfermaUploadButton_Click(sender As Object, e As System.EventArgs) Handles ConfermaUploadButton.Click
        If Me.AllegatoUpload.UploadedFiles.Count > 0 Then

            Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles(0)
            If file.FileName <> "" Then
                Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento
                Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDownload") & Session.SessionID & "_" & file.FileName
                file.SaveAs(pathDownload)
                documentoFascicolo.IdDocumento = -1


                documentoFascicolo.NomeDocumento = file.FileName

                'SE L'ALLEGATO DEL FASCICOLO E' DI TIPO GENERICO IL CAMPO NOMEDOCUMENTOORIGINALE RAPPRESENTA LA SUA DESCRIZIONE O IL NOME DEL FILE
                If Not String.IsNullOrEmpty(Me.DescrizioneAllegatoTextBox.Text.Trim) Then
                    documentoFascicolo.NomeDocumentoOriginale = Me.DescrizioneAllegatoTextBox.Text
                Else
                    documentoFascicolo.NomeDocumentoOriginale = file.FileName
                End If



                documentoFascicolo.TipoDocumento = 1  'Documento Generico
                documentoFascicolo.DescrizioneTipoDocumento = Me.GetImagePath(file.FileName)
                documentoFascicolo.DataDocumento = Now
                documentoFascicolo.NomeFileTemp = Session.SessionID & "_" & file.FileName
                Session("DocumentoFascicoloSelezionato") = documentoFascicolo

                Me.AggiornaDocumento()
            End If
        Else
            If Not Session("DocumentoFascicoloSelezionato") Is Nothing Then
                Dim documentoFascicolo As ParsecAdmin.FascicoloDocumento = Session("DocumentoFascicoloSelezionato")

                If Not String.IsNullOrEmpty(Me.DescrizioneAllegatoTextBox.Text.Trim) Then
                    documentoFascicolo.NomeDocumentoOriginale = Me.DescrizioneAllegatoTextBox.Text
                Else
                    documentoFascicolo.NomeDocumentoOriginale = documentoFascicolo.NomeDocumento
                End If

                Session("DocumentoFascicoloSelezionato") = documentoFascicolo

                Me.AggiornaDocumento()
            End If
        End If

        ' ParsecUtility.Utility.ClosePopup(True)
    End Sub

    Private Function GetImagePath(ByVal filename As String) As String
        Dim estensione As String = IO.Path.GetExtension(filename)
        Select Case estensione
            Case "pdf"
                Return "~/sep/images/ext/pdf.gif"
            Case "xls"
                Return "~/sep/images/ext/xls.png"
            Case "doc"
                Return "~/sep/images/ext/doc.jpg"
            Case "ppt"
                Return "~/sep/images/ext/ppoint.png"
            Case "txt"
                Return "~/sep/images/ext/txt.jpg"
            Case Else
                Return "~/sep/images/ext/docum.png"
        End Select
    End Function


    Private Function GetClientScript(ByVal daScanner As Boolean) As String
        Dim script As New System.Text.StringBuilder



        script.Append("var allegatoUpload= $find('" & Me.AllegatoUpload.ClientID & "');")
        script.Append("var chk1= document.getElementById('" & Me.FronteRetroCheckBox.ClientID & "');")
        script.Append("var chk2= document.getElementById('" & Me.VisualizzaUICheckBox.ClientID & "');")
        script.Append("var btn= document.getElementById('" & Me.ScansionaImageButton.ClientID & "');")
        script.Append("var btn2= document.getElementById('" & Me.EliminaDocumentoScansionatoImageButton.ClientID & "');")

        script.Append("var txt= document.getElementById('" & Me.NomeFileDocumentoScansionatoTextbox.ClientID & "');")


        If daScanner Then

            script.Append("allegatoUpload.set_enabled(false);")
            script.Append("chk1.disabled=false;")
            script.Append("chk2.disabled=false;")
            script.Append("btn.disabled=false;")


        Else

            script.Append("allegatoUpload.set_enabled(true);")
            script.Append("chk1.disabled=true;")
            script.Append("chk2.disabled=true;")
            script.Append("btn.disabled=true;")
            script.Append("txt.innerText='';")
            script.Append("PageMethods.EliminaDocumentoScansionato();")
            'script.Append("btn2.click();")
            script.Append("btn2.disabled=true;")
        End If




        Return script.ToString
    End Function

    <WebMethod()>
    Public Shared Sub EliminaDocumentoScansionato()
        System.Web.HttpContext.Current.Session("DocumentoFascicoloSelezionato") = Nothing
    End Sub


    Protected Sub ScansionaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaImageButton.Click
        Me.AllegatoUpload.Enabled = False
        Me.AvviaScansione()
    End Sub


#Region "GESTIONE SCANSIONE"

    Private Sub RegistraScansione()
        Dim script As String = ParsecAdmin.ScannerParameters.RegistraScansione
        Me.MainPage.RegisterComponent(script)
    End Sub



    Private Sub AvviaScansione()
        Dim scanner As New ParsecAdmin.ScannerParameters
        Dim data As String = scanner.CreaDataSource(New ParsecAdmin.DatiCredenziali, New ParsecAdmin.DatiScansione With {.Duplex = Me.FronteRetroCheckBox.Checked, .UseUi = Me.VisualizzaUICheckBox.Checked})

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiScansione(data, Me.ScanUploadButton.ClientID, Me.infoScansioneHidden.ClientID, True, False)
        Else
            'UTILIZZO IL SOCKET  
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, AddressOf Me.NotificaInfoScansione, AddressOf Me.NotificaScansione)
        End If
    End Sub


    Private Sub NotificaInfoScansione(ByVal data As String)
        Me.infoScansioneHidden.Value = data
    End Sub



    Private Sub NotificaScansione()
        If Not String.IsNullOrEmpty(Me.infoScansioneHidden.Value) Then
            Dim ds As New DataSet


            Dim ms As IO.MemoryStream = Nothing

            Dim search As String = "DATIBASE64:"
            Dim pos = Me.infoScansioneHidden.Value.IndexOf(search)
            Dim infoScansione As String = String.Empty
            Try
                If pos <> -1 Then
                    infoScansione = Me.infoScansioneHidden.Value.Substring(pos + search.Length, Me.infoScansioneHidden.Value.Length - pos - search.Length)
                Else
                    infoScansione = Me.infoScansioneHidden.Value
                End If

                'LO SCANNER OLIVETTI IN CASO DI ERRORE SCRIVE NELL'OUTPUT DEL COMPONENTE
                If String.IsNullOrEmpty(infoScansione.Trim) Then
                    Exit Sub
                End If

                ms = New IO.MemoryStream(System.Convert.FromBase64String(infoScansione))
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Si è verificato un errore durante la scansione:" & vbCrLf & infoScansione, False)
                Exit Sub
            End Try

            ds.ReadXml(ms)

            Dim nomeFileDigitalizzato As String = IO.Path.GetFileName(CStr(ds.Tables(0).Rows(0)("NomeFile")))

            Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento
            Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDownload") & nomeFileDigitalizzato

            Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & nomeFileDigitalizzato

            IO.File.Copy(pathRootTemp, pathDownload)

            documentoFascicolo.IdDocumento = -1
            documentoFascicolo.NomeDocumento = nomeFileDigitalizzato
            documentoFascicolo.TipoDocumento = 1  'Documento Generico
            documentoFascicolo.DescrizioneTipoDocumento = Me.GetImagePath(nomeFileDigitalizzato)
            documentoFascicolo.DataDocumento = Now
            documentoFascicolo.NomeFileTemp = nomeFileDigitalizzato

            Session("DocumentoFascicoloSelezionato") = documentoFascicolo
            Me.NomeFileDocumentoScansionatoTextbox.Text = nomeFileDigitalizzato

        End If

    End Sub

    Protected Sub ScanUploadButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadButton.Click
        NotificaScansione()
    End Sub

    Protected Sub EliminaDocumentoScansionatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaDocumentoScansionatoImageButton.Click
        Me.NomeFileDocumentoScansionatoTextbox.Text = String.Empty
        Session("DocumentoFascicoloSelezionato") = Nothing
    End Sub


#End Region



#Region "GESTIONE AMMINISTRAZIONI PARTECIPANTI"


    Private Sub EliminaAmministrazione(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim id As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"))
        Dim amministrazione = Me.Amministrazioni.Where(Function(c) c.Id = id)
        If amministrazione.Any Then
            Me.Amministrazioni.Remove(amministrazione.FirstOrDefault)
        End If
    End Sub

    Protected Sub AmministrazioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AmministrazioniGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                EliminaAmministrazione(e)
        End Select
    End Sub

    Protected Sub TrovaAmministrazioneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaAmministrazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaAmministrazioneIpaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaAmministrazioneImageButton.ClientID)
        queryString.Add("cat", Me.CategorieAmministrazioneComboBox.SelectedValue)
        queryString.Add("key", Me.ChiaveRicercaTextBox.Text)
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 600, queryString, False)
    End Sub

    Protected Sub AggiornaAmministrazioneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaAmministrazioneImageButton.Click
        If Not ParsecUtility.SessionManager.AmministrazioneIpa Is Nothing Then
            Dim amministrazioneIpa = CType(ParsecUtility.SessionManager.AmministrazioneIpa, ParsecPro.AmministrazioneIpa)
            Dim amm As New ParsecAdmin.FascicoloAmministrazionePartecipante
            amm.Descrizione = amministrazioneIpa.Nome
            amm.Id = Me.GetNuovoIdAmministrazioneTemporaneo
            amm.CodiceIpa = amministrazioneIpa.Codice

            Dim esiste = Me.Amministrazioni.Where(Function(c) c.Descrizione = amministrazioneIpa.Nome).Any
            If Not esiste Then
                Me.Amministrazioni.Add(amm)
            Else
                ParsecUtility.Utility.MessageBox("Amministrazione già presente!", False)
            End If
            ParsecUtility.SessionManager.AmministrazioneIpa = Nothing
            Me.ChiaveRicercaTextBox.Text = String.Empty
            Me.CategorieAmministrazioneComboBox.Text = String.Empty
        End If
    End Sub

    Private Function GetNuovoIdAmministrazioneTemporaneo() As Integer
        Dim nuovoId As Integer = -1
        If Me.Amministrazioni.Count > 0 Then
            Dim allId = Me.Amministrazioni.Min(Function(c) c.Id) - 1
            If allId < 0 Then
                nuovoId = allId
            End If
        End If
        Return nuovoId
    End Function

    <WebMethod()> _
    Public Shared Function GetCategorieAmministrazioni(ByVal context As RadComboBoxContext) As RadComboBoxData
        Dim categorie As New ParsecPro.CategorieIpaRepository
        Dim data = ParsecPro.CategorieIpaRepository.CreateCategories.Where(Function(c) c.Descrizione.ToLower.StartsWith(context.Text.ToLower))

        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData) '(endOffset - itemOffset)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            Dim item = data.ElementAt(i)
            itemData.Text = item.Descrizione
            itemData.Value = item.Valore
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function

#End Region
End Class
