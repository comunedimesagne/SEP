<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="RegistrazioneEmailArrivoPage.aspx.vb" Inherits="RegistrazioneEmailArrivoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UI/Protocollo/pages/user/VisualizzaEmailUserControl.ascx" TagName="VisualizzaEmailControl"
    TagPrefix="parsec" %>
<%@ Register Src="~/UI/Protocollo/pages/user/VisualizzaFatturaUserControl.ascx" TagName="VisualizzaFatturaControl"
    TagPrefix="parsec" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");

        var panelIndex = -1;   //0 = Email ; 1 = Fattura
        var hideEmailPanel = true;
        var hideFatturaPanel = true;

        var hideMessage = true;

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
            $get("pageContent").appendChild(_backgroundElement);
            $get("pageContent").appendChild(overlay);

            if (panelIndex != -1) {

                //VISUALIZZO E NASCONDO IL  POPUP (FATTURA)

                if (panelIndex == 1) {
                    if (hideFatturaPanel) {
                        HideFatturaElettronicaPanel();
                    } else {
                        ShowFatturaElettronicaPanel();
                    }

                }

                //VISUALIZZO E NASCONDO IL  POPUP (EMAIL)
                if (panelIndex == 0) {

                    if (hideEmailPanel) {
                        HideEmailPanel();
                    } else {
                        ShowEmailPanel();
                    }

                }
            }
        }


        function OnBeginRequest(sender, args) {
            EnableUI(false);
        }

        function OnEndRequest(sender, args) {

            var message = $get('<%= infoOperazioneHidden.ClientId %>').value;

            if (message !== '') {

                //VISUALIZZO IL MESSAGGIO

                ShowMessagePanel(message);

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

        function updating(sender, args) {
            var d = $get("progressBar");
            d.style.display = '';
            if (args.get_progressData() && args.get_progressData().OperationComplete == 'true') {
                args.set_cancel(true);
                d.style.display = 'none';
            }
        }


        function HideMessagePanel() {
            var panel = document.getElementById("mainPanelMessage");
            panel.style.display = "none";
            overlay.style.display = 'none';
            _backgroundElement.style.display = 'none';
            EnableUI(true);
        }


        function ShowMessagePanel() {
            overlay.style.display = '';
            var panel = document.getElementById("mainPanelMessage");
            panel.style.display = '';
            panel.style.position = 'absolute';
            overlay.style.position = 'absolute';
            overlay.style.left = '0px';
            overlay.style.top = '0px';
            overlay.style.width = '100%';
            overlay.style.height = '100%';
            overlay.style.zIndex = 10000;
            overlay.style.backgroundColor = '#09718F';
            overlay.style.filter = "alpha(opacity=20)";
            overlay.style.opacity = "0.2";

            var shadow = document.getElementById("containerPanelMessage");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }

        }


        function HideEmailPanel() {

            var panel = document.getElementById("EmailPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
            panelIndex = -1;
        }

        function ShowEmailPanel() {

            panelIndex = 0;
            var panel = document.getElementById("EmailPanel");

            panel.style.display = '';
            panel.style.position = 'absolute';

            overlay.style.display = '';
            overlay.style.position = 'absolute';
            overlay.style.left = '0px';
            overlay.style.top = '0px';
            overlay.style.width = '100%';
            overlay.style.height = '100%';
            overlay.style.zIndex = 10000;
            overlay.style.backgroundColor = '#09718F';
            overlay.style.filter = "alpha(opacity=20)";
            overlay.style.opacity = "0.2";


            var shadow = document.getElementById("ShadowEmailPanel");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }

        }



        function HideFatturaElettronicaPanel() {

            var panel = document.getElementById("FatturaPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
            panelIndex = -1;
        }



        function ShowFatturaElettronicaPanel() {

            panelIndex = 1;
            var panel = document.getElementById("FatturaPanel");

            panel.style.display = '';
            panel.style.position = 'absolute';

            overlay.style.display = '';
            overlay.style.position = 'absolute';
            overlay.style.left = '0px';
            overlay.style.top = '0px';
            overlay.style.width = '100%';
            overlay.style.height = '100%';
            overlay.style.zIndex = 10000;
            overlay.style.backgroundColor = '#09718F';
            overlay.style.filter = "alpha(opacity=20)";
            overlay.style.opacity = "0.2";


            var shadow = document.getElementById("ShadowFatturaPanel");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }

        }



    </script>
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
            <div id="progressBar" style="position: absolute; width: 100%; text-align: center;
                top: 300px; z-index: 2000000">
                <div id="Div3" style="width: 310px; text-align: center; background-color: #BFDBFF;
                    margin: 0 auto">
                    <telerik:RadProgressArea OnClientProgressUpdating="updating" Skin="Office2007" ID="RadProgressArea1"
                        runat="server" ProgressIndicators="RequestSize,TotalProgressPercent,TotalProgressBar"
                        HeaderText="Download e-mail in corso...">
                        <Localization Total="Totale:" Uploaded="Completato:" />
                    </telerik:RadProgressArea>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent" style="height: 100%; width: 100%; position: relative">
                <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>
                <center>
                    <telerik:RadProgressManager ID="Radprogressmanager1" runat="server" Skin="Office2007" />
                    <table style="width: 900px; height: 670px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td valign="top">
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                    height: 100%">
                                    <tr style="height: 20px">
                                        <td valign="top">
                                            <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="FiltroEmailLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Filtro E-mail Ricevute" />
                                                    </td>
                                                    <td align="center" style="width: 30">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Effettua la ricerca con i filtri impostati" Style="border-style: none;
                                                            border-color: inherit; border-width: 0; width: 16px;" ImageAlign="AbsMiddle"
                                                            TabIndex="9" />
                                                    </td>
                                                    <td align="center" style="width: 30">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" TabIndex="10" />
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
                                                    <td style="width: 70px">
                                                        <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="Mittente" />
                                                    </td>
                                                    <td style="width: 830px">
                                                        <telerik:RadComboBox ID="MittentiComboBox" runat="server" Width="100%" Height="150"
                                                            EmptyMessage="- Seleziona mittente -" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="Contains" Skin="Office2007"
                                                            LoadingMessage="Caricamento in corso..." TabIndex="3">
                                                            <WebServiceSettings Method="GetMittenti" Path="RegistrazioneEmailArrivoPage.aspx" />
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 70px">
                                                        <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                    </td>
                                                    <td style="width: 830px">
                                                        <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                            TabIndex="4" ToolTip="Oggetto dell'e-mail" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 70px">
                                                        <asp:Label ID="DataInvioLabel" runat="server" CssClass="Etichetta" Text="Data" />
                                                    </td>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 40px">
                                                                    <asp:Label ID="DataInvioInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                </td>
                                                                <td style="width: 100px">
                                                                    <telerik:RadDatePicker ID="DataInvioInizioTextBox" Skin="Office2007" Width="110px"
                                                                        runat="server" MinDate="1753-01-01" ToolTip="Data di arrivo e-mail da">
                                                                        <Calendar>
                                                                            <SpecialDays>
                                                                                <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                            </SpecialDays>
                                                                        </Calendar>
                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td style="width: 40px; text-align: center">
                                                                    <asp:Label ID="DataInvioFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                </td>
                                                                <td style="width: 110px">
                                                                    <telerik:RadDatePicker ID="DataInvioFineTextBox" Skin="Office2007" Width="110px"
                                                                        runat="server" MinDate="1753-01-01" ToolTip="Data di arrivo e-mail a">
                                                                        <Calendar>
                                                                            <SpecialDays>
                                                                                <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                            </SpecialDays>
                                                                        </Calendar>
                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td style="width: 60px; text-align: center">
                                                                    <asp:Label ID="StatoLetturaLabel" runat="server" CssClass="Etichetta" Text="Letta"
                                                                        Width="60px" />
                                                                </td>
                                                                <td style="width: 100px;">
                                                                    <telerik:RadComboBox ID="StatoLetturaComboBox" runat="server" Skin="Office2007" Width="100%"
                                                                        EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px"
                                                                        ToolTip="Seleziona lo stato dell'e-mail" />
                                                                </td>
                                                                <td style="width: 60px; text-align: center">
                                                                    <asp:Label ID="StatoLabel" runat="server" CssClass="Etichetta" Text="Stato" Width="60px" />
                                                                </td>
                                                                <td>
                                                                    <telerik:RadComboBox ID="StatoComboBox" runat="server" Skin="Office2007" Width="100%"
                                                                        EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px"
                                                                        TabIndex="7" ToolTip="Seleziona lo stato dell'e-mail rispetto alla protocollazione" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 70px">
                                                        <asp:Label ID="TipologiaLabel" runat="server" CssClass="Etichetta" Text="Tipologia" />
                                                    </td>
                                                    <td style="width: 100%">
                                                        <div id="ZoneID1">
                                                            <asp:CheckBoxList ID="TipologieCheckBoxList" runat="server" ToolTip="Elenco tipologie e-mail"
                                                                RepeatDirection="Horizontal" CssClass="CkeckListCss" TabIndex="8" RepeatColumns="5" />
                                                        </div>
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
                                                        <asp:Label ID="ElencoEmailLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Elenco E-mail Ricevute" />
                                                    </td>
                                                    <td align="center" style="width: 30; display: none">
                                                        <asp:ImageButton ID="RecuperaEmailImageButton" Style="border: 0" runat="server" ImageUrl="~/images/undo16.png"
                                                            ToolTip="Recupera e-mail (cancellate) selezionate" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30; display: block">
                                                        <asp:ImageButton ID="CollegaEmailImageButton" Style="border-style: none; border-color: inherit;
                                                            border-width: 0; width: 20px;" runat="server" ImageUrl="~/images/email_link32.png"
                                                            ToolTip="Collegamento massivo dell'e-mail di notifica SdI al relativo protocollo"
                                                            ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30">
                                                        <asp:ImageButton ID="EliminaEmailImageButton" Style="border-style: none; border-color: inherit;
                                                            border-width: 0; width: 16px;" runat="server" ImageUrl="~/images/RecycleEmpty.png"
                                                            ToolTip="Cancella e-mail selezionate" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30">
                                                        <asp:ImageButton ID="EsportaXls" Style="border: 0" runat="server" ImageUrl="~/images/excel16.png"
                                                            ToolTip="Esporta l'elenco dell'e-mail in formato Excel" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30">
                                                        <asp:ImageButton ID="RiceviEmailImageButton" Style="border: 0" runat="server" ImageUrl="~/images/receiveEmail.png"
                                                            ToolTip="Ricevi e-mail" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 60px">
                                                        <asp:Label ID="CaselleEmailLabel" runat="server" CssClass="Etichetta" Text="Casella" />
                                                    </td>
                                                    <td align="left">
                                                        <telerik:RadComboBox ID="CaselleEmailComboBox" runat="server" Skin="Office2007" AutoPostBack="true"
                                                            Width="440px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                            MaxHeight="400px" TabIndex="1" ToolTip="Casella di posta elettronica" />
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
                                                        <div id="scrollPanel" runat="server" style="overflow: auto; height: 490px; border: 1 solid #5D8CC9">
                                                            <telerik:RadGrid ID="EmailGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                AllowMultiRowSelection="True" Culture="it-IT" PageSize="15" TabIndex="11">
                                                                <MasterTableView DataKeyNames="Id">
                                                                    <NoRecordsTemplate>
                                                                        Nessuna e-mail trovata</NoRecordsTemplate>
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutte l'e-mails"
                                                                            AllowFiltering="False">
                                                                            <HeaderTemplate>
                                                                                <div style="width: 16px; height: 16px" align="center">
                                                                                    <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                                        runat="server" />
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                                    runat="server" ToolTip="Seleziona l'e-mail" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" Width="16px" />
                                                                            <ItemStyle Width="16px" />
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn Visible="False" UniqueName="Id" DataField="Id" />
                                                                        <telerik:GridTemplateColumn DataField="Tipo" UniqueName="Tipo" ItemStyle-Width="20px"
                                                                            HeaderStyle-Width="20px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Image runat="server" ID="EmailImage" />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Mittente" UniqueName="Mittente" HeaderText="Mittente"
                                                                            DataField="Mittente" HeaderStyle-Width="230px" ItemStyle-Width="230px">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="MittenteLabel" />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                            DataField="Oggetto" HeaderStyle-Width="280px" ItemStyle-Width="280px">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="OggettoLabel" />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="DataArrivo" UniqueName="DataArrivo" HeaderText="Data"
                                                                            DataField="DataArrivo" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="DataArrivoLabel" />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Sblocca" UniqueName="Sblocca"
                                                                            HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-VerticalAlign="Middle"
                                                                            ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="PreviewFattura" ImageUrl="~\images\xml_16.png"
                                                                            UniqueName="PreviewFattura" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-VerticalAlign="Middle"
                                                                            ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" ImageUrl="~\images\knob-search16.png"
                                                                            UniqueName="Preview" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-VerticalAlign="Middle"
                                                                            ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Protocolla" ImageUrl="~\images\text.png"
                                                                            UniqueName="Protocolla" Text="Protocolla e-mail" HeaderStyle-Width="20px" ItemStyle-Width="20px"
                                                                            ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" />
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="Delete"
                                                                            HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-VerticalAlign="Middle"
                                                                            ItemStyle-HorizontalAlign="Center" />
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
                    <asp:ImageButton ID="AggiornaEmailSenzaInvioNotificaButton" runat="server" ImageUrl="~/images//knob-search16.png"
                        Style="display: none" />
                </center>
                <div id="mainPanelMessage" style="position: absolute; width: 100%; text-align: center;
                    z-index: 2000000; display: none; top: 100px">
                    <div id="containerPanelMessage" style="width: 430px; text-align: center; background-color: #BFDBFF;
                        margin: 0 auto">
                        <table width="700px" cellpadding="5" cellspacing="5" border="0">
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
                                                            <asp:Label ID="TitoloPanelMessage" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                Text="Informazioni" CssClass="Etichetta" />
                                                        </td>
                                                        <td align="right">
                                                            <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HideMessagePanel();hideMessage=true;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <%-- BODY--%>
                                        <tr>
                                            <td class="ContainerMargin">
                                                <asp:Panel ID="MessagePanel" runat="server" Height="200px" Width="100%" Style="overflow: auto;
                                                    background-color: White">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="EmailPanel" style="position: absolute; width: 100%; text-align: center;
                    z-index: 2000000; display: none; top: 0px">
                    <div id="ShadowEmailPanel" style="width: 800px; text-align: center; background-color: #BFDBFF;
                        margin: 0 auto">
                        <parsec:VisualizzaEmailControl runat="server" ID="VisualizzaEmailControl" />
                    </div>
                </div>
                <div id="FatturaPanel" style="position: absolute; width: 100%; text-align: center;
                    z-index: 2000000; display: none; top: 0px">
                    <div id="ShadowFatturaPanel" style="width: 800px; text-align: center; background-color: #BFDBFF;
                        margin: 0 auto">
                        <parsec:VisualizzaFatturaControl runat="server" ID="VisualizzaFatturaControl" />
                    </div>
                </div>
                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
