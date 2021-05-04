Imports ParsecAdmin
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class ProcedimentiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Procedimento() As ParsecAdmin.Procedimento
        Get
            Return CType(Session("ProcedimentiPage_Procedimento"), ParsecAdmin.Procedimento)
        End Get
        Set(ByVal value As ParsecAdmin.Procedimento)
            Session("ProcedimentiPage_Procedimento") = value
        End Set
    End Property

    Public Property Procedimenti() As List(Of ParsecAdmin.Procedimento)
        Get
            Return CType(Session("ProcedimentiPage_Procedimenti"), List(Of ParsecAdmin.Procedimento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Procedimento))
            Session("ProcedimentiPage_Procedimenti") = value
        End Set
    End Property

    Public Property SettoriProcedimento() As List(Of ParsecAdmin.SettoreProcedimento)
        Get
            Return CType(Session("ProcedimentiPage_SettoriProcedimento"), List(Of ParsecAdmin.SettoreProcedimento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.SettoreProcedimento))
            Session("ProcedimentiPage_SettoriProcedimento") = value
        End Set
    End Property


    Public Property ModuliProcedimento() As List(Of ParsecAdmin.ModuloProcedimento)
        Get
            Return CType(Session("ProcedimentiPage_ModuliProcedimento"), List(Of ParsecAdmin.ModuloProcedimento))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.ModuloProcedimento))
            Session("ProcedimentiPage_ModuliProcedimento") = value
        End Set
    End Property

    Public Property GiorniSospensione As Integer
        Get
            Return CType(Session("ProcedimentiPage_GiorniSospensione"), Integer)
        End Get
        Set(value As Integer)
            Session("ProcedimentiPage_GiorniSospensione") = value
        End Set
    End Property



#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        'Try
        '    Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        '    Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
        '    Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
        '    Dim sp As New ParsecWebServices.StatoPraticaWS("12", utenteCorrente.Username, 6)
        '    sp.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)
        '    sp.Insert()
        'Catch ex As Exception

        'End Try

     

        'Dim ip = ParsecUtility.Utility.GetUserIP

        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Procedimenti"

        If Not Me.Page.IsPostBack Then
            Me.Procedimenti = Nothing
            Me.ModuliProcedimento = New List(Of ParsecAdmin.ModuloProcedimento)
            Me.ResettaVista()
            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Nome"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.ProcedimentiGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

            Dim moduli As New ParsecAdmin.ModuleRepository
            Dim modulo = moduli.Where(Function(c) c.Id = ParsecAdmin.TipoModulo.IOL).FirstOrDefault
            moduli.Dispose()

            If modulo Is Nothing Then
                Me.ResponsabileInerziaLabel.Text = "Resp. Inerzia"
                Me.IterLabel.Text = "Iter"
            Else
                If Not modulo.Abilitato Then
                    Me.ResponsabileInerziaLabel.Text = "Resp. Inerzia"
                    Me.IterLabel.Text = "Iter"
                End If
            End If

            Me.CaricaIter()
            Me.CaricaIterIntegrazione()
            Me.GiorniSospensione = Me.GetGiorniSospensione

        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.GeneralePanel.Style.Add("width", widthStyle)
        Me.ModuliPanel.Style.Add("width", widthStyle)
        Me.VisibilitaPanel.Style.Add("width", widthStyle)
        Me.GrigliaModuliPanel.Style.Add("width", widthStyle)
        Me.ModuliGridView.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.ProcedimentiGridView.Style.Add("width", widthStyle)
        Me.SettoriGridView.Style.Add("width", widthStyle)



    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il procedimento selezionato?", False, Not Me.Procedimento Is Nothing)
        Me.TitoloLabel.Text = "Elenco Procedimenti&nbsp;&nbsp;&nbsp;" & If(Me.Procedimenti.Count > 0, "( " & Me.Procedimenti.Count.ToString & " )", "")
        Me.AggiornaGrigliaSettori()
        Me.AggiornaGrigliaModuli()

        Me.AttiTabStrip.Tabs(1).Text = "Documentazione" & If(Me.ModuliProcedimento.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.ModuliProcedimento.Count.ToString & ")</span>", "")
        Me.ModuliLabel.Text = "Documentazione" & If(Me.ModuliProcedimento.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.ModuliProcedimento.Count.ToString & ")</span>", "")
        Me.AttiTabStrip.Tabs(2).Text = "Visibilità" & If(Me.SettoriProcedimento.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.SettoriProcedimento.Count.ToString & ")</span>", "")
    End Sub

#End Region

#Region "GESTIONE SETTORE COMPETENZA"

    Protected Sub TrovaSettoreCompetenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaSettoreCompetenzaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaSettoreCompetenzaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 910, 670, queryString, False)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", ParsecAdmin.TipoModulo.ATT)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100")
        parametriPagina.Add("ultimoLivelloStruttura", "100")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    End Sub

    Protected Sub EliminaSettoreCompetenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaSettoreCompetenzaImageButton.Click
        Me.SettoreCompetenzaTextBox.Text = String.Empty
        Me.IdSettoreCompetenzaTextBox.Text = String.Empty
        Me.CodiceSettoreCompetenzaTextBox.Text = String.Empty
    End Sub


    Private Sub AggiungiSettoreVisibilita(ByVal idStruttura As Integer)
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim strutturaSelezionata = strutture.Where(Function(c) c.Id = idStruttura).FirstOrDefault
        If Not strutturaSelezionata Is Nothing Then
            Dim settore = strutture.Where(Function(c) c.Id = strutturaSelezionata.IdPadre And c.LogStato Is Nothing And c.IdGerarchia = 100).FirstOrDefault
            If Not settore Is Nothing Then
                Dim esiste = Not Me.SettoriProcedimento.Where(Function(c) c.CodiceStruttura = settore.Codice).FirstOrDefault Is Nothing
                If Not esiste Then
                    Dim nuovoSettore As New ParsecAdmin.SettoreProcedimento
                    nuovoSettore.CodiceStruttura = settore.Codice
                    nuovoSettore.struttura = settore
                    Me.SettoriProcedimento.Add(nuovoSettore)
                End If
            End If
        End If
        strutture.Dispose()
    End Sub



    Protected Sub AggiornaSettoreCompetenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSettoreCompetenzaImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim struttura = struttureSelezionate.First

            'Aggiorno il settore
            Me.SettoreCompetenzaTextBox.Text = struttura.Descrizione
            Me.IdSettoreCompetenzaTextBox.Text = struttura.Id.ToString
            Me.CodiceSettoreCompetenzaTextBox.Text = struttura.Codice

            Dim idStruttura As Integer = struttura.Id

            Dim strutture As New ParsecAdmin.StructureRepository
            Dim strutturaSelezionata = strutture.GetQuery.Where(Function(c) c.Id = idStruttura).FirstOrDefault
            strutture.Dispose()

            If Not strutturaSelezionata Is Nothing Then
                Dim esiste = Not Me.SettoriProcedimento.Where(Function(c) c.CodiceStruttura = strutturaSelezionata.Codice).FirstOrDefault Is Nothing
                If Not esiste Then
                    Dim nuovoSettore As New ParsecAdmin.SettoreProcedimento
                    nuovoSettore.CodiceStruttura = struttura.Codice
                    nuovoSettore.struttura = strutturaSelezionata
                    Me.SettoriProcedimento.Add(nuovoSettore)
                End If
            End If

            Session("SelectedStructures") = Nothing

        End If
    End Sub

#End Region

#Region "GESTIONE RESPONSABILE INERZIA"

    Protected Sub TrovaResponsabileInerziaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaResponsabileInerziaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaResponsabileInerziaImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("idModulo", ParsecAdmin.TipoModulo.ATT)
        'parametriPagina.Add("idUtente", utenteCollegato.Id)
        parametriPagina.Add("livelliSelezionabili", "400")
        parametriPagina.Add("ultimoLivelloStruttura", "400")
        'parametriPagina.Add("ApplicaAbilitazioni", True)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Protected Sub AggiornaResponsabileInerziaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaResponsabileInerziaImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim struttura = struttureSelezionate.First
            Me.ResponsabileInerziaTextBox.Text = struttura.Descrizione
            Me.IdResponsabileInerziaTextBox.Text = struttura.Id.ToString
            Me.CodiceResponsabileInerziaTextBox.Text = struttura.Codice.ToString

            Me.AggiungiSettoreVisibilita(struttura.Id)

           Session("SelectedStructures") = Nothing
        End If
    End Sub

    Protected Sub EliminaResponsabileInerziaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaResponsabileInerziaImageButton.Click
        Me.ResponsabileInerziaTextBox.Text = String.Empty
        Me.IdResponsabileInerziaTextBox.Text = String.Empty
        Me.CodiceResponsabileInerziaTextBox.Text = String.Empty
    End Sub

#End Region

#Region "GESTIONE RESPONSABILE"

    Protected Sub TrovaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaResponsabileImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaResponsabileImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("idModulo", ParsecAdmin.TipoModulo.ATT)
        'parametriPagina.Add("idUtente", utenteCollegato.Id)
        parametriPagina.Add("livelliSelezionabili", "400")
        parametriPagina.Add("ultimoLivelloStruttura", "400")
        'parametriPagina.Add("ApplicaAbilitazioni", True)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Protected Sub AggiornaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaResponsabileImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim struttura = struttureSelezionate.First
            Me.ResponsabileTextBox.Text = struttura.Descrizione
            Me.IdResponsabileTextBox.Text = struttura.Id.ToString
            Me.CodiceResponsabileTextBox.Text = struttura.Codice.ToString

            Me.AggiungiSettoreVisibilita(struttura.Id)

            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Protected Sub EliminaResponsabileImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaResponsabileImageButton.Click
        Me.ResponsabileTextBox.Text = String.Empty
        Me.IdResponsabileTextBox.Text = String.Empty
        Me.CodiceResponsabileTextBox.Text = String.Empty
    End Sub

#End Region

#Region "GESTIONE DOCUMENTAZIONE"

    Private Sub DeleteDocument(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim modulo As ParsecAdmin.ModuloProcedimento = Me.ModuliProcedimento.Where(Function(c) c.Id = id).FirstOrDefault
        If Not modulo Is Nothing Then
            Me.ModuliProcedimento.Remove(modulo)
            'Se è un allegato temporaneo.
            If modulo.Id < 0 Then
                Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & modulo.NomeFileTemp
                If IO.File.Exists(pathDownload) Then
                    IO.File.Delete(pathDownload)
                End If
            End If
        End If
    End Sub

    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim id As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim modulo As ParsecAdmin.ModuloProcedimento = Me.ModuliProcedimento.Where(Function(c) c.Id = id).FirstOrDefault

        If Not modulo Is Nothing Then
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If modulo.Id < 0 Then
                pathDownload = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & modulo.NomeFileTemp
            Else
                Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathModuli")
                pathDownload = percorsoRoot & String.Format("MOD_{0}_{1}", modulo.Codice.ToString.PadLeft(9, "0"), modulo.Nomefile)
            End If
            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("Il modulo selezionato non esiste!", False)
            End If
        End If
    End Sub

    Protected Sub ModuliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ModuliGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.DeleteDocument(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.DownloadFile(e.Item)
        End If
    End Sub

    Private Sub ModuliGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ModuliGridView.ItemCreated

        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Private Sub AggiornaGrigliaModuli()
        Me.ModuliGridView.DataSource = Me.ModuliProcedimento
        Me.ModuliGridView.DataBind()
    End Sub

    Protected Sub AggiungiModuloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiModuloImageButton.Click

        Dim modulo As New ParsecAdmin.ModuloProcedimento

        If Me.ModuliProcedimento.Count > 0 Then
            Dim allId = Me.ModuliProcedimento.Min(Function(c) c.Id) - 1
            If allId > 0 Then
                modulo.Id = -1
            Else
                modulo.Id = allId
            End If
        Else
            modulo.Id = -1
        End If

        If Me.AllegatoUpload.UploadedFiles.Count > 0 Then
            Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles(0)
            If file.FileName <> "" Then
                Dim nomefile As String = Guid.NewGuid.ToString
                Dim filenameTemp As String = nomefile & "_" & file.FileName
                Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

                If Not IO.Directory.Exists(pathRootTemp) Then
                    IO.Directory.CreateDirectory(pathRootTemp)
                End If

                Dim pathDownload As String = pathRootTemp & filenameTemp
                Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathModuli")


                Dim filename As String = file.FileName
                file.SaveAs(pathDownload)
                modulo.Nomefile = filename
                modulo.NomeFileTemp = filenameTemp
            End If
        End If

        modulo.Nome = Me.NomeModuloTextBox.Text
        modulo.Descrizione = Me.DescrizioneModuloTextBox.Text
        If Not String.IsNullOrEmpty(Me.NoteModuloTextBox.Text) Then
            modulo.Note = Me.NoteModuloTextBox.Text
        End If

        modulo.Obbligatorio = Me.ObbligatorioCheckBox.Checked
        modulo.ObbligoFirmaDigitale = Me.ObbligatorioFirmaDigitaleCheckBox.Checked

        'Dim impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
        'allegato.Impronta = BitConverter.ToString(impronta).Replace("-", "")
        'If Me.Allegati.Where(Function(c) c.Impronta = allegato.Impronta).FirstOrDefault Is Nothing Then
        '    Me.Allegati.Add(allegato)
        'Else
        '    ParsecUtility.Utility.MessageBox("Il file selezionato è stato già allegato!", False)
        'End If

        Me.ModuliProcedimento.Add(modulo)

        Me.DescrizioneModuloTextBox.Text = String.Empty
        Me.NoteModuloTextBox.Text = String.Empty
        Me.NomeModuloTextBox.Text = String.Empty
        Me.ObbligatorioCheckBox.Checked = True
        Me.ObbligatorioFirmaDigitaleCheckBox.Checked = False

    End Sub

#End Region

#Region "GESTIONE CLASSIFICAZIONE"


    Protected Sub TrovaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "2,3")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    End Sub

    Protected Sub AggiornaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim codici = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione(idClassificazione, 1)
            Dim classificazioneCompleta As String = codici & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneTextBox.Text = idClassificazione.ToString
            Me.CodiceClassificazioneTextBox.Text = codici
            Session("ClassificazioniSelezionate") = Nothing
        End If
    End Sub

    Protected Sub EliminaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaClassificazioneImageButton.Click
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.CodiceClassificazioneTextBox.Text = String.Empty
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
                    Dim erroreUpload As String = String.Empty
                    If Not String.IsNullOrEmpty(Me.infoOperazioneHidden.Value) Then
                        erroreUpload = Me.infoOperazioneHidden.Value
                        Me.infoOperazioneHidden.Value = "<span style='width:10px'></span><span><img  src='/sep/Images/Successo16.png'></img><span style='width:10px'></span>Procedimento salvato con successo!</span><br>" & "<span style='width:10px'></span><span style='color:#FF0000'>" & "<img  src='/sep/Images/alert16.png'></img><span style='width:10px'></span>" & erroreUpload & "</span>"


                    Else
                        Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                    End If

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
                    If Not Me.Procedimento Is Nothing Then
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
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un procedimento!", False)
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

    Protected Sub ProcedimentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ProcedimentiGridView.NeedDataSource
        If Me.Procedimenti Is Nothing Then
            Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
            Me.Procedimenti = procedimenti.GetView(Nothing)
            procedimenti.Dispose()
        End If
        Me.ProcedimentiGridView.DataSource = Me.Procedimenti
    End Sub

    Protected Sub ProcedimentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ProcedimentiGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub


    Protected Sub ProcedimentiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ProcedimentiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Function GetGiorniSospensione() As Integer
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("GiorniAttesaIntegrazioneIstanzaOnLine")
        parametri.Dispose()
        Dim giorniSospensione As Integer = 30
        If Not parametro Is Nothing Then
            Try
                giorniSospensione = CInt(parametro.Valore)
            Catch ex As Exception
            End Try
        End If
        Return giorniSospensione
    End Function

    Private Sub CaricaIter()
        Dim modelli As New ParsecWKF.ModelliRepository

        Dim res = From m In modelli.GetQuery
                  Where m.IdModuloDestinazione = ParsecAdmin.TipoModulo.IOL
                  Select New With {
                      .Id = m.Id,
                      .Descrizione = m.Descrizione & " - " & m.NomeFile
                      }

        Me.IterComboBox.DataValueField = "Id"
        Me.IterComboBox.DataTextField = "Descrizione"
        Me.IterComboBox.DataSource = res
        Me.IterComboBox.DataBind()
        Me.IterComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.IterComboBox.SelectedIndex = 0

       

        modelli.Dispose()
    End Sub


    Private Sub CaricaIterIntegrazione()
        Dim modelli As New ParsecWKF.ModelliRepository

        Dim res = From m In modelli.GetQuery
                   Where m.IdModuloDestinazione = ParsecAdmin.TipoModulo.PRO
                 Select New With {
                      .Id = m.Id,
                      .Descrizione = m.Descrizione & " - " & m.NomeFile
                      }

       
        Me.IterIntegrazioneComboBox.DataValueField = "Id"
        Me.IterIntegrazioneComboBox.DataTextField = "Descrizione"
        Me.IterIntegrazioneComboBox.DataSource = res
        Me.IterIntegrazioneComboBox.DataBind()
        Me.IterIntegrazioneComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.IterIntegrazioneComboBox.SelectedIndex = 0

        modelli.Dispose()
    End Sub




    Private Sub AggiornaGriglia()
        Me.Procedimenti = Nothing
        Me.ProcedimentiGridView.Rebind()
    End Sub


    Private Sub AggiornaGrigliaSettori()
        Me.SettoriGridView.DataSource = Me.SettoriProcedimento
        Me.SettoriGridView.DataBind()

    End Sub

    Private Sub Search()
        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
        Dim filtro As ParsecAdmin.FiltroProcedimento = Me.GetFiltro
        Me.Procedimenti = procedimenti.GetView(filtro)
        Me.ProcedimentiGridView.Rebind()
        procedimenti.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecAdmin.FiltroProcedimento
        Dim filtro As New ParsecAdmin.FiltroProcedimento
        filtro.Nome = Me.NomeTextBox.Text.Trim
        Return filtro
    End Function

    Private Sub Print()
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaElencoProcedimenti")
        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
        parametriStampa.Add("DatiStampa", procedimenti.GetViewStampa(Nothing))
        procedimenti.Dispose()
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()

        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
        Try

            Dim moduli As New ParsecAdmin.ModuleRepository
            Dim modulo = moduli.Where(Function(c) c.Id = ParsecAdmin.TipoModulo.IOL And c.Abilitato).FirstOrDefault
            moduli.Dispose()

            Dim eseguiWS As Boolean = False

            If Not modulo Is Nothing Then
                eseguiWS = Me.Procedimento.Pubblicabile
            End If

            'SE IL MODULO IOL E' ATTIVO E SE IL PROCEDIMENTO E' MARCATO PER LA PUBBLICAZIONE
            If eseguiWS Then
                procedimenti.Delete(Me.Procedimento.Id, AddressOf Me.DeleteCallback)
            Else
                procedimenti.Delete(Me.Procedimento.Id, Nothing)
            End If

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            procedimenti.Dispose()
        End Try

    End Sub


    Private Sub DeleteCallback()

        Using procedimentoWS As New ParsecWebServices.ProcedimentoWS
            Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
            Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
            Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
            procedimentoWS.Utente = utente.Username
            procedimentoWS.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)
            procedimentoWS.Delete(Me.Procedimento.Id)
        End Using

    End Sub

    Private Sub Save()

        If Not Me.AbilitatoCheckBox.Checked Then
            If Me.PubblicabileCheckBox.Checked Then
                Throw New ApplicationException("Per pubblicare un procedimento è necessario abilitarlo!")
                Exit Sub
            End If
        End If

        Dim nuovo As Boolean = Me.Procedimento Is Nothing

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository

        Dim procedimento As ParsecAdmin.Procedimento = procedimenti.CreateFromInstance(Me.Procedimento)
        procedimento.Nome = Me.NomeTextBox.Text
        procedimento.RiferimentoNormativo = Me.NormativaTextBox.Text

        procedimento.Abilitato = Me.AbilitatoCheckBox.Checked
        'procedimento.Abilitato = True

        procedimento.Pubblicabile = Me.PubblicabileCheckBox.Checked

        procedimento.Tempo = Nothing

        If Me.TermineConclusioneTextBox.Value.HasValue Then
            procedimento.Tempo = CInt(Me.TermineConclusioneTextBox.Value)
        End If

        procedimento.GiorniSospensione = Nothing
        If Me.GiorniSospensioneTextBox.Value.HasValue Then
            procedimento.GiorniSospensione = CInt(Me.GiorniSospensioneTextBox.Value)
        End If

        procedimento.IdUtente = utenteCollegato.Id


        If Not String.IsNullOrEmpty(Me.CodiceSettoreCompetenzaTextBox.Text) Then
            procedimento.CodiceStruttura = CInt(Me.CodiceSettoreCompetenzaTextBox.Text)
        Else
            procedimento.CodiceStruttura = Nothing
        End If

        If Not String.IsNullOrEmpty(Me.CodiceResponsabileTextBox.Text) Then
            procedimento.CodiceResponsabile = CInt(Me.CodiceResponsabileTextBox.Text)
        Else
            procedimento.CodiceResponsabile = Nothing
        End If

        If Not String.IsNullOrEmpty(Me.CodiceResponsabileInerziaTextBox.Text) Then
            procedimento.CodiceResponsabileInerzia = CInt(Me.CodiceResponsabileInerziaTextBox.Text)
        Else
            procedimento.CodiceResponsabileInerzia = Nothing
        End If

        procedimento.Imprese = Me.ImpresaCheckBox.Checked
        procedimento.Professionisti = Me.ProfessionistaCheckBox.Checked
        procedimento.Privati = Me.PrivatoCheckBox.Checked
        procedimento.Pubblico = Me.PubblicoCheckBox.Checked


        procedimento.Descrizione = Me.DescrizioneTextBox.Text
        procedimento.Indicazioni = Me.IndicazioniTextBox.Text
        procedimento.Note = Me.NoteTextBox.Text

        procedimento.IdModelloWkf = Nothing
        If Me.IterComboBox.SelectedIndex <> 0 Then
            procedimento.IdModelloWkf = CInt(Me.IterComboBox.SelectedValue)
        End If

        procedimento.IdModelloWkfIntegrazione = Nothing
        If Me.IterIntegrazioneComboBox.SelectedIndex <> 0 Then
            procedimento.IdModelloWkfIntegrazione = CInt(Me.IterIntegrazioneComboBox.SelectedValue)
        End If


        procedimento.ClasseRischio = Me.ClasseTextBox.Text
        procedimento.Codice = Me.CodiceTextBox.Text

        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            procedimento.CodClassificaAppartenenza = CInt(Me.IdClassificazioneTextBox.Text)
        End If

        'associo i settori
        Dim listaSettoriTemp = New List(Of ParsecAdmin.SettoreProcedimento)
        Dim settore As ParsecAdmin.SettoreProcedimento = Nothing
        For Each item In Me.SettoriProcedimento
            settore = New ParsecAdmin.SettoreProcedimento
            settore.CodiceStruttura = item.CodiceStruttura
            settore.struttura = item.struttura
            listaSettoriTemp.Add(settore)
        Next
        procedimento.ListaSettori = listaSettoriTemp

        '*****************************************************************
        'ASSOCIO I MODULI AL PROCEDIMENTO
        '*****************************************************************
        Dim nuovoModulo As ParsecAdmin.ModuloProcedimento = Nothing
        For Each modulo In Me.ModuliProcedimento
            nuovoModulo = New ParsecAdmin.ModuloProcedimento
            nuovoModulo.NomeFileTemp = modulo.NomeFileTemp
            nuovoModulo.Nomefile = modulo.Nomefile
            nuovoModulo.Nome = modulo.Nome
            nuovoModulo.Descrizione = modulo.Descrizione
            nuovoModulo.Note = modulo.Note
            nuovoModulo.Obbligatorio = modulo.Obbligatorio
            nuovoModulo.ObbligoFirmaDigitale = modulo.ObbligoFirmaDigitale
            nuovoModulo.Id = modulo.Id
            nuovoModulo.Codice = modulo.Codice
            nuovoModulo.IdUtente = utenteCollegato.Id
            nuovoModulo.DataOperazione = Now
            procedimento.ModuliProcedimento.Add(nuovoModulo)
        Next
        '*****************************************************************

        Me.DescrizioneModuloTextBox.Text = String.Empty
        Me.NoteModuloTextBox.Text = String.Empty
        Me.NomeModuloTextBox.Text = String.Empty
        Me.ObbligatorioCheckBox.Checked = True
        Me.ObbligatorioFirmaDigitaleCheckBox.Checked = False


        Try

            Dim moduli As New ParsecAdmin.ModuleRepository
            Dim modulo = moduli.Where(Function(c) c.Id = ParsecAdmin.TipoModulo.IOL And c.Abilitato).FirstOrDefault
            moduli.Dispose()

            Dim eseguiWS As Boolean = False

            If Not modulo Is Nothing Then
                If nuovo Then
                    eseguiWS = procedimento.Pubblicabile
                Else
                    'SE NON E' STATO GIA' PUBBLICATO
                    If Not Me.Procedimento.Pubblicabile Then
                        eseguiWS = procedimento.Pubblicabile
                    Else
                        eseguiWS = True
                    End If

                End If
            End If

            'Dim eseguiWS As Boolean = Not modulo Is Nothing


            'SE IL MODULO IOL E' ATTIVO E SE IL PROCEDIMENTO E' MARCATO PER LA PUBBLICAZIONE
            If eseguiWS Then
                procedimenti.Save(procedimento, AddressOf Me.SalveCallback)
            Else
                procedimenti.Save(procedimento, Nothing)
            End If




            Me.Procedimento = procedimenti.GetById(procedimento.Id)
            Me.ModuliProcedimento = Me.Procedimento.ModuliProcedimento

            If eseguiWS Then
                Me.SalvaModuli()
            End If

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            procedimenti.Dispose()
        End Try
    End Sub


    Private Sub SalvaModuli()



        Dim documenti = Me.Procedimento.ModuliProcedimento.Where(Function(c) Not c.Nomefile Is Nothing)
        If documenti.Count > 0 Then
            Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathModuli")
            Using procedimentoWS As New ParsecWebServices.ProcedimentoWS

                Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
                Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
                procedimentoWS.Utente = utente.Username
                procedimentoWS.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)

                Dim path As String = String.Empty

                For Each documento In documenti
                    path = percorsoRoot & String.Format("MOD_{0}_{1}", documento.Codice.ToString.PadLeft(9, "0"), documento.Nomefile)
                    If IO.File.Exists(path) Then
                        Try
                            'Dim f = 0
                            'Dim i = 2 \ f
                            procedimentoWS.UploadFile(documento.Id, path)
                        Catch ex As Exception
                            Me.infoOperazioneHidden.Value = "Riscontrati problemi durante l'upload!"
                        End Try
                    End If
                Next
            End Using
        End If


    End Sub

    Private Sub SalveCallback(ByVal id As Integer)

        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
        Dim procedimento = procedimenti.GetById(id)
        procedimenti.Dispose()

        'Dim f = 0
        'Dim i = 2 \ f

        Using procedimentoWS As New ParsecWebServices.ProcedimentoWS
            Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
            Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
            Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
            procedimentoWS.Utente = utente.Username
            procedimentoWS.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)

            Dim nuovoModulo As ParsecWebServices.ModuloProcedimentoWS = Nothing
            For Each modulo In procedimento.ModuliProcedimento
                nuovoModulo = New ParsecWebServices.ModuloProcedimentoWS With {
                    .Descrizione = modulo.Descrizione,
                    .FirmaDigitale = modulo.ObbligoFirmaDigitale,
                    .IdModulo = modulo.Id,
                    .IdProcedimentoSep = modulo.IdProcedimento,
                    .NomeFile = If(String.IsNullOrEmpty(modulo.Nomefile), "", modulo.Nomefile),
                    .NomeModulo = modulo.Nome,
                    .Note = modulo.Note,
                    .Obbligatorio = modulo.Obbligatorio
                }
                procedimentoWS.Modulistica.Add(nuovoModulo)
            Next

            procedimentoWS.IdProcedimentoSep = procedimento.Id
            procedimentoWS.Nome = procedimento.Nome
            procedimentoWS.Descrizione = procedimento.Descrizione
            procedimentoWS.Indicazioni = procedimento.Indicazioni
            procedimentoWS.RifNormativo = procedimento.RiferimentoNormativo
            procedimentoWS.Note = procedimento.Note


            If procedimento.Tempo.HasValue Then
                procedimentoWS.Termine = procedimento.Tempo
            End If

            procedimentoWS.CodiceSettoreSep = procedimento.CodiceStruttura
            procedimentoWS.CodiceResponsabileSep = procedimento.CodiceResponsabile
            procedimentoWS.CodiceResponsabileInerziaSep = procedimento.CodiceResponsabileInerzia
            procedimentoWS.Imprese = procedimento.Imprese
            procedimentoWS.Professionisti = procedimento.Professionisti
            procedimentoWS.Privati = procedimento.Privati
            procedimentoWS.Pubblico = procedimento.Pubblico


            If Not procedimento.Pubblicabile Then
                procedimentoWS.DataFineValidita = Now
            End If

            Dim modelli As New ParsecWKF.ModelliRepository
            Dim nomeFileIter = modelli.Where(Function(c) c.Id = procedimento.IdModelloWkf).Select(Function(c) c.NomeFile)
            If nomeFileIter.Any Then
                procedimentoWS.Iter = nomeFileIter.FirstOrDefault
            End If
            modelli.Dispose()

            'If Me.Procedimento Is Nothing Then
            '    procedimentoWS.Insert()
            'Else
            '    procedimentoWS.Update()
            'End If

            procedimentoWS.Save()

            Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathModuli")

            Dim path As String = String.Empty
            Dim documenti = procedimento.ModuliProcedimento.Where(Function(c) Not c.Nomefile Is Nothing)
            For Each documento In documenti
                path = percorsoRoot & String.Format("MOD_{0}_{1}", documento.Codice.ToString.PadLeft(9, "0"), documento.Nomefile)
                If IO.File.Exists(path) Then
                    procedimentoWS.UploadFile(documento.Id, path)
                End If
            Next
        End Using


    End Sub

    Private Sub ResettaVista()
        Me.Procedimento = Nothing

        Me.NomeTextBox.Text = String.Empty
        Me.NormativaTextBox.Text = String.Empty

        Me.TermineConclusioneTextBox.Value = 1


        Me.GiorniSospensioneTextBox.Text = Me.GiorniSospensione.ToString


        Me.AbilitatoCheckBox.Checked = True
        Me.PubblicabileCheckBox.Checked = False

        Me.ClasseTextBox.Text = String.Empty
        Me.CodiceTextBox.Text = String.Empty
        Me.ResettaSettore()
        Me.SettoriProcedimento = New List(Of ParsecAdmin.SettoreProcedimento)

        Me.DescrizioneTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty
        Me.IndicazioniTextBox.Text = String.Empty

        Me.ResponsabileTextBox.Text = String.Empty
        Me.IdResponsabileTextBox.Text = String.Empty
        Me.CodiceResponsabileTextBox.Text = String.Empty

        Me.ResponsabileInerziaTextBox.Text = String.Empty
        Me.IdResponsabileInerziaTextBox.Text = String.Empty
        Me.CodiceResponsabileInerziaTextBox.Text = String.Empty

        Me.SettoreCompetenzaTextBox.Text = String.Empty
        Me.CodiceSettoreCompetenzaTextBox.Text = String.Empty
        Me.IdSettoreCompetenzaTextBox.Text = String.Empty

        Me.ImpresaCheckBox.Checked = True
        Me.ProfessionistaCheckBox.Checked = True
        Me.PrivatoCheckBox.Checked = True
        Me.PubblicoCheckBox.Checked = True


        Me.DescrizioneModuloTextBox.Text = String.Empty
        Me.NoteModuloTextBox.Text = String.Empty
        Me.NomeModuloTextBox.Text = String.Empty
        Me.ObbligatorioCheckBox.Checked = True
        Me.ObbligatorioFirmaDigitaleCheckBox.Checked = False
        Me.ModuliProcedimento = New List(Of ParsecAdmin.ModuloProcedimento)


        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.ClassificazioneTextBox.Text = String.Empty
        Me.CodiceClassificazioneTextBox.Text = String.Empty

        Me.IterComboBox.SelectedIndex = 0
        Me.IterIntegrazioneComboBox.SelectedIndex = 0

    End Sub

    Private Sub AggiornaVista(ByVal procedimento As ParsecAdmin.Procedimento)
        Me.ResettaVista()

        Me.NomeTextBox.Text = procedimento.Nome
        Me.NormativaTextBox.Text = procedimento.RiferimentoNormativo
        Me.CodiceTextBox.Text = procedimento.Codice
        Me.ClasseTextBox.Text = procedimento.ClasseRischio

        Me.TermineConclusioneTextBox.Value = Nothing

        If procedimento.Tempo.HasValue Then
            Me.TermineConclusioneTextBox.Value = procedimento.Tempo.Value
        End If

        If procedimento.GiorniSospensione.HasValue Then
            Me.GiorniSospensioneTextBox.Text = procedimento.GiorniSospensione.Value
        Else
            Me.GiorniSospensioneTextBox.Text = String.Empty
        End If

        Me.AbilitatoCheckBox.Checked = procedimento.Abilitato

        Me.PubblicabileCheckBox.Checked = procedimento.Pubblicabile

        Me.DescrizioneTextBox.Text = procedimento.Descrizione
        Me.IndicazioniTextBox.Text = procedimento.Indicazioni
        Me.NoteTextBox.Text = procedimento.Note

        If procedimento.IdResponsabile.HasValue Then
            Me.ResponsabileTextBox.Text = procedimento.DescrizioneResponsabile
            Me.CodiceResponsabileTextBox.Text = procedimento.CodiceResponsabile
            Me.IdResponsabileTextBox.Text = procedimento.IdResponsabile
        End If

        If procedimento.IdResponsabileInerzia.HasValue Then
            Me.ResponsabileInerziaTextBox.Text = procedimento.DescrizioneResponsabileInerzia
            Me.CodiceResponsabileInerziaTextBox.Text = procedimento.CodiceResponsabileInerzia
            Me.IdResponsabileInerziaTextBox.Text = procedimento.IdResponsabileInerzia
        End If

        If procedimento.IdSettoreCompetenza.HasValue Then
            Me.SettoreCompetenzaTextBox.Text = procedimento.DescrizioneSettoreCompetenza
            Me.CodiceSettoreCompetenzaTextBox.Text = procedimento.CodiceStruttura
            Me.IdSettoreCompetenzaTextBox.Text = procedimento.IdSettoreCompetenza
        End If

        Me.ImpresaCheckBox.Checked = procedimento.Imprese
        Me.ProfessionistaCheckBox.Checked = procedimento.Professionisti
        Me.PrivatoCheckBox.Checked = procedimento.Privati
        Me.PubblicoCheckBox.Checked = procedimento.Pubblico


        Me.SettoriProcedimento = procedimento.ListaSettori
        Me.ModuliProcedimento = procedimento.ModuliProcedimento


        If procedimento.CodClassificaAppartenenza.HasValue Then
            'Dim titolario As ParsecAdmin.TitolarioClassificazione = (New ParsecAdmin.TitolarioClassificazioneRepository).GetQuery.Where(Function(c) c.Id = procedimento.CodClassificaAppartenenza).FirstOrDefault
            'Dim codici = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione(CInt(procedimento.CodClassificaAppartenenza.Value.ToString), 1)
            'Dim classificazioneCompleta As String = codici & " " & titolario.Descrizione
            Me.IdClassificazioneTextBox.Text = procedimento.CodClassificaAppartenenza.Value.ToString
            Me.ClassificazioneTextBox.Text = procedimento.DescrizioneClassificazione
            Me.CodiceClassificazioneTextBox.Text = procedimento.CodiciClassificazione
        End If

        If procedimento.IdModelloWkf.HasValue Then
            Me.IterComboBox.FindItemByValue(procedimento.IdModelloWkf).Selected = True
        End If

        If procedimento.IdModelloWkfIntegrazione.HasValue Then
            Me.IterIntegrazioneComboBox.FindItemByValue(procedimento.IdModelloWkfIntegrazione).Selected = True
        End If

        Me.Procedimento = procedimento
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
        Dim procedimento = procedimenti.GetById(id)
        procedimenti.Dispose()
        Me.AggiornaVista(procedimento)
    End Sub

#End Region

#Region "EVENTI GRIGLIA SETTORI"

    Private Sub AggiornaSettore()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim struttura = struttureSelezionate.First
            'Aggiorno il settore
            Me.SettoreTextBox.Text = struttura.Descrizione
            Me.IdSettoreTextBox.Text = struttura.Id.ToString
            Me.CodiceSettoreTextBox.Text = struttura.Codice
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Private Sub TrovaSettore()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaSettoreImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 910, 670, queryString, False)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", ParsecAdmin.TipoModulo.ATT)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100")
        parametriPagina.Add("ultimoLivelloStruttura", "100")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    End Sub

    Private Sub ResettaSettore()
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
        Me.CodiceSettoreTextBox.Text = String.Empty
    End Sub

    Protected Sub addSettoreCommandButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles addSettoreCommandButton.Click
        'If Me.SettoriProcedimento Is Nothing Then
        '    Me.SettoriProcedimento = New List(Of ParsecAdmin.SettoreProcedimento)
        'End If

        If String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un settore!", False)
            Exit Sub
        End If

        If Not Me.SettoriProcedimento Is Nothing Then
            Dim esiste = Not Me.SettoriProcedimento.Where(Function(w) w.CodiceStruttura = Me.CodiceSettoreTextBox.Text).FirstOrDefault Is Nothing
            If esiste Then
                ParsecUtility.Utility.MessageBox("Il settore è già presente!", False)
                Me.ResettaSettore()
                Exit Sub
            End If
        End If

        Dim strutture As New ParsecAdmin.StructureRepository()
        Dim settore = strutture.GetById(Me.IdSettoreTextBox.Text)

        If Not settore Is Nothing Then
            Dim settoreProcedimento As New ParsecAdmin.SettoreProcedimento
            settoreProcedimento.CodiceStruttura = Me.CodiceSettoreTextBox.Text
            settoreProcedimento.struttura = settore
            Me.SettoriProcedimento.Add(settoreProcedimento)
        End If
        strutture.Dispose()
        Me.ResettaSettore()


    End Sub

    Protected Sub SettoriGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles SettoriGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub SettoriGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles SettoriGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Me.EliminaSettore(e)
        End Select
    End Sub

    Private Sub EliminaSettore(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim codiceStruttura As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("CodiceStruttura")

        Dim settoriProcedimento As List(Of ParsecAdmin.SettoreProcedimento) = Me.SettoriProcedimento

        'Elimino il settore selezionato dalla cache
        Dim settore As ParsecAdmin.SettoreProcedimento = settoriProcedimento.Where(Function(c) c.CodiceStruttura = codiceStruttura).FirstOrDefault

        If Not settore Is Nothing Then
            settoriProcedimento.Remove(settore)
        End If

        Me.SettoriProcedimento = settoriProcedimento

    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub EsportaInExcelImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EsportaInExcelImageButton.Click
        Me.Esporta()
    End Sub

    Protected Sub TrovaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaSettoreImageButton.Click
        Me.TrovaSettore()
    End Sub

    Protected Sub AggiornaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSettoreImageButton.Click
        Me.AggiornaSettore()
    End Sub

   
    Private Sub Esporta()
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("Procedimenti_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)


        Dim line As New StringBuilder

        Dim settori As String = String.Empty
        line.Append("NOME" & vbTab)
        line.Append("STRUTTURA COMPETENTE" & vbTab)
        line.Append("RESPONSABILE" & vbTab)
        line.Append("NORMATIVA" & vbTab)
        line.Append("TERMINE" & vbTab)

        swExport.WriteLine(line.ToString)
        line.Clear()

        'Dim settoriProcedimento As New SettoreProcedimentoRepository
        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
        Dim lista = procedimenti.GetViewStampa(Nothing)
        For Each p As ParsecAdmin.Procedimento In lista

            line.Append(If(Not String.IsNullOrEmpty(p.Nome), p.Nome.Replace(";", ","), String.Empty) & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(p.DescrizioneSettoreCompetenza), p.DescrizioneSettoreCompetenza.Replace(";", ","), String.Empty) & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(p.DescrizioneResponsabile), p.DescrizioneResponsabile.Replace(";", ","), String.Empty) & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(p.RiferimentoNormativo), p.RiferimentoNormativo.Replace(";", ","), String.Empty) & vbTab)
            line.Append(If(p.Tempo.HasValue, p.Tempo.Value.ToString, String.Empty) & vbTab)


            'Dim lista = settoriProcedimento.getSettori(p.Id)
            'Dim i As Integer = 0
            'If lista.Count > 0 Then
            '    For Each s In lista
            '        If i = 0 Then
            '            line.Append(s.struttura.Descrizione)
            '            swExport.WriteLine(line.ToString)
            '            line.Clear()
            '        Else
            '            line.Append("" & vbTab)
            '            line.Append("" & vbTab)
            '            line.Append(s.struttura.Descrizione)

            '            swExport.WriteLine(line.ToString)
            '            line.Clear()
            '        End If
            '        i += 1
            '    Next
            'Else
            swExport.WriteLine(line.ToString)
            line.Clear()
            'End If

        Next

        procedimenti.Dispose()
        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

        Me.EsportaInExcelImageButton.Enabled = True
    End Sub

#End Region

End Class