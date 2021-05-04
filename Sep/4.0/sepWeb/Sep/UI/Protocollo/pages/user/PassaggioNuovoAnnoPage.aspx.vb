Imports ParsecAdmin

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class PassaggioNuovoAnnoPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Private annoCorrente As Integer = 0
    Private annoSuccessivo As Integer = 0

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Passaggio a nuovo anno"

        Dim esercizi As New ParsecPro.EsercizioRepository
        Me.annoCorrente = esercizi.GetAnnoEsercizioCorrente
        If Me.annoCorrente <> -1 Then
            Me.annoSuccessivo = Year(Now)
            Me.AnnoCorrenteLabel.Text = "Anno corrente: " & Me.annoCorrente.ToString
            If Me.annoSuccessivo > Me.annoCorrente Then
                Me.AnnoSuccessivoLabel.Text = "Premere il tasto Ok per impostare come corrente l'anno " & Me.annoSuccessivo.ToString & "."
            Else
                Me.SalvaButton.Enabled = False
            End If
        Else
            Me.AnnoSuccessivoLabel.Text = "Contattare l'amministratore, non è definito l'anno di esercizio per il protocollo."
            Me.SalvaButton.Enabled = False
        End If
        esercizi.Dispose()
        Me.SalvaButton.Attributes.Add("onclick", "this.disabled=true;")
    End Sub

    'Metod che salva su DB. Richiamato da SalvaButton_Click
    Private Sub Save()
        Dim esercizi As New ParsecPro.EsercizioRepository
        Try
            esercizi.AggiornaAnnoEsercizio(annoSuccessivo)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            esercizi.Dispose()
        End Try
    End Sub

    'Evento click che fa partire il Salvataggio
    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Me.SalvaButton.Enabled = False
        Dim message As String = "Operazione conclusa con successo!"
        Try
            Me.Save()
        Catch ex As Exception
            message = ex.Message
            Me.SalvaButton.Enabled = True
        Finally
            ParsecUtility.Utility.MessageBox(message, False)
            Me.AnnoSuccessivoLabel.Text = ""
            Me.AnnoCorrenteLabel.Text = "Anno corrente: " & Me.annoSuccessivo.ToString
        End Try
    End Sub

End Class