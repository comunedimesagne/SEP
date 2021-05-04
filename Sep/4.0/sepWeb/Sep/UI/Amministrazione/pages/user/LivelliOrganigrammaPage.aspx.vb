Imports ParsecAdmin
Imports System.Transactions

Partial Class LivelliTitolarioPage
    Inherits System.Web.UI.Page


    Private WithEvents MainPage As MainPage

    Private temp As List(Of ParsecAdmin.StrutturaLivello)
    Private nodeInfos As List(Of ParsecAdmin.StrutturaLivello)

    Public Property LivelloStruttura() As ParsecAdmin.StrutturaLivello
        Get
            Return CType(Session(CStr(ViewState("Livello_Ticks"))), ParsecAdmin.StrutturaLivello)
        End Get
        Set(ByVal value As ParsecAdmin.StrutturaLivello)
            If ViewState("Livello_Ticks") Is Nothing Then
                ViewState("Livello_Ticks") = "Livello_" & Now.Ticks.ToString
            End If
            Session(CStr(ViewState("Livello_Ticks"))) = value
        End Set
    End Property


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Amministrazione"
        MainPage.DescrizioneProcedura = "> Gestione Livelli Organigramma"
        Me.BuildTree(Nothing)
    End Sub


    Private Sub BuildTree(ByVal idToSelect As String)
        Me.nodeInfos = (New ParsecAdmin.StrutturaLivelloRepository).GetQuery.OrderBy(Function(c) c.Id).ToList
        Me.LivelliOrganigrammaTreeView.Nodes.Clear()
        Dim rootNode As New Telerik.Web.UI.RadTreeNode("Livelli Organigramma", 0)
        Me.LivelliOrganigrammaTreeView.Nodes.Add(rootNode)
        Me.AddNode(Me.nodeInfos, rootNode, idToSelect)
        Me.LivelliOrganigrammaTreeView.ExpandAllNodes()
    End Sub


    Private Sub AddNode(ByVal nodes As List(Of ParsecAdmin.StrutturaLivello), ByVal t As Telerik.Web.UI.RadTreeNode, ByVal idToSelect As String)
        Me.temp = Me.nodeInfos.Where(Function(c) c.IdPadre = CInt(t.Value)).ToList
        For Each node As ParsecAdmin.StrutturaLivello In temp
            Dim Id As Integer = node.Id
            Dim childItem As New Telerik.Web.UI.RadTreeNode(node.Descrizione, node.Id)
            t.Nodes.Add(childItem)
            childItem.ToolTip = "Gerarchia: " & node.Gerarchia.ToString
            Me.temp = Me.nodeInfos.Where(Function(c) c.IdPadre = Id).ToList
            If Not idToSelect Is Nothing Then
                If node.Gerarchia = CInt(idToSelect) Then
                    childItem.Selected = True
                End If
            End If
            Me.AddNode(nodes, childItem, idToSelect)
        Next
    End Sub


    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare il livello selezionato?", False, Not Me.LivelloStruttura Is Nothing)
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
                Me.LivelloTextBox.Text = (New ParsecAdmin.StrutturaLivelloRepository).GetNuovoGerarchia.ToString
            Case "Annulla"
                Me.ResettaVista()
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.LivelloStruttura Is Nothing Then
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
        Try
            Dim livelliStruttura As New ParsecAdmin.StrutturaLivelloRepository
            livelliStruttura.Delete(Me.LivelloStruttura)
            livelliStruttura.Dispose()
        Catch ex As Exception
            ParsecUtility.Utility.MessageBox(ex.Message, False)
        End Try
    End Sub


    Private Sub Save()

        If Me.LivelliOrganigrammaTreeView.SelectedNode Is Nothing Then
            Throw New ApplicationException("E' necessario selezionare un livello!")
            Return
        End If

        Dim livelliStruttura As New ParsecAdmin.StrutturaLivelloRepository
        Dim livello As ParsecAdmin.StrutturaLivello = Nothing
        If Me.LivelloStruttura Is Nothing Then
            livello = New ParsecAdmin.StrutturaLivello
            livello.IdPadre = CInt(Me.LivelliOrganigrammaTreeView.SelectedNode.Value)
        Else
            livello = livelliStruttura.GetQuery.Where(Function(c) c.Id = Me.LivelloStruttura.Id).FirstOrDefault
        End If

        livello.Descrizione = Me.DescrizioneTextBox.Text
        livello.UrlIcona = Me.IconaTextBox.Text
        livello.Gerarchia = CInt(Me.LivelloTextBox.Text)

        Try
            livelliStruttura.Save(livello)
            Me.LivelloStruttura = livelliStruttura.LivelloStruttura

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            livelliStruttura.Dispose()
        End Try
    End Sub

    Private Sub ResettaVista()
        Me.LivelloStruttura = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
        Me.LivelloTextBox.Text = String.Empty
        Me.IconaTextBox.Text = String.Empty
    End Sub


    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub


    Protected Sub LivelliClassificazioniTreeView_NodeClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles LivelliOrganigrammaTreeView.NodeClick
        Me.AggiornaVista(e)
    End Sub

 
    Private Sub AggiornaVista(ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs)
        Me.RadToolBar.FindItemByText("Elimina").Enabled = True
        Me.RadToolBar.FindItemByText("Nuovo").Enabled = True
        Me.RadToolBar.FindItemByText("Salva").Enabled = True
        Dim livelliStruttura As New ParsecAdmin.StrutturaLivelloRepository
        If e.Node.Value <> "0" Then
            Me.ResettaVista()
            Dim id As Integer = e.Node.Value

            Me.LivelloStruttura = livelliStruttura.GetQuery.Where(Function(c) c.Id = id).FirstOrDefault

            Me.DescrizioneTextBox.Text = Me.LivelloStruttura.Descrizione

            Me.LivelloTextBox.Text = Me.LivelloStruttura.Gerarchia
            Me.IconaTextBox.Text = Me.LivelloStruttura.UrlIcona

        Else
            Me.ResettaVista()
        End If

        If Not Me.LivelloStruttura Is Nothing Then
            Dim last As Boolean = False
            Dim livelli = livelliStruttura.GetQuery
            If livelli.Count > 0 Then
                Dim max = (From r In livelli Select r.Gerarchia).Max
                last = Me.LivelloStruttura.Gerarchia = max
            End If
            If Not last Then
                Me.RadToolBar.FindItemByText("Elimina").Enabled = False
                Me.RadToolBar.FindItemByText("Nuovo").Enabled = False
            End If
        Else
            Me.RadToolBar.FindItemByText("Elimina").Enabled = False
            Me.RadToolBar.FindItemByText("Nuovo").Enabled = False
            Me.RadToolBar.FindItemByText("Salva").Enabled = False
        End If

        livelliStruttura.Dispose()

    End Sub


End Class