Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class MonitoraggioPrestazioniServerPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage


#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Monitoraggio Prestazioni Server"
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ReadTimeButton_Click(sender As Object, e As System.EventArgs) Handles ReadTimeButton.Click

        Dim timer As New System.Diagnostics.Stopwatch
        timer.Start()
        Dim testTables As New ParsecAdmin.TestTableRepository
        Dim tables = testTables.GetQuery.ToList
        testTables.Dispose()
        timer.Stop()
        Me.TempoLetturaLabel.Text = String.Format("{0} ms.", timer.ElapsedMilliseconds)
    End Sub

    Protected Sub WriteTimeButton_Click(sender As Object, e As System.EventArgs) Handles WriteTimeButton.Click

        Dim timer As New System.Diagnostics.Stopwatch
        timer.Start()
        Dim testTables As New ParsecAdmin.TestTableRepository
        If Me.NumeroTextBox.Value = 0 OrElse Not NumeroTextBox.Value.HasValue Then
            timer.Stop()
            Exit Sub
        End If
        Dim item As ParsecAdmin.TestTable
        For i As Integer = 1 To NumeroTextBox.Value
            item = New ParsecAdmin.TestTable With
                   {
                        .CampoStringa1 = "Test Inserimento CampoStringa1 riga" + i.ToString,
                        .CampoStringa2 = "Test Inserimento CampoStringa2 riga" + i.ToString,
                        .CampoStringa3 = "Test Inserimento CampoStringa3 riga" + i.ToString,
                        .CampoStringa4 = "Test Inserimento CampoStringa4 riga" + i.ToString,
                        .CampoStringa5 = "Test Inserimento CampoStringa5 riga" + i.ToString,
                        .CampoInt1 = i + 1,
                        .CampoInt2 = i + 2,
                        .CampoInt3 = i + 3,
                        .CampoInt4 = i + 4,
                        .CampoInt5 = i + 5,
                        .CampoData1 = Now,
                        .CampoData2 = Now.AddMinutes(1),
                        .CampoData3 = Now.AddMinutes(2)
                    }



            testTables.Add(item)
            testTables.SaveChanges()
        Next

        testTables.Dispose()
        timer.Stop()

        Me.TempoScritturaLabel.Text = String.Format("{0} ms.", timer.ElapsedMilliseconds)
    End Sub

    Protected Sub LoadTimeButton_Click(sender As Object, e As System.EventArgs) Handles LoadTimeButton.Click
        Me.TestGridView.Rebind()
    End Sub
  
#End Region


    Protected Sub TestGridView_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles TestGridView.NeedDataSource
        Dim timer As New System.Diagnostics.Stopwatch
        timer.Start()
        Dim testTables As New ParsecAdmin.TestTableRepository
        Me.TestGridView.DataSource = testTables.GetQuery.ToList
        testTables.Dispose()
        timer.Stop()
        Me.TempoCaricamentoLabel.Text = String.Format("{0} ms.", timer.ElapsedMilliseconds)
    End Sub

   
    Protected Sub CancellaButton_Click(sender As Object, e As System.EventArgs) Handles CancellaButton.Click
        Dim testTables As New ParsecAdmin.TestTableRepository
        Dim predicate As Func(Of ParsecAdmin.TestTable, Boolean) = Function(c) True
        testTables.Delete(predicate)
        testTables.SaveChanges()
        testTables.Dispose()
        Me.TestGridView.Rebind()
    End Sub
End Class