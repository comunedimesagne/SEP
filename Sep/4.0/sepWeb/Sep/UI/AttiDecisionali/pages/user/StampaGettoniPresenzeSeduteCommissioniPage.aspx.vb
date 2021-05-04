Imports ParsecAdmin
Imports Telerik.Web.UI
Imports System.Data

Imports System.Linq
Imports System.Data.Objects

Partial Class StampaGettoniPresenzeSeduteCommissioniPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

    Public Class ConteggioSeduteGenerali
        Public Property IdCommissione As Integer
        Public Property Totale As Integer
    End Class

    Public Class RiepilogoNominativo

        Public Property IdCommissione As Integer
        Public Property NominativoConsigliere As String = String.Empty
        Public Property DescrizioneCommissione As String = String.Empty
        Public Property TotalePresenze As Integer
        Public Property TotaleAssenze As Integer
        Public Property TotaleAssenzeGiustificate As Integer
        Public Property TotaleAssenzeIngiustificate As Integer

    End Class

    Public Class RiepilogoSeduteNominativoPerCommissione

        Public Property NominativoConsigliere As String = String.Empty
        Public Property DescrizioneCommissione As String = String.Empty
        Public Property Presenza As Integer
        Public Property AssenzeGiustificate As Integer
        Public Property AssenzeIngiustificate As Integer
        Public Property Data As DateTime

    End Class


    Public Property Commissioni() As List(Of ParsecAtt.Commissione)
        Get
            Return CType(Session("StampaGettoniPresenzeSeduteCommissioniPage_Commissione"), List(Of ParsecAtt.Commissione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Commissione))
            Session("StampaGettoniPresenzeSeduteCommissioniPage_Commissione") = value
        End Set
    End Property

#Region "EVENTI PAGINA"


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Atti Decisionali"
        Me.MainPage.DescrizioneProcedura = "> Stampa Gettoni Presenze Commissioni"
        'Me.StampaButton.Attributes.Add("onclick", "this.disabled=true;")


        If Not Me.Page.IsPostBack Then
            Me.ResettaFiltro()
            Me.Commissioni = Nothing
            Me.CaricaCommissioni()
        End If

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraParsecOpenOffice()
        End If
        Me.TitoloElencoCommissioniLabel.Text = "Elenco Commissioni&nbsp;&nbsp;&nbsp;" & If(Me.Commissioni.Count > 0, "( " & Me.Commissioni.Count.ToString & " )", "")

        Dim widthStyle As String = "auto"

        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.CommissioniGridView.Style.Add("width", widthStyle)

    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub CommissioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles CommissioniGridView.ItemCommand
        Select Case e.CommandName
            Case "Stampa"
                'Me.AggiornaVista(e.Item)
                Dim idCommissione As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
                'Me.GetRiepilogoSedutePerCommissione(idCommissione, Me.DataInizioTextBox.SelectedDate, Me.DataFineTextBox.SelectedDate)
                If (idCommissione > 0) Then
                    If checkErrori() Then
                        Me.RegistraParsecOpenOfficeCalcProcessingSingle(idCommissione)
                    Else
                        ParsecUtility.Utility.MessageBox("Le date sono obbligatorie.", False)
                    End If
                Else
                    ParsecUtility.Utility.MessageBox("Nessuna Commissione selezionata.", False)
                End If

                    'ParsecUtility.Utility.MessageBox("Stampa non ancora implementata.", False)
        End Select
    End Sub

    Protected Sub CommissioniGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles CommissioniGridView.NeedDataSource
        If Me.Commissioni Is Nothing Then
            Dim commissioni As New ParsecAtt.CommissioniRepository
            Me.Commissioni = commissioni.GetView(Nothing)
            commissioni.Dispose()
        End If
        Me.CommissioniGridView.DataSource = Me.Commissioni
    End Sub

    Protected Sub CommissioniGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles CommissioniGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Public Function GetRiepilogoSedutePerCommissione(ByVal idCommissione As Integer, ByVal daData As Date, ByVal aData As Date) As List(Of RiepilogoSeduteNominativoPerCommissione)

        Dim listaPresenzeRet As New List(Of RiepilogoSeduteNominativoPerCommissione)

        Dim consiglieri = New ParsecAtt.ConsiglieriRepository
        Dim commissioni = (New ParsecAtt.CommissioniRepository(consiglieri.Context)).GetQuery

        Dim commissione = commissioni.Where(Function(w) w.Id = idCommissione).FirstOrDefault



        Dim presenze = (New ParsecAtt.PresenzaCommissioneRepository(consiglieri.Context)).GetQuery

        Dim sedute = (New ParsecAtt.SeduteCommissioneRepository(consiglieri.Context)).GetQuery

        Dim dataIniziale As Date = New Date(daData.Year, daData.Month, daData.Day, 0, 0, 0)
        Dim dataFinale As Date = New Date(aData.Year, aData.Month, aData.Day, 23, 59, 59)

        Dim lista = (From seduta In sedute
                    Join presenza In presenze On presenza.IdSeduta Equals seduta.Id
                    Join consigliere In consiglieri.GetQuery On consigliere.Id Equals presenza.IdConsigliere
                    Where seduta.IdCommissione = idCommissione And seduta.Data >= dataIniziale And seduta.Data <= dataFinale
                    Let p = If(presenza.Presenza.HasValue, If(presenza.Presenza, 1, 0), 0)
                    Let ag = If(presenza.AssenzaGiustificata.HasValue, If(presenza.AssenzaGiustificata, 1, 0), 0)
                    Let ai = If(presenza.AssenzaIngiustificata.HasValue, If(presenza.AssenzaIngiustificata, 1, 0), 0)).AsEnumerable.Select(Function(s) New RiepilogoSeduteNominativoPerCommissione With {
                                                                                                                                                                    .NominativoConsigliere = s.consigliere.Nominativo,
                                                                                                                                                                    .AssenzeGiustificate = s.ag,
                                                                                                                                                                    .AssenzeIngiustificate = s.ai,
                                                                                                                                                                    .Presenza = s.p,
                                                                                                                                                                    .Data = s.seduta.Data,
                                                                                                                                                                    .DescrizioneCommissione = commissione.Descrizione
                                                                                                                                                                    })

        Dim listaRet = lista.ToList

        Dim listaConsiglieriTotali = (From list In lista
                                Order By list.NominativoConsigliere
                               Select list.NominativoConsigliere).Distinct

        Dim gruppiData = (From ls In lista
                            Select ls.Data, ls.DescrizioneCommissione).Distinct

        For Each dt In gruppiData
            Dim dataSeduta = dt.Data

            Dim listaConsiglieriInclusiPerData = (From l In lista
                                          Where l.Data = dataSeduta
                                          Select l.NominativoConsigliere).ToList

            Dim listaConsiglieriNonPresentiInElenco = From l In listaConsiglieriTotali
                                                      Where Not listaConsiglieriInclusiPerData.Contains(l)

            For Each elemento In listaConsiglieriNonPresentiInElenco

                Dim nuovoRiepilogoSeduteNominativoPerCommissione As New RiepilogoSeduteNominativoPerCommissione
                nuovoRiepilogoSeduteNominativoPerCommissione.AssenzeGiustificate = 1
                nuovoRiepilogoSeduteNominativoPerCommissione.AssenzeIngiustificate = 0
                nuovoRiepilogoSeduteNominativoPerCommissione.Presenza = 0
                nuovoRiepilogoSeduteNominativoPerCommissione.Data = dataSeduta
                nuovoRiepilogoSeduteNominativoPerCommissione.NominativoConsigliere = elemento
                nuovoRiepilogoSeduteNominativoPerCommissione.DescrizioneCommissione = dt.DescrizioneCommissione

                listaRet.Add(nuovoRiepilogoSeduteNominativoPerCommissione)

            Next


        Next




        Return listaRet.OrderBy(Function(o) o.NominativoConsigliere).ThenBy(Function(o2) o2.Data).ToList


        'Dim listaSedute = From seduta In sedute.Where(Function(w) w.Data >= daData And w.Data <= aData).OrderByDescending(Function(o) o.Data)


        '        SELECT tC.CnsNominativo,tS.SedData,tP.Presenza,tP.AssGiusti,tP.AssIngiusti 
        'FROM tSeduta tS INNER JOIN dbo.tPresenza tP ON tS.SedId=tP.SedId 
        '     INNER JOIN dbo.tConsigliere tC ON tP.CnsId=tC.CnsId 
        '     WHERE LEFT(tS.SedData,11)>=convert(datetime,'01/01/2015',103) AND 
        '     LEFT(tS.SedData,11)<=convert(datetime,'30/03/2016',103) AND tS.comid=1 
        '      ORDER BY tc.CnsNominativo,tS.sedData



        'Dim prodottoCartesianoConsComm = (From commissione In commissioni.GetQuery.ToList
        '                         From consigliere In consiglieri.GetQuery.ToList
        '                         Where commissione.Stato Is Nothing And consigliere.Stato Is Nothing And commissione.Id = idCommissione
        '                         Select consigliere.Id, consigliere.Nominativo, commissione)




        'Dim sedute = (New ParsecAtt.SeduteCommissioneRepository(consiglieri.Context))
        'Dim listaSedute = From seduta In sedute.Where(Function(w) w.Data >= daData And w.Data <= aData).OrderByDescending(Function(o) o.Data)

        'If listaSedute.Count > 7 Then
        '    listaSedute = listaSedute.Take(7)
        'End If

        'Dim presenzaSeduta = (New ParsecAtt.PresenzaCommissioneRepository(consiglieri.Context)).GetQuery

        'For Each elemento In prodottoCartesianoConsComm

        '    Dim idCommiss = elemento.commissione.Id
        '    Dim descrizioneCommissione = elemento.commissione.Descrizione
        '    Dim idConsigliere = elemento.Id
        '    Dim descrizioneConsigliere = elemento.Nominativo


        '    Dim presenze = (From presenza In presenzaSeduta
        '                   Join sedut In listaSedute On sedut.Id Equals presenza.IdSeduta
        '                   Join consigliere In consiglieri.GetQuery On consigliere.Id Equals presenza.IdConsigliere
        '                   Where presenza.IdConsigliere = idConsigliere And sedut.IdCommissione = idCommiss
        '                   Let p = If(presenza.Presenza.HasValue, If(presenza.Presenza, 1, 0), 0)
        '                   Let ag = If(presenza.AssenzaGiustificata.HasValue, If(presenza.AssenzaGiustificata, 1, 0), 0)
        '                   Let ai = If(presenza.AssenzaIngiustificata.HasValue, If(presenza.AssenzaIngiustificata, 1, 0), 0)).AsEnumerable.Select(Function(s) New RiepilogoSeduteNominativoPerCommissione With {
        '                                                                                                                                                            .NominativoConsigliere = s.consigliere.Nominativo,
        '                                                                                                                                                            .AssenzeGiustificate = s.ag,
        '                                                                                                                                                            .AssenzeIngiustificate = s.ai,
        '                                                                                                                                                            .Presenza = s.p,
        '                                                                                                                                                            .Data = s.sedut.Data,
        '                                                                                                                                                            .DescrizioneCommissione = descrizioneCommissione
        '                                                                                                                                                            })



        '    Dim elementoRiepilogo As New RiepilogoSeduteNominativoPerCommissione
        '    If (presenze.Count = 0) Then
        '    Else
        '        listaPresenzeRet.AddRange(presenze.ToList)
        '    End If


        'Next

        'Return listaPresenzeRet.OrderBy(Function(o) o.NominativoConsigliere).ThenBy(Function(o2) o2.Data).ToList

    End Function

    Public Function GetRiepilogoConsiglieri(ByVal daData As Date, ByVal aData As Date) As List(Of RiepilogoNominativo)

        Dim listaPresenzeRet As New List(Of RiepilogoNominativo)

        Dim consiglieri = New ParsecAtt.ConsiglieriRepository
        Dim commissioni = New ParsecAtt.CommissioniRepository(consiglieri.Context)

        Dim sedute = (New ParsecAtt.SeduteCommissioneRepository(consiglieri.Context))

        Dim dataIniziale As Date = New Date(daData.Year, daData.Month, daData.Day, 0, 0, 0)
        Dim dataFinale As Date = New Date(aData.Year, aData.Month, aData.Day, 23, 59, 59)

        Dim listaSedute = From seduta In sedute.Where(Function(w) w.Data >= dataIniziale And w.Data <= dataFinale)

        'serve per il messaggio di mancate sedute
        If (listaSedute.Count = 0) Then
            Return listaPresenzeRet
        End If

        Dim prodottoCartesianoConsComm = (From commissione In commissioni.GetQuery.ToList
                                 From consigliere In consiglieri.GetQuery.ToList
                                 Where commissione.Stato Is Nothing And consigliere.Stato Is Nothing
                                 Select consigliere.Id, consigliere.Nominativo, commissione).Distinct



        Dim presenzaSeduta = (New ParsecAtt.PresenzaCommissioneRepository(sedute.Context)).GetQuery


        For Each elemento In prodottoCartesianoConsComm

            Dim idCommiss = elemento.commissione.Id
            Dim descrizioneCommissione = elemento.commissione.Descrizione
            Dim idConsigliere = elemento.Id
            Dim descrizioneConsigliere = elemento.Nominativo

            Dim presenze = From presenza In presenzaSeduta
                           Join sedut In listaSedute On sedut.Id Equals presenza.IdSeduta
                           Where presenza.IdConsigliere = idConsigliere And sedut.IdCommissione = idCommiss
                           Let p = If(presenza.Presenza.HasValue, If(presenza.Presenza, 1, 0), 0)
                           Let a = If(presenza.AssenzaGiustificata.HasValue, If(presenza.AssenzaGiustificata, 1, 0), 0) + If(presenza.AssenzaIngiustificata.HasValue, If(presenza.AssenzaIngiustificata, 1, 0), 0)
                           Let ag = If(presenza.AssenzaGiustificata.HasValue, If(presenza.AssenzaGiustificata, 1, 0), 0)
                           Let ai = If(presenza.AssenzaIngiustificata.HasValue, If(presenza.AssenzaIngiustificata, 1, 0), 0)
                           Select p, a

            Dim groups = ((From j In presenze
                        Group By x = New With {Key .idCommissione = idCommiss, Key .descrComm = descrizioneCommissione, Key .descrCons = descrizioneConsigliere} Into g = Group
                        Select New RiepilogoNominativo With {
                            .IdCommissione = x.idCommissione,
                            .DescrizioneCommissione = x.descrComm,
                            .NominativoConsigliere = x.descrCons,
                            .TotalePresenze = g.Sum(Function(r) CInt(r.p)),
                            .TotaleAssenze = g.Sum(Function(r) CInt(r.a)),
                            .TotaleAssenzeGiustificate = g.Sum(Function(r) CInt(r.a)),
                            .TotaleAssenzeIngiustificate = g.Sum(Function(r) CInt(r.a))
                        })).ToList

            Dim elementoRiepilogo As New RiepilogoNominativo
            If (groups.Count = 0) Then
                elementoRiepilogo.IdCommissione = idCommiss
                elementoRiepilogo.DescrizioneCommissione = descrizioneCommissione
                elementoRiepilogo.NominativoConsigliere = descrizioneConsigliere
                elementoRiepilogo.TotalePresenze = 0
                elementoRiepilogo.TotaleAssenze = 0
                elementoRiepilogo.TotaleAssenzeGiustificate = 0
                elementoRiepilogo.TotaleAssenzeIngiustificate = 0
            Else
                elementoRiepilogo.IdCommissione = groups(0).IdCommissione
                elementoRiepilogo.DescrizioneCommissione = groups(0).DescrizioneCommissione
                elementoRiepilogo.NominativoConsigliere = groups(0).NominativoConsigliere
                elementoRiepilogo.TotalePresenze = groups(0).TotalePresenze
                elementoRiepilogo.TotaleAssenze = groups(0).TotaleAssenze
                elementoRiepilogo.TotaleAssenzeGiustificate = groups(0).TotaleAssenzeGiustificate
                elementoRiepilogo.TotaleAssenzeIngiustificate = groups(0).TotaleAssenzeIngiustificate
            End If

            listaPresenzeRet.Add(elementoRiepilogo)
        Next
        'qua acerco di si sistemare il conteggio per queiconsiglieri che non ci sono proprio in qualche seduta di commisisone (neanche come assenti)
        Dim ConteggiSeduteCommissione = (From seduta In sedute.GetQuery
                                        Where (seduta.Data >= dataIniziale And seduta.Data <= dataFinale)
                                        Group By x = New With {Key .idCommissione = seduta.IdCommissione} Into g = Group
                                        Select New ConteggioSeduteGenerali With {.IdCommissione = x.idCommissione, .Totale = g.Count()})

        For Each elemento In listaPresenzeRet
            Dim idCommissione = elemento.IdCommissione
            Dim conteggi = ConteggiSeduteCommissione.Where(Function(w) w.IdCommissione = idCommissione).FirstOrDefault
            If Not conteggi Is Nothing Then
                Dim conteggio = conteggi.Totale
                Dim differenza = conteggio - (elemento.TotaleAssenze + elemento.TotalePresenze)
                If differenza > 0 Then
                    elemento.TotaleAssenzeGiustificate = elemento.TotaleAssenzeGiustificate + differenza
                    elemento.TotaleAssenze = elemento.TotaleAssenze + differenza
                End If
            End If
        Next
        'fine sistemazione

        ''///////////////////////////////////////////////////////////////////////////////////////
        'in questa sezione devo tener conto di tutti quei consiglieri con lo stato Annullato e che hanno fatto qualche Seduta

        'trovo i nominativi che nel perido specificato hanno fatto qualche seduta e che sono stati "Annullati"
        Dim listaNominativiCheHannoFattoSedute = (From seduta In sedute.GetQuery
                    Join presenza In presenzaSeduta On presenza.IdSeduta Equals seduta.Id
                    Join consigliere In consiglieri.GetQuery On consigliere.Id Equals presenza.IdConsigliere
                    Where seduta.Data >= dataIniziale And seduta.Data <= dataFinale And consigliere.Stato = "A"
                    Select consigliere.Nominativo, seduta.IdCommissione).Distinct



        Dim listaRet = listaPresenzeRet.ToList

        'estraggo le commissioni
        Dim gruppiCommissione = (From ls In listaPresenzeRet
                                 Select ls.IdCommissione, ls.DescrizioneCommissione).Distinct

        Dim presenzeConsiglieriAnnullati As Integer = 0
        Dim assenzeIngiustificateConsiglieriAnnullati As Integer = 0
        Dim assenzeGiustificateConsiglieriAnnullati As Integer = 0

        'ciclo per ogni commisisone
        For Each commissione In gruppiCommissione
            Dim idCommissione = commissione.IdCommissione
            Dim descrizioneCommissione = commissione.DescrizioneCommissione
            'trovo i nominaitvi dei consiglieri che hanno fatto una seduta per la commissione
            Dim listaSeduteFiltrataPerCommissione = listaNominativiCheHannoFattoSedute.Where(Function(w) w.IdCommissione = idCommissione).ToList

            If (listaSeduteFiltrataPerCommissione.Count = 0) Then
                ''non ho nessuno consigliere per la commissione corrente
                'devo comunque aggiugnerli e segnare che non hanno fatto ninete
                Dim listaConsiglieri = listaNominativiCheHannoFattoSedute.Select(Function(s) s.Nominativo).Distinct
                For Each consigliereSeduta In listaConsiglieri


                    'trovo le presenze e assenze totali
                    Dim nominativoConsigliere = consigliereSeduta
                    presenzeConsiglieriAnnullati = 0
                    assenzeIngiustificateConsiglieriAnnullati = 0
                    assenzeGiustificateConsiglieriAnnullati = 0

                    Dim elementoRiepilogo As New RiepilogoNominativo
                    elementoRiepilogo.IdCommissione = idCommissione
                    elementoRiepilogo.DescrizioneCommissione = descrizioneCommissione
                    elementoRiepilogo.NominativoConsigliere = consigliereSeduta
                    elementoRiepilogo.TotalePresenze = presenzeConsiglieriAnnullati
                    elementoRiepilogo.TotaleAssenze = assenzeGiustificateConsiglieriAnnullati + assenzeIngiustificateConsiglieriAnnullati
                    elementoRiepilogo.TotaleAssenzeGiustificate = assenzeGiustificateConsiglieriAnnullati
                    elementoRiepilogo.TotaleAssenzeIngiustificate = assenzeIngiustificateConsiglieriAnnullati

                    listaRet.Add(elementoRiepilogo)

                Next
            Else
                'ho trovato deiconsiglieri che hanno fatto sedute per la commissione corrente
                For Each consigliereSeduta In listaSeduteFiltrataPerCommissione

                    If (consigliereSeduta.IdCommissione = idCommissione) Then
                        'trovo le presenze e assenze totali
                        Dim nominativoConsigliere = consigliereSeduta.Nominativo
                        presenzeConsiglieriAnnullati = 0
                        assenzeIngiustificateConsiglieriAnnullati = 0
                        assenzeGiustificateConsiglieriAnnullati = 0

                        Dim presenzePerCommissione As List(Of RiepilogoSeduteNominativoPerCommissione) = Me.GetRiepilogoSedutePerCommissione(idCommissione, Me.DataInizioTextBox.SelectedDate, Me.DataFineTextBox.SelectedDate).Where(Function(w) w.NominativoConsigliere = nominativoConsigliere).ToList
                        If presenzePerCommissione.Any Then
                            presenzeConsiglieriAnnullati = presenzePerCommissione.Select(Function(s) s.Presenza).Sum
                            assenzeGiustificateConsiglieriAnnullati = presenzePerCommissione.Select(Function(s) s.AssenzeGiustificate).Sum
                            assenzeIngiustificateConsiglieriAnnullati = presenzePerCommissione.Select(Function(s) s.AssenzeIngiustificate).Sum
                        End If

                        Dim elementoRiepilogo As New RiepilogoNominativo
                        elementoRiepilogo.IdCommissione = idCommissione
                        elementoRiepilogo.DescrizioneCommissione = descrizioneCommissione
                        elementoRiepilogo.NominativoConsigliere = consigliereSeduta.Nominativo
                        elementoRiepilogo.TotalePresenze = presenzeConsiglieriAnnullati
                        elementoRiepilogo.TotaleAssenzeGiustificate = assenzeGiustificateConsiglieriAnnullati
                        elementoRiepilogo.TotaleAssenzeIngiustificate = assenzeIngiustificateConsiglieriAnnullati

                        elementoRiepilogo.TotaleAssenze = assenzeGiustificateConsiglieriAnnullati + assenzeIngiustificateConsiglieriAnnullati
                        listaRet.Add(elementoRiepilogo)
                    Else

                    End If

                Next
            End If

        Next
        '//////////////////////////////////////

        Return listaRet.OrderBy(Function(o) o.NominativoConsigliere).ThenBy(Function(o2) o2.DescrizioneCommissione).ToList
    End Function



    Private Sub CaricaCommissioni()
        Dim commissioni As New ParsecAtt.CommissioniRepository
        Me.Commissioni = commissioni.GetView(Nothing)
        commissioni.Dispose()
    End Sub

    Private Sub ResettaFiltro()

        Dim ultimoGiornoMesePrecedente As Date = DateSerial(Now.Year, Now.Month, 0)
        Me.DataInizioTextBox.SelectedDate = New Date(ultimoGiornoMesePrecedente.Year, ultimoGiornoMesePrecedente.Month, 1)
        Me.DataFineTextBox.SelectedDate = New Date(ultimoGiornoMesePrecedente.Year, ultimoGiornoMesePrecedente.Month, ultimoGiornoMesePrecedente.Day)

    End Sub


    Private Sub NotificaOperazione()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim path = ParsecAdmin.WebConfigSettings.GetKey("PathDownload")
        Dim files = IO.Directory.GetFiles(path, "*.txt")
        Dim nomefile As String = String.Empty
        Try
            For Each f In files
                nomefile = IO.Path.GetFileName(f)
                If nomefile.StartsWith("Utente_" & utenteCollegato.Id.ToString & "_") Then
                    IO.File.Delete(f)
                End If

            Next
        Catch ex As Exception
        End Try
    End Sub

#End Region

#Region "EVENTI CONTROLLI PAGINA"

    Protected Sub AnnullaButton_Click(sender As Object, e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

    Protected Sub StampaButton_Click(sender As Object, e As System.EventArgs) Handles StampaButton.Click
        If checkErrori() Then
            Me.RegistraParsecOpenOfficeCalcProcessing()
        Else
            ParsecUtility.Utility.MessageBox("Le date sono obbligatorie.", False)
        End If
    End Sub

    Protected Sub notificaOperazioneButton_Click(sender As Object, e As System.EventArgs) Handles notificaOperazioneButton.Click
        Me.NotificaOperazione()
    End Sub

#End Region



#Region "Gestione di ParsecOpenDocument"

    Private Function checkErrori() As Boolean
        If (Me.DataInizioTextBox.SelectedDate Is Nothing Or Me.DataFineTextBox.SelectedDate Is Nothing) Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

    Private Sub RegistraParsecOpenOfficeCalcProcessing()

        Dim datiCelle = Me.CreateDataSourceSedute()

        If Not datiCelle Is Nothing Then
            Dim openOfficeParameters As New ParsecAdmin.OpenOfficeParameters

            Dim remotePath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti") & Now.Year.ToString & "/"
            Dim remotePathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates")
            Dim template As String = "" '"templateReportCommissioni.ods"

            template = "templateReportCommissioni.ods"


            Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = remotePathTemplate & template,
                                                         .DestRemotePath = remotePath & "Report.ods",
                                                         .ShowWindow = True,
                                                         .Enabled = True,
                                                         .FunctionName = "ProcessDocument"}

            Dim data As String = openOfficeParameters.CreateDataSource(datiInput, datiCelle)

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

            Dim localFilenameDataSource = "Utente_" & utenteCollegato.Id.ToString & "_" & Guid.NewGuid.ToString & ".txt"
            Dim localPathDataSource As String = String.Format("{0}{1}", ParsecAdmin.WebConfigSettings.GetKey("PathDownload"), localFilenameDataSource)
            IO.File.WriteAllText(localPathDataSource, data)
            Dim dataSource = openOfficeParameters.CreateDataSource(localFilenameDataSource)



            'UTILIZZO L'APPLET
            If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(dataSource, Me.notificaOperazioneButton.ClientID, True, False)
            Else
                'UTILIZZO IL SOCKET
                ParsecUtility.Utility.EseguiServerComunicatorService(dataSource, True, AddressOf Me.NotificaOperazione)

            End If
        Else

            ParsecUtility.Utility.MessageBox("Non ci sono SEDUTE di COMMISSIONI per il periodo: " & Me.DataInizioTextBox.SelectedDate & " - " & Me.DataFineTextBox.SelectedDate & " !", False)
        End If

    End Sub

    Private Function CreateDataSourceSedute() As ParsecAdmin.DatiCelle

        Dim ht As New Hashtable

        Dim presenze = GetRiepilogoConsiglieri(Me.DataInizioTextBox.SelectedDate, Me.DataFineTextBox.SelectedDate)



        If presenze.Count > 0 Then

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName("CostoOrarioSedutaCommissione")
            parametri.Dispose()

            If Not parametro Is Nothing Then
                ht.Add("B33", parametro.Valore)
            End If

            ht.Add("A1", "PRESENZE COMPONENTI CONSILIARI DELLE SEDUTE DAL " & Me.DataInizioTextBox.SelectedDate.Value.ToShortDateString & " AL " & Me.DataFineTextBox.SelectedDate.Value.ToShortDateString)

            Dim nome As String = ""
            Dim colonna As Integer = AscW("B")
            Dim riga As Integer = 4
            Dim ncomm As Integer = 0


            Dim elencoDescrizioni = presenze.GroupBy(Function(c) c.DescrizioneCommissione).Select(Function(c) c.FirstOrDefault).Select(Function(c) c.DescrizioneCommissione).ToList

            'STAMPO LA DESCRIZIONE DI OGNI COMMISSIONE
            For Each descizione In elencoDescrizioni
                ncomm += 1
                ht.Add(Convert.ToChar(colonna) & "2", descizione)
                colonna += 2
            Next


            'presenze = presenze.Where(Function(w) w.TotaleAssenze > 0 Or w.TotaleAssenzeGiustificate > 0 Or w.TotaleAssenzeIngiustificate > 0 Or w.TotalePresenze > 0).ToList
            'Do
            '    ht.Add(Convert.ToChar(colonna) & "2", presenze(ncomm).DescrizioneCommissione & " - " & (ncomm + 1).ToString & " COMMISSIONE")
            '    colonna += 2
            '    nome = CStr(presenze(ncomm).NominativoConsigliere)
            '    ncomm += 1

            '    If ncomm >= presenze.Count Then
            '        Exit Do
            '    End If

            'Loop While (nome = CStr(presenze(ncomm).NominativoConsigliere))

            colonna = AscW("B")

            Dim cntriga As Integer = 0

            For i As Integer = 0 To presenze.Count - 1


                ht.Add(Convert.ToChar(colonna) & riga.ToString, presenze(i).TotalePresenze.ToString)
                colonna += 1
                ht.Add(Convert.ToChar(colonna) & riga.ToString, presenze(i).TotaleAssenze.ToString)
                cntriga += 1
                If (cntriga = ncomm) Then
                    colonna = AscW("B")
                    ht.Add("A" & riga.ToString, presenze(i).NominativoConsigliere)
                    riga += 1
                    cntriga = 0
                Else
                    colonna += 1
                End If
            Next

        Else
            Return Nothing
        End If

        Return New ParsecAdmin.DatiCelle(ht)

    End Function


    Private Sub RegistraParsecOpenOfficeCalcProcessingSingle(ByVal idCommissione As Integer)

        Dim datiCelle = Me.CreateDataSourceSeduteSingle(idCommissione)

        If Not datiCelle Is Nothing Then
            Dim openOfficeParameters As New ParsecAdmin.OpenOfficeParameters

            Dim remotePath As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti") & Now.Year.ToString & "/"
            Dim remotePathTemplate As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiTemplates")
            Dim template As String = "templateReportCommissioniSingle.ods" '"templateReportCommissioni.ods"


            Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = remotePathTemplate & template,
                                                         .DestRemotePath = remotePath & "Report.ods",
                                                         .ShowWindow = True,
                                                         .Enabled = True,
                                                         .FunctionName = "ProcessDocument"}

            Dim data As String = openOfficeParameters.CreateDataSource(datiInput, datiCelle)

            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

            Dim localFilenameDataSource = "Utente_" & utenteCollegato.Id.ToString & "_" & Guid.NewGuid.ToString & ".txt"
            Dim localPathDataSource As String = String.Format("{0}{1}", ParsecAdmin.WebConfigSettings.GetKey("PathDownload"), localFilenameDataSource)
            IO.File.WriteAllText(localPathDataSource, data)
            Dim dataSource = openOfficeParameters.CreateDataSource(localFilenameDataSource)



            'UTILIZZO L'APPLET
            If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(dataSource, Me.notificaOperazioneButton.ClientID, True, False)
            Else
                'UTILIZZO IL SOCKET
                ParsecUtility.Utility.EseguiServerComunicatorService(dataSource, True, AddressOf Me.NotificaOperazione)

            End If
        Else

            ParsecUtility.Utility.MessageBox("Non ci sono SEDUTE di COMMISSIONI per il periodo: " & Me.DataInizioTextBox.SelectedDate & " - " & Me.DataFineTextBox.SelectedDate & " !", False)
        End If

    End Sub

    Private Function CreateDataSourceSeduteSingle(ByVal idCommissione As Integer) As ParsecAdmin.DatiCelle


        Dim ht As New Hashtable

        Dim commissioniRepository As New ParsecAtt.CommissioniRepository
        Dim commissione = commissioniRepository.GetById(idCommissione)
        Dim descrizioneCommissione = commissione.Descrizione
        If descrizioneCommissione.Length > 50 Then
            descrizioneCommissione = descrizioneCommissione.Substring(0, 50) & "..."
        End If

        Dim presenze = Me.GetRiepilogoSedutePerCommissione(idCommissione, Me.DataInizioTextBox.SelectedDate, Me.DataFineTextBox.SelectedDate)


        If presenze.Count > 0 Then


            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName("CostoOrarioSedutaCommissione")
            parametri.Dispose()

            If Not parametro Is Nothing Then
                ht.Add("B33", parametro.Valore)
            End If

            ht.Add("A1", descrizioneCommissione.ToUpper & " - PRESENZE E ASSENZE DAL " & Me.DataInizioTextBox.SelectedDate.Value.ToShortDateString & " AL " & Me.DataFineTextBox.SelectedDate.Value.ToShortDateString)


            Dim nome As String = ""
            Dim colonna As Integer = AscW("B")
            Dim riga As Integer = 5

            Dim nsed As Integer = 0



            Dim elencoDate = presenze.GroupBy(Function(c) c.Data).Select(Function(c) c.FirstOrDefault).Select(Function(c) c.Data).OrderBy(Function(c) c).ToList

            If elencoDate.Count > 7 Then
                ParsecUtility.Utility.MessageBox("Sono state trovate più di 7 sedute nel periodo indicato: verranno visaulizzate le ultime 7.", False)
                elencoDate = elencoDate.Take(7).ToList
                presenze = presenze.Where(Function(c) elencoDate.Contains(c.Data)).ToList
            End If



            'STAMPO LA DESCRIZIONE DI OGNI COMMISSIONE
            For Each dt In elencoDate
                ht.Add(Convert.ToChar(colonna) & "2", dt.ToShortDateString)
                nsed += 1
                colonna += 3
            Next


            colonna = AscW("B")

            Dim cntriga As Integer = 0
            Dim valprec As Integer = -1



            For i As Integer = 0 To presenze.Count - 1

                If presenze(i).Presenza Then
                    If i <> 0 Then

                        If valprec <> -1 Then

                            If valprec = 1 Then

                                colonna += 3 'fatto

                            ElseIf valprec = 0 Then

                                colonna += 2 ' fatto

                            ElseIf valprec = 2 Then

                                colonna += 1 'fatto

                            End If

                        End If

                    End If

                    valprec = 1

                ElseIf presenze(i).AssenzeGiustificate Then

                    If i <> 0 Then

                        If valprec <> -1 Then

                            If valprec = 0 Then

                                colonna += 3 'fatto

                            ElseIf valprec = 1 Then

                                colonna += 4 'fatto

                            ElseIf valprec = 2 Then

                                colonna += 2 'fatto

                            End If

                        Else

                            colonna += 1 'fatto

                        End If

                    Else

                        colonna += 1 'fatto

                    End If

                    valprec = 0
                ElseIf presenze(i).AssenzeIngiustificate Then
                    If i <> 0 Then

                        If valprec <> -1 Then

                            If valprec = 0 Then

                                colonna += 4 'fatto

                            ElseIf valprec = 1 Then

                                colonna += 5 'fatto

                            ElseIf valprec = 2 Then

                                colonna += 3 'fatto

                            End If

                        Else

                            colonna += 2 'fatto

                        End If

                    Else

                        colonna += 2 'fatto

                    End If

                    valprec = 2


                End If


                ht.Add(Convert.ToChar(colonna) & riga.ToString, (1).ToString)

                cntriga += 1

                If (cntriga = nsed) Then
                    colonna = AscW("B")
                    ht.Add("A" & riga.ToString, presenze(i).NominativoConsigliere)
                    riga += 1
                    cntriga = 0
                    valprec = -1
                End If





            Next



        Else
            Return Nothing
        End If

        Return New ParsecAdmin.DatiCelle(ht)

    End Function


#End Region




End Class