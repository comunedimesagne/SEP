Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Xml.Schema
Imports System.Xml.Xsl
Imports System.Xml.Xsl.XslCompiledTransform

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class VisualizzaFatturaPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA' PAGINA"

    'Variabile di sessione: Fattura Elettronica corrente
    Public Property FatturaElettronica As ParsecPro.FatturaElettronica
        Get
            Return CType(Session("VisualizzaFatturaPage_FatturaElettronica"), ParsecPro.FatturaElettronica)
        End Get
        Set(ByVal value As ParsecPro.FatturaElettronica)
            Session("VisualizzaFatturaPage_FatturaElettronica") = value
        End Set
    End Property

    'Variabile di sessione: Fattura Elettronica in formato html
    Public Property HtmlFattura As String
        Get
            Return CType(Session("VisualizzaFatturaPage_HtmlFattura"), String)
        End Get
        Set(ByVal value As String)
            Session("VisualizzaFatturaPage_HtmlFattura") = value
        End Set
    End Property

    'Variabile di sessione: lista degli Allegati associati alla fattura
    Public Property AllegatiFattura As List(Of ParsecPro.AllegatoFattura)
        Get
            Return CType(Session("VisualizzaFatturaPage_AllegatiFattura"), List(Of ParsecPro.AllegatoFattura))
        End Get
        Set(ByVal value As List(Of ParsecPro.AllegatoFattura))
            Session("VisualizzaFatturaPage_AllegatiFattura") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then
            Me.HtmlFattura = Nothing

            Me.AllegatiFattura = Nothing

            If Not Me.Request.QueryString("IdFatturaElettronica") Is Nothing Then
                Dim idFatturaElettronica = Me.Request.QueryString("IdFatturaElettronica")
                Me.CaricaFattura(idFatturaElettronica, ParsecUtility.VersioneVisualizzazione.Classica)
            Else
                ParsecUtility.Utility.MessageBox("Attenzione: nessuna fattura trovata.", False)
            End If
        Else
            Me.pannelloFattura.Controls.Add(New LiteralControl(Me.HtmlFattura))
        End If

        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        ParsecUtility.Utility.CloseWindow(False)
        '***************************************************************************

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.pannelloFattura.Style.Add("width", "870px")

        Me.StampaFatturaButton.Attributes.Add("onclick", "PrintContent();return false;")
    End Sub

    'Evento LoadComplete: setta il titolo alla griglia degi allegati
    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        Me.DocumentiLabel.Text = "Allegati " & If(Me.DocumentiGridView.MasterTableView.Items.Count > 0, "( " & Me.DocumentiGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "PRIVATI"

    'Restituisce la lista degli allegati presenti nella Fattura Elettromica
    Private Function GetAllegati(ByVal pathFattura As String) As List(Of ParsecPro.AllegatoFattura)

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

    'Legge il file della fattura elettronica: il contenuto viene visualizzato tramite il pannellino
    Private Sub CaricaFattura(ByVal idFattura As Integer, ByVal versioneVisualizzazione As ParsecUtility.VersioneVisualizzazione)
        Try
            Dim pathFileTrasformazione As String = String.Empty
            Dim fatturaElettronicaRepository As New ParsecPro.FatturaElettronicaRepository

            Me.FatturaElettronica = fatturaElettronicaRepository.GetById(idFattura)

            If Not Me.FatturaElettronica Is Nothing Then


                Dim pathFattura As String = String.Empty
                If Me.FatturaElettronica.IdRegistrazione.HasValue Then
                    pathFattura = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & Me.FatturaElettronica.MessaggioSdI.PercorsoRelativo
                Else
                    pathFattura = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
                End If

                pathFileTrasformazione = ParsecAdmin.WebConfigSettings.GetKey("ModelloFatturaElettronica")

                Dim fatturaUtility As New ParsecUtility.FatturaUtility
                Dim fatturahtmlString As String = String.Empty

                Dim path As String = pathFattura & Me.FatturaElettronica.MessaggioSdI.Nomefile
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

                    fatturahtmlString = fatturaUtility.TrasformaFatturaInHtmlString(pathFileTrasformazione, signedCms.ContentInfo.Content, Me.FatturaElettronica.VersioneFattura, versioneVisualizzazione)
                Else
                    fatturahtmlString = fatturaUtility.TrasformaFatturaInHtmlString(pathFileTrasformazione, pathFattura, Me.FatturaElettronica.MessaggioSdI.Nomefile, Me.FatturaElettronica.VersioneFattura, versioneVisualizzazione)
                End If

                If Me.FatturaElettronica.NumeroProtocollo.HasValue Then
                    Dim registrazioni As New ParsecPro.RegistrazioniRepository
                    Dim registrazione = registrazioni.Where(Function(c) c.Id = Me.FatturaElettronica.IdRegistrazione).FirstOrDefault
                    registrazioni.Dispose()
                    If Not registrazione Is Nothing Then
                        fatturahtmlString = fatturahtmlString.Replace("<div id=""NumeroProtocollo""></div>", "<div id=""NumeroProtocollo"">Protocollo n. " & Me.FatturaElettronica.NumeroProtocollo.ToString & " del " & registrazione.DataImmissione.Value.ToShortDateString & "</div>")
                    End If
                End If

                fatturahtmlString = fatturahtmlString.Replace("<div id=""IdentificativoSDI""></div>", "<div id=""IdentificativoSDI"">Ident. SDI " & Me.FatturaElettronica.IdentificativoSdI & "</div>")

                If String.IsNullOrEmpty(fatturahtmlString) Then
                    Throw New Exception("Attenzione: nessuna fattura trovata!")
                Else
                    Me.pannelloFattura.Controls.Clear()
                    Me.pannelloFattura.Controls.Add(New LiteralControl(fatturahtmlString))
                    Me.HtmlFattura = fatturahtmlString
                End If
                fatturaElettronicaRepository.Dispose()
            Else
                fatturaElettronicaRepository.Dispose()
                Throw New Exception("Attenzione: nessuna fattura trovata!")
            End If

        Catch ex As Exception
            Me.pannelloFattura.Controls.Add(New LiteralControl(ex.Message))
        End Try

    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento NeedDataSource associato alla griglia DocumentiGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione AllegatiFattura.
    Protected Sub DocumentiGridView_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DocumentiGridView.NeedDataSource
        If Me.AllegatiFattura Is Nothing Then
            Dim pathFattura As String = String.Empty
            If Me.FatturaElettronica.IdRegistrazione.HasValue Then
                pathFattura = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & Me.FatturaElettronica.MessaggioSdI.PercorsoRelativo & Me.FatturaElettronica.MessaggioSdI.Nomefile
            Else
                pathFattura = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.FatturaElettronica.MessaggioSdI.Nomefile
            End If

            Me.AllegatiFattura = Me.GetAllegati(pathFattura)
        End If
        Me.DocumentiGridView.DataSource = Me.AllegatiFattura
    End Sub

    'Evento ItemCommand associato alla DocumentiGridView. Fa partire i comandi associati alla griglia degli allegati.
    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.DownloadFile(e.Item)
        End If
    End Sub

    'Effettua il download della fattura
    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim pathFattura As String = String.Empty
        If Me.FatturaElettronica.IdRegistrazione.HasValue Then
            pathFattura = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & Me.FatturaElettronica.MessaggioSdI.PercorsoRelativo & Me.FatturaElettronica.MessaggioSdI.Nomefile
        Else
            pathFattura = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.FatturaElettronica.MessaggioSdI.Nomefile
        End If

        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

        Dim allegato As ParsecPro.AllegatoFattura = Me.AllegatiFattura.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then

            Dim file As New IO.FileInfo(pathFattura)
            If file.Exists Then
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
            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    'Evento che richiama la visualizzazione della fattura
    Protected Sub VisualizzaFatturaButton_Click(sender As Object, e As System.EventArgs) Handles VisualizzaFatturaButton.Click
        If VisualizzaFatturaButton.Text = "Tabellare" Then

            Me.CaricaFattura(Me.FatturaElettronica.Id, ParsecUtility.VersioneVisualizzazione.Tabellare)
            Me.VisualizzaFatturaButton.Text = "Classica"
            Me.VisualizzaFatturaButton.Icon.PrimaryIconUrl = "~/images/List.png"   '"../../../../images/List.png"
        Else
            Me.CaricaFattura(Me.FatturaElettronica.Id, ParsecUtility.VersioneVisualizzazione.Classica)
            Me.VisualizzaFatturaButton.Text = "Tabellare"
            Me.VisualizzaFatturaButton.Icon.PrimaryIconUrl = "~/images/Table.png" ' "../../../../images/Table.png"
        End If

    End Sub

    'Chiude il pannellino di visualizzazione della fattura
    Protected Sub ChiudiFatturaButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiFatturaButton.Click
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub

#End Region


End Class