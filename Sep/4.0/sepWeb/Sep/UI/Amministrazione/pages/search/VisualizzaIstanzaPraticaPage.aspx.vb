Partial Class VisualizzaIstanzaPraticaPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA' PAGINA"

    Public Property IstanzaPraticaOnline As ParsecAdmin.IstanzaPraticaOnline
        Get
            Return CType(Session("VisualizzaIstanzaPraticaPage_IstanzaPraticaOnline"), ParsecAdmin.IstanzaPraticaOnline)
        End Get
        Set(ByVal value As ParsecAdmin.IstanzaPraticaOnline)
            Session("VisualizzaIstanzaPraticaPage_IstanzaPraticaOnline") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then
            Dim id As Nullable(Of Integer) = Nothing
            If Not Me.Page.Request.QueryString("IdIstanzaPraticaOnline") Is Nothing Then
                id = CInt(Me.Page.Request.QueryString("IdIstanzaPraticaOnline"))
            End If

            If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
                Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
                If parametriPagina.ContainsKey("Filtro") Then
                    id = parametriPagina("Filtro")
                End If
                ParsecUtility.SessionManager.ParametriPagina = Nothing
            End If

            If id.HasValue Then
                Dim istanzePratica As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Me.IstanzaPraticaOnline = istanzePratica.GetFullById(id)
                Me.AggiornaVista(Me.IstanzaPraticaOnline)
                istanzePratica.Dispose()
            End If


        End If

        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        ParsecUtility.Utility.CloseWindow(False)
        '***************************************************************************

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If


        Me.GrigliaAllegatiPanel.Style.Add("width", widthStyle)
        Me.GrigliaDocumentiPanel.Style.Add("width", widthStyle)

        Me.AllegatiGridView.Style.Add("width", widthStyle)
        Me.DocumentiGridView.Style.Add("width", widthStyle)
        Me.DatiGeneraliPanel.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        Me.AllegatiLabel.Text = "Allegati&nbsp;&nbsp;" & If(Me.AllegatiGridView.MasterTableView.Items.Count > 0, "( " & Me.AllegatiGridView.MasterTableView.Items.Count.ToString & " )", "")
        Me.DocumentiLabel.Text = "Documenti&nbsp;&nbsp;" & If(Me.DocumentiGridView.MasterTableView.Items.Count > 0, "( " & Me.DocumentiGridView.MasterTableView.Items.Count.ToString & " )", "")
    End Sub


#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ChiudiFinestraButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiFinestraButton.Click
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub

#End Region

#Region "PRIVATI"

    Private Sub AggiornaVista(ByVal istanza As ParsecAdmin.IstanzaPraticaOnline)
        If Not istanza Is Nothing Then
            Me.NumeroPraticaTextBox.Text = istanza.NumeroPratica

            Select Case CType(istanza.IdStato, ParsecAdmin.StatoIstanzaPraticaOnline)
                Case ParsecAdmin.StatoIstanzaPraticaOnline.Protocollata
                    Me.StatoPraticaTextBox.Text = "Protocollata"
                Case ParsecAdmin.StatoIstanzaPraticaOnline.Avviata, ParsecAdmin.StatoIstanzaPraticaOnline.PreparazioneRichiestaIntegrazione
                    Me.StatoPraticaTextBox.Text = "In Lavorazione"
                Case ParsecAdmin.StatoIstanzaPraticaOnline.AttesaRichiestaIntegrazione
                    Me.StatoPraticaTextBox.Text = "Sospesa"
                Case ParsecAdmin.StatoIstanzaPraticaOnline.Chiusa
                    Me.StatoPraticaTextBox.Text = "Chiusa"
                Case ParsecAdmin.StatoIstanzaPraticaOnline.ComunicatoResponsabileProcedimento
                    Me.StatoPraticaTextBox.Text = "Comunicato Responsabile Procedimento"
                Case ParsecAdmin.StatoIstanzaPraticaOnline.IntegrazioneDocumentiPreRilascio
                    Me.StatoPraticaTextBox.Text = "Integrazione Documenti PreRilascio"
                Case ParsecAdmin.StatoIstanzaPraticaOnline.RichiestaPareri
                    Me.StatoPraticaTextBox.Text = "Richiesta Pareri"
            End Select

            Me.DataPresentazioneTextBox.Text = istanza.DataPresentazione.ToShortDateString
            Me.TerminePrevistoTextBox.Text = istanza.DataPresentazione.AddDays(istanza.TermineProcedimento).ToShortDateString


            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione = registrazioni.Where(Function(c) c.Id = IstanzaPraticaOnline.IdRegistrazione).FirstOrDefault
            Me.ProtocolloLabel.Text = "Prot. N° " & registrazione.NumeroProtocollo.ToString.PadLeft(7, "0") & "/" & registrazioni.GetDescrizioneTipoRegistrazione(registrazione.TipoRegistrazione).ToUpper.Chars(0) & " del " & String.Format("{0:dd/MM/yyyy}", registrazione.DataImmissione)
            registrazioni.Dispose()




            If Not istanza.Procedimento Is Nothing Then
                Me.ProcedimentoTextBox.Text = istanza.Procedimento.Nome
                If Not istanza.ResponsabileProcedimento Is Nothing Then
                    Me.ResponsabileProcedimentoTextBox.Text = istanza.ResponsabileProcedimento.Descrizione
                End If

            End If


            If Not istanza.Richiedente Is Nothing Then
                Me.DenominazioneCognomeRichiedenteTextBox.Text = istanza.Richiedente.Denominazione
                Me.NomeRichiedenteTextBox.Text = istanza.Richiedente.Nome
                Me.CodiceFiscaleRichiedenteTextBox.Text = istanza.Richiedente.CodiceFiscale
                Me.ComuneResidenzaRichiedenteTextBox.Text = istanza.Richiedente.Comune
                Me.CapResidenzaRichiedenteTextBox.Text = istanza.Richiedente.CAP
                Me.ProvinciaResidenzaRichiedenteTextBox.Text = istanza.Richiedente.Provincia
                Me.IndirizzoResidenzaRichiedenteTextBox.Text = istanza.Richiedente.Indirizzo
                Me.IndirizzoPecRichiedenteTextBox.Text = istanza.Richiedente.Email
                If Not istanza.Committente Is Nothing Then
                    If istanza.Committente.Id = istanza.Richiedente.Id Then
                        Me.MessaggioLabel.Text = "Il richiedente e il committente corrispondono"
                        Me.CommittentePanel.Visible = False
                    Else
                        Me.CommittentePanel.Visible = True

                        Me.DenominazioneCognomeCommittenteTextBox.Text = istanza.Committente.Denominazione
                        Me.NomeCommittenteTextBox.Text = istanza.Committente.Nome
                        Me.CodiceFiscaleCommittenteTextBox.Text = istanza.Committente.CodiceFiscale
                        Me.ComuneResidenzaCommittenteTextBox.Text = istanza.Committente.Comune
                        Me.CapResidenzaCommittenteTextBox.Text = istanza.Committente.CAP
                        Me.ProvinciaResidenzaCommittenteTextBox.Text = istanza.Committente.Provincia
                        Me.IndirizzoResidenzaCommittenteTextBox.Text = istanza.Committente.Indirizzo
                        Me.IndirizzoPecCommittenteTextBox.Text = istanza.Committente.Email

                    End If
                Else
                    Me.CommittentePanel.Visible = False
                End If
            End If



        End If
    End Sub

    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

        Dim allegato As ParsecAdmin.AllegatoIstanzaPraticaOnline = Me.IstanzaPraticaOnline.Allegati.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then
            Dim pathDownload As String = String.Empty

            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
            pathDownload = percorsoRoot & allegato.PercorsoRelativo & allegato.IdAllegato.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.Nomefile

            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        End If
    End Sub

    Private Sub VisualizzaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idDocumento As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        'Dim documentiFascicolo As New ParsecAdmin.FascicoloDocumentoRepository
        Dim documento As ParsecAdmin.FascicoloDocumento = Me.IstanzaPraticaOnline.Documenti.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        Select Case documento.TipoDocumento
            Case 1 'Documento generico
                Me.VisualizzaDocumentoGenerico(documento)
            Case ParsecAdmin.TipoModulo.PRO
                Me.VisualizzaRegistrazione(documento)
            Case ParsecAdmin.TipoModulo.ATT
                Me.VisualizzaAtto(documento.IdDocumento)

        End Select
    End Sub

    Private Sub VisualizzaAtto(ByVal idDocumento As Integer)
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        documenti.Dispose()
        If Not documento Is Nothing Then
            Dim queryString As New Hashtable
            queryString.Add("Tipo", documento.IdTipologiaDocumento)
            queryString.Add("Mode", "View")
            queryString.Add("Procedura", "10")
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AttoAmministrativoPage.aspx"
            Dim parametriPagina As New Hashtable
            parametriPagina.Add("IdDocumentoIter", idDocumento)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 650, queryString, False)
        Else
            ParsecUtility.Utility.MessageBox("Il documento selezionato non esiste!", False)
        End If
    End Sub


    Private Sub VisualizzaRegistrazione(ByVal documento As ParsecAdmin.FascicoloDocumento)
        If Not documento Is Nothing Then
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetQuery.Where(Function(c) c.Id = documento.IdDocumento).FirstOrDefault
            registrazioni.Dispose()
            If Not registrazione Is Nothing Then
                Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
                Dim queryString As New Hashtable
                queryString.Add("filtro", registrazione.Id)
                Dim parametriPagina As New Hashtable
                parametriPagina.Add("Filtro", registrazione.Id)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 510, queryString, False)
            Else
                ParsecUtility.Utility.MessageBox("La registrazione selezionata non esiste!", False)
            End If
        End If
    End Sub

    Private Sub VisualizzaDocumentoGenerico(ByVal documento As ParsecAdmin.FascicoloDocumento)
        If Not documento Is Nothing Then
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If documento.Id < 0 Then
                pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDownload") & documento.NomeFileTemp
            Else
                pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti") & documento.path & documento.NomeDocumento
            End If
            Dim file As New IO.FileInfo(pathDownload)

            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("Il documento selezionato non esiste!", False)
            End If
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub AllegatiGridView_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles AllegatiGridView.NeedDataSource
        Me.AllegatiGridView.DataSource = Me.IstanzaPraticaOnline.Allegati
    End Sub

    Protected Sub AllegatiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.DownloadFile(e.Item)
        End If
    End Sub


    Protected Sub DocumentiGridView_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DocumentiGridView.NeedDataSource
        Me.DocumentiGridView.DataSource = Me.IstanzaPraticaOnline.Documenti
    End Sub

    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        If e.CommandName = "Preview" Then
            Me.VisualizzaDocumento(e.Item)
        End If
    End Sub



#End Region

End Class