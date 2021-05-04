Imports Telerik.Web.UI


Partial Class AttoConcessionePage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    Public Property AttoConcessione() As ParsecAdmin.AttoConcessione
        Get
            Return CType(Session("AttoConcessionePage_AttoConcessione"), ParsecAdmin.AttoConcessione)
        End Get
        Set(ByVal value As ParsecAdmin.AttoConcessione)
            Session("AttoConcessionePage_AttoConcessione") = value
        End Set
    End Property

#End Region


#Region "EVENTI PAGINA"


    'todo ricerca impegno di spesa

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then

            If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
                Me.AttoConcessione = Nothing
                Me.GetParametri()
            End If


            'CaricaModalita()

            Dim mode As String = String.Empty
            If Not Me.Request.QueryString("Mode") Is Nothing Then
                mode = Me.Request.QueryString("Mode")
            End If

            Me.ResettaVista()
            Me.AggiornaVista()

            Select Case mode
                Case "Nuovo", "Copia"
                    Me.TitleLabel.Text = "Nuovo Atto di Concessione"
                Case "Modifica"
                    Me.TitleLabel.Text = "Modifica Atto di Concessione"
                Case "Preview"
                    Me.TitleLabel.Text = "Dettaglio Atto di Concessione"
                    DisabilitaUI()
            End Select
        End If

        Me.DisabilitaPulsantePredefinito.Attributes.Add("onclick", "return false;")
    End Sub


#End Region



#Region "METODI PRIVATI"

    'Private Sub CaricaModalita()
    '    Dim tipologie As New ParsecAdmin.CriterioSceltaContraenteRepository
    '    Me.ModalitaComboBox.DataValueField = "Id"
    '    Me.ModalitaComboBox.DataTextField = "Descrizione"
    '    Me.ModalitaComboBox.DataSource = tipologie.GetKeyValue()
    '    Me.ModalitaComboBox.DataBind()
    '    Me.ModalitaComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
    '    Me.ModalitaComboBox.SelectedIndex = 0
    '    tipologie.Dispose()
    'End Sub

    Private Sub GetParametri()
        Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
        If parametriPagina.ContainsKey("AttoConcessione") Then
            Me.AttoConcessione = parametriPagina("AttoConcessione")
        End If
        ParsecUtility.SessionManager.ParametriPagina = Nothing
    End Sub

    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder
        Dim messageErroreFormale As New StringBuilder

        'If String.IsNullOrEmpty(Me.ImportoTextBox.Text) Then
        '    message.AppendLine("L'importo impegnato.")
        'End If

        If String.IsNullOrEmpty(Me.NormaTextBox.Text) Then
            message.AppendLine("Norma/titolo alla base dell'atto di concessione.")
        End If

        'Nessuna modalità selezionata
        'If Not CBool(ModalitaComboBox.SelectedIndex) Then
        If String.IsNullOrEmpty(ModalitaTextBox.Text) Then
            message.AppendLine("Modalità di scelta del beneficiario")
        End If

        If String.IsNullOrEmpty(Me.ImportoAttoConcessioneTextBox.Text) Then
            message.AppendLine("Importo dell'atto di concessione.")
        End If

        'Beneficiario
        If String.IsNullOrEmpty(Me.RubricaComboBox.Text) Then
            message.AppendLine("Beneficiario dell'atto di concessione.")
        End If


        If Not String.IsNullOrEmpty(Me.CodiceFiscalePartitaIvaTextBox.Text) Then
            Dim rgxPartitaIva As Regex = New Regex("^[0-9]{11}$")
            'Dim rgxCodiceFiscale As Regex = New Regex("^[A-Z]{6}[\d+]{2}[ABCDEHLMPRST]{1}[\d+]{2}([A-Z]{1}[\d+]{3})[A-Z]{1}$")
            Dim rgxCodiceFiscale As Regex = New Regex("^[A-Z]{6}[A-Z0-9]{2}[A-Z][A-Z0-9]{2}[A-Z][A-Z0-9]{3}[A-Z]$")

            Dim matchCodiceFiscale As Match = rgxCodiceFiscale.Match(Me.CodiceFiscalePartitaIvaTextBox.Text.ToUpper())
            Dim matchPartitaIva As Match = rgxPartitaIva.Match(Me.CodiceFiscalePartitaIvaTextBox.Text)

            If Not matchCodiceFiscale.Success AndAlso Not matchPartitaIva.Success Then
                messageErroreFormale.AppendLine("Il dato fiscale (codice fiscale/partita iva).")
            End If
        Else
            message.AppendLine("Codice fiscale/partita iva del beneficiario.")
        End If


        'CODICE IDENTIFICATIVO GARA
        'If Not String.IsNullOrEmpty(Me.CigTextBox.Text) Then
        '    Dim rgxCIG As Regex = New Regex("^[a-zA-Z0-9]{10}$")
        '    Dim matchCIG As Match = rgxCIG.Match(Me.CigTextBox.Text)
        '    If Not matchCIG.Success Then
        '        messageErroreFormale.AppendLine("CIG.")
        '    End If
        'End If


        'CODICE UNICO di PROGETTO
        'If Not String.IsNullOrEmpty(Me.CupTextBox.Text) Then
        '    Dim rgxCUP As Regex = New Regex("^[a-zA-Z0-9]{15}$")
        '    Dim matchCUP As Match = rgxCUP.Match(Me.CupTextBox.Text)
        '    If Not matchCUP.Success Then
        '        messageErroreFormale.AppendLine("CUP.")
        '    End If
        'End If


        'CODICE IDENTIFICATIVO PRATICA (PRESENTE NEL DOCUMENTO DURC)
        'If Not String.IsNullOrEmpty(Me.DurcTextBox.Text) Then
        '    Dim rgxCIP As Regex = New Regex("^[0-9]{14}$")
        '    Dim matchCIP As Match = rgxCIP.Match(Me.DurcTextBox.Text)
        '    If Not matchCIP.Success Then
        '        messageErroreFormale.AppendLine("Codice identificativo pratica del DURC.")
        '    End If
        'End If



        Dim m As New StringBuilder

        If message.Length > 0 Then
            message.Insert(0, "I seguenti campi sono obbligatori:" & vbCrLf)
            m.Append(message.ToString)
        End If

        If messageErroreFormale.Length > 0 Then
            m.AppendLine("")
            m.AppendLine("I sequenti campi non sono formalmente corretti:")
            m.Append(messageErroreFormale.ToString)
        End If
        If m.Length > 0 Then
            ParsecUtility.Utility.MessageBox(m.ToString, False)
        End If


        Return m.Length = 0
    End Function

    Private Sub ResettaVista()

        '******************************************************************************************
        'Dati atto di concessione
        '******************************************************************************************

        NormaTextBox.Text = String.Empty
        'ModalitaComboBox.SelectedIndex = 0
        ModalitaTextBox.Text = String.Empty
        ImportoAttoConcessioneTextBox.Text = String.Empty

        '******************************************************************************************
        'Dati beneficiario
        '******************************************************************************************

        RubricaComboBox.Text = String.Empty
        Me.RubricaComboBox.SelectedValue = String.Empty
        CodiceFiscalePartitaIvaTextBox.Text = String.Empty

    End Sub

    Private Sub DisabilitaUI()

        '******************************************************************************************
        'Dati atto di concessione
        '******************************************************************************************

        NormaTextBox.Enabled = False
        'ModalitaComboBox.Enabled = False
        ModalitaTextBox.Enabled = False
        ImportoAttoConcessioneTextBox.Enabled = False
        ProgettoUpload.Enabled = False
        RimuoviProgettoImageButton.Visible = False

        '******************************************************************************************
        'Dati beneficiario
        '******************************************************************************************

        RubricaComboBox.Enabled = False
        TrovaBeneficiarioImageButton.Visible = False
        CodiceFiscalePartitaIvaTextBox.Enabled = False
        CurriculumUpload.Enabled = False
        RimuoviCurriculumImageButton.Visible = False

        SalvaButton.Visible = False
        AnnullaButton.Visible = False
    End Sub

    Private Sub AggiornaVista()

        '******************************************************************************************
        'Dati atto di concessione
        '******************************************************************************************

        Me.NormaTextBox.Text = AttoConcessione.TitoloNorma

        'If Not Me.ModalitaComboBox.Items.FindItemByText(AttoConcessione.Modalita) Is Nothing Then
        '    Me.ModalitaComboBox.Items.FindItemByText(AttoConcessione.Modalita).Selected = True
        'End If

        Me.ModalitaTextBox.Text = AttoConcessione.Modalita


        Me.ImportoAttoConcessioneTextBox.Value = AttoConcessione.Importo

        'SE E' UN NUOVO ATTO DI CONCESSIONE E LIMPORTO 
        If Me.AttoConcessione.Id <= 0 Then
            If AttoConcessione.Importo = 0D Then
                Me.ImportoAttoConcessioneTextBox.Text = String.Empty
            End If
        End If

        If Not AttoConcessione.UrlProgetto Is Nothing Then
            If AttoConcessione.UrlProgetto <> String.Empty Then
                ProgettoAllegatoLinkButton.Text = AttoConcessione.UrlProgetto
                NomeFileProgettoLabel.Text = AttoConcessione.ProgettoTemp
                progettoUpload1.Visible = False
                progettoUpload2.Visible = True
            End If
        End If


        '******************************************************************************************
        'Dati Beneficiario
        '******************************************************************************************
        Me.RubricaComboBox.Text = Me.AttoConcessione.Beneficiario
        Me.CodiceFiscalePartitaIvaTextBox.Text = Me.AttoConcessione.DatoFiscaleBeneficiario

        If Me.AttoConcessione.IdBeneficiario > 0 Then
            Me.RubricaComboBox.SelectedValue = Me.AttoConcessione.IdBeneficiario.ToString
        End If



        If Not AttoConcessione.UrlCurriculum Is Nothing Then
            If AttoConcessione.UrlCurriculum <> String.Empty Then
                CurriculumAllegatoLinkButton.Text = AttoConcessione.UrlCurriculum
                NomeFileCurriculumLabel.Text = AttoConcessione.CurriculumTemp
                curriculumUpload1.Visible = False
                curriculumUpload2.Visible = True
            End If
        End If

        '******************************************************************************************

    End Sub

#End Region



#Region "EVENTI CONTROLLI"

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        If Me.VerificaDati() Then


            'Dim attoConcessione As ParsecAtt.AttoConcessione = Nothing


            'If Me.AttoConcessione Is Nothing Then
            '    attoConcessione = New ParsecAtt.AttoConcessione
            'Else

            '    If Me.Request.QueryString("Mode") = "Copia" Then
            '        attoConcessione = New ParsecAtt.AttoConcessione
            '    Else
            '        attoConcessione = Me.AttoConcessione
            '    End If

            'End If

            '******************************************************************************************
            'Dati atto di concessione
            '******************************************************************************************

            If Not String.IsNullOrEmpty(Me.NormaTextBox.Text) Then
                attoConcessione.TitoloNorma = Me.NormaTextBox.Text
            End If

            'If Not String.IsNullOrEmpty(Me.ModalitaComboBox.SelectedItem.Text) Then
            '    attoConcessione.Modalita = Me.ModalitaComboBox.SelectedItem.Text
            'End If

            attoConcessione.Modalita = Me.ModalitaTextBox.Text

            If Not String.IsNullOrEmpty(Me.ImportoAttoConcessioneTextBox.Text) Then
                attoConcessione.Importo = Me.ImportoAttoConcessioneTextBox.Value
            End If

            attoConcessione.UrlProgetto = ProgettoAllegatoLinkButton.Text
            attoConcessione.ProgettoTemp = NomeFileProgettoLabel.Text

            '******************************************************************************************


            '******************************************************************************************
            'Dati Beneficiario
            '******************************************************************************************

            If Not String.IsNullOrEmpty(Me.RubricaComboBox.Text) Then
                AttoConcessione.Beneficiario = Me.RubricaComboBox.Text.Trim
            End If

            If Not String.IsNullOrEmpty(Me.RubricaComboBox.SelectedValue) Then
                AttoConcessione.IdBeneficiario = RubricaComboBox.SelectedValue
            End If

            If Not String.IsNullOrEmpty(Me.CodiceFiscalePartitaIvaTextBox.Text) Then
                attoConcessione.DatoFiscaleBeneficiario = Me.CodiceFiscalePartitaIvaTextBox.Text
            End If

            attoConcessione.UrlCurriculum = CurriculumAllegatoLinkButton.Text
            attoConcessione.CurriculumTemp = NomeFileCurriculumLabel.Text

            '******************************************************************************************

            attoConcessione.Path = "\" & Now.Year & "\"

            ParsecUtility.SessionManager.AttoConcessione = attoConcessione
            ParsecUtility.Utility.ClosePopup(True)
        End If

    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaVista()
    End Sub

    Protected Sub TrovaBeneficiarioImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaBeneficiarioImageButton.Click
        'Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/RicercaBeneficiarioPage.aspx"
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
        Dim queryString As New Hashtable
        Dim parametriPagina As New Hashtable
        queryString.Add("obj", Me.AggiornaBeneficiarioImageButton.ClientID)
        queryString.Add("mode", "search")

        parametriPagina.Add("Filtro", Me.RubricaComboBox.Text)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

        If Not String.IsNullOrEmpty(Me.RubricaComboBox.Text) Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.RubricaComboBox.Text) And c.LogStato Is Nothing).ToList
            If struttureEsterne.Count = 1 Then
                Me.AggiornaReferenteEsterno(struttureEsterne(0))
                ParsecUtility.SessionManager.ParametriPagina = Nothing
            Else
                ParsecUtility.Utility.ShowPopup(pageUrl, 910, 730, queryString, False)
            End If
            rubrica.Dispose()
        Else
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 730, queryString, False)
        End If

        'ParsecUtility.Utility.ShowPopup(pageUrl, 910, 730, queryString, False)
    End Sub

    Private Sub AggiornaReferenteEsterno(ByVal strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo)
        Me.RubricaComboBox.Text = strutturaEsterna.Denominazione
        If Not String.IsNullOrEmpty(strutturaEsterna.CodiceFiscale) Then
            Me.CodiceFiscalePartitaIvaTextBox.Text = strutturaEsterna.CodiceFiscale
        Else
            Me.CodiceFiscalePartitaIvaTextBox.Text = strutturaEsterna.PartitaIVA
        End If

        Me.RubricaComboBox.SelectedValue = strutturaEsterna.Id
    End Sub

    Protected Sub AggiornaBeneficiarioImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBeneficiarioImageButton.Click
        If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
            Dim r As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
            AggiornaReferenteEsterno(r)
            ParsecUtility.SessionManager.Rubrica = Nothing
        End If
    End Sub

    Protected Sub AggiungiProgettoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiProgettoImageButton.Click
        If Me.ProgettoUpload.UploadedFiles.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Per inserire un progetto, è necessario specificarne il file relativo!", False)
        Else
            If Me.ProgettoUpload.UploadedFiles.Count > 0 Then
                Dim file As Telerik.Web.UI.UploadedFile = Me.ProgettoUpload.UploadedFiles(0)
                If file.FileName <> "" Then
                    Dim filenameTemp As String = Session.SessionID & "_" & file.FileName
                    Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

                    If Not IO.Directory.Exists(pathRootTemp) Then
                        IO.Directory.CreateDirectory(pathRootTemp)
                    End If

                    Dim pathDownload As String = pathRootTemp & filenameTemp
                    Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
                    Dim filename As String = file.FileName
                    file.SaveAs(pathDownload)

                    ProgettoAllegatoLinkButton.Text = filename
                    NomeFileProgettoLabel.Text = pathDownload

                    progettoUpload1.Visible = False
                    progettoUpload2.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub RimuoviProgettoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles RimuoviProgettoImageButton.Click
        Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & NomeFileProgettoLabel.Text
        If IO.File.Exists(pathDownload) Then
            IO.File.Delete(pathDownload)
        End If
        ProgettoAllegatoLinkButton.Text = ""
        NomeFileProgettoLabel.Text = ""

        progettoUpload1.Visible = True
        progettoUpload2.Visible = False
    End Sub

#End Region

    Protected Sub AggiungiCurriculumImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiCurriculumImageButton.Click
        If Me.CurriculumUpload.UploadedFiles.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Per inserire un curriculum, è necessario specificarne il file relativo!", False)
        Else
            If Me.CurriculumUpload.UploadedFiles.Count > 0 Then
                Dim file As Telerik.Web.UI.UploadedFile = Me.CurriculumUpload.UploadedFiles(0)
                If file.FileName <> "" Then
                    Dim filenameTemp As String = Session.SessionID & "_" & file.FileName
                    Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

                    If Not IO.Directory.Exists(pathRootTemp) Then
                        IO.Directory.CreateDirectory(pathRootTemp)
                    End If

                    Dim pathDownload As String = pathRootTemp & filenameTemp
                    Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
                    Dim filename As String = file.FileName
                    file.SaveAs(pathDownload)

                    CurriculumAllegatoLinkButton.Text = filename
                    NomeFileCurriculumLabel.Text = pathDownload

                    curriculumUpload1.Visible = False
                    curriculumUpload2.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub RimuoviCurriculumImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles RimuoviCurriculumImageButton.Click
        Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & NomeFileCurriculumLabel.Text
        If IO.File.Exists(pathDownload) Then
            IO.File.Delete(pathDownload)
        End If
        CurriculumAllegatoLinkButton.Text = ""
        NomeFileCurriculumLabel.Text = ""

        curriculumUpload1.Visible = True
        curriculumUpload2.Visible = False
    End Sub

    Protected Sub ProgettoAllegatoLinkButton_Click(sender As Object, e As System.EventArgs) Handles ProgettoAllegatoLinkButton.Click
        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathAttiPubblicazioni")
        Dim filename As String = ProgettoAllegatoLinkButton.Text 'AttoConcessione.UrlProgetto
        Dim filenameTemp As String = NomeFileProgettoLabel.Text

        If Not filename Is Nothing Then
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If Not String.IsNullOrEmpty(filenameTemp) Then
                'pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & filenameTemp
                pathDownload = filenameTemp
            Else
                percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                pathDownload = percorsoRoot & AttoConcessione.Path & filename
            End If
            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("Il progetto allegato non esiste!", False)
            End If
        End If
    End Sub

    Protected Sub CurriculumAllegatoLinkButton_Click(sender As Object, e As System.EventArgs) Handles CurriculumAllegatoLinkButton.Click
        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathAttiPubblicazioni")
        Dim filename As String = CurriculumAllegatoLinkButton.Text 'AttoConcessione.UrlCurriculum
        Dim filenameTemp As String = NomeFileCurriculumLabel.Text

        If Not filename Is Nothing Then
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If Not String.IsNullOrEmpty(filenameTemp) Then
                'pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & filenameTemp
                pathDownload = filenameTemp
            Else
                percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                pathDownload = percorsoRoot & AttoConcessione.Path & filename
            End If
            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("Il progetto allegato non esiste!", False)
            End If
        End If
    End Sub

 
End Class
