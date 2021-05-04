<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="ClientePage.aspx.vb" Inherits="ClientePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var overlay = document.createElement("div");
        var overlayAOO = document.createElement("div");
        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');

        var hide = true;
        var hideAOO = true;

        var count = 1;

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
            $get("pageContent").appendChild(_backgroundElement);
            $get("pageContent").appendChild(overlay);
            $get("pageContent").appendChild(overlayAOO);
            if (hide) {
                HidePanel();

            } else {
                ShowPanel();
            }

            if (hideAOO) {
                HidePanelAOO();

            } else {
                ShowPanelAOO();
            }


        }

        function OnBeginRequest(sender, args) {
            // EnableUI(false);
        }

        function OnEndRequest(sender, args) {
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
                // _backgroundElement.className = "modalBackground";
                _backgroundElement.style.backgroundColor = '#09718F';
                _backgroundElement.style.filter = "alpha(opacity=20)";
                _backgroundElement.style.opacity = "0.2";
            }
            else {
                _backgroundElement.style.display = 'none';

            }
        }

        function HidePanel() {
            var panel = document.getElementById("printPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';

            _backgroundElement.style.display = 'none';
        }


        function ShowPanel() {

            with (overlay) {
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

            var panel = document.getElementById("printPanel");

            with (panel) {
                style.display = '';

                //style.position = 'absolute';
                //style.zIndex = 2000000;
                //style.textAlign = 'center';
                      

                style.width = '410px';
                //style.height = '400px';

                style.left = '50%';
                style.top = '35%';
                style.marginLeft = '-205px';
                style.marginTop = '-200px';
            }


            var shadow = document.getElementById("containerPanel");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }
        }



        function HidePanelAOO() {
            var panel = document.getElementById("mainPanelAOO");
            panel.style.display = "none";

            overlayAOO.style.display = 'none';
            _backgroundElement.style.display = 'none';
        }




        function ShowPanelAOO() {

            with (overlayAOO) {
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

            var panel = document.getElementById("mainPanelAOO");

         

            with (panel) {
                style.display = '';
               style.width = '410px';
               // style.height = '400px';

             style.left = '50%';
              style.top = '35%';
                style.marginLeft = '-205px';
                style.marginTop = '-200px';
            }


            var shadow = document.getElementById("containerPanelAOO");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }
        }



        function ShowMessageBox(message) {

            var panel = document.getElementById("pageContent");

            panel.appendChild(messageBox);
            panel.appendChild(messageBoxPanel);

            //           this.document.body.appendChild(messageBox);
            //           this.document.body.appendChild(messageBoxPanel);

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
                style.left = '50%';
                style.top = '25%';
                style.marginLeft = '-150px';
                style.marginTop = '-20px';
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
            //           xc = Math.round((document.body.clientWidth / 2) - (300 / 2));
            //           yc = Math.round((document.body.clientHeight / 2) - (40 / 2));
            //           messageBox.style.left = xc + "px";
            //           messageBox.style.top = yc + "px";
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
           <%-- <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 200px; z-index: 2000000">
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
            </div>--%>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <center>

                <div id="pageContent">

                    <table width="650px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text="Cliente" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                style="background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        <telerik:RadTabStrip ID="PubblicazioniTabStrip" runat="server" MultiPageID="PubblicazioniMultiPage"
                                                            SelectedIndex="0" Skin="Office2007" Width="100%">
                                                            <Tabs>
                                                                <telerik:RadTab Selected="True" Text="Generale" Style="text-align: center" />
                                                                <telerik:RadTab Text="Convenzioni Bancarie" Style="text-align: center" />
                                                                <telerik:RadTab Text="Aree Organizzative Omogenee" Style="text-align: center" />
                                                            </Tabs>
                                                        </telerik:RadTabStrip>
                                                        <telerik:RadMultiPage ID="PubblicazioniMultiPage" runat="server" BorderColor="#3399FF"
                                                            CssClass="multiPage" Height="100%" SelectedIndex="0">
                                                            <telerik:RadPageView ID="GeneralePageView" runat="server" CssClass="corporatePageView"
                                                                Height="230px">
                                                                <div id="GeneralePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="lbldisId" runat="server" CssClass="Etichetta" Text="Tipo cliente" />
                                                                            </td>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="TipoClienteComboBox" runat="server" Filter="StartsWith"
                                                                                    MaxHeight="50px" Skin="Office2007" Width="300px" TabIndex="1">
                                                                                    <Items>
                                                                                        <telerik:RadComboBoxItem runat="server" Text="Ente locale generico (COMUNE/PROVINCIA)"
                                                                                            Value="0" />
                                                                                        <telerik:RadComboBoxItem runat="server" Text="Altro (P.A.L. Generica)" Value="1" />
                                                                                    </Items>
                                                                                </telerik:RadComboBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="lbldisCodice" runat="server" CssClass="Etichetta" Text="Provincia" />
                                                                            </td>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="ProvinciaComboBox" runat="server" AutoPostBack="True" EmptyMessage="- Selezionare -"
                                                                                    Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                                    Width="300px" TabIndex="2" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="lbldisDescrizione" runat="server" CssClass="Etichetta" Text="Comune" />
                                                                            </td>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="ComuneComboBox" runat="server" EmptyMessage="- Selezionare -"
                                                                                    Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                                    AutoPostBack="True" Width="300px" TabIndex="3" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="Label8" runat="server" CssClass="Etichetta" Text="Partita IVA" />
                                                                            </td>
                                                                            <td>

                                                                            <table>
                                                                            <tr>

                                                                            <td>
                                                                             <telerik:RadTextBox ID="PIVATextBox" runat="server" Skin="Office2007" Width="100px"
                                                                                    MaxLength="11" TabIndex="4" />
                                                                            </td>

                                                                             <td style="width: 120px; text-align: center">
                                                                                <asp:Label ID="CodiceFiscaleLabel" runat="server" CssClass="Etichetta" Text="Codice fiscale"
                                                                                    Width="110" />
                                                                            </td>

                                                                            <td>
                                                                             <telerik:RadTextBox ID="CodiceFiscaleTextBox" runat="server" Skin="Office2007" Width="130px"
                                                                                    MaxLength="16" TabIndex="5" />
                                                                            </td>
                                                                            </tr>

                                                                            </table>
                                                                               
                                                                            </td>

                                                                           
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="lbldisIdImpianto" runat="server" CssClass="Etichetta" Text="Descrizione" />
                                                                            </td>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="300px"
                                                                                    TabIndex="6" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Indirizzo" />
                                                                            </td>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="IndirizzoTextBox" runat="server" Skin="Office2007" Width="300px"
                                                                                    MaxLength="100" TabIndex="7" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="Label10" runat="server" CssClass="Etichetta" Text="Telefono" />
                                                                            </td>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="TelefonoTextBox" runat="server" Skin="Office2007" Width="150px"
                                                                                    MaxLength="20" TabIndex="8" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 90px">
                                                                                <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="C.A.P." />
                                                                            </td>
                                                                            <td>
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="CapTextBox" runat="server" ReadOnly="true" Skin="Office2007"
                                                                                                Width="50px" />
                                                                                        </td>
                                                                                        <td style="width: 120px; text-align: center">
                                                                                            <asp:Label ID="Label3" runat="server" CssClass="Etichetta" Text="Codice cliente"
                                                                                                Width="110" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="CodiceClienteTextBox" runat="server" Skin="Office2007" Width="50px"
                                                                                                MaxLength="4" TabIndex="9" />
                                                                                        </td>
                                                                                        <td style="width: 120px; text-align: center">
                                                                                            <asp:Label ID="Label4" runat="server" CssClass="Etichetta" Text="Codice licenza"
                                                                                                Width="110" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="CodiceLicenzaTextBox" runat="server" Skin="Office2007" Width="80px"
                                                                                                MaxLength="8" TabIndex="10" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                           
                                                                            <td style="width: 120px">
                                                                                <asp:Label ID="CodiceAmministrazioneLabel" runat="server" CssClass="Etichetta" Text="Codice Ammin." />
                                                                            </td>
                                                                            <td>
                                                                                 <table style="width: 100%; padding:0px; border-spacing: 0;border-collapse: collapse;" >
                                                                                    <tr>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="CodiceAmministrazioneTextBox" runat="server" Skin="Office2007"
                                                                                                Width="150px" MaxLength="16" ToolTip="Codice assegnato automaticamente all'Amministrazione dall'IPA" TabIndex="11" />
                                                                                        </td>
                                                                                        <td style="width:90px;text-align:center"> <asp:Label ID="IndirizzoIpLabel" runat="server" CssClass="Etichetta" Text="Indirizzo IP" /></td>
                                                                                        <td>
                                                                                             <telerik:RadTextBox ID="IndirizzoIpTextBox" runat="server" Skin="Office2007"
                                                                                                Width="190px" MaxLength="20" ToolTip="Indirizzo IP del Server" TabIndex="12" />
                                                                                        </td>
                                                                                    </tr>

                                                                                </table>

                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </telerik:RadPageView>
                                                            <telerik:RadPageView ID="ConvenzioniPageView" runat="server" CssClass="corporatePageView"
                                                                Height="230px">
                                                                <div id="ConvenzioniPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                        <tr>
                                                                            <td>
                                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="ConvenzioniBancarieLabel" runat="server" Font-Bold="True" Style="width: 300px;
                                                                                                color: #00156E; background-color: #BFDBFF" Text="Convenzioni Bancarie" />
                                                                                        </td>
                                                                                        <td style="text-align: right">
                                                                                            <asp:ImageButton ID="AggiungiConvenzioneBancariaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                ToolTip="Aggiungi Convenzione Bancaria" ImageAlign="AbsMiddle" BorderStyle="None"
                                                                                                TabIndex="12" /><asp:ImageButton ID="AggiornaConvenzioneBancariaImageButton" runat="server"
                                                                                                    Style="display: none" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div style="overflow: auto; height: 185px; width: 100%; background-color: #FFFFFF;
                                                                                    border: 0px solid #5D8CC9;">
                                                                                    <telerik:RadGrid ID="ConvenzioniGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        CellSpacing="0" Culture="it-IT" GridLines="None" PageSize="5" Skin="Office2007"
                                                                                        TabIndex="13">
                                                                                        <MasterTableView DataKeyNames="id">
                                                                                            <Columns>
                                                                                                <telerik:GridBoundColumn DataField="id" DataType="System.Int32" FilterControlAltText="Filter column"
                                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="id" UniqueName="id" Visible="false" />
                                                                                                <telerik:GridTemplateColumn DataField="Denominazione" HeaderStyle-Width="130px" HeaderText="Denominazione"
                                                                                                    ItemStyle-Width="130px" SortExpression="Denominazione" UniqueName="Denominazione">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("Denominazione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                            text-overflow: ellipsis; width: 130px;">
                                                                                                            <%# Eval("Denominazione")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="ABI" HeaderStyle-Width="40px" HeaderText="ABI"
                                                                                                    ItemStyle-Width="40px" SortExpression="ABI" UniqueName="ABI">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("Abi")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                            width: 40px;">
                                                                                                            <%# Eval("Abi")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="CAB" HeaderStyle-Width="40px" HeaderText="CAB"
                                                                                                    ItemStyle-Width="40px" SortExpression="CAB" UniqueName="CAB">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("Cab")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                            width: 40px;">
                                                                                                            <%# Eval("Cab")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="CIN" HeaderStyle-Width="30px" HeaderText="CIN"
                                                                                                    ItemStyle-Width="30px" SortExpression="CIN" UniqueName="CIN">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("Cin")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                            width: 30px;">
                                                                                                            <%# Eval("Cin")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="Iban" HeaderStyle-Width="130px" HeaderText="IBAN"
                                                                                                    ItemStyle-Width="130px" SortExpression="Iban" UniqueName="Iban">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("Iban")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                            width: 130px;">
                                                                                                            <%# Eval("Iban")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="numeroContoCorrente" HeaderStyle-Width="60px"
                                                                                                    HeaderText="N. Conto" ItemStyle-Width="60px" SortExpression="numeroContoCorrente"
                                                                                                    UniqueName="numeroContoCorrente">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("numeroContoCorrente")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                            text-overflow: ellipsis; width: 60px;">
                                                                                                            <%# Eval("numeroContoCorrente")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                                                    Text="Modifica Convenzione Bancaria..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                                                    ImageUrl="~\images\edit16.png" UniqueName="Select" />
                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" HeaderStyle-Width="20px"
                                                                                                    ItemStyle-Width="20px" FilterControlAltText="Filter Delete column" ImageUrl="~\images\Delete16.png"
                                                                                                    UniqueName="Delete" Text="Elimina Convenzione Bancaria" />
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </telerik:RadPageView>
                                                            <telerik:RadPageView ID="AreeOrganizzativeOmogeneeView" runat="server" CssClass="corporatePageView"
                                                                Height="230px">
                                                                <div id="AreeOrganizzativeOmogeneePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                        <tr>
                                                                            <td>
                                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="TitoloAreeOrganizzativeOmogenee" runat="server" Font-Bold="True" Style="width: 300px;
                                                                                                color: #00156E; background-color: #BFDBFF" Text="Aree Organizzative Omogenee" />
                                                                                        </td>
                                                                                        <td style="text-align: right">
                                                                                            <asp:ImageButton ID="AggiungiAreaOrganizzativaOmogeneaImageButton" runat="server"
                                                                                                ImageUrl="~/images//knob-search16.png" ToolTip="Aggiungi AOO..." ImageAlign="AbsMiddle"
                                                                                                BorderStyle="None" /><asp:ImageButton ID="AggiornaAreaOrganizzativaOmogeneaImageButton"
                                                                                                    runat="server" Style="display: none" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div style="overflow: auto; height: 185px; width: 100%; background-color: #FFFFFF;
                                                                                    border: 0px solid #5D8CC9;">
                                                                                    <telerik:RadGrid ID="AreeOrganizzativeOmogeneeGridView" runat="server" AllowPaging="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" Culture="it-IT" GridLines="None"
                                                                                        PageSize="5" Skin="Office2007">
                                                                                        <MasterTableView DataKeyNames="Id,IdCliente">
                                                                                            <Columns>
                                                                                                <telerik:GridBoundColumn DataField="id" DataType="System.Int32" FilterControlAltText="Filter column"
                                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="id" Visible="false" />
                                                                                                <telerik:GridTemplateColumn DataField="CodiceAOO" HeaderText="Codice" SortExpression="CodiceAOO"
                                                                                                    UniqueName="CodiceAOO" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("CodiceAOO")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                            text-overflow: ellipsis; width: 100px; border: 0px solid red">
                                                                                                            <%# Eval("CodiceAOO")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="DenominazioneAOO" HeaderText="Denominazione"
                                                                                                    SortExpression="DenominazioneAOO" UniqueName="DenominazioneAOO" ItemStyle-Width="200px"
                                                                                                    HeaderStyle-Width="200px">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("DenominazioneAOO")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                            text-overflow: ellipsis; width: 200px; border: 0px solid red">
                                                                                                            <%# Eval("DenominazioneAOO")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="IndirizzoTelematico" HeaderText="Email" SortExpression="IndirizzoTelematico"
                                                                                                    UniqueName="IndirizzoTelematico" ItemStyle-Width="190px" HeaderStyle-Width="190px">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("IndirizzoTelematico")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                            text-overflow: ellipsis; width: 190px; border: 0px solid red">
                                                                                                            <%# Eval("IndirizzoTelematico")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                                                    Text="Modifica AOO..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                                                    ImageUrl="~\images\edit16.png" UniqueName="Select" />
                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" HeaderStyle-Width="20px"
                                                                                                    ItemStyle-Width="20px" FilterControlAltText="Filter Delete column" ImageUrl="~\images\Delete16.png"
                                                                                                    UniqueName="Delete" Text="Elimina AOO" />
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </telerik:RadPageView>
                                                        </telerik:RadMultiPage>
                                                    </td>
                                                </tr>
                                    </tr>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 0px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="SalvaButton" runat="server" Skin="Office2007" Text="Salva"
                                                ToolTip="Salva dati cliente" Width="80px" TabIndex="14">
                                                <Icon PrimaryIconLeft="5px" PrimaryIconUrl="../../../../images/Save16.png" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                 

                  

                </div>

                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
            </center>


               <div id="printPanel" style="position: absolute; width: 100%; text-align: center; z-index: 2000000; display: none">
                        <div id="containerPanel" style="width: 430px; text-align: center; background-color: #BFDBFF;
                            margin: 0 auto">
                            <table width="430px" cellpadding="5" cellspacing="5" border="0">
                                <tr>
                                    <td>
                                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <%--  HEADER--%>
                                            <tr>
                                                <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="TitoloRicercaLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                    Text="Nuova Convenzione Bancaria" CssClass="Etichetta" />
                                                            </td>
                                                            <td align="right">
                                                                <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HidePanel();hide=true;document.getElementById('<%=AggiornaConvenzioneBancariaImageButton.ClientID%>').click();" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <%-- BODY--%>
                                            <tr>
                                                <td class="ContainerMargin">
                                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                        style="background-color: #DFE8F6">
                                                        <tr>
                                                            <td style="width: 120px; text-align: right">
                                                                <asp:Label ID="SezioneLabel" runat="server" CssClass="Etichetta" Text="Denominazione *" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="DenominazioneConvenzioneTextBox" runat="server" Skin="Office2007"
                                                                    Width="270px" TabIndex="15" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px; text-align: right">
                                                                <asp:Label ID="ABI" runat="server" CssClass="Etichetta" Text="ABI *" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="AbiTextBox" runat="server" Enabled="true" Skin="Office2007"
                                                                    Width="50px" TabIndex="16" MaxLength="5" ToolTip="ABI" />
                                                            

                                                                &nbsp;<asp:Label ID="Label5" runat="server" CssClass="Etichetta" Text="CAB *" />
                                                              
                                                                &nbsp;<telerik:RadTextBox ID="CabTextBox" runat="server" Enabled="true" Skin="Office2007"
                                                                    Width="50px" TabIndex="17" MaxLength="5"  ToolTip="CAB" />
                                                                 
                                                              

                                                                &nbsp;<asp:Label ID="Label7" runat="server" CssClass="Etichetta" Text="CIN *" />

                                                                &nbsp;<telerik:RadTextBox ID="CinTextBox" runat="server" Enabled="true" MaxLength="1"
                                                                    Skin="Office2007" Width="20px" TabIndex="18" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px; text-align: right">
                                                                <asp:Label ID="Label9" runat="server" CssClass="Etichetta" Text="IBAN *" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="IbanTextBox" runat="server" Enabled="true" Skin="Office2007"
                                                                    Width="180px" TabIndex="19" MaxLength="27" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px; text-align: right">
                                                                <asp:Label ID="Label6" runat="server" CssClass="Etichetta" Text="N. Conto *" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="NumeroContoCorrenteTextBox" runat="server" Enabled="true"
                                                                    Skin="Office2007" Width="120px" TabIndex="20" MaxLength="12" ToolTip="Numero Conto" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <%-- FOOTER--%>
                                            <tr>
                                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                                    <telerik:RadButton ID="SalvaConvenzioneButton" runat="server" Text="Ok" Width="90px"
                                                        TabIndex="21" Skin="Office2007" ToolTip="Salva i dati della Convenzione">
                                                        <Icon PrimaryIconUrl="../../../../images/save16.png" PrimaryIconLeft="5px" />
                                                    </telerik:RadButton>
                                                    <telerik:RadButton ID="AnnullaConvenzioneButton" runat="server" Text="Annulla" Width="90px"
                                                        TabIndex="22" Skin="Office2007" ToolTip="Annulla Modifiche della Convenzione">
                                                        <Icon PrimaryIconUrl="../../../../images/Annulla.png" PrimaryIconLeft="5px" />
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
            


           <div id="mainPanelAOO" style="position: absolute; width: 100%; text-align: center; z-index: 2000000; display: none">

                        <div id="containerPanelAOO" style="width: 430px; text-align: center; background-color: #BFDBFF;  margin: 0 auto">
                            <table width="430px" cellpadding="5" cellspacing="5" border="0">
                                <tr>
                                    <td>
                                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <%--  HEADER--%>
                                            <tr>
                                                <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="TitoloRicercaAreaOrganizzativaOmogeneaLabel" runat="server" Style="color: #00156E"
                                                                    Font-Bold="True" Text="Nuova Area Organizzativa Omogenea" CssClass="Etichetta" />
                                                            </td>
                                                            <td align="right">


                                                                <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HidePanelAOO();hideAOO=true;document.getElementById('<%=AggiornaAreaOrganizzativaOmogeneaImageButton.ClientID%>').click();" />
                                                            
                                                            
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <%-- BODY--%>
                                            <tr>
                                                <td class="ContainerMargin">
                                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                        style="background-color: #DFE8F6">
                                                        <tr>
                                                            <td style="width: 120px">
                                                                <asp:Label ID="CodiceAreaOrganizzativaOmogeneaLabel" runat="server" CssClass="Etichetta"
                                                                    Text="Codice *" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="CodiceAreaOrganizzativaOmogeneaTextBox" runat="server" Skin="Office2007"
                                                                    Width="200px" MaxLength="16" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px">
                                                                <asp:Label ID="DenominazioneAreaOrganizzativaOmogeneaLabel" runat="server" CssClass="Etichetta"
                                                                    Text="Denominazione *" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="DenominazioneAreaOrganizzativaOmogeneaTextBox" runat="server"
                                                                    Enabled="true" Skin="Office2007" Width="275px" MaxLength="30" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 120px">
                                                                <asp:Label ID="IndirizzoTelematicoLabel" runat="server" CssClass="Etichetta" Text="Email *" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="IndirizzoTelematicoTextBox" runat="server" Enabled="true"
                                                                    Skin="Office2007" Width="275px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <%-- FOOTER--%>
                                            <tr>
                                                <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                &nbsp;
                                                            </td>
                                                            <td style="text-align: center">
                                                          
                                                          
                                                                <telerik:RadButton ID="SalvaAreaOrganizzativaOmogeneaButton" runat="server" Text="Ok"
                                                                    Width="90px" Skin="Office2007" ToolTip="Salva i dati della AOO">
                                                                    <Icon PrimaryIconUrl="../../../../images/save16.png" PrimaryIconLeft="5px" />
                                                                </telerik:RadButton>
                                                         
                                                            </td>

                                                            <td style="text-align: center">
                                                                <telerik:RadButton ID="AnnullaAreaOrganizzativaOmogeneaButton" runat="server" Text="Annulla"
                                                                    Width="90px" Skin="Office2007" ToolTip="Annulla modifiche della AOO">
                                                                    <Icon PrimaryIconUrl="../../../../images/Annulla.png" PrimaryIconLeft="5px" />
                                                                </telerik:RadButton>
                                                            </td>
                                                            <td style="width: 90px">
                                                              
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
