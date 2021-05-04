<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="AcquisizioneAutomaticaAllegatPage.aspx.vb" Inherits="AcquisizioneAutomaticaAllegatPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");

        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');

        var rowSelectedHidden;
        var count = 2;

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
            $get("pageContent").appendChild(_backgroundElement);

            rowSelectedHidden = $get('<%= rowSelectedHidden.ClientID %>');

        }


        function OnBeginRequest(sender, args) {
            EnableUI(false);
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
                _backgroundElement.style.backgroundColor = '#09718F';
                _backgroundElement.style.filter = "alpha(opacity=20)";
                _backgroundElement.style.opacity = "0.2";
            }
            else {
                _backgroundElement.style.display = 'none';

            }
        }


        function RowSelected(sender, eventArgs) {
            var grid = sender;
            var masterTable = grid.get_masterTableView();
            var numeroRigheSelezionate = masterTable.get_selectedItems().length;
            var numeroRighe = masterTable.get_dataItems().length;

            rowSelectedHidden.value = numeroRigheSelezionate;


        }

        function ConfirmDeleteScansioni(message) {
            var numeroRigheSelezionate = parseInt(rowSelectedHidden.value, 10);
            if (numeroRigheSelezionate > 0) {
                return confirm(message);
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
                style.width = '300px';
                style.height = '40px';
                style.backgroundColor = '#BFDBFF';
                style.border = 'solid #4892FF 2px';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.zIndex = 10000;
                innerHTML = message;
                style.color = '#00156E';
                style.backgroundImage = 'url(/sep/Images/success.png)';
                style.backgroundPosition = '10px center';
                style.backgroundRepeat = 'no-repeat';
                style.padding = '15px 10px 15px 50px';
                style.margin = '15px 0px';
            }


            xc = Math.round((document.body.clientWidth / 2) - (300 / 2)) + 120;
            yc = Math.round((document.body.clientHeight / 2) - (40 / 2)) - 300;


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
    <asp:UpdateProgress runat="server" ID="UpdateProgress1">
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px;">
                <table style="padding: 20px; background-color: #4892FF">
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
                <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>
                <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator>
                <telerik:RadFormDecorator ID="RadFormDecorator3" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID3" Skin="Web20"></telerik:RadFormDecorator>
                <center>
                    <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 2px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloLabel" runat="server" Font-Bold="True" Style="color: #00156E;
                                                            background-color: #BFDBFF" Text="Allega Documenti Scansionati" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%-- CONTENT--%>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <%-- INIZIO TABELLA OPZIONI--%>
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                            <%-- HEADER--%>
                                                            <tr>
                                                                <td valign="top">
                                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;<asp:Label ID="FiltroLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                    color: #00156E; background-color: #BFDBFF" Text="Filtro Allegati" />
                                                                            </td>
                                                                            <td align="center" style="width: 30px">
                                                                                <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                                                    ToolTip="Effettua la ricerca con i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" />
                                                                            </td>
                                                                            <td align="center" style="width: 30">
                                                                                <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                                                    ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--  CONTENT--%>
                                                            <tr style="background-color: #DFE8F6">
                                                                <td valign="top">
                                                                    <div style="overflow: auto; height: 100%; width: 100%; background-color: #DFE8F6;
                                                                        border: 0px solid #5D8CC9;">
                                                                        <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td style="width: 150px">
                                                                                                &nbsp;<asp:Label ID="DataAcquisizioneLabel" runat="server" CssClass="Etichetta" Text="Data acquisizione" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 30px">
                                                                                                            <asp:Label ID="DataAcquisizioneInizioLabel" runat="server" CssClass="Etichetta" Text="da"
                                                                                                                ToolTip="Data di acquisizione da" />
                                                                                                        </td>
                                                                                                        <td style="width: 150px">
                                                                                                            <telerik:RadDatePicker ID="DataAcquisizioneInizioTextBox" Skin="Office2007" Width="110px"
                                                                                                                runat="server" MinDate="1753-01-01" ToolTip="Data di acquisizione da">
                                                                                                                <Calendar runat="server">
                                                                                                                    <SpecialDays>
                                                                                                                        <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                                    </SpecialDays>
                                                                                                                </Calendar>
                                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                            </telerik:RadDatePicker>
                                                                                                        </td>
                                                                                                        <td style="width: 30px">
                                                                                                            <asp:Label ID="DataAcquisizioneFineLabel" runat="server" CssClass="Etichetta" Text="a"
                                                                                                                ToolTip="Data di acquisizione a" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadDatePicker ID="DataAcquisizioneFineTextBox" Skin="Office2007" Width="110px"
                                                                                                                runat="server" MinDate="1753-01-01" ToolTip="Data di acquisizione a">
                                                                                                                <Calendar runat="server">
                                                                                                                    <SpecialDays>
                                                                                                                        <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                                    </SpecialDays>
                                                                                                                </Calendar>
                                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                            </telerik:RadDatePicker>
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
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- FINE TABELLA OPZIONI--%>
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                            <%-- HEADER--%>
                                                            <tr>
                                                                <td valign="top">
                                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;<asp:Label ID="OpzioniLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                    color: #00156E; background-color: #BFDBFF" Text="Opzioni scansione" />
                                                                            </td>
                                                                            <td align="center" style="width: 30px">
                                                                                <asp:ImageButton ID="ScansionaImageButton" runat="server" ImageUrl="~/images//Scanner.png"
                                                                                    Style="width: 16px" ToolTip="Allega documento digitalizzato" ImageAlign="AbsMiddle" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--  CONTENT--%>
                                                            <tr style="background-color: #DFE8F6">
                                                                <td valign="top">
                                                                    <div style="overflow: auto; height: 100%; width: 100%; background-color: #DFE8F6;
                                                                        border: 0px solid #5D8CC9;">
                                                                        <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table style="width: 100%" id="ZoneID3">
                                                                                        <tr>
                                                                                            <td style="width: 80px">
                                                                                                &nbsp;<asp:Label ID="FronteRetroLabel" runat="server" CssClass="Etichetta" Text="Fronte retro"
                                                                                                    ToolTip="Attiva scansione fronte-retro (solo modelli scanner che lo supportano)" />
                                                                                            </td>
                                                                                            <td style="width: 50px">
                                                                                                <asp:CheckBox ID="FronteRetroCheckBox" runat="server" Text="&amp;nbsp;" ToolTip="Attiva scansione fronte-retro (solo modelli scanner che lo supportano)" />
                                                                                            </td>
                                                                                            <td style="width: 180px">
                                                                                                <asp:Label ID="ScansionaCodiceBarreLabel" runat="server" CssClass="Etichetta" Text="Riconoscimento codice barre"
                                                                                                    ToolTip="Attiva riconoscimento codice barre" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:CheckBox AutoPostBack="False" ID="ScansionaCodiceBarreRadioButton" runat="server"
                                                                                                    Text="&amp;nbsp;" ToolTip="Attiva riconoscimento codice barre" />
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
                                            </table>
                                            <div id="GrigliaDocumentiPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" width="100%" border="0" style="background-color: #BFDBFF">
                                                                <tr>
                                                                    <td>
                                                                        <telerik:RadTabStrip ID="ScansioniTabStrip" runat="server" MultiPageID="ScansioniMultiPage"
                                                                            SelectedIndex="0" Skin="Office2007" Width="100%">
                                                                            <Tabs>
                                                                                <telerik:RadTab Selected="True" Text="Scansioni associate" Style="text-align: center" />
                                                                                <telerik:RadTab Text="Scansioni non associate" Style="text-align: center" />
                                                                            </Tabs>
                                                                        </telerik:RadTabStrip>
                                                                        <telerik:RadMultiPage ID="ScansioniMultiPage" runat="server" BorderColor="#3399FF"
                                                                            CssClass="multiPage" Height="100%" SelectedIndex="0" Width="100%">
                                                                            <telerik:RadPageView ID="GeneralePageView" runat="server" CssClass="corporatePageView"
                                                                                Height="420px" Width="100%">
                                                                                <div id="GeneralePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9"
                                                                                        id="ZoneID1">
                                                                                        <tr>
                                                                                            <td style="height: 20px">
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="ScansioniLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                Style="width: 295px; color: #00156E; background-color: #BFDBFF" Text="Scansioni associate" />
                                                                                                        </td>
                                                                                                        <td style="width: 65px">
                                                                                                            &nbsp;<asp:Label ID="NumeroProtocolloLabel" runat="server" CssClass="Etichetta" Text="Numero" />
                                                                                                        </td>
                                                                                                        <td style="width: 90px">
                                                                                                            <telerik:RadNumericTextBox ID="NumeroProtocolloTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                                                ShowSpinButtons="True" ToolTip="Numero del protocollo a cui allegare uno o più documenti scansionati (Se non è attivo il riconoscimento dei codici a barre)">
                                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                            </telerik:RadNumericTextBox>
                                                                                                        </td>
                                                                                                        <td style="width: 50px">
                                                                                                            <asp:Label ID="AnnoProtocolloLabel" runat="server" CssClass="Etichetta" Text="Anno" />
                                                                                                        </td>
                                                                                                        <td style="width: 90px">
                                                                                                            <telerik:RadNumericTextBox ID="AnnoProtocolloTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="70px" DataType="System.Int32" MaxLength="4" MaxValue="9999" MinValue="1"
                                                                                                                ToolTip="Anno del protocollo a cui allegare uno o più documenti scansionati (Se non è attivo il riconoscimento dei codici a barre)"
                                                                                                                ShowSpinButtons="True">
                                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                            </telerik:RadNumericTextBox>
                                                                                                        </td>
                                                                                                        <td style="width: 80px">
                                                                                                            <asp:Label ID="RegistrazioneInternaLabel" runat="server" CssClass="Etichetta" Text="Reg. interna" />
                                                                                                        </td>
                                                                                                        <td style="width: 40px">
                                                                                                            <asp:CheckBox ID="RegistrazioneInternaCheckBox" runat="server" Text="&amp;nbsp;" />
                                                                                                        </td>
                                                                                                        <td align="center" style="width: 30px">
                                                                                                            <asp:ImageButton ID="SalvaAllegatiSelezionatiImageButton" runat="server" ImageUrl="~/images//Save16.png"
                                                                                                                Style="width: 16px" ToolTip="Salva allegati selezionati" ImageAlign="AbsMiddle" />
                                                                                                        </td>
                                                                                                        <td align="center" style="width: 30px">
                                                                                                            <asp:ImageButton ID="EliminaAllegatiSelezionatiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                                                Style="width: 16px" ToolTip="Cancella allegati selezionati" ImageAlign="AbsMiddle" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr style="background-color: #FFFFFF">
                                                                                            <td>
                                                                                                <div id="scrollPanel" runat="server" style="overflow: auto; height: 380px; border: 1px solid #5D8CC9">
                                                                                                    <telerik:RadGrid ID="AllegatiScansionatiGridView" runat="server" AllowPaging="False"
                                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                        Width="100%" AllowSorting="True" AllowMultiRowSelection="True" Culture="it-IT">
                                                                                                        <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                                            <Columns>
                                                                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                                                                    AllowFiltering="False" HeaderStyle-Width="35px" ItemStyle-Width="35px">
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
                                                                                                                <telerik:GridBoundColumn DataField="IdUtente" DataType="System.Int32" FilterControlAltText="Filter IdUtente column"
                                                                                                                    HeaderText="Id Utente" ReadOnly="True" SortExpression="IdUtente" UniqueName="IdUtente"
                                                                                                                    Visible="False" />
                                                                                                                <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                                                                                    HeaderText="N. prot." DataField="NumeroProtocollo" HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("NumeroProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                            text-overflow: ellipsis; width: 100%; border: 0px solid red;">
                                                                                                                            <%# Eval("NumeroProtocollo")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn SortExpression="AnnoProtocollo" UniqueName="AnnoProtocollo"
                                                                                                                    HeaderText="Anno" DataField="AnnoProtocollo" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("AnnoProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                            text-overflow: ellipsis; width: 100%; border: 0px solid red;">
                                                                                                                            <%# Eval("AnnoProtocollo")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn SortExpression="Utente" UniqueName="Utente" HeaderText="Utente"
                                                                                                                    DataField="Utente" HeaderStyle-Width="170px" ItemStyle-Width="170px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("Utente")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                                            width: 100%;">
                                                                                                                            <%# Eval("Utente")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                                    DataField="NomeFile">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                            text-overflow: ellipsis; width: 100%;">
                                                                                                                            <%# Eval("NomeFile")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn SortExpression="DataAcquisizione" UniqueName="DataAcquisizione"
                                                                                                                    HeaderText="Data acquis." DataField="DataAcquisizione" HeaderStyle-Width="100px"
                                                                                                                    ItemStyle-Width="100px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("DataAcquisizione","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                                            overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red;">
                                                                                                                            <%# Eval("DataAcquisizione", "{0:dd/MM/yyyy}")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                                                    ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="30px"
                                                                                                                    ItemStyle-Width="30px" />
                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Save" FilterControlAltText="Filter Save column"
                                                                                                                    ImageUrl="~\images\Save16.png" UniqueName="Save" HeaderStyle-Width="30px" ItemStyle-Width="30px" />
                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="30px"
                                                                                                                    ItemStyle-Width="30px" />
                                                                                                            </Columns>
                                                                                                        </MasterTableView>
                                                                                                    </telerik:RadGrid>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </telerik:RadPageView>
                                                                            <telerik:RadPageView ID="ErroriPageView" runat="server" CssClass="corporatePageView"
                                                                                Height="420px" Width="100%">
                                                                                <div id="ErroriPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9"
                                                                                        id="ZoneID2">
                                                                                        <tr>
                                                                                            <td style="height: 20px">
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="ScansioniErroreLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                Style="width: 295px; color: #00156E; background-color: #BFDBFF" Text="Scansioni non associate" />
                                                                                                        </td>
                                                                                                        <td style="width: 65px">
                                                                                                            &nbsp;<asp:Label ID="NumeroProtocolloErroreLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Numero" />
                                                                                                        </td>
                                                                                                        <td style="width: 90px">
                                                                                                            <telerik:RadNumericTextBox ID="NumeroProtocolloErroreTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                                                ShowSpinButtons="True" ToolTip="Numero del protocollo a cui allegare uno o più documenti scansionati">
                                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                            </telerik:RadNumericTextBox>
                                                                                                        </td>
                                                                                                        <td style="width: 50px">
                                                                                                            <asp:Label ID="AnnoProtocolloErroreLabel" runat="server" CssClass="Etichetta" Text="Anno" />
                                                                                                        </td>
                                                                                                        <td style="width: 90px">
                                                                                                            <telerik:RadNumericTextBox ID="AnnoProtocolloErroreTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="70px" DataType="System.Int32" MaxLength="4" MaxValue="9999" MinValue="1"
                                                                                                                ToolTip="Anno del protocollo a cui allegare uno o più documenti scansionati"
                                                                                                                ShowSpinButtons="True">
                                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                            </telerik:RadNumericTextBox>
                                                                                                        </td>
                                                                                                        <td style="width: 80px">
                                                                                                            <asp:Label ID="RegistrazioneInternaErroreLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Reg. interna" />
                                                                                                        </td>
                                                                                                        <td style="width: 40px">
                                                                                                            <asp:CheckBox ID="RegistrazioneInternaErroreCheckBox" runat="server" Text="&amp;nbsp;" />
                                                                                                        </td>
                                                                                                        <td align="center" style="width: 30px">
                                                                                                            <asp:ImageButton ID="SalvaAllegatiSelezionatiErroreImageButton" runat="server" ImageUrl="~/images//Save16.png"
                                                                                                                Style="width: 16px" ToolTip="Salva allegati selezionati" ImageAlign="AbsMiddle" />
                                                                                                        </td>
                                                                                                        <td align="center" style="width: 30px">
                                                                                                            <asp:ImageButton ID="EliminaAllegatiSelezionatiErroreImageButton" runat="server"
                                                                                                                ImageUrl="~/images//RecycleEmpty.png" Style="width: 16px" ToolTip="Cancella allegati selezionati"
                                                                                                                ImageAlign="AbsMiddle" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr style="background-color: #FFFFFF">
                                                                                            <td>
                                                                                                <div id="scrollPanelErrore" runat="server" style="overflow: auto; height: 380px;
                                                                                                    border: 1px solid #5D8CC9">
                                                                                                    <telerik:RadGrid ID="AllegatiScansionatiErroreGridView" runat="server" AllowPaging="False"
                                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                        Width="99.8%" AllowSorting="True" AllowMultiRowSelection="True" Culture="it-IT">
                                                                                                        <ClientSettings>
                                                                                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                                            <ClientEvents OnRowDeselected="RowSelected" OnRowSelected="RowSelected" />
                                                                                                        </ClientSettings>
                                                                                                        <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                                            <Columns>
                                                                                                                <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px" ItemStyle-Width="35px">
                                                                                                                </telerik:GridClientSelectColumn>
                                                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                                                                <telerik:GridBoundColumn DataField="IdUtente" DataType="System.Int32" FilterControlAltText="Filter IdUtente column"
                                                                                                                    HeaderText="Id Utente" ReadOnly="True" SortExpression="IdUtente" UniqueName="IdUtente"
                                                                                                                    Visible="False" />
                                                                                                                <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                                                                                    HeaderText="N. prot." DataField="NumeroProtocollo" HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("NumeroProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                            text-overflow: ellipsis; width: 100%;">
                                                                                                                            <%# Eval("NumeroProtocollo")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn SortExpression="AnnoProtocollo" UniqueName="AnnoProtocollo"
                                                                                                                    HeaderText="Anno" DataField="AnnoProtocollo" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("AnnoProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                            text-overflow: ellipsis; width: 100%;">
                                                                                                                            <%# Eval("AnnoProtocollo")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn SortExpression="Utente" UniqueName="Utente" HeaderText="Utente"
                                                                                                                    DataField="Utente" HeaderStyle-Width="170px" ItemStyle-Width="170px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("Utente")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                                            width: 170px;">
                                                                                                                            <%# Eval("Utente")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                                    DataField="NomeFile">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                            text-overflow: ellipsis; width: 100%;">
                                                                                                                            <%# Eval("NomeFile")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn SortExpression="DataAcquisizione" UniqueName="DataAcquisizione"
                                                                                                                    HeaderText="Data acquis." DataField="DataAcquisizione" HeaderStyle-Width="100px"
                                                                                                                    ItemStyle-Width="100px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div title='<%# Eval("DataAcquisizione","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                                            overflow: hidden; text-overflow: ellipsis; width: 100%;">
                                                                                                                            <%# Eval("DataAcquisizione", "{0:dd/MM/yyyy}")%></div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                                                    ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="30px"
                                                                                                                    ItemStyle-Width="30px" />
                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Save" FilterControlAltText="Filter Save column"
                                                                                                                    ImageUrl="~\images\Save16.png" UniqueName="Save" HeaderStyle-Width="30px" ItemStyle-Width="30px" />
                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="30px"
                                                                                                                    ItemStyle-Width="30px" />
                                                                                                            </Columns>
                                                                                                        </MasterTableView>
                                                                                                    </telerik:RadGrid>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </telerik:RadPageView>
                                                                        </telerik:RadMultiPage>
                                                                    </td>
                                                                </tr>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <%--FOOTER--%>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="scrollPosErroreHidden" runat="server" Value="0" />
                    <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
                    <asp:HiddenField ID="infoScansioneHidden" runat="server" />
                    <asp:HiddenField ID="rowSelectedHidden" runat="server" Value="0" />
                    <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
                    <asp:ImageButton ID="ScanUploadButton" Style="display: none" runat="server" ImageUrl="~/images//RecycleEmpty.png" />
                </center>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ScanUploadButton" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
