Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class EsportazioneWebPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadListBox .rlbCheck,.RadListBox .rlbImage,.RadListBox .rlbText{vertical-align: middle;padding-left:5px;}"
        Me.Page.Header.Controls.Add(css)

        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = "div.RadUploadProgressArea_Office2007 .ruProgress { background-image: none;}" & vbCrLf
        css.InnerHtml += ".RadUploadProgressArea { width: 320px !important;}" & vbCrLf
        css.InnerHtml += "div.RadUploadProgressArea li.ruProgressHeader{ margin: 10px 18px 0px; }" & vbCrLf
        css.InnerHtml += "table.CkeckListCss tr td label {margin-right:10px;padding-right:10px;}" & vbCrLf
        Me.Page.Header.Controls.Add(css)



        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Esportazione Web Atti"
        If Not Me.Page.IsPostBack Then
            Me.Documenti = New List(Of ParsecAtt.Documento)
            Me.CaricaTipologieDocumento()
            Me.CaricaTipologieRegistro()
            Me.CaricaTipologieSeduta()
            Me.CaricaTipologieConversione()
            Me.CaricaPublicazioniWeb()
            Me.CaricaPublicazioniAlbo()
            Me.ResettaFiltro()

            Me.SoloElencoRadioButton.Checked = True
            Me.TipoConversioneComboBox.Enabled = False
            Me.SelectAllCheckBox.Enabled = False
            Me.EsportaButton.Enabled = False
            Me.AnteprimaStampaButton.Enabled = False


            Me.PercorsoTextBox.Text = "D:\Sep_Export"
            Dim path = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentExport")
            If Not String.IsNullOrEmpty(path) Then
                Me.PercorsoTextBox.Text = path
            End If

        End If


        Me.SoloElencoRadioButton.Attributes.Add("onclick", Me.GetClientScript(False))
        Me.TuttoRadioButton.Attributes.Add("onclick", Me.GetClientScript(True))


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If


        Me.ContatoreGeneraleInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.ContatoreGeneraleInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.ContatoreGeneraleFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")



    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        If TuttoRadioButton.Checked Then
            Me.TipoConversioneComboBox.Enabled = True
            Me.IncludiAllegatiCheckBox.Enabled = True
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.PannelloRisultatiLabel.Text = "Elenco Atti&nbsp;&nbsp;&nbsp;" & If(Me.AttiListBox.Items.Count > 0, "( " & Me.AttiListBox.Items.Count.ToString & " )", "")
    End Sub

#End Region

#Region "PROPRIETA'"

    Public Property Documenti() As List(Of ParsecAtt.Documento)
        Get
            Return CType(Session("EsportazioneWebPage_Documenti"), List(Of ParsecAtt.Documento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Documento))
            Session("EsportazioneWebPage_Documenti") = value
        End Set
    End Property

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub AnteprimaStampaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnteprimaStampaButton.Click
        Try
            Me.Print()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message.ToString, False)
        End Try
    End Sub

    Protected Sub EsportaButton_Click(sender As Object, e As System.EventArgs) Handles EsportaButton.Click
        Try
            Me.GeneraDataSourceWebExport()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message.ToString, False)
        End Try
    End Sub


    Protected Sub TipologieDocumentoComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieDocumentoComboBox.SelectedIndexChanged
        Me.CaricaTipologieRegistro()
        Me.ImpostaAbilitazioneUI()
    End Sub

    Protected Sub PubblicazioneAlboComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles PubblicazioneAlboComboBox.SelectedIndexChanged
        If e.Value = "0" Then
            Me.DataPubblicazioneInizioTextBox.Enabled = False
            Me.DataPubblicazioneFineTextBox.Enabled = False
            Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
            Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing
        Else
            Me.DataPubblicazioneInizioTextBox.Enabled = True
            Me.DataPubblicazioneFineTextBox.Enabled = True
        End If
    End Sub

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Try
                Me.ApplicaFiltro()
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    Protected Sub TrovaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaSettoreImageButton.Click
        Me.TrovaSettore()
    End Sub

    Protected Sub AggiornaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSettoreImageButton.Click
        Me.AggiornaSettore()
    End Sub

    Protected Sub EliminaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaSettoreImageButton.Click
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
    End Sub

    Private Sub NotificaOperazione()
        Me.infoOperazioneHidden.Value = "Esportazione conclusa con successo!"
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim path = ParsecAdmin.WebConfigSettings.GetKey("PathDownload")
        Dim files = IO.Directory.GetFiles(path, "*.txt")
        Dim nomefile As String = String.Empty
        Try
            For Each f In files
                nomefile = IO.Path.GetFileName(f)
                If nomefile.StartsWith("Utente_" & utenteCollegato.Id.ToString & "_") Then
                    IO.File.Delete(f)
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub notificaOperazioneButton_Click(sender As Object, e As System.EventArgs) Handles notificaOperazioneButton.Click
        Me.NotificaOperazione()
    End Sub

#End Region

#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

#Region "METODI PRIVATI"


    Private Function GetClientScript(ByVal enable As Boolean) As String
        Dim script As New System.Text.StringBuilder

        script.Append("var combo= $find('" & Me.TipoConversioneComboBox.ClientID & "');")
        If enable Then
            script.Append("combo.enable();")
        Else
            script.Append("combo.disable();")
            script.Append("combo.clearSelection();")
        End If

        script.Append("var chk= document.getElementById('" & Me.IncludiAllegatiCheckBox.ClientID & "');")
        If enable Then
            script.Append("chk.disabled=false;")
        Else
            script.Append("chk.checked= false;")
            script.Append("chk.disabled=true;")
        End If

        Return script.ToString
    End Function

    Private Sub ApplicaFiltro()

        Dim filtro As ParsecAtt.FiltroDocumento = Me.GetFiltro
        Dim documenti As New ParsecAtt.DocumentoRepository
        Me.Documenti = documenti.GetView(filtro)



        If Me.Documenti.Count = 0 Then
            Me.AttiListBox.Items.Clear()
            Me.EsportaButton.Enabled = False
            Me.AnteprimaStampaButton.Enabled = False
            Me.SelectAllCheckBox.Checked = False
            Me.SelectAllCheckBox.Enabled = False
            Throw New ApplicationException("Nessun atto amministrativo trovato con i criteri di filtro impostati!")
        End If

        Me.AttiListBox.DataSource = Me.Documenti
        Me.AttiListBox.DataValueField = "Id"
        Me.AttiListBox.DataTextField = "Descrizione"
        Me.AttiListBox.DataBind()

        documenti.Dispose()

        Me.SelectAllCheckBox.Checked = True
        Dim checked As Boolean = Me.SelectAllCheckBox.Checked
        For i As Integer = 0 To Me.AttiListBox.Items.Count - 1
            Me.AttiListBox.Items(i).Checked = checked
        Next

        Me.SelectAllCheckBox.Enabled = True
        Me.EsportaButton.Enabled = True
        Me.AnteprimaStampaButton.Enabled = True


    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroDocumento
        Dim filtro As New ParsecAtt.FiltroDocumento

        filtro.DataDocumentoInizio = Me.DataDocumentoInizioTextBox.SelectedDate
        filtro.DataDocumentoFine = Me.DataDocumentoFineTextBox.SelectedDate
        filtro.DataPubblicazioneInizio = Me.DataPubblicazioneInizioTextBox.SelectedDate
        filtro.DataPubblicazioneFine = Me.DataPubblicazioneFineTextBox.SelectedDate

        If Me.ContatoreGeneraleInizioTextBox.Value.HasValue Then
            filtro.ContatoreGeneraleInizio = CInt(Me.ContatoreGeneraleInizioTextBox.Value)
        End If

        If Me.ContatoreGeneraleFineTextBox.Value.HasValue Then
            filtro.ContatoreGeneraleFine = CInt(Me.ContatoreGeneraleFineTextBox.Value)
        End If

        If Me.TipologieDocumentoComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaDocumento = CInt(Me.TipologieDocumentoComboBox.SelectedValue)
        End If

        If Me.TipologieRegistroComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaRegistro = CInt(Me.TipologieRegistroComboBox.SelectedValue)
        End If

        'TIPOLOGIA SEDUTA
        If Me.TipologieSedutaComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
        End If

        'SETTORE
        'If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
        '    filtro.IdSettore = CInt(Me.IdSettoreTextBox.Text)
        'Else
        filtro.Settore = Me.SettoreTextBox.Text.Trim
        'End If



        'PUBBLICATO MODULO MESSI
        If Me.PubblicazioneAlboComboBox.SelectedIndex > 0 Then
            filtro.PubblicazioneAlbo = CBool(Me.PubblicazioneAlboComboBox.SelectedValue)
        End If

        'PUBBLICABILE
        If Me.PubblicazioneWebComboBox.SelectedIndex > 0 Then
            filtro.PubblicazioneWeb = CBool(Me.PubblicazioneWebComboBox.SelectedValue)
        End If

        filtro.ApplicaAbilitazione = True

        Return filtro
    End Function

    Public Sub GeneraDataSourceWebExport()


        Dim includeAttachments As Boolean = Me.IncludiAllegatiCheckBox.Checked


        If Me.AttiListBox.CheckedItems.Count = 0 Then
            Throw New ApplicationException("E' necessario selezionare almeno un atto amministrativo!")
        End If

        If String.IsNullOrEmpty(Me.PercorsoTextBox.Text) Then
            Throw New ApplicationException("E' necessario specificare il percorso dell'esportazione!")
        End If

        If TuttoRadioButton.Checked Then
            If Me.TipoConversioneComboBox.SelectedItem Is Nothing Then
                Throw New ApplicationException("E' necessario specificare il formato dell'esportazione!")
            End If
        End If



        Dim exportLocalPath As String = Me.PercorsoTextBox.Text.Replace("\", "/")
        If Not exportLocalPath.EndsWith("/") Then
            exportLocalPath &= "/"
        End If


        Dim listaId As List(Of Integer) = Me.AttiListBox.CheckedItems.Select(Function(c) CInt(c.Value)).ToList

        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim atto As ParsecAtt.Documento = Nothing
        Dim allegati As List(Of ParsecAtt.Allegato) = Nothing

        Dim datiInput As New ParsecAdmin.DatiInput With {.FunctionName = "ExportForWeb", .ShowWindow = False, .PdfA = False}

        Dim srcHttpPath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti")

        Dim srcHttpPathAllegati As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocument")




        Dim datiSrc As New ParsecAdmin.DatiInputSrc
        Dim datiAttachment As New ParsecAdmin.DatiInputAttachment

        Dim filterType As String = String.Empty

        Dim datiXsl As New ParsecAdmin.DatiXsl
        If Not Me.TipoConversioneComboBox.SelectedItem Is Nothing Then
            Select Case Me.TipoConversioneComboBox.SelectedItem.Value
                Case "writer_pdf_Export"
                    datiXsl.Extension = ".pdf"
                Case "Rich Text Format"
                    datiXsl.Extension = ".rtf"
                Case "MS Word 97"
                    datiXsl.Extension = ".doc"
            End Select
            filterType = Me.TipoConversioneComboBox.SelectedItem.Value
        End If




        Dim exportDocument As ParsecAdmin.ExportDocument = Nothing
        Dim exportAttachment As ParsecAdmin.ExportDocument = Nothing

        Dim count = listaId.Count
        Dim context = RadProgressContext.Current
        context.PrimaryTotal = count.ToString
        Dim i As Integer = 0

        For Each item In listaId
            i += 1

            context.PrimaryValue = i.ToString
            context.PrimaryPercent = CInt(((i / count) * 100)).ToString

            If Not Response.IsClientConnected Then
                Exit For
            End If


            Dim idDocumento As Integer = item
            atto = documenti.GetById(idDocumento)

            If Me.TuttoRadioButton.Checked Then
                Dim nomefile As String = String.Format("{0}{1}/{2}", srcHttpPath, atto.Data.Value.Year, atto.Nomefile)
                datiSrc.Filenames.Add(nomefile)
            End If

            exportDocument = New ParsecAdmin.ExportDocument
            exportDocument.TipoDocumento = atto.TipologiaDocumento.ToString
            exportDocument.Settore = atto.DescrizioneSettore
            exportDocument.Oggetto = atto.Oggetto
            exportDocument.NumeroGenerale = atto.ContatoreGenerale

            If atto.ContatoreStruttura.HasValue Then
                exportDocument.NumeroSettore = atto.ContatoreStruttura
            End If

            exportDocument.Data = atto.Data
            exportDocument.NomeFile = atto.Nomefile

            If includeAttachments Then
                datiXsl.IncludeAttachments = True

                allegati = documenti.GetAllegati(atto.Id)
                If allegati.Count > 0 Then
                    For Each all In allegati

                        Dim nomefileAllegato As String = String.Format("{0}{1}/{2}", srcHttpPathAllegati, all.PercorsoRelativo.Replace("\", ""), all.Nomefile)

                        datiAttachment.Filenames.Add(nomefileAllegato)
                        exportAttachment = New ParsecAdmin.ExportDocument With {.Oggetto = all.Oggetto, .NomeFile = all.Nomefile}
                        exportDocument.ExportAttachments.Add(exportAttachment)
                    Next
                End If
            End If

            datiXsl.ExportDocuments.Add(exportDocument)

        Next

        context.OperationComplete = True

        Dim datiDest As New ParsecAdmin.DatiInputDest

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("NomeFileElencoDocumentiEsportati", ParsecAdmin.TipoModulo.ATT)
        datiDest.DestXslLocalPath = "elenco.XLS"
        If Not parametro Is Nothing Then
            datiDest.DestXslLocalPath = parametro.Valore.Trim
        End If
        parametri.Dispose()


        datiDest.DestLocalPath = exportLocalPath
        datiDest.CreateZip = Me.CreaZipCheckBox.Checked
        datiDest.FilterType = filterType
        datiDest.SrcDatHttpPath = ""


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        documenti.Dispose()

        Dim data = openofficeParameters.CreateDataSourceWebExport(datiInput, datiAttachment, datiSrc, datiDest, datiXsl)

        '************************************************************************************************************************
        'SCRIVO IL FILE CHE CONTIENE IL DATASOURCE 
        'LA LUNGHEZZA MASSIMA DELLA LINEA DI COMANDO E' 32K (32768 byte) 16384 CARATTERI
        '************************************************************************************************************************
        Dim localFilenameDataSource = "Utente_" & utenteCollegato.Id.ToString & "_" & Guid.NewGuid.ToString & ".txt"
        Dim localPathDataSource As String = String.Format("{0}{1}", ParsecAdmin.WebConfigSettings.GetKey("PathDownload"), localFilenameDataSource)
        IO.File.WriteAllText(localPathDataSource, data)
        Dim dataSource = openofficeParameters.CreateDataSource(localFilenameDataSource)
        '************************************************************************************************************************

        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(dataSource, Me.notificaOperazioneButton.ClientID, True, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(dataSource, True, AddressOf Me.NotificaOperazione)
        End If


    End Sub

    Private Sub Print()

        If Me.AttiListBox.CheckedItems.Count = 0 Then
            Throw New ApplicationException("E' necessario selezionare almeno un atto amministrativo!")
        End If

        Dim listaId As List(Of Integer) = Me.AttiListBox.CheckedItems.Select(Function(c) CInt(c.Value)).ToList
        Dim parametriStampa As New Hashtable

        Dim tipologiaDocumento As ParsecAtt.TipoDocumento = CType(CInt(Me.TipologieDocumentoComboBox.SelectedValue), ParsecAtt.TipoDocumento)

        Select Case tipologiaDocumento
            Case ParsecAtt.TipoDocumento.Determina
                parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleDetermine")
            Case ParsecAtt.TipoDocumento.Delibera
                parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleDelibera")
            Case ParsecAtt.TipoDocumento.Decreto
                parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleDecreto")
            Case ParsecAtt.TipoDocumento.Ordinanza
                parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleOrdinanza")
        End Select
        Dim datiStampa = Me.Documenti.Where(Function(c) listaId.Contains(c.Id))
        parametriStampa.Add("DatiStampa", datiStampa)
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)

    End Sub


    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean

        If Me.TipologieDocumentoComboBox.SelectedIndex = 0 Then
            message.AppendLine("E' necessario selezionare una tipologia di documento!")
        End If

        If Me.ContatoreGeneraleFineTextBox.Value.HasValue AndAlso Me.ContatoreGeneraleInizioTextBox.Value.HasValue Then
            If Me.ContatoreGeneraleInizioTextBox.Value > Me.ContatoreGeneraleFineTextBox.Value Then
                message.AppendLine("Il campo 'Registro Generale da' deve essere inferiore o uguale al campo 'Registro Generale a'!")
            End If
        End If

        'Se la data iniziale è maggiore di quella finale.
        If Me.DataDocumentoInizioTextBox.SelectedDate.HasValue AndAlso Me.DataDocumentoFineTextBox.SelectedDate.HasValue Then
            If Date.Compare(Me.DataDocumentoInizioTextBox.SelectedDate, Me.DataDocumentoFineTextBox.SelectedDate) > 0 Then
                message.AppendLine("Il campo 'Data Documento da' deve essere antecedente o uguale al campo 'Data Documento a'!")
            End If
        End If

        If Me.DataPubblicazioneInizioTextBox.SelectedDate.HasValue AndAlso Me.DataPubblicazioneFineTextBox.SelectedDate.HasValue Then
            If Date.Compare(Me.DataPubblicazioneInizioTextBox.SelectedDate, Me.DataPubblicazioneFineTextBox.SelectedDate) > 0 Then
                message.AppendLine("Il campo 'Data Pubblicazione da' deve essere antecedente o uguale al campo 'Data Pubblicazione a'!")
            End If
        End If


        Return Not message.Length > 0
        Return True
    End Function

    Private Sub ResettaFiltro()
        Me.DataDocumentoInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataDocumentoFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
        Me.ContatoreGeneraleInizioTextBox.Text = String.Empty
        Me.ContatoreGeneraleFineTextBox.Text = String.Empty

        Me.TipologieDocumentoComboBox.SelectedIndex = 0
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0



        Me.TipologieRegistroComboBox.SelectedIndex = 0
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.ImpostaAbilitazioneUI()
        Me.AttiListBox.Items.Clear()

        Me.SelectAllCheckBox.Enabled = False
        Me.SelectAllCheckBox.Checked = False
        Me.EsportaButton.Enabled = False
        Me.AnteprimaStampaButton.Enabled = False
    End Sub

    Private Sub ImpostaAbilitazioneUI()
        If Me.TipologieDocumentoComboBox.SelectedValue <> "-1" Then
            Me.ImpostaAbilitazione(True)
            Dim tipologiaDocumento As ParsecAtt.TipoDocumento = CType(Me.TipologieDocumentoComboBox.SelectedValue, ParsecAtt.TipoDocumento)
            Select Case tipologiaDocumento
                Case ParsecAtt.TipoDocumento.Delibera
                    Me.ImpostaAbilitazioneDelibera()
                Case ParsecAtt.TipoDocumento.Determina
                    Me.ImpostaAbilitazioneDetermina()
                Case ParsecAtt.TipoDocumento.Ordinanza
                    Me.ImpostaAbilitazioneOrdinanza()
                Case ParsecAtt.TipoDocumento.Decreto
                    Me.ImpostaAbilitazioneOrdinanza()
            End Select
        Else
            Me.ImpostaAbilitazione(False)
        End If
        Me.ResettaComboBox()
    End Sub


    Private Sub ImpostaAbilitazioneDelibera()
        Me.TipologiaRegistroPanel.Enabled = False
    End Sub

    Private Sub ImpostaAbilitazioneDetermina()
        Me.TipologiaSedutaPanel.Enabled = False
    End Sub

    Private Sub ImpostaAbilitazioneOrdinanza()
        Me.ImpostaAbilitazione(False)
    End Sub

    Private Sub ResettaComboBox()
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        Me.PubblicazioneWebComboBox.SelectedIndex = 0
        Me.TipologieRegistroComboBox.SelectedIndex = 0
    End Sub

    Private Sub ImpostaAbilitazione(ByVal abilita As Boolean)
        Me.TipologiaSedutaPanel.Enabled = abilita
        Me.PubblicazioneWePanel.Enabled = abilita
        Me.TipologiaRegistroPanel.Enabled = abilita
    End Sub

    Private Sub CaricaTipologieDocumento()
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Me.TipologieDocumentoComboBox.DataValueField = "Id"
        Me.TipologieDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologieDocumentoComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaDocumento With {.Modellizzabile = True}).Where(Function(c) Not c.Descrizione.StartsWith("Proposta"))
        Me.TipologieDocumentoComboBox.DataBind()
        Me.TipologieDocumentoComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieDocumentoComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub CaricaTipologieConversione()
        Me.TipoConversioneComboBox.AllowCustomText = True
        Me.TipoConversioneComboBox.Items.Add(New RadComboBoxItem("Portable Document Format (pdf)", "writer_pdf_Export"))
        Me.TipoConversioneComboBox.Items.Add(New RadComboBoxItem("Rich Text Format (rtf)", "Rich Text Format"))
        Me.TipoConversioneComboBox.Items.Add(New RadComboBoxItem("Microsoft Word 97 (doc)", "MS Word 97"))
        Me.TipoConversioneComboBox.SelectedIndex = -1

    End Sub

    Private Sub CaricaPublicazioniAlbo()
        Me.PubblicazioneAlboComboBox.Items.Add(New RadComboBoxItem("Pubblicate", "1"))
        Me.PubblicazioneAlboComboBox.Items.Add(New RadComboBoxItem("Non pubblicate", "0"))
        Me.PubblicazioneAlboComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0
    End Sub


    Private Sub CaricaPublicazioniWeb()
        Me.PubblicazioneWebComboBox.Items.Add(New RadComboBoxItem("Pubblicabili", "1"))
        Me.PubblicazioneWebComboBox.Items.Add(New RadComboBoxItem("Non pubblicabili", "0"))
        Me.PubblicazioneWebComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

    End Sub

    Private Sub CaricaTipologieRegistro(idTipologiaDocumento As Nullable(Of Integer))
        Dim tipologie As New ParsecAtt.TipologieRegistroRepository
        Me.TipologieRegistroComboBox.DataValueField = "Id"
        Me.TipologieRegistroComboBox.DataTextField = "Descrizione"
        Me.TipologieRegistroComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaRegistro With {.Disattivato = False, .IdTipologiaDocumento = idTipologiaDocumento})
        Me.TipologieRegistroComboBox.DataBind()
        Me.TipologieRegistroComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieRegistroComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub CaricaTipologieRegistro()
        Dim idTipologiaDocumento As Nullable(Of Integer) = Nothing
        If Me.TipologieDocumentoComboBox.SelectedIndex > 0 Then
            idTipologiaDocumento = CInt(Me.TipologieDocumentoComboBox.SelectedValue)
            Me.CaricaTipologieRegistro(idTipologiaDocumento)
        Else
            Me.TipologieRegistroComboBox.Items.Clear()
        End If
    End Sub

    Private Sub CaricaTipologieSeduta()
        Dim tipologieSedute As New ParsecAtt.TipologiaSedutaRepository
        Me.TipologieSedutaComboBox.DataValueField = "Id"
        Me.TipologieSedutaComboBox.DataTextField = "Descrizione"
        Me.TipologieSedutaComboBox.DataSource = tipologieSedute.GetKeyValue(Nothing)
        Me.TipologieSedutaComboBox.DataBind()
        Me.TipologieSedutaComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        tipologieSedute.Dispose()
    End Sub

    Private Sub AggiornaSettore()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim idSettore As Integer = struttureSelezionate.First.Id
            'Aggiorno il settore
            Me.SettoreTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdSettoreTextBox.Text = idSettore.ToString
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
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100")
        parametriPagina.Add("ultimoLivelloStruttura", "100")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    End Sub

#End Region


End Class