Imports ParsecAdmin
Imports Telerik.Web.UI
Imports System.Data


Partial Class StampaRegistroGeneralePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


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


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BlankPage.master"
        End If
    End Sub

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

       
        Dim tipologiaDocumento As ParsecAtt.TipoDocumento = CType(Me.Request.QueryString("Tipo"), ParsecAtt.TipoDocumento)
        Dim tipologiaStampa As ParsecAtt.TipologiaStampaAttoAmministrativo = CType(Me.Request.QueryString("TipoStampa"), ParsecAtt.TipologiaStampaAttoAmministrativo)

        If Me.Request.QueryString("Mode") Is Nothing Then

            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Atti Decisionali"
            Select Case tipologiaDocumento
                Case ParsecAtt.TipoDocumento.Determina

                    Select Case tipologiaStampa
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroSettore
                            Me.MainPage.DescrizioneProcedura = "> Stampa Registro Settore Determine"
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                            Me.MainPage.DescrizioneProcedura = "> Stampa Registro Generale Determine"
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa
                            Me.MainPage.DescrizioneProcedura = "> Stampa Elenco Determine (Spesa)"
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.Liquidazioni
                            Me.MainPage.DescrizioneProcedura = "> Stampa Elenco Determine (Liquidazione)"
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni
                            Me.MainPage.DescrizioneProcedura = "> Stampa Elenco Determine (Pubblicazione)"
                    End Select

                Case ParsecAtt.TipoDocumento.Delibera

                    Select Case tipologiaStampa
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                            Me.MainPage.DescrizioneProcedura = "> Stampa Registro Generale Delibere"
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa
                            Me.MainPage.DescrizioneProcedura = "> Stampa Elenco Delibere (Spesa)"
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni
                            Me.MainPage.DescrizioneProcedura = "> Stampa Elenco Delibere (Pubblicazione)"
                    End Select

                Case ParsecAtt.TipoDocumento.Decreto

                    Select Case tipologiaStampa
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                            Me.MainPage.DescrizioneProcedura = "> Stampa Registro Generale Decreti"
                    End Select

                Case ParsecAtt.TipoDocumento.Ordinanza
                    Select Case tipologiaStampa
                        Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                            Me.MainPage.DescrizioneProcedura = "> Stampa Registro Generale Ordinanze"
                    End Select

            End Select
        End If

        Select Case tipologiaDocumento
            Case ParsecAtt.TipoDocumento.Determina

                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroSettore
                        Me.TitoloLabel.Text = "Stampa Registro Settore Determine"
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        Me.TitoloLabel.Text = "Stampa Registro Generale Determine"
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa
                        Me.TitoloLabel.Text = "Stampa Elenco Determine (Spesa)"
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.Liquidazioni
                        Me.TitoloLabel.Text = "Stampa Elenco Determine (Liquidazione)"
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni
                        Me.TitoloLabel.Text = "Stampa Elenco Determine (Pubblicazione)"

                End Select

            Case ParsecAtt.TipoDocumento.Delibera

                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        Me.TitoloLabel.Text = "Stampa Registro Generale Delibere"
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa
                        Me.TitoloLabel.Text = "Stampa Elenco Delibere (Spesa)"
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni
                        Me.TitoloLabel.Text = "Stampa Elenco Delibere (Pubblicazione)"
                End Select


            Case ParsecAtt.TipoDocumento.Decreto

                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        Me.TitoloLabel.Text = "Stampa Registro Generale Decreti"
                End Select


            Case ParsecAtt.TipoDocumento.Ordinanza

                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        Me.TitoloLabel.Text = "Stampa Registro Generale Ordinanze"
                End Select

        End Select


        If Not Me.Page.IsPostBack Then
            Me.ResettaVista()

            If Not Me.Request.QueryString("Tipo") Is Nothing Then
                Me.CaricaTipologieRegistro()
                Me.ImpostaAbilitazioneUI()
            End If
            Me.CaricaTipologieRegistro(tipologiaDocumento)
            Me.CaricaStatiDiscussione()
            Me.CaricaPublicazioniWeb()
            Me.CaricaPublicazioniAlbo()
            Me.CaricaAdottato()
            Me.CaricaTipologieSeduta()

            Me.CaricaModelli()



        End If

        If tipologiaStampa = ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroSettore Then
            Me.SettoreLabel.Text = "Settore *"
        Else
            Me.SettoreLabel.Text = "Settore"
        End If

        Me.ContatoreGeneraleInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.ContatoreGeneraleInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.ContatoreGeneraleFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

        Me.ContatoreSettoreInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.ContatoreSettoreInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.ContatoreSettoreFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub StampaButton_Click(sender As Object, e As System.EventArgs) Handles StampaButton.Click
        Try
            Me.Print()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub


    Protected Sub AnnullaButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaVista()
    End Sub

    Protected Sub TrovaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUfficioImageButton.Click
        Me.TrovaUfficio()
    End Sub

    Protected Sub AggiornaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUfficioImageButton.Click
        Me.AggiornaUfficio()
    End Sub

    Protected Sub EliminaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUfficioImageButton.Click
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
    End Sub

    Protected Sub TrovaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaSettoreImageButton.Click
        Me.TrovaSettore()
    End Sub

    Protected Sub AggiornaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSettoreImageButton.Click
        Me.AggiornaSettore()
    End Sub

    Protected Sub EliminaSettoreImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaSettoreImageButton.Click
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
    End Sub


    'Protected Sub TipologieRegistroComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieRegistroComboBox.SelectedIndexChanged
    '    Me.CaricaModelli()
    'End Sub

    Protected Sub TipologieSedutaComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieSedutaComboBox.SelectedIndexChanged
        Me.CaricaModelli()
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub Print()

        Dim tipologiaStampa As ParsecAtt.TipologiaStampaAttoAmministrativo = CType(Me.Request.QueryString("TipoStampa"), ParsecAtt.TipologiaStampaAttoAmministrativo)

        If tipologiaStampa = ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroSettore Then
            If String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
                Throw New ApplicationException("E' necessario selezionare un settore!")
            End If
        End If

        Dim tipologiaDocumento As ParsecAtt.TipoDocumento = CType(Me.Request.QueryString("Tipo"), ParsecAtt.TipoDocumento)

        Dim filtro As ParsecAtt.FiltroDocumento = Me.GetFiltro
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim parametriStampa As New Hashtable

        Select Case tipologiaDocumento
            Case ParsecAtt.TipoDocumento.Determina

                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroSettore
                        parametriStampa.Add("TipologiaStampa", "StampaRegistroSettoreDetermine")

                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleDetermine")

                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa
                        parametriStampa.Add("TipologiaStampa", "StampaElencoDetermineImpegnoSpesa")
                        filtro.ModelloImpegnoSpesa = True


                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.Liquidazioni
                        parametriStampa.Add("TipologiaStampa", "StampaElencoDetermineLiquidazione")
                        filtro.ModelloLiquidazione = True

                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni
                        filtro.PubblicazioneAlbo = True
                        parametriStampa.Add("TipologiaStampa", "StampaElencoDeterminePubblicazione")
                End Select


            Case ParsecAtt.TipoDocumento.Delibera

                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleDelibera")
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroSettore
                        parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleDelibera")
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa
                        parametriStampa.Add("TipologiaStampa", "StampaElencoDelibereImpegnoSpesa")
                        filtro.ModelloImpegnoSpesa = True
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni
                        parametriStampa.Add("TipologiaStampa", "StampaElencoDeliberePubblicazione")
                        filtro.PubblicazioneAlbo = True

                End Select




            Case ParsecAtt.TipoDocumento.Decreto
                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleDecreto")
                End Select



            Case ParsecAtt.TipoDocumento.Ordinanza
                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        parametriStampa.Add("TipologiaStampa", "StampaRegistroGeneraleOrdinanza")
                End Select

        End Select

        Dim datiStampa As List(Of ParsecAtt.Documento) = Nothing

        If tipologiaDocumento = ParsecAtt.TipoDocumento.Determina AndAlso (tipologiaStampa = ParsecAtt.TipologiaStampaAttoAmministrativo.Liquidazioni OrElse tipologiaStampa = ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa) Then
            datiStampa = documenti.GetViewStampa(filtro)
        Else
            datiStampa = documenti.GetView(filtro)
        End If


       

        If datiStampa.Count = 0 Then
            Throw New ApplicationException("Nessun atto amministrativo trovato con i criteri di filtro impostati!")
        End If


        Select Case tipologiaDocumento
            Case ParsecAtt.TipoDocumento.Determina
                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroSettore
                        datiStampa = datiStampa.OrderBy(Function(c) c.ContatoreStruttura).ToList
                End Select
        End Select


        parametriStampa.Add("DatiStampa", datiStampa)
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
        documenti.Dispose()
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

    Private Sub AggiornaUfficio()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim idUfficio As Integer = struttureSelezionate.First.Id
            'Aggiorno l'ufficio
            Me.UfficioTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdUfficioTextBox.Text = idUfficio.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Private Sub AggiornaSettore()
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim idSettore As Integer = struttureSelezionate.First.Id
            'Aggiorno il settore
            Me.SettoreTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdSettoreTextBox.Text = idSettore.ToString
            Session("SelectedStructures") = Nothing
        End If
    End Sub

    Private Sub TrovaUfficio()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUfficioImageButton.ClientID)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "200")
        parametriPagina.Add("ultimoLivelloStruttura", "200")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Private Sub TrovaSettore()
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaSettoreImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100")
        parametriPagina.Add("ultimoLivelloStruttura", "100")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    End Sub

    Private Sub CaricaAdottato()
        'Dim lista As New List(Of Adottato)
        'lista.Add(New Adottato With {.Id = 1, .Descrizione = "Adottato"})
        'lista.Add(New Adottato With {.Id = 0, .Descrizione = "Non adottato"})
        'Me.AdottateComboBox.DataValueField = "Id"
        'Me.AdottateComboBox.DataTextField = "Descrizione"
        'Me.AdottateComboBox.DataSource = lista
        'Me.AdottateComboBox.DataBind()
        'Me.AdottateComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        'Me.AdottateComboBox.SelectedIndex = 0
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



    Private Sub CaricaTipologieRegistro()
        Dim idTipologiaDocumento As Integer = CInt(Me.Request.QueryString("Tipo"))
        Me.CaricaTipologieRegistro(idTipologiaDocumento)
    End Sub

    Private Sub CaricaModelli(idTipologiaDocumento As Nullable(Of Integer), idTipologiaSeduta As Nullable(Of Integer))
        Dim modelli As New ParsecAtt.ModelliRepository
        Me.ModelliComboBox.DataValueField = "Id"
        Me.ModelliComboBox.DataTextField = "Descrizione"
        If idTipologiaSeduta.HasValue Then
            Me.ModelliComboBox.DataSource = modelli.GetKeyValue(New ParsecAtt.FiltroModello With {.TipologiaDocumento = idTipologiaDocumento, .Disabilitato = False, .IdTipologiaSeduta = idTipologiaSeduta})
        Else
            Me.ModelliComboBox.DataSource = modelli.GetKeyValue(New ParsecAtt.FiltroModello With {.TipologiaDocumento = idTipologiaDocumento, .Disabilitato = False})
        End If

        Me.ModelliComboBox.DataBind()
        Me.ModelliComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.ModelliComboBox.SelectedIndex = 0
        modelli.Dispose()
    End Sub

    Private Sub CaricaStatiDiscussione()
        'Dim stati As New ParsecAtt.StatoDiscussioneRepository
        'Me.StatiApprovazioneComboBox.DataValueField = "Id"
        'Me.StatiApprovazioneComboBox.DataTextField = "Descrizione"
        'Me.StatiApprovazioneComboBox.DataSource = stati.GetKeyValue(Nothing)
        'Me.StatiApprovazioneComboBox.DataBind()
        'Me.StatiApprovazioneComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        'Me.StatiApprovazioneComboBox.SelectedIndex = 0
        'stati.Dispose()
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

    Private Sub CaricaModelli()
        Dim idTipologiaDocumento As Integer = CInt(Me.Request.QueryString("Tipo"))
        Dim idTipologiaSeduta As Nullable(Of Integer) = Nothing
        If Me.TipologieSedutaComboBox.SelectedIndex > 0 Then
            idTipologiaSeduta = CInt(Me.TipologieSedutaComboBox.SelectedValue)
        End If
        'If idTipologiaSeduta.HasValue Then
        Me.CaricaModelli(idTipologiaDocumento, idTipologiaSeduta)
        'End If
    End Sub

    Private Sub ResettaVista()

        Me.DataDocumentoInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataDocumentoFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        Me.DataSedutaInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataSedutaFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        Me.DataPubblicazioneInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataPubblicazioneFineTextBox.SelectedDate = New Date(Now.Year, 12, 31)

        Me.OggettoTextBox.Text = String.Empty
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
        Me.ContatoreGeneraleInizioTextBox.Text = String.Empty
        Me.ContatoreGeneraleFineTextBox.Text = String.Empty
        Me.ContatoreSettoreInizioTextBox.Text = String.Empty
        Me.ContatoreSettoreFineTextBox.Text = String.Empty

        Me.TipologieRegistroComboBox.SelectedIndex = 0
        Me.ModelliComboBox.SelectedIndex = 0
        'Me.StatiApprovazioneComboBox.SelectedIndex = 0
        'Me.AdottateComboBox.SelectedIndex = 0
        Me.TipologieSedutaComboBox.SelectedIndex = 0
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.AbilitaTutto()

    End Sub

    Private Sub AbilitaTutto()
        Me.ContatoreSettorePanel.Enabled = True
        'Me.AdottatePanel.Enabled = True
        'Me.ApprovazionePanel.Enabled = True
        Me.TipologiaSedutaPanel.Enabled = True
        Me.DataSedutaPanel.Enabled = True
        Me.PubblicazioneAlboPanel.Enabled = True
        Me.DataPubblicazionePanel.Enabled = True
        Me.PubblicazioneWebPanel.Enabled = True
        Me.ModelloPanel.Enabled = True

        Me.TipologiaRegistroPanel.Enabled = True

    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroDocumento
        Dim filtro As New ParsecAtt.FiltroDocumento

        filtro.DataDocumentoInizio = Me.DataDocumentoInizioTextBox.SelectedDate
        filtro.DataDocumentoFine = Me.DataDocumentoFineTextBox.SelectedDate
        filtro.DataSedutaInizio = Me.DataSedutaInizioTextBox.SelectedDate
        filtro.DataSedutaFine = Me.DataSedutaFineTextBox.SelectedDate
        filtro.DataPubblicazioneInizio = Me.DataPubblicazioneInizioTextBox.SelectedDate
        filtro.DataPubblicazioneFine = Me.DataPubblicazioneFineTextBox.SelectedDate

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

        filtro.IdTipologiaDocumento = CInt(Me.Request.QueryString("Tipo"))

        If Me.TipologieRegistroComboBox.SelectedIndex > 0 Then
            filtro.IdTipologiaRegistro = CInt(Me.TipologieRegistroComboBox.SelectedValue)
        End If

        If Me.ModelliComboBox.SelectedIndex > 0 Then
            filtro.IdModello = CInt(Me.ModelliComboBox.SelectedValue)
        End If

        'If Me.StatiApprovazioneComboBox.SelectedIndex > 0 Then
        '    filtro.IdStatoDiscussione = CInt(Me.StatiApprovazioneComboBox.SelectedValue)
        'End If

        'If Me.AdottateComboBox.SelectedIndex > 0 Then
        '    filtro.Adottato = CBool(Me.AdottateComboBox.SelectedValue)
        'End If

        If Me.PubblicazioneAlboComboBox.SelectedIndex > 0 Then
            filtro.PubblicazioneAlbo = CBool(Me.PubblicazioneAlboComboBox.SelectedValue)
        End If

        If Me.PubblicazioneWebComboBox.SelectedIndex > 0 Then
            filtro.PubblicazioneWeb = CBool(Me.PubblicazioneWebComboBox.SelectedValue)
        End If

      



        Return filtro
    End Function

    Private Sub ImpostaAbilitazioneUI()
        Me.AbilitaTutto()
        Dim tipologiaDocumento As ParsecAtt.TipoDocumento = CType(Me.Request.QueryString("Tipo"), ParsecAtt.TipoDocumento)
        Dim tipologiaStampa As ParsecAtt.TipologiaStampaAttoAmministrativo = CType(Me.Request.QueryString("TipoStampa"), ParsecAtt.TipologiaStampaAttoAmministrativo)


        Select Case tipologiaDocumento
            Case ParsecAtt.TipoDocumento.Delibera
                Me.ImpostaAbilitazioneDelibera()
            Case ParsecAtt.TipoDocumento.Determina
                Select Case tipologiaStampa
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale
                        Me.ImpostaAbilitazioneDetermina()
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa
                        Me.ImpostaAbilitazioneDeterminaImpegniSpesa()
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni
                        Me.ImpostaAbilitazionePubblicazioni()
                    Case ParsecAtt.TipologiaStampaAttoAmministrativo.Liquidazioni
                        Me.ImpostaAbilitazioneDeterminaImpegniSpesa()
                End Select

            Case ParsecAtt.TipoDocumento.Ordinanza
                Me.ImpostaAbilitazioneOrdinanza()
            Case ParsecAtt.TipoDocumento.Decreto
                Me.ImpostaAbilitazioneOrdinanza()
        End Select
    End Sub

    Private Sub ImpostaAbilitazioneDelibera()
        Me.ContatoreSettorePanel.Enabled = False
        Me.ContatoreSettoreInizioTextBox.Text = String.Empty
        Me.ContatoreSettoreFineTextBox.Text = String.Empty
        Me.TipologiaRegistroPanel.Enabled = False
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
       
        Me.PubblicazioneWebPanel.Enabled = False
        Me.PubblicazioneWebComboBox.SelectedIndex = 0


        Me.PubblicazioneAlboPanel.Enabled = False
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0


        Me.DataPubblicazionePanel.Enabled = False
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing
        Me.TipologiaRegistroPanel.Enabled = False
    End Sub

    Private Sub ImpostaAbilitazioneDetermina()

        Me.PubblicazioneAlboPanel.Enabled = False
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0

        Me.PubblicazioneWebPanel.Enabled = False
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.TipologiaSedutaPanel.Enabled = False
        Me.TipologieSedutaComboBox.SelectedIndex = 0

        Me.DataPubblicazionePanel.Enabled = False
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

        Me.DataSedutaPanel.Enabled = False
        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing


        Me.TipologiaRegistroPanel.Enabled = True
    End Sub

    Private Sub ImpostaAbilitazioneDeterminaImpegniSpesa()

        Me.PubblicazioneAlboPanel.Enabled = False
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0

        Me.PubblicazioneWebPanel.Enabled = False
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.TipologiaSedutaPanel.Enabled = False
        Me.TipologieSedutaComboBox.SelectedIndex = 0

        Me.DataPubblicazionePanel.Enabled = True
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

        Me.DataSedutaPanel.Enabled = False
        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing

        Me.ModelloPanel.Enabled = False
        Me.ModelliComboBox.SelectedIndex = 0
        Me.TipologiaRegistroPanel.Enabled = False

    End Sub

    Private Sub ImpostaAbilitazionePubblicazioni()

        Me.PubblicazioneAlboPanel.Enabled = False
        Me.PubblicazioneAlboComboBox.SelectedIndex = 0

        Me.PubblicazioneWebPanel.Enabled = True
        Me.PubblicazioneWebComboBox.SelectedIndex = 0

        Me.TipologiaSedutaPanel.Enabled = False
        Me.TipologieSedutaComboBox.SelectedIndex = 0

        Me.DataPubblicazionePanel.Enabled = True
        Me.DataPubblicazioneInizioTextBox.SelectedDate = Nothing
        Me.DataPubblicazioneFineTextBox.SelectedDate = Nothing

        Me.DataSedutaPanel.Enabled = False
        Me.DataSedutaInizioTextBox.SelectedDate = Nothing
        Me.DataSedutaFineTextBox.SelectedDate = Nothing
        Me.TipologiaRegistroPanel.Enabled = False

    End Sub

#End Region


End Class