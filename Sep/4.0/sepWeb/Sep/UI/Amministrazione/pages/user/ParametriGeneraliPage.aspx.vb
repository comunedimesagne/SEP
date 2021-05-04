Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class ParametriGeneraliPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Private Property ModuloPredefinito As Integer
        Set(value As Integer)
            ViewState("ModuloPredefinito") = value
        End Set
        Get
            Return ViewState("ModuloPredefinito")
        End Get
    End Property

    Private Property Parametro() As ParsecAdmin.Parametri
        Get
            Return CType(Session("ParametriGeneraliPage_Parametro"), ParsecAdmin.Parametri)
        End Get
        Set(ByVal value As ParsecAdmin.Parametri)
            Session("ParametriGeneraliPage_Parametro") = value
        End Set
    End Property

    Private Property Parametri() As List(Of ParsecAdmin.Parametri)
        Get
            Return CType(Session("ParametriGeneraliPage_Parametri"), List(Of ParsecAdmin.Parametri))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Parametri))
            Session("ParametriGeneraliPage_Parametri") = value
        End Set
    End Property

    Private Property FullSize As Boolean
        Set(value As Boolean)
            ViewState("FullSize") = value
        End Set
        Get
            Return ViewState("FullSize")
        End Get
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.ModuloPredefinito = CInt(Request.QueryString("m"))

        Me.MainPage = CType(Me.Master, MainPage)
        Select Case CType(Me.ModuloPredefinito, ParsecAdmin.TipoModulo)
            Case TipoModulo.SEP
                Me.MainPage.NomeModulo = "Amministrazione"
            Case TipoModulo.PRO
                Me.MainPage.NomeModulo = "Protocollo"
            Case TipoModulo.ATT
                Me.MainPage.NomeModulo = "Atti Decisionali"
        End Select

        If Not Me.Page.IsPostBack Then
            Me.Parametri = Nothing
            Me.FullSize = False
        End If
        Me.CaricaModuli()
        Me.MainPage.DescrizioneProcedura = "> Gestione Parametri Generali"

        Me.ModuloComboBox.FindItemByValue(Me.ModuloPredefinito).Selected = True

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.ParametriGridView.Style.Add("width", widthStyle)
        Me.FullSizeParametriGridView.Style.Add("width", widthStyle)


    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il parametro selezionato?", False, Not Me.Parametro Is Nothing)
        If Not Me.Parametri Is Nothing Then
            Me.TitoloElencoParametriLabel.Text = "Elenco Parametri&nbsp;&nbsp;&nbsp;" & If(Me.Parametri.Count > 0, "( " & Me.Parametri.Count.ToString & " )", "")
            Me.TitoloElencoFullSizeParametriLabel.Text = Me.TitoloElencoParametriLabel.Text
        End If
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim message As String = String.Empty
                Try
                    Me.Save()
                    Me.AggiornaGriglia()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try

                If String.IsNullOrEmpty(message) Then
                    Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                Else
                    ParsecUtility.Utility.MessageBox(message, False)
                End If

            Case "Nuovo"
                Me.ResettaVista()
                Me.AggiornaGriglia()
            Case "Annulla"
                Me.ResettaVista()
                Me.AggiornaGriglia()
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Parametro Is Nothing Then
                        Dim message As String = String.Empty
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            AggiornaGriglia()
                        Catch ex As Exception
                            message = ex.Message
                        End Try
                        If String.IsNullOrEmpty(message) Then
                            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                        Else
                            ParsecUtility.Utility.MessageBox(message, False)
                        End If

                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un parametro!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"

                Me.Search()
              
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ParametriGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ParametriGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

    Protected Sub ParametriGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ParametriGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona parametro"
            End If
        End If
        If TypeOf e.Item Is Telerik.Web.UI.GridHeaderItem Then
            Dim header As GridHeaderItem = CType(e.Item, GridHeaderItem)
            Dim lb As LinkButton = Nothing
            'DATO CHE HO IMPOSTATO LA PROPRIETA' HeaderTooltip CHE MI SOVRASCRIVE LA DESCRIZIONE DEL TOOLTIP DEL LINKBUTTON
            'LO REIMPOSTO IN MODO CORRETTO.
            If TypeOf header("Modulo").Controls(0) Is LinkButton Then
                lb = CType(header("Modulo").Controls(0), LinkButton)
                lb.ToolTip = "Cliccare qui per ordinare"
            End If
        End If

    End Sub

    Protected Sub ParametriGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ParametriGridView.NeedDataSource
        If Me.Parametri Is Nothing Then
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim filtro As New ParsecAdmin.FiltroParametro With {.SuperUser = utenteCorrente.SuperUser}
            filtro.IdModulo = Me.ModuloPredefinito
            Me.Parametri = parametri.GetParametri(filtro)
            parametri.Dispose()
        End If
        Me.ParametriGridView.DataSource = Me.Parametri
    End Sub

    Protected Sub ParametriGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ParametriGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If

        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

  

#End Region

#Region "METODI PRIVATI"

    Private Sub Print()
        'Dim parametriStampa As New Hashtable
        'parametriStampa.Add("TipologiaStampa", "StampaQualificheOrganigramma")
        'parametriStampa.Add("DatiStampa", Me.Avvisi)
        'Session("ParametriStampa") = parametriStampa
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Try
            Dim parametri As New ParsecAdmin.ParametriRepository
            parametri.Delete(Me.Parametro.Id)
            parametri.Dispose()
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try
    End Sub

    Private Sub Save()
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.CreateFromInstance(Me.Parametro)

        parametro.Nome = Me.NomeTextBox.Text
        parametro.Descrizione = Me.DescrizioneTextBox.Text
        parametro.Valore = Me.ValoreTextBox.Text
        If Me.ModuloComboBox.SelectedIndex > 0 Then
            parametro.IdModulo = CInt(Me.ModuloComboBox.SelectedValue)
        End If

        parametro.Riservato = Me.RiservatoCheckBox.Checked

        Try
            parametri.Save(parametro)
            Me.Parametro = parametri.Parametro
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            parametri.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.Parametro = Nothing
        Me.NomeTextBox.Text = String.Empty
        Me.ValoreTextBox.Text = String.Empty
        Me.DescrizioneTextBox.Text = String.Empty
        Me.RiservatoCheckBox.Checked = False
        Me.ModuloComboBox.FindItemByValue(Me.ModuloPredefinito).Selected = True
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim parametri As New ParsecAdmin.ParametriRepository
        Me.Parametro = parametri.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        Me.NomeTextBox.Text = Me.Parametro.Nome
        Me.ValoreTextBox.Text = Me.Parametro.Valore
        Me.DescrizioneTextBox.Text = Me.Parametro.Descrizione
        If Not Me.Parametro.Riservato Is Nothing Then
            Me.RiservatoCheckBox.Checked = Me.Parametro.Riservato
        End If
        Me.ModuloComboBox.FindItemByValue(Me.Parametro.IdModulo).Selected = True
        parametri.Dispose()
    End Sub

    Private Sub CaricaModuli()
        Dim moduleRepository As New ParsecAdmin.ModuleRepository
        Dim moduli = moduleRepository.GetQuery.Where(Function(c) c.Abilitato = True).OrderBy(Function(c) c.Descrizione)
        Me.ModuloComboBox.DataTextField = "Descrizione"
        Me.ModuloComboBox.DataValueField = "Id"
        Me.ModuloComboBox.DataSource = moduli
        Me.ModuloComboBox.DataBind()
        moduleRepository.Dispose()
        Me.ModuloComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
    End Sub

    Private Sub AggiornaGriglia()
        Me.Parametri = Nothing
        Me.ParametriGridView.Rebind()
    End Sub

    Private Sub Search()
        Dim filtro = Me.GetFiltro
        Dim parametri As New ParsecAdmin.ParametriRepository
        Me.Parametri = parametri.GetParametri(filtro)
        Me.ParametriGridView.Rebind()
        parametri.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FiltroParametro
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim filtro As New ParsecAdmin.FiltroParametro
        If Not String.IsNullOrEmpty(Me.NomeTextBox.Text) Then
            filtro.NomeParametro = Me.NomeTextBox.Text.Trim
        End If
        If Me.ModuloComboBox.SelectedIndex > 0 Then
            filtro.IdModulo = CInt(Me.ModuloComboBox.SelectedValue)
        End If
        filtro.SuperUser = utenteCorrente.SuperUser
        Return filtro
    End Function

#End Region


  
#Region "EVENTI GRIGLIA FULL SIZE"


    Protected Sub VisualizzaSchermoInteroImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaSchermoInteroImageButton.Click
        Me.FullSize = True
        Me.FullSizeParametriGridView.Rebind()
        Dim script As New StringBuilder
        script.AppendLine("<script>")
        script.AppendLine("ShowFullSizeGridPanel();")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
    End Sub


    Protected Sub FullSizeParametriGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FullSizeParametriGridView.NeedDataSource
        If Me.FullSize Then
            Me.FullSizeParametriGridView.DataSource = Me.Parametri
        End If
    End Sub

    Protected Sub FullSizeParametriGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FullSizeParametriGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
            Dim script As New StringBuilder
            script.AppendLine("<script>")
            script.AppendLine("HideFullSizeGridPanel();")
            script.AppendLine("</script>")
            ParsecUtility.Utility.RegisterScript(script, False)
            Me.FullSize = False
        End If
    End Sub

    Protected Sub FullSizeParametriGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FullSizeParametriGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub


    Protected Sub NoPaging_Click(sender As Object, e As System.EventArgs) Handles NoPaging.Click
        Me.ParametriGridView.AllowPaging = Not Me.ParametriGridView.AllowPaging
        If Me.ParametriGridView.AllowPaging Then
            Me.NoPaging.Text = "Non Paginare"
            Me.NoPaging.Icon.PrimaryIconUrl = "~/images/Next.png"
        Else
            Me.NoPaging.Text = "Paginare"
            Me.NoPaging.Icon.PrimaryIconUrl = "~/images/Previous.png"
        End If
        Me.ParametriGridView.Rebind()
    End Sub

    Protected Sub FullSizeNoPaging_Click(sender As Object, e As System.EventArgs) Handles FullSizeNoPaging.Click
        Me.FullSizeParametriGridView.AllowPaging = Not Me.FullSizeParametriGridView.AllowPaging
        If Me.FullSizeParametriGridView.AllowPaging Then
            Me.FullSizeNoPaging.Text = "Non Paginare"
            Me.FullSizeNoPaging.Icon.PrimaryIconUrl = "~/images/Next.png"
        Else
            Me.FullSizeNoPaging.Text = "Paginare"
            Me.FullSizeNoPaging.Icon.PrimaryIconUrl = "~/images/Previous.png"
        End If
        Me.FullSizeParametriGridView.Rebind()
    End Sub

#End Region

  
    
End Class