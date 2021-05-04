Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Xml.Schema
Imports System.Xml.Xsl
Imports System.Xml.Xsl.XslCompiledTransform

'* SPDX-License-Identifier: GPL-3.0-only

'Maschera Utilizzata dall'Iter
Partial Class NotificaFatturaPage
    Inherits System.Web.UI.Page

#Region "PROPRIETA' PAGINA"

    'Variabile di Sessione: oggetto FatturaElettronica corrente.
    Public Property FatturaElettronica As ParsecPro.FatturaElettronica
        Get
            Return CType(Session("NotificaFatturaPage_FatturaElettronica"), ParsecPro.FatturaElettronica)
        End Get
        Set(ByVal value As ParsecPro.FatturaElettronica)
            Session("NotificaFatturaPage_FatturaElettronica") = value
        End Set
    End Property

    'Variabile di Sessione: oggetto rappresentante la fattura in html.
    Public Property HtmlFattura As String
        Get
            Return CType(Session("NotificaFatturaPage_HtmlFattura"), String)
        End Get
        Set(ByVal value As String)
            Session("NotificaFatturaPage_HtmlFattura") = value
        End Set
    End Property

    'Variabile di Sessione: oggetto rappresentante il path della fattura.
    Public Property PathFattura As String
        Get
            Return CType(Session("NotificaFatturaPage_PathFattura"), String)
        End Get
        Set(ByVal value As String)
            Session("NotificaFatturaPage_PathFattura") = value
        End Set
    End Property

    'Variabile di Sessione: oggetto rappresentante il path dei file di trasformazione della fattura.
    Public Property PathFileTrasformazione As String
        Get
            Return CType(Session("NotificaFatturaPage_PathFileTrasformazione"), String)
        End Get
        Set(ByVal value As String)
            Session("NotificaFatturaPage_PathFileTrasformazione") = value
        End Set
    End Property

    'Variabile di Sessione: oggetto rappresentante gli allegati della fattura.
    Public Property AllegatiFattura As List(Of ParsecPro.AllegatoFattura)
        Get
            Return CType(Session("NotificaFatturaPage_AllegatiFattura"), List(Of ParsecPro.AllegatoFattura))
        End Get
        Set(ByVal value As List(Of ParsecPro.AllegatoFattura))
            Session("NotificaFatturaPage_AllegatiFattura") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then
            Me.HtmlFattura = Nothing
            Me.PathFattura = Nothing
            Me.PathFileTrasformazione = Nothing
            Me.AllegatiFattura = Nothing

            If Not Me.Request.QueryString("IdFatturaElettronica") Is Nothing Then
                Dim idFatturaElettronica = Me.Request.QueryString("IdFatturaElettronica")
                caricaFattura(idFatturaElettronica)
                Me.PathFattura = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & Me.FatturaElettronica.MessaggioSdI.PercorsoRelativo
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

    End Sub

#End Region

#Region "PRIVATI"

    'Restituisce la lista degli allegati associati alla fattura.
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


    'Metodo che carica la fattura e la associa al pannello per la Visualizazzione in html.
    'Parametri in Input: idFatura da Visualizzaree trattare
    Private Sub caricaFattura(ByVal idFattura As Integer)
        Try
            Dim fatturaElettronicaRepository As New ParsecPro.FatturaElettronicaRepository
            Me.FatturaElettronica = fatturaElettronicaRepository.GetById(idFattura)

            If Not Me.FatturaElettronica Is Nothing Then

                Dim residuo = (Me.FatturaElettronica.MessaggioSdI.DataRicezioneInvio.AddDays(16) - Now).Days
                If residuo < 1 Then
                    Me.cmdEsitoNegativo.Enabled = False
                    Me.cmdEsitoPositivo.Enabled = False
                    Me.NotificataScadutaButton.Enabled = True
                Else
                    Me.cmdEsitoNegativo.Enabled = True
                    Me.cmdEsitoPositivo.Enabled = True
                    Me.NotificataScadutaButton.Enabled = True
                End If

                PathFattura = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & Me.FatturaElettronica.MessaggioSdI.PercorsoRelativo
                PathFileTrasformazione = ParsecAdmin.WebConfigSettings.GetKey("ModelloFatturaElettronica")

                Dim fatturaUtility As New ParsecUtility.FatturaUtility
                Dim fatturahtmlString As String = String.Empty

                Dim path As String = PathFattura & Me.FatturaElettronica.MessaggioSdI.Nomefile
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

                    fatturahtmlString = fatturaUtility.TrasformaFatturaInHtmlString(PathFileTrasformazione, signedCms.ContentInfo.Content, Me.FatturaElettronica.VersioneFattura)
                Else
                    fatturahtmlString = fatturaUtility.TrasformaFatturaInHtmlString(PathFileTrasformazione, PathFattura, Me.FatturaElettronica.MessaggioSdI.Nomefile, Me.FatturaElettronica.VersioneFattura)
                End If

                If (fatturahtmlString Is Nothing) Then
                    Throw New Exception("Attenzione: nessuna fattura trovata!")
                Else
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
            Me.cmdEsitoNegativo.Enabled = False
            Me.cmdEsitoPositivo.Enabled = False
            Me.NotificataScadutaButton.Enabled = False
        End Try
    End Sub

    'Evento che genera una Notifica Positiva: EC01
    Protected Sub cmdEsitoPositivo_Click(sender As Object, e As System.EventArgs) Handles cmdEsitoPositivo.Click
        Try
            If (Me.FatturaElettronica Is Nothing) Then
                Throw New Exception("Riscontrati problemi: nessuna fattura trovata!")
            Else
                Me.generaEsito("EC01")
                ParsecUtility.SessionManager.NotificaEsitoCommittente = New ParsecPro.NotificaEsitoCommittente With {.Esito = ParsecPro.TipologiaEsitoCommittente.Accettazione}
            End If
        Catch ex As Exception
            Me.pannelloFattura.Controls.Add(New LiteralControl(ex.Message))
        End Try
    End Sub

    'Evento che genera una Notifica Positiva: EC02
    Protected Sub cmdEsitoNegativo_Click(sender As Object, e As System.EventArgs) Handles cmdEsitoNegativo.Click
        Try
            If (Me.FatturaElettronica Is Nothing) Then
                Throw New Exception("Riscontrati problemi: nessuna fattura trovata!")
            Else

                Me.generaEsito("EC02")

                ParsecUtility.SessionManager.NotificaEsitoCommittente = New ParsecPro.NotificaEsitoCommittente With {.Esito = ParsecPro.TipologiaEsitoCommittente.Rfiutato}

            End If

        Catch ex As Exception
            Me.pannelloFattura.Controls.Add(New LiteralControl(ex.Message))
        End Try
    End Sub

    'Evento Click che permette di anadare avanti nell'iter senza generare un esito
    Protected Sub NotificataScadutaButton_Click(sender As Object, e As System.EventArgs) Handles NotificataScadutaButton.Click
        ParsecUtility.SessionManager.ListaNotificheFatturazione = New List(Of String)
        ParsecUtility.SessionManager.ListaNotificheFatturazione.Add("")
        ParsecUtility.SessionManager.ListaNotificheFatturazione.Add("")
        ParsecUtility.SessionManager.NotificaEsitoCommittente = New ParsecPro.NotificaEsitoCommittente With {.Esito = ParsecPro.TipologiaEsitoCommittente.AccettazioneSenzaNotifica}
        Me.infoOperazioneHidden.Value = "Operazione eseguita con successo!"
    End Sub

    'MGenera l'esito di scarto o accettazione di una fattura
    Private Sub generaEsito(ByVal codiceEsito As String)
        Try
            Dim fatturaUtility As New ParsecUtility.FatturaUtility
            Dim notifiche As New ParsecPro.NotificaSdIRepository
            Dim numeroNotifiche = notifiche.GetNumeroNotifiche(Me.FatturaElettronica.Id, ParsecPro.TipologiaMessaggioSdI.NotificaEsitoCessionarioCommittente)


            If Me.FatturaElettronica.MessaggioSdI.Nomefile.ToLower.EndsWith(".p7m") Then
                Dim path As String = Me.PathFattura & Me.FatturaElettronica.MessaggioSdI.Nomefile
                Dim buffer As Byte() = IO.File.ReadAllBytes(path)
                Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
                'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
                Try
                    buffer = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(buffer))
                Catch ex As Exception
                    'NIENTE
                End Try
               
                signedCms.Decode(buffer)
                fatturaUtility.generaNotificaEsito(PathFileTrasformazione, Me.PathFattura, Me.FatturaElettronica.MessaggioSdI.Nomefile, signedCms.ContentInfo.Content, Me.FatturaElettronica.IdentificativoSdI, Me.FatturaElettronica.NumeroFatture, codiceEsito, txtMessaggio.Text, numeroNotifiche)

            Else
                fatturaUtility.generaNotificaEsito(PathFileTrasformazione, Me.PathFattura, Me.FatturaElettronica.MessaggioSdI.Nomefile, Me.FatturaElettronica.IdentificativoSdI, Me.FatturaElettronica.NumeroFatture, codiceEsito, txtMessaggio.Text, numeroNotifiche)
            End If


            resettaView()
            notifiche.Dispose()
            Me.infoOperazioneHidden.Value = "Notifica generata con successo!"

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    'Verifica la validità del numero di fattura.
    Private Function VerificaNumeroFattura(ByVal numeroFattura As String) As Boolean
        Dim pattern As String = "\b(\p{IsBasicLatin}{1,20})"
        Return Regex.IsMatch(numeroFattura, pattern)
    End Function

    'Evento che chiude il pop-up
    Protected Sub cmdChiudi_Click(sender As Object, e As System.EventArgs) Handles cmdChiudi.Click
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub

    'Metodo che resetta la View
    Private Sub resettaView()
        Me.txtMessaggio.Text = String.Empty
    End Sub

#End Region

    'Evento NeedDataSource associato alla griglia DocumentiGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione AllegatiFattura.
    Protected Sub DocumentiGridView_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DocumentiGridView.NeedDataSource
        If Me.AllegatiFattura Is Nothing Then
            Me.AllegatiFattura = Me.GetAllegatiFattura(Me.PathFattura & Me.FatturaElettronica.MessaggioSdI.Nomefile)
        End If
        Me.DocumentiGridView.DataSource = Me.AllegatiFattura
    End Sub

    'Lancia i comandi dalla griglia: in questo caso scarica la fattura
    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.DownloadFile(e.Item)
        End If
    End Sub

    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim path As String = Me.PathFattura & Me.FatturaElettronica.MessaggioSdI.Nomefile

        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

        Dim allegato As ParsecPro.AllegatoFattura = Me.AllegatiFattura.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then
          
            Dim file As New IO.FileInfo(path)
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

    'Evento LoadComplete: setta il titolo sulla lista degli allegati
    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        Me.DocumentiLabel.Text = "Allegati " & If(Me.DocumentiGridView.MasterTableView.Items.Count > 0, "( " & Me.DocumentiGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

  
End Class