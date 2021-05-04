Imports Telerik.Web.UI

Imports System
Imports System.Runtime.InteropServices
Imports System.Diagnostics
Imports System.Security.Principal
Imports System.Web.Services

Partial Class ImpegnoSpesaPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"


    Private Property ImpegnoSpesa() As ParsecAtt.ImpegnoSpesa
        Get
            Return CType(Session("ImpegnoSpesaPage_ImpegnoSpesa"), ParsecAtt.ImpegnoSpesa)
        End Get
        Set(ByVal value As ParsecAtt.ImpegnoSpesa)
            Session("ImpegnoSpesaPage_ImpegnoSpesa") = value
        End Set
    End Property

    Private Property ImpegniSpesa() As List(Of ParsecAtt.ImpegnoSpesa)
        Get
            Return CType(Session("ImpegnoSpesaPage_ImpegniSpesa"), List(Of ParsecAtt.ImpegnoSpesa))
        End Get
        Set(ByVal value As List(Of ParsecAtt.ImpegnoSpesa))
            Session("ImpegnoSpesaPage_ImpegniSpesa") = value
        End Set
    End Property

    Private Property TipologiaGestioneContabilita() As ParsecAtt.TipologiaGestioneContabilita
        Get
            Return CType(Session("ImpegnoSpesaPage_TipologiaGestioneContabilita"), ParsecAtt.TipologiaGestioneContabilita)
        End Get

        Set(ByVal value As ParsecAtt.TipologiaGestioneContabilita)
            Session("ImpegnoSpesaPage_TipologiaGestioneContabilita") = value
        End Set
    End Property


#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Me.Page.IsPostBack Then

            Me.ImpegniSpesa = New List(Of ParsecAtt.ImpegnoSpesa)

            Dim impegni = ParsecUtility.SessionManager.ParametriPagina("ElencoImpegniSpesa")

            For Each imp In impegni
                Me.ImpegniSpesa.Add(imp)
            Next

            ParsecUtility.SessionManager.ParametriPagina("ElencoImpegniSpesa") = Nothing

            Me.CaricaAnni()
            Me.SetTipologia()


            'DEDAGROUP
            If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then
                CacheFornitori = Nothing
                CacheFornitori = Me.GetFornitoriDedaGroup()
            End If


            Dim anteprima As Boolean = False

            If Not Me.Request.QueryString("preview") Is Nothing Then
                anteprima = CBool(Me.Request.QueryString("preview"))
            End If

            If anteprima Then

                Me.ImpegnoSpesa = ParsecUtility.SessionManager.ImpegnoSpesa
                Me.AggiornaVista()
                Me.TitleLabel.Text = "Dettaglio Impegno di Spesa"

                Me.DisabilitaUI()

            Else
                Me.ChiudiButton.Visible = False
                If ParsecUtility.SessionManager.ImpegnoSpesa Is Nothing Then
                    Me.ResettaVista()
                    Me.TitleLabel.Text = "Nuovo Impegno di Spesa"
                Else
                    Me.ImpegnoSpesa = ParsecUtility.SessionManager.ImpegnoSpesa
                    Me.AggiornaVista()
                    If Not Me.Request.QueryString("copia") Is Nothing Then
                        Me.TitleLabel.Text = "Nuovo Impegno di Spesa"
                    Else
                        Me.TitleLabel.Text = "Modifica Impegno di Spesa"

                        Select Case Me.TipologiaGestioneContabilita

                            Case ParsecAtt.TipologiaGestioneContabilita.DedaGroup
                                If Me.ImpegnoSpesa.IdImpegnoDedaGroup.HasValue Then
                                    Me.TrovaImpegnoJSibacImageButton.Visible = False
                                    Me.TrovaCapitoloImageButton.Visible = False

                                    Me.AbilitaModalitaDedaGroup(False)

                                End If
                        End Select

                    End If

                End If


                Select Case Me.TipologiaGestioneContabilita
                    Case ParsecAtt.TipologiaGestioneContabilita.Classica
                        Me.TrovaImpegnoJSibacImageButton.Visible = False
                        Me.FornitoriTable.Visible = False
                    Case ParsecAtt.TipologiaGestioneContabilita.JSibac
                        'JSIBAC
                        Me.TrovaImpegnoJSibacImageButton.Visible = True
                        Me.FornitoriTable.Visible = False
                    Case ParsecAtt.TipologiaGestioneContabilita.APK
                        Me.TrovaImpegnoJSibacImageButton.Visible = True
                        Me.FornitoriTable.Visible = False

                    Case ParsecAtt.TipologiaGestioneContabilita.DedaGroup
                        'DEDAGROUP
                        Me.FornitoriTable.Visible = True
                        'Me.TrovaImpegnoJSibacImageButton.Visible = True



                        'todo disabilitare 


                    Case Else
                        'TODO
                End Select
            End If

        End If
    End Sub


#End Region

#Region "METODI PRIVATI"

    Private Const ItemsPerRequest As Integer = 10
    Shared CacheFornitori As List(Of ParsecWebServices.FornitoreImpegnoModel) = Nothing

    Private Sub AggiornaImpegnoDedaGroup(ByVal impegno As ParsecWebServices.ImpegnoModel)

        Me.AbilitaModalitaDedaGroup(False)

        'HO CERCATO L'IMPEGNO
        Me.IdImpegnoDedaGroup.Value = impegno.IdImpegno

        Me.CapitoloTextBox.Text = impegno.NumeroCapitolo

        Me.ArticoloTextBox.Text = impegno.NumeroArticolo.ToString
        Me.DescrizioneTextBox.Text = impegno.Descrizione

        Me.MissioneTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.CodiceMissione) Then
            Me.MissioneTextBox.Text = impegno.CodiceMissione
        End If

        Me.ProgrammaTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.CodiceProgramma) Then
            Me.ProgrammaTextBox.Text = impegno.CodiceProgramma
        End If

        Me.MacroAggregatoTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.MacroAggregato) Then
            Me.MacroAggregatoTextBox.Text = impegno.MacroAggregato
        End If

        Me.TitoloTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.CodiceTitolo) Then
            Me.TitoloTextBox.Text = impegno.CodiceTitolo
        End If

        If Not String.IsNullOrEmpty(impegno.NumeroCapitolo) Then

            Try
                Dim postParameters As New Dictionary(Of String, Object)

                postParameters.Add("EntrataUscita", ParsecWebServices.Direzione.Uscita)
                postParameters.Add("Anno", impegno.AnnoImpegno)
                postParameters.Add("NumeroCapitolo", impegno.NumeroCapitolo)

                Dim endPoint As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupBaseUrlRagioneria")
                Dim scope As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupScopeRagioneria")
                Dim accessTokenUri As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupAccessTokenUriRagioneria")
                Dim clientId As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientIdRagioneria")
                Dim clientSecret As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientSecretRagioneria")

                Dim parameter As New ParsecWebServices.OAuth2Parameter With {.AccessTokenUri = accessTokenUri, .ClientId = clientId, .ClientSecret = clientSecret, .Scope = scope}
                Dim service As New ParsecWebServices.DedaGroupService(String.Format(endPoint, "CercaCapitoli"), parameter)

                Dim capitolo = service.QueryCapitoli(postParameters).FirstOrDefault

                If Not capitolo Is Nothing Then

                    Me.StanziamentoInizialeTextBox.Value = Nothing
                    Me.StanziamentoInizialeTextBox.Value = capitolo.StanziamentoIniziale

                    Me.StanziamentoAssestatoTextBox.Value = Nothing
                    Me.StanziamentoAssestatoTextBox.Value = capitolo.StanziamentoIniziale + capitolo.Variazioni
                    Me.DisponibilitaResidualeTextBox.Value = Nothing

                    Me.DisponibilitaResidualeTextBox.Value = capitolo.Disponibilita

                End If
            Catch ex As Exception
                ParsecUtility.Utility.MessageBox(ex.Message, False)
            End Try

        End If

        Me.ImpegnoTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.NumeroImpegno) Then
            Me.ImpegnoTextBox.Text = impegno.NumeroImpegno
        End If

        If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then  'DEDAGROUP

            If Not CacheFornitori Is Nothing Then
                Dim fornitore = CacheFornitori.Where(Function(c) c.IdFornitore = impegno.IdFornitore).FirstOrDefault
                If Not fornitore Is Nothing Then
                    Me.FornitoriComboBox.Text = fornitore.CognomeDenominazione
                    Me.FornitoriComboBox.SelectedValue = fornitore.IdFornitore
                    Me.FornitoriComboBox.Enabled = False
                End If
            End If

        End If

        Me.ImportoTextBox.Value = Nothing
        Me.ImportoTextBox.Value = impegno.Importo

    End Sub

    Private Sub AbilitaModalitaDedaGroup(abilita As Boolean)

        Me.MissioneTextBox.Enabled = abilita
        Me.ProgrammaTextBox.Enabled = abilita
        Me.TitoloTextBox.Enabled = abilita
        Me.MacroAggregatoTextBox.Enabled = abilita

        Me.CapitoloTextBox.Enabled = abilita
        Me.ArticoloTextBox.Enabled = abilita
        Me.StanziamentoInizialeTextBox.Enabled = abilita
        Me.StanziamentoAssestatoTextBox.Enabled = abilita
        Me.DisponibilitaResidualeTextBox.Enabled = abilita
        Me.DescrizioneTextBox.Enabled = True
        Me.ImpegnoTextBox.Enabled = abilita
    End Sub

    Private Function GetFiltro() As ParsecPostgres.FiltroImpegno
        Dim filtro As New ParsecPostgres.FiltroImpegno

        filtro.Tipologia = 2  ' 1=ENTRATA 2= SPESA
        filtro.AnnoEsercizio = Me.AnniComboBox.SelectedValue
        filtro.AnnoImpegno = Me.AnniComboBox.SelectedValue

        If Me.CapitoloTextBox.Value.HasValue Then
            filtro.NumeroCapitolo = Me.CapitoloTextBox.Value
        End If

        If Me.ArticoloTextBox.Value.HasValue Then
            filtro.Articolo = Me.ArticoloTextBox.Value
        End If
        filtro.AnnoEsercizioImpegno = CInt(Me.AnniComboBox.SelectedValue)

        Return filtro
    End Function


    Private Sub SetTipologia()
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
        Me.TipologiaGestioneContabilita = tipologia
    End Sub

    Private Sub DisabilitaUI()
        Me.ChiudiButton.Attributes.Add("onclick", "window.close()")

        Me.TrovaImpegnoJSibacImageButton.Visible = False
        Me.TrovaCapitoloImageButton.Visible = False
        Me.SalvaButton.Visible = False
        Me.AnnullaButton.Visible = False



        Me.DescrizioneTextBox.Enabled = False
        Me.StanziamentoInizialeTextBox.Enabled = False
        Me.ImportoTextBox.Enabled = False


        Me.AnniComboBox.Enabled = False



        Me.CapitoloTextBox.Enabled = False

        Me.ArticoloTextBox.Enabled = False
        Me.ImpegnoTextBox.Enabled = False
        Me.SubImpegnoTextBox.Enabled = False

        Me.NumeroAttoAssunzioneTextBox.Enabled = False
        Me.DisponibilitaResidualeTextBox.Enabled = False
        Me.StanziamentoAssestatoTextBox.Enabled = False
        Me.CigTextBox.Enabled = False

        '********************************************************************************************************
        'D. Lgs. 118/2011
        '********************************************************************************************************
        Me.MissioneTextBox.Enabled = False
        Me.ProgrammaTextBox.Enabled = False
        Me.TitoloTextBox.Enabled = False
        Me.MacroAggregatoTextBox.Enabled = False

        '********************************************************************************************************
    End Sub

    <WebMethod()>
    Public Shared Function GetFornitori(ByVal context As RadComboBoxContext) As RadComboBoxData

        Dim data = CacheFornitori.Where(Function(c) c.CognomeDenominazione.ToUpper.Contains(context.Text.ToUpper))

        Dim comboData As New RadComboBoxData()
        Dim itemOffset As Integer = context.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Count)

        comboData.EndOfItems = (endOffset = data.Count)
        Dim result As New List(Of RadComboBoxItemData) '(endOffset - itemOffset)
        For i As Integer = itemOffset To endOffset - 1
            Dim itemData As New RadComboBoxItemData()
            'Dim item = data.Skip(i).FirstOrDefault
            Dim item = data.ElementAt(i)

            itemData.Text = item.CognomeDenominazione

            itemData.Value = item.IdFornitore
            result.Add(itemData)
        Next
        comboData.Message = If(data.Count > 0, String.Format("Elementi trovati <b>{0}</b> su <b>{1}</b>", endOffset, data.Count), "Nessun elemento trovato")
        comboData.Items = result.ToArray()
        Return comboData
    End Function


    Private Function GetFornitoriDedaGroup() As List(Of ParsecWebServices.FornitoreImpegnoModel)
        Try
            Dim endPoint As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupBaseUrlRagioneria")
            Dim scope As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupScopeRagioneria")
            Dim accessTokenUri As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupAccessTokenUriRagioneria")
            Dim clientId As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientIdRagioneria")
            Dim clientSecret As String = ParsecAdmin.WebConfigSettings.GetKey("DedaGroupClientSecretRagioneria")
            Dim parameter As New ParsecWebServices.OAuth2Parameter With {.AccessTokenUri = accessTokenUri, .ClientId = clientId, .ClientSecret = clientSecret, .Scope = scope}
            Dim service As New ParsecWebServices.DedaGroupService(String.Format(endPoint, "CercaFornitori"), parameter)

            Dim postParametersList As New Dictionary(Of String, Object)
            postParametersList.Add("Nome", "")

            Dim fornitori = service.QueryFornitori(postParametersList).Where(Function(c) Not String.IsNullOrEmpty(Trim(c.CognomeDenominazione))).OrderBy(Function(c) c.CognomeDenominazione).ToList

            Return fornitori

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try

        Return Nothing
    End Function

    Private Sub CaricaAnni()
        Dim anni As New ParsecAtt.AnnoRepository
        Dim elencoAnni = anni.GetQuery.OrderBy(Function(c) c.Valore).Select(Function(c) New With {.Valore = c.Valore})
        Me.AnniComboBox.DataValueField = "Valore"
        Me.AnniComboBox.DataTextField = "Valore"
        Me.AnniComboBox.DataSource = elencoAnni
        Me.AnniComboBox.DataBind()
        anni.Dispose()
    End Sub

    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder

        'DEDAGROUP
        'If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then

        '    'SE HO SELEZIONATO IL CAPITOLO
        '    If Not String.IsNullOrEmpty(Me.IdCapitoloDedagroup.Value) Then
        '        If Me.FornitoriComboBox.SelectedIndex = 0 Then
        '            message.AppendLine("Il fornitore.")
        '        End If
        '    End If
        'End If

        If String.IsNullOrEmpty(Me.ImportoTextBox.Text) Then
            message.AppendLine("L'importo impegnato.")
        End If
        If String.IsNullOrEmpty(Me.DescrizioneTextBox.Text) Then
            message.AppendLine("La descrizione dell'impegno di spesa.")
        End If
        If String.IsNullOrEmpty(Me.CapitoloTextBox.Text) Then
            message.AppendLine("Il capitolo dell'impegno di spesa.")
        End If

        If message.Length > 0 Then
            message.Insert(0, "E' necessario specificare:" & vbCrLf)
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If

        Return message.Length = 0
    End Function

    Private Sub ResettaVista()

        Me.ImportoTextBox.Text = String.Empty
        Me.DescrizioneTextBox.Text = String.Empty
        Me.AnniComboBox.Items.FindItemByValue(Now.Year).Selected = True
        Me.CapitoloTextBox.Text = String.Empty
        Me.ArticoloTextBox.Text = String.Empty
        Me.ImpegnoTextBox.Text = String.Empty
        Me.SubImpegnoTextBox.Text = String.Empty

        Me.NumeroAttoAssunzioneTextBox.Text = String.Empty
        Me.DisponibilitaResidualeTextBox.Text = String.Empty
        Me.StanziamentoAssestatoTextBox.Text = String.Empty
        Me.StanziamentoInizialeTextBox.Text = String.Empty

        '********************************************************************************************************
        'D. Lgs. 118/2011
        '********************************************************************************************************
        Me.MissioneTextBox.Text = String.Empty
        Me.ProgrammaTextBox.Text = String.Empty
        Me.TitoloTextBox.Text = String.Empty
        Me.MacroAggregatoTextBox.Text = String.Empty ' "000000000000"

        Me.DescrizionePianoPrecedenteLabel.Text = String.Empty
        '********************************************************************************************************

        Me.CigTextBox.Text = String.Empty

        Me.IdCapitoloDedagroup.Value = String.Empty
        Me.IdImpegnoDedaGroup.Value = String.Empty


        Me.ImpegnoSpesa = Nothing

        Me.FornitoriComboBox.Enabled = True
        Me.FornitoriComboBox.Text = String.Empty
        Me.FornitoriComboBox.SelectedValue = String.Empty

        Me.AbilitaModalitaDedaGroup(True)

    End Sub

    Private Sub AggiornaVista()

        If Me.ImpegnoSpesa.Importo.HasValue Then
            Me.ImportoTextBox.Value = Me.ImpegnoSpesa.Importo
        End If

        Me.DescrizioneTextBox.Text = Me.ImpegnoSpesa.Note

        If Me.ImpegnoSpesa.AnnoEsercizio.HasValue Then
            Me.AnniComboBox.Items.FindItemByValue(Me.ImpegnoSpesa.AnnoEsercizio).Selected = True
        End If


        If Me.ImpegnoSpesa.Capitolo.HasValue Then
            Me.CapitoloTextBox.Text = Me.ImpegnoSpesa.Capitolo.ToString
        End If


        If Me.ImpegnoSpesa.Articolo.HasValue Then
            Me.ArticoloTextBox.Text = Me.ImpegnoSpesa.Articolo.ToString
        End If


        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.NumeroImpegno) Then
            Me.ImpegnoTextBox.Text = Me.ImpegnoSpesa.NumeroImpegno
        End If


        Me.SubImpegnoTextBox.Text = Me.ImpegnoSpesa.NumeroSubImpegno

        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.NumeroAttoAssunzioneOriginale) Then
            Me.NumeroAttoAssunzioneTextBox.Text = Me.ImpegnoSpesa.NumeroAttoAssunzioneOriginale
        End If

        If Me.ImpegnoSpesa.DisponibilitaResiduale.HasValue Then
            Me.DisponibilitaResidualeTextBox.Value = Me.ImpegnoSpesa.DisponibilitaResiduale
        End If

        If Me.ImpegnoSpesa.StanziamentoIniziale.HasValue Then
            Me.StanziamentoInizialeTextBox.Value = Me.ImpegnoSpesa.StanziamentoIniziale
        End If

        If Me.ImpegnoSpesa.StanziamentoAssestato.HasValue Then
            Me.StanziamentoAssestatoTextBox.Value = Me.ImpegnoSpesa.StanziamentoAssestato
        End If

        '********************************************************************************************************
        'D. Lgs. 118/2011
        '********************************************************************************************************
        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.Missione) Then
            Me.MissioneTextBox.Text = Me.ImpegnoSpesa.Missione
        End If

        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.Programma) Then
            Me.ProgrammaTextBox.Text = Me.ImpegnoSpesa.Programma
        End If

        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.Titolo) Then
            Me.TitoloTextBox.Text = Me.ImpegnoSpesa.Titolo
        End If

        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.MacroAggregato) Then
            Me.MacroAggregatoTextBox.Text = Me.ImpegnoSpesa.MacroAggregato
        End If

        Dim desc As New List(Of String)
        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.Funzione) Then
            desc.Add("Funzione: " & Me.ImpegnoSpesa.Funzione)
        End If

        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.Servizio) Then
            desc.Add("Servizio: " & Me.ImpegnoSpesa.Funzione)
        End If

        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.Intervento) Then
            desc.Add("Intervento: " & Me.ImpegnoSpesa.Funzione)
        End If

        Me.DescrizionePianoPrecedenteLabel.Text = String.Join("  -   ", desc.ToArray)
        '********************************************************************************************************

        If Not String.IsNullOrEmpty(Me.ImpegnoSpesa.CIG) Then
            Me.CigTextBox.Text = Me.ImpegnoSpesa.CIG
        End If

        If Me.ImpegnoSpesa.IdCapitoloDedaGroup.HasValue Then
            Me.IdCapitoloDedagroup.Value = Me.ImpegnoSpesa.IdCapitoloDedaGroup.Value.ToString
        End If

        If Me.ImpegnoSpesa.IdImpegnoDedaGroup.HasValue Then
            Me.IdImpegnoDedaGroup.Value = Me.ImpegnoSpesa.IdImpegnoDedaGroup.Value.ToString
        End If

        If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then  'DEDAGROUP

            If Me.ImpegnoSpesa.IdFornitoreDedaGroup.HasValue Then

                If Not CacheFornitori Is Nothing Then
                    Dim fornitore = CacheFornitori.Where(Function(c) c.IdFornitore = Me.ImpegnoSpesa.IdFornitoreDedaGroup.Value).FirstOrDefault
                    If Not fornitore Is Nothing Then

                        Me.FornitoriComboBox.Text = fornitore.CognomeDenominazione
                        Me.FornitoriComboBox.SelectedValue = fornitore.IdFornitore
                    End If
                End If


            End If

        End If

        ParsecUtility.SessionManager.ImpegnoSpesa = Nothing
    End Sub

#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click

        If Me.VerificaDati() Then

            Dim impegno As ParsecAtt.ImpegnoSpesa = Nothing
            If Me.ImpegnoSpesa Is Nothing Then
                impegno = New ParsecAtt.ImpegnoSpesa
                impegno.Guid = Guid.NewGuid.ToString
            Else
                If Not Me.Request.QueryString("copia") Is Nothing Then
                    impegno = New ParsecAtt.ImpegnoSpesa
                    impegno.Guid = Guid.NewGuid.ToString
                Else
                    impegno = Me.ImpegnoSpesa
                End If

            End If

            impegno.Importo = Me.ImportoTextBox.Value

            impegno.Note = Me.DescrizioneTextBox.Text.Trim
            impegno.Capitolo = CLng(Me.CapitoloTextBox.Text)

            impegno.AnnoEsercizio = CInt(Me.AnniComboBox.SelectedItem.Text)

            impegno.Articolo = Me.ArticoloTextBox.Value
            impegno.NumeroImpegno = Me.ImpegnoTextBox.Text

            impegno.NumeroSubImpegno = Me.SubImpegnoTextBox.Text

            impegno.NumeroAttoAssunzioneOriginale = Me.NumeroAttoAssunzioneTextBox.Text
            impegno.DisponibilitaResiduale = Me.DisponibilitaResidualeTextBox.Value
            impegno.StanziamentoIniziale = Me.StanziamentoInizialeTextBox.Value
            impegno.StanziamentoAssestato = Me.StanziamentoAssestatoTextBox.Value

            '********************************************************************************************************
            'D. Lgs. 118/2011
            '********************************************************************************************************
            impegno.Missione = Nothing
            impegno.Programma = Nothing
            impegno.Titolo = Nothing
            impegno.MacroAggregato = Nothing

            If Not String.IsNullOrEmpty(Me.MissioneTextBox.Text) Then
                impegno.Missione = Me.MissioneTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.ProgrammaTextBox.Text) Then
                impegno.Programma = Me.ProgrammaTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.TitoloTextBox.Text) Then
                impegno.Titolo = Me.TitoloTextBox.Text
            End If

            If Not String.IsNullOrEmpty(Me.MacroAggregatoTextBox.Text) Then
                impegno.MacroAggregato = Me.MacroAggregatoTextBox.TextWithLiterals
            End If

            impegno.Servizio = Nothing
            impegno.Funzione = Nothing
            impegno.Intervento = Nothing
            '********************************************************************************************************

            If Not String.IsNullOrEmpty(Me.CigTextBox.Text) Then
                impegno.CIG = Me.CigTextBox.Text
            End If

            If Me.ImpegniSpesa Is Nothing Then
                Me.ImpegniSpesa = New List(Of ParsecAtt.ImpegnoSpesa)
            End If

            Dim esiste As Boolean = False

            If impegno.Id = 0 Then
                esiste = Not Me.ImpegniSpesa.Where(Function(c) c.Guid = impegno.Guid).FirstOrDefault Is Nothing
            Else
                esiste = Not Me.ImpegniSpesa.Where(Function(c) c.Id = impegno.Id).FirstOrDefault Is Nothing
            End If

            If Not esiste Then
                Me.ImpegniSpesa.Add(impegno)
            End If

            Dim somma = Me.ImpegniSpesa.Where(Function(c) c.NumeroImpegno = impegno.NumeroImpegno And c.AnnoEsercizio = 0).Sum(Function(c) c.Importo)

            If Me.ImportoTextBox.Value > Me.DisponibilitaResidualeTextBox.Value Then
                ParsecUtility.Utility.MessageBox("Avviso" & vbCrLf & "L'importo è maggiore della disponibilità residuale", False)
            End If

            If Me.TipologiaGestioneContabilita = ParsecAtt.TipologiaGestioneContabilita.DedaGroup Then  'DEDAGROUP

                impegno.IdCapitoloDedaGroup = Nothing
                impegno.IdImpegnoDedaGroup = Nothing
                impegno.IdFornitoreDedaGroup = Nothing

                If Not String.IsNullOrEmpty(Me.FornitoriComboBox.SelectedValue) Then
                    impegno.IdFornitoreDedaGroup = CInt(Me.FornitoriComboBox.SelectedValue)
                End If


                'SE HO SELEZIONATO IL CAPITOLO
                If Not String.IsNullOrEmpty(Me.IdCapitoloDedagroup.Value) Then
                    impegno.IdCapitoloDedaGroup = Me.IdCapitoloDedagroup.Value
                End If

                'SE HO SELEZIONATO L'IMPEGNO
                If Not String.IsNullOrEmpty(Me.IdImpegnoDedaGroup.Value) Then
                    impegno.IdImpegnoDedaGroup = Me.IdImpegnoDedaGroup.Value
                End If

                'SE NON HO SELEZIONATO UN CAPITOLO DEDAGROPUP (IdCapitoloDedaGroup E' NOTHING) NON DEVO SALVARE L'IMPEGNO SUL DB DEDAGROUP

            End If

            ParsecUtility.SessionManager.ImpegnoSpesa = impegno
            ParsecUtility.Utility.ClosePopup(True)
        End If

    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaVista()
    End Sub

    Protected Sub TrovaImpegnoJSibacImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles TrovaImpegnoJSibacImageButton.Click

        'Dim impegniTrovati As List(Of ParsecPostgres.ImpegnoSpesa) = Nothing

        'Dim impegni As New ParsecPostgres.ImpegniSpesa
        'Dim filtro As ParsecPostgres.FiltroImpegno = Me.GetFiltro
        'impegni.FiltraByCapitolo(filtro)

        'Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        'If utenteCollegato.SuperUser = False Then

        '    Dim listaImpegni = impegni.ToList

        '    Dim utentiCentro As New ParsecAdmin.UtenteCentroResponsabilitaRepository
        '    Dim view = (From impegno In listaImpegni
        '              Join centroResponsabilita In utentiCentro.GetQuery.Where(Function(c) c.IdUtente = utenteCollegato.Id).ToList
        '              On impegno.CodiceCentroResponsabilita Equals centroResponsabilita.CodiceCentroResponsabilita
        '              Select impegno) '.Distinct

        '    impegniTrovati = view.ToList

        'Else
        '    impegniTrovati = impegni.ToList
        'End If

        'If impegniTrovati.Count = 1 Then
        '    Me.AggiornaImpegnoJSibac(impegniTrovati.FirstOrDefault)
        'Else
        Dim pageUrl As String = String.Empty
        Dim queryString As New Hashtable


        Select Case Me.TipologiaGestioneContabilita
            Case ParsecAtt.TipologiaGestioneContabilita.Classica
                'NIENTE
            Case ParsecAtt.TipologiaGestioneContabilita.JSibac
                'JSIBAC
                pageUrl = "~/UI/AttiDecisionali/pages/search/RicercaImpegnoJSibacPage.aspx"
                queryString.Add("obj", Me.AggiornaImpegnoJSibacImageButton.ClientID)

            Case ParsecAtt.TipologiaGestioneContabilita.APK
                'APK

                pageUrl = "~/UI/AttiDecisionali/pages/search/RicercaImpegnoApkPage.aspx"
                queryString.Add("obj", Me.AggiornaImpegnoApkImageButton.ClientID)
            Case ParsecAtt.TipologiaGestioneContabilita.DedaGroup
                'DEDAGROUP

                pageUrl = "~/UI/AttiDecisionali/pages/search/RicercaImpegnoDedaGroupPage.aspx"
                queryString.Add("obj", Me.AggiornaImpegnoDedaGroupImageButton.ClientID)

            Case Else
                'TODO
        End Select



        queryString.Add("Tipologia", 2) ' 1=ENTRATA 2= SPESA
        queryString.Add("AnnoEsercizio", Me.AnniComboBox.SelectedValue)
        queryString.Add("AnnoImpegno", Me.AnniComboBox.SelectedValue)


        If Me.CapitoloTextBox.Value.HasValue Then
            queryString.Add("NumeroCapitolo", Me.CapitoloTextBox.Value)
        End If

        If Me.ArticoloTextBox.Value.HasValue Then
            queryString.Add("Articolo", Me.ArticoloTextBox.Value)
        End If

        ParsecUtility.Utility.ShowPopup(pageUrl, 970, 490, queryString, False)
        'End If

    End Sub

    Private Sub AggiornaImpegnoJSibac(ByVal impegno As ParsecPostgres.ImpegnoSpesa)
        Me.CapitoloTextBox.Text = impegno.NumeroCapitolo

        Me.ArticoloTextBox.Text = impegno.Articolo.ToString
        Me.DescrizioneTextBox.Text = impegno.DescrizioneCapitolo

        If Not String.IsNullOrEmpty(impegno.Missione) Then
            Me.MissioneTextBox.Text = impegno.Missione
        End If

        If Not String.IsNullOrEmpty(impegno.Programma) Then
            Me.ProgrammaTextBox.Text = impegno.Programma
        End If
        If Not String.IsNullOrEmpty(impegno.MacroAggregato) Then
            Me.MacroAggregatoTextBox.Text = impegno.MacroAggregato
        End If

        If Not String.IsNullOrEmpty(impegno.Titolo) Then
            Me.TitoloTextBox.Text = impegno.Titolo
        End If

        If impegno.PrevisioneIniziale.HasValue Then
            Me.StanziamentoInizialeTextBox.Value = impegno.PrevisioneIniziale
        End If

        If impegno.Assestato.HasValue Then
            Me.StanziamentoAssestatoTextBox.Value = impegno.Assestato
        End If

        If impegno.DisponibilitaResidua.HasValue Then
            Me.DisponibilitaResidualeTextBox.Value = impegno.DisponibilitaResidua
        End If

        If impegno.NumeroImpegno.HasValue Then
            Me.ImpegnoTextBox.Text = impegno.NumeroImpegno
        End If

        If impegno.Importo.HasValue Then
            Me.ImportoTextBox.Value = impegno.Importo
        End If
    End Sub

    Private Sub AggiornaImpegnoApk(ByVal impegno As ParsecWebServices.ImpegnoApkModel)
        Me.CapitoloTextBox.Text = impegno.NumeroCapitolo

        Me.ArticoloTextBox.Text = impegno.Articolo.ToString
        Me.DescrizioneTextBox.Text = impegno.DescrizioneCapitolo

        Me.MissioneTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.Missione) Then
            Me.MissioneTextBox.Text = impegno.Missione
        End If

        Me.ProgrammaTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.Programma) Then
            Me.ProgrammaTextBox.Text = impegno.Programma
        End If

        Me.MacroAggregatoTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.Macroaggregato) Then
            Me.MacroAggregatoTextBox.Text = impegno.Macroaggregato
        End If

        Me.TitoloTextBox.Text = String.Empty
        If Not String.IsNullOrEmpty(impegno.Titolo) Then
            Me.TitoloTextBox.Text = impegno.Titolo
        End If

        If Not String.IsNullOrEmpty(impegno.NumeroCapitolo) Then

            Dim req As New ParsecWebServices.CapitoliApkRequest

            req.Tipologia = "S"
            req.Esercizio = impegno.Esercizio
            req.Anno = impegno.AnnoImpegno
            req.NumeroCapitolo = impegno.NumeroCapitolo

            Dim baseUrl As String = ParsecAdmin.WebConfigSettings.GetKey("BaseUrlWsApk")

            Dim service As New ParsecWebServices.CapitoliApkService(baseUrl, req)

            Dim capitolo = service.GetView.FirstOrDefault

            If Not capitolo Is Nothing Then

                Me.StanziamentoInizialeTextBox.Value = Nothing
                If capitolo.ImportoIniziale.HasValue Then
                    Me.StanziamentoInizialeTextBox.Value = capitolo.ImportoIniziale
                End If

                Me.StanziamentoAssestatoTextBox.Value = Nothing
                If capitolo.ImportoAttuale.HasValue Then
                    Me.StanziamentoAssestatoTextBox.Value = capitolo.ImportoAttuale
                End If

                Me.DisponibilitaResidualeTextBox.Value = Nothing
                If capitolo.ResiduoDaImpegnareAccertare.HasValue Then
                    Me.DisponibilitaResidualeTextBox.Value = capitolo.ResiduoDaImpegnareAccertare
                End If

            End If

        End If

        Me.ImpegnoTextBox.Text = String.Empty
        If String.IsNullOrEmpty(impegno.NumeroImpegno) Then
            Me.ImpegnoTextBox.Text = impegno.NumeroImpegno
        End If

        Me.ImportoTextBox.Value = Nothing
        If impegno.Importo.HasValue Then
            Me.ImportoTextBox.Value = impegno.Importo
        End If
    End Sub

    Protected Sub AggiornaImpegnoJSibacImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaImpegnoJSibacImageButton.Click
        If Not ParsecUtility.SessionManager.ImpegnoSpesa Is Nothing Then
            Dim impegno As ParsecPostgres.ImpegnoSpesa = ParsecUtility.SessionManager.ImpegnoSpesa
            ParsecUtility.SessionManager.ImpegnoSpesa = Nothing
            Me.AggiornaImpegnoJSibac(impegno)
        End If
    End Sub

    Protected Sub AggiornaImpegnoApkImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaImpegnoApkImageButton.Click
        If Not ParsecUtility.SessionManager.ImpegnoSpesa Is Nothing Then
            Dim impegno As ParsecWebServices.ImpegnoApkModel = ParsecUtility.SessionManager.ImpegnoSpesa
            ParsecUtility.SessionManager.ImpegnoSpesa = Nothing
            Me.AggiornaImpegnoApk(impegno)
        End If
    End Sub

    Protected Sub TrovaCapitoloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaCapitoloImageButton.Click

        Dim pageUrl As String = String.Empty
        Dim queryString As New Hashtable

        Select Case Me.TipologiaGestioneContabilita
            Case ParsecAtt.TipologiaGestioneContabilita.Classica
                pageUrl = "~/UI/AttiDecisionali/pages/search/RicercaCapitoloPage.aspx"
                queryString.Add("obj", Me.AggiornaCapitoloImageButton.ClientID)
            Case ParsecAtt.TipologiaGestioneContabilita.JSibac
                'JSIBAC
                pageUrl = "~/UI/AttiDecisionali/pages/search/RicercaCapitoloJSibacPage.aspx"
                queryString.Add("obj", Me.AggiornaCapitoloJSibacImageButton.ClientID)
            Case ParsecAtt.TipologiaGestioneContabilita.APK
                'APK
                pageUrl = "~/UI/AttiDecisionali/pages/search/RicercaCapitoloApkPage.aspx"
                queryString.Add("obj", Me.AggiornaCapitoloApkImageButton.ClientID)

            Case ParsecAtt.TipologiaGestioneContabilita.DedaGroup
                'DEDAGROUP

                pageUrl = "~/UI/AttiDecisionali/pages/search/RicercaCapitoloDedaGroupPage.aspx"
                queryString.Add("obj", Me.AggiornaCapitoloDedaGroupImageButton.ClientID)

            Case Else
                'TODO
        End Select

        queryString.Add("tipo", 2) ' 1=ENTRATA 2= SPESA
        queryString.Add("anno", Me.AnniComboBox.SelectedValue)
        ParsecUtility.Utility.ShowPopup(pageUrl, 970, 450, queryString, False)
    End Sub

    Protected Sub AggiornaCapitoloJSibacImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaCapitoloJSibacImageButton.Click
        If Not ParsecUtility.SessionManager.Capitolo Is Nothing Then
            Dim capitolo As ParsecPostgres.Capitolo = ParsecUtility.SessionManager.Capitolo
            ParsecUtility.SessionManager.Capitolo = Nothing

            Me.CapitoloTextBox.Text = capitolo.NumeroCapitolo

            Me.ArticoloTextBox.Text = capitolo.Articolo.ToString
            Me.DescrizioneTextBox.Text = capitolo.Descrizione

            If Not String.IsNullOrEmpty(capitolo.Missione) Then
                Me.MissioneTextBox.Text = capitolo.Missione
            End If

            If Not String.IsNullOrEmpty(capitolo.Programma) Then
                Me.ProgrammaTextBox.Text = capitolo.Programma
            End If
            If Not String.IsNullOrEmpty(capitolo.MacroAggregato) Then
                Me.MacroAggregatoTextBox.Text = capitolo.MacroAggregato
            End If

            If Not String.IsNullOrEmpty(capitolo.Titolo) Then
                Me.TitoloTextBox.Text = capitolo.Titolo
            End If

            'solo jsibac
            If capitolo.PrevisioneIniziale.HasValue Then
                Me.StanziamentoInizialeTextBox.Value = capitolo.PrevisioneIniziale
            End If

            If capitolo.Assestato.HasValue Then
                Me.StanziamentoAssestatoTextBox.Value = capitolo.Assestato
            End If

            If capitolo.Disponibilita.HasValue Then
                Me.DisponibilitaResidualeTextBox.Value = capitolo.Disponibilita
            End If


        End If
    End Sub

    Protected Sub AggiornaCapitoloImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaCapitoloImageButton.Click
        If Not ParsecUtility.SessionManager.Capitolo Is Nothing Then

            Dim capitolo As ParsecAtt.Capitolo = ParsecUtility.SessionManager.Capitolo
            ParsecUtility.SessionManager.Capitolo = Nothing
            Me.CapitoloTextBox.Text = capitolo.NumeroCapitolo

            Me.ArticoloTextBox.Text = capitolo.Articolo.ToString
            Me.DescrizioneTextBox.Text = capitolo.Descrizione

            If Not String.IsNullOrEmpty(capitolo.Missione) Then
                Me.MissioneTextBox.Text = capitolo.Missione
            End If

            If Not String.IsNullOrEmpty(capitolo.Programma) Then
                Me.ProgrammaTextBox.Text = capitolo.Programma
            End If
            If Not String.IsNullOrEmpty(capitolo.MacroAggregato) Then
                Me.MacroAggregatoTextBox.Text = capitolo.MacroAggregato
            End If

            If Not String.IsNullOrEmpty(capitolo.Titolo) Then
                Me.TitoloTextBox.Text = capitolo.Titolo
            End If

        End If

    End Sub

    Protected Sub AggiornaCapitoloApkImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaCapitoloApkImageButton.Click

        If Not ParsecUtility.SessionManager.Capitolo Is Nothing Then
            Dim capitolo As ParsecWebServices.CapitoloApkModel = ParsecUtility.SessionManager.Capitolo
            ParsecUtility.SessionManager.Capitolo = Nothing

            Me.CapitoloTextBox.Text = capitolo.NumeroCapitolo

            Me.ArticoloTextBox.Text = capitolo.Articolo.ToString
            Me.DescrizioneTextBox.Text = capitolo.CapitoloDescrizione

            Me.MissioneTextBox.Text = String.Empty
            If Not String.IsNullOrEmpty(capitolo.Missione) Then
                Me.MissioneTextBox.Text = capitolo.Missione
            End If

            Me.ProgrammaTextBox.Text = String.Empty
            If Not String.IsNullOrEmpty(capitolo.Programma) Then
                Me.ProgrammaTextBox.Text = capitolo.Programma
            End If

            Me.MacroAggregatoTextBox.Text = String.Empty
            If Not String.IsNullOrEmpty(capitolo.Macroaggregato) Then
                Me.MacroAggregatoTextBox.Text = capitolo.Macroaggregato
            End If

            Me.TitoloTextBox.Text = String.Empty
            If Not String.IsNullOrEmpty(capitolo.Titolo) Then
                Me.TitoloTextBox.Text = capitolo.Titolo
            End If

            Me.StanziamentoInizialeTextBox.Value = Nothing
            If capitolo.ImportoIniziale.HasValue Then
                Me.StanziamentoInizialeTextBox.Value = capitolo.ImportoIniziale
            End If

            Me.StanziamentoAssestatoTextBox.Value = Nothing
            If capitolo.ImportoAttuale.HasValue Then
                Me.StanziamentoAssestatoTextBox.Value = capitolo.ImportoAttuale
            End If

            Me.DisponibilitaResidualeTextBox.Value = Nothing
            If capitolo.ResiduoDaImpegnareAccertare.HasValue Then
                Me.DisponibilitaResidualeTextBox.Value = capitolo.ResiduoDaImpegnareAccertare
            End If

        End If

    End Sub

    Protected Sub AggiornaCapitoloDedaGroupImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaCapitoloDedaGroupImageButton.Click

        If Not ParsecUtility.SessionManager.Capitolo Is Nothing Then

            Me.ImpegnoTextBox.Text = String.Empty

            Me.AbilitaModalitaDedaGroup(False)

            'SE HO SELEZIONATO UN IMPEGNO DEDAGROUP
            If Not Me.FornitoriComboBox.Enabled Then
                Me.FornitoriComboBox.Enabled = True
                Me.FornitoriComboBox.Text = String.Empty
                Me.FornitoriComboBox.SelectedValue = String.Empty
            End If

            Me.IdImpegnoDedaGroup.Value = String.Empty
            Me.IdCapitoloDedagroup.Value = String.Empty

            Dim capitolo As ParsecWebServices.CapitoloModel = ParsecUtility.SessionManager.Capitolo

            'HO CERCATO IL CAPITOLO
            Me.IdCapitoloDedagroup.Value = capitolo.IdCapitolo

            ParsecUtility.SessionManager.Capitolo = Nothing

            Me.CapitoloTextBox.Text = capitolo.Capitolo

            Me.ArticoloTextBox.Text = capitolo.Articolo.ToString
            Me.DescrizioneTextBox.Text = capitolo.Oggetto


            Me.MissioneTextBox.Text = String.Empty
            If Not String.IsNullOrEmpty(capitolo.CodiceMissione) Then
                Me.MissioneTextBox.Text = capitolo.CodiceMissione
            End If

            Me.ProgrammaTextBox.Text = String.Empty
            If Not String.IsNullOrEmpty(capitolo.CodiceProgramma) Then
                Me.ProgrammaTextBox.Text = capitolo.CodiceProgramma
            End If

            Me.MacroAggregatoTextBox.Text = String.Empty
            If Not String.IsNullOrEmpty(capitolo.MacroAggregato) Then
                Me.MacroAggregatoTextBox.Text = capitolo.MacroAggregato
            End If

            Me.TitoloTextBox.Text = String.Empty
            If Not String.IsNullOrEmpty(capitolo.CodiceTitolo) Then
                Me.TitoloTextBox.Text = capitolo.CodiceTitolo
            End If

            Me.StanziamentoInizialeTextBox.Value = Nothing

            Me.StanziamentoInizialeTextBox.Value = capitolo.StanziamentoIniziale

            Me.StanziamentoAssestatoTextBox.Value = Nothing

            'Lo stanziamento di cassa indica l'ammontare complessivo della liquidità movimentabile su un determinato capitolo, 
            'in particolare per le spese indica la somma di denaro che si prevede di pagare durante l'esercizio.

            'Le variazioni di bilancio sono gli incrementi o i decrementi degli stanziamenti previsti nei capitoli.

            Me.StanziamentoAssestatoTextBox.Value = capitolo.StanziamentoIniziale + capitolo.Variazioni

            Me.DisponibilitaResidualeTextBox.Value = Nothing

            Me.DisponibilitaResidualeTextBox.Value = capitolo.Disponibilita
        End If

    End Sub

    Protected Sub AggiornaImpegnoDedaGroupImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles AggiornaImpegnoDedaGroupImageButton.Click
        If Not ParsecUtility.SessionManager.ImpegnoSpesa Is Nothing Then
            Me.IdImpegnoDedaGroup.Value = String.Empty
            Me.IdCapitoloDedagroup.Value = String.Empty
            Dim impegno As ParsecWebServices.ImpegnoModel = ParsecUtility.SessionManager.ImpegnoSpesa
            ParsecUtility.SessionManager.ImpegnoSpesa = Nothing
            Me.AggiornaImpegnoDedaGroup(impegno)
        End If
    End Sub

#End Region

End Class