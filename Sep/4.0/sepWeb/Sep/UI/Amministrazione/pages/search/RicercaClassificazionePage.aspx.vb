Imports ParsecAdmin

Partial Class RicercaClassificazionePage
    Inherits System.Web.UI.Page

    Private nodeInfos As List(Of ParsecAdmin.TitolarioClassificazione)

    Private temp As List(Of ParsecAdmin.TitolarioClassificazione)

    Private enabledNodes As New List(Of ParsecAdmin.TitolarioClassificazione)

    Private ultimoLivelloStruttura As Integer = 400

    'Private livelliSelezionabili As String = "1,2"

    Private livelliSelezionabili As String = "100,200,300,400"

    Private Property Filtro As String
        Get
            Return ViewState("Filtro")
        End Get
        Set(ByVal value As String)
            ViewState("Filtro") = value
        End Set
    End Property

    Private Property NomeModulo As String
        Get
            Return ViewState("NomeModulo")
        End Get
        Set(ByVal value As String)
            ViewState("NomeModulo") = value
        End Set
    End Property

    Private Property TipoSelezione As Integer '(0 - Singola; 1 - Multipla)
        Get
            Return ViewState("TipoSelezione")
        End Get
        Set(ByVal value As Integer)
            ViewState("TipoSelezione") = value
        End Set
    End Property

    Private Property IdUtente As Integer
        Get
            Return ViewState("IdUtente")
        End Get
        Set(ByVal value As Integer)
            ViewState("IdUtente") = value
        End Set
    End Property

    Private Property IdModulo As Integer
        Get
            Return ViewState("IdModulo")
        End Get
        Set(ByVal value As Integer)
            ViewState("IdModulo") = value
        End Set
    End Property

    Private Sub GetParametri()
        Me.TipoSelezione = 1
        If Not ParsecUtility.SessionManager.ParametriPagina Is Nothing Then
            Dim parametriPagina As Hashtable = ParsecUtility.SessionManager.ParametriPagina
            If parametriPagina.ContainsKey("tipoSelezione") Then
                Me.TipoSelezione = parametriPagina("tipoSelezione")
            End If
            If parametriPagina.ContainsKey("livelliSelezionabili") Then
                Me.livelliSelezionabili = parametriPagina("livelliSelezionabili")
            End If
            If parametriPagina.ContainsKey("nomeModulo") Then
                Me.NomeModulo = parametriPagina("nomeModulo")
            End If
            If parametriPagina.ContainsKey("idUtente") Then
                Me.IdUtente = parametriPagina("idUtente")
            End If
            If parametriPagina.ContainsKey("idModulo") Then
                Me.IdModulo = parametriPagina("idModulo")
            End If
            If parametriPagina.ContainsKey("filtro") Then
                Me.Filtro = parametriPagina("filtro")
                Me.FiltroTextBox.Text = Me.Filtro
            End If
            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.GetParametri()
            If Me.TipoSelezione = 0 Then
                Me.ConfermaButton.Visible = False
            End If
            Session("nodeInfos") = Me.GetInfoNodes(Nothing)
            Me.LoadTree()
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

    Private Sub LoadTree()
        If String.IsNullOrEmpty(Me.FiltroTextBox.Text) Then
            Me.nodeInfos = Session("nodeInfos")
        Else
            Me.nodeInfos = Me.FilterNodes(Session("nodeInfos"))
        End If
        Me.BuildTree()
    End Sub

    Private Sub BuildTree()
        Dim parametri As New ParsecAdmin.ParametriRepository
        Dim parametro = parametri.GetByName("EspandiAlberoClassificazione", ParsecAdmin.TipoModulo.SEP)
        parametri.Dispose()
        Me.ClassificazioniTreeView.Nodes.Clear()
        Me.temp = Me.nodeInfos.Where(Function(c) c.IdPadre = 0).ToList
        Dim rootNode As New Telerik.Web.UI.RadTreeNode("Titolario", 0)
        Me.ClassificazioniTreeView.Nodes.Add(rootNode)
        rootNode.Checkable = False
        rootNode.Expanded = True
        Me.ClassificazioniTreeView.CheckBoxes = (TipoSelezione = 1)
        Me.AddNode(Me.temp, rootNode)

        If Me.FiltroTextBox.Text.Length > 0 Then
            Me.ClassificazioniTreeView.ExpandAllNodes()
            Me.ToggleNodeImageButton.ImageUrl = "~/images/collapse.png"
        Else
            If Not parametro Is Nothing Then
                If parametro.Valore = "1" Then
                    Me.ClassificazioniTreeView.ExpandAllNodes()
                    Me.ToggleNodeImageButton.ImageUrl = "~/images/collapse.png"
                    Me.ToggleNodeImageButton.ToolTip = "Comprimi tutto"
                Else
                    Me.ToggleNodeImageButton.ImageUrl = "~/images/expand.png"
                    Me.ToggleNodeImageButton.ToolTip = "Espandi tutto"
                End If
            End If
        End If

    End Sub

    Public Function GetInfoNodes(ByVal descrizione As String) As List(Of ParsecAdmin.TitolarioClassificazione)
        Dim res As List(Of ParsecAdmin.TitolarioClassificazione) = (New ParsecAdmin.TitolarioClassificazioneRepository).GetClassificazioni(descrizione)
        Return res
    End Function

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Me.SelezionaClassificazione()
    End Sub

    Private Function GetStruttureAbilitate(ByVal idUtente As Integer, ByVal idModulo As Integer) As List(Of Integer)
        Dim res As List(Of Integer)
        Dim abilitazioniUtenteStruttura = (New ParsecAdmin.AbilitazioneUtenteStrutturaRepository).GetQuery.Where(Function(c) c.IdUtente = idUtente AndAlso c.IdModulo = idModulo AndAlso c.LogTipoOperazione Is Nothing).ToList
        Dim strutture = (New ParsecAdmin.StructureRepository).GetQuery.Where(Function(c) c.LogStato Is Nothing).ToList
        res = (From aus In abilitazioniUtenteStruttura Join s In strutture On aus.CodiceStruttura Equals s.Codice Select s.Id).ToList
        Return res
    End Function

    Private Sub SelezionaClassificazione()
        Dim ids As New List(Of Integer)
        Dim titolario As New ParsecAdmin.TitolarioClassificazioneRepository
        Dim classificazioni As List(Of ParsecAdmin.TitolarioClassificazione) = titolario.GetClassificazioni(Nothing)
        If Me.TipoSelezione = 1 Then
            For Each node As Telerik.Web.UI.RadTreeNode In Me.ClassificazioniTreeView.CheckedNodes
                ids.Add(CInt(node.Value))
            Next
        Else
            ids.Add(CInt(Me.ClassificazioniTreeView.SelectedNode.Value))
        End If
        Dim res = From c In classificazioni
                   Join i In ids On c.Id Equals i
                    Order By c.Ordinale
                    Select c

        Session("ClassificazioniSelezionate") = res.ToList
        ParsecUtility.Utility.ClosePopup(True)
        titolario.Dispose()
    End Sub

    Private Function FilterNodes(ByVal nodeInfos As List(Of ParsecAdmin.TitolarioClassificazione)) As List(Of ParsecAdmin.TitolarioClassificazione)
        Dim result As New List(Of ParsecAdmin.TitolarioClassificazione)
        Dim filteredNodes As List(Of ParsecAdmin.TitolarioClassificazione) = Me.GetInfoNodes(Me.FiltroTextBox.Text)
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

    Protected Sub FiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltroImageButton.Click
        Me.LoadTree()
    End Sub

    Private Sub AddNode(ByVal nodes As List(Of ParsecAdmin.TitolarioClassificazione), ByVal t As Telerik.Web.UI.RadTreeNode)
        For Each node As ParsecAdmin.TitolarioClassificazione In nodes
            If node.IdGerarchia <= ultimoLivelloStruttura Then
                Dim Id As Integer = node.Id
                Dim childItem As New Telerik.Web.UI.RadTreeNode(node.CodificaCompleta, node.Id)

                If Not Me.livelliSelezionabili.Contains(node.IdGerarchia) Then

                    childItem.Checkable = False
                    childItem.Enabled = True
                    childItem.Attributes.Add("nonselezionabile", "")

                    childItem.ToolTip = "Codice class.: " & node.Codice.ToString & " (NON SELEZIONABILE)"
                Else
                    childItem.ToolTip = "Codice class.: " & node.Codice.ToString
                End If

                t.Nodes.Add(childItem)
                childItem.ImageUrl = node.UrlIcona
                Me.temp = Me.nodeInfos.Where(Function(c) c.IdPadre = Id).ToList
                Me.AddNode(Me.temp, childItem)
            End If
        Next
    End Sub

    Private Sub RecursiveBuildNode(ByVal filteredNodes As List(Of NodeItemInfo), ByVal result As List(Of NodeItemInfo), ByVal nodes As List(Of NodeItemInfo))
        For Each node As NodeItemInfo In filteredNodes
            Dim Id As Integer = node.Id

            '*********************************************************************************************
            '  Costruisco i nodi superiori a partire dal nodo corrente.
            '*********************************************************************************************
            Dim parent As ParsecAdmin.NodeItemInfo = Nothing
            Dim parentId As Integer = node.ParentID
            parent = nodes.Where(Function(c) c.Id = parentId).FirstOrDefault
            While Not parent Is Nothing
                If Not result.Contains(parent) Then
                    result.Add(parent)
                    'Lo disabilito se non è presente nella lista dei nodi abilitati.
                    If Not filteredNodes.Contains(parent) Then
                        parent.Enabled = False
                    End If
                End If
                parent = nodes.Where(Function(c) c.Id = parent.ParentID).FirstOrDefault
            End While
            '*********************************************************************************************

            '*********************************************************************************************
            '  Costruisco i nodi inferiori a partire dal nodo corrente se non sono stati già inseriti.
            '*********************************************************************************************
            If Not result.Contains(node) Then
                result.Add(node)
            End If
            '*********************************************************************************************

            Me.RecursiveBuildNode(nodes.Where(Function(c) c.ParentID = Id).ToList, result, nodes)
        Next
    End Sub

    Protected Sub ClassificazioniTreeView_NodeClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles ClassificazioniTreeView.NodeClick
        If Me.TipoSelezione = 0 Then
            If e.Node.Value <> "0" Then
                SelezionaClassificazione()
            End If
        End If
    End Sub

    Protected Sub ToggleNodeImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ToggleNodeImageButton.Click
        If Me.ToggleNodeImageButton.ImageUrl = "~/images/collapse.png" Then
            Dim rootNode = Me.ClassificazioniTreeView.FindNodeByText("Titolario")

            Me.ClassificazioniTreeView.CollapseAllNodes()
            Me.ToggleNodeImageButton.ImageUrl = "~/images/expand.png"
            Me.ToggleNodeImageButton.ToolTip = "Espandi tutto"
            rootNode.Expanded = True

        Else
            Me.ClassificazioniTreeView.ExpandAllNodes()
            Me.ToggleNodeImageButton.ImageUrl = "~/images/collapse.png"
            Me.ToggleNodeImageButton.ToolTip = "Comprimi tutto"
        End If

    End Sub
End Class