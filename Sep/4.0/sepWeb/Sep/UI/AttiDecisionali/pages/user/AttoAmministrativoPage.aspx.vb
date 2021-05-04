Imports Telerik.Web.UI
Imports System.Data
Imports System.Web.Mail
Imports System.Web.Services
Imports Rebex.Net


Partial Class AttoAmministrativoPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As Object

#Region "PROPRIETA'"

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

    Public Property IdSessione As String
        Set(ByVal value As String)
            Me.infoSessioneHidden.Value = value
        End Set
        Get
            Return Me.infoSessioneHidden.Value
        End Get
    End Property

    'luca 01/07/2020
    '' ''Public Property IdGaraContratti As String
    '' ''    Set(ByVal value As String)
    '' ''        Me.IdBandoGaraHidden.Value = value
    '' ''    End Set
    '' ''    Get
    '' ''        Return Me.IdBandoGaraHidden.Value
    '' ''    End Get
    '' ''End Property

    Public Property IdFirmaModificabile As String
        Set(ByVal value As String)
            Me.IdFirmaModificabileHidden.Value = value
        End Set
        Get
            Return Me.IdFirmaModificabileHidden.Value
        End Get
    End Property

    Public Property ElencoIdAllegatiBloccati As String
        Set(ByVal value As String)
            Me.ElencoIdAllegatiBloccatiHidden.Value = value
        End Set
        Get
            Return Me.ElencoIdAllegatiBloccatiHidden.Value
        End Get
    End Property



    Public Property SelectedItems As Dictionary(Of String, Boolean)
        Get
            If Session("AttoAmministrativoPage_SelectedItems" & Me.IdSessione) Is Nothing Then
                Session("AttoAmministrativoPage_SelectedItems" & Me.IdSessione) = New Dictionary(Of String, Boolean)
            End If
            Return CType(Session("AttoAmministrativoPage_SelectedItems" & Me.IdSessione), Dictionary(Of String, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            Session("AttoAmministrativoPage_SelectedItems" & Me.IdSessione) = value
        End Set
    End Property

    Public Property TipologiaDocumento As ParsecAtt.TipoDocumento
        Set(ByVal value As ParsecAtt.TipoDocumento)
            Session("AttoAmministrativoPage_TipologiaDocumento" & Me.IdSessione) = value
        End Set
        Get
            Return CType(Session("AttoAmministrativoPage_TipologiaDocumento" & Me.IdSessione), ParsecAtt.TipoProcedura)
        End Get
    End Property

    Public Property TipologiaDocumentoApertura As ParsecAtt.TipoDocumento
        Set(ByVal value As ParsecAtt.TipoDocumento)
            Session("AttoAmministrativoPage_TipologiaDocumentoApertura" & Me.IdSessione) = value
        End Set
        Get
            Return CType(Session("AttoAmministrativoPage_TipologiaDocumentoApertura" & Me.IdSessione), ParsecAtt.TipoProcedura)
        End Get
    End Property

    Public Property TipologiaProceduraCorrente As ParsecAtt.TipoProcedura
        Set(ByVal value As ParsecAtt.TipoProcedura)
            Session("AttoAmministrativoPage_TipologiaProcedura" & Me.IdSessione) = value
        End Set
        Get
            Return CType(Session("AttoAmministrativoPage_TipologiaProcedura" & Me.IdSessione), ParsecAtt.TipoProcedura)
        End Get
    End Property

    Public Property TipologiaProceduraApertura As ParsecAtt.TipoProcedura
        Set(ByVal value As ParsecAtt.TipoProcedura)
            Session("AttoAmministrativoPage_TipologiaProceduraApertura" & Me.IdSessione) = value
        End Set
        Get
            Return CType(Session("AttoAmministrativoPage_TipologiaProceduraApertura" & Me.IdSessione), ParsecAtt.TipoProcedura)
        End Get
    End Property

    Public Property TipologiaModello As Integer
        Set(ByVal value As Integer)
            ViewState("TipologiaModello") = value
        End Set
        Get
            Return CType(ViewState("TipologiaModello"), Integer)
        End Get
    End Property

    Public Property Documenti() As List(Of ParsecAtt.Documento)
        Get
            Return CType(Session("AttoAmministrativoPage_Documenti" & Me.IdSessione), List(Of ParsecAtt.Documento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Documento))
            Session("AttoAmministrativoPage_Documenti" & Me.IdSessione) = value
        End Set
    End Property

    Public Property Documento() As ParsecAtt.Documento
        Get
            Return CType(Session("AttoAmministrativoPage_Documento" & Me.IdSessione), ParsecAtt.Documento)
        End Get
        Set(ByVal value As ParsecAtt.Documento)
            Session("AttoAmministrativoPage_Documento" & Me.IdSessione) = value
        End Set
    End Property

    Public Property Liquidazioni() As List(Of ParsecAtt.Liquidazione)
        Get
            Return CType(Session("AttoAmministrativoPage_Liquidazioni" & Me.IdSessione), List(Of ParsecAtt.Liquidazione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Liquidazione))
            Session("AttoAmministrativoPage_Liquidazioni" & Me.IdSessione) = value
        End Set
    End Property

    Public Property ImpegniSpesa() As List(Of ParsecAtt.ImpegnoSpesa)
        Get
            Return CType(Session("AttoAmministrativoPage_ImpegniSpesa" & Me.IdSessione), List(Of ParsecAtt.ImpegnoSpesa))
        End Get
        Set(ByVal value As List(Of ParsecAtt.ImpegnoSpesa))
            Session("AttoAmministrativoPage_ImpegniSpesa" & Me.IdSessione) = value
        End Set
    End Property

    Public Property Accertamenti() As List(Of ParsecAtt.Accertamento)
        Get
            Return CType(Session("AttoAmministrativoPage_Accertamenti" & Me.IdSessione), List(Of ParsecAtt.Accertamento))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Accertamento))
            Session("AttoAmministrativoPage_Accertamenti" & Me.IdSessione) = value
        End Set
    End Property

    Public Property Classificazioni() As List(Of ParsecAtt.DocumentoClassificazione)
        Get
            Return CType(Session("AttoAmministrativoPage_Classificazioni" & Me.IdSessione), List(Of ParsecAtt.DocumentoClassificazione))
        End Get
        Set(ByVal value As List(Of ParsecAtt.DocumentoClassificazione))
            Session("AttoAmministrativoPage_Classificazioni" & Me.IdSessione) = value
        End Set
    End Property

    Public Property Presenze() As List(Of ParsecAtt.DocumentoPresenza)
        Get
            Return CType(Session("AttoAmministrativoPage_Presenze" & Me.IdSessione), List(Of ParsecAtt.DocumentoPresenza))
        End Get
        Set(ByVal value As List(Of ParsecAtt.DocumentoPresenza))
            Session("AttoAmministrativoPage_Presenze" & Me.IdSessione) = value
        End Set
    End Property

    Public Property Firme() As List(Of ParsecAtt.Firma)
        Get
            Return CType(Session("AttoAmministrativoPage_Firme" & Me.IdSessione), List(Of ParsecAtt.Firma))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Firma))
            Session("AttoAmministrativoPage_Firme" & Me.IdSessione) = value
        End Set
    End Property

    Public Property Allegati() As List(Of ParsecAtt.Allegato)
        Get
            Return CType(Session("AttoAmministrativoPage_Allegati" & Me.IdSessione), List(Of ParsecAtt.Allegato))
        End Get
        Set(ByVal value As List(Of ParsecAtt.Allegato))
            Session("AttoAmministrativoPage_Allegati" & Me.IdSessione) = value
        End Set
    End Property


    Public Property AllegatiPubblicazione() As List(Of ParsecAdmin.AllegatoPubblicazione)
        Get
            Return CType(Session("AttoAmministrativoPage_AllegatiPubblicazione" & Me.IdSessione), List(Of ParsecAdmin.AllegatoPubblicazione))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.AllegatoPubblicazione))
            Session("AttoAmministrativoPage_AllegatiPubblicazione" & Me.IdSessione) = value
        End Set
    End Property

    Public ReadOnly Property IterAttivato As Boolean
        Get
            Dim res As Boolean = False
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AttivaGestioneScrivanieAtt", ParsecAdmin.TipoModulo.SEP)
            If Not parametro Is Nothing Then
                res = CBool(parametro.Valore)
            End If
            parametri.Dispose()
            Return res
        End Get
    End Property

    Public ReadOnly Property AbilitaNumerazioneDeliberaManuale As Boolean
        Get
            Dim res As Boolean = False
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("abilitaContatoreGeneraleDeliberaManuale", ParsecAdmin.TipoModulo.ATT)
            If Not parametro Is Nothing Then
                res = CBool(parametro.Valore)
            End If
            parametri.Dispose()
            Return res
        End Get
    End Property

    Public Property Rigenera As Boolean
        Get
            Return CBool(ViewState("Rigenera"))
        End Get
        Set(value As Boolean)
            ViewState("Rigenera") = value
        End Set
    End Property

    Public Property NascondiDocumento As Boolean
        Get
            Return CBool(ViewState("NascondiDocumento"))
        End Get
        Set(value As Boolean)
            ViewState("NascondiDocumento") = value
        End Set
    End Property

    Public Property CararicaDaDB As Boolean
        Get
            Return CBool(ViewState("CararicaDaDB"))
        End Get
        Set(value As Boolean)
            ViewState("CararicaDaDB") = value
        End Set
    End Property

    Public Property NomeFileCopia As String
        Get
            Return CStr(ViewState("NomeFileCopia"))
        End Get
        Set(value As String)
            ViewState("NomeFileCopia") = value
        End Set
    End Property

    'Elenco di gruppi o di utenti abilitati a visualizzare un protcollo.
    Public Property Visibilita() As List(Of ParsecAdmin.VisibilitaDocumento)
        Get
            Return Session("AttoAmministrativoPage_Visibilita" & Me.IdSessione)
        End Get
        Set(ByVal value As List(Of ParsecAdmin.VisibilitaDocumento))
            Session("AttoAmministrativoPage_Visibilita" & Me.IdSessione) = value
        End Set
    End Property

    'luca 01/07/2020
    '' ''Public Property Trasparenza() As ParsecAdmin.Pubblicazione
    '' ''    Get
    '' ''        Return Session("AttoAmministrativoPage_Trasparenza" & Me.IdSessione)
    '' ''    End Get
    '' ''    Set(ByVal value As ParsecAdmin.Pubblicazione)
    '' ''        Session("AttoAmministrativoPage_Trasparenza" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    Public Property UfficioSelezionato As Integer
        Get
            Return CInt(ViewState("UfficioSelezionato"))
        End Get
        Set(value As Integer)
            ViewState("UfficioSelezionato") = value
        End Set
    End Property

    Public Property DocumentoCopia() As ParsecAtt.Documento
        Get
            Return CType(Session("AttoAmministrativoPage_DocumentoCopia" & Me.IdSessione), ParsecAtt.Documento)
        End Get
        Set(ByVal value As ParsecAtt.Documento)
            Session("AttoAmministrativoPage_DocumentoCopia" & Me.IdSessione) = value
        End Set
    End Property

    Public Property MessaggioAvviso As String
        Get
            Return ViewState("MessaggioAvviso")
        End Get
        Set(value As String)
            ViewState("MessaggioAvviso") = value
        End Set
    End Property

    Public Property DataSource() As String
        Get
            Return CType(Session("AttoAmministrativoPage_DataSource" & Me.IdSessione), String)
        End Get
        Set(ByVal value As String)
            Session("AttoAmministrativoPage_DataSource" & Me.IdSessione) = value
        End Set
    End Property

    'luca 01/07/2020
    '' ''Public Property AttiConcessione() As List(Of ParsecAdmin.AttoConcessione)
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_AttiConcessione" & Me.IdSessione), List(Of ParsecAdmin.AttoConcessione))
    '' ''    End Get
    '' ''    Set(ByVal value As List(Of ParsecAdmin.AttoConcessione))
    '' ''        Session("AttoAmministrativoPage_AttiConcessione" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property AttoConcessione() As ParsecAdmin.AttoConcessione
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_AttoConcessione" & Me.IdSessione), ParsecAdmin.AttoConcessione)
    '' ''    End Get
    '' ''    Set(ByVal value As ParsecAdmin.AttoConcessione)
    '' ''        Session("AttoAmministrativoPage_AttoConcessione" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property Consulenza() As ParsecAdmin.Consulenza
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_Consulenza" & Me.IdSessione), ParsecAdmin.Consulenza)
    '' ''    End Get
    '' ''    Set(ByVal value As ParsecAdmin.Consulenza)
    '' ''        Session("AttoAmministrativoPage_Consulenza" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property IncaricoAmministrativoDirigenziale() As ParsecAdmin.IncaricoAmministrativoDirigenziale
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_IncaricoAmministrativoDirigenziale" & Me.IdSessione), ParsecAdmin.IncaricoAmministrativoDirigenziale)
    '' ''    End Get
    '' ''    Set(ByVal value As ParsecAdmin.IncaricoAmministrativoDirigenziale)
    '' ''        Session("AttoAmministrativoPage_IncaricoAmministrativoDirigenziale" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property BandoConcorso() As ParsecAdmin.BandoConcorso
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_BandoConcorso" & Me.IdSessione), ParsecAdmin.BandoConcorso)
    '' ''    End Get
    '' ''    Set(ByVal value As ParsecAdmin.BandoConcorso)
    '' ''        Session("AttoAmministrativoPage_BandoConcorso" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property EnteControllato() As ParsecAdmin.EnteControllato
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_EnteControllato" & Me.IdSessione), ParsecAdmin.EnteControllato)
    '' ''    End Get
    '' ''    Set(ByVal value As ParsecAdmin.EnteControllato)
    '' ''        Session("AttoAmministrativoPage_EnteControllato" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property TrattamentiEconomiciRappresentante() As List(Of ParsecAdmin.TrattamentoEconomicoRappresentante)
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_TrattamentiEconomiciRappresentante" & Me.IdSessione), List(Of ParsecAdmin.TrattamentoEconomicoRappresentante))
    '' ''    End Get
    '' ''    Set(ByVal value As List(Of ParsecAdmin.TrattamentoEconomicoRappresentante))
    '' ''        Session("AttoAmministrativoPage_TrattamentiEconomiciRappresentante" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property TrattamentiEconomiciIncaricoAmministratore() As List(Of ParsecAdmin.TrattamentoEconomicoIncaricoAmministratore)
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_TrattamentiEconomiciIncaricoAmministratore" & Me.IdSessione), List(Of ParsecAdmin.TrattamentoEconomicoIncaricoAmministratore))
    '' ''    End Get
    '' ''    Set(ByVal value As List(Of ParsecAdmin.TrattamentoEconomicoIncaricoAmministratore))
    '' ''        Session("AttoAmministrativoPage_TrattamentiEconomiciIncaricoAmministratore" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property BilanciEnteControllato() As List(Of ParsecAdmin.BilancioEsercizioEnteControllato)
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_BilanciEnteControllato" & Me.IdSessione), List(Of ParsecAdmin.BilancioEsercizioEnteControllato))
    '' ''    End Get
    '' ''    Set(ByVal value As List(Of ParsecAdmin.BilancioEsercizioEnteControllato))
    '' ''        Session("AttoAmministrativoPage_BilanciEnteControllato" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property Pubblicazione() As ParsecPub.Pubblicazione
    '' ''    Get
    '' ''        Return CType(Session("AttoAmministrativoPage_Pubblicazione" & Me.IdSessione), ParsecPub.Pubblicazione)
    '' ''    End Get
    '' ''    Set(ByVal value As ParsecPub.Pubblicazione)
    '' ''        Session("AttoAmministrativoPage_Pubblicazione" & Me.IdSessione) = value
    '' ''    End Set
    '' ''End Property

    'luca 01/07/2020
    '' ''Public Property ProvvedimentoPubblicato As Boolean
    '' ''    Get
    '' ''        Return CBool(ViewState("ProvvedimentoPubblicato"))
    '' ''    End Get
    '' ''    Set(value As Boolean)
    '' ''        ViewState("ProvvedimentoPubblicato") = value
    '' ''    End Set
    '' ''End Property

    Public ReadOnly Property LarghezzaContenitore As String
        Get
            Dim widthStyle As String = "auto"
            Dim browser = Request.Browser.Browser
            If browser.ToLower.Contains("ie") Then
                widthStyle = "100%"
            End If
            Return widthStyle
        End Get
    End Property

    Public Property Fascicoli() As List(Of ParsecAdmin.Fascicolo)
        Get
            Return CType(Session("AttoAmministrativoPage_Fascicoli" & Me.IdSessione), List(Of ParsecAdmin.Fascicolo))
        End Get
        Set(ByVal value As List(Of ParsecAdmin.Fascicolo))
            Session("AttoAmministrativoPage_Fascicoli" & Me.IdSessione) = value
        End Set
    End Property

    Public Property Fascicolo() As ParsecAdmin.Fascicolo
        Get
            Return CType(Session("AttoAmministrativoPage_Fascicolo" & Me.IdSessione), ParsecAdmin.Fascicolo)
        End Get
        Set(ByVal value As ParsecAdmin.Fascicolo)
            Session("AttoAmministrativoPage_Fascicolo" & Me.IdSessione) = value
        End Set
    End Property

    Public Property FiltroFascicolo() As ParsecAdmin.FascicoloFiltro
        Get
            Return CType(Session("AttoAmministrativoPage_FiltroFascicolo" & Me.IdSessione), ParsecAdmin.FascicoloFiltro)
        End Get
        Set(ByVal value As ParsecAdmin.FascicoloFiltro)
            Session("AttoAmministrativoPage_FiltroFascicolo" & Me.IdSessione) = value
        End Set
    End Property


    Public Property OperazioneAvvenuta As Boolean
        Get
            Return CBool(ViewState("OperazioneAvvenuta"))
        End Get
        Set(value As Boolean)
            ViewState("OperazioneAvvenuta") = value
        End Set
    End Property


    Private Property FullSize As Boolean
        Set(value As Boolean)
            ViewState("FullSize") = value
        End Set
        Get
            Return ViewState("FullSize")
        End Get
    End Property


    Public ReadOnly Property TipologiaGestioneContabilita As ParsecAtt.TipologiaGestioneContabilita
        Get
            If Session("AttoAmministrativoPage_TipologiaGestioneContabilita" & Me.IdSessione) Is Nothing Then
                Dim parametri As New ParsecAdmin.ParametriRepository
                Dim parametro = parametri.GetByName("TipologiaGestioneContabilita")
                parametri.Dispose()

                Dim tipologia As ParsecAtt.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.Classica
                If Not parametro Is Nothing Then
                    Try
                        tipologia = CType(parametro.Valore, ParsecAtt.TipologiaGestioneContabilita)
                    Catch ex As Exception
                    End Try
                End If
                Session("AttoAmministrativoPage_TipologiaGestioneContabilita" & Me.IdSessione) = tipologia
            End If
            Return Session("AttoAmministrativoPage_TipologiaGestioneContabilita" & Me.IdSessione)
        End Get
    End Property

#End Region

#Region "ITER"

    Public Sub CreaIstanzaAtt(ByVal documento As ParsecAtt.Documento)

        Dim roles As New List(Of String)
        Dim role As ParsecWKF.Ruolo = Nothing
        Dim nomeAttore As String = String.Empty

        Dim statoDaEseguire As Integer = 5

        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanzaPrecedente As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.ContatoreGenerale = documento.ContatoreGenerale AndAlso c.IdModulo = 3 AndAlso Year(c.DataInserimento) = Year(documento.DataDocumento.Value)).FirstOrDefault

        Dim esiste As Boolean = False

        If Not istanzaPrecedente Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim doc = documenti.GetQuery.Where(Function(c) c.Id = istanzaPrecedente.IdDocumento).FirstOrDefault
            If doc.Codice = documento.Codice Then
                esiste = True
            End If
            documenti.Dispose()
        End If

        '*********************************************************************************************************************
        'Se l'istanza del documento non è in iter
        '*********************************************************************************************************************
        If Not esiste Then
            If Not documento.Modello Is Nothing Then
                Dim idModelloIter = documento.Modello.IdIter
                Dim modelliIter As New ParsecWKF.ModelliRepository
                Dim modelloIter As ParsecWKF.Modello = modelliIter.GetById(idModelloIter)
                modelliIter.Dispose()

                '*********************************************************************************************************************
                'Inserisco l'istanza
                '*********************************************************************************************************************

                'Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository
                'Dim tipologiaRegistro As ParsecAtt.TipologiaRegistro = tipologieRegistro.GetById(documento.IdTipologiaRegistro)
                'Dim descrizioneDocumento As String = String.Format("{0} n. {1}/{2} - Oggetto : {3}", tipologiaRegistro.Descrizione, documento.ContatoreGenerale.ToString, documento.DataDocumento.Value.Year.ToString, documento.Oggetto)


                Dim statoIniziale As Integer = 1
                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                Dim idMittente As Integer = utenteCollegato.Id


                Dim istanza As New ParsecWKF.Istanza
                istanza.Riferimento = documento.GetDescrizione
                istanza.IdStato = statoIniziale
                istanza.DataInserimento = Now
                istanza.DataScadenza = Now.AddDays(modelloIter.DurataIter)
                istanza.IdModello = modelloIter.Id
                istanza.IdDocumento = documento.Id
                istanza.Ufficio = 0
                istanza.ContatoreGenerale = documento.ContatoreGenerale
                istanza.IdModulo = modelloIter.RiferimentoModulo
                istanza.IdUtente = idMittente
                istanza.FileIter = modelloIter.NomeFile

                Try


                    istanze.Save(istanza)
                    Dim idIstanza As Integer = istanze.Istanza.Id
                    istanze.Dispose()

                    '*********************************************************************************************************************
                    'Inserisco il task dell'istanza appena inserita
                    '*********************************************************************************************************************
                    Dim tasks As New ParsecWKF.TaskRepository
                    Dim task As New ParsecWKF.Task
                    task.IdIstanza = istanze.Istanza.Id
                    task.Nome = "" 'modello.StatoIniziale
                    task.Corrente = modelloIter.StatoIniziale
                    task.Successivo = modelloIter.StatoSuccessivo(modelloIter.StatoIniziale)

                    Dim actors = ParsecWKF.ModelloInfo.ReadActors(modelloIter.NomeFile)

                    Dim list As List(Of ParsecWKF.Action) = ParsecWKF.ModelloInfo.ReadActionInfo(task.Corrente, modelloIter.NomeFile)
                    'Recupero il nome del ruolo (To) associato all'azione.
                    Dim roleToName = list(0).ToActor
                    role = (New ParsecWKF.RuoloRepository).GetQuery.Where(Function(c) c.Descrizione = roleToName).FirstOrDefault

                    Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
                    Dim parametroProcesso As ParsecWKF.ParametroProcesso = Nothing

                    Dim ruoli As New ParsecWKF.RuoloRepository

                    Dim documenti As New ParsecAtt.DocumentoRepository
                    Dim firme = documenti.GetFirme(documento.Id)
                    documenti.Dispose()


                    Dim strutture As New ParsecAdmin.StructureRepository

                    For Each firma As ParsecAtt.Firma In firme
                        Dim idRuolo As Integer = firma.IdRuolo
                        Dim ruolo = ruoli.GetQuery.Where(Function(c) c.Id = idRuolo).FirstOrDefault

                        If Not ruolo Is Nothing Then
                            Dim attore = actors.Where(Function(c) c.Name.ToLower = ruolo.Descrizione.ToLower).FirstOrDefault
                            If Not attore Is Nothing Then
                                parametroProcesso = New ParsecWKF.ParametroProcesso With {.IdProcesso = idIstanza, .Nome = attore.Name, .Valore = firma.IdUtente.ToString}
                                parametriProcesso.Add(parametroProcesso)
                                parametriProcesso.SaveChanges()
                            End If
                        End If
                    Next

                    strutture.Dispose()

                    parametriProcesso.Dispose()

                    task.Mittente = idMittente
                    'task.Destinatario = IdDestinatario
                    task.IdStato = statoDaEseguire
                    task.DataInizio = Now
                    task.DataEsecuzione = Now
                    task.DataFine = Now.AddDays(modelloIter.DurataTask)
                    task.Operazione = "NUOVO"
                    task.Cancellato = False
                    task.IdUtenteOperazione = utenteCollegato.Id

                    Try
                        tasks.Save(task)
                        tasks.Dispose()

                        'istanza = istanze.GetById(istanza.Id)
                        'istanza.FileIter = modelloIter.NomeFile
                        'istanze.SaveChanges()

                        'Aggiungo il nuovo task
                        Me.Procedi(task, istanza, modelloIter.AzioneIniziale, idMittente)

                    Catch ex As Exception
                        Throw New ApplicationException(ex.Message)
                    End Try
                Catch ex As Exception
                    Throw New ApplicationException(ex.Message)
                End Try

            End If
        End If

        istanze.Dispose()

    End Sub

    Private Sub Procedi(ByVal taskAttivo As ParsecWKF.Task, ByVal istanzaAttiva As ParsecWKF.Istanza, ByVal action As String, ByVal idUtente As Integer)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim tasks As New ParsecWKF.TaskRepository

        'Dim watch = System.Diagnostics.Stopwatch.StartNew()
        tasks.Attach(taskAttivo)

        'Dim taskAttivo As ParsecWKF.TaskAttivo = tasks.GetView(New ParsecWKF.TaskFiltro With {.IdUtente = idUtente, .IdModulo = ParsecAdmin.TipoModulo.ATT}).Where(Function(c) c.IdIstanza = istanzaAttiva.Id).FirstOrDefault

        'watch.Stop()
        'Dim elapsedMs = watch.ElapsedMilliseconds



        Dim nomeFileIter As String = istanzaAttiva.FileIter


        Dim statoEseguito As Integer = 6
        Dim statoDaEseguire As Integer = 5

        Dim operazione As String = "INIZIO"

        '*********************************************************************************************************************
        'Aggiorno il task corrente
        '*********************************************************************************************************************

        'Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = taskAttivo.Id).FirstOrDefault
        'taskAttivo.Note = ""
        taskAttivo.IdStato = statoEseguito
        taskAttivo.DataEsecuzione = Now
        'taskAttivo.Destinatario = destinatario.Id
        taskAttivo.Operazione = operazione
        tasks.SaveChanges()


        Dim statoSuccessivo As String = ParsecWKF.ModelloInfo.StatoSuccessivoAction(taskAttivo.Corrente, action, nomeFileIter)
        Dim durata As Integer = ParsecWKF.ModelloInfo.DurataTaskIter(statoSuccessivo, nomeFileIter)

        '*********************************************************************************************************************
        'Inserisco il nuovo task 
        '*********************************************************************************************************************
        Dim nuovotask As New ParsecWKF.Task
        nuovotask.IdIstanza = taskAttivo.IdIstanza
        nuovotask.TaskPadre = taskAttivo.Id

        nuovotask.Nome = taskAttivo.Corrente
        nuovotask.Corrente = statoSuccessivo
        nuovotask.Successivo = ParsecWKF.ModelloInfo.StatoSuccessivoIter(statoSuccessivo, nomeFileIter)

        nuovotask.Mittente = idUtente 'destinatario.Id
        ' nuovotask.Destinatario = 
        nuovotask.IdStato = statoDaEseguire
        nuovotask.DataInizio = Now
        'nuovotask.Note = ""
        'task.DataEsecuzione = Now
        nuovotask.DataFine = Now.AddDays(durata)
        nuovotask.Cancellato = False
        nuovotask.Notificato = False
        nuovotask.IdUtenteOperazione = utenteCollegato.Id

        tasks.Add(nuovotask)
        tasks.SaveChanges()

        tasks.Dispose()


    End Sub

    Public Sub AggiornaIstanzaWorkflow(ByVal idDocumento As Integer, ByVal documento As ParsecAtt.Documento, ByVal idModulo As Integer)
        Dim istanze As New ParsecWKF.IstanzaRepository
        Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.IdDocumento = idDocumento AndAlso c.IdModulo = idModulo).FirstOrDefault
        If Not istanza Is Nothing Then
            istanza.IdDocumento = documento.Id
            istanza.Riferimento = documento.GetDescrizione
            istanza.ContatoreGenerale = documento.ContatoreGenerale
        End If
        istanze.SaveChanges()
        istanze.Dispose()
    End Sub

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.Request("Mode") Is Nothing Then
            Me.MasterPageFile = "~/BasePage.master"
        End If
    End Sub


    'da spostare in ChiudiApplicazionePage
    Private Sub CancellaDataSoucesUtenteCollegato()

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim path = ParsecAdmin.WebConfigSettings.GetKey("PathDownload")
        Dim files = IO.Directory.GetFiles(path, "*.txt")
        Dim nomefile As String = String.Empty
        Try
            For Each f In files
                nomefile = IO.Path.GetFileName(f)
                If nomefile.StartsWith("Utente_" & utenteCollegato.Id.ToString & "_DataSource") Then
                    IO.File.Delete(f)
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub


   

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init



        'Dim adetti As New ParsecAdmin.UserRepository
        'Dim f = adetti.Where(Function(c) c.Id = 1).Select(Function(c) System.Data.Objects.EntityFunctions.DiffMinutes(c.DataNascita, c.DataUltimoSettaggioPsw)).Sum
        'Dim f = adetti.GetQuery.Where("id=1").Select("SUM(DATEDIFF('minute', uteDataNascita, uteDataUltimoSettaggioPsw))")
        'Dim m = f.Cast(Of Integer).FirstOrDefault()

        'todo rimuovere
        'Me.AllegatiGridView.MasterTableView.GetColumnSafe("Firma").Display = False
        'Me.AllegatiGridView.MasterTableView.GetColumnSafe("SignedPreview").Display = False


        'La prima volta imposto lo stato predefinito della pagina.
        If Not Me.Page.IsPostBack Then
            Me.IdSessione = Now.Ticks.ToString
            Me.FullSize = False
            'luca 01/07/2020
            '' ''Me.IdGaraContratti = String.Empty
        End If

        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False

        'GESTIONE PER TIPOLOGIA DI DOCUMENTO 
        'DISABILITO LA POSSIBILITA' DI CAMBIARE LA TIPOLOGIA DI DOCUMENTO
        Me.TipologieDocumentoComboBox.Enabled = False

        Me.OggettoTextBox.Attributes.Add("onblur", "this.style.borderColor=''")
        Me.OggettoTextBox.Attributes.Add("onmouseout", "this.style.borderColor=''")
        Me.OggettoTextBox.Attributes.Add("onfocus", "this.style.borderColor='#305090'")
        Me.OggettoTextBox.Attributes.Add("onmouseover", "this.style.borderColor='#305090'")

        Dim css As HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".CustomFooter .RadGrid .rgFooter { background-image: none;background-color:#DFE8F6;}" & vbCrLf
        css.InnerHtml &= " div.RadGrid .rgFilterRow td {  padding: 3px;}" & vbCrLf
        Me.Page.Header.Controls.Add(css)


        css = New HtmlGenericControl
        css = New HtmlGenericControl
        css.TagName = "style"
        css.Attributes.Add("type", "text/css")
        css.InnerHtml = ".RadListBox .rlbCheck,.RadListBox .rlbImage,.RadListBox .rlbText{vertical-align: middle;padding-left:5px;}"
        Me.Page.Header.Controls.Add(css)


        'Me.NumeroRegistroPubblicazioneTextBox.ReadOnly = False

        Me.Rigenera = True
        Me.NascondiDocumento = False

        If Page.Request("Mode") Is Nothing Then
            Me.MainPage = CType(Me.Master, MainPage)
            Me.MainPage.NomeModulo = "Atti Decisionali"
            Me.MainPage.DescrizioneProcedura = "> Atto Amministrativo"
        Else
            Me.MainPage = CType(Me.Master, BasePage)
            CType(Me.Master, BasePage).ImpostaLarghezzaHeader(900)
            If Page.Request("Mode") = "View" Then
                Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
                CType(Me.Master, BasePage).DescrizioneProcedura = "Dettaglio Atto Decisionale"
            End If
            If Not Page.Request("Rigenera") Is Nothing Then
                Me.Rigenera = CBool(Page.Request("Rigenera"))
            End If
            If Not Page.Request("NascondiDocumento") Is Nothing Then
                Me.NascondiDocumento = CBool(Page.Request("NascondiDocumento"))
            End If
        End If

        Dim tipoDocumentoPredefinito As Integer = CInt(Request.QueryString("Tipo"))
        If Not Me.Page.IsPostBack Then
            Me.TipologiaDocumento = CType(tipoDocumentoPredefinito, ParsecAtt.TipoDocumento)
            Me.TipologiaDocumentoApertura = Me.TipologiaDocumento
        End If

        Me.CaricaTipologieDocumento()
        Me.CaricaStatiDiscussione()

        'luca 01/07/2020
        '' ''Me.CaricaTipologieSceltaContraente()

        Me.CaricaTipologieAllegati()

        Me.CaricaFasi()

        Me.CaricaModelli()

        'La prima volta imposto lo stato predefinito della pagina.
        If Not Me.Page.IsPostBack Then

            Me.VisualizzaFatturaControl.Visible = False

            Me.CancellaDataSoucesUtenteCollegato()

            Me.NumeroSettoreTable.Visible = False
            Me.NumeroSettoreTextBox.Enabled = False
            Me.DataSource = Nothing
            Me.UfficioSelezionato = 0
            Me.Documenti = Nothing
            Me.DocumentoCopia = Nothing
            Me.CararicaDaDB = True
            Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Nuovo
            Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Nuovo
            If Not Request.QueryString("Procedura") Is Nothing Then
                Me.TipologiaProceduraApertura = CInt(Request.QueryString("Procedura"))
            End If
            Select Case Me.TipologiaProceduraApertura
                Case ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.Numerazione, ParsecAtt.TipoProcedura.Pubblicazione, ParsecAtt.TipoProcedura.Classificazione, ParsecAtt.TipoProcedura.ModificaAmministrativa, ParsecAtt.TipoProcedura.AggiungiDatiContabili
                    Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Ricerca
            End Select
            Me.TipologieDocumentoComboBox.FindItemByValue(tipoDocumentoPredefinito).Selected = True
            'Dim sortExpr As New GridSortExpression()
            'sortExpr.FieldName = "LogDataRegistrazione"
            'sortExpr.SortOrder = GridSortOrder.Descending
            'Me.DocumentiGridView.MasterTableView.SortExpressions.AddSortExpression(sortExpr)
            Me.AttiTabStrip.MultiPage.PageViews(0).Selected = True
            Me.AttiTabStrip.Tabs(0).Selected = True
            Me.ResettaVista()
            Me.DisabilitaUI()
            Me.ImpostaDescrizioneTipologiaDocumento(Me.TipologiaDocumento, Me.TipologiaProceduraApertura)

            If Not Request.QueryString("Mode") Is Nothing Then
                Me.GetParametri()
                Me.AggiornaVista(Me.Documento, True)
            End If

            Me.AbilitaUI(Me.TipologiaProceduraCorrente)

            'Me.EspRicXls.Visible = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto Or Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera Or _
            '                        Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Or Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Direttiva Or Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza)
        End If


        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            Me.RegistraScansione()
            Me.RegistraParsecOpenOffice()
            Me.RegistraParsecDigitalSign()
        End If




        Me.DisabilitaPulsantePredefinito.Attributes.Add("onclick", "return false;")
        Me.RegistraMarcaturaTesto()
        Me.DelimitaTestoImageButton.Attributes.Add("onclick", "Racchiudi();return false;")
        'Me.OggettoTextBox.Attributes.Add("onmouseup", "GetSelection(this)")


        '********************************************************************************************************************************
        'Gestione chiusura finestra dalla X della barra del titolo. 
        '********************************************************************************************************************************
        ParsecUtility.Utility.CloseWindow(False)
        'WINDOW.CLOSE GENERA L'EVENTO WINDOW.ONBEFOREUNLOAD (REGISTRATO CON ParsecUtility.Utility.CloseWindow(False) )
        'CHE ESEGUE IL CLIC SUL PULSANTE PASSATO NELLA QUERY STRING DELLA WINDOW.OPEN 
        '********************************************************************************************************************************

        Dim livelli As New ParsecAdmin.StrutturaLivelloRepository
        Dim primoLivelloStruttura As ParsecAdmin.StrutturaLivello = livelli.GetQuery.Where(Function(c) c.Gerarchia = 100).FirstOrDefault
        Dim secondoLivelloStruttura As ParsecAdmin.StrutturaLivello = livelli.GetQuery.Where(Function(c) c.Gerarchia = 200).FirstOrDefault

        If Not primoLivelloStruttura Is Nothing Then
            Me.SettoreLabel.Text = primoLivelloStruttura.Descrizione
        End If

        If Not secondoLivelloStruttura Is Nothing Then
            Me.UfficioLabel.Text = secondoLivelloStruttura.Descrizione & " *"
        End If

        livelli.Dispose()
        'luca 01/07/2020
        '' ''ParsecUtility.Utility.RegisterDefaultButton(Me.SezioneTrasparenzaTextBox, Me.TrovaSezioneImageButton)
        '' ''ParsecUtility.Utility.RegisterDefaultButton(Me.FiltroBandoGaraTextBox, Me.TrovaBandoGaraImageButton)

        Select Case Me.TipologiaProceduraApertura
            Case ParsecAtt.TipoProcedura.AggiungiDatiContabili, ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.Classificazione, ParsecAtt.TipoProcedura.Numerazione, ParsecAtt.TipoProcedura.Pubblicazione, ParsecAtt.TipoProcedura.ModificaAmministrativa
                Me.DocumentiGridView.MasterTableView.GetColumnSafe("Copy").Visible = False
        End Select

        'Me.OggettoTextBox.Attributes.Add("onfocus", "var timeoutid = null; var obj= this;var truncatetext = function() { if(obj.value.length > 1500) { obj.value = obj.value.substring(0, 1500); }};  if(timeoutid){  clearInterval(timeoutid); } timeoutid = setInterval(truncatetext, 100);")

        Me.chiudiImageButton.Attributes.Add("onclick", "window.close();return false;")

        Dim widthStyle As String = "auto"
        Dim browser = Request.Browser.Browser
        If browser.ToLower.Contains("ie") Then
            widthStyle = "100%"
        End If

        Me.ClassificazioniPanel.Style.Add("width", widthStyle)
        Me.DettaglioClassificazionePanel.Style.Add("width", widthStyle)
        Me.GrigliaClassificazioniPanel.Style.Add("width", widthStyle)
        'luca 01/07/2020
        '' ''Me.TrasparenzaPanel.Style.Add("width", widthStyle)
        '' ''Me.AttiConsessionePanel2.Style.Add("width", widthStyle)

        Me.VisibilitaPanel.Style.Add("width", widthStyle)
        Me.AllegatiPanel.Style.Add("width", widthStyle)
        Me.GrigliaAllegatiPanel.Style.Add("width", widthStyle)
        Me.ContabilitaPanel.Style.Add("width", widthStyle)
        Me.GeneralePanel.Style.Add("width", widthStyle)
        Me.AttiMultiPage.Style.Add("width", widthStyle)
        Me.FirmeGridView.Style.Add("width", widthStyle)
        Me.DocumentiGridView.Style.Add("width", widthStyle)
        Me.PresenzeGridView.Style.Add("width", widthStyle)
        Me.ImpegniSpesaGridView.Style.Add("width", widthStyle)
        Me.LiquidazioniGridView.Style.Add("width", widthStyle)
        Me.AccertamentiGridView.Style.Add("width", widthStyle)
        Me.AllegatiGridView.Style.Add("width", widthStyle)
        Me.ClassificazioniGridView.Style.Add("width", widthStyle)
        Me.VisibilitaGridView.Style.Add("width", widthStyle)
        Me.PresenzePanel.Style.Add("width", widthStyle)
        Me.GrigliaPresenzePanel.Style.Add("width", widthStyle)
        'luca 01/07/2020
        '' ''Me.DenominazioneConsulenzaTextBox.Style.Add("overflow-x", "hidden")
        '' ''Me.RagioneIncaricoConsulenzaTextBox.Style.Add("overflow-x", "hidden")
        '' ''Me.CollaborazioneConsulenzaPanel.Style.Add("width", widthStyle)
        '' ''Me.OggettoBandoGaraTextBox.Style.Add("width", widthStyle)
        ' '' ''PUBBLICAZIONE GENERICA
        '' ''Me.PubblicazioneGenericaPanel.Style.Add("width", widthStyle)


        Me.GrigliaFascicoliPanel.Style.Add("width", widthStyle)
        Me.FascicoliGridView.Style.Add("width", widthStyle)
        Me.FascicoliPanel.Style.Add("width", widthStyle)

        Me.FullSizeDocumentiGridView.Style.Add("width", widthStyle)

        'luca 01/07/2020
        '' ''Me.AllegatiBandoGaraPanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiBandoGaraPanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiBandoGaraGridView.Style.Add("width", widthStyle)


        '' ''Me.AllegatiPubblicazioneGenericaPanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiPubblicazioneGenericaPanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiPubblicazioneGenericaGridView.Style.Add("width", widthStyle)


        '' ''Me.AllegatiIncaricoDipendentePanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiIncaricoDipendentePanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiIncaricoDipendenteGridView.Style.Add("width", widthStyle)

        '' ''Me.AllegatiCompensoConsulenzaPanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiCompensoConsulenzaPanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiCompensoConsulenzaGridView.Style.Add("width", widthStyle)



        '' ''Me.AllegatiAttiConcessionePanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiAttiConcessionePanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiAttiConcessioneGridView.Style.Add("width", widthStyle)
        '' ''Me.BeneficiariGridView.Style.Add("width", widthStyle)

        '' ''Me.AllegatiIncarichiAmministrativiDirigenzialiPanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiIncarichiAmministrativiDirigenzialiPanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiIncarichiAmministrativiDirigenzialiGridView.Style.Add("width", widthStyle)


        '' ''Me.AllegatiBandoConcorsoPanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiBandoConcorsoPanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiBandoConcorsoGridView.Style.Add("width", widthStyle)

        '' ''Me.AllegatiEnteControllatoPanel.Style.Add("width", widthStyle)
        '' ''Me.GrigliaAllegatiEnteControllatoPanel.Style.Add("width", widthStyle)
        '' ''Me.AllegatiEnteControllatoGridView.Style.Add("width", widthStyle)

        '' ''Me.TrattamentiEconomiciRappresentantiEnteControllatoPanel.Style.Add("width", widthStyle)
        '' ''Me.TrattamentiEconomiciRappresentantiEnteControllatoGridView.Style.Add("width", widthStyle)

        '' ''Me.TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView.Style.Add("width", widthStyle)
        '' ''Me.TrattamentiEconomiciIncarichiAmministratoreEnteControllatoPanel.Style.Add("width", widthStyle)

        '' ''Me.BilanciEnteControllatoPanel.Style.Add("width", widthStyle)
        '' ''Me.BilanciEnteControllatoGridView.Style.Add("width", widthStyle)


        'FIREFOX AGGIUNGE ALTEZZA ALLA TEXTAREA PER RISERVARE IL POSTO ALLA SCROLLBAR ORIZZONTALE 
        'Me.OggettoTextBox.Style.Add("overflow-x", "hidden")
        'Me.AnnotazioniTextBox.Style.Add("overflow-x", "hidden")
        'Me.NoteTextBox.Style.Add("overflow-x", "hidden")
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Annullare il documento selezionato?", False, Not Me.Documento Is Nothing)
        ParsecUtility.Utility.ConfirmDelete("Si è verificato un problema durante il salvataggio del documento! Premere OK per rigenerarlo.", False, "Salvataggio")

        Me.AggiornaGrigliaLiquidazioni()
        Me.AggiornaGrigliaImpegniSpesa()
        Me.AggiornaGrigliaAccertamenti()
        Me.AggiornaGrigliaClassificazioni()
        Me.AggiornaGrigliaPresenze()
        Me.AggiornaGrigliaFirme()
        Me.AggiornaGrigliaAllegati()
        Me.AggiornaGrigliaVisibilita()

        'luca 01/07/2020
        '' ''Me.AggiornaGrigliaAttiConcessione()
        '' ''Me.AggiornaGrigliaAllegatiPubblicazione()


        Me.AggiornaGrigliaFascicoli()


        Dim presenzeTab As RadTab = Me.AttiTabStrip.FindTabByText("Presenze")

        Select Case Me.TipologiaProceduraApertura
            Case ParsecAtt.TipoProcedura.Nuovo, ParsecAtt.TipoProcedura.Modifica, ParsecAtt.TipoProcedura.Visualizzazione
                presenzeTab.Enabled = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera)
            Case ParsecAtt.TipoProcedura.Classificazione, ParsecAtt.TipoProcedura.Pubblicazione, ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.AggiungiDatiContabili, ParsecAtt.TipoProcedura.ModificaAmministrativa
                presenzeTab.Enabled = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera) And Not Me.Documento Is Nothing
            Case ParsecAtt.TipoProcedura.Numerazione
                presenzeTab.Enabled = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDelibera And Not Me.Documento Is Nothing)
        End Select
    

        Me.AttiTabStrip.Tabs.FindTabByText("Classificazioni").Enabled = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto)
        Dim idModello As Integer = CInt(Me.ModelliComboBox.SelectedValue)
        Me.AbilitaUiBaseModello(idModello)

        '**************************************************************
        'Gestione visibilità pulsanti e descrizioni barra di notifica.
        '**************************************************************
        Me.VisualizzaDocumentoCollegatoImageButton.Visible = False
        Me.VisualizzaCopiaDocumentoCollegatoImageButton.Visible = False
        Me.VisualizzaStoricoDocumentoImageButton.Visible = False
        Me.ImpostaBarraNotifica(Me.Documento)
        '**************************************************************

        Me.ElencoFirmeLabel.Text = "Visti e Pareri" & If(Me.Firme.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Firme.Count.ToString & ")</span>", "")
        Me.TitoloPresenzeLabel.Text = "Elenco Presenze" & If(Me.Presenze.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Presenze.Count.ToString & ")</span>", "")
        Me.TitoloImpegniSpesaLabel.Text = "Elenco Impegni di Spesa" & If(Me.ImpegniSpesa.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.ImpegniSpesa.Count.ToString & ")</span>", "")
        Me.TitoloLiquidazioneLabel.Text = "Elenco Liquidazioni" & If(Me.Liquidazioni.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Liquidazioni.Count.ToString & ")</span>", "")
        Me.TitoloAccertamentoLabel.Text = "Elenco Accertamenti" & If(Me.Accertamenti.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Accertamenti.Count.ToString & ")</span>", "")



        CType(Me.AttiTabStrip.FindTabByText("Presenze").FindControl("TabLabel"), Label).Text = "Presenze" & If(Me.Presenze.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Presenze.Count.ToString & ")</span>", "")
        CType(Me.AttiTabStrip.FindTabByText("Classificazioni").FindControl("TabLabel"), Label).Text = "Classificazioni" & If(Me.Classificazioni.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Classificazioni.Count.ToString & ")</span>", "")
        CType(Me.AttiTabStrip.FindTabByText("Contabilità").FindControl("TabLabel"), Label).Text = "Contabilità" & If((Me.ImpegniSpesa.Count + Me.Liquidazioni.Count + Me.Accertamenti.Count) > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & (Me.ImpegniSpesa.Count + Me.Liquidazioni.Count + Me.Accertamenti.Count).ToString & ")</span>", "")


        Me.AttiTabStrip.Tabs(3).Text = "Allegati " & If(Me.Allegati.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Allegati.Count.ToString & ")</span>", "")
        Me.AttiTabStrip.Tabs(5).Text = "Visibilità" & If(Me.Visibilita.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Visibilita.Count.ToString & ")</span>", "")

        If Not Me.Fascicoli Is Nothing Then
            'luca 01/07/2020
            Me.AttiTabStrip.Tabs(6).Text = "Fascicoli" & If(Me.Fascicoli.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.Fascicoli.Count.ToString & ")</span>", "")
        End If

        'luca 01/07/2020
        '' ''CType(Me.AttiTabStrip.FindTabByText("Trasparenza").FindControl("TabLabel"), Label).Text = "Trasparenza" & If(Not Me.Trasparenza Is Nothing, "<span style='width:20px;color:#00156E'>&nbsp;(1)</span>", "")

        'luca 01/07/2020
        '' ''If Me.BandiGareContrattiPanel.Visible Then
        '' ''    Me.BandoGaraTabStrip.Tabs(1).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        '' ''    Dim allegatoPrimario As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
        '' ''    Me.DocumentoAllegatoBandoGaraRadioButton.Checked = Not allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioBandoGaraRadioButton.Checked = allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioBandoGaraRadioButton.Enabled = allegatoPrimario Is Nothing
        '' ''End If
        'luca 01/07/2020
        '' ''If Me.PubblicazioneGenericaPanel.Visible Then
        '' ''    Me.PubblicazioneGenericaTabStrip.Tabs(1).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        '' ''    Dim allegatoPrimario As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
        '' ''    Me.DocumentoAllegatoPubblicazioneGenericaRadioButton.Checked = Not allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioPubblicazioneGenericaRadioButton.Checked = allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioPubblicazioneGenericaRadioButton.Enabled = allegatoPrimario Is Nothing
        '' ''End If

        'luca 01/07/2020
        '' ''If Me.IncaricoPanel.Visible Then
        '' ''    Me.IncaricoDipendenteTabStrip.Tabs(1).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        '' ''    Dim allegatoPrimario As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
        '' ''    Me.DocumentoAllegatoIncaricoDipendenteRadioButton.Checked = Not allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioIncaricoDipendenteRadioButton.Checked = allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioIncaricoDipendenteRadioButton.Enabled = allegatoPrimario Is Nothing
        '' ''End If
        'luca 01/07/2020
        '' ''If Me.CollaborazioneConsulenzaPanel.Visible Then
        '' ''    Me.CompensoConsulenzaTabStrip.Tabs(2).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        '' ''    Dim allegatoPrimario As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
        '' ''    Me.DocumentoAllegatoCompensoConsulenzaRadioButton.Checked = Not allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioCompensoConsulenzaRadioButton.Checked = allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioCompensoConsulenzaRadioButton.Enabled = allegatoPrimario Is Nothing
        '' ''End If
        'luca 01/07/2020
        '' ''If Me.AttiConcessionePanel.Visible Then
        '' ''    Me.AttiConcessioneTabStrip.Tabs(1).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        '' ''    Dim allegatoPrimario As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
        '' ''    Me.DocumentoAllegatoAttiConcessioneRadioButton.Checked = Not allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioAttiConcessioneRadioButton.Checked = allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioAttiConcessioneRadioButton.Enabled = allegatoPrimario Is Nothing
        '' ''End If
        'luca 01/07/2020
        '' ''If Me.IncarichiAmministrativiDirigenzialiPanel.Visible Then
        '' ''    Me.IncarichiAmministrativiDirigenzialiTabStrip.Tabs(2).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        '' ''    Dim allegatoPrimario As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
        '' ''    Me.DocumentoAllegatoIncarichiAmministrativiDirigenzialiRadioButton.Checked = Not allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioIncarichiAmministrativiDirigenzialiRadioButton.Checked = allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioIncarichiAmministrativiDirigenzialiRadioButton.Enabled = allegatoPrimario Is Nothing
        '' ''End If
        'luca 01/07/2020
        '' ''If Me.BandoConcorsoPanel.Visible Then
        '' ''    Me.BandoConcorsoTabStrip.Tabs(1).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        '' ''    Dim allegatoPrimario As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
        '' ''    Me.DocumentoAllegatoBandoConcorsoRadioButton.Checked = Not allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioBandoConcorsoRadioButton.Checked = allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioBandoConcorsoRadioButton.Enabled = allegatoPrimario Is Nothing
        '' ''End If

        'luca 01/07/2020
        '' ''If Me.EnteControllatoPanel.Visible Then
        '' ''    Me.EnteControllatoTabStrip.Tabs(4).Text = "Allegati " & If(Me.AllegatiPubblicazione.Count > 0, "<span style='width:20px;color:#00156E'>&nbsp;(" & Me.AllegatiPubblicazione.Count.ToString & ")</span>", "")
        '' ''    Dim allegatoPrimario As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
        '' ''    Me.DocumentoAllegatoEnteControllatoRadioButton.Checked = Not allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioEnteControllatoRadioButton.Checked = allegatoPrimario Is Nothing
        '' ''    Me.DocumentoPrimarioEnteControllatoRadioButton.Enabled = allegatoPrimario Is Nothing


        '' ''    Me.TrattamentiEconomiciRappresentantiEnteControllatoGridView.DataSource = Me.TrattamentiEconomiciRappresentante
        '' ''    Me.TrattamentiEconomiciRappresentantiEnteControllatoGridView.DataBind()


        '' ''    Me.TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView.DataSource = Me.TrattamentiEconomiciIncaricoAmministratore
        '' ''    Me.TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView.DataBind()


        '' ''    Me.BilanciEnteControllatoGridView.DataSource = Me.BilanciEnteControllato
        '' ''    Me.BilanciEnteControllatoGridView.DataBind(

        '' ''End If




    End Sub

#End Region

#Region "GESTIONE FINESTRA POPUP"

    Protected Sub ChiudiButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiudiButton.Click
        'NON E' NECESSARIO 
        'ParsecUtility.Utility.ClosePopup(False)
        ParsecUtility.Utility.DoWindowClose(False)
    End Sub


    Private Sub GetParametri()

        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            Dim documenti As New ParsecAtt.DocumentoRepository

            If parametriPagina.ContainsKey("IdDocumentoIter") Then
                Dim idDocumento As Integer = parametriPagina("IdDocumentoIter")
                Me.Documento = documenti.GetById(idDocumento)
            End If

            If Not Me.Documento Is Nothing Then
                'Se sto aprendo la finestra come popup
                If Not Page.Request("Mode") Is Nothing Then


                    Me.TipologiaProceduraCorrente = CType(Me.Page.Request("Procedura"), ParsecAtt.TipoProcedura)

                    If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.ModificaParere Then
                        If parametriPagina.ContainsKey("IdFirmaModificabile") Then
                            Me.IdFirmaModificabile = parametriPagina("IdFirmaModificabile")
                        End If
                    End If

                    Me.DocumentiPanel.Style.Add("display", "none")
                    Me.DocumentiGridView.Visible = False
                    Me.ChiudiButton.Visible = True

                    Select Case Me.Page.Request("Mode")

                        Case "Update"

                            'Disabilito tutti i pulsanti della toolbar
                            Me.RadToolBar.FindItemByText("Nuovo").Enabled = False
                            Me.RadToolBar.FindItemByText("Elimina").Enabled = False

                            'Me.RadToolBar.FindItemByText("Elimina").Attributes.Remove("onclick")



                            Me.RadToolBar.FindItemByText("Trova").Enabled = False
                            Me.RadToolBar.FindItemByText("Annulla").Enabled = False
                            Me.RadToolBar.FindItemByText("Stampa").Enabled = False
                            Me.RadToolBar.FindItemByText("Ricerca Avanzata").Enabled = False
                            Me.RadToolBar.FindItemByText("Home").Enabled = False
                            Me.RadToolBar.FindItemByText("Salva").Enabled = True

                        Case "Delete"

                            Me.RadToolBar.FindItemByText("Elimina").Enabled = True
                            'Me.RadToolBar.Items.FindItemByText("Elimina").Attributes.Add("onclick", "return Confirm();")

                            'Disabilito tutti i pulsanti della toolbar
                            Me.RadToolBar.FindItemByText("Nuovo").Enabled = False
                            Me.RadToolBar.FindItemByText("Salva").Enabled = False
                            Me.RadToolBar.FindItemByText("Trova").Enabled = False
                            Me.RadToolBar.FindItemByText("Annulla").Enabled = False
                            Me.RadToolBar.FindItemByText("Stampa").Enabled = False
                            Me.RadToolBar.FindItemByText("Ricerca Avanzata").Enabled = False
                            Me.RadToolBar.FindItemByText("Home").Enabled = False

                        Case "View"
                            Me.RadToolBar.Visible = False
                    End Select
                End If
            End If

        End If



    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    Private Sub StampaRegistroGeneraleDelibere()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "4")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub

    Private Sub StampaRegistroGeneraleDetermine()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "2")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub

    Private Sub StampaRegistroGeneraleOrdinanze()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "12")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub

    Private Sub StampaRegistroGeneraleDecreti()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "13")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroGenerale))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub

    Private Sub StampaRegistroSettoreDetermine()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "2")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.RegistroSettore))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub

    Private Sub StampaElencoDetermineImpegnoSpesa()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "2")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub

    Private Sub StampaElencoDeterminePubblicazione()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "2")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub

    'todo
    Private Sub StampaElencoDetermineLiquidazione()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "2")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.Liquidazioni))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub


    Private Sub StampaElencoDelibereImpegnoSpesa()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "4")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.ImpegniSpesa))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub

    Private Sub StampaElencoDeliberePubblicazione()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/StampaRegistroGeneralePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("Tipo", "4")
        queryString.Add("TipoStampa", CInt(ParsecAtt.TipologiaStampaAttoAmministrativo.Pubblicazioni))
        queryString.Add("Mode", "1")
        ParsecUtility.Utility.ShowPopup(pageUrl, 650, 480, queryString, False)
    End Sub



    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                ' Me.Print()
            Case "RegistroGeneraleDelibere"
                Me.StampaRegistroGeneraleDelibere()

            Case "RegistroGeneraleDetermine"
                Me.StampaRegistroGeneraleDetermine()

            Case "RegistroGeneraleDecreti"
                Me.StampaRegistroGeneraleDecreti()

            Case "RegistroGeneraleOrdinanze"
                Me.StampaRegistroGeneraleOrdinanze()

            Case "RegistroSettoreDetermine"
                Me.StampaRegistroSettoreDetermine()

            Case "ElencoDetermineImpegnoSpesa"
                Me.StampaElencoDetermineImpegnoSpesa()

            Case "ElencoDeterminePubblicazione"
                Me.StampaElencoDeterminePubblicazione()

            Case "ElencoDetermineLiquidazione"
                Me.StampaElencoDetermineLiquidazione()

            Case "ElencoDelibereImpegnoSpesa"
                Me.StampaElencoDelibereImpegnoSpesa()
            Case "ElencoDeliberePubblicazione"

                Me.StampaElencoDeliberePubblicazione()
            Case "Salva"

                'DISABILITO IL PULSANTE SALVA DELLA TOOLBAR
                Me.RadToolBar.FindItemByText("Salva").Enabled = False

                'Dim message As String = "Operazione conclusa con successo!"
                Dim message As String = String.Empty
                Try

                    Select Case Me.TipologiaProceduraApertura
                        Case ParsecAtt.TipoProcedura.ModificaAmministrativa
                            If Me.Documento Is Nothing Then
                                ParsecUtility.Utility.MessageBox("E' necessario selezionare un documento!", False)
                                Exit Sub
                            End If



                            Me.Save()

                        Case ParsecAtt.TipoProcedura.Nuovo, ParsecAtt.TipoProcedura.Modifica, ParsecAtt.TipoProcedura.ModificaParere
                            Me.Save()
                            If Not Page.Request("Mode") Is Nothing Then

                                ParsecUtility.SessionManager.Documento = Me.Documento


                            End If

                        Case ParsecAtt.TipoProcedura.Numerazione
                            If Me.Documento Is Nothing Then
                                ParsecUtility.Utility.MessageBox("E' necessario selezionare un documento!", False)
                                Exit Sub
                            End If

                            Dim documenti As New ParsecAtt.DocumentoRepository
                            Dim propostaNumerata = documenti.Where(Function(c) c.IdPadre = Me.Documento.Id).Any
                            documenti.Dispose()

                            If Not propostaNumerata Then
                                Me.Numera()
                            Else
                                ParsecUtility.Utility.MessageBox("La proposta è stata già numerata!", False)
                                Exit Sub
                            End If


                            If Not Page.Request("Mode") Is Nothing Then
                                Me.RadToolBar.FindItemByText("Salva").Enabled = False
                                ParsecUtility.SessionManager.Documento = Me.Documento
                            End If


                        Case ParsecAtt.TipoProcedura.Pubblicazione, ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.AggiungiDatiContabili
                            If Me.Documento Is Nothing Then
                                ParsecUtility.Utility.MessageBox("E' necessario selezionare un documento!", False)
                                Exit Sub
                            End If
                            Me.Save()

                            'If Not Me.Rigenera Then
                            '    ParsecUtility.Utility.MessageBox("Operazione conclusa con successo!", False)
                            'End If


                            If Not Page.Request("Mode") Is Nothing Then
                                If Me.OperazioneAvvenuta Then
                                    ParsecUtility.SessionManager.Documento = Me.Documento
                                End If
                            End If
                            Me.OperazioneAvvenuta = False


                        Case ParsecAtt.TipoProcedura.Classificazione
                            If Me.Documento Is Nothing Then
                                ParsecUtility.Utility.MessageBox("E' necessario selezionare un documento!", False)
                                Exit Sub
                            End If
                            Me.Classifica()
                            Me.SalvaContenuto()


                            If Not Page.Request("Mode") Is Nothing Then
                                ParsecUtility.SessionManager.Documento = Me.Documento
                            End If


                    End Select


                    Me.AggiornaGriglia()


                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                If Not String.IsNullOrEmpty(message) Then


                    ParsecUtility.Utility.MessageBox(message, False)


                    'ABILITO IL PULSANTE SALVA DELLA TOOLBAR
                    Me.RadToolBar.FindItemByText("Salva").Enabled = True


                End If

            Case "Nuovo", "Annulla"

                Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Nuovo
                Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Nuovo

                If Not Request.QueryString("Procedura") Is Nothing Then
                    Me.TipologiaProceduraApertura = CInt(Request.QueryString("Procedura"))
                End If

                Me.TipologiaDocumento = CType(CInt(Me.TipologieDocumentoComboBox.SelectedValue), ParsecAtt.TipoDocumento)

                Me.ResettaVista()
                Me.DisabilitaUI()
                Select Case Me.TipologiaProceduraApertura
                    Case ParsecAtt.TipoProcedura.Nuovo, ParsecAtt.TipoProcedura.Modifica
                        Me.AbilitaUI(ParsecAtt.TipoProcedura.Nuovo)
                    Case ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.Numerazione, ParsecAtt.TipoProcedura.Pubblicazione, ParsecAtt.TipoProcedura.Classificazione, ParsecAtt.TipoProcedura.ModificaAmministrativa, ParsecAtt.TipoProcedura.AggiungiDatiContabili
                        Me.AbilitaUI(ParsecAtt.TipoProcedura.Ricerca)

                End Select
                Me.ImpostaDescrizioneTipologiaDocumento(Me.TipologiaDocumento, Me.TipologiaProceduraApertura)
                Me.AggiornaGriglia()


            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Documento Is Nothing Then
                        Try
                            Me.Delete()

                            If Not Page.Request("Mode") Is Nothing Then
                                ParsecUtility.SessionManager.Documento = Me.Documento
                                Me.infoOperazioneHidden.Value = "Cancellazione conclusa con successo!"

                                Me.ResettaVista()
                                Me.DisabilitaUI()

                                'GESTISCO LA CHIUSURA DALL'EVENTO OnEndRequest
                                'ParsecUtility.Utility.DoWindowClose(False)
                                'Non funziona
                                'ParsecUtility.Utility.ClosePopup(False)
                                Exit Sub
                            End If

                            Me.infoOperazioneHidden.Value = "Cancellazione conclusa con successo!"
                            Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Nuovo
                            Me.ResettaVista()
                            Me.DisabilitaUI()
                            Me.AggiornaGriglia()

                        Catch ex As Exception
                            ParsecUtility.Utility.MessageBox(ex.Message, False)
                        End Try

                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un documento!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
            Case "RicercaAvanzata"
                Me.AdvancedSearch()

        End Select
    End Sub


    Private Sub Delete()
        Dim success As Boolean = True
        Try
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            documenti.Delete(Me.Documento, utenteCollegato)
            documenti.Dispose()
        Catch ex As Exception
            success = False
            Throw New ApplicationException(ex.Message)
        End Try
        If success Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            istanze.AnnullaIter(Me.Documento.Id, ParsecAdmin.TipoModulo.ATT)
            istanze.Dispose()
        End If
    End Sub

    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            'e.Item.Attributes.Add("onclick", "return Confirm();")
        End If

        If e.Item.Text = "Stampa" Then
            ' e.Item.Attributes.Add("onclick", "ShowPanel();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    Protected Sub DocumentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles DocumentiGridView.NeedDataSource

        '**********************************************************************************************
        'SE LA GRIGLIA NON E' VISIBILE NON CARICO I DOCUMENTI
        '**********************************************************************************************
        'If Not Request.QueryString("Mode") Is Nothing Then
        If Not DocumentiGridView.Visible Then
            Exit Sub
        End If
        'End If
        '**********************************************************************************************

        If Me.Documenti Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Select Case Me.TipologiaProceduraApertura

                Case ParsecAtt.TipoProcedura.Nuovo
                    Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.IdUtente = utenteCollegato.Id, .ApplicaAbilitazione = True, .IdTipologiaDocumento = CInt(Me.TipologiaDocumento), .UltimeCinque = True})

                Case ParsecAtt.TipoProcedura.Modifica
                    Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.EscludiDocumentiPubblicati = True, .EscludiDocumentiNumerati = True, .IdUtente = utenteCollegato.Id, .ApplicaAbilitazione = True, .IdTipologiaDocumento = CInt(Me.TipologiaDocumento), .UltimeCinque = True})

                Case ParsecAtt.TipoProcedura.Classificazione
                    Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.EscludiDocumentiNumerati = True, .IdUtente = utenteCollegato.Id, .IdTipologiaDocumento = CInt(Me.TipologiaDocumento), .ApplicaAbilitazione = True, .UltimeCinque = True})

                Case ParsecAtt.TipoProcedura.Pubblicazione

                    '**********************************************************************************************
                    'ESCLUDO TUTTI I DOCUMENTI IL CUI MODELLO NON PREVEDE LA PUBBLICAZIONE
                    '**********************************************************************************************
                    Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.EscludiSenzaPubblicazione = True, .EscludiDocumentiNumerati = True, .IdUtente = utenteCollegato.Id, .IdTipologiaDocumento = CInt(Me.TipologiaDocumento), .ApplicaAbilitazione = True, .UltimeCinque = True})

                Case ParsecAtt.TipoProcedura.Numerazione
                    Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.EscludiDocumentiNumerati = True, .IdUtente = utenteCollegato.Id, .IdTipologiaDocumento = CInt(Me.TipologiaDocumentoApertura), .ApplicaAbilitazione = True, .UltimeCinque = True})

                Case ParsecAtt.TipoProcedura.CambioModello
                    Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.EscludiDocumentiPubblicati = True, .EscludiDocumentiNumerati = True, .IdUtente = utenteCollegato.Id, .IdTipologiaDocumento = CInt(Me.TipologiaDocumento), .ApplicaAbilitazione = True, .UltimeCinque = True})

                Case ParsecAtt.TipoProcedura.AggiungiDatiContabili
                    Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.DocumenticonDatiContabili = True, .EscludiDocumentiPubblicati = True, .EscludiDocumentiNumerati = True, .IdUtente = utenteCollegato.Id, .IdTipologiaDocumento = CInt(Me.TipologiaDocumento), .ApplicaAbilitazione = True, .UltimeCinque = True})

                Case ParsecAtt.TipoProcedura.ModificaAmministrativa
                    Me.Documenti = documenti.GetView(New ParsecAtt.FiltroDocumento With {.IdTipologiaDocumento = CInt(Me.TipologiaDocumento), .EscludiModificati = True, .EscludiDocumentiNumerati = True, .UltimeCinque = True})

            End Select

            documenti.Dispose()
        End If
        Me.DocumentiGridView.DataSource = Me.Documenti
    End Sub

    Protected Sub DocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles DocumentiGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.SelezionaDocumento(e)
            Case "Copy"

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
                Dim abilitatoNuovoDocumento As Boolean = Me.AbilitatoFunzioneNuovoDocumento(utenteCollegato)

                'If Me.RadToolBar.FindItemByText("Salva").Enabled = False Then
                If Not abilitatoNuovoDocumento Then
                    If String.IsNullOrEmpty(Me.RadToolBar.FindItemByText("Salva").ToolTip) Then
                        ParsecUtility.Utility.MessageBox("Utente non abilitato all'operazione selezionata!", False)
                    Else
                        ParsecUtility.Utility.MessageBox(Me.RadToolBar.FindItemByText("Salva").ToolTip.Replace(".", "!"), False)
                    End If

                    Exit Sub
                End If

                Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Nuovo
                If Not Request.QueryString("Procedura") Is Nothing Then
                    Me.TipologiaProceduraApertura = CInt(Request.QueryString("Procedura"))
                End If
                Me.TipologieDocumentoComboBox.Items.FindItemByValue(CInt(Me.TipologiaDocumentoApertura)).Selected = True
                Me.TipologiaDocumento = CType(CInt(Me.TipologieDocumentoComboBox.SelectedValue), ParsecAtt.TipoDocumento)
                Me.ResettaVista()
                Me.DisabilitaUI()
                SelezionaDocumento(e)
                Me.Documento = Nothing
            Case "Unlock"
                Me.SbloccaDocumento(e.Item)
        End Select
    End Sub

    Private Sub SelezionaDocumento(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim id As Integer = e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id")
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetById(id)
        documenti.Dispose()
        Me.SelezionaDocumento(documento, e.CommandName = "Select")
    End Sub

    Private Sub SelezionaDocumento(documento As ParsecAtt.Documento, ByVal selezione As Boolean)
        Me.ResettaVista()
        Select Case Me.TipologiaProceduraApertura
            Case ParsecAtt.TipoProcedura.Nuovo, ParsecAtt.TipoProcedura.Modifica
                Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Nuovo
                If selezione Then
                    Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Modifica
                End If
            Case ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.Numerazione, ParsecAtt.TipoProcedura.Pubblicazione, ParsecAtt.TipoProcedura.Classificazione, ParsecAtt.TipoProcedura.ModificaAmministrativa, ParsecAtt.TipoProcedura.AggiungiDatiContabili
                Me.TipologiaProceduraCorrente = Me.TipologiaProceduraApertura
        End Select
        Me.DisabilitaUI()
        Me.AggiornaVista(documento, selezione)
        Me.AbilitaUI(Me.TipologiaProceduraCorrente)
    End Sub

    Private Sub SbloccaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = documenti.GetById(id)
        Dim documentiBloccati As New ParsecAtt.LockDocumentoRepository
        Dim documentoBloccato As ParsecAtt.LockDocumento = documentiBloccati.GetQuery.Where(Function(c) c.CodiceDocumento = documento.Codice).FirstOrDefault
        If Not documentoBloccato Is Nothing Then
            documentiBloccati.Delete(documentoBloccato.Id)
        End If
        documenti.Dispose()
        documentiBloccati.Dispose()
        Me.AggiornaGriglia()

    End Sub

    Protected Sub DocumentiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub

    Protected Sub DocumentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles DocumentiGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim documento As ParsecAtt.Documento = CType(e.Item.DataItem, ParsecAtt.Documento)

            If TypeOf dataItem("Stato").Controls(0) Is ImageButton Then

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                Dim btnDeleteLock = CType(dataItem("Unlock").Controls(0), ImageButton)
                btnDeleteLock.ImageUrl = "~/images/vuoto.png"
                btnDeleteLock.Attributes.Add("onclick", "return false")
                btnDeleteLock.ToolTip = ""



                If utenteCollegato.SuperUser Then
                    Dim documentiBloccati As New ParsecAtt.LockDocumentoRepository
                    Dim documentoBloccato As ParsecAtt.LockDocumento = documentiBloccati.GetQuery.Where(Function(c) c.CodiceDocumento = documento.Codice).FirstOrDefault
                    If Not documentoBloccato Is Nothing Then
                        If documentoBloccato.IdUtente <> utenteCollegato.Id Then
                            btnDeleteLock.Style.Add("cursor", "default")
                            btnDeleteLock.ToolTip = "Sblocca documento"
                            btnDeleteLock.ImageUrl = "~/images/LockDelete16.png"
                            btnDeleteLock.Attributes.Remove("onclick")
                        End If

                    End If

                    documentiBloccati.Dispose()
                End If

                btn = CType(dataItem("Stato").Controls(0), ImageButton)
                btn.Attributes.Add("onclick", "return false")
                btn.Style.Add("cursor", "default")
                btn.ImageUrl = "~/images/Unlock_16.png"
                btn.ToolTip = "Modificabile"

                Dim bloccato As Boolean = False

                If Me.TipologiaProceduraApertura <> ParsecAtt.TipoProcedura.ModificaAmministrativa Then
                    If documento.IdFiglio.HasValue Then
                        btn.ImageUrl = "~/images/Lock_16.png"
                        btn.ToolTip = "Sola lettura (Numerata)"
                        bloccato = True
                    End If

                    Select Case Me.TipologiaProceduraApertura
                        Case ParsecAtt.TipoProcedura.Nuovo, ParsecAtt.TipoProcedura.Modifica, ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.Classificazione, ParsecAtt.TipoProcedura.AggiungiDatiContabili
                            If documento.DataAffissioneDa.HasValue Then
                                btn.ImageUrl = "~/images/Lock_16.png"
                                btn.ToolTip = "Sola lettura (Pubblicata)"
                                bloccato = True
                            End If
                    End Select

                    Dim istanze As New ParsecWKF.IstanzaRepository
                    Dim istanza = istanze.GetQuery.Where(Function(c) c.IdDocumento = documento.Id And c.IdModulo = ParsecAdmin.TipoModulo.ATT).FirstOrDefault
                    If Not istanza Is Nothing Then
                        btn.ImageUrl = "~/images/Lock_16.png"
                        btn.ToolTip = "Sola lettura (In Iter)"
                        bloccato = True
                    End If
                    istanze.Dispose()

                    Dim abilitato As Boolean = True

                    Select Case Me.TipologiaProceduraApertura
                        Case ParsecAtt.TipoProcedura.Modifica, ParsecAtt.TipoProcedura.Nuovo
                            abilitato = Me.AbilitatoFunzioneModifica(documento, utenteCollegato)
                        Case ParsecAtt.TipoProcedura.Pubblicazione
                            abilitato = Me.AbilitatoFunzioneRipubblicazione(documento, utenteCollegato)
                    End Select

                    If Not abilitato Then
                        If Not bloccato Then
                            btn.ImageUrl = "~/images/Lock_16.png"
                            btn.ToolTip = "Sola lettura"
                        End If
                    Else
                        If Not bloccato Then
                            btn.ImageUrl = "~/images/Unlock_16.png"
                            btn.ToolTip = "Modificabile"
                        End If
                    End If
                End If

            End If
        End If


    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub VisualizzaStoricoDocumentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaStoricoDocumentoImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/StoricoAttiAmministrativiPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("r", Me.Documento.Codice)
        ParsecUtility.Utility.ShowPopup(pageUrl, 950, 660, queryString, False)
    End Sub

    Protected Sub ModelliComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ModelliComboBox.SelectedIndexChanged

        Dim idModello As Integer = CInt(e.Value)

        Select Case Me.TipologiaProceduraApertura

            Case ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.ModificaAmministrativa

                If Me.ModelliComboBox.SelectedIndex <> 0 Then
                    If Not Me.Documento Is Nothing Then

                        Dim modelli As New ParsecAtt.ModelliRepository
                        Dim modello As ParsecAtt.Modello = modelli.GetQuery.Where(Function(c) c.Id = idModello).FirstOrDefault
                        modelli.Dispose()
                        If Not modello.Liquidazione Then
                            Me.Liquidazioni = New List(Of ParsecAtt.Liquidazione)
                        End If
                        If Not modello.ImpegnoSpesa Then
                            Me.ImpegniSpesa = New List(Of ParsecAtt.ImpegnoSpesa)
                        End If
                        If Not modello.Accertamento Then
                            Me.Accertamenti = New List(Of ParsecAtt.Accertamento)
                        End If
                        Dim firme As New ParsecAtt.FirmeRepository


                        If idModello <> Me.Documento.IdModello Then

                            Dim nuoveFirme = firme.GetViewModello(New ParsecAtt.FiltroFirma With {.IdModello = idModello})
                            'Dim vecchieFirme = firme.GetViewModello(New ParsecAtt.FiltroFirma With {.IdModello = Me.Documento.IdModello})
                            Dim vecchieFirme = firme.GetViewDocumento(New ParsecAtt.FiltroFirma With {.IdDocumento = Me.Documento.Id})
                            Me.Firme = vecchieFirme

                            'Eseguo il merge delle firme.
                            For Each firma In nuoveFirme
                                Dim idFirma As Integer = firma.Id
                                If Me.Firme.Where(Function(c) c.Id = idFirma).FirstOrDefault Is Nothing Then
                                    Me.Firme.Add(firma)
                                End If
                            Next

                            'Me.Firme = nuoveFirme.Union(vecchieFirme).GroupBy(Function(c) c.Id).Select(Function(c) c.First).ToList

                        Else
                            Me.Firme = firme.GetViewDocumento(New ParsecAtt.FiltroFirma With {.IdDocumento = Me.Documento.Id})

                        End If
                        firme.Dispose()

                    End If
                End If
            Case ParsecAtt.TipoProcedura.Nuovo

                If Not Me.DocumentoCopia Is Nothing Then

                    Me.TipologiaModello = CInt(e.Value)
                    Me.SedutaTextBox.Text = String.Empty
                    Me.IdSedutaTextBox.Text = String.Empty
                    Me.Presenze = New List(Of ParsecAtt.DocumentoPresenza)
                    'Me.ResettaVistaPannelliTrasparenza()

                    Dim modelli As New ParsecAtt.ModelliRepository
                    Dim modello As ParsecAtt.Modello = modelli.GetQuery.Where(Function(c) c.Id = idModello).FirstOrDefault
                    modelli.Dispose()


                    Me.Liquidazioni = New List(Of ParsecAtt.Liquidazione)
                    Me.Accertamenti = New List(Of ParsecAtt.Accertamento)
                    Me.ImpegniSpesa = New List(Of ParsecAtt.ImpegnoSpesa)

                    If modello.Liquidazione Then
                        Me.Liquidazioni = Me.DocumentoCopia.Liquidazioni
                    End If
                    If modello.ImpegnoSpesa Then
                        Me.ImpegniSpesa = Me.DocumentoCopia.ImpegniSpesa
                    End If
                    If modello.Accertamento Then
                        Me.Accertamenti = Me.DocumentoCopia.Accertamenti
                    End If

                Else
                    'Se ho cambiato modello
                    If Me.TipologiaModello <> idModello Then


                        Me.TipologiaModello = CInt(e.Value)
                        Me.SedutaTextBox.Text = String.Empty
                        Me.IdSedutaTextBox.Text = String.Empty
                        Me.Presenze = New List(Of ParsecAtt.DocumentoPresenza)

                        Me.Liquidazioni = New List(Of ParsecAtt.Liquidazione)
                        Me.Accertamenti = New List(Of ParsecAtt.Accertamento)
                        Me.ImpegniSpesa = New List(Of ParsecAtt.ImpegnoSpesa)

                        'Me.ResettaVistaPannelliTrasparenza()

                    End If
                End If



                'Carico le firme associate al modello di documento selezionato.
                Dim firme As New ParsecAtt.FirmeRepository
                Me.Firme = firme.GetViewModello(New ParsecAtt.FiltroFirma With {.IdModello = Me.TipologiaModello})
                firme.Dispose()

                'Aggiorno le firme.
                If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) OrElse Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
                    Me.AggiornaFirme()
                End If



        End Select
    End Sub

    Protected Sub TipologieDocumentoComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles TipologieDocumentoComboBox.SelectedIndexChanged
        Me.TipologiaDocumento = CType(CInt(e.Value), ParsecAtt.TipoDocumento)
        Me.TipologiaDocumentoApertura = Me.TipologiaDocumento
        Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Nuovo
        Me.ImpostaDescrizioneTipologiaDocumento(Me.TipologiaDocumento, Me.TipologiaProceduraApertura)
        Me.ResettaVista()
        'Me.ResettaVistaPannelliTrasparenza()
        Me.CaricaModelli()
        Me.AbilitaUI(Me.TipologiaProceduraApertura)
    End Sub

    Private Sub SaveDocumentContent()
        If Not String.IsNullOrEmpty(Me.documentContentHidden.Value) Then
            If Me.documentContentHidden.Value.EndsWith("OK") Then
                Me.documentContentHidden.Value = Me.documentContentHidden.Value.Remove(Me.documentContentHidden.Value.Length - 2)
            End If
            Dim contenuto As New ParsecAdmin.ContenutoDocumentiRepository
            contenuto.Save(Me.documentContentHidden.Value, ParsecAdmin.TipoModulo.ATT, Me.Documento.Id, Me.Documento.Codice)
            contenuto.Dispose()
            Me.documentContentHidden.Value = String.Empty
        End If

    End Sub

    Private Sub SalvaContenuto()
        ' Dim attoDefinitivo As Boolean = Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza
        If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Pubblicazione Then
            Try
                SaveDocumentContent()
            Catch ex As Exception
                Stop
            End Try
        End If


        Me.DataSource = Me.Documento.DataSource
        Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), Me.Documento.DataDocumento.Value.Year, Me.Documento.Nomefile)

        Dim message As String = String.Empty
        Dim successo As Boolean = True

        Dim documenti As New ParsecAtt.DocumentoRepository

        Dim documento As ParsecAtt.Documento
        Select Case Me.TipologiaProceduraApertura

            Case ParsecAtt.TipoProcedura.Nuovo, ParsecAtt.TipoProcedura.Modifica, ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.Pubblicazione, ParsecAtt.TipoProcedura.Classificazione, ParsecAtt.TipoProcedura.AggiungiDatiContabili, ParsecAtt.TipoProcedura.ModificaAmministrativa, ParsecAtt.TipoProcedura.ModificaParere
                'Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Modifica
                'Me.AggiornaVista(documento)


                If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Pubblicazione OrElse Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.ModificaAmministrativa Then
                    'luca 02/07/2020
                    '' ''Dim pubblicato As Boolean = False
                    '' ''Try
                    '' ''    Select Case Me.Documento.TipologiaDocumento
                    '' ''        Case ParsecAtt.TipoDocumento.Delibera, ParsecAtt.TipoDocumento.Determina, ParsecAtt.TipoDocumento.Ordinanza, ParsecAtt.TipoDocumento.Decreto
                    '' ''            If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Pubblicazione Then
                    '' ''                Me.SalvaAlbo(Me.Documento.Id)
                    '' ''                pubblicato = True
                    '' ''            Else
                    '' ''                'PUBBLICO SOLO SE VIENE SPECIFICATA LA DATA DI AFFISSIONE
                    '' ''                If Me.DataAffissioneTextBox.SelectedDate.HasValue Then
                    '' ''                    Me.SalvaAlbo(Me.Documento.Id)
                    '' ''                    pubblicato = True
                    '' ''                End If

                    '' ''            End If

                    '' ''    End Select

                    '' ''Catch ex As Exception
                    '' ''    successo = False
                    '' ''    message = ex.Message
                    '' ''End Try

                    If successo Then
                        If Not String.IsNullOrEmpty(Me.MessaggioAvviso) Then
                            ParsecUtility.Utility.MessageBox(Me.MessaggioAvviso, False)
                            Me.MessaggioAvviso = String.Empty
                        End If
                    Else
                        If Not String.IsNullOrEmpty(Me.MessaggioAvviso) Then
                            message &= vbCrLf & Me.MessaggioAvviso
                            Me.MessaggioAvviso = String.Empty
                        End If
                        ParsecUtility.Utility.MessageBox(message, False)
                    End If

                    'successo = True

                    '**************************************************************
                    'DEPRECATA
                    '**************************************************************
                    'If pubblicato Then
                    '    If TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Then
                    '        If Me.Documento.Modello.Liquidazione Then
                    '            Try
                    '                Me.PubblicaLiquidazione()
                    '            Catch ex As Exception
                    '                successo = False
                    '                ParsecUtility.Utility.MessageBox(ex.Message & "!", False)
                    '            End Try
                    '        End If
                    '    End If
                    'End If

                    '**************************************************************

                End If

                If successo Then
                    Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
                End If

                documento = documenti.GetById(Me.Documento.Id)

                If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Nuovo Then
                    Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Modifica
                End If

                Me.SelezionaDocumento(documento, True)



            Case ParsecAtt.TipoProcedura.Numerazione
                documento = documenti.GetById(Me.Documento.Id)

                If Not Request.QueryString("Mode") Is Nothing Then
                    Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Modifica
                    Me.AggiornaVista(documento, True)
                Else

                    Me.SelezionaDocumento(documento, True)
                    Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Ricerca
                    ' Me.ResettaVista()
                    Me.ImpostaDescrizioneTipologiaDocumento(documento.TipologiaDocumento, Me.TipologiaProceduraCorrente)
                End If


                ViewState("SalvaAbilitato") = False


                Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
        End Select


        documenti.Dispose()



        If Not IO.File.Exists(localPath) Then
            Me.verificaRigenerazioneDocumento.Attributes.Add("onclick", "return ConfirmDeleteSalvataggio();")
            Me.verificaDocumentoHidden.Value = "SI"
        End If

        'IMPOSTO L'ABILITAZIONE DEL PULSANTE SALVA DELLA TOOLBAR
        Me.RadToolBar.FindItemByText("Salva").Enabled = ViewState("SalvaAbilitato")

        If successo Then
            'SE STO APRENDO LA FINESTRA COME POPUP 
            If Not Page.Request("Mode") Is Nothing Then
                'GESTISCO LA CHIUSURA DALL'EVENTO OnEndRequest
                'ParsecUtility.Utility.DoWindowClose(False)

            End If
            Me.OperazioneAvvenuta = True

            '????????????????
            ParsecUtility.SessionManager.Documento = Me.Documento
        Else
            Me.OperazioneAvvenuta = False
        End If
    End Sub

    Protected Sub salvaContenutoButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles salvaContenutoButton.Click
        Me.SalvaContenuto()
    End Sub

    Protected Sub verificaRigenerazioneDocumento_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles verificaRigenerazioneDocumento.Click
        Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), Me.Documento.DataDocumento.Value.Year, Me.Documento.Nomefile)
        If Not IO.File.Exists(localPath) Then
            rigeneraDocumento_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub rigeneraDocumento_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles rigeneraDocumento.Click
        ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(Me.DataSource, verificaRigenerazioneDocumento.ClientID, Nothing, False, False)
    End Sub

    Private Function GetAnnoEsercizio(documento As ParsecAtt.Documento) As Integer
        Dim annoEsercizio As Integer = Now.Year
        Try
            Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("\d{4}")
            annoEsercizio = CInt(rgx.Match(documento.Nomefile).Value)
        Catch ex As Exception

        End Try

        Return annoEsercizio
    End Function

    Protected Sub VisualizzaDocumentoCollegatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoCollegatoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim attoCollegato As ParsecAtt.Documento = Nothing
            ' Se si visualizza il dettaglio di un proposta, il documento collegato è l'atto finale adottato
            ' altrimenti il documento collegato è la proposta iniziale

            If Me.Documento.Modello.Proposta Then
                attoCollegato = Me.Documento.DocumentoGenerato  'atto finale
                ' attoCollegato = documenti.GetQuery.Where(Function(c) c.IdPadre = Me.Documento.Id).OrderByDescending(Function(c) c.NumVersione).FirstOrDefault
            Else
                attoCollegato = Me.Documento.DocumentoCollegato  'proposta
                'attoCollegato = documenti.GetQuery.Where(Function(c) c.Id = Me.Documento.IdPadre).OrderByDescending(Function(c) c.NumVersione).FirstOrDefault
            End If


            If Not attoCollegato Is Nothing Then
                Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), Me.GetAnnoEsercizio(Me.Documento), attoCollegato.Nomefile)
                If IO.File.Exists(localPath) Then
                    Me.VisualizzaDocumento(attoCollegato.Nomefile, Me.GetAnnoEsercizio(attoCollegato), False)
                Else
                    ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
                End If

            End If
            documenti.Dispose()
        End If

    End Sub

    Protected Sub VisualizzaCopiaDocumentoCollegatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaCopiaDocumentoCollegatoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim attoCollegato As ParsecAtt.Documento = Nothing
            If Me.Documento.Modello.Proposta Then
                attoCollegato = Me.Documento.DocumentoGenerato  'atto finale
                'attoCollegato = documenti.GetQuery.Where(Function(c) c.IdPadre = Me.Documento.Id).OrderByDescending(Function(c) c.NumVersione).FirstOrDefault
            Else
                attoCollegato = Me.Documento.DocumentoCollegato  'proposta
                'attoCollegato = documenti.GetQuery.Where(Function(c) c.Id = Me.Documento.IdPadre).OrderByDescending(Function(c) c.NumVersione).FirstOrDefault
            End If

            If Not attoCollegato Is Nothing Then
                Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), Me.GetAnnoEsercizio(Me.Documento), attoCollegato.Nomefile)
                If IO.File.Exists(localPath) Then
                    Me.VisualizzaCopiaDocumento(attoCollegato.Nomefile, Me.GetAnnoEsercizio(attoCollegato), False)
                Else
                    ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
                End If

            End If
            documenti.Dispose()
        End If
    End Sub

    Protected Sub VisualizzaCopiaDocumentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaCopiaDocumentoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim nomefile As String = Me.Documento.Nomefile

            Dim annoEsercizio As Integer = Now.Year
            If Me.Documento.Modello.Proposta Then
                annoEsercizio = Me.Documento.DataProposta.Value.Year
            Else
                annoEsercizio = Me.Documento.Data.Value.Year
            End If

            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefile)
            If IO.File.Exists(localPath) Then
                Me.VisualizzaCopiaDocumento(nomefile, annoEsercizio, False)
            Else
                ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
            End If
        End If
    End Sub

    Protected Sub VisualizzaDocumentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoImageButton.Click
        If Not Me.Documento Is Nothing Then
            Dim nomefile As String = Me.Documento.Nomefile


            Dim annoEsercizio As Integer = Now.Year
            If Me.Documento.Modello.Proposta Then
                annoEsercizio = Me.Documento.DataProposta.Value.Year
            Else
                annoEsercizio = Me.Documento.Data.Value.Year
            End If

            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefile)

            If IO.File.Exists(localPath) Then
                Me.VisualizzaDocumento(nomefile, annoEsercizio, False)
            Else
                'todo RIGENERARE IL DOCUMENTO
                ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
            End If
        End If
    End Sub

    Protected Sub VisualizzaDocumentoFirmatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaDocumentoFirmatoImageButton.Click
        If Not Me.Documento Is Nothing Then
            'Dim nomefileFirmato As String = IO.Path.GetFileNameWithoutExtension(Me.Documento.Nomefile) & ".pdf.p7m"


            Dim annoEsercizio As Integer = Now.Year
            'If Me.Documento.Modello.Proposta Then
            '    annoEsercizio = Me.Documento.DataProposta.Value.Year
            'Else
            '    annoEsercizio = Me.Documento.Data.Value.Year
            'End If

            'Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefileFirmato)
            Dim localPath As String = Me.GetNomeFileFirmato()

            If IO.File.Exists(localPath) Then
                ' Me.VisualizzaDocumento(nomefileFirmato, annoEsercizio, False)
                Me.VisualizzaDocumentoP7M(localPath, annoEsercizio)
            End If



        End If
    End Sub

    Protected Sub AggiornaAttiImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaAttiImageButton.Click
        Me.ApplicaFiltro()
    End Sub

    Protected Sub SalvaNoteButton_Click(sender As Object, e As System.EventArgs) Handles SalvaNoteButton.Click
        If Not Me.Documento Is Nothing Then
            Dim documenti As New ParsecAtt.DocumentoRepository
            Dim documento = documenti.Where(Function(c) c.Id = Me.Documento.Id).FirstOrDefault
            If Not documento Is Nothing Then
                documento.Note = Me.NoteTextBox.Text
                documenti.SaveChanges()
            End If
            documenti.Dispose()
        End If
    End Sub

#End Region

#Region "GESTIONE BOZZA"

    Protected Sub TrovaBozzaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaBozzaImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/RicercaBozzaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaBozzaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 370, queryString, False)
    End Sub

    Protected Sub AggiornaBozzaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBozzaImageButton.Click

        If Not ParsecUtility.SessionManager.Bozza Is Nothing Then

            Dim copyHttpPath As String = String.Empty

            If TypeOf (ParsecUtility.SessionManager.Bozza) Is ParsecAtt.Bozza Then
                Dim bozza As ParsecAtt.Bozza = ParsecUtility.SessionManager.Bozza
                Me.BozzaTextBox.Text = bozza.Descrizione
                copyHttpPath = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeBozze") & bozza.Nomefile
            Else
                Dim documento As ParsecAtt.Documento = ParsecUtility.SessionManager.Bozza
                Me.BozzaTextBox.Text = documento.ToString & " " & documento.Descrizione
                copyHttpPath = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), documento.Data.Value.Year, documento.Nomefile)
            End If
            Me.PercorsoRemotoCorpoDocumentoTextBox.Text = copyHttpPath ' bozza.Id.ToString
            ParsecUtility.SessionManager.Bozza = Nothing

        End If
    End Sub

    'Protected Sub EliminaBozzaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaBozzaImageButton.Click
    '    Me.BozzaTextBox.Text = String.Empty
    '    Me.IdBozzaTextBox.Text = String.Empty
    'End Sub

    Protected Sub EliminaBozzaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaBozzaImageButton.Click
        Me.BozzaTextBox.Text = String.Empty
        Me.PercorsoRemotoCorpoDocumentoTextBox.Text = String.Empty
    End Sub

#End Region

#Region "GESTIONE UFFICIO"

    Protected Sub TrovaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUfficioImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUfficioImageButton.ClientID)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("idModulo", 3)
        parametriPagina.Add("idUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "100,200")
        parametriPagina.Add("ultimoLivelloStruttura", "200")
        parametriPagina.Add("ApplicaAbilitazioni", True)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub

    Protected Sub AggiornaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUfficioImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")

            Dim idUfficio As Integer = struttureSelezionate.First.Id


            Dim idUfficioPrecedente As Integer = Me.UfficioSelezionato

            Me.UfficioSelezionato = 0

            'SE NON HO CANCELLATO L'UFFICIO PRECEDENTE
            If idUfficioPrecedente = 0 Then
                If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
                    idUfficioPrecedente = CInt(Me.IdUfficioTextBox.Text)
                Else
                    If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
                        idUfficioPrecedente = CInt(Me.IdSettoreTextBox.Text)
                    End If
                End If
            End If






            Dim strutture As New ParsecAtt.StrutturaViewRepository
            Dim struttura = strutture.GetQuery.Where(Function(c) c.Id = idUfficio).FirstOrDefault
            Dim idPadre As Integer = 0

            If struttura.IdGerarchia = 200 Then 'SECONDO LIVELLO
                'AGGIORNO L'UFFICIO
                Me.UfficioTextBox.Text = struttureSelezionate.First.Descrizione
                Me.IdUfficioTextBox.Text = idUfficio.ToString

                'AGGIORNO LA STRUTTURA DI PRIMO LIVELLO ES. SETTORE (PARAMETRIZZARE IL LIVELLO)
                Dim settore = Me.GetIdStruttura(idUfficio, 100)
                Me.IdSettoreTextBox.Text = settore.Id.ToString
                Me.SettoreTextBox.Text = settore.Descrizione
                idPadre = struttura.IdPadre
            Else
                Me.IdUfficioTextBox.Text = String.Empty
                Me.UfficioTextBox.Text = String.Empty

                Me.IdSettoreTextBox.Text = idUfficio.ToString
                Me.SettoreTextBox.Text = struttureSelezionate.First.Descrizione
                idPadre = struttura.Id
            End If



            '*************************************************************************************************************
            'AGGIUNGO ALLA VISIBILITA' IL RESPONSABILE DELLA STRUTTURA DI PRIMO LIVELLLO ES. SETTORE
            '*************************************************************************************************************

            'CANCELLO L'UTENTE DI VISIBILITA'
            Dim strutturaPrecedente = strutture.GetQuery.Where(Function(c) c.Id = idUfficioPrecedente).FirstOrDefault
            If Not strutturaPrecedente Is Nothing Then
                Dim idPadrePrecedente = strutturaPrecedente.IdPadre
                Dim responsabileSettorePrecedente = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 400 And c.Responsabile And (c.IdPadre = idPadrePrecedente Or c.IdPadre = strutturaPrecedente.Id)).FirstOrDefault
                If Not responsabileSettorePrecedente Is Nothing Then
                    If responsabileSettorePrecedente.IdUtente.HasValue Then
                        Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Me.Visibilita.Where(Function(c) c.IdEntita = responsabileSettorePrecedente.IdUtente And c.TipoEntita = ParsecAdmin.TipoEntita.Utente).FirstOrDefault
                        Me.Visibilita.Remove(utenteVisibilita)
                    End If
                End If
            End If

            Dim responsabileSettore = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 400 And c.Responsabile And c.IdPadre = idPadre).FirstOrDefault
            If Not responsabileSettore Is Nothing Then
                If responsabileSettore.IdUtente.HasValue Then
                    Me.AggiungiUtenteVisibilita(responsabileSettore.IdUtente)
                End If
            End If
            strutture.Dispose()


            '*************************************************************************************************************

            Me.AggiornaFirme()

            Session("SelectedStructures") = Nothing
        End If
    End Sub


    'Private Sub AggiornaFirme()
    '    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '    Dim gerarchiaPersona As Integer = 400
    '    Dim strutture As New ParsecAtt.StrutturaViewRepository
    '    Dim idGerarchia As Integer = 0
    '    Dim idQualifica As Integer = 0
    '    Dim notificato As Boolean = False
    '    Dim qualificheEsaminate As New List(Of Integer)
    '    For Each firma In Me.Firme



    '        Select Case firma.IdTipologiaFirma
    '            Case 1 'STATICA
    '                firma.IdUtente = firma.IdUtente
    '            Case 2  'UTENTE COLLEGATO
    '                firma.IdUtente = utenteCollegato.Id
    '                firma.DefaultStruttura = utenteCollegato.Nome & " " & utenteCollegato.Cognome
    '            Case 3 '  ORGANIGRAMMA
    '                idGerarchia = firma.IdGerarchia
    '                idQualifica = firma.IdTipologiaQualifica

    '                If Not qualificheEsaminate.Contains(idQualifica) Then
    '                    notificato = False
    '                Else
    '                    notificato = True
    '                End If
    '                Dim ufficioSettore As ParsecAtt.Struttura = Nothing
    '                Dim idUfficio As Integer = 0
    '                If String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
    '                    idUfficio = CInt(Me.IdSettoreTextBox.Text)
    '                Else
    '                    idUfficio = CInt(Me.IdUfficioTextBox.Text)
    '                End If

    '                ufficioSettore = strutture.GetQuery.Where(Function(c) c.IdGerarchia = idGerarchia And (c.Id = Me.IdSettoreTextBox.Text Or c.Id = idUfficio)).FirstOrDefault
    '                If ufficioSettore Is Nothing Then
    '                    ufficioSettore = strutture.GetQuery.Where(Function(c) c.Id = idUfficio).FirstOrDefault
    '                End If


    '                Dim persona = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = gerarchiaPersona And c.IdQualifica = idQualifica And c.IdPadre = ufficioSettore.Id).FirstOrDefault

    '                If persona Is Nothing Then
    '                    Dim uffici = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 200 And c.IdPadre = ufficioSettore.Id).ToList
    '                    For Each uf In uffici
    '                        Dim uffId As Integer = uf.Id
    '                        persona = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = gerarchiaPersona And c.IdQualifica = idQualifica And c.IdPadre = uffId).FirstOrDefault
    '                        If Not persona Is Nothing Then
    '                            Exit For
    '                        End If
    '                    Next

    '                End If


    '                If Not persona Is Nothing Then
    '                    firma.IdUtente = persona.IdUtente
    '                    firma.DefaultStruttura = persona.Descrizione
    '                Else
    '                    If Not notificato Then
    '                        Dim qualifiche As New ParsecAdmin.QualificaOrganigrammaRepository
    '                        Dim qualifica As ParsecAdmin.QualificaOrganigramma = qualifiche.GetQuery.Where(Function(c) c.Id = idQualifica).FirstOrDefault
    '                        ParsecUtility.Utility.MessageBox("L'ufficio selezionato non possiede un " & qualifica.Nome.ToLower & "!" & vbCrLf & "Verificare l'organigramma.", False)
    '                        qualifiche.Dispose()
    '                        notificato = True

    '                    End If
    '                    qualificheEsaminate.Add(idQualifica)
    '                End If

    '        End Select

    '        If Me.Documento Is Nothing Then
    '            firma.IdUtentePredefinito = firma.IdUtente
    '        End If

    '    Next
    '    strutture.Dispose()
    'End Sub


    Private Sub AggiornaFirme()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim gerarchiaPersona As Integer = 400
        Dim strutture As New ParsecAtt.StrutturaViewRepository
        Dim idGerarchia As Integer = 0
        Dim idQualifica As Integer = 0
        Dim notificato As Boolean = False
        Dim qualificheEsaminate As New List(Of Integer)

        Dim utenti As New ParsecAdmin.UserRepository

        For Each firma In Me.Firme

            Select Case firma.IdTipologiaFirma
                Case 1 'STATICA
                    firma.IdUtente = firma.IdUtente

                Case 2  'UTENTE COLLEGATO
                    firma.IdUtente = utenteCollegato.Id
                    firma.DefaultStruttura = utenteCollegato.Nome & " " & utenteCollegato.Cognome

                Case 3 '  ORGANIGRAMMA
                    idGerarchia = firma.IdGerarchia
                    idQualifica = firma.IdTipologiaQualifica

                    If Not qualificheEsaminate.Contains(idQualifica) Then
                        notificato = False
                    Else
                        notificato = True
                    End If
                    Dim ufficioSettore As ParsecAtt.Struttura = Nothing
                    Dim idUfficio As Integer = 0
                    If String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
                        idUfficio = CInt(Me.IdSettoreTextBox.Text)
                    Else
                        idUfficio = CInt(Me.IdUfficioTextBox.Text)
                    End If

                    ufficioSettore = strutture.GetQuery.Where(Function(c) c.IdGerarchia = idGerarchia And (c.Id = Me.IdSettoreTextBox.Text Or c.Id = idUfficio)).FirstOrDefault
                    If ufficioSettore Is Nothing Then
                        ufficioSettore = strutture.GetQuery.Where(Function(c) c.Id = idUfficio).FirstOrDefault
                    End If


                    Dim persona = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = gerarchiaPersona And c.IdQualifica = idQualifica And c.IdPadre = ufficioSettore.Id).FirstOrDefault

                    If persona Is Nothing Then
                        Dim uffici = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 200 And c.IdPadre = ufficioSettore.Id).ToList
                        For Each uf In uffici
                            Dim uffId As Integer = uf.Id
                            persona = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = gerarchiaPersona And c.IdQualifica = idQualifica And c.IdPadre = uffId).FirstOrDefault
                            If Not persona Is Nothing Then
                                Exit For
                            End If
                        Next

                    End If


                    If Not persona Is Nothing Then
                        firma.IdUtente = persona.IdUtente
                        firma.DefaultStruttura = persona.Descrizione
                    Else
                        If Not notificato Then
                            Dim qualifiche As New ParsecAdmin.QualificaOrganigrammaRepository
                            Dim qualifica As ParsecAdmin.QualificaOrganigramma = qualifiche.GetQuery.Where(Function(c) c.Id = idQualifica).FirstOrDefault
                            ParsecUtility.Utility.MessageBox("L'ufficio selezionato non possiede un " & qualifica.Nome.ToLower & "!" & vbCrLf & "Verificare l'organigramma.", False)
                            qualifiche.Dispose()
                            notificato = True

                        End If
                        qualificheEsaminate.Add(idQualifica)
                    End If

            End Select

            If Me.Documento Is Nothing Then
                firma.IdUtentePredefinito = firma.IdUtente
            End If

            'IMPOSTAZIONE DI DEFAULT
            firma.DefaultQualifica = firma.DefaultQualifica

            Dim utente = utenti.Where(Function(c) c.Id = firma.IdUtente).FirstOrDefault
            If Not utente Is Nothing Then
                If utente.Sesso = "M" Then
                    firma.DefaultQualifica = firma.DefaultQualifica
                Else
                    If Not String.IsNullOrEmpty(firma.DefaultQualificaAlFemminile) Then
                        firma.DefaultQualifica = firma.DefaultQualificaAlFemminile
                    End If
                End If
            End If

        Next
        utenti.Dispose()
        strutture.Dispose()
    End Sub


    Private Function GetIdStruttura(ByVal id As Integer, ByVal livello As Integer) As ParsecAtt.KeyValue
        Dim strutture As New ParsecAdmin.StructureRepository
        Dim s = strutture.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
        Dim idG As Integer = s.IdGerarchia
        Dim res As Integer = 0
        Dim desc As String = String.Empty
        While idG > livello
            s = strutture.GetQuery.Where(Function(c) c.Id = s.IdPadre).FirstOrDefault
            idG = s.IdGerarchia
            res = s.Id
            desc = s.Descrizione
        End While

        Return New ParsecAtt.KeyValue With {.Id = res, .Descrizione = desc}
    End Function

    Protected Sub EliminaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUfficioImageButton.Click

        'MEMORIZZO L'UFFICIO O IL SETTORE IMPOSTATO SE PRESENTE
        If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) OrElse Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
            If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
                Me.UfficioSelezionato = CInt(Me.IdUfficioTextBox.Text)
            Else
                Me.UfficioSelezionato = CInt(Me.IdSettoreTextBox.Text)
            End If
        End If

        'CANCELLO L'UTENTE DI VISIBILITA'
        Dim strutture As New ParsecAtt.StrutturaViewRepository
        Dim strutturaPrecedente = strutture.GetQuery.Where(Function(c) c.Id = Me.UfficioSelezionato).FirstOrDefault
        If Not strutturaPrecedente Is Nothing Then
            Dim idPadrePrecedente = strutturaPrecedente.IdPadre
            Dim responsabileSettorePrecedente = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 400 And c.Responsabile And (c.IdPadre = idPadrePrecedente Or c.IdPadre = strutturaPrecedente.Id)).FirstOrDefault
            If Not responsabileSettorePrecedente Is Nothing Then
                If responsabileSettorePrecedente.IdUtente.HasValue Then
                    Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Me.Visibilita.Where(Function(c) c.IdEntita = responsabileSettorePrecedente.IdUtente And c.TipoEntita = ParsecAdmin.TipoEntita.Utente).FirstOrDefault
                    Me.Visibilita.Remove(utenteVisibilita)
                End If
            End If
        End If


        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
    End Sub


#End Region

#Region "GESTIONE CONTABILITA'"

    '*************************************************************************************************************************************************************************
    'GESTIONE IMPEGNI DI SPESA
    '*************************************************************************************************************************************************************************
    'Protected Sub AggiungiImpegnoSpesaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiImpegnoSpesaImageButton.Click

    '    If (Me.Liquidazioni.Count + Me.ImpegniSpesa.Count + Me.Accertamenti.Count) >= 20 Then
    '        ParsecUtility.Utility.MessageBox("Non è possibile inserire più di 20 elementi nei dati contatabili!", False)
    '        Exit Sub
    '    End If


    '    Dim parametriPagina As New Hashtable
    '    parametriPagina.Add("ElencoImpegniSpesa", Me.ImpegniSpesa)
    '    ParsecUtility.SessionManager.ParametriPagina = parametriPagina


    '    Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/ImpegnoSpesaPage.aspx"
    '    Dim queryString As New Hashtable
    '    queryString.Add("obj", Me.AggiornaImpegnoSpesaImageButton.ClientID)
    '    ParsecUtility.Utility.ShowPopup(pageUrl, 900, 330, queryString, False)
    'End Sub

    Protected Sub AggiungiImpegnoSpesaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiImpegnoSpesaImageButton.Click

        'If (Me.Liquidazioni.Count + Me.ImpegniSpesa.Count + Me.Accertamenti.Count) >= 20 Then
        '    ParsecUtility.Utility.MessageBox("Non è possibile inserire più di 20 elementi nei dati contatabili!", False)
        '    Exit Sub
        'End If


        Dim parametriPagina As New Hashtable
        parametriPagina.Add("ElencoImpegniSpesa", Me.ImpegniSpesa)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina


        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/ImpegnoSpesaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaImpegnoSpesaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 330, queryString, False)
    End Sub


    Protected Sub AggiornaImpegnoSpesaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaImpegnoSpesaImageButton.Click
        If Not ParsecUtility.SessionManager.ImpegnoSpesa Is Nothing Then

            Dim impegno As ParsecAtt.ImpegnoSpesa = ParsecUtility.SessionManager.ImpegnoSpesa
            Dim id As Integer = impegno.Id

            'Se è stato gia memorizzato


            If id <> 0 Then
                impegno = Me.ImpegniSpesa.Where(Function(c) c.Id = id).FirstOrDefault
            Else
                Dim guid As String = impegno.Guid
                impegno = Me.ImpegniSpesa.Where(Function(c) c.Guid = guid).FirstOrDefault
            End If

            If impegno Is Nothing Then
                Me.ImpegniSpesa.Add(ParsecUtility.SessionManager.ImpegnoSpesa)
            Else
                impegno = ParsecUtility.SessionManager.ImpegnoSpesa
            End If



            ParsecUtility.SessionManager.ImpegnoSpesa = Nothing
        End If
    End Sub

    Protected Sub ImpegniSpesaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ImpegniSpesaGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.ModificaImpegno(e.Item)
        End If
        If e.CommandName = "Delete" Then
            Me.EliminaImpegno(e.Item)
        End If
        If e.CommandName = "Copy" Then
            Me.CopiaImpegno(e.Item)
        End If

        If e.CommandName = "Preview" Then
            Me.VisualizzaImpegno(e.Item)
        End If

    End Sub

    Private totaleImportoImpegni As Double

    'Protected Sub ImpegniSpesaGridView_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ImpegniSpesaGridView.ItemDataBound
    '    If TypeOf e.Item Is GridDataItem Then
    '        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
    '        Dim impegno = CType(dataItem.DataItem, ParsecAtt.ImpegnoSpesa)
    '        totaleImportoImpegni += impegno.Importo


    '        Dim nascondi As Boolean = False

    '        'DEDAGROUP
    '        If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then
    '            If impegno.IdImpegnoDedaGroup.HasValue Then
    '                If impegno.Id > 0 Then
    '                    If impegno.IdCapitoloDedaGroup.HasValue Then

    '                        nascondi = True
    '                    End If
    '                End If
    '            End If
    '        End If



    '        If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
    '            Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
    '            btn.Attributes.Add("onclick", "return confirm('Eliminare l`impegno di spesa selezionato?')")

    '            If nascondi Then
    '                btn.ImageUrl = "~\images\vuoto.png"
    '                btn.ToolTip = ""
    '                btn.Attributes.Add("onclick", "return false;")
    '            End If


    '        End If


    '        If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
    '            Dim btnSelect As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)

    '            If nascondi Then
    '                btnSelect.ImageUrl = "~\images\vuoto.png"
    '                btnSelect.ToolTip = ""
    '                btnSelect.Attributes.Add("onclick", "return false;")
    '            End If


    '        End If

    '        If TypeOf dataItem("Copy").Controls(0) Is ImageButton Then
    '            Dim btnCopy As ImageButton = CType(dataItem("Copy").Controls(0), ImageButton)

    '            If nascondi Then
    '                btnCopy.ImageUrl = "~\images\vuoto.png"
    '                btnCopy.ToolTip = ""
    '                btnCopy.Attributes.Add("onclick", "return false;")
    '            End If


    '        End If

    '    End If

    '    If TypeOf e.Item Is GridFooterItem Then
    '        Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
    '        footerItem("Importo").Text = String.Format("{0:N2}", totaleImportoImpegni)
    '    End If



    'End Sub

    Protected Sub ImpegniSpesaGridView_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles ImpegniSpesaGridView.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
            Dim impegno = CType(dataItem.DataItem, ParsecAtt.ImpegnoSpesa)
            totaleImportoImpegni += impegno.Importo


            Dim nascondi As Boolean = False

            'DEDAGROUP
            If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then
                If impegno.IdImpegnoDedaGroup.HasValue Then
                    If impegno.Id > 0 Then
                        If impegno.IdCapitoloDedaGroup.HasValue Then

                            nascondi = True
                        End If
                    End If
                End If
            End If

            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.Attributes.Add("onclick", "return confirm('Eliminare l`impegno di spesa selezionato?')")
                'If nascondi Then
                '    btn.ImageUrl = "~\images\vuoto.png"
                '    btn.ToolTip = ""
                '    btn.Attributes.Add("onclick", "return false;")
                'End If
            End If


            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                Dim btnSelect As ImageButton = CType(dataItem("Select").Controls(0), ImageButton)
                'If nascondi Then
                '    btnSelect.ImageUrl = "~\images\vuoto.png"
                '    btnSelect.ToolTip = ""
                '    btnSelect.Attributes.Add("onclick", "return false;")
                'End If
            End If


            If TypeOf dataItem("Copy").Controls(0) Is ImageButton Then
                Dim btnCopy As ImageButton = CType(dataItem("Copy").Controls(0), ImageButton)
                If nascondi Then
                    btnCopy.ImageUrl = "~\images\vuoto.png"
                    btnCopy.ToolTip = ""
                    btnCopy.Attributes.Add("onclick", "return false;")
                End If
            End If

        End If

        If TypeOf e.Item Is GridFooterItem Then
            Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
            footerItem("Importo").Text = String.Format("{0:N2}", totaleImportoImpegni)
        End If

    End Sub

    Private Sub CopiaImpegno(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim impegno As ParsecAtt.ImpegnoSpesa = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            impegno = Me.ImpegniSpesa.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            impegno = Me.ImpegniSpesa.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If

        If Not impegno Is Nothing Then

            Dim parametriPagina As New Hashtable
            parametriPagina.Add("ElencoImpegniSpesa", Me.ImpegniSpesa)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina


            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/ImpegnoSpesaPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaImpegnoSpesaImageButton.ClientID)
            queryString.Add("copia", True)
            ParsecUtility.Utility.ShowPopup(pageUrl, 900, 300, queryString, False)
            ParsecUtility.SessionManager.ImpegnoSpesa = impegno
        End If
    End Sub


    Private Sub ModificaImpegno(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim impegno As ParsecAtt.ImpegnoSpesa = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            impegno = Me.ImpegniSpesa.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            impegno = Me.ImpegniSpesa.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If

        If Not impegno Is Nothing Then

            Dim parametriPagina As New Hashtable
            parametriPagina.Add("ElencoImpegniSpesa", Me.ImpegniSpesa)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina

            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/ImpegnoSpesaPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaImpegnoSpesaImageButton.ClientID)
            ParsecUtility.Utility.ShowPopup(pageUrl, 900, 300, queryString, False)
            ParsecUtility.SessionManager.ImpegnoSpesa = impegno
        End If
    End Sub


    Private Sub VisualizzaImpegno(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim impegno As ParsecAtt.ImpegnoSpesa = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            impegno = Me.ImpegniSpesa.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            impegno = Me.ImpegniSpesa.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If

        If Not impegno Is Nothing Then

            Dim parametriPagina As New Hashtable
            parametriPagina.Add("ElencoImpegniSpesa", Me.ImpegniSpesa)
            ParsecUtility.SessionManager.ParametriPagina = parametriPagina

            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/ImpegnoSpesaPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaImpegnoSpesaImageButton.ClientID)
            queryString.Add("preview", True)
            ParsecUtility.Utility.ShowPopup(pageUrl, 900, 300, queryString, False)
            ParsecUtility.SessionManager.ImpegnoSpesa = impegno
        End If
    End Sub


    Private Sub EliminaImpegno(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim impegno As ParsecAtt.ImpegnoSpesa = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            impegno = Me.ImpegniSpesa.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            impegno = Me.ImpegniSpesa.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If
        If Not impegno Is Nothing Then
            Me.ImpegniSpesa.Remove(impegno)
        End If
    End Sub

    '*************************************************************************************************************************************************************************


    '*************************************************************************************************************************************************************************
    'GESTIONE LIQUIDAZIONI
    '*************************************************************************************************************************************************************************
    Protected Sub AggiungiLiquidazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiLiquidazioneImageButton.Click

        If (Me.Liquidazioni.Count + Me.ImpegniSpesa.Count + Me.Accertamenti.Count) >= 20 Then
            ParsecUtility.Utility.MessageBox("Non è possibile inserire più di 20 elementi nei dati contatabili!", False)
            Exit Sub
        End If

        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/LiquidazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaLiquidazioneImageButton.ClientID)
        'luca 01/07/2020
        '' ''If Not String.IsNullOrEmpty(Me.CigBandoGaraTextBox.Text) Then
        '' ''    Dim parametriPagina As New Hashtable
        '' ''    parametriPagina.Add("cig", Me.CigBandoGaraTextBox.Text)
        '' ''    ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        '' ''End If

        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 625, queryString, False)
    End Sub


    Protected Sub AggiornaLiquidazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaLiquidazioneImageButton.Click

        If Not ParsecUtility.SessionManager.Liquidazione Is Nothing Then

            Dim liquidazione As ParsecAtt.Liquidazione = ParsecUtility.SessionManager.Liquidazione
            Dim id As Integer = liquidazione.Id

            'Se è stato gia memorizzato
            If id <> 0 Then
                liquidazione = Me.Liquidazioni.Where(Function(c) c.Id = id).FirstOrDefault
            Else
                Dim guid As String = liquidazione.Guid
                liquidazione = Me.Liquidazioni.Where(Function(c) c.Guid = guid).FirstOrDefault
            End If

            If liquidazione Is Nothing Then
                Me.Liquidazioni.Add(ParsecUtility.SessionManager.Liquidazione)
            Else
                liquidazione = ParsecUtility.SessionManager.Liquidazione
            End If

            ParsecUtility.SessionManager.Liquidazione = Nothing
        End If
    End Sub

    Protected Sub LiquidazioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles LiquidazioniGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.ModificaLiquidazione(e.Item)
        End If
        If e.CommandName = "Delete" Then
            Me.EliminaLiquidazione(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.VisualizzaLiquidazione(e.Item)
        End If

        If e.CommandName = "Copy" Then
            Me.CopiaLiquidazione(e.Item)
        End If

    End Sub


    Private totaleImportoLiquidazioni As Double

    Protected Sub LiquidazioniGridView_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles LiquidazioniGridView.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
            Dim liquidazione = CType(dataItem.DataItem, ParsecAtt.Liquidazione)
            totaleImportoLiquidazioni += liquidazione.ImportoLiquidato

            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.Attributes.Add("onclick", "return confirm('Eliminare la liquidazione selezionata?')")
            End If

        End If
        If TypeOf e.Item Is GridFooterItem Then
            Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
            footerItem("ImportoLiquidato").Text = String.Format("{0:N2}", totaleImportoLiquidazioni)
        End If
    End Sub


    Private Sub ModificaLiquidazione(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim liquidazione As ParsecAtt.Liquidazione = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            liquidazione = Me.Liquidazioni.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            liquidazione = Me.Liquidazioni.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If

        If Not liquidazione Is Nothing Then
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/LiquidazionePage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaLiquidazioneImageButton.ClientID)
            ParsecUtility.Utility.ShowPopup(pageUrl, 900, 625, queryString, False)
            ParsecUtility.SessionManager.Liquidazione = liquidazione
        End If

    End Sub


    Private Sub VisualizzaLiquidazione(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim liquidazione As ParsecAtt.Liquidazione = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            liquidazione = Me.Liquidazioni.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            liquidazione = Me.Liquidazioni.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If

        If Not liquidazione Is Nothing Then
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/LiquidazionePage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("preview", True)
            ParsecUtility.Utility.ShowPopup(pageUrl, 900, 625, queryString, False)
            ParsecUtility.SessionManager.Liquidazione = liquidazione
        End If

    End Sub

    Private Sub CopiaLiquidazione(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim liquidazione As ParsecAtt.Liquidazione = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            liquidazione = Me.Liquidazioni.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            liquidazione = Me.Liquidazioni.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If

        If Not liquidazione Is Nothing Then
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/LiquidazionePage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaLiquidazioneImageButton.ClientID)
            queryString.Add("copia", True)
            ParsecUtility.Utility.ShowPopup(pageUrl, 900, 625, queryString, False)
            ParsecUtility.SessionManager.Liquidazione = liquidazione
        End If

    End Sub

    Private Sub EliminaLiquidazione(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim liquidazione As ParsecAtt.Liquidazione = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            liquidazione = Me.Liquidazioni.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            liquidazione = Me.Liquidazioni.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If
        If Not liquidazione Is Nothing Then
            Me.Liquidazioni.Remove(liquidazione)
        End If

    End Sub



    '*************************************************************************************************************************************************************************

    '*************************************************************************************************************************************************************************
    'GESTIONE ACCERTAMENTI 
    '*************************************************************************************************************************************************************************
    Protected Sub AggiungiAccertamentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiAccertamentoImageButton.Click
        If (Me.Liquidazioni.Count + Me.ImpegniSpesa.Count + Me.Accertamenti.Count) >= 20 Then
            ParsecUtility.Utility.MessageBox("Non è possibile inserire più di 20 elementi nei dati contatabili!", False)
            Exit Sub
        End If
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AccertamentoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaAccertamentoImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 300, queryString, False)
    End Sub

    Protected Sub AggiornaAccertamentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaAccertamentoImageButton.Click
        If Not ParsecUtility.SessionManager.Accertamento Is Nothing Then

            Dim accertamento As ParsecAtt.Accertamento = ParsecUtility.SessionManager.Accertamento
            Dim id As Integer = accertamento.Id

            'Se è stato gia memorizzato
            If id <> 0 Then
                accertamento = Me.Accertamenti.Where(Function(c) c.Id = id).FirstOrDefault
            Else
                Dim guid As String = accertamento.Guid
                accertamento = Me.Accertamenti.Where(Function(c) c.Guid = guid).FirstOrDefault
            End If

            If accertamento Is Nothing Then
                Me.Accertamenti.Add(ParsecUtility.SessionManager.Accertamento)
            Else
                accertamento = ParsecUtility.SessionManager.Accertamento
            End If

            ParsecUtility.SessionManager.Accertamento = Nothing
        End If
    End Sub

    Protected Sub AccertamentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AccertamentiGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.ModificaAccertamento(e.Item)
        End If
        If e.CommandName = "Delete" Then
            Me.EliminaAccertamento(e.Item)
        End If
        If e.CommandName = "Copy" Then
            Me.CopiaAccertamento(e.Item)
        End If
    End Sub


    Private totaleImportoAccertamenti As Double

    Protected Sub AccertamentiGridView_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles AccertamentiGridView.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
            Dim accertamento = CType(dataItem.DataItem, ParsecAtt.Accertamento)
            totaleImportoAccertamenti += accertamento.Importo

            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.Attributes.Add("onclick", "return confirm('Eliminare l`accertamento selezionato?')")
            End If

        End If
        If TypeOf e.Item Is GridFooterItem Then
            Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
            footerItem("Importo").Text = String.Format("{0:N2}", totaleImportoAccertamenti)
        End If
    End Sub


    Private Sub ModificaAccertamento(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim accertamento As ParsecAtt.Accertamento = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            accertamento = Me.Accertamenti.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            accertamento = Me.Accertamenti.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If

        If Not accertamento Is Nothing Then
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AccertamentoPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaAccertamentoImageButton.ClientID)
            ParsecUtility.Utility.ShowPopup(pageUrl, 900, 300, queryString, False)
            ParsecUtility.SessionManager.Accertamento = accertamento
        End If

    End Sub

    Private Sub CopiaAccertamento(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim accertamento As ParsecAtt.Accertamento = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            accertamento = Me.Accertamenti.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            accertamento = Me.Accertamenti.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If

        If Not accertamento Is Nothing Then
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AccertamentoPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaAccertamentoImageButton.ClientID)
            queryString.Add("copia", True)
            ParsecUtility.Utility.ShowPopup(pageUrl, 900, 300, queryString, False)
            ParsecUtility.SessionManager.Accertamento = accertamento
        End If

    End Sub

    Private Sub EliminaAccertamento(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim accertamento As ParsecAtt.Accertamento = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")

        'Se è stato gia memorizzato
        If id <> 0 Then
            accertamento = Me.Accertamenti.Where(Function(c) c.Id = id).FirstOrDefault
        Else
            Dim guid As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Guid")
            accertamento = Me.Accertamenti.Where(Function(c) c.Guid = guid).FirstOrDefault
        End If
        If Not accertamento Is Nothing Then
            Me.Accertamenti.Remove(accertamento)
        End If

    End Sub

    '*************************************************************************************************************************************************************************


#End Region

#Region "METODI PRIVATI"

    Private Sub AbilitaUiBaseModello(idModello As Integer)

        Dim contabilitaTab As RadTab = Me.AttiTabStrip.FindTabByText("Contabilità")
        'luca 01/07/2020
        '' ''Dim trasparenzaTab As RadTab = Me.AttiTabStrip.FindTabByText("Trasparenza")
        '' ''contabilitaTab.Enabled = False
        ' '' ''If Me.DocumentoCopia Is Nothing Then
        ' '' ''    trasparenzaTab.Enabled = False
        ' '' ''Else
        '' ''trasparenzaTab.Enabled = True
        ' '' ''End If


        If idModello <> -1 Then
            Dim modelli As New ParsecAtt.ModelliRepository
            Dim modello As ParsecAtt.Modello = modelli.GetById(idModello)
            contabilitaTab.Enabled = (modello.Liquidazione OrElse modello.ImpegnoSpesa OrElse modello.Accertamento)
            'trasparenzaTab.Enabled = True

            Me.ImpegniSpesaTable.Visible = modello.ImpegnoSpesa
            Me.LiquidazioniTable.Visible = modello.Liquidazione
            Me.AccertamentiTable.Visible = modello.Accertamento
            modelli.Dispose()
        End If
    End Sub


    Private Function GetVersione(ByVal nomeFile As String) As String
        Dim dot As Integer = nomeFile.IndexOf(".")
        Dim v As Integer = nomeFile.LastIndexOf("_v") + 2
        Dim versione = nomeFile.Substring(v, dot - v)
        Return versione
    End Function

    Private Function GetNomeFileFirmato() As String

        Dim nomefileFirmato As String = IO.Path.GetFileNameWithoutExtension(Me.Documento.Nomefile) & ".pdf.p7m"
        Dim annoEsercizio As Integer = Me.GetAnnoEsercizio(Me.Documento)
        Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefileFirmato)


        If Not IO.File.Exists(localPath) Then

            Dim tipoDocumento = Me.Documento.TipologiaDocumento
            Dim proposta As Boolean = tipoDocumento = ParsecAtt.TipoDocumento.PropostaDetermina OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaDelibera OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaOrdinanza OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaDecreto
            'Dim proposta As Boolean = Me.Documento.DataProposta.HasValue
            'Dim proposta As Boolean = Me.Documento.Modello.Proposta


            Dim prefissoProposta As String = "Prop"

            
            Dim versione As Integer = 0
            Dim v As Integer = 0

            For Each f In Me.Firme
                If Not String.IsNullOrEmpty(f.FileFirmato) Then
                    If Not proposta Then
                        'ESCLUDO LE PROPOSTE FIRMATE SE IL DOCUMENTO E' UN ATTO DEFINITIVO
                        If Not f.FileFirmato.StartsWith(prefissoProposta) Then
                            v = GetVersione(f.FileFirmato)
                            If v > versione Then
                                versione = v
                            End If
                        End If

                    End If

                End If
            Next

            Try
                Dim token = nomefileFirmato.Split("_")
                token(3) = "v" & versione.ToString & ".pdf.p7m"

                nomefileFirmato = String.Join("_", token)
                localPath = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefileFirmato)
                If IO.File.Exists(localPath) Then
                    Return localPath
                End If
            Catch ex As Exception
                'IN CASO DI ESPORTAZIONE DA ALTRI SISTEMI 
                Return Nothing
            End Try

        Else
            Return localPath
        End If
        Return Nothing
    End Function

    '******************************************************************************************
    'Gestione visibilità pulsanti e descrizioni barra di notifica.
    '******************************************************************************************
    Private Sub ImpostaBarraNotifica(documento As ParsecAtt.Documento)

        If documento Is Nothing Then
            Me.VisualizzaStoricoDocumentoImageButton.Visible = False
            Me.VisualizzaDocumentoImageButton.Visible = False
            Me.VisualizzaCopiaDocumentoImageButton.Visible = False
            Me.VisualizzaDocumentoFirmatoImageButton.Visible = False
            Me.VisualizzaDocumentoCollegatoImageButton.Visible = False
            Me.VisualizzaCopiaDocumentoCollegatoImageButton.Visible = False
            Me.InfoUtenteImageButton.Attributes.Remove("onclick")
            Me.InfoUtenteImageButton.Attributes.Remove("onmouseout")
            Exit Sub
        End If

        Me.VisualizzaDocumentoImageButton.ToolTip = "Visualizza documento" & vbCrLf & documento.Nomefile
        Me.VisualizzaCopiaDocumentoImageButton.ToolTip = "Visualizza la copia conforme del documento" & vbCrLf & documento.Nomefile


        Dim proposta As Nullable(Of Boolean) = documento.Modello.Proposta

        If documento.NumVersione > 0 Then
            Me.VisualizzaStoricoDocumentoImageButton.Visible = True
            Dim messaggio As String = "Operazione eseguita da <b>" & Replace(documento.LogUtente.ToUpper, "'", "&acute;") & "</b> il <b>" & String.Format("{0:dd/MM/yyyy}", documento.LogDataRegistrazione) & "</b>"
            Dim width As Integer = 450
            Dim height As Integer = 21
            Me.InfoUtenteImageButton.Attributes.Add("onclick", "ShowTooltip(this,'" & messaggio & "'," & width & "," & height & ");")
            Me.InfoUtenteImageButton.Attributes.Add("onmouseout", "HideTooltip();")
        End If

        Me.VisualizzaDocumentoImageButton.Visible = True

        If proposta.HasValue Then
            Me.VisualizzaCopiaDocumentoImageButton.Visible = Not proposta
        End If

   
        Dim nomefileFirmato As String = Me.GetNomeFileFirmato()
        Me.VisualizzaDocumentoFirmatoImageButton.Visible = Not String.IsNullOrEmpty(nomefileFirmato)



        If Not documento.DocumentoCollegato Is Nothing Then
            Me.VisualizzaDocumentoCollegatoImageButton.Visible = True
            If Not documento.DocumentoCollegato.Modello Is Nothing Then
                If documento.DocumentoCollegato.Modello.Proposta Then
                    Me.InfoDocumentoCollegatoLabel.Text = "<font color='#00156E'>Proposta n. </font><font color='#FF6600'>" & documento.DocumentoCollegato.ContatoreGenerale.ToString & " </font><font color='#00156E'> del </font><font color='#FF6600'>" & String.Format("{0:dd/MM/yyyy}", documento.DocumentoCollegato.DataProposta) & "</font>"
                End If
            End If
        End If

        If Not documento.DocumentoGenerato Is Nothing Then
            Me.VisualizzaDocumentoCollegatoImageButton.Visible = True
            Me.VisualizzaCopiaDocumentoCollegatoImageButton.Visible = True
            If Not documento.DocumentoGenerato.Modello Is Nothing Then
                If Not documento.DocumentoGenerato.Modello.Proposta Then
                    Me.InfoDocumentoCollegatoLabel.Text = "<font color='#00156E'>" & documento.DocumentoGenerato.ToString & " n. </font><font color='#FF6600'>" & documento.DocumentoGenerato.ContatoreGenerale.ToString & " </font><font color='#00156E'> del </font><font color='#FF6600'>" & String.Format("{0:dd/MM/yyyy}", documento.DocumentoGenerato.Data) & "</font>"
                End If
            End If
        End If

    End Sub

    Private Function AbilitatoFunzioneRipubblicazione(documento As ParsecAtt.Documento, utenteCollegato As ParsecAdmin.Utente) As Boolean
        If Not Page.Request("Iter") Is Nothing Then
            Return True
        End If


        Dim res As Boolean = False
        If documento.DataAffissioneDa.HasValue Then

            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim istanza = istanze.GetQuery.Where(Function(c) c.IdDocumento = documento.Id And c.IdModulo = ParsecAdmin.TipoModulo.ATT).FirstOrDefault
            istanze.Dispose()
            If Not istanza Is Nothing Then
                Return False
            End If

            Select Case TipologiaDocumento
                Case ParsecAtt.TipoDocumento.Determina
                    res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaDatiPubblicazioneDetermina)).FirstOrDefault Is Nothing
                Case ParsecAtt.TipoDocumento.Delibera
                    res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaDatiPubbicazioneDelibera)).FirstOrDefault Is Nothing
                Case ParsecAtt.TipoDocumento.Ordinanza
                    res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaDatiPubblicazioneOrdinanza)).FirstOrDefault Is Nothing
                Case ParsecAtt.TipoDocumento.Decreto
                    res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaDatiPubblicazioneDecreto)).FirstOrDefault Is Nothing
            End Select
        Else
            res = True
        End If
        Return res
    End Function

    Private Function VerificaDocumentoInIter(documento As ParsecAtt.Documento) As Boolean
        If Not documento Is Nothing Then
            Dim istanze As New ParsecWKF.IstanzaRepository
            Dim res As Boolean = Not istanze.GetQuery.Where(Function(c) c.IdDocumento = documento.Id And c.IdModulo = ParsecAdmin.TipoModulo.ATT).FirstOrDefault Is Nothing
            istanze.Dispose()
            Return res
        End If
        Return False
    End Function

    Private Function AbilitatoFunzioneModifica(documento As ParsecAtt.Documento, utenteCollegato As ParsecAdmin.Utente) As Boolean
        If Not Page.Request("Iter") Is Nothing Then
            Return True
        End If

        Dim res As Boolean = False
        Dim tipologiaDocumento As ParsecAtt.TipoDocumento = CType(documento.IdTipologiaDocumento, ParsecAtt.TipoDocumento)
        Select Case Me.TipologiaProceduraApertura
            Case ParsecAtt.TipoProcedura.Modifica, ParsecAtt.TipoProcedura.Nuovo

                If Me.VerificaDocumentoInIter(documento) Then
                    Return False
                End If

                Select Case tipologiaDocumento
                    Case ParsecAtt.TipoDocumento.Determina
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaDetermina)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.Delibera
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaDelibera)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.Ordinanza
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaOrdinanza)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.Decreto
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaDecreto)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.PropostaDetermina
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaPropostaDetermina)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.PropostaDelibera
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaPropostaDelibera)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.PropostaOrdinanza
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaPropostaOrdinanza)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.PropostaDecreto
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.ModificaPropostaDecreto)).FirstOrDefault Is Nothing
                End Select
        End Select
        Return res
    End Function

    Private Function AbilitatoFunzioneCancellazione(utenteCollegato As ParsecAdmin.Utente) As Boolean
        If Not Page.Request("Iter") Is Nothing Then
            Return True
        End If

        Dim res As Boolean = False
        Select Case Me.TipologiaProceduraApertura
            Case ParsecAtt.TipoProcedura.Modifica, ParsecAtt.TipoProcedura.Nuovo


                Dim istanze As New ParsecWKF.IstanzaRepository
                Dim istanza = istanze.GetQuery.Where(Function(c) c.IdDocumento = Documento.Id And c.IdModulo = ParsecAdmin.TipoModulo.ATT).FirstOrDefault
                istanze.Dispose()
                If Not istanza Is Nothing Then
                    Return False
                End If

                Select Case Me.TipologiaDocumento
                    Case ParsecAtt.TipoDocumento.Determina
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.CancellaDetermina)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.Delibera
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.CancellaDelibera)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.Ordinanza
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.CancellaOrdinanza)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.Decreto
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.CancellaDecreto)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.PropostaDetermina
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.CancellaPropostaDetermina)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.PropostaDelibera
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.CancellaPropostaDelibera)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.PropostaOrdinanza
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.CancellaPropostaOrdinanza)).FirstOrDefault Is Nothing
                    Case ParsecAtt.TipoDocumento.PropostaDecreto
                        res = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.CancellaPropostaDecreto)).FirstOrDefault Is Nothing
                End Select
            Case ParsecAtt.TipoProcedura.Numerazione
                Return False
            Case ParsecAtt.TipoProcedura.Pubblicazione
                Return False
            Case ParsecAtt.TipoProcedura.ModificaAmministrativa
                Return True
        End Select
        Return res
    End Function

    Private Function AbilitatoFunzioneNuovoDocumento(utenteCollegato As ParsecAdmin.Utente) As Boolean

        If Not Page.Request("Iter") Is Nothing Then
            Return True
        End If

        Dim abilitatoNuovoDocumento As Boolean = True
        Dim nuovoDocumento As ParsecAdmin.Funzione = Nothing
        Select Case Me.TipologiaDocumento
            Case ParsecAtt.TipoDocumento.Determina
                nuovoDocumento = utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.NuovaDetermina)).FirstOrDefault
            Case ParsecAtt.TipoDocumento.Delibera
                nuovoDocumento = utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.NuovaDelibera)).FirstOrDefault
            Case ParsecAtt.TipoDocumento.Ordinanza
                nuovoDocumento = utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.NuovaOrdinanza)).FirstOrDefault
            Case ParsecAtt.TipoDocumento.Decreto
                nuovoDocumento = utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.NuovoDecreto)).FirstOrDefault
            Case ParsecAtt.TipoDocumento.PropostaDetermina
                nuovoDocumento = utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.NuovaPropostaDetermina)).FirstOrDefault
            Case ParsecAtt.TipoDocumento.PropostaDelibera
                nuovoDocumento = utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.NuovaPropostaDelibera)).FirstOrDefault
            Case ParsecAtt.TipoDocumento.PropostaOrdinanza
                nuovoDocumento = utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.NuovaPropostaOrdinanza)).FirstOrDefault
            Case ParsecAtt.TipoDocumento.PropostaDecreto
                nuovoDocumento = utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.NuovaPropostaDecreto)).FirstOrDefault
        End Select
        abilitatoNuovoDocumento = Not nuovoDocumento Is Nothing
        Return abilitatoNuovoDocumento
    End Function

    Private Sub EliminaLockDocumento()
        Dim lockDocumento As ParsecAtt.LockDocumento = CType(ParsecUtility.SessionManager.LockDocumento, ParsecAtt.LockDocumento)
        If Not lockDocumento Is Nothing Then
            Dim documentiBloccati As New ParsecAtt.LockDocumentoRepository
            documentiBloccati.Delete(lockDocumento.Id)
            documentiBloccati.Dispose()
            ParsecUtility.SessionManager.LockDocumento = Nothing
        End If
    End Sub

    Private Sub ApplicaFiltro()
        If Not ParsecUtility.SessionManager.FiltroDocumento Is Nothing Then
            Dim filtro As ParsecAtt.FiltroDocumento = ParsecUtility.SessionManager.FiltroDocumento

            '**********************************************************************************************
            'ESCLUDO TUTTI I DOCUMENTI IL CUI MODELLO NON PREVEDE LA PUBBLICAZIONE
            '**********************************************************************************************
            If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Pubblicazione Then
                filtro.EscludiSenzaPubblicazione = True
            End If
            '**********************************************************************************************



            Dim documenti As New ParsecAtt.DocumentoRepository
            Me.Documenti = documenti.GetView(filtro)
            documenti.Dispose()
            Me.DocumentiGridView.Rebind()
            ParsecUtility.SessionManager.FiltroDocumento = Nothing
            Me.ResettaVista()
        End If
    End Sub

    Private Sub CaricaUfficioDefault()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        If utenteCollegato.CodiceStutturaDefault.HasValue Then
            Dim codice As Integer = utenteCollegato.CodiceStutturaDefault
            Dim strutture As New ParsecAtt.StrutturaViewRepository
            Dim struttura = strutture.GetQuery.Where(Function(c) c.Codice = codice And c.LogStato Is Nothing).FirstOrDefault
            Dim idPadre As Integer = 0
            If Not struttura Is Nothing Then

                If struttura.IdGerarchia = 200 Then  'UFFICIO
                    Me.IdUfficioTextBox.Text = struttura.Id
                    Me.UfficioTextBox.Text = struttura.Descrizione
                    '<add key="parsec_LivelloStrutturaRegistroDetermine" value="100" />
                    'Aggiorno il settore (parametrizzare il livello)
                    Dim settore = Me.GetIdStruttura(struttura.Id, 100)
                    Me.IdSettoreTextBox.Text = settore.Id.ToString
                    Me.SettoreTextBox.Text = settore.Descrizione
                    idPadre = struttura.IdPadre
                Else
                    Me.IdSettoreTextBox.Text = struttura.Id
                    Me.SettoreTextBox.Text = struttura.Descrizione
                    idPadre = struttura.Id
                End If



                Dim responsabileSettore = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 400 And c.Responsabile And c.IdPadre = idPadre).FirstOrDefault
                If Not responsabileSettore Is Nothing Then
                    If responsabileSettore.IdUtente.HasValue Then
                        Me.AggiungiUtenteVisibilita(responsabileSettore.IdUtente)
                    End If
                End If



            End If
            strutture.Dispose()
        End If
    End Sub

    Private Sub AggiornaVista(ByVal documento As ParsecAtt.Documento, ByVal selezione As Boolean)

        'Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), documento.PercorsoRelativo.Replace("\", ""), documento.Nomefile)
        'If Not IO.File.Exists(localPath) Then
        'End If


        CararicaDaDB = True
        Dim documenti As New ParsecAtt.DocumentoRepository


        If selezione Then
            Me.TipologiaDocumento = CType(documento.IdTipologiaDocumento, ParsecAtt.TipoDocumento)
        End If

        Me.CaricaModelli()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Not selezione Then
            Me.NomeFileCopia = documento.Nomefile

            Me.CaricaUfficioDefault()
            Me.TipologiaDocumento = Me.TipologiaDocumentoApertura
        End If

        Me.OggettoTextBox.Text = documento.Oggetto

        If selezione Then
            '**************************************************************************************
            'Scheda Contabilità
            '**************************************************************************************
            'Carico le liquidazioni associate al documento corrente
            Me.Liquidazioni = documenti.GetLiquidazioni(documento.Id)

            'Carico gli impegni di spesa associati al documento corrente
            Me.ImpegniSpesa = documenti.GetImpegniSpesa(documento.Id)

            'Carico gli accertamenti associati al documento corrente
            Me.Accertamenti = documenti.GetAccertamenti(documento.Id)
        Else
            Me.DocumentoCopia = documento
            Me.DocumentoCopia.Accertamenti = documenti.GetAccertamenti(documento.Id)
            Me.DocumentoCopia.ImpegniSpesa = documenti.GetImpegniSpesa(documento.Id)
            Me.DocumentoCopia.Liquidazioni = documenti.GetLiquidazioni(documento.Id)


            '********************************************************************************************
            'SCHEDA TRASPARENZA
            '********************************************************************************************
            'luca 01/07/2020
            '' ''If Not documento.Trasparenza Is Nothing Then
            '' ''    Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
            '' ''    Me.Trasparenza = documento.Trasparenza
            '' ''    Me.Trasparenza.Codice = pubblicazioni.GetNuovoCodice()


            '' ''    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
            '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
            '' ''    Dim srcFile As String = String.Empty

            '' ''    'COPIO I FILE NELLA CARTELLA TEMPORANEA

            '' ''    Select Case Me.Trasparenza.TipologiaSezioneTrasparente
            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
            '' ''            For Each p In Me.Trasparenza.ProcedureAffidamento
            '' ''                Dim attoConcessione As ParsecAdmin.AttoConcessione = CType(p, ParsecAdmin.AttoConcessione)


            '' ''                attoConcessione.CurriculumTemp = pathDownload & attoConcessione.UrlCurriculum
            '' ''                attoConcessione.ProgettoTemp = pathDownload & attoConcessione.UrlProgetto
            '' ''                srcFile = percorsoRoot & attoConcessione.Path & attoConcessione.UrlCurriculum
            '' ''                If IO.File.Exists(srcFile) Then
            '' ''                    IO.File.Copy(srcFile, attoConcessione.CurriculumTemp, True)
            '' ''                End If

            '' ''                srcFile = percorsoRoot & attoConcessione.Path & attoConcessione.UrlProgetto
            '' ''                If IO.File.Exists(srcFile) Then
            '' ''                    IO.File.Copy(srcFile, attoConcessione.ProgettoTemp, True)
            '' ''                End If


            '' ''            Next

            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti


            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti

            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso


            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
            '' ''            Dim consulenza As ParsecAdmin.Consulenza = Me.Trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
            '' ''            consulenza.CurriculumTemp = pathDownload & consulenza.UrlCv
            '' ''            consulenza.InsussistenzaTemp = pathDownload & consulenza.UrlAttestazioneInsussistenzaConflittoInteressi

            '' ''            srcFile = percorsoRoot & consulenza.Path & consulenza.UrlCv
            '' ''            If IO.File.Exists(srcFile) Then
            '' ''                IO.File.Copy(srcFile, consulenza.CurriculumTemp, True)
            '' ''            End If

            '' ''            srcFile = percorsoRoot & consulenza.Path & consulenza.UrlAttestazioneInsussistenzaConflittoInteressi

            '' ''            If IO.File.Exists(srcFile) Then
            '' ''                IO.File.Copy(srcFile, consulenza.InsussistenzaTemp, True)
            '' ''            End If

            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale
            '' ''            Dim incaricoAmministrativoDirigenziale As ParsecAdmin.IncaricoAmministrativoDirigenziale = Me.Trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo

            '' ''            incaricoAmministrativoDirigenziale.CurriculumTemp = pathDownload & incaricoAmministrativoDirigenziale.urlcv
            '' ''            incaricoAmministrativoDirigenziale.IncompatilitaTemp = pathDownload & incaricoAmministrativoDirigenziale.urlAttestazioneIncompatilita
            '' ''            incaricoAmministrativoDirigenziale.InconferibilitaTemp = pathDownload & incaricoAmministrativoDirigenziale.urlAttestazioneInconferibilita

            '' ''            srcFile = percorsoRoot & incaricoAmministrativoDirigenziale.path & incaricoAmministrativoDirigenziale.urlcv
            '' ''            If IO.File.Exists(srcFile) Then
            '' ''                IO.File.Copy(srcFile, incaricoAmministrativoDirigenziale.CurriculumTemp, True)
            '' ''            End If

            '' ''            srcFile = percorsoRoot & incaricoAmministrativoDirigenziale.path & incaricoAmministrativoDirigenziale.urlAttestazioneIncompatilita
            '' ''            If IO.File.Exists(srcFile) Then
            '' ''                IO.File.Copy(srcFile, incaricoAmministrativoDirigenziale.IncompatilitaTemp, True)
            '' ''            End If

            '' ''            srcFile = percorsoRoot & incaricoAmministrativoDirigenziale.path & incaricoAmministrativoDirigenziale.urlAttestazioneInconferibilita
            '' ''            If IO.File.Exists(srcFile) Then
            '' ''                IO.File.Copy(srcFile, incaricoAmministrativoDirigenziale.InconferibilitaTemp, True)
            '' ''            End If

            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate

            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati

            '' ''            Dim ente As ParsecAdmin.EnteControllato = Me.Trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo

            '' ''            ente.IncompatibilitaTemp = pathDownload & ente.urlDichiarazioneIncompatibilita
            '' ''            ente.InconferibilitaTemp = pathDownload & ente.urlDichiarazioneInconferibilita

            '' ''            srcFile = percorsoRoot & ente.path & ente.urlDichiarazioneIncompatibilita
            '' ''            If IO.File.Exists(srcFile) Then
            '' ''                IO.File.Copy(srcFile, ente.IncompatibilitaTemp, True)
            '' ''            End If

            '' ''            srcFile = percorsoRoot & ente.path & ente.urlDichiarazioneInconferibilita
            '' ''            If IO.File.Exists(srcFile) Then
            '' ''                IO.File.Copy(srcFile, ente.InconferibilitaTemp, True)
            '' ''            End If



            '' ''    End Select

            '' ''    pubblicazioni.Dispose()
            '' ''End If
            'luca 01/07/2020
            '' ''If Not Me.Trasparenza Is Nothing Then
            '' ''    Me.VisualizzaPannelliTrasparenza(documento.Trasparenza.TipologiaSezioneTrasparente)
            '' ''    Me.AggiornaVistaPannelliTrasparenza(documento.Trasparenza, selezione)
            '' ''End If

        End If



        '**************************************************************************************

        Me.TipologieDocumentoComboBox.FindItemByValue(CInt(Me.TipologiaDocumento)).Selected = True

        '********************************************************************************************
        'SCHEDA VISIBILITA (Carico i gruppi e gli utenti associati al protocollo selezionato)
        '********************************************************************************************
        Me.Visibilita = documenti.GetVisibilita(documento.Id)


        '********************************************************************************************
        'SE STO COPIANDO L'ATTO AGGIUNGO L'UTENTE CORRENTE ALLA VISIBILITA
        '********************************************************************************************
        If Not selezione Then
            Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
            utenteVisibilita.IdEntita = utenteCollegato.Id
            utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
            utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.ATT
            utenteVisibilita.Descrizione = (If(utenteCollegato.Username = Nothing, "", utenteCollegato.Username) + " - " + If(utenteCollegato.Cognome = Nothing, "", utenteCollegato.Cognome) + " " + If(utenteCollegato.Nome = Nothing, "", utenteCollegato.Nome))
            utenteVisibilita.LogIdUtente = utenteCollegato.Id
            utenteVisibilita.LogDataOperazione = Now
            utenteVisibilita.AbilitaCancellaEntita = False
            Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)


            'SONO TUTTI CANCELLABILI TRANNE L'UTENTE COLLEGATO
            For Each v In Me.Visibilita
                If v.IdEntita <> utenteCollegato.Id Then
                    v.AbilitaCancellaEntita = True
                End If
            Next

          

        End If
        '********************************************************************************************


        If selezione Then

            '**************************************************************************************
            'Scheda Generale
            '**************************************************************************************

            If Me.AbilitaNumerazioneDeliberaManuale And Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDelibera And Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Numerazione Then
                Me.NumeroAttoTextBox.Text = String.Empty
            Else
                Me.NumeroAttoTextBox.Text = documento.ContatoreGenerale.ToString
            End If


            If documento.ContatoreStruttura.HasValue Then
                Me.NumeroSettoreTextBox.Value = documento.ContatoreStruttura
                Me.InfoSettoreLabel.Text = "<font color='#00156E'>Registro di Settore N° </font><font color='#FF6600'>" & documento.ContatoreStruttura.ToString & " </font><font color='#00156E'> - </font><font color='#FF6600'>" & documento.DescrizioneSettore.ToUpper & "</font>"
            End If

            Me.DataTextBox.SelectedDate = documento.DataDocumento


            Me.NoteTextBox.Text = documento.Note

            If documento.Pubblicato.HasValue Then
                Me.PubblicatoCheckBox.Checked = documento.Pubblicato
            End If



            If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.CambioModello Then
                Dim item = Me.ModelliComboBox.Items.FindItemByValue(documento.IdModello)
                If Not item Is Nothing Then
                    Me.ModelliComboBox.Items.Remove(item)
                End If
            Else
                If documento.IdModello.HasValue Then
                    Try
                        Me.ModelliComboBox.Items.FindItemByValue(documento.IdModello).Selected = True
                    Catch ex As Exception
                        'SE IL MODELLO E STATO DISABILITATO
                        Dim modelli As New ParsecAtt.ModelliRepository
                        Dim modello = modelli.Where(Function(c) c.Id = documento.IdModello).FirstOrDefault
                        If Not modello Is Nothing Then
                            'Dim b = CType(Me.ModelliComboBox.DataSource, List(Of ParsecAtt.KeyValue))
                            'b.Add(New ParsecAtt.KeyValue With {.Id = modello.Id, .Descrizione = modello.Descrizione})
                            'Me.ModelliComboBox.DataSource = b
                            'Me.ModelliComboBox.DataBind()
                            'Me.ModelliComboBox.Items.Clear()
                            Me.ModelliComboBox.Items.Add(New RadComboBoxItem(modello.Descrizione, modello.Id.ToString))
                            Me.ModelliComboBox.FindItemByValue(modello.Id.ToString).Selected = True
                        End If

                    End Try

                End If
            End If

            Me.UfficioTextBox.Text = documento.DescrizioneUfficio
            Me.SettoreTextBox.Text = documento.DescrizioneSettore
            Me.IdUfficioTextBox.Text = documento.IdUfficio.ToString
            Me.IdSettoreTextBox.Text = documento.IdStruttura.ToString


            'ORDINANZA E DECRETO
            If documento.NumeroProtocollo.HasValue Then
                Me.NumeroProtocolloTextBox.Text = documento.NumeroProtocollo.ToString
            End If
            If documento.DataOraRegistrazione.HasValue Then
                Me.DataProtocolloTextBox.Text = String.Format("{0:dd/MM/yyyy}", documento.DataOraRegistrazione)
            End If

            If Me.TipologiaProceduraApertura <> ParsecAtt.TipoProcedura.Pubblicazione Then
                Me.DataAffissioneTextBox.SelectedDate = Nothing
            End If

            'DELIBERA  DETERMINA ORDINANZA E DECRETO
            If documento.DataAffissioneDa.HasValue Then
                Me.DataAffissioneTextBox.SelectedDate = documento.DataAffissioneDa
            End If
            If documento.NumeroRegistroPubblicazione.HasValue Then
                Me.NumeroRegistroPubblicazioneTextBox.Text = documento.NumeroRegistroPubblicazione
            End If
            If documento.GiorniAffissione.HasValue Then
                Me.GiorniAffissioneTextBox.Text = documento.GiorniAffissione
            Else
                If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Pubblicazione Then
                    Me.GiorniAffissioneTextBox.Text = "15"
                    Dim parametri As New ParsecAdmin.ParametriRepository
                    Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("GiorniDefaultPubblicazione", ParsecAdmin.TipoModulo.ATT)
                    If Not parametro Is Nothing Then
                        Me.GiorniAffissioneTextBox.Text = parametro.Valore
                    End If
                    parametri.Dispose()
                End If
            End If


            'DELIBERA
            If documento.GiorniEsecutivita.HasValue Then
                Me.GiorniEsecutivitaTextBox.Text = documento.GiorniEsecutivita.ToString
            End If
            Me.EsecutivitaImmediataCheckBox.Checked = Not documento.GiorniEsecutivita.HasValue

            'If documento.Formalizzazione.HasValue Then
            '    Me.FormalizzataCheckBox.Checked = documento.Formalizzazione
            'End If

            '**************************************************************************************



            '**************************************************************************************
            'Scheda Classificazione
            '**************************************************************************************
            'Carico le classificazioni associate al documento corrente
            Me.Classificazioni = documenti.GetClassificazioni(documento.Id)
            '**************************************************************************************

            '**************************************************************************************
            'Scheda Presenze
            '**************************************************************************************
            'Carico le presenze associate al documento corrente
            'Todo parametro OriginePresenzeVerbale (Fronteaspizio)


            Dim presenze As New ParsecAtt.DocumentoPresenzaRepository
            Me.Presenze = presenze.GetViewDocumento(New ParsecAtt.FiltroDocumentoPresenza With {.IdDocumento = documento.Id})

            'Carico le presenze dalla seduta
            If Me.Presenze.Count = 0 Then
                If documento.IdSeduta.HasValue Then
                    Me.Presenze = presenze.GetViewSeduta(New ParsecAtt.FiltroPresenzaSeduta With {.IdSeduta = documento.IdSeduta})
                End If
            End If
            presenze.Dispose()

            documento.DocumentoCollegato = documenti.GetDocumentoCollegato(documento.IdPadre)
            If Not documento.DocumentoCollegato Is Nothing Then
                documento.DocumentoCollegato.Modello = documenti.GetModello(documento.DocumentoCollegato.IdModello)
            End If

            documento.DocumentoGenerato = documenti.GetDocumentoGenerato(documento.Id)
            If Not documento.DocumentoGenerato Is Nothing Then
                documento.DocumentoGenerato.Modello = documenti.GetModello(documento.DocumentoGenerato.IdModello)
            End If


            documento.Modello = documenti.GetModello(documento.IdModello)

            documento.Seduta = documenti.GetSeduta(documento.IdSeduta)
            If Not documento.Seduta Is Nothing Then
                Me.SedutaTextBox.Text = String.Format("di {0} del {1}", documento.Seduta.DescrizioneTipologiaSeduta, String.Format("{0:dd/MM/yyyy}", documento.Seduta.DataConvocazione))
                Me.IdSedutaTextBox.Text = documento.Seduta.Id.ToString
                documento.OrdineGiorno = documenti.GetOrdineGiorno(documento.IdSeduta, documento.Codice)
                If Not documento.OrdineGiorno Is Nothing Then
                    If documento.OrdineGiorno.IdStatoDiscussione.HasValue Then
                        Me.TipiApprovazioneComboBox.Items.FindItemByValue(documento.OrdineGiorno.IdStatoDiscussione).Selected = True
                    End If
                End If
            End If

            '**************************************************************************************

            '**************************************************************************************
            'SCHEDA VISTI E PARERI
            '**************************************************************************************
            Me.Firme = documenti.GetFirme(documento.Id)
            '**************************************************************************************

            '**************************************************************************************
            'SCHEDA ALLEGATI
            '**************************************************************************************
            Me.Allegati = documenti.GetAllegati(documento.Id)
            '**************************************************************************************

            'MODALITA PROCEDURA MODIFICARAGIONIERE 11

            If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.ModificaParere Then
                If Not String.IsNullOrEmpty(Me.IdFirmaModificabile) Then
                    If Me.Allegati.Count > 0 Then
                        Me.ElencoIdAllegatiBloccati = String.Join(";", Me.Allegati.Select(Function(c) c.Id).ToList)
                    End If
                End If
            End If


            '********************************************************************************************
            'SCHEDA TRASPARENZA
            '********************************************************************************************
            'luca 01/07/2020
            '' ''Me.Trasparenza = documento.Trasparenza
            '' ''If Not Me.Trasparenza Is Nothing Then
            '' ''    Me.VisualizzaPannelliTrasparenza(documento.Trasparenza.TipologiaSezioneTrasparente)
            '' ''    Me.AggiornaVistaPannelliTrasparenza(documento.Trasparenza, selezione)
            '' ''End If

            '********************************************************************************************

            '**************************************************************************************
            'SCHEDA FASCICOLI
            '**************************************************************************************
            Me.Fascicoli = documento.Fascicoli
            If Not Me.Fascicoli Is Nothing Then
                AggiornaGrigliaFascicoli()
            End If
            '**************************************************************************************

            Me.RiservatoCheckBox.Checked = documento.Riservato

            If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Pubblicazione Then
                'AGGIUNGO IL GRUPPO DI VISIBILITA' TUTTI GLI UTENTI SE NON E' RISERVATO
                If Me.RiservatoCheckBox.Checked = False Then
                    Me.AggiungiGruppoVisibilitaTuttiUtenti()
                End If
            End If

            If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Numerazione Then



                '*******************************************************
                'VERIFICO SE IL DOCUMENTO ADOTTATO E' PUBBLICABILE
                '*******************************************************
                Dim pubblicabile As Boolean = False
                Dim modelli As New ParsecAtt.ModelliRepository
                Dim idModelloAdottato As Nullable(Of Integer) = documento.Modello.IdModelloAdottato
                If idModelloAdottato.HasValue Then
                    Dim modelloAdottato = modelli.Where(Function(c) c.Id = idModelloAdottato).FirstOrDefault
                    If Not modelloAdottato Is Nothing Then
                        If modelloAdottato.Pubblicazione.HasValue Then
                            If modelloAdottato.Pubblicazione Then
                                pubblicabile = True
                            End If
                        End If
                        If Not pubblicabile Then
                            'AGGIUNGO IL GRUPPO DI VISIBILITA' TUTTI GLI UTENTI SE NON E' RISERVATO
                            If Me.RiservatoCheckBox.Checked = False Then
                                Me.AggiungiGruppoVisibilitaTuttiUtenti()
                            End If

                        End If
                    End If
                End If
                modelli.Dispose()
            End If




            '**************************************************************************************
            'Aggiorno l'oggetto 
            '**************************************************************************************
            Me.Documento = documento
            '**************************************************************************************

        End If

        Me.ImpostaDescrizioneTipologiaDocumento(Me.TipologiaDocumento, Me.TipologiaProceduraCorrente)

        documenti.Dispose()
    End Sub

    Private Sub ImpostaAbilitazionePannelloAffissione(enabled As Boolean)
        Me.DataAffissioneTextBox.Enabled = enabled
        Me.GiorniAffissioneTextBox.Enabled = enabled
        Me.NumeroRegistroPubblicazioneTextBox.Enabled = Not AbilitatoAlbo()
    End Sub

    Private Sub ResettaVista()
        Me.SelectedItems = Nothing
        Me.DocumentoCopia = Nothing
        Me.NomeFileCopia = String.Empty

        Me.InfoSettoreLabel.Text = String.Empty
        Me.Documento = Nothing
        Me.InfoDocumentoCollegatoLabel.Text = String.Empty

        'Numero Atto viene abilitatato sulla trasformazione
        Me.NumeroAttoTextBox.Text = String.Empty
        Me.NumeroSettoreTextBox.Text = String.Empty


        Me.ModelliComboBox.SelectedIndex = 0


        Me.UfficioTextBox.Text = String.Empty
        Me.IdUfficioTextBox.Text = String.Empty
        Me.SettoreTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty




        Me.OggettoTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty
        Me.PubblicatoCheckBox.Checked = False
        'Me.IdBozzaTextBox.Text = String.Empty
        Me.PercorsoRemotoCorpoDocumentoTextBox.Text = String.Empty
        Me.BozzaTextBox.Text = String.Empty

        Me.ClassificazioneTextBox.Text = String.Empty
        Me.IdClassificazioneTextBox.Text = String.Empty
        Me.AnnotazioniTextBox.Text = String.Empty

        'ORDINANZA E DECRETO
        Me.NumeroProtocolloTextBox.Text = String.Empty
        Me.DataProtocolloTextBox.Text = String.Empty

        'DELIBERA  DETERMINA ORDINANZA E DECRETO

        If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Pubblicazione Then
            Me.DataAffissioneTextBox.SelectedDate = Now
        End If



        Me.NumeroRegistroPubblicazioneTextBox.Text = String.Empty
        Me.GiorniAffissioneTextBox.Text = String.Empty

        'DELIBERA
        Me.EsecutivitaImmediataCheckBox.Checked = True
        Me.GiorniEsecutivitaTextBox.Text = String.Empty
        Me.TipiApprovazioneComboBox.SelectedIndex = 0


        Me.SedutaTextBox.Text = String.Empty
        Me.IdSedutaTextBox.Text = String.Empty

        Me.Liquidazioni = New List(Of ParsecAtt.Liquidazione)
        Me.ImpegniSpesa = New List(Of ParsecAtt.ImpegnoSpesa)
        Me.Accertamenti = New List(Of ParsecAtt.Accertamento)
        Me.Classificazioni = New List(Of ParsecAtt.DocumentoClassificazione)
        Me.Presenze = New List(Of ParsecAtt.DocumentoPresenza)
        Me.Firme = New List(Of ParsecAtt.Firma)
        Me.Allegati = New List(Of ParsecAtt.Allegato)

        Me.AllegatiPubblicazione = New List(Of ParsecAdmin.AllegatoPubblicazione)

        Me.Visibilita = New List(Of ParsecAdmin.VisibilitaDocumento)

        Me.RiservatoCheckBox.Checked = False

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Nuovo Then
            Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
            utenteVisibilita.IdEntita = utenteCollegato.Id
            utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
            utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.ATT
            utenteVisibilita.Descrizione = (If(utenteCollegato.Username = Nothing, "", utenteCollegato.Username) + " - " + If(utenteCollegato.Cognome = Nothing, "", utenteCollegato.Cognome) + " " + If(utenteCollegato.Nome = Nothing, "", utenteCollegato.Nome))
            utenteVisibilita.LogIdUtente = utenteCollegato.Id
            utenteVisibilita.LogDataOperazione = Now
            utenteVisibilita.AbilitaCancellaEntita = False
            Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)
        End If



        If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Nuovo Then
            Dim parametri As New ParsecAdmin.ParametriRepository
            Me.DataTextBox.SelectedDate = parametri.GetDataValida
            parametri.Dispose()
            Me.CaricaUfficioDefault()
        End If

        'luca 01/07/2020
        '' ''Me.Trasparenza = Nothing
        '' ''Me.NascondiPannelliTrasparenza()

        '' ''Me.ResettaVistaPannelliTrasparenza()

        Me.ResettaVistaFascicoli()
        'luca 01/07/2020
        '' ''Me.IdGaraContratti = String.Empty
    End Sub

    Private Sub CaricaTipologieDocumento()
        Dim tipologie As New ParsecAtt.TipologieDocumentoRepository
        Me.TipologieDocumentoComboBox.DataValueField = "Id"
        Me.TipologieDocumentoComboBox.DataTextField = "Descrizione"
        Me.TipologieDocumentoComboBox.DataSource = tipologie.GetKeyValue(New ParsecAtt.FiltroTipologiaDocumento With {.Modellizzabile = True})
        Me.TipologieDocumentoComboBox.DataBind()
        tipologie.Dispose()
    End Sub

    Private Sub CaricaModelli()
        Dim modelli As New ParsecAtt.ModelliRepository
        Me.ModelliComboBox.DataValueField = "Id"
        Me.ModelliComboBox.DataTextField = "Descrizione"
        Me.ModelliComboBox.DataSource = modelli.GetKeyValue(New ParsecAtt.FiltroModello With {.TipologiaDocumento = Me.TipologiaDocumento, .Disabilitato = False})
        Me.ModelliComboBox.DataBind()
        Me.ModelliComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.ModelliComboBox.SelectedIndex = 0
        modelli.Dispose()
    End Sub

    Private Sub CaricaStatiDiscussione()
        Dim statiDiscussione As New ParsecAtt.StatoDiscussioneRepository
        Me.TipiApprovazioneComboBox.DataValueField = "Id"
        Me.TipiApprovazioneComboBox.DataTextField = "Descrizione"
        Me.TipiApprovazioneComboBox.DataSource = statiDiscussione.GetKeyValue(Nothing)
        Me.TipiApprovazioneComboBox.DataBind()
        Me.TipiApprovazioneComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.TipiApprovazioneComboBox.SelectedIndex = 0
        statiDiscussione.Dispose()
    End Sub

    Private Sub ImpostaDescrizioneTipologiaDocumento(ByVal tipo As ParsecAtt.TipoDocumento, procedura As ParsecAtt.TipoProcedura)
        Me.AreaInfoLabel.Text = Me.GetDescrizioneTipologiaDocumento(tipo, procedura)
    End Sub

    Private Function GetDescrizioneTipologiaDocumento(ByVal tipo As ParsecAtt.TipoDocumento, procedura As ParsecAtt.TipoProcedura) As String
        Dim res As String = String.Empty
        Dim descrizioneFunzione As String = String.Empty

        Select Case procedura
            Case ParsecAtt.TipoProcedura.Nuovo
                Select Case tipo
                    Case ParsecAtt.TipoDocumento.Decreto
                        descrizioneFunzione = "Nuovo"
                    Case Else
                        descrizioneFunzione = "Nuova"
                End Select

            Case ParsecAtt.TipoProcedura.Pubblicazione
                descrizioneFunzione = "Pubblicazione"
            Case ParsecAtt.TipoProcedura.Numerazione
                descrizioneFunzione = "Numerazione"
            Case ParsecAtt.TipoProcedura.CambioModello
                descrizioneFunzione = "Cambia Modello"
            Case ParsecAtt.TipoProcedura.Classificazione
                descrizioneFunzione = "Classificazione"
            Case ParsecAtt.TipoProcedura.ModificaAmministrativa
                descrizioneFunzione = "Modifica Amministrativa"
            Case ParsecAtt.TipoProcedura.AggiungiDatiContabili
                descrizioneFunzione = "Aggiungi Dati Contabili"
            Case ParsecAtt.TipoProcedura.ModificaParere
                descrizioneFunzione = "Modifica Firma/Aggiungi Allegati"

        End Select

        If Not Me.Documento Is Nothing Then
            res = String.Format("{0} {1} N° {2} del {3}", descrizioneFunzione, Me.Documento.ToString, Me.Documento.ContatoreGenerale.ToString, String.Format("{0:dd/MM/yyyy}", Me.Documento.DataDocumento))
        Else
            Dim descrizioneAtto As String = String.Empty
            Select Case tipo
                Case ParsecAtt.TipoDocumento.PropostaDelibera
                    descrizioneAtto = "Proposta di Delibera"
                Case ParsecAtt.TipoDocumento.Delibera
                    descrizioneAtto = "Delibera"
                Case ParsecAtt.TipoDocumento.Determina
                    descrizioneAtto = "Determina"
                Case ParsecAtt.TipoDocumento.PropostaDetermina
                    descrizioneAtto = "Proposta di Determina"
                Case ParsecAtt.TipoDocumento.Ordinanza
                    descrizioneAtto = "Ordinanza"
                Case ParsecAtt.TipoDocumento.PropostaOrdinanza
                    descrizioneAtto = "Proposta di Ordinanza"
                Case ParsecAtt.TipoDocumento.Decreto
                    descrizioneAtto = "Decreto"
                Case ParsecAtt.TipoDocumento.PropostaDecreto
                    descrizioneAtto = "Proposta di Decreto"
            End Select
            res = String.Format("{0} {1}", descrizioneFunzione, descrizioneAtto)
        End If
        Return res
    End Function

    Private Sub AggiornaGrigliaLiquidazioni()
        Me.LiquidazioniGridView.DataSource = Me.Liquidazioni
        Me.LiquidazioniGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaImpegniSpesa()
        Me.ImpegniSpesaGridView.DataSource = Me.ImpegniSpesa
        Me.ImpegniSpesaGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaAccertamenti()
        Me.AccertamentiGridView.DataSource = Me.Accertamenti
        Me.AccertamentiGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaClassificazioni()
        Me.ClassificazioniGridView.DataSource = Me.Classificazioni
        Me.ClassificazioniGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaVisibilita()
        Me.VisibilitaGridView.DataSource = Me.Visibilita
        Me.VisibilitaGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaPresenze()
        Me.PresenzeGridView.DataSource = Me.Presenze
        Me.PresenzeGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaFirme()
        Me.FirmeGridView.DataSource = Me.Firme
        Me.FirmeGridView.DataBind()
    End Sub

    Private Sub AggiornaGrigliaAllegati()
        Me.AllegatiGridView.DataSource = Me.Allegati
        Me.AllegatiGridView.DataBind()
    End Sub

    'luca 01/07/2020
    '' ''Private Sub AggiornaGrigliaAllegatiPubblicazione()

    '' ''    If Me.BandiGareContrattiPanel.Visible Then
    '' ''        Me.AllegatiBandoGaraGridView.DataSource = Me.AllegatiPubblicazione
    '' ''        Me.AllegatiBandoGaraGridView.DataBind()
    '' ''    End If

    '' ''    If Me.PubblicazioneGenericaPanel.Visible Then
    '' ''        Me.AllegatiPubblicazioneGenericaGridView.DataSource = Me.AllegatiPubblicazione
    '' ''        Me.AllegatiPubblicazioneGenericaGridView.DataBind()
    '' ''    End If

    '' ''    If Me.IncaricoPanel.Visible Then
    '' ''        Me.AllegatiIncaricoDipendenteGridView.DataSource = Me.AllegatiPubblicazione
    '' ''        Me.AllegatiIncaricoDipendenteGridView.DataBind()
    '' ''    End If

    '' ''    If Me.CollaborazioneConsulenzaPanel.Visible Then
    '' ''        Me.AllegatiCompensoConsulenzaGridView.DataSource = Me.AllegatiPubblicazione
    '' ''        Me.AllegatiCompensoConsulenzaGridView.DataBind()
    '' ''    End If


    '' ''    If Me.AttiConcessionePanel.Visible Then
    '' ''        Me.AllegatiAttiConcessioneGridView.DataSource = Me.AllegatiPubblicazione
    '' ''        Me.AllegatiAttiConcessioneGridView.DataBind()
    '' ''    End If

    '' ''    If Me.IncarichiAmministrativiDirigenzialiPanel.Visible Then
    '' ''        Me.AllegatiIncarichiAmministrativiDirigenzialiGridView.DataSource = Me.AllegatiPubblicazione
    '' ''        Me.AllegatiIncarichiAmministrativiDirigenzialiGridView.DataBind()
    '' ''    End If

    '' ''    If Me.BandoConcorsoPanel.Visible Then
    '' ''        Me.AllegatiBandoConcorsoGridView.DataSource = Me.AllegatiPubblicazione
    '' ''        Me.AllegatiBandoConcorsoGridView.DataBind()
    '' ''    End If

    '' ''    If Me.EnteControllatoPanel.Visible Then
    '' ''        Me.AllegatiEnteControllatoGridView.DataSource = Me.AllegatiPubblicazione
    '' ''        Me.AllegatiEnteControllatoGridView.DataBind()
    '' ''    End If

    '' ''End Sub

    Private Sub AggiornaGriglia()
        Me.Documenti = Nothing
        Me.DocumentiGridView.Rebind()
    End Sub

    'luca 01/07/2020
    '' ''Private Sub AggiornaGrigliaAttiConcessione()
    '' ''    Me.BeneficiariGridView.DataSource = Me.AttiConcessione
    '' ''    Me.BeneficiariGridView.DataBind()
    '' ''End Sub

    'Private Sub CopiaDocumento(ByVal input As String, ByVal output As String)
    '    Try
    '        If Not IO.File.Exists(output) Then
    '            IO.File.Copy(input, output)
    '            If (IO.File.GetAttributes(output) And IO.FileAttributes.ReadOnly) = IO.FileAttributes.ReadOnly Then
    '                IO.File.SetAttributes(output, IO.FileAttributes.Normal)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    'End Sub

    Private Sub CopiaDocumento(ByVal input As String, ByVal output As String)
        Try
            If Not IO.File.Exists(output) Then
                IO.File.Copy(input, output)
                If (IO.File.GetAttributes(output) And IO.FileAttributes.ReadOnly) = IO.FileAttributes.ReadOnly Then
                    IO.File.SetAttributes(output, IO.FileAttributes.Normal)
                End If
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.Message.Replace("\", "\\"))
        End Try
    End Sub

    Private Function AggiornaModello() As ParsecAtt.Documento

        For Each item As Telerik.Web.UI.GridDataItem In Me.AllegatiGridView.Items
            Dim chk As CheckBox = CType(item("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
            Dim allegato As ParsecAtt.Allegato = Me.Allegati.Where(Function(c) c.Id = id).FirstOrDefault
            If Not allegato Is Nothing Then
                allegato.Pubblicato = chk.Checked
            End If
        Next

        Dim documenti As New ParsecAtt.DocumentoRepository

        '********************************************************************************************
        'E' sempre un nuovo documento.
        '********************************************************************************************
        Dim documento As New ParsecAtt.Documento
        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim proposta As ParsecAtt.Documento = Nothing
        If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Numerazione Then
            proposta = Me.Documento
        End If


        If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Numerazione Then
            Select Case Me.TipologiaDocumento
                Case ParsecAtt.TipoDocumento.PropostaOrdinanza
                    documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza
                Case ParsecAtt.TipoDocumento.PropostaDetermina
                    documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina
                Case ParsecAtt.TipoDocumento.PropostaDelibera
                    documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Delibera

                    If Me.AbilitaNumerazioneDeliberaManuale Then
                        documento.ContatoreGeneraleProposto = Me.NumeroAttoTextBox.Value
                    End If


                Case ParsecAtt.TipoDocumento.PropostaDecreto
                    documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto
            End Select
            documento.IdTipologiaDocumento = CInt(documento.TipologiaDocumento)
        Else
            documento.TipologiaDocumento = Me.TipologiaDocumento
            documento.IdTipologiaDocumento = CInt(Me.TipologiaDocumento)
        End If


        '********************************************************************************************
        'AGGIORNO IL MODELLO DEL DOCUMENTO
        '********************************************************************************************
        Dim modelli As New ParsecAtt.ModelliRepository
        If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Numerazione Then
            proposta.Modello = modelli.GetById(proposta.IdModello)
            documento.Modello = modelli.GetById(proposta.Modello.IdModelloAdottato)
            documento.IdModello = documento.Modello.Id
        Else
            Dim idModello As Integer = CInt(Me.ModelliComboBox.SelectedValue)
            If Not Me.Documento Is Nothing Then
                If idModello = "-1" Then
                    idModello = Me.Documento.IdModello
                End If
            End If

            If idModello <> -1 Then
                If Me.ModelliComboBox.SelectedIndex <> 0 Then
                    documento.IdModello = idModello
                End If

                documento.Modello = modelli.GetById(idModello)
                If documento.Modello.Proposta Then
                    documento.DataProposta = documento.DataDocumento
                Else
                    documento.Data = documento.DataDocumento
                End If
            End If
        End If
        modelli.Dispose()
        '********************************************************************************************


        If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
            documento.IdStruttura = CInt(Me.IdSettoreTextBox.Text)
        End If
        If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
            documento.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            documento.Oggetto = Me.OggettoTextBox.Text.Trim
        End If

        If Not String.IsNullOrEmpty(Me.NoteTextBox.Text) Then
            documento.Note = Me.NoteTextBox.Text.Trim
        End If

        If Me.EsecutivitaImmediataCheckBox.Checked Then
            documento.EsecutivitaImmediata = Me.EsecutivitaImmediataCheckBox.Checked
        End If

        If Not String.IsNullOrEmpty(Me.NumeroSettoreTextBox.Text) Then
            documento.ContatoreStruttura = NumeroSettoreTextBox.Value
        End If


        documento.LogIdUtente = utenteCorrente.Id
        documento.LogUtente = utenteCorrente.Username
        documento.IdAutore = utenteCorrente.Id

        If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Numerazione Then
            documento.IdPadre = proposta.Id
            documento.IdTipologiaSeduta = proposta.IdTipologiaSeduta
        End If

        If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Numerazione Then

            Dim p As New ParsecAdmin.ParametriRepository
            Dim parametroAbilitaImpostazioneManualeDataAtto As ParsecAdmin.Parametri = p.GetByName("AbilitaImpostazioneManualeDataAtto", ParsecAdmin.TipoModulo.ATT)
            Dim abilitaImpostazioneManualeDataAtto As Boolean = False
            If Not parametroAbilitaImpostazioneManualeDataAtto Is Nothing Then
                abilitaImpostazioneManualeDataAtto = (parametroAbilitaImpostazioneManualeDataAtto.Valore = "1")
            End If

            'SE STO NUMERANDO UNA PROPOSTA DI DETERMINA E L'ITER E' DISATTIVATO E IL PARAMETRO E' PRESENTE ED E' IMPOSTATO SU 1 
            'LA DATA DEL DOCUMENTO VIENE VALORIZATA DA QUELLA SPECIFICATA DALL'UTENTE (MODIFICA PER CARMIANO)
            If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina AndAlso Not Me.IterAttivato AndAlso abilitaImpostazioneManualeDataAtto Then
                If Me.DataTextBox.SelectedDate.HasValue Then
                    documento.DataDocumento = New Date(Me.DataTextBox.SelectedDate.Value.Year, Me.DataTextBox.SelectedDate.Value.Month, Me.DataTextBox.SelectedDate.Value.Day, Now.Hour, Now.Minute, Now.Second)
                End If
            Else
                documento.DataDocumento = p.GetDataValida
            End If

            p.Dispose()

        Else
            If Me.DataTextBox.SelectedDate.HasValue Then
                documento.DataDocumento = New Date(Me.DataTextBox.SelectedDate.Value.Year, Me.DataTextBox.SelectedDate.Value.Month, Me.DataTextBox.SelectedDate.Value.Day, Now.Hour, Now.Minute, Now.Second)
            End If
        End If


        '********************************************************************************************
        'AGGIORNO LA SEDUTA DEL DOCUMENTO
        '********************************************************************************************
        Dim seduta As ParsecAtt.Seduta = Nothing

        If Not String.IsNullOrEmpty(Me.IdSedutaTextBox.Text) Then
            documento.IdSeduta = CInt(Me.IdSedutaTextBox.Text)
            seduta = documenti.GetSeduta(CInt(Me.IdSedutaTextBox.Text))
            documento.DataDocumento = seduta.DataConvocazione
            If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Numerazione Then
                If Me.TipiApprovazioneComboBox.SelectedIndex <> 0 Then
                    documento.IdStatoDiscussione = CInt(Me.TipiApprovazioneComboBox.SelectedValue)
                End If
            End If
        End If
        '********************************************************************************************

        'If Not documento.DataDocumento.HasValue Then
        '    documento.DataDocumento = Me.DataTextBox.SelectedDate
        'End If


        '********************************************************************************************
        'ORDINANZA E DECRETO
        '********************************************************************************************
        If Not String.IsNullOrEmpty(Me.NumeroProtocolloTextBox.Text) Then
            documento.NumeroProtocollo = CInt(Me.NumeroProtocolloTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.DataProtocolloTextBox.Text) Then
            documento.DataOraRegistrazione = CDate(Me.DataProtocolloTextBox.Text)
        End If
        '********************************************************************************************

        '********************************************************************************************
        'DELIBERA  DETERMINA ORDINANZA E DECRETO
        '********************************************************************************************
        documento.DataAffissioneDa = Me.DataAffissioneTextBox.SelectedDate
        If Not String.IsNullOrEmpty(Me.GiorniAffissioneTextBox.Text) Then
            documento.GiorniAffissione = CInt(Me.GiorniAffissioneTextBox.Text)

        End If
        If Not String.IsNullOrEmpty(Me.NumeroRegistroPubblicazioneTextBox.Text) Then
            documento.NumeroRegistroPubblicazione = CInt(Me.NumeroRegistroPubblicazioneTextBox.Text)
        End If
        '********************************************************************************************

        '********************************************************************************************
        'DELIBERA
        '********************************************************************************************
        Dim tipoCalcolo As Integer = 0
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("TipoCalcoloDataImmediataEsecutività", ParsecAdmin.TipoModulo.ATT)
        If Not parametro Is Nothing Then
            tipoCalcolo = CInt(parametro.Valore)
        End If

        If Not String.IsNullOrEmpty(Me.GiorniEsecutivitaTextBox.Text) Then
            Dim giorni As Integer = 0
            Integer.TryParse(Me.GiorniEsecutivitaTextBox.Text, giorni)
            If giorni > 0 Then
                documento.GiorniEsecutivita = giorni
            End If
            documento.GiorniEsecutivita = giorni
        End If


        If Me.EsecutivitaImmediataCheckBox.Checked Then
            If tipoCalcolo = 0 Then

                If Not seduta Is Nothing Then
                    documento.DataEsecutivaDa = seduta.DataConvocazione
                End If

            ElseIf tipoCalcolo = 1 Then
                documento.DataEsecutivaDa = documento.DataAffissioneDa

            ElseIf tipoCalcolo = 2 OrElse tipoCalcolo = 3 Then
                If Not seduta Is Nothing Then
                    documento.DataEsecutivaDa = seduta.DataConvocazione
                End If
            End If

        Else
            If Not String.IsNullOrEmpty(Me.GiorniEsecutivitaTextBox.Text) Then
                Dim giorni As Integer = 0
                Integer.TryParse(Me.GiorniEsecutivitaTextBox.Text, giorni)
                If giorni > 0 Then
                    documento.GiorniEsecutivita = giorni
                End If
            End If
            If tipoCalcolo = 0 Then
                If Not seduta Is Nothing Then
                    If documento.GiorniEsecutivita.HasValue Then
                        documento.DataEsecutivaDa = seduta.DataConvocazione.AddDays(documento.GiorniEsecutivita)
                    Else
                        documento.DataEsecutivaDa = seduta.DataConvocazione
                    End If

                End If

            ElseIf tipoCalcolo = 1 Then
                If documento.DataAffissioneDa.HasValue Then
                    If documento.GiorniEsecutivita.HasValue Then
                        documento.DataEsecutivaDa = documento.DataAffissioneDa.Value.AddDays(documento.GiorniEsecutivita)
                    End If
                Else
                    documento.DataEsecutivaDa = Nothing
                End If

            ElseIf tipoCalcolo = 2 Then
                If documento.DataAffissioneDa.HasValue Then
                    documento.DataEsecutivaDa = documento.DataAffissioneDa.Value.AddDays(10)
                Else
                    documento.DataEsecutivaDa = Nothing
                End If

            ElseIf tipoCalcolo = 3 Then
                If documento.DataAffissioneDa.HasValue Then
                    documento.DataEsecutivaDa = documento.DataAffissioneDa.Value
                Else
                    documento.DataEsecutivaDa = Nothing
                End If

            End If


        End If

        '********************************************************************************************
        'Non gestito
        'documento.Formalizzazione = Me.FormalizzataCheckBox.Checked
        '********************************************************************************************

        'If Not String.IsNullOrEmpty(Me.IdBozzaTextBox.Text) Then
        '    documento.IdBozza = CInt(Me.IdBozzaTextBox.Text)
        'End If

        If Not String.IsNullOrEmpty(Me.PercorsoRemotoCorpoDocumentoTextBox.Text) Then
            documento.PercorsoRemotoCorpoDocumento = Me.PercorsoRemotoCorpoDocumentoTextBox.Text
        End If

        '********************************************************************************************
        'OGGETTI COLLEGATI
        '********************************************************************************************
        documento.Seduta = seduta

        documento.Classificazioni = Me.Classificazioni
        documento.ImpegniSpesa = Me.ImpegniSpesa
        documento.Accertamenti = Me.Accertamenti
        documento.Liquidazioni = Me.Liquidazioni
        documento.Presenze = Me.Presenze
        documento.Firme = Me.Firme
        documento.Visibilita = Me.Visibilita


        documento.Riservato = Me.RiservatoCheckBox.Checked



        '-----------------------------------------------------------------------------
        'INIZIO GESTIONE AMMINISTRAZIONE TRASPARENTE
        '---------------------------------------------------------------------------->
        'luca 01/07/2020
        '' ''If Not String.IsNullOrEmpty(Me.IdSezioneTrasparenzaTextBox.Text) Then
        '' ''    Dim pubblicazioneR As New ParsecAdmin.PubblicazioneRepository
        '' ''    Dim trasparenza As New ParsecAdmin.Pubblicazione

        '' ''    If Not Me.Trasparenza Is Nothing Then
        '' ''        trasparenza.DataCreazione = Me.Trasparenza.DataCreazione
        '' ''        trasparenza.Codice = Me.Trasparenza.Codice
        '' ''        trasparenza.Stato = Me.Trasparenza.Stato
        '' ''        trasparenza.Pubblicato = Me.Trasparenza.Pubblicato
        '' ''        trasparenza.IdDocumento = Me.Trasparenza.IdDocumento
        '' ''        trasparenza.CampoRicerca = Me.Trasparenza.CampoRicerca
        '' ''    Else
        '' ''        trasparenza.DataCreazione = Date.Now
        '' ''        trasparenza.Codice = pubblicazioneR.GetNuovoCodice()
        '' ''        trasparenza.Stato = "S"
        '' ''        trasparenza.Pubblicato = False
        '' ''        trasparenza.IdDocumento = Nothing
        '' ''    End If

        '' ''    trasparenza.IdSezione = CInt(Me.IdSezioneTrasparenzaTextBox.Text)

        '' ''    If String.IsNullOrEmpty(Me.comboboxSottoSezione.Text) Then
        '' ''        trasparenza.IdSottoSezione = -1
        '' ''    Else
        '' ''        trasparenza.IdSottoSezione = comboboxSottoSezione.SelectedValue
        '' ''    End If

        '' ''    trasparenza.DataInizioPubblicazione = Me.DataInizioPubblicazioneTextBox.SelectedDate
        '' ''    trasparenza.DataFinePubblicazione = Me.DataFinePubblicazioneTextBox.SelectedDate

        '' ''    trasparenza.IdUtente = utenteCorrente.Id

        '' ''    trasparenza.DataOperazione = Date.Now

        '' ''    trasparenza.IdModulo = ParsecAdmin.TipoModulo.ATT

        '' ''    trasparenza.TipologiaSezioneTrasparente = CType(trasparenza.IdSezione, ParsecAdmin.TipologiaSezioneTrasparente)

        '' ''    Select Case trasparenza.TipologiaSezioneTrasparente

        '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
        '' ''            For Each proc In AttiConcessione
        '' ''                trasparenza.ProcedureAffidamento.Add(proc)
        '' ''                proc.Allegati = Me.AllegatiPubblicazione
        '' ''            Next

        '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti

        '' ''            Dim incarico As New ParsecAdmin.IncaricoDipendente

        '' ''            incarico.Beneficiario = Me.BeneficiarioIncaricoComboBox.Text

        '' ''            If Not String.IsNullOrEmpty(Me.BeneficiarioIncaricoComboBox.SelectedValue) Then
        '' ''                Dim idRubrica As Integer = CInt(Me.BeneficiarioIncaricoComboBox.SelectedValue)
        '' ''                Dim elemento = Me.GetElementoRubrica(idRubrica)
        '' ''                If Not elemento Is Nothing Then
        '' ''                    incarico.Beneficiario = elemento.Denominazione
        '' ''                    incarico.IdBeneficiario = idRubrica
        '' ''                End If
        '' ''            End If

        '' ''            incarico.Oggetto = Me.OggettoIncaricoDipendenteTextBox.Text

        '' ''            incarico.Compenso = Me.CompensoIncaricoDipendenteTextBox.Value
        '' ''            incarico.DataInizio = Me.DataInizioIncaricoDipendenteTextBox.SelectedDate
        '' ''            incarico.DataFine = Me.DataFineIncaricoDipendenteTextBox.SelectedDate
        '' ''            incarico.Ragione = Me.RagioneIncaricoDipendenteTextBox.Text

        '' ''            '****************************************************************************************************************
        '' ''            'RECUPERO IL CAMPO IdPubblicazioneWS 
        '' ''            '****************************************************************************************************************
        '' ''            If Not Me.Trasparenza Is Nothing Then
        '' ''                If Not Me.Trasparenza.ProcedureAffidamento Is Nothing Then
        '' ''                    Dim incaricoDipendentePrecedente As ParsecAdmin.IncaricoDipendente = Me.Trasparenza.ProcedureAffidamento.FirstOrDefault
        '' ''                    If Not incaricoDipendentePrecedente Is Nothing Then
        '' ''                        If incaricoDipendentePrecedente.IdPubblicazioneWS.HasValue Then
        '' ''                            incarico.IdPubblicazioneWS = incaricoDipendentePrecedente.IdPubblicazioneWS
        '' ''                        End If
        '' ''                    End If
        '' ''                End If
        '' ''            End If

        '' ''            '****************************************************************************************************************

        '' ''            incarico.Allegati = Me.AllegatiPubblicazione

        '' ''            trasparenza.ProcedureAffidamento.Add(incarico)



        '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti

        '' ''            Dim bandoGara As New ParsecAdmin.BandoGara

        '' ''            bandoGara.Oggetto = Me.OggettoBandoGaraTextBox.Text.Trim
        '' ''            bandoGara.Partecipanti = String.Join(";", Me.PartecipantiListBox.Items.Select(Function(c) c.Value))
        '' ''            bandoGara.Aggiudicatario = String.Join(";", Me.AggiudicatariListBox.Items.Select(Function(c) c.Value))
        '' ''            bandoGara.Cig = Me.CigBandoGaraTextBox.Text

        '' ''            If Me.ImportoAggiudicazioneTextBox.Value.HasValue Then
        '' ''                bandoGara.ImportoAggiudicazione = Me.ImportoAggiudicazioneTextBox.Value
        '' ''            End If

        '' ''            If Me.ImportoLiquidatoTextBox.Value.HasValue Then
        '' ''                bandoGara.ImportoLiquidato = Me.ImportoLiquidatoTextBox.Value
        '' ''            End If

        '' ''            If Me.NumeroOfferentiTextBox.Value.HasValue Then
        '' ''                bandoGara.NumeroOfferenti = CInt(Me.NumeroOfferentiTextBox.Value)
        '' ''            End If

        '' ''            bandoGara.DataInizioOpera = Me.DataInizioLavoroTextBox.SelectedDate
        '' ''            bandoGara.DataFineOpera = Me.DataFineLavoroTextBox.SelectedDate
        '' ''            bandoGara.TipologiaSceltaContraente = Me.TipologiaSceltaComboBox.Text

        '' ''            If Not String.IsNullOrEmpty(Me.DenominazioneStrutturaProponenteTextBox.Text.Trim) Then
        '' ''                bandoGara.StrutturaProponente = Me.DenominazioneStrutturaProponenteTextBox.Text.Trim
        '' ''            End If

        '' ''            If Not String.IsNullOrEmpty(Me.CodiceFiscaleProponenteTextBox.Text.Trim) Then
        '' ''                bandoGara.CodiceFiscaleStrutturaProponente = Me.CodiceFiscaleProponenteTextBox.Text.Trim
        '' ''            End If


        '' ''            '****************************************************************************************************************
        '' ''            'RECUPERO IL CAMPO IdPubblicazioneWS 
        '' ''            '****************************************************************************************************************

        '' ''            If Not Me.Trasparenza Is Nothing Then
        '' ''                If Not Me.Trasparenza.ProcedureAffidamento Is Nothing Then
        '' ''                    Dim bandoGaraPrecedente As ParsecAdmin.BandoGara = Me.Trasparenza.ProcedureAffidamento.FirstOrDefault
        '' ''                    If Not bandoGaraPrecedente Is Nothing Then
        '' ''                        If bandoGaraPrecedente.IdPubblicazioneWS.HasValue Then
        '' ''                            bandoGara.IdPubblicazioneWS = bandoGaraPrecedente.IdPubblicazioneWS
        '' ''                        End If
        '' ''                    End If
        '' ''                End If
        '' ''            End If


        '' ''            '****************************************************************************************************************

        '' ''            If Not String.IsNullOrEmpty(Me.IdGaraContratti) Then
        '' ''                bandoGara.idGaraContratti = CInt(Me.IdGaraContratti)
        '' ''            End If


        '' ''            bandoGara.Allegati = Me.AllegatiPubblicazione

        '' ''            trasparenza.ProcedureAffidamento.Add(bandoGara)

        '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
        '' ''            Dim consulenza As New ParsecAdmin.Consulenza


        '' ''            consulenza.Beneficiario = Me.BeneficiarioConsulenzaComboBox.Text

        '' ''            If Not String.IsNullOrEmpty(Me.BeneficiarioConsulenzaComboBox.SelectedValue) Then
        '' ''                Dim idRubrica As Integer = CInt(Me.BeneficiarioConsulenzaComboBox.SelectedValue)
        '' ''                Dim elemento = Me.GetElementoRubrica(idRubrica)
        '' ''                If Not elemento Is Nothing Then
        '' ''                    consulenza.Beneficiario = elemento.Denominazione
        '' ''                    consulenza.IdBeneficiario = idRubrica
        '' ''                End If
        '' ''            End If

        '' ''            consulenza.Oggetto = Me.DenominazioneConsulenzaTextBox.Text
        '' ''            consulenza.RagioneIncarico = Me.RagioneIncaricoConsulenzaTextBox.Text
        '' ''            consulenza.Compenso = Me.CompensoConsulenzaTextBox.Text
        '' ''            consulenza.CompensoVariabile = Me.VariabileCompensoConsulenzaTextBox.Text
        '' ''            consulenza.AltreCariche = Me.altreCaricheTextBox.Text
        '' ''            consulenza.DatiAltriIncarichi = Me.altriIncarichiTextBox.Text
        '' ''            consulenza.DatiAltreAttivitàProfessionali = Me.altreAttivitaProfessionaliTextBox.Text

        '' ''            consulenza.NumeroDetermina = Me.NumeroAttoIncaricoConsulenzaCollaborazioneTextBox.Value
        '' ''            consulenza.DataDetermina = Me.DataAttoIncaricoConsulenzaCollaborazioneTextBox.SelectedDate

        '' ''            If Not Me.DataInizioIncaricoConsulenzaTextBox.SelectedDate Is Nothing Then
        '' ''                consulenza.DataInizioIncaricoConsulenza = Me.DataInizioIncaricoConsulenzaTextBox.SelectedDate
        '' ''            End If
        '' ''            If Not Me.DataFineIncaricoConsulenzaTextBox.SelectedDate Is Nothing Then
        '' ''                consulenza.DataFineIncaricoConsulenza = Me.DataFineIncaricoConsulenzaTextBox.SelectedDate
        '' ''            End If

        '' ''            consulenza.UrlCv = CurriculumAllegatoLinkButton.Text
        '' ''            consulenza.CurriculumTemp = NomeFileCurriculumLabel.Text

        '' ''            consulenza.UrlAttestazioneInsussistenzaConflittoInteressi = InconsistenzaAllegatoLinkButton.Text
        '' ''            consulenza.InsussistenzaTemp = NomeFileInsussistenzaLabel.Text
        '' ''            consulenza.Path = "\" & Now.Year & "\"

        '' ''            '****************************************************************************************************************
        '' ''            'RECUPERO IL CAMPO IdPubblicazioneWS 
        '' ''            '****************************************************************************************************************

        '' ''            If Not Me.Trasparenza Is Nothing Then
        '' ''                If Not Me.Trasparenza.ProcedureAffidamento Is Nothing Then
        '' ''                    Dim consulenzaPrecedente As ParsecAdmin.Consulenza = Me.Trasparenza.ProcedureAffidamento.FirstOrDefault
        '' ''                    If Not consulenzaPrecedente Is Nothing Then
        '' ''                        If consulenzaPrecedente.IdPubblicazioneWS.HasValue Then
        '' ''                            consulenza.IdPubblicazioneWS = consulenzaPrecedente.IdPubblicazioneWS
        '' ''                        End If
        '' ''                    End If
        '' ''                End If
        '' ''            End If


        '' ''            '****************************************************************************************************************

        '' ''            consulenza.Allegati = Me.AllegatiPubblicazione

        '' ''            trasparenza.ProcedureAffidamento.Add(consulenza)


        '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale

        '' ''            Dim nuovaIncarico As New ParsecAdmin.IncaricoAmministrativoDirigenziale

        '' ''            nuovaIncarico.titolare = Me.TitolareIncaricoComboBox.Text

        '' ''            If Not String.IsNullOrEmpty(Me.TitolareIncaricoComboBox.SelectedValue) Then
        '' ''                Dim idRubrica As Integer = CInt(Me.TitolareIncaricoComboBox.SelectedValue)
        '' ''                Dim elemento = Me.GetElementoRubrica(idRubrica)
        '' ''                If Not elemento Is Nothing Then
        '' ''                    nuovaIncarico.titolare = elemento.Denominazione
        '' ''                    nuovaIncarico.idTitolare = idRubrica
        '' ''                End If
        '' ''            End If


        '' ''            nuovaIncarico.denominazione = Me.DenominazioneIncaricoAmministrativoTextBox.Text
        '' ''            nuovaIncarico.compenso = Me.CompensoIncaricoAmministrativoTextBox.Text
        '' ''            nuovaIncarico.compensoVariabile = Me.CompensiVariabiliIncaricoAmministrativoTextBox.Text
        '' ''            nuovaIncarico.dal = Me.DataInizioIncaricoAmministrativoTextBox.SelectedDate
        '' ''            nuovaIncarico.al = Me.DataFineIncaricoAmministrativoTextBox.SelectedDate
        '' ''            nuovaIncarico.ragioneIncarico = Me.RagioneIncaricoAmministrativoTextBox.Text
        '' ''            nuovaIncarico.altreCariche = Me.CaricheIncaricoAmministrativoTextBox.Text
        '' ''            nuovaIncarico.altriIncarichi = Me.AltriIncarichiIncaricoAmministrativoTextBox.Text
        '' ''            nuovaIncarico.altreAttivitàProfessionali = Me.AltreAttivitaIncaricoAmministrativoTextBox.Text
        '' ''            nuovaIncarico.numerodet = Me.NumeroAttoIncaricoAmministrativoTextBox.Value
        '' ''            nuovaIncarico.datadet = Me.DataAttoIncaricoAmministrativoTextBox.SelectedDate

        '' ''            nuovaIncarico.urlcv = CurriculumIncaricoAmministrativoLinkButton.Text
        '' ''            nuovaIncarico.CurriculumTemp = NomeFileCurriculumIncaricoAmministrativoLabel.Text

        '' ''            nuovaIncarico.urlAttestazioneInconferibilita = InconferibilitaIncaricoAmministrativoLinkButton.Text
        '' ''            nuovaIncarico.InconferibilitaTemp = NomeFileInconferibilitaIncaricoAmministrativoLabel.Text

        '' ''            nuovaIncarico.urlAttestazioneIncompatilita = IncompatibilitaIncaricoAmministrativoLinkButton.Text
        '' ''            nuovaIncarico.IncompatilitaTemp = NomeFileIncompatibilitaIncaricoAmministrativoLabel.Text

        '' ''            nuovaIncarico.path = "\" & Now.Year & "\"


        '' ''            '****************************************************************************************************************
        '' ''            'RECUPERO IL CAMPO IdPubblicazioneWS 
        '' ''            '****************************************************************************************************************

        '' ''            If Not Me.Trasparenza Is Nothing Then
        '' ''                If Not Me.Trasparenza.ProcedureAffidamento Is Nothing Then
        '' ''                    Dim pubblicazionePrecedente As ParsecAdmin.IncaricoAmministrativoDirigenziale = Me.Trasparenza.ProcedureAffidamento.FirstOrDefault
        '' ''                    If Not pubblicazionePrecedente Is Nothing Then
        '' ''                        If pubblicazionePrecedente.idPubblicazioneWS.HasValue Then
        '' ''                            nuovaIncarico.idPubblicazioneWS = pubblicazionePrecedente.idPubblicazioneWS
        '' ''                        End If
        '' ''                    End If
        '' ''                End If
        '' ''            End If
        '' ''            '****************************************************************************************************************

        '' ''            nuovaIncarico.Allegati = Me.AllegatiPubblicazione

        '' ''            trasparenza.ProcedureAffidamento.Add(nuovaIncarico)

        '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso


        '' ''            Dim bandoConcorso As New ParsecAdmin.BandoConcorso

        '' ''            bandoConcorso.tipoAssunzione = Me.BandoConcorsoTipoAssunzioneTextBox.Text
        '' ''            bandoConcorso.oggetto = Me.BandoConcorsoOggettoTextBox.Text
        '' ''            bandoConcorso.profilo = Me.BandoConcorsoProfiloTextBox.Text
        '' ''            bandoConcorso.categoria = Me.BandoConcorsoCategoriaTextBox.Text
        '' ''            bandoConcorso.spesa = Me.BandoConcorsoSpesaTextBox.Value
        '' ''            bandoConcorso.numeroDipendentiAssunti = Me.BandoConcorsoNumeroDipendentiAssuntiTextBox.Value
        '' ''            bandoConcorso.estremiPrincipaliDocumenti = Me.BandoConcorsoEstremiDocumentiTextBox.Text

        '' ''            '****************************************************************************************************************
        '' ''            'RECUPERO IL CAMPO IdPubblicazioneWS 
        '' ''            '****************************************************************************************************************

        '' ''            If Not Me.Trasparenza Is Nothing Then
        '' ''                If Not Me.Trasparenza.ProcedureAffidamento Is Nothing Then
        '' ''                    Dim pubblicazionePrecedente As ParsecAdmin.BandoConcorso = Me.Trasparenza.ProcedureAffidamento.FirstOrDefault
        '' ''                    If Not pubblicazionePrecedente Is Nothing Then
        '' ''                        If pubblicazionePrecedente.idPubblicazioneWS.HasValue Then
        '' ''                            bandoConcorso.idPubblicazioneWS = pubblicazionePrecedente.idPubblicazioneWS
        '' ''                        End If
        '' ''                    End If
        '' ''                End If
        '' ''            End If
        '' ''            '****************************************************************************************************************

        '' ''            bandoConcorso.Allegati = Me.AllegatiPubblicazione

        '' ''            trasparenza.ProcedureAffidamento.Add(bandoConcorso)


        '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate

        '' ''            Dim nuovoEnteControllato As New ParsecAdmin.EnteControllato

        '' ''            nuovoEnteControllato.ragioneSociale = Me.RagioneSocialeEnteControllatoCombobox.Text

        '' ''            If Not String.IsNullOrEmpty(Me.RagioneSocialeEnteControllatoCombobox.SelectedValue) Then
        '' ''                Dim idRubrica As Integer = CInt(Me.RagioneSocialeEnteControllatoCombobox.SelectedValue)
        '' ''                Dim elemento = Me.GetElementoRubrica(idRubrica)
        '' ''                If Not elemento Is Nothing Then
        '' ''                    nuovoEnteControllato.ragioneSociale = elemento.Denominazione
        '' ''                    nuovoEnteControllato.idEnteRubrica = idRubrica
        '' ''                 End If
        '' ''            End If

        '' ''            nuovoEnteControllato.attivitaFavoreAmministrazione = Me.AttivitaFavoreAmministrazioneEnteControllatoTextBox.Text
        '' ''            nuovoEnteControllato.attivitaServizioPubblico = Me.AttivitaServizioPubblicoEnteControllatoTextBox.Text
        '' ''            nuovoEnteControllato.durataImpegno = Me.DurataImpegnoEnteControllatoTextbox.Text
        '' ''            nuovoEnteControllato.funzioniAttribuite = Me.FunzioniAttribuiteEnteControllatoTextBox.Text

        '' ''            If Me.IdSezioneTrasparenzaTextBox.Text = ParsecPub.TipologiaSezioneTrasparente.EntiPubbliciVigilati Then
        '' ''                nuovoEnteControllato.idTipoEnteControllato = 2
        '' ''            ElseIf Me.IdSezioneTrasparenzaTextBox.Text = ParsecPub.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati Then
        '' ''                nuovoEnteControllato.idTipoEnteControllato = 3
        '' ''            Else
        '' ''                nuovoEnteControllato.idTipoEnteControllato = 1
        '' ''            End If

        '' ''            nuovoEnteControllato.misuraPartecipazione = Me.MisuraPartecipazioneEnteControllatoTextBox.Text
        '' ''            nuovoEnteControllato.urlSitoIstituzionale = Me.UrlSitoIstituzionaleEnteControllatoTextBox.Text
        '' ''            nuovoEnteControllato.onereComplessivo = Me.OnereComplessivoEnteControllatoTextBox.Value


        '' ''            nuovoEnteControllato.numeroRappresentanti = Me.NumeroRappresentantiEnteControllatoTextBox.Value

        '' ''            nuovoEnteControllato.urlDichiarazioneIncompatibilita = IncompatibilitaAllegatoLinkButton.Text
        '' ''            nuovoEnteControllato.IncompatibilitaTemp = NomeFileIncompatibilitaLabel.Text

        '' ''            nuovoEnteControllato.urlDichiarazioneInconferibilita = InconferibilitaAllegatoLinkButton.Text
        '' ''            nuovoEnteControllato.InconferibilitaTemp = NomeFileInconferibilitaLabel.Text
        '' ''            nuovoEnteControllato.path = "\" & Now.Year & "\"

        '' ''            nuovoEnteControllato.entita = Me.EntitaSocietaPartecipateTextBox.Value

        '' ''            'Trattamento Economico Rappresentanti

        '' ''            Dim trattamentoRappresentanti As String = String.Empty
        '' ''            If Me.TrattamentiEconomiciRappresentante.Count > 0 Then
        '' ''                trattamentoRappresentanti = String.Join(";", Me.TrattamentiEconomiciRappresentante.Select(Function(c) c.Descrizione))
        '' ''            End If
        '' ''            nuovoEnteControllato.trattamentoEconomicoRappresentanti = trattamentoRappresentanti


        '' ''            'Trattamento Economico Incarichi
        '' ''            Dim trattamentoIncarichi As String = String.Empty
        '' ''            If Me.TrattamentiEconomiciIncaricoAmministratore.Count > 0 Then
        '' ''                trattamentoIncarichi = String.Join(";", Me.TrattamentiEconomiciIncaricoAmministratore.Select(Function(c) c.Incarico & "#" & c.Descrizione))
        '' ''            End If
        '' ''            nuovoEnteControllato.incarichiAmministratoreTrattamentoEconomico = trattamentoIncarichi


        '' ''            Dim nuovoBilancio As ParsecAdmin.BilancioEsercizioEnteControllato
        '' ''            If Me.BilanciEnteControllato.Count > 0 Then
        '' ''                For Each bilancio In Me.BilanciEnteControllato
        '' ''                    nuovoBilancio = New ParsecAdmin.BilancioEsercizioEnteControllato
        '' ''                    nuovoBilancio.anno = bilancio.anno
        '' ''                    nuovoBilancio.risultato = bilancio.risultato
        '' ''                    nuovoEnteControllato.Bilanci.Add(nuovoBilancio)
        '' ''                Next
        '' ''            End If


        '' ''            '****************************************************************************************************************
        '' ''            'RECUPERO IL CAMPO IdPubblicazioneWS 
        '' ''            '****************************************************************************************************************


        '' ''            If Not Me.Trasparenza Is Nothing Then
        '' ''                If Not Me.Trasparenza.ProcedureAffidamento Is Nothing Then
        '' ''                    Dim pubblicazionePrecedente As ParsecAdmin.EnteControllato = Me.Trasparenza.ProcedureAffidamento.FirstOrDefault
        '' ''                    If Not pubblicazionePrecedente Is Nothing Then
        '' ''                        If pubblicazionePrecedente.idPubblicazioneWS.HasValue Then
        '' ''                            nuovoEnteControllato.idPubblicazioneWS = pubblicazionePrecedente.idPubblicazioneWS
        '' ''                        End If
        '' ''                    End If
        '' ''                End If
        '' ''            End If
        '' ''            '****************************************************************************************************************

        '' ''            nuovoEnteControllato.Allegati = Me.AllegatiPubblicazione


        '' ''            trasparenza.ProcedureAffidamento.Add(nuovoEnteControllato)


        '' ''        Case Else    'PUBBLICAZIONE GENERICA

        '' ''            Dim pubblicazioneGenerica As New ParsecAdmin.PubblicazioneGenerica

        '' ''            If Not String.IsNullOrEmpty(Me.PubblicazioneContenutoTextBox.Text.Trim) Then
        '' ''                pubblicazioneGenerica.contenuto = Me.PubblicazioneContenutoTextBox.Text.Trim
        '' ''            End If

        '' ''            If Not String.IsNullOrEmpty(Me.PubblicazioneTitoloTextBox.Text.Trim) Then
        '' ''                pubblicazioneGenerica.titolo = Me.PubblicazioneTitoloTextBox.Text.Trim
        '' ''            End If

        '' ''            If Not String.IsNullOrEmpty(Me.PubblicazioneNumeroTextBox.Text.Trim) Then
        '' ''                pubblicazioneGenerica.numero = CInt(Me.PubblicazioneNumeroTextBox.Value)
        '' ''            End If

        '' ''            pubblicazioneGenerica.dataInizioRiferimento = Me.PubblicazioneInizioRiferimentoTextBox.SelectedDate
        '' ''            pubblicazioneGenerica.dataFineriferimento = Me.PubblicazioneFineRiferimentoTextBox.SelectedDate

        '' ''            '****************************************************************************************************************
        '' ''            'RECUPERO IL CAMPO IdPubblicazioneWS 
        '' ''            '****************************************************************************************************************

        '' ''            If Not Me.Trasparenza Is Nothing Then
        '' ''                If Not Me.Trasparenza.ProcedureAffidamento Is Nothing Then
        '' ''                    Dim pubblicazioneGenericaPrecedente As ParsecAdmin.PubblicazioneGenerica = Me.Trasparenza.ProcedureAffidamento.FirstOrDefault
        '' ''                    If Not pubblicazioneGenericaPrecedente Is Nothing Then
        '' ''                        If pubblicazioneGenericaPrecedente.idPubblicazioneWS.HasValue Then
        '' ''                            pubblicazioneGenerica.idPubblicazioneWS = pubblicazioneGenericaPrecedente.idPubblicazioneWS
        '' ''                        End If
        '' ''                    End If
        '' ''                End If
        '' ''            End If
        '' ''            '****************************************************************************************************************

        '' ''            pubblicazioneGenerica.Allegati = Me.AllegatiPubblicazione


        '' ''            trasparenza.ProcedureAffidamento.Add(pubblicazioneGenerica)


        '' ''    End Select

        '' ''    documento.Trasparenza = trasparenza
        '' ''    pubblicazioneR.Dispose()
        '' ''End If
        '-----------------------------------------------------------------------------
        'FINE GESTIONE AMMINISTRAZIONE TRASPARENTE
        '<----------------------------------------------------------------------------|


        '********************************************************************************************
        'GESTIONE FASCICOLI
        '********************************************************************************************
        documento.Fascicoli = Me.Fascicoli


        If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Numerazione Then

            'Copio gli allegati
            documento.Allegati = New List(Of ParsecAtt.Allegato)
            Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            pathRoot = pathRoot.Remove(pathRoot.Length - 1, 1)
            Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
            Dim all As ParsecAtt.Allegato = Nothing
            For Each allegato In Me.Allegati
                Dim filename As String = String.Empty
                Dim filenameTemp As String = String.Empty
                If allegato.Id < 0 Then
                    filename = allegato.Nomefile
                    filenameTemp = allegato.NomeFileTemp
                Else
                    filename = "clone_" & allegato.Nomefile
                    filenameTemp = "clone_" & allegato.Nomefile

                    allegato.Id = -1
                    If documento.Allegati.Count > 0 Then
                        Dim allId = documento.Allegati.Min(Function(c) c.Id) - 1
                        If allId < 0 Then
                            allegato.Id = allId
                        End If
                    End If

                End If
                all = New ParsecAtt.Allegato
                all.PercorsoRoot = pathRoot
                all.PercorsoRootTemp = pathRootTemp
                all.Nomefile = filename
                all.NomeFileTemp = filenameTemp
                all.Oggetto = allegato.Oggetto
                all.Impronta = allegato.Impronta
                all.Pubblicato = allegato.Pubblicato
                all.Id = allegato.Id
                all.IdFatturaElettronica = allegato.IdFatturaElettronica


                Dim input As String = pathRoot & allegato.PercorsoRelativo & allegato.Nomefile
                Dim output As String = pathRootTemp & filenameTemp
                Me.CopiaDocumento(input, output)

                If Not String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
                    all.NomeFileFirmato = "clone_" & allegato.NomeFileFirmato
                End If

                documento.Allegati.Add(all)
            Next

        Else
            documento.Allegati = Me.Allegati
        End If
        '********************************************************************************************

        Dim proceduraCorrente = Me.TipologiaProceduraCorrente
        Dim proceduraApertura = Me.TipologiaProceduraApertura
        Dim tipologiaDocumento = Me.TipologiaDocumento

        'SE STO CREANDO UNA NUOVA PROPOSTA DI DETERMINA O SE STO NUMERANDO UNA PROPOSTA DI DETERMINA O STO MODIFICANDO (MODIFICA PER CARMIANO)
        If (proceduraCorrente = ParsecAtt.TipoProcedura.Nuovo AndAlso tipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina) OrElse (proceduraCorrente = ParsecAtt.TipoProcedura.Numerazione AndAlso tipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina) OrElse proceduraCorrente = ParsecAtt.TipoProcedura.Modifica Then
            If Me.NumeroSettoreTextBox.Enabled Then
                documento.ContatoreSettoreProposto = Me.NumeroSettoreTextBox.Value
            End If
        End If

        Return documento

    End Function


  
    Public Function GetElementoRubrica(ByVal id As Integer) As ParsecAdmin.StrutturaEsternaInfo
        Dim rubrica As New ParsecAdmin.RubricaRepository
        Dim elemento = rubrica.Where(Function(c) c.Id = id).FirstOrDefault
        rubrica.Dispose()
        Return elemento
    End Function

    Private Function AssegnaNumeroSettore() As Boolean
        Dim res As Boolean = False

        If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Numerazione OrElse Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Nuovo Then
            If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Then
                Dim parametri As New ParsecAdmin.ParametriRepository
                Dim parametroMomentoNumerazioneSettore As ParsecAdmin.Parametri = parametri.GetByName("MomentoNumerazioneSettore", ParsecAdmin.TipoModulo.ATT)
                If Not parametroMomentoNumerazioneSettore Is Nothing Then
                    Select Case parametroMomentoNumerazioneSettore.Valore
                        Case "0"
                            'LA NUMERAZIONE DEL SETTORE E' DISATTIVATA
                            ' COSA FARE SE E' DISATTIVATA MA L'ITER IMPOSTA IL PARAMETRO NUMERAZIONESETTORE?
                        Case "1"
                            'LA NUMERAZIONE DI SETTORE E' GESTITA IN FASE DI PROPOSTA


                            '*******************************************************************
                            'MODIFICA PER CARMIANO - MODIFICA NUMERAZIONE SETTORE MANUALE
                            '*******************************************************************
                            If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina Then
                                Dim parametroAbilitaModificaNumerazioneSettore As ParsecAdmin.Parametri = parametri.GetByName("AbilitaModificaNumerazioneSettore", ParsecAdmin.TipoModulo.ATT)

                                If Not parametroAbilitaModificaNumerazioneSettore Is Nothing Then

                                    If parametroAbilitaModificaNumerazioneSettore.Valore = "1" Then
                                        If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Modifica OrElse Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Numerazione Then
                                            res = True
                                        End If
                                    End If


                                End If
                            End If
                            '*******************************************************************




                            If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Nuovo Then
                                If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina Then
                                    res = True
                                End If
                            End If
                        Case "2"
                            'LA NUMERAZIONE DI SETTORE E' GESTITA DURANTE LA NUMERAZIONE (TRASFORMAZIONE PROPOSTA A DETERMINA)
                            If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Numerazione Then

                                If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina Then
                                    res = True
                                End If
                            End If
                        Case "3"
                            'LA NUMERAZIONE DEL SETTORE E' GESTITA DALL'ITER
                    End Select
                End If
            End If
        End If


        Return res
    End Function

    Private Sub Numera()
        Dim idDocumento As Integer = Me.Documento.Id

        Dim documento As ParsecAtt.Documento = Me.AggiornaModello

        documento.AssegnaNumeroSettore = Me.AssegnaNumeroSettore
        Dim documenti As New ParsecAtt.DocumentoRepository
        Try

            documenti.Documento = Nothing
            documento.DocumentoCollegato = Me.Documento
            documento.DocumentoGenerato = Nothing
            documento.CodiceProposta = Me.Documento.Codice
            documenti.TipologiaProcedura = Me.TipologiaProceduraCorrente
            documenti.Save(documento)

            'TODO GETBYID CHE CARICA TUTTI GLI OGGETTI COLLEGATI AL TERMINE DEL METODO SAVE DEL REPOSITORY
            Me.Documento = documenti.Documento


            Me.Documento.Trasparenza = documenti.GetTrasparenza(Me.Documento.Id)
            Me.Documento.Allegati = documenti.GetAllegati(Me.Documento.Id)



           

            '*******************************************************************
            'MODIFICA PER CARMIANO - MODIFICA NUMERAZIONE SETTORE MANUALE
            '*******************************************************************
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametroAbilitaModificaNumerazioneSettore As ParsecAdmin.Parametri = parametri.GetByName("AbilitaModificaNumerazioneSettore", ParsecAdmin.TipoModulo.ATT)
            If Not parametroAbilitaModificaNumerazioneSettore Is Nothing Then
                If parametroAbilitaModificaNumerazioneSettore.Valore = "1" Then
                    Dim proposta = documenti.Where(Function(c) c.Id = Me.Documento.IdPadre).FirstOrDefault
                    If Not proposta Is Nothing Then
                        proposta.ContatoreStruttura = Me.Documento.ContatoreStruttura
                        documenti.SaveChanges()
                    End If
                End If

            End If
            parametri.Dispose()
            '*******************************************************************


            '*******************************************************************
            'Gestione iter
            '*******************************************************************
            If Not Page.Request("Iter") Is Nothing Then
                Me.AggiornaIstanzaWorkflow(idDocumento, Me.Documento, 3)
                '**************************************************************************************************************
                'AGGIORNO I PARAMETRI DI PROCESSO 
                '**************************************************************************************************************
                Me.AggiornaParametriProcesso()
                '**************************************************************************************************************
            End If


            Me.RegistraScriptOpenOffice()

            If TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina Then

                Me.Documento.Modello = documenti.GetModello(Me.Documento.IdModello)
                Me.Documento.Liquidazioni = documenti.GetLiquidazioni(Me.Documento.Id)

                'If Me.Documento.Modello.Liquidazione Then
                Try

                    '**************************************************************
                    'DEPRECATA
                    '**************************************************************
                    'Me.PubblicaLiquidazione()
                    '**************************************************************

                    '*********************************************************************************************************
                    'SE STO NUMERANDO UNA PROPOSTA DI DETERMINA
                    '*********************************************************************************************************
                    'luca 01/07/2020
                    '' ''If Not Me.Documento.Trasparenza Is Nothing Then
                    '' ''    Me.PubblicaTrasparenza()

                    '' ''End If
            '*********************************************************************************************************

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message & "!", False)
        End Try
                'End If
            End If

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            documenti.Dispose()
        End Try

    End Sub

    Private Sub Classifica()
        Dim message As String = "Operazione conclusa con successo!"
        Dim classificazioni As New ParsecAtt.DocumentoClassificazioneRepository
        Try
            Me.Documento.Classificazioni = Me.Classificazioni
            classificazioni.DeleteAll(Me.Documento)
            classificazioni.SaveAll(Me.Documento)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            classificazioni.Dispose()
        End Try
    End Sub

    Private Sub ConvalidaIter()
        Dim nuovo As Boolean = Me.Documento Is Nothing
        If Me.IterAttivato Then
            If nuovo Then
                For Each firma As ParsecAtt.Firma In Me.Firme.Where(Function(c) c.Iter = True).ToList
                    Dim idRuolo As Nullable(Of Integer) = firma.IdRuolo
                    If Not idRuolo.HasValue Then
                        Throw New ApplicationException("Il ruolo della firma " & firma.Id.ToString & " - " & firma.Descrizione & " non è configurato")
                        Exit Sub
                    Else
                        Dim ruoli As New ParsecWKF.RuoloRepository
                        Dim ruolo = ruoli.GetQuery.Where(Function(c) c.Id = idRuolo).FirstOrDefault

                        If Not ruolo Is Nothing Then

                            Dim modelliIter As New ParsecWKF.ModelliRepository

                            Dim idModello As Integer = CInt(Me.ModelliComboBox.SelectedValue)
                            Dim modelli As New ParsecAtt.ModelliRepository

                            Dim idIter As Nullable(Of Integer) = modelli.Where(Function(c) c.Id = idModello).FirstOrDefault.IdIter
                            If idIter.HasValue Then

                                Dim modelloIter As ParsecWKF.Modello = modelliIter.GetById(idIter)

                                If Not modelloIter Is Nothing Then
                                    Dim actors = ParsecWKF.ModelloInfo.ReadActors(modelloIter.NomeFile)

                                    Dim attore = actors.Where(Function(c) c.Name.ToLower = ruolo.Descrizione.ToLower).FirstOrDefault
                                    If attore Is Nothing Then
                                        Throw New ApplicationException("Il ruolo " & ruolo.Descrizione & " non è configurato nell'iter " & modelloIter.NomeFile)
                                        Exit Sub
                                    End If
                                End If
                            End If

                        End If
                    End If
                Next
            End If

        End If
    End Sub

    Private Sub AggiornaParametriProcesso()
        Dim instanze As New ParsecWKF.IstanzaRepository
        Dim instanza = instanze.Where(Function(c) c.IdDocumento = Me.Documento.Id And c.IdModulo = ParsecAdmin.TipoModulo.ATT).FirstOrDefault

        If Not instanza Is Nothing Then
            Dim idProcesso = instanza.Id
            Dim parametriProcesso As New ParsecWKF.ParametriProcessoRepository
            Dim ruoli As New ParsecWKF.RuoloRepository


            Dim idModelloIter As Nullable(Of Integer) = Nothing
            If Me.Documento.Modello.Proposta Then
                idModelloIter = Me.Documento.Modello.IdIter
            Else
                Dim documenti As New ParsecAtt.DocumentoRepository
                Dim documentoCollegato = documenti.GetDocumentoCollegato(Me.Documento.IdPadre)
                If Not documentoCollegato Is Nothing Then
                    documentoCollegato.Modello = documenti.GetModello(documentoCollegato.IdModello)
                    If Not documentoCollegato.Modello Is Nothing Then
                        idModelloIter = documentoCollegato.Modello.IdIter
                    End If
                End If
                documenti.Dispose()
            End If

            If idModelloIter.HasValue Then
                Dim modelliIter As New ParsecWKF.ModelliRepository
                Dim modelloIter As ParsecWKF.Modello = modelliIter.GetById(idModelloIter)
                modelliIter.Dispose()
                Dim actors = ParsecWKF.ModelloInfo.ReadActors(modelloIter.NomeFile)

                For Each firma As ParsecAtt.Firma In Me.Documento.Firme
                    Dim idRuolo As Integer = firma.IdRuolo
                    Dim ruolo = ruoli.GetQuery.Where(Function(c) c.Id = idRuolo).FirstOrDefault
                    If Not ruolo Is Nothing Then
                        Dim attore = actors.Where(Function(c) c.Name.ToLower = ruolo.Descrizione.ToLower).FirstOrDefault
                        If Not attore Is Nothing Then
                            Dim parametroProcesso = parametriProcesso.Where(Function(c) c.Nome = attore.Name And c.IdProcesso = idProcesso).FirstOrDefault
                            If Not parametroProcesso Is Nothing Then
                                parametroProcesso.Valore = firma.IdUtente
                            End If

                            parametriProcesso.SaveChanges()
                        End If
                    End If
                Next
            End If

            ruoli.Dispose()
            parametriProcesso.Dispose()
        End If
        instanze.Dispose()
    End Sub

    Private Sub Save()

        If (HttpContext.Current.IsDebuggingEnabled) Then

#If DEBUG Then
        Try
            Me.ConvalidaIter()
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try

#End If


        End If

        Dim nuovo As Boolean = Me.Documento Is Nothing
        Dim idDocumento As Integer = 0

        If Not nuovo Then
            idDocumento = Me.Documento.Id
        End If

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento As ParsecAtt.Documento = Me.AggiornaModello



        Try
            '*******************************************************************
            'Gestione storico
            '*******************************************************************
            documenti.Documento = Me.Documento
            If Not Me.Documento Is Nothing Then
                documenti.Documento.Modello = documento.Modello
                documenti.Documento.Seduta = documento.Seduta
            End If

            documenti.TipologiaProcedura = Me.TipologiaProceduraCorrente
            documento.Rigenera = Me.Rigenera
            documento.NascondiDocumento = Me.NascondiDocumento

            documento.NomeFileCopia = Me.NomeFileCopia

            documento.AssegnaNumeroSettore = Me.AssegnaNumeroSettore


            '****************************************************************************************************************
            'DA ITER GESTIONE DEDAGROUP
            '****************************************************************************************************************
            If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then
                If Not Me.Request.QueryString("SalvaImpegniDefinitiviDedaGroup") Is Nothing Then
                    documento.SalvaImpegniDefinitiviDedaGroup = CBool(Me.Request.QueryString("SalvaImpegniDefinitiviDedaGroup"))
                End If
            End If


            '****************************************************************************************************************

            documenti.Save(documento)

            If Not String.IsNullOrEmpty(documenti.Documento.MessaggioAvvisoDedaGroup) Then
                ParsecUtility.Utility.MessageBox(documenti.Documento.MessaggioAvvisoDedaGroup, False)
            End If


            'TODO GETBYID CHE CARICA TUTTI GLI OGGETTI COLLEGATI AL TERMINE DEL METODO SAVE DEL REPOSITORY
            Me.Documento = documenti.Documento

            Me.Documento.Trasparenza = documenti.GetTrasparenza(Me.Documento.Id)
            Me.Documento.Allegati = documenti.GetAllegati(Me.Documento.Id)

            'luca 01/07/2020
            '' ''If Not Me.Documento.Trasparenza Is Nothing Then
            '' ''    Select Case Me.Documento.Trasparenza.TipologiaSezioneTrasparente
            '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
            '' ''            If Not String.IsNullOrEmpty(Me.IdGaraContratti) Then
            '' ''                Me.AggiornaImportoLiquidatoPubblicazione(CInt(Me.IdGaraContratti))
            '' ''            End If
            '' ''    End Select
            '' ''End If






            '*******************************************************************
            'Gestione Pubblicazione
            '*******************************************************************


            '*******************************************************************
            'Gestione iter
            '*******************************************************************
            If Me.IterAttivato Then


                If Not Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.CambioModello Then
                    'todo
                End If


                If Not nuovo Then
                    Me.AggiornaIstanzaWorkflow(idDocumento, Me.Documento, 3)

                    '**************************************************************************************************************
                    'AGGIORNO I PARAMETRI DI PROCESSO 
                    '**************************************************************************************************************
                    Me.AggiornaParametriProcesso()
                    '**************************************************************************************************************

                Else
                    If documento.Modello.IdIter.HasValue Then
                        Dim modelliIter As New ParsecWKF.ModelliRepository
                        Dim modelloIter As ParsecWKF.Modello = modelliIter.GetById(documento.Modello.IdIter)
                        modelliIter.Dispose()
                        If Not modelloIter Is Nothing Then
                            If Not String.IsNullOrEmpty(modelloIter.NomeFile) Then
                                Dim path As String = ParsecAdmin.WebConfigSettings.GetKey("ModelloWorkflow") & "\" & modelloIter.NomeFile
                                If IO.File.Exists(path) Then
                                    Me.CreaIstanzaAtt(Me.Documento)
                                End If
                            End If
                        End If

                    End If
                End If
            End If



            'LA PUBBLICAZIONE IN MODALITA' MODIFICA AMMINISTRATIVA NON E ' GESTITA
            'If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Pubblicazione OrElse Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.ModificaAmministrativa Then
            'luca 02/07/2020
            '' ''If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Pubblicazione Then
            '' ''    'SE E' UNA NUOVA PUBBLICAZIONE
            '' ''    If Not Me.Documento.NumeroRegistroPubblicazione.HasValue Then

            '' ''        Dim contatori As New ParsecMES.ContatoriAlboRepository
            '' ''        Dim contatore As ParsecMES.ContatoreAlbo = contatori.GetQuery.Where(Function(c) c.Corrente = True).FirstOrDefault
            '' ''        Me.AggiornaDataSourceDocumento("NUMERO_REGISTRO_PUBBLICAZIONE", contatore.NumeroRegistro.ToString)
            '' ''        contatori.Dispose()

            '' ''    End If

            '' ''End If


            Me.RegistraScriptOpenOffice()


            Dim pubblicataDaIter As Boolean = False

            '*********************************************************************************************************
            'DA ITER   ????????????
            '*********************************************************************************************************
            'luca 01/07/2020
            '' ''If Not Me.Request.QueryString("PubblicaTrasparenza") Is Nothing Then
            '' ''    Dim pubblicaTrasparenza = CBool(Me.Request.QueryString("PubblicaTrasparenza"))
            '' ''    If pubblicaTrasparenza Then
            '' ''        If Not Me.Documento.Trasparenza Is Nothing Then
            '' ''            Me.PubblicaTrasparenza()
            '' ''            pubblicataDaIter = True
            '' ''        End If
            '' ''    End If
            '' ''End If
            '*********************************************************************************************************

            '*********************************************************************************************************
            'SE STO GENERANDO UNA NUOVA DETERMINA O SE STO NUMERANDO UNA PROPOSTA DI DETERMINA
            '*********************************************************************************************************
            'luca 01/07/2020
            '' ''If Not pubblicataDaIter Then
            '' ''    If nuovo OrElse Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Numerazione Then
            '' ''        'If Me.Documento.Modello.Liquidazione AndAlso Me.Documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Then
            '' ''        If Me.Documento.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Then
            '' ''            If Not Me.Documento.Trasparenza Is Nothing Then
            '' ''                Me.PubblicaTrasparenza()
            '' ''            End If
            '' ''        End If
            '' ''    End If
            '' ''End If

            '*********************************************************************************************************

        Catch ex As Exception

            Dim mes = ex.ToString.Replace("\", "/")
            Throw New ApplicationException(ex.Message)


        Finally
            documenti.Dispose()
        End Try

    End Sub


    'Private Sub TerminaTask()
    '    'TERMINO IL TASK CORRENTE
    '    Dim statoTaskEseguito As Integer = 6
    '    Dim statoIstanzaCompletato As Integer = 3

    '    Dim tasks As New ParsecWKF.TaskRepository
    '    Dim task As ParsecWKF.Task = tasks.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.Id).FirstOrDefault
    '    task.IdStato = statoTaskEseguito
    '    task.DataEsecuzione = Now
    '    task.Destinatario = Me.TaskAttivo.IdMittente
    '    task.Operazione = "MODIFICA"
    '    task.Notificato = True
    '    tasks.SaveChanges()

    '    'Cambia lo stato del processo di workflow
    '    Dim istanze As New ParsecWKF.IstanzaRepository
    '    Dim istanza As ParsecWKF.Istanza = istanze.GetQuery.Where(Function(c) c.Id = Me.TaskAttivo.IdIstanza).FirstOrDefault
    '    istanza.IdStato = statoIstanzaCompletato
    '    istanza.DataEsecuzione = Now
    '    istanze.SaveChanges()

    '    Dim nuovoTask As New ParsecWKF.Task
    '    nuovoTask.IdIstanza = Me.TaskAttivo.IdIstanza
    '    nuovoTask.Nome = task.Corrente
    '    nuovoTask.Corrente = "FINE"
    '    nuovoTask.Successivo = String.Empty
    '    nuovoTask.Destinatario = Me.TaskAttivo.IdMittente
    '    nuovoTask.Mittente = Me.TaskAttivo.IdMittente
    '    nuovoTask.TaskPadre = task.Id
    '    nuovoTask.DataEsecuzione = Now
    '    nuovoTask.DataInizio = task.DataInizio
    '    nuovoTask.DataFine = task.DataFine
    '    nuovoTask.IdStato = statoTaskEseguito
    '    nuovoTask.Operazione = "FINE"
    '    nuovoTask.Notificato = True
    '    nuovoTask.Note = task.Note
    '    nuovoTask.IdUtenteOperazione = utenteCollegato.Id
    '    tasks.Add(nuovoTask)
    '    tasks.SaveChanges()
    'End Sub

    'Private Sub AggiornaDataSourceDocumento(ByVal nomeCampo As String, ByVal valoreCampo As String)
    '    Dim dataSource As New DataSet
    '    Dim enc As System.Text.Encoding = System.Text.Encoding.UTF8

    '    Dim bytes As Byte() = Nothing
    '    'bytes = IO.File.ReadAllBytes(Me.Documento.DataSourceFilename)
    '    'Dim dataStream As New IO.MemoryStream(bytes)

    '    Dim dataStream As New IO.MemoryStream(enc.GetBytes(Me.Documento.DataSource))

    '    dataSource.ReadXml(dataStream)
    '    dataStream.Close()

    '    For Each row In dataSource.Tables("DatiCampiUtente").Rows
    '        If row(0) = nomeCampo Then
    '            row(1) = valoreCampo
    '            Exit For
    '        End If
    '    Next
    '    dataSource.AcceptChanges()

    '    Dim ms As New IO.MemoryStream
    '    dataSource.WriteXml(ms)
    '    ms.Position = 0
    '    bytes = ms.ToArray
    '    ms.Close()

    '    'SCRIVO IL FILE CHE CONTIENE IL DATASOURCE (AGGIORNO)
    '    'IO.File.WriteAllBytes(Me.Documento.DataSourceFilename, bytes)

    '    'NON E' NECESSARIO PERCHE' IL RIFERIMENTO DEL NOME DEL FILE CHE CONTIENE I DATI NON E' CAMBIATO
    '    Me.Documento.DataSource = enc.GetString(bytes).Replace(Chr(13), "").Replace(Chr(34), "'").Replace(Chr(10), "")
    'End Sub

    Private Sub AggiornaDataSourceDocumento(ByVal nomeCampo As String, ByVal valoreCampo As String)

        Dim parametriApplicazione As New ParsecAdmin.ParametriRepository
        Dim modalitaGenerazioneDocumentoOdf = parametriApplicazione.GetByName("ModalitaGenerazioneDocumentoOdf", ParsecAdmin.TipoModulo.ATT)
        parametriApplicazione.Dispose()

        Dim dataSource As New DataSet
        Dim enc As System.Text.Encoding = System.Text.Encoding.UTF8

        Dim bytes As Byte() = Nothing
        Dim dataStream As IO.MemoryStream = Nothing

        Dim modalitaClassica As Boolean = (modalitaGenerazioneDocumentoOdf Is Nothing OrElse modalitaGenerazioneDocumentoOdf.Valore = "0")

        If modalitaClassica Then
            dataStream = New IO.MemoryStream(enc.GetBytes(Me.Documento.DataSource))
        Else
            bytes = IO.File.ReadAllBytes(Me.Documento.DataSourceFilename)
            dataStream = New IO.MemoryStream(bytes)
        End If

        dataSource.ReadXml(dataStream)
        dataStream.Close()

        For Each row In dataSource.Tables("DatiCampiUtente").Rows
            If row(0) = nomeCampo Then
                row(1) = valoreCampo
                Exit For
            End If
        Next
        dataSource.AcceptChanges()

        Dim ms As New IO.MemoryStream
        dataSource.WriteXml(ms)
        ms.Position = 0
        bytes = ms.ToArray
        ms.Close()

        If modalitaClassica Then
            'NON E' NECESSARIO PERCHE' IL RIFERIMENTO DEL NOME DEL FILE CHE CONTIENE I DATI NON E' CAMBIATO
            Me.Documento.DataSource = enc.GetString(bytes).Replace(Chr(13), "").Replace(Chr(34), "'").Replace(Chr(10), "")
        Else

            'SCRIVO IL FILE CHE CONTIENE IL DATASOURCE (AGGIORNO)
            IO.File.WriteAllBytes(Me.Documento.DataSourceFilename, bytes)
        End If

    End Sub



    'Da testare
    'luca 01/07/2020
    '' ''Public Sub PubblicaTrasparenza()
    '' ''    Dim pubblicazioniRepository As New ParsecPub.PubblicazioneRepository
    '' ''    Dim hashTable As New Dictionary(Of Integer, ParsecWebServices.PubblicazioneOutputWS)
    '' ''    Dim pubblicazioneOnline As Boolean = True
    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    Dim pubblicazioniR As New ParsecAdmin.PubblicazioneRepository
    '' ''    Dim pubblicazioneDaAggiornare As ParsecAdmin.Pubblicazione = pubblicazioniR.GetQuery.Where(Function(c) c.Id = Documento.Trasparenza.Id).FirstOrDefault

    '' ''    Dim data As DateTime = Now
    '' ''    pubblicazioneDaAggiornare.DataOperazione = data
    '' ''    pubblicazioneDaAggiornare.DataInizioPubblicazione = Me.Documento.Trasparenza.DataInizioPubblicazione
    '' ''    pubblicazioneDaAggiornare.DataFinePubblicazione = Me.Documento.Trasparenza.DataFinePubblicazione
    '' ''    pubblicazioneDaAggiornare.IdUtente = utenteCollegato.Id
    '' ''    pubblicazioneDaAggiornare.Stato = Nothing

    '' ''    Select Case Me.Documento.Trasparenza.TipologiaSezioneTrasparente
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''            Dim info As String = String.Empty
    '' ''            For Each procedura In Me.Documento.Trasparenza.ProcedureAffidamento
    '' ''                Dim attoConcessione As ParsecAdmin.AttoConcessione = procedura
    '' ''                info = info & attoConcessione.Beneficiario & " - importo: " & attoConcessione.Importo & ";" & vbCrLf
    '' ''            Next
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = info

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''            Dim consulenza As ParsecAdmin.Consulenza = Me.Documento.Trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = consulenza.Beneficiario & " - " & consulenza.Oggetto & " dal " & consulenza.DataInizioIncaricoConsulenza & " al " & consulenza.DataFineIncaricoConsulenza

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''            Dim bandoGara As ParsecAdmin.BandoGara = Me.Documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = "CIG " & bandoGara.Cig & " - Oggetto: " & bandoGara.Oggetto & " - Aggiudicatari: " & bandoGara.Aggiudicatario

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''            Dim incarico As ParsecAdmin.IncaricoDipendente = Me.Documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = incarico.Beneficiario & " - Oggetto: " & incarico.Oggetto & " - importo: " & incarico.Compenso

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale

    '' ''            Dim incarico As ParsecAdmin.IncaricoAmministrativoDirigenziale = Me.Documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = incarico.titolare & " - " & incarico.denominazione & " dal " & incarico.dal & " al " & incarico.al


    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso

    '' ''            Dim bando As ParsecAdmin.BandoConcorso = Me.Documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = bando.oggetto


    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate

    '' ''            Dim ente As ParsecAdmin.EnteControllato = Me.Documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = ente.ragioneSociale


    '' ''        Case Else    'PUBBLICAZIONE GENERICA

    '' ''            Dim pubblicazioneGenerica As ParsecAdmin.PubblicazioneGenerica = Me.Documento.Trasparenza.ProcedureAffidamento(0)
    '' ''            pubblicazioneDaAggiornare.CampoRicerca = pubblicazioneGenerica.titolo & " - " & pubblicazioneGenerica.contenuto

    '' ''    End Select

    '' ''    pubblicazioniR.SaveChanges()

    '' ''    Dim pubblicazionePub As ParsecPub.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazioneDaAggiornare.Id)





    '' ''    Select Case Me.Documento.Trasparenza.TipologiaSezioneTrasparente
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''            Dim attoConcessionePubblicazione As New ParsecWebServices.AttoConcessione
    '' ''            hashTable = attoConcessionePubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)
    '' ''            Me.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazionePub.Id)
    '' ''            pubblicazioniRepository.Dispose()

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''            'caso bando gara

    '' ''            Dim bandoGaraPubblicazione As New ParsecWebServices.BandoGara
    '' ''            hashTable = bandoGaraPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)
    '' ''            Me.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazionePub.Id)
    '' ''            pubblicazioniRepository.Dispose()

    '' ''            'luca 01/07/2020
    '' ''            '' ''If Not String.IsNullOrEmpty(Me.IdGaraContratti) Then
    '' ''            '' ''    Me.AggiornaImportoLiquidatoPubblicazione(CInt(Me.IdGaraContratti))
    '' ''            '' ''End If


    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''            Dim incaricoPubblicazione As New ParsecWebServices.IncaricoDipendente
    '' ''            hashTable = incaricoPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)
    '' ''            Me.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazionePub.Id)
    '' ''            pubblicazioniRepository.Dispose()

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''            Dim cunsulenzaPubblicazione As New ParsecWebServices.ConsulenzaCollaborazione
    '' ''            hashTable = cunsulenzaPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)
    '' ''            Me.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazionePub.Id)
    '' ''            pubblicazioniRepository.Dispose()

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale

    '' ''            Dim incaricoPubblicazione As New ParsecWebServices.IncaricoDirigenzialeAmministrativo
    '' ''            hashTable = incaricoPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)
    '' ''            Me.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazionePub.Id)
    '' ''            pubblicazioniRepository.Dispose()

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso

    '' ''            Dim bandoConcorsoPubblicazione As New ParsecWebServices.BandoConcorso
    '' ''            hashTable = bandoConcorsoPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)
    '' ''            Me.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazionePub.Id)
    '' ''            pubblicazioniRepository.Dispose()

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate

    '' ''            Dim entePubblicazione As New ParsecWebServices.EnteControllato
    '' ''            hashTable = entePubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)
    '' ''            Me.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazionePub.Id)
    '' ''            pubblicazioniRepository.Dispose()

    '' ''        Case Else    'PUBBLICAZIONE GENERICA

    '' ''            Dim genericaPubblicazione As New ParsecWebServices.PubblicazioneGenerica
    '' ''            hashTable = genericaPubblicazione.Pubblica(pubblicazionePub)
    '' ''            aggiornaParsecPubRepository(pubblicazionePub, hashTable)
    '' ''            Me.Pubblicazione = pubblicazioniRepository.GetFullById(pubblicazionePub.Id)
    '' ''            pubblicazioniRepository.Dispose()

    '' ''    End Select


    '' ''    If Not pubblicazioneDaAggiornare Is Nothing Then
    '' ''        If pubblicazioneOnline Then
    '' ''            'Dim pubblicazioneDaAggiornare As ParsecAdmin.Pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.Id = pubblicazione.Id).FirstOrDefault
    '' ''            pubblicazioneDaAggiornare.Pubblicato = True
    '' ''            pubblicazioneDaAggiornare.Stato = Nothing
    '' ''            pubblicazioniR.SaveChanges()
    '' ''        Else
    '' ''            pubblicazioneDaAggiornare.Stato = "S"
    '' ''            pubblicazioniR.SaveChanges()
    '' ''        End If
    '' ''    End If

    '' ''    pubblicazioniR.Dispose()

    '' ''End Sub

    'metodo per aggiornare il database di parsecpub
    'luca 01/07/2020
    '' ''Private Sub aggiornaParsecPubRepository(ByVal pubblicazione As ParsecPub.Pubblicazione, ByVal hashTableOutputWS As Dictionary(Of Integer, ParsecWebServices.PubblicazioneOutputWS))
    '' ''    Try

    '' ''        Select Case pubblicazione.idSezione
    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''                'caso atto concessione
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                Dim attoRep As New ParsecPub.AttoConcessioneRepository
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.AttoConcessione = attoRep.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    attoRep.SaveChanges()
    '' ''                Next
    '' ''                attoRep.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''                'caso bando gara
    '' ''                Dim bandogaraRep As New ParsecPub.BandoGaraRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.BandoGara = bandogaraRep.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    bandogaraRep.SaveChanges()
    '' ''                Next
    '' ''                bandogaraRep.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''                'caso consulenti e collaboratori
    '' ''                Dim consulenzaRep As New ParsecPub.ConsulenzaRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.Consulenza = consulenzaRep.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    consulenzaRep.SaveChanges()
    '' ''                Next
    '' ''                consulenzaRep.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''                'caso incarichi dipendenti
    '' ''                Dim incaricoRep As New ParsecPub.IncaricoDipendenteRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.IncaricoDipendente = incaricoRep.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    incaricoRep.SaveChanges()
    '' ''                Next
    '' ''                incaricoRep.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale

    '' ''                Dim incarichi As New ParsecPub.IncaricoAmministrativoDirigenzialeRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.IncaricoAmministrativoDirigenziale = incarichi.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    incarichi.SaveChanges()
    '' ''                Next
    '' ''                incarichi.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)


    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso


    '' ''                Dim bandi As New ParsecPub.BandoConcorsoRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.BandoConcorso = bandi.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    bandi.SaveChanges()
    '' ''                Next
    '' ''                bandi.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''            Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate


    '' ''                Dim enti As New ParsecPub.EnteControllatoRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.EnteControllato = enti.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.Id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    enti.SaveChanges()
    '' ''                Next
    '' ''                enti.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)



    '' ''            Case Else    'PUBBLICAZIONE GENERICA

    '' ''                Dim pubblicazioniGeneriche As New ParsecPub.PubblicazioneGenericaRepository
    '' ''                Dim dataInizioPubblicazioneWS As New Date
    '' ''                Dim dataFinePubblicazioneWS As New Date
    '' ''                For Each de In hashTableOutputWS
    '' ''                    Dim idAtto = de.Key
    '' ''                    Dim attoWS = de.Value
    '' ''                    Dim atto As ParsecPub.PubblicazioneGenerica = pubblicazioniGeneriche.GetQuery.Where(Function(c) c.idPubblicazione = pubblicazione.Id And c.id = idAtto).FirstOrDefault
    '' ''                    atto.idPubblicazioneWS = attoWS.idPubblicazioneWS
    '' ''                    Try
    '' ''                        dataInizioPubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.datainizio, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'Date.SpecifyKind(attoWS.datainizio, DateTimeKind.Local)
    '' ''                        dataFinePubblicazioneWS = TimeZoneInfo.ConvertTime(attoWS.dataFine, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))  'attoWS.dataFine
    '' ''                    Catch ex As Exception
    '' ''                        dataInizioPubblicazioneWS = attoWS.datainizio
    '' ''                        dataFinePubblicazioneWS = attoWS.dataFine
    '' ''                    End Try
    '' ''                    pubblicazioniGeneriche.SaveChanges()
    '' ''                Next
    '' ''                pubblicazioniGeneriche.Dispose()
    '' ''                aggiornaStatoPubblicazione(pubblicazione, dataInizioPubblicazioneWS, dataFinePubblicazioneWS)

    '' ''        End Select

    '' ''    Catch ex As Exception
    '' ''        Throw New Exception("Riscontrati problemi con la Pubblicazione: " & ex.Message)
    '' ''    End Try
    '' ''End Sub

    'aggiorno lo stato della Pubblicazione insieme alle date di Pubblicazine.
    'Tale metodo va richamato solo alla prima pubblicazione quando le date di pubblicazione possono essere note solo dopo la pubblicazione on-line
    'luca 01/07/2020
    '' ''Private Sub aggiornaStatoPubblicazione(ByVal pubblicazione As ParsecPub.Pubblicazione, ByVal datainizio As Date, ByVal dataFine As Date)
    '' ''    If (Not pubblicazione.Pubblicato) Then
    '' ''        Dim pubblicazioniRepository As New ParsecPub.PubblicazioneRepository
    '' ''        Dim pubbl As ParsecPub.Pubblicazione = pubblicazioniRepository.GetQuery.Where(Function(c) c.Id = pubblicazione.Id).FirstOrDefault
    '' ''        pubbl.Pubblicato = True
    '' ''        pubbl.DataInizioPubblicazione = datainizio
    '' ''        pubbl.DataFinePubblicazione = dataFine
    '' ''        pubblicazioniRepository.SaveChanges()
    '' ''    End If
    '' ''End Sub

    'luca 02/07/2020
    '' ''Private Function AggiornaAlbo(idDocumento As Integer, pubblicato As Boolean) As ParsecMES.Pubblicazione

    '' ''    Dim documenti As New ParsecAtt.DocumentoRepository
    '' ''    Dim documento As ParsecAtt.Documento = documenti.GetById(idDocumento)
    '' ''    documento.Allegati = documenti.GetAllegati(idDocumento)
    '' ''    documenti.Dispose()

    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

    '' ''    Dim parametri As New ParsecAdmin.ParametriRepository
    '' ''    Dim parametroOmissis = parametri.GetByName("Omissis", ParsecAdmin.TipoModulo.ATT)
    '' ''    parametri.Dispose()

    '' ''    Dim descrizione As String = String.Empty

    '' ''    Dim idTipologia As Integer = 0
    '' ''    Select Case Me.TipologiaDocumento
    '' ''        Case ParsecAtt.TipoDocumento.Delibera

    '' ''            Dim tipologieSeduta As New ParsecAtt.TipologiaSedutaRepository
    '' ''            Dim tipologiaSeduta = tipologieSeduta.GetQuery.Where(Function(c) c.Id = documento.IdTipologiaSeduta).FirstOrDefault

    '' ''            Dim tipiDocumento As New ParsecMES.TipiDocumentoRepository

    '' ''            Dim tipoDocumento = tipiDocumento.GetQuery.Where(Function(c) c.Codice = tipologiaSeduta.CodiceTipologiaDocumento).FirstOrDefault

    '' ''            idTipologia = tipoDocumento.Codice
    '' ''            descrizione = tipoDocumento.Descrizione

    '' ''            'Select Case documento.TipologiaSeduta
    '' ''            '    Case ParsecAtt.TipologiaOrganoDeliberante.CommissarioPrefettizio
    '' ''            '        idTipologia = 7 'Delibera di Commissario Straordinario
    '' ''            '        descrizione = "Delibera di Commissario"
    '' ''            '    Case ParsecAtt.TipologiaOrganoDeliberante.GiuntaComunale
    '' ''            '        idTipologia = 4 'Delibera di Giunta Comunale
    '' ''            '        descrizione = "Delibera di Giunta"
    '' ''            '    Case ParsecAtt.TipologiaOrganoDeliberante.ConsiglioComunale
    '' ''            '        idTipologia = 5 'Delibera di Consiglio Comunale
    '' ''            '        descrizione = "Delibera di Consiglio"
    '' ''            '    Case ParsecAtt.TipologiaOrganoDeliberante.SubCommissarioPrefettizio
    '' ''            '        idTipologia = 14 'Delibera di Sub-commissario Prefettizio
    '' ''            '        descrizione = "Delibera di SubCommissario"
    '' ''            'End Select
    '' ''        Case ParsecAtt.TipoDocumento.Determina
    '' ''            idTipologia = documento.IdTipologiaDocumento
    '' ''            descrizione = "Determina"
    '' ''        Case ParsecAtt.TipoDocumento.Decreto
    '' ''            idTipologia = documento.IdTipologiaDocumento
    '' ''            descrizione = "Decreto"
    '' ''        Case ParsecAtt.TipoDocumento.Ordinanza
    '' ''            idTipologia = documento.IdTipologiaDocumento
    '' ''            descrizione = "Ordinanza"
    '' ''    End Select

    '' ''    'https://regex101.com/
    '' ''    'https://www.tutorialspoint.com/html/html_entities.htm

    '' ''    'Dim pattern As String = "\s+\#[^\#]*\#"

    '' ''    Dim pattern As String = "#[\u0000-\uFFFF]+#"
    '' ''    Dim rgx As New Regex(pattern)
    '' ''    Dim oggetto As String = documento.Oggetto
    '' ''    If Not parametroOmissis Is Nothing Then
    '' ''        oggetto = rgx.Replace(documento.Oggetto, parametroOmissis.Valore)
    '' ''    End If

    '' ''    Dim pubblicazione As New ParsecMES.Pubblicazione



    '' ''    If Not pubblicato Then
    '' ''        pubblicazione.DataRegistrazione = Now
    '' ''    End If

    '' ''    pubblicazione.Oggetto = String.Format("{0} N. {1} del {2} Oggetto: {3}", descrizione, documento.ContatoreGenerale, documento.Data.Value.ToShortDateString, oggetto)
    '' ''    pubblicazione.ContatoreDocumento = documento.ContatoreGenerale
    '' ''    pubblicazione.DataDocumento = documento.Data
    '' ''    pubblicazione.IdModulo = CInt(ParsecAdmin.TipoModulo.ATT)



    '' ''    pubblicazione.Struttura = documento.DescrizioneSettore
    '' ''    pubblicazione.IdTipologia = idTipologia
    '' ''    pubblicazione.DataInizioPubblicazione = documento.DataAffissioneDa

    '' ''    If documento.GiorniAffissione.HasValue Then
    '' ''        pubblicazione.GiorniPubblicazione = documento.GiorniAffissione
    '' ''    Else
    '' ''        pubblicazione.GiorniPubblicazione = 15

    '' ''        parametri = New ParsecAdmin.ParametriRepository
    '' ''        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("GiorniDefaultPubblicazione", ParsecAdmin.TipoModulo.ATT)
    '' ''        If Not parametro Is Nothing Then
    '' ''            pubblicazione.GiorniPubblicazione = parametro.Valore
    '' ''        End If
    '' ''        parametri.Dispose()
    '' ''    End If

    '' ''    pubblicazione.DataFinePubblicazione = pubblicazione.DataInizioPubblicazione.Value.AddDays(pubblicazione.GiorniPubblicazione)
    '' ''    pubblicazione.IdUtente = utenteCollegato.Id
    '' ''    pubblicazione.IdDocumento = documento.Id


    '' ''    Dim nomeFilePdf As String = IO.Path.GetFileNameWithoutExtension(documento.Nomefile) & ".pdf"
    '' ''    Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo"), Me.GetAnnoEsercizio(documento), nomeFilePdf)
    '' ''    Dim localPathTemp As String = String.Format("{0}{1}", ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp"), nomeFilePdf)

    '' ''    Dim allegato As New ParsecMES.Documento
    '' ''    allegato.Nomefile = nomeFilePdf
    '' ''    allegato.Descrizione = oggetto
    '' ''    Dim impronta = ParsecUtility.Utility.CalcolaHashFromFile(localPath)
    '' ''    allegato.Impronta = BitConverter.ToString(impronta).Replace("-", "")
    '' ''    allegato.IdTipologia = 1  'Primario
    '' ''    'Copio l'allegato nella cartella temporanea
    '' ''    Me.CopiaDocumento(localPath, localPathTemp)

    '' ''    pubblicazione.Documenti.Add(allegato)

    '' ''    Dim nomefileAllegato As String = String.Empty

    '' ''    For Each all In documento.Allegati.Where(Function(c) c.Pubblicato = True).ToList

    '' ''        If Not String.IsNullOrEmpty(all.NomeFileFirmato) Then
    '' ''            nomefileAllegato = all.NomeFileFirmato
    '' ''        Else
    '' ''            nomefileAllegato = all.Nomefile
    '' ''        End If

    '' ''        localPathTemp = String.Format("{0}{1}", ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp"), nomefileAllegato)
    '' ''        localPath = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti"), all.PercorsoRelativo, nomefileAllegato)

    '' ''        allegato = New ParsecMES.Documento
    '' ''        allegato.Nomefile = nomefileAllegato
    '' ''        allegato.Descrizione = all.Oggetto
    '' ''        impronta = ParsecUtility.Utility.CalcolaHashFromFile(localPath)
    '' ''        allegato.Impronta = BitConverter.ToString(impronta).Replace("-", "")
    '' ''        allegato.IdTipologia = 2  'Secondario
    '' ''        pubblicazione.Documenti.Add(allegato)
    '' ''        'Copio l'allegato nella cartella temporanea
    '' ''        Me.CopiaDocumento(localPath, localPathTemp)
    '' ''    Next


    '' ''    Return pubblicazione
    '' ''End Function

    Private Function AbilitatoAlbo() As Boolean
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("AbilitaAlboPretorio", ParsecAdmin.TipoModulo.ATT)
        parametri.Dispose()

        Dim abilitato As Boolean = False
        If Not parametro Is Nothing Then
            abilitato = CBool(parametro.Valore)
        End If
        Return abilitato
    End Function

    Private Function AbilitatoAmministrazioneAperta() As Boolean
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("AbilitaAmministrazioneAperta", ParsecAdmin.TipoModulo.ATT)
        parametri.Dispose()

        Dim abilitato As Boolean = False
        If Not parametro Is Nothing Then
            abilitato = CBool(parametro.Valore)
        End If
        Return abilitato
    End Function

    Private Function PubblicazioneAlboAutomatica() As Boolean
        Dim abiltatoAlbo As Boolean = False
        If Not Page.Request("PubblicazioneDiretta") Is Nothing Then
            abiltatoAlbo = CBool(Page.Request("PubblicazioneDiretta"))
        Else
            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName("PubblicazioneAlboAutomatica", ParsecAdmin.TipoModulo.ATT)
            parametri.Dispose()
            If Not parametro Is Nothing Then
                abiltatoAlbo = CBool(parametro.Valore)
            End If
        End If
        Return abiltatoAlbo
    End Function

    ' In base al parametro settato (ServizioAlboPretorio) seleziona la versione corretta del servizio di albo pretorio da utilizzare
    'luca 02/07/2020
    '' ''Private Sub Pubblica(ByVal id As Integer)
    '' ''    Dim parametriR As New ParsecAdmin.ParametriRepository
    '' ''    Dim parametro As ParsecAdmin.Parametri = parametriR.GetByName("ServizioAlboPretorio", ParsecAdmin.TipoModulo.SEP)
    '' ''    If Not parametro Is Nothing Then
    '' ''        Select Case parametro.Valore
    '' ''            Case 0 'Albo pretorio classico
    '' ''                PubblicaServizio0(id)
    '' ''            Case 1 'Nuovo Albo pretorio
    '' ''                PubblicaServizio1(id)
    '' ''            Case Else 'Albo pretorio classico
    '' ''                PubblicaServizio0(id) 'Albo pretorio classico
    '' ''        End Select
    '' ''    Else
    '' ''        PubblicaServizio0(id)
    '' ''    End If
    '' ''    parametriR.Dispose()
    '' ''End Sub

    'luca 02/07/2020
    ' Albo pretorio classico (ASP.NET)    
    '' ''Private Sub PubblicaServizio0(idPubblicazione As Integer)

    '' ''    Dim pubblicazioni As New ParsecMES.AlboRepository
    '' ''    Dim pubblicazione As ParsecMES.Pubblicazione = pubblicazioni.GetById(idPubblicazione)
    '' ''    Dim messaggio As String = String.Empty

    '' ''    Try

    '' ''        ' If Not pubblicazione.Pubblicato Then

    '' ''        Dim clienti As New ParsecAdmin.ClientRepository
    '' ''        Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''        Dim codiceEnte As String = cliente.Identificativo
    '' ''        clienti.Dispose()

    '' ''        Dim anno As String = pubblicazione.DataRegistrazione.Value.Year.ToString

    '' ''        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")
    '' ''        Dim localPathAtti = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")



    '' ''        Dim ws As New PubblicazioneAlbo.wsPubblicazioneAlbo
    '' ''        ws.Timeout = -1

    '' ''        Dim percorsoNomeFile As String = String.Empty
    '' ''        Dim percorsoNomeFileFirmato As String = String.Empty

    '' ''        Dim documentoPrimario = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 1).FirstOrDefault
    '' ''        If Not documentoPrimario Is Nothing Then

    '' ''            percorsoNomeFile = localPath & anno & "\" & documentoPrimario.Nomefile
    '' ''            percorsoNomeFileFirmato = localPath & anno & "\" & documentoPrimario.NomeFileFirmato

    '' ''            Dim nomefile As String = documentoPrimario.Nomefile

    '' ''            If Not String.IsNullOrEmpty(documentoPrimario.NomeFileFirmato) Then

    '' ''                If IO.File.Exists(percorsoNomeFileFirmato) Then
    '' ''                    If IO.File.Exists(percorsoNomeFile) Then
    '' ''                        messaggio = PubblicazioneConFileFirmatoDigitalmente(pubblicazione, percorsoNomeFile, percorsoNomeFileFirmato, codiceEnte, ws)
    '' ''                    Else
    '' ''                        messaggio = "Il documento primario " & documentoPrimario.Nomefile & " non esiste!"
    '' ''                    End If
    '' ''                Else
    '' ''                    messaggio = "Il documento primario firmato" & documentoPrimario.NomeFileFirmato & " non esiste!"
    '' ''                End If
    '' ''            Else
    '' ''                If Not String.IsNullOrEmpty(documentoPrimario.Nomefile) Then
    '' ''                    If IO.File.Exists(percorsoNomeFile) Then
    '' ''                        messaggio = PubblicazioneSenzaFileFirmatoDigitalmente(pubblicazione, percorsoNomeFile, codiceEnte, ws)
    '' ''                    Else
    '' ''                        messaggio = "Il documento primario " & documentoPrimario.Nomefile & " non esiste!"
    '' ''                    End If
    '' ''                End If
    '' ''            End If
    '' ''        Else
    '' ''            messaggio = PubblicazioneSoloOggetto(pubblicazione, codiceEnte, ws)
    '' ''        End If

    '' ''        If String.IsNullOrEmpty(messaggio) Then

    '' ''            Dim documentiAllegati As List(Of ParsecMES.Documento) = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 2).ToList
    '' ''            Dim bytesNomefile As Byte() = Nothing
    '' ''            For Each doc In documentiAllegati
    '' ''                Dim percorsoFile As String = ""
    '' ''                If pubblicazione.IdModulo = CInt(ParsecAdmin.TipoModulo.ATT) Then
    '' ''                    'percorsoFile = localPathAtti & "ALL\" & doc.Nomefile
    '' ''                    percorsoFile = localPath & anno & "\" & doc.Nomefile
    '' ''                Else
    '' ''                    If pubblicazione.IdModulo = 9 Then
    '' ''                        percorsoFile = localPath & anno & "\" & doc.Nomefile
    '' ''                    End If
    '' ''                End If
    '' ''                If IO.File.Exists(percorsoFile) Then
    '' ''                    bytesNomefile = IO.File.ReadAllBytes(percorsoFile)
    '' ''                    Me.MessaggioAvviso &= ws.PubblicaAllegato(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, doc.Descrizione, bytesNomefile, doc.Nomefile)
    '' ''                    Me.MessaggioAvviso &= vbCrLf
    '' ''                End If

    '' ''            Next

    '' ''            If String.IsNullOrEmpty(messaggio) Then
    '' ''                '**********************************************************************************
    '' ''                'Aggiorno la pubblicazione
    '' ''                '**********************************************************************************

    '' ''                Dim pubbl As ParsecMES.Pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.Id = pubblicazione.Id).FirstOrDefault
    '' ''                pubbl.Pubblicato = True
    '' ''                pubblicazioni.SaveChanges()

    '' ''                Dim mailTo As String = String.Empty
    '' ''                If pubblicazione.IdDocumento > 0 Then
    '' ''                    mailTo = Me.GetDestinatariDocumento(pubblicazione)
    '' ''                Else
    '' ''                    If Not pubblicazione.Email Is Nothing Then
    '' ''                        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
    '' ''                        Dim m = emailRegex.Match(pubblicazione.Email.Email)
    '' ''                        If m.Success Then
    '' ''                            mailTo = pubblicazione.Email.Email
    '' ''                        End If
    '' ''                    End If
    '' ''                End If

    '' ''                If Not String.IsNullOrEmpty(mailTo) Then
    '' ''                    Dim emailsAlbo As New ParsecMES.EmailAlboRepository

    '' ''                    If pubblicazione.IdDocumento > 0 Then
    '' ''                        '**********************************************************************************
    '' ''                        'Inserisco una nuova email
    '' ''                        '**********************************************************************************
    '' ''                        Dim exist As Boolean = Not emailsAlbo.GetQuery.Where(Function(c) c.IdAlbo = pubblicazione.Id).FirstOrDefault Is Nothing
    '' ''                        If Not exist Then
    '' ''                            Dim email As New ParsecMES.EmailAlbo With {.IdAlbo = pubblicazione.Id, .Email = mailTo}
    '' ''                            emailsAlbo.Add(email)
    '' ''                            emailsAlbo.SaveChanges()
    '' ''                        End If
    '' ''                    End If

    '' ''                    Try

    '' ''                        '**********************************************************************************
    '' ''                        'Invio l'email anche se non ci sono allegati. 
    '' ''                        '**********************************************************************************
    '' ''                        If CheckConfigurazioneInvioEmail() Then
    '' ''                            Me.InviaEmailAvvenutaPubblicazione(pubblicazione, mailTo, percorsoNomeFile, percorsoNomeFileFirmato)
    '' ''                        End If

    '' ''                        'IGNORO EVENTUALE ERRORE CASUATO DALL'INVIO DELL'EMAIL
    '' ''                    Catch ex As Exception
    '' ''                    End Try

    '' ''                    Try
    '' ''                        '**********************************************************************************
    '' ''                        'Aggiorno i dati di pubblicazione dell'email
    '' ''                        '**********************************************************************************
    '' ''                        Dim email As ParsecMES.EmailAlbo = emailsAlbo.GetQuery.Where(Function(c) c.IdAlbo = pubblicazione.Id).FirstOrDefault
    '' ''                        If Not email Is Nothing Then
    '' ''                            email.Pubblicata = True
    '' ''                            email.DataPubblicazione = Now
    '' ''                            emailsAlbo.SaveChanges()
    '' ''                        End If

    '' ''                    Catch ex As Exception
    '' ''                        messaggio &= vbCrLf
    '' ''                        messaggio &= ex.Message
    '' ''                    End Try
    '' ''                    emailsAlbo.Dispose()

    '' ''                Else
    '' ''                    'Me.MessaggioAvviso &= "Destinatari/o assenti - Impossibile inviare e-mail avvenuta pubblicazione"
    '' ''                End If

    '' ''            End If

    '' ''        End If

    '' ''        '  End If


    '' ''    Catch ex As Exception
    '' ''        messaggio &= vbCrLf
    '' ''        messaggio &= ex.Message
    '' ''    Finally
    '' ''        pubblicazioni.Dispose()
    '' ''    End Try

    '' ''    If Not String.IsNullOrEmpty(messaggio) Then
    '' ''        Throw New ApplicationException(messaggio)
    '' ''    End If

    '' ''End Sub

    'luca 02/07/2020
    ' Nuovo Albo pretorio (Liferay)   
    '' ''Private Sub PubblicaServizio1(idPubblicazione As Integer)
    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    Dim pubblicazioni As New ParsecMES.AlboRepository
    '' ''    Dim pubblicazione As ParsecMES.Pubblicazione = pubblicazioni.GetById(idPubblicazione)
    '' ''    Dim messaggio As String = String.Empty

    '' ''    Try

    '' ''        ' If Not pubblicazione.Pubblicato Then

    '' ''        Dim clienti As New ParsecAdmin.ClientRepository
    '' ''        Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''        Dim codiceEnte As String = cliente.Identificativo
    '' ''        clienti.Dispose()

    '' ''        Dim anno As String = pubblicazione.DataRegistrazione.Value.Year.ToString
    '' ''        Dim numeroAtto As Integer = IIf(pubblicazione.ContatoreDocumento Is Nothing, 0, pubblicazione.ContatoreDocumento)
    '' ''        Dim dataAtto As Date = IIf(pubblicazione.DataDocumento Is Nothing, "01-01-1900", pubblicazione.DataDocumento)

    '' ''        Dim localPath As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")
    '' ''        Dim localPathAtti = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")



    '' ''        Dim ws As New PubblicazioneAlbo.PubblicazioneAlboPretorioServiceSoapService
    '' ''        ws.Timeout = -1

    '' ''        Dim percorsoNomeFile As String = String.Empty
    '' ''        Dim percorsoNomeFileFirmato As String = String.Empty

    '' ''        Dim documentoPrimario = pubblicazione.Documenti.Where(Function(c) c.IdTipologia = 1).FirstOrDefault
    '' ''        If Not documentoPrimario Is Nothing Then
    '' ''            percorsoNomeFile = localPath & anno & "\" & documentoPrimario.Nomefile
    '' ''            percorsoNomeFileFirmato = localPath & anno & "\" & documentoPrimario.NomeFileFirmato
    '' ''        End If

    '' ''        'Pubblicazione dati
    '' ''        Try
    '' ''            Dim pubblicazioneOnLine = ws.pubblica(codiceEnte, cliente.CodLicenza, utenteCollegato.Username, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, pubblicazione.DescrizioneTipologia, pubblicazione.NumeroRegistro, pubblicazione.DataRegistrazione, pubblicazione.Oggetto, pubblicazione.Struttura, "", "", "", numeroAtto, dataAtto)
    '' ''        Catch ex As Exception
    '' ''            messaggio = ex.Message
    '' ''        End Try

    '' ''        If String.IsNullOrEmpty(messaggio) Then
    '' ''            'Pubblicazione documenti allegati
    '' ''            Dim documenti As List(Of ParsecMES.Documento) = pubblicazione.Documenti.ToList
    '' ''            Dim documentiFirmati As List(Of ParsecMES.Documento) = (From doc In pubblicazione.Documenti Select New ParsecMES.Documento With {
    '' ''                                                                                                  .Id = doc.Id,
    '' ''                                                                                                  .IdAlbo = doc.IdAlbo,
    '' ''                                                                                                  .Nomefile = doc.NomeFileFirmato,
    '' ''                                                                                                  .IdTipologia = doc.IdTipologia,
    '' ''                                                                                                  .Descrizione = doc.Descrizione,
    '' ''                                                                                                  .NomeFileFirmato = doc.NomeFileFirmato,
    '' ''                                                                                                  .Impronta = doc.Impronta
    '' ''                                                                                                  }).ToList

    '' ''            Dim bytesNomefile As Byte() = Nothing
    '' ''            For Each doc In documenti.Union(documentiFirmati).OrderBy(Function(d) d.Id).ThenBy(Function(d) d.Nomefile)
    '' ''                Dim percorsoFile As String = ""
    '' ''                If pubblicazione.IdModulo = CInt(ParsecAdmin.TipoModulo.ATT) Then
    '' ''                    'percorsoFile = localPathAtti & "ALL\" & doc.Nomefile
    '' ''                    percorsoFile = localPath & anno & "\" & doc.Nomefile
    '' ''                Else
    '' ''                    If pubblicazione.IdModulo = 9 Then
    '' ''                        percorsoFile = localPath & anno & "\" & doc.Nomefile
    '' ''                    End If
    '' ''                End If
    '' ''                If IO.File.Exists(percorsoFile) Then
    '' ''                    Dim messaggioAllegato As String = String.Empty

    '' ''                    bytesNomefile = IO.File.ReadAllBytes(percorsoFile)
    '' ''                    Dim principale As Boolean
    '' ''                    Select Case doc.IdTipologia
    '' ''                        Case 1 'Documento Primario
    '' ''                            principale = True
    '' ''                        Case 2 'Documento Secondario
    '' ''                            principale = False
    '' ''                    End Select
    '' ''                    Try
    '' ''                        Dim documentoOnLine = ws.pubblicaDocumento(codiceEnte, cliente.CodLicenza, pubblicazione.NumeroRegistro, Year(pubblicazione.DataRegistrazione), doc.Nomefile, bytesNomefile, doc.Descrizione, principale, True)
    '' ''                    Catch ex As Exception
    '' ''                        Me.MessaggioAvviso &= "Allegato '" & doc.Nomefile & "' non pubblicato - " & ex.Message & vbCrLf
    '' ''                    End Try
    '' ''                End If

    '' ''            Next

    '' ''            If String.IsNullOrEmpty(messaggio) Then
    '' ''                '**********************************************************************************
    '' ''                'Aggiorno la pubblicazione
    '' ''                '**********************************************************************************

    '' ''                Dim pubbl As ParsecMES.Pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.Id = pubblicazione.Id).FirstOrDefault
    '' ''                pubbl.Pubblicato = True
    '' ''                pubblicazioni.SaveChanges()

    '' ''                Dim mailTo As String = String.Empty
    '' ''                If pubblicazione.IdDocumento > 0 Then
    '' ''                    mailTo = Me.GetDestinatariDocumento(pubblicazione)
    '' ''                Else
    '' ''                    If Not pubblicazione.Email Is Nothing Then
    '' ''                        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
    '' ''                        Dim m = emailRegex.Match(pubblicazione.Email.Email)
    '' ''                        If m.Success Then
    '' ''                            mailTo = pubblicazione.Email.Email
    '' ''                        End If
    '' ''                    End If
    '' ''                End If

    '' ''                If Not String.IsNullOrEmpty(mailTo) Then
    '' ''                    Dim emailsAlbo As New ParsecMES.EmailAlboRepository

    '' ''                    If pubblicazione.IdDocumento > 0 Then
    '' ''                        '**********************************************************************************
    '' ''                        'Inserisco una nuova email
    '' ''                        '**********************************************************************************
    '' ''                        Dim exist As Boolean = Not emailsAlbo.GetQuery.Where(Function(c) c.IdAlbo = pubblicazione.Id).FirstOrDefault Is Nothing
    '' ''                        If Not exist Then
    '' ''                            Dim email As New ParsecMES.EmailAlbo With {.IdAlbo = pubblicazione.Id, .Email = mailTo}
    '' ''                            emailsAlbo.Add(email)
    '' ''                            emailsAlbo.SaveChanges()
    '' ''                        End If
    '' ''                    End If

    '' ''                    Try

    '' ''                        '**********************************************************************************
    '' ''                        'Invio l'email anche se non ci sono allegati. 
    '' ''                        '**********************************************************************************
    '' ''                        If CheckConfigurazioneInvioEmail() Then
    '' ''                            Me.InviaEmailAvvenutaPubblicazione(pubblicazione, mailTo, percorsoNomeFile, percorsoNomeFileFirmato)
    '' ''                        End If

    '' ''                        'IGNORO EVENTUALE ERRORE CASUATO DALL'INVIO DELL'EMAIL
    '' ''                    Catch ex As Exception
    '' ''                    End Try

    '' ''                    Try
    '' ''                        '**********************************************************************************
    '' ''                        'Aggiorno i dati di pubblicazione dell'email
    '' ''                        '**********************************************************************************
    '' ''                        Dim email As ParsecMES.EmailAlbo = emailsAlbo.GetQuery.Where(Function(c) c.IdAlbo = pubblicazione.Id).FirstOrDefault
    '' ''                        If Not email Is Nothing Then
    '' ''                            email.Pubblicata = True
    '' ''                            email.DataPubblicazione = Now
    '' ''                            emailsAlbo.SaveChanges()
    '' ''                        End If

    '' ''                    Catch ex As Exception
    '' ''                        messaggio &= vbCrLf
    '' ''                        messaggio &= ex.Message
    '' ''                    End Try
    '' ''                    emailsAlbo.Dispose()

    '' ''                Else
    '' ''                    'Me.MessaggioAvviso &= "Destinatari/o assenti - Impossibile inviare e-mail avvenuta pubblicazione"
    '' ''                End If

    '' ''            End If

    '' ''        End If

    '' ''        '  End If


    '' ''    Catch ex As Exception
    '' ''        messaggio &= vbCrLf
    '' ''        messaggio &= ex.Message
    '' ''    Finally
    '' ''        pubblicazioni.Dispose()
    '' ''    End Try

    '' ''    If Not String.IsNullOrEmpty(messaggio) Then
    '' ''        Throw New ApplicationException(messaggio)
    '' ''    End If

    '' ''End Sub

    'luca 02/07/2020
    '' ''Private Function GetDestinatariDocumento(pubblicazione As ParsecMES.Pubblicazione) As String
    '' ''    Dim mailTo As String = String.Empty
    '' ''    Dim firmeDocumento As New ParsecAtt.DocumentiFirmeRepository
    '' ''    Dim firme = firmeDocumento.GetQuery.Where(Function(c) c.IdDocumento = pubblicazione.IdDocumento).GroupBy(Function(c) c.IdUtente).Select(Function(c) c.FirstOrDefault).Select(Function(c) c.IdUtente).ToList
    '' ''    Dim utenti As New ParsecAdmin.UserRepository
    '' ''    Dim emails = utenti.GetQuery.Where(Function(c) firme.Contains(c.Id) And c.Email <> "" And c.Email <> ";").Select(Function(c) c).Select(Function(c) c.Email).ToList
    '' ''    utenti.Dispose()
    '' ''    firmeDocumento.Dispose()

    '' ''    Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)

    '' ''    For Each email In emails
    '' ''        Dim m = emailRegex.Match(email)
    '' ''        If m.Success Then
    '' ''            mailTo &= email & ";"
    '' ''        End If
    '' ''    Next
    '' ''    If mailTo.EndsWith(";") Then
    '' ''        mailTo = mailTo.Remove(mailTo.Length - 1)
    '' ''    End If
    '' ''    Return mailTo
    '' ''End Function

    'Private Function ConfiguraCdoEmail() As MailMessage

    '    Dim parametri As New ParsecAdmin.ParametriRepository
    '    Dim parametro = parametri.GetByName("MittenteEmail", ParsecAdmin.TipoModulo.ATT)
    '    parametri.Dispose()
    '    'If parametro Is Nothing Then
    '    '    Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "Il parametro 'MittenteEmail' non è presente.")
    '    '    Exit Function
    '    'End If

    '    Const cdoBasic As Integer = 1
    '    Const cdoSendUsingPort As Integer = 2
    '    Dim mail As MailMessage = Nothing
    '    Dim casellePec As New ParsecAdmin.ParametriPecRepository
    '    Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetQuery.Where(Function(c) c.Email = parametro.Valore).FirstOrDefault
    '    ' If Not casellaPec Is Nothing Then

    '    mail = New MailMessage
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", casellaPec.SmtpServer)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", casellaPec.SmtpPorta)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", casellaPec.UserId)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", casellaPec.Password)
    '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", casellaPec.SmtpIsSSL)
    '    mail.From = casellaPec.Email
    '    mail.BodyFormat = MailFormat.Html
    '    mail.Priority = MailPriority.High
    '    SmtpMail.SmtpServer = casellaPec.SmtpServer & ":" & casellaPec.SmtpPorta.ToString
    '    'Else
    '    'Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "La casella di posta '" & parametro.Valore & "' non è presente.")
    '    'Exit Function
    '    'End If
    '    Return mail
    'End Function

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
            Throw New ApplicationException("Si è verificato il seguente errore: " & ex.Message)
        End Try
        Return client
    End Function

    Private Function CheckEmail(ByVal Indirizzo As String) As Boolean
        Dim emailRegex As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase)
        Dim m = emailRegex.Match(Indirizzo)
        Return m.Success
    End Function



    Private Function CheckConfigurazioneInvioEmail() As Boolean
        Dim successo As Boolean = False
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("MittenteEmail", ParsecAdmin.TipoModulo.ATT)
        parametri.Dispose()
        If Not parametro Is Nothing Then
            Dim casellePec As New ParsecAdmin.ParametriPecRepository
            Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetQuery.Where(Function(c) c.Email = parametro.Valore).FirstOrDefault
            If Not casellaPec Is Nothing Then
                successo = True
            End If
            casellePec.Dispose()
        End If
        parametri.Dispose()
        Return successo
    End Function

    'Private Sub InviaEmailAvvenutaPubblicazione(pubblicazione As ParsecMES.Pubblicazione, ByVal mailTo As String, ByVal pathNomefile As String, ByVal pathNomefilefirmato As String)

    '    Dim parametri As New ParsecAdmin.ParametriRepository
    '    Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AvvenutaPubblicazione", ParsecAdmin.TipoModulo.SEP)
    '    parametri.Dispose()
    '    Dim body As String = String.Empty

    '    If Not parametro Is Nothing Then
    '        body = parametro.Valore
    '        body = body.Replace("#Documento#", "<b>" & pubblicazione.Oggetto & "</b>")
    '        body = body.Replace("#numeroAlbo#", "<b>" & pubblicazione.NumeroRegistro.ToString & "</b>")
    '        body = body.Replace("#datapubblicazione#", "<b>" & Now.ToShortDateString & "</b>")
    '        body = body.Replace("#datada#", "<b>" & pubblicazione.DataInizioPubblicazione.Value.ToShortDateString & "</b>")
    '        body = body.Replace("#dataa#", "<b>" & pubblicazione.DataFinePubblicazione.Value.ToShortDateString & "</b>")
    '        body &= "<br/>Codice (ID) referta <b>" & pubblicazione.Id.ToString & "</b><br/>N.B.E-mail generata automaticamente dal sistema SEP, non rispondere, indirizzo inesistente!"
    '    End If

    '    Dim mail As MailMessage = Nothing

    '    Try
    '        mail = Me.ConfiguraCdoEmail
    '    Catch ex As Exception
    '        Throw New ApplicationException(ex.Message)
    '        Exit Sub
    '    End Try

    '    mail.To = mailTo
    '    mail.Subject = "Pubblicazione dell'atto numero di registrazione " & pubblicazione.NumeroRegistro.ToString
    '    mail.Body = body

    '    If IO.File.Exists(pathNomefile) Then
    '        mail.Attachments.Add(New MailAttachment(pathNomefile))
    '    End If

    '    If IO.File.Exists(pathNomefilefirmato) Then
    '        mail.Attachments.Add(New MailAttachment(pathNomefilefirmato))
    '    End If

    '    Try
    '        '********************************************************************************************************************
    '        'L'invio di PEC con indirizzi in copia nascosta (BCC o CCN) non è permesso dalla normativa 
    '        'sulla posta elettronica certificata in quanto nascondono l'indirizzo del destinatario.
    '        '********************************************************************************************************************
    '        'Try
    '        '    mail.Bcc = ParsecAdmin.WebConfigSettings.GetKey("BCCEmail")
    '        'Catch ex As Exception
    '        'End Try
    '        '********************************************************************************************************************
    '        SmtpMail.Send(mail)
    '    Catch ex As Exception
    '        Throw New ApplicationException(ex.Message)
    '    End Try

    'End Sub

    'luca 02/07/2020
    '' ''Private Sub InviaEmailAvvenutaPubblicazione(pubblicazione As ParsecMES.Pubblicazione, ByVal mailTo As String, ByVal pathNomefile As String, ByVal pathNomefilefirmato As String)

    '' ''    Try

    '' ''        Dim parametri As New ParsecAdmin.ParametriRepository
    '' ''        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("AvvenutaPubblicazione", ParsecAdmin.TipoModulo.SEP)

    '' ''        Dim body As String = String.Empty

    '' ''        If Not parametro Is Nothing Then
    '' ''            body = parametro.Valore
    '' ''            body = body.Replace("#Documento#", "<b>" & pubblicazione.Oggetto & "</b>")
    '' ''            body = body.Replace("#numeroAlbo#", "<b>" & pubblicazione.NumeroRegistro.ToString & "</b>")
    '' ''            body = body.Replace("#datapubblicazione#", "<b>" & Now.ToShortDateString & "</b>")
    '' ''            body = body.Replace("#datada#", "<b>" & pubblicazione.DataInizioPubblicazione.Value.ToShortDateString & "</b>")
    '' ''            body = body.Replace("#dataa#", "<b>" & pubblicazione.DataFinePubblicazione.Value.ToShortDateString & "</b>")
    '' ''            body &= "<br/>Codice (ID) referta <b>" & pubblicazione.Id.ToString & "</b><br/>N.B.E-mail generata automaticamente dal sistema SEP, non rispondere, indirizzo inesistente!"
    '' ''        End If

    '' ''        parametro = parametri.GetByName("MittenteEmail", ParsecAdmin.TipoModulo.ATT)
    '' ''        parametri.Dispose()

    '' ''        If parametro Is Nothing Then
    '' ''            Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "Il parametro 'MittenteEmail' non è presente.")
    '' ''        End If

    '' ''        Dim casellePec As New ParsecAdmin.ParametriPecRepository
    '' ''        Dim casellaPec As ParsecAdmin.ParametriPec = casellePec.GetQuery.Where(Function(c) c.Email = parametro.Valore).FirstOrDefault
    '' ''        casellePec.Dispose()

    '' ''        If casellaPec Is Nothing Then
    '' ''            Throw New ApplicationException("Il sistema non è configurato per l'invio delle e-mail." & vbCrLf & "La casella di posta '" & parametro.Valore & "' non è presente.")
    '' ''        End If

    '' ''        Dim client As Rebex.Net.Smtp = Me.ConfigureSmtp(casellaPec)

    '' ''        Dim mail As New Rebex.Mail.MailMessage
    '' ''        Dim mailAttach As Rebex.Mail.Attachment = Nothing

    '' ''        mail.From = casellaPec.Email

    '' ''        Dim listaEmail = mailTo.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)

    '' ''        For Each s In listaEmail
    '' ''            If Me.CheckEmail(s) Then
    '' ''                If Not mail.To.Contains(s) Then
    '' ''                    mail.To.Add(s)
    '' ''                End If
    '' ''            End If
    '' ''        Next

    '' ''        mail.Subject = "Pubblicazione dell'atto numero di registrazione " & pubblicazione.NumeroRegistro.ToString
    '' ''        mail.BodyHtml = body
    '' ''        mail.Priority = Rebex.Mail.MailPriority.High

    '' ''        If IO.File.Exists(pathNomefile) Then
    '' ''            mailAttach = New Rebex.Mail.Attachment(pathNomefile)
    '' ''            mailAttach.FileName = IO.Path.GetFileName(pathNomefile)
    '' ''            mail.Attachments.Add(mailAttach)
    '' ''        End If

    '' ''        If IO.File.Exists(pathNomefilefirmato) Then
    '' ''            mailAttach = New Rebex.Mail.Attachment(pathNomefilefirmato)
    '' ''            mailAttach.FileName = IO.Path.GetFileName(pathNomefilefirmato)
    '' ''            mail.Attachments.Add(mailAttach)
    '' ''        End If

    '' ''        client.Timeout = 0
    '' ''        client.Send(mail)
    '' ''        client.Disconnect()

    '' ''    Catch ex As Exception
    '' ''        Throw New ApplicationException(ex.Message)
    '' ''    End Try
    '' ''End Sub



    'luca 02/07/2020
    '' ''Private Function PubblicazioneConFileFirmatoDigitalmente(ByVal pubblicazione As ParsecMES.Pubblicazione, ByVal percorsoNomeFile As String, percorsoNomeFileFirmato As String, codiceEnte As String, ByVal ws As PubblicazioneAlbo.wsPubblicazioneAlbo) As String
    '' ''    Dim ret As String = String.Empty
    '' ''    Dim nomefile As String = IO.Path.GetFileName(percorsoNomeFile)
    '' ''    Dim nomefileFirmato As String = IO.Path.GetFileName(percorsoNomeFileFirmato)
    '' ''    Dim bytesNomefile As Byte() = IO.File.ReadAllBytes(percorsoNomeFile)
    '' ''    Dim bytesNomefileFirmato As Byte() = IO.File.ReadAllBytes(percorsoNomeFileFirmato)
    '' ''    If pubblicazione.ContatoreDocumento > 0 Then
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.ContatoreDocumento, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, bytesNomefile, nomefile, bytesNomefileFirmato, nomefileFirmato)
    '' ''    Else
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, bytesNomefile, nomefile, bytesNomefileFirmato, nomefileFirmato)
    '' ''    End If
    '' ''    Return ret
    '' ''End Function

    'luca 02/07/2020
    '' ''Private Function PubblicazioneSenzaFileFirmatoDigitalmente(ByVal pubblicazione As ParsecMES.Pubblicazione, ByVal percorsoNomeFile As String, codiceEnte As String, ByVal ws As PubblicazioneAlbo.wsPubblicazioneAlbo) As String
    '' ''    Dim ret As String = String.Empty
    '' ''    Dim bytesNomefile As Byte() = IO.File.ReadAllBytes(percorsoNomeFile)
    '' ''    Dim nomefile As String = IO.Path.GetFileName(percorsoNomeFile)
    '' ''    If pubblicazione.ContatoreDocumento > 0 Then
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.ContatoreDocumento, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, bytesNomefile, nomefile)
    '' ''    Else
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione, bytesNomefile, nomefile)
    '' ''    End If
    '' ''    Return ret
    '' ''End Function

    'luca 02/07/2020
    '' ''Private Function PubblicazioneSoloOggetto(ByVal pubblicazione As ParsecMES.Pubblicazione, codiceEnte As String, ByVal ws As PubblicazioneAlbo.wsPubblicazioneAlbo) As String
    '' ''    Dim ret As String = String.Empty
    '' ''    If pubblicazione.ContatoreDocumento > 0 Then
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.ContatoreDocumento, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione)
    '' ''    Else
    '' ''        ret = ws.Pubblica(pubblicazione.NumeroRegistro, codiceEnte, pubblicazione.DataRegistrazione.Value.ToShortDateString, pubblicazione.Struttura, pubblicazione.Oggetto, pubblicazione.IdTipologia, pubblicazione.DataInizioPubblicazione, pubblicazione.DataFinePubblicazione)
    '' ''    End If
    '' ''    Return ret
    '' ''End Function

    'luca 02/07/2020
    '' ''Private Sub SalvaAlbo(idDocumento As Integer)
    '' ''    Try
    '' ''        Dim documenti As New ParsecAtt.DocumentoRepository
    '' ''        If Me.AbilitatoAlbo Then


    '' ''            Dim documentiView As New ParsecMES.AttiAmministrativiViewRepository
    '' ''            Dim pubblicazioni As New ParsecMES.AlboRepository(documentiView.Context)

    '' ''           Dim pubblicazionePrecedente = (From doc In documentiView.GetQuery
    '' ''                   Join pub In pubblicazioni.GetQuery
    '' ''                   On doc.Id Equals pub.IdDocumento
    '' ''                   Where doc.Codice = Me.Documento.Codice And pub.Stato Is Nothing And pub.IdModulo = ParsecAdmin.TipoModulo.ATT
    '' ''                   Select pub).FirstOrDefault


    '' ''            Dim pubblicato = Not pubblicazionePrecedente Is Nothing

    '' ''            Dim pubblicazione As ParsecMES.Pubblicazione = Me.AggiornaAlbo(idDocumento, pubblicato)
    '' ''            pubblicazioni.Pubblicazione = pubblicazionePrecedente

    '' ''            pubblicazioni.Save(pubblicazione)

    '' ''            If Not pubblicato Then
    '' ''                '********************************************************************************************************
    '' ''                'Aggiorno il numero di registro
    '' ''                '********************************************************************************************************
    '' ''                Dim documentoDaAggiornare As ParsecAtt.Documento = documenti.GetQuery.Where(Function(c) c.Id = idDocumento).FirstOrDefault
    '' ''                documentoDaAggiornare.NumeroRegistroPubblicazione = pubblicazione.NumeroRegistro
    '' ''                '********************************************************************************************************

    '' ''                documenti.SaveChanges()
    '' ''            End If



    '' ''            If Me.PubblicazioneAlboAutomatica() Then

    '' ''                Dim idPubblicazione As Integer = pubblicazioni.Pubblicazione.Id

    '' ''                Dim nomeFileFirmato As String = String.Format("{0}{1}.p7m", IO.Path.GetFileNameWithoutExtension(Me.Documento.Nomefile), ".pdf")

    '' ''                pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.Id = idPubblicazione).FirstOrDefault
    '' ''                Dim annoEsercizio As String = Me.Documento.DataDocumento.Value.Year.ToString

    '' ''                Dim localPathAtti As String = ParsecAdmin.WebConfigSettings.GetKey("PathAtti")
    '' ''                Dim pathNomeFileFirmatoAtti As String = String.Format("{0}{1}\{2}", localPathAtti, annoEsercizio, nomeFileFirmato)
    '' ''                If IO.File.Exists(pathNomeFileFirmatoAtti) Then
    '' ''                    Dim documentiMes As New ParsecMES.DocumentiRepository
    '' ''                    Dim annoEsercizioAlbo As String = pubblicazione.DataRegistrazione.Value.Year.ToString

    '' ''                    Dim documentoPrimario As ParsecMES.Documento = documentiMes.GetQuery.Where(Function(c) c.IdAlbo = idPubblicazione And c.IdTipologia = 1).FirstOrDefault
    '' ''                    Dim nomeFileFirmatoAlbo As String = String.Format("{0}{1}.p7m", IO.Path.GetFileNameWithoutExtension(documentoPrimario.Nomefile), ".pdf")

    '' ''                    Dim localPathAlbo As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")
    '' ''                    Dim pathNomeFileFirmatoAlbo As String = String.Format("{0}{1}\{2}", localPathAlbo, annoEsercizioAlbo, nomeFileFirmatoAlbo)

    '' ''                    Me.CopiaDocumento(pathNomeFileFirmatoAtti, pathNomeFileFirmatoAlbo)
    '' ''                    documentoPrimario.NomeFileFirmato = nomeFileFirmatoAlbo
    '' ''                    documentiMes.SaveChanges()
    '' ''                    documentiMes.Dispose()
    '' ''                End If
    '' ''                Me.Pubblica(idPubblicazione)
    '' ''            End If
    '' ''        End If

    '' ''        'luca 01/07/2020
    '' ''        '' ''Dim parametri As New ParsecAdmin.ParametriRepository
    '' ''        '' ''Dim parametro = parametri.GetByName("AbilitaPubblicazioneProvvedimenti", ParsecAdmin.TipoModulo.ATT)
    '' ''        '' ''Dim moduli As New ParsecAdmin.ModuleRepository
    '' ''        '' ''Dim modulo = moduli.Where(Function(c) c.Id = ParsecAdmin.TipoModulo.PUB And c.Abilitato = True).FirstOrDefault
    '' ''        '' ''If Not parametro Is Nothing AndAlso Not modulo Is Nothing Then
    '' ''        '' ''    If parametro.Valore = "1" Then
    '' ''        '' ''        Dim documentoDaPubblicare As ParsecAtt.Documento = documenti.GetById(idDocumento)

    '' ''        '' ''        'Inserisco nel modulo pubblicazioni
    '' ''        '' ''       Me.InserisciInModuloPubblicazioni(documentoDaPubblicare)

    '' ''        '' ''    End If
    '' ''        '' ''End If

    '' ''        documenti.SaveChanges()

    '' ''        'luca 01/07/2020
    '' ''        '' ''parametri.Dispose()
    '' ''        '' ''moduli.Dispose()

    '' ''    Catch ex As Exception
    '' ''        If Not ex.InnerException Is Nothing Then
    '' ''            Throw New ApplicationException(ex.InnerException.Message)
    '' ''        Else
    '' ''            Throw New ApplicationException(ex.Message)
    '' ''        End If

    '' ''    End Try
    '' ''End Sub

    'luca 01/07/2020
    ' '' ''Protected Sub eseguiPubblicazioneOnline_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles eseguiPubblicazioneOnline.Click
    ' '' ''    If Not Me.Pubblicazione Is Nothing Then
    ' '' ''        Dim id As Integer = Me.Pubblicazione.Id

    ' '' ''        Dim successo As Boolean = True
    ' '' ''        Dim pubblicazioni As New ParsecPub.PubblicazioneRepository
    ' '' ''        Try

    ' '' ''            Dim provvedimentoWS As New ParsecWebServices.Provvedimento
    ' '' ''            provvedimentoWS.Pubblica(Me.Pubblicazione)

    ' '' ''        Catch ex As Exception
    ' '' ''            successo = False
    ' '' ''        End Try

    ' '' ''        'Se è stata pubblicata online aggiorno lo stato
    ' '' ''        If successo Then
    ' '' ''            'Dim pubblicazioneDaAggiornare As ParsecPub.Pubblicazione = pubblicazioni.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
    ' '' ''            'If Not pubblicazioneDaAggiornare Is Nothing Then
    ' '' ''            '    pubblicazioneDaAggiornare.Pubblicato = True
    ' '' ''            '    pubblicazioni.SaveChanges()
    ' '' ''            'End If
    ' '' ''        End If
    ' '' ''        Me.Pubblicazione = Nothing
    ' '' ''    End If
    ' '' ''End Sub

    'Private Sub PubblicaLiquidazione()

    '    Dim pubblicazioni As New ParsecMES.AlboRepository
    '    Dim contatore As ParsecMES.ContatoreAlbo = pubblicazioni.GetContatoreCorrente
    '    Dim annoEsercizio As Integer = Now.Year
    '    If Not contatore Is Nothing Then
    '        annoEsercizio = pubblicazioni.GetContatoreCorrente.Anno
    '    End If
    '    pubblicazioni.Dispose()


    '    Dim localPathAlbo As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo") & annoEsercizio.ToString & "\LIQ\"

    '    Dim localPathDocumenti As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")

    '    '********************************************************************************************************
    '    'CREO LE CARTELLE SE NON ESISTONO
    '    '********************************************************************************************************
    '    If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo")) Then
    '        IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo"))
    '    End If

    '    If Not IO.Directory.Exists(ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo") & annoEsercizio.ToString) Then
    '        IO.Directory.CreateDirectory(ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo") & annoEsercizio.ToString)
    '    End If

    '    If Not IO.Directory.Exists(localPathAlbo) Then
    '        IO.Directory.CreateDirectory(localPathAlbo)
    '    End If
    '    '********************************************************************************************************

    '    Dim maxLengthAlbo As Integer = ParsecAdmin.WebConfigSettings.GetKey("MaxLengthAlbo")


    '    Dim utenteCollegato As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
    '    Dim messaggio As String = String.Empty

    '    '********************************************************************************************************
    '    'SALVO GLI ALLEGATI CHE NON SONO DI TIPO ALLEGATO GENERICO IN  ALBO/ANNO/LIQ
    '    '********************************************************************************************************

    '    Dim allegatiDaPubblicare As List(Of ParsecAtt.Allegato) = Me.Documento.Allegati.Where(Function(c) c.IdTipologiaAllegato <> 0).ToList

    '    For Each allegato In allegatiDaPubblicare
    '        Dim input As String = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti"), allegato.PercorsoRelativo, allegato.Nomefile)
    '        Dim output As String = localPathAlbo & allegato.Nomefile
    '        Me.CopiaDocumento(input, output)
    '    Next

    '    '********************************************************************************************************
    '    'ANNULLO LE PRECEDENTI LIQUIDAZIONI DA PUBBLICARE
    '    '********************************************************************************************************
    '    Dim liquidazioniAlbo As New ParsecMES.AlboLiquidazioneRepository

    '    Dim documenti As New ParsecAtt.DocumentoRepository
    '    Dim liquidazioniDocumento As New ParsecAtt.LiquidazioneRepository

    '    Dim elencoIdDocumenti = documenti.GetQuery.Where(Function(c) c.Codice = Me.Documento.Codice).Select(Function(c) c.Id).ToList
    '    Dim elencoIdLiquidazioni = liquidazioniDocumento.GetQuery.Where(Function(c) elencoIdDocumenti.Contains(c.IdDocumento)).Select(Function(c) c.Id).ToList

    '    liquidazioniDocumento.Dispose()
    '    documenti.Dispose()

    '    Dim liquidazioni As List(Of ParsecMES.AlboLiquidazione) = liquidazioniAlbo.GetQuery.Where(Function(c) elencoIdLiquidazioni.Contains(c.IdLiquidazione)).ToList
    '    For Each liq In liquidazioni
    '        liq.Stato = "M"
    '    Next
    '    liquidazioniAlbo.SaveChanges()
    '    '********************************************************************************************************


    '    Dim clienti As New ParsecAdmin.ClientRepository
    '    Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '    Dim codiceEnte As String = cliente.Identificativo
    '    clienti.Dispose()



    '    Dim contratto = Me.Documento.Allegati.Where(Function(c) c.IdTipologiaAllegato = 1).FirstOrDefault
    '    Dim curriculumVitae = Me.Documento.Allegati.Where(Function(c) c.IdTipologiaAllegato = 2).FirstOrDefault
    '    Dim elencoBeneficiari = Me.Documento.Allegati.Where(Function(c) c.IdTipologiaAllegato = 3).FirstOrDefault


    '    Dim nomefileContratto As String = String.Empty
    '    Dim nomefileCurriculumVitae As String = String.Empty
    '    Dim nomefilEelencoBeneficiari As String = String.Empty

    '    If Not contratto Is Nothing Then
    '        nomefileContratto = contratto.Nomefile
    '    End If
    '    If Not curriculumVitae Is Nothing Then
    '        nomefileCurriculumVitae = curriculumVitae.Nomefile
    '    End If
    '    If Not elencoBeneficiari Is Nothing Then
    '        nomefilEelencoBeneficiari = elencoBeneficiari.Nomefile
    '    End If



    '    For Each liquidazione In Me.Documento.Liquidazioni

    '        '********************************************************************************************************
    '        'INSERISCO LA NUOVA LIQUIDAZIONE DA PUBBLICARE ON-LINE
    '        '********************************************************************************************************
    '        Dim liquadazioneDaPubblicare As New ParsecMES.AlboLiquidazione
    '        liquadazioneDaPubblicare.IdLiquidazione = liquidazione.Id
    '        liquadazioneDaPubblicare.IdUtente = utenteCollegato.Id
    '        liquadazioneDaPubblicare.Data = Now
    '        liquadazioneDaPubblicare.Pubblicato = False
    '        If Not contratto Is Nothing Then
    '            liquadazioneDaPubblicare.Contratto = contratto.Nomefile
    '        End If
    '        If Not curriculumVitae Is Nothing Then
    '            liquadazioneDaPubblicare.Cv = curriculumVitae.Nomefile
    '        End If
    '        If Not elencoBeneficiari Is Nothing Then
    '            liquadazioneDaPubblicare.ElencoBeneficiari = elencoBeneficiari.Nomefile
    '        End If
    '        liquidazioniAlbo.Add(liquadazioneDaPubblicare)
    '        liquidazioniAlbo.SaveChanges()

    '        '********************************************************************************************************
    '        If Me.AbilitatoAmministrazioneAperta And Me.Documento.Modello.PubblicazioneLiq Then

    '            Try
    '                Dim ws As New PubblicazioneAlbo.wsPubblicazioneAlbo
    '                ws.Timeout = -1

    '                Dim l = liquidazioniAlbo.GetById(liquadazioneDaPubblicare.Id)

    '                messaggio = ws.Pubblica(l.IdLiquidazione, l.Beneficiario, codiceEnte, l.CodiceFiscalePartitaIva, l.ImportoLiquidato, l.Modalita, l.ResponsabileUfficio, l.Norma, l.LinkDocumento, nomefileContratto, nomefileCurriculumVitae, nomefilEelencoBeneficiari)

    '                If String.IsNullOrEmpty(messaggio) Then

    '                    l.Pubblicato = True
    '                    liquidazioniAlbo.SaveChanges()

    '                    'ws.CancellaPubblicazioneLiquidazione(liquidazione.Id)

    '                    For Each allegato In allegatiDaPubblicare
    '                        Dim percorsoFile As String = localPathAlbo & allegato.Nomefile
    '                        If IO.File.Exists(percorsoFile) Then
    '                            Dim fi As New IO.FileInfo(percorsoFile)
    '                            Dim kbLength As Integer = fi.Length \ 1024
    '                            If utenteCollegato.SuperUser OrElse kbLength < maxLengthAlbo Then
    '                                messaggio = ws.PubblicaFilesLiquidazione(codiceEnte, annoEsercizio, IO.File.ReadAllBytes(percorsoFile), allegato.Nomefile)
    '                            End If
    '                        End If
    '                    Next
    '                End If

    '            Catch ex As Exception
    '                messaggio &= ex.Message
    '                Throw New ApplicationException(messaggio)
    '            End Try
    '        End If
    '    Next
    '    liquidazioniAlbo.Dispose()
    'End Sub

    'luca 01/07/2020
    '' ''Private Sub InserisciInModuloPubblicazioni(ByVal documento As ParsecAtt.Documento)

    '' ''    Dim modelli As New ParsecAtt.ModelliRepository
    '' ''    Dim modello = modelli.Where(Function(c) c.Id = documento.IdModello).FirstOrDefault
    '' ''    modelli.Dispose()

    '' ''    If modello Is Nothing Then
    '' ''        Exit Sub
    '' ''    End If
    '' ''    If Not modello.IdSezioneTrasparenza.HasValue Then
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim sezione = CType(modello.IdSezioneTrasparenza, ParsecPub.TipologiaSezioneTrasparente)

    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    Dim pubblicazioni As New ParsecPub.PubblicazioneRepository
    '' ''    Dim provvedimenti As New ParsecPub.ProvvedimentoRepository
    '' ''    Dim oldProvvedimento As ParsecPub.Provvedimento = Nothing

    '' ''    Dim data As DateTime = Now
    '' ''    Dim pubblicata As Boolean = False


    '' ''    '*********************************************************************************************************************
    '' ''    'DATI DELLA PUBBLICAZIONE
    '' ''    '*********************************************************************************************************************
    '' ''    Dim pubblicazione As New ParsecPub.Pubblicazione
    '' ''    'pubblicazione.titolo = Me.Documento.Oggetto
    '' ''    pubblicazione.dataCreazione = data
    '' ''    pubblicazione.DataOperazione = data

    '' ''    Dim documenti As New ParsecAtt.DocumentoRepository
    '' ''    Dim oldDoc = documenti.Where(Function(c) c.Codice = documento.Codice And c.LogStato = "M").OrderByDescending(Function(c) c.Id).FirstOrDefault
    '' ''    Dim oldPubblicazione As ParsecPub.Pubblicazione = Nothing
    '' ''    If Not oldDoc Is Nothing Then

    '' ''        oldPubblicazione = pubblicazioni.GetQuery.Where(Function(p) p.IdDocumento = oldDoc.Id And p.Stato Is Nothing And p.IdModulo = ParsecAdmin.TipoModulo.ATT And (p.idSezione = 56 Or p.idSezione = 55)).FirstOrDefault

    '' ''        'oldPubblicazione risulta nothing
    '' ''        'problema che si verifica se durante la ripubblicazione viene salvato l'atto ma non la pubblicazione (ad esempio se durante il salvataggio l'utente preme f5)
    '' ''        If oldPubblicazione Is Nothing Then
    '' ''            If oldDoc.NumeroRegistroPubblicazione.HasValue Then
    '' ''                Dim ids = documenti.GetQuery.Where(Function(c) c.Codice = documento.Codice And c.LogStato = "M").Select(Function(c) c.Id).ToList
    '' ''                oldPubblicazione = pubblicazioni.Where(Function(c) ids.Contains(c.IdDocumento) And c.Stato Is Nothing And c.IdModulo = 3 And (c.idSezione = 56 Or c.idSezione = 55)).OrderByDescending(Function(c) c.Id).FirstOrDefault
    '' ''            End If
    '' ''        End If


    '' ''    End If

    '' ''    pubblicazione.idSezione = sezione

    '' ''    Select Case documento.TipologiaDocumento
    '' ''        Case ParsecAtt.TipoDocumento.Delibera, ParsecAtt.TipoDocumento.Determina
    '' ''            pubblicazione.DescrizioneTipologiaWS = (documento.Data.Value.Year.ToString & " - " & documento.DescrizioneTipologia)

    '' ''        Case ParsecAtt.TipoDocumento.Decreto, ParsecAtt.TipoDocumento.Ordinanza
    '' ''            If sezione = ParsecPub.TipologiaSezioneTrasparente.ProvvedimentiDirigenti Then
    '' ''                pubblicazione.DescrizioneTipologiaWS = (documento.Data.Value.Year.ToString & " - " & documento.DescrizioneTipologia & " Dirigenziale")
    '' ''            Else
    '' ''                pubblicazione.DescrizioneTipologiaWS = (documento.Data.Value.Year.ToString & " - " & documento.DescrizioneTipologia & " Sindacale")
    '' ''            End If

    '' ''    End Select

    '' ''    pubblicazione.DataInizioPubblicazione = data
    '' ''    pubblicazione.DataFinePubblicazione = If(Now.DayOfYear = 1, New DateTime(Now.Year, 12, 31).AddYears(4), New DateTime(Now.Year, 12, 31).AddYears(5))

    '' ''    If Not oldPubblicazione Is Nothing Then
    '' ''        oldProvvedimento = provvedimenti.GetQuery.Where(Function(p) p.idPubblicazione = oldPubblicazione.Id).FirstOrDefault
    '' ''        pubblicazione.Codice = oldPubblicazione.Codice
    '' ''        oldPubblicazione.Stato = "M"
    '' ''        'pubblicazione.contenuto = oldPubblicazione.contenuto
    '' ''    Else
    '' ''        pubblicazione.Codice = pubblicazioni.GetNuovoCodice
    '' ''        'Dim contenutoDocRepository As New ParsecAdmin.ContenutoDocumentiRepository
    '' ''        'Dim contenutoDocumento As String = contenutoDocRepository.GetQuery.Where(Function(c) c.IdRifDocumento = documento.Id And c.IdRifModulo = ParsecAdmin.TipoModulo.ATT).Select(Function(c) c.Contenuto).FirstOrDefault
    '' ''        'contenutoDocRepository.Dispose()
    '' ''        'pubblicazione.contenuto = contenutoDocumento
    '' ''    End If


    '' ''    Dim descrizione As String = String.Empty


    '' ''    Dim tipologieRegistro As New ParsecAtt.TipologieRegistroRepository
    '' ''    Dim tipologiaRegistro As ParsecAtt.TipologiaRegistro = tipologieRegistro.GetById(documento.IdTipologiaRegistro)
    '' ''    tipologieRegistro.Dispose()

    '' ''    descrizione = tipologiaRegistro.Descrizione

    '' ''    'Select Case documento.TipologiaDocumento
    '' ''    '    Case ParsecAtt.TipoDocumento.Delibera
    '' ''    '        descrizione = "Delibera"
    '' ''    '    Case ParsecAtt.TipoDocumento.Determina
    '' ''    '        descrizione = "Determina"
    '' ''    '    Case ParsecAtt.TipoDocumento.Ordinanza
    '' ''    '        descrizione = "Ordinanza"
    '' ''    '    Case ParsecAtt.TipoDocumento.Decreto
    '' ''    '        descrizione = "Decreto"
    '' ''    'End Select



    '' ''    Dim pattern As String = "#[\u0000-\uFFFF]+#"
    '' ''    Dim rgx As New Regex(pattern)

    '' ''    Dim parametri As New ParsecAdmin.ParametriRepository
    '' ''    Dim parametroOmissis = parametri.GetByName("Omissis", ParsecAdmin.TipoModulo.ATT)
    '' ''    parametri.Dispose()


    '' ''    Dim oggettoDocumento As String = documento.Oggetto
    '' ''    If Not parametroOmissis Is Nothing Then
    '' ''        oggettoDocumento = rgx.Replace(documento.Oggetto, parametroOmissis.Valore)
    '' ''    End If



    '' ''    Dim oggetto = String.Format("{0} N. {1} del {2} Oggetto: {3}", descrizione, documento.ContatoreGenerale, documento.Data.Value.ToShortDateString, oggettoDocumento)


    '' ''    'todo testare aggiungendo allegati dal modulo pubblicazione

    '' ''    pubblicazione.IdDocumento = Me.Documento.Id
    '' ''    pubblicazione.IdModulo = ParsecAdmin.TipoModulo.ATT
    '' ''    pubblicazione.IdUtente = utenteCollegato.Id
    '' ''    pubblicazione.Pubblicato = False
    '' ''    pubblicazione.Stato = Nothing

    '' ''    'sovrascrivo o prendo dalla pubblicazione precedente ???????
    '' ''    pubblicazione.campoRicerca = oggetto

    '' ''    pubblicazioni.Add(pubblicazione)
    '' ''    pubblicazioni.SaveChanges()
    '' ''    '*********************************************************************************************************************


    '' ''    '*********************************************************************************************************************
    '' ''    'DATI DEL PROVVEDIMENTO
    '' ''    '*********************************************************************************************************************
    '' ''    Dim provvedimento As New ParsecPub.Provvedimento
    '' ''    'provvedimento.idAtto = documento.Id
    '' ''    provvedimento.idPubblicazione = pubblicazione.Id
    '' ''    If Not oldProvvedimento Is Nothing Then
    '' ''        provvedimento.spesaPrevista = oldProvvedimento.spesaPrevista
    '' ''        provvedimento.modalitaSelezione = oldProvvedimento.modalitaSelezione
    '' ''    End If

    '' ''    provvedimento.numeroAtto = documento.ContatoreGenerale
    '' ''    provvedimento.dataAtto = documento.Data

    '' ''    'sovrascrivo o prendo dalla pubblicazione precedente ?????????
    '' ''    provvedimento.oggetto = oggetto

    '' ''    provvedimenti.Add(provvedimento)
    '' ''    provvedimenti.SaveChanges()
    '' ''    '*********************************************************************************************************************

    '' ''    '*********************************************************************************************************************
    '' ''    'DOCUMENTO PRIMARIO
    '' ''    '*********************************************************************************************************************
    '' ''    Dim allegati As New ParsecPub.AllegatoRepository
    '' ''    Dim nomeFilePdf As String = IO.Path.GetFileNameWithoutExtension(documento.Nomefile) & ".pdf"
    '' ''    Dim annoEsercizio As String = Me.GetAnnoEsercizio(documento).ToString
    '' ''    Dim relativePath As String = String.Format("\{0}\", annoEsercizio)

    '' ''    Dim documentoPrimario As New ParsecPub.Allegato
    '' ''    documentoPrimario.idPubblicazione = pubblicazione.Id
    '' ''    documentoPrimario.descrizione = "" 'Da fare
    '' ''    documentoPrimario.nomefile = nomeFilePdf
    '' ''    documentoPrimario.idTipologiaAllegato = ParsecPub.TipoDocumentoAllegato.Primario 'Documento primario
    '' ''    documentoPrimario.PathRelativo = relativePath
    '' ''    documentoPrimario.data = Now
    '' ''    allegati.Add(documentoPrimario)
    '' ''    allegati.SaveChanges()

    '' ''    '*********************************************************************************************************************

    '' ''    '*********************************************************************************************************************
    '' ''    'CREO LA CARTELLA SE NON ESISTE.
    '' ''    '*********************************************************************************************************************
    '' ''    If Not IO.Directory.Exists(String.Format("{0}{1}", ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni"), annoEsercizio)) Then
    '' ''        IO.Directory.CreateDirectory(String.Format("{0}{1}", ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni"), annoEsercizio))
    '' ''    End If
    '' ''    '*********************************************************************************************************************


    '' ''    '*********************************************************************************************************************
    '' ''    'ALLEGATI ATTO
    '' ''    '*********************************************************************************************************************

    '' ''    Dim sourcePath As String = String.Empty
    '' ''    Dim destinationPath As String = String.Empty

    '' ''    Dim documentoSecondario As ParsecPub.Allegato = Nothing

    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
    '' ''    percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)


    '' ''    Dim allegatiAtto = documenti.GetAllegati(documento.Id).Where(Function(c) c.Pubblicato = True).ToList

    '' ''    Dim nomefileAllegato As String = String.Empty

    '' ''    For Each all In allegatiAtto

    '' ''        If Not String.IsNullOrEmpty(all.NomeFileFirmato) Then
    '' ''            nomefileAllegato = all.NomeFileFirmato
    '' ''        Else
    '' ''            nomefileAllegato = all.Nomefile
    '' ''        End If
    '' ''        documentoSecondario = New ParsecPub.Allegato
    '' ''        documentoSecondario.idPubblicazione = pubblicazione.Id
    '' ''        documentoSecondario.descrizione = all.Oggetto 'Da fare
    '' ''        documentoSecondario.nomefile = nomefileAllegato
    '' ''        documentoSecondario.idTipologiaAllegato = ParsecPub.TipoDocumentoAllegato.Allegato 'Documento secondario
    '' ''        documentoSecondario.PathRelativo = relativePath
    '' ''        documentoSecondario.data = Now
    '' ''        allegati.Add(documentoSecondario)


    '' ''        sourcePath = String.Format("{0}{1}{2}", percorsoRoot, all.PercorsoRelativo, nomefileAllegato)
    '' ''        destinationPath = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni"), annoEsercizio, nomefileAllegato)

    '' ''        Me.CopiaDocumento(sourcePath, destinationPath)
    '' ''    Next

    '' ''    allegati.SaveChanges()
    '' ''    '*********************************************************************************************************************
    '' ''    allegati.Dispose()


    '' ''    pubblicazione.Allegati = pubblicazioni.GetAllegati(pubblicazione.Id)



    '' ''    If Me.AbilitatoAlbo Then
    '' ''        sourcePath = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAttiAlbo"), annoEsercizio, nomeFilePdf)
    '' ''        destinationPath = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni"), annoEsercizio, nomeFilePdf)
    '' ''        'Ho già il pdf
    '' ''        'Copio il file nella cartella delle pubblicazioni
    '' ''        Me.CopiaDocumento(sourcePath, destinationPath)

    '' ''    Else

    '' ''        '************************************************************************************************************
    '' ''        'CONVERTO IL DOCUMENTO ODT IN PDF E PUBBLICO
    '' ''        '************************************************************************************************************
    '' ''        Dim nomefile As String = IO.Path.GetFileNameWithoutExtension(documento.Nomefile)

    '' ''        Dim srcRemotePath = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomefile)
    '' ''        Dim destRemotePath = String.Format("{0}{1}{2}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAttiPubblicazioni"), annoEsercizio)

    '' ''        Dim openOfficeParameters As New ParsecAdmin.OpenOfficeParameters
    '' ''        Dim datiInput As New ParsecAdmin.DatiInput
    '' ''        datiInput.SrcRemotePath = srcRemotePath
    '' ''        datiInput.DestRemotePath = destRemotePath

    '' ''        Dim remotePathFiligrana As String = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & ParsecAdmin.WebConfigSettings.GetKey("PathRelativeFiligrana") & "Filigrana.png"

    '' ''        Dim fullPathFiligrana As String = ParsecAdmin.WebConfigSettings.GetKey("PathFiligrana") & "Filigrana.png"

    '' ''        'todo modificare in documentorepository
    '' ''        If IO.File.Exists(fullPathFiligrana) Then
    '' ''            datiInput.WatermarkUrl = remotePathFiligrana
    '' ''        End If

    '' ''        Dim dataPdf As String = openOfficeParameters.CreateDataSourceForPdf(datiInput)

    '' ''        'luca 01/07/2020
    '' ''        '' ''Me.Pubblicazione = pubblicazione

    '' ''        Me.ProvvedimentoPubblicato = pubblicata

    '' ''        'luca 01/07/2020
    '' ''        '' ''ParsecUtility.Utility.RegistraTimerEseguiConversionePdf(dataPdf, Me.eseguiPubblicazioneOnline.ClientID, True, False)

    '' ''        '************************************************************************************************************

    '' ''    End If


    '' ''    Dim provvedimentoWS As New ParsecWebServices.Provvedimento
    '' ''    provvedimentoWS.Pubblica(pubblicazione)

    '' ''    Try
    '' ''        pubblicazioni.Dispose()
    '' ''        documenti.Dispose()
    '' ''        provvedimenti.Dispose()
    '' ''    Catch ex As Exception

    '' ''    End Try



    '' ''End Sub



    Private Sub Search()
        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim filtro As ParsecAtt.FiltroDocumento = Me.GetFiltro
        Me.Documenti = documenti.GetView(filtro)
        documenti.Dispose()
        Me.DocumentiGridView.Rebind()
    End Sub

    'Private Function GetFiltro() As ParsecAtt.FiltroDocumento
    '    Dim filtro As New ParsecAtt.FiltroDocumento
    '    filtro.Oggetto = Me.OggettoTextBox.Text
    '    If Me.TipologiaProceduraApertura <> ParsecAtt.TipoProcedura.ModificaAmministrativa Then
    '        'filtro.EscludiDocumentiNumerati = True
    '        'filtro.EscludiDocumentiPubblicati = True
    '    End If
    '    filtro.DataDocumento = Me.DataTextBox.SelectedDate
    '    filtro.IdTipologiaDocumento = CInt(Me.TipologiaDocumento) ' Me.TipologieDocumentoComboBox.SelectedValue
    '    If Me.ModelliComboBox.SelectedIndex <> 0 Then
    '        filtro.IdModello = CInt(Me.ModelliComboBox.SelectedValue)
    '    End If

    '    If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
    '        filtro.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
    '    End If

    '    If Not String.IsNullOrEmpty(Me.NumeroAttoTextBox.Text) Then
    '        filtro.ContatoreGenerare = CInt(Me.NumeroAttoTextBox.Text)
    '    End If

    '    Return filtro
    'End Function

    Private Function GetFiltro() As ParsecAtt.FiltroDocumento
        Dim filtro As New ParsecAtt.FiltroDocumento
        filtro.Oggetto = Me.OggettoTextBox.Text
        If Me.TipologiaProceduraApertura <> ParsecAtt.TipoProcedura.ModificaAmministrativa Then
            filtro.ApplicaAbilitazione = True

            'filtro.EscludiDocumentiNumerati = True
            'filtro.EscludiDocumentiPubblicati = True
        End If
        filtro.DataDocumento = Me.DataTextBox.SelectedDate
        filtro.IdTipologiaDocumento = CInt(Me.TipologiaDocumento) ' Me.TipologieDocumentoComboBox.SelectedValue
        If Me.ModelliComboBox.SelectedIndex <> 0 Then
            filtro.IdModello = CInt(Me.ModelliComboBox.SelectedValue)
        End If

        If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
            filtro.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.NumeroAttoTextBox.Text) Then
            filtro.ContatoreGenerare = CInt(Me.NumeroAttoTextBox.Text)
        End If


        Return filtro
    End Function


    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Me.AdvancedSearch()
    End Sub

    Protected Sub RipristinaFiltroInizialeImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles RipristinaFiltroInizialeImageButton.Click
        ' Me.RipristinaFiltroInizialeImageButton.Enabled = False
        Me.Documenti = Nothing
        Me.DocumentiGridView.Rebind()
    End Sub

    Private Sub AdvancedSearch()
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/RicercaAttoPage.aspx"
        Dim queryString As New Hashtable
        Select Case Me.TipologiaProceduraApertura
            Case ParsecAtt.TipoProcedura.CambioModello, ParsecAtt.TipoProcedura.Classificazione, ParsecAtt.TipoProcedura.Pubblicazione, ParsecAtt.TipoProcedura.AggiungiDatiContabili
                queryString.Add("Tipo", CInt(Me.TipologiaDocumento))
            Case ParsecAtt.TipoProcedura.Numerazione
                queryString.Add("Tipo", CInt(Me.TipologiaDocumentoApertura))
            Case Else
                queryString.Add("Tipo", CInt(Me.TipologiaDocumento))
                'GESTIONE PER TIPOLOGIA DI DOCUMENTO 
                ' queryString.Add("Abilita", 1)
        End Select
        queryString.Add("obj", Me.AggiornaAttiImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 630, 600, queryString, False)
    End Sub

#End Region

#Region "SCRIPT MARCATURA OMISSIS"

    Private Sub RegistraMarcaturaTesto()
        Dim script As New StringBuilder
        script.AppendLine("<script language='javascript'>")


        script.AppendLine("var pos ;")

        script.AppendLine("function GetSelection (target)")
        script.AppendLine("{")

        script.AppendLine("pos = { start: 0, end: 0 };")
        script.AppendLine("if (typeof target.selectionStart == 'number'  && typeof target.selectionEnd == 'number') {")
        script.AppendLine("pos.start = target.selectionStart;")
        script.AppendLine("pos.end = target.selectionEnd;")
        script.AppendLine("} else if (document.selection) {")
        script.AppendLine("var bookmark = document.selection.createRange().getBookmark();")
        script.AppendLine("var sel = target.createTextRange();")
        script.AppendLine("var bfr = sel.duplicate();")
        script.AppendLine(" sel.moveToBookmark(bookmark);")
        script.AppendLine("bfr.setEndPoint('EndToStart', sel);")
        script.AppendLine("pos.start = bfr.text.length;")
        script.AppendLine("pos.end = pos.start + sel.text.length;")
        script.AppendLine("}")
        script.AppendLine("return pos;")
        script.AppendLine("}")

        script.AppendLine("function Racchiudi3() ")
        script.AppendLine("{")
        script.AppendLine("var s;")
        script.AppendLine("var txtBox = document.getElementById('" & Me.OggettoTextBox.ClientID & "');")
        script.AppendLine("if (txtBox.value) {")

        script.AppendLine("GetSelection(txtBox);")

        script.AppendLine("s = txtBox.value.substring (0, pos.start) + '#' + txtBox.value.substring  (pos.start, pos.end) + '#' + txtBox.value.substring (pos.end);")
        script.AppendLine("txtBox.value = s;")


        script.AppendLine("}")
        script.AppendLine("}")

        script.AppendLine("function Racchiudi() {")
        script.AppendLine(" var textarea = document.getElementById('" & Me.OggettoTextBox.ClientID & "');")
        script.AppendLine(" if ('selectionStart' in textarea) {")
        script.AppendLine("     if (textarea.selectionStart != textarea.selectionEnd) {")
        script.AppendLine("         var newText = textarea.value.substring (0, textarea.selectionStart) + '#' + textarea.value.substring(textarea.selectionStart, textarea.selectionEnd) + '#' + textarea.value.substring (textarea.selectionEnd);")
        script.AppendLine("         textarea.value = newText;")
        script.AppendLine("     }")
        script.AppendLine(" }")
        script.AppendLine(" else {")
        script.AppendLine("     var textRange = document.selection.createRange();")
        script.AppendLine("     var rangeParent = textRange.parentElement ();")
        script.AppendLine("     if (rangeParent === textarea) {")
        script.AppendLine("         textRange.text = '#' + textRange.text + '#';")
        script.AppendLine("     }")
        script.AppendLine(" }")
        script.AppendLine("}")




        script.AppendLine("</script>")
        Me.Header.Controls.Add(New LiteralControl(script.ToString))
    End Sub

#End Region

#Region "SCRIPT PARSECOPENOFFICE"

    Private Sub VisualizzaDocumento(ByVal nomeFile As String, annoEsercizio As Integer, ByVal enabled As Boolean)
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim datiInput As ParsecAdmin.DatiInput
        Dim pathDownload = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomeFile)
        If IO.Path.GetExtension(nomeFile).ToLower <> ".odt" Then
            datiInput = New ParsecAdmin.DatiInput With {.Path = pathDownload, .ShowWindow = False, .Enabled = False, .FunctionName = "OpenGenericDocument"}
        Else
            datiInput = New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = "ViewDocument"}
        End If
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

    Private Sub VisualizzaCopiaDocumento(ByVal nomeFile As String, annoEsercizio As Integer, ByVal enabled As Boolean)
        Dim originale As String = ParsecAdmin.WebConfigSettings.GetKey("PrefissoSezioneOriginale")
        Dim copia As String = ParsecAdmin.WebConfigSettings.GetKey("PrefissoSezioneCopiaConforme")
        Dim shadow As String = String.Empty
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("nome_sezione_shadow", ParsecAdmin.TipoModulo.ATT)
        If Not parametro Is Nothing Then
            shadow = parametro.Valore
        End If
        Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        Dim pathDownload = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, nomeFile)
        Dim datiInput As New ParsecAdmin.DatiInput With {.SrcRemotePath = pathDownload, .ShowWindow = True, .Enabled = enabled, .FunctionName = "ViewDocument", .PrefixCopy = copia, .PrefixOriginal = originale, .Shadow = shadow}
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

    Private Sub RegistraParsecOpenOffice()
        Dim script As String = ParsecAdmin.OpenOfficeParameters.RegistraParsecOpenOffice
        Me.MainPage.RegisterComponent(script)
    End Sub

    Private Sub RegistraScriptOpenOffice()

        If Not Me.Documento Is Nothing Then
            If Not String.IsNullOrEmpty(Me.Documento.DataSource) Then

                Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

                'Dim s As String = IO.File.ReadAllText("D:\Sviluppo_4.0\Sep\4.0\sepWeb\sep\PerMarioNardo.txt")
                'Me.Documento.DataSource = s.Replace(vbCrLf, "")

                Dim c = System.Text.ASCIIEncoding.Default.GetByteCount(Me.Documento.DataSource)

                'UTILIZZO L'APPLET
                If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                    ParsecUtility.Utility.RegistraTimerElaborazioneParsecOpenDocument(Me.Documento.DataSource, Me.salvaContenutoButton.ClientID, Me.documentContentHidden.ClientID, False, False)
                    'ParsecUtility.Utility.RegistraScriptElaborazioneParsecOpenDocument(Me.Documento.DataSource, Me.salvaContenutoButton.ClientID, False, False)
                Else
                    'UTILIZZO IL SOCKET  
                    ParsecUtility.Utility.EseguiServerComunicatorService(Me.Documento.DataSource, True, AddressOf Me.SalvaContenuto)
                End If

                ' Me.Documento.DataSource = ""
            End If
        End If
    End Sub

#End Region

#Region "GESTIONE FIRMA DIGITALE"

    Private Sub RegistraParsecDigitalSign()
        Dim script As String = ParsecAdmin.SignParameters.RegistraParsecDigitalSign
        Me.MainPage.RegisterComponent(script)
    End Sub

#End Region

#Region "GESTIONE CLASSIFICAZIONI"

    Protected Sub ClassificazioniGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles ClassificazioniGridView.ItemCommand
        If e.CommandName = "Delete" Then
            Me.EliminaClassificazione(e.Item)
        End If
    End Sub

    Private Sub EliminaClassificazione(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim classificazione As ParsecAtt.DocumentoClassificazione = Nothing
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("IdClassificazione")
        classificazione = Me.Classificazioni.Where(Function(c) c.IdClassificazione = id).FirstOrDefault
        If Not classificazione Is Nothing Then
            Me.Classificazioni.Remove(classificazione)
        End If
    End Sub

    Protected Sub TrovaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaClassificazioneImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaClassificazionePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaClassificazioneImageButton.ClientID)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("tipoSelezione", 0) 'singola
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
    End Sub

    Protected Sub AggiornaClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaClassificazioneImageButton.Click
        If Not Session("ClassificazioniSelezionate") Is Nothing Then
            Dim classificazioniSelezionate As List(Of ParsecAdmin.TitolarioClassificazione) = Session("ClassificazioniSelezionate")
            Dim idClassificazione As Integer = classificazioniSelezionate.First.Id
            Dim classificazioneCompleta As String = (New ParsecAdmin.TitolarioClassificazioneRepository).GetCodiciClassificazione2(idClassificazione, 1) & " " & classificazioniSelezionate.First.Descrizione
            Me.ClassificazioneTextBox.Text = classificazioneCompleta
            Me.IdClassificazioneTextBox.Text = idClassificazione.ToString
            Session("ClassificazioniSelezionate") = Nothing
        End If
    End Sub

    Protected Sub AggiungiClassificazioneImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiClassificazioneImageButton.Click
        If Not String.IsNullOrEmpty(Me.IdClassificazioneTextBox.Text) Then
            Dim classificazione As New ParsecAtt.DocumentoClassificazione
            'classificazione.IdDocumento = Me.Documento.Id
            classificazione.Descrizione = Me.ClassificazioneTextBox.Text
            classificazione.Note = Me.AnnotazioniTextBox.Text
            classificazione.IdClassificazione = CInt(Me.IdClassificazioneTextBox.Text)
            Dim exist As Boolean = Not Me.Classificazioni.Where(Function(c) c.IdClassificazione = CInt(Me.IdClassificazioneTextBox.Text)).FirstOrDefault Is Nothing
            If Not exist Then
                Me.Classificazioni.Add(classificazione)
            End If
            Me.ClassificazioneTextBox.Text = String.Empty
            Me.IdClassificazioneTextBox.Text = String.Empty
            Me.AnnotazioniTextBox.Text = String.Empty
        End If

    End Sub

#End Region

#Region "GESTIONE SEDUTA E PRESENZE"

    Protected Sub PresenzeGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles PresenzeGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim presenza As ParsecAtt.DocumentoPresenza = CType(e.Item.DataItem, ParsecAtt.DocumentoPresenza)
            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("PresenteCheckBox"), CheckBox)
            chk.Checked = presenza.Presente

           
            chk.Enabled = (Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Nuovo OrElse Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Modifica OrElse Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.Numerazione OrElse ParsecAtt.TipoProcedura.ModificaAmministrativa)
        End If
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        ' CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Dim item = CType(CType(sender, CheckBox).NamingContainer, GridItem)
        Dim dataItem As GridDataItem = CType(item, GridDataItem)
        Dim id As Integer = CInt(dataItem("IdStruttura").Text)
        Me.Presenze.Where(Function(c) c.IdStruttura = id).FirstOrDefault.Presente = CType(sender, CheckBox).Checked
    End Sub

    Protected Sub TrovaSedutaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaSedutaImageButton.Click
        If Me.ModelliComboBox.SelectedIndex <> 0 Then
            Dim modelli As New ParsecAtt.ModelliRepository
            Dim modello As ParsecAtt.Modello = modelli.GetById(CInt(Me.ModelliComboBox.SelectedValue))
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/RicercaSedutaPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("obj", Me.AggiornaSedutaImageButton.ClientID)
            queryString.Add("filtro", modello.IdTipologiaSeduta)
            'Dim parametriPagina As New Hashtable
            'parametriPagina.Add("tipoSelezione", 0) 'singola
            'ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 800, 400, queryString, False)
            modelli.Dispose()
        Else
            ParsecUtility.Utility.MessageBox("E' necessario selezionare un modello di documento", False)
        End If
    End Sub

    Private Function GetNuovaGestionePresenze() As Boolean
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("GestionePresenzeSeduta")
        parametri.Dispose()

        Dim visualizzaPulsante As Boolean = False
        If Not parametro Is Nothing Then
            If parametro.Valore = "1" Then
                Return True
            End If
        End If
        Return False
    End Function

    Protected Sub AggiornaSedutaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSedutaImageButton.Click
        If Not ParsecUtility.SessionManager.Seduta Is Nothing Then
            Dim seduta As ParsecAtt.Seduta = ParsecUtility.SessionManager.Seduta
            Me.SedutaTextBox.Text = String.Format("di {0} del {1}", seduta.DescrizioneTipologiaSeduta, String.Format("{0:dd/MM/yyyy}", seduta.DataConvocazione))
            Me.IdSedutaTextBox.Text = seduta.Id.ToString
            ParsecUtility.SessionManager.Seduta = Nothing

            Dim nuovaGestionePresenze As Boolean = Me.GetNuovaGestionePresenze()

            If nuovaGestionePresenze Then
                '***************************************************************************************************************
                'RECUPERO LE PRESENZE DALLA SEDUTA SELEZIONATA
                '***************************************************************************************************************
                Dim presenze As New ParsecAtt.DocumentoPresenzaRepository
                Me.Presenze = presenze.GetViewSeduta(New ParsecAtt.FiltroPresenzaSeduta With {.IdSeduta = seduta.Id, .IncludiAnnullate = True})
                presenze.Dispose()
                '***************************************************************************************************************
            Else

                '***************************************************************************************************************
                'RECUPERO LE PRESENZE DALL'ORGANIGRAMMA
                '***************************************************************************************************************
                Dim tipoSeduta As ParsecAtt.TipologiaOrganoDeliberante = CType(seduta.IdTipologiaSeduta, ParsecAtt.TipologiaOrganoDeliberante)

                Dim valore As String = ""
                Select Case tipoSeduta
                    Case ParsecAtt.TipologiaOrganoDeliberante.GiuntaComunale
                        valore = "codStrutturaRuoloAssessore"
                    Case ParsecAtt.TipologiaOrganoDeliberante.ConsiglioComunale
                        valore = "codStrutturaRuoloConsigliere"
                End Select

                If Not String.IsNullOrEmpty(valore) Then
                    Dim parametri As New ParsecAdmin.ParametriRepository
                    Dim parametro As ParsecAdmin.Parametri = parametri.GetByName(valore, ParsecAdmin.TipoModulo.SEP)
                    parametri.Dispose()

                    Dim presenze As New ParsecAtt.DocumentoPresenzaRepository
                    Dim strutturaPadre As ParsecAdmin.Struttura = (New ParsecAdmin.StructureRepository).GetQuery.Where(Function(c) c.Codice = CInt(parametro.Valore) And c.LogStato Is Nothing).FirstOrDefault
                    Me.Presenze = presenze.GetView(New ParsecAtt.FiltroDocumentoPresenza With {.IdPadre = strutturaPadre.Id})
                    presenze.Dispose()
                End If
            End If
        End If
    End Sub

#End Region

#Region "GESTIONE SCANSIONE"

    Private Sub RegistraScansione()
        Dim script As String = ParsecAdmin.ScannerParameters.RegistraScansione
        Me.MainPage.RegisterComponent(script)
    End Sub



    Private Sub AvviaScansione()
        Dim scanner As New ParsecAdmin.ScannerParameters
        Dim data As String = scanner.CreaDataSource(New ParsecAdmin.DatiCredenziali, New ParsecAdmin.DatiScansione With {.Duplex = Me.FronteRetroCheckBox.Checked, .UseUi = Me.VisualizzaUICheckBox.Checked})

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'UTILIZZO L'APPLET
        If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
            ParsecUtility.Utility.RegistraTimerEseguiScansione(data, Me.ScanUploadButton.ClientID, Me.infoScansioneHidden.ClientID, True, False)
        Else
            'UTILIZZO IL SOCKET  
            ParsecUtility.Utility.EseguiServerComunicatorService(data, True, AddressOf Me.NotificaInfoScansione, AddressOf Me.NotificaScansione)
        End If
    End Sub


    Private Sub NotificaInfoScansione(ByVal data As String)
        Me.infoScansioneHidden.Value = data
    End Sub

    Protected Sub ScansionaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ScansionaImageButton.Click
        Me.AvviaScansione()
    End Sub

    Private Sub NotificaScansione()
        If Not String.IsNullOrEmpty(Me.infoScansioneHidden.Value) Then
            Dim ds As New DataSet


            Dim ms As IO.MemoryStream = Nothing

            Dim search As String = "DATIBASE64:"
            Dim pos = Me.infoScansioneHidden.Value.IndexOf(search)
            Dim infoScansione As String = String.Empty
            Try
                If pos <> -1 Then
                    infoScansione = Me.infoScansioneHidden.Value.Substring(pos + search.Length, Me.infoScansioneHidden.Value.Length - pos - search.Length)
                Else
                    infoScansione = Me.infoScansioneHidden.Value
                End If

                'LO SCANNER OLIVETTI IN CASO DI ERRORE SCRIVE NELL'OUTPUT DEL COMPONENTE
                If String.IsNullOrEmpty(infoScansione.Trim) Then
                    Exit Sub
                End If

                ms = New IO.MemoryStream(System.Convert.FromBase64String(infoScansione))
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox("Si è verificato un errore durante la scansione:" & vbCrLf & infoScansione, False)
                Exit Sub
            End Try

            ds.ReadXml(ms)

            Dim nomeFileDigitalizzato As String = IO.Path.GetFileName(CStr(ds.Tables(0).Rows(0)("NomeFile")))
            Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

            If Not IO.Directory.Exists(pathRootTemp) Then
                IO.Directory.CreateDirectory(pathRootTemp)
            End If

            Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            Dim pathDownload As String = pathRootTemp & nomeFileDigitalizzato



            Dim allegato As New ParsecAtt.Allegato

            allegato.Id = -1

            If Me.Allegati.Count > 0 Then
                Dim allId = Me.Allegati.Min(Function(c) c.Id) - 1
                If allId < 0 Then
                    allegato.Id = allId
                End If
            End If

            allegato.Nomefile = nomeFileDigitalizzato
            allegato.NomeFileTemp = nomeFileDigitalizzato
            allegato.Pubblicato = False

            'If Not String.IsNullOrEmpty(Me.DescrizioneDocumentoTextBox.Text) Then
            allegato.Oggetto = Me.DescrizioneDocumentoTextBox.Text
            'Else
            '    allegato.Oggetto = "Allegato scansionato"
            'End If

            allegato.PercorsoRoot = pathRoot
            allegato.PercorsoRootTemp = pathRootTemp
            Dim impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
            allegato.Impronta = BitConverter.ToString(impronta).Replace("-", "")
            allegato.IdTipologiaAllegato = CInt(Me.TipologiaAllegatoComboBox.SelectedValue)
            '**********************************************************************************************************
            'Se il tipo di file scansionato è PDF
            '**********************************************************************************************************
            If nomeFileDigitalizzato.ToLower.EndsWith(".pdf") Then
                allegato.Scansionato = True
            End If
            '**********************************************************************************************************

            Me.Allegati.Add(allegato)
            Me.DescrizioneDocumentoTextBox.Text = String.Empty
        End If
    End Sub

    Protected Sub ScanUploadButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadButton.Click
        NotificaScansione()
    End Sub

#End Region

#Region "GESTIONE ALLEGATI"


    Protected Sub AggiungiDocumentoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoImageButton.Click


        If Me.AllegatoUpload.UploadedFiles.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Per inserire un allegato, è necessario specificarne il file relativo!", False)
            Exit Sub
        End If

        Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles.Cast(Of Telerik.Web.UI.UploadedFile).FirstOrDefault

        If String.IsNullOrEmpty(Me.DescrizioneDocumentoTextBox.Text.Trim) Then
            Me.DescrizioneDocumentoTextBox.Text = file.FileName
        End If


        If Not String.IsNullOrEmpty(file.FileName) Then

            Dim nomefile As String = Guid.NewGuid.ToString
            Dim filenameTemp As String = nomefile & "_" & file.FileName
            Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

            If Not IO.Directory.Exists(pathRootTemp) Then
                IO.Directory.CreateDirectory(pathRootTemp)
            End If

            Dim pathDownload As String = pathRootTemp & filenameTemp
            Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            Dim filename As String = file.FileName
            file.SaveAs(pathDownload)
            Dim allegato As New ParsecAtt.Allegato
            allegato.Id = -1

            If Me.Allegati.Count > 0 Then
                Dim allId = Me.Allegati.Min(Function(c) c.Id) - 1
                If allId < 0 Then
                    allegato.Id = allId
                End If
            End If

            allegato.Nomefile = filename
            allegato.NomeFileTemp = filenameTemp
            allegato.Oggetto = Me.DescrizioneDocumentoTextBox.Text
            allegato.PercorsoRoot = pathRoot
            allegato.PercorsoRootTemp = pathRootTemp
            allegato.Pubblicato = False
            allegato.IdTipologiaAllegato = CInt(Me.TipologiaAllegatoComboBox.SelectedValue)
            Dim impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownload)
            allegato.Impronta = BitConverter.ToString(impronta).Replace("-", "")
            If Me.Allegati.Where(Function(c) c.Impronta = allegato.Impronta).FirstOrDefault Is Nothing Then
                Me.Allegati.Add(allegato)
            Else
                ParsecUtility.Utility.MessageBox("Il file selezionato è stato già allegato!", False)
            End If

            Me.DescrizioneDocumentoTextBox.Text = String.Empty
        End If


    End Sub

    Private Sub DeleteDocument(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        '********************************************************************************
        'Modificata per utilizzo GridTemplateColumn
        'Dim selectedItem As GridDataItem = item.OwnerTableView.Items(item.ItemIndex)
        'Dim filename As String = selectedItem("Nomefile").Text
        '********************************************************************************
        Dim allegato As ParsecAtt.Allegato = Me.Allegati.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then
            Me.Allegati.Remove(allegato)
            If Me.SelectedItems.ContainsKey(allegato.Id) Then
                Me.SelectedItems.Remove(allegato.Id)
            End If


            'Se è un allegato temporaneo.
            If allegato.Id < 0 Then
                Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.NomeFileTemp
                If IO.File.Exists(pathDownload) Then
                    IO.File.Delete(pathDownload)
                End If
            End If
        End If
    End Sub



    Private Sub DownloadFile(ByVal item As Telerik.Web.UI.GridDataItem)
        '********************************************************************************
        'Modificata per utilizzo GridTemplateColumn
        'Dim selectedItem As GridDataItem = item.OwnerTableView.Items(item.ItemIndex)
        'Dim filename As String = selectedItem("Nomefile").Text
        '********************************************************************************
        Dim percorsoRoot As String = System.Configuration.ConfigurationManager.AppSettings("PathDocumenti")
        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

        Dim allegato As ParsecAtt.Allegato = Me.Allegati.Where(Function(c) c.Nomefile = filename).FirstOrDefault
        If Not allegato Is Nothing Then

            If Not allegato.IdFatturaElettronica.HasValue Then
                Dim pathDownload As String = String.Empty
                'Se è un allegato temporaneo.
                If allegato.Id < 0 Then
                    pathDownload = System.Configuration.ConfigurationManager.AppSettings("PathDocumentiTemp") & allegato.NomeFileTemp
                Else
                    percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                    pathDownload = percorsoRoot & allegato.PercorsoRelativo & allegato.Nomefile
                End If
                Dim file As New IO.FileInfo(pathDownload)
                If file.Exists Then
                    Session("AttachmentFullName") = file.FullName
                    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                    ParsecUtility.Utility.PageReload(pageUrl, False)
                    'Dim pageUrl As String = String.Empty
                    'If allegato.Id < 0 Then
                    '    pageUrl = ParsecAdmin.WebConfigSettings.GetKey("PathHTTPDocumentTemp") & file.Name & "?rnd=" & Now.Millisecond.ToString
                    'Else
                    '    pageUrl = ParsecAdmin.WebConfigSettings.GetKey("PathHTTPDocument") & allegato.PercorsoRelativo.Replace("\", "/") & file.Name & "?rnd=" & Now.Millisecond.ToString
                    'End If
                    'ParsecUtility.Utility.ShowPopup(pageUrl.Replace("//", "/").Replace("'", "\'"), 900, 700, Nothing, False)

                Else
                    ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
                End If

            Else
                'FATTURA
                Me.VisualizzaFattura(allegato.IdFatturaElettronica)

            End If

        End If
    End Sub

    Private Sub FirmaDocumento(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        If Not utenteCollegato.IdProviderFirma.HasValue Then
            Dim message As New StringBuilder
            message.AppendLine("L'utente corrente non è configurato correttamente!")
            message.AppendLine("E' necessario specificare il provider di firma.")
            ParsecUtility.Utility.MessageBox(message.ToString, False)
            Exit Sub
        End If

        Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As ParsecAtt.Allegato = Me.Allegati.Where(Function(c) c.Nomefile = filename).FirstOrDefault

        If Not allegato Is Nothing Then

            Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
            Dim percorsoRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

            Dim pathRelativeDocument As String = ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocument")
            Dim pathRelativeDocumentiTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathRelativeDocumentiTemp")

            Dim pathFileToSign As String = String.Empty
            Dim pathDownload As String = String.Empty
            'Se è un allegato temporaneo.
            If allegato.Id < 0 Then
                Dim fileNameToSign As String = String.Empty
                If String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
                    fileNameToSign = allegato.NomeFileTemp
                Else
                    fileNameToSign = allegato.NomeFileTemp & ".p7m"
                End If

                pathDownload = percorsoRootTemp & fileNameToSign 'allegato.NomeFileTemp
                pathFileToSign = ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl") & pathRelativeDocumentiTemp & fileNameToSign ' allegato.NomeFileTemp
            Else
                Dim fileNameToSign As String = String.Empty
                If String.IsNullOrEmpty(allegato.NomeFileFirmato) Then
                    fileNameToSign = allegato.Nomefile
                Else
                    fileNameToSign = allegato.NomeFileFirmato
                End If
                If percorsoRoot.EndsWith("\") Then
                    percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                End If

                pathDownload = percorsoRoot & allegato.PercorsoRelativo & fileNameToSign
                pathFileToSign = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), pathRelativeDocument, allegato.PercorsoRelativo.Replace("\", ""), fileNameToSign)


            End If

            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then

                Dim certificateId As String = utenteCollegato.IdCertificato
                Dim provider As ParsecAdmin.ProviderType = CType(utenteCollegato.IdProviderFirma, ParsecAdmin.ProviderType)

                Dim signParameters As New ParsecAdmin.SignParameters

                Dim functionName As String = "SignDocument"

                If file.Extension.ToLower = ".p7m" Then
                    functionName = "CoSignDocument"
                End If

                Dim datiInput As New ParsecAdmin.DatiFirma With {
                    .RemotePathToSign = pathFileToSign,
                    .Provider = provider,
                    .FunctionName = functionName,
                    .CertificateId = certificateId
                }

                Dim data As String = signParameters.CreaDataSource(datiInput)


                Dim parametriPagina As New Hashtable

                If file.Extension.ToLower = ".p7m" Then
                    parametriPagina.Add("PathNomeFileFirmato", pathDownload)
                Else

                    '********************************************************************************
                    'LA FIRMA DI UN DOCUMENTO ODT LO CONVERTE IN PDF
                    '********************************************************************************
                    If IO.Path.GetExtension(pathDownload).ToLower = ".odt" Then
                        pathDownload = pathDownload.Remove(pathDownload.Length - 4, 4) & ".pdf"
                    End If
                    '********************************************************************************
                    parametriPagina.Add("PathNomeFileFirmato", String.Format("{0}.p7m", pathDownload))
                End If

                parametriPagina.Add("NomeFile", allegato.Nomefile)


                ParsecUtility.SessionManager.ParametriPagina = parametriPagina

                'UTILIZZO L'APPLET
                If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
                    ParsecUtility.Utility.RegistraTimerEseguiFirma(data, Me.AggiornaFirmaDigitaleImageButton.ClientID, Me.signerOutputHidden.ClientID, False, False)
                Else
                    'UTILIZZO IL SOCKET  
                    ParsecUtility.Utility.EseguiServerComunicatorService(data, True, Sub(c) Me.signerOutputHidden.Value = c, AddressOf Me.AggiornaFirmaDigitale)
                End If

            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        Else
            'NIENTE
        End If

    End Sub


    Private Sub VisualizzaDocumentoP7M(ByVal item As Telerik.Web.UI.GridDataItem)

        Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        Dim percorsoRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        Dim nomeFile As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
        Dim allegato As ParsecAtt.Allegato = Me.Allegati.Where(Function(c) c.Nomefile = nomeFile).FirstOrDefault

        If Not allegato Is Nothing Then

            Dim pathDownload As String = String.Empty

            If allegato.Id < 0 Then

                Dim temp = allegato.NomeFileTemp
                '********************************************************************************
                'LA FIRMA DI UN DOCUMENTO ODT LO CONVERTE IN PDF
                '********************************************************************************
                If IO.Path.GetExtension(allegato.Nomefile).ToLower = ".odt" Then
                    temp = temp.Remove(temp.Length - 4, 4) & ".pdf"
                End If
                '********************************************************************************
                pathDownload = percorsoRootTemp & temp & ".p7m"

            Else
                If percorsoRoot.EndsWith("\") Then
                    percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                End If
                pathDownload = percorsoRoot & allegato.PercorsoRelativo & allegato.NomeFileFirmato
            End If

            Dim file As New IO.FileInfo(pathDownload)
            If file.Exists Then

                Session("AttachmentFullName") = file.FullName
                Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
                ParsecUtility.Utility.PageReload(pageUrl, False)

            Else
                ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
            End If
        Else
            'NIENTE
        End If
    End Sub


    Private Sub AggiornaFirmaDigitale()

        '****************************************************************
        'SE HO FIRMATO
        '****************************************************************
        Dim successo As Boolean = (Me.signerOutputHidden.Value = "OK")

        If successo Then
            Dim pathNomeFileFirmato As String = ParsecUtility.SessionManager.ParametriPagina("PathNomeFileFirmato")
            Dim filename As String = ParsecUtility.SessionManager.ParametriPagina("NomeFile")

            If IO.File.Exists(pathNomeFileFirmato) Then
                Dim allegato = Me.Allegati.Where(Function(c) c.Nomefile = filename).FirstOrDefault
                If Not allegato Is Nothing Then

                    '********************************************************************************
                    'LA FIRMA DI UN DOCUMENTO ODT LO CONVERTE IN PDF
                    '********************************************************************************
                    If IO.Path.GetExtension(allegato.Nomefile).ToLower = ".odt" Then
                        filename = filename.Remove(filename.Length - 4, 4) & ".pdf"
                    End If
                    '********************************************************************************
                    allegato.NomeFileFirmato = String.Format("{0}.p7m", filename)

                    'If allegato.Id < 0 Then
                    '    Dim temp = allegato.NomeFileTemp
                    '    '********************************************************************************
                    '    'LA FIRMA DI UN DOCUMENTO ODT LO CONVERTE IN PDF
                    '    '********************************************************************************
                    '    If IO.Path.GetExtension(allegato.Nomefile).ToLower = ".odt" Then
                    '        temp = temp.Remove(temp.Length - 4, 4) & ".pdf"
                    '        'allegato.NomeFileTemp = temp
                    '    End If
                    '    '********************************************************************************
                    '    allegato.NomeFileFirmato = String.Format("{0}.p7m", temp)

                    'Else
                    '    '********************************************************************************
                    '    'LA FIRMA DI UN DOCUMENTO ODT LO CONVERTE IN PDF
                    '    '********************************************************************************
                    '    If IO.Path.GetExtension(allegato.Nomefile).ToLower = ".odt" Then
                    '        filename = filename.Remove(filename.Length - 4, 4) & ".pdf"
                    '    End If
                    '    '********************************************************************************
                    '    allegato.NomeFileFirmato = String.Format("{0}.p7m", filename)
                    'End If

                End If
                Me.infoOperazioneHidden.Value = "Firma eseguita correttamente!"
            End If
        Else
            'NASCONDO IL PANNELLO MODALE
            Dim sb As New StringBuilder
            sb.AppendLine("<script>")
            sb.AppendLine("showUI = true;")
            sb.AppendLine("</script>")
            ParsecUtility.Utility.RegisterScript(sb, False)
        End If



        Me.signerOutputHidden.Value = String.Empty
        ParsecUtility.SessionManager.ParametriPagina = Nothing
    End Sub

    Protected Sub AggiornaFirmaDigitaleImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaFirmaDigitaleImageButton.Click
        Me.AggiornaFirmaDigitale()
    End Sub


    Protected Sub AllegatiGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles AllegatiGridView.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item

            Dim allegato As ParsecAtt.Allegato = CType(e.Item.DataItem, ParsecAtt.Allegato)



            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Delete").Controls(0), ImageButton)
                btn.ToolTip = "Elimina allegato"

                If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.ModificaParere Then
                    If Not String.IsNullOrEmpty(Me.ElencoIdAllegatiBloccati) Then
                        Dim elenco As String() = Me.ElencoIdAllegatiBloccati.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
                        If elenco.Contains(allegato.Id) Then
                            btn.ImageUrl = "~\images\vuoto.png"
                            btn.Attributes.Add("onclick", "return false;")
                            btn.ToolTip = ""
                        End If
                    End If
                End If

               
            End If

            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                Dim btn As ImageButton = CType(dataItem("Preview").Controls(0), ImageButton)

                If allegato.IdFatturaElettronica.HasValue Then
                    btn.ToolTip = "Visualizza Fattura..."
                    btn.ImageUrl = "~\images\xml_16.png"
                Else
                    btn.ToolTip = "Visualizza Allegato..."
                    'btn.ImageUrl = "~\images\xml_16.png"
                End If

            End If

            If Me.Allegati.Count > 0 Then
                Dim lbl As Label = CType(e.Item.FindControl("NumeratoreLabel"), Label)
                lbl.Text = (e.Item.ItemIndex + 1).ToString
            End If


            If TypeOf dataItem("Firma").Controls(0) Is ImageButton Then
                Dim btnFirma As ImageButton = CType(dataItem("Firma").Controls(0), ImageButton)
                'Dim ext As String = System.IO.Path.GetExtension(nomeFile).ToUpper
                'If String.IsNullOrEmpty(nomeFileFirmato) And estensioniConsentite.Contains(ext) Then
                '    btnFirma.ImageUrl = "~\images\firmaDocumento16.png"
                btnFirma.ToolTip = "Apponi firma digitale al documento."
                btnFirma.Attributes.Add("onclick", "showUI=false;")
                'Else
                '    btnFirma.ImageUrl = "~\images\vuoto.png"
                '    btnFirma.Attributes.Add("onclick", "return false;")
                '    btnFirma.ToolTip = "Documento firmato, o che non è possibile firmare digitalmente."
                'End If
            End If


            If TypeOf dataItem("SignedPreview").Controls(0) Is ImageButton Then
                Dim btnSignedPreview As ImageButton = CType(dataItem("SignedPreview").Controls(0), ImageButton)

                Dim nomeFileFirmato As String = dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("NomeFileFirmato")
                If Not String.IsNullOrEmpty(nomeFileFirmato) Then
                    btnSignedPreview.ImageUrl = "~\images\signedDocument16.png"
                    btnSignedPreview.ToolTip = "Visualizza documento firmato."
                Else
                    btnSignedPreview.ImageUrl = "~\images\vuoto.png"
                    btnSignedPreview.Attributes.Add("onclick", "return false;")
                    btnSignedPreview.ToolTip = ""
                End If

            End If





            Dim chk As CheckBox = CType(dataItem("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox)
            Dim id As String = allegato.Id

            If CararicaDaDB Then

                If allegato.Pubblicato Then
                    chk.Checked = True
                    dataItem.Selected = True
                    Me.SelectedItems.Add(id, True)
                End If
            Else


                If SelectedItems.ContainsKey(id) Then
                    'Seleziono la checkbox e la riga.
                    chk.Checked = Convert.ToBoolean(SelectedItems(id).ToString())
                    dataItem.Selected = True
                End If

            End If



            If e.Item.ItemIndex = (Me.Allegati.Count - 1) Then
                CararicaDaDB = False
            End If

        End If
    End Sub




    Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
        For Each dataItem As GridDataItem In AllegatiGridView.MasterTableView.Items
            Dim chk As CheckBox = CType(dataItem.FindControl("SelectCheckBox"), CheckBox)
            If chk.Enabled Then
                chk.Checked = headerCheckBox.Checked
                dataItem.Selected = headerCheckBox.Checked
            End If
        Next
        Me.SaveSelectedItems()
    End Sub

    Protected Sub AllegatiToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).NamingContainer, GridItem).Selected = CType(sender, CheckBox).Checked
        Me.SaveSelectedItems()
    End Sub

    Private Sub SaveSelectedItems()
        For Each item As GridItem In Me.AllegatiGridView.Items
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



    Protected Sub AllegatiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Me.DeleteDocument(e.Item)
            Case "Preview"
                Me.DownloadFile(e.Item)
            Case "Firma"
                Me.FirmaDocumento(e.Item)
            Case "SignedPreview"
                Me.VisualizzaDocumentoP7M(e.Item)
        End Select


    End Sub

    Private Sub AllegatiScansionatiGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles AllegatiGridView.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf AllegatiGridView_ItemPreRender
        End If
        If TypeOf e.Item Is GridHeaderItem Then
            e.Item.Style.Add("position", "relative")
            e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop)")
            e.Item.Style.Add("z-index", "99")
            e.Item.Style.Add("background-color", "White")
        End If
    End Sub

    Protected Sub AllegatiGridView_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, GridDataItem)("CheckBoxTemplateColumn").FindControl("SelectCheckBox"), CheckBox).Checked = CType(sender, GridDataItem).Selected
    End Sub


    Protected Sub AllegatiGridView_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles AllegatiGridView.PreRender
        Dim headerItem As GridHeaderItem = CType(Me.AllegatiGridView.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        If Me.AllegatiGridView.Items.Count > 0 Then
            CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Checked = (Me.AllegatiGridView.SelectedItems.Count = Me.AllegatiGridView.Items.Count)
        End If
        CType(headerItem.FindControl("SelectAllCheckBox"), CheckBox).Enabled = Me.AllegatiGridView.Items.Count > 0
    End Sub

#End Region

#Region "GESTIONE ALLEGATI PUBBLICAZIONI"

    'luca 01/07/2020
    '' ''Private Sub AvviaScansionePubblicazione(ByVal duplex As Boolean, ByVal useUI As Boolean, ByVal scanUploadButton As String)
    '' ''    Dim scanner As New ParsecAdmin.ScannerParameters
    '' ''    Dim data As String = scanner.CreaDataSource(New ParsecAdmin.DatiCredenziali, New ParsecAdmin.DatiScansione With {.Duplex = duplex, .UseUi = useUI})

    '' ''    Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''    'UTILIZZO L'APPLET
    '' ''    If Not utenteCollegato.TecnologiaClientSide.HasValue OrElse utenteCollegato.TecnologiaClientSide = 0 Then
    '' ''        ParsecUtility.Utility.RegistraTimerEseguiScansione(data, scanUploadButton, Me.infoScansioneHidden.ClientID, True, False)
    '' ''    Else
    '' ''        'UTILIZZO IL SOCKET  
    '' ''        ParsecUtility.Utility.EseguiServerComunicatorService(data, True, AddressOf Me.NotificaInfoScansione, AddressOf Me.NotificaScansionePubblicazione)
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub NotificaScansionePubblicazione()
    '' ''    If Not String.IsNullOrEmpty(Me.infoScansioneHidden.Value) Then
    '' ''        Dim ds As New DataSet


    '' ''        Dim ms As IO.MemoryStream = Nothing

    '' ''        Dim search As String = "DATIBASE64:"
    '' ''        Dim pos = Me.infoScansioneHidden.Value.IndexOf(search)
    '' ''        Dim infoScansione As String = String.Empty
    '' ''        Try
    '' ''            If pos <> -1 Then
    '' ''                infoScansione = Me.infoScansioneHidden.Value.Substring(pos + search.Length, Me.infoScansioneHidden.Value.Length - pos - search.Length)
    '' ''            Else
    '' ''                infoScansione = Me.infoScansioneHidden.Value
    '' ''            End If

    '' ''            'LO SCANNER OLIVETTI IN CASO DI ERRORE SCRIVE NELL'OUTPUT DEL COMPONENTE
    '' ''            If String.IsNullOrEmpty(infoScansione.Trim) Then
    '' ''                Exit Sub
    '' ''            End If

    '' ''            ms = New IO.MemoryStream(System.Convert.FromBase64String(infoScansione))
    '' ''        Catch ex As Exception
    '' ''            ParsecUtility.Utility.MessageBox("Si è verificato un errore durante la scansione:" & vbCrLf & infoScansione, False)
    '' ''            Exit Sub
    '' ''        End Try

    '' ''        ds.ReadXml(ms)

    '' ''        Dim nomeFileDigitalizzato As String = IO.Path.GetFileName(CStr(ds.Tables(0).Rows(0)("NomeFile")))
    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
    '' ''        Dim pathDownload As String = pathRootTemp & nomeFileDigitalizzato



    '' ''        Dim allegato As New ParsecAdmin.AllegatoPubblicazione
    '' ''        allegato.Id = -1
    '' ''        If Me.AllegatiPubblicazione.Count > 0 Then
    '' ''            Dim allId = Me.AllegatiPubblicazione.Min(Function(c) c.Id) - 1
    '' ''            If allId < 0 Then
    '' ''                allegato.Id = allId
    '' ''            End If
    '' ''        End If



    '' ''        allegato.NomeFileOriginale = nomeFileDigitalizzato
    '' ''        allegato.Nomefile = nomeFileDigitalizzato

    '' ''        Dim primario As Boolean

    '' ''        If Me.BandiGareContrattiPanel.Visible Then
    '' ''            'If Not String.IsNullOrEmpty(Me.DescrizioneDocumentoBandoGaraTextBox.Text) Then
    '' ''            allegato.Descrizione = Me.DescrizioneDocumentoBandoGaraTextBox.Text
    '' ''            'Else
    '' ''            '    allegato.Descrizione = "Allegato scansionato"
    '' ''            'End If

    '' ''            Me.DescrizioneDocumentoBandoGaraTextBox.Text = String.Empty
    '' ''            primario = Me.DocumentoPrimarioBandoGaraRadioButton.Checked
    '' ''        End If

    '' ''        If Me.PubblicazioneGenericaPanel.Visible Then
    '' ''            'If Not String.IsNullOrEmpty(Me.DescrizioneDocumentoPubblicazioneGenericaTextBox.Text) Then
    '' ''            allegato.Descrizione = Me.DescrizioneDocumentoPubblicazioneGenericaTextBox.Text
    '' ''            'Else
    '' ''            '    allegato.Descrizione = "Allegato scansionato"
    '' ''            'End If

    '' ''            Me.DescrizioneDocumentoPubblicazioneGenericaTextBox.Text = String.Empty
    '' ''            primario = Me.DocumentoPrimarioPubblicazioneGenericaRadioButton.Checked
    '' ''        End If

    '' ''        allegato.PathRelativo = "\" & Now.Year & "\"


    '' ''        If primario Then
    '' ''            allegato.IdTipologiaAllegato = 1  'Primario
    '' ''            allegato.TipoDocumentoAllegato = "Primario"
    '' ''        Else
    '' ''            allegato.IdTipologiaAllegato = 2 'Allegato
    '' ''            allegato.TipoDocumentoAllegato = "Allegato"
    '' ''        End If

    '' ''        '**********************************************************************************************************
    '' ''        'Se il tipo di file scansionato è PDF
    '' ''        '**********************************************************************************************************
    '' ''        'If nomeFileDigitalizzato.ToLower.EndsWith(".pdf") Then
    '' ''        '    allegato.Scansionato = True
    '' ''        'End If
    '' ''        '**********************************************************************************************************

    '' ''        Me.AllegatiPubblicazione.Add(allegato)

    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub DeleteAllegatoPubblicazione(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")
    '' ''    '********************************************************************************
    '' ''    'Modificata per utilizzo GridTemplateColumn
    '' ''    'Dim selectedItem As GridDataItem = item.OwnerTableView.Items(item.ItemIndex)
    '' ''    'Dim filename As String = selectedItem("Nomefile").Text
    '' ''    '********************************************************************************
    '' ''    Dim allegato As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.Nomefile = filename).FirstOrDefault
    '' ''    If Not allegato Is Nothing Then
    '' ''        Me.AllegatiPubblicazione.Remove(allegato)
    '' ''        'Se è un allegato temporaneo.
    '' ''        If allegato.Id < 0 Then
    '' ''            Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegato.Nomefile
    '' ''            If IO.File.Exists(pathDownload) Then
    '' ''                IO.File.Delete(pathDownload)
    '' ''            End If
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub DownloadAllegatoPubblicazione(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    '********************************************************************************
    '' ''    'Modificata per utilizzo GridTemplateColumn
    '' ''    'Dim selectedItem As GridDataItem = item.OwnerTableView.Items(item.ItemIndex)
    '' ''    'Dim filename As String = selectedItem("Nomefile").Text
    '' ''    '********************************************************************************
    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")


    '' ''    Dim filename As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Nomefile")

    '' ''    Dim allegato As ParsecAdmin.AllegatoPubblicazione = Me.AllegatiPubblicazione.Where(Function(c) c.Nomefile = filename).FirstOrDefault
    '' ''    If Not allegato Is Nothing Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If allegato.Id < 0 Then
    '' ''            pathDownload = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & allegato.Nomefile
    '' ''        Else
    '' ''            pathDownload = percorsoRoot & allegato.PathRelativo & allegato.Nomefile
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato selezionato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub AggiungiAllegatoPubblicazione(ByVal allegatoUpload As Telerik.Web.UI.RadAsyncUpload, ByVal descrizioneDocumentoTextBox As Telerik.Web.UI.RadTextBox, ByVal primario As Boolean)

    '' ''    If allegatoUpload.UploadedFiles.Count = 0 Then
    '' ''        ParsecUtility.Utility.MessageBox("Per inserire un allegato, è necessario specificarne il file relativo!", False)
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim file As Telerik.Web.UI.UploadedFile = allegatoUpload.UploadedFiles.Cast(Of Telerik.Web.UI.UploadedFile).FirstOrDefault

    '' ''    If String.IsNullOrEmpty(descrizioneDocumentoTextBox.Text.Trim) Then
    '' ''        descrizioneDocumentoTextBox.Text = file.FileName
    '' ''    End If

    '' ''    If Not String.IsNullOrEmpty(file.FileName) Then

    '' ''        Dim nomefile As String = Guid.NewGuid.ToString
    '' ''        Dim filenameTemp As String = nomefile & "_" & file.FileName

    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathDownload As String = pathRootTemp & filenameTemp

    '' ''        file.SaveAs(pathDownload)

    '' ''        Dim pathRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")

    '' ''        Dim allegato As New ParsecAdmin.AllegatoPubblicazione

    '' ''        allegato.Id = -1

    '' ''        If Me.AllegatiPubblicazione.Count > 0 Then
    '' ''            Dim allId = Me.AllegatiPubblicazione.Min(Function(c) c.Id) - 1
    '' ''            If allId < 0 Then
    '' ''                allegato.Id = allId
    '' ''            End If
    '' ''        End If

    '' ''        allegato.NomeFileOriginale = file.FileName
    '' ''        allegato.Nomefile = filenameTemp
    '' ''        allegato.Descrizione = descrizioneDocumentoTextBox.Text
    '' ''        allegato.PathRelativo = "\" & Now.Year & "\"

    '' ''        If primario Then
    '' ''            allegato.IdTipologiaAllegato = 1  'Primario
    '' ''            allegato.TipoDocumentoAllegato = "Primario"
    '' ''        Else
    '' ''            allegato.IdTipologiaAllegato = 2 'Allegato
    '' ''            allegato.TipoDocumentoAllegato = "Allegato"
    '' ''        End If

    '' ''        Me.AllegatiPubblicazione.Add(allegato)
    '' ''        descrizioneDocumentoTextBox.Text = String.Empty

    '' ''    End If

    '' ''End Sub


    '***************************************************************************************************************************************************
    'BANDO DI GARA
    '***************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub ScanUploadBandoGaraButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadBandoGaraButton.Click
    '' ''    Me.NotificaScansionePubblicazione()
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub ScansionaBandoGaraImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaBandoGaraImageButton.Click
    '' ''    Me.AvviaScansionePubblicazione(Me.FronteRetroBandoGaraCheckBox.Checked, Me.VisualizzaUIBandoGaraCheckBox.Checked, Me.ScanUploadBandoGaraButton.ClientID)
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiDocumentoBandoGaraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoBandoGaraImageButton.Click
    '' ''    Me.AggiungiAllegatoPubblicazione(Me.AllegatoBandoGaraUpload, Me.DescrizioneDocumentoBandoGaraTextBox, Me.DocumentoPrimarioBandoGaraRadioButton.Checked)
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AllegatiBandoGaraGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiBandoGaraGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.DeleteAllegatoPubblicazione(e.Item)
    '' ''        Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub

    '***************************************************************************************************************************************************

    '***************************************************************************************************************************************************
    'PUBBLICAZIONI GENERICHE
    '***************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AllegatiPubblicazioneGenericaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiPubblicazioneGenericaGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.DeleteAllegatoPubblicazione(e.Item)
    '' ''        Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub ScansionaPubblicazioneGenericaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaPubblicazioneGenericaImageButton.Click
    '' ''    Me.AvviaScansionePubblicazione(Me.FronteRetroPubblicazioneGenericaCheckBox.Checked, Me.VisualizzaUIPubblicazioneGenericaCheckBox.Checked, Me.ScanUploadPubblicazioneGenericaButton.ClientID)
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub ScanUploadPubblicazioneGenericaButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadPubblicazioneGenericaButton.Click
    '' ''    Me.NotificaScansionePubblicazione()
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiDocumentoPubblicazioneGenericaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoPubblicazioneGenericaImageButton.Click
    '' ''    Me.AggiungiAllegatoPubblicazione(Me.AllegatoPubblicazioneGenericaUpload, Me.DescrizioneDocumentoPubblicazioneGenericaTextBox, Me.DocumentoPrimarioPubblicazioneGenericaRadioButton.Checked)
    '' ''End Sub

    '***************************************************************************************************************************************************

    '***************************************************************************************************************************************************
    'INCARICO DIPENDENTE
    '***************************************************************************************************************************************************

    'luca 01/07/2020
    '' ''Protected Sub AllegatiIncaricoDipendenteGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiIncaricoDipendenteGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.DeleteAllegatoPubblicazione(e.Item)
    '' ''        Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub ScansionaIncaricoDipendenteImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaIncaricoDipendenteImageButton.Click
    '' ''    Me.AvviaScansionePubblicazione(Me.FronteRetroIncaricoDipendenteCheckBox.Checked, Me.VisualizzaUIIncaricoDipendenteCheckBox.Checked, Me.ScanUploadIncaricoDipendenteButton.ClientID)
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub ScanUploadIncaricoDipendenteButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadIncaricoDipendenteButton.Click
    '' ''    Me.NotificaScansionePubblicazione()
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiDocumentoIncaricoDipendenteImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoIncaricoDipendenteImageButton.Click
    '' ''    Me.AggiungiAllegatoPubblicazione(Me.AllegatoIncaricoDipendenteUpload, Me.DescrizioneDocumentoIncaricoDipendenteTextBox, Me.DocumentoPrimarioIncaricoDipendenteRadioButton.Checked)
    '' ''End Sub

    '***************************************************************************************************************************************************

    '***************************************************************************************************************************************************
    'COMPENSO - CONSULENZA
    '***************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AllegatiCompensoConsulenzaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiCompensoConsulenzaGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.DeleteAllegatoPubblicazione(e.Item)
    '' ''        Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub ScansionaCompensoConsulenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaCompensoConsulenzaImageButton.Click
    '' ''    Me.AvviaScansionePubblicazione(Me.FronteRetroCompensoConsulenzaCheckBox.Checked, Me.VisualizzaUICompensoConsulenzaCheckBox.Checked, Me.ScanUploadCompensoConsulenzaButton.ClientID)
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScanUploadCompensoConsulenzaButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadCompensoConsulenzaButton.Click
    '' ''    Me.NotificaScansionePubblicazione()
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub AggiungiDocumentoCompensoConsulenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoCompensoConsulenzaImageButton.Click
    '' ''    Me.AggiungiAllegatoPubblicazione(Me.AllegatoCompensoConsulenzaUpload, Me.DescrizioneDocumentoCompensoConsulenzaTextBox, Me.DocumentoPrimarioCompensoConsulenzaRadioButton.Checked)
    '' ''End Sub

    '***************************************************************************************************************************************************

    '***************************************************************************************************************************************************
    'ATTI DI CONCESSIONE
    '***************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AllegatiAttiConcessioneGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiAttiConcessioneGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.DeleteAllegatoPubblicazione(e.Item)
    '' ''        Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScansionaAttiConcessioneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaAttiConcessioneImageButton.Click
    '' ''    Me.AvviaScansionePubblicazione(Me.FronteRetroAttiConcessioneCheckBox.Checked, Me.VisualizzaUIAttiConcessioneCheckBox.Checked, Me.ScanUploadAttiConcessioneButton.ClientID)
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScanUploadAttiConcessioneButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadAttiConcessioneButton.Click
    '' ''    Me.NotificaScansionePubblicazione()
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub AggiungiDocumentoAttiConcessioneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoAttiConcessioneImageButton.Click
    '' ''    Me.AggiungiAllegatoPubblicazione(Me.AllegatoAttiConcessioneUpload, Me.DescrizioneDocumentoAttiConcessioneTextBox, Me.DocumentoPrimarioAttiConcessioneRadioButton.Checked)
    '' ''End Sub

    '***************************************************************************************************************************************************

    '***************************************************************************************************************************************************
    'INCARICO AMMINISTRATIVO - DIRIGENZIALE
    '***************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AllegatiIncarichiAmministrativiDirigenzialiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiIncarichiAmministrativiDirigenzialiGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.DeleteAllegatoPubblicazione(e.Item)
    '' ''        Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScansionaIncarichiAmministrativiDirigenzialiImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaIncarichiAmministrativiDirigenzialiImageButton.Click
    '' ''    Me.AvviaScansionePubblicazione(Me.FronteRetroIncarichiAmministrativiDirigenzialiCheckBox.Checked, Me.VisualizzaUIIncarichiAmministrativiDirigenzialiCheckBox.Checked, Me.ScanUploadIncarichiAmministrativiDirigenzialiButton.ClientID)
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScanUploadIncarichiAmministrativiDirigenzialiButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadIncarichiAmministrativiDirigenzialiButton.Click
    '' ''    Me.NotificaScansionePubblicazione()
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub AggiungiDocumentoIncarichiAmministrativiDirigenzialiImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoIncarichiAmministrativiDirigenzialiImageButton.Click
    '' ''    Me.AggiungiAllegatoPubblicazione(Me.AllegatoIncarichiAmministrativiDirigenzialiUpload, Me.DescrizioneDocumentoIncarichiAmministrativiDirigenzialiTextBox, Me.DocumentoPrimarioIncarichiAmministrativiDirigenzialiRadioButton.Checked)
    '' ''End Sub

    '***************************************************************************************************************************************************

    '***************************************************************************************************************************************************
    'BANDO CONCORSO
    '***************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AllegatiBandoConcorsoGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiBandoConcorsoGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.DeleteAllegatoPubblicazione(e.Item)
    '' ''        Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScansionaBandoConcorsoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaBandoConcorsoImageButton.Click
    '' ''    Me.AvviaScansionePubblicazione(Me.FronteRetroBandoConcorsoCheckBox.Checked, Me.VisualizzaUIBandoConcorsoCheckBox.Checked, Me.ScanUploadBandoConcorsoButton.ClientID)
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScanUploadBandoConcorsoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadBandoConcorsoButton.Click
    '' ''    Me.NotificaScansionePubblicazione()
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub AggiungiDocumentoBandoConcorsoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoBandoConcorsoImageButton.Click
    '' ''    Me.AggiungiAllegatoPubblicazione(Me.AllegatoBandoConcorsoUpload, Me.DescrizioneDocumentoBandoConcorsoTextBox, Me.DocumentoPrimarioBandoConcorsoRadioButton.Checked)
    '' ''End Sub

    '***************************************************************************************************************************************************

    '***************************************************************************************************************************************************
    'ENTE CONTROLLATO
    '***************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AllegatiEnteControllatoGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles AllegatiEnteControllatoGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.DeleteAllegatoPubblicazione(e.Item)
    '' ''        Case "Preview"
    '' ''            Me.DownloadAllegatoPubblicazione(e.Item)
    '' ''    End Select
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScansionaEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScansionaEnteControllatoImageButton.Click
    '' ''    Me.AvviaScansionePubblicazione(Me.FronteRetroEnteControllatoCheckBox.Checked, Me.VisualizzaUIEnteControllatoCheckBox.Checked, Me.ScanUploadEnteControllatoButton.ClientID)
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub ScanUploadEnteControllatoButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ScanUploadEnteControllatoButton.Click
    '' ''    Me.NotificaScansionePubblicazione()
    '' ''End Sub
    'luca 01/07/2020
    '' ''Protected Sub AggiungiDocumentoEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiDocumentoEnteControllatoImageButton.Click
    '' ''    Me.AggiungiAllegatoPubblicazione(Me.AllegatoEnteControllatoUpload, Me.DescrizioneDocumentoEnteControllatoTextBox, Me.DocumentoPrimarioEnteControllatoRadioButton.Checked)
    '' ''End Sub

    '***************************************************************************************************************************************************

#End Region

#Region "GESTIONE PROTOCOLLAZIONE (ORDINANZA E DECRETO)"

    Protected Sub TrovaProtocolloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaProtocolloImageButton.Click
        Me.ProtocollaAtto()
    End Sub

    Protected Sub EliminaProtocolloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaProtocolloImageButton.Click
        Me.NumeroProtocolloTextBox.Text = String.Empty
        Me.DataProtocolloTextBox.Text = String.Empty
    End Sub

    Protected Sub AggiornaProtocolloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaProtocolloImageButton.Click
        If Not ParsecUtility.SessionManager.Registrazione Is Nothing Then
            Dim registrazione As ParsecPro.Registrazione = ParsecUtility.SessionManager.Registrazione
            Me.NumeroProtocolloTextBox.Text = registrazione.NumeroProtocollo.ToString
            Me.DataProtocolloTextBox.Text = registrazione.DataImmissione.Value.ToShortDateString
            ParsecUtility.SessionManager.Registrazione = Nothing
        End If
    End Sub



    Private Sub ProtocollaAtto()
        If Not Me.Documento Is Nothing Then
            Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
            Dim idDocumento As Integer = Me.Documento.Id

            Dim registrazione As New ParsecPro.Registrazione

            '*******************************************************************************
            'CARICO I MITTENTI INTERNI
            '*******************************************************************************
            Dim mittenteInterno As ParsecPro.IReferente = Nothing
            Dim mittenteInternoUfficio As ParsecPro.IReferente = Nothing
            mittenteInterno = New ParsecPro.Mittente(Me.Documento.IdStruttura, True)
            If Me.Documento.IdStruttura <> Me.Documento.IdUfficio Then
                mittenteInternoUfficio = New ParsecPro.Mittente(Me.Documento.IdUfficio, True)
            End If
            registrazione.Mittenti.Add(mittenteInterno)

            If Not mittenteInternoUfficio Is Nothing Then
                registrazione.Mittenti.Add(mittenteInternoUfficio)
            End If
            '*******************************************************************************

            Dim oggetto As String = Me.GetDescrizioneTipologiaDocumento(Me.Documento.TipologiaDocumento, ParsecAtt.TipoProcedura.Modifica) & " Oggetto : " & Me.Documento.Oggetto


            registrazione.TipologiaRegistrazione = ParsecPro.TipoRegistrazione.Partenza
            registrazione.TipoRegistrazione = 1  'PARTENZA
            registrazione.TipoRegistro = ParsecPro.TipoRegistro.Generale
            'registrazione.ModalitaApertura = ParsecPro.ModalitaApertura.InserimentoEntrata
            registrazione.DataOraRegistrazione = Now
            registrazione.DataImmissione = Now
            registrazione.IdUtente = utenteCorrente.Id
            registrazione.UtenteUsername = utenteCorrente.Username
            registrazione.IdTipoRicezione = Nothing
            registrazione.ProtocolloMittente = String.Empty
            registrazione.Note = String.Empty
            registrazione.NoteInterne = String.Empty
            registrazione.Oggetto = oggetto
            registrazione.AnticipatoViaFax = False


            '*******************************************************************************
            'CARICO LA VISIBILITA'
            '*******************************************************************************

            Dim strutture As New ParsecAdmin.StructureRepository
            Dim startDate As Date = New Date(Now.Year, Now.Month, Now.Day, 0, 0, 0)
            Dim endDate As Date = New Date(Now.Year, Now.Month, Now.Day, 23, 59, 59)

            Dim gruppi As New ParsecAdmin.GruppoRepository(strutture.Context)
            Dim gruppo = (From s In strutture.GetQuery
                    Join g In gruppi.GetQuery
                    On s.IdGruppo Equals g.Id
                    Where g.Abilitato = True And s.Id = mittenteInterno.Id And g.DataInizioValidita <= startDate And (g.DataFineValidita >= Now Or Not g.DataFineValidita.HasValue)
                    Select g).FirstOrDefault

            If Not gruppo Is Nothing Then
                Dim gruppoDefaut As New ParsecAdmin.VisibilitaDocumento
                gruppoDefaut.AbilitaCancellaEntita = False
                gruppoDefaut.Descrizione = gruppo.Descrizione
                gruppoDefaut.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
                gruppoDefaut.IdEntita = gruppo.Id
                gruppoDefaut.IdModulo = ParsecAdmin.TipoModulo.PRO
                gruppoDefaut.LogIdUtente = utenteCorrente.Id
                gruppoDefaut.LogDataOperazione = Now
                registrazione.Visibilita.Add(gruppoDefaut)
            End If
            '*******************************************************************************

            '*******************************************************************************
            'CARICO GLI ALLEGATI
            '*******************************************************************************
            Dim allegato As ParsecPro.Allegato = Nothing

            Dim oggettoAllegato As String = "Ordinanza"
            If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto Then
                oggettoAllegato = "Decreto"
            End If

            allegato = Me.GetAllegatoProtocollo(ParsecAdmin.WebConfigSettings.GetKey("PathAtti") & Me.GetAnnoEsercizio(Me.Documento) & "\", oggettoAllegato, 1, Me.Documento.Nomefile)
            registrazione.Allegati.Add(allegato)

            For Each all In Me.Allegati
                Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
                allegato = Me.GetAllegatoProtocollo(percorsoRoot & all.PercorsoRelativo, oggettoAllegato, 0, all.Nomefile)
                registrazione.Allegati.Add(allegato)
            Next

            '*******************************************************************************

            Dim pageUrl As String = "~/UI/Protocollo/pages/user/RegistrazioneArrivoPage.aspx"
            Dim queryString As New Hashtable
            queryString.Add("Mode", "Insert")
            queryString.Add("Tipo", "1")
            queryString.Add("obj", Me.AggiornaProtocolloImageButton.ClientID)
            Dim parametriPagina As New Hashtable
            parametriPagina.Add("RegistrazioneInIter", registrazione)

            parametriPagina.Add("IdDocumento", idDocumento)

            ParsecUtility.SessionManager.ParametriPagina = parametriPagina
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 600, queryString, False)
        End If


    End Sub

    Private Function GetAllegatoProtocollo(ByVal srcPath As String, ByVal oggettoAllegato As String, tipologiaAllegato As Integer, ByVal nomeFile As String) As ParsecPro.Allegato
        Dim allegato As ParsecPro.Allegato = Nothing

        'Copio l'allegato nella cartella temporanea.
        Dim pathDownloadTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & nomeFile
        IO.File.Copy(srcPath & nomeFile, pathDownloadTemp, True)

        allegato = New ParsecPro.Allegato
        allegato.NomeFile = nomeFile
        allegato.NomeFileTemp = nomeFile
        allegato.IdTipologiaDocumento = tipologiaAllegato
        allegato.DescrizioneTipologiaDocumento = "Primario"
        If tipologiaAllegato = 0 Then
            allegato.DescrizioneTipologiaDocumento = "Allegato"
        End If

        allegato.Oggetto = oggettoAllegato
        allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
        allegato.PercorsoRootTemp = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
        allegato.Impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathDownloadTemp)
        allegato.ImprontaEsadecimale = BitConverter.ToString(allegato.Impronta).Replace("-", "")
        Return allegato
    End Function

#End Region

#Region "GESTIONE FIRME"

    Protected Sub FirmeGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FirmeGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.ModificaFirma(e.Item)
        End If
        If e.CommandName = "Preview" Then
            Me.AnteprimaP7m(e.Item)
        End If
    End Sub

    Private Sub AnteprimaP7m(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim firma As ParsecAtt.Firma = Me.Firme.Where(Function(c) c.Id = id).FirstOrDefault

        Dim annoEsercizio As Integer = Now.Year
        If Me.Documento.Modello.Proposta Then
            annoEsercizio = Me.Documento.DataProposta.Value.Year
        Else
            annoEsercizio = Me.Documento.Data.Value.Year
        End If

        Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, firma.FileFirmato)

        If Not IO.File.Exists(localPath) Then
            ParsecUtility.Utility.MessageBox("Il file firmato selezionato non esiste!", False)
        Else
            Me.VisualizzaDocumentoP7M(localPath, annoEsercizio)
        End If
    End Sub

    Private Sub VisualizzaDocumentoP7M(ByVal localPath As String, annoEsercizio As Integer)
        'Dim openofficeParameters As New ParsecAdmin.OpenOfficeParameters
        'Dim pathDownload = String.Format("{0}{1}{2}/{3}", ParsecAdmin.WebConfigSettings.GetKey("DocumentServerUrl"), ParsecAdmin.WebConfigSettings.GetKey("PathRelativeAtti"), annoEsercizio, IO.Path.GetFileName(localPath))
        'Dim datiInput As New ParsecAdmin.DatiInput With {.Path = pathDownload, .ShowWindow = False, .Enabled = False, .FunctionName = "OpenGenericDocument"}
        'Dim data As String = openofficeParameters.CreateOpenParameter(datiInput)
        'ParsecUtility.Utility.RegistraScriptAperturaParsecOpenDocument(data, False, False)


        Session("AttachmentFullName") = localPath
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
    End Sub

    Protected Sub FirmeGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FirmeGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            Dim firma As ParsecAtt.Firma = CType(e.Item.DataItem, ParsecAtt.Firma)
            Dim selectButton = CType(dataItem("Select").Controls(0), ImageButton)

            selectButton.ToolTip = "Modifica Firma..."

            If Me.TipologiaProceduraCorrente = ParsecAtt.TipoProcedura.ModificaParere Then
                If Not String.IsNullOrEmpty(Me.IdFirmaModificabile) Then

                    If firma.Id <> CInt(Me.IdFirmaModificabile) Then
                        selectButton.ImageUrl = "~\images\vuoto.png"
                        selectButton.ToolTip = ""
                        selectButton.Attributes.Add("onclick", "return false;")
                    End If
                End If
            End If



            If TypeOf dataItem("Preview").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Preview").Controls(0), ImageButton)
                If Not String.IsNullOrEmpty(firma.FileFirmato) Then
                    If IO.Path.GetExtension(firma.FileFirmato).ToLower = ".p7m" Then
                        btn.ImageUrl = "~\images\signedDocument.png"
                        btn.ToolTip = "Visualizza file firmato..."
                    Else
                        btn.ImageUrl = "~\images\vuoto.png"
                        btn.ToolTip = ""
                        btn.Attributes.Add("onclick", "return false;")
                    End If
                Else
                    btn.ImageUrl = "~\images\vuoto.png"
                    btn.ToolTip = ""
                    btn.Attributes.Add("onclick", "return false;")
                End If
            End If

        End If
    End Sub

    Private Sub ModificaFirma(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim firma As ParsecAtt.Firma = Me.Firme.Where(Function(c) c.Id = id).FirstOrDefault

        If firma.IdTipologiaFirma.HasValue Then
            'ORGANIGRAMMA
            If firma.IdTipologiaFirma = 3 Then
                If String.IsNullOrEmpty(Me.SettoreTextBox.Text) Then
                    ParsecUtility.Utility.MessageBox("Per modificare la firma selezionata Ã¨ necessario selezionare prima un settore!", False)
                    Exit Sub
                End If
            End If
        End If

        If Not String.IsNullOrEmpty(firma.DefaultStruttura) Then
            ParsecUtility.SessionManager.Firma = firma
            Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/FirmaPage.aspx"
            Dim queryString As New Hashtable
            Dim idUfficio As String = "0"
            Dim idSettore As String = "0"
            If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
                idUfficio = Me.IdUfficioTextBox.Text
            End If
            If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
                idSettore = Me.IdSettoreTextBox.Text
            End If

            queryString.Add("IdUfficio", idUfficio)
            queryString.Add("IdSettore", idSettore)
            queryString.Add("obj", Me.AggiornaFirmaImageButton.ClientID)
            ParsecUtility.Utility.ShowPopup(pageUrl, 650, 330, queryString, False)
        Else
            ParsecUtility.Utility.MessageBox("La firma selezionata non può essere modificata, perchè non ha specificato il firmatario!", False)
        End If

    End Sub

    Protected Sub AggiornaFirmaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaFirmaImageButton.Click
        If Not ParsecUtility.SessionManager.Firma Is Nothing Then
            Dim firma As ParsecAtt.Firma = ParsecUtility.SessionManager.Firma
            Dim firmaToUpdate = Me.Firme.Where(Function(c) c.Id = firma.Id).FirstOrDefault
            '******************************************************************************
            'Mi scambia l'ordine
            'Me.Firme.Remove(firma)
            'Me.Firme.Add(firma)
            '******************************************************************************
            firmaToUpdate = firma
            ParsecUtility.SessionManager.Firma = Nothing
            Me.AggiornaGrigliaFirme()
        End If
    End Sub




#End Region

#Region "IMPOSTA ABILITAZIONI"

    Private Sub DisabilitaUI()

        Me.AttiTabStrip.Tabs.FindTabByText("Presenze").Enabled = False
        Me.AttiTabStrip.Tabs.FindTabByText("Contabilità").Enabled = False
        Me.AttiTabStrip.Tabs.FindTabByText("Classificazioni").Enabled = False
        'Me.AttiTabStrip.FindTabByText("Visibilità").Enabled 
        'luca 01/07/2020
        '' ''Me.AttiTabStrip.Tabs.FindTabByText("Trasparenza").Enabled = False


        Me.RadToolBar.FindItemByText("Nuovo").Enabled = False
        Me.RadToolBar.FindItemByText("Nuovo").ToolTip = ""
        Me.RadToolBar.FindItemByText("Elimina").Enabled = False
        '   Me.RadToolBar.FindItemByText("Elimina").Attributes.Remove("onclick")
        Me.RadToolBar.FindItemByText("Elimina").ToolTip = ""


        '***************************************************************************************
        'SCHEDA GENERALE
        '***************************************************************************************
        Me.TipologieDocumentoComboBox.Enabled = False
        Me.ModelliComboBox.Enabled = False
        Me.NumeroSettoreTextBox.Enabled = False
        Me.NumeroAttoTextBox.Enabled = False
        Me.DataTextBox.Enabled = False
        Me.UfficioTextBox.Enabled = False
        Me.TrovaUfficioImageButton.Visible = False
        Me.EliminaUfficioImageButton.Visible = False
        Me.SettoreTextBox.Enabled = False
        Me.OggettoTextBox.Enabled = False
        Me.DelimitaTestoImageButton.Visible = False

        Me.NoteTextBox.Enabled = False
        Me.SalvaNoteButton.Visible = False


        Me.ImpostaAbilitazionePannelloAffissione(False)
        Me.AffissionePanel.Visible = False


        Me.NumeroProtocolloTextBox.Enabled = False
        Me.DataProtocolloTextBox.Enabled = False
        Me.TrovaProtocolloImageButton.Visible = False
        Me.EliminaProtocolloImageButton.Visible = False
        Me.ProtocolloPanel.Visible = False

        Me.BozzaTextBox.Enabled = False
        Me.TrovaBozzaImageButton.Visible = False
        Me.EliminaBozzaImageButton.Visible = False
        Me.BozzaPanel.Visible = False


        '***************************************************************************************

        '***************************************************************************************
        'VISTI E PARERI
        '***************************************************************************************
        Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
        'Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Preview").Visible = False
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA PRESENZE
        '***************************************************************************************
        Me.ImpostaAbilitazioneSchedaPresenze(False)
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA CONTABILITA
        '***************************************************************************************
        Me.ImpostaAbilitazioneSchedaContabilita(False)
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA ALLEGATI
        '***************************************************************************************
        Me.ImpostaAbilitazioneSchedaAllegati(False)
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA CLASSIFICAZIONI
        '***************************************************************************************
        Me.ImpostaAbilitazioneSchedaClassificazioni(False)
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA VISIBILITA'
        '***************************************************************************************
        Me.ImpostaAbilitazioneSchedaVisibilita(False)
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA TRASPARENZA
        '***************************************************************************************
        'luca 01/07/2020
        '' ''Me.ImpostaAbilitazioneSchedaTrasparenza(False)
        '***************************************************************************************

        '***************************************************************************************
        'SCHEDA FASCICOLI
        '***************************************************************************************
        Me.ImpostaAbilitazioneSchedaFascicoli(False)
        '***************************************************************************************
    End Sub

    Private Sub ImpostaAbilitazioneSchedaContabilita(enabled As Boolean)
        '***************************************************************************************
        'SCHEDA CONTABILITA
        '***************************************************************************************
        Me.AggiungiImpegnoSpesaImageButton.Visible = enabled
        Me.AggiungiLiquidazioneImageButton.Visible = enabled
        Me.AggiungiAccertamentoImageButton.Visible = enabled
        Me.ImpegniSpesaGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = enabled
        Me.ImpegniSpesaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
        Me.ImpegniSpesaGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = enabled
        Me.ImpegniSpesaGridView.MasterTableView.Columns.FindByUniqueName("Preview").Visible = Not enabled

        Me.LiquidazioniGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = enabled
        Me.LiquidazioniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
        Me.LiquidazioniGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = enabled
        Me.LiquidazioniGridView.MasterTableView.Columns.FindByUniqueName("Preview").Visible = Not enabled
        Me.AccertamentiGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = enabled
        Me.AccertamentiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
        Me.AccertamentiGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = enabled
        '***************************************************************************************

    End Sub

    Private Sub ImpostaAbilitazioneSchedaAllegati(enabled As Boolean)
        '***************************************************************************************
        'SCHEDA ALLEGATI
        '***************************************************************************************
        Me.AllegatiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
        Me.DescrizioneDocumentoTextBox.Enabled = enabled
        Me.AllegatoUpload.Enabled = enabled
        Me.FronteRetroCheckBox.Enabled = enabled
        Me.VisualizzaUICheckBox.Enabled = enabled
        Me.ScansionaImageButton.Visible = enabled
        Me.AggiungiDocumentoImageButton.Visible = enabled
        Me.TrovaFatturaImageButton.Visible = enabled
        Me.AllegatiGridView.MasterTableView.Columns.FindByUniqueName("Firma").Display = enabled
        'Me.AllegatiGridView.MasterTableView.GetColumnSafe("Firma").Display = enabled
        '***************************************************************************************
    End Sub

    Private Sub ImpostaAbilitazioneSchedaVisibilita(enabled As Boolean)
        '***************************************************************************************
        'SCHEDA VISIBILITA'
        '***************************************************************************************
        Me.VisibilitaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
        Me.TrovaGruppoVisibilitaImageButton.Visible = enabled
        Me.TrovaUtenteVisibilitaImageButton.Visible = enabled
        Me.RiservatoCheckBox.Enabled = enabled
    End Sub

    'luca 01/07/2020
    '' ''Private Sub ImpostaAbilitazioneSchedaTrasparenza(enabled As Boolean)
    '' ''    '***************************************************************************************
    '' ''    'SCHEDA TRASPARENZA
    '' ''    '***************************************************************************************

    '' ''    Me.SezioneTrasparenzaTextBox.Enabled = enabled
    '' ''    Me.TrovaSezioneImageButton.Visible = enabled
    '' ''    Me.EliminaSezioneImageButton.Visible = enabled
    '' ''    Me.AggiungiBeneficiarioImageButton.Visible = enabled
    '' ''    Me.comboboxSottoSezione.Enabled = enabled
    '' ''    Me.BeneficiariGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = enabled
    '' ''    Me.BeneficiariGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.BeneficiariGridView.MasterTableView.Columns.FindByUniqueName("Copy").Visible = enabled
    '' ''    Me.BeneficiariGridView.MasterTableView.Columns.FindByUniqueName("Preview").Visible = Not enabled
    '' ''    ImpostaAbilitazioneConcessione(enabled)
    '' ''    ImpostaAbilitazioneConsulenti(enabled)
    '' ''    Me.ImpostaAbilitazioneBandoGara(enabled)
    '' ''    Me.ImpostaAbilitazionePubblicazioneGenerica(enabled)

    '' ''    Me.ImpostaAbilitazioneIncaricoDipendente(enabled)
    '' ''    Me.ImpostaAbilitazioneIncaricoAmministrativoDirigenziale(enabled)
    '' ''    Me.ImpostaAbilitazioneBandoConcorso(enabled)
    '' ''    Me.ImpostaAbilitazioneEnteControllato(enabled)
    '' ''End Sub

    Private Sub ImpostaAbilitazioneSchedaClassificazioni(enabled As Boolean)
        '***************************************************************************************
        'SCHEDA CLASSIFICAZIONI
        '***************************************************************************************

        'ClassificazioniGridView
        Me.ClassificazioniGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
        Me.AnnotazioniTextBox.Enabled = enabled
        Me.ClassificazioneTextBox.Enabled = enabled
        Me.TrovaClassificazioneImageButton.Visible = enabled
        Me.AggiungiClassificazioneImageButton.Visible = enabled

        '***************************************************************************************
    End Sub

    Private Sub ImpostaAbilitazioneSchedaPresenze(enabled As Boolean)
        '***************************************************************************************
        'SCHEDA PRESENZE
        '***************************************************************************************
        'PresenzeGridView
        Me.EsecutivitaImmediataCheckBox.Enabled = enabled
        Me.GiorniEsecutivitaTextBox.Enabled = enabled
        Me.TipiApprovazioneComboBox.Enabled = enabled
        Me.SedutaTextBox.Enabled = False
        Me.TrovaSedutaImageButton.Visible = enabled

        '***************************************************************************************
    End Sub

    'luca 01/07/2020
    '' ''Private Sub ImpostaAbilitazioneConcessione(ByVal enabled As Boolean)
    '' ''    '***************************************************************************************
    '' ''    'SCHEDA GENERALE
    '' ''    '***************************************************************************************

    '' ''    Me.RubricaComboBox.Enabled = enabled
    '' ''    Me.AggiungiBeneficiarioImageButton.Visible = enabled

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA ALLEGATI
    '' ''    '***************************************************************************************
    '' ''    Me.AllegatiAttiConcessioneGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.DescrizioneDocumentoAttiConcessioneTextBox.Enabled = enabled
    '' ''    Me.AllegatoAttiConcessioneUpload.Enabled = enabled
    '' ''    Me.FronteRetroAttiConcessioneCheckBox.Enabled = enabled
    '' ''    Me.VisualizzaUIAttiConcessioneCheckBox.Enabled = enabled
    '' ''    Me.ScansionaAttiConcessioneImageButton.Visible = enabled
    '' ''    Me.AggiungiDocumentoAttiConcessioneImageButton.Visible = enabled
    '' ''    '***************************************************************************************


    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub ImpostaAbilitazioneConsulenti(ByVal enabled As Boolean)

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA GENERALE
    '' ''    '***************************************************************************************
    '' ''    Me.BeneficiarioConsulenzaComboBox.Enabled = enabled
    '' ''    Me.TrovaBeneficiarioConsulenzaImageButton.Visible = enabled
    '' ''    Me.DenominazioneConsulenzaTextBox.Enabled = enabled
    '' ''    Me.RagioneIncaricoConsulenzaTextBox.Enabled = enabled
    '' ''    Me.CompensoConsulenzaTextBox.Enabled = enabled
    '' ''    Me.VariabileCompensoConsulenzaTextBox.Enabled = enabled
    '' ''    Me.altreCaricheTextBox.Enabled = enabled
    '' ''    Me.altriIncarichiTextBox.Enabled = enabled
    '' ''    Me.altreAttivitaProfessionaliTextBox.Enabled = enabled
    '' ''    Me.DataInizioIncaricoConsulenzaTextBox.Enabled = enabled
    '' ''    Me.DataFineIncaricoConsulenzaTextBox.Enabled = enabled

    '' ''    Me.DataAttoIncaricoConsulenzaCollaborazioneTextBox.Enabled = enabled
    '' ''    Me.NumeroAttoIncaricoConsulenzaCollaborazioneTextBox.Enabled = enabled

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA DICHIARAZIONI
    '' ''    '***************************************************************************************
    '' ''    Me.RimuoviCurriculumImageButton.Visible = enabled
    '' ''    Me.RimuoviInconsistenzaImageButton.Visible = enabled
    '' ''    Me.CurriculumUpload.Enabled = enabled
    '' ''    Me.InconsistenzaUpload.Enabled = enabled
    '' ''    '***************************************************************************************

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA ALLEGATI
    '' ''    '***************************************************************************************
    '' ''    Me.AllegatiCompensoConsulenzaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.DescrizioneDocumentoCompensoConsulenzaTextBox.Enabled = enabled
    '' ''    Me.AllegatoCompensoConsulenzaUpload.Enabled = enabled
    '' ''    Me.FronteRetroCompensoConsulenzaCheckBox.Enabled = enabled
    '' ''    Me.VisualizzaUICompensoConsulenzaCheckBox.Enabled = enabled
    '' ''    Me.ScansionaCompensoConsulenzaImageButton.Visible = enabled
    '' ''    Me.AggiungiDocumentoCompensoConsulenzaImageButton.Visible = enabled
    '' ''    '***************************************************************************************

    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub ImpostaAbilitazioneBandoGara(ByVal enabled As Boolean)

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA GENERALE
    '' ''    '***************************************************************************************
    '' ''    Me.FiltroBandoGaraTextBox.Enabled = enabled
    '' ''    Me.TrovaBandoGaraImageButton.Visible = enabled
    '' ''    Me.OggettoBandoGaraTextBox.Enabled = enabled
    '' ''    Me.PartecipanteComboBox.Enabled = enabled
    '' ''    Me.TrovaPartecipanteImageButton.Visible = enabled
    '' ''    Me.AggiungiPartecipanteImageButton.Visible = enabled
    '' ''    Me.TrovaRaggruppamentoImageButton.Visible = enabled
    '' ''    Me.EliminaPartecipanteImageButton.Visible = enabled
    '' ''    Me.EliminaAggiudicatarioImageButton.Visible = enabled
    '' ''    Me.AggiungiTuttoImageButton.Visible = enabled
    '' ''    Me.AggiungiImageButton.Visible = enabled
    '' ''    Me.CigBandoGaraTextBox.Enabled = enabled
    '' ''    Me.ImportoAggiudicazioneTextBox.Enabled = enabled
    '' ''    Me.ImportoLiquidatoTextBox.Enabled = enabled
    '' ''    Me.NumeroOfferentiTextBox.Enabled = enabled
    '' ''    Me.DataInizioLavoroTextBox.Enabled = enabled
    '' ''    Me.DataFineLavoroTextBox.Enabled = enabled
    '' ''    Me.TipologiaSceltaComboBox.Enabled = enabled
    '' ''    Me.DenominazioneStrutturaProponenteTextBox.Enabled = enabled
    '' ''    Me.CodiceFiscaleProponenteTextBox.Enabled = enabled
    '' ''    '***************************************************************************************

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA ALLEGATI
    '' ''    '***************************************************************************************
    '' ''    Me.AllegatiBandoGaraGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.DescrizioneDocumentoBandoGaraTextBox.Enabled = enabled
    '' ''    Me.AllegatoBandoGaraUpload.Enabled = enabled
    '' ''    Me.FronteRetroBandoGaraCheckBox.Enabled = enabled
    '' ''    Me.VisualizzaUIBandoGaraCheckBox.Enabled = enabled
    '' ''    Me.ScansionaBandoGaraImageButton.Visible = enabled
    '' ''    Me.AggiungiDocumentoBandoGaraImageButton.Visible = enabled
    '' ''    '***************************************************************************************

    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub ImpostaAbilitazionePubblicazioneGenerica(ByVal enabled As Boolean)
    '' ''    '***************************************************************************************
    '' ''    'SCHEDA GENERALE
    '' ''    '***************************************************************************************
    '' ''    Me.PubblicazioneTitoloTextBox.Enabled = enabled
    '' ''    Me.PubblicazioneContenutoTextBox.Enabled = enabled
    '' ''    Me.PubblicazioneNumeroTextBox.Enabled = enabled
    '' ''    Me.PubblicazioneInizioRiferimentoTextBox.Enabled = enabled
    '' ''    Me.PubblicazioneFineRiferimentoTextBox.Enabled = enabled
    '' ''    '***************************************************************************************

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA ALLEGATI
    '' ''    '***************************************************************************************
    '' ''    Me.AllegatiPubblicazioneGenericaGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.DescrizioneDocumentoPubblicazioneGenericaTextBox.Enabled = enabled
    '' ''    Me.AllegatoPubblicazioneGenericaUpload.Enabled = enabled
    '' ''    Me.FronteRetroPubblicazioneGenericaCheckBox.Enabled = enabled
    '' ''    Me.VisualizzaUIPubblicazioneGenericaCheckBox.Enabled = enabled
    '' ''    Me.ScansionaPubblicazioneGenericaImageButton.Visible = enabled
    '' ''    Me.AggiungiDocumentoPubblicazioneGenericaImageButton.Visible = enabled
    '' ''    '***************************************************************************************
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub ImpostaAbilitazioneIncaricoDipendente(ByVal enabled As Boolean)
    '' ''    '***************************************************************************************
    '' ''    'SCHEDA GENERALE
    '' ''    '***************************************************************************************
    '' ''    Me.BeneficiarioIncaricoComboBox.Enabled = enabled
    '' ''    Me.TrovaBeneficiarioIncaricoDipendenteImageButton.Visible = enabled
    '' ''    Me.OggettoIncaricoDipendenteTextBox.Enabled = enabled
    '' ''    Me.RagioneIncaricoDipendenteTextBox.Enabled = enabled
    '' ''    Me.CompensoIncaricoDipendenteTextBox.Enabled = enabled
    '' ''    Me.DataInizioIncaricoDipendenteTextBox.Enabled = enabled
    '' ''    Me.DataFineIncaricoDipendenteTextBox.Enabled = enabled
    '' ''    '***************************************************************************************


    '' ''    '***************************************************************************************
    '' ''    'SCHEDA ALLEGATI
    '' ''    '***************************************************************************************
    '' ''    Me.AllegatiIncaricoDipendenteGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.DescrizioneDocumentoIncaricoDipendenteTextBox.Enabled = enabled
    '' ''    Me.AllegatoIncaricoDipendenteUpload.Enabled = enabled
    '' ''    Me.FronteRetroIncaricoDipendenteCheckBox.Enabled = enabled
    '' ''    Me.VisualizzaUIIncaricoDipendenteCheckBox.Enabled = enabled
    '' ''    Me.ScansionaIncaricoDipendenteImageButton.Visible = enabled
    '' ''    Me.AggiungiDocumentoIncaricoDipendenteImageButton.Visible = enabled
    '' ''    '***************************************************************************************
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub ImpostaAbilitazioneEnteControllato(ByVal enabled As Boolean)

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA GENERALE
    '' ''    '***************************************************************************************
    '' ''    Me.TrovaEnteControllatoImageButton.Visible = enabled
    '' ''    Me.RagioneSocialeEnteControllatoCombobox.Enabled = enabled
    '' ''    Me.AttivitaFavoreAmministrazioneEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.AttivitaServizioPubblicoEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.DurataImpegnoEnteControllatoTextbox.Enabled = enabled
    '' ''    Me.FunzioniAttribuiteEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.MisuraPartecipazioneEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.UrlSitoIstituzionaleEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.OnereComplessivoEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.EntitaSocietaPartecipateTextBox.Enabled = enabled
    '' ''    Me.NumeroRappresentantiEnteControllatoTextBox.Enabled = enabled

    '' ''    Me.TrattamentiEconomiciRappresentantiEnteControllatoGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.TrattamentiEconomiciRappresentantiEnteControllatoGridView.MasterTableView.Columns.FindByUniqueName("SelectCheckBox").Visible = enabled
    '' ''    Me.AggiungiTrattamentoEconomicoRappresentanteEnteControllatoImageButton.Visible = enabled
    '' ''    Me.DeleteTrattamentoEconomicoRappresentanteEnteControllatoImageButton.Visible = enabled
    '' ''    Me.TrattamentoEconomicoRappresentanteEnteControllatoTextbox.Enabled = enabled



    '' ''    Me.TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView.MasterTableView.Columns.FindByUniqueName("SelectCheckBox").Visible = enabled
    '' ''    Me.AggiungiTrattamentoEconomicoIncaricoAmministratoreEnteControllatoImageButton.Visible = enabled
    '' ''    Me.DeleteTrattamentoEconomicoIncaricoAmministratoreEnteControllatoImageButton.Visible = enabled
    '' ''    Me.TrattamentoEconomicoAmministratoreEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.IncaricoAmministratoreEnteControllatoTextBox.Enabled = enabled


    '' ''    Me.BilanciEnteControllatoGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.BilanciEnteControllatoGridView.MasterTableView.Columns.FindByUniqueName("SelectCheckBox").Visible = enabled
    '' ''    Me.AggiungiBilancioEnteControllatoImageButton.Visible = enabled
    '' ''    Me.DeleteBilancioEnteControllatoImageButton.Visible = enabled
    '' ''    Me.AnnoBilancioEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.BilancioEnteControllatoTextBox.Enabled = enabled



    '' ''    '***************************************************************************************

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA DICHIARAZIONI
    '' ''    '***************************************************************************************
    '' ''    Me.InconferibilitaUpload.Enabled = enabled
    '' ''    Me.IncompatibilitaUpload.Enabled = enabled
    '' ''    Me.RimuoviInconferibilitaImageButton.Enabled = enabled
    '' ''    Me.RimuoviIncompatibilitaImageButton.Enabled = enabled
    '' ''    '***************************************************************************************



    '' ''    '***************************************************************************************
    '' ''    'SCHEDA ALLEGATI
    '' ''    '***************************************************************************************
    '' ''    Me.AllegatiEnteControllatoGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.DescrizioneDocumentoEnteControllatoTextBox.Enabled = enabled
    '' ''    Me.AllegatoEnteControllatoUpload.Enabled = enabled
    '' ''    Me.FronteRetroEnteControllatoCheckBox.Enabled = enabled
    '' ''    Me.VisualizzaUIEnteControllatoCheckBox.Enabled = enabled
    '' ''    Me.ScansionaEnteControllatoImageButton.Visible = enabled
    '' ''    Me.AggiungiDocumentoEnteControllatoImageButton.Visible = enabled

    '' ''    '***************************************************************************************

    '' ''End Sub

    'luca 01/07/2020
    'Private Sub ImpostaAbilitazioneBandoConcorso(ByVal enabled As Boolean)

    '    '***************************************************************************************
    '    'SCHEDA GENERALE
    '    '***************************************************************************************

    '    Me.BandoConcorsoTipoAssunzioneTextBox.Enabled = enabled
    '    Me.BandoConcorsoOggettoTextBox.Enabled = enabled
    '    Me.BandoConcorsoProfiloTextBox.Enabled = enabled
    '    Me.BandoConcorsoCategoriaTextBox.Enabled = enabled
    '    Me.BandoConcorsoSpesaTextBox.Enabled = enabled
    '    Me.BandoConcorsoNumeroDipendentiAssuntiTextBox.Enabled = enabled
    '    Me.BandoConcorsoEstremiDocumentiTextBox.Enabled = enabled

    '    '***************************************************************************************

    '    '***************************************************************************************
    '    'SCHEDA ALLEGATI
    '    '***************************************************************************************
    '    Me.AllegatiBandoConcorsoGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '    Me.DescrizioneDocumentoBandoConcorsoTextBox.Enabled = enabled
    '    Me.AllegatoBandoConcorsoUpload.Enabled = enabled
    '    Me.FronteRetroBandoConcorsoCheckBox.Enabled = enabled
    '    Me.VisualizzaUIBandoConcorsoCheckBox.Enabled = enabled
    '    Me.ScansionaBandoConcorsoImageButton.Visible = enabled
    '    Me.AggiungiDocumentoBandoConcorsoImageButton.Visible = enabled
    '    '***************************************************************************************

    'End Sub

    'luca 01/07/2020
    '' ''Private Sub ImpostaAbilitazioneIncaricoAmministrativoDirigenziale(ByVal enabled As Boolean)


    '' ''    '***************************************************************************************
    '' ''    'SCHEDA GENERALE
    '' ''    '***************************************************************************************
    '' ''    Me.TitolareIncaricoComboBox.Enabled = enabled
    '' ''    Me.TrovaTitolareIncaricoImageButton.Visible = enabled
    '' ''    Me.DenominazioneIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.RagioneIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.DataInizioIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.DataFineIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.CompensoIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.CompensiVariabiliIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.CaricheIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.AltriIncarichiIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.AltreAttivitaIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.NumeroAttoIncaricoAmministrativoTextBox.Enabled = enabled
    '' ''    Me.DataAttoIncaricoAmministrativoTextBox.Enabled = enabled


    '' ''    '***************************************************************************************
    '' ''    'SCHEDA DICHIARAZIONI
    '' ''    '***************************************************************************************
    '' ''    Me.RimuoviCurriculumIncaricoAmministrativoImageButton.Visible = enabled
    '' ''    Me.RimuoviInconferibilitaIncaricoAmministrativoImageButton.Visible = enabled
    '' ''    Me.RimuoviIncompatibilitaIncaricoAmministrativoImageButton.Visible = enabled
    '' ''    Me.CurriculumIncaricoAmministrativoUpload.Enabled = enabled
    '' ''    Me.InconferibilitaIncaricoAmministrativoUpload.Enabled = enabled
    '' ''    Me.incompatibilitaIncaricoAmministrativoUpload.Enabled = enabled
    '' ''    '***************************************************************************************

    '' ''    '***************************************************************************************

    '' ''    '***************************************************************************************
    '' ''    'SCHEDA ALLEGATI
    '' ''    '***************************************************************************************
    '' ''    Me.AllegatiIncarichiAmministrativiDirigenzialiGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    '' ''    Me.DescrizioneDocumentoIncarichiAmministrativiDirigenzialiTextBox.Enabled = enabled
    '' ''    Me.AllegatoIncarichiAmministrativiDirigenzialiUpload.Enabled = enabled
    '' ''    Me.FronteRetroIncarichiAmministrativiDirigenzialiCheckBox.Enabled = enabled
    '' ''    Me.VisualizzaUIIncarichiAmministrativiDirigenzialiCheckBox.Enabled = enabled
    '' ''    Me.ScansionaIncarichiAmministrativiDirigenzialiImageButton.Visible = enabled
    '' ''    Me.AggiungiDocumentoIncarichiAmministrativiDirigenzialiImageButton.Visible = enabled
    '' ''    '***************************************************************************************


    '' ''End Sub

    Private Sub AbilitaUI(ByVal procedura As ParsecAtt.TipoProcedura)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim proposta = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDelibera OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaOrdinanza OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDecreto)
        Me.DocumentiGridView.MasterTableView.Columns.FindByUniqueName("Unlock").Visible = utenteCollegato.SuperUser
        Dim documentoModificabile As Boolean = True
        Dim modificaNumeroSettore As Boolean = False

        If Not Me.Documento Is Nothing Then
            Dim blocca As Boolean = True
            If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.ModificaAmministrativa Or Not Page.Request("Iter") Is Nothing Then blocca = False
            If blocca Then
                '**********************************************************************************************************************
                'Gestione blocco documento
                '**********************************************************************************************************************
                Dim documentiBloccati As New ParsecAtt.LockDocumentoRepository
                'Se il documento non è bloccato dall'utente collegato.
                Dim documentoBloccato As ParsecAtt.LockDocumento = documentiBloccati.GetQuery.Where(Function(c) c.CodiceDocumento = Documento.Codice And c.IdUtente <> utenteCollegato.Id).FirstOrDefault
                If Not documentoBloccato Is Nothing Then
                    If Me.TipologiaProceduraApertura <> ParsecAtt.TipoProcedura.Visualizzazione Then
                        Dim utenteLock As ParsecAdmin.Utente = (New ParsecAdmin.UserRepository).GetQuery.Where(Function(c) c.Id = documentoBloccato.IdUtente).FirstOrDefault
                        ParsecUtility.Utility.MessageBox("L'atto amministrativo selezionato è bloccato da " & utenteLock.Username & "!", False)
                    End If
                    documentoModificabile = False
                Else
                    'Blocco il documento corrente.
                    documentoBloccato = New ParsecAtt.LockDocumento With
                                                           {
                                                            .IdUtente = utenteCollegato.Id,
                                                             .CodiceDocumento = Documento.Codice
                                                            }
                    documentiBloccati.Save(documentoBloccato)
                    ParsecUtility.SessionManager.LockDocumento = documentoBloccato
                    documentiBloccati.Dispose()
                End If
                If documentoModificabile And procedura <> ParsecAtt.TipoProcedura.Classificazione Then
                    documentoModificabile = (Not Me.Documento.IdFiglio.HasValue And Not Me.Documento.DataAffissioneDa.HasValue)
                Else
                    documentoModificabile = (Not Me.Documento.IdFiglio.HasValue)
                End If
            End If
        End If

        If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina OrElse Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina Then
            If procedura = ParsecAtt.TipoProcedura.Nuovo OrElse procedura = ParsecAtt.TipoProcedura.Ricerca OrElse procedura = ParsecAtt.TipoProcedura.Numerazione OrElse procedura = ParsecAtt.TipoProcedura.Modifica Then
                Dim parametri As New ParsecAdmin.ParametriRepository

                Dim numerazioneAttivata As Boolean = False
                Dim parametroMomentoNumerazioneSettore As ParsecAdmin.Parametri = parametri.GetByName("MomentoNumerazioneSettore", ParsecAdmin.TipoModulo.ATT)
                If Not parametroMomentoNumerazioneSettore Is Nothing Then
                    numerazioneAttivata = (parametroMomentoNumerazioneSettore.Valore <> "0")
                End If
                Me.NumeroSettoreTable.Visible = numerazioneAttivata

                If numerazioneAttivata Then
                    Dim parametroNumerazioneManuale As ParsecAdmin.Parametri = parametri.GetByName("abilitaContatoreSettoreDetermineManuale", ParsecAdmin.TipoModulo.ATT)
                    Dim modalitaManualeAttivata As Boolean = False
                    If Not parametroNumerazioneManuale Is Nothing Then
                        modalitaManualeAttivata = CBool(parametroNumerazioneManuale.Valore = "1")


                        '*******************************************************************
                        'MODIFICA PER CARMIANO - MODIFICA NUMERAZIONE SETTORE MANUALE
                        '*******************************************************************
                        If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina Then
                            Dim parametroAbilitaModificaNumerazioneSettore As ParsecAdmin.Parametri = parametri.GetByName("AbilitaModificaNumerazioneSettore", ParsecAdmin.TipoModulo.ATT)

                            If Not parametroAbilitaModificaNumerazioneSettore Is Nothing Then

                                If parametroAbilitaModificaNumerazioneSettore.Valore = "1" Then
                                    If procedura = ParsecAtt.TipoProcedura.Modifica OrElse procedura = ParsecAtt.TipoProcedura.Numerazione Then
                                        Me.NumeroSettoreTextBox.Enabled = modalitaManualeAttivata
                                        modificaNumeroSettore = True
                                    End If
                                End If


                            End If
                        End If
                        '*******************************************************************


                        'EDITABILE
                        If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina Then
                            If procedura = ParsecAtt.TipoProcedura.Nuovo AndAlso parametroMomentoNumerazioneSettore.Valore = "1" Then
                                Me.NumeroSettoreTextBox.Enabled = modalitaManualeAttivata
                            End If
                        End If

                        If procedura = ParsecAtt.TipoProcedura.Numerazione AndAlso parametroMomentoNumerazioneSettore.Valore = "2" Then
                            If Me.Documento.ContatoreStruttura.HasValue Then
                                modalitaManualeAttivata = False
                            End If
                            Me.NumeroSettoreTextBox.Enabled = modalitaManualeAttivata
                        End If


                    End If
                End If
            Else
                '*********************************************************************************************************************************************
                'VISUALIZZO IL NUMERO DI SETTORE ANCHE SE IL PARAMETRO MOMENTONUMERAZIONESETTORE E' IMPOSTATO SU 0 (DISATTIVATO)
                '*********************************************************************************************************************************************
                If Not Me.Documento Is Nothing Then
                    If Me.Documento.ContatoreStruttura.HasValue Then
                        Me.NumeroSettoreTable.Visible = True
                        Me.NumeroSettoreTextBox.Enabled = False
                    End If
                End If
                '*********************************************************************************************************************************************
            End If
        End If
        Select Case procedura
            Case ParsecAtt.TipoProcedura.ModificaAmministrativa
                'Me.AttiTabStrip.Tabs.FindTabByText("Classificazioni").Enabled = True
                '***************************************************************************************
                'SCHEDA GENERALE
                '***************************************************************************************
                Me.NoteTextBox.Enabled = True
                Me.OggettoTextBox.Enabled = True
                Me.DelimitaTestoImageButton.Visible = True
                Me.ModelliComboBox.Enabled = True
                '***************************************************************************************

                '***************************************************************************************
                'VISTI E PARERI
                '***************************************************************************************
                Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = True
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA PRESENZE
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaPresenze(True)
                'Me.TipiApprovazioneComboBox.Enabled = False
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA CONTABILITA
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaContabilita(True)
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA ALLEGATI
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaAllegati(True)
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA CLASSIFICAZIONI
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaClassificazioni(True)
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA VISIBILITA'
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaVisibilita(True)
                '***************************************************************************************

                'luca 01/07/2020
                '' ''Me.ImpostaAbilitazioneSchedaTrasparenza(True)
                '' ''Me.TrovaSezioneImageButton.Visible = False
                '' ''Me.EliminaSezioneImageButton.Visible = False
                '' ''Me.SezioneTrasparenzaTextBox.Enabled = False

                '***************************************************************************************
                'SCHEDA FASCICOLI
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaFascicoli(True)
                '***************************************************************************************
                Me.AffissionePanel.Visible = Not proposta
                Me.ImpostaAbilitazionePannelloAffissione(Not proposta)

            Case ParsecAtt.TipoProcedura.Classificazione

                Me.RadToolBar.FindItemByText("Salva").Enabled = documentoModificabile
                Me.RadToolBar.FindItemByText("Salva").ToolTip = ""

                '***************************************************************************************
                'SCHEDA CLASSIFICAZIONI
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaClassificazioni(documentoModificabile)
                '***************************************************************************************

            Case ParsecAtt.TipoProcedura.Numerazione

                Me.RadToolBar.FindItemByText("Salva").Enabled = True

                '***************************************************************************************
                'SCHEDA GENERALE
                '***************************************************************************************
                Me.NumeroAttoTextBox.Enabled = Me.AbilitaNumerazioneDeliberaManuale And Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDelibera

                Me.DataTextBox.Enabled = Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina



                Me.OggettoTextBox.Enabled = True
                Me.NoteTextBox.Enabled = True

                'Dati Protocollo
                Dim protocolloVisibile As Boolean = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto)
                Me.ProtocolloPanel.Visible = protocolloVisibile

                '*******************************************************
                'VERIFICO SE IL DOCUMENTO ADOTTATO NON E' PUBBLICABILE
                '*******************************************************
                If (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaOrdinanza) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDecreto) Then
                    Dim idModelloAdottato As Nullable(Of Integer) = Me.Documento.Modello.IdModelloAdottato

                    If idModelloAdottato.HasValue Then
                        Dim modelli As New ParsecAtt.ModelliRepository
                        Dim modelloAdottato = modelli.Where(Function(c) c.Id = idModelloAdottato).FirstOrDefault
                        If Not modelloAdottato Is Nothing Then
                            If Not modelloAdottato.Pubblicazione.HasValue OrElse Not modelloAdottato.Pubblicazione Then

                                If Not Me.Documento.NumeroProtocollo.HasValue Then
                                    Me.ProtocolloPanel.Visible = True
                                    Me.TrovaProtocolloImageButton.Visible = True
                                    Me.EliminaProtocolloImageButton.Visible = True
                                End If

                            End If
                        End If
                    End If
                End If


                '***************************************************************************************

                '***************************************************************************************
                'VISTI E PARERI
                '***************************************************************************************
                Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = True
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA PRESENZE
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaPresenze(True)
                '***************************************************************************************
                Dim inIter As Boolean = False

                'Se questa pagina non è stata richiamata dall'iter.
                If Page.Request("Iter") Is Nothing Then
                    'Se per il documento corrente è avviato un iter.
                    If Me.VerificaDocumentoInIter(Documento) Then
                        inIter = True
                        Me.RadToolBar.FindItemByText("Salva").Enabled = False
                        Me.RadToolBar.FindItemByText("Salva").ToolTip = "Sola lettura (In Iter)"
                    End If

                    If Me.Documento.IdFiglio.HasValue Then
                        Me.RadToolBar.FindItemByText("Salva").Enabled = False
                        If Not inIter Then
                            Me.RadToolBar.FindItemByText("Salva").ToolTip = "Sola lettura (Numerata)"
                        End If
                    End If

                End If





            Case ParsecAtt.TipoProcedura.Pubblicazione

                Dim abilitatoModificaPubblicazione As Boolean = Me.AbilitatoFunzioneRipubblicazione(Me.Documento, utenteCollegato)

                Me.RadToolBar.FindItemByText("Salva").Enabled = abilitatoModificaPubblicazione
                Me.RadToolBar.FindItemByText("Salva").ToolTip = If(Not (abilitatoModificaPubblicazione), "Utente non abilitato alla modifica del tipo di documento selezionato.", "")

                '***************************************************************************************
                'VISTI E PARERI
                '***************************************************************************************
                Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = abilitatoModificaPubblicazione
                '***************************************************************************************

                '***************************************************************************************
                'DATI AFFISSIONE
                '***************************************************************************************
                Me.ImpostaAbilitazionePannelloAffissione(abilitatoModificaPubblicazione)
                Me.AffissionePanel.Visible = True
                '***************************************************************************************

                '***************************************************************************************
                'DATI PROTOCOLLO
                '***************************************************************************************
                Dim protocolloVisibile As Boolean = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto)
                Me.ProtocolloPanel.Visible = protocolloVisibile

                protocolloVisibile = protocolloVisibile And abilitatoModificaPubblicazione And Not Me.Documento.NumeroRegistroPubblicazione.HasValue
                If Not protocolloVisibile Then
                    protocolloVisibile = Not Me.Documento.NumeroProtocollo.HasValue
                End If
                'Me.TrovaProtocolloImageButton.Visible = protocolloVisibile And abilitatoModificaPubblicazione And Not Me.Documento.NumeroRegistroPubblicazione.HasValue
                'Me.EliminaProtocolloImageButton.Visible = protocolloVisibile And abilitatoModificaPubblicazione And Not Me.Documento.NumeroRegistroPubblicazione.HasValue

                Me.TrovaProtocolloImageButton.Visible = protocolloVisibile

                Me.EliminaProtocolloImageButton.Visible = protocolloVisibile

                '***************************************************************************************

            Case ParsecAtt.TipoProcedura.CambioModello

                'Se questa pagina non è stata richiamata dall'iter.
                If Page.Request("Iter") Is Nothing Then
                    'Se per il documento corrente è avviato un iter.
                    If Me.VerificaDocumentoInIter(Documento) Then
                        Me.RadToolBar.FindItemByText("Salva").Enabled = False
                        Me.RadToolBar.FindItemByText("Salva").ToolTip = "Sola lettura (In Iter)"
                    End If
                End If

                'Me.RadToolBar.FindItemByText("Salva").Enabled = documentoModificabile
                'Me.RadToolBar.FindItemByText("Salva").ToolTip = ""

                'Abilito solo il campo del modello
                Me.ModelliComboBox.Enabled = documentoModificabile


                '***************************************************************************************
                'DATI AFFISSIONE
                '***************************************************************************************
                Me.AffissionePanel.Visible = Not proposta
                '***************************************************************************************

                '***************************************************************************************
                'DATI PROTOCOLLO
                '***************************************************************************************
                Dim protocolloVisibile As Boolean = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto)
                Me.ProtocolloPanel.Visible = protocolloVisibile
                '***************************************************************************************


            Case ParsecAtt.TipoProcedura.ModificaParere

                '***************************************************************************************
                'SCHEDA ALLEGATI
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaAllegati(True)
                '***************************************************************************************

                Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = True


            Case ParsecAtt.TipoProcedura.AggiungiDatiContabili

                'Se questa pagina non è stata richiamata dall'iter.
                If Page.Request("Iter") Is Nothing Then
                    'Se per il documento corrente è avviato un iter.
                    If Me.VerificaDocumentoInIter(Documento) Then
                        Me.RadToolBar.FindItemByText("Salva").Enabled = False
                        Me.RadToolBar.FindItemByText("Salva").ToolTip = "Sola lettura (In Iter)"
                    Else
                        '***************************************************************************************
                        'SCHEDA CONTABILITA
                        '***************************************************************************************
                        Me.ImpostaAbilitazioneSchedaContabilita(True)
                        '***************************************************************************************
                    End If
                Else
                    '***************************************************************************************
                    'SCHEDA CONTABILITA
                    '***************************************************************************************
                    Me.ImpostaAbilitazioneSchedaContabilita(True)
                    '***************************************************************************************

                End If



            Case ParsecAtt.TipoProcedura.Ricerca

                If Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.ModificaAmministrativa Then
                    ' Me.TipologieDocumentoComboBox.Enabled = True
                End If

                'Abilito i campi utilizzati per la ricerca
                Me.DataTextBox.Enabled = True
                Me.TrovaUfficioImageButton.Visible = True
                Me.EliminaUfficioImageButton.Visible = True
                Me.OggettoTextBox.Enabled = True
                Me.NumeroAttoTextBox.Enabled = True
                Me.ModelliComboBox.Enabled = True
                Me.RadToolBar.FindItemByText("Salva").Enabled = True
                Me.RadToolBar.FindItemByText("Salva").ToolTip = ""

            Case ParsecAtt.TipoProcedura.Visualizzazione
                Me.AffissionePanel.Visible = True

                '******************************************************************************************
                'SCHEDA FASCICOLI
                '******************************************************************************************

                Me.TrovaFascicoloImageButton.Visible = False
                Me.NuovoFascicoloImageButton.Visible = False
                Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = False
                Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = False
                Me.FaseDocumentoFascicoloLabel.Visible = False
                Me.FaseDocumentoFascicoloComboBox.Visible = False
                '******************************************************************************************


                Me.AllegatiGridView.MasterTableView.GetColumnSafe("Firma").Display = False



            Case ParsecAtt.TipoProcedura.Nuovo

                Dim abilitatoNuovoDocumento As Boolean = Me.AbilitatoFunzioneNuovoDocumento(utenteCollegato)
                Me.RadToolBar.FindItemByText("Salva").Enabled = abilitatoNuovoDocumento
                Me.RadToolBar.FindItemByText("Salva").ToolTip = If(Not abilitatoNuovoDocumento, "Utente non abilitato alla creazione del tipo di documento selezionato.", "")

                Me.RadToolBar.FindItemByText("Nuovo").Enabled = True
                Me.RadToolBar.FindItemByText("Elimina").Enabled = True
                'Me.RadToolBar.FindItemByText("Elimina").Attributes.Add("onclick", "return Confirm();")

                Me.AttiTabStrip.Tabs.FindTabByText("Classificazioni").Enabled = True

                '***************************************************************************************
                'VISTI E PARERI
                '***************************************************************************************
                Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = abilitatoNuovoDocumento
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA GENERALE
                '***************************************************************************************

                'Abilito sempre i campi di ricerca ricerca

                If String.IsNullOrEmpty(Me.NomeFileCopia) Then
                    'Me.TipologieDocumentoComboBox.Enabled = True
                End If

                Me.ModelliComboBox.Enabled = True
                Me.DataTextBox.Enabled = True
                Me.TrovaUfficioImageButton.Visible = True
                Me.EliminaUfficioImageButton.Visible = True
                Me.NumeroAttoTextBox.Enabled = True
                Me.OggettoTextBox.Enabled = True

                Me.NoteTextBox.Enabled = abilitatoNuovoDocumento
                Me.DelimitaTestoImageButton.Visible = abilitatoNuovoDocumento

                Me.TrovaBozzaImageButton.Visible = abilitatoNuovoDocumento
                Me.EliminaBozzaImageButton.Visible = abilitatoNuovoDocumento
                Me.BozzaPanel.Visible = True



                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA PRESENZE
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaPresenze(abilitatoNuovoDocumento)
                Me.TipiApprovazioneComboBox.Enabled = False
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA CONTABILITA
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaContabilita(abilitatoNuovoDocumento)
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA ALLEGATI
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaAllegati(abilitatoNuovoDocumento)
                '***************************************************************************************

                '***************************************************************************************
                'SCHEDA VISIBILITA'
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaVisibilita(abilitatoNuovoDocumento)
                '***************************************************************************************
                'luca 01/07/2020
                '' ''Me.ImpostaAbilitazioneSchedaTrasparenza(abilitatoNuovoDocumento)

                '***************************************************************************************
                'SCHEDA FASCICOLI
                '***************************************************************************************
                Me.ImpostaAbilitazioneSchedaFascicoli(abilitatoNuovoDocumento)
                '***************************************************************************************

            Case ParsecAtt.TipoProcedura.Modifica

                Dim abilitatoCancellazione As Boolean = Me.AbilitatoFunzioneCancellazione(utenteCollegato)

                Dim abilitatoModifica As Boolean = Me.AbilitatoFunzioneModifica(Documento, utenteCollegato)


                Dim abilita As Boolean = (abilitatoModifica And documentoModificabile)

                If Page.Request("Mode") Is Nothing Then

                    Me.RadToolBar.FindItemByText("Nuovo").Enabled = True
                    Me.RadToolBar.FindItemByText("Elimina").Enabled = True

                    Me.RadToolBar.FindItemByText("Elimina").Enabled = abilitatoCancellazione
                    Me.RadToolBar.FindItemByText("Elimina").ToolTip = If(Not abilitatoCancellazione, "Utente non abilitato alla cancellazione del tipo di documento selezionato.", "")
                    'Me.RadToolBar.FindItemByText("Elimina").Attributes.Add("onclick", If(abilitatoCancellazione, "return Confirm();", "return false;"))
                    Me.RadToolBar.FindItemByText("Salva").Enabled = abilita

                End If


                Me.RadToolBar.FindItemByText("Salva").ToolTip = If(Not (abilitatoModifica), "Utente non abilitato alla modifica del tipo di documento selezionato.", "")

                '*******************************************************************
                'MODIFICA PER CARMIANO - MODIFICA NUMERAZIONE SETTORE MANUALE
                '*******************************************************************
                If modificaNumeroSettore Then
                    If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.PropostaDetermina Then
                        Me.NumeroSettoreTextBox.Enabled = modificaNumeroSettore
                    End If
                End If
                '*******************************************************************


                'Se il documento è stato pubblicato
                'Se il documento non è una proposta già numerata.
                'Se l'utente è abilitato alla modifica
                If abilita Then


                    Me.FirmeGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = True
                    Me.OggettoTextBox.Enabled = True
                    Me.DelimitaTestoImageButton.Visible = True
                    Me.NoteTextBox.Enabled = True

                    '***************************************************************************************
                    'SCHEDA PRESENZE
                    '***************************************************************************************
                    Me.ImpostaAbilitazioneSchedaPresenze(True)
                    Me.TipiApprovazioneComboBox.Enabled = False
                    '***************************************************************************************

                    '***************************************************************************************
                    'SCHEDA CONTABILITA
                    '***************************************************************************************
                    Me.ImpostaAbilitazioneSchedaContabilita(True)
                    '***************************************************************************************

                    '***************************************************************************************
                    'SCHEDA ALLEGATI
                    '***************************************************************************************
                    Me.ImpostaAbilitazioneSchedaAllegati(True)
                    '***************************************************************************************

                    '***************************************************************************************
                    'SCHEDA VISIBILITA'
                    '***************************************************************************************
                    Me.ImpostaAbilitazioneSchedaVisibilita(True)
                    '***************************************************************************************

                    '***************************************************************************************
                    'SCHEDA TRASPARENZA 
                    '***************************************************************************************
                    'luca 01/07/2020
                    '' ''Me.ImpostaAbilitazioneSchedaTrasparenza(True)
                    'Me.TrovaSezioneImageButton.Visible = False
                    'Me.EliminaSezioneImageButton.Visible = False
                    'Me.SezioneTrasparenzaTextBox.Enabled = False

                    '***************************************************************************************

                    '***************************************************************************************
                    'SCHEDA FASCICOLI
                    '***************************************************************************************
                    Me.ImpostaAbilitazioneSchedaFascicoli(True)
                    '***************************************************************************************

                End If

                '***************************************************************************************
                'DATI AFFISSIONE
                '***************************************************************************************
                Me.AffissionePanel.Visible = Not proposta
                '***************************************************************************************

                '***************************************************************************************
                'DATI PROTOCOLLO
                '***************************************************************************************
                Dim protocolloVisibile As Boolean = (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Ordinanza) OrElse (Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Decreto)
                Me.ProtocolloPanel.Visible = protocolloVisibile

                If protocolloVisibile Then
                    '*******************************************************
                    'VERIFICO SE IL DOCUMENTO ADOTTATO NON E' PUBBLICABILE
                    '*******************************************************
                    Dim idModello As Nullable(Of Integer) = Me.Documento.IdModello

                    If idModello.HasValue Then
                        Dim modelli As New ParsecAtt.ModelliRepository
                        Dim modelloAdottato = modelli.Where(Function(c) c.Id = idModello).FirstOrDefault
                        If Not modelloAdottato Is Nothing Then
                            If Not modelloAdottato.Pubblicazione.HasValue OrElse Not modelloAdottato.Pubblicazione Then

                                If Not Me.Documento.NumeroProtocollo.HasValue Then
                                    Me.ProtocolloPanel.Visible = True
                                    Me.TrovaProtocolloImageButton.Visible = True
                                    Me.EliminaProtocolloImageButton.Visible = True
                                End If

                            End If
                        End If
                    End If
                End If


                '***************************************************************************************

        End Select

        'Sempre


        Dim contabilitaTab As RadTab = Me.AttiTabStrip.FindTabByText("Contabilità")
        Dim contabilitaSelezionata As Boolean = contabilitaTab.PageView.Selected
        If contabilitaTab.Enabled = False And contabilitaSelezionata Then
            Me.AttiTabStrip.MultiPage.PageViews(0).Selected = True
            Me.AttiTabStrip.Tabs(0).Selected = True
        End If

        'luca 01/07/2020
        '' ''Dim trasparenzaTab As RadTab = Me.AttiTabStrip.FindTabByText("Trasparenza")
        '' ''Dim trasparenzaSelezionata As Boolean = trasparenzaTab.PageView.Selected
        '' ''If trasparenzaTab.Enabled = False And trasparenzaSelezionata Then
        '' ''    Me.AttiTabStrip.MultiPage.PageViews(0).Selected = True
        '' ''    Me.AttiTabStrip.Tabs(0).Selected = True
        '' ''End If

        Dim classificazioneTab As RadTab = Me.AttiTabStrip.FindTabByText("Classificazioni")
        Dim classificazioneSelezionata As Boolean = classificazioneTab.PageView.Selected

        If classificazioneTab.Enabled = False And classificazioneSelezionata Then
            Me.AttiTabStrip.MultiPage.PageViews(0).Selected = True
            Me.AttiTabStrip.Tabs(0).Selected = True
        End If

        Dim presenzeTab As RadTab = Me.AttiTabStrip.FindTabByText("Presenze")
        Dim presenzeSelezionata As Boolean = False
        presenzeSelezionata = presenzeTab.PageView.Selected
        If presenzeTab.Enabled = False And presenzeSelezionata Then
            Me.AttiTabStrip.MultiPage.PageViews(0).Selected = True
            Me.AttiTabStrip.Tabs(0).Selected = True
        End If

        ViewState("SalvaAbilitato") = Me.RadToolBar.FindItemByText("Salva").Enabled

        Dim abilitatoModificaNoteAtti As Boolean = Not utenteCollegato.Funzioni.Where(Function(c) c.Codice = CInt(ParsecAtt.TipologiaFunzione.AbilitaModificaNoteAtti)).FirstOrDefault Is Nothing
        If abilitatoModificaNoteAtti Then
            Me.NoteTextBox.Enabled = True
            If Not Me.RadToolBar.FindItemByText("Salva").Enabled OrElse Not Me.RadToolBar.Visible Then
                Me.SalvaNoteButton.Visible = True
            End If
        End If


    End Sub

#End Region

#Region "GESTIONE VISIBILITA'"

    Protected Sub TrovaGruppoVisibilitaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaGruppoVisibilitaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaGruppoPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaGruppoVisibilitaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaGruppoVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaGruppoVisibilitaImageButton.Click
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        If Not Session("GruppiSelezionati") Is Nothing Then
            Dim gruppiSelezionati As SortedList(Of Integer, String) = Session("GruppiSelezionati")
            Dim idGruppo As Integer = 0
            For Each gruppoSelezionato In gruppiSelezionati
                idGruppo = gruppoSelezionato.Key
                Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = idGruppo And c.TipoEntita = ParsecAdmin.TipoEntita.Gruppo).FirstOrDefault Is Nothing
                If Not esiste Then
                    Dim gruppoVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetGruppoVisibilita(gruppoSelezionato)
                    Me.AggiungiGruppoUtenteVisibilita(gruppoVisibilita)
                End If
            Next
            Session("GruppiSelezionati") = Nothing
        End If
    End Sub

    Protected Sub TrovaUtenteVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteVisibilitaImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteVisibilitaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'MULTIPLA
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaUtenteVisibilitaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteVisibilitaImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim idUtente As Integer = 0
            For Each utenteSelezionato In utentiSelezionati
                idUtente = utenteSelezionato.Key
                Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = idUtente And c.TipoEntita = ParsecAdmin.TipoEntita.Utente).FirstOrDefault Is Nothing
                If Not esiste Then
                    Dim utenti As New ParsecAdmin.UserRepository
                    Dim utente As ParsecAdmin.Utente = utenti.GetUserById(utenteSelezionato.Key).FirstOrDefault
                    If Not utente Is Nothing Then
                        Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetUtenteVisibilita(utente)
                        Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)
                    End If
                End If
            Next
            Session("SelectedUsers") = Nothing
        End If
    End Sub


    Protected Sub VisibilitaGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles VisibilitaGridView.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                EliminaGruppoUtente(e)
        End Select
    End Sub

    Protected Sub VisibilitaGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles VisibilitaGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Delete").Controls(0) Is ImageButton Then
                Dim gruppoUtente As ParsecAdmin.VisibilitaDocumento = CType(e.Item.DataItem, ParsecAdmin.VisibilitaDocumento)
                btn = CType(dataItem("Delete").Controls(0), ImageButton)
                Dim message As String = "Eliminare l'elemento selezionato?"
                If Not gruppoUtente.AbilitaCancellaEntita Then
                    message = "L'elemento selezionato non può essere cancellato!"
                    btn.Attributes.Add("onclick", "alert(""" & message & """);return false;")
                Else
                    btn.Attributes.Add("onclick", "return confirm(""" & message & """)")
                End If
            End If
        End If
    End Sub

    Private Sub EliminaGruppoUtente(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim idEntita As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("IdEntita"))
        Dim tipoEntita As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("TipoEntita"))
        Dim gruppoUtente As ParsecAdmin.VisibilitaDocumento = Me.Visibilita.Where(Function(c) c.IdEntita = idEntita And c.TipoEntita = tipoEntita).FirstOrDefault
        If Not gruppoUtente Is Nothing Then
            Me.Visibilita.Remove(gruppoUtente)
        End If
    End Sub

    Private Sub AggiungiGruppoUtenteVisibilita(gruppoUtente As ParsecAdmin.VisibilitaDocumento)
        Dim esiste As Boolean = Not Me.Visibilita.Where(Function(c) c.IdEntita = gruppoUtente.IdEntita And c.TipoEntita = gruppoUtente.TipoEntita).FirstOrDefault Is Nothing
        If Not esiste Then
            Me.Visibilita.Add(gruppoUtente)
        End If
    End Sub

    Private Sub AggiungiGruppoVisibilitaTuttiUtenti()
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim gruppi As New ParsecAdmin.GruppoRepository
        Dim gruppoTuttiUtenti As ParsecAdmin.Gruppo = gruppi.GetQuery.Where(Function(c) c.Id = 1).FirstOrDefault
        If Not gruppoTuttiUtenti Is Nothing Then
            Dim gruppoVisibilita As New ParsecAdmin.VisibilitaDocumento
            gruppoVisibilita.IdEntita = gruppoTuttiUtenti.Id
            gruppoVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
            gruppoVisibilita.IdModulo = ParsecAdmin.TipoModulo.ATT
            gruppoVisibilita.Descrizione = gruppoTuttiUtenti.Descrizione
            gruppoVisibilita.LogIdUtente = utenteCollegato.Id
            gruppoVisibilita.LogDataOperazione = Now
            gruppoVisibilita.AbilitaCancellaEntita = False
            Me.AggiungiGruppoUtenteVisibilita(gruppoVisibilita)
        End If
    End Sub

    Private Sub AggiungiUtenteVisibilita(ByVal idUtente As Integer)
        Dim utenti As New ParsecAdmin.UserRepository
        Dim utente As ParsecAdmin.Utente = utenti.GetQuery.Where(Function(c) c.Id = idUtente).FirstOrDefault
        If Not utente Is Nothing Then
            Dim utenteVisibilita As ParsecAdmin.VisibilitaDocumento = Me.GetUtenteVisibilita(utente)
            Me.AggiungiGruppoUtenteVisibilita(utenteVisibilita)
        End If
        utenti.Dispose()
    End Sub

    Private Function GetUtenteVisibilita(ByVal utente As ParsecAdmin.Utente) As ParsecAdmin.VisibilitaDocumento
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim utenteVisibilita As New ParsecAdmin.VisibilitaDocumento
        utenteVisibilita.AbilitaCancellaEntita = True
        utenteVisibilita.Descrizione = (If(utente.Username = Nothing, "", utente.Username) + " - " + If(utente.Cognome = Nothing, "", utente.Cognome) + " " + If(utente.Nome = Nothing, "", utente.Nome))
        utenteVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Utente
        utenteVisibilita.IdEntita = utente.Id
        utenteVisibilita.IdModulo = ParsecAdmin.TipoModulo.ATT
        utenteVisibilita.LogIdUtente = utenteCollegato.Id
        utenteVisibilita.LogDataOperazione = Now
        Return utenteVisibilita
    End Function


    Private Function GetGruppoVisibilita(ByVal gruppoSelezionato As KeyValuePair(Of Integer, String)) As ParsecAdmin.VisibilitaDocumento
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim gruppoVisibilita As New ParsecAdmin.VisibilitaDocumento
        gruppoVisibilita.IdEntita = gruppoSelezionato.Key
        gruppoVisibilita.TipoEntita = ParsecAdmin.TipoEntita.Gruppo
        gruppoVisibilita.IdModulo = ParsecAdmin.TipoModulo.ATT
        gruppoVisibilita.Descrizione = gruppoSelezionato.Value
        gruppoVisibilita.LogIdUtente = utenteCollegato.Id
        gruppoVisibilita.LogDataOperazione = Now
        gruppoVisibilita.AbilitaCancellaEntita = True
        Return gruppoVisibilita
    End Function

#End Region

#Region "GESTIONE TRASPARENZA"
    'luca 01/07/2020
    '' ''Private Sub aggiornaSottoSezione(ByVal idSottoSezione As Integer?)
    '' ''    Dim clienti As New ParsecAdmin.ClientRepository
    '' ''    Dim cliente = clienti.GetQuery.FirstOrDefault
    '' ''    clienti.Dispose()

    '' ''    Dim codiceEnte As String = cliente.Identificativo
    '' ''    Try
    '' ''        ParsecWebServices.TipologiaPubblicazione.servizio.isAlive()
    '' ''        Dim listaSezioni = ParsecWebServices.TipologiaPubblicazione.servizio.getTipologiePubblicazione(codiceEnte, Me.IdSezioneTrasparenzaTextBox.Text)

    '' ''        If (Not listaSezioni Is Nothing) Then
    '' ''            Me.comboboxSottoSezione.Items.Clear()
    '' ''            comboboxSottoSezione.DataSource = listaSezioni
    '' ''            comboboxSottoSezione.DataTextField = "denominazione"
    '' ''            comboboxSottoSezione.DataValueField = "tipologiaId"
    '' ''            comboboxSottoSezione.DataBind()
    '' ''            comboboxSottoSezione.Items.Insert(0, New RadComboBoxItem("", -1))
    '' ''            comboboxSottoSezione.SelectedIndex = 0
    '' ''        End If
    '' ''        ParsecWebServices.TipologiaPubblicazione.servizio.Dispose()
    '' ''        If (idSottoSezione.HasValue And idSottoSezione > 0) Then
    '' ''            Me.comboboxSottoSezione.SelectedValue = idSottoSezione
    '' ''        End If
    '' ''    Catch ex As Exception
    '' ''        Me.comboboxSottoSezione.Items.Insert(0, New RadComboBoxItem("", -1))
    '' ''    End Try

    '' ''End Sub

    Private Sub CaricaTipologieAllegati()
        Me.TipologiaAllegatoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Allegato", "0"))
        Me.TipologiaAllegatoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Contratto", "1"))
        Me.TipologiaAllegatoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Curriculum", "2"))
        Me.TipologiaAllegatoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Elenco Beneficiari", "3"))
        Me.TipologiaAllegatoComboBox.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Insussistenza Conflitto Interessi", "4"))
        Me.TipologiaAllegatoComboBox.SelectedIndex = 0
    End Sub

    'luca 01/07/2020
    '' ''Private Sub CaricaTipologieSceltaContraente()
    '' ''    Dim tipologie As New ParsecAdmin.CriterioSceltaContraenteRepository
    '' ''    Me.TipologiaSceltaComboBox.DataValueField = "Id"
    '' ''    Me.TipologiaSceltaComboBox.DataTextField = "Descrizione"
    '' ''    Me.TipologiaSceltaComboBox.DataSource = tipologie.GetKeyValue()
    '' ''    Me.TipologiaSceltaComboBox.DataBind()
    '' ''    Me.TipologiaSceltaComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
    '' ''    Me.TipologiaSceltaComboBox.SelectedIndex = 0
    '' ''    tipologie.Dispose()
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub ResettaVistaPannelliTrasparenza()


    '' ''    Me.SezioneTrasparenzaTextBox.Text = String.Empty
    '' ''    Me.IdSezioneTrasparenzaTextBox.Text = String.Empty
    '' ''    Me.comboboxSottoSezione.SelectedValue = String.Empty
    '' ''    'Me.comboboxSottoSezione.Enabled = False

    '' ''    Me.comboboxSottoSezione.Items.Clear()
    '' ''    Me.comboboxSottoSezione.Text = String.Empty

    '' ''    Me.DataInizioPubblicazioneTextBox.Clear()
    '' ''    Me.DataFinePubblicazioneTextBox.Clear()

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO DATI PUBBLICAZIONE
    '' ''    '*******************************************************************************

    '' ''    Me.DataInizioPubblicazioneTextBox.DateInput.ReadOnly = True
    '' ''    Me.DataInizioPubblicazioneTextBox.DatePopupButton.Visible = False
    '' ''    Me.DataInizioPubblicazioneTextBox.DateInput.Attributes.Add("onkeydown", "event.returnValue=false;")

    '' ''    Me.DataFinePubblicazioneTextBox.DateInput.ReadOnly = True
    '' ''    Me.DataFinePubblicazioneTextBox.DatePopupButton.Visible = False
    '' ''    Me.DataFinePubblicazioneTextBox.DateInput.Attributes.Add("onkeydown", "event.returnValue=false;")


    '' ''    'Me.TitoloPubblicazioneTextBox.Text = String.Empty
    '' ''    'Me.DescrizionePubblicazioneTextBox.Text = String.Empty

    '' ''    If Me.TipologiaDocumento = ParsecAtt.TipoDocumento.Determina OrElse Me.TipologiaProceduraApertura = ParsecAtt.TipoProcedura.Numerazione Then
    '' ''        Me.DataInizioPubblicazioneTextBox.SelectedDate = Now
    '' ''        Me.DataFinePubblicazioneTextBox.SelectedDate = If(Now.DayOfYear = 1, New DateTime(Now.Year, 12, 31).AddYears(4), New DateTime(Now.Year, 12, 31).AddYears(5))

    '' ''    End If



    '' ''    '*******************************************************************************


    '' ''    '*******************************************************************************
    '' ''    'PANNELLO ATTI DI CONCESSIONE
    '' ''    '*******************************************************************************
    '' ''    Me.AttiConcessione = New List(Of ParsecAdmin.AttoConcessione)

    '' ''    '*******************************************************************************

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO INCARICHI CONFERITI E AUTORIZZATI AI DIPENDENTI
    '' ''    '*******************************************************************************
    '' ''    Me.DataInizioIncaricoDipendenteTextBox.SelectedDate = Nothing
    '' ''    Me.DataFineIncaricoDipendenteTextBox.SelectedDate = Nothing
    '' ''    Me.BeneficiarioIncaricoComboBox.Text = String.Empty
    '' ''    Me.OggettoIncaricoDipendenteTextBox.Text = String.Empty
    '' ''    Me.RagioneIncaricoDipendenteTextBox.Text = String.Empty
    '' ''    Me.CompensoIncaricoDipendenteTextBox.Value = Nothing
    '' ''    Me.BeneficiarioIncaricoComboBox.SelectedValue = String.Empty
    '' ''    '*******************************************************************************

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO BANDI DI GARA E CONTRATTI
    '' ''    '*******************************************************************************
    '' ''    Me.FiltroBandoGaraTextBox.Text = String.Empty
    '' ''    Me.OggettoBandoGaraTextBox.Text = String.Empty
    '' ''    Me.PartecipantiListBox.Items.Clear()
    '' ''    Me.AggiudicatariListBox.Items.Clear()
    '' ''    Me.CigBandoGaraTextBox.Text = String.Empty
    '' ''    Me.ImportoAggiudicazioneTextBox.Value = Nothing
    '' ''    Me.ImportoLiquidatoTextBox.Value = Nothing
    '' ''    Me.NumeroOfferentiTextBox.Text = String.Empty
    '' ''    Me.DataInizioLavoroTextBox.SelectedDate = Nothing
    '' ''    Me.DataFineLavoroTextBox.SelectedDate = Nothing
    '' ''    Me.TipologiaSceltaComboBox.SelectedIndex = 0
    '' ''    Me.DenominazioneStrutturaProponenteTextBox.Text = String.Empty
    '' ''    Me.CodiceFiscaleProponenteTextBox.Text = String.Empty
    '' ''    '*******************************************************************************

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO CONSULENTI E COLLABORATORI
    '' ''    '*******************************************************************************
    '' ''    Me.BeneficiarioConsulenzaComboBox.Text = String.Empty
    '' ''    Me.BeneficiarioConsulenzaComboBox.SelectedValue = String.Empty
    '' ''    Me.DenominazioneConsulenzaTextBox.Text = String.Empty
    '' ''    Me.RagioneIncaricoConsulenzaTextBox.Text = String.Empty
    '' ''    Me.CompensoConsulenzaTextBox.Text = String.Empty
    '' ''    Me.VariabileCompensoConsulenzaTextBox.Text = String.Empty
    '' ''    Me.altreCaricheTextBox.Text = String.Empty
    '' ''    Me.altriIncarichiTextBox.Text = String.Empty
    '' ''    Me.altreAttivitaProfessionaliTextBox.Text = String.Empty
    '' ''    Me.DataInizioIncaricoConsulenzaTextBox.SelectedDate = Nothing
    '' ''    Me.DataFineIncaricoConsulenzaTextBox.SelectedDate = Nothing

    '' ''    Me.DataAttoIncaricoConsulenzaCollaborazioneTextBox.SelectedDate = Nothing
    '' ''    Me.NumeroAttoIncaricoConsulenzaCollaborazioneTextBox.Text = String.Empty

    '' ''    Me.CurriculumAllegatoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileCurriculumLabel.Text = String.Empty
    '' ''    Me.curriculumUpload1.Visible = True
    '' ''    Me.curriculumUpload2.Visible = False

    '' ''    Me.InconsistenzaAllegatoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileInsussistenzaLabel.Text = String.Empty
    '' ''    Me.InconsistenzaUpload1.Visible = True
    '' ''    Me.InconsistenzaUpload2.Visible = False
    '' ''    '*******************************************************************************


    '' ''    '*******************************************************************************
    '' ''    'PANNELLO PUBBLICAZIONI INCARICO AMMINISTRATIVO-DIRIGENZIALE
    '' ''    '*******************************************************************************

    '' ''    Me.TitolareIncaricoComboBox.Text = String.Empty
    '' ''    Me.TitolareIncaricoComboBox.SelectedValue = String.Empty
    '' ''    Me.DenominazioneIncaricoAmministrativoTextBox.Text = String.Empty
    '' ''    Me.RagioneIncaricoAmministrativoTextBox.Text = String.Empty
    '' ''    Me.DataInizioIncaricoAmministrativoTextBox.SelectedDate = Nothing
    '' ''    Me.DataFineIncaricoAmministrativoTextBox.SelectedDate = Nothing
    '' ''    Me.CompensoIncaricoAmministrativoTextBox.Text = String.Empty
    '' ''    Me.CompensiVariabiliIncaricoAmministrativoTextBox.Text = String.Empty
    '' ''    Me.CaricheIncaricoAmministrativoTextBox.Text = String.Empty
    '' ''    Me.AltriIncarichiIncaricoAmministrativoTextBox.Text = String.Empty
    '' ''    Me.AltreAttivitaIncaricoAmministrativoTextBox.Text = String.Empty
    '' ''    Me.NumeroAttoIncaricoAmministrativoTextBox.Text = String.Empty
    '' ''    Me.DataAttoIncaricoAmministrativoTextBox.SelectedDate = Nothing


    '' ''    Me.CurriculumIncaricoAmministrativoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileCurriculumIncaricoAmministrativoLabel.Text = String.Empty
    '' ''    Me.curriculumincaricoAmministrativoUpload1.Visible = True
    '' ''    Me.curriculumincaricoAmministrativoUpload2.Visible = False

    '' ''    Me.InconferibilitaIncaricoAmministrativoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileInconferibilitaIncaricoAmministrativoLabel.Text = String.Empty
    '' ''    Me.inconferibilitaIncaricoAmministrativoUpload1.Visible = True
    '' ''    Me.InconferibilitaIncaricoAmministrativoUpload2.Visible = False


    '' ''    Me.NomeFileIncompatibilitaIncaricoAmministrativoLabel.Text = String.Empty
    '' ''    Me.incompatibilitaIncaricoAmministrativoUpload1.Visible = True
    '' ''    Me.incompatibilitaIncaricoAmministrativoUpload2.Visible = False

    '' ''    '*******************************************************************************


    '' ''    '*******************************************************************************
    '' ''    'PANNELLO PUBBLICAZIONI ENTE CONTROLLATO
    '' ''    '*******************************************************************************

    '' ''    Me.RagioneSocialeEnteControllatoCombobox.Text = String.Empty
    '' ''    Me.RagioneSocialeEnteControllatoCombobox.SelectedValue = String.Empty
    '' ''    Me.AttivitaFavoreAmministrazioneEnteControllatoTextBox.Text = String.Empty
    '' ''    Me.AttivitaServizioPubblicoEnteControllatoTextBox.Text = String.Empty
    '' ''    Me.DurataImpegnoEnteControllatoTextbox.Text = String.Empty
    '' ''    Me.FunzioniAttribuiteEnteControllatoTextBox.Text = String.Empty
    '' ''    Me.MisuraPartecipazioneEnteControllatoTextBox.Text = String.Empty
    '' ''    Me.UrlSitoIstituzionaleEnteControllatoTextBox.Text = String.Empty
    '' ''    Me.OnereComplessivoEnteControllatoTextBox.Value = Nothing
    '' ''    Me.NumeroRappresentantiEnteControllatoTextBox.Value = Nothing
    '' ''    Me.EntitaSocietaPartecipateTextBox.Value = Nothing

    '' ''    Me.incompatibilitaUpload1.Visible = True
    '' ''    Me.inconferibilitaUpload1.Visible = True

    '' ''    Me.incompatibilitaUpload2.Visible = False
    '' ''    Me.InconferibilitaUpload2.Visible = False

    '' ''    Me.IncompatibilitaAllegatoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileIncompatibilitaLabel.Text = String.Empty

    '' ''    Me.InconferibilitaAllegatoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileInconferibilitaLabel.Text = String.Empty




    '' ''    Me.TrattamentiEconomiciRappresentante = New List(Of ParsecAdmin.TrattamentoEconomicoRappresentante)
    '' ''    Me.TrattamentiEconomiciIncaricoAmministratore = New List(Of ParsecAdmin.TrattamentoEconomicoIncaricoAmministratore)
    '' ''    Me.BilanciEnteControllato = New List(Of ParsecAdmin.BilancioEsercizioEnteControllato)

    '' ''    '*******************************************************************************


    '' ''    '*******************************************************************************
    '' ''    'PANNELLO BANDO DI CONCORSO
    '' ''    '*******************************************************************************

    '' ''    Me.BandoConcorsoTipoAssunzioneTextBox.Text = String.Empty
    '' ''    Me.BandoConcorsoOggettoTextBox.Text = String.Empty
    '' ''    Me.BandoConcorsoProfiloTextBox.Text = String.Empty
    '' ''    Me.BandoConcorsoCategoriaTextBox.Text = String.Empty
    '' ''    Me.BandoConcorsoSpesaTextBox.Text = String.Empty
    '' ''    Me.BandoConcorsoNumeroDipendentiAssuntiTextBox.Text = String.Empty
    '' ''    Me.BandoConcorsoEstremiDocumentiTextBox.Text = String.Empty

    '' ''    '*******************************************************************************

    '' ''    '*******************************************************************************
    '' ''    'PANNELLO PUBBLICAZIONI GENERICHE
    '' ''    '*******************************************************************************

    '' ''    Me.PubblicazioneContenutoTextBox.Text = String.Empty
    '' ''    Me.PubblicazioneTitoloTextBox.Text = String.Empty
    '' ''    Me.PubblicazioneNumeroTextBox.Value = Nothing

    '' ''    Me.PubblicazioneInizioRiferimentoTextBox.SelectedDate = Nothing
    '' ''    Me.PubblicazioneFineRiferimentoTextBox.SelectedDate = Nothing

    '' ''    Me.AllegatiPubblicazione = New List(Of ParsecAdmin.AllegatoPubblicazione)

    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub AggiornaVistaPannelliTrasparenza(trasparenza As ParsecAdmin.Pubblicazione, ByVal selezione As Boolean)

    '' ''    'Me.ResettaVistaPannelliTrasparenza()
    '' ''    'Me.TitoloPubblicazioneTextBox.Text = trasparenza.Titolo
    '' ''    'Me.DescrizionePubblicazioneTextBox.Text = trasparenza.Contenuto

    '' ''    Me.SezioneTrasparenzaTextBox.Text = trasparenza.SezioneTrasparente
    '' ''    Me.IdSezioneTrasparenzaTextBox.Text = trasparenza.IdSezione.ToString
    '' ''    aggiornaSottoSezione(trasparenza.IdSottoSezione)


    '' ''    If Me.TipologiaProceduraApertura <> ParsecAtt.TipoProcedura.Numerazione Then
    '' ''        Me.DataInizioPubblicazioneTextBox.SelectedDate = trasparenza.DataInizioPubblicazione
    '' ''        Me.DataFinePubblicazioneTextBox.SelectedDate = trasparenza.DataFinePubblicazione
    '' ''    End If



    '' ''    Select Case trasparenza.TipologiaSezioneTrasparente

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''            For Each procedura In trasparenza.ProcedureAffidamento
    '' ''                Me.AttiConcessione.Add(procedura)

    '' ''                If selezione Then
    '' ''                    Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                    Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(CType(procedura, ParsecAdmin.AttoConcessione).IdPubblicazione)
    '' ''                    pubblicazioni.Dispose()
    '' ''                End If

    '' ''            Next

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti

    '' ''            Dim incarico As ParsecAdmin.IncaricoDipendente = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo

    '' ''            Me.DataInizioIncaricoDipendenteTextBox.SelectedDate = incarico.DataInizio
    '' ''            Me.DataFineIncaricoDipendenteTextBox.SelectedDate = incarico.DataFine
    '' ''            Me.BeneficiarioIncaricoComboBox.Text = incarico.Beneficiario
    '' ''            Me.OggettoIncaricoDipendenteTextBox.Text = incarico.Oggetto
    '' ''            Me.RagioneIncaricoDipendenteTextBox.Text = incarico.Ragione
    '' ''            Me.CompensoIncaricoDipendenteTextBox.Value = incarico.Compenso

    '' ''            Me.BeneficiarioIncaricoComboBox.SelectedValue = String.Empty
    '' ''            Me.BeneficiarioIncaricoComboBox.Text = incarico.Beneficiario

    '' ''            If incarico.IdBeneficiario.HasValue Then
    '' ''                Me.BeneficiarioIncaricoComboBox.SelectedValue = incarico.IdBeneficiario
    '' ''                Dim elemento = Me.GetElementoRubrica(incarico.IdBeneficiario)
    '' ''                If Not elemento Is Nothing Then
    '' ''                    Me.BeneficiarioIncaricoComboBox.Text = elemento.Denominazione & " " & If(Not String.IsNullOrEmpty(elemento.Nome), elemento.Nome & ", ", "") & If(Not String.IsNullOrEmpty(elemento.Indirizzo), elemento.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(elemento.Comune), elemento.Comune & " ", "") & If(Not String.IsNullOrEmpty(elemento.CAP), elemento.CAP & " ", "") & If(Not String.IsNullOrEmpty(elemento.Provincia), "(" & elemento.Provincia & ")", "")
    '' ''                End If
    '' ''            End If

    '' ''            If selezione Then
    '' ''                Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(incarico.IdPubblicazione)
    '' ''                pubblicazioni.Dispose()
    '' ''            End If

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''            If Not trasparenza.ProcedureAffidamento(0) Is Nothing Then
    '' ''                Dim bandoGara As ParsecAdmin.BandoGara = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''                Me.OggettoBandoGaraTextBox.Text = bandoGara.Oggetto


    '' ''                If bandoGara.idGaraContratti.HasValue Then
    '' ''                    Me.IdGaraContratti = bandoGara.idGaraContratti.Value.ToString
    '' ''                End If

    '' ''                Dim item As Telerik.Web.UI.RadListBoxItem = Nothing

    '' ''                If Not String.IsNullOrEmpty(bandoGara.Partecipanti) Then
    '' ''                    Dim partecipanti As String() = bandoGara.Partecipanti.Split(New Char() {";"})
    '' ''                    For Each partecipante As String In partecipanti
    '' ''                        item = New Telerik.Web.UI.RadListBoxItem(partecipante.Replace(";", ","), partecipante.Replace(";", ","))
    '' ''                        Me.PartecipantiListBox.Items.Add(item)
    '' ''                    Next
    '' ''                End If

    '' ''                If Not String.IsNullOrEmpty(bandoGara.Aggiudicatario) Then
    '' ''                    Dim aggiudicatari As String() = bandoGara.Aggiudicatario.Split(New Char() {";"})
    '' ''                    For Each aggiudicatario As String In aggiudicatari
    '' ''                        item = New Telerik.Web.UI.RadListBoxItem(aggiudicatario.Replace(";", ","), aggiudicatario.Replace(";", ","))
    '' ''                        Me.AggiudicatariListBox.Items.Add(item)
    '' ''                    Next
    '' ''                End If


    '' ''                If Not String.IsNullOrEmpty(bandoGara.StrutturaProponente) Then
    '' ''                    Me.DenominazioneStrutturaProponenteTextBox.Text = bandoGara.StrutturaProponente
    '' ''                End If

    '' ''                If Not String.IsNullOrEmpty(bandoGara.CodiceFiscaleStrutturaProponente) Then
    '' ''                    Me.CodiceFiscaleProponenteTextBox.Text = bandoGara.CodiceFiscaleStrutturaProponente
    '' ''                End If


    '' ''                Me.CigBandoGaraTextBox.Text = bandoGara.Cig
    '' ''                Me.ImportoAggiudicazioneTextBox.Value = bandoGara.ImportoAggiudicazione
    '' ''                Me.ImportoLiquidatoTextBox.Value = bandoGara.ImportoLiquidato
    '' ''                Me.NumeroOfferentiTextBox.Value = bandoGara.NumeroOfferenti
    '' ''                Me.DataInizioLavoroTextBox.SelectedDate = bandoGara.DataInizioOpera
    '' ''                Me.DataFineLavoroTextBox.SelectedDate = bandoGara.DataFineOpera
    '' ''                Me.TipologiaSceltaComboBox.SelectedValue = 0

    '' ''                If Not String.IsNullOrEmpty(bandoGara.TipologiaSceltaContraente) Then
    '' ''                    Dim crieteriScelta As New ParsecAdmin.CriterioSceltaContraenteRepository
    '' ''                    Dim criterio = crieteriScelta.GetKeyValue().Where(Function(c) c.Descrizione.ToUpper = bandoGara.TipologiaSceltaContraente.ToUpper).FirstOrDefault()
    '' ''                    If Not criterio Is Nothing Then
    '' ''                        Me.TipologiaSceltaComboBox.SelectedValue = criterio.Id
    '' ''                    End If
    '' ''                End If

    '' ''                If selezione Then
    '' ''                    Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                    Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(bandoGara.IdPubblicazione)
    '' ''                    pubblicazioni.Dispose()
    '' ''                End If


    '' ''            End If

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori

    '' ''            Dim consulenza As ParsecAdmin.Consulenza = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''            Me.Consulenza = consulenza
    '' ''            Me.BeneficiarioConsulenzaComboBox.SelectedValue = consulenza.IdBeneficiario
    '' ''            Me.BeneficiarioConsulenzaComboBox.Text = consulenza.Beneficiario
    '' ''            Me.DenominazioneConsulenzaTextBox.Text = consulenza.Oggetto
    '' ''            Me.RagioneIncaricoConsulenzaTextBox.Text = consulenza.RagioneIncarico
    '' ''            Me.CompensoConsulenzaTextBox.Text = consulenza.Compenso
    '' ''            Me.VariabileCompensoConsulenzaTextBox.Text = consulenza.CompensoVariabile
    '' ''            Me.altreCaricheTextBox.Text = consulenza.AltreCariche
    '' ''            Me.altriIncarichiTextBox.Text = consulenza.DatiAltriIncarichi
    '' ''            Me.altreAttivitaProfessionaliTextBox.Text = consulenza.DatiAltreAttivitàProfessionali
    '' ''            Me.DataInizioIncaricoConsulenzaTextBox.SelectedDate = consulenza.DataInizioIncaricoConsulenza
    '' ''            Me.DataFineIncaricoConsulenzaTextBox.SelectedDate = consulenza.DataFineIncaricoConsulenza


    '' ''            Me.NumeroAttoIncaricoConsulenzaCollaborazioneTextBox.Value = consulenza.NumeroDetermina


    '' ''            Me.DataAttoIncaricoConsulenzaCollaborazioneTextBox.SelectedDate = consulenza.DataDetermina

    '' ''            If Not String.IsNullOrEmpty(consulenza.UrlCv) Then
    '' ''                Me.CurriculumAllegatoLinkButton.Text = consulenza.UrlCv
    '' ''                Me.NomeFileCurriculumLabel.Text = consulenza.CurriculumTemp
    '' ''                Me.curriculumUpload1.Visible = False
    '' ''                Me.curriculumUpload2.Visible = True
    '' ''            Else
    '' ''                Me.CurriculumAllegatoLinkButton.Text = String.Empty
    '' ''                Me.NomeFileCurriculumLabel.Text = String.Empty
    '' ''                Me.curriculumUpload1.Visible = True
    '' ''                Me.curriculumUpload2.Visible = False
    '' ''            End If


    '' ''            If Not String.IsNullOrEmpty(consulenza.UrlAttestazioneInsussistenzaConflittoInteressi) Then
    '' ''                Me.InconsistenzaAllegatoLinkButton.Text = consulenza.UrlAttestazioneInsussistenzaConflittoInteressi
    '' ''                Me.NomeFileInsussistenzaLabel.Text = consulenza.InsussistenzaTemp
    '' ''                Me.InconsistenzaUpload1.Visible = False
    '' ''                Me.InconsistenzaUpload2.Visible = True
    '' ''            Else
    '' ''                Me.InconsistenzaAllegatoLinkButton.Text = String.Empty
    '' ''                Me.NomeFileInsussistenzaLabel.Text = String.Empty
    '' ''                Me.InconsistenzaUpload1.Visible = True
    '' ''                Me.InconsistenzaUpload2.Visible = False
    '' ''            End If


    '' ''            If selezione Then
    '' ''                Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(consulenza.IdPubblicazione)
    '' ''                pubblicazioni.Dispose()
    '' ''            End If

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale

    '' ''            Dim incaricoAmministrativoDirigenziale As ParsecAdmin.IncaricoAmministrativoDirigenziale = trasparenza.ProcedureAffidamento(0)
    '' ''            Me.IncaricoAmministrativoDirigenziale = incaricoAmministrativoDirigenziale

    '' ''            Me.TitolareIncaricoComboBox.Text = incaricoAmministrativoDirigenziale.titolare
    '' ''            Me.TitolareIncaricoComboBox.SelectedValue = String.Empty

    '' ''            If incaricoAmministrativoDirigenziale.idTitolare.HasValue Then
    '' ''                Me.TitolareIncaricoComboBox.SelectedValue = incaricoAmministrativoDirigenziale.idTitolare
    '' ''                Dim elemento = GetElementoRubrica(incaricoAmministrativoDirigenziale.idTitolare)
    '' ''                If Not elemento Is Nothing Then
    '' ''                    Me.TitolareIncaricoComboBox.Text = elemento.Denominazione & " " & If(Not String.IsNullOrEmpty(elemento.Nome), elemento.Nome & ", ", "") & If(Not String.IsNullOrEmpty(elemento.Indirizzo), elemento.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(elemento.Comune), elemento.Comune & " ", "") & If(Not String.IsNullOrEmpty(elemento.CAP), elemento.CAP & " ", "") & If(Not String.IsNullOrEmpty(elemento.Provincia), "(" & elemento.Provincia & ")", "")
    '' ''                End If
    '' ''            End If

    '' ''            Me.CaricheIncaricoAmministrativoTextBox.Text = incaricoAmministrativoDirigenziale.altreCariche
    '' ''            Me.DenominazioneIncaricoAmministrativoTextBox.Text = incaricoAmministrativoDirigenziale.denominazione
    '' ''            Me.RagioneIncaricoAmministrativoTextBox.Text = incaricoAmministrativoDirigenziale.ragioneIncarico
    '' ''            Me.CompensoIncaricoAmministrativoTextBox.Text = incaricoAmministrativoDirigenziale.compenso
    '' ''            Me.CompensiVariabiliIncaricoAmministrativoTextBox.Text = incaricoAmministrativoDirigenziale.compensoVariabile
    '' ''            Me.DataInizioIncaricoAmministrativoTextBox.SelectedDate = incaricoAmministrativoDirigenziale.dal
    '' ''            Me.DataFineIncaricoAmministrativoTextBox.SelectedDate = incaricoAmministrativoDirigenziale.al
    '' ''            Me.AltriIncarichiIncaricoAmministrativoTextBox.Text = incaricoAmministrativoDirigenziale.altriIncarichi
    '' ''            Me.AltreAttivitaIncaricoAmministrativoTextBox.Text = incaricoAmministrativoDirigenziale.altreCariche
    '' ''            Me.altreAttivitaProfessionaliTextBox.Text = incaricoAmministrativoDirigenziale.altreAttivitàProfessionali

    '' ''            Me.NumeroAttoIncaricoAmministrativoTextBox.Value = incaricoAmministrativoDirigenziale.numerodet
    '' ''            Me.DataAttoIncaricoAmministrativoTextBox.SelectedDate = incaricoAmministrativoDirigenziale.datadet

    '' ''            If Not String.IsNullOrEmpty(incaricoAmministrativoDirigenziale.urlcv) Then
    '' ''                Me.CurriculumIncaricoAmministrativoLinkButton.Text = incaricoAmministrativoDirigenziale.urlcv
    '' ''                Me.NomeFileCurriculumIncaricoAmministrativoLabel.Text = incaricoAmministrativoDirigenziale.CurriculumTemp
    '' ''                Me.curriculumincaricoAmministrativoUpload1.Visible = False
    '' ''                Me.curriculumincaricoAmministrativoUpload2.Visible = True
    '' ''            Else
    '' ''                Me.CurriculumIncaricoAmministrativoLinkButton.Text = String.Empty
    '' ''                Me.NomeFileCurriculumIncaricoAmministrativoLabel.Text = String.Empty
    '' ''                Me.curriculumincaricoAmministrativoUpload1.Visible = True
    '' ''                Me.curriculumincaricoAmministrativoUpload2.Visible = False
    '' ''            End If


    '' ''            If Not String.IsNullOrEmpty(incaricoAmministrativoDirigenziale.urlAttestazioneIncompatilita) Then
    '' ''                Me.IncompatibilitaIncaricoAmministrativoLinkButton.Text = incaricoAmministrativoDirigenziale.urlAttestazioneIncompatilita
    '' ''                Me.NomeFileIncompatibilitaIncaricoAmministrativoLabel.Text = incaricoAmministrativoDirigenziale.IncompatilitaTemp
    '' ''                Me.incompatibilitaIncaricoAmministrativoUpload1.Visible = False
    '' ''                Me.incompatibilitaIncaricoAmministrativoUpload2.Visible = True
    '' ''            Else
    '' ''                Me.IncompatibilitaIncaricoAmministrativoLinkButton.Text = String.Empty
    '' ''                Me.NomeFileIncompatibilitaIncaricoAmministrativoLabel.Text = String.Empty
    '' ''                Me.incompatibilitaIncaricoAmministrativoUpload1.Visible = True
    '' ''                Me.incompatibilitaIncaricoAmministrativoUpload2.Visible = False
    '' ''            End If


    '' ''            If Not String.IsNullOrEmpty(incaricoAmministrativoDirigenziale.urlAttestazioneInconferibilita) Then
    '' ''                Me.InconferibilitaIncaricoAmministrativoLinkButton.Text = incaricoAmministrativoDirigenziale.urlAttestazioneInconferibilita
    '' ''                Me.NomeFileInconferibilitaIncaricoAmministrativoLabel.Text = incaricoAmministrativoDirigenziale.InconferibilitaTemp
    '' ''                Me.inconferibilitaIncaricoAmministrativoUpload1.Visible = False
    '' ''                Me.InconferibilitaIncaricoAmministrativoUpload2.Visible = True
    '' ''            Else
    '' ''                Me.InconferibilitaIncaricoAmministrativoLinkButton.Text = String.Empty
    '' ''                Me.NomeFileInconferibilitaIncaricoAmministrativoLabel.Text = String.Empty
    '' ''                Me.inconferibilitaIncaricoAmministrativoUpload1.Visible = True
    '' ''                Me.InconferibilitaIncaricoAmministrativoUpload2.Visible = False
    '' ''            End If


    '' ''            If selezione Then
    '' ''                Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(incaricoAmministrativoDirigenziale.idPubblicazione)
    '' ''                pubblicazioni.Dispose()
    '' ''            End If


    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso

    '' ''            Dim bando As ParsecAdmin.BandoConcorso = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''            Me.BandoConcorso = bando

    '' ''            Me.BandoConcorsoOggettoTextBox.Text = bando.oggetto
    '' ''            Me.BandoConcorsoProfiloTextBox.Text = bando.profilo
    '' ''            Me.BandoConcorsoTipoAssunzioneTextBox.Text = bando.tipoAssunzione
    '' ''            Me.BandoConcorsoSpesaTextBox.Value = bando.spesa
    '' ''            Me.BandoConcorsoCategoriaTextBox.Text = bando.categoria
    '' ''            Me.BandoConcorsoNumeroDipendentiAssuntiTextBox.Value = bando.numeroDipendentiAssunti
    '' ''            Me.BandoConcorsoEstremiDocumentiTextBox.Text = bando.estremiPrincipaliDocumenti



    '' ''            If selezione Then
    '' ''                Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(bando.idPubblicazione)
    '' ''                pubblicazioni.Dispose()
    '' ''            End If

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate

    '' ''            Dim ente As ParsecAdmin.EnteControllato = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo
    '' ''            Me.EnteControllato = ente

    '' ''            Me.RagioneSocialeEnteControllatoCombobox.SelectedValue = String.Empty
    '' ''            Me.RagioneSocialeEnteControllatoCombobox.Text = ente.ragioneSociale

    '' ''            If ente.idEnteRubrica.HasValue Then
    '' ''                Me.RagioneSocialeEnteControllatoCombobox.SelectedValue = ente.idEnteRubrica
    '' ''                Dim elemento = Me.GetElementoRubrica(ente.idEnteRubrica)
    '' ''                If Not elemento Is Nothing Then
    '' ''                    RagioneSocialeEnteControllatoCombobox.Text = elemento.Denominazione & " " & If(Not String.IsNullOrEmpty(elemento.Nome), elemento.Nome & ", ", "") & If(Not String.IsNullOrEmpty(elemento.Indirizzo), elemento.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(elemento.Comune), elemento.Comune & " ", "") & If(Not String.IsNullOrEmpty(elemento.CAP), elemento.CAP & " ", "") & If(Not String.IsNullOrEmpty(elemento.Provincia), "(" & elemento.Provincia & ")", "")
    '' ''                End If
    '' ''            End If

    '' ''            Me.AttivitaFavoreAmministrazioneEnteControllatoTextBox.Text = ente.attivitaFavoreAmministrazione
    '' ''            Me.AttivitaServizioPubblicoEnteControllatoTextBox.Text = ente.attivitaServizioPubblico
    '' ''            Me.DurataImpegnoEnteControllatoTextbox.Text = ente.durataImpegno
    '' ''            Me.FunzioniAttribuiteEnteControllatoTextBox.Text = ente.funzioniAttribuite

    '' ''            Me.MisuraPartecipazioneEnteControllatoTextBox.Text = ente.misuraPartecipazione
    '' ''            Me.UrlSitoIstituzionaleEnteControllatoTextBox.Text = ente.urlSitoIstituzionale
    '' ''            Me.OnereComplessivoEnteControllatoTextBox.Value = ente.onereComplessivo


    '' ''            Me.NumeroRappresentantiEnteControllatoTextBox.Value = ente.numeroRappresentanti

    '' ''            If Not String.IsNullOrEmpty(ente.urlDichiarazioneIncompatibilita) Then
    '' ''                Me.IncompatibilitaAllegatoLinkButton.Text = ente.urlDichiarazioneIncompatibilita
    '' ''                Me.NomeFileIncompatibilitaLabel.Text = ente.IncompatibilitaTemp
    '' ''                Me.incompatibilitaUpload1.Visible = False
    '' ''                Me.incompatibilitaUpload2.Visible = True
    '' ''            Else
    '' ''                Me.IncompatibilitaAllegatoLinkButton.Text = String.Empty
    '' ''                Me.NomeFileIncompatibilitaLabel.Text = String.Empty
    '' ''                Me.incompatibilitaUpload1.Visible = True
    '' ''                Me.incompatibilitaUpload2.Visible = False
    '' ''            End If


    '' ''            If Not String.IsNullOrEmpty(ente.urlDichiarazioneInconferibilita) Then
    '' ''                Me.InconferibilitaAllegatoLinkButton.Text = ente.urlDichiarazioneInconferibilita
    '' ''                Me.NomeFileInconferibilitaLabel.Text = ente.InconferibilitaTemp
    '' ''                Me.inconferibilitaUpload1.Visible = False
    '' ''                Me.InconferibilitaUpload2.Visible = True
    '' ''            Else
    '' ''                Me.IncompatibilitaAllegatoLinkButton.Text = String.Empty
    '' ''                Me.NomeFileIncompatibilitaLabel.Text = String.Empty
    '' ''                Me.incompatibilitaUpload1.Visible = True
    '' ''                Me.incompatibilitaUpload2.Visible = False
    '' ''            End If



    '' ''            If Not String.IsNullOrEmpty(ente.trattamentoEconomicoRappresentanti) Then
    '' ''                Dim i As Integer = 0
    '' ''                Dim trattamenti As String() = ente.trattamentoEconomicoRappresentanti.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
    '' ''                For Each trattamento As String In trattamenti
    '' ''                    Me.TrattamentiEconomiciRappresentante.Add(New ParsecAdmin.TrattamentoEconomicoRappresentante With {.Id = i, .Descrizione = trattamento})
    '' ''                    i += 1
    '' ''                Next
    '' ''            End If


    '' ''            If Not String.IsNullOrEmpty(ente.incarichiAmministratoreTrattamentoEconomico) Then
    '' ''                Dim i As Integer = 0
    '' ''                Dim trattamentiIncarichi As String() = ente.incarichiAmministratoreTrattamentoEconomico.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
    '' ''                For Each trattamentoIncarico As String In trattamentiIncarichi
    '' ''                    Dim s = trattamentoIncarico.Split(New Char() {"#"}, StringSplitOptions.RemoveEmptyEntries)
    '' ''                    If s.Length = 2 Then
    '' ''                        Me.TrattamentiEconomiciIncaricoAmministratore.Add(New ParsecAdmin.TrattamentoEconomicoIncaricoAmministratore With {.Id = i, .Incarico = s(0), .Descrizione = s(1)})
    '' ''                        i += 1
    '' ''                    End If
    '' ''                Next
    '' ''            End If

    '' ''            Me.BilanciEnteControllato = ente.Bilanci

    '' ''            Me.EntitaSocietaPartecipateTextBox.Value = ente.entita



    '' ''            If selezione Then
    '' ''                Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(ente.idPubblicazione)
    '' ''                pubblicazioni.Dispose()
    '' ''            End If


    '' ''        Case Else    'PUBBLICAZIONE GENERICA

    '' ''            If Not trasparenza.ProcedureAffidamento(0) Is Nothing Then
    '' ''                Dim pubblicazioneGenerica As ParsecAdmin.PubblicazioneGenerica = trasparenza.ProcedureAffidamento(0) 'Prendo solo il primo

    '' ''                Me.PubblicazioneContenutoTextBox.Text = pubblicazioneGenerica.contenuto
    '' ''                Me.PubblicazioneTitoloTextBox.Text = pubblicazioneGenerica.titolo
    '' ''                Me.PubblicazioneNumeroTextBox.Value = pubblicazioneGenerica.numero
    '' ''                Me.PubblicazioneInizioRiferimentoTextBox.SelectedDate = pubblicazioneGenerica.dataInizioRiferimento
    '' ''                Me.PubblicazioneFineRiferimentoTextBox.SelectedDate = pubblicazioneGenerica.dataFineriferimento

    '' ''                If selezione Then
    '' ''                    Dim pubblicazioni As New ParsecAdmin.PubblicazioneRepository
    '' ''                    Me.AllegatiPubblicazione = pubblicazioni.GetAllegati(pubblicazioneGenerica.idPubblicazione)
    '' ''                    pubblicazioni.Dispose()
    '' ''                End If

    '' ''                Me.DatiPubblicazioneGenericaLabel.Text = "Dati " & trasparenza.SezioneTrasparente

    '' ''            End If


    '' ''    End Select


    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub NascondiPannelliTrasparenza()
    '' ''    Me.PubblicazionePanel.Visible = False
    '' ''    Me.AttiConcessionePanel.Visible = False
    '' ''    Me.BandiGareContrattiPanel.Visible = False
    '' ''    Me.CollaborazioneConsulenzaPanel.Visible = False
    '' ''    Me.IncaricoPanel.Visible = False
    '' ''    Me.IncarichiAmministrativiDirigenzialiPanel.Visible = False
    '' ''    Me.BandoConcorsoPanel.Visible = False
    '' ''    Me.EnteControllatoPanel.Visible = False


    '' ''    Me.AttiTabStrip.FindTabByText("Trasparenza").Enabled = False


    '' ''    'Me.comboboxSottoSezione.Enabled = False
    '' ''    Me.SezioneTrasparenzaTextBox.Enabled = False

    '' ''    'PUBBLICAZIONE GENERICA
    '' ''    Me.PubblicazioneGenericaPanel.Visible = False


    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub VisualizzaPannelliTrasparenza(tipologia As ParsecAdmin.TipologiaSezioneTrasparente)
    '' ''    Me.NascondiPannelliTrasparenza()
    '' ''    Me.PubblicazionePanel.Visible = True
    '' ''    Me.AttiTabStrip.FindTabByText("Trasparenza").Enabled = True

    '' ''    'Me.comboboxSottoSezione.Enabled = True
    '' ''    SezioneTrasparenzaTextBox.Enabled = True
    '' ''    Select Case tipologia
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.AttiConcessione
    '' ''            Me.AttiConcessionePanel.Visible = True
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti
    '' ''            Me.BandiGareContrattiPanel.Visible = True
    '' ''            If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
    '' ''                Me.OggettoBandoGaraTextBox.Text = Me.OggettoTextBox.Text
    '' ''            End If

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.ConsulentiCollaboratori
    '' ''            Me.CollaborazioneConsulenzaPanel.Visible = True
    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncarichiConferitiDipendenti
    '' ''            Me.IncaricoPanel.Visible = True

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.BandoConcorso
    '' ''            Me.BandoConcorsoPanel.Visible = True

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo, ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale
    '' ''            Me.IncarichiAmministrativiDirigenzialiPanel.Visible = True
    '' ''            Select Case tipologia
    '' ''                Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoAmministrativo
    '' ''                    Me.DatiIncarichiAmministrativiDirigenzialiLabel.Text = "Dati Incarico Amministrativo"
    '' ''                Case ParsecAdmin.TipologiaSezioneTrasparente.IncaricoDirigenziale
    '' ''                    Me.DatiIncarichiAmministrativiDirigenzialiLabel.Text = "Dati Incarico Dirigenziale"
    '' ''            End Select

    '' ''        Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati, ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati, ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate
    '' ''            Me.EnteControllatoPanel.Visible = True

    '' ''            Me.DichiarazioniEnteControllatoPageView.Visible = False
    '' ''            Me.PannelloSocietaPartecipate.Visible = False
    '' ''            Me.EnteControllatoTabStrip.Tabs(1).Visible = False

    '' ''            Select Case tipologia
    '' ''                Case ParsecAdmin.TipologiaSezioneTrasparente.EntiDirittoPrivatoControllati
    '' ''                    Me.DatiEnteControllatoLabel.Text = "Dati Ente Privato Controllato"
    '' ''                    Me.DichiarazioniEnteControllatoPageView.Visible = True
    '' ''                    Me.EnteControllatoTabStrip.Tabs(1).Visible = True
    '' ''                Case ParsecAdmin.TipologiaSezioneTrasparente.EntiPubbliciVigilati
    '' ''                    Me.DatiEnteControllatoLabel.Text = "Dati Ente Pubblico Vigilato"
    '' ''                    Me.DichiarazioniEnteControllatoPageView.Visible = True
    '' ''                    Me.EnteControllatoTabStrip.Tabs(1).Visible = True
    '' ''                Case ParsecAdmin.TipologiaSezioneTrasparente.SocietaPartecipate
    '' ''                    Me.DatiEnteControllatoLabel.Text = "Dati Società Partecipata"
    '' ''                    Me.PannelloSocietaPartecipate.Visible = True
    '' ''            End Select


    '' ''        Case Else    'PUBBLICAZIONE GENERICA
    '' ''            Me.PubblicazioneGenericaPanel.Visible = True
    '' ''            Me.DatiPubblicazioneGenericaLabel.Text = "Dati " & Me.SezioneTrasparenzaTextBox.Text
    '' ''    End Select



    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub TrovaSezioneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaSezioneImageButton.Click
    '' ''    Dim sezioni As New ParsecAdmin.SezioneTrasparenzaRepository
    '' ''    Dim sezioniSelezionate = sezioni.GetSezioniModulo(Me.SezioneTrasparenzaTextBox.Text.ToLower.Trim, ParsecAdmin.TipoModulo.ATT)
    '' ''    If sezioniSelezionate.Count = 1 Then
    '' ''        Dim sezione = sezioniSelezionate.FirstOrDefault
    '' ''        If sezione.Descrizione <> Me.SezioneTrasparenzaTextBox.Text Then
    '' ''            ResettaVistaPannelliTrasparenza()
    '' ''        End If
    '' ''        Me.SezioneTrasparenzaTextBox.Text = sezione.Descrizione
    '' ''        Me.IdSezioneTrasparenzaTextBox.Text = sezione.Id
    '' ''        Dim tipologia As ParsecAdmin.TipologiaSezioneTrasparente = CType(sezione.Id, ParsecAdmin.TipologiaSezioneTrasparente)
    '' ''        Me.VisualizzaPannelliTrasparenza(tipologia)
    '' ''        Me.FiltroBandoGaraTextBox.Focus()


    '' ''        Me.aggiornaSottoSezione(sezione.Id)


    '' ''        If tipologia = ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti Then
    '' ''            Dim clienti As New ParsecAdmin.ClientRepository
    '' ''            Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''            If Not cliente Is Nothing Then
    '' ''                Me.DenominazioneStrutturaProponenteTextBox.Text = cliente.Descrizione
    '' ''                If Not String.IsNullOrEmpty(cliente.CodiceFiscale) Then
    '' ''                    Me.CodiceFiscaleProponenteTextBox.Text = cliente.CodiceFiscale
    '' ''                Else
    '' ''                    If Not String.IsNullOrEmpty(cliente.PIVA) Then
    '' ''                        Me.CodiceFiscaleProponenteTextBox.Text = cliente.PIVA
    '' ''                    End If
    '' ''                End If
    '' ''            End If
    '' ''            clienti.Dispose()
    '' ''        End If

    '' ''    Else

    '' ''        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '' ''        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaSezioneTrasparenzaPage.aspx"
    '' ''        Dim queryString As New Hashtable
    '' ''        queryString.Add("obj", Me.AggiornaSezioneImageButton.ClientID)

    '' ''        Dim parametriPagina As New Hashtable
    '' ''        parametriPagina.Add("idModulo", ParsecAdmin.TipoModulo.ATT)
    '' ''        parametriPagina.Add("idUtente", utenteCollegato.Id)
    '' ''        parametriPagina.Add("tipoSelezione", 0) 'SINGOLA
    '' ''        parametriPagina.Add("livelliSelezionabili", "100,200")
    '' ''        parametriPagina.Add("ultimoLivelloStruttura", "200")
    '' ''        parametriPagina.Add("selezionaSezioniAbrogate", False)
    '' ''        ' parametriPagina.Add("Filtro", Me.FiltroDenominazioneTextBox.Text)

    '' ''        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    '' ''        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    '' ''    End If


    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiornaSezioneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaSezioneImageButton.Click
    '' ''    If Not ParsecUtility.SessionManager.SezioniSelezionate Is Nothing Then
    '' ''        Dim sezione As ParsecAdmin.SezioneTrasparenza = CType(ParsecUtility.SessionManager.SezioniSelezionate, List(Of ParsecAdmin.SezioneTrasparenza)).FirstOrDefault
    '' ''        If sezione.Descrizione <> Me.SezioneTrasparenzaTextBox.Text Then
    '' ''            ResettaVistaPannelliTrasparenza()
    '' ''        End If
    '' ''        If Not sezione Is Nothing Then
    '' ''            Me.SezioneTrasparenzaTextBox.Text = sezione.Descrizione
    '' ''            Me.IdSezioneTrasparenzaTextBox.Text = sezione.Id
    '' ''            Dim tipologia As ParsecAdmin.TipologiaSezioneTrasparente = CType(sezione.Id, ParsecAdmin.TipologiaSezioneTrasparente)
    '' ''            Me.VisualizzaPannelliTrasparenza(tipologia)

    '' ''            Me.aggiornaSottoSezione(sezione.Id)

    '' ''            If tipologia = ParsecAdmin.TipologiaSezioneTrasparente.BandiGaraContratti Then

    '' ''                Dim clienti As New ParsecAdmin.ClientRepository
    '' ''                Dim cliente As ParsecAdmin.Cliente = clienti.GetQuery.FirstOrDefault
    '' ''                If Not cliente Is Nothing Then
    '' ''                    Me.DenominazioneStrutturaProponenteTextBox.Text = cliente.Descrizione
    '' ''                    If Not String.IsNullOrEmpty(cliente.CodiceFiscale) Then
    '' ''                        Me.CodiceFiscaleProponenteTextBox.Text = cliente.CodiceFiscale
    '' ''                    Else
    '' ''                        If Not String.IsNullOrEmpty(cliente.PIVA) Then
    '' ''                            Me.CodiceFiscaleProponenteTextBox.Text = cliente.PIVA
    '' ''                        End If
    '' ''                    End If
    '' ''                End If
    '' ''                clienti.Dispose()
    '' ''            End If

    '' ''        End If
    '' ''        ParsecUtility.SessionManager.SezioniSelezionate = Nothing




    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub EliminaSezioneImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EliminaSezioneImageButton.Click
    '' ''    Me.SezioneTrasparenzaTextBox.Text = String.Empty
    '' ''    Me.IdSezioneTrasparenzaTextBox.Text = String.Empty
    '' ''    Me.NascondiPannelliTrasparenza()
    '' ''    Me.IdGaraContratti = String.Empty
    '' ''    ' ResettaVistaPannelliTrasparenza()
    '' ''    Me.AllegatiPubblicazione = New List(Of ParsecAdmin.AllegatoPubblicazione)
    '' ''    Me.SezioneTrasparenzaTextBox.Enabled = True
    '' ''End Sub

#End Region

#Region "BENEFICIARIO"
    'luca 01/07/2020
    '' ''Protected Sub AggiungiBeneficiarioImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiBeneficiarioImageButton.Click

    '' ''    If Not String.IsNullOrEmpty(Me.RubricaComboBox.SelectedValue) Then
    '' ''        Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''        Dim strutturaEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.RubricaComboBox.SelectedValue)).FirstOrDefault
    '' ''        If Not strutturaEsterna Is Nothing Then
    '' ''            Dim exist As Boolean = Not Me.AttiConcessione.Where(Function(c) c.IdBeneficiario.HasValue And c.IdBeneficiario = strutturaEsterna.Id).FirstOrDefault Is Nothing
    '' ''            If exist Then
    '' ''                Me.RubricaComboBox.Text = String.Empty
    '' ''                Me.RubricaComboBox.SelectedValue = String.Empty
    '' ''                ParsecUtility.Utility.MessageBox("Il beneficiario selezionato è già presente!", False)
    '' ''                Exit Sub
    '' ''            End If
    '' ''        End If
    '' ''    End If


    '' ''    Dim attoConcessione As New ParsecAdmin.AttoConcessione
    '' ''    'If Me.AttiConcessione.Count > 0 Then
    '' ''    '    attoConcessione = Me.CopiaAttoConcessione(Me.AttiConcessione.LastOrDefault, False)
    '' ''    'End If

    '' ''    If Not String.IsNullOrEmpty(Me.RubricaComboBox.SelectedValue) Then
    '' ''        Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''        Dim strutturaEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.RubricaComboBox.SelectedValue)).FirstOrDefault
    '' ''        If Not strutturaEsterna Is Nothing Then
    '' ''            attoConcessione.IdBeneficiario = strutturaEsterna.Id
    '' ''            If Not strutturaEsterna.Denominazione Is Nothing Then
    '' ''                attoConcessione.Beneficiario = strutturaEsterna.Denominazione
    '' ''            End If
    '' ''            If Not strutturaEsterna.CodiceFiscale Is Nothing Then
    '' ''                attoConcessione.DatoFiscaleBeneficiario = strutturaEsterna.CodiceFiscale
    '' ''            Else
    '' ''                attoConcessione.DatoFiscaleBeneficiario = strutturaEsterna.PartitaIVA
    '' ''            End If
    '' ''        End If
    '' ''        Me.RubricaComboBox.Text = String.Empty
    '' ''        Me.RubricaComboBox.SelectedValue = String.Empty
    '' ''    End If

    '' ''    Me.VisualizzaAttoConcessione(attoConcessione, Me.InserisciBeneficiarioImageButton.ClientID, "Nuovo")


    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub InserisciBeneficiarioImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles InserisciBeneficiarioImageButton.Click
    '' ''    If Not ParsecUtility.SessionManager.AttoConcessione Is Nothing Then
    '' ''        Dim attoConcessione As ParsecAdmin.AttoConcessione = ParsecUtility.SessionManager.AttoConcessione

    '' ''        If attoConcessione.IdBeneficiario.HasValue Then
    '' ''            Dim atti = Me.AttiConcessione.Where(Function(c) c.IdBeneficiario.HasValue).ToList

    '' ''            Dim exist As Boolean = Not atti.Where(Function(c) c.IdBeneficiario = attoConcessione.IdBeneficiario).FirstOrDefault Is Nothing
    '' ''            If exist Then
    '' ''                ParsecUtility.Utility.MessageBox("Il beneficiario selezionato è già presente!", False)
    '' ''                Exit Sub
    '' ''            End If

    '' ''        End If

    '' ''        '*****************************************************************************
    '' ''        'ASSOCIO AGLI ELEMENTI TEMPORANEI UN ID NEGATIVO
    '' ''        '*****************************************************************************
    '' ''        attoConcessione.Id = -1
    '' ''        If Me.AttiConcessione.Count > 0 Then
    '' ''            Dim min = Me.AttiConcessione.Min(Function(c) c.Id)
    '' ''            If min < 0 Then
    '' ''                attoConcessione.Id = min - 1
    '' ''            End If
    '' ''        End If
    '' ''        '*****************************************************************************

    '' ''        Me.AttiConcessione.Add(attoConcessione)
    '' ''        ParsecUtility.SessionManager.AttoConcessione = Nothing

    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub ModificaBeneficiarioImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ModificaBeneficiarioImageButton.Click

    '' ''    If Not ParsecUtility.SessionManager.AttoConcessione Is Nothing Then
    '' ''        Dim attoConcessioneSelezionato As ParsecAdmin.AttoConcessione = ParsecUtility.SessionManager.AttoConcessione

    '' ''        If attoConcessioneSelezionato.IdBeneficiario.HasValue Then
    '' ''            Dim atti = Me.AttiConcessione.Where(Function(c) c.IdBeneficiario.HasValue).ToList
    '' ''            Dim exist As Boolean = Not atti.Where(Function(c) c.IdBeneficiario = attoConcessioneSelezionato.IdBeneficiario And c.Id <> attoConcessioneSelezionato.Id).FirstOrDefault Is Nothing
    '' ''            If exist Then
    '' ''                ParsecUtility.Utility.MessageBox("Il beneficiario selezionato è già presente!", False)
    '' ''                Exit Sub
    '' ''            End If
    '' ''        End If


    '' ''        Dim attoConcessione As ParsecAdmin.AttoConcessione = Me.AttiConcessione.Where(Function(a) a.Id = attoConcessioneSelezionato.Id).FirstOrDefault
    '' ''        Dim index = Me.AttiConcessione.IndexOf(attoConcessione)
    '' ''        Me.AttiConcessione.Remove(attoConcessione)

    '' ''        Me.AttiConcessione.Insert(index, attoConcessioneSelezionato)
    '' ''        ParsecUtility.SessionManager.AttoConcessione = Nothing

    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub BeneficiariGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles BeneficiariGridView.ItemCommand
    '' ''    If e.CommandName = "Delete" Then
    '' ''        Me.DeleteBeneficiario(e.Item)
    '' ''    End If
    '' ''    If e.CommandName = "Select" Then
    '' ''        Me.SelezionaBeneficiario(e.Item)
    '' ''    End If
    '' ''    If e.CommandName = "Copy" Then
    '' ''        Me.CopiaBeneficiario(e.Item)
    '' ''    End If
    '' ''    If e.CommandName = "Preview" Then
    '' ''        Me.PreviewBeneficiario(e.Item)
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub BeneficiariGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles BeneficiariGridView.ItemCreated
    '' ''    If TypeOf e.Item Is GridHeaderItem Then
    '' ''        e.Item.Style.Add("position", "relative")
    '' ''        e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop -1)")
    '' ''        e.Item.Style.Add("z-index", "99")
    '' ''        e.Item.Style.Add("background-color", "White")
    '' ''    End If
    '' ''End Sub

#End Region

#Region "CONSULENZA"

    'luca 01/07/2020
    '' ''Protected Sub TrovaBeneficiarioConsulenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaBeneficiarioConsulenzaImageButton.Click

    '' ''      Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
    '' ''      Dim queryString As New Hashtable
    '' ''      queryString.Add("obj", Me.AggiornaBeneficiarioConsulenzaImageButton.ClientID)
    '' ''      queryString.Add("mode", "search")

    '' ''      Dim parametriPagina As New Hashtable

    '' ''      ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    '' ''      If String.IsNullOrEmpty(Me.BeneficiarioConsulenzaComboBox.SelectedValue) Then
    '' ''          parametriPagina.Add("Filtro", Me.BeneficiarioConsulenzaComboBox.Text)

    '' ''          If Not String.IsNullOrEmpty(Me.BeneficiarioConsulenzaComboBox.Text) Then
    '' ''              Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''              Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.BeneficiarioConsulenzaComboBox.Text) And c.LogStato Is Nothing).ToList
    '' ''              If struttureEsterne.Count = 1 Then
    '' ''                  Me.BeneficiarioConsulenzaComboBox.SelectedValue = struttureEsterne(0).Id
    '' ''                  Me.BeneficiarioConsulenzaComboBox.Text = struttureEsterne(0).Denominazione & " " & If(Not String.IsNullOrEmpty(struttureEsterne(0).Nome), struttureEsterne(0).Nome & ", ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Indirizzo), struttureEsterne(0).Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Comune), struttureEsterne(0).Comune & " ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).CAP), struttureEsterne(0).CAP & " ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Provincia), "(" & struttureEsterne(0).Provincia & ")", "")
    '' ''                  ParsecUtility.SessionManager.ParametriPagina = Nothing
    '' ''              Else
    '' ''                  ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''              End If
    '' ''              rubrica.Dispose()
    '' ''          Else
    '' ''              ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''          End If
    '' ''      Else
    '' ''          Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''          Dim struttureEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.BeneficiarioConsulenzaComboBox.SelectedValue)).FirstOrDefault
    '' ''          rubrica.Dispose()
    '' ''          If Not struttureEsterna Is Nothing Then
    '' ''              parametriPagina.Add("Filtro", struttureEsterna.Denominazione)
    '' ''          End If
    '' ''          ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''      End If
    '' ''  End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiornaBeneficiarioConsulenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBeneficiarioConsulenzaImageButton.Click
    '' ''    If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
    '' ''        Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
    '' ''        BeneficiarioConsulenzaComboBox.SelectedValue = strutturaEsterna.Id
    '' ''        Me.BeneficiarioConsulenzaComboBox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "")
    '' ''        ParsecUtility.SessionManager.Rubrica = Nothing
    '' ''    End If
    '' ''End Sub

#End Region

#Region "BANDI DI GARA"
    'luca 01/07/2020
    '' ''Protected Sub TrovaRaggruppamentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaRaggruppamentoImageButton.Click
    '' ''    Dim moduli As New ParsecAdmin.ModuleRepository
    '' ''    Dim modulo = moduli.Where(Function(c) c.Id = CInt(ParsecAdmin.TipoModulo.CNT) And c.Abilitato = True).FirstOrDefault
    '' ''    moduli.Dispose()
    '' ''    If Not modulo Is Nothing Then
    '' ''        Dim pageUrl As String = "~/UI/Contratti/pages/search/RicercaRaggruppamentoImpresaPage.aspx"
    '' ''        Dim queryString As New Hashtable
    '' ''        queryString.Add("obj", Me.AggiornaRaggruppamentoImageButton.ClientID)
    '' ''        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)

    '' ''    Else
    '' ''        ParsecUtility.Utility.MessageBox("Attenzione, il modulo di gestione gare non è attivo. Inserire i dati manualmente.", False)
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiornaRaggruppamentoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaRaggruppamentoImageButton.Click


    '' ''    If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
    '' ''        Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
    '' ''        If parametriPagina.ContainsKey("SelectedRaggruppamenti") Then

    '' ''            Dim raggruppamentiSelezionati As SortedList(Of Integer, String) = parametriPagina("SelectedRaggruppamenti")
    '' ''            For Each raggruppamentoSelezionato In raggruppamentiSelezionati
    '' ''                Dim raggruppamentoId = raggruppamentoSelezionato.Key

    '' ''                Dim partecipanti As New ParsecContratti.RaggruppamentoElementoRepository
    '' ''                Dim items = partecipanti.GetView(New ParsecContratti.RaggruppamentoStrutturaEsternaInfoFiltro With {.idRaggruppamento = raggruppamentoId}).Select(Function(c) c.DenominazioneStrutturaEsternaInfo)
    '' ''                Dim v = raggruppamentoSelezionato.Value & " ( " & String.Join(" - ", items) & " )"
    '' ''                Dim item As New Telerik.Web.UI.RadListBoxItem(v, v)
    '' ''                Me.PartecipantiListBox.Items.Add(item)

    '' ''            Next
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub TrovaBandoGaraImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaBandoGaraImageButton.Click
    '' ''    'Controllo se il modulo n. 14 "Contratti" è attivo
    '' ''    Dim moduli As New ParsecAdmin.ModuleRepository
    '' ''    Dim modulo = moduli.GetQuery.Where(Function(m) m.Id = CInt(ParsecAdmin.TipoModulo.CNT) And m.Abilitato = True).FirstOrDefault
    '' ''    moduli.Dispose()

    '' ''    If Not modulo Is Nothing Then
    '' ''        Dim bandiGara As New ParsecContratti.GaraRepository

    '' ''        Dim bandi = bandiGara.GetQuery.ToList
    '' ''        If Not String.IsNullOrEmpty(FiltroBandoGaraTextBox.Text.Trim) Then
    '' ''            bandi = bandi.Where(Function(c) c.oggetto.ToLower.Contains(Me.FiltroBandoGaraTextBox.Text.ToLower.Trim) OrElse c.cig.Contains(Me.FiltroBandoGaraTextBox.Text.ToLower.Trim)).ToList()
    '' ''        End If

    '' ''        Select Case bandi.Count
    '' ''            Case 1 'è presente un solo bando di gare
    '' ''                Dim bandoSelezionato = bandi.FirstOrDefault
    '' ''                bandoSelezionato.listaPartecipantiToString = bandiGara.GetPartecipanti(bandoSelezionato.idGara)
    '' ''                bandoSelezionato.listaAggiudicatariToString = bandiGara.GetAggiudicatari(bandoSelezionato.idGara)


    '' ''                Me.IdGaraContratti = bandoSelezionato.idGara.ToString

    '' ''                Me.OggettoBandoGaraTextBox.Text = bandoSelezionato.oggetto

    '' ''                Dim partecipante As List(Of String) = bandoSelezionato.listaPartecipantiToString
    '' ''                ' Use For Each loop over words and display them
    '' ''                Dim word As String
    '' ''                Dim item As Telerik.Web.UI.RadListBoxItem = Nothing
    '' ''                For Each word In partecipante
    '' ''                    'Verifico se il partecipante esiste già
    '' ''                    If Not CBool(PartecipantiListBox.Items.Where(Function(p) p.Text = word).Count) Then
    '' ''                        item = New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                        Me.PartecipantiListBox.Items.Add(item)
    '' ''                    Else
    '' ''                        Me.PartecipantiListBox.Items.Clear()
    '' ''                        item = New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                        Me.PartecipantiListBox.Items.Add(item)
    '' ''                    End If
    '' ''                Next

    '' ''                'Me.AggiudicatariTextBox.Text = String.Join(", ", bandoSelezionato.listaAggiudicatariToString)
    '' ''                Dim aggiudicatario As List(Of String) = bandoSelezionato.listaAggiudicatariToString
    '' ''                ' Use For Each loop over words and display them
    '' ''                word = ""
    '' ''                For Each word In aggiudicatario
    '' ''                    'Verifico prima se l'aggiudicatario esiste già
    '' ''                    If Not CBool(AggiudicatariListBox.Items.Where(Function(a) a.Text = word).Count) Then
    '' ''                        item = New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                        Me.AggiudicatariListBox.Items.Add(item)
    '' ''                    Else
    '' ''                        Me.AggiudicatariListBox.Items.Clear()
    '' ''                        item = New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                        Me.AggiudicatariListBox.Items.Add(item)
    '' ''                    End If
    '' ''                Next

    '' ''                Me.CigBandoGaraTextBox.Text = bandoSelezionato.cig
    '' ''                Me.ImportoAggiudicazioneTextBox.Value = bandoSelezionato.importoAggiudicazione

    '' ''                'Me.ImportoLiquidatoTextBox.Value = bandoSelezionato.importoSommaLiquidata

    '' ''                If CBool(bandoSelezionato.listaPartecipantiToString.Count) Then
    '' ''                    Me.NumeroOfferentiTextBox.Value = bandoSelezionato.listaPartecipantiToString.Count
    '' ''                Else
    '' ''                    Me.NumeroOfferentiTextBox.Text = ""
    '' ''                End If
    '' ''                Me.DataInizioLavoroTextBox.SelectedDate = bandoSelezionato.dataInizioLavori
    '' ''                Me.DataFineLavoroTextBox.SelectedDate = bandoSelezionato.dataFineLavori
    '' ''                Me.TipologiaSceltaComboBox.SelectedValue = bandoSelezionato.fkCriterioScelta
    '' ''                Me.FiltroBandoGaraTextBox.Text = String.Empty

    '' ''                Me.FiltroBandoGaraTextBox.Focus()

    '' ''                Me.DenominazioneStrutturaProponenteTextBox.Text = bandoSelezionato.denominazioneProponente
    '' ''                Me.CodiceFiscaleProponenteTextBox.Text = bandoSelezionato.codiceFiscaleProponente



    '' ''            Case Is > 1 'sono presenti almeno 2 bandi di gara


    '' ''                Dim pageUrl As String = "~/UI/Contratti/pages/search/RicercaGaraAppaltoPage.aspx"
    '' ''                Dim queryString As New Hashtable
    '' ''                queryString.Add("obj", Me.AggiornaBandoGaraImageButton.ClientID)
    '' ''                'passo i parametri (lista degli id delle gare "agganciate")
    '' ''                Dim parametriPagina As New Hashtable

    '' ''                Dim pubblicazioni As New ParsecPub.PubblicazioneRepository
    '' ''                Dim pubblicazioniBandogara As New ParsecPub.BandoGaraRepository(pubblicazioni.Context)

    '' ''                Dim listaIdGare As List(Of Integer) = (From pubblicazione In pubblicazioni.Where(Function(w) w.Stato = Nothing And w.idSezione = ParsecPub.TipologiaSezioneTrasparente.BandiGaraContratti)
    '' ''                                                       Join gara In pubblicazioniBandogara.GetQuery
    '' ''                                                       On pubblicazione.Id Equals gara.idPubblicazione
    '' ''                                                       Where Not gara.idGaraContratti Is Nothing
    '' ''                                                       Select gara.idGaraContratti.Value).Distinct.ToList


    '' ''                parametriPagina.Add("ListaIdGarePubblicate", listaIdGare)
    '' ''                Dim listaIdGareFiltrate As List(Of Integer) = bandi.Select(Function(s) s.idGara).ToList
    '' ''                parametriPagina.Add("ListaIdGareFiltrate", listaIdGareFiltrate)
    '' ''                ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    '' ''                ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    '' ''                pubblicazioni.Dispose()


    '' ''            Case Is < 1 'non sono presenti bandi di gara
    '' ''                ParsecUtility.Utility.MessageBox("Non sono stati trovati bandi di gara.", False)
    '' ''        End Select
    '' ''        bandiGara.Dispose()
    '' ''    Else
    '' ''        ParsecUtility.Utility.MessageBox("Attenzione, il modulo di gestione gare non è attivo. Inserire i dati manualmente.", False)
    '' ''    End If

    '' ''End Sub


    'luca 01/07/2020
    '' ''Private Sub AggiornaImportoLiquidatoPubblicazione(ByVal idGaraContratti As Integer)

    '' ''    Try
    '' ''        Dim parametri As New ParsecAdmin.ParametriRepository
    '' ''        Dim parametro = parametri.GetByName("AutomatizzazionePubblicazioneGare", ParsecAdmin.TipoModulo.PUB)
    '' ''        parametri.Dispose()

    '' ''        If Not parametro Is Nothing AndAlso parametro.Valore = "1" Then

    '' ''            Dim pubblicazioni As New ParsecPub.PubblicazioneRepository
    '' ''            Dim pubblicazioniGara As New ParsecPub.BandoGaraRepository(pubblicazioni.Context)



    '' ''            Dim importoTotaleLiquidato = (From gara In pubblicazioniGara.Where(Function(g) g.idGaraContratti = idGaraContratti)
    '' ''                                          Join pubblicazione In pubblicazioni.Where(Function(p) p.Stato Is Nothing)
    '' ''                                          On gara.idPubblicazione Equals pubblicazione.Id
    '' ''                                          Select gara.importoLiquidato)

    '' ''            If importoTotaleLiquidato.Any Then
    '' ''                Dim importo As Decimal = importoTotaleLiquidato.Sum
    '' ''                If importo >= 0 Then

    '' ''                    Dim gare As New ParsecContratti.GaraRepository

    '' ''                    Dim gara As ParsecContratti.Gara = gare.Where(Function(o) o.idGara = idGaraContratti).FirstOrDefault

    '' ''                    If Not gara Is Nothing Then
    '' ''                        gara.importoSommaLiquidataPubblicazione = importo
    '' ''                        gare.SaveChanges()
    '' ''                    End If


    '' ''                    gare.Dispose()

    '' ''                End If
    '' ''            End If

    '' ''            pubblicazioni.Dispose()



    '' ''        End If
    '' ''    Catch ex As Exception
    '' ''        'NIENTE - NON BLOCCANTE
    '' ''    End Try

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiornaBandoGaraImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBandoGaraImageButton.Click
    '' ''    If Not ParsecUtility.SessionManager.GaraSelezionata Is Nothing Then
    '' ''        Dim bandoGara As ParsecContratti.Gara = ParsecUtility.SessionManager.GaraSelezionata

    '' ''        Me.IdGaraContratti = bandoGara.idGara.ToString

    '' ''        ParsecUtility.SessionManager.GaraSelezionata = Nothing
    '' ''        Me.OggettoBandoGaraTextBox.Text = bandoGara.oggetto

    '' ''        Dim partecipante As List(Of String) = bandoGara.listaPartecipantiToString
    '' ''        ' Use For Each loop over words and display them
    '' ''        Dim word As String
    '' ''        For Each word In partecipante
    '' ''            'Verifico se il partecipante esiste già
    '' ''            If Not CBool(PartecipantiListBox.Items.Where(Function(p) p.Text = word).Count) Then
    '' ''                Dim item As New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                Me.PartecipantiListBox.Items.Add(item)
    '' ''            Else
    '' ''                Me.PartecipantiListBox.Items.Clear()
    '' ''                Dim item As New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                Me.PartecipantiListBox.Items.Add(item)
    '' ''            End If
    '' ''        Next

    '' ''        Dim aggiudicatario As List(Of String) = bandoGara.listaAggiudicatariToString
    '' ''        ' Use For Each loop over words and display them
    '' ''        word = ""
    '' ''        For Each word In aggiudicatario
    '' ''            'Verifico prima se l'aggiudicatario esiste già
    '' ''            If Not CBool(AggiudicatariListBox.Items.Where(Function(a) a.Text = word).Count) Then
    '' ''                Dim item As New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                Me.AggiudicatariListBox.Items.Add(item)
    '' ''            Else
    '' ''                Me.AggiudicatariListBox.Items.Clear()
    '' ''                Dim item As New Telerik.Web.UI.RadListBoxItem(word.Replace(";", ","), word.Replace(";", ","))
    '' ''                Me.AggiudicatariListBox.Items.Add(item)
    '' ''            End If
    '' ''        Next

    '' ''        Me.CigBandoGaraTextBox.Text = bandoGara.cig
    '' ''        Me.ImportoAggiudicazioneTextBox.Value = bandoGara.importoAggiudicazione
    '' ''        'Me.ImportoLiquidatoTextBox.Value = bandoGara.importoSommaLiquidata
    '' ''        If CBool(bandoGara.listaPartecipantiToString.Count) Then
    '' ''            Me.NumeroOfferentiTextBox.Value = bandoGara.listaPartecipantiToString.Count
    '' ''        Else
    '' ''            Me.NumeroOfferentiTextBox.Text = ""
    '' ''        End If
    '' ''        Me.DataInizioLavoroTextBox.SelectedDate = bandoGara.dataInizioLavori
    '' ''        Me.DataFineLavoroTextBox.SelectedDate = bandoGara.dataFineLavori
    '' ''        Me.TipologiaSceltaComboBox.SelectedValue = bandoGara.fkCriterioScelta

    '' ''        Me.DenominazioneStrutturaProponenteTextBox.Text = bandoGara.denominazioneProponente
    '' ''        Me.CodiceFiscaleProponenteTextBox.Text = bandoGara.codiceFiscaleProponente

    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiPartecipanteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiPartecipanteImageButton.Click
    '' ''    Dim descrizione As String = String.Empty
    '' ''    If Me.PartecipanteComboBox.SelectedValue <> "" Then
    '' ''        Dim partecipante = GetElementoRubrica(Me.PartecipanteComboBox.SelectedValue)
    '' ''        If Not partecipante Is Nothing Then
    '' ''            descrizione = partecipante.Denominazione
    '' ''            If Not partecipante.PartitaIVA Is Nothing AndAlso Not String.IsNullOrEmpty(partecipante.PartitaIVA.Trim) Then
    '' ''                descrizione &= " P.IVA " & partecipante.PartitaIVA.Trim
    '' ''            Else
    '' ''                If Not partecipante.CodiceFiscale Is Nothing AndAlso Not String.IsNullOrEmpty(partecipante.CodiceFiscale.Trim) Then
    '' ''                    descrizione &= " C.F. " & partecipante.CodiceFiscale.Trim
    '' ''                End If
    '' ''            End If
    '' ''        End If
    '' ''    Else
    '' ''        descrizione = Me.PartecipanteComboBox.Text.Trim
    '' ''    End If

    '' ''    If Not String.IsNullOrEmpty(descrizione) Then
    '' ''        'Verifico se il partecipante esiste già
    '' ''        If PartecipantiListBox.Items.Where(Function(p) p.Text = descrizione).Count = 0 Then
    '' ''            Dim item As New Telerik.Web.UI.RadListBoxItem(descrizione.Replace(";", ","), descrizione.Replace(";", ","))
    '' ''            Me.PartecipantiListBox.Items.Add(item)
    '' ''            PartecipanteComboBox.Text = String.Empty
    '' ''            Me.NumeroOfferentiTextBox.Value = Me.PartecipantiListBox.Items.Count
    '' ''        Else
    '' ''            PartecipanteComboBox.Text = String.Empty
    '' ''            ParsecUtility.Utility.MessageBox("Partecipante già inserito", False)
    '' ''        End If
    '' ''    End If


    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub EliminaPartecipanteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaPartecipanteImageButton.Click
    '' ''    Me.DeleteSelectedItems(Me.PartecipantiListBox)
    '' ''    If Me.PartecipantiListBox.Items.Count = 0 Then
    '' ''        Me.NumeroOfferentiTextBox.Value = Nothing
    '' ''    Else
    '' ''        Me.NumeroOfferentiTextBox.Value = Me.PartecipantiListBox.Items.Count
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub EliminaAggiudicatarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaAggiudicatarioImageButton.Click
    '' ''    Me.DeleteSelectedItems(Me.AggiudicatariListBox)
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub DeleteSelectedItems(ByVal list As Telerik.Web.UI.RadListBox)
    '' ''    For Each item As Telerik.Web.UI.RadListBoxItem In list.CheckedItems
    '' ''        list.Items.Remove(item)
    '' ''    Next
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiTuttoImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiTuttoImageButton.Click
    '' ''    For Each item As Telerik.Web.UI.RadListBoxItem In Me.PartecipantiListBox.Items
    '' ''        AggiungiPartecipante(item)
    '' ''    Next
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiungiImageButton.Click
    '' ''    For Each item As Telerik.Web.UI.RadListBoxItem In Me.PartecipantiListBox.CheckedItems
    '' ''        AggiungiPartecipante(item)
    '' ''    Next
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiPartecipante(ByVal item As Telerik.Web.UI.RadListBoxItem)
    '' ''    'Verifico prima se il partecipante selezionato è già stato inserito nella lista degli aggiudicatari
    '' ''    If Not CBool(AggiudicatariListBox.Items.Where(Function(a) a.Text = item.Text).Count) Then
    '' ''        Dim newItem As New Telerik.Web.UI.RadListBoxItem(item.Text, item.Text)
    '' ''        Me.AggiudicatariListBox.Items.Add(newItem)
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub TrovaPartecipanteImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaPartecipanteImageButton.Click

    '' ''    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
    '' ''    Dim queryString As New Hashtable
    '' ''    queryString.Add("obj", Me.AggiornaPartecipanteGaraImageButton.ClientID)
    '' ''    queryString.Add("mode", "search")

    '' ''    Dim parametriPagina As New Hashtable

    '' ''    ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    '' ''    If String.IsNullOrEmpty(Me.PartecipanteComboBox.SelectedValue) Then
    '' ''        parametriPagina.Add("Filtro", Me.PartecipanteComboBox.Text)

    '' ''        If Not String.IsNullOrEmpty(Me.PartecipanteComboBox.Text) Then
    '' ''            Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.PartecipanteComboBox.Text) And c.LogStato Is Nothing).ToList
    '' ''            If struttureEsterne.Count = 1 Then
    '' ''                Me.AggiornaPartecipanteGara(struttureEsterne(0))
    '' ''                ParsecUtility.SessionManager.ParametriPagina = Nothing
    '' ''            Else
    '' ''                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''            End If
    '' ''            rubrica.Dispose()
    '' ''        Else
    '' ''            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''        End If
    '' ''    Else
    '' ''        Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''        Dim struttureEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.PartecipanteComboBox.SelectedValue)).FirstOrDefault
    '' ''        rubrica.Dispose()
    '' ''        If Not struttureEsterna Is Nothing Then
    '' ''            parametriPagina.Add("Filtro", struttureEsterna.Denominazione)
    '' ''        End If
    '' ''        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub AggiornaPartecipanteGara(ByVal strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo)
    '' ''    Me.PartecipanteComboBox.SelectedValue = strutturaEsterna.Id
    '' ''    Me.PartecipanteComboBox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "")
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiornaPartecipanteGaraImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaPartecipanteGaraImageButton.Click
    '' ''    If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
    '' ''        Dim r As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
    '' ''        AggiornaPartecipanteGara(r)
    '' ''        ParsecUtility.SessionManager.Rubrica = Nothing
    '' ''    End If
    '' ''End Sub

#End Region

#Region "ATTO DI CONCESSIONE"

    'luca 01/07/2020
    '' ''Private Sub DeleteBeneficiario(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    '' ''    Dim attoConcessione As ParsecAdmin.AttoConcessione = Me.AttiConcessione.Where(Function(b) b.Id = id).FirstOrDefault
    '' ''    If Not attoConcessione Is Nothing Then
    '' ''        Me.AttiConcessione.Remove(attoConcessione)
    '' ''    End If
    '' ''End Sub
    'luca 01/07/2020
    '' ''Private Sub SelezionaBeneficiario(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    '' ''    Dim attoConcessione As ParsecAdmin.AttoConcessione = Me.AttiConcessione.Where(Function(b) b.Id = id).FirstOrDefault
    '' ''    Dim clone As ParsecAdmin.AttoConcessione = Me.CopiaAttoConcessione(attoConcessione, True)
    '' ''    Me.VisualizzaAttoConcessione(clone, Me.ModificaBeneficiarioImageButton.ClientID, "Modifica")
    '' ''End Sub
    'luca 01/07/2020
    '' ''Private Sub PreviewBeneficiario(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    '' ''    Dim attoConcessione As ParsecAdmin.AttoConcessione = AttiConcessione.Where(Function(b) b.Id = id).FirstOrDefault
    '' ''    Me.VisualizzaAttoConcessione(attoConcessione, Nothing, "Preview")
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub VisualizzaAttoConcessione(ByVal attoConcessione As ParsecAdmin.AttoConcessione, ByVal buttonToClick As String, ByVal mode As String)
    '' ''    Dim pageUrl As String = "~/UI/AttiDecisionali/pages/user/AttoConcessionePage.aspx"
    '' ''    Dim queryString As New Hashtable
    '' ''    queryString.Add("Mode", mode)
    '' ''    If Not String.IsNullOrEmpty(buttonToClick) Then
    '' ''        queryString.Add("obj", buttonToClick)
    '' ''    End If
    '' ''    Dim parametriPagina As New Hashtable
    '' ''    parametriPagina.Add("AttoConcessione", attoConcessione)
    '' ''    ParsecUtility.SessionManager.ParametriPagina = parametriPagina
    '' ''    ParsecUtility.Utility.ShowPopup(pageUrl, 610, 500, queryString, False)
    '' ''End Sub
    'luca 01/07/2020
    '' ''Private Sub CopiaBeneficiario(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    '' ''    Dim attoConcessione As ParsecAdmin.AttoConcessione = AttiConcessione.Where(Function(b) b.Id = id).FirstOrDefault
    '' ''    Dim clone = CopiaAttoConcessione(attoConcessione, False)
    '' ''    Me.VisualizzaAttoConcessione(clone, Me.InserisciBeneficiarioImageButton.ClientID, "Copia")
    '' ''End Sub
    'luca 01/07/2020
    '' ''Private Function CopiaAttoConcessione(ByVal attoConcessione As ParsecAdmin.AttoConcessione, ByVal copiaDatiBenefificario As Boolean) As ParsecAdmin.AttoConcessione
    '' ''    Dim clone As New ParsecAdmin.AttoConcessione
    '' ''    clone.TitoloNorma = attoConcessione.TitoloNorma
    '' ''    clone.Modalita = attoConcessione.Modalita
    '' ''    clone.Importo = attoConcessione.Importo
    '' ''    clone.TipologiaConcessione = attoConcessione.TipologiaConcessione
    '' ''    clone.UrlProgetto = attoConcessione.UrlProgetto
    '' ''    clone.ProgettoTemp = attoConcessione.ProgettoTemp
    '' ''    clone.UrlCurriculum = attoConcessione.UrlCurriculum
    '' ''    clone.CurriculumTemp = attoConcessione.CurriculumTemp
    '' ''    clone.Path = attoConcessione.Path
    '' ''    If copiaDatiBenefificario Then
    '' ''        clone.DatoFiscaleBeneficiario = attoConcessione.DatoFiscaleBeneficiario
    '' ''        clone.Beneficiario = attoConcessione.Beneficiario
    '' ''        clone.IdBeneficiario = attoConcessione.IdBeneficiario
    '' ''    End If
    '' ''    clone.Id = attoConcessione.Id
    '' ''    Return clone
    '' ''End Function

#End Region

#Region "INCARICO DIPENDENTE"

    'luca 01/07/2020
    '' ''Protected Sub TrovaBeneficiarioIncaricoDipendenteImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaBeneficiarioIncaricoDipendenteImageButton.Click

    '' ''    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
    '' ''    Dim queryString As New Hashtable
    '' ''    queryString.Add("obj", Me.AggiornaBeneficiarioIncaricoDipendenteImageButton.ClientID)
    '' ''    queryString.Add("mode", "search")

    '' ''    Dim parametriPagina As New Hashtable

    '' ''    ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    '' ''    If String.IsNullOrEmpty(Me.BeneficiarioIncaricoComboBox.SelectedValue) Then
    '' ''        parametriPagina.Add("Filtro", Me.BeneficiarioIncaricoComboBox.Text)

    '' ''        If Not String.IsNullOrEmpty(Me.BeneficiarioIncaricoComboBox.Text) Then
    '' ''            Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.BeneficiarioIncaricoComboBox.Text) And c.LogStato Is Nothing).ToList
    '' ''            If struttureEsterne.Count = 1 Then
    '' ''                Me.BeneficiarioIncaricoComboBox.SelectedValue = struttureEsterne(0).Id
    '' ''                Me.BeneficiarioIncaricoComboBox.Text = struttureEsterne(0).Denominazione & " " & If(Not String.IsNullOrEmpty(struttureEsterne(0).Nome), struttureEsterne(0).Nome & ", ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Indirizzo), struttureEsterne(0).Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Comune), struttureEsterne(0).Comune & " ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).CAP), struttureEsterne(0).CAP & " ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Provincia), "(" & struttureEsterne(0).Provincia & ")", "")
    '' ''                ParsecUtility.SessionManager.ParametriPagina = Nothing
    '' ''            Else
    '' ''                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''            End If
    '' ''            rubrica.Dispose()
    '' ''        Else
    '' ''            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''        End If
    '' ''    Else
    '' ''        Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''        Dim struttureEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.BeneficiarioIncaricoComboBox.SelectedValue)).FirstOrDefault
    '' ''        rubrica.Dispose()
    '' ''        If Not struttureEsterna Is Nothing Then
    '' ''            parametriPagina.Add("Filtro", struttureEsterna.Denominazione)
    '' ''        End If
    '' ''        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub AggiornaBeneficiarioIncaricoDipendente(ByVal strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo)
    '' ''    Me.BeneficiarioIncaricoComboBox.SelectedValue = strutturaEsterna.Id
    '' ''    Me.BeneficiarioIncaricoComboBox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "") 'strutturaEsterna.Denominazione
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiornaBeneficiarioIncaricoDipendenteImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBeneficiarioIncaricoDipendenteImageButton.Click
    '' ''    If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
    '' ''        Dim r As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
    '' ''        AggiornaBeneficiarioIncaricoDipendente(r)
    '' ''        ParsecUtility.SessionManager.Rubrica = Nothing
    '' ''    End If
    '' ''End Sub
#End Region

#Region "INCARICO AMMINISTRATIVO/DIRIGENZIALE"

    'luca 01/07/2020
    '' ''Protected Sub TrovaTitolareIncaricoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaTitolareIncaricoImageButton.Click

    '' ''    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
    '' ''    Dim queryString As New Hashtable
    '' ''    queryString.Add("obj", Me.AggiornaTitolareIncaricoImageButton.ClientID)
    '' ''    queryString.Add("mode", "search")

    '' ''    Dim parametriPagina As New Hashtable

    '' ''    ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    '' ''    If String.IsNullOrEmpty(Me.TitolareIncaricoComboBox.SelectedValue) Then
    '' ''        parametriPagina.Add("Filtro", Me.TitolareIncaricoComboBox.Text)

    '' ''        If Not String.IsNullOrEmpty(Me.TitolareIncaricoComboBox.Text) Then
    '' ''            Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.TitolareIncaricoComboBox.Text) And c.LogStato Is Nothing).ToList
    '' ''            If struttureEsterne.Count = 1 Then
    '' ''                Me.TitolareIncaricoComboBox.SelectedValue = struttureEsterne(0).Id
    '' ''                Me.TitolareIncaricoComboBox.Text = struttureEsterne(0).Denominazione & " " & If(Not String.IsNullOrEmpty(struttureEsterne(0).Nome), struttureEsterne(0).Nome & ", ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Indirizzo), struttureEsterne(0).Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Comune), struttureEsterne(0).Comune & " ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).CAP), struttureEsterne(0).CAP & " ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Provincia), "(" & struttureEsterne(0).Provincia & ")", "")
    '' ''                ParsecUtility.SessionManager.ParametriPagina = Nothing
    '' ''            Else
    '' ''                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''            End If
    '' ''            rubrica.Dispose()
    '' ''        Else
    '' ''            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''        End If
    '' ''    Else
    '' ''        Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''        Dim struttureEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.TitolareIncaricoComboBox.SelectedValue)).FirstOrDefault
    '' ''        rubrica.Dispose()
    '' ''        If Not struttureEsterna Is Nothing Then
    '' ''            parametriPagina.Add("Filtro", struttureEsterna.Denominazione)
    '' ''        End If
    '' ''        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiornaTitolareIncaricoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaTitolareIncaricoImageButton.Click
    '' ''    If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
    '' ''        Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
    '' ''        Me.TitolareIncaricoComboBox.SelectedValue = strutturaEsterna.Id
    '' ''        Me.TitolareIncaricoComboBox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "")
    '' ''        ParsecUtility.SessionManager.Rubrica = Nothing
    '' ''    End If
    '' ''End Sub

#End Region

#Region "ENTE CONTROLLATO"

    'luca 01/07/2020
    '' ''Protected Sub TrovaEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaEnteControllatoImageButton.Click

    '' ''    Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
    '' ''    Dim queryString As New Hashtable
    '' ''    queryString.Add("obj", Me.AggiornaEnteControllatoImageButton.ClientID)
    '' ''    queryString.Add("mode", "search")

    '' ''    Dim parametriPagina As New Hashtable

    '' ''    ParsecUtility.SessionManager.ParametriPagina = parametriPagina

    '' ''    If String.IsNullOrEmpty(Me.RagioneSocialeEnteControllatoCombobox.SelectedValue) Then
    '' ''        parametriPagina.Add("Filtro", Me.RagioneSocialeEnteControllatoCombobox.Text)

    '' ''        If Not String.IsNullOrEmpty(Me.RagioneSocialeEnteControllatoCombobox.Text) Then
    '' ''            Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.RagioneSocialeEnteControllatoCombobox.Text) And c.LogStato Is Nothing).ToList
    '' ''            If struttureEsterne.Count = 1 Then
    '' ''                Me.RagioneSocialeEnteControllatoCombobox.SelectedValue = struttureEsterne(0).Id
    '' ''                Me.RagioneSocialeEnteControllatoCombobox.Text = struttureEsterne(0).Denominazione & " " & If(Not String.IsNullOrEmpty(struttureEsterne(0).Nome), struttureEsterne(0).Nome & ", ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Indirizzo), struttureEsterne(0).Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Comune), struttureEsterne(0).Comune & " ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).CAP), struttureEsterne(0).CAP & " ", "") & If(Not String.IsNullOrEmpty(struttureEsterne(0).Provincia), "(" & struttureEsterne(0).Provincia & ")", "")
    '' ''                ParsecUtility.SessionManager.ParametriPagina = Nothing
    '' ''            Else
    '' ''                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''            End If
    '' ''            rubrica.Dispose()
    '' ''        Else
    '' ''            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''        End If
    '' ''    Else
    '' ''        Dim rubrica As New ParsecAdmin.RubricaRepository
    '' ''        Dim struttureEsterna = rubrica.GetQuery.Where(Function(c) c.Id = CInt(Me.RagioneSocialeEnteControllatoCombobox.SelectedValue)).FirstOrDefault
    '' ''        rubrica.Dispose()
    '' ''        If Not struttureEsterna Is Nothing Then
    '' ''            parametriPagina.Add("Filtro", struttureEsterna.Denominazione)
    '' ''        End If
    '' ''        ParsecUtility.Utility.ShowPopup(pageUrl, 930, 750, queryString, False)
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiornaEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaEnteControllatoImageButton.Click
    '' ''    If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
    '' ''        Dim strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica
    '' ''        Me.RagioneSocialeEnteControllatoCombobox.SelectedValue = strutturaEsterna.Id
    '' ''        Me.RagioneSocialeEnteControllatoCombobox.Text = strutturaEsterna.Denominazione & " " & If(Not String.IsNullOrEmpty(strutturaEsterna.Nome), strutturaEsterna.Nome & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Indirizzo), strutturaEsterna.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Comune), strutturaEsterna.Comune & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.CAP), strutturaEsterna.CAP & " ", "") & If(Not String.IsNullOrEmpty(strutturaEsterna.Provincia), "(" & strutturaEsterna.Provincia & ")", "")
    '' ''        ParsecUtility.SessionManager.Rubrica = Nothing
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiTrattamentoEconomicoRappresentanteEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiTrattamentoEconomicoRappresentanteEnteControllatoImageButton.Click
    '' ''    If Not String.IsNullOrEmpty(Me.TrattamentoEconomicoRappresentanteEnteControllatoTextbox.Text.Trim) Then
    '' ''        Dim trattamento As New ParsecAdmin.TrattamentoEconomicoRappresentante
    '' ''        If Me.TrattamentiEconomiciRappresentante.Count > 0 Then
    '' ''            trattamento.Id = Me.TrattamentiEconomiciRappresentante.Max(Function(c) c.Id) + 1
    '' ''        Else
    '' ''            trattamento.Id = 0
    '' ''        End If
    '' ''        trattamento.Descrizione = Me.TrattamentoEconomicoRappresentanteEnteControllatoTextbox.Text.Replace(";", ",")
    '' ''        Me.TrattamentiEconomiciRappresentante.Add(trattamento)
    '' ''        Me.TrattamentoEconomicoRappresentanteEnteControllatoTextbox.Text = String.Empty
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub DeleteTrattamentoEconomicoRappresentanteEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles DeleteTrattamentoEconomicoRappresentanteEnteControllatoImageButton.Click
    '' ''    For Each item As GridDataItem In Me.TrattamentiEconomiciRappresentantiEnteControllatoGridView.SelectedItems
    '' ''        Me.EliminaTrattamentoEconomicoRappresentante(item)
    '' ''    Next
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub TrattamentiEconomiciRappresentantiEnteControllatoGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles TrattamentiEconomiciRappresentantiEnteControllatoGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.EliminaTrattamentoEconomicoRappresentante(e.Item)
    '' ''    End Select
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub EliminaTrattamentoEconomicoRappresentante(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    '' ''    Dim trattamento As ParsecAdmin.TrattamentoEconomicoRappresentante = Me.TrattamentiEconomiciRappresentante.Where(Function(c) c.Id = id).FirstOrDefault
    '' ''    If Not trattamento Is Nothing Then
    '' ''        Me.TrattamentiEconomiciRappresentante.Remove(trattamento)
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub EliminaTrattamentoEconomicoIncaricoAmministrazione(ByVal item As Telerik.Web.UI.GridDataItem)

    '' ''    Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
    '' ''    'Me.TrattamentiEconomiciIncaricoAmministratore.RemoveAll(Function(c) c.Id = id)

    '' ''    Dim trattamento As ParsecAdmin.TrattamentoEconomicoIncaricoAmministratore = Me.TrattamentiEconomiciIncaricoAmministratore.Where(Function(c) c.Id = id).FirstOrDefault
    '' ''    If Not trattamento Is Nothing Then
    '' ''        Me.TrattamentiEconomiciIncaricoAmministratore.Remove(trattamento)
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiTrattamentoEconomicoIncaricoAmministratoreEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiTrattamentoEconomicoIncaricoAmministratoreEnteControllatoImageButton.Click

    '' ''    If Not String.IsNullOrEmpty(Me.IncaricoAmministratoreEnteControllatoTextBox.Text.Trim) AndAlso Not String.IsNullOrEmpty(Me.TrattamentoEconomicoAmministratoreEnteControllatoTextBox.Text.Trim) Then

    '' ''        Dim trattamento As New ParsecAdmin.TrattamentoEconomicoIncaricoAmministratore
    '' ''        If Me.TrattamentiEconomiciIncaricoAmministratore.Count > 0 Then
    '' ''            trattamento.Id = Me.TrattamentiEconomiciIncaricoAmministratore.Max(Function(c) c.Id) + 1
    '' ''        Else
    '' ''            trattamento.Id = 0
    '' ''        End If
    '' ''        trattamento.Incarico = Me.IncaricoAmministratoreEnteControllatoTextBox.Text.Replace("#", " ")
    '' ''        trattamento.Descrizione = Me.TrattamentoEconomicoAmministratoreEnteControllatoTextBox.Text.Replace("#", " ")

    '' ''        Me.TrattamentiEconomiciIncaricoAmministratore.Add(trattamento)


    '' ''        Me.IncaricoAmministratoreEnteControllatoTextBox.Text = String.Empty
    '' ''        Me.TrattamentoEconomicoAmministratoreEnteControllatoTextBox.Text = String.Empty
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub DeleteTrattamentoEconomicoIncaricoAmministratoreEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles DeleteTrattamentoEconomicoIncaricoAmministratoreEnteControllatoImageButton.Click
    '' ''    For Each item As GridDataItem In Me.TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView.SelectedItems
    '' ''        Me.EliminaTrattamentoEconomicoIncaricoAmministrazione(item)
    '' ''    Next
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.EliminaTrattamentoEconomicoIncaricoAmministrazione(e.Item)
    '' ''    End Select
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiBilancioEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiBilancioEnteControllatoImageButton.Click
    '' ''    If Me.AnnoBilancioEnteControllatoTextBox.Value.HasValue AndAlso Me.BilancioEnteControllatoTextBox.Value.HasValue Then
    '' ''        Dim bilancio As ParsecAdmin.BilancioEsercizioEnteControllato = Me.BilanciEnteControllato.Where(Function(c) c.anno = Me.AnnoBilancioEnteControllatoTextBox.Value).FirstOrDefault
    '' ''        If bilancio Is Nothing Then
    '' ''            bilancio = New ParsecAdmin.BilancioEsercizioEnteControllato
    '' ''            bilancio.anno = Me.AnnoBilancioEnteControllatoTextBox.Value
    '' ''            bilancio.risultato = Me.BilancioEnteControllatoTextBox.Value
    '' ''            Me.BilanciEnteControllato.Add(bilancio)
    '' ''            Me.AnnoBilancioEnteControllatoTextBox.Value = Nothing
    '' ''            Me.BilancioEnteControllatoTextBox.Value = Nothing
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("L'anno di esercizio finanziario è stato già inserito!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub DeleteBilancioEnteControllatoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles DeleteBilancioEnteControllatoImageButton.Click
    '' ''    For Each item As GridDataItem In Me.BilanciEnteControllatoGridView.SelectedItems
    '' ''        Me.EliminaBilancio(item)
    '' ''    Next
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub BilanciEnteControllatoGridView_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles BilanciEnteControllatoGridView.ItemCommand
    '' ''    Select Case e.CommandName
    '' ''        Case "Delete"
    '' ''            Me.EliminaBilancio(e.Item)
    '' ''    End Select
    '' ''End Sub

    'luca 01/07/2020
    '' ''Private Sub EliminaBilancio(ByVal item As Telerik.Web.UI.GridDataItem)
    '' ''    Dim anno As String = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Anno")
    '' ''    Dim bilancio As ParsecAdmin.BilancioEsercizioEnteControllato = Me.BilanciEnteControllato.Where(Function(c) c.anno = anno).FirstOrDefault
    '' ''    If Not bilancio Is Nothing Then
    '' ''        Me.BilanciEnteControllato.Remove(bilancio)
    '' ''    End If
    '' ''End Sub

#End Region

#Region "GESTIONE FASCICOLI"

    Private Sub CaricaFasi()
        Dim dati As New Dictionary(Of String, String)
        dati.Add(ParsecAdmin.FaseDocumentoEnumeration.INIZIALE, "Iniziale")
        dati.Add(ParsecAdmin.FaseDocumentoEnumeration.FINALE, "Finale")
        Dim ds = dati.Select(Function(c) New With {.Id = c.Key, .Descrizione = c.Value})
        Me.FaseDocumentoFascicoloComboBox.DataSource = ds
        Me.FaseDocumentoFascicoloComboBox.DataTextField = "Descrizione"
        Me.FaseDocumentoFascicoloComboBox.DataValueField = "Id"
        Me.FaseDocumentoFascicoloComboBox.DataBind()
        Me.FaseDocumentoFascicoloComboBox.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("", "0"))
        Me.FaseDocumentoFascicoloComboBox.SelectedIndex = 0
    End Sub

    Private Sub AggiornaGrigliaFascicoli()
        If Not Me.Fascicoli Is Nothing Then
            Me.FascicoliGridView.DataSource = Me.Fascicoli.Where(Function(f) f.Stato Is Nothing)
            Me.FascicoliGridView.DataBind()
        End If
    End Sub

    Protected Sub FascicoliGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FascicoliGridView.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Me.ModificaFascicolo(e.Item)
            Case "Delete"
                Me.EliminaFascicolo(e.Item)
        End Select
    End Sub

    Private Sub ModificaFascicolo(ByVal item As GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/FascicoliPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.ModificaFascicoloImageButton.ClientID)
        queryString.Add("mode", "update")
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdFascicolo", id)
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 600, queryString, False)
    End Sub

    Private Sub EliminaFascicolo(ByVal item As Telerik.Web.UI.GridDataItem)
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim fascicolo = Me.Fascicoli.Where(Function(c) c.Id = id).FirstOrDefault
        If Not fascicolo Is Nothing Then
            Me.Fascicoli.Remove(fascicolo)
        End If
    End Sub

    Private Sub ResettaVistaFascicoli()
        Me.FaseDocumentoFascicoloComboBox.SelectedIndex = 0
        Me.Fascicoli = New List(Of ParsecAdmin.Fascicolo)
    End Sub


    Protected Sub TrovaFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaFascicoloImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/ElencoFascicoliPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("modalita", "ricerca")
        queryString.Add("obj", Me.InserisciFascicoloImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 650, queryString, False)
    End Sub

    Protected Sub NuovoFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NuovoFascicoloImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/FascicoliPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.InserisciFascicoloImageButton.ClientID)
        queryString.Add("mode", "insert")
        Dim parametriPagina As New Hashtable
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 600, queryString, False)
    End Sub


    Protected Sub InserisciFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles InserisciFascicoloImageButton.Click
        If Not ParsecUtility.SessionManager.Fascicolo Is Nothing Then
            Dim fascicolo As ParsecAdmin.Fascicolo = ParsecUtility.SessionManager.Fascicolo

            If Me.FaseDocumentoFascicoloComboBox.SelectedIndex > 0 Then
                fascicolo.Fase = Me.FaseDocumentoFascicoloComboBox.SelectedValue
            Else
                fascicolo.Fase = Nothing
            End If

            Dim exist As Boolean = Not Me.Fascicoli.Where(Function(c) c.Codice = fascicolo.Codice).FirstOrDefault Is Nothing
            If Not exist Then
                Me.Fascicoli.Add(fascicolo)
            End If
            ParsecUtility.SessionManager.Fascicolo = Nothing
        End If
    End Sub

    Protected Sub ModificaFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ModificaFascicoloImageButton.Click
        If Not ParsecUtility.SessionManager.Fascicolo Is Nothing Then
            Dim fascicolo As ParsecAdmin.Fascicolo = ParsecUtility.SessionManager.Fascicolo

            Dim fascicoloPrecedente = Me.Fascicoli.Where(Function(c) c.Codice = fascicolo.Codice).FirstOrDefault
            If Not fascicoloPrecedente Is Nothing Then
                fascicolo.Fase = fascicoloPrecedente.Fase
                Dim index = Me.Fascicoli.FindIndex(Function(c) c.Codice = fascicolo.Codice)
                'Dim index2 = Me.Fascicoli.IndexOf(fascicoloPrecedente)
                Me.Fascicoli.Remove(fascicoloPrecedente)
                Me.Fascicoli.Insert(index, fascicolo)
            End If
            ParsecUtility.SessionManager.Fascicolo = Nothing

        End If
    End Sub

    'Protected Sub AggiungiFascicoloImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiFascicoloImageButton.Click
    '    If Not String.IsNullOrEmpty(FascicoliComboBox.SelectedValue) Then
    '        'Associo il documento ad un fascicolo esistente
    '        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
    '        Dim idFascicolo As Integer = FascicoliComboBox.SelectedValue
    '        Dim descrizioneTitolare As String = FascicoliComboBox.Text
    '        Dim fascicoloR As New ParsecAdmin.FascicoliRepository
    '        Dim fascicoloFiltro As New ParsecAdmin.FascicoloFiltro
    '        fascicoloFiltro.Id = idFascicolo

    '        fascicoloFiltro.IdUtenteCollegato = utenteCollegato.Id
    '        Dim nuovoFascicolo As ParsecAdmin.Fascicolo = fascicoloR.GetView(fascicoloFiltro).FirstOrDefault
    '        If CBool(Me.FaseDocumentoFascicoloComboBox.SelectedIndex) Then
    '            Dim docFasR As New ParsecAdmin.FascicoloDocumentoRepository
    '            If Not docFasR.EsisteDocumentoFase(nuovoFascicolo.Id, Me.FaseDocumentoFascicoloComboBox.SelectedValue) Then
    '                nuovoFascicolo.Fase = Me.FaseDocumentoFascicoloComboBox.SelectedValue
    '            Else
    '                ParsecUtility.Utility.MessageBox("Al fascicolo selezionato è già associato un documento con la stessa fase.", False)
    '                Exit Sub
    '            End If
    '            docFasR.Dispose()
    '        End If

    '        Dim exist As Boolean = Not Me.Fascicoli.Where(Function(c) c.Id = idFascicolo).FirstOrDefault Is Nothing
    '        If exist Then
    '            ParsecUtility.Utility.MessageBox("Il fascicolo selezionato è già presente!", False)
    '            Exit Sub
    '        End If

    '        Me.Fascicoli.Add(nuovoFascicolo)
    '        FascicoliComboBox.Text = ""
    '        FascicoliComboBox.SelectedValue = Nothing
    '        AggiornaGrigliaFascicoli()
    '        fascicoloR.Dispose()
    '    Else
    '        ParsecUtility.Utility.MessageBox("Nessun Fascicolo selezionato: impossibile proseguire!", False)
    '    End If


    'End Sub

    Private Sub ImpostaAbilitazioneSchedaFascicoli(enabled As Boolean)
        '***************************************************************************************
        'SCHEDA FASCICOLI
        '***************************************************************************************

        Me.FaseDocumentoFascicoloComboBox.Enabled = enabled
        Me.NuovoFascicoloImageButton.Visible = enabled
        Me.TrovaFascicoloImageButton.Visible = enabled
        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Select").Visible = enabled
        Me.FascicoliGridView.MasterTableView.Columns.FindByUniqueName("Delete").Visible = enabled
    End Sub



#End Region

#Region "WEB METHODS"

    <WebMethod()> _
    Public Shared Function GetElementiRubrica(ByVal context As RadComboBoxContext) As RadComboBoxData
        Const ItemsPerRequest As Integer = 10
        Dim rubrica As New ParsecAdmin.RubricaRepository
        Dim data = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(context.Text) And c.Denominazione <> "" And c.LogStato Is Nothing And c.InRubrica = True).OrderBy(Function(c) c.Denominazione).ToList
        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData) '(endOffset - itemOffset)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            'Dim item = data.Skip(i).FirstOrDefault
            Dim item = data.ElementAt(i)
            itemData.Text = (item.Denominazione & " " & If(Not String.IsNullOrEmpty(item.Nome), item.Nome & ", ", "") & If(Not String.IsNullOrEmpty(item.Indirizzo), item.Indirizzo & ", ", "") & If(Not String.IsNullOrEmpty(item.Comune), item.Comune & " ", "") & If(Not String.IsNullOrEmpty(item.CAP), item.CAP & " ", "") & If(Not String.IsNullOrEmpty(item.Provincia), "(" & item.Provincia & ")", "")).Trim
            itemData.Value = item.Id
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function

#End Region

#Region "ESPORTAZIONE EXCEL"

    Private Sub EsportaXls()

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

        If Me.Documenti Is Nothing Then
            Throw New ApplicationException("Non ci sono atti decisionali." & vbCrLf & "Impossibile eseguire l'esportazione!")
        End If

        If Me.Documenti.Count = 0 Then
            Throw New ApplicationException("Non ci sono atti decisionali." & vbCrLf & "Impossibile eseguire l'esportazione!")
        End If

        '***************************************************************************************
        'CREO IL FILE
        '***************************************************************************************
        Dim utente As ParsecAdmin.Utente = ParsecUtility.Applicazione.UtenteCorrente
        Dim exportFilename As String = String.Format("Atti_{0}_AL_{1}.xls", utente.Id, Now.ToString("ddMM_yyyy"))
        Dim fullPathExport As String = pathExport & exportFilename

        Dim swExport As New IO.StreamWriter(fullPathExport, False, System.Text.Encoding.Default)
        Dim line As New StringBuilder

        line.Append("NUMERO" & vbTab)
        line.Append("TIPO" & vbTab)
        line.Append("DATA" & vbTab)
        line.Append("OGGETTO" & vbTab)
        line.Append("UFFICIO" & vbTab)
        line.Append("SETTORE" & vbTab)
        swExport.WriteLine(line.ToString)
        line.Clear()

        For Each doc As ParsecAtt.Documento In Me.Documenti

            line.Append(doc.ContatoreGenerale.ToString & vbTab)
            line.Append(Me.Escape(doc.DescrizioneTipologia) & vbTab)
            line.Append(String.Format("{0:dd/MM/yyyy}", doc.DataDocumento) & vbTab)
            line.Append(Me.Escape(doc.Oggetto) & vbTab)
            line.Append(Me.Escape(doc.DescrizioneUfficio) & vbTab)
            line.Append(Me.Escape(doc.DescrizioneSettore) & vbTab)

            swExport.WriteLine(line.ToString)
            line.Clear()
        Next
        swExport.Close()

        '***************************************************************************************

        '***************************************************************************************
        'VISUALIZZO IL FILE
        '***************************************************************************************
        Session("AttachmentFullName") = fullPathExport
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/ExportExcelPage.aspx"
        ParsecUtility.Utility.PageReload(pageUrl, False)
        '***************************************************************************************

        '***************************************************************************************
        'SALVO L'ESPORTAZIONE NEL DB
        '***************************************************************************************
        Dim esportazioniExcel As New ParsecAdmin.ExportExcelRepository
        Dim exportExcel As ParsecAdmin.ExportExcel = esportazioniExcel.CreateFromInstance(Nothing)
        exportExcel.NomeFile = exportFilename
        exportExcel.Oggetto = "Elenco Atti Decisionali"
        exportExcel.Utente = utente.Username
        exportExcel.Data = Now
        esportazioniExcel.Save(exportExcel)
        esportazioniExcel.Dispose()
        '***************************************************************************************
    End Sub

    'https://www.ietf.org/rfc/rfc4180.txt

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


    Protected Sub EspRicXls_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles EspRicXls.Click
        Try
            Me.EsportaXls()
        Catch ex As Exception
            If Not ex.InnerException Is Nothing Then
                ParsecUtility.Utility.MessageBox(ex.InnerException.Message, False)
            Else
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End If
        End Try

    End Sub


#End Region

#Region "EVENTI GRIGLIA FULL SIZE"


    Protected Sub VisualizzaSchermoInteroImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles VisualizzaSchermoInteroImageButton.Click
        Me.FullSize = True

        'RESETTO I FILTRI
        For Each column As GridColumn In Me.FullSizeDocumentiGridView.MasterTableView.RenderColumns
            If column.SupportsFiltering Then
                column.CurrentFilterValue = String.Empty
                column.CurrentFilterFunction = GridKnownFunction.NoFilter
            End If
        Next

        Me.FullSizeDocumentiGridView.MasterTableView.FilterExpression = String.Empty
        Me.FullSizeDocumentiGridView.CurrentPageIndex = 0
        Me.FullSizeDocumentiGridView.Rebind()


        Dim tipoDocumento = CType(CInt(Request.QueryString("Tipo")), ParsecAtt.TipoDocumento)
        Dim proposta As Boolean = tipoDocumento = ParsecAtt.TipoDocumento.PropostaDetermina OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaDelibera OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaOrdinanza OrElse tipoDocumento = ParsecAtt.TipoDocumento.PropostaDecreto
        Me.FullSizeDocumentiGridView.MasterTableView.GetColumnSafe("VisualizzaCopiaDocumento").Display = Not proposta


        Dim script As New StringBuilder
        script.AppendLine("<script>")
        script.AppendLine("ShowFullSizeGridPanel();")
        script.AppendLine("</script>")
        ParsecUtility.Utility.RegisterScript(script, False)
    End Sub



    Protected Sub FullSizeDocumentiGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FullSizeDocumentiGridView.NeedDataSource
        If Me.FullSize Then
            Me.FullSizeDocumentiGridView.DataSource = Me.Documenti
        Else
            Me.FullSizeDocumentiGridView.DataSource = New List(Of ParsecAtt.Documento)
        End If
    End Sub


    Protected Sub FullSizeDocumentiGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FullSizeDocumentiGridView.ItemCommand

        Select Case e.CommandName
            Case "Select"
                Dim aggiorna As Boolean = False
                Dim selezionato As Boolean = Not Me.Documento Is Nothing
                Dim id = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Id"))

                If selezionato Then
                    If Me.Documento.Id <> id Then
                        aggiorna = True
                    End If
                End If

                If aggiorna OrElse Not selezionato Then

                    Me.SelezionaDocumento(e)

                    'DESELEZIONO LA RIGA
                    Me.DocumentiGridView.SelectedIndexes.Clear()

                    'SELEZIONO LA NUOVA RIGA
                    If Not Me.Documento Is Nothing Then
                        Dim item As GridDataItem = Me.DocumentiGridView.MasterTableView.FindItemByKeyValue("Id", id)
                        If Not item Is Nothing Then
                            item.Selected = True
                        End If
                    End If
                End If


                Dim script As New StringBuilder
                script.AppendLine("<script>")
                script.AppendLine("HideFullSizeGridPanel();")
                script.AppendLine("</script>")
                ParsecUtility.Utility.RegisterScript(script, False)
                Me.FullSize = False
            Case "VisualizzaDocumento"

                Me.VisualizzaDocumento(e.Item, False)

            Case "VisualizzaCopiaDocumento"
                Me.VisualizzaDocumento(e.Item, True)
        End Select


    End Sub

    Private Sub VisualizzaDocumento(ByVal item As Telerik.Web.UI.GridDataItem, ByVal copia As Boolean)
        Dim id = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id"))

        Dim documenti As New ParsecAtt.DocumentoRepository
        Dim documento = documenti.GetFullById(id)
        documenti.Dispose()

        If Not documento Is Nothing Then
            Dim nomefile As String = documento.Nomefile

            Dim annoEsercizio As Integer = Now.Year
            If documento.Modello.Proposta Then
                annoEsercizio = documento.DataProposta.Value.Year
            Else
                annoEsercizio = documento.Data.Value.Year
            End If

            Dim localPath As String = String.Format("{0}{1}\{2}", ParsecAdmin.WebConfigSettings.GetKey("PathAtti"), annoEsercizio, nomefile)
            If IO.File.Exists(localPath) Then
                If copia Then
                    Me.VisualizzaCopiaDocumento(nomefile, annoEsercizio, False)
                Else
                    Me.VisualizzaDocumento(nomefile, annoEsercizio, False)
                End If
            Else
                ParsecUtility.Utility.MessageBox("Il file del documento selezionato non esiste!", False)
            End If
        End If

    End Sub


    Protected Sub FullSizeDocumentiGridView_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FullSizeDocumentiGridView.ItemCreated

        Dim browser = Request.Browser.Browser

        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False

            If browser.ToLower.Contains("ie") Then
                e.Item.Style.Add("position", "relative")
                e.Item.Style.Add("bottom", "expression(this.offsetParent.scrollHeight - this.offsetParent.scrollTop - this.offsetParent.clientHeight-1)")
                e.Item.Style.Add("z-index", "99")
            End If

        End If


        If TypeOf e.Item Is GridFilteringItem Then

            If browser.ToLower.Contains("ie") Then
                e.Item.Style.Add("position", "relative")
                e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
                e.Item.Style.Add("z-index", "99")
                'e.Item.Style.Add("background-color", "White")
            End If
        End If

        If TypeOf e.Item Is GridHeaderItem Then
            If browser.ToLower.Contains("ie") Then
                e.Item.Style.Add("position", "relative")
                e.Item.Style.Add("top", "expression(this.offsetParent.scrollTop-1)")
                e.Item.Style.Add("z-index", "99")
                e.Item.Style.Add("background-color", "White")

            End If
        End If

    End Sub


    Protected Sub NoPaging_Click(sender As Object, e As System.EventArgs) Handles NoPaging.Click
        Me.DocumentiGridView.AllowPaging = Not Me.DocumentiGridView.AllowPaging
        If Me.DocumentiGridView.AllowPaging Then
            Me.NoPaging.Text = "Non Paginare"
            Me.NoPaging.Icon.PrimaryIconUrl = "~/images/Next.png"
        Else
            Me.NoPaging.Text = "Paginare"
            Me.NoPaging.Icon.PrimaryIconUrl = "~/images/Previous.png"
        End If
        Me.DocumentiGridView.Rebind()
    End Sub

    Protected Sub FullSizeNoPaging_Click(sender As Object, e As System.EventArgs) Handles FullSizeNoPaging.Click
        Me.FullSizeDocumentiGridView.AllowPaging = Not Me.FullSizeDocumentiGridView.AllowPaging
        If Me.FullSizeDocumentiGridView.AllowPaging Then
            Me.FullSizeNoPaging.Text = "Non Paginare"
            Me.FullSizeNoPaging.Icon.PrimaryIconUrl = "~/images/Next.png"
        Else
            Me.FullSizeNoPaging.Text = "Paginare"
            Me.FullSizeNoPaging.Icon.PrimaryIconUrl = "~/images/Previous.png"
        End If
        Me.FullSizeDocumentiGridView.Rebind()
    End Sub

#End Region

#Region "GESTIONE FATTURA ELETTRONICA"

    Protected Sub TrovaFatturaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaFatturaImageButton.Click
        Dim pageUrl As String = String.Empty
        Dim queryString As New Hashtable
        pageUrl = "~/UI/Protocollo/pages/search/RicercaFatturaElettronicaPage.aspx"
        queryString.Add("obj", Me.AllegaFatturaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 970, 500, queryString, False)
    End Sub

    Protected Sub AllegaFatturaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AllegaFatturaImageButton.Click
        If Not ParsecUtility.SessionManager.FatturaElettronica Is Nothing Then
            Dim fattura As ParsecPro.FatturaElettronica = ParsecUtility.SessionManager.FatturaElettronica

            If String.IsNullOrEmpty(Me.DescrizioneDocumentoTextBox.Text.Trim) Then
                Me.DescrizioneDocumentoTextBox.Text = fattura.Oggetto
            End If


            Dim filename As String = fattura.MessaggioSdI.Nomefile

            '*********************************************************************************************************************************************************************
            'COPIO LA FATTURA NELLA CARTELLA TEMPORANEA E AGGIUNGO L'ALLEGATO NELLA COLLEZIONE
            '*********************************************************************************************************************************************************************
            Dim pathFattura As String = ParsecAdmin.WebConfigSettings.GetKey("PathFattureElettroniche") & fattura.MessaggioSdI.PercorsoRelativo & filename
            Dim filenameTemp As String = Guid.NewGuid.ToString & "_" & filename
            Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")
            If Not IO.Directory.Exists(pathRootTemp) Then
                IO.Directory.CreateDirectory(pathRootTemp)
            End If
            Dim pathDownload As String = pathRootTemp & filenameTemp


            Dim impronta = ParsecUtility.Utility.CalcolaHashFromFile(pathFattura)
            Dim improntaAllegato As String = BitConverter.ToString(impronta).Replace("-", "")
            If Me.Allegati.Where(Function(c) c.Impronta = improntaAllegato).FirstOrDefault Is Nothing Then

                IO.File.Copy(pathFattura, pathDownload, True)

                Dim allegato As New ParsecAtt.Allegato
                allegato.Id = -1

                If Me.Allegati.Count > 0 Then
                    Dim allId = Me.Allegati.Min(Function(c) c.Id) - 1
                    If allId < 0 Then
                        allegato.Id = allId
                    End If
                End If

                allegato.Nomefile = filename
                allegato.NomeFileTemp = filenameTemp
                allegato.Pubblicato = False
                allegato.Oggetto = Me.DescrizioneDocumentoTextBox.Text
                allegato.PercorsoRoot = ParsecAdmin.WebConfigSettings.GetKey("PathDocumenti")
                allegato.PercorsoRootTemp = pathRootTemp
                allegato.IdTipologiaAllegato = CInt(Me.TipologiaAllegatoComboBox.SelectedValue)
                allegato.Impronta = improntaAllegato
                allegato.IdFatturaElettronica = fattura.Id
                Me.Allegati.Add(allegato)
            Else
                ParsecUtility.Utility.MessageBox("Il file selezionato è stato già allegato!", False)
            End If
            '*********************************************************************************************************************************************************************

            Me.DescrizioneDocumentoTextBox.Text = String.Empty
            ParsecUtility.SessionManager.FatturaElettronica = Nothing
        End If

    End Sub

    Protected Sub VisualizzaFatturaControl_OnCloseEvent() Handles VisualizzaFatturaControl.OnCloseEvent
        Me.VisualizzaFatturaControl.Visible = False
    End Sub

    Private Sub VisualizzaFattura(ByVal idFattura As Integer)

        Dim fatture As New ParsecPro.FatturaElettronicaRepository
        Dim fattura = fatture.Where(Function(c) c.Id = idFattura).FirstOrDefault

        If Not fattura Is Nothing Then
            Me.VisualizzaFatturaControl.ShowPanel()
            Me.VisualizzaFatturaControl.InitUI(fattura)
        End If
        fatture.Dispose()

    End Sub

#End Region

#Region "GESTIONE CURRICULUM - INCONFERIBILITA' - INCOPATIBILITA' PUBBLICAZIONE "


    '**********************************************************************************************************************************************************************
    'INCARICO AMMINISTRATIVO/DIRIGENZIALE
    '**********************************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AggiungiInconferibilitaIncaricoAmministrativoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiInconferibilitaIncaricoAmministrativoImageButton.Click

    '' ''    If Me.InconferibilitaIncaricoAmministrativoUpload.UploadedFiles.Count = 0 Then
    '' ''        ParsecUtility.Utility.MessageBox("Per inserire la dichiarazione, è necessario specificarne il file relativo!", False)
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim file As Telerik.Web.UI.UploadedFile = Me.InconferibilitaIncaricoAmministrativoUpload.UploadedFiles(0)

    '' ''    If Not String.IsNullOrEmpty(file.FileName) Then

    '' ''        Dim maxKiloBytesLength As Integer = CInt(ParsecAdmin.WebConfigSettings.GetKey("MaxKiloBytesLengthAlbo"))

    '' ''        If file.ContentLength = 0 Then
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è vuoto!", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        If (file.ContentLength / 1024) > maxKiloBytesLength Then
    '' ''            Dim mb As Single = (file.ContentLength / 1024) / 1024
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è di " & mb.ToString("0.00") & " megabyte !" & vbCrLf & "Si può allegare un file di massimo 4 megabyte.", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathDownload As String = pathRootTemp & Session.SessionID & "_" & file.FileName
    '' ''        file.SaveAs(pathDownload)

    '' ''        Me.InconferibilitaIncaricoAmministrativoLinkButton.Text = file.FileName
    '' ''        Me.NomeFileInconferibilitaIncaricoAmministrativoLabel.Text = pathDownload

    '' ''        Me.inconferibilitaIncaricoAmministrativoUpload1.Visible = False
    '' ''        Me.InconferibilitaIncaricoAmministrativoUpload2.Visible = True
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub RimuoviInconferibilitaIncaricoAmministrativoImageButton_Click(sender As Object, e As System.EventArgs) Handles RimuoviInconferibilitaIncaricoAmministrativoImageButton.Click
    '' ''    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.NomeFileInconferibilitaIncaricoAmministrativoLabel.Text
    '' ''    If IO.File.Exists(pathDownload) Then
    '' ''        IO.File.Delete(pathDownload)
    '' ''    End If
    '' ''    Me.InconferibilitaIncaricoAmministrativoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileInconferibilitaIncaricoAmministrativoLabel.Text = String.Empty

    '' ''    Me.inconferibilitaIncaricoAmministrativoUpload1.Visible = True
    '' ''    Me.InconferibilitaIncaricoAmministrativoUpload2.Visible = False
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub InconferibilitaIncaricoAmministrativoLinkButton_Click(sender As Object, e As System.EventArgs) Handles InconferibilitaIncaricoAmministrativoLinkButton.Click
    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
    '' ''    Dim filename As String = Me.InconferibilitaIncaricoAmministrativoLinkButton.Text
    '' ''    Dim filenameTemp As String = Me.NomeFileInconferibilitaIncaricoAmministrativoLabel.Text

    '' ''    If Not String.IsNullOrEmpty(filename) Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If Not String.IsNullOrEmpty(filenameTemp) Then
    '' ''            pathDownload = filenameTemp
    '' ''        Else
    '' ''            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
    '' ''            pathDownload = percorsoRoot & Me.IncaricoAmministrativoDirigenziale.path & filename
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("Il file allegato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiCurriculumIncaricoAmministrativoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiCurriculumIncaricoAmministrativoImageButton.Click

    '' ''    If Me.CurriculumIncaricoAmministrativoUpload.UploadedFiles.Count = 0 Then
    '' ''        ParsecUtility.Utility.MessageBox("Per inserire il curriculum, è necessario specificarne il file relativo!", False)
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim file As Telerik.Web.UI.UploadedFile = Me.CurriculumIncaricoAmministrativoUpload.UploadedFiles(0)

    '' ''    If Not String.IsNullOrEmpty(file.FileName) Then

    '' ''        Dim maxKiloBytesLength As Integer = CInt(ParsecAdmin.WebConfigSettings.GetKey("MaxKiloBytesLengthAlbo"))

    '' ''        If file.ContentLength = 0 Then
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è vuoto!", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        If (file.ContentLength / 1024) > maxKiloBytesLength Then
    '' ''            Dim mb As Single = (file.ContentLength / 1024) / 1024
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è di " & mb.ToString("0.00") & " megabyte !" & vbCrLf & "Si può allegare un file di massimo 4 megabyte.", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathDownload As String = pathRootTemp & Session.SessionID & "_" & file.FileName
    '' ''        file.SaveAs(pathDownload)

    '' ''        Me.CurriculumIncaricoAmministrativoLinkButton.Text = file.FileName
    '' ''        Me.NomeFileCurriculumIncaricoAmministrativoLabel.Text = pathDownload

    '' ''        Me.curriculumincaricoAmministrativoUpload1.Visible = False
    '' ''        Me.curriculumincaricoAmministrativoUpload2.Visible = True

    '' ''    End If


    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub RimuoviCurriculumIncaricoAmministrativoImageButton_Click(sender As Object, e As System.EventArgs) Handles RimuoviCurriculumIncaricoAmministrativoImageButton.Click
    '' ''    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.NomeFileCurriculumIncaricoAmministrativoLabel.Text
    '' ''    If IO.File.Exists(pathDownload) Then
    '' ''        IO.File.Delete(pathDownload)
    '' ''    End If
    '' ''    Me.CurriculumIncaricoAmministrativoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileCurriculumIncaricoAmministrativoLabel.Text = String.Empty

    '' ''    Me.curriculumincaricoAmministrativoUpload1.Visible = True
    '' ''    Me.curriculumincaricoAmministrativoUpload2.Visible = False

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub CurriculumIncaricoAmministrativoLinkButton_Click(sender As Object, e As System.EventArgs) Handles CurriculumIncaricoAmministrativoLinkButton.Click
    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
    '' ''    Dim filename As String = Me.CurriculumIncaricoAmministrativoLinkButton.Text
    '' ''    Dim filenameTemp As String = Me.NomeFileCurriculumIncaricoAmministrativoLabel.Text

    '' ''    If Not filename Is Nothing Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If Not String.IsNullOrEmpty(filenameTemp) Then
    '' ''            pathDownload = filenameTemp
    '' ''        Else
    '' ''            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
    '' ''            pathDownload = percorsoRoot & Me.IncaricoAmministrativoDirigenziale.path & filename
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("Il file allegato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiIncompatibilitaIncaricoAmministrativoImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiIncompatibilitaIncaricoAmministrativoImageButton.Click

    '' ''    If Me.incompatibilitaIncaricoAmministrativoUpload.UploadedFiles.Count = 0 Then
    '' ''        ParsecUtility.Utility.MessageBox("Per inserire la dichiarazione, è necessario specificarne il file relativo!", False)
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim file As Telerik.Web.UI.UploadedFile = Me.incompatibilitaIncaricoAmministrativoUpload.UploadedFiles(0)

    '' ''    If Not String.IsNullOrEmpty(file.FileName) Then

    '' ''        Dim maxKiloBytesLength As Integer = CInt(ParsecAdmin.WebConfigSettings.GetKey("MaxKiloBytesLengthAlbo"))

    '' ''        If file.ContentLength = 0 Then
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è vuoto!", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        If (file.ContentLength / 1024) > maxKiloBytesLength Then
    '' ''            Dim mb As Single = (file.ContentLength / 1024) / 1024
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è di " & mb.ToString("0.00") & " megabyte !" & vbCrLf & "Si può allegare un file di massimo 4 megabyte.", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathDownload As String = pathRootTemp & Session.SessionID & "_" & file.FileName
    '' ''        file.SaveAs(pathDownload)

    '' ''        Me.IncompatibilitaIncaricoAmministrativoLinkButton.Text = file.FileName
    '' ''        Me.NomeFileIncompatibilitaIncaricoAmministrativoLabel.Text = pathDownload

    '' ''        Me.incompatibilitaIncaricoAmministrativoUpload1.Visible = False
    '' ''        Me.incompatibilitaIncaricoAmministrativoUpload2.Visible = True
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub RimuoviIncompatibilitaIncaricoAmministrativoImageButton_Click(sender As Object, e As System.EventArgs) Handles RimuoviIncompatibilitaIncaricoAmministrativoImageButton.Click
    '' ''    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.NomeFileIncompatibilitaIncaricoAmministrativoLabel.Text
    '' ''    If IO.File.Exists(pathDownload) Then
    '' ''        IO.File.Delete(pathDownload)
    '' ''    End If
    '' ''    Me.IncompatibilitaIncaricoAmministrativoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileIncompatibilitaIncaricoAmministrativoLabel.Text = String.Empty

    '' ''    Me.incompatibilitaIncaricoAmministrativoUpload1.Visible = True
    '' ''    Me.incompatibilitaIncaricoAmministrativoUpload2.Visible = False

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub IncompatibilitaIncaricoAmministrativoLinkButton_Click(sender As Object, e As System.EventArgs) Handles IncompatibilitaIncaricoAmministrativoLinkButton.Click
    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
    '' ''    Dim filename As String = Me.IncompatibilitaIncaricoAmministrativoLinkButton.Text
    '' ''    Dim filenameTemp As String = Me.NomeFileIncompatibilitaIncaricoAmministrativoLabel.Text

    '' ''    If Not String.IsNullOrEmpty(filename) Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If Not String.IsNullOrEmpty(filenameTemp) Then
    '' ''            pathDownload = filenameTemp
    '' ''        Else
    '' ''            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
    '' ''            pathDownload = percorsoRoot & Me.IncaricoAmministrativoDirigenziale.path & filename
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("Il file allegato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    '**********************************************************************************************************************************************************************

    '**********************************************************************************************************************************************************************
    'CONSULENTE/COLLABORATORE
    '**********************************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AggiungiCurriculumImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiCurriculumImageButton.Click

    '' ''    If Me.CurriculumUpload.UploadedFiles.Count = 0 Then
    '' ''        ParsecUtility.Utility.MessageBox("Per inserire il curriculum, è necessario specificarne il file relativo!", False)
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim file As Telerik.Web.UI.UploadedFile = Me.CurriculumUpload.UploadedFiles(0)

    '' ''    If Not String.IsNullOrEmpty(file.FileName) Then

    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathDownload As String = pathRootTemp & Session.SessionID & "_" & file.FileName
    '' ''        file.SaveAs(pathDownload)

    '' ''        Me.CurriculumAllegatoLinkButton.Text = file.FileName
    '' ''        Me.NomeFileCurriculumLabel.Text = pathDownload

    '' ''        Me.curriculumUpload1.Visible = False
    '' ''        Me.curriculumUpload2.Visible = True
    '' ''    End If


    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub RimuoviCurriculumImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles RimuoviCurriculumImageButton.Click
    '' ''    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & NomeFileCurriculumLabel.Text
    '' ''    If IO.File.Exists(pathDownload) Then
    '' ''        IO.File.Delete(pathDownload)
    '' ''    End If
    '' ''    Me.CurriculumAllegatoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileCurriculumLabel.Text = String.Empty

    '' ''    Me.curriculumUpload1.Visible = True
    '' ''    Me.curriculumUpload2.Visible = False
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub CurriculumAllegatoLinkButton_Click(sender As Object, e As System.EventArgs) Handles CurriculumAllegatoLinkButton.Click
    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
    '' ''    Dim filename As String = Me.CurriculumAllegatoLinkButton.Text
    '' ''    Dim filenameTemp As String = Me.NomeFileCurriculumLabel.Text

    '' ''    If Not String.IsNullOrEmpty(filename) Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If Not String.IsNullOrEmpty(filenameTemp) Then
    '' ''            pathDownload = filenameTemp
    '' ''        Else
    '' ''            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
    '' ''            pathDownload = percorsoRoot & Me.Consulenza.Path & filename
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("Il curriculum allegato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub


    'Attestazione dell'avvenuta verifica dell'insussistenza di situazioni, anche potenziali, di conflitto di interesse
    'luca 01/07/2020
    '' ''Protected Sub AggiungiInconsistenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiInconsistenzaImageButton.Click

    '' ''    If Me.InconsistenzaUpload.UploadedFiles.Count = 0 Then
    '' ''        ParsecUtility.Utility.MessageBox("Per inserire la dichiarazione, è necessario specificarne il file relativo!", False)
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim file As Telerik.Web.UI.UploadedFile = Me.InconsistenzaUpload.UploadedFiles(0)

    '' ''    If Not String.IsNullOrEmpty(file.FileName) Then

    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathDownload As String = pathRootTemp & Session.SessionID & "_" & file.FileName
    '' ''        file.SaveAs(pathDownload)

    '' ''        Me.InconsistenzaAllegatoLinkButton.Text = file.FileName
    '' ''        Me.NomeFileInsussistenzaLabel.Text = pathDownload

    '' ''        Me.InconsistenzaUpload1.Visible = False
    '' ''        Me.InconsistenzaUpload2.Visible = True
    '' ''    End If

    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub RimuoviInconsistenzaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles RimuoviInconsistenzaImageButton.Click
    '' ''    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.NomeFileInsussistenzaLabel.Text
    '' ''    If IO.File.Exists(pathDownload) Then
    '' ''        IO.File.Delete(pathDownload)
    '' ''    End If
    '' ''    Me.InconsistenzaAllegatoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileInsussistenzaLabel.Text = String.Empty

    '' ''    Me.InconsistenzaUpload1.Visible = True
    '' ''    Me.InconsistenzaUpload2.Visible = False
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub InconsistenzaAllegatoLinkButton_Click(sender As Object, e As System.EventArgs) Handles InconsistenzaAllegatoLinkButton.Click

    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
    '' ''    Dim filename As String = Me.InconsistenzaAllegatoLinkButton.Text
    '' ''    Dim filenameTemp As String = Me.NomeFileInsussistenzaLabel.Text

    '' ''    If Not String.IsNullOrEmpty(filename) Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If Not String.IsNullOrEmpty(filenameTemp) Then
    '' ''            pathDownload = filenameTemp
    '' ''        Else
    '' ''            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
    '' ''            pathDownload = percorsoRoot & Me.Consulenza.Path & filename
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("Il progetto allegato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    '**********************************************************************************************************************************************************************


    '**********************************************************************************************************************************************************************
    'ENTE CONTROLLATO
    '**********************************************************************************************************************************************************************
    'luca 01/07/2020
    '' ''Protected Sub AggiungiInconferibilitaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiInconferibilitaImageButton.Click

    '' ''    If Me.InconferibilitaUpload.UploadedFiles.Count = 0 Then
    '' ''        ParsecUtility.Utility.MessageBox("Per inserire la dichiarazione, è necessario specificarne il file relativo!", False)
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim file As Telerik.Web.UI.UploadedFile = Me.InconferibilitaUpload.UploadedFiles(0)

    '' ''    If Not String.IsNullOrEmpty(file.FileName) Then

    '' ''        Dim maxKiloBytesLength As Integer = CInt(ParsecAdmin.WebConfigSettings.GetKey("MaxKiloBytesLengthAlbo"))

    '' ''        If file.ContentLength = 0 Then
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è vuoto!", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        If (file.ContentLength / 1024) > maxKiloBytesLength Then
    '' ''            Dim mb As Single = (file.ContentLength / 1024) / 1024
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è di " & mb.ToString("0.00") & " megabyte !" & vbCrLf & "Si può allegare un file di massimo 4 megabyte.", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathDownload As String = pathRootTemp & Session.SessionID & "_" & file.FileName

    '' ''        file.SaveAs(pathDownload)

    '' ''        Me.InconferibilitaAllegatoLinkButton.Text = file.FileName
    '' ''        Me.NomeFileInconferibilitaLabel.Text = pathDownload

    '' ''        Me.inconferibilitaUpload1.Visible = False
    '' ''        Me.InconferibilitaUpload2.Visible = True
    '' ''    End If


    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub RimuoviInconferibilitaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles RimuoviInconferibilitaImageButton.Click
    '' ''    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.NomeFileInconferibilitaLabel.Text
    '' ''    If IO.File.Exists(pathDownload) Then
    '' ''        IO.File.Delete(pathDownload)
    '' ''    End If
    '' ''    Me.InconferibilitaAllegatoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileInconferibilitaLabel.Text = String.Empty

    '' ''    Me.inconferibilitaUpload1.Visible = True
    '' ''    Me.InconferibilitaUpload2.Visible = False
    '' ''End Sub

    '' ''Protected Sub InconferibilitaAllegatoLinkButton_Click(sender As Object, e As System.EventArgs) Handles InconferibilitaAllegatoLinkButton.Click
    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
    '' ''    Dim filename As String = Me.InconferibilitaAllegatoLinkButton.Text
    '' ''    Dim filenameTemp As String = Me.NomeFileInconferibilitaLabel.Text

    '' ''    If Not String.IsNullOrEmpty(filename) Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If Not String.IsNullOrEmpty(filenameTemp) Then
    '' ''            pathDownload = filenameTemp
    '' ''        Else
    '' ''            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
    '' ''            pathDownload = percorsoRoot & Me.EnteControllato.path & filename
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("Il file allegato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub AggiungiIncompatibilitaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiungiIncompatibilitaImageButton.Click

    '' ''    If Me.IncompatibilitaUpload.UploadedFiles.Count = 0 Then
    '' ''        ParsecUtility.Utility.MessageBox("Per inserire la dichiarazione, è necessario specificarne il file relativo!", False)
    '' ''        Exit Sub
    '' ''    End If

    '' ''    Dim file As Telerik.Web.UI.UploadedFile = Me.IncompatibilitaUpload.UploadedFiles(0)

    '' ''    If Not String.IsNullOrEmpty(file.FileName) Then

    '' ''        Dim maxKiloBytesLength As Integer = CInt(ParsecAdmin.WebConfigSettings.GetKey("MaxKiloBytesLengthAlbo"))

    '' ''        If file.ContentLength = 0 Then
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è vuoto!", False)
    '' ''            Exit Sub
    '' ''        End If

    '' ''        If (file.ContentLength / 1024) > maxKiloBytesLength Then
    '' ''            Dim mb As Single = (file.ContentLength / 1024) / 1024
    '' ''            ParsecUtility.Utility.MessageBox("L'allegato è di " & mb.ToString("0.00") & " megabyte !" & vbCrLf & "Si può allegare un file di massimo 4 megabyte.", False)
    '' ''            Exit Sub
    '' ''        End If


    '' ''        Dim pathRootTemp As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp")

    '' ''        If Not IO.Directory.Exists(pathRootTemp) Then
    '' ''            IO.Directory.CreateDirectory(pathRootTemp)
    '' ''        End If

    '' ''        Dim pathDownload As String = pathRootTemp & Session.SessionID & "_" & file.FileName
    '' ''        file.SaveAs(pathDownload)

    '' ''        Me.IncompatibilitaAllegatoLinkButton.Text = file.FileName
    '' ''        Me.NomeFileIncompatibilitaLabel.Text = pathDownload

    '' ''        Me.incompatibilitaUpload1.Visible = False
    '' ''        Me.incompatibilitaUpload2.Visible = True
    '' ''    End If


    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub RimuoviIncompatibilitaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles RimuoviIncompatibilitaImageButton.Click
    '' ''    Dim pathDownload As String = ParsecAdmin.WebConfigSettings.GetKey("PathDocumentiTemp") & Me.NomeFileIncompatibilitaLabel.Text
    '' ''    If IO.File.Exists(pathDownload) Then
    '' ''        IO.File.Delete(pathDownload)
    '' ''    End If
    '' ''    Me.IncompatibilitaAllegatoLinkButton.Text = String.Empty
    '' ''    Me.NomeFileIncompatibilitaLabel.Text = String.Empty

    '' ''    Me.incompatibilitaUpload1.Visible = True
    '' ''    Me.incompatibilitaUpload2.Visible = False
    '' ''End Sub

    'luca 01/07/2020
    '' ''Protected Sub IncompatibilitaAllegatoLinkButton_Click(sender As Object, e As System.EventArgs) Handles IncompatibilitaAllegatoLinkButton.Click
    '' ''    Dim percorsoRoot As String = ParsecAdmin.WebConfigSettings.GetKey("PathAttiPubblicazioni")
    '' ''    Dim filename As String = Me.IncompatibilitaAllegatoLinkButton.Text
    '' ''    Dim filenameTemp As String = Me.NomeFileIncompatibilitaLabel.Text

    '' ''    If Not String.IsNullOrEmpty(filename) Then
    '' ''        Dim pathDownload As String = String.Empty
    '' ''        'Se è un allegato temporaneo.
    '' ''        If Not String.IsNullOrEmpty(filenameTemp) Then
    '' ''            pathDownload = filenameTemp
    '' ''        Else
    '' ''            percorsoRoot = percorsoRoot.Remove(percorsoRoot.Length - 1, 1)
    '' ''            pathDownload = percorsoRoot & Me.EnteControllato.path & filename
    '' ''        End If
    '' ''        Dim file As New IO.FileInfo(pathDownload)
    '' ''        If file.Exists Then
    '' ''            Session("AttachmentFullName") = file.FullName
    '' ''            Dim pageUrl As String = "~/UI/Amministrazione/pages/user/DownloadPage.aspx"
    '' ''            ParsecUtility.Utility.PageReload(pageUrl, False)
    '' ''        Else
    '' ''            ParsecUtility.Utility.MessageBox("Il file allegato non esiste!", False)
    '' ''        End If
    '' ''    End If
    '' ''End Sub

    '**********************************************************************************************************************************************************************

#End Region

End Class