#Region "Namespaces importati"

Imports ParsecAdmin
Imports ParsecCommon
Imports ParsecUtility
Imports ParsecWKF
Imports Telerik.Web.UI

#End Region

Partial Class StoricoPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


    Public Class FilterInfo
        Public Property Value As String
        Public Property ColumnName As String
        Public Property FunctionName As String
        Public Property SortOrder As String
    End Class

#Region "PROPRIETA'"


    Public Property Filters() As List(Of FilterInfo)
        Get
            Return CType(Session("StoricoPage_Filters"), List(Of FilterInfo))
        End Get
        Set(value As List(Of FilterInfo))
            Session("StoricoPage_Filters") = value
        End Set
    End Property

    Public Property Istanze() As List(Of ParsecWKF.Istanza)
        Get
            Return CType(Session("StoricoPage_Istanze"), List(Of ParsecWKF.Istanza))
        End Get
        Set(ByVal value As List(Of ParsecWKF.Istanza))
            Session("StoricoPage_Istanze") = value
        End Set
    End Property

    Public Property Visualizzazioni() As List(Of ParsecWKF.VisualizzazioneRepository)
        Get
            Return CType(Session("StoricoPage_Visualizzazioni"), List(Of ParsecWKF.VisualizzazioneRepository))
        End Get
        Set(ByVal value As List(Of ParsecWKF.VisualizzazioneRepository))
            Session("StoricoPage_Visualizzazioni") = value
        End Set
    End Property

    Protected Function DataInserimento(ByVal container As GridItem) As System.Nullable(Of DateTime)
        If container.OwnerTableView.GetColumn("DataInserimento").CurrentFilterValue = String.Empty Then
            Return New System.Nullable(Of DateTime)()
        Else
            Try
                Return DateTime.Parse(container.OwnerTableView.GetColumn("DataInserimento").CurrentFilterValue)
            Catch ex As Exception
                Return New System.Nullable(Of DateTime)()
            End Try

        End If
    End Function

    Private Property Filtri As List(Of Filtro)
        Get
            Return CType(Session(Session.SessionID & "Filtri"), List(Of Filtro))
        End Get
        Set(value As List(Of Filtro))
            Session(Session.SessionID & "Filtri") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Scrivania"
        MainPage.DescrizioneProcedura = "> Scrivania STORICO"

        If Not Me.IsPostBack Then
            Me.Istanze = Nothing
            Me.Visualizzazioni = Nothing
            Me.CaricaStatiTask()
            Me.CaricaModelliWorkflow(Nothing)
            Me.CaricaModuli()
            Me.CaricaStatiProcesso()
            Me.CaricaUtenti()
            Me.ResettaFiltro()
            Me.GetCookie()
            Me.VisualizzaFiltroCorrente()
            Me.FiltraImageButton.Focus() '
            Me.FiltraImageButton.Attributes.Add("onclick", "ShowPanel();hide=false; return false;")
            Me.SalvaButton.Attributes.Add("onclick", "HidePanel();hide=true;")
            Me.ResettaFiltroButton.Attributes.Add("onclick", "HidePanel();hide=true;return false;")
            Me.Filters = New List(Of FilterInfo)
        End If

        Me.TipologiaDocumentoComboBox.AutoPostBack = False
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Me.Istanze Is Nothing Then
            Me.ElencoIstanzaLabel.Text = "Istanze&nbsp;" & If(Me.Istanze.Count > 0, "( " & Me.Istanze.Count.ToString & " )", "")
        End If
        ParsecUtility.Utility.SaveScrollPosition(Me.scrollPanel, Me.scrollPosHidden, False)
        If Not DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue And Not DataFineIstanzaFiltroTextBox.SelectedDate.HasValue Then
            If Me.SalvaButton.Visible Then
                Me.SalvaButton.Attributes.Add("onclick", "HidePanel();hide=false;")
                Me.SalvaButton.Enabled = False
                Me.SalvaFiltroButton.Enabled = False
                Utility.MessageBox("E' obbligatorio almeno una tra la 'Data Inizio da' e la 'Data Inizio a'!", False)
            End If
        Else
            Me.SalvaButton.Attributes.Add("onclick", "HidePanel();hide=true;")
            Me.SalvaButton.Enabled = True
            Me.SalvaFiltroButton.Enabled = True
        End If
        Me.SalvaFiltroButton.Enabled = Me.SalvaButton.Enabled
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        Me.FiltriLabel.Visible = Not ((Me.Filtri Is Nothing) Or (Me.Filtri.Count = 0))
        Me.ToolBarFiltri.DataSource = Me.Filtri
        Me.ToolBarFiltri.DataBind()
    End Sub

#End Region

#Region "METODI PRIVATI"

  

    Private Sub CaricaStatiTask()
        Dim tasks As New ParsecWKF.TaskRepository
        Me.StatoTaskComboBox.DataSource = tasks.GetKeyValue.Where(Function(c) c.Descrizione <> String.Empty).ToList
        Me.StatoTaskComboBox.DataTextField = "Descrizione"
        Me.StatoTaskComboBox.DataValueField = "Id"
        Me.StatoTaskComboBox.DataBind()
        Me.StatoTaskComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Seleziona -", "-1"))
        Me.StatoTaskComboBox.SelectedIndex = 0
        tasks.Dispose()
    End Sub

    Private Sub CaricaStatiProcesso()
        Dim stati As New ParsecWKF.StatoRepository
        Me.StatoIterListBox.DataSource = stati.GetStato()
        Me.StatoIterListBox.DataTextField = "Descrizione"
        Me.StatoIterListBox.DataValueField = "Id"
        Me.StatoIterListBox.DataBind()
        Me.StatoIterListBox.Items.Insert(0, New RadListBoxItem("Seleziona Tutto", "-1"))
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'If Not utenteCorrente.SuperUser Then
        Dim listItem = Me.StatoIterListBox.FindItemByValue("4")
        If Not listItem Is Nothing Then
            Me.StatoIterListBox.FindItemByValue("4").Remove()
        End If
        'End If
        stati.Dispose()
    End Sub

  

    Private Sub CaricaModelliWorkflow(ByVal idModulo As Nullable(Of Integer))
        Dim modelli As New ParsecWKF.ModelliRepository
        Me.TipologiaDocumentoComboBox.DataSource = modelli.GetKeyValue(idModulo)
        Me.TipologiaDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologiaDocumentoComboBox.DataValueField = "Id"
        Me.TipologiaDocumentoComboBox.DataBind()
        Me.TipologiaDocumentoComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Tutte -", "-1"))
        Me.TipologiaDocumentoComboBox.SelectedIndex = 0
        modelli.Dispose()
    End Sub


    Private Sub CaricaModuli()
        Dim moduli As New ParsecAdmin.ModuleRepository
        Me.ModuliComboBox.DataSource = moduli.GetQuery.Where(Function(c) c.Id = 2 Or c.Id = 3 Or c.Id = 5 Or c.Id = 17 Or c.Id = 1001).OrderBy(Function(c) c.Descrizione).ToList
        Me.ModuliComboBox.DataTextField = "Descrizione"
        Me.ModuliComboBox.DataValueField = "Id"
        Me.ModuliComboBox.DataBind()
        Me.ModuliComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Tutti -", "-1"))
        moduli.Dispose()
    End Sub



    Private Sub CaricaUtenti()
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        With Me.UtentiComboBox
            .DataSource = istanze.GetUtenti()
            .DataTextField = "Descrizione"
            .DataValueField = "Id"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem("- Seleziona -", "-1"))
            .SelectedIndex = 0
        End With
        istanze.Dispose()
    End Sub

    Private Function GetFiltro() As ParsecWKF.FiltroIstanza
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim filtro As New ParsecWKF.FiltroIstanza
        filtro.IdUtente = utenteCorrente.Id
        If Me.UtentiComboBox.SelectedValue > -1 Then filtro.IdOperatore = CInt(Me.UtentiComboBox.SelectedValue)
        If Me.ModuliComboBox.SelectedValue > -1 Then filtro.IdModulo = CInt(Me.ModuliComboBox.SelectedValue)
        If Me.StatoTaskComboBox.SelectedValue > -1 Then filtro.Stato = Me.StatoTaskComboBox.SelectedItem.Text
        If Me.TipologiaDocumentoComboBox.SelectedValue > -1 Then filtro.IdModello = CInt(Me.TipologiaDocumentoComboBox.SelectedValue)

        If Me.StatoIterListBox.CheckedItems.Count > 0 Then
            filtro.IdStatiIterSelezionati = New List(Of Integer)
            For Each s In Me.StatoIterListBox.CheckedItems
                If s.Value <> "-1" Then
                    filtro.IdStatiIterSelezionati.Add(s.Value)
                End If
            Next
        Else
            filtro.IdStatiIterSelezionati = New List(Of Integer)
            For Each s In Me.StatoIterListBox.Items
                If s.Value <> "-1" Then
                    filtro.IdStatiIterSelezionati.Add(s.Value)
                End If
            Next
        End If


        filtro.DataInizioInserimento = Me.DataInizioIstanzaFiltroTextBox.SelectedDate
        filtro.DataFineInserimento = Me.DataFineIstanzaFiltroTextBox.SelectedDate
        filtro.Documento = Me.RiferimentoFiltroTextBox.Text
        Return filtro
    End Function

    Private Sub AggiornaGriglia()
        Me.Istanze = Nothing
        Me.IstanzaGridView.Rebind()
    End Sub

    Private Sub ResettaFiltro()
        Me.RiferimentoFiltroTextBox.Text = String.Empty
        Me.DataInizioIstanzaFiltroTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataFineIstanzaFiltroTextBox.SelectedDate = New Date(Now.Year, 12, 31)
        Me.ModuliComboBox.ClearSelection()
        Me.TipologiaDocumentoComboBox.ClearSelection()
        Me.StatoIterListBox.ClearChecked()
        Me.StatoTaskComboBox.ClearSelection()
        Me.UtentiComboBox.ClearSelection()
        Dim listItem = Me.StatoIterListBox.FindItemByValue("1")
        If Not listItem Is Nothing Then Me.StatoIterListBox.FindItemByValue("1").Checked = True
        Me.VisualizzaFiltroCorrente()
    End Sub

    Private Sub Cancella(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("ID")
        Dim istanze As New IstanzaRepository
        Dim ist As Istanza = istanze.GetById(id)
        If Not ist Is Nothing Then
            Dim message As String = "L'istanza '" & ist.Riferimento & "'  del  '" & String.Format("{0:dd/MM/yyyy}", ist.DataInserimento) & "'  è stata cancellata con successo!"
            Try
                istanze.Delete(ist, utenteCorrente)
                Me.Istanze = Nothing
                Me.IstanzaGridView.Rebind()
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Impossibile cancellare l'istanza selezionata, per il seguente errore:" & vbCrLf & ex.Message, False)
            Finally
                ParsecUtility.Utility.MessageBox(message, False)
                istanze.Dispose()
            End Try
        End If
    End Sub

 

    Private Sub PrintIter(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("ID")
        Dim ist As Istanza = (New IstanzaRepository).GetById(id)
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "IterStorico")
        parametriStampa.Add("DatiStampa", (New IstanzaRepository).GetTaskIstanze(id))
        parametriStampa.Add("Titolo", "Istanza: " & ist.Riferimento)
        Session("ParametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

   

    Private Sub Preview(ByVal item As Telerik.Web.UI.GridDataItem)
        ' registrazione della visualizzazione

        'Dim viss As New VisualizzazioneRepository
        'Dim vis As New Visualizzazione
        'Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)


        'vis.Data = Now
        'vis.IdDoc = ist.IdDocumento
        'vis.IdIstanza = id
        'vis.IdUtente = utenteCorrente.Id
        'vis.Operazione = "Visualizzazione ITER"
        'viss.Save(vis)

        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("ID")
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As Istanza = istanze.GetById(id)
        istanze.Dispose()

        Select Case CType(istanza.IdModulo, ParsecAdmin.TipoModulo)
            Case TipoModulo.PRO
                Dim pageUrl As String = "~/UI/Protocollo/pages/search/VisualizzaRegistrazionePage.aspx"
                Dim parametriPagina As New Hashtable
                parametriPagina.Add("Filtro", istanza.IdDocumento)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 510, Nothing, False)
            Case TipoModulo.ATT
                Me.VisualizzaAtto(istanza.IdDocumento)

            Case TipoModulo.PED ' Pratiche Edilizie
                Dim pageUrl As String = "~/UI/PraticheEdilizie/pages/search/VisualizzaPraticaPage.aspx"
                Dim parametriPagina As New Hashtable
                parametriPagina.Add("Filtro", istanza.IdDocumento)
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 610, Nothing, False)

            Case TipoModulo.CON
                'visualizza pratiche
                Dim pageUrl As String = "~/UI/Contenzioso/Pages/user/FormularioPage.aspx"
                ParsecUtility.Utility.ShowPopup(pageUrl, 580, 370, Nothing, False)
            Case TipoModulo.IOL
                Dim parametriPagina As New Hashtable
                Dim pageUrl As String = "~/UI/Amministrazione/pages/search/VisualizzaIstanzaPraticaPage.aspx"
                parametriPagina.Add("Filtro", CInt(istanza.IdDocumento))
                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
                ParsecUtility.Utility.ShowPopup(pageUrl, 940, 670, Nothing, False)
            Case TipoModulo.CSRA

                Dim idAtto As Integer = istanza.IdDocumento
                Dim documenti As New ParsecAtt.DocumentoRepository
                Dim documento = documenti.Where(Function(c) c.Id = idAtto).FirstOrDefault
                documenti.Dispose()

                Dim schede As New ParsecAtt.SchedaControlloSuccessivoRegolaritaAmministrativaRepository
                Dim scheda = schede.Where(Function(c) c.NumeroAtto = documento.ContatoreGenerale And c.DataAtto = documento.Data).FirstOrDefault
                schede.Dispose()

                Dim queryString As New Hashtable
                queryString.Add("IdSchedaControlloSuccessivoRegolaritaAmministrativa", scheda.Id.ToString)
                queryString.Add("Mode", "V")
                Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/SchedaControlloSuccessivoRegolaritaAmministrativaPage.aspx"


                ParsecUtility.Utility.ShowPopup(pageUrl, 910, 560, queryString, False)


        End Select

    End Sub

    Private Sub VisualizzaAtto(idDocumento As Integer)
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
            ParsecUtility.Utility.ShowPopup(pageUrl, 910, 600, queryString, False)
        End If
        documenti.Dispose()
    End Sub

    Private Sub VisualizzaFiltroCorrente()
        Dim tooltip As String = String.Empty
        Dim numeroparametriapplicati As Integer = 0
        If Not Filtri Is Nothing Then Me.Filtri.Clear() Else Me.Filtri = New List(Of Filtro)
        Dim filtro As New Filtro
        If CInt(ModuliComboBox.SelectedValue) > -1 Then
            filtro.Id = TipologiaFiltro.Modulo
            filtro.Valore = Me.ModuliComboBox.SelectedItem.Text
            If Me.ModuliComboBox.SelectedItem.Text.Length > 10 Then filtro.Descrizione = Me.ModuliComboBox.SelectedItem.Text.Substring(0, 10) Else filtro.Descrizione = Me.ModuliComboBox.SelectedItem.Text
            filtro.Cancellabile = True
            filtro.Tooltip = "-Filtro Modulo: " & Me.ModuliComboBox.SelectedItem.Text
            tooltip &= filtro.Tooltip & vbCrLf
            Me.Filtri.Add(filtro)
            numeroparametriapplicati += 1
        End If
        If CInt(TipologiaDocumentoComboBox.SelectedValue) > -1 Then
            filtro = New Filtro
            filtro.Id = TipologiaFiltro.Tipologia
            filtro.Valore = Me.TipologiaDocumentoComboBox.SelectedItem.Text
            If Me.TipologiaDocumentoComboBox.SelectedItem.Text.Length > 10 Then
                filtro.Descrizione = Me.TipologiaDocumentoComboBox.SelectedItem.Text.Substring(0, 10)
            Else
                filtro.Descrizione = Me.TipologiaDocumentoComboBox.SelectedItem.Text
            End If
            filtro.Tooltip = "-Filtro Tipologia Documento: " & Me.TipologiaDocumentoComboBox.SelectedItem.Text
            tooltip &= filtro.Tooltip & vbCrLf
            filtro.Cancellabile = True
            Me.Filtri.Add(filtro)
            numeroparametriapplicati += 1
        End If
        If Me.DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue Then
            filtro = New Filtro
            filtro.Id = TipologiaFiltro.DataInizio
            filtro.Valore = Me.DataInizioIstanzaFiltroTextBox.SelectedDate
            filtro.Descrizione = Me.DataInizioIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
            filtro.Cancellabile = True
            filtro.Tooltip = "-Filtro Data Inizio Istanza: " & Me.DataInizioIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
            tooltip &= filtro.Tooltip & vbCrLf
            Me.Filtri.Add(filtro)
            numeroparametriapplicati += 1
        End If
        If Me.DataFineIstanzaFiltroTextBox.SelectedDate.HasValue Then
            filtro = New Filtro
            filtro.Id = TipologiaFiltro.DataFine
            filtro.Valore = Me.DataFineIstanzaFiltroTextBox.SelectedDate
            filtro.Descrizione = Me.DataFineIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
            filtro.Cancellabile = True
            filtro.Tooltip = "-Filtro Data Fine Istanza: " & Me.DataFineIstanzaFiltroTextBox.SelectedDate.Value.ToShortDateString
            tooltip &= filtro.Tooltip & vbCrLf
            Me.Filtri.Add(filtro)
            numeroparametriapplicati += 1
        End If
        If Me.StatoIterListBox.CheckedItems.Count > 0 Then
            filtro = New Filtro
            filtro.Id = TipologiaFiltro.StatoIter
            For Each s In Me.StatoIterListBox.CheckedItems
                If s.Value <> "-1" Then
                    filtro.Valore &= s.Text & ";"
                Else
                    filtro.Valore = "Tutti"
                    Exit For
                End If
            Next
            filtro.Tooltip = "-Filtro Stato Iter: " & filtro.Valore
            tooltip &= filtro.Tooltip & vbCrLf
            If filtro.Valore.ToString.Length > 10 Then filtro.Descrizione = filtro.Valore.ToString.Substring(0, 10) Else filtro.Descrizione = filtro.Valore
            filtro.Cancellabile = True
            Me.Filtri.Add(filtro)
            numeroparametriapplicati += 1
        End If
        If numeroparametriapplicati > 4 Then
            If CInt(Me.StatoTaskComboBox.SelectedValue) > -1 Or Not String.IsNullOrEmpty(Me.RiferimentoFiltroTextBox.Text) Or CInt(Me.UtentiComboBox.SelectedValue) > -1 Then
                filtro = New Filtro
                filtro.Id = TipologiaFiltro.Generico
                'filtro.Valore = Me.StatoTaskComboBox.SelectedItem.Text
                filtro.Descrizione = "..."
                If CInt(Me.StatoTaskComboBox.SelectedValue) > -1 Then filtro.Tooltip = "-Filtro Stato Ultimo Task: " & Me.StatoTaskComboBox.SelectedItem.Text & vbCrLf
                If Not String.IsNullOrEmpty(Me.RiferimentoFiltroTextBox.Text) Then filtro.Tooltip &= "-Filtro Riferimento: " & Me.RiferimentoFiltroTextBox.Text & vbCrLf
                If CInt(Me.UtentiComboBox.SelectedValue) > -1 Then filtro.Tooltip &= "-Assegnato a: " & Me.UtentiComboBox.SelectedItem.Text & vbCrLf
                filtro.Tooltip &= tooltip
                filtro.Cancellabile = False
                Me.Filtri.Add(filtro)
            End If
        End If
        'Me.IstanzaGridView.Rebind()
    End Sub

    Private Sub GetCookie()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim cookie As HttpCookie = Request.Cookies("PreferitiFiltriStorico_" & utenteCollegato.Id.ToString)
        If Not cookie Is Nothing Then
            Dim item As RadComboBoxItem = Me.ModuliComboBox.Items.FindItemByValue(cookie("FiltroModulo"))
            Dim item2 As RadComboBoxItem = Me.TipologiaDocumentoComboBox.Items.FindItemByValue(cookie("FiltroTipoDocumento"))
            Dim item3 As RadComboBoxItem = Me.UtentiComboBox.Items.FindItemByValue(cookie("FiltroAssegnatoA"))
            Dim item4 As RadComboBoxItem = Me.StatoTaskComboBox.Items.FindItemByText(cookie("FiltroStatoTask"))
            'If Not item Is Nothing AndAlso Not item2 Is Nothing AndAlso Not item3 Is Nothing AndAlso Not item4 Is Nothing Then
            item.Selected = True
            item2.Selected = True
            item3.Selected = True
            item4.Selected = True
            If Not IsNothing(cookie("FiltroDataInzio")) And (cookie("FiltroDataInzio") <> "") Then
                Me.DataInizioIstanzaFiltroTextBox.SelectedDate = cookie("FiltroDataInzio")
            Else
                Me.DataInizioIstanzaFiltroTextBox.SelectedDate = Nothing
            End If
            If Not IsNothing(cookie("FiltroDataFine")) And (cookie("FiltroDataFine") <> "") Then
                Me.DataFineIstanzaFiltroTextBox.SelectedDate = cookie("FiltroDataFine")
            Else
                Me.DataFineIstanzaFiltroTextBox.SelectedDate = Nothing
            End If
            If Not IsNothing(cookie("FiltroDocumento")) Then Me.RiferimentoFiltroTextBox.Text = cookie("FiltroDocumento")
            If Not IsNothing(cookie("FiltroStatiIter")) And (cookie("FiltroStatiIter") <> "") Then
                If cookie("FiltroStatiIter").Contains("|") Then
                    Dim s() As String
                    s = Split(cookie("FiltroStatiIter"), "|")
                    For i As Integer = 0 To s.Length - 2 'UBound(s)
                        Me.StatoIterListBox.FindItemByValue(s(i)).Checked = True
                    Next
                Else
                    If cookie("FiltroStatiIter") = "-1" Then
                        For i As Integer = 0 To Me.StatoIterListBox.Items.Count - 1
                            Me.StatoIterListBox.Items(i).Checked = True
                        Next
                    Else
                        Me.StatoIterListBox.FindItemByValue(cookie("FiltroStatiIter")).Checked = True
                    End If
                End If

            Else
                Me.StatoIterListBox.ClearChecked()
            End If
            Me.Search()
            'End If
        End If
    End Sub

    Private Sub SaveCookie()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim cookie As HttpCookie = Request.Cookies("PreferitiFiltriStorico_" & utenteCollegato.Id.ToString)
        If cookie Is Nothing Then cookie = New HttpCookie("PreferitiFiltriStorico_" & utenteCollegato.Id.ToString)
        cookie("FiltroModulo") = Me.ModuliComboBox.SelectedValue
        cookie("FiltroTipoDocumento") = Me.TipologiaDocumentoComboBox.SelectedValue
        cookie("FiltroAssegnatoA") = Me.UtentiComboBox.SelectedValue
        cookie("FiltroStatoTask") = Me.StatoTaskComboBox.SelectedItem.Text
        If Me.DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue Then cookie("FiltroDataInzio") = Me.DataInizioIstanzaFiltroTextBox.SelectedDate Else cookie("FiltroDataInzio") = Nothing
        If Me.DataFineIstanzaFiltroTextBox.SelectedDate.HasValue Then cookie("FiltroDataFine") = Me.DataFineIstanzaFiltroTextBox.SelectedDate Else cookie("FiltroDataFine") = Nothing
        cookie("FiltroDocumento") = Me.RiferimentoFiltroTextBox.Text
        If Me.StatoIterListBox.CheckedItems.Count > 0 Then
            cookie("FiltroStatiIter") = ""
            For Each s In Me.StatoIterListBox.CheckedItems
                If s.Value <> "-1" Then
                    cookie("FiltroStatiIter") &= s.Value & "|"
                Else
                    cookie("FiltroStatiIter") = s.Value
                    Exit For
                End If
            Next
        Else
            cookie("FiltroStatiIter") = Nothing
        End If
        cookie.Expires = DateTime.Now.AddYears(100)
        Response.Cookies.Add(cookie)
        'Utility.MessageBox("Salvata, nei cookie, la configurazione dei filtri per la tua scrivania STORICA!" & vbCrLf & "Ora puoi premere 'OK'!", False)
    End Sub

    Private Sub Search()
       Me.AggiornaGriglia()
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub IstanzaGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles IstanzaGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim rigaStorico As ParsecWKF.Istanza = CType(e.Item.DataItem, ParsecWKF.Istanza)
            ' è un'atto
            'If rigaStorico.IdModulo = 3 Then
            '    'If Not DataLayerWKF.DaNumerare(e.Row.DataItem("IdDocumento")) Then
            '    '    e.Row.BackColor = Drawing.Color.LightBlue
            '    '    If DataLayerSEP.ModuloAbilitato(Modulo.TipoModulo.MES) And DataLayerWKF.Pubblicato(e.Row.DataItem("IdDocumento")) Then
            '    '        e.Row.BackColor = Drawing.Color.Gold
            '    '        e.Row.Cells(8).ToolTip = "Atto pubblicato onLine, verificare sul sito istutizionale!"
            '    '    End If
            '    'Else
            '    '    e.Row.BackColor = Drawing.Color.LightPink
            '    'End If
            '    'è un protocollo
            'ElseIf rigaStorico.IdModulo = 2 Then
            '    e.Item.BackColor = Drawing.Color.LightGreen
            '    ' è una pratica di conciliazione CORECOM
            'ElseIf rigaStorico.IdModulo = 13 Then
            '    e.Item.BackColor = Drawing.Color.Aqua
            'End If


            Dim referentiEsterni As New ParsecWKF.ReferenteEsternoViewRepository()
            Dim rubrica As New ParsecWKF.StrutturaEsternaViewRepository(referentiEsterni.Context)


            Dim vistaReferente = From re In referentiEsterni.GetQuery
                                 Join r In rubrica.GetQuery.Select(Function(c) New With {c.Denominazione, c.Nome, c.Indirizzo, c.CAP, c.Comune, c.Provincia, c.Email, c.Id})
                                 On re.Id Equals r.Id
                                 Where re.IdRegistrazione = rigaStorico.IdDocumento
                                 Order By re.Id
                                 Select r


            CType(e.Item.FindControl("InterlocutoreLabel"), Label).Text = rigaStorico.Proponente

            If vistaReferente.Any Then
                Dim ref = vistaReferente.FirstOrDefault
                Dim interlocutore = IIf(ref.Denominazione = Nothing, "", ref.Denominazione) + " " + IIf(ref.Nome = Nothing, "", ref.Nome) + ", " + IIf(ref.Indirizzo = Nothing, "", ref.Indirizzo) + ", " + IIf(ref.CAP = Nothing, "", ref.CAP) + " " + IIf(ref.Comune = Nothing, "", ref.Comune) + " (" + IIf(ref.Provincia = Nothing, "", ref.Provincia) + ")" + IIf(ref.Email = Nothing, "", " | " & ref.Email)
                CType(e.Item.FindControl("InterlocutoreLabel"), Label).ToolTip = interlocutore
            End If





            If Not IsDBNull(rigaStorico.DataEsecuzione) Then
                e.Item.Cells(9).Text = " - "
                e.Item.Cells(9).ToolTip = "Istanza conclusa - Informazione sui giorni INUTILE!"
            Else
                e.Item.Cells(9).ToolTip = "Giorni di permanenza sulla scrivania attuale!"
            End If
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                Dim visibile As Boolean = utenteCorrente.SuperUser And rigaStorico.IdStato = 1
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                If visibile Then
                    btn.ImageUrl = "~\images\Delete16.png"
                    btn.ToolTip = "Elimina l'istanza"
                    Dim message As String = "Sei sicuro di eliminare l'Istanza selezionata?"
                    btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
                Else
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")

                End If

                'If btn.Visible Then

                'End If
            End If
            'If TypeOf dataItem("PIN").Controls(0) Is ImageButton Then
            '    btn = CType(dataItem("PIN").Controls(0), ImageButton)
            '    btn.Visible = (rigaStorico.IdModulo = 3)
            'End If
        End If
    End Sub


    '********************************************************************************************************************************
    'RESTITUISCE UNA LISTA DI ISTANZE STORICHE FILTRATA IN BASE AI FILTRI IMPOSTATI NELLA GRIGLIA ED ORDINATA IN BASE ALLA COLONNA
    '********************************************************************************************************************************
    Private Function FiltraIstanzeStoriche() As List(Of ParsecWKF.Istanza)

        Dim res = Me.Istanze.ToList


        If Not Me.Filters Is Nothing Then
            Dim key As String = String.Empty
            For Each f In Me.Filters
                key = f.Value
                If Not String.IsNullOrEmpty(key) Then
                    key = key.ToLower
                    Select Case f.FunctionName
                        Case "NoFilter"
                            Exit Select

                        Case "Contains"
                            Select Case f.ColumnName
                                Case "DescrizioneRiferimento"
                                    res = res.Where(Function(c) c.DescrizioneRiferimento.ToLower.Contains(key)).ToList

                                Case "Proponente"
                                    res = res.Where(Function(c) c.Proponente.ToLower.Contains(key)).ToList

                                Case "Stato"
                                    res = res.Where(Function(c) c.Stato.ToLower.Contains(key)).ToList

                            End Select
                        Case "EqualTo"
                            Select Case f.ColumnName
                                Case "DataInserimento"
                                    res = res.Where(Function(c) c.DataInserimento = key).ToList
                            End Select

                    End Select
                End If

                Dim order As String = String.Empty
                If Not String.IsNullOrEmpty(f.SortOrder) Then
                    order = f.SortOrder.ToLower
                End If


                Select Case f.ColumnName

                    Case "DescrizioneRiferimento"
                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.DescrizioneRiferimento).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.DescrizioneRiferimento).ToList
                        End Select

                    Case "Proponente"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.Proponente).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.Proponente).ToList
                        End Select

                    Case "Stato"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.Stato).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.Stato).ToList
                        End Select

                    Case "DataInserimento"

                        Select Case order
                            Case "ascending"
                                res = res.OrderBy(Function(c) c.DataInserimento).ToList
                            Case "descending"
                                res = res.OrderByDescending(Function(c) c.DataInserimento).ToList
                        End Select


                End Select

            Next

        End If

        Return res
    End Function

    Protected Sub IstanzaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles IstanzaGridView.ItemCommand
        Select Case e.CommandName


            Case Telerik.Web.UI.RadGrid.FilterCommandName
                Dim filter = CType(e.CommandArgument, Pair)
                Dim nomeColonna As String = filter.Second.ToString
                Dim value = Me.IstanzaGridView.MasterTableView.Columns.FindByUniqueName(nomeColonna).CurrentFilterValue
                Dim functionName = filter.First.ToString()
                Dim info As New FilterInfo With {.Value = value, .ColumnName = nomeColonna, .FunctionName = functionName}
                Dim old = Me.Filters.Where(Function(c) c.ColumnName = nomeColonna).FirstOrDefault
                If Not old Is Nothing Then
                    Me.Filters.Remove(old)
                End If
                Me.Filters.Add(info)

            Case Telerik.Web.UI.RadGrid.SortCommandName

                Dim m = DirectCast(e, Telerik.Web.UI.GridSortCommandEventArgs).NewSortOrder.ToString
                Dim old = Me.Filters.Where(Function(c) c.ColumnName = e.CommandArgument).FirstOrDefault
                If old Is Nothing Then
                    Dim info As New FilterInfo With {.ColumnName = e.CommandArgument, .SortOrder = m}
                    For Each f In Me.Filters
                        f.SortOrder = ""
                    Next
                    Me.Filters.Add(info)
                Else
                    For Each f In Me.Filters
                        f.SortOrder = ""
                    Next
                    old.SortOrder = m
                End If

            Case Telerik.Web.UI.RadGrid.ExpandCollapseCommandName
                If Not e.Item.Expanded Then
                    Dim parentItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
                    Dim innerGrid As Telerik.Web.UI.RadGrid = CType(parentItem.ChildItem.FindControl("IterGridView"), Telerik.Web.UI.RadGrid)
                    innerGrid.Rebind()
                End If
            Case "Delete"
                Me.Cancella(e.Item)
            Case "Preview"
                Me.Preview(e.Item)
            Case "PrintIter"
                Me.PrintIter(e.Item)
        End Select

    End Sub


    Protected Sub IstanzaGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles IstanzaGridView.NeedDataSource
        Try
            If Me.Istanze Is Nothing Then
                Dim istanze As New ParsecWKF.IstanzaRepository
                Dim filtro As ParsecWKF.FiltroIstanza = Me.GetFiltro
                Me.Istanze = istanze.GetView(filtro)
                istanze.Dispose()
            End If
            IstanzaGridView.DataSource = Me.Istanze
        Catch ex As Exception
            If Not ex.InnerException Is Nothing Then
                ParsecUtility.Utility.MessageBox(ex.InnerException.Message, False)
            Else
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End If
        End Try
    End Sub

  

    Protected Sub IstanzaGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles IstanzaGridView.ItemCreated
        If TypeOf e.Item Is GridFilteringItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            'e.Item.Style.Add("background-color", "White")
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
        If TypeOf e.Item Is GridNestedViewItem Then
            AddHandler CType(e.Item.FindControl("IterGridView"), RadGrid).NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf Me.IterGridView_NeedDataSource)
            AddHandler CType(e.Item.FindControl("IterGridView"), RadGrid).ItemCreated, New GridItemEventHandler(AddressOf Me.IterGridView_ItemCreated)
        End If
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
            'e.Item.Style.Add("position", "relative")
            'e.Item.Style.Add("bottom", "expression(this.offsetParent.scrollHeight - this.offsetParent.scrollTop - this.offsetParent.clientHeight-1)")
            'e.Item.Style.Add("z-index", "99")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA INTERNA"

    Protected Sub IterGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Dim parentItem As Telerik.Web.UI.GridDataItem = CType(CType(CType(sender, Telerik.Web.UI.RadGrid).NamingContainer, Telerik.Web.UI.GridNestedViewItem).ParentItem, Telerik.Web.UI.GridDataItem)
        Dim id As Integer = parentItem.GetDataKeyValue("ID")
        CType(sender, Telerik.Web.UI.RadGrid).DataSource = (New IstanzaRepository).GetTaskIstanze(id)
    End Sub

    Protected Sub IterGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click

        Me.Filters = New List(Of FilterInfo)

        For Each col As GridColumn In Me.IstanzaGridView.MasterTableView.Columns
            col.CurrentFilterValue = String.Empty
        Next
        Me.IstanzaGridView.MasterTableView.FilterExpression = String.Empty
        Me.ResettaFiltro()
        Me.VisualizzaFiltroCorrente()
        Me.AggiornaGriglia()



    End Sub

    'Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
    '    Me.AggiornaGriglia()
    'End Sub

    Protected Sub EsportaPdfButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EsportaPdfButton.Click
        If Me.Istanze Is Nothing Then
            ParsecUtility.Utility.MessageBox("Non ci sono istanze storiche, non si può esportare nulla", False)
        Else
            If Me.Istanze.Count > 0 Then
                Dim parametriStampa As New Hashtable
                parametriStampa.Add("TipologiaStampa", "ScrivaniaStorico")
                parametriStampa.Add("DatiStampa", Me.Istanze)
                Session("ParametriStampa") = parametriStampa
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
                ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
            Else
                ParsecUtility.Utility.MessageBox("Non ci sono istanze storiche, non si può esportare nulla", False)
            End If
        End If
    End Sub

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Me.VisualizzaFiltroCorrente()
        Me.AggiornaGriglia()
        'Me.ResettaFiltro()
    End Sub

    Protected Sub AnnullaButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

    Protected Sub ResettaFiltroButton_Click(sender As Object, e As System.EventArgs) Handles ResettaFiltroButton.Click
        Me.ResettaFiltro()
    End Sub

    Protected Sub ToolBarFiltri_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles ToolBarFiltri.ItemCommand
        Dim id As Integer = CInt(e.CommandArgument)
        Dim filtro As Filtro = Filtri.Where(Function(c) c.Id = id).FirstOrDefault

        If filtro.Id = TipologiaFiltro.DataInizio Then
            If Me.Filtri.Where(Function(c) c.Id = TipologiaFiltro.DataFine).FirstOrDefault Is Nothing Then
                ParsecUtility.Utility.MessageBox("E' obbligatorio almeno una tra la 'Data Inizio da' e la 'Data Inizio a'!", False)
                Exit Sub
            End If
        End If

        If filtro.Id = TipologiaFiltro.DataFine Then
            If Me.Filtri.Where(Function(c) c.Id = TipologiaFiltro.DataInizio).FirstOrDefault Is Nothing Then
                ParsecUtility.Utility.MessageBox("E' obbligatorio almeno una tra la 'Data Inizio da' e la 'Data Inizio a'!", False)
                Exit Sub
            End If
        End If

        Me.Filtri.Remove(filtro)
        Select Case CType(id, TipologiaFiltro)
            Case TipologiaFiltro.Modulo
                Me.ModuliComboBox.SelectedIndex = 0
            Case TipologiaFiltro.Tipologia
                Me.TipologiaDocumentoComboBox.SelectedIndex = 0
            Case TipologiaFiltro.DataInizio
                Me.DataInizioIstanzaFiltroTextBox.SelectedDate = Nothing
            Case TipologiaFiltro.DataFine
                Me.DataFineIstanzaFiltroTextBox.SelectedDate = Nothing
            Case TipologiaFiltro.StatoIter
                Me.StatoIterListBox.ClearChecked()
        End Select
        Me.AggiornaGriglia()

    End Sub

    Protected Sub ModuliComboBox_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ModuliComboBox.SelectedIndexChanged
        Dim idModulo As Nullable(Of Integer) = Nothing
        If Me.ModuliComboBox.SelectedIndex > 0 Then
            idModulo = CInt(e.Value)
        End If
        Me.CaricaModelliWorkflow(idModulo)
    End Sub



    Protected Sub DataInizioIstanzaFiltroTextBox_SDC(ByVal sender As Object, ByVal e As Calendar.SelectedDateChangedEventArgs) Handles DataInizioIstanzaFiltroTextBox.SelectedDateChanged
        If Not Me.SalvaButton.Enabled Then
            Me.SalvaButton.Enabled = Me.DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue
            Me.SalvaFiltroButton.Enabled = Me.DataInizioIstanzaFiltroTextBox.SelectedDate.HasValue
        End If
    End Sub

    Protected Sub DataFineIstanzaFiltroTextBox_SDC(ByVal sender As Object, ByVal e As Calendar.SelectedDateChangedEventArgs) Handles DataFineIstanzaFiltroTextBox.SelectedDateChanged
        If Not Me.SalvaButton.Enabled Then
            Me.SalvaButton.Enabled = Me.DataFineIstanzaFiltroTextBox.SelectedDate.HasValue
            Me.SalvaFiltroButton.Enabled = Me.DataFineIstanzaFiltroTextBox.SelectedDate.HasValue
        End If
    End Sub

    Protected Sub SalvaFiltroButton_Click(sender As Object, e As System.EventArgs) Handles SalvaFiltroButton.Click
        SaveCookie()
    End Sub

#End Region

    Protected Sub EsportaInExcelImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EsportaInExcelImageButton.Click
        Me.Esporta()
    End Sub



    Private Sub Esporta()
        Dim lista = Me.FiltraIstanzeStoriche

        If lista.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Non ci sono istanze storiche." & vbCrLf & "Impossibile eseguire l'esportazione!", False)
            Exit Sub
        End If

        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("Istanze_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))

        Dim pathExport As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)

        Dim line As New StringBuilder


        line.Append("DOCUMENTO" & vbTab)
        line.Append("PROPONENTE/DESTINATARIO" & vbTab)
        line.Append("DATA INIZIO" & vbTab)
        line.Append("STATO" & vbTab)


        swExport.WriteLine(line.ToString)
        line.Clear()

        Dim data As String = String.Empty

        For Each istanza As ParsecWKF.Istanza In lista
            line.Append(If(Not String.IsNullOrEmpty(istanza.DescrizioneRiferimento), istanza.DescrizioneRiferimento.Replace(vbCrLf, " "), String.Empty) & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(istanza.Proponente), istanza.Proponente.Replace(vbCrLf, " "), String.Empty) & vbTab)
            data = String.Format("{0:dd/MM/yyyy}", istanza.DataInserimento)
            line.Append(data & vbTab)
            line.Append(If(Not String.IsNullOrEmpty(istanza.Stato), istanza.Stato.Replace(vbCrLf, " "), String.Empty) & vbTab)
            swExport.WriteLine(line.ToString)
            line.Clear()
        Next


        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)

        Me.EsportaInExcelImageButton.Enabled = True
    End Sub
End Class