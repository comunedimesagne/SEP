<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VisualizzaFatturaPage.aspx.vb"
    Inherits="VisualizzaFatturaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Anteprima Fattura</title>
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

    function PrintContent() {
        var content = $get("pannelloFattura").innerHTML;

        var printIframe = $get("ifmcontentstoprint");
        var printDocument = printIframe.contentWindow.document;
        printDocument.designMode = "on";
        printDocument.open();
        printDocument.write("<html><head></head><body>" + content + "</body></html>");
        printDocument.close();
        try {
            if (document.all) {
                printDocument.execCommand("Print", null, false);
            }
            else {
                printIframe.contentWindow.print();
            }
        }
        catch (ex) {
            alert(ex.Message);
        }
    }
       
</script>
<body onload="self.focus();">
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
    <iframe id="ifmcontentstoprint" style="position: absolute; top: -1000px; left: -1000px;">
    </iframe>
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent" style="overflow: auto; border: 0px solid red; height: 100%">
                <center>
                    <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text="Anteprima Fattura" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pannelloFattura" runat="server" Height="300px" Style="overflow: auto;">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="GrigliaAllegatiPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                <tr>
                                                                    <td style="height: 20px">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr style="background-color: #FFFFFF">
                                                                    <td>
                                                                        <div style="overflow: auto; height: 100px; border: 1px solid #5D8CC9">
                                                                            <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco allegati associati alla fattura elettronica"
                                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                Width="99.8%" Culture="it-IT">
                                                                                <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                    <Columns>
                                                                                        <telerik:GridTemplateColumn SortExpression="Posizione" UniqueName="Posizione" HeaderText="Posizione"
                                                                                            DataField="Posizione" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("Posizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                    text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                                                    <%# Eval("Posizione")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn SortExpression="Estremi" UniqueName="Estremi" HeaderText="Estremi"
                                                                                            DataField="Estremi" HeaderStyle-Width="230px" ItemStyle-Width="230px">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("Estremi")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                    text-overflow: ellipsis; width: 230px; border: 0px solid red">
                                                                                                    <%# Eval("Estremi")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                            DataField="NomeFile" HeaderStyle-Width="480px" ItemStyle-Width="480px">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                    text-overflow: ellipsis; width: 480px; border: 0px solid red">
                                                                                                    <%# Eval("NomeFile")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                            Text="Visualizza Allegato..." ImageUrl="~\images\knob-search16.png" UniqueName="Preview"
                                                                                            HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                                                        </telerik:GridButtonColumn>
                                                                                    </Columns>
                                                                                </MasterTableView></telerik:RadGrid>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                                        <telerik:RadButton ID="VisualizzaFatturaButton" runat="server" Text="Tabellare" Width="100px"
                                                            Skin="Office2007" ToolTip="Imposta layout visualizzazione fattura">
                                                            <Icon PrimaryIconUrl="../../../../images/Table.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                        &nbsp; &nbsp; &nbsp;
                                                        <telerik:RadButton ID="StampaFatturaButton" runat="server" Text="Stampa" Width="90px"
                                                            Skin="Office2007" ToolTip="Stampa Fattura">
                                                            <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                        &nbsp; &nbsp; &nbsp;
                                                        <telerik:RadButton ID="ChiudiFatturaButton" runat="server" Text="Chiudi" Width="90px"
                                                            Skin="Office2007" ToolTip="Chiudi">
                                                            <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
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
