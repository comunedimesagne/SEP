<%@ Page Title="Stampa Bozze" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="StampaBozzePage.aspx.vb" Inherits="StampaBozzePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
                // _backgroundElement.className = "modalBackground";
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
                    <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                        DecorationZoneID="ZoneID" Skin="Web20"></telerik:RadFormDecorator>
                    <table width="450px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text="Stampa Bozze" CssClass="Etichetta" />
                                        </td>
                                    </tr>
                                    <%-- BODY--%>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 80px; width: 100%; background-color: #DFE8F6;
                                                            border: 0px solid #5D8CC9;">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 70px">
                                                                        <asp:Label ID="NumeroLabel" runat="server" CssClass="Etichetta" Text="Numero" />
                                                                    </td>
                                                                    <td>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 30px">
                                                                                    <asp:Label ID="NumeroInizioLabel" runat="server" CssClass="Etichetta" Text="da" ForeColor="#00156E" />
                                                                                </td>
                                                                                <td style="width: 150px">
                                                                                    <telerik:RadTextBox ID="NumeroInizioTextBox" runat="server" Skin="Office2007" Width="80px" />
                                                                                </td>
                                                                                <td style="width: 30px">
                                                                                    <asp:Label ID="NumeroFineLabel" runat="server" CssClass="Etichetta" Text="a" ForeColor="#00156E" />
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadTextBox ID="NumeroFineTextBox" runat="server" Skin="Office2007" Width="80px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 70px">
                                                                        <asp:Label ID="DataLabel" runat="server" CssClass="Etichetta" Text="Data" />
                                                                    </td>
                                                                    <td>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 30px">
                                                                                    <asp:Label ID="DataInizioLabel" runat="server" CssClass="Etichetta" Text="da" ForeColor="#00156E" />
                                                                                </td>
                                                                                <td style="width: 150px">
                                                                                    <telerik:RadDatePicker ID="DataInizioTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                                        MinDate="1753-01-01" />
                                                                                </td>
                                                                                <td style="width: 30px">
                                                                                    <asp:Label ID="DataAFineLabel" runat="server" CssClass="Etichetta" Text="a" ForeColor="#00156E" />
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadDatePicker ID="DataFineTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                                        MinDate="1753-01-01" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%-- FOOTER--%>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="StampaButton" runat="server" Text="Stampa" Width="90px" Skin="Office2007"
                                                ToolTip="Effettua la stampa con i filtri impostati">
                                                <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="90px"
                                                Skin="Office2007" ToolTip="Annulla i filtri impostati">
                                                <Icon PrimaryIconUrl="../../../../images/Annulla.png" PrimaryIconLeft="5px" />
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
</asp:Content>
