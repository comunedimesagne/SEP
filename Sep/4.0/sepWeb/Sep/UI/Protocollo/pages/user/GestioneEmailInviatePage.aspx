<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneEmailInviatePage.aspx.vb" Inherits="GestioneEmailInviatePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <telerik:RadFormDecorator ID="rfd_check" runat="server" DecoratedControls="CheckBoxes" />
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
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="200">
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px">
                <table cellpadding="4" style="background-color: #4892FF">
                    <tr>
                        <td>
                            <div id="loadingContainer" style="width: 300px; text-align: center; background-color: #BFDBFF;
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
                    <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
                    <div style="padding: 20px 2px 2px 2px; width: 100%">
                        <table cellpadding="0" style="width: 900px; background-color: #BFDBFF; border: 1px solid #5D8CC9;">
                            <tr>
                                <td>
                                    <table style="width: 100%; background-color: #BFDBFF">
                                        <tr>
                                            <td>
                                                <asp:Label ID="TitoloElencoMailLabel" Style="width: 755px; color: #00156E;" runat="server"
                                                    CssClass="Etichetta" Font-Bold="True" Text="Elenco E-mail Inviate" />
                                            </td>
                                            <td align="center" style="width: 30px">
                                                <asp:ImageButton ID="InvioMassivoImageButton" runat="server" ImageUrl="~/images//SendEmail.png"
                                                    Style="border: 0px" ToolTip="Esegui invio massivo delle e-mail selezionate" Width="16px"
                                                    ImageAlign="Top" />
                                            </td>
                                            <td align="center" style="width: 30px">
                                                <asp:ImageButton ID="EsportaExcelImageButton" runat="server" ImageUrl="~/images//export.png"
                                                    Style="border-style: none; border-color: inherit; border-width: 0px; width: 16px;"
                                                    ToolTip="Esporta e-mail visualizzate in un file formato excel" ImageAlign="Top" />
                                            </td>
                                            <td align="center" style="width: 20px">
                                                <asp:ImageButton ID="SeparatoreImageButton" runat="server" ImageUrl="~/images//NavigatorSeparator.png"
                                                    Style="border: 0px" ImageAlign="Top" />
                                            </td>
                                            <td align="center" style="width: 30px">
                                                <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                    ToolTip="Effettua la ricerca con i filtri impostati" Style="border-style: none;
                                                    border-color: inherit; border-width: 0; width: 16px;" ImageAlign="AbsMiddle" />
                                            </td>
                                            <td align="center" style="width: 30">
                                                <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                    ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="background-color: #FFFFFF">
                                <td>
                                    <table style="width: 100%; border-right: 3px solid #BFDBFF; border-left: 3px solid #BFDBFF">
                                        <tr style="height: 40px">
                                            <td style="width: 70px">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="Etichetta" Text="Data" />
                                            </td>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 30px">
                                                            <asp:Label ID="DataInvioInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                        </td>
                                                        <td style="width: 150px">
                                                            <telerik:RadDatePicker ID="DataInvioInizioTextBox" Skin="Office2007" Width="110px"
                                                                runat="server" MinDate="1753-01-01" />
                                                        </td>
                                                        <td style="width: 30px">
                                                            <asp:Label ID="DataInvioFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                        </td>
                                                        <td style="width: 150px">
                                                            <telerik:RadDatePicker ID="DataInvioFineTextBox" Skin="Office2007" Width="110px"
                                                                runat="server" MinDate="1753-01-01" />
                                                        </td>
                                                        <td style="width: 60px">
                                                            <asp:Label ID="InviateLabel" runat="server" CssClass="Etichetta" Text="Inviate" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="EmailInviateComboBox" runat="server" Skin="Office2007" Width="100px"
                                                                EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="background-color: #FFFFFF">
                                <td>
                                    <div id="scrollPanel" runat="server" style="overflow: auto; width: 100%; height: 680px;
                                        background-color: #FFFFFF; border-right: 3px solid #BFDBFF; border-left: 3px solid #BFDBFF;
                                        border-bottom: 3px solid #BFDBFF">
                                        <telerik:RadGrid ID="EmailGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            AllowFilteringByColumn="True" CellSpacing="0" GridLines="None" Skin="Office2007"
                                            Width="99.8%" AllowSorting="True" Culture="it-IT" PageSize="20" AllowMultiRowSelection="True">
                                            <MasterTableView DataKeyNames="Id">
                                                <NestedViewTemplate>
                                                    <div style="margin-left: 30px">
                                                        <telerik:RadGrid ID="AllegatiGridView" runat="server" AutoGenerateColumns="False"
                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="700px" AllowSorting="True"
                                                            AllowPaging="False">
                                                            <MasterTableView DataKeyNames="IdAllegato">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Nomefile" FilterControlAltText="Filter Nomefile column"
                                                                        ItemStyle-Width="680px" HeaderStyle-Width="680px" HeaderText="Nomefile" ReadOnly="True"
                                                                        SortExpression="Nomefile" UniqueName="Nomefile" />
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" ItemStyle-Width="20px"
                                                                        HeaderStyle-Width="20px" FilterControlAltText="Filter Preview column" ImageUrl="~\images\knob-search16.png"
                                                                        UniqueName="Preview" />
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </NestedViewTemplate>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderTooltip="Seleziona tutto"
                                                        AllowFiltering="False">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                runat="server"></asp:CheckBox>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                runat="server"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                    <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="150px"
                                                        HeaderStyle-Width="150px" AutoPostBackOnFilter="True" DataField="EmailMittente"
                                                        FilterControlAltText="Filter EmailMittente column" FilterControlWidth="100%"
                                                        HeaderText="Mittente" ShowFilterIcon="False" SortExpression="EmailMittente" UniqueName="EmailMittente">
                                                        <ItemTemplate>
                                                            <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 150px"
                                                                title='<%# Eval("EmailMittente")%>'>
                                                                <%# Eval("EmailMittente")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="150px"
                                                        HeaderStyle-Width="150px" AutoPostBackOnFilter="True" DataField="Destinatari"
                                                        FilterControlAltText="Filter Destinatari column" FilterControlWidth="100%" HeaderText="Destinatario"
                                                        ShowFilterIcon="False" SortExpression="Destinatari" UniqueName="Destinatari">
                                                        <ItemTemplate>
                                                            <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 150px"
                                                                title='<%# Eval("Destinatari")%>'>
                                                                <%# Eval("Destinatari")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="140px"
                                                        HeaderStyle-Width="140px" AutoPostBackOnFilter="True" DataField="Oggetto" FilterControlAltText="Filter Oggetto column"
                                                        FilterControlWidth="100%" HeaderText="Oggetto" ShowFilterIcon="False" SortExpression="Oggetto"
                                                        UniqueName="Oggetto">
                                                        <ItemTemplate>
                                                            <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 140px"
                                                                title='<%# Eval("Oggetto")%>'>
                                                                <%# Eval("Oggetto")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="140px"
                                                        HeaderStyle-Width="140px" AutoPostBackOnFilter="True" DataField="Corpo" FilterControlAltText="Filter Corpo column"
                                                        FilterControlWidth="100%" HeaderText="Corpo" ShowFilterIcon="False" SortExpression="Corpo"
                                                        UniqueName="Corpo">
                                                        <ItemTemplate>
                                                            <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 140px"
                                                                title='<%# Eval("Corpo")%>'>
                                                                <%# Eval("Corpo")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn SortExpression="DataInvio" UniqueName="DataInvio" HeaderText="Data invio"
                                                        AllowFiltering="False" DataField="DataInvio" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DataInvio","{0:dd/MM/yyyy hh:mm}")%>' style="white-space: nowrap;
                                                                overflow: hidden; text-overflow: ellipsis; width: 90px">
                                                                <%# Eval("DataInvio", "{0:dd/MM/yyyy hh:mm}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                        CommandName="Preview" FilterControlAltText="Filter Preview column" ImageUrl="~\images\knob-search16.png"
                                                        UniqueName="Preview">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ItemStyle-Width="20px" HeaderStyle-Width="20px" ButtonType="ImageButton"
                                                        CommandName="SendMail" FilterControlAltText="Filter SendMail column" ImageUrl="~\images\mailSend.png"
                                                        UniqueName="SendMail">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ItemStyle-Width="20px" HeaderStyle-Width="20px" ButtonType="ImageButton"
                                                        CommandName="Delete" FilterControlAltText="Filter Delete column" ImageUrl="~\images\Delete16.png"
                                                        UniqueName="Delete">
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
