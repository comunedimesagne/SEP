Imports ParsecAdmin




Partial Class RicercaSezioneTrasparenzaPage
    Inherits System.Web.UI.Page

    Private nodeInfos As List(Of ParsecAdmin.NodeItemInfo)
    Private temp As List(Of NodeItemInfo)

    Private enabledNodes As New List(Of NodeItemInfo)


    Private ultimoLivelloStruttura As Integer = 400
    Private livelliSelezionabili As String = "100,200,300,400"

    Private selezionaSezioniAbrogate As Boolean = True


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
            If parametriPagina.ContainsKey("ultimoLivelloStruttura") Then
                Me.ultimoLivelloStruttura = parametriPagina("ultimoLivelloStruttura")
            End If
            If parametriPagina.ContainsKey("Filtro") Then
                Me.Filtro = parametriPagina("Filtro")
                Me.FiltroTextBox.Text = Me.Filtro
            End If

            If parametriPagina.ContainsKey("selezionaSezioniAbrogate") Then
                Me.selezionaSezioniAbrogate = parametriPagina("selezionaSezioniAbrogate")
            End If


            ParsecUtility.SessionManager.ParametriPagina = Nothing
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'todo passare i parametri
        If Not Me.IsPostBack Then
            Me.GetParametri()
            If Me.TipoSelezione = 0 Then
                Me.ConfermaButton.Visible = False
            End If
            Session("nodeInfos") = Me.GetInfoNodes("")
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
        Me.SezioniTrasparenzaTreeView.Nodes.Clear()
        Me.temp = Me.nodeInfos.Where(Function(c) c.ParentID = 0).ToList
        Dim clientRepository As New ClientRepository
        Dim clienteCorrente As ParsecAdmin.Cliente = clientRepository.GetQuery.FirstOrDefault
        clientRepository.Dispose()
        Dim rootNode As New Telerik.Web.UI.RadTreeNode(clienteCorrente.Descrizione, 0)
        Me.SezioniTrasparenzaTreeView.Nodes.Add(rootNode)
        rootNode.Checkable = False
        Me.SezioniTrasparenzaTreeView.CheckBoxes = (TipoSelezione = 1)
        Me.AddNode(Me.temp, rootNode)
        Me.SezioniTrasparenzaTreeView.ExpandAllNodes()
    End Sub

    Public Function GetInfoNodes(ByVal descrizione As String) As List(Of NodeItemInfo)


        Dim sezioni As New ParsecAdmin.SezioneTrasparenzaRepository
        Dim livelloSezioni As New ParsecAdmin.GerarchiaSezioneTrasparenzaRepository(sezioni.Context)



        'Se ho specificato il modulo controllo se per quel modulo è abilitato
        If CBool(IdModulo) Then
            'Altri moduli
            Dim abilitazioneModuloR As New ParsecAdmin.SezioneAmministrazioneTrasparenteModuloRepository(sezioni.Context)
            'Dim sezioniAbilitate As List(Of ParsecAdmin.SezioneAmministrazioneTrasparenteModulo) = abilitazioneModuloR.GetQuery.Where(Function(s) s.IdModulo = IdModulo).ToList

            Dim infoNodes1 = From sezione In sezioni.GetQuery
                                Group Join livelloSezione In livelloSezioni.GetQuery On sezione.IdGerarchia Equals livelloSezione.IdGerarchia Into elenco = Group From livelloSezione In elenco.DefaultIfEmpty
                                Group Join abilitazModulo In abilitazioneModuloR.GetQuery On sezione.Id Equals abilitazModulo.IdSezione Into elenco2 = Group From abilitazModulo In elenco2.DefaultIfEmpty
                        Select sezione, livelloSezione, abilitazModulo

            If Not String.IsNullOrEmpty(descrizione) Then
                infoNodes1 = infoNodes1.Where(Function(c) c.sezione.Descrizione.Contains(descrizione))
            End If

            Dim res1 = infoNodes1.AsEnumerable.Select(Function(c) New NodeItemInfo With {
                                       .Id = c.sezione.Id,
                                       .ParentID = c.sezione.IdPadre,
                                        .Description = If(c.sezione.Descrizione Is Nothing, "", c.sezione.Descrizione),
                                       .HierarchyId = c.sezione.IdGerarchia * 100,
                                       .Icon = If(c.livelloSezione Is Nothing, "", c.livelloSezione.UrlIcona),
                                       .HierarchyDescription = If(c.livelloSezione Is Nothing, "", c.livelloSezione.Descrizione),
                                       .Enabled = c.sezione.Abilitata,
                                       .EnabledModule = If(c.abilitazModulo Is Nothing, False, True),
                                       .Active = Not c.sezione.Abrogata
                                       })

            Return res1.ToList
            abilitazioneModuloR.Dispose()
        Else
            'Modulo Pubblicazioni
            Dim infoNodes2 = From sezione In sezioni.GetQuery
                             Group Join livelloSezione In livelloSezioni.GetQuery
                             On sezione.IdGerarchia Equals livelloSezione.IdGerarchia
                             Into elenco = Group
                             From livelloSezione In elenco.DefaultIfEmpty
                             Select sezione, livelloSezione


            If Not String.IsNullOrEmpty(descrizione) Then
                infoNodes2 = infoNodes2.Where(Function(c) c.sezione.Descrizione.Contains(descrizione))
            End If

            Dim res2 = infoNodes2.AsEnumerable.Select(Function(c) New NodeItemInfo With {
                               .Id = c.sezione.Id,
                               .ParentID = c.sezione.IdPadre,
                                .Description = If(c.sezione.Descrizione Is Nothing, "", c.sezione.Descrizione),
                               .HierarchyId = c.sezione.IdGerarchia * 100,
                               .Icon = If(c.livelloSezione Is Nothing, "", c.livelloSezione.UrlIcona),
                               .HierarchyDescription = If(c.livelloSezione Is Nothing, "", c.livelloSezione.Descrizione),
                               .Enabled = c.sezione.Abilitata,
                               .Active = Not c.sezione.Abrogata
                               })

            Return res2.ToList
        End If

    End Function

    Protected Sub ConfermaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConfermaButton.Click
        Me.SelezionaSezioni()
    End Sub


 
    Private Sub SelezionaSezioni()

        Dim ids As New List(Of Integer)
        Dim sezioni As New ParsecAdmin.SezioneTrasparenzaRepository
        If Me.TipoSelezione = 1 Then
            For Each node As Telerik.Web.UI.RadTreeNode In Me.SezioniTrasparenzaTreeView.CheckedNodes
                ids.Add(CInt(node.Value))
            Next
        Else
            ids.Add(CInt(Me.SezioniTrasparenzaTreeView.SelectedNode.Value))
        End If
        Dim res = From sezione In sezioni.GetQuery
                Join i In ids On sezione.Id Equals i
                Order By sezione.Descrizione
                Select sezione


        ParsecUtility.SessionManager.SezioniSelezionate = res.ToList

        sezioni.Dispose()
        ParsecUtility.Utility.ClosePopup(True)

    End Sub


    Private Function FilterNodes(ByVal nodeInfos As List(Of NodeItemInfo)) As List(Of NodeItemInfo)
        Dim result As New List(Of ParsecAdmin.NodeItemInfo)
        Dim filteredNodes As List(Of ParsecAdmin.NodeItemInfo) = Me.GetInfoNodes(Me.FiltroTextBox.Text)
        Dim parent As ParsecAdmin.NodeItemInfo = Nothing
        For Each node As ParsecAdmin.NodeItemInfo In filteredNodes
            Dim parentId As Integer = node.ParentID
            parent = nodeInfos.Where(Function(c) c.Id = parentId).FirstOrDefault
            If Not result.Contains(node) Then
                result.Add(node)
            End If
            While Not parent Is Nothing
                If Not result.Select(Function(a) a.Id).Contains(parent.Id) Then
                    result.Add(parent)
                End If
                parent = nodeInfos.Where(Function(c) c.Id = parent.ParentID).FirstOrDefault
            End While
        Next
        Return result
    End Function

    Protected Sub FiltroImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FiltroImageButton.Click
        Me.LoadTree()
    End Sub

    Private Sub AddNode(ByVal nodes As List(Of NodeItemInfo), ByVal t As Telerik.Web.UI.RadTreeNode)
        For Each node As NodeItemInfo In nodes
            If node.HierarchyId <= ultimoLivelloStruttura Then

                Dim Id As Integer = node.Id
                Dim childItem As New Telerik.Web.UI.RadTreeNode(node.Description, node.Id)
                t.Nodes.Add(childItem)
                childItem.ImageUrl = node.Icon

                If node.Responsable Then
                    childItem.ImageUrl = "/sep/images/SupUser.gif"
                End If


                If Me.livelliSelezionabili.Contains(node.HierarchyId.ToString) Then


                    'If enabledNodes.Count > 0 Then
                    '    If Not Me.enabledNodes.Contains(node) Then

                    '    End If
                    'End If

                    If Not node.Enabled Then
                        childItem.Checkable = False
                        childItem.Enabled = False
                    End If

                    If CBool(IdModulo) Then
                        If Not node.EnabledModule Then
                            childItem.Checkable = False
                            childItem.Enabled = False
                        End If
                    End If


                    If Not node.Active Then
                        If Not selezionaSezioniAbrogate Then
                            childItem.Checkable = False
                            childItem.Attributes.Add("nonselezionabile", "")
                        End If

                        childItem.ForeColor = Drawing.Color.Red
                        childItem.ToolTip = "Abrogata"

                    End If

                Else


                    childItem.ImageUrl = "/sep/images/pallino_x.gif"
                    childItem.Checkable = False
                    childItem.ForeColor = Drawing.Color.Red
                    childItem.Enabled = False
                End If


                'childItem.ToolTip = "Codice struttura: " & node.Code.ToString
                Me.temp = Me.nodeInfos.Where(Function(c) c.ParentID = Id).ToList
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

    Protected Sub SezioniTrasparenzaTreeView_NodeClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles SezioniTrasparenzaTreeView.NodeClick
        If Me.TipoSelezione = 0 Then
            If e.Node.Value <> "0" Then
                SelezionaSezioni()
            End If
        End If
    End Sub

End Class
