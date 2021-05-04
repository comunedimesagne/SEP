<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneEmergenzaPage.aspx.vb" Inherits="GestioneEmergenzaPage" %>

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
    <asp:UpdatePanel ID="Pannello" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <div id="pageContent">
                <asp:TextBox runat="server" ID="messaggio" Style="display: none" />
                <table style="width: 900px">
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
                <table style="width: 900px">
                    <tr>
                        <td>
                            <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *"
                                ForeColor="#FF8040" />
                        </td>
                        <td>
                            <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="600px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="PeriodoLabel" runat="server" CssClass="Etichetta" Text="Periodo" />
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                        <asp:Label ID="DataEmergenzaInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                    </td>
                                    <td style="width: 230px">
                                        <telerik:RadDatePicker ID="DataEmergenzaInizioTextBox" Skin="Office2007" Width="110px"
                                            runat="server" MinDate="1753-01-01" />
                                        &nbsp;
                                        <telerik:RadTimePicker ID="OrarioEmergenzaInizioTextBox" Skin="Office2007" Width="70px"
                                            runat="server" />
                                    </td>
                                    <td style="width: 30px">
                                        <asp:Label ID="DataEmergenzaFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="DataEmergenzaFineTextBox" runat="server" MinDate="1753-01-01"
                                            Skin="Office2007" Width="110px" />
                                        &nbsp;
                                        <telerik:RadTimePicker ID="OrarioEmergenzaFineTextBox" Skin="Office2007" Width="70px"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="NumeroRegistrazioniLabel" runat="server" CssClass="Etichetta" Text="N. registrazioni" />
                        </td>
                        <td>
                            <telerik:RadTextBox ID="NumeroRegistrazioniTextBox" runat="server" Skin="Office2007"
                                Width="70px" MaxLength="5" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="CausaLabel" runat="server" CssClass="Etichetta" Text="Motivo" />
                        </td>
                        <td>
                            <telerik:RadTextBox ID="CausaTextBox" runat="server" Skin="Office2007" Width="600px"
                                Rows="4" TextMode="MultiLine" MaxLength="1000" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox ID="SbloccaSistemaCheckBox" runat="server" Text="Dopo il salvataggio sblocca il sistema" />
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 900px; background-color: #BFDBFF; border: 1 solid #00156E">
                    <tr>
                        <td style="height: 20px">
                            &nbsp;<asp:Label ID="TitoloElencoSessioniEmergenzaLabel" runat="server" Font-Bold="True"
                                Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco sessioni emergenza" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadGrid ID="SessioniEmergenzaGridView" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                Width="900px" AllowSorting="True">
                                <MasterTableView DataKeyNames="Id">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                            HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                            HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="380px" ItemStyle-Width="380px">
                                            <ItemTemplate>
                                                <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                    text-overflow: ellipsis; width: 380px;">
                                                    <%# Eval("Descrizione")%></div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="DataOraInizio" UniqueName="DataOraInizio"
                                            HeaderText="Data inizio" DataField="DataOraInizio" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <div title='<%# Eval("DataOraInizio","{0:dd/MM/yyyy hh:mm}")%>' style="white-space: nowrap;
                                                    overflow: hidden; text-overflow: ellipsis; width: 90px;">
                                                    <%# Eval("DataOraInizio", "{0:dd/MM/yyyy hh:mm}")%></div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="DataOraFine" UniqueName="DataOraFine"
                                            HeaderText="Data fine" DataField="DataOraFine" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <div title='<%# Eval("DataOraFine","{0:dd/MM/yyyy hh:mm}")%>' style="white-space: nowrap;
                                                    overflow: hidden; text-overflow: ellipsis; width: 90px;">
                                                    <%# Eval("DataOraFine", "{0:dd/MM/yyyy hh:mm}")%></div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="NumeroRegistrazioni" UniqueName="NumeroRegistrazioni"
                                            HeaderText="N. registrazioni" DataField="NumeroRegistrazioni" HeaderStyle-Width="70px"
                                            ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <div title='<%# Eval("NumeroRegistrazioni")%>' style="white-space: nowrap; overflow: hidden;
                                                    text-overflow: ellipsis; width: 70px;">
                                                    <%# Eval("NumeroRegistrazioni")%></div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                            ImageUrl="~\images\checks.png" UniqueName="Select">
                                            <HeaderStyle Width="10px" />
                                            <ItemStyle Width="10px" />
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
