<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaIndiceIpaPage.aspx.vb"
    Inherits="RicercaIndiceIpaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Referente IPA</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>
<script type="text/javascript">

    var _backgroundElement = document.createElement("div");

    function pageLoad() {
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
        manager.add_endRequest(OnEndRequest);
        $get("pageContent").appendChild(_backgroundElement);

    }


    function OnBeginRequest(sender, args) {
        EnableUI(false);
    }

    function OnEndRequest(sender, args) {
        EnableUI(true);

    }

    function EnableUI(state) {
        if (!state) {
            _backgroundElement.style.display = '';
            _backgroundElement.style.position = 'absolute';
            _backgroundElement.style.left = '0px';
            _backgroundElement.style.top = '0px';

            _backgroundElement.style.width = '100%';
            _backgroundElement.style.height = '100%';

            _backgroundElement.style.zIndex = 10000;
            _backgroundElement.style.backgroundColor = '#09718F';
            _backgroundElement.style.filter = "alpha(opacity=20)";
            _backgroundElement.style.opacity = "0.2";
        }
        else {
            _backgroundElement.style.display = 'none';

        }
    }

</script>
<body runat="server" id="CorpoPagina">
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 150px; z-index: 2000000">
                <table cellpadding="4" style="background-color: #4892FF; margin: 0 auto">
                    <tr>
                        <td>
                            <div id="loadingInner" style="width: 300px; text-align: center; background-color: #BFDBFF;
                                height: 60px">
                                <span style="color: #00156E">Attendere prego ... </span>
                                <br />
                                <br />
                                <img alt="" src="../../../../images/loading.gif" border="0">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <center>
                    <table style="width: 800px; height: 100%" cellpadding="0" cellspacing="0" border="0">
                        <tr style="height: 400px">
                            <td valign="top">
                                <asp:Panel ID="FiltroPanel" runat="server" Height="100%">
                                    <table style="width: 100%; height: 100%" cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td valign="top">
                                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                                    height: 100%">
                                                    <tr style="height: 20px">
                                                        <td valign="top">
                                                            <table style="width: 100%; background-color: #BFDBFF">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;<asp:Label ID="ElencoAmministrazioniLabel" runat="server" Font-Bold="True"
                                                                            Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Elenco Amministrazioni" />
                                                                    </td>
                                                                    <td align="center" style="width: 30px">
                                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                                            ToolTip="Filtra amministrazioni" Style="border: 0" ImageAlign="AbsMiddle" />
                                                                    </td>
                                                                    <td align="center" style="width: 30px">
                                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                                            ToolTip="Ripristina filtro iniziale" ImageAlign="AbsMiddle" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 30px">
                                                        <td valign="top">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 75px">
                                                                        &nbsp;&nbsp;<asp:Label ID="CategorieLabel" runat="server" CssClass="Etichetta" Text="Categorie" />
                                                                    </td>
                                                                    <td>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadComboBox ID="CategorieComboBox" runat="server" Skin="Office2007" Width="330px"
                                                                                        EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" NoWrap="True"
                                                                                        MaxHeight="400px" />
                                                                                    &nbsp;&nbsp;
                                                                                    <asp:Label ID="ChiaveRicercaLabel" runat="server" CssClass="Etichetta" Text="Chiave ricerca" />&nbsp;&nbsp;
                                                                                    <telerik:RadTextBox ID="ChiaveRicercaTextBox" runat="server" Skin="Office2007" Width="250px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="ContainerMargin">
                                                            <table class="Container" cellpadding="0" cellspacing="4" style="width: 100%; height: 100%"
                                                                border="0">
                                                                <tr style="background-color: #DFE8F6">
                                                                    <td valign="top">
                                                                        <div style="overflow: auto; height: 345px">
                                                                            <telerik:RadGrid ID="AmministrazioniGridView" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                                Culture="it-IT" AllowFilteringByColumn="true">
                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                <MasterTableView DataKeyNames="Codice">
                                                                                    <Columns>
                                                                                        <telerik:GridBoundColumn DataField="Codice" FilterControlAltText="Filter Codice column"
                                                                                            HeaderText="Codice" ReadOnly="True" SortExpression="Codice" UniqueName="Codice"
                                                                                            Visible="false" />
                                                                                        <telerik:GridTemplateColumn SortExpression="Nome" UniqueName="Nome" HeaderText="Descrizione"
                                                                                            DataField="Nome" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                                            CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                            FilterControlAltText="Filter Nome column" AllowFiltering="true">
                                                                                            <ItemTemplate>
                                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 200px;"
                                                                                                    title='<%# Eval("Nome")%>'>
                                                                                                    <%# Eval("Nome")%>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn SortExpression="Sede" UniqueName="Sede" HeaderText="Sede"
                                                                                            DataField="Sede" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                                            CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                            FilterControlAltText="Filter Sede column" AllowFiltering="true">
                                                                                            <ItemTemplate>
                                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 200px;"
                                                                                                    title='<%# Eval("Sede")%>'>
                                                                                                    <%# Eval("Sede")%>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn SortExpression="Email" UniqueName="Email" HeaderText="E-mail"
                                                                                            DataField="Email" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                                            CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                            FilterControlAltText="Filter Email column" AllowFiltering="true">
                                                                                            <ItemTemplate>
                                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 200px;"
                                                                                                    title='<%# Eval("Email")%>'>
                                                                                                    <%# Eval("Email")%>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="SelectUffici" FilterControlAltText="Filter SelectUffici column"
                                                                                            ImageUrl="~/images/Uffici.png" UniqueName="SelectUffici" HeaderStyle-Width="20px"
                                                                                            ItemStyle-Width="20px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                                        </telerik:GridButtonColumn>
                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="SelectAoo" FilterControlAltText="Filter SelectAoo column"
                                                                                            ImageUrl="~/images/Aoo.png" UniqueName="SelectAoo" HeaderStyle-Width="20px" ItemStyle-Width="20px"
                                                                                            ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                                        </telerik:GridButtonColumn>
                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                                                            ImageUrl="~/images/Checks.png" UniqueName="Select" HeaderStyle-Width="20px" ItemStyle-Width="20px"
                                                                                            ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                                        </telerik:GridButtonColumn>
                                                                                    </Columns>
                                                                                </MasterTableView>
                                                                            </telerik:RadGrid>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="RisultatiPanel" runat="server" Visible="False" Height="100%">
                                    <table style="width: 100%; height: 100%" cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td valign="top">
                                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                                    height: 100%">
                                                    <tr style="height: 20px">
                                                        <td valign="top">
                                                            <table style="width: 100%; background-color: #BFDBFF">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;<asp:Label ID="ElencoUfficiLabel" runat="server" Font-Bold="True" Style="width: 700px;
                                                                            color: #00156E; background-color: #BFDBFF" Text="Elenco Uffici" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="ContainerMargin">
                                                            <table class="Container" cellpadding="0" cellspacing="4" style="width: 100%; height: 100%"
                                                                border="0">
                                                                <tr style="background-color: #DFE8F6">
                                                                    <td valign="top">
                                                                        <div style="overflow: auto; height: 345px">
                                                                            <telerik:RadGrid ID="UfficiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                                Culture="it-IT" AllowFilteringByColumn="true">
                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                <MasterTableView DataKeyNames="Codice">
                                                                                    <Columns>
                                                                                        <telerik:GridBoundColumn DataField="Codice" FilterControlAltText="Filter Codice column"
                                                                                            HeaderText="Codice" ReadOnly="True" SortExpression="Codice" UniqueName="Codice"
                                                                                            Visible="false" />
                                                                                        <telerik:GridTemplateColumn SortExpression="Nome" UniqueName="Nome" HeaderText="Nome"
                                                                                            DataField="Nome" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                                            CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                            FilterControlAltText="Filter Nome column" AllowFiltering="true">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("Nome")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                    width: 200px;">
                                                                                                    <%# Eval("Nome")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn SortExpression="Sede" UniqueName="Sede" HeaderText="Sede"
                                                                                            DataField="Sede" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                                            CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                            FilterControlAltText="Filter Sede column" AllowFiltering="true">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("Sede")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                    width: 200px;">
                                                                                                    <%# Eval("Sede")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn SortExpression="Email" UniqueName="Email" HeaderText="E-mail"
                                                                                            DataField="Email" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                                            CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                            FilterControlAltText="Filter Email column" AllowFiltering="true">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("Email")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                    width: 200px;">
                                                                                                    <%# Eval("Email")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                                                            ImageUrl="~/images/Checks.png" UniqueName="Select" HeaderStyle-Width="20px" ItemStyle-Width="20px"
                                                                                            ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                                        </telerik:GridButtonColumn>
                                                                                    </Columns>
                                                                                </MasterTableView>
                                                                            </telerik:RadGrid>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table style="width: 100%; height: 200px" cellpadding="5" cellspacing="5" border="0">
                                    <tr>
                                        <td>
                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                                height: 100%">
                                                <tr style="height: 20px">
                                                    <td valign="top">
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;<asp:Label ID="ElencoReferentiSelezionatiLabel" runat="server" Font-Bold="True"
                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Elenco Referenti Selezionati" />
                                                                </td>
                                                                <td align="center" style="width: 30px">
                                                                    <asp:ImageButton ID="EliminaTuttiReferentiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                        ToolTip="Elimina tutti i referenti" Style="border: 0" ImageAlign="AbsMiddle" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #DFE8F6">
                                                    <td valign="top" class="ContainerMargin">
                                                        <div style="overflow: auto; height: 150px; border: 1 solid #5D8CC9">
                                                            <telerik:RadGrid ID="ElementiSelezionatiGridView" runat="server" AllowPaging="False"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="99.8%" AllowSorting="True" Culture="it-IT">
                                                                <MasterTableView DataKeyNames="Codice">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="Codice" FilterControlAltText="Filter Codice column"
                                                                            HeaderText="Codice" ReadOnly="True" SortExpression="Codice" UniqueName="Codice"
                                                                            Visible="false" />
                                                                        <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Nome" HeaderText="Descrizione"
                                                                            DataField="Descrizione" HeaderStyle-Width="700px" ItemStyle-Width="700px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 700px;">
                                                                                    <%# Eval("Descrizione")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                            ImageUrl="~/images/Delete16.png" UniqueName="Delete" HeaderStyle-Width="20px"
                                                                            ItemStyle-Width="20px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                        </telerik:GridButtonColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="GridFooter">
                                                    <td align="center">
                                                        <telerik:RadButton ID="IndietroRisultatiButton" runat="server" Text="Indietro" Width="100px"
                                                            Skin="Office2007" ToolTip="Torna ai risultati">
                                                            <Icon PrimaryIconUrl="../../../../images/back.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                        &nbsp; &nbsp;
                                                        <telerik:RadButton ID="ConfermaButton" runat="server" Text="Conferma" Width="100px"
                                                            Skin="Office2007" ToolTip="Conferma e chiudi finestra">
                                                            <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
