Imports ParsecAdmin
Imports ParsecPro
Imports Telerik.Web.UI
Imports System.IO
Imports System.Web.Mail

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class GestioneEmailCestinatePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage
    Private message As New StringBuilder

#Region "PROPRIETA'"

    'Variabile di Sessione: lista delle mail mappata sulla griglia.
    Public Property Emails() As List(Of ParsecPro.EmailArrivo)
        Get
            Return CType(Session("GestioneEmailCestinatePage_Emails"), List(Of ParsecPro.EmailArrivo))
        End Get
        Set(ByVal value As List(Of ParsecPro.EmailArrivo))
            Session("GestioneEmailCestinatePage_Emails") = value
        End Set
    End Property

    'Variabile di Sessione: lista degli id delle Mail selezionate nella Griglia.
    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("GestioneEmailCestinatePage_SelectedItems") Is Nothing Then
                Session("GestioneEmailCestinatePage_SelectedItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("GestioneEmailCestinatePage_SelectedItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("GestioneEmailCestinatePage_SelectedItems") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> E-mail cestinate"

        Dim caselle As New ParsecAdmin.ParametriPecRepository
        Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Me.CaselleEmailComboBox.DataSource = caselle.GetViewAssociatoUtente(utenteCorrente.Id)
        Me.CaselleEmailComboBox.DataTextField = "Email"
        Me.CaselleEmailComboBox.DataValueField = "Id"
        Me.CaselleEmailComboBox.DataBind()
        Me.CaselleEmailComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("Seleziona", "-1"))

        If Not IsPostBack AndAlso Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
            Me.ResettaFiltro()
            Me.SelectedItems = Nothing
        End If

    End Sub

    'Evento LoadComplete associato alla Pagina. Setta i Titoli delle Griglie e gestisce i messagi di cancellazione dei Messaggi.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloElencoMailLabel.Text = "Elenco E-mail Cestinate " & If(Me.Emails.Count > 0, "( " & Me.Emails.Count.ToString & " )", "")
        If Me.EmailGridView.SelectedItems.Count > 0 Then
            Dim message As String = "Eliminare tutti gli elementi selezionati?"
            Me.EliminaEmailSelezionateImageButton.Attributes.Add("onclick", "return confirm(""" & message & """)")
        Else
            Dim message As String = "E' necessario selezionare almeno una e-mail!"
            Me.EliminaEmailSelezionateImageButton.Attributes.Add("onclick", "alert(""" & message & """); return false;")
        End If
    End Sub

    'Evento PreRender associato alla Pagina: nasconde la scrollbar di scrollPanel
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Effettuta il download del file. Richiamato da EmailGridView.ItemCommand
   Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Try
            Dim mail As ParsecPro.EmailArrivo = Me.Emails.Where(Function(c) c.Id = id).FirstOrDefault
            If Not mail Is Nothing Then
                Dim fullPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata") & mail.PercorsoRelativo & mail.NomeFileEml
                Dim file As New IO.FileInfo(fullPath)
                If file.Exists Then
                    Session("AttachmentFullName") = file.FullName
                    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                    ParsecUtility.Utility.PageReload(pageUrl, False)
                Else
                    ParsecUtility.Utility.MessageBox("L'e-mail selezionato non esiste!", False)
                End If
            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    'Evento ItemCommand associato alla Griglia EmailGridView. Esegue i vari comandi associati alla griglia.
    Protected Sub EmailGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles EmailGridView.ItemCommand
      Select Case e.CommandName
            Case "Recupera"
                Me.RecuperaEmail(e.Item)
            Case "Preview"
                DownloadFile(e.Item)
            Case "Delete"
                Me.EliminaEmail(e.Item)
            Case "Sort"
                Me.scrollPosHidden.Value = "0"
        End Select
    End Sub

    'Evento associato alla Griglia EmailGridView per la gestione dei Check.
    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedItems()
    End Sub

    'Evento associato alla Griglia EmailGridView per la gestione dei Check.
    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In EmailGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
    End Sub

    'Evento ItemCreated associato alla Griglia EmailGridView. Setta i vari stili per gli GridHeaderItem e la navigazione tra le pagine.
    Private Sub EmailGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles EmailGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf EmailGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    'Elimina una Email. Richiamato da EmailGridView.ItemCommand
    Private Sub EliminaEmail(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")


        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim mail As ParsecPro.EmailArrivo = emails.GetById(id)

        If Not mail Is Nothing Then
            Dim message As String = "L'e-mail indirizzata a " & mail.Destinatario & " del " & String.Format("{0:dd/MM/yyyy}", mail.DataArrivo) & " è stata cancellata con successo!"
            Try
                Dim fullPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata") & mail.PercorsoRelativo & mail.NomeFileEml
                If IO.File.Exists(fullPath) Then
                    IO.File.Delete(fullPath)
                End If
                emails.Delete(mail.Id)
                Me.Emails = Nothing
                Me.EmailGridView.Rebind()
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Impossibile cancellare l'e-mail selezionata, per il seguente errore:" & vbCrLf & ex.Message, False)
            Finally
                emails.Dispose()
                ParsecUtility.Utility.MessageBox(message, False)

            End Try

        End If
    End Sub

    'Recupera una email. Richiamto da EmailGridView.ItemCommand
    Private Sub RecuperaEmail(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim mail As ParsecPro.EmailArrivo = emails.GetById(id)
        Dim message As String = String.Empty
        If Not mail Is Nothing Then
            Try
                If Not mail.Cancellata Then
                    message = "L'e-mail selezionata è stata già recuperata!"
                Else
                    emails.UnDelete(mail)
                    message = "L'e-mail selezionata è stata recuperata con successo!"
                End If
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Impossibile recuperare l'e-mail selezionata, per il seguente errore:" & vbCrLf & ex.Message, False)
            Finally
                emails.Dispose()
                ParsecUtility.Utility.MessageBox(message, False)
                'Leggo le email dal database.
                Me.Emails = Nothing
                Me.EmailGridView.Rebind()
            End Try
        End If
    End Sub

    'Costruisce una lista di Id di email selezionate.
    Private Sub SaveSelectedItems()
        For Each item As GridItem In Me.EmailGridView.Items
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

    'Evento ItemPreRender associato alla griglia EmailGridView. Gestisce i check di selezione multipla.
    Protected Sub EmailGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    'Evento PreRender associato alla griglia EmailGridView. Gestisce i check di selezione multipla.
    Protected Sub EmailGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles EmailGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.EmailGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.EmailGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.EmailGridView.SelectedItems.Count = Me.EmailGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.EmailGridView.Items.Count > 0
    End Sub

    'Evento ItemDataBound associato alla griglia EmailGridView. Setta i vari tooltip e immagini in base al contenuto delle celle.
    Protected Sub EmailGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles EmailGridView.ItemDataBound
        Dim btn As ImageButton = Nothing

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim mail As ParsecPro.EmailArrivo = CType(e.Item.DataItem, ParsecPro.EmailArrivo)
            Dim visibile As Boolean = True


            If TypeOf dataItem("Recupera").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Recupera").Controls(0), ImageButton)
                btn.ToolTip = "Recupera e-mail cestinata"
                Dim message As String = "Recuperare l'e-mail selezionata?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
                If Not visibile Then
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If


           If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina e-mail cestinata"
                Dim message As String = "Eliminare l'e-mail selezionata?"
                btn.Attributes.Add("onclick", "return confirm(""" & message & """)")

                If Not visibile Then
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If

            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Preview").Controls(0), ImageButton)
                btn.ToolTip = "Apri/Salva e-mail"
                btn.Style.Add("cursor", "hand")

                If Not visibile Then
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If

            Dim id As String = mail.Id
           
            If Me.SelectedItems.ContainsKey(id) Then
                Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(SelectedItems(id).ToString())
                dataItem.Selected = True
            End If

        End If
    End Sub

    'Evento NeedDataSource associato alla griglia EmailGridView. Effettua il mapping col datasource di EmailGridView. Aggiorna la lista delle email (variabile di sessione Emails).
    Protected Sub EmailGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles EmailGridView.NeedDataSource
        '*******************************************************************
        'Leggo le email dal database applicando i filtri impostati.
        '*******************************************************************
        If Me.Emails Is Nothing Then
            Dim emailsArrivo As New ParsecPro.EmailArrivoRepository
            Me.Emails = emailsArrivo.GetViewCancellate(Me.GetFiltro)
            emailsArrivo.Dispose()
        End If
        Me.EmailGridView.DataSource = Me.Emails

    End Sub

#End Region

#Region "AZIONI PANNELLO FILTRO"

    'Costruisce il Filtro per la ricerca e lo restituisce.
    Private Function GetFiltro() As ParsecPro.EmailArrivoFiltro
        Dim filtro As New ParsecPro.EmailArrivoFiltro
        filtro.IdCasella = CInt(Me.CaselleEmailComboBox.SelectedValue)
        filtro.Cancellata = True
        If Not String.IsNullOrWhiteSpace(Me.OggettoTextBox.Text) Then
            filtro.Oggetto = Me.OggettoTextBox.Text
        End If
        If Not String.IsNullOrWhiteSpace(Me.MittenteTextBox.Text) Then
            filtro.Mittente = Me.MittenteTextBox.Text
        End If
        filtro.UtenteCancellazione = ParsecUtility.Applicazione.UtenteCorrente

        Return filtro
    End Function

    'Resetta il filtro per la ricerca.
    Private Sub ResettaFiltro()
        Me.OggettoTextBox.Text = String.Empty
        Me.MittenteTextBox.Text = String.Empty

        If CInt(Me.CaselleEmailComboBox.SelectedValue) <> -1 Then
            'Leggo le email dal database.
            Me.Emails = Nothing
        Else
            Me.Emails = New List(Of ParsecPro.EmailArrivo)
        End If
        Me.EmailGridView.Rebind()
    End Sub

    'Evento click che lancia la ricerca.
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        If CInt(Me.CaselleEmailComboBox.SelectedValue) <> -1 Then
            'Leggo le email dal database.
            Me.Emails = Nothing
            Me.EmailGridView.Rebind()
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un destinatario!", False)
        End If
    End Sub

    'Resetta il filtro per la ricerca
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

    'Evento che elimina le Email.
    Protected Sub EliminaEmailSelezionateImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaEmailSelezionateImageButton.Click

        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim id As Integer = 0
        Dim mail As ParsecPro.EmailArrivo = Nothing

        For Each item As GridDataItem In Me.EmailGridView.SelectedItems
            id = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            mail = emails.GetById(id)
            If Not mail Is Nothing Then
                'If Not mail.Cancellata Then
                Dim message As String = "L'e-mail indirizzata a " & mail.Destinatario & " del " & String.Format("{0:dd/MM/yyyy}", mail.DataArrivo) & " è stata cancellata con successo!"
                Try
                    Dim fullPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata") & mail.PercorsoRelativo & mail.NomeFileEml
                    If IO.File.Exists(fullPath) Then
                        IO.File.Delete(fullPath)
                    End If
                    emails.Delete(mail.Id)
                    Me.message.AppendLine(message)
                Catch ex As Exception
                    Me.message.AppendLine("Impossibile cancellare l'e-mail selezionata, per il seguente errore:" & vbCrLf & ex.Message)
                End Try
                'End If
            End If
        Next

        Me.Emails = Nothing
        Me.EmailGridView.Rebind()

        If Me.message.Length > 0 Then
            ParsecUtility.Utility.MessageBox(Me.message.ToString, False)
            Me.message.Clear()
        End If

        emails.Dispose()
    End Sub

#End Region

End Class
