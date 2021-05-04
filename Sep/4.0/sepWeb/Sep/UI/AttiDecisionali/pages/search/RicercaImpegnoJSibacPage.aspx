<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaImpegnoJSibacPage.aspx.vb"
    Inherits="RicercaImpegnoJSibacPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text="Elenco Impegni" />
                                        </td>
                                    </tr>

                                    <tr style="height: 30px;">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style=" width:160px;">
                                                        &nbsp;<asp:Label ID="AnnoLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                            Text="Anno Es. Impegno" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="AnniComboBox" AutoPostBack="true" runat="server" EmptyMessage="- Seleziona -"
                                                            MaxHeight="150px" Skin="Office2007" Width="120px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                         <td>
                                                                    <div style="overflow: auto; height: 360px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">

                                                                        <telerik:RadGrid ID="ImpegniGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                            Culture="it-IT" AllowFilteringByColumn="True">
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <MasterTableView DataKeyNames="Id">
                                                                                <Columns>


                                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />

                                                                                   
                                                                                      <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroCapitolo"
                                                                                        FilterControlAltText="Filter NumeroCapitolo column" HeaderText="Capitolo" SortExpression="NumeroCapitolo"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="NumeroCapitolo">
                                                                                        <FilterTemplate>
                                                                                            <telerik:RadNumericTextBox Width="100%" ID="NumeroCapitoloTextBox" runat="server"
                                                                                                ClientEvents-OnLoad="OnNumeroCapitoloTextBoxLoad" DbValue='<%# TryCast(Container,GridItem).OwnerTableView.GetColumn("NumeroCapitolo").CurrentFilterValue %>'
                                                                                                ClientEvents-OnKeyPress="OnNumeroCapitoloTextBoxKeyPressed" Skin="Office2007">
                                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                            <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                                                                <script type="text/javascript">
                                                                                                    function OnNumeroCapitoloTextBoxKeyPressed(sender, args) {
                                                                                                        if (13 == args.get_keyCode()) {
                                                                                                            var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                            var c = sender.get_textBoxValue();
                                                                                                            tableView.filter("NumeroCapitolo", c, "EqualTo");
                                                                                                            args.set_cancel(true);
                                                                                                        }
                                                                                                        var text = sender.get_value() + args.get_keyCharacter();
                                                                                                        if (!text.match('^[0-9]+$'))
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
                                                                                            <div title='<%# Eval("DescrizioneCapitolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                                <%# Eval("NumeroCapitolo")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="Articolo"
                                                                                        FilterControlAltText="Filter Articolo column" HeaderText="Articolo" SortExpression="Articolo"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="Articolo">
                                                                                        <FilterTemplate>
                                                                                            <telerik:RadNumericTextBox Width="100%" ID="ArticoloTextBox" runat="server" DbValue='<%# TryCast(Container,GridItem).OwnerTableView.GetColumn("Articolo").CurrentFilterValue %>'
                                                                                                ClientEvents-OnLoad="OnArticoloTextBoxLoad" ClientEvents-OnKeyPress="OnArticoloTextBoxKeyPressed"
                                                                                                Skin="Office2007">
                                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                            <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                                                                                <script type="text/javascript">
                                                                                                    function OnArticoloTextBoxKeyPressed(sender, args) {
                                                                                                        if (13 == args.get_keyCode()) {
                                                                                                            var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                            var c = sender.get_textBoxValue();
                                                                                                            tableView.filter("Articolo", c, "EqualTo");
                                                                                                            args.set_cancel(true);
                                                                                                        }
                                                                                                        var text = sender.get_value() + args.get_keyCharacter();
                                                                                                        if (!text.match('^[0-9]+$'))
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





                                                                                 

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="50px" ItemStyle-Width="50px" DataField="AnnoImpegno"
                                                                                        FilterControlAltText="Filter AnnoImpegno column" HeaderText="Anno"
                                                                                        SortExpression="AnnoImpegno" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                                        FilterControlWidth="100%" ShowFilterIcon="False" UniqueName="AnnoImpegno" AllowFiltering="false">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("AnnoImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 50px; border: 0px solid red">
                                                                                                <%# Eval("AnnoImpegno")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>


                                                                                   


                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroImpegno"
                                                                                        FilterControlAltText="Filter NumeroImpegno column" HeaderText="Numero" SortExpression="NumeroImpegno"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="NumeroImpegno">
                                                                                        <FilterTemplate>
                                                                                            <telerik:RadNumericTextBox Width="100%" ID="NumeroImpegnoTextBox" runat="server"
                                                                                                ClientEvents-OnLoad="OnNumeroImpegnoTextBoxLoad" DbValue='<%# TryCast(Container,GridItem).OwnerTableView.GetColumn("NumeroImpegno").CurrentFilterValue %>'
                                                                                                ClientEvents-OnKeyPress="OnNumeroImpegnoTextBoxKeyPressed" Skin="Office2007">
                                                                                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                            <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                                                                                <script type="text/javascript">
                                                                                                    function OnNumeroImpegnoTextBoxKeyPressed(sender, args) {
                                                                                                        if (13 == args.get_keyCode()) {
                                                                                                            var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                            var c = sender.get_textBoxValue();
                                                                                                            tableView.filter("NumeroImpegno", c, "EqualTo");
                                                                                                            args.set_cancel(true);
                                                                                                        }
                                                                                                        var text = sender.get_value() + args.get_keyCharacter();
                                                                                                        if (!text.match('^[0-9]+$'))
                                                                                                            args.set_cancel(true);
                                                                                                    }

                                                                                                    // SOVRASCRIVO GLI STILI
                                                                                                    function OnNumeroImpegnoTextBoxLoad(sender, args) {
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
                                                                                            <div title='<%# Eval("NumeroImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                                <%# Eval("NumeroImpegno")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>


                                                                                       <telerik:GridTemplateColumn HeaderStyle-Width="380px" ItemStyle-Width="380px" DataField="Descrizione"
                                                                                        FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" UniqueName="Descrizione">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 380px; border: 0px solid red">
                                                                                                <%# Eval("Descrizione")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>


                                                                                    <telerik:GridTemplateColumn SortExpression="Importo" UniqueName="Importo" 
                                                                                        HeaderText="Importo" DataField="Importo"
                                                                                        ItemStyle-HorizontalAlign="Right"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" FilterControlWidth="100%"
                                                                                        ShowFilterIcon="False" FilterControlAltText="Filter Importo column">
                                                                                        
                                                                                        <FilterTemplate>
                                                                                            <telerik:RadNumericTextBox ID="ImportoTextBox" runat="server" Width="100%" DbValue='<%# CType(Container,GridItem).OwnerTableView.GetColumn("Importo").CurrentFilterValue %>'
                                                                                                ClientEvents-OnLoad="OnImportoTextBoxLoad" ClientEvents-OnKeyPress="OnImportoTextBoxKeyPressed"
                                                                                                Skin="Office2007" />
                                                                                          
                                                                                            <telerik:RadScriptBlock ID="RadScriptBlock4" runat="server">
                                                                                                <script type="text/javascript">
                                                                                                    function OnImportoTextBoxKeyPressed(sender, args) {
                                                                                                        if (13 == args.get_keyCode()) {
                                                                                                            var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                            var c = sender.get_textBoxValue();
                                                                                                            tableView.filter("Importo", c, "EqualTo");
                                                                                                            args.set_cancel(true);
                                                                                                        }
                                                                                                    }

                                                                                                    // SOVRASCRIVO GLI STILI
                                                                                                    function OnImportoTextBoxLoad(sender, args) {
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
                                                                                            <div title='<%# Eval("Importo","{0:N2}")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis;  border: 0px solid red">
                                                                                                <%# Eval("Importo", "{0:N2}")%></div>
                                                                                        </ItemTemplate>

                                                                                    </telerik:GridTemplateColumn>







                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                                        Text="Seleziona Impegno" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
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
                                        <td align="center"  style="background-color: #BFDBFF;padding: 4px; border-bottom:0px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
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
    </form>
</body>
</html>
