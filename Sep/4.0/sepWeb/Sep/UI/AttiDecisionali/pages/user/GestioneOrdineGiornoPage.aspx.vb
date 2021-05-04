Imports ParsecAtt
Imports System.Transactions
Imports Telerik.Web.UI
Imports System.Web.Mail
Imports Rebex.Net

Partial Class GestioneOrdineGiornoPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object


    Private Enum Direction
        Up = -1
        Down = 1
    End Enum

#Region "PROPRIETA'"

    Public Property Seduta() As ParsecAtt.Seduta
        Get
            Return CType(Session("GestioneOrdineGiornoPage_Seduta"), ParsecAtt.Seduta)
        End Get
        Set(ByVal value As ParsecAtt.Seduta)
            Session("GestioneOrdineGiornoPage_Seduta") = value
        End Set
    End Property

    Public Property Sedute() As List(Of ParsecAtt.Seduta)
        Get
            Return CType(Session("GestioneOrdineGiornoPage_Sedute"), List(Of ParsecAtt.Seduta))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Seduta))
            Session("GestioneOrdineGiornoPage_Sedute") = value
        End Set
    End Property

    'elemeto ordine del giorno (proposta o argomento) 

    Public Property ElementiOrdineGiorno() As List(Of ParsecAtt.OrdineGiorno)
        Get
            Return CType(Session("GestioneOrdineGiornoPage_OrdineGiorno"), List(Of ParsecAtt.OrdineGiorno))
        End Get
        Set(ByVal value As List(Of ParsecAtt.OrdineGiorno))
            Session("GestioneOrdineGiornoPage_OrdineGiorno") = value
        End Set
    End Property

    'proposte che non sono contenute nella collezione ElementiOrdineGiorno
    Public Property Proposte() As List(Of ParsecAtt.Documento)
        Get
            Return CType(Session("GestioneOrdineGiornoPage_Proposte"), List(Of ParsecAtt.Documento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Documento))
            Session("GestioneOrdineGiornoPage_Proposte") = value
        End Set
    End Property

    Public Property SelectedItems As List(Of Integer)
        Get
            If Session("GestioneOrdineGiornoPage_SelectedItems") Is Nothing Then
                Session("GestioneOrdineGiornoPage_SelectedItems") = New List(Of Integer)
            End If
            Return CType(Session("GestioneOrdineGiornoPage_SelectedItems"), List(Of Integer))
        End Get
        Set(ByVal value As List(Of Integer))
            Session("GestioneOrdineGiornoPage_SelectedItems") = value
        End Set
    End Property

#End Region

#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

    Private Sub RegistraScriptOpenOffice(ByVal notifica As Boolean)
        If Not Me.Seduta Is Nothing Then
            If Not String.IsNullOrEmpty(Me.Seduta.DataSource) Then


                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                'UTILIZZO L'APPLET
                If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                    ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(Me.Seduta.DataSource, If(notifica, Me.AggiornaPaginaButton.ClientID, Nothing), Nothing, False, False)
                    Me.Seduta.DataSource = String.Empty
                Else
                    'UTILIZZO IL SOCKET  
                    ParsecUtility.Utility.EseguiServerComunicatorService(Me.Seduta.DataSource, notifica, AddressOf Me.AggiornaPagina)
                End If
            End If
        End If
    End Sub



#End Region

#Region "EVENTI PAGINA"


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Gestione Ordine del Giorno"

        Me.CaricaAnni()

        If Not Me.Page.IsPostBack Then
            Me.Sedute = Nothing
            Me.ResettaVista()
            Me.PeriodoTextBox.SelectedDate = Now
            Me.CaricaTipologieSedute()

            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "DataConvocazione"
            sortExpr.SortOrder = GridSortOrder.Descending
            Me.SeduteGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If
        Me.ElementiOrdineGiornoGridView.Style.Add("width", widthStyle)
        Me.ProposteGridView.Style.Add("width", widthStyle)
        Me.PannelloDettaglio.Style.Add("width", widthStyle)
        Me.PannelloGriglia.Style.Add("width", widthStyle)

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la seduta selezionata?", False, Not Me.Seduta Is Nothing)
        Me.AggiornaGrigliaProposte()
        Me.AggiornaGrigliaOrdiniGiorno()
        If Not Me.Sedute Is Nothing Then
            Me.TitoloElencoSeduteLabel.Text = "Elenco Sedute&nbsp;&nbsp;&nbsp;" & If(Me.Sedute.Count > 0, "( " & Me.Sedute.Count.ToString & " )", "")
        End If
        Me.ElencoOrdiniGiornoLabel.Text = "Ordine del Giorno&nbsp;&nbsp;" & If(Me.ElementiOrdineGiornoGridView.MasterTableView.Items.Count > 0, "(" & Me.ElementiOrdineGiornoGridView.MasterTableView.Items.Count.ToString & ")", "")
        Me.ElencoProposteLabel.Text = "Proposte&nbsp;&nbsp;" & If(Me.ProposteGridView.MasterTableView.Items.Count > 0, "(" & Me.ProposteGridView.MasterTableView.Items.Count.ToString & ")", "")

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.PeriodoTextBox.MonthYearNavigationSettings.TodayButtonCaption = "Oggi"
        Me.PeriodoTextBox.MonthYearNavigationSettings.CancelButtonCaption = "Annulla"
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                '  Me.Print()

            Case "StampaAvvisoConvocazione"
                Me.StampaAvvisoConvocazione()

            Case "StampaOrdineGiorno"
                Me.StampaOrdineGiorno()

            Case "Export"
                Me.Esporta()

            Case "InviaEmail"
                Try
                    Me.InviaEmail()
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                End Try


            Case "Salva"
                Dim message As String = String.Empty
                Try
                    Me.Save()
                    Me.AggiornaGriglia()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                If String.IsNullOrEmpty(message) Then
                    Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                Else
                    ParsecUtility.Utility.MessageBox(message, False)
                End If

            Case "Nuovo"
                'Me.ResettaVista()
                'Me.AggiornaGriglia()

            Case "Annulla"

                Me.PeriodoTextBox.SelectedDate = Now
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Elimina"

                If Me.Seduta Is Nothing Then
                    ParsecUtility.Utility.MessageBox("E' necessario selezionare una seduta!", False)
                    Exit Sub
                End If

                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
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

                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"

            Case "Trova"

                Me.ResettaVista()
                Me.AggiornaGriglia()

        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Private Sub ProposteGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ProposteGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Private Sub ElementiOrdineGiornoGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ElementiOrdineGiornoGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If

        If TypeOf e.Item Is GridHeaderItem Then
            Dim headerItem As GridHeaderItem = DirectCast(e.Item, GridHeaderItem)
            Dim chk As CheckBox = DirectCast(headerItem("SelectCheckBox").Controls(0), CheckBox)
            Me.CheckBoxSelectAll.Value = chk.ClientID
        End If
    End Sub

    Protected Sub SeduteGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles SeduteGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
        End Select
    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroSeduta
        Dim filtro As New ParsecAtt.FiltroSeduta
        If Not Me.PeriodoTextBox.SelectedDate Is Nothing Then
            filtro.Anno = Me.PeriodoTextBox.SelectedDate.Value.Year
            filtro.Mese = Me.PeriodoTextBox.SelectedDate.Value.Month
        End If
        If Me.TipologieSedutaComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
        End If
        Return filtro
    End Function

    Protected Sub SeduteGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles SeduteGridView.NeedDataSource
        If Me.Sedute Is Nothing Then
            Dim sedute As New ParsecAtt.SedutaRepository
            Me.Sedute = sedute.GetView(Me.GetFiltro)
            sedute.Dispose()
        End If
        Me.SeduteGridView.DataSource = Me.Sedute
    End Sub

    Protected Sub SeduteGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles SeduteGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub ElementiOrdineGiornoGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles ElementiOrdineGiornoGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim ordine As ParsecAtt.OrdineGiorno = CType(dataItem.DataItem, ParsecAtt.OrdineGiorno)

            If ordine.CodiceDocumento.HasValue Then
                Dim codiceDocumento As Integer = ordine.CodiceDocumento.Value
                If Me.SelectedItems.Contains(codiceDocumento) Then
                    dataItem.Selected = True
                    Me.SelectedItems.Clear()
                End If
            End If


            Dim propostaNumerata As Boolean = False

            '***************************************************************************************************************************
            'VERIFICO SE L'ORDINE DEL GIORNO CORRENTE E' UNA PROPOSTA DI DELIBERA GIA' NUMERATA
            '***************************************************************************************************************************
            If ordine.CodiceDocumento.HasValue Then
                Dim documenti As New ParsecAtt.DocumentoRepository
                Dim proposta = documenti.Where(Function(c) c.Codice = ordine.CodiceDocumento And c.LogStato Is Nothing).FirstOrDefault
                If Not proposta Is Nothing Then
                    Dim id = proposta.Id
                    Dim delibera = documenti.Where(Function(c) c.IdPadre = id).FirstOrDefault
                    If Not delibera Is Nothing Then
                        propostaNumerata = True
                    End If
                End If
                documenti.Dispose()
            End If
            '***************************************************************************************************************************

            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                If propostaNumerata Then
                    btn.Attributes.Add("onclick", "return false")
                    btn.Style.Add("cursor", "default")
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = "Proposta Numerata"
                Else
                    btn.ToolTip = "Elimina punto all'ordine del giorno"
                    btn.Attributes.Add("onclick", "return confirm('Eliminare il punto all\'ordine del giorno selezionato?')")
                End If
            End If

            If TypeOf dataItem("Modifica").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Modifica").Controls(0), ImageButton)
                If propostaNumerata Then
                    btn.Attributes.Add("onclick", "return false")
                    btn.Style.Add("cursor", "default")
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = "Proposta Numerata"
                Else
                    btn.ToolTip = "Modifica punto all'ordine del giorno..."
                End If
            End If

            If propostaNumerata Then
                Dim chk = CType(dataItem("SelectCheckBox").Controls(0), CheckBox)
                chk.Enabled = False
                chk.ToolTip = "Proposta Numerata"
            End If

        End If
    End Sub

    Protected Sub ElementiOrdineGiornoGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ElementiOrdineGiornoGridView.ItemCommand
        Select Case e.CommandName
            Case "Modifica"
                Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
                Dim elementoOrdineGiorno As ParsecAtt.OrdineGiorno = Nothing

                If id > 0 Then
                    elementoOrdineGiorno = Me.ElementiOrdineGiorno.Where(Function(c) c.Id = id).FirstOrDefault
                Else
                    Dim codice As Nullable(Of Integer) = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("CodiceDocumento")
                    'Se l'elemento dell'ordine del giorno selezionato è una proposta.
                    If codice.HasValue AndAlso codice.Value > 0 Then
                        elementoOrdineGiorno = Me.ElementiOrdineGiorno.Where(Function(c) c.CodiceDocumento = codice).FirstOrDefault
                    Else
                        Dim guid As String = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Guid")
                        elementoOrdineGiorno = Me.ElementiOrdineGiorno.Where(Function(c) c.Guid = guid).FirstOrDefault
                    End If
                End If
                Me.VisualizzaDettaglioElementoOrdineGiorno(elementoOrdineGiorno)

            Case "Delete"
                Me.DeleteElementoOrdineGiorno(e.Item)

        End Select
    End Sub




#End Region

#Region "METODI PRIVATI"


    Private Sub CaricaTipologieSedute()
        Dim tipologie As New ParsecAtt.TipologiaSedutaRepository
        Me.TipologieSedutaComboBox.DataValueField = "Id"
        Me.TipologieSedutaComboBox.DataTextField = "Descrizione"
        Me.TipologieSedutaComboBox.DataSource = tipologie.GetKeyValue(Nothing)
        Me.TipologieSedutaComboBox.DataBind()
        Me.TipologieSedutaComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub Delete()
        Dim sedute As New ParsecAtt.SedutaRepository
        Try
            sedute.Delete(Me.Seduta)
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox("La seduta selezionata non può essere cancellata per il seguente motivo: " & vbCrLf & ex.Message, False)
        End Try
        sedute.Dispose()
    End Sub

    Private Sub ResettaVista()
        Me.ElementiOrdineGiorno = New List(Of ParsecAtt.OrdineGiorno)
        Me.Proposte = New List(Of ParsecAtt.Documento)
        Me.Seduta = Nothing
        Me.AnniComboBox.FindItemByValue(Now.Year.ToString).Selected = True
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
                Dim password As String = ParsecCommon.CryptoUtil.Decrypt(casellaPec.Password)
                client.Connect(casellaPec.SmtpServer, casellaPec.SmtpPorta.Value, mode)
                client.Login(casellaPec.UserId, password)
            End If
        Catch ex As Exception
            Throw New ApplicationException("Si è verificato il seguente errore: " & ex.Message)
        End Try
        Return client
    End Function

    Private Function CheckEmail(ByVal Indirizzo As String) As Boolean
        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
        Dim m = emailRegex.Match(Indirizzo)
        Return m.Success
    End Function

    Private Sub InviaEmail()

        If Me.Seduta Is Nothing Then
            Throw New ApplicationException("E' necessario selezionare una seduta!")
            Exit Sub
        End If

        Try

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("MittenteEmail", ParsecAdmin.TipoModulo.ATT)
            parametri.Dispose()

            If parametro Is Nothing Then
                Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "Il parametro 'MittenteEmail' non è presente.")
            End If

            Dim casellePec As New ParsecAdmin.ParametriPecRepository
            Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetQuery.Where(Function(c) c.Email = parametro.Valore).FirstOrDefault
            casellePec.Dispose()

            If casellaPec Is Nothing Then
                Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "La casella di posta '" & parametro.Valore & "' non è presente.")
            End If

            Dim client As Rebex.Net.Smtp = Me.ConfigureSmtp(casellaPec)

            Dim mail As New Rebex.Mail.MailMessage

            Dim message As New System.Text.StringBuilder


            Dim body As String = "<b>Avviso di convocazione - Seduta di " & Me.Seduta.DescrizioneTipologiaSeduta & " del " & Me.Seduta.DataConvocazione.ToShortDateString & " " & "alle ore " & Me.Seduta.OreSeduta & ".</b><br/>"
            Dim destinatari As New List(Of String)
            Dim presenze As New ParsecAtt.PresenzaSedutaRepository
            Dim convocati = presenze.GetView(New ParsecAtt.FiltroPresenzaSeduta With {.IdSeduta = Me.Seduta.Id})
            presenze.Dispose()

            Dim desc As String = "Il Consigliere"
            Dim desc2 As String = "al Consigliere"
            If Me.Seduta.TipologiaSeduta = TipologiaOrganoDeliberante.GiuntaComunale Then
                desc = "L'Assessore"
                desc2 = "all'Assessore"
            End If

            For Each convocato In convocati
                If Not String.IsNullOrEmpty(convocato.Email) Then
                    If Me.CheckEmail(convocato.Email) Then
                        destinatari.Add(convocato.Email)
                        message.AppendLine(String.Format("L'invio dell'email '{0} '{1}' è stato eseguito correttamente", desc2, convocato.Convocato))
                    Else
                        message.AppendLine(String.Format("{0} '{1}' non possiede un e-mail valida", desc, convocato.Convocato))
                    End If
                Else
                    message.AppendLine(String.Format("{0} '{1}' non possiede l'e-mail", desc, convocato.Convocato))
                End If
            Next

            'Elenco proposte ordine del giorno salvate.
            Dim proposte = Me.ElementiOrdineGiorno.Where(Function(c) c.Id > 0 And c.DataProposta.HasValue).OrderBy(Function(c) c.Ordinale).ToList()
            If proposte.Count > 0 Then
                body &= "All'<b>Ordine del giorno</b> : <br/>"
                For Each proposta In proposte
                    body &= "- Proposta N. <b>" & proposta.ContatoreGenerale.ToString & "</b> del <b>" & proposta.DataProposta.Value.ToShortDateString & "</b> con oggetto <b>" & proposta.Oggetto & "</b> " & "istruita dal settore <b>" & proposta.DescrizioneSettore & "</b>;<br/>"
                Next
            End If

            For Each dest In destinatari
                If Not mail.To.Contains(dest) Then
                    mail.To.Add(dest)
                End If
            Next

            mail.From = casellaPec.Email
            mail.Subject = "Avviso di convocazione"
            mail.Priority = Rebex.Mail.MailPriority.High
            mail.BodyHtml = body

            If mail.To.Count > 0 Then

                client.Timeout = 0
                client.Send(mail)
                client.Disconnect()

                ParsecUtility.Utility.MessageBox(message.ToString, False)
            Else
                ParsecUtility.Utility.MessageBox("Nessun convocato possiede l'e-mail!", False)
            End If

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try

    End Sub

    Private Sub CaricaAnni()
        Dim anni As New ParsecAtt.AnnoRepository
        Me.AnniComboBox.DataValueField = "Valore"
        Me.AnniComboBox.DataTextField = "Valore"
        Me.AnniComboBox.DataSource = anni.GetQuery.ToList
        Me.AnniComboBox.DataBind()
        anni.Dispose()
    End Sub

    Private Sub DeleteElementoOrdineGiorno(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim elementoOrdineGiorno As ParsecAtt.OrdineGiorno = Nothing

        'Elemento salvato.
        If id > 0 Then
            elementoOrdineGiorno = Me.ElementiOrdineGiorno.Where(Function(c) c.Id = id).FirstOrDefault
            Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
            ordiniGiorno.Delete(elementoOrdineGiorno)

            Me.AggiornaVista(Me.Seduta)

            '**************************************************************************************************************
            'CARICO GLI ORDINI DEL GIORNO DAL DB
            Me.ElementiOrdineGiorno = ordiniGiorno.GetView(New ParsecAtt.FiltroOrdineGiorno With {.IdSeduta = Me.Seduta.Id})
            ordiniGiorno.Dispose()
            '**************************************************************************************************************

        Else
            'Elemento temporaneo.
            Dim codice As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("CodiceDocumento")
            If codice > 0 Then
                elementoOrdineGiorno = Me.ElementiOrdineGiorno.Where(Function(c) c.CodiceDocumento = codice).FirstOrDefault
                Me.ElementiOrdineGiorno.Remove(elementoOrdineGiorno)
            Else
                Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
                elementoOrdineGiorno = Me.ElementiOrdineGiorno.Where(Function(c) c.Guid = guid).FirstOrDefault
                Me.ElementiOrdineGiorno.Remove(elementoOrdineGiorno)
            End If
        End If
        Me.AggiornaOrdinamento()
    End Sub

    Private Sub AggiornaOrdinamento()
        Dim count As Integer = 1
        For Each item In Me.ElementiOrdineGiorno
            item.Ordinale = count
            count += 1
        Next
    End Sub

    Private Sub AggiornaVista(ByVal seduta As ParsecAtt.Seduta)

        If seduta Is Nothing Then
            Me.ElementiOrdineGiorno = New List(Of ParsecAtt.OrdineGiorno)
            Me.Proposte = New List(Of ParsecAtt.Documento)
            Me.Seduta = Nothing
            Exit Sub
        End If


        'Dim idTipologiaSeduta As Integer = seduta.IdTipologiaSeduta

        'Dim anno As Integer = CInt(Me.AnniComboBox.SelectedValue)

        'Dim documenti As New ParsecAtt.DocumentoRepository
        'Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository(documenti.Context)

        ''Elenco delibere
        'Dim elencoIdPadre = documenti.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdTipologiaDocumento = CInt(ParsecAtt.TipoDocumento.Delibera) And Not c.IdPadre Is Nothing).Select(Function(c) c.IdPadre).ToList

        'Dim elencoCodici = ordiniGiorno.GetQuery.Where(Function(c) c.IdSeduta = seduta.Id).Select(Function(c) c.CodiceDocumento).ToList

        'Dim res = From documento In documenti.GetQuery
        '           Where documento.LogStato Is Nothing And _
        '           documento.IdTipologiaDocumento = CInt(ParsecAtt.TipoDocumento.PropostaDelibera) And _
        '           documento.IdTipologiaSeduta = idTipologiaSeduta And _
        '           Year(documento.DataProposta) = anno
        '           Order By documento.ContatoreGenerale
        '           Select New With {documento.Id, documento.Oggetto, documento.DataProposta, documento.ContatoreGenerale, documento.Codice, documento.IdStruttura, documento.IdUfficio}






        'Me.Proposte = res.AsEnumerable.Select(Function(c) New ParsecAtt.Documento With
        '                                               {
        '                                                   .Id = c.Id,
        '                                                   .Oggetto = c.Oggetto,
        '                                                   .DataProposta = c.DataProposta,
        '                                                   .ContatoreGenerale = c.ContatoreGenerale,
        '                                                   .Codice = c.Codice,
        '                                                   .IdStruttura = c.IdStruttura,
        '                                                   .IdUfficio = c.IdUfficio
        '                                                   }).ToList


        'If elencoCodici.Count > 0 Then
        '    Me.Proposte = Me.Proposte.Where(Function(c) Not elencoCodici.Contains(c.Codice)).ToList
        'End If

        'If elencoIdPadre.Count > 0 Then
        '    Me.Proposte = Me.Proposte.Where(Function(c) Not elencoIdPadre.Contains(c.Id)).ToList
        'End If

        'documenti.Dispose()
        'ordiniGiorno.Dispose()

    End Sub

    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim idSeduta As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim sedute As New ParsecAtt.SedutaRepository
        Me.Seduta = sedute.GetById(idSeduta)
        sedute.Dispose()
        Me.AggiornaVista(Me.Seduta)

        '**************************************************************************************************************
        'CARICO GLI ORDINI DEL GIORNO DAL DB
        Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
        Me.ElementiOrdineGiorno = ordiniGiorno.GetView(New ParsecAtt.FiltroOrdineGiorno With {.IdSeduta = Me.Seduta.Id})
        ordiniGiorno.Dispose()
        '**************************************************************************************************************
        Me.AggiornaOrdinamento()


    End Sub

    Private Sub AggiornaGrigliaProposte()

        If Not Me.Seduta Is Nothing Then

            Dim documenti As New ParsecAtt.DocumentoRepository

            Dim idTipologiaSeduta As Integer = Me.Seduta.IdTipologiaSeduta
            Dim anno As Integer = CInt(Me.AnniComboBox.SelectedValue)


            Dim proposte = documenti.Where(Function(c) c.LogStato Is Nothing And
                                               c.IdTipologiaDocumento = CInt(ParsecAtt.TipoDocumento.PropostaDelibera) And
                                               c.IdTipologiaSeduta = idTipologiaSeduta And
                                               Year(c.DataProposta) = anno).AsEnumerable.Select(Function(c) New ParsecAtt.Documento With {
                                               .Id = c.Id,
                                               .Oggetto = c.Oggetto,
                                               .DataProposta = c.DataProposta,
                                               .ContatoreGenerale = c.ContatoreGenerale,
                                               .Codice = c.Codice,
                                               .IdStruttura = c.IdStruttura,
                                               .IdUfficio = c.IdUfficio
                                                                                       }).ToList





            documenti.Dispose()

            'ESCLUDO TUTTE LE PROPOSTE DI DELIBERA MESSE NELL'ORDINE DEL GIORNO
            Dim codici = Me.ElementiOrdineGiorno.Select(Function(c) c.CodiceDocumento).ToList

            Dim ordini As New ParsecAtt.OrdineGiornoRepository
            Dim codiciSalvati = ordini.GetQuery.Select(Function(c) c.CodiceDocumento).ToList
            ordini.Dispose()


            'ESCLUDO TUTTE LE PROPOSTE DI DELIBERA ASSOCIATE AD UN ITER CONCLUSO

            Dim istanze As New ParsecWKF.IstanzaRepository()
            Dim atti As New ParsecWKF.AttoAmministrativoViewRepository(istanze.Context)

            Dim statoIstanzaCompletato As Integer = 3


            Dim contatoriProposteDelibereIstanzeCompletate = (From istanza In istanze.GetQuery
                                                              Join documento In atti.GetQuery
                           On istanza.IdDocumento Equals documento.Id
                                                              Where istanza.IdStato = statoIstanzaCompletato And istanza.IdModulo = ParsecAdmin.TipoModulo.ATT And documento.IdTipologiaDocumento = TipoDocumento.PropostaDelibera And Year(documento.DataProposta) = anno And documento.IdTipologiaSeduta = idTipologiaSeduta
                                                              Select istanza.ContatoreGenerale).ToList





            istanze.Dispose()

            Me.Proposte = proposte.Where(Function(c) Not contatoriProposteDelibereIstanzeCompletate.Contains(c.ContatoreGenerale) And Not codici.Contains(c.Codice) And Not codiciSalvati.Contains(c.Codice)).GroupBy(Function(c) c.Codice).Select(Function(c) c.FirstOrDefault).OrderBy(Function(c) c.ContatoreGenerale).ToList()


        End If

        Me.ProposteGridView.DataSource = Me.Proposte
        Me.ProposteGridView.DataBind()


    End Sub

    Private Sub AggiornaGrigliaOrdiniGiorno()
        Me.ElementiOrdineGiornoGridView.DataSource = Me.ElementiOrdineGiorno.OrderBy(Function(c) c.Ordinale).ToList()
        Me.ElementiOrdineGiornoGridView.DataBind()
    End Sub

    Private Sub AggiornaGriglia()
        Me.Sedute = Nothing
        Me.SeduteGridView.Rebind()
    End Sub


    Private Sub MoveRow(ByVal dir As Direction)
        If Me.ElementiOrdineGiornoGridView.SelectedIndexes.Count = 0 Then
            Exit Sub
        End If
        If Me.ElementiOrdineGiornoGridView.SelectedIndexes.Count > 1 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un solo elemento!", False)
            Exit Sub
        End If
        Dim id As Integer = Me.ElementiOrdineGiornoGridView.SelectedValues("Id")

        Dim codice As Nullable(Of Integer) = Me.ElementiOrdineGiornoGridView.SelectedValues("CodiceDocumento")
        Dim ordine As ParsecAtt.OrdineGiorno = Nothing

        If id > 0 Then
            ordine = Me.ElementiOrdineGiorno.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            'Codice negativo se è un argomento.
            ordine = Me.ElementiOrdineGiorno.Where(Function(c) c.CodiceDocumento = codice).FirstOrDefault
        End If

        Dim ordineSuccessivo = Me.ElementiOrdineGiorno.Where(Function(c) c.Ordinale = (ordine.Ordinale + CInt(dir))).FirstOrDefault
        If Not ordineSuccessivo Is Nothing Then
            Dim temp = ordineSuccessivo.Ordinale
            ordineSuccessivo.Ordinale = ordine.Ordinale
            ordine.Ordinale = temp
        End If
        Me.SelectedItems.Add(codice)
    End Sub

    Private Sub Save()
        If Me.Seduta Is Nothing Then
            Throw New ApplicationException("E' necessario selezionare una seduta!")
            Exit Sub
        End If

        Dim elementiOrdineGiorno As New ParsecAtt.OrdineGiornoRepository
        Try
            Dim documenti As New ParsecAtt.DocumentoRepository


            Me.Seduta.OrdineDelGiorno = Me.ElementiOrdineGiorno.OrderBy(Function(c) c.Ordinale).ToList()

            'Dim elementiProposteDaAnnullare = Me.Proposte.Where(Function(c) Me.Codici.Contains(c.Codice)).ToList

            'For Each item In elementiProposteDaAnnullare
            '    Dim codice As Integer = item.Codice
            '    Dim proposta As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Codice = codice And c.LogStato Is Nothing).FirstOrDefault
            '    If Not proposta Is Nothing Then
            '        If proposta.IdSeduta.HasValue Then
            '            'Elimino la seduta associata alla proposta di delibera che precedentemente era associata alla seduta correntemente selezionata.
            '            If proposta.IdSeduta = Me.Seduta.Id Then
            '                proposta.IdSeduta = Nothing
            '                documenti.SaveChanges()
            '            End If
            '        End If
            '    End If
            'Next

            Dim elementiProposteDaAnnullare = documenti.GetQuery.Where(Function(c) c.IdSeduta = Me.Seduta.Id And c.LogStato Is Nothing And c.DataProposta.HasValue).ToList
            For Each proposta In elementiProposteDaAnnullare
                proposta.IdSeduta = Nothing
                documenti.SaveChanges()
            Next



            elementiOrdineGiorno.DeleteAll(Me.Seduta)
            elementiOrdineGiorno.SaveAll(Me.Seduta)


            Dim elementiProposte = Me.ElementiOrdineGiorno.Where(Function(c) c.DataProposta.HasValue).OrderBy(Function(c) c.Ordinale).ToList()
            For Each item In elementiProposte
                Dim codice As Integer = item.CodiceDocumento
                Dim proposta As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Codice = codice And c.LogStato Is Nothing).FirstOrDefault
                If Not proposta Is Nothing Then
                    'Una proposta di delibera può essere presente in più sedute
                    'quindi verrà associata sempre quella corrente. 
                    proposta.IdSeduta = Me.Seduta.Id
                    documenti.SaveChanges()
                End If
            Next
            documenti.Dispose()

            '*******************************************************************
            'Aggiorno l'oggetto corrente.
            '*******************************************************************

            Me.AggiornaVista(Me.Seduta)

            '**************************************************************************************************************
            'CARICO GLI ORDINI DEL GIORNO DAL DB
            Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
            Me.ElementiOrdineGiorno = ordiniGiorno.GetView(New ParsecAtt.FiltroOrdineGiorno With {.IdSeduta = Me.Seduta.Id})
            ordiniGiorno.Dispose()
            '**************************************************************************************************************

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            elementiOrdineGiorno.Dispose()
        End Try
    End Sub


    Private Sub VisualizzaDettaglioElementoOrdineGiorno(ByVal elementoOrdineGiorno As ParsecAtt.OrdineGiorno)
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/OrdineGiornoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaElementoOrdineGiornoButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("ElementoOrdineGiorno", elementoOrdineGiorno)
        parametriPagina.Add("IdSeduta", Me.Seduta.Id)
        If elementoOrdineGiorno Is Nothing Then
            Dim ordinale = Me.ElementiOrdineGiorno.Max(Function(c) c.Ordinale + 1)
            If Not ordinale.HasValue Then
                ordinale = 1
            End If
            parametriPagina.Add("Ordinale", ordinale)
        Else
            parametriPagina.Add("Ordinale", elementoOrdineGiorno.Ordinale)
        End If

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowRadWindow(pageUrl, "OrdineGiornoRadWindow", queryString, False)
    End Sub

    Private Sub StampaAvvisoConvocazione()
        If Not Me.Seduta Is Nothing Then
            'Elenco elementi (proposte o argomenti) ordine del giorno salvati.

            Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
            Dim lista = ordiniGiorno.GetView(New ParsecAtt.FiltroOrdineGiorno With {.IdSeduta = Seduta.Id}).OrderBy(Function(c) c.Ordinale).ToList()
            ordiniGiorno.Dispose()


            If lista.Count = 0 Then
                ParsecUtility.Utility.MessageBox("L'ordine del giorno è vuoto!", False)
                Exit Sub
            End If

            Dim sedute As New ParsecAtt.SedutaRepository
            sedute.Seduta = Me.Seduta
            sedute.Seduta.OrdineDelGiorno = lista

            Dim presenze As New ParsecAtt.PresenzaSedutaRepository
            sedute.Seduta.Presenze = presenze.GetView(New ParsecAtt.FiltroPresenzaSeduta With {.IdSeduta = Me.Seduta.Id})
            presenze.Dispose()
            Try
                sedute.GeneraDataSource(ParsecAtt.TipoDocumento.AvvisoConvocazione)
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try

            sedute.Dispose()
            Me.RegistraScriptOpenOffice(False)
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una seduta!", False)
        End If
    End Sub

    Private Sub StampaOrdineGiorno()
        If Not Me.Seduta Is Nothing Then
            'Elenco elementi (proposte o argomenti) ordine del giorno salvati.
            Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
            Dim lista = ordiniGiorno.GetView(New ParsecAtt.FiltroOrdineGiorno With {.IdSeduta = Seduta.Id}).OrderBy(Function(c) c.Ordinale).ToList()
            ordiniGiorno.Dispose()


            If lista.Count = 0 Then
                ParsecUtility.Utility.MessageBox("L'ordine del giorno è vuoto!", False)
                Exit Sub
            End If
            Dim sedute As New ParsecAtt.SedutaRepository
            sedute.Seduta = Me.Seduta
            sedute.Seduta.OrdineDelGiorno = lista
            Try
                sedute.GeneraDataSource(ParsecAtt.TipoDocumento.OrdineGiorno)
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try

            sedute.Dispose()
            Me.RegistraScriptOpenOffice(False)
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una seduta!", False)
        End If
    End Sub

    Private Sub Esporta()
        If Not Me.Seduta Is Nothing Then
            'Elenco proposte ordine del giorno salvate.

            Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
            Dim lista = ordiniGiorno.GetView(New ParsecAtt.FiltroOrdineGiorno With {.IdSeduta = Seduta.Id}).OrderBy(Function(c) c.Ordinale).ToList()
            ordiniGiorno.Dispose()


            If lista.Count = 0 Then
                ParsecUtility.Utility.MessageBox("L'ordine del giorno è vuoto!", False)
                Exit Sub
            End If
            Dim sedute As New ParsecAtt.SedutaRepository
            sedute.Seduta = Me.Seduta

            sedute.Seduta.OrdineDelGiorno = lista
            sedute.GeneraDataSourceWebExport(True)
            sedute.Dispose()


            Me.RegistraScriptOpenOffice(True)
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una seduta!", False)
        End If
    End Sub


#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub AggiornaElementoOrdineGiornoButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaElementoOrdineGiornoButton.Click
        If Not ParsecUtility.SessionManager.ElementoOrdineGiorno Is Nothing Then
            Dim ordine As ParsecAtt.OrdineGiorno = ParsecUtility.SessionManager.ElementoOrdineGiorno
            If ordine.Id = 0 Then
                'Se è una proposta ????? Non può essere mai aggiunta.
                If ordine.CodiceDocumento.HasValue AndAlso ordine.CodiceDocumento.Value > 0 Then
                    If Me.ElementiOrdineGiorno.Where(Function(c) c.CodiceDocumento = ordine.CodiceDocumento).FirstOrDefault Is Nothing Then
                        Me.ElementiOrdineGiorno.Add(ordine)
                    End If
                Else
                    If Me.ElementiOrdineGiorno.Where(Function(c) c.Guid = ordine.Guid).FirstOrDefault Is Nothing Then
                        Dim codice As Nullable(Of Integer) = Me.ElementiOrdineGiorno.Min(Function(c) c.CodiceDocumento)
                        If codice.HasValue AndAlso codice < 0 Then
                            codice -= 1
                        Else
                            codice = -1
                        End If
                        ordine.CodiceDocumento = codice

                        Me.ElementiOrdineGiorno.Add(ordine)
                    End If
                End If
            End If

            ParsecUtility.SessionManager.ElementoOrdineGiorno = Nothing
            Me.AggiornaGrigliaOrdiniGiorno()
        End If
    End Sub

    Protected Sub AggiungiArgomentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiArgomentoImageButton.Click
        If Not Me.Seduta Is Nothing Then
            Me.VisualizzaDettaglioElementoOrdineGiorno(Nothing)
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una seduta!", False)
        End If
    End Sub


    Protected Sub AggiungiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiImageButton.Click
        If Me.ProposteGridView.SelectedIndexes.Count = 0 Then
            Exit Sub
        End If
        Dim item As GridDataItem = Nothing
        Dim ordine As ParsecAtt.OrdineGiorno = Nothing
        Dim proposta As ParsecAtt.Documento = Nothing
        Dim codice As Integer
        For i As Integer = 0 To Me.ProposteGridView.SelectedIndexes.Count - 1
            item = CType(Me.ProposteGridView.Items(Me.ProposteGridView.SelectedIndexes(i)), GridDataItem)
            codice = CInt(item("Codice").Text)
            proposta = Me.Proposte.Where(Function(c) c.Codice = codice).FirstOrDefault
            ordine = Me.GetOrdineDelGiorno(proposta)
            Me.ElementiOrdineGiorno.Add(ordine)
        Next
    End Sub

    Private Function GetOrdineDelGiorno(ByVal proposta As ParsecAtt.Documento) As ParsecAtt.OrdineGiorno
        Dim ordine As New ParsecAtt.OrdineGiorno With
                                                        {
                                                            .Oggetto = proposta.Oggetto,
                                                            .DataProposta = proposta.DataProposta,
                                                            .ContatoreGenerale = proposta.ContatoreGenerale,
                                                            .CodiceDocumento = proposta.Codice,
                                                            .IdStruttura = proposta.IdStruttura,
                                                            .IdUfficio = proposta.IdUfficio
                                                            }

        If Me.ElementiOrdineGiorno.Count > 0 Then
            ordine.Ordinale = 0
            ordine.Ordinale = ElementiOrdineGiorno.Max(Function(c) c.Ordinale + 1)
        End If
        If Not ordine.Ordinale.HasValue Then
            ordine.Ordinale = 1
        End If

        ordine.Id = -1

        'Dim id As Nullable(Of Integer) = -1
        If Me.ElementiOrdineGiorno.Count > 0 Then
            Dim id = Me.ElementiOrdineGiorno.Min(Function(c) c.Id) - 1
            If id < 0 Then
                ordine.Id = id
            End If
            'If id.HasValue AndAlso id < 0 Then
            '    id -= 1
            'End If
        End If

        'ordine.Id = id

        Return ordine
    End Function


    Protected Sub EliminaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaImageButton.Click
        If Me.ElementiOrdineGiornoGridView.SelectedIndexes.Count = 0 Then
            Exit Sub
        End If
        Dim item As GridDataItem = Nothing
        Dim id As Integer
        Dim ordine As ParsecAtt.OrdineGiorno = Nothing
        For i As Integer = 0 To Me.ElementiOrdineGiornoGridView.SelectedIndexes.Count - 1
            item = CType(Me.ElementiOrdineGiornoGridView.Items(Me.ElementiOrdineGiornoGridView.SelectedIndexes(i)), GridDataItem)

            Dim chk = CType(item("SelectCheckBox").Controls(0), CheckBox)
            If chk.Enabled Then
                id = CInt(item("Id").Text)
                ordine = Me.ElementiOrdineGiorno.Where(Function(c) c.Id = id).FirstOrDefault
                'If ordine.CodiceDocumento <> -1 Then
                Me.ElementiOrdineGiorno.Remove(ordine)
                'End If
            End If

        Next
        Me.AggiornaOrdinamento()
    End Sub


    Protected Sub AggiungiTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiTuttoImageButton.Click
        If Me.ProposteGridView.MasterTableView.Items.Count = 0 Then
            Exit Sub
        End If
        Dim ordine As ParsecAtt.OrdineGiorno = Nothing
        Dim proposta As ParsecAtt.Documento = Nothing
        Dim item As GridDataItem = Nothing
        Dim codice As Integer
        For i As Integer = 0 To Me.ProposteGridView.Items.Count - 1
            item = CType(Me.ProposteGridView.Items(i), GridDataItem)

            codice = CInt(item("Codice").Text)
            proposta = Me.Proposte.Where(Function(c) c.Codice = codice).FirstOrDefault
            ordine = Me.GetOrdineDelGiorno(proposta)
            Me.ElementiOrdineGiorno.Add(ordine)


        Next
        Me.AggiornaOrdinamento()
    End Sub


    Protected Sub EliminaTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaTuttoImageButton.Click
        If Me.ElementiOrdineGiornoGridView.MasterTableView.Items.Count = 0 Then
            Exit Sub
        End If

        Dim ordine As ParsecAtt.OrdineGiorno = Nothing

        Dim item As GridDataItem = Nothing
        Dim id As Integer

        For i As Integer = 0 To Me.ElementiOrdineGiornoGridView.Items.Count - 1
            item = CType(Me.ElementiOrdineGiornoGridView.Items(i), GridDataItem)
            Dim chk = CType(item("SelectCheckBox").Controls(0), CheckBox)
            If chk.Enabled Then
                id = CInt(item("Id").Text)
                ordine = Me.ElementiOrdineGiorno.Where(Function(c) c.Id = id).FirstOrDefault
                Me.ElementiOrdineGiorno.Remove(ordine)
            End If
        Next
        Me.AggiornaOrdinamento()
        'Me.ElementiOrdineGiorno.Clear()
    End Sub

    Protected Sub SpostaSuImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SpostaSuImageButton.Click
        Me.MoveRow(Direction.Up)
    End Sub

    Protected Sub SpostaGiuImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SpostaGiuImageButton.Click
        Me.MoveRow(Direction.Down)
    End Sub

    Protected Sub AnniComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles AnniComboBox.SelectedIndexChanged
        Me.AggiornaVista(Me.Seduta)
    End Sub

    Protected Sub AggiornaPaginaButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaPaginaButton.Click
        Me.AggiornaPagina()
    End Sub

    Private Sub AggiornaPagina()
        ParsecUtility.Utility.MessageBox(String.Format("L'esportazione è stata salvata con successo in 'C:\ODG_Export_{0}'", String.Format("{0:ddMMyyyy}", Me.Seduta.DataConvocazione)), False)
    End Sub

#End Region

End Class