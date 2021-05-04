<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestionePecPage.aspx.vb" Inherits="GestionePecPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');
        var count = 2;

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
            count = 2;
            var message = $get('<%= infoOperazioneHidden.ClientId %>').value;
            if (message !== '') {
                //VISUALIZZO IL MESSAGGIO
                ShowMessageBox(message);

                var intervallo = setInterval(function () {
                    count = count - 1;
                    if (count <= 0) {
                        HideMessageBox();
                        EnableUI(true);
                        clearInterval(intervallo);

                    }
                }, 1000);
                $get('<%= infoOperazioneHidden.ClientId %>').value = '';

            } else { EnableUI(true); }
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


        function ShowMessageBox(message) {
            this.document.body.appendChild(messageBox);
            this.document.body.appendChild(messageBoxPanel);

            with (messageBoxPanel) {
                style.display = '';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.width = '100%';
                style.height = '100%';
                style.zIndex = 10000;
                style.backgroundColor = '#09718F';
                style.filter = "alpha(opacity=20)";
                style.opacity = "0.2";
            }

            with (messageBox) {
                style.width = '305px';
                style.height = '40px';
                style.backgroundColor = '#BFDBFF';
                style.border = 'solid #4892FF 2px';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.zIndex = 10000;
                style.textAlign = 'center';
                style.verticalAlign = 'middle';
                innerHTML = message;
                style.color = '#00156E';
                style.backgroundImage = 'url(/sep/Images/success.png)';
                style.backgroundPosition = '5px center';
                style.backgroundRepeat = 'no-repeat';
                style.lineHeight = '40px';
            }


            xc = Math.round((document.body.clientWidth / 2) - (300 / 2));
            yc = Math.round((document.body.clientHeight / 2) - (40 / 2));


            messageBox.style.left = xc + "px";
            messageBox.style.top = yc + "px";
            messageBox.style.display = 'block';

        }

        function HideMessageBox() {
            try {
                messageBox.style.display = 'none';
                messageBoxPanel.style.display = 'none';
            }
            catch (e) { }
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
                            <div id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 110px">
                                                        <asp:Label ID="NomeCasellaLabel" runat="server" CssClass="Etichetta" Text="Nome casella *"
                                                            ForeColor="#FF8040" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="NomeCasellaTextBox" runat="server" Skin="Office2007" Width="700px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 110px">
                                                        <asp:Label ID="UserIdLabel" runat="server" CssClass="Etichetta" Text="User id" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="UserIdTextBox" runat="server" Skin="Office2007" Width="340px" />
                                                    </td>
                                                    <td style="text-align: center; width: 90px">
                                                        <asp:Label ID="PasswordLabel" runat="server" CssClass="Etichetta" Text="Password" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="PasswordTextBox" runat="server" Skin="Office2007" Width="340px"
                                                            TextMode="Password" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 110px">
                                                        <asp:Label ID="DimensionemassimaLabel" runat="server" CssClass="Etichetta" Text="Dim. massima" />
                                                    </td>
                                                    <td style="width: 400px">
                                                        <telerik:RadNumericTextBox ID="DimensioneMassimaTextBox" runat="server" Skin="Office2007"
                                                            Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="0"
                                                            ShowSpinButtons="True" ToolTip="Dimensione massima che può raggiungere un'email in uscita (compresi gli allegati)">
                                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                        </telerik:RadNumericTextBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="MantieniCopiaSulServerCheckBox" Text="&nbsp;" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Conserva una copia dei messaggi sul server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="TitoloServerPop3Label" runat="server" Font-Bold="True" Style="width: 270px;
                                                color: #00156E;" Text="Impostazioni server di posta in entrata" />
                                        </td>
                                        <td>
                                            <hr style="width: 610px; background-color: #00156E" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td style="width: 90px">
                                                        <asp:Label ID="Pop3ServerLabel" runat="server" CssClass="Etichetta" Text="Server POP3" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="Pop3ServerTextBox" runat="server" Skin="Office2007" Width="250px" />
                                                    </td>
                                                    <td style="width: 60px; text-align: center">
                                                        <asp:Label ID="Pop3PortaLabel" runat="server" CssClass="Etichetta" Text="Porta" />
                                                    </td>
                                                    <td style="width: 70px">
                                                        <telerik:RadTextBox ID="Pop3PortaTextBox" runat="server" Skin="Office2007" Width="50px" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="ServerPop3UtilizzaSslCheckBox" Text="&nbsp;" />
                                                    </td>
                                                    <td style="width: 80px; text-align: center">
                                                        <asp:Label ID="ServerPop3UtilizzaSslLabel" runat="server" CssClass="Etichetta" Text="Utilizza SSL" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="TitoloServerSmtpLabel" runat="server" Font-Bold="True" Style="width: 270px;
                                                color: #00156E;" Text="Impostazioni server di posta in uscita" />
                                        </td>
                                        <td>
                                            <hr style="width: 610px; background-color: #00156E" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td style="width: 90px">
                                                        <asp:Label ID="SmtpServerLabel" runat="server" CssClass="Etichetta" Text="Server SMTP" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="SmtpServerTextBox" runat="server" Skin="Office2007" Width="250px" />
                                                    </td>
                                                    <td style="width: 60px; text-align: center">
                                                        <asp:Label ID="SmtpPortaLabel" runat="server" CssClass="Etichetta" Text="Porta" />
                                                    </td>
                                                    <td style="width: 70px">
                                                        <telerik:RadTextBox ID="SmtpPortaTextBox" runat="server" Skin="Office2007" Width="50px" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="ServerSmtpUtilizzaSslCheckBox" Text="&nbsp;" />
                                                    </td>
                                                    <td style="width: 90px; text-align: center">
                                                        <asp:Label ID="ServerSmtpUtilizzaSslLabel" runat="server" CssClass="Etichetta" Text="Utilizza SSL" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="ServerSmtpRichiedeAutenticazioneCheckBox" Text="&nbsp;" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="ServerSmtpRichiedeAutenticazioneLabel" runat="server" CssClass="Etichetta"
                                                            Text="Richiede autenticazione" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="PannelloGrigliaUtenti" runat="server" style="padding: 2px 2px 2px 2px;">
                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr style="height: 20px; background-color: #BFDBFF">
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 250px">
                                                        &nbsp;<asp:Label ID="UtentiLabel" runat="server" CssClass="Etichetta" Text="Utenti"
                                                            AccessKey="U" Font-Bold="True" Style="color: #00156E;" AssociatedControlID="TrovaUtenteImageButton" />
                                                    </td>
                                                    <td align="right">
                                                        <telerik:RadTextBox ID="FiltroUtenteTextBox" runat="server" Skin="Office2007" Width="300px" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                            Style="display: none" />
                                                        <telerik:RadTextBox ID="IdReferenteInternoTextBox" runat="server" Skin="Office2007"
                                                            Style="display: none" Width="10px" />
                                                    </td>
                                                    <td style="width: 30px; text-align: center">
                                                        <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                            ToolTip="Seleziona utente (ALT + U) ..." ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td style="width: 30px; text-align: center">
                                                        <asp:ImageButton ID="EliminaUtentiSelezionatiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                            Style="width: 16px" ToolTip="Cancella utenti selezionati" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="scrollPanel" runat="server" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9;
                                                background-color: White">
                                                <telerik:RadGrid ID="UtentiGridView" runat="server" ToolTip="Elenco utenti associati al casella PEC"
                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                    Culture="it-IT" AllowMultiRowSelection="True">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="false" />
                                                            <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                AllowFiltering="False" ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                        runat="server"></asp:CheckBox>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                        runat="server"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                HeaderText="Utente" SortExpression="Descrizione" UniqueName="Descrizione">
                                                                <HeaderStyle Width="890px" />
                                                                <ItemStyle Width="890px" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\Delete16.png" UniqueName="Delete">
                                                            </telerik:GridButtonColumn>
                                                        </Columns>
                                                    </MasterTableView></telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td style="height: 20px">
                                            &nbsp;
                                            <asp:Label ID="TitoloElencoCaselleEmailLabel" runat="server" Font-Bold="True" CssClass="Etichetta"
                                                Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco caselle e-mail" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: White">
                                            <div style="overflow: auto; height: 190px; border: 1px solid #5D8CC9">
                                                <telerik:RadGrid ID="CaselleEmailGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="True" PageSize="5"
                                                    Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn SortExpression="Email" UniqueName="Email" HeaderText="Nome"
                                                                DataField="Email" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Email")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                        width: 300px; border: 0px solid red">
                                                                        <%# Eval("Email")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="Pop3Server" UniqueName="Pop3Server" HeaderText="Server in entrata POP3"
                                                                DataField="Pop3Server" HeaderStyle-Width="155px" ItemStyle-Width="155px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Pop3Server")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 155px; border: 0px solid red">
                                                                        <%# Eval("Pop3Server")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="Pop3Porta" UniqueName="Pop3Porta" HeaderText="Porta POP3"
                                                                DataField="Pop3Porta" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Pop3Porta")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                        <%# Eval("Pop3Porta")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="SmtpServer" UniqueName="SmtpServer" HeaderText="Server in uscita SMTP"
                                                                DataField="SmtpServer" HeaderStyle-Width="155px" ItemStyle-Width="155px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("SmtpServer")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 155px; border: 0px solid red">
                                                                        <%# Eval("SmtpServer")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="SmtpPorta" UniqueName="SmtpPorta" HeaderText="Porta SMTP"
                                                                DataField="SmtpPorta" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("SmtpPorta")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                        <%# Eval("SmtpPorta")%></div>
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
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
            <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
