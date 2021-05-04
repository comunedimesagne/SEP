Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class StoricoAttiAmministrativiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object


#Region "ENUMERAZIONI"

    Public Enum TipoPannello
        Risultati = 0
        Dettaglio = 1
    End Enum

#End Region

#Region "GESTIONI PANNELLI"

    Private Sub VisualizzaPannello(ByVal tipo As TipoPannello)
        Me.RisultatiPanel.Visible = False
        Me.DettaglioPanel.Visible = False
        Select Case tipo
            Case TipoPannello.Risultati
                Me.RisultatiPanel.Visible = True
            Case TipoPannello.Dettaglio
                Me.DettaglioPanel.Visible = True
        End Select
    End Sub

#End Region

#Region "PROPRIETA'"

    Public Property TipologiaDocumento As ParsecAtt.TipoDocumento
        Set(ByVal value As ParsecAtt.TipoDocumento)
            ViewState("TipologiaDocumento") = value
        End Set
        Get
            Return CType(ViewState("TipologiaDocumento"), ParsecAtt.TipoDocumento)
        End Get
    End Property

    Public Property CurrentPosition As Integer
        Get
            Return Session("StoricoAttiAmministrativiPage_CurrentPosition")
        End Get
        Set(ByVal value As Integer)
            Session("StoricoAttiAmministrativiPage_CurrentPosition") = value
        End Set
    End Property

    Public Property IdDocumentoSelezionato As Nullable(Of Integer)
        Get
            Return Session("StoricoAttiAmministrativiPage_IdDocumentoSelezionato")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            Session("StoricoAttiAmministrativiPage_IdDocumentoSelezionato") = value
        End Set
    End Property


    Public Property Documento() As ParsecAtt.Documento
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_Documento"), ParsecAtt.Documento)
        End Get
        Set(ByVal value As ParsecAtt.Documento)
            Session("StoricoAttiAmministrativiPage_Documento") = value
        End Set
    End Property


    Public Property Documenti() As List(Of ParsecAtt.Documento)
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_Documenti"), List(Of ParsecAtt.Documento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Documento))
            Session("StoricoAttiAmministrativiPage_Documenti") = value
        End Set
    End Property


    Public Property Liquidazioni() As List(Of ParsecAtt.Liquidazione)
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_Liquidazioni"), List(Of ParsecAtt.Liquidazione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Liquidazione))
            Session("StoricoAttiAmministrativiPage_Liquidazioni") = value
        End Set
    End Property

    Public Property ImpegniSpesa() As List(Of ParsecAtt.ImpegnoSpesa)
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_ImpegniSpesa"), List(Of ParsecAtt.ImpegnoSpesa))
        End Get
        Set(ByVal value As List(Of ParsecAtt.ImpegnoSpesa))
            Session("StoricoAttiAmministrativiPage_ImpegniSpesa") = value
        End Set
    End Property

    Public Property Accertamenti() As List(Of ParsecAtt.Accertamento)
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_Accertamenti"), List(Of ParsecAtt.Accertamento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Accertamento))
            Session("StoricoAttiAmministrativiPage_Accertamenti") = value
        End Set
    End Property

    Public Property Classificazioni() As List(Of ParsecAtt.DocumentoClassificazione)
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_Classificazioni"), List(Of ParsecAtt.DocumentoClassificazione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.DocumentoClassificazione))
            Session("StoricoAttiAmministrativiPage_Classificazioni") = value
        End Set
    End Property

    Public Property Presenze() As List(Of ParsecAtt.DocumentoPresenza)
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_Presenze"), List(Of ParsecAtt.DocumentoPresenza))
        End Get
        Set(ByVal value As List(Of ParsecAtt.DocumentoPresenza))
            Session("StoricoAttiAmministrativiPage_Presenze") = value
        End Set
    End Property

    Public Property Firme() As List(Of ParsecAtt.Firma)
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_Firme"), List(Of ParsecAtt.Firma))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Firma))
            Session("StoricoAttiAmministrativiPage_Firme") = value
        End Set
    End Property

    Public Property Allegati() As List(Of ParsecAtt.Allegato)
        Get
            Return CType(Session("StoricoAttiAmministrativiPage_Allegati"), List(Of ParsecAtt.Allegato))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Allegato))
            Session("StoricoAttiAmministrativiPage_Allegati") = value
        End Set
    End Property


    'Elenco di gruppi o di utenti abilitati a visualizzare un protcollo.
    Public Property Visibilita() As List(Of ParsecAdmin.VisibilitaDocumento)
        Get
            Return Session("StoricoAttiAmministrativiPage_Visibilita")
        End Get
        Set(ByVal value As List(Of ParsecAdmin.VisibilitaDocumento))
            Session("StoricoAttiAmministrativiPage_Visibilita") = value
        End Set
    End Property

    'luca 10/07/2020
    '' ''Public Property Trasparenza() As ParsecAdmin.Pubblicazione
    '' ''    Get
    '' ''        Return Session("StoricoAttiAmministrativiPage_Trasparenza")
    '' ''    End Get
    '' ''    Set(ByVal value As ParsecAdmin.Pubblicazione)
    '' ''        Session("StoricoAttiAmministrativiPage_Trasparenza") = value
    '' ''    End Set
    '' ''End Property

    'luca 10/07/2020
    '' ''Public Property AttiConcessione() As List(Of ParsecAdmin.AttoConcessione)
    '' ''    Get
    '' ''        Return CType(Session("StoricoAttiAmministrativiPage_AttiConcessione"), List(Of ParsecAdmin.AttoConcessione))
    '' ''    End Get
    '' ''    Set(ByVal value As List(Of ParsecAdmin.AttoConcessione))
    '' ''        Session("StoricoAttiAmministrativiPage_AttiConcessione") = value
    '' ''    End Set
    '' ''End Property


    Public Property Fascicoli() As List(Of ParsecAdmin.Fascicolo)
        Get
            Return Session("StoricoAttiAmministrativiPage_Fascicoli")
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Fascicolo))
            Session("StoricoAttiAmministrativiPage_Fascicoli") = value
        End Set
    End Property

    'luca 10/07/2020
    '' ''Public Property AllegatiPubblicazione() As List(Of ParsecAdmin.AllegatoPubblicazione)
    '' ''    Get
    '' ''        Return CType(Session("StoricoAttiAmministrativiPage_AllegatiPubblicazione"), List(Of ParsecAdmin.AllegatoPubblicazione))
    '' ''    End Get
    '' ''    Set(ByVal value As List(Of ParsecAdmin.AllegatoPubblicazione))
    '' ''        Session("StoricoAttiAmministrativiPage_AllegatiPubblicazione") = value
    '' ''    End Set
    '' ''End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.MainPage = CType(Me.Master, BasePage)
        CType(Me.Master, BasePage).ImpostaLarghezzaHeader(900)
        CType(Me.Master, BasePage).DescrizioneProcedura = "Storico Atti Decisionali"

        Me.CaricaTipologieDocumento()
        Me.CaricaStatiDiscussione()

        If Not Me.IsPostBack Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Me.Documenti = New List(Of ParsecAtt.Documento)
            Dim codice As Integer = CInt(Request.QueryString("r"))
            Me.Documenti = documenti.GetStorico(codice)
            documenti.Dispose()

            Me.AttiTabStrip.MultiPage.PageViews(0).Selected = True
            Me.AttiTabStrip.Tabs(0).Selected = True

        End If

        Me.VisualizzaPannello(TipoPannello.Risultati)
        Me.SetButtonImage()

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If




        Dim livelli As New ParsecAdmin.StrutturaLivelloRepository
        Dim primoLivelloStruttura As ParsecAdmin.StrutturaLivello = livelli.GetQuery.Where(Function(c) c.Gerarchia = 100).FirstOrDefault
        Dim secondoLivelloStruttura As ParsecAdmin.StrutturaLivello = livelli.GetQuery.Where(Function(c) c.Gerarchia = 200).FirstOrDefault

        If Not primoLivelloStruttura Is Nothing Then
            Me.SettoreLabel.Text = primoLivelloStruttura.Descrizione

        End If
        If Not secondoLivelloStruttura Is Nothing Then
            Me.UfficioLabel.Text = secondoLivelloStruttura.Descrizione & " *"

        End If
        livelli.Dispose()



        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

       

        Me.ClassificazioniPanel.Style.Add("width", widthStyle)
        Me.DettaglioClassificazionePanel.Style.Add("width", widthStyle)
        Me.GrigliaClassificazioniPanel.Style.Add("width", widthStyle)

        'luca 10/07/2020
        '' ''Me.TrasparenzaPanel.Style.Add("width", widthStyle)
        '' ''Me.AttiConsessionePanel2.Style.Add("width", widthStyle)

        Me.VisibilitaPanel.Style.Add("width", widthStyle)
        Me.AllegatiPanel.Style.Add("width", widthStyle)
        Me.GrigliaAllegatiPanel.Style.Add("width", widthStyle)
        Me.ContabilitaPanel.Style.Add("width", widthStyle)
        Me.PresenzePanel.Style.Add("width", widthStyle)
        Me.GrigliaPresenzePanel.Style.Add("width", widthStyle)
        Me.GeneralePanel.Style.Add("width", widthStyle)
         Me.AttiMultiPage.Style.Add("width", widthStyle)

        Me.mainAreaPanel.Style.Add("width", widthStyle)
        Me.mainAreaPanel.Style.Add("height", "538px")

        Me.GrigliaFascicoliPanel.Style.Add("width", widthStyle)
        Me.FascicoliGridView.Style.Add("width", widthStyle)
        Me.FascicoliPanel.Style.Add("width", widthStyle)

        'luca 10/07/2020
        '' ''Me.CompensoConsulenzaPanel.Style.Add("width", widthStyle)
        '' ''Me.BandiGareContrattiPanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiBandoGaraPanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiBandoGaraPanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiBandoGaraGridView.Style.Add("width", widthStyle)


    End Sub

 

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub DocumentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DocumentiGridView.NeedDataSource
        Me.DocumentiGridView.DataSource = Me.Documenti
    End Sub

    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
        End Select
    End Sub

    Protected Sub DocumentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona registrazione"
            End If
        End If
    End Sub


#End Region

#Region "EVENTI CONTROLLI"

    Private Function GetAnnoEsercizio(documento As ParsecAtt.Documento) As Integer
        Dim annoEsercizio As Integer = Now.Year
        Try
            Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
            annoEsercizio = CInt(rgx.Match(documento.Nomefile).Value)
        Catch ex As Exception
        End Try
        Return annoEsercizio
    End Function

    Protected Sub VisualizzaDocumentoCollegatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoCollegatoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim attoCollegato As ParsecAtt.Documento = Nothing
            ' Se si visualizza il dettaglio di un proposta, il documento collegato è l'atto finale adottato
            ' altrimenti il documento collegato è la proposta iniziale

            If Me.Documento.Modello.Proposta Then
                attoCollegato = Me.Documento.DocumentoGenerato  'atto finale
                ' attoCollegato = documenti.GetQuery.Where(Function(c) c.IdPadre = Me.Documento.Id).OrderByDescending(Function(c) c.NumVersione).FirstOrDefault
            Else
                attoCollegato = Me.Documento.DocumentoCollegato  'proposta
                'attoCollegato = documenti.GetQuery.Where(Function(c) c.Id = Me.Documento.IdPadre).OrderByDescending(Function(c) c.NumVersione).FirstOrDefault
            End If


            If Not attoCollegato Is Nothing Then
                Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), Me.GetAnnoEsercizio(Me.Documento), attoCollegato.Nomefile)
                If IO.File.Exists(localPath) Then
                    Me.VisualizzaDocumento(attoCollegato.Nomefile, Me.GetAnnoEsercizio(attoCollegato), False)
                Else
                    ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
                End If

            End If
            documenti.Dispose()
        End If

    End Sub

    Protected Sub VisualizzaCopiaDocumentoCollegatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaCopiaDocumentoCollegatoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim attoCollegato As ParsecAtt.Documento = Nothing
            If Me.Documento.Modello.Proposta Then
                attoCollegato = Me.Documento.DocumentoGenerato  'atto finale
                'attoCollegato = documenti.GetQuery.Where(Function(c) c.IdPadre = Me.Documento.Id).OrderByDescending(Function(c) c.NumVersione).FirstOrDefault
            Else
                attoCollegato = Me.Documento.DocumentoCollegato  'proposta
                'attoCollegato = documenti.GetQuery.Where(Function(c) c.Id = Me.Documento.IdPadre).OrderByDescending(Function(c) c.NumVersione).FirstOrDefault
            End If

            If Not attoCollegato Is Nothing Then
                Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), Me.GetAnnoEsercizio(Me.Documento), attoCollegato.Nomefile)
                If IO.File.Exists(localPath) Then
                    Me.VisualizzaCopiaDocumento(attoCollegato.Nomefile, Me.GetAnnoEsercizio(attoCollegato), False)
                Else
                    ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
                End If

            End If
            documenti.Dispose()
        End If
    End Sub

    Protected Sub VisualizzaCopiaDocumentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaCopiaDocumentoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim nomefile As String = Me.Documento.Nomefile

            Dim annoEsercizio As Integer = Now.Year
            If Me.Documento.Modello.Proposta Then
                annoEsercizio = Me.Documento.DataProposta.Value.Year
            Else
                annoEsercizio = Me.Documento.Data.Value.Year
            End If

            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefile)

            If IO.File.Exists(localPath) Then
                Me.VisualizzaCopiaDocumento(nomefile, annoEsercizio, False)
            Else
                ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
            End If
        End If
    End Sub

    Protected Sub VisualizzaDocumentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim nomefile As String = Me.Documento.Nomefile

            Dim annoEsercizio As Integer = Now.Year
            If Me.Documento.Modello.Proposta Then
                annoEsercizio = Me.Documento.DataProposta.Value.Year
            Else
                annoEsercizio = Me.Documento.Data.Value.Year
            End If

            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefile)

            If IO.File.Exists(localPath) Then
                Me.VisualizzaDocumento(nomefile, annoEsercizio, False)
            Else
                ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
            End If
        End If
    End Sub

    Protected Sub VisualizzaDocumentoFirmatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoFirmatoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim nomefileFirmato As String = IO.Path.GetFileNameWithoutExtension(Me.Documento.Nomefile) & ".pdf.p7m"

            Dim annoEsercizio As Integer = Now.Year
            If Me.Documento.Modello.Proposta Then
                annoEsercizio = Me.Documento.DataProposta.Value.Year
            Else
                annoEsercizio = Me.Documento.Data.Value.Year
            End If

            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefileFirmato)

            If IO.File.Exists(localPath) Then
                Me.VisualizzaDocumento(nomefileFirmato, annoEsercizio, False)
            End If
        End If
    End Sub


#End Region

#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub VisualizzaDocumento(ByVal nomeFile As String, annoEsercizio As Integer, ByVal enabled As Boolean)
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim datiInput As ParsecAdmin.DatiInput

        Dim pathDownload = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomeFile)

        If IO.Path.GetExtension(nomeFile).ToLower <> ".odt" Then
            datiInput = New ParsecAdmin.DatiInput With {.Path = pathDownload, .ShowWindow = False, .Enabled = False, .FunctionName = "OpenGenericDocument"}
        Else
            datiInput = New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = "ViewDocument"}
        End If
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

    Private Sub VisualizzaCopiaDocumento(ByVal nomeFile As String, annoEsercizio As Integer, ByVal enabled As Boolean)
        Dim originale As String = ParsecAdmin.WebConfigSettings.GetKey("PrefissoSezioneOriginale")
        Dim copia As String = ParsecAdmin.WebConfigSettings.GetKey("PrefissoSezioneCopiaConforme")
        Dim shadow As String = String.Empty
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("nome_sezione_shadow", ParsecAdmin.TipoModulo.ATT)
        If Not parametro Is Nothing Then
            shadow = parametro.Valore
        End If
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters

        Dim pathDownload = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomeFile)

        Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = "ViewDocument", .PrefixCopy = copia, .PrefixOriginal = originale, .Shadow = shadow}
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

#Region "GESTIONE FIRME"

    Protected Sub FirmeGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FirmeGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim firma As ParsecAtt.Firma = CType(e.Item.DataItem, ParsecAtt.Firma)
            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Preview").Controls(0), ImageButton)
                If Not String.IsNullOrEmpty(firma.FileFirmato) Then
                    If IO.Path.GetExtension(firma.FileFirmato).ToLower = ".p7m" Then
                        btn.ImageUrl = "~\images\signedDocument.png"
                        btn.ToolTip = "Visualizza file firmato..."
                    Else
                        btn.ImageUrl = "~\images\vuoto.png"
                        btn.ToolTip = ""
                        btn.Attributes.Add("onclick", "return false;")
                    End If
                Else
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If
        End If
    End Sub


    Protected Sub FirmeGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FirmeGridView.ItemCommand
        If e.CommandName = "Select" Then
            'Me.ModificaFirma(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.AnteprimaP7m(e.Item)
        End If
    End Sub

    Private Sub AnteprimaP7m(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim firma As ParsecAtt.Firma = Me.Firme.Where(Function(c) c.Id = id).FirstOrDefault


        Dim annoEsercizio As Integer = Now.Year
        If Me.Documento.Modello.Proposta Then
            annoEsercizio = Me.Documento.DataProposta.Value.Year
        Else
            annoEsercizio = Me.Documento.Data.Value.Year
        End If

        Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, firma.FileFirmato)

        If Not IO.File.Exists(localPath) Then
            ParsecUtility.Utility.MessageBox("Il file firmato selezionato non esiste!", False)
        Else
            Me.VisualizzaDocumentoP7M(firma.FileFirmato, annoEsercizio)
        End If
    End Sub

    Private Sub VisualizzaDocumentoP7M(ByVal nomeFile As String, annoEsercizio As Integer)
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters

        Dim pathDownload = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomeFile)

        Dim datiInput As New ParsecAdmin.DatiInput With {.Path = pathDownload, .ShowWindow = False, .Enabled = False, .FunctionName = "OpenGenericDocument"}
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


#End Region

#Region "GESTIONE PRESENZE"

    Protected Sub PresenzeGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles PresenzeGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim presenza As ParsecAtt.DocumentoPresenza = CType(e.Item.DataItem, ParsecAtt.DocumentoPresenza)
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("PresenteCheckBox"), CheckBox)
            chk.Checked = presenza.Presente
            chk.Enabled = False
        End If
    End Sub

#End Region

    'luca 10/07/2020
    '' ''#Region "GESTIONE TRASPARENZA (ATTI CONCESSIONE)"

    '' ''    Protected Sub BeneficiariGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles BeneficiariGridView.ItemCommand
    '' ''        If e.CommandName = "Preview" Then
    '' ''            Me.PreviewBeneficiario(e.Item)
    '' ''        End If
    '' ''    End Sub

    '' ''    Private Sub BeneficiariGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles BeneficiariGridView.ItemCreated
    '' ''        If TypeOf e.Item Is GridHeaderItem Then
    '' ''            e.Item.Style.Add("position", "relative")
    '' ''            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop -1)")
    '' ''            e.Item.Style.Add("z-index", "99")
    '' ''            e.Item.Style.Add("background-color", "White")
    '' ''        End If
    '' ''    End Sub

    '' ''    Private Sub PreviewBeneficiario(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''        Dim idBeneficiario As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdBeneficiario")
    '' ''        Dim attoConcessione As ParsecAdmin.AttoConcessione = AttiConcessione.Where(Function(b) b.IdBeneficiario = idBeneficiario).FirstOrDefault
    '' ''        Me.VisualizzaAttoConcessione(attoConcessione, Nothing, "Preview")
    '' ''    End Sub


    '' ''    Private Sub VisualizzaAttoConcessione(ByVal attoConcessione As ParsecAdmin.AttoConcessione, ByVal buttonToClick As String, ByVal mode As String)
    '' ''        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AttoConcessionePage.aspx"
    '' ''        Dim queryString As New Hashtable
    '' ''        queryString.Add("Mode", mode)
    '' ''        If Not String.IsNullOrEmpty(buttonToClick) Then
    '' ''            queryString.Add("obj", buttonToClick)
    '' ''        End If
    '' ''        Dim parametriPagina As New Hashtable
    '' ''        parametriPagina.Add("AttoConcessione", attoConcessione)
    '' ''        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    '' ''        ParsecUtility.Utility.ShowPopup(pageUrl, 610, 550, queryString, False)
    '' ''    End Sub

    '' ''#End Region

#Region "GESTIONE ALLEGATI"


    Private Sub VisualizzaDocumentoP7M(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        Dim percorsoRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim nomeFile As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As ParsecAtt.Allegato = Me.Allegati.Where(Function(c) c.Nomefile = nomeFile).FirstOrDefault

        If Not allegato Is Nothing Then

            Dim pathDownload As String = String.Empty

            If allegato.Id < 0 Then

                Dim temp = allegato.NomeFileTemp
                '********************************************************************************
                'LA FIRMA DI UN DOCUMENTO ODT LO CONVERTE IN PDF
                '********************************************************************************
                If IO.Path.GetExtension(allegato.Nomefile).ToLower = ".odt" Then
                    temp = temp.Remove(temp.Length - 4, 4) & ".pdf"
                End If
                '********************************************************************************
                pathDownload = percorsoRootTemp & temp & ".p7m"

            Else
                If percorsoRoot.EndsWith("\") Then
                    percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                End If
                pathDownload = percorsoRoot & allegato.PercorsoRelativo & allegato.NomeFileFirmato
            End If

            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then

                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)

            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        Else
            'NIENTE
        End If
    End Sub


    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)
        '********************************************************************************
        'Modificata per utilizzo GridTemplateColumn
        'Dim selectedItem As GridDataItem = item.OwnerTableView.Items(item.ItemIndex)
        'Dim filename As String = selectedItem("Nomefile").Text
        '********************************************************************************
        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

        Dim allegato As ParsecAtt.Allegato = Me.Allegati.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If allegato.Id = 0 Then
                pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.NomeFileTemp
            Else
                percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                pathDownload = percorsoRoot & allegato.PercorsoRelativo & allegato.Nomefile
            End If
            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
                'Dim pageUrl As String = String.Empty
                'If allegato.Id = 0 Then
                '    pageUrl = ParsecAdmin.WebConfigSettings.GetKey("PathHTTPDocumentTemp") & file.Name & "?rnd=" & Now.Millisecond.ToString
                'Else
                '    pageUrl = ParsecAdmin.WebConfigSettings.GetKey("PathHTTPDocument") & allegato.PercorsoRelativo.Replace("\", "/") & file.Name & "?rnd=" & Now.Millisecond.ToString
                'End If

                'ParsecUtility.Utility.ShowPopup(pageUrl.Replace("//", "/").Replace("'", "\'"), 900, 700, Nothing, False)

            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        End If
    End Sub

    Protected Sub AllegatiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AllegatiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina allegato"
            End If

            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Preview").Controls(0), ImageButton)
                btn.ToolTip = "Visualizza allegato"
            End If

            If TypeOf dataItem("SignedPreview").Controls(0) Is ImageButton Then

                Dim btnSignedPreview As ImageButton = CType(dataItem("SignedPreview").Controls(0), ImageButton)

                'NASCONDO IL PULSANTE
                'btnSignedPreview.Visible = False
                'dataItem("SignedPreview").Controls.Add(New LiteralControl("&nbsp;"))

                Dim nomeFileFirmato As String = dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("NomeFileFirmato")
                If Not String.IsNullOrEmpty(nomeFileFirmato) Then
                    btnSignedPreview.ImageUrl = "~\images\signedDocument16.png"
                    btnSignedPreview.ToolTip = "Visualizza documento firmato."
                Else
                    btnSignedPreview.ImageUrl = "~\images\vuoto.png"
                    btnSignedPreview.Attributes.Add("onclick", "return false;")
                    btnSignedPreview.ToolTip = ""
                End If

            End If


            If Me.Allegati.Count > 0 Then
                Dim lbl As Label = CType(e.Item.FindControl("NumeratoreLabel"), Label)
                lbl.Text = (e.Item.ItemIndex + 1).ToString
            End If
        End If
    End Sub

    Protected Sub AllegatiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiGridView.ItemCommand
       

        Select Case e.CommandName
            Case "Preview"
                Me.DownloadFile(e.Item)

            Case "SignedPreview"
                Me.VisualizzaDocumentoP7M(e.Item)
        End Select
    End Sub


#End Region

#Region "AZIONI PANNELLO RISULTATI"

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.IdDocumentoSelezionato = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    End Sub

    Protected Sub DettaglioImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DettaglioImageButton.Click
        If Me.IdDocumentoSelezionato.HasValue Then
            'Trovo la posizione corrente nella lista
            Dim list As List(Of ParsecAtt.Documento) = Me.Documenti
            Me.CurrentPosition = list.FindIndex(Function(c) c.Id = Me.IdDocumentoSelezionato)
            Me.CountItemLabel.Text = String.Format("di {0}", list.Count)
            Me.PositionItemTextBox.Text = (Me.CurrentPosition + 1).ToString
            Me.SetButtonState()
            Me.VisualizzaPannello(TipoPannello.Dettaglio)
            Me.AggiornaVista(Me.IdDocumentoSelezionato)
            Me.IdDocumentoSelezionato = Nothing
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un documento!", False)
        End If
    End Sub



#End Region

#Region "AZIONI PANNELLO DETTAGLIO"

    Protected Sub IndietroRisultatiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles IndietroRisultatiButton.Click
        Me.VisualizzaPannello(TipoPannello.Risultati)
        'Deseleziono la riga
        Me.DocumentiGridView.SelectedIndexes.Clear()
        Me.IdDocumentoSelezionato = Nothing
    End Sub

#End Region

#Region "GESTIONE VISUALIZZAZIONE ATTO"


    Private Sub CaricaTipologieDocumento()
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Me.TipologieDocumentoComboBox.DataValueField = "Id"
        Me.TipologieDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologieDocumentoComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaDocumento With {.Modellizzabile = True})
        Me.TipologieDocumentoComboBox.DataBind()
        tipologie.Dispose()
    End Sub

    Private Sub CaricaModelli()
        Dim modelli As New ParsecAtt.ModelliRepository
        Me.ModelliComboBox.DataValueField = "Id"
        Me.ModelliComboBox.DataTextField = "Descrizione"
        Me.ModelliComboBox.DataSource = modelli.GetKeyValue(New ParsecAtt.FiltroModello With {.TipologiaDocumento = Me.TipologiaDocumento, .Disabilitato = False})
        Me.ModelliComboBox.DataBind()
        Me.ModelliComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.ModelliComboBox.SelectedIndex = 0
        modelli.Dispose()
    End Sub

    Private Sub CaricaStatiDiscussione()
        Dim statiDiscussione As New ParsecAtt.StatoDiscussioneRepository
        Me.TipiApprovazioneComboBox.DataValueField = "Id"
        Me.TipiApprovazioneComboBox.DataTextField = "Descrizione"
        Me.TipiApprovazioneComboBox.DataSource = statiDiscussione.GetKeyValue(Nothing)
        Me.TipiApprovazioneComboBox.DataBind()
        Me.TipiApprovazioneComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipiApprovazioneComboBox.SelectedIndex = 0
        statiDiscussione.Dispose()
    End Sub

    Private Sub ResettaVista()

        Me.InfoDocumentoCollegatoLabel.Text = String.Empty
        Me.AreaInfoLabel.Text = String.Empty

        Me.NumeroAttoTextBox.Text = String.Empty
        Me.NumeroSettoreTextBox.Text = String.Empty

        Me.ModelliComboBox.SelectedIndex = 0
        Me.DataTextBox.SelectedDate = Now
        Me.OggettoTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
        Me.PubblicatoCheckBox.Checked = False

        Me.IdBozzaTextBox.Text = String.Empty
        Me.BozzaTextBox.Text = String.Empty

        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.AnnotazioniTextBox.Text = String.Empty

        'ORDINANZA E DECRETO
        Me.NumeroProtocolloTextBox.Text = String.Empty
        Me.DataProtocolloTextBox.Text = String.Empty

        'DELIBERA  DETERMINA ORDINANZA E DECRETO
        Me.DataAffissioneTextBox.SelectedDate = Nothing
        Me.NumeroRegistroPubblicazioneTextBox.Text = String.Empty
        Me.GiorniAffissioneTextBox.Text = String.Empty

        'DELIBERA
        Me.EsecutivitaImmediataCheckBox.Checked = False
        Me.GiorniEsecutivitaTextBox.Text = String.Empty

        Me.TipiApprovazioneComboBox.SelectedIndex = 0

        Me.SedutaTextBox.Text = String.Empty
        Me.IdSedutaTextBox.Text = String.Empty

        Me.Liquidazioni = New List(Of ParsecAtt.Liquidazione)
        Me.ImpegniSpesa = New List(Of ParsecAtt.ImpegnoSpesa)
        Me.Accertamenti = New List(Of ParsecAtt.Accertamento)
        Me.Classificazioni = New List(Of ParsecAtt.DocumentoClassificazione)
        Me.Presenze = New List(Of ParsecAtt.DocumentoPresenza)
        Me.Firme = New List(Of ParsecAtt.Firma)
        Me.Allegati = New List(Of ParsecAtt.Allegato)
        Me.Fascicoli = New List(Of ParsecAdmin.Fascicolo)
        Me.Visibilita = New List(Of ParsecAdmin.VisibilitaDocumento)

        'luca 10/07/2020
        '' ''Me.AllegatiPubblicazione = New List(Of ParsecAdmin.AllegatoPubblicazione)
        '' ''Me.AttiConcessione = New List(Of ParsecAdmin.AttoConcessione)
        '' ''Me.Trasparenza = Nothing

    End Sub


    '******************************************************************************************
    'Gestione visibilità pulsanti e descrizioni barra di notifica.
    '******************************************************************************************

    Private Sub ImpostaBarraNotifica(documento As ParsecAtt.Documento)

        Dim modelli As New ParsecAtt.ModelliRepository
        Dim documenti As New ParsecAtt.DocumentoRepository

        Me.VisualizzaDocumentoCollegatoImageButton.Visible = False
        Me.VisualizzaCopiaDocumentoCollegatoImageButton.Visible = False

        Dim proposta As Nullable(Of Boolean) = documento.Modello.Proposta


        'Me.VisualizzaStoricoDocumentoImageButton.Visible = True
        Dim messaggio As String = "Operazione eseguita da <b>" & Replace(documento.LogUtente.ToUpper, "'", "&acute;") & "</b> il <b>" & String.Format("{0:dd/MM/yyyy}", documento.LogDataRegistrazione) & "</b>"
        Dim width As Integer = 450
        Dim height As Integer = 21
        Me.InfoUtenteImageButton.Attributes.Add("onclick", "ShowTooltip(this,'" & messaggio & "'," & width & "," & height & ");")
        Me.InfoUtenteImageButton.Attributes.Add("onmouseout", "HideTooltip();")


        Me.VisualizzaDocumentoImageButton.Visible = True

        If proposta.HasValue Then
            Me.VisualizzaCopiaDocumentoImageButton.Visible = Not proposta
        End If

        Dim nomefileFirmato As String = IO.Path.GetFileNameWithoutExtension(documento.Nomefile) & ".pdf.p7m"

        Dim annoEsercizio As Integer = Me.GetAnnoEsercizio(Me.Documento)
        Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefileFirmato)

        Me.VisualizzaDocumentoFirmatoImageButton.Visible = IO.File.Exists(localPath)


        Dim documentoCollegato As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = documento.IdPadre).FirstOrDefault
        Dim documentoGenerato As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.IdPadre = documento.Id).FirstOrDefault


        If Not documentoCollegato Is Nothing Then
            Me.VisualizzaDocumentoCollegatoImageButton.Visible = True
            Dim modelloDocumentoCollegato As ParsecAtt.Modello = modelli.GetQuery.Where(Function(c) c.Id = documentoCollegato.IdModello).FirstOrDefault
            If Not modelloDocumentoCollegato Is Nothing Then
                If modelloDocumentoCollegato.Proposta Then
                    Me.InfoDocumentoCollegatoLabel.Text = "<font color='#00156E'>Proposta n. </font><font color='#FF6600'>" & documentoCollegato.ContatoreGenerale.ToString & " </font><font color='#00156E'> del </font><font color='#FF6600'>" & String.Format("{0:dd/MM/yyyy}", documentoCollegato.DataProposta) & "</font>"
                End If
            End If
        End If

        If Not documentoGenerato Is Nothing Then
            Me.VisualizzaDocumentoCollegatoImageButton.Visible = True
            Me.VisualizzaCopiaDocumentoCollegatoImageButton.Visible = True
            Dim modelloDocumentoGenerato As ParsecAtt.Modello = modelli.GetQuery.Where(Function(c) c.Id = documentoGenerato.IdModello).FirstOrDefault
            If Not modelloDocumentoGenerato Is Nothing Then
                If Not modelloDocumentoGenerato.Proposta Then
                    Me.InfoDocumentoCollegatoLabel.Text = "<font color='#00156E'>" & documentoGenerato.ToString & " n. </font><font color='#FF6600'>" & documentoGenerato.ContatoreGenerale.ToString & " </font><font color='#00156E'> del </font><font color='#FF6600'>" & String.Format("{0:dd/MM/yyyy}", documentoGenerato.Data) & "</font>"
                End If
            End If
        End If

        documenti.Dispose()
        modelli.Dispose()

    End Sub

    Private Sub AggiornaVista(id As Integer)

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault

        documento.DocumentoCollegato = documenti.GetDocumentoCollegato(documento.IdPadre)
        documento.DocumentoGenerato = documenti.GetDocumentoGenerato(documento.Id)


        Me.TipologiaDocumento = CType(documento.IdTipologiaDocumento, ParsecAtt.TipoDocumento)

        Dim modelli As New ParsecAtt.ModelliRepository

        Dim modello As ParsecAtt.Modello = modelli.GetById(documento.IdModello)
        modelli.Dispose()
        documento.Modello = modello

        Me.Documento = documento

        Me.ResettaVista()
        Me.DisabilitaUi()

        Me.ImpostaUi(documento)
        Me.ImpostaBarraNotifica(documento)

        Dim dataDocumento As Nullable(Of Date) = Nothing
        If documento.Modello.Proposta Then
            dataDocumento = documento.DataProposta
        Else
            dataDocumento = documento.Data
        End If

        Dim info As String = String.Format("{0} N° {1} del {2}", documento.ToString, documento.ContatoreGenerale.ToString, String.Format("{0:dd/MM/yyyy}", dataDocumento))
        Me.AreaInfoLabel.Text = info


        Me.CaricaModelli()

        '**************************************************************************************
        'Scheda Generale
        '**************************************************************************************

        Me.NumeroAttoTextBox.Text = documento.ContatoreGenerale.ToString

        If documento.ContatoreStruttura.HasValue Then
            Me.NumeroSettoreTextBox.Text = documento.ContatoreStruttura
        End If


        Me.OggettoTextBox.Text = documento.Oggetto
        Me.DataTextBox.SelectedDate = dataDocumento


        Me.NoteTextBox.Text = documento.Note

        If documento.Pubblicato.HasValue Then
            Me.PubblicatoCheckBox.Checked = documento.Pubblicato
        End If

        Me.TipologieDocumentoComboBox.FindItemByValue(documento.IdTipologiaDocumento).Selected = True

        If documento.IdModello.HasValue Then
            Me.ModelliComboBox.Items.FindItemByValue(documento.IdModello).Selected = True
        End If

        Dim strutture As New ParsecAtt.StrutturaViewRepository
        If documento.IdUfficio.HasValue Then
            Me.UfficioTextBox.Text = strutture.GetQuery.Where(Function(c) c.Id = documento.IdUfficio).FirstOrDefault.Descrizione
            Me.IdUfficioTextBox.Text = documento.IdUfficio.ToString
        End If

        Me.SettoreTextBox.Text = strutture.GetQuery.Where(Function(c) c.Id = documento.IdStruttura).FirstOrDefault.Descrizione
        Me.IdSettoreTextBox.Text = documento.IdStruttura.ToString
        strutture.Dispose()



        'ORDINANZA E DECRETO
        If documento.NumeroProtocollo.HasValue Then
            Me.NumeroProtocolloTextBox.Text = documento.NumeroProtocollo.ToString
        End If
        If documento.DataOraRegistrazione.HasValue Then
            Me.DataProtocolloTextBox.Text = String.Format("{0:dd/MM/yyyy}", documento.DataOraRegistrazione)
        End If


        'DELIBERA  DETERMINA ORDINANZA E DECRETO
        If documento.DataAffissioneDa.HasValue Then
            Me.DataAffissioneTextBox.SelectedDate = documento.DataAffissioneDa
        End If
        If documento.NumeroRegistroPubblicazione.HasValue Then
            Me.NumeroRegistroPubblicazioneTextBox.Text = documento.NumeroRegistroPubblicazione
        End If
        If documento.GiorniAffissione.HasValue Then
            Me.GiorniAffissioneTextBox.Text = documento.GiorniAffissione
        End If


        'DELIBERA
        If documento.GiorniEsecutivita.HasValue Then
            Me.GiorniEsecutivitaTextBox.Text = documento.GiorniEsecutivita.ToString
        End If
        Me.EsecutivitaImmediataCheckBox.Checked = Not documento.GiorniEsecutivita.HasValue

        '**************************************************************************************



        '**************************************************************************************
        'Scheda Contabilità
        '**************************************************************************************
        'Carico le liquidazioni associate al documento corrente
        Dim liquidazioni As New ParsecAtt.LiquidazioneRepository
        Me.Liquidazioni = liquidazioni.GetView(New ParsecAtt.FiltroLiquidazione With {.IdDocumento = documento.Id})
        liquidazioni.Dispose()


        'Carico gli impegni di spesa associati al documento corrente
        Dim impegniSpesa As New ParsecAtt.ImpegnoSpesaRepository
        Me.ImpegniSpesa = impegniSpesa.GetView(New ParsecAtt.FiltroImpegno With {.IdDocumento = documento.Id})
        impegniSpesa.Dispose()
        '**************************************************************************************

        'Carico gli accertamenti associati al documento corrente
        Dim accertamenti As New ParsecAtt.AccertamentoRepository
        Me.Accertamenti = accertamenti.GetView(New ParsecAtt.FiltroAccertamento With {.IdDocumento = documento.Id})
        accertamenti.Dispose()
        '**************************************************************************************


        '**************************************************************************************
        'Scheda Classificazione
        '**************************************************************************************
        'Carico le classificazioni associate al documento corrente
        Dim classificazioni As New ParsecAtt.DocumentoClassificazioneRepository
        Me.Classificazioni = classificazioni.GetView(New ParsecAtt.FiltroDocumentoClassificazione With {.IdDocumento = documento.Id})
        classificazioni.Dispose()
        '**************************************************************************************


        '**************************************************************************************
        'Scheda Visibilità
        '**************************************************************************************
        Me.Visibilita = documenti.GetVisibilita(documento.Id)

        '**************************************************************************************
        'Scheda Presenze
        '**************************************************************************************
        'Carico le presenze associate al documento corrente
        'Todo parametro OriginePresenzeVerbale (Fronteaspizio)



        '********************************************************************************************
        'SCHEDA TRASPARENZA
        '********************************************************************************************
        'luca 10/07/2020
        '' ''Me.NascondiPannelliTrasparenza()
        '' ''Me.Trasparenza = documenti.GetTrasparenza(documento.Id, True)
        '' ''If Not Me.Trasparenza Is Nothing Then
        '' ''    Me.VisualizzaPannelliTrasparenza(Me.Trasparenza.TipologiaSezioneTrasparente)
        '' ''    Me.AggiornaVistaPannelliTrasparenza(Me.Trasparenza)
        '' ''End If

        '********************************************************************************************


        Dim presenze As New ParsecAtt.DocumentoPresenzaRepository
        Me.Presenze = presenze.GetViewDocumento(New ParsecAtt.FiltroDocumentoPresenza With {.IdDocumento = documento.Id})

        'Carico le presenze dalla seduta
        If Me.Presenze.Count = 0 Then
            If documento.IdSeduta.HasValue Then
                Me.Presenze = presenze.GetViewSeduta(New ParsecAtt.FiltroPresenzaSeduta With {.IdSeduta = documento.IdSeduta})
            End If
        End If
        presenze.Dispose()

        If documento.IdSeduta.HasValue Then
            Dim sedute As New ParsecAtt.SedutaRepository
            Dim seduta As ParsecAtt.Seduta = sedute.GetById(documento.IdSeduta)
            Me.SedutaTextBox.Text = String.Format("di {0} del {1}", seduta.DescrizioneTipologiaSeduta, String.Format("{0:dd/MM/yyyy}", seduta.DataConvocazione))
            Me.IdSedutaTextBox.Text = seduta.Id.ToString
            Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
            Dim ordineGiorno As ParsecAtt.OrdineGiorno = ordiniGiorno.GetQuery.Where(Function(c) c.IdSeduta = seduta.Id And c.CodiceDocumento = documento.Codice).FirstOrDefault
            ordiniGiorno.Dispose()
            If Not ordineGiorno Is Nothing Then
                If ordineGiorno.IdStatoDiscussione.HasValue Then
                    Me.TipiApprovazioneComboBox.Items.FindItemByValue(ordineGiorno.IdStatoDiscussione).Selected = True
                End If
            End If
            sedute.Dispose()
        End If


        '**************************************************************************************

        '**************************************************************************************
        'Scheda Visti e Pareri
        '**************************************************************************************
        Dim firme As New ParsecAtt.FirmeRepository
        Me.Firme = firme.GetViewDocumento(New ParsecAtt.FiltroFirma With {.IdDocumento = documento.Id})
        firme.Dispose()
        '**************************************************************************************

        '**************************************************************************************
        'Scheda Allegati
        '**************************************************************************************


        Me.Allegati = documenti.GetAllegati(documento.Id)

        '**************************************************************************************


        '**************************************************************************************
        'Scheda Fascicoli
        '**************************************************************************************
        Me.Fascicoli = documenti.GetFascicoli(documento.Id)


        '**************************************************************************************
        documenti.Dispose()



        Me.AttiTabStrip.Tabs(2).Text = "Contabilità" & If((Me.ImpegniSpesa.Count + Me.Liquidazioni.Count + Me.Accertamenti.Count) > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & (Me.ImpegniSpesa.Count + Me.Liquidazioni.Count + Me.Accertamenti.Count).ToString & ")</span>", "<span style='width:20px'></span>")

        Me.AttiTabStrip.Tabs(3).Text = "Allegati" & If(Me.Allegati.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Allegati.Count.ToString & ")</span>", "<span style='width:20px'></span>")


        Me.AttiTabStrip.Tabs(5).Text = "Visibilità" & If(Me.Visibilita.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Visibilita.Count.ToString & ")</span>", "<span style='width:20px'></span>")

        If Not Me.Fascicoli Is Nothing Then
            Me.AttiTabStrip.Tabs(6).Text = "Fascicoli" & If(Me.Fascicoli.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Fascicoli.Count.ToString & ")</span>", "<span style='width:20px'></span>")
        End If

        'luca 10/07/2020
        '' ''Me.AttiTabStrip.Tabs(6).Text = "Trasparenza" & If(Not Me.Trasparenza Is Nothing, "<span style='width:20px;color:#00156E'>&nbsp;(" & 1.ToString & ")</span>", "<span style='width:20px'></span>")



    End Sub

    'luca 10/07/2020
    '' ''Private Sub AggiornaGrigliaAttiConcessione()
    '' ''    Me.BeneficiariGridView.DataSource = Me.AttiConcessione
    '' ''    Me.BeneficiariGridView.DataBind()
    '' ''End Sub

    'luca 10/07/2020
    '' ''Private Sub ResettaVistaPannelliTrasparenza()
    '' ''    Me.SezioneTrasparenzaTextBox.Text = String.Empty
    '' ''    Me.IdSezioneTrasparenzaTextBox.Text = String.Empty
    '' ''    Me.DataInizioPubblicazioneTextBox.Clear()
    '' ''    Me.DataFinePubblicazioneTextBox.Clear()

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO DATI PUBBLICAZIONE
    '' ''    '*******************************************************************************

    '' ''    Me.DataInizioPubblicazioneTextBox.DateInput.ReadOnly = True
    '' ''    Me.DataInizioPubblicazioneTextBox.DatePopupButton.Visible = False
    '' ''    Me.DataInizioPubblicazioneTextBox.DateInput.Attributes.Add("onkeydown", "event.returnValue=false;")

    '' ''    Me.DataFinePubblicazioneTextBox.DateInput.ReadOnly = True
    '' ''    Me.DataFinePubblicazioneTextBox.DatePopupButton.Visible = False
    '' ''    Me.DataFinePubblicazioneTextBox.DateInput.Attributes.Add("onkeydown", "event.returnValue=false;")

    '' ''    '*******************************************************************************

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO ATTI DI CONCESSIONE
    '' ''    '*******************************************************************************

    '' ''    Me.AttiConcessione = New List(Of ParsecAdmin.AttoConcessione)

    '' ''    '*******************************************************************************

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO INCARICHI CONFERITI E AUTORIZZATI AI DIPENDENTI
    '' ''    '*******************************************************************************
    '' ''    DataInizioIncaricoDipendenteTextBox.SelectedDate = Nothing
    '' ''    DataFineIncaricoDipendenteTextBox.SelectedDate = Nothing
    '' ''    BeneficiarioIncaricoComboBox.Text = String.Empty
    '' ''    oggettoIncaricoDipendenteTextBox.Text = String.Empty
    '' ''    ragioneIncaricoDipendenteTextBox.Text = String.Empty
    '' ''    compensoIncaricoDipendenteTextBox.Value = Nothing
    '' ''    Me.BeneficiarioIncaricoComboBox.SelectedValue = String.Empty
    '' ''    '*******************************************************************************

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO BANDI DI GARA E CONTRATTI
    '' ''    '*******************************************************************************
    '' ''    Me.FiltroBandoGaraTextBox.Text = String.Empty
    '' ''    Me.OggettoBandoGaraTextBox.Text = String.Empty
    '' ''    Me.PartecipantiListBox.Items.Clear()
    '' ''    Me.AggiudicatariListBox.Items.Clear()
    '' ''    Me.CigBandoGaraTextBox.Text = String.Empty
    '' ''    Me.ImportoAggiudicazioneTextBox.Value = Nothing
    '' ''    Me.ImportoLiquidatoTextBox.Value = Nothing
    '' ''    Me.NumeroOfferentiTextBox.Text = String.Empty
    '' ''    Me.DataInizioLavoroTextBox.SelectedDate = Nothing
    '' ''    Me.DataFineLavoroTextBox.SelectedDate = Nothing
    '' ''    Me.TipologiaSceltaComboBox.SelectedIndex = 0
    '' ''    Me.DenominazioneStrutturaProponenteTextBox.Text = String.Empty
    '' ''    Me.CodiceFiscaleProponenteTextBox.Text = String.Empty
    '' ''    '*******************************************************************************

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO CONSULENTI E COLLABORATORI
    '' ''    '*******************************************************************************
    '' ''    Me.BeneficiarioConsulenzaComboBox.Text = String.Empty
    '' ''    Me.BeneficiarioConsulenzaComboBox.SelectedValue = String.Empty
    '' ''    Me.DenominazioneConsulenzaTextBox.Text = String.Empty
    '' ''    Me.RagioneIncaricoConsulenzaTextBox.Text = String.Empty
    '' ''    Me.CompensoConsulenzaTextBox.Text = String.Empty
    '' ''    Me.VariabileCompensoConsulenzaTextBox.Text = String.Empty
    '' ''    Me.altreCaricheTextBox.Text = String.Empty
    '' ''    Me.altriIncarichiTextBox.Text = String.Empty
    '' ''    Me.altreAttivitaProfessionaliTextBox.Text = String.Empty
    '' ''    Me.DataInizioIncaricoConsulenzaTextBox.SelectedDate = Nothing
    '' ''    Me.DataFineIncaricoConsulenzaTextBox.SelectedDate = Nothing

    '' ''    CurriculumAllegatoLinkButton.Text = ""
    '' ''    NomeFileCurriculumLabel.Text = ""
    '' ''    curriculumUpload1.Visible = True
    '' ''    curriculumUpload2.Visible = False

    '' ''    InconsistenzaAllegatoLinkButton.Text = ""
    '' ''    NomeFileInsussistenzaLabel.Text = ""
    '' ''    InconsistenzaUpload1.Visible = True
    '' ''    InconsistenzaUpload2.Visible = False

    '' ''End Sub

    Public Function getElementoRubrica(ByVal idElemento As Long, ByVal denominazione As String) As ParsecAdmin.StrutturaEsternaInfo
        Dim rubrica As New ParsecAdmin.RubricaRepository
        Dim filtro As New ParsecAdmin.FiltroStrutturaEsternaInfo
        filtro.Annullati = True
        If (denominazione <> "") Then
            filtro.Denominazione = denominazione
        End If
        Dim elemento = rubrica.GetView(filtro).Where(Function(c) c.Id = idElemento).FirstOrDefault()
        Return elemento
    End Function

    'luca 10/07/2020
    '' ''Private Sub AggiornaVistaPannelliTrasparenza(trasparenza As ParsecAdmin.Pubblicazione)

    '' ''    Me.ResettaVistaPannelliTrasparenza()

    '' ''    Me.SezioneTrasparenzaTextBox.Text = trasparenza.SezioneTrasparente
    '' ''    Me.IdSezioneTrasparenzaTextBox.Text = trasparenza.IdSezione.ToString


    '' ''    Me.DataInizioPubblicazioneTextBox.SelectedDate = trasparenza.DataInizioPubblicazione
    '' ''    Me.DataFinePubblicazioneTextBox.SelectedDate = trasparenza.DataFinePubblicazione


    '' ''    Select Case trasparenza.TipologiaSezioneTrasparente
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''            For Each procedura In trasparenza.ProcedureAffidamento
    '' ''                Me.AttiConcessione.Add(procedura)
    '' ''            Next

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''            Dim incarico As ParsecAdmin.IncaricoDipendente = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''            DataInizioIncaricoDipendenteTextBox.SelectedDate = incarico.DataInizio
    '' ''            DataFineIncaricoDipendenteTextBox.SelectedDate = incarico.DataFine
    '' ''            BeneficiarioIncaricoComboBox.Text = incarico.Beneficiario
    '' ''            oggettoIncaricoDipendenteTextBox.Text = incarico.Oggetto
    '' ''            ragioneIncaricoDipendenteTextBox.Text = incarico.Ragione
    '' ''            compensoIncaricoDipendenteTextBox.Value = incarico.Compenso
    '' ''            If (incarico.IdBeneficiario Is Nothing) Then
    '' ''                Me.BeneficiarioIncaricoComboBox.SelectedValue = ""
    '' ''                BeneficiarioIncaricoComboBox.Text = incarico.Beneficiario
    '' ''            Else
    '' ''                Me.BeneficiarioIncaricoComboBox.SelectedValue = incarico.IdBeneficiario
    '' ''                Dim elemento = getElementoRubrica(incarico.IdBeneficiario, incarico.Beneficiario)
    '' ''                If (Not elemento Is Nothing) Then
    '' ''                    BeneficiarioIncaricoComboBox.Text = elemento.Denominazione & " " & If(Not String.IsNullOrEmpty(elemento.Nome), elemento.Nome & ", ", "") & If(Not String.IsNullOrEmpty(elemento.Indirizzo), elemento.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(elemento.Comune), elemento.Comune & " ", "") & If(Not String.IsNullOrEmpty(elemento.CAP), elemento.CAP & " ", "") & If(Not String.IsNullOrEmpty(elemento.Provincia), "(" & elemento.Provincia & ")", "")

    '' ''                End If
    '' ''            End If

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''            If Not trasparenza.ProcedureAffidamento(0) Is Nothing Then
    '' ''                Dim bandoGara As ParsecAdmin.BandoGara = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''                Me.OggettoBandoGaraTextBox.Text = bandoGara.Oggetto

    '' ''                If (bandoGara.Partecipanti <> "") Then
    '' ''                    Dim partecipante As String() = bandoGara.Partecipanti.Split(New Char() {";"})
    '' ''                    ' Use For Each loop over words and display them
    '' ''                    Dim word As String
    '' ''                    For Each word In partecipante
    '' ''                        Dim item As New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                        Me.PartecipantiListBox.Items.Add(item)
    '' ''                    Next
    '' ''                End If
    '' ''                If (bandoGara.Aggiudicatario <> "") Then
    '' ''                    Dim aggiudicatario As String() = bandoGara.Aggiudicatario.Split(New Char() {";"})
    '' ''                    ' Use For Each loop over words and display them
    '' ''                    Dim word As String
    '' ''                    For Each word In aggiudicatario
    '' ''                        Dim item As New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                        Me.AggiudicatariListBox.Items.Add(item)
    '' ''                    Next
    '' ''                End If

    '' ''                Me.CigBandoGaraTextBox.Text = bandoGara.Cig
    '' ''                Me.ImportoAggiudicazioneTextBox.Value = bandoGara.ImportoAggiudicazione
    '' ''                Me.ImportoLiquidatoTextBox.Value = bandoGara.ImportoLiquidato
    '' ''                Me.NumeroOfferentiTextBox.Value = bandoGara.NumeroOfferenti
    '' ''                Me.DataInizioLavoroTextBox.SelectedDate = bandoGara.DataInizioOpera
    '' ''                Me.DataFineLavoroTextBox.SelectedDate = bandoGara.DataFineOpera
    '' ''                Me.TipologiaSceltaComboBox.SelectedValue = 0
    '' ''                If (bandoGara.TipologiaSceltaContraente <> "") Then
    '' ''                    Dim crieterioSceltaR As New ParsecAdmin.CriterioSceltaContraenteRepository
    '' ''                    Dim criterio = crieterioSceltaR.GetKeyValue().Where(Function(c) c.Descrizione.ToUpper = bandoGara.TipologiaSceltaContraente.ToUpper).FirstOrDefault()
    '' ''                    If (Not criterio Is Nothing) Then
    '' ''                        Me.TipologiaSceltaComboBox.SelectedValue = criterio.Id
    '' ''                    End If
    '' ''                End If


    '' ''                If Not String.IsNullOrEmpty(bandoGara.StrutturaProponente) Then
    '' ''                    Me.DenominazioneStrutturaProponenteTextBox.Text = bandoGara.StrutturaProponente
    '' ''                End If

    '' ''                If Not String.IsNullOrEmpty(bandoGara.CodiceFiscaleStrutturaProponente) Then
    '' ''                    Me.CodiceFiscaleProponenteTextBox.Text = bandoGara.CodiceFiscaleStrutturaProponente
    '' ''                End If



    '' ''                Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                    Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(bandoGara.IdPubblicazione)
    '' ''                    pubblicazioni.Dispose()



    '' ''            End If

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''            Dim consulenza As ParsecAdmin.Consulenza = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''            Me.BeneficiarioConsulenzaComboBox.SelectedValue = consulenza.IdBeneficiario
    '' ''            Me.BeneficiarioConsulenzaComboBox.Text = consulenza.Beneficiario
    '' ''            Me.DenominazioneConsulenzaTextBox.Text = consulenza.Oggetto
    '' ''            Me.RagioneIncaricoConsulenzaTextBox.Text = consulenza.RagioneIncarico
    '' ''            Me.CompensoConsulenzaTextBox.Text = consulenza.Compenso
    '' ''            Me.VariabileCompensoConsulenzaTextBox.Text = consulenza.CompensoVariabile
    '' ''            Me.altreCaricheTextBox.Text = consulenza.AltreCariche
    '' ''            Me.altriIncarichiTextBox.Text = consulenza.DatiAltriIncarichi
    '' ''            Me.altreAttivitaProfessionaliTextBox.Text = consulenza.DatiAltreAttivitàProfessionali
    '' ''            Me.DataInizioIncaricoConsulenzaTextBox.SelectedDate = consulenza.DataInizioIncaricoConsulenza
    '' ''            Me.DataFineIncaricoConsulenzaTextBox.SelectedDate = consulenza.DataFineIncaricoConsulenza

    '' ''            If Not consulenza.UrlCv Is Nothing Then
    '' ''                If consulenza.UrlCv <> String.Empty Then
    '' ''                    CurriculumAllegatoLinkButton.Text = consulenza.UrlCv
    '' ''                    NomeFileCurriculumLabel.Text = consulenza.CurriculumTemp
    '' ''                    curriculumUpload1.Visible = False
    '' ''                    curriculumUpload2.Visible = True
    '' ''                Else
    '' ''                    CurriculumAllegatoLinkButton.Text = ""
    '' ''                    NomeFileCurriculumLabel.Text = ""
    '' ''                    curriculumUpload1.Visible = True
    '' ''                    curriculumUpload2.Visible = False
    '' ''                End If
    '' ''            End If

    '' ''            If Not consulenza.UrlAttestazioneInsussistenzaConflittoInteressi Is Nothing Then
    '' ''                If consulenza.UrlAttestazioneInsussistenzaConflittoInteressi <> String.Empty Then
    '' ''                    InconsistenzaAllegatoLinkButton.Text = consulenza.UrlAttestazioneInsussistenzaConflittoInteressi
    '' ''                    NomeFileInsussistenzaLabel.Text = consulenza.InsussistenzaTemp
    '' ''                    InconsistenzaUpload1.Visible = False
    '' ''                    InconsistenzaUpload2.Visible = True
    '' ''                Else
    '' ''                    InconsistenzaAllegatoLinkButton.Text = ""
    '' ''                    NomeFileInsussistenzaLabel.Text = ""
    '' ''                    InconsistenzaUpload1.Visible = True
    '' ''                    InconsistenzaUpload2.Visible = False
    '' ''                End If
    '' ''            End If

    '' ''    End Select
    '' ''End Sub

    'luca 10/07/2020
    '' ''Private Sub NascondiPannelliTrasparenza()
    '' ''    Me.PubblicazionePanel.Visible = False
    '' ''    Me.AttiConcessionePanel.Visible = False
    '' ''    Me.BandiGareContrattiPanel.Visible = False
    '' ''    Me.CompensoConsulenzaPanel.Visible = False
    '' ''    Me.IncaricoPanel.Visible = False


    '' ''    Me.AttiTabStrip.Tabs(6).Enabled = False
    '' ''End Sub

    'luca 10/07/2020
    '' ''Private Sub VisualizzaPannelliTrasparenza(tipologia As ParsecAdmin.TipologiaSezioneTrasparente)
    '' ''    'Me.NascondiPannelliTrasparenza()
    '' ''    Me.PubblicazionePanel.Visible = True
    '' ''    Me.AttiTabStrip.Tabs(6).Enabled = True

    '' ''    Select Case tipologia
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''            Me.AttiConcessionePanel.Visible = True
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''            Me.BandiGareContrattiPanel.Visible = True
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''            Me.CompensoConsulenzaPanel.Visible = True
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''            Me.IncaricoPanel.Visible = True
    '' ''    End Select
    '' ''End Sub

    '*************************************************************************************************************
    'Gestione visibilità e abilitazione controlli pagina in base alla tipologia di documento e al modello associato.
    '*************************************************************************************************************

    Private Sub ImpostaUi(documento As ParsecAtt.Documento)

        Dim contabilitaTab As RadTab = Me.AttiTabStrip.Tabs(2)
        contabilitaTab.Enabled = (documento.Modello.Liquidazione OrElse documento.Modello.ImpegnoSpesa OrElse documento.Modello.Accertamento)

        Me.ImpegniSpesaTable.Visible = documento.Modello.ImpegnoSpesa
        Me.LiquidazioniTable.Visible = documento.Modello.Liquidazione
        Me.AccertamentiTable.Visible = documento.Modello.Accertamento

        Dim proposta As Nullable(Of Boolean) = documento.Modello.Proposta
        Dim pubblicazione As Nullable(Of Boolean) = documento.Modello.Pubblicazione


        Dim classificazioneTab As RadTab = Me.AttiTabStrip.Tabs.FindTabByText("Classificazioni")
        Dim presenzeTab As RadTab = Me.AttiTabStrip.Tabs.FindTabByText("Presenze")

        'Dati Protocollo
        Dim protocolloVisibile As Boolean = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto)
        Me.ProtocolloPanel.Visible = protocolloVisibile

        'Dati pubblicazione
        Dim pubblicazioneVisibile As Boolean = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto)
        If pubblicazione.HasValue Then
            pubblicazioneVisibile = pubblicazioneVisibile AndAlso pubblicazione
        End If
        Me.AffissionePanel.Visible = pubblicazioneVisibile

        classificazioneTab.Enabled = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina)
        presenzeTab.Enabled = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDelibera)

    End Sub

    Private Sub DisabilitaUi()

        '***************************************************************************************
        'SCHEDA TRASPARENZA
        '***************************************************************************************
        'luca 10/07/2020
        '' ''Me.SezioneTrasparenzaTextBox.Enabled = False
        '' ''Me.TrovaSezioneImageButton.Visible = False
        '' ''Me.EliminaSezioneImageButton.Visible = False
        '' ''Me.AggiungiBeneficiarioImageButton.Visible = False
        '' ''Me.DataInizioPubblicazioneTextBox.Enabled = False
        '' ''Me.DataFinePubblicazioneTextBox.Enabled = False
        ' '' ''Atto concessione
        '' ''Me.RubricaComboBox.Visible = False
        '' ''Me.comboboxSottoSezione.Enabled = False
        '' ''Me.BeneficiariGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
        '' ''Me.BeneficiariGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        '' ''Me.BeneficiariGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False
        '' ''Me.BeneficiariGridView.MasterTableView.Columns.FindByUniqueName("Preview").Visible = True
        ' '' ''Bandi Gara
        '' ''Me.FiltroBandoGaraTextBox.Enabled = False
        '' ''Me.TrovaBandoGaraImageButton.Visible = False
        '' ''Me.OggettoBandoGaraTextBox.Enabled = False
        '' ''Me.PartecipanteComboBox.Visible = False
        '' ''Me.TrovaPartecipanteImageButton.Visible = False
        '' ''Me.TrovaRaggruppamentoImageButton.Visible = False
        '' ''Me.AggiungiPartecipanteImageButton.Visible = False
        '' ''Me.EliminaPartecipanteImageButton.Visible = False
        '' ''Me.EliminaAggiudicatarioImageButton.Visible = False
        '' ''Me.PartecipantiListBox.Enabled = False
        '' ''Me.AggiudicatariListBox.Enabled = False
        '' ''Me.AggiungiTuttoImageButton.Visible = False
        '' ''Me.AggiungiImageButton.Visible = False
        '' ''Me.CigBandoGaraTextBox.Enabled = False
        '' ''Me.ImportoAggiudicazioneTextBox.Enabled = False
        '' ''Me.ImportoLiquidatoTextBox.Enabled = False
        '' ''Me.NumeroOfferentiTextBox.Enabled = False
        '' ''Me.DataInizioLavoroTextBox.Enabled = False
        '' ''Me.DataFineLavoroTextBox.Enabled = False
        '' ''Me.TipologiaSceltaComboBox.Enabled = False
        ' '' ''Consulenza
        '' ''Me.BeneficiarioConsulenzaComboBox.Enabled = False
        '' ''Me.TrovaBeneficiarioConsulenzaImageButton.Visible = False
        '' ''Me.DenominazioneConsulenzaTextBox.Enabled = False
        '' ''Me.RagioneIncaricoConsulenzaTextBox.Enabled = False
        '' ''Me.CompensoConsulenzaTextBox.Enabled = False
        '' ''Me.VariabileCompensoConsulenzaTextBox.Enabled = False
        '' ''Me.altreCaricheTextBox.Enabled = False
        '' ''Me.altriIncarichiTextBox.Enabled = False
        '' ''Me.altreAttivitaProfessionaliTextBox.Enabled = False
        '' ''Me.DataInizioIncaricoConsulenzaTextBox.Enabled = False
        '' ''DataFineIncaricoConsulenzaTextBox.Enabled = False

        '' ''CurriculumAllegatoLinkButton.Text = ""
        '' ''NomeFileCurriculumLabel.Text = ""
        '' ''curriculumUpload1.Visible = False
        '' ''curriculumUpload2.Visible = True

        '' ''InconsistenzaAllegatoLinkButton.Text = ""
        '' ''NomeFileInsussistenzaLabel.Text = ""
        '' ''InconsistenzaUpload1.Visible = False
        '' ''InconsistenzaUpload2.Visible = True
        ' '' ''Incarichi dipendenti
        '' ''Me.BeneficiarioIncaricoComboBox.Enabled = False
        '' ''Me.TrovaBeneficiarioIncaricoDipendenteImageButton.Visible = False
        '' ''Me.oggettoIncaricoDipendenteTextBox.Enabled = False
        '' ''Me.ragioneIncaricoDipendenteTextBox.Enabled = False
        '' ''Me.compensoIncaricoDipendenteTextBox.Enabled = False
        '' ''Me.DataInizioIncaricoDipendenteTextBox.Enabled = False
        '' ''Me.DataFineIncaricoDipendenteTextBox.Enabled = False
        '***************************************************************************************


        '***************************************************************************************
        'SCHEDA ALLEGATI
        '***************************************************************************************
        Me.AllegatiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.DescrizioneDocumentoTextBox.Enabled = False
        Me.AllegatoUpload.Enabled = False
        Me.FronteRetroCheckBox.Enabled = False
        Me.VisualizzaUICheckBox.Enabled = False
        Me.ScansionaImageButton.Visible = False
        Me.AggiungiDocumentoImageButton.Visible = False
        Me.TipologiaAllegatoComboBox.Enabled = False

        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA CLASSIFICAZIONI
        '***************************************************************************************
        Me.ClassificazioniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.AnnotazioniTextBox.Enabled = False
        Me.ClassificazioneTextBox.Enabled = False
        Me.TrovaClassificazioneImageButton.Visible = False
        Me.AggiungiClassificazioneImageButton.Visible = False
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA VISIBILITA'
        '***************************************************************************************
        Me.VisibilitaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.TrovaGruppoVisibilitaImageButton.Visible = False
        Me.TrovaUtenteVisibilitaImageButton.Visible = False
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA CONTABILITA
        '***************************************************************************************
        Me.AggiungiImpegnoSpesaImageButton.Visible = False
        Me.AggiungiLiquidazioneImageButton.Visible = False
        Me.AggiungiAccertamentoImageButton.Visible = False
        Me.ImpegniSpesaGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
        Me.ImpegniSpesaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.ImpegniSpesaGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False

        Me.LiquidazioniGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
        Me.LiquidazioniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.LiquidazioniGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False
        Me.LiquidazioniGridView.MasterTableView.Columns.FindByUniqueName("Preview").Visible = True
        Me.AccertamentiGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
        Me.AccertamentiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.AccertamentiGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA PRESENZE
        '***************************************************************************************
        Me.EsecutivitaImmediataCheckBox.Enabled = False
        Me.GiorniEsecutivitaTextBox.Enabled = False
        Me.TipiApprovazioneComboBox.Enabled = False
        Me.SedutaTextBox.Enabled = False
        Me.TrovaSedutaImageButton.Visible = False
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA GENERALE
        '***************************************************************************************
        Me.TipologieDocumentoComboBox.Enabled = False
        Me.ModelliComboBox.Enabled = False
        Me.NumeroSettoreTextBox.Enabled = False
        Me.NumeroAttoTextBox.Enabled = False
        Me.DataTextBox.Enabled = False
        Me.UfficioTextBox.Enabled = False
        Me.TrovaUfficioImageButton.Visible = False
        Me.EliminaUfficioImageButton.Visible = False
        Me.SettoreTextBox.Enabled = False
        Me.OggettoTextBox.Enabled = False
        Me.DelimitaTestoImageButton.Visible = False
        Me.NoteTextBox.Enabled = False

        'Pannello Affissione
        Me.DataAffissioneTextBox.Enabled = False
        Me.GiorniAffissioneTextBox.Enabled = False


        Me.AffissionePanel.Visible = False

        Me.NumeroProtocolloTextBox.Enabled = False
        Me.DataProtocolloTextBox.Enabled = False
        Me.TrovaProtocolloImageButton.Visible = False
        Me.EliminaProtocolloImageButton.Visible = False
        Me.ProtocolloPanel.Visible = False

        Me.BozzaTextBox.Enabled = False
        Me.TrovaBozzaImageButton.Visible = False
        Me.EliminaBozzaImageButton.Visible = False
        Me.BozzaPanel.Visible = False
        Me.NumeroSettoreTable.Visible = False

        If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Then
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim numerazioneAttivata As Boolean = False
            Dim parametroMomentoNumerazioneSettore As ParsecAdmin.Parametri = Parametri.GetByName("MomentoNumerazioneSettore", ParsecAdmin.TipoModulo.ATT)
            If Not parametroMomentoNumerazioneSettore Is Nothing Then
                numerazioneAttivata = (parametroMomentoNumerazioneSettore.Valore <> "0")
            End If
            Me.NumeroSettoreTable.Visible = numerazioneAttivata
            parametri.Dispose()
        End If


        '***************************************************************************************

        '***************************************************************************************
        'VISTI E PARERI
        '***************************************************************************************
        Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
        '***************************************************************************************

        '******************************************************************************************
        'SCHEDA FASCICOLI
        '******************************************************************************************

        Me.TrovaFascicoloImageButton.Visible = False
        Me.NuovoFascicoloImageButton.Visible = False
        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
        Me.FaseDocumentoFascicoloLabel.Visible = False
        Me.FaseDocumentoFascicoloComboBox.Visible = False
        '******************************************************************************************

        'Annullo la pressione dei tasti per evitare che la pressione del tasto backspace faccia tornare alla pagina precedente 

        'Me.NumeroAttoTextBox.ReadOnly = True
        'Me.NumeroAttoTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")


        'Me.BozzaTextBox.ReadOnly = True
        'Me.BozzaTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")

        'Me.SedutaTextBox.ReadOnly = True
        'Me.SedutaTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")

        'Me.UfficioTextBox.ReadOnly = True
        'Me.UfficioTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")

        'Me.SettoreTextBox.ReadOnly = True
        'Me.SettoreTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")

        'Me.ClassificazioneTextBox.ReadOnly = True
        'Me.ClassificazioneTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")


        ''Disabilito il pannello dati affissione
        'Me.DataAffissioneTextBox.DatePopupButton.Enabled = False
        'Me.DataAffissioneTextBox.DateInput.ReadOnly = True
        'Me.DataAffissioneTextBox.DateInput.Attributes.Add("onkeydown", "event.returnValue=false;")
        'Me.GiorniAffissioneTextBox.ReadOnly = True
        'Me.GiorniAffissioneTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")
        'Me.NumeroRegistroPubblicazioneTextBox.ReadOnly = True
        'Me.NumeroRegistroPubblicazioneTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")

        ''Disabilito il pannello dati protocollo
        'Me.NumeroProtocolloTextBox.ReadOnly = True
        'Me.NumeroProtocolloTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")
        'Me.DataProtocolloTextBox.ReadOnly = True
        'Me.DataProtocolloTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")
        'Me.TrovaProtocolloImageButton.Visible = False
        'Me.EliminaProtocolloImageButton.Visible = False


        ''Disabilito il pannello dati esecutività
        'Me.GiorniEsecutivitaTextBox.ReadOnly = True
        'Me.GiorniEsecutivitaTextBox.Attributes.Add("onkeydown", "event.returnValue=false;")

        '***************************************************************************************
        'SCHEDA ALLEGATI
        '***************************************************************************************
        'luca 10/07/2020
        '' ''Me.AllegatiBandoGaraGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
        '' ''Me.DescrizioneDocumentoBandoGaraTextBox.Enabled = False
        '' ''Me.AllegatoBandoGaraUpload.Enabled = False
        '' ''Me.FronteRetroBandoGaraCheckBox.Enabled = False
        '' ''Me.VisualizzaUIBandoGaraCheckBox.Enabled = False
        '' ''Me.ScansionaBandoGaraImageButton.Visible = False
        '' ''Me.AggiungiDocumentoBandoGaraImageButton.Visible = False
        '' ''Me.DocumentoAllegatoBandoGaraRadioButton.Enabled = False
        '' ''Me.DocumentoPrimarioBandoGaraRadioButton.Enabled = False

        '***************************************************************************************

    End Sub

    'luca 10/07/2020
    '' ''Protected Sub AllegatiBandoGaraGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiBandoGaraGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''         Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub

    'luca 10/07/2020
    '' ''Private Sub DownloadAllegatoPubblicazione(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    '********************************************************************************
    '' ''    'Modificata per utilizzo GridTemplateColumn
    '' ''    'Dim selectedItem As GridDataItem = item.OwnerTableView.Items(item.ItemIndex)
    '' ''    'Dim filename As String = selectedItem("Nomefile").Text
    '' ''    '********************************************************************************
    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")


    '' ''    Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

    '' ''    Dim allegato As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.Nomefile = filename).FirstOrDefault
    '' ''    If Not allegato Is Nothing Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If allegato.Id < 0 Then
    '' ''            pathDownload = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegato.Nomefile
    '' ''        Else
    '' ''            pathDownload = percorsoRoot & allegato.PathRelativo & allegato.Nomefile
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    Private Sub AggiornaGrigliaLiquidazioni()
        Me.LiquidazioniGridView.DataSource = Me.Liquidazioni
        Me.LiquidazioniGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaImpegniSpesa()
        Me.ImpegniSpesaGridView.DataSource = Me.ImpegniSpesa
        Me.ImpegniSpesaGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaAccertamenti()
        Me.AccertamentiGridView.DataSource = Me.Accertamenti
        Me.AccertamentiGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaClassificazioni()
        Me.ClassificazioniGridView.DataSource = Me.Classificazioni
        Me.ClassificazioniGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaPresenze()
        Me.PresenzeGridView.DataSource = Me.Presenze
        Me.PresenzeGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaFirme()
        Me.FirmeGridView.DataSource = Me.Firme
        Me.FirmeGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaAllegati()
        Me.AllegatiGridView.DataSource = Me.Allegati
        Me.AllegatiGridView.DataBind()
    End Sub

    'luca 10/07/2020
    '' ''Private Sub AggiornaGrigliaAllegatiPubblicazione()
    '' ''    Me.AllegatiBandoGaraGridView.DataSource = Me.AllegatiPubblicazione
    '' ''    Me.AllegatiBandoGaraGridView.DataBind()
    '' ''End Sub

    Private Sub AggiornaGrigliaFascicoli()
        Me.FascicoliGridView.DataSource = Me.Fascicoli
        Me.FascicoliGridView.DataBind()
    End Sub

    Private Sub AggiornaGriglia()
        Me.Documenti = Nothing
        Me.DocumentiGridView.Rebind()
    End Sub

    Private Sub AggiornaGrigliaVisibilita()
        Me.VisibilitaGridView.DataSource = Me.Visibilita
        Me.VisibilitaGridView.DataBind()
    End Sub

#End Region

#Region "GESTIONE NAVIGAZIONE"

    Protected Sub VaiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles VaiImageButton.Click
        If Not String.IsNullOrEmpty(Me.PositionItemTextBox.Text) Then
            Dim position As Integer
            If UInt32.TryParse(Me.PositionItemTextBox.Text, position) Then
                If position <= Me.Documenti.Count Then
                    Me.CurrentPosition = position - 1
                    Me.ScorriRegistrazioni(Me.CurrentPosition)
                    Me.SetButtonState()
                End If
            End If
        End If
        Me.PositionItemTextBox.Text = (Me.CurrentPosition + 1).ToString
    End Sub

    Protected Sub PrimoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PrimoImageButton.Click
        Me.CurrentPosition = 0
        Me.ScorriRegistrazioni(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    Protected Sub PrecedenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PrecedenteImageButton.Click
        Me.CurrentPosition -= 1
        Me.ScorriRegistrazioni(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    Protected Sub UltimoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles UltimoImageButton.Click
        Me.CurrentPosition = Me.Documenti.Count - 1
        Me.ScorriRegistrazioni(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    Protected Sub SuccessivoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SuccessivoImageButton.Click
        Me.CurrentPosition += 1
        Me.ScorriRegistrazioni(Me.CurrentPosition)
        Me.SetButtonState()
    End Sub

    Private Sub ScorriRegistrazioni(ByVal position As Integer)
        Me.PositionItemTextBox.Text = (position + 1).ToString
        Dim id As Integer = Me.Documenti(Me.CurrentPosition).Id
        Me.IdDocumentoSelezionato = id
        Me.AggiornaVista(id)
      End Sub

    Private Sub SetButtonState()
        Dim enableForward As Boolean = (Me.CurrentPosition < Me.Documenti.Count - 1)
        Dim enableBack As Boolean = Me.CurrentPosition > 0
        Dim enableGoto As Boolean = Me.Documenti.Count > 1

        Me.UltimoImageButton.Enabled = enableForward
        Me.SuccessivoImageButton.Enabled = enableForward
        Me.PrimoImageButton.Enabled = enableBack
        Me.PrecedenteImageButton.Enabled = enableBack
        Me.UltimoImageButton.ImageUrl = "~\images\" & If(enableForward, "Last", "LastDisabled") & ".png"
        Me.SuccessivoImageButton.ImageUrl = "~\images\" & If(enableForward, "Next", "NextDisabled") & ".png"
        Me.PrecedenteImageButton.ImageUrl = "~\images\" & If(enableBack, "Previous", "PreviousDisabled") & ".png"
        Me.PrimoImageButton.ImageUrl = "~\images\" & If(enableBack, "First", "FirstDisabled") & ".png"

        Me.PositionItemTextBox.Enabled = enableGoto
        Me.VaiImageButton.ImageUrl = "~\images\" & If(enableGoto, "Goto", "GotoDisabled") & ".png"
        Me.VaiImageButton.Enabled = enableGoto
    End Sub

    Private Sub SetButtonImage()

        Me.PrimoImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\FirstSelected.png") & "'")
        Me.PrimoImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\First.png") & "'")

        Me.PrecedenteImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\PreviousSelected.png") & "'")
        Me.PrecedenteImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\Previous.png") & "'")

        Me.SuccessivoImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\NextSelected.png") & "'")
        Me.SuccessivoImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\Next.png") & "'")

        Me.UltimoImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\LastSelected.png") & "'")
        Me.UltimoImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\Last.png") & "'")

        Me.VaiImageButton.Attributes.Add("onMouseOver", "this.src='" & Me.ResolveClientUrl("~\images\GotoSelected.png") & "'")
        Me.VaiImageButton.Attributes.Add("onMouseOut", "this.src='" & Me.ResolveClientUrl("~\images\Goto.png") & "'")
    End Sub

#End Region

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        If Me.DettaglioPanel.Visible = True Then
            Me.AggiornaGrigliaLiquidazioni()
            Me.AggiornaGrigliaImpegniSpesa()
            Me.AggiornaGrigliaAccertamenti()
            Me.AggiornaGrigliaClassificazioni()
            Me.AggiornaGrigliaPresenze()
            Me.AggiornaGrigliaFirme()
            Me.AggiornaGrigliaAllegati()
            Me.AggiornaGrigliaVisibilita()
            Me.AggiornaGrigliaFascicoli()
            'luca 10/07/2020
            '' ''Me.AggiornaGrigliaAttiConcessione()
            '' ''Me.AggiornaGrigliaAllegatiPubblicazione()
            '' ''Me.BandoGaraTabStrip.Tabs(1).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        End If


    End Sub
End Class
