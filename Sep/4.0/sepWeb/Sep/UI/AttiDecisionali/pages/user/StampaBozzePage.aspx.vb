Partial Class StampaBozzePage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Stampa Bozze"
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        'Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region


#Region "EVENTI CONTROLLI"

    Protected Sub StampaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaButton.Click
        Dim filtro As ParsecAtt.FiltroBozza = Me.GetFiltro
        Dim bozze As New ParsecAtt.BozzeRepository
        Dim view As List(Of ParsecAtt.Bozza) = bozze.GetView(filtro)
        bozze.Dispose()
        Me.Print(view)
    End Sub

    Protected Sub AnnullaButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub ResettaFiltro()
        Me.NumeroInizioTextBox.Text = String.Empty
        Me.NumeroFineTextBox.Text = String.Empty
        Me.DataInizioTextBox.SelectedDate = Nothing
        Me.DataFineTextBox.SelectedDate = Nothing
    End Sub

    Private Sub Print(ByVal bozze As List(Of ParsecAtt.Bozza))
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaBozze")
        parametriStampa.Add("DatiStampa", bozze)
        Session("parametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub


    Private Function GetFiltro() As ParsecAtt.FiltroBozza
        Dim filtro As New ParsecAtt.FiltroBozza

        Dim numeroInizio As Integer = 0
        If Not String.IsNullOrEmpty(Me.NumeroInizioTextBox.Text) Then
            Integer.TryParse(Me.NumeroInizioTextBox.Text, numeroInizio)
            If numeroInizio <> 0 Then
                filtro.NumeroInizio = numeroInizio
            End If
        End If

        Dim numeroFine As Integer = 0
        If Not String.IsNullOrEmpty(Me.NumeroFineTextBox.Text) Then
            Integer.TryParse(Me.NumeroFineTextBox.Text, numeroFine)
            If numeroFine <> 0 Then
                filtro.NumeroFine = numeroFine
            End If
        End If

        filtro.DataInizio = Me.DataInizioTextBox.SelectedDate
        filtro.DataFine = Me.DataFineTextBox.SelectedDate

        Return filtro
    End Function


#End Region




End Class
