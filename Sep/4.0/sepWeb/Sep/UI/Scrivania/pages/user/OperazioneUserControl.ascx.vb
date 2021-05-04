Imports ParsecAdmin
Imports Telerik.Web.UI
Imports System.Data
Imports System.Xml
Imports System.Web.Mail
Imports Rebex.Net
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser
Imports System.Net
Imports System.Reflection

Partial Class OperazioneUserControl
    Inherits System.Web.UI.UserControl

    Public Enum Direction
        Forward = 0
        Backward = 1
    End Enum


    Public Class InfoFirma
        Public Property Anno As Integer
        Public Property NomeFile As String
        Public Property EntitaId As Integer
    End Class


    Private salvaImpegniDefinitiviDedaGroup As ParsecWKF.ParametroProcesso = Nothing

#Region "PROPRIETA' PUBBLICHE A LIVELLO DI PAGINA"


    Public Property TaskAttivi() As List(Of ParsecWKF.TaskAttivo)
        Get
            Return CType(Session("OperazionePage_TaskAttivi"), List(Of ParsecWKF.TaskAttivo))
        End Get
        Set(ByVal value As List(Of ParsecWKF.TaskAttivo))
            Session("OperazionePage_TaskAttivi") = value
        End Set
    End Property

    Public Property TaskAttivo() As ParsecWKF.TaskAttivo
        Get
            Return CType(Session("OperazionePage_TaskAttivo"), ParsecWKF.TaskAttivo)
        End Get
        Set(ByVal value As ParsecWKF.TaskAttivo)
            Session("OperazionePage_TaskAttivo") = value
        End Set
    End Property

    Public Property RuoloId() As Nullable(Of Integer)
        Get
            Return CType(Session("OperazionePage_RuoloId"), Integer)
        End Get
        Set(ByVal value As Nullable(Of Integer))
            Session("OperazionePage_RuoloId") = value
        End Set
    End Property

    Public Property Nomefile() As String
        Get
            Return CType(Session("OperazionePage_Nomefile"), String)
        End Get
        Set(ByVal value As String)
            Session("OperazionePage_Nomefile") = value
        End Set
    End Property


    Public Property Action() As ParsecWKF.Action
        Get
            Return CType(Session("OperazionePage_Action"), ParsecWKF.Action)
        End Get
        Set(ByVal value As ParsecWKF.Action)
            Session("OperazionePage_Action") = value
        End Set
    End Property

    Public Property FirstClick() As Boolean
        Get
            Return CType(Session("OperazionePage_FirstClick"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Session("OperazionePage_FirstClick") = value
        End Set
    End Property


    Public Property MessaggioAvviso As String
        Get
            Return ViewState("MessaggioAvviso")
        End Get
        Set(value As String)
            ViewState("MessaggioAvviso") = value
        End Set
    End Property


    Public Property AutomaticTask() As Boolean
        Get
            Return CType(Session("OperazionePage_AutomaticTask"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Session("OperazionePage_AutomaticTask") = value
        End Set
    End Property

    Public Property OperazioneMassiva() As Boolean
        Get
            Return CType(Session("OperazionePage_OperazioneMassiva"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Session("OperazionePage_OperazioneMassiva") = value
        End Set
    End Property

    Public Property StoricoNote() As List(Of InfoNote)
        Get
            Return CType(Session("OperazionePage_StoricoNote"), List(Of InfoNote))
        End Get
        Set(ByVal value As List(Of InfoNote))
            Session("OperazionePage_StoricoNote") = value
        End Set
    End Property


    Public Property DataSources() As List(Of String)
        Get
            Return CType(Session("OperazionePage_DataSources"), List(Of String))
        End Get
        Set(ByVal value As List(Of String))
            Session("OperazionePage_DataSources") = value
        End Set
    End Property


    Public Property FileToSignDictionary() As Dictionary(Of Integer, InfoFirma)
        Get
            Return CType(Session("OperazionePage_FileToSignDictionary"), Dictionary(Of Integer, InfoFirma))
        End Get
        Set(ByVal value As Dictionary(Of Integer, InfoFirma))
            Session("OperazionePage_FileToSignDictionary") = value
        End Set
    End Property

    Public Property BackupFileToDeleteDictionary() As Dictionary(Of Integer, String)
        Get
            Return CType(Session("OperazionePage_BackupFileToDeleteDictionary"), Dictionary(Of Integer, String))
        End Get
        Set(ByVal value As Dictionary(Of Integer, String))
            Session("OperazionePage_BackupFileToDeleteDictionary") = value
        End Set
    End Property

    Public Property ElencoDestinatariDictionary() As Dictionary(Of Integer, Integer)
        Get
            Return CType(Session("OperazionePage_ElencoDestinatariDictionary"), Dictionary(Of Integer, Integer))
        End Get
        Set(ByVal value As Dictionary(Of Integer, Integer))
            Session("OperazionePage_ElencoDestinatariDictionary") = value
        End Set
    End Property


    'Public Property AllegatiFattura As List(Of ParsecPro.AllegatoFattura)
    '    Get
    '        Return CType(Session("OperazionePage_AllegatiFattura"), List(Of ParsecPro.AllegatoFattura))
    '    End Get
    '    Set(ByVal value As List(Of ParsecPro.AllegatoFattura))
    '        Session("OperazionePage_AllegatiFattura") = value
    '    End Set
    'End Property

    'Public Property HtmlFattura As String
    '    Get
    '        Return CType(Session("OperazionePage_HtmlFattura"), String)
    '    End Get
    '    Set(ByVal value As String)
    '        Session("OperazionePage_HtmlFattura") = value
    '    End Set
    'End Property

    Public Property DataFirma As Nullable(Of DateTime)
        Get
            Return CType(Session("OperazionePage_DataFirma"), Nullable(Of DateTime))
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            Session("OperazionePage_DataFirma") = value
        End Set
    End Property


    Public Property ElencoErrori As String
        Get
            Return CType(Session("OperazionePage_ElencoErrori"), String)
        End Get
        Set(value As String)
            Session("OperazionePage_ElencoErrori") = value
        End Set
    End Property


    Public Property DocumentoGenericoDaFileSystem() As Nullable(Of Boolean)
        Get
            Return CType(Session("OperazionePage_DocumentoGenericoDaFileSystem"), Nullable(Of Boolean))
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            Session("OperazionePage_DocumentoGenericoDaFileSystem") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        'Me.ChiudiFatturaButton.Attributes.Add("onclick", "HideFatturaPanelOperazione();HideControlPanel();")

        ''Me.StampaFatturaButton.Attributes.Add("onclick", "PrintFatturaContent();HideFatturaPanelOperazione();HideControlPanel();return false;")
        'Me.StampaFatturaButton.Attributes.Add("onclick", "PrintFatturaContent();return false;")


        Me.ChiudiButton.Attributes.Add("onclick", "currentPanel=0;HidePanel(currentPanel);hide = true; updateGrid=true;return false;")
        Me.AggiornaIterButton.Attributes.Add("onclick", "panelIsVisible=false;")

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.PannelloGriglia.Style.Add("width", widthStyle)
        Me.NoteGridView.Style.Add("width", widthStyle)

        'Me.ChiudiRecipientsButton.Attributes.Add("onclick", "HideRecipientsPanel();hideRecipients = true; return false;")
        'Me.ConfermaRecipientsButton.Attributes.Add("onclick", "HideRecipientsPanel();hideRecipients = true;")

        Me.ChiudiRecipientsButton.Attributes.Add("onclick", "HideRecipientsPanel();HideControlPanel();hideRecipients = true; return false;")
        Me.ConfermaRecipientsButton.Attributes.Add("onclick", "HideRecipientsPanel();HideControlPanel();hideRecipients = true;")

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'If Not Me.HtmlFattura Is Nothing Then
        '    Me.pannelloFattura.Controls.Clear()
        '    Me.pannelloFattura.Controls.Add(New LiteralControl(Me.HtmlFattura))
        'End If
    End Sub


#End Region

#Region "EVENTI CONTROLLO PAGINA"

    'Protected Sub VisualizzaDocumentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoImageButton.Click

    '    Select Case Me.TaskAttivo.IdModulo
    '        Case ParsecAdmin.TipoModulo.ATT, ParsecAdmin.TipoModulo.CSRA
    '            Dim documenti As New ParsecAtt.DocumentoRepository
    '            Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '            documenti.Dispose()

    '            If Not documento Is Nothing Then
    '                Dim nomefile As String = documento.Nomefile
    '                Dim annoEsercizio As Integer = Now.Year
    '                documento.Modello = documenti.GetModello(documento.IdModello)
    '                If documento.Modello.Proposta Then
    '                    annoEsercizio = documento.DataProposta.Value.Year
    '                Else
    '                    annoEsercizio = documento.Data.Value.Year
    '                End If
    '                Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefile)
    '                If IO.File.Exists(localPath) Then
    '                    Me.VisualizzaDocumento(nomefile, annoEsercizio, False)
    '                Else
    '                    ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
    '                End If
    '            End If
    '        Case ParsecAdmin.TipoModulo.PRO



    '            Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
    '            Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault


    '            '***************************************************************************************
    '            'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
    '            '***************************************************************************************
    '            If Not documentoGenerico Is Nothing Then
    '                Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

    '                Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)

    '                Dim visibile As Boolean = False

    '                If modello.VisibilitaPubblica.HasValue Then
    '                    visibile = modello.VisibilitaPubblica
    '                End If

    '                If Not visibile Then
    '                    visibile = (utenteCorrente.SuperUser OrElse documentoGenerico.IdUtente = utenteCorrente.Id)
    '                End If


    '                If visibile Then
    '                    Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
    '                    Dim sorgenti As New ParsecAdmin.SorgentiRepository
    '                    Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
    '                    sorgenti.Dispose()
    '                    Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
    '                    Dim fileExists As Boolean = False
    '                    Select Case sorgenteCorrente.IdTipologia
    '                        Case 1 'Locale
    '                            Dim localPath As String = source.Path & anno & "\" & documentoGenerico.NomeFile
    '                            fileExists = IO.File.Exists(localPath)
    '                        Case 2  'Ftp
    '                            Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
    '                            fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
    '                    End Select

    '                    If Not fileExists Then
    '                        ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
    '                        Exit Sub
    '                    Else
    '                        Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, False, source)
    '                    End If
    '                Else
    '                    ParsecUtility.Utility.MessageBox("Non si possiedono le autorizzazioni per visualizzare il documento!", False)
    '                End If
    '            End If

    '            documentiGenerici.Dispose()

    '    End Select


    '    Me.EnableUiHidden.Value = "Abilita"
    'End Sub

    Protected Sub VisualizzaDocumentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoImageButton.Click


        Select Case Me.TaskAttivo.IdModulo



            Case ParsecAdmin.TipoModulo.ATT, ParsecAdmin.TipoModulo.CSRA
                Dim documenti As New ParsecAtt.DocumentoRepository
                Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                documenti.Dispose()

                If Not documento Is Nothing Then
                    Dim nomefile As String = documento.Nomefile
                    Dim annoEsercizio As Integer = Now.Year
                    documento.Modello = documenti.GetModello(documento.IdModello)
                    If documento.Modello.Proposta Then
                        annoEsercizio = documento.DataProposta.Value.Year
                    Else
                        annoEsercizio = documento.Data.Value.Year
                    End If
                    Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefile)
                    If IO.File.Exists(localPath) Then
                        Me.VisualizzaDocumento(nomefile, annoEsercizio, False)
                    Else
                        ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
                    End If
                End If
            Case ParsecAdmin.TipoModulo.PRO



                Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault


                '***************************************************************************************
                'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
                '***************************************************************************************
                If Not documentoGenerico Is Nothing Then
                    Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                    Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)

                    Dim visibile As Boolean = False

                    If modello.VisibilitaPubblica.HasValue Then
                        visibile = modello.VisibilitaPubblica
                    End If

                    If Not visibile Then
                        visibile = (utenteCorrente.SuperUser OrElse documentoGenerico.IdUtente = utenteCorrente.Id)
                    End If


                    If visibile Then
                        Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
                        Dim sorgenti As New ParsecAdmin.SorgentiRepository
                        Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
                        sorgenti.Dispose()
                        Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
                        Dim fileExists As Boolean = False
                        Select Case sorgenteCorrente.IdTipologia
                            Case 1 'Locale
                                Dim localPath As String = source.Path & anno & "\" & documentoGenerico.NomeFile
                                fileExists = IO.File.Exists(localPath)
                            Case 2  'Ftp
                                Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
                                fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
                        End Select

                        If Not fileExists Then
                            ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
                            Exit Sub
                        Else
                            If documentoGenerico.GeneratoSistemaSEP Then
                                Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, False, source)
                            Else
                                Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, source)
                            End If
                        End If
                    Else
                        ParsecUtility.Utility.MessageBox("Non si possiedono le autorizzazioni per visualizzare il documento!", False)
                    End If
                End If

                documentiGenerici.Dispose()

        End Select


        Me.EnableUiHidden.Value = "Abilita"
    End Sub



    Protected Sub VisualizzaStoricoPraticheImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaStoricoPraticheImageButton.Click
        Dim pageUrl As String = "~/UI/Contenzioso/pages/user/StoricoPratichePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Mode", "View")
        ParsecUtility.Utility.ShowPopup(pageUrl, 1130, 600, queryString, False)
    End Sub

    Protected Sub ChiudiButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiButton.Click
        Me.ResettaUI()
    End Sub

    Protected Sub AggiornaTaskButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaTaskButton.Click
        Dim script As New StringBuilder
        script.AppendLine("<script>")
        script.AppendLine("UpdateTask();")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
    End Sub

    Private Sub ConvertiDocumentoGenericoInPdf(ByVal allegato As ParsecPro.Allegato)


        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
        percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
        Dim dest = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile


        'Dim sorgenti As New ParsecAdmin.SorgentiRepository
        'Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(Me.DocumentoGenerico.IdSorgente)
        'sorgenti.Dispose()
        'Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)


        ''************************************************************************************************************
        ''CONVERTO IL DOCUMENTO ODT IN PDF
        ''************************************************************************************************************

        'Dim openOfficeParameters As New ParsecAdmin.OpenOfficeParameters
        'Dim datiInput As New ParsecAdmin.DatiInput

        'Dim anno = Me.DocumentoGenerico.PercorsoRelativo.Replace("\", "")
        'Dim nomefile = Me.DocumentoGenerico.NomeFile
        'Dim relativePath = source.RelativePath

        'Dim srcRemotePath = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), relativePath, anno, nomefile)
        'Dim destRemotePath = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), relativePath, anno)

        'datiInput.SrcRemotePath = srcRemotePath
        'datiInput.DestRemotePath = destRemotePath


        'Dim dataPdf As String = openOfficeParameters.CreateDataSourceForPdf(datiInput)

        'ParsecUtility.Utility.RegistraTimerEseguiConversionePdf(dataPdf, Me.convertiInPdfButton.ClientID, True, False)




        '  Protected Sub convertiInPdfButton_Click(sender As Object, e As System.EventArgs) Handles convertiInPdfButton.Click

        '  End Sub



        '<asp:Button runat="server" ID="convertiInPdfButton" Style="width: 0px; height: 0px;
        '                  left: -1000px; position: absolute" />



    End Sub

    Protected Sub AggiornaProtocolloButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaProtocolloButton.Click
        If Not Me.Nomefile Is Nothing Then
            Dim idRegistrazione As Integer = 0

            If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
                idRegistrazione = Me.TaskAttivo.IdDocumento
            End If

            If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.IOL Then
                Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim pratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                pratiche.Dispose()
                If Not pratica Is Nothing Then
                    'REGISTRAZIONE COLLEGATA
                    idRegistrazione = pratica.IdRegistrazione2
                End If
            End If

            If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
                Dim idSegnalazione = Me.TaskAttivo.IdDocumento
                Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
                Dim segnalazione = segnalazioni.Where(Function(c) c.Id = idSegnalazione)
                If segnalazione.Any Then
                    'REGISTRAZIONE COLLEGATA
                    idRegistrazione = segnalazione.FirstOrDefault.IdRegistrazione
                End If
                segnalazioni.Dispose()
            End If


            'Documento Primario
            Dim allegati As New ParsecPro.AllegatiRepository
            Dim allegato = allegati.GetAllegatiProtocollo(idRegistrazione).Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
            allegati.Dispose()

            Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
            Dim dest = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile

            Dim src As String = percorsoRoot & allegato.PercorsoRelativo & Me.Nomefile


            If IO.File.Exists(src) Then
                IO.File.Copy(src, dest, True)
                IO.File.Delete(src)
            Else
                'Stop
            End If

            Me.Nomefile = Nothing

            Me.RegistraScriptPersistenzaVisibilitaPannello()


            If Me.AutomaticTask = False Then
                Dim script As New StringBuilder
                script.AppendLine("<script>")
                script.AppendLine("UpdateTask();")
                script.AppendLine("</script>")
                ParsecUtility.Utility.RegisterScript(script, False)
            End If

            'AGGIORNO SOLO IL TASK
            Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
            Dim operazione As String = Me.Action.Description.ToUpper
            Me.WriteTask(Me.TaskAttivo.IdMittente, operazione, notificato)


            If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
                'Me.ConvertiDocumentoGenericoInPdf(allegato)
            End If


        End If
    End Sub

    '***************************************************************************************************************
    'Questo pulsante viene pressato dalla finestra popup per aggiornare l'istanza di workflow (case).
    '***************************************************************************************************************
    Protected Sub AggiornaIterButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaIterButton.Click
        'Me.RegistraScriptPersistenzaVisibilitaPannello()
        Me.AggiornaIter()
    End Sub

#End Region

#Region "METODI PRIVATI"

    Public Function GetEnableUiHiddenControl() As String
        Return Me.EnableUiHidden.ClientID
    End Function

    Private Sub ResettaUI()
        Me.VisualizzaDocumentoImageButton.Visible = False
        Me.RiferimentoDocumentoLabel.Text = String.Empty
        Dim list As New List(Of ParsecWKF.Action)
        Me.ToolbarButtonList.DataSource = list
        Me.ToolbarButtonList.DataBind()
        Me.NoteInterneTextBox.Text = String.Empty
    End Sub

    Public Sub InitUI(ByVal idAttoreCorrente As Integer, ByVal taskIdList As List(Of Integer), ByVal idModulo As Integer)

        Me.TitoloLabel.Text = "Operazione Massiva"

        Me.NoteInterneTextBox.Text = String.Empty

        Me.OperazioneMassiva = True
        Me.Nomefile = Nothing
        Me.TaskAttivo = Nothing


        Me.TaskAttivi = Nothing
        Me.RuoloId = Nothing
        Me.Action = Nothing
        Me.MessaggioAvviso = String.Empty

        Me.GetParametri(idAttoreCorrente, taskIdList, idModulo)

        Me.TaskAttivo = Me.TaskAttivi.FirstOrDefault



        Me.InizializzaToolbar()

        Me.VisualizzaNote()
        'Me.RiferimentoDocumentoLabel.Text = Me.TaskAttivo.Documento

        Me.FirstClick = True
        Me.AutomaticTask = False

        Me.DataSources = Nothing

        Me.FileToSignDictionary = Nothing
        Me.BackupFileToDeleteDictionary = Nothing
        Me.ElencoDestinatariDictionary = New Dictionary(Of Integer, Integer)

        Me.VisualizzaDocumentoImageButton.Visible = False
        Me.NoteGridView.Rebind()

        'Me.AllegatiFattura = New List(Of ParsecPro.AllegatoFattura)
        'Me.HtmlFattura = Nothing

        'Me.TitoloElencoNoteLabel.Text = "Storico Note&nbsp;&nbsp;&nbsp;" & If(Me.StoricoNote.Count > 0, "( " & Me.StoricoNote.Count.ToString & " )", "")


    End Sub


    Public Sub InitUI(ByVal idAttoreCorrente As Integer, ByVal idTask As Integer, ByVal idModulo As Integer)

        Me.TitoloLabel.Text = "Operazione"

        Me.NoteInterneTextBox.Text = String.Empty

        Me.OperazioneMassiva = False
        Me.Nomefile = Nothing
        Me.TaskAttivo = Nothing

        Me.TaskAttivi = Nothing

        Me.RuoloId = Nothing
        Me.Action = Nothing
        Me.MessaggioAvviso = String.Empty

        Me.GetParametri(idAttoreCorrente, idTask, idModulo)


        Me.InizializzaToolbar()
        Me.VisualizzaNote()
        Me.RiferimentoDocumentoLabel.Text = Me.TaskAttivo.Documento

        Me.FirstClick = True
        Me.AutomaticTask = False
        Me.DataSources = Nothing

        Me.FileToSignDictionary = Nothing
        Me.BackupFileToDeleteDictionary = Nothing
        Me.ElencoDestinatariDictionary = New Dictionary(Of Integer, Integer)


        Select Case Me.TaskAttivo.IdModulo
            Case ParsecAdmin.TipoModulo.ATT, ParsecAdmin.TipoModulo.CSRA
                Me.VisualizzaDocumentoImageButton.Visible = True
            Case ParsecAdmin.TipoModulo.PRO
                Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault
                documentiGenerici.Dispose()
                Me.VisualizzaDocumentoImageButton.Visible = Not documentoGenerico Is Nothing
        End Select

        Me.NoteGridView.Rebind()


        'Me.AllegatiFattura = New List(Of ParsecPro.AllegatoFattura)
        'Me.HtmlFattura = Nothing


        Me.TitoloElencoNoteLabel.Text = "Storico Note&nbsp;&nbsp;&nbsp;" & If(Me.StoricoNote.Count > 0, "( " & Me.StoricoNote.Count.ToString & " )", "")

    End Sub


    Public Sub InitUI(ByVal idAttoreCorrente As Integer, ByVal task As ParsecWKF.TaskAttivo)


        Me.TitoloLabel.Text = "Operazione"

        Me.NoteInterneTextBox.Text = String.Empty

        Me.OperazioneMassiva = False
        Me.Nomefile = Nothing
        Me.TaskAttivo = Nothing

        Me.TaskAttivi = Nothing

        Me.RuoloId = Nothing
        Me.Action = Nothing
        Me.MessaggioAvviso = String.Empty

        Me.TaskAttivo = task
        Me.TaskAttivo.IdAttoreCorrente = idAttoreCorrente

        Me.InizializzaToolbar()
        Me.VisualizzaNote()
        Me.RiferimentoDocumentoLabel.Text = Me.TaskAttivo.Documento

        Me.FirstClick = True
        Me.AutomaticTask = False
        Me.DataSources = Nothing

        Me.FileToSignDictionary = Nothing
        Me.BackupFileToDeleteDictionary = Nothing
        Me.ElencoDestinatariDictionary = New Dictionary(Of Integer, Integer)


        Select Case Me.TaskAttivo.IdModulo
            Case ParsecAdmin.TipoModulo.ATT, ParsecAdmin.TipoModulo.CSRA
                Me.VisualizzaDocumentoImageButton.Visible = True
            Case ParsecAdmin.TipoModulo.PRO
                Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault
                documentiGenerici.Dispose()
                Me.VisualizzaDocumentoImageButton.Visible = Not documentoGenerico Is Nothing
        End Select

        Me.NoteGridView.Rebind()


        'Me.AllegatiFattura = New List(Of ParsecPro.AllegatoFattura)
        'Me.HtmlFattura = Nothing



        Me.TitoloElencoNoteLabel.Text = "Storico Note&nbsp;&nbsp;&nbsp;" & If(Me.StoricoNote.Count > 0, "( " & Me.StoricoNote.Count.ToString & " )", "")

    End Sub

    Private Sub GetParametri(ByVal IdAttoreCorrente As Integer, ByVal IdTask As Integer, ByVal idModulo As Integer)
        Me.GetTask(IdAttoreCorrente, IdTask, idModulo)
    End Sub

    Private Sub GetParametri(ByVal IdAttoreCorrente As Integer, ByVal taskIdList As List(Of Integer), ByVal idModulo As Integer)
        Me.GetTasks(IdAttoreCorrente, taskIdList, idModulo)
    End Sub

    Private Sub GetTasks(ByVal IdAttoreCorrente As Integer, ByVal taskIdList As List(Of Integer), ByVal idModulo As Integer)
        Dim tasks As New ParsecWKF.TaskRepository
        Dim filtro As New ParsecWKF.TaskFiltro
        filtro.IdUtente = IdAttoreCorrente
        filtro.IdModulo = idModulo
        Me.TaskAttivi = tasks.GetView(filtro).Where(Function(c) taskIdList.Contains(c.Id)).ToList
        For Each t In Me.TaskAttivi
            t.IdAttoreCorrente = IdAttoreCorrente
        Next
        tasks.Dispose()
    End Sub

    Private Sub GetTask(ByVal IdAttoreCorrente As Integer, ByVal IdTask As Integer, ByVal idModulo As Integer)
        Dim tasks As New ParsecWKF.TaskRepository
        Dim filtro As New ParsecWKF.TaskFiltro
        filtro.IdUtente = IdAttoreCorrente
        filtro.Id = IdTask
        filtro.IdModulo = idModulo
        Me.TaskAttivo = tasks.GetView(filtro).FirstOrDefault
        Me.TaskAttivo.IdAttoreCorrente = IdAttoreCorrente
        tasks.Dispose()
    End Sub

    Private Sub InizializzaToolbar()
        Dim list As List(Of ParsecWKF.Action) = ParsecWKF.ModelloInfo.ReadActionInfo(Me.TaskAttivo.TaskCorrente, Me.TaskAttivo.NomeFileIter)

        Me.ToolbarButtonList.DataSource = list
        Me.ToolbarButtonList.DataBind()
        '*******************************************************************************************************************************
        'Il pulsante per la visualizzazione dello storico delle pratiche è visibile solo se è presente l'azione verifica istanza
        '*******************************************************************************************************************************
        Dim azioneVerifica As ParsecWKF.Action = list.Where(Function(c) c.Description.Trim.ToLower = "verifica istanza").FirstOrDefault
        If azioneVerifica Is Nothing Then
            Me.VisualizzaStoricoPraticheImageButton.Visible = False
        End If
        '*******************************************************************************************************************************
    End Sub

    Private Sub VisualizzaNote()
        Dim tasks As New ParsecWKF.TaskRepository

        'TASK PRECEDENTE
        Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdPadre).FirstOrDefault
        If Not task Is Nothing Then
            Me.NotePresentiTextBox.Text = task.Note
            '    'Else
            'Me.NotePresentiTextBox.Text = Me.TaskAttivo.Note
        End If

    End Sub

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

    Private Sub VisualizzaImpostaAmmissibilita()
        Dim parametriProcesso As List(Of ParsecWKF.ParametroProcesso) = Nothing
        Dim parametri As New ParsecWKF.ParametriProcessoRepository
        Dim queryString As New Hashtable
        parametriProcesso = parametri.GetQuery.Where(Function(c) c.IdProcesso = Me.TaskAttivo.IdIstanza).ToList
        parametri.Dispose()
        If parametriProcesso.Count = 0 Then
            parametriProcesso = ParsecWKF.ModelloInfo.ReadParameters(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
        Else
            queryString.Add("Mode", "Update")
        End If
        Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
        queryString.Add("obj", Me.AggiornaIterButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("ParametriProcesso", parametriProcesso)
        parametriPagina.Add("IdProcesso", Me.TaskAttivo.IdIstanza)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 500, 200, queryString, False)
    End Sub

    Private Function SplitQueryString(queryString As String) As Hashtable
        Dim res As New Hashtable
        If queryString.Contains("?") Then
            queryString = queryString.Substring(queryString.IndexOf("?") + 1)
        End If

        For Each token As String In System.Text.RegularExpressions.Regex.Split(queryString, "&")
            Dim keyValue = System.Text.RegularExpressions.Regex.Split(token, "=")
            If keyValue.Length = 2 Then
                res.Add(keyValue(0), keyValue(1))
            Else
                res.Add(keyValue(0), "")
            End If
        Next
        Return res
    End Function

    Private Function AggiornaModello(doc As ParsecAtt.Documento) As ParsecAtt.Documento

        Dim documenti As New ParsecAtt.DocumentoRepository

        '********************************************************************************************
        'E' sempre un nuovo documento.
        '********************************************************************************************
        Dim documento As New ParsecAtt.Documento
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Select Case CType(doc.IdTipologiaDocumento, ParsecAtt.TipoDocumento)
            Case ParsecAtt.TipoDocumento.PropostaOrdinanza
                documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza
            Case ParsecAtt.TipoDocumento.PropostaDetermina
                documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina
            Case ParsecAtt.TipoDocumento.PropostaDelibera
                documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera
            Case ParsecAtt.TipoDocumento.PropostaDecreto
                documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto
        End Select
        documento.IdTipologiaDocumento = CInt(documento.TipologiaDocumento)

        documento.ContatoreStruttura = doc.ContatoreStruttura

        documento.Riservato = doc.Riservato

        documento.IdStruttura = doc.IdStruttura
        documento.IdUfficio = doc.IdUfficio
        documento.Oggetto = doc.Oggetto
        documento.Note = doc.Note
        documento.EsecutivitaImmediata = True

        documento.LogIdUtente = utenteCorrente.Id
        documento.LogUtente = utenteCorrente.Username
        documento.IdAutore = utenteCorrente.Id


        documento.IdPadre = doc.Id
        documento.IdTipologiaSeduta = doc.IdTipologiaSeduta

        'documento.DataDocumento = Now
        Dim parametri As New ParsecAdmin.ParametriRepository
        documento.DataDocumento = parametri.GetDataValida
        parametri.Dispose()

        '********************************************************************************************
        'AGGIORNO LA SEDUTA DEL DOCUMENTO
        '********************************************************************************************

        If doc.IdSeduta.HasValue Then
            documento.IdSeduta = doc.IdSeduta
            documento.DataDocumento = doc.Seduta.DataConvocazione
            If doc.IdStatoDiscussione.HasValue Then
                documento.IdStatoDiscussione = doc.IdStatoDiscussione
            End If
        End If

        '********************************************************************************************


        '********************************************************************************************
        'ORDINANZA E DECRETO
        '********************************************************************************************

        documento.NumeroProtocollo = doc.NumeroProtocollo
        documento.DataOraRegistrazione = doc.DataOraRegistrazione

        '********************************************************************************************

        '********************************************************************************************
        'DELIBERA  DETERMINA ORDINANZA E DECRETO
        '********************************************************************************************
        documento.DataAffissioneDa = doc.DataEsecutivaDa
        documento.GiorniAffissione = doc.GiorniAffissione
        documento.NumeroRegistroPubblicazione = doc.NumeroRegistroPubblicazione

        '********************************************************************************************

        '********************************************************************************************
        'OGGETTI COLLEGATI
        '********************************************************************************************
        documento.Seduta = doc.Seduta
        documento.Classificazioni = doc.Classificazioni
        documento.ImpegniSpesa = doc.ImpegniSpesa
        documento.Accertamenti = doc.Accertamenti
        documento.Liquidazioni = doc.Liquidazioni
        documento.Presenze = doc.Presenze
        documento.Firme = doc.Firme
        documento.DocumentoCollegato = doc
        documento.DocumentoGenerato = Nothing
        documento.Trasparenza = doc.Trasparenza
        documento.Visibilita = doc.Visibilita
        documento.Fascicoli = doc.Fascicoli

        If Not documento.Trasparenza Is Nothing Then
            documento.Trasparenza.DataInizioPubblicazione = Now
            documento.Trasparenza.DataFinePubblicazione = If(Now.DayOfYear = 1, New DateTime(Now.Year, 12, 31).AddYears(4), New DateTime(Now.Year, 12, 31).AddYears(5))
        End If

        '********************************************************************************************
        'AGGIORNO IL MODELLO DEL DOCUMENTO
        '********************************************************************************************
        Dim modelli As New ParsecAtt.ModelliRepository
        If Not doc.Modello Is Nothing Then
            documento.Modello = modelli.GetById(doc.Modello.IdModelloAdottato)
            documento.IdModello = documento.Modello.Id
        End If
        modelli.Dispose()
        '********************************************************************************************

        'Copio gli allegati
        documento.Allegati = New List(Of ParsecAtt.Allegato)
        Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        pathRoot = pathRoot.Remove(pathRoot.Length - 1, 1)
        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim all As ParsecAtt.Allegato = Nothing
        For Each allegato In doc.Allegati
            Dim filename As String = String.Empty
            Dim filenameTemp As String = String.Empty
            If allegato.Id < 0 Then
                filename = allegato.Nomefile
                filenameTemp = allegato.NomeFileTemp
            Else
                filename = "clone_" & allegato.Nomefile
                filenameTemp = "clone_" & allegato.Nomefile

                allegato.Id = -1
                If documento.Allegati.Count > 0 Then
                    Dim allId = documento.Allegati.Min(Function(c) c.Id) - 1
                    If allId < 0 Then
                        allegato.Id = allId
                    End If
                End If

            End If
            all = New ParsecAtt.Allegato
            all.PercorsoRoot = pathRoot
            all.PercorsoRootTemp = pathRootTemp
            all.Nomefile = filename
            all.NomeFileTemp = filenameTemp
            all.Oggetto = allegato.Oggetto
            all.Impronta = allegato.Impronta
            all.Pubblicato = allegato.Pubblicato
            all.Id = allegato.Id
            all.IdFatturaElettronica = allegato.IdFatturaElettronica

            Dim input As String = pathRoot & allegato.PercorsoRelativo & allegato.Nomefile
            Dim output As String = pathRootTemp & filenameTemp
            Me.CopiaDocumento(input, output)

            If Not String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
                all.NomeFileFirmato = "clone_" & allegato.NomeFileFirmato
            End If

            documento.Allegati.Add(all)
        Next
        '********************************************************************************************

        Return documento

    End Function

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

    Private Function GetIdDestinatario() As Integer

        Dim idDestinatario As Integer = 0

        If Not Me.RuoloId Is Nothing Then
            If Me.RuoloId <> 0 Then
                idDestinatario = Me.RuoloId
                Me.RuoloId = Nothing
                Return idDestinatario
            End If
        End If


        'Recupero il nome del ruolo (To) associato all'azione.
        Dim roleToName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, Me.Action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Receiver).FirstOrDefault.Name

        Dim processi As New ParsecWKF.ParametriProcessoRepository
        Dim parametroProcesso As ParsecWKF.ParametroProcesso = processi.GetQuery.Where(Function(c) c.IdProcesso = Me.TaskAttivo.IdIstanza And c.Nome = roleToName).FirstOrDefault
        processi.Dispose()
        If Not parametroProcesso Is Nothing Then
            idDestinatario = CInt(parametroProcesso.Valore)
        Else
            Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleToName).FirstOrDefault
            Dim idRuolo As Integer
            'Se il ruolo non è presente
            If Not role Is Nothing Then
                idRuolo = role.Id
                idDestinatario = (New ParsecWKF.RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault.IdUtente
            End If
            'Throw New ApplicationException("L'attore '" & roleToName & "' non è presente nell'archivio!")
        End If

        '****************************************************************************************
        'Gestione con i ruoli
        '****************************************************************************************
        'If Me.Action.FromActor <> Me.Action.ToActor Then
        '    Dim roleToName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, Me.Action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Receiver).FirstOrDefault.Name
        '    Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleToName).FirstOrDefault
        '    Dim idRuolo As Integer
        '    'Se il ruolo non è presente
        '    If Not role Is Nothing Then
        '        idRuolo = role.Id
        '        idDestinatario = (New ParsecWKF.RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault.IdUtente
        '    End If
        'Else
        '    idDestinatario = Me.TaskAttivo.IdMittente
        'End If
        '****************************************************************************************

        Return idDestinatario
    End Function

    Private Sub AggiornaIter()
        Select Case Me.TaskAttivo.IdModulo
            Case ParsecAdmin.TipoModulo.ATT
                Me.AggiornaIterATT()
            Case ParsecAdmin.TipoModulo.PRO
                Me.AggiornaIterPRO()
            Case ParsecAdmin.TipoModulo.PED
                Me.AggiornaIterSUE()
            Case ParsecAdmin.TipoModulo.IOL
                Me.AggiornaIterIOL()
            Case ParsecAdmin.TipoModulo.CSRA
                Me.AggiornaIterCSRA()
            Case ParsecAdmin.TipoModulo.WBT
                Me.AggiornaIterWBT()
        End Select
    End Sub

 

    Private Sub AggiornaIterATT()


        Select Case Me.Action.Type
            Case "FIRMA"
                Select Case Me.signerOutputHidden.Value.ToUpper



                    Case "OK"
                        Dim idDestinatario As Integer = Me.GetIdDestinatario()

                        Me.FirstClick = True

                        'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
                        Me.WriteTaskAndUpdateParent(idDestinatario)

                        '*******************************************************************************
                        'CANCELLO I DOCUMENTI DI BACKUP
                        '*******************************************************************************
                        If Not Me.BackupFileToDeleteDictionary Is Nothing Then
                            Dim backFileToDelete = Me.BackupFileToDeleteDictionary.Where(Function(c) c.Key = Me.TaskAttivo.Id).FirstOrDefault.Value
                            If IO.File.Exists(backFileToDelete) Then
                                IO.File.Delete(backFileToDelete)
                            End If
                            BackupFileToDeleteDictionary = Nothing
                        End If
                        '*******************************************************************************


                    Case "ERRORE"

                        '******************************************************************************
                        'ANNULLO LE INFORMAZIONI DELLA FIRMA
                        '******************************************************************************
                        If Not Me.OperazioneMassiva Then
                            Me.AnnullaFirma()
                        Else
                            'ANNULLAMENTO FIRMA MASSIVA
                            For Each taskCorrente In Me.TaskAttivi
                                Me.TaskAttivo = taskCorrente
                                Me.AnnullaFirma()
                            Next
                        End If

                        '******************************************************************************
                        'VERRA' CLICCATO DAL PULSANTE AnnullaFirmaDocumentoButton
                        'ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
                        '******************************************************************************

                    Case Else  'OPERAZIONE MASSIVA

                        '*******************************************************************************
                        'Leggo i valori restituiti dalla firma massiva
                        '*******************************************************************************
                        Dim output = System.Convert.FromBase64String(Me.signerOutputHidden.Value)
                        Dim ms As New IO.MemoryStream(output)
                        Dim ds As New DataSet
                        ds.ReadXml(ms)
                        Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
                        Dim operazione As String = Me.Action.Description.ToUpper

                        For Each row As DataRow In ds.Tables(0).Rows
                            Dim idTask As Integer = CInt(row("RowId"))
                            Dim result As Boolean = CBool(row("Result"))
                            Me.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = idTask).FirstOrDefault

                            '*******************************************************************************
                            'CANCELLO I DOCUMENTI DI BACKUP
                            '*******************************************************************************
                            If Not Me.BackupFileToDeleteDictionary Is Nothing Then
                                Dim backFileToDelete = Me.BackupFileToDeleteDictionary.Where(Function(c) c.Key = Me.TaskAttivo.Id).FirstOrDefault.Value
                                If IO.File.Exists(backFileToDelete) Then
                                    IO.File.Delete(backFileToDelete)
                                End If
                                BackupFileToDeleteDictionary = Nothing
                            End If
                            '*******************************************************************************

                            If result Then
                                Dim idDestinatario = Me.ElencoDestinatariDictionary.Where(Function(c) c.Key = idTask).FirstOrDefault.Value
                                Me.WriteTask(idDestinatario, operazione, notificato)
                            Else
                                Me.AnnullaFirma()
                            End If
                        Next

                        '**************************************************************************************
                        'NOTIFICO GLI ERRORI RISCONTRATI DURANTE LA VERIFICA MASSIVA DEL CORPO DEL DOCUMENTO
                        '**************************************************************************************
                        If Not String.IsNullOrEmpty(Me.MessaggioAvviso) Then
                            Me.RegistraScriptPersistenzaVisibilitaPannello()
                            ParsecUtility.Utility.MessageBox(Me.MessaggioAvviso, False)
                        End If
                        Me.MessaggioAvviso = Nothing
                        '**************************************************************************************

                        Me.FirstClick = False
                        'NON FUNZIONA SU CHROME
                        Me.RegistraButtonClick()

                        ''Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
                        'Me.WriteAllTasksAndUpdateParent(Me.ElencoDestinatariDictionary)
                        Me.ElencoDestinatariDictionary = Nothing

                End Select
                Me.signerOutputHidden.Value = String.Empty

            Case "NUMERAZIONE"

                If Not ParsecUtility.SessionManager.Documento Is Nothing Then
                    ParsecUtility.SessionManager.Documento = Nothing
                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                End If


            Case "MODIFICA"

                'AGGIUNGO LA PROPOSTA DI DELIBERA ALL'ORDINE DEL GIORNO DELLA SEDUTA SELEZIONATA

                Dim aggiungiAOrdineDelGiorno = Me.Action.GetParameterByName("AggiungiAOrdineDelGiorno")
                If Not aggiungiAOrdineDelGiorno Is Nothing Then
                    If Not ParsecUtility.SessionManager.Seduta Is Nothing Then

                        Dim seduta As ParsecAtt.Seduta = CType(ParsecUtility.SessionManager.Seduta, ParsecAtt.Seduta)

                        Dim idSeduta As Integer = seduta.Id

                        Dim documenti As New ParsecAtt.DocumentoRepository
                        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                        documento.IdSeduta = idSeduta
                        documenti.SaveChanges()
                        documenti.Dispose()


                        Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
                        Dim elementoOrdineGiorno As New ParsecAtt.OrdineGiorno
                        Dim ordinale = ordiniGiorno.GetQuery.Where(Function(c) c.IdSeduta = idSeduta).Max(Function(c) c.Ordinale)

                        If ordinale.HasValue Then
                            ordinale += 1
                        Else
                            ordinale = 1
                        End If

                        elementoOrdineGiorno.Ordinale = ordinale
                        elementoOrdineGiorno.IdSeduta = idSeduta
                        elementoOrdineGiorno.IdStatoDiscussione = 1 'approvata
                        elementoOrdineGiorno.Oggetto = documento.Oggetto
                        elementoOrdineGiorno.IdUfficio = documento.IdUfficio
                        elementoOrdineGiorno.IdStruttura = documento.IdStruttura
                        ordiniGiorno.Add(elementoOrdineGiorno)
                        ordiniGiorno.SaveChanges()
                        ordiniGiorno.Dispose()

                        ParsecUtility.SessionManager.Seduta = Nothing
                        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)

                    Else
                        Me.RegistraButtonClick()
                    End If
                    Exit Sub
                End If

                If Not ParsecUtility.SessionManager.Documento Is Nothing Then
                    ParsecUtility.SessionManager.Documento = Nothing
                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                Else
                    'NEL CASO SI VERIFICHI UN ERRORE DURANTE LA PUBBLICAZIONE.
                    'CHIUDO LA FINESTRA PER AGGIORNARE IL RIFERIMENTO AL DOCUMENTO

                    Me.RegistraButtonClick()
                End If






            Case "CANCELLA"
                If Not ParsecUtility.SessionManager.Documento Is Nothing Then
                    ParsecUtility.SessionManager.Documento = Nothing
                    Dim istanze As New ParsecWKF.IstanzaRepository
                    istanze.AnnullaTask(Me.TaskAttivo.IdIstanza)
                    istanze.Dispose()
                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                End If

        End Select


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.SbloccaTasks(utenteCollegato.Id)
        Me.SbloccaDocumenti(utenteCollegato.Id)
    End Sub

    Private Sub Modifica(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.ATT Then
            Me.ModificaATT(e)
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
            Me.ModificaPRO(e)
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.IOL Then
            Me.ModificaIOL(e)
        End If


        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.CSRA Then
            Me.VisualizzaSchedaControlloSuccessivo()
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
            Me.ModificaWBT(e)
        End If



    End Sub


    Private Sub Cancella(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)


        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.ATT Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
            documenti.Dispose()
            If Not documento Is Nothing Then
                Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, e.CommandName, Me.TaskAttivo.NomeFileIter)
                Dim queryString As New Hashtable
                queryString.Add("Tipo", documento.IdTipologiaDocumento.ToString)
                queryString.Add("Iter", "1")
                queryString.Add("Mode", "Delete")
                queryString.Add("obj", Me.AggiornaIterButton.ClientID)

                Dim qs As String() = pageUrl.Split("?")
                Dim chiaveValore = qs(1).Split("=")
                queryString.Add(chiaveValore(0), chiaveValore(1))

                Dim parametriPagina As New Hashtable
                parametriPagina.Add("IdDocumentoIter", Me.TaskAttivo.IdDocumento)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(qs(0), 930, 700, queryString, False)
            End If
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
            VisualizzaProtocolloInModifica()
        End If



    End Sub

    Private Sub Firma(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.ATT Then


            Dim abilitaControlloImpegniSpesa As Boolean = Not Me.Action.GetParameterByName("AbilitaControlloImpegniSpesa") Is Nothing

            If abilitaControlloImpegniSpesa Then
                'Dim documenti As New ParsecAtt.DocumentoRepository
                'Dim impegni = documenti.GetImpegniSpesa(Me.TaskAttivo.IdDocumento)
                'documenti.Dispose()

                Dim impegniSpesa As New ParsecAtt.ImpegnoSpesaRepository
                Dim impegni = impegniSpesa.Where(Function(c) c.IdDocumento = Me.TaskAttivo.IdDocumento)

                If Not impegni.Any Then
                    Throw New ApplicationException("Impossibile eseguire la FIRMA" & vbCrLf & "Specificare almeno un impegno di spesa!")
                Else
                    Dim impegniSpesaSenzaNumeroImpegno = impegni.Where(Function(c) c.NumeroImpegno Is Nothing Or String.IsNullOrEmpty(c.NumeroImpegno)).Any
                    If impegniSpesaSenzaNumeroImpegno Then
                        Throw New ApplicationException("Impossibile eseguire la FIRMA" & vbCrLf & "Specificare il numero di impegno!")
                    End If
                End If

            End If


            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

            'SOSPESO
            'If Me.TaskAttivo.IdAttoreCorrente <> utenteCollegato.Id Then
            '    Dim abilitatoFirmaInDelega As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AbilitaFirmaInDelega)).FirstOrDefault Is Nothing
            '    If Not abilitatoFirmaInDelega Then

            '        'Dim btn As RadButton = e.Item.FindControl("ExecuteTaskButton")
            '        'Dim sender As String = btn.Attributes("Sender")

            '        Dim message As New StringBuilder
            '        message.AppendLine("Attenzione!")
            '        message.AppendLine("Non si possiedono le autorizzazioni per poter firmare in luogo del " & Me.Action.FromActor & ".")
            '        Throw New ApplicationException(message.ToString)
            '    End If
            'End If




            Dim abilitaControlloCorpoDocumento As Boolean = Not Me.Action.GetParameterByName("AbilitaControlloCorpoDocumento") Is Nothing
            If abilitaControlloCorpoDocumento Then
                'UTILIZZO L'APPLET
                If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                    Me.RegistraScriptVerificaCorpoOpenOffice(Me.verificaCorpoDocumentoButton.ClientID, False)
                Else
                    'UTILIZZO IL SOCKET
                    Me.RegistraScriptVerificaCorpoOpenOffice(AddressOf Me.VerificaCorpoDocumentoCallback, False)
                End If
            Else
                Me.FirmaATT(e)
            End If
        End If
        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
            Me.FirmaPRO(e)
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.IOL Then
            Me.FirmaIOL(e)
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.CSRA Then
            Me.FirmaCSRA(e)
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
            Me.FirmaWBT(e)
        End If

    End Sub

    Private Function GetAnnoEsercizio() As Integer
        Dim annoEsercizio As Integer = Now.Year
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AnnoCorrente", ParsecAdmin.TipoModulo.ATT)
        If Not parametro Is Nothing Then
            annoEsercizio = CInt(parametro.Valore)
        End If
        parametri.Dispose()
        Return annoEsercizio
    End Function

    Private Function CreaSeduta(ByVal documento As ParsecAtt.Documento) As Integer

        Dim idSeduta As Integer = 0

        Dim sedute As New ParsecAtt.SedutaRepository
        Dim dataDocumento As DateTime = Nothing
        'dataDocumento = documento.DataProposta.Value
        Dim tipo = documento.Modello.IdTipologiaSeduta


        'Dim documenti As New ParsecAtt.DocumentoRepository
        'Dim annoEsercizio As Integer = Me.GetAnnoEsercizio
        'Dim maxData As Nullable(Of Date) = documenti.GetQuery.Where(Function(c) c.IdTipologiaDocumento = 4 And c.Data.Value.Year = annoEsercizio).Select(Function(c) c.Data).Max

        'If maxData.HasValue Then
        '    dataDocumento = maxData
        'Else
        dataDocumento = Now
        'End If

        Dim startDate As Date = New Date(dataDocumento.Year, dataDocumento.Month, dataDocumento.Day, 0, 0, 0)
        Dim endDate As Date = New Date(dataDocumento.Year, dataDocumento.Month, dataDocumento.Day, 23, 59, 59)

        'OTTENGO LA SEDUTA DI OGGI O CON DATA >= ALLA DATA DELLA DELIBERA PIU' RECENTE
        Dim seduta = sedute.GetQuery.Where(Function(c) c.IdTipologiaSeduta = tipo And c.DataPrimaConvocazione.Value >= startDate And c.DataPrimaConvocazione.Value <= endDate).FirstOrDefault

        If seduta Is Nothing Then

            'AGGIUNGO UNA SEDUTA
            Dim nuovaSeduta As New ParsecAtt.Seduta
            nuovaSeduta.IdTipologiaSeduta = documento.Modello.IdTipologiaSeduta
            nuovaSeduta.DataPrimaConvocazione = Now
            nuovaSeduta.OrarioPrimaConvocazione = Now
            nuovaSeduta.IdTipologiaConvocazione = 1 '1=ordinaria 2 =Straordinaria
            nuovaSeduta.PrimaConvocazione = True

            Dim tipoSeduta As ParsecAtt.TipologiaOrganoDeliberante = CType(nuovaSeduta.IdTipologiaSeduta, ParsecAtt.TipologiaOrganoDeliberante)
            Dim valore As String = String.Empty
            Select Case tipoSeduta
                Case ParsecAtt.TipologiaOrganoDeliberante.GiuntaComunale
                    valore = "codStrutturaRuoloAssessore"
                Case ParsecAtt.TipologiaOrganoDeliberante.ConsiglioComunale
                    valore = "codStrutturaRuoloConsigliere"
                Case ParsecAtt.TipologiaOrganoDeliberante.CommissarioPrefettizio

                Case ParsecAtt.TipologiaOrganoDeliberante.SubCommissarioPrefettizio

            End Select

            If Not String.IsNullOrEmpty(valore) Then
                Dim parametri As New ParsecAdmin.ParametriRepository
                Dim parametro As ParsecAdmin.Parametri = parametri.GetByName(valore, ParsecAdmin.TipoModulo.SEP)
                parametri.Dispose()

                Dim presenze As New ParsecAtt.PresenzaSedutaRepository
                Dim strutturaPadre As ParsecAdmin.Struttura = (New ParsecAdmin.StructureRepository).GetQuery.Where(Function(c) c.Codice = CInt(parametro.Valore) And c.LogStato Is Nothing).FirstOrDefault
                nuovaSeduta.Presenze = presenze.GetDefaultView(New ParsecAtt.FiltroPresenzaSeduta With {.IdPadre = strutturaPadre.Id})
                presenze.Dispose()
            Else
                nuovaSeduta.Presenze = New List(Of ParsecAtt.PresenzaSeduta)
            End If

            sedute.Save(nuovaSeduta)
            idSeduta = nuovaSeduta.Id

        Else
            idSeduta = seduta.Id

        End If


        'Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
        'Dim elementoOrdineGiorno As New ParsecAtt.OrdineGiorno
        'Dim ordinale = ordiniGiorno.GetQuery.Where(Function(c) c.IdSeduta = idSeduta).Max(Function(c) c.Ordinale + 1)

        'elementoOrdineGiorno.Ordinale = ordinale
        'elementoOrdineGiorno.IdSeduta = idSeduta
        'elementoOrdineGiorno.IdStatoDiscussione = 1 'approvata
        'elementoOrdineGiorno.Oggetto = documento.Oggetto
        'elementoOrdineGiorno.IdUfficio = documento.IdUfficio
        'elementoOrdineGiorno.IdStruttura = documento.IdStruttura
        'ordiniGiorno.Add(elementoOrdineGiorno)
        'ordiniGiorno.SaveChanges()

        'ordiniGiorno.Dispose()
        sedute.Dispose()

        Return idSeduta
    End Function

    Private Function GetAddetti(ByVal idResponsabile As Integer) As List(Of ParsecAdmin.Utente)

        Dim addetti As New ParsecAdmin.AddettiRepository
        Dim utenti As New ParsecAdmin.UserRepository(addetti.Context)

        Dim result = (From addetto In addetti.GetQuery
                  Join utente In utenti.GetQuery
                  On addetto.IdAddetto Equals utente.Id
                  Where addetto.IdResponsabile = idResponsabile And utente.LogTipoOperazione Is Nothing
                  Select utente).AsEnumerable.Select(Function(c) New ParsecAdmin.Utente With {
                                                         .Id = c.Id,
                                                         .Descrizione = c.Username.ToUpper & " - " & If(c.Titolo Is Nothing, "", c.Titolo.ToUpper) & " " & c.Cognome.ToUpper & " " & c.Nome
                                                     }).OrderBy(Function(c) c.Descrizione)


        Return result.ToList

    End Function

    Private Function GetUtentiRuolo(ByVal idRuolo As Integer) As List(Of ParsecAdmin.Utente)
        Dim ruoli As New ParsecWKF.RuoloRelUtenteRepository
        Dim utenti As New ParsecWKF.UtenteViewRepository(ruoli.Context)


        Dim result = (From ruolo In ruoli.GetQuery
                     Join utente In utenti.GetQuery
                     On utente.Id Equals ruolo.IdUtente
                     Where ruolo.IdRuolo = idRuolo And utente.LogTipoOperazione Is Nothing
                     Select utente).AsEnumerable.Select(Function(c) New ParsecAdmin.Utente With {
                                                            .Id = c.Id,
                                                            .Descrizione = c.Username.ToUpper & " - " & If(c.Titolo Is Nothing, "", c.Titolo.ToUpper) & " " & c.Cognome.ToUpper & " " & c.Nome
                                                        }).OrderBy(Function(c) c.Descrizione).ToList


        Return result.ToList

    End Function


    Private Function OperazioneMassivaConsentita(ByVal action As ParsecWKF.Action) As Boolean

        If Me.OperazioneMassiva Then
            ' Dim taskSuccessivoAutomatico As ParsecWKF.ParametroProcesso = action.Parameters.Where(Function(c) c.Nome = "TaskSuccessivoAutomatico").FirstOrDefault
            Dim nascondiDocumento As ParsecWKF.ParametroProcesso = action.Parameters.Where(Function(c) c.Nome = "NascondiDocumento").FirstOrDefault
            Dim parameterCount As Integer = action.Parameters.Count

            Select Case action.Type.ToUpper

                Case "NUMERAZIONE"
                    Dim visualizzaDocumento As Boolean = False

                    Dim parametri As New ParsecAdmin.ParametriRepository
                    'Determino se visualizzare il documento dopo la numerazione
                    Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("ModificheDuranteTrasformaProposta", ParsecAdmin.TipoModulo.ATT)
                    If Not parametro Is Nothing Then
                        visualizzaDocumento = CBool(parametro.Valore)
                    End If
                    parametri.Dispose()
                    'NUMERAZIONE SENZA PARAMETRI (VISUALIZZA LA RISORSA PER CUI NON E' POSSIBILE ESEGUIRE QUESTA OPERAZIONE IN MODALITA' MASSIVA)
                    If parameterCount > 0 Then
                        'If taskSuccessivoAutomatico Is Nothing And Not visualizzaDocumento Then
                        If Not visualizzaDocumento Then
                            Return True
                        End If
                    End If

                Case "FIRMA"
                    'If taskSuccessivoAutomatico Is Nothing Then
                    Return True
                    'End If
                Case "MODIFICA"

                    'SE STO ESPORTANDO LE FATTURE
                    If Not action.GetParameterByName("EsportaFattura") Is Nothing AndAlso Not action.GetParameterByName("UsaFtp") Is Nothing Then
                        Return True
                    End If

            End Select

            'If (action.Type.ToUpper = "INVIA AVANTI" AndAlso taskSuccessivoAutomatico Is Nothing) OrElse action.Type.ToUpper = "FINE" Then
            If (action.Type.ToUpper = "INVIA AVANTI") OrElse action.Type.ToUpper = "FINE" Then
                Return True
            End If
        Else
            Return True
        End If
        Return False
    End Function

#End Region

#Region "SCRIPT PARSECOPENOFFICE"

  
    Private Sub RegistraScriptOpenOffice(ByVal documento As ParsecAtt.Documento, ByVal buttonToClick As String)
        If Not documento Is Nothing Then
            If Not String.IsNullOrEmpty(documento.DataSource) Then
                Me.RegistraScriptPersistenzaVisibilitaPannello()
                ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(documento.DataSource, buttonToClick, Me.documentContentHidden.ClientID, False, False)
                documento.DataSource = ""
            End If
        End If
    End Sub


    Private Sub RegistraScriptOpenOffice(ByVal documento As ParsecAtt.Documento, ByVal callBack As System.Action)
        If Not documento Is Nothing Then
            If Not String.IsNullOrEmpty(documento.DataSource) Then
                Me.RegistraScriptPersistenzaVisibilitaPannello()
                ParsecUtility.Utility.EseguiServerComunicatorService(documento.DataSource, True, Sub(data) Me.documentContentHidden.Value = data, callBack)
                documento.DataSource = ""
            End If
        End If
    End Sub


    Private Function GetScriptVerificaCorpoOpenOffice(ByVal operazioneMassiva As Boolean) As String
        Dim openOfficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim modelli As New ParsecAtt.ModelliRepository
        Dim datiToCheck As New ParsecAdmin.DatiVerifica
        Dim documento As ParsecAtt.Documento = Nothing
        Dim modello As ParsecAtt.Modello = Nothing
        Dim remotePathTemplate As String = String.Empty
        Dim remotePathToCheck As String = String.Empty

        If operazioneMassiva Then
            For Each taskCorrente In Me.TaskAttivi
                documento = documenti.GetById(taskCorrente.IdDocumento)
                Dim annoEsercizio = documento.DataDocumento.Value.Year.ToString

                Dim proposta As ParsecAtt.Documento = Nothing
                If documento.IdPadre.HasValue Then
                    proposta = documenti.GetDocumentoCollegato(documento.IdPadre)
                End If
                If documento.NumVersione = 0 And Not proposta Is Nothing Then
                    modello = modelli.GetQuery.Where(Function(c) c.Id = proposta.IdModello).FirstOrDefault
                Else
                    modello = modelli.GetQuery.Where(Function(c) c.Id = documento.IdModello).FirstOrDefault
                End If

                remotePathTemplate = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates") & modello.FileName
                remotePathToCheck = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, documento.Nomefile)
                datiToCheck.RowInfos.Add(New ParsecAdmin.VerificaInfo With {.RowId = taskCorrente.Id, .RemotePathTemplate = remotePathTemplate, .RemotePathToCheck = remotePathToCheck})
            Next
        Else
            documento = documenti.GetById(Me.TaskAttivo.IdDocumento)
            Dim annoEsercizio = documento.DataDocumento.Value.Year.ToString
            Dim proposta As ParsecAtt.Documento = Nothing
            If documento.IdPadre.HasValue Then
                proposta = documenti.GetDocumentoCollegato(documento.IdPadre)
            End If
            If documento.NumVersione = 0 And Not proposta Is Nothing Then
                modello = modelli.GetQuery.Where(Function(c) c.Id = proposta.IdModello).FirstOrDefault
            Else
                modello = modelli.GetQuery.Where(Function(c) c.Id = documento.IdModello).FirstOrDefault
            End If


            remotePathTemplate = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates") & modello.FileName
            remotePathToCheck = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, documento.Nomefile)
            datiToCheck.RowInfos.Add(New ParsecAdmin.VerificaInfo With {.RowId = Me.TaskAttivo.Id, .RemotePathTemplate = remotePathTemplate, .RemotePathToCheck = remotePathToCheck})
        End If

        documenti.Dispose()
        modelli.Dispose()

        Dim pathExe As String = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(ParsecAdmin.WebConfigSettings.GetKey("PathParsecOpenOffice") & "ParsecOpenOffice.exe"))
        Dim datiInput As New DatiInput With {.FunctionName = "CheckDocumentMassiva", .PathExe = pathExe, .ShowWindow = False, .Enabled = True}


    

        Dim data = openOfficeParameters.CreateDataSource(datiInput, datiToCheck)
        Return data
    End Function

    Private Sub RegistraScriptVerificaCorpoOpenOffice(ByVal buttonToClick As String, ByVal operazioneMassiva As Boolean)
        Dim data = GetScriptVerificaCorpoOpenOffice(operazioneMassiva)
        Me.RegistraScriptPersistenzaVisibilitaPannello()
        ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(data, buttonToClick, Me.checkContentHidden.ClientID, False, False)
    End Sub

    Private Sub RegistraScriptVerificaCorpoOpenOffice(ByVal callback As System.Action, ByVal operazioneMassiva As Boolean)
        Dim data = GetScriptVerificaCorpoOpenOffice(operazioneMassiva)
        Me.RegistraScriptPersistenzaVisibilitaPannello()
        ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.checkContentHidden.Value = c, callback)
    End Sub

    Private Sub RegistraScriptPersistenzaVisibilitaPannello()
        Dim script As New StringBuilder
        script.AppendLine("<script>")
        script.AppendLine("ShowOverlay();currentPanel=0;ShowPanel(currentPanel);hide=false;ShowControlPanel();")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
    End Sub

    Private Sub VisualizzaDocumento(ByVal nomeFile As String, ByVal anno As String, ByVal enabled As Boolean, ByVal sorgente As ParsecAdmin.SourceElement)

        Dim pathDownload As String = String.Empty
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim datiInput As ParsecAdmin.DatiInput = Nothing
        Dim data As String = String.Empty
        Dim functionName As String = "ViewDocument"

        Select Case sorgente.Tipologia
            Case 1 'Locale
                pathDownload = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & sorgente.RelativePath & anno & "/" & nomeFile
                If IO.Path.GetExtension(nomeFile).ToLower <> ".odt" Then
                    functionName = "OpenGenericDocument"
                End If

                datiInput = New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = functionName}
                data = openofficeParameters.CreateOpenParameter(datiInput)

            Case 2  'Ftp
                pathDownload = String.Format("ftp://{0}/{1}", sorgente.Path, sorgente.RelativePath & anno & "/" & nomeFile)
                If IO.Path.GetExtension(nomeFile).ToLower <> ".odt" Then
                    functionName = "OpenGenericDocument"
                End If
                datiInput = New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = functionName}
                data = openofficeParameters.CreateOpenParameter(New ParsecAdmin.DatiCredenziali With {.Password = sorgente.Password, .Port = sorgente.Port, .Username = sorgente.Username}, datiInput)

        End Select

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(data, Nothing, Me.documentContentHidden.ClientID, False, False)
            'ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, False, Nothing)
        End If



    End Sub


    Private Sub VisualizzaDocumento(ByVal nomeFile As String, ByVal anno As String, ByVal sorgente As ParsecAdmin.SourceElement)

        Dim pathDownload As String = String.Empty
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim datiInput As ParsecAdmin.DatiInput = Nothing
        Dim data As String = String.Empty
        Dim functionName As String = "ViewDocument"

        Select Case sorgente.Tipologia
            Case 1 'Locale
                pathDownload = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & sorgente.RelativePath & anno & "/" & nomeFile
                'If IO.Path.GetExtension(nomeFile).ToLower <> ".odt" Then
                '    functionName = "OpenGenericDocument"
                'End If

                datiInput = New ParsecAdmin.DatiInput With {.Path = pathDownload, .ShowWindow = False, .Enabled = False, .FunctionName = "OpenGenericDocument"}

                'datiInput = New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = False, .Enabled = False, .FunctionName = functionName}

                data = openofficeParameters.CreateOpenParameter(datiInput)

            Case 2  'Ftp
                pathDownload = String.Format("ftp://{0}/{1}", sorgente.Path, sorgente.RelativePath & anno & "/" & nomeFile)
                'If IO.Path.GetExtension(nomeFile).ToLower <> ".odt" Then
                '    functionName = "OpenGenericDocument"
                'End If
                datiInput = New ParsecAdmin.DatiInput With {.Path = pathDownload, .ShowWindow = False, .Enabled = False, .FunctionName = "OpenGenericDocument"}

                ' datiInput = New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = False, .Enabled = False, .FunctionName = functionName}
                data = openofficeParameters.CreateOpenParameter(New ParsecAdmin.DatiCredenziali With {.Password = sorgente.Password, .Port = sorgente.Port, .Username = sorgente.Username}, datiInput)

        End Select

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(data, Nothing, Me.documentContentHidden.ClientID, False, False)
            'ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, False, Nothing)
        End If

    End Sub


    Private Sub RegistraButtonClick()
        Me.RegistraScriptPersistenzaVisibilitaPannello()
        ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
    End Sub

#End Region

#Region "GESTIONE TOOLBARBUTTONLIST"

    Protected Sub OnCtx_ItemClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadMenuEventArgs)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim menuItem As Telerik.Web.UI.RadMenuItem = e.Item
        Dim userId As String = menuItem.Attributes("UserId")
        Dim commandName As String = menuItem.Attributes("CommandName")
        Dim commandArgument As String = menuItem.Attributes("CommandArgument")

        Dim list As List(Of ParsecWKF.Action) = ParsecWKF.ModelloInfo.ReadActionInfo(Me.TaskAttivo.TaskCorrente, Me.TaskAttivo.NomeFileIter)
        Me.Action = list.Where(Function(c) c.Name = commandName).FirstOrDefault
        Dim attore = Me.Action.ToActor
        Me.RuoloId = userId

        Dim parametroProcesso As ParsecWKF.ParametroProcesso = Nothing

        Select Case commandArgument
            Case "INVIA AVANTI"

                Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository

                If Me.OperazioneMassiva Then

                    If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
                        ParsecUtility.Utility.MessageBox("OPERAZIONE NON IMPLEMENTATA", False)
                        Me.EnableUiHidden.Value = "Abilita"
                        Me.SbloccaTasks(utenteCollegato.Id)
                        Exit Sub
                    End If

                    Me.ProcediMassiva(Nothing, Direction.Forward)

                    For Each taskCorrente In Me.TaskAttivi
                        Dim task = taskCorrente
                        parametroProcesso = parametriProcesso.GetQuery.Where(Function(c) c.IdProcesso = task.IdIstanza And c.Nome = attore).FirstOrDefault
                        If parametroProcesso Is Nothing Then
                            parametroProcesso = New ParsecWKF.ParametroProcesso
                            parametroProcesso.Valore = userId
                            parametroProcesso.IdProcesso = task.IdIstanza
                            parametroProcesso.Nome = attore
                            parametriProcesso.Add(parametroProcesso)
                        Else
                            parametroProcesso.Valore = userId
                        End If
                        parametriProcesso.SaveChanges()
                    Next

                Else
                    Me.Procedi(Nothing, Direction.Forward)

                    parametroProcesso = parametriProcesso.GetQuery.Where(Function(c) c.IdProcesso = Me.TaskAttivo.IdIstanza And c.Nome = attore).FirstOrDefault
                    If parametroProcesso Is Nothing Then
                        parametroProcesso = New ParsecWKF.ParametroProcesso
                        parametroProcesso.Valore = userId
                        parametroProcesso.IdProcesso = Me.TaskAttivo.IdIstanza
                        parametroProcesso.Nome = attore
                        parametriProcesso.Add(parametroProcesso)
                    Else
                        parametroProcesso.Valore = userId
                    End If
                    parametriProcesso.SaveChanges()


                    If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then

                        Try
                            Me.AggiornaStatoSegnalazione(utenteCollegato.Id)
                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)
                            Me.EnableUiHidden.Value = "Abilita"
                            Me.SbloccaTasks(utenteCollegato.Id)
                        End Try


                    End If



                End If

            Case "FIRMA"
                Try
                    If Me.OperazioneMassiva Then
                        Me.FirmaMassiva(Nothing)
                    Else
                        Me.Firma(Nothing)
                    End If
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                    Me.EnableUiHidden.Value = "Abilita"
                    Me.SbloccaTasks(utenteCollegato.Id)
                End Try
        End Select

    End Sub

    Private Sub CaricaMenuContesto(ByVal role As ParsecWKF.Ruolo, ByVal action As ParsecWKF.Action, ByVal ctx As RadContextMenu, ByVal btn As RadButton, tipoSmistamento As Nullable(Of Integer))

        Dim enabled = Me.OperazioneMassivaConsentita(action)

        Dim vista As List(Of ParsecAdmin.Utente) = Nothing
        Dim dinamica As Boolean = False

        If tipoSmistamento.HasValue Then

            If tipoSmistamento = 0 Then
                '0= STATICA
                dinamica = False
            Else
                '1= DINAMICA
                dinamica = True
            End If
        Else
            If role.Tipologia = 1 Then 'DINAMICA
                dinamica = True
            Else
                dinamica = False
            End If
        End If

        If dinamica Then
            Dim idResponsabile As Integer = Me.TaskAttivo.IdMittente
            vista = Me.GetAddetti(idResponsabile)
        Else
            Dim idRuolo As Integer = role.Id
            vista = Me.GetUtentiRuolo(idRuolo)
        End If


        '****************************************************************************************
        'AGGIUNGO L'UTENTE COLLEGATO E RIORDINO LA LISTA DI ADDETTI/UTENTI RUOLO
        '****************************************************************************************

        If Not action.GetParameterByName("IncludiUtenteCollegato") Is Nothing Then
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

            'SE L'UTENTE COLLEGATO NON E' GIA PRESENTE NELLA LISTA DI ADDETTI/UTENTI RUOLO
            If vista.Where(Function(c) c.Id = utenteCollegato.Id).FirstOrDefault Is Nothing Then
                Dim nuovoUtente As New ParsecAdmin.Utente With {
                           .Id = utenteCollegato.Id,
                           .Descrizione = "ME STESSO (UTENTE COLLEGATO)"
                           }
                vista.Add(nuovoUtente)
                vista = vista.OrderBy(Function(c) c.Descrizione).ToList
            End If

        End If
        '****************************************************************************************

        If vista.Count > 0 Then
            For Each item In vista
                Dim menuItem As New RadMenuItem(item.Descrizione)
                menuItem.Attributes.Add("CommandName", action.Name)
                menuItem.Attributes.Add("UserId", item.Id.ToString)
                menuItem.Attributes.Add("CommandArgument", action.Type)
                menuItem.Attributes.Add("onclick", "ShowControlPanel();")
                'ctx.AutoScrollMinimumHeight = 20
                ctx.Items.Add(menuItem)
            Next
            If ctx.Items.Count > 10 Then
                ctx.DefaultGroupSettings.Height = Unit.Pixel(200)
                ctx.Height = Unit.Pixel(300)
            End If
            btn.Attributes.Add("menu", ctx.ClientID)
        Else
            Dim processi As New ParsecWKF.ParametriProcessoRepository
            Dim parametroProcesso As ParsecWKF.ParametroProcesso = Nothing
            parametroProcesso = processi.GetQuery.Where(Function(c) c.IdProcesso = Me.TaskAttivo.IdIstanza And c.Nome = action.ToActor).FirstOrDefault
            If Not parametroProcesso Is Nothing Then
                btn.Attributes.Add("UserId", parametroProcesso.Valore)
                btn.Attributes.Add("onclick", "ShowControlPanel();")
                btn.EnableSplitButton = False
                btn.AutoPostBack = True
                btn.OnClientClicked = Nothing
                ctx.Visible = False
            Else

                Dim ruoloUtente = (New ParsecWKF.RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = role.Id).FirstOrDefault
                If Not ruoloUtente Is Nothing Then
                    btn.Attributes.Add("UserId", ruoloUtente.IdUtente)
                    btn.Attributes.Add("onclick", "ShowControlPanel();")
                    btn.EnableSplitButton = False
                    btn.AutoPostBack = True
                    btn.OnClientClicked = Nothing
                    ctx.Visible = False
                Else
                    btn.EnableSplitButton = False
                    btn.AutoPostBack = True
                    btn.OnClientClicked = Nothing
                    ctx.Visible = False
                    btn.Enabled = False
                    btn.ToolTip = "Il ruolo " & role.Descrizione & " non è associato a nessun utente."
                    Exit Sub
                End If
            End If

            'If vista.Count = 1 Then
            '    btn.Attributes.Add("UserId", vista(0).Id.ToString)
            '    btn.Attributes.Add("onclick", "ShowControlPanel();")
            'End If

        End If
        btn.Enabled = enabled
    End Sub

    Private Sub ConfiguraPulsante(ByVal idUtente As String, ByVal ctx As RadContextMenu, ByVal btn As RadButton, action As ParsecWKF.Action)

        Dim enabled = Me.OperazioneMassivaConsentita(action)
        btn.Attributes.Add("UserId", idUtente)
        If enabled Then
            Dim script As String = "ShowControlPanel();"
            If action.Parameters.Count > 0 Then
                Dim smistamentoMultiplo As Boolean = Not action.GetParameterByName("AttivaSmistamentoMultiplo") Is Nothing AndAlso Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.ATT
                'Dim visualizzaFatturaParametro = action.GetParameterByName("VisualizzaFattura")
                If smistamentoMultiplo Then
                    script &= "ShowRecipientsPanel();hideRecipients=false;"
                End If
            End If
            btn.Attributes.Add("onclick", script)
            btn.AutoPostBack = True
        Else
            btn.Attributes.Add("onclick", "return false;")
            btn.ToolTip = "Operazione non consentita"
        End If
        btn.EnableSplitButton = False
        btn.OnClientClicked = Nothing
        btn.Enabled = enabled
        ctx.Visible = False
    End Sub

    Protected Sub ToolbarButtonList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles ToolbarButtonList.ItemDataBound
        Dim action As ParsecWKF.Action = e.Item.DataItem
        Dim btn As RadButton = e.Item.FindControl("ExecuteTaskButton")

        Dim ctx As RadContextMenu = e.Item.FindControl("ExecuteContextMenu")
        btn.Attributes.Add("Sender", action.FromActor)

        'Recupero il nome del ruolo (To) associato all'azione.
        Dim roleToName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Receiver).FirstOrDefault.Name

        Dim processi As New ParsecWKF.ParametriProcessoRepository
        Dim parametroProcesso As ParsecWKF.ParametroProcesso = Nothing

        Dim ruoli As New ParsecWKF.RuoloRepository

        'Ottengo il ruolo destinatario
        Dim roleTo = ruoli.GetQuery.Where(Function(c) c.Descrizione = roleToName).FirstOrDefault


        Dim smistamentoStatico As Boolean = Not action.GetParameterByName("AttivaSmistamentoStatico") Is Nothing
        Dim smistamentoDinamico As Boolean = Not action.GetParameterByName("AttivaSmistamentoDinamico") Is Nothing

        If Me.OperazioneMassiva Then

            Dim elenco As New List(Of String)
            For Each taskCorrente In Me.TaskAttivi


                If Not roleTo Is Nothing Then
                    'SE HO SPECIFICATO UNO DEI DUE PARAMETRI NEL FILE DELLA DEFINIZIONE DEL PROCESSO
                    If smistamentoDinamico OrElse smistamentoStatico Then
                        If smistamentoDinamico Then
                            Me.CaricaMenuContesto(roleTo, action, ctx, btn, 1)
                        Else
                            Me.CaricaMenuContesto(roleTo, action, ctx, btn, 0)
                        End If
                        Exit Sub
                    End If
                End If



                Dim idProcesso As Integer = taskCorrente.IdIstanza
                parametroProcesso = processi.GetQuery.Where(Function(c) c.IdProcesso = idProcesso And c.Nome = roleToName).FirstOrDefault

                'SE IL RUOLO E' STATO MEMORIZZATO NEL DB (PER ESEMPIO IL RUOLO VIENE MEMORIZZATO SE E' ASSOCIATO AD UNA FIRMA)
                'SE LA TIPOLOGIA DEL RUOLO NON E' DINAMICA (ALIMENTATATA DAGLI ADDETTI)
                If Not parametroProcesso Is Nothing AndAlso Not roleTo Is Nothing AndAlso Not roleTo.Tipologia.HasValue Then
                    If Not elenco.Contains(parametroProcesso.Valore) Then
                        elenco.Add(parametroProcesso.Valore)
                    End If
                Else

                    'Gli attori sono diversi
                    If action.ToActor <> action.FromActor Then
                        If Not roleTo Is Nothing Then
                            Me.CaricaMenuContesto(roleTo, action, ctx, btn, Nothing)
                            Exit For
                        Else
                            '*************************************************************************
                            'Se il ruolo non esiste presumo che sia l'utente che ha avviato l'iter
                            '*************************************************************************
                            Dim istanze As New ParsecWKF.IstanzaRepository
                            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
                            istanze.Dispose()
                            Me.ConfiguraPulsante(istanza.IdUtente.ToString, ctx, btn, action)

                        End If
                    Else
                        'Gli attori sono li stessi

                        Dim idUtente As Integer = 0
                        If Not roleTo Is Nothing Then

                            If Not parametroProcesso Is Nothing Then
                                idUtente = parametroProcesso.Valore
                                Me.ConfiguraPulsante(idUtente.ToString, ctx, btn, action)
                                Exit Sub
                            End If

                            Dim ruoloUtente = (New ParsecWKF.RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = roleTo.Id).FirstOrDefault
                            If Not ruoloUtente Is Nothing Then
                                idUtente = ruoloUtente.IdUtente
                            Else
                                Dim istanze As New ParsecWKF.IstanzaRepository
                                Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = idProcesso).FirstOrDefault
                                istanze.Dispose()
                                idUtente = istanza.IdUtente
                            End If
                        Else
                            Dim istanze As New ParsecWKF.IstanzaRepository
                            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = idProcesso).FirstOrDefault
                            istanze.Dispose()
                            idUtente = istanza.IdUtente
                        End If

                        If Not elenco.Contains(idUtente) Then
                            elenco.Add(idUtente)
                        End If
                    End If
                End If

            Next

            Dim elencoUtenti = String.Join(";", elenco.ToArray())
            If Not String.IsNullOrEmpty(elencoUtenti) Then
                Me.ConfiguraPulsante(elencoUtenti, ctx, btn, action)
            End If
        Else

            Dim smistamentoMultiplo As Boolean = Not action.GetParameterByName("AttivaSmistamentoMultiplo") Is Nothing AndAlso Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.ATT

            If Not smistamentoMultiplo Then

                If Not roleTo Is Nothing Then
                    'SE HO SPECIFICATO UNO DEI DUE PARAMETRI NEL FILE DELLA DEFINIZIONE DEL PROCESSO
                    If smistamentoDinamico OrElse smistamentoStatico Then
                        If smistamentoDinamico Then
                            Me.CaricaMenuContesto(roleTo, action, ctx, btn, 1)
                        Else
                            Me.CaricaMenuContesto(roleTo, action, ctx, btn, 0)
                        End If
                        Exit Sub
                    End If
                End If

                parametroProcesso = processi.GetQuery.Where(Function(c) c.IdProcesso = Me.TaskAttivo.IdIstanza And c.Nome = roleToName).FirstOrDefault

                'SE IL RUOLO E' STATO MEMORIZZATO NEL DB (PER ESEMPIO IL RUOLO VIENE MEMORIZZATO SE E' ASSOCIATO AD UNA FIRMA)
                'SE LA TIPOLOGIA DEL RUOLO NON E' DINAMICA (ALIMENTATATA DAGLI ADDETTI)
                If Not roleTo Is Nothing AndAlso Not roleTo.Tipologia.HasValue Then
                    If Not parametroProcesso Is Nothing Then
                        Dim idUtente As String = parametroProcesso.Valore
                        Me.ConfiguraPulsante(idUtente, ctx, btn, action)
                        Exit Sub
                    End If
                End If

                If action.ToActor <> action.FromActor Then

                    '*************************************************************************
                    'Se il ruolo esiste 
                    '*************************************************************************
                    If Not roleTo Is Nothing Then
                        Me.CaricaMenuContesto(roleTo, action, ctx, btn, Nothing)
                    Else

                        '*************************************************************************
                        'Se il ruolo non esiste presumo che sia l'utente che ha avviato l'iter
                        '*************************************************************************
                        Dim istanze As New ParsecWKF.IstanzaRepository
                        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
                        istanze.Dispose()
                        Me.ConfiguraPulsante(istanza.IdUtente.ToString, ctx, btn, action)

                    End If

                Else 'Gli attori sono li stessi
                    Dim idUtente As Integer = 0

                    If Not roleTo Is Nothing Then


                        If Not parametroProcesso Is Nothing Then
                            idUtente = parametroProcesso.Valore
                            Me.ConfiguraPulsante(idUtente.ToString, ctx, btn, action)
                            Exit Sub
                        End If


                        Dim ruoloUtente = (New ParsecWKF.RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = roleTo.Id).FirstOrDefault
                        If Not ruoloUtente Is Nothing Then
                            idUtente = ruoloUtente.IdUtente
                        Else
                            Dim istanze As New ParsecWKF.IstanzaRepository
                            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
                            istanze.Dispose()
                            idUtente = istanza.IdUtente
                        End If
                    Else
                        Dim istanze As New ParsecWKF.IstanzaRepository
                        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
                        istanze.Dispose()
                        idUtente = istanza.IdUtente
                    End If
                    Me.ConfiguraPulsante(idUtente.ToString, ctx, btn, action)
                End If

            Else

                'AVVIO ITER NOTIFICA ATTO AMMINISTRATIVO
                Me.CaricaUtentiAbilitatiNotifica()


                parametroProcesso = processi.GetQuery.Where(Function(c) c.IdProcesso = Me.TaskAttivo.IdIstanza And c.Nome = roleToName).FirstOrDefault
                Dim idUtente As String = parametroProcesso.Valore
                Me.ConfiguraPulsante(idUtente.ToString, ctx, btn, action)

            End If

        End If

        processi.Dispose()


    End Sub

    Private Sub CaricaUtentiAbilitatiNotifica()

        Dim utenti As New ParsecAdmin.UserRepository
        Dim funzioniUtente As New ParsecAdmin.FunzioniUtenteRepository(utenti.Context)

        Dim res = (From utente In utenti.GetQuery.Select(Function(c) New With {c.Id, c.Nome, c.Cognome, c.Username, c.LogTipoOperazione})
                   Join funzioneUtente In funzioniUtente.GetQuery
                   On funzioneUtente.IdUtente Equals utente.Id
                   Where utente.LogTipoOperazione Is Nothing And funzioneUtente.IdFunzione = ParsecAtt.TipologiaFunzione.AbilitaNotificaAttoAmministrativo
                   Select utente).AsEnumerable.Select(Function(c) New With {
                                                          .Id = c.Id,
                                                          .Descrizione = (c.Username & " - " & c.Cognome & " " & c.Nome)
                                                      }).OrderBy(Function(c) c.Descrizione)



        Me.UtentiListBox.DataValueField = "Id"
        Me.UtentiListBox.DataTextField = "Descrizione"
        Me.UtentiListBox.DataSource = res
        Me.UtentiListBox.DataBind()

        utenti.Dispose()
    End Sub

    Protected Sub ToolbarButtonList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles ToolbarButtonList.ItemCommand

        'e.Item.FindControl("ExecuteTaskButton")

        ParsecUtility.SessionManager.Documento = Nothing

        Me.AutomaticTask = False

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim lock = Me.GetLock()

        If Not lock Is Nothing Then
            If lock.IdUtente <> utenteCollegato.Id Then
                ParsecUtility.Utility.MessageBox("Il task selezionato è bloccato dall'utente " & lock.Utente, False)
                Me.EnableUiHidden.Value = "Abilita"
                Exit Sub
            Else
                'NIENTE
            End If
        Else 'SE IL TASK NON E' BLOCCATO
            Me.BloccaTask(utenteCollegato)
        End If

        Dim startTick As Long = DateTime.Now.Ticks

        Dim name As String = ToolbarButtonList.DataKeys(e.Item.ItemIndex)
        Dim list As List(Of ParsecWKF.Action) = ParsecWKF.ModelloInfo.ReadActionInfo(Me.TaskAttivo.TaskCorrente, Me.TaskAttivo.NomeFileIter)

        'todo verificare se ci sono azioni con lo stesso identificativo (attributo name)

        If list.Where(Function(c) c.Name = name).Count > 1 Then
            ParsecUtility.Utility.MessageBox("Esistono più azioni con lo stesso identificativo '" & name & "'." & vbCrLf & "Impossibile eseguire l'operazione." & vbCrLf & "Contattare l'assistenza.", False)
            Me.EnableUiHidden.Value = "Abilita"
            Exit Sub
        End If


        Me.Action = list.Where(Function(c) c.Name = name).FirstOrDefault

        'Dim endTick As Long = DateTime.Now.Ticks
        'Dim tick As Long = endTick - startTick
        'Dim milliseconds As Long = tick / TimeSpan.TicksPerMillisecond


        Select Case Me.Action.Type
            Case "MODIFICA"

                If Me.OperazioneMassiva Then
                    'SOLO SE STO ESPORTANDO LA FATTURA
                    Dim esportaFatturaParametro = Me.Action.GetParameterByName("EsportaFattura")
                    Dim usaFtpParametro = Me.Action.GetParameterByName("UsaFtp")
                    If Not esportaFatturaParametro Is Nothing Then

                        Dim parametri As New ParsecAdmin.ParametriRepository
                        Dim parametro = parametri.GetByName("TipoEsportazioneFattura", ParsecAdmin.TipoModulo.PRO)
                        parametri.Dispose()
                        Dim tipoEsportazione As String = "0"

                        If Not parametro Is Nothing Then
                            tipoEsportazione = parametro.Valore
                        End If

                        Select Case tipoEsportazione
                            Case "0", "2" 'ESPORTAZIONE JSIBAC - APSYSTEMS 
                                If Not usaFtpParametro Is Nothing Then
                                    Me.EsportaFatturaMassivaJSibacApSystems(tipoEsportazione)
                                End If
                            Case "2"  'ESPORTAZIONE TINN
                                Me.EsportaFatturaMassivaTINN()
                            Case "3" 'ESPORTAZIONE DEDAGROUP
                                Me.EseguiEsportazioneDedaGroup(Me.TaskAttivi)
                        End Select

                        

                    Else
                        Me.Modifica(e)
                    End If
                Else
                    Me.Modifica(e)
                End If


            Case "INVIA AVANTI"

                Dim smistamentoMultiplo As Boolean = Not Action.GetParameterByName("AttivaSmistamentoMultiplo") Is Nothing AndAlso Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.ATT

                If Not smistamentoMultiplo Then
                    If Me.OperazioneMassiva Then

                        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
                            ParsecUtility.Utility.MessageBox("OPERAZIONE NON IMPLEMENTATA", False)
                            Me.EnableUiHidden.Value = "Abilita"
                            Me.SbloccaTasks(utenteCollegato.Id)
                            Exit Sub
                        End If

                        Me.ProcediMassiva(e, Direction.Forward)
                    Else
                        Me.Procedi(e, Direction.Forward)

                        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then

                            Try
                                Me.AggiornaStatoSegnalazione(utenteCollegato.Id)

                            Catch ex As Exception
                                ParsecUtility.Utility.MessageBox(ex.Message, False)
                                Me.EnableUiHidden.Value = "Abilita"
                                Me.SbloccaTasks(utenteCollegato.Id)
                            End Try
                        End If
                    End If
                Else
                    'AVVIO ITER NOTIFICA ATTO AMMINISTRATIVO
                    'NIENTE VERRA' VISUALIZZATO UN POPUP 
                End If



            Case "FINE"
                If Me.OperazioneMassiva Then

                    If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
                        ParsecUtility.Utility.MessageBox("OPERAZIONE NON IMPLEMENTATA", False)
                        Me.EnableUiHidden.Value = "Abilita"
                        Me.SbloccaTasks(utenteCollegato.Id)
                        Exit Sub
                    End If

                    Me.FineMassiva(e)
                Else
                    Me.Fine(e)
                End If

            Case "CANCELLA"
                Me.Cancella(e)
            Case "INVIA INDIETRO"
                Me.Procedi(e, Direction.Backward)
            Case "SMISTAMENTO"
                Me.Procedi(e, Direction.Forward)

            Case "NUMERAZIONE"
                Try
                    If Me.OperazioneMassiva Then
                        Me.NumerazioneMassiva(e)
                    Else
                        Me.Numera(e)
                    End If
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                    Me.EnableUiHidden.Value = "Abilita"
                    Me.SbloccaTasks(utenteCollegato.Id)
                End Try

            Case "FIRMA"
                Try
                    If Me.OperazioneMassiva Then

                        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
                            ParsecUtility.Utility.MessageBox("OPERAZIONE NON IMPLEMENTATA", False)
                            Me.EnableUiHidden.Value = "Abilita"
                            Me.SbloccaTasks(utenteCollegato.Id)
                            Exit Sub
                        End If

                        Me.FirmaMassiva(e)
                    Else
                        Me.Firma(e)
                    End If
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                    Me.EnableUiHidden.Value = "Abilita"
                    Me.SbloccaTasks(utenteCollegato.Id)
                End Try
            Case "NUMERAZIONE PRATICA"
                Me.NumeraPratica(e)

            Case "ASSOCIA PROTOCOLLO SUE"
                Me.LavoraPratica(e)

            Case "ASSOCIA PROTOCOLLO SUAP"
                Me.LavoraPratica(e)

            Case ""
        End Select

        'Dim t As Type = GetType(ParsecWKF.Action)
        'Dim methodInfo As System.Reflection.MethodInfo = t.GetMethod(e.CommandArgument)
        'Dim obj As Object = Activator.CreateInstance(t)
        'methodInfo.Invoke(obj, Nothing)
    End Sub

#End Region

#Region "GESTIONE VISIBILITA'"

    Private Sub AggiungiUtenteVisibilita(ByVal idUtente As Integer)

        Dim idDocumento As Integer = 0
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
        If Not istanza Is Nothing Then
            idDocumento = istanza.IdDocumento
        End If
        istanze.Dispose()

        Dim visibilitaDocumento As New ParsecAdmin.VisibilitaDocumentoRepository
        Dim visibilita = visibilitaDocumento.GetQuery.Where(Function(c) c.IdDocumento = idDocumento And c.IdModulo = Me.TaskAttivo.IdModulo).ToList

        Dim esiste As Boolean = Not visibilita.Where(Function(c) c.IdEntita = idUtente And c.TipoEntita = ParsecAdmin.TipoEntita.Utente).FirstOrDefault Is Nothing
        If Not esiste Then
            Dim utenti As New ParsecAdmin.UserRepository
            Dim utente As ParsecAdmin.Utente = utenti.GetUserById(idUtente).FirstOrDefault
            utenti.Dispose()
            If Not utente Is Nothing Then
                Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetUtenteVisibilita(utente, Me.TaskAttivo.IdModulo)
                utenteVisibilita.IdDocumento = idDocumento
                visibilitaDocumento.Add(utenteVisibilita)
                visibilitaDocumento.SaveChanges()
            End If
        End If
        visibilitaDocumento.Dispose()
    End Sub

    Private Sub AggiungiGruppoVisibilitaTuttiUtenti(ByVal idDocumento As Integer)

        Dim gruppoVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetGruppoVisibilitaTuttiUtenti(Me.TaskAttivo.IdModulo)
        If Not gruppoVisibilita Is Nothing Then

            Dim visibilitaDocumento As New ParsecAdmin.VisibilitaDocumentoRepository
            Dim visibilita = visibilitaDocumento.GetQuery.Where(Function(c) c.IdDocumento = idDocumento And c.IdModulo = Me.TaskAttivo.IdModulo).ToList

            Dim esiste As Boolean = Not visibilita.Where(Function(c) c.IdEntita = gruppoVisibilita.Id And c.TipoEntita = ParsecAdmin.TipoEntita.Gruppo).FirstOrDefault Is Nothing
            If Not esiste Then
                gruppoVisibilita.IdDocumento = idDocumento
                visibilitaDocumento.Add(gruppoVisibilita)
                visibilitaDocumento.SaveChanges()
            End If
            visibilitaDocumento.Dispose()
        End If

    End Sub

    Private Function GetUtenteVisibilita(ByVal utente As ParsecAdmin.Utente, ByVal idModulo As Integer) As ParsecAdmin.VisibilitaDocumento
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
        utenteVisibilita.AbilitaCancellaEntita = False
        utenteVisibilita.Descrizione = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
        utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
        utenteVisibilita.IdEntita = utente.Id
        utenteVisibilita.IdModulo = idModulo
        utenteVisibilita.LogIdUtente = utenteCollegato.Id
        utenteVisibilita.LogDataOperazione = Now
        Return utenteVisibilita
    End Function

    Private Function GetGruppoVisibilitaTuttiUtenti(ByVal idModulo As Integer) As ParsecAdmin.VisibilitaDocumento
        Dim gruppi As New ParsecAdmin.GruppoRepository
        Dim gruppo As ParsecAdmin.Gruppo = gruppi.GetQuery.Where(Function(c) c.Id = 1).FirstOrDefault
        gruppi.Dispose()
        Dim gruppoVisibilita As ParsecAdmin.VisibilitaDocumento = Nothing
        If Not gruppo Is Nothing Then
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            gruppoVisibilita = New ParsecAdmin.VisibilitaDocumento
            gruppoVisibilita.IdEntita = 1
            gruppoVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
            gruppoVisibilita.IdModulo = idModulo
            gruppoVisibilita.Descrizione = gruppo.Descrizione
            gruppoVisibilita.LogIdUtente = utenteCollegato.Id
            gruppoVisibilita.LogDataOperazione = Now
            gruppoVisibilita.AbilitaCancellaEntita = False
        End If
        Return gruppoVisibilita
    End Function

#End Region

#Region "GESTIONE LOCK UNLOCK"

    Private Sub BloccaTask(ByVal utenteCollegato As ParsecAdmin.Utente)

        Dim locks As New ParsecWKF.LockTaskRepository
        Dim lock As New ParsecWKF.LockTask
        lock.IdTask = Me.TaskAttivo.Id
        lock.IdUtente = utenteCollegato.Id
        lock.Valore = True
        lock.DataInizio = Now
        locks.Add(lock)
        locks.SaveChanges()
        locks.Dispose()
    End Sub

    Private Function GetLock(ByVal utenteCollegato As ParsecAdmin.Utente) As ParsecWKF.LockTask
        Dim locks As New ParsecWKF.LockTaskRepository
        Dim lock As ParsecWKF.LockTask = locks.GetQuery.Where(Function(c) c.IdUtente = utenteCollegato.Id).FirstOrDefault
        Return lock
    End Function

    Private Function GetLock() As ParsecWKF.LockTask
        Dim locks As New ParsecWKF.LockTaskRepository
        Dim lock As ParsecWKF.LockTask = locks.GetByIdTask(Me.TaskAttivo.Id)
        Return lock
    End Function

    Private Sub SbloccaTasks(idUtente As Integer)
        'Elimino tutti i task bloccati dall'utente corrente.
        Dim taskBloccati As New ParsecWKF.LockTaskRepository
        taskBloccati.DeleteAll(idUtente)
        taskBloccati.Dispose()
    End Sub

    Private Sub SbloccaDocumenti(idUtente As Integer)
        Dim documentiBloccati As New ParsecAtt.LockDocumentoRepository
        'Elimino tutti i documenti bloccati dall'utente corrente.
        documentiBloccati.DeleteAll(idUtente)
        documentiBloccati.Dispose()
    End Sub

    Private Sub SbloccaRegistrazioni(idUtente As Integer)
        Dim registrazioniBloccate As New ParsecPro.LockRegistrazioneRepository
        'Elimino tutti i protocolli bloccati dall'utente corrente.
        registrazioniBloccate.DeleteAll(idUtente)
        registrazioniBloccate.Dispose()
    End Sub

#End Region

#Region "GESTIONE TRASPARENZA"

    'luca 01/07/2020
    '' ''Public Sub PubblicaTrasparenza(ByVal documento As ParsecAtt.Documento)

    '' ''    Dim hashTable As New Dictionary(Of Integer, ParsecWebServices.PubblicazioneOutputWS)
    '' ''    Dim pubblicazioneOnline As Boolean = True
    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    Dim pubblicazioniR As New ParsecAdmin.PubblicazioneRepository
    '' ''    Dim pubblicazioneDaAggiornare As ParsecAdmin.Pubblicazione = pubblicazioniR.GetQuery.Where(Function(c) c.Id = documento.Trasparenza.Id).FirstOrDefault

    '' ''    Dim idGaraContratti As Nullable(Of Integer) = Nothing

    '' ''    Dim data As DateTime = Now
    '' ''    pubblicazioneDaAggiornare.DataOperazione = data
    '' ''    pubblicazioneDaAggiornare.DataInizioPubblicazione = documento.Trasparenza.DataInizioPubblicazione
    '' ''    pubblicazioneDaAggiornare.DataFinePubblicazione = documento.Trasparenza.DataFinePubblicazione
    '' ''    pubblicazioneDaAggiornare.IdUtente = utenteCollegato.Id
    '' ''    pubblicazioneDaAggiornare.Stato = Nothing

    '' ''    Select Case documento.Trasparenza.TipologiaSezioneTrasparente
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''            Dim info As String = String.Empty
    '' ''            For Each procedura In documento.Trasparenza.ProcedureAffidamento
    '' ''                Dim attoConcessione As ParsecAdmin.AttoConcessione = procedura
    '' ''                info = info & attoConcessione.Beneficiario & " - importo: " & attoConcessione.Importo & ";" & vbCrLf
    '' ''            Next
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = info

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''            Dim consulenza As ParsecAdmin.Consulenza = documento.Trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = consulenza.Beneficiario & " - " & consulenza.Oggetto & " dal " & consulenza.DataInizioIncaricoConsulenza & " al " & consulenza.DataFineIncaricoConsulenza

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''            Dim bandoGara As ParsecAdmin.BandoGara = documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = "CIG " & bandoGara.Cig & " - Oggetto: " & bandoGara.Oggetto & " - Aggiudicatari: " & bandoGara.Aggiudicatario

    '' ''            idGaraContratti = bandoGara.idGaraContratti

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''            Dim incarico As ParsecAdmin.IncaricoDipendente = documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = incarico.Beneficiario & " - Oggetto: " & incarico.Oggetto & " - importo: " & incarico.Compenso

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale

    '' ''            Dim incarico As ParsecAdmin.IncaricoAmministrativoDirigenziale = documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = incarico.titolare & " - " & incarico.denominazione & " dal " & incarico.dal & " al " & incarico.al

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso

    '' ''            Dim bando As ParsecAdmin.BandoConcorso = documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = bando.oggetto

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate
    '' ''            Dim ente As ParsecAdmin.EnteControllato = documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = ente.ragioneSociale

    '' ''        Case Else    'PUBBLICAZIONE GENERICA
    '' ''            Dim pubblicazioneGenerica As ParsecAdmin.PubblicazioneGenerica = documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = pubblicazioneGenerica.titolo & " - " & pubblicazioneGenerica.contenuto

    '' ''    End Select

    '' ''    pubblicazioniR.SaveChanges()

    '' ''    Dim pubblicazioniRepository As New ParsecPub.PubblicazioneRepository
    '' ''    Dim pubblicazionePub As ParsecPub.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazioneDaAggiornare.Id)
    '' ''    pubblicazioniRepository.Dispose()

    '' ''    Select Case documento.Trasparenza.TipologiaSezioneTrasparente
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''            Dim attoConcessionePubblicazione As New ParsecWebServices.AttoConcessione
    '' ''            hashTable = attoConcessionePubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''            'caso bando gara
    '' ''            Dim bandoGaraPubblicazione As New ParsecWebServices.BandoGara
    '' ''            hashTable = bandoGaraPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)

    '' ''            'LUCA 01/'7/2020
    '' ''            '' ''If idGaraContratti.HasValue Then
    '' ''            '' ''    Me.AggiornaImportoLiquidatoPubblicazione(idGaraContratti.Value)
    '' ''            '' ''End If

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''            Dim incaricoPubblicazione As New ParsecWebServices.IncaricoDipendente
    '' ''            hashTable = incaricoPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''            Dim cunsulenzaPubblicazione As New ParsecWebServices.ConsulenzaCollaborazione
    '' ''            hashTable = cunsulenzaPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale
    '' ''            Dim incaricoPubblicazione As New ParsecWebServices.IncaricoDirigenzialeAmministrativo
    '' ''            hashTable = incaricoPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)


    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso

    '' ''            Dim bandoConcorsoPubblicazione As New ParsecWebServices.BandoConcorso
    '' ''            hashTable = bandoConcorsoPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)


    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate

    '' ''            Dim entePubblicazione As New ParsecWebServices.EnteControllato
    '' ''            hashTable = entePubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)


    '' ''        Case Else    'PUBBLICAZIONE GENERICA

    '' ''            Dim genericaPubblicazione As New ParsecWebServices.PubblicazioneGenerica
    '' ''            hashTable = genericaPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)

    '' ''    End Select


    '' ''    If Not pubblicazioneDaAggiornare Is Nothing Then
    '' ''        If pubblicazioneOnline Then
    '' ''            'Dim pubblicazioneDaAggiornare As ParsecAdmin.Pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.Id = pubblicazione.Id).FirstOrDefault
    '' ''            pubblicazioneDaAggiornare.Pubblicato = True
    '' ''            pubblicazioneDaAggiornare.Stato = Nothing
    '' ''            pubblicazioniR.SaveChanges()
    '' ''        Else
    '' ''            pubblicazioneDaAggiornare.Stato = "S"
    '' ''            pubblicazioniR.SaveChanges()
    '' ''        End If
    '' ''    End If

    '' ''    pubblicazioniR.Dispose()

    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub AggiornaImportoLiquidatoPubblicazione(ByVal idGaraContratti As Integer)

    '' ''    Try
    '' ''        Dim parametri As New ParsecAdmin.ParametriRepository
    '' ''        Dim parametro = parametri.GetByName("AutomatizzazionePubblicazioneGare", ParsecAdmin.TipoModulo.PUB)
    '' ''        parametri.Dispose()

    '' ''        If Not parametro Is Nothing AndAlso parametro.Valore = "1" Then

    '' ''            Dim pubblicazioni As New ParsecPub.PubblicazioneRepository
    '' ''            Dim pubblicazioniGara As New ParsecPub.BandoGaraRepository(pubblicazioni.Context)



    '' ''            Dim importoTotaleLiquidato = (From gara In pubblicazioniGara.Where(Function(g) g.idGaraContratti = idGaraContratti)
    '' ''                                          Join pubblicazione In pubblicazioni.Where(Function(p) p.Stato Is Nothing)
    '' ''                                          On gara.idPubblicazione Equals pubblicazione.Id
    '' ''                                          Select gara.importoLiquidato)

    '' ''            If importoTotaleLiquidato.Any Then
    '' ''                Dim importo As Decimal = importoTotaleLiquidato.Sum
    '' ''                If importo >= 0 Then

    '' ''                    Dim gare As New ParsecContratti.GaraRepository

    '' ''                    Dim gara As ParsecContratti.Gara = gare.Where(Function(o) o.idGara = idGaraContratti).FirstOrDefault

    '' ''                    If Not gara Is Nothing Then
    '' ''                        gara.importoSommaLiquidataPubblicazione = importo
    '' ''                        gare.SaveChanges()
    '' ''                    End If

    '' ''                    gare.Dispose()

    '' ''                End If
    '' ''            End If

    '' ''            pubblicazioni.Dispose()



    '' ''        End If
    '' ''    Catch ex As Exception
    '' ''        'NIENTE - NON BLOCCANTE
    '' ''    End Try

    '' ''End Sub

    'metodo per aggiornare il database di parsecpub
    'luca 01/07/2020
    '' ''Private Sub aggiornaParsecPubRepository(ByVal pubblicazione As ParsecPub.Pubblicazione, ByVal hashTableOutputWS As Dictionary(Of Integer, ParsecWebServices.PubblicazioneOutputWS))
    '' ''    Try

    '' ''        Select Case pubblicazione.idSezione
    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''                'caso atto concessione
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                Dim attoRep As New ParsecPub.AttoConcessioneRepository
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.AttoConcessione = attoRep.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    attoRep.SaveChanges()
    '' ''                Next
    '' ''                attoRep.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''                'caso bando gara
    '' ''                Dim bandogaraRep As New ParsecPub.BandoGaraRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.BandoGara = bandogaraRep.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    bandogaraRep.SaveChanges()
    '' ''                Next
    '' ''                bandogaraRep.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''                'caso consulenti e collaboratori
    '' ''                Dim consulenzaRep As New ParsecPub.ConsulenzaRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.Consulenza = consulenzaRep.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    consulenzaRep.SaveChanges()
    '' ''                Next
    '' ''                consulenzaRep.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''                'caso incarichi dipendenti
    '' ''                Dim incaricoRep As New ParsecPub.IncaricoDipendenteRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.IncaricoDipendente = incaricoRep.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    incaricoRep.SaveChanges()
    '' ''                Next
    '' ''                incaricoRep.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale

    '' ''                Dim incarichi As New ParsecPub.IncaricoAmministrativoDirigenzialeRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.IncaricoAmministrativoDirigenziale = incarichi.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    incarichi.SaveChanges()
    '' ''                Next
    '' ''                incarichi.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso

    '' ''                Dim bandi As New ParsecPub.BandoConcorsoRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.BandoConcorso = bandi.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    bandi.SaveChanges()
    '' ''                Next
    '' ''                bandi.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate

    '' ''                Dim enti As New ParsecPub.EnteControllatoRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.EnteControllato = enti.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    enti.SaveChanges()
    '' ''                Next
    '' ''                enti.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case Else    'PUBBLICAZIONE GENERICA

    '' ''                Dim pubblicazioniGeneriche As New ParsecPub.PubblicazioneGenericaRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.PubblicazioneGenerica = pubblicazioniGeneriche.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    pubblicazioniGeneriche.SaveChanges()
    '' ''                Next
    '' ''                pubblicazioniGeneriche.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''        End Select

    '' ''    Catch ex As Exception
    '' ''        Throw New Exception("Riscontrati problemi con la Pubblicazione: " & ex.Message)
    '' ''    End Try
    '' ''End Sub

    'aggiorno lo stato della Pubblicazione insieme alle date di Pubblicazine.
    'Tale metodo va richamato solo alla prima pubblicazione quando le date di pubblicazione possono essere note solo dopo la pubblicazione on-line
    'luca 01/07/2020
    '' ''Private Sub aggiornaStatoPubblicazione(ByVal pubblicazione As ParsecPub.Pubblicazione, ByVal datainizio As Date, ByVal dataFine As Date)
    '' ''    If (Not pubblicazione.Pubblicato) Then
    '' ''        Dim pubblicazioniRepository As New ParsecPub.PubblicazioneRepository
    '' ''        Dim pubbl As ParsecPub.Pubblicazione = pubblicazioniRepository.GetQuery.Where(Function(c) c.Id = pubblicazione.Id).FirstOrDefault
    '' ''        pubbl.Pubblicato = True
    '' ''        pubbl.DataInizioPubblicazione = datainizio
    '' ''        pubbl.DataFinePubblicazione = dataFine
    '' ''        pubblicazioniRepository.SaveChanges()
    '' ''    End If
    '' ''End Sub

   
#End Region

#Region "GESTIONE PUBBLICAZIONE"

    'Private Sub PubblicaLiquidazione(ByVal documento As ParsecAtt.Documento)

    '    Dim parametri As New ParsecAdmin.ParametriRepository
    '    Dim parametro = parametri.GetByName("AbilitaAmministrazioneAperta")
    '    parametri.Dispose()

    '    Dim abilitatoAmministrazioneAperta As Boolean = False
    '    If Not parametro Is Nothing Then
    '        abilitatoAmministrazioneAperta = CBool(parametro.Valore)
    '    End If

    '    Dim pubblicazioni As New ParsecMES.AlboRepository
    '    Dim contatore As ParsecMES.ContatoreAlbo = pubblicazioni.GetContatoreCorrente
    '    Dim annoEsercizio As Integer = Now.Year
    '    If Not contatore Is Nothing Then
    '        annoEsercizio = pubblicazioni.GetContatoreCorrente.Anno
    '    End If
    '    pubblicazioni.Dispose()


    '    Dim localPathAlbo As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo") & annoEsercizio.ToString & "\LIQ\"

    '    Dim localPathDocumenti As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")

    '    '********************************************************************************************************
    '    'CREO LE CARTELLE SE NON ESISTONO
    '    '********************************************************************************************************
    '    If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")) Then
    '        IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo"))
    '    End If

    '    If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo") & annoEsercizio.ToString) Then
    '        IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo") & annoEsercizio.ToString)
    '    End If

    '    If Not IO.Directory.Exists(localPathAlbo) Then
    '        IO.Directory.CreateDirectory(localPathAlbo)
    '    End If
    '    '********************************************************************************************************

    '    Dim maxLengthAlbo As Integer = ParsecAdmin.WebConfigSettings.GetKey("MaxLengthAlbo")


    '    Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
    '    Dim messaggio As String = String.Empty

    '    '********************************************************************************************************
    '    'SALVO GLI ALLEGATI CHE NON SONO DI TIPO ALLEGATO GENERICO IN  ALBO/ANNO/LIQ
    '    '********************************************************************************************************

    '    Dim allegatiDaPubblicare As List(Of ParsecAtt.Allegato) = documento.Allegati.Where(Function(c) c.IdTipologiaAllegato <> 0).ToList

    '    For Each allegato In allegatiDaPubblicare
    '        Dim input As String = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti"), allegato.PercorsoRelativo, allegato.Nomefile)
    '        Dim output As String = localPathAlbo & allegato.Nomefile
    '        Me.CopiaDocumento(input, output)
    '    Next

    '    '********************************************************************************************************
    '    'ANNULLO LE PRECEDENTI LIQUIDAZIONI DA PUBBLICARE
    '    '********************************************************************************************************
    '    Dim liquidazioniAlbo As New ParsecMES.AlboLiquidazioneRepository

    '    Dim documenti As New ParsecAtt.DocumentoRepository
    '    Dim liquidazioniDocumento As New ParsecAtt.LiquidazioneRepository

    '    Dim elencoIdDocumenti = documenti.GetQuery.Where(Function(c) c.Codice = documento.Codice).Select(Function(c) c.Id).ToList
    '    Dim elencoIdLiquidazioni = liquidazioniDocumento.GetQuery.Where(Function(c) elencoIdDocumenti.Contains(c.IdDocumento)).Select(Function(c) c.Id).ToList

    '    liquidazioniDocumento.Dispose()
    '    documenti.Dispose()

    '    Dim liquidazioni As List(Of ParsecMES.AlboLiquidazione) = liquidazioniAlbo.GetQuery.Where(Function(c) elencoIdLiquidazioni.Contains(c.IdLiquidazione)).ToList
    '    For Each liq In liquidazioni
    '        liq.Stato = "M"
    '    Next
    '    liquidazioniAlbo.SaveChanges()
    '    '********************************************************************************************************


    '    Dim clienti As New ParsecAdmin.ClientRepository
    '    Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '    Dim codiceEnte As String = cliente.Identificativo
    '    clienti.Dispose()



    '    Dim contratto = documento.Allegati.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
    '    Dim curriculumVitae = documento.Allegati.Where(Function(c) c.IdTipologiaAllegato = 2).FirstOrDefault
    '    Dim elencoBeneficiari = documento.Allegati.Where(Function(c) c.IdTipologiaAllegato = 3).FirstOrDefault


    '    Dim nomefileContratto As String = String.Empty
    '    Dim nomefileCurriculumVitae As String = String.Empty
    '    Dim nomefilEelencoBeneficiari As String = String.Empty

    '    If Not contratto Is Nothing Then
    '        nomefileContratto = contratto.Nomefile
    '    End If
    '    If Not curriculumVitae Is Nothing Then
    '        nomefileCurriculumVitae = curriculumVitae.Nomefile
    '    End If
    '    If Not elencoBeneficiari Is Nothing Then
    '        nomefilEelencoBeneficiari = elencoBeneficiari.Nomefile
    '    End If



    '    For Each liquidazione In documento.Liquidazioni

    '        '********************************************************************************************************
    '        'INSERISCO LA NUOVA LIQUIDAZIONE DA PUBBLICARE ON-LINE
    '        '********************************************************************************************************
    '        Dim liquadazioneDaPubblicare As New ParsecMES.AlboLiquidazione
    '        liquadazioneDaPubblicare.IdLiquidazione = liquidazione.Id
    '        liquadazioneDaPubblicare.IdUtente = utenteCollegato.Id
    '        liquadazioneDaPubblicare.Data = Now
    '        liquadazioneDaPubblicare.Pubblicato = False
    '        If Not contratto Is Nothing Then
    '            liquadazioneDaPubblicare.Contratto = contratto.Nomefile
    '        End If
    '        If Not curriculumVitae Is Nothing Then
    '            liquadazioneDaPubblicare.Cv = curriculumVitae.Nomefile
    '        End If
    '        If Not elencoBeneficiari Is Nothing Then
    '            liquadazioneDaPubblicare.ElencoBeneficiari = elencoBeneficiari.Nomefile
    '        End If
    '        liquidazioniAlbo.Add(liquadazioneDaPubblicare)
    '        liquidazioniAlbo.SaveChanges()

    '        '********************************************************************************************************
    '        If abilitatoAmministrazioneAperta And documento.Modello.PubblicazioneLiq Then

    '            Try
    '                Dim ws As New PubblicazioneAlbo.wsPubblicazioneAlbo
    '                ws.Timeout = -1

    '                Dim l = liquidazioniAlbo.GetById(liquadazioneDaPubblicare.Id)

    '                messaggio = ws.Pubblica(l.IdLiquidazione, l.Beneficiario, codiceEnte, l.CodiceFiscalePartitaIva, l.ImportoLiquidato, l.Modalita, l.ResponsabileUfficio, l.Norma, l.LinkDocumento, nomefileContratto, nomefileCurriculumVitae, nomefilEelencoBeneficiari)

    '                If String.IsNullOrEmpty(messaggio) Then

    '                    l.Pubblicato = True
    '                    liquidazioniAlbo.SaveChanges()

    '                    'ws.CancellaPubblicazioneLiquidazione(liquidazione.Id)

    '                    For Each allegato In allegatiDaPubblicare
    '                        Dim percorsoFile As String = localPathAlbo & allegato.Nomefile
    '                        If IO.File.Exists(percorsoFile) Then
    '                            Dim fi As New IO.FileInfo(percorsoFile)
    '                            Dim kbLength As Integer = fi.Length \ 1024
    '                            If utenteCollegato.SuperUser OrElse kbLength < maxLengthAlbo Then
    '                                messaggio = ws.PubblicaFilesLiquidazione(codiceEnte, annoEsercizio, IO.File.ReadAllBytes(percorsoFile), allegato.Nomefile)
    '                            End If
    '                        End If
    '                    Next
    '                End If

    '            Catch ex As Exception
    '                messaggio &= ex.Message
    '                Throw New ApplicationException(messaggio)
    '            End Try
    '        End If
    '    Next
    '    liquidazioniAlbo.Dispose()
    'End Sub

#End Region

#Region "GESTIONE PUBBLICAZIONE ON-LINE"
    ''' <summary>
    ''' In base al parametro settato (ServizioAlboPretorio) seleziona la versione corretta del servizio di albo pretorio da utilizzare
    ''' </summary>
    ''' <param name="pubblicazione"></param>
    ''' <remarks></remarks>
    ''' ' luca 02/07/2020
    '' ''Private Sub Pubblica(ByVal pubblicazione As ParsecMES.Pubblicazione)
    '' ''    Dim parametriR As New ParsecAdmin.ParametriRepository
    '' ''    Dim parametro As ParsecAdmin.Parametri = parametriR.GetByName("ServizioAlboPretorio")
    '' ''    If Not parametro Is Nothing Then
    '' ''        Select Case parametro.Valore
    '' ''            Case 0 'Albo pretorio classico
    '' ''                PubblicaServizio0(pubblicazione)
    '' ''            Case 1 'Nuovo Albo pretorio
    '' ''                PubblicaServizio1(pubblicazione)
    '' ''            Case Else 'Albo pretorio classico
    '' ''                PubblicaServizio0(pubblicazione) 'Albo pretorio classico
    '' ''        End Select
    '' ''    Else
    '' ''        PubblicaServizio0(pubblicazione)
    '' ''    End If
    '' ''    parametriR.Dispose()
    '' ''End Sub

    ''' <summary>
    ''' Albo pretorio classico (ASP.NET)
    ''' </summary>
    ''' <param name="pubblicazione"></param>
    ''' <remarks></remarks>
    ''' luca 02/07/2020
    '' ''Private Sub PubblicaServizio0(ByVal pubblicazione As ParsecMES.Pubblicazione)

    '' ''    Dim pubblicazioni As New ParsecMES.AlboRepository

    '' ''    Dim messaggio As String = String.Empty


    '' ''    Try

    '' ''        'If Not pubblicazione.Pubblicato Then

    '' ''        Dim clienti As New ParsecAdmin.ClientRepository
    '' ''        Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''        Dim codiceEnte As String = cliente.Identificativo
    '' ''        clienti.Dispose()

    '' ''        Dim anno As String = pubblicazione.DataRegistrazione.Value.Year.ToString

    '' ''        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")
    '' ''        Dim localPathAtti = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")



    '' ''        Dim ws As New PubblicazioneAlbo.wsPubblicazioneAlbo
    '' ''        ws.Timeout = -1

    '' ''        Dim percorsoNomeFile As String = String.Empty
    '' ''        Dim percorsoNomeFileFirmato As String = String.Empty

    '' ''        pubblicazione.Documenti = pubblicazioni.GetDocumenti(pubblicazione.Id)

    '' ''        Dim documentoPrimario = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 1).FirstOrDefault
    '' ''        If Not documentoPrimario Is Nothing Then

    '' ''            percorsoNomeFile = localPath & anno & "\" & documentoPrimario.Nomefile
    '' ''            percorsoNomeFileFirmato = localPath & anno & "\" & documentoPrimario.NomeFileFirmato

    '' ''            Dim nomefile As String = documentoPrimario.Nomefile

    '' ''            If Not String.IsNullOrEmpty(documentoPrimario.NomeFileFirmato) Then
    '' ''                If IO.File.Exists(percorsoNomeFileFirmato) Then
    '' ''                    If IO.File.Exists(percorsoNomeFile) Then
    '' ''                        messaggio = PubblicazioneConFileFirmatoDigitalmente(pubblicazione, percorsoNomeFile, percorsoNomeFileFirmato, codiceEnte, ws)
    '' ''                    Else
    '' ''                        messaggio = "Il documento primario " & documentoPrimario.Nomefile & " non esiste!"
    '' ''                        'ERRORE: C'E' LA RIGA MA NON C'E' IL FILE
    '' ''                    End If
    '' ''                Else
    '' ''                    messaggio = "Il documento primario firmato " & documentoPrimario.NomeFileFirmato & " non esiste!"
    '' ''                    'ERRORE: C'E' LA RIGA MA NON C'E' IL FILE
    '' ''                End If
    '' ''            Else
    '' ''                If Not String.IsNullOrEmpty(documentoPrimario.Nomefile) Then
    '' ''                    If IO.File.Exists(percorsoNomeFile) Then
    '' ''                        messaggio = PubblicazioneSenzaFileFirmatoDigitalmente(pubblicazione, percorsoNomeFile, codiceEnte, ws)
    '' ''                    Else
    '' ''                        messaggio = "Il documento primario " & documentoPrimario.Nomefile & " non esiste!"
    '' ''                        'ERRORE: C'E' LA RIGA MA NON C'E' IL FILE
    '' ''                    End If
    '' ''                End If
    '' ''            End If
    '' ''        Else
    '' ''            messaggio = PubblicazioneSoloOggetto(pubblicazione, codiceEnte, ws)
    '' ''        End If

    '' ''        If String.IsNullOrEmpty(messaggio) Then
    '' ''            Dim documentiAllegati As List(Of ParsecMES.Documento) = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 2).ToList
    '' ''            Dim bytesNomefile As Byte() = Nothing
    '' ''            For Each doc In documentiAllegati
    '' ''                Dim percorsoFile As String = ""
    '' ''                If pubblicazione.IdModulo = CInt(ParsecAdmin.TipoModulo.ATT) Then
    '' ''                    'percorsoFile = localPathAtti & "ALL\" & doc.Nomefile
    '' ''                    percorsoFile = localPath & anno & "\" & doc.Nomefile
    '' ''                Else
    '' ''                    If pubblicazione.IdModulo = 9 Then
    '' ''                        percorsoFile = localPath & anno & "\" & doc.Nomefile
    '' ''                    End If
    '' ''                End If
    '' ''                If IO.File.Exists(percorsoFile) Then
    '' ''                    bytesNomefile = IO.File.ReadAllBytes(percorsoFile)
    '' ''                    Me.MessaggioAvviso &= ws.PubblicaAllegato(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, doc.Descrizione, bytesNomefile, doc.Nomefile)
    '' ''                    Me.MessaggioAvviso &= vbCrLf
    '' ''                End If
    '' ''            Next

    '' ''            If String.IsNullOrEmpty(messaggio) Then
    '' ''                '**********************************************************************************
    '' ''                'Aggiorno la pubblicazione
    '' ''                '**********************************************************************************

    '' ''                Dim pubbl As ParsecMES.Pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.Id = pubblicazione.Id).FirstOrDefault
    '' ''                pubbl.Pubblicato = True
    '' ''                pubblicazioni.SaveChanges()

    '' ''                Dim mailTo As String = String.Empty
    '' ''                If pubblicazione.IdDocumento > 0 Then
    '' ''                    mailTo = Me.GetDestinatariDocumento(pubblicazione)
    '' ''                Else
    '' ''                    If Not pubblicazione.Email Is Nothing Then
    '' ''                        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
    '' ''                        Dim m = emailRegex.Match(pubblicazione.Email.Email)
    '' ''                        If m.Success Then
    '' ''                            mailTo = pubblicazione.Email.Email
    '' ''                        End If
    '' ''                    End If
    '' ''                End If

    '' ''                If Not String.IsNullOrEmpty(mailTo) Then
    '' ''                    Dim emailsAlbo As New ParsecMES.EmailAlboRepository

    '' ''                    If pubblicazione.IdDocumento > 0 Then
    '' ''                        '**********************************************************************************
    '' ''                        'Inserisco una nuova email
    '' ''                        '**********************************************************************************
    '' ''                        Dim exist As Boolean = Not emailsAlbo.GetQuery.Where(Function(c) c.IdAlbo = pubblicazione.Id).FirstOrDefault Is Nothing
    '' ''                        If Not exist Then
    '' ''                            Dim email As New ParsecMES.EmailAlbo With {.IdAlbo = pubblicazione.Id, .Email = mailTo}
    '' ''                            emailsAlbo.Add(email)
    '' ''                            emailsAlbo.SaveChanges()
    '' ''                        End If
    '' ''                    End If

    '' ''                    Try
    '' ''                        '**********************************************************************************
    '' ''                        'Invio l'email anche se non ci sono allegati. 
    '' ''                        '**********************************************************************************
    '' ''                        If CheckConfigurazioneInvioEmail() Then
    '' ''                            Me.InviaEmailAvvenutaPubblicazione(pubblicazione, mailTo, percorsoNomeFile, percorsoNomeFileFirmato)
    '' ''                        End If

    '' ''                        'IGNORO EVENTUALE ERRORE CASUATO DALL'INVIO DELL'EMAIL
    '' ''                    Catch ex As Exception
    '' ''                    End Try

    '' ''                    Try

    '' ''                        '**********************************************************************************
    '' ''                        'Aggiorno i dati di pubblicazione dell'email
    '' ''                        '**********************************************************************************
    '' ''                        Dim email As ParsecMES.EmailAlbo = emailsAlbo.GetQuery.Where(Function(c) c.IdAlbo = pubblicazione.Id).FirstOrDefault
    '' ''                        If Not email Is Nothing Then
    '' ''                            email.Pubblicata = True
    '' ''                            email.DataPubblicazione = Now
    '' ''                            emailsAlbo.SaveChanges()
    '' ''                        End If

    '' ''                    Catch ex As Exception
    '' ''                        messaggio &= vbCrLf
    '' ''                        messaggio &= ex.Message
    '' ''                    End Try
    '' ''                    emailsAlbo.Dispose()

    '' ''                Else

    '' ''                    'Me.MessaggioAvviso &= "Destinatari/o assenti - Impossibile inviare e-mail avvenuta pubblicazione"
    '' ''                End If

    '' ''            End If

    '' ''        End If

    '' ''        'End If


    '' ''    Catch ex As Exception
    '' ''        messaggio &= vbCrLf
    '' ''        messaggio &= ex.Message
    '' ''    Finally
    '' ''        pubblicazioni.Dispose()
    '' ''    End Try

    '' ''    If Not String.IsNullOrEmpty(messaggio) Then
    '' ''        Throw New ApplicationException(messaggio)
    '' ''    End If


    '' ''End Sub


    ''' <summary>
    ''' Nuovo Albo pretorio (Liferay)
    ''' </summary>
    ''' <param name="pubblicazione"></param>
    ''' <remarks></remarks>
    ''' ' luca 02/07/2020
    '' ''Private Sub PubblicaServizio1(ByVal pubblicazione As ParsecMES.Pubblicazione)
    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    Dim pubblicazioni As New ParsecMES.AlboRepository

    '' ''    Dim messaggio As String = String.Empty


    '' ''    Try

    '' ''        'If Not pubblicazione.Pubblicato Then

    '' ''        Dim clienti As New ParsecAdmin.ClientRepository
    '' ''        Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''        Dim codiceEnte As String = cliente.Identificativo
    '' ''        clienti.Dispose()

    '' ''        Dim anno As String = pubblicazione.DataRegistrazione.Value.Year.ToString
    '' ''        Dim numeroAtto As Integer = IIf(pubblicazione.ContatoreDocumento Is Nothing, 0, pubblicazione.ContatoreDocumento)
    '' ''        Dim dataAtto As Date = IIf(pubblicazione.DataDocumento Is Nothing, "01-01-1900", pubblicazione.DataDocumento)

    '' ''        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")
    '' ''        Dim localPathAtti = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")



    '' ''        Dim ws As New PubblicazioneAlbo.PubblicazioneAlboPretorioServiceSoapService
    '' ''        ws.Timeout = -1

    '' ''        Dim percorsoNomeFile As String = String.Empty
    '' ''        Dim percorsoNomeFileFirmato As String = String.Empty

    '' ''        pubblicazione.Documenti = pubblicazioni.GetDocumenti(pubblicazione.Id)

    '' ''        Dim documentoPrimario = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 1).FirstOrDefault
    '' ''        If Not documentoPrimario Is Nothing Then
    '' ''            percorsoNomeFile = localPath & anno & "\" & documentoPrimario.Nomefile
    '' ''            percorsoNomeFileFirmato = localPath & anno & "\" & documentoPrimario.NomeFileFirmato
    '' ''        End If

    '' ''        'Pubblicazione dati
    '' ''        Try
    '' ''            Dim pubblicazioneOnLine = ws.pubblica(codiceEnte, cliente.CodLicenza, utenteCollegato.Username, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, pubblicazione.DescrizioneTipologia, pubblicazione.NumeroRegistro, pubblicazione.DataRegistrazione, pubblicazione.Oggetto, pubblicazione.Struttura, "", "", "", numeroAtto, dataAtto)
    '' ''        Catch ex As Exception
    '' ''            messaggio = ex.Message
    '' ''        End Try

    '' ''        If String.IsNullOrEmpty(messaggio) Then
    '' ''            'Pubblicazione documenti allegati
    '' ''            Dim documenti As List(Of ParsecMES.Documento) = pubblicazione.Documenti.ToList
    '' ''            Dim documentiFirmati As List(Of ParsecMES.Documento) = (From doc In pubblicazione.Documenti Select New ParsecMES.Documento With {
    '' ''                                                                                                  .Id = doc.Id,
    '' ''                                                                                                  .IdAlbo = doc.IdAlbo,
    '' ''                                                                                                  .Nomefile = doc.NomeFileFirmato,
    '' ''                                                                                                  .IdTipologia = doc.IdTipologia,
    '' ''                                                                                                  .Descrizione = doc.Descrizione,
    '' ''                                                                                                  .NomeFileFirmato = doc.NomeFileFirmato,
    '' ''                                                                                                  .Impronta = doc.Impronta
    '' ''                                                                                                  }).ToList


    '' ''            ' Dim documentiAllegati As List(Of ParsecMES.Documento) = documenti.Union(documentiFirmati).OrderBy(Function(d) d.Id).ThenBy(Function(d) d.Nomefile).ToList

    '' ''            Dim bytesNomefile As Byte() = Nothing
    '' ''            For Each doc In documenti.Union(documentiFirmati).OrderBy(Function(d) d.Id).ThenBy(Function(d) d.Nomefile)
    '' ''                Dim percorsoFile As String = ""
    '' ''                If pubblicazione.IdModulo = CInt(ParsecAdmin.TipoModulo.ATT) Then
    '' ''                    'percorsoFile = localPathAtti & "ALL\" & doc.Nomefile
    '' ''                    percorsoFile = localPath & anno & "\" & doc.Nomefile
    '' ''                Else
    '' ''                    If pubblicazione.IdModulo = 9 Then
    '' ''                        percorsoFile = localPath & anno & "\" & doc.Nomefile
    '' ''                    End If
    '' ''                End If
    '' ''                If IO.File.Exists(percorsoFile) Then
    '' ''                    bytesNomefile = IO.File.ReadAllBytes(percorsoFile)
    '' ''                    Dim principale As Boolean
    '' ''                    Select Case doc.IdTipologia
    '' ''                        Case 1 'Documento Primario
    '' ''                            principale = True
    '' ''                        Case 2 'Documento Secondario
    '' ''                            principale = False
    '' ''                    End Select
    '' ''                    Try
    '' ''                        Dim documentoOnLine = ws.pubblicaDocumento(codiceEnte, cliente.CodLicenza, pubblicazione.NumeroRegistro, Year(pubblicazione.DataRegistrazione), doc.Nomefile, bytesNomefile, doc.Descrizione, principale, True)
    '' ''                    Catch ex As Exception
    '' ''                        Me.MessaggioAvviso &= "Allegato '" & doc.Nomefile & "' non pubblicato - " & ex.Message & vbCrLf
    '' ''                    End Try
    '' ''                End If
    '' ''            Next

    '' ''            If String.IsNullOrEmpty(messaggio) Then
    '' ''                '**********************************************************************************
    '' ''                'Aggiorno la pubblicazione
    '' ''                '**********************************************************************************

    '' ''                Dim pubbl As ParsecMES.Pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.Id = pubblicazione.Id).FirstOrDefault
    '' ''                pubbl.Pubblicato = True
    '' ''                pubblicazioni.SaveChanges()

    '' ''                Dim mailTo As String = String.Empty
    '' ''                If pubblicazione.IdDocumento > 0 Then
    '' ''                    mailTo = Me.GetDestinatariDocumento(pubblicazione)
    '' ''                Else
    '' ''                    If Not pubblicazione.Email Is Nothing Then
    '' ''                        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
    '' ''                        Dim m = emailRegex.Match(pubblicazione.Email.Email)
    '' ''                        If m.Success Then
    '' ''                            mailTo = pubblicazione.Email.Email
    '' ''                        End If
    '' ''                    End If
    '' ''                End If

    '' ''                If Not String.IsNullOrEmpty(mailTo) Then
    '' ''                    Dim emailsAlbo As New ParsecMES.EmailAlboRepository

    '' ''                    If pubblicazione.IdDocumento > 0 Then
    '' ''                        '**********************************************************************************
    '' ''                        'Inserisco una nuova email
    '' ''                        '**********************************************************************************
    '' ''                        Dim exist As Boolean = Not emailsAlbo.GetQuery.Where(Function(c) c.IdAlbo = pubblicazione.Id).FirstOrDefault Is Nothing
    '' ''                        If Not exist Then
    '' ''                            Dim email As New ParsecMES.EmailAlbo With {.IdAlbo = pubblicazione.Id, .Email = mailTo}
    '' ''                            emailsAlbo.Add(email)
    '' ''                            emailsAlbo.SaveChanges()
    '' ''                        End If
    '' ''                    End If

    '' ''                    Try
    '' ''                        '**********************************************************************************
    '' ''                        'Invio l'email anche se non ci sono allegati. 
    '' ''                        '**********************************************************************************
    '' ''                        If CheckConfigurazioneInvioEmail() Then
    '' ''                            Me.InviaEmailAvvenutaPubblicazione(pubblicazione, mailTo, percorsoNomeFile, percorsoNomeFileFirmato)
    '' ''                        End If

    '' ''                        'IGNORO EVENTUALE ERRORE CASUATO DALL'INVIO DELL'EMAIL
    '' ''                    Catch ex As Exception
    '' ''                    End Try

    '' ''                    Try

    '' ''                        '**********************************************************************************
    '' ''                        'Aggiorno i dati di pubblicazione dell'email
    '' ''                        '**********************************************************************************
    '' ''                        Dim email As ParsecMES.EmailAlbo = emailsAlbo.GetQuery.Where(Function(c) c.IdAlbo = pubblicazione.Id).FirstOrDefault
    '' ''                        If Not email Is Nothing Then
    '' ''                            email.Pubblicata = True
    '' ''                            email.DataPubblicazione = Now
    '' ''                            emailsAlbo.SaveChanges()
    '' ''                        End If

    '' ''                    Catch ex As Exception
    '' ''                        messaggio &= vbCrLf
    '' ''                        messaggio &= ex.Message
    '' ''                    End Try
    '' ''                    emailsAlbo.Dispose()

    '' ''                Else

    '' ''                    'Me.MessaggioAvviso &= "Destinatari/o assenti - Impossibile inviare e-mail avvenuta pubblicazione"
    '' ''                End If

    '' ''            End If

    '' ''        End If

    '' ''        'End If


    '' ''    Catch ex As Exception
    '' ''        messaggio &= vbCrLf
    '' ''        messaggio &= ex.Message
    '' ''    Finally
    '' ''        pubblicazioni.Dispose()
    '' ''    End Try

    '' ''    If Not String.IsNullOrEmpty(messaggio) Then
    '' ''        Throw New ApplicationException(messaggio)
    '' ''    End If


    '' ''End Sub

    ' luca 02/07/2020
    '' ''Private Function GetDestinatariDocumento(pubblicazione As ParsecMES.Pubblicazione) As String
    '' ''    Dim mailTo As String = String.Empty
    '' ''    Dim firmeDocumento As New ParsecAtt.DocumentiFirmeRepository
    '' ''    Dim firme = firmeDocumento.GetQuery.Where(Function(c) c.IdDocumento = pubblicazione.IdDocumento).GroupBy(Function(c) c.IdUtente).Select(Function(c) c.FirstOrDefault).Select(Function(c) c.IdUtente).ToList
    '' ''    Dim utenti As New ParsecAdmin.UserRepository
    '' ''    Dim emails = utenti.GetQuery.Where(Function(c) firme.Contains(c.Id) And c.Email <> "" And c.Email <> ";").Select(Function(c) c).Select(Function(c) c.Email).ToList
    '' ''    utenti.Dispose()
    '' ''    firmeDocumento.Dispose()

    '' ''    Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)

    '' ''    For Each email In emails
    '' ''        Dim m = emailRegex.Match(email)
    '' ''        If m.Success Then
    '' ''            mailTo &= email & ";"
    '' ''        End If
    '' ''    Next
    '' ''    If mailTo.EndsWith(";") Then
    '' ''        mailTo = mailTo.Remove(mailTo.Length - 1)
    '' ''    End If
    '' ''    Return mailTo
    '' ''End Function

    Private Function CheckConfigurazioneInvioEmail() As Boolean
        Dim successo As Boolean = False
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("MittenteEmail", ParsecAdmin.TipoModulo.ATT)
        parametri.Dispose()
        If Not parametro Is Nothing Then
            Dim casellePec As New ParsecAdmin.ParametriPecRepository
            Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetQuery.Where(Function(c) c.Email = parametro.Valore).FirstOrDefault
            If Not casellaPec Is Nothing Then
                successo = True
            End If
            casellePec.Dispose()
        End If
        parametri.Dispose()
        Return successo
    End Function

    'Private Function ConfiguraCdoEmail() As MailMessage

    '    Dim parametri As New ParsecAdmin.ParametriRepository
    '    Dim parametro = parametri.GetByName("MittenteEmail", ParsecAdmin.TipoModulo.ATT)
    '    parametri.Dispose()
    '    'If parametro Is Nothing Then
    '    '    Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "Il parametro 'MittenteEmail' non è presente.")
    '    '    Exit Function
    '    'End If

    '    Const cdoBasic As Integer = 1
    '    Const cdoSendUsingPort As Integer = 2
    '    Dim mail As MailMessage = Nothing
    '    Dim casellePec As New ParsecAdmin.ParametriPecRepository
    '    Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetQuery.Where(Function(c) c.Email = parametro.Valore).FirstOrDefault
    '    ' If Not casellaPec Is Nothing Then

    '    mail = New MailMessage
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", casellaPec.SmtpServer)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", casellaPec.SmtpPorta)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", casellaPec.UserId)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", casellaPec.Password)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", casellaPec.SmtpIsSSL)
    '    mail.From = casellaPec.Email
    '    mail.BodyFormat = MailFormat.Html
    '    mail.Priority = MailPriority.High
    '    SmtpMail.SmtpServer = casellaPec.SmtpServer & ":" & casellaPec.SmtpPorta.ToString
    '    'Else
    '    'Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "La casella di posta '" & parametro.Valore & "' non è presente.")
    '    'Exit Function
    '    'End If
    '    Return mail
    'End Function


    Private Function CheckEmail(ByVal Indirizzo As String) As Boolean
        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
        Dim m = emailRegex.Match(Indirizzo)
        Return m.Success
    End Function




    'Private Sub InviaEmailAvvenutaPubblicazione(pubblicazione As ParsecMES.Pubblicazione, ByVal mailTo As String, ByVal pathNomefile As String, ByVal pathNomefilefirmato As String)

    '    Dim parametri As New ParsecAdmin.ParametriRepository
    '    Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AvvenutaPubblicazione", ParsecAdmin.TipoModulo.SEP)
    '    parametri.Dispose()
    '    Dim body As String = String.Empty

    '    If Not parametro Is Nothing Then
    '        body = parametro.Valore
    '        body = body.Replace("#Documento#", "<b>" & pubblicazione.Oggetto & "</b>")
    '        body = body.Replace("#numeroAlbo#", "<b>" & pubblicazione.NumeroRegistro.ToString & "</b>")
    '        body = body.Replace("#datapubblicazione#", "<b>" & Now.ToShortDateString & "</b>")
    '        body = body.Replace("#datada#", "<b>" & pubblicazione.DataInizioPubblicazione.Value.ToShortDateString & "</b>")
    '        body = body.Replace("#dataa#", "<b>" & pubblicazione.DataFinePubblicazione.Value.ToShortDateString & "</b>")
    '        body &= "<br/>Codice (ID) referta <b>" & pubblicazione.Id.ToString & "</b><br/>N.B.E-mail generata automaticamente dal sistema SEP, non rispondere, indirizzo inesistente!"
    '    End If

    '    Dim mail As MailMessage = Nothing

    '    Try
    '        mail = Me.ConfiguraCdoEmail
    '    Catch ex As Exception
    '        Throw New ApplicationException(ex.Message)
    '        Exit Sub
    '    End Try

    '    mail.To = mailTo
    '    mail.Subject = "Pubblicazione dell'atto numero di registrazione " & pubblicazione.NumeroRegistro.ToString
    '    mail.Body = body

    '    If IO.File.Exists(pathNomefile) Then
    '        mail.Attachments.Add(New MailAttachment(pathNomefile))
    '    End If

    '    If IO.File.Exists(pathNomefilefirmato) Then
    '        mail.Attachments.Add(New MailAttachment(pathNomefilefirmato))
    '    End If

    '    Try
    '        '********************************************************************************************************************
    '        'L'invio di PEC con indirizzi in copia nascosta (BCC o CCN) non è permesso dalla normativa 
    '        'sulla posta elettronica certificata in quanto nascondono l'indirizzo del destinatario.
    '        '********************************************************************************************************************
    '        'Try
    '        '    mail.Bcc = ParsecAdmin.WebConfigSettings.GetKey("BCCEmail")
    '        'Catch ex As Exception
    '        'End Try
    '        '********************************************************************************************************************
    '        SmtpMail.Send(mail)
    '    Catch ex As Exception
    '        Throw New ApplicationException(ex.Message)
    '    End Try

    'End Sub

    ' luca 02/07/2020
    '' ''Private Sub InviaEmailAvvenutaPubblicazione(pubblicazione As ParsecMES.Pubblicazione, ByVal mailTo As String, ByVal pathNomefile As String, ByVal pathNomefilefirmato As String)

    '' ''    Try

    '' ''        Dim parametri As New ParsecAdmin.ParametriRepository
    '' ''        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AvvenutaPubblicazione", ParsecAdmin.TipoModulo.SEP)

    '' ''        Dim body As String = String.Empty

    '' ''        If Not parametro Is Nothing Then
    '' ''            body = parametro.Valore
    '' ''            body = body.Replace("#Documento#", "<b>" & pubblicazione.Oggetto & "</b>")
    '' ''            body = body.Replace("#numeroAlbo#", "<b>" & pubblicazione.NumeroRegistro.ToString & "</b>")
    '' ''            body = body.Replace("#datapubblicazione#", "<b>" & Now.ToShortDateString & "</b>")
    '' ''            body = body.Replace("#datada#", "<b>" & pubblicazione.DataInizioPubblicazione.Value.ToShortDateString & "</b>")
    '' ''            body = body.Replace("#dataa#", "<b>" & pubblicazione.DataFinePubblicazione.Value.ToShortDateString & "</b>")
    '' ''            body &= "<br/>Codice (ID) referta <b>" & pubblicazione.Id.ToString & "</b><br/>N.B.E-mail generata automaticamente dal sistema SEP, non rispondere, indirizzo inesistente!"
    '' ''        End If

    '' ''        parametro = parametri.GetByName("MittenteEmail", ParsecAdmin.TipoModulo.ATT)
    '' ''        parametri.Dispose()

    '' ''        If parametro Is Nothing Then
    '' ''            Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "Il parametro 'MittenteEmail' non è presente.")
    '' ''        End If

    '' ''        Dim casellePec As New ParsecAdmin.ParametriPecRepository
    '' ''        Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetQuery.Where(Function(c) c.Email = parametro.Valore).FirstOrDefault
    '' ''        casellePec.Dispose()

    '' ''        If casellaPec Is Nothing Then
    '' ''            Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "La casella di posta '" & parametro.Valore & "' non è presente.")
    '' ''        End If

    '' ''        Dim client As Rebex.Net.Smtp = Me.ConfigureSmtp(casellaPec)

    '' ''        Dim mail As New Rebex.Mail.MailMessage
    '' ''        Dim mailAttach As Rebex.Mail.Attachment = Nothing

    '' ''        mail.From = casellaPec.Email

    '' ''        Dim listaEmail = mailTo.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)

    '' ''        For Each s In listaEmail
    '' ''            If Me.CheckEmail(s) Then
    '' ''                If Not mail.To.Contains(s) Then
    '' ''                    mail.To.Add(s)
    '' ''                End If
    '' ''            End If
    '' ''        Next

    '' ''        mail.Subject = "Pubblicazione dell'atto numero di registrazione " & pubblicazione.NumeroRegistro.ToString
    '' ''        mail.BodyHtml = body
    '' ''        mail.Priority = Rebex.Mail.MailPriority.High

    '' ''        If IO.File.Exists(pathNomefile) Then
    '' ''            mailAttach = New Rebex.Mail.Attachment(pathNomefile)
    '' ''            mailAttach.FileName = IO.Path.GetFileName(pathNomefile)
    '' ''            mail.Attachments.Add(mailAttach)
    '' ''        End If

    '' ''        If IO.File.Exists(pathNomefilefirmato) Then
    '' ''            mailAttach = New Rebex.Mail.Attachment(pathNomefilefirmato)
    '' ''            mailAttach.FileName = IO.Path.GetFileName(pathNomefilefirmato)
    '' ''            mail.Attachments.Add(mailAttach)
    '' ''        End If

    '' ''        client.Timeout = 0
    '' ''        client.Send(mail)
    '' ''        client.Disconnect()

    '' ''    Catch ex As Exception
    '' ''        Throw New ApplicationException(ex.Message)
    '' ''    End Try
    '' ''End Sub


    ' luca 02/07/2020
    '' ''Private Function PubblicazioneConFileFirmatoDigitalmente(ByVal pubblicazione As ParsecMES.Pubblicazione, ByVal percorsoNomeFile As String, percorsoNomeFileFirmato As String, codiceEnte As String, ByVal ws As PubblicazioneAlbo.wsPubblicazioneAlbo) As String
    '' ''    Dim ret As String = String.Empty
    '' ''    Dim nomefile As String = IO.Path.GetFileName(percorsoNomeFile)
    '' ''    Dim nomefileFirmato As String = IO.Path.GetFileName(percorsoNomeFileFirmato)
    '' ''    Dim bytesNomefile As Byte() = IO.File.ReadAllBytes(percorsoNomeFile)
    '' ''    Dim bytesNomefileFirmato As Byte() = IO.File.ReadAllBytes(percorsoNomeFileFirmato)
    '' ''    If pubblicazione.ContatoreDocumento > 0 Then
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.ContatoreDocumento, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, bytesNomefile, nomefile, bytesNomefileFirmato, nomefileFirmato)
    '' ''    Else
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, bytesNomefile, nomefile, bytesNomefileFirmato, nomefileFirmato)
    '' ''    End If
    '' ''    Return ret
    '' ''End Function

    ' luca 02/07/2020
    '' ''Private Function PubblicazioneSenzaFileFirmatoDigitalmente(ByVal pubblicazione As ParsecMES.Pubblicazione, ByVal percorsoNomeFile As String, codiceEnte As String, ByVal ws As PubblicazioneAlbo.wsPubblicazioneAlbo) As String
    '' ''    Dim ret As String = String.Empty
    '' ''    Dim bytesNomefile As Byte() = IO.File.ReadAllBytes(percorsoNomeFile)
    '' ''    Dim nomefile As String = IO.Path.GetFileName(percorsoNomeFile)
    '' ''    If pubblicazione.ContatoreDocumento > 0 Then
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.ContatoreDocumento, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, bytesNomefile, nomefile)
    '' ''    Else
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, bytesNomefile, nomefile)
    '' ''    End If
    '' ''    Return ret
    '' ''End Function

    ' luca 02/07/2020
    '' ''Private Function PubblicazioneSoloOggetto(ByVal pubblicazione As ParsecMES.Pubblicazione, codiceEnte As String, ByVal ws As PubblicazioneAlbo.wsPubblicazioneAlbo) As String
    '' ''    Dim ret As String = String.Empty
    '' ''    If pubblicazione.ContatoreDocumento > 0 Then
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.ContatoreDocumento, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione)
    '' ''    Else
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione)
    '' ''    End If
    '' ''    Return ret
    '' ''End Function

#End Region

#Region "GESTIONE GRIGLIA NOTE"

    Public Class InfoNote
        Public Property Utente As String = String.Empty
        Public Property Note As String = String.Empty
        Public Property Id As Integer = 0
        Public Property Data As DateTime = Nothing
    End Class

    Protected Sub NoteGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles NoteGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim noteLabel As Label = CType(e.Item.FindControl("NoteLabel"), Label)
            Dim copyButton As ImageButton = CType(dataItem("Copy").Controls(0), ImageButton)
            copyButton.Attributes.Add("onclick", "javascript:SetClipboard('" & noteLabel.Text & "');")
        End If
    End Sub

    Protected Sub NoteGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles NoteGridView.NeedDataSource
        If Not Me.TaskAttivo Is Nothing Then
            Dim utenti As New ParsecWKF.UtenteViewRepository
            Dim tasks As New ParsecWKF.TaskRepository(utenti.Context)

            Dim res = From utente In utenti.GetQuery
                      Join task In tasks.GetQuery
                      On utente.Id Equals task.Mittente
                      Where task.IdIstanza = Me.TaskAttivo.IdIstanza And task.Note <> "" And task.IdStato = 6
                      Select utente, task

            Me.StoricoNote = res.AsEnumerable.Select(Function(c) New InfoNote With {
                                                   .Utente = c.utente.Username.ToUpper & If(Not String.IsNullOrEmpty(c.utente.Cognome), " - " & c.utente.Cognome, "") & If(Not String.IsNullOrEmpty(c.utente.Nome), " " & c.utente.Nome, ""),
                                                   .Data = c.task.DataEsecuzione,
                                                   .Note = c.task.Note,
                                                   .Id = c.task.Id
                                               }).ToList


            Me.NoteGridView.DataSource = Me.StoricoNote

        End If

    End Sub

#End Region



#Region "GESTIONE FIRMA (FIRMA - COFIRMA)"

    Private Sub FirmaATT(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'todo
        'per verificare se esiste una firma associata al ruolo
        'Dim roleFromName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, Me.Action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Sender).FirstOrDefault.Name
        'Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleFromName).FirstOrDefault

        'If Not role Is Nothing Then
        '    Dim idDocumento As Integer = Me.TaskAttivo.IdDocumento
        '    Dim idRuolo = role.Id
        '    Dim documenti As New ParsecAtt.DocumentoRepository
        '    Dim firma As ParsecAtt.Firma = documenti.GetFirme(idDocumento).Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault

        '    If Not firma Is Nothing Then

        '    End If
        'End If



        Dim messaggioAvviso As String = String.Empty
        'luca 01/07/2020
        '' ''Dim pubblicaTrasparenza As Boolean = Not Me.Action.GetParameterByName("PubblicaTrasparenza") Is Nothing
        '' ''Try
        '' ''    If pubblicaTrasparenza Then
        '' ''        Dim documenti As New ParsecAtt.DocumentoRepository
        '' ''        Dim documento = documenti.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
        '' ''        If Not documento Is Nothing Then
        '' ''            documento.Trasparenza = documenti.GetTrasparenza(documento.Id)
        '' ''            If Not documento.Trasparenza Is Nothing Then
        '' ''                Me.PubblicaTrasparenza(documento)
        '' ''            End If
        '' ''        End If
        '' ''        documenti.Dispose()
        '' ''    End If
        '' ''Catch ex As Exception
        '' ''    messaggioAvviso = ex.Message
        '' ''End Try

        If Not String.IsNullOrEmpty(messaggioAvviso) Then
            ParsecUtility.Utility.MessageBox(messaggioAvviso, False)
        End If


        ' luca 02/07/2020
        '' ''Dim pubblicazioneOnline = Me.Action.GetParameterByName("PubblicazioneOnline")

        '' ''If Not pubblicazioneOnline Is Nothing Then
        '' ''    Me.EseguiFirmaPubblicazione()
        '' ''Else
        Dim cofirma = Me.Action.GetParameterByName("Cofirma")
        If cofirma Is Nothing Then
            Me.EseguiFirma()
        Else
            Me.EseguiCofirma()
        End If
        '' ''End If

    End Sub

    Private Sub EseguiCofirma()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)


        Dim disabilitaControlloDelega = Me.Action.GetParameterByName("DisabilitaControlloDelega")

        If Not disabilitaControlloDelega Is Nothing Then
            If Me.TaskAttivo.IdAttoreCorrente <> utenteCollegato.Id Then
                Dim utente = (New ParsecAdmin.UserRepository).GetUserById(Me.TaskAttivo.IdAttoreCorrente).FirstOrDefault
                Dim nominativo As String = "un'altro utente"

                Dim message As New StringBuilder
                message.AppendLine("Attenzione!")
                If Not utente Is Nothing Then
                    nominativo = utente.Cognome.ToUpper & " " & utente.Nome
                End If

                message.AppendLine("Non è consentito cofirmare in luogo di " & nominativo & ".")
                Throw New ApplicationException(message.ToString)
            End If
        End If


        Dim roleFromName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, Me.Action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Sender).FirstOrDefault.Name
        Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleFromName).FirstOrDefault

        If Not role Is Nothing Then


            Dim idDocumento As Integer = Me.TaskAttivo.IdDocumento
            Dim idRuolo = role.Id
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim firma As ParsecAtt.Firma = documenti.GetFirme(idDocumento).Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault

            If Not firma Is Nothing Then

                Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = idDocumento).FirstOrDefault

                If Not documento Is Nothing Then


                    Dim firmaDigitalmente As Boolean = Me.CheckAbilitazioneFirmaDigitale(utenteCollegato)

                    '**************************************************************************
                    '1) Aggiorno il nome del file firmato della firma
                    '**************************************************************************
                    Dim firmeDocumento As New ParsecAtt.DocumentiFirmeRepository
                    Dim idFirma = firma.Id
                    Dim firmaDocumento = firmeDocumento.GetQuery.Where(Function(c) c.IdFirma = idFirma And c.IdDocumento = idDocumento).FirstOrDefault
                    If Not firmaDocumento Is Nothing Then
                        Dim nomeFileFirmato As String = documento.Nomefile


                        If firmaDigitalmente Then
                            nomeFileFirmato = String.Format("{0}.pdf.p7m", IO.Path.GetFileNameWithoutExtension(documento.Nomefile))
                        End If

                        firmaDocumento.NomeFile = nomeFileFirmato
                        firmeDocumento.SaveChanges()
                    End If
                    firmeDocumento.Dispose()
                    '**************************************************************************

                    If Not OperazioneMassiva Then
                        If firmaDigitalmente Then
                            '**************************************************************************
                            'COFIRMO IL DOCUMENTO P7M
                            '**************************************************************************
                            Me.FirmaDocumento(documento.Nomefile)
                        Else
                            Dim idDestinatario As Integer = Me.GetIdDestinatario()
                            'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
                            Me.WriteTaskAndUpdateParent(idDestinatario)
                            Me.SbloccaTasks(utenteCollegato.Id)
                        End If
                    Else
                        'MEMORIZZO IL NOME DEL FILE DEL DOCUMENTO DA FIRMARE

                        If Me.FileToSignDictionary Is Nothing Then
                            Me.FileToSignDictionary = New Dictionary(Of Integer, InfoFirma)
                        End If
                        Me.FileToSignDictionary.Add(Me.TaskAttivo.Id, New InfoFirma With {.NomeFile = documento.Nomefile})
                    End If


                End If
            End If
            documenti.Dispose()
        End If
    End Sub

    Private Function GetFirmaRuolo() As ParsecAtt.Firma

        Dim roleFromName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, Me.Action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Sender).FirstOrDefault.Name
        Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleFromName).FirstOrDefault
        Dim firma As ParsecAtt.Firma = Nothing
        If Not role Is Nothing Then
            Dim idDocumento As Integer = Me.TaskAttivo.IdDocumento
            Dim idRuolo = role.Id
            Dim documenti As New ParsecAtt.DocumentoRepository
            firma = documenti.GetFirme(idDocumento).Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault
        End If
        Return firma

    End Function

    Private Sub EseguiFirma()
        'Dim fromActor = Me.Action.FromActor

        Dim roleFromName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, Me.Action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Sender).FirstOrDefault.Name
        Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleFromName).FirstOrDefault



        If Not role Is Nothing Then

            Dim idDocumento As Integer = Me.TaskAttivo.IdDocumento
            Dim idRuolo = role.Id
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim firma As ParsecAtt.Firma = documenti.GetFirme(idDocumento).Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault

            If Not firma Is Nothing Then

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                'LA PRIMA VOLTA CHE ESEGUO LA FIRMA
                If Me.FileToSignDictionary Is Nothing Then
                    If Me.TaskAttivo.IdAttoreCorrente <> utenteCollegato.Id Then
                        Me.RegistraScriptPersistenzaVisibilitaPannello()
                        ParsecUtility.Utility.MessageBox("Attenzione stai firmando al posto di un'altro utente!", False)
                    End If
                End If


                Dim disabilitaAggiornamentoDataFirma = Me.Action.GetParameterByName("DisabilitaAggiornamentoDataFirma")

                '**************************************************************************************
                '1) Aggiorno la data della firma.
                '**************************************************************************************
                Dim idFirma As Integer = firma.Id

                Dim firmeDocumento As New ParsecAtt.DocumentiFirmeRepository

                Dim firmaDocumento As ParsecAtt.DocumentoFirma = Nothing

                'Se il parametro non esiste.

                If disabilitaAggiornamentoDataFirma Is Nothing Then
                    firmaDocumento = firmeDocumento.GetQuery.Where(Function(c) c.IdFirma = idFirma And c.IdDocumento = idDocumento).FirstOrDefault
                    If Not firmaDocumento Is Nothing Then

                        'MEMORIZZO LA DATA DELLA FIRMA IN CASO DI PROBLEMI DURANTE L'ESECUZIONE DELLA FIRMA
                        Me.DataFirma = firmaDocumento.Data

                        'firmaDocumento.Data = Now
                        Dim parametri As New ParsecAdmin.ParametriRepository
                        firmaDocumento.Data = parametri.GetDataValida
                        parametri.Dispose()
                        firmeDocumento.SaveChanges()
                    End If
                End If
                '**************************************************************************************



                If Me.TaskAttivo.IdAttoreCorrente <> utenteCollegato.Id Then

                    '**************************************************************************************
                    '2) Aggiorno il firmatario
                    '**************************************************************************************
                    If Not firmaDocumento Is Nothing Then
                        Dim delegato As String = If(Not String.IsNullOrEmpty(utenteCollegato.Titolo), utenteCollegato.Titolo & " ", "") & If(Not String.IsNullOrEmpty(utenteCollegato.Nome), utenteCollegato.Nome & " ", "") & If(Not String.IsNullOrEmpty(utenteCollegato.Cognome), utenteCollegato.Cognome, "")
                        firmaDocumento.Struttura = delegato
                    End If
                    firmeDocumento.SaveChanges()
                    '**************************************************************************************
                End If


                '**************************************************************************************
                '3) Rigenero l'atto amministrativo
                '**************************************************************************************

                Dim documento As ParsecAtt.Documento = documenti.Clone(idDocumento)
                documento = documenti.LoadOggettiCollegati(documento)
                documento = documenti.LoadAllegatiPubblicazione(documento)

                Dim numerazioneSettore = Me.Action.GetParameterByName("NumerazioneSettore")



                If Not numerazioneSettore Is Nothing Then
                    Dim parametri As New ParsecAdmin.ParametriRepository
                    Dim parametroMomentoNumerazioneSettore As ParsecAdmin.Parametri = parametri.GetByName("MomentoNumerazioneSettore", ParsecAdmin.TipoModulo.ATT)
                    If Not parametroMomentoNumerazioneSettore Is Nothing Then
                        If parametroMomentoNumerazioneSettore.Valore <> "3" Then
                            Throw New ApplicationException("Il parametro di sistema 'MomentoNumerazioneSettore' non è configurato correttamente." & vbCrLf & "Contattare gli amministratori del sistema.")
                        End If
                    End If
                    parametri.Dispose()
                    documento.AssegnaNumeroSettore = True
                End If


                documento.LogIdUtente = utenteCollegato.Id
                documento.LogUtente = utenteCollegato.Username
                documento.IdAutore = utenteCollegato.Id

                'Dim nomeFileDaVerificare As String = documento.Nomefile

                Try
                    '*******************************************************************
                    'Gestione storico
                    '*******************************************************************
                    documenti.Documento = documento
                    If Not documento Is Nothing Then
                        documenti.Documento.Modello = documento.Modello
                        documenti.Documento.Seduta = documento.Seduta
                    End If
                    documenti.TipologiaProcedura = ParsecAtt.TipoProcedura.Rigenera

                    Dim firmaDigitalmente As Boolean = Me.CheckAbilitazioneFirmaDigitale(utenteCollegato)

                    '*******************************************************************
                    '4) Aggiorno il nome del file firmato della firma
                    '*******************************************************************

                    If firmaDigitalmente Then
                        Dim f = documento.Firme.Where(Function(c) c.Id = firma.Id).FirstOrDefault
                        f.FirmatoDigitalmente = True
                    End If

                    '*******************************************************************

                    documenti.Save(documento)
                    documento = documenti.Documento


                    '*********************************************************************
                    'CREO UNA COPIA DI BACKUP DEL DOCUMENTO ODT IN CASO DI ANNULLAMENTO 
                    'DELLA FIRMA CHE UTILIZZERO' PER RIPRISTINARE IL DOCUMENTO ORIGINALE
                    '*********************************************************************
                    Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")
                    Dim srcFilename As String = String.Format("{0}{1}\{2}", localPath, documento.AnnoEsercizio, documento.Nomefile)
                    Dim destFilename As String = String.Format("{0}{1}\{2}", localPath, documento.AnnoEsercizio, IO.Path.GetFileNameWithoutExtension(documento.Nomefile) & "_Backup" & IO.Path.GetExtension(documento.Nomefile))
                    IO.File.Copy(srcFilename, destFilename, True)

                    If Me.BackupFileToDeleteDictionary Is Nothing Then
                        Me.BackupFileToDeleteDictionary = New Dictionary(Of Integer, String)
                    End If
                    Me.BackupFileToDeleteDictionary.Add(Me.TaskAttivo.Id, destFilename)

                    If Not OperazioneMassiva Then




                        '*******************************************************************
                        'MEMORIZZO I DATASOURCE
                        '*******************************************************************
                        If Me.DataSources Is Nothing Then
                            Me.DataSources = New List(Of String)
                        End If
                        Me.DataSources.Add(documento.DataSource)
                        Me.Nomefile = documento.Nomefile


                        '*******************************************************************
                        'RIGENERO IL DOCUMENTO ODT E FIRMO
                        '*******************************************************************
                        'UTILIZZO L'APPLET
                        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                            Me.RegistraScriptOpenOffice(documento, Me.firmaDocumentoButton.ClientID)
                        Else

                            'TODO ESEGUIRE IL QUESTO COMANDO DA UN PULSANTE NASCOSTO ALTRIMENTI I COMANDI REGISTRATI DINAMICAMENTE NELLA PAGINA
                            'VERRANNO ESEGUITI A FINE ELABORAZIONE (AD ESEMPIO IL MESSAGGIO:
                            'ParsecUtility.Utility.MessageBox("Attenzione stai firmando al posto di un'altro utente!", False))

                            'UTILIZZO IL SOCKET
                            Me.RegistraScriptOpenOffice(documento, AddressOf Me.FirmaDocumentoCallback)
                        End If


                    Else
                        '*******************************************************************
                        'MEMORIZZO I DATASOURCE
                        '*******************************************************************
                        If Me.DataSources Is Nothing Then
                            Me.DataSources = New List(Of String)
                        End If
                        Me.DataSources.Add(documento.DataSource)

                        '*******************************************************************
                        'MEMORIZZO IL NOME DEL FILE DEL DOCUMENTO DA FIRMARE
                        '*******************************************************************
                        If Me.FileToSignDictionary Is Nothing Then
                            Me.FileToSignDictionary = New Dictionary(Of Integer, InfoFirma)
                        End If
                        Me.FileToSignDictionary.Add(Me.TaskAttivo.Id, New InfoFirma With {.NomeFile = documento.Nomefile})


                    End If


                    '**************************************************************************************



                    '*******************************************************************
                    '5) Aggiorno il nome del file firmato della firma
                    '*******************************************************************
                    firma = documenti.GetFirme(documento.Id).Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault
                    idFirma = firma.Id


                    firmaDocumento = firmeDocumento.GetQuery.Where(Function(c) c.IdFirma = idFirma And c.IdDocumento = documento.Id).FirstOrDefault
                    If Not firmaDocumento Is Nothing Then
                        Dim nomeFileFirmato As String = documento.Nomefile

                        If firmaDigitalmente Then
                            nomeFileFirmato = String.Format("{0}.pdf.p7m", IO.Path.GetFileNameWithoutExtension(documento.Nomefile))
                            firmaDocumento.FirmatoDigitalmente = True
                        End If

                        firmaDocumento.NomeFile = nomeFileFirmato
                        firmeDocumento.SaveChanges()



                    End If
                    '*******************************************************************

                    firmeDocumento.Dispose()


                    'Aggiorno il riferimento al documento.
                    Me.AggiornaIstanzaWorkflow(documento)

                Catch ex As Exception
                    Throw New ApplicationException(ex.Message)
                Finally
                    documenti.Dispose()
                End Try
            Else
                'todo

            End If
        End If

    End Sub

    Private Sub VerificaCorpoDocumentoCallback()
        Dim output = System.Convert.FromBase64String(Me.checkContentHidden.Value)
        Me.checkContentHidden.Value = String.Empty

        Dim ms As New IO.MemoryStream(output)
        Dim ds As New DataSet
        ds.ReadXml(ms)
        Dim sb As New StringBuilder
        'todo verificare se table(0) esiste
        For Each row As DataRow In ds.Tables(0).Rows
            Dim idTask As Integer = CInt(row("RowId"))
            Dim result As Boolean = CBool(row("Result"))
            Dim description As String = CStr(row("Description"))
            '*******************************************************************
            'RIGENERO IL DOCUMENTO ODT E LO FIRMO
            '*******************************************************************
            If result Then
                'Me.RegistraScriptPersistenzaVisibilitaPannello()
                'ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(Me.DataSources(0), Me.firmaDocumentoButton.ClientID, Me.documentContentHidden.ClientID, False, False)
                'Me.DataSources = Nothing
                Me.FirmaATT(Nothing)
            Else
                sb.AppendLine("ATTENZIONE!")
                sb.AppendLine("")
                sb.AppendLine(description)
                Dim max = 0
                If Me.TaskAttivo.Documento.Length > description.Length - 7 Then
                    max = description.Length - 7
                    sb.AppendLine("Rif. " & Me.TaskAttivo.Documento.Substring(0, max) & "...")
                Else
                    sb.AppendLine("Rif. " & Me.TaskAttivo.Documento)
                End If
                sb.AppendLine("")
                sb.AppendLine("Contattare gli amministratori del sistema.")
                Me.RegistraScriptPersistenzaVisibilitaPannello()
                ParsecUtility.Utility.MessageBox(sb.ToString, False)
                ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
            End If
            Exit For
        Next
    End Sub

    Protected Sub verificaCorpoDocumentoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles verificaCorpoDocumentoButton.Click
        Me.VerificaCorpoDocumentoCallback()
    End Sub

    Private Sub VerificaMassivaCorpoDocumentiCallback()
        '*******************************************************************************
        'LEGGO I VALORI RESTITUITI DALLA VERIFICA MASSIVA
        '*******************************************************************************
        Dim output = System.Convert.FromBase64String(Me.checkContentHidden.Value)
        Me.checkContentHidden.Value = String.Empty

        Dim ms As New IO.MemoryStream(output)
        Dim ds As New DataSet
        ds.ReadXml(ms)

        Dim sb As New StringBuilder
        Dim i As Integer = 0
        Dim max As Integer = 0
        For Each row As DataRow In ds.Tables(0).Rows
            Dim idTask As Integer = CInt(row("RowId"))
            Dim result As Boolean = CBool(row("Result"))
            Dim description As String = CStr(row("Description"))
            If Not result Then
                'MEMORIZZO I MESSAGGI DA VISUALIZZARE ALLA FINE DELL'OPERAZIONE 
                'SE IL PARAMETRO AbilitaControlloCorpoDocumento E' PRESENTE
                If i = 0 Then
                    sb.AppendLine("ATTENZIONE!")
                    sb.AppendLine("")
                End If
                sb.AppendLine(description)

                Dim task As ParsecWKF.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = idTask).FirstOrDefault

                max = 0
                If task.Documento.Length > description.Length - 7 Then
                    max = description.Length - 7
                    sb.AppendLine("Rif. " & task.Documento.Substring(0, max) & "...")
                Else
                    sb.AppendLine("Rif. " & task.Documento)
                End If
                sb.AppendLine("")

                'ELIMINO I TASK CHE NON HANNO AVUTO UN ESITO POSITIVO
                If Not task Is Nothing Then
                    Me.TaskAttivi.Remove(task)
                End If
                i += 1
            End If
        Next

        If sb.Length > 0 Then
            sb.AppendLine("")
            sb.AppendLine("Contattare gli amministratori del sistema.")
        End If
        Me.MessaggioAvviso = sb.ToString

        'ESEGUO SOLO I TASK CHE HANNO AVUTO UN ESITO POSITIVO
        If Me.TaskAttivi.Count > 0 Then
            Me.FirmaMassivaAtti(Nothing)
        Else

            If Not String.IsNullOrEmpty(Me.MessaggioAvviso) Then
                Me.RegistraScriptPersistenzaVisibilitaPannello()
                ParsecUtility.Utility.MessageBox(sb.ToString, False)
            End If

            Me.MessaggioAvviso = Nothing

            Me.FirstClick = False
            'NON FUNZIONA SU CHROME
            Me.RegistraButtonClick()
        End If
    End Sub

    Protected Sub verificaMassivaCorpoDocumentiButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles verificaMassivaCorpoDocumentiButton.Click
        Me.VerificaMassivaCorpoDocumentiCallback()
    End Sub


    Private Sub FirmaDocumentoCallback()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)


        Dim abilitaFirmaElettronica As ParsecWKF.ParametroProcesso = Me.Action.GetParameterByName("AbilitaFirmaElettronica")

        If Not abilitaFirmaElettronica Is Nothing Then
            If Not String.IsNullOrEmpty(utenteCollegato.NomefileCertificato) Then
                If Not Me.Nomefile Is Nothing Then
                    Me.FirmaDeboleDocumento(Me.Nomefile)
                    If Me.Action.Type = "FIRMA" Then
                        Me.Nomefile = Nothing
                    End If
                    Exit Sub
                End If
            Else
                Dim idDestinatario As Integer = Me.GetIdDestinatario()
                'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
                Me.WriteTaskAndUpdateParent(idDestinatario)
                Me.SbloccaTasks(utenteCollegato.Id)
            End If
        End If

        Dim firmaDigitalmente As Boolean = Me.CheckAbilitazioneFirmaDigitale(utenteCollegato)
        If firmaDigitalmente Then
            If Not Me.Nomefile Is Nothing Then
                Me.FirmaDocumento(Me.Nomefile)
                If Me.Action.Type = "FIRMA" Then
                    Me.Nomefile = Nothing
                End If
            End If
        Else
            Dim idDestinatario As Integer = Me.GetIdDestinatario()
            'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
            Me.WriteTaskAndUpdateParent(idDestinatario)
            Me.SbloccaTasks(utenteCollegato.Id)
        End If
    End Sub

    '*********************************************************************************************************
    'QUESTO PULSANTE VIENE PRESSATO DAL COMPONENTE PARSECOPENOFFICE PER AVVIARE IL PROCESSO DI FIRMA
    '*********************************************************************************************************
    Protected Sub firmaDocumentoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles firmaDocumentoButton.Click
        Me.FirmaDocumentoCallback()
    End Sub

    Private Sub FirmaDocumento(ByVal nomeFile As String)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato

        Dim provider As ParsecAdmin.ProviderType = CType(utenteCorrente.IdProviderFirma, ParsecAdmin.ProviderType)
        If provider = ProviderType.Nessuno Then
            provider = ProviderType.PkNet   'Valore predefinito
        End If

        Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
        Dim annoEsercizio As String = rgx.Match(nomeFile).Value
        Dim pathFileToSign As String = String.Empty
        pathFileToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomeFile)

        Dim cofirma As ParsecWKF.ParametroProcesso = Nothing
        Dim parametriProcesso = ParsecWKF.ModelloInfo.ReadParameters(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
        If Not parametriProcesso Is Nothing Then
            cofirma = parametriProcesso.Where(Function(c) c.Nome = "Cofirma").FirstOrDefault
        End If
        Dim functionName As String = "SignDocument"
        If Not cofirma Is Nothing Then
            Dim p7m As String = IO.Path.GetFileNameWithoutExtension(nomeFile) & ".pdf.p7m"
            functionName = "CoSignDocument"
            pathFileToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, p7m)
        End If

        Dim signParameters As New ParsecAdmin.SignParameters
        Dim datiInput As New ParsecAdmin.DatiFirma With {
            .RemotePathToSign = pathFileToSign,
            .Provider = provider,
            .FunctionName = functionName,
            .CertificateId = certificateId
        }

        Dim data As String = signParameters.CreaDataSource(datiInput)
        Me.RegistraScriptPersistenzaVisibilitaPannello()


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaIterButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.AggiornaIter)
        End If



    End Sub

    Private Sub FirmaDeboleDocumento(ByVal nomeFile As String)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato
        Dim provider As ParsecAdmin.ProviderType = ProviderType.CryptoApi
        Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
        Dim annoEsercizio As String = rgx.Match(nomeFile).Value
        Dim remotePathToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomeFile)
        Dim remotePathCertificate = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeCertificati"), utenteCorrente.NomefileCertificato)

        Dim signParameters As New ParsecAdmin.SignParameters
        Dim datiInput As New ParsecAdmin.DatiFirma With {
            .RemotePathToSign = remotePathToSign,
            .Provider = provider,
            .CertificateId = certificateId,
            .RemotePathCertificate = remotePathCertificate,
            .FunctionType = FunctionType.WeakSignDocument
        }

        Dim data As String = signParameters.CreaDataSource(datiInput)
        Me.RegistraScriptPersistenzaVisibilitaPannello()


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaIterButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.AggiornaIter)
        End If


    End Sub

    Private Sub AnnullaFirma()

        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza = istanze.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
        istanze.Dispose()

        If Not istanza Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim idDocumento As Integer = istanza.IdDocumento

            'OTTENGO L'OGGETTO COMPLETO
            Dim documento = documenti.GetFullById(idDocumento)
            documento.NascondiDocumento = True
            documento.Rigenera = False

            If Not documento Is Nothing Then
                Dim roleFromName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, Me.Action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Sender).FirstOrDefault.Name
                Dim ruoli As New ParsecWKF.RuoloRepository
                Dim role = ruoli.GetQuery.Where(Function(c) c.Descrizione = roleFromName).FirstOrDefault
                ruoli.Dispose()
                If Not role Is Nothing Then
                    Dim idRuolo = role.Id

                    Dim firma As ParsecAtt.Firma = documento.Firme.Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault

                    If Not firma Is Nothing Then

                        'ANNULLO LE INFORMAZIONI DELLA FIRMA SELEZIONATA ASSOCIATA AL DOCUMENTO CORRENTE
                        Dim idFirma As Integer = firma.Id
                        Dim firmeDocumento As New ParsecAtt.DocumentiFirmeRepository
                        Dim firmaDocumento = firmeDocumento.GetQuery.Where(Function(c) c.IdFirma = idFirma And c.IdDocumento = idDocumento).FirstOrDefault
                        If Not firmaDocumento Is Nothing Then

                            '*********************************************************************************************************
                            'SE HO AGGIORNATO LA DATA DELLA FIRMA ANNULLO LE MODIFICHE
                            'LA PROPRIETA' DATAFIRMA VIENE VALORIZZATA SOLO SE NON C'E' IL PARAMETRO DisabilitaAggiornamentoDataFirma
                            '*********************************************************************************************************
                            If Not Me.DataFirma Is Nothing Then
                                If Me.DataFirma.HasValue Then
                                    firmaDocumento.Data = Me.DataFirma
                                    Me.DataFirma = Nothing
                                End If
                            End If
                            'firmaDocumento.Data = Nothing
                            '*********************************************************************************************************

                            firmaDocumento.NomeFile = documento.Nomefile
                            firmaDocumento.FirmatoDigitalmente = False
                            firmeDocumento.SaveChanges()
                        End If
                        firmeDocumento.Dispose()

                        'RIGENERO IL DOCUMENTO ODT SENZA INFORMAZIONI DELLA FIRMA
                        firma.FirmatoDigitalmente = False
                        firma.FileFirmato = String.Empty
                        'documenti.GeneraDataSource(documento)
                        'Me.RegistraScriptOpenOffice(documento, Me.AggiornaTaskButton.ClientID)

                        '*******************************************************************
                        'RECUPERO LA COPIA DI BACKUP DEL DOCUMENTO ODT 
                        'CHE UTILIZZO PER RIPRISTINARE IL DOCUMENTO
                        '*******************************************************************
                        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")
                        Dim srcFilename As String = String.Format("{0}{1}\{2}", localPath, documento.AnnoEsercizio, documento.Nomefile)
                        Dim destFilename As String = String.Format("{0}{1}\{2}", localPath, documento.AnnoEsercizio, IO.Path.GetFileNameWithoutExtension(documento.Nomefile) & "_Backup" & IO.Path.GetExtension(documento.Nomefile))

                        If IO.File.Exists(destFilename) Then
                            IO.File.Delete(srcFilename)
                            IO.File.Move(destFilename, srcFilename)
                            'CANCELLO IL FILE DI BACKUP
                            IO.File.Delete(destFilename)
                        End If

                        Me.RegistraButtonClick()

                    End If
                End If
            End If

            documenti.Dispose()

        End If


    End Sub

    '*******************************************************************************************************************
    'QUESTO PULSANTE VIENE PRESSATO DAL COMPONENTE PARSECOPENOFFICE IN CASO DI ANNULLAMENTO DEL PROCESSO DI FIRMA
    '*******************************************************************************************************************
    Protected Sub AnnullaFirmaDocumentoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFirmaDocumentoButton.Click
        ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
    End Sub

    Private Function CheckAbilitazioneFirmaDigitale(ByVal utenteCollegato As ParsecAdmin.Utente) As Boolean
        Dim res As Boolean = False
        Dim abilitatoFirmaDigitale As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AbilitaFirmaDigitale)).FirstOrDefault Is Nothing
        Dim disabilitaFirmaDigitale As ParsecWKF.ParametroProcesso = Me.Action.GetParameterByName("DisabilitaFirmaDigitale")

        If abilitatoFirmaDigitale Then
            'Se il parametro non esiste.
            If disabilitaFirmaDigitale Is Nothing Then
                res = True
            End If
        End If
        Return res
    End Function

#End Region

#Region "GESTIONE FIRMA (PARAMETRO PUBBLICAZIONEONLINE)"

    'TODO TESTARE CON ALBO PRETORIO DISABILITATO
    ' luca 02/07/2020
    '' ''Private Sub EseguiFirmaPubblicazione()

    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    Dim firmaDigitalmente As Boolean = Me.CheckAbilitazioneFirmaDigitale(utenteCollegato)

    '' ''    'LA PRIMA VOLTA CHE ESEGUO LA FIRMA
    '' ''    If Me.FileToSignDictionary Is Nothing Then
    '' ''        If Me.TaskAttivo.IdAttoreCorrente <> utenteCollegato.Id Then
    '' ''            Me.RegistraScriptPersistenzaVisibilitaPannello()
    '' ''            ParsecUtility.Utility.MessageBox("Attenzione stai firmando al posto di un'altro utente!", False)
    '' ''        End If
    '' ''    End If


    '' ''    If firmaDigitalmente Then
    '' ''        Dim pubblicazioni As New ParsecMES.AlboRepository

    '' ''        Dim pubblicazione As ParsecMES.Pubblicazione = pubblicazioni.GetView(New ParsecMES.FiltroPubblicazione With {.IdDocumento = Me.TaskAttivo.IdDocumento, .IdModulo = ParsecAdmin.TipoModulo.ATT}).FirstOrDefault
    '' ''        pubblicazione.Documenti = pubblicazioni.GetDocumenti(pubblicazione.Id)
    '' ''        pubblicazioni.Dispose()

    '' ''        If Not Me.OperazioneMassiva Then
    '' ''            Me.FirmaPubblicazione(pubblicazione)
    '' ''        Else
    '' ''            'MEMORIZZO I FILE DA FIRMARE E L'ANNO
    '' ''            If Me.FileToSignDictionary Is Nothing Then
    '' ''                Me.FileToSignDictionary = New Dictionary(Of Integer, InfoFirma)
    '' ''            End If
    '' ''            Dim annoEsercizio As String = pubblicazione.DataRegistrazione.Value.Year.ToString
    '' ''            Dim documentoPrimario = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 1).FirstOrDefault
    '' ''            Dim nomefile As String = documentoPrimario.Nomefile
    '' ''            Me.FileToSignDictionary.Add(Me.TaskAttivo.Id, New InfoFirma With {.Anno = annoEsercizio, .NomeFile = nomefile, .EntitaId = Me.TaskAttivo.IdDocumento})
    '' ''        End If
    '' ''    Else
    '' ''        Dim idDestinatario As Integer = Me.GetIdDestinatario()
    '' ''        'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
    '' ''        Me.WriteTaskAndUpdateParent(idDestinatario)
    '' ''        Me.SbloccaTasks(utenteCollegato.Id)
    '' ''    End If
    '' ''End Sub

    ' luca 02/07/2020
    '' ''Private Sub FirmaPubblicazione(ByVal pubblicazione As ParsecMES.Pubblicazione)

    '' ''    Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    Dim certificateId As String = utenteCorrente.IdCertificato

    '' ''    Dim provider As ParsecAdmin.ProviderType = CType(utenteCorrente.IdProviderFirma, ParsecAdmin.ProviderType)
    '' ''    If provider = ProviderType.Nessuno Then
    '' ''        provider = ProviderType.PkNet   'Valore predefinito
    '' ''    End If


    '' ''    Dim annoEsercizio As String = pubblicazione.DataRegistrazione.Value.Year.ToString
    '' ''    Dim documentoPrimario = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 1).FirstOrDefault
    '' ''    Dim nomefile As String = documentoPrimario.Nomefile

    '' ''    Dim pathFileToSign As String = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiAlbo"), annoEsercizio, nomefile)

    '' ''    Dim cofirma = Me.Action.GetParameterByName("Cofirma")

    '' ''    Dim functionName As String = "SignDocument"
    '' ''    If Not cofirma Is Nothing Then
    '' ''        Dim p7m As String = IO.Path.GetFileNameWithoutExtension(nomefile) & ".pdf.p7m"
    '' ''        functionName = "CoSignDocument"
    '' ''        pathFileToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiAlbo"), annoEsercizio, p7m)
    '' ''    End If



    '' ''    Dim signParameters As New ParsecAdmin.SignParameters
    '' ''    Dim datiInput As New ParsecAdmin.DatiFirma With {
    '' ''        .RemotePathToSign = pathFileToSign,
    '' ''        .Provider = provider,
    '' ''        .FunctionName = functionName,
    '' ''        .CertificateId = certificateId
    '' ''    }

    '' ''    Dim data As String = signParameters.CreaDataSource(datiInput)
    '' ''    Me.RegistraScriptPersistenzaVisibilitaPannello()

    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    'UTILIZZO L'APPLET
    '' ''    If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
    '' ''        ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.PubblicaOnlineButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
    '' ''    Else
    '' ''        'UTILIZZO IL SOCKET
    '' ''        ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.PubblicaOnlineCallback)
    '' ''    End If



    '' ''End Sub

    ' luca 02/07/2020
    '' ''Private Sub PubblicaOnlineCallback()
    '' ''    '****************************************************************
    '' ''    'SE HO FIRMATO
    '' ''    '****************************************************************
    '' ''    Dim successo As Boolean = (Me.signerOutputHidden.Value = "OK")
    '' ''    If successo Then

    '' ''        Dim messaggio As String = String.Empty
    '' ''        Dim pubblicazioneOnline = Me.Action.Parameters.Where(Function(c) c.Nome = "PubblicazioneOnline").FirstOrDefault
    '' ''        If Not pubblicazioneOnline Is Nothing Then

    '' ''            '*******************************************************************************
    '' ''            'AGGIORNO IL DOCUMENTO PRIMARIO CON IL NOME DEL FILE FIRMATO
    '' ''            '*******************************************************************************
    '' ''            Dim pubblicazioni As New ParsecMES.AlboRepository

    '' ''            Dim pubblicazione As ParsecMES.Pubblicazione = pubblicazioni.GetView(New ParsecMES.FiltroPubblicazione With {.IdDocumento = Me.TaskAttivo.IdDocumento, .IdModulo = ParsecAdmin.TipoModulo.ATT}).FirstOrDefault

    '' ''            pubblicazione.Documenti = pubblicazioni.GetDocumenti(pubblicazione.Id)

    '' ''            Dim idAlbo = pubblicazione.Id
    '' ''            Dim documenti As New ParsecMES.DocumentiRepository
    '' ''            Dim documentoPrimario = documenti.Where(Function(c) c.IdAlbo = idAlbo And c.IdTipologia = 1).FirstOrDefault
    '' ''            If Not documentoPrimario Is Nothing Then
    '' ''                documentoPrimario.NomeFileFirmato = documentoPrimario.Nomefile & ".p7m"
    '' ''                documenti.SaveChanges()
    '' ''            End If
    '' ''            documenti.Dispose()
    '' ''            pubblicazioni.Dispose()
    '' ''            '*******************************************************************************

    '' ''            Try
    '' ''                Me.Pubblica(pubblicazione)
    '' ''            Catch ex As Exception
    '' ''                successo = False
    '' ''                messaggio = ex.Message
    '' ''            End Try

    '' ''            If successo Then
    '' ''                'Me.signerOutputHidden.Value = "OK"
    '' ''                If Not String.IsNullOrEmpty(Me.MessaggioAvviso) Then
    '' ''                    ParsecUtility.Utility.MessageBox(Me.MessaggioAvviso, False)
    '' ''                    Me.MessaggioAvviso = String.Empty
    '' ''                End If
    '' ''                Me.AggiornaIterATT()
    '' ''            Else

    '' ''                If Not String.IsNullOrEmpty(Me.MessaggioAvviso) Then
    '' ''                    messaggio &= vbCrLf & Me.MessaggioAvviso
    '' ''                    Me.MessaggioAvviso = String.Empty
    '' ''                End If

    '' ''                ParsecUtility.Utility.MessageBox(messaggio, False)
    '' ''                'Me.EnableUiHidden.Value = "Abilita"
    '' ''                ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

    '' ''            End If
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    '*********************************************************************************************************
    'QUESTO PULSANTE VIENE PRESSATO DAL COMPONENTE PARSECDIGITALSIGN PER AVVIARE LA PUBBLICAZIONE
    '*********************************************************************************************************
    ' luca 02/07/2020
    '' ''Protected Sub PubblicaOnlineButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles PubblicaOnlineButton.Click
    '' ''    Me.PubblicaOnlineCallback()
    '' ''End Sub

#End Region

#Region "GESTIONE FIRMA MASSIVA"




    'Private Shared Function TaskAttivoPredicate(t As ParsecWKF.TaskAttivo) As Boolean
    '    Return True
    'End Function

    Private Sub FirmaMassiva(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.ATT Then


            'Metodo normale 
            'Me.TaskAttivi.RemoveAll(AddressOf TaskAttivoPredicate)

            'Metodo anonimo
            'Me.TaskAttivi.RemoveAll(Function(c)
            '                            Return True
            '                        End Function)

            'Espressione lambda
            'Me.TaskAttivi.RemoveAll(Function(c) True)


            '***********************************************************************************************************************************************************
            '
            '***********************************************************************************************************************************************************
            Dim abilitaControlloImpegniSpesa As Boolean = Not Me.Action.GetParameterByName("AbilitaControlloImpegniSpesa") Is Nothing
            If abilitaControlloImpegniSpesa Then

                Dim elencoErrori As New StringBuilder
                Dim documenti As New ParsecAtt.DocumentoRepository
                For Each taskCorrente In Me.TaskAttivi.ToArray
                    Dim impegni = documenti.GetImpegniSpesa(taskCorrente.IdDocumento)
                    'SE NON CI SONO IMPEGNI DI SPESA
                    If Not impegni.Any Then
                        'MEMORIZZO L'ECCEZIONE
                        elencoErrori.AppendLine("  " & taskCorrente.DescrizioneDocumento.Split("-")(0) & " - Specificare almeno un impegno di spesa!")
                        'Me.TaskAttivi.Remove(taskCorrente)
                    Else
                        Dim impegniSpesaSenzaNumeroImpegno = impegni.Where(Function(c) c.NumeroImpegno Is Nothing Or String.IsNullOrEmpty(c.NumeroImpegno)).Any
                        'SE CI SONO IMPEGNI DI SPESA SENZA NUMERO DI IMPEGNO
                        If impegniSpesaSenzaNumeroImpegno Then
                            'MEMORIZZO L'ECCEZIONE
                            elencoErrori.AppendLine("  " & taskCorrente.DescrizioneDocumento.Split("-")(0) & " - Specificare il numero di impegno!")
                            'Me.TaskAttivi.Remove(taskCorrente)
                        End If
                    End If
                Next
                documenti.Dispose()

                'SE CI SONO ERRORI
                If elencoErrori.Length > 0 Then
                    'SE NON CI SONO TASK DA ESEGUIRE
                    elencoErrori.Insert(0, "Impossibile eseguire la FIRMA per i seguenti documenti:" & vbCrLf)
                    'If Me.TaskAttivi.Count = 0 Then

                    'Me.EnableUiHidden.Value = "Abilita"
                    'ParsecUtility.Utility.MessageBox(elencoErrori.ToString, False)

                    'CHIUDE LA FINESTRA DELLE OPERAZIONI 
                    'Me.RegistraButtonClick()
                    'ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

                    Throw New ApplicationException(elencoErrori.ToString)

                    Exit Sub
                    'Else
                    '    'MEMORIZZO LE ECCEZIONI CHE VERRANNO VISUALIZZATE DOPO L'OPERAZIONE DI FIRMA MASSIVA
                    '    Me.ElencoErrori = elencoErrori.ToString
                    'End If
                End If
            End If
            '***********************************************************************************************************************************************************


            Dim abilitaControlloCorpoDocumento As Boolean = Not Me.Action.GetParameterByName("AbilitaControlloCorpoDocumento") Is Nothing
            'ESEGUO LA VERIFICA MASSIVA DEI DOCUMENTI
            If abilitaControlloCorpoDocumento Then

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                'UTILIZZO L'APPLET
                If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                    Me.RegistraScriptVerificaCorpoOpenOffice(Me.verificaMassivaCorpoDocumentiButton.ClientID, True)
                Else
                    'UTILIZZO IL SOCKET
                    Me.RegistraScriptVerificaCorpoOpenOffice(AddressOf Me.VerificaMassivaCorpoDocumentiCallback, True)
                End If
            Else
                Me.FirmaMassivaAtti(e)
            End If
        End If
        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
            Me.FirmaMassivaDocumentiGenerici()
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.IOL Then
            ParsecUtility.Utility.MessageBox("Funzione non implementata!", False)
            Me.EnableUiHidden.Value = "Abilita"
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.CSRA Then
            'ParsecUtility.Utility.MessageBox("Funzione non implementata!", False)
            'Me.EnableUiHidden.Value = "Abilita"
            FirmaMassivaCSRA(e)
        End If

    End Sub

    Private Sub FirmaMassivaAtti(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        For Each taskCorrente In Me.TaskAttivi
            Me.TaskAttivo = taskCorrente
            Me.FirmaATT(e)

            'Se questa funzione viene chiamata dal datalist
            If Not e Is Nothing Then
                Dim idDestinatario As Integer = Me.GetIdDestinatario()
                Me.ElencoDestinatariDictionary.Add(taskCorrente.Id, idDestinatario)
            Else
                'Se questa funzione viene chiamata dal item del menu di contesto

                If Me.RuoloId = 0 Then
                    Dim idDestinatario As Integer = Me.GetIdDestinatario()
                    Me.ElencoDestinatariDictionary.Add(taskCorrente.Id, idDestinatario)
                Else
                    Me.ElencoDestinatariDictionary.Add(taskCorrente.Id, Me.RuoloId)
                End If
            End If
        Next
        If Not e Is Nothing Then
            Me.RuoloId = Nothing
        End If


        Dim pubblicazioneOnline = Me.Action.GetParameterByName("PubblicazioneOnline")
        If Not pubblicazioneOnline Is Nothing Then
            Me.FirmaMassivaPubblicazioni(Me.FileToSignDictionary)
            'Me.FileToSignDictionary = Nothing
        Else
            Dim cofirma = Me.Action.GetParameterByName("Cofirma")
            If cofirma Is Nothing Then
                'ELABORO TUTTI I DOCUMENTI (LA CONCLUSIONE DELL'ELABORAZIONE AVVIA LA FIRMA DI TUTTI I DOCUMENTI ASSOCIATI AI TASK SELEZIONATI)
                Me.RegistraScriptPersistenzaVisibilitaPannello()


                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                'UTILIZZO L'APPLET
                If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                    ParsecUtility.Utility.RegistraScriptElaborazioneMassiva(Me.DataSources, Me.firmaMassivaDocumentiButton.ClientID, True, False)
                Else
                    'UTILIZZO IL SOCKET
                     ParsecUtility.Utility.EseguiServerComunicatorService(Me.DataSources, True, AddressOf Me.EseguiFirmaMassivaDocumenti)
                End If


                Me.DataSources = Nothing
            Else
                'AVVIO LA COFIRMA DI TUTTI I DOCUMENTI ASSOCIATI AI TASK SELEZIONATI
                Me.EseguiFirmaMassivaDocumenti()
            End If
        End If
    End Sub

    Private Sub FirmaMassivaPubblicazioni(ByVal fileToSignDictionary As Dictionary(Of Integer, InfoFirma))

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato

        Dim signParameters As New ParsecAdmin.SignParameters
        Dim datiSrc As New ParsecAdmin.DatiFirmaSrc

        Dim cofirma = Me.Action.GetParameterByName("Cofirma")

        For Each item As KeyValuePair(Of Integer, InfoFirma) In fileToSignDictionary

            Dim infoFirma As InfoFirma = item.Value
            Dim annoEsercizio As String = infoFirma.Anno
            Dim fileToSign As String = infoFirma.NomeFile

            Dim p7m As String = IO.Path.GetFileNameWithoutExtension(fileToSign) & ".pdf.p7m"

            Dim pathFileToSign As String = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiAlbo"), annoEsercizio, fileToSign)

            If Not cofirma Is Nothing Then
                pathFileToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiAlbo"), annoEsercizio, p7m)
            End If
            datiSrc.RowInfos.Add(item.Key, pathFileToSign)


        Next

        Dim datiInput As New DatiFirma
        datiInput.RemotePathToSign = ""
        datiInput.RemotePathCertificate = ""
        datiInput.FunctionType = FunctionType.MultiSignDocument
        If Not cofirma Is Nothing Then
            datiInput.FunctionType = FunctionType.MultiCosignDocument
        End If

        datiInput.CertificateId = certificateId
        datiInput.Provider = ProviderType.PkNet

        Dim data As String = signParameters.CreaDataSource(datiInput, datiSrc)

        'Me.RegistraScriptPersistenzaVisibilitaPannello()

        ' luca 02/07/2020
        '' ''Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        ' '' ''UTILIZZO L'APPLET
        '' ''If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
        '' ''    ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.PubblicaOnlineMassivaButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
        '' ''Else
        '' ''    'UTILIZZO IL SOCKET
        '' ''    ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.PubblicaOnlineMassivaCallback)
        '' ''End If

    End Sub

    ' luca 02/07/2020
    '' ''Private Sub PubblicaOnlineMassivaCallback()
    '' ''    Dim messaggio As New StringBuilder

    '' ''    Select Case Me.signerOutputHidden.Value
    '' ''        Case "OK"

    '' ''        Case "ERRORE"


    '' ''        Case Else

    '' ''            '****************************************************************
    '' ''            'SE HO FIRMATO
    '' ''            '****************************************************************

    '' ''            Dim pubblicazioni As New ParsecMES.AlboRepository
    '' ''            Dim documenti As New ParsecMES.DocumentiRepository
    '' ''            Dim successo As Boolean = False

    '' ''            '*******************************************************************************
    '' ''            'Leggo i valori restituiti dalla firma massiva
    '' ''            '*******************************************************************************
    '' ''            Dim output = System.Convert.FromBase64String(Me.signerOutputHidden.Value)
    '' ''            Dim ms As New IO.MemoryStream(output)
    '' ''            Dim ds As New DataSet
    '' ''            ds.ReadXml(ms)
    '' ''            Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
    '' ''            Dim operazione As String = Me.Action.Description.ToUpper

    '' ''            For Each row As DataRow In ds.Tables(0).Rows
    '' ''                Dim idTask As Integer = CInt(row("RowId"))
    '' ''                Dim result As Boolean = CBool(row("Result"))
    '' ''                Me.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = idTask).FirstOrDefault

    '' ''                If result Then
    '' ''                    successo = True
    '' ''                    Dim item = Me.FileToSignDictionary.Where(Function(c) c.Key = idTask).FirstOrDefault
    '' ''                    Dim infoFirma As InfoFirma = item.Value
    '' ''                    Dim fileToSign = infoFirma.NomeFile
    '' ''                    Dim idDocumento As Integer = infoFirma.EntitaId



    '' ''                    Dim pubblicazione As ParsecMES.Pubblicazione = pubblicazioni.GetView(New ParsecMES.FiltroPubblicazione With {.IdDocumento = idDocumento, .IdModulo = ParsecAdmin.TipoModulo.ATT}).FirstOrDefault


    '' ''                    Try

    '' ''                        '*******************************************************************************
    '' ''                        'AGGIORNO IL DOCUMENTO PRIMARIO CON IL NOME DEL FILE FIRMATO
    '' ''                        '*******************************************************************************

    '' ''                        pubblicazione.Documenti = pubblicazioni.GetDocumenti(pubblicazione.Id)
    '' ''                        Dim idAlbo = pubblicazione.Id
    '' ''                        Dim documentoPrimario = documenti.Where(Function(c) c.IdAlbo = idAlbo And c.IdTipologia = 1).FirstOrDefault
    '' ''                        If Not documentoPrimario Is Nothing Then
    '' ''                            documentoPrimario.NomeFileFirmato = documentoPrimario.Nomefile & ".p7m"
    '' ''                            documenti.SaveChanges()
    '' ''                        End If
    '' ''                        '*******************************************************************************

    '' ''                        Me.Pubblica(pubblicazione)
    '' ''                    Catch ex As Exception
    '' ''                        successo = False
    '' ''                        messaggio.AppendLine("Si è verificato un errore durante la pubblicazione n." & pubblicazione.NumeroRegistro & " del " & pubblicazione.DataRegistrazione.Value.ToShortDateString & vbCrLf & "Dettaglio errore: " & ex.Message)
    '' ''                    End Try

    '' ''                    If successo Then
    '' ''                        Dim idDestinatario = Me.ElencoDestinatariDictionary.Where(Function(c) c.Key = idTask).FirstOrDefault.Value
    '' ''                        Me.WriteTask(idDestinatario, operazione, notificato)
    '' ''                    End If

    '' ''                End If
    '' ''            Next




    '' ''            'For Each taskCorrente In Me.TaskAttivi
    '' ''            '    successo = True
    '' ''            '    Me.TaskAttivo = taskCorrente
    '' ''            '    Dim idDestinatario As Integer = Me.GetIdDestinatario()

    '' ''            '    '*******************************************************************************
    '' ''            '    'AGGIORNO IL DOCUMENTO PRIMARIO CON IL NOME DEL FILE FIRMATO
    '' ''            '    '*******************************************************************************

    '' ''            '    Dim pubblicazione As ParsecMES.Pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.IdDocumento = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '' ''            '    pubblicazione.Documenti = pubblicazioni.GetDocumenti(pubblicazione.Id)
    '' ''            '    Dim idAlbo = pubblicazione.Id
    '' ''            '    Dim documentoPrimario = documenti.Where(Function(c) c.IdAlbo = idAlbo And c.IdTipologia = 1).FirstOrDefault
    '' ''            '    If Not documentoPrimario Is Nothing Then
    '' ''            '        documentoPrimario.NomeFileFirmato = documentoPrimario.Nomefile & ".p7m"
    '' ''            '        documenti.SaveChanges()
    '' ''            '    End If
    '' ''            '    '*******************************************************************************

    '' ''            '    Try
    '' ''            '        Me.Pubblica(pubblicazione)
    '' ''            '    Catch ex As Exception
    '' ''            '        successo = False
    '' ''            '        messaggio.AppendLine("Si è verificato un errore durante la pubblicazione n." & pubblicazione.NumeroRegistro & " del " & pubblicazione.DataRegistrazione.Value.ToShortDateString & ex.Message)
    '' ''            '    End Try

    '' ''            '    If successo Then
    '' ''            '        Me.WriteTask(idDestinatario, operazione, notificato)
    '' ''            '    End If


    '' ''            'Next


    '' ''            documenti.Dispose()
    '' ''            pubblicazioni.Dispose()

    '' ''    End Select



    '' ''    If messaggio.Length > 0 Then
    '' ''        ParsecUtility.Utility.MessageBox(messaggio.ToString, False)
    '' ''    End If

    '' ''    Me.FirstClick = False

    '' ''    'NON FUNZIONA SU CHROME
    '' ''    ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

    '' ''    Me.FileToSignDictionary = Nothing
    '' ''    Me.ElencoDestinatariDictionary = Nothing
    '' ''End Sub

    '*********************************************************************************************************
    'QUESTO PULSANTE VIENE PRESSATO DAL COMPONENTE PARSECDIGITALSIGN PER AVVIARE LA PUBBLICAZIONE MASSIVA
    '*********************************************************************************************************
    ' luca 02/07/2020
    '' ''Protected Sub PubblicaOnlineMassivaButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles PubblicaOnlineMassivaButton.Click
    '' ''    Me.PubblicaOnlineMassivaCallback()
    '' ''End Sub

    '*********************************************************************************************************
    'QUESTO PULSANTE VIENE PRESSATO DAL COMPONENTE PARSECOPENOFFICE PER AVVIARE IL PROCESSO DI FIRMA MASSIVA
    '*********************************************************************************************************
    Protected Sub firmaMassivaDocumentiButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles firmaMassivaDocumentiButton.Click
        Me.EseguiFirmaMassivaDocumenti()
    End Sub

    Private Sub EseguiFirmaMassivaDocumenti()
        'FIRMA DEBOLE MASSIVA DA IMPLEMENTARE

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim abilitaFirmaElettronica As ParsecWKF.ParametroProcesso = Me.Action.GetParameterByName("AbilitaFirmaElettronica")

        If Not abilitaFirmaElettronica Is Nothing Then
            If Not String.IsNullOrEmpty(utenteCollegato.NomefileCertificato) Then
                If Not Me.FileToSignDictionary Is Nothing Then
                    Me.FirmaDeboleMassivaDocumenti(Me.FileToSignDictionary)
                    If Me.Action.Type = "FIRMA" Then
                        ' Me.FileToSignList = Nothing
                    End If
                    Exit Sub
                End If
            Else
                'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
                Me.WriteAllTasksAndUpdateParent(Me.ElencoDestinatariDictionary)
                Me.ElencoDestinatariDictionary = Nothing
                Me.SbloccaTasks(utenteCollegato.Id)
            End If
        End If


        Dim firmaDigitalmente As Boolean = Me.CheckAbilitazioneFirmaDigitale(utenteCollegato)

        If firmaDigitalmente Then
            If Not Me.FileToSignDictionary Is Nothing Then
                Me.FirmaMassivaDocumenti(Me.FileToSignDictionary)
                If Me.Action.Type = "FIRMA" Then
                    'Me.FileToSignList = Nothing
                End If
            End If
        Else

            'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
            Me.WriteAllTasksAndUpdateParent(Me.ElencoDestinatariDictionary)
            Me.ElencoDestinatariDictionary = Nothing
            Me.SbloccaTasks(utenteCollegato.Id)
        End If
    End Sub


    Private Sub FirmaDeboleMassivaDocumenti(ByVal fileToSignList As Dictionary(Of Integer, InfoFirma))

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato
        Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
        Dim datiSrc As New ParsecAdmin.DatiFirmaSrc
        Dim signParameters As New ParsecAdmin.SignParameters

        Dim remotePathCertificate = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeCertificati"), utenteCorrente.NomefileCertificato)
        For Each item In fileToSignList
            Dim infoFirma As InfoFirma = item.Value
            Dim annoEsercizio As String = rgx.Match(infoFirma.NomeFile).Value
            Dim remotePathToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, infoFirma.NomeFile)
            datiSrc.RowInfos.Add(item.Key, remotePathToSign)
        Next

        Dim datiInput As New DatiFirma
        datiInput.RemotePathToSign = ""
        datiInput.RemotePathCertificate = remotePathCertificate
        datiInput.FunctionType = FunctionType.MultiWeakSignDocument
        datiInput.CertificateId = certificateId
        datiInput.Provider = ProviderType.CryptoApi

        Dim data As String = signParameters.CreaDataSource(datiInput, datiSrc)

        Me.RegistraScriptPersistenzaVisibilitaPannello()

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaIterButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.AggiornaIter)
        End If





    End Sub

    Private Sub FirmaMassivaDocumenti(ByVal fileToSignList As Dictionary(Of Integer, InfoFirma))

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato

        Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")

        Dim signParameters As New ParsecAdmin.SignParameters
        Dim datiSrc As New ParsecAdmin.DatiFirmaSrc

        Dim cofirma = Me.Action.GetParameterByName("Cofirma")

        For Each item In fileToSignList

            Dim infoFirma As InfoFirma = item.Value

            Dim annoEsercizio As String = rgx.Match(infoFirma.NomeFile).Value

            Dim p7m As String = IO.Path.GetFileNameWithoutExtension(infoFirma.NomeFile) & ".pdf.p7m"

            Dim pathFileToSign As String = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, infoFirma.NomeFile)
            If Not cofirma Is Nothing Then
                pathFileToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, p7m)
            End If

            datiSrc.RowInfos.Add(item.Key, pathFileToSign)
        Next

        Dim datiInput As New DatiFirma
        datiInput.RemotePathToSign = ""
        datiInput.RemotePathCertificate = ""
        datiInput.FunctionType = FunctionType.MultiSignDocument
        If Not cofirma Is Nothing Then
            datiInput.FunctionType = FunctionType.MultiCosignDocument
        End If

        datiInput.CertificateId = certificateId

        '********************************************************************************************************
        'FIRMA MASSIVA FORTE SOLO PER I PROVIDER PkNet E Namirial 
        '********************************************************************************************************
        Dim provider As ParsecAdmin.ProviderType = CType(utenteCorrente.IdProviderFirma, ParsecAdmin.ProviderType)
        datiInput.Provider = provider
        If provider = ProviderType.CryptoApi Then
            datiInput.Provider = ProviderType.PkNet
        End If

        Dim data As String = signParameters.CreaDataSource(datiInput, datiSrc)

        Me.RegistraScriptPersistenzaVisibilitaPannello()


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaIterButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.AggiornaIter)
        End If




    End Sub

#End Region

#Region "GESTIONE NUMERAZIONE"


    Private Sub Numera(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)


        If Me.Action.Parameters.Count = 0 Then
            Me.VisualizzaAttoAmministrativoInModifica()
            Exit Sub
        End If

        Dim assegnaNumeroSettore As Boolean = Not Me.Action.GetParameterByName("NumerazioneSettore") Is Nothing

        Dim salvaImpegniDefinitiviDedaGroup = Not Me.Action.GetParameterByName("SalvaImpegniDefinitiviDedaGroup") Is Nothing

        '**************************************************************
        'DEPRECATA
        '**************************************************************
        'Dim pubblicaLiquidazione As Boolean = Not Me.Action.GetParameterByName("PubblicaLiquidazione") Is Nothing
        '**************************************************************

        Dim pubblicaTrasparenza As Boolean = Not Me.Action.GetParameterByName("PubblicaTrasparenza") Is Nothing

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault

        Dim propostaNumerata = documenti.Where(Function(c) c.IdPadre = documento.Id).Any
        If propostaNumerata Then
            Throw New ApplicationException("La proposta è stata già numerata!")
        End If


        If assegnaNumeroSettore Then

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametroMomentoNumerazioneSettore As ParsecAdmin.Parametri = parametri.GetByName("MomentoNumerazioneSettore", ParsecAdmin.TipoModulo.ATT)
            If Not parametroMomentoNumerazioneSettore Is Nothing Then
                If parametroMomentoNumerazioneSettore.Valore <> "3" Then
                    Throw New ApplicationException("Il parametro di sistema 'MomentoNumerazioneSettore' non è configurato correttamente." & vbCrLf & "Contattare gli amministratori del sistema.")
                End If
            End If
            parametri.Dispose()
        End If


        If Not Me.Action.GetParameterByName("CreaSeduta") Is Nothing Then
            documento.Modello = documenti.GetModello(documento.IdModello)
            Dim idSeduta As Integer = Me.CreaSeduta(documento)
            documento.IdSeduta = idSeduta
            documenti.SaveChanges()
        End If

        Try

            documento = documenti.GetFullById(Me.TaskAttivo.IdDocumento)

            If Not documento Is Nothing Then

                Dim nuovoDocumento As ParsecAtt.Documento = Me.AggiornaModello(documento)

                nuovoDocumento = documenti.LoadAllegatiPubblicazione(nuovoDocumento)

                nuovoDocumento.SalvaImpegniDefinitiviDedaGroup = salvaImpegniDefinitiviDedaGroup

                '**************************************************************************************************************************
                'DURANTE LA NUMERAZIONE IL PARAMETRO ModificheDuranteTrasformaProposta DETERMINA SE VISUALIZZARE IL DOCUMENTO ODT
                'nuovoDocumento.NascondiDocumento = True 
                '**************************************************************************************************************************

                If assegnaNumeroSettore Then
                    nuovoDocumento.AssegnaNumeroSettore = True
                End If


                documenti.Documento = Nothing
                nuovoDocumento.CodiceProposta = documento.Codice
                documenti.TipologiaProcedura = ParsecAtt.TipoProcedura.Numerazione 'NUMERAZIONE

                documenti.Save(nuovoDocumento)

                If Not String.IsNullOrEmpty(documenti.Documento.MessaggioAvvisoDedaGroup) Then
                    ParsecUtility.Utility.MessageBox(documenti.Documento.MessaggioAvvisoDedaGroup, False)
                End If

                Dim idDocumento As Integer = nuovoDocumento.Id

                '************************************************
                'VERIFICO SE IL DOCUMENTO E' PUBBLICABILE
                '************************************************
                Dim pubblicabile As Boolean = False
                If nuovoDocumento.Modello.Pubblicazione.HasValue Then
                    If nuovoDocumento.Modello.Pubblicazione Then
                        pubblicabile = True
                    End If
                End If

                If Not pubblicabile Then
                    'AGGIUNGO IL GRUPPO DI VISIBILITA' TUTTI GLI UTENTI SE NON E' RISERVATO
                    If Not nuovoDocumento.Riservato Then
                        Me.AggiungiGruppoVisibilitaTuttiUtenti(nuovoDocumento.Id)
                    End If

                End If
                '************************************************

                '**************************************************************
                'DEPRECATA.
                '*********************************************************************************************************
                'SE STO NUMERANDO UNA DETERMINA
                '*********************************************************************************************************
                'If pubblicaLiquidazione Then
                '    If nuovoDocumento.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Then
                '        If nuovoDocumento.Modello.Liquidazione Then

                '            'nuovoDocumento = documenti.GetById(nuovoDocumento.Id)
                '            nuovoDocumento.Liquidazioni = documenti.GetLiquidazioni(nuovoDocumento.Id)

                '            Me.PubblicaLiquidazione(nuovoDocumento)
                '        End If
                '    End If
                'End If
                '**************************************************************

                '*********************************************************************************************************
                'SE STO NUMERANDO UNA DETERMINA
                '*********************************************************************************************************
                'Da testare.

                Dim messaggioAvviso As String = String.Empty
                'luca 01/07/2020
                '' ''Try
                '' ''    If pubblicaTrasparenza Then
                '' ''        If Not nuovoDocumento Is Nothing Then
                '' ''            nuovoDocumento.Trasparenza = documenti.GetTrasparenza(nuovoDocumento.Id)
                '' ''            If Not nuovoDocumento.Trasparenza Is Nothing Then
                '' ''                Me.PubblicaTrasparenza(nuovoDocumento)
                '' ''            End If
                '' ''        End If
                '' ''    End If
                '' ''Catch ex As Exception
                '' ''    messaggioAvviso = ex.Message
                '' ''End Try

                If Not String.IsNullOrEmpty(messaggioAvviso) Then
                    ParsecUtility.Utility.MessageBox(messaggioAvviso, False)
                End If


                Me.AggiornaIstanzaWorkflow(nuovoDocumento)

                If Not Me.OperazioneMassiva Then
                    Me.RegistraScriptOpenOffice(nuovoDocumento, Me.numeraDocumentoButton.ClientID)
                Else
                    'MEMORIZZO I DATASOURCE
                    If Me.DataSources Is Nothing Then
                        Me.DataSources = New List(Of String)
                    End If
                    Me.DataSources.Add(nuovoDocumento.DataSource)
                End If



                Me.Nomefile = nuovoDocumento.Nomefile

            End If
            documenti.Dispose()

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try

    End Sub

    '*********************************************************************************************************
    'QUESTO PULSANTE VIENE PRESSATO DAL COMPONENTE PARSECOPENOFFICE PER AGGIORNARE IL TASK DEL WORKFLOW
    '*********************************************************************************************************
    Protected Sub numeraDocumentoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles numeraDocumentoButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim idDestinatario As Integer = Me.GetIdDestinatario()
        'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
        Me.WriteTaskAndUpdateParent(idDestinatario)
        Me.SbloccaTasks(utenteCollegato.Id)
    End Sub

#End Region

#Region "GESTIONE NUMERAZIONE MASSIVA"

    Private Sub NumerazioneMassiva(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        For Each taskCorrente In Me.TaskAttivi
            Me.TaskAttivo = taskCorrente

            Dim idDestinatario As Integer = Me.GetIdDestinatario()
            Me.ElencoDestinatariDictionary.Add(taskCorrente.Id, idDestinatario)

            'NUMERO E MEMORIZZO I DATASOURCE
            Me.Numera(e)

        Next
        'ELABORO TUTTI I DOCUMENTI (LA CONCLUSIONE DELL'ELABORAZIONE AVVIA L'AGGIORNAMENTO DI TUTTI I TASK SELEZIONATI)
        Me.RegistraScriptPersistenzaVisibilitaPannello()



        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraScriptElaborazioneMassiva(Me.DataSources, Me.aggiornaTaskMassiviButton.ClientID, True, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(Me.DataSources, True, AddressOf Me.AggiornaTaskMassiviCallback)
        End If



        Me.DataSources = Nothing
    End Sub

    Private Sub AggiornaTaskMassiviCallback()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.WriteAllTasksAndUpdateParent(Me.ElencoDestinatariDictionary)
        Me.ElencoDestinatariDictionary = Nothing
        Me.SbloccaTasks(utenteCollegato.Id)
    End Sub

    '*********************************************************************************************************
    'QUESTO PULSANTE VIENE PRESSATO DAL COMPONENTE PARSECOPENOFFICE PER AGGIORNARE I TASK DEL WORKFLOW
    '*********************************************************************************************************
    Protected Sub aggiornaTaskMassiviButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles aggiornaTaskMassiviButton.Click
        Me.AggiornaTaskMassiviCallback()
    End Sub

#End Region

#Region "GESTIONE INVIA AVANTI"

    Private Sub ProcediMassiva(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs, ByVal direction As Direction)
        Dim idDestinatario As String = String.Empty
        'Se questa funzione viene chiamata dal datalist
        If Not e Is Nothing Then
            Dim btn As RadButton = e.Item.FindControl("ExecuteTaskButton")

            idDestinatario = btn.Attributes("UserId")
            Dim elenco As List(Of String) = idDestinatario.Split(";").ToList
            If elenco.Count = 1 Then
                Me.WriteAllTasksAndUpdateParent(CInt(elenco(0)))
            Else
                Me.WriteAllTasksAndUpdateParent(elenco)
            End If
            'Se questa funzione viene chiamata dal item del menu di contesto
        Else
            idDestinatario = Me.RuoloId
            Me.WriteAllTasksAndUpdateParent(idDestinatario)
        End If

        Me.RuoloId = Nothing
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.SbloccaTasks(utenteCollegato.Id)
    End Sub

    Private Sub Procedi(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs, ByVal direction As Direction)

        Select Case Me.TaskAttivo.TaskCorrente

            Case "PREPARA DOCUMENTO"

                Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = TaskAttivo.IdIstanza AndAlso c.Stato Is Nothing).FirstOrDefault
                If documentoGenerico Is Nothing Then
                    ParsecUtility.Utility.MessageBox("Per inviarlo alla protocollazione è necessario generare il documento!", False)
                    Me.EnableUiHidden.Value = "Abilita"
                    Exit Sub
                End If
                documentiGenerici.Dispose()

        End Select

        Dim operazione As String = Me.Action.Description.ToUpper
        Dim idDestinatario As Integer
        Dim attore As String = String.Empty
        'Se questa funzione viene chiamata dal datalist
        If Not e Is Nothing Then
            Dim btn As RadButton = e.Item.FindControl("ExecuteTaskButton")
            idDestinatario = btn.Attributes("UserId")
            'Se questa funzione viene chiamata dal item del menu di contesto
        Else
            idDestinatario = Me.RuoloId
            Me.RuoloId = Nothing

        End If


        If idDestinatario <> 0 Then
            'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
            Me.WriteTaskAndUpdateParent(idDestinatario, operazione)
        Else
            Me.RegistraScriptPersistenzaVisibilitaPannello()

            ParsecUtility.Utility.MessageBox("Per il Ruolo " & Me.Action.ToActor & " il destinatario non è configurato!", False)
            ' Me.EnableUiHidden.Value = "Abilita"
            ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.SbloccaTasks(utenteCollegato.Id)



    End Sub


    Private Sub Procedi(ByVal direction As Direction)

        Dim operazione As String = Me.Action.Description.ToUpper
        Dim idDestinatario As Integer
        Dim attore As String = String.Empty
        idDestinatario = Me.RuoloId
        Me.RuoloId = Nothing

        If idDestinatario <> 0 Then
            'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
            Me.WriteTaskAndUpdateParent(idDestinatario, operazione)
        Else
            Me.RegistraScriptPersistenzaVisibilitaPannello()

            ParsecUtility.Utility.MessageBox("Per il Ruolo " & Me.Action.ToActor & " il destinatario non è configurato!", False)
            ' Me.EnableUiHidden.Value = "Abilita"
            ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.SbloccaTasks(utenteCollegato.Id)

    End Sub

#End Region

#Region "GESTIONE FINE"

    Private Sub Fine(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.CSRA Then
            Dim idAtto As Integer = Me.TaskAttivo.IdDocumento
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim documento = documenti.Where(Function(c) c.Id = idAtto).FirstOrDefault
            documenti.Dispose()
            If Not documento Is Nothing Then
                Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
                Dim scheda = schede.Where(Function(c) c.NumeroAtto = documento.ContatoreGenerale And c.DataAtto = documento.Data).FirstOrDefault
                schede.Dispose()

                'IL CONTROLLO NON E' STATO ANCORA ESEGUITO
                If Not scheda.DataControllo.HasValue Then
                    'Me.RegistraScriptPersistenzaVisibilitaPannello()
                    Me.EnableUiHidden.Value = "Abilita"
                    ParsecUtility.Utility.MessageBox("E' necessario valutare la scheda!!", False)
                    Me.SbloccaTasks(utenteCollegato.Id)
                    Exit Sub
                End If
            End If
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
            Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
            Dim segnalazione = segnalazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
            Dim stato As String = "RIFIUTATA"
            If Not segnalazione Is Nothing Then
                If segnalazione.Stato.ToUpper <> "CONCLUSA" AndAlso segnalazione.Stato.ToUpper <> stato Then
                    segnalazione.Stato = stato
                    segnalazioni.SaveChanges()

                    Try
                        Me.AggiornaStatoSegnalazioneOnline(utenteCollegato.Id, segnalazione.GuidSegnalazione, stato)
                    Catch ex As Exception

                    End Try

                End If
            End If
            segnalazioni.Dispose()
        End If

        Dim operazione As String = Me.Action.Description.ToUpper
        Dim idDestinatario As Integer = Me.GetIdDestinatario
        'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
        Me.WriteTaskAndUpdateParent(idDestinatario, operazione)


        Me.SbloccaTasks(utenteCollegato.Id)

    End Sub

    Private Sub FineMassiva(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        Dim elencoDestinatari As New Dictionary(Of Integer, Integer)

        For Each taskCorrente In Me.TaskAttivi
            Me.TaskAttivo = taskCorrente
            Dim idDestinatario As Integer = Me.GetIdDestinatario
            elencoDestinatari.Add(taskCorrente.Id, idDestinatario)
        Next

        'Chiudo la pagina e aggiorno la griglia della pagina chiamante.
        Me.WriteAllTasksAndUpdateParent(elencoDestinatari)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.SbloccaTasks(utenteCollegato.Id)
    End Sub

#End Region

#Region "GESTIONE MODIFICA"

    Private Function GetVersione(ByVal nomeFile As String) As String
        Dim dot As Integer = nomeFile.IndexOf(".")
        Dim v As Integer = nomeFile.LastIndexOf("_v") + 2
        Dim versione = nomeFile.Substring(v, dot - v)
        Return versione
    End Function

    Private Function GetAnnoEsercizio(documento As ParsecAtt.Documento) As Integer
        Dim annoEsercizio As Integer = Now.Year
        Try
            Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
            annoEsercizio = CInt(rgx.Match(documento.Nomefile).Value)
        Catch ex As Exception

        End Try

        Return annoEsercizio
    End Function

    Private Function GetNomeFileFirmato(ByVal documento As ParsecAtt.Documento) As String

        Dim nomefileFirmato As String = IO.Path.GetFileNameWithoutExtension(documento.Nomefile) & ".pdf.p7m"
        Dim annoEsercizio As Integer = Me.GetAnnoEsercizio(documento)
        Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefileFirmato)

        If Not IO.File.Exists(localPath) Then

            Dim tipoDocumento = documento.TipologiaDocumento
            Dim proposta As Boolean = tipoDocumento = ParsecAtt.TipoDocumento.PropostaDetermina OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaDelibera OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaOrdinanza OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaDecreto

            Dim prefissoProposta As String = "Prop"

            Dim versione As Integer = 0
            Dim v As Integer = 0

            For Each f In documento.Firme
                If Not String.IsNullOrEmpty(f.FileFirmato) Then
                    If Not proposta Then
                        'ESCLUDO LE PROPOSTE FIRMATE SE IL DOCUMENTO E' UN ATTO DEFINITIVO
                        If Not f.FileFirmato.StartsWith(prefissoProposta) Then
                            v = GetVersione(f.FileFirmato)
                            If v > versione Then
                                versione = v
                            End If
                        End If

                    End If

                End If
            Next

            

            Dim token = nomefileFirmato.Split("_")
            token(3) = "v" & versione.ToString & ".pdf.p7m"

            nomefileFirmato = String.Join("_", token)
            localPath = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefileFirmato)
            If IO.File.Exists(localPath) Then
                Return localPath
            End If
        Else
            Return localPath
        End If
        Return Nothing
    End Function

    'Private Function ConservaDocumentiFirmatiProposta(ByVal proposta As ParsecAtt.Documento, ByVal idDocumentoConservatoPadre As Integer, ByVal codiceLicenza As String, ByRef documentiConservati As List(Of Integer), ByVal algoritmoEnteConservatoreAttivo As String) As String

    '    Dim firmeProposta = proposta.Firme.Where(Function(c) Not String.IsNullOrEmpty(c.FileFirmato)).GroupBy(Function(c) c.FileFirmato).Select(Function(c) c.FirstOrDefault).ToList
    '    Dim messaggioErrore As String = String.Empty
    '    Dim annoEsercizio As Integer = Me.GetAnnoEsercizio(proposta)
    '    Dim localPath As String = String.Empty
    '    Dim pathAtti As String = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")

    '    Dim request As wsConservazione.SoapConservaAttoRequest = Nothing
    '    Dim metaDato As wsConservazione.MetaDatoAtto = Nothing
    '    Dim wsConservazioneSoap As New wsConservazione.wsConservazione
    '    Dim documentoConservato As wsConservazione.SoapDocumentoBaseResponse = Nothing


    '    'CONSERVO I FILE P7M ASSOCIATI ALLE FIRME DELLA PROPOSTA DI DELIBERA
    '    If firmeProposta.Count > 0 Then
    '        Try
    '            For Each firmaProposta In firmeProposta

    '                localPath = String.Format("{0}{1}\{2}", pathAtti, annoEsercizio, firmaProposta.FileFirmato)

    '                If Not IO.File.Exists(localPath) Then
    '                    messaggioErrore = "File '" & localPath.Replace("\", "/") & "' non trovato!"
    '                    Exit For
    '                Else

    '                    metaDato = New wsConservazione.MetaDatoAtto
    '                    metaDato.DataChiusura = Now
    '                    metaDato.FileName = firmaProposta.FileFirmato
    '                    metaDato.IdDocumentoConservatoPadre = idDocumentoConservatoPadre

    '                    If proposta.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDelibera Then
    '                        metaDato.TipoDocumento = "DELIBERA"
    '                    Else
    '                        metaDato.TipoDocumento = "DETERMINA"
    '                    End If

    '                    metaDato.idDocumentoSep = proposta.Id
    '                    metaDato.idModuloSep = ParsecAdmin.TipoModulo.ATT
    '                    metaDato.Informazioni = proposta.Oggetto
    '                    metaDato.Oggetto = proposta.Oggetto

    '                    Select Case algoritmoEnteConservatoreAttivo
    '                        Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1
    '                            metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash(localPath)
    '                        Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256
    '                            metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash256FromFile(localPath)
    '                    End Select

    '                    metaDato.AlgoritmoImpronta = algoritmoEnteConservatoreAttivo


    '                    request = New wsConservazione.SoapConservaAttoRequest
    '                    request.codiceLicenza = codiceLicenza
    '                    request.file = Nothing 'IO.File.ReadAllBytes(localPath)
    '                    request.sourcePath = localPath
    '                    request.metaDatoAtto = metaDato

    '                    documentoConservato = wsConservazioneSoap.conservaAttoAllegato(request)

    '                    If String.IsNullOrEmpty(documentoConservato.messaggioErrore) Then
    '                        documentiConservati.Add(documentoConservato.ListaDocumentiBase(0).idDocumento)
    '                    Else
    '                        messaggioErrore = documentoConservato.messaggioErrore
    '                        Exit For
    '                    End If

    '                End If
    '            Next
    '        Catch ex As Exception
    '            messaggioErrore = ex.Message
    '        End Try

    '    Else
    '        'CONSERVO IL FILE ODT DELLA PROPOSTA DI DELIBERA
    '    End If

    '    Return messaggioErrore
    'End Function


    '' ''Private Function ConservaDocumentiFirmatiProposta(ByVal proposta As ParsecAtt.Documento, ByVal idDocumentoConservatoPadre As Integer, ByVal codiceLicenza As String, ByRef documentiConservati As List(Of Integer), ByVal algoritmoEnteConservatoreAttivo As String) As String

    '' ''    Dim firmeProposta = proposta.Firme.Where(Function(c) Not String.IsNullOrEmpty(c.FileFirmato)).GroupBy(Function(c) c.FileFirmato).Select(Function(c) c.FirstOrDefault).ToList
    '' ''    Dim messaggioErrore As String = String.Empty
    '' ''    Dim annoEsercizio As Integer = Me.GetAnnoEsercizio(proposta)
    '' ''    Dim localPath As String = String.Empty
    '' ''    Dim pathAtti As String = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")

    '' ''    Dim request As wsConservazione.SoapConservaAttoRequest = Nothing
    '' ''    Dim metaDato As wsConservazione.MetaDatoAtto = Nothing
    '' ''    Dim wsConservazioneSoap As New wsConservazione.wsConservazione
    '' ''    Dim documentoConservato As wsConservazione.SoapDocumentoBaseResponse = Nothing


    '' ''    'CONSERVO I FILE P7M ASSOCIATI ALLE FIRME DELLA PROPOSTA DI DELIBERA
    '' ''    If firmeProposta.Count > 0 Then
    '' ''        Try
    '' ''            For Each firmaProposta In firmeProposta

    '' ''                localPath = String.Format("{0}{1}\{2}", pathAtti, annoEsercizio, firmaProposta.FileFirmato)

    '' ''                If Not IO.File.Exists(localPath) Then
    '' ''                    messaggioErrore = "File '" & localPath.Replace("\", "/") & "' non trovato!"
    '' ''                    Exit For
    '' ''                Else

    '' ''                    metaDato = New wsConservazione.MetaDatoAtto
    '' ''                    metaDato.DataChiusura = Now
    '' ''                    metaDato.FileName = firmaProposta.FileFirmato
    '' ''                    metaDato.IdDocumentoConservatoPadre = idDocumentoConservatoPadre

    '' ''                    Select Case proposta.TipologiaDocumento
    '' ''                        Case ParsecAtt.TipoDocumento.PropostaDelibera
    '' ''                            metaDato.TipoDocumento = "DELIBERA"
    '' ''                        Case ParsecAtt.TipoDocumento.PropostaDetermina
    '' ''                            metaDato.TipoDocumento = "DETERMINA"
    '' ''                        Case ParsecAtt.TipoDocumento.PropostaDecreto
    '' ''                            metaDato.TipoDocumento = "DECRETO"
    '' ''                        Case ParsecAtt.TipoDocumento.PropostaOrdinanza
    '' ''                            metaDato.TipoDocumento = "ORDINANZA"
    '' ''                    End Select



    '' ''                    metaDato.idDocumentoSep = proposta.Id
    '' ''                    metaDato.idModuloSep = ParsecAdmin.TipoModulo.ATT
    '' ''                    metaDato.Informazioni = proposta.Oggetto
    '' ''                    metaDato.Oggetto = proposta.Oggetto

    '' ''                    Select Case algoritmoEnteConservatoreAttivo
    '' ''                        Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1
    '' ''                            metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash(localPath)
    '' ''                        Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256
    '' ''                            metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash256FromFile(localPath)
    '' ''                    End Select

    '' ''                    metaDato.AlgoritmoImpronta = algoritmoEnteConservatoreAttivo


    '' ''                    request = New wsConservazione.SoapConservaAttoRequest
    '' ''                    request.codiceLicenza = codiceLicenza
    '' ''                    request.file = Nothing 'IO.File.ReadAllBytes(localPath)
    '' ''                    request.sourcePath = localPath
    '' ''                    request.metaDatoAtto = metaDato

    '' ''                    documentoConservato = wsConservazioneSoap.conservaAttoAllegato(request)

    '' ''                    If String.IsNullOrEmpty(documentoConservato.messaggioErrore) Then
    '' ''                        documentiConservati.Add(documentoConservato.ListaDocumentiBase(0).idDocumento)
    '' ''                    Else
    '' ''                        messaggioErrore = documentoConservato.messaggioErrore
    '' ''                        Exit For
    '' ''                    End If

    '' ''                End If
    '' ''            Next
    '' ''        Catch ex As Exception
    '' ''            messaggioErrore = ex.Message
    '' ''        End Try

    '' ''    Else
    '' ''        'CONSERVO IL FILE ODT DELLA PROPOSTA DI DELIBERA
    '' ''    End If

    '' ''    Return messaggioErrore
    '' ''End Function



    '' ''Private Function ConservaRelataPubblicazioneFirmata(ByVal documento As ParsecAtt.Documento, ByVal idDocumentoConservatoPadre As Integer, ByVal codiceLicenza As String, ByRef documentiConservati As List(Of Integer), ByVal algoritmoEnteConservatoreAttivo As String) As String
    '' ''    Dim messaggioErrore As String = String.Empty

    '' ''    Dim pathDownload As String = String.Empty
    '' ''    Dim request As wsConservazione.SoapConservaAttoRequest = Nothing
    '' ''    Dim metaDato As wsConservazione.MetaDatoAtto = Nothing
    '' ''    Dim documentoConservato As wsConservazione.SoapDocumentoBaseResponse = Nothing
    '' ''    Dim wsConservazioneSoap As New wsConservazione.wsConservazione
    '' ''    Dim relataPubblicazioneFirmata As String = String.Empty

    '' ''    'CONSERVO IL FILE P7M ASSOCIATO ALLA REALATA DI PUBBLICAZIONE
    '' ''    Try

    '' ''        Dim pubblicazioni As New ParsecMES.AlboRepository
    '' ''        Dim pubblicazione = pubblicazioni.Where(Function(c) c.IdDocumento = documento.Id And c.IdModulo = ParsecAdmin.TipoModulo.ATT).FirstOrDefault
    '' ''        If Not pubblicazione Is Nothing Then
    '' ''            Dim documenti As New ParsecMES.DocumentiRepository
    '' ''            Dim idPubblicazione As Integer = pubblicazione.Id
    '' ''            ' Dim documentoPrimario = documenti.Where(Function(c) c.IdAlbo = idPubblicazione And c.IdTipologia = 1).Select(Function(c) c.NomeFileFirmato)
    '' ''            Dim documentoPrimario = documenti.Where(Function(c) c.IdAlbo = idPubblicazione And c.IdTipologia = 1).Select(Function(c) c.Nomefile)
    '' ''            If documentoPrimario.Any Then
    '' ''                Dim anno As String = pubblicazione.DataRegistrazione.Value.Year.ToString
    '' ''                Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")
    '' ''                relataPubblicazioneFirmata = documentoPrimario.FirstOrDefault & ".p7m"
    '' ''                pathDownload = localPath & anno & "\" & relataPubblicazioneFirmata
    '' ''                documenti.Dispose()
    '' ''            Else
    '' ''                Return "La pubblicazione non ha nessun documento primario!"
    '' ''            End If
    '' ''        Else
    '' ''            Return "Pubblicazione non trovata!"
    '' ''        End If
    '' ''        pubblicazioni.Dispose()

    '' ''        'If String.IsNullOrEmpty(pathDownload) Then
    '' ''        '    messaggioErrore = "Relata di pubblicazione non trovata"
    '' ''        '    Return messaggioErrore
    '' ''        'End If

    '' ''        If Not IO.File.Exists(pathDownload) Then
    '' ''            messaggioErrore = "File '" & pathDownload.Replace("\", "/") & "' non trovato!"
    '' ''            Return messaggioErrore
    '' ''        End If


    '' ''        metaDato = New wsConservazione.MetaDatoAtto
    '' ''        metaDato.DataChiusura = Now
    '' ''        metaDato.FileName = relataPubblicazioneFirmata
    '' ''        metaDato.IdDocumentoConservatoPadre = idDocumentoConservatoPadre
    '' ''        metaDato.TipoDocumento = documento.ToString.ToUpper
    '' ''        metaDato.idDocumentoSep = documento.Id
    '' ''        metaDato.idModuloSep = ParsecAdmin.TipoModulo.ATT
    '' ''        metaDato.Informazioni = documento.Oggetto
    '' ''        metaDato.Oggetto = documento.Oggetto

    '' ''        Select Case algoritmoEnteConservatoreAttivo
    '' ''            Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1
    '' ''                metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash(pathDownload)
    '' ''            Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256
    '' ''                metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash256FromFile(pathDownload)
    '' ''        End Select

    '' ''        metaDato.AlgoritmoImpronta = algoritmoEnteConservatoreAttivo


    '' ''        request = New wsConservazione.SoapConservaAttoRequest
    '' ''        request.codiceLicenza = codiceLicenza
    '' ''        request.file = Nothing 'IO.File.ReadAllBytes(pathDownload)
    '' ''        request.sourcePath = pathDownload
    '' ''        request.metaDatoAtto = metaDato

    '' ''        documentoConservato = wsConservazioneSoap.conservaAttoAllegato(request)

    '' ''        If String.IsNullOrEmpty(documentoConservato.messaggioErrore) Then
    '' ''            documentiConservati.Add(documentoConservato.ListaDocumentiBase(0).idDocumento)
    '' ''        Else
    '' ''            messaggioErrore = documentoConservato.messaggioErrore
    '' ''        End If

    '' ''    Catch ex As Exception
    '' ''        messaggioErrore = ex.Message
    '' ''    End Try

    '' ''    Return messaggioErrore


    '' ''End Function

    '' ''Private Function ConservaAllegatiAttoAmministrativo(ByVal documento As ParsecAtt.Documento, ByVal idDocumentoConservatoPadre As Integer, ByVal codiceLicenza As String, ByRef documentiConservati As List(Of Integer), ByVal algoritmoEnteConservatoreAttivo As String) As String

    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
    '' ''    percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
    '' ''    Dim messaggioErrore As String = String.Empty
    '' ''    Dim pathDownload As String = String.Empty

    '' ''    Dim request As wsConservazione.SoapConservaAttoRequest = Nothing
    '' ''    Dim metaDato As wsConservazione.MetaDatoAtto = Nothing
    '' ''    Dim documentoConservato As wsConservazione.SoapDocumentoBaseResponse = Nothing
    '' ''    Dim wsConservazioneSoap As New wsConservazione.wsConservazione
    '' ''    Dim nomefile As String = String.Empty

    '' ''    'CONSERVO GLI ALLEGATI ASSOCIATI ALL'ATTO AMMINISTRATIVO
    '' ''    Try
    '' ''        For Each allegato In documento.Allegati
    '' ''            Try

    '' ''                If Not String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
    '' ''                    nomefile = allegato.NomeFileFirmato
    '' ''                Else
    '' ''                    nomefile = allegato.Nomefile
    '' ''                End If


    '' ''                pathDownload = percorsoRoot & allegato.PercorsoRelativo & nomefile
    '' ''                If Not IO.File.Exists(pathDownload) Then
    '' ''                    messaggioErrore = "File '" & pathDownload.Replace("\", "/") & "' non trovato!"
    '' ''                    Exit For
    '' ''                End If

    '' ''                metaDato = New wsConservazione.MetaDatoAtto
    '' ''                metaDato.DataChiusura = Now
    '' ''                metaDato.FileName = nomefile 'allegato.Nomefile
    '' ''                metaDato.IdDocumentoConservatoPadre = idDocumentoConservatoPadre
    '' ''                metaDato.TipoDocumento = documento.ToString.ToUpper
    '' ''                metaDato.idDocumentoSep = allegato.Id
    '' ''                metaDato.idModuloSep = ParsecAdmin.TipoModulo.ATT
    '' ''                metaDato.Informazioni = allegato.Oggetto
    '' ''                metaDato.Oggetto = allegato.Oggetto


    '' ''                Select Case algoritmoEnteConservatoreAttivo
    '' ''                    Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1
    '' ''                        metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash(pathDownload)
    '' ''                    Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256
    '' ''                        metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash256FromFile(pathDownload)
    '' ''                End Select

    '' ''                metaDato.AlgoritmoImpronta = algoritmoEnteConservatoreAttivo



    '' ''                request = New wsConservazione.SoapConservaAttoRequest
    '' ''                request.codiceLicenza = codiceLicenza
    '' ''                request.file = Nothing 'IO.File.ReadAllBytes(pathDownload)
    '' ''                request.sourcePath = pathDownload
    '' ''                request.metaDatoAtto = metaDato

    '' ''                documentoConservato = wsConservazioneSoap.conservaAttoAllegato(request)

    '' ''                If String.IsNullOrEmpty(documentoConservato.messaggioErrore) Then
    '' ''                    documentiConservati.Add(documentoConservato.ListaDocumentiBase(0).idDocumento)
    '' ''                Else
    '' ''                    messaggioErrore = documentoConservato.messaggioErrore
    '' ''                    Exit For
    '' ''                End If
    '' ''            Catch ex As Exception
    '' ''                messaggioErrore = ex.Message
    '' ''                Exit For
    '' ''            End Try
    '' ''        Next
    '' ''    Catch ex As Exception
    '' ''        messaggioErrore = ex.Message
    '' ''    End Try

    '' ''    Return messaggioErrore
    '' ''End Function


    '' ''Private Sub CancellaDocumentiConservati(ByVal codiceLicenza As String, ByVal documentiConservati As List(Of Integer))
    '' ''    Dim cancellazioneRequest As wsConservazione.SoapCancellaDocumentoRequest = Nothing
    '' ''    Dim wsConservazioneSoap As wsConservazione.wsConservazione = Nothing

    '' ''    If documentiConservati.Count > 0 Then
    '' ''        wsConservazioneSoap = New wsConservazione.wsConservazione
    '' ''        cancellazioneRequest = New wsConservazione.SoapCancellaDocumentoRequest
    '' ''        cancellazioneRequest.codiceLicenza = codiceLicenza
    '' ''        cancellazioneRequest.documentoFiltro = New wsConservazione.DocumentoBaseConservazioneFiltro
    '' ''    End If

    '' ''    For Each documentoConservato In documentiConservati
    '' ''        Try
    '' ''            cancellazioneRequest.documentoFiltro.idDocumento = documentoConservato
    '' ''            wsConservazioneSoap.cancellaDocumento(cancellazioneRequest)
    '' ''        Catch ex As Exception
    '' ''            'NIENTE
    '' ''        End Try
    '' ''    Next
    '' ''End Sub


    '' ''Private Function ConservaAllegatiProtocollo(ByVal registrazioneProtocollo As ParsecPro.Registrazione, ByVal codiceLicenza As String, ByRef idDocumentoConservatoPadre As Integer, ByRef documentiConservati As List(Of Integer), ByVal algoritmoEnteConservatoreAttivo As String) As String

    '' ''    Dim messaggioErrore As String = String.Empty


    '' ''    Try

    '' ''        If registrazioneProtocollo Is Nothing Then
    '' ''            messaggioErrore = "Protocollo non trovato!"
    '' ''            Return messaggioErrore
    '' ''        End If

    '' ''        If registrazioneProtocollo.IdDocumentoWS.HasValue Then
    '' ''            messaggioErrore = "Protocollo già conservato!"
    '' ''            Return messaggioErrore
    '' ''        End If


    '' ''        If Not String.IsNullOrEmpty(algoritmoEnteConservatoreAttivo) Then
    '' ''            If algoritmoEnteConservatoreAttivo <> ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1 AndAlso algoritmoEnteConservatoreAttivo <> ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256 Then
    '' ''                messaggioErrore = "Algoritmo impronta non valido!"
    '' ''                Return messaggioErrore
    '' ''            End If
    '' ''        Else
    '' ''            messaggioErrore = "Algoritmo impronta non specificato!"
    '' ''            Return messaggioErrore
    '' ''        End If


    '' ''        If registrazioneProtocollo.Allegati.Count = 0 Then
    '' ''            messaggioErrore = "Il protocollo non possiede allegati!"
    '' ''            Return messaggioErrore
    '' ''        End If

    '' ''        Dim allegati = registrazioneProtocollo.Allegati

    '' ''        Dim allegatoPrimario = allegati.Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
    '' ''        'SE NON TROVO L'ALLEGATO PRIMARIO
    '' ''        If allegatoPrimario Is Nothing Then
    '' ''            'TROVO IL PRIMO ALLEGATO FIRMATO E LO IMPOSTO COME PRIMARIO
    '' ''            allegatoPrimario = allegati.Where(Function(c) Not String.IsNullOrEmpty(c.NomeFileFirmato)).FirstOrDefault
    '' ''            If allegatoPrimario Is Nothing Then
    '' ''                'TROVO IL PRIMO ALLEGATO E LO IMPOSTO COME PRIMARIO
    '' ''                allegatoPrimario = allegati.FirstOrDefault
    '' ''            End If
    '' ''        End If

    '' ''        allegatoPrimario.IdTipologiaDocumento = 1

    '' ''        allegati = allegati.OrderByDescending(Function(c) c.IdTipologiaDocumento).ToList


    '' ''        Dim wsConservazioneSoap As New wsConservazione.wsConservazione
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        Dim nomefile As String = String.Empty
    '' ''        Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
    '' ''        Dim documentoConservato As wsConservazione.SoapDocumentoBaseResponse = Nothing
    '' ''        Dim metaDatoProtocollo As wsConservazione.MetaDatoProtocollo = Nothing

    '' ''        Dim oggetto As String = registrazioneProtocollo.Oggetto.Trim
    '' ''        If String.IsNullOrEmpty(registrazioneProtocollo.Oggetto.Trim) Then
    '' ''            oggetto = "OGGETTO MANCANTE"
    '' ''        End If

    '' ''        Dim Informazioni As String = String.Empty
    '' ''        If String.IsNullOrEmpty(registrazioneProtocollo.Note.Trim) Then
    '' ''            Informazioni = "Protocollo del " & registrazioneProtocollo.NumeroProtocollo & " del " & registrazioneProtocollo.DataImmissione.Value.ToShortDateString
    '' ''        Else
    '' ''            Informazioni = registrazioneProtocollo.Note
    '' ''        End If

    '' ''        Dim destinatarioDocumentoProtocollato As String = String.Empty
    '' ''        Dim mittenteDocumentoProtocollato As String = String.Empty
    '' ''        If registrazioneProtocollo.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo Then
    '' ''            destinatarioDocumentoProtocollato = registrazioneProtocollo.ElencoReferentiInterni
    '' ''            mittenteDocumentoProtocollato = registrazioneProtocollo.ElencoReferentiEsterni
    '' ''        Else
    '' ''            destinatarioDocumentoProtocollato = registrazioneProtocollo.ElencoReferentiEsterni
    '' ''            mittenteDocumentoProtocollato = registrazioneProtocollo.ElencoReferentiInterni
    '' ''        End If

    '' ''        Dim i As Integer = 0
    '' ''        For Each allegato In allegati


    '' ''            If Not String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
    '' ''                nomefile = allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFileFirmato
    '' ''            Else
    '' ''                nomefile = allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegato.NomeFile
    '' ''            End If

    '' ''            pathDownload = percorsoRoot & allegato.PercorsoRelativo & nomefile

    '' ''            If Not IO.File.Exists(pathDownload) Then
    '' ''                messaggioErrore = "File '" & pathDownload.Replace("\", "/") & "' non trovato!"
    '' ''                Exit For
    '' ''            End If


    '' ''            metaDatoProtocollo = New wsConservazione.MetaDatoProtocollo
    '' ''            metaDatoProtocollo.FileName = nomefile
    '' ''            metaDatoProtocollo.Informazioni = Informazioni
    '' ''            metaDatoProtocollo.Oggetto = oggetto
    '' ''            metaDatoProtocollo.idDocumentoSep = registrazioneProtocollo.Id
    '' ''            metaDatoProtocollo.idModuloSep = ParsecAdmin.TipoModulo.PRO
    '' ''            metaDatoProtocollo.TipoDocumento = "DOC_PROTOCOLLO"
    '' ''            metaDatoProtocollo.DataProtocollo = registrazioneProtocollo.DataImmissione
    '' ''            metaDatoProtocollo.NumeroProtocollo = registrazioneProtocollo.NumeroProtocollo
    '' ''            metaDatoProtocollo.tipologiaDocumentoProtocollato = registrazioneProtocollo.DescrizioneTipologiaRegistristrazione.ToUpper
    '' ''            metaDatoProtocollo.destinatarioDocumentoProtocollato = destinatarioDocumentoProtocollato
    '' ''            metaDatoProtocollo.mittenteDocumentoProtocollato = mittenteDocumentoProtocollato



    '' ''            'SE STO CONSERVANDO IL DOCUMENTO PRIMARIO
    '' ''            If i = 0 Then
    '' ''                metaDatoProtocollo.IdDocumentoConservatoPadre = Nothing
    '' ''            Else
    '' ''                metaDatoProtocollo.IdDocumentoConservatoPadre = idDocumentoConservatoPadre
    '' ''            End If

    '' ''            metaDatoProtocollo.NumeroDocumento = metaDatoProtocollo.NumeroProtocollo
    '' ''            metaDatoProtocollo.DataDocumento = metaDatoProtocollo.DataProtocollo

    '' ''            metaDatoProtocollo.DataChiusura = Now


    '' ''            Select Case algoritmoEnteConservatoreAttivo
    '' ''                Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1
    '' ''                    metaDatoProtocollo.ImprontaFile = ParsecUtility.Utility.CalcolaHash(pathDownload)
    '' ''                Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256
    '' ''                    metaDatoProtocollo.ImprontaFile = ParsecUtility.Utility.CalcolaHash256FromFile(pathDownload)
    '' ''            End Select

    '' ''            metaDatoProtocollo.AlgoritmoImpronta = algoritmoEnteConservatoreAttivo

    '' ''            Dim metaDatoProtocolloSoap As New wsConservazione.SoapConservaProtocolloRequest
    '' ''            metaDatoProtocolloSoap.codiceLicenza = codiceLicenza
    '' ''            metaDatoProtocolloSoap.metaDatoProtocollo = metaDatoProtocollo
    '' ''            metaDatoProtocolloSoap.file = Nothing 'IO.File.ReadAllBytes(pathDownload)
    '' ''            metaDatoProtocolloSoap.sourcePath = pathDownload

    '' ''            documentoConservato = wsConservazioneSoap.conservaProtocollo(metaDatoProtocolloSoap)


    '' ''            If String.IsNullOrEmpty(documentoConservato.messaggioErrore) Then
    '' ''                'SE STO CONSERVANDO IL DOCUMENTO PRIMARIO
    '' ''                If i = 0 Then
    '' ''                    idDocumentoConservatoPadre = documentoConservato.ListaDocumentiBase(0).idDocumento
    '' ''                End If
    '' ''                documentiConservati.Add(documentoConservato.ListaDocumentiBase(0).idDocumento)
    '' ''            Else
    '' ''                messaggioErrore = documentoConservato.messaggioErrore
    '' ''                Exit For
    '' ''            End If

    '' ''            i += 1

    '' ''        Next

    '' ''    Catch ex As Exception
    '' ''        messaggioErrore = ex.Message

    '' ''    End Try

    '' ''    Return messaggioErrore

    '' ''End Function

    'Private Sub ConservaRegistrazioneProtocollo()

    '    Dim listaDocumentiConservati As New List(Of Integer)
    '    Dim clienti As New ParsecAdmin.ClientRepository
    '    Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '    clienti.Dispose()

    '    If cliente Is Nothing Then
    '        Throw New ApplicationException("Cliente non trovato!")
    '    End If

    '    Try

    '        Dim idDocumentoConservatoPadre As Integer = 0

    '        Dim messaggioErrore As String = String.Empty

    '        Dim wsConservazioneSoap As New wsConservazione.wsConservazione
    '        Dim getEnteSoapRequest As New wsConservazione.SoapGetEnteConservatoreRequest
    '        getEnteSoapRequest.codiceLicenza = cliente.CodLicenza
    '        Dim enteConservatoreAttivo = wsConservazioneSoap.getEnteConservatoreAttivo(getEnteSoapRequest)
    '        If Not String.IsNullOrEmpty(enteConservatoreAttivo.messaggioErrore) Then
    '            Throw New ApplicationException(enteConservatoreAttivo.messaggioErrore)
    '        End If

    '        'OTTENGO L'OGGETTO COMPLETO
    '        Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '        Dim registrazione = registrazioni.GetById(Me.TaskAttivo.IdDocumento)

    '        If registrazione.Allegati Is Nothing Then
    '            Throw New ApplicationException("E' necessario che ci sia almeno un allegato!")
    '        End If
    '        If registrazione.Allegati.Count = 0 Then
    '            Throw New ApplicationException("E' necessario che ci sia almeno un allegato!")
    '        End If


    '        Dim algoritmoImpronta As String = enteConservatoreAttivo.ListaEntiConservatori(0).algoritmoImpronta

    '        If String.IsNullOrEmpty(algoritmoImpronta) Then
    '            Throw New ApplicationException("Algoritmo impronta non specificato!")
    '        End If

    '        algoritmoImpronta = algoritmoImpronta.Trim.ToUpper

    '        '**************************************************************************************************************
    '        '1) CONSERVO GLI ALLEGATI
    '        '**************************************************************************************************************
    '        messaggioErrore = Me.ConservaAllegatiProtocollo(registrazione, cliente.CodLicenza, idDocumentoConservatoPadre, listaDocumentiConservati, algoritmoImpronta)

    '        If Not String.IsNullOrEmpty(messaggioErrore) Then
    '            '**************************************************************************************************************
    '            'ANNULLO I SALVATAGGI IN CASO DI ERRORE
    '            '**************************************************************************************************************
    '            Me.CancellaDocumentiConservati(cliente.CodLicenza, listaDocumentiConservati)
    '            Throw New Exception(messaggioErrore)
    '        Else
    '            '**************************************************************************************************************
    '            '2) AGGIORNO LA REGISTRAZIONE
    '            '**************************************************************************************************************
    '            registrazione.IdDocumentoWS = idDocumentoConservatoPadre
    '            registrazioni.SaveChanges()
    '        End If
    '        '**************************************************************************************************************

    '    Catch ex As Exception
    '        If ex.InnerException Is Nothing Then
    '            Throw New Exception(ex.Message)
    '        Else
    '            Throw New Exception(ex.InnerException.Message)
    '        End If
    '    End Try

    'End Sub

    '' ''Private Sub ConservaRegistrazioneProtocollo()

    '' ''    Dim listaDocumentiConservati As New List(Of Integer)
    '' ''    Dim clienti As New ParsecAdmin.ClientRepository
    '' ''    Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''    clienti.Dispose()

    '' ''    If cliente Is Nothing Then
    '' ''        Throw New ApplicationException("Cliente non trovato!")
    '' ''    End If

    '' ''    Try

    '' ''        Dim idDocumentoConservatoPadre As Integer = 0

    '' ''        Dim messaggioErrore As String = String.Empty

    '' ''        Dim wsConservazioneSoap As New wsConservazione.wsConservazione
    '' ''        Dim getEnteSoapRequest As New wsConservazione.SoapGetEnteConservatoreRequest
    '' ''        getEnteSoapRequest.codiceLicenza = cliente.CodLicenza
    '' ''        Dim enteConservatoreAttivo = wsConservazioneSoap.getEnteConservatoreAttivo(getEnteSoapRequest)
    '' ''        If Not String.IsNullOrEmpty(enteConservatoreAttivo.messaggioErrore) Then
    '' ''            Throw New ApplicationException(enteConservatoreAttivo.messaggioErrore)
    '' ''        End If

    '' ''        'OTTENGO L'OGGETTO COMPLETO
    '' ''        Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '' ''        Dim registrazione = registrazioni.GetById(Me.TaskAttivo.IdDocumento)

    '' ''        If registrazione.IdDocumentoWS.HasValue Then
    '' ''            Exit Sub
    '' ''        End If

    '' ''        If registrazione.Allegati Is Nothing Then
    '' ''            Throw New ApplicationException("E' necessario che ci sia almeno un allegato!")
    '' ''        End If
    '' ''        If registrazione.Allegati.Count = 0 Then
    '' ''            Throw New ApplicationException("E' necessario che ci sia almeno un allegato!")
    '' ''        End If


    '' ''        Dim algoritmoImpronta As String = enteConservatoreAttivo.ListaEntiConservatori(0).algoritmoImpronta

    '' ''        If String.IsNullOrEmpty(algoritmoImpronta) Then
    '' ''            Throw New ApplicationException("Algoritmo impronta non specificato!")
    '' ''        End If

    '' ''        algoritmoImpronta = algoritmoImpronta.Trim.ToUpper

    '' ''        '**************************************************************************************************************
    '' ''        '1) CONSERVO GLI ALLEGATI
    '' ''        '**************************************************************************************************************
    '' ''        messaggioErrore = Me.ConservaAllegatiProtocollo(registrazione, cliente.CodLicenza, idDocumentoConservatoPadre, listaDocumentiConservati, algoritmoImpronta)

    '' ''        If Not String.IsNullOrEmpty(messaggioErrore) Then
    '' ''            '**************************************************************************************************************
    '' ''            'ANNULLO I SALVATAGGI IN CASO DI ERRORE
    '' ''            '**************************************************************************************************************
    '' ''            Me.CancellaDocumentiConservati(cliente.CodLicenza, listaDocumentiConservati)
    '' ''            Throw New Exception(messaggioErrore)
    '' ''        Else
    '' ''            '**************************************************************************************************************
    '' ''            '2) AGGIORNO LA REGISTRAZIONE
    '' ''            '**************************************************************************************************************
    '' ''            registrazione.IdDocumentoWS = idDocumentoConservatoPadre
    '' ''            registrazioni.SaveChanges()
    '' ''        End If
    '' ''        '**************************************************************************************************************

    '' ''    Catch ex As Exception
    '' ''        If ex.InnerException Is Nothing Then
    '' ''            Throw New Exception(ex.Message)
    '' ''        Else
    '' ''            Throw New Exception(ex.InnerException.Message)
    '' ''        End If
    '' ''    End Try

    '' ''End Sub

    'Private Sub ConservaAttoAmministrativo()

    '    Dim listaAttiConservati As New List(Of Integer)
    '    Dim clienti As New ParsecAdmin.ClientRepository
    '    Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '    clienti.Dispose()

    '    If cliente Is Nothing Then
    '        Throw New ApplicationException("Cliente non trovato!")
    '    End If

    '    Dim wsConservazioneSoap As New wsConservazione.wsConservazione

    '    Dim algoritmoImpronta As String = String.Empty

    '    Try
    '        'OTTENGO L'OGGETTO COMPLETO
    '        Dim documenti As New ParsecAtt.DocumentoRepository
    '        Dim documento = documenti.GetFullById(Me.TaskAttivo.IdDocumento)

    '        If documento Is Nothing Then
    '            Throw New ApplicationException("Atto amministrativo non trovato!")
    '        End If


    '        Dim getEnteSoapRequest As New wsConservazione.SoapGetEnteConservatoreRequest
    '        getEnteSoapRequest.codiceLicenza = cliente.CodLicenza
    '        Dim enteConservatoreAttivo = wsConservazioneSoap.getEnteConservatoreAttivo(getEnteSoapRequest)
    '        If Not String.IsNullOrEmpty(enteConservatoreAttivo.messaggioErrore) Then
    '            Throw New ApplicationException(enteConservatoreAttivo.messaggioErrore)
    '        End If

    '        algoritmoImpronta = enteConservatoreAttivo.ListaEntiConservatori(0).algoritmoImpronta


    '        If String.IsNullOrEmpty(algoritmoImpronta) Then
    '            Throw New ApplicationException("Algoritmo impronta non specificato!")
    '        End If

    '        algoritmoImpronta = algoritmoImpronta.Trim.ToUpper

    '        If Not String.IsNullOrEmpty(algoritmoImpronta) Then
    '            If algoritmoImpronta <> ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1 AndAlso algoritmoImpronta <> ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256 Then
    '                Throw New ApplicationException("Algoritmo impronta non valido!")
    '            End If
    '        Else
    '            Throw New ApplicationException("Algoritmo impronta non specificato!")
    '        End If



    '        Dim messaggio As String = String.Empty
    '        Dim idDocumentoConservatoPadre As Integer = 0


    '        Try

    '            '**************************************************************************************************************
    '            'CERCO IL DOCUMENTO DA CONSERVARE
    '            '**************************************************************************************************************
    '            Dim nomeFilePrimarioDaConservare As String = Me.GetNomeFileFirmato(documento)

    '            If String.IsNullOrEmpty(nomeFilePrimarioDaConservare) Then

    '                ''SE NON TROVO IL DOCUMENTO FIRMATO CERCO IL DOCUMENTO PDF
    '                'Dim pubblicazioni As New ParsecMES.AlboRepository
    '                'Dim pubblicazione As ParsecMES.Pubblicazione = pubblicazioni.GetView(New ParsecMES.FiltroPubblicazione With {.IdDocumento = Me.TaskAttivo.IdDocumento}).FirstOrDefault
    '                'pubblicazione.Documenti = pubblicazioni.GetDocumenti(pubblicazione.Id)
    '                'pubblicazioni.Dispose()
    '                'If Not pubblicazione Is Nothing Then

    '                '    Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")
    '                '    Dim anno As String = pubblicazione.DataRegistrazione.Value.Year.ToString
    '                '    Dim documentoPrimario = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 1).FirstOrDefault

    '                '    If Not documentoPrimario Is Nothing Then
    '                '        If String.IsNullOrEmpty(documentoPrimario.Nomefile) Then
    '                '            Throw New Exception(messaggio & vbCrLf & "File '" & nomeFilePrimarioDaConservare.Replace("\", "/") & "' non trovato!")
    '                '        End If
    '                '        nomeFilePrimarioDaConservare = localPath & anno & "\" & documentoPrimario.Nomefile
    '                '    End If
    '                'End If

    '                'SE NON TROVO IL DOCUMENTO FIRMATO  CONSERVO IL DOCUMENTO ODT
    '                Dim annoEsercizio As Integer = Me.GetAnnoEsercizio(documento)
    '                nomeFilePrimarioDaConservare = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, documento.Nomefile)
    '            End If

    '            If Not IO.File.Exists(nomeFilePrimarioDaConservare) Then
    '                Throw New Exception(messaggio & vbCrLf & "File '" & nomeFilePrimarioDaConservare.Replace("\", "/") & "' non trovato!")
    '            End If
    '            '**************************************************************************************************************

    '            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '            Dim soggettoProduttore = utenteCollegato.Cognome & ", " & utenteCollegato.Nome & If(Not String.IsNullOrEmpty(utenteCollegato.CodiceFiscale), ", " & utenteCollegato.CodiceFiscale, "")


    '            '**************************************************************************************************************
    '            '1) CONSERVO IL DOCUMENTO FIRMATO O IL PDF ASSOCIATO ALL'ATTO AMMINISTRATIVO
    '            '**************************************************************************************************************

    '            Dim metaDato As New wsConservazione.MetaDatoAtto
    '            metaDato.DataChiusura = Now
    '            metaDato.DataDocumento = documento.DataDocumento
    '            metaDato.DataProtocollo = documento.DataOraRegistrazione
    '            metaDato.DestinatarioDocumento = String.Empty '?
    '            metaDato.FileName = IO.Path.GetFileName(nomeFilePrimarioDaConservare)
    '            metaDato.IdDocumentoConservatoPadre = Nothing
    '            metaDato.idDocumentoSep = documento.Id
    '            metaDato.idModuloSep = ParsecAdmin.TipoModulo.ATT
    '            metaDato.Informazioni = documento.Oggetto
    '            metaDato.NumeroDocumento = documento.ContatoreGenerale
    '            metaDato.NumeroProtocollo = If(documento.NumeroProtocollo.HasValue, documento.NumeroProtocollo.Value.ToString, String.Empty)
    '            metaDato.Oggetto = documento.Oggetto
    '            metaDato.SoggettoProduttoreDocumento = soggettoProduttore

    '            metaDato.TipoDocumento = documento.ToString.ToUpper
    '            'metaDatoAtto.TipoDocumento = documento.DescrizioneTipologia

    '            If Not String.IsNullOrEmpty(documento.DescrizioneUfficio) Then
    '                metaDato.Ufficio = documento.DescrizioneUfficio
    '            Else
    '                metaDato.Ufficio = documento.DescrizioneSettore
    '            End If

    '            If documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera Then
    '                metaDato.OrganoDeliberanteAtto = documento.DescrizioneTipologiaSeduta
    '            End If


    '            Select Case algoritmoImpronta
    '                Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1
    '                    metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash(nomeFilePrimarioDaConservare)
    '                   Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256
    '                    metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash256FromFile(nomeFilePrimarioDaConservare)
    '            End Select
    '            metaDato.AlgoritmoImpronta = algoritmoImpronta


    '            Dim request As New wsConservazione.SoapConservaAttoRequest
    '            request.codiceLicenza = cliente.CodLicenza
    '            request.file = Nothing 'IO.File.ReadAllBytes(nomeFilePrimarioDaConservare)
    '            request.sourcePath = nomeFilePrimarioDaConservare
    '            request.metaDatoAtto = metaDato

    '            Dim documentoConservato = wsConservazioneSoap.conservaAttoAllegato(request)

    '            If String.IsNullOrEmpty(documentoConservato.messaggioErrore) Then
    '                idDocumentoConservatoPadre = documentoConservato.ListaDocumentiBase(0).idDocumento
    '                listaAttiConservati.Add(idDocumentoConservatoPadre)
    '                Dim doc = documenti.Where(Function(c) c.Id = documento.Id).FirstOrDefault
    '                doc.IdDocumentoWS = idDocumentoConservatoPadre
    '            Else
    '                Throw New Exception(messaggio & vbCrLf & documentoConservato.messaggioErrore)
    '            End If
    '        Catch ex As Exception
    '            Throw New Exception(messaggio & vbCrLf & ex.Message)

    '        End Try

    '        '**************************************************************************************************************

    '        '**************************************************************************************************************
    '        '2) CONSERVO GLI ALLEGATI ASSOCIATI ALL'ATTO AMMINISTRATIVO
    '        '**************************************************************************************************************
    '        Dim messaggioErrore As String = String.Empty

    '        messaggioErrore = Me.ConservaAllegatiAttoAmministrativo(documento, idDocumentoConservatoPadre, cliente.CodLicenza, listaAttiConservati, algoritmoImpronta)
    '        If Not String.IsNullOrEmpty(messaggioErrore) Then

    '            '**************************************************************************************************************
    '            'ANNULLO I SALVATAGGI IN CASO DI ERRORE
    '            '**************************************************************************************************************
    '            Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '            Throw New Exception(messaggioErrore)
    '        End If
    '        '****************************************************************************************************************************



    '        '****************************************************************************************************************************
    '        '3) CONSERVO I FILE P7M ASSOCIATI ALLE FIRME DELLA PROPOSTA DI DELIBERA O DELLA PROPOSTA DI DETERMINA
    '        '****************************************************************************************************************************

    '        If documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera Then
    '            If documento.IdPadre.HasValue Then
    '                Dim proposta = documenti.GetFullById(documento.IdPadre.Value)
    '                messaggioErrore = Me.ConservaDocumentiFirmatiProposta(proposta, idDocumentoConservatoPadre, cliente.CodLicenza, listaAttiConservati, algoritmoImpronta)
    '                If Not String.IsNullOrEmpty(messaggioErrore) Then
    '                    '**************************************************************************************************************
    '                    'ANNULLO I SALVATAGGI IN CASO DI ERRORE E SOLLEVO L'ECCEZIONE
    '                    '**************************************************************************************************************
    '                    Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '                    Throw New Exception(messaggioErrore)
    '                End If
    '            End If
    '        End If

    '        If documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Then
    '            If documento.IdPadre.HasValue Then

    '                Dim parametri As New ParsecAdmin.ParametriRepository
    '                Dim parametro = parametri.GetByName("ConservaDocumentiFirmatiPropostaDetermina")
    '                parametri.Dispose()

    '                Dim conservaDocumentiFirmatiPropostaDetermina As Boolean = False
    '                If Not parametro Is Nothing Then
    '                    conservaDocumentiFirmatiPropostaDetermina = (parametro.Valore = "1")
    '                End If

    '                If conservaDocumentiFirmatiPropostaDetermina Then
    '                    Dim proposta = documenti.GetFullById(documento.IdPadre.Value)
    '                    If Not proposta Is Nothing Then
    '                        messaggioErrore = ConservaDocumentiFirmatiProposta(proposta, idDocumentoConservatoPadre, cliente.CodLicenza, listaAttiConservati, algoritmoImpronta)

    '                        If Not String.IsNullOrEmpty(messaggioErrore) Then
    '                            '**************************************************************************************************************
    '                            'ANNULLO I SALVATAGGI IN CASO DI ERRORE E SOLLEVO L'ECCEZIONE
    '                            '**************************************************************************************************************
    '                            Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '                            Throw New Exception(messaggioErrore)
    '                        End If
    '                    End If
    '                End If

    '            End If
    '        End If

    '        '****************************************************************************************************************************

    '        '****************************************************************************************************************************
    '        '4) CONSERVO IL FILE P7M DELLA RELATA DI PUBBLICAZIONE
    '        '****************************************************************************************************************************
    '        messaggioErrore = ConservaRelataPubblicazioneFirmata(documento, idDocumentoConservatoPadre, cliente.CodLicenza, listaAttiConservati, algoritmoImpronta)
    '        If Not String.IsNullOrEmpty(messaggioErrore) Then
    '            '**************************************************************************************************************
    '            'ANNULLO I SALVATAGGI IN CASO DI ERRORE E SOLLEVO L'ECCEZIONE
    '            '**************************************************************************************************************
    '            Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '            Throw New Exception(messaggioErrore)
    '        End If
    '        '****************************************************************************************************************************



    '        documenti.SaveChanges()
    '        documenti.Dispose()

    '    Catch ex As Exception
    '        Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '       Throw New Exception(ex.Message)
    '    End Try


    'End Sub


    '' ''Private Sub ConservaAttoAmministrativo()

    '' ''    Dim listaAttiConservati As New List(Of Integer)
    '' ''    Dim clienti As New ParsecAdmin.ClientRepository
    '' ''    Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''    clienti.Dispose()

    '' ''    If cliente Is Nothing Then
    '' ''        Throw New ApplicationException("Cliente non trovato!")
    '' ''    End If

    '' ''    Dim wsConservazioneSoap As New wsConservazione.wsConservazione

    '' ''    Dim algoritmoImpronta As String = String.Empty

    '' ''    Try
    '' ''        'OTTENGO L'OGGETTO COMPLETO
    '' ''        Dim documenti As New ParsecAtt.DocumentoRepository
    '' ''        Dim documento = documenti.GetFullById(Me.TaskAttivo.IdDocumento)

    '' ''        If documento Is Nothing Then
    '' ''            Throw New ApplicationException("Atto amministrativo non trovato!")
    '' ''        End If


    '' ''        Dim getEnteSoapRequest As New wsConservazione.SoapGetEnteConservatoreRequest
    '' ''        getEnteSoapRequest.codiceLicenza = cliente.CodLicenza
    '' ''        Dim enteConservatoreAttivo = wsConservazioneSoap.getEnteConservatoreAttivo(getEnteSoapRequest)
    '' ''        If Not String.IsNullOrEmpty(enteConservatoreAttivo.messaggioErrore) Then
    '' ''            Throw New ApplicationException(enteConservatoreAttivo.messaggioErrore)
    '' ''        End If

    '' ''        algoritmoImpronta = enteConservatoreAttivo.ListaEntiConservatori(0).algoritmoImpronta


    '' ''        If String.IsNullOrEmpty(algoritmoImpronta) Then
    '' ''            Throw New ApplicationException("Algoritmo impronta non specificato!")
    '' ''        End If

    '' ''        algoritmoImpronta = algoritmoImpronta.Trim.ToUpper

    '' ''        If Not String.IsNullOrEmpty(algoritmoImpronta) Then
    '' ''            If algoritmoImpronta <> ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1 AndAlso algoritmoImpronta <> ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256 Then
    '' ''                Throw New ApplicationException("Algoritmo impronta non valido!")
    '' ''            End If
    '' ''        Else
    '' ''            Throw New ApplicationException("Algoritmo impronta non specificato!")
    '' ''        End If



    '' ''        Dim messaggio As String = String.Empty
    '' ''        Dim idDocumentoConservatoPadre As Integer = 0


    '' ''        Try

    '' ''            '**************************************************************************************************************
    '' ''            'CERCO IL DOCUMENTO DA CONSERVARE
    '' ''            '**************************************************************************************************************
    '' ''            Dim nomeFilePrimarioDaConservare As String = Me.GetNomeFileFirmato(documento)

    '' ''            If String.IsNullOrEmpty(nomeFilePrimarioDaConservare) Then

    '' ''                ''SE NON TROVO IL DOCUMENTO FIRMATO CERCO IL DOCUMENTO PDF
    '' ''                'Dim pubblicazioni As New ParsecMES.AlboRepository
    '' ''                'Dim pubblicazione As ParsecMES.Pubblicazione = pubblicazioni.GetView(New ParsecMES.FiltroPubblicazione With {.IdDocumento = Me.TaskAttivo.IdDocumento}).FirstOrDefault
    '' ''                'pubblicazione.Documenti = pubblicazioni.GetDocumenti(pubblicazione.Id)
    '' ''                'pubblicazioni.Dispose()
    '' ''                'If Not pubblicazione Is Nothing Then

    '' ''                '    Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")
    '' ''                '    Dim anno As String = pubblicazione.DataRegistrazione.Value.Year.ToString
    '' ''                '    Dim documentoPrimario = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 1).FirstOrDefault

    '' ''                '    If Not documentoPrimario Is Nothing Then
    '' ''                '        If String.IsNullOrEmpty(documentoPrimario.Nomefile) Then
    '' ''                '            Throw New Exception(messaggio & vbCrLf & "File '" & nomeFilePrimarioDaConservare.Replace("\", "/") & "' non trovato!")
    '' ''                '        End If
    '' ''                '        nomeFilePrimarioDaConservare = localPath & anno & "\" & documentoPrimario.Nomefile
    '' ''                '    End If
    '' ''                'End If

    '' ''                'SE NON TROVO IL DOCUMENTO FIRMATO  CONSERVO IL DOCUMENTO ODT
    '' ''                Dim annoEsercizio As Integer = Me.GetAnnoEsercizio(documento)
    '' ''                nomeFilePrimarioDaConservare = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, documento.Nomefile)
    '' ''            End If

    '' ''            If Not IO.File.Exists(nomeFilePrimarioDaConservare) Then
    '' ''                Throw New Exception(messaggio & vbCrLf & "File '" & nomeFilePrimarioDaConservare.Replace("\", "/") & "' non trovato!")
    '' ''            End If
    '' ''            '**************************************************************************************************************

    '' ''            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''            Dim soggettoProduttore = utenteCollegato.Cognome & ", " & utenteCollegato.Nome & If(Not String.IsNullOrEmpty(utenteCollegato.CodiceFiscale), ", " & utenteCollegato.CodiceFiscale, "")


    '' ''            '**************************************************************************************************************
    '' ''            '1) CONSERVO IL DOCUMENTO FIRMATO O IL PDF ASSOCIATO ALL'ATTO AMMINISTRATIVO
    '' ''            '**************************************************************************************************************

    '' ''            Dim metaDato As New wsConservazione.MetaDatoAtto
    '' ''            metaDato.DataChiusura = Now
    '' ''            metaDato.DataDocumento = documento.DataDocumento
    '' ''            metaDato.DataProtocollo = documento.DataOraRegistrazione
    '' ''            metaDato.DestinatarioDocumento = String.Empty '?
    '' ''            metaDato.FileName = IO.Path.GetFileName(nomeFilePrimarioDaConservare)
    '' ''            metaDato.IdDocumentoConservatoPadre = Nothing
    '' ''            metaDato.idDocumentoSep = documento.Id
    '' ''            metaDato.idModuloSep = ParsecAdmin.TipoModulo.ATT
    '' ''            metaDato.Informazioni = documento.Oggetto
    '' ''            metaDato.NumeroDocumento = documento.ContatoreGenerale
    '' ''            metaDato.NumeroProtocollo = If(documento.NumeroProtocollo.HasValue, documento.NumeroProtocollo.Value.ToString, String.Empty)
    '' ''            metaDato.Oggetto = documento.Oggetto
    '' ''            metaDato.SoggettoProduttoreDocumento = soggettoProduttore

    '' ''            metaDato.TipoDocumento = documento.ToString.ToUpper
    '' ''            'metaDatoAtto.TipoDocumento = documento.DescrizioneTipologia

    '' ''            If Not String.IsNullOrEmpty(documento.DescrizioneUfficio) Then
    '' ''                metaDato.Ufficio = documento.DescrizioneUfficio
    '' ''            Else
    '' ''                metaDato.Ufficio = documento.DescrizioneSettore
    '' ''            End If

    '' ''            If documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera Then
    '' ''                metaDato.OrganoDeliberanteAtto = documento.DescrizioneTipologiaSeduta
    '' ''            End If


    '' ''            Select Case algoritmoImpronta
    '' ''                Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1
    '' ''                    metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash(nomeFilePrimarioDaConservare)
    '' ''                Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256
    '' ''                    metaDato.ImprontaFile = ParsecUtility.Utility.CalcolaHash256FromFile(nomeFilePrimarioDaConservare)
    '' ''            End Select
    '' ''            metaDato.AlgoritmoImpronta = algoritmoImpronta


    '' ''            Dim request As New wsConservazione.SoapConservaAttoRequest
    '' ''            request.codiceLicenza = cliente.CodLicenza
    '' ''            request.file = Nothing 'IO.File.ReadAllBytes(nomeFilePrimarioDaConservare)
    '' ''            request.sourcePath = nomeFilePrimarioDaConservare
    '' ''            request.metaDatoAtto = metaDato

    '' ''            Dim documentoConservato = wsConservazioneSoap.conservaAttoAllegato(request)

    '' ''            If String.IsNullOrEmpty(documentoConservato.messaggioErrore) Then
    '' ''                idDocumentoConservatoPadre = documentoConservato.ListaDocumentiBase(0).idDocumento
    '' ''                listaAttiConservati.Add(idDocumentoConservatoPadre)
    '' ''                Dim doc = documenti.Where(Function(c) c.Id = documento.Id).FirstOrDefault
    '' ''                doc.IdDocumentoWS = idDocumentoConservatoPadre
    '' ''            Else
    '' ''                Throw New Exception(messaggio & vbCrLf & documentoConservato.messaggioErrore)
    '' ''            End If
    '' ''        Catch ex As Exception
    '' ''            Throw New Exception(messaggio & vbCrLf & ex.Message)

    '' ''        End Try

    '' ''        '**************************************************************************************************************

    '' ''        '**************************************************************************************************************
    '' ''        '2) CONSERVO GLI ALLEGATI ASSOCIATI ALL'ATTO AMMINISTRATIVO
    '' ''        '**************************************************************************************************************
    '' ''        Dim messaggioErrore As String = String.Empty

    '' ''        messaggioErrore = Me.ConservaAllegatiAttoAmministrativo(documento, idDocumentoConservatoPadre, cliente.CodLicenza, listaAttiConservati, algoritmoImpronta)
    '' ''        If Not String.IsNullOrEmpty(messaggioErrore) Then

    '' ''            '**************************************************************************************************************
    '' ''            'ANNULLO I SALVATAGGI IN CASO DI ERRORE
    '' ''            '**************************************************************************************************************
    '' ''            Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '' ''            Throw New Exception(messaggioErrore)
    '' ''        End If
    '' ''        '****************************************************************************************************************************



    '' ''        '****************************************************************************************************************************
    '' ''        '3) CONSERVO I FILE P7M ASSOCIATI ALLE FIRME DELLA PROPOSTA DI DELIBERA O DELLA PROPOSTA DI DETERMINA-DECRETO-ORDINANZA
    '' ''        '****************************************************************************************************************************

    '' ''        If documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera Then
    '' ''            If documento.IdPadre.HasValue Then
    '' ''                Dim proposta = documenti.GetFullById(documento.IdPadre.Value)
    '' ''                messaggioErrore = Me.ConservaDocumentiFirmatiProposta(proposta, idDocumentoConservatoPadre, cliente.CodLicenza, listaAttiConservati, algoritmoImpronta)
    '' ''                If Not String.IsNullOrEmpty(messaggioErrore) Then
    '' ''                    '**************************************************************************************************************
    '' ''                    'ANNULLO I SALVATAGGI IN CASO DI ERRORE E SOLLEVO L'ECCEZIONE
    '' ''                    '**************************************************************************************************************
    '' ''                    Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '' ''                    Throw New Exception(messaggioErrore)
    '' ''                End If
    '' ''            End If
    '' ''        End If

    '' ''        If documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina OrElse documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto OrElse documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza Then
    '' ''            If documento.IdPadre.HasValue Then

    '' ''                Dim parametri As New ParsecAdmin.ParametriRepository
    '' ''                Dim parametro = parametri.GetByName("ConservaDocumentiFirmatiPropostaDetermina")
    '' ''                parametri.Dispose()

    '' ''                Dim conservaDocumentiFirmatiProposta As Boolean = False
    '' ''                If Not parametro Is Nothing Then
    '' ''                    conservaDocumentiFirmatiProposta = (parametro.Valore = "1")
    '' ''                End If

    '' ''                If conservaDocumentiFirmatiProposta Then
    '' ''                    Dim proposta = documenti.GetFullById(documento.IdPadre.Value)
    '' ''                    If Not proposta Is Nothing Then
    '' ''                        messaggioErrore = Me.ConservaDocumentiFirmatiProposta(proposta, idDocumentoConservatoPadre, cliente.CodLicenza, listaAttiConservati, algoritmoImpronta)

    '' ''                        If Not String.IsNullOrEmpty(messaggioErrore) Then
    '' ''                            '**************************************************************************************************************
    '' ''                            'ANNULLO I SALVATAGGI IN CASO DI ERRORE E SOLLEVO L'ECCEZIONE
    '' ''                            '**************************************************************************************************************
    '' ''                            Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '' ''                            Throw New Exception(messaggioErrore)
    '' ''                        End If
    '' ''                    End If
    '' ''                End If

    '' ''            End If
    '' ''        End If

    '' ''        '****************************************************************************************************************************

    '' ''        '****************************************************************************************************************************
    '' ''        '4) CONSERVO IL FILE P7M DELLA RELATA DI PUBBLICAZIONE
    '' ''        '****************************************************************************************************************************
    '' ''        messaggioErrore = ConservaRelataPubblicazioneFirmata(documento, idDocumentoConservatoPadre, cliente.CodLicenza, listaAttiConservati, algoritmoImpronta)
    '' ''        If Not String.IsNullOrEmpty(messaggioErrore) Then
    '' ''            '**************************************************************************************************************
    '' ''            'ANNULLO I SALVATAGGI IN CASO DI ERRORE E SOLLEVO L'ECCEZIONE
    '' ''            '**************************************************************************************************************
    '' ''            Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '' ''            Throw New Exception(messaggioErrore)
    '' ''        End If
    '' ''        '****************************************************************************************************************************



    '' ''        documenti.SaveChanges()
    '' ''        documenti.Dispose()

    '' ''    Catch ex As Exception
    '' ''        Me.CancellaDocumentiConservati(cliente.CodLicenza, listaAttiConservati)
    '' ''        Throw New Exception(ex.Message)
    '' ''    End Try


    '' ''End Sub


    Private Sub ModificaATT(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        Dim modificaDocumento = Me.Action.GetParameterByName("ModificaDocumento")
        '' ''Dim conservaAttoAmministrativoParametro = Me.Action.GetParameterByName("Conserva")

        Dim aggiungiAOrdineDelGiorno = Me.Action.GetParameterByName("AggiungiAOrdineDelGiorno")

        Dim modificaRagioniere = Me.Action.GetParameterByName("ModificaParere")

        Me.salvaImpegniDefinitiviDedaGroup = Me.Action.GetParameterByName("SalvaImpegniDefinitiviDedaGroup")

        If Not modificaDocumento Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            'OTTENGO L'OGGETTO COMPLETO
            Dim documento = documenti.GetFullById(Me.TaskAttivo.IdDocumento)
            If Not documento Is Nothing Then
                documento.NascondiDocumento = False
                documento.Rigenera = True
                documenti.GeneraDataSource(documento)
                documenti.Dispose()
                Me.RegistraScriptOpenOffice(documento, Me.salvaDocumentoButton.ClientID)
                Exit Sub
            End If
        End If

        '' ''If Not conservaAttoAmministrativoParametro Is Nothing Then
        '' ''    Try
        '' ''        Me.ConservaAttoAmministrativo()
        '' ''        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
        '' ''    Catch ex As Exception

        '' ''        Me.RegistraScriptPersistenzaVisibilitaPannello()
        '' ''        Me.EnableUiHidden.Value = "Abilita"
        '' ''        ParsecUtility.Utility.MessageBox("Impossibile conservare l'atto amministrativo per il seguente motivo: " & vbCrLf & ex.Message, False)
        '' ''        ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

        '' ''    End Try
        '' ''    Exit Sub
        '' ''End If

        If Not aggiungiAOrdineDelGiorno Is Nothing Then


            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
            documenti.Dispose()

            If Not documento Is Nothing Then

                Dim modelli As New ParsecAtt.ModelliRepository
                Dim modello As ParsecAtt.Modello = modelli.GetById(documento.IdModello)
                modelli.Dispose()

                Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
                Dim queryString As Hashtable = Me.SplitQueryString(pageUrl)


                queryString.Add("filtro", modello.IdTipologiaSeduta)
                queryString.Add("filtroDataSeduta", "1")
                queryString.Add("obj", Me.AggiornaIterButton.ClientID)

                If pageUrl.Contains("?") Then
                    pageUrl = pageUrl.Substring(0, pageUrl.IndexOf("?"))
                End If

                ParsecUtility.Utility.ShowPopup(pageUrl, 800, 400, queryString, False)
            End If


            Exit Sub
        End If

        If Not modificaRagioniere Is Nothing Then
            Me.VisualizzaAttoAmministrativoInModificaRagioniere()
            Exit Sub
        End If



        Me.VisualizzaAttoAmministrativoInModifica()
    End Sub


    Protected Sub salvaDocumentoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles salvaDocumentoButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento = documenti.Clone(Me.TaskAttivo.IdDocumento)
        documento = documenti.LoadOggettiCollegati(documento)

        documento = documenti.LoadAllegatiPubblicazione(documento)

        documento.LogIdUtente = utenteCollegato.Id
        documento.LogUtente = utenteCollegato.Username
        documento.IdAutore = utenteCollegato.Id
        documenti.Documento = documento
        documento.Rigenera = True
        Try
            documenti.Save(documento)
            Me.AggiornaIstanzaWorkflow(documento)
        Catch ex As Exception

        End Try
        documenti.Dispose()

        'Chiudo la pagina e aggiorno la griglia della pagina chiamante.
        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
        Me.SbloccaTasks(utenteCollegato.Id)
        Me.SbloccaDocumenti(utenteCollegato.Id)
    End Sub


    Private Sub VisualizzaAttoAmministrativoInModificaRagioniere()
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
        documenti.Dispose()

        If Not documento Is Nothing Then
            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)

            Dim queryString As Hashtable = Me.SplitQueryString(pageUrl)

            queryString.Add("Tipo", documento.IdTipologiaDocumento.ToString)
            queryString.Add("Iter", "1")
            queryString.Add("Mode", "Update")
            queryString.Add("obj", Me.AggiornaIterButton.ClientID)

            Dim nascondiDocumento = Not Me.Action.GetParameterByName("NascondiDocumento") Is Nothing
            Dim pubblicaTrasparenza As Boolean = Not Me.Action.GetParameterByName("PubblicaTrasparenza") Is Nothing

            queryString.Add("NascondiDocumento", nascondiDocumento)
            queryString.Add("PubblicaTrasparenza", pubblicaTrasparenza)

            If pageUrl.Contains("?") Then
                pageUrl = pageUrl.Substring(0, pageUrl.IndexOf("?"))
            End If

            Dim parametriPagina As New Hashtable
            parametriPagina.Add("IdDocumentoIter", documento.Id)


            parametriPagina.Add("IdFirmaModificabile", Me.GetFirmaRuolo.Id)

            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 700, queryString, False)
        End If
    End Sub


    Private Sub VisualizzaAttoAmministrativoInModifica()
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
        documenti.Dispose()

        If Not documento Is Nothing Then
            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)

            Dim queryString As Hashtable = Me.SplitQueryString(pageUrl)

            queryString.Add("Tipo", documento.IdTipologiaDocumento.ToString)
            queryString.Add("Iter", "1")
            queryString.Add("Mode", "Update")
            queryString.Add("obj", Me.AggiornaIterButton.ClientID)

            Dim nascondiDocumento = Not Me.Action.GetParameterByName("NascondiDocumento") Is Nothing
            Dim pubblicaTrasparenza As Boolean = Not Me.Action.GetParameterByName("PubblicaTrasparenza") Is Nothing

            queryString.Add("NascondiDocumento", nascondiDocumento)
            queryString.Add("PubblicaTrasparenza", pubblicaTrasparenza)

            If Not Me.salvaImpegniDefinitiviDedaGroup Is Nothing Then
                queryString.Add("SalvaImpegniDefinitiviDedaGroup", True)
            End If

            If pageUrl.Contains("?") Then
                pageUrl = pageUrl.Substring(0, pageUrl.IndexOf("?"))
            End If

            Dim parametriPagina As New Hashtable
            parametriPagina.Add("IdDocumentoIter", documento.Id)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 700, queryString, False)
        End If
    End Sub

#End Region


#Region "GESTIONE TASK E ISTANZA"

    Private Sub WriteTaskWithConditionsAndUpdateParent(ByVal expression As String)
        If Me.FirstClick Then
            Me.FirstClick = False
            Dim condition As ParsecWKF.Condition = Me.Action.Conditions.Where(Function(c) c.ExpressionValue = expression).FirstOrDefault
            If Not condition Is Nothing Then
                Dim processi As New ParsecWKF.ParametriProcessoRepository
                Dim parametroProcesso As ParsecWKF.ParametroProcesso = processi.GetQuery.Where(Function(c) c.IdProcesso = Me.TaskAttivo.IdIstanza And c.Nome = condition.ToActor).FirstOrDefault
                processi.Dispose()
                If Not parametroProcesso Is Nothing Then
                    Dim idDestinatario As Integer = CInt(parametroProcesso.Valore)
                    Dim notificato As Boolean = (Me.Action.FromActor = condition.ToActor)
                    'Dim operazione As String = Me.Action.Description.ToUpper
                    Dim operazione As String = condition.NextTask.ToUpper

                    Me.WriteTask2(idDestinatario, operazione, condition.NextTask, notificato)
                    ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
                End If
            End If
        End If

    End Sub

    Private Sub WriteTaskAndUpdateParent(ByVal idDestinatario As Integer)
        If Me.FirstClick Then
            Me.FirstClick = False
            Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
            Dim operazione As String = Me.Action.Description.ToUpper
            Me.WriteTask(idDestinatario, operazione, notificato)

            'NON FUNZIONA SU CHROME
            Me.RegistraButtonClick()
        End If
    End Sub

    Private Sub WriteAllTasksAndUpdateParent(ByVal idDestinatario As Integer)
        Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
        Dim operazione As String = Me.Action.Description.ToUpper
        For Each taskCorrente In Me.TaskAttivi
            Me.TaskAttivo = taskCorrente
            Me.WriteTask(idDestinatario, operazione, notificato)
        Next
        Me.FirstClick = False
        'NON FUNZIONA SU CHROME
        Me.RegistraButtonClick()
    End Sub

    Private Sub WriteAllTasksAndUpdateParent(ByVal elencoDestinatari As List(Of String))
        Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
        Dim operazione As String = Me.Action.Description.ToUpper
        Dim i As Integer = 0
        For Each taskCorrente In Me.TaskAttivi
            Dim idDestinatario = CInt(elencoDestinatari(i))
            Me.TaskAttivo = taskCorrente
            Me.WriteTask(idDestinatario, operazione, notificato)
            i += 1
        Next
        Me.FirstClick = False
        'NON FUNZIONA SU CHROME
        Me.RegistraButtonClick()
    End Sub

    Private Sub WriteAllTasksAndUpdateParent(ByVal elencoDestinatari As Dictionary(Of Integer, Integer))
        Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
        Dim operazione As String = Me.Action.Description.ToUpper

        For Each item As KeyValuePair(Of Integer, Integer) In elencoDestinatari
            Dim idTask = item.Key
            Dim idDestinatario = item.Value
            Me.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = idTask).FirstOrDefault
            Me.WriteTask(idDestinatario, operazione, notificato)
        Next

        Me.FirstClick = False
        'NON FUNZIONA SU CHROME
        Me.RegistraButtonClick()
    End Sub

    Private Sub WriteTaskAndUpdateParent(ByVal idDestinatario As Integer, ByVal operazione As String)
        If Me.FirstClick Then
            Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
            Me.WriteTask(idDestinatario, operazione, notificato)
            Me.FirstClick = False
            'NON FUNZIONA SU CHROME
            Me.RegistraButtonClick()
        End If
    End Sub

    Private Sub AggiornaIstanzaWorkflow(ByVal documento As ParsecAtt.Documento)
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
        If Not istanza Is Nothing Then
            istanza.IdDocumento = documento.Id
            istanza.Riferimento = documento.GetDescrizione
            istanza.ContatoreGenerale = documento.ContatoreGenerale
            istanze.SaveChanges()
        End If
        istanze.Dispose()
    End Sub

    Private Sub AggiornaIstanzaWorkflow(protocollo As ParsecPro.Registrazione)
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
        If Not istanza Is Nothing Then
            Dim descrizioneRegistrazione As String = String.Format("Prot. n. {0}/{1} - Oggetto : {2}", protocollo.NumeroProtocollo.ToString.PadLeft(7, "0"), protocollo.DataImmissione.Value.Year.ToString, protocollo.Oggetto)
            istanza.IdDocumento = protocollo.Id
            istanza.Riferimento = descrizioneRegistrazione
            istanza.ContatoreGenerale = protocollo.NumeroProtocollo
            istanze.SaveChanges()
        End If
        istanze.Dispose()
    End Sub


    Private Sub AggiornaIstanzaWorkflow(istanzaPratica As ParsecAdmin.IstanzaPraticaOnline)
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
        If Not istanza Is Nothing Then

            Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim protocollo As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
            registrazioni.Dispose()

            istanza.IdDocumento = istanzaPratica.Id
            istanza.Riferimento = String.Format("Istanza n. {0} del {1} - Prot. n. {2}/{3} - Oggetto : {4}", istanzaPratica.NumeroPratica, istanzaPratica.DataPresentazione.ToShortDateString, istanzaPratica.NumeroProtocollo.ToString.PadLeft(7, "0"), istanzaPratica.AnnoProtocollo.ToString, protocollo.Oggetto)
            'istanza.ContatoreGenerale = Nothing
            istanza.IdModulo = 17
            istanze.SaveChanges()
        End If
        istanze.Dispose()
    End Sub

    '**************************************************************************************************
    'Aggiorno il task corrente e inserisco il task successivo.
    '**************************************************************************************************
    Private Sub WriteTask(ByVal idDestinatario As Integer, ByVal operazione As String, ByVal notificato As Boolean)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.AggiungiUtenteVisibilita(idDestinatario)

        Dim statoIstanzaCompletato As Integer = 3


        Dim statoTaskEseguito As Integer = 6
        Dim statoTaskDaEseguire As Integer = 5

        Dim tasks As New ParsecWKF.TaskRepository
        'Aggiorno il task corrente
        Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.Id).FirstOrDefault

        If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
            task.Note = Me.NoteInterneTextBox.Text
        End If

        If Me.AutomaticTask = False Then
            Me.AutomaticTask = True
        Else
            'TASK PRECEDENTE
            Dim taskPrecedente As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = task.TaskPadre).FirstOrDefault
            If Not taskPrecedente Is Nothing Then
                taskPrecedente.Note = Nothing
            End If
        End If


        task.IdUtenteOperazione = utenteCollegato.Id

        task.IdStato = statoTaskEseguito
        task.DataEsecuzione = Now
        task.Destinatario = idDestinatario
        'If operazione = "FINE" Then
        '    task.Operazione = task.Corrente.ToUpper
        'Else
        task.Operazione = operazione.ToUpper
        'End If

        task.Notificato = notificato
        tasks.SaveChanges()

        'TODO Enum
        Dim actionType As String = Me.Action.Type.ToUpper


        'If operazione = "FINE" Then
        If actionType = "FINE" Then
            'Cambia lo stato del processo di workflow
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
            istanza.IdStato = statoIstanzaCompletato
            istanza.DataEsecuzione = Now
            istanze.SaveChanges()

            'SE IL TASK FINE NON E' STATO ANCORA REGISTRATO
            If task.Corrente <> "FINE" Then
                Dim nuovoTask As New ParsecWKF.Task
                nuovoTask.IdIstanza = Me.TaskAttivo.IdIstanza
                nuovoTask.Nome = task.Corrente
                nuovoTask.Corrente = operazione.ToUpper
                nuovoTask.Successivo = String.Empty
                nuovoTask.Destinatario = idDestinatario
                nuovoTask.Mittente = idDestinatario
                nuovoTask.TaskPadre = task.Id
                nuovoTask.DataEsecuzione = Now
                nuovoTask.DataInizio = task.DataInizio
                nuovoTask.DataFine = task.DataFine
                nuovoTask.IdStato = statoTaskEseguito
                nuovoTask.Operazione = operazione.ToUpper
                nuovoTask.Notificato = notificato
                nuovoTask.Note = task.Note
                nuovoTask.IdUtenteOperazione = utenteCollegato.Id
                tasks.Add(nuovoTask)
                tasks.SaveChanges()

            End If
        End If


        'If operazione <> "FINE" Then
        If actionType <> "FINE" Then
            Dim statoSuccessivo As String = ParsecWKF.ModelloInfo.StatoSuccessivoAction(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)

            'Inserisco un nuovo task valorizzando il riferimento del precedente
            Dim nuovotask As New ParsecWKF.Task
            nuovotask.IdIstanza = task.IdIstanza
            nuovotask.TaskPadre = task.Id

            nuovotask.Nome = Me.TaskAttivo.TaskCorrente
            nuovotask.Corrente = statoSuccessivo
            nuovotask.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo, Me.TaskAttivo.NomeFileIter)

            nuovotask.Mittente = idDestinatario
            ' nuovotask.Destinatario = 
            nuovotask.IdStato = statoTaskDaEseguire
            nuovotask.DataInizio = Now


            'If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
            '    nuovotask.Note = Me.NoteInterneTextBox.Text
            'End If
            'nuovotask.Note = task.Note ' Me.NoteInterneTextBox.Text

            'task.DataEsecuzione = Now

            Dim durata As Integer = 0
            If Not String.IsNullOrEmpty(statoSuccessivo) Then
                durata = ParsecWKF.ModelloInfo.DurataTaskIter(statoSuccessivo, Me.TaskAttivo.NomeFileIter)
            Else
                nuovotask.Operazione = "CANCELLATO"
            End If
            nuovotask.DataFine = Now.AddDays(durata)
            nuovotask.Notificato = notificato
            nuovotask.Cancellato = False
            nuovotask.IdUtenteOperazione = utenteCollegato.Id

            tasks.Add(nuovotask)
            tasks.SaveChanges()


        End If


        'aggiunto per ripristinare le pratiche sospese  27/02/2018 **
        If actionType = "ASSOCIA PROTOCOLLO SUE" Then
            If Not IsNothing(Session("NumeroIstanzaOnLine")) Then
                'Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim NumeroIstanzaOL = Session("NumeroIstanzaOnLine").ToString
                ' Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.NumeroPratica = NumeroIstanzaOL).FirstOrDefault


                Dim istanzeSUE_rep As New ParsecWKF.IstanzaRepository

                Dim IstanzaSUE As New ParsecWKF.Istanza
                Dim istanzePratica = istanzeSUE_rep.Where(Function(c) c.Riferimento.ToLower.Contains(NumeroIstanzaOL) And c.IdModulo = 5).FirstOrDefault


                ' Dim TaskAttivo2 As New ParsecWKF.TaskAttivo

                'leggere il task attivo della pratica

                'Dim tasks2 As New ParsecWKF.TaskRepository
                'Dim filtro2 As New ParsecWKF.TaskFiltro
                'filtro2.IdUtente = ParsecUtility.ut
                'Me.TaskAttivi = tasks2.GetView(filtro).Where(Function(c) taskIdList.Contains(c.Id)).ToList

                Dim utenteCollegato2 As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                Dim task2 As New ParsecWKF.TaskRepository
                Dim taskAttivo2 As ParsecWKF.TaskAttivo
                taskAttivo2 = task2.GetView(New ParsecWKF.TaskFiltro With {.IdUtente = utenteCollegato2.Id}).Where(Function(c) c.IdIstanza = istanzePratica.Id).FirstOrDefault

                If istanzePratica.IdStato = 8 Then
                    taskAttivo2 = task2.GetIstanzeSospeseSUE(New ParsecWKF.TaskFiltro With {.IdUtente = utenteCollegato2.Id}).Where(Function(c) c.IdIstanza = istanzePratica.Id).FirstOrDefault
                End If

                'Dim tasks2 As New ParsecWKF.TaskRepository
                'Aggiorno il task corrente



                Dim TaskP As New ParsecWKF.Task
                TaskP = task2.GetQuery.Where(Function(c) c.Id = taskAttivo2.Id).FirstOrDefault


                TaskP.IdStato = statoTaskEseguito
                TaskP.DataEsecuzione = Now

                TaskP.Destinatario = TaskP.Mittente


                TaskP.Operazione = operazione.ToUpper
                'End If

                TaskP.Notificato = notificato
                task2.SaveChanges()

                Dim statoSuccessivo2 As String

                statoSuccessivo2 = ParsecWKF.ModelloInfo.StatoSuccessivoAction(taskAttivo2.TaskCorrente, Me.Action.Name, taskAttivo2.NomeFileIter)

                If istanzePratica.IdStato = 8 Then
                    statoSuccessivo2 = ParsecWKF.ModelloInfo.StatoSuccessivoIter(taskAttivo2.TaskCorrente, taskAttivo2.NomeFileIter)
                End If



                'Inserisco un nuovo task valorizzando il riferimento del precedente
                Dim nuovotask2 As New ParsecWKF.Task
                nuovotask2.IdIstanza = TaskP.IdIstanza
                nuovotask2.TaskPadre = TaskP.Id

                nuovotask2.Nome = Me.TaskAttivo.TaskCorrente
                nuovotask2.Corrente = statoSuccessivo2
                nuovotask2.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo2, taskAttivo2.NomeFileIter)

                nuovotask2.Mittente = TaskP.Mittente


                nuovotask2.IdStato = 5



                nuovotask2.DataInizio = Now

                Dim durata2 As Integer = 0
                If Not String.IsNullOrEmpty(statoSuccessivo2) Then
                    durata2 = ParsecWKF.ModelloInfo.DurataTaskIter(statoSuccessivo2, taskAttivo2.NomeFileIter)
                Else
                    nuovotask2.Operazione = "CANCELLATO"
                End If
                nuovotask2.DataFine = Now.AddDays(durata2)
                nuovotask2.Notificato = notificato
                nuovotask2.Cancellato = False
                nuovotask2.IdUtenteOperazione = utenteCollegato.Id

                task2.Add(nuovotask2)
                task2.SaveChanges()

                Dim istanze As New ParsecWKF.IstanzaRepository
                Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = taskAttivo2.IdIstanza).FirstOrDefault
                If istanza.IdStato = 8 Then
                    istanza.IdStato = 1
                    istanza.DataEsecuzione = Now
                    istanze.SaveChanges()
                End If

                Session("NumeroIstanzaOnLine") = Nothing

            End If
        End If

        'aggiunto per ripristinare le pratiche SUAP sospese  17/04/2018 **
        If actionType = "ASSOCIA PROTOCOLLO SUAP" Then
            If Not IsNothing(Session("NumeroIstanzaOnLineSUAP")) Then
                'Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim NumeroIstanzaOL = Session("NumeroIstanzaOnLineSUAP").ToString
                ' Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.NumeroPratica = NumeroIstanzaOL).FirstOrDefault


                Dim istanzeSUAP_rep As New ParsecWKF.IstanzaRepository

                Dim IstanzaSUAP As New ParsecWKF.Istanza
                Dim istanzePratica = istanzeSUAP_rep.Where(Function(c) c.Riferimento.ToLower.Contains(NumeroIstanzaOL) And c.IdModulo = 8).FirstOrDefault


                ' Dim TaskAttivo2 As New ParsecWKF.TaskAttivo

                'leggere il task attivo della pratica

                'Dim tasks2 As New ParsecWKF.TaskRepository
                'Dim filtro2 As New ParsecWKF.TaskFiltro
                'filtro2.IdUtente = ParsecUtility.ut
                'Me.TaskAttivi = tasks2.GetView(filtro).Where(Function(c) taskIdList.Contains(c.Id)).ToList

                Dim utenteCollegato2 As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                Dim task2 As New ParsecWKF.TaskRepository
                Dim taskAttivo2 As ParsecWKF.TaskAttivo
                taskAttivo2 = task2.GetView(New ParsecWKF.TaskFiltro With {.IdUtente = utenteCollegato2.Id}).Where(Function(c) c.IdIstanza = istanzePratica.Id).FirstOrDefault

                If istanzePratica.IdStato = 8 Then
                    taskAttivo2 = task2.GetIstanzeSospeseSUAP(New ParsecWKF.TaskFiltro With {.IdUtente = utenteCollegato2.Id}).Where(Function(c) c.IdIstanza = istanzePratica.Id).FirstOrDefault
                End If

                'Dim tasks2 As New ParsecWKF.TaskRepository
                'Aggiorno il task corrente



                Dim TaskP As New ParsecWKF.Task
                TaskP = task2.GetQuery.Where(Function(c) c.Id = taskAttivo2.Id).FirstOrDefault


                TaskP.IdStato = statoTaskEseguito
                TaskP.DataEsecuzione = Now

                TaskP.Destinatario = TaskP.Mittente


                TaskP.Operazione = operazione.ToUpper
                'End If

                TaskP.Notificato = notificato
                task2.SaveChanges()

                Dim statoSuccessivo2 As String

                statoSuccessivo2 = ParsecWKF.ModelloInfo.StatoSuccessivoAction(taskAttivo2.TaskCorrente, Me.Action.Name, taskAttivo2.NomeFileIter)

                If istanzePratica.IdStato = 8 Then
                    statoSuccessivo2 = ParsecWKF.ModelloInfo.StatoSuccessivoIter(taskAttivo2.TaskCorrente, taskAttivo2.NomeFileIter)
                End If



                'Inserisco un nuovo task valorizzando il riferimento del precedente
                Dim nuovotask2 As New ParsecWKF.Task
                nuovotask2.IdIstanza = TaskP.IdIstanza
                nuovotask2.TaskPadre = TaskP.Id

                nuovotask2.Nome = Me.TaskAttivo.TaskCorrente
                nuovotask2.Corrente = statoSuccessivo2
                nuovotask2.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo2, taskAttivo2.NomeFileIter)

                nuovotask2.Mittente = TaskP.Mittente


                nuovotask2.IdStato = 5



                nuovotask2.DataInizio = Now

                Dim durata2 As Integer = 0
                If Not String.IsNullOrEmpty(statoSuccessivo2) Then
                    durata2 = ParsecWKF.ModelloInfo.DurataTaskIter(statoSuccessivo2, taskAttivo2.NomeFileIter)
                Else
                    nuovotask2.Operazione = "CANCELLATO"
                End If
                nuovotask2.DataFine = Now.AddDays(durata2)
                nuovotask2.Notificato = notificato
                nuovotask2.Cancellato = False
                nuovotask2.IdUtenteOperazione = utenteCollegato.Id

                task2.Add(nuovotask2)
                task2.SaveChanges()

                Dim istanze As New ParsecWKF.IstanzaRepository
                Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = taskAttivo2.IdIstanza).FirstOrDefault
                If istanza.IdStato = 8 Then
                    istanza.IdStato = 1
                    istanza.DataEsecuzione = Now
                    istanze.SaveChanges()
                End If

                Session("NumeroIstanzaOnLineSUAP") = Nothing

            End If
        End If



        '************************************************************



        If Not Me.OperazioneMassiva Then
            Dim taskSuccessivoAutomatico As ParsecWKF.ParametroProcesso = Me.Action.Parameters.Where(Function(c) c.Nome = "TaskSuccessivoAutomatico").FirstOrDefault
            If Not taskSuccessivoAutomatico Is Nothing Then
                Dim ultimoTask = tasks.GetQuery.Where(Function(c) c.TaskPadre = Me.TaskAttivo.Id).FirstOrDefault
                If Not ultimoTask Is Nothing Then
                    Me.GetTask(ultimoTask.Mittente, ultimoTask.Id, Me.TaskAttivo.IdModulo)

                    Dim list As List(Of ParsecWKF.Action) = ParsecWKF.ModelloInfo.ReadActionInfo(Me.TaskAttivo.TaskCorrente, Me.TaskAttivo.NomeFileIter)
                    If list.Count = 1 Then
                        Me.Action = list(0)
                    Else
                        Me.Action = list.Where(Function(c) c.Name.ToLower = taskSuccessivoAutomatico.Valore.ToLower).FirstOrDefault
                    End If

                    Select Case Me.Action.Type
                        Case "FIRMA"
                            Try
                                Me.FirstClick = True
                                Me.Firma(Nothing)
                            Catch ex As Exception
                                ParsecUtility.Utility.MessageBox(ex.Message, False)
                                'ABILITO L'INTERFACCIA
                                Me.EnableUiHidden.Value = "Abilita"
                                Me.SbloccaTasks(utenteCollegato.Id)
                            End Try
                        Case "INVIA AVANTI"
                            Try


                                Me.FirstClick = True
                                Me.RuoloId = Me.GetIdDestinatario


                                Me.Procedi(Direction.Forward)

                            Catch ex As Exception
                                ParsecUtility.Utility.MessageBox(ex.Message, False)
                                'ABILITO L'INTERFACCIA
                                Me.EnableUiHidden.Value = "Abilita"
                                Me.SbloccaTasks(utenteCollegato.Id)
                            End Try

                        Case "MODIFICA"
                            Select Case Me.TaskAttivo.IdModulo
                                Case ParsecAdmin.TipoModulo.PRO
                                    Try
                                        Me.FirstClick = True
                                        Me.FirmaDocumentoGenerico(Me.TaskAttivo.IdDocumento)
                                    Catch ex As Exception
                                        ParsecUtility.Utility.MessageBox(ex.Message, False)
                                        'ABILITO L'INTERFACCIA
                                        Me.EnableUiHidden.Value = "Abilita"
                                        Me.SbloccaTasks(utenteCollegato.Id)
                                    End Try

                            End Select
                    End Select



                End If
            End If
        End If


        tasks.Dispose()
    End Sub

    Private Sub WriteTask2(ByVal idDestinatario As Integer, ByVal operazione As String, ByVal statoSuccessivo As String, ByVal notificato As Boolean)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.AggiungiUtenteVisibilita(idDestinatario)

        Dim statoIstanzaCompletato As Integer = 3
        Dim statoTaskEseguito As Integer = 6
        Dim statoTaskDaEseguire As Integer = 5

        Dim tasks As New ParsecWKF.TaskRepository
        'Aggiorno il task corrente
        Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.Id).FirstOrDefault

        If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
            task.Note = Me.NoteInterneTextBox.Text
        End If

        task.IdUtenteOperazione = utenteCollegato.Id
        task.IdStato = statoTaskEseguito
        task.DataEsecuzione = Now
        task.Destinatario = idDestinatario
        'If operazione = "FINE" Then
        '    task.Operazione = task.Corrente.ToUpper
        'Else
        task.Operazione = operazione.ToUpper
        'End If

        task.Notificato = notificato
        tasks.SaveChanges()

        'Dim statoSuccessivo As String = ParsecWKF.ModelloInfo.StatoSuccessivoAction(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)

        Dim actionType As String = Me.Action.Type.ToUpper

        'If operazione = "FINE" Then
        If actionType = "FINE" Then
            'Cambia lo stato del processo di workflow
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
            istanza.IdStato = statoIstanzaCompletato
            istanza.DataEsecuzione = Now
            istanze.SaveChanges()

            'SE IL TASK FINE NON E' STATO ANCORA REGISTRATO
            If task.Corrente <> "FINE" Then
                Dim nuovoTask As New ParsecWKF.Task
                nuovoTask.IdIstanza = Me.TaskAttivo.IdIstanza
                nuovoTask.Nome = task.Corrente
                nuovoTask.Corrente = operazione.ToUpper
                nuovoTask.Successivo = String.Empty
                nuovoTask.Destinatario = idDestinatario
                nuovoTask.Mittente = idDestinatario
                nuovoTask.TaskPadre = task.Id
                nuovoTask.DataEsecuzione = Now
                nuovoTask.DataInizio = task.DataInizio
                nuovoTask.DataFine = task.DataFine
                nuovoTask.IdStato = statoTaskEseguito
                nuovoTask.Operazione = operazione.ToUpper
                nuovoTask.Notificato = notificato
                nuovoTask.Note = task.Note
                nuovoTask.IdUtenteOperazione = utenteCollegato.Id
                tasks.Add(nuovoTask)
                tasks.SaveChanges()

            End If
        End If

        'If operazione <> "FINE" Then
        If actionType <> "FINE" Then
            'Inserisco un nuovo task valorizzando il riferimento del precedente
            Dim nuovotask As New ParsecWKF.Task
            nuovotask.IdIstanza = task.IdIstanza
            nuovotask.TaskPadre = task.Id

            nuovotask.Nome = Me.TaskAttivo.TaskCorrente
            nuovotask.Corrente = statoSuccessivo
            nuovotask.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo, Me.TaskAttivo.NomeFileIter)

            nuovotask.Mittente = idDestinatario
            ' nuovotask.Destinatario = 
            nuovotask.IdStato = statoTaskDaEseguire
            nuovotask.DataInizio = Now
            If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
                nuovotask.Note = Me.NoteInterneTextBox.Text
            End If

            'task.DataEsecuzione = Now

            Dim durata As Integer = 0
            If Not String.IsNullOrEmpty(statoSuccessivo) Then
                durata = ParsecWKF.ModelloInfo.DurataTaskIter(statoSuccessivo, Me.TaskAttivo.NomeFileIter)
            Else
                nuovotask.Operazione = "CANCELLATO"
            End If
            nuovotask.DataFine = Now.AddDays(durata)
            nuovotask.Notificato = notificato
            nuovotask.Cancellato = False
            nuovotask.IdUtenteOperazione = utenteCollegato.Id

            tasks.Add(nuovotask)
            tasks.SaveChanges()
        End If



        tasks.Dispose()


    End Sub

#End Region

#Region "GESTIONE PROTOCOLLO"

    '' ''Private Sub ConservaFattura(ByRef fattura As ParsecPro.FatturaElettronica, ByVal numeroProtocollo As String, ByVal dataProtocollo As DateTime)
    '' ''    Dim messaggio As String = "La fattura elettronica con ident. sdi '" & fattura.IdentificativoSdI & "' non è stata conservata per il seguente motivo:"

    '' ''    Try

    '' ''        Dim clienti As New ParsecAdmin.ClientRepository
    '' ''        Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''        clienti.Dispose()

    '' ''        If cliente Is Nothing Then
    '' ''            Throw New Exception(messaggio & vbCrLf & "Cliente non trovato!")
    '' ''        End If

    '' ''        Dim nomeFileFattura = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.MessaggioSdI.Nomefile

    '' ''        If Not IO.File.Exists(nomeFileFattura) Then
    '' ''            Throw New Exception(messaggio & vbCrLf & "File '" & nomeFileFattura & "' non trovato")
    '' ''        End If

    '' ''        Dim fileFattura As Byte() = IO.File.ReadAllBytes(nomeFileFattura)

    '' ''        Dim wsConservazioneSoap As New wsConservazione.wsConservazione

    '' ''        Dim getEnteSoapRequest As New wsConservazione.SoapGetEnteConservatoreRequest
    '' ''        getEnteSoapRequest.codiceLicenza = cliente.CodLicenza
    '' ''        Dim enteConservatoreAttivo = wsConservazioneSoap.getEnteConservatoreAttivo(getEnteSoapRequest)
    '' ''        If Not String.IsNullOrEmpty(enteConservatoreAttivo.messaggioErrore) Then
    '' ''            Throw New ApplicationException(messaggio & vbCrLf & enteConservatoreAttivo.messaggioErrore)
    '' ''        End If


    '' ''        Dim algoritmoImpronta As String = enteConservatoreAttivo.ListaEntiConservatori(0).algoritmoImpronta

    '' ''        If String.IsNullOrEmpty(algoritmoImpronta) Then
    '' ''            Throw New ApplicationException(messaggio & vbCrLf & "Algoritmo impronta non specificato!")
    '' ''        End If

    '' ''        algoritmoImpronta = algoritmoImpronta.Trim.ToUpper

    '' ''        If Not String.IsNullOrEmpty(algoritmoImpronta) Then
    '' ''            If algoritmoImpronta <> ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1 AndAlso algoritmoImpronta <> ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256 Then
    '' ''                Throw New ApplicationException(messaggio & vbCrLf & "Algoritmo impronta non valido!")
    '' ''            End If
    '' ''        Else
    '' ''            Throw New ApplicationException(messaggio & vbCrLf & "Algoritmo impronta non specificato!")
    '' ''        End If


    '' ''        Dim ms As System.IO.MemoryStream = Nothing
    '' ''        ms = New System.IO.MemoryStream(ParsecUtility.Utility.FixVersioneXml(fileFattura).ToArray)

    '' ''        Dim fatturaElement As XElement = Nothing

    '' ''        Try
    '' ''            'è un xml forse
    '' ''            fatturaElement = XElement.Load(ms)
    '' ''        Catch ex As Exception
    '' ''            'è un p7m forse
    '' ''            Try
    '' ''                Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
    '' ''                'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
    '' ''                Try
    '' ''                    fileFattura = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(fileFattura))
    '' ''                Catch ex2 As Exception
    '' ''                    'NIENTE
    '' ''                End Try
    '' ''                signedCms.Decode(fileFattura)
    '' ''                ms = New IO.MemoryStream(ParsecUtility.Utility.FixVersioneXml(signedCms.ContentInfo.Content).ToArray)
    '' ''                fatturaElement = XElement.Load(ms)
    '' ''            Catch ex2 As Exception
    '' ''                'non è un formato valido
    '' ''                Throw New Exception(messaggio & vbCrLf & "Riscontrati problemi nella lettura della Fattura: " & vbCrLf & " Il file '" & nomeFileFattura & "' non è valido! " & vbCrLf & "Dettaglio errore:" & vbCrLf & ex2.Message)
    '' ''            End Try
    '' ''        End Try


    '' ''        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

    '' ''        Dim soggettoProduttore = utenteCollegato.Cognome & ", " & utenteCollegato.Nome & If(Not String.IsNullOrEmpty(utenteCollegato.CodiceFiscale), ", " & utenteCollegato.CodiceFiscale, "")



    '' ''        Dim idPaeseCedentePrestatore = fatturaElement.Element("FatturaElettronicaHeader").Element("CedentePrestatore").Element("DatiAnagrafici").Element("IdFiscaleIVA").Element("IdPaese").Value

    '' ''        Dim partitaIvaCessionarioCommittente As String = String.Empty
    '' ''        If Not fatturaElement.Element("FatturaElettronicaHeader").Element("CessionarioCommittente").Element("DatiAnagrafici").Element("IdFiscaleIVA") Is Nothing Then
    '' ''            partitaIvaCessionarioCommittente = fatturaElement.Element("FatturaElettronicaHeader").Element("CessionarioCommittente").Element("DatiAnagrafici").Element("IdFiscaleIVA").Element("IdCodice").Value
    '' ''        End If

    '' ''        Dim denominazioneCessionarioCommittente As String = String.Empty
    '' ''        If Not fatturaElement.Element("FatturaElettronicaHeader").Element("CessionarioCommittente").Element("DatiAnagrafici").Element("Anagrafica").Element("Denominazione") Is Nothing Then
    '' ''            denominazioneCessionarioCommittente = fatturaElement.Element("FatturaElettronicaHeader").Element("CessionarioCommittente").Element("DatiAnagrafici").Element("Anagrafica").Element("Denominazione").Value
    '' ''        Else
    '' ''            denominazioneCessionarioCommittente = cliente.Descrizione
    '' ''        End If


    '' ''        Dim codiceFiscaleCessionarioCommittente = String.Empty
    '' ''        If Not fatturaElement.Element("FatturaElettronicaHeader").Element("CessionarioCommittente").Element("DatiAnagrafici").Element("CodiceFiscale") Is Nothing Then
    '' ''            codiceFiscaleCessionarioCommittente = fatturaElement.Element("FatturaElettronicaHeader").Element("CessionarioCommittente").Element("DatiAnagrafici").Element("CodiceFiscale").Value
    '' ''        End If


    '' ''        Dim body = fatturaElement.Elements("FatturaElettronicaBody")

    '' ''        'Dim lista As New List(Of String)

    '' ''        'Dim i As Integer = 0
    '' ''        'For Each datiGenerali In body
    '' ''        '    If i > 0 Then
    '' ''        '        lista.Add(datiGenerali.Element("DatiGenerali").Element("DatiGeneraliDocumento").Element("Numero").Value & " / " & Date.Parse(datiGenerali.Element("DatiGenerali").Element("DatiGeneraliDocumento").Element("Data").Value).Year.ToString)
    '' ''        '    End If
    '' ''        '    i += 1
    '' ''        'Next

    '' ''        'Dim info As String = String.Empty
    '' ''        'If lista.Count > 0 Then
    '' ''        '    info = "Lotto contenente le seguenti altri fatture:" & String.Join("; ", lista.ToArray)
    '' ''        'End If

    '' ''        Dim datiGeneraliDocumentoElement = fatturaElement.Element("FatturaElettronicaBody").Element("DatiGenerali").Element("DatiGeneraliDocumento")

    '' ''        Dim tipoDocumento = datiGeneraliDocumentoElement.Element("TipoDocumento").Value
    '' ''        Dim dataFattura = Date.Parse(datiGeneraliDocumentoElement.Element("Data").Value)
    '' ''        Dim numeroDocumento = datiGeneraliDocumentoElement.Element("Numero").Value
    '' ''        Dim divisa = datiGeneraliDocumentoElement.Element("Divisa").Value

    '' ''        Dim totaleFattura As Nullable(Of Double) = Nothing
    '' ''        If Not datiGeneraliDocumentoElement.Element("ImportoTotaleDocumento") Is Nothing Then
    '' ''            Dim importo = datiGeneraliDocumentoElement.Element("ImportoTotaleDocumento").Value
    '' ''            If Not String.IsNullOrEmpty(importo) Then
    '' ''                totaleFattura = Double.Parse(importo, System.Globalization.NumberFormatInfo.InvariantInfo)
    '' ''            End If
    '' ''        End If


    '' ''        Dim metaDatoFattura As New wsConservazione.MetaDatoFattura
    '' ''        metaDatoFattura.DataDocumento = dataFattura
    '' ''        metaDatoFattura.FileName = IO.Path.GetFileName(nomeFileFattura)

    '' ''        metaDatoFattura.Oggetto = fattura.Oggetto '& "." & vbCrLf & info
    '' ''        metaDatoFattura.Informazioni = metaDatoFattura.Oggetto


    '' ''        metaDatoFattura.NumeroDocumento = numeroDocumento


    '' ''        metaDatoFattura.TipoDocumento = "FATTURAELETTRONICA"
    '' ''        metaDatoFattura.codiceDocumento = tipoDocumento
    '' ''        metaDatoFattura.tipologiaFattura = "PASSIVA"
    '' ''        metaDatoFattura.valuta = divisa
    '' ''        metaDatoFattura.totaleFattura = totaleFattura
    '' ''        metaDatoFattura.SoggettoProduttoreDocumento = soggettoProduttore
    '' ''        metaDatoFattura.codicePaeseCedente = idPaeseCedentePrestatore
    '' ''        metaDatoFattura.cedentePrestatoreFattura = fattura.DenominazioneFornitore
    '' ''        metaDatoFattura.partitaIVACedentePrestatore = fattura.PartitaIvaFornitore
    '' ''        metaDatoFattura.codicePaeseCessionario = "IT"

    '' ''        metaDatoFattura.cessionarioCommittente = denominazioneCessionarioCommittente

    '' ''        metaDatoFattura.partitaIvaCessionarioCommittente = partitaIvaCessionarioCommittente
    '' ''        metaDatoFattura.codiceFiscaleCessionarioCommittente = codiceFiscaleCessionarioCommittente

    '' ''        metaDatoFattura.DataProtocollo = dataProtocollo
    '' ''        metaDatoFattura.NumeroProtocollo = numeroProtocollo


    '' ''        Select Case algoritmoImpronta
    '' ''            Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA1
    '' ''                metaDatoFattura.ImprontaFile = ParsecUtility.Utility.CalcolaHash(nomeFileFattura)
    '' ''            Case ParsecUtility.AlgoritmiImprontaConservazioneEnumeration.SHA256
    '' ''                metaDatoFattura.ImprontaFile = ParsecUtility.Utility.CalcolaHash256FromFile(nomeFileFattura)
    '' ''        End Select
    '' ''        metaDatoFattura.AlgoritmoImpronta = algoritmoImpronta


    '' ''        Dim request As New wsConservazione.SoapConservaFatturaRequest
    '' ''        request.codiceLicenza = cliente.CodLicenza
    '' ''        request.file = Nothing 'fileFattura
    '' ''        request.sourcePath = nomeFileFattura
    '' ''        request.metaDatoFattura = metaDatoFattura


    '' ''        Dim documentoConservato = wsConservazioneSoap.conservaFatturaOrNotifica(request)


    '' ''        If String.IsNullOrEmpty(documentoConservato.messaggioErrore) Then
    '' ''            fattura.MessaggioSdI.IdDocumentoWS = documentoConservato.ListaDocumentiBase(0).idDocumento
    '' ''        Else
    '' ''            Throw New Exception(messaggio & vbCrLf & documentoConservato.messaggioErrore)
    '' ''        End If


    '' ''    Catch ex As Exception
    '' ''        If ex.InnerException Is Nothing Then
    '' ''            Throw New Exception(messaggio & vbCrLf & ex.Message)
    '' ''        Else
    '' ''            Throw New Exception(messaggio & vbCrLf & ex.InnerException.Message)
    '' ''        End If
    '' ''    End Try

    '' ''End Sub


    Private Sub TerminaTask()

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'TERMINO IL TASK CORRENTE
        Dim statoTaskEseguito As Integer = 6
        Dim statoIstanzaCompletato As Integer = 3

        Dim tasks As New ParsecWKF.TaskRepository
        Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.Id).FirstOrDefault
        task.IdUtenteOperazione = utenteCollegato.Id
        task.IdStato = statoTaskEseguito
        task.DataEsecuzione = Now
        task.Destinatario = Me.TaskAttivo.IdMittente
        task.Operazione = "MODIFICA"
        task.Notificato = True
        tasks.SaveChanges()

        'Cambia lo stato del processo di workflow
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
        istanza.IdStato = statoIstanzaCompletato
        istanza.DataEsecuzione = Now
        istanze.SaveChanges()

        Dim nuovoTask As New ParsecWKF.Task
        nuovoTask.IdIstanza = Me.TaskAttivo.IdIstanza
        nuovoTask.Nome = task.Corrente
        nuovoTask.Corrente = "FINE"
        nuovoTask.Successivo = String.Empty
        nuovoTask.Destinatario = Me.TaskAttivo.IdMittente
        nuovoTask.Mittente = Me.TaskAttivo.IdMittente
        nuovoTask.TaskPadre = task.Id
        nuovoTask.DataEsecuzione = Now
        nuovoTask.DataInizio = task.DataInizio
        nuovoTask.DataFine = task.DataFine
        nuovoTask.IdStato = statoTaskEseguito
        nuovoTask.Operazione = "FINE"
        nuovoTask.Notificato = True
        nuovoTask.Note = task.Note
        nuovoTask.IdUtenteOperazione = utenteCollegato.Id
        tasks.Add(nuovoTask)
        tasks.SaveChanges()
    End Sub

    Private Function GetDestinatarioIter(ByVal idReferenteInterno As Integer) As Integer
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim struttura = strutture.GetQuery.Where(Function(c) c.Id = idReferenteInterno).FirstOrDefault

        Dim idDestinatario As Nullable(Of Integer) = struttura.IDUtente

        If Not idDestinatario.HasValue OrElse idDestinatario.Value < 1 Then


            idDestinatario = (From s In strutture.GetQuery
                              Where s.IdPadre = idReferenteInterno And s.LogStato Is Nothing And s.IdGerarchia = 400 And s.Responsabile = True
                              Select s.IDUtente).FirstOrDefault

            If Not idDestinatario.HasValue Then

                Dim innerQuery = From s In strutture.GetQuery Where s.Id = idReferenteInterno And s.IdGerarchia = 200 Select s.IdPadre
                idDestinatario = (From s In strutture.GetQuery
                               Where s.IdPadre = innerQuery.FirstOrDefault And s.LogStato Is Nothing And s.IdGerarchia = 400 And s.Responsabile = True
                               Select s.IDUtente).FirstOrDefault

            End If
        End If


        strutture.Dispose()

        Return idDestinatario
    End Function

    Public Sub CreaIstanzaPro(ByVal protocollo As ParsecPro.Registrazione)


        Dim modelli As New ParsecWKF.ModelliRepository
        Dim idModello As Integer = Me.TaskAttivo.IdModello
        Dim modello As ParsecWKF.Modello = modelli.GetById(idModello)
        modelli.Dispose()

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim destinatariInterniInIter As List(Of ParsecPro.Destinatario) = protocollo.Destinatari.Where(Function(c) c.Interno = True And c.Iter = True).ToList

        For i As Integer = 0 To destinatariInterniInIter.Count - 1

            Dim statoIniziale As Integer = 1
            Dim idIstanza As Integer = 0


            Dim idReferenteInterno As Integer = destinatariInterniInIter(i).Id

            Dim strutture As New ParsecAdmin.StructureRepository
            Dim struttura = strutture.GetQuery.Where(Function(c) c.Id = idReferenteInterno).FirstOrDefault
            strutture.Dispose()

            Dim IdDestinatario As Nullable(Of Integer) = Me.GetDestinatarioIter(idReferenteInterno)

            Dim descrizioneRegistrazione As String = String.Format("Prot. n. {0}/{1} - Oggetto : {2}", protocollo.NumeroProtocollo.ToString.PadLeft(7, "0"), protocollo.DataImmissione.Value.Year.ToString, protocollo.Oggetto)


            'mittente del protocollo 
            Dim idMittente As Integer = utenteCollegato.Id


            Dim numeroProtocollo As Integer = protocollo.NumeroProtocollo
            Dim istanze As New ParsecWKF.IstanzaRepository

           

            Dim istanza As New ParsecWKF.Istanza
            istanza.Riferimento = descrizioneRegistrazione
            istanza.IdStato = statoIniziale
            istanza.DataInserimento = Now
            istanza.DataScadenza = Now.AddDays(modello.DurataIter)
            istanza.IdModello = modello.Id
            istanza.IdDocumento = protocollo.Id
            istanza.Ufficio = IdDestinatario
            istanza.ContatoreGenerale = numeroProtocollo
            istanza.IdModulo = modello.RiferimentoModulo
            istanza.IdUtente = utenteCollegato.Id
            istanza.FileIter = modello.NomeFile

            Try
                istanze.Save(istanza)
                idIstanza = istanze.Istanza.Id
                istanze.Dispose()

                '*******************************************************************************************************************************
                'Inserisco nei parametri del processo l'attore DESTINATARIO corrente.
                Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
                Dim processo As New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "DESTINATARIO", .Valore = IdDestinatario.ToString}
                parametriProcesso.Add(processo)
                parametriProcesso.SaveChanges()



                'Inserisco nei parametri del processo l'attore MITTENTE se non esiste.
                Dim parametroProcessoExist As Boolean = Not parametriProcesso.GetQuery.Where(Function(c) c.IdProcesso = idIstanza And c.Nome = "MITTENTE").FirstOrDefault Is Nothing
                If Not parametroProcessoExist Then
                    processo = New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "MITTENTE", .Valore = utenteCollegato.Id.ToString}
                    parametriProcesso.Add(processo)
                    parametriProcesso.SaveChanges()
                End If

                parametriProcesso.Dispose()
                '*******************************************************************************************************************************

                ' inserisco il task dell'istanza appena inserita
                Dim tasks As New ParsecWKF.TaskRepository
                Dim task As New ParsecWKF.Task
                task.IdIstanza = idIstanza
                task.Nome = ""
                Dim corrente As String = modello.StatoIniziale
                task.Corrente = corrente
                task.Successivo = modello.StatoSuccessivo(corrente)



                Dim list As List(Of ParsecWKF.Action) = ParsecWKF.ModelloInfo.ReadActionInfo(task.Corrente, modello.NomeFile)
                'Recupero il nome del ruolo (To) associato all'azione.
                Dim roleToName = list(0).ToActor

                Dim role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleToName).FirstOrDefault
                Dim idRuolo As Integer
                If Not role Is Nothing Then
                    idRuolo = role.Id
                    Dim ruoloUtente As ParsecWKF.RuoloRelUtente = (New ParsecWKF.RuoloRelUtenteRepository).GetQuery.Where(Function(c) c.IdRuolo = idRuolo).FirstOrDefault
                    If Not ruoloUtente Is Nothing Then
                        idMittente = ruoloUtente.IdUtente
                    End If
                End If

                task.Mittente = idMittente
                task.IdStato = 5
                task.DataInizio = Now
                task.DataEsecuzione = Now
                task.DataFine = Now.AddDays(modello.DurataTask)
                task.Operazione = "NUOVO"
                task.Cancellato = False
                task.IdUtenteOperazione = utenteCollegato.Id

                Try
                    tasks.Save(task)
                    tasks.Dispose()
                  
                    'Aggiungo il nuovo task
                    Me.Procedi(task, istanza, idMittente, IdDestinatario)

                Catch ex As Exception
                    Throw New ApplicationException(ex.Message)
                End Try

            Catch ex As Exception
                Throw New ApplicationException(ex.Message)
            End Try


        Next

    End Sub

    '*************************************************************************************************************
    'INSERISCO IL TASK AUTOMATICO
    '*************************************************************************************************************
    Private Sub Procedi(ByVal taskAttivo As ParsecWKF.Task, ByVal istanzaAttiva As ParsecWKF.Istanza, ByVal idUtente As Integer, ByVal idDestinatario As Integer)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim statoEseguito As Integer = 6
        Dim statoDaEseguire As Integer = 5

        Dim nomeFileIter As String = istanzaAttiva.FileIter

        Dim tasks As New ParsecWKF.TaskRepository
        tasks.Attach(taskAttivo)

        'Dim taskAttivo As ParsecWKF.TaskAttivo = tasks.GetView(New ParsecWKF.TaskFiltro With {.IdUtente = idUtente}).Where(Function(c) c.IdIstanza = idIstanza).FirstOrDefault

        Dim actions = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Corrente, nomeFileIter)

        Dim taskSuccessivoAutomatico As ParsecWKF.ParametroProcesso = actions(0).Parameters.Where(Function(c) c.Nome = "TaskSuccessivoAutomatico").FirstOrDefault

        Dim azione As ParsecWKF.Action = Nothing

        If Not taskSuccessivoAutomatico Is Nothing Then
            azione = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Successivo, nomeFileIter).Where(Function(c) c.Name = taskSuccessivoAutomatico.Valore).FirstOrDefault
        Else
            azione = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Successivo, nomeFileIter)(0)
        End If

        Dim operazione As String = "INIZIO"
        Dim procediAction = actions.Where(Function(c) c.Type = "INIZIO").FirstOrDefault
        If Not procediAction Is Nothing Then
            If Not String.IsNullOrEmpty(procediAction.Description) Then
                operazione = procediAction.Description.ToUpper
            End If
        End If


        'Recupero il nome del ruolo (To) associato all'azione.
        Dim roleToName = azione.ToActor
        Dim roleFromName = azione.FromActor
        If roleToName <> roleFromName Then
            idUtente = idDestinatario
        End If


        '*************************************************************************************************************
        'AGGIORNO IL TASK PRECEDENTE
        '*************************************************************************************************************
        'Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = taskAttivo.Id).FirstOrDefault
        If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
            taskAttivo.Note = Me.NoteInterneTextBox.Text
        End If


        taskAttivo.IdStato = statoEseguito
        taskAttivo.DataEsecuzione = Now
        taskAttivo.Operazione = operazione
        taskAttivo.Destinatario = idUtente
        taskAttivo.Notificato = True
        tasks.SaveChanges()
        '*************************************************************************************************************

        '*************************************************************************************************************
        'INSERISCO IL NUOVO TASK
        '*************************************************************************************************************
        Dim statoSuccessivo As String = ParsecWKF.ModelloInfo.StatoSuccessivoAction(taskAttivo.Successivo, azione.Name, nomeFileIter)
        Dim durata As Integer = ParsecWKF.ModelloInfo.DurataTaskIter(statoSuccessivo, nomeFileIter)

        Dim nuovotask As New ParsecWKF.Task
        nuovotask.IdIstanza = taskAttivo.IdIstanza
        nuovotask.TaskPadre = taskAttivo.Id

        nuovotask.Nome = taskAttivo.Corrente
        nuovotask.Corrente = statoSuccessivo
        nuovotask.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo, nomeFileIter)

        nuovotask.Mittente = idUtente
        nuovotask.IdStato = statoDaEseguire
        nuovotask.DataInizio = Now

        If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
            nuovotask.Note = Me.NoteInterneTextBox.Text
        End If


        nuovotask.DataFine = Now.AddDays(durata)
        nuovotask.Cancellato = False
        nuovotask.Notificato = False
        nuovotask.IdUtenteOperazione = utenteCollegato.Id

        tasks.Add(nuovotask)
        tasks.SaveChanges()
        '*************************************************************************************************************

        tasks.Dispose()
    End Sub


    Private Sub EsportaFatturaMassivaTINN()
        Dim sb As New System.Text.StringBuilder

        For Each taskCorrente In Me.TaskAttivi
            Me.TaskAttivo = taskCorrente

            Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = registrazioni.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault
            registrazioni.Dispose()

            If Not registrazione Is Nothing Then

                Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
                Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault

           

                If Not fattura Is Nothing Then

                    Dim nomefile As String = fattura.MessaggioSdI.Nomefile

                    Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefile

                    If IO.File.Exists(localPath) Then
                        Dim esportazioneFattureTINN As New ParsecPro.EsportazioneFatturaTINNRepository
                        Dim fatturaDaEsportare As New ParsecPro.EsportazioneFatturaTINN

                        Dim oggetto As String = fattura.Oggetto
                        Dim numeroFattura As String = String.Empty
                        Dim dataFattura As DateTime = Nothing
                        Dim reNumero As New Regex("(?<Numero>[\d]+)")
                        Dim reData As New Regex("(?<Data>[\d]{2}/[\d]{2}/[\d]{4})")
                        Dim m As Match = reNumero.Match(oggetto)
                        If m.Success Then
                            numeroFattura = m.Groups("Numero").Value
                        End If
                        m = reData.Match(oggetto)
                        If m.Success Then
                            dataFattura = CDate(m.Groups("Data").Value)
                        End If

                        fatturaDaEsportare.NomeFileFattura = nomefile
                        fatturaDaEsportare.PercorsoRelativo = fattura.MessaggioSdI.PercorsoRelativo
                        fatturaDaEsportare.AnnoProtocollo = fattura.AnnoProtocollo
                        fatturaDaEsportare.DataProtocollo = registrazione.DataImmissione.Value
                        fatturaDaEsportare.NumeroProtocollo = fattura.NumeroProtocollo
                        fatturaDaEsportare.NumeroFattura = numeroFattura
                        fatturaDaEsportare.DataFattura = dataFattura
                        fatturaDaEsportare.StatoFattura = "0" 'NON CONTABILIZZATA
                        fatturaDaEsportare.Inviata = False
                        fatturaDaEsportare.IdFattura = fattura.Id
                        Try

                            Dim esiste = esportazioneFattureTINN.Where(Function(c) c.IdFattura = fattura.Id).Any

                            If Not esiste Then
                                esportazioneFattureTINN.Add(fatturaDaEsportare)
                                esportazioneFattureTINN.SaveChanges()
                            End If

                            esportazioneFattureTINN.Dispose()


                            Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
                            Dim operazione As String = Me.Action.Description.ToUpper
                            Me.WriteTask(Me.TaskAttivo.IdMittente, operazione, notificato)

                            'Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)

                        Catch ex As Exception
                            Dim messaggio As String = String.Empty
                            If Not ex.InnerException Is Nothing Then
                                messaggio = ex.InnerException.Message
                            Else
                                messaggio = ex.Message
                            End If
                            'Throw New ApplicationException("Impossibile esportare la fattura per il seguente motivo: " & vbCrLf & messaggio)
                            sb.AppendLine("Il task " & Me.TaskAttivo.DescrizioneDocumento & " non è stato eseguito per il seguente motivo:" & vbCrLf)
                            sb.AppendLine("Impossibile esportare la fattura per il seguente motivo: " & vbCrLf & ex.Message)
                        End Try
                    Else
                        'Throw New ApplicationException("Il file della fattura " & nomefile & " non esiste!")
                        sb.AppendLine("Il task " & Me.TaskAttivo.DescrizioneDocumento & " non è stato eseguito per il seguente motivo:" & vbCrLf)
                        sb.AppendLine("Il file della fattura " & nomefile & " non esiste!")
                    End If
                Else
                    ' Throw New ApplicationException("La fattura associata al protocollo n. " & registrazione.NumeroProtocollo & " del " & registrazione.DataImmissione.Value.ToShortDateString & " non esiste!")
                    sb.AppendLine("Il task " & Me.TaskAttivo.DescrizioneDocumento & " non è stato eseguito per il seguente motivo:" & vbCrLf)
                    sb.AppendLine("La fattura associata al protocollo n. " & registrazione.NumeroProtocollo & " del " & registrazione.DataImmissione.Value.ToShortDateString & " non esiste!")
                End If
            Else
                'Throw New ApplicationException("Il protocollo associato al task corrente non esiste!")
                sb.AppendLine("Il task " & Me.TaskAttivo.DescrizioneDocumento & " non è stato eseguito per il seguente motivo:" & vbCrLf)
                sb.AppendLine("Il protocollo associato al task corrente non esiste!")
            End If
        Next


        If sb.Length > 0 Then

            Me.RegistraScriptPersistenzaVisibilitaPannello()
            ParsecUtility.Utility.MessageBox(sb.ToString, False)
            Me.RegistraButtonClick()
            'ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
            'Me.EnableUiHidden.Value = "Abilita"
        Else
            Me.FirstClick = False
            'NON FUNZIONA SU CHROME
            Me.RegistraButtonClick()
        End If


    End Sub


    Private Sub EstraiMetaDatiFatturaDaEmail(ByVal email As ParsecPro.EmailArrivo, ByVal localPathMetaDati As String)

        Dim element As XElement = Nothing
        Dim ms As IO.MemoryStream = Nothing
        Dim innerMessage As Rebex.Mail.MailMessage = Nothing
        Dim trovato As Boolean = False

        Dim fullPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata") & email.PercorsoRelativo & email.NomeFileEml.Replace("Prot_", "")

        If Not IO.File.Exists(fullPath) Then
            Exit Sub
        End If

        Dim message As New Rebex.Mail.MailMessage
        message.Settings.IgnoreInvalidTnefMessages = True

        message.Load(fullPath)

        For Each att In message.Attachments.Where(Function(c) c.FileName.ToLower.EndsWith(".eml")).ToList

            innerMessage = New Rebex.Mail.MailMessage
            innerMessage.Settings.IgnoreInvalidTnefMessages = True

            ms = New IO.MemoryStream
            att.Save(ms)
            ms.Position = 0
            innerMessage.Load(ms)

            For Each s In innerMessage.Attachments.Where(Function(c) c.FileName.ToLower.EndsWith(".xml")).ToList

                ms = New IO.MemoryStream
                s.Save(ms)
                ms.Position = 0
                ms = ParsecUtility.Utility.FixVersioneXml(ms.ToArray)

                Try
                    element = XElement.Load(ms)
                    If Not element Is Nothing Then
                        Dim identificativoSdIElement = element.Element("IdentificativoSdI")
                        Dim formatoElement = element.Element("Formato")

                        If Not identificativoSdIElement Is Nothing Then
                            If Not formatoElement Is Nothing Then
                                s.Save(localPathMetaDati)
                                trovato = True
                                Exit For
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try

                If trovato Then
                    Exit For
                End If

            Next

        Next
    End Sub


    Private Sub EsportaFatturaMassivaJSibacApSystems(ByVal tipoEsportazione As String)

        Dim sb As New System.Text.StringBuilder

        For Each taskCorrente In Me.TaskAttivi
            Me.TaskAttivo = taskCorrente

            Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
            registrazioni.Dispose()

            If Not registrazione Is Nothing Then
                Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
                Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
                If Not fattura Is Nothing Then
                    Dim nomefile As String = fattura.MessaggioSdI.Nomefile
                    Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefile
                    Dim localPathMetaDati As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.NomeFileMetadati
                    If IO.File.Exists(localPath) Then

                       

                        Dim password = WebConfigSettings.GetKey("FatturaServerPwd")
                        password = ParsecCommon.CryptoUtil.Decrypt(password)
                        Dim user = WebConfigSettings.GetKey("FatturaServerUser")
                        Dim port = WebConfigSettings.GetKey("FatturaServerPort")
                        Dim host = WebConfigSettings.GetKey("FatturaServerUrl").Split(New Char() {"//"}, StringSplitOptions.RemoveEmptyEntries)(1)
                        Dim url As String = WebConfigSettings.GetKey("FatturaServerPath")

                        Dim uri As String = String.Format("ftp://{0}:{1}/{2}", host, port.ToString, url)
                        Try
                            Me.UploadFile(user, password, uri, localPath)

                            Dim riga As String = String.Empty
                            Dim pos = nomefile.IndexOf(".")
                            Dim nomeFileSenzaEstensione As String = nomefile.Substring(0, pos)

                            Dim emails As New ParsecPro.EmailArrivoRepository
                            Dim email = emails.Where(Function(c) c.Id = fattura.MessaggioSdI.IdEmail).FirstOrDefault

                            If IO.File.Exists(localPathMetaDati) Then
                                Me.UploadFile(user, password, uri, localPathMetaDati)
                            Else
                                '*************************************************************************************************
                                'ESTRAGGO IL FILE METADATI DALLA EMAIL
                                '*************************************************************************************************
                                If Not email Is Nothing Then
                                    Me.EstraiMetaDatiFatturaDaEmail(email, localPathMetaDati)
                                    If IO.File.Exists(localPathMetaDati) Then
                                        Me.UploadFile(user, password, uri, localPathMetaDati)
                                    End If
                                End If
                                '*************************************************************************************************
                            End If

                            If tipoEsportazione = "0" Then

                                Dim destinatarioInterno = registrazione.Destinatari.Where(Function(c) c.Interno = True).FirstOrDefault
                                Dim dataProtocollo As String = registrazione.DataImmissione.Value.ToShortDateString.Replace("/", "")
                                Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
                                Dim dataConsegnaPec As String = fattura.MessaggioSdI.DataRicezioneInvio.ToShortDateString.Replace("/", "")
                                Dim utenteSdI As String = If(Not email Is Nothing, email.Mittente, "")
                                Dim nomeFileFattura As String = fattura.MessaggioSdI.Nomefile
                                Dim nomeFileMetaDatiFattura As String = fattura.NomeFileMetadati

                                Dim ufficioBilancio As String = String.Empty
                                If Not destinatarioInterno Is Nothing Then
                                    ufficioBilancio = destinatarioInterno.UfficioBilancio
                                End If
                                Dim statoFattura As String = "A"
                                Dim identificativoSdI As String = fattura.IdentificativoSdI
                                Dim codiceIPA As String = fattura.CodiceIpaDestinatario


                                riga = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", dataProtocollo, numeroProtocollo, dataConsegnaPec, utenteSdI, nomeFileFattura, nomeFileMetaDatiFattura, ufficioBilancio, statoFattura, identificativoSdI, codiceIPA)
                                nomefile = nomefile.Substring(0, pos) & ".fec"
                            Else
                                nomefile = nomeFileSenzaEstensione & ".txt"
                                Dim nomeFileFattura As String = nomeFileSenzaEstensione & ".xml"
                                Dim dataProtocollo As String = String.Format("{0:yyyyMMdd}", registrazione.DataImmissione.Value)
                                Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
                                riga = String.Format("{0};{1};{2}", nomeFileFattura, dataProtocollo, numeroProtocollo)
                            End If


                            Dim buffer As Byte() = System.Text.Encoding.Default.GetBytes(riga)
                            Me.UploadFile(user, password, uri, nomefile, buffer)

                            Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
                            Dim operazione As String = Me.Action.Description.ToUpper
                            Me.WriteTask(Me.TaskAttivo.IdMittente, operazione, notificato)

                            'Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                            fattura.IdStato = CInt(ParsecPro.StatoFattura.Contabilizzata)
                            fattureElettroniche.SaveChanges()



                        Catch ex As Exception
                            sb.AppendLine("Il task " & Me.TaskAttivo.DescrizioneDocumento & " non è stato seguito per il seguente motivo:")

                            sb.AppendLine("Impossibile esportare la fattura per il seguente motivo: " & ex.Message)
                        End Try
                    Else
                        sb.AppendLine("Il task " & Me.TaskAttivo.DescrizioneDocumento & " non è stato seguito per il seguente motivo:")

                        sb.AppendLine("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!")
                    End If
                Else
                    sb.AppendLine("Il task " & Me.TaskAttivo.DescrizioneDocumento & " non è stato seguito per il seguente motivo:")

                    sb.AppendLine("Il protocollo selezionato non ha registrato nessuna fattura!")
                End If
            Else
                sb.AppendLine("Il task " & Me.TaskAttivo.DescrizioneDocumento & " non è stato seguito per il seguente motivo:")

                sb.AppendLine("Il protocollo selezionato non ha registrato nessuna fattura!")
            End If
        Next

        If sb.Length > 0 Then

            Me.RegistraScriptPersistenzaVisibilitaPannello()
            ParsecUtility.Utility.MessageBox(sb.ToString, False)
            Me.RegistraButtonClick()
            'ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
            'Me.EnableUiHidden.Value = "Abilita"
        Else
            Me.FirstClick = False
            'NON FUNZIONA SU CHROME
            Me.RegistraButtonClick()
        End If

    End Sub


    Private Sub VisualizzaIstanzaPratica()
        Dim queryString As New Hashtable
        Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
        Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
        Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.IdRegistrazione = idRegistrazione).FirstOrDefault
        istanzePraticaOnline.Dispose()
        If Not istanzePratica Is Nothing Then
            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
            queryString.Add("obj", Me.AggiornaIterButton.ClientID)
            queryString.Add("IdIstanzaPraticaOnline", istanzePratica.Id.ToString)
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 710, queryString, False)
        End If
    End Sub

    Private Sub VisualizzaDocumentoGenerico(ByVal documentoGenerico As ParsecAdmin.DocumentoGenerico, ByVal modello As ParsecAdmin.ModelloDocumento)

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim visibile As Boolean = False

        If modello.VisibilitaPubblica.HasValue Then
            visibile = modello.VisibilitaPubblica
        End If

        If Not visibile Then
            visibile = (utenteCorrente.SuperUser OrElse DocumentoGenerico.IdUtente = utenteCorrente.Id)
        End If

        If visibile Then

            Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
            Dim sorgenti As New ParsecAdmin.SorgentiRepository
            Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
            sorgenti.Dispose()
            Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
            Dim fileExists As Boolean = False
            Select Case sorgenteCorrente.IdTipologia
                Case 1 'Locale
                    Dim localPath As String = source.Path & anno & "\" & documentoGenerico.NomeFile
                    fileExists = IO.File.Exists(localPath)
                Case 2  'Ftp
                    Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
                    fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
            End Select

            If Not fileExists Then
                ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
                Exit Sub
            Else
                Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, True, source)

                ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

            End If
        Else
            ParsecUtility.Utility.MessageBox("Non si possiedono le autorizzazioni per visualizzare il documento!", False)
        End If
    End Sub


    Private Sub ModificaIOL(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        Dim preparaDocumentoParametro = Me.Action.GetParameterByName("PreparaDocumento")
        Dim protocollazioneParametro = Me.Action.GetParameterByName("Protocollazione")
        Dim protocollazioneIntegrazioneParametro = Me.Action.GetParameterByName("ProtocollazioneIntegrazione")
        Dim disabilitaIterParametro = Me.Action.GetParameterByName("DisabilitaIter")
        Dim inviaEmailParametro = Me.Action.GetParameterByName("InviaEmail")
        Dim richiediIntegrazioneParametro = Me.Action.GetParameterByName("RichiediIntegrazione")


        '******************************************************************************************************************************************************************
        '1) ESEGUO IL TASK RICHIEDI INTEGRAZIONE
        '******************************************************************************************************************************************************************
        If Not richiediIntegrazioneParametro Is Nothing Then

            Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
            Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing And (c.Inviato = False Or c.Inviato Is Nothing)).FirstOrDefault

            '***************************************************************************************
            'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
            '***************************************************************************************
            If Not documentoGenerico Is Nothing Then
                Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)
                Me.VisualizzaDocumentoGenerico(documentoGenerico, modello)
            Else
                '***************************************************************************************
                'Apro la pagina per la generazione del documento generico.
                'Il salvattaggio esegue il click sul AggiornaIterButton  (persistenza stato del processo corrente)
                '***************************************************************************************
                Me.VisualizzaDocumentoGenericoInModifica()
            End If
            documentiGenerici.Dispose()
            Exit Sub

        End If
        '******************************************************************************************************************************************************************


        '******************************************************************************************************************************************************************
        '2) ESEGUO IL TASK PREPARA DOCUMENTO
        '******************************************************************************************************************************************************************

        If Not preparaDocumentoParametro Is Nothing Then

            Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
            Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing And (c.Inviato = False Or c.Inviato Is Nothing)).FirstOrDefault

            '***************************************************************************************
            'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
            '***************************************************************************************
            If Not documentoGenerico Is Nothing Then
                Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)
                Me.VisualizzaDocumentoGenerico(documentoGenerico, modello)

            Else

                '***************************************************************************************
                'Apro la pagina per la generazione del documento generico.
                'Il salvattaggio esegue il click sul AggiornaIterButton  (persistenza stato del processo corrente)
                '***************************************************************************************

                Me.VisualizzaDocumentoGenericoInModifica()
            End If

            documentiGenerici.Dispose()

            Exit Sub
        End If
        '******************************************************************************************************************************************************************

        '******************************************************************************************************************************************************************
        '3) ESEGUO IL TASK PROTOCOLLA
        '******************************************************************************************************************************************************************
        If Not protocollazioneParametro Is Nothing OrElse Not protocollazioneIntegrazioneParametro Is Nothing Then

            Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
            Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = TaskAttivo.IdIstanza AndAlso c.Stato Is Nothing And (c.Inviato = False Or c.Inviato Is Nothing)).FirstOrDefault

            'Se il documento generico non è stato ancora generato
            If documentoGenerico Is Nothing Then
                ParsecUtility.Utility.MessageBox("Per protocollare è necessario generare il documento!", False)
                Me.EnableUiHidden.Value = "Abilita"
            Else

                Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim pratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                pratiche.Dispose()
                Dim idRegistrazione As Integer = pratica.IdRegistrazione

                Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, e.CommandName, Me.TaskAttivo.NomeFileIter)
                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)

                If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna Then
                    Dim mittenti As List(Of ParsecPro.Mittente) = registrazione.Mittenti
                    Dim destinatari As List(Of ParsecPro.Destinatario) = registrazione.Destinatari
                    'Rendo effettive le modifiche di inversione dei due gruppi di referenti
                    registrazione.Mittenti = ParsecPro.Destinatari.ConvertiInMittenti(destinatari)
                    registrazione.Destinatari = ParsecPro.Mittenti.ConvertiInDestinatari(mittenti)

                    If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo Then
                        registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza
                    Else
                        registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna
                        For Each dest In registrazione.Destinatari.Where(Function(c) c.Interno = True)
                            dest.Iter = True
                        Next
                    End If

                    registrazione.TipoRegistrazione = CInt(registrazione.TipologiaRegistrazione)
                    registrazione.DataDocumento = Now

                    '*****************************************************************************
                    'Aggiungo il collegamento alla registrazione che sta per essere inserita
                    '*****************************************************************************
                    'Dim collegamento As New ParsecPro.Collegamento
                    'collegamento.AnnoProtocollo2 = Year(registrazione.DataImmissione)
                    'collegamento.NumeroProtocollo2 = registrazione.NumeroProtocollo
                    'collegamento.TipoRegistrazione2 = ParsecPro.TipoRegistrazione.Arrivo



                    'collegamento.Diretto = True
                    'collegamento.Oggetto = registrazione.Oggetto
                    'collegamento.NumeroProtocollo = collegamento.GetNumeroProtocollo(registrazione.NumeroProtocollo, registrazione.TipoRegistrazione)

                    'Dim referenti As New StringBuilder
                    'Dim uffici As New StringBuilder


                    'For Each m As ParsecPro.Mittente In registrazione.Mittenti
                    '    If m.Interno Then
                    '        uffici.Append(m.Descrizione)
                    '    Else
                    '        referenti.Append(m.Descrizione)
                    '    End If
                    'Next

                    'For Each d As ParsecPro.Destinatario In registrazione.Destinatari
                    '    If d.Interno Then
                    '        uffici.Append(d.Descrizione)
                    '    Else
                    '        referenti.Append(d.Descrizione)
                    '    End If
                    'Next

                    'collegamento.Referenti = referenti.ToString
                    'collegamento.Uffici = uffici.ToString

                    'registrazione.Collegamenti.Clear()
                    'registrazione.Collegamenti.Add(collegamento)


                    registrazione.NumeroProtocolloRiscontro = registrazione.NumeroProtocollo
                    registrazione.DataImmissioneRiscontro = String.Format("{0:dd/MM/yyyy}", registrazione.DataImmissione)





                    '*****************************************************************************
                    'Copio l'allegato nella cartella temporanea.
                    '*****************************************************************************
                    Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
                    Dim sorgenti As New ParsecAdmin.SorgentiRepository
                    Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
                    sorgenti.Dispose()
                    Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
                    Dim fileExists As Boolean = False

                    Dim localPath As String = String.Empty

                    Select Case sorgenteCorrente.IdTipologia
                        Case 1 'Locale
                            localPath = source.Path & anno & "\" & documentoGenerico.NomeFile
                            fileExists = IO.File.Exists(localPath)
                        Case 2  'Ftp
                            Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
                            fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
                    End Select

                    If Not fileExists Then
                        ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
                        Exit Sub
                    Else
                        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & documentoGenerico.NomeFile
                        IO.File.Copy(localPath, pathDownload, True)


                        '*****************************************************************************
                        'Aggiungo l'allegato (documento generico) alla registrazione che sta per essere inserita
                        '*****************************************************************************

                        Dim allegato As New ParsecPro.Allegato
                        allegato.NomeFile = documentoGenerico.NomeFile
                        allegato.NomeFileTemp = documentoGenerico.NomeFile
                        allegato.IdTipologiaDocumento = 1 'Primario
                        allegato.DescrizioneTipologiaDocumento = "Primario"
                        If Not protocollazioneIntegrazioneParametro Is Nothing Then
                            allegato.Oggetto = "Documento di richiesta integrazione"
                        Else
                            allegato.Oggetto = "Documento di risposta"
                        End If

                        allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                        allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
                        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
                        registrazione.Allegati.Clear()
                        registrazione.Allegati.Add(allegato)
                        '*****************************************************************************
                    End If



                End If

                Dim parametriPagina As New Hashtable
                parametriPagina.Add("RegistrazioneInIter", registrazione)


                Dim tipo As String = "1"
                If Not protocollazioneParametro Is Nothing Then
                    Select Case protocollazioneParametro.Valore.ToUpper
                        Case "INTERNO"
                            tipo = "2"
                            ' parametriPagina.Add("TaskAttivo", Me.TaskAttivo)
                        Case "ARRIVO"
                            tipo = "0"
                    End Select
                End If


                If Not disabilitaIterParametro Is Nothing Then
                    parametriPagina.Add("DisabilitaIter", "1")
                End If


                Dim queryString As New Hashtable
                queryString.Add("Mode", "Insert")
                queryString.Add("Tipo", tipo) 'Partenza
                queryString.Add("obj", Me.AggiornaIterButton.ClientID)

                'queryString.Add("IstanzaOnline", istanzaOnline.IterProcedimento)
                'If avviaIter Then
                '    queryString.Add("AvviaIter", "1")
                'Else
                queryString.Add("AvviaIter", "0")
                'End If

                Dim istanzeOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim istanzaOnline = istanzeOnline.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                istanzeOnline.Dispose()

                Dim istanza As New ParsecPro.IstanzaOnline
                istanza.Nuovo = False
                istanza.Id = istanzaOnline.Id
                istanza.IdProcedimento = istanzaOnline.IdProcedimento
                istanza.NumeroPratica = istanzaOnline.NumeroPratica

                parametriPagina.Add("IstanzaOnline", istanza)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 600, queryString, False)


            End If

            Exit Sub
        End If
        '******************************************************************************************************************************************************************


        '******************************************************************************************************************************************************************
        '4) ESEGUO IL TASK INVIA EMAIL
        '******************************************************************************************************************************************************************
        If Not inviaEmailParametro Is Nothing Then
            Me.VisualizzaEmailInModifica()
            Exit Sub
        End If
        '******************************************************************************************************************************************************************

    End Sub

    'Private Sub ModificaPRO(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

    '    '*************************************************************************************************************************************
    '    'Leggo tutti i parametri 
    '    '*************************************************************************************************************************************

    '    Dim preparaDocumentoParametro = Me.Action.GetParameterByName("PreparaDocumento")
    '    Dim riferimentDocumentoParametro = Me.Action.GetParameterByName("RiferimentoDocumento") 'OUT
    '    Dim protocollazioneParametro = Me.Action.GetParameterByName("Protocollazione")
    '    Dim inviaEmailParametro = Me.Action.GetParameterByName("InviaEmail")

    '    Dim notificaEsitoFatturaParametro = Me.Action.GetParameterByName("NotificaEsitoFattura")
    '    Dim esportaFatturaParametro = Me.Action.GetParameterByName("EsportaFattura")

    '    Dim visualizzaFatturaParametro = Me.Action.GetParameterByName("VisualizzaFattura")
    '    Dim conservaFatturaParametro = Me.Action.GetParameterByName("Conserva")
    '    Dim avviaIterParametro = Me.Action.GetParameterByName("AvviaIter")
    '    Dim disabilitaIterParametro = Me.Action.GetParameterByName("DisabilitaIter")

    '    Dim creaIstanzaPratica = Me.Action.GetParameterByName("CreaIstanzaPratica")
    '    Dim visualizzaIstanzaPratica = Me.Action.GetParameterByName("VisualizzaIstanzaPratica")

    '    Dim conservaProtocolloParametro = Me.Action.GetParameterByName("ConservaProtocollo")


    '    If Not conservaProtocolloParametro Is Nothing Then
    '        Try
    '            Me.ConservaRegistrazioneProtocollo()
    '            Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '        Catch ex As Exception

    '            Me.RegistraScriptPersistenzaVisibilitaPannello()
    '            Me.EnableUiHidden.Value = "Abilita"
    '            ParsecUtility.Utility.MessageBox("Impossibile conservare il protocollo per il seguente motivo: " & vbCrLf & ex.Message, False)
    '            ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
    '        End Try
    '        Exit Sub
    '    End If


    '    If Not creaIstanzaPratica Is Nothing Then

    '        'todo verificare istanzePratica.IdFascicolo
    '        Dim fascicoli As New ParsecAdmin.FascicoliRepository
    '        Dim fascicoliProtocollo = fascicoli.GetByIdDocumento(Me.TaskAttivo.IdDocumento, ParsecAdmin.TipoModulo.PRO)
    '        fascicoli.Dispose()
    '        If fascicoliProtocollo.Count = 0 Then
    '            ParsecUtility.Utility.MessageBox("E' necessario aggiungere il protocollo in un fascicolo!", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '            Exit Sub
    '        End If
    '        Try

    '            Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
    '            Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.IdRegistrazione = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '            If Not istanzePratica Is Nothing Then
    '                istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.Avviata
    '                istanzePraticaOnline.SaveChanges()
    '                istanzePraticaOnline.Dispose()


    '                Try
    '                    Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
    '                    Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
    '                    Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
    '                    Dim sp As New ParsecWebServices.StatoPraticaWS(istanzePratica.NumeroPratica, utenteCorrente.Username, ParsecAdmin.StatoIstanzaPraticaOnline.Avviata)
    '                    sp.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)
    '                    sp.Insert()

    '                Catch ex As Exception
    '                    'todo scrivere nel log
    '                End Try

    '                '*************************************************************************************************************
    '                'AGGIUNGO LA PRATICA CORRENTE AI DOCUMENTI DEL FASCICOLO ASSOCIATO ALL'ISTANZA ONLINE CORRENTE
    '                '*************************************************************************************************************
    '                fascicoli = New ParsecAdmin.FascicoliRepository
    '                Dim fascicolo = fascicoli.Where(Function(c) c.Codice = istanzePratica.CodiceFascicolo And c.Stato Is Nothing).FirstOrDefault



    '                If Not fascicolo Is Nothing Then
    '                    Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento
    '                    documentoFascicolo.IdDocumento = istanzePratica.Id
    '                    documentoFascicolo.NomeDocumento = "Istanza N° " & istanzePratica.NumeroPratica & " del " & String.Format("{0:dd/MM/yyyy}", istanzePratica.DataPresentazione)
    '                    documentoFascicolo.NomeDocumentoOriginale = documentoFascicolo.NomeDocumento
    '                    documentoFascicolo.TipoDocumento = ParsecAdmin.TipoModulo.IOL
    '                    documentoFascicolo.DescrizioneTipoDocumento = "IOL"
    '                    documentoFascicolo.DataDocumento = istanzePratica.DataPresentazione
    '                    'documentoFascicolo.NumeroDocumento = istanzePratica.NumeroPratica
    '                    documentoFascicolo.Definitivo = True
    '                    documentoFascicolo.Fase = String.Empty 'ParsecAdmin.FaseDocumentoEnumeration.INIZIALE
    '                    documentoFascicolo.DescrizioneFase = String.Empty '"Iniziale"
    '                    documentoFascicolo.Id = -1


    '                    documentoFascicolo.IdFascicolo = fascicolo.Id
    '                    documentoFascicolo.path = fascicolo.DataCreazioneFascicolo.Year.ToString & "\"


    '                    Dim documentiFascicolo As New ParsecAdmin.FascicoloDocumentoRepository
    '                    documentiFascicolo.Add(documentoFascicolo)
    '                    documentiFascicolo.SaveChanges()
    '                    documentiFascicolo.Dispose()


    '                End If
    '                fascicoli.Dispose()
    '                '*************************************************************************************************************

    '            Else
    '                ParsecUtility.Utility.MessageBox("Impossibile creare la pratica per il seguente motivo: " & vbCrLf & "La pratica associata al protocollo con id " & Me.TaskAttivo.IdDocumento & " non  è stata trovata!", False)
    '                Me.EnableUiHidden.Value = "Abilita"
    '                Exit Sub
    '            End If
    '            Me.AggiornaIstanzaWorkflow(istanzePratica)
    '            Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '            Exit Sub
    '        Catch ex As Exception
    '            ParsecUtility.Utility.MessageBox("Impossibile creare la pratica per il seguente motivo: " & vbCrLf & ex.Message, False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '            Exit Sub
    '        End Try

    '    End If

    '    If Not visualizzaIstanzaPratica Is Nothing Then
    '        Me.VisualizzaIstanzaPratica()
    '        Exit Sub
    '    End If



    '    '*************************************************************************************************************************************

    '    If Not avviaIterParametro Is Nothing Then

    '        Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '        Dim protocollo = registrazioni.GetById(Me.TaskAttivo.IdDocumento)
    '        registrazioni.Dispose()
    '        If Not protocollo Is Nothing Then
    '            'TERMINO L'ITER CORRENTE
    '            Me.TerminaTask()
    '            'AVVIO UN NUOVO ITER RELATIVO AL PROTOCOLLO CORRENTE
    '            Me.CreaIstanzaPro(protocollo)
    '            Me.RegistraButtonClick()
    '        Else
    '            ParsecUtility.Utility.MessageBox("Impossibile trovare il protocollo con id " & Me.TaskAttivo.IdDocumento & " !", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '        End If
    '        Exit Sub
    '    End If


    '    If Not conservaFatturaParametro Is Nothing Then

    '        Dim fatture As New ParsecPro.FatturaElettronicaRepository
    '        Dim fattura = fatture.Where(Function(c) c.IdRegistrazione = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '        If Not fattura Is Nothing Then
    '            Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '            Dim registrazione = registrazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '            If Not registrazione Is Nothing Then

    '                Try
    '                    Me.ConservaFattura(fattura, registrazione.NumeroProtocollo.ToString, registrazione.DataImmissione.Value)
    '                    fattura.IdStato = CInt(ParsecPro.StatoFattura.Convervata)
    '                    fatture.SaveChanges()
    '                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                End Try

    '            Else
    '                ParsecUtility.Utility.MessageBox("Protocollo non trovato!", False)
    '                Me.EnableUiHidden.Value = "Abilita"
    '            End If
    '        Else
    '            ParsecUtility.Utility.MessageBox("Fattura elettronica non trovata!", False)
    '            Me.EnableUiHidden.Value = "Abilita"

    '        End If

    '        Exit Sub
    '    End If



    '    If Not visualizzaFatturaParametro Is Nothing Then
    '        Me.VisualizzaFattura()
    '        Exit Sub
    '    End If


    '    If Me.Action.Parameters.Count = 0 Then
    '        Me.VisualizzaProtocolloInModifica()
    '        Exit Sub
    '    End If

    '    If Not inviaEmailParametro Is Nothing Then
    '        Me.VisualizzaEmailInModifica()
    '        Exit Sub
    '    End If

    '    If Not notificaEsitoFatturaParametro Is Nothing Then
    '        Me.VisualizzaImpostaEsito()
    '        Exit Sub
    '    End If

    '    Dim parametri As New ParsecAdmin.ParametriRepository
    '    Dim parametro = parametri.GetByName("TipoEsportazioneFattura", ParsecAdmin.TipoModulo.PRO)
    '    parametri.Dispose()

    '    Dim tipoEsportazione As String = "0"

    '    If Not parametro Is Nothing Then
    '        tipoEsportazione = parametro.Valore
    '    End If

    '    If Not esportaFatturaParametro Is Nothing Then

    '        Select Case tipoEsportazione
    '            'ESPORTAZIONE JSIBAC (0),   ESPORTAZIONE APSYSTEMS (2)
    '            Case "0", "2"

    '                ' Me.VisualizzaEsportaFattura()
    '                Dim usaFtpParametro = Me.Action.GetParameterByName("UsaFtp")

    '                If usaFtpParametro Is Nothing Then

    '                    Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '                    Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '                    Dim registrazione As ParsecPro.Registrazione = registrazioni.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault
    '                    registrazioni.Dispose()

    '                    If Not registrazione Is Nothing Then
    '                        Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
    '                        Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
    '                        If Not fattura Is Nothing Then
    '                            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.MessaggioSdI.Nomefile

    '                            If IO.File.Exists(localPath) Then
    '                                Session("AttachmentFullName") = localPath
    '                                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '                                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                                'Me.RegistraScriptPersistenzaVisibilitaPannello()
    '                                ParsecUtility.Utility.PageReload(pageUrl, False)
    '                            Else
    '                                ParsecUtility.Utility.MessageBox("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!", False)
    '                                Me.EnableUiHidden.Value = "Abilita"
    '                            End If
    '                        Else
    '                            ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                            Me.EnableUiHidden.Value = "Abilita"
    '                        End If
    '                    End If
    '                    Exit Sub
    '                Else


    '                    Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '                    Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '                    Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)

    '                    registrazioni.Dispose()

    '                    If Not registrazione Is Nothing Then
    '                        Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
    '                        Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
    '                        If Not fattura Is Nothing Then
    '                            Dim nomefile As String = fattura.MessaggioSdI.Nomefile
    '                            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefile
    '                            Dim localPathMetaDati As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.NomeFileMetadati
    '                            If IO.File.Exists(localPath) Then

    '                                Dim riga As String = String.Empty

    '                                Dim password = WebConfigSettings.GetKey("FatturaServerPwd")
    '                                password = ParsecCommon.CryptoUtil.Decrypt(password)
    '                                Dim user = WebConfigSettings.GetKey("FatturaServerUser")
    '                                Dim port = WebConfigSettings.GetKey("FatturaServerPort")
    '                                Dim host = WebConfigSettings.GetKey("FatturaServerUrl").Split(New Char() {"//"}, StringSplitOptions.RemoveEmptyEntries)(1)
    '                                Dim url As String = WebConfigSettings.GetKey("FatturaServerPath")

    '                                Dim pos As Integer = nomefile.IndexOf(".")
    '                                Dim nomeFileSenzaEstensione As String = nomefile.Substring(0, pos)

    '                                Dim emails As New ParsecPro.EmailArrivoRepository
    '                                Dim email = emails.Where(Function(c) c.Id = fattura.MessaggioSdI.IdEmail).FirstOrDefault

    '                                Dim uri As String = String.Format("ftp://{0}:{1}/{2}", host, port.ToString, url)
    '                                Try
    '                                    Me.UploadFile(user, password, uri, localPath)

    '                                    If IO.File.Exists(localPathMetaDati) Then
    '                                        Me.UploadFile(user, password, uri, localPathMetaDati)
    '                                    Else
    '                                        '*************************************************************************************************
    '                                        'ESTRAGGO IL FILE METADATI DALLA EMAIL
    '                                        '*************************************************************************************************
    '                                        If Not email Is Nothing Then
    '                                            Me.EstraiMetaDatiFatturaDaEmail(email, localPathMetaDati)
    '                                            If IO.File.Exists(localPathMetaDati) Then
    '                                                Me.UploadFile(user, password, uri, localPathMetaDati)
    '                                            End If
    '                                        End If
    '                                        '*************************************************************************************************
    '                                    End If


    '                                    If tipoEsportazione = "0" Then

    '                                        Dim destinatarioInterno = registrazione.Destinatari.Where(Function(c) c.Interno = True).FirstOrDefault

    '                                        Dim dataProtocollo As String = registrazione.DataImmissione.Value.ToShortDateString.Replace("/", "")
    '                                        Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
    '                                        Dim dataConsegnaPec As String = fattura.MessaggioSdI.DataRicezioneInvio.ToShortDateString.Replace("/", "")
    '                                        Dim utenteSdI As String = If(Not email Is Nothing, email.Mittente, "")
    '                                        Dim nomeFileFattura As String = fattura.MessaggioSdI.Nomefile
    '                                        Dim nomeFileMetaDatiFattura As String = fattura.NomeFileMetadati
    '                                        Dim ufficioBilancio As String = String.Empty

    '                                        If Not destinatarioInterno Is Nothing Then
    '                                            ufficioBilancio = destinatarioInterno.UfficioBilancio
    '                                        End If

    '                                        Dim statoFattura As String = "A"
    '                                        Dim identificativoSdI As String = fattura.IdentificativoSdI
    '                                        Dim codiceIPA As String = fattura.CodiceIpaDestinatario

    '                                        riga = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", dataProtocollo, numeroProtocollo, dataConsegnaPec, utenteSdI, nomeFileFattura, nomeFileMetaDatiFattura, ufficioBilancio, statoFattura, identificativoSdI, codiceIPA)


    '                                        nomefile = nomeFileSenzaEstensione & ".fec"

    '                                    Else

    '                                        nomefile = nomeFileSenzaEstensione & ".txt"
    '                                        Dim nomeFileFattura As String = nomeFileSenzaEstensione & ".xml"
    '                                        Dim dataProtocollo As String = String.Format("{0:yyyyMMdd}", registrazione.DataImmissione.Value)
    '                                        Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
    '                                        riga = String.Format("{0};{1};{2}", nomeFileFattura, dataProtocollo, numeroProtocollo)

    '                                    End If



    '                                    Dim buffer As Byte() = System.Text.Encoding.Default.GetBytes(riga)
    '                                    Me.UploadFile(user, password, uri, nomefile, buffer)

    '                                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                                    fattura.IdStato = CInt(ParsecPro.StatoFattura.Contabilizzata)
    '                                    fattureElettroniche.SaveChanges()

    '                                Catch ex As Exception
    '                                    ParsecUtility.Utility.MessageBox("Impossibile esportare la fattura per il seguente motivo: " & vbCrLf & ex.Message, False)
    '                                    Me.EnableUiHidden.Value = "Abilita"
    '                                    Exit Sub
    '                                End Try

    '                            Else
    '                                ParsecUtility.Utility.MessageBox("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!", False)
    '                                Me.EnableUiHidden.Value = "Abilita"
    '                            End If
    '                        Else
    '                            ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                            Me.EnableUiHidden.Value = "Abilita"
    '                        End If
    '                    Else
    '                        ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                        Me.EnableUiHidden.Value = "Abilita"
    '                    End If

    '                    Exit Sub

    '                End If

    '                'ESPORTAZIONE TINN
    '            Case "1"

    '                Try
    '                    Me.EseguiEsportazioneTINN()
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                    Exit Sub
    '                End Try
    '                Exit Sub

    '            Case "3"  'ESPORTAZIONE DEDAGROUP

    '                Try
    '                    Dim taskAttivi As New List(Of ParsecWKF.TaskAttivo)
    '                    taskAttivi.Add(Me.TaskAttivo)
    '                    Me.EseguiEsportazioneDedaGroup(taskAttivi)
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                    Exit Sub
    '                End Try
    '                Exit Sub


    '        End Select


    '    End If

    '    If Not protocollazioneParametro Is Nothing Then

    '        Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
    '        Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = TaskAttivo.IdIstanza AndAlso c.Stato Is Nothing).FirstOrDefault

    '        'Se il documento generico non è stato ancora generato
    '        If documentoGenerico Is Nothing Then
    '            ParsecUtility.Utility.MessageBox("Per protocollare è necessario generare il documento!", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '        Else


    '            Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, e.CommandName, Me.TaskAttivo.NomeFileIter)
    '            Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)

    '            If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna Then


    '                Dim mittenti As List(Of ParsecPro.Mittente) = registrazione.Mittenti
    '                Dim destinatari As List(Of ParsecPro.Destinatario) = registrazione.Destinatari
    '                'Rendo effettive le modifiche di inversione dei due gruppi di referenti
    '                registrazione.Mittenti = ParsecPro.Destinatari.ConvertiInMittenti(destinatari)
    '                registrazione.Destinatari = ParsecPro.Mittenti.ConvertiInDestinatari(mittenti)

    '                If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo Then
    '                    registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza
    '                Else
    '                    registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna
    '                    For Each dest In registrazione.Destinatari.Where(Function(c) c.Interno = True)
    '                        dest.Iter = True
    '                    Next
    '                End If

    '                registrazione.TipoRegistrazione = CInt(registrazione.TipologiaRegistrazione)
    '                registrazione.DataDocumento = Now

    '                '*****************************************************************************
    '                'Aggiungo il collegamento alla registrazione che sta per essere inserita
    '                '*****************************************************************************
    '                Dim collegamento As New ParsecPro.Collegamento
    '                collegamento.AnnoProtocollo2 = Year(registrazione.DataImmissione)
    '                collegamento.NumeroProtocollo2 = registrazione.NumeroProtocollo
    '                collegamento.TipoRegistrazione2 = registrazione.TipoRegistrazione



    '                collegamento.Diretto = True
    '                collegamento.Oggetto = registrazione.Oggetto
    '                collegamento.NumeroProtocollo = collegamento.GetNumeroProtocollo(registrazione.NumeroProtocollo, registrazione.TipoRegistrazione)

    '                Dim referenti As New StringBuilder
    '                Dim uffici As New StringBuilder


    '                For Each m As ParsecPro.Mittente In registrazione.Mittenti
    '                    If m.Interno Then
    '                        uffici.Append(m.Descrizione)
    '                    Else
    '                        referenti.Append(m.Descrizione)
    '                    End If
    '                Next

    '                For Each d As ParsecPro.Destinatario In registrazione.Destinatari
    '                    If d.Interno Then
    '                        uffici.Append(d.Descrizione)
    '                    Else
    '                        referenti.Append(d.Descrizione)
    '                    End If
    '                Next




    '                collegamento.Referenti = referenti.ToString
    '                collegamento.Uffici = uffici.ToString

    '                registrazione.NumeroProtocolloRiscontro = registrazione.NumeroProtocollo
    '                registrazione.DataImmissioneRiscontro = String.Format("{0:dd/MM/yyyy}", registrazione.DataImmissione)



    '                registrazione.Collegamenti.Clear()
    '                registrazione.Collegamenti.Add(collegamento)

    '                '*****************************************************************************
    '                'Copio l'allegato nella cartella temporanea.
    '                '*****************************************************************************
    '                Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
    '                Dim sorgenti As New ParsecAdmin.SorgentiRepository
    '                Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
    '                sorgenti.Dispose()
    '                Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
    '                Dim fileExists As Boolean = False

    '                Dim localPath As String = String.Empty

    '                Dim nomefile = documentoGenerico.NomeFile

    '                Select Case sorgenteCorrente.IdTipologia
    '                    Case 1 'Locale
    '                        localPath = source.Path & anno & "\" & nomefile
    '                        fileExists = IO.File.Exists(localPath)
    '                    Case 2  'Ftp
    '                        Dim relativePath As String = source.RelativePath & anno & "/" & nomefile
    '                        fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
    '                End Select

    '                If Not fileExists Then
    '                    ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
    '                    Exit Sub
    '                Else
    '                    Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
    '                    If Not IO.File.Exists(pathDownload) Then
    '                        IO.File.Copy(localPath, pathDownload)
    '                    End If

    '                    '*****************************************************************************
    '                    'Aggiungo l'allegato (documento generico) alla registrazione che sta per essere inserita
    '                    '*****************************************************************************

    '                    Dim allegato As New ParsecPro.Allegato
    '                    allegato.NomeFile = nomefile
    '                    allegato.NomeFileTemp = nomefile
    '                    allegato.IdTipologiaDocumento = 1 'Primario
    '                    allegato.DescrizioneTipologiaDocumento = "Primario"
    '                    allegato.Oggetto = "Documento di risposta"
    '                    allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
    '                    allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
    '                    allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
    '                    allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
    '                    registrazione.Allegati.Clear()
    '                    registrazione.Allegati.Add(allegato)
    '                    '*****************************************************************************
    '                End If



    '            End If

    '            Dim parametriPagina As New Hashtable
    '            parametriPagina.Add("RegistrazioneInIter", registrazione)


    '            Dim tipo As String = "1"
    '            Select Case protocollazioneParametro.Valore.ToUpper
    '                Case "INTERNO"
    '                    tipo = "2"
    '                    ' parametriPagina.Add("TaskAttivo", Me.TaskAttivo)
    '                Case "ARRIVO"
    '                    tipo = "0"
    '            End Select

    '            If Not disabilitaIterParametro Is Nothing Then
    '                parametriPagina.Add("DisabilitaIter", "1")
    '            End If


    '            Dim queryString As New Hashtable
    '            queryString.Add("Mode", "Insert")
    '            queryString.Add("Tipo", tipo) 'Partenza
    '            queryString.Add("obj", Me.AggiornaIterButton.ClientID)



    '            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    '            ParsecUtility.Utility.ShowPopup(pageUrl, 940, 600, queryString, False)
    '        End If

    '        Exit Sub
    '    End If



    '    If Not preparaDocumentoParametro Is Nothing Then

    '        Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
    '        Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault




    '        '***************************************************************************************
    '        'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
    '        '***************************************************************************************
    '        If Not documentoGenerico Is Nothing Then
    '            Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)

    '            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

    '            Dim visibile As Boolean = False

    '            If modello.VisibilitaPubblica.HasValue Then
    '                visibile = modello.VisibilitaPubblica
    '            End If

    '            If Not visibile Then
    '                visibile = (utenteCorrente.SuperUser OrElse documentoGenerico.IdUtente = utenteCorrente.Id)
    '            End If

    '            If visibile Then

    '                Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
    '                Dim sorgenti As New ParsecAdmin.SorgentiRepository
    '                Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
    '                sorgenti.Dispose()
    '                Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
    '                Dim fileExists As Boolean = False
    '                Select Case sorgenteCorrente.IdTipologia
    '                    Case 1 'Locale
    '                        Dim localPath As String = source.Path & anno & "\" & documentoGenerico.NomeFile
    '                        fileExists = IO.File.Exists(localPath)
    '                    Case 2  'Ftp
    '                        Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
    '                        fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
    '                End Select

    '                If Not fileExists Then
    '                    ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
    '                    Exit Sub
    '                Else
    '                    If documentoGenerico.GeneratoSistemaSEP Then
    '                        Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, True, source)
    '                    Else
    '                        Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, source)
    '                    End If


    '                    ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

    '                End If
    '            Else
    '                ParsecUtility.Utility.MessageBox("Non si possiedono le autorizzazioni per visualizzare il documento!", False)
    '            End If

    '        Else

    '            '***************************************************************************************
    '            'Apro la pagina per la generazione del documento generico.
    '            'Il salvattaggio esegue il click sul AggiornaIterButton  (persistenza stato del processo corrente)
    '            '***************************************************************************************

    '            Me.VisualizzaDocumentoGenericoInModifica()
    '        End If

    '        documentiGenerici.Dispose()

    '        Exit Sub
    '    End If
    'End Sub

    'Private Sub ModificaPRO(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

    '    '*************************************************************************************************************************************
    '    'Leggo tutti i parametri 
    '    '*************************************************************************************************************************************

    '    Dim preparaDocumentoParametro = Me.Action.GetParameterByName("PreparaDocumento")
    '    'Dim riferimentDocumentoParametro = Me.Action.GetParameterByName("RiferimentoDocumento") 'OUT
    '    Dim protocollazioneParametro = Me.Action.GetParameterByName("Protocollazione")
    '    Dim inviaEmailParametro = Me.Action.GetParameterByName("InviaEmail")

    '    Dim notificaEsitoFatturaParametro = Me.Action.GetParameterByName("NotificaEsitoFattura")
    '    Dim esportaFatturaParametro = Me.Action.GetParameterByName("EsportaFattura")

    '    Dim visualizzaFatturaParametro = Me.Action.GetParameterByName("VisualizzaFattura")
    '    Dim conservaFatturaParametro = Me.Action.GetParameterByName("Conserva")
    '    Dim avviaIterParametro = Me.Action.GetParameterByName("AvviaIter")
    '    Dim disabilitaIterParametro = Me.Action.GetParameterByName("DisabilitaIter")

    '    Dim creaIstanzaPratica = Me.Action.GetParameterByName("CreaIstanzaPratica")
    '    Dim visualizzaIstanzaPratica = Me.Action.GetParameterByName("VisualizzaIstanzaPratica")

    '    Dim conservaProtocolloParametro = Me.Action.GetParameterByName("ConservaProtocollo")


    '    If Not conservaProtocolloParametro Is Nothing Then
    '        Try
    '            Me.ConservaRegistrazioneProtocollo()
    '            Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '        Catch ex As Exception

    '            Me.RegistraScriptPersistenzaVisibilitaPannello()
    '            Me.EnableUiHidden.Value = "Abilita"
    '            ParsecUtility.Utility.MessageBox("Impossibile conservare il protocollo per il seguente motivo: " & vbCrLf & ex.Message, False)
    '            ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
    '        End Try
    '        Exit Sub
    '    End If


    '    If Not creaIstanzaPratica Is Nothing Then

    '        'todo verificare istanzePratica.IdFascicolo
    '        Dim fascicoli As New ParsecAdmin.FascicoliRepository
    '        Dim fascicoliProtocollo = fascicoli.GetByIdDocumento(Me.TaskAttivo.IdDocumento, ParsecAdmin.TipoModulo.PRO)
    '        fascicoli.Dispose()
    '        If fascicoliProtocollo.Count = 0 Then
    '            ParsecUtility.Utility.MessageBox("E' necessario aggiungere il protocollo in un fascicolo!", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '            Exit Sub
    '        End If
    '        Try

    '            Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
    '            Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.IdRegistrazione = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '            If Not istanzePratica Is Nothing Then
    '                istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.Avviata
    '                istanzePraticaOnline.SaveChanges()
    '                istanzePraticaOnline.Dispose()


    '                Try
    '                    Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
    '                    Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
    '                    Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
    '                    Dim sp As New ParsecWebServices.StatoPraticaWS(istanzePratica.NumeroPratica, utenteCorrente.Username, ParsecAdmin.StatoIstanzaPraticaOnline.Avviata)
    '                    sp.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)
    '                    sp.Insert()

    '                Catch ex As Exception
    '                    'todo scrivere nel log
    '                End Try

    '                '*************************************************************************************************************
    '                'AGGIUNGO LA PRATICA CORRENTE AI DOCUMENTI DEL FASCICOLO ASSOCIATO ALL'ISTANZA ONLINE CORRENTE
    '                '*************************************************************************************************************
    '                fascicoli = New ParsecAdmin.FascicoliRepository
    '                Dim fascicolo = fascicoli.Where(Function(c) c.Codice = istanzePratica.CodiceFascicolo And c.Stato Is Nothing).FirstOrDefault



    '                If Not fascicolo Is Nothing Then
    '                    Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento
    '                    documentoFascicolo.IdDocumento = istanzePratica.Id
    '                    documentoFascicolo.NomeDocumento = "Istanza N° " & istanzePratica.NumeroPratica & " del " & String.Format("{0:dd/MM/yyyy}", istanzePratica.DataPresentazione)
    '                    documentoFascicolo.NomeDocumentoOriginale = documentoFascicolo.NomeDocumento
    '                    documentoFascicolo.TipoDocumento = ParsecAdmin.TipoModulo.IOL
    '                    documentoFascicolo.DescrizioneTipoDocumento = "IOL"
    '                    documentoFascicolo.DataDocumento = istanzePratica.DataPresentazione
    '                    'documentoFascicolo.NumeroDocumento = istanzePratica.NumeroPratica
    '                    documentoFascicolo.Definitivo = True
    '                    documentoFascicolo.Fase = String.Empty 'ParsecAdmin.FaseDocumentoEnumeration.INIZIALE
    '                    documentoFascicolo.DescrizioneFase = String.Empty '"Iniziale"
    '                    documentoFascicolo.Id = -1


    '                    documentoFascicolo.IdFascicolo = fascicolo.Id
    '                    documentoFascicolo.path = fascicolo.DataCreazioneFascicolo.Year.ToString & "\"


    '                    Dim documentiFascicolo As New ParsecAdmin.FascicoloDocumentoRepository
    '                    documentiFascicolo.Add(documentoFascicolo)
    '                    documentiFascicolo.SaveChanges()
    '                    documentiFascicolo.Dispose()


    '                End If
    '                fascicoli.Dispose()
    '                '*************************************************************************************************************

    '            Else
    '                istanzePraticaOnline.Dispose()
    '                ParsecUtility.Utility.MessageBox("Impossibile creare la pratica per il seguente motivo: " & vbCrLf & "La pratica associata al protocollo con id " & Me.TaskAttivo.IdDocumento & " non  è stata trovata!", False)
    '                Me.EnableUiHidden.Value = "Abilita"
    '                Exit Sub
    '            End If
    '            Me.AggiornaIstanzaWorkflow(istanzePratica)
    '            Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '            Exit Sub
    '        Catch ex As Exception
    '            ParsecUtility.Utility.MessageBox("Impossibile creare la pratica per il seguente motivo: " & vbCrLf & ex.Message, False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '            Exit Sub
    '        End Try

    '    End If

    '    If Not visualizzaIstanzaPratica Is Nothing Then
    '        Me.VisualizzaIstanzaPratica()
    '        Exit Sub
    '    End If



    '    '*************************************************************************************************************************************

    '    If Not avviaIterParametro Is Nothing Then

    '        Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '        Dim protocollo = registrazioni.GetById(Me.TaskAttivo.IdDocumento)
    '        registrazioni.Dispose()
    '        If Not protocollo Is Nothing Then
    '            'TERMINO L'ITER CORRENTE
    '            Me.TerminaTask()
    '            'AVVIO UN NUOVO ITER RELATIVO AL PROTOCOLLO CORRENTE
    '            Me.CreaIstanzaPro(protocollo)
    '            Me.RegistraButtonClick()
    '        Else
    '            ParsecUtility.Utility.MessageBox("Impossibile trovare il protocollo con id " & Me.TaskAttivo.IdDocumento & " !", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '        End If
    '        Exit Sub
    '    End If


    '    If Not conservaFatturaParametro Is Nothing Then

    '        Dim fatture As New ParsecPro.FatturaElettronicaRepository
    '        Dim fattura = fatture.Where(Function(c) c.IdRegistrazione = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '        If Not fattura Is Nothing Then
    '            Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '            Dim registrazione = registrazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '            If Not registrazione Is Nothing Then

    '                Try
    '                    Me.ConservaFattura(fattura, registrazione.NumeroProtocollo.ToString, registrazione.DataImmissione.Value)
    '                    fattura.IdStato = CInt(ParsecPro.StatoFattura.Convervata)
    '                    fatture.SaveChanges()
    '                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                End Try

    '            Else
    '                ParsecUtility.Utility.MessageBox("Protocollo non trovato!", False)
    '                Me.EnableUiHidden.Value = "Abilita"
    '            End If
    '        Else
    '            ParsecUtility.Utility.MessageBox("Fattura elettronica non trovata!", False)
    '            Me.EnableUiHidden.Value = "Abilita"

    '        End If
    '        fatture.Dispose()
    '        Exit Sub
    '    End If



    '    If Not visualizzaFatturaParametro Is Nothing Then
    '        Me.VisualizzaFattura()
    '        Exit Sub
    '    End If


    '    If Me.Action.Parameters.Count = 0 Then
    '        Me.VisualizzaProtocolloInModifica()
    '        Exit Sub
    '    End If

    '    If Not inviaEmailParametro Is Nothing Then
    '        Me.VisualizzaEmailInModifica()
    '        Exit Sub
    '    End If

    '    If Not notificaEsitoFatturaParametro Is Nothing Then
    '        Me.VisualizzaImpostaEsito()
    '        Exit Sub
    '    End If

    '    Dim parametri As New ParsecAdmin.ParametriRepository
    '    Dim parametro = parametri.GetByName("TipoEsportazioneFattura", ParsecAdmin.TipoModulo.PRO)
    '    parametri.Dispose()

    '    Dim tipoEsportazione As String = "0"

    '    If Not parametro Is Nothing Then
    '        tipoEsportazione = parametro.Valore
    '    End If

    '    If Not esportaFatturaParametro Is Nothing Then

    '        Select Case tipoEsportazione
    '            'ESPORTAZIONE JSIBAC (0),   ESPORTAZIONE APSYSTEMS (2)
    '            Case "0", "2"

    '                ' Me.VisualizzaEsportaFattura()
    '                Dim usaFtpParametro = Me.Action.GetParameterByName("UsaFtp")

    '                If usaFtpParametro Is Nothing Then

    '                    Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '                    Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '                    Dim registrazione As ParsecPro.Registrazione = registrazioni.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault
    '                    registrazioni.Dispose()

    '                    If Not registrazione Is Nothing Then
    '                        Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
    '                        Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
    '                        If Not fattura Is Nothing Then
    '                            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.MessaggioSdI.Nomefile

    '                            If IO.File.Exists(localPath) Then
    '                                Session("AttachmentFullName") = localPath
    '                                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '                                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                                'Me.RegistraScriptPersistenzaVisibilitaPannello()
    '                                ParsecUtility.Utility.PageReload(pageUrl, False)
    '                            Else
    '                                ParsecUtility.Utility.MessageBox("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!", False)
    '                                Me.EnableUiHidden.Value = "Abilita"
    '                            End If
    '                        Else
    '                            ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                            Me.EnableUiHidden.Value = "Abilita"
    '                        End If
    '                        fattureElettroniche.Dispose()
    '                    End If
    '                    Exit Sub
    '                Else


    '                    Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '                    Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '                    Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)

    '                    registrazioni.Dispose()

    '                    If Not registrazione Is Nothing Then
    '                        Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
    '                        Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
    '                        If Not fattura Is Nothing Then
    '                            Dim nomefile As String = fattura.MessaggioSdI.Nomefile
    '                            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefile
    '                            Dim localPathMetaDati As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.NomeFileMetadati
    '                            If IO.File.Exists(localPath) Then

    '                                Dim riga As String = String.Empty

    '                                Dim password = WebConfigSettings.GetKey("FatturaServerPwd")
    '                                password = ParsecCommon.CryptoUtil.Decrypt(password)
    '                                Dim user = WebConfigSettings.GetKey("FatturaServerUser")
    '                                Dim port = WebConfigSettings.GetKey("FatturaServerPort")
    '                                Dim host = WebConfigSettings.GetKey("FatturaServerUrl").Split(New Char() {"//"}, StringSplitOptions.RemoveEmptyEntries)(1)
    '                                Dim url As String = WebConfigSettings.GetKey("FatturaServerPath")

    '                                Dim pos As Integer = nomefile.IndexOf(".")
    '                                Dim nomeFileSenzaEstensione As String = nomefile.Substring(0, pos)

    '                                Dim emails As New ParsecPro.EmailArrivoRepository
    '                                Dim email = emails.Where(Function(c) c.Id = fattura.MessaggioSdI.IdEmail).FirstOrDefault

    '                                Dim uri As String = String.Format("ftp://{0}:{1}/{2}", host, port.ToString, url)
    '                                Try
    '                                    Me.UploadFile(user, password, uri, localPath)

    '                                    If IO.File.Exists(localPathMetaDati) Then
    '                                        Me.UploadFile(user, password, uri, localPathMetaDati)
    '                                    Else
    '                                        '*************************************************************************************************
    '                                        'ESTRAGGO IL FILE METADATI DALLA EMAIL
    '                                        '*************************************************************************************************
    '                                        If Not email Is Nothing Then
    '                                            Me.EstraiMetaDatiFatturaDaEmail(email, localPathMetaDati)
    '                                            If IO.File.Exists(localPathMetaDati) Then
    '                                                Me.UploadFile(user, password, uri, localPathMetaDati)
    '                                            End If
    '                                        End If
    '                                        '*************************************************************************************************
    '                                    End If


    '                                    If tipoEsportazione = "0" Then

    '                                        Dim destinatarioInterno = registrazione.Destinatari.Where(Function(c) c.Interno = True).FirstOrDefault

    '                                        Dim dataProtocollo As String = registrazione.DataImmissione.Value.ToShortDateString.Replace("/", "")
    '                                        Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
    '                                        Dim dataConsegnaPec As String = fattura.MessaggioSdI.DataRicezioneInvio.ToShortDateString.Replace("/", "")
    '                                        Dim utenteSdI As String = If(Not email Is Nothing, email.Mittente, "")
    '                                        Dim nomeFileFattura As String = fattura.MessaggioSdI.Nomefile
    '                                        Dim nomeFileMetaDatiFattura As String = fattura.NomeFileMetadati
    '                                        Dim ufficioBilancio As String = String.Empty

    '                                        If Not destinatarioInterno Is Nothing Then
    '                                            ufficioBilancio = destinatarioInterno.UfficioBilancio
    '                                        End If

    '                                        Dim statoFattura As String = "A"
    '                                        Dim identificativoSdI As String = fattura.IdentificativoSdI
    '                                        Dim codiceIPA As String = fattura.CodiceIpaDestinatario

    '                                        riga = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", dataProtocollo, numeroProtocollo, dataConsegnaPec, utenteSdI, nomeFileFattura, nomeFileMetaDatiFattura, ufficioBilancio, statoFattura, identificativoSdI, codiceIPA)


    '                                        nomefile = nomeFileSenzaEstensione & ".fec"

    '                                    Else

    '                                        nomefile = nomeFileSenzaEstensione & ".txt"
    '                                        Dim nomeFileFattura As String = nomeFileSenzaEstensione & ".xml"
    '                                        Dim dataProtocollo As String = String.Format("{0:yyyyMMdd}", registrazione.DataImmissione.Value)
    '                                        Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
    '                                        riga = String.Format("{0};{1};{2}", nomeFileFattura, dataProtocollo, numeroProtocollo)

    '                                    End If



    '                                    Dim buffer As Byte() = System.Text.Encoding.Default.GetBytes(riga)
    '                                    Me.UploadFile(user, password, uri, nomefile, buffer)

    '                                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                                    fattura.IdStato = CInt(ParsecPro.StatoFattura.Contabilizzata)
    '                                    fattureElettroniche.SaveChanges()

    '                                Catch ex As Exception
    '                                    ParsecUtility.Utility.MessageBox("Impossibile esportare la fattura per il seguente motivo: " & vbCrLf & ex.Message, False)
    '                                    Me.EnableUiHidden.Value = "Abilita"
    '                                    Exit Sub
    '                                End Try

    '                            Else
    '                                ParsecUtility.Utility.MessageBox("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!", False)
    '                                Me.EnableUiHidden.Value = "Abilita"
    '                            End If
    '                        Else
    '                            fattureElettroniche.Dispose()
    '                            ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                            Me.EnableUiHidden.Value = "Abilita"
    '                        End If
    '                    Else
    '                        ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                        Me.EnableUiHidden.Value = "Abilita"
    '                    End If

    '                    Exit Sub

    '                End If

    '                'ESPORTAZIONE TINN
    '            Case "1"

    '                Try
    '                    Me.EseguiEsportazioneTINN()
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                    Exit Sub
    '                End Try
    '                Exit Sub

    '            Case "3"  'ESPORTAZIONE DEDAGROUP

    '                Try
    '                    Dim taskAttivi As New List(Of ParsecWKF.TaskAttivo)
    '                    taskAttivi.Add(Me.TaskAttivo)
    '                    Me.EseguiEsportazioneDedaGroup(taskAttivi)
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                    Exit Sub
    '                End Try
    '                Exit Sub


    '        End Select


    '    End If

    '    If Not protocollazioneParametro Is Nothing Then

    '        Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
    '        Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = TaskAttivo.IdIstanza AndAlso c.Stato Is Nothing).FirstOrDefault
    '        documentiGenerici.Dispose()

    '        'Se il documento generico non è stato ancora generato
    '        If documentoGenerico Is Nothing Then
    '            ParsecUtility.Utility.MessageBox("Per protocollare è necessario generare il documento!", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '        Else


    '            Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, e.CommandName, Me.TaskAttivo.NomeFileIter)
    '            Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
    '            registrazioni.Dispose()

    '            'If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna Then

    '            If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza Then
    '                Dim mittenti As List(Of ParsecPro.Mittente) = registrazione.Mittenti
    '                Dim destinatari As List(Of ParsecPro.Destinatario) = registrazione.Destinatari
    '                'Rendo effettive le modifiche di inversione dei due gruppi di referenti
    '                registrazione.Mittenti = ParsecPro.Destinatari.ConvertiInMittenti(destinatari)
    '                registrazione.Destinatari = ParsecPro.Mittenti.ConvertiInDestinatari(mittenti)

    '                If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo Then
    '                    registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza
    '                Else
    '                    registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna
    '                    For Each dest In registrazione.Destinatari.Where(Function(c) c.Interno = True)
    '                        dest.Iter = True
    '                    Next
    '                End If

    '                Dim tipoRegistrazione As Integer = registrazione.TipoRegistrazione
    '                registrazione.TipoRegistrazione = CInt(registrazione.TipologiaRegistrazione)
    '                registrazione.DataDocumento = Now

    '                '*****************************************************************************
    '                'Aggiungo il collegamento alla registrazione che sta per essere inserita
    '                '*****************************************************************************
    '                Dim collegamento As New ParsecPro.Collegamento
    '                collegamento.AnnoProtocollo2 = Year(registrazione.DataImmissione)
    '                collegamento.NumeroProtocollo2 = registrazione.NumeroProtocollo
    '                collegamento.TipoRegistrazione2 = registrazione.TipoRegistrazione



    '                collegamento.Diretto = True
    '                collegamento.Oggetto = registrazione.Oggetto
    '                collegamento.NumeroProtocollo = collegamento.GetNumeroProtocollo(registrazione.NumeroProtocollo, tipoRegistrazione)

    '                Dim referenti As New StringBuilder
    '                Dim uffici As New StringBuilder


    '                For Each m As ParsecPro.Mittente In registrazione.Mittenti
    '                    If m.Interno Then
    '                        uffici.Append(m.Descrizione)
    '                    Else
    '                        referenti.Append(m.Descrizione)
    '                    End If
    '                Next

    '                For Each d As ParsecPro.Destinatario In registrazione.Destinatari
    '                    If d.Interno Then
    '                        uffici.Append(d.Descrizione)
    '                    Else
    '                        referenti.Append(d.Descrizione)
    '                    End If
    '                Next




    '                collegamento.Referenti = referenti.ToString
    '                collegamento.Uffici = uffici.ToString

    '                registrazione.NumeroProtocolloRiscontro = registrazione.NumeroProtocollo
    '                registrazione.DataImmissioneRiscontro = String.Format("{0:dd/MM/yyyy}", registrazione.DataImmissione)



    '                registrazione.Collegamenti.Clear()
    '                registrazione.Collegamenti.Add(collegamento)

    '                '*****************************************************************************
    '                'Copio l'allegato nella cartella temporanea.
    '                '*****************************************************************************
    '                Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
    '                Dim sorgenti As New ParsecAdmin.SorgentiRepository
    '                Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
    '                sorgenti.Dispose()
    '                Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
    '                Dim fileExists As Boolean = False

    '                Dim localPath As String = String.Empty

    '                Dim nomefile = documentoGenerico.NomeFile

    '                Select Case sorgenteCorrente.IdTipologia
    '                    Case 1 'Locale
    '                        localPath = source.Path & anno & "\" & nomefile
    '                        fileExists = IO.File.Exists(localPath)
    '                    Case 2  'Ftp
    '                        Dim relativePath As String = source.RelativePath & anno & "/" & nomefile
    '                        fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
    '                End Select

    '                If Not fileExists Then
    '                    ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
    '                    Exit Sub
    '                Else
    '                    Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
    '                    If Not IO.File.Exists(pathDownload) Then
    '                        IO.File.Copy(localPath, pathDownload)
    '                    End If

    '                    '*****************************************************************************
    '                    'Aggiungo l'allegato (documento generico) alla registrazione che sta per essere inserita
    '                    '*****************************************************************************

    '                    Dim allegato As New ParsecPro.Allegato
    '                    allegato.NomeFile = nomefile
    '                    allegato.NomeFileTemp = nomefile
    '                    allegato.IdTipologiaDocumento = 1 'Primario
    '                    allegato.DescrizioneTipologiaDocumento = "Primario"
    '                    allegato.Oggetto = "Documento di risposta"
    '                    allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
    '                    allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
    '                    allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
    '                    allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
    '                    registrazione.Allegati.Clear()
    '                    registrazione.Allegati.Add(allegato)
    '                    '*****************************************************************************
    '                End If



    '            End If

    '            Dim parametriPagina As New Hashtable
    '            parametriPagina.Add("RegistrazioneInIter", registrazione)


    '            Dim tipo As String = "1"
    '            Select Case protocollazioneParametro.Valore.ToUpper
    '                Case "INTERNO"
    '                    tipo = "2"
    '                    ' parametriPagina.Add("TaskAttivo", Me.TaskAttivo)
    '                Case "ARRIVO"
    '                    tipo = "0"
    '            End Select

    '            If Not disabilitaIterParametro Is Nothing Then
    '                parametriPagina.Add("DisabilitaIter", "1")
    '            End If


    '            Dim queryString As New Hashtable
    '            queryString.Add("Mode", "Insert")
    '            queryString.Add("Tipo", tipo) 'Partenza
    '            queryString.Add("obj", Me.AggiornaIterButton.ClientID)



    '            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    '            ParsecUtility.Utility.ShowPopup(pageUrl, 940, 600, queryString, False)
    '        End If

    '        Exit Sub
    '    End If



    '    If Not preparaDocumentoParametro Is Nothing Then

    '        Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
    '        Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault



    '        '***************************************************************************************
    '        'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
    '        '***************************************************************************************
    '        If Not documentoGenerico Is Nothing Then
    '            Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)

    '            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

    '            Dim visibile As Boolean = False

    '            If modello.VisibilitaPubblica.HasValue Then
    '                visibile = modello.VisibilitaPubblica
    '            End If

    '            If Not visibile Then
    '                visibile = (utenteCorrente.SuperUser OrElse documentoGenerico.IdUtente = utenteCorrente.Id)
    '            End If

    '            If visibile Then

    '                Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
    '                Dim sorgenti As New ParsecAdmin.SorgentiRepository
    '                Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
    '                sorgenti.Dispose()
    '                Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
    '                Dim fileExists As Boolean = False
    '                Select Case sorgenteCorrente.IdTipologia
    '                    Case 1 'Locale
    '                        Dim localPath As String = source.Path & anno & "\" & documentoGenerico.NomeFile
    '                        fileExists = IO.File.Exists(localPath)
    '                    Case 2  'Ftp
    '                        Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
    '                        fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
    '                End Select

    '                If Not fileExists Then
    '                    ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
    '                    Exit Sub
    '                Else
    '                    If documentoGenerico.GeneratoSistemaSEP Then
    '                        Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, True, source)
    '                    Else
    '                        Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, source)
    '                    End If


    '                    ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

    '                End If
    '            Else
    '                ParsecUtility.Utility.MessageBox("Non si possiedono le autorizzazioni per visualizzare il documento!", False)
    '            End If

    '        Else

    '            '***************************************************************************************
    '            'Apro la pagina per la generazione del documento generico.
    '            'Il salvattaggio esegue il click sul AggiornaIterButton  (persistenza stato del processo corrente)
    '            '***************************************************************************************

    '            Me.VisualizzaDocumentoGenericoInModifica()
    '        End If

    '        documentiGenerici.Dispose()

    '        Exit Sub
    '    End If

    'End Sub


    'Private Sub ModificaPRO(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

    '    '*************************************************************************************************************************************
    '    'Leggo tutti i parametri 
    '    '*************************************************************************************************************************************

    '    Dim preparaDocumentoParametro = Me.Action.GetParameterByName("PreparaDocumento")
    '    'Dim riferimentDocumentoParametro = Me.Action.GetParameterByName("RiferimentoDocumento") 'OUT
    '    Dim protocollazioneParametro = Me.Action.GetParameterByName("Protocollazione")
    '    Dim inviaEmailParametro = Me.Action.GetParameterByName("InviaEmail")

    '    Dim notificaEsitoFatturaParametro = Me.Action.GetParameterByName("NotificaEsitoFattura")
    '    Dim esportaFatturaParametro = Me.Action.GetParameterByName("EsportaFattura")

    '    Dim visualizzaFatturaParametro = Me.Action.GetParameterByName("VisualizzaFattura")
    '    Dim conservaFatturaParametro = Me.Action.GetParameterByName("Conserva")
    '    Dim avviaIterParametro = Me.Action.GetParameterByName("AvviaIter")
    '    Dim disabilitaIterParametro = Me.Action.GetParameterByName("DisabilitaIter")

    '    Dim creaIstanzaPratica = Me.Action.GetParameterByName("CreaIstanzaPratica")
    '    Dim visualizzaIstanzaPratica = Me.Action.GetParameterByName("VisualizzaIstanzaPratica")

    '    Dim conservaProtocolloParametro = Me.Action.GetParameterByName("ConservaProtocollo")


    '    If Not conservaProtocolloParametro Is Nothing Then
    '        Try
    '            Me.ConservaRegistrazioneProtocollo()
    '            Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '        Catch ex As Exception

    '            Me.RegistraScriptPersistenzaVisibilitaPannello()
    '            Me.EnableUiHidden.Value = "Abilita"
    '            ParsecUtility.Utility.MessageBox("Impossibile conservare il protocollo per il seguente motivo: " & vbCrLf & ex.Message, False)
    '            ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
    '        End Try
    '        Exit Sub
    '    End If


    '    If Not creaIstanzaPratica Is Nothing Then

    '        'todo verificare istanzePratica.IdFascicolo
    '        Dim fascicoli As New ParsecAdmin.FascicoliRepository
    '        Dim fascicoliProtocollo = fascicoli.GetByIdDocumento(Me.TaskAttivo.IdDocumento, ParsecAdmin.TipoModulo.PRO)
    '        fascicoli.Dispose()
    '        If fascicoliProtocollo.Count = 0 Then
    '            ParsecUtility.Utility.MessageBox("E' necessario aggiungere il protocollo in un fascicolo!", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '            Exit Sub
    '        End If
    '        Try

    '            Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
    '            Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.IdRegistrazione = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '            If Not istanzePratica Is Nothing Then
    '                istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.Avviata
    '                istanzePraticaOnline.SaveChanges()
    '                istanzePraticaOnline.Dispose()


    '                Try
    '                    Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
    '                    Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
    '                    Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
    '                    Dim sp As New ParsecWebServices.StatoPraticaWS(istanzePratica.NumeroPratica, utenteCorrente.Username, ParsecAdmin.StatoIstanzaPraticaOnline.Avviata)
    '                    sp.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)
    '                    sp.Insert()

    '                Catch ex As Exception
    '                    'todo scrivere nel log
    '                End Try

    '                '*************************************************************************************************************
    '                'AGGIUNGO LA PRATICA CORRENTE AI DOCUMENTI DEL FASCICOLO ASSOCIATO ALL'ISTANZA ONLINE CORRENTE
    '                '*************************************************************************************************************
    '                fascicoli = New ParsecAdmin.FascicoliRepository
    '                Dim fascicolo = fascicoli.Where(Function(c) c.Codice = istanzePratica.CodiceFascicolo And c.Stato Is Nothing).FirstOrDefault



    '                If Not fascicolo Is Nothing Then
    '                    Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento
    '                    documentoFascicolo.IdDocumento = istanzePratica.Id
    '                    documentoFascicolo.NomeDocumento = "Istanza N° " & istanzePratica.NumeroPratica & " del " & String.Format("{0:dd/MM/yyyy}", istanzePratica.DataPresentazione)
    '                    documentoFascicolo.NomeDocumentoOriginale = documentoFascicolo.NomeDocumento
    '                    documentoFascicolo.TipoDocumento = ParsecAdmin.TipoModulo.IOL
    '                    documentoFascicolo.DescrizioneTipoDocumento = "IOL"
    '                    documentoFascicolo.DataDocumento = istanzePratica.DataPresentazione
    '                    'documentoFascicolo.NumeroDocumento = istanzePratica.NumeroPratica
    '                    documentoFascicolo.Definitivo = True
    '                    documentoFascicolo.Fase = String.Empty 'ParsecAdmin.FaseDocumentoEnumeration.INIZIALE
    '                    documentoFascicolo.DescrizioneFase = String.Empty '"Iniziale"
    '                    documentoFascicolo.Id = -1


    '                    documentoFascicolo.IdFascicolo = fascicolo.Id
    '                    documentoFascicolo.path = fascicolo.DataCreazioneFascicolo.Year.ToString & "\"


    '                    Dim documentiFascicolo As New ParsecAdmin.FascicoloDocumentoRepository
    '                    documentiFascicolo.Add(documentoFascicolo)
    '                    documentiFascicolo.SaveChanges()
    '                    documentiFascicolo.Dispose()


    '                End If
    '                fascicoli.Dispose()
    '                '*************************************************************************************************************

    '            Else
    '                istanzePraticaOnline.Dispose()
    '                ParsecUtility.Utility.MessageBox("Impossibile creare la pratica per il seguente motivo: " & vbCrLf & "La pratica associata al protocollo con id " & Me.TaskAttivo.IdDocumento & " non  è stata trovata!", False)
    '                Me.EnableUiHidden.Value = "Abilita"
    '                Exit Sub
    '            End If
    '            Me.AggiornaIstanzaWorkflow(istanzePratica)
    '            Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '            Exit Sub
    '        Catch ex As Exception
    '            ParsecUtility.Utility.MessageBox("Impossibile creare la pratica per il seguente motivo: " & vbCrLf & ex.Message, False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '            Exit Sub
    '        End Try

    '    End If

    '    If Not visualizzaIstanzaPratica Is Nothing Then
    '        Me.VisualizzaIstanzaPratica()
    '        Exit Sub
    '    End If



    '    '*************************************************************************************************************************************

    '    If Not avviaIterParametro Is Nothing Then

    '        Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '        Dim protocollo = registrazioni.GetById(Me.TaskAttivo.IdDocumento)
    '        registrazioni.Dispose()
    '        If Not protocollo Is Nothing Then
    '            'TERMINO L'ITER CORRENTE
    '            Me.TerminaTask()
    '            'AVVIO UN NUOVO ITER RELATIVO AL PROTOCOLLO CORRENTE
    '            Me.CreaIstanzaPro(protocollo)
    '            Me.RegistraButtonClick()
    '        Else
    '            ParsecUtility.Utility.MessageBox("Impossibile trovare il protocollo con id " & Me.TaskAttivo.IdDocumento & " !", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '        End If
    '        Exit Sub
    '    End If


    '    If Not conservaFatturaParametro Is Nothing Then

    '        Dim fatture As New ParsecPro.FatturaElettronicaRepository
    '        Dim fattura = fatture.Where(Function(c) c.IdRegistrazione = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '        If Not fattura Is Nothing Then
    '            Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '            Dim registrazione = registrazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
    '            If Not registrazione Is Nothing Then

    '                Try
    '                    Me.ConservaFattura(fattura, registrazione.NumeroProtocollo.ToString, registrazione.DataImmissione.Value)
    '                    fattura.IdStato = CInt(ParsecPro.StatoFattura.Convervata)
    '                    fatture.SaveChanges()
    '                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                End Try

    '            Else
    '                ParsecUtility.Utility.MessageBox("Protocollo non trovato!", False)
    '                Me.EnableUiHidden.Value = "Abilita"
    '            End If
    '        Else
    '            ParsecUtility.Utility.MessageBox("Fattura elettronica non trovata!", False)
    '            Me.EnableUiHidden.Value = "Abilita"

    '        End If
    '        fatture.Dispose()
    '        Exit Sub
    '    End If



    '    If Not visualizzaFatturaParametro Is Nothing Then
    '        Me.VisualizzaFattura()
    '        Exit Sub
    '    End If


    '    If Me.Action.Parameters.Count = 0 Then
    '        Me.VisualizzaProtocolloInModifica()
    '        Exit Sub
    '    End If

    '    If Not inviaEmailParametro Is Nothing Then
    '        Me.VisualizzaEmailInModifica()
    '        Exit Sub
    '    End If

    '    If Not notificaEsitoFatturaParametro Is Nothing Then
    '        Me.VisualizzaImpostaEsito()
    '        Exit Sub
    '    End If

    '    Dim parametri As New ParsecAdmin.ParametriRepository
    '    Dim parametro = parametri.GetByName("TipoEsportazioneFattura", ParsecAdmin.TipoModulo.PRO)
    '    parametri.Dispose()

    '    Dim tipoEsportazione As String = "0"

    '    If Not parametro Is Nothing Then
    '        tipoEsportazione = parametro.Valore
    '    End If

    '    If Not esportaFatturaParametro Is Nothing Then

    '        Select Case tipoEsportazione
    '            'ESPORTAZIONE JSIBAC (0),   ESPORTAZIONE APSYSTEMS (2)
    '            Case "0", "2"

    '                ' Me.VisualizzaEsportaFattura()
    '                Dim usaFtpParametro = Me.Action.GetParameterByName("UsaFtp")

    '                If usaFtpParametro Is Nothing Then

    '                    Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '                    Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '                    Dim registrazione As ParsecPro.Registrazione = registrazioni.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault
    '                    registrazioni.Dispose()

    '                    If Not registrazione Is Nothing Then
    '                        Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
    '                        Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
    '                        If Not fattura Is Nothing Then
    '                            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.MessaggioSdI.Nomefile

    '                            If IO.File.Exists(localPath) Then
    '                                Session("AttachmentFullName") = localPath
    '                                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '                                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                                'Me.RegistraScriptPersistenzaVisibilitaPannello()
    '                                ParsecUtility.Utility.PageReload(pageUrl, False)
    '                            Else
    '                                ParsecUtility.Utility.MessageBox("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!", False)
    '                                Me.EnableUiHidden.Value = "Abilita"
    '                            End If
    '                        Else
    '                            ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                            Me.EnableUiHidden.Value = "Abilita"
    '                        End If
    '                        fattureElettroniche.Dispose()
    '                    End If
    '                    Exit Sub
    '                Else


    '                    Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '                    Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '                    Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)

    '                    registrazioni.Dispose()

    '                    If Not registrazione Is Nothing Then
    '                        Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
    '                        Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
    '                        If Not fattura Is Nothing Then
    '                            Dim nomefile As String = fattura.MessaggioSdI.Nomefile
    '                            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefile
    '                            Dim localPathMetaDati As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.NomeFileMetadati
    '                            If IO.File.Exists(localPath) Then

    '                                Dim riga As String = String.Empty

    '                                Dim password = WebConfigSettings.GetKey("FatturaServerPwd")
    '                                password = ParsecCommon.CryptoUtil.Decrypt(password)
    '                                Dim user = WebConfigSettings.GetKey("FatturaServerUser")
    '                                Dim port = WebConfigSettings.GetKey("FatturaServerPort")
    '                                Dim host = WebConfigSettings.GetKey("FatturaServerUrl").Split(New Char() {"//"}, StringSplitOptions.RemoveEmptyEntries)(1)
    '                                Dim url As String = WebConfigSettings.GetKey("FatturaServerPath")

    '                                Dim pos As Integer = nomefile.IndexOf(".")
    '                                Dim nomeFileSenzaEstensione As String = nomefile.Substring(0, pos)

    '                                Dim emails As New ParsecPro.EmailArrivoRepository
    '                                Dim email = emails.Where(Function(c) c.Id = fattura.MessaggioSdI.IdEmail).FirstOrDefault

    '                                Dim uri As String = String.Format("ftp://{0}:{1}/{2}", host, port.ToString, url)
    '                                Try
    '                                    Me.UploadFile(user, password, uri, localPath)

    '                                    If IO.File.Exists(localPathMetaDati) Then
    '                                        Me.UploadFile(user, password, uri, localPathMetaDati)
    '                                    Else
    '                                        '*************************************************************************************************
    '                                        'ESTRAGGO IL FILE METADATI DALLA EMAIL
    '                                        '*************************************************************************************************
    '                                        If Not email Is Nothing Then
    '                                            Me.EstraiMetaDatiFatturaDaEmail(email, localPathMetaDati)
    '                                            If IO.File.Exists(localPathMetaDati) Then
    '                                                Me.UploadFile(user, password, uri, localPathMetaDati)
    '                                            End If
    '                                        End If
    '                                        '*************************************************************************************************
    '                                    End If


    '                                    If tipoEsportazione = "0" Then

    '                                        Dim destinatarioInterno = registrazione.Destinatari.Where(Function(c) c.Interno = True).FirstOrDefault

    '                                        Dim dataProtocollo As String = registrazione.DataImmissione.Value.ToShortDateString.Replace("/", "")
    '                                        Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
    '                                        Dim dataConsegnaPec As String = fattura.MessaggioSdI.DataRicezioneInvio.ToShortDateString.Replace("/", "")
    '                                        Dim utenteSdI As String = If(Not email Is Nothing, email.Mittente, "")
    '                                        Dim nomeFileFattura As String = fattura.MessaggioSdI.Nomefile
    '                                        Dim nomeFileMetaDatiFattura As String = fattura.NomeFileMetadati
    '                                        Dim ufficioBilancio As String = String.Empty

    '                                        If Not destinatarioInterno Is Nothing Then
    '                                            ufficioBilancio = destinatarioInterno.UfficioBilancio
    '                                        End If

    '                                        Dim statoFattura As String = "A"
    '                                        Dim identificativoSdI As String = fattura.IdentificativoSdI
    '                                        Dim codiceIPA As String = fattura.CodiceIpaDestinatario

    '                                        riga = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", dataProtocollo, numeroProtocollo, dataConsegnaPec, utenteSdI, nomeFileFattura, nomeFileMetaDatiFattura, ufficioBilancio, statoFattura, identificativoSdI, codiceIPA)


    '                                        nomefile = nomeFileSenzaEstensione & ".fec"

    '                                    Else

    '                                        nomefile = nomeFileSenzaEstensione & ".txt"
    '                                        Dim nomeFileFattura As String = nomeFileSenzaEstensione & ".xml"
    '                                        Dim dataProtocollo As String = String.Format("{0:yyyyMMdd}", registrazione.DataImmissione.Value)
    '                                        Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
    '                                        riga = String.Format("{0};{1};{2}", nomeFileFattura, dataProtocollo, numeroProtocollo)

    '                                    End If



    '                                    Dim buffer As Byte() = System.Text.Encoding.Default.GetBytes(riga)
    '                                    Me.UploadFile(user, password, uri, nomefile, buffer)

    '                                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    '                                    fattura.IdStato = CInt(ParsecPro.StatoFattura.Contabilizzata)
    '                                    fattureElettroniche.SaveChanges()

    '                                Catch ex As Exception
    '                                    ParsecUtility.Utility.MessageBox("Impossibile esportare la fattura per il seguente motivo: " & vbCrLf & ex.Message, False)
    '                                    Me.EnableUiHidden.Value = "Abilita"
    '                                    Exit Sub
    '                                End Try

    '                            Else
    '                                ParsecUtility.Utility.MessageBox("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!", False)
    '                                Me.EnableUiHidden.Value = "Abilita"
    '                            End If
    '                        Else
    '                            fattureElettroniche.Dispose()
    '                            ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                            Me.EnableUiHidden.Value = "Abilita"
    '                        End If
    '                    Else
    '                        ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
    '                        Me.EnableUiHidden.Value = "Abilita"
    '                    End If

    '                    Exit Sub

    '                End If

    '                'ESPORTAZIONE TINN
    '            Case "1"

    '                Try
    '                    Me.EseguiEsportazioneTINN()
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                    Exit Sub
    '                End Try
    '                Exit Sub

    '            Case "3"  'ESPORTAZIONE DEDAGROUP

    '                Try
    '                    Dim taskAttivi As New List(Of ParsecWKF.TaskAttivo)
    '                    taskAttivi.Add(Me.TaskAttivo)
    '                    Me.EseguiEsportazioneDedaGroup(taskAttivi)
    '                Catch ex As Exception
    '                    ParsecUtility.Utility.MessageBox(ex.Message, False)
    '                    Me.EnableUiHidden.Value = "Abilita"
    '                    Exit Sub
    '                End Try
    '                Exit Sub


    '        End Select


    '    End If

    '    If Not protocollazioneParametro Is Nothing Then

    '        Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
    '        Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = TaskAttivo.IdIstanza AndAlso c.Stato Is Nothing).FirstOrDefault
    '        documentiGenerici.Dispose()

    '        'Se il documento generico non è stato ancora generato
    '        If documentoGenerico Is Nothing Then
    '            ParsecUtility.Utility.MessageBox("Per protocollare è necessario generare il documento!", False)
    '            Me.EnableUiHidden.Value = "Abilita"
    '        Else


    '            Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
    '            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, e.CommandName, Me.TaskAttivo.NomeFileIter)
    '            Dim registrazioni As New ParsecPro.RegistrazioniRepository
    '            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
    '            registrazioni.Dispose()

    '            'If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna Then

    '            If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza Then
    '                Dim mittenti As List(Of ParsecPro.Mittente) = registrazione.Mittenti
    '                Dim destinatari As List(Of ParsecPro.Destinatario) = registrazione.Destinatari
    '                'Rendo effettive le modifiche di inversione dei due gruppi di referenti
    '                registrazione.Mittenti = ParsecPro.Destinatari.ConvertiInMittenti(destinatari)
    '                registrazione.Destinatari = ParsecPro.Mittenti.ConvertiInDestinatari(mittenti)

    '                If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo Then
    '                    registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza
    '                Else
    '                    registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna
    '                    For Each dest In registrazione.Destinatari.Where(Function(c) c.Interno = True)
    '                        dest.Iter = True
    '                    Next
    '                End If

    '                Dim tipoRegistrazione As Integer = registrazione.TipoRegistrazione
    '                registrazione.TipoRegistrazione = CInt(registrazione.TipologiaRegistrazione)
    '                registrazione.DataDocumento = Now

    '                '*****************************************************************************
    '                'Aggiungo il collegamento alla registrazione che sta per essere inserita
    '                '*****************************************************************************
    '                Dim collegamento As New ParsecPro.Collegamento
    '                collegamento.AnnoProtocollo2 = Year(registrazione.DataImmissione)
    '                collegamento.NumeroProtocollo2 = registrazione.NumeroProtocollo
    '                collegamento.TipoRegistrazione2 = registrazione.TipoRegistrazione



    '                collegamento.Diretto = True
    '                collegamento.Oggetto = registrazione.Oggetto
    '                collegamento.NumeroProtocollo = collegamento.GetNumeroProtocollo(registrazione.NumeroProtocollo, tipoRegistrazione)

    '                Dim referenti As New StringBuilder
    '                Dim uffici As New StringBuilder


    '                For Each m As ParsecPro.Mittente In registrazione.Mittenti
    '                    If m.Interno Then
    '                        uffici.Append(m.Descrizione)
    '                    Else
    '                        referenti.Append(m.Descrizione)
    '                    End If
    '                Next

    '                For Each d As ParsecPro.Destinatario In registrazione.Destinatari
    '                    If d.Interno Then
    '                        uffici.Append(d.Descrizione)
    '                    Else
    '                        referenti.Append(d.Descrizione)
    '                    End If
    '                Next




    '                collegamento.Referenti = referenti.ToString
    '                collegamento.Uffici = uffici.ToString

    '                registrazione.NumeroProtocolloRiscontro = registrazione.NumeroProtocollo
    '                registrazione.DataImmissioneRiscontro = String.Format("{0:dd/MM/yyyy}", registrazione.DataImmissione)



    '                registrazione.Collegamenti.Clear()
    '                registrazione.Collegamenti.Add(collegamento)

    '                '*****************************************************************************
    '                'Copio l'allegato nella cartella temporanea.
    '                '*****************************************************************************
    '                Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
    '                Dim sorgenti As New ParsecAdmin.SorgentiRepository
    '                Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
    '                sorgenti.Dispose()
    '                Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
    '                Dim fileExists As Boolean = False

    '                Dim localPath As String = String.Empty

    '                Dim nomefile = documentoGenerico.NomeFile

    '                Select Case sorgenteCorrente.IdTipologia
    '                    Case 1 'Locale
    '                        localPath = source.Path & anno & "\" & nomefile
    '                        fileExists = IO.File.Exists(localPath)
    '                    Case 2  'Ftp
    '                        Dim relativePath As String = source.RelativePath & anno & "/" & nomefile
    '                        fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
    '                End Select

    '                If Not fileExists Then
    '                    ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
    '                    Exit Sub
    '                Else
    '                    Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
    '                    If Not IO.File.Exists(pathDownload) Then
    '                        IO.File.Copy(localPath, pathDownload)
    '                    End If

    '                    '*****************************************************************************
    '                    'Aggiungo l'allegato (documento generico) alla registrazione che sta per essere inserita
    '                    '*****************************************************************************

    '                    Dim allegato As New ParsecPro.Allegato
    '                    allegato.NomeFile = nomefile
    '                    allegato.NomeFileTemp = nomefile
    '                    allegato.IdTipologiaDocumento = 1 'Primario
    '                    allegato.DescrizioneTipologiaDocumento = "Primario"
    '                    allegato.Oggetto = "Documento di risposta"
    '                    allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
    '                    allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
    '                    allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
    '                    allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
    '                    registrazione.Allegati.Clear()

    '                    If nomefile.ToLower.EndsWith(".pdf") Then
    '                        allegato.Scansionato = True
    '                    End If

    '                    registrazione.Allegati.Add(allegato)
    '                    '*****************************************************************************
    '                End If



    '            End If

    '            Dim parametriPagina As New Hashtable
    '            parametriPagina.Add("RegistrazioneInIter", registrazione)


    '            Dim tipo As String = "1"
    '            Select Case protocollazioneParametro.Valore.ToUpper
    '                Case "INTERNO"
    '                    tipo = "2"
    '                    ' parametriPagina.Add("TaskAttivo", Me.TaskAttivo)
    '                Case "ARRIVO"
    '                    tipo = "0"
    '            End Select

    '            If Not disabilitaIterParametro Is Nothing Then
    '                parametriPagina.Add("DisabilitaIter", "1")
    '            End If


    '            Dim queryString As New Hashtable
    '            queryString.Add("Mode", "Insert")
    '            queryString.Add("Tipo", tipo) 'Partenza
    '            queryString.Add("obj", Me.AggiornaIterButton.ClientID)



    '            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    '            ParsecUtility.Utility.ShowPopup(pageUrl, 940, 600, queryString, False)
    '        End If

    '        Exit Sub
    '    End If



    '    If Not preparaDocumentoParametro Is Nothing Then

    '        Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
    '        Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault



    '        '***************************************************************************************
    '        'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
    '        '***************************************************************************************
    '        If Not documentoGenerico Is Nothing Then
    '            Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)

    '            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

    '            Dim visibile As Boolean = False

    '            If modello.VisibilitaPubblica.HasValue Then
    '                visibile = modello.VisibilitaPubblica
    '            End If

    '            If Not visibile Then
    '                visibile = (utenteCorrente.SuperUser OrElse documentoGenerico.IdUtente = utenteCorrente.Id)
    '            End If

    '            If visibile Then

    '                Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
    '                Dim sorgenti As New ParsecAdmin.SorgentiRepository
    '                Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
    '                sorgenti.Dispose()
    '                Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
    '                Dim fileExists As Boolean = False
    '                Select Case sorgenteCorrente.IdTipologia
    '                    Case 1 'Locale
    '                        Dim localPath As String = source.Path & anno & "\" & documentoGenerico.NomeFile
    '                        fileExists = IO.File.Exists(localPath)
    '                    Case 2  'Ftp
    '                        Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
    '                        fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
    '                End Select

    '                If Not fileExists Then
    '                    ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
    '                    Exit Sub
    '                Else
    '                    If documentoGenerico.GeneratoSistemaSEP Then
    '                        Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, True, source)
    '                    Else
    '                        Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, source)
    '                    End If


    '                    ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

    '                End If
    '            Else
    '                ParsecUtility.Utility.MessageBox("Non si possiedono le autorizzazioni per visualizzare il documento!", False)
    '            End If

    '        Else

    '            '***************************************************************************************
    '            'Apro la pagina per la generazione del documento generico.
    '            'Il salvattaggio esegue il click sul AggiornaIterButton  (persistenza stato del processo corrente)
    '            '***************************************************************************************

    '            Me.VisualizzaDocumentoGenericoInModifica()
    '        End If

    '        documentiGenerici.Dispose()

    '        Exit Sub
    '    End If

    'End Sub



    Private Sub ModificaPRO(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        '*************************************************************************************************************************************
        'Leggo tutti i parametri 
        '*************************************************************************************************************************************

        Dim preparaDocumentoParametro = Me.Action.GetParameterByName("PreparaDocumento")
        'Dim riferimentDocumentoParametro = Me.Action.GetParameterByName("RiferimentoDocumento") 'OUT
        Dim protocollazioneParametro = Me.Action.GetParameterByName("Protocollazione")
        Dim inviaEmailParametro = Me.Action.GetParameterByName("InviaEmail")

        Dim notificaEsitoFatturaParametro = Me.Action.GetParameterByName("NotificaEsitoFattura")
        Dim esportaFatturaParametro = Me.Action.GetParameterByName("EsportaFattura")

        Dim visualizzaFatturaParametro = Me.Action.GetParameterByName("VisualizzaFattura")
        '' ''Dim conservaFatturaParametro = Me.Action.GetParameterByName("Conserva")
        Dim avviaIterParametro = Me.Action.GetParameterByName("AvviaIter")
        Dim disabilitaIterParametro = Me.Action.GetParameterByName("DisabilitaIter")

        Dim creaIstanzaPratica = Me.Action.GetParameterByName("CreaIstanzaPratica")
        Dim visualizzaIstanzaPratica = Me.Action.GetParameterByName("VisualizzaIstanzaPratica")

        '' ''Dim conservaProtocolloParametro = Me.Action.GetParameterByName("ConservaProtocollo")


        '' ''If Not conservaProtocolloParametro Is Nothing Then
        '' ''    Try
        '' ''        Me.ConservaRegistrazioneProtocollo()
        '' ''        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
        '' ''    Catch ex As Exception

        '' ''        Me.RegistraScriptPersistenzaVisibilitaPannello()
        '' ''        Me.EnableUiHidden.Value = "Abilita"
        '' ''        ParsecUtility.Utility.MessageBox("Impossibile conservare il protocollo per il seguente motivo: " & vbCrLf & ex.Message, False)
        '' ''        ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)
        '' ''    End Try
        '' ''    Exit Sub
        '' ''End If


        If Not creaIstanzaPratica Is Nothing Then

            'todo verificare istanzePratica.IdFascicolo
            Dim fascicoli As New ParsecAdmin.FascicoliRepository
            Dim fascicoliProtocollo = fascicoli.GetByIdDocumento(Me.TaskAttivo.IdDocumento, ParsecAdmin.TipoModulo.PRO)
            fascicoli.Dispose()
            If fascicoliProtocollo.Count = 0 Then
                ParsecUtility.Utility.MessageBox("E' necessario aggiungere il protocollo in un fascicolo!", False)
                Me.EnableUiHidden.Value = "Abilita"
                Exit Sub
            End If
            Try

                Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.IdRegistrazione = Me.TaskAttivo.IdDocumento).FirstOrDefault
                If Not istanzePratica Is Nothing Then
                    istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.Avviata
                    istanzePraticaOnline.SaveChanges()
                    istanzePraticaOnline.Dispose()


                    Try
                        Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
                        Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
                        Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                        Dim sp As New ParsecWebServices.StatoPraticaWS(istanzePratica.NumeroPratica, utenteCorrente.Username, ParsecAdmin.StatoIstanzaPraticaOnline.Avviata)
                        sp.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)
                        sp.Insert()

                    Catch ex As Exception
                        'todo scrivere nel log
                    End Try

                    '*************************************************************************************************************
                    'AGGIUNGO LA PRATICA CORRENTE AI DOCUMENTI DEL FASCICOLO ASSOCIATO ALL'ISTANZA ONLINE CORRENTE
                    '*************************************************************************************************************
                    fascicoli = New ParsecAdmin.FascicoliRepository
                    Dim fascicolo = fascicoli.Where(Function(c) c.Codice = istanzePratica.CodiceFascicolo And c.Stato Is Nothing).FirstOrDefault



                    If Not fascicolo Is Nothing Then
                        Dim documentoFascicolo As New ParsecAdmin.FascicoloDocumento
                        documentoFascicolo.IdDocumento = istanzePratica.Id
                        documentoFascicolo.NomeDocumento = "Istanza N° " & istanzePratica.NumeroPratica & " del " & String.Format("{0:dd/MM/yyyy}", istanzePratica.DataPresentazione)
                        documentoFascicolo.NomeDocumentoOriginale = documentoFascicolo.NomeDocumento
                        documentoFascicolo.TipoDocumento = ParsecAdmin.TipoModulo.IOL
                        documentoFascicolo.DescrizioneTipoDocumento = "IOL"
                        documentoFascicolo.DataDocumento = istanzePratica.DataPresentazione
                        'documentoFascicolo.NumeroDocumento = istanzePratica.NumeroPratica
                        documentoFascicolo.Definitivo = True
                        documentoFascicolo.Fase = String.Empty 'ParsecAdmin.FaseDocumentoEnumeration.INIZIALE
                        documentoFascicolo.DescrizioneFase = String.Empty '"Iniziale"
                        documentoFascicolo.Id = -1


                        documentoFascicolo.IdFascicolo = fascicolo.Id
                        documentoFascicolo.path = fascicolo.DataCreazioneFascicolo.Year.ToString & "\"


                        Dim documentiFascicolo As New ParsecAdmin.FascicoloDocumentoRepository
                        documentiFascicolo.Add(documentoFascicolo)
                        documentiFascicolo.SaveChanges()
                        documentiFascicolo.Dispose()


                    End If
                    fascicoli.Dispose()
                    '*************************************************************************************************************

                Else
                    istanzePraticaOnline.Dispose()
                    ParsecUtility.Utility.MessageBox("Impossibile creare la pratica per il seguente motivo: " & vbCrLf & "La pratica associata al protocollo con id " & Me.TaskAttivo.IdDocumento & " non  è stata trovata!", False)
                    Me.EnableUiHidden.Value = "Abilita"
                    Exit Sub
                End If
                Me.AggiornaIstanzaWorkflow(istanzePratica)
                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                Exit Sub
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Impossibile creare la pratica per il seguente motivo: " & vbCrLf & ex.Message, False)
                Me.EnableUiHidden.Value = "Abilita"
                Exit Sub
            End Try

        End If

        If Not visualizzaIstanzaPratica Is Nothing Then
            Me.VisualizzaIstanzaPratica()
            Exit Sub
        End If



        '*************************************************************************************************************************************

        If Not avviaIterParametro Is Nothing Then

            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim protocollo = registrazioni.GetById(Me.TaskAttivo.IdDocumento)
            registrazioni.Dispose()
            If Not protocollo Is Nothing Then
                'TERMINO L'ITER CORRENTE
                Me.TerminaTask()
                'AVVIO UN NUOVO ITER RELATIVO AL PROTOCOLLO CORRENTE
                Me.CreaIstanzaPro(protocollo)
                Me.RegistraButtonClick()
            Else
                ParsecUtility.Utility.MessageBox("Impossibile trovare il protocollo con id " & Me.TaskAttivo.IdDocumento & " !", False)
                Me.EnableUiHidden.Value = "Abilita"
            End If
            Exit Sub
        End If


        '' ''If Not conservaFatturaParametro Is Nothing Then

        '' ''    Dim fatture As New ParsecPro.FatturaElettronicaRepository
        '' ''    Dim fattura = fatture.Where(Function(c) c.IdRegistrazione = Me.TaskAttivo.IdDocumento).FirstOrDefault
        '' ''    If Not fattura Is Nothing Then
        '' ''        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        '' ''        Dim registrazione = registrazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
        '' ''        If Not registrazione Is Nothing Then

        '' ''            Try
        '' ''                Me.ConservaFattura(fattura, registrazione.NumeroProtocollo.ToString, registrazione.DataImmissione.Value)
        '' ''                fattura.IdStato = CInt(ParsecPro.StatoFattura.Convervata)
        '' ''                fatture.SaveChanges()
        '' ''                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
        '' ''            Catch ex As Exception
        '' ''                ParsecUtility.Utility.MessageBox(ex.Message, False)
        '' ''                Me.EnableUiHidden.Value = "Abilita"
        '' ''            End Try

        '' ''        Else
        '' ''            ParsecUtility.Utility.MessageBox("Protocollo non trovato!", False)
        '' ''            Me.EnableUiHidden.Value = "Abilita"
        '' ''        End If
        '' ''    Else
        '' ''        ParsecUtility.Utility.MessageBox("Fattura elettronica non trovata!", False)
        '' ''        Me.EnableUiHidden.Value = "Abilita"

        '' ''    End If
        '' ''    fatture.Dispose()
        '' ''    Exit Sub
        '' ''End If



        If Not visualizzaFatturaParametro Is Nothing Then
            Me.VisualizzaFattura()
            Exit Sub
        End If


        If Me.Action.Parameters.Count = 0 Then
            Me.VisualizzaProtocolloInModifica()
            Exit Sub
        End If

        If Not inviaEmailParametro Is Nothing Then
            Me.VisualizzaEmailInModifica()
            Exit Sub
        End If

        If Not notificaEsitoFatturaParametro Is Nothing Then
            Me.VisualizzaImpostaEsito()
            Exit Sub
        End If

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("TipoEsportazioneFattura", ParsecAdmin.TipoModulo.PRO)
        parametri.Dispose()

        Dim tipoEsportazione As String = "0"

        If Not parametro Is Nothing Then
            tipoEsportazione = parametro.Valore
        End If

        If Not esportaFatturaParametro Is Nothing Then

            Select Case tipoEsportazione
                'ESPORTAZIONE JSIBAC (0),   ESPORTAZIONE APSYSTEMS (2)
                Case "0", "2"

                    ' Me.VisualizzaEsportaFattura()
                    Dim usaFtpParametro = Me.Action.GetParameterByName("UsaFtp")

                    If usaFtpParametro Is Nothing Then

                        Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
                        Dim registrazioni As New ParsecPro.RegistrazioniRepository
                        Dim registrazione As ParsecPro.Registrazione = registrazioni.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault
                        registrazioni.Dispose()

                        If Not registrazione Is Nothing Then
                            Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
                            Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
                            If Not fattura Is Nothing Then
                                Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.MessaggioSdI.Nomefile

                                If IO.File.Exists(localPath) Then
                                    Session("AttachmentFullName") = localPath
                                    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                                    'Me.RegistraScriptPersistenzaVisibilitaPannello()
                                    ParsecUtility.Utility.PageReload(pageUrl, False)
                                Else
                                    ParsecUtility.Utility.MessageBox("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!", False)
                                    Me.EnableUiHidden.Value = "Abilita"
                                End If
                            Else
                                ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
                                Me.EnableUiHidden.Value = "Abilita"
                            End If
                            fattureElettroniche.Dispose()
                        End If
                        Exit Sub
                    Else


                        Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
                        Dim registrazioni As New ParsecPro.RegistrazioniRepository
                        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)

                        registrazioni.Dispose()

                        If Not registrazione Is Nothing Then
                            Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
                            Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
                            If Not fattura Is Nothing Then
                                Dim nomefile As String = fattura.MessaggioSdI.Nomefile
                                Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefile
                                Dim localPathMetaDati As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & fattura.NomeFileMetadati
                                If IO.File.Exists(localPath) Then

                                    Dim riga As String = String.Empty

                                    Dim password = WebConfigSettings.GetKey("FatturaServerPwd")
                                    password = ParsecCommon.CryptoUtil.Decrypt(password)
                                    Dim user = WebConfigSettings.GetKey("FatturaServerUser")
                                    Dim port = WebConfigSettings.GetKey("FatturaServerPort")
                                    Dim host = WebConfigSettings.GetKey("FatturaServerUrl").Split(New Char() {"//"}, StringSplitOptions.RemoveEmptyEntries)(1)
                                    Dim url As String = WebConfigSettings.GetKey("FatturaServerPath")

                                    Dim pos As Integer = nomefile.IndexOf(".")
                                    Dim nomeFileSenzaEstensione As String = nomefile.Substring(0, pos)

                                    Dim emails As New ParsecPro.EmailArrivoRepository
                                    Dim email = emails.Where(Function(c) c.Id = fattura.MessaggioSdI.IdEmail).FirstOrDefault

                                    Dim uri As String = String.Format("ftp://{0}:{1}/{2}", host, port.ToString, url)
                                    Try
                                        Me.UploadFile(user, password, uri, localPath)

                                        If IO.File.Exists(localPathMetaDati) Then
                                            Me.UploadFile(user, password, uri, localPathMetaDati)
                                        Else
                                            '*************************************************************************************************
                                            'ESTRAGGO IL FILE METADATI DALLA EMAIL
                                            '*************************************************************************************************
                                            If Not email Is Nothing Then
                                                Me.EstraiMetaDatiFatturaDaEmail(email, localPathMetaDati)
                                                If IO.File.Exists(localPathMetaDati) Then
                                                    Me.UploadFile(user, password, uri, localPathMetaDati)
                                                End If
                                            End If
                                            '*************************************************************************************************
                                        End If


                                        If tipoEsportazione = "0" Then

                                            Dim destinatarioInterno = registrazione.Destinatari.Where(Function(c) c.Interno = True).FirstOrDefault

                                            Dim dataProtocollo As String = registrazione.DataImmissione.Value.ToShortDateString.Replace("/", "")
                                            Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
                                            Dim dataConsegnaPec As String = fattura.MessaggioSdI.DataRicezioneInvio.ToShortDateString.Replace("/", "")
                                            Dim utenteSdI As String = If(Not email Is Nothing, email.Mittente, "")
                                            Dim nomeFileFattura As String = fattura.MessaggioSdI.Nomefile
                                            Dim nomeFileMetaDatiFattura As String = fattura.NomeFileMetadati
                                            Dim ufficioBilancio As String = String.Empty

                                            If Not destinatarioInterno Is Nothing Then
                                                ufficioBilancio = destinatarioInterno.UfficioBilancio
                                            End If

                                            Dim statoFattura As String = "A"
                                            Dim identificativoSdI As String = fattura.IdentificativoSdI
                                            Dim codiceIPA As String = fattura.CodiceIpaDestinatario

                                            riga = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", dataProtocollo, numeroProtocollo, dataConsegnaPec, utenteSdI, nomeFileFattura, nomeFileMetaDatiFattura, ufficioBilancio, statoFattura, identificativoSdI, codiceIPA)


                                            nomefile = nomeFileSenzaEstensione & ".fec"

                                        Else

                                            nomefile = nomeFileSenzaEstensione & ".txt"
                                            Dim nomeFileFattura As String = nomeFileSenzaEstensione & ".xml"
                                            Dim dataProtocollo As String = String.Format("{0:yyyyMMdd}", registrazione.DataImmissione.Value)
                                            Dim numeroProtocollo As String = registrazione.NumeroProtocollo.Value.ToString
                                            riga = String.Format("{0};{1};{2}", nomeFileFattura, dataProtocollo, numeroProtocollo)

                                        End If



                                        Dim buffer As Byte() = System.Text.Encoding.Default.GetBytes(riga)
                                        Me.UploadFile(user, password, uri, nomefile, buffer)

                                        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                                        fattura.IdStato = CInt(ParsecPro.StatoFattura.Contabilizzata)
                                        fattureElettroniche.SaveChanges()

                                    Catch ex As Exception
                                        ParsecUtility.Utility.MessageBox("Impossibile esportare la fattura per il seguente motivo: " & vbCrLf & ex.Message, False)
                                        Me.EnableUiHidden.Value = "Abilita"
                                        Exit Sub
                                    End Try

                                Else
                                    ParsecUtility.Utility.MessageBox("Il documento selezionato " & fattura.MessaggioSdI.Nomefile & " non esiste!", False)
                                    Me.EnableUiHidden.Value = "Abilita"
                                End If
                            Else
                                fattureElettroniche.Dispose()
                                ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
                                Me.EnableUiHidden.Value = "Abilita"
                            End If
                        Else
                            ParsecUtility.Utility.MessageBox("Il protocollo selezionato non ha registrato nessuna fattura!", False)
                            Me.EnableUiHidden.Value = "Abilita"
                        End If

                        Exit Sub

                    End If

                    'ESPORTAZIONE TINN
                Case "1"

                    Try
                        Me.EseguiEsportazioneTINN()
                    Catch ex As Exception
                        ParsecUtility.Utility.MessageBox(ex.Message, False)
                        Me.EnableUiHidden.Value = "Abilita"
                        Exit Sub
                    End Try
                    Exit Sub

                Case "3"  'ESPORTAZIONE DEDAGROUP

                    Try
                        Dim taskAttivi As New List(Of ParsecWKF.TaskAttivo)
                        taskAttivi.Add(Me.TaskAttivo)
                        Me.EseguiEsportazioneDedaGroup(taskAttivi)
                    Catch ex As Exception
                        ParsecUtility.Utility.MessageBox(ex.Message, False)
                        Me.EnableUiHidden.Value = "Abilita"
                        Exit Sub
                    End Try
                    Exit Sub


            End Select


        End If

        If Not protocollazioneParametro Is Nothing Then

            Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
            Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = TaskAttivo.IdIstanza AndAlso c.Stato Is Nothing).FirstOrDefault
            documentiGenerici.Dispose()

            'Se il documento generico non è stato ancora generato
            If documentoGenerico Is Nothing Then
                ParsecUtility.Utility.MessageBox("Per protocollare è necessario generare il documento!", False)
                Me.EnableUiHidden.Value = "Abilita"
            Else


                Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
                Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, e.CommandName, Me.TaskAttivo.NomeFileIter)
                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
                registrazioni.Dispose()

                'If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna Then

                If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna OrElse registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza Then
                    Dim mittenti As List(Of ParsecPro.Mittente) = registrazione.Mittenti
                    Dim destinatari As List(Of ParsecPro.Destinatario) = registrazione.Destinatari
                    'Rendo effettive le modifiche di inversione dei due gruppi di referenti
                    registrazione.Mittenti = ParsecPro.Destinatari.ConvertiInMittenti(destinatari)
                    registrazione.Destinatari = ParsecPro.Mittenti.ConvertiInDestinatari(mittenti)

                    If registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Arrivo Then
                        registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza
                    Else
                        registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Interna
                        For Each dest In registrazione.Destinatari.Where(Function(c) c.Interno = True)
                            dest.Iter = True
                        Next
                    End If

                    Dim tipoRegistrazione As Integer = registrazione.TipoRegistrazione
                    registrazione.TipoRegistrazione = CInt(registrazione.TipologiaRegistrazione)
                    registrazione.DataDocumento = Now

                    '*****************************************************************************
                    'Aggiungo il collegamento alla registrazione che sta per essere inserita
                    '*****************************************************************************
                    'Dim collegamento As New ParsecPro.Collegamento
                    'collegamento.AnnoProtocollo2 = Year(registrazione.DataImmissione)
                    'collegamento.NumeroProtocollo2 = registrazione.NumeroProtocollo
                    'collegamento.TipoRegistrazione2 = registrazione.TipoRegistrazione

                    Dim collegamento As New ParsecPro.Collegamento
                    collegamento.AnnoProtocollo2 = Year(registrazione.DataImmissione)
                    collegamento.NumeroProtocollo2 = registrazione.NumeroProtocollo

                    collegamento.TipoRegistrazione2 = tipoRegistrazione 'registrazione.TipoRegistrazione
                    collegamento.AnnoProtocollo = Year(registrazione.DataImmissione)


                    collegamento.Diretto = True
                    collegamento.Oggetto = registrazione.Oggetto
                    collegamento.NumeroProtocollo = collegamento.GetNumeroProtocollo(registrazione.NumeroProtocollo, tipoRegistrazione)

                    Dim referenti As New StringBuilder
                    Dim uffici As New StringBuilder


                    For Each m As ParsecPro.Mittente In registrazione.Mittenti
                        If m.Interno Then
                            uffici.Append(m.Descrizione)
                        Else
                            referenti.Append(m.Descrizione)
                        End If
                    Next

                    For Each d As ParsecPro.Destinatario In registrazione.Destinatari
                        If d.Interno Then
                            uffici.Append(d.Descrizione)
                        Else
                            referenti.Append(d.Descrizione)
                        End If
                    Next




                    collegamento.Referenti = referenti.ToString
                    collegamento.Uffici = uffici.ToString

                    registrazione.NumeroProtocolloRiscontro = registrazione.NumeroProtocollo
                    registrazione.DataImmissioneRiscontro = String.Format("{0:dd/MM/yyyy}", registrazione.DataImmissione)



                    registrazione.Collegamenti.Clear()
                    registrazione.Collegamenti.Add(collegamento)

                    '*****************************************************************************
                    'Copio l'allegato nella cartella temporanea.
                    '*****************************************************************************
                    Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
                    Dim sorgenti As New ParsecAdmin.SorgentiRepository
                    Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
                    sorgenti.Dispose()
                    Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
                    Dim fileExists As Boolean = False

                    Dim localPath As String = String.Empty

                    Dim nomefile = documentoGenerico.NomeFile

                    Select Case sorgenteCorrente.IdTipologia
                        Case 1 'Locale
                            localPath = source.Path & anno & "\" & nomefile
                            fileExists = IO.File.Exists(localPath)
                        Case 2  'Ftp
                            Dim relativePath As String = source.RelativePath & anno & "/" & nomefile
                            fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
                    End Select

                    If Not fileExists Then
                        ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
                        Exit Sub
                    Else
                        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
                        If Not IO.File.Exists(pathDownload) Then
                            IO.File.Copy(localPath, pathDownload)
                        End If

                        '*****************************************************************************
                        'Aggiungo l'allegato (documento generico) alla registrazione che sta per essere inserita
                        '*****************************************************************************

                        Dim allegato As New ParsecPro.Allegato
                        allegato.NomeFile = nomefile
                        allegato.NomeFileTemp = nomefile
                        allegato.IdTipologiaDocumento = 1 'Primario
                        allegato.DescrizioneTipologiaDocumento = "Primario"
                        allegato.Oggetto = "Documento di risposta"
                        allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                        allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
                        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
                        registrazione.Allegati.Clear()

                        If nomefile.ToLower.EndsWith(".pdf") Then
                            allegato.Scansionato = True
                        End If

                        registrazione.Allegati.Add(allegato)
                        '*****************************************************************************
                    End If



                End If

                Dim parametriPagina As New Hashtable
                parametriPagina.Add("RegistrazioneInIter", registrazione)


                Dim tipo As String = "1"
                Select Case protocollazioneParametro.Valore.ToUpper
                    Case "INTERNO"
                        tipo = "2"
                        ' parametriPagina.Add("TaskAttivo", Me.TaskAttivo)
                    Case "ARRIVO"
                        tipo = "0"
                End Select

                If Not disabilitaIterParametro Is Nothing Then
                    parametriPagina.Add("DisabilitaIter", "1")
                End If


                Dim queryString As New Hashtable
                queryString.Add("Mode", "Insert")
                queryString.Add("Tipo", tipo) 'Partenza
                queryString.Add("obj", Me.AggiornaIterButton.ClientID)



                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 600, queryString, False)
            End If

            Exit Sub
        End If



        If Not preparaDocumentoParametro Is Nothing Then

            Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
            Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault



            '***************************************************************************************
            'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
            '***************************************************************************************
            If Not documentoGenerico Is Nothing Then
                Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)

                Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                Dim visibile As Boolean = False

                If modello.VisibilitaPubblica.HasValue Then
                    visibile = modello.VisibilitaPubblica
                End If

                If Not visibile Then
                    visibile = (utenteCorrente.SuperUser OrElse documentoGenerico.IdUtente = utenteCorrente.Id)
                End If

                If visibile Then

                    Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
                    Dim sorgenti As New ParsecAdmin.SorgentiRepository
                    Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
                    sorgenti.Dispose()
                    Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
                    Dim fileExists As Boolean = False
                    Select Case sorgenteCorrente.IdTipologia
                        Case 1 'Locale
                            Dim localPath As String = source.Path & anno & "\" & documentoGenerico.NomeFile
                            fileExists = IO.File.Exists(localPath)
                        Case 2  'Ftp
                            Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
                            fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
                    End Select

                    If Not fileExists Then
                        ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
                        Exit Sub
                    Else
                        If documentoGenerico.GeneratoSistemaSEP Then
                            Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, True, source)
                        Else
                            Me.VisualizzaDocumento(documentoGenerico.NomeFile, anno, source)
                        End If


                        ParsecUtility.Utility.ButtonClick(Me.AggiornaTaskButton.ClientID, False)

                    End If
                Else
                    ParsecUtility.Utility.MessageBox("Non si possiedono le autorizzazioni per visualizzare il documento!", False)
                End If

            Else

                '***************************************************************************************
                'Apro la pagina per la generazione del documento generico.
                'Il salvattaggio esegue il click sul AggiornaIterButton  (persistenza stato del processo corrente)
                '***************************************************************************************

                Me.VisualizzaDocumentoGenericoInModifica()
            End If

            documentiGenerici.Dispose()

            Exit Sub
        End If

    End Sub


    Private Sub EseguiEsportazioneDedaGroup(ByVal taskAttivi As List(Of ParsecWKF.TaskAttivo))

        Dim sb As New StringBuilder

        Try

            Dim clienti As New ParsecAdmin.ClientRepository
            Dim cliente = clienti.GetFull
            clienti.Dispose()

            Dim codiceAOO As String = String.Empty

            If Not cliente.AreeOrganizzativeOmogenee Is Nothing Then
                If cliente.AreeOrganizzativeOmogenee.Count > 0 Then
                    codiceAOO = cliente.AreeOrganizzativeOmogenee(0).CodiceAOO
                End If
            End If

            Dim endPoint As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupBaseUrl")
            Dim scope As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupScope")
            Dim accessTokenUri As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupAccessTokenUri")
            Dim clientId As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientId")
            Dim clientSecret As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientSecret")

            Dim parameter As New ParsecWebServices.OAuth2Parameter With {.AccessTokenUri = accessTokenUri, .ClientId = clientId, .ClientSecret = clientSecret, .Scope = scope}

            Dim service As New ParsecWebServices.DedaGroupService(endPoint, parameter)

            Dim idRegistrazione As Integer = 0
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = Nothing
            Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
            Dim fattura As ParsecPro.FatturaElettronica = Nothing


            Dim fatturaModel As ParsecWebServices.FatturaElettronicaPAModel = Nothing
            Dim singolaFatturaModel As ParsecWebServices.SingolaFatturaModel = Nothing
            Dim postParameters As Dictionary(Of String, Object) = Nothing
            Dim res As ParsecWebServices.FatturaElettronicaPAResult = Nothing
            Dim xmlBuffer As Byte() = New Byte() {}
            Dim buffer As Byte() = New Byte() {}
            Dim signedCms As System.Security.Cryptography.Pkcs.SignedCms = Nothing
            Dim notificato As Boolean = False
            Dim operazione As String = String.Empty

            For Each taskCorrente As ParsecWKF.TaskAttivo In taskAttivi

                Me.TaskAttivo = taskCorrente

                idRegistrazione = Me.TaskAttivo.IdDocumento
                registrazione = registrazioni.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault

                If Not registrazione Is Nothing Then

                    fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
                    If Not fattura Is Nothing Then

                        Dim nomefile As String = fattura.MessaggioSdI.Nomefile
                        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefile

                        If IO.File.Exists(localPath) Then

                            Try

                                If localPath.ToLower.EndsWith(".p7m") Then

                                    buffer = IO.File.ReadAllBytes(localPath)
                                    signedCms = New System.Security.Cryptography.Pkcs.SignedCms
                                    'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
                                    Try
                                        buffer = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(buffer))
                                    Catch ex As Exception
                                        'NIENTE
                                    End Try

                                    signedCms.Decode(buffer)
                                    xmlBuffer = signedCms.ContentInfo.Content
                                    nomefile = IO.Path.GetFileNameWithoutExtension(nomefile)

                                Else
                                    xmlBuffer = IO.File.ReadAllBytes(localPath)
                                End If

                                'fatturaModel = New ParsecWebServices.FatturaElettronicaPAModel
                                'fatturaModel.IdentificativoSDI = fattura.IdentificativoSdI
                                'fatturaModel.IsLotto = "false"
                                'fatturaModel.DataRicezione = String.Format("{0:yyyy-MM-dd}", fattura.MessaggioSdI.DataRicezioneInvio)
                                'fatturaModel.SingolaFatture = New List(Of ParsecWebServices.SingolaFatturaModel)

                                'singolaFatturaModel = New ParsecWebServices.SingolaFatturaModel
                                'singolaFatturaModel.Oggetto = fattura.Oggetto
                                'singolaFatturaModel.TipoOperazione = ParsecWebServices.TipoOperazioneFatturaElettronicaPA.Add

                                fatturaModel = New ParsecWebServices.FatturaElettronicaPAModel
                                fatturaModel.IdentificativoSDI = fattura.IdentificativoSdI

                                If fattura.NumeroFatture = 1 Then
                                    fatturaModel.IsLotto = "false"
                                Else
                                    fatturaModel.IsLotto = "true"
                                End If

                                fatturaModel.DataRicezione = String.Format("{0:yyyy-MM-dd}", fattura.MessaggioSdI.DataRicezioneInvio)
                                fatturaModel.SingolaFatture = New List(Of ParsecWebServices.SingolaFatturaModel)

                                singolaFatturaModel = New ParsecWebServices.SingolaFatturaModel
                                If fattura.NumeroFatture > 1 Then
                                    singolaFatturaModel.Oggetto = "Lotto di n." & fattura.NumeroFatture.ToString & " fatture - Identificativo SdI " & fattura.IdentificativoSdI
                                Else
                                    singolaFatturaModel.Oggetto = fattura.Oggetto
                                End If

                                singolaFatturaModel.TipoOperazione = ParsecWebServices.TipoOperazioneFatturaElettronicaPA.Add

                                singolaFatturaModel.Stato = ParsecWebServices.StatoFatturaElettronicaPA.Accettata
                                singolaFatturaModel.Fornitore = ParsecWebServices.PersonaGiuridicaFornitoreModel.CreateFornitore(fattura.DenominazioneFornitore, fattura.PartitaIvaFornitore)

                                fatturaModel.SingolaFatture.Add(singolaFatturaModel)
                                fatturaModel.XMLFattura = New ParsecWebServices.AllegatoFatturaElettronicaPAModel With {.NomeFile = nomefile, .MimeType = "application/xml"}


                                fatturaModel.Protocollo = New ParsecWebServices.ProtocolloModel With {.Numero = registrazione.NumeroProtocollo, .CodiceAOO = codiceAOO, .DataRegistrazione = String.Format("{0:yyyy-MM-dd}", registrazione.DataImmissione), .OraRegistrazione = String.Format("{0:HH:mm}", registrazione.DataImmissione)}


                                postParameters = New Dictionary(Of String, Object)
                                postParameters.Add("fepa", New ParsecWebServices.FileParameter With {.Buffer = fatturaModel.ToJson, .FileName = "fepa.json", .ContentType = "application/json"})
                                postParameters.Add("xml", New ParsecWebServices.FileParameter With {.Buffer = xmlBuffer, .FileName = nomefile, .ContentType = "application/xml"})

                                res = service.AcquisisciFatture(postParameters)

                                If res.esito = 200 Then
                                    notificato = (Me.Action.FromActor = Me.Action.ToActor)
                                    operazione = Me.Action.Description.ToUpper
                                    Me.WriteTask(Me.TaskAttivo.IdMittente, operazione, notificato)

                                    fattura.IdStato = CInt(ParsecPro.StatoFattura.Contabilizzata)
                                    fattureElettroniche.SaveChanges()
                                Else
                                    sb.AppendLine("La fattura elettronica con ident. sdi '" & fattura.IdentificativoSdI & "' non è stata esportata per il seguente motivo:" & vbCrLf & res.resultDescription)
                                End If


                            Catch ex As Exception
                                sb.AppendLine("La fattura elettronica con ident. sdi '" & fattura.IdentificativoSdI & "' non è stata esportata per il seguente motivo:" & vbCrLf & ex.Message)
                            End Try
                        Else
                            sb.AppendLine("La fattura elettronica con ident. sdi '" & fattura.IdentificativoSdI & "' non è stata esportata per il seguente motivo:" & vbCrLf & "File '" & nomefile & "' non trovato")
                        End If
                    Else
                        sb.AppendLine("La fattura elettronica associata al protocollo n. " & registrazione.NumeroProtocollo & " del " & registrazione.DataImmissione.Value.ToShortDateString & " non esiste!")
                    End If
                Else
                    sb.AppendLine("Il protocollo associato al task '" & Me.TaskAttivo.DescrizioneDocumento & "' non esiste!")
                End If
            Next

            registrazioni.Dispose()
            fattureElettroniche.Dispose()



        Catch ex As Exception
            sb.AppendLine(ex.Message)
        End Try

        If sb.Length > 0 Then
            Me.RegistraScriptPersistenzaVisibilitaPannello()
            ParsecUtility.Utility.MessageBox(sb.ToString, False)
            Me.RegistraButtonClick()
        Else
            Me.FirstClick = False
            'NON FUNZIONA SU CHROME
            Me.RegistraButtonClick()
        End If

    End Sub

    Private Sub EseguiEsportazioneTINN()

        Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault
        registrazioni.Dispose()

        If Not registrazione Is Nothing Then

            Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
            Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault

            If Not fattura Is Nothing Then

                Dim nomefile As String = fattura.MessaggioSdI.Nomefile

                Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefile

                If IO.File.Exists(localPath) Then
                    Dim esportazioneFattureTINN As New ParsecPro.EsportazioneFatturaTINNRepository
                    Dim fatturaDaEsportare As New ParsecPro.EsportazioneFatturaTINN

                    Dim oggetto As String = fattura.Oggetto
                    Dim numeroFattura As String = String.Empty
                    Dim dataFattura As DateTime = Nothing
                    Dim reNumero As New Regex("(?<Numero>[\d]+)")
                    Dim reData As New Regex("(?<Data>[\d]{2}/[\d]{2}/[\d]{4})")
                    Dim m As Match = reNumero.Match(oggetto)
                    If m.Success Then
                        numeroFattura = m.Groups("Numero").Value
                    End If
                    m = reData.Match(oggetto)
                    If m.Success Then
                        dataFattura = CDate(m.Groups("Data").Value)
                    End If

                    fatturaDaEsportare.NomeFileFattura = nomefile
                    fatturaDaEsportare.PercorsoRelativo = fattura.MessaggioSdI.PercorsoRelativo
                    fatturaDaEsportare.AnnoProtocollo = fattura.AnnoProtocollo
                    fatturaDaEsportare.DataProtocollo = registrazione.DataImmissione.Value
                    fatturaDaEsportare.NumeroProtocollo = fattura.NumeroProtocollo
                    fatturaDaEsportare.NumeroFattura = numeroFattura
                    fatturaDaEsportare.DataFattura = dataFattura
                    fatturaDaEsportare.StatoFattura = "0" 'NON CONTABILIZZATA
                    fatturaDaEsportare.Inviata = False
                    fatturaDaEsportare.IdFattura = fattura.Id
                    Try

                        Dim esiste = esportazioneFattureTINN.Where(Function(c) c.IdFattura = fattura.Id).Any

                        If Not esiste Then
                            esportazioneFattureTINN.Add(fatturaDaEsportare)
                            esportazioneFattureTINN.SaveChanges()
                        End If

                        esportazioneFattureTINN.Dispose()

                        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)

                    Catch ex As Exception
                        Dim messaggio As String = String.Empty
                        If Not ex.InnerException Is Nothing Then
                            messaggio = ex.InnerException.Message
                        Else
                            messaggio = ex.Message
                        End If
                        Throw New ApplicationException("Impossibile esportare la fattura per il seguente motivo: " & vbCrLf & messaggio)
                    End Try
                Else
                    Throw New ApplicationException("Il file della fattura " & nomefile & " non esiste!")
                End If
            Else
                Throw New ApplicationException("La fattura associata al protocollo n. " & registrazione.NumeroProtocollo & " del " & registrazione.DataImmissione.Value.ToShortDateString & " non esiste!")
            End If
        Else
            Throw New ApplicationException("Il protocollo associato al task corrente non esiste!")
        End If
    End Sub

    Private Function ConfigureSmtp(ByVal casellaPec As ParsecAdmin.ParametriPec) As Rebex.Net.Smtp
        Dim client As Rebex.Net.Smtp = Nothing
        Try
            If Not casellaPec Is Nothing Then
                client = New Rebex.Net.Smtp
                client.Settings.SslAcceptAllCertificates = True
                client.Settings.SslAllowedSuites = client.Settings.SslAllowedSuites And TlsCipherSuite.DH_anon_EXPORT_WITH_RC4_40_MD5 Or TlsCipherSuite.DH_anon_WITH_RC4_128_MD5 Or TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA Or TlsCipherSuite.DHE_DSS_WITH_RC4_128_SHA Or TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA Or TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 Or TlsCipherSuite.RSA_WITH_RC4_128_SHA Or TlsCipherSuite.RSA_WITH_RC4_128_MD5 Or TlsCipherSuite.RSA_WITH_3DES_EDE_CBC_SHA Or TlsCipherSuite.RSA_WITH_AES_128_CBC_SHA
                Dim mode As Rebex.Net.SslMode = SslMode.None
                Select Case casellaPec.SmtpPorta.Value
                    Case 465
                        mode = SslMode.Implicit
                    Case 25, 587
                        If casellaPec.SmtpIsSSL Then
                            mode = SslMode.Explicit
                        End If
                End Select

                Dim connesso As Boolean = False

                Try
                    client.Connect(casellaPec.SmtpServer, casellaPec.SmtpPorta.Value, mode)
                    connesso = True
                Catch ex As Exception

                    For Each suit As TlsCipherSuite In System.Enum.GetValues(GetType(TlsCipherSuite))
                        Try
                            client.Settings.SslAllowedSuites = suit
                            client.Connect(casellaPec.SmtpServer, casellaPec.SmtpPorta.Value, mode)
                            connesso = True
                            Exit For
                        Catch ex2 As Exception
                            connesso = False
                        End Try
                    Next

                End Try

                If connesso Then
                    Dim password As String = ParsecCommon.CryptoUtil.Decrypt(casellaPec.Password)
                    client.Login(casellaPec.UserId, password)
                Else
                    Throw New ApplicationException("Impossibile connettersi al server SMTP " & casellaPec.SmtpServer)
                End If

            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try
        Return client
    End Function

    Private Function InviaEmail(ByVal fullPathFattura As String, ByVal fattura As ParsecPro.FatturaElettronica, ByVal registrazione As ParsecPro.Registrazione) As ParsecPro.Email

        Dim idEmail As Integer = fattura.MessaggioSdI.IdEmail
        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim email = emails.Where(Function(c) c.Id = idEmail).FirstOrDefault
        emails.Dispose()

        If Not email Is Nothing Then
            Dim caselle As New ParsecAdmin.ParametriPecRepository
            Dim casellaPec = caselle.Where(Function(c) c.Id = email.IdCasella).FirstOrDefault
            caselle.Dispose()

            Dim client As Rebex.Net.Smtp = Nothing
            Dim mail As New Rebex.Mail.MailMessage
            If Not casellaPec Is Nothing Then
                Try
                    client = Me.ConfigureSmtp(casellaPec)
                Catch ex As Exception
                    Throw New ApplicationException(ex.Message)
                End Try


                mail.From = casellaPec.Email
                mail.To.Add(email.Mittente)


                Dim subject As String = fattura.Oggetto

                'SOSTITUISCO TUTTO TRANNE I CARATTERI SPECIFICATI DAL RANGE 
                subject = System.Text.RegularExpressions.Regex.Replace(subject, "[^\u0020-\u00FF]", " ")


                Dim riferimentoProtocollo As String = String.Empty

                riferimentoProtocollo = "Prot. N. " & registrazione.NumeroProtocollo.Value.ToString & " del " & registrazione.DataImmissione.Value.ToShortDateString & " - "
                mail.Subject = riferimentoProtocollo & "Notifica (Rif. " & subject & ")"

                mail.BodyText = ""

                Dim mailAttach As New Rebex.Mail.Attachment(fullPathFattura)
                Dim nomefileNotifica = IO.Path.GetFileName(fullPathFattura)

                Dim identificativoSDI = fattura.IdentificativoSdI & "_"


                If nomefileNotifica.StartsWith(identificativoSDI) Then
                    nomefileNotifica = nomefileNotifica.Remove(0, identificativoSDI.Length)
                End If
               
                mailAttach.FileName = nomefileNotifica
                mail.Attachments.Add(mailAttach)
                '***********************************************************************************************

                '***********************************************************************************************
                'INVIO L'EMAIL
                '***********************************************************************************************
                Try
                    client.Timeout = 0
                    client.Send(mail)
                    client.Disconnect()
                Catch ex As Exception
                    Throw New ApplicationException("Impossibile inviare l'email per il seguente motivo:" & vbCrLf & ex.Message)
                End Try


                '************************************
                'SALVO L'EMAIL SU DISCO
                '************************************

                Dim nomeEmail As String = Guid.NewGuid.ToString & ".eml"
                Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & email.PercorsoRelativo & nomeEmail
                If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata")) Then
                    IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata"))
                End If
                If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & email.PercorsoRelativo) Then
                    IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & email.PercorsoRelativo)
                End If

                mail.Save(mailBoxPath, Rebex.Mail.MailFormat.Mime)


                '************************************
                'INSERISCO L'EMAIL NEL DB
                '************************************
                Dim emailsInviate As New ParsecPro.EmailRepository

                Dim emailInviata As ParsecPro.Email = emailsInviate.CreateFromInstance(Nothing)
                Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
                emailInviata.IdCasella = email.IdCasella
                emailInviata.Inviata = True
                emailInviata.IdUtente = utente.Id
                emailInviata.DataInvio = Now
                emailInviata.Corpo = mail.BodyText
                'emailInviata.Oggetto = netMailMessage.Subject
                emailInviata.Oggetto = mail.Subject
                emailInviata.Destinatari = email.Mittente
                emailInviata.PercorsoRelativo = String.Format("\{0}\", Now.Year)
                emailInviata.NomeFileEml = nomeEmail


                emailInviata.NumeroProtocollo = fattura.NumeroProtocollo.Value
                emailInviata.AnnoProtocollo = fattura.AnnoProtocollo.Value

                emailInviata.MessaggioId = mail.MessageId.Id
                emailInviata.TipoProtocollo = registrazione.TipoRegistrazione

                emailsInviate.Save(emailInviata)

                Return emailInviata
            Else
                Throw New ApplicationException("La casella PEC con id: " & email.IdCasella.ToString & " non esiste!")
            End If
        Else
            Throw New ApplicationException("L'email in arrivo con id: " & idEmail.ToString & " non esiste!")
        End If
        Return Nothing

    End Function

    Private Sub SaveEmail(message As Net.Mail.MailMessage, fullPath As String)
        Dim assembly As Reflection.Assembly = GetType(Net.Mail.SmtpClient).Assembly
        Dim mailWriterType As Type = assembly.GetType("System.Net.Mail.MailWriter")
        Using ms As New IO.MemoryStream
            Dim mailWriterContructor As ConstructorInfo = mailWriterType.GetConstructor(BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Type() {GetType(IO.Stream)}, Nothing)
            Dim mailWriter As Object = mailWriterContructor.Invoke(New Object() {ms})
            Dim sendMethod As MethodInfo = GetType(Net.Mail.MailMessage).GetMethod("Send", BindingFlags.Instance Or BindingFlags.NonPublic)
            sendMethod.Invoke(message, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Object() {mailWriter, True}, Nothing)
            ms.Position = 0
            Dim bytes As Byte() = ms.ToArray
            IO.File.WriteAllBytes(fullPath, bytes)
            Dim closeMethod As MethodInfo = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance Or BindingFlags.NonPublic)
            closeMethod.Invoke(mailWriter, BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Object() {}, Nothing)
        End Using
    End Sub

    Private Sub NotificaEsitoFattura()

        If Not ParsecUtility.SessionManager.ListaNotificheFatturazione Is Nothing Then

            Dim emailInviata As ParsecPro.Email = Nothing

            Dim notificaEsitoCommittente As ParsecPro.NotificaEsitoCommittente = Nothing
            If Not ParsecUtility.SessionManager.NotificaEsitoCommittente Is Nothing Then
                notificaEsitoCommittente = CType(ParsecUtility.SessionManager.NotificaEsitoCommittente, ParsecPro.NotificaEsitoCommittente)
                ParsecUtility.SessionManager.NotificaEsitoCommittente = Nothing
            End If

            Dim esito = ParsecUtility.SessionManager.ListaNotificheFatturazione
            Dim accettata = esito(0).ToUpper = "EC01"
            Dim nomefileNotifica As String = esito(1)

            Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = registrazioni.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault
            registrazioni.Dispose()

            If Not registrazione Is Nothing Then

                Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
                Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault

                If Not fattura Is Nothing Then

                    If Not String.IsNullOrEmpty(nomefileNotifica) Then
                        '***********************************************************************************************
                        'COMPONGO E INVIO L'EMAIL
                        '***********************************************************************************************
                        Dim fullPathFattura As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & nomefileNotifica
                        Try
                            'SIMULO UN'ECCEZIONE
                            'Dim a = 0
                            'Dim i = 10 \ a

                            emailInviata = Me.InviaEmail(fullPathFattura, fattura, registrazione)


                        Catch ex As Exception
                            Throw New ApplicationException(ex.Message)
                        End Try
                        '***********************************************************************************************

                        '***********************************************************************************************
                        'SALVO LA NOTIFICA
                        '***********************************************************************************************
                        Try

                            Dim notificaSdI As New ParsecPro.NotificaSdI
                            notificaSdI.MessaggioSdI = New ParsecPro.MessaggioSdI
                            notificaSdI.MessaggioSdI.Nomefile = nomefileNotifica
                            notificaSdI.MessaggioSdI.Direzione = CInt(ParsecPro.DirezioneMessaggioSdI.Trasmesso)
                            notificaSdI.MessaggioSdI.PercorsoRelativo = fattura.MessaggioSdI.PercorsoRelativo
                            notificaSdI.MessaggioSdI.IdEmail = emailInviata.Id
                            notificaSdI.MessaggioSdI.DataRicezioneInvio = Now
                            notificaSdI.IdFatturaElettronica = fattura.Id
                            notificaSdI.IdTipologiaNotificaSdI = ParsecPro.TipologiaMessaggioSdI.NotificaEsitoCessionarioCommittente 'EC
                            Dim notifiche As New ParsecPro.NotificaSdIRepository
                            notifiche.Save(notificaSdI)


                        Catch ex As Exception
                            If ex.InnerException Is Nothing Then
                                Throw New ApplicationException(ex.Message)
                            Else
                                Throw New ApplicationException(ex.InnerException.Message)
                            End If
                        End Try
                        '***********************************************************************************************

                        If accettata Then
                            fattura.IdStato = ParsecPro.StatoFattura.Accettata
                        Else
                            fattura.IdStato = ParsecPro.StatoFattura.Rifiutata
                        End If



                    Else
                        fattura.IdStato = ParsecPro.StatoFattura.Accettata

                    End If

                    fattureElettroniche.SaveChanges()





                    If Me.Action.Conditions.Count > 0 Then
                        Dim variableOutputParametro = Me.Action.Parameters.Where(Function(c) c.Direction = "Out").FirstOrDefault

                        Dim expression As String = variableOutputParametro.Nome & " == " & CInt(notificaEsitoCommittente.Esito).ToString
                        Select Case notificaEsitoCommittente.Esito
                            Case ParsecPro.TipologiaEsitoCommittente.Accettazione
                            Case ParsecPro.TipologiaEsitoCommittente.Rfiutato
                            Case ParsecPro.TipologiaEsitoCommittente.AccettazioneSenzaNotifica
                                'Dim residuo = (fattura.MessaggioSdI.DataRicezioneInvio.AddDays(16) - Now).Days
                                'If residuo <= 0 Then

                                'Else


                                'End If

                        End Select

                        'If notificaEsitoCommittente.Esito = ParsecPro.TipologiaEsitoCommittente.RifiutoParziale OrElse notificaEsitoCommittente.Esito = ParsecPro.TipologiaEsitoCommittente.Rfiutato Then
                        '    Me.NoteInterneTextBox.Text = "Estremi fattura rifiutata: " & notificaEsitoCommittente.RiferimentoNumeroFattura & "/" & notificaEsitoCommittente.RiferimentoAnnoFattura
                        'End If

                        Me.WriteTaskWithConditionsAndUpdateParent(expression)
                        Me.FirstClick = True
                    Else
                        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                    End If



                    ParsecUtility.SessionManager.ListaNotificheFatturazione = Nothing

                Else
                    Throw New ApplicationException("La fattura elettronica con n. prot/anno: " & registrazione.NumeroProtocollo & "/" & registrazione.DataImmissione.Value.Year.ToString & " non esiste!")
                End If

            Else
                Throw New ApplicationException("Il protocollo con id: " & idRegistrazione.ToString & " non esiste!")
            End If
        End If
    End Sub

    Private Sub AggiornaIterIOL()

        'TODO TESTARE CHE SOLO UNO DEI PARAMETRI SIA SETTATO
        Dim preparaDocumentoParametro = Me.Action.GetParameterByName("PreparaDocumento")

        Dim protocollazioneParametro = Me.Action.GetParameterByName("Protocollazione")
        Dim protocollazioneIntegrazioneParametro = Me.Action.GetParameterByName("ProtocollazioneIntegrazione")



        Dim inviaEmailParametro = Me.Action.GetParameterByName("InviaEmail")

        Dim richiediIntegrazioneParametro = Me.Action.GetParameterByName("RichiediIntegrazione")

        If Not richiediIntegrazioneParametro Is Nothing Then

            If Not ParsecUtility.SessionManager.DocumentoGenerico Is Nothing Then
                Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = CType(ParsecUtility.SessionManager.DocumentoGenerico, ParsecAdmin.DocumentoGenerico)
                Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                Dim doc As ParsecAdmin.DocumentoGenerico = documentiGenerici.CreateFromInstance(documentoGenerico)
                doc.IdIstanza = Me.TaskAttivo.IdIstanza
                documentiGenerici.SaveChanges()
                documentiGenerici.Dispose()
                ParsecUtility.SessionManager.DocumentoGenerico = Nothing


                Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim istanzePratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                If Not istanzePratica Is Nothing Then
                    istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.PreparazioneRichiestaIntegrazione
                    pratiche.SaveChanges()
                End If


                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
            End If

            Exit Sub
        End If


        '******************************************************************************************************************************************************************
        '1) AGGIORNO L'ITER DOPO AVER ESEGUITO IL TASK PREPARA DOCUMENTO
        '******************************************************************************************************************************************************************
        If Not preparaDocumentoParametro Is Nothing Then
            If Not ParsecUtility.SessionManager.DocumentoGenerico Is Nothing Then
                Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = CType(ParsecUtility.SessionManager.DocumentoGenerico, ParsecAdmin.DocumentoGenerico)
                Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                Dim doc As ParsecAdmin.DocumentoGenerico = documentiGenerici.CreateFromInstance(documentoGenerico)
                doc.IdIstanza = Me.TaskAttivo.IdIstanza
                documentiGenerici.SaveChanges()
                documentiGenerici.Dispose()
                ParsecUtility.SessionManager.DocumentoGenerico = Nothing
                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)

                '*****************************************************************************************************
                'AGGIORNO LO STATO SE L'UTENTE CONTINUA SENZA ATTENDERE LA PRESENTAZIONE DELL'INTEGRAZIONE
                '*****************************************************************************************************
                Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim istanzePratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                If Not istanzePratica Is Nothing Then
                    If istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.AttesaRichiestaIntegrazione Then
                        istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.Avviata
                        istanzePratica.DataRiattivazione = Now

                        Try
                            Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
                            Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
                            Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                            Dim sp As New ParsecWebServices.StatoPraticaWS(istanzePratica.NumeroPratica, utente.Username, ParsecAdmin.StatoIstanzaPraticaOnline.Avviata)
                            sp.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)
                            sp.Insert()
                        Catch ex As Exception
                            'TODO SCRIVERE NEL LOG
                        End Try


                    End If
                    pratiche.SaveChanges()

                End If
                pratiche.Dispose()


                '*****************************************************************************************************

            End If

            Exit Sub
        End If
        '******************************************************************************************************************************************************************

        '******************************************************************************************************************************************************************
        '2) AGGIORNO L'ITER DOPO AVER ESEGUITO IL TASK PROTOCOLLA
        '******************************************************************************************************************************************************************
        If Not protocollazioneParametro Is Nothing OrElse Not protocollazioneIntegrazioneParametro Is Nothing Then

            If Not ParsecUtility.SessionManager.Registrazione Is Nothing Then
                Dim protocollo As ParsecPro.Registrazione = ParsecUtility.SessionManager.Registrazione

                'Me.AggiornaIstanzaWorkflow(protocollo)
                Me.AggiornaDocumentoGenerico(protocollo)
                'Me.GetTask(Me.TaskAttivo.IdAttoreCorrente, Me.TaskAttivo.Id)

                Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim istanzePratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                If Not istanzePratica Is Nothing Then
                    istanzePratica.IdRegistrazione2 = protocollo.Id
                    pratiche.SaveChanges()
                End If
                pratiche.Dispose()

                ParsecUtility.SessionManager.Registrazione = Nothing

            End If
            Exit Sub
        End If
        '******************************************************************************************************************************************************************

        '******************************************************************************************************************************************************************
        '3) AGGIORNO L'ITER DOPO AVER ESEGUITO IL TASK INVIA EMAIL
        '******************************************************************************************************************************************************************
        If Not inviaEmailParametro Is Nothing Then
            If Not ParsecUtility.SessionManager.EmailInviata Is Nothing Then
                ParsecUtility.SessionManager.EmailInviata = Nothing
                Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim istanzePratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                If Not istanzePratica Is Nothing Then
                    Dim sospensione As Boolean = False


                    Dim statoIstanza = Me.Action.GetParameterByName("StatoIstanza")

                    If statoIstanza Is Nothing Then
                        'SE HO PREPARATO IL DOCUMENTO PER LA RICHIESTA DI INTEGRAZIONE
                        If istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.PreparazioneRichiestaIntegrazione Then
                            istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.AttesaRichiestaIntegrazione
                            'DATA INIZIO SOSPENSIONE DOPO N GIORNI 
                            istanzePratica.DataRichiestaIntegrazione = Now
                            sospensione = True
                        Else
                            istanzePratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.Chiusa
                        End If
                    Else
                        Dim stato = CType(statoIstanza.Valore, ParsecAdmin.StatoIstanzaPraticaOnline)
                        istanzePratica.IdStato = stato
                    End If


                    pratiche.SaveChanges()





                    'CHIUDO O SOSPENDO LA PRATICA
                    Dim idStato As ParsecAdmin.StatoIstanzaPraticaOnline = ParsecAdmin.StatoIstanzaPraticaOnline.Chiusa

                    If Not statoIstanza Is Nothing Then
                        idStato = CType(statoIstanza.Valore, ParsecAdmin.StatoIstanzaPraticaOnline)
                    Else
                        If sospensione Then
                            idStato = ParsecAdmin.StatoIstanzaPraticaOnline.AttesaRichiestaIntegrazione
                        End If
                    End If


                    Try
                        Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
                        Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")
                        Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
                        Dim sp As New ParsecWebServices.StatoPraticaWS(istanzePratica.NumeroPratica, utenteCorrente.Username, idStato)
                        sp.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)
                        sp.Insert()
                    Catch ex As Exception
                        'todo scrivere nel log
                    End Try



                    'AGGIORNO IL DOCUMENTO GENERICO
                    Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                    'Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing).FirstOrDefault
                    Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza AndAlso c.Stato Is Nothing And (c.Inviato = False Or c.Inviato Is Nothing)).FirstOrDefault

                    If Not documentoGenerico Is Nothing Then
                        documentoGenerico.Inviato = True
                        documentoGenerico.DataInvio = Now
                        documentiGenerici.SaveChanges()
                    End If
                    documentiGenerici.Dispose()

                End If
                pratiche.Dispose()
                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)

            End If
            Exit Sub
        End If
        '******************************************************************************************************************************************************************

    End Sub

    Private Sub AggiornaIterPRO()

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.SbloccaTasks(utenteCollegato.Id)
        Me.SbloccaRegistrazioni(utenteCollegato.Id)

        Select Case Me.Action.Type
            Case "CANCELLA"
                If Not ParsecUtility.SessionManager.Registrazione Is Nothing Then
                    ParsecUtility.SessionManager.Registrazione = Nothing
                    Dim istanze As New ParsecWKF.IstanzaRepository
                    istanze.AnnullaTask(Me.TaskAttivo.IdIstanza)
                    istanze.Dispose()
                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                End If
                Exit Sub
        End Select



        'Nota: Utilizzare i parametri di processo per caratterizzare il metodo MODIFICA

        '*************************************************************************************************************************************
        'Leggo tutti i parametri 
        '*************************************************************************************************************************************
        Dim parametriProcesso = ParsecWKF.ModelloInfo.ReadParameters(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
        Dim preparaDocumentoParametro = parametriProcesso.Where(Function(c) c.Nome = "PreparaDocumento").FirstOrDefault
        Dim riferimentDocumentoParametro = parametriProcesso.Where(Function(c) c.Nome = "RiferimentoDocumento").FirstOrDefault 'OUT
        Dim protocollazioneParametro = parametriProcesso.Where(Function(c) c.Nome = "Protocollazione").FirstOrDefault
        Dim firmaParametro = parametriProcesso.Where(Function(c) c.Nome = "Firma").FirstOrDefault
        Dim inviaEmailParametro = parametriProcesso.Where(Function(c) c.Nome = "InviaEmail").FirstOrDefault

        Dim notificaEsitoFatturaParametro = Me.Action.GetParameterByName("NotificaEsitoFattura")
        Dim esportaFatturaParametro = Me.Action.GetParameterByName("EsportaFattura")
        '*************************************************************************************************************************************

        If parametriProcesso.Count = 0 Then
            If Not ParsecUtility.SessionManager.Registrazione Is Nothing Then

                ParsecUtility.SessionManager.Registrazione = Nothing

                'SE L'ISTANZA CORRENTE E' STATA ANNULLATA
                Dim istanze As New ParsecWKF.IstanzaRepository

                Dim istanza = istanze.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
                If Not istanze Is Nothing Then
                    If istanza.IdStato = 3 Then

                        'Me.Action.Description = "FINE"

                        'Dim tasks As New ParsecWKF.TaskRepository
                        'Dim filtro As New ParsecWKF.TaskFiltro
                        'filtro.IdUtente = Me.TaskAttivo.IdAttoreCorrente
                        'Me.TaskAttivo = tasks.GetView(filtro).Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza).OrderBy(Function(c) c.Id).FirstOrDefault
                        '' Me.TaskAttivo.IdAttoreCorrente = Me.TaskAttivo.IdAttoreCorrente
                        'tasks.Dispose()

                        Me.RegistraButtonClick()

                        Exit Sub

                    End If
                End If


                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)

            End If
            Exit Sub
        End If

        If Not inviaEmailParametro Is Nothing Then
            If Not ParsecUtility.SessionManager.EmailInviata Is Nothing Then
                ParsecUtility.SessionManager.EmailInviata = Nothing
                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
            End If
            Exit Sub
        End If

        If Not protocollazioneParametro Is Nothing Then

            If Not ParsecUtility.SessionManager.Registrazione Is Nothing Then
                Dim protocollo As ParsecPro.Registrazione = ParsecUtility.SessionManager.Registrazione

                'Select Case protocollazioneParametro.Valore.ToUpper
                '    Case "INTERNO"
                '        'MEMORIZZO IL PROTOCOLLO PER IL QUALE NON E' STATO ANCORA AVVIATO L'ITER (IN SOSPESO )
                '        Dim parametri As New ParsecWKF.ParametriProcessoRepository
                '        Dim processo As New ParsecWKF.ParametroProcesso With {.IdProcesso = Me.TaskAttivo.IdIstanza, .Nome = "IDREGISTRAZIONEPROTOCOLLO", .Valore = protocollo.Id.ToString}
                '        parametri.Add(processo)
                '        parametri.SaveChanges()
                '    Case "", "PARTENZA"
                'End Select

                Me.AggiornaIstanzaWorkflow(protocollo)
                Me.AggiornaDocumentoGenerico(protocollo)
                Me.GetTask(Me.TaskAttivo.IdAttoreCorrente, Me.TaskAttivo.Id, Me.TaskAttivo.IdModulo)

                'todo generare il pdf e allegarlo al protocollo


                ParsecUtility.SessionManager.Registrazione = Nothing



            End If
            Exit Sub
        End If


        '************************************************************************************************
        'GESTIONE FATTURA ELETTRONICA
        '************************************************************************************************
        If Not notificaEsitoFatturaParametro Is Nothing Then
            Try
                Me.NotificaEsitoFattura()
            Catch ex As Exception
                If ex.InnerException Is Nothing Then
                    ParsecUtility.Utility.MessageBox("Si è verificato il seguente errore: " & vbCrLf & ex.Message, False)
                Else
                    ParsecUtility.Utility.MessageBox("Si è verificato il seguente errore: " & vbCrLf & ex.InnerException.Message, False)
                End If
            End Try
            Exit Sub
        End If

        'If Not esportaFatturaParametro Is Nothing Then
        '    If Not ParsecUtility.SessionManager.FatturaEsportata Is Nothing Then
        '        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
        '    End If
        '    ParsecUtility.SessionManager.FatturaEsportata = Nothing
        '    Exit Sub
        'End If
        '************************************************************************************************

        If Not preparaDocumentoParametro Is Nothing Then
            If Not ParsecUtility.SessionManager.DocumentoGenerico Is Nothing Then
                Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = CType(ParsecUtility.SessionManager.DocumentoGenerico, ParsecAdmin.DocumentoGenerico)
                Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                Dim doc As ParsecAdmin.DocumentoGenerico = documentiGenerici.CreateFromInstance(documentoGenerico)
                doc.IdIstanza = Me.TaskAttivo.IdIstanza
                documentiGenerici.SaveChanges()
                documentiGenerici.Dispose()
                ParsecUtility.SessionManager.DocumentoGenerico = Nothing
                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
            End If
            Exit Sub
        End If


        If Not ParsecUtility.SessionManager.Pratica Is Nothing Then
            ParsecUtility.SessionManager.Pratica = Nothing
            Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
        End If

    End Sub


    Private Sub FirmaMassivaDocumentiGenerici()
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato

        Dim datiSrc As New ParsecAdmin.DatiFirmaSrc
        Dim signParameters As New ParsecAdmin.SignParameters

        Dim pathFileToSign As String = String.Empty

        If Me.FileToSignDictionary Is Nothing Then
            Me.FileToSignDictionary = New Dictionary(Of Integer, InfoFirma)
        End If

        For Each taskCorrente In Me.TaskAttivi
            Me.TaskAttivo = taskCorrente
            Dim allegati As New ParsecPro.AllegatiRepository
            Dim allegatoDaFirmare = allegati.GetAllegatiProtocollo(taskCorrente.IdDocumento).Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
            allegati.Dispose()
            If Not allegatoDaFirmare Is Nothing Then
                pathFileToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocument"), allegatoDaFirmare.PercorsoRelativo.Replace("\", ""), allegatoDaFirmare.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegatoDaFirmare.NomeFile)
            End If
            Me.FileToSignDictionary.Add(taskCorrente.Id, New InfoFirma With {.NomeFile = allegatoDaFirmare.NomeFile, .EntitaId = taskCorrente.IdDocumento, .Anno = 0})

            datiSrc.RowInfos.Add(taskCorrente.Id, pathFileToSign)

            Dim idDestinatario As Integer = Me.GetIdDestinatario()
            Me.ElencoDestinatariDictionary.Add(taskCorrente.Id, idDestinatario)

        Next

        Dim datiInput As New DatiFirma
        datiInput.RemotePathToSign = ""
        datiInput.RemotePathCertificate = ""
        datiInput.FunctionType = FunctionType.MultiSignDocument
        datiInput.CertificateId = certificateId
        datiInput.Provider = ProviderType.PkNet

        Dim data As String = signParameters.CreaDataSource(datiInput, datiSrc)

        ' Me.RegistraScriptPersistenzaVisibilitaPannello()
        ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaProtocolloInserisciDocumentoFirmatoButton.ClientID, Me.signerOutputHidden.ClientID, False, False)

    End Sub

    Private Sub FirmaPRO(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        'FIRMO IL DOCUMENTO GENERICO E LO ALLEGO AL PROTOCOLLO
        Me.FirmaDocumentoGenerico(Me.TaskAttivo.IdDocumento)
    End Sub

    Private Sub FirmaIOL(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        'FIRMO IL DOCUMENTO GENERICO E LO ALLEGO AL PROTOCOLLO
        Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
        Dim pratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
        If Not pratica Is Nothing Then
            If pratica.IdRegistrazione2.HasValue Then
                Me.FirmaDocumentoGenerico(pratica.IdRegistrazione2.Value)
            End If
        End If
        pratiche.Dispose()
    End Sub



    '*********************************************************************************************************
    'FIRMO IL DOCUMENTO GENERICO
    '*********************************************************************************************************
    Private Sub FirmaDocumentoGenerico(ByVal idRegistrazione As Integer)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'Sto eseguendo l'operazione in delega
        If Me.TaskAttivo.IdAttoreCorrente <> utenteCollegato.Id Then
            Dim abilitatoFirmaInDelega As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AbilitaFirmaInDelega)).FirstOrDefault Is Nothing
            If Not abilitatoFirmaInDelega Then


                Dim message As New StringBuilder
                message.AppendLine("Attenzione!")
                message.AppendLine("Non si possiedono le autorizzazioni per poter firmare in luogo di " & utenteCollegato.Username & ".")
                ParsecUtility.Utility.MessageBox(message.ToString, False)

                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                'Me.EnableUiHidden.Value = "Abilita"
                Exit Sub
            End If
        Else

            Dim abilitatoFirmaDigitale As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AbilitaFirmaDigitale)).FirstOrDefault Is Nothing
            If Not abilitatoFirmaDigitale Then
                Dim message As New StringBuilder
                message.AppendLine("Attenzione!")
                message.AppendLine("Non si possiedono le autorizzazioni per poter firmare.")
                ParsecUtility.Utility.MessageBox(message.ToString, False)
                Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                Exit Sub
            End If

        End If



        'Documento Primario
        Dim allegati As New ParsecPro.AllegatiRepository
        Dim allegatoDaFirmare = allegati.GetAllegatiProtocollo(idRegistrazione).Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
        allegati.Dispose()

        If Not allegatoDaFirmare Is Nothing Then
            Dim pathFileToSign As String = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocument"), allegatoDaFirmare.PercorsoRelativo.Replace("\", ""), allegatoDaFirmare.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegatoDaFirmare.NomeFile)

            Dim certificateId As String = utenteCollegato.IdCertificato

            Dim provider As ParsecAdmin.ProviderType = CType(utenteCollegato.IdProviderFirma, ParsecAdmin.ProviderType)
            If provider = ProviderType.Nessuno Then
                provider = ProviderType.PkNet 'Valore predefinito
            End If

            Dim funzione As FunctionType = FunctionType.SignDocument

            Dim abilitaFirmaElettronica As Boolean = Not Me.Action.GetParameterByName("AbilitaFirmaElettronica") Is Nothing
            Dim remotePathCertificate As String = String.Empty

            If abilitaFirmaElettronica Then
                funzione = FunctionType.WeakSignDocument
                provider = ProviderType.CryptoApi
                remotePathCertificate = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeCertificati"), utenteCollegato.NomefileCertificato)
            End If


            Dim functionName As String = "SignDocument"


            Dim signParameters As New ParsecAdmin.SignParameters
            Dim datiInput As New ParsecAdmin.DatiFirma With {
                .RemotePathToSign = pathFileToSign,
                .Provider = provider,
                .FunctionName = functionName,
                .CertificateId = certificateId,
                .FunctionType = funzione,
                .RemotePathCertificate = remotePathCertificate
            }

            Dim data As String = signParameters.CreaDataSource(datiInput)

            Me.RegistraScriptPersistenzaVisibilitaPannello()

            ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaProtocolloInserisciDocumentoFirmatoButton.ClientID, Me.signerOutputHidden.ClientID, False, False)

            Me.Nomefile = allegatoDaFirmare.NomeFile

        End If



    End Sub

    '*********************************************************************************************************
    'AGGIUNGO IL DOCUMENTO GENERICO FIRMATO AGLI ALLEGATI DEL PROTOCOLLO
    '*********************************************************************************************************
    Protected Sub AggiornaProtocolloInserisciDocumentoFirmatoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaProtocolloInserisciDocumentoFirmatoButton.Click
        'If Me.Action.Conditions.Count > 0 Then

        '    Dim variableOutputParametro = Me.Action.Parameters.Where(Function(c) c.Direction = "Out").FirstOrDefault
        '    Dim expression As String = String.Empty

        '    Select Case Me.signerOutputHidden.Value.ToUpper
        '        Case "OK"

        '            If Not Me.Nomefile Is Nothing Then

        '                Me.AllegatoDocumentoGenericoFirmatoProcollo(Me.Nomefile)
        '                Me.Nomefile = Nothing
        '                expression = variableOutputParametro.Nome & " == 1"
        '            End If

        '        Case "ERRORE"

        '            expression = variableOutputParametro.Nome & " == 0"

        '    End Select

        '    Me.WriteTaskWithConditionsAndUpdateParent(expression)
        '    Me.signerOutputHidden.Value = String.Empty

        'Else


        Select Case Me.signerOutputHidden.Value.ToUpper
            Case "OK"

                'TODO TESTARE PARAMETRO: AllegaDocumentoFirmato
                If Not Me.OperazioneMassiva Then
                    If Not Me.Nomefile Is Nothing Then

                        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
                            Me.AllegatoDocumentoGenericoFirmatoProcollo(Me.TaskAttivo.IdDocumento, Me.Nomefile)
                        End If

                        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.IOL Then
                            'GESTIONE ISTANZE ONLINE
                            Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
                            Dim pratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                            pratiche.Dispose()
                            If Not pratica Is Nothing Then
                                If pratica.IdRegistrazione2.HasValue Then
                                    Me.AllegatoDocumentoGenericoFirmatoProcollo(pratica.IdRegistrazione2.Value, Me.Nomefile)
                                End If
                            End If
                        End If

                        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
                            Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
                            Dim segnalazione = segnalazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                            If Not segnalazione Is Nothing Then
                                If segnalazione.IdRegistrazione.HasValue Then
                                    Me.AllegatoDocumentoGenericoFirmatoProcollo(segnalazione.IdRegistrazione.Value, Me.Nomefile)
                                End If
                            End If
                            segnalazioni.Dispose()
                        End If

                        Me.Nomefile = Nothing
                        Dim idDestinatario As Integer = Me.GetIdDestinatario()
                        'Chiudo la pagina e aggiorno la griglia della pagina chiamante.
                        Me.FirstClick = True
                        Me.WriteTaskAndUpdateParent(idDestinatario)
                    End If
                End If

            Case "ERRORE"
                'ABILITO L'INTERFACCIA
                Me.EnableUiHidden.Value = "Abilita"

            Case Else

                'TESTARE GESTIONE ISTANZE ONLINE FIRMA MASSIVA E SEGNALAZIONI


                '*******************************************************************************
                'Leggo i valori restituiti dalla firma massiva
                '*******************************************************************************
                Dim output = System.Convert.FromBase64String(Me.signerOutputHidden.Value)
                Dim ms As New IO.MemoryStream(output)
                Dim ds As New DataSet
                ds.ReadXml(ms)
                Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
                Dim operazione As String = Me.Action.Description.ToUpper

                For Each row As DataRow In ds.Tables(0).Rows
                    Dim idTask As Integer = CInt(row("RowId"))
                    Dim result As Boolean = CBool(row("Result"))
                    Me.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = idTask).FirstOrDefault

                    If result Then
                        Dim item = Me.FileToSignDictionary.Where(Function(c) c.Key = idTask).FirstOrDefault
                        Dim infoFirma As InfoFirma = item.Value
                        Dim fileToSign = infoFirma.NomeFile
                        Dim idRegistrazione = infoFirma.EntitaId
                        Me.AllegatoDocumentoGenericoFirmatoProcollo(idRegistrazione, fileToSign)
                        Dim idDestinatario = Me.ElencoDestinatariDictionary.Where(Function(c) c.Key = idTask).FirstOrDefault.Value
                        Me.WriteTask(idDestinatario, operazione, notificato)
                    End If
                Next

                Me.FirstClick = False
                'NON FUNZIONA SU CHROME
                Me.RegistraButtonClick()

                ''Chiudo la pagina e aggiorno la griglia della pagina chiamante.
                'Me.WriteAllTasksAndUpdateParent(Me.ElencoDestinatariDictionary)
                Me.ElencoDestinatariDictionary = Nothing
                Me.FileToSignDictionary = Nothing


                'For Each item As KeyValuePair(Of Integer, InfoFirma) In Me.FileToSignDictionary
                '    Dim infoFirma As InfoFirma = item.Value
                '    Dim fileToSign = infoFirma.NomeFile
                '    Dim idRegistrazione = infoFirma.EntitaId
                '    Me.AllegatoDocumentoGenericoFirmatoProcollo(idRegistrazione, fileToSign)
                'Next

                'Me.FileToSignDictionary = Nothing
                'Dim idDestinatario As Integer = Me.GetIdDestinatario()
                'Me.WriteAllTasksAndUpdateParent(idDestinatario)

        End Select
        ' End If


    End Sub

    '*********************************************************************************************************
    'AGGIORNO IL CAMPO UTENTE 'PROTOCOLLO' DEL DOCUMENTO GENERICO ASSOCIATO AGLI ALLEGATI DEL PROTOCOLLO
    '*********************************************************************************************************
    Public Sub AggiornaDocumentoGenerico(ByVal registrazione As ParsecPro.Registrazione)

        '*******************************************************************************************************************
        'GESTIONE DOCUMENTO RISPOSTA NON GENERATO DAL SISTEMA SEP MA ALLEGATO
        '*******************************************************************************************************************
        Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
        Dim documentoGenerico = documentiGenerici.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza).FirstOrDefault
        documentiGenerici.Dispose()

        If Not documentoGenerico Is Nothing Then
            If Not documentoGenerico.GeneratoSistemaSEP Then
                Me.Nomefile = Guid.NewGuid.ToString & IO.Path.GetExtension(documentoGenerico.NomeFile)
                Me.DocumentoGenericoDaFileSystem = True
                ParsecUtility.Utility.ButtonClick(Me.AggiornaProtocolloButton.ClientID, False)
                Exit Sub
            End If
        End If

        '*******************************************************************************************************************

        'Documento Primario

        Dim allegati As New ParsecPro.AllegatiRepository
        Dim allegatoDaFirmare = allegati.GetAllegatiProtocollo(registrazione.Id).Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
        allegati.Dispose()

        If Not allegatoDaFirmare Is Nothing Then

            Dim srcHttpPath As String = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocument"), allegatoDaFirmare.PercorsoRelativo.Replace("\", ""), allegatoDaFirmare.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegatoDaFirmare.NomeFile)
            Dim copyHttpPath As String = String.Empty

            Me.Nomefile = Guid.NewGuid.ToString & ".odt" 'allegatoDaFirmare.NomeFile 

            Dim destHttpPath As String = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocument"), allegatoDaFirmare.PercorsoRelativo.Replace("\", ""), Me.Nomefile)
            Dim openofficeParameters As New OpenOfficeParameters
            Dim tables As New List(Of DataTable)

            Dim dtMapping As New DataTable("DatiCampiUtente")
            dtMapping.Columns.Add(New DataColumn("NomeCampo", GetType(System.String)))
            dtMapping.Columns.Add(New DataColumn("ValoreCampo", GetType(System.String)))
            Dim row As DataRow = dtMapping.NewRow
            row("NomeCampo") = "Protocollo"
            row("ValoreCampo") = registrazione.NumeroProtocollo
            dtMapping.Rows.Add(row)
            tables.Add(dtMapping)

            Dim datiInput As New DatiInput
            datiInput.SrcRemotePath = srcHttpPath
            datiInput.DestRemotePath = destHttpPath
            datiInput.CopyRemotePath = copyHttpPath
            datiInput.ShowWindow = False
            datiInput.FunctionName = "ProcessDocument"


            Dim datasource As String = openofficeParameters.CreateDataSource(New DatiCredenziali, datiInput, tables)
            Me.RegistraScriptPersistenzaVisibilitaPannello()
            ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(datasource, Me.AggiornaProtocolloButton.ClientID, Me.documentContentHidden.ClientID, False, False)

        End If



    End Sub

   

    Private Sub AllegatoDocumentoGenericoFirmatoProcollo(ByVal idRegistrazione As Integer, ByVal nomefile As String)
        'Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetQuery.Where(Function(c) c.Id = idRegistrazione).FirstOrDefault

        'Documento Primario
        Dim allegati As New ParsecPro.AllegatiRepository
        Dim allegatoDocumentoGenerico = allegati.GetAllegatiProtocollo(idRegistrazione).Where(Function(c) c.IdTipologiaDocumento = 1).FirstOrDefault
        allegati.Dispose()

        Dim nomefileP7m As String = IO.Path.GetFileNameWithoutExtension(nomefile) & ".pdf.p7m"



        '*******************************************************************************************************************
        'GESTIONE DOCUMENTO RISPOSTA NON GENERATO DAL SISTEMA SEP MA ALLEGATO
        '*******************************************************************************************************************
        Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
        Dim documentoGenerico = documentiGenerici.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza).FirstOrDefault
        documentiGenerici.Dispose()

        If Not documentoGenerico Is Nothing Then
            If Not documentoGenerico.GeneratoSistemaSEP Then
                nomefileP7m = IO.Path.GetFileNameWithoutExtension(nomefile) & IO.Path.GetExtension(nomefile) & ".p7m"
            End If
        End If



        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
        percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
        Dim srcPathDownload As String = percorsoRoot & allegatoDocumentoGenerico.PercorsoRelativo & allegatoDocumentoGenerico.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & nomefileP7m


        Dim allegato As New ParsecPro.Allegato
        allegato.NomeFile = nomefileP7m
        allegato.NomeFileTemp = nomefileP7m
        allegato.IdTipologiaDocumento = 0 'Allegato
        allegato.DescrizioneTipologiaDocumento = "Allegato"
        allegato.Oggetto = "Documento di risposta firmato"
        allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
        allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(srcPathDownload)
        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
        registrazioni.SaveAllegato(allegato, registrazione)
        registrazioni.Dispose()

        'RINOMINO IL FILE 

        Dim destPathDownload As String = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & nomefileP7m
        'IO.File.Move(srcPathDownload, destPathDownload)

        IO.File.Copy(srcPathDownload, destPathDownload, True)
        IO.File.Delete(srcPathDownload)
    End Sub

    Private Sub VisualizzaDocumentoGenericoInModifica()
        Dim queryString As New Hashtable
        queryString.Add("Mode", "Update")
        queryString.Add("obj", Me.AggiornaIterButton.ClientID)

        Dim parametriPagina As New Hashtable

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
            parametriPagina.Add("IdRegistrazioneIter", Me.TaskAttivo.IdDocumento)
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.IOL Then
            Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
            Dim pratica = pratiche.GetFullById(Me.TaskAttivo.IdDocumento)
            pratiche.Dispose()
            If Not pratica Is Nothing Then

                'Se l'istanza è stata respinta,riceverete una comunicazione contenente anche le motivazioni del diniego
                'e quindi le indicazioni degli eventuali documenti da sostituire

                Dim oggetto As String = String.Empty
                Dim codiceFiscale As String = String.Empty

                If Not pratica.Committente Is Nothing Then
                    codiceFiscale = pratica.Committente.CodiceFiscale
                Else
                    codiceFiscale = pratica.Richiedente.CodiceFiscale
                End If

                Dim richiediIntegrazioneParametro = Me.Action.GetParameterByName("RichiediIntegrazione")
                If richiediIntegrazioneParametro Is Nothing Then
                    oggetto = "Pratica n. " & pratica.NumeroPratica & " del " & pratica.DataPresentazione.ToShortDateString & " ( prot. n. " & pratica.NumeroProtocollo.ToString & " del " & "{0}" & " ) - C.F. " & codiceFiscale
                Else
                    oggetto = "Richiesta integrazione - Pratica n. " & pratica.NumeroPratica & " del " & pratica.DataPresentazione.ToShortDateString & " ( prot. n. " & pratica.NumeroProtocollo.ToString & " del " & "{0}" & " ) - C.F. " & codiceFiscale
                End If
                parametriPagina.Add("IdRegistrazioneIter", pratica.IdRegistrazione)

                parametriPagina.Add("Oggetto", oggetto)

            End If
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then

            Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository

            Dim segnalazione = segnalazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
            If Not segnalazione Is Nothing Then
                Dim oggetto As String = "Segnalazione di condotte illecite n. " & segnalazione.NumeroSeriale & " del " & segnalazione.DataCreazione.ToShortDateString
                parametriPagina.Add("Oggetto", oggetto)
            End If

            parametriPagina.Add("IdSegnalazione", Me.TaskAttivo.IdDocumento)

        End If


        parametriPagina.Add("Modulo", Me.TaskAttivo.IdModulo)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina


        Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
        ParsecUtility.Utility.ShowPopup(pageUrl, 920, 500, queryString, False)
    End Sub

    Private Sub VisualizzaProtocolloInModifica()
        Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
        Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
        'Dim queryString As New Hashtable
        Dim queryString As Hashtable = Me.SplitQueryString(pageUrl)
        queryString.Add("Mode", "Update")
        queryString.Add("Tipo", registrazione.TipoRegistrazione)
        queryString.Add("obj", Me.AggiornaIterButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("RegistrazioneInIter", registrazione)
        parametriPagina.Add("TaskAttivo", Me.TaskAttivo)

        Dim nomeParametro As String = "IdModelloWorkflowFatturaElettronica"
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName(nomeParametro, ParsecAdmin.TipoModulo.PRO)
        parametri.Dispose()
        If Not parametro Is Nothing Then
            'SE E' UNA FATTURA
            If parametro.Valore = Me.TaskAttivo.IdModello Then
                queryString.Add("Fattura", "1")
            End If
        End If

        If pageUrl.Contains("?") Then
            pageUrl = pageUrl.Substring(0, pageUrl.IndexOf("?"))
        End If

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 940, 600, queryString, False)
    End Sub

    Private Sub VisualizzaEmailInModifica()
        Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaIterButton.ClientID)
        Dim idRegistrazione As Integer = 0

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.PRO Then
            idRegistrazione = Me.TaskAttivo.IdDocumento
        End If

        Dim idCasella As Nullable(Of Integer) = Nothing
        Dim parametriPagina As New Hashtable

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.IOL Then
            Dim pratiche As New ParsecAdmin.IstanzaPraticaOnlineRepository
            Dim pratica = pratiche.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
            pratiche.Dispose()
            If Not pratica Is Nothing Then
                If pratica.IdRegistrazione2.HasValue Then
                    idRegistrazione = pratica.IdRegistrazione2.Value
                End If

                Dim statoIstanza = Me.Action.GetParameterByName("StatoIstanza")

                If statoIstanza Is Nothing Then
                    'SE HO PREPARATO IL DOCUMENTO PER LA RICHIESTA DI INTEGRAZIONE
                    If pratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.PreparazioneRichiestaIntegrazione Then
                        pratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.AttesaRichiestaIntegrazione
                    Else
                        pratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.Chiusa
                    End If
                Else
                    Dim stato = CType(statoIstanza.Valore, ParsecAdmin.StatoIstanzaPraticaOnline)
                    pratica.IdStato = stato
                End If

                If pratica.IdStato = ParsecAdmin.StatoIstanzaPraticaOnline.Chiusa Then
                    parametriPagina.Add("CorpoEmail", "La PRATICA è stata CHIUSA in data " & Now.ToShortDateString & ", verificare l'esito sul documento di risposta allegato")
                End If


            End If
            Dim emails As New ParsecPro.EmailArrivoRepository
            Dim email = emails.Where(Function(c) c.IdRegistrazione = pratica.IdRegistrazione).Select(Function(c) c.IdCasella)
            If email.Any Then
                idCasella = email.FirstOrDefault
            End If
            emails.Dispose()
        End If

        If Me.TaskAttivo.IdModulo = ParsecAdmin.TipoModulo.WBT Then
            Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
            Dim segnalazione = segnalazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento And Not c.IdRegistrazione Is Nothing).Select(Function(c) c.IdRegistrazione)
            If segnalazione.Any Then
                idRegistrazione = segnalazione.FirstOrDefault.Value
            End If
            segnalazioni.Dispose()
        End If

        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
        registrazioni.Dispose()

        parametriPagina.Add("RegistrazioneDaInviare", registrazione)
        parametriPagina.Add("IdCasella", idCasella)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 600, queryString, False)

    End Sub


    Private Sub VisualizzaEsportaFattura()

        Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
        registrazioni.Dispose()

        If Not registrazione Is Nothing Then
            Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
            Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
            If Not fattura Is Nothing Then
                Dim queryString As New Hashtable
                Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
                queryString.Add("obj", Me.AggiornaIterButton.ClientID)
                queryString.Add("IdFatturaElettronica", fattura.Id.ToString)
                ParsecUtility.Utility.ShowPopup(pageUrl, 610, 150, queryString, False)
            End If
        End If

    End Sub

    Private Sub VisualizzaImpostaEsito()
        Dim queryString As New Hashtable
        Dim idRegistrazione As Integer = Me.TaskAttivo.IdDocumento
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(idRegistrazione)
        registrazioni.Dispose()
        Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
        Dim fattura = fattureElettroniche.Where(Function(c) c.AnnoProtocollo = registrazione.DataImmissione.Value.Year And c.NumeroProtocollo = registrazione.NumeroProtocollo).FirstOrDefault
        If Not fattura Is Nothing Then
            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
            queryString.Add("obj", Me.AggiornaIterButton.ClientID)
            queryString.Add("IdFatturaElettronica", fattura.Id.ToString)
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 710, queryString, False)
        End If
    End Sub

    Private Sub VisualizzaFattura()
         DirectCast(Me.Page, Object).VisualizzaFattura(Me.TaskAttivo)
        'Me.Page.GetType.InvokeMember("VisualizzaFattura", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {Me.TaskAttivo})
    End Sub

  
    Private Sub UploadFile(userName As String, password As String, ftpDestinationFilePath As String, localSourceFilePath As String)
        Dim request As FtpWebRequest = CreateFtpWebRequest(ftpDestinationFilePath & "/" & IO.Path.GetFileName(localSourceFilePath), userName, password, False)
        Dim buffer As Byte() = IO.File.ReadAllBytes(localSourceFilePath)
        request.Method = WebRequestMethods.Ftp.UploadFile
        request.ContentLength = buffer.Length
        Dim writer As IO.Stream = request.GetRequestStream
        writer.Write(buffer, 0, buffer.Length)
        writer.Close()
    End Sub



    Private Sub UploadFile(userName As String, password As String, ftpDestinationFilePath As String, localSourceFilePath As String, ByVal buffer As Byte())
        Dim request As FtpWebRequest = CreateFtpWebRequest(ftpDestinationFilePath & "/" & IO.Path.GetFileName(localSourceFilePath), userName, password, False)
        request.Method = WebRequestMethods.Ftp.UploadFile
        request.ContentLength = buffer.Length
        Dim writer As IO.Stream = request.GetRequestStream
        writer.Write(buffer, 0, buffer.Length)
        writer.Close()
    End Sub

    Private Function CreateFtpWebRequest(ftpDirectoryPath As String, userName As String, password As String, keepAlive As Boolean) As FtpWebRequest
        Dim request = CType(WebRequest.Create(New Uri(ftpDirectoryPath)), FtpWebRequest)
        request.Proxy = Nothing
        request.UsePassive = True
        request.UseBinary = True
        request.KeepAlive = keepAlive
        request.Credentials = New NetworkCredential(userName, password)
        Return request
    End Function


#End Region


#Region "GESTIONE PRATICA SUE"


    Private Sub NumeraPratica(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
        registrazioni.Dispose()

        If Not registrazione Is Nothing Then
            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)

            Dim queryString As Hashtable = Me.SplitQueryString(pageUrl)
            queryString.Add("IdRegistrazione", registrazione.Id)
            queryString.Add("DataProtocollo", registrazione.DataImmissione)
            queryString.Add("Iter", "1")
            queryString.Add("Mode", "NewPratica")
            queryString.Add("obj", Me.AggiornaIterButton.ClientID)

            If pageUrl.Contains("?") Then
                pageUrl = pageUrl.Substring(0, pageUrl.IndexOf("?"))
            End If

            Dim parametriPagina As New Hashtable
            parametriPagina.Add("IdDocumentoIter", 1)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 1080, 600, queryString, False)
        End If

    End Sub


    Private Sub AggiornaIterSUE()
        Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
    End Sub


    Private Sub LavoraPratica(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        Dim GeneraLetteraRdP = Me.Action.GetParameterByName("LetteraRdP")
        Dim ControllaLetteraRdP = Me.Action.GetParameterByName("ControllaRdP")
        Dim ProtocollaLetteraRdP = Me.Action.GetParameterByName("ProtocollaRdP")
        Dim FirmaLetteraRdP = Me.Action.GetParameterByName("FirmaRdP")
        Dim InviaLetteraRdP = Me.Action.GetParameterByName("InviaRdP")

        Dim GeneraLetteraIntegrazione = Me.Action.GetParameterByName("LetteraIntegrazione")
        Dim ControllaLetteraIntegrazione = Me.Action.GetParameterByName("ControllaIntegrazione")
        Dim ProtocollaLetteraIntegrazione = Me.Action.GetParameterByName("ProtocollaIntegrazione")
        Dim FirmaIntegrazione = Me.Action.GetParameterByName("FirmaIntegrazione")
        Dim InviaIntegrazione = Me.Action.GetParameterByName("InviaIntegrazione")

        Dim GeneraLetteraIntegrazionePrerilascio = Me.Action.GetParameterByName("LetteraIntegrazionePrerilascio")
        Dim ControllaLetteraIntegrazionePrerilascio = Me.Action.GetParameterByName("ControllaIntegrazionePrerilascio")
        Dim ProtocollaLetteraIntegrazionePrerilascio = Me.Action.GetParameterByName("ProtocollaIntegrazionePrerilascio")
        Dim FirmaIntegrazionePrerilascio = Me.Action.GetParameterByName("FirmaIntegrazionePrerilascio")
        Dim InviaIntegrazionePrerilascio = Me.Action.GetParameterByName("InviaIntegrazionePrerilascio")

        Dim GeneraLetteraPareri = Me.Action.GetParameterByName("LetteraPareri")
        Dim ControllaLetteraPareri = Me.Action.GetParameterByName("ControllaPareri")
        Dim ProtocollaLetteraPareri = Me.Action.GetParameterByName("ProtocollaPareri")
        Dim FirmaPareri = Me.Action.GetParameterByName("FirmaPareri")
        Dim InviaPareri = Me.Action.GetParameterByName("InviaPareri")

        Dim GeneraLetteraSospensione = Me.Action.GetParameterByName("LetteraSospensione")
        Dim ControllaLetteraSospensione = Me.Action.GetParameterByName("ControllaSospensione")
        Dim ProtocollaLetteraSospensione = Me.Action.GetParameterByName("ProtocollaSospensione")
        Dim FirmaSospensione = Me.Action.GetParameterByName("FirmaSospensione")
        Dim InviaSospensione = Me.Action.GetParameterByName("InviaSospensione")

        Dim GeneraLetteraPreavvisoDiniego = Me.Action.GetParameterByName("LetteraPreavviso")
        Dim ControllaLetteraPreavvisoDiniego = Me.Action.GetParameterByName("ControllaPreavviso")
        Dim ProtocollaLetteraPreavvisoDiniego = Me.Action.GetParameterByName("ProtocollaPreavviso")
        Dim FirmaPreavvisoDiniego = Me.Action.GetParameterByName("FirmaPreavviso")
        Dim InviaPreavvisoDiniego = Me.Action.GetParameterByName("InviaPreavviso")

        Dim GeneraLetteraPermesso = Me.Action.GetParameterByName("LetteraPermesso")
        Dim ControllaLetteraPermesso = Me.Action.GetParameterByName("ControllaPermesso")
        Dim ProtocollaLetteraPermesso = Me.Action.GetParameterByName("ProtocollaPermesso")
        Dim FirmaPermesso = Me.Action.GetParameterByName("FirmaPermesso")
        Dim InviaPermesso = Me.Action.GetParameterByName("InviaPermesso")


        Dim AssociaProtocolloIntegrazione = Me.Action.GetParameterByName("AssociaProtocollo")



        Dim Azione As String = ""
        If Not IsNothing(GeneraLetteraRdP) Then
            Azione = GeneraLetteraRdP.Nome
        End If
        If Not IsNothing(GeneraLetteraIntegrazione) Then
            Azione = GeneraLetteraIntegrazione.Nome
        End If
        If Not IsNothing(GeneraLetteraIntegrazionePrerilascio) Then
            Azione = GeneraLetteraIntegrazionePrerilascio.Nome
        End If
        If Not IsNothing(GeneraLetteraPareri) Then
            Azione = GeneraLetteraPareri.Nome
        End If

        If Not IsNothing(GeneraLetteraSospensione) Then
            Azione = GeneraLetteraSospensione.Nome
        End If

        If Not IsNothing(GeneraLetteraPreavvisoDiniego) Then
            Azione = GeneraLetteraPreavvisoDiniego.Nome
        End If

        If Not IsNothing(GeneraLetteraPermesso) Then
            Azione = GeneraLetteraPermesso.Nome
        End If


        If Not IsNothing(FirmaLetteraRdP) Then
            Azione = FirmaLetteraRdP.Nome
        End If
        If Not IsNothing(FirmaIntegrazione) Then
            Azione = FirmaIntegrazione.Nome
        End If
        If Not IsNothing(FirmaIntegrazionePrerilascio) Then
            Azione = FirmaIntegrazionePrerilascio.Nome
        End If
        If Not IsNothing(FirmaPareri) Then
            Azione = FirmaPareri.Nome
        End If

        If Not IsNothing(FirmaSospensione) Then
            Azione = FirmaSospensione.Nome
        End If

        If Not IsNothing(FirmaPreavvisoDiniego) Then
            Azione = FirmaPreavvisoDiniego.Nome
        End If

        If Not IsNothing(FirmaPermesso) Then
            Azione = FirmaPermesso.Nome
        End If


        If Not IsNothing(ControllaLetteraRdP) Then
            Azione = ControllaLetteraRdP.Nome
        End If
        If Not IsNothing(ControllaLetteraIntegrazione) Then
            Azione = ControllaLetteraIntegrazione.Nome
        End If
        If Not IsNothing(ControllaLetteraIntegrazionePrerilascio) Then
            Azione = ControllaLetteraIntegrazionePrerilascio.Nome
        End If
        If Not IsNothing(ControllaLetteraPareri) Then
            Azione = ControllaLetteraPareri.Nome
        End If
        If Not IsNothing(ControllaLetteraSospensione) Then
            Azione = ControllaLetteraSospensione.Nome
        End If
        If Not IsNothing(ControllaLetteraPreavvisoDiniego) Then
            Azione = ControllaLetteraPreavvisoDiniego.Nome
        End If
        If Not IsNothing(ControllaLetteraPermesso) Then
            Azione = ControllaLetteraPermesso.Nome
        End If

        If Not IsNothing(AssociaProtocolloIntegrazione) Then
            Azione = AssociaProtocolloIntegrazione.Nome
        End If

        Dim idpratica As Integer = -1
        Dim istanzaOL As String = String.Empty
        Select Case Me.TaskAttivo.IdModulo
            Case 2


                Dim registrazioni As New ParsecPro.RegistrazioniRepository
                Dim registrazione As ParsecPro.Registrazione = registrazioni.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                registrazioni.Dispose()


                Dim IdProt As Integer = registrazione.Id
                Dim AllegatoDD = (New ParsecPro.AllegatiRepository())
                Dim documenti = (New ParsecPro.DocumentoViewRepository(AllegatoDD.Context)).GetQuery



                Dim istanzePraticaOnline As New ParsecAdmin.IstanzaPraticaOnlineRepository
                Dim istanzePratica = istanzePraticaOnline.Where(Function(c) c.IdRegistrazione = IdProt).FirstOrDefault


                Dim AllegatoD = AllegatoDD.Where(Function(c) c.IdRegistrazione = IdProt)

                Dim view = From allegato In AllegatoD
                                         Join documento In documenti
                                         On allegato.Id Equals documento.Id
                                         Select allegato, documento

                Dim allegati = view.AsEnumerable.Select(Function(c) New ParsecPro.Allegato With {
                                                           .Id = c.allegato.Id,
                                                           .IdRegistrazione = c.allegato.IdRegistrazione,
                                                           .Impronta = c.documento.Impronta,
                                                           .PercorsoRelativo = c.documento.PercorsoRelativo,
                                                           .Oggetto = c.allegato.Oggetto,
                                                           .NomeFile = c.documento.NomeFileOriginale
                                                           }).ToList


                For h As Integer = 0 To allegati.Count - 1


                    Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                    Dim localPathTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentitemp")

                    Dim file As New IO.FileInfo(localPath & allegati(h).PercorsoRelativo & allegati(h).Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & allegati(h).NomeFile)
                    Dim innerMessage As Rebex.Mail.MailMessage = Nothing
                    Dim ms As IO.MemoryStream = Nothing
                    ' Dim attEmail As New Rebex.Mail.Attachment
                    Dim Email As New Rebex.Mail.MailMessage
                    Dim message As New Rebex.Mail.MailMessage
                    message.Settings.IgnoreInvalidTnefMessages = True

                    Dim segnaturaAttachment As Rebex.Mail.Attachment = Nothing


                    message.Load(file.FullName)
                    Dim attEmail = message.Attachments.Where(Function(c) c.FileName.ToLower.EndsWith(".eml")).FirstOrDefault
                    If Not attEmail Is Nothing Then
                        innerMessage = New Rebex.Mail.MailMessage
                        innerMessage.Settings.IgnoreInvalidTnefMessages = True
                        ms = New IO.MemoryStream
                        attEmail.Save(ms)
                        ms.Position = 0
                        innerMessage.Load(ms)
                        Email = innerMessage
                        ' If Not istanzePratica Is Nothing Then
                        segnaturaAttachment = innerMessage.Attachments.Where(Function(c) c.FileName.ToLower.Contains("richiestaintegrazione") AndAlso c.FileName.ToLower.EndsWith(".xml")).FirstOrDefault
                        'End If
                    Else
                        Email = message
                    End If

                    '*************************** leggo i dati dall'xml dell'istanza inviata ***************************
                    If Not segnaturaAttachment Is Nothing Then
                        ms = New IO.MemoryStream
                        segnaturaAttachment.Save(ms)
                        ms.Position = 0
                        Try
                            Dim xml = XElement.Load(ms)
                            If Not xml.Element("numeroPratica") Is Nothing Then
                                istanzaOL = xml.Element("numeroPratica").Value
                            End If
                            ' Dim registrazione As ParsecPro.Registrazione = LeggiIstanzaOnline(istanza)


                        Catch ex As Exception
                        End Try
                    End If
                    '******************************************************************




                Next


                
        End Select

        'If Not documento Is Nothing Then
        Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)

        Dim queryString As Hashtable = Me.SplitQueryString(pageUrl)
        queryString.Add("IdPraticaWKF", idpratica)
        queryString.Add("IdProtocolloWKF", Me.TaskAttivo.IdDocumento)
        'queryString.Add("DataProtocollo", documento.DataProtocollo)
        queryString.Add("Iter", "1")
        queryString.Add("Mode", "UpdatePratica")
        queryString.Add("ParametroAzione", Azione)
        queryString.Add("obj", Me.AggiornaIterButton.ClientID)
        If pageUrl.Contains("?") Then
            pageUrl = pageUrl.Substring(0, pageUrl.IndexOf("?"))
        End If

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdDocumentoIter", 1)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 1080, 600, queryString, False)


    End Sub


#End Region


    Private Sub AggiornaIterCSRA()
        Select Case Me.Action.Type
            Case "MODIFICA"
                If Not ParsecUtility.SessionManager.SchedaControlloSuccessivoRegolaritaAmministrativa Is Nothing Then
                    ParsecUtility.SessionManager.SchedaControlloSuccessivoRegolaritaAmministrativa = Nothing
                    Dim idDestinatario As Integer = Me.GetIdDestinatario()

                    'Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                    Me.WriteTaskAndUpdateParent(idDestinatario)
                Else
                    'CHIUDO LA FINESTRA PER AGGIORNARE IL RIFERIMENTO AL DOCUMENTO
                    'Me.RegistraButtonClick()
                End If

            Case "FIRMA"
                Select Case Me.signerOutputHidden.Value.ToUpper
                    Case "OK"

                        '***********************************************************************************************************************************************
                        'AGGIORNO LA SCHEDA
                        '***********************************************************************************************************************************************
                        Dim idAtto As Integer = Me.TaskAttivo.IdDocumento
                        Dim documenti As New ParsecAtt.DocumentoRepository
                        Dim documento = documenti.Where(Function(c) c.Id = idAtto).FirstOrDefault
                        documenti.Dispose()

                        If Not documento Is Nothing Then
                            Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
                            Dim scheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa = schede.Where(Function(c) c.NumeroAtto = documento.ContatoreGenerale And c.DataAtto = documento.Data).FirstOrDefault

                            Dim nomefileP7m As String = IO.Path.GetFileNameWithoutExtension(scheda.Nomefile) & ".pdf.p7m"
                            scheda.NomefileFirmato = nomefileP7m
                            schede.SaveChanges()
                            schede.Dispose()

                        End If
                        '***********************************************************************************************************************************************


                        Dim idDestinatario As Integer = Me.GetIdDestinatario()
                        Me.FirstClick = True

                        'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
                        Me.WriteTaskAndUpdateParent(idDestinatario)

                    Case "ERRORE"
                        'NIENTE
                    Case Else  'OPERAZIONE MASSIVA

                        '*******************************************************************************
                        'Leggo i valori restituiti dalla firma massiva
                        '*******************************************************************************
                        Dim output = System.Convert.FromBase64String(Me.signerOutputHidden.Value)
                        Dim ms As New IO.MemoryStream(output)
                        Dim ds As New DataSet
                        ds.ReadXml(ms)
                        Dim notificato As Boolean = (Me.Action.FromActor = Me.Action.ToActor)
                        Dim operazione As String = Me.Action.Description.ToUpper

                        For Each row As DataRow In ds.Tables(0).Rows
                            Dim idTask As Integer = CInt(row("RowId"))
                            Dim result As Boolean = CBool(row("Result"))
                            Me.TaskAttivo = Me.TaskAttivi.Where(Function(c) c.Id = idTask).FirstOrDefault

                           If result Then
                                Dim idDestinatario = Me.ElencoDestinatariDictionary.Where(Function(c) c.Key = idTask).FirstOrDefault.Value
                                Me.WriteTask(idDestinatario, operazione, notificato)
                            End If
                        Next

                        '**************************************************************************************
                        'NOTIFICO GLI ERRORI RISCONTRATI DURANTE LA VERIFICA MASSIVA DEL CORPO DEL DOCUMENTO
                        '**************************************************************************************
                        If Not String.IsNullOrEmpty(Me.MessaggioAvviso) Then
                            Me.RegistraScriptPersistenzaVisibilitaPannello()
                            ParsecUtility.Utility.MessageBox(Me.MessaggioAvviso, False)
                        End If
                        Me.MessaggioAvviso = Nothing
                        '**************************************************************************************

                        Me.FirstClick = False
                        'NON FUNZIONA SU CHROME
                        Me.RegistraButtonClick()

                  End Select

                Me.signerOutputHidden.Value = String.Empty

        End Select

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.SbloccaTasks(utenteCollegato.Id)
    End Sub


    Private Sub VisualizzaSchedaControlloSuccessivo()

        'MODIFICO LA SCHEDA CONTROLLO
        Dim queryString As New Hashtable
        Dim idAtto As Integer = Me.TaskAttivo.IdDocumento

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento = documenti.Where(Function(c) c.Id = idAtto).FirstOrDefault
        documenti.Dispose()
        If Not documento Is Nothing Then

            Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
            Dim scheda = schede.Where(Function(c) c.NumeroAtto = documento.ContatoreGenerale And c.DataAtto = documento.Data).FirstOrDefault
            schede.Dispose()
            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
            queryString.Add("obj", Me.AggiornaIterButton.ClientID)
            queryString.Add("IdSchedaControlloSuccessivoRegolaritaAmministrativa", scheda.Id.ToString)
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 710, queryString, False)
        End If
    End Sub


    Private Sub VisualizzaSegnalazione()

        'MODIFICO LA SEGNALAZIONE
        Dim queryString As New Hashtable
        Dim idDocumento As Integer = Me.TaskAttivo.IdDocumento

        Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
        Dim segnalazione = segnalazioni.Where(Function(c) c.Id = idDocumento).FirstOrDefault
        segnalazioni.Dispose()
        If Not segnalazione Is Nothing Then
            Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, Me.Action.Name, Me.TaskAttivo.NomeFileIter)
            queryString.Add("obj", Me.AggiornaIterButton.ClientID)
            queryString.Add("GuidSegnalazione", segnalazione.GuidSegnalazione)
            queryString.Add("Mode", "M")
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 710, queryString, False)
        End If

    End Sub


    Private Sub ModificaWBT(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        Dim preparaDocumentoParametro = Me.Action.GetParameterByName("PreparaDocumento")
        Dim protocollazioneParametro = Me.Action.GetParameterByName("Protocollazione")
        Dim inviaEmailParametro = Me.Action.GetParameterByName("InviaEmail")

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        '******************************************************************************************************************************************************************
        '1) ESEGUO IL TASK PREPARA DOCUMENTO
        '******************************************************************************************************************************************************************
        If Not preparaDocumentoParametro Is Nothing Then

            Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
            Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = Me.TaskAttivo.IdIstanza And c.Stato Is Nothing And (c.Inviato = False Or c.Inviato Is Nothing)).FirstOrDefault

            '***************************************************************************************
            'Se il documento generico esiste visualizzo il documento generico con ParsecOpenOffice
            '***************************************************************************************
            If Not documentoGenerico Is Nothing Then
                Dim modello = documentiGenerici.GetModello(documentoGenerico.IdModello)
                Me.VisualizzaDocumentoGenerico(documentoGenerico, modello)

            Else

                '***************************************************************************************
                'Apro la pagina per la generazione del documento generico.
                'Il salvattaggio esegue il click sul AggiornaIterButton  (persistenza stato del processo corrente)
                '***************************************************************************************

                Me.VisualizzaDocumentoGenericoInModifica()
            End If

            documentiGenerici.Dispose()

            Exit Sub

        End If


        '******************************************************************************************************************************************************************
        '2) ESEGUO IL TASK PROTOCOLLA
        '******************************************************************************************************************************************************************
        If Not protocollazioneParametro Is Nothing Then


            Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
            Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = documentiGenerici.GetQuery.Where(Function(c) c.IdIstanza = TaskAttivo.IdIstanza AndAlso c.Stato Is Nothing And (c.Inviato = False Or c.Inviato Is Nothing)).FirstOrDefault

            'Se il documento generico non è stato ancora generato
            If documentoGenerico Is Nothing Then
                ParsecUtility.Utility.MessageBox("Per protocollare è necessario generare il documento!", False)
                Me.EnableUiHidden.Value = "Abilita"
            Else


                Dim registrazione As New ParsecPro.Registrazione

                Dim dataOdierna = Now
                registrazione.DataOraRegistrazione = dataOdierna
                registrazione.IdUtente = utenteCollegato.Id
                registrazione.UtenteUsername = utenteCollegato.Username
                registrazione.DataImmissione = dataOdierna

                registrazione.Riservato = False
                registrazione.Modificato = False
                registrazione.Annullato = False
                registrazione.PresentiModifiche = False
                registrazione.DataOraAnnullamento = Nothing
                registrazione.IdUtenteAnnullamento = Nothing
                registrazione.UtenteUsernameAnnullamento = Nothing

                registrazione.TipoRegistrazione = 1  'PARTENZA
                registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza
                registrazione.TipoRegistro = ParsecPro.TipoRegistro.Generale

                registrazione.DataOraRicezioneInvio = Nothing
                registrazione.IdTipoDocumento = Nothing
                registrazione.IdTipoRicezione = Nothing
                registrazione.ProtocolloMittente = String.Empty


                registrazione.IdClassificazione = Nothing
                registrazione.NumeroProtocolloRiscontro = Nothing
                registrazione.DataImmissioneRiscontro = Nothing
                registrazione.IdSessioneEmergenza = Nothing
                registrazione.NumeroEmergenza = Nothing
                registrazione.IdUtenteEmergenza = Nothing
                registrazione.UtenteEmergenzaUsername = Nothing
                registrazione.Note = String.Empty
                registrazione.NoteInterne = String.Empty
                registrazione.Spid = Nothing
                registrazione.AnticipatoViaFax = False
                registrazione.TipologiaAllegatoPrimario = ParsecPro.TipologiaAllegatoPrimario.Generico

                registrazione.DataDocumento = dataOdierna
                registrazione.DataProtocollo = dataOdierna
                registrazione.Riservato = True


                registrazione.Oggetto = documentoGenerico.Oggetto

                Dim tipiRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository
                Dim tipoInvio = tipiRicezioneInvio.Where(Function(c) c.Descrizione.ToLower = "pec" And c.LogStato = Nothing).FirstOrDefault
                If Not tipoInvio Is Nothing Then
                    registrazione.IdTipoRicezione = tipoInvio.Id
                End If

                Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
                Dim tipoDocumento = tipiDocumento.Where(Function(c) c.Descrizione.ToLower = "elettronico").FirstOrDefault
                If Not tipoDocumento Is Nothing Then
                    registrazione.IdTipoDocumento = tipoDocumento.Id
                End If
                tipiDocumento.Dispose()

                Dim idUtente As Integer = Me.TaskAttivo.IdMittente

                Dim credenziali As New ParsecAdmin.CredenzialiRiceventeRepository
                Dim credenziale = credenziali.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault
                credenziali.Dispose()

                If Not credenziale Is Nothing Then

                    Dim strutture As New ParsecAdmin.StructureRepository
                    Dim struttura = strutture.GetQuery.Where(Function(c) c.Codice = credenziale.CodiceStruttura And c.LogStato Is Nothing).FirstOrDefault
                    strutture.Dispose()

                    If Not struttura Is Nothing Then
                        '***********************************************************************************************************************************
                        'AGGIUNGO IL MITTENTE INTERNO
                        '***********************************************************************************************************************************
                        Dim mittenteInterno As New ParsecPro.Mittente(struttura.Id, True)
                        registrazione.Mittenti.Add(mittenteInterno)
                        '***********************************************************************************************************************************
                    End If
                End If


                '***********************************************************************************************************************************
                'AGGIUNGO IL DESTINATARIO ESTERNO
                '***********************************************************************************************************************************
                Dim destinatari = documentiGenerici.GetDestinatari(documentoGenerico.Id, 10)
                For Each dest In destinatari
                    Dim destinatario As New ParsecPro.Destinatario(dest.IdReferenteEsterno, False)
                    registrazione.Destinatari.Add(destinatario)
                Next
                '***********************************************************************************************************************************


                '***********************************************************************************************************************************
                'CARICO LA VISIBILITA
                '***********************************************************************************************************************************
                Dim visibilita As New ParsecAdmin.VisibilitaDocumentoRepository
                Dim idUtentiVisibilitaSegnalazione = visibilita.Where(Function(c) c.IdModulo = Me.TaskAttivo.IdModulo And c.IdDocumento = Me.TaskAttivo.IdDocumento).Select(Function(c) c.IdEntita).ToList
                Dim utenti As New ParsecAdmin.UserRepository
                For Each idUtenteVisibilitaSegnalazione In idUtentiVisibilitaSegnalazione
                    Dim idUte = idUtenteVisibilitaSegnalazione
                    Dim utente = utenti.Where(Function(c) c.Id = idUte).FirstOrDefault
                    Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
                    utenteVisibilita.IdEntita = idUtenteVisibilitaSegnalazione
                    utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
                    utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.PRO
                    utenteVisibilita.Descrizione = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
                    utenteVisibilita.LogIdUtente = utenteCollegato.Id
                    utenteVisibilita.LogDataOperazione = Now
                    utenteVisibilita.AbilitaCancellaEntita = False
                    Dim esiste As Boolean = Not registrazione.Visibilita.Where(Function(c) c.IdEntita = utenteVisibilita.IdEntita And c.TipoEntita = utenteVisibilita.TipoEntita).FirstOrDefault Is Nothing
                    If Not esiste Then
                        registrazione.Visibilita.Add(utenteVisibilita)
                    End If
                Next
                visibilita.Dispose()
                utenti.Dispose()
                '***********************************************************************************************************************************

                '***********************************************************************************************************************************
                'CARICO GLI ALLEGATI
                '***********************************************************************************************************************************
                Dim anno As String = documentoGenerico.DataCreazione.Value.Year.ToString
                Dim sorgenti As New ParsecAdmin.SorgentiRepository
                Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documentoGenerico.IdSorgente)
                sorgenti.Dispose()
                Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)
                Dim fileExists As Boolean = False

                Dim localPath As String = String.Empty

                Select Case sorgenteCorrente.IdTipologia
                    Case 1 'Locale
                        localPath = source.Path & anno & "\" & documentoGenerico.NomeFile
                        fileExists = IO.File.Exists(localPath)
                    Case 2  'Ftp
                        Dim relativePath As String = source.RelativePath & anno & "/" & documentoGenerico.NomeFile
                        fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
                End Select

                If Not fileExists Then
                    ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
                    Exit Sub
                Else
                    '*****************************************************************************
                    'Copio l'allegato nella cartella temporanea.
                    '*****************************************************************************
                    Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & documentoGenerico.NomeFile
                    IO.File.Copy(localPath, pathDownload, True)
                    '*****************************************************************************

                    '*****************************************************************************
                    'Aggiungo l'allegato (documento generico) alla registrazione che sta per essere inserita
                    '*****************************************************************************
                    Dim allegato As New ParsecPro.Allegato
                    allegato.NomeFile = documentoGenerico.NomeFile
                    allegato.NomeFileTemp = documentoGenerico.NomeFile
                    allegato.IdTipologiaDocumento = 1 'Primario
                    allegato.DescrizioneTipologiaDocumento = "Primario"
                    allegato.Oggetto = "Documento di risposta"
                    allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                    allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
                    allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                    allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
                    registrazione.Allegati.Add(allegato)
                    '*****************************************************************************
                End If



                '***********************************************************************************************************************************


                Dim pageUrl As String = "~" & ParsecWKF.ModelloInfo.ReadResource(Me.TaskAttivo.TaskCorrente, e.CommandName, Me.TaskAttivo.NomeFileIter)
                Dim parametriPagina As New Hashtable
                parametriPagina.Add("RegistrazioneInIter", registrazione)
                Dim queryString As New Hashtable
                queryString.Add("Mode", "Insert")
                queryString.Add("Tipo", "1") 'Partenza
                queryString.Add("obj", Me.AggiornaIterButton.ClientID)
                'queryString.Add("AvviaIter", "0")
                queryString.Add("Segnalazione", Me.TaskAttivo.IdDocumento)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 600, queryString, False)


            End If

            Exit Sub
        End If

        '******************************************************************************************************************************************************************
        '3) ESEGUO IL TASK INVIA EMAIL
        '******************************************************************************************************************************************************************
        If Not inviaEmailParametro Is Nothing Then
            Me.VisualizzaEmailInModifica()
            Exit Sub
        End If


        '******************************************************************************************************************************************************************
        '3) ESEGUO IL TASK MODIFICA SENZA PARAMETRI
        '******************************************************************************************************************************************************************
        If Me.Action.Parameters.Count = 0 Then
            Me.VisualizzaSegnalazione()
            Exit Sub
        End If


    End Sub

    Private Sub AggiornaIterWBT()

        Dim preparaDocumentoParametro = Me.Action.GetParameterByName("PreparaDocumento")
        Dim protocollazioneParametro = Me.Action.GetParameterByName("Protocollazione")
        Dim inviaEmailParametro = Me.Action.GetParameterByName("InviaEmail")


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        '******************************************************************************************************************************************************************
        '1) AGGIORNO L'ITER DOPO AVER ESEGUITO IL TASK PREPARA DOCUMENTO
        '******************************************************************************************************************************************************************
        If Not preparaDocumentoParametro Is Nothing Then

            If Not ParsecUtility.SessionManager.DocumentoGenerico Is Nothing Then

                Try
                    Dim documentoGenerico As ParsecAdmin.DocumentoGenerico = CType(ParsecUtility.SessionManager.DocumentoGenerico, ParsecAdmin.DocumentoGenerico)
                    Dim documentiGenerici As New ParsecAdmin.DocumentiGenericiRepository
                    Dim doc As ParsecAdmin.DocumentoGenerico = documentiGenerici.CreateFromInstance(documentoGenerico)
                    doc.IdIstanza = Me.TaskAttivo.IdIstanza
                    documentiGenerici.SaveChanges()
                    documentiGenerici.Dispose()
                    ParsecUtility.SessionManager.DocumentoGenerico = Nothing
                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)
                    Me.AggiornaStatoSegnalazione(utenteCollegato.Id)

                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                    Me.EnableUiHidden.Value = "Abilita"
                    'Me.SbloccaTasks(utenteCollegato.Id)
                End Try

            End If

            Me.SbloccaTasks(utenteCollegato.Id)
            Exit Sub

        End If


        '******************************************************************************************************************************************************************
        '2) AGGIORNO L'ITER DOPO AVER ESEGUITO IL TASK PROTOCOLLA
        '******************************************************************************************************************************************************************
        If Not protocollazioneParametro Is Nothing Then

            Try
                If Not ParsecUtility.SessionManager.Registrazione Is Nothing Then
                    Dim protocollo As ParsecPro.Registrazione = ParsecUtility.SessionManager.Registrazione

                    Dim stato As String = "INOLTRATA"
                    'ASSOCIO LA REGISTRAZIONE ALLA SEGNALAZIONE E AGGIORNO LO STATO
                    Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
                    Dim segnalazione = segnalazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault

                    If Not segnalazione Is Nothing Then
                        segnalazione.IdRegistrazione = protocollo.Id
                        segnalazione.Stato = stato
                        segnalazioni.SaveChanges()

                    End If
                    segnalazioni.Dispose()

                    Me.AggiornaDocumentoGenerico(protocollo)

                    ParsecUtility.SessionManager.Registrazione = Nothing

                    If Not segnalazione Is Nothing Then
                        Me.AggiornaStatoSegnalazioneOnline(utenteCollegato.Id, segnalazione.GuidSegnalazione, stato)
                    End If


                   

                End If

            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
                Me.EnableUiHidden.Value = "Abilita"
                'Me.SbloccaTasks(utenteCollegato.Id)
            End Try

            Me.SbloccaTasks(utenteCollegato.Id)
            Exit Sub
        End If

        '******************************************************************************************************************************************************************
        '3) AGGIORNO L'ITER DOPO AVER ESEGUITO IL TASK INVIA EMAIL
        '******************************************************************************************************************************************************************
        If Not inviaEmailParametro Is Nothing Then
            Try
                If Not ParsecUtility.SessionManager.EmailInviata Is Nothing Then
                    ParsecUtility.SessionManager.EmailInviata = Nothing

                    Dim stato As String = "CONCLUSA"
                    Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
                    Dim segnalazione = segnalazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
                    If Not segnalazione Is Nothing Then
                        segnalazione.Stato = stato
                        segnalazioni.SaveChanges()
                    End If
                    segnalazioni.Dispose()

                    Me.WriteTaskAndUpdateParent(Me.TaskAttivo.IdMittente)

                    If Not segnalazione Is Nothing Then
                        Me.AggiornaStatoSegnalazioneOnline(utenteCollegato.Id, segnalazione.GuidSegnalazione, stato)
                    End If

                   
                End If
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
                Me.EnableUiHidden.Value = "Abilita"
                'Me.SbloccaTasks(utenteCollegato.Id)
            End Try

            Me.SbloccaTasks(utenteCollegato.Id)
            Exit Sub

        End If


        '******************************************************************************************************************************************************************
        '3) AGGIORNO L'ITER DOPO AVER ESEGUITO IL TASK MODIFICA SENZA PARAMETRI
        '******************************************************************************************************************************************************************
        If Me.Action.Parameters.Count = 0 Then
            Select Case Me.Action.Type

                Case "MODIFICA"
                    If Not ParsecUtility.SessionManager.SegnalazioneIllecito Is Nothing Then
                        Try
                            ParsecUtility.SessionManager.SegnalazioneIllecito = Nothing
                            Dim idDestinatario As Integer = Me.GetIdDestinatario()
                            Me.WriteTaskAndUpdateParent(idDestinatario)

                            Me.AggiornaStatoSegnalazione(utenteCollegato.Id)

                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)
                            Me.EnableUiHidden.Value = "Abilita"
                            'Me.SbloccaTasks(utenteCollegato.Id)
                        End Try
                    Else
                        'CHIUDO LA FINESTRA PER AGGIORNARE IL RIFERIMENTO AL DOCUMENTO
                        'Me.RegistraButtonClick()
                    End If

                    Me.SbloccaTasks(utenteCollegato.Id)
                    Exit Sub

            End Select

        End If

        Me.SbloccaTasks(utenteCollegato.Id)
    End Sub


    Private Sub AggiornaStatoSegnalazione(ByVal idUtente As Integer)
        Try
            Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
            Dim segnalazione = segnalazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento).FirstOrDefault
            If Not segnalazione Is Nothing Then

                Dim stato As String = segnalazione.Stato.ToUpper

                If segnalazione.Stato.ToUpper = "NUOVA" Then
                    stato = "PRESA IN CARICO"
                ElseIf segnalazione.Stato.ToUpper = "PRESA IN CARICO" Then
                    stato = "IN LAVORAZIONE"
                End If

                If stato <> segnalazione.Stato.ToUpper Then
                    segnalazione.Stato = stato
                    segnalazioni.SaveChanges()
                    Me.AggiornaStatoSegnalazioneOnline(idUtente, segnalazione.GuidSegnalazione, stato)
                End If
            End If
            segnalazioni.Dispose()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AggiornaStatoSegnalazioneOnline(ByVal idUtente As Integer, ByVal guidSegnalazione As String, ByVal stato As String)
        Try
            Dim parametri As New ParsecAdmin.ParametriSegnalazioneRepository
            Dim parametro = parametri.GetQuery.FirstOrDefault
            parametri.Dispose()

            Dim credenziali As New ParsecAdmin.CredenzialiRiceventeRepository
            Dim cred = credenziali.Where(Function(c) c.IdUtente = idUtente).FirstOrDefault
            credenziali.Dispose()


            Dim userName As String = ParsecCommon.CryptoUtil.Decrypt(cred.NomeUtenteRicevente)
            Dim password As String = ParsecCommon.CryptoUtil.Decrypt(cred.PasswordRicevente)
            Dim baseUrl As String = parametro.EndPointServizio

            Dim service As New ParsecWebServices.WhistleBlowingService(baseUrl)

            Dim ricevente = service.GetUser(userName, password)
            service.SetLabel(ricevente.session_id, guidSegnalazione, stato)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub



    Private Enum TipoFirma
        Nessuna = -1
        FirmaQualificata = 0
        CofirmaQualificata = 1
        FirmaElettronica = 2
    End Enum

    Private Sub FirmaMassivaSchedaControllo(ByVal fileToSignDictionary As Dictionary(Of Integer, InfoFirma), ByVal tipoFirma As TipoFirma)

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato

        Dim signParameters As New ParsecAdmin.SignParameters
        Dim datiSrc As New ParsecAdmin.DatiFirmaSrc

        Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
        Dim infoFirma As InfoFirma = Nothing
        For Each item As KeyValuePair(Of Integer, InfoFirma) In fileToSignDictionary
            infoFirma = item.Value
            'Dim annoEsercizio As String = infoFirma.Anno
            Dim fileToSign As String = infoFirma.NomeFile
            Dim annoEsercizio As String = rgx.Match(fileToSign).Value

            If tipoFirma = OperazioneUserControl.TipoFirma.CofirmaQualificata Then
                fileToSign = IO.Path.GetFileNameWithoutExtension(fileToSign) & ".pdf.p7m"
            End If

            Dim pathFileToSign As String = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeSchedeControllo"), annoEsercizio, fileToSign)

            datiSrc.RowInfos.Add(item.Key, pathFileToSign)
        Next

        Dim remotePathCertificate As String = String.Empty
        If tipoFirma = OperazioneUserControl.TipoFirma.FirmaElettronica Then
            remotePathCertificate = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeCertificati"), utenteCorrente.NomefileCertificato)
        End If

        Dim funzione As FunctionType = FunctionType.MultiSignDocument
        Dim provider As ParsecAdmin.ProviderType = CType(utenteCorrente.IdProviderFirma, ParsecAdmin.ProviderType)

        If tipoFirma = OperazioneUserControl.TipoFirma.FirmaElettronica Then
            funzione = FunctionType.MultiWeakSignDocument
            provider = ProviderType.CryptoApi
        Else
            If tipoFirma = OperazioneUserControl.TipoFirma.CofirmaQualificata Then
                funzione = FunctionType.MultiCosignDocument
            End If
        End If

        If provider = ProviderType.Nessuno Then
            provider = ProviderType.PkNet 'Valore predefinito
        End If

        Dim datiInput As New DatiFirma
        datiInput.RemotePathToSign = String.Empty
        datiInput.RemotePathCertificate = remotePathCertificate
        datiInput.FunctionType = funzione
        datiInput.CertificateId = certificateId
        datiInput.Provider = provider

        Dim data As String = signParameters.CreaDataSource(datiInput, datiSrc)

        'Me.RegistraScriptPersistenzaVisibilitaPannello()

        'UTILIZZO L'APPLET
        If Not utenteCorrente.TecnologiaClientSide.HasValue OrElse utenteCorrente.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaIterButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.AggiornaIter)
        End If



    End Sub


    Private Sub FirmaMassivaCSRA(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        '***********************************************************************************************************************************************
        'GESTIONE DELEGA
        '***********************************************************************************************************************************************

        If Me.TaskAttivo.IdAttoreCorrente <> utenteCollegato.Id Then
            Me.RegistraScriptPersistenzaVisibilitaPannello()
            ParsecUtility.Utility.MessageBox("Attenzione stai firmando al posto di un'altro utente!", False)
        End If


        '***********************************************************************************************************************************************
        'GESTIONE PERMESSI
        '***********************************************************************************************************************************************
        Dim firmaDigitalmente As Boolean = Me.CheckAbilitazioneFirmaDigitale(utenteCollegato)
        '***********************************************************************************************************************************************

        Dim abilitaFirmaElettronica As Boolean = Not Me.Action.GetParameterByName("AbilitaFirmaElettronica") Is Nothing

        'SE HO I PERMESSI PER LA FIRMA DIGITALE O E' UNA FIRMA ELETTRONICA
        If firmaDigitalmente Or abilitaFirmaElettronica Then

            Dim fileToSignDictionary As Dictionary(Of Integer, InfoFirma) = New Dictionary(Of Integer, InfoFirma)


            For Each taskCorrente In Me.TaskAttivi
                Me.TaskAttivo = taskCorrente


                Dim scheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa = Me.GetScheda
                'MEMORIZZO IL NOME DEL FILE DEL DOCUMENTO DA FIRMARE
                If Not scheda Is Nothing Then
                    fileToSignDictionary.Add(Me.TaskAttivo.Id, New InfoFirma With {.NomeFile = scheda.Nomefile})
                End If

                'MEMORIZZO I DESTINATARI
                'Se questa funzione viene chiamata dal datalist
                If Not e Is Nothing Then
                    Dim idDestinatario As Integer = Me.GetIdDestinatario()
                    Me.ElencoDestinatariDictionary.Add(taskCorrente.Id, idDestinatario)
                Else
                    'Se questa funzione viene chiamata dal item del menu di contesto

                    If Me.RuoloId = 0 Then
                        Dim idDestinatario As Integer = Me.GetIdDestinatario()
                        Me.ElencoDestinatariDictionary.Add(taskCorrente.Id, idDestinatario)
                    Else
                        Me.ElencoDestinatariDictionary.Add(taskCorrente.Id, Me.RuoloId)
                    End If
                End If
            Next


            Dim cofirma As Boolean = Not Me.Action.GetParameterByName("Cofirma") Is Nothing
            Dim tipoFirma As TipoFirma = tipoFirma.FirmaQualificata

            If abilitaFirmaElettronica Then
                tipoFirma = OperazioneUserControl.TipoFirma.FirmaElettronica
            Else
                If cofirma Then
                    tipoFirma = OperazioneUserControl.TipoFirma.CofirmaQualificata
                End If
            End If

            Me.FirmaMassivaSchedaControllo(fileToSignDictionary, tipoFirma)

        Else
            'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
            Me.WriteAllTasksAndUpdateParent(Me.ElencoDestinatariDictionary)
            Me.SbloccaTasks(utenteCollegato.Id)
        End If

        If Not e Is Nothing Then
            Me.RuoloId = Nothing
        End If

    End Sub


    Private Sub FirmaCSRA(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        'FIRMO LA SCHEDA CONTROLLO

        Dim roleFromName = ParsecWKF.ModelloInfo.ReadActorInfo(Me.TaskAttivo.NomeFileIter, Me.TaskAttivo.TaskCorrente, Me.Action.Name).Where(Function(c) c.Type = ParsecWKF.Actor.ActorType.Sender).FirstOrDefault.Name
        Dim ruoli As New ParsecWKF.RuoloRepository
        Dim role As ParsecWKF.Ruolo = ruoli.Where(Function(c) c.Descrizione = roleFromName).FirstOrDefault
        ruoli.Dispose()

        If role Is Nothing Then
            'TODO
            Exit Sub
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        '***********************************************************************************************************************************************
        'GESTIONE DELEGA
        '***********************************************************************************************************************************************
       
        If Me.TaskAttivo.IdAttoreCorrente <> utenteCollegato.Id Then
            Me.RegistraScriptPersistenzaVisibilitaPannello()
            ParsecUtility.Utility.MessageBox("Attenzione stai firmando al posto di un'altro utente!", False)
        End If


        '***********************************************************************************************************************************************

        '***********************************************************************************************************************************************
        'GESTIONE PERMESSI
        '***********************************************************************************************************************************************
        Dim firmaDigitalmente As Boolean = Me.CheckAbilitazioneFirmaDigitale(utenteCollegato)
        '***********************************************************************************************************************************************

        Dim abilitaFirmaElettronica As Boolean = Not Me.Action.GetParameterByName("AbilitaFirmaElettronica") Is Nothing

        'SE HO I PERMESSI PER LA FIRMA DIGITALE O E' UNA FIRMA ELETTRONICA
        If firmaDigitalmente Or abilitaFirmaElettronica Then
            Dim scheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa = Me.GetScheda
            If Not scheda Is Nothing Then

                Dim cofirma As Boolean = Not Me.Action.GetParameterByName("Cofirma") Is Nothing
                Dim tipoFirma As TipoFirma = tipoFirma.FirmaQualificata

                If abilitaFirmaElettronica Then
                    tipoFirma = OperazioneUserControl.TipoFirma.FirmaElettronica
                Else
                    If cofirma Then
                        tipoFirma = OperazioneUserControl.TipoFirma.CofirmaQualificata
                    End If
                End If

                Me.FirmaSchedaControllo(scheda.Nomefile, tipoFirma)
            End If

        Else
            Dim idDestinatario As Integer = Me.GetIdDestinatario()
            'Chiudo la pagina  e aggiorno la griglia della pagina chiamante.
            Me.WriteTaskAndUpdateParent(idDestinatario)
            Me.SbloccaTasks(utenteCollegato.Id)
        End If

    End Sub

    Private Function GetScheda() As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa
        Dim scheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa = Nothing
        Dim idAtto As Integer = Me.TaskAttivo.IdDocumento
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento = documenti.Where(Function(c) c.Id = idAtto).FirstOrDefault
        documenti.Dispose()

        If Not documento Is Nothing Then
            Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
            scheda = schede.Where(Function(c) c.NumeroAtto = documento.ContatoreGenerale And c.DataAtto = documento.Data).FirstOrDefault
            schede.Dispose()
        End If
        Return scheda
    End Function

    Private Sub FirmaSchedaControllo(ByVal nomeFile As String, ByVal tipoFirma As TipoFirma)


        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato

        Dim remotePathToSign As String = String.Empty
        Dim provider As ParsecAdmin.ProviderType = CType(utenteCorrente.IdProviderFirma, ParsecAdmin.ProviderType)
        Dim remotePathCertificate As String = String.Empty

       
        Dim funzione As FunctionType = FunctionType.SignDocument
        Dim functionName As String = "SignDocument"

        If tipoFirma = OperazioneUserControl.TipoFirma.FirmaElettronica Then
            funzione = FunctionType.WeakSignDocument
            provider = ProviderType.CryptoApi
            remotePathCertificate = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeCertificati"), utenteCorrente.NomefileCertificato)
        Else
            If tipoFirma = OperazioneUserControl.TipoFirma.CofirmaQualificata Then
                funzione = FunctionType.CosignDocument
                functionName = "CoSignDocument"
                nomeFile = IO.Path.GetFileNameWithoutExtension(nomeFile) & ".pdf.p7m"
            End If
        End If


        If provider = ProviderType.Nessuno Then
            provider = ProviderType.PkNet 'Valore predefinito
        End If

        Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
        Dim annoEsercizio As String = rgx.Match(nomeFile).Value



        Dim localPathToSign As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathSchedeControllo"), annoEsercizio, nomeFile)
        If Not IO.File.Exists(localPathToSign) Then
            Throw New ApplicationException(String.Format("Il file '{0}' non esiste!", localPathToSign.Replace("\", "/")))
        End If

        remotePathToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeSchedeControllo"), annoEsercizio, nomeFile)



        Dim signParameters As New ParsecAdmin.SignParameters
        Dim datiInput As New ParsecAdmin.DatiFirma With {
            .RemotePathToSign = remotePathToSign,
            .Provider = provider,
            .CertificateId = certificateId,
            .RemotePathCertificate = remotePathCertificate,
            .FunctionType = funzione,
            .FunctionName = functionName
        }



        Dim data As String = signParameters.CreaDataSource(datiInput)
        Me.RegistraScriptPersistenzaVisibilitaPannello()


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaIterButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.AggiornaIter)
        End If

    End Sub

    Private Sub FirmaWBT(ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        'FIRMO IL DOCUMENTO GENERICO E LO ALLEGO AL PROTOCOLLO
        Dim segnalazioni As New ParsecAdmin.SegnalazioneRepository
        Dim segnalazione = segnalazioni.Where(Function(c) c.Id = Me.TaskAttivo.IdDocumento And Not c.IdRegistrazione Is Nothing).Select(Function(c) c.IdRegistrazione)
        If segnalazione.Any Then
            Me.FirmaDocumentoGenerico(segnalazione.FirstOrDefault.Value)
        End If
        segnalazioni.Dispose()
    End Sub


    Protected Sub ConfermaRecipientsButton_Click(sender As Object, e As System.EventArgs) Handles ConfermaRecipientsButton.Click

        'INVIO 

        If Me.OperazioneMassiva Then


        Else
            Me.RuoloId = Me.GetIdDestinatario()
            Me.Procedi(Nothing, Direction.Forward)
            If Me.UtentiListBox.CheckedItems.Count > 0 Then

                Dim istanze As New ParsecWKF.IstanzaRepository
                Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
                istanze.Dispose()

                Dim contatoreGenerale As Integer = istanza.ContatoreGenerale

                For Each checkedItem In Me.UtentiListBox.CheckedItems
                    Dim idDestinatario As Integer = checkedItem.Value

                    Me.CreaIstanzaNotificaAttoAmministrativo(Me.TaskAttivo, idDestinatario, contatoreGenerale)
                Next
            End If

        End If





    End Sub

    'Protected Sub RadGrid1_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs)
    '    If TypeOf e.Item Is GridDataItem Then
    '        Dim item As GridDataItem = CType(e.Item, GridDataItem)
    '        Dim list As CheckBox = CType(item.FindControl("CheckBox1"), CheckBox)
    '        list.Attributes.Add("onclick", "OnSelectedIndexChange('" & item.ItemIndex & "','" + list.ClientID & "');")
    '    End If
    'End Sub




    'Function OnSelectedIndexChange(index, sender)
    '    {
    '        var grid = $find("<%=RadGrid1.ClientID %>");
    '        var MasterTable = grid.get_masterTableView();
    '        var row = MasterTable.get_dataItems()[index];
    '        var chk = document.getElementById(sender);
    '        var textBox;
    '    If (chk.checked) Then
    '        {
    '            row.findElement("TextBoxCantidadEquipo").disabled = false;
    '            return;
    '        }

    '      // Elemento DOM
    '        row.findElement("TextBoxCantidadEquipo").disabled = true; 
    '        //RadControl
    '        row.findControl("ddlCoverage").set_enabled(false);    
    '    }



    Public Sub CreaIstanzaNotificaAttoAmministrativo(ByVal taskAttivo As ParsecWKF.TaskAttivo, ByVal idDestinatario As Integer, ByVal contatoreGenerale As Integer)

        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanzaPrecedente As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.IdDocumento = taskAttivo.IdDocumento AndAlso c.IdModulo = ParsecAdmin.TipoModulo.NAA And c.Ufficio = idDestinatario).FirstOrDefault


        'Se l'istanza del documento non è in iter
        If istanzaPrecedente Is Nothing Then

            Dim descrizioneDocumento As String = String.Format("Notifica Atto - {0}", taskAttivo.DescrizioneDocumento)


            Dim modelli As New ParsecWKF.ModelliRepository
            Dim modello As ParsecWKF.Modello = modelli.Where(Function(c) c.RiferimentoModulo = ParsecAdmin.TipoModulo.NAA).FirstOrDefault
            modelli.Dispose()

            Dim adesso As DateTime = Now

            Dim statoIniziale As Integer = 1
            Dim statoDaEseguire As Integer = 5

            Dim idIstanza As Integer = 0

            'MITTENTE DELLA NOTIFICA 
            Dim idMittente As Integer = taskAttivo.IdAttoreCorrente


            Dim istanza As New ParsecWKF.Istanza
            istanza.Riferimento = descrizioneDocumento
            istanza.IdStato = statoIniziale
            istanza.DataInserimento = adesso
            istanza.DataScadenza = adesso '.AddDays(modelloIter.DurataIter)
            istanza.IdModulo = modello.RiferimentoModulo
            istanza.IdModello = modello.Id
            istanza.IdDocumento = taskAttivo.IdDocumento
            istanza.ContatoreGenerale = contatoreGenerale
            istanza.FileIter = modello.NomeFile

            istanza.Ufficio = idDestinatario
            istanza.IdUtente = idMittente


            Try

                istanze.Add(istanza)
                istanze.SaveChanges()
                istanze.Dispose()

                idIstanza = istanza.Id


                '*******************************************************************************************************************************
                'Inserisco nei parametri del processo l'attore DESTINATARIO corrente.
                Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
                Dim processo As New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "DESTINATARIO", .Valore = idDestinatario.ToString}
                parametriProcesso.Add(processo)
                parametriProcesso.SaveChanges()



                'Inserisco nei parametri del processo l'attore MITTENTE se non esiste.
                Dim parametroProcessoExist As Boolean = Not parametriProcesso.GetQuery.Where(Function(c) c.IdProcesso = idIstanza And c.Nome = "MITTENTE").FirstOrDefault Is Nothing
                If Not parametroProcessoExist Then
                    processo = New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = "MITTENTE", .Valore = idMittente.ToString}
                    parametriProcesso.Add(processo)
                    parametriProcesso.SaveChanges()
                End If

                parametriProcesso.Dispose()
                '*******************************************************************************************************************************

                ' inserisco il task dell'istanza appena inserita
                Dim tasks As New ParsecWKF.TaskRepository

                Dim task As New ParsecWKF.Task
                task.IdIstanza = idIstanza
                task.Nome = ""


                Dim corrente As String = modello.StatoIniziale()
                task.Corrente = corrente
                task.Successivo = modello.StatoSuccessivo(corrente)

                task.Mittente = idMittente
                'task.Destinatario = IdDestinatario
                task.IdStato = statoDaEseguire
                task.DataInizio = adesso
                task.DataEsecuzione = adesso
                task.DataFine = adesso '.AddDays(modelloIter.DurataTask)
                task.Operazione = "NUOVO"
                task.Cancellato = False
                task.IdUtenteOperazione = idMittente

                Try

                    tasks.Add(task)
                    tasks.SaveChanges()

                    tasks.Dispose()

                    'Aggiungo il nuovo task
                    Me.ProcediNotifica(task, istanza, idDestinatario)


                Catch ex As Exception
                    Throw New ApplicationException(ex.Message)
                End Try

            Catch ex As Exception
                Throw New ApplicationException(ex.Message)
            End Try

        End If

    End Sub


    '*************************************************************************************************************
    'INSERISCO IL TASK AUTOMATICO
    '*************************************************************************************************************
    Private Sub ProcediNotifica(ByVal taskAttivo As ParsecWKF.Task, ByVal istanzaAttiva As ParsecWKF.Istanza, ByVal idDestinatario As Integer)

        Dim statoEseguito As Integer = 6
        Dim statoDaEseguire As Integer = 5
        Dim statoIstanzaCompletato As Integer = 3

        Dim nomeFileIter As String = istanzaAttiva.FileIter
        Dim idUtente As Integer = taskAttivo.Mittente

        Dim tasks As New ParsecWKF.TaskRepository


        tasks.Attach(taskAttivo)
        'Dim taskAttivo As ParsecWKF.Task = tasks.Where(Function(c) c.Id = taskAttivo.Id).FirstOrDefault


        Dim actions = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Corrente, nomeFileIter)


        Dim taskSuccessivoAutomatico As ParsecWKF.ParametroProcesso = actions(0).Parameters.Where(Function(c) c.Nome = "TaskSuccessivoAutomatico").FirstOrDefault

        Dim azione As ParsecWKF.Action = Nothing


        If Not taskSuccessivoAutomatico Is Nothing Then
            azione = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Successivo, nomeFileIter).Where(Function(c) c.Name = taskSuccessivoAutomatico.Valore).FirstOrDefault
        Else
            azione = ParsecWKF.ModelloInfo.ReadActionInfo(taskAttivo.Successivo, nomeFileIter)(0)
        End If

        Dim operazione As String = "INIZIO"
        Dim procediAction = actions.Where(Function(c) c.Type = "INIZIO").FirstOrDefault
        If Not procediAction Is Nothing Then
            If Not String.IsNullOrEmpty(procediAction.Description) Then
                operazione = procediAction.Description.ToUpper
            End If
        End If



        '*************************************************************************************************************
        'AGGIORNO IL TASK PRECEDENTE
        '*************************************************************************************************************

        'If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
        '    task.Note = Me.NoteInterneTextBox.Text
        'End If


        taskAttivo.IdStato = statoEseguito
        taskAttivo.DataEsecuzione = Now
        taskAttivo.Operazione = operazione
        taskAttivo.Destinatario = idUtente
        taskAttivo.Notificato = True
        tasks.SaveChanges()
        '*************************************************************************************************************

        '*************************************************************************************************************
        'INSERISCO IL NUOVO TASK
        '*************************************************************************************************************

        Dim statoSuccessivo As String = ParsecWKF.ModelloInfo.StatoSuccessivoAction(taskAttivo.Successivo, azione.Name, nomeFileIter)

        Dim durata As Integer = 1

        If Not String.IsNullOrEmpty(statoSuccessivo) Then
            'durata = ModelloInfo.DurataTaskIter(statoSuccessivo, taskAttivo.NomeFileIter)
        End If


        'ANNULLO L'ISTANZA
        If azione.Type = "FINE" Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = istanzaAttiva.Id).FirstOrDefault
            istanza.IdStato = statoIstanzaCompletato
            istanza.DataEsecuzione = Now
            istanze.SaveChanges()
            istanze.Dispose()
        End If

        Dim nuovotask As New ParsecWKF.Task
        nuovotask.IdIstanza = taskAttivo.IdIstanza
        nuovotask.TaskPadre = taskAttivo.Id

        nuovotask.Nome = taskAttivo.Corrente


        nuovotask.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo, nomeFileIter)

        nuovotask.Mittente = idDestinatario 'idUtente

        If azione.Type = "FINE" Then
            nuovotask.IdStato = statoEseguito
            nuovotask.Corrente = "FINE"
            nuovotask.Destinatario = idUtente
            nuovotask.Operazione = "FINE"
            nuovotask.DataEsecuzione = Now
        Else
            nuovotask.IdStato = statoDaEseguire
            nuovotask.Corrente = statoSuccessivo
        End If

        nuovotask.DataInizio = Now

        'If Not String.IsNullOrEmpty(Me.NoteInterneTextBox.Text) Then
        '    nuovotask.Note = Me.NoteInterneTextBox.Text
        'End If


        nuovotask.DataFine = Now.AddDays(durata)
        nuovotask.Cancellato = False
        nuovotask.Notificato = False
        nuovotask.IdUtenteOperazione = idUtente

        tasks.Add(nuovotask)
        tasks.SaveChanges()
        '*************************************************************************************************************

        tasks.Dispose()

    End Sub


End Class