<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaBozzaPage.aspx.vb"
    Inherits="RicercaBozzaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Bozza</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />

     <div id="pageContent">
    
    <center>


        <table width="800px" cellpadding="5" cellspacing="5" border="0">
            <tr>
                <td>
                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                           <td>
                               <table style="width:100%" cellpadding="0" cellspacing="0">
                                   <tr>
                                       <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 25px">
                                           <asp:Label ID="DocumentiLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                               Text="Documenti" />
                                       </td>
                                  
                                       <td style="background-color: #4892FF; width: 40px; text-align:center">
                                           <asp:Table ID="componentPlaceHolder" runat="server" CellPadding="0" CellSpacing="0"
                                               ToolTip="Componenti installati" Style="width: 100%">
                                               <asp:TableRow>
                                               </asp:TableRow>
                                           </asp:Table>

                                       </td>
                                   </tr>

                               </table>
                           </td>
                           
                        </tr>
                        <tr>
                            <td class="ContainerMargin">
                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                    style="background-color: #BFDBFF">
                                    <tr>
                                        <td>
                                            <telerik:RadTabStrip ID="DocumentiTabStrip" runat="server" MultiPageID="DocumentiMultiPage"
                                                SelectedIndex="0" Skin="Office2007" Width="100%">
                                                <Tabs>
                                                    <telerik:RadTab Selected="True" Text="Bozze" Style="text-align: center" />
                                                    <telerik:RadTab Text="Determine" Style="text-align: center" />
                                                   
                                                </Tabs>
                                            </telerik:RadTabStrip>

                                            <telerik:RadMultiPage ID="DocumentiMultiPage" runat="server" BorderColor="#3399FF"
                                                CssClass="multiPage" Height="100%" SelectedIndex="0">

                                           
                                                <telerik:RadPageView ID="BozzePageView" runat="server" CssClass="corporatePageView"
                                                    Height="230px">
                                                    <div id="BozzePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="TitoloBozzeLabel" runat="server" Font-Bold="True" Style="width: 300px; color: #00156E; background-color: #BFDBFF"
                                                                                    Text="Elenco Bozze" />
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                            
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="overflow: auto; height: 185px; width: 100%; background-color: #FFFFFF; border: 0px solid #5D8CC9;">

                                                                    

                                                                        <telerik:RadGrid ID="BozzeGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True" Culture="it-IT" AllowFilteringByColumn="True">
                                                                            <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                <Columns>

                                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />

                                                                                    <telerik:GridTemplateColumn  DataField="Descrizione"
                                                                                        FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                                        FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                        UniqueName="Descrizione">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                <%# Eval("Descrizione")%>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>


                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="250px" ItemStyle-Width="250px" DataField="Utente"
                                                                                        FilterControlAltText="Filter Utente column" HeaderText="Utente" SortExpression="Utente"
                                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                                        FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                        UniqueName="Utente">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Utente")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                <%# Eval("Utente")%>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="VisualizzaDocumento"
                                                                                        FilterControlAltText="Filter VisualizzaDocumento column" ImageUrl="~\images\Documento16.gif"
                                                                                        UniqueName="VisualizzaDocumento" HeaderStyle-Width="30px" Text="Visualizza Bozza..."
                                                                                        ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />

                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="30px" Text="Seleziona Bozza"
                                                                                        ItemStyle-Width="30px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png"
                                                                                        UniqueName="Select"  ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />

                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>

                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </telerik:RadPageView>

                                                <telerik:RadPageView ID="DeterminePageView" runat="server" CssClass="corporatePageView"
                                                    Height="230px">
                                                    <div id="DeterminePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="TitoloDetermineLabel" runat="server" Font-Bold="True" Style="width: 300px; color: #00156E; background-color: #BFDBFF"
                                                                                    Text="Elenco Determine" />
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                             
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="overflow: auto; height: 185px; width: 100%; background-color: #FFFFFF; border: 0px solid #5D8CC9;">

                                  <telerik:RadGrid ID="DetermineGridView" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                    AllowSorting="True" AllowFilteringByColumn="True" EnableLinqExpressions="false" PageSize="5"
                                    Culture="it-IT">
                                    <GroupingSettings CaseSensitive="False" />
                                    <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                        <Columns>
                                          
                                              <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="false" />
                                            
                                            <telerik:GridTemplateColumn SortExpression="ContatoreGenerale" UniqueName="ContatoreGenerale"
                                                HeaderText="N." DataField="ContatoreGenerale" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                                                FilterControlWidth="100%">
                                                <FilterTemplate>
                                                    <telerik:RadNumericTextBox Width="100%" ID="FiltroContatoreGeneraleTextBox" runat="server"
                                                        MaxLength="9" ClientEvents-OnLoad="OnFiltroContatoreGeneraleTextBoxLoad" DbValue='<%# TryCast(Container, GridItem).OwnerTableView.GetColumn("ContatoreGenerale").CurrentFilterValue %>'
                                                        ClientEvents-OnKeyPress="OnFiltroContatoreGeneraleTextBoxKeyPressed" Skin="Office2007">
                                                        <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                    </telerik:RadNumericTextBox>
                                                    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                        <script type="text/javascript">
                                                            function OnFiltroContatoreGeneraleTextBoxKeyPressed(sender, args) {
                                                                if (13 == args.get_keyCode()) {
                                                                    var tableView = $find("<%# CType(Container, GridItem).OwnerTableView.ClientID %>");
                                                                    var c = sender.get_textBoxValue();
                                                                    tableView.filter("ContatoreGenerale", c, "EqualTo");
                                                                    args.set_cancel(true);
                                                                }
                                                                var text = sender.get_value() + args.get_keyCharacter();
                                                                if (!text.match('^[0-9\b]+$'))
                                                                    args.set_cancel(true);
                                                            }

                                                            // SOVRASCRIVO GLI STILI
                                                            function OnFiltroContatoreGeneraleTextBoxLoad(sender, args) {
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
                                                    <div title='<%# Eval("ContatoreGenerale")%>' style="white-space: nowrap; overflow: hidden;
                                                        text-overflow: ellipsis; width: 55px;">
                                                        <%# Eval("ContatoreGenerale")%>
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <%--<telerik:GridTemplateColumn SortExpression="DescrizioneTipologia" UniqueName="DescrizioneTipologia"
                                                AllowFiltering="false" HeaderText="Tipo" DataField="DescrizioneTipologia" HeaderStyle-Width="100px"
                                                ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <div title='<%# Eval("DescrizioneTipologia")%>' style="white-space: nowrap; overflow: hidden;
                                                        text-overflow: ellipsis; width: 95px;">
                                                        <%# Eval("DescrizioneTipologia")%></div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>

                                            <telerik:GridTemplateColumn SortExpression="DataDocumento" UniqueName="DataDocumento"
                                                HeaderText="Data" DataField="DataDocumento" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                <FilterTemplate>
                                                    <telerik:RadDatePicker ID="DataDocumentoTextBox" Skin="Office2007" ShowPopupOnFocus="true"
                                                        DatePopupButton-Visible="false" Width="100%" runat="server" MinDate="1753-01-01"
                                                        ClientEvents-OnDateSelected="OnDataDocumentoTextBoxDateSelected" DbSelectedDate='<%# DataDocumento(Container) %>'
                                                        DateInput-ClientEvents-OnKeyPress="OnDataDocumentoTextBoxKeyPressed">
                                                        <Calendar  runat="server">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                                        <script type="text/javascript">

                                                            function OnDataDocumentoTextBoxKeyPressed(sender, args) {
                                                                if (13 == args.get_keyCode()) {
                                                                    var tableView = $find("<%# CType(Container, GridItem).OwnerTableView.ClientID %>");
                                                                    var c = sender.get_textBoxValue();

                                                                    var idPicker = sender.get_element().parentNode.parentNode.children(0).id;
                                                                    var picker = $find(idPicker)

                                                                    if (Date.parse(c)) {

                                                                        var ddmmyyyy = c.split('/');
                                                                        var mmddyyyy = ddmmyyyy[1] + '/' + ddmmyyyy[0] + '/' + ddmmyyyy[2];

                                                                        picker.set_selectedDate(new Date(mmddyyyy));
                                                                        // OnDataDocumentoTextBoxDateSelected(picker, "");
                                                                        // picker.hidePopup();
                                                                    } else {
                                                                        picker.set_selectedDate(null);
                                                                        //picker.hidePopup();
                                                                    }
                                                                    args.set_cancel(true);

                                                                }
                                                            }


                                                            function OnDataDocumentoTextBoxDateSelected(sender, args) {
                                                                var tableView = $find("<%# ctype(Container, GridItem).OwnerTableView.ClientID %>");

                                                                var date = FormatSelectedDate(sender);
                                                                var toDate = '';

                                                                try {
                                                                    var dateInput = sender.get_dateInput();
                                                                    var d = sender.get_selectedDate();
                                                                    d.setDate(d.getDate() + 1);
                                                                }
                                                                catch (e) {
                                                                }


                                                                toDate = dateInput.get_dateFormatInfo().FormatDate(d, dateInput.get_displayDateFormat());

                                                                tableView.filter("DataDocumento", date + " " + toDate, "Between");

                                                            }

                                                            function FormatSelectedDate(picker) {
                                                                var date = picker.get_selectedDate();
                                                                var dateInput = picker.get_dateInput();
                                                                var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
                                                                return formattedDate;
                                                            }

                                                                                           

                                                        </script>
                                                    </telerik:RadScriptBlock>
                                                </FilterTemplate>
                                                <ItemTemplate>
                                                    <div title='<%# Eval("DataDocumento", "{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                        overflow: hidden; text-overflow: ellipsis; width: 100%;">
                                                        <%# Eval("DataDocumento", "{0:dd/MM/yyyy}")%></div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>



                                             <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto"
                                                AllowFiltering="True" HeaderText="Oggetto" DataField="Oggetto" AutoPostBackOnFilter="True"
                                                FilterControlWidth="100%" ShowFilterIcon="False">
                                                <ItemTemplate>
                                                    <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                        text-overflow: ellipsis; width:100%;">
                                                        <%# Eval("Oggetto")%></div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>


                                             <telerik:GridTemplateColumn SortExpression="DescrizioneUfficio" UniqueName="DescrizioneUfficio"
                                                AllowFiltering="True" HeaderText="Ufficio" DataField="DescrizioneUfficio" AutoPostBackOnFilter="True"
                                                FilterControlWidth="100%" ShowFilterIcon="False"  HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                <ItemTemplate>
                                                    <div title='<%# Eval("DescrizioneUfficio")%>' style="white-space: nowrap; overflow: hidden;
                                                        text-overflow: ellipsis; width:100%;">
                                                        <%# Eval("DescrizioneUfficio")%></div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>


                                             <telerik:GridTemplateColumn SortExpression="DescrizioneSettore" UniqueName="DescrizioneSettore"
                                                AllowFiltering="True" HeaderText="Settore" DataField="DescrizioneSettore" AutoPostBackOnFilter="True"
                                                FilterControlWidth="100%" ShowFilterIcon="False"  HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                <ItemTemplate>
                                                    <div title='<%# Eval("DescrizioneSettore")%>' style="white-space: nowrap; overflow: hidden;
                                                        text-overflow: ellipsis; width:100%;">
                                                        <%# Eval("DescrizioneSettore")%></div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                         

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="VisualizzaDocumento"
                                                FilterControlAltText="Filter VisualizzaDocumento column" ImageUrl="~\images\Documento16.gif"
                                                UniqueName="VisualizzaDocumento" HeaderStyle-Width="30px" Text="Visualizza Documento..."
                                                ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                                          
                                             <telerik:GridButtonColumn FilterControlAltText="Filter Select column" ImageUrl="~/images/Checks.png"
                                                ItemStyle-Width="30px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" UniqueName="Select" ButtonType="ImageButton"
                                                CommandName="Select" Text="Seleziona Documento" />
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
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8; border-top: 0px solid  #9ABBE8; height: 25px">

                                <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="100px" Skin="Office2007" AutoPostBack="false"
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
