Imports ParsecAdmin

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class SbloccoSistemaProtocolloPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    'Variabile di Sessione: oggetto TipoDocumento corrente.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Sblocco Sistema Protocollo"

        Dim esercizi As New ParsecPro.EsercizioRepository
        If Not esercizi.CheckBloccoProtocollo Then
            MessaggioLabel.Text = "Il servizio di protocollazione è già SBLOCCATO!"
            Me.SalvaButton.Enabled = False
        End If
        esercizi.Dispose()
        Me.SalvaButton.Attributes.Add("onclick", "this.disabled=true;")
    End Sub

    'Metodo che salva su database lo sblocco degli anni di esercizio. Invocato da SalvaButton.Click
    Private Sub Save()
        Dim esercizi As New ParsecPro.EsercizioRepository
        Try
            esercizi.AggiornaServizioProtocollazione(False)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            esercizi.Dispose()
        End Try
    End Sub

    'Evento click che lancia il salvataggio delle informazioni
    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        SalvaButton.Enabled = False
        Dim message As String = "Operazione conclusa con successo!"
        Try
            Me.Save()
        Catch ex As Exception
            message = ex.Message
            SalvaButton.Enabled = True
        Finally
            ParsecUtility.Utility.MessageBox(message, False)
            MessaggioLabel.Text = "Il servizio di protocollazione è stato SBLOCCATO."
        End Try
    End Sub



End Class
