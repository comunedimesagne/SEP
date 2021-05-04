Imports ParsecAdmin
Imports Telerik.Web.UI
Imports System.Data


Partial Class StampaGettoniPresenzeSedutePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Private Class DatiStampa
        Public Property DataPrimaConvocazione As Nullable(Of DateTime)
        Public Property Presenza As Boolean = False
        Public Property Struttura As String = String.Empty

    End Class

#Region "EVENTI PAGINA"


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Stampa Gettoni Presenze Organi"
        Me.StampaButton.Attributes.Add("onclick", "this.disabled=true;")

        Me.TipologieSedutaComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Consiglio Comunale", "3"))
        Me.TipologieSedutaComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Giunta Comunale", "2"))

        If Not Me.Page.IsPostBack Then
            Me.ResettaFiltro()
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If

       

    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub ResettaFiltro()
        Me.DataInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        Me.TipologieSedutaComboBox.SelectedIndex = 0
    End Sub

    Private Function ConvertiIndiceColonnaInLettera(ByVal indiceColonna As Integer) As String
        Dim div As Integer = indiceColonna
        Dim letteraColonna As String = String.Empty
        Dim modnum As Integer = 0

        While div > 0
            modnum = (div - 1) Mod 26
            letteraColonna = Chr(65 + modnum) & letteraColonna
            div = CInt((div - modnum) \ 26)
        End While

        Return letteraColonna
    End Function

    Private Sub Print()



        Dim idTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
        Dim dataInizio = Me.DataInizioTextBox.SelectedDate.Value
        Dim dataFine = Me.DataFineTextBox.SelectedDate.Value

        Dim startDate As Date = New Date(dataInizio.Year, dataInizio.Month, dataInizio.Day, 0, 0, 0)
        Dim endDate As Date = New Date(dataFine.Year, dataFine.Month, dataFine.Day, 23, 59, 59)


        Dim template As String = "templateReportGettoni.ods"
        Dim localPathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiTemplates")
        Dim input As String = localPathTemplate & template

        If Not IO.File.Exists(input) Then
            Throw New ApplicationException(String.Format("Il(file) '{0}' non esiste!", input))
        End If

        Dim presenzeSeduta As New ParsecAtt.PresenzaSedutaRepository
        Dim sedute As New ParsecAtt.SedutaRepository(presenzeSeduta.Context)
        Dim strutture As New ParsecAtt.StrutturaViewRepository(presenzeSeduta.Context)

        Dim presenzeDocumento As New ParsecAtt.DocumentoPresenzaRepository(presenzeSeduta.Context)
        Dim documenti As New ParsecAtt.DocumentoRepository(presenzeSeduta.Context)


        'Dim datePrimaConvocazione = (From presenza In presenzeSeduta.GetQuery
        '                             Join seduta In sedute.GetQuery On presenza.IdSeduta Equals seduta.Id
        '                             Join struttura In strutture.GetQuery On presenza.CodiceStruttura Equals struttura.Codice
        '                             Order By seduta.DataPrimaConvocazione
        '                             Where seduta.IdTipologiaSeduta = idTipologiaSeduta And seduta.DataPrimaConvocazione >= startDate And seduta.DataPrimaConvocazione <= endDate
        '                             Select seduta.DataPrimaConvocazione).Distinct.ToList



        Dim datePrimaConvocazione = (From presenza In presenzeDocumento.GetQuery
                                      Join documento In documenti.GetQuery On presenza.IdDocumento Equals documento.Id
                                      Join seduta In sedute.GetQuery On seduta.Id Equals documento.IdSeduta
                                      Join struttura In strutture.GetQuery On presenza.IdStruttura Equals struttura.Id
                                      Order By seduta.DataPrimaConvocazione
                                      Where documento.IdTipologiaDocumento = 4 And documento.LogStato Is Nothing And seduta.IdTipologiaSeduta = idTipologiaSeduta And seduta.DataPrimaConvocazione >= startDate And seduta.DataPrimaConvocazione <= endDate
                                      Select seduta.DataPrimaConvocazione).Distinct.ToList



        'Dim nominativi = (From presenza In presenzeSeduta.GetQuery
        '                  Join seduta In sedute.GetQuery On presenza.IdSeduta Equals seduta.Id
        '                  Join struttura In strutture.GetQuery On presenza.CodiceStruttura Equals struttura.Codice
        '                  Where seduta.IdTipologiaSeduta = idTipologiaSeduta And seduta.DataPrimaConvocazione >= startDate And seduta.DataPrimaConvocazione <= endDate
        '                  Order By struttura.Descrizione
        '                  Select struttura.Descrizione).Distinct.ToList



        Dim nominativi = (From presenza In presenzeDocumento.GetQuery
                           Join documento In documenti.GetQuery On presenza.IdDocumento Equals documento.Id
                           Join seduta In sedute.GetQuery On seduta.Id Equals documento.IdSeduta
                           Join struttura In strutture.GetQuery On presenza.IdStruttura Equals struttura.Id
                           Where struttura.LogStato Is Nothing And documento.IdTipologiaDocumento = 4 And documento.LogStato Is Nothing And seduta.IdTipologiaSeduta = idTipologiaSeduta And seduta.DataPrimaConvocazione >= startDate And seduta.DataPrimaConvocazione <= endDate
                           Order By struttura.Descrizione
                           Select struttura.Descrizione
                          ).Distinct.ToList




        Dim presenze = From presenza In presenzeDocumento.GetQuery
                       Join documento In documenti.GetQuery On presenza.IdDocumento Equals documento.Id
                       Join seduta In sedute.GetQuery
                       On documento.IdSeduta Equals seduta.Id
                       Join struttura In strutture.GetQuery
                       On presenza.IdStruttura Equals struttura.Id
                       Where documento.IdTipologiaDocumento = 4 And documento.LogStato Is Nothing And seduta.IdTipologiaSeduta = idTipologiaSeduta And seduta.DataPrimaConvocazione >= startDate And seduta.DataPrimaConvocazione <= endDate
                       Order By seduta.DataPrimaConvocazione, struttura.Descrizione
                       Select New DatiStampa With {.Struttura = struttura.Descrizione, .Presenza = presenza.Presente, .DataPrimaConvocazione = seduta.DataPrimaConvocazione}

        Dim view = presenze.AsEnumerable '.GroupBy(Function(c) c.Struttura).Select(Function(c) c.First()).ToList


        If view.Count > 0 Then
            Dim ht As New Hashtable
            ht.Add("A1", "Elenco presenze di " & Me.TipologieSedutaComboBox.SelectedItem.Text & " per le sedute dal " & Me.DataInizioTextBox.SelectedDate.Value.ToShortDateString & " al " + Me.DataFineTextBox.SelectedDate.Value.ToShortDateString & ", per il periodo specificato risultano " & datePrimaConvocazione.Count.ToString & " Sedute.")


            Dim colonna As Integer = 2
            Dim riga As Integer = 3

            Dim letteraColonna = Me.ConvertiIndiceColonnaInLettera(colonna)
            For Each v As Nullable(Of DateTime) In datePrimaConvocazione
                ht.Add(letteraColonna & riga.ToString, v.Value.ToShortDateString)
                colonna += 1
                letteraColonna = Me.ConvertiIndiceColonnaInLettera(colonna)
            Next

            riga = 4

            For Each v As String In nominativi
                Dim nominativo = v
                ht.Add("A" & riga.ToString, nominativo)

                colonna = 2
                letteraColonna = Me.ConvertiIndiceColonnaInLettera(colonna)
                For Each c As Nullable(Of DateTime) In datePrimaConvocazione
                    Dim dataPrimaConvocazione = c
                    Dim p = view.Where(Function(cc) cc.Struttura = nominativo And cc.DataPrimaConvocazione = dataPrimaConvocazione And cc.Presenza = True).FirstOrDefault
                    If Not p Is Nothing Then
                        ht.Add(letteraColonna & riga.ToString, "X")
                    End If
                    colonna += 1
                    letteraColonna = Me.ConvertiIndiceColonnaInLettera(colonna)
                Next
                riga += 1
            Next

            Dim openOfficeParameters As New ParsecAdmin.OpenOfficeParameters


            Dim remotePath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti") & Now.Year.ToString & "/"
            Dim remotePathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates")

            'Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            'Dim dataFilename As String = utenteCollegato.Id.ToString & "_" & Now.Ticks.ToString & ".xml"
            'Dim dataLocalPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & dataFilename
            'Dim dataRemotePath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocumentiTemp") & dataFilename


            Dim datiCelle As New ParsecAdmin.DatiCelle(ht)

            'openOfficeParameters.SerializeDataSourceToFile(datiCelle.ToTable, dataLocalPath)


            Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = remotePathTemplate & template,
                                                           .DestRemotePath = remotePath & "Report.ods",
                                                           .ShowWindow = True,
                                                           .Enabled = True,
                                                           .FunctionName = "ProcessDocument"}

            Dim data As String = openOfficeParameters.CreateDataSource(datiInput, datiCelle)

            '************************************************************************************************************************
            'SCRIVO IL FILE CHE CONTIENE IL DATASOURCE 
            'LA LUNGHEZZA MASSIMA DELLA LINEA DI COMANDO E' 32K (32768 byte) 16384 CARATTERI
            '************************************************************************************************************************

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim localFilenameDataSource = "Utente_" & utenteCollegato.Id.ToString & "_" & Guid.NewGuid.ToString & ".txt"
            Dim localPathDataSource As String = String.Format("{0}{1}", ParsecAdmin.WebConfigSettings.GetKey("PathDownload"), localFilenameDataSource)
            IO.File.WriteAllText(localPathDataSource, data)
            Dim dataSource = openOfficeParameters.CreateDataSource(localFilenameDataSource)

            '************************************************************************************************************************

            'UTILIZZO L'APPLET
            If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(dataSource, Me.notificaOperazioneButton.ClientID, True, False)
            Else
                'UTILIZZO IL SOCKET
                ParsecUtility.Utility.EseguiServerComunicatorService(dataSource, True, AddressOf Me.NotificaOperazione)
            End If

        Else
            ParsecUtility.Utility.MessageBox("Non ci sono sedute per il periodo indicato!", False)
        End If

    End Sub


    Protected Sub notificaOperazioneButton_Click(sender As Object, e As System.EventArgs) Handles notificaOperazioneButton.Click
        Me.NotificaOperazione()
    End Sub

    Private Sub NotificaOperazione()
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

#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub AnnullaButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

    Protected Sub StampaButton_Click(sender As Object, e As System.EventArgs) Handles StampaButton.Click
        Try
            Me.Print()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox("Impossibile eseguire l'operazione per il seguente motivo:" & vbCrLf & ex.Message.Replace("\", "/"), False)
        End Try

        Me.StampaButton.Enabled = True
    End Sub

#End Region

#Region "SCRIPT PARSECOPENOFFICE"


    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

End Class