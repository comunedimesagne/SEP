
'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StampaTipiDocumentoPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Stampa Tipologie Documento"
    End Sub

    'Evento LoadComplete associato alla Pagina. Fa partire il metodo Print()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Print()
    End Sub

    'Stampa le Tipologie di ricezione invio
    Private Sub Print()
        Dim tipologieDocumento As New ParsecPro.TipiDocumentoRepository
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaTipologieDocumento")
        parametriStampa.Add("DatiStampa", tipologieDocumento.GetView(Nothing))
        Session("ParametriStampaPro") = parametriStampa
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
        tipologieDocumento.Dispose()
    End Sub

End Class