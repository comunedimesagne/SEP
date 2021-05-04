Partial Class GestioneSedutePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadScheduler_Office2007 .rsMonthView .rsTodayCell{background-color: #C6C6FF;color: #000;border: 1px solid #000;}"
        'css.InnerHtml &= ".rsDateWrap {height: 0px !important;}"
        'css.InnerHtml &= ".rsWrap {height: 40px !important;}"
        'css.InnerHtml &= ".rsFirstWrap { margin-top: 0px !important;}"
        css.InnerHtml &= ".RadScheduler.rsMonthView.rsApt {font-size:10px !important;  height:16px !important;}"
        'css.InnerHtml &= ".RadScheduler .rsMonthView .rsAptContent {border: 1px solid red !important; min-height: 35px !important; left: -2px !important; right:-2px !important;}"

        Me.Page.Header.Controls.Add(css)


        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Gestione Sedute"

        If Not Me.Page.IsPostBack Then
            Me.InizializzaScheduler()
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.AggiornaScheduler()
    End Sub

#End Region


#Region "METODI PRIVATI"

    Private Sub InizializzaScheduler()
        Me.SeduteScheduler.DataKeyField = "Id"
        Me.SeduteScheduler.DataStartField = "DataInizio"
        Me.SeduteScheduler.DataEndField = "DataFine"
        Me.SeduteScheduler.DataSubjectField = "DescrizioneSeduta"
    End Sub

    Private Sub AggiornaScheduler()
        Dim sedute As New ParsecAtt.SedutaRepository
        Me.SeduteScheduler.DataSource = sedute.GetView(Nothing)
        sedute.Dispose()
    End Sub

  
    Private Sub VisualizzaDettaglioSeduta()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/SedutaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaPaginaButton.ClientID)
        queryString.Add("IdSeduta", Me.IdSedutaHidden.Value)
        queryString.Add("DataSelezionata", Me.dataAppuntamentoHidden.Value)
        ParsecUtility.Utility.ShowRadWindow(pageUrl, "SedutaRadWindow", queryString, False)
    End Sub

    Private Sub Delete(ByVal appointment As Telerik.Web.UI.Appointment)
        Dim sedute As New ParsecAtt.SedutaRepository
        Dim idSeduta As Integer = CInt(appointment.ID.ToString)
        Dim seduta As ParsecAtt.Seduta = sedute.GetQuery.Where(Function(c) c.Id = idSeduta).FirstOrDefault
        If Not seduta Is Nothing Then
            sedute.Delete(seduta)
        End If
        sedute.Dispose()
    End Sub

#End Region


#Region "EVENTI CONTROLLI"

    Protected Sub AggiornaPaginaButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaPaginaButton.Click
        Me.AggiornaScheduler()
    End Sub

    Protected Sub VisualizzaSedutaButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaSedutaButton.Click
        Me.VisualizzaDettaglioSeduta()
    End Sub

#End Region

#Region "EVENTI SCHEDULER"

#End Region

    Protected Sub SeduteScheduler_AppointmentDelete(ByVal sender As Object, ByVal e As Telerik.Web.UI.AppointmentDeleteEventArgs) Handles SeduteScheduler.AppointmentDelete
        Try
            Me.Delete(e.Appointment)
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox("La seduta selezionata non può essere cancellata per il seguente motivo: " & vbCrLf & ex.Message, False)
        End Try
    End Sub

    Protected Sub SeduteScheduler_AppointmentDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.SchedulerEventArgs) Handles SeduteScheduler.AppointmentDataBound
        Dim seduta As ParsecAtt.Seduta = DirectCast(e.Appointment.DataItem, ParsecAtt.Seduta)
        Select Case seduta.TipologiaSeduta
            Case ParsecAtt.TipologiaOrganoDeliberante.ConsiglioComunale
                e.Appointment.CssClass = "rsCategoryGreen"
            Case ParsecAtt.TipologiaOrganoDeliberante.GiuntaComunale
                e.Appointment.CssClass = "rsCategoryOrange"
            Case ParsecAtt.TipologiaOrganoDeliberante.CommissarioPrefettizio
                e.Appointment.CssClass = "rsCategoryBlue"
            Case ParsecAtt.TipologiaOrganoDeliberante.SubCommissarioPrefettizio
                e.Appointment.CssClass = "rsCategoryYellow"
        End Select

    End Sub

   

End Class
