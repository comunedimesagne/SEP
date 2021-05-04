Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class GestioneConsiglieriPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Me.Page.Request("Mode") Is Nothing Then
            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Atti Decisionali"
            Me.MainPage.DescrizioneProcedura = "> Gestione Consiglieri"
            Me.ConfermaButton.Visible = False
            Me.GeneraButton.Visible = True

        Else
            Me.MainPage = CType(Me.Master, BasePage)
            Me.ConfermaButton.Visible = True
            Me.ConfermaButton.Attributes.Add("onclick", "this.disabled=true;")
            Me.GeneraButton.Visible = False
            'Me.MainPage.ImpostaLarghezzaHeader("600")
            CType(Me.Master, BasePage).NascondiHeader()
            Me.TitoloLabel.Text = "Ricerca Consigliere"
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Me.Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BasePage.master"
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ConsiglieriLabel.Text = "Elenco Consiglieri&nbsp;&nbsp;&nbsp;" & If(Me.ConsiglieriGridView.MasterTableView.Items.Count > 0, "( " & Me.ConsiglieriGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub


    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Dim consiglieriSelezionati As New List(Of ParsecAtt.Consigliere)
        Dim consiglieri As New ParsecAtt.ConsiglieriRepository
        For Each item As GridDataItem In Me.ConsiglieriGridView.Items
            Dim selectCheckBox As CheckBox = CType(item.FindControl("SelectCheckBox"), CheckBox)
            If selectCheckBox.Checked Then
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                Dim consigliere As ParsecAtt.Consigliere = consiglieri.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
                If Not consigliere Is Nothing Then

                    consiglieriSelezionati.Add(consigliere)
                End If
            End If
        Next
        ParsecUtility.SessionManager.Consigliere = consiglieriSelezionati
        ParsecUtility.Utility.ClosePopup(False)
        consiglieri.Dispose()
    End Sub


    Protected Sub ConsiglieriGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ConsiglieriGridView.NeedDataSource
        Dim consiglieri As New ParsecAtt.ConsiglieriRepository
        Me.ConsiglieriGridView.DataSource = consiglieri.GetView(Nothing)
        consiglieri.Dispose()

        If Not Page.Request("Mode") Is Nothing Then
            Select Case Page.Request("Mode")
                Case "SelezioneSingola"
                    Me.ConsiglieriGridView.MasterTableView.GetColumnSafe("Select").Visible = True
                    Me.ConsiglieriGridView.MasterTableView.GetColumnSafe("CheckBoxTemplateColumn").Visible = False
                    Me.ConfermaButton.Visible = False
                Case "SelezioneMultipla"
                    Me.ConsiglieriGridView.MasterTableView.GetColumnSafe("Select").Visible = False
                    Me.ConsiglieriGridView.MasterTableView.GetColumnSafe("CheckBoxTemplateColumn").Visible = True
            End Select
        Else
            Me.ConsiglieriGridView.MasterTableView.GetColumnSafe("Select").Visible = False
            Me.ConsiglieriGridView.MasterTableView.GetColumnSafe("CheckBoxTemplateColumn").Visible = False
        End If


    End Sub

    Protected Sub ConsiglieriGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ConsiglieriGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf ConsiglieriGridView_ItemPreRender
        End If
    End Sub

    Protected Sub ConsiglieriGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub ConsiglieriGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles ConsiglieriGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.ConsiglieriGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = Me.ConsiglieriGridView.SelectedItems.Count = Me.ConsiglieriGridView.Items.Count
    End Sub

    Protected Sub ConsiglieriGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ConsiglieriGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.Seleziona(e.Item)
        End If
    End Sub

    Private Sub Seleziona(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim consiglieri As New ParsecAtt.ConsiglieriRepository
        Dim consigliere As ParsecAtt.Consigliere = consiglieri.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        consiglieri.Dispose()
        ParsecUtility.SessionManager.Consigliere = consigliere
        ParsecUtility.Utility.ClosePopup(False)
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub


    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.ConsiglieriGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub


    Private Function GetStrutturaAnnullata(ByVal idPadre As Integer) As Boolean
        Dim annullata As Boolean = False
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim settore = strutture.Where(Function(c) c.Id = idPadre).FirstOrDefault

        While Not settore Is Nothing
            If settore.LogStato = "A" Then
                annullata = True
                Exit While
            End If
            settore = strutture.Where(Function(c) c.Id = settore.IdPadre).FirstOrDefault
        End While

        strutture.Dispose()
        Return annullata
    End Function


    Protected Sub GeneraButton_Click(sender As Object, e As System.EventArgs) Handles GeneraButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim qualificaConsigliere As Integer = 6
        Dim gerarchiaRuolo As Integer = 300
        Dim consiglioComunale As String = "consiglio comunale"

        Dim strutture As New ParsecAdmin.StructureRepository


        Dim elencoStrutture = strutture.GetQuery

        Dim elenco = (From struttura In elencoStrutture
                      Where (From s In elencoStrutture Where s.LogStato Is Nothing And s.Descrizione.Contains(consiglioComunale) And s.IdGerarchia = gerarchiaRuolo Select s.Id).Contains(struttura.IdPadre) And struttura.LogStato Is Nothing And struttura.Qualifica = qualificaConsigliere
                      Select struttura).ToList



        Dim consiglieriDaEscludere As New List(Of Integer)


        For Each item In elenco
            Dim annullata = Me.GetStrutturaAnnullata(item.IdPadre)
            If annullata Then
                consiglieriDaEscludere.Add(item.Id)
            End If
        Next


        'elenco = (From item In elenco
        '         Where Not lista.Contains(item.Id)).ToList

        Dim nominativi = (From item In elenco
                 Where Not consiglieriDaEscludere.Any(Function(c) c = item.Id)).Select(Function(c) c.Descrizione).ToList



        Dim consiglieri As New ParsecAtt.ConsiglieriRepository

        Dim nuovoConsigliere As ParsecAtt.Consigliere
        For Each nominativo As String In nominativi
            Dim nome As String = nominativo
            Dim esiste = Not consiglieri.GetQuery.Where(Function(c) c.Stato Is Nothing And c.Nominativo.Contains(nome)).FirstOrDefault Is Nothing
            If Not esiste Then
                nuovoConsigliere = New ParsecAtt.Consigliere With {.Nominativo = nome, .DataA = Now, .DataOperazione = Now, .IdUtente = utenteCollegato.Id}
                consiglieri.Add(nuovoConsigliere)
                consiglieri.SaveChanges()
            End If
        Next

        Dim elencoConsiglieri As List(Of ParsecAtt.Consigliere) = consiglieri.GetQuery.Where(Function(c) c.Stato Is Nothing).ToList
        For Each consigliere As ParsecAtt.Consigliere In elencoConsiglieri
            Dim nominativo As String = consigliere.Nominativo
            Dim esiste = Not (From struttura In elencoStrutture
                              Where (From s In elencoStrutture Where s.LogStato Is Nothing And s.Descrizione.Contains(consiglioComunale) And s.IdGerarchia = gerarchiaRuolo Select s.Id).Contains(struttura.IdPadre) And struttura.LogStato Is Nothing And struttura.Qualifica = qualificaConsigliere And struttura.Descrizione.Contains(nominativo)
                              Select struttura.Descrizione).FirstOrDefault Is Nothing

            If Not esiste Then
                consigliere.IdUtente = utenteCollegato.Id
                consigliere.DataA = Now
                consigliere.Stato = "A"
                consigliere.DataOperazione = Now
                consiglieri.SaveChanges()
            End If

        Next


        strutture.Dispose()

        consiglieri.Dispose()
        Me.ConsiglieriGridView.Rebind()

    End Sub
End Class