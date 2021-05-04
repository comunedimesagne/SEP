<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InvioMailDistintaPostale.aspx.vb"
    Inherits="InvioMailDistintaPostale" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invio Mail Distinta Postale</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");

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
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px;
                z-index: 2000000">
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
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <center>
                    <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:Label ID="TitleLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                            Text="Destinatario" />
                                                    </td>
                                                    <td style="text-align: right">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0px" cellspacing="4px" width="100%">
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 80px">
                                                                        <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Mittente" />
                                                                    </td>
                                                                    <td style="width: 130px">
                                                                        <telerik:RadComboBox ID="MittenteComboBox" AutoPostBack="true" runat="server" EmptyMessage="- Seleziona -"
                                                                            Filter="StartsWith" ItemsPerRequest="5" MaxHeight="200px" Skin="Office2007" Width="345px" />
                                                                    </td>
                                                                    <td style="width: 80px">
                                                                        <asp:Label ID="Label3" runat="server" CssClass="Etichetta" Text="Destinatario" />
                                                                    </td>
                                                                    <td style="width: 130px">
                                                                        <telerik:RadTextBox ID="txtDestinatario" runat="server" Skin="Office2007" Width="345px"
                                                                            MaxLength="150" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 80px">
                                                                        <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                                    </td>
                                                                    <td style="width: 610px">
                                                                        <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="600px"
                                                                            MaxLength="100" />
                                                                    </td>
                                                                    <td style="width: 80px">
                                                                        <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="Mail Distinte" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkMailDistinte" runat="server" ToolTip="Manda Mail Distinte oppure Mail Cumulativa" />
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
                                        <td>
                                            <div style="overflow: auto; height: 570px; width: 99.8%; border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="grigliaProtocolli" runat="server" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowMultiRowSelection="true"
                                                    AllowSorting="true" AllowPaging="true" PageSize="18" Culture="it-IT" TableLayout="Fixed">
                                                    <MasterTableView DataKeyNames="IdRegistrazione">
                                                        <Columns>
                                                            <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                AllowFiltering="False" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="true"
                                                                        runat="server"></asp:CheckBox>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="true"
                                                                        runat="server"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                                HeaderText="N." DataField="NumeroProtocollo" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                                                AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("NumeroProtocollo", "")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 50px;">
                                                                        <%# Eval("NumeroProtocollo", "")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="DataProtocollo" UniqueName="DataProtocollo"
                                                                HeaderText="Data" DataField="DataProtocollo" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("DataProtocollo","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 60px;">
                                                                        <%# Eval("DataProtocollo", "{0:dd/MM/yyyy}")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                DataField="Oggetto" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                                                <ItemTemplate>
                                                                    <div id="pi" title='<%# Eval("oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 250px;">
                                                                        <%# Eval("oggetto", "")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="dimensioneAllegati" HeaderText="Dim. Allegati [MByte]"
                                                                ShowFilterIcon="False" AllowSorting="false" UniqueName="dimensioneAllegati" AllowFiltering="False">
                                                                <HeaderStyle Width="60px" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="InviaButton" runat="server" Text="Invia" Width="100px" Skin="Office2007"
                                                ToolTip="Invia E-Mail">
                                                <Icon PrimaryIconUrl="../../../../images/mail16.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
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
