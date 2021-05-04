Imports ParsecAdmin
Imports System.Transactions

Partial Class TitolarioPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

    Private temp As List(Of ParsecAdmin.TitolarioClassificazione)
    Private nodeInfos As List(Of ParsecAdmin.TitolarioClassificazione)

    Public Property Titolario() As ParsecAdmin.TitolarioClassificazione
        Get
            Return CType(Session(CStr(ViewState("Titolario_Ticks"))), ParsecAdmin.TitolarioClassificazione)
        End Get
        Set(ByVal value As ParsecAdmin.TitolarioClassificazione)
            If ViewState("Titolario_Ticks") Is Nothing Then
                ViewState("Titolario_Ticks") = "Titolario_" & Now.Ticks.ToString
            End If
            Session(CStr(ViewState("Titolario_Ticks"))) = value
        End Set
    End Property


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Titolario Classificazione"
        Me.BuildTree(Nothing, Nothing)
    End Sub


    Public Function GetInfoNodes(ByVal descrizione As String) As List(Of ParsecAdmin.TitolarioClassificazione)
        Dim res As List(Of ParsecAdmin.TitolarioClassificazione) = (New ParsecAdmin.TitolarioClassificazioneRepository).GetClassificazioni(descrizione)
        Return res
    End Function


    Private Sub BuildTree(ByVal idToSelect As String, ByVal descr As String)

        Me.nodeInfos = (New ParsecAdmin.TitolarioClassificazioneRepository).GetClassificazioni(Nothing)
        If descr Is Nothing Then
            Session("nodeInfos") = Me.nodeInfos
        Else
            Session("nodeInfos") = Me.FilterNodes(Me.nodeInfos, descr)
            Me.nodeInfos = Session("nodeInfos")
        End If

        Me.ClassificazioniTreeView.Nodes.Clear()
        Dim rootNode As New Telerik.Web.UI.RadTreeNode("Titolario", 0)
        Me.ClassificazioniTreeView.Nodes.Add(rootNode)
        Me.AddNode(Me.nodeInfos, rootNode, idToSelect)
        Me.ClassificazioniTreeView.ExpandAllNodes()
    End Sub



    Private Function FilterNodes(ByVal nodeInfos As List(Of ParsecAdmin.TitolarioClassificazione), ByVal descr As String) As List(Of ParsecAdmin.TitolarioClassificazione)
        Dim result As New List(Of ParsecAdmin.TitolarioClassificazione)
        Dim filteredNodes As List(Of ParsecAdmin.TitolarioClassificazione) = Me.GetInfoNodes(descr)
        Dim parent As ParsecAdmin.TitolarioClassificazione = Nothing
        For Each node As ParsecAdmin.TitolarioClassificazione In filteredNodes
            Dim parentId As Integer = node.IdPadre
            parent = nodeInfos.Where(Function(c) c.Id = parentId).FirstOrDefault
            If Not result.Contains(node) Then
                result.Add(node)
            End If
            While Not parent Is Nothing
                If Not result.Contains(parent) Then
                    result.Add(parent)
                End If
                parent = nodeInfos.Where(Function(c) c.Id = parent.IdPadre).FirstOrDefault
            End While
        Next
        Return result
    End Function


    Private Sub AddNode(ByVal nodes As List(Of ParsecAdmin.TitolarioClassificazione), ByVal t As Telerik.Web.UI.RadTreeNode, ByVal idToSelect As String)
        Me.temp = Me.nodeInfos.Where(Function(c) c.IdPadre = CInt(t.Value)).ToList
        For Each node As ParsecAdmin.TitolarioClassificazione In temp
            Dim Id As Integer = node.Id
            Dim childItem As New Telerik.Web.UI.RadTreeNode(node.CodificaCompleta, node.Id)
            t.Nodes.Add(childItem)
            childItem.ToolTip = "Codice class.: " & node.Codice.ToString & " Ordinale: " & node.Ordinale.ToString
            Me.temp = Me.nodeInfos.Where(Function(c) c.IdPadre = Id).ToList
            If Not idToSelect Is Nothing Then
                If node.Id = CInt(idToSelect) Then
                    childItem.Selected = True
                End If
            End If
            Me.AddNode(nodes, childItem, idToSelect)
        Next
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il nodo del titolario selezionato?", False, Not Me.Titolario Is Nothing)
    End Sub


    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Dim message As String = "Operazione conclusa con successo!"

        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()

            Case "Salva"
                Try
                    Me.Save()
                    Me.BuildTree(Me.Titolario.Id, Nothing)
                Catch ex As ApplicationException
                    message = ex.Message
                End Try
                ParsecUtility.Utility.MessageBox(message, False)

            Case "Nuovo"

                'Seleziono il sotto-livello
                Dim livello As New ParsecAdmin.TitolarioClassificazioneLivello
                If Not Me.Titolario Is Nothing Then


                    livello = (New ParsecAdmin.TitolarioClassificazioneLivelloRepository).GetQuery.Where(Function(c) c.Id = Me.Titolario.IdGerarchia + 1).FirstOrDefault
                    'Non ci sono altri sotto-livelli
                    If livello Is Nothing Then
                        ParsecUtility.Utility.MessageBox("Non è possibile inserire sotto-nodi a questo livello!", False)
                        Exit Sub
                    End If
                Else
                    Dim id As Integer = (New ParsecAdmin.TitolarioClassificazioneLivelloRepository).GetIdPrimoLivello
                    livello = (New ParsecAdmin.TitolarioClassificazioneLivelloRepository).GetQuery.Where(Function(c) c.Id = id).FirstOrDefault
                End If

                Me.ResettaVista()

                Me.DescrizioneLivelloTextBox.Text = livello.Descrizione
                Me.IdLivelloTextBox.Text = livello.Id



            Case "Annulla"
                Me.BuildTree(Nothing, Nothing)
                Me.ResettaVista()

            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.Titolario Is Nothing Then
                        Try
                            Me.Delete()
                            Me.ResettaVista()
                            Me.BuildTree(Nothing, Nothing)
                        Catch ex As Exception
                            message = ex.Message
                        End Try
                    Else
                        message = "E' necessario selezionare un nodo del titolario!"
                    End If
                    ParsecUtility.Utility.MessageBox(message, False)

                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.BuildTree(Nothing, Me.DescrizioneTextBox.Text)
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
        Dim titolario As New ParsecAdmin.TitolarioClassificazioneRepository
        Try
            titolario.Delete(Me.Titolario)
            titolario.Dispose()
        Catch ex As Exception
            titolario.Dispose()
            Throw New ApplicationException(ex.Message)
        End Try
    End Sub

    Private Sub Ordina()
        Dim titolario As New ParsecAdmin.TitolarioClassificazioneRepository
        Dim curTitolario As ParsecAdmin.TitolarioClassificazione = titolario.GetQuery.Where(Function(c) c.Id = Me.Titolario.Id).FirstOrDefault
        Dim direzione As ParsecAdmin.TitolarioClassificazione.DirezioneOrdinamento = Me.Titolario.Ordinamento

        Dim message As String = ""
        Dim processa As Boolean = True
        If Not Me.ClassificazioniTreeView.SelectedNode Is Nothing Then
            If Me.ClassificazioniTreeView.SelectedNode.Value <> "0" Then
                Dim curOrdinale As Integer = curTitolario.Ordinale
                Dim maxOrdinale As Integer = titolario.GetMaxMinOrdinale(curTitolario.IdPadre, True)
                Dim minOrdinale As Integer = titolario.GetMaxMinOrdinale(curTitolario.IdPadre, False)
                If direzione = TitolarioClassificazione.DirezioneOrdinamento.Decremento Then
                    If curOrdinale = maxOrdinale Then processa = False
                End If
                If direzione = TitolarioClassificazione.DirezioneOrdinamento.Incremento Then
                    If curOrdinale = minOrdinale Then processa = False
                End If
                If processa Then
                    Dim destTitolario As ParsecAdmin.TitolarioClassificazione = Nothing
                    If direzione = TitolarioClassificazione.DirezioneOrdinamento.Incremento Then
                        destTitolario = titolario.GetTitolarioPrecedente(curTitolario.IdPadre, curOrdinale)
                    End If
                    If direzione = TitolarioClassificazione.DirezioneOrdinamento.Decremento Then
                        destTitolario = titolario.GetTitolarioSuccessivo(curTitolario.IdPadre, curOrdinale)
                    End If
                    curTitolario.Ordinale = destTitolario.Ordinale
                    destTitolario.Ordinale = curOrdinale
                    titolario.SaveChanges()
                    Me.BuildTree(curTitolario.Id, Nothing)
                End If
            Else
                message = "Non è possibile spostare il nodo radice del titolario!"
            End If
        Else
            message = "E' necessario selezionare la voce di titolario che si desidera riposizionare!"
        End If
        If Not String.IsNullOrEmpty(message) Then
            ParsecUtility.Utility.MessageBox(message, False)
        End If

    End Sub



    Private Sub Save()
        If Me.ClassificazioniTreeView.SelectedNode Is Nothing Then
            Throw New ApplicationException("E' necessario selezionare una voce di titolario!")
            Return
        End If

        Dim titolario As New ParsecAdmin.TitolarioClassificazioneRepository
        Dim nodoTitolario As New ParsecAdmin.TitolarioClassificazione


        '******************************************************************************************
        'Inserisco sempre una nuova voce di titolario.
        '******************************************************************************************
        If Me.Titolario Is Nothing Then
            nodoTitolario.Codice = titolario.GetNuovoCodice
            nodoTitolario.IdGerarchia = If(String.IsNullOrEmpty(Me.IdLivelloTextBox.Text), 0, CInt(Me.IdLivelloTextBox.Text))
            nodoTitolario.IdPadre = CInt(Me.ClassificazioniTreeView.SelectedNode.Value)
            nodoTitolario.Ordinale = titolario.GetMaxOrdinale(nodoTitolario.IdPadre)
        Else
            Dim tit As ParsecAdmin.TitolarioClassificazione = titolario.GetQuery.Where(Function(c) c.Id = Me.Titolario.Id).FirstOrDefault
            nodoTitolario.Codice = tit.Codice
            nodoTitolario.IdGerarchia = tit.IdGerarchia
            nodoTitolario.IdPadre = tit.IdPadre
            nodoTitolario.Ordinale = tit.Ordinale
        End If

        nodoTitolario.Codifica = Me.CodificaTextBox.Text
        nodoTitolario.Descrizione = Me.DescrizioneTextBox.Text.ToUpper
        nodoTitolario.LogDataRegistrazione = Now


        Try

            titolario.Titolario = Me.Titolario

            titolario.Save(nodoTitolario)
            Me.Titolario = titolario.Titolario

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            titolario.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.Titolario = Nothing
        Me.CodificaTextBox.Text = String.Empty
        Me.DescrizioneLivelloTextBox.Text = String.Empty
        Me.DescrizioneTextBox.Text = String.Empty
        Me.IdLivelloTextBox.Text = String.Empty
    End Sub


    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub


    Protected Sub ClassificazioniTreeView_NodeClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles ClassificazioniTreeView.NodeClick
        Me.AggiornaVista(e)
    End Sub


    Private Sub AggiornaVista(ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs)
        If e.Node.Value <> "0" Then
            Me.ResettaVista()
            Dim id As Integer = e.Node.Value
            Dim nodeInfos As List(Of ParsecAdmin.TitolarioClassificazione) = Session("nodeInfos")
            Me.Titolario = nodeInfos.Where(Function(c) c.Id = id).FirstOrDefault

            Me.CodificaTextBox.Text = Me.Titolario.Codifica
            Me.DescrizioneLivelloTextBox.Text = Me.Titolario.DescrizioneLivello
            Me.DescrizioneTextBox.Text = Me.Titolario.Descrizione

        Else
            Me.ResettaVista()
        End If

    End Sub


    Protected Sub SpostaSuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SpostaSuButton.Click
        If Not Me.Titolario Is Nothing Then
            Me.Titolario.Ordinamento = TitolarioClassificazione.DirezioneOrdinamento.Incremento
            Ordina()
        End If

    End Sub

    Protected Sub SpostaGiuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SpostaGiuButton.Click
        If Not Me.Titolario Is Nothing Then
            Me.Titolario.Ordinamento = TitolarioClassificazione.DirezioneOrdinamento.Decremento
            Ordina()
        End If

    End Sub

    Protected Sub ClassificazioniTreeView_NodeDrop(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeDragDropEventArgs) Handles ClassificazioniTreeView.NodeDrop

        Dim titolario As New ParsecAdmin.TitolarioClassificazioneRepository
        Dim sourceNode As Telerik.Web.UI.RadTreeNode = e.SourceDragNode
        Dim destNode As Telerik.Web.UI.RadTreeNode = e.DestDragNode
        Dim dropPosition As Telerik.Web.UI.RadTreeViewDropPosition = e.DropPosition

        If Not destNode Is Nothing Then
            If Not sourceNode.Equals(destNode) Then
                Dim sourceTitolario As ParsecAdmin.TitolarioClassificazione = titolario.GetQuery.Where(Function(c) c.Id = CInt(sourceNode.Value)).FirstOrDefault
                Dim destTitolario As ParsecAdmin.TitolarioClassificazione = titolario.GetQuery.Where(Function(c) c.Id = CInt(destNode.Value)).FirstOrDefault
                Dim processa As Boolean = False
                Select Case dropPosition
                    Case Telerik.Web.UI.RadTreeViewDropPosition.Over
                        processa = True
                End Select

                If sourceTitolario.IdPadre <> destTitolario.IdPadre Then
                    processa = False
                End If
                If processa Then
                    Dim curOrdinale As Integer = sourceTitolario.Ordinale
                    sourceTitolario.Ordinale = destTitolario.Ordinale
                    destTitolario.Ordinale = curOrdinale
                    titolario.SaveChanges()
                    Me.BuildTree(sourceTitolario.Id, Nothing)
                End If

            End If

        End If



    End Sub
End Class