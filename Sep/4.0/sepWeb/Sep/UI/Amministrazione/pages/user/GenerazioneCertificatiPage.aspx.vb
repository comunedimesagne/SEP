Imports ParsecAdmin
Imports Telerik.Web.UI


Partial Class GenerazioneCertificatiPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage


    Public Class UtenteInfo
        Public Property Id As Integer = 0
        Public Property Nominativo As String = String.Empty
        Public Property Username As String = String.Empty
        Public Property ErroreElaborazione As String = String.Empty
        Public Property Processato As Boolean = False
    End Class

#Region "PROPRIETA'"

    Public Property UtentiInfo() As List(Of UtenteInfo)
        Get
            Return CType(Session("GenerazioneCertificatiPage_UtenteInfo"), List(Of UtenteInfo))
        End Get
        Set(ByVal value As List(Of UtenteInfo))
            Session("GenerazioneCertificatiPage_UtenteInfo") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Amministrazione"
        Me.MainPage.DescrizioneProcedura = "> Generazione Certificati"

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = "div.RadUploadProgressArea_Office2007 .ruProgress { background-image: none;}" & vbCrLf
        css.InnerHtml += ".RadUploadProgressArea { width: 320px !important;}" & vbCrLf
        css.InnerHtml += "div.RadUploadProgressArea li.ruProgressHeader{ margin: 10px 18px 0px; }" & vbCrLf
        Me.Page.Header.Controls.Add(css)

        If Not Me.Page.IsPostBack Then
            Me.UtentiInfo = Nothing
        End If


        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.UtentiGridView.Style.Add("width", widthStyle)
        Me.GrigliaUtentiPanel.Style.Add("width", widthStyle)
     

    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Dim utenti As New ParsecAdmin.UserRepository
        Dim pathCertificati As String = ParsecAdmin.WebConfigSettings.GetKey("PathCertificati")

        If Not IO.Directory.Exists(pathCertificati) Then
            IO.Directory.CreateDirectory(pathCertificati)
        End If

        Dim sb As New StringBuilder
        Dim i As Integer = 0
        Dim percentCompleted As Integer = 0
        Dim count = Me.UtentiInfo.Count
        Dim context = RadProgressContext.Current
        context.PrimaryTotal = count.ToString

        Dim idUtente As Integer = 0
        Dim nomefileCertificato As String = String.Empty
        Dim fullPath As String = String.Empty
        Dim n = 0

        For Each info In Me.UtentiInfo

            Threading.Thread.Sleep(100)
            i += 1
            context.PrimaryValue = i.ToString
            percentCompleted = CInt((0.5F + ((100.0F * i) / count)))
            context.PrimaryPercent = percentCompleted.ToString

            If Not Response.IsClientConnected Then
                Exit For
            End If

            idUtente = info.Id
            nomefileCertificato = String.Format("Certificato{0}{1}", info.Id.ToString.PadLeft(7, "0"), ".cer")
            Try
                info.Processato = True
                'If i = 1 Then
                '    Dim d = 2 \ n
                'End If

                fullPath = pathCertificati & nomefileCertificato
                'If Not IO.File.Exists(fullPath) Then
                Dim utenteCorrente = utenti.Where(Function(c) c.Id = idUtente).FirstOrDefault
                If Not utenteCorrente Is Nothing Then

                    Me.CreaCertificato(utenteCorrente, fullPath)
                    utenteCorrente.NomefileCertificato = nomefileCertificato
                    utenti.SaveChanges()
                End If
                'End If
            Catch ex As Exception

                If sb.Length = 0 Then
                    sb.AppendLine("Riscontrati errori durante l'elaborazione!" & vbCrLf & "Posizionare il puntatore del mouse sull'icona rossa " & vbCrLf & "con un punto esclamativo per visualizzare la descrizione dell'errore")
                End If

                If ex.InnerException Is Nothing Then
                    info.ErroreElaborazione = ex.Message
                Else
                    info.ErroreElaborazione = ex.InnerException.Message
                End If

            End Try

        Next

        context.OperationComplete = True

        If sb.Length = 0 Then
            Me.infoOperazioneHidden.Value = "Elaborazione conclusa con successo!"
        Else
            ParsecUtility.Utility.MessageBox(sb.ToString, False)
        End If

        utenti.Dispose()

        Me.UtentiGridView.Rebind()
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub UtentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles UtentiGridView.NeedDataSource
        If Me.UtentiInfo Is Nothing Then
            Me.UtentiInfo = Me.GetUtentiInfo
        End If

        Me.UtentiGridView.DataSource = Me.UtentiInfo

        Me.UtentiLabel.Text = "Utenti&nbsp;&nbsp;&nbsp;" & If(Me.UtentiInfo.Count > 0, "( " & Me.UtentiInfo.Count.ToString & " )", "")
    End Sub

    Private Sub UtentiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles UtentiGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub UtentiGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles UtentiGridView.ItemDataBound

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            Dim statoButton As ImageButton = Nothing
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim info As UtenteInfo = CType(e.Item.DataItem, UtenteInfo)

            If TypeOf dataItem("StatoElaborazione").Controls(0) Is ImageButton Then
                statoButton = CType(dataItem("StatoElaborazione").Controls(0), ImageButton)

                statoButton.Enabled = False

                If info.Processato Then
                    If Not String.IsNullOrEmpty(info.ErroreElaborazione) Then
                        statoButton.ImageUrl = "~\images\alert16.png"
                        statoButton.ToolTip = info.ErroreElaborazione
                    Else
                        statoButton.ImageUrl = "~\images\Successo16.png"
                        statoButton.ToolTip = String.Empty
                    End If
                Else
                    statoButton.ImageUrl = "~\images\vuoto.png"
                    statoButton.ToolTip = String.Empty
                End If
            End If
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

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
        ParsecUtility.SelfSignCertificate.Create(fullPath, utente.Cognome, utente.Nome, utente.Email, organizzazione, commonName, utente.PasswordHash)
        'ParsecUtility.SelfSignCertificate.Create(fullPath, utente.Cognome, utente.Nome, utente.Email, organizzazione, commonName, Now, New DateTime(Now.Year + 5, Now.Month, Now.Day), utente.PasswordHash)
    End Sub

    Private Function GetUtentiInfo() As List(Of UtenteInfo)

        'And c.NomefileCertificato Is Nothing

        Dim utenti As New ParsecAdmin.UserRepository
        Dim view = utenti.Where(Function(c) c.LogTipoOperazione Is Nothing)


        Dim query = From utente In view.ToList
                       Order By (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
                        Select New UtenteInfo With
                               {
                                   .Id = utente.Id,
                                   .Nominativo = (If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome)),
                                   .Username = utente.Username
                                  }

        utenti.Dispose()
        Return query.ToList


    End Function

#End Region

End Class