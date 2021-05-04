<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaCapitoloDedaGroupPage.aspx.vb"
    Inherits="RicercaCapitoloDedaGroupPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Capitoli</title>
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
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />

     <div id="pageContent">
    
        
     <center>

                  <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                   


                                       <tr>
                                        <td  style="background-color: #BFDBFF;padding: 4px; border-bottom:1px solid #9ABBE8;border-top:1px solid #9ABBE8; height:25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" 
                                                runat="server" Style="color: #00156E" Font-Bold="True"  
                                                Text="Elenco Capitoli" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                         <td>
                                                                    <div style="overflow: auto; height: 360px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">

                                                                        <telerik:RadGrid ID="CapitoliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                            Culture="it-IT" AllowFilteringByColumn="True">
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <MasterTableView DataKeyNames="IdCapitolo">
                                                                                <Columns>


                                                                                    <telerik:GridBoundColumn DataField="IdCapitolo" DataType="System.Int32" FilterControlAltText="Filter IdCapitolo column"
                                                                                        HeaderText="IdCapitolo" Visible="false" SortExpression="IdCapitolo" UniqueName="IdCapitolo" />

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="Capitolo"
                                                                                        FilterControlAltText="Filter Capitolo column" HeaderText="Capitolo" SortExpression="Capitolo"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="Capitolo">
                                                                                        <FilterTemplate>
                                                                                            <telerik:RadNumericTextBox Width="100%" ID="NumeroCapitoloTextBox" runat="server"
                                                                                                ClientEvents-OnLoad="OnNumeroCapitoloTextBoxLoad" DbValue='<%# TryCast(Container, GridItem).OwnerTableView.GetColumn("Capitolo").CurrentFilterValue %>'
                                                                                                ClientEvents-OnKeyPress="OnNumeroCapitoloTextBoxKeyPressed" Skin="Office2007">
                                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                            <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                                                                <script type="text/javascript">
                                                                                                    function OnNumeroCapitoloTextBoxKeyPressed(sender, args) {
                                                                                                        if (13 == args.get_keyCode()) {
                                                                                                            var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                            var c = sender.get_textBoxValue();
                                                                                                            tableView.filter("Capitolo", c, "EqualTo");
                                                                                                            args.set_cancel(true);
                                                                                                        }
                                                                                                        var text = sender.get_value() + args.get_keyCharacter();
                                                                                                        if (!text.match('^[0-9\b]+$'))
                                                                                                            args.set_cancel(true);
                                                                                                    }

                                                                                                    // SOVRASCRIVO GLI STILI
                                                                                                    function OnNumeroCapitoloTextBoxLoad(sender, args) {
                                                                                                        sender.get_styles().HoveredStyle[0] = "";
                                                                                                        sender.get_styles().HoveredStyle[1] = "";
                                                                                                        sender.get_styles().FocusedStyle[0] = "";
                                                                                                        sender.get_styles().FocusedStyle[1] = "";
                                                                                                        sender.updateCssClass();
                                                                                                    }
                                                                                                </script>
                                                                                            </telerik:RadScriptBlock>
                                                                                        </FilterTemplate>
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Capitolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                                <%# Eval("Capitolo")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="Articolo"
                                                                                        FilterControlAltText="Filter Articolo column" HeaderText="Articolo" SortExpression="Articolo"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="Articolo">
                                                                                        <FilterTemplate>
                                                                                            <telerik:RadNumericTextBox Width="100%" ID="ArticoloTextBox" runat="server" DbValue='<%# TryCast(Container, GridItem).OwnerTableView.GetColumn("Articolo").CurrentFilterValue %>'
                                                                                                ClientEvents-OnLoad="OnArticoloTextBoxLoad" ClientEvents-OnKeyPress="OnArticoloTextBoxKeyPressed"
                                                                                                Skin="Office2007">
                                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                            <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                                                                                <script type="text/javascript">
                                                                                                    function OnArticoloTextBoxKeyPressed(sender, args) {
                                                                                                        if (13 == args.get_keyCode()) {
                                                                                                            var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                            var c = sender.get_textBoxValue();
                                                                                                            tableView.filter("Articolo", c, "EqualTo");
                                                                                                            args.set_cancel(true);
                                                                                                        }
                                                                                                        var text = sender.get_value() + args.get_keyCharacter();
                                                                                                        if (!text.match('^[0-9\b]+$'))
                                                                                                            args.set_cancel(true);
                                                                                                    }

                                                                                                    // SOVRASCRIVO GLI STILI
                                                                                                    function OnArticoloTextBoxLoad(sender, args) {
                                                                                                        sender.get_styles().HoveredStyle[0] = "";
                                                                                                        sender.get_styles().HoveredStyle[1] = "";
                                                                                                        sender.get_styles().FocusedStyle[0] = "";
                                                                                                        sender.get_styles().FocusedStyle[1] = "";
                                                                                                        sender.updateCssClass();
                                                                                                    }
                                                                                                </script>
                                                                                            </telerik:RadScriptBlock>
                                                                                        </FilterTemplate>
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Articolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                                <%# Eval("Articolo")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                   <%-- <telerik:GridTemplateColumn HeaderStyle-Width="50px" ItemStyle-Width="50px" DataField="DescrizioneTipologiaCapitolo"
                                                                                        FilterControlAltText="Filter DescrizioneTipologiaCapitolo column" HeaderText="Tipo"
                                                                                        SortExpression="DescrizioneTipologiaCapitolo" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                                        FilterControlWidth="100%" ShowFilterIcon="False" UniqueName="DescrizioneTipologiaCapitolo"
                                                                                        AllowFiltering="false">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DescrizioneTipologiaCapitolo")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 50px; border: 0px solid red">
                                                                                                <%# Eval("DescrizioneTipologiaCapitolo")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>--%>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="50px" ItemStyle-Width="50px" DataField="CodiceMissione"
                                                                                        FilterControlAltText="Filter CodiceMissione column" HeaderText="Miss." SortExpression="CodiceMissione"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="CodiceMissione" AllowFiltering="true">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("CodiceMissione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 50px; border: 0px solid red">
                                                                                                <%# Eval("CodiceMissione")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="50px" ItemStyle-Width="50px" DataField="CodiceProgramma"
                                                                                        FilterControlAltText="Filter CodiceProgramma column" HeaderText="Prog." SortExpression="CodiceProgramma"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="CodiceProgramma" AllowFiltering="true">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("CodiceProgramma")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 50px; border: 0px solid red">
                                                                                                <%# Eval("CodiceProgramma")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="MacroAggregato"
                                                                                        FilterControlAltText="Filter MacroAggregato column" HeaderText="M. Aggr." SortExpression="MacroAggregato"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="MacroAggregato" AllowFiltering="true">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("MacroAggregato")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                                <%# Eval("MacroAggregato")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150px" ItemStyle-Width="150px" DataField="Oggetto"
                                                                                        FilterControlAltText="Filter Oggetto column" HeaderText="Descrizione" SortExpression="Oggetto"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="Oggetto">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 150px; border: 0px solid red">
                                                                                                <%# Eval("Oggetto")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150px" ItemStyle-Width="150px" DataField="DescrizioneCdr"
                                                                                        FilterControlAltText="Filter DescrizioneCdr column" HeaderText="Struttura"
                                                                                        SortExpression="DescrizioneCdr" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                                        FilterControlWidth="100%" ShowFilterIcon="False" UniqueName="DescrizioneCdr">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DescrizioneCdr")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 150px; border: 0px solid red">
                                                                                                <%# Eval("DescrizioneCdr")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>


                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" DataField="Disponibilita"
                                                                                        FilterControlAltText="Filter Disponibilita column" HeaderText="Disponibilità"
                                                                                        SortExpression="Disponibilita" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                                        FilterControlWidth="100%" ShowFilterIcon="False" UniqueName="Disponibilita" ItemStyle-HorizontalAlign="Right">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Disponibilita", "{0:N2}")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 100px; border: 0px solid red">
                                                                                                <%# Eval("Disponibilita", "{0:N2}")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>


                                                                                
                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                                        Text="Seleziona Capitolo" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
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

                                          <tr>
                                        <td style="background-color: #BFDBFF;padding: 4px; border-bottom:0px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px; text-align:center">
                                           <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="80px" Skin="Office2007"
                                                            ToolTip="Chiudi finestra">
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
    </form>
</body>
</html>
