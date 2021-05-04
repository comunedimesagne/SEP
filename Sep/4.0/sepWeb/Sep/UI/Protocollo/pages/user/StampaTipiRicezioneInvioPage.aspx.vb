
'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StampaTipiRicezioneInvioPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Stampa Tipologie Ricezione Invio"
    End Sub

    'Evento LoadComplete associato alla Pagina. Fa partire il metodo Print()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Print()
    End Sub

    'Stampa le Tipologie di ricezione invio
    Private Sub Print()
        Dim tipologieRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaTipologieRicezioneInvio")
        parametriStampa.Add("DatiStampa", tipologieRicezioneInvio.GetView(Nothing))
        Session("ParametriStampaPro") = parametriStampa
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
        tipologieRicezioneInvio.Dispose()
    End Sub

End Class