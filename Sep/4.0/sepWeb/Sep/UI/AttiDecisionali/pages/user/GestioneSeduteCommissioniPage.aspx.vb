Imports ParsecAtt
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneSeduteCommissioniPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Public Property SedutaCommissione() As ParsecAtt.SedutaCommissione
        Get
            Return CType(Session("GestioneSeduteCommissioniPage_SedutaCommissione"), ParsecAtt.SedutaCommissione)
        End Get
        Set(ByVal value As ParsecAtt.SedutaCommissione)
            Session("GestioneSeduteCommissioniPage_SedutaCommissione") = value
        End Set
    End Property

    Public Property SeduteCommissioni() As List(Of ParsecAtt.SedutaCommissione)
        Get
            Return CType(Session("GestioneSeduteCommissioniPage_SeduteCommissioni"), List(Of ParsecAtt.SedutaCommissione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.SedutaCommissione))
            Session("GestioneSeduteCommissioniPage_SeduteCommissioni") = value
        End Set
    End Property

    'Public Property Consiglieri() As List(Of ParsecAtt.ConsigliereCommissione)
    '    Get
    '        Return CType(Session("GestioneSeduteCommissioniPage_Consiglieri"), List(Of ParsecAtt.ConsigliereCommissione))
    '    End Get
    '    Set(ByVal value As List(Of ParsecAtt.ConsigliereCommissione))
    '        Session("GestioneSeduteCommissioniPage_Consiglieri") = value
    '    End Set
    'End Property

    Public Property PresenzeCommissione() As List(Of ParsecAtt.PresenzaCommissione)
        Get
            Return CType(Session("GestioneSeduteCommissioniPage_PresenzaCommissione"), List(Of ParsecAtt.PresenzaCommissione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.PresenzaCommissione))
            Session("GestioneSeduteCommissioniPage_PresenzaCommissione") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Me.Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BasePage.master"
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Me.Page.Request("Mode") Is Nothing Then
            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Atti Decisionali"
            Me.MainPage.DescrizioneProcedura = "> Gestione Commissioni"
        Else
            Me.MainPage = CType(Me.Master, PopupPage)
        End If
        If Not Me.Page.IsPostBack Then
            Me.SeduteCommissioni = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "DescrizioneCommissione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.SeduteCommissioniGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

            'Me.CaricaCommissioni()

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("CostoOrarioSedutaCommissione")
            If parametro Is Nothing Then
                ParsecUtility.Utility.MessageBox("Il parametro CostoOrarioSedutaCommissione non è stato trovato.", False)
            Else
                Me.txtCostoOrario.Text = parametro.Valore
            End If
            parametri.Dispose()

        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.ConsiglieriGridView.Style.Add("width", widthStyle)
        Me.SeduteCommissioniGridView.Style.Add("width", widthStyle)

        
        Me.CaricaCommissioni()

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la seduta di commissione selezionata?", False, Not Me.SedutaCommissione Is Nothing)
        If Not Me.SeduteCommissioni Is Nothing Then
            Me.TitoloElencoCommissioniLabel.Text = "Elenco Sedute Commissioni&nbsp;&nbsp;&nbsp;" & If(Me.SeduteCommissioni.Count > 0, "( " & Me.SeduteCommissioni.Count.ToString & " )", "")
        End If
        Me.TitoloElencoConsiglieriLabel.Text = "Elenco Consiglieri&nbsp;&nbsp;&nbsp;" & If(Me.PresenzeCommissione.Count > 0, "( " & Me.PresenzeCommissione.Count.ToString & " )", "")

        Me.AggiornaGrigliaPresenzeCommissione()

        'SELEZIONO LA RIGA
        If Not Me.SedutaCommissione Is Nothing Then
            Dim item As GridDataItem = Me.SeduteCommissioniGridView.MasterTableView.FindItemByKeyValue("Id", Me.SedutaCommissione.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If



    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollConsiglieriPanel, Me.scrollPosConsiglieriHidden, False)
    End Sub

#End Region

#Region "EVENTI TOOLBAR"


    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                ' Me.Print()
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
                CommissioniComboBox.Enabled = True

            Case "Annulla"
                Me.ResettaVista()
                Me.AggiornaGriglia()
                CommissioniComboBox.Enabled = True

            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.SedutaCommissione Is Nothing Then
                        Dim message As String = String.Empty
                        Try

                            Me.Delete()
                            Me.ResettaVista()
                            Me.AggiornaGriglia()
                        Catch ex As Exception
                            message = ex.Message
                        End Try

                        If String.IsNullOrEmpty(message) Then
                            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                        Else
                            ParsecUtility.Utility.MessageBox(message, False)
                        End If
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una commissione!", False)
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

    Protected Sub SeduteCommissioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles SeduteCommissioniGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
                CommissioniComboBox.Enabled = False
        End Select
    End Sub


    Protected Sub SeduteCommissioniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles SeduteCommissioniGridView.NeedDataSource
        If Me.SeduteCommissioni Is Nothing Then
            Dim seduteCommissioni As New ParsecAtt.SeduteCommissioneRepository
            Me.SeduteCommissioni = seduteCommissioni.GetView(Nothing)
            seduteCommissioni.Dispose()
        End If
        Me.SeduteCommissioniGridView.DataSource = Me.SeduteCommissioni
    End Sub

    Protected Sub SeduteCommissioniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles SeduteCommissioniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub calcolaTotali()
        Dim totalePresenti = Me.PresenzeCommissione.Where(Function(w) w.Presenza = True).Count
        Dim assentiGiustificati = Me.PresenzeCommissione.Where(Function(w) w.AssenzaGiustificata = True).Count
        Dim assentiIngiustificati = Me.PresenzeCommissione.Where(Function(w) w.AssenzaIngiustificata = True).Count

        Me.txtCostoSeduta.Value = Me.txtCostoOrario.Value * totalePresenti
        Me.txtConsiglieriPresenti.Value = totalePresenti
        Me.txtConsiglieriAssentigiustificati.Value = assentiGiustificati
        Me.txtConsiglieriAssentiIngiustificati.Value = assentiIngiustificati

    End Sub

    Private Sub azzeraTotali()

        Me.txtCostoSeduta.Value = 0
        Me.txtConsiglieriPresenti.Value = 0
        Me.txtConsiglieriAssentigiustificati.Value = 0
        Me.txtConsiglieriAssentiIngiustificati.Value = 0

    End Sub

    Private Sub CaricaCommissioni()
        Dim commissioni As New ParsecAtt.CommissioniRepository
        Me.CommissioniComboBox.DataSource = commissioni.GetView(Nothing)
        Me.CommissioniComboBox.DataTextField = "Descrizione"
        Me.CommissioniComboBox.DataValueField = "Id"
        Me.CommissioniComboBox.DataBind()
        Me.CommissioniComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.CommissioniComboBox.SelectedIndex = 0
        commissioni.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.SeduteCommissioni = Nothing
        Me.SeduteCommissioniGridView.Rebind()
    End Sub

    Private Sub AggiornaGrigliaPresenzeCommissione()
        Me.ConsiglieriGridView.DataSource = Me.PresenzeCommissione
        Me.ConsiglieriGridView.DataBind()
    End Sub

    Private Sub Search()
        Dim seduteCommissioni As New ParsecAtt.SeduteCommissioneRepository
        Dim filtro As ParsecAtt.FiltroSedutaCommissione = Me.GetFiltro
        Me.SeduteCommissioni = seduteCommissioni.GetView(filtro)
        seduteCommissioni.Dispose()
        Me.SeduteCommissioniGridView.Rebind()
    End Sub

    Private Sub ResettaVista()
        Me.CommissioniComboBox.SelectedIndex = 0
        Me.PresidenteTextBox.Text = String.Empty
        Me.ViceTextBox.Text = String.Empty
        Me.SedutaCommissione = Nothing
        Me.PresenzeCommissione = New List(Of ParsecAtt.PresenzaCommissione)
        Me.AggiornaGrigliaPresenzeCommissione()
        'Me.DataSedutaTextBox.SelectedDate = Now
        Me.DataSedutaTextBox.SelectedDate = Nothing
        'Me.OrarioSedutaTextBox.SelectedDate = Now
        Me.OrarioSedutaTextBox.SelectedDate = Nothing
        azzeraTotali()
        Me.CaricaCommissioni()
    End Sub


    Private Sub AggiornaVista(sedutaCommissione As ParsecAtt.SedutaCommissione)


        Me.ResettaVista()
        Try
            Me.CommissioniComboBox.Items.FindItemByValue(sedutaCommissione.IdCommissione).Selected = True
        Catch ex As Exception
            Dim commissioneRepository As New ParsecAtt.CommissioniRepository
            Dim commissione = commissioneRepository.GetById(sedutaCommissione.IdCommissione)
            If commissione Is Nothing Then
                ParsecUtility.Utility.MessageBox("Commissione non trovata!", False)
                Exit Sub
            Else
                Me.CommissioniComboBox.Items.Insert(sedutaCommissione.IdCommissione, New Telerik.Web.UI.RadComboBoxItem(commissione.Descrizione, sedutaCommissione.IdCommissione.ToString))
                Me.CommissioniComboBox.Items.FindItemByValue(sedutaCommissione.IdCommissione).Selected = True
            End If
        End Try


        Me.DataSedutaTextBox.SelectedDate = sedutaCommissione.Data
        Me.OrarioSedutaTextBox.SelectedDate = sedutaCommissione.Data

        If Not String.IsNullOrEmpty(sedutaCommissione.Presidente) Then
            Me.PresidenteTextBox.Text = sedutaCommissione.Presidente
        End If
        If Not String.IsNullOrEmpty(sedutaCommissione.VicePresidente) Then
            Me.ViceTextBox.Text = sedutaCommissione.VicePresidente
        End If
        Me.SedutaCommissione = sedutaCommissione

        'Me.PresenzeCommissione = sedutaCommissione.Commissione.Consiglieri

        Dim presenzaCommissioneRepository As New ParsecAtt.PresenzaCommissioneRepository
        Me.PresenzeCommissione = presenzaCommissioneRepository.GetListaConsiglieriByIdSeduta(sedutaCommissione.Id)
        presenzaCommissioneRepository.Dispose()

        calcolaTotali()




    End Sub


    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim seduteCommissioni As New ParsecAtt.SeduteCommissioneRepository
        Dim sedutaCommissione As ParsecAtt.SedutaCommissione = seduteCommissioni.GetById(id)
        seduteCommissioni.Dispose()
        Me.AggiornaVista(sedutaCommissione)
    End Sub

    Private Sub Delete()
        Dim seduteCommissioni As New ParsecAtt.SeduteCommissioneRepository
        Try
            seduteCommissioni.Delete(Me.SedutaCommissione.Id)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            seduteCommissioni.Dispose()
        End Try
    End Sub



    Private Sub Save()

        Dim seduteCommissioni As New ParsecAtt.SeduteCommissioneRepository

        '*****************************************************************************************************************
        'Aggiorno il modello.
        '*****************************************************************************************************************
        Dim sedutaCommissione As ParsecAtt.SedutaCommissione = seduteCommissioni.CreateFromInstance(Me.SedutaCommissione)

        sedutaCommissione.IdCommissione = Nothing
        If Me.CommissioniComboBox.SelectedIndex > 0 Then
            sedutaCommissione.IdCommissione = CInt(Me.CommissioniComboBox.SelectedValue)
        End If

        sedutaCommissione.Data = Nothing
        If Me.DataSedutaTextBox.SelectedDate.HasValue Then
            If Me.OrarioSedutaTextBox.SelectedDate.HasValue Then
                sedutaCommissione.Data = Me.DataSedutaTextBox.SelectedDate.Value.Date.AddHours(Me.OrarioSedutaTextBox.SelectedDate.Value.Hour).AddMinutes(Me.OrarioSedutaTextBox.SelectedDate.Value.Minute)
            Else
                sedutaCommissione.Data = Me.DataSedutaTextBox.SelectedDate.Value.Date.AddHours(Now.Hour).AddMinutes(Now.Minute)
            End If
        End If


        'associo le presenze (ciclo sulla Griglia)
        Dim listaPresenzeTemp = New List(Of ParsecAtt.PresenzaCommissione)
        Dim presenzaSedutaRepository As New ParsecAtt.PresenzaCommissioneRepository
        For Each item In Me.PresenzeCommissione
            Dim presenza As New ParsecAtt.PresenzaCommissione
            presenza.Presenza = item.Presenza
            presenza.AssenzaGiustificata = item.AssenzaGiustificata
            presenza.AssenzaIngiustificata = item.AssenzaIngiustificata
            presenza.IdConsigliere = item.IdConsigliere
            presenza.Nominativo = item.Nominativo
            listaPresenzeTemp.Add(presenza)
        Next
        presenzaSedutaRepository.Dispose()
        sedutaCommissione.ListaPresenze = listaPresenzeTemp

        '*****************************************************************************************************************
        Try
            seduteCommissioni.Save(sedutaCommissione)
            sedutaCommissione = seduteCommissioni.GetById(seduteCommissioni.SedutaCommissione.Id)
            Me.AggiornaVista(sedutaCommissione)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            seduteCommissioni.Dispose()
        End Try
    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroSedutaCommissione
        Dim filtro As New ParsecAtt.FiltroSedutaCommissione
        If Me.CommissioniComboBox.SelectedIndex > 0 Then
            filtro.IdCommissione = CInt(Me.CommissioniComboBox.SelectedValue)
        End If
        If Me.DataSedutaTextBox.SelectedDate.HasValue Then
            filtro.DataSeduta = Me.DataSedutaTextBox.SelectedDate
        End If
        Return filtro
    End Function

#End Region

#Region "GESTIONE GRIGLIA CONSIGLIERI"


    'Private Sub ConsiglieriGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ConsiglieriGridView.ItemCreated
    '    If TypeOf e.Item Is GridHeaderItem Then
    '        e.Item.Style.Add("position", "relative")
    '        e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
    '        e.Item.Style.Add("z-index", "99")
    '        e.Item.Style.Add("background-color", "White")
    '    End If
    'End Sub

    Private Sub ConsiglieriGridView_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ConsiglieriGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim IdConsigliere As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("IdConsigliere")

            Dim PresenzaRadioButton As CheckBox = CType(e.Item.FindControl("Presenza"), RadioButton)
            Dim AssenzaGiustificataRadioButton As CheckBox = CType(e.Item.FindControl("AssenzaGiustificata"), RadioButton)
            Dim AssenzaIngiustificataRadioButton As CheckBox = CType(e.Item.FindControl("AssenzaIngiustificata"), RadioButton)

            PresenzaRadioButton.Attributes.Add("onclick", "document.getElementById('" & Me.IdConsigliereTextBox.ClientID & "').value=" & IdConsigliere & ";document.getElementById('" & Me.PresenzaSedutaButton.ClientID & "').click();")
            AssenzaGiustificataRadioButton.Attributes.Add("onclick", "document.getElementById('" & Me.IdConsigliereTextBox.ClientID & "').value=" & IdConsigliere & ";document.getElementById('" & Me.AssenzaSedutaButton.ClientID & "').click();")
            AssenzaIngiustificataRadioButton.Attributes.Add("onclick", "document.getElementById('" & Me.IdConsigliereTextBox.ClientID & "').value=" & IdConsigliere & ";document.getElementById('" & Me.AssenzaIngiustificataSedutaButton.ClientID & "').click();")

            Dim referente = Me.PresenzeCommissione.Where(Function(c) c.IdConsigliere = IdConsigliere).FirstOrDefault

            PresenzaRadioButton.Checked = referente.Presenza
            AssenzaGiustificataRadioButton.Checked = referente.AssenzaGiustificata
            AssenzaIngiustificataRadioButton.Checked = referente.AssenzaIngiustificata

        End If
    End Sub

    Protected Sub presenzaSedutaButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PresenzaSedutaButton.Click
        If Not String.IsNullOrEmpty(Me.IdConsigliereTextBox.Text) Then
            Dim id As Integer = CInt(Me.IdConsigliereTextBox.Text)
            Dim partecipante As ParsecAtt.PresenzaCommissione = Me.PresenzeCommissione.Where(Function(c) c.IdConsigliere = id).FirstOrDefault
            partecipante.Presenza = True 'Not partecipante.Presenza
            ' If (partecipante.Presenza) Then
            partecipante.AssenzaIngiustificata = False
            partecipante.AssenzaGiustificata = False
            'End If
            calcolaTotali()
        End If
    End Sub

    Protected Sub AssenzaSedutaButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AssenzaSedutaButton.Click
        If Not String.IsNullOrEmpty(Me.IdConsigliereTextBox.Text) Then
            Dim id As Integer = CInt(Me.IdConsigliereTextBox.Text)
            Dim partecipante As ParsecAtt.PresenzaCommissione = Me.PresenzeCommissione.Where(Function(c) c.IdConsigliere = id).FirstOrDefault
            partecipante.AssenzaGiustificata = True ' Not partecipante.AssenzaGiustificata
            'If (partecipante.AssenzaGiustificata) Then
            partecipante.AssenzaIngiustificata = False
            partecipante.Presenza = False
            'End If
            calcolaTotali()
        End If
    End Sub

    Protected Sub AssenzaIngiustificataSedutaButtonn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AssenzaIngiustificataSedutaButton.Click
        If Not String.IsNullOrEmpty(Me.IdConsigliereTextBox.Text) Then
            Dim id As Integer = CInt(Me.IdConsigliereTextBox.Text)
            Dim partecipante As ParsecAtt.PresenzaCommissione = Me.PresenzeCommissione.Where(Function(c) c.IdConsigliere = id).FirstOrDefault
            partecipante.AssenzaIngiustificata = True ' Not partecipante.AssenzaIngiustificata
            'If (partecipante.AssenzaIngiustificata) Then
            partecipante.Presenza = False
            partecipante.AssenzaGiustificata = False
            'End If
            calcolaTotali()
        End If
    End Sub

#End Region


    Protected Sub CommissioniComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles CommissioniComboBox.SelectedIndexChanged
        If (Me.CommissioniComboBox.SelectedIndex > 0) Then
            Dim idCommissione = CInt(Me.CommissioniComboBox.SelectedValue)

            Dim commissioniRep As New ParsecAtt.CommissioniRepository
            Dim commissione As ParsecAtt.Commissione = commissioniRep.GetById(idCommissione)

            'Dim commissioneRep As New ParsecAtt.CommissioniRepository
            Me.PresenzeCommissione = commissioniRep.CreaListaPresenzeSedutaCommissioneInizialeByIdCommissione(CommissioniComboBox.SelectedValue)
            AggiornaGrigliaPresenzeCommissione()

            Me.PresidenteTextBox.Text = commissione.Presidente
            Me.ViceTextBox.Text = commissione.VicePresidente

            commissioniRep.Dispose()
            calcolaTotali()
        Else
            Me.ResettaVista()
            calcolaTotali()
        End If

    End Sub

End Class