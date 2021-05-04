Imports Telerik.Web.UI


Partial Class LiquidazionePage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    Public Property Liquidazione() As ParsecAtt.Liquidazione
        Get
            Return CType(Session("LiquidazionePage_Liquidazione"), ParsecAtt.Liquidazione)
        End Get
        Set(ByVal value As ParsecAtt.Liquidazione)
            Session("LiquidazionePage_Liquidazione") = value
        End Set
    End Property

    Public Property CampiObbligatoriAggiuntivi() As Boolean
        Get
            Return CType(Session("LiquidazionePage_CampiObbligatoriAggiuntivi"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Session("LiquidazionePage_CampiObbligatoriAggiuntivi") = value
        End Set
    End Property

    Public Property ObbligatorietaImpegnoSubImpegnoLiquidazione() As Boolean
        Get
            Return CType(Session("LiquidazionePage_ObbligatorietaImpegnoSubImpegnoLiquidazione"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Session("LiquidazionePage_ObbligatorietaImpegnoSubImpegnoLiquidazione") = value
        End Set
    End Property

#End Region


#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then

            Dim parametri As New ParsecAdmin.ParametriRepository
            Dim parametro = parametri.GetByName("CampiObbligatoriAggiuntiviLiquidazione", ParsecAdmin.TipoModulo.ATT)

            Me.CampiObbligatoriAggiuntivi = False
            If Not parametro Is Nothing Then
                If parametro.Valore = "1" Then
                    Me.CampiObbligatoriAggiuntivi = True
                    Me.ImpostaEtichette()
                End If
            End If


            Me.ObbligatorietaImpegnoSubImpegnoLiquidazione = False
            parametro = parametri.GetByName("ObbligatorietaImpegnoSubImpegnoLiquidazione", ParsecAdmin.TipoModulo.ATT)
            parametri.Dispose()
            If Not parametro Is Nothing Then
                If parametro.Valore = "1" Then
                    Me.ObbligatorietaImpegnoSubImpegnoLiquidazione = True
                    Me.ImpostaEtichetteImpegnoSubImpegno()
                End If
            End If




            Me.CaricaAnni()

            Dim anteprima As Boolean = False
            If Not Me.Request.QueryString("preview") Is Nothing Then
                anteprima = CBool(Me.Request.QueryString("preview"))
            End If

            Dim copia As Boolean = False
            If Not Me.Request.QueryString("copia") Is Nothing Then
                copia = CBool(Me.Request.QueryString("copia"))
            End If

            If anteprima Then
                Me.Liquidazione = ParsecUtility.SessionManager.Liquidazione
                Me.AggiornaVista()
                Me.TitleLabel.Text = "Dettaglio Liquidazione"
                DisabilitaUI()
            Else
                Me.ChiudiButton.Visible = False
                If ParsecUtility.SessionManager.Liquidazione Is Nothing Then
                    Me.ResettaVista()
                    Me.TitleLabel.Text = "Nuova Liquidazione"
                Else
                    Me.Liquidazione = ParsecUtility.SessionManager.Liquidazione
                    Me.AggiornaVista()
                    If copia Then
                        Me.TitleLabel.Text = "Nuova Liquidazione"
                    Else
                        Me.TitleLabel.Text = "Modifica Liquidazione"
                    End If

                End If
            End If
            Me.GetParametri()

        End If

    End Sub

#End Region



#Region "METODI PRIVATI"

    Private Sub ImpostaEtichette()
        Me.ImpegnoLabel.Text = "Impegno *"
        Me.CodiceFiscalePartitaIvaLabel.Text = "C.F - P. IVA *"
        Me.BeneficiarioLabel.Text = "Beneficiario *"
        Me.CigLabel.Text = "CIG *"
        Me.CupLabel.Text = "CUP *"
        Me.NumeroDeterminaLabel.Text = "N. Det. *"
        Me.AnnoDeterminaLabel.Text = "Anno Det. *"
    End Sub


    Private Sub ImpostaEtichetteImpegnoSubImpegno()
        Me.ImpegnoLabel.Text = "Impegno *"
        Me.SubImpegnoLabel.Text = "Sub Impegno *"
    End Sub


    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("cig") Then
                Me.CigTextBox.Text = CStr(parametriPagina("cig"))
            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    Private Sub CaricaAnni()
        Dim anni As New ParsecAtt.AnnoRepository

        Dim elencoAnni = anni.GetQuery.OrderBy(Function(c) c.Valore).Select(Function(c) New With {.Valore = c.Valore})
        Me.AnniComboBox.DataValueField = "Valore"
        Me.AnniComboBox.DataTextField = "Valore"
        Me.AnniComboBox.DataSource = elencoAnni
        Me.AnniComboBox.DataBind()

        Me.AnniLiquidazioneComboBox.DataValueField = "Valore"
        Me.AnniLiquidazioneComboBox.DataTextField = "Valore"
        Me.AnniLiquidazioneComboBox.DataSource = elencoAnni
        Me.AnniLiquidazioneComboBox.DataBind()

        anni.Dispose()
    End Sub

    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder
        Dim messageErroreFormale As New StringBuilder

        'If String.IsNullOrEmpty(Me.ImportoTextBox.Text) Then
        '    message.AppendLine("L'importo impegnato.")
        'End If

        If Me.CampiObbligatoriAggiuntivi Then

            If String.IsNullOrEmpty(Me.ImpegnoTextBox.Text) Then
                message.AppendLine("Numero di impegno.")
            End If

            If String.IsNullOrEmpty(Me.CodiceFiscalePartitaIvaTextBox.Text) Then
                message.AppendLine("Codice fiscale/partita iva.")
            End If

            If String.IsNullOrEmpty(Me.BeneficiarioTextBox.Text) Then
                message.AppendLine("Beneficiario.")
            End If

            If String.IsNullOrEmpty(Me.CigTextBox.Text) Then
                message.AppendLine("Cig.")
            End If

            If String.IsNullOrEmpty(Me.CupTextBox.Text) Then
                message.AppendLine("Cup.")
            End If

            If Not Me.NumeroDeterminaTextBox.Value.HasValue Then
                message.AppendLine("Numero della determina.")
            End If
            If Not Me.AnnoDeterminaTextBox.Value.HasValue Then
                message.AppendLine("Anno della determina.")
            End If

        End If

        If Me.ObbligatorietaImpegnoSubImpegnoLiquidazione Then
            If String.IsNullOrEmpty(Me.ImpegnoTextBox.Text) Then
                message.AppendLine("Numero di impegno.")
            End If

            If String.IsNullOrEmpty(Me.SubImpegnoTextBox.Text) Then
                message.AppendLine("Numero di sub impegno.")
            End If

        End If


        If String.IsNullOrEmpty(Me.CapitoloTextBox.Text) Then
            message.AppendLine("Capitolo dell'impegno di spesa.")
        End If

        If String.IsNullOrEmpty(Me.ImportoLiquidazioneTextBox.Text) Then
            message.AppendLine("Importo liquidato.")
        End If

        If Not String.IsNullOrEmpty(Me.CodiceFiscalePartitaIvaTextBox.Text) Then
            Dim rgxPartitaIva As Regex = New Regex("^[0-9]{11}$")
            Dim rgxCodiceFiscale As Regex = New Regex("^[A-Z]{6}[\d+]{2}[ABCDEHLMPRST]{1}[\d+]{2}([A-Z]{1}[\d+]{3})[A-Z]{1}$")


            Dim matchCodiceFiscale As Match = rgxCodiceFiscale.Match(Me.CodiceFiscalePartitaIvaTextBox.Text.ToUpper)
            Dim matchPartitaIva As Match = rgxPartitaIva.Match(Me.CodiceFiscalePartitaIvaTextBox.Text)

            If Not matchCodiceFiscale.Success AndAlso Not matchPartitaIva.Success Then
                messageErroreFormale.AppendLine("Il dato fiscale (codice fiscale/partita iva).")
            End If


        End If

        'If Not String.IsNullOrEmpty(Me.IbanTextBox.Text) Then
        '    If Not Me.ChekIban(Me.IbanTextBox.Text) Then
        '        messageErroreFormale.AppendLine("IBAN.")

        '    End If
        'End If


        'CODICE IDENTIFICATIVO GARA
        'If Not String.IsNullOrEmpty(Me.CigTextBox.Text) Then
        '    Dim rgxCIG As Regex = New Regex("^[a-zA-Z0-9]{10}$")
        '    Dim matchCIG As Match = rgxCIG.Match(Me.CigTextBox.Text)
        '    If Not matchCIG.Success Then
        '        messageErroreFormale.AppendLine("CIG.")
        '    End If
        'End If


        'CODICE UNICO di PROGETTO
        'If Not String.IsNullOrEmpty(Me.CupTextBox.Text) Then
        '    Dim rgxCUP As Regex = New Regex("^[a-zA-Z0-9]{15}$")
        '    Dim matchCUP As Match = rgxCUP.Match(Me.CupTextBox.Text)
        '    If Not matchCUP.Success Then
        '        messageErroreFormale.AppendLine("CUP.")
        '    End If
        'End If


        'CODICE IDENTIFICATIVO PRATICA (PRESENTE NEL DOCUMENTO DURC)
        If Not String.IsNullOrEmpty(Me.DurcTextBox.Text) Then
            Dim rgxCIP As Regex = New Regex("^[0-9]{14}$")
            Dim matchCIP As Match = rgxCIP.Match(Me.DurcTextBox.Text)
            If Not matchCIP.Success Then
                messageErroreFormale.AppendLine("Codice identificativo pratica del DURC.")
            End If
        End If



        Dim m As New StringBuilder

        If message.Length > 0 Then
            message.Insert(0, "I seguenti campi sono obbligatori:" & vbCrLf)
            m.Append(message.ToString)
        End If

        If messageErroreFormale.Length > 0 Then
            m.AppendLine("")
            m.AppendLine("I sequenti campi non sono formalmente corretti:")
            m.Append(messageErroreFormale.ToString)
        End If
        If m.Length > 0 Then
            ParsecUtility.Utility.MessageBox(m.ToString, False)
        End If


        Return m.Length = 0
    End Function

    Private Function ChekIban(iban As String) As Boolean
        iban = iban.ToUpper
        If iban.Length <> 27 Then
            Return False
        End If

        If Not Regex.IsMatch(iban, "^[A-Z 0-9]") Then
            Return False
        End If

        iban = iban.Replace(" ", String.Empty)
        Dim iban2 = iban.Substring(4, iban.Length - 4) + iban.Substring(0, 4)

        Const asciiShift As Integer = 55
        Dim sb As New StringBuilder()

        For Each c In iban2
            Dim x As Integer
            If Char.IsLetter(c) Then
                x = AscW(c) - asciiShift
            Else
                x = Integer.Parse(c.ToString())
            End If
            sb.Append(x)
        Next
        Dim checkSumString = sb.ToString()
        Dim checksum As Integer = Integer.Parse(checkSumString.Substring(0, 1))
        For i = 1 To checkSumString.Length - 1
            Dim v As Integer = Integer.Parse(checkSumString.Substring(i, 1))
            checksum *= 10
            checksum += v
            checksum = checksum Mod 97
        Next
        Return (checksum = 1)
    End Function

    Private Sub ResettaVista()

        '******************************************************************************************
        'Dati Impegno 10
        '******************************************************************************************

        Me.ImportoTextBox.Text = String.Empty
        Me.AnniComboBox.Items.FindItemByValue(Now.Year).Selected = True
        Me.CapitoloTextBox.Text = String.Empty
        Me.ArticoloTextBox.Text = String.Empty
        Me.ImpegnoTextBox.Text = String.Empty
        Me.SubImpegnoTextBox.Text = String.Empty

        '********************************************************************************************************
        'D. Lgs. 118/2011
        '********************************************************************************************************
        Me.MissioneTextBox.Text = String.Empty
        Me.ProgrammaTextBox.Text = String.Empty
        Me.TitoloTextBox.Text = String.Empty
        Me.MacroAggregatoTextBox.Text = String.Empty
        Me.DescrizionePianoPrecedenteLabel.Text = String.Empty
        '********************************************************************************************************

        Me.NumeroDeterminaTextBox.Text = String.Empty
        Me.AnnoDeterminaTextBox.Text = String.Empty


        '******************************************************************************************
        'Dati Liquidazione  16
        '******************************************************************************************

        Me.AnniLiquidazioneComboBox.Items.FindItemByValue(Now.Year).Selected = True
        Me.LiquidazioneTextBox.Text = String.Empty
        Me.MandatoTextBox.Text = String.Empty
        Me.ImportoLiquidazioneTextBox.Text = String.Empty
        Me.DisponibilitaTextBox.Text = String.Empty
        Me.ModalitaPagamentoTextBox.Text = String.Empty
        Me.CodiceFiscalePartitaIvaTextBox.Text = String.Empty
        Me.IbanTextBox.Text = String.Empty
        Me.CigTextBox.Text = String.Empty
        Me.CupTextBox.Text = String.Empty
        Me.DurcTextBox.Text = String.Empty
        Me.DescrizioneTextBox.Text = String.Empty
        Me.BeneficiarioTextBox.Text = String.Empty
        Me.QuietanzanteTextBox.Text = String.Empty
        Me.NormaTextBox.Text = String.Empty
        Me.ModalitaTextBox.Text = String.Empty
        Me.Liquidazione = Nothing
    End Sub

    Private Sub DisabilitaUI()

        Me.ChiudiButton.Attributes.Add("onclick", "window.close()")

        Me.TrovaImpegnoSpesaImageButton.Visible = False
        Me.TrovaBeneficiarioImageButton.Visible = False

        '******************************************************************************************
        'Dati Impegno 10
        '******************************************************************************************

        Me.ImportoTextBox.Enabled = False
        Me.AnniComboBox.Enabled = False

        Me.CapitoloTextBox.Enabled = False
        Me.ArticoloTextBox.Enabled = False
        Me.ImpegnoTextBox.Enabled = False
        Me.SubImpegnoTextBox.Enabled = False

        Me.TitoloTextBox.Enabled = False
        Me.MissioneTextBox.Enabled = False
        Me.ProgrammaTextBox.Enabled = False
        Me.MacroAggregatoTextBox.Enabled = False

        Me.NumeroDeterminaTextBox.Enabled = False
        Me.AnnoDeterminaTextBox.Enabled = False


        '******************************************************************************************
        'Dati Liquidazione  16
        '******************************************************************************************

        Me.AnniLiquidazioneComboBox.Enabled = False
        Me.LiquidazioneTextBox.Enabled = False
        Me.MandatoTextBox.Enabled = False
        Me.ImportoLiquidazioneTextBox.Enabled = False
        Me.DisponibilitaTextBox.Enabled = False
        Me.ModalitaPagamentoTextBox.Enabled = False
        Me.CodiceFiscalePartitaIvaTextBox.Enabled = False
        Me.IbanTextBox.Enabled = False
        Me.CigTextBox.Enabled = False
        Me.CupTextBox.Enabled = False
        Me.DurcTextBox.Enabled = False
        Me.DescrizioneTextBox.Enabled = False
        Me.BeneficiarioTextBox.Enabled = False
        Me.QuietanzanteTextBox.Enabled = False
        Me.NormaTextBox.Enabled = False
        Me.ModalitaTextBox.Enabled = False
        Me.SalvaButton.Visible = False
        Me.AnnullaButton.Visible = False
    End Sub

    Private Sub AggiornaVista()

        Dim liquidazioni As New ParsecAtt.LiquidazioneRepository
        Dim documenti As New ParsecAtt.DocumentoRepository(liquidazioni.Context)

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("LiquidazioneAutomatica", ParsecAdmin.TipoModulo.ATT)
        Dim liquidazioneAutomatica As Boolean = False
        If Not parametro Is Nothing Then
            liquidazioneAutomatica = CBool(parametro.Valore)
        End If
        parametri.Dispose()

        Dim idImpegno As Integer = Me.Liquidazione.IdImpegno

        Dim numeroImpegno As String = String.Empty

        If Not String.IsNullOrEmpty(Me.Liquidazione.NumeroImpegno) Then
            numeroImpegno = Me.Liquidazione.NumeroImpegno
        End If
        Dim annoImpegno As Integer = Me.Liquidazione.AnnoImpegno

        If idImpegno = 0 Then


            If liquidazioneAutomatica Then



                idImpegno = (From l In liquidazioni.GetQuery
                          Join d In documenti.GetQuery
                          On l.IdDocumento Equals d.Id
                          Where d.IdTipologiaDocumento = 2 And l.AnnoEsercizio = annoImpegno And l.NumeroImpegno = numeroImpegno And (d.LogStato Is Nothing Or d.LogStato = "A")).Select(Function(c) c.l.IdImpegno).FirstOrDefault


            End If


        Else

            'Lb_Rif.Visible = True
            'Lb_Rif.Text = Liquidazione.CaricaRiferimento(.IdImpegno)
            'IdDocumento.Text = Liquidazione.CaricaRiferimentoDoc(.IdImpegno)
        End If

        '******************************************************************************************
        'Dati Impegno 10
        '******************************************************************************************

        '  Me.ImportoTextBox.Text = String.Format("{0:N2}", Me.Liquidazione.ImportoImpegno)  '1.111,00

        Me.ImportoTextBox.Value = Me.Liquidazione.ImportoImpegno


        Me.AnniComboBox.Items.FindItemByValue(Me.Liquidazione.AnnoImpegno).Selected = True
        Me.CapitoloTextBox.Text = Me.Liquidazione.Capitolo.ToString

        If Me.Liquidazione.Articolo.HasValue Then
            Me.ArticoloTextBox.Text = Me.Liquidazione.Articolo.ToString
        End If
        If Not String.IsNullOrEmpty(Me.Liquidazione.NumeroImpegno) Then
            Me.ImpegnoTextBox.Text = Me.Liquidazione.NumeroImpegno
        End If

        If Not String.IsNullOrEmpty(Me.Liquidazione.NumeroSubImpegno) Then
            Me.SubImpegnoTextBox.Text = Me.Liquidazione.NumeroSubImpegno
        End If


        If Me.Liquidazione.NumeroDetermina.HasValue Then
            Me.NumeroDeterminaTextBox.Text = Me.Liquidazione.NumeroDetermina
        End If

        If Me.Liquidazione.AnnoDetermina.HasValue Then
            Me.AnnoDeterminaTextBox.Text = Me.Liquidazione.AnnoDetermina
        End If


        '********************************************************************************************************
        'D. Lgs. 118/2011
        '********************************************************************************************************
        If Not String.IsNullOrEmpty(Me.Liquidazione.Missione) Then
            Me.MissioneTextBox.Text = Me.Liquidazione.Missione
        End If

        If Not String.IsNullOrEmpty(Me.Liquidazione.Programma) Then
            Me.ProgrammaTextBox.Text = Me.Liquidazione.Programma
        End If

        If Not String.IsNullOrEmpty(Me.Liquidazione.Titolo) Then
            Me.TitoloTextBox.Text = Me.Liquidazione.Titolo
        End If

        If Not String.IsNullOrEmpty(Me.Liquidazione.MacroAggregato) Then
            Me.MacroAggregatoTextBox.Text = Me.Liquidazione.MacroAggregato
        End If

        Dim desc As New List(Of String)
        If Not String.IsNullOrEmpty(Me.Liquidazione.Funzione) Then
            desc.Add("Funzione: " & Me.Liquidazione.Funzione)
        End If

        If Not String.IsNullOrEmpty(Me.Liquidazione.Servizio) Then
            desc.Add("Servizio: " & Me.Liquidazione.Funzione)
        End If

        If Not String.IsNullOrEmpty(Me.Liquidazione.Intervento) Then
            desc.Add("Intervento: " & Me.Liquidazione.Funzione)
        End If

        Me.DescrizionePianoPrecedenteLabel.Text = String.Join("  -   ", desc.ToArray)
        '********************************************************************************************************

        '******************************************************************************************


        '******************************************************************************************
        'Dati Liquidazione 16
        '******************************************************************************************

        Me.AnniLiquidazioneComboBox.Items.FindItemByValue(Me.Liquidazione.AnnoEsercizio).Selected = True
        If Me.Liquidazione.Numero > 0 Then
            Me.LiquidazioneTextBox.Text = Me.Liquidazione.Numero
        End If

        Me.MandatoTextBox.Text = Me.Liquidazione.Mandato

        'Me.ImportoLiquidazioneTextBox.Text = String.Format("{0:N2}", Me.Liquidazione.ImportoLiquidato).Replace(",", ".")

        Me.ImportoLiquidazioneTextBox.Value = Me.Liquidazione.ImportoLiquidato

        If liquidazioneAutomatica Then

            Dim totaleImportoLiquidato = (From l In liquidazioni.GetQuery
                        Join d In documenti.GetQuery
                        On l.IdDocumento Equals d.Id
                        Where d.IdTipologiaDocumento = 2 And l.IdImpegno = idImpegno And (d.LogStato Is Nothing Or d.LogStato = "A")).Select(Function(c) c.l.ImportoLiquidato).Sum(Function(c) CType(c, Nullable(Of Decimal)))

            If totaleImportoLiquidato.HasValue Then
                'Me.DisponibilitaTextBox.Text = String.Format("{0:N2}", Me.Liquidazione.ImportoImpegno - totaleImportoLiquidato).Replace(",", ".")

                Me.DisponibilitaTextBox.Text = (Me.Liquidazione.ImportoImpegno - totaleImportoLiquidato)
            End If


        End If


        Me.ModalitaPagamentoTextBox.Text = Me.Liquidazione.ModalitaPagamento
        Me.CodiceFiscalePartitaIvaTextBox.Text = Me.Liquidazione.CFPIVA
        Me.IbanTextBox.Text = Me.Liquidazione.IBAN
        Me.CigTextBox.Text = Me.Liquidazione.CIG
        Me.CupTextBox.Text = Me.Liquidazione.CUP
        Me.DurcTextBox.Text = Me.Liquidazione.DURC
        Me.DescrizioneTextBox.Text = Me.Liquidazione.Note
        Me.BeneficiarioTextBox.Text = Me.Liquidazione.Nominativo
        Me.QuietanzanteTextBox.Text = Me.Liquidazione.Quietanzante
        Me.NormaTextBox.Text = Me.Liquidazione.Norma
        Me.ModalitaTextBox.Text = Me.Liquidazione.Modalita

        '******************************************************************************************

        ParsecUtility.SessionManager.Liquidazione = Nothing

        liquidazioni.Dispose()
    End Sub

#End Region



#Region "EVENTI CONTROLLI"

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        If Me.VerificaDati() Then



            Dim liquidazione As ParsecAtt.Liquidazione = Nothing
            If Me.Liquidazione Is Nothing Then
                liquidazione = New ParsecAtt.Liquidazione
                liquidazione.Guid = Guid.NewGuid.ToString
            Else

                If Not Me.Request.QueryString("copia") Is Nothing Then
                    liquidazione = New ParsecAtt.Liquidazione
                    liquidazione.Guid = Guid.NewGuid.ToString
                Else
                    liquidazione = Me.Liquidazione
                End If

            End If

            '******************************************************************************************
            'Dati Impegno
            '******************************************************************************************

            liquidazione.ImportoImpegno = 0D
            If Not String.IsNullOrEmpty(Me.ImportoTextBox.Text) Then
                liquidazione.ImportoImpegno = Me.ImportoTextBox.Value
            End If


            liquidazione.Capitolo = CLng(Me.CapitoloTextBox.Text)

            liquidazione.AnnoImpegno = CInt(Me.AnniComboBox.SelectedItem.Text)


            liquidazione.Articolo = Me.ArticoloTextBox.Value
            liquidazione.NumeroImpegno = Me.ImpegnoTextBox.Text
            liquidazione.NumeroSubImpegno = Me.SubImpegnoTextBox.Text

            '********************************************************************************************************
            'D. Lgs. 118/2011
            '********************************************************************************************************
            liquidazione.Missione = Nothing
            liquidazione.Programma = Nothing
            liquidazione.Titolo = Nothing
            liquidazione.MacroAggregato = Nothing

            If Not String.IsNullOrEmpty(Me.MissioneTextBox.Text) Then
                liquidazione.Missione = Me.MissioneTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.ProgrammaTextBox.Text) Then
                liquidazione.Programma = Me.ProgrammaTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.TitoloTextBox.Text) Then
                liquidazione.Titolo = Me.TitoloTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.MacroAggregatoTextBox.Text) Then
                liquidazione.MacroAggregato = Me.MacroAggregatoTextBox.TextWithLiterals
            End If

            liquidazione.Servizio = Nothing
            liquidazione.Funzione = Nothing
            liquidazione.Intervento = Nothing
            '********************************************************************************************************

            '******************************************************************************************


            '******************************************************************************************
            'Dati Liquidazione
            '******************************************************************************************
            liquidazione.AnnoEsercizio = Me.AnniLiquidazioneComboBox.SelectedValue

            If Not String.IsNullOrEmpty(Me.LiquidazioneTextBox.Text) Then
                liquidazione.Numero = CInt(Me.LiquidazioneTextBox.Text)
            Else
                liquidazione.Numero = 0
            End If


            liquidazione.Mandato = Me.MandatoTextBox.Text.Trim


            If Not String.IsNullOrEmpty(Me.ImportoLiquidazioneTextBox.Text) Then
                liquidazione.ImportoLiquidato = Me.ImportoLiquidazioneTextBox.Value
            End If

            If Not String.IsNullOrEmpty(Me.ModalitaPagamentoTextBox.Text) Then
                liquidazione.ModalitaPagamento = Me.ModalitaPagamentoTextBox.Text.Trim
            End If

            If Not String.IsNullOrEmpty(Me.CodiceFiscalePartitaIvaTextBox.Text) Then
                liquidazione.CFPIVA = Me.CodiceFiscalePartitaIvaTextBox.Text
            End If


            liquidazione.IBAN = Me.IbanTextBox.Text
            liquidazione.CIG = Me.CigTextBox.Text
            liquidazione.CUP = Me.CupTextBox.Text
            liquidazione.DURC = Me.DurcTextBox.Text


            If Not String.IsNullOrEmpty(Me.DescrizioneTextBox.Text) Then
                liquidazione.Note = Me.DescrizioneTextBox.Text.Trim
            End If

            If Not String.IsNullOrEmpty(Me.BeneficiarioTextBox.Text) Then
                liquidazione.Nominativo = Me.BeneficiarioTextBox.Text.Trim
            End If


            liquidazione.Quietanzante = Me.QuietanzanteTextBox.Text.Trim


            If Not String.IsNullOrEmpty(Me.NormaTextBox.Text) Then
                liquidazione.Norma = Me.NormaTextBox.Text.Trim
            End If

            If Not String.IsNullOrEmpty(Me.ModalitaTextBox.Text) Then
                liquidazione.Modalita = Me.ModalitaTextBox.Text.Trim
            End If

            If Me.NumeroDeterminaTextBox.Value.HasValue Then
                liquidazione.NumeroDetermina = Me.NumeroDeterminaTextBox.Value
            End If

            If Me.AnnoDeterminaTextBox.Value.HasValue Then
                liquidazione.AnnoDetermina = Me.AnnoDeterminaTextBox.Value
            End If

            'liquidazione.Disponibilita = Me.DisponibilitaTextBox.Text


            ParsecUtility.SessionManager.Liquidazione = liquidazione
            ParsecUtility.Utility.ClosePopup(True)
        End If

    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click

        If Me.TitleLabel.Text = "Modifica Liquidazione" Then
            Me.AggiornaVista()
        Else
            Me.ResettaVista()
        End If


    End Sub

#End Region

    Protected Sub TrovaImpegnoSpesaImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaImpegnoSpesaImageButton.Click
        Dim pageUrl As String = "~/UI/AttiDecisionali/pages/search/RicercaImpegnoSpesaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaImpegnoSpesaImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 500, queryString, False)
    End Sub



    Protected Sub AggiornaImpegnoSpesaImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaImpegnoSpesaImageButton.Click
        If Not ParsecUtility.SessionManager.ImpegnoSpesa Is Nothing Then
            Dim impegno As ParsecAtt.ImpegnoSpesa = ParsecUtility.SessionManager.ImpegnoSpesa
            ParsecUtility.SessionManager.ImpegnoSpesa = Nothing
            If impegno.Capitolo.HasValue Then
                Me.CapitoloTextBox.Text = impegno.Capitolo
            End If
            If impegno.Articolo.HasValue Then
                Me.ArticoloTextBox.Text = impegno.Articolo
            End If
            If Not String.IsNullOrEmpty(impegno.NumeroImpegno) Then
                Me.ImpegnoTextBox.Text = impegno.NumeroImpegno
            End If
            If Not String.IsNullOrEmpty(impegno.NumeroSubImpegno) Then
                Me.SubImpegnoTextBox.Text = impegno.NumeroSubImpegno
            End If
            If impegno.Importo.HasValue Then
                Me.ImportoTextBox.Value = impegno.Importo
            End If
            If Not String.IsNullOrEmpty(impegno.Oggetto) Then
                Me.DescrizioneTextBox.Text = impegno.Oggetto
            End If

            ' Me.AnniComboBox.Items.FindItemByValue(impegno.AnnoEsercizio).Selected = True

            Me.NumeroDeterminaTextBox.Text = impegno.ContatoreGenerale
            Me.AnnoDeterminaTextBox.Text = impegno.DataDocumento.Year.ToString


        End If
    End Sub

  

    Protected Sub TrovaBeneficiarioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaBeneficiarioImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/user/RubricaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaBeneficiarioImageButton.ClientID)
        queryString.Add("mode", "search")

        Dim parametriPagina As New Hashtable
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina

        If Not String.IsNullOrEmpty(Me.BeneficiarioTextBox.Text) Then
            parametriPagina.Add("Filtro", Me.BeneficiarioTextBox.Text)
            Dim rubrica As New ParsecAdmin.RubricaRepository
            Dim struttureEsterne = rubrica.GetQuery.Where(Function(c) c.Denominazione.Contains(Me.BeneficiarioTextBox.Text) And c.LogStato Is Nothing And c.InRubrica = True).ToList
            If struttureEsterne.Count = 1 Then
                Me.AggiornaReferenteEsterno(struttureEsterne(0))
                ParsecUtility.SessionManager.ParametriPagina = Nothing
            Else
                ParsecUtility.Utility.ShowPopup(pageUrl, 930, 730, queryString, False)
            End If
            rubrica.Dispose()
        Else
            ParsecUtility.Utility.ShowPopup(pageUrl, 930, 730, queryString, False)
        End If
    End Sub

    Private Sub AggiornaReferenteEsterno(ByVal strutturaEsterna As ParsecAdmin.StrutturaEsternaInfo)
        Me.BeneficiarioTextBox.Text = strutturaEsterna.Denominazione
        Me.IbanTextBox.Text = strutturaEsterna.IBAN
        If Not String.IsNullOrEmpty(strutturaEsterna.CodiceFiscale) Then
            Me.CodiceFiscalePartitaIvaTextBox.Text = strutturaEsterna.CodiceFiscale
        Else
            Me.CodiceFiscalePartitaIvaTextBox.Text = strutturaEsterna.PartitaIVA
        End If
    End Sub

    Protected Sub AggiornaBeneficiarioImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaBeneficiarioImageButton.Click

        If Not ParsecUtility.SessionManager.Rubrica Is Nothing Then
            Dim r As ParsecAdmin.StrutturaEsternaInfo = ParsecUtility.SessionManager.Rubrica

            Me.AggiornaReferenteEsterno(r)
           
            ParsecUtility.SessionManager.Rubrica = Nothing

        End If
        'If Not ParsecUtility.SessionManager.Liquidazione Is Nothing Then
        '    Dim liquidazione As ParsecAtt.Liquidazione = ParsecUtility.SessionManager.Liquidazione
        '    Me.BeneficiarioTextBox.Text = liquidazione.Nominativo
        '    Me.IbanTextBox.Text = liquidazione.IBAN
        '    Me.CodiceFiscalePartitaIvaTextBox.Text = liquidazione.CFPIVA
        '    ParsecUtility.SessionManager.Liquidazione = Nothing
        'End If
    End Sub
End Class
