Imports Telerik.Web.UI

Partial Class OrdineGiornoPage
    Inherits System.Web.UI.Page

#Region "PROPRIETA'"

    Public Property ElementoOrdineGiorno As ParsecAtt.OrdineGiorno
        Get
            Return CType(Session("OrdineGiornoPage_ElementoOrdineGiorno"), ParsecAtt.OrdineGiorno)
        End Get
        Set(ByVal value As ParsecAtt.OrdineGiorno)
            Session("OrdineGiornoPage_ElementoOrdineGiorno") = value
        End Set
    End Property

    Public Property IdSeduta As Integer
        Get
            Return CType(Session("OrdineGiornoPage_IdSeduta"), Integer)
        End Get
        Set(ByVal value As Integer)
            Session("OrdineGiornoPage_IdSeduta") = value
        End Set
    End Property


    Public Property Ordinale As Integer
        Get
            Return CType(Session("OrdineGiornoPage_Ordinale"), Integer)
        End Get
        Set(ByVal value As Integer)
            Session("OrdineGiornoPage_Ordinale") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.UfficioTextBox.ReadOnly = True
     
        Me.CaricaStatiDiscussione()
        Me.NumeroTextBox.ReadOnly = True

        If Not Me.Page.IsPostBack Then
            Me.GetParametri()
            If Not Me.ElementoOrdineGiorno Is Nothing Then
                'Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
                'If Me.ElementoOrdineGiorno.Id <> 0 Then
                '    Me.ElementoOrdineGiorno = ordiniGiorno.GetById(Me.ElementoOrdineGiorno.Id)
                '    ordiniGiorno.Dispose()
                'End If
                Me.AggiornaVista(Me.ElementoOrdineGiorno)
            Else
                Me.ResettaVista()
            End If
        End If
    End Sub

#End Region

#Region "GESTIONE UFFICIO"


    Protected Sub TrovaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUfficioImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaOrganigrammaPage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUfficioImageButton.ClientID)
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)
        Dim parametriPagina As New Hashtable
        parametriPagina.Add("IdModulo", 3)
        parametriPagina.Add("IdUtente", utenteCollegato.Id)
        parametriPagina.Add("tipoSelezione", 0) 'singola
        parametriPagina.Add("livelliSelezionabili", "200")
        parametriPagina.Add("ultimoLivelloStruttura", "200")
        ParsecUtility.SessionManager.ParametriPagina = parametriPagina
        ParsecUtility.Utility.ShowPopup(pageUrl, 900, 670, queryString, False)
    End Sub


    Protected Sub AggiornaUfficioImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUfficioImageButton.Click
        If Not Session("SelectedStructures") Is Nothing Then
            Dim struttureSelezionate As List(Of ParsecAdmin.StrutturaAbilitata) = Session("SelectedStructures")
            Dim idUfficio As Integer = struttureSelezionate.First.Id
            'Aggiorno l'ufficio
            Me.UfficioTextBox.Text = struttureSelezionate.First.Descrizione
            Me.IdUfficioTextBox.Text = idUfficio.ToString

            Dim settore = Me.GetIdStruttura(idUfficio, 100)
            Me.IdSettoreTextBox.Text = settore.Id.ToString
            Me.SettoreTextBox.Text = settore.Descrizione

            Session("SelectedStructures") = Nothing
        End If
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

#End Region

#Region "MEDOTI PRIVATI"

    Private Sub GetParametri()
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("ElementoOrdineGiorno") Then
                Me.ElementoOrdineGiorno = parametriPagina("ElementoOrdineGiorno")
                Me.IdSeduta = CInt(parametriPagina("IdSeduta"))
                Me.Ordinale = CInt(parametriPagina("Ordinale"))
            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    Private Sub ResettaVista()
        Me.StatoDiscussioneComboBox.SelectedIndex = 0
        Me.OggettoTextBox.Text = String.Empty
        Me.NoteTextBox.Text = String.Empty

        Me.NumeroTextBox.Text = Me.Ordinale.ToString


        Me.IdUfficioTextBox.Text = String.Empty
        Me.IdSettoreTextBox.Text = String.Empty
        Me.UfficioTextBox.Text = String.Empty
        Me.SettoreTextBox.Text = String.Empty
        Me.TitleLabel.Text = "Nuovo Punto Ordine del Giorno"
    End Sub

    Private Sub AggiornaVista(ByVal elementoOrdineGiorno As ParsecAtt.OrdineGiorno)
        If elementoOrdineGiorno.IdStatoDiscussione.HasValue Then
            Me.StatoDiscussioneComboBox.FindItemByValue(elementoOrdineGiorno.IdStatoDiscussione.Value.ToString).Selected = True
        Else
            Me.StatoDiscussioneComboBox.SelectedIndex = 0
        End If
        Me.OggettoTextBox.Text = elementoOrdineGiorno.Oggetto
        Me.NoteTextBox.Text = elementoOrdineGiorno.Note
        Me.NumeroTextBox.Text = Me.Ordinale.ToString

        If elementoOrdineGiorno.IdUfficio.HasValue Then
            Me.IdUfficioTextBox.Text = elementoOrdineGiorno.IdUfficio
        End If

        Me.IdSettoreTextBox.Text = elementoOrdineGiorno.IdStruttura

        'Se è una proposta
        If elementoOrdineGiorno.DataProposta.HasValue Then
            Dim strutture As New ParsecAtt.StrutturaViewRepository
            Dim ufficio = strutture.GetQuery.Where(Function(c) c.Id = elementoOrdineGiorno.IdUfficio).FirstOrDefault
            If Not ufficio Is Nothing Then
                Me.UfficioTextBox.Text = ufficio.Descrizione
            End If
            Dim settore = strutture.GetQuery.Where(Function(c) c.Id = elementoOrdineGiorno.IdStruttura).FirstOrDefault
            If Not settore Is Nothing Then
                Me.SettoreTextBox.Text = settore.Descrizione
            End If
            strutture.Dispose()
        Else
            Me.UfficioTextBox.Text = elementoOrdineGiorno.DescrizioneUfficio
            Me.SettoreTextBox.Text = elementoOrdineGiorno.DescrizioneSettore
        End If


        Me.TrovaUfficioImageButton.Visible = False
        Me.EliminaUfficioImageButton.Visible = False

        'Se l'elemento dell'ordine del giorno selezionato è una proposta.
        If elementoOrdineGiorno.DataProposta.HasValue Then
            Me.OggettoTextBox.ReadOnly = True
            Me.NoteTextBox.ReadOnly = True
            Me.InfoPropostaLabel.Text = String.Format("Proposta N. {0} del {1}", elementoOrdineGiorno.ContatoreGenerale, String.Format("{0:dd/MM/yyyy}", elementoOrdineGiorno.DataProposta))
        End If

        Me.TitleLabel.Text = "Modifica Punto Ordine del Giorno"

    End Sub



    Private Sub CaricaStatiDiscussione()
        Dim stati As New ParsecAtt.StatoDiscussioneRepository
        Me.StatoDiscussioneComboBox.DataValueField = "Id"
        Me.StatoDiscussioneComboBox.DataTextField = "Descrizione"
        Me.StatoDiscussioneComboBox.DataSource = stati.GetKeyValue(Nothing)
        Me.StatoDiscussioneComboBox.DataBind()
        Me.StatoDiscussioneComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.StatoDiscussioneComboBox.SelectedIndex = 0
        stati.Dispose()
    End Sub


    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder

        If String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            message.AppendLine("L'oggetto.")
        End If
        If String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
            message.AppendLine("L'ufficio.")
        End If
       

        If message.Length > 0 Then
            message.Insert(0, "E' necessario specificare:" & vbCrLf)
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If
        Return message.Length = 0
    End Function

    Private Sub AggiornaOggetto()
        Dim elementoOrdineGiorno As ParsecAtt.OrdineGiorno = Nothing

        If Me.ElementoOrdineGiorno Is Nothing Then
            elementoOrdineGiorno = New ParsecAtt.OrdineGiorno
            elementoOrdineGiorno.Guid = Guid.NewGuid.ToString
            elementoOrdineGiorno.DescrizioneUfficio = Me.UfficioTextBox.Text
            elementoOrdineGiorno.DescrizioneSettore = Me.SettoreTextBox.Text
        Else
            elementoOrdineGiorno = Me.ElementoOrdineGiorno
        End If

        elementoOrdineGiorno.Ordinale = CInt(Me.NumeroTextBox.Text)
        elementoOrdineGiorno.IdSeduta = Me.IdSeduta

        elementoOrdineGiorno.IdStatoDiscussione = Nothing
        If Me.StatoDiscussioneComboBox.SelectedIndex <> 0 Then
            elementoOrdineGiorno.IdStatoDiscussione = CInt(Me.StatoDiscussioneComboBox.SelectedValue)
        End If

        If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
            elementoOrdineGiorno.Oggetto = Me.OggettoTextBox.Text.Trim
        End If

        If Not String.IsNullOrEmpty(Me.NoteTextBox.Text) Then
            elementoOrdineGiorno.Note = Me.NoteTextBox.Text.Trim
        End If

        If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
            elementoOrdineGiorno.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
        End If

        If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
            elementoOrdineGiorno.IdStruttura = CInt(Me.IdSettoreTextBox.Text)
        End If

        Me.ElementoOrdineGiorno = elementoOrdineGiorno

    End Sub


    Private Sub Save()
        Dim ordiniGiorno As New ParsecAtt.OrdineGiornoRepository
        Try
            Dim elementoOrdineGiorno As ParsecAtt.OrdineGiorno = ordiniGiorno.CreateFromInstance(Me.ElementoOrdineGiorno)

            elementoOrdineGiorno.Ordinale = CInt(Me.NumeroTextBox.Text)
            elementoOrdineGiorno.IdSeduta = Me.IdSeduta

            If Me.StatoDiscussioneComboBox.SelectedIndex <> 0 Then
                elementoOrdineGiorno.IdStatoDiscussione = CInt(Me.StatoDiscussioneComboBox.SelectedValue)
            End If

            If Not String.IsNullOrEmpty(Me.OggettoTextBox.Text) Then
                elementoOrdineGiorno.Oggetto = Me.OggettoTextBox.Text.Trim
            End If

            If Not String.IsNullOrEmpty(Me.NoteTextBox.Text) Then
                elementoOrdineGiorno.Note = Me.NoteTextBox.Text.Trim
            End If

            If Not String.IsNullOrEmpty(Me.IdUfficioTextBox.Text) Then
                elementoOrdineGiorno.IdUfficio = CInt(Me.IdUfficioTextBox.Text)
            End If

            If Not String.IsNullOrEmpty(Me.IdSettoreTextBox.Text) Then
                elementoOrdineGiorno.IdStruttura = CInt(Me.IdSettoreTextBox.Text)
            End If

            '****************************************************************
            'Gestione storico non utilizzata.
            'ordiniGiorno.ElementoOrdineGiorno = Me.ElementoOrdineGiorno
            '****************************************************************
            ordiniGiorno.Save(elementoOrdineGiorno)

            'Aggiorno l'oggetto
            Me.ElementoOrdineGiorno = ordiniGiorno.ElementoOrdineGiorno

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            ordiniGiorno.Dispose()
        End Try
    End Sub




#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Try
            If VerificaDati() Then
                Me.AggiornaOggetto()
                ParsecUtility.Utility.CloseRadWindowAndUpadateParent(False)
                ParsecUtility.SessionManager.ElementoOrdineGiorno = Me.ElementoOrdineGiorno
                Me.ElementoOrdineGiorno = Nothing
                Me.IdSeduta = Nothing
                Me.Ordinale = Nothing
            End If

        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        If Me.ElementoOrdineGiorno Is Nothing Then
            Me.ResettaVista()
        Else
            Me.AggiornaVista(Me.ElementoOrdineGiorno)
        End If
    End Sub

#End Region


End Class
