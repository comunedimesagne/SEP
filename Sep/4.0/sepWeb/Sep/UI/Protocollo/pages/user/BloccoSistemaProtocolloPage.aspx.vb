Imports ParsecAdmin

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class BloccoSistemaProtocolloPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Blocco Sistema Protocollo"

        Dim esercizi As New ParsecPro.EsercizioRepository
        If esercizi.CheckBloccoProtocollo Then
            MessaggioLabel.Text = "Il servizio di protocollazione è già BLOCCATO!"

            Me.SalvaButton.Enabled = False
        End If
        esercizi.Dispose()
        Me.SalvaButton.Attributes.Add("onclick", "this.disabled=true;")
    End Sub

    'Metodo che effettua il Salvataggio su DB.
    Private Sub Save()
        Dim esercizi As New ParsecPro.EsercizioRepository
        Try
            esercizi.AggiornaServizioProtocollazione(True)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            esercizi.Dispose()
        End Try
    End Sub

    'Evento click per il Salvataggio.
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
            MessaggioLabel.Text = "Il servizio di protocollazione è stato BLOCCATO."
        End Try
    End Sub



End Class
