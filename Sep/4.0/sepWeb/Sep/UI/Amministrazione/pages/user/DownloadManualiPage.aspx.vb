Imports ParsecAdmin
Imports System.Transactions
Imports System.Text.RegularExpressions
Imports Telerik.Web.UI

Partial Class DownloadManualiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Manuali() As List(Of ParsecAdmin.Manuale)
        Get
            Return CType(Session("ScaricoManualiPage_Manuale"), List(Of ParsecAdmin.Manuale))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Manuale))
            Session("ScaricoManualiPage_Manuale") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Manuali Utente"
        If Not Me.Page.IsPostBack Then
            Me.Manuali = Me.GetManuali
            If (Me.Manuali Is Nothing) Then
                Me.Manuali = New List(Of Manuale)
            End If
            'Me.ManualiGridView.Rebind()

        End If
        Me.ManualiGridView.GroupingSettings.CaseSensitive = False
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Manuali&nbsp;&nbsp;&nbsp;" & If(Me.Manuali.Count > 0, "( " & Me.Manuali.Count.ToString & " )", "")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Function GetManuali() As List(Of ParsecAdmin.Manuale)

        Dim manuali As New ParsecAdmin.ManualiRepository
        Dim filtro As New ParsecAdmin.FiltroManuale
        Dim manualiToReturn = manuali.GetView(filtro)
        Return manualiToReturn.ToList

    End Function

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ManualiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ManualiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If

    End Sub

    Protected Sub ManualiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ManualiGridView.NeedDataSource
        Me.ManualiGridView.DataSource = Me.Manuali
    End Sub

    Protected Sub ManualiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ManualiGridView.ItemCommand
        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathManuali")

        Select Case e.CommandName
            Case "PreviewDocumento"
                Dim NomeFile As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("NomeFile")

                Try

                    Dim file As New IO.FileInfo(pathDownload & NomeFile)
                    If file.Exists Then
                        Dim dimensioneFile As Double = file.Length
                        Session("AttachmentFullName") = file.FullName
                        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                        ParsecUtility.Utility.PageReload(pageUrl, False)
                    Else
                        ParsecUtility.Utility.MessageBox("File non trovato.", False)
                    End If

                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox("Errore. " & ex.Message, False)
                End Try

        End Select
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

#End Region


End Class