#Region "IMPORTS"

Imports ParsecAdmin
Imports Telerik.Web.UI

#End Region


Partial Class SelezionaAttoPage
    Inherits System.Web.UI.Page

#Region "ENUMERAZIONI"

    Public Enum TipoPannello
        Filtro = 0
        Risultati = 1
    End Enum

    Public Enum ModalitaPagina
        Filtro = 0
        Ricerca = 1
    End Enum

#End Region

#Region "GESTIONI PANNELLI"

    Private Sub VisualizzaPannello(ByVal tipo As TipoPannello)
        Me.FiltroPanel.Visible = False
        Me.RisultatiPanel.Visible = False
        Select Case tipo
            Case TipoPannello.Filtro
                Me.FiltroPanel.Visible = True
            Case TipoPannello.Risultati
                Me.RisultatiPanel.Visible = True
        End Select

    End Sub

#End Region

#Region "PROPRIETA'"

    Public Property PageMode As ModalitaPagina
        Get
            Return Session("SelezionaAttoPage_PageMode")
        End Get
        Set(ByVal value As ModalitaPagina)
            Session("SelezionaAttoPage_PageMode") = value
        End Set
    End Property

    Public Property CurrentPosition As Integer
        Get
            Return Session("SelezionaAttoPage_CurrentPosition")
        End Get
        Set(ByVal value As Integer)
            Session("SelezionaAttoPage_CurrentPosition") = value
        End Set
    End Property

    Public Property Documento() As ParsecAtt.Documento
        Get
            Return CType(Session("SelezionaAttoPage_Documento"), ParsecAtt.Documento)
        End Get
        Set(ByVal value As ParsecAtt.Documento)
            Session("SelezionaAttoPage_Documento") = value
        End Set
    End Property

    Public Property Documenti() As List(Of ParsecAtt.Documento)
        Get
            Return CType(Session("SelezionaAttoPage_Documenti"), List(Of ParsecAtt.Documento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Documento))
            Session("SelezionaAttoPage_Documenti") = value
        End Set
    End Property

#End Region

#Region "CLASSI PRIVATE"

    Private Class Pubblicazione
        Public Property Id As Integer = 0
        Public Property Descrizione As String = String.Empty
    End Class

    Private Class Adottato
        Public Property Id As Integer = 0
        Public Property Descrizione As String = String.Empty
    End Class

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then

            Me.CaricaTipologieDocumento()

            If Not Me.Request.QueryString("Tipo") Is Nothing Then
                If Me.Request.QueryString("Abilita") Is Nothing Then
                    Me.TipologieDocumentoComboBox.Enabled = False
                End If
                Me.TipologieDocumentoComboBox.FindItemByValue(CInt(Me.Request.QueryString("Tipo"))).Selected = True
                Me.CaricaTipologieRegistro()
                Me.ImpostaAbilitazioneUI()
            End If

            Me.CaricaStatiDiscussione()
            Me.CaricaPublicazioniWeb()
            Me.CaricaPublicazioniAlbo()
            Me.CaricaAdottato()
            Me.CaricaTipologieSeduta()

            Me.DataDocumentoInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
            Me.DataDocumentoFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)

            Dim tipologiaDocumento As ParsecAtt.TipoDocumento = CType(Me.TipologieDocumentoComboBox.SelectedValue, ParsecAtt.TipoDocumento)
            Select Case tipologiaDocumento
                Case ParsecAtt.TipoDocumento.Delibera, ParsecAtt.TipoDocumento.PropostaDelibera
                    Me.DataSedutaInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
                    Me.DataSedutaFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
            End Select

            Select Case tipologiaDocumento
                Case ParsecAtt.TipoDocumento.Delibera, ParsecAtt.TipoDocumento.Ordinanza, ParsecAtt.TipoDocumento.Decreto, ParsecAtt.TipoDocumento.Determina
                    Me.DataPubblicazioneInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
                    Me.DataPubblicazioneFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
            End Select

        End If

        Me.VisualizzaPannello(TipoPannello.Filtro)


        If Request.QueryString("modalita").ToLower = "filtro" Then
            Me.PageMode = ModalitaPagina.Filtro
            Me.AvantiImageButton.Visible = False
        ElseIf Request.QueryString("modalita").ToLower = "ricerca" Then
            Me.PageMode = ModalitaPagina.Ricerca
            Me.CercaButton.Visible = False
        Else
            '
        End If

        '***************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '***************************************************************************
        ParsecUtility.Utility.CloseWindow(False)
        '***************************************************************************

        Me.ContatoreGeneraleInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.ContatoreGeneraleInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.ContatoreGeneraleFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

        Me.ContatoreSettoreInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.ContatoreSettoreInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.ContatoreSettoreFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

    End Sub

    Protected Sub TipologieDocumentoComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieDocumentoComboBox.SelectedIndexChanged
        Me.CaricaTipologieRegistro()
        Me.ImpostaAbilitazioneUI()
        Me.ModelliComboBox.Items.Clear()
    End Sub

    Protected Sub TipologieRegistroComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieRegistroComboBox.SelectedIndexChanged
        Me.CaricaModelli()
    End Sub

    Protected Sub TipologieSedutaComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieSedutaComboBox.SelectedIndexChanged
        Me.CaricaModelli()
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub DocumentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DocumentiGridView.NeedDataSource
        If Me.Documenti Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

            Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.IdUtente = utenteCollegato.Id, .ApplicaAbilitazione = True})

            documenti.Dispose()
        End If
        Me.DocumentiGridView.DataSource = Me.Documenti
    End Sub

    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                SelezionaRegistrazione(e.Item)
            Case "Preview"
                Me.VisualizzaAtto(e.Item)
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

    Protected Sub DocumentiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Private Sub VisualizzaAtto(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idDocumento As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = idDocumento).FirstOrDefault
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

        End If
        documenti.Dispose()
    End Sub

#End Region

#Region "AZIONI PANNELLO FILTRO"

    Private Sub ResettaFiltro()
        Me.DataDocumentoInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataDocumentoFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)

        Dim tipologiaDocumento As ParsecAtt.TipoDocumento = CType(Me.TipologieDocumentoComboBox.SelectedValue, ParsecAtt.TipoDocumento)
        Select Case tipologiaDocumento
            Case ParsecAtt.TipoDocumento.Delibera, ParsecAtt.TipoDocumento.PropostaDelibera
                Me.DataSedutaInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
                Me.DataSedutaFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        End Select

        Select Case tipologiaDocumento
            Case ParsecAtt.TipoDocumento.Delibera, ParsecAtt.TipoDocumento.Ordinanza, ParsecAtt.TipoDocumento.Decreto, ParsecAtt.TipoDocumento.Determina
                Me.DataPubblicazioneInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
                Me.DataPubblicazioneFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        End Select

        Me.OggettoTextBox.Text = String.Empty
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
        Me.ContatoreGeneraleInizioTextBox.Text = String.Empty
        Me.ContatoreGeneraleFineTextBox.Text = String.Empty
        Me.ContatoreSettoreInizioTextBox.Text = String.Empty
        Me.ContatoreSettoreFineTextBox.Text = String.Empty

        If Not Me.Request.QueryString("Tipo") Is Nothing Then
            If Me.Request.QueryString("Abilita") Is Nothing Then
                Me.TipologieDocumentoComboBox.Enabled = False
                Me.TipologieDocumentoComboBox.FindItemByValue(CInt(Me.Request.QueryString("Tipo"))).Selected = True
            End If
        Else
            Me.TipologieDocumentoComboBox.SelectedIndex = 0
        End If


        Me.TipologieRegistroComboBox.SelectedIndex = 0
        Me.ModelliComboBox.SelectedIndex = 0
        Me.StatiApprovazioneComboBox.SelectedIndex = 0
        Me.AdottateComboBox.SelectedIndex = 0
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.Documenti = Nothing
        Me.DocumentiGridView.Rebind()
    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean
        If Not (String.IsNullOrEmpty(Me.DataDocumentoInizioTextBox.SelectedDate.ToString) And String.IsNullOrEmpty(Me.DataDocumentoFineTextBox.SelectedDate.ToString)) Then
            If Me.DataDocumentoInizioTextBox.SelectedDate > Me.DataDocumentoFineTextBox.SelectedDate Then
                message.AppendLine("Se specificato, il valore del filtro iniziale sulla data del documento deve essere minore o uguale del valore finale.")
            End If
        End If
        If Not (String.IsNullOrEmpty(Me.DataSedutaInizioTextBox.SelectedDate.ToString) And String.IsNullOrEmpty(Me.DataSedutaFineTextBox.SelectedDate.ToString)) Then
            If Me.DataSedutaInizioTextBox.SelectedDate > Me.DataSedutaFineTextBox.SelectedDate Then
                message.AppendLine("Se specificato, il valore del filtro iniziale sulla data della seduta deve essere minore o uguale del valore finale.")
            End If
        End If
        If Not (String.IsNullOrEmpty(Me.DataPubblicazioneInizioTextBox.SelectedDate.ToString) And String.IsNullOrEmpty(Me.DataPubblicazioneFineTextBox.SelectedDate.ToString)) Then
            If Me.DataPubblicazioneInizioTextBox.SelectedDate > Me.DataPubblicazioneFineTextBox.SelectedDate Then
                message.AppendLine("Se specificato, il valore del filtro iniziale sulla data di pubblicazione deve essere minore o uguale del valore finale.")
            End If
        End If

        Return Not message.Length > 0
    End Function

    Private Function GetFiltroDocumenti() As ParsecAtt.FiltroDocumento
        Dim filtro As New ParsecAtt.FiltroDocumento

        filtro.DataDocumentoInizio = Me.DataDocumentoInizioTextBox.SelectedDate
        filtro.DataDocumentoFine = Me.DataDocumentoFineTextBox.SelectedDate
        filtro.DataSedutaInizio = Me.DataSedutaInizioTextBox.SelectedDate
        filtro.DataSedutaFine = Me.DataSedutaFineTextBox.SelectedDate
        filtro.DataPubblicazioneInizio = Me.DataPubblicazioneInizioTextBox.SelectedDate
        filtro.DataPubblicazioneFine = Me.DataPubblicazioneFineTextBox.SelectedDate

        filtro.Adottato = False

        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            filtro.Oggetto = Me.OggettoTextBox.Text
        End If

        If Me.ContatoreGeneraleInizioTextBox.Value.HasValue Then
            filtro.ContatoreGeneraleInizio = CInt(Me.ContatoreGeneraleInizioTextBox.Value)
        End If

        If Me.ContatoreGeneraleFineTextBox.Value.HasValue Then
            filtro.ContatoreGeneraleFine = CInt(Me.ContatoreGeneraleFineTextBox.Value)
        End If

        If Me.ContatoreSettoreInizioTextBox.Value.HasValue Then
            filtro.ContatoreSettoreInizio = CInt(Me.ContatoreSettoreInizioTextBox.Value)
        End If

        If Me.ContatoreSettoreFineTextBox.Value.HasValue Then
            filtro.ContatoreSettoreFine = CInt(Me.ContatoreSettoreFineTextBox.Value)
        End If

        'TIPOLOGIA SEDUTA
        If Me.TipologieSedutaComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
        End If

        'FILTRO PER UFFICIO E SETTORE UTILIZZANDO IL CAMPO DESCRIZIONE PER POTER TROVARE ANCHE GLI ATTI CON UFFICI E SETTORI MODIFICATI
        'If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
        '    filtro.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        'Else
        filtro.Ufficio = Me.UfficioTextBox.Text.Trim
        'End If


        'If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
        '    filtro.IdSettore = CInt(Me.IdSettoreTextBox.Text)
        'Else
        filtro.Settore = Me.SettoreTextBox.Text.Trim
        'End If


        If Me.TipologieDocumentoComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaDocumento = CInt(Me.TipologieDocumentoComboBox.SelectedValue)
        End If

        If Me.TipologieRegistroComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaRegistro = CInt(Me.TipologieRegistroComboBox.SelectedValue)
        End If

        If Me.ModelliComboBox.SelectedIndex > 0 Then
            filtro.IdModello = CInt(Me.ModelliComboBox.SelectedValue)
        End If

        If Me.StatiApprovazioneComboBox.SelectedIndex > 0 Then
            filtro.IdStatoDiscussione = CInt(Me.StatiApprovazioneComboBox.SelectedValue)
        End If

        If Me.AdottateComboBox.SelectedIndex > 0 Then
            filtro.Adottato = CBool(Me.AdottateComboBox.SelectedValue)
        End If

        If Me.PubblicazioneAlboComboBox.SelectedIndex > 0 Then
            filtro.PubblicazioneAlbo = CBool(Me.PubblicazioneAlboComboBox.SelectedValue)
        End If

        If Me.PubblicazioneWebComboBox.SelectedIndex > 0 Then
            filtro.PubblicazioneWeb = CBool(Me.PubblicazioneWebComboBox.SelectedValue)
        End If

        filtro.ApplicaAbilitazione = True

        Return filtro

    End Function

    Private Sub FiltraDocumenti()

        Me.Documenti = New List(Of ParsecAtt.Documento)

        Dim documentiR As New ParsecAtt.DocumentoRepository

        Dim res = documentiR.GetView(Me.GetFiltroDocumenti).GetEnumerator

        While res.MoveNext
            Dim m = res.Current
            Dim doc As New ParsecAtt.Documento With {
                .Id = m.Id,
                .ContatoreGenerale = m.ContatoreGenerale,
                .DescrizioneTipologia = m.DescrizioneTipologia,
                .DataDocumento = m.DataDocumento,
                .Oggetto = m.Oggetto,
                .DescrizioneUfficio = m.DescrizioneUfficio,
                .DescrizioneSettore = m.DescrizioneSettore
            }

            Me.Documenti.Add(doc)

        End While

        'registrazioni.Dispose()
    End Sub

    Protected Sub CercaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CercaButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            ParsecUtility.SessionManager.FiltroDocumento = Me.GetFiltroDocumenti
            ParsecUtility.Utility.ClosePopup(False)

        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    Protected Sub AvantiImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AvantiImageButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Me.FiltraDocumenti()
            Me.DocumentiGridView.Rebind()
            Me.VisualizzaPannello(TipoPannello.Risultati)
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

#End Region

#Region "AZIONI PANNELLO RISULTATI"

    Private Sub SelezionaRegistrazione(ByVal item As Telerik.Web.UI.GridDataItem)
        Session("SelezionaAtto_IdDocumentoSelezionato") = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    End Sub

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Me.Conferma()
    End Sub

    Private Sub Conferma()
        If Not Session("SelezionaAtto_IdDocumentoSelezionato") Is Nothing Then
            ' If Me.PageMode = ModalitaPagina.Filtro Then
            'ParsecUtility.Utility.ClosePopup(False)
            ParsecUtility.Utility.DoWindowClose(False)
            'End If
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una registrazione!", False)
        End If
    End Sub


    Protected Sub IndietroImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles IndietroImageButton.Click
        Me.VisualizzaPannello(TipoPannello.Filtro)
        Session("SelezionaAtto_IdDocumentoSelezionato") = Nothing
    End Sub

#End Region

#Region "METODI PRIVATI"

    'Private Sub CaricaTipologieDocumento()
    '    Dim filtroTipoDoc As New ParsecAtt.FiltroTipologiaDocumento
    '    If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
    '        Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
    '        If parametriPagina.ContainsKey("filtroTipoDoc") Then
    '            filtroTipoDoc = parametriPagina("filtroTipoDoc")
    '        End If
    '    Else
    '        filtroTipoDoc.Modellizzabile = True
    '    End If
    '    Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
    '    Me.TipologieDocumentoComboBox.DataValueField = "Id"
    '    Me.TipologieDocumentoComboBox.DataTextField = "Descrizione"
    '    Me.TipologieDocumentoComboBox.DataSource = tipologie.GetKeyValue(filtroTipoDoc)
    '    Me.TipologieDocumentoComboBox.DataBind()
    '    Me.TipologieDocumentoComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
    '    Me.TipologieDocumentoComboBox.SelectedIndex = 0
    '    tipologie.Dispose()
    'End Sub

    Private Sub CaricaTipologieDocumento()
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Me.TipologieDocumentoComboBox.DataValueField = "Id"
        Me.TipologieDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologieDocumentoComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaDocumento With {.Modellizzabile = True})
        Me.TipologieDocumentoComboBox.DataBind()
        Me.TipologieDocumentoComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieDocumentoComboBox.SelectedIndex = 0
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

    Private Sub CaricaStatiDiscussione()
        Dim stati As New ParsecAtt.StatoDiscussioneRepository
        Me.StatiApprovazioneComboBox.DataValueField = "Id"
        Me.StatiApprovazioneComboBox.DataTextField = "Descrizione"
        Me.StatiApprovazioneComboBox.DataSource = stati.GetKeyValue(Nothing)
        Me.StatiApprovazioneComboBox.DataBind()
        Me.StatiApprovazioneComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.StatiApprovazioneComboBox.SelectedIndex = 0
        stati.Dispose()
    End Sub

    Private Sub CaricaAdottato()
        Dim lista As New List(Of Adottato)
        lista.Add(New Adottato With {.Id = 1, .Descrizione = "Adottato"})
        lista.Add(New Adottato With {.Id = 0, .Descrizione = "Non adottato"})
        Me.AdottateComboBox.DataValueField = "Id"
        Me.AdottateComboBox.DataTextField = "Descrizione"
        Me.AdottateComboBox.DataSource = lista
        Me.AdottateComboBox.DataBind()
        Me.AdottateComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.AdottateComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaPublicazioniWeb()
        Dim lista As New List(Of Pubblicazione)
        lista.Add(New Pubblicazione With {.Id = 1, .Descrizione = "Pubblicabili"})
        lista.Add(New Pubblicazione With {.Id = 0, .Descrizione = "Non pubblicabili"})
        Me.PubblicazioneWebComboBox.DataValueField = "Id"
        Me.PubblicazioneWebComboBox.DataTextField = "Descrizione"
        Me.PubblicazioneWebComboBox.DataSource = lista
        Me.PubblicazioneWebComboBox.DataBind()
        Me.PubblicazioneWebComboBox.Items.Insert(0, New RadComboBoxItem("", "-1"))
        Me.PubblicazioneWebComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaPublicazioniAlbo()
        Dim lista As New List(Of Pubblicazione)
        lista.Add(New Pubblicazione With {.Id = 1, .Descrizione = "Pubblicate"})
        lista.Add(New Pubblicazione With {.Id = 0, .Descrizione = "Non pubblicate"})
        Me.PubblicazioneAlboComboBox.DataValueField = "Id"
        Me.PubblicazioneAlboComboBox.DataTextField = "Descrizione"
        Me.PubblicazioneAlboComboBox.DataSource = lista
        Me.PubblicazioneAlboComboBox.DataBind()
        Me.PubblicazioneAlboComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaModelli(idTipologiaDocumento As Nullable(Of Integer), idTipologiaSeduta As Nullable(Of Integer))
        Dim modelli As New ParsecAtt.ModelliRepository
        Me.ModelliComboBox.DataValueField = "Id"
        Me.ModelliComboBox.DataTextField = "Descrizione"
        Me.ModelliComboBox.DataSource = modelli.GetKeyValue(New ParsecAtt.FiltroModello With {.TipologiaDocumento = idTipologiaDocumento, .Disabilitato = False, .IdTipologiaSeduta = idTipologiaSeduta})
        Me.ModelliComboBox.DataBind()
        Me.ModelliComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.ModelliComboBox.SelectedIndex = 0
        modelli.Dispose()
    End Sub

    Private Sub CaricaModelli()
        Dim idTipologiaDocumento As Nullable(Of Integer) = Nothing
        Dim idTipologiaSeduta As Nullable(Of Integer) = Nothing

        If Me.TipologieDocumentoComboBox.SelectedIndex > 0 Then
            idTipologiaDocumento = CInt(Me.TipologieDocumentoComboBox.SelectedValue)
        End If
        If Me.TipologieSedutaComboBox.SelectedIndex > 0 Then
            idTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
        End If
        If idTipologiaSeduta.HasValue OrElse idTipologiaDocumento.HasValue Then
            Me.CaricaModelli(idTipologiaDocumento, idTipologiaSeduta)
        End If
    End Sub

    Private Sub AbilitaTutto()
        Me.ContatoreSettorePanel.Enabled = True
        Me.AdottatePanel.Enabled = True
        Me.ApprovazionePanel.Enabled = True
        Me.TipologiaSedutaPanel.Enabled = True
        Me.DataSedutaPanel.Enabled = True
        Me.PubblicazioneAlboPanel.Enabled = True
        Me.DataPubblicazionePanel.Enabled = True
        Me.PubblicazioneWePanel.Enabled = True
        Me.ModelloPanel.Enabled = True
    End Sub

    Private Sub ImpostaAbilitazioneUI()
        Me.AbilitaTutto()
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
            Case ParsecAtt.TipoDocumento.PropostaDelibera
                Me.ImpostaAbilitazionePropostaDelibera()
            Case ParsecAtt.TipoDocumento.PropostaDetermina
                Me.ImpostaAbilitazionePropostaDetermina()
            Case ParsecAtt.TipoDocumento.PropostaOrdinanza
                Me.ImpostaAbilitazionePropostaOrdinanza()
            Case ParsecAtt.TipoDocumento.PropostaDecreto
                Me.ImpostaAbilitazionePropostaOrdinanza()
        End Select
    End Sub

    Private Sub ImpostaAbilitazioneDelibera()
        Me.ContatoreSettorePanel.Enabled = False
        Me.ContatoreSettoreInizioTextBox.Text = String.Empty
        Me.ContatoreSettoreFineTextBox.Text = String.Empty

        Me.DataSedutaInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataSedutaFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        Me.DataPubblicazioneInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataPubblicazioneFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)

        Me.AdottatePanel.Enabled = False
        Me.AdottateComboBox.SelectedIndex = 0

        Me.ApprovazionePanel.Enabled = False
        Me.StatiApprovazioneComboBox.SelectedIndex = 0



    End Sub

    Private Sub ImpostaAbilitazioneOrdinanza()

        Me.ContatoreSettorePanel.Enabled = False
        Me.ContatoreSettoreInizioTextBox.Text = String.Empty
        Me.ContatoreSettoreFineTextBox.Text = String.Empty

        Me.TipologiaSedutaPanel.Enabled = False
        Me.TipologieSedutaComboBox.SelectedIndex = 0

        Me.DataSedutaPanel.Enabled = False
        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing

        Me.ApprovazionePanel.Enabled = False
        Me.StatiApprovazioneComboBox.SelectedIndex = 0

        Me.AdottatePanel.Enabled = False
        Me.AdottateComboBox.SelectedIndex = 0


        Me.PubblicazioneWePanel.Enabled = False
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataPubblicazioneFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
    End Sub

    Private Sub ImpostaAbilitazioneDetermina()
        Me.TipologiaSedutaPanel.Enabled = False
        Me.TipologieSedutaComboBox.SelectedIndex = 0

        Me.DataSedutaPanel.Enabled = False
        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing


        Me.ApprovazionePanel.Enabled = False
        Me.StatiApprovazioneComboBox.SelectedIndex = 0

        Me.AdottatePanel.Enabled = False
        Me.AdottateComboBox.SelectedIndex = 0

        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataPubblicazioneFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
    End Sub

    Private Sub ImpostaAbilitazionePropostaDelibera()

        Me.ContatoreSettorePanel.Enabled = False
        Me.ContatoreSettoreInizioTextBox.Text = String.Empty
        Me.ContatoreSettoreFineTextBox.Text = String.Empty



        Me.DataSedutaPanel.Enabled = False
        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing


        Me.PubblicazioneAlboPanel.Enabled = False
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0

        Me.DataPubblicazionePanel.Enabled = False
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

        Me.PubblicazioneWePanel.Enabled = False
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.ModelloPanel.Enabled = False
        Me.ModelliComboBox.SelectedIndex = 0

        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

    End Sub

    Private Sub ImpostaAbilitazionePropostaOrdinanza()

        Me.ContatoreSettorePanel.Enabled = False
        Me.ContatoreSettoreInizioTextBox.Text = String.Empty
        Me.ContatoreSettoreFineTextBox.Text = String.Empty

        Me.TipologiaSedutaPanel.Enabled = False
        Me.TipologieSedutaComboBox.SelectedIndex = 0

        Me.DataSedutaPanel.Enabled = False
        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing

        Me.ApprovazionePanel.Enabled = False
        Me.StatiApprovazioneComboBox.SelectedIndex = 0


        Me.PubblicazioneAlboPanel.Enabled = False
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0

        Me.DataPubblicazionePanel.Enabled = False
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing


        Me.PubblicazioneWePanel.Enabled = False
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.ModelloPanel.Enabled = False
        Me.ModelliComboBox.SelectedIndex = 0

        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

    End Sub

    Private Sub ImpostaAbilitazionePropostaDetermina()

        Me.TipologiaSedutaPanel.Enabled = False
        Me.TipologieSedutaComboBox.SelectedIndex = 0

        Me.DataSedutaPanel.Enabled = False
        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing


        Me.ApprovazionePanel.Enabled = False
        Me.StatiApprovazioneComboBox.SelectedIndex = 0


        Me.PubblicazioneAlboPanel.Enabled = False
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0

        Me.DataPubblicazionePanel.Enabled = False
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

        Me.PubblicazioneWePanel.Enabled = False
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.ModelloPanel.Enabled = False
        Me.ModelliComboBox.SelectedIndex = 0

        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

    End Sub

#End Region
End Class