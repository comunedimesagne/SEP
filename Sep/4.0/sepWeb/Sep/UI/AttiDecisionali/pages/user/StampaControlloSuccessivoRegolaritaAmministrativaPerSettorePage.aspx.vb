Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Telerik.Web.UI

Partial Class StampaControlloSuccessivoRegolaritaAmministrativaPerSettorePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "CLASSI PRIVATE"

    Private Class HeaderFooter
        Inherits PdfPageEventHelper

        Private canvas As PdfContentByte
        Private template As PdfTemplate
        Private bf As BaseFont = Nothing
        Private dtfi As System.Globalization.DateTimeFormatInfo

        Public Sub New()
            Me.dtfi = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat
            Me.dtfi = CType(Me.dtfi.Clone(), System.Globalization.DateTimeFormatInfo)
            Me.dtfi.DayNames = Me.UpperNames(Me.dtfi.DayNames)
            Me.dtfi.MonthGenitiveNames = Me.UpperNames(Me.dtfi.MonthGenitiveNames)
        End Sub

        Private Function UpperNames(ByVal values As String()) As String()
            Dim res As String() = New String(values.Length - 1) {}

            For i As Integer = 0 To res.Length - 1
                If Not String.IsNullOrEmpty(values(i)) Then
                    'res(i) = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(values(i).ToLower())
                    'res(i) = Char.ToUpper(values(i)(0)) & values(i).Substring(1)
                    'res(i) = Regex.Replace(values(i), "^[a-z]", Function(m) m.Value.ToUpper())

                    'Dim chars = values(i).ToCharArray()
                    'chars(0) = Char.ToUpper(chars(0))
                    'res(i) = New String(chars)

                    'res(i) = values(i).First.ToString.ToUpper + String.Join("", values(i).Skip(1)).ToLower
                    'res(i) = String.Concat(values(i).Select(Function(currentChar, index) If(index = 0, Char.ToUpper(currentChar), currentChar)))


                    Dim sb = New StringBuilder(values(i))
                    sb(0) = Char.ToUpper(sb(0))
                    res(i) = sb.ToString


                Else
                    res(i) = ""
                End If
            Next

            Return res
        End Function

        Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
            MyBase.OnOpenDocument(writer, document)
            Me.bf = FontFactory.GetFont("Arial", 9, Font.NORMAL).GetCalculatedBaseFont(False)
            Me.canvas = writer.DirectContent
            Me.template = canvas.CreateTemplate(50, 50)
        End Sub

        Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
            MyBase.OnStartPage(writer, document)
            'Dim pageSize As Rectangle = document.PageSize
        End Sub

        Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
            MyBase.OnEndPage(writer, document)

            Dim text As String = String.Format("Pagina {0} di ", writer.PageNumber)

            Dim len As Single = Me.bf.GetWidthPoint(text, 9)

            Dim pageSize As Rectangle = document.PageSize

            Me.canvas.BeginText()
            Me.canvas.SetFontAndSize(bf, 9)
            Me.canvas.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(30))

            Me.canvas.ShowText(text)
            Me.canvas.EndText()

            Me.canvas.AddTemplate(template, pageSize.GetLeft(40) + len, pageSize.GetBottom(30))


            Dim oggi = DateTime.Now.ToString("dddd, dd MMMM yyyy", Me.dtfi)
            Me.canvas.BeginText()
            Me.canvas.SetFontAndSize(bf, 9)
            Me.canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, oggi, pageSize.GetRight(40), pageSize.GetBottom(30), 0)
            Me.canvas.EndText()
        End Sub


        Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
            MyBase.OnCloseDocument(writer, document)
            Me.template.BeginText()
            Me.template.SetFontAndSize(Me.bf, 9)
            Me.template.SetTextMatrix(0, 0)
            Me.template.ShowText((writer.PageNumber - 1).ToString)
            Me.template.EndText()
        End Sub

    End Class

    Private Class ControlloRegolarita
        Public Property Id As Integer
        Public Property PeriodoRiferimento As String

    End Class

    Private Class FiltroControlloRegolarita
        Public Property DataInizio As Nullable(Of DateTime) = Nothing
        Public Property DataFine As Nullable(Of DateTime) = Nothing

    End Class

    Private Class Estrazione
        Public Property PeriodoRiferimento As String
        Public Property Settori As New List(Of SettoreEstrazione)

    End Class

    Private Class SettoreEstrazione
        Public Property DescrizioneSettore As String
        Public Property Tipologie As New List(Of TipologiaAttoEstrazione)

    End Class

    Private Class TipologiaAttoEstrazione
        Public Property DescrizioneTipologiaAtto As String
        Public Property Schede As New List(Of SchedaEstrazione)

    End Class

    Private Class SchedaEstrazione
        Public Property NumeroAtto As String
        Public Property DataAtto As String
        Public Property OggettoAtto As String
        Public Property Osservazioni As String

    End Class

#End Region

#Region "PROPRIETA'"

    Private Property ControlliRegolarita() As List(Of ControlloRegolarita)
        Get
            Return CType(Session("StampaControlloSuccessivoRegolaritaAmministrativaPerSettorePage_ControlliRegolarita"), List(Of ControlloRegolarita))
        End Get
        Set(ByVal value As List(Of ControlloRegolarita))
            Session("StampaControlloSuccessivoRegolaritaAmministrativaPerSettorePage_ControlliRegolarita") = value
        End Set
    End Property

#End Region


#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Stampa Controllo Regolarità Amministrativa Per Settore"

        If Not Me.Page.IsPostBack Then
            Me.ResettaFiltro()
        End If


        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.ControlliRegolaritaAmministrativaGridView.Style.Add("width", widthStyle)
        Me.ControlliRegolaritaAmministrativaPanel.Style.Add("width", widthStyle)
        Me.GrigliaControlliRegolaritaAmministrativaPanel.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete

        If Not Me.ControlliRegolarita Is Nothing Then
            Me.ControlliRegolaritaAmministrativaLabel.Text = "Controlli Regolarità Amministrativa&nbsp;&nbsp;" & If(Me.ControlliRegolarita.Count > 0, "( " & Me.ControlliRegolarita.Count.ToString & " )", "")
        End If

    End Sub

#End Region


#Region "METODI PRIVATI"


    Private Sub ResettaFiltro()
        Me.DataInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataFineTextBox.SelectedDate = Now
        Me.ControlliRegolarita = Nothing

    End Sub

    Private Function GetFiltro() As FiltroControlloRegolarita
        Dim filtro As New FiltroControlloRegolarita
        filtro.DataInizio = Me.DataInizioTextBox.SelectedDate
        filtro.DataFine = Me.DataFineTextBox.SelectedDate
        Return filtro
    End Function


    Private Function GetControlliRegolaritaEseguiti(ByVal filtro As FiltroControlloRegolarita) As List(Of ControlloRegolarita)

        Dim controlli As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativaRepository
        Dim dettagli As New ParsecAtt.DettaglioControlloSuccessivoRegolaritaAmministrativaRepository(controlli.Context)

        Dim view = From controllo In controlli.GetQuery
                   Join dettaglio In dettagli.GetQuery
                       On controllo.Id Equals dettaglio.IdControlloSuccessivoRegolaritaAmministrativa
                   Where controllo.Eseguito = True
                   Select controllo

        If filtro.DataInizio.HasValue Then
            Dim dataInizio = filtro.DataInizio.Value
            Dim startDate As Date = New Date(dataInizio.Year, dataInizio.Month, dataInizio.Day, 0, 0, 0)
            view = view.Where(Function(c) c.DataEstrazione >= startDate)
        End If

        If filtro.DataFine.HasValue Then
            Dim dataFine = filtro.DataFine.Value
            Dim endDate As Date = New Date(dataFine.Year, dataFine.Month, dataFine.Day, 23, 59, 59)
            view = view.Where(Function(c) c.DataEstrazione <= endDate)
        End If



        Dim res = view.AsEnumerable.Distinct.Select(Function(c) New ControlloRegolarita With {
                                               .Id = c.Id,
                                               .PeriodoRiferimento = "Dal " & c.DataEstrazione.Subtract(TimeSpan.FromDays(c.Periodicita - 1)).ToShortDateString & " al " & c.DataEstrazione.ToShortDateString
                                               }).ToList

        controlli.Dispose()



        Return res
    End Function


    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim idControlloSuccessivoRegolaritaAmministrativa As Integer = CInt(item.GetDataKeyValue("Id"))

        Try
            Dim estrazione = Me.GetControlloSuccessivoRegolaritaAmministrativa(idControlloSuccessivoRegolaritaAmministrativa)
            Me.GeneraReportPdf(estrazione)

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try


    End Sub




    Private Function GetControlloSuccessivoRegolaritaAmministrativa(ByVal idControlloSuccessivoRegolaritaAmministrativa As Integer) As Estrazione

        Dim estrazione As New Estrazione

        Dim controlli As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativaRepository

        Dim controllo = controlli.Where(Function(c) c.Id = idControlloSuccessivoRegolaritaAmministrativa).FirstOrDefault

        estrazione.PeriodoRiferimento = "Periodo di riferimento dal " & controllo.DataEstrazione.Subtract(TimeSpan.FromDays(controllo.Periodicita - 1)).ToShortDateString & " al " & controllo.DataEstrazione.ToShortDateString
        controlli.Dispose()

        Dim dettagli As New ParsecAtt.DettaglioControlloSuccessivoRegolaritaAmministrativaRepository
        Dim descrizioneSettori = dettagli.Where(Function(c) c.IdControlloSuccessivoRegolaritaAmministrativa = idControlloSuccessivoRegolaritaAmministrativa).OrderBy(Function(c) c.DescrizioneSettore).Select(Function(c) c.DescrizioneSettore).Distinct.ToList
        dettagli.Dispose()

        Dim settore As SettoreEstrazione = Nothing
        For Each descrizioneSettore In descrizioneSettori
            settore = New SettoreEstrazione
            settore.DescrizioneSettore = descrizioneSettore

            Dim schedeControllo As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
            Dim descrizioniTipologiaAtto = schedeControllo.Where(Function(c) c.IdControlloSuccessivoRegolaritaAmministrativa = idControlloSuccessivoRegolaritaAmministrativa And c.DescrizioneSettore = descrizioneSettore).OrderBy(Function(c) c.DescrizioneTipologiaAtto).Select(Function(c) c.DescrizioneTipologiaAtto).Distinct


            Dim tipologiaAttoEstrazione As TipologiaAttoEstrazione = Nothing


            For Each descrizioneTipologiaAtto In descrizioniTipologiaAtto
                tipologiaAttoEstrazione = New TipologiaAttoEstrazione
                tipologiaAttoEstrazione.DescrizioneTipologiaAtto = descrizioneTipologiaAtto

                Dim schede = schedeControllo.Where(Function(c) c.IdControlloSuccessivoRegolaritaAmministrativa = idControlloSuccessivoRegolaritaAmministrativa And c.DescrizioneSettore = descrizioneSettore And c.DescrizioneTipologiaAtto = descrizioneTipologiaAtto).OrderBy(Function(c) c.DataAtto).ToList

                Dim schedaEstrazione As SchedaEstrazione = Nothing
                For Each scheda In schede
                    schedaEstrazione = New SchedaEstrazione
                    schedaEstrazione.DataAtto = scheda.DataAtto.ToShortDateString
                    schedaEstrazione.NumeroAtto = scheda.NumeroAtto
                    schedaEstrazione.OggettoAtto = scheda.OggettoAtto
                    schedaEstrazione.Osservazioni = scheda.Osservazioni

                    tipologiaAttoEstrazione.Schede.Add(schedaEstrazione)

                Next

                settore.Tipologie.Add(tipologiaAttoEstrazione)


            Next

            schedeControllo.Dispose()

            estrazione.Settori.Add(settore)
        Next
        Return estrazione

    End Function

    Private Function CreateCell(ByVal text As String, ByVal align As Integer, ByVal fontSize As Integer, Optional ByVal border As Integer = PdfPCell.NO_BORDER, Optional ByVal paddingBottom As Integer = 8) As PdfPCell
        text = text.Replace(vbCrLf, " ")
        text = text.Replace(vbCr, " ")
        text = text.Replace(vbLf, " ")
        Dim p As New Paragraph(text, FontFactory.GetFont("Arial", fontSize, Font.NORMAL))
        p.Alignment = align

        Dim cell As New PdfPCell(p)
        cell.HorizontalAlignment = align
        cell.PaddingBottom = paddingBottom
        cell.PaddingRight = 3
        cell.Border = border
        Return cell
    End Function

    Private Sub GeneraReportPdf(estrazione As Estrazione)



        Dim cliente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente

        Dim stream As New IO.MemoryStream
        Using document As New Document(PageSize.A4, 18.5, 18.5, 36, 36)

            Dim writer As PdfWriter = PdfWriter.GetInstance(document, stream)

            writer.PageEvent = New HeaderFooter

            document.Open()

            Dim canvas As PdfContentByte = writer.DirectContent

            Dim pageSize As Rectangle = document.PageSize

            Dim cell As PdfPCell = Nothing

            Dim totalWidth = document.PageSize.Width - (document.LeftMargin + document.RightMargin)

            Dim headerTable As New PdfPTable(2)
            headerTable.TotalWidth = 560
            headerTable.LockedWidth = True
            headerTable.DefaultCell.Border = 0
            headerTable.SpacingAfter = 10
            cell = Me.CreateCell(cliente.Descrizione, Element.ALIGN_LEFT, 14, paddingBottom:=6)
            headerTable.AddCell(cell)
            cell = Me.CreateCell("Controlli di Regolarità Amministrativa", Element.ALIGN_RIGHT, 14, paddingBottom:=6)
            headerTable.AddCell(cell)


            document.Add(headerTable)

            Dim reportTable As New PdfPTable(1)
            reportTable.TotalWidth = 560
            reportTable.LockedWidth = True
            reportTable.DefaultCell.Border = 0
            reportTable.SpacingAfter = 10
            cell = Me.CreateCell(estrazione.PeriodoRiferimento, Element.ALIGN_CENTER, 14, paddingBottom:=6)
            reportTable.AddCell(cell)
            cell = Me.CreateCell("", Element.ALIGN_CENTER, 14, paddingBottom:=6)
            reportTable.AddCell(cell)

            document.Add(reportTable)



            Dim settoreTable As PdfPTable = Nothing


            For Each settore In estrazione.Settori
                settoreTable = New PdfPTable(1)
                settoreTable.TotalWidth = 560
                settoreTable.LockedWidth = True
                settoreTable.DefaultCell.Border = 0
                settoreTable.SpacingAfter = 10
                settoreTable.SpacingBefore = 10

                cell = Me.CreateCell(settore.DescrizioneSettore, Element.ALIGN_CENTER, 9, paddingBottom:=4)
                cell.BackgroundColor = New iTextSharp.text.BaseColor(224, 224, 224)
                settoreTable.AddCell(cell)
                document.Add(settoreTable)

                Dim tipologiaTable As PdfPTable = Nothing
                For Each tipologia In settore.Tipologie
                    tipologiaTable = New PdfPTable(1)
                    tipologiaTable.TotalWidth = 560
                    tipologiaTable.LockedWidth = True
                    tipologiaTable.DefaultCell.Border = 0
                    tipologiaTable.SpacingAfter = 3

                    cell = Me.CreateCell(tipologia.DescrizioneTipologiaAtto, Element.ALIGN_CENTER, 12, paddingBottom:=6)
                    cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                    tipologiaTable.AddCell(cell)

                    document.Add(tipologiaTable)

                    Dim schedaTable As PdfPTable = Nothing
                    Dim schedaCell As PdfPCell


                    Dim estremiTable As PdfPTable = Nothing
                    Dim oggettoTable As PdfPTable = Nothing
                    Dim osservazioniTable As PdfPTable = Nothing



                    For Each scheda In tipologia.Schede

                        schedaTable = New PdfPTable(1)
                        schedaTable.TotalWidth = 560
                        schedaTable.LockedWidth = True
                        schedaTable.DefaultCell.Border = 0
                        schedaTable.SpacingAfter = 3
                        schedaTable.KeepTogether = True

                        '********************************************
                        schedaCell = New PdfPCell
                        schedaCell.Border = 0


                        estremiTable = New PdfPTable(3)
                        estremiTable.TotalWidth = 560
                        estremiTable.SetWidths(New Single() {216.0F, 156.0F, 188.0F})
                        estremiTable.LockedWidth = True
                        estremiTable.DefaultCell.Border = 0
                        estremiTable.SpacingAfter = 3

                        Dim atto As String = String.Format("Atto n. {0}", scheda.NumeroAtto)
                        cell = Me.CreateCell(atto, Element.ALIGN_LEFT, 9, paddingBottom:=4)
                        cell.BackgroundColor = New iTextSharp.text.BaseColor(224, 224, 224)
                        estremiTable.AddCell(cell)

                        cell = Me.CreateCell(String.Format("del {0}", scheda.DataAtto), Element.ALIGN_LEFT, 9, paddingBottom:=4)
                        cell.BackgroundColor = New iTextSharp.text.BaseColor(224, 224, 224)
                        estremiTable.AddCell(cell)

                        cell = Me.CreateCell("", Element.ALIGN_LEFT, 9, paddingBottom:=4)
                        estremiTable.AddCell(cell)

                        schedaCell.AddElement(estremiTable)

                        schedaTable.AddCell(schedaCell)

                        '********************************************
                        schedaCell = New PdfPCell
                        schedaCell.Border = 0

                        oggettoTable = New PdfPTable(1)
                        oggettoTable.TotalWidth = 560
                        oggettoTable.LockedWidth = True
                        oggettoTable.DefaultCell.Border = 0
                        oggettoTable.SpacingAfter = 3


                        cell = Me.CreateCell("Oggetto", Element.ALIGN_LEFT, 9, paddingBottom:=4)
                        oggettoTable.AddCell(cell)
                        cell = Me.CreateCell(scheda.OggettoAtto, Element.ALIGN_LEFT, 9, paddingBottom:=4)
                        oggettoTable.AddCell(cell)

                        schedaCell.AddElement(oggettoTable)

                        schedaTable.AddCell(schedaCell)
                        '********************************************

                        schedaCell = New PdfPCell
                        schedaCell.Border = 0

                        osservazioniTable = New PdfPTable(2)
                        osservazioniTable.TotalWidth = 560
                        osservazioniTable.SetWidths(New Single() {70.0F, 490.0F})
                        osservazioniTable.LockedWidth = True
                        osservazioniTable.DefaultCell.Border = 0
                        osservazioniTable.SpacingAfter = 3


                        cell = Me.CreateCell("Osservazioni", Element.ALIGN_LEFT, 9, paddingBottom:=4)
                        osservazioniTable.AddCell(cell)

                        cell = Me.CreateCell(If(scheda.Osservazioni Is Nothing, " ", scheda.Osservazioni), Element.ALIGN_LEFT, 9, paddingBottom:=4)
                        cell.BackgroundColor = New iTextSharp.text.BaseColor(224, 224, 224)

                        osservazioniTable.AddCell(cell)

                        schedaCell.AddElement(osservazioniTable)

                        schedaTable.AddCell(schedaCell)


                        document.Add(schedaTable)

                        'canvas.MoveTo(document.LeftMargin, document.Top + 10)
                        'canvas.LineTo(document.PageSize.Width - document.LeftMargin, document.Top + 10)
                        'canvas.Stroke()

                    Next

                Next


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



#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ControlliRegolaritaAmministrativaGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ControlliRegolaritaAmministrativaGridView.NeedDataSource
        Try
            If Me.ControlliRegolarita Is Nothing Then
                Me.ControlliRegolarita = Me.GetControlliRegolaritaEseguiti(Me.GetFiltro)
            End If
            Me.ControlliRegolaritaAmministrativaGridView.DataSource = Me.ControlliRegolarita
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    Protected Sub ControlliRegolaritaAmministrativaGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ControlliRegolaritaAmministrativaGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If

        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub




    Protected Sub ControlliRegolaritaAmministrativaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ControlliRegolaritaAmministrativaGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

#End Region


#Region "EVENTI CONTROLLI"

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Me.ControlliRegolarita = Nothing
        Me.ControlliRegolaritaAmministrativaGridView.Rebind()
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.ControlliRegolaritaAmministrativaGridView.Rebind()
    End Sub

#End Region

End Class
