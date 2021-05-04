Imports System.Data

Partial Class SchedaControlloSuccessivoRegolaritaAmministrativaPage
    Inherits System.Web.UI.Page


#Region "CLASSI PRIVATE"
    Private Class IndicatoreConformita
        Public Property Id As Integer
        Public Property Descrizione As String
        Public Property IdIndicatore As Integer
        Public Property EsitoConformita As Integer
        Public Property Annotazioni As String

    End Class

#End Region

#Region "PROPRIETA' PAGINA"

    Private Property SchedaControlloSuccessivoRegolaritaAmministrativa As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa
        Get
            Return CType(Session("SchedaControlloSuccessivoRegolaritaAmministrativaPage_SchedaControlloSuccessivoRegolaritaAmministrativa"), ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa)
        End Get
        Set(ByVal value As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa)
            Session("SchedaControlloSuccessivoRegolaritaAmministrativaPage_SchedaControlloSuccessivoRegolaritaAmministrativa") = value
        End Set
    End Property


    Private Property IndicatoriConformita As List(Of IndicatoreConformita)
        Get
            Return CType(Session("IndicatoriConformita_SchedaControlloSuccessivoRegolaritaAmministrativa"), List(Of IndicatoreConformita))
        End Get
        Set(ByVal value As List(Of IndicatoreConformita))
            Session("IndicatoriConformita_SchedaControlloSuccessivoRegolaritaAmministrativa") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then

            Me.IndicatoriConformita = Nothing


            Dim idScheda As Nullable(Of Integer) = Nothing
            If Not Me.Page.Request.QueryString("IdSchedaControlloSuccessivoRegolaritaAmministrativa") Is Nothing Then
                idScheda = CInt(Me.Page.Request.QueryString("IdSchedaControlloSuccessivoRegolaritaAmministrativa"))
            End If

            If Not Me.Page.Request.QueryString("Mode") Is Nothing Then
                If Me.Page.Request.QueryString("Mode") = "V" Then
                    Me.SalvaButton.Visible = False
                    Me.AggiornaNoteImageButton.Visible = False
                    Me.AnnotazioniTextBox.Visible = False
                    Me.AnnotazioniLabel.Visible = False
                    Me.IndicatoriGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
                End If

            End If

            If idScheda.HasValue Then
                Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
                Me.SchedaControlloSuccessivoRegolaritaAmministrativa = schede.Where(Function(c) c.Id = idScheda.Value).FirstOrDefault
                schede.Dispose()
                Me.AggiornaVista(Me.SchedaControlloSuccessivoRegolaritaAmministrativa)

                '*******************************************************************************************************
                'ABILITO I PULSANTI PER LA VISUALIZZAZIONE DELLA SCHEDA 
                '*******************************************************************************************************
                Dim nomefile As String = Me.SchedaControlloSuccessivoRegolaritaAmministrativa.Nomefile
                If Not String.IsNullOrEmpty(nomefile) Then
                    Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
                    Dim annoEsercizio As String = rgx.Match(nomefile).Value

                    Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathSchedeControllo"), annoEsercizio, nomefile)
                    If IO.File.Exists(localPath) Then
                        Me.VisualizzaDocumentoImageButton.Visible = True
                    End If
                End If

                Dim nomefileFirmato As String = Me.SchedaControlloSuccessivoRegolaritaAmministrativa.NomefileFirmato
                If Not String.IsNullOrEmpty(nomefileFirmato) Then
                    Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
                    Dim annoEsercizio As String = rgx.Match(nomefile).Value

                    Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathSchedeControllo"), annoEsercizio, nomefileFirmato)
                    If IO.File.Exists(localPath) Then
                        Me.VisualizzaDocumentoFirmatoImageButton.Visible = True
                    End If
                End If
                '*******************************************************************************************************


            End If


        End If



        '*********************************************************************************
        'Registro gli script usati per instanziare il componente ParsecOpenOffice.
        '*********************************************************************************

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If
        '***************************************************************************


        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        ParsecUtility.Utility.CloseWindow(False)
        '***************************************************************************

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.GrigliaIndicatoriPanel.Style.Add("width", widthStyle)
        Me.IndicatoriGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        Me.IndicatoriLabel.Text = "Indicatori Conformità&nbsp;&nbsp;" & If(Me.IndicatoriGridView.MasterTableView.Items.Count > 0, "( " & Me.IndicatoriGridView.MasterTableView.Items.Count.ToString & " )", "")

        If Not Me.Page.IsPostBack Then
            If String.IsNullOrEmpty(Me.SchedaControlloSuccessivoRegolaritaAmministrativa.Nomefile) Then
                Dim siRadioButton As RadioButton = Nothing
                Dim noRadioButton As RadioButton = Nothing
                Dim noApplicabileRadioButton As RadioButton = Nothing
                For Each item As Telerik.Web.UI.GridDataItem In Me.IndicatoriGridView.Items
                    siRadioButton = CType(item.FindControl("SiRadioButton"), RadioButton)
                    noRadioButton = CType(item.FindControl("NoRadioButton"), RadioButton)
                    noApplicabileRadioButton = CType(item.FindControl("NoApplicabileRadioButton"), RadioButton)
                    siRadioButton.Checked = False
                    noRadioButton.Checked = False
                    noApplicabileRadioButton.Checked = False
                Next
            End If
        End If

    End Sub


#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub ChiudiFinestraButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiFinestraButton.Click
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub

    Protected Sub notificaOperazioneButton_Click(sender As Object, e As System.EventArgs) Handles notificaOperazioneButton.Click
        Me.NotificaOperazione()
    End Sub

    Protected Sub SalvaButton_Click(sender As Object, e As System.EventArgs) Handles SalvaButton.Click

        Dim valutazioneCompleta As Boolean = True
        Dim siRadioButton As RadioButton = Nothing
        Dim noRadioButton As RadioButton = Nothing
        Dim noApplicabileRadioButton As RadioButton = Nothing

        Dim noteLabel As Label = Nothing

        For Each item As Telerik.Web.UI.GridDataItem In Me.IndicatoriGridView.Items

            siRadioButton = CType(item.FindControl("SiRadioButton"), RadioButton)
            noRadioButton = CType(item.FindControl("NoRadioButton"), RadioButton)
            noApplicabileRadioButton = CType(item.FindControl("NoApplicabileRadioButton"), RadioButton)

            If Not siRadioButton.Checked And Not noRadioButton.Checked And Not noApplicabileRadioButton.Checked Then
                valutazioneCompleta = False
                Exit For
            End If
        Next

        If valutazioneCompleta Then

            Try

                '*******************************************************************************************************************************
                'AGGIORNO GLI INDICATORI ASSOCIATI ALLA SCHEDA
                '*******************************************************************************************************************************
                Dim dettagliScheda As New ParsecAtt.DettaglioSchedaControlloSuccessivoRegolaritaAmministrativaRepository
                Dim dettagli = dettagliScheda.Where(Function(c) c.IdScheda = Me.SchedaControlloSuccessivoRegolaritaAmministrativa.Id).ToList
                Dim idIndicatore As Integer = 0

                For Each item As Telerik.Web.UI.GridDataItem In Me.IndicatoriGridView.Items
                    siRadioButton = CType(item.FindControl("SiRadioButton"), RadioButton)
                    noRadioButton = CType(item.FindControl("NoRadioButton"), RadioButton)
                    noApplicabileRadioButton = CType(item.FindControl("NoApplicabileRadioButton"), RadioButton)

                    noteLabel = CType(item.FindControl("NoteTextBox"), Label)

                    idIndicatore = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdIndicatore")

                    If siRadioButton.Checked Then
                        dettagli.Where(Function(c) c.IdIndicatore = idIndicatore).FirstOrDefault.EsitoConformita = ParsecAtt.ConformitaControlloSuccessivoRegolaritaAmministrativa.Positivo
                    End If

                    If noRadioButton.Checked Then
                        dettagli.Where(Function(c) c.IdIndicatore = idIndicatore).FirstOrDefault.EsitoConformita = ParsecAtt.ConformitaControlloSuccessivoRegolaritaAmministrativa.Negativo
                    End If

                    If noApplicabileRadioButton.Checked Then
                        dettagli.Where(Function(c) c.IdIndicatore = idIndicatore).FirstOrDefault.EsitoConformita = ParsecAtt.ConformitaControlloSuccessivoRegolaritaAmministrativa.NonApplicabile
                    End If

                    If Not String.IsNullOrEmpty(noteLabel.Text) Then
                        dettagli.Where(Function(c) c.IdIndicatore = idIndicatore).FirstOrDefault.Annotazioni = noteLabel.Text
                    End If


                Next


                dettagliScheda.SaveChanges()
                dettagliScheda.Dispose()
                '*******************************************************************************************************************************


                '*******************************************************************************************************************************
                'AGGIORNO LA SCHEDA
                '*******************************************************************************************************************************
                Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
                Dim scheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa = schede.Where(Function(c) c.Id = Me.SchedaControlloSuccessivoRegolaritaAmministrativa.Id).FirstOrDefault

                Dim anno As String = Year(scheda.DataEstrazione).ToString
                scheda.Nomefile = String.Format("{0}_{1}_{2}.odt", "SchedaControllo", anno, scheda.Id.ToString.PadLeft(7, "0"))

                If Not String.IsNullOrEmpty(Me.OsservazioniTextBox.Text) Then
                    scheda.Osservazioni = Me.OsservazioniTextBox.Text
                End If



                If Me.EsitoPositivoRadioButton.Checked Then
                    scheda.Esito = ParsecAtt.EsitoControlloSuccessivoRegolaritaAmministrativa.Positivo
                End If

                If Me.EsitoNegativoRadioButton.Checked Then
                    scheda.Esito = ParsecAtt.EsitoControlloSuccessivoRegolaritaAmministrativa.Negativo
                End If

                scheda.DataControllo = Now

                schede.SaveChanges()
                schede.Dispose()
                '*******************************************************************************************************************************

                Me.GeneraDataSource(scheda)

                Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"

            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try



        Else
            ParsecUtility.Utility.MessageBox("E' necessario valutare tutti gli indicatori della scheda!", False)
        End If

    End Sub

    Protected Sub AggiornaNoteImageButton_Click(sender As Object, e As ImageClickEventArgs) Handles AggiornaNoteImageButton.Click
        If Not String.IsNullOrEmpty(Me.IdIndicatoreHidden.Value) Then

            Dim idIndicatore As Integer = CInt(Me.IdIndicatoreHidden.Value)

            Dim indicatore = Me.IndicatoriConformita.Where(Function(c) c.IdIndicatore = idIndicatore).FirstOrDefault

            indicatore.Annotazioni = Me.AnnotazioniTextBox.Text

            Me.AnnotazioniTextBox.Text = String.Empty
            Me.IdIndicatoreHidden.Value = String.Empty

            Me.AggiornaEsitoIndicatoriConformita()

            Me.IndicatoriGridView.Rebind()
        End If

    End Sub

    Protected Sub VisualizzaDocumentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoImageButton.Click
        Try
            Me.VisualizzaDocumento()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.ToString, False)
        End Try
    End Sub

    Protected Sub VisualizzaDocumentoFirmatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoFirmatoImageButton.Click
        Try
            Me.VisualizzaDocumentoFirmato()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.ToString, False)
        End Try
    End Sub

#End Region

#Region "PRIVATI"

    Private Sub AggiornaEsitoIndicatoriConformita()

        Dim siRadioButton As RadioButton = Nothing
        Dim noRadioButton As RadioButton = Nothing
        Dim noApplicabileRadioButton As RadioButton = Nothing
        Dim idIndicatore As Integer = -1
        Dim indicatore As IndicatoreConformita = Nothing

        For Each item As Telerik.Web.UI.GridDataItem In Me.IndicatoriGridView.Items

            idIndicatore = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdIndicatore"))
            indicatore = Me.IndicatoriConformita.Where(Function(c) c.IdIndicatore = idIndicatore).FirstOrDefault

            If Not indicatore Is Nothing Then

                siRadioButton = CType(item.FindControl("SiRadioButton"), RadioButton)
                If siRadioButton.Checked Then
                    indicatore.EsitoConformita = ParsecAtt.ConformitaControlloSuccessivoRegolaritaAmministrativa.Positivo
                    Continue For
                End If

                noRadioButton = CType(item.FindControl("NoRadioButton"), RadioButton)
                If noRadioButton.Checked Then
                    indicatore.EsitoConformita = ParsecAtt.ConformitaControlloSuccessivoRegolaritaAmministrativa.Negativo
                    Continue For
                End If

                noApplicabileRadioButton = CType(item.FindControl("NoApplicabileRadioButton"), RadioButton)
                If noApplicabileRadioButton.Checked Then
                    indicatore.EsitoConformita = ParsecAtt.ConformitaControlloSuccessivoRegolaritaAmministrativa.NonApplicabile
                    Continue For
                End If

            End If

        Next
    End Sub

    Private Sub RegisterComponent(ByVal script As String)
        Dim cell As New TableCell
        cell.Width = New Unit(30)
        cell.Controls.Add(New LiteralControl(script))
        Me.componentPlaceHolder.Rows(0).Cells.Add(cell)
    End Sub

    Private Function CreaTabellaMapping(ByVal scheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa) As DataTable
        Dim dtMapping As New DataTable("DatiCampiUtente")
        Dim dc1 As New DataColumn("NomeCampo", GetType(System.String))
        Dim dc2 As New DataColumn("ValoreCampo", GetType(System.String))
        dtMapping.Columns.Add(dc1)
        dtMapping.Columns.Add(dc2)


        Me.AddDataRow("Settore_Atto", scheda.DescrizioneSettore, dtMapping)
        Me.AddDataRow("Numero_Atto", scheda.NumeroAtto, dtMapping)
        Me.AddDataRow("Oggetto_Atto", scheda.OggettoAtto, dtMapping)
        Me.AddDataRow("Data_Atto", scheda.DataAtto.ToShortDateString, dtMapping)
        Me.AddDataRow("Responsabile_Atto", scheda.Responsabile, dtMapping)

        Me.AddDataRow("Annotazioni_Scheda", scheda.DataAtto.ToShortDateString, dtMapping)


        'ELIMINARE IL CAMPO CONTROLLORE DALLA SCHEDA E RECUPERARLO DALLA CONFIGURAZIONE DEL CONTROLLO?

        Me.AddDataRow("Controllore_Scheda", scheda.Controllore, dtMapping)

        Me.AddDataRow("DataEstrazione_Scheda", scheda.DataEstrazione.ToShortDateString, dtMapping)

        Me.AddDataRow("DataControllo_Scheda", scheda.DataControllo.Value.ToShortDateString, dtMapping)

        Me.AddDataRow("Osservazioni_Scheda", scheda.Osservazioni, dtMapping)

        Select Case scheda.Esito
            Case ParsecAtt.EsitoControlloSuccessivoRegolaritaAmministrativa.Positivo
                Me.AddDataRow("EsitoPositivo_Scheda", "X", dtMapping)
                Me.AddDataRow("EsitoNegativo_Scheda", "", dtMapping)

            Case ParsecAtt.EsitoControlloSuccessivoRegolaritaAmministrativa.Negativo
                Me.AddDataRow("EsitoNegativo_Scheda", "X", dtMapping)
                Me.AddDataRow("EsitoPositivo_Scheda", "", dtMapping)

        End Select



        Return dtMapping
    End Function

    Private Sub AddDataRow(ByVal nomeCampo As String, ByVal valoreCampo As String, ByRef dt As DataTable)
        Dim row As DataRow = dt.NewRow
        row("NomeCampo") = nomeCampo
        row("ValoreCampo") = valoreCampo
        dt.Rows.Add(row)
    End Sub

    Private Function CreaTabellaIndicatori() As DataTable
        Dim dtIndicatori As New DataTable("TabellaIndicatori")
        Dim dc1 As New DataColumn("Descrizione_Indicatore", GetType(System.String))
        Dim dc2 As New DataColumn("ValutazionePositiva_Indicatore", GetType(System.String))
        Dim dc3 As New DataColumn("ValutazioneNegativa_Indicatore", GetType(System.String))
        Dim dc4 As New DataColumn("ValutazioneNonApplicabile_Indicatore", GetType(System.String))
        Dim dc5 As New DataColumn("Note_Indicatore", GetType(System.String))

        dtIndicatori.Columns.Add(dc1)
        dtIndicatori.Columns.Add(dc2)
        dtIndicatori.Columns.Add(dc3)
        dtIndicatori.Columns.Add(dc4)
        dtIndicatori.Columns.Add(dc5)

        Dim siRadioButton As RadioButton
        Dim noRadioButton As RadioButton
        Dim noApplicabileRadioButton As RadioButton
        Dim noteLabel As Label = Nothing

        Dim row As DataRow = Nothing

        For Each item As Telerik.Web.UI.GridDataItem In Me.IndicatoriGridView.Items

            row = dtIndicatori.NewRow

            siRadioButton = CType(item.FindControl("SiRadioButton"), RadioButton)
            noRadioButton = CType(item.FindControl("NoRadioButton"), RadioButton)
            noApplicabileRadioButton = CType(item.FindControl("NoApplicabileRadioButton"), RadioButton)

            noteLabel = CType(item.FindControl("NoteTextBox"), Label)

            Dim descrizione = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Descrizione")
            row("Descrizione_Indicatore") = descrizione

            If siRadioButton.Checked Then
                row("ValutazionePositiva_Indicatore") = "X"
                row("ValutazioneNegativa_Indicatore") = String.Empty
                row("ValutazioneNonApplicabile_Indicatore") = String.Empty
            End If

            If noRadioButton.Checked Then
                row("ValutazioneNegativa_Indicatore") = "X"
                row("ValutazionePositiva_Indicatore") = String.Empty
                row("ValutazioneNonApplicabile_Indicatore") = String.Empty
            End If

            If noApplicabileRadioButton.Checked Then
                row("ValutazioneNonApplicabile_Indicatore") = "X"
                row("ValutazioneNegativa_Indicatore") = String.Empty
                row("ValutazionePositiva_Indicatore") = String.Empty

            End If

            row("Note_Indicatore") = noteLabel.Text



            dtIndicatori.Rows.Add(row)
        Next

        Return dtIndicatori
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

    Private Sub GeneraDataSource(ByVal scheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa)

        Dim controlliSuccessivi As New ParsecAtt.ControlloSuccessivoRegolaritaAmministrativaRepository
        Dim controllo = controlliSuccessivi.Where(Function(c) c.Id = scheda.IdControlloSuccessivoRegolaritaAmministrativa).FirstOrDefault

        Dim nomeFileTemplate As String = controllo.NomeFileTemplate

        Dim anno As String = Year(scheda.DataEstrazione).ToString

        Dim localPathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")
        Dim input As String = localPathTemplate & nomeFileTemplate





        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathSchedeControllo")
        Dim output As String = String.Format("{0}{1}\{2}", localPath, anno, scheda.Nomefile)

        If Not IO.File.Exists(input) Then
            Throw New ApplicationException(String.Format("Il file '{0}' non esiste!", nomeFileTemplate))
        End If

        If Not IO.Directory.Exists(localPath) Then
            IO.Directory.CreateDirectory(localPath)
        End If

        Dim pathCartellaAnno As String = String.Format("{0}{1}\", localPath, anno)

        If Not IO.Directory.Exists(pathCartellaAnno) Then
            IO.Directory.CreateDirectory(pathCartellaAnno)
        End If

        'COPIO IL TREMPLATE
        Me.CopiaDocumento(input, output)

        Dim srcHttpPath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates") & nomeFileTemplate
        Dim destHttpPath = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeSchedeControllo"), anno, scheda.Nomefile)
        Dim copyHttpPath As String = String.Empty


        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim tables As New List(Of DataTable)
        tables.Add(Me.CreaTabellaMapping(scheda))
        tables.Add(Me.CreaTabellaIndicatori)

        Dim datiInput As New ParsecAdmin.DatiInput
        datiInput.SrcRemotePath = srcHttpPath
        datiInput.DestRemotePath = destHttpPath
        datiInput.CopyRemotePath = copyHttpPath
        datiInput.ShowWindow = True
        datiInput.FunctionName = "ProcessDocument"

        Dim dataSource As String = openofficeParameters.CreateDataSource(New ParsecAdmin.DatiCredenziali, datiInput, tables)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(dataSource, Me.notificaOperazioneButton.ClientID, True, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(dataSource, True, AddressOf Me.NotificaOperazione)
        End If

    End Sub

    Private Sub NotificaOperazione()
        ParsecUtility.Utility.DoWindowClose(False)
        ParsecUtility.SessionManager.SchedaControlloSuccessivoRegolaritaAmministrativa = Me.SchedaControlloSuccessivoRegolaritaAmministrativa
    End Sub

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.RegisterComponent(script)
    End Sub

    Private Sub AggiornaVista(ByVal scheda As ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativa)

        Me.OggettoTextBox.Text = scheda.OggettoAtto
        Me.NumeroTextBox.Text = scheda.NumeroAtto
        Me.DataTextBox.Text = scheda.DataAtto.ToShortDateString
        Me.SettoreTextBox.Text = scheda.DescrizioneSettore
        Me.ResponsabileTextBox.Text = scheda.Responsabile
        Me.OsservazioniTextBox.Text = scheda.Osservazioni


        Select Case scheda.Esito
            Case ParsecAtt.EsitoControlloSuccessivoRegolaritaAmministrativa.Positivo
                Me.EsitoPositivoRadioButton.Checked = True
            Case ParsecAtt.EsitoControlloSuccessivoRegolaritaAmministrativa.Negativo
                Me.EsitoNegativoRadioButton.Checked = True
        End Select

    End Sub

    Private Sub AggiornaNote(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idIndicatore As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdIndicatore")

        Dim note = Me.IndicatoriConformita.Where(Function(c) c.IdIndicatore = idIndicatore).Select(Function(c) c.Annotazioni).FirstOrDefault
        Me.AnnotazioniTextBox.Text = note
        Me.IdIndicatoreHidden.Value = idIndicatore
    End Sub

    Private Sub VisualizzaDocumento()
        Dim nomefile As String = Me.SchedaControlloSuccessivoRegolaritaAmministrativa.Nomefile

        If Not String.IsNullOrEmpty(nomefile) Then
            Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
            Dim annoEsercizio As String = rgx.Match(nomefile).Value


            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathSchedeControllo"), annoEsercizio, nomefile)
            If Not IO.File.Exists(localPath) Then
                Throw New ApplicationException(String.Format("Il file '{0}' non esiste!", localPath.Replace("\", "/")))
            End If


            Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
            Dim pathDownload = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeSchedeControllo"), annoEsercizio, nomefile)
            Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = False, .FunctionName = "ViewDocument", .PrefixCopy = "", .PrefixOriginal = "", .Shadow = ""}
            Dim data As String = openofficeParameters.CreateOpenParameter(datiInput)

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            'UTILIZZO L'APPLET
            If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)
            Else
                'UTILIZZO IL SOCKET  
                ParsecUtility.Utility.EseguiServerComunicatorService(data, False, Nothing)
            End If
        End If
    End Sub

    Private Sub VisualizzaDocumentoFirmato()
        Dim nomefileFirmato As String = Me.SchedaControlloSuccessivoRegolaritaAmministrativa.NomefileFirmato
        If Not String.IsNullOrEmpty(nomefileFirmato) Then
            Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
            Dim annoEsercizio As String = rgx.Match(nomefileFirmato).Value

            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathSchedeControllo"), annoEsercizio, nomefileFirmato)
            If Not IO.File.Exists(localPath) Then
                Throw New ApplicationException(String.Format("Il file '{0}' non esiste!", localPath.Replace("\", "/")))
            End If

            Session("AttachmentFullName") = localPath
            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
            ParsecUtility.Utility.PageReload(pageUrl, False)
        End If
    End Sub


#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub IndicatoriGridView_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles IndicatoriGridView.NeedDataSource


        If Me.IndicatoriConformita Is Nothing Then
            Dim dettagliScheda As New ParsecAtt.DettaglioSchedaControlloSuccessivoRegolaritaAmministrativaRepository
            Dim indicatori As New ParsecAtt.IndicatoreControlloSuccessivoRegolaritaAmministrativaRepository(dettagliScheda.Context)

            Me.IndicatoriConformita = (From ds In dettagliScheda.GetQuery.Select(Function(c) New With {c.IdScheda, c.IdIndicatore, c.EsitoConformita, c.Annotazioni})
                                       Join i In indicatori.GetQuery.Select(Function(c) New With {c.Id, c.Descrizione})
                      On ds.IdIndicatore Equals i.Id
                                       Where ds.IdScheda = Me.SchedaControlloSuccessivoRegolaritaAmministrativa.Id
                                       Select New IndicatoreConformita With {
                           .Id = i.Id,
                           .Descrizione = i.Descrizione,
                           .IdIndicatore = ds.IdIndicatore,
                           .EsitoConformita = ds.EsitoConformita,
                           .Annotazioni = ds.Annotazioni
                          }).ToList

            dettagliScheda.Dispose()
        End If

        Me.IndicatoriGridView.DataSource = Me.IndicatoriConformita

    End Sub

    Protected Sub IndicatoriGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles IndicatoriGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaNote(e.Item)
        End Select
    End Sub


#End Region

End Class