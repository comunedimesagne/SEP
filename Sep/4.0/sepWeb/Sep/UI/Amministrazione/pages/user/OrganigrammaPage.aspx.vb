Imports ParsecAdmin
Imports System.Transactions

Partial Class OrganigrammaPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage
    Private temp As List(Of NodeItemInfo)
    Private nodeInfos As List(Of ParsecAdmin.NodeItemInfo)


#Region "PROPRIETA'"

    Public Property Struttura() As ParsecAdmin.Struttura
        Get
            Return CType(Session("OrganigrammaPage_Struttura"), ParsecAdmin.Struttura)
        End Get
        Set(ByVal value As ParsecAdmin.Struttura)
            Session("OrganigrammaPage_Struttura") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Organigramma"

        Me.TipoStruttureComboBox.Enabled = False

        If Not Me.Page.IsPostBack Then
            Me.CaricaTipologieStruttura(-1)
            Me.BuildTree(Nothing)
            Me.CaricaUfficiFatturazione()
            Me.CaricaQualifiche()
        End If

    End Sub

    Private Sub CaricaUfficiFatturazione()
        Try
            Using ipaSearch = New ParsecPro.IpaSearch
                Dim cliente = CType(ParsecUtility.Applicazione.ClienteCorrente, Cliente)
                If Not String.IsNullOrEmpty(cliente.CodiceAmministrazione) Then

                    Dim ufficiFatturazione = ipaSearch.DoSearchUfficiFatturazione(cliente.CodiceAmministrazione)

                    Dim ds = ufficiFatturazione.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Key & " - " & c.Value})

                    Me.CodiceUnivocoUfficioComboBox.DataSource = ds
                    Me.CodiceUnivocoUfficioComboBox.DataTextField = "Descrizione"
                    Me.CodiceUnivocoUfficioComboBox.DataValueField = "Id"
                    Me.CodiceUnivocoUfficioComboBox.DataBind()
                    'Me.CodiceUnivocoUfficioComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
                    'Me.CodiceUnivocoUfficioComboBox.SelectedIndex = 0

                End If
            End Using

        Catch ex As Exception

        End Try

        Me.CodiceUnivocoUfficioComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.CodiceUnivocoUfficioComboBox.SelectedIndex = 0
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la struttura selezionata?", False, Not Me.Struttura Is Nothing)
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    Me.BuildTree(Me.Struttura.Codice)
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)
            Case "Nuovo"

                'Carico le tipologie di struttura in base alla gerarchia della struttura selezionata.
                If Not Me.Struttura Is Nothing Then
                    If Struttura.IdGerarchia <> 400 Then
                        Me.CaricaTipologieStruttura(Me.Struttura.IdGerarchia)
                    Else
                        ParsecUtility.Utility.MessageBox("Impossibile associare una struttura ad una struttura di tipo Persona!", False)
                        Exit Sub
                    End If
                Else
                    CaricaTipologieStruttura(-1)
                End If

                Me.ResettaVista()
                Me.TipoStruttureComboBox.Enabled = True
                Me.TrovaStrutturaImageButton.Visible = True

                'Imposto come struttura di appartenenza predefinita la struttura selezionata.
                If Not Me.StruttureTreeView.SelectedNode Is Nothing Then
                    Me.StrutturaAppartenenzaTextBox.Text = Me.StruttureTreeView.SelectedNode.Text
                    Me.IdStrutturaTextBox.Text = Me.StruttureTreeView.SelectedNode.Value
                Else
                    Me.StrutturaAppartenenzaTextBox.Text = Me.StruttureTreeView.Nodes(0).Text
                    Me.IdStrutturaTextBox.Text = Me.StruttureTreeView.Nodes(0).Value
                End If

                Try
                    Me.AbilitaUI(Me.TipoStruttureComboBox.SelectedValue)
                Catch ex As Exception

                End Try


            Case "Annulla"
                'Me.ResettaVista()
                CaricaTipologieStruttura(-1)
                If Not Me.StruttureTreeView.SelectedNode Is Nothing Then
                    AggiornaVista(CInt(Me.StruttureTreeView.SelectedNode.Value))
                End If

            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Struttura Is Nothing Then
                        Dim message As String = "Operazione conclusa con successo!"
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.BuildTree(Nothing)
                        Catch ex As Exception
                            message = ex.Message
                        End Try
                        ParsecUtility.Utility.MessageBox(message, False)
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una struttura!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                'Dim avvisi As New ParsecAdmin.AvvisiRepository
                'Dim avviso As New ParsecAdmin.Avviso
                'avviso.Contenuto = Me.ContenutoTextBox.Text
                'Me.Avvisi = avvisi.GetAvvisi(avviso)
                'Me.AvvisiGridView.Rebind()
                'avvisi.Dispose()
        End Select
    End Sub
#End Region


#Region "EVENTI CONTROLLI"

    Protected Sub TipoStruttureComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipoStruttureComboBox.SelectedIndexChanged

        Me.AbilitaUI(e.Value)
        'Me.TrovaGruppoImageButton.Visible = True
        'Me.EliminaGruppoImageButton.Visible = True

        'Me.TrovaUtenteImageButton.Visible = True
        'Me.EliminaUtenteImageButton.Visible = True
        'Me.ResponsabileCheckBox.Enabled = True
        'Me.QualificheComboBox.Enabled = True

        'Select Case e.Value
        '    Case "400"  'PERSONA
        '        Me.TrovaGruppoImageButton.Visible = False
        '        Me.EliminaGruppoImageButton.Visible = False

        '    Case Else

        '        Me.TrovaUtenteImageButton.Visible = False
        '        Me.EliminaUtenteImageButton.Visible = False
        '        Me.ResponsabileCheckBox.Checked = False
        '        Me.ResponsabileCheckBox.Enabled = False
        '        Me.QualificheComboBox.SelectedIndex = 0
        '        Me.QualificheComboBox.Enabled = False

        'End Select
    End Sub


    Private Sub AbilitaUI(ByVal IdGerarchia As String)
        Me.TrovaGruppoImageButton.Visible = True
        Me.EliminaGruppoImageButton.Visible = True

        Me.TrovaUtenteImageButton.Visible = True
        Me.EliminaUtenteImageButton.Visible = True
        Me.ResponsabileCheckBox.Enabled = True
        Me.QualificheComboBox.Enabled = True

        Select Case IdGerarchia
            Case "400"  'PERSONA
                Me.TrovaGruppoImageButton.Visible = False
                Me.EliminaGruppoImageButton.Visible = False
            Case Else

                Me.TrovaUtenteImageButton.Visible = False
                Me.EliminaUtenteImageButton.Visible = False
                Me.ResponsabileCheckBox.Checked = False
                Me.ResponsabileCheckBox.Enabled = False
                Me.QualificheComboBox.SelectedIndex = 0
                Me.QualificheComboBox.Enabled = False

        End Select
    End Sub



    Protected Sub TrovaGruppoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaGruppoImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaGruppoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaGruppoImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 0 'SINGOLA
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaGruppoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaGruppoImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        If Not Session("GruppiSelezionati") Is Nothing Then
            Dim gruppiSelezionati As SortedList(Of Integer, String) = Session("GruppiSelezionati")
            Me.GruppoTextBox.Text = gruppiSelezionati.First.Value
            Me.idGruppoTextBox.Text = gruppiSelezionati.First.Key
            Session("GruppiSelezionati") = Nothing
        End If
    End Sub

    Protected Sub EliminaGruppoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaGruppoImageButton.Click
        Me.GruppoTextBox.Text = String.Empty
        Me.idGruppoTextBox.Text = String.Empty
    End Sub


    Protected Sub TrovaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaStrutturaImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaStrutturaImageButton.ClientID)

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("idUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        'todo abilitare selezione nodo radice.
        Select Case Me.TipoStruttureComboBox.SelectedItem.Value
            Case "100" 'Settore
                'todo oppure disabilitare i pulsanti
                Me.StrutturaAppartenenzaTextBox.Text = Me.StruttureTreeView.Nodes(0).Text
                Me.IdStrutturaTextBox.Text = Me.StruttureTreeView.Nodes(0).Value
                Return 'Il settore può essere associato solo alla struttura radice
            Case "200" 'Ufficio
                parametriPagina.Add("ultimoLivelloStruttura", "100")
            Case "300" 'Ruolo
                parametriPagina.Add("ultimoLivelloStruttura", "200")
            Case "400" 'Persona
                parametriPagina.Add("ultimoLivelloStruttura", "300")
        End Select
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Protected Sub AggiornaStrutturaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaStrutturaImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Me.StrutturaAppartenenzaTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdStrutturaTextBox.Text = struttureSelezionate.First.Id
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 0 'singola
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Me.UtenteTextBox.Text = utentiSelezionati.First.Value
            Me.IdUtenteTextBox.Text = utentiSelezionati.First.Key
            Session("SelectedUsers") = Nothing
        End If
    End Sub

    Protected Sub EliminaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteImageButton.Click
        Me.UtenteTextBox.Text = String.Empty
        Me.IdUtenteTextBox.Text = String.Empty
    End Sub

    Protected Sub SpostaSuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SpostaSuButton.Click
        If Not Me.Struttura Is Nothing Then
            Me.Struttura.DirezioneOrdine = ParsecAdmin.Struttura.DirezioneOrdinamento.Incremento
            Me.Ordina()
        End If
    End Sub

    Protected Sub SpostaGiuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SpostaGiuButton.Click
        If Not Me.Struttura Is Nothing Then
            Me.Struttura.DirezioneOrdine = ParsecAdmin.Struttura.DirezioneOrdinamento.Decremento
            Me.Ordina()
        End If
    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub CaricaTipologieStruttura(ByVal idGerarchia As Integer)
        Select Case idGerarchia
            Case Is <> 400
                Me.TipoStruttureComboBox.DataSource = (New ParsecAdmin.StrutturaLivelloRepository).GetQuery.Where(Function(c) c.Gerarchia > idGerarchia).OrderBy(Function(c) c.Id).ToList
        End Select
        Me.TipoStruttureComboBox.DataTextField = "Descrizione"
        Me.TipoStruttureComboBox.DataValueField = "Gerarchia"
        Me.TipoStruttureComboBox.DataBind()
        Me.TipoStruttureComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaQualifiche()
        Me.QualificheComboBox.DataSource = (New ParsecAdmin.QualificaOrganigrammaRepository).GetQuery.OrderBy(Function(c) c.Id).ToList
        Me.QualificheComboBox.DataTextField = "Nome"
        Me.QualificheComboBox.DataValueField = "Id"
        Me.QualificheComboBox.DataBind()
        Me.QualificheComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.QualificheComboBox.SelectedIndex = 0
    End Sub

    Private Sub BuildTree(ByVal idToSelect As String)
        Me.nodeInfos = Me.GetInfoNodes
        Me.temp = Me.nodeInfos.Where(Function(c) c.ParentID = 0).ToList

        Me.StruttureTreeView.Nodes.Clear()
        Dim clienteCorrente As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
        Dim rootNode As New Telerik.Web.UI.RadTreeNode(clienteCorrente.Descrizione, 0)
        Me.StruttureTreeView.Nodes.Add(rootNode)
        Me.AddNode(Me.temp, rootNode, idToSelect)
        Me.StruttureTreeView.ExpandAllNodes()
    End Sub

    Public Function GetInfoNodes() As List(Of NodeItemInfo)
      

        Dim strutture As New StructureRepository
        Dim struttureLivello As New StrutturaLivelloRepository(strutture.Context)


        Dim infoNodes = From struttura In strutture.GetQuery
                        Group Join strutturaLivello In struttureLivello.GetQuery
                        On struttura.IdGerarchia Equals strutturaLivello.Gerarchia
                        Into elenco = Group
                        From strutturaLivello In elenco.DefaultIfEmpty
                        Where struttura.LogStato Is Nothing
                        Order By struttura.Ordine
                        Select New NodeItemInfo() With {
                            .Id = struttura.Id,
                            .ParentID = struttura.IdPadre,
                            .Description = If(struttura.Descrizione Is Nothing, "", struttura.Descrizione),
                            .HierarchyId = struttura.IdGerarchia,
                            .Code = struttura.Codice,
                            .Responsable = struttura.Responsabile,
                            .Icon = If(strutturaLivello Is Nothing, "", strutturaLivello.UrlIcona),
                            .HierarchyDescription = If(strutturaLivello Is Nothing, "", strutturaLivello.Descrizione),
                            .Order = struttura.Ordine,
                            .Enabled = True
                        }


        'Dim res As List(Of NodeItemInfo) = infoNodes.OrderBy(Function(c) c.ParentID).ThenBy(Function(c) c.Description).ThenBy(Function(c) c.Order).ToList


        Dim res As List(Of NodeItemInfo) = infoNodes.ToList

        'Restituisco tutte le strutture.
        Return res

    End Function

    Private Sub AddNode(ByVal nodes As List(Of NodeItemInfo), ByVal t As Telerik.Web.UI.RadTreeNode, ByVal idToSelect As String)
        '  Me.temp = Me.nodeInfos.Where(Function(c) c.ParentID = CInt(t.Value)).ToList
        For Each node As NodeItemInfo In nodes
            Dim Id As Integer = node.Id
            Dim childItem As New Telerik.Web.UI.RadTreeNode(node.Description, node.Id)
            t.Nodes.Add(childItem)
            childItem.ImageUrl = node.Icon
            childItem.ToolTip = "Codice struttura: " & node.Code.ToString
            If Not idToSelect Is Nothing Then
                If node.Id = CInt(idToSelect) Then
                    childItem.Selected = True
                End If
            End If
            If node.Responsable Then
                childItem.ImageUrl = "~/images/SupUser.gif"
                childItem.ToolTip &= " - RESPONSABILE - "
            End If
            Me.temp = Me.nodeInfos.Where(Function(c) c.ParentID = Id).ToList
            Me.AddNode(Me.temp, childItem, idToSelect)
            'Me.AddNode(nodes, childItem, idToSelect)
        Next
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
        Dim strutture As New ParsecAdmin.StructureRepository
        strutture.Delete(Me.Struttura)
    End Sub


    Private Sub SalveCallback(ByVal id As Integer)

        Dim strutture As New ParsecAdmin.StructureRepository
        Dim struttura = strutture.GetById(id)


        'Dim f = 0
        'Dim i = 2 \ f

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
        Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsIstanzeOnline")

        Select Case struttura.IdGerarchia
            Case 100 'SETTORE

                Using settoreWS As New ParsecWebServices.SettoreWS

                    settoreWS.Utente = utente.Username
                    settoreWS.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)

                    settoreWS.CodiceSettore = struttura.Codice

                    If Not String.IsNullOrEmpty(struttura.Descrizione) Then
                        settoreWS.Nome = struttura.Descrizione
                    End If

                    If Not String.IsNullOrEmpty(struttura.DescrizioneDettagliata) Then
                        settoreWS.Descrizione = struttura.DescrizioneDettagliata
                    End If

                    If Not String.IsNullOrEmpty(struttura.Indirizzo) Then
                        settoreWS.Indirizzo = struttura.Indirizzo
                    End If

                    If Not String.IsNullOrEmpty(struttura.Fax) Then
                        settoreWS.Fax = struttura.Fax
                    End If

                    If Not String.IsNullOrEmpty(struttura.Telefono) Then
                        settoreWS.Telefono = struttura.Telefono
                    End If

                    If Not String.IsNullOrEmpty(struttura.Email) Then
                        settoreWS.PEC = struttura.Email
                        settoreWS.Email = struttura.Email
                    End If

                    settoreWS.Assessore = Nothing

                    settoreWS.Save()


                End Using

            Case 400  'PERSONA

                Using responsabileWS As New ParsecWebServices.ResponsabileWS
                    responsabileWS.Utente = utente.Username
                    responsabileWS.SetUp(baseUrl, cl.Identificativo, cl.CodLicenza)

                    responsabileWS.CodiceResponsabile = struttura.Codice
                    responsabileWS.Nome = Nothing

                    If Not String.IsNullOrEmpty(struttura.Descrizione) Then
                        responsabileWS.Cognome = struttura.Descrizione
                    End If

                    If Not String.IsNullOrEmpty(struttura.Email) Then
                        responsabileWS.Email = struttura.Email
                        responsabileWS.PEC = struttura.Email
                    End If

                    If Not String.IsNullOrEmpty(struttura.Telefono) Then
                        responsabileWS.Telefono = struttura.Telefono
                    End If

                    'Dim codiceSettore = strutture.Where(Function(c) c.Id = struttura.IdPadre).FirstOrDefault.Codice

                    Dim codiceSettore = Me.GetCodiceSettore(struttura.IdPadre)

                    responsabileWS.CodiceSettore = codiceSettore

                    responsabileWS.Save()

                End Using


            Case Else
                Exit Sub
        End Select
        strutture.Dispose()
      
    End Sub

    Private Sub Save()

        Dim success As Boolean = False
       
        Dim nuovo As Boolean = Me.Struttura Is Nothing

        Dim strutture As New ParsecAdmin.StructureRepository

        'Inserisco sempre una nuova struttura.
        Dim struttura As ParsecAdmin.Struttura = strutture.CreateFromInstance(Nothing)

        struttura.Cap = Trim(Me.CapTextBox.Text)
        struttura.Descrizione = Trim(Me.DescrizioneTextBox.Text)
        struttura.Email = Trim(Me.EmailTextBox.Text)
        struttura.Fax = Trim(Me.FaxTextBox.Text)
        struttura.Indirizzo = Trim(Me.IndirizzoTextBox.Text)
        struttura.Localita = Trim(Me.LocalitaTextBox.Text)
        struttura.Provincia = Trim(Me.ProvinciaTextBox.Text)


        'struttura.CodiceIPA = Trim(Me.CodiceIpaTextBox.Text)
        struttura.UfficioBilancio = Trim(Me.CodiceUfficioBilancioTextBox.Text)


        If Me.CodiceUnivocoUfficioComboBox.SelectedItem.Value <> "0" Then
            If Me.CodiceUnivocoUfficioComboBox.SelectedItem.Value = "-2" Then
                struttura.CodiceIPA = Me.CodiceUnivocoUfficioComboBox.SelectedItem.Text.Split("-")(0).Trim
            Else
                struttura.CodiceIPA = Trim(Me.CodiceUnivocoUfficioComboBox.SelectedValue)
            End If
        Else
            struttura.CodiceIPA = String.Empty
        End If


        If Me.QualificheComboBox.SelectedItem.Value <> "-1" Then
            struttura.Qualifica = Me.QualificheComboBox.SelectedItem.Value
        End If




        struttura.Responsabile = Me.ResponsabileCheckBox.Checked
        struttura.Telefono = Trim(Me.TelefonoTextBox.Text)

        If Not String.IsNullOrEmpty(Me.IdUtenteTextBox.Text) Then
            If CInt(Me.IdUtenteTextBox.Text) > 0 Then
                struttura.IDUtente = CInt(Me.IdUtenteTextBox.Text)
            End If
        End If

        If Not String.IsNullOrEmpty(Me.idGruppoTextBox.Text) Then
            struttura.IdGruppo = CInt(Me.idGruppoTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.IdStrutturaTextBox.Text) Then
            struttura.IdPadre = CInt(Me.IdStrutturaTextBox.Text)
        End If

        If nuovo Then
            'struttura.Codice = strutture.GetNuovoCodice
            struttura.IdGerarchia = Me.TipoStruttureComboBox.SelectedItem.Value
        Else
            struttura.Codice = Me.Struttura.Codice
            struttura.IdGerarchia = Me.Struttura.IdGerarchia
        End If

        struttura.LogDataRegistrazione = Now
        Dim utenteConnesso As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        struttura.LogIdUtente = utenteConnesso.Id
        struttura.LogUtente = utenteConnesso.Username

        If Not String.IsNullOrEmpty(Me.DescrizioneDettagliataTextBox.Text) Then
            struttura.DescrizioneDettagliata = Me.DescrizioneDettagliataTextBox.Text
        End If

        Try
            strutture.Struttura = Me.Struttura


            Dim moduli As New ParsecAdmin.ModuleRepository
            Dim modulo = moduli.Where(Function(c) c.Id = ParsecAdmin.TipoModulo.IOL And c.Abilitato).FirstOrDefault
            Dim eseguiWS As Boolean = Not modulo Is Nothing

            'SE IL MODULO IOL E' ATTIVO E SE IL PROCEDIMENTO E' MARCATO PER LA PUBBLICAZIONE
            If eseguiWS Then
                strutture.Save(struttura, AddressOf Me.SalveCallback)
            Else
                strutture.Save(struttura, Nothing)
            End If


            success = True

            '***********************************************************************************************************
            'AGGIUNGO IL CONTATORE DI SETTORE QUANDO VIENE CREATO UN NUOVO SETTORE
            '***********************************************************************************************************
            Dim moduloAtti As ParsecAdmin.Modulo = moduli.GetQuery.Where(Function(c) c.Abilitato = True AndAlso c.Id = 3).FirstOrDefault
            If Not moduloAtti Is Nothing Then
                If nuovo And struttura.IdGerarchia = 100 Then
                    Dim contatori As New ParsecAtt.ContatoreSettoreRepository
                    Dim contatore As New ParsecAtt.ContatoreSettore
                    contatore.IdTipologiaRegistro = 4
                    contatore.Valore = 1
                    contatore.Anno = Now.Year
                    contatore.CodiceSettore = struttura.Codice
                    contatori.Add(contatore)
                    contatori.SaveChanges()
                    contatori.Dispose()
                End If
            End If
            moduli.Dispose()
            '***********************************************************************************************************

            'Salvo il gruppo
            If Not struttura.IdGruppo.HasValue Then
                If nuovo AndAlso struttura.IdGerarchia <= 200 Then
                    Dim gruppi As New ParsecAdmin.GruppoRepository
                    Dim gruppo As New ParsecAdmin.Gruppo
                    gruppo.Descrizione = struttura.Descrizione
                    gruppo.DataInizioValidita = Now
                    gruppo.Abilitato = True
                    gruppi.Save(gruppo)
                    gruppi.Dispose()

                    Dim strutturaDaAggiornare = strutture.GetQuery.Where(Function(c) c.Id = struttura.Id).FirstOrDefault
                    If Not strutturaDaAggiornare Is Nothing Then
                        strutturaDaAggiornare.IdGruppo = gruppo.Id
                        strutture.SaveChanges()
                    End If


                End If
            End If

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try

        If success Then
            Me.Struttura = strutture.GetById(strutture.Struttura.Id)
            Me.AggiornaVista(strutture.Struttura.Id)
        End If
        strutture.Dispose()
    End Sub

    Private Sub ResettaVista()
        Me.Struttura = Nothing
        Me.TipoStruttureComboBox.Enabled = False
        Me.StrutturaAppartenenzaTextBox.Text = String.Empty
        Me.IdStrutturaTextBox.Text = String.Empty
        Me.GruppoTextBox.Text = String.Empty
        Me.idGruppoTextBox.Text = String.Empty
        Me.UtenteTextBox.Text = String.Empty
        Me.IdUtenteTextBox.Text = String.Empty
        Me.ResponsabileCheckBox.Checked = False
        Me.DescrizioneTextBox.Text = String.Empty
        Me.IndirizzoTextBox.Text = String.Empty
        Me.CapTextBox.Text = String.Empty
        Me.LocalitaTextBox.Text = String.Empty
        Me.ProvinciaTextBox.Text = String.Empty
        Me.TelefonoTextBox.Text = String.Empty
        Me.FaxTextBox.Text = String.Empty
        Me.EmailTextBox.Text = String.Empty
        'Me.CodiceIpaTextBox.Text = String.Empty
        Me.CodiceUfficioBilancioTextBox.Text = String.Empty
        Me.CodiceUnivocoUfficioComboBox.SelectedIndex = 0
    End Sub

    Private Function GetCodiceSettore(ByVal idPadre As Integer) As Integer
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim settore = strutture.Where(Function(c) c.Id = idPadre And c.LogStato Is Nothing).FirstOrDefault
        While settore.IdGerarchia <> 100
            settore = strutture.Where(Function(c) c.Id = settore.IdPadre And c.LogStato Is Nothing).FirstOrDefault
        End While
        strutture.Dispose()
        Return settore.Codice
    End Function

    Private Sub AggiornaVista(ByVal id As Integer)

        Me.CaricaTipologieStruttura(-1)
        If id <> 0 Then
            Me.ResettaVista()
            Dim strutture As New ParsecAdmin.StructureRepository


            Dim strutturaSelezionata As ParsecAdmin.Struttura = strutture.GetById(id)

            'If strutturaSelezionata.IdPadre <> 0 Then
            '    Dim settore = strutture.Where(Function(c) c.Id = strutturaSelezionata.IdPadre And c.LogStato Is Nothing).FirstOrDefault
            '    While settore.IdGerarchia <> 100
            '        settore = strutture.Where(Function(c) c.Id = settore.IdPadre And c.LogStato Is Nothing).FirstOrDefault
            '    End While
            'End If


            Try
                Me.TipoStruttureComboBox.FindItemByValue(strutturaSelezionata.IdGerarchia.ToString).Selected = True
            Catch ex As Exception
                Me.TipoStruttureComboBox.SelectedIndex = 0
            End Try





            If strutturaSelezionata.IdPadre = 0 Then
                Me.StrutturaAppartenenzaTextBox.Text = Me.StruttureTreeView.Nodes(0).Text
            Else
                Me.StrutturaAppartenenzaTextBox.Text = strutturaSelezionata.Padre
            End If

            If strutturaSelezionata.IdGruppo.HasValue Then
                Me.GruppoTextBox.Text = strutturaSelezionata.Gruppo
                Me.idGruppoTextBox.Text = strutturaSelezionata.IdGruppo
            End If



            Me.IdStrutturaTextBox.Text = strutturaSelezionata.IdPadre
            Me.UtenteTextBox.Text = strutturaSelezionata.Utente
            Me.IdUtenteTextBox.Text = strutturaSelezionata.IDUtente
            Me.ResponsabileCheckBox.Checked = strutturaSelezionata.Responsabile
            Me.DescrizioneTextBox.Text = strutturaSelezionata.Descrizione

            'Me.CodiceIpaTextBox.Text = strutturaSelezionata.CodiceIPA
            Me.CodiceUfficioBilancioTextBox.Text = strutturaSelezionata.UfficioBilancio


            Dim gestStorico As Telerik.Web.UI.RadComboBoxItem = Me.CodiceUnivocoUfficioComboBox.FindItemByValue(-2)
            If Not gestStorico Is Nothing Then
                gestStorico.Remove()
            End If

            If Not String.IsNullOrEmpty(strutturaSelezionata.CodiceIPA) AndAlso Not String.IsNullOrEmpty(strutturaSelezionata.CodiceIPA.Trim) Then
                Try
                    Me.CodiceUnivocoUfficioComboBox.FindItemByValue(strutturaSelezionata.CodiceIPA).Selected = True
                Catch ex As Exception

                    gestStorico = Me.CodiceUnivocoUfficioComboBox.FindItemByValue(-2)
                    If gestStorico Is Nothing Then
                        Me.CodiceUnivocoUfficioComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem(strutturaSelezionata.CodiceIPA & " - " & "CUU NON APPARTENENTE A QUESTA AMMINISTRAZIONE", -2))
                        Me.CodiceUnivocoUfficioComboBox.FindItemByValue(-2).Selected = True
                    End If
                End Try
            End If


            Try
                If strutturaSelezionata.Qualifica <> 0 Then
                    Me.QualificheComboBox.FindItemByValue(strutturaSelezionata.Qualifica.ToString).Selected = True
                Else
                    Me.QualificheComboBox.SelectedIndex = 0
                End If
            Catch ex As Exception

            End Try

            Me.IndirizzoTextBox.Text = strutturaSelezionata.Indirizzo
            Me.CapTextBox.Text = strutturaSelezionata.Cap
            Me.LocalitaTextBox.Text = strutturaSelezionata.Localita
            Me.ProvinciaTextBox.Text = strutturaSelezionata.Provincia
            Me.TelefonoTextBox.Text = strutturaSelezionata.Telefono
            Me.FaxTextBox.Text = strutturaSelezionata.Fax
            Me.EmailTextBox.Text = strutturaSelezionata.Email


            Me.TrovaStrutturaImageButton.Visible = True
            Me.TrovaGruppoImageButton.Visible = True
            Me.EliminaGruppoImageButton.Visible = True
            Me.TrovaUtenteImageButton.Visible = True
            Me.EliminaUtenteImageButton.Visible = True
            Me.ResponsabileCheckBox.Enabled = True
            Me.QualificheComboBox.Enabled = True

            If strutturaSelezionata.IdGerarchia = 400 Then
                Me.TrovaGruppoImageButton.Visible = False
                Me.EliminaGruppoImageButton.Visible = False
            Else
                If strutturaSelezionata.IdGerarchia = 100 Then
                    Me.TrovaStrutturaImageButton.Visible = False
                End If
                Me.TrovaUtenteImageButton.Visible = False
                Me.EliminaUtenteImageButton.Visible = False
                Me.ResponsabileCheckBox.Checked = False
                Me.ResponsabileCheckBox.Enabled = False
                Me.QualificheComboBox.SelectedIndex = 0
                Me.QualificheComboBox.Enabled = False
            End If


            Me.Struttura = strutturaSelezionata
            strutture.Dispose()
        Else
            Me.ResettaVista()
        End If
    End Sub



    Private Sub Ordina()

        Dim message As String = ""
        Dim processa As Boolean = True

        If Not Me.StruttureTreeView.SelectedNode Is Nothing Then
            If Me.StruttureTreeView.SelectedNode.Value <> "0" Then

                Dim strutture As New ParsecAdmin.StructureRepository
                Dim curStruttura As ParsecAdmin.Struttura = strutture.GetQuery.Where(Function(c) c.Id = Me.Struttura.Id).FirstOrDefault
                Dim curOrdinale As Integer = curStruttura.Ordine

                Dim maxOrdinale As Integer = strutture.GetOrdinale(curStruttura, StructureRepository.TipoOrdinale.Max)
                Dim minOrdinale As Integer = strutture.GetOrdinale(curStruttura, StructureRepository.TipoOrdinale.Min)
                Dim direzione As ParsecAdmin.Struttura.DirezioneOrdinamento = Me.Struttura.DirezioneOrdine

                If direzione = ParsecAdmin.Struttura.DirezioneOrdinamento.Decremento Then
                    If curOrdinale = maxOrdinale Then processa = False
                End If
                If direzione = ParsecAdmin.Struttura.DirezioneOrdinamento.Incremento Then
                    If curOrdinale = minOrdinale Then
                        processa = False
                    End If

                End If
                If processa Then

                    Dim destStruttura As ParsecAdmin.Struttura = Nothing

                    If direzione = ParsecAdmin.Struttura.DirezioneOrdinamento.Incremento Then
                        destStruttura = strutture.GetOrdinamentoStrutturaPrecedente(curStruttura.IdPadre, curOrdinale)
                    End If

                    If direzione = ParsecAdmin.Struttura.DirezioneOrdinamento.Decremento Then
                        destStruttura = strutture.GetOrdinamentoStrutturaSuccessiva(curStruttura.IdPadre, curOrdinale)
                    End If


                    curStruttura.Ordine = destStruttura.Ordine
                    destStruttura.Ordine = curOrdinale

                    strutture.SaveChanges()

                    Me.BuildTree(curStruttura.Id)

                End If

                strutture.Dispose()

            Else
                message = "Non è possibile spostare il nodo radice dell'organigramma!"
            End If
        Else
            message = "E' necessario selezionare la struttura che si desidera riposizionare!"
        End If


    End Sub

#End Region


#Region "EVENTI TREEVIEW"

    Protected Sub StruttureTreeView_NodeClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles StruttureTreeView.NodeClick
        Me.AggiornaVista(CInt(e.Node.Value))
    End Sub


    Protected Sub StruttureTreeView_NodeDrop(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeDragDropEventArgs) Handles StruttureTreeView.NodeDrop

        Dim sourceNode As Telerik.Web.UI.RadTreeNode = e.SourceDragNode
        Dim destNode As Telerik.Web.UI.RadTreeNode = e.DestDragNode
        Dim dropPosition As Telerik.Web.UI.RadTreeViewDropPosition = e.DropPosition

        If Not destNode Is Nothing Then
            If Not sourceNode.Equals(destNode) Then

                Dim strutture As New ParsecAdmin.StructureRepository
                Dim curStruttura As ParsecAdmin.Struttura = strutture.Where(Function(c) c.Id = CInt(sourceNode.Value)).FirstOrDefault
                Dim destStruttura As ParsecAdmin.Struttura = strutture.Where(Function(c) c.Id = CInt(destNode.Value)).FirstOrDefault
                Dim processa As Boolean = False

                Select Case dropPosition
                    Case Telerik.Web.UI.RadTreeViewDropPosition.Over
                        processa = True
                End Select

                If curStruttura.IdPadre <> destStruttura.IdPadre Then
                    processa = False
                End If

                If processa Then
                    Dim curOrdinale As Integer = curStruttura.Ordine
                    Dim destOrdinale As Integer = destStruttura.Ordine
                    curStruttura.Ordine = destOrdinale
                    destStruttura.Ordine = curOrdinale
                    strutture.SaveChanges()

                    Me.BuildTree(curStruttura.Id)
                End If

                strutture.Dispose()
            End If
        End If

    End Sub

#End Region


    Protected Sub EsportaButton_Click(sender As Object, e As EventArgs) Handles EsportaButton.Click
        Try
            Me.Esporta()
        Catch ex As Exception
            If Not ex.InnerException Is Nothing Then
                ParsecUtility.Utility.MessageBox(ex.InnerException.Message, False)
            Else
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End If
        End Try
    End Sub



    Private Function Escape(ByVal s As String) As String
        Dim res As String = String.Empty

        If Not String.IsNullOrEmpty(s) Then
            s = s.Replace(vbCrLf, " ")
            s = s.Replace(vbTab, "")
            s = s.Replace("""", """""")
            Dim formatString As String = """{0}"""
            res = String.Format(formatString, s)
        End If

        Return res
    End Function


    Private Sub Esporta()
        If Me.Struttura Is Nothing Then
            Throw New ApplicationException("E' necessario selezionare una struttura!")
        End If

        Dim pathExport As String = String.Empty

        Try
            pathExport = ParsecAdmin.WebConfigSettings.GetKey("PathExport")
        Catch ex As Exception
            Throw New ApplicationException(ex.Message & vbCrLf & "Cartella dell'export non configurata, contattare gli amministratori del sistema!")
        End Try

        Try
            If Not System.IO.Directory.Exists(pathExport) Then
                System.IO.Directory.CreateDirectory(pathExport)
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.Message.Replace("\", "\\"))
        End Try

        Dim procedimenti As New ParsecAdmin.ProcedimentoRepository
        Dim view = procedimenti.Where(Function(c) c.CodiceResponsabile = Me.Struttura.Codice).ToList

        If view Is Nothing OrElse view.Count = 0 Then
            Throw New ApplicationException("Non ci sono procedimenti associati alla struttura selezionata." & vbCrLf & "Impossibile eseguire l'esportazione!")
        End If


        '***************************************************************************************
        'CREO IL FILE
        '***************************************************************************************
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("ProcedimentiResponsabile_{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy"))
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As New StringBuilder

        line.Append("ID" & vbTab)
        line.Append("NOME" & vbTab)
        line.Append("DESCRIZIONE" & vbTab)

        swExport.WriteLine(line.ToString)
        line.Clear()

        For Each item As ParsecAdmin.Procedimento In view

            line.Append(item.Id.ToString & vbTab)
            line.Append(Me.Escape(item.Nome) & vbTab)
            line.Append(Me.Escape(item.Descrizione) & vbTab)


            swExport.WriteLine(line.ToString)
            line.Clear()
        Next
        swExport.Close()

        '***************************************************************************************

        '***************************************************************************************
        'VISUALIZZO IL FILE
        '***************************************************************************************
        Session("AttachmentFullName") = fullPathExport
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
        '***************************************************************************************

    End Sub


End Class