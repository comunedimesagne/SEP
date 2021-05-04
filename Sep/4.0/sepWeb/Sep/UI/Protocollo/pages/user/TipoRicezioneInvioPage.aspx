<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="TipoRicezioneInvioPage.aspx.vb" Inherits="TipoRicezioneInvioPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%--SPDX-License-Identifier: GPL-3.0-only--%>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
            $get("pageContent").appendChild(_backgroundElement);

            //QUESTA FUNZIONE E' DEFINITA NELLA MAINPAGE E NELLA BASEPAGE

            ResetDirty();
            OnChangeHandler();

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
    <asp:UpdatePanel ID="Pannello" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <div id="pageContent">
                <asp:TextBox runat="server" ID="messaggio" Style="display: none" />
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
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                    CommandName="Stampa" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Descrizione *"
                                            ForeColor="#FF8040" />
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="700px" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table style="width: 100%; background-color: #BFDBFF; border: 1 solid #5D8CC9">
                                <tr>
                                    <td style="height: 20px">
                                        &nbsp;<asp:Label ID="TitoloElencoTipiRicezioneInvioLabel" runat="server" Font-Bold="True"
                                            Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco tipi ricezione/invio" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: auto; height: 320px; width: 100%; background-color: #FFFFFF;
                                            border: 1px solid #5D8CC9;">
                                            <telerik:RadGrid ID="TipiRicezioneInvioGridView" runat="server" AllowPaging="True"
                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                Width="99.8%" AllowSorting="True" Culture="it-IT" PageSize="10">
                                                <MasterTableView DataKeyNames="Id">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                            HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                            HeaderText="Descrizione" SortExpression="Descrizione" UniqueName="Descrizione">
                                                            <HeaderStyle Width="890px" />
                                                            <ItemStyle Width="890px" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                            ImageUrl="~\images\checks.png" UniqueName="Select">
                                                            <HeaderStyle Width="10px" />
                                                            <ItemStyle Width="10px" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
