Imports ParsecAtt
Imports System.Transactions
Imports Telerik.Web.UI

Partial Class GestioneBozzePage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    Public Property Bozza() As ParsecAtt.Bozza
        Get
            Return CType(Session("GestioneBozzePage_Bozza"), ParsecAtt.Bozza)
        End Get
        Set(ByVal value As ParsecAtt.Bozza)
            Session("GestioneBozzePage_Bozza") = value
        End Set
    End Property

    Public Property Bozze() As List(Of ParsecAtt.Bozza)
        Get
            Return CType(Session("GestioneBozzePage_Bozze"), List(Of ParsecAtt.Bozza))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Bozza))
            Session("GestioneBozzePage_Bozze") = value
        End Set
    End Property

#End Region


#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Atti Decisionali"
        MainPage.DescrizioneProcedura = "> Gestione Bozze"

        If Not Me.Page.IsPostBack Then
            Me.Bozze = Nothing
            Me.ResettaVista()

            Dim sortExpr As New GridSortExpression()
            sortExpr.FieldName = "Descrizione"
            sortExpr.SortOrder = GridSortOrder.Ascending
            Me.BozzeGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)

        End If

        Me.RegistraParsecOpenOffice()
        Me.RegistraParsecDigitalSign()

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare la bozza selezionata?", False, Not Me.Bozza Is Nothing)

        If Not Me.Bozze Is Nothing Then
            Me.TitoloElencoBozzeLabel.Text = "Elenco Bozze&nbsp;&nbsp;&nbsp;" & If(Me.Bozze.Count > 0, "( " & Me.Bozze.Count.ToString & " )", "")
        End If
        Me.ChiudiButton.Attributes.Add("onclick", "HidePanel();return false;")
        Me.StampaImageButton.Attributes.Add("onclick", "HidePanel();return false;")
    End Sub

#End Region


#Region "EVENTI TOOLBAR"


    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                ' Me.Print()
            Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    Me.AggiornaGriglia()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)
            Case "Nuovo"
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Annulla"
                Me.ResettaVista()
                Me.AggiornaGriglia()

            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Bozza Is Nothing Then
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.AggiornaGriglia()
                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)
                        End Try
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare una bozza!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                Dim bozze As New ParsecAtt.BozzeRepository
                Dim filtro As New ParsecAtt.FiltroBozza With {.Utente = utenteCorrente, .Descrizione = Me.DescrizioneTextBox.Text}
                Me.Bozze = bozze.GetView(filtro)
                Me.BozzeGridView.Rebind()
                bozze.Dispose()
        End Select
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If

        If e.Item.Text = "Stampa" Then
            e.Item.Attributes.Add("onclick", "ShowPanel();")
        End If
    End Sub

#End Region


#Region "EVENTI GRIGLIA"

    Protected Sub BozzeGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles BozzeGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.Anteprima(e.Item)
        End If
    End Sub

  
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


    Protected Sub BozzeGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles BozzeGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region



#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub VisualizzaDocumento(ByVal nomeFile As String, ByVal enabled As Boolean)
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeBozze") & nomeFile
        Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = "ViewDocument"}
        Dim data As String = openofficeParameters.CreateOpenParameter(datiInput)
        ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)
    End Sub



    Private Sub FirmaDocumento(ByVal nomeFile As String)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim certificateId As String = utenteCorrente.IdCertificato
        Dim provider As ParsecAdmin.ProviderType = CType(utenteCorrente.IdProviderFirma, ParsecAdmin.ProviderType)

        Dim pathFileToSign As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeBozze") & nomeFile

        Dim signParameters As New ParsecAdmin.SignParameters

        'Dim datiInput As New ParsecAdmin.DatiFirma With {
        '    .RemotePathToSign = pathFileToSign,
        '    .Provider = provider,
        '    .FunctionName = "CoSignDocument",
        '    .CertificateId = certificateId
        '}

        Dim datiBiometrici As New ParsecAdmin.DatiFirmaBiometrica
        datiBiometrici.DatiAggiuntiviFirma = New ParsecAdmin.DatiAggiuntiviFirma With {.PdfSignInfoReason = "MioMotivo", .PdfSignInfoLocation = "MiaCitta", .PdfSignInfoContactInfo = "MioContatto"}

        Dim datiInput As New ParsecAdmin.DatiFirma With {
           .RemotePathToSign = pathFileToSign,
           .FunctionType = ParsecAdmin.FunctionType.BiometricSignDocument,
           .BiometricaData = datiBiometrici
           }


        Dim data As String = signParameters.CreaDataSource(datiInput)
        ParsecUtility.Utility.RegistraTimerEseguiFirma(data, FirmaButton.ClientID, False, False)

    End Sub

  



    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

    Private Sub RegistraParsecDigitalSign()
        Dim script As String = ParsecAdmin.SignParameters.RegistraParsecDigitalSign
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region





    Private Sub AggiornaGriglia()
        Me.Bozze = Nothing
        Me.BozzeGridView.Rebind()
    End Sub


    Private Sub Anteprima(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim bozze As New ParsecAtt.BozzeRepository
        Dim bozza As ParsecAtt.Bozza = bozze.GetById(id)
        Dim localPathBozze As String = ParsecAdmin.WebConfigSettings.GetKey("PathBozze") & bozza.Nomefile
        If Not IO.File.Exists(localPathBozze) Then
            ParsecUtility.Utility.MessageBox("Il file della bozza non esiste!", False)
        Else
            Me.VisualizzaDocumento(bozza.Nomefile, True)
            'Me.FirmaDocumento("Prova.pdf")
        End If
        bozze.Dispose()
    End Sub



    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()


        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim bozze As New ParsecAtt.BozzeRepository
        Dim bozza As ParsecAtt.Bozza = bozze.GetById(id)

        Me.DescrizioneTextBox.Text = bozza.Descrizione
        Me.UtenteTextBox.Text = bozza.Utente
        Me.DataTextBox.Text = String.Format("{0:dd/MM/yyyy}", bozza.Data)
        If Not bozza.VisibilitaPubblica Is Nothing Then
            Me.VisibilitaCheckBox.Checked = bozza.VisibilitaPubblica
        End If

        Me.Bozza = bozza

        bozze.Dispose()
    End Sub



    Private Sub Print()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaBozzePage.aspx"
        Dim queryString As New Hashtable
        ParsecUtility.Utility.ShowRadWindow(pageUrl, "StampaBozzeRadWindow", queryString, False)
    End Sub


    Private Sub Print(ByVal bozze As List(Of ParsecAtt.Bozza))
        Dim parametriStampa As New Hashtable
        parametriStampa.Add("TipologiaStampa", "StampaBozze")
        parametriStampa.Add("DatiStampa", bozze)
        Session("parametriStampa") = parametriStampa
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaPage.aspx"
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Dim bozze As New ParsecAtt.BozzeRepository
        Try
            bozze.Delete(Me.Bozza.Id)
            bozze.Dispose()
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            bozze.Dispose()
            Dim localPathBozze As String = ParsecAdmin.WebConfigSettings.GetKey("PathBozze") & Me.Bozza.Nomefile
            If IO.File.Exists(localPathBozze) Then
                IO.File.Delete(localPathBozze)
            End If
        End Try
    End Sub

    Private Sub Save()
        Dim bozze As New ParsecAtt.BozzeRepository
        Dim bozza As ParsecAtt.Bozza = bozze.CreateFromInstance(Me.Bozza)
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'Aggiorno il modello
        bozza.Descrizione = Me.DescrizioneTextBox.Text.Trim
        bozza.Data = Now
        bozza.VisibilitaPubblica = Me.VisibilitaCheckBox.Checked
        bozza.IdUtente = utenteCorrente.Id

        Try
            bozze.Save(bozza)
            Me.Bozza = bozze.Bozza

            Dim localPathModello As String = ParsecAdmin.WebConfigSettings.GetKey("PathModelliDocumento")
            Dim localPathBozza As String = ParsecAdmin.WebConfigSettings.GetKey("PathBozze")

            Dim srcLocalPath As String = localPathModello & "Empty.odt"

            Dim destLocalPath As String = localPathBozza & Me.Bozza.Nomefile
            Me.CopiaDocumento(srcLocalPath, destLocalPath)


        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            bozze.Dispose()
        End Try

    End Sub

    Private Sub CopiaDocumento(ByVal input As String, ByVal output As String)
        Try
            If Not IO.File.Exists(output) Then
                IO.File.Copy(input, output)
                If (IO.File.GetAttributes(output) And IO.FileAttributes.ReadOnly) = IO.FileAttributes.ReadOnly Then
                    IO.File.SetAttributes(output, IO.FileAttributes.Normal)
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub ResettaVista()
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Me.Bozza = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
        Me.DataTextBox.Text = String.Format("{0:dd/MM/yyyy}", Now)
        Dim descrizioneUtente As String = (If(utenteCorrente.Username = Nothing, "", utenteCorrente.Username) + " - " + If(utenteCorrente.Cognome = Nothing, "", utenteCorrente.Cognome) + " " + If(utenteCorrente.Nome = Nothing, "", utenteCorrente.Nome))
        Me.UtenteTextBox.Text = descrizioneUtente
        Me.VisibilitaCheckBox.Checked = False
    End Sub

    Private Sub ResettaFiltroStampa()
        Me.NumeroInizioTextBox.Text = String.Empty
        Me.NumeroFineTextBox.Text = String.Empty
        Me.DataInizioTextBox.SelectedDate = Nothing
        Me.DataFineTextBox.SelectedDate = Nothing
    End Sub


    Protected Sub StampaImageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaImageButton.Click
        Dim filtro As ParsecAtt.FiltroBozza = Me.GetFiltro
        Dim bozze As New ParsecAtt.BozzeRepository
        Dim view As List(Of ParsecAtt.Bozza) = bozze.GetView(filtro)
        bozze.Dispose()
        Me.Print(view)
        Me.ResettaFiltroStampa()
    End Sub

    Private Function GetFiltro() As ParsecAtt.FiltroBozza
        Dim filtro As New ParsecAtt.FiltroBozza

        Dim numeroInizio As Integer = 0
        If Not String.IsNullOrEmpty(Me.NumeroInizioTextBox.Text) Then
            Integer.TryParse(Me.NumeroInizioTextBox.Text, numeroInizio)
            If numeroInizio <> 0 Then
                filtro.NumeroInizio = numeroInizio
            End If
        End If

        Dim numeroFine As Integer = 0
        If Not String.IsNullOrEmpty(Me.NumeroFineTextBox.Text) Then
            Integer.TryParse(Me.NumeroFineTextBox.Text, numeroFine)
            If numeroFine <> 0 Then
                filtro.NumeroFine = numeroFine
            End If
        End If

        filtro.DataInizio = Me.DataInizioTextBox.SelectedDate
        filtro.DataFine = Me.DataFineTextBox.SelectedDate

        Return filtro
    End Function

    Protected Sub ChiudiButton_Click(sender As Object, e As System.EventArgs) Handles ChiudiButton.Click
        Me.ResettaFiltroStampa()
    End Sub

    Protected Sub FirmaButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles FirmaButton.Click

    End Sub
End Class