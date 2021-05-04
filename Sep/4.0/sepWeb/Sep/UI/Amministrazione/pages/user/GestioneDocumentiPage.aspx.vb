Imports ParsecAdmin
Imports System.Net
Imports Telerik.Web.UI
Imports System.Web.Services


Partial Class GestioneDocumentiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Public Property DocumentoGenerico() As ParsecAdmin.DocumentoGenerico
        Get
            Return CType(Session("GestioneDocumentiPage_DocumentoGenerico"), ParsecAdmin.DocumentoGenerico)
        End Get
        Set(ByVal value As ParsecAdmin.DocumentoGenerico)
            Session("GestioneDocumentiPage_DocumentoGenerico") = value
        End Set
    End Property

    Public Property DocumentiGenerici() As List(Of ParsecAdmin.DocumentoGenerico)
        Get
            Return CType(Session("GestioneDocumentiPage_DocumentiGenerici"), List(Of ParsecAdmin.DocumentoGenerico))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.DocumentoGenerico))
            Session("GestioneDocumentiPage_DocumentiGenerici") = value
        End Set
    End Property

    Public Property Destinatari() As List(Of ParsecAdmin.DocumentoGenericoReferenteEsterno)
        Get
            Return CType(Session("GestioneDocumentiPage_Destinatari"), List(Of ParsecAdmin.DocumentoGenericoReferenteEsterno))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.DocumentoGenericoReferenteEsterno))
            Session("GestioneDocumentiPage_Destinatari") = value
        End Set
    End Property

    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("GestioneDocumentiPage_SelectedItems") Is Nothing Then
                Session("GestioneDocumentiPage_SelectedItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("GestioneDocumentiPage_SelectedItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("GestioneDocumentiPage_SelectedItems") = value
        End Set
    End Property


    Public Property IdRegistrazione As Nullable(Of Integer)
        Get
            Return Session("GestioneDocumentiPage_IdRegistrazione")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            Session("GestioneDocumentiPage_IdRegistrazione") = value
        End Set
    End Property

    Public Property Modulo As Nullable(Of Integer)
        Get
            Return Session("GestioneDocumentiPage_Modulo")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            Session("GestioneDocumentiPage_Modulo") = value
        End Set
    End Property


    Public Property Oggetto As String
        Get
            Return Session("GestioneDocumentiPage_Oggetto")
        End Get
        Set(ByVal value As String)
            Session("GestioneDocumentiPage_Oggetto") = value
        End Set
    End Property


    Public Property IdSegnalazione As Nullable(Of Integer)
        Get
            Return Session("GestioneDocumentiPage_IdSegnalazione")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            Session("GestioneDocumentiPage_IdSegnalazione") = value
        End Set
    End Property
   


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BasePage.master"
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.Request("Mode") Is Nothing Then
            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Amministrazione"
            Me.MainPage.DescrizioneProcedura = "> Gestione Documenti"

            Me.DocumentiGenericiTabStrip.Tabs(1).Visible = False

        Else
            Me.MainPage = CType(Me.Master, BasePage)
            CType(Me.Master, BasePage).DescrizioneProcedura = "Dettaglio Documento"

            '****************************************************************************************************
            'Nascondo la lista dei protocolli.
            Me.DocumentiPanel.Style.Add("display", "none")
            Me.ChiudiButton.Visible = True

            'Disabilito tutti i pulsanti della toolbar
            For i As Integer = 0 To Me.RadToolBar.Items.Count - 1
                Me.RadToolBar.Items(i).Enabled = False
            Next
            'Abilito solo il pulsante salva
            Me.RadToolBar.Items.FindItemByText("Salva").Enabled = True
            '****************************************************************************************************
        End If

        Me.GetParametri()

        If Not Me.Page.IsPostBack Then

            Me.SalvaButton.Enabled = False

            Me.ResettaVista()
            Me.SelectedItems = Nothing
            Me.DocumentiGenerici = Nothing

            If Me.IdRegistrazione.HasValue Then
                Me.AggiornaVistaDaRegistrazione(Me.IdRegistrazione.Value)
            End If

            If Me.IdSegnalazione.HasValue Then
                If Me.Modulo = ParsecAdmin.TipoModulo.WBT Then
                    Me.OggettoTextBox.Text = Me.Oggetto
                    Me.Oggetto = String.Empty
                End If
            End If

            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "NomeModello"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.DocumentiGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
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
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        ParsecUtility.Utility.CloseWindow(False)
        '***************************************************************************


        Me.DisabilitaPulsantePredefinito.Attributes.Add("onclick", "return false;")
        ParsecUtility.Utility.RegisterDefaultButton(Me.FiltroDenominazioneTextBox, Me.TrovaDestinatarioImageButton)

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.UtentiPanel.Style.Add("width", widthStyle)
        Me.DocumentiPanel.Style.Add("width", widthStyle)
        Me.DestinatariGridView.Style.Add("width", widthStyle)
        Me.DocumentiGridView.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.TitoloElencoDestinatariLabel.Text = "Elenco Destinatari&nbsp;&nbsp;" & If(Me.Destinatari.Count > 0, "( " & Me.Destinatari.Count.ToString & " )", "")
        If Not Me.DocumentiGenerici Is Nothing Then
            Me.TitoloElencoDocumentiLabel.Text = "Elenco Documenti&nbsp;&nbsp;" & If(Me.DocumentiGenerici.Count > 0, "( " & Me.DocumentiGenerici.Count.ToString & " )", "")
        End If
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'elemento selezionato?", False, Not Me.DocumentoGenerico Is Nothing)
        If Me.DestinatariGridView.SelectedItems.Count > 0 Then
            Dim message As String = "Eliminare tutti gli elementi selezionati?"
            Me.EliminaDestinatariSelezionatiImageButton.Attributes.Add("onclick", "return confirm(""" & message & """)")
        Else
            Dim message As String = "E' necessario selezionare almeno un addetto!"
            Me.EliminaDestinatariSelezionatiImageButton.Attributes.Add("onclick", "alert(""" & message & """); return false;")
        End If


        'SELEZIONO LA RIGA
        If Not Me.DocumentoGenerico Is Nothing Then
            Dim item As GridDataItem = Me.DocumentiGridView.MasterTableView.FindItemByKeyValue("Id", Me.DocumentoGenerico.Id)
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
                Me.Print()
            Case "Salva"

                Dim message As String = String.Empty

                'DISABILITO IL PULSANTE SALVA DELLA TOOLBAR
                Me.RadToolBar.FindItemByText("Salva").Enabled = False

                Try
                    Me.Save()
                    Me.AggiornaGriglia()
                   
                Catch ex As ApplicationException
                    message = ex.Message
                End Try

                'Dim sm As ScriptManager = ScriptManager.GetCurrent(Me.Page)
                'sm.RegisterDataItem(Me.messaggio, message)

                If String.IsNullOrEmpty(message) Then
                    '*********************************************************************************
                    'Registro gli script usati per l'elaborazione del documento.
                    '*********************************************************************************
                    Me.RegistraScriptOpenOffice()
                Else
                    Me.RadToolBar.FindItemByText("Salva").Enabled = True
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
                    If Not Me.DocumentoGenerico Is Nothing Then
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
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un documento!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
                Me.ResettaGridView()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "GESTIONE CHIUSURA FINESTRA POPUP"


    Protected Sub SalvaButton_Click(sender As Object, e As System.EventArgs) Handles SalvaButton.Click

        Dim message As String = String.Empty
       
        If Not String.IsNullOrEmpty(Me.NomeFileDocumentoLabel.Text) Then
            Try
                Me.SalvaDocumentoGenerico()
            Catch ex As Exception
                message = ex.Message
            End Try
            If String.IsNullOrEmpty(message) Then
                Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"

                If Me.IdRegistrazione.HasValue Then
                    ParsecUtility.SessionManager.DocumentoGenerico = Me.DocumentoGenerico
                    ParsecUtility.Utility.DoWindowClose(False)
                End If

                If Me.IdSegnalazione.HasValue Then
                    ParsecUtility.SessionManager.DocumentoGenerico = Me.DocumentoGenerico
                    ParsecUtility.Utility.DoWindowClose(False)
                End If
            Else
                ParsecUtility.Utility.MessageBox(message, False)
            End If

        Else
            'TODO
        End If

    End Sub


    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click

        'NON E' NECESSARIO 
        'ParsecUtility.Utility.ClosePopup(False)
        ParsecUtility.Utility.DoWindowClose(False)

    End Sub


#End Region


#Region "EVENTI GRIGLIA"

    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand


        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If

        If e.CommandName = "Preview" Then


            Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
            Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
            Dim documenti As New ParsecAdmin.DocumentiGenericiRepository
            Dim documento As ParsecAdmin.DocumentoGenerico = documenti.GetById(id)
            Dim anno As String = documento.DataCreazione.Value.Year.ToString


            Dim sorgenti As New ParsecAdmin.SorgentiRepository
            'Dim sorgentePredefinita As ParsecAdmin.Sorgente = sorgenti.GetDefault
            Dim sorgenteCorrente As ParsecAdmin.Sorgente = sorgenti.GetById(documento.IdSorgente)
            sorgenti.Dispose()

            Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgenteCorrente.NomeSezioneParametri)



            Dim fileExists As Boolean = False
            Select Case sorgenteCorrente.IdTipologia
                Case 1 'Locale
                    Dim localPath As String = source.Path & anno & "\" & documento.NomeFile
                    fileExists = IO.File.Exists(localPath)
                Case 2  'Ftp
                    Dim relativePath As String = source.RelativePath & anno & "/" & documento.NomeFile
                    fileExists = ParsecUtility.Utility.FtpFileExist(source.Path, source.Port, relativePath, source.Username, source.Password)
            End Select


            If Not fileExists Then
                ParsecUtility.Utility.MessageBox("Il file del documento non esiste!", False)
            Else

                Dim modello = documenti.GetModello(documento.IdModello)
                Dim visibile As Boolean = False

                If modello.VisibilitaPubblica.HasValue Then
                    visibile = modello.VisibilitaPubblica
                End If

                If Not visibile Then
                    visibile = (utenteCollegato.SuperUser OrElse documento.IdUtente = utenteCollegato.Id)
                End If

                If visibile Then
                    Me.VisualizzaDocumento(documento.NomeFile, anno, True, source)
                Else
                    ParsecUtility.Utility.MessageBox("Non si possiedono le autorizzazioni per visualizzare il documento!", False)
                End If

            End If
        End If
    End Sub

    Protected Sub DocumentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DocumentiGridView.NeedDataSource
        If Me.DocumentiGenerici Is Nothing Then
            Dim documenti As New ParsecAdmin.DocumentiGenericiRepository
            Me.DocumentiGenerici = documenti.GetView(Nothing)
            documenti.Dispose()
        End If
        Me.DocumentiGridView.DataSource = Me.DocumentiGenerici
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub AggiornaGriglia()
        Me.DocumentiGenerici = Nothing
        Me.DocumentiGridView.Rebind()
    End Sub

    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("IdRegistrazioneIter") Then
                Me.IdRegistrazione = parametriPagina("IdRegistrazioneIter")
            End If
            If parametriPagina.ContainsKey("Modulo") Then
                Me.Modulo = parametriPagina("Modulo")
            End If

            If parametriPagina.ContainsKey("Oggetto") Then
                Me.Oggetto = parametriPagina("Oggetto")
            End If

            If parametriPagina.ContainsKey("IdSegnalazione") Then
                Me.IdSegnalazione = parametriPagina("IdSegnalazione")
            End If

            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If

    End Sub

    Private Sub Print()
        'Dim parametriStampa As New Hashtable
        'parametriStampa.Add("TipologiaStampa", "StampaQualificheOrganigramma")
        'parametriStampa.Add("DatiStampa", Me.Avvisi)
        'Session("ParametriStampa") = parametriStampa
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Dim documenti As New ParsecAdmin.DocumentiGenericiRepository
        Try
            documenti.Delete(Me.DocumentoGenerico)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            documenti.Dispose()
        End Try
    End Sub

    Private Function GetFiltro() As ParsecAdmin.DocumentoGenericoFiltro
        Dim filtro As New ParsecAdmin.DocumentoGenericoFiltro
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            filtro.Oggetto = Me.OggettoTextBox.Text
        End If
        'If Not String.IsNullOrEmpty(Me.IdModelloTextBox.Text) Then
        '    filtro.IdModello = CInt(Me.IdModelloTextBox.Text)
        'End If

        If Not String.IsNullOrEmpty(Me.ModelliComboBox.SelectedValue) Then
            filtro.IdModello = CInt(Me.ModelliComboBox.SelectedValue)
        End If
        If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
            filtro.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        End If
        Return filtro
    End Function

    Private Sub Search()
        Dim documenti As New ParsecAdmin.DocumentiGenericiRepository
        Dim searchTemplate As ParsecAdmin.DocumentoGenericoFiltro = Me.GetFiltro()
        Me.DocumentiGenerici = documenti.GetView(searchTemplate)
        Me.DocumentiGridView.Rebind()
        documenti.Dispose()
    End Sub

    '*********************************************************************************************
    'SALVO IL DOCUMENTO GENERICO A PARTIRE DA UN FILE ESISTENTE
    '*********************************************************************************************
    Private Sub SalvaDocumentoGenerico()

        Dim estensione As String = IO.Path.GetExtension(Me.NomeFileDocumentoLabel.Text)

        Dim documenti As New ParsecAdmin.DocumentiGenericiRepository
        Dim documento As ParsecAdmin.DocumentoGenerico = New ParsecAdmin.DocumentoGenerico
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        documento.Oggetto = Me.OggettoTextBox.Text
        documento.IdUtente = utenteCollegato.Id
        documento.GeneratoSistemaSEP = False

        If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
            documento.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        End If

        Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
        Dim modello = modelli.Where(Function(c) c.VisibilitaPubblica = True).FirstOrDefault
        modelli.Dispose()
        If Not modello Is Nothing Then
            documento.IdModello = modello.Id
        End If

        documento.TipoGenerazione = ParsecAdmin.TipologiaGenerazione.FromTemplate 'INSERIMENTO è sempre da template

        Dim nuovoId As Integer = documenti.GetNuovoId

        Dim anno As String = String.Empty

        documento.Codice = nuovoId
        documento.DataCreazione = Now
        anno = Year(documento.DataCreazione).ToString
        documento.NomeFile = "doc" & "_" & anno & "_" & nuovoId.ToString & "_v" & documento.Versione.ToString & estensione
        documento.PercorsoRelativo = anno & "\"

        documento.Destinatari = Me.Destinatari

        documento.DescrizioneUfficio = "" ' Me.UfficioTextBox.Text
        documento.ObbligoDestinatari = False ' Me.ObbligoDestinatariCheckBox.Checked

        Dim success As Boolean = True
        Try
            '*******************************************************************
            'Gestione storico non utilizzata.
            '*******************************************************************
            documenti.DocumentoGenerico = Me.DocumentoGenerico
            documenti.Save(documento)


            '*******************************************************************
            'Aggiorno l'oggetto corrente 
            '*******************************************************************
            ' Me.AggiornaVista(documenti.DocumentoGenerico)

            Me.DocumentoGenerico = documenti.DocumentoGenerico
            documenti.Dispose()



            Dim sorgenti As New ParsecAdmin.SorgentiRepository
            Dim sorgentePredefinita As ParsecAdmin.Sorgente = sorgenti.GetDefault
            sorgenti.Dispose()

            Dim source As ParsecAdmin.SourceElement = ParsecAdmin.WebConfigSettings.GetSource(sorgentePredefinita.NomeSezioneParametri)

            Dim destLocalPath As String = source.Path & anno

            If Not IO.Directory.Exists(destLocalPath) Then
                IO.Directory.CreateDirectory(destLocalPath)
            End If


            IO.File.Copy(Me.NomeFileDocumentoLabel.Text, destLocalPath & "/" & documento.NomeFile, True)

            IO.File.Delete(Me.NomeFileDocumentoLabel.Text)


        Catch ex As Exception
            success = False
            Throw New ApplicationException(ex.Message)
        Finally
            documenti.Dispose()
        End Try



    End Sub


    Private Sub SalvaContenuto()
        Me.SaveDocumentContent()
        Dim message As String = "Operazione conclusa con successo!"
        Me.infoOperazioneHidden.Value = message
        Me.RadToolBar.FindItemByText("Salva").Enabled = True
        'Se la pagina è stata aperta come popup
        If Me.IdRegistrazione.HasValue Then
            ParsecUtility.SessionManager.DocumentoGenerico = Me.DocumentoGenerico
            'ParsecUtility.Utility.ClosePopup(False)
            ParsecUtility.Utility.DoWindowClose(False)
        End If

        If Me.IdSegnalazione.HasValue Then
            ParsecUtility.SessionManager.DocumentoGenerico = Me.DocumentoGenerico
            ParsecUtility.Utility.DoWindowClose(False)
        End If

    End Sub
 
   


    Protected Sub salvaContenutoButton_Click(sender As Object, e As System.EventArgs) Handles salvaContenutoButton.Click
        Me.SalvaContenuto()
    End Sub

    Private Sub NotificaContenuto(ByVal data As String)
        Me.documentContentHidden.Value = data
    End Sub

    Private Sub SaveDocumentContent()
        If Not String.IsNullOrEmpty(Me.documentContentHidden.Value) Then
            Dim idModulo As Integer = 10
            Dim contenuto As New ContenutoDocumentiRepository
            contenuto.Save(Me.documentContentHidden.Value, idModulo, Me.DocumentoGenerico)
            contenuto.Dispose()
            Me.documentContentHidden.Value = String.Empty
        End If

    End Sub

    Private Sub AggiornaVistaDaModello(ByVal registrazione As ParsecPro.Registrazione)

        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        'documento.IdUtente = utenteCollegato.Id

        For Each dest In registrazione.Destinatari.Where(Function(c) c.Interno = True).ToList
            Me.IdUfficioTextBox.Text = dest.Id
            Me.UfficioTextBox.Text = dest.Descrizione
            Exit For
        Next


        If Me.Modulo = ParsecAdmin.TipoModulo.PRO Then
            Me.OggettoTextBox.Text = "Risposta al Protocollo N. " & registrazione.NumeroProtocollo & "/" & "" & IIf(registrazione.TipoRegistrazione = 0, "A", IIf(registrazione.TipoRegistrazione = 1, "P", "I")) & "" & "/" & registrazione.DataImmissione.Value.Year & " con Oggetto : " & registrazione.Oggetto & "  -"
            Me.Modulo = Nothing
            If CType(registrazione.TipoRegistrazione, ParsecPro.TipoRegistrazione) = ParsecPro.TipoRegistrazione.Interna Then
                Me.ObbligoDestinatariCheckBox.Checked = False
                Me.ObbligoDestinatariCheckBox.Enabled = False
            End If
        End If

        If Me.Modulo = ParsecAdmin.TipoModulo.IOL Then
            Me.OggettoTextBox.Text = String.Format(Me.Oggetto, registrazione.DataImmissione.Value.ToShortDateString)
            Me.Oggetto = String.Empty
        End If

       


        Me.DataDocumentoTextBox.SelectedDate = Now
        Me.GeneraDaModelloCheckBox.Enabled = utenteCollegato.SuperUser

        'Aggiungo i destinatari
        Dim idModulo As Integer = 10
        Dim rubrica As New ParsecAdmin.RubricaRepository
        For Each mitt In registrazione.Mittenti.Where(Function(c) c.Interno = False).ToList

            Dim id = mitt.Id
            Dim r = rubrica.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault

            Dim destinatario As New ParsecAdmin.DocumentoGenericoReferenteEsterno With
              {
                  .Id = r.Id,
                  .Codice = r.Codice,
                  .Descrizione = (IIf(r.Denominazione = Nothing, "", r.Denominazione) + " " + IIf(r.Nome = Nothing, "", r.Nome) + ", " + IIf(r.Indirizzo = Nothing, "", r.Indirizzo) + ", " + IIf(r.CAP = Nothing, "", r.CAP) + " " + IIf(r.Comune = Nothing, "", r.Comune) + " (" + IIf(r.Provincia = Nothing, "", r.Provincia) + ")" + IIf(r.Email = Nothing, "", " | " & r.Email)),
                  .IdModulo = idModulo,
                  .IdReferenteEsterno = r.Id,
                  .PerConoscenza = False
              }
            Me.Destinatari.Add(destinatario)
        Next

        rubrica.Dispose()

        'Aggiorno i destinatari
        Me.DestinatariGridView.DataSource = Me.Destinatari
        Me.DestinatariGridView.Rebind()
    End Sub

    Private Sub AggiornaVistaDaRegistrazione(ByVal id As Integer)
        Dim registrazioni As New ParsecPro.RegistrazioniRepository
        Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(id)
        registrazioni.Dispose()
        Me.AggiornaVistaDaModello(registrazione)
    End Sub

    Private Sub Save()
        Dim documenti As New ParsecAdmin.DocumentiGenericiRepository
        Dim documento As ParsecAdmin.DocumentoGenerico = New ParsecAdmin.DocumentoGenerico  '.CreateFromInstance(Me.DocumentoGenerico)

        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        documento.Oggetto = Me.OggettoTextBox.Text.Trim
        documento.IdUtente = utenteCollegato.Id
        documento.GeneratoSistemaSEP = True

        If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
            documento.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        End If

        'If Not String.IsNullOrEmpty(Me.IdModelloTextBox.Text) Then
        '    documento.IdModello = CInt(Me.IdModelloTextBox.Text)
        'End If

        If Not String.IsNullOrEmpty(Me.ModelliComboBox.SelectedValue) Then
            documento.IdModello = CInt(Me.ModelliComboBox.SelectedValue)
        End If

        documento.TipoGenerazione = ParsecAdmin.TipologiaGenerazione.FromTemplate  'INSERIMENTO è sempre da template

        Dim nuovoId As Integer = documenti.GetNuovoId

        Dim anno As String = String.Empty

        If Me.DocumentoGenerico Is Nothing Then
            documento.Codice = nuovoId

            If Not Me.DataDocumentoTextBox.SelectedDate Is Nothing Then
                documento.DataCreazione = Me.DataDocumentoTextBox.SelectedDate
                anno = Year(documento.DataCreazione).ToString
            End If

            documento.NomeFile = "doc" & "_" & anno & "_" & nuovoId.ToString & "_v" & documento.Versione.ToString & ".odt"

        Else

            documento.Codice = Me.DocumentoGenerico.Codice
            documento.DataCreazione = Me.DocumentoGenerico.DataCreazione


            If Not Me.DataDocumentoTextBox.SelectedDate Is Nothing Then
                documento.DataModifica = Me.DataDocumentoTextBox.SelectedDate
                anno = Year(documento.DataModifica).ToString
            End If

            documento.NomeFile = "doc" & "_" & anno & "_" & documento.Codice.ToString & "_v" & (Me.DocumentoGenerico.Versione + 1).ToString & ".odt"

            'IN MODIFICA
            If utenteCollegato.SuperUser Then
                If Not Me.GeneraDaModelloCheckBox.Checked Then
                    documento.TipoGenerazione = ParsecAdmin.TipologiaGenerazione.FromPreviousVersion
                End If
            End If

        End If

        documento.PercorsoRelativo = anno & "\"

        documento.Destinatari = Me.Destinatari
        documento.DescrizioneUfficio = Me.UfficioTextBox.Text

        documento.ObbligoDestinatari = Me.ObbligoDestinatariCheckBox.Checked

        Dim success As Boolean = True
        Try
            '*******************************************************************
            'Gestione storico non utilizzata.
            '*******************************************************************
            documenti.DocumentoGenerico = Me.DocumentoGenerico
            documenti.Save(documento)

            '*******************************************************************
            'Aggiorno l'oggetto corrente 
            '*******************************************************************
            Me.AggiornaVista(documenti.DocumentoGenerico)

            'Me.DocumentoGenerico = documenti.DocumentoGenerico
            documenti.Dispose()

        Catch ex As Exception
            success = False
            Throw New ApplicationException(ex.Message)
        Finally
            documenti.Dispose()
        End Try
    End Sub

    Private Sub RegistraScriptOpenOffice()
        If Not Me.DocumentoGenerico Is Nothing Then
            If Not String.IsNullOrEmpty(Me.DocumentoGenerico.DataSource) Then

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                'UTILIZZO L'APPLET
                If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                    ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(Me.DocumentoGenerico.DataSource, Me.salvaContenutoButton.ClientID, Me.documentContentHidden.ClientID, False, False)
                Else
                    'UTILIZZO IL SOCKET
                    ParsecUtility.Utility.EseguiServerComunicatorService(Me.DocumentoGenerico.DataSource, True, AddressOf Me.NotificaContenuto, AddressOf Me.SalvaContenuto)
                End If

                Me.DocumentoGenerico.DataSource = ""

            End If
        End If
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

    Private Sub ResettaVista()
        Me.DocumentoGenerico = Nothing
        Me.GeneraDaModelloCheckBox.Checked = True
        Me.ObbligoDestinatariCheckBox.Checked = True
        Me.ModelliComboBox.Text = String.Empty
        Me.GeneraDaModelloCheckBox.Enabled = False
        Me.OggettoTextBox.Text = String.Empty
        Me.DataDocumentoTextBox.SelectedDate = Now

        Me.IdUfficioTextBox.Text = String.Empty

        'Me.IdModelloTextBox.Text = String.Empty
        'Me.ModelloTextBox.Text = String.Empty

        Me.UfficioTextBox.Text = String.Empty

        Me.RubricaComboBox.Text = String.Empty
        Me.RubricaComboBox.SelectedValue = String.Empty
        Me.ResettaGridView()
    End Sub

    Private Sub ResettaGridView()
        Me.Destinatari = New List(Of ParsecAdmin.DocumentoGenericoReferenteEsterno)
        Me.DestinatariGridView.Rebind()
    End Sub

    Private Sub AggiornaVista(ByVal documento As ParsecAdmin.DocumentoGenerico)
        Me.ResettaVista()
        Dim documenti As New ParsecAdmin.DocumentiGenericiRepository
        Me.OggettoTextBox.Text = documento.Oggetto
        If documento.DataModifica.HasValue Then
            Me.DataDocumentoTextBox.SelectedDate = documento.DataModifica
        Else
            Me.DataDocumentoTextBox.SelectedDate = Now
        End If
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Me.GeneraDaModelloCheckBox.Enabled = utenteCollegato.SuperUser
        Me.GeneraDaModelloCheckBox.Checked = (documento.TipoGenerazione = 0)

        'Me.ModelloTextBox.Text = documento.NomeModello
        'Me.IdModelloTextBox.Text = documento.IdModello

        Me.ObbligoDestinatariCheckBox.Checked = documento.ObbligoDestinatari


        Me.ModelliComboBox.Text = documento.NomeModello
        Me.ModelliComboBox.SelectedValue = documento.IdModello

        Me.UfficioTextBox.Text = documento.DescrizioneUfficio
        Me.IdUfficioTextBox.Text = documento.IdUfficio
        Me.DocumentoGenerico = documento
        Me.Destinatari = documenti.GetDestinatari(documento.Id, 10)
        Me.DestinatariGridView.DataSource = Me.Destinatari
        Me.DestinatariGridView.Rebind()
        documenti.Dispose()
    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim documenti As New ParsecAdmin.DocumentiGenericiRepository
        Dim documento As ParsecAdmin.DocumentoGenerico = documenti.GetById(id)
        documenti.Dispose()
        Me.AggiornaVista(documento)
    End Sub

#End Region

#Region "SCRIPT PARSECOPENOFFICE"


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

        'ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)
        ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(data, "", "", False, False)
    End Sub


    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub



#End Region

#Region "GESTIONE MODELLO DOCUMENTO"

    'Protected Sub TrovaModelloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaModelloImageButton.Click
    '    Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaModelloDocumentoPage.aspx"
    '    Dim queryString As New Hashtable
    '    queryString.Add("obj", Me.AggiornaModelloImageButton.ClientID)
    '    ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
    'End Sub

    'Protected Sub AggiornaModelloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaModelloImageButton.Click
    '    If Not ParsecUtility.SessionManager.ModelloDocumento Is Nothing Then
    '        Dim modello As ParsecAdmin.ModelloDocumento = CType(ParsecUtility.SessionManager.ModelloDocumento, ParsecAdmin.ModelloDocumento)
    '        Me.ModelloTextBox.Text = modello.Nome
    '        Me.IdModelloTextBox.Text = modello.Id.ToString
    '        ParsecUtility.SessionManager.ModelloDocumento = Nothing
    '    End If
    'End Sub

    'Protected Sub EliminaModelloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaModelloImageButton.Click
    '    Me.ModelloTextBox.Text = String.Empty
    '    Me.IdModelloTextBox.Text = String.Empty
    'End Sub

#End Region

#Region "GESTIONE UFFICIO"

    Protected Sub TrovaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUfficioImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUfficioImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100,200")
        parametriPagina.Add("ultimoLivelloStruttura", "200")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Protected Sub AggiornaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUfficioImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Me.UfficioTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdUfficioTextBox.Text = struttureSelezionate.First.Id
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Protected Sub EliminaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUfficioImageButton.Click
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
    End Sub


#End Region

#Region "GESTIONE DESTINATARI"

    Protected Sub EliminaDestinatariSelezionatiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaDestinatariSelezionatiImageButton.Click
        For Each item As GridDataItem In Me.DestinatariGridView.SelectedItems
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim destinatario As ParsecAdmin.DocumentoGenericoReferenteEsterno = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
            Me.Destinatari.Remove(destinatario)
        Next
        Me.DestinatariGridView.Rebind()
    End Sub

    Private Sub SaveSelectedItems()
        For Each item As GridItem In Me.DestinatariGridView.Items
            If TypeOf item Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(item, GridDataItem)
                Dim id As Integer = CInt(dataItem("Id").Text)
                Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        If Not Me.SelectedItems.ContainsKey(id) Then
                            Me.SelectedItems.Add(id, True)
                        End If
                    Else
                        If Me.SelectedItems.ContainsKey(id) Then
                            Me.SelectedItems.Remove(id)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedItems()
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In DestinatariGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
    End Sub

    Private Sub DestinatariGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles DestinatariGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf DestinatariGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub DestinatariGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub DestinatariGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles DestinatariGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.DestinatariGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.DestinatariGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.DestinatariGridView.SelectedItems.Count = Me.DestinatariGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.DestinatariGridView.Items.Count > 0
    End Sub

    Protected Sub DestinatariGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DestinatariGridView.NeedDataSource
        Me.DestinatariGridView.DataSource = Me.Destinatari
    End Sub

    Protected Sub DestinatariGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DestinatariGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaDestinatario(e.Item)
        End If
    End Sub

    Private Sub EliminaDestinatario(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim destinatario As ParsecAdmin.DocumentoGenericoReferenteEsterno = Me.Destinatari.Where(Function(c) c.Id = id).FirstOrDefault
        Me.Destinatari.Remove(destinatario)
        Me.DestinatariGridView.Rebind()
    End Sub

    Protected Sub DestinatariGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DestinatariGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina destinatario"
                Dim message As String = "Eliminare l'elemento selezionato?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
            End If

            Dim destinatario As ParsecAdmin.DocumentoGenericoReferenteEsterno = CType(e.Item.DataItem, ParsecAdmin.DocumentoGenericoReferenteEsterno)
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim id As String = destinatario.Id

            If SelectedItems.ContainsKey(id) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(SelectedItems(id).ToString())
                dataItem.Selected = True
            End If
        End If
    End Sub

    Protected Sub TrovaDestinatarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaDestinatarioImageButton.Click

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaDestinatarioImageButton.ClientID)
        queryString.Add("mode", "search")
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("Filtro", Me.RubricaComboBox.Text)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina


        If Not String.IsNullOrEmpty(Me.RubricaComboBox.Text) Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.RubricaComboBox.Text) And c.LogStato Is Nothing).ToList
            If struttureEsterne.Count = 1 Then
                Dim referente As ParsecAdmin.StrutturaEsternaInfo = struttureEsterne.FirstOrDefault
                AggiornaReferenteEsterno(referente)
                Me.RubricaComboBox.Text = String.Empty
                Me.RubricaComboBox.SelectedValue = String.Empty
                ParsecUtility.SessionManager.ParametriPagina = Nothing
            Else
                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
            End If
            rubrica.Dispose()
        Else
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
        End If
    End Sub

    Private Sub AggiornaReferenteEsterno(ByVal referente As ParsecAdmin.StrutturaEsternaInfo)
        Dim idModulo As Integer = 10
        Dim destinatario As New ParsecAdmin.DocumentoGenericoReferenteEsterno With
              {
                  .Id = referente.Id,
                  .Codice = referente.Codice,
                  .Descrizione = (IIf(referente.Denominazione = Nothing, "", referente.Denominazione) + " " + IIf(referente.Nome = Nothing, "", referente.Nome) + ", " + IIf(referente.Indirizzo = Nothing, "", referente.Indirizzo) + ", " + IIf(referente.CAP = Nothing, "", referente.CAP) + " " + IIf(referente.Comune = Nothing, "", referente.Comune) + " (" + IIf(referente.Provincia = Nothing, "", referente.Provincia) + ")" + IIf(referente.Email = Nothing, "", " | " & referente.Email)),
                  .IdModulo = idModulo,
                  .IdReferenteEsterno = referente.Id,
                  .PerConoscenza = False
              }

        Dim exist As Boolean = False

        exist = Not Me.Destinatari.Where(Function(c) c.Id = destinatario.Id).FirstOrDefault Is Nothing
        If Not exist Then
            Me.Destinatari.Add(destinatario)
        End If
        Me.DestinatariGridView.Rebind()
    End Sub

    Protected Sub AggiungiDestinatarioImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDestinatarioImageButton.Click
        If Not String.IsNullOrEmpty(Me.RubricaComboBox.SelectedValue) Then
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim strutturaEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.RubricaComboBox.SelectedValue)).FirstOrDefault
            If Not strutturaEsterna Is Nothing Then
                Me.AggiornaReferenteEsterno(strutturaEsterna)
            End If
            Me.RubricaComboBox.Text = String.Empty
            Me.RubricaComboBox.SelectedValue = String.Empty
        End If
    End Sub

    Protected Sub AggiornaDestinatarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaDestinatarioImageButton.Click
        If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
            Dim r As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
            Dim idModulo As Integer = 10
            Dim destinatario As New ParsecAdmin.DocumentoGenericoReferenteEsterno With
                {
                    .Id = r.Id,
                    .Codice = r.Codice,
                    .Descrizione = (IIf(r.Denominazione = Nothing, "", r.Denominazione) + " " + IIf(r.Nome = Nothing, "", r.Nome) + ", " + IIf(r.Indirizzo = Nothing, "", r.Indirizzo) + ", " + IIf(r.CAP = Nothing, "", r.CAP) + " " + IIf(r.Comune = Nothing, "", r.Comune) + " (" + IIf(r.Provincia = Nothing, "", r.Provincia) + ")" + IIf(r.Email = Nothing, "", " | " & r.Email)),
                    .IdModulo = idModulo,
                    .IdReferenteEsterno = r.Id,
                    .PerConoscenza = False
                }

            Dim exist As Boolean = False
            exist = Not Me.Destinatari.Where(Function(c) c.Id = destinatario.Id).FirstOrDefault Is Nothing
            If Not exist Then
                Me.Destinatari.Add(destinatario)
            End If

            Me.RubricaComboBox.Text = String.Empty
            Me.RubricaComboBox.SelectedValue = String.Empty
            ParsecUtility.SessionManager.Rubrica = Nothing
            Me.DestinatariGridView.Rebind()
        End If
    End Sub

    Private Const ItemsPerRequest As Integer = 10

    <WebMethod()> _
    Public Shared Function GetModelliDocumento(ByVal context As RadComboBoxContext) As RadComboBoxData
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim filtro As New ParsecAdmin.ModelloDocumentoFiltro
        If Not utenteCollegato.SuperUser Then
            filtro.IdUtente = utenteCollegato.Id
        End If
        filtro.Validi = True
        Dim modelli As New ParsecAdmin.ModelliDocumentoRepository
        Dim data = modelli.GetView(filtro)
        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            'Dim item = data.Skip(i).FirstOrDefault
            Dim item = data.ElementAt(i)
            itemData.Text = item.Nome
            itemData.Value = item.Id
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function

    <WebMethod()> _
    Public Shared Function GetElementiRubrica(ByVal context As RadComboBoxContext) As RadComboBoxData
        Dim rubrica As New ParsecAdmin.RubricaRepository
        'Dim data = rubrica.GetQuery.Where(Function(c) c.Denominazione.StartsWith(context.Text) And c.Denominazione <> "").OrderBy(Function(c) c.Denominazione).Select(Function(c) New With {c.Denominazione, c.Nome, c.Indirizzo, c.Comune, c.Provincia, c.Id, c.CAP})
        Dim data = rubrica.GetQuery.Where(Function(c) c.Denominazione.StartsWith(context.Text) And c.Denominazione <> "" And c.LogStato Is Nothing).OrderBy(Function(c) c.Denominazione).ToList
        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData) '(endOffset - itemOffset)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            'Dim item = data.Skip(i).FirstOrDefault
            Dim item = data.ElementAt(i)
            itemData.Text = item.Denominazione & " " & If(Not String.IsNullOrEmpty(item.Nome), item.Nome & ", ", "") & If(Not String.IsNullOrEmpty(item.Indirizzo), item.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(item.Comune), item.Comune & " ", "") & If(Not String.IsNullOrEmpty(item.CAP), item.CAP & " ", "") & If(Not String.IsNullOrEmpty(item.Provincia), "(" & item.Provincia & ")", "")
            itemData.Value = item.Id
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function



#End Region

    Protected Sub AggiungiDocumentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoImageButton.Click

        If Me.DocumentoUpload.UploadedFiles.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Per inserire il documento, è necessario specificarne il file relativo!", False)
            Exit Sub
        End If

        Dim file As Telerik.Web.UI.UploadedFile = Me.DocumentoUpload.UploadedFiles(0)

        If Not String.IsNullOrEmpty(file.FileName) Then

            'Dim maxKiloBytesLength As Integer = CInt(ParsecAdmin.WebConfigSettings.GetKey("MaxKiloBytesLengthAlbo"))

            If file.ContentLength = 0 Then
                ParsecUtility.Utility.MessageBox("L'allegato è vuoto!", False)
                Exit Sub
            End If

            'If (file.ContentLength / 1024) > maxKiloBytesLength Then
            '    Dim mb As Single = (file.ContentLength / 1024) / 1024
            '    ParsecUtility.Utility.MessageBox("L'allegato è di " & mb.ToString("0.00") & " megabyte !" & vbCrLf & "Si può allegare un file di massimo 4 megabyte.", False)
            '    Exit Sub
            'End If

            Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

            If Not IO.Directory.Exists(pathRootTemp) Then
                IO.Directory.CreateDirectory(pathRootTemp)
            End If

            Dim pathDownload As String = pathRootTemp & Session.SessionID & "_" & file.FileName
            file.SaveAs(pathDownload)

            Me.NomeFileDocumentoLinkButton.Text = file.FileName
            Me.NomeFileDocumentoLabel.Text = pathDownload

            Me.AddDocumentoPanel.Visible = False
            Me.RemoveDocumentoPanel.Visible = True

            Me.SalvaButton.Enabled = True
            Me.RadToolBar.FindItemByText("Salva").Enabled = False

        End If

    End Sub

    Protected Sub RimuoviDocumentoImageButtonImageButton_Click(sender As Object, e As System.EventArgs) Handles RimuoviDocumentoImageButton.Click
        Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.NomeFileDocumentoLabel.Text
        If IO.File.Exists(pathDownload) Then
            IO.File.Delete(pathDownload)
        End If
        Me.NomeFileDocumentoLinkButton.Text = String.Empty
        Me.NomeFileDocumentoLabel.Text = String.Empty

        Me.AddDocumentoPanel.Visible = True
        Me.RemoveDocumentoPanel.Visible = False

        Me.SalvaButton.Enabled = False
        Me.RadToolBar.FindItemByText("Salva").Enabled = True

    End Sub

    Protected Sub NomeFileDocumentoLinkButton_Click(sender As Object, e As System.EventArgs) Handles NomeFileDocumentoLinkButton.Click
        Dim filenameTemp As String = Me.NomeFileDocumentoLabel.Text
        If Not String.IsNullOrEmpty(filenameTemp) Then
            Dim file As New IO.FileInfo(filenameTemp)
            If file.Exists Then
                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)
            Else
                ParsecUtility.Utility.MessageBox("Il file allegato non esiste!", False)
            End If
        End If
    End Sub


End Class