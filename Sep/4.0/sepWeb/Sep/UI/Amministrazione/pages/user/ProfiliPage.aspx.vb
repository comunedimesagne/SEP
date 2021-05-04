Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class ProfiliPage
    Inherits System.Web.UI.Page


    Private Class IRadListBoxItemComparer
        Implements IEqualityComparer(Of Telerik.Web.UI.RadListBoxItem)

        Public Function IEqualityComparer_Equals(ByVal x As Telerik.Web.UI.RadListBoxItem, ByVal y As Telerik.Web.UI.RadListBoxItem) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of Telerik.Web.UI.RadListBoxItem).Equals
            Return x.Value = y.Value

        End Function

        Public Function IEqualityComparer_GetHashCode(ByVal obj As Telerik.Web.UI.RadListBoxItem) As Integer Implements System.Collections.Generic.IEqualityComparer(Of Telerik.Web.UI.RadListBoxItem).GetHashCode
            Return obj.GetHashCode
        End Function


    End Class

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Profilo() As ParsecAdmin.Profilo
        Get
            Return CType(Session(CStr(ViewState("Profilo_Ticks"))), ParsecAdmin.Profilo)
        End Get
        Set(ByVal value As ParsecAdmin.Profilo)
            If ViewState("Profilo_Ticks") Is Nothing Then
                ViewState("Profilo_Ticks") = "Profilo_" & Now.Ticks.ToString
            End If
            Session(CStr(ViewState("Profilo_Ticks"))) = value
        End Set
    End Property

    Public Property Profili() As Object
        Get
            Return CType(Session(CStr(ViewState("Profili_Ticks"))), Object)
        End Get
        Set(ByVal value As Object)
            If ViewState("Profili_Ticks") Is Nothing Then
                ViewState("Profili_Ticks") = "Profili_" & Now.Ticks.ToString
            End If
            Session(CStr(ViewState("Profili_Ticks"))) = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadListBox .rlbCheck,.RadListBox .rlbImage,.RadListBox .rlbText{vertical-align: middle;padding-left:5px;}"
        Me.Page.Header.Controls.Add(css)

        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Profili"

        If Not Me.Page.IsPostBack Then
            Me.CaricaModuli()
        End If
       

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.DatiUtenteMultiPage.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il profilo selezionato?", False, Not Me.Profilo Is Nothing)
        Me.ProcedureLabel.Text = "Procedure " & If(Me.ProcedureListBox.Items.Count > 0, "( " & Me.ProcedureListBox.Items.Count.ToString & " )", "")
        Me.UtentiLabel.Text = "Utenti " & If(Me.UtentiListBox.Items.Count > 0, "( " & Me.UtentiListBox.Items.Count.ToString & " )", "")
        Me.TitoloElencoProfiliLabel.Text = "Elenco Profili&nbsp;&nbsp;&nbsp;" & If(Me.Profili.Count > 0, "( " & Me.Profili.Count.ToString & " )", "")
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
                    If Not Me.Profilo Is Nothing Then
                        Me.Delete()
                        Me.ResettaVista()
                        Me.AggiornaGriglia()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un profilo!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                Dim filtro As New ParsecAdmin.ProfiloFiltro With {.IdModulo = CInt(Me.ModuloComboBox.SelectedItem.Value), .Utente = utenteCollegato}
                Dim profili As New ProfileRepository
                Me.Profili = profili.GetProfiliUtente(filtro)
                Me.ProfiliGridView.Rebind()
                profili.Dispose()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub ProfiliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ProfiliGridView.ItemCommand
        If e.CommandName = "Select" Then
            AggiornaVista(e.Item)
        End If
    End Sub

    Protected Sub ProfiliGridView_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles ProfiliGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub ProfiliGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ProfiliGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona profilo"
            End If
        End If
    End Sub

    Protected Sub ProfiliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ProfiliGridView.NeedDataSource
        If Me.Profili Is Nothing Then
            Dim profili As New ProfileRepository
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim filtro As New ParsecAdmin.ProfiloFiltro With {.Utente = utenteCollegato}
            Me.Profili = profili.GetProfiliUtente(filtro)
            ' profili.Dispose()
        End If
        Me.ProfiliGridView.DataSource = Me.Profili
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub AggiornaGriglia()
        Me.Profili = Nothing
        Me.ProfiliGridView.Rebind()
    End Sub

    Private Sub Print()
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaProfili")
        parametriStampa.Add("DatiStampa", Me.Profili)
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Dim profiloRepository As New ProfileRepository
        profiloRepository.Delete(Me.Profilo)
        profiloRepository.Dispose()
    End Sub

    Private Sub Save()
        Dim profili As New ParsecAdmin.ProfileRepository
        Dim profilo As ParsecAdmin.Profilo = profili.CreateFromInstance(Me.Profilo)
        profilo.Descrizione = Me.DescrizioneTextBox.Text
        profilo.IdModulo = Me.ModuloComboBox.SelectedItem.Value
        profilo.Disabilitato = Me.BloccatoCheckBox.Checked
        profilo.Riservato = Me.RiservatoCheckBox.Checked

        'Inserisco tutte le procedure associate al profilo corrente
        Dim proceduraProfilo As ParsecAdmin.ProceduraProfilo
        Dim procedure As New List(Of ProceduraProfilo)
        For i As Integer = 0 To Me.ProcedureListBox.Items.Count - 1
            proceduraProfilo = New ParsecAdmin.ProceduraProfilo

            proceduraProfilo.IdProcedura = Me.ProcedureListBox.Items(i).Value
            procedure.Add(proceduraProfilo)
        Next

        'Inserisco tutti gli utenti associati al profilo corrente
        Dim utenteProfilo As ParsecAdmin.UtenteProfilo
        Dim utenti As New List(Of UtenteProfilo)
        For i As Integer = 0 To Me.UtentiListBox.Items.Count - 1
            utenteProfilo = New ParsecAdmin.UtenteProfilo
            utenteProfilo.Disabilitato = False
            utenteProfilo.IdUtente = Me.UtentiListBox.Items(i).Value
            utenti.Add(utenteProfilo)
        Next

        Try
            profili.Save(profilo, procedure, utenti)
            Me.Profilo = profili.Profilo
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            profili.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.Profilo = Nothing
        Me.DescrizioneTextBox.Text = ""
        Me.ModuloComboBox.SelectedIndex = 0
        Me.BloccatoCheckBox.Checked = False
        Me.RiservatoCheckBox.Checked = False
        Me.UtentiListBox.Items.Clear()
        Me.ProcedureListBox.Items.Clear()
    End Sub

    Private Sub CaricaModuli()
        Dim moduli As New ModuleRepository
        Me.ModuloComboBox.DataSource = moduli.GetAbilitazioniModuli
        Me.ModuloComboBox.DataTextField = "Descrizione"
        Me.ModuloComboBox.DataValueField = "Id"
        Me.ModuloComboBox.DataBind()
        'Me.ModuloComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Seleziona Modulo -"))
        Me.ModuloComboBox.SelectedIndex = 0
        moduli.Dispose()
    End Sub


    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim profili As New ProfileRepository



        Me.Profilo = profili.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault

        Me.DescrizioneTextBox.Text = Me.Profilo.Descrizione
        Me.ModuloComboBox.FindItemByValue(Me.Profilo.IdModulo).Selected = True

        Me.BloccatoCheckBox.Checked = Me.Profilo.Disabilitato
        Me.RiservatoCheckBox.Checked = Me.Profilo.Riservato

        Me.UtentiListBox.DataValueField = "Id"
        Me.UtentiListBox.DataTextField = "Descrizione"
        Me.UtentiListBox.DataSource = profili.GetUtenti(id)
        Me.UtentiListBox.DataBind()


        Me.ProcedureListBox.DataValueField = "Id"
        Me.ProcedureListBox.DataTextField = "Descrizione"
        Me.ProcedureListBox.DataSource = profili.GetProcedureProfilo(id)
        Me.ProcedureListBox.DataBind()



        profili.Dispose()
    End Sub

    Private Sub DeleteSelectedItems(ByVal list As Telerik.Web.UI.RadListBox)
        For Each item As Telerik.Web.UI.RadListBoxItem In list.CheckedItems
            list.Items.Remove(item)
        Next
    End Sub

#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub TrovaProceduraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaProceduraImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaProceduraPage.aspx"
        Dim queryString As New Hashtable

        queryString.Add("search", Me.ModuloComboBox.SelectedItem.Value)
        queryString.Add("obj", Me.AggiornaProceduraImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
    End Sub

    Protected Sub AggiornaProceduraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaProceduraImageButton.Click
        If Not Session("SelectedProcedures") Is Nothing Then
            Dim procedureSelezionate As SortedList(Of Integer, String) = Session("SelectedProcedures")
            For Each procedura In procedureSelezionate
                Dim item As New Telerik.Web.UI.RadListBoxItem(procedura.Value, procedura.Key)
                If Not Me.ProcedureListBox.Items.Contains(item, New IRadListBoxItemComparer) Then
                    Me.ProcedureListBox.Items.Add(item)
                End If
            Next
        End If
        Session("SelectedProcedures") = Nothing
    End Sub

    Protected Sub EliminaProceduraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaProceduraImageButton.Click
        Me.DeleteSelectedItems(Me.ProcedureListBox)
    End Sub

    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'multipla
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            For Each utente In utentiSelezionati
                Dim item As New Telerik.Web.UI.RadListBoxItem(utente.Value, utente.Key)
                If Not Me.UtentiListBox.Items.Contains(item, New IRadListBoxItemComparer) Then
                    Me.UtentiListBox.Items.Add(item)
                End If
            Next
        End If
        Session("SelectedUsers") = Nothing
    End Sub

    Protected Sub EliminaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteImageButton.Click
        Me.DeleteSelectedItems(Me.UtentiListBox)
    End Sub



#End Region


End Class