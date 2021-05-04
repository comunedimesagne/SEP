Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class StatisticheUtentiAttiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


    Public Property StatisticheUtente() As List(Of ParsecAtt.Statistica)
        Get
            Return CType(Session("StatisticheUtentiAttiPage_StatisticheUtente"), List(Of ParsecAtt.Statistica))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Statistica))
            Session("StatisticheUtentiAttiPage_StatisticheUtente") = value
        End Set
    End Property


    Public Property StatisticheTipologia() As List(Of ParsecAtt.Statistica)
        Get
            Return CType(Session("StatisticheUtentiAttiPage_StatisticheTipologia"), List(Of ParsecAtt.Statistica))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Statistica))
            Session("StatisticheUtentiAttiPage_StatisticheTipologia") = value
        End Set
    End Property



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Statistiche Atti Decisionali"
        Me.SalvaButton.Attributes.Add("onclick", "this.disabled=true;")
        If Not Me.Page.IsPostBack Then
            Me.ResettaFiltro()
        End If
    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroDocumento
        Dim filtro As New ParsecAtt.FiltroDocumento
        filtro.DataInizio = Me.DataInizioTextBox.SelectedDate
        filtro.DataFine = Me.DataFineTextBox.SelectedDate
        Return filtro
    End Function


    Private Sub ResettaFiltro()
        Me.DataInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataFineTextBox.SelectedDate = Now
        Me.StatisticheTipologia = Nothing
        Me.StatisticheUtente = Nothing
    End Sub


    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click

        'Key = HeaderText   Value = Property
        Dim mapping As New Dictionary(Of String, String)

        mapping.Add("Utente", "Utente")
        mapping.Add("N. Atti", "NumeroDocumenti")
        mapping.Add("N. Annullati", "NumeroDocumentiAnnullati")
        mapping.Add("N. Determine", "NumeroDetermine")
        mapping.Add("N. Delibere", "NumeroDelibere")
        mapping.Add("N. Direttive", "NumeroDirettive")
        mapping.Add("N. Decreti", "NumeroDecreti")
        mapping.Add("N. Ordinanze", "NumeroOrdinanze")

        Dim csv As String = (New ParsecAtt.DocumentoRepository).ToCsv(Me.StatisticheUtente, mapping)




        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim nomeFile As String = String.Format("StatisticheATTI_UT{0}_AL_{1}.xls", utenteCollegato.Id, Now.ToString("ddMM_yyyy_hhmmss"))
        Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDownload") & nomeFile
        IO.File.WriteAllText(pathDownload, csv)
        Dim fileUrl As String = "~/Download/" & nomeFile
        Dim file As New IO.FileInfo(pathDownload)
        Session("AttachmentFullName") = file.FullName
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
        Me.SalvaButton.Enabled = True
    End Sub

    Protected Sub StatisticheGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles StatisticheGridView.NeedDataSource
        If Me.StatisticheUtente Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Me.StatisticheUtente = documenti.GetDocumentiPerUtente(Me.GetFiltro)
            documenti.Dispose()
        End If
        Me.StatisticheGridView.DataSource = Me.StatisticheUtente
    End Sub

    Protected Sub StatisticheDocumentoGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles StatisticheDocumentoGridView.NeedDataSource
        If Me.StatisticheTipologia Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Me.StatisticheTipologia = documenti.GetStatistiche(Me.GetFiltro)
            documenti.Dispose()
        End If
        Me.StatisticheDocumentoGridView.DataSource = Me.StatisticheTipologia
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
        Me.StatisticheTipologia = Nothing
        Me.StatisticheUtente = Nothing
        Me.StatisticheDocumentoGridView.Rebind()
        Me.StatisticheGridView.Rebind()
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.StatisticheDocumentoGridView.Rebind()
        Me.StatisticheGridView.Rebind()
    End Sub

End Class