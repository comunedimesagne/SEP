<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="StampaDistintaPostalePage.aspx.vb" Inherits="StampaDistintaPostalePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
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
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <center>
                    <table style="width: 600px; height: 520px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td valign="top">
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                    height: 100%">
                                    <tr style="height: 20px">
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="PannelloFiltroLabel" runat="server" Font-Bold="True" Style="color: #00156E;
                                                            background-color: #BFDBFF" Text="Filtro Registrazione" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                    </td>
                                                    <td align="center" style="width: 30px">
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
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 130px">
                                                                    <asp:Label ID="NumeroProtocolloLabel" runat="server" CssClass="Etichetta" Text="Numero prot." />
                                                                </td>
                                                                <td style="width: 30px">
                                                                    <asp:Label ID="NumeroProtocolloInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                </td>
                                                                <td style="width: 150px">
                                                                    <telerik:RadNumericTextBox ID="NumeroProtocolloInizioTextBox" runat="server" Skin="Office2007"
                                                                        Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                        ShowSpinButtons="True" ToolTip="">
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                    </telerik:RadNumericTextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="NumeroProtocolloFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                </td>
                                                                <td>
                                                                    <telerik:RadNumericTextBox ID="NumeroProtocolloFineTextBox" runat="server" Skin="Office2007"
                                                                        Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                        ShowSpinButtons="True" ToolTip="">
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                    </telerik:RadNumericTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 130px">
                                                                    <asp:Label ID="DataProtocolloLabel" runat="server" CssClass="Etichetta" Text="Data prot." />
                                                                </td>
                                                                <td style="width: 30px">
                                                                    <asp:Label ID="DataProtocolloInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                </td>
                                                                <td style="width: 150px">
                                                                    <telerik:RadDatePicker ID="DataProtocolloInizioTextBox" Skin="Office2007" Width="110px"
                                                                        runat="server" MinDate="1753-01-01" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="DataProtocolloFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="DataProtocolloFineTextBox" runat="server" MinDate="1753-01-01"
                                                                        Skin="Office2007" Width="110px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                                    <tr>
                                                                                        <td>
                                                                                            &nbsp;<asp:Label ID="ElencoUfficiLabel" runat="server" Font-Bold="True" Style="width: 400px;
                                                                                                color: #00156E; background-color: #BFDBFF" Text="Tipologia invio" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="SelectAllCheckBox" runat="server" Text="Seleziona tutto" AutoPostBack="True" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadListBox ID="TipoRicezioneInvioListBox" runat="server" Skin="Office2007"
                                                                                    Style="width: 600px; height: 360px" Height="360px" SortCaseSensitive="False"
                                                                                    Sort="Ascending" CheckBoxes="True" SelectionMode="Multiple">
                                                                                </telerik:RadListBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="GridFooter">
                                                    <td align="center">
                                                        <telerik:RadButton ID="AnteprimaStampaButton" runat="server" Text="Stampa" Width="100px"
                                                            Skin="Office2007" ToolTip="Effettua la stampa con i filtri impostati">
                                                            <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                        &nbsp;&nbsp;
                                                        <telerik:RadButton ID="inviaMailButton" runat="server" Text="Mail" Width="100px"
                                                            Skin="Office2007" ToolTip="Consente l'invio di una mail con i filtri impostati">
                                                            <Icon PrimaryIconUrl="../../../../images/mail16.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                        &nbsp;&nbsp;
                                                        <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="100px"
                                                            Skin="Office2007" ToolTip="Annulla i filtri impostati">
                                                            <Icon PrimaryIconUrl="../../../../images/Annulla.png" PrimaryIconLeft="5px" />
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
</asp:Content>
