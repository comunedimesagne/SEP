Imports ParsecAdmin
Imports ParsecUtility
Imports System.Transactions
Imports Telerik.Web.UI


Partial Class UtentiPage
    Inherits System.Web.UI.Page

#Region "CLASSE IRadListBoxItemComparer"

    Private Class IRadListBoxItemComparer
        Implements IEqualityComparer(Of RadListBoxItem)

        Public Function IEqualityComparer_Equals(ByVal x As RadListBoxItem, ByVal y As RadListBoxItem) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of RadListBoxItem).Equals
            Return x.Value = y.Value
        End Function

        Public Function IEqualityComparer_GetHashCode(ByVal obj As RadListBoxItem) As Integer Implements System.Collections.Generic.IEqualityComparer(Of RadListBoxItem).GetHashCode
            Return obj.GetHashCode
        End Function

    End Class

#End Region

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Utente() As Utente
        Get
            Return CType(Session("UtentiPage_Utente"), Utente)
        End Get
        Set(ByVal value As Utente)
            Session("UtentiPage_Utente") = value
        End Set
    End Property

    Public Property Utenti() As Object
        Get
            Return CType(Session("UtentiPage_Utenti"), Object)
        End Get
        Set(ByVal value As Object)
            Session("UtentiPage_Utenti") = value
        End Set
    End Property

    Public Property StruttureAbilitate() As List(Of StrutturaAbilitata)
        Get
            Return CType(Session("UtentiPage_StruttureAbilitate"), List(Of StrutturaAbilitata))
        End Get
        Set(ByVal value As List(Of StrutturaAbilitata))
            Session("UtentiPage_StruttureAbilitate") = value
        End Set
    End Property

    Public Property NumeroAbilitazioniFunzioneUtente() As Integer
        Get
            Return CType(Session("UtentiPage_NumeroAbilitazioniFunzioneUtente"), Integer)
        End Get
        Set(ByVal value As Integer)
            Session("UtentiPage_NumeroAbilitazioniFunzioneUtente") = value
        End Set
    End Property

    Public Property NumeroAbilitazioniModuliUtente() As Integer
        Get
            Return CType(Session("UtentiPage_NumeroAbilitazioniModuliUtente"), Integer)
        End Get
        Set(ByVal value As Integer)
            Session("UtentiPage_NumeroAbilitazioniModuliUtente") = value
        End Set
    End Property

    Public Property Note() As String
        Get
            Return CType(Session("UtentiPage_Note"), String)
        End Get
        Set(ByVal value As String)
            Session("UtentiPage_Note") = value
        End Set
    End Property

    Public Property Gruppi() As List(Of ParsecAdmin.Gruppo)
        Get
            Return CType(Session("UtentiPage_Gruppi"), List(Of ParsecAdmin.Gruppo))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Gruppo))
            Session("UtentiPage_Gruppi") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Utenti"

        Me.CaricaProviderFirmaD()
        Me.CaricaVersioneJava()
        Me.CaricaModuli()
        Me.CaricaTecnologiaClientSide()

        Me.AbilitazioniModuloGridView.DataSource = (New ParsecAdmin.ModuleRepository).GetQuery.Where(Function(c) c.Abilitato = True).OrderBy(Function(c) c.Descrizione).ToList
        Me.PasswordNonSettataCheckBox.Text = ""
        Me.AbilitazioniFunzioniGridView.DataSource = (New ParsecAdmin.FunzioniRepository).GetView()

        If Not IsPostBack AndAlso Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
            Me.PasswordNonSettataCheckBox.Checked = True
            Me.UltimoSettaggioTextBox.Enabled = False
            Me.StruttureAbilitate = New List(Of ParsecAdmin.StrutturaAbilitata)
            Me.PasswordPrimoAccessoTextBox.Text = ParsecUtility.Utility.GeneraPassword
            CognomeTextBox.Focus()
            Me.Gruppi = New List(Of ParsecAdmin.Gruppo)
            Me.Utente = Nothing

            Me.SessoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("-", "0"))
            Me.SessoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("M", "1"))
            Me.SessoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("F", "2"))
            Me.SessoComboBox.SelectedIndex = 0

            'Dim parametri As New ParsecAdmin.ParametriRepository
            'Dim parametro = parametri.GetByName("AbilitaLoginTramiteCredenzialiWindows", ParsecAdmin.TipoModulo.SEP)
            'parametri.Dispose()

            'Dim visualizzaSelezioneUtenteWindows As Boolean = False
            'If Not parametro Is Nothing Then
            '    visualizzaSelezioneUtenteWindows = (parametro.Valore = "1")
            'End If

            'Me.TrovaUtenteWindowsImageButton.Visible = visualizzaSelezioneUtenteWindows
            'Me.EliminaUtenteWindwsImageButton.Visible = visualizzaSelezioneUtenteWindows

        End If

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadListBox .rlbCheck,.RadListBox .rlbImage,.RadListBox .rlbText{vertical-align: middle;padding-left:5px;}"
        Me.Page.Header.Controls.Add(css)

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.DatiUtenteMultiPage.Style.Add("width", widthStyle)
        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.GruppiVisibilitaPanel.Style.Add("width", widthStyle)
        Me.GruppiGridView.Style.Add("width", widthStyle)
        Me.StruttureAbilitateGridView.Style.Add("width", widthStyle)
        Me.StruttureAbilitatePanel.Style.Add("width", widthStyle)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'utente selezionato?", False, Not Me.Utente Is Nothing)
      
        Me.ProfiliLabel.Text = "Profili&nbsp;&nbsp;&nbsp;" & If(Me.ProfiliListBox.Items.Count > 0, "( " & Me.ProfiliListBox.Items.Count.ToString & " )", "")
        Me.StruttureAbilitateLabel.Text = "Strutture abilitate&nbsp;&nbsp;&nbsp;" & If(Me.StruttureAbilitate.Count > 0, "( " & Me.StruttureAbilitate.Count.ToString & " )", "")
        Me.PasswordPrimoAccessoTextBox.Enabled = Me.Utente Is Nothing


        Me.TitoloElencoGruppiLabel.Text = "Elenco Gruppi&nbsp;&nbsp;&nbsp;" & If(Me.Gruppi.Count > 0, "( " & Me.Gruppi.Count.ToString & " )", "")

        Dim message As String = String.Empty
        If Me.GruppiGridView.SelectedItems.Count > 0 Then
            message = "Eliminare tutti gli elementi selezionati?"
            Me.EliminaGruppiSelezionatiImageButton.Attributes.Add("onclick", "alert(""" & message & """); return true;")
        Else
            message = "E' necessario selezionare almeno un gruppo!"
            Me.EliminaGruppiSelezionatiImageButton.Attributes.Add("onclick", "alert(""" & message & """); return false;")
        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanelAbilitazioniFunzioni, Me.scrollPosHiddenAbilitazioniFunzioni, False)
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanelStruttureAbilitate, Me.scrollPosHiddenStruttureAbilitate, False)
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim message As String = String.Empty
                Try
                    Me.Save()
                    Me.Utenti = Nothing
                    Me.UtentiGridView.Rebind()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try

                If String.IsNullOrEmpty(message) Then
                    Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                Else
                    ParsecUtility.Utility.MessageBox(message, False)
                End If

            Case "Nuovo"
                Me.ResettaVista()
                ' Abilito il check di SuperUser solo se l'utente collegato è Super User
                Dim utenteCollegato As Utente = CType(Applicazione.UtenteCorrente, Utente)
                Me.SuperUserCheckBox.Enabled = utenteCollegato.SuperUser
                Me.PasswordNonSettataCheckBox.Checked = True
                Me.PasswordPrimoAccessoTextBox.Text = Utility.GeneraPassword
                Me.Utenti = Nothing
                Me.UtentiGridView.Rebind()
                Me.StruttureAbilitateGridView.Rebind()
                Me.GruppiGridView.Rebind()
            Case "Annulla"
                Me.ResettaVista()
                Me.SuperUserCheckBox.Enabled = True
                Me.PasswordNonSettataCheckBox.Checked = True
                Me.PasswordPrimoAccessoTextBox.Text = Utility.GeneraPassword
                Me.Utenti = Nothing
                Me.UtentiGridView.Rebind()
                Me.StruttureAbilitateGridView.Rebind()
                Me.GruppiGridView.Rebind()
            Case "Elimina"

                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Utente Is Nothing Then
                        Dim utenteCollegato As Utente = CType(Applicazione.UtenteCorrente, Utente)
                        If Me.Utente.Id = utenteCollegato.Id Then
                            Utility.MessageBox("Non è consentito eliminare sè stessi!", False)
                            Exit Sub
                        End If
                        If Not utenteCollegato.SuperUser Then
                            If Me.Utente.SuperUser Then
                                ParsecUtility.Utility.MessageBox("Non è consentito eliminare l'utente selezionato, perchè è un SUPER USER!", False)
                                Exit Sub
                            End If
                        End If
                        Me.Delete()
                        Me.Utenti = Nothing
                        Me.ResettaVista()
                        Me.UtentiGridView.Rebind()
                        Me.StruttureAbilitateGridView.Rebind()
                        Me.GruppiGridView.Rebind()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un utente!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Utente = Nothing
                Dim utenti As New ParsecAdmin.UserRepository
                Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, Utente)
                Dim utenteDaTrovare As New ParsecAdmin.Utente With {.Nome = Me.NomeTextBox.Text, .Cognome = Me.CognomeTextBox.Text, .Username = Me.UsernameTextBox.Text, .SuperUser = utenteCorrente.SuperUser}
                Dim filtroUtenti = utenti.GetUsers(utenteDaTrovare)
                Me.Utenti = filtroUtenti
                Me.UtentiGridView.Rebind()
                utenti.Dispose()
        End Select
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource
        If Me.Utenti Is Nothing Then
            Dim utenti As New ParsecAdmin.UserRepository
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, Utente)
            Dim utenteDaTrovare As New ParsecAdmin.Utente With {.SuperUser = utenteCorrente.SuperUser}
            Me.Utenti = utenti.GetUsers(utenteDaTrovare)
            utenti.Dispose()
        End If
        Me.UtentiGridView.DataSource = Me.Utenti
    End Sub

    Protected Sub UtentiGridView_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles UtentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                AggiornaVista(e)
            Case "Copy"
                Me.AggiornaVista(e)
                Me.Utente = Nothing
        End Select
    End Sub

    Protected Sub UtentiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub DeleteSelectedItems(ByVal list As Telerik.Web.UI.RadListBox)
        For Each item As Telerik.Web.UI.RadListBoxItem In list.CheckedItems
            list.Items.Remove(item)
        Next
    End Sub

    Private Sub ResettaVista()
        Me.Utente = Nothing
        Me.CognomeTextBox.Text = String.Empty
        Me.NomeTextBox.Text = String.Empty
        Me.CodiceFiscaleTextBox.Text = String.Empty
        Me.TitoloTextBox.Text = String.Empty
        Me.DataNascitaTextBox.SelectedDate = Nothing
        Me.EmailTextBox.Text = String.Empty
        Me.UsernameTextBox.Text = String.Empty

        Me.ModuloComboBox.SelectedIndex = 0
        Me.CellulareTextBox.Text = String.Empty
        Me.NumeroSerialeCertificatoTextBox.Text = String.Empty
        Me.UltimoSettaggioTextBox.Text = String.Empty
        Me.BloccatoCheckBox.Checked = False
        Me.SuperUserCheckBox.Checked = False
        Me.NumeroAbilitazioniFunzioneUtente = 0
        Me.NumeroAbilitazioniModuliUtente = 0
        Me.ProfiliListBox.Items.Clear()
        Me.StruttureAbilitate = New List(Of ParsecAdmin.StrutturaAbilitata)
        Me.AbilitazioniFunzioniGridView.MasterTableView.ClearSelectedItems()
        Me.AbilitazioniFunzioniGridView.Rebind()
        Me.AbilitazioniModuloGridView.Rebind()
        Me.ProviderFirmaComboBox.SelectedIndex = 0
        Me.VersioneJavaComboBox.SelectedIndex = 0
        Me.TecnologiaClientSideComboBox.SelectedIndex = 0
        Me.StrutturaDefaultTextBox.Text = String.Empty
        Me.CodiceStrutturaDefaultTextBox.Text = String.Empty
        Me.Note = String.Empty
        Me.NoteTextBox.Text = String.Empty
        Me.NomeFileCertificatoLabel.Text = String.Empty

        Me.UtenteWindowsTextBox.Text = String.Empty
        Me.Gruppi = New List(Of ParsecAdmin.Gruppo)

        Me.SessoComboBox.SelectedIndex = 0
    End Sub

    Private Sub AggiornaVista(ByVal e As GridCommandEventArgs)

        Me.ResettaVista()

        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Dim userRepository As New ParsecAdmin.UserRepository
        Me.Utente = userRepository.GetUserById(id).FirstOrDefault

        Dim gruppi As New ParsecAdmin.GruppoRepository
        Me.Gruppi = gruppi.GetGruppi(id)
        gruppi.Dispose()





        If e.CommandName = "Select" Then

            Me.UtenteWindowsTextBox.Text = Me.Utente.GuidUtenteWindows

            If Not String.IsNullOrEmpty(Me.Utente.Sesso) Then
                Me.SessoComboBox.FindItemByText(Me.Utente.Sesso).Selected = True
            Else
                Me.SessoComboBox.SelectedIndex = 0
            End If

            Me.CognomeTextBox.Text = Me.Utente.Cognome
            Me.NomeTextBox.Text = Me.Utente.Nome
            Me.CodiceFiscaleTextBox.Text = Me.Utente.CodiceFiscale
            Me.TitoloTextBox.Text = Me.Utente.Titolo

            If Not Me.Utente.DataNascita Is Nothing Then
                Me.DataNascitaTextBox.SelectedDate = CDate(String.Format("{0:dd/MM/yyyy}", Me.Utente.DataNascita))
            End If

            Me.EmailTextBox.Text = Me.Utente.Email
            Me.UsernameTextBox.Text = Me.Utente.Username
            Me.PasswordPrimoAccessoTextBox.Text = ""

            Me.CellulareTextBox.Text = Me.Utente.Cellulare
            Me.NumeroSerialeCertificatoTextBox.Text = Me.Utente.IdCertificato
            Me.UltimoSettaggioTextBox.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", Me.Utente.DataUltimoSettaggioPsw)
            Me.PasswordNonSettataCheckBox.Checked = Me.Utente.PswNonSettata

            If Me.Utente.IdProviderFirma.HasValue Then
                Me.ProviderFirmaComboBox.Items.FindItemByValue(Me.Utente.IdProviderFirma).Selected = True
            Else
                Me.ProviderFirmaComboBox.SelectedIndex = 0
            End If

            If Not String.IsNullOrEmpty(Me.Utente.VersioneJava) Then
                Me.VersioneJavaComboBox.Items.FindItemByText(Me.Utente.VersioneJava.Trim).Selected = True
            Else
                Me.VersioneJavaComboBox.SelectedIndex = 0
            End If

            If Me.Utente.TecnologiaClientSide.HasValue Then
                Me.TecnologiaClientSideComboBox.Items.FindItemByValue(Me.Utente.TecnologiaClientSide).Selected = True
            Else
                Me.TecnologiaClientSideComboBox.SelectedIndex = 0
            End If


            Dim struttureOrganigramma As New ParsecAdmin.StructureRepository


            Dim strutturaOrganigramma = struttureOrganigramma.GetQuery.Where(Function(c) c.Codice = Me.Utente.CodiceStutturaDefault).FirstOrDefault
            If Not strutturaOrganigramma Is Nothing Then
                Me.StrutturaDefaultTextBox.Text = strutturaOrganigramma.Descrizione
                Me.CodiceStrutturaDefaultTextBox.Text = strutturaOrganigramma.Codice
            End If





            CaricaStorico()
        Else
            Me.PasswordNonSettataCheckBox.Checked = True
            Me.PasswordPrimoAccessoTextBox.Text = Utility.GeneraPassword
            Me.PasswordNonSettataCheckBox.Checked = True
        End If

        Me.BloccatoCheckBox.Checked = Me.Utente.Bloccato
        Me.SuperUserCheckBox.Checked = Me.Utente.SuperUser
        Dim utenteCollegato As Utente = CType(Applicazione.UtenteCorrente, Utente)
        Me.SuperUserCheckBox.Enabled = utenteCollegato.SuperUser



        Try
            Me.ModuloComboBox.FindItemByValue(Me.Utente.IdModuloDefault).Selected = True
        Catch ex As Exception

        End Try


        Me.ProfiliListBox.DataValueField = "Id"
        Me.ProfiliListBox.DataTextField = "Descrizione"
        Me.ProfiliListBox.DataSource = userRepository.GetProfili(id)
        Me.ProfiliListBox.DataBind()




        Dim struttureAbilitate As List(Of StrutturaAbilitata) = userRepository.GetStruttureAbilitate(id)
        Me.StruttureAbilitate = struttureAbilitate
        Me.StruttureAbilitateGridView.Rebind()
        userRepository.Dispose()



        For Each it As GridDataItem In Me.AbilitazioniModuloGridView.Items
            Dim idModulo As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
            Dim abilitaTuttoCheckBox As CheckBox = CType(it.FindControl("abilitaTuttoCheckBox"), CheckBox)
            Dim responsabileCheckBox As CheckBox = CType(it.FindControl("responsabileCheckBox"), CheckBox)
            Dim abilitazioneUtenteTutteStrutture As ParsecAdmin.AbilitazioneUtenteTutteStrutture = (New ParsecAdmin.AbilitazioneUtenteTutteStruttureRepository).GetQuery.Where(Function(c) c.IdModulo = idModulo And c.IdUtente = id And c.LogTipoOperazione Is Nothing).FirstOrDefault
            Dim abilitazioneResponsabileModulo As ParsecAdmin.AbilitazioneResponsabileModulo = (New ParsecAdmin.AbilitazioneResponsabileModuloRepository).GetQuery.Where(Function(c) c.IdModulo = idModulo And c.IdUtente = id And c.LogTipoOperazione Is Nothing).FirstOrDefault
            If Not abilitazioneUtenteTutteStrutture Is Nothing Then
                abilitaTuttoCheckBox.Checked = True
            End If

            If Not abilitazioneResponsabileModulo Is Nothing Then
                responsabileCheckBox.Checked = True
            End If

            If abilitaTuttoCheckBox.Checked And responsabileCheckBox.Checked Then Me.NumeroAbilitazioniModuliUtente += 1
        Next

        For Each it As GridDataItem In Me.AbilitazioniFunzioniGridView.Items
            Dim idFunzione As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
            Dim abilitataCheckBox As CheckBox = CType(it.FindControl("abilitataCheckBox"), CheckBox)
            Dim funzioneUtente As FunzioneUtente = (New FunzioniUtenteRepository).GetQuery.Where(Function(c) c.IdFunzione = idFunzione And c.IdUtente = id).FirstOrDefault
            abilitataCheckBox.Checked = Not funzioneUtente Is Nothing
            If abilitataCheckBox.Checked Then Me.NumeroAbilitazioniFunzioneUtente += 1
            If Not utenteCollegato.SuperUser Then
                If idFunzione = 45 Then
                    Dim funzioni As New FunzioniUtenteRepository
                    Dim abilitatoVisibilita As Boolean = Not funzioni.GetQuery.Where(Function(c) c.IdFunzione = 45 And c.IdUtente = utenteCollegato.Id).FirstOrDefault Is Nothing
                    funzioni.Dispose()
                    abilitataCheckBox.Enabled = abilitatoVisibilita
                    If Not abilitatoVisibilita Then abilitataCheckBox.ToolTip = "Check non modificabile!" Else abilitataCheckBox.ToolTip = String.Empty
                End If
            End If
        Next

        Me.NoteTextBox.Text = Me.Utente.Note
        Me.Note = Me.NoteTextBox.Text

        Me.NomeFileCertificatoLabel.Visible = utenteCollegato.SuperUser
        If utenteCollegato.SuperUser Then
            If Not String.IsNullOrEmpty(Me.Utente.NomefileCertificato) Then
                Me.NomeFileCertificatoLabel.Text = WebConfigSettings.GetKey("PathCertificati") & Me.Utente.NomefileCertificato
            End If
       End If

        Me.GruppiGridView.Rebind()
    End Sub

    Private Sub Print()
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaUtenti")
        parametriStampa.Add("DatiStampa", Me.Utenti)
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub


    Private Sub Save()

        Dim nuovo As Boolean = Me.Utente Is Nothing
        Dim username As String = String.Empty
        Dim password As Byte() = Nothing
        If Not nuovo Then
            username = Me.Utente.Username
            password = Me.Utente.PasswordHash
        End If

        Dim utenti As New ParsecAdmin.UserRepository
        Dim utente As ParsecAdmin.Utente = utenti.CreateFromInstance(Me.Utente)

        utente.Username = Me.UsernameTextBox.Text
        utente.Cognome = Me.CognomeTextBox.Text
        utente.Nome = Me.NomeTextBox.Text
        utente.Titolo = Me.TitoloTextBox.Text
        utente.Email = Me.EmailTextBox.Text
        utente.DataNascita = Me.DataNascitaTextBox.SelectedDate
        utente.CodiceFiscale = Me.CodiceFiscaleTextBox.Text

        utente.GuidUtenteWindows = Nothing
        If Not String.IsNullOrEmpty(Me.UtenteWindowsTextBox.Text) Then
            utente.GuidUtenteWindows = Me.UtenteWindowsTextBox.Text
        End If


        'utente.LivelloRetributivo = Me.LivelloRetributivoTextBox.Text


        utente.IdModuloDefault = CInt(Me.ModuloComboBox.SelectedItem.Value)
        utente.SuperUser = Me.SuperUserCheckBox.Checked
        utente.Bloccato = Me.BloccatoCheckBox.Checked
        utente.Note = Me.NoteTextBox.Text

        If Not String.IsNullOrEmpty(Me.CodiceStrutturaDefaultTextBox.Text) Then
            utente.CodiceStutturaDefault = CInt(Me.CodiceStrutturaDefaultTextBox.Text)
        Else
            utente.CodiceStutturaDefault = Nothing
        End If

        If Me.SessoComboBox.SelectedIndex <> 0 Then
            utente.Sesso = Me.SessoComboBox.SelectedItem.Text
        Else
            utente.Sesso = Nothing
        End If


        'la check è sempre disabilitata
        utente.PswNonSettata = If(Me.Utente Is Nothing, True, False)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, Utente)
        utente.LogIdUtente = utenteCollegato.Id
        utente.LogUsername = utenteCollegato.Username
        utente.LogDataOperazione = Now
        utente.IdCertificato = Me.NumeroSerialeCertificatoTextBox.Text
        utente.IdProviderFirma = CInt(Me.ProviderFirmaComboBox.SelectedValue)

        If Me.VersioneJavaComboBox.SelectedIndex > 0 Then
            utente.VersioneJava = Me.VersioneJavaComboBox.SelectedItem.Text.Trim
        End If

        utente.TecnologiaClientSide = Nothing
        If Me.TecnologiaClientSideComboBox.SelectedIndex > 0 Then
            utente.TecnologiaClientSide = CInt(Me.TecnologiaClientSideComboBox.SelectedItem.Value)
        End If

        utente.Cellulare = Me.CellulareTextBox.Text

        If nuovo Then
            If Not String.IsNullOrEmpty(Me.PasswordPrimoAccessoTextBox.Text) Then
                utente.PasswordHash = Utility.CalcolaHash(Me.PasswordPrimoAccessoTextBox.Text)
                utente.Password = Me.PasswordPrimoAccessoTextBox.Text
            End If
        End If

        '***********************************************************************
        'Gestione profili.
        '***********************************************************************
        Dim profileRepository As New ParsecAdmin.ProfileRepository
        Dim profiloUtente As ParsecAdmin.UtenteProfilo = Nothing
        Dim profili As New List(Of ParsecAdmin.UtenteProfilo)
        For i As Integer = 0 To Me.ProfiliListBox.Items.Count - 1
            Dim idProfilo As Integer = CInt(Me.ProfiliListBox.Items(i).Value)
            profiloUtente = New ParsecAdmin.UtenteProfilo
            profiloUtente.IdProfilo = idProfilo
            profiloUtente.Disabilitato = profileRepository.GetQuery.Where(Function(c) c.Id = idProfilo).Select(Function(c) c.Disabilitato).FirstOrDefault
            profili.Add(profiloUtente)
        Next
        profileRepository.Dispose()
        '***********************************************************************

        '***********************************************************************
        'Gestione abilitazioni strutture.
        '***********************************************************************
        Dim abilitazioni As New List(Of ParsecAdmin.AbilitazioneUtenteStruttura)
        Dim abilitazioneUtenteStruttura As ParsecAdmin.AbilitazioneUtenteStruttura = Nothing
        Dim struttureAbilitate As List(Of ParsecAdmin.StrutturaAbilitata) = Me.StruttureAbilitate
        For Each struttura As ParsecAdmin.StrutturaAbilitata In struttureAbilitate
            abilitazioneUtenteStruttura = New ParsecAdmin.AbilitazioneUtenteStruttura
            abilitazioneUtenteStruttura.IdModulo = struttura.IdModulo
            abilitazioneUtenteStruttura.CodiceStruttura = struttura.Codice
            abilitazioni.Add(abilitazioneUtenteStruttura)
        Next
        '***********************************************************************

        '***********************************************************************
        'Gestione abilitazioni tutte strutture e responsabile.
        '***********************************************************************
        Dim abilitazioniUtenteTutteStrutture As New List(Of ParsecAdmin.AbilitazioneUtenteTutteStrutture)
        Dim abilitazioniResponsabileModulo As New List(Of ParsecAdmin.AbilitazioneResponsabileModulo)
        For Each item As Telerik.Web.UI.GridDataItem In Me.AbilitazioniModuloGridView.Items
            Dim idModulo As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim abilitaTuttoCheckBox As CheckBox = CType(item.FindControl("abilitaTuttoCheckBox"), CheckBox)
            Dim responsabileCheckBox As CheckBox = CType(item.FindControl("responsabileCheckBox"), CheckBox)

            Dim abilitazioneUtenteTutteStrutture As ParsecAdmin.AbilitazioneUtenteTutteStrutture = Nothing
            If abilitaTuttoCheckBox.Checked Then
                'Inserisco l'abilitazione a tutte strutture associate al modulo specificato per l'utente selezionato
                abilitazioneUtenteTutteStrutture = New ParsecAdmin.AbilitazioneUtenteTutteStrutture
                abilitazioneUtenteTutteStrutture.IdModulo = idModulo
                abilitazioniUtenteTutteStrutture.Add(abilitazioneUtenteTutteStrutture)
            End If

            Dim abilitazioneResponsabileModulo As ParsecAdmin.AbilitazioneResponsabileModulo = Nothing
            If responsabileCheckBox.Checked Then
                'Inserisco l'utente selezionato come responsabile del modulo 
                abilitazioneResponsabileModulo = New ParsecAdmin.AbilitazioneResponsabileModulo
                abilitazioneResponsabileModulo.IdModulo = idModulo
                abilitazioniResponsabileModulo.Add(abilitazioneResponsabileModulo)
            End If
        Next

        '***********************************************************************
        ' Gestione funzioni utente
        '***********************************************************************
        Dim funzioniUtente As New List(Of ParsecAdmin.FunzioneUtente)
        For Each it As Telerik.Web.UI.GridDataItem In Me.AbilitazioniFunzioniGridView.Items
            Dim idFunzione As Integer = it.OwnerTableView.DataKeyValues(it.ItemIndex)("Id")
            Dim abilitataCheckBox As CheckBox = CType(it.FindControl("abilitataCheckBox"), CheckBox)
            Dim funzioneUtente As ParsecAdmin.FunzioneUtente = Nothing
            If abilitataCheckBox.Checked Then
                funzioneUtente = New ParsecAdmin.FunzioneUtente
                funzioneUtente.IdFunzione = idFunzione
                funzioniUtente.Add(funzioneUtente)
            End If
        Next
        '***********************************************************************

        Try

            utenti.Utente = Me.Utente
            utente.Gruppi = Me.Gruppi

            utenti.Save(utente, profili, abilitazioni, abilitazioniUtenteTutteStrutture, abilitazioniResponsabileModulo, funzioniUtente)

            Me.Utente = utenti.Utente

            If utenteCollegato.Id = Me.Utente.Id Then
                ParsecUtility.Applicazione.UtenteCorrente = utenti.CreateFromInstance(Me.Utente)
            End If

            'Aggiungo il nuovo utente al gruppo predefinito.
            If nuovo Then
                Dim gruppi As New ParsecAdmin.GruppoRepository
                Dim gruppoDefault As ParsecAdmin.Gruppo = gruppi.GetById(1)
                If Not gruppoDefault Is Nothing Then
                    Dim utenteGruppo As New ParsecAdmin.GruppoUtente
                    utenteGruppo.IdGruppo = gruppoDefault.Id
                    utenteGruppo.IdUtente = Me.Utente.Id

                    Dim esiste = gruppoDefault.Utenti.Where(Function(c) c.IdUtente = Me.Utente.Id).Any
                    If Not esiste Then
                        gruppoDefault.Utenti.Add(utenteGruppo)
                    End If

                    gruppi.Save(gruppoDefault)
                End If
            End If

            Dim nomefileCertificato As String = String.Format("Certificato{0}{1}", Me.Utente.Id.ToString.PadLeft(7, "0"), ".cer")



            Try
                Dim fullPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathCertificati") & nomefileCertificato
                If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PathCertificati")) Then
                    IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PathCertificati"))
                End If

                If Not IO.File.Exists(fullPath) Then
                    Me.CreaCertificato(Me.Utente, fullPath)
                Else
                    If Not password.SequenceEqual(Me.Utente.PasswordHash) Then
                        Me.CreaCertificato(Me.Utente, fullPath)
                    End If
                End If
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Impossibile creare il certificato per il seguente motivo: " & vbCrLf & ex.Message, False)
            Finally
                Dim ute = utenti.Where(Function(c) c.Id = Me.Utente.Id).FirstOrDefault
                If Not ute Is Nothing Then
                    ute.NomefileCertificato = nomefileCertificato
                    utenti.SaveChanges()
                End If
            End Try

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            utenti.Dispose()
        End Try
    End Sub

    Private Sub CreaCertificato(ByVal utente As Utente, ByVal fullPath As String)

        Dim organizzazione As String = String.Empty

        Dim cliente = CType(ParsecUtility.Applicazione.ClienteCorrente, Cliente)
        If Not cliente Is Nothing Then
            organizzazione = cliente.Descrizione
        End If

        Dim commonName As String = String.Empty

        If String.IsNullOrEmpty(utente.Cognome) Then
            commonName = utente.Cognome
        End If

        If Not String.IsNullOrEmpty(utente.Nome) Then
            If Not String.IsNullOrEmpty(commonName) Then
                commonName &= " " & utente.Nome
            Else
                commonName &= utente.Nome
            End If
        End If

        If Not String.IsNullOrEmpty(utente.CodiceFiscale) Then
            If Not String.IsNullOrEmpty(commonName) Then
                commonName &= "/" & utente.CodiceFiscale
            Else
                commonName &= utente.CodiceFiscale
            End If
        End If

        If String.IsNullOrEmpty(commonName) Then
            commonName = utente.Cognome
        End If

        'ParsecUtility.SelfSignCertificate.Create(fullPath, Me.Utente.Cognome, Me.Utente.Nome, Me.Utente.Email, organizzazione, commonName, Now, New DateTime(Now.Year + 5, Now.Month, Now.Day), Me.Utente.PasswordHash)

        ParsecUtility.SelfSignCertificate.Create(fullPath, Me.Utente.Cognome, Me.Utente.Nome, Me.Utente.Email, organizzazione, commonName, Me.Utente.PasswordHash)
    End Sub

    Private Sub Delete()
        Dim userRepository As New ParsecAdmin.UserRepository
        Dim utente As ParsecAdmin.Utente
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        utente = userRepository.GetQuery.Where(Function(c) c.Id = Me.Utente.Id).FirstOrDefault
        utente.LogDataOperazione = Now
        utente.LogUsername = utenteCollegato.Username
        utente.LogIdUtente = utenteCollegato.Id
        utente.LogTipoOperazione = "A"
        userRepository.SaveChanges()
        userRepository.Dispose()
    End Sub

    Private Sub CaricaModuli()
        Dim moduli As New ModuleRepository
        With Me.ModuloComboBox
            .DataSource = moduli.GetAbilitazioniModuli
            .DataTextField = "Descrizione"
            .DataValueField = "Id"
            .DataBind()
            .SelectedIndex = 0
        End With
        With Me.ModuloAbilitatoComboBox
            .DataSource = moduli.GetAbilitazioniModuli
            .DataTextField = "Descrizione"
            .DataValueField = "Id"
            .DataBind()
            .SelectedIndex = 0
        End With
        moduli.Dispose()
    End Sub

    Private Sub CaricaProviderFirmaD()
        With Me.ProviderFirmaComboBox
            .Items.Add(New RadComboBoxItem("", "-1"))
            .Items.Add(New RadComboBoxItem("CryptoApi", "0"))
            .Items.Add(New RadComboBoxItem("PkNet", "1"))
            .Items.Add(New RadComboBoxItem("Namirial", "2"))
            .SelectedIndex = 0
        End With
    End Sub

    Private Sub CaricaStorico()
        Dim uR As New UserRepository
        Dim storico = uR.GetStorico(Me.Utente.Username)
        Dim nl As Integer = 1
        Dim messaggio As String = String.Empty '= "Operazione eseguita da <b>" & Replace(Utente.DescrizioneUtente.ToUpper, "'", "&acute;") & "</b> il <b>" & registrazione.DataOraRegistrazione & "</b>"
        Dim width As Integer = 500
        For Each r In storico
            messaggio &= "Operazione eseguita da <b>" & Replace(r.Utente.ToUpper, "'", "&acute;") & "</b> il <b>" & r.DataOperazione & "</b><br>"
            nl += 1
        Next
        If messaggio.Length = 0 Then
            If Not Me.Utente.LogIdUtente Is Nothing Then
                Dim utente As Utente = uR.GetUserById(Me.Utente.LogIdUtente).FirstOrDefault
                messaggio = "Utenza creata da <b>" & utente.Username & " - " & utente.Cognome.ToUpper & " " & utente.Nome & "</b> il <b>" & utente.LogDataOperazione
            End If
        End If
        Dim height As Integer = nl * 16 + 5
        InfoUtenteImageButton.Attributes.Add("onclick", "ShowTooltip(this,'" & messaggio & "'," & width & "," & height & ");")
        InfoUtenteImageButton.Attributes.Add("onmouseout", "HideTooltip();")
        uR.Dispose()
    End Sub

    Private Sub CaricaVersioneJava()
        With Me.VersioneJavaComboBox
            .Items.Add(New RadComboBoxItem("", "-1"))
            'Aggiunta per la #2281
            .Items.Add(New RadComboBoxItem("1.0", "2"))

            .Items.Add(New RadComboBoxItem("1.6", "0"))
            .Items.Add(New RadComboBoxItem("1.7", "1"))
            .SelectedIndex = 0
        End With
    End Sub


    Private Sub CaricaTecnologiaClientSide()
        Me.TecnologiaClientSideComboBox.Items.Add(New RadComboBoxItem("", "-1"))
        Me.TecnologiaClientSideComboBox.Items.Add(New RadComboBoxItem("JAVA", "0"))
        Me.TecnologiaClientSideComboBox.Items.Add(New RadComboBoxItem("SOCKET", "1"))
        Me.TecnologiaClientSideComboBox.SelectedIndex = 0
    End Sub


#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub TrovaProfiloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaProfiloImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaProfiliPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaProfiloImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
    End Sub

    Protected Sub TrovaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaStrutturaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaStrutturaImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("nomeModulo", Me.ModuloAbilitatoComboBox.SelectedItem.Text)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Protected Sub AggiornaProfiloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaProfiloImageButton.Click
        If Not Session("SelectedProfiles") Is Nothing Then
            Dim profiliSelezionati As SortedList(Of Integer, String) = Session("SelectedProfiles")
            For Each profilo In profiliSelezionati
                Dim item As New Telerik.Web.UI.RadListBoxItem(profilo.Value, profilo.Key)

                If Not Me.ProfiliListBox.Items.Contains(item, New IRadListBoxItemComparer) Then
                    Me.ProfiliListBox.Items.Add(item)
                End If

            Next
        End If
        Session("SelectedProfiles") = Nothing
    End Sub

    Protected Sub EliminaProfiloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaProfiloImageButton.Click
        Me.DeleteSelectedItems(Me.ProfiliListBox)
    End Sub

    Protected Sub AggiornaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaStrutturaImageButton.Click
        Dim struttureAbilitate As List(Of ParsecAdmin.StrutturaAbilitata) = Me.StruttureAbilitate

        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")

            Dim gruppi As New ParsecAdmin.GruppoRepository

            For Each s As ParsecAdmin.StrutturaAbilitata In struttureSelezionate
                Dim idStruttura = s.Id
                Dim idModulo = s.IdModulo
                Dim idGruppo = s.IdGruppo
                Dim struttura = Me.StruttureAbilitate.Where(Function(c) c.Id = idStruttura And c.IdModulo = idModulo).FirstOrDefault
                If struttura Is Nothing Then
                    Me.StruttureAbilitate.Add(s)

                    'SE LA STRUTTURA E' ASSOCIATA AD UN GRUPPO
                    If s.IdGruppo.HasValue Then
                        If AggiornaGruppoVisibilitaCheckBox.Checked Then

                            '****************************************************************************************************
                            'AGGIUNGO IL GRUPPO DI VISIBILITA'
                            '****************************************************************************************************
                            Dim esiste As Boolean = Not Me.Gruppi.Where(Function(c) c.Id = idGruppo).FirstOrDefault Is Nothing
                            If Not esiste Then
                                Dim gruppo = gruppi.Where(Function(c) c.Id = idGruppo).FirstOrDefault
                                If Not gruppo Is Nothing Then
                                    Me.Gruppi.Add(gruppo)
                                End If
                            End If
                            '****************************************************************************************************
                        End If
                    End If



                End If
            Next
            'Dim strutture = struttureAbilitate.Union(struttureSelezionate).GroupBy(Function(c) New With {c.Id, c.IdModulo}).Select(Function(c) c.FirstOrDefault).ToList
            Me.StruttureAbilitateGridView.Rebind()
            Me.GruppiGridView.Rebind()
            'Me.StruttureAbilitate = strutture
            gruppi.Dispose()
        End If
        Session("SelectedStructures") = Nothing

    End Sub

    Protected Sub EliminaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaStrutturaImageButton.Click
        Dim struttureAbilitate As List(Of ParsecAdmin.StrutturaAbilitata) = Me.StruttureAbilitate
        For Each dataItem As GridDataItem In Me.StruttureAbilitateGridView.MasterTableView.Items
            If dataItem.Selected Then
                Dim id As Integer = dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("Id")
                Dim idModulo As Integer = dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("IdModulo")

                Dim s = Me.StruttureAbilitate.Where(Function(c) c.Id = id And c.IdModulo = idModulo).FirstOrDefault
                If Not s Is Nothing Then
                    Me.StruttureAbilitate.Remove(s)
                End If
            End If
        Next
        Me.StruttureAbilitateGridView.Rebind()
    End Sub

    Protected Sub TrovaStrutturaDefaultImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaStrutturaDefaultImageButton.Click
        Me.TrovaStrutturaDefault()
    End Sub

    Protected Sub AggiornaStrutturaDefaultImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaStrutturaDefaultImageButton.Click
        Me.AggiornaStrutturaDefault()
    End Sub

    Protected Sub EliminaStrutturaDefaultImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaStrutturaDefaultImageButton.Click
        Me.StrutturaDefaultTextBox.Text = String.Empty
        Me.CodiceStrutturaDefaultTextBox.Text = String.Empty
    End Sub

    Private Sub TrovaStrutturaDefault()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaStrutturaDefaultImageButton.ClientID)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100,200") 'SETTORE ED UFFICIO
        parametriPagina.Add("ultimoLivelloStruttura", "200") 'UFFICIO

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Private Sub AggiornaStrutturaDefault()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim codiceSettore As Integer = struttureSelezionate.First.Codice
            'Aggiorno il settore
            Me.StrutturaDefaultTextBox.Text = struttureSelezionate.First.Descrizione
            Me.CodiceStrutturaDefaultTextBox.Text = codiceSettore.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Protected Sub TrovaUtenteWindowsImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteWindowsImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtenteWindowsPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteWindowsImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 450, queryString, False)
    End Sub

    Protected Sub AggiornaUtenteWindowsImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteWindowsImageButton.Click
        If Not ParsecUtility.SessionManager.WindowsUser Is Nothing Then
            Dim windowsUser As ParsecAdmin.WindowsUser = ParsecUtility.SessionManager.WindowsUser
            ParsecUtility.SessionManager.WindowsUser = Nothing
            Me.UtenteWindowsTextBox.Text = windowsUser.ObjectGuid
        End If
    End Sub

    Protected Sub EliminaUtenteWindwsImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteWindwsImageButton.Click
        Me.UtenteWindowsTextBox.Text = String.Empty
    End Sub


   
#End Region

#Region "EVENTI GRIGLIA PROFILI"

    Protected Sub ProfiliGridView_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona utente"
            End If
            If TypeOf dataItem("Copy").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Copy").Controls(0), ImageButton)
                btn.ToolTip = "Copia utente"
            End If
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA ABILITAZIONI MODULO"

    Private Sub AbilitazioniModuloGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles AbilitazioniModuloGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA STRUTTURE ABILITATE"

    Protected Sub StruttureAbilitateGridView_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles StruttureAbilitateGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Me.EliminaStrutturaAbilitata(e)
        End Select
    End Sub

    Private Sub EliminaStrutturaAbilitata(ByVal e As GridCommandEventArgs)
        Dim id As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"))
        Dim idModulo As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("IdModulo"))
        Dim struttura = Me.StruttureAbilitate.Where(Function(c) c.Id = id And c.IdModulo = idModulo).FirstOrDefault
        If Not struttura Is Nothing Then
            Me.StruttureAbilitate.Remove(struttura)
        End If
    End Sub

    Private Sub StruttureAbilitateGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles StruttureAbilitateGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf StruttureAbilitateGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub StruttureAbilitateGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.StruttureAbilitateGridView.MasterTableView.Items
            CType(dataItem.FindControl("SelectCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    Protected Sub StruttureAbilitateGridView_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles StruttureAbilitateGridView.NeedDataSource
        Me.StruttureAbilitateGridView.DataSource = Me.StruttureAbilitate
    End Sub

    Protected Sub StruttureAbilitateGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles StruttureAbilitateGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(StruttureAbilitateGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.StruttureAbilitateGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.StruttureAbilitateGridView.SelectedItems.Count = Me.StruttureAbilitateGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.StruttureAbilitateGridView.Items.Count > 0
    End Sub

#End Region



#Region "EVENTI GRIGLIA ABILITAZIONI FUNZIONI"

    Protected Sub AbilitazioniFunzioniGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles AbilitazioniFunzioniGridView.PreRender
        If AbilitazioniFunzioniGridView.MasterTableView.GetItems(GridItemType.Item).Length > 0 Then
            Dim headerItem As GridHeaderItem = CType(AbilitazioniFunzioniGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = AbilitazioniFunzioniGridView.SelectedItems.Count = AbilitazioniFunzioniGridView.Items.Count
        End If
    End Sub

    Private Sub AbilitazioniFunzioniGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles AbilitazioniFunzioniGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf AbilitazioniFunzioniGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub AbilitazioniFunzioniGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        ' CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("abilitataCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub AbilitazioniFunzioniToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.AbilitazioniFunzioniGridView.MasterTableView.Items
            CType(dataItem.FindControl("abilitataCheckBox"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Selected = headerCheckBox.Checked
        Next
    End Sub

    Protected Sub AbilitazioniFunzioniToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

#End Region

    Protected Sub TrovaGruppoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaGruppoImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaGruppoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaGruppoImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaGruppoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaGruppoImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Not Session("GruppiSelezionati") Is Nothing Then
            Dim gruppi As New ParsecAdmin.GruppoRepository
            Dim gruppiSelezionati As SortedList(Of Integer, String) = Session("GruppiSelezionati")
            Dim idGruppo As Integer = 0
            Dim gruppo As ParsecAdmin.Gruppo = Nothing
            For Each gruppoSelezionato In gruppiSelezionati
                idGruppo = gruppoSelezionato.Key
                Dim esiste As Boolean = Not Me.Gruppi.Where(Function(c) c.Id = idGruppo).FirstOrDefault Is Nothing
                If Not esiste Then
                    gruppo = gruppi.Where(Function(c) c.Id = idGruppo).FirstOrDefault
                    If Not gruppo Is Nothing Then
                        Me.Gruppi.Add(gruppo)
                    End If
                End If
            Next
            Session("GruppiSelezionati") = Nothing
            Me.GruppiGridView.Rebind()
        End If
    End Sub

    Protected Sub EliminaGruppiSelezionatiImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaGruppiSelezionatiImageButton.Click
        Dim gruppo As ParsecAdmin.Gruppo = Nothing
        For Each selectedItem As GridDataItem In Me.GruppiGridView.SelectedItems
            Dim id = CInt(selectedItem("Id").Text)
            gruppo = Me.Gruppi.Where(Function(c) c.Id = id).FirstOrDefault
            If Not gruppo Is Nothing Then
                Me.Gruppi.Remove(gruppo)
            End If
        Next
        Me.GruppiGridView.Rebind()
    End Sub





    Private Sub GruppiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles GruppiGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf GruppiGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub GruppiToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    Protected Sub GruppiToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In Me.GruppiGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
    End Sub

    Protected Sub GruppiGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles GruppiGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.GruppiGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.GruppiGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.GruppiGridView.SelectedItems.Count = Me.GruppiGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.GruppiGridView.Items.Count > 0
    End Sub

    Protected Sub GruppiGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    Protected Sub GruppiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles GruppiGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                EliminaGruppo(e)
        End Select
    End Sub

    Protected Sub GruppiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles GruppiGridView.NeedDataSource
        Me.GruppiGridView.DataSource = Me.Gruppi
    End Sub

    Private Sub EliminaGruppo(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim id As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"))
        Dim gruppo As ParsecAdmin.Gruppo = Me.Gruppi.Where(Function(c) c.Id = id).FirstOrDefault
        If Not gruppo Is Nothing Then
            Me.Gruppi.Remove(gruppo)
        End If
    End Sub

End Class