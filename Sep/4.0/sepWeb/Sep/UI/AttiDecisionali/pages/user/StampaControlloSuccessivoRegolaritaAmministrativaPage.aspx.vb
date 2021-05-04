Imports iTextSharp.text
Imports iTextSharp.text.pdf


Partial Class StampaControlloSuccessivoRegolaritaAmministrativaPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


#Region "CLASSI PRIVATE"


    Private Class Estrazione

        Public Property DataEstrazione As String
        Public Property Elementi As New List(Of RigaEstrazione)

    End Class

    Private Class RigaEstrazione

        Public Property Struttura As String
        Public Property NumeroAttiEstratti As String


    End Class



#End Region

#Region "EVENTI PAGINA"


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BlankPage.master"
        End If
    End Sub


    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init


        If Me.Request.QueryString("Mode") Is Nothing Then

            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Atti Decisionali"

        End If

        If Not Me.Page.IsPostBack Then
            Me.ResettaVista()
        End If


    End Sub

#End Region

#Region "EVENTI CONTROLLI"

        Protected Sub StampaButton_Click(sender As Object, e As System.EventArgs) Handles StampaButton.Click
            Try
                Me.Print()
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        End Sub


        Protected Sub AnnullaButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaButton.Click
            Me.ResettaVista()
        End Sub


#End Region

#Region "METODI PRIVATI"

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


    Private Sub GeneraReportPdf(ByVal items As List(Of Estrazione))

        Dim stream As New IO.MemoryStream
        Using document As New Document(PageSize.A4, 18.5, 18.5, 36, 36)

            Dim canvas As PdfContentByte
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, stream)


            document.Open()

            canvas = writer.DirectContent
            Dim pageSize As Rectangle = document.PageSize


            Dim pg As New iTextSharp.text.Paragraph("Elenco Estrazioni", FontFactory.GetFont("Arial", 14, Font.BOLD))
            pg.SpacingAfter = 10
            document.Add(pg)



            Dim detailTable As PdfPTable
            Dim innerTable As PdfPTable


            Dim detailCell As PdfPCell


            For Each p In items

                detailTable = New PdfPTable(1)
                detailTable.TotalWidth = 560
                detailTable.LockedWidth = True
                detailTable.DefaultCell.Border = 0

                detailTable.AddCell(Me.CreateCell("Estrazione del " & p.DataEstrazione, Element.ALIGN_LEFT, 9, paddingBottom:=4))


                innerTable = New PdfPTable(2)
                innerTable.TotalWidth = 560
                innerTable.LockedWidth = True
                innerTable.DefaultCell.Border = PdfPCell.BOTTOM_BORDER

                innerTable.SetWidths(New Single() {280.0F, 280.0F})

                innerTable.AddCell(Me.CreateHeaderCell("STRUTTURA"))
                innerTable.AddCell(Me.CreateHeaderCell("NUMERO ATTI ESTRATTI"))

                For Each ele In p.Elementi
                    innerTable.AddCell(Me.CreateCell(ele.Struttura, Element.ALIGN_LEFT, 9, PdfPCell.BOTTOM_BORDER))
                    innerTable.AddCell(Me.CreateCell(ele.NumeroAttiEstratti, Element.ALIGN_LEFT, 9, PdfPCell.BOTTOM_BORDER))
                Next




                detailCell = New PdfPCell
                detailCell.Border = 0

                detailCell.AddElement(innerTable)


                detailTable.AddCell(detailCell)

                detailTable.AddCell(Me.CreateCell("  ", Element.ALIGN_LEFT, 9, paddingBottom:=10))


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





    Private Sub Print()

        Dim controlliSuccessivi As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativaRepository
        Dim dettagliControllo As New ParsecAtt.DettaglioControlloSuccessivoRegolaritaAmministrativaRepository

        Dim controlli = controlliSuccessivi.Where(Function(c) c.Eseguito)


        If Me.DataEstrazioneInizioTextBox.SelectedDate.HasValue Then
            Dim d As Date = Me.DataEstrazioneInizioTextBox.SelectedDate.Value
            Dim startDate As Date = New Date(d.Year, d.Month, d.Day, 0, 0, 0)
            controlli = controlli.Where(Function(c) c.DataEstrazione >= startDate)
        End If

        If Me.DataEstrazioneFineTextBox.SelectedDate.HasValue Then
            Dim d As Date = Me.DataEstrazioneFineTextBox.SelectedDate.Value
            Dim endDate As Date = New Date(d.Year, d.Month, d.Day, 23, 59, 59)
            controlli = controlli.Where(Function(c) c.DataEstrazione <= endDate)
        End If

        Dim listaControlli = controlli.ToList

        Dim listaDettagli = dettagliControllo.GetQuery.ToList


        Dim items As New List(Of Estrazione)


        Dim item As Estrazione = Nothing
        Dim codice As Integer = 0
        Dim idControllo As Integer = 0

        For i = 0 To listaControlli.Count - 1
            idControllo = listaControlli(i).Id
            item = New Estrazione
            item.DataEstrazione = listaControlli(i).DataEstrazione.ToShortDateString

            Dim codici = listaDettagli.Where(Function(c) c.IdControlloSuccessivoRegolaritaAmministrativa = idControllo).Select(Function(c) c.CodiceSettore).Distinct

            For j = 0 To codici.Count - 1
                codice = codici(j)
                Dim struttura = listaDettagli.Where(Function(c) c.IdControlloSuccessivoRegolaritaAmministrativa = idControllo And c.CodiceSettore = codice).Select(Function(c) c.DescrizioneSettore).FirstOrDefault

                Dim numeroAttiEstratti = listaDettagli.Where(Function(c) c.IdControlloSuccessivoRegolaritaAmministrativa = idControllo And c.CodiceSettore = codice).Select(Function(c) c.NumeroAttiEstratti).Sum

                item.Elementi.Add(New RigaEstrazione With {.Struttura = struttura, .NumeroAttiEstratti = numeroAttiEstratti})
            Next

            items.Add(item)
        Next

        controlliSuccessivi.Dispose()
        dettagliControllo.Dispose()

        If items.Count = 0 Then
            ParsecUtility.Utility.MessageBox("La ricerca non ha prodotto risultati!", False)
            Exit Sub
        End If

        Me.GeneraReportPdf(items)
    End Sub


    Private Sub ResettaVista()
        Me.DataEstrazioneInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataEstrazioneFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
    End Sub


#End Region


End Class