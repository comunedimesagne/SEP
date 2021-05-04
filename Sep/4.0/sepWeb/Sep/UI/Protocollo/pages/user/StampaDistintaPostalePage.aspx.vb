Imports System.Data.SqlClient

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StampaDistintaPostalePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> Stampa Distinta Postale"

        Me.ResettaFiltro()

        Dim tipiRicezione As New ParsecPro.TipiRicezioneInvioRepository
        Me.TipoRicezioneInvioListBox.DataSource = tipiRicezione.GetView(Nothing)
        Me.TipoRicezioneInvioListBox.DataValueField = "Id"
        Me.TipoRicezioneInvioListBox.DataTextField = "Descrizione"
        Me.TipoRicezioneInvioListBox.DataBind()
        tipiRicezione.Dispose()

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If

        Dim script As New StringBuilder
        script.AppendLine("var value =  $find('" & Me.NumeroProtocolloInizioTextBox.ClientID & "').get_displayValue(); var textbox =  $find('" & Me.NumeroProtocolloFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")
        Me.NumeroProtocolloInizioTextBox.Attributes.Add("onblur", script.ToString)

    End Sub


#End Region

    'Metodo che costruisce e restituisce il Filtro per la ricerca.
    Private Function GetFiltro() As ParsecPro.RegistrazioneFiltro

        Dim listaIdInvio As New List(Of Integer)

        For Each it As Telerik.Web.UI.RadListBoxItem In Me.TipoRicezioneInvioListBox.CheckedItems
            listaIdInvio.Add(CInt(it.Value))
        Next

        Dim numeroProtocolloInizio As Nullable(Of Int32)
        Dim numeroProtocolloFine As Nullable(Of Int32)

        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloInizioTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloInizioTextBox.Text) Then
                numeroProtocolloInizio = CInt(Me.NumeroProtocolloInizioTextBox.Text)
            End If
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloFineTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloFineTextBox.Text) Then
                numeroProtocolloFine = CInt(Me.NumeroProtocolloFineTextBox.Text)
            End If
        End If

        Dim filtro As New ParsecPro.RegistrazioneFiltro With
            {
                .DataProtocolloInizio = Me.DataProtocolloInizioTextBox.SelectedDate,
                .DataProtocolloFine = Me.DataProtocolloFineTextBox.SelectedDate,
                .NumeroProtocolloInizio = numeroProtocolloInizio,
                .NumeroProtocolloFine = numeroProtocolloFine,
                .ElencoId = listaIdInvio
            }

        Return filtro

    End Function

    'Cancella i file temporanei di tipo xml
    Private Sub DeleteFileTemporanei(ByVal utenteCollegato As ParsecAdmin.Utente)
        Dim localTempPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim files As String() = IO.Directory.GetFiles(localTempPath, "*.xml")
        For Each f As String In files
            If f.Contains(utenteCollegato.Id.ToString & "_") Then
                IO.File.Delete(f)
            End If
        Next
    End Sub

    'Stampa la distinta postale avendo come template il modello "templateProTrasmissione.odt"
    Private Sub Print()

        Dim template As String = "templateProTrasmissione.odt"
        Dim localPathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")
        Dim input As String = localPathTemplate & template

        If Not IO.File.Exists(input) Then
            Throw New ApplicationException(String.Format("Il file '{0}' non esiste!", template))
        End If

        Dim annoEsercizio As Integer = Now.Year
        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")
        Dim pathCartellaAnno As String = String.Format("{0}{1}\", localPath, annoEsercizio)

        If Not IO.Directory.Exists(localPath) Then
            IO.Directory.CreateDirectory(localPath)
        End If

        If Not IO.Directory.Exists(pathCartellaAnno) Then
            IO.Directory.CreateDirectory(pathCartellaAnno)
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Me.DeleteFileTemporanei(utenteCollegato)

        Dim protocolli As New ParsecPro.RegistrazioniRepository
        Dim filtro As ParsecPro.RegistrazioneFiltro = Me.GetFiltro
        Dim res As System.Data.DataTable = protocolli.GetDistintePostali(filtro)

        If res Is Nothing Then
            ParsecUtility.Utility.MessageBox("Nessuna registrazione trovata con i criteri di filtro impostati!", False)
            Exit Sub
        End If
        res.TableName = "TabellaTrasmissioneProtocolli"

        protocolli.Dispose()

        Dim tipologieRicezioneInvio As New ParsecPro.TipiRicezioneInvioRepository
        Dim lista As List(Of String) = (From tipo In tipologieRicezioneInvio.GetQuery.ToList
                                      Where filtro.ElencoId.Contains(tipo.Id)
                                      Select tipo.Descrizione).ToList

        Dim elencoTipologieRicezioneInvio As String = String.Join(", ", lista)

        tipologieRicezioneInvio.Dispose()


        Dim openOfficeParameters As New ParsecAdmin.OpenOfficeParameters

        Dim remotePath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti") & annoEsercizio.ToString & "/"
        Dim remotePathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates")

        Dim dataFilename As String = utenteCollegato.Id.ToString & "_" & Now.Ticks.ToString & ".xml"
        Dim dataLocalPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & dataFilename
        Dim dataRemotePath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocumentiTemp") & dataFilename

        openOfficeParameters.SerializeDataSourceToFile(res, dataLocalPath)

        Dim parametri As New Hashtable
        parametri.Add("DATA_CORRENTE", String.Format("{0:dd/MM/yyyy}", Now))
        parametri.Add("TIPO_RICEZIONE_INVIO", elencoTipologieRicezioneInvio)

        Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = remotePathTemplate & template,
                                                         .DestRemotePath = remotePath & "Report.odt",
                                                         .ShowWindow = True,
                                                         .Enabled = True,
                                                         .DataRemotePath = dataRemotePath,
                                                         .FunctionName = "ProcessDocument"}

        Dim data As String = openOfficeParameters.CreateDataSource(datiInput, New ParsecAdmin.DatiCampiUtente(parametri))

        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(data, "", False, False)
        Else
            'UTILIZZO IL SOCKET
            ParsecUtility.Utility.EseguiServerComunicatorService(data, False, Nothing)
        End If

    End Sub

    'Metodo che invoca la stampa. Prima convalida i parametri di ricerca.
    Protected Sub AnteprimaStampaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnteprimaStampaButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Try
                Me.Print()
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    'Invia una mail: apre la maschera InvioMailDistintaPostale.aspx. Prima convalida i parametri
    Protected Sub inviaMailButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles inviaMailButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Try
                'Me.Print()
                Dim pageUrl As String = "~/UI/Protocollo/pages/user/InvioMailDistintaPostale.aspx"
                Dim queryString As New Hashtable
                ParsecUtility.SessionManager.FiltroRegistrazioneProtocollo = Me.GetFiltro
                ParsecUtility.Utility.ShowPopup(pageUrl, 900, 780, queryString, False)

            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    'Metodo che resetta il filtro resettando la pagina. Richiama il metodo ResettaFiltro
    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

    'Convalida gli eventuali parametri di ricerca
    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean
      If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloInizioTextBox.Text)) Then
            If ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloInizioTextBox.Text) Then
                message.AppendLine("Se specificato, il campo 'Numero protocollo da' deve essere un numero!")
            End If
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloFineTextBox.Text)) Then
            If ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloFineTextBox.Text) Then
                message.AppendLine("Se specificato, il campo 'Numero protocollo a' deve essere un numero!")
            End If
        End If
        Return Not message.Length > 0
    End Function

    'Resetta i campi della Maschera
    Private Sub ResettaFiltro()
        'Dal primo dell'anno a oggi
        Me.DataProtocolloInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataProtocolloFineTextBox.SelectedDate = Now
        Me.NumeroProtocolloInizioTextBox.Text = String.Empty
        Me.NumeroProtocolloFineTextBox.Text = String.Empty
        Me.TipoRicezioneInvioListBox.ClearChecked()
        Me.TipoRicezioneInvioListBox.ClearSelection()
        Me.SelectAllCheckBox.Checked = False

      End Sub

    'Seleziona/Deseleziona le voci contenuti in TipoRicezioneInvioListBox
    Protected Sub SelectAllCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectAllCheckBox.CheckedChanged
        Dim checked As Boolean = Me.SelectAllCheckBox.Checked
        For i As Integer = 0 To Me.TipoRicezioneInvioListBox.Items.Count - 1
            'Me.TipoRicezioneInvioListBox.Items(i).Selected = checked
            Me.TipoRicezioneInvioListBox.Items(i).Checked = checked
        Next
    End Sub

#Region "Script ParsecOpenOffice"

    'Registra l' openOfficenella pagina per poterlo utilizzare
    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

  
End Class