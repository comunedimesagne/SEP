<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InviaEmailRegistrazionePage.aspx.vb"
    Inherits="InviaEmailRegistrazionePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invia Email</title>
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
            var clientBounds = this.GetClientBounds();
            var clientWidth = clientBounds.width;
            var clientHeight = clientBounds.height;
            _backgroundElement.style.width = Math.max(Math.max(document.documentElement.scrollWidth, document.body.scrollWidth), clientWidth) + 'px';
            _backgroundElement.style.height = Math.max(Math.max(document.documentElement.scrollHeight, document.body.scrollHeight), clientHeight) + 'px';
            _backgroundElement.style.zIndex = 10000;
            _backgroundElement.style.backgroundColor = '#09718F';
            _backgroundElement.style.filter = "alpha(opacity=20)";
            _backgroundElement.style.opacity = "0.2";

        }
        else {
            _backgroundElement.style.display = 'none';

        }
    }

    function GetClientBounds() {
        var clientWidth;
        var clientHeight;
        switch (Sys.Browser.agent) {
            case Sys.Browser.InternetExplorer:
                clientWidth = document.documentElement.clientWidth;
                clientHeight = document.documentElement.clientHeight;
                break;
            case Sys.Browser.Safari:
                clientWidth = window.innerWidth;
                clientHeight = window.innerHeight;
                break;
            case Sys.Browser.Opera:
                clientWidth = Math.min(window.innerWidth, document.body.clientWidth);
                clientHeight = Math.min(window.innerHeight, document.body.clientHeight);
                break;
            default:  // Sys.Browser.Firefox, etc.
                clientWidth = Math.min(window.innerWidth, document.documentElement.clientWidth);
                clientHeight = Math.min(window.innerHeight, document.documentElement.clientHeight);
                break;
        }
        return new Sys.UI.Bounds(0, 0, clientWidth, clientHeight);
    }      
</script>
<body runat="server" id="CorpoPagina">
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 300px">
                <table cellpadding="4" style="background-color: #4892FF;">
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
    <telerik:RadScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="360000" />
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <center>
                    <table style="width: 890px; height: 100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td valign="top">
                                <table style="width: 100%; height: 100%" cellpadding="5" cellspacing="5" border="0">
                                    <tr>
                                        <td valign="top">
                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                                height: 100%">
                                                <tr style="height: 20px">
                                                    <td valign="top" style="border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8">
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="InviaEmailLabel" runat="server" Font-Bold="True" Style="width: 700px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Invia e-mail" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 340px">
                                                    <td valign="top">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 100px" align="right">
                                                                    <asp:Label ID="MittenteLabel" runat="server" CssClass="Etichetta" Text="Mittente" />
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="CaselleEmailComboBox" runat="server" Skin="Office2007" Width="330px"
                                                                                    EmptyMessage="Selezionare Mittente" ItemsPerRequest="10" Filter="StartsWith"
                                                                                    NoWrap="True" MaxHeight="400px" TabIndex="1" AutoPostBack="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px" align="right">
                                                                    <asp:Label ID="DestinatariLabel" runat="server" CssClass="Etichetta" Text="Destinatari" />
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadListBox ID="DestinatariListBox" runat="server" Skin="Office2007" Style="width: 100%;
                                                                                    height: 120px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True" TabIndex="2"
                                                                                    Height="120px">
                                                                                </telerik:RadListBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px" align="right">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Altri Destinatari" />
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="AltriDestinatariTB" runat="server" Skin="Office2007" Width="100%"
                                                                                    MaxLength="1000" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px" align="right">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="Destinatari Cc" />
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="DestinatariCcTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                    MaxLength="1000" ToolTip="Elenco di indirizzi (separati dal punto e virgola) a cui inviare l'email solo per conoscenza" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px" align="right">
                                                                    <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:LinkButton runat="server" ID="OggettoLinkButton" CssClass="Etichetta" Text=""
                                                                                    ToolTip="Visualizzazione del protocollo collegato" TabIndex="6" />
                                                                                <asp:CheckBox ID="RiferimentoProtocolloChkBox" runat="server" AutoPostBack="true"
                                                                                    ToolTip="Cliccando si aggiunge il riferimento del protocollo nell'oggetto dell'e-mail" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                    TabIndex="3" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px" align="right">
                                                                    <asp:Label ID="CorpoLabel" runat="server" CssClass="Etichetta" Text="Corpo" />
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="CorpoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                    Height="100px" MaxLength="1000" TextMode="MultiLine" TabIndex="4" />
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
                                                        <table class="Container" cellpadding="0" cellspacing="4" style="width: 100%; height: 100%;
                                                            background-color: #BFDBFF" border="0">
                                                            <tr style="height: 20px">
                                                                <td valign="middle">
                                                                    <asp:Label ID="AllegatiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                        Style="width: 300px; color: #00156E" Text="Elenco Allegati" />&nbsp;
                                                                    <asp:Label ID="DimensioneAllegtiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                        Style="width: 500px; color: #00156E" ToolTip="Dimensione degli allegati (massimo consentito 20 MB!)"
                                                                        Width="456px" />
                                                                </td>
                                                            </tr>
                                                            <tr style="background-color: #DFE8F6">
                                                                <td valign="top">
                                                                    <div id="scrollPanel" runat="server" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9">
                                                                        <telerik:RadGrid ID="DocumentiGridView" runat="server" AutoGenerateColumns="False"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True"
                                                                            TabIndex="5" ShowFooter="true">
                                                                            <MasterTableView DataKeyNames="Id">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                                        AllowFiltering="False">
                                                                                        <HeaderTemplate>
                                                                                            <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                                                runat="server" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                                                runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                                                                        <ItemStyle Width="20px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridBoundColumn DataField="Id" UniqueName="Id" Visible="False" />
                                                                                    <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="N.">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="NumeratoreLabel" runat="server" Width="10px" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="10px" />
                                                                                        <ItemStyle Width="10px" HorizontalAlign="Right" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn UniqueName="NomeFile" HeaderText="Nome file" DataField="NomeFile"
                                                                                        HeaderStyle-Width="170px" ItemStyle-Width="170px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 170px">
                                                                                                <%# Eval("NomeFile")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn UniqueName="ImprontaEsadecimale" HeaderText="Impronta"
                                                                                        DataField="ImprontaEsadecimale" HeaderStyle-Width="270px" ItemStyle-Width="270px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("ImprontaEsadecimale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 270px;">
                                                                                                <%# Eval("ImprontaEsadecimale")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn UniqueName="Oggetto" HeaderText="Oggetto" DataField="Oggetto"
                                                                                        HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 140px;">
                                                                                                <%# Eval("Oggetto")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn UniqueName="Template2Column" HeaderText="Dim. (MB)" HeaderTooltip="Dimensione del file espressa in MegaByte">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="DimensioneLabel" runat="server" Width="60px" ToolTip="Dimensione in MB del file allegato" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="50px" />
                                                                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="DescrizioneTipologiaDocumento" UniqueName="DescrizioneTipologiaDocumento"
                                                                                        HeaderText="Tipo" DataField="DescrizioneTipologiaDocumento" HeaderStyle-Width="50px"
                                                                                        ItemStyle-Width="50px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DescrizioneTipologiaDocumento")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 50px;">
                                                                                                <%# Eval("DescrizioneTipologiaDocumento")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                                <FooterStyle HorizontalAlign="Right" />
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="GridFooter">
                                                                <td align="center">
                                                                    <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="100px" Skin="Office2007"
                                                                        ToolTip="Chiudi">
                                                                        <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                                                    </telerik:RadButton>
                                                                    &nbsp; &nbsp;
                                                                    <telerik:RadButton ID="InviaEmailButton" runat="server" Text="Invia e-mail" Width="110px"
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
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
                    <asp:HiddenField ID="DimensioneCasellaHidden" runat="server" Value="" />
                    <asp:HiddenField ID="DimensioneAllegatiSelezionatiHidden" runat="server" Value="" />
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
