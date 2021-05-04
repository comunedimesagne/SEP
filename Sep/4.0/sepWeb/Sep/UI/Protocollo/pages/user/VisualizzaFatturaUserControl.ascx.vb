Imports ParsecPro

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class VisualizzaFatturaUserControl
    Inherits System.Web.UI.UserControl

#Region "EVENTI"

    Public Event OnCloseEvent()
    Public Event OnShowEvent()

#End Region

#Region "PROPRIETA'"

    'Variabile di sessione: lista allegati della fattura
    Public Property AllegatiFattura As List(Of ParsecPro.AllegatoFattura)
        Get
            Return CType(Session("VisualizzaFatturaUserControl_AllegatiFattura" & Me.IdSessione), List(Of ParsecPro.AllegatoFattura))
        End Get
        Set(ByVal value As List(Of ParsecPro.AllegatoFattura))
            Session("VisualizzaFatturaUserControl_AllegatiFattura" & Me.IdSessione) = value
        End Set
    End Property

    'Variabile di sessione: fattura in formato html
    Public Property HtmlFattura As String
        Get
            Return CType(Session("VisualizzaFatturaUserControl_HtmlFattura" & Me.IdSessione), String)
        End Get
        Set(ByVal value As String)
            Session("VisualizzaFatturaUserControl_HtmlFattura" & Me.IdSessione) = value
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

    'Variabile di sessione: oggetto Fattura corrente
    Public Property Fattura As ParsecPro.FatturaElettronica
        Get
            Return CType(Session("VisualizzaFatturaUserControl_Fattura" & Me.IdSessione), ParsecPro.FatturaElettronica)
        End Get
        Set(ByVal value As ParsecPro.FatturaElettronica)
            Session("VisualizzaFatturaUserControl_Fattura" & Me.IdSessione) = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    'Metodo PreRender della pagina: prepara i pannelli alla visualizzazione. Se ha allegati setta i titoli della relativa griglia.
    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        If Not Me.HtmlFattura Is Nothing Then
            Me.pannelloFattura.Controls.Clear()
            Me.pannelloFattura.Controls.Add(New LiteralControl(Me.HtmlFattura))
        End If

        If Not Me.AllegatiFattura Is Nothing Then
            Me.DocumentiLabel.Text = "Allegati " & If(Me.AllegatiFattura.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiFattura.Count.ToString & ")</span>", "")
            Me.DocumentiGridView.DataSource = Me.AllegatiFattura
            Me.DocumentiGridView.DataBind()
        Else
            Me.DocumentiLabel.Text = "Allegati"
        End If

    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemCommand associato alla DocumentiGridView. Fa partire i comandi associati alla griglia.
    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.DownloadFileFattura(e.Item)
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI"


    'Chiude il pannello ripulendo gli oggetti popolati.
    Protected Sub ChiudiButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiButton.Click
        RaiseEvent OnCloseEvent()
        Me.AllegatiFattura = Nothing
        Me.HtmlFattura = Nothing
        Me.IdSessione = Nothing
        Me.Fattura = Nothing
        Me.pannelloFattura.Controls.Clear()

        Me.VisualizzaFatturaButton.Text = "Tabellare"
        Me.VisualizzaFatturaButton.Icon.PrimaryIconUrl = "~/images/Table.png" '"../../../../images/Table.png"
    End Sub

    'Download della Fattura 
    Protected Sub SalvaFatturaButton_Click(sender As Object, e As System.EventArgs) Handles SalvaFatturaButton.Click
        Session("AttachmentFullName") = Me.FullNameHidden.Value
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
    End Sub

    'Visualizza la fattura o in modlità classica o modalità tabellare.
    Protected Sub VisualizzaFatturaButton_Click(sender As Object, e As System.EventArgs) Handles VisualizzaFatturaButton.Click
        If VisualizzaFatturaButton.Text = "Tabellare" Then
            Me.AnteprimaFattura(Me.Fattura, ParsecUtility.VersioneVisualizzazione.Tabellare)
            Me.VisualizzaFatturaButton.Text = "Classica"
            Me.VisualizzaFatturaButton.Icon.PrimaryIconUrl = "~/images/List.png" ' "../../../../images/List.png"
        Else
            Me.AnteprimaFattura(Me.Fattura, ParsecUtility.VersioneVisualizzazione.Classica)
            Me.VisualizzaFatturaButton.Text = "Tabellare"
            Me.VisualizzaFatturaButton.Icon.PrimaryIconUrl = "~/images/Table.png" ' "../../../../images/Table.png"
        End If

    End Sub




#End Region

#Region "METODI PRIVATI"

    'Registra gli script per la stampa del contentuto del pannello
    Private Sub RegisterPrintScript()

        Me.StampaFatturaButton.Attributes.Add("onclick", "PrintContent();return false;")

        Dim sb As New System.Text.StringBuilder
        sb.AppendLine("<script language='javascript'>")
        sb.AppendLine("function PrintContent() {")

        sb.AppendLine(" var content = document.getElementById('" & Me.pannelloFattura.ClientID & "').innerHTML;")
        sb.AppendLine(" var printIframe = $get('ifmcontentstoprint');")
        sb.AppendLine(" var printDocument = printIframe.contentWindow.document;")
        sb.AppendLine(" printDocument.designMode = 'on';")
        sb.AppendLine(" printDocument.open();")
        sb.AppendLine(" printDocument.write('<html><head></head><body>' + content + '</body></html>');")
        sb.AppendLine(" printDocument.close();")
        sb.AppendLine("     try {")
        sb.AppendLine("         if (document.all) {")
        sb.AppendLine("             printDocument.execCommand('Print', null, false);")
        sb.AppendLine("         }")
        sb.AppendLine("         else {")
        sb.AppendLine("             printIframe.contentWindow.print();")
        sb.AppendLine("         }")
        sb.AppendLine(" }")
        sb.AppendLine(" catch (ex) {")
        sb.AppendLine("     alert(ex.Message);")
        sb.AppendLine(" }")


        sb.AppendLine("}")
        sb.AppendLine("</script>")

        ParsecUtility.Utility.RegisterScript(sb, False)

    End Sub

    'Visualizza l' anteprima della notifica
    Private Sub AnteprimaNotifica(ByVal fattura As ParsecPro.FatturaElettronica, ByVal idNotifica As Integer)

        Me.GrigliaAllegatiPanel.Visible = False

        Dim pathNotifica As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche")
        Dim pathModelloNotifica As String = ParsecAdmin.WebConfigSettings.GetKey("ModelloFatturaElettronica")

        Dim pathRelativoNotifica As String = fattura.MessaggioSdI.PercorsoRelativo

        Dim nomeFileNotifica As String = fattura.NotificaSdI.Where(Function(n) n.Id = idNotifica).FirstOrDefault.MessaggioSdI.Nomefile

        Dim tipoNotifica As String = fattura.NotificaSdI.Where(Function(n) n.Id = idNotifica).FirstOrDefault.TipologiaNotificaSdI.Codice


        Dim path As String = pathNotifica & pathRelativoNotifica & nomeFileNotifica
        Me.FullNameHidden.Value = path

        Dim fatturaUtility As New ParsecUtility.FatturaUtility
        Try
            Me.HtmlFattura = fatturaUtility.TrasformaNotificaInHtmlString(pathModelloNotifica, pathNotifica & pathRelativoNotifica, nomeFileNotifica, tipoNotifica)


            If fattura.NumeroProtocollo.HasValue Then
                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim registrazione = registrazioni.Where(Function(c) c.Id = fattura.IdRegistrazione).FirstOrDefault
                registrazioni.Dispose()
                If Not registrazione Is Nothing Then
                    Me.HtmlFattura = Me.HtmlFattura.Replace("<div id=""NumeroProtocollo""></div>", "<div id=""NumeroProtocollo"">Protocollo n. " & fattura.NumeroProtocollo.ToString & " del " & registrazione.DataImmissione.Value.ToShortDateString & "</div>")
                End If
            End If

        Catch ex As Exception
            If IO.File.Exists(path) Then
                Dim s = IO.File.ReadAllText(path)
                Me.HtmlFattura = "<HR><span style='color:red;font-size:30px'>ATTENZIONE IL FILE NON E' VALIDO</span><BR><HR><BR>" & ParsecUtility.Utility.SpecialXmlEscape(s)
            Else
                ParsecUtility.Utility.MessageBox(ex.Message.Replace("\", "/"), False)
            End If
        End Try
    End Sub

    'Visualizza l'anteprima della Fattura
    Private Sub AnteprimaFattura(ByVal fattura As ParsecPro.FatturaElettronica, ByVal versioneVisualizzazione As ParsecUtility.VersioneVisualizzazione)

        Me.GrigliaAllegatiPanel.Visible = True

        Me.AllegatiFattura = New List(Of ParsecPro.AllegatoFattura)
        Me.HtmlFattura = Nothing

        Dim pathModelloFatturaElettronica As String = ParsecAdmin.WebConfigSettings.GetKey("ModelloFatturaElettronica")

        Dim pathRelativoFatturaElettronica As String = String.Empty
        Dim nomeFileFatturaElettronica As String = fattura.MessaggioSdI.Nomefile



        Dim versioneFatturaElettronica As String = fattura.VersioneFattura

        Dim pathFatturaElettronica As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

        If fattura.IdStato <> StatoFattura.Ricevuta Then
            pathFatturaElettronica = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche")
            pathRelativoFatturaElettronica = fattura.MessaggioSdI.PercorsoRelativo
        End If


        Dim fatturaUtility As New ParsecUtility.FatturaUtility
        Dim path As String = pathFatturaElettronica & pathRelativoFatturaElettronica & nomeFileFatturaElettronica

        Me.FullNameHidden.Value = path

        Try

            Dim listaAllegatiFattura = Me.GetAllegatiFattura(path)
            Me.AllegatiFattura = listaAllegatiFattura


            If path.ToLower.EndsWith(".p7m") Then
                Dim buffer As Byte() = IO.File.ReadAllBytes(path)
                Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
                'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
                Try
                    buffer = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(buffer))
                Catch ex As Exception
                    'NIENTE
                End Try

                signedCms.Decode(buffer)
                Me.HtmlFattura = fatturaUtility.TrasformaFatturaInHtmlString(pathModelloFatturaElettronica, signedCms.ContentInfo.Content, versioneFatturaElettronica, versioneVisualizzazione)
            Else
                Me.HtmlFattura = fatturaUtility.TrasformaFatturaInHtmlString(pathModelloFatturaElettronica, pathFatturaElettronica & pathRelativoFatturaElettronica, nomeFileFatturaElettronica, versioneFatturaElettronica, versioneVisualizzazione)
            End If

            If fattura.NumeroProtocollo.HasValue Then

                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim registrazione = registrazioni.Where(Function(c) c.Id = fattura.IdRegistrazione).FirstOrDefault
                registrazioni.Dispose()
                If Not registrazione Is Nothing Then
                    Me.HtmlFattura = Me.HtmlFattura.Replace("<div id=""NumeroProtocollo""></div>", "<div id=""NumeroProtocollo"">Protocollo n. " & fattura.NumeroProtocollo.ToString & " del " & registrazione.DataImmissione.Value.ToShortDateString & "</div>")
                End If
            End If

            Me.HtmlFattura = Me.HtmlFattura.Replace("<div id=""IdentificativoSDI""></div>", "<div id=""IdentificativoSDI"">Ident. SDI " & fattura.IdentificativoSdI & "</div>")


        Catch ex As Exception
            If IO.File.Exists(path) Then
                Dim s = IO.File.ReadAllText(path)
                Me.HtmlFattura = "<HR><span style='color:red;font-size:30px'>ATTENZIONE IL FILE NON E' VALIDO</span><BR><HR><BR>" & ParsecUtility.Utility.SpecialXmlEscape(s)
            Else
                ParsecUtility.Utility.MessageBox(ex.Message.Replace("\", "/"), False)
            End If
        End Try

    End Sub

    'Effettua il download della fattura
    Private Sub DownloadFileFattura(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As ParsecPro.AllegatoFattura = Me.AllegatiFattura.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then

            Dim estensione As String = IO.Path.GetExtension(filename)
            Dim ht As New Hashtable
            ht.Add("Content", allegato.Content)
            ht.Add("Extension", estensione)
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

    'Restituisce la lista degli allegati associati alla Fattura
    Private Function GetAllegatiFattura(ByVal pathFattura As String) As List(Of ParsecPro.AllegatoFattura)

        Dim el As XElement = Nothing
        Dim ms As IO.MemoryStream = Nothing

        If pathFattura.ToLower.EndsWith(".p7m") Then
            Dim buffer As Byte() = IO.File.ReadAllBytes(pathFattura)
            Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
            'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
            Try
                buffer = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(buffer))
            Catch ex As Exception
                'NIENTE
            End Try

            signedCms.Decode(buffer)
            ms = ParsecUtility.Utility.FixVersioneXml(signedCms.ContentInfo.Content)

        Else
            Dim bytes As Byte() = IO.File.ReadAllBytes(pathFattura)
            ms = ParsecUtility.Utility.FixVersioneXml(bytes)
        End If

        el = XElement.Load(ms)

        Dim listaAllegatiFattura As New List(Of ParsecPro.AllegatoFattura)

        Dim header = el.Element("FatturaElettronicaHeader")

        'SE E' UNA FATTURA ELETTRONICA
        If Not header Is Nothing Then
            Dim oggetto As String = String.Empty
            Dim listaBody = el.Elements("FatturaElettronicaBody")
            Dim i As Integer = 0
            For Each body In listaBody
                i += 1
                oggetto = String.Empty

                Dim datiGeneraliDocumento = body.Element("DatiGenerali").Element("DatiGeneraliDocumento")
                Dim numerofattura = datiGeneraliDocumento.Element("Numero").Value

                Dim descrizioneTipoDocumento As String = "Fattura"
                If Not datiGeneraliDocumento.Element("TipoDocumento") Is Nothing Then
                    Dim tipoDocumento = datiGeneraliDocumento.Element("TipoDocumento").Value
                    Select Case tipoDocumento
                        Case "TD01"
                            descrizioneTipoDocumento = "Fattura"
                        Case "TD02"
                            descrizioneTipoDocumento = "Acconto/anticipo su fattura"
                        Case "TD03"
                            descrizioneTipoDocumento = "Acconto/anticipo su parcella"
                        Case "TD04"
                            descrizioneTipoDocumento = "Nota di credito"
                        Case "TD05"
                            descrizioneTipoDocumento = "Nota di debito"
                        Case "TD06"
                            descrizioneTipoDocumento = "Parcella"
                    End Select
                End If


                oggetto &= descrizioneTipoDocumento & " n. " & numerofattura.ToString
                Dim datafattura = Date.Parse(datiGeneraliDocumento.Element("Data").Value).ToShortDateString

                oggetto &= " del " & datafattura


                Dim allegati = body.Elements("Allegati")
                Dim allegatoFattura As ParsecPro.AllegatoFattura = Nothing
                For Each allegato In allegati
                    Dim estensione As String = String.Empty
                    If Not allegato.Element("FormatoAttachment") Is Nothing Then
                        estensione = allegato.Element("FormatoAttachment")
                    End If

                    allegatoFattura = New ParsecPro.AllegatoFattura
                    allegatoFattura.Id = Guid.NewGuid.ToString
                    allegatoFattura.Posizione = i
                    allegatoFattura.Estremi = oggetto

                    Try
                    
                        Dim s = allegato.Element("Attachment").Value.Replace("&#13;&#10;", "")
                        s = s.Replace("&#13", "")
                        s = s.Replace("&#10", "")
                        s = s.Replace("&#xA;", "")
                        s = s.Replace("&#xD;", "")

                        allegatoFattura.Content = System.Convert.FromBase64String(s)
                        If BitConverter.ToInt32(allegatoFattura.Content, 0) = &H4034B50 Then
                            estensione = "zip"
                        End If
                    Catch ex As Exception
                        allegatoFattura.Content = System.Text.ASCIIEncoding.Default.GetBytes("ATTENZIONE L'ALLEGATO NON E' VALIDO" & vbCrLf & allegato.Element("Attachment").Value)
                        allegatoFattura.Nomefile = allegato.Element("NomeAttachment").Value & "." & estensione & ".txt"
                    End Try

                    If Not String.IsNullOrEmpty(estensione) Then
                        allegatoFattura.Nomefile = allegato.Element("NomeAttachment").Value & "." & estensione
                    Else
                        allegatoFattura.Nomefile = allegato.Element("NomeAttachment").Value
                    End If

                    listaAllegatiFattura.Add(allegatoFattura)
                Next

            Next

        End If

        Return listaAllegatiFattura

    End Function

#End Region

#Region "METODI PUBBLICI - VISUALIZZAZIONE FATTURA"


    Public Property CloseScript As String
    Public Property ShowScript As String

    'Visualizza il pannello di visualizzazione della fattura
    Public Sub ShowPanel()

        If String.IsNullOrEmpty(Me.CloseScript) Then
            Me.ChiudiButton.Attributes.Add("onclick", "HideFatturaElettronicaPanel();hideFatturaPanel=true;return false;")
        Else
            Me.ChiudiButton.Attributes.Add("onclick", Me.CloseScript)
        End If

        Me.RegisterPrintScript()

        Dim script As New Text.StringBuilder
        script.AppendLine("<script language='javascript'>")

        If String.IsNullOrEmpty(Me.ShowScript) Then
            script.AppendLine("ShowFatturaElettronicaPanel();hideFatturaPanel=false;")
        Else
            script.AppendLine(Me.ShowScript)
        End If

        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
        Me.Visible = True
        RaiseEvent OnShowEvent()
        Me.IdSessione = Now.Ticks.ToString
    End Sub

    'Inizializza il Pannello di Visulizzazione
    Public Sub InitUI(ByVal fattura As ParsecPro.FatturaElettronica, Optional ByVal idNotifica As Nullable(Of Integer) = Nothing)
        Me.Fattura = fattura

        If idNotifica.HasValue Then
            Me.TitoloPannelloFatturaLabel.Text = "Anteprima Notifica"
            Me.AnteprimaNotifica(fattura, idNotifica)
        Else
            Me.TitoloPannelloFatturaLabel.Text = "Anteprima Fattura"
            Me.AnteprimaFattura(fattura, ParsecUtility.VersioneVisualizzazione.Classica)
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.DocumentiGridView.Style.Add("width", widthStyle)


    End Sub

#End Region



   
End Class