Imports ParsecAdmin
Imports Telerik.Web.UI


'* SPDX-License-Identifier: GPL-3.0-only

Partial Class TipoDocumentoPage
    Inherits System.Web.UI.Page

    Private WithEvents MainPage As MainPage

#Region "PROPRIETA'"

    'Variabile di Sessione: oggetto TipoDocumento corrente.
    Public Property TipoDocumento() As ParsecPro.TipoDocumento
        Get
            Return CType(Session("TipoDocumentoPage_TipoDocumento"), ParsecPro.TipoDocumento)
        End Get
        Set(ByVal value As ParsecPro.TipoDocumento)
            Session("TipoDocumentoPage_TipoDocumento") = value
        End Set
    End Property

    'Variabile di Sessione: lista di oggetti TipoDocumento associata alla griglia TipiDocumentoGridView
    Public Property TipiDocumento() As List(Of ParsecPro.TipoDocumento)
        Get
            Return CType(Session("TipoDocumentoPage_TipiDocumento"), List(Of ParsecPro.TipoDocumento))
        End Get
        Set(ByVal value As List(Of ParsecPro.TipoDocumento))
            Session("TipoDocumentoPage_TipiDocumento") = value
        End Set
    End Property

#End Region

#Region "EVENTI PAGINA"

    'Evento Init associato alla Pagina: inizializza la pagina
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MainPage = CType(Me.Master, MainPage)
        MainPage.NomeModulo = "Protocollo"
        MainPage.DescrizioneProcedura = "> Gestione Tipi di Documento"

        If Not Me.Page.IsPostBack Then
            Me.TipiDocumento = Nothing
            Me.ResettaVista()
        End If

    End Sub

    'Evento LoadComplete associato alla Pagina. Gestisce il messaggio di conferma cancellazione e il titolo della griglia.
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ParsecUtility.Utility.Confirm("Eliminare l'elemento selezionato?", False, Not Me.TipoDocumento Is Nothing)
        Me.TitoloElencoTipiDocumentoLabel.Text = "Elenco tipi documento&nbsp;&nbsp;&nbsp;" & If(Me.TipiDocumento.Count > 0, "( " & Me.TipiDocumento.Count.ToString & " )", "")
    End Sub

#End Region

#Region "EVENTI TOOLBAR"

    'Evento Cick associato alla RadToolBar. Lancia i vari comandi.
    Private Sub RadToolBar_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ButtonClick
        Select Case CType(e.Item, Telerik.Web.UI.RadToolBarButton).CommandName
            Case "Stampa"
                Me.Print()
            Case "Salva"
                Dim message As String = "Operazione conclusa con successo!"
                Try
                    Me.Save()
                    Me.TipiDocumento = Nothing
                    Me.TipiDocumentoGridView.Rebind()
                Catch ex As ApplicationException
                    message = ex.Message
                End Try

                ParsecUtility.Utility.MessageBox(message, False)

            Case "Nuovo"
                Me.ResettaVista()
                Me.TipiDocumento = Nothing
                Me.TipiDocumentoGridView.Rebind()
            Case "Annulla"
                Me.ResettaVista()
                Me.TipiDocumento = Nothing
                Me.TipiDocumentoGridView.Rebind()
            Case "Elimina"
                If CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "1" Then
                    If Not Me.TipoDocumento Is Nothing Then
                        Me.Delete()
                        Me.TipiDocumento = Nothing
                        Me.ResettaVista()
                        Me.TipiDocumentoGridView.Rebind()
                    Else
                        ParsecUtility.Utility.MessageBox("E' necessario selezionare un tipo di documento!", False)
                    End If
                End If
                CType(Me.MainPage.FindControl("hflVerificaElimina"), HiddenField).Value = "0"
            Case "Trova"
                Me.Search()
        End Select
    End Sub

    'Evento ItemCreated associato alla RadToolBar. Aggiunge l'evento onclick sul pulsante "Elimina".
    Protected Sub RadToolBar_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles RadToolBar.ItemCreated
        If e.Item.Text = "Elimina" Then
            e.Item.Attributes.Add("onclick", "return Confirm();")
        End If
    End Sub

#End Region

#Region "EVENTI GRIGLIA"

    'Evento ItemCommand associato alla TipiDocumentoGridView. Fa partire i comandi associati alla griglia.
    Protected Sub TipiDocumentoGridView_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles TipiDocumentoGridView.ItemCommand
        If e.CommandName = "Select" Then
            Me.AggiornaVista(e.Item)
        End If
    End Sub

    'Evento ItemDataBound associato alla TipiDocumentoGridView. Setta alcuni tooltip. 
    Protected Sub TipiDocumentoGridView_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles TipiDocumentoGridView.ItemDataBound
        Dim btn As ImageButton = Nothing
        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
            Dim dataItem As Telerik.Web.UI.GridDataItem = e.Item
            If TypeOf dataItem("Select").Controls(0) Is ImageButton Then
                btn = CType(dataItem("Select").Controls(0), ImageButton)
                btn.ToolTip = "Seleziona tipo documento"
            End If
        End If
    End Sub

    'Evento NeedDataSource associato alla griglia TipiDocumentoGridView. Aggancia il datasource della griglia al DB. Aggiorna la variabile di sessione TipiDocumento.
    Protected Sub TipiDocumentoGridView_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles TipiDocumentoGridView.NeedDataSource
        If Me.TipiDocumento Is Nothing Then
            Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
            Me.TipiDocumento = tipiDocumento.GetView(Nothing)
            tipiDocumento.Dispose()
        End If
        Me.TipiDocumentoGridView.DataSource = Me.TipiDocumento
    End Sub

    'Evento ItemCreated associato alla Griglia TipiDocumentoGridView. Gestisce la navigazione tra pagine della griglia.
    Private Sub TipiDocumentoGridView_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles TipiDocumentoGridView.ItemCreated
        If TypeOf e.Item Is Telerik.Web.UI.GridPagerItem Then
            Dim pageSizeComboBox As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)
            pageSizeComboBox.Visible = False
            Dim changePageSizelbl As Label = CType(e.Item.FindControl("ChangePageSizeLabel"), Label)
            changePageSizelbl.Visible = False
        End If
    End Sub


#End Region

#Region "METODI PRIVATI"

    'Metodo Print: non implementato
    Private Sub Print()

    End Sub

    'Invoca il metodo Delete per la cancellazione dal DB. Invocato dalla Toolbar nel caso "Elimina"
    Private Sub Delete()
        Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
        tipiDocumento.Delete(Me.TipoDocumento.Id)
        tipiDocumento.Dispose()
    End Sub

    'Metodo search per la Ricerca
    Private Sub Search()
        Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
        Dim searchTemplate As New ParsecPro.TipoDocumento With
            {
                .Descrizione = Me.DescrizioneTextBox.Text
            }

        Me.TipiDocumento = tipiDocumento.GetView(searchTemplate)
        Me.TipiDocumentoGridView.Rebind()
        tipiDocumento.Dispose()
    End Sub

    'Metodo save per il salvataggio su DB.  Invocato dalla Toolbar
    Private Sub Save()
        Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
        Dim tipoDocumento As ParsecPro.TipoDocumento = tipiDocumento.CreateFromInstance(Me.TipoDocumento)
        tipoDocumento.Descrizione = Trim(Me.DescrizioneTextBox.Text.ToUpper)
        Try
            '*******************************************************************
            'Gestione storico non utilizzata.
            '*******************************************************************
            tipiDocumento.TipoDocumento = Me.TipoDocumento
            tipiDocumento.Save(tipoDocumento)

            '*******************************************************************
            'Aggiorno l'oggetto corrente
            '*******************************************************************
            Me.TipoDocumento = tipiDocumento.TipoDocumento
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            tipiDocumento.Dispose()
        End Try
    End Sub

    'Resetta la View
    Private Sub ResettaVista()
        Me.TipoDocumento = Nothing
        Me.DescrizioneTextBox.Text = String.Empty
    End Sub

    'Aggiorna la view quando si seleziona un elemento dalla griglia
    Private Sub AggiornaVista(ByVal item As Telerik.Web.UI.GridDataItem)
        Me.ResettaVista()
        Dim id As Integer = item.OwnerTableView.DataKeyValues(item.ItemIndex)("Id")
        Dim tipiDocumento As New ParsecPro.TipiDocumentoRepository
        Me.TipoDocumento = tipiDocumento.GetById(id)
        Me.DescrizioneTextBox.Text = Me.TipoDocumento.Descrizione
        tipiDocumento.Dispose()
    End Sub

#End Region

End Class