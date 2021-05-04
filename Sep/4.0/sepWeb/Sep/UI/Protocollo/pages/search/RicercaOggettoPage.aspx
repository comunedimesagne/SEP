<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaOggettoPage.aspx.vb"
    Inherits="RicercaOggettoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Oggetto</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link type="text/css" href="../../../../Styles/Styles.css" rel="stylesheet" />
    <style type="text/css">
        .rgAltRow, .rgRow
        {
            cursor: pointer !important;
        }
        .style1
        {
            height: 25px;
        }
    </style>
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
<body>
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 300px; z-index: 2000000">
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
            <div id="pageContent" style="text-align: center">
                <table style="width: 900px; border: 1px solid #5D8CC9">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                                            <Items>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                                    CommandName="Nuovo" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                    CommandName="Trova" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="DetailPanel" runat="server" Style="padding-top: 3px">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="vertical-align: top; width: 50%">
                                            <table style="width: 430px; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;<asp:Label ID="OggettoLabel" runat="server" Font-Bold="True" Style="width: 100px;
                                                                        color: #FF8040; background-color: #BFDBFF" Text="Oggetto *" CssClass="Etichetta" />
                                                                </td>
                                                                <td align="right" style="width: 20px">
                                                                    <asp:ImageButton ID="EliminaOggettoImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                        ToolTip="Cancella oggetto" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                            Rows="5" TextMode="MultiLine" MaxLength="500" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td rowspan="2">
                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;<asp:Label ID="StrutturaLabel" runat="server" Font-Bold="True" Style="width: 200px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Settore/Ufficio" CssClass="Etichetta" />
                                                                </td>
                                                                <td align="right">
                                                                    <asp:ImageButton ID="AggiornaStrutturaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                        Style="display: none" />
                                                                    &nbsp;<asp:ImageButton ID="TrovaStrutturaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                        ToolTip="Seleziona struttura..." />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 150px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <telerik:RadGrid ID="StruttureGridView" runat="server" ToolTip="Elenco degli uffici/settori"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="100%">
                                                                <MasterTableView DataKeyNames="Id">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                            <HeaderStyle Width="30px" />
                                                                            <ItemStyle Width="30px" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                            HeaderText="Descrizione" SortExpression="Descrizione" UniqueName="Descrizione">
                                                                            <HeaderStyle Width="500px" />
                                                                            <ItemStyle Width="500px" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                            ImageUrl="~\images\Delete16.png" UniqueName="Delete">
                                                                            <HeaderStyle Width="20px" />
                                                                            <ItemStyle Width="20px" />
                                                                        </telerik:GridButtonColumn>
                                                                    </Columns>
                                                                </MasterTableView></telerik:RadGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 430px; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;<asp:Label ID="ClassificazioneLabel" runat="server" Font-Bold="True" Style="width: 100px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Classificazione" CssClass="Etichetta" />
                                                                </td>
                                                                <td align="right">
                                                                    <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                        Width="10px" Style="display: none" />
                                                                    <asp:ImageButton ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                        Style="display: none" />
                                                                </td>
                                                                <td style="width: 25px; text-align: center">
                                                                    <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                        ImageAlign="AbsMiddle" ToolTip="Seleziona classificazione..." />
                                                                </td>
                                                                <td style="width: 25px; text-align: center">
                                                                    <asp:ImageButton ID="EliminaClassificazioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                        ImageAlign="AbsMiddle" ToolTip="Cancella classificazione" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadTextBox ID="ClassificazioneTextBox" runat="server" Skin="Office2007"
                                                            Width="100%" Enabled="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="OggettiPanel" runat="server" Style="padding-top: 3px">
                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Elenco Oggetti" CssClass="Etichetta" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 310px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="OggettiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                    Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="400px" ItemStyle-Width="400px" DataField="Contenuto"
                                                                FilterControlAltText="Filter Contenuto column" HeaderText="Contenuto" SortExpression="Contenuto"
                                                                UniqueName="Contenuto">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Contenuto")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 400px; border: 0px solid red">
                                                                        <%# Eval("Contenuto")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="390px" ItemStyle-Width="390px" DataField="Classificazione"
                                                                FilterControlAltText="Filter Classificazione column" HeaderText="Classificazione"
                                                                SortExpression="Classificazione" UniqueName="Classificazione">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Classificazione")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 390px; border: 0px solid red">
                                                                        <%# Eval("Classificazione")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn Text="Seleziona Oggetto e Chiudi" FilterControlAltText="Filter ConfirmSelectAndClose column"
                                                                ImageUrl="~/images/accept.png" UniqueName="ConfirmSelectAndClose" ButtonType="ImageButton"
                                                                CommandName="ConfirmSelectAndClose" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                Text="Seleziona Oggetto" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                ImageUrl="~\images\checks.png" UniqueName="Select" />
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
