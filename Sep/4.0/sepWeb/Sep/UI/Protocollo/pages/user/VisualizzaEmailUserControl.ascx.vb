Imports System.IO

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class VisualizzaEmailUserControl
    Inherits System.Web.UI.UserControl

#Region "CLASSE UTILITA'"

    Public Class AllegatoEmail
        Public Property Id As String
        Public Property Nomefile As String
        Public Property Content As Byte()
    End Class

#End Region

#Region "EVENTI"

    Public Event OnCloseEvent()
    Public Event OnShowEvent()

#End Region

#Region "PROPRIETA'"

    'Variabile di sessione: lista allegati delle mail
    Public Property AllegatiEmail As List(Of AllegatoEmail)
        Get
            Return CType(Session("VisualizzaEmailUserControl_AllegatiEmail" & Me.IdSessione), List(Of AllegatoEmail))
        End Get
        Set(ByVal value As List(Of AllegatoEmail))
            Session("VisualizzaEmailUserControl_AllegatiEmail" & Me.IdSessione) = value
        End Set
    End Property

    'Variabile di sessione: oggeto MailMessage corrente
    Public Property MailMessage As Rebex.Mail.MailMessage
        Get
            Return CType(Session("VisualizzaEmailUserControl_MailMessage" & Me.IdSessione), Rebex.Mail.MailMessage)
        End Get
        Set(ByVal value As Rebex.Mail.MailMessage)
            Session("VisualizzaEmailUserControl_MailMessage" & Me.IdSessione) = value
        End Set
    End Property

    'Variabile di sessione: id della sessione
    Public Property IdSessione As String
        Set(ByVal value As String)
            Me.infoSessioneHidden.Value = value
        End Set
        Get
            Return Me.infoSessioneHidden.Value
        End Get
    End Property

#End Region

#Region "EVENTI PAGINA"

  

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemCommand associato alla AllegatiEmailGridView. Fa partire i comandi associati alla griglia.
    Protected Sub AllegatiEmailGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiEmailGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.DownloadAllegatoEmail(e.Item)
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    'Chiude la anteprima della mail resettando i vari oggetti popolati
    Protected Sub ChiudiAnteprimaEmailButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiAnteprimaEmailButton.Click
        RaiseEvent OnCloseEvent()

        Me.AllegatiEmail = Nothing
        Me.MailMessage = Nothing
        Me.IdSessione = Nothing
    End Sub

    'Stampa la mail
    Protected Sub StampaEmailButton_Click(sender As Object, e As System.EventArgs) Handles StampaEmailButton.Click
      
        Dim posizione As Nullable(Of Integer) = Nothing
        If Me.PosizioneTimbroComboBox.Visible Then
            posizione = Me.PosizioneTimbroComboBox.SelectedItem.Index
        End If

        Dim bytes As Byte() = Nothing
        Dim pdfDocument As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4)
        Using memoryStream As New MemoryStream()
            Dim writer As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocument, memoryStream)
            pdfDocument.Open()

            Dim mainTable As New iTextSharp.text.pdf.PdfPTable(1) With {.WidthPercentage = 100}


            Dim table As New iTextSharp.text.pdf.PdfPTable(2) With {.WidthPercentage = 100}
            table.SetWidths(New Single() {20.0F, 80.0F})


            Dim fontBold As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)
            Dim fontNormal As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL)

            Dim cell As New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("Oggetto: ", fontBold))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(Me.MailMessage.Subject, fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)


            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("Inviata: ", fontBold))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)


            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(Me.MailMessage.Date.OriginalTime.ToString("dddd dd MMMM yyyy HH:mm"), fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)


            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("Da: ", fontBold))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(Me.MailMessage.From.ToString, fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("A: ", fontBold))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(Me.MailMessage.To.ToString, fontNormal))
            cell.Border = 0
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)


            If Me.MailMessage.Attachments.Count > 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("Allegati: ", fontBold))
                cell.Border = iTextSharp.text.Rectangle.NO_BORDER
                cell.Padding = 0
                cell.UseDescender = True
                table.AddCell(cell)

                Dim allegati As String = Me.MailMessage.Attachments.Select(Function(c) c.DisplayName).Aggregate(Function(c, n) c & " - " & n)


                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(allegati, fontNormal))
                cell.Border = 0
                cell.Padding = 0
                cell.UseDescender = True
                table.AddCell(cell)
            End If

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", fontNormal))

            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER
            cell.Padding = 0
            cell.FixedHeight = 10
            cell.UseDescender = True
            table.AddCell(cell)

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", fontNormal))
            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER
            cell.FixedHeight = 10
            cell.Padding = 0
            cell.UseDescender = True
            table.AddCell(cell)


            Dim bodyTable As New iTextSharp.text.pdf.PdfPTable(1) With {.WidthPercentage = 100}
            bodyTable.SetWidths(New Single() {100.0F})

            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(vbCrLf, fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            bodyTable.AddCell(cell)

            Dim corpo As String = Me.ExtractTextFromHtml(Me.contenutoEmail.Text)
            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(corpo, fontNormal))
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.Padding = 0
            cell.UseDescender = True
            bodyTable.AddCell(cell)


            Dim mainCell As New iTextSharp.text.pdf.PdfPCell
            mainCell.Border = iTextSharp.text.Rectangle.NO_BORDER

            mainCell.AddElement(table)
            mainCell.AddElement(bodyTable)


            mainTable.AddCell(mainCell)


            pdfDocument.Add(mainTable)


            pdfDocument.Close()
            bytes = memoryStream.ToArray()
            memoryStream.Close()


        End Using


        If posizione.HasValue Then
            If posizione <> 0 Then
                If Not String.IsNullOrEmpty(Me.WatermarkHidden.Value) Then
                    Dim watermark As String = Me.WatermarkHidden.Value
                    bytes = AddWatermarkToPdf(watermark, posizione, bytes)
                End If
            End If
        End If

        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaFattura")
        parametriStampa.Add("FullPath", bytes)
        Session("ParametriStampaPro") = parametriStampa
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)

    End Sub

    'Visualizza la Fattura
    Protected Sub VisualizzaFatturaButton_Click(sender As Object, e As System.EventArgs) Handles VisualizzaFatturaButton.Click
        Me.VisualizzaFattura()
    End Sub

    'Effettua il download della mail
    Protected Sub VisualizzaEmailButton_Click(sender As Object, e As System.EventArgs) Handles VisualizzaEmailButton.Click
        Session("AttachmentFullName") = Me.FullNameHidden.Value
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
    End Sub

#End Region

#Region "METODI PRIVATI"

    'Carica la combo relativa alla posizione del timbro
    Private Sub CaricaPosizioneTimbro()
        Dim dati As New Dictionary(Of String, String)
        dati.Add("1", "Sopra")
        dati.Add("2", "Sotto")
        dati.Add("3", "Destra")
        dati.Add("4", "Sinistra")
        Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})
        Me.PosizioneTimbroComboBox.DataSource = ds
        Me.PosizioneTimbroComboBox.DataTextField = "Descrizione"
        Me.PosizioneTimbroComboBox.DataValueField = "Id"
        Me.PosizioneTimbroComboBox.DataBind()
        Me.PosizioneTimbroComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.PosizioneTimbroComboBox.SelectedIndex = 3

    End Sub

    'Estrae il testo dall'Html
    Private Function ExtractTextFromHtml(s As String) As String
        Dim pattern As String = "<.*?>"

        Dim reg As Regex = New Regex("<[^>]+>", RegexOptions.IgnoreCase)
        Dim regexCss = New Regex("(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
        s = regexCss.Replace(s, String.Empty)

        s = reg.Replace(s, String.Empty)

        s = s.Replace("&nbsp;", String.Empty)
        s = HttpUtility.HtmlDecode(s).Trim
        Return s
    End Function

    'Rimuove dal testo passato come parametro i tag html
    Private Function RemoveHtmlTag(ByVal text As String, ByVal fromTag As String, ByVal toTag As String) As String
        If text Is Nothing Then Return Nothing
        Dim pattern As String = "" & fromTag & ".*?" & toTag
        Dim rgx As Regex = New Regex(pattern, RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        Dim matches As MatchCollection = rgx.Matches(text)
        Return If(matches.Count <= 0, text, matches.Cast(Of Match)().Where(Function(match) Not String.IsNullOrEmpty(match.Value)).Aggregate(text, Function(current, match) current.Replace(match.Value, "")))
    End Function

    'Visaulizza l'anteprima della mail
    Private Sub AnteprimaEmail(ByVal fullPathEmail As String)

        Me.MailMessage = Nothing
        Me.OggettoEmailLabel.Text = String.Empty
        Me.DataEmailLabel.Text = String.Empty
        Me.MittenteEmailLabel.Text = String.Empty
        Me.DestinatarioEmailLabel.Text = String.Empty
        Me.contenutoEmail.Text = String.Empty

        Dim innerMessage As Rebex.Mail.MailMessage = Nothing
        Dim ms As IO.MemoryStream = Nothing
        Dim lista As New List(Of AllegatoEmail)
        Dim message As New Rebex.Mail.MailMessage
        message.Load(fullPathEmail)
        message.Settings.IgnoreInvalidTnefMessages = True

        Dim attEmail = message.Attachments.Where(Function(c) c.FileName.ToLower.EndsWith(".eml")).FirstOrDefault

        If Not attEmail Is Nothing Then
            innerMessage = New Rebex.Mail.MailMessage
            innerMessage.Settings.IgnoreInvalidTnefMessages = True
            ms = New IO.MemoryStream
            attEmail.Save(ms)
            ms.Position = 0
            innerMessage.Load(ms)
            Me.MailMessage = innerMessage
        Else
            Me.MailMessage = message
        End If

        Me.OggettoEmailLabel.Text = "<span><b>Oggetto:</b></span>&nbsp" & Me.MailMessage.Subject
        Try
            Me.DataEmailLabel.Text = "<span><b>Inviata:</b></span>&nbsp" & Me.MailMessage.Date.OriginalTime.ToString("dddd dd MMMM yyyy HH:mm")
        Catch ex As Exception
            Try
                Me.DataEmailLabel.Text = "<span><b>Inviata:</b></span>&nbsp" & message.Date.OriginalTime.ToString("dddd dd MMMM yyyy HH:mm")
            Catch ex2 As Exception

            End Try
        End Try

        Me.MittenteEmailLabel.Text = "<span><b>Da:</b></span>&nbsp" & Me.MailMessage.From.ToString
        Me.DestinatarioEmailLabel.Text = "<span><b>A:</b></span>&nbsp" & Me.MailMessage.To.ToString


        Dim i As Integer = 0
        If Me.MailMessage.HasBodyHtml Then


            Me.contenutoEmail.Text = Me.RemoveHtmlTag(Me.MailMessage.BodyHtml, "<form", "</form>")
            'ELIMINO I TAG FORM CHE NON HANNO LA CHIUSURA
            Me.contenutoEmail.Text = Me.RemoveHtmlTag(Me.contenutoEmail.Text, "<form", ">")

        Else
            Me.contenutoEmail.Text = Me.MailMessage.BodyText.Replace(Chr(10), "</br>")
        End If

        Dim content As Byte() = Nothing
        Dim estensione As String = String.Empty
        Dim fatturaTrovata As Boolean = False

        For Each innerAtt In Me.MailMessage.Attachments
            i += 1
            ms = New IO.MemoryStream
            innerAtt.Save(ms)
            ms.Position = 0
            content = ms.ToArray
            lista.Add(New AllegatoEmail With {.Id = i, .Nomefile = innerAtt.FileName, .Content = content})


            estensione = IO.Path.GetExtension(innerAtt.FileName)
            Select Case estensione.ToLower
                Case ".p7m"
                    'estrarre il documento interno se è un pdf 
                    Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
                    Try
                        content = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(content))
                    Catch ex As Exception
                        'NIENTE
                    End Try
                    Try
                        signedCms.Decode(content)
                        content = signedCms.ContentInfo.Content
                        estensione = "." & innerAtt.FileName.Split(".")(1)
                    Catch ex As Exception

                        content = System.Text.ASCIIEncoding.Default.GetBytes("Il file p7m non è valido per il seguente motivo:" & vbCrLf & ex.Message)
                        estensione = ".txt"
                    End Try


            End Select


            If Not fatturaTrovata Then
                If estensione.ToLower = ".xml" Then
                    Dim msxml As IO.MemoryStream = ParsecUtility.Utility.FixVersioneXml(content)
                    Try
                        Dim element = XElement.Load(msxml)
                        If Not element Is Nothing Then
                            Dim header = element.Element("FatturaElettronicaHeader")
                            If Not header Is Nothing Then
                                fatturaTrovata = True
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                End If
            End If
        Next


        Dim resStream As IO.MemoryStream = Nothing
        Dim base64 As String = String.Empty
        For Each res In Me.MailMessage.Resources
            resStream = New IO.MemoryStream
            Try
                res.Save(resStream)
                base64 = System.Convert.ToBase64String(resStream.GetBuffer)
                Me.contenutoEmail.Text = Me.contenutoEmail.Text.Replace("cid:" & res.ContentId.Id, "data:" & res.MediaType & ";base64," & base64)
                resStream.Close()
            Catch ex As Exception
                'NIENTE
            End Try
        Next


        Dim resources = Me.MailMessage.Resources.Where(Function(c) Not c.ContentDisposition Is Nothing)

        For Each res In resources.Where(Function(c) Not c.ContentDisposition.Inline)


            i += 1
            ms = New IO.MemoryStream
            res.Save(ms)
            ms.Position = 0
            content = ms.ToArray
            If Not String.IsNullOrEmpty(res.FileName) Then
                lista.Add(New AllegatoEmail With {.Id = i, .Nomefile = res.FileName, .Content = content})

                estensione = IO.Path.GetExtension(res.FileName)
                Select Case estensione.ToLower
                    Case ".p7m"
                        'estrarre il documento interno se è un pdf 
                        Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
                        Try
                            content = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(content))
                        Catch ex As Exception
                            'NIENTE
                        End Try
                        Try
                            signedCms.Decode(content)
                            content = signedCms.ContentInfo.Content
                            estensione = "." & res.FileName.Split(".")(1)
                        Catch ex As Exception

                            content = System.Text.ASCIIEncoding.Default.GetBytes("Il file p7m non è valido per il seguente motivo:" & vbCrLf & ex.Message)
                            estensione = ".txt"
                        End Try

                End Select

            End If

        Next

        Me.VisualizzaFatturaButton.Visible = fatturaTrovata

        Dim fattura As ParsecPro.FatturaElettronica = Me.GetFattura


        Me.VisualizzaFatturaButton.Visible = Not fattura Is Nothing

        Me.AllegatiEmail = lista
        Me.AllegatiEmailGridView.DataSource = lista
        Me.AllegatiEmailGridView.DataBind()

        AllegatiEmailLabel.Text = "Allegati" & If(lista.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & lista.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        Me.PosizioneTimbroComboBox.SelectedIndex = 3



    End Sub

    'Restituisce la Fattura memorizzata su DB
    Private Function GetFattura() As ParsecPro.FatturaElettronica
        Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
        Dim fattura As ParsecPro.FatturaElettronica = Nothing

        If Not String.IsNullOrEmpty(Me.IdEmailHidden.Value) Then
            fattura = fattureElettroniche.Where(Function(c) c.MessaggioSdI.IdEmail = CInt(Me.IdEmailHidden.Value)).FirstOrDefault
        Else
            If Not String.IsNullOrEmpty(Me.AnnoProtocolloHidden.Value) Then
                If Not String.IsNullOrEmpty(Me.NumeroProtocolloHidden.Value) Then
                    fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = CInt(Me.AnnoProtocolloHidden.Value) And c.NumeroProtocollo = CInt(Me.NumeroProtocolloHidden.Value)).FirstOrDefault
                End If

            End If
        End If
        fattureElettroniche.Dispose()
        Return fattura

    End Function

    'Scarica l'allegato della mail
    Private Sub DownloadAllegatoEmail(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim posizione As Nullable(Of Integer) = Nothing
        If Me.PosizioneTimbroComboBox.Visible Then
            posizione = Me.PosizioneTimbroComboBox.SelectedItem.Index
        End If

        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As AllegatoEmail = Me.AllegatiEmail.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then

            Dim estensione As String = IO.Path.GetExtension(filename)
            Dim content As Byte() = allegato.Content

            Select Case estensione.ToLower
                Case ".pdf"

                    If posizione.HasValue Then
                        If posizione <> 0 Then
                            If Not String.IsNullOrEmpty(Me.WatermarkHidden.Value) Then
                                Dim watermark As String = Me.WatermarkHidden.Value
                                content = AddWatermarkToPdf(watermark, posizione, content)
                            End If
                        End If
                    End If

                Case ".p7m"
                    'todo

                Case Else
                    'todo

            End Select

            Dim ht As New Hashtable

            Dim est As String = IO.Path.GetExtension(filename)
            Select Case est.ToLower
                Case ".p7m"
                    ht.Add("Content", allegato.Content)
                    ht.Add("Extension", ".p7m")
                Case Else
                    ht.Add("Content", content)
                    ht.Add("Extension", estensione)
            End Select

            ht.Add("Filename", filename)
            Session("AttachmentFullName") = ht
            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"

            If estensione.ToLower = ".pdf" Then
                ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
            Else
                ParsecUtility.Utility.PageReload(pageUrl, False)
            End If

        End If
    End Sub


    'Visualizza la Fattura Elettronica nel Pannellino
    Private Sub VisualizzaFattura()

        Dim fattura As ParsecPro.FatturaElettronica = Me.GetFattura

        If Not fattura Is Nothing Then
            Dim queryString As New Hashtable
            Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaFatturaPage.aspx"
            queryString.Add("IdFatturaElettronica", fattura.Id.ToString)
            ParsecUtility.Utility.ShowPopup(pageUrl, 950, 600, queryString, False)
        End If

    End Sub

    'Aggiunge la Segnatura sul PDF
    Private Function AddWatermarkToPdf(ByVal watermark As String, ByVal posizione As Integer, ByVal bytes As Byte()) As Byte()
        Dim ms As New MemoryStream()
        Dim reader As New iTextSharp.text.pdf.PdfReader(bytes)
        Dim pdfStamper As New iTextSharp.text.pdf.PdfStamper(reader, ms)

        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim rot As Single = 0.0F

        For i As Integer = 1 To reader.NumberOfPages
            Dim size As iTextSharp.text.Rectangle = reader.GetPageSizeWithRotation(i)
            Dim cb As iTextSharp.text.pdf.PdfContentByte = pdfStamper.GetOverContent(i)
            cb.SaveState()
            Dim bf As iTextSharp.text.pdf.BaseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED)
            cb.SetColorFill(iTextSharp.text.BaseColor.BLACK)
            cb.SetFontAndSize(bf, 12)
            cb.BeginText()

            Select Case posizione
                'SOPRA
                Case 1
                    x = (size.Right + size.Left) / 2
                    y = size.Top - 15
                    rot = 0

                    'SOTTO
                Case 2

                    x = (size.Right + size.Left) / 2
                    y = 5
                    rot = 0

                    'DESTRA
                Case 3

                    x = size.Width - 15
                    y = size.Height / 2
                    rot = -90
                    'SINISTRA
                Case 4

                    x = 15
                    y = (size.Top - size.Bottom) / 2
                    rot = 90

            End Select

            cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermark, x, y, rot)

            cb.EndText()
            cb.RestoreState()
        Next

        pdfStamper.Close()
        reader.Close()

        bytes = ms.ToArray()
        ms.Close()
        Return bytes
    End Function

#End Region

#Region "METODI PUBBLICI - VISUALIZZAZIONE EMAIL"

    'Visualizza il pannello della mail
    Public Sub ShowPanel()

        Me.ChiudiAnteprimaEmailButton.Attributes.Add("onclick", "HideEmailPanel();hideEmailPanel=true;return false;")

        Dim script As New Text.StringBuilder
        script.AppendLine("<script language='javascript'>")
        script.AppendLine("ShowEmailPanel();hideEmailPanel=false;")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
        Me.Visible = True
        RaiseEvent OnShowEvent()
        Me.IdSessione = Now.Ticks.ToString
    End Sub

    'Inizializza la User Interface
    Public Sub InitUI(ByVal fullPathEmail As String, ByVal idEmail As Nullable(Of Integer), ByVal watermark As String)
        Dim visibile As Boolean = Not String.IsNullOrEmpty(watermark)
        Me.PosizioneTimbroLabel.Visible = visibile
        Me.PosizioneTimbroComboBox.Visible = visibile
        If visibile Then
            Me.CaricaPosizioneTimbro()
        End If

        Me.IdEmailHidden.Value = String.Empty

        Me.WatermarkHidden.Value = watermark
        If idEmail.HasValue Then
            Me.IdEmailHidden.Value = idEmail.ToString
        End If

        Me.FullNameHidden.Value = fullPathEmail
        AnteprimaEmail(fullPathEmail)
    End Sub

    'Inizializza la User Interface
    Public Sub InitUI(ByVal fullPathEmail As String, ByVal numeroProtocollo As Nullable(Of Integer), ByVal annoProtocollo As Nullable(Of Integer), ByVal watermark As String)
        Dim visibile As Boolean = Not String.IsNullOrEmpty(watermark)
        Me.PosizioneTimbroLabel.Visible = visibile
        Me.PosizioneTimbroComboBox.Visible = visibile
        If visibile Then
            Me.CaricaPosizioneTimbro()
        End If

        Me.WatermarkHidden.Value = watermark

        Me.NumeroProtocolloHidden.Value = String.Empty

        If numeroProtocollo.HasValue Then
            Me.NumeroProtocolloHidden.Value = numeroProtocollo.ToString
        End If

        Me.AnnoProtocolloHidden.Value = String.Empty

        If annoProtocollo.HasValue Then
            Me.AnnoProtocolloHidden.Value = annoProtocollo.ToString
        End If

        Me.FullNameHidden.Value = fullPathEmail
        AnteprimaEmail(fullPathEmail)
    End Sub

#End Region


End Class