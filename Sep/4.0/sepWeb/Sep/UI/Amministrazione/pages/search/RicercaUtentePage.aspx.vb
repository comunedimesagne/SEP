Imports ParsecAdmin
Imports Telerik.Web.UI

Partial Class RicercaUtentePage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    Public Property TipoSelezione As Integer
        Get
            Return ViewState("TipoSelezione")
        End Get
        Set(ByVal value As Integer)
            ViewState("TipoSelezione") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim utenteCorrente As ParsecAdmin.Utente = CType(Session("CurrentUser"), ParsecAdmin.Utente)
        Dim utenti = (New UserRepository).GetQuery.ToList

        If utenteCorrente.SuperUser Then


            Dim query = From utente In utenti
                        Where utente.LogTipoOperazione Is Nothing
                        Order By (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
                        Select ID = utente.Id,
                                    Descrizione = (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome)),
                                    Utenza = utente.Username,
                                    CodiceFiscale = utente.CodiceFiscale,
                                    Bloccato = If(utente.Bloccato = 0, "NO", "SI")
            Me.UtentiGridView.DataSource = query
        Else

            Dim query = From utente In utenti
                       Where utente.LogTipoOperazione Is Nothing And utente.SuperUser = False
                       Order By (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
                       Select ID = utente.Id,
                                   Descrizione = (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome)),
                                   Utenza = utente.Username,
                                   CodiceFiscale = utente.CodiceFiscale,
                                   Bloccato = If(utente.Bloccato = 0, "NO", "SI")
            Me.UtentiGridView.DataSource = query
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Elenco Utenti&nbsp;&nbsp;&nbsp;" & If(Me.UtentiGridView.MasterTableView.Items.Count > 0, "( " & Me.UtentiGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.GetParametri()
            If Me.TipoSelezione = 0 Then
                Me.UtentiGridView.Columns(0).Visible = False
                Me.ConfermaButton.Visible = False
            End If
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Private Sub UtentiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf UtentiGridView_ItemPreRender
        End If
    End Sub

    Protected Sub UtentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles UtentiGridView.ItemCommand
        If e.CommandName = "Select" Then
            If Me.TipoSelezione = 0 Then
                Me.SelezionaUtente(e.Item)
            End If
        End If
    End Sub

    Protected Sub UtentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona utente"
            End If
        End If
    End Sub

    Protected Sub UtentiGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub UtentiGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles UtentiGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.UtentiGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.UtentiGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.UtentiGridView.SelectedItems.Count = Me.UtentiGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.UtentiGridView.Items.Count > 0
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In UtentiGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub GetParametri()

        Dim ht As Hashtable = Session("Parametri")
        If Not ht Is Nothing Then
            If ht.ContainsKey("tipoSelezione") Then
                Me.TipoSelezione = ht("tipoSelezione")
            End If
        End If
        Session("Parametri") = Nothing
    End Sub

    Private Sub SelezionaUtente(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim utentiSelezionati As New SortedList(Of Integer, String)
        Dim userRepository As New UserRepository

        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim utente As ParsecAdmin.Utente = userRepository.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        If Not utente Is Nothing Then
            Dim descrizioneUtente As String = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
            utentiSelezionati.Add(utente.Id, descrizioneUtente)
        End If
        Session("SelectedUsers") = utentiSelezionati

        ParsecUtility.Utility.ClosePopup(True)
        userRepository.Dispose()
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Dim utentiSelezionati As New SortedList(Of Integer, String)

        Dim userRepository As New UserRepository

        For Each item As GridDataItem In Me.UtentiGridView.Items
            Dim selectCheckBox As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)
            If selectCheckBox.Checked Then
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim utente As ParsecAdmin.Utente = userRepository.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
                If Not utente Is Nothing Then
                    utentiSelezionati.Add(utente.Id, (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome)))
                End If

            End If
        Next
        Session("SelectedUsers") = utentiSelezionati

        ParsecUtility.Utility.ClosePopup(True)
        userRepository.Dispose()

    End Sub

    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

#End Region

End Class