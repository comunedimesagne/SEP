Imports ParsecAdmin

Partial Class RicercaOrganigrammaPage
    Inherits System.Web.UI.Page

    Private nodeInfos As List(Of ParsecAdmin.NodeItemInfo)
    Private temp As List(Of NodeItemInfo)
    Private enabledNodes As New List(Of NodeItemInfo)
    Private ultimoLivelloStruttura As Integer = 400
    Private livelliSelezionabili As String = "100,200,300,400"

#Region "PROPRIETA'"


    Private Property ApplicaAbilitazioni As Boolean
        Get
            Return ViewState("ApplicaAbilitazioni")
        End Get
        Set(ByVal value As Boolean)
            ViewState("ApplicaAbilitazioni") = value
        End Set
    End Property

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

#End Region

#Region "EVENTI PAGINA"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'todo passare i parametri

        If Not Me.IsPostBack Then
            Me.GetParametri()
            Session("nodeInfos") = Me.GetInfoNodes("")
            Me.LoadTree()
        End If

        If Me.TipoSelezione = 0 Then
            Me.ConfermaButton.Visible = False
            AddHandler Me.StruttureTreeView.NodeClick, AddressOf StruttureTreeView_NodeClick
        Else
            Me.StruttureTreeView.OnClientDoubleClick = "DoubleClick"

        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.ChiudiButton.Attributes.Add("onclick", "window.close();return false;")
    End Sub

#End Region

#Region "METODI PRIVATI"

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
            If parametriPagina.ContainsKey("ultimoLivelloStruttura") Then
                Me.ultimoLivelloStruttura = parametriPagina("ultimoLivelloStruttura")
            End If
            If parametriPagina.ContainsKey("Filtro") Then
                Me.Filtro = parametriPagina("Filtro")
                Me.FiltroTextBox.Text = Me.Filtro
            End If

            If parametriPagina.ContainsKey("ApplicaAbilitazioni") Then
                Me.ApplicaAbilitazioni = parametriPagina("ApplicaAbilitazioni")
            Else
                Me.ApplicaAbilitazioni = True
            End If

            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
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
        Me.StruttureTreeView.Nodes.Clear()

        Dim clientRepository As New ClientRepository
        Dim clienteCorrente As ParsecAdmin.Cliente = clientRepository.GetQuery.FirstOrDefault
        clientRepository.Dispose()

        Dim rootNode As New Telerik.Web.UI.RadTreeNode(clienteCorrente.Descrizione, 0)
        rootNode.Checkable = False
        rootNode.Expanded = True
        Me.StruttureTreeView.Nodes.Add(rootNode)

        If Not Me.nodeInfos Is Nothing AndAlso Me.nodeInfos.Count > 0 Then
            Me.temp = Me.nodeInfos.Where(Function(c) c.ParentID = 0).ToList

            Me.StruttureTreeView.CheckBoxes = (TipoSelezione = 1)
            Me.AddNode(Me.temp, rootNode)
            If Me.FiltroTextBox.Text.Length > 0 Then
                Me.StruttureTreeView.ExpandAllNodes()
            End If

            Me.FiltroImageButton.Enabled = True
            Me.FiltroTextBox.Enabled = True

            'If IsPostBack Then rootNode.ExpandChildNodes()
        Else

            Me.ConfermaButton.Enabled = False
            'Me.FiltroImageButton.Enabled = False
            'Me.FiltroTextBox.Enabled = False
            Me.ToggleNodeImageButton.Enabled = False

            Dim childItem As New Telerik.Web.UI.RadTreeNode("Nessuna Struttura Abilitata", -99)
            childItem.ForeColor = Drawing.Color.Red
            childItem.Enabled = True
            childItem.Attributes.Add("nonselezionabile", "")
            rootNode.Nodes.Add(childItem)

        End If

    End Sub

    Private Function GetInfoNodes(ByVal descrizione As String) As List(Of NodeItemInfo)

        Dim strutture As New StructureRepository
        Dim struttureLivello As New StrutturaLivelloRepository(strutture.Context)


        Dim infoNodes = From struttura In strutture.GetQuery
                        Group Join strutturaLivello In struttureLivello.GetQuery
                        On struttura.IdGerarchia Equals strutturaLivello.Gerarchia
                        Into elenco = Group
                        From strutturaLivello In elenco.DefaultIfEmpty
                        Where struttura.LogStato Is Nothing And struttura.Descrizione.Contains(descrizione)
                        Order By struttura.Ordine
                        Select New NodeItemInfo() With {
                            .Id = struttura.Id,
                            .ParentID = struttura.IdPadre,
                            .Description = If(struttura.Descrizione Is Nothing, "", struttura.Descrizione),
                            .HierarchyId = struttura.IdGerarchia,
                            .Code = struttura.Codice,
                            .Responsable = struttura.Responsabile,
                            .Icon = If(strutturaLivello Is Nothing, "", strutturaLivello.UrlIcona),
                            .HierarchyDescription = If(strutturaLivello Is Nothing, "", strutturaLivello.Descrizione),
                            .Order = struttura.Ordine,
                            .Enabled = True
                        }



        If Not Me.ApplicaAbilitazioni Then
            Return infoNodes.ToList
        End If

        Dim abilitazioneUtenteTutteStrutture As ParsecAdmin.AbilitazioneUtenteTutteStrutture = (New ParsecAdmin.AbilitazioneUtenteTutteStruttureRepository).GetQuery.Where(Function(c) c.IdUtente = Me.IdUtente AndAlso c.IdModulo = Me.IdModulo AndAlso c.LogTipoOperazione Is Nothing).FirstOrDefault
        If Not abilitazioneUtenteTutteStrutture Is Nothing Then
            Return infoNodes.ToList
        End If

        If Me.IdUtente <> 0 AndAlso Me.IdModulo <> 0 Then

            Dim ids As List(Of Integer) = Me.GetStruttureAbilitate(Me.IdUtente, Me.IdModulo)


            Dim livelli As New ParsecAdmin.StrutturaLivelloRepository
            Dim maxDepth As Integer = livelli.GetQuery.Count
            livelli.Dispose()

            Dim rs As New Stack(Of IQueryable(Of ParsecAdmin.Struttura))


            Dim view = strutture.GetQuery.Where(Function(c) c.LogStato Is Nothing)

            '*****************************************************************
            'PER OGNI STRUTTURA ABILITATA RECUPERO IL NODO COMPLETO
            '*****************************************************************

            Dim listaIds As New List(Of Integer)

            For Each idStruttura In ids
                rs.Clear()
                Dim idStrutturaCorrente = idStruttura

                Dim struttura = view.Where(Function(c) c.Id = idStrutturaCorrente)
                If struttura.Any Then
                    rs.Push(struttura)
                End If


                For i As Integer = 0 To maxDepth - 1
                    Dim strutturaSuccessiva = From r In rs.Peek
                                              Join s In view
                                              On r.Id Equals s.IdPadre
                                              Select s

                    If strutturaSuccessiva.Any Then
                        rs.Push(strutturaSuccessiva)
                    End If

                Next

                Dim ramoOrganigramma = rs.Aggregate(Function(q1, q2) q1.Union(q2))
                listaIds.AddRange(ramoOrganigramma.Select(Function(c) c.Id).ToList)

            Next

            listaIds = listaIds.Distinct.ToList


            Try
                'Dim ramoOrganigramma = rs.Aggregate(Function(q1, q2) q1.Union(q2))
                'Dim listaIds = ramoOrganigramma.Select(Function(c) c.Id).ToList


                Dim filteredNodes As List(Of ParsecAdmin.NodeItemInfo) = infoNodes.Where(Function(c) listaIds.Contains(c.Id)).ToList
                'Dim filteredNodes As List(Of ParsecAdmin.NodeItemInfo) = (From r In infoNodes Join i In listaIds On r.Id Equals i Select r).ToList

                Me.RecursiveBuildNode(filteredNodes, Me.enabledNodes, infoNodes.ToList)
                'Restituisco le strutture che hanno almeno un nodo abilitato.

                Return Me.enabledNodes

            Catch ex As Exception
                Return Nothing
            End Try

        Else
            'Restituisco tutte le strutture.
            Return infoNodes.ToList
        End If

    End Function

    Private Function GetStruttureAbilitate(ByVal idUtente As Integer, ByVal idModulo As Integer) As List(Of Integer)
        Dim res As List(Of Integer) = Nothing

        Dim abilitazioniUtenteStruttura As New ParsecAdmin.AbilitazioneUtenteStrutturaRepository
        Dim strutture As New ParsecAdmin.StructureRepository(abilitazioniUtenteStruttura.Context)

        res = (From aus In abilitazioniUtenteStruttura.GetQuery
              Join s In strutture.GetQuery
              On aus.CodiceStruttura Equals s.Codice
              Where s.LogStato Is Nothing And aus.IdUtente = idUtente AndAlso aus.IdModulo = idModulo AndAlso aus.LogTipoOperazione Is Nothing
              Select s.Id).ToList

        abilitazioniUtenteStruttura.Dispose()

        Return res
    End Function

    Private Sub SelezionaStrutture()
        Dim ids As New List(Of Integer)
        If Me.TipoSelezione = 1 Then
            For Each node As Telerik.Web.UI.RadTreeNode In Me.StruttureTreeView.CheckedNodes
                ids.Add(CInt(node.Value))
            Next
        Else
            ids.Add(CInt(Me.StruttureTreeView.SelectedNode.Value))
        End If
        Dim res = SelezionaStrutture(ids)
        Session("SelectedStructures") = res
        ParsecUtility.Utility.ClosePopup(True)
    End Sub

    Private Function FilterNodes(ByVal nodeInfos As List(Of NodeItemInfo)) As List(Of NodeItemInfo)
        Dim result As New List(Of ParsecAdmin.NodeItemInfo)
        Dim filteredNodes As List(Of ParsecAdmin.NodeItemInfo) = Me.GetInfoNodes(Me.FiltroTextBox.Text)
        Dim parent As ParsecAdmin.NodeItemInfo = Nothing
        For Each node As ParsecAdmin.NodeItemInfo In filteredNodes
            Dim parentId As Integer = node.ParentID
            parent = nodeInfos.Where(Function(c) c.Id = parentId).FirstOrDefault
            Dim id As Integer = node.Id
            Dim exist = Not result.Where(Function(c) c.Id = id).FirstOrDefault Is Nothing
            If Not exist Then
                result.Add(node)
            End If
            While Not parent Is Nothing
                id = parent.Id
                exist = Not result.Where(Function(c) c.Id = id).FirstOrDefault Is Nothing
                If Not exist Then
                    result.Add(parent)
                End If
                parent = nodeInfos.Where(Function(c) c.Id = parent.ParentID).FirstOrDefault
            End While
        Next
        Return result
    End Function

    Private Function SelezionaStrutture(ByVal idStruttureSelezionate As List(Of Integer)) As List(Of ParsecAdmin.StrutturaAbilitata)
        Dim strutture As New StructureRepository
        Dim idModulo As Integer = Me.IdModulo

        If idModulo = 0 Then
            If Not String.IsNullOrEmpty(Me.NomeModulo) Then
                Dim moduli As New ParsecAdmin.ModuleRepository
                idModulo = (From m In moduli.GetQuery Where m.Descrizione = Me.NomeModulo Select m.Id).FirstOrDefault
                moduli.Dispose()
            End If
        End If

        Dim res = (From struttura In strutture.GetQuery
                   Join idStrutturaSelezionata In idStruttureSelezionate
                   On struttura.Id Equals idStrutturaSelezionata
                   Order By struttura.Descrizione
                   Select New ParsecAdmin.StrutturaAbilitata With {
                       .Id = struttura.Id,
                       .Descrizione = struttura.Descrizione,
                       .Modulo = Me.NomeModulo,
                       .Codice = struttura.Codice,
                       .IdModulo = idModulo,
                       .IdGerarchia = struttura.IdGerarchia,
                       .IdUtente = struttura.IDUtente,
                       .IdGruppo = struttura.IdGruppo
                   }).ToList

        strutture.Dispose()

        Return res
    End Function

    Private Sub AddNode(ByVal nodes As List(Of NodeItemInfo), ByVal t As Telerik.Web.UI.RadTreeNode)
        For Each node As NodeItemInfo In nodes
            If node.HierarchyId <= ultimoLivelloStruttura Then

                Dim Id As Integer = node.Id
                Dim childItem As New Telerik.Web.UI.RadTreeNode(node.Description, node.Id)
                childItem.ToolTip = "Codice struttura: " & node.Code.ToString
                childItem.ImageUrl = node.Icon
                If node.Responsable Then
                    childItem.ImageUrl = "~/images/SupUser.gif"
                End If
                If Me.livelliSelezionabili.Contains(node.HierarchyId.ToString) Then
                    'If enabledNodes.Count > 0 Then
                    '    If Not Me.enabledNodes.Contains(node) Then

                    '    End If
                    'End If
                    If Not node.Enabled Then
                        'childItem.Expanded = True
                        childItem.Checkable = False
                        childItem.Enabled = True
                        childItem.Attributes.Add("nonselezionabile", "")
                        childItem.ToolTip = "Codice struttura: " & node.Code.ToString & " (NON SELEZIONABILE)"
                    End If
                Else
                    childItem.ImageUrl = "~/images/pallino_x.gif"
                    childItem.Checkable = False
                    childItem.ForeColor = Drawing.Color.Red
                    childItem.Enabled = True 'False
                    childItem.Attributes.Add("nonselezionabile", "")
                    childItem.ToolTip = "Codice struttura: " & node.Code.ToString & " (NON SELEZIONABILE)"
                End If


                t.Nodes.Add(childItem)
                Me.temp = Me.nodeInfos.Where(Function(c) c.ParentID = Id).ToList
                Me.AddNode(Me.temp, childItem)
            End If
        Next
    End Sub

    Private Sub RecursiveBuildNode(ByVal filteredNodes As List(Of NodeItemInfo), ByVal result As List(Of NodeItemInfo), ByVal nodes As List(Of NodeItemInfo))
        Dim exist As Boolean = False
        Dim code As Integer = 0
        For Each node As NodeItemInfo In filteredNodes
            Dim Id As Integer = node.Id

            '*********************************************************************************************
            '  Costruisco i nodi superiori a partire dal nodo corrente.
            '*********************************************************************************************
            Dim parent As ParsecAdmin.NodeItemInfo = Nothing
            Dim parentId As Integer = node.ParentID
            parent = nodes.Where(Function(c) c.Id = parentId).FirstOrDefault
            While Not parent Is Nothing
                code = parent.Code
                exist = Not result.Where(Function(c) c.Code = code).FirstOrDefault Is Nothing

                If Not exist Then
                    result.Add(parent)
                    'Lo disabilito se non è presente nella lista dei nodi abilitati.
                    exist = Not filteredNodes.Where(Function(c) c.Code = code).FirstOrDefault Is Nothing

                    If Not exist Then
                        parent.Enabled = False

                    End If
                End If
                parent = nodes.Where(Function(c) c.Id = parent.ParentID).FirstOrDefault
            End While
            '*********************************************************************************************

            '*********************************************************************************************
            '  Costruisco i nodi inferiori a partire dal nodo corrente se non sono stati già inseriti.
            '*********************************************************************************************
            code = node.Code
            exist = Not result.Where(Function(c) c.Code = code).FirstOrDefault Is Nothing

            If Not exist Then
                result.Add(node)
            End If
            '*********************************************************************************************
            Me.RecursiveBuildNode(nodes.Where(Function(c) c.ParentID = Id).ToList, result, nodes)
        Next
    End Sub

#End Region


#Region "EVENTI CONTROLLI"

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Me.SelezionaStrutture()
    End Sub

    Protected Sub FiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltroImageButton.Click
        Me.LoadTree()
    End Sub

    Protected Sub StruttureTreeView_NodeClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs)
        If Me.TipoSelezione = 0 Then
            If e.Node.Value <> "0" Then
                SelezionaStrutture()
            End If
        End If
    End Sub

    Protected Sub DoppioClickImageButton_Click(sender As Object, e As System.EventArgs) Handles DoppioClickImageButton.Click
        If Not String.IsNullOrEmpty(Me.IdStrutturaSelezionataHidden.Value) Then
            If Me.IdStrutturaSelezionataHidden.Value <> "0" Then
                Dim ids As New List(Of Integer)
                ids.Add(CInt(Me.IdStrutturaSelezionataHidden.Value))
                Dim res = SelezionaStrutture(ids)
                Session("SelectedStructures") = res
                Me.IdStrutturaSelezionataHidden.Value = String.Empty
                ParsecUtility.Utility.ClosePopup(True)
            End If
        End If
    End Sub

    Protected Sub ToggleNodeImageButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ToggleNodeImageButton.Click
        If Me.ToggleNodeImageButton.ImageUrl = "~/images/collapse.png" Then
            Dim rootNode = Me.StruttureTreeView.Nodes(0)

            Me.StruttureTreeView.CollapseAllNodes()
            Me.ToggleNodeImageButton.ImageUrl = "~/images/expand.png"
            Me.ToggleNodeImageButton.ToolTip = "Espandi tutto"
            rootNode.Expanded = True

        Else
            Me.StruttureTreeView.ExpandAllNodes()
            Me.ToggleNodeImageButton.ImageUrl = "~/images/collapse.png"
            Me.ToggleNodeImageButton.ToolTip = "Comprimi tutto"
        End If

    End Sub

#End Region

End Class