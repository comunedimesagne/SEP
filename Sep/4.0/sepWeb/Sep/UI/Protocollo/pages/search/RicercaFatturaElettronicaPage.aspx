<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaFatturaElettronicaPage.aspx.vb"
    Inherits="RicercaFatturaElettronicaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Impegno</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .rgAltRow, .rgRow
        {
            cursor: pointer !important;
        }
        .style1
        {
            height: 25px;
        }
    </style>
</head>
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
<body>
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
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
        DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <center>
                    <table width="905px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <%--HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                            Text="Elenco Fatture Elettroniche" />
                                                    </td>
                                                    <td align="center" style="width: 30px; background-color: #BFDBFF;">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Effettua la ricerca con i filtri impostati" Style="border-style: none;
                                                            border-color: inherit; border-width: 0; width: 16px;" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px; background-color: #BFDBFF;">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%--FILTER--%>
                                    <tr style="height: 30px;">
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 80px">
                                                        <asp:Label ID="DataInvioInizioLabel" runat="server" CssClass="Etichetta" Text="Ricevuta da" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="DataInvioInizioTextBox" Skin="Office2007" Width="110px"
                                                            runat="server" MinDate="1753-01-01" ToolTip="Data inizio ricezione fattura">
                                                            <Calendar ID="Calendar1" runat="server">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                </SpecialDays>
                                                            </Calendar>
                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td style="width: 10px">
                                                        <asp:Label ID="DataInvioFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="DataInvioFineTextBox" Skin="Office2007" Width="110px"
                                                            runat="server" MinDate="1753-01-01" ToolTip="Data fine ricezione fattura">
                                                            <Calendar ID="Calendar2" runat="server">
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
                                                                    <td style="width: 180px">
                                                                        <asp:Label ID="StatoFatturaElettronicaLabel" runat="server" CssClass="Etichetta"
                                                                            Font-Bold="True" ForeColor="#FF9900" Text="Stato Fattura" />
                                                                    </td>
                                                                    <td style="width: 100px">
                                                                        <asp:CheckBox ID="ProtocollataCheckBox" runat="server" Checked="True" CssClass="etichetta"
                                                                            Text="Protocollata" ToolTip="Se selezionato permette di visualizzare solo le fatture protocollate"
                                                                            Width="100px" />
                                                                    </td>
                                                                    <td style="width: 80px">
                                                                        <asp:CheckBox ID="AccettataCheckBox" runat="server" CssClass="etichetta" Text="Accettata"
                                                                            ToolTip="Se selezionato permette di visualizzare solo le fatture accettate" Width="80px" />
                                                                    </td>
                                                                    <td style="width: 80px">
                                                                        <asp:CheckBox ID="RifiutataCheckBox" runat="server" CssClass="etichetta" Text="Rifiutata"
                                                                            ToolTip="Se selezionato permette di visualizzare solo le fatture rifiutate" Width="80px" />
                                                                    </td>
                                                                    <td style="width: 110px">
                                                                        <asp:CheckBox ID="ContabilizzataCheckBox" runat="server" CssClass="etichetta" Text="Contabilizzata"
                                                                            ToolTip="Se selezionato permette di visualizzare solo le fatture contabilizzate"
                                                                            Width="110px" />
                                                                    </td>
                                                                    <td style="width: 95px">
                                                                        <asp:CheckBox ID="ConservataCheckBox" runat="server" CssClass="etichetta" Text="Conservata"
                                                                            ToolTip="Se selezionato permette di visualizzare solo le fatture conservate"
                                                                            Width="95px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%--BODY--%>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 360px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <telerik:RadGrid ID="FattureElettronicheGridView" runat="server" AllowPaging="True"
                                                                AutoGenerateColumns="False" AllowFilteringByColumn="True" CellSpacing="0" GridLines="None"
                                                                Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT" PageSize="20"
                                                                AllowMultiRowSelection="True">
                                                                <MasterTableView DataKeyNames="Id" TableLayout="fixed">
                                                                    <NoRecordsTemplate>
                                                                        <div>
                                                                            Nessuna Fattura Trovata</div>
                                                                    </NoRecordsTemplate>
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn DataField="IdStato" UniqueName="IdStato" ItemStyle-Width="30px"
                                                                            HeaderStyle-Width="30px" AllowFiltering="False" HeaderTooltip="Stato Fattura">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="IdStato" runat="server" /></ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                        <telerik:GridBoundColumn DataField="NumeroProtocollo" AutoPostBackOnFilter="True"
                                                                            FilterControlAltText="Filter NumeroProtocollo column" HeaderText="N. Prot." SortExpression="NumeroProtocollo"
                                                                            UniqueName="NumeroProtocollo" ShowFilterIcon="False" FilterControlWidth="100%"
                                                                            ItemStyle-Width="75px" HeaderStyle-Width="75px" HeaderTooltip="Numero Protocollo">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn AutoPostBackOnFilter="True" FilterControlWidth="100%" DataField="AnnoProtocollo"
                                                                            FilterControlAltText="Filter AnnoProtocollo column" HeaderText="Anno" SortExpression="AnnoProtocollo"
                                                                            UniqueName="AnnoProtocollo" ShowFilterIcon="False" ItemStyle-Width="65px" HeaderStyle-Width="65px"
                                                                            HeaderTooltip="Anno Protocollo">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="190px"
                                                                            HeaderStyle-Width="190px" AutoPostBackOnFilter="True" DataField="DenominazioneFornitore"
                                                                            FilterControlAltText="Filter DenominazioneFornitore column" FilterControlWidth="100%"
                                                                            HeaderText="Fornitore" ShowFilterIcon="False" SortExpression="DenominazioneFornitore"
                                                                            UniqueName="DenominazioneFornitore" HeaderTooltip="Fornitore">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%;
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
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%;
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
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("Oggetto"), "'", "&#039;")%>'>
                                                                                    <%# Eval("Oggetto")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn AllowFiltering="False" DataField="MessaggioSdI.DataRicezioneInvio"
                                                                            DataType="System.DateTime" FilterControlAltText="Filter MessaggioSdI.DataRicezioneInvio column"
                                                                            HeaderText="Ricezione" ShowFilterIcon="False" SortExpression="MessaggioSdI.DataRicezioneInvio"
                                                                            UniqueName="MessaggioSdI.DataRicezioneInvio" DataFormatString="{0:dd/MM/yyyy}"
                                                                            HeaderTooltip="Data di ricezione della Fattura" ItemStyle-Width="90px" HeaderStyle-Width="90px">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="30px"
                                                                            Text="Seleziona Fattura Elettronica" ItemStyle-Width="30px" FilterControlAltText="Filter Select column"
                                                                            ImageUrl="~\images\checks.png" UniqueName="Select" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%--FOOTER--%>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="80px" Skin="Office2007"
                                                ToolTip="Chiudi finestra" AutoPostBack="false">
                                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
