Imports System.Data.SqlClient
Imports ParsecReportPDF
Imports System.Data

'* SPDX-License-Identifier: GPL-3.0-only

Partial Class StampaEtichettePage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.MainPage = CType(Me.Master, MainPage)
        Me.MainPage.NomeModulo = "Protocollo"
        Me.MainPage.DescrizioneProcedura = "> Stampa Etichette Protocolli in Partenza"

        If Not Me.Page.IsPostBack Then
            Me.ResettaFiltro()
        End If

        Dim tipiRicezione As New ParsecPro.TipiRicezioneInvioRepository
        Me.TipoRicezioneInvioListBox.DataSource = tipiRicezione.GetView(Nothing)
        Me.TipoRicezioneInvioListBox.DataValueField = "Id"
        Me.TipoRicezioneInvioListBox.DataTextField = "Descrizione"
        Me.TipoRicezioneInvioListBox.DataBind()
        tipiRicezione.Dispose()

        Me.NumeroProtocolloInizioTextBox.Attributes.Add("onblur", "var value =  $find('" & Me.NumeroProtocolloInizioTextBox.ClientID & "').get_value(); var textbox =  $find('" & Me.NumeroProtocolloFineTextBox.ClientID & "'); var s = textbox.get_value(); if(s == '') {textbox.set_value(value);textbox.selectAllText();}")

    End Sub

#End Region

#Region "PROPRIETA'"

    'Variabile di Sessione: lista delle etichette
    Public Property Etichette() As IEnumerable
        Get
            Return Session("StampaEtichettePage_Etichette")
        End Get
        Set(ByVal value As IEnumerable)
            Session("StampaEtichettePage_Etichette") = value
        End Set
    End Property

#End Region

    'Costruisce e restituisce il Filtro per la ricerca.
    Private Function GetFiltro() As ParsecPro.RegistrazioneFiltro

        Dim listaIdInvio As New List(Of Integer)

        For Each it As Telerik.Web.UI.RadListBoxItem In Me.TipoRicezioneInvioListBox.CheckedItems
            listaIdInvio.Add(CInt(it.Value))
        Next


        Dim numeroProtocolloInizio As Nullable(Of Int32)
        Dim numeroProtocolloFine As Nullable(Of Int32)

        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloInizioTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloInizioTextBox.Text) Then
                numeroProtocolloInizio = CInt(Me.NumeroProtocolloInizioTextBox.Text)
            End If
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloFineTextBox.Text)) Then
            If Not ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloFineTextBox.Text) Then
                numeroProtocolloFine = CInt(Me.NumeroProtocolloFineTextBox.Text)
            End If
        End If


        Dim filtro As New ParsecPro.RegistrazioneFiltro With
            {
                .DataProtocolloInizio = Me.DataProtocolloInizioTextBox.SelectedDate,
                .DataProtocolloFine = Me.DataProtocolloFineTextBox.SelectedDate,
                .NumeroProtocolloInizio = numeroProtocolloInizio,
                .NumeroProtocolloFine = numeroProtocolloFine,
                .ElencoId = listaIdInvio
            }


        Return filtro

    End Function

    'Metodo di Stampa delle Etichette
    Private Sub Print()

        Dim utenteCorrente As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim remotePath As String = ParsecAdmin.WebConfigSettings.GetKey("PathReport")
        Dim template As String = remotePath & "Label.xml"
        Dim filename As String = "rpt_" & utenteCorrente.Id.ToString & "_" & Session.SessionID & Now.Ticks.ToString & ".pdf"
        Dim reportPdf As String = remotePath & filename

        Dim listaId As New List(Of String)
        For Each it As Telerik.Web.UI.RadListBoxItem In Me.EtichetteListBox.CheckedItems
            listaId.Add(it.Value)
        Next

        Dim etichetteSelezionate = (From etichetta In Me.Etichette
                  Where listaId.Contains(etichetta.idEtichetta)
                  Select etichetta).ToList

        Dim dt As DataTable = Nothing

        If etichetteSelezionate.Count > 0 Then
            Dim row As DataRow = Nothing
            dt = New DataTable("Label")
            Dim dc1 As New DataColumn("Id", GetType(System.Int32))

            Dim dc2 As New DataColumn("DescEtichetta", GetType(System.String))

            Dim dc3 As New DataColumn("NumeroProt", GetType(System.String))

            Dim dc4 As New DataColumn("Destinatario", GetType(System.String))

            Dim dc5 As New DataColumn("Indirizzo", GetType(System.String))

            Dim dc6 As New DataColumn("Destinazione", GetType(System.String))

            dt.Columns.Add(dc1)
            dt.Columns.Add(dc2)
            dt.Columns.Add(dc3)
            dt.Columns.Add(dc4)
            dt.Columns.Add(dc5)
            dt.Columns.Add(dc6)

            For i As Integer = 0 To etichetteSelezionate.Count - 1
                row = dt.NewRow
                row("Id") = (i + 1).ToString
                row("DescEtichetta") = etichetteSelezionate(i).DescrizioneEtichetta
                row("NumeroProt") = etichetteSelezionate(i).NumeroProtocollo
                row("Destinatario") = etichetteSelezionate(i).Destinatario
                row("Indirizzo") = etichetteSelezionate(i).Indirizzo
                row("Destinazione") = etichetteSelezionate(i).Destinazione
                dt.Rows.Add(row)
            Next i

            Dim ds As New DataSet
            ds.Tables.Add(dt)

            PdfLabelPrint.LoadFromFile(ds, template, reportPdf)

            Dim parametriStampa As New Hashtable
            parametriStampa.Add("TipologiaStampa", "StampaEtichette")
            parametriStampa.Add("FullPath", reportPdf)
            Session("ParametriStampaPro") = parametriStampa
            Dim pageUrl As String = "~/UI/Protocollo/pages/user/StampaPage.aspx"
            ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)

        End If

    End Sub

    'AAvvia la stampa
    Protected Sub AnteprimaStampaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnteprimaStampaButton.Click
        If Me.EtichetteListBox.CheckedItems.Count = 0 Then
            ParsecUtility.Utility.MessageBox("E' necessario selezionare almeno un'etichetta!", False)
        Else
            Me.Print()
        End If
    End Sub

    'Resetta i campi della Maschera
    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.ResettaFiltro()
    End Sub

    'Metodo che convalida i parametriprima della Ricerca
    Private Function ConvalidaParametri(ByVal message As StringBuilder) As Boolean
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloInizioTextBox.Text)) Then
            If ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloInizioTextBox.Text) Then
                message.AppendLine("Se specificato, il campo 'Numero protocollo da' deve essere un numero!")
            End If
        End If
        If Not String.IsNullOrEmpty(Trim(Me.NumeroProtocolloFineTextBox.Text)) Then
            If ParsecUtility.Utility.CheckNumber(Me.NumeroProtocolloFineTextBox.Text) Then
                message.AppendLine("Se specificato, il campo 'Numero protocollo a' deve essere un numero!")
            End If
        End If
        Return Not message.Length > 0
    End Function

    'Metodo che resetta i campi della Maschera. Invocato da AnnullaButton.Click
    Private Sub ResettaFiltro()
        Me.Etichette = Nothing

        Me.DataProtocolloInizioTextBox.SelectedDate = Now
        Me.DataProtocolloFineTextBox.SelectedDate = Now
        Me.NumeroProtocolloInizioTextBox.Text = String.Empty
        Me.NumeroProtocolloFineTextBox.Text = String.Empty

        Me.TipoRicezioneInvioListBox.ClearChecked()
        Me.TipoRicezioneInvioListBox.ClearSelection()
        Me.SelectAllCheckBox.Checked = False

        Me.SelezionaTuttoProtocolliCheckBox.Checked = False
        Me.EtichetteListBox.ClearChecked()
        Me.EtichetteListBox.ClearSelection()

    End Sub

    'Seleziona/Deseleziona tutti i check della Lista TipoRicezioneInvioListBox.
    Protected Sub SelectAllCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectAllCheckBox.CheckedChanged
        Dim checked As Boolean = Me.SelectAllCheckBox.Checked
        For i As Integer = 0 To Me.TipoRicezioneInvioListBox.Items.Count - 1
            Me.TipoRicezioneInvioListBox.Items(i).Checked = checked
        Next
    End Sub

    'Effettua la Ricerca.
    Protected Sub FiltraImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltraImageButton.Click
        Dim message As New StringBuilder
        If Me.ConvalidaParametri(message) Then
            Me.ApplicaFiltro()
        Else
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
    End Sub

    'Applica il filtro prima nella Ricerca: richiamato da FiltraImageButton.Click
    Private Sub ApplicaFiltro()
        Dim filtro As ParsecPro.RegistrazioneFiltro = Me.GetFiltro
        Dim protocolli As New ParsecPro.RegistrazioniRepository
        Me.Etichette = protocolli.GetEtichette(filtro)
        Dim res = (From etichette In Me.Etichette
                  Select etichette).ToList
        If res.Count = 0 Then
            ParsecUtility.Utility.MessageBox("Nessuna registrazione trovata con i criteri di filtro impostati!", False)
        End If
        protocolli.Dispose()

        Me.EtichetteListBox.DataSource = Me.Etichette
        Me.EtichetteListBox.DataValueField = "IdEtichetta"
        Me.EtichetteListBox.DataTextField = "DescrizioneEtichetta"
        Me.EtichetteListBox.DataBind()
    End Sub

    'Evento LoadComplete della Pagina. 
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    'Metodo che seleziona o deseleziona tutti i check della Lista EtichetteListBox
    Protected Sub SelezionaTuttoProtocolliCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelezionaTuttoProtocolliCheckBox.CheckedChanged
        Dim checked As Boolean = Me.SelezionaTuttoProtocolliCheckBox.Checked
        For i As Integer = 0 To Me.EtichetteListBox.Items.Count - 1
            Me.EtichetteListBox.Items(i).Checked = checked
        Next
    End Sub


End Class