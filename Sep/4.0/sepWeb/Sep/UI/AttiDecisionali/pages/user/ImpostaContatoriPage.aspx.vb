Imports ParsecAdmin


Partial Class ImpostaContatoriPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Private annoCorrente As Integer = 0
    Private annoSuccessivo As Integer = 0



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Passaggio a nuovo anno"

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AnnoCorrente", ParsecAdmin.TipoModulo.ATT)

        Me.annoCorrente = -1
        If Not parametro Is Nothing Then
            Integer.TryParse(parametro.Valore, Me.annoCorrente)
        End If

        parametri.Dispose()


        If Me.annoCorrente <> -1 Then

            Dim contatoriGenerali As New ParsecAtt.ContatoreGeneraleRepository

            Dim exist As Boolean = contatoriGenerali.GetQuery.Where(Function(c) c.Anno = Me.annoCorrente).Count > 0

            Me.annoSuccessivo = Year(Now)
            Me.AnnoCorrenteLabel.Text = "Anno corrente: " & Me.annoCorrente.ToString

            'If Me.annoSuccessivo > Me.annoCorrente Then
            If Not exist Then
                Me.AnnoSuccessivoLabel.Text = "Premere il tasto Ok per generare i contatori per l'anno " & Me.annoSuccessivo.ToString & "."
            Else
                Me.SalvaButton.Enabled = False
                Me.AnnoSuccessivoLabel.Text = "I contatori, per l'anno " & annoCorrente.ToString & " sono già stati generati!"
            End If

            contatoriGenerali.Dispose()
        Else
            Me.AnnoSuccessivoLabel.Text = "Contattare l'amministratore, non è definito l'anno di esercizio per il modulo Atti Decisionali."
            Me.SalvaButton.Enabled = False
        End If

        Me.SalvaButton.Attributes.Add("onclick", "this.disabled=true;")
    End Sub



    Private Sub Save()
        Dim contatoriGenerali As New ParsecAtt.ContatoreGeneraleRepository
        Dim contatoriSettori As New ParsecAtt.ContatoreSettoreRepository
        Try
            contatoriGenerali.AggiornaAnnoEsercizio(annoSuccessivo)
            contatoriSettori.AggiornaAnnoEsercizio(annoSuccessivo)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            contatoriGenerali.Dispose()
            contatoriSettori.Dispose()
        End Try
    End Sub


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
            Me.AnnoSuccessivoLabel.Text = "I contatori, per l'anno " & annoSuccessivo.ToString & " sono stati generati!"
        End Try
    End Sub

End Class