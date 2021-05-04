Imports ParsecAdmin
Imports Telerik.Web.UI

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StatisticheRegistrazioniProtocolloPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    'Classe di Appoggio: filtro per la ricerca
    Public Class FiltroStatistica

        Public Property DataInizio As Nullable(Of DateTime) = Nothing
        Public Property DataFine As Nullable(Of DateTime) = Nothing

    End Class

    'Enum per i Raggrupapmenti della Statistica
    Public Enum TipoRaggruppamentoStatistica
        Nessuno = 0
        Struttura = 1
        Operatore = 2
        Settore = 3
    End Enum

    'Classe di Appoggio: oggetto Statistica
    Public Class Statistica
        Public Property Id As String
        Public Property Descrizione As String

        Public Property ProtocolliAnnullati As Integer
        Public Property ProtocolliArrivo As Integer
        Public Property ProtocolliPartenza As Integer
        Public Property ProtocolliInterni As Integer

        Public Property Totale As Integer

    End Class

    'Variabile di Sessione: lista di oggetti Statistica associata alla griglia StatisticheGridView.
    Public Property Statistiche() As List(Of Statistica)
        Get
            Return CType(Session("StatisticheRegistrazioniProtocolloPage_Statistiche"), List(Of Statistica))
        End Get
        Set(ByVal value As List(Of Statistica))
            Session("StatisticheRegistrazioniProtocolloPage_Statistiche") = value
        End Set
    End Property

    'Evento Init associato alla Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Statistiche Registrazioni Protocollo"
        Me.EsportaExcelButton.Attributes.Add("onclick", "this.disabled=true;")
        If Not Me.Page.IsPostBack Then
            Me.ResettaFiltro()
        End If

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".CustomFooter .RadGrid .rgFooter { background-image: none;background-color:#DFE8F6;}" & vbCrLf
        css.InnerHtml &= " div.RadGrid .rgFilterRow td {  padding: 3px;}" & vbCrLf
        Me.Page.Header.Controls.Add(css)

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.StatisticheGridView.Style.Add("width", widthStyle)
    End Sub

    ' Costruisce e restituisce il filtroper la ricerca.
    Private Function GetFiltro() As FiltroStatistica
        Dim filtro As New FiltroStatistica
        filtro.DataInizio = Me.DataInizioTextBox.SelectedDate
        filtro.DataFine = Me.DataFineTextBox.SelectedDate
        Return filtro
    End Function

    'Restituisce l'Oggetto Raggruppamentoin base al contenuto dei campi StrutturaRadioButton, OperatoreRadioButton e SettoreRadioButton
    Private Function GetRaggruppamento() As TipoRaggruppamentoStatistica
        Dim raggruppamento = TipoRaggruppamentoStatistica.Nessuno

        If Me.StrutturaRadioButton.Checked Then
            raggruppamento = TipoRaggruppamentoStatistica.Struttura
        End If

        If Me.OperatoreRadioButton.Checked Then
            raggruppamento = TipoRaggruppamentoStatistica.Operatore
        End If
        If Me.SettoreRadioButton.Checked Then
            raggruppamento = TipoRaggruppamentoStatistica.Settore
        End If
        Return raggruppamento
    End Function

    'Resetta i campi della maschera
    Private Sub ResettaFiltro()
        Me.DataInizioTextBox.SelectedDate = New Date(Now.Year, 1, 1)
        Me.DataFineTextBox.SelectedDate = Now
        Me.Statistiche = Nothing
        Me.SettoreRadioButton.Checked = True
    End Sub

    'Calcola le statistiche 
    Private Function GetStatistiche(ByVal filtro As FiltroStatistica, ByVal raggruppamento As TipoRaggruppamentoStatistica) As List(Of Statistica)
        Dim lista As New List(Of Statistica)

        Dim registrazioni As New ParsecPro.RegistrazioniRepository


        Dim base = registrazioni.Where(Function(c) c.Modificato = False)


        If filtro.DataInizio.HasValue Then
            Dim dataInizio = filtro.DataInizio.Value
            Dim startDate As Date = New Date(dataInizio.Year, dataInizio.Month, dataInizio.Day, 0, 0, 0)
            base = base.Where(Function(c) c.DataImmissione >= startDate)
        End If

        If filtro.DataFine.HasValue Then
            Dim dataFine = filtro.DataFine.Value
            Dim endDate As Date = New Date(dataFine.Year, dataFine.Month, dataFine.Day, 23, 59, 59)
            base = base.Where(Function(c) c.DataImmissione <= endDate)
        End If

        If raggruppamento = TipoRaggruppamentoStatistica.Settore Then

            Dim strutture As New ParsecAdmin.StructureRepository
            Dim res = strutture.GetQuery.Select(Function(c) New With {.Id = c.Id, .IdPadre = c.IdPadre, .Descrizione = c.Descrizione})

            Dim dict As New Dictionary(Of Integer, String)

            For Each r In res.AsEnumerable
                If r.IdPadre = 0 Then
                    dict.Add(r.Id, r.Descrizione)
                Else

                    Dim t = res.Where(Function(c) c.Id = r.IdPadre).FirstOrDefault

                    If Not t Is Nothing Then
                        While t.IdPadre <> 0
                            t = res.Where(Function(c) c.Id = t.IdPadre).FirstOrDefault
                            If t Is Nothing Then
                                Exit While
                            End If
                        End While
                        dict.Add(r.Id, t.Descrizione)
                    End If


                End If
            Next


            Dim ids = dict.Select(Function(c) c.Key).ToList

            Dim referentiInterni As New ParsecPro.ReferentiInterniRepository(registrazioni.Context)



            Dim vista = (From registrazione In base
                        Join referenteInterno In referentiInterni.GetQuery
                        On registrazione.Id Equals referenteInterno.IdRegistrazione
                        Where ids.Contains(referenteInterno.Id)).AsEnumerable.Select(Function(c) New With {
                                                                                         .IdReferenteInterno = c.referenteInterno.Id,
                                                                                         .TipoRegistrazione = c.registrazione.TipoRegistrazione,
                                                                                         .Descrizione = dict(c.referenteInterno.Id),
                                                                                         .Annullato = c.registrazione.Annullato
                                                                                         })


            lista = (vista.GroupBy(Function(item) item.Descrizione).AsEnumerable.Select(Function(group) New Statistica With {
                                                                                                       .Id = group.Key,
                                                                                                       .Descrizione = group.FirstOrDefault.Descrizione,
                                                                                                       .ProtocolliAnnullati = group.Where(Function(c) c.Annullato = True).Count,
                                                                                                       .ProtocolliArrivo = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Arrivo).Count,
                                                                                                       .ProtocolliPartenza = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Partenza).Count,
                                                                                                       .ProtocolliInterni = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Interna).Count,
                                                                                                       .Totale = .ProtocolliArrivo + .ProtocolliPartenza + .ProtocolliAnnullati + .ProtocolliInterni
                                                                                                   })).ToList



        End If


        If raggruppamento = TipoRaggruppamentoStatistica.Struttura Then



            Dim referentiInterni As New ParsecPro.ReferentiInterniRepository(registrazioni.Context)
            Dim strutture As New ParsecPro.StrutturaViewRepository(registrazioni.Context)


            Dim vista = From registrazione In base
                        Join referenteInterno In referentiInterni.GetQuery
                        On registrazione.Id Equals referenteInterno.IdRegistrazione
                        Join struttura In strutture.GetQuery
                        On referenteInterno.Id Equals struttura.Id
                        Select IdReferenteInterno = referenteInterno.Id, TipoRegistrazione = registrazione.TipoRegistrazione, Descrizione = struttura.Descrizione, Annullato = registrazione.Annullato


            lista = (vista.GroupBy(Function(item) item.IdReferenteInterno).AsEnumerable.Select(Function(group) New Statistica With {
                                                                                                       .Id = group.Key,
                                                                                                       .Descrizione = group.FirstOrDefault.Descrizione,
                                                                                                       .ProtocolliAnnullati = group.Where(Function(c) c.Annullato = True).Count,
                                                                                                       .ProtocolliArrivo = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Arrivo).Count,
                                                                                                       .ProtocolliPartenza = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Partenza).Count,
                                                                                                       .ProtocolliInterni = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Interna).Count,
                                                                                                       .Totale = .ProtocolliArrivo + .ProtocolliPartenza + .ProtocolliAnnullati + .ProtocolliInterni
                                                                                                   })).ToList


        End If


        If raggruppamento = TipoRaggruppamentoStatistica.Operatore Then

            Dim utenti As New ParsecPro.UtenteViewRepository(registrazioni.Context)

            Dim vista = From registrazione In base
                        Join utente In utenti.GetQuery
                        On registrazione.IdUtente Equals utente.Id
                        Select IdUtente = utente.Id, TipoRegistrazione = registrazione.TipoRegistrazione, Descrizione = utente.Nome & " " & utente.Cognome, Annullato = registrazione.Annullato

            lista = (vista.GroupBy(Function(item) item.IdUtente).AsEnumerable.Select(Function(group) New Statistica With {
                                                                                                          .Id = group.Key,
                                                                                                          .Descrizione = group.FirstOrDefault.Descrizione,
                                                                                                          .ProtocolliAnnullati = group.Where(Function(c) c.Annullato = True).Count,
                                                                                                          .ProtocolliArrivo = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Arrivo).Count,
                                                                                                          .ProtocolliPartenza = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Partenza).Count,
                                                                                                          .ProtocolliInterni = group.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Interna).Count,
                                                                                                          .Totale = .ProtocolliArrivo + .ProtocolliPartenza + .ProtocolliAnnullati + .ProtocolliInterni
                                                                                                      })).ToList


        End If



        If raggruppamento = TipoRaggruppamentoStatistica.Nessuno Then

            Dim vista = From registrazione In base
                        Select Id = -1, TipoRegistrazione = registrazione.TipoRegistrazione, Descrizione = "", Annullato = registrazione.Annullato

            Dim v = (vista.AsEnumerable.Select(Function(item) New Statistica With {
                                                   .Id = item.Id,
                                                   .Descrizione = item.Descrizione,
                                                   .ProtocolliAnnullati = vista.Where(Function(c) c.Annullato = True).Count,
                                                   .ProtocolliArrivo = vista.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Arrivo).Count,
                                                   .ProtocolliPartenza = vista.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Partenza).Count,
                                                   .ProtocolliInterni = vista.Where(Function(c) c.Annullato = False And c.TipoRegistrazione = ParsecPro.TipoRegistrazione.Interna).Count,
                                                   .Totale = .ProtocolliArrivo + .ProtocolliPartenza + .ProtocolliAnnullati + .ProtocolliInterni
                                               })).FirstOrDefault

            If Not v Is Nothing Then
                lista.Add(v)
            End If




        End If

        Return lista

    End Function

    'Metodo di pulizia di un campo stringa 
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

    'Esporta in excel il contenuto della griglia
    Private Sub EsportaExcel()
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

        Dim statistiche = Me.Statistiche

        If statistiche Is Nothing Then
            Throw New ApplicationException("Non ci sono statistiche." & vbCrLf & "Impossibile eseguire l'esportazione!")
        End If

        If statistiche.Count = 0 Then
            Throw New ApplicationException("Non ci sono statistiche." & vbCrLf & "Impossibile eseguire l'esportazione!")
        End If


        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("StatistichePRO_UT{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy_hhmmss"))


        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)

        Dim line As New StringBuilder

        line.Append("ARRIVO" & vbTab)
        line.Append("PARTENZA" & vbTab)
        line.Append("INTERNI" & vbTab)

        line.Append("ANNULLATI" & vbTab)
        line.Append("TOTALE" & vbTab)
        line.Append("DESCRIZIONE" & vbTab)
        swExport.WriteLine(line.ToString)
        line.Clear()

        Dim ta As Integer = 0
        Dim tp As Integer = 0
        Dim tan As Integer = 0
        Dim tt As Integer = 0
        Dim ti As Integer = 0

        For Each statistica As Statistica In statistiche
            ta += statistica.ProtocolliArrivo
            tp += statistica.ProtocolliPartenza
            ti += statistica.ProtocolliInterni

            tan += statistica.ProtocolliAnnullati
            tt += statistica.Totale

            line.Append(Me.Escape(statistica.ProtocolliArrivo) & vbTab)
            line.Append(Me.Escape(statistica.ProtocolliPartenza) & vbTab)
            line.Append(Me.Escape(statistica.ProtocolliInterni) & vbTab)

            line.Append(Me.Escape(statistica.ProtocolliAnnullati) & vbTab)
            line.Append(Me.Escape(statistica.Totale) & vbTab)
            line.Append(Me.Escape(statistica.Descrizione) & vbTab)

            swExport.WriteLine(line.ToString)
            line.Clear()
        Next

        line.Append(Me.Escape("-----------------------") & vbTab)
        line.Append(Me.Escape("-----------------------") & vbTab)
        line.Append(Me.Escape("-----------------------") & vbTab)
        line.Append(Me.Escape("-----------------------") & vbTab)
        line.Append(Me.Escape("-----------------------") & vbTab)
        line.Append(Me.Escape("--------------------------------------------------------------------------------------------------------") & vbTab)
        swExport.WriteLine(line.ToString)

        line.Clear()

        line.Append(Me.Escape(ta) & vbTab)
        line.Append(Me.Escape(tp) & vbTab)
        line.Append(Me.Escape(ti) & vbTab)

        line.Append(Me.Escape(tan) & vbTab)
        line.Append(Me.Escape(tt) & vbTab)
        line.Append(Me.Escape("") & vbTab)

        swExport.WriteLine(line.ToString)

        swExport.Close()

        Session("AttachmentFullName") = fullPathExport

        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
    End Sub

    'Fapartire la esportazione in Excel della Griglia
    Protected Sub EsportaExcelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EsportaExcelButton.Click
        Try
            Me.EsportaExcel()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
       Me.EsportaExcelButton.Enabled = True
    End Sub


    Private totaleArrivo As Integer
    Private totalePartenza As Integer
    Private totaleInterni As Integer

    Private totaleAnnullati As Integer
    Private totale As Integer

    'Evento ItemDataBound associato alla StatisticheGridView. Setta i totali della griglia. 
    Protected Sub StatisticheGridView_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles StatisticheGridView.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
            Dim statistica = CType(dataItem.DataItem, Statistica)

            Me.totaleArrivo += statistica.ProtocolliArrivo
            Me.totalePartenza += statistica.ProtocolliPartenza
            Me.totaleInterni += statistica.ProtocolliInterni

            Me.totaleAnnullati += statistica.ProtocolliAnnullati
            Me.totale += statistica.Totale

        End If

        If TypeOf e.Item Is GridFooterItem Then
            Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
            footerItem("ProtocolliArrivo").Text = Me.totaleArrivo.ToString
            footerItem("ProtocolliPartenza").Text = Me.totalePartenza.ToString
            footerItem("ProtocolliInterni").Text = Me.totaleInterni.ToString

            footerItem("ProtocolliAnnullati").Text = Me.totaleAnnullati.ToString
            footerItem("Totale").Text = Me.totale.ToString
        End If

    End Sub

    'Evento NeedDataSource associato alla griglia StatisticheGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione Statistiche.
    Protected Sub StatisticheGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles StatisticheGridView.NeedDataSource
        If Me.Statistiche Is Nothing Then
            Me.Statistiche = Me.GetStatistiche(Me.GetFiltro, Me.GetRaggruppamento)
        End If
        Me.StatisticheGridView.DataSource = Me.Statistiche
    End Sub

    'Evento ItemCreated associato alla Griglia StatisticheGridView. Setta lo stile agli item di tipo GridHeaderItem.
    Protected Sub StatisticheGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles StatisticheGridView.ItemCreated
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-2)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    'Fa partire la ricerca
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Me.Statistiche = Nothing
        Me.StatisticheGridView.Rebind()
    End Sub

    'Resetta i campi di ricerca e riaggiorna la griglia.
    Protected Sub AnnullaFiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AnnullaFiltroImageButton.Click
        Me.ResettaFiltro()
        Me.StatisticheGridView.Rebind()
    End Sub


End Class