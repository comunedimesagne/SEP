<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="PassaggioNuovoAnnoPage.aspx.vb" Inherits="PassaggioNuovoAnnoPage" %>

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

                var h = document.getElementById("lyrCorpoPagina").offsetHeight;
                var w = document.getElementById("lyrCorpoPagina").offsetWidth;


                _backgroundElement.style.width = w + 'px';
                _backgroundElement.style.height = h + 'px';

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
    <asp:UpdateProgress runat="server" ID="UpdateProgress1">
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px;">
                <table style="padding: 20px; background-color: #4892FF">
                    <tr>
                        <td>
                            <div id="Div1" style="width: 300px; text-align: center; background-color: #BFDBFF;
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
                    <table width="600px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr class="ContainerHeader">
                                        <td>
                                            Gestione anno esercizio
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td align="center" style="height: 60px">
                                                        <asp:Label ID="AnnoCorrenteLabel" runat="server" Text="" Font-Size="Medium" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="height: 60px">
                                                        <asp:Label ID="AnnoSuccessivoLabel" runat="server" Text="" Font-Size="Medium" />
                                                    </td>
                                                </tr>
                                                <tr class="GridFooter">
                                                    <td align="center" colspan="1">
                                                        <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="80px" Skin="Office2007"
                                                            ToolTip="Esegui passaggio a nuovo anno">
                                                            <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
