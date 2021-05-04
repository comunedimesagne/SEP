Partial Class StampaPage
    Inherits System.Web.UI.Page

    Private ParametriStampa As Hashtable = Nothing

    'Evento Init associato alla Pagina: inizializza la pagina. Ed in base alla tiplogia di stampa passato tramite ParametriStampa("TipologiaStampa") fa partire la stampa corretta.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tipologiaStampa As String = String.Empty
        If Not Session("ParametriStampaPro") Is Nothing Then
            Me.ParametriStampa = Session("ParametriStampaPro")
            tipologiaStampa = ParametriStampa("TipologiaStampa")
            Select Case tipologiaStampa
                Case "StampaTipologieDocumento"
                    Me.StampaTipologieDocumento(ParametriStampa("DatiStampa"))
                Case "StampaTipologieRicezioneInvio"
                    Me.StampaTipologieRicezioneInvio(ParametriStampa("DatiStampa"))
                Case "StampaElencoRegistrazioni"
                    Me.StampaElencoRegistrazioni(ParametriStampa("DatiStampa"))
                Case "StampaRicevutaRegistrazione"
                    Me.StampaRicevutaRegistrazione(ParametriStampa("DatiStampa"))
                Case "StampaEtichette"
                    Me.StampaEtichette()
                Case "StampaFattura"
                    Me.StampaFattura()
                Case "StampaElencoRegistrazioniDiretta"
                    Me.StampaFattura()
                Case "StampaElencoDocumentiFascicolo"
                    Me.StampaFattura()

            End Select
            Session("ParametriStampaPro") = Nothing
        Else
            tipologiaStampa = Request.QueryString("TipologiaStampa")
        End If
    End Sub

    'Recupera le informazioni dell'Ente configurato
    Private Function GetCliente() As ParsecAdmin.Cliente
        Dim clientRepository As New ParsecAdmin.ClientRepository
        Dim cliente As ParsecAdmin.Cliente = clientRepository.GetQuery.FirstOrDefault
        clientRepository.Dispose()
        Return cliente
    End Function

    'Stampa l'Elenco Tipologie di Documento
    Private Sub StampaTipologieDocumento(ByVal dataSource As Object)
        Dim r As ParsecReporting.TipologieDocumento = ParsecReporting.ReportManager.CreaStampaTipologieDocumento(dataSource)
        r.Titolo = "Elenco Tipologie di Documento"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)

    End Sub

    'Stampa l'Elenco Tipologie di Invio\Ricezione
    Private Sub StampaTipologieRicezioneInvio(ByVal dataSource As Object)
        Dim r As ParsecReporting.TipiRicezioneInvioReport = ParsecReporting.ReportManager.CreaStampaTipologieRicezioneInvio(dataSource)
        r.Titolo = "Elenco Tipologie di Invio\Ricezione"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    'Stampa il Registro di Protocollo
    Private Sub StampaElencoRegistrazioni(ByVal dataSource As Object)
        Dim r As ParsecReporting.StampaElencoRegistrazioniReport = ParsecReporting.ReportManager.CreaStampaElencoRegistrazioni(dataSource)
        r.Titolo = "Registro Protocollo"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    'Stampa la Ricevuta Protocollo
    Private Sub StampaRicevutaRegistrazione(ByVal dataSource As Object)
        Dim r As ParsecReporting.RicevutaProtocolloReport = ParsecReporting.ReportManager.CreaStampaRicevutaRegistrazione(dataSource)
        r.Titolo = "Ricevuta Protocollo"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    'Stampa le Etichette
    Private Sub StampaEtichette()
        Dim fullPath As String = ParametriStampa("FullPath")
        Dim bytes As Byte() = IO.File.ReadAllBytes(fullPath)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.ContentType = "application/pdf"
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Private)
        HttpContext.Current.Response.Expires = -1
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.BinaryWrite(bytes)
        HttpContext.Current.Response.End()
    End Sub

    'Stampa la fattura
    Private Sub StampaFattura()
        Dim bytes As Byte() = ParametriStampa("FullPath")
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.ContentType = "application/pdf"
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Private)
        HttpContext.Current.Response.Expires = -1
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.BinaryWrite(bytes)
        HttpContext.Current.Response.End()
    End Sub

End Class
