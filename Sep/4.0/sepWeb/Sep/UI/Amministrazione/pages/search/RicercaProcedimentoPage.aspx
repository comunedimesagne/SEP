<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaProcedimentoPage.aspx.vb"
    Inherits="RicercaProcedimentoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Procedimento</title>
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
                                        Text="Elenco Procedimenti" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ContainerMargin">
                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <div style="overflow: auto; height: 400px; width: 100%; background-color: #FFFFFF;
                                                    border: 0px solid #5D8CC9;">
                                                    <telerik:RadGrid ID="ProcedimentiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        ToolTip="Elenco Procedimenti" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                        Width="99.8%" AllowSorting="True" Culture="it-IT" AllowFilteringByColumn="True">
                                                        <MasterTableView DataKeyNames="Id">
                                                            <Columns>

                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                              
                                                              
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="500px" ItemStyle-Width="500px" DataField="Descrizione"
                                                                    FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                                    UniqueName="Descrizione" AutoPostBackOnFilter="True" FilterControlWidth="100%"
                                                                    ShowFilterIcon="False">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                            overflow: hidden; text-overflow: ellipsis; width: 500px; border: 0px solid red">
                                                                            <%# Eval("Descrizione")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                 <telerik:GridTemplateColumn HeaderStyle-Width="200px" ItemStyle-Width="200px" DataField="DescrizioneSettore"
                                                                    FilterControlAltText="Filter DescrizioneSettore column" HeaderText="Settore/Area" SortExpression="DescrizioneSettore"
                                                                    UniqueName="DescrizioneSettore" AutoPostBackOnFilter="True" FilterControlWidth="100%"
                                                                    ShowFilterIcon="False">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Replace(Eval("DescrizioneSettore"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                            overflow: hidden; text-overflow: ellipsis; width: 200px; border: 0px solid red">
                                                                            <%# Eval("DescrizioneSettore")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>


                                                                

                                                                <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="Tempo"
                                                                    FilterControlAltText="Filter Tempo column" HeaderText="Termine" SortExpression="Tempo"
                                                                    AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" FilterControlWidth="100%"
                                                                    ShowFilterIcon="False" UniqueName="Tempo">
                                                                    <FilterTemplate>
                                                                        <telerik:RadNumericTextBox Width="100%" ID="TempoTextBox" runat="server"
                                                                            ClientEvents-OnLoad="OnTempoTextBoxLoad" DbValue='<%# TryCast(Container,GridItem).OwnerTableView.GetColumn("Tempo").CurrentFilterValue %>'
                                                                            ClientEvents-OnKeyPress="OnTempoTextBoxKeyPressed" Skin="Office2007">
                                                                            <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                        </telerik:RadNumericTextBox>
                                                                        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                                            <script type="text/javascript">
                                                                                function OnTempoTextBoxKeyPressed(sender, args) {
                                                                                if (13 == args.get_keyCode()) {
                                                                                    var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                    var c = sender.get_textBoxValue();
                                                                                    tableView.filter("Tempo", c, "EqualTo");
                                                                                    args.set_cancel(true);
                                                                                }
                                                                                var text = sender.get_value() + args.get_keyCharacter();
                                                                                if (!text.match('^[0-9]+$'))
                                                                                    args.set_cancel(true);
                                                                            }

                                                                            // SOVRASCRIVO GLI STILI
                                                                            function OnTempoTextBoxLoad(sender, args) {
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
                                                                        <div title='<%# Eval("Tempo")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                            width: 70px; border: 0px solid red">
                                                                            <%# Eval("Tempo")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>





                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                    Text="Seleziona Procedimento" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                    ImageUrl="~\images\checks.png" UniqueName="Select" />

                                                                   
                                                            </Columns>
                                                        </MasterTableView>
                                                          <GroupingSettings CaseSensitive="False" />
                                                        </telerik:RadGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
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
