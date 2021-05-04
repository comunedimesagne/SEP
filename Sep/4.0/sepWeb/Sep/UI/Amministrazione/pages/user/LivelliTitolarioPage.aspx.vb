Imports ParsecAdmin
Imports System.Transactions

Partial Class LivelliTitolarioPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

    Private temp As List(Of ParsecAdmin.TitolarioClassificazioneLivello)
    Private nodeInfos As List(Of ParsecAdmin.TitolarioClassificazioneLivello)

    Public Property LivelloTitolario() As ParsecAdmin.TitolarioClassificazioneLivello
        Get
            Return CType(Session(CStr(ViewState("Livello_Ticks"))), ParsecAdmin.TitolarioClassificazioneLivello)
        End Get
        Set(ByVal value As ParsecAdmin.TitolarioClassificazioneLivello)
            If ViewState("Livello_Ticks") Is Nothing Then
                ViewState("Livello_Ticks") = "Livello_" & Now.Ticks.ToString
            End If
            Session(CStr(ViewState("Livello_Ticks"))) = value
        End Set
    End Property


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Livelli Titolario"
        Me.BuildTree(Nothing)
    End Sub


    Private Sub BuildTree(ByVal idToSelect As String)
        Me.nodeInfos = (New ParsecAdmin.TitolarioClassificazioneLivelloRepository).GetQuery.OrderBy(Function(c) c.Id).ToList
        Me.LivelliClassificazioniTreeView.Nodes.Clear()
        Dim rootNode As New Telerik.Web.UI.RadTreeNode("Livelli Titolario", 0)
        Me.LivelliClassificazioniTreeView.Nodes.Add(rootNode)
        Me.AddNode(Me.nodeInfos, rootNode, idToSelect)
        Me.LivelliClassificazioniTreeView.ExpandAllNodes()
    End Sub


    Private Sub AddNode(ByVal nodes As List(Of ParsecAdmin.TitolarioClassificazioneLivello), ByVal t As Telerik.Web.UI.RadTreeNode, ByVal idToSelect As String)
        Me.temp = Me.nodeInfos.Where(Function(c) c.IdPadre = CInt(t.Value)).ToList
        For Each node As ParsecAdmin.TitolarioClassificazioneLivello In temp
            Dim Id As Integer = node.Id
            Dim childItem As New Telerik.Web.UI.RadTreeNode(node.Descrizione, node.Id)
            t.Nodes.Add(childItem)
            childItem.ToolTip = "Codice livello: " & node.Codice.ToString
            Me.temp = Me.nodeInfos.Where(Function(c) c.IdPadre = Id).ToList
            If Not idToSelect Is Nothing Then
                If node.Codice = CInt(idToSelect) Then
                    childItem.Selected = True
                End If
            End If
            Me.AddNode(nodes, childItem, idToSelect)
        Next
    End Sub

 
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il livello selezionato?", False, Not Me.LivelloTitolario Is Nothing)
    End Sub


    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()

                    Me.BuildTree(Me.LivelloTextBox.Text)
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)
            Case "Nuovo"
                Me.ResettaVista()
                Me.LivelloTextBox.Text = (New ParsecAdmin.TitolarioClassificazioneLivelloRepository).GetNuovoCodice.ToString
             Case "Annulla"
                Me.ResettaVista()
             Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.LivelloTitolario Is Nothing Then
                        Me.Delete()
                        Me.ResettaVista()
                        Me.BuildTree(Nothing)
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un livello!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                'Dim avvisi As New ParsecAdmin.AvvisiRepository
                'Dim avviso As New ParsecAdmin.Avviso
                'avviso.Contenuto = Me.ContenutoTextBox.Text
                'Me.Avvisi = avvisi.GetAvvisi(avviso)
                'Me.AvvisiGridView.Rebind()
                'avvisi.Dispose()
        End Select
    End Sub

    Private Sub Print()
        'Dim parametriStampa As New Hashtable
        'parametriStampa.Add("TipologiaStampa", "StampaQualificheOrganigramma")
        'parametriStampa.Add("DatiStampa", Me.Avvisi)
        'Session("ParametriStampa") = parametriStampa
        'Dim pageUrl As String = "~/UI/Amministrazione/pages/user/StampaPage.aspx"
        'ParsecUtility.Utility.ShowPopup(pageUrl, 800, 600, Nothing, False)
    End Sub

    Private Sub Delete()
        Dim livelliTitolario As New ParsecAdmin.TitolarioClassificazioneLivelloRepository
        livelliTitolario.Delete(Me.LivelloTitolario)
       livelliTitolario.Dispose()
    End Sub


    Private Sub Save()

        If Me.LivelliClassificazioniTreeView.SelectedNode Is Nothing Then
            Throw New ApplicationException("E' necessario selezionare un livello di titolario!")
            Return
        End If

        Dim livelliTitolario As New ParsecAdmin.TitolarioClassificazioneLivelloRepository
        Dim livello As ParsecAdmin.TitolarioClassificazioneLivello = Nothing
        If Me.LivelloTitolario Is Nothing Then
            livello = New ParsecAdmin.TitolarioClassificazioneLivello
            livello.Codice = livelliTitolario.GetNuovoCodice
            livello.IdPadre = CInt(Me.LivelliClassificazioniTreeView.SelectedNode.Value)
        Else
            livello = livelliTitolario.GetQuery.Where(Function(c) c.Codice = Me.LivelloTitolario.Id).FirstOrDefault
        End If

        livello.Descrizione = Me.DescrizioneTextBox.Text
        livello.CarattereSeparatore = Me.CarattereTextBox.Text
        livello.UrlIcona = If(Me.AllegatoUpload.UploadedFiles.Count > 0, Me.AllegatoUpload.UploadedFiles(0).FileName, "")


        Try
            livelliTitolario.Save(livello)
            If Me.AllegatoUpload.UploadedFiles.Count > 0 Then
                Dim file As Telerik.Web.UI.UploadedFile = Me.AllegatoUpload.UploadedFiles(0)
                If file.FileName <> "" Then
                    'Dim pathDownload As String = System.Configuration.ConfigurationManager.AppSettings("PathDownload")
                    'Dim pathAvviso As String = System.Configuration.ConfigurationManager.AppSettings("PathAvviso")
                    'file.SaveAs(pathDownload & file.FileName)
                    'manuale.NomeFile = file.FileName
                End If
            End If

            Me.LivelloTitolario = livelliTitolario.LivelloTitolario
            Me.LivelloTextBox.Text = Me.LivelloTitolario.Codice
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            livelliTitolario.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.LivelloTitolario = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
        Me.LivelloTextBox.Text = String.Empty
        Me.CarattereTextBox.Text = String.Empty
    End Sub


    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub


    Protected Sub LivelliClassificazioniTreeView_NodeClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles LivelliClassificazioniTreeView.NodeClick
        Me.AggiornaVista(e)
    End Sub

 
    Private Sub AggiornaVista(ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs)
        If e.Node.Value <> "0" Then
            Me.ResettaVista()
            Dim id As Integer = e.Node.Value
            Dim livelliTitolario As New ParsecAdmin.TitolarioClassificazioneLivelloRepository
            Me.LivelloTitolario = livelliTitolario.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault

            Me.DescrizioneTextBox.Text = Me.LivelloTitolario.Descrizione
            Me.LivelloTextBox.Text = Me.LivelloTitolario.Codice
            Me.CarattereTextBox.Text = Me.LivelloTitolario.CarattereSeparatore
            livelliTitolario.Dispose()
        Else
            Me.ResettaVista()
        End If

    End Sub


End Class