Imports ParsecAtt
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneModelliPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property FiltroModello() As ParsecAtt.FiltroModello
        Get
            Return CType(Session("GestioneModelliPage_FiltroModello"), ParsecAtt.FiltroModello)
        End Get
        Set(ByVal value As ParsecAtt.FiltroModello)
            Session("GestioneModelliPage_FiltroModello") = value
        End Set
    End Property

    Public Property Modello() As ParsecAtt.Modello
        Get
            Return CType(Session("GestioneModelliPage_Modello"), ParsecAtt.Modello)
        End Get
        Set(ByVal value As ParsecAtt.Modello)
            Session("GestioneModelliPage_Modello") = value
        End Set
    End Property

    Public Property Modelli() As List(Of ParsecAtt.Modello)
        Get
            Return CType(Session("GestioneModelliPage_Modelli"), List(Of ParsecAtt.Modello))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Modello))
            Session("GestioneModelliPage_Modelli") = value
        End Set
    End Property

    Public Property Firme() As List(Of ParsecAtt.ModelloFirma)
        Get
            Return CType(Session("GestioneModelliPage_Firme"), List(Of ParsecAtt.ModelloFirma))
        End Get
        Set(ByVal value As List(Of ParsecAtt.ModelloFirma))
            Session("GestioneModelliPage_Firme") = value
        End Set
    End Property

#End Region


#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Gestione Modelli"

        Me.CaricaTipologieDocumento()
        Me.CaricaTipologieRegistro()
        Me.CaricaMetaModelli()
        Me.CaricaModelliCollegati()
        Me.CaricaTipologieSeduta()
        Me.CaricaIter()
        Me.CaricaSezioneTrasparente()

        If Not Me.Page.IsPostBack Then
            Me.Modelli = Nothing
            Me.FiltroModello = Nothing
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.ModelliGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

            Me.CaricaTipologieDocumentoFiltro()
            Me.CaricaStatoModello()

        End If

        Me.DisabilitaPulsantePredefinito.Attributes.Add("onclick", "return false;")
        ParsecUtility.Utility.RegisterDefaultButton(Me.FiltroTextBox, Me.AggiungiFirmaImageButton)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If


        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.ModelliMultiPage.Style.Add("width", widthStyle)
        Me.PannelloFirme.Style.Add("width", widthStyle)
        Me.ModelliGridView.Style.Add("width", widthStyle)
        Me.FirmeGridView.Style.Add("width", widthStyle)

        Me.ChiudiRicercaImageButton.Attributes.Add("onclick", "HideSearchPanel();hideSearchPanel = true;")
        Me.SalvaButton.Attributes.Add("onclick", "HideSearchPanel();hideSearchPanel=true;")

        Me.FiltraImageButton.Attributes.Add("onclick", "ShowSearchPanel();hideSearchPanel=false;return false;")


    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        Me.AggiornaGrigliaFirme()
        ParsecUtility.Utility.Confirm("Eliminare il modello selezionato?", False, Not Me.Modello Is Nothing)

        If Not Me.Modelli Is Nothing Then
            Me.TitoloElencoModelliLabel.Text = "Elenco Modelli&nbsp;&nbsp;&nbsp;" & If(Me.Modelli.Count > 0, "( " & Me.Modelli.Count.ToString & " )", "")
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Not Me.Modello Is Nothing Then

            Dim tipologieDocumento As New ParsecAtt.TipologieDocumentoRepository
            Dim tipologiaDocumento As ParsecAtt.TipologiaDocumento = tipologieDocumento.GetById(Modello.IdTipologiaDocumento)

            Me.RadToolBar.Items.FindItemByText("Salva").Enabled = tipologiaDocumento.Modellizzabile
            Me.RadToolBar.Items.FindItemByText("Elimina").Enabled = tipologiaDocumento.Modellizzabile AndAlso utenteCollegato.SuperUser
            tipologieDocumento.Dispose()

        End If

        'SELEZIONO LA RIGA
        If Not Me.Modello Is Nothing Then
            Dim item As GridDataItem = Me.ModelliGridView.MasterTableView.FindItemByKeyValue("Id", Me.Modello.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If

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
                    If Me.VerificaDati Then
                        Me.Save()
                        Me.AggiornaGriglia()
                    Else
                        Exit Sub
                    End If
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
                Me.FiltroModello = Nothing
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Modello Is Nothing Then
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.AggiornaGriglia()
                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)
                        End Try
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un modello!", False)
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
        If e.Item.Text = "Ricerca Avanzata" Then
            e.Item.Attributes.Add("onclick", "ShowSearchPanel();hideSearchPanel=false;return false;")
        End If
    End Sub

#End Region


#Region "EVENTI GRIGLIA"

    Protected Sub ModelliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ModelliGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.Anteprima(e.Item)
        End If
    End Sub

    Protected Sub ModelliGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ModelliGridView.NeedDataSource
        If Me.Modelli Is Nothing Then
            Dim modelli As New ParsecAtt.ModelliRepository
            If Me.FiltroModello Is Nothing Then
                Me.Modelli = modelli.GetView(Nothing)
            Else
                Me.Modelli = modelli.GetView(Me.FiltroModello)
            End If
            modelli.Dispose()
        End If
        Me.ModelliGridView.DataSource = Me.Modelli
    End Sub

    Protected Sub ModelliGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ModelliGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub FirmeGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FirmeGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaFirma(e.Item)
        End If
    End Sub

    Protected Sub FirmeGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FirmeGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                Dim message As String = "Eliminare l'elemento selezionato?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If
        End If
    End Sub


#End Region


#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub VisualizzaDocumento(ByVal nomeFile As String, ByVal enabled As Boolean)
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates") & nomeFile
        Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = "ViewDocument"}
        Dim data As String = openofficeParameters.CreateOpenParameter(datiInput)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, False, Nothing)
        End If
    End Sub


    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub Anteprima(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim modelli As New ParsecAtt.ModelliRepository
        Dim modello As ParsecAtt.Modello = modelli.GetById(id)
        Dim localPathModello As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates") & modello.FileName
        If Not IO.File.Exists(localPathModello) Then
            ParsecUtility.Utility.MessageBox("Il file del modello selezionato non esiste!", False)
        Else
            Me.VisualizzaDocumento(modello.FileName, True)
        End If
        modelli.Dispose()
    End Sub

    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder

        If Me.TipologieDocumentoComboBox.SelectedIndex = 0 Then
            message.AppendLine("La tipologia di documento a cui fa riferimento il modello.")
        End If

        If String.IsNullOrEmpty(Me.DescrizioneTextBox.Text) Then
            message.AppendLine("La descrizione del modello.")
        End If

        If Me.ModelliAdottatiComboBox.SelectedIndex = 0 AndAlso Me.PropostaCheckBox.Checked Then
            message.AppendLine("Il modello adottato.")
        End If

        'Se sto inserendo un nuovo modello.
        If Me.Modello Is Nothing Then
            If Me.MetaModelliComboBox.SelectedIndex = 0 Then
                message.AppendLine("Il meta modello a cui fa riferimento il modello.")
            Else
                Dim template As String = "metaModello.odt"
                Dim remotePath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")
                Dim input As String = remotePath & template
                If Not IO.File.Exists(input) Then
                    ParsecUtility.Utility.MessageBox(String.Format("Il file '{0}' non esiste!", template), False)
                    Return False
                End If
            End If
        End If


        If String.IsNullOrEmpty(Me.PrefissoTextBox.Text) Then
            message.AppendLine("Il prefisso del modello.")
        End If


        If message.Length > 0 Then
            message.Insert(0, "E' necessario specificare:" & vbCrLf)
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If

        Return message.Length = 0

    End Function

    Private Sub CaricaTipologieDocumento()
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Me.TipologieDocumentoComboBox.DataValueField = "Id"
        Me.TipologieDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologieDocumentoComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaDocumento With {.Modellizzabile = True})
        Me.TipologieDocumentoComboBox.DataBind()
        Me.TipologieDocumentoComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieDocumentoComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub CaricaTipologieDocumentoFiltro()
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Me.TipologieDocumentoFiltroComboBox.DataValueField = "Id"
        Me.TipologieDocumentoFiltroComboBox.DataTextField = "Descrizione"
        Me.TipologieDocumentoFiltroComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaDocumento With {.Modellizzabile = True})
        Me.TipologieDocumentoFiltroComboBox.DataBind()
        Me.TipologieDocumentoFiltroComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieDocumentoFiltroComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub CaricaStatoModello()
        Dim dati As New Dictionary(Of String, String)
        dati.Add(0, "Abilitati")
        dati.Add(1, "Disabilitati")
        Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})
        Me.StatoFiltroComboBox.DataSource = ds
        Me.StatoFiltroComboBox.DataTextField = "Descrizione"
        Me.StatoFiltroComboBox.DataValueField = "Id"
        Me.StatoFiltroComboBox.DataBind()
        Me.StatoFiltroComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", -1))
        Me.StatoFiltroComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaTipologieRegistro()
        Dim tipologie As New ParsecAtt.TipologieRegistroRepository
        Me.TipologieRegistroComboBox.DataValueField = "Id"
        Me.TipologieRegistroComboBox.DataTextField = "Descrizione"
        Me.TipologieRegistroComboBox.DataSource = tipologie.GetKeyValue(Nothing)
        Me.TipologieRegistroComboBox.DataBind()
        Me.TipologieRegistroComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieRegistroComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub CaricaMetaModelli()
        Dim metaModelli As New ParsecAtt.MetaModelliRegistroRepository
        Me.MetaModelliComboBox.DataValueField = "Id"
        Me.MetaModelliComboBox.DataTextField = "Descrizione"
        Me.MetaModelliComboBox.DataSource = metaModelli.GetKeyValue(Nothing)
        Me.MetaModelliComboBox.DataBind()
        Me.MetaModelliComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.MetaModelliComboBox.SelectedIndex = 0
        metaModelli.Dispose()
    End Sub

    Private Sub CaricaModelliCollegati()
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Dim idTipologieDocumento = (From s In tipologie.GetView(New ParsecAtt.FiltroTipologiaDocumento With {.Modellizzabile = True})
                  Select s.Id).ToList
        tipologie.Dispose()

        Dim modelli As New ParsecAtt.ModelliRepository

        Dim view = modelli.GetQuery.Where(Function(c) c.Proposta = False And idTipologieDocumento.Contains(c.IdTipologiaDocumento)).OrderBy(Function(c) c.Descrizione).ToList

        Me.ModelliAdottatiComboBox.DataValueField = "Id"
        Me.ModelliAdottatiComboBox.DataTextField = "Descrizione"
        Me.ModelliAdottatiComboBox.DataSource = view
        Me.ModelliAdottatiComboBox.DataBind()
        Me.ModelliAdottatiComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.ModelliAdottatiComboBox.SelectedIndex = 0

    End Sub

    Private Sub CaricaTipologieSeduta()
        Dim tipologieSedute As New ParsecAtt.TipologiaSedutaRepository
        Me.TipologieOrganoComboBox.DataValueField = "Id"
        Me.TipologieOrganoComboBox.DataTextField = "Descrizione"
        Me.TipologieOrganoComboBox.DataSource = tipologieSedute.GetKeyValue(Nothing)
        Me.TipologieOrganoComboBox.DataBind()
        Me.TipologieOrganoComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieOrganoComboBox.SelectedIndex = 0
        tipologieSedute.Dispose()
    End Sub

    Private Sub CaricaSezioneTrasparente()
        Dim sezioni As New ParsecAdmin.SezioneTrasparenzaRepository
        Me.SezioneTrasparenzaComboBox.DataValueField = "Id"
        Me.SezioneTrasparenzaComboBox.DataTextField = "Descrizione"
        Me.SezioneTrasparenzaComboBox.DataSource = sezioni.GetKeyValue().Where(Function(c) c.Id = 55 Or c.Id = 56)
        Me.SezioneTrasparenzaComboBox.DataBind()
        Me.SezioneTrasparenzaComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.SezioneTrasparenzaComboBox.SelectedIndex = 0
        sezioni.Dispose()
    End Sub

    Private Sub CaricaIter()
        Dim modelli As New ParsecWKF.ModelliRepository
        Me.IterComboBox.DataValueField = "Id"
        Me.IterComboBox.DataTextField = "Descrizione"
        Dim view = (From modello In modelli.GetQuery Select New ParsecAtt.KeyValue With {.Id = modello.Id, .Descrizione = modello.NomeFile}).ToList
        Me.IterComboBox.DataSource = view
        Me.IterComboBox.DataBind()
        Me.IterComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.IterComboBox.SelectedIndex = 0
        modelli.Dispose()
    End Sub

    Private Sub AggiornaGriglia()
        Me.Modelli = Nothing
        Me.ModelliGridView.Rebind()
    End Sub

    Private Sub AggiornaGrigliaFirme()
        Me.FirmeGridView.DataSource = Me.Firme
        Me.FirmeGridView.DataBind()
    End Sub

    Private Sub Search()
        Dim modelli As New ParsecAtt.ModelliRepository
        Dim filtro As ParsecAtt.FiltroModello = Me.GetFiltro
        Me.Modelli = modelli.GetView(filtro)

        Me.FiltroModello = filtro
        modelli.Dispose()
        Me.ModelliGridView.Rebind()
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()

        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim modelli As New ParsecAtt.ModelliRepository

        Dim modello As ParsecAtt.Modello = modelli.GetById(id)

        Me.ImpostaAbilitazione(modello.TipologiaDocumento)

        Me.DescrizioneTextBox.Text = modello.Descrizione
        Me.DataInizioValiditaTextBox.Text = String.Format("{0:dd/MM/yyyy}", modello.DataInizio)
        Me.PrefissoTextBox.Text = modello.FileNameNuovoDoc

        If modello.Disabilitato.HasValue Then
            Me.DisabilitatoCheckBox.Checked = modello.Disabilitato
        End If

        Me.PropostaCheckBox.Checked = modello.Proposta
        Me.ObbligoAllegatiCheckBox.Checked = modello.ObbligoAllegato
        Me.PubblicazioneAlboCheckBox.Checked = modello.Pubblicazione

        Me.AccertamentoCheckBox.Checked = modello.Accertamento
        Me.ImpegnoSpesaCheckBox.Checked = modello.ImpegnoSpesa
        Me.LiquidazioneCheckBox.Checked = modello.Liquidazione
        Me.PubblicazioneLiquidazioneCheckBox.Checked = modello.PubblicazioneLiq

        If modello.IdSezioneTrasparenza.HasValue Then
            Me.SezioneTrasparenzaComboBox.FindItemByValue(modello.IdSezioneTrasparenza).Selected = True
        End If


        If modello.IdTipologiaDocumento.HasValue Then
            If modello.IdTipologiaDocumento <> 0 AndAlso modello.IdTipologiaDocumento <> -1 Then
                Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
                Dim tipologiaDocumento As ParsecAtt.TipologiaDocumento = tipologie.GetQuery.Where(Function(c) c.Id = modello.IdTipologiaDocumento).FirstOrDefault
                If Not tipologiaDocumento Is Nothing Then
                    If tipologiaDocumento.Modellizzabile Then
                        Me.TipologieDocumentoComboBox.FindItemByValue(modello.IdTipologiaDocumento).Selected = True
                    End If
                End If
                tipologie.Dispose()
            End If
        End If

        If modello.IdTipologiaRegistro.HasValue Then
            If modello.IdTipologiaRegistro <> 0 AndAlso modello.IdTipologiaRegistro <> -1 Then
                Me.TipologieRegistroComboBox.FindItemByValue(modello.IdTipologiaRegistro).Selected = True
            End If
        End If

        If modello.IdModelloAdottato.HasValue Then
            If modello.IdModelloAdottato <> 0 AndAlso modello.IdModelloAdottato <> -1 Then
                Me.ModelliAdottatiComboBox.FindItemByValue(modello.IdModelloAdottato).Selected = True
            End If
        End If

        If modello.IdTipologiaSeduta.HasValue Then
            If modello.IdTipologiaSeduta <> 0 AndAlso modello.IdTipologiaSeduta <> -1 Then
                Me.TipologieOrganoComboBox.FindItemByValue(modello.IdTipologiaSeduta).Selected = True
            End If
        End If

        If modello.IdIter.HasValue Then
            If modello.IdIter <> 0 AndAlso modello.IdIter <> -1 Then
                Dim it = Me.IterComboBox.FindItemByValue(modello.IdIter)
                If Not it Is Nothing Then
                    it.Selected = True
                End If
            End If
        End If




        Me.MetaModelliComboBox.Enabled = False
        Me.TipologieDocumentoComboBox.Enabled = False

        Dim delibera As Boolean = (modello.IdTipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDelibera) OrElse (modello.IdTipologiaDocumento = ParsecAtt.TipoDocumento.Delibera)
        Me.TipologieOrganoComboBox.Enabled = delibera

        If modello.Proposta.HasValue Then
            Me.ModelliAdottatiComboBox.Enabled = modello.Proposta
            Me.IterComboBox.Enabled = modello.Proposta
        End If

        Me.Modello = modello

        modelli.Dispose()


        Dim modelliFirme As New ParsecAtt.ModelliFirmeRepository
        Me.Firme = modelliFirme.GetView(New ParsecAtt.FiltroModelloFirma With {.IdModello = modello.Id})
        modelliFirme.Dispose()

    End Sub

    Private Sub Delete()
        'Dim firme As New ParsecAtt.FirmeRepository
        'Try
        '    firme.Delete(Me.Firma)
        'Catch ex As Exception
        '    Throw New ApplicationException(ex.Message)
        'Finally
        '    firme.Dispose()
        'End Try
    End Sub

    Private Sub Save()
        Dim nuovo As Boolean = Me.Modello Is Nothing

        Dim modelli As New ParsecAtt.ModelliRepository
        Dim modello As ParsecAtt.Modello = modelli.CreateFromInstance(Me.Modello)

        Dim dataNulla As Nullable(Of Date) = Nothing
        Dim numeroNullo As Nullable(Of Integer) = Nothing

        'Aggiorno il modello
        modello.IdTipologiaDocumento = CInt(Me.TipologieDocumentoComboBox.SelectedValue)

        If Me.TipologieRegistroComboBox.SelectedIndex <> 0 Then
            modello.IdTipologiaRegistro = CInt(Me.TipologieRegistroComboBox.SelectedValue)
        End If

        If Me.TipologieOrganoComboBox.SelectedIndex <> 0 Then
            modello.IdTipologiaSeduta = CInt(Me.TipologieOrganoComboBox.SelectedValue)
        End If

        If Me.ModelliAdottatiComboBox.SelectedIndex <> 0 Then
            modello.IdModelloAdottato = CInt(Me.ModelliAdottatiComboBox.SelectedValue)
        End If

        If Me.IterComboBox.SelectedIndex <> 0 Then
            modello.IdIter = CInt(Me.IterComboBox.SelectedValue)
        Else
            'IN QUESTO CASO L'ITER NON VERRA' AVVIATO
            modello.IdIter = numeroNullo
        End If


        modello.Descrizione = Me.DescrizioneTextBox.Text.Trim
        modello.DataInizio = Now
        modello.DataFine = dataNulla

        Dim template As String = Me.MetaModelliComboBox.SelectedItem.Text

        If nuovo Then
            modello.FileName = template & "{0}.odt"
            modello.Path = String.Empty
        End If

        If Me.SezioneTrasparenzaComboBox.SelectedIndex <> 0 Then
            modello.IdSezioneTrasparenza = CInt(Me.SezioneTrasparenzaComboBox.SelectedValue)
        End If


        modello.DirName = "\TEMPLATES"
        modello.FileNameNuovoDoc = Me.PrefissoTextBox.Text.Trim

        modello.Disabilitato = Me.DisabilitatoCheckBox.Checked
        modello.Proposta = Me.PropostaCheckBox.Checked
        modello.ImpegnoSpesa = Me.ImpegnoSpesaCheckBox.Checked
        modello.Pubblicazione = Me.PubblicazioneAlboCheckBox.Checked
        modello.Liquidazione = Me.LiquidazioneCheckBox.Checked
        modello.Accertamento = Me.AccertamentoCheckBox.Checked
        modello.ObbligoAllegato = Me.ObbligoAllegatiCheckBox.Checked
        modello.PubblicazioneLiq = Me.PubblicazioneLiquidazioneCheckBox.Checked

        Try

            modello.Firme = Me.Firme
            modelli.Modello = Me.Modello
            modelli.Save(modello)

            Me.Modello = modelli.Modello
            Me.Firme = modelli.Modello.Firme

            If nuovo Then
                'Copio il nuovo template odt nella cartella dedicata
                Dim remotePath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")

                Dim input As String = remotePath & "metaModello.odt"
                Dim output As String = remotePath & Me.Modello.FileName
                Me.CopiaDocumento(input, output)

            End If

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            modelli.Dispose()
        End Try

    End Sub

    Private Sub CopiaDocumento(ByVal input As String, ByVal output As String)
        Try
            If Not IO.File.Exists(output) Then
                IO.File.Copy(input, output)
                If (IO.File.GetAttributes(output) And IO.FileAttributes.ReadOnly) = IO.FileAttributes.ReadOnly Then
                    IO.File.SetAttributes(output, IO.FileAttributes.Normal)
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub AbilitaCheckBox(ByVal abilita As Boolean)
        If abilita Then
            Me.DisabilitatoPanel.Attributes.Remove("disabled")
            Me.PropostaPanel.Attributes.Remove("disabled")
            Me.ObbligoAllegatiPanel.Attributes.Remove("disabled")
            Me.PubblicazioneAlboPanel.Attributes.Remove("disabled")
            Me.AccertamentoPanel.Attributes.Remove("disabled")
            Me.ImpegnoSpesaPanel.Attributes.Remove("disabled")
            Me.LiquidazionePanel.Attributes.Remove("disabled")
            Me.PubblicazioneLiquidazionePanel.Attributes.Remove("disabled")
        Else
            Me.DisabilitatoPanel.Attributes.Add("disabled", "disabled")
            Me.PropostaPanel.Attributes.Add("disabled", "disabled")
            Me.ObbligoAllegatiPanel.Attributes.Add("disabled", "disabled")
            Me.PubblicazioneAlboPanel.Attributes.Add("disabled", "disabled")
            Me.AccertamentoPanel.Attributes.Add("disabled", "disabled")
            Me.ImpegnoSpesaPanel.Attributes.Add("disabled", "disabled")
            Me.LiquidazionePanel.Attributes.Add("disabled", "disabled")
            Me.PubblicazioneLiquidazionePanel.Attributes.Add("disabled", "disabled")
        End If

    End Sub

    Private Sub ResettaCheckBox()

        Me.DisabilitatoCheckBox.Checked = False
        Me.PropostaCheckBox.Checked = False
        Me.ObbligoAllegatiCheckBox.Checked = False
        Me.PubblicazioneAlboCheckBox.Checked = False

        Me.AccertamentoCheckBox.Checked = False
        Me.ImpegnoSpesaCheckBox.Checked = False
        Me.LiquidazioneCheckBox.Checked = False
        Me.PubblicazioneLiquidazioneCheckBox.Checked = False

    End Sub

    Private Sub ResettaVista()

        Me.DescrizioneTextBox.Text = String.Empty
        Me.DataInizioValiditaTextBox.Text = String.Format("{0:dd/MM/yyyy}", Now)
        Me.PrefissoTextBox.Text = String.Empty

        Me.TipologieDocumentoComboBox.SelectedIndex = 0
        Me.TipologieRegistroComboBox.SelectedIndex = 0
        Me.MetaModelliComboBox.SelectedIndex = 0
        Me.ModelliAdottatiComboBox.SelectedIndex = 0
        Me.TipologieOrganoComboBox.SelectedIndex = 0
        Me.IterComboBox.SelectedIndex = 0

        Me.SezioneTrasparenzaComboBox.SelectedIndex = 0

        'Disabilito tutte le checkbox
        AbilitaCheckBox(False)

        Me.ResettaCheckBox()


        '*********************************************************************
        'L'abilitazione viene impostata in base alla tipologia di documento.
        '*********************************************************************
        Me.ModelliAdottatiComboBox.Enabled = False
        Me.TipologieOrganoComboBox.Enabled = False
        Me.IterComboBox.Enabled = False

        Me.Firme = New List(Of ParsecAtt.ModelloFirma)

        Me.MetaModelliComboBox.Enabled = True
        Me.TipologieDocumentoComboBox.Enabled = True
        'Me.FirmeGridView.DataSource = Me.Firme
        'Me.FirmeGridView.DataBind()

        Me.Modello = Nothing

    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroModello
        Dim filtro As New ParsecAtt.FiltroModello
        filtro.Descrizione = Me.DescrizioneTextBox.Text
        If Me.TipologieDocumentoComboBox.SelectedIndex <> 0 Then
            filtro.TipologiaDocumento = CInt(Me.TipologieDocumentoComboBox.SelectedValue)
        End If
        Return filtro
    End Function




    Private Sub EliminaFirma(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idFirma As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdFirma")
        Dim firma As ParsecAtt.ModelloFirma = Me.Firme.Where(Function(c) c.IdFirma = idFirma).FirstOrDefault
        If Not firma Is Nothing Then
            Me.Firme.Remove(firma)
        End If
    End Sub

    Private Sub ImpostaAbilitazione(ByVal tipo As ParsecAtt.TipoDocumento)

        'Abilito e deseleziono tutte le checkBox
        AbilitaCheckBox(tipo <> -1)



        Dim proposta As Boolean = (tipo = ParsecAtt.TipoDocumento.PropostaDecreto) OrElse (tipo = ParsecAtt.TipoDocumento.PropostaDelibera) _
                                  OrElse (tipo = ParsecAtt.TipoDocumento.PropostaDetermina) OrElse (tipo = ParsecAtt.TipoDocumento.PropostaOrdinanza)




        Dim delibera As Boolean = (tipo = ParsecAtt.TipoDocumento.PropostaDelibera) OrElse (tipo = ParsecAtt.TipoDocumento.Delibera)


        'Disabilito le check in base alla tipologia di documento
        'Disabilito se è una proposta
        If proposta Then

            Me.PubblicazioneAlboPanel.Attributes.Add("disabled", "disabled")
            Me.PubblicazioneLiquidazionePanel.Attributes.Add("disabled", "disabled")

            If tipo = ParsecAtt.TipoDocumento.PropostaDelibera Then
                Me.AccertamentoPanel.Attributes.Add("disabled", "disabled")
                Me.LiquidazionePanel.Attributes.Add("disabled", "disabled")
            End If

            If tipo = ParsecAtt.TipoDocumento.PropostaOrdinanza OrElse tipo = ParsecAtt.TipoDocumento.PropostaDecreto Then
                Me.ImpegnoSpesaPanel.Attributes.Add("disabled", "disabled")
                Me.AccertamentoPanel.Attributes.Add("disabled", "disabled")
                Me.LiquidazionePanel.Attributes.Add("disabled", "disabled")
            End If

        Else

            Me.PropostaPanel.Attributes.Add("disabled", "disabled")

            'Tutte tranne la determina
            If tipo <> ParsecAtt.TipoDocumento.Determina Then


                Me.PubblicazioneLiquidazionePanel.Attributes.Add("disabled", "disabled")
                Me.AccertamentoPanel.Attributes.Add("disabled", "disabled")
                Me.LiquidazionePanel.Attributes.Add("disabled", "disabled")

                If tipo <> ParsecAtt.TipoDocumento.Delibera Then
                    Me.ImpegnoSpesaPanel.Attributes.Add("disabled", "disabled")
                End If
            End If

        End If


        Me.PropostaCheckBox.Checked = proposta
        Me.ModelliAdottatiComboBox.Enabled = proposta


        Me.IterComboBox.Enabled = proposta
        If Not proposta Then
            Me.ModelliAdottatiComboBox.SelectedIndex = 0
            Me.IterComboBox.SelectedIndex = 0
        End If

        Me.TipologieOrganoComboBox.Enabled = delibera

        If Not delibera Then
            Me.TipologieOrganoComboBox.SelectedIndex = 0
        End If

        '***************************************************************************************
        'Sono sempre modellizzabili   ?????????????
        '***************************************************************************************
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Dim idTipologia As Integer = CInt(tipo)
        Dim tipologiaDocumento As ParsecAtt.TipologiaDocumento = tipologie.GetQuery.Where(Function(c) c.Id = idTipologia).FirstOrDefault
        If Not tipologiaDocumento Is Nothing Then

            Me.PrefissoTextBox.ReadOnly = Not tipologiaDocumento.Modellizzabile
            Me.TipologieRegistroComboBox.Enabled = tipologiaDocumento.Modellizzabile
            Me.AggiungiFirmaImageButton.Enabled = tipologiaDocumento.Modellizzabile
            Me.FirmeGridView.MasterTableView.GetColumn("Delete").Visible = tipologiaDocumento.Modellizzabile

        End If

        tipologie.Dispose()
        '***************************************************************************************

    End Sub

#End Region


#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub TipologieDocumentoComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieDocumentoComboBox.SelectedIndexChanged

        Dim value = CInt(TipologieDocumentoComboBox.SelectedValue)
        Me.ResettaCheckBox()
        Me.ImpostaAbilitazione(value)

    End Sub

    Protected Sub AggiungiFirmaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiFirmaImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/RicercaFirmaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Mode", "search")
        queryString.Add("obj", Me.AggiornaFirmaImageButton.ClientID)
        queryString.Add("filtro", Me.FiltroTextBox.Text)

        'Dim parametriPagina As New Hashtable
        'parametriPagina.Add("Filtro", Me.FiltroTextBox.Text)
        'ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 910, 600, queryString, False)

    End Sub

    Protected Sub AggiornaFirmaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaFirmaImageButton.Click
        Me.FiltroTextBox.Text = String.Empty
        If Not Session("FirmeSelezionate") Is Nothing Then
            Dim firmeSelezionate As List(Of ParsecAtt.ModelloFirma) = Session("FirmeSelezionate")
            For Each firmaModello As ParsecAtt.ModelloFirma In firmeSelezionate
                Dim IdFirma As Integer = firmaModello.IdFirma
                Dim exist As Boolean = Not Me.Firme.Where(Function(c) c.IdFirma = IdFirma).FirstOrDefault Is Nothing
                If Not exist Then
                    Me.Firme.Add(firmaModello)
                End If
            Next
            Session("FirmeSelezionate") = Nothing
        End If
    End Sub

#End Region


#Region "GESTIONE RICERCA AVANZATA"

    Private Sub ResettaFiltro()
        Me.TipologieDocumentoFiltroComboBox.SelectedIndex = 0
        Me.DescrizioneFiltroTextBox.Text = String.Empty
        Me.StatoFiltroComboBox.SelectedIndex = 0
    End Sub

    Private Function GetFiltroAvanzato() As ParsecAtt.FiltroModello
        Dim filtro As New ParsecAtt.FiltroModello
        filtro.Descrizione = Me.DescrizioneFiltroTextBox.Text
        If Me.TipologieDocumentoFiltroComboBox.SelectedIndex <> 0 Then
            filtro.TipologiaDocumento = CInt(Me.TipologieDocumentoFiltroComboBox.SelectedValue)
        End If
        If Me.StatoFiltroComboBox.SelectedIndex <> 0 Then
            filtro.Disabilitato = Me.StatoFiltroComboBox.SelectedValue
        End If
        Return filtro
    End Function


    Protected Sub RipristinaFiltroInizialeImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles RipristinaFiltroInizialeImageButton.Click
        Me.FiltroModello = Nothing
        Me.Modelli = Nothing
        Me.ModelliGridView.Rebind()
    End Sub

    Protected Sub SalvaButton_Click(sender As Object, e As System.EventArgs) Handles SalvaButton.Click
        Me.FiltroModello = GetFiltroAvanzato()
        Me.Modelli = Nothing
        Me.ModelliGridView.Rebind()
        Me.ResettaFiltro()
    End Sub

    Protected Sub ChiudiRicercaImageButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiRicercaImageButton.Click
        Me.ResettaFiltro()
    End Sub


#End Region

End Class