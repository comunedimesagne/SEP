Imports ParsecAdmin
Imports ParsecPro
Imports Rebex.Net
Imports Rebex.Mail
Imports Telerik.Web.UI
Imports System.IO
Imports System.Web.Mail
Imports System.Web.Services

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class RegistrazioneEmailArrivoPage
    Inherits System.Web.UI.Page

#Region "DICHIARAZIONI"

    Private WithEvents MainPage As MainPage
    Private message As New StringBuilder
    Private Const ItemsPerRequest As Integer = 10

    'Classe Item di appoggio
    Private Class Item
        Public Property Id As Integer = 0
        Public Property Descrizione As String = String.Empty
    End Class

#End Region

#Region "PROPRIETA'"

    'Variabile di sessione: lista delle mail associate alla griglia EmailGridView
    Public Property Emails() As List(Of ParsecPro.EmailArrivo)
        Get
            Return CType(Session("RegistrazioneEmailArrivoPage_Emails"), List(Of ParsecPro.EmailArrivo))
        End Get
        Set(ByVal value As List(Of ParsecPro.EmailArrivo))
            Session("RegistrazioneEmailArrivoPage_Emails") = value
        End Set
    End Property

    'Variabile di sessione: oggetto Email corrente.
    Public Property Email As ParsecPro.EmailArrivo
        Get
            Return CType(Session("RegistrazioneEmailArrivoPage_Email"), ParsecPro.EmailArrivo)
        End Get
        Set(ByVal value As ParsecPro.EmailArrivo)
            Session("RegistrazioneEmailArrivoPage_Email") = value
        End Set
    End Property

    'Variabile di sessione: lista di Id delle mail selezionate nella griglia EmailGridView.
    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("RegistrazioneEmailArrivoPage_SelectedItems") Is Nothing Then
                Session("RegistrazioneEmailArrivoPage_SelectedItems") = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("RegistrazioneEmailArrivoPage_SelectedItems"), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("RegistrazioneEmailArrivoPage_SelectedItems") = value
        End Set
    End Property

    'Variabile di sessione: elelnco delle caselle PEC definite nel DB
    Public Property CasellaPec() As ParsecAdmin.ParametriPec
        Get
            Return CType(Session("RegistrazioneEmailArrivoPage_CasellaPec"), ParsecAdmin.ParametriPec)
        End Get
        Set(ByVal value As ParsecAdmin.ParametriPec)
            Session("RegistrazioneEmailArrivoPage_CasellaPec") = value
        End Set
    End Property

    'Variabile di sessione: indica la segnatura del Protocollo
    Public Property Segnatura As Interoperabilita.Segnatura
        Get
            Return CType(Session("RegistrazioneEmailArrivoPage_Segnatura"), Interoperabilita.Segnatura)
        End Get
        Set(value As Interoperabilita.Segnatura)
            Session("RegistrazioneEmailArrivoPage_Segnatura") = value
        End Set
    End Property

    'Variabile a sola lettura: indica se è attivata la Scrivania per il protocollo.
    Public ReadOnly Property IterAttivato As Boolean
        Get
            Dim res As Boolean = False
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AttivaGestioneScrivaniePro", ParsecAdmin.TipoModulo.SEP)
            If Not parametro Is Nothing Then
                res = CBool(parametro.Valore)
            End If
            parametri.Dispose()
            Return res
        End Get
    End Property


#End Region

#Region "EVENTI PAGINA"

    'carica le caselle di posta elettronica nel campo CaselleEmailComboBox della pagina aspx
    Private Sub CaricaCaselle()
        Dim caselle As New ParsecAdmin.ParametriPecRepository
        Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim caselleAssociate = caselle.GetViewAssociatoUtente(utenteCorrente.Id)
        Me.CaselleEmailComboBox.DataSource = caselleAssociate
        Me.CaselleEmailComboBox.DataTextField = "Email"
        Me.CaselleEmailComboBox.DataValueField = "Id"
        Me.CaselleEmailComboBox.DataBind()
        Dim casellaPredefinita As ParsecAdmin.ParametriPec = caselleAssociate.Where(Function(c) Not c.Predefinita Is Nothing).FirstOrDefault
        Me.CaselleEmailComboBox.SelectedIndex = 0
        If Not casellaPredefinita Is Nothing Then
            If casellaPredefinita.Predefinita Then
                Me.CaselleEmailComboBox.Items.FindItemByValue(casellaPredefinita.Id).Selected = True
            End If
        End If
    End Sub

    'carica le tipologie di mail e le associa al campo TipologieCheckBoxList della maschera.
    Private Sub CaricaTipologie()
        Dim items As New List(Of Item)
        items.Add(New Item With {.Id = 0, .Descrizione = "E-mail"})
        items.Add(New Item With {.Id = 1, .Descrizione = "PEC"})
        items.Add(New Item With {.Id = 2, .Descrizione = "Anomalie"})
        items.Add(New Item With {.Id = 3, .Descrizione = "Accettazione"})
        items.Add(New Item With {.Id = 4, .Descrizione = "Consegna"})
        items.Add(New Item With {.Id = 5, .Descrizione = "Errore Consegna"})
        items.Add(New Item With {.Id = 6, .Descrizione = "Non Accettazione"})
        items.Add(New Item With {.Id = 7, .Descrizione = "Preavviso Errore Consegna"})
        items.Add(New Item With {.Id = 8, .Descrizione = "Fatture Elettroniche"})
        Me.TipologieCheckBoxList.DataSource = items
        Me.TipologieCheckBoxList.DataTextField = "Descrizione"
        Me.TipologieCheckBoxList.DataValueField = "Id"
        Me.TipologieCheckBoxList.DataBind()
    End Sub

    'Carica gli stati delle Mail e li associa al campo StatoComboBox della maschera
    Private Sub CaricaStatiProtocollo()
        Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente

        Dim items As New List(Of Item)
        items.Add(New Item With {.Id = 0, .Descrizione = "Tutte escluse l'eliminate"})
        items.Add(New Item With {.Id = 1, .Descrizione = "Protocollate"})
        items.Add(New Item With {.Id = 2, .Descrizione = "Non Protocollate"})


        If utenteCorrente.SuperUser Then
            items.Add(New Item With {.Id = 3, .Descrizione = "Eliminate"})
        End If

        Me.StatoComboBox.DataSource = items
        Me.StatoComboBox.DataTextField = "Descrizione"
        Me.StatoComboBox.DataValueField = "Id"
        Me.StatoComboBox.DataBind()
        Me.StatoComboBox.SelectedIndex = 2
    End Sub

    'Carica gli Stati di Lettura delle Mail e li associa al campo StatoLetturaComboBox della maschera.
    Private Sub CaricaStatiLettura()
        Dim items As New List(Of Item)
        items.Add(New Item With {.Id = 0, .Descrizione = "Tutte"})
        items.Add(New Item With {.Id = 1, .Descrizione = "Sì"})
        items.Add(New Item With {.Id = 2, .Descrizione = "No"})

        Me.StatoLetturaComboBox.DataSource = items
        Me.StatoLetturaComboBox.DataTextField = "Descrizione"
        Me.StatoLetturaComboBox.DataValueField = "Id"
        Me.StatoLetturaComboBox.DataBind()
        Me.StatoLetturaComboBox.SelectedIndex = 0
    End Sub

    'Evento Init della Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = "div.RadUploadProgressArea_Office2007 .ruProgress { background-image: none;}" & vbCrLf
        css.InnerHtml += ".RadUploadProgressArea { width: 320px !important;}" & vbCrLf
        css.InnerHtml += "div.RadUploadProgressArea li.ruProgressHeader{ margin: 10px 18px 0px; }" & vbCrLf
        css.InnerHtml += "table.CkeckListCss tr td label {margin-right:10px;padding-right:10px;}" & vbCrLf
        Me.Page.Header.Controls.Add(css)

        Me.Title = "Registrazione"
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> Registrazione E-mail in Arrivo"

        If Not IsPostBack Then

            Me.VisualizzaFatturaControl.Visible = False
            Me.VisualizzaEmailControl.Visible = False

            Me.CaricaCaselle()

            Me.CaricaTipologie()
            Me.CaricaStatiProtocollo()
            Me.CaricaStatiLettura()

            'Determino se l'utente passato è responsabile del modulo protocollo.
            Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
            Dim responsabileModulo As Boolean = Not (New ParsecAdmin.AbilitazioneResponsabileModuloRepository).GetQuery.Where(Function(c) c.IdModulo = 2 And c.IdUtente = utenteCorrente.Id And c.LogTipoOperazione Is Nothing).FirstOrDefault Is Nothing
            Me.EliminaEmailImageButton.Enabled = responsabileModulo
            Me.RecuperaEmailImageButton.Enabled = responsabileModulo

            If Not utenteCorrente.SuperUser Then
                Dim abilitaRicezioneEmail = utenteCorrente.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.DisabilitaRicezioneEmail)).FirstOrDefault Is Nothing
                Me.RiceviEmailImageButton.Enabled = abilitaRicezioneEmail
                If Not abilitaRicezioneEmail Then
                    Me.RiceviEmailImageButton.ToolTip = "Non abilitato alla ricezione delle email"
                End If
            End If

        End If

        If Not IsPostBack AndAlso Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
            Me.ResettaFiltro()

            Me.SelectedItems = Nothing
            Me.CasellaPec = Nothing

        End If

    End Sub

    'Evento LoadComplete della Pagina: gestisce i messaggi riguardanti la eliminazione delle mail.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        Me.ElencoEmailLabel.Text = "Elenco E-mail Ricevute&nbsp;" & If(Me.Emails.Count > 0, "( " & Me.Emails.Count.ToString & " )  ", "")
        If Me.EmailGridView.SelectedItems.Count > 0 Then
            Dim message As String = "Eliminare tutti gli elementi selezionati?"
            Me.EliminaEmailImageButton.Attributes.Add("onclick", "return confirm(""" & message & """)")
        Else
            Dim message As String = "E' necessario selezionare almeno un'email!"
            Me.EliminaEmailImageButton.Attributes.Add("onclick", "alert(""" & message & """); return false;")
        End If

    End Sub

#End Region

#Region "EVENTI USERCONTROL FATTURA ED EMAIL"

    'Nasconde il controllo VisualizzaEmailControl
    Protected Sub VisualizzaEmailControl_OnCloseEvent() Handles VisualizzaEmailControl.OnCloseEvent
        Me.VisualizzaEmailControl.Visible = False
    End Sub

    'Nasconde il controllo VisualizzaFatturaControl
    Protected Sub VisualizzaFatturaControl_OnCloseEvent() Handles VisualizzaFatturaControl.OnCloseEvent
        Me.VisualizzaFatturaControl.Visible = False
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemCommand della griglia EmailGridView: fa partire i comandi associati alla griglia EmailGridView.
    Protected Sub EmailGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles EmailGridView.ItemCommand
        Dim casellePec As New ParsecAdmin.ParametriPecRepository
        Me.CasellaPec = casellePec.GetByIdAssociatoUtente(CInt(Me.CaselleEmailComboBox.SelectedValue))
        casellePec.Dispose()
        Select Case e.CommandName
            Case "Protocolla"
                Me.ProtocollaEmail(e.Item)
            Case "Preview"
                Me.DownloadFile(e.Item)
            Case "Delete"
                Me.EliminaEmail(e.Item)
            Case "PreviewFattura"
                Me.VisualizzaFattura(e.Item)
        End Select
    End Sub

    'Visualizza la Fattura Elettronica nell'apposito pannellino.
    Private Sub VisualizzaFattura(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim fatture As New ParsecPro.FatturaElettronicaRepository
        Dim fattura = fatture.Where(Function(c) c.MessaggioSdI.IdEmail = id).FirstOrDefault

        If Not fattura Is Nothing Then

            Me.VisualizzaFatturaControl.ShowPanel()
            Me.VisualizzaFatturaControl.InitUI(fattura)
            Dim emails As New ParsecPro.EmailArrivoRepository
            Dim email = emails.Where(Function(c) c.Id = id).FirstOrDefault
            If Not email Is Nothing Then
                If Not email.Letta Then
                    email.Letta = True
                    emails.SaveChanges()
                    'Leggo le email dal database.
                    Me.Emails = Nothing
                    Me.EmailGridView.Rebind()
                End If
            End If
            emails.Dispose()
        End If
        fatture.Dispose()

    End Sub

    'Metodo ItemCreated della griglia EmailGridView: associa lo stile agli itemi di tipo GridHeaderItem e cura la navigazione tra pagine.
    Private Sub EmailGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles EmailGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf EmailGridView_ItemPreRender
        End If

        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
        End If

        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If

    End Sub

    'Metodo ItemPreRender della griglia EmailGridView: gestisce i check per la selezione degli item nella griglia.
    Protected Sub EmailGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub

    'Metodo PreRender della griglia EmailGridView: gestisce i check per la selezione degli item nella griglia.
    Protected Sub EmailGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles EmailGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.EmailGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.EmailGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.EmailGridView.SelectedItems.Count = Me.EmailGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.EmailGridView.Items.Count > 0
    End Sub

    'Metodo ItemDataBound della griglia EmailGridView: cura i text, i tooltip e le icone nella griglia in base al contenuto delle celle.
    Protected Sub EmailGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles EmailGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        Dim utenteCorrente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim message As ParsecPro.EmailArrivo = CType(e.Item.DataItem, ParsecPro.EmailArrivo)
            Dim visibile As Boolean = Not message.Lavorata
            Dim cancellata As Boolean = message.Cancellata
            Dim protocollata As Boolean = (Not message.IdRegistrazione Is Nothing)
            Dim esisteemail As Boolean = True

            Dim emailImage As Image = CType(e.Item.FindControl("EmailImage"), Image)
            emailImage.ImageUrl = "~\images\icon-msg-unread.gif"

            Dim tipoEmail As ParsecPro.Pop3.Header.TipologiaEmail = CType(message.Tipo, ParsecPro.Pop3.Header.TipologiaEmail)


            If Not message.Letta Then
                CType(e.Item.FindControl("MittenteLabel"), Label).Text = "<div title='" & message.Mittente & "'style=""white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 230px;font-weight: bold; "">" & message.Mittente & "</div>"
                CType(e.Item.FindControl("OggettoLabel"), Label).Text = "<div title='" & message.Oggetto.Replace("'", "&#39;") & "'style=""white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 280px;font-weight: bold; "">" & message.Oggetto.Replace("'", "&#39;") & "</div>"
                CType(e.Item.FindControl("DataArrivoLabel"), Label).Text = "<div title='" & String.Format("{0:dd/MM/yyyy HH:mm}", message.DataArrivo) & "'style=""white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 90px;font-weight: bold; "">" & String.Format("{0:dd/MM/yyyy HH:mm}", message.DataArrivo) & "</div>"

                If tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC Then
                    emailImage.ImageUrl = "~\images\icon-msg-signed.gif"
                Else
                    emailImage.ImageUrl = "~\images\icon-msg-unread.gif"
                End If
                emailImage.ToolTip = "Non Letta"
            Else
                CType(e.Item.FindControl("MittenteLabel"), Label).Text = "<div title='" & message.Mittente & "'style=""white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 230px;"">" & message.Mittente & "</div>"
                CType(e.Item.FindControl("OggettoLabel"), Label).Text = "<div title='" & message.Oggetto.Replace("'", "&#39;") & "'style=""white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 280px;"">" & message.Oggetto.Replace("'", "&#39;") & "</div>"
                CType(e.Item.FindControl("DataArrivoLabel"), Label).Text = "<div title='" & String.Format("{0:dd/MM/yyyy HH:mm}", message.DataArrivo) & "'style=""white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 90px;"">" & String.Format("{0:dd/MM/yyyy HH:mm}", message.DataArrivo) & "</div>"

                If tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC Then
                    emailImage.ImageUrl = "~\images\icon-msg-signed-read.gif"
                Else
                    emailImage.ImageUrl = "~\images\icon-msg-read.gif"
                End If
                emailImage.ToolTip = "Letta"
            End If

            If TypeOf dataItem("PreviewFattura").Controls(0) Is ImageButton Then
                btn = CType(dataItem("PreviewFattura").Controls(0), ImageButton)
                Dim fatture As New ParsecPro.FatturaElettronicaRepository
                Dim fattura = fatture.Where(Function(c) c.MessaggioSdI.IdEmail = message.Id)
                If fattura.Any Then
                    btn.ToolTip = "Visualizza Fattura..."
                    btn.Style.Add("cursor", "hand")
                Else
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.Attributes.Add("onclick", "return false;")
                End If
                fatture.Dispose()
            End If

            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Preview").Controls(0), ImageButton)
                If protocollata Then
                    Dim rr As New RegistrazioniRepository
                    Dim r As ParsecPro.Registrazione = rr.GetById(message.IdRegistrazione)
                    Dim ut As ParsecAdmin.Utente = (New UserRepository).GetQuery.Where(Function(c) c.Id = r.IdUtente).FirstOrDefault
                    If Not r Is Nothing Then
                        btn.ToolTip = "Visualizza Protocollo N. " & r.NumeroProtocollo & " " & r.DescrizioneTipologiaRegistristrazione.Substring(0, 1).ToUpper & " /" & CDate(r.DataOraRegistrazione).Year
                        If Not ut Is Nothing Then btn.ToolTip &= " - " & ut.Username.ToUpper
                    Else
                        btn.ToolTip = "Visualizza Protocollo..."
                    End If
                    rr.Dispose()
                    btn.Style.Add("cursor", "hand")
                Else
                    Dim fPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata") & message.PercorsoRelativo & message.NomeFileEml
                    Dim file As New IO.FileInfo(fPath)
                    If file.Exists Then
                        btn.ToolTip = "Apri e-mail..."
                        btn.Style.Add("cursor", "hand")
                    Else
                        esisteemail = False
                        btn.ImageUrl = "~\images\EliminaAvviso16.png"
                        btn.ToolTip = "Il file " & message.NomeFileEml & " non esiste!"
                        btn.Attributes.Add("onclick", "return false;")
                    End If
                End If
            End If

            If TypeOf dataItem("Protocolla").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Protocolla").Controls(0), ImageButton)
                btn.Style.Add("cursor", "hand")
                If cancellata Then
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = "E-mail cancellata"
                    btn.Attributes.Add("onclick", "return false;")
                ElseIf protocollata Then
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = "E-mail protocollata..."
                    btn.Attributes.Add("onclick", "return false;")
                ElseIf Not esisteemail Then
                    btn.ImageUrl = "~\images\NonInviato.png"
                    btn.ToolTip = "Non protocollabile perchè non esiste il file " & message.NomeFileEml & " dell'e-mail!"
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If

            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.Style.Add("cursor", "hand")
                If cancellata Then
                    btn.ImageUrl = "~\images\undo16.png"
                    btn.ToolTip = "E-mail cancellata, "
                    Dim ut As ParsecAdmin.Utente = (New UserRepository).GetQuery.Where(Function(c) c.Id = message.IdUtenteCancellazione).FirstOrDefault
                    btn.ToolTip &= "da - " & ut.Username.ToUpper & " il " & message.DataCancellazione & " -, recuperarla"
                    btn.Attributes.Add("onclick", "return false;")
                ElseIf protocollata Then
                    btn.ImageUrl = "~\images\vuoto.png"
                    Dim rr As New RegistrazioniRepository
                    Dim r As ParsecPro.Registrazione = rr.GetById(message.IdRegistrazione)
                    If Not r Is Nothing Then
                        btn.ToolTip = "E-mail protocollata: Protocollo N. " & r.NumeroProtocollo & " " & r.DescrizioneTipologiaRegistristrazione.Substring(0, 1).ToUpper & " /" & CDate(r.DataOraRegistrazione).Year
                    Else
                        btn.ToolTip = "E-mail protocollata..."
                    End If
                    rr.Dispose()
                    btn.Attributes.Add("onclick", "return false;")
                Else
                    Dim messaggio As String = "Eliminare l'e-mail selezionata?"
                    btn.Attributes.Add("onclick", "if (window.confirm(""" & messaggio & """)==false) return false;")
                    btn.ImageUrl = "~\images\Delete16.png"
                    btn.ToolTip = "Cancella e-mail"
                End If
                'Determino se l'utente passato è responsabile del modulo protocollo.
                Dim responsabileModulo As Boolean = Not (New ParsecAdmin.AbilitazioneResponsabileModuloRepository).GetQuery.Where(Function(c) c.IdModulo = 2 And c.IdUtente = utenteCorrente.Id And c.LogTipoOperazione Is Nothing).FirstOrDefault Is Nothing
                btn.Enabled = responsabileModulo
            End If

            If TypeOf dataItem("Sblocca").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Sblocca").Controls(0), ImageButton)
                btn.Style.Add("cursor", "hand")
                If Not protocollata And Not cancellata Then
                    If message.Lockata Then
                        btn.ImageUrl = "~\images\Lock_16.png"
                        If message.IdUtenteLock = utenteCorrente.Id Then
                            btn.ToolTip = "Sblocca l'e-mail"
                        Else
                            Dim ut As ParsecAdmin.Utente = (New UserRepository).GetQuery.Where(Function(c) c.Id = message.IdUtenteLock).FirstOrDefault
                            btn.ToolTip = "E-mail BLOCCATA "
                            If Not ut Is Nothing Then btn.ToolTip &= "da -" & ut.Username.ToUpper
                            btn.Attributes.Add("onclick", "return false;")
                        End If
                    ElseIf Not esisteemail Then
                        btn.ImageUrl = "~\images\vuoto.png"
                        btn.Attributes.Add("onclick", "return false;")
                    Else
                        btn.ImageUrl = "~\images\Unlock_16.png"
                        btn.ToolTip = "E-mail Disponibile"
                        btn.Attributes.Add("onclick", "return false;")
                    End If
                Else
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If

            Dim headers As GridItem() = Me.EmailGridView.MasterTableView.GetItems(GridItemType.Header)
            Dim header As GridHeaderItem = headers(0)
            Dim selectAllCheckBox As CheckBox = CType(header.Controls(2).Controls(1), CheckBox)
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            chk.Style.Add("display", If(visibile, "block", "none"))
            selectAllCheckBox.Visible = visibile
            Dim id As String = message.Id
            If SelectedItems.ContainsKey(id) Then
                'Seleziono la checkbox e la riga.
                chk.Checked = Convert.ToBoolean(SelectedItems(id).ToString())
                dataItem.Selected = True
            End If
        End If

    End Sub

    'NeedDataSource della Griglia EmailGridView: effettua una ricerca e i risultati li associa alla griglia..aggiornandone il contenuto. Aggiorna la variabile di sessione Emails 
    Protected Sub EmailGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles EmailGridView.NeedDataSource
        'Leggo le email dal database applicando i filtri impostati.
        If Me.Emails Is Nothing Then
            Dim emailsArrivo As New ParsecPro.EmailArrivoRepository
            Me.Emails = emailsArrivo.GetView(Me.GetFiltro)
            emailsArrivo.Dispose()
        End If
        Me.EmailGridView.DataSource = Me.Emails
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    'Elimina, se si può, le mail dal db e quindi dalla Griglia.
    Protected Sub EliminaEmailImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaEmailImageButton.Click

        Dim sbMessage As New StringBuilder
        If Me.EmailGridView.SelectedItems.Count = 0 Then
            Dim message As String = "Per eseguire la cancellazione massiva è necessario selezionare almeno un elemento!"
            ParsecUtility.Utility.MessageBox(message, False)
            Exit Sub
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim i As Integer = 0
        For Each item As GridDataItem In Me.EmailGridView.SelectedItems
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim emailBloccata As ParsecPro.EmailArrivo = emails.GetLock(id)
            If Not emailBloccata Is Nothing Then
                sbMessage.AppendLine("L'email - '" & emailBloccata.Oggetto & "' -" & vbCrLf & " è bloccata dall'utente '" & emailBloccata.UtenteBlocco & "'!")
                emailBloccata = Nothing
            End If
            Dim mailMessage As ParsecPro.EmailArrivo = emails.Where(Function(c) c.Id = id).FirstOrDefault
            If Not mailMessage Is Nothing Then
                Dim success As Boolean = True
                Try
                    If Not mailMessage.Cancellata Then
                        emails.Delete(mailMessage, utenteCollegato)
                        i += 1
                    End If
                Catch ex As Exception
                    success = False
                    sbMessage.AppendLine("Impossibile eliminare l'e-mail - " & mailMessage.Oggetto & "- , per il seguente errore:" & vbCrLf & ex.Message)
                End Try
            End If
        Next
        emails.Dispose()

        If sbMessage.Length = 0 Then
            Dim message As String = String.Empty
            If i = 0 Then
                message = "La cancellazione massiva non si è potuto eseguire perchè l'e-mail selezionate erano già cancellate!"
            Else
                message = "La cancellazione massiva è stata eseguita con successo!" & vbCrLf & "Eliminate " & i & " / " & Me.EmailGridView.SelectedItems.Count & " emails!"
            End If
            ParsecUtility.Utility.MessageBox(message, False)
        Else
            Dim message As String = "Eliminate " & i & " / " & Me.EmailGridView.SelectedItems.Count & " emails!" & vbCrLf & "ERRORE" & vbCrLf & sbMessage.ToString
            ParsecUtility.Utility.MessageBox(sbMessage.ToString, False)
        End If

        'Leggo le email dal database.
        Me.Emails = Nothing
        Me.EmailGridView.Rebind()
    End Sub

    'Visualizza la Registrazione di protocollo associata alla mail.
    Protected Sub VisualizzaRegistrazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaRegistrazioneImageButton.Click

        If Not Session("RicercaRegistrazione_IdRegistrazioneSelezionata") Is Nothing Then
            Dim registrazioni As New ParsecPro.RegistrazioniRepository
            Dim registrazione As ParsecPro.Registrazione = registrazioni.GetById(Session("RicercaRegistrazione_IdRegistrazioneSelezionata"))
            Dim pageUrl As String = "~/UI/Protocollo/pages/user/RegistrazioneArrivoPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("Mode", "Update")

            If Not Session("Allegato") Is Nothing Then
                registrazione.Allegati.Add(Session("Allegato"))
                Session("Allegato") = Nothing
            End If

            Dim parametriPagina As New Hashtable
            parametriPagina.Add("RegistrazioneEmail", registrazione)
            queryString.Add("obj", Me.AggiornaEmailButton.ClientID)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina


            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 650, queryString, False)
            Session("RicercaRegistrazione_IdRegistrazioneSelezionata") = Nothing
        Else
            'Sblocco l'e-mail
            Dim emails As New ParsecPro.EmailArrivoRepository
            emails.SetLock(Me.Email.Id, Nothing)
            emails.Dispose()
        End If

    End Sub

    'Questo pulsante viene pressato per aggiornare lo stato dell'email dalla finestra popup.
    Protected Sub AggiornaEmailButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaEmailButton.Click
        Dim emails As New ParsecPro.EmailArrivoRepository

        If Not ParsecUtility.SessionManager.Registrazione Is Nothing Then
            Dim registrazione As ParsecPro.Registrazione = ParsecUtility.SessionManager.Registrazione
            'Impostare l'email su lavorata
            ParsecUtility.SessionManager.Registrazione = Nothing
            'Invio e-mail di avvenuta protocollazione
            Try
                Me.InviaEmailAvvenutaProtocollazione(registrazione)

            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try

            'Leggo le email dal database.
            Me.Emails = Nothing
            Me.EmailGridView.Rebind()
        End If

        emails.SetLock(Me.Email.Id, Nothing)
        emails.Dispose()
    End Sub

    'Questo pulsante viene pressato per aggiornare lo stato dell'email dalla finestra popup.
    Protected Sub AggiornaEmailSenzaInvioNotificaButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaEmailSenzaInvioNotificaButton.Click
        Dim emails As New ParsecPro.EmailArrivoRepository

        If Not ParsecUtility.SessionManager.Registrazione Is Nothing Then
            Dim registrazione As ParsecPro.Registrazione = ParsecUtility.SessionManager.Registrazione
            'Impostare l'email su lavorata
            ParsecUtility.SessionManager.Registrazione = Nothing

            'Leggo le email dal database.
            Me.Emails = Nothing
            Me.EmailGridView.Rebind()
        End If

        emails.SetLock(Me.Email.Id, Nothing)
        emails.Dispose()
    End Sub

    'Resetta la pagina
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
    End Sub

    'Effettua una ricerca secondo i filtri impostati
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        If Me.CaselleEmailComboBox.Items.Count > 0 Then
            If Me.CaselleEmailComboBox.SelectedIndex > -1 Then
                If Not DateEsatte() Then Exit Sub
                'Leggo le email dal database.
                Me.Emails = Nothing
                Me.EmailGridView.Rebind()
            Else
                ParsecUtility.Utility.MessageBox("E' necessario selezionare una casella di posta!", False)
            End If
        Else
            ParsecUtility.Utility.MessageBox("E' necessario configurare almeno una casella di posta!", False)
        End If
    End Sub

    'Metodo SelectedIndexChanged del controllo CaselleEmailComboBox: al variare della casella aggiorno il contenuto della maschera
    Protected Sub CaselleEmailComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles CaselleEmailComboBox.SelectedIndexChanged
        If Me.CaselleEmailComboBox.SelectedIndex > -1 Then
            ImpostaTipologia()
            'Leggo le email dal database.
            Me.Emails = Nothing
            Me.EmailGridView.Rebind()
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una casella di posta!", False)
        End If
    End Sub

    'Evento click che effettua un "Ricevi" della casella postale selezionata
    Protected Sub RiceviEmailImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RiceviEmailImageButton.Click
        If Me.CaselleEmailComboBox.Items.Count > 0 Then
            If Me.CaselleEmailComboBox.SelectedIndex > -1 Then
                Me.RiceviEmail()
            Else
                ParsecUtility.Utility.MessageBox("E' necessario selezionare una casella di posta!", False)
            End If
        Else
            ParsecUtility.Utility.MessageBox("E' necessario configurare almeno una casella di posta!", False)
        End If
    End Sub

    'Collega le mail di tipo Notifiche SDI al protocollo fattura
    Protected Sub CollegaEmailImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles CollegaEmailImageButton.Click
        If Me.CaselleEmailComboBox.Items.Count > 0 Then
            If Me.CaselleEmailComboBox.SelectedIndex > -1 Then
                Try
                    Me.CollegaNotificheSdiAlProtocolloFattura()
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox("Si è verificato il seguente errore:" & vbCrLf & ex.Message, False)
                End Try

            Else
                ParsecUtility.Utility.MessageBox("E' necessario selezionare una casella di posta!", False)
            End If
        Else
            ParsecUtility.Utility.MessageBox("E' necessario configurare almeno una casella di posta!", False)
        End If
    End Sub

    'Esporta la griglia delle email in foprmato excel
    Protected Sub EsportaXls_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EsportaXls.Click
        If Me.Emails Is Nothing Then
            ParsecUtility.Utility.MessageBox("Fase dell'esportazione dell'e-mails non possibile!", False)
        Else
            If Me.Emails.Count > 0 Then
                Dim pathExport As String = ConfigurationManager.AppSettings("PathExport")
                If pathExport Is Nothing Then
                    ParsecUtility.Utility.MessageBox("Cartella dell'export non configurata, contattare gli amministratori del sistema!", False)
                    Exit Sub
                Else
                    If pathExport.Length = 0 Then
                        ParsecUtility.Utility.MessageBox("Cartella dell'export non configurata, contattare gli amministratori del sistema!", False)
                        Exit Sub
                    Else
                        If Not System.IO.Directory.Exists(pathExport) Then System.IO.Directory.CreateDirectory(pathExport)
                    End If
                End If
                Dim uC As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                Dim exportFilename As String = "Email_" & uC.Id.ToString & "_AL_" & String.Format("{0:dd_MM_yyyy}", Now) & ".xls"
                Dim fullPathExport As String = pathExport & exportFilename
                Dim swExport As New System.IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
                HeaderFile(swExport)
                For Each doc As EmailArrivo In Me.Emails
                    DetailFileDoc(swExport, doc)
                Next
                SaveStreamWriter(fullPathExport, exportFilename, swExport)
            Else
                ParsecUtility.Utility.MessageBox("Non ci sono E-mails, non si può esportare nulla!", False)
            End If
        End If
    End Sub

    'Cerca di effettuare un recupero massivo delle mail 
    Protected Sub RecuperaEmailImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles RecuperaEmailImageButton.Click
        Dim sbMessage As New StringBuilder
        If Me.EmailGridView.SelectedItems.Count = 0 Then
            Dim message As String = "Per eseguire il recupero massivo è necessario selezionare almeno un elemento!"
            ParsecUtility.Utility.MessageBox(message, False)
            Exit Sub
        End If

        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim i As Integer = 0
        For Each item As GridDataItem In Me.EmailGridView.SelectedItems
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim mailMessage As ParsecPro.EmailArrivo = emails.Where(Function(c) c.Id = id And c.Cancellata = True).FirstOrDefault
            If Not mailMessage Is Nothing Then
                Dim success As Boolean = True
                Try
                    emails.UnDelete(mailMessage)
                    i += 1
                Catch ex As Exception
                    success = False
                    sbMessage.AppendLine("Impossibile recuperare l'e-mail - " & mailMessage.Oggetto & "- , per il seguente errore:" & vbCrLf & ex.Message)
                End Try
            End If
        Next
        emails.Dispose()
        If sbMessage.Length = 0 Then
            Dim message As String = String.Empty
            If i = 0 Then
                message = "Il recupero massivo non si è potuto eseguire perchè l'e-mail selezionate non erano cancellate!"
            Else
                message = "Il recupero massivo è stato eseguito con successo!" & vbCrLf & "Recuperate " & i & " / " & Me.EmailGridView.SelectedItems.Count & " emails!"
            End If
            ParsecUtility.Utility.MessageBox(message, False)
        Else
            Dim message As String = "Recuperate " & i & " / " & Me.EmailGridView.SelectedItems.Count & " emails!" & vbCrLf & "ERRORE" & vbCrLf & sbMessage.ToString
            ParsecUtility.Utility.MessageBox(message, False)
        End If
        'Leggo le email dal database.
        Me.Emails = Nothing
        Me.EmailGridView.Rebind()
    End Sub

#End Region

#Region "METODI PRIVATI"

    'configura l'smtp per l'invio delle mail
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
            WriteLog(ex)
        End Try
        Return client
    End Function

    'Verifica la correttezza di un indirizzo mail
    Private Function CheckEmail(ByVal Indirizzo As String) As Boolean
        If String.IsNullOrEmpty(Indirizzo) Then
            Return False
        End If
        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
        Dim m = emailRegex.Match(Indirizzo)
        Return m.Success
    End Function

    'Invia mail di avvenuta protocollazione
    Private Sub InviaEmailAvvenutaProtocollazione(ByVal prot As ParsecPro.Registrazione)

        If Not NotificaEmailProtocollazione() Then
            Exit Sub
        End If

        If Me.VerificaMittenteFatturaElettronica(Me.Email.Mittente) Then
            Exit Sub
        End If

        Dim caselle As New ParsecAdmin.ParametriPecRepository
        Dim cl As ParsecAdmin.Cliente = ParsecUtility.Applicazione.ClienteCorrente
        Dim casella As ParsecAdmin.ParametriPec = Nothing
        If Me.CheckEmail(cl.Email) Then
            casella = caselle.Where(Function(c) c.Email = cl.Email).FirstOrDefault
        End If
        If casella Is Nothing Then
            casella = caselle.GetById(Me.Email.IdCasella)
        End If
        caselle.Dispose()

        If Not casella Is Nothing Then

            Dim oggettoEmail = ChrW(34) & Me.Email.Oggetto & ChrW(34)
            Dim numero As String = prot.NumeroProtocollo.ToString
            Dim anno As String = prot.DataImmissione.Value.Year.ToString
            Dim data = prot.DataImmissione.Value.ToShortDateString
            Dim orario = prot.DataImmissione.Value.ToLongTimeString
            Dim oggetto As String = cl.Descrizione & " - Avvenuta Protocollazione N° " & numero & "/" & anno & " del " & data & " - oggetto: " & oggettoEmail & "."

            Dim corpo As String = "Il giorno " & data & " alle ore " & orario & " il messaggio con oggetto:"
            corpo &= vbCrLf & oggettoEmail & " è stato protocollato con n° " & numero & "/" & anno & "."
            corpo &= vbCrLf & vbCrLf & "ATTENZIONE: mail generata automaticamente dal sistema di protocollo del " & cl.Descrizione & " - non rispondere. "

            Dim mail As New Rebex.Mail.MailMessage
            Dim client As Rebex.Net.Smtp = Me.ConfigureSmtp(casella)
            mail.From = casella.Email
            mail.To.Add(Me.Email.Mittente)

            mail.Subject = oggetto
            mail.BodyText = corpo
            mail.Priority = Rebex.Mail.MailPriority.High

            If Not Me.Segnatura Is Nothing Then
                Dim allegaConferma As Interoperabilita.DestinazioneConfermaRicezione = Interoperabilita.DestinazioneConfermaRicezione.no

                If Not Me.Segnatura.Intestazione.Destinazione Is Nothing Then
                    allegaConferma = Me.Segnatura.Intestazione.Destinazione(0).confermaRicezione
                End If

                If allegaConferma = Interoperabilita.DestinazioneConfermaRicezione.si Then

                    Dim filenameTemp As String = Guid.NewGuid.ToString & "_ConfermaRicezione.xml"

                    Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
                    If Not IO.Directory.Exists(pathRootTemp) Then
                        IO.Directory.CreateDirectory(pathRootTemp)
                    End If

                    Dim pathDownload As String = pathRootTemp & filenameTemp

                    Me.GeneraConfermaRicezione(prot, Me.Segnatura, pathDownload)

                    Dim mailAttach As New Rebex.Mail.Attachment(pathDownload, "ConfermaRicezione.xml", Rebex.Mail.MediaTypeNames.Text.Xml)

                    mail.Attachments.Add(mailAttach)

                    IO.File.Delete(pathDownload)

                End If

                Me.Segnatura = Nothing
            End If

            Try
                client.Timeout = 0
                client.Send(mail)
                client.Disconnect()
            Catch ex As Exception
                WriteLog(ex)
                Throw New ApplicationException("Impossibile inviare l'email di avvenuta protocollazione per il seguente motivo:" & vbCrLf & ex.Message)
                Exit Sub
            End Try

            'SALVO L'EMAIL SU DISCO
            Dim percorsoRelativo As String = String.Format("\{0}\", Now.Year)
            Dim nomeEmail As String = Guid.NewGuid.ToString & ".eml"
            Try

                Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & percorsoRelativo & nomeEmail
                If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata")) Then
                    IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata"))
                End If
                If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & percorsoRelativo) Then
                    IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PostaInviata") & percorsoRelativo)
                End If
                mail.Save(mailBoxPath, Rebex.Mail.MailFormat.Mime)
            Catch ex As Exception
                Throw New ApplicationException("Impossibile salvare l'email su disco per il seguente motivo:" & vbCrLf & ex.Message)
                Exit Sub
            End Try

            'INSERISCO L'EMAIL NEL DB
            Try
                Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
                Dim emails As New EmailRepository
                Dim email As ParsecPro.Email = emails.CreateFromInstance(Nothing)
                email.IdCasella = casella.Id
                email.Inviata = True
                email.IdUtente = utente.Id
                email.DataInvio = Now
                email.Corpo = mail.BodyText
                email.Oggetto = mail.Subject
                email.Destinatari = Me.Email.Mittente
                email.PercorsoRelativo = percorsoRelativo
                email.NomeFileEml = nomeEmail

                email.MessaggioId = mail.MessageId.Id

                emails.Save(email)
                emails.Dispose()

            Catch ex As Exception
                Dim msg As String = String.Empty
                If Not ex.InnerException Is Nothing Then
                    msg = ex.InnerException.Message
                Else
                    msg = ex.Message
                End If
                Throw New ApplicationException("Impossibile salvare i dati dell'email nel database per il seguente motivo:" & vbCrLf & msg)
                Exit Sub
            End Try
        End If

    End Sub

    'Elimina la mail selezionata
    Private Sub EliminaEmail(ByVal item As Telerik.Web.UI.GridDataItem)

        If Me.CasellaPec Is Nothing Then
            Exit Sub
        End If

        Dim sbMessage As New StringBuilder
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim emailBloccata As ParsecPro.EmailArrivo = emails.GetLock(id)

        If Not emailBloccata Is Nothing Then
            ParsecUtility.Utility.MessageBox("L'email - '" & emailBloccata.Oggetto & "' -" & vbCrLf & " non è cancellabile perchè bloccata dall'utente '" & emailBloccata.UtenteBlocco & "'!", False)
            emailBloccata = Nothing
            emails.Dispose()
            Exit Sub
        End If

        Dim mailMessage As ParsecPro.EmailArrivo = emails.Where(Function(c) c.Id = id).FirstOrDefault
        If Not mailMessage Is Nothing Then
            If mailMessage.Cancellata Then
                Dim success As Boolean = True
                Try
                    emails.UnDelete(mailMessage)
                Catch ex As Exception
                    success = False
                    sbMessage.AppendLine("Impossibile recuperare l'e-mail selezionata, per il seguente errore:" & vbCrLf & ex.Message)
                End Try
                If sbMessage.Length = 0 Then
                    Dim message As String = "L'e-mail selezionata è stata recuperata con successo!"
                    ParsecUtility.Utility.MessageBox(message, False)
                Else
                    ParsecUtility.Utility.MessageBox(sbMessage.ToString, False)
                End If
            Else
                Dim success As Boolean = True
                Try
                    emails.Delete(mailMessage, utenteCollegato)
                Catch ex As Exception
                    success = False
                    sbMessage.AppendLine("Impossibile eliminare l'e-mail selezionata, per il seguente errore:" & vbCrLf & ex.Message)
                End Try
                If sbMessage.Length = 0 Then
                    Dim message As String = "L'e-mail selezionata è stata cancellata con successo!"
                    ParsecUtility.Utility.MessageBox(message, False)
                Else
                    ParsecUtility.Utility.MessageBox(sbMessage.ToString, False)
                End If
            End If
        End If
        emails.Dispose()

        'Leggo le email dal database.
        Me.Emails = Nothing
        Me.EmailGridView.Rebind()
    End Sub

    'Deserializza la segnatura di Protocollo
    Private Function DeserializeSegnatura(ByVal strm As IO.Stream) As ParsecPro.Interoperabilita.Segnatura
        Dim segnatura As ParsecPro.Interoperabilita.Segnatura = Nothing
        Try
            Dim serializer = New System.Xml.Serialization.XmlSerializer(GetType(ParsecPro.Interoperabilita.Segnatura))
            segnatura = serializer.Deserialize(strm)
        Catch ex As Exception
        End Try
        Return segnatura
    End Function

    'Metodo per svuotare l'xmlns se uguale a xmlns=""http://www.digitPa.gov.it/protocollo/""
    Private Function FixSegnaturaXml(ByVal strm As IO.Stream) As IO.Stream
        Dim doc = XDocument.Load(strm)
        Dim xml As String = doc.ToString
        xml = xml.Replace("xmlns=""http://www.digitPa.gov.it/protocollo/""", "")
        doc = XDocument.Parse(xml)
        Dim ms As New IO.MemoryStream
        doc.Save(ms)
        ms.Position = 0
        Return ms
    End Function

    'Ritorna l'allegato Segnatura associato alla mail
    Private Function GetAllegatoSegnatura(ByVal nomefile As String, ByVal buffer As Byte()) As ParsecPro.Allegato
        Dim filenameTemp As String = Guid.NewGuid.ToString & "_" & nomefile

        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        If Not IO.Directory.Exists(pathRootTemp) Then
            IO.Directory.CreateDirectory(pathRootTemp)
        End If

        Dim pathDownload As String = pathRootTemp & filenameTemp
        Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")

        IO.File.WriteAllBytes(pathDownload, buffer)

        Dim allegato As New ParsecPro.Allegato

        allegato.NomeFile = nomefile
        allegato.NomeFileTemp = filenameTemp

        allegato.IdTipologiaDocumento = 0
        allegato.DescrizioneTipologiaDocumento = "Allegato"

        allegato.Oggetto = "Allegato Segnatura Protocollo"
        allegato.PercorsoRoot = pathRoot
        allegato.PercorsoRootTemp = pathRootTemp
        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

        If nomefile.ToLower.EndsWith(".pdf") Then
            allegato.Scansionato = True
        End If

        Return allegato
    End Function

    'Restituisce l'allegato della mail
    Private Function GetAllegatoEmail(ByVal mailMessage As ParsecPro.EmailArrivo) As ParsecPro.Allegato

        Dim nomefile As String = Guid.NewGuid.ToString & ".eml"
        Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
        Dim fullPathEmail As String = mailBoxPath & mailMessage.PercorsoRelativo & mailMessage.NomeFileEml
        Dim tipoEmail As ParsecPro.Pop3.Header.TipologiaEmail = CType(mailMessage.Tipo, ParsecPro.Pop3.Header.TipologiaEmail)

        'Copio l'allegato nella cartella temporanea.
        Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
        IO.File.Copy(fullPathEmail, pathDownload)
        Dim allegato As New ParsecPro.Allegato
        allegato.NomeFile = nomefile
        allegato.NomeFileTemp = nomefile
        allegato.IdTipologiaDocumento = 1 'Primario
        allegato.DescrizioneTipologiaDocumento = "Primario"

        Select Case tipoEmail
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_Normale
                allegato.Oggetto = "Messaggio e-mail"
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC
                Dim parametri As New ParsecAdmin.ParametriRepository
                Dim parametro = parametri.GetByName("PEC_DescrizioneMailIn", ParsecAdmin.TipoModulo.PRO)
                parametri.Dispose()
                If Not parametro Is Nothing Then
                    allegato.Oggetto = parametro.Valore
                Else
                    allegato.Oggetto = "Messaggio originale da PEC"
                End If
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC_Anomalia
                allegato.Oggetto = "Messaggio PEC con anomalia"
        End Select
        allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
        Return allegato
    End Function

    'Legge la Fattura Elettronica e costruisce, restituendolo, un oggetto Registrazione
    Private Function LeggiFattura(fattura As XElement) As ParsecPro.Registrazione
        Dim registrazione As New Registrazione

        Dim header = fattura.Element("FatturaElettronicaHeader")

        'Destinatario Fattura
        Dim codiceIpaDestinatario = header.Element("DatiTrasmissione").Element("CodiceDestinatario").Value

        'Mittente fattura
        Dim cedentePrestatore = header.Element("CedentePrestatore")
        Dim datiAnagrafici = cedentePrestatore.Element("DatiAnagrafici")
        Dim anagrafica = datiAnagrafici.Element("Anagrafica")

        Dim denominazione As String = String.Empty
        Dim nome As String = String.Empty
        Dim cognome As String = String.Empty
        Dim codicePartitaIva As String = String.Empty
        Dim codiceFiscale As String = String.Empty
        Dim numeroCivivo As String = String.Empty
        Dim provincia As String = String.Empty
        Dim email As String = String.Empty

        If Not anagrafica.Element("Denominazione") Is Nothing Then
            denominazione = anagrafica.Element("Denominazione").Value
        End If

        If Not anagrafica.Element("Nome") Is Nothing Then
            nome = anagrafica.Element("Nome").Value
        End If
        If Not anagrafica.Element("Cognome") Is Nothing Then
            cognome = anagrafica.Element("Cognome").Value
        End If

        'Il numero di identificazione fiscale ai fini IVA
        Dim fiscalePartitaIva = datiAnagrafici.Element("IdFiscaleIVA")
        If Not fiscalePartitaIva.Element("IdCodice") Is Nothing Then
            codicePartitaIva = fiscalePartitaIva.Element("IdCodice").Value
        End If

        Dim contatti = cedentePrestatore.Element("Contatti")
        If Not contatti Is Nothing Then
            If Not contatti.Element("Email") Is Nothing Then
                email = contatti.Element("Email").Value
            End If

        End If

        Dim sede = cedentePrestatore.Element("Sede")

        Dim indirizzo = sede.Element("Indirizzo").Value
        If Not sede.Element("NumeroCivico") Is Nothing Then
            numeroCivivo = sede.Element("NumeroCivico").Value
        End If

        Dim cap = sede.Element("CAP").Value
        Dim comune = sede.Element("Comune").Value
        If Not sede.Element("Provincia") Is Nothing Then
            provincia = sede.Element("Provincia").Value
        End If

        Dim oggetto As String = String.Empty

        Dim body = fattura.Elements("FatturaElettronicaBody")
        For Each datiGenerali In body
            Dim datiGeneraliDocumento = datiGenerali.Element("DatiGenerali").Element("DatiGeneraliDocumento")

            Dim numerofattura = datiGeneraliDocumento.Element("Numero").Value

            Dim descrizioneTipoDocumento As String = "Fattura"
            If Not datiGeneraliDocumento.Element("TipoDocumento") Is Nothing Then
                Dim tipoDocumento = datiGeneraliDocumento.Element("TipoDocumento").Value
                Select Case tipoDocumento
                    Case "TD01"
                        descrizioneTipoDocumento = "Fattura"
                    Case "TD02"
                        descrizioneTipoDocumento = "Acconto/anticipo su fattura"
                    Case "TD03"
                        descrizioneTipoDocumento = "Acconto/anticipo su parcella"
                    Case "TD04"
                        descrizioneTipoDocumento = "Nota di credito"
                    Case "TD05"
                        descrizioneTipoDocumento = "Nota di debito"
                    Case "TD06"
                        descrizioneTipoDocumento = "Parcella"
                End Select
            End If

            oggetto &= descrizioneTipoDocumento & " n. " & numerofattura.ToString
            Dim datafattura = Date.Parse(datiGeneraliDocumento.Element("Data").Value).ToShortDateString

            oggetto &= " del " & datafattura & vbCrLf


            If Not datiGeneraliDocumento.Element("Causale") Is Nothing Then
                Dim causalefattura = datiGeneraliDocumento.Element("Causale").Value
            End If

        Next

        Dim destinatarioTrovato As Boolean = False

        'RICERCO IL DESTINATARIO PER CIG
        'SE IL PARAMETRO CHE ABILITA QUESTA FUNZIONALITA' ESISTE ED E' IMPOSTATO SU TRUE

        Dim cig As String = String.Empty

        Dim codiceCIG = fattura.Descendants("CodiceCIG").GroupBy(Function(c) c).Select(Function(c) c.FirstOrDefault).ToList

        If codiceCIG.Count = 1 Then
            cig = codiceCIG.FirstOrDefault

            Dim impegni As New ParsecAtt.ImpegnoSpesaRepository

            Dim atti As New ParsecAtt.DocumentoRepository(impegni.Context)

            Dim impegniConCig = (From impegno In impegni.GetQuery
                       Join atto In atti.GetQuery
                       On impegno.IdDocumento Equals atto.Id
                       Where atto.LogStato Is Nothing And impegno.CIG = cig And atto.IdTipologiaDocumento = 2
                       Select impegno).ToList


            impegni.Dispose()

            If impegniConCig.Count = 1 Then

                Dim documenti As New ParsecAtt.DocumentoRepository
                Dim idDocumento = impegniConCig.FirstOrDefault.IdDocumento
                Dim documento = documenti.Where(Function(c) c.Id = idDocumento).FirstOrDefault
                documenti.Dispose()

                If Not documento Is Nothing Then
                    Dim strutture As New ParsecAtt.StrutturaViewRepository
                    Dim struttura = strutture.Where(Function(c) c.Id = documento.IdStruttura And (c.LogStato Is Nothing Or c.LogStato = "M")).FirstOrDefault
                    If Not struttura Is Nothing Then
                        If struttura.LogStato = "M" Then
                            struttura = strutture.Where(Function(c) c.Codice = struttura.Codice And c.LogStato Is Nothing).FirstOrDefault
                            If Not struttura Is Nothing Then
                                Dim dest = New ParsecPro.Destinatario(struttura.Id, True)
                                dest.Iter = True
                                registrazione.Destinatari.Add(dest)
                                destinatarioTrovato = True
                            End If
                        Else
                            Dim dest = New ParsecPro.Destinatario(struttura.Id, True)
                            dest.Iter = True
                            registrazione.Destinatari.Add(dest)
                            destinatarioTrovato = True
                        End If
                    End If
                    strutture.Dispose()
                End If
            End If
        End If

        'RICERCO IL DESTINATARIO PER CODICDE IPA
        If Not destinatarioTrovato Then
            Dim referenti As New ParsecAdmin.StructureRepository
            Dim referenteInterno = referenti.Where(Function(c) c.CodiceIPA = codiceIpaDestinatario And c.LogStato Is Nothing).FirstOrDefault
            referenti.Dispose()

            If Not referenteInterno Is Nothing Then
                Dim dest = New ParsecPro.Destinatario(referenteInterno.Id, True)
                dest.Iter = True
                registrazione.Destinatari.Add(dest)
            End If
        End If

        Dim mittente As ParsecPro.Mittente = Nothing

        'CERCO L'ULTIMO
        Dim rubrica As New RubricaRepository
        Dim strutturaEsterna As StrutturaEsternaInfo = rubrica.GetQuery.Where(Function(c) c.CodiceFiscale = codicePartitaIva And c.LogStato Is Nothing).OrderByDescending(Function(c) c.Id).FirstOrDefault
        rubrica.Dispose()

        If strutturaEsterna Is Nothing Then
            mittente = New Mittente
            mittente.Rubrica = False
            mittente.Indirizzo = indirizzo & If(Not String.IsNullOrEmpty(numeroCivivo), ", " & numeroCivivo, "")
            mittente.Cap = cap
            mittente.Provincia = provincia
            mittente.Citta = comune
            mittente.Email = email
            mittente.CodiceFiscalePartitaIva = codicePartitaIva

        Else
            If (strutturaEsterna.Denominazione = denominazione OrElse strutturaEsterna.Denominazione = cognome) AndAlso strutturaEsterna.Nome = nome AndAlso strutturaEsterna.Comune = comune AndAlso strutturaEsterna.CAP = cap Then
                mittente = New Mittente(strutturaEsterna.Id, False)
            Else
                mittente = New Mittente
                mittente.Rubrica = False
                mittente.Indirizzo = indirizzo & If(Not String.IsNullOrEmpty(numeroCivivo), ", " & numeroCivivo, "")
                mittente.Cap = cap
                mittente.Provincia = provincia
                mittente.Citta = comune
                mittente.Email = email
                mittente.CodiceFiscalePartitaIva = codicePartitaIva
            End If


        End If

        If String.IsNullOrEmpty(denominazione) Then
            mittente.Cognome = cognome
            mittente.Nome = nome
            mittente.Descrizione = cognome & " " & nome
        Else
            mittente.Cognome = denominazione
            mittente.Descrizione = denominazione
        End If

        registrazione.Mittenti.Add(mittente)
        registrazione.Oggetto = oggetto

        Return registrazione
    End Function

    'Restituisce l'Oggetto FatturaElettronica a partire dall'XML ricevuto
    Private Function CaricaFattura(fattura As XElement) As ParsecPro.FatturaElettronica
        Dim fatturaElettronica As New ParsecPro.FatturaElettronica

        Dim header = fattura.Element("FatturaElettronicaHeader")

        Dim codiceCIG = fattura.Descendants("CodiceCIG").GroupBy(Function(c) c).Select(Function(c) c.FirstOrDefault).ToList

        If codiceCIG.Count > 0 Then
            Dim listaCIG = codiceCIG.Select(Function(c) c.Value).ToList
            fatturaElettronica.ElencoCig = String.Join(" ", listaCIG)
        End If

        'Mittente fattura
        Dim cedentePrestatore = header.Element("CedentePrestatore")
        Dim datiAnagrafici = cedentePrestatore.Element("DatiAnagrafici")
        Dim anagrafica = datiAnagrafici.Element("Anagrafica")

        Dim denominazione As String = String.Empty
        Dim nome As String = String.Empty
        Dim cognome As String = String.Empty
        Dim codicePartitaIva As String = String.Empty
        Dim codiceFiscale As String = String.Empty
        Dim numeroCivivo As String = String.Empty
        Dim provincia As String = String.Empty
        Dim email As String = String.Empty

        Dim causali As String = String.Empty

        If Not anagrafica.Element("Denominazione") Is Nothing Then
            denominazione = anagrafica.Element("Denominazione").Value
        End If

        If Not anagrafica.Element("Nome") Is Nothing Then
            nome = anagrafica.Element("Nome").Value
        End If
        If Not anagrafica.Element("Cognome") Is Nothing Then
            cognome = anagrafica.Element("Cognome").Value
        End If

        'Il numero di identificazione fiscale ai fini IVA
        Dim fiscalePartitaIva = datiAnagrafici.Element("IdFiscaleIVA")
        If Not fiscalePartitaIva.Element("IdCodice") Is Nothing Then
            codicePartitaIva = fiscalePartitaIva.Element("IdCodice").Value
        End If

        Dim contatti = cedentePrestatore.Element("Contatti")
        If Not contatti Is Nothing Then
            If Not contatti.Element("Email") Is Nothing Then
                email = contatti.Element("Email").Value
            End If
        End If

        Dim sede = cedentePrestatore.Element("Sede")

        Dim indirizzo = sede.Element("Indirizzo").Value
        If Not sede.Element("NumeroCivico") Is Nothing Then
            numeroCivivo = sede.Element("NumeroCivico").Value
        End If

        Dim cap = sede.Element("CAP").Value
        Dim comune = sede.Element("Comune").Value
        If Not sede.Element("Provincia") Is Nothing Then
            provincia = sede.Element("Provincia").Value
        End If

        Dim oggetto As String = String.Empty
        Dim body = fattura.Elements("FatturaElettronicaBody")
        Dim i As Integer = 0
        For Each datiGenerali In body
            i += 1
            Dim datiGeneraliDocumento = datiGenerali.Element("DatiGenerali").Element("DatiGeneraliDocumento")
            Dim numerofattura = datiGeneraliDocumento.Element("Numero").Value
            Dim descrizioneTipoDocumento As String = "Fattura"
            If Not datiGeneraliDocumento.Element("TipoDocumento") Is Nothing Then
                Dim tipoDocumento = datiGeneraliDocumento.Element("TipoDocumento").Value
                Select Case tipoDocumento
                    Case "TD01"
                        descrizioneTipoDocumento = "Fattura"
                    Case "TD02"
                        descrizioneTipoDocumento = "Acconto/anticipo su fattura"
                    Case "TD03"
                        descrizioneTipoDocumento = "Acconto/anticipo su parcella"
                    Case "TD04"
                        descrizioneTipoDocumento = "Nota di credito"
                    Case "TD05"
                        descrizioneTipoDocumento = "Nota di debito"
                    Case "TD06"
                        descrizioneTipoDocumento = "Parcella"
                End Select
            End If
            oggetto &= descrizioneTipoDocumento & " n. " & numerofattura.ToString
            Dim datafattura = Date.Parse(datiGeneraliDocumento.Element("Data").Value).ToShortDateString
            oggetto &= " del " & datafattura
            If i <> body.Count Then
                oggetto &= " - "
            End If


            Dim causaliElements = datiGeneraliDocumento.Elements("Causale")
            Dim cnt As Integer = 1
            For Each causaleElement In causaliElements
                If causaliElements.Count > 1 Then
                    causali &= cnt.ToString & ") " & causaleElement.Value & " "
                    cnt += 1
                Else
                    causali &= causaleElement.Value
                End If
            Next
            causali = causali.Trim

        Next

        fatturaElettronica.NumeroFatture = body.Count

        fatturaElettronica.ComuneFornitore = comune
        fatturaElettronica.PartitaIvaFornitore = codicePartitaIva
        fatturaElettronica.CapFornitore = cap
        fatturaElettronica.IndirizzoFornitore = indirizzo

        If Not String.IsNullOrEmpty(nome) Then
            fatturaElettronica.NomeFornitore = nome
        End If

        If Not String.IsNullOrEmpty(cognome) Then
            fatturaElettronica.CognomeFornitore = cognome
        End If

        If Not String.IsNullOrEmpty(email) Then
            fatturaElettronica.EmailFornitore = email
        End If

        If Not String.IsNullOrEmpty(numeroCivivo) Then
            fatturaElettronica.NumeroCivicoFornitore = numeroCivivo
        End If

        If Not String.IsNullOrEmpty(provincia) Then
            fatturaElettronica.ProvinciaFornitore = provincia
        End If


        If Not String.IsNullOrEmpty(denominazione) Then
            fatturaElettronica.DenominazioneFornitore = denominazione
        Else
            fatturaElettronica.DenominazioneFornitore = cognome & " " & nome
        End If

        fatturaElettronica.Oggetto = oggetto

        fatturaElettronica.Causale = causali

        Return fatturaElettronica
    End Function

    'Effettua il Salvataggio su file system della conferma di ricezione
    'richiamato da GeneraConfermaRicezione
    Private Sub SalvaConfermaRicezione(ByVal path As String, confermaRicezione As Interoperabilita.ConfermaRicezione)
        Try
            'ELIMINO I NAMESPACES
            Dim ns As New System.Xml.Serialization.XmlSerializerNamespaces()
            ns.Add("", "")
            Dim serializer = New System.Xml.Serialization.XmlSerializer(GetType(Interoperabilita.ConfermaRicezione))
            Using stream = New IO.StreamWriter(path)
                Dim xw As New System.Xml.XmlTextWriter(stream)
                xw.Formatting = System.Xml.Formatting.Indented
                xw.WriteStartDocument()
                xw.WriteDocType("ConfermaRicezione", Nothing, "Segnatura.dtd", Nothing)

                serializer.Serialize(xw, confermaRicezione, ns)
                xw.WriteEndDocument()
            End Using
        Catch ex As Exception
        End Try
    End Sub

    'Tramite il messaggio di conferma di ricezione, l'amministrazione destinataria di un messaggio di protocollo 
    'comunica all'amministrazione mittente l'avvenuta protocollazione (in arrivo) del messaggio ricevuto. 
    Private Sub GeneraConfermaRicezione(ByVal registrazione As ParsecPro.Registrazione, ByVal segnatura As Interoperabilita.Segnatura, ByVal path As String)
        Dim ricevuta As New Interoperabilita.ConfermaRicezione

        Dim clienti As New ParsecAdmin.ClientRepository
        Dim cliente As ParsecAdmin.Cliente = clienti.GetFull
        Dim area = cliente.AreeOrganizzativeOmogenee.FirstOrDefault
        clienti.Dispose()

        If area Is Nothing Then
            Throw New ApplicationException("E' necessario configurare l'area organizzativa omogenea!")
            Exit Sub
        End If

        Dim codiceRegistro As String = String.Empty
        Select Case registrazione.TipologiaRegistrazione
            Case TipoRegistrazione.Arrivo
                codiceRegistro = "A"
            Case TipoRegistrazione.Interna
                codiceRegistro = "I"
            Case TipoRegistrazione.Partenza
                codiceRegistro = "P"
        End Select

        'PROTOCOLLO ARRIVO (AMMINISTRAZIONE DESTINATARIA)
        ricevuta.Identificatore = New Interoperabilita.Identificatore
        ricevuta.Identificatore.CodiceAmministrazione = cliente.CodiceAmministrazione
        ricevuta.Identificatore.CodiceAOO = area.CodiceAOO
        ricevuta.Identificatore.NumeroRegistrazione = registrazione.NumeroProtocollo.ToString.PadLeft(7, "0")
        ricevuta.Identificatore.DataRegistrazione = String.Format("{0:yyyy-MM-dd}", registrazione.DataImmissione.Value)
        ricevuta.Identificatore.CodiceRegistro = codiceRegistro


        'PROTOCOLLO PARTENZA (AMMINISTRAZIONE MITTENTE) LEGGERE LA SEGNATURA
        Dim identificatore As New Interoperabilita.Identificatore
        ricevuta.MessaggioRicevuto = New Interoperabilita.MessaggioRicevuto


        identificatore.CodiceAmministrazione = segnatura.Intestazione.Identificatore.CodiceAmministrazione
        identificatore.CodiceAOO = segnatura.Intestazione.Identificatore.CodiceAOO
        identificatore.NumeroRegistrazione = segnatura.Intestazione.Identificatore.NumeroRegistrazione
        identificatore.DataRegistrazione = segnatura.Intestazione.Identificatore.DataRegistrazione
        identificatore.CodiceRegistro = segnatura.Intestazione.Identificatore.CodiceRegistro
        ricevuta.MessaggioRicevuto.Items = {identificatore}

        Me.SalvaConfermaRicezione(path, ricevuta)
    End Sub

    'Legge la segnatura di Protocollo
    Private Function LeggiSegnatura(ByVal segnatura As ParsecPro.Interoperabilita.Segnatura, ByVal message As Rebex.Mail.MailMessage, ByVal messaggioErrore As StringBuilder) As ParsecPro.Registrazione

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Const Tls12 As System.Net.SecurityProtocolType = CType(CType(&HC00, System.Security.Authentication.SslProtocols), System.Net.SecurityProtocolType)
        Const Tls11 As System.Net.SecurityProtocolType = CType(CType(&H300, System.Security.Authentication.SslProtocols), System.Net.SecurityProtocolType)
        System.Net.ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol Or Tls12 Or Tls11 Or System.Security.Authentication.SslProtocols.Default
        System.Net.ServicePointManager.ServerCertificateValidationCallback = Function() True

        Dim registrazione As New Registrazione

        registrazione.Note = segnatura.Intestazione.Note
        Dim numeroProtocollo As String = segnatura.Intestazione.Identificatore.NumeroRegistrazione
        Dim dataRegistrazione = DateTime.Parse(segnatura.Intestazione.Identificatore.DataRegistrazione)
        registrazione.Oggetto = "Prot. N. " & numeroProtocollo & " del " & dataRegistrazione.ToShortDateString & " " & segnatura.Intestazione.Oggetto
        registrazione.ProtocolloMittente = numeroProtocollo & "/" & dataRegistrazione.Year.ToString


        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("AbilitaLetturaAllegatiSegnatura")
        parametri.Dispose()

        Dim leggiAllegati As Boolean = False
        If Not parametro Is Nothing Then
            If parametro.Valore = "1" Then
                leggiAllegati = True
            End If
        End If

        If leggiAllegati Then

            Dim numeroErrori As Integer = 0
            If Not segnatura.Descrizione Is Nothing Then
                Dim documentoPrincipale As Interoperabilita.Documento = segnatura.Descrizione.Item
                If Not documentoPrincipale Is Nothing Then
                    Dim col = documentoPrincipale.CollocazioneTelematica
                    If Not String.IsNullOrEmpty(col) Then
                        Dim wc As New Net.WebClient
                        Try
                            Dim bytes = wc.DownloadData(col)
                            Dim allegato = Me.GetAllegatoSegnatura(documentoPrincipale.nome, bytes)
                            registrazione.Allegati.Add(allegato)
                        Catch ex As Exception
                            numeroErrori += 1
                            Dim errori As New ParsecAdmin.LogErroriRepository
                            Dim errore As New ParsecAdmin.LogErrore
                            errore.Data = Now
                            errore.Descrizione = "Protocollo Segnatura - Impossibile leggere il documento principale dal seguente URL: '" & col & "'"
                            errore.IdModulo = ParsecAdmin.TipoModulo.PRO
                            errore.IdEntita = -1
                            errore.Utente = utenteCollegato.Username
                            errore.RiferimentoEntita = Me.CaselleEmailComboBox.Text & " - " & message.Subject & " del " & message.Date.OriginalTime
                            errori.Add(errore)
                            errori.SaveChanges()
                            errori.Dispose()

                        End Try
                    End If
                End If

                If Not segnatura.Descrizione.Allegati Is Nothing Then
                    Dim allegati As List(Of Interoperabilita.Documento) = segnatura.Descrizione.Allegati.Cast(Of Interoperabilita.Documento).ToList
                    For Each all In allegati
                        Dim col = all.CollocazioneTelematica
                        If Not String.IsNullOrEmpty(col) Then
                            Dim wc As New Net.WebClient
                            Try
                                Dim bytes = wc.DownloadData(col)
                                Dim allegato = Me.GetAllegatoSegnatura(all.nome, bytes)
                                registrazione.Allegati.Add(allegato)
                            Catch ex As Exception
                                numeroErrori += 1
                                Dim errori As New ParsecAdmin.LogErroriRepository
                                Dim errore As New ParsecAdmin.LogErrore
                                errore.Data = Now
                                errore.Descrizione = "Protocollo Segnatura - Impossibile leggere l'allegato dal seguente URL: '" & col & "'"
                                errore.IdModulo = ParsecAdmin.TipoModulo.PRO
                                errore.IdEntita = -1
                                errore.Utente = utenteCollegato.Username
                                errore.RiferimentoEntita = Me.CaselleEmailComboBox.Text & " - " & message.Subject & " del " & message.Date.OriginalTime
                                errori.Add(errore)
                                errori.SaveChanges()
                                errori.Dispose()
                            End Try
                        End If
                    Next
                End If
            End If

            If numeroErrori > 0 Then
                messaggioErrore.AppendFormat("Protocollo Segnatura" & vbCrLf & vbCrLf & "Si è verificato un errore durante la protocollazione della Segnatura." & vbCrLf & vbCrLf & "Per visualizzare i log degli errori è necessario selezionare la voce di menu Gestione Log Errori." & vbCrLf)
            End If

        End If

        If Not segnatura.Intestazione.Origine.Mittente.AOO Is Nothing Then
            Dim mitt = segnatura.Intestazione.Origine.Mittente
            Dim mittente As ParsecPro.Mittente = Nothing

            Dim indirizzo As String = String.Empty
            If Not String.IsNullOrEmpty(segnatura.Intestazione.Origine.IndirizzoTelematico.Text(0)) Then
                indirizzo = segnatura.Intestazione.Origine.IndirizzoTelematico.Text(0).Trim
            End If

            Dim codiceAOO As String = String.Empty
            Dim cognome As String = String.Empty


            If Not String.IsNullOrEmpty(mitt.AOO.Denominazione) Then
                cognome = mitt.AOO.Denominazione.Trim
                If Not String.IsNullOrEmpty(mitt.AOO.CodiceAOO) Then
                    codiceAOO = mitt.AOO.CodiceAOO.Trim
                End If
            Else
                If Not mitt.Amministrazione Is Nothing Then
                    cognome = mitt.Amministrazione.Denominazione.Trim
                    If Not String.IsNullOrEmpty(mitt.Amministrazione.CodiceAmministrazione) Then
                        codiceAOO = mitt.Amministrazione.CodiceAmministrazione.Trim
                    End If
                End If
            End If

            'CERCO L'ULTIMO
            Dim rubrica As New RubricaRepository

            Dim strutturaEsterna As StrutturaEsternaInfo = Nothing

            If Not String.IsNullOrEmpty(codiceAOO) Then
                Dim view = rubrica.GetQuery.Where(Function(c) c.CodiceIPA = codiceAOO And c.LogStato Is Nothing)
                If Not String.IsNullOrEmpty(cognome) Then
                    view = view.Where(Function(c) c.Denominazione = cognome)
                End If
                If Not String.IsNullOrEmpty(indirizzo) Then
                    view = view.Where(Function(c) c.Email = indirizzo)
                End If
                strutturaEsterna = view.OrderByDescending(Function(c) c.Id).FirstOrDefault
            End If

            'SE L'AOO DELL'AMMINISTRAZIONE MITTENTE E' GIA'N PRESENTE IN RUBRICA
            If Not strutturaEsterna Is Nothing Then
                mittente = New Mittente(strutturaEsterna.Id, False)

            Else

                mittente = New Mittente
                mittente.Rubrica = False
                mittente.Email = indirizzo
                mittente.Cognome = cognome
                If Not String.IsNullOrEmpty(codiceAOO) Then
                    mittente.CodiceIPA = codiceAOO
                Else
                    mittente.CodiceIPA = Nothing
                End If

                mittente.LivelloGerarchiaIPA = 3 'AOO
            End If

            registrazione.Mittenti.Add(mittente)

        End If

        If Not segnatura.Intestazione.Destinazione Is Nothing Then
            If Not segnatura.Intestazione.Destinazione(0).Destinatario Is Nothing Then
                For i As Integer = 0 To segnatura.Intestazione.Destinazione(0).Destinatario.Length - 1
                    Dim destinatario = segnatura.Intestazione.Destinazione(0).Destinatario(i).Items(0)

                    If TypeOf destinatario Is ParsecPro.Interoperabilita.Amministrazione Then
                        Dim amm = CType(destinatario, ParsecPro.Interoperabilita.Amministrazione)
                        If Not amm.Items Is Nothing Then
                            Dim uo = CType(amm.Items(0), ParsecPro.Interoperabilita.UnitaOrganizzativa)
                            If Not uo Is Nothing Then

                                Dim referenti As New ParsecAdmin.StructureRepository
                                Dim referenteInterno = referenti.Where(Function(c) c.CodiceIPA = uo.Identificativo And c.LogStato Is Nothing).FirstOrDefault
                                referenti.Dispose()
                                If Not referenteInterno Is Nothing Then
                                    Dim dest = New ParsecPro.Destinatario(referenteInterno.Id, True)
                                    registrazione.Destinatari.Add(dest)
                                End If

                            End If
                        End If

                    End If

                    If TypeOf destinatario Is ParsecPro.Interoperabilita.AOO Then
                        Dim aoo = CType(destinatario, ParsecPro.Interoperabilita.AOO)

                        Dim referenti As New ParsecAdmin.StructureRepository
                        Dim referenteInterno = referenti.Where(Function(c) c.CodiceIPA = aoo.CodiceAOO And c.LogStato Is Nothing).FirstOrDefault
                        referenti.Dispose()

                        If Not referenteInterno Is Nothing Then
                            Dim dest = New ParsecPro.Destinatario(referenteInterno.Id, True)
                            registrazione.Destinatari.Add(dest)
                        End If
                    End If
                Next
            End If

        End If

        Return registrazione
    End Function

    'Protocolla una Fattura Elettronica restuiendo true se tutto è andato a buon fine.
    Private Function ProtocollaFatturaElettronica(ByVal message As Rebex.Mail.MailMessage, ByVal mailMessage As ParsecPro.EmailArrivo) As Boolean

        Dim ms As IO.MemoryStream = Nothing
        Dim innerMessage As Rebex.Mail.MailMessage = Nothing
        Dim IdentificativoSdI As String = String.Empty
        Dim nomeFileFattura As String = String.Empty
        Dim formato As String = String.Empty
        Dim fatturaElement As XElement = Nothing
        Dim element As XElement = Nothing
        Dim fatturaTrovata As Boolean = False
        Dim metaDatitrovati As Boolean = False

        For Each att In message.Attachments
            If att.FileName.ToLower.EndsWith(".eml") Then
                innerMessage = New Rebex.Mail.MailMessage
                innerMessage.Settings.IgnoreInvalidTnefMessages = True
                ms = New IO.MemoryStream
                att.Save(ms)
                ms.Position = 0
                innerMessage.Load(ms)

                For Each s In innerMessage.Attachments
                    'SE L'ALLEGATO DELL'EMAIL E' UN FILE XML O UN FILE P7M
                    If s.FileName.ToLower.EndsWith(".xml") OrElse s.FileName.ToLower.EndsWith(".p7m") Then
                        ms = New IO.MemoryStream
                        s.Save(ms)
                        ms.Position = 0

                        If s.FileName.ToLower.EndsWith(".p7m") Then
                            Dim buffer As Byte() = ms.ToArray
                            Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms

                            'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
                            Try
                                buffer = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(buffer))
                            Catch ex As Exception
                                'NIENTE
                            End Try

                            signedCms.Decode(buffer)
                            ms = ParsecUtility.Utility.FixVersioneXml(signedCms.ContentInfo.Content)
                        Else
                            ms = ParsecUtility.Utility.FixVersioneXml(ms.ToArray)
                        End If

                        Try
                            element = XElement.Load(ms)
                        Catch ex As Exception
                            nomeFileFattura = s.FileName
                        End Try

                        If Not element Is Nothing Then
                            Dim header = element.Element("FatturaElettronicaHeader")

                            Dim identificativoSdIElement = element.Element("IdentificativoSdI")
                            Dim formatoElement = element.Element("Formato")
                            'SE E' UNA FATTURA ELETTRONICA
                            If Not header Is Nothing Then
                                fatturaTrovata = True
                                fatturaElement = element
                            End If

                            If Not identificativoSdIElement Is Nothing Then
                                If Not formatoElement Is Nothing Then
                                    metaDatitrovati = True
                                    IdentificativoSdI = identificativoSdIElement.Value
                                    formato = formatoElement.Value
                                End If
                            End If

                        End If
                    End If

                    If fatturaTrovata And metaDatitrovati Then
                        Exit For
                    End If
                Next
            End If
            If fatturaTrovata And metaDatitrovati Then
                Exit For
            End If
        Next

        If fatturaTrovata And metaDatitrovati Then

            If Not Me.IterAttivato Then
                ParsecUtility.Utility.MessageBox("Per protocollare la fattura selezionata è necessario abilitare l'iter del protocollo!", False)
                Return True
            End If

            Dim registrazione As ParsecPro.Registrazione = Nothing
            registrazione = Me.LeggiFattura(fatturaElement)
            Dim allegatoEmail = Me.GetAllegatoEmail(mailMessage)
            registrazione.Allegati.Add(allegatoEmail)
            registrazione.DataOraRicezioneInvio = mailMessage.DataArrivo

            registrazione.TipologiaRegistrazione = TipoRegistrazione.Arrivo

            Dim fatture As New ParsecPro.FatturaElettronicaRepository
            Dim messaggi As New ParsecPro.MessaggioSdIRepository
            Dim fatturaElettronica As ParsecPro.FatturaElettronica = fatture.GetQuery.Where(Function(c) c.MessaggioSdI.IdEmail = mailMessage.Id).FirstOrDefault
            Me.VisualizzaProtocollo(registrazione, fatturaElettronica, "Insert", True)
            Return True
        ElseIf metaDatitrovati And Not fatturaTrovata Then

            If Not IterAttivato Then
                ParsecUtility.Utility.MessageBox("Per protocollare la fattura selezionata è necessario abilitare l'iter del protocollo!", False)
                Return True
            End If

            Dim registrazione As New ParsecPro.Registrazione
            registrazione.Oggetto = "ERRORE DURANTE LA LETTURA DELLA FATTURA " & nomeFileFattura
            Dim allegatoEmail = Me.GetAllegatoEmail(mailMessage)
            registrazione.Allegati.Add(allegatoEmail)
            registrazione.TipologiaRegistrazione = TipoRegistrazione.Arrivo

            Dim fatture As New ParsecPro.FatturaElettronicaRepository
            Dim messaggi As New ParsecPro.MessaggioSdIRepository
            Dim fatturaElettronica As ParsecPro.FatturaElettronica = fatture.GetQuery.Where(Function(c) c.MessaggioSdI.IdEmail = mailMessage.Id).FirstOrDefault
            Me.VisualizzaProtocollo(registrazione, fatturaElettronica, "Insert", True)
            Return True
        End If

        Return False
    End Function

    'Protocolla una Notifica Inviata dallo SDI. Restituisce true se non ci sono stati problemi.
    Private Function ProtocollaNotificaInviataDaSdI(ByVal message As Rebex.Mail.MailMessage, ByVal mailMessage As ParsecPro.EmailArrivo) As Boolean
        Dim nomefileFattura As String = String.Empty
        Dim ms As IO.MemoryStream = Nothing
        Dim innerMessage As Rebex.Mail.MailMessage = Nothing
        For Each att In message.Attachments
            If att.FileName.ToLower.EndsWith(".eml") Then

                innerMessage = New Rebex.Mail.MailMessage
                innerMessage.Settings.IgnoreInvalidTnefMessages = True
                ms = New IO.MemoryStream
                att.Save(ms)
                ms.Position = 0
                innerMessage.Load(ms)

                Dim attMs As IO.MemoryStream = Nothing
                For Each innerAtt In innerMessage.Attachments
                    'TODO ALTRI TIPI DI NOTIFICA
                    'NOTIFICA DI ESITO

                    Dim codiceNotifica = Regex.Match(innerAtt.FileName, "_(?<codiceNotifica>[A-Z]{2})_", RegexOptions.IgnoreCase).Groups("codiceNotifica").Value

                    If Not String.IsNullOrEmpty(codiceNotifica) Then

                        If codiceNotifica <> "NS" AndAlso codiceNotifica <> "MC" AndAlso codiceNotifica <> "NE" AndAlso codiceNotifica <> "DT" AndAlso codiceNotifica <> "RC" AndAlso codiceNotifica <> "SE" Then
                            Dim token = innerAtt.FileName.Split("_")
                            codiceNotifica = token.Reverse.Skip(1).Take(1).FirstOrDefault()
                        End If

                    End If

                    If Not String.IsNullOrEmpty(codiceNotifica) Then

                        attMs = New IO.MemoryStream
                        innerAtt.Save(attMs)
                        attMs.Position = 0
                        Dim element = XElement.Load(attMs)
                        Dim identificativoSdI = element.Elements("IdentificativoSdI").FirstOrDefault.Value

                        Dim pos = innerAtt.FileName.IndexOf(".")
                        Dim ext = innerAtt.FileName.Substring(pos, innerAtt.FileName.Length - pos)

                        Dim pos2 = innerAtt.FileName.IndexOf("_" & codiceNotifica)

                        nomefileFattura = innerAtt.FileName.Substring(0, pos2)

                        Dim key As String = String.Empty

                        Select Case codiceNotifica
                            Case "NS"
                                key = "Notifica di scarto"
                            Case "MC"
                                key = "Notifica di mancata consegna"
                            Case "NE"
                                key = "Notifica di esito cedente/prestatore"
                            Case "DT"
                                key = "Notifica di decorrenza termini"
                            Case "RC"
                                key = "Ricevuta di consegna"
                            Case "SE"
                                key = "Scarto esito"

                        End Select
                        If Not String.IsNullOrEmpty(key) Then

                            Dim registrazioni As New ParsecPro.RegistrazioniRepository
                            Dim idRegistrazione = Me.GetIdRegistrazione(identificativoSdI, nomefileFattura)
                            If idRegistrazione.HasValue Then
                                Dim registrazione = registrazioni.GetById(idRegistrazione.Value)
                                Dim nomefile As String = Guid.NewGuid.ToString & ".eml"
                                Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
                                Dim fullPathEmail As String = mailBoxPath & mailMessage.PercorsoRelativo & mailMessage.NomeFileEml
                                Dim allegato As New ParsecPro.Allegato
                                allegato.NomeFile = nomefile
                                allegato.NomeFileTemp = nomefile
                                allegato.IdTipologiaDocumento = 0 'Allegato
                                allegato.DescrizioneTipologiaDocumento = "Allegato"
                                allegato.Oggetto = key
                                allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                                allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
                                allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(fullPathEmail)
                                allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
                                registrazione.Allegati.Add(allegato)

                                'Copio l'allegato nella cartella temporanea.
                                Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
                                IO.File.Copy(fullPathEmail, pathDownload)

                                Me.VisualizzaProtocollo(registrazione, "Update", True)

                                Return True
                            Else
                                Dim fatture As New FatturaElettronicaRepository
                                Dim fattura = fatture.Where(Function(c) c.MessaggioSdI.Nomefile.Contains(nomefileFattura) And c.IdentificativoSdI = identificativoSdI).FirstOrDefault
                                fatture.Dispose()
                                Throw New ApplicationException("Impossibile protocollare l'email selezionata." & vbCrLf & "E' necessario prima protocollare l'email avente il seguente oggetto:" & vbCrLf & " FATTURA - Invio File " & fattura.IdentificativoSdI)
                                Return False

                            End If
                        End If
                        Return False

                    End If

                Next

            End If
        Next
        Return False
    End Function

    'Protocolla la Segnatura. Restituisce true se non ci sono stati problemi.
    Private Function ProtocollaSegnatura(ByVal message As Rebex.Mail.MailMessage, ByVal mailMessage As ParsecPro.EmailArrivo) As Boolean
        Me.Segnatura = Nothing

        Dim innerMessage As Rebex.Mail.MailMessage = Nothing
        Dim segnaturaAttachment As Rebex.Mail.Attachment = Nothing
        Dim tipoEmail As ParsecPro.Pop3.Header.TipologiaEmail = CType(mailMessage.Tipo, ParsecPro.Pop3.Header.TipologiaEmail)

        If tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC Then
            For Each att In message.Attachments
                If att.FileName.ToLower.EndsWith(".eml") Then
                    innerMessage = New Rebex.Mail.MailMessage
                    innerMessage.Settings.IgnoreInvalidTnefMessages = True
                    Using ms = New IO.MemoryStream
                        att.Save(ms)
                        ms.Position = 0
                        innerMessage.Load(ms)
                        segnaturaAttachment = innerMessage.Attachments.Where(Function(c) c.FileName.Contains("Segnatura") AndAlso c.FileName.ToLower.EndsWith(".xml")).FirstOrDefault
                        If Not segnaturaAttachment Is Nothing Then
                            Exit For
                        End If
                    End Using

                End If
            Next
        Else
            segnaturaAttachment = message.Attachments.Where(Function(c) c.FileName.Contains("Segnatura") AndAlso c.FileName.ToLower.EndsWith(".xml")).FirstOrDefault
        End If

        If Not segnaturaAttachment Is Nothing Then
            Using ms = New IO.MemoryStream
                segnaturaAttachment.Save(ms)
                ms.Position = 0
                Try

                    Dim registrazione As ParsecPro.Registrazione = Nothing
                    Dim segnatura As Interoperabilita.Segnatura = Nothing

                    Using strm = Me.FixSegnaturaXml(ms)
                        segnatura = Me.DeserializeSegnatura(strm)
                    End Using

                    Dim messaggioErrore As New StringBuilder
                    If Not innerMessage Is Nothing Then
                        registrazione = Me.LeggiSegnatura(segnatura, innerMessage, messaggioErrore)
                    Else
                        registrazione = Me.LeggiSegnatura(segnatura, message, messaggioErrore)
                    End If

                    If messaggioErrore.Length > 0 Then
                        ParsecUtility.Utility.MessageBox(messaggioErrore.ToString, False)
                    End If


                    Dim allegatoEmail = Me.GetAllegatoEmail(mailMessage)
                    registrazione.Allegati.Insert(0, allegatoEmail)

                    registrazione.TipologiaRegistrazione = TipoRegistrazione.Arrivo

                    registrazione.DataOraRicezioneInvio = mailMessage.DataArrivo

                    Me.VisualizzaProtocollo(registrazione, "Insert")
                    Me.Segnatura = segnatura

                    Return True


                Catch ex As Exception
                    Return False
                End Try

            End Using

        End If
        Return False
    End Function

    'Permette di visualizzare in Modifica una registrazione di protocollo. Richiama la pagina RegistrazioneArrivoPage.aspx.
    Private Sub VisualizzaProtocolloInModifica(ByVal registrazione As ParsecPro.Registrazione)

        Dim pageUrl As String = "~/UI/Protocollo/pages/user/RegistrazioneArrivoPage.aspx"

        Dim queryString As New Hashtable
        queryString.Add("Mode", "Update")
        queryString.Add("Tipo", registrazione.TipoRegistrazione)
        queryString.Add("Procedura", "3")
        queryString.Add("obj", Me.AggiornaEmailSenzaInvioNotificaButton.ClientID)

        registrazione.IdEmail = Me.Email.Id

        Dim parametriPagina As New Hashtable
        parametriPagina.Add("RegistrazioneEmail", registrazione)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 940, 650, queryString, False)

    End Sub

    'Protocolla la mail
    Private Sub ProtocollaEmail(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim emails As New ParsecPro.EmailArrivoRepository
        Dim emailBloccata As ParsecPro.EmailArrivo = emails.GetLock(id)
        Dim rimastaBloccata As Boolean = False

        Dim nomefile As String = Guid.NewGuid.ToString & ".eml"

        If Not emailBloccata Is Nothing Then
            If emailBloccata.IdUtenteLock <> utenteCollegato.Id Then
                ParsecUtility.Utility.MessageBox("L'email - '" & emailBloccata.Oggetto & "' -" & vbCrLf & " non è protocollabile perchè bloccata dall'utente '" & emailBloccata.UtenteBlocco & "'!", False)
                emailBloccata = Nothing
                emails.Dispose()
                Exit Sub
            Else
                rimastaBloccata = True
            End If
        End If

        Dim emailProtocollata As ParsecPro.EmailArrivo = emails.GetProtocollata(id)
        If Not emailProtocollata Is Nothing Then
            ParsecUtility.Utility.MessageBox("L'email - '" & emailProtocollata.Oggetto & "' -" & vbCrLf & " è stata protocollata dall'utente '" & emailProtocollata.UtenteProtocollazione & "'!", False)

            emailProtocollata = Nothing
            emails.Dispose()
            'Leggo le email dal database.
            Me.Emails = Nothing
            Me.EmailGridView.Rebind()
            Exit Sub
        End If

        'Se non è già bloccata dall'utente collegato.
        If Not rimastaBloccata Then
            emails.SetLock(id, utenteCollegato.Id)
        End If

        Dim mailMessage As ParsecPro.EmailArrivo = Me.Emails.Where(Function(c) c.Id = id).FirstOrDefault
        If Not mailMessage Is Nothing Then


            'VERIFICO LA PRESENZA DI UNA EMAIL PROTOCOLLATA CHE ABBIA IL MESSAGE-ID UGUALE A QUELLO DELL'EMAIL SELEZIONATA
            If Not String.IsNullOrEmpty(mailMessage.IdEmail) Then
                Dim emailInviataPiuCaselleProtocollata = emails.Where(Function(c) c.IdEmail = mailMessage.IdEmail And c.IdRegistrazione.HasValue).FirstOrDefault
                If Not emailInviataPiuCaselleProtocollata Is Nothing Then
                    Dim registrazioni As New ParsecPro.RegistrazioniRepository
                    Dim reg = registrazioni.GetById(emailInviataPiuCaselleProtocollata.IdRegistrazione)
                    registrazioni.Dispose()

                    If Not reg Is Nothing Then
                        Me.Email = mailMessage
                        ParsecUtility.Utility.MessageBox("L'email selezionata è stata già protocollata con il seguente numero '" & reg.NumeroProtocollo.ToString.PadLeft(7, "0") & "/" & reg.DataImmissione.Value.Year.ToString & "'!", False)
                        Me.VisualizzaProtocolloInModifica(reg)
                    End If

                    Exit Sub
                End If
            End If

            Me.Email = mailMessage
            Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
            Dim fullPathEmail As String = mailBoxPath & mailMessage.PercorsoRelativo & mailMessage.NomeFileEml
            If Not IO.File.Exists(fullPathEmail) Then
                ParsecUtility.Utility.MessageBox("L'e-mail selezionato non esiste!", False)
                emails.SetLock(id, Nothing)
                Exit Sub
            End If

            Try

                Dim message As New Rebex.Mail.MailMessage
                message.Settings.IgnoreInvalidTnefMessages = True
                message.Load(fullPathEmail)
                Dim tipoEmail As ParsecPro.Pop3.Header.TipologiaEmail = CType(mailMessage.Tipo, ParsecPro.Pop3.Header.TipologiaEmail)


                If tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_Normale OrElse tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC Then

                    '**********************************************************************************************************************************************
                    'GESTIONE FATTURA ELETTRONICA
                    '**********************************************************************************************************************************************

                    '*****************************************************************************************************************************************
                    'ANALIZZO LA FATTURA ELETTRONICA SOLO SE E' STATA INVIATA TRAMITE PEC E L'INDIRIZZO EMAIL DEL MITTENTE CORRISPONDE A QUELLO
                    'CONFIGURATO NEL SISTEMA COME INDIRIZZO DEL SISTEMA DI INTERSCAMBIO
                    '*****************************************************************************************************************************************
                    If Me.VerificaMittenteFatturaElettronica(mailMessage.Mittente) Then

                        If Me.ProtocollaFatturaElettronica(message, mailMessage) Then
                            Exit Sub
                        End If

                        Try
                            If Me.ProtocollaNotificaInviataDaSdI(message, mailMessage) Then
                                Exit Sub
                            End If
                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)

                            If Not utenteCollegato.SuperUser Then
                                Exit Sub
                            Else

                                Dim allegato As New ParsecPro.Allegato
                                'Copio l'allegato nella cartella temporanea.
                                Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
                                IO.File.Copy(fullPathEmail, pathDownload)

                                Select Case tipoEmail
                                    Case ParsecPro.Pop3.Header.TipologiaEmail.Email_Normale
                                        allegato.Oggetto = "Messaggio e-mail"
                                    Case ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC
                                        Dim parametri As New ParsecAdmin.ParametriRepository
                                        Dim parametro = parametri.GetByName("PEC_DescrizioneMailIn", ParsecAdmin.TipoModulo.PRO)
                                        parametri.Dispose()
                                        If Not parametro Is Nothing Then
                                            allegato.Oggetto = parametro.Valore
                                        Else
                                            allegato.Oggetto = "Messaggio originale da PEC"
                                        End If
                                    Case ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC_Anomalia
                                        allegato.Oggetto = "Messaggio PEC con anomalia"
                                End Select


                                allegato.NomeFile = nomefile
                                allegato.NomeFileTemp = nomefile
                                allegato.IdTipologiaDocumento = 0 'Secondario
                                allegato.DescrizioneTipologiaDocumento = "Allegato"
                                allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                                allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
                                allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                                allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
                                Me.RicercaProtocollo(allegato)
                                Exit Sub
                            End If

                        End Try

                    End If

                    '**********************************************************************************************************************************************
                    'GESTIONE SEGNATURA.XML
                    '**********************************************************************************************************************************************

                    If Me.ProtocollaSegnatura(message, mailMessage) Then
                        Exit Sub
                    End If

                End If

                '***********************************************************************************************
                'Se l'email non è una ricevuta apro la registrazione in modalità INSERIMENTO.
                '***********************************************************************************************
                If (tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_Normale Or tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC Or tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC_Anomalia) Then
                    'Aggiungo il mittente.
                    Dim mittente As ParsecPro.Mittente = Nothing
                    Dim indirizzoMittente As String = mailMessage.Mittente
                    Dim denominazione As String = indirizzoMittente
                    denominazione = Replace(denominazione, "<", "-")
                    denominazione = Replace(denominazione, ">", "-")
                    Dim r As Regex = New Regex("\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase Or RegexOptions.Multiline)
                    Dim indirizzo As String = r.Match(indirizzoMittente).Groups(0).Value
                    Dim rubrica As New RubricaRepository

                    'CERCO L'ULTIMO
                    Dim strutturaEsterna = rubrica.Where(Function(c) c.Email = indirizzo And c.LogStato Is Nothing And c.Denominazione = denominazione).OrderByDescending(Function(c) c.Id).FirstOrDefault
                    Dim registrazione As New ParsecPro.Registrazione

                    If Not strutturaEsterna Is Nothing Then
                        mittente = New ParsecPro.Mittente
                        mittente.Interno = False
                        mittente.Rubrica = strutturaEsterna.InRubrica
                        mittente.PerConoscenza = False
                        mittente.Esistente = True
                        mittente.Id = strutturaEsterna.Id
                        mittente.Codice = strutturaEsterna.Codice
                        mittente.Cognome = strutturaEsterna.Denominazione
                        mittente.Descrizione = (If(strutturaEsterna.Denominazione = Nothing, "", strutturaEsterna.Denominazione) + " " + If(strutturaEsterna.Nome = Nothing, "", strutturaEsterna.Nome) + ", " + If(strutturaEsterna.Indirizzo = Nothing, "", strutturaEsterna.Indirizzo) + ", " + If(strutturaEsterna.CAP = Nothing, "", strutturaEsterna.CAP) + " " + If(strutturaEsterna.Comune = Nothing, "", strutturaEsterna.Comune) + " (" + If(strutturaEsterna.Provincia = Nothing, "", strutturaEsterna.Provincia) + ")" + If(strutturaEsterna.Email = Nothing, "", " | " & strutturaEsterna.Email))
                        mittente.Nome = strutturaEsterna.Nome
                        mittente.Indirizzo = strutturaEsterna.Indirizzo
                        mittente.Cap = strutturaEsterna.CAP
                        mittente.Citta = strutturaEsterna.Comune
                        mittente.Provincia = strutturaEsterna.Provincia
                        mittente.CodiceFiscalePartitaIva = strutturaEsterna.CodiceFiscale
                        mittente.Telefono = strutturaEsterna.Telefono
                        mittente.Fax = strutturaEsterna.Fax
                        mittente.Email = strutturaEsterna.Email

                        registrazione.Mittenti.Add(mittente)
                    Else
                        mittente = New ParsecPro.Mittente
                        mittente.Rubrica = False
                        mittente.Email = indirizzo
                        mittente.Cognome = denominazione
                        registrazione.Mittenti.Add(mittente)
                    End If
                    rubrica.Dispose()

                    registrazione.Oggetto = mailMessage.Oggetto
                    registrazione.DataOraRicezioneInvio = mailMessage.DataArrivo
                    Dim parametri As New ParsecAdmin.ParametriRepository
                    Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("PEC_IncludereMailIn", ParsecAdmin.TipoModulo.PRO)
                    parametri.Dispose()
                    Dim includiEmail As Boolean = False

                    If Not parametro Is Nothing Then
                        If parametro.Valore = 1 Then
                            includiEmail = True
                        End If
                    End If


                    If includiEmail OrElse tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_Normale Then
                        Dim allegatoEmail = Me.GetAllegatoEmail(mailMessage)
                        registrazione.Allegati.Add(allegatoEmail)
                    End If

                    registrazione.TipologiaRegistrazione = TipoRegistrazione.Arrivo
                    Me.VisualizzaProtocollo(registrazione, "Insert")
                Else
                    '***********************************************************************************************
                    'Se l'email  è una ricevuta apro la registrazione in modalità MODIFICA.
                    '***********************************************************************************************
                    '****************************************************************************************************
                    'Allego l'e-mail
                    '****************************************************************************************************
                    Dim allegato As New ParsecPro.Allegato
                    'Copio l'allegato nella cartella temporanea.
                    Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & nomefile
                    IO.File.Copy(fullPathEmail, pathDownload)
                    allegato.Oggetto = Me.GetOggettoAllegato(tipoEmail)
                    allegato.NomeFile = nomefile
                    allegato.NomeFileTemp = nomefile
                    allegato.IdTipologiaDocumento = 0 'Secondario
                    allegato.DescrizioneTipologiaDocumento = "Allegato"
                    allegato.PercorsoRoot = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                    allegato.PercorsoRootTemp = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp")
                    allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
                    allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

                    'Se l'oggetto dell'email contiene Prot. N. (è stata inviata dal sistema) o l'e-mail è una ricevuta.
                    'Cerco di trovare il protocollo associato.
                    If (mailMessage.Oggetto.Contains("Prot. N.") OrElse (mailMessage.Oggetto.Contains("Protocollazione N°"))) And mailMessage.Tipo <> 0 Then
                        Dim numeroProtocollo As Integer = 0
                        Dim annoProtocollo As Integer = 0
                        Dim reNumero As New Regex("(?<Numero>[\d]+)")
                        Dim reData As New Regex("(?<Data>[\d]{2}/[\d]{2}/[\d]{4})")
                        Dim m As Match = reNumero.Match(mailMessage.Oggetto)
                        If m.Success Then
                            numeroProtocollo = CInt(m.Groups("Numero").Value)
                        End If
                        m = reData.Match(mailMessage.Oggetto)
                        If m.Success Then
                            annoProtocollo = CDate(m.Groups("Data").Value).Year
                        End If
                        If numeroProtocollo <> 0 AndAlso annoProtocollo <> 0 Then
                            Dim registrazioni As New ParsecPro.RegistrazioniRepository
                            Dim protocollo As ParsecPro.Registrazione = registrazioni.GetRegistrazioneByTipoRegistro(numeroProtocollo, annoProtocollo, ParsecPro.TipoRegistro.Generale)
                            If Not protocollo Is Nothing Then
                                protocollo.Allegati.Add(allegato)
                                VisualizzaProtocollo(protocollo, "Update")
                            Else
                                Me.RicercaProtocollo(allegato)
                            End If
                            registrazioni.Dispose()
                        Else
                            Me.RicercaProtocollo(allegato)
                        End If
                    Else
                        Me.RicercaProtocollo(allegato)
                    End If
                End If
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try
        End If
    End Sub

    'Effettua il Download del file selezinato dalla griglia
    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        Try
            If Not Me.CasellaPec Is Nothing Then
                Dim mailMessage As ParsecPro.EmailArrivo = Me.Emails.Where(Function(c) c.Id = id).FirstOrDefault

                If Not mailMessage Is Nothing Then
                    If mailMessage.Lavorata Then
                        Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
                        Dim parametriPagina As New Hashtable
                        parametriPagina.Add("Filtro", mailMessage.IdRegistrazione)
                        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                        ParsecUtility.Utility.ShowPopup(pageUrl, 940, 510, Nothing, False)
                    Else
                        Dim fullPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata") & mailMessage.PercorsoRelativo & mailMessage.NomeFileEml
                        Dim file As New IO.FileInfo(fullPath)
                        If file.Exists Then
                            Try
                                Me.VisualizzaEmailControl.ShowPanel()
                                Me.VisualizzaEmailControl.InitUI(file.FullName, id, Nothing)
                            Catch ex As Exception
                                ParsecUtility.Utility.MessageBox("Si è verificato un errore durante la lettura dell'email!" & vbCrLf & "Il file verrà aperto con l'applicazione associata.", False)
                                'IN CASO DI ERRORE VISUALIZZO IL FILE NON MODO CLASSICO
                                Session("AttachmentFullName") = file.FullName
                                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                                ParsecUtility.Utility.PageReload(pageUrl, False)
                            End Try


                            If Not mailMessage.Letta Then
                                Dim emails As New ParsecPro.EmailArrivoRepository
                                Dim email = emails.Where(Function(c) c.Id = mailMessage.Id).FirstOrDefault
                                If Not email Is Nothing Then
                                    email.Letta = True
                                    emails.SaveChanges()
                                    'Leggo le email dal database.
                                    Me.Emails = Nothing
                                    Me.EmailGridView.Rebind()
                                End If
                                emails.Dispose()
                            End If

                        Else
                            ParsecUtility.Utility.MessageBox("L'e-mail selezionata non esiste!", False)
                        End If

                    End If
                End If

            End If
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    'Crea una lista degli id selezionati nella griglia EmailGridView
    Private Sub SaveSelectedItems()
        For Each item As GridItem In Me.EmailGridView.Items
            If TypeOf item Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(item, GridDataItem)
                Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
                ' Dim id As Integer = CInt(dataItem("Id").Text)
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

    'Decodifica la tipologia di Mail restituendone la descrizione.
    Private Function GetOggettoAllegato(ByVal tipoEmail As ParsecPro.Pop3.Header.TipologiaEmail) As String
        Dim res As String = ""
        Select Case tipoEmail
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_Normale
                res = "E-mail"
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC
                res = "PEC"
            Case ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC_Anomalia
                res = "Anomalia"
            Case ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_Accettazione
                res = "Ricevuta Accettazione"
            Case ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_AvvenutaConsegna
                res = "Ricevuta Avvenuta Consegna"
            Case ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_ErroreConsegna
                res = "Ricevuta Errore Consegna"
            Case ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_NonAccettazione
                res = "Ricevuta Non Accettazione"
            Case ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_PreavvisoErroreConsegna
                res = "Ricevuta Preavviso Errore Consegna"
            Case ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_PresaInCarico
                res = "Ricevuta Presa In Carico"
            Case ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_RilevazioneVirus
                res = "Ricevuta Rilevazione Virus"
            Case ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_Sconosciuta
                res = "Ricevuta Sconosciuta"
        End Select
        Return res
    End Function

    'Ricerca un protocollo richiamando l'opportuna maschera di ricerca (RicercaRegistrazionePage.aspx)
    Private Sub RicercaProtocollo(ByVal allegato As ParsecPro.Allegato)
        Session("Allegato") = allegato
        Dim pageUrl As String = "~/UI/Protocollo/pages/search/RicercaRegistrazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.VisualizzaRegistrazioneImageButton.ClientID)
        queryString.Add("modalita", "ricerca")
        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 600, queryString, False)
    End Sub

    'Visualizza i dettagli di un protocollo richiamando la maschera RegistrazioneArrivoPage.aspx.
    Private Sub VisualizzaProtocollo(ByVal protocollo As ParsecPro.Registrazione, ByVal mode As String)
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/RegistrazioneArrivoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Mode", mode)
        If mode.ToLower = "insert" Then
            queryString.Add("Tipo", "0")
        Else
            queryString.Add("Tipo", "1")
        End If
        queryString.Add("obj", Me.AggiornaEmailButton.ClientID)
        protocollo.IdEmail = Me.Email.Id
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("RegistrazioneEmail", protocollo)

        Dim destinatarioPec As Boolean = False

        If Me.CasellaPec.Pop3IsSSL.HasValue Then
            destinatarioPec = Me.CasellaPec.Pop3IsSSL
        End If

        parametriPagina.Add("IsPec", (Me.Email.Tipo = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC) AndAlso destinatarioPec)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 620, queryString, False)
    End Sub


    'Visualizza i dettagli di un protocollo nel caso di fattura richiamando la maschera RegistrazioneArrivoPage.aspx
    Private Sub VisualizzaProtocollo(ByVal protocollo As ParsecPro.Registrazione, ByVal mode As String, ByVal fattura As Boolean)
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/RegistrazioneArrivoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Mode", mode)
        If mode.ToLower = "insert" Then
            queryString.Add("Tipo", "0")
        Else
            queryString.Add("Tipo", "1")
        End If
        If fattura Then
            queryString.Add("Fattura", "1")
        End If
        queryString.Add("obj", Me.AggiornaEmailButton.ClientID)
        protocollo.IdEmail = Me.Email.Id
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("RegistrazioneEmail", protocollo)

        Dim destinatarioPec As Boolean = False

        If Me.CasellaPec.Pop3IsSSL.HasValue Then
            destinatarioPec = Me.CasellaPec.Pop3IsSSL
        End If

        parametriPagina.Add("IsPec", (Me.Email.Tipo = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC) AndAlso destinatarioPec)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 620, queryString, False)
    End Sub

    'Visualizza i dettagli di un protocollo nel caso di fattura richiamando la maschera RegistrazioneArrivoPage.aspx
    Private Sub VisualizzaProtocollo(ByVal protocollo As ParsecPro.Registrazione, ByVal fatturaElettronica As ParsecPro.FatturaElettronica, ByVal mode As String, fattura As Boolean)
        Dim pageUrl As String = "~/UI/Protocollo/pages/user/RegistrazioneArrivoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Mode", mode)
        If mode.ToLower = "insert" Then
            queryString.Add("Tipo", "0")
        Else
            queryString.Add("Tipo", "1")
        End If
        If fattura Then
            queryString.Add("Fattura", "1")
        End If
        queryString.Add("obj", Me.AggiornaEmailButton.ClientID)
        Dim parametriPagina As New Hashtable
        protocollo.IdEmail = Me.Email.Id
        parametriPagina.Add("RegistrazioneEmail", protocollo)
        parametriPagina.Add("FatturaElettronica", fatturaElettronica)

        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 620, queryString, False)
    End Sub

    'Metodo associato alla griglia delle mail per la gestione dei check
    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedItems()
    End Sub

    'Metodo associato alla griglia delle mail per la gestione dei check
    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In EmailGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
        Me.SaveSelectedItems()
    End Sub

    'resetta il filtro di ricerca
    Private Sub ResettaFiltro()
        Me.OggettoTextBox.Text = String.Empty
        Me.DataInvioInizioTextBox.SelectedDate = Nothing
        Me.DataInvioFineTextBox.SelectedDate = Nothing
        For Each r As ListItem In Me.TipologieCheckBoxList.Items
            r.Selected = True
        Next
        Me.StatoComboBox.SelectedIndex = 2
        Me.StatoLetturaComboBox.SelectedIndex = 0
        If Me.CaselleEmailComboBox.SelectedIndex > -1 Then
            'Leggo le email dal database.
            Me.Emails = Nothing
        Else
            Me.Emails = New List(Of ParsecPro.EmailArrivo)
        End If
        If Not Me.Page.IsPostBack Then
            Me.ImpostaTipologia()
        End If
        Me.MittentiComboBox.Text = ""
        Me.MittentiComboBox.SelectedValue = ""
        Me.EmailGridView.Rebind()
    End Sub

    'Restituisce il filtro di ricerca i base ai campi compialti sulla maschera.
    Private Function GetFiltro() As ParsecPro.EmailArrivoFiltro
        Dim filtro As New ParsecPro.EmailArrivoFiltro
        If Me.CaselleEmailComboBox.SelectedIndex > -1 Then
            filtro.IdCasella = CInt(Me.CaselleEmailComboBox.SelectedValue)
        End If

        If Me.MittentiComboBox.SelectedValue <> String.Empty Then
            filtro.Mittente = (New EmailArrivoRepository).GetQuery.Where(Function(c) c.Id = CInt(Me.MittentiComboBox.SelectedValue)).FirstOrDefault.Mittente
        Else
            filtro.Mittente = Me.MittentiComboBox.Text
        End If

        If Me.StatoComboBox.SelectedIndex <> 0 Then
            filtro.Protocollata = Me.StatoComboBox.SelectedIndex = 1
            filtro.Cancellata = Me.StatoComboBox.SelectedIndex = 3 'False

        Else
            filtro.Cancellata = False
        End If

        If Me.StatoLetturaComboBox.SelectedIndex <> 0 Then
            filtro.Letta = Me.StatoLetturaComboBox.SelectedIndex = 1
        End If

        For Each r As ListItem In Me.TipologieCheckBoxList.Items
            If r.Selected = True Then
                If r.Value <> "8" Then
                    filtro.Tipologie.Add(r.Value)
                Else
                    filtro.FatturaElettronica = True
                End If
            End If
        Next

        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            filtro.Oggetto = Me.OggettoTextBox.Text
        End If

        If Not Me.DataInvioInizioTextBox.SelectedDate Is Nothing Then
            filtro.DataInizio = Me.DataInvioInizioTextBox.SelectedDate
        End If

        If Not Me.DataInvioFineTextBox.SelectedDate Is Nothing Then
            filtro.DataFine = Me.DataInvioFineTextBox.SelectedDate
        End If

        Return filtro
    End Function

    'Scrive il Log su file
    Private Sub WriteLog(ex As Exception)
        Dim logFilePath As String = ParsecAdmin.WebConfigSettings.GetKey("LogFilePath")
        Dim w As IO.StreamWriter = IO.File.AppendText(logFilePath & "Log.txt")
        w.WriteLine(New String("*", 20))
        w.WriteLine("Eccezione del {0} alle ore {1}", DateTime.Now.ToLongDateString, DateTime.Now.ToLongTimeString)
        w.WriteLine(ex.Message)
        If Not ex.InnerException Is Nothing Then
            w.WriteLine("Eccezione Interna")
            w.WriteLine(ex.InnerException.Message)
        End If
        w.WriteLine(New String("*", 20))
        w.Flush()
        w.Close()
    End Sub

    'Gestisce il caso di una notifica mandata dallo sdi. Richiamato da CollegaNotificheSdiAlProtocolloFattura e da RiceviEmail
    Private Function NotificaInviataDaSdI(ByVal email As Rebex.Mail.MailMessage) As Dictionary(Of String, Dictionary(Of String, String))
        Dim res As New Dictionary(Of String, Dictionary(Of String, String))
        Dim kv As Dictionary(Of String, String) = Nothing
        Dim nomefileFattura As String = String.Empty
        Dim ms As IO.MemoryStream = Nothing

        email.Settings.IgnoreInvalidTnefMessages = True

        Dim innerMessage As Rebex.Mail.MailMessage = Nothing
        For Each att In email.Attachments
            If att.FileName.ToLower.EndsWith(".eml") Then

                innerMessage = New Rebex.Mail.MailMessage
                innerMessage.Settings.IgnoreInvalidTnefMessages = True
                ms = New IO.MemoryStream
                att.Save(ms)
                ms.Position = 0
                innerMessage.Load(ms)

                Dim attMs As IO.MemoryStream = Nothing

                For Each innerAtt In innerMessage.Attachments

                    'NOTIFICA DI ESITO

                    Dim codiceNotifica = Regex.Match(innerAtt.FileName, "_(?<codiceNotifica>[A-Z]{2})_", RegexOptions.IgnoreCase).Groups("codiceNotifica").Value

                    If Not String.IsNullOrEmpty(codiceNotifica) Then
                        If codiceNotifica <> "NS" AndAlso codiceNotifica <> "MC" AndAlso codiceNotifica <> "NE" AndAlso codiceNotifica <> "DT" AndAlso codiceNotifica <> "RC" AndAlso codiceNotifica <> "SE" Then
                            Dim token = innerAtt.FileName.Split("_")
                            codiceNotifica = token.Reverse.Skip(1).Take(1).FirstOrDefault()
                        End If
                    End If

                    If Not String.IsNullOrEmpty(codiceNotifica) Then

                        attMs = New IO.MemoryStream
                        innerAtt.Save(attMs)
                        attMs.Position = 0
                        Dim element = XElement.Load(attMs)
                        Dim identificativoSdI = element.Elements("IdentificativoSdI").FirstOrDefault.Value

                        Dim pos = innerAtt.FileName.IndexOf(".")
                        Dim ext = innerAtt.FileName.Substring(pos, innerAtt.FileName.Length - pos)

                        Dim pos2 = innerAtt.FileName.IndexOf("_" & codiceNotifica)

                        nomefileFattura = innerAtt.FileName.Substring(0, pos2)

                        Dim key As String = String.Empty

                        Select Case codiceNotifica
                            Case "NS"
                                key = "Notifica di scarto"
                            Case "MC"
                                key = "Notifica di mancata consegna"
                            Case "NE"
                                key = "Notifica di esito cedente/prestatore"
                            Case "DT"
                                key = "Notifica di decorrenza termini"
                            Case "RC"
                                key = "Ricevuta di consegna"
                            Case "SE"
                                key = "Scarto esito"

                        End Select
                        If Not String.IsNullOrEmpty(key) Then
                            kv = New Dictionary(Of String, String)
                            kv.Add(identificativoSdI, nomefileFattura)
                            res.Add(key, kv)
                        End If

                        Return res

                    End If

                Next

            End If
        Next
        Return res
    End Function

    'Salva la Fattura Elettronica se la mail contiene una fattura.
    Private Sub SalvaFatturaElettronica(ByVal emailArrivo As ParsecPro.EmailArrivo, ByVal email As Rebex.Mail.MailMessage)

        email.Settings.IgnoreInvalidTnefMessages = True

        Dim ms As IO.MemoryStream = Nothing
        Dim innerMessage As Rebex.Mail.MailMessage = Nothing
        Dim segnaturaAttachment As Rebex.Mail.Attachment = Nothing

        Dim IdentificativoSdI As String = String.Empty
        Dim nomeFileFattura As String = String.Empty
        Dim nomeFileMetadatiFattura As String = String.Empty
        Dim formato As String = String.Empty

        Dim fatturaElement As XElement = Nothing
        Dim element As XElement = Nothing

        Dim fatturaTrovata As Boolean = False
        Dim metaDatiTrovati As Boolean = False

        Dim codiceIpaDestinatario As String = String.Empty

        Dim fatturaAttachment As Rebex.Mail.Attachment = Nothing
        For Each att In email.Attachments

            If att.FileName.ToLower.EndsWith(".eml") Then
                innerMessage = New Rebex.Mail.MailMessage
                innerMessage.Settings.IgnoreInvalidTnefMessages = True
                ms = New IO.MemoryStream
                att.Save(ms)
                ms.Position = 0
                innerMessage.Load(ms)

                For Each s In innerMessage.Attachments
                    '***********************************************************************************************
                    'SE L'ALLEGATO DELL'EMAIL E' UN FILE XML O UN FILE P7M
                    '***********************************************************************************************
                    If s.FileName.ToLower.EndsWith(".xml") OrElse s.FileName.ToLower.EndsWith(".p7m") Then
                        ms = New IO.MemoryStream
                        s.Save(ms)
                        ms.Position = 0

                        If s.FileName.ToLower.EndsWith(".p7m") Then
                            Dim buffer As Byte() = ms.ToArray
                            Dim signedCms As New System.Security.Cryptography.Pkcs.SignedCms
                            'SE IL CONTENUTO DEL FILE P7M E' CODIFICATO IN BASE64 LO DECODIFICO
                            Try
                                buffer = System.Convert.FromBase64String(System.Text.ASCIIEncoding.Default.GetString(buffer))
                            Catch ex As Exception
                                'NIENTE
                            End Try

                            signedCms.Decode(buffer)

                            ms = ParsecUtility.Utility.FixVersioneXml(signedCms.ContentInfo.Content)

                        Else
                            ms = ParsecUtility.Utility.FixVersioneXml(ms.ToArray)

                        End If

                        Try
                            element = XElement.Load(ms)
                        Catch ex As Exception
                            Dim pathTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & s.FileName
                            s.Save(pathTemp)
                            nomeFileFattura = s.FileName
                        End Try

                        If Not element Is Nothing Then
                            Dim header = element.Element("FatturaElettronicaHeader")
                            Dim identificativoSdIElement = element.Element("IdentificativoSdI")
                            Dim formatoElement = element.Element("Formato")

                            '***********************************************************************************************
                            'SE E' UNA FATTURA ELETTRONICA
                            '***********************************************************************************************
                            If Not header Is Nothing Then
                                fatturaTrovata = True
                                fatturaElement = element
                                '***********************************************************************************************
                                'SALVO LA FATTURA XML NELLA CARTELLA TEMPORANEA
                                '***********************************************************************************************

                                fatturaAttachment = s

                                codiceIpaDestinatario = header.Element("DatiTrasmissione").Element("CodiceDestinatario").Value
                            End If

                            If Not identificativoSdIElement Is Nothing Then
                                If Not formatoElement Is Nothing Then
                                    metaDatiTrovati = True
                                    '***********************************************************************************************
                                    'SALVO I METADATI DELLA FATTURA XML NELLA CARTELLA TEMPORANEA
                                    '***********************************************************************************************
                                    IdentificativoSdI = identificativoSdIElement.Value
                                    nomeFileMetadatiFattura = IdentificativoSdI & "_" & s.FileName
                                    Dim pathTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & nomeFileMetadatiFattura
                                    s.Save(pathTemp)

                                    formato = formatoElement.Value
                                End If

                            End If
                        End If

                    End If
                    If fatturaTrovata AndAlso metaDatiTrovati Then
                        Exit For
                    End If
                Next
            End If
            If fatturaTrovata AndAlso metaDatiTrovati Then
                Exit For
            End If
        Next

        nomeFileFattura = IdentificativoSdI & "_" & fatturaAttachment.FileName
        fatturaAttachment.Save(ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & nomeFileFattura)

        If fatturaTrovata AndAlso metaDatiTrovati Then

            Dim fatturaElettronica As ParsecPro.FatturaElettronica = Me.CaricaFattura(fatturaElement)

            fatturaElettronica.IdStato = ParsecPro.StatoFattura.Ricevuta
            fatturaElettronica.CodiceFornitore = Nothing
            fatturaElettronica.IdentificativoSdI = IdentificativoSdI
            fatturaElettronica.VersioneFattura = formato
            fatturaElettronica.NumeroProtocollo = Nothing
            fatturaElettronica.AnnoProtocollo = Nothing

            fatturaElettronica.MessaggioSdI = New ParsecPro.MessaggioSdI
            fatturaElettronica.MessaggioSdI.Nomefile = nomeFileFattura
            fatturaElettronica.MessaggioSdI.PercorsoRelativo = Nothing
            fatturaElettronica.MessaggioSdI.IdEmail = emailArrivo.Id
            fatturaElettronica.MessaggioSdI.DataRicezioneInvio = emailArrivo.DataArrivo
            fatturaElettronica.MessaggioSdI.Direzione = ParsecPro.DirezioneMessaggioSdI.Ricevuto
            fatturaElettronica.NomeFileMetadati = nomeFileMetadatiFattura

            If Not String.IsNullOrEmpty(codiceIpaDestinatario) Then
                fatturaElettronica.CodiceIpaDestinatario = codiceIpaDestinatario
            End If

            '*******************************************************************************************************************
            'RICERCO IL DESTINATARIO PER CIG
            '*******************************************************************************************************************
            Dim referenti As New ParsecAdmin.StructureRepository
            Dim referenteInterno As ParsecAdmin.Struttura = Nothing

            If Not String.IsNullOrEmpty(fatturaElettronica.ElencoCig) Then
                Dim elencoCig = fatturaElettronica.ElencoCig.Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries).ToList

                If elencoCig.Count = 1 Then
                    Dim cig = elencoCig.FirstOrDefault

                    Dim impegni As New ParsecAtt.ImpegnoSpesaRepository


                    Dim atti As New ParsecAtt.DocumentoRepository(impegni.Context)

                    Dim impegniConCig = (From impegno In impegni.GetQuery
                               Join atto In atti.GetQuery
                               On impegno.IdDocumento Equals atto.Id
                               Where atto.LogStato Is Nothing And impegno.CIG = cig And atto.IdTipologiaDocumento = 2
                               Select impegno).ToList

                    impegni.Dispose()

                    If impegniConCig.Count = 1 Then

                        Dim documenti As New ParsecAtt.DocumentoRepository
                        Dim idDocumento = impegniConCig.FirstOrDefault.IdDocumento
                        Dim documento = documenti.Where(Function(c) c.Id = idDocumento).FirstOrDefault
                        documenti.Dispose()

                        If Not documento Is Nothing Then
                            referenteInterno = referenti.Where(Function(c) c.Id = documento.IdStruttura And (c.LogStato Is Nothing Or c.LogStato = "M")).FirstOrDefault
                            If Not referenteInterno Is Nothing Then
                                If referenteInterno.LogStato = "M" Then
                                    referenteInterno = referenti.Where(Function(c) c.Codice = referenteInterno.Codice And c.LogStato Is Nothing).FirstOrDefault
                                    If Not referenteInterno Is Nothing Then
                                        fatturaElettronica.DenominazioneDestinatario = referenteInterno.Descrizione
                                    End If
                                Else
                                    fatturaElettronica.DenominazioneDestinatario = referenteInterno.Descrizione
                                End If
                            End If
                        End If
                    End If

                End If
            End If
            '*******************************************************************************************************************
            'RICERCO IL DESTINATARIO PER CODICDE IPA
            '*******************************************************************************************************************
            If String.IsNullOrEmpty(fatturaElettronica.DenominazioneDestinatario) Then
                If Not String.IsNullOrEmpty(codiceIpaDestinatario) Then
                    referenteInterno = referenti.Where(Function(c) c.CodiceIPA = codiceIpaDestinatario And c.LogStato Is Nothing).FirstOrDefault
                    If Not referenteInterno Is Nothing Then
                        fatturaElettronica.DenominazioneDestinatario = referenteInterno.Descrizione
                    End If
                End If
            End If

            referenti.Dispose()



            Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
            fattureElettroniche.Save(fatturaElettronica)

            emailArrivo.Oggetto = "FATTURA - " & emailArrivo.Oggetto

        ElseIf metaDatiTrovati AndAlso Not fatturaTrovata Then

            Dim fatturaElettronica As New ParsecPro.FatturaElettronica
            fatturaElettronica.NumeroFatture = 0
            fatturaElettronica.ComuneFornitore = ""
            fatturaElettronica.PartitaIvaFornitore = ""
            fatturaElettronica.DenominazioneFornitore = ""
            fatturaElettronica.Oggetto = "ERRORE DURANTE LA LETTURA DELLA FATTURA"

            fatturaElettronica.IdStato = ParsecPro.StatoFattura.Ricevuta
            fatturaElettronica.CodiceFornitore = Nothing
            fatturaElettronica.IdentificativoSdI = IdentificativoSdI
            fatturaElettronica.VersioneFattura = formato
            fatturaElettronica.NumeroProtocollo = Nothing
            fatturaElettronica.AnnoProtocollo = Nothing

            fatturaElettronica.MessaggioSdI = New ParsecPro.MessaggioSdI
            fatturaElettronica.MessaggioSdI.Nomefile = nomeFileFattura
            fatturaElettronica.MessaggioSdI.PercorsoRelativo = Nothing
            fatturaElettronica.MessaggioSdI.IdEmail = emailArrivo.Id
            fatturaElettronica.MessaggioSdI.DataRicezioneInvio = emailArrivo.DataArrivo
            fatturaElettronica.MessaggioSdI.Direzione = ParsecPro.DirezioneMessaggioSdI.Ricevuto
            fatturaElettronica.NomeFileMetadati = nomeFileMetadatiFattura

            Dim fattureElettroniche As New ParsecPro.FatturaElettronicaRepository
            fattureElettroniche.Save(fatturaElettronica)

            emailArrivo.Oggetto = "FATTURA - " & emailArrivo.Oggetto

        End If

    End Sub

    'Verifica il mittente della Fattura Elettronica
    Private Function VerificaMittenteFatturaElettronica(ByVal email As String) As Boolean
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("DominioServizioSdI", ParsecAdmin.TipoModulo.PRO)
        parametri.Dispose()
        If Not parametro Is Nothing Then
            Dim atPos = email.IndexOf("@")
            Dim domainPart = email.Substring(atPos + 1)
            If domainPart = parametro.Valore Then
                'SE L'EMAIL PROVIENE DAL SISTEMA DI INTERSCAMBIO
                Return True
            End If
        End If
        Return False
    End Function

    'Legge il parametro di configurazione NotificaEmailProtocollazione
    'Se 0= Non invia l`email di avvenuta protocollazione 1= Invia l`email di avvenuta protocollazione
    Private Function NotificaEmailProtocollazione() As Boolean
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("NotificaEmailProtocollazione", ParsecAdmin.TipoModulo.PRO)
        parametri.Dispose()
        If Not parametro Is Nothing Then
            If parametro.Valore = "1" Then
                Return True
            End If
        End If
        Return False
    End Function

    'Restituisce l'id di registrazione a cui appartiene l'allegato della fattura elettronica
    Private Function GetIdRegistrazione(ByVal identificativoSdI As String, ByVal nomefile As String) As Nullable(Of Integer)
        Dim registrazioni As New ParsecPro.RegistrazioniRepository

        'FATTURE PASSIVE
        Dim fatture As New ParsecPro.FatturaElettronicaRepository

        Dim fattura = fatture.Where(Function(c) c.IdentificativoSdI = identificativoSdI And c.MessaggioSdI.Nomefile.Contains(nomefile) And c.IdRegistrazione.HasValue).FirstOrDefault


        fatture.Dispose()

        If Not fattura Is Nothing Then
            Dim protocollo = registrazioni.Where(Function(c) c.NumeroProtocollo = fattura.NumeroProtocollo And c.DataImmissione.Value.Year = fattura.AnnoProtocollo And c.Modificato = False).FirstOrDefault
            If Not protocollo Is Nothing Then
                registrazioni.Dispose()
                Return protocollo.Id
            End If

        End If


        'FATTURE ATTIVE
        Dim allegati As New ParsecPro.AllegatiRepository(registrazioni.Context)
        Dim documenti As New ParsecPro.DocumentoViewRepository(registrazioni.Context)


        Dim view = (From allegato In allegati.GetQuery
                                 Join documento In documenti.GetQuery
                                 On allegato.Id Equals documento.Id
                                  Join reg In registrazioni.GetQuery
                                  On reg.Id Equals allegato.IdRegistrazione
                                 Where documento.NomeFileOriginale.Contains(nomefile) And reg.Modificato = False
                                 Select allegato, documento).Select(Function(c) c.allegato.IdRegistrazione)


        If view.Any Then
            Return view.FirstOrDefault
        End If


        Return Nothing

    End Function


    'Restiuisce true o false se la mail proviene dal Sistema di Interscambio
    Private Function EmailProvieneSistemaInterscambio(email As String, parametro As String) As Boolean
        If Not String.IsNullOrEmpty(parametro) Then
            Dim atPos = email.IndexOf("@")
            Dim domainPart = email.Substring(atPos + 1)
            If domainPart = parametro Then
                'SE L'EMAIL PROVIENE DAL SISTEMA DI INTERSCAMBIO
                Return True
            End If
        End If
        Return False
    End Function

    'Collega le notifiche dello SDI al Protocollo della fattura
    Private Sub CollegaNotificheSdiAlProtocolloFattura()

        Try

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName("DominioServizioSdI", ParsecAdmin.TipoModulo.PRO)
            parametri.Dispose()

            Dim valoreParametro As String = String.Empty
            If Not parametro Is Nothing Then
                valoreParametro = parametro.Valore
            End If

            Dim emailsArrivo As New ParsecPro.EmailArrivoRepository

            '************************************************************************************************
            'PEC NON PROTOCOLLATE E NON CANCELLATE E PROVIENTI DAL SISTEMA DI INTERSCAMBIO
            '************************************************************************************************
            Dim emailPecNonProtocollate = emailsArrivo.Where(Function(c) Not c.IdRegistrazione.HasValue And c.Tipo = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC And c.IdCasella = Me.CaselleEmailComboBox.SelectedValue And c.Cancellata = False).ToList
            emailPecNonProtocollate = emailPecNonProtocollate.Where(Function(c) EmailProvieneSistemaInterscambio(c.Mittente, valoreParametro)).ToList

            Dim count As Integer = emailPecNonProtocollate.Count

            If count = 0 Then
                ParsecUtility.Utility.MessageBox("Nessuna notifica SdI da collegare!", False)
                Exit Sub
            End If

            Dim context = RadProgressContext.Current
            context.PrimaryTotal = count.ToString

            Dim i As Integer = 0
            Dim sb As New StringBuilder
            Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
            Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
            Dim registrazioni As New ParsecPro.RegistrazioniRepository


            For Each emailPecNonProtocollata In emailPecNonProtocollate

                i += 1

                context.PrimaryValue = i.ToString
                context.PrimaryPercent = CInt(((i / count) * 100)).ToString
                If Not Response.IsClientConnected Then
                    Exit For
                End If

                '************************************************************************************************
                'SE L'EMAIL PROVIENE DAL SISTEMA DI INTERSCAMBIO
                '************************************************************************************************

                Dim fullPathEmail As String = mailBoxPath & emailPecNonProtocollata.PercorsoRelativo & emailPecNonProtocollata.NomeFileEml

                If IO.File.Exists(fullPathEmail) Then
                    Dim mail As New Rebex.Mail.MailMessage
                    Dim buffer = IO.File.ReadAllBytes(fullPathEmail)
                    mail.Load(buffer)

                    '************************************************************************************************
                    'SE L'EMAIL E' UNA NOTIFICA PROVENIENTE DAL SISTEMA DI INTERSCAMBIO
                    '************************************************************************************************
                    Dim dict = Me.NotificaInviataDaSdI(mail)

                    If dict.Count > 0 Then

                        Dim nomefileFattura As String = dict.FirstOrDefault.Value.FirstOrDefault.Value
                        Dim identificativoSdI = dict.FirstOrDefault.Value.FirstOrDefault.Key

                        If Not String.IsNullOrEmpty(nomefileFattura) Then

                            Dim idRegistrazione = Me.GetIdRegistrazione(identificativoSdI, nomefileFattura)


                            If idRegistrazione.HasValue Then

                                Dim registrazione = registrazioni.GetById(idRegistrazione.Value)
                                Dim nomefile As String = Guid.NewGuid.ToString & ".eml"

                                Dim allegato As New ParsecPro.Allegato
                                allegato.NomeFile = nomefile
                                allegato.NomeFileTemp = nomefile
                                allegato.IdTipologiaDocumento = 0 'Allegato
                                allegato.DescrizioneTipologiaDocumento = "Allegato"
                                allegato.Oggetto = dict.FirstOrDefault.Key
                                allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                                allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
                                allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(fullPathEmail)
                                allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

                                registrazioni.SaveAllegato(allegato, registrazione)

                                Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                                percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                                Dim destPathDownload As String = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & nomefile


                                IO.File.Copy(fullPathEmail, destPathDownload, True)

                                emailsArrivo.SetProtocollata(emailPecNonProtocollata.Id, utenteCollegato.Id, registrazione.Id)
                                sb.Append("<span style='color:#00156E'>La " & allegato.Oggetto & " è stata aggiunta al protocollo n. " & registrazione.NumeroProtocollo.ToString & " del " & registrazione.DataImmissione.Value.ToShortDateString & "</span><br/>")


                            End If

                        End If


                    End If

                End If

            Next

            context.OperationComplete = True

            If sb.Length > 0 Then
                Me.infoOperazioneHidden.Value = sb.ToString
                Me.MessagePanel.Controls.Clear()
                Me.MessagePanel.Controls.Add(New LiteralControl(sb.ToString))
            Else
                ParsecUtility.Utility.MessageBox("Nessuna notifica SdI da collegare!", False)
            End If

            Me.Emails = emailsArrivo.GetView(Me.GetFiltro)
            Me.EmailGridView.Rebind()

            emailsArrivo.Dispose()
            registrazioni.Dispose()

        Catch ex As Exception
            If ex.InnerException Is Nothing Then
                Throw New ApplicationException(ex.Message)
            Else
                Throw New ApplicationException(ex.InnerException.Message)
            End If

        End Try

    End Sub

    'Richiamato dall'evento RiceviEmailImageButton.Click per scaricare le mail 
    Private Sub RiceviEmail()

        Dim sb As New StringBuilder

        Dim elencoRicevutePec As New Dictionary(Of Rebex.Mail.MailMessage, ParsecPro.EmailArrivo)

        Dim emailNuove As Integer = 0
        Dim idCasella As Integer = CInt(Me.CaselleEmailComboBox.SelectedValue)

        If idCasella = -1 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare una casella di posta elettronica!", False)
            Exit Sub
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim locks As New LockCasellaEmailRepository
        Dim lock As ParsecPro.LockCasellaEmail = locks.GetLock(idCasella)
        Dim bloccato As Boolean = False

        'Se la casella è bloccata dall'utente collegato.
        If Not lock Is Nothing Then
            If lock.IdUtente <> utenteCollegato.Id Then
                ParsecUtility.Utility.MessageBox("La casella '" & Me.CaselleEmailComboBox.SelectedItem.Text & "' è bloccata dall'utente '" & lock.Utente & "'!", False)
                locks.Dispose()
                Exit Sub
            Else
                bloccato = True
            End If
        End If

        'Se la casella di posta non è bloccata
        If Not bloccato Then
            'Blocco la casella di posta
            lock = locks.CreateFromInstance(Nothing)
            lock.IdEmail = idCasella
            lock.IdUtente = utenteCollegato.Id
            lock.Inizio = Now
            lock.Fine = Now
            lock.Stato = True
            locks.Save(lock)
        End If

        Try
            Dim casellePec As New ParsecAdmin.ParametriPecRepository
            Me.CasellaPec = casellePec.GetByIdAssociatoUtente(idCasella)

            If Not Me.CasellaPec Is Nothing Then

                'Valore predefinito
                Dim conservaCopiaServer As Boolean = True
                If Me.CasellaPec.MantieniCopiaServer.HasValue Then
                    conservaCopiaServer = Me.CasellaPec.MantieniCopiaServer
                End If

                Dim client As New ParsecPro.Pop3Client
                Try
                    Dim password As String = ParsecCommon.CryptoUtil.Decrypt(Me.CasellaPec.Password)
                    client.Connect(Me.CasellaPec.Pop3Server, Me.CasellaPec.Pop3Porta, Me.CasellaPec.Pop3IsSSL)
                    client.Login(Me.CasellaPec.UserId, password)
                Catch ex As Exception
                    ParsecUtility.Utility.MessageBox(ex.Message, False)
                    Exit Sub
                End Try

                Dim count = client.GetMessageCount
                If count = 0 Then
                    ParsecUtility.Utility.MessageBox("La casella '" & Me.CaselleEmailComboBox.SelectedItem.Text & "' è vuota!", False)
                    'Sblocco la casella di posta
                    lock = locks.CreateFromInstance(locks.LockCasellaEmail)
                    lock.Fine = Now
                    lock.Stato = False
                    locks.Save(lock)
                    locks.Dispose()
                    Exit Sub
                End If


                Dim percorsoRelativo As String = String.Format("\{0}\", Now.Year)
                Dim emailsArrivo As New ParsecPro.EmailArrivoRepository
                Dim path As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")

                If Not IO.Directory.Exists(path) Then
                    IO.Directory.CreateDirectory(path)
                End If

                If Not IO.Directory.Exists(path & percorsoRelativo) Then
                    IO.Directory.CreateDirectory(path & percorsoRelativo)
                End If

                If IO.Directory.Exists(path) Then
                    IO.Directory.CreateDirectory(path)
                End If

                Dim context = RadProgressContext.Current
                context.PrimaryTotal = count.ToString
                Dim i As Integer = 0

                Dim registrazioni As New ParsecPro.RegistrazioniRepository

                For j As Integer = 0 To count - 1
                    i += 1

                    context.PrimaryValue = i.ToString
                    context.PrimaryPercent = CInt(((i / count) * 100)).ToString
                    If Not Response.IsClientConnected Then
                        Exit For
                    End If

                    Dim uid = client.UidlMessage(j + 1)

                    Dim esiste As Boolean = Not emailsArrivo.GetQuery.Where(Function(c) c.IdCasella = idCasella And c.IdentificativoUnivoco = uid).FirstOrDefault Is Nothing

                    If Not esiste Then

                        Dim email As New Rebex.Mail.MailMessage
                        Dim rawData As Byte() = client.GetMessageAsByte(j + 1)

                        Try

                            'CALCOLO L'HASH 
                            Dim improntaEsadecimale As String = BitConverter.ToString(ParsecUtility.Utility.CalcolaHash(rawData)).Replace("-", "") & ".eml"

                            Dim emailDaVerificare = emailsArrivo.GetQuery.Where(Function(c) c.IdCasella = idCasella And (c.NomeFileEml = improntaEsadecimale Or c.NomeFileEml = "Prot_" & improntaEsadecimale)).FirstOrDefault

                            esiste = Not emailDaVerificare Is Nothing

                            If Not esiste Then

                                email.Load(rawData)

                                emailNuove += 1

                                Dim pathEmail As String = path & percorsoRelativo & improntaEsadecimale

                                Try
                                    IO.File.WriteAllBytes(pathEmail, rawData)
                                Catch ex As Exception
                                    Me.WriteLog(ex)
                                End Try

                                Dim messageId As String = String.Empty
                                If Not email.MessageId Is Nothing Then
                                    messageId = email.MessageId.Id
                                End If

                                Dim emailArrivo As ParsecPro.EmailArrivo = emailsArrivo.CreateFromInstance(Nothing)
                                emailArrivo.IdCasella = idCasella
                                emailArrivo.Ricevuta = True
                                emailArrivo.IdUtente = utenteCollegato.Id
                                emailArrivo.IdEmail = messageId
                                emailArrivo.IdentificativoUnivoco = uid

                                Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
                                Dim emailMatches As MatchCollection = emailRegex.Matches(email.From.FirstOrDefault.DisplayName)
                                For Each emailMatch As Match In emailMatches
                                    emailArrivo.Mittente = emailMatch.Value
                                    Exit For
                                Next

                                If String.IsNullOrEmpty(emailArrivo.Mittente) Then
                                    emailArrivo.Mittente = email.From.FirstOrDefault.Address
                                End If

                                If String.IsNullOrEmpty(emailArrivo.Mittente) Then
                                    emailArrivo.Mittente = email.Sender.Address
                                End If

                                If String.IsNullOrEmpty(emailArrivo.Mittente) Then
                                    emailArrivo.Mittente = "email@nontrovata.it"
                                End If

                                emailArrivo.Oggetto = email.Subject
                                emailArrivo.Tipo = CInt(Me.GetTipoEmail(email))
                                emailArrivo.DataArrivo = email.Date.OriginalTime
                                emailArrivo.NomeFileEml = improntaEsadecimale
                                emailArrivo.PercorsoRelativo = String.Format("\{0}\", Now.Year)
                                emailArrivo.DataRegistrazione = Now

                                emailArrivo.Letta = False

                                emailsArrivo.Add(emailArrivo)
                                emailsArrivo.SaveChanges()

                                '*****************************************************************************************************************************************
                                'ANALIZZO E SALVO LA FATTURA ELETTRONICA SOLO SE E' STATA INVIATA TRAMITE PEC E L'INDIRIZZO EMAIL DEL MITTENTE CORRISPONDE A QUELLO
                                'CONFIGURATO NEL SISTEMA COME INDIRIZZO DEL SISTEMA DI INTERSCAMBIO
                                '*****************************************************************************************************************************************
                                Dim tipoEmail As ParsecPro.Pop3.Header.TipologiaEmail = CType(emailArrivo.Tipo, ParsecPro.Pop3.Header.TipologiaEmail)


                                'MEMORIZZO TUTTE LE RICEVUTE PEC
                                If tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_Accettazione OrElse tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_AvvenutaConsegna Then
                                    elencoRicevutePec.Add(email, emailArrivo)
                                End If


                                If tipoEmail = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC AndAlso Me.VerificaMittenteFatturaElettronica(emailArrivo.Mittente) Then

                                    'SE L'EMAIL PROVIENE DAL SISTEMA DI INTERSCAMBIO

                                    'SE L'EMAIL E' UNA NOTIFICA PROVENIENTE DAL SISTEMA DI INTERSCAMBIO

                                    Dim dict = Me.NotificaInviataDaSdI(email)
                                    If dict.Count > 0 Then


                                        Dim nomefileFattura As String = dict.FirstOrDefault.Value.FirstOrDefault.Value
                                        Dim identificativoSdI = dict.FirstOrDefault.Value.FirstOrDefault.Key

                                        If Not String.IsNullOrEmpty(nomefileFattura) Then

                                            Dim idRegistrazione = Me.GetIdRegistrazione(identificativoSdI, nomefileFattura)


                                            If idRegistrazione.HasValue Then

                                                Dim registrazione = registrazioni.GetById(idRegistrazione.Value)
                                                Dim nomefile As String = Guid.NewGuid.ToString & ".eml"
                                                Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
                                                Dim fullPathEmail As String = mailBoxPath & emailArrivo.PercorsoRelativo & emailArrivo.NomeFileEml
                                                Dim allegato As New ParsecPro.Allegato
                                                allegato.NomeFile = nomefile
                                                allegato.NomeFileTemp = nomefile
                                                allegato.IdTipologiaDocumento = 0 'Allegato
                                                allegato.DescrizioneTipologiaDocumento = "Allegato"
                                                allegato.Oggetto = dict.FirstOrDefault.Key
                                                allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                                                allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
                                                allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(fullPathEmail)
                                                allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

                                                registrazioni.SaveAllegato(allegato, registrazione)

                                                Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                                                percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                                                Dim destPathDownload As String = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & nomefile


                                                IO.File.Copy(fullPathEmail, destPathDownload, True)

                                                emailsArrivo.SetProtocollata(emailArrivo.Id, utenteCollegato.Id, registrazione.Id)
                                                sb.Append("<span style='color:#00156E'>La " & allegato.Oggetto & " è stata aggiunta al protocollo n. " & registrazione.NumeroProtocollo.ToString & " del " & registrazione.DataImmissione.Value.ToShortDateString & "</span><br/>")
                                            End If

                                        End If

                                    Else
                                        Me.SalvaFatturaElettronica(emailArrivo, email)
                                    End If

                                    emailsArrivo.SaveChanges()
                                End If

                            Else
                                'SE E' STATA TROVATA TRAMITE L'HASH SIGNIFICA CHE L'UID NON E' STATO MAI REGISTRATO PER L'EMAIL CORRENTE

                                If String.IsNullOrEmpty(emailDaVerificare.IdentificativoUnivoco) Then
                                    emailDaVerificare.IdentificativoUnivoco = uid
                                    emailsArrivo.SaveChanges()
                                End If

                            End If

                            If Not conservaCopiaServer Then
                                client.DeleteMessage(j + 1)
                            End If

                        Catch ex As Exception
                            Me.WriteLog(ex)
                        End Try

                    End If

                Next


                Dim emailInviate As New ParsecPro.EmailRepository

                registrazioni = New ParsecPro.RegistrazioniRepository

                'CERCO DI AGGANCIARE LE RICEVUTE AL PROTOCOLLO
                For Each ricevutaPec In elencoRicevutePec

                    For Each att In ricevutaPec.Key.Attachments
                        If att.FileName.ToLower = "daticert.xml" Then

                            Dim ms As New IO.MemoryStream
                            att.Save(ms)
                            ms.Position = 0
                            Dim el As XElement = XElement.Load(ms)
                            Dim msgId As String = el.Descendants("msgid").Value

                            If Not String.IsNullOrEmpty(msgId) Then
                                msgId = msgId.Replace("<", "").Replace(">", "").Trim
                                Dim emailInviata = emailInviate.Where(Function(c) c.MessaggioId = msgId).FirstOrDefault

                                If Not emailInviata Is Nothing Then
                                    Dim numeroProtocollo = emailInviata.NumeroProtocollo
                                    Dim annoProtocollo = emailInviata.AnnoProtocollo
                                    Dim tipoProtocollo = emailInviata.TipoProtocollo

                                    Dim registrazione = registrazioni.Where(Function(c) c.NumeroProtocollo = numeroProtocollo And Year(c.DataImmissione) = annoProtocollo And c.Modificato = False And c.Annullato = False And c.TipoRegistrazione = tipoProtocollo).FirstOrDefault
                                    If Not registrazione Is Nothing Then


                                        Dim nomefile As String = Guid.NewGuid.ToString & ".eml"
                                        Dim mailBoxPath As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
                                        Dim fullPathEmail As String = mailBoxPath & ricevutaPec.Value.PercorsoRelativo & ricevutaPec.Value.NomeFileEml

                                        Dim allegato As New ParsecPro.Allegato
                                        allegato.NomeFile = nomefile
                                        allegato.NomeFileTemp = nomefile
                                        allegato.IdTipologiaDocumento = 0 'Allegato
                                        allegato.DescrizioneTipologiaDocumento = "Allegato"

                                        Dim tipoEmail As ParsecPro.Pop3.Header.TipologiaEmail = CType(ricevutaPec.Value.Tipo, ParsecPro.Pop3.Header.TipologiaEmail)

                                        allegato.Oggetto = Me.GetOggettoAllegato(tipoEmail)

                                        allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                                        allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
                                        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(fullPathEmail)
                                        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")

                                        registrazioni.SaveAllegato(allegato, registrazione)

                                        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
                                        percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                                        Dim destPathDownload As String = percorsoRoot & allegato.PercorsoRelativo & allegato.Id.ToString.PadLeft(9, "0") & "_" & "1".PadLeft(4, "0") & "_" & nomefile

                                        IO.File.Copy(fullPathEmail, destPathDownload, True)

                                        emailsArrivo.SetProtocollata(ricevutaPec.Value.Id, utenteCollegato.Id, registrazione.Id)

                                        sb.Append("<span style='color:#00156E'>La " & allegato.Oggetto & " è stata aggiunta al protocollo n. " & registrazione.NumeroProtocollo.ToString & " del " & registrazione.DataImmissione.Value.ToShortDateString & "</span><br/>")

                                    End If

                                End If

                            End If

                        End If

                    Next
                Next

                registrazioni.Dispose()

                Me.Emails = emailsArrivo.GetView(Me.GetFiltro)
                Me.EmailGridView.Rebind()
                emailsArrivo.Dispose()
                client.Disconnect()
                context.OperationComplete = True

            Else
                Me.Emails = Nothing
                Me.EmailGridView.Rebind()
            End If

            casellePec.Dispose()

        Catch ex As Exception
            Dim innerEx As String = String.Empty
            If Not ex.InnerException Is Nothing Then
                innerEx = "Eccezione Interna: " & vbCrLf & ex.InnerException.Message
            End If
            ParsecUtility.Utility.MessageBox(ex.Message & innerEx, False)
        End Try
        'Sblocco la casella di posta
        lock = locks.CreateFromInstance(locks.LockCasellaEmail)

        If Not lock Is Nothing Then
            lock.Fine = Now
            lock.Stato = False
            locks.Save(lock)
        End If

        locks.Dispose()


        If emailNuove = 0 Then
            ParsecUtility.Utility.MessageBox("Non ci sono nuovi messaggi", False)
        Else
            If sb.Length > 0 Then
                Me.infoOperazioneHidden.Value = sb.ToString
                Me.MessagePanel.Controls.Clear()
                Me.MessagePanel.Controls.Add(New LiteralControl(sb.ToString))
            End If
        End If

    End Sub

    'Restituisce le tipologie di email che si possono ricevere
    Private Function GetTipoEmail(ByVal email As Rebex.Mail.MailMessage) As ParsecPro.Pop3.Header.TipologiaEmail
        Dim res As ParsecPro.Pop3.Header.TipologiaEmail
        Dim mimeHeaderRicevuta As Rebex.Mime.MimeHeader = email.Headers.Where(Function(c) c.Name = "X-Ricevuta").FirstOrDefault
        If Not mimeHeaderRicevuta Is Nothing Then
            If mimeHeaderRicevuta.Raw.Contains("non-accettazione") Then
                res = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_NonAccettazione
            ElseIf mimeHeaderRicevuta.Raw.Contains("accettazione") Then
                res = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_Accettazione
            ElseIf mimeHeaderRicevuta.Raw.Contains("avvenuta-consegna") Then
                res = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_AvvenutaConsegna
            ElseIf mimeHeaderRicevuta.Raw.Contains("preavviso-errore-consegna") Then
                res = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_PreavvisoErroreConsegna
            ElseIf mimeHeaderRicevuta.Raw.Contains("errore-consegna") Then
                res = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_ErroreConsegna
            ElseIf mimeHeaderRicevuta.Raw.Contains("presa-in-carico") Then
                res = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_PresaInCarico
            ElseIf mimeHeaderRicevuta.Raw.Contains("rilevazione-virus") Then
                res = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_RilevazioneVirus
            Else
                res = ParsecPro.Pop3.Header.TipologiaEmail.PEC_Ricevuta_Sconosciuta
            End If
        Else
            Dim mimeHeaderTrasporto As Rebex.Mime.MimeHeader = email.Headers.Where(Function(c) c.Name = "X-Trasporto").FirstOrDefault
            If Not mimeHeaderTrasporto Is Nothing Then
                If mimeHeaderTrasporto.Raw.Contains("posta-certificata") Then
                    res = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC
                ElseIf mimeHeaderTrasporto.Raw.Contains("errore") Then
                    res = ParsecPro.Pop3.Header.TipologiaEmail.Email_PEC_Anomalia
                End If
            Else
                res = ParsecPro.Pop3.Header.TipologiaEmail.Email_Normale
            End If
        End If
        Return res
    End Function


    'Metodo che restituisce la lista dei Mittenti
    <WebMethod()> _
    Public Shared Function GetMittenti(ByVal context As RadComboBoxContext) As RadComboBoxData
        Dim ea As New EmailArrivoRepository
        Dim data = ea.GetMittenti.Where(Function(c) c.Mittente.Contains(context.Text) And c.Mittente <> "").OrderBy(Function(c) c.Mittente).ToList
        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)
        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            Dim item = data.ElementAt(i)
            itemData.Text = item.Mittente
            itemData.Value = item.Id
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun mittente trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function

    'imposta predefinite le tipologie. Richiamato da CaselleEmailComboBox.SelectedIndexChanged
    Private Sub ImpostaTipologia()
        If Me.CaselleEmailComboBox.Items.Count > 0 Then
            For i As Integer = 0 To Me.TipologieCheckBoxList.Items.Count - 1
                Me.TipologieCheckBoxList.Items.FindByValue(i).Selected = True
            Next
        Else
            ParsecUtility.Utility.MessageBox("Attenzione l'utente non è collegato a nessuna Casella E-mail!", False)
        End If
    End Sub

    'Clacola la dimensione su disco delle mail
    Private Function CalcoloSpazio() As String
        Dim dimensione As Double = 0
        Dim nf As Integer = 0
        Dim path As String = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
        If Not Me.Emails Is Nothing Then
            For Each ea As EmailArrivo In Me.Emails
                path &= ea.PercorsoRelativo.Substring(1, 5) & ea.NomeFileEml
                If IO.File.Exists(path) Then
                    dimensione += IO.File.ReadAllBytes(path).Count
                    nf += 1
                End If
                path = ParsecAdmin.WebConfigSettings.GetKey("PostaImportata")
            Next
        End If
        If Math.Round(dimensione / 1024, 2) > 0 Then
            If Math.Round(dimensione / 1024, 2) > 1000 Then
                Return " ( - " & nf & " - " & Math.Round((dimensione / 1024) / 1024, 2) & " MB ) "
            Else
                Return " ( - " & nf & " - " & Math.Round(dimensione / 1024, 2) & " KB ) "
            End If
        Else
            Return String.Empty
        End If
    End Function

    'Effettua uun controllo tra la Data di Inizio Invio e di fine invio
    Private Function DateEsatte() As Boolean
        If Me.DataInvioInizioTextBox.SelectedDate.HasValue And Me.DataInvioFineTextBox.SelectedDate.HasValue Then
            If Me.DataInvioInizioTextBox.SelectedDate > Me.DataInvioFineTextBox.SelectedDate Then
                ParsecUtility.Utility.MessageBox("Le date specificate sono incogruenti! " & vbCrLf & " da (" & Me.DataInvioInizioTextBox.SelectedDate & ") deve essere ( < ) di a (" & Me.DataInvioFineTextBox.SelectedDate & ")", False)
                Return False
            End If
        End If
        Return True
    End Function

#Region "Esportazione excel"

    'Costruisce la prima riga del file excel di esportazione della griglia
    Private Sub HeaderFile(ByRef sw As System.IO.StreamWriter)
        Dim line As String = String.Empty
        Dim titolo As String = "Filtro : "
        If Me.MittentiComboBox.SelectedValue <> "" Then titolo &= (New EmailArrivoRepository).GetQuery.Where(Function(c) c.Id = CInt(Me.MittentiComboBox.SelectedValue)).FirstOrDefault.Mittente & vbTab
        If Me.CaselleEmailComboBox.SelectedValue <> "" Then titolo &= Me.CaselleEmailComboBox.SelectedItem.Text & vbTab
        If Me.OggettoTextBox.Text.Length > 0 Then titolo &= Me.OggettoTextBox.Text & vbTab
        If Me.DataInvioInizioTextBox.SelectedDate.HasValue Then titolo &= Me.DataInvioInizioTextBox.SelectedDate
        If Me.DataInvioFineTextBox.SelectedDate.HasValue Then titolo &= " - " & Me.DataInvioFineTextBox.SelectedDate & vbTab Else titolo &= vbTab
        titolo &= Me.StatoComboBox.SelectedItem.Text & vbTab
        For Each r As ListItem In Me.TipologieCheckBoxList.Items
            If r.Selected = True Then titolo &= r.Text & "; "
        Next
        line &= titolo & vbCrLf & vbCrLf
        line &= "MITTENTE" & vbTab & "DESTINATARIO" & vbTab & "OGGETTO" & vbTab & "DATA" & vbTab & "PROTOCOLLATA" & vbTab & "NUMERO PROTOCOLLO" & vbTab & "CANCELLATA" & vbTab & "DATA CANCELLAZIONE" & vbTab & "UTENTE CANCELLAZIONE"
        sw.WriteLine(line)
    End Sub

    'Costruisce il contenuto del file excel di esportazione della griglia
    Private Sub DetailFileDoc(ByRef sw As System.IO.StreamWriter, ByVal em As EmailArrivo)
        Dim line As String = ""
        Dim np As String = ""
        Dim rr As New RegistrazioniRepository
        Dim r As ParsecPro.Registrazione = Nothing
        If Not em.IdRegistrazione Is Nothing Then r = rr.GetById(em.IdRegistrazione)
        If Not r Is Nothing Then np = r.NumeroProtocollo & " " & r.DescrizioneTipologiaRegistristrazione.Substring(0, 1).ToUpper & " /" & CDate(r.DataOraRegistrazione).Year
        Dim uc As String = ""
        Dim ur As New UserRepository
        Dim u As New ParsecAdmin.Utente
        If Not em.IdUtenteCancellazione Is Nothing Then u = ur.GetQuery.Where(Function(c) c.Id = em.IdUtenteCancellazione).FirstOrDefault
        If u.Id > 0 Then uc = u.Username & " - " & u.Titolo & " " & u.Cognome.ToUpper & " " & u.Nome
        With em
            line &= .Mittente & vbTab & _
                    Me.CaselleEmailComboBox.SelectedItem.Text & vbTab & _
                    .Oggetto & vbTab & _
                     String.Format("{0:dd/MM/yyyy HH:mm:ss}", .DataArrivo) & vbTab & _
                    IIf(Not .IdRegistrazione Is Nothing, "SI", "NO") & vbTab & _
                    IIf(np.Length = 0, "", np) & vbTab & _
                    IIf(.Cancellata, "SI", "NO") & vbTab & _
                    IIf(.DataCancellazione.HasValue, .DataCancellazione, "") & vbTab & _
                    IIf(uc.Length = 0, "", uc)
        End With
        sw.WriteLine(line)
    End Sub

    'lancia la esportazione della griglia. Richiamato da EsportaXls.Click
    Private Sub SaveStreamWriter(ByVal path As String, ByVal nf As String, ByRef sw As System.IO.StreamWriter)
        sw.Close()
        Session("AttachmentFullName") = path
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
        Dim esportazioniExcel As New ParsecAdmin.ExportExcelRepository
        Dim exportExcel As ParsecAdmin.ExportExcel = esportazioniExcel.CreateFromInstance(Nothing)
        Dim uC As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        With exportExcel
            .NomeFile = nf
            .Oggetto = "Elenco Email"
            .Utente = uC.Username
            .Data = Now
        End With
        esportazioniExcel.Save(exportExcel)
    End Sub

#End Region

#End Region


End Class