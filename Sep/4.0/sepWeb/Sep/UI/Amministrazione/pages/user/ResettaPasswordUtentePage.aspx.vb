Imports ParsecAdmin
Imports System.Transactions
Imports System.Text.RegularExpressions

Partial Class ResettaPasswordUtentePage
    Inherits System.Web.UI.Page

    Private Class IRadListBoxItemComparer
        Implements IEqualityComparer(Of Telerik.Web.UI.RadListBoxItem)

        Public Function IEqualityComparer_Equals(ByVal x As Telerik.Web.UI.RadListBoxItem, ByVal y As Telerik.Web.UI.RadListBoxItem) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of Telerik.Web.UI.RadListBoxItem).Equals
            Return x.Value = y.Value

        End Function

        Public Function IEqualityComparer_GetHashCode(ByVal obj As Telerik.Web.UI.RadListBoxItem) As Integer Implements System.Collections.Generic.IEqualityComparer(Of Telerik.Web.UI.RadListBoxItem).GetHashCode
            Return obj.GetHashCode
        End Function


    End Class

    Private WithEvents MainPage As MainPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Resetta Password Utente"
        Me.PasswordTextBox.Text = ParsecUtility.Utility.GeneraPassword
    End Sub


    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.TitoloLabel.Text = "Utenti " & If(Me.UtentiListBox.Items.Count > 0, "( " & Me.UtentiListBox.Items.Count.ToString & " )", "")
    End Sub


    Protected Sub TrovaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles TrovaUtenteImageButton.Click
        Dim pageUrl As String = "~/UI/Amministrazione/pages/search/RicercaUtentePage.aspx"
        Dim queryString As New Hashtable
        queryString.Add("obj", Me.AggiornaUtenteImageButton.ClientID)
        ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, queryString, False)
        Dim ht As New Hashtable
        ht("tipoSelezione") = 1 'multipla
        Session("Parametri") = ht
    End Sub

    Protected Sub AggiornaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AggiornaUtenteImageButton.Click
        If Not Session("SelectedUsers") Is Nothing Then
            Dim utentiSelezionati As SortedList(Of Integer, String) = Session("SelectedUsers")
            Dim users As New ParsecAdmin.UserRepository
            For Each utente In utentiSelezionati
                Dim ute = users.GetUserById(utente.Key).FirstOrDefault
                Dim descrizione = (If(ute.Username = Nothing, "", ute.Username) + " - " + If(ute.Cognome = Nothing, "", ute.Cognome) + " " + If(ute.Nome = Nothing, "", ute.Nome))
                Dim item As New Telerik.Web.UI.RadListBoxItem(descrizione, ute.Id)
                If Not Me.UtentiListBox.Items.Contains(item, New IRadListBoxItemComparer) Then
                    Me.UtentiListBox.Items.Add(item)
                End If
            Next
            users.Dispose()
        End If
        Session("SelectedUsers") = Nothing
    End Sub


    Private Sub DeleteSelectedItems(ByVal list As Telerik.Web.UI.RadListBox)
        For Each item As Telerik.Web.UI.RadListBoxItem In list.CheckedItems
            list.Items.Remove(item)
        Next
    End Sub

  

    Protected Sub EliminaUtenteImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EliminaUtenteImageButton.Click
        Me.DeleteSelectedItems(Me.UtentiListBox)
    End Sub


    Private Sub Save()
        If Me.UtentiListBox.Items.Count = 0 Then
            Throw New ApplicationException("E' necessario selezionare almeno un'utente!")
        Else
            Dim utenti As New ParsecAdmin.UserRepository
            Dim utente As ParsecAdmin.Utente = Nothing
            Dim utentiDaResettare As New List(Of ParsecAdmin.Utente)
            For i As Integer = 0 To Me.UtentiListBox.Items.Count - 1
                Dim idUtente As Integer = CInt(Me.UtentiListBox.Items(i).Value)
                utente = utenti.GetUserById(idUtente).FirstOrDefault
                If Not utente Is Nothing Then
                    'Questo obbliga l'utente a modifica la password resettata
                    utente.PswNonSettata = True
                    utente.DataUltimoSettaggioPsw = Now
                    utente.PasswordHash = ParsecUtility.Utility.CalcolaHash(Me.PasswordTextBox.Text)
                    utente.Password = Me.PasswordTextBox.Text
                    utentiDaResettare.Add(utente)
                End If
            Next
            Try
                utenti.ResetPassword(utentiDaResettare)
            Catch ex As Exception
                Throw New ApplicationException(ex.Message)
            Finally
                utenti.Dispose()
            End Try
        End If
    End Sub

    Protected Sub SalvaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalvaButton.Click
        Dim message As String = String.Empty
        Try
            Me.Save()
            Me.PasswordTextBox.Text = ParsecUtility.Utility.GeneraPassword
            Me.UtentiListBox.Items.Clear()
        Catch ex As ApplicationException
            message = ex.Message
        End Try
        If String.IsNullOrEmpty(message) Then
            Me.infoOperazioneHidden.Value = "Operazione conclusa con successo!"
        Else
            ParsecUtility.Utility.MessageBox(message, False)
        End If

    End Sub

End Class