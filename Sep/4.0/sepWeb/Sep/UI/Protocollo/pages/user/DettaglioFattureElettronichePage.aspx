<%@ Page Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="DettaglioFattureElettronichePage.aspx.vb" Inherits="DettaglioFattureElettronichePage" %>

<%--SPDX-License-Identifier: GPL-3.0-only--%>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UI/Protocollo/pages/user/VisualizzaFatturaUserControl.ascx" TagName="VisualizzaFatturaControl"
    TagPrefix="parsec" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");
        var hideFatturaPanel = true;
        Sys.Application.add_init(function () {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        });

        function pageLoad() {

            $get("pageContent").appendChild(_backgroundElement);
            $get("pageContent").appendChild(overlay);

            if (hideFatturaPanel) {
                HideFatturaElettronicaPanel();
            } else {
                ShowFatturaElettronicaPanel();
            }

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

        function ShowMessageBox(message) {


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

            var messageBoxPanel = document.getElementById("messageBoxPanel");

            with (messageBoxPanel) {
                style.display = '';
            }

            var messageBox = document.getElementById("messageBox");

            with (messageBox) {
                style.width = '300px';
                style.height = '40px';
                style.backgroundColor = '#BFDBFF';
                style.border = 'solid #4892FF 2px';
                innerHTML = message;
                style.color = '#00156E';
                style.backgroundImage = 'url(/sep/Images/success.png)';
                style.backgroundPosition = '10px center';
                style.backgroundRepeat = 'no-repeat';
                style.padding = '15px 10px 15px 50px';
                style.margin = '15px 0px';

            }

        }

        function HideMessageBox() {
            try {
                var messageBoxPanel = document.getElementById("messageBoxPanel");
                messageBoxPanel.style.display = 'none';
                overlay.style.display = 'none';
            }
            catch (e) { }
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

        function HideFatturaElettronicaPanel() {

            var panel = document.getElementById("FatturaPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
        }

        function ShowFatturaElettronicaPanel() {


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

        function updating(sender, args) {
            var d = $get("progressBar");
            d.style.display = '';
            if (args.get_progressData() && args.get_progressData().OperationComplete == 'true') {
                args.set_cancel(true);
                d.style.display = 'none';
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
            <div id="progressBar" style="position: absolute; width: 100%; text-align: center;
                top: 300px; z-index: 2000000">
                <div id="Div3" style="width: 310px; text-align: center; background-color: #BFDBFF;
                    margin: 0 auto">
                    <telerik:RadProgressArea OnClientProgressUpdating="updating" Skin="Office2007" ID="RadProgressArea1"
                        runat="server" ProgressIndicators="RequestSize,TotalProgressPercent,TotalProgressBar"
                        HeaderText="Esportazione in corso ...">
                        <Localization Total="Totale:" Uploaded="Completato:" />
                    </telerik:RadProgressArea>
                </div>
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
                <center>
                    <telerik:RadProgressManager ID="Radprogressmanager1" runat="server" Skin="Office2007" />
                    <table width="100%" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                                        &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                            Text="Filtro Dettaglio Fatture Elettroniche" />
                                                    </td>
                                                    <td align="center" style="width: 30px; background-color: #BFDBFF; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8;">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Effettua la ricerca con i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px; background-color: #BFDBFF; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8;">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td align="center" style="height: 50px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 90px">
                                                                    <asp:Label ID="DataInvioInizioLabel" runat="server" CssClass="Etichetta" Text="Ricevuta da" />
                                                                </td>
                                                                <td style="width: 125px">
                                                                    <telerik:RadDatePicker ID="DataInvioInizioTextBox" Skin="Office2007" Width="110px"
                                                                        runat="server" MinDate="1753-01-01" ToolTip="Data inizio ricezione fattura">
                                                                        <Calendar>
                                                                            <SpecialDays>
                                                                                <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                            </SpecialDays>
                                                                        </Calendar>
                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td style="width: 20px">
                                                                    <asp:Label ID="DataInvioFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                </td>
                                                                <td style="width: 125px">
                                                                    <telerik:RadDatePicker ID="DataInvioFineTextBox" Skin="Office2007" Width="110px"
                                                                        runat="server" MinDate="1753-01-01" ToolTip="Data fine ricezione fattura">
                                                                        <Calendar>
                                                                            <SpecialDays>
                                                                                <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                            </SpecialDays>
                                                                        </Calendar>
                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td>
                                                                    <div id="ZoneID1">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 100px">
                                                                                    <asp:Label ID="StatoFatturaElettronicaLabel" runat="server" CssClass="Etichetta"
                                                                                        Font-Bold="True" ForeColor="#FF9900" Text="Stato Fattura" Width="100px" />
                                                                                </td>
                                                                                <td style="width: 90px">
                                                                                    <asp:CheckBox ID="chkRicevuta" runat="server" Checked="True" CssClass="etichetta"
                                                                                        Text="Ricevuta" ToolTip="Se selezionato permette di visualizzare solo le fatture ricevute"
                                                                                        Width="90px" />
                                                                                </td>
                                                                                <td style="width: 110px">
                                                                                    <asp:CheckBox ID="chkProtocollata" runat="server" Checked="True" CssClass="etichetta"
                                                                                        Text="Protocollata" ToolTip="Se selezionato permette di visualizzare solo le fatture protocollate"
                                                                                        Width="110px" />
                                                                                </td>
                                                                                <td style="width: 90px">
                                                                                    <asp:CheckBox ID="chkAccettata" runat="server" CssClass="etichetta" Text="Accettata"
                                                                                        ToolTip="Se selezionato permette di visualizzare solo le fatture accettate" Width="90px" />
                                                                                </td>
                                                                                <td style="width: 90px">
                                                                                    <asp:CheckBox ID="chkRifiutata" runat="server" CssClass="etichetta" Text="Rifiutata"
                                                                                        ToolTip="Se selezionato permette di visualizzare solo le fatture rifiutate" Width="90px" />
                                                                                </td>
                                                                                <td style="width: 120px">
                                                                                    <asp:CheckBox ID="chkContabilizzata" runat="server" CssClass="etichetta" Text="Contabilizzata"
                                                                                        ToolTip="Se selezionato permette di visualizzare solo le fatture contabilizzate"
                                                                                        Width="120px" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkConservata" runat="server" CssClass="etichetta" Text="Conservata"
                                                                                        ToolTip="Se selezionato permette di visualizzare solo le fatture conservate"
                                                                                        Width="110px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;<asp:Label ID="TitoloElencoFattureLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Elenco Fatture Elettroniche" />
                                                                </td>
                                                                <td align="center" style="width: 40px">
                                                                    <asp:ImageButton ID="sincronizzazioneFattureButton" Style="border: 0" runat="server"
                                                                        ImageUrl="~/images/Sincro32.png" ToolTip="Sincronizzazione Fatture Elettroniche"
                                                                        ImageAlign="AbsMiddle" />
                                                                </td>
                                                                <td align="center" style="width: 40px">
                                                                    <asp:ImageButton ID="EsportaInExcelImageButton" Style="border: 0" runat="server"
                                                                        ImageUrl="~/images//excel32.png" ToolTip="Esporta le fatture visualizzate in un file formato excel"
                                                                        ImageAlign="AbsMiddle" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="ZoneID2" style="overflow: auto; height: 600px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <telerik:RadGrid ID="FattureElettronicheGridView" runat="server" AllowPaging="True"
                                                                AutoGenerateColumns="False" AllowFilteringByColumn="True" CellSpacing="0" GridLines="None"
                                                                Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT" PageSize="21"
                                                                AllowMultiRowSelection="True">
                                                                <MasterTableView DataKeyNames="Id">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn DataField="IdStato" UniqueName="IdStato" ItemStyle-Width="16px"
                                                                            HeaderStyle-Width="16px" AllowFiltering="False" HeaderTooltip="Stato Fattura">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="IdStato" runat="server" /></ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                        <telerik:GridBoundColumn DataField="NumeroProtocollo" AutoPostBackOnFilter="True"
                                                                            FilterControlAltText="Filter NumeroProtocollo column" HeaderText="N. Prot." SortExpression="NumeroProtocollo"
                                                                            UniqueName="NumeroProtocollo" ShowFilterIcon="False" FilterControlWidth="100%"
                                                                            ItemStyle-Width="59px" HeaderStyle-Width="59px" HeaderTooltip="Numero Protocollo">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="AnnoProtocollo" AutoPostBackOnFilter="True" FilterControlAltText="Filter AnnoProtocollo column"
                                                                            HeaderText="Anno" SortExpression="AnnoProtocollo" UniqueName="AnnoProtocollo"
                                                                            ShowFilterIcon="False" FilterControlWidth="100%" ItemStyle-Width="48px" HeaderStyle-Width="48px"
                                                                            HeaderTooltip="Anno Protocollo">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="150px"
                                                                            HeaderStyle-Width="150px" AutoPostBackOnFilter="True" DataField="DenominazioneFornitore"
                                                                            FilterControlAltText="Filter DenominazioneFornitore column" FilterControlWidth="100%"
                                                                            HeaderText="Fornitore" ShowFilterIcon="False" SortExpression="DenominazioneFornitore"
                                                                            UniqueName="DenominazioneFornitore" HeaderTooltip="Fornitore">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 170px;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("DenominazioneFornitore"), "'", "&#039;")%>'>
                                                                                    <%# Eval("DenominazioneFornitore")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="190px"
                                                                            HeaderStyle-Width="190px" AutoPostBackOnFilter="True" DataField="DenominazioneDestinatario"
                                                                            FilterControlAltText="Filter DenominazioneDestinatario column" FilterControlWidth="100%"
                                                                            HeaderText="Destinatario" ShowFilterIcon="False" SortExpression="DenominazioneDestinatario"
                                                                            UniqueName="DenominazioneDestinatario" HeaderTooltip="Destinatario">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 190px;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("DenominazioneDestinatario"), "'", "&#039;")%>'>
                                                                                    <%# Eval("DenominazioneDestinatario")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" AutoPostBackOnFilter="True"
                                                                            DataField="Oggetto" FilterControlAltText="Filter Oggetto column" FilterControlWidth="100%"
                                                                            HeaderText="Estremi Fattura" ShowFilterIcon="False" SortExpression="Oggetto"
                                                                            UniqueName="Oggetto" HeaderTooltip="Estremi della Fattura">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red"
                                                                                    title='<%# Replace(Eval("Oggetto"), "'", "&#039;")%>'>
                                                                                    <%# Eval("Oggetto")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn DataField="PartitaIvaFornitore" FilterControlAltText="Filter PartitaIvaFornitore column"
                                                                            HeaderText="P. IVA" SortExpression="PartitaIvaFornitore" UniqueName="PartitaIvaFornitore"
                                                                            AutoPostBackOnFilter="True" ShowFilterIcon="False" FilterControlWidth="100%"
                                                                            ItemStyle-Width="70px" HeaderStyle-Width="70px" HeaderTooltip="Partita IVA">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="MessaggioSdI.Nomefile" FilterControlAltText="Filter MessaggioSdI.Nomefile column"
                                                                            HeaderText="Fattura" ShowFilterIcon="False" SortExpression="MessaggioSdI.Nomefile"
                                                                            UniqueName="MessaggioSdI.Nomefile" AllowFiltering="False" Visible="False">
                                                                            <HeaderStyle Width="200px" />
                                                                            <ItemStyle Width="200px" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="MessaggioSdI.PercorsoRelativo" FilterControlAltText="Filter MessaggioSdI.PercorsoRelativo column"
                                                                            HeaderText="PercorsoRelativo" ShowFilterIcon="False" SortExpression="MessaggioSdI.PercorsoRelativo"
                                                                            UniqueName="MessaggioSdI.PercorsoRelativo" Visible="False">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="VersioneFattura" FilterControlAltText="Filter VersioneFattura column"
                                                                            HeaderText="VersioneFattura" ShowFilterIcon="False" SortExpression="VersioneFattura"
                                                                            UniqueName="VersioneFattura" Visible="False">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="CIG" FilterControlAltText="Filter CIG column"
                                                                            HeaderText="CIG" SortExpression="CIG" UniqueName="CIG" AutoPostBackOnFilter="True"
                                                                            ShowFilterIcon="False" FilterControlWidth="100%" ItemStyle-Width="40px" HeaderStyle-Width="40px"
                                                                            HeaderTooltip="CIG">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="CUP" FilterControlAltText="Filter CUP column"
                                                                            HeaderText="CUP" SortExpression="CUP" UniqueName="CUP" AutoPostBackOnFilter="True"
                                                                            ShowFilterIcon="False" FilterControlWidth="100%" ItemStyle-Width="70px" HeaderStyle-Width="70px"
                                                                            HeaderTooltip="CUP" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="185px"
                                                                            HeaderStyle-Width="185px" AutoPostBackOnFilter="True" DataField="CollocazioneUtenza"
                                                                            FilterControlAltText="Filter CollocazioneUtenza column" FilterControlWidth="100%"
                                                                            HeaderText="Collocazione" ShowFilterIcon="False" SortExpression="CollocazioneUtenza"
                                                                            UniqueName="CollocazioneUtenza" HeaderTooltip="Collocazione Utenza">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 185px;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("CollocazioneUtenza"), "'", "&#039;")%>'>
                                                                                    <%# Eval("CollocazioneUtenza")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn DataField="TotaleImponibile" FilterControlAltText="Filter TotaleImponibile column"
                                                                            HeaderText="Imponibile (€)" SortExpression="TotaleImponibile" UniqueName="TotaleImponibile"
                                                                            ItemStyle-HorizontalAlign="Right" AutoPostBackOnFilter="True" ShowFilterIcon="False"
                                                                            FilterControlWidth="100%" ItemStyle-Width="95px" HeaderStyle-Width="95px" HeaderTooltip="Totale Imponibile"
                                                                            AllowFiltering="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="AliquotaIVA" FilterControlAltText="Filter CollocazioneUtenza column"
                                                                            HeaderText="IVA (€)" SortExpression="AliquotaIVA" UniqueName="AliquotaIVA" ItemStyle-HorizontalAlign="Right"
                                                                            AutoPostBackOnFilter="True" ShowFilterIcon="False" FilterControlWidth="100%"
                                                                            ItemStyle-Width="60px" HeaderStyle-Width="60px" HeaderTooltip="Aliquota IVA"
                                                                            AllowFiltering="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="TotaleFattura" FilterControlAltText="Filter TotaleFattura column"
                                                                            HeaderText="Totale (€)" SortExpression="TotaleFattura" UniqueName="TotaleFattura"
                                                                            ItemStyle-HorizontalAlign="Right" AutoPostBackOnFilter="True" ShowFilterIcon="False"
                                                                            FilterControlWidth="100%" ItemStyle-Width="70px" HeaderStyle-Width="70px" HeaderTooltip="Totale Fattura"
                                                                            AllowFiltering="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridButtonColumn ItemStyle-Width="20px" HeaderStyle-Width="20px" ButtonType="ImageButton"
                                                                            CommandName="Anteprima" FilterControlAltText="Filter Anteprima column" ImageUrl="~/images/knob-search16.png"
                                                                            UniqueName="Anteprima" Text="Anteprima Fattura" HeaderTooltip="Anteprima Fattura">
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
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                border-top: 1px solid  #9ABBE8; height: 25px">
                            </td>
                        </tr>
                    </table>
                </center>
                <div id="messageBoxPanel" style="position: absolute; width: 100%; text-align: center;
                    z-index: 2000000; display: none; top: 300px">
                    <div id="messageBox" style="width: 300px; height: 40; text-align: center; background-color: #BFDBFF;
                        margin: 0 auto;">
                    </div>
                </div>
                <div id="FatturaPanel" style="position: absolute; width: 100%; text-align: center;
                    z-index: 2000000; display: none; top: 40px">
                    <div id="ShadowFatturaPanel" style="width: 800px; text-align: center; background-color: #BFDBFF;
                        margin: 0 auto">
                        <parsec:VisualizzaFatturaControl runat="server" ID="VisualizzaFatturaControl" />
                    </div>
                </div>
                <table style="width: 100%; border: 1px solid #9ABBE8; text-align: center">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 80px">
                                        <asp:Label ID="LegendaLabel" runat="server" CssClass="Etichetta" Text="Legenda :"
                                            Font-Bold="True" ForeColor="#FF9900" />
                                    </td>
                                    <td>
                                        <img alt="" src="../../../../Images/pGiallo16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaRicevuta" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Ricevuta" Width="80px" />
                                    </td>
                                    <td>
                                        <img alt="" src="../../../../Images/pArancio16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaProtocollata" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Protocollata" Width="90px" />
                                    </td>
                                    <td>
                                        <img alt="" src="../../../../Images/pVerde16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaAccettata" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Accettata" Width="80px" />
                                    </td>
                                    <td>
                                        <img alt="" src="../../../../Images/pRosso16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaRifiutata" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Rifiutata" Width="80px" />
                                    </td>
                                    <td>
                                        <img alt="" src="../../../../Images/pFucsia16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaContabilizzata" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Contabilizzata" Width="100px" />
                                    </td>
                                    <td>
                                        <img alt="" src="../../../../Images/pBlue16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaConservata" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Conservata" Width="90px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
                <asp:ImageButton ID="AggiornaEmailButton" runat="server" ImageUrl="~/images//knob-search16.png"
                    Style="display: none" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
