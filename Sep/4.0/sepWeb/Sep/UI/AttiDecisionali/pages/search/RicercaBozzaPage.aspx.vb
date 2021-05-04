Imports Telerik.Web.UI

Partial Class RicercaBozzaPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"


    Private Property Bozze() As List(Of ParsecAtt.Bozza)
        Get
            Return CType(Session("RicercaBozzaPage_Bozze"), List(Of ParsecAtt.Bozza))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Bozza))
            Session("RicercaBozzaPage_Bozze") = value
        End Set
    End Property

    Private Property Documenti() As List(Of ParsecAtt.Documento)
        Get
            Return CType(Session("RicercaBozzaPage_Documenti"), List(Of ParsecAtt.Documento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Documento))
            Session("RicercaBozzaPage_Documenti") = value
        End Set
    End Property


    Protected Function DataDocumento(ByVal container As GridItem) As System.Nullable(Of DateTime)
        If container.OwnerTableView.GetColumn("DataDocumento").CurrentFilterValue = String.Empty Then
            Return New System.Nullable(Of DateTime)()
        Else
            Try
                'Siccome la funzione è Between prendo la prima data
                Return DateTime.Parse(container.OwnerTableView.GetColumn("DataDocumento").CurrentFilterValue.Split(" ")(0))
            Catch ex As Exception
                Return New System.Nullable(Of DateTime)()
            End Try

        End If
    End Function

#End Region

#Region "EVENTI PAGINA"


    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        Dim contatoreBozze As String = If(Me.Bozze.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Bozze.Count.ToString & ")</span>", "")
        Dim contatoreDetermine As String = If(Me.Documenti.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Documenti.Count.ToString & ")</span>", "")

        Me.TitoloDetermineLabel.Text = "Elenco Determine&nbsp;&nbsp;&nbsp;" & contatoreDetermine
        Me.TitoloBozzeLabel.Text = "Elenco Bozze&nbsp;&nbsp;&nbsp;" & contatoreBozze

        Me.DocumentiTabStrip.Tabs(0).Text = "Bozze " & contatoreBozze
        Me.DocumentiTabStrip.Tabs(1).Text = "Determine" & contatoreDetermine

        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")

    End Sub


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '*********************************************************************************
        'La prima volta imposto lo stato predefinito della pagina.
        '*********************************************************************************
        If Not Me.Page.IsPostBack Then
            Me.Documenti = Nothing
            Me.Bozze = Nothing
        End If
        '*********************************************************************************


        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") OrElse browser.ToLower.Contains("explorer") Then
            widthStyle = "100%"
        End If

        Me.DocumentiMultiPage.Style.Add("width", widthStyle)


        Me.BozzePanel.Style.Add("width", widthStyle)
        Me.BozzeGridView.Style.Add("width", widthStyle)

        Me.DeterminePanel.Style.Add("width", widthStyle)
        Me.DetermineGridView.Style.Add("width", widthStyle)

        '*********************************************************************************
        'Registro gli script usati per instanziare il componente ParsecOpenOffice.
        '*********************************************************************************

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If
        '*********************************************************************************

    End Sub



    'Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
    '    ParsecUtility.Utility.ClosePopup(True)
    'End Sub



#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub BozzeGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles BozzeGridView.NeedDataSource
        If Me.Bozze Is Nothing Then
            Dim bozze As New ParsecAtt.BozzeRepository
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim filtro As New ParsecAtt.FiltroBozza With {.Utente = utenteCorrente}
            Me.Bozze = bozze.GetView(filtro)
            bozze.Dispose()
        End If
        Me.BozzeGridView.DataSource = Me.Bozze
    End Sub


    Protected Sub BozzeGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles BozzeGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.SelezionaBozza(e.Item)
            Case "VisualizzaDocumento"
                Me.VisualizzaBozza(e.Item)
        End Select
    End Sub

    Protected Sub BozzeGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles BozzeGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub


    Protected Sub DetermineGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DetermineGridView.NeedDataSource
        If Me.Documenti Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim filtro As New ParsecAtt.FiltroDocumento With {.IdUtente = utenteCorrente.Id, .ApplicaAbilitazione = True, .IdTipologiaDocumento = ParsecAtt.TipoDocumento.Determina, .UltimeCinque = False, .PubblicazioneAlbo = False}
            Me.Documenti = documenti.GetView(filtro)
            documenti.Dispose()
        End If
        Me.DetermineGridView.DataSource = Me.Documenti
    End Sub


    Protected Sub DetermineGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DetermineGridView.ItemCommand

        Select Case e.CommandName
            Case "Select"
                Me.SelezionaDetermina(e.Item)
            Case "VisualizzaDocumento"
                Me.VisualizzaDocumento(e.Item)
        End Select

    End Sub

    Protected Sub DetermineGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DetermineGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub


#End Region

#Region "METODI PRIVATI"

    Private Sub RegisterComponent(ByVal script As String)
        Dim cell As New TableCell
        cell.Width = New Unit(30)
        cell.HorizontalAlign = HorizontalAlign.Center
        cell.Controls.Add(New LiteralControl(script))
        Me.componentPlaceHolder.Rows(0).Cells.Add(cell)
    End Sub

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.RegisterComponent(script)
    End Sub

    Private Sub SelezionaBozza(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Dim bozze As New ParsecAtt.BozzeRepository
        'Dim bozza As ParsecAtt.Bozza = bozze.GetById(id)
        'bozze.Dispose()

        Dim bozza As ParsecAtt.Bozza = Me.Bozze.Where(Function(c) c.Id = id).FirstOrDefault

        ParsecUtility.SessionManager.Bozza = bozza
        ParsecUtility.Utility.ClosePopup(True)
    End Sub


    Private Sub VisualizzaBozza(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id"))

        'Dim bozze As New ParsecAtt.BozzeRepository
        'Dim bozza As ParsecAtt.Bozza = bozze.GetById(id)
        'bozze.Dispose()

        Dim bozza As ParsecAtt.Bozza = Me.Bozze.Where(Function(c) c.Id = id).FirstOrDefault

        If Not bozza Is Nothing Then
            Dim nomefile As String = bozza.Nomefile

            Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathBozze") & bozza.Nomefile

            If IO.File.Exists(localPath) Then
                Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeBozze") & nomefile
                Me.VisualizzaDocumento(pathDownload, False)
            Else
                ParsecUtility.Utility.MessageBox("Il file della bozza selezionata non esiste!", False)
            End If
        End If

    End Sub


    Private Sub SelezionaDetermina(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        'Dim documenti As New ParsecAtt.DocumentoRepository
        'Dim documento As ParsecAtt.Documento = documenti.GetById(id)
        'documenti.Dispose()

        Dim documento As ParsecAtt.Documento = Me.Documenti.Where(Function(c) c.Id = id).FirstOrDefault

        ParsecUtility.SessionManager.Bozza = documento
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

    Private Sub VisualizzaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id"))

        'Dim documenti As New ParsecAtt.DocumentoRepository
        'Dim documento = documenti.GetFullById(id)
        'documenti.Dispose()

        Dim documento As ParsecAtt.Documento = Me.Documenti.Where(Function(c) c.Id = id).FirstOrDefault

        If Not documento Is Nothing Then
            Dim nomefile As String = documento.Nomefile
            Dim annoEsercizio As Integer = documento.Data.Value.Year
            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefile)
            If IO.File.Exists(localPath) Then

                Dim pathDownload = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomefile)

                Me.VisualizzaDocumento(pathDownload, False)
            Else
                ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
            End If
        End If

    End Sub

    Private Sub VisualizzaDocumento(ByVal remotePath As String, ByVal enabled As Boolean)
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim datiInput As ParsecAdmin.DatiInput

        datiInput = New ParsecAdmin.DatiInput With {.SrcRemotePath = remotePath, .ShowWindow = True, .Enabled = enabled, .FunctionName = "ViewDocument"}

        Dim data As String = openofficeParameters.CreateOpenParameter(datiInput)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)
        Else
            'UTILIZZO IL SOCKET  
            ParsecUtility.Utility.EseguiServerComunicatorService(data, False, Nothing)
        End If

    End Sub

#End Region

End Class