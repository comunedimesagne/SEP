Imports Telerik.Web.UI

Partial Class FirmaPage
    Inherits System.Web.UI.Page


#Region "PROPRIETA'"

    Public Property Firma() As ParsecAtt.Firma
        Get
            Return CType(Session("FirmaPage_Firma"), ParsecAtt.Firma)
        End Get
        Set(ByVal value As ParsecAtt.Firma)
            Session("FirmaPage_Firma") = value
        End Set
    End Property

    Public Property IdUfficio() As Integer
        Get
            Return CType(Session("FirmaPage_IdUfficio"), Integer)
        End Get
        Set(ByVal value As Integer)
            Session("FirmaPage_IdUfficio") = value
        End Set
    End Property

    Public Property IdSettore() As Integer
        Get
            Return CType(Session("FirmaPage_IdSettore"), Integer)
        End Get
        Set(ByVal value As Integer)
            Session("FirmaPage_IdSettore") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack Then
            Me.Firma = ParsecUtility.SessionManager.Firma
            Me.IdUfficio = CInt(Request.QueryString("idUfficio"))
            Me.IdSettore = CInt(Request.QueryString("IdSettore"))
            Me.AggiornaVista(Me.IdUfficio)
        End If
    End Sub

#End Region

#Region "METODI PRIVATI"

    Private Sub CaricaDelegati(ByVal id As Integer, ByVal keyValue As ParsecAtt.KeyValue, ByVal responsabile As Boolean)
        Dim deleghe As New ParsecAtt.DelegaViewRepository
        Me.DelegatiComboBox.DataValueField = "Id"
        Me.DelegatiComboBox.DataTextField = "Descrizione"

        Dim filtro As New ParsecAtt.FiltroDelega
        If responsabile Then
            filtro.IdStruttura = id
        Else
            filtro.IdUtente = id
        End If
        Dim lista = deleghe.GetKeyValue(filtro)
        lista.Add(keyValue)

        Me.DelegatiComboBox.DataSource = lista.OrderBy(Function(c) c.Descrizione).ToList
        Me.DelegatiComboBox.DataBind()
        ' Me.DelegatiComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.DelegatiComboBox.SelectedIndex = 0
        deleghe.Dispose()
    End Sub

    Private Sub CaricaFirmatari(ByVal id As Integer, ByVal keyValue As ParsecAtt.KeyValue, ByVal responsabile As Boolean)
        Dim deleghe As New ParsecAtt.DelegaViewRepository
        Me.FirmatariComboBox.DataValueField = "Id"
        Me.FirmatariComboBox.DataTextField = "Descrizione"
        Dim filtro As New ParsecAtt.FiltroDelega
        If responsabile Then
            filtro.IdStruttura = id
        Else
            filtro.IdUtente = id
        End If

        Dim lista = deleghe.GetKeyValue(filtro)
        lista.Add(keyValue)

        Me.FirmatariComboBox.DataSource = lista.OrderBy(Function(c) c.Descrizione).ToList
        Me.FirmatariComboBox.DataBind()
        'Me.FirmatariComboBox.Items.Insert(0, New RadComboBoxItem("", -1))
        Me.FirmatariComboBox.SelectedIndex = 0
        deleghe.Dispose()
    End Sub


    Private Function GetDefaultStruttura(ByVal firma As ParsecAtt.Firma) As String
        Dim res As String = String.Empty

        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Select Case firma.IdTipologiaFirma
            Case 1 'STATICA

                Dim firme As New ParsecAtt.FirmeRepository
                Dim firmaPrecedente = firme.Where(Function(c) c.Id = firma.Id).FirstOrDefault
                res = firmaPrecedente.DefaultStruttura
                firme.Dispose()

            Case 2  'UTENTE COLLEGATO
                res = utenteCollegato.Nome & " " & utenteCollegato.Cognome
            Case 3 '  ORGANIGRAMMA

                Dim gerarchiaPersona As Integer = 400
                Dim strutture As New ParsecAtt.StrutturaViewRepository
                Dim idGerarchia As Integer = 0
                Dim idQualifica As Integer = 0
                Dim idUfficio As Integer = 0
                If Me.IdUfficio = 0 Then
                    idUfficio = Me.IdSettore
                Else
                    idUfficio = Me.IdUfficio
                End If

                idGerarchia = firma.IdGerarchia
                idQualifica = firma.IdTipologiaQualifica
                Dim ufficioSettore As ParsecAtt.Struttura = Nothing
                ufficioSettore = strutture.GetQuery.Where(Function(c) c.IdGerarchia = idGerarchia And (c.Id = Me.IdSettore Or c.Id = idUfficio)).FirstOrDefault
                If ufficioSettore Is Nothing Then
                    ufficioSettore = strutture.GetQuery.Where(Function(c) c.Id = idUfficio).FirstOrDefault
                End If
                Dim persona = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = gerarchiaPersona And c.IdQualifica = idQualifica And c.IdPadre = ufficioSettore.Id).FirstOrDefault
                If persona Is Nothing Then
                    Dim uffici = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 200 And c.IdPadre = ufficioSettore.Id).ToList
                    For Each uf In uffici
                        Dim idUff As Integer = uf.Id
                        persona = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = gerarchiaPersona And c.IdQualifica = idQualifica And c.IdPadre = idUff).FirstOrDefault
                        If Not persona Is Nothing Then
                            Exit For
                        End If
                    Next
                End If

                strutture.Dispose()

                If Not persona Is Nothing Then
                    res = persona.Descrizione
                Else
                End If


        End Select

        Return res
    End Function


    


    Private Sub AggiornaVista(ByVal idUfficio As Integer)
        Dim firma As ParsecAtt.Firma = Me.Firma
        Dim id As Integer = 0
        Dim utenteCollegato As ParsecAdmin.Utente = CType(ParsecUtility.Applicazione.UtenteCorrente, ParsecAdmin.Utente)

        Dim idResponsabile As Integer = 0

        Dim responsabile As Boolean = False

        If firma.IdUtente <> 0 And firma.IdUtente <> -1 Then
            id = firma.IdUtente
            If firma.IdUtentePredefinito.HasValue Then
                id = firma.IdUtentePredefinito
            End If
        Else
            If firma.Id <> 2 Then
                If firma.Id = 1 Or firma.Id = 4 Then

                    If idUfficio <> 0 Then
                        Dim strutture As New ParsecAtt.StrutturaViewRepository
                        Dim idPadre = strutture.GetQuery.Where(Function(c) c.Id = idUfficio).FirstOrDefault.IdPadre
                        idResponsabile = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing And c.IdGerarchia = 400 And c.Responsabile And c.IdPadre = idPadre).FirstOrDefault.Id
                        strutture.Dispose()
                    End If

                    id = idResponsabile
                    responsabile = True
                End If
            Else
                id = utenteCollegato.Id
            End If

        End If

        Dim keyValue As New ParsecAtt.KeyValue With {.Id = firma.IdUtente, .Descrizione = firma.DefaultStruttura}
        If firma.IdUtentePredefinito.HasValue Then
              keyValue = New ParsecAtt.KeyValue With {.Id = firma.IdUtentePredefinito, .Descrizione = Me.GetDefaultStruttura(firma)}
        End If

        Me.CaricaFirmatari(id, keyValue, responsabile)
        Me.FirmatariComboBox.FindItemByValue(firma.IdUtente.ToString).Selected = True

        Me.CaricaDelegati(id, keyValue, responsabile)
        If firma.IdDelegato.HasValue Then
            'Me.DelegatiComboBox.FindItemByValue(keyValue.Id.ToString).Selected = True
            Me.DelegatiComboBox.FindItemByValue(firma.IdDelegato.Value.ToString).Selected = True
        End If


        Me.TitoloLabel.Text = "Modifica Firma - " & firma.Descrizione
        'Me.DescrizioneFirmaLabel.Text = firma.Descrizione
        Me.QualificaTextBox.Text = firma.DefaultQualifica
        Me.ParereTextBox.Text = firma.DefaultEsito

        Me.DataFirmaTextBox.SelectedDate = firma.DataFirma

        If Not String.IsNullOrEmpty(firma.DefaultStrutturaDelegato) Then
            Me.DelegaCheckBox.Checked = True
            Me.QualificaDelegatoTextBox.Text = firma.DefaultQualificaDelegato
        Else
            Me.DelegaCheckBox.Checked = False
        End If

        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro As ParsecAdmin.Parametri = parametri.GetByName("ParereNonModificabile", ParsecAdmin.TipoModulo.ATT)
        parametri.Dispose()
        If Not parametro Is Nothing Then
            If CBool(parametro.Valore) Then
                If firma.IdUtente <> utenteCollegato.Id OrElse Not String.IsNullOrEmpty(firma.DefaultStrutturaDelegato) Then
                    Me.ParereTextBox.ReadOnly = True
                    Me.ParereTextBox.ToolTip = "Il parere può essere modificato solo dall'utente " & firma.DefaultStruttura
                End If
            End If
        End If

        Me.AggiornaDelega()
    End Sub

    Private Sub AggiornaDelega()
        Me.QualificaDelegatoTextBox.ReadOnly = Not Me.DelegaCheckBox.Checked
        Me.DelegatiComboBox.Enabled = Me.DelegaCheckBox.Checked
    End Sub

    Private Function VerificaDati() As Boolean
        Dim message As New StringBuilder

        If String.IsNullOrEmpty(Me.QualificaTextBox.Text) Then
            message.AppendLine("La qualifica del firmatario.")
        End If
        If Me.DelegaCheckBox.Checked Then
            If String.IsNullOrEmpty(Me.QualificaDelegatoTextBox.Text) Then
                message.AppendLine("La qualifica del delegato.")
            End If
        End If

        If message.Length > 0 Then
            message.Insert(0, "E' necessario specificare:" & vbCrLf)
            ParsecUtility.Utility.MessageBox(message.ToString, False)
        End If

        Return message.Length = 0
    End Function


#End Region

#Region "EVENTI CONTROLLI"

    Protected Sub DelegaCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DelegaCheckBox.CheckedChanged
        AggiornaDelega()
        Me.QualificaDelegatoTextBox.Text = String.Empty
    End Sub



    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        If Me.VerificaDati() Then
            Dim firma As ParsecAtt.Firma = Me.Firma
            firma.DefaultStruttura = Me.FirmatariComboBox.SelectedItem.Text
            firma.DataFirma = Me.DataFirmaTextBox.SelectedDate
            firma.DefaultQualifica = Me.QualificaTextBox.Text
            firma.DefaultEsito = Me.ParereTextBox.Text
            If Me.DelegaCheckBox.Checked Then
                firma.DefaultQualificaDelegato = Me.QualificaDelegatoTextBox.Text
                firma.DefaultStrutturaDelegato = Me.DelegatiComboBox.SelectedItem.Text
                firma.IdDelegato = Me.DelegatiComboBox.SelectedValue
            Else
                firma.DefaultQualificaDelegato = Nothing
                firma.DefaultStrutturaDelegato = Nothing
                firma.IdDelegato = Nothing
            End If
            firma.IdUtente = Me.FirmatariComboBox.SelectedValue
            ParsecUtility.SessionManager.Firma = firma
            ParsecUtility.Utility.ClosePopup(True)
        End If
    End Sub

    Protected Sub AnnullaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnullaButton.Click
        Me.AggiornaVista(Me.IdUfficio)
    End Sub

#End Region

End Class
