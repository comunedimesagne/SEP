Partial Class StampaPage
    Inherits System.Web.UI.Page

    Private ParametriStampa As Hashtable = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tipologiaStampa As String = String.Empty
        If Not Session("ParametriStampa") Is Nothing Then
            Me.ParametriStampa = Session("parametriStampa")
            tipologiaStampa = ParametriStampa("TipologiaStampa")
            Select Case tipologiaStampa
                Case "StampaBozze"
                    Me.StampaBozze(ParametriStampa("DatiStampa"))
                Case "StampaRegistroGeneraleDetermine"
                    Me.StampaRegistroGeneraleDetermine(ParametriStampa("DatiStampa"))
                Case "StampaRegistroGeneraleDelibera"
                    Me.StampaRegistroGeneraleDelibere(ParametriStampa("DatiStampa"))
                Case "StampaRegistroGeneraleDecreto"
                    Me.StampaRegistroGeneraleOrdinanze(ParametriStampa("DatiStampa"), False)
                Case "StampaRegistroGeneraleOrdinanza"
                    Me.StampaRegistroGeneraleOrdinanze(ParametriStampa("DatiStampa"), True)
                Case "StampaRegistroSettoreDetermine"
                    Me.StampaRegistroSettoreDetermine(ParametriStampa("DatiStampa"))
                Case "StampaElencoDetermineImpegnoSpesa"
                    Me.StampaElencoDetermineImpegnoSpesa(ParametriStampa("DatiStampa"))
                Case "StampaElencoDeterminePubblicazione"
                    Me.StampaElencoDeterminePubblicazione(ParametriStampa("DatiStampa"))
                Case "StampaElencoDetermineLiquidazione"
                    Me.StampaElencoDetermineLiquidazione(ParametriStampa("DatiStampa"))
                Case "StampaElencoDelibereImpegnoSpesa"
                    Me.StampaElencoDelibereImpegnoSpesa(ParametriStampa("DatiStampa"))
                Case "StampaElencoDeliberePubblicazione"
                    Me.StampaElencoDeliberePubblicazione(ParametriStampa("DatiStampa"))
                Case "StampaSchedaControllo"
                    Me.StampaSchedaControllo(ParametriStampa("DatiStampa"))
                Case Else
                    ParsecUtility.Utility.MessageBox("Tipologia di Stampa sconosciuta!", False)
            End Select
            Session("ParametriStampa") = Nothing
        Else
            tipologiaStampa = Request.QueryString("TipologiaStampa")
            If tipologiaStampa Is Nothing Then ParsecUtility.Utility.MessageBox("Tipologia di Stampa sconosciuta!", False)
        End If
    End Sub

    Private Function GetCliente() As ParsecAdmin.Cliente
        Dim clientRepository As New ParsecAdmin.ClientRepository
        Dim cliente As ParsecAdmin.Cliente = clientRepository.GetQuery.FirstOrDefault
        clientRepository.Dispose()
        Return cliente
    End Function

    Private Sub StampaBozze(ByVal dataSource As Object)
        Dim r As ParsecReporting.BozzeReport = ParsecReporting.ReportManager.CreaStampaBozze(dataSource)
        r.Titolo = "ELENCO BOZZE"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaRegistroGeneraleDetermine(ByVal dataSource As Object)
        Dim r As ParsecReporting.RegistroGeneraleDetermineReport = ParsecReporting.ReportManager.CreaStampaRegistroGeneraleDetermine(dataSource)
        r.Titolo = "REGISTRO GENERALE DETERMINE"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaRegistroSettoreDetermine(ByVal dataSource As Object)
        Dim r As ParsecReporting.RegistroSettoreDetermineReport = ParsecReporting.ReportManager.CreaStampaRegistroSettoreDetermine(dataSource)
        r.Titolo = "REGISTRO SETTORE DETERMINE"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaElencoDetermineImpegnoSpesa(ByVal dataSource As Object)
        Dim r As ParsecReporting.ElencoDetermineImpegnoSpesaReport = ParsecReporting.ReportManager.CreaStampaElencoDetermineImpegnoSpesa(dataSource)
        r.Titolo = "ELENCO DETERMINE (IMPEGNO SPESA)"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaElencoDeterminePubblicazione(ByVal dataSource As Object)
        Dim r As ParsecReporting.ElencoDeterminePubblicazioneReport = ParsecReporting.ReportManager.CreaStampaElencoDeterminePubblicazione(dataSource)
        r.Titolo = "ELENCO DETERMINE (PUBBLICAZIONE)"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaElencoDetermineLiquidazione(ByVal dataSource As Object)
        Dim r As ParsecReporting.ElencoDetermineLiquidazioneReport = ParsecReporting.ReportManager.CreaStampaElencoDetermineLiquidazione(dataSource)
        r.Titolo = "ELENCO DETERMINE (LIQUIDAZIONE)"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaElencoDelibereImpegnoSpesa(ByVal dataSource As Object)
        Dim r As ParsecReporting.ElencoDelibereImpegnoSpesaReport = ParsecReporting.ReportManager.CreaStampaElencoDelibereImpegnoSpesa(dataSource)
        r.Titolo = "ELENCO DELIBERE (IMPEGNO SPESA)"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaElencoDeliberePubblicazione(ByVal dataSource As Object)
        Dim r As ParsecReporting.ElencoDetermineLiquidazioneReport = ParsecReporting.ReportManager.CreaStampaElencoDeliberePubblicazioni(dataSource)
        r.Titolo = "ELENCO DELIBERE (PUBBLICAZIONE)"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaRegistroGeneraleDelibere(ByVal dataSource As Object)
        Dim r As ParsecReporting.RegistroGeneraleDelibereReport = ParsecReporting.ReportManager.CreaStampaRegistroGeneraleDelibere(dataSource)
        r.Titolo = "REGISTRO GENERALE DELIBERE"
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaRegistroGeneraleOrdinanze(ByVal dataSource As Object, ordinanza As Boolean)
        Dim r As ParsecReporting.RegistroGeneraleOrdinanzeReport = ParsecReporting.ReportManager.CreaStampaRegistroGeneraleOrdinanze(dataSource)
        If ordinanza Then
            r.Titolo = "REGISTRO GENERALE ORDINANZE"
        Else
            r.Titolo = "REGISTRO GENERALE DECRETI"
        End If
        r.RagioneSociale = GetCliente.Descrizione
        ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

    Private Sub StampaSchedaControllo(ByVal dataSource As Object)
        'Dim r As ParsecReporting.SchedaControlloReport = ParsecReporting.ReportManager.CreaStampaSchedaControllo(dataSource)
        'Dim es As ParsecAtt.EtichettaScheda = CType(dataSource, ParsecAtt.EtichettaScheda)
        'With r
        '    .Titolo = "CONTROLLO SUCCESSIVO DI REGOLARITA' AMMINISTRATIVA"
        '    .RagioneSociale = GetCliente.Descrizione & " ( " & GetCliente.Provincia & " ) "
        '    .DatoFiscale = GetCliente.PIVA
        '    .Indirizzo = GetCliente.Indirizzo & " - " & GetCliente.Citta & " ( " & GetCliente.Provincia & " ) - " & GetCliente.CAP
        'End With
        'ParsecReporting.ReportManager.ExportToPDF(r)
    End Sub

End Class