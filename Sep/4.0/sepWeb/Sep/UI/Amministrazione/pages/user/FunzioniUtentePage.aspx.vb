Imports ParsecAdmin
Imports System.Transactions
Imports System.Text.RegularExpressions
Imports Telerik.Web.UI

Partial Class FunzioniUtentePage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Utenti() As List(Of ParsecAdmin.Utente)
        Get
            Return CType(Session("FunzioniUtentePage_Utente"), List(Of ParsecAdmin.Utente))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Utente))
            Session("FunzioniUtentePage_Utente") = value
        End Set
    End Property


#End Region


#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Funzioni utente"
        ScriptManager.GetCurrent(Me.Page).RegisterPostBackControl(Me.StampaImageButton)
        If Not Me.Page.IsPostBack Then
            Me.Utenti = Nothing
        End If
        Me.UtentiGridView.GroupingSettings.CaseSensitive = False
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ' Me.TitoloElencoFunzioniLabel.Text = "Elenco E-mail Inviate&nbsp;&nbsp;&nbsp;" & If(Me.Utenti.Count > 0, "( " & Me.Utenti.Count.ToString & " )", "")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region


#Region "METODI PRIVATI"


    Private Function GetUtenti() As List(Of ParsecAdmin.Utente)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(Session("CurrentUser"), ParsecAdmin.Utente)

        Dim utenti As New ParsecAdmin.UserRepository

        Dim filtroUtenti As IQueryable(Of ParsecAdmin.Utente) = utenti.Where(Function(c) c.LogTipoOperazione Is Nothing)
        If Not utenteCorrente.SuperUser Then
            filtroUtenti = filtroUtenti.Where(Function(c) c.SuperUser = False)
        End If

        Dim query = From utente In filtroUtenti.ToList
                       Order By (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
                        Select New ParsecAdmin.Utente With
                               {
                                   .Id = utente.Id,
                                   .Nominativo = (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome)),
                                   .Username = utente.Username,
                                   .CodiceFiscale = utente.CodiceFiscale,
                                   .Bloccato = utente.Bloccato
                               }

        utenti.Dispose()
        Return query.ToList

       
    End Function

    Private Function GetFunzioni(ByVal idUtente As Integer) As IEnumerable

        Dim utenti As New ParsecAdmin.UserRepository
        Dim profiliUtente As New ParsecAdmin.UserProfileRepository(utenti.Context)
        Dim procedureProfilo As New ParsecAdmin.ProcedureProfileRepository(utenti.Context)
        Dim procedure As New ParsecAdmin.ProcedureRepository(utenti.Context)
        Dim profili As New ParsecAdmin.ProfileRepository(utenti.Context)

        Dim result = From u In utenti.GetQuery
                      Group Join pu In profiliUtente.GetQuery On u.Id Equals pu.IdUtente
                      Into elenco = Group
                      From pu In elenco.DefaultIfEmpty()
                      Group Join pp In procedureProfilo.GetQuery On pp.IdProfilo Equals pu.IdProfilo
                      Into elenco2 = Group
                      From pp In elenco2.DefaultIfEmpty()
                      Group Join p In procedure.GetQuery On p.Id Equals pp.IdProcedura
                      Into elenco3 = Group
                      From p In elenco3.DefaultIfEmpty()
                      Group Join pr In profili.GetQuery On pr.Id Equals pu.IdProfilo
                      Into elenco4 = Group
                      From pr In elenco4.DefaultIfEmpty()
        Where
        u.Id = idUtente AndAlso
        u.LogTipoOperazione Is Nothing AndAlso
        pu.LogTipoOperazione Is Nothing AndAlso
        pp.LogTipoOperazione Is Nothing
        Order By p.Descrizione, pr.Descrizione
        Select New With
        {
            .Funzione = p.Descrizione,
            .Profilo = pr.Descrizione,
            .Data = pu.Log_DataOperazione}


        Return result

    End Function

#End Region


#Region "EVENTI GRIGLIA"


    Protected Sub UtentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UtentiGridView.ItemCommand
        If e.CommandName = Telerik.Web.UI.RadGrid.ExpandCollapseCommandName AndAlso Not e.Item.Expanded Then
            Dim parentItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
            Dim innerGrid As Telerik.Web.UI.RadGrid = CType(parentItem.ChildItem.FindControl("FunzioniGridView"), Telerik.Web.UI.RadGrid)
            innerGrid.Rebind()
        End If
    End Sub

    Protected Sub UtentiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiGridView.ItemCreated
        If TypeOf e.Item Is GridNestedViewItem Then
            AddHandler CType(e.Item.FindControl("FunzioniGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.FunzioniGridView_NeedDataSource)
            'AddHandler CType(e.Item.FindControl("FunzioniGridView"), RadGrid).ItemDataBound, New GridItemEventHandler(AddressOf Me.AllegatiGridView_ItemDataBound)
            ' AddHandler CType(e.Item.FindControl("FunzioniGridView"), RadGrid).ItemCommand, New GridCommandEventHandler(AddressOf Me.AllegatiGridView_ItemCommand)
            AddHandler CType(e.Item.FindControl("FunzioniGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.FunzioniGridView_ItemCreated)
        End If

        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource
        If Me.Utenti Is Nothing Then
            Me.Utenti = Me.GetUtenti
        End If

        Me.UtentiGridView.DataSource = Me.Utenti
    End Sub

    Protected Sub UtentiGridView_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles UtentiGridView.PreRender
        If Session("Stampa") = True Then
            UtentiGridView.ExportSettings.OpenInNewWindow = True
            UtentiGridView.ExportSettings.ExportOnlyData = True
            UtentiGridView.ExportSettings.IgnorePaging = True
            UtentiGridView.MasterTableView.HierarchyDefaultExpanded = True
            Me.UtentiGridView.MasterTableView.ExportToExcel()
            Session("Stampa") = False
        End If
    End Sub

    Protected Sub FunzioniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Dim parentItem As Telerik.Web.UI.GridDataItem = CType(CType(CType(sender, Telerik.Web.UI.RadGrid).NamingContainer, Telerik.Web.UI.GridNestedViewItem).ParentItem, Telerik.Web.UI.GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("Id")
        CType(sender, Telerik.Web.UI.RadGrid).DataSource = Me.GetFunzioni(id)
    End Sub

    Protected Sub FunzioniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub StampaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles StampaImageButton.Click
        Session("Stampa") = True
    End Sub

#End Region


End Class