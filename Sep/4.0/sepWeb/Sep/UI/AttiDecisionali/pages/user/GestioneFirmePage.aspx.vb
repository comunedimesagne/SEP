Imports ParsecAtt
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneFirmePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

    Public Property Firma() As ParsecAtt.Firma
        Get
            Return CType(Session("GestioneFirmePage_Firma"), ParsecAtt.Firma)
        End Get
        Set(ByVal value As ParsecAtt.Firma)
            Session("GestioneFirmePage_Firma") = value
        End Set
    End Property

    Public Property Firme() As List(Of ParsecAtt.Firma)
        Get
            Return CType(Session("GestioneFirmePage_Firme"), List(Of ParsecAtt.Firma))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Firma))
            Session("GestioneFirmePage_Firme") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Me.Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/PopupPage.master"
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Me.Page.Request("Mode") Is Nothing Then

            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Atti Decisionali"
            Me.MainPage.DescrizioneProcedura = "> Gestione Firme"
            Me.RadToolBar.Items.FindItemByText("Salva e Chiudi").Visible = False
            Me.FirmeGridView.MasterTableView.GetColumn("ConfirmSelectAndClose").Visible = False

        Else
            Me.MainPage = CType(Me.Master, PopupPage)
        End If

        If Not Me.Page.IsPostBack Then
            Me.Firme = Nothing
            Me.ResettaVista()
            Me.GetParametri()

            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Id"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.FirmeGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

            Me.CaricaTipologieFirme()
            Me.CaricaQualifiche()
            Me.CaricaLivelliOrganigramma()

        End If



    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la firma selezionata?", False, Not Me.Firma Is Nothing)

        If Not Me.Firme Is Nothing Then
            Me.TitoloElencoFirmeLabel.Text = "Elenco Firme&nbsp;&nbsp;&nbsp;" & If(Me.Firme.Count > 0, "( " & Me.Firme.Count.ToString & " )", "")
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'Determino se l'utente passato è responsabile del modulo atti.
        Dim responsabileModulo As Boolean = Not (New ParsecAdmin.AbilitazioneResponsabileModuloRepository).GetQuery.Where(Function(c) c.IdModulo = 3 And c.IdUtente = utenteCollegato.Id And c.LogTipoOperazione Is Nothing).FirstOrDefault Is Nothing
        Dim eliminaAbilitato = utenteCollegato.SuperUser And responsabileModulo
        Me.RadToolBar.Items.FindItemByText("Salva").Enabled = eliminaAbilitato

        'SELEZIONO LA RIGA
        If Not Me.Firma Is Nothing Then
            Dim item As GridDataItem = Me.FirmeGridView.MasterTableView.FindItemByKeyValue("Id", Me.Firma.Id)
            If Not item Is Nothing Then
                item.Selected = True
            End If
        End If

    End Sub

#End Region

#Region "EVENTI TOOLBAR"


    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName

            Case "SalvaChiudi"
                Dim success As Boolean = True
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    ParsecUtility.SessionManager.Firma = Me.Firma
                Catch ex As Exception
                    success = False
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)
                If success Then
                    ParsecUtility.Utility.ClosePopup(False)
                End If


            Case "Stampa"
                ' Me.Print()
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
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Annulla"
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Firma Is Nothing Then
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
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una firma!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub FirmeGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FirmeGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.AggiornaVista(e.Item)
            Case "ConfirmSelectAndClose"
                Me.SelezionaFirma(e)
        End Select
    End Sub


    Protected Sub FirmeGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FirmeGridView.NeedDataSource
        If Me.Firme Is Nothing Then
            Dim firme As New ParsecAtt.FirmeRepository
            Me.Firme = firme.GetView(Nothing)
            firme.Dispose()
        End If
        Me.FirmeGridView.DataSource = Me.Firme
    End Sub

    Protected Sub FirmeGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FirmeGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "GESTIONE UTENTE"

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


#End Region

#Region "GESTIONE RUOLO"

    Protected Sub TrovaRuoloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaRuoloImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/RicercaRuoloPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaRuoloImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 0 'singola
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaRuoloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaRuoloImageButton.Click
        If Not ParsecUtility.SessionManager.Ruolo Is Nothing Then
            Dim ruolo As ParsecWKF.Ruolo = ParsecUtility.SessionManager.Ruolo
            Me.RuoloTextBox.Text = ruolo.Descrizione
            Me.IdRuoloTextBox.Text = ruolo.Id
            ParsecUtility.SessionManager.Ruolo = Nothing
        End If
    End Sub


    Protected Sub EliminaRuoloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaRuoloImageButton.Click
        Me.RuoloTextBox.Text = String.Empty
        Me.IdRuoloTextBox.Text = String.Empty
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("Filtro") Then
                Dim filtro = parametriPagina("Filtro")

                '****************************************************************************************************
                'Nascondo la lista delle firme.
                'Me.FirmePanel.Style.Add("display", "none")

                'Disabilito tutti i pulsanti della toolbar
                For i As Integer = 0 To Me.RadToolBar.Items.Count - 1
                    Me.RadToolBar.Items(i).Enabled = False
                Next
                'Abilito i pulsanti
                Me.RadToolBar.Items.FindItemByText("Salva").Enabled = True
                Me.RadToolBar.Items.FindItemByText("Trova").Enabled = True
                Me.RadToolBar.Items.FindItemByText("Annulla").Enabled = True
                '****************************************************************************************************

                Dim firme As New ParsecAtt.FirmeRepository
                Me.Firme = firme.GetView(New ParsecAtt.FiltroFirma With {.Descrizione = filtro})
                firme.Dispose()
                Me.FirmeGridView.Rebind()

                Me.DescrizioneTextBox.Text = filtro


            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If

    End Sub

    Private Sub SelezionaFirma(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Dim firme As New ParsecAtt.FirmeRepository
        ParsecUtility.SessionManager.Firma = firme.GetById(id)
        firme.Dispose()
        ParsecUtility.Utility.ClosePopup(False)
    End Sub

    Private Sub AggiornaGriglia()
        Me.Firme = Nothing
        Me.FirmeGridView.Rebind()
    End Sub


    Private Sub Search()
        Dim firme As New ParsecAtt.FirmeRepository
        Dim filtro As ParsecAtt.FiltroFirma = Me.GetFiltro
        Me.Firme = firme.GetView(filtro)
        firme.Dispose()
        Me.FirmeGridView.Rebind()
    End Sub


    Private Sub CaricaTipologieFirme()
        Dim tipologie As New ParsecAtt.TiplogiaFirmaRepository
        Me.TipologieFirmaComboBox.DataValueField = "Id"
        Me.TipologieFirmaComboBox.DataTextField = "Descrizione"
        Me.TipologieFirmaComboBox.DataSource = tipologie.GetKeyValue()
        Me.TipologieFirmaComboBox.DataBind()
        Me.TipologieFirmaComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipologieFirmaComboBox.SelectedIndex = 0
        tipologie.Dispose()
    End Sub

    Private Sub CaricaQualifiche()
        Me.TipologieQualificaComboBox.DataSource = (New ParsecAdmin.QualificaOrganigrammaRepository).GetQuery.OrderBy(Function(c) c.Id).ToList
        Me.TipologieQualificaComboBox.DataTextField = "Nome"
        Me.TipologieQualificaComboBox.DataValueField = "Id"
        Me.TipologieQualificaComboBox.DataBind()
        Me.TipologieQualificaComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.TipologieQualificaComboBox.SelectedIndex = 0
    End Sub

    Private Sub CaricaLivelliOrganigramma()
        Me.LivelliOrganigrammaComboBox.DataSource = (New ParsecAdmin.StrutturaLivelloRepository).GetQuery.OrderBy(Function(c) c.Gerarchia).ToList
        Me.LivelliOrganigrammaComboBox.DataTextField = "Descrizione"
        Me.LivelliOrganigrammaComboBox.DataValueField = "Gerarchia"
        Me.LivelliOrganigrammaComboBox.DataBind()
        Me.LivelliOrganigrammaComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "-1"))
        Me.LivelliOrganigrammaComboBox.SelectedIndex = 0
    End Sub



    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()

        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim firme As New ParsecAtt.FirmeRepository

        Dim firma As ParsecAtt.Firma = firme.GetById(id)

        Me.DescrizioneTextBox.Text = firma.Descrizione
        Me.FirmatarioTextBox.Text = firma.DefaultStruttura
        Me.InfoTextBox.Text = firma.Info
        Me.EsitoTextBox.Text = firma.DefaultEsito
        Me.TipiEsitoTextBox.Text = firma.TipiEsito
        Me.PrefissoTextBox.Text = firma.DefaultPrefisso
        Me.TipoPrefissoTextBox.Text = firma.TipiPrefisso
        Me.QualificaTextBox.Text = firma.DefaultQualifica
        Me.QualificaFemminileTextBox.Text = firma.DefaultQualificaAlFemminile

        Me.FirmaSempliceCheckBox.Checked = firma.FirmaSemplice
        If Not firma.DataDefault Is Nothing Then
            Me.DataFirmaCheckBox.Checked = firma.DataDefault.Value
        End If

        If firma.IdTipologiaFirma.HasValue Then
            Me.TipologieFirmaComboBox.Items.FindItemByValue(firma.IdTipologiaFirma).Selected = True
        End If

        If firma.IdTipologiaQualifica.HasValue Then
            Me.TipologieQualificaComboBox.Items.FindItemByValue(firma.IdTipologiaQualifica).Selected = True
        End If
        If firma.IdGerarchia.HasValue Then
            Me.LivelliOrganigrammaComboBox.Items.FindItemByValue(firma.IdGerarchia).Selected = True
        End If


        Me.UtenteTextBox.Text = firma.Utente
        Me.IdUtenteTextBox.Text = firma.IdUtente.ToString

        Me.RuoloTextBox.Text = firma.Ruolo
        Me.IdRuoloTextBox.Text = firma.IdRuolo

        Me.Firma = firma

        firme.Dispose()
    End Sub

    Private Sub Delete()
        Dim firme As New ParsecAtt.FirmeRepository
        Try
            firme.Delete(Me.Firma)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            firme.Dispose()
        End Try
    End Sub

    Private Sub Save()

        Dim firme As New ParsecAtt.FirmeRepository
        Dim firma As ParsecAtt.Firma = firme.CreateFromInstance(Me.Firma)

        'Aggiorno il modello
        firma.Descrizione = Me.DescrizioneTextBox.Text.Trim
        firma.DefaultStruttura = Me.FirmatarioTextBox.Text.Trim
        firma.Info = Me.InfoTextBox.Text.Trim
        firma.DefaultEsito = Me.EsitoTextBox.Text.Trim
        firma.TipiEsito = Me.TipiEsitoTextBox.Text.Trim
        firma.DefaultPrefisso = Me.PrefissoTextBox.Text.Trim
        firma.TipiPrefisso = Me.TipoPrefissoTextBox.Text.Trim
        firma.DefaultQualifica = Me.QualificaTextBox.Text.Trim
        firma.DefaultQualificaAlFemminile = Me.QualificaFemminileTextBox.Text.Trim

        firma.FirmaSemplice = Me.FirmaSempliceCheckBox.Checked
        firma.DataDefault = Me.DataFirmaCheckBox.Checked

        firma.IdTipologiaFirma = Nothing
        If Me.TipologieFirmaComboBox.SelectedIndex > 0 Then
            firma.IdTipologiaFirma = CInt(Me.TipologieFirmaComboBox.SelectedValue)
        End If

        firma.IdTipologiaQualifica = Nothing

        If Me.TipologieQualificaComboBox.SelectedIndex > 0 Then
            firma.IdTipologiaQualifica = CInt(Me.TipologieQualificaComboBox.SelectedValue)
        End If

        firma.IdGerarchia = Nothing
        If Me.LivelliOrganigrammaComboBox.SelectedIndex > 0 Then
            firma.IdGerarchia = CInt(Me.LivelliOrganigrammaComboBox.SelectedValue)
        End If

        firma.IdRuolo = Nothing

        If Not String.IsNullOrEmpty(Me.IdRuoloTextBox.Text) Then
            firma.IdRuolo = CInt(Me.IdRuoloTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.IdUtenteTextBox.Text) Then
            firma.IdUtente = CInt(Me.IdUtenteTextBox.Text)
        Else
            firma.IdUtente = 0
        End If

        Try
            firme.Save(firma)
            Me.Firma = firme.Firma
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            firme.Dispose()
        End Try

    End Sub

    Private Sub ResettaVista()
        Me.DescrizioneTextBox.Text = String.Empty
        Me.FirmatarioTextBox.Text = String.Empty
        Me.InfoTextBox.Text = String.Empty
        Me.EsitoTextBox.Text = String.Empty
        Me.TipiEsitoTextBox.Text = String.Empty
        Me.PrefissoTextBox.Text = String.Empty
        Me.TipoPrefissoTextBox.Text = String.Empty
        Me.QualificaTextBox.Text = String.Empty
        Me.QualificaFemminileTextBox.Text = String.Empty
        Me.FirmaSempliceCheckBox.Checked = False
        Me.DataFirmaCheckBox.Checked = False
        Me.UtenteTextBox.Text = String.Empty
        Me.IdUtenteTextBox.Text = String.Empty
        Me.RuoloTextBox.Text = String.Empty
        Me.IdRuoloTextBox.Text = String.Empty
        Me.TipologieFirmaComboBox.SelectedIndex = 0
        Me.TipologieQualificaComboBox.SelectedIndex = 0
        Me.LivelliOrganigrammaComboBox.SelectedIndex = 0
        Me.Firma = Nothing
    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroFirma
        Dim filtro As New ParsecAtt.FiltroFirma
        filtro.Descrizione = Me.DescrizioneTextBox.Text
        Return filtro
    End Function

#End Region

End Class