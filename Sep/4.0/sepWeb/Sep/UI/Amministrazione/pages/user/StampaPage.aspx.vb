Partial Class StampaPage
    Inherits System.Web.UI.Page

    Private ParametriStampa As Hashtable = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tipologiaStampa As String = String.Empty
        If Not Session("ParametriStampa") Is Nothing Then
            Me.ParametriStampa = Session("parametriStampa")
            tipologiaStampa = ParametriStampa("TipologiaStampa")
            Select Case tipologiaStampa
                Case "StampaUtenti"
                    StampaUtenti(ParametriStampa("DatiStampa"))
                Case "StampaProfili"
                    Me.StampaProfili(ParametriStampa("DatiStampa"))
                Case "StampaQualificheOrganigramma"
                    Me.StampaQualificheOrganigramma(ParametriStampa("DatiStampa"))
                Case "ScrivaniaStorico"
                    StampaStorico(ParametriStampa("DatiStampa"))
                Case "VisualizzaIstanzaStorico"
                    StampaElencoUtentiIstanza(ParametriStampa("DatiStampa"), Me.ParametriStampa("Riferimento"))
                Case "IterStorico"
                    StampaTaskIter(ParametriStampa("DatiStampa"), Me.ParametriStampa("Titolo"))
                Case "StampaDeleghe"
                    StampaDeleghe(ParametriStampa("DatiStampa"))
                Case "StampaElencoProcedimenti"
                    StampaElencoProcedimenti(ParametriStampa("DatiStampa"))
                Case "StampaStatisticheProcedimenti"
                    StampaStatisticheProcedimenti(ParametriStampa("DatiStampa"))
                Case "StampaRegistri"
                    StampaRegistri(ParametriStampa("DatiStampa"), Me.ParametriStampa("Titolo"))
            End Select
            Session("ParametriStampa") = Nothing
        Else
            tipologiaStampa = Request.QueryString("TipologiaStampa")
        End If
    End Sub


    Private Function GetCliente() As ParsecAdmin.Cliente
        Dim clientRepository As New ParsecAdmin.ClientRepository
        Dim cliente As ParsecAdmin.Cliente = clientRepository.GetQuery.FirstOrDefault
        clientRepository.Dispose()
        Return cliente
    End Function

    Private Sub StampaUtenti(ByVal dataSource As Object)
        Dim r As ParsecReporting.UtentiReport = ParsecReporting.ReportManager.CreaStampaUtenti(dataSource)
        r.Titolo = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaProfili(ByVal dataSource As Object)
        Dim r As ParsecReporting.ProfiliReport = ParsecReporting.ReportManager.CreaStampaProfili(dataSource)
        r.Titolo = "ELENCO PROFILI"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaQualificheOrganigramma(ByVal dataSource As Object)
        Dim r As ParsecReporting.QualificheOrganigrammaReport = ParsecReporting.ReportManager.CreaStampaQualificheOrganigramma(dataSource)
        r.Titolo = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaStorico(ByVal dataSource As Object)
        Dim r As ParsecReporting.StoricoReport = ParsecReporting.ReportManager.CreaStampaStorico(dataSource)
        r.Titolo = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaElencoUtentiIstanza(ByVal dataSource As Object, ByVal titolo As String)
        Dim r As ParsecReporting.VisualizzazioneIstanzaReport = ParsecReporting.ReportManager.CreaStampaVisualizzazioneIstanza(dataSource)
        r.Titolo = titolo
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaTaskIter(ByVal dataSource As Object, ByVal titolo As String)
        Dim r As ParsecReporting.TaskIstanzaReport = ParsecReporting.ReportManager.CreaStampaVisualizzazioneIterIstanza(dataSource)
        r.Titolo = titolo
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaDeleghe(ByVal dataSource As Object)
        Dim r As ParsecReporting.DelegheReport = ParsecReporting.ReportManager.CreaStampaDeleghe(dataSource)
        r.Titolo = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaElencoProcedimenti(ByVal dataSource As Object)
        Dim r As ParsecReporting.ElencoProcedimentiReport = ParsecReporting.ReportManager.CreaStampaElencoProcedimenti(dataSource)
        r.Titolo = "ELENCO PROCEDIMENTI"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaStatisticheProcedimenti(ByVal dataSource As Object)
        Dim r As ParsecReporting.StatisticheProcedimentiReport = ParsecReporting.ReportManager.CreaStampaStatisticheProcedimenti(dataSource)
        r.Titolo = "STATISTICHE PROCEDIMENTI"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaRegistri(ByVal dataSource As Object, ByVal titolo As String)
        Dim r As ParsecReporting.ElencoRegistriReport = ParsecReporting.ReportManager.CreaStampaElencoRegistri(dataSource)
        r.Titolo = titolo
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

End Class