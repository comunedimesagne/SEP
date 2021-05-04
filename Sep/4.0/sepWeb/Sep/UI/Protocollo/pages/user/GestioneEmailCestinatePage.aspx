<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneEmailCestinatePage.aspx.vb" Inherits="GestioneEmailCestinatePage" %>

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
                    <table style="width: 900px; height: 780px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td valign="top">
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                    height: 100%">
                                    <tr style="height: 20px">
                                        <td valign="top">
                                            <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="FiltroEmailLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Filtro E-mail" />
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
                                    <tr style="height: 30px">
                                        <td valign="top">
                                            <%-- INIZIO FILTRO--%>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 90px">
                                                        <asp:Label ID="DestinatarioLabel" runat="server" CssClass="Etichetta" Text="Destinatario" />
                                                    </td>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 610px">
                                                                    <telerik:RadComboBox ID="CaselleEmailComboBox" runat="server" EmptyMessage="- Selezionare -"
                                                                        Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                                                        Width="600px" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 90px">
                                                        <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                    </td>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 310px">
                                                                    <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="300px" />
                                                                </td>
                                                                <td style="width: 100px; text-align: center;">
                                                                    <asp:Label ID="MittenteLabel" runat="server" CssClass="Etichetta" Text="Mittente" />
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTextBox ID="MittenteTextBox" runat="server" Skin="Office2007" Width="300px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- FINE FILTRO--%>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px">
                                        <td valign="top">
                                            <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloElencoMailLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Elenco E-mail Cestinate" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="EliminaEmailSelezionateImageButton" Style="border: 0" runat="server"
                                                            ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella e-mail selezionate" ImageAlign="AbsMiddle" />
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
                                                        <div id="scrollPanel" runat="server" style="overflow: auto; height: 100%; border: 1px solid #5D8CC9">
                                                            <telerik:RadGrid ID="EmailGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="880px" AllowSorting="True"
                                                                Culture="it-IT" PageSize="20" AllowMultiRowSelection="True">
                                                                <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                            HeaderStyle-Width="33px" ItemStyle-Width="33px" HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-VerticalAlign="Middle" AllowFiltering="False">
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
                                                                        <telerik:GridTemplateColumn SortExpression="DataCancellazione" UniqueName="DataCancellazione"
                                                                            HeaderText="Cancellata il" AllowFiltering="False" DataField="DataInvio" HeaderStyle-Width="105px"
                                                                            ItemStyle-Width="105px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DataCancellazione","{0:dd/MM/yyyy hh:mm}")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; text-overflow: ellipsis; width: 100%">
                                                                                    <%# Eval("DataCancellazione", "{0:dd/MM/yyyy hh:mm}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Mittente" UniqueName="Mittente" HeaderText="Mittente"
                                                                            DataField="Mittente" HeaderStyle-Width="270px" ItemStyle-Width="270px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Mittente")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                    <%# Eval("Mittente")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                            DataField="Oggetto">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                    <%# Eval("Oggetto")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                            ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="30px"
                                                                            ItemStyle-Width="30px" />
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Recupera" FilterControlAltText="Filter Recupera column"
                                                                            ImageUrl="~\images\undo.png" UniqueName="Recupera" HeaderStyle-Width="30px" ItemStyle-Width="30px" />
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                            ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="30px"
                                                                            ItemStyle-Width="30px" />
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="30px" ItemStyle-Width="30px" AllowFiltering="false">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="OperatoreImage" runat="Server" ImageAlign="AbsMiddle" ImageUrl="~/images/UserInfo16.png"
                                                                                    ToolTip='<%# "Cancellata da: " & Eval("UtenteCancellazione")%>' />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
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
                    <asp:ImageButton ID="VisualizzaRegistrazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                        Style="display: none" />
                    <asp:ImageButton ID="AggiornaEmailButton" runat="server" ImageUrl="~/images//knob-search16.png"
                        Style="display: none" />
                    <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
