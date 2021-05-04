Partial Class ClientePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"


    Public Property Cliente() As ParsecAdmin.Cliente
        Get
            Return CType(Session("ClientePage_Cliente"), ParsecAdmin.Cliente)
        End Get
        Set(ByVal value As ParsecAdmin.Cliente)
            Session("ClientePage_Cliente") = value
        End Set
    End Property

    Private Property Convenzioni() As List(Of ParsecAdmin.ConvenzioneBancaria)
        Get
            Return CType(Session("ClientePage_Convenzioni"), List(Of ParsecAdmin.ConvenzioneBancaria))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.ConvenzioneBancaria))
            Session("ClientePage_Convenzioni") = value
        End Set
    End Property

    Private Property Convenzione() As ParsecAdmin.ConvenzioneBancaria
        Get
            Return CType(Session("ClientePage_Convenzione"), ParsecAdmin.ConvenzioneBancaria)
        End Get
        Set(ByVal value As ParsecAdmin.ConvenzioneBancaria)
            Session("ClientePage_Convenzione") = value
        End Set
    End Property

    Public Property AreaOrganizzativaOmogenea As ParsecAdmin.ComuneAreaOrganizzativaOmogenea
        Get
            Return CType(Session("ClientePage_AreaOrganizzativaOmogenea"), ParsecAdmin.ComuneAreaOrganizzativaOmogenea)
        End Get
        Set(value As ParsecAdmin.ComuneAreaOrganizzativaOmogenea)
            Session("ClientePage_AreaOrganizzativaOmogenea") = value
        End Set
    End Property

    Public Property AreeOrganizzativeOmogenee As List(Of ParsecAdmin.ComuneAreaOrganizzativaOmogenea)
        Get
            Return CType(Session("ClientePage_AreeOrganizzativeOmogenee"), List(Of ParsecAdmin.ComuneAreaOrganizzativaOmogenea))
        End Get
        Set(value As List(Of ParsecAdmin.ComuneAreaOrganizzativaOmogenea))
            Session("ClientePage_AreeOrganizzativeOmogenee") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Cliente"



        If Not Me.Page.IsPostBack Then
            Dim clientRepository As New ParsecAdmin.ClientRepository
            Dim clienteCorrente As ParsecAdmin.Cliente = clientRepository.GetQuery.FirstOrDefault
            clientRepository.Dispose()

            If Not clienteCorrente Is Nothing Then
                Me.Cliente = clienteCorrente


                CaricaProvince(clienteCorrente)
                CaricaComuni(clienteCorrente)

                Me.TipoClienteComboBox.FindItemByValue(clienteCorrente.Tipo).Selected = True
                Me.DescrizioneTextBox.Text = clienteCorrente.Descrizione
                Me.IndirizzoTextBox.Text = clienteCorrente.Indirizzo
                Me.CapTextBox.Text = clienteCorrente.CAP
                Me.CodiceClienteTextBox.Text = clienteCorrente.Identificativo
                Me.CodiceLicenzaTextBox.Text = clienteCorrente.CodLicenza
                Me.PIVATextBox.Text = clienteCorrente.PIVA
                Me.TelefonoTextBox.Text = clienteCorrente.Telefono
                Me.CodiceAmministrazioneTextBox.Text = clienteCorrente.CodiceAmministrazione
                Me.CodiceFiscaleTextBox.Text = clienteCorrente.CodiceFiscale
                Me.IndirizzoIpTextBox.Text = clienteCorrente.IndirizzoIP
                Me.TipoClienteComboBox.Focus()

            End If

            Me.Convenzioni = Nothing
            Me.Convenzione = Nothing
            Me.AreeOrganizzativeOmogenee = Nothing
            Me.AreaOrganizzativaOmogenea = Nothing


        End If


        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.PubblicazioniMultiPage.Style.Add("width", widthStyle)
        Me.GeneralePanel.Style.Add("width", widthStyle)
        Me.ConvenzioniPanel.Style.Add("width", widthStyle)
        Me.ConvenzioniGridView.Style.Add("width", widthStyle)

        Me.AreeOrganizzativeOmogeneePanel.Style.Add("width", widthStyle)
        Me.AreeOrganizzativeOmogeneeGridView.Style.Add("width", widthStyle)


        Me.SalvaConvenzioneButton.Attributes.Add("onclick", "HidePanel();hide = true; ")
        Me.AggiungiConvenzioneBancariaImageButton.Attributes.Add("onclick", "ShowPanel();hide=false;")

        Me.SalvaAreaOrganizzativaOmogeneaButton.Attributes.Add("onclick", "HidePanelAOO();hideAOO = true;")
        Me.AggiungiAreaOrganizzativaOmogeneaImageButton.Attributes.Add("onclick", "ShowPanelAOO();hideAOO=false;")
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Not Me.Convenzioni Is Nothing Then
            Me.PubblicazioniTabStrip.Tabs(1).Text = "Convenzioni Bancarie&nbsp;" & If(Me.Convenzioni.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Convenzioni.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Dim message As String = String.Empty
        Try
            Me.Save()
        Catch ex As Exception
            message = ex.Message
        End Try
        If String.IsNullOrEmpty(message) Then
            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
        Else
            ParsecUtility.Utility.MessageBox(message, False)
        End If
    End Sub

    Protected Sub ComuneComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ComuneComboBox.SelectedIndexChanged
        Dim comuneRepository As New ParsecAdmin.ComuneRepository
        Dim comune As ParsecAdmin.Comune = comuneRepository.GetQuery.Where(Function(c) c.Id = e.Value).FirstOrDefault
        If Not comune Is Nothing Then
            Me.CapTextBox.Text = comune.Cap
            Me.DescrizioneTextBox.Text = "COMUNE DI " & comune.Descrizione
            Me.CodiceClienteTextBox.Text = comune.CodiceIstat.ToUpper
        End If
        comuneRepository.Dispose()
    End Sub

    Protected Sub ProvinciaComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ProvinciaComboBox.SelectedIndexChanged
        Dim comuneRepository As New ParsecAdmin.ComuneRepository
        Dim provinciaRepository As New ParsecAdmin.ProvinciaRepository
        Dim provincia As ParsecAdmin.Provincia = provinciaRepository.GetQuery.Where(Function(c) c.Descrizione.ToLower = e.Text.ToLower).FirstOrDefault
        If Not provincia Is Nothing Then
            Dim comuniPerProvincia As List(Of ParsecAdmin.Comune) = comuneRepository.GetQuery.Where(Function(c) c.Provincia.ToLower = provincia.Sigla.ToLower).ToList
            With Me.ComuneComboBox
                .DataTextField = "Descrizione"
                .DataValueField = "Id"
                .DataSource = comuniPerProvincia
                .DataBind()
            End With
        End If
        comuneRepository.Dispose()
        provinciaRepository.Dispose()
    End Sub

    Protected Sub AnnullaConvenzioneButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaConvenzioneButton.Click
        If Me.Convenzione Is Nothing Then
            Me.ResettaConvenzione()
        Else
            Me.AggiornaVistaConvenzione()
        End If
    End Sub

    Protected Sub SalvaConvenzioneButton_Click(sender As Object, e As System.EventArgs) Handles SalvaConvenzioneButton.Click
        If (Me.DenominazioneConvenzioneTextBox.Text <> "" And Me.ABI.Text <> "" And Me.IbanTextBox.Text <> "" And Me.CinTextBox.Text <> "" And
            Me.CabTextBox.Text <> "" And Me.NumeroContoCorrenteTextBox.Text <> "") Then
            If Not (Me.IbanTextBox.Text.Contains(Me.AbiTextBox.Text) And Me.IbanTextBox.Text.Contains(Me.CabTextBox.Text) And
                    Me.IbanTextBox.Text.Contains(Me.CinTextBox.Text) And Me.IbanTextBox.Text.Contains(Me.NumeroContoCorrenteTextBox.Text)) Then
                ParsecUtility.Utility.MessageBox("In base ai dati specificati - ABI, CAB, CIN, Numero Conto - l'IBAN non corrisponde!", False)
                Exit Sub
            End If
            Dim convenzione As ParsecAdmin.ConvenzioneBancaria = Nothing
            If Me.Convenzione Is Nothing Then
                convenzione = New ParsecAdmin.ConvenzioneBancaria
                convenzione.id = -1
                If Me.Convenzioni.Count > 0 Then
                    Dim id = Me.Convenzioni.Min(Function(c) c.id) - 1
                    If id < 0 Then convenzione.id = id
                End If
            Else
                convenzione = Me.Convenzione
            End If
            convenzione.abi = Me.AbiTextBox.Text
            convenzione.cab = Me.CabTextBox.Text
            convenzione.cin = Me.CinTextBox.Text
            convenzione.denominazione = Me.DenominazioneConvenzioneTextBox.Text
            convenzione.iban = Me.IbanTextBox.Text
            convenzione.numeroContoCorrente = Me.NumeroContoCorrenteTextBox.Text
            If Me.Convenzione Is Nothing Then
                Me.Convenzioni.Add(convenzione)
            Else
                Dim con = Me.Convenzioni.Where(Function(c) c.id = convenzione.id).FirstOrDefault
                If Not con Is Nothing Then
                    Me.Convenzioni.Remove(con)
                    Me.Convenzioni.Add(convenzione)
                End If
            End If
            Me.ConvenzioniGridView.Rebind()
            Me.ResettaConvenzione()
            Me.TitoloRicercaLabel.Text = "Nuova Convenzione Bancaria"
        Else
            ParsecUtility.Utility.MessageBox("Dati della Convenzione Bancaria incompleti!", False)
        End If
        Me.Convenzione = Nothing
    End Sub

    Protected Sub AggiornaConvenzioneBancariaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaConvenzioneBancariaImageButton.Click
        Me.ResettaConvenzione()
    End Sub

    Protected Sub AggiornaAreaOrganizzativaOmogeneaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaAreaOrganizzativaOmogeneaImageButton.Click
        Me.ResettaVistaAreaOrganizzativaOmogenea()
    End Sub

    Protected Sub SalvaAreaOrganizzativaOmogeneaButton_Click(sender As Object, e As System.EventArgs) Handles SalvaAreaOrganizzativaOmogeneaButton.Click
        If Not String.IsNullOrEmpty(Me.CodiceAreaOrganizzativaOmogeneaTextBox.Text) AndAlso Not String.IsNullOrEmpty(Me.DenominazioneAreaOrganizzativaOmogeneaTextBox.Text) AndAlso Not String.IsNullOrEmpty(Me.IndirizzoTelematicoTextBox.Text) Then

            Dim nuovaArea As Boolean = Me.AreaOrganizzativaOmogenea Is Nothing

            Dim area As ParsecAdmin.ComuneAreaOrganizzativaOmogenea = Nothing
            'NUOVA AREA ORGANIZZATIVA OMOGENEA
            If nuovaArea Then
                area = New ParsecAdmin.ComuneAreaOrganizzativaOmogenea
                area.Id = -1
                'ASSEGNO UN ID TEMPORANEO
                If Me.AreeOrganizzativeOmogenee.Count > 0 Then
                    Dim id = Me.AreeOrganizzativeOmogenee.Min(Function(c) c.Id) - 1
                    If id < 0 Then
                        area.Id = id
                    End If
                End If
            Else
                area = Me.AreaOrganizzativaOmogenea
            End If
            area.DenominazioneAOO = Me.DenominazioneAreaOrganizzativaOmogeneaTextBox.Text
            area.CodiceAOO = Me.CodiceAreaOrganizzativaOmogeneaTextBox.Text
            area.IndirizzoTelematico = Me.IndirizzoTelematicoTextBox.Text

            If Not nuovaArea Then
                'SE L'AREA ESISTE
                Dim aoo = Me.AreeOrganizzativeOmogenee.Where(Function(c) c.Id = area.Id).FirstOrDefault
                If Not aoo Is Nothing Then
                    Me.AreeOrganizzativeOmogenee.Remove(aoo)
                End If
            End If

            'AGGIORNO LA GRIGLIA
            Me.AreeOrganizzativeOmogenee.Add(area)
            Me.AreeOrganizzativeOmogeneeGridView.Rebind()

            'RESETTO
            Me.ResettaVistaAreaOrganizzativaOmogenea()
        Else
            ParsecUtility.Utility.MessageBox("E' necessario inserire i campi obbligatori!", False)
        End If


    End Sub

    Protected Sub AnnullaAreaOrganizzativaOmogeneaButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaAreaOrganizzativaOmogeneaButton.Click
        If Me.AreaOrganizzativaOmogenea Is Nothing Then
            Me.ResettaVistaAreaOrganizzativaOmogenea()
        Else
            Me.AggiornaVistaAreaOrganizzativaOmogenea()
        End If
    End Sub


#End Region

#Region "GRIGLIA CONVENZIONI"

    Protected Sub ConvenzioniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ConvenzioniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As Telerik.Web.UI.RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), Telerik.Web.UI.RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub ConvenzioniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ConvenzioniGridView.NeedDataSource
        If Me.Convenzioni Is Nothing Then
            Dim clientRepository As New ParsecAdmin.ClientRepository
            Me.Convenzioni = clientRepository.getConvenzioniBancarie(Me.Cliente.Id)
        End If
        Me.ConvenzioniGridView.DataSource = Me.Convenzioni
    End Sub

    Protected Sub ConvenzioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ConvenzioniGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Me.EliminaConvenzione(e)
            Case "Select"
                Me.ModificaConvenzione(e)
        End Select
    End Sub

    Protected Sub ConvenzioniGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ConvenzioniGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                btn.Attributes.Add("onclick", "ShowPanel();hide=false;")
            End If
        End If
    End Sub


#End Region


#Region "GRIGLIA AREE ORGANIZZATIVE OMOGENEE"

    Protected Sub AreeOrganizzativeOmogeneeGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AreeOrganizzativeOmogeneeGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As Telerik.Web.UI.RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), Telerik.Web.UI.RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub AreeOrganizzativeOmogeneeGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles AreeOrganizzativeOmogeneeGridView.NeedDataSource
        If Me.AreeOrganizzativeOmogenee Is Nothing Then
            Dim aree As New ParsecAdmin.ComuneAreaOrganizzativaOmogeneaRepository
            Me.AreeOrganizzativeOmogenee = aree.GetView(Me.Cliente.Id)
        End If
        Me.AreeOrganizzativeOmogeneeGridView.DataSource = Me.AreeOrganizzativeOmogenee
    End Sub

    Protected Sub AreeOrganizzativeOmogeneeGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AreeOrganizzativeOmogeneeGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Me.EliminaAreaOrganizzativaOmogenea(e)
            Case "Select"
                Me.ModificaAreaOrganizzativaOmogenea(e)
        End Select
    End Sub

    Protected Sub AreeOrganizzativeOmogeneeGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AreeOrganizzativeOmogeneeGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                btn.Attributes.Add("onclick", "ShowPanelAOO();hideAOO=false;")
            End If
        End If
    End Sub

    Private Sub EliminaAreaOrganizzativaOmogenea(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Me.AreeOrganizzativeOmogenee.RemoveAt(e.Item.ItemIndex)
    End Sub

    Private Sub ModificaAreaOrganizzativaOmogenea(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Me.ResettaVistaAreaOrganizzativaOmogenea()
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Me.AreaOrganizzativaOmogenea = Me.AreeOrganizzativeOmogenee.Where(Function(c) c.Id = id).FirstOrDefault
        Me.AggiornaVistaAreaOrganizzativaOmogenea()
        Me.TitoloRicercaAreaOrganizzativaOmogeneaLabel.Text = "Modifica Area Organizzativa Omogenea"
    End Sub

    Private Sub AggiornaVistaAreaOrganizzativaOmogenea()
        Me.CodiceAreaOrganizzativaOmogeneaTextBox.Text = Me.AreaOrganizzativaOmogenea.CodiceAOO
        Me.DenominazioneAreaOrganizzativaOmogeneaTextBox.Text = Me.AreaOrganizzativaOmogenea.DenominazioneAOO
        Me.IndirizzoTelematicoTextBox.Text = Me.AreaOrganizzativaOmogenea.IndirizzoTelematico
    End Sub

    Private Sub ResettaVistaAreaOrganizzativaOmogenea()
        Me.CodiceAreaOrganizzativaOmogeneaTextBox.Text = String.Empty
        Me.DenominazioneAreaOrganizzativaOmogeneaTextBox.Text = String.Empty
        Me.IndirizzoTelematicoTextBox.Text = String.Empty
        Me.TitoloRicercaAreaOrganizzativaOmogeneaLabel.Text = "Nuova Area Organizzativa Omogenea"
        Me.AreaOrganizzativaOmogenea = Nothing
    End Sub


#End Region

#Region "METODI PRIVATI"

    Private Sub AggiornaVistaConvenzione()
        Me.AbiTextBox.Text = Me.Convenzione.abi
        Me.CabTextBox.Text = Me.Convenzione.cab
        Me.CinTextBox.Text = Me.Convenzione.cin
        Me.DenominazioneConvenzioneTextBox.Text = Me.Convenzione.denominazione
        Me.IbanTextBox.Text = Me.Convenzione.iban
        Me.NumeroContoCorrenteTextBox.Text = Me.Convenzione.numeroContoCorrente
    End Sub

    Private Sub CaricaComuni(ByVal cc As ParsecAdmin.Cliente)
        Dim comuneRepository As New ParsecAdmin.ComuneRepository

        Me.ComuneComboBox.DataSource = comuneRepository.GetQuery.Where(Function(c) c.Provincia.ToUpper = cc.Provincia.ToUpper)
        Me.ComuneComboBox.DataTextField = "Descrizione"
        Me.ComuneComboBox.DataValueField = "Id"
        Me.ComuneComboBox.DataBind()
        Try
            Me.ComuneComboBox.FindItemByText(cc.Citta.ToUpper, True).Selected = True
        Catch ex As Exception
        End Try
        comuneRepository.Dispose()
    End Sub

    Private Sub CaricaProvince(ByVal cc As ParsecAdmin.Cliente)
        Dim provinciaRepository As New ParsecAdmin.ProvinciaRepository

        Me.ProvinciaComboBox.DataSource = provinciaRepository.GetQuery.OrderBy(Function(c) c.Descrizione)
        Me.ProvinciaComboBox.DataTextField = "Descrizione"
        Me.ProvinciaComboBox.DataValueField = "Sigla"
        Me.ProvinciaComboBox.DataBind()
        Try
            Me.ProvinciaComboBox.FindItemByValue(cc.Provincia.ToUpper, True).Selected = True
        Catch ex As Exception
        End Try
        provinciaRepository.Dispose()
    End Sub

    Private Sub EliminaConvenzione(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Me.Convenzioni.RemoveAt(e.Item.ItemIndex)
    End Sub

    Private Sub ModificaConvenzione(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("id")
        Me.Convenzione = Me.Convenzioni.Where(Function(c) c.id = id).FirstOrDefault
        Me.AggiornaVistaConvenzione()
        Me.TitoloRicercaLabel.Text = "Modifica Convenzione Bancaria"
    End Sub

    Private Sub ResettaConvenzione()
        Me.AbiTextBox.Text = String.Empty
        Me.CabTextBox.Text = String.Empty
        Me.CinTextBox.Text = String.Empty
        Me.DenominazioneConvenzioneTextBox.Text = String.Empty
        Me.IbanTextBox.Text = String.Empty
        Me.NumeroContoCorrenteTextBox.Text = String.Empty
        Me.TitoloRicercaLabel.Text = "Nuova Convenzione Bancaria"
    End Sub

    Private Sub Save()
        Dim clienti As New ParsecAdmin.ClientRepository
        Dim cliente As ParsecAdmin.Cliente = clienti.CreateFromInstance(Me.Cliente)

        cliente.Descrizione = If(String.IsNullOrEmpty(Me.DescrizioneTextBox.Text), "", Me.DescrizioneTextBox.Text)
        cliente.Identificativo = If(String.IsNullOrEmpty(Me.CodiceClienteTextBox.Text), "", Me.CodiceClienteTextBox.Text)
        cliente.Indirizzo = If(String.IsNullOrEmpty(Me.IndirizzoTextBox.Text), "", Me.IndirizzoTextBox.Text)
        cliente.Citta = Me.ComuneComboBox.SelectedItem.Text
        cliente.Provincia = Me.ProvinciaComboBox.SelectedItem.Value
        cliente.CAP = If(String.IsNullOrEmpty(Me.CapTextBox.Text), "", Me.CapTextBox.Text)
        cliente.CodLicenza = If(String.IsNullOrEmpty(Me.CodiceLicenzaTextBox.Text), "", Me.CodiceLicenzaTextBox.Text)
        cliente.Tipo = Me.TipoClienteComboBox.SelectedItem.Value
        cliente.PIVA = If(String.IsNullOrEmpty(Me.PIVATextBox.Text), "", Me.PIVATextBox.Text)
        cliente.Telefono = If(String.IsNullOrEmpty(Me.TelefonoTextBox.Text), "", Me.TelefonoTextBox.Text)
        cliente.CodiceAmministrazione = If(String.IsNullOrEmpty(Me.CodiceAmministrazioneTextBox.Text), "", Me.CodiceAmministrazioneTextBox.Text)
        cliente.ConvenzioniBancarie = Me.Convenzioni
        cliente.AreeOrganizzativeOmogenee = Me.AreeOrganizzativeOmogenee

        cliente.IndirizzoIP = If(String.IsNullOrEmpty(Me.IndirizzoIpTextBox.Text.Trim), "", Me.IndirizzoIpTextBox.Text.Trim)

        cliente.CodiceFiscale = If(String.IsNullOrEmpty(Me.CodiceFiscaleTextBox.Text.Trim), "", Me.CodiceFiscaleTextBox.Text.Trim)


        Try
            clienti.Save(cliente)
            Me.Cliente = clienti.Cliente
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            clienti.Dispose()
        End Try
    End Sub

#End Region



 
End Class