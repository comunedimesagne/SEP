Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class StatisticheUtentiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


    Public Property StatisticheUtente() As List(Of ParsecAdmin.Statistica)
        Get
            Return CType(Session("StatisticheUtentiPage_StatisticheUtente"), List(Of ParsecAdmin.Statistica))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Statistica))
            Session("StatisticheUtentiPage_StatisticheUtente") = value
        End Set
    End Property


    Public Property LogEventi() As List(Of ParsecAdmin.LogEvento)
        Get
            Return CType(Session("StatisticheUtentiPage_LogEventi"), List(Of ParsecAdmin.LogEvento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.LogEvento))
            Session("StatisticheUtentiPage_LogEventi") = value
        End Set
    End Property



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Statistiche Utenti S.E.P."
        Me.SalvaButton.Attributes.Add("onclick", "this.disabled=true;")
        'Me.EsportaInExcelImageButton.Attributes.Add("onclick", "this.disabled=true;")
        If Not Me.Page.IsPostBack Then
            Me.LogEventi = New List(Of ParsecAdmin.LogEvento)
            Me.ResettaFiltro()
        End If
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        Me.LogEventiGridView.DataSource = Me.LogEventi
        Me.LogEventiGridView.DataBind()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FiltroStatistica
        Dim filtro As New ParsecAdmin.FiltroStatistica
        filtro.DataInizio = Me.DataInizioTextBox.SelectedDate
        filtro.DataFine = Me.DataFineTextBox.SelectedDate
        Return filtro
    End Function


    Private Sub ResettaFiltro()
        Me.DataInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataFineTextBox.SelectedDate = Now
        Me.StatisticheUtente = Nothing
    End Sub


    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click

        ' strCampiDaEsportare="DataOra||IdUte||Utenza||Descrizione||IP";	

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("StatisticheSEP_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As String = String.Empty

        line &= "Utente;Ultimo Accesso;Ultimo Cambio Password;Numero Accessi"
        swExport.WriteLine(line)
        line = ""
        For Each statistica As ParsecAdmin.Statistica In Me.StatisticheUtente

            line &= statistica.Utente & ";" & _
                String.Format("{0:dd/MM/yyyy HH:mm:ss}", statistica.DataUltimoAccesso) & ";" & _
                String.Format("{0:dd/MM/yyyy HH:mm:ss}", statistica.DataUltimoSettaggioPassword) & ";" & _
                statistica.NumeroAccessi

            swExport.WriteLine(line)
            line = ""
        Next
        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

        Me.SalvaButton.Enabled = True
    End Sub


    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idUtente As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim logs As New ParsecAdmin.LogEventoRepository
        Me.LogEventi = logs.GetView(New ParsecAdmin.FiltroLogEvento With {.IdUtente = idUtente}).OrderByDescending(Function(c) c.Data).ToList
        logs.Dispose()

        Me.LogEventiLabel.Text = "Elenco Log&nbsp;&nbsp;&nbsp;" & " Eseguiti Da:&nbsp;&nbsp;&nbsp;" & Me.StatisticheUtente.Where(Function(c) c.Id = idUtente).FirstOrDefault.Utente
    End Sub

    Protected Sub StatisticheGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles StatisticheGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

    Protected Sub StatisticheGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles StatisticheGridView.NeedDataSource
        If Me.StatisticheUtente Is Nothing Then
            Dim logs As New ParsecAdmin.LogEventoRepository
            Me.StatisticheUtente = logs.GetView(Me.GetFiltro)
            logs.Dispose()
        End If
        Me.StatisticheGridView.DataSource = Me.StatisticheUtente
    End Sub


    Protected Sub StatisticheGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles StatisticheGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Me.StatisticheUtente = Nothing
        Me.StatisticheGridView.Rebind()
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.LogEventi = New List(Of ParsecAdmin.LogEvento)
        Me.StatisticheGridView.Rebind()
    End Sub

  
    Protected Sub EsportaInExcelImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EsportaInExcelImageButton.Click

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("LogEventi_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As String = String.Empty

        line &= "Data Ora;Id Utente;Utente;Descrizione;Indirizzo IP"
        swExport.WriteLine(line)
        line = ""
        For Each log As ParsecAdmin.LogEvento In Me.LogEventi

            line &= String.Format("{0:dd/MM/yyyy HH:mm:ss}", log.Data) & ";" & _
                    log.IdUtente & ";" & _
                    log.Utente & ";" & _
                    log.Descrizione & ";" & _
                    log.IndirizzoIP

            
            swExport.WriteLine(line)
            line = ""
        Next
        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

        Me.EsportaInExcelImageButton.Enabled = True

    End Sub
End Class