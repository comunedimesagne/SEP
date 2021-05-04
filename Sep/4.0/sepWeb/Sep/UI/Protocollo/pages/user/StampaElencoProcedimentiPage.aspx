<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="StampaElencoProcedimentiPage.aspx.vb" Inherits="StampaElencoProcedimentiPage" %>

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

        //***************************************************************************************************************************
        //SELEZIONA LA CHECKBOX 'SELEZIONA TUTTO' QUANDO LE CHECKBOX DEL CONTROLLO RADLISTBOX SONO TUTTE SELEZIONATE.
        //***************************************************************************************************************************
        function OnItemChecked(sender, e) {
            var anteprimaStampaButton = $find('<%= AnteprimaStampaButton.ClientId %>');
            var esportaButton = $find('<%= EsportaButton.ClientId %>');
            var items = sender.get_items();
            var chk = $get('<%= SelezionaTuttiUtentiCheckBox.ClientId %>');
            chk.checked = (sender.get_checkedItems().length == items.get_count());
            var checked = (sender.get_checkedItems().length > 0);
        }


        //***************************************************************************************************************************
        //SELEZIONA-DESELEZIONA TUTTE LE CHECKBOX DEL CONTROLLO RADLISTBOX QUANDO VIENE SELEZIONATA LA CHECKBOX 'SELEZIONA TUTTO'.
        //***************************************************************************************************************************
        function OnCheckBoxClick(checkBox) {
            var listbox = $find('<%= UtentiListBox.ClientId %>');
            var anteprimaStampaButton = $find('<%= AnteprimaStampaButton.ClientId %>');
            var esportaButton = $find('<%= EsportaButton.ClientId %>');
            var items = listbox.get_items();
            var checked = checkBox.checked;
            items.forEach(function (itm) { itm.set_checked(checked); });

        }

        //***************************************************************************************************************************
        //SELEZIONA LA CHECKBOX 'SELEZIONA TUTTO' QUANDO LE CHECKBOX DEL CONTROLLO RADLISTBOX SONO TUTTE SELEZIONATE.
        //***************************************************************************************************************************
        function OnProcedimentiItemChecked(sender, e) {
            var anteprimaStampaButton = $find('<%= AnteprimaStampaButton.ClientId %>');
            var esportaButton = $find('<%= EsportaButton.ClientId %>');
            var items = sender.get_items();
            var chk = $get('<%= SelezionaTuttiProcedimentiCheckBox.ClientId %>');
            chk.checked = (sender.get_checkedItems().length == items.get_count());
            var checked = (sender.get_checkedItems().length > 0);
        }

        //***************************************************************************************************************************
        //SELEZIONA-DESELEZIONA TUTTE LE CHECKBOX DEL CONTROLLO RADLISTBOX QUANDO VIENE SELEZIONATA LA CHECKBOX 'SELEZIONA TUTTO'.
        //***************************************************************************************************************************
        function OnProcedimentiCheckBoxClick(checkBox) {
            var listbox = $find('<%= ProcedimentiListBox.ClientId %>');
            var anteprimaStampaButton = $find('<%= AnteprimaStampaButton.ClientId %>');
            var esportaButton = $find('<%= EsportaButton.ClientId %>');
            var items = listbox.get_items();
            var checked = checkBox.checked;
            items.forEach(function (itm) { itm.set_checked(checked); });

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
                                                        &nbsp;<asp:Label ID="PannelloFiltroLabel" runat="server" Font-Bold="True" Style="color: #00156E;
                                                            background-color: #BFDBFF" Text="Filtro Procedimenti" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Applica i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%-- CONTENT--%>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 100%; width: 100%; background-color: #DFE8F6;
                                                            border: 0px solid #5D8CC9;">
                                                            <table style="width: 100%">
                                                                <tr style="height: 25px">
                                                                    <td style="width: 50px">
                                                                        <asp:Label ID="StatoProcedimentoLabel" runat="server" CssClass="Etichetta" Text="Stato" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px; width: 190px">
                                                                        <asp:Panel ID="StatoProcedimentoPanel" runat="server">
                                                                            <telerik:RadComboBox ID="StatoProcedimentoComboBox" AutoPostBack="true" runat="server"
                                                                                EmptyMessage="- Seleziona Stato Procedimento -" MaxHeight="150px" Skin="Office2007"
                                                                                Width="100%" />
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td style="width: 50px; text-align: right">
                                                                        <asp:Label ID="DataProcedimentoLabel" runat="server" CssClass="Etichetta" Text="Data" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center" style="width: 40px; height: 26px;">
                                                                                    <asp:Label ID="DataProcedimentoInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                                </td>
                                                                                <td style="width: 80px; height: 26px;">
                                                                                    <telerik:RadDatePicker ID="DataProcedimentoInizioTextBox" Skin="Office2007" Width="110px"
                                                                                        runat="server" MinDate="1753-01-01">
                                                                                        <Calendar>
                                                                                            <SpecialDays>
                                                                                                <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                            </SpecialDays>
                                                                                        </Calendar>
                                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                                    </telerik:RadDatePicker>
                                                                                </td>
                                                                                <td align="center" style="width: 40px; height: 26px;">
                                                                                    <asp:Label ID="DataProcedimentoFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                                </td>
                                                                                <td style="height: 26px">
                                                                                    <telerik:RadDatePicker ID="DataProcedimentoFineTextBox" Skin="Office2007" Width="110px"
                                                                                        runat="server" MinDate="1753-01-01">
                                                                                        <Calendar>
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
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 50%">
                                                        <%-- INIZIO TABELLA RISULTATI--%>
                                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                        <%-- HEADER--%>
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <div>
                                                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                                                        <tr>
                                                                                            <td>
                                                                                                &nbsp;<asp:Label ID="ElencoUtentiLabel" runat="server" Font-Bold="True" Style="width: 260px;
                                                                                                    color: #00156E; background-color: #BFDBFF" Text="Elenco Utenti" />
                                                                                            </td>
                                                                                            <td style="width: 30px">
                                                                                                <asp:CheckBox CssClass="Etichetta" ID="SelezionaTuttiUtentiCheckBox" runat="server"
                                                                                                    Text="&nbsp;" AutoPostBack="false" onclick="OnCheckBoxClick(this);" />
                                                                                            </td>
                                                                                            <td style="width: 110px">
                                                                                                <asp:Label ID="SelezionaTuttiUtentiLabel" runat="server" Text="Seleziona Tutto" CssClass="Etichetta"
                                                                                                    Font-Bold="True" Style="color: #00156E" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
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
                                                                                                        <telerik:RadListBox ID="UtentiListBox" runat="server" Skin="Office2007" Style="width: 100%;
                                                                                                            height: 200px" Height="400px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True"
                                                                                                            SelectionMode="Multiple" OnClientItemChecked="OnItemChecked">
                                                                                                        </telerik:RadListBox>
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
                                                        <%-- FINE TABELLA RISULTATI--%>
                                                    </td>
                                                    <td style="width: 50%">
                                                        <%-- INIZIO TABELLA RISULTATI--%>
                                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                        <%-- HEADER--%>
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <div>
                                                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                                                        <tr>
                                                                                            <td>
                                                                                                &nbsp;<asp:Label ID="ElencoProcedimentiLabel" runat="server" Font-Bold="True" Style="width: 260px;
                                                                                                    color: #00156E; background-color: #BFDBFF" Text="Elenco Procedimenti" />
                                                                                            </td>
                                                                                            <td style="width: 30px">
                                                                                                <asp:CheckBox CssClass="Etichetta" ID="SelezionaTuttiProcedimentiCheckBox" runat="server"
                                                                                                    Text="&nbsp;" AutoPostBack="false" onclick="OnProcedimentiCheckBoxClick(this);" />
                                                                                            </td>
                                                                                            <td style="width: 110px">
                                                                                                <asp:Label ID="SelezionaTuttiProcedimentiLabel" runat="server" Text="Seleziona Tutto"
                                                                                                    CssClass="Etichetta" Font-Bold="True" Style="color: #00156E" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
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
                                                                                                        <telerik:RadListBox ID="ProcedimentiListBox" runat="server" Skin="Office2007" Style="width: 100%;
                                                                                                            height: 200px" Height="400px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True"
                                                                                                            SelectionMode="Multiple" OnClientItemChecked="OnProcedimentiItemChecked">
                                                                                                        </telerik:RadListBox>
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
                                                        <%-- FINE TABELLA RISULTATI--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%; background-color: #BFDBFF">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;<asp:Label ID="TitoloLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                                                            color: #00156E; background-color: #BFDBFF" Text="Elenco Procedimenti" CssClass="Etichetta" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                                                border: 0px solid #5D8CC9;">
                                                                <telerik:RadGrid ID="ProcedimentiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                    CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="True" Culture="it-IT">
                                                                    <MasterTableView DataKeyNames="Id">
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="600px" ItemStyle-Width="600px" DataField="Descrizione"
                                                                                FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                                                UniqueName="Descrizione">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                        overflow: hidden; text-overflow: ellipsis; width: 600px; border: 0px solid red">
                                                                                        <%# Eval("Descrizione")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="Conteggio"
                                                                                HeaderTooltip="Numero di volte che il procedimento è stato avviato" FilterControlAltText="Filter Conteggio column"
                                                                                HeaderText="Frequenza" SortExpression="Conteggio" UniqueName="Conteggio">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("Conteggio")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                        <%# Eval("Conteggio")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="Tempo" UniqueName="Tempo" HeaderText="Termine"
                                                                                HeaderTooltip="Termine di conclusione espresso in giorni" DataField="Tempo" HeaderStyle-Width="65px"
                                                                                ItemStyle-Width="65px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("Tempo") %>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                        width: 65px; border: 0px solid red">
                                                                                        <%# Eval("Tempo")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="Media" UniqueName="Media" HeaderText="Media"
                                                                                HeaderTooltip="Valore medio dei giorni trascorsi dall'avvio del procedimento"
                                                                                DataField="Media" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("Media") %>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                        width: 65px; border: 0px solid red">
                                                                                        <%# Eval("Media")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
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
                                    <%--FOOTER--%>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="AnteprimaStampaButton" runat="server" Text="Stampa" Width="100px"
                                                Skin="Office2007" ToolTip="Effettua la stampa degli atti selezionati">
                                                <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="EsportaButton" runat="server" Text="Esporta" Width="100px"
                                                Skin="Office2007" ToolTip="Effettua l'esportazione dei procedimenti">
                                                <Icon PrimaryIconUrl="../../../../images/export.png" PrimaryIconLeft="5px" />
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
</asp:Content>
