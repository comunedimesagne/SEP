<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaImpegnoSpesaPage.aspx.vb"
    Inherits="RicercaImpegnoSpesaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Impegno Spesa</title>
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
                                        Text="Elenco Impegni Spesa" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ContainerMargin">
                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <div style="overflow: auto; height: 400px; width: 100%; background-color: #FFFFFF;
                                                    border: 0px solid #5D8CC9;">


                                               

                                                         <telerik:RadGrid ID="ImpegniSpesaGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False" ToolTip="Elenco impegni di spesa"
                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT"  AllowFilteringByColumn="True">


                                                        <MasterTableView DataKeyNames="Id, IdDocumento">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />


                                                  


                                                                <telerik:GridTemplateColumn SortExpression="AnnoEsercizio" UniqueName="AnnoEsercizio"
                                                                    HeaderText="Anno" DataField="AnnoEsercizio" HeaderStyle-Width="40px" ItemStyle-Width="40px"
                                                                    AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" 
                                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                                     FilterControlAltText="Filter AnnoEsercizio column" AllowFiltering="true">
                                                                    <FilterTemplate>
                                                                        <telerik:RadNumericTextBox ID="AnnoEsercizioTextBox"  runat="server" Width="100%" 
                                                                         DbValue='<%# CType(Container,GridItem).OwnerTableView.GetColumn("AnnoEsercizio").CurrentFilterValue %>'
                                                                         ClientEvents-OnLoad="OnAnnoEsercizioTextBoxLoad"   ClientEvents-OnKeyPress="OnAnnoEsercizioTextBoxKeyPressed" Skin="Office2007">
                                                                        <NumberFormat GroupSeparator="" DecimalDigits="0" /> 
                                                                         </telerik:RadNumericTextBox> 
                                                                     
                                                                        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                                            <script type="text/javascript">
                                                                                function OnAnnoEsercizioTextBoxKeyPressed(sender, args) {
                                                                                    if (13 == args.get_keyCode()) {
                                                                                        var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                        var c = sender.get_textBoxValue();
                                                                                        tableView.filter("AnnoEsercizio", c, "EqualTo");
                                                                                        // args.get_domEvent().preventDefault();
                                                                                         //args.get_domEvent().stopPropagation();  

                                                                                        args.set_cancel(true);
                                                                                        
                                                                                    }
                                                                                }

                                                                                // SOVRASCRIVO GLI STILI
                                                                                function OnAnnoEsercizioTextBoxLoad(sender, args) {
                                                                                    // sender.get_styles().EnabledStyle[0]  += "background-color: Yellow";
                                                                                    sender.get_styles().HoveredStyle[0] = "";  // += "background-color: Yellow";
                                                                                    sender.get_styles().HoveredStyle[1] = "";
                                                                                    sender.get_styles().FocusedStyle[0]= ""; // += "background-color: Yellow";
                                                                                    sender.get_styles().FocusedStyle[1] = "";
                                                                                    sender.updateCssClass();
                                                                                 }
                                                                            </script>
                                                                        </telerik:RadScriptBlock>
                                                                    </FilterTemplate>
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("AnnoEsercizio")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                            <%# Eval("AnnoEsercizio")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>



                                                        


                                                              

                                                                <telerik:GridTemplateColumn SortExpression="NumeroImpegno" UniqueName="NumeroImpegno"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderText="Impegno" DataField="NumeroImpegno"
                                                                    HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"
                                                                       AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" 
                                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                                     FilterControlAltText="Filter NumeroImpegno column">


                                                                      <FilterTemplate>

                                                                          <telerik:RadNumericTextBox ID="NumeroImpegnoTextBox" runat="server" Width="100%"
                                                                              DbValue='<%# CType(Container,GridItem).OwnerTableView.GetColumn("NumeroImpegno").CurrentFilterValue %>'
                                                                              ClientEvents-OnLoad="OnNumeroImpegnoTextBoxLoad" ClientEvents-OnKeyPress="OnNumeroImpegnoTextBoxKeyPressed"
                                                                              Skin="Office2007">
                                                                              <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                          </telerik:RadNumericTextBox>
                                                                     
                                                                        <telerik:RadScriptBlock ID="RadScriptBlock5" runat="server">
                                                                            <script type="text/javascript">
                                                                                function OnNumeroImpegnoTextBoxKeyPressed(sender, args) {
                                                                                    if (13 == args.get_keyCode()) {
                                                                                        var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                        var c = sender.get_textBoxValue();
                                                                                        tableView.filter("NumeroImpegno", c, "EqualTo");
                                                                                        args.set_cancel(true);
                                                                                    }


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
                                                                            text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                            <%# Eval("NumeroImpegno")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="NumeroSubImpegno" UniqueName="NumeroSubImpegno"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderText="Sub Impegno" DataField="NumeroSubImpegno"
                                                                    HeaderStyle-Width="80px" ItemStyle-Width="80px"
                                                                       AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" 
                                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                                     FilterControlAltText="Filter NumeroSubImpegno column">


                                                                      <FilterTemplate>
                                                                          <telerik:RadNumericTextBox ID="NumeroSubImpegnoTextBox" runat="server" Width="100%"
                                                                              DbValue='<%# CType(Container,GridItem).OwnerTableView.GetColumn("NumeroSubImpegno").CurrentFilterValue %>'
                                                                              ClientEvents-OnLoad="OnNumeroSubImpegnoTextBoxLoad" ClientEvents-OnKeyPress="OnNumeroSubImpegnoTextBoxKeyPressed"
                                                                              Skin="Office2007">
                                                                              <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                          </telerik:RadNumericTextBox>

                                                                          <telerik:RadScriptBlock ID="RadScriptBlock6" runat="server">
                                                                            <script type="text/javascript">
                                                                                function OnNumeroSubImpegnoTextBoxKeyPressed(sender, args) {
                                                                                    if (13 == args.get_keyCode()) {
                                                                                        var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                        var c = sender.get_textBoxValue();
                                                                                        tableView.filter("NumeroSubImpegno", c, "EqualTo");
                                                                                        args.set_cancel(true);
                                                                                    }

                                                                                  


                                                                                }

                                                                                // SOVRASCRIVO GLI STILI
                                                                                function OnNumeroSubImpegnoTextBoxLoad(sender, args) {
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
                                                                        <div title='<%# Eval("NumeroSubImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                            <%# Eval("NumeroSubImpegno")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>


                                                             <telerik:GridTemplateColumn SortExpression="Importo" UniqueName="Importo" FooterStyle-HorizontalAlign="Right"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderText="Importo" DataField="Importo"
                                                                    HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right"
                                                                       AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" 
                                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                                     FilterControlAltText="Filter Importo column">


                                                                      <FilterTemplate>
                                                                        <telerik:RadNumericTextBox ID="ImportoTextBox"  runat="server" Width="100%"
                                                                          DbValue='<%# CType(Container,GridItem).OwnerTableView.GetColumn("Importo").CurrentFilterValue %>'
                                                                       ClientEvents-OnLoad="OnImportoTextBoxLoad"   ClientEvents-OnKeyPress="OnImportoTextBoxKeyPressed" Skin="Office2007" />
                                                                     
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
                                                                            text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                            <%# Eval("Importo", "{0:N2}")%></div>
                                                                    </ItemTemplate>


                                                                </telerik:GridTemplateColumn>

                                                                   <telerik:GridTemplateColumn SortExpression="Settore" UniqueName="Settore" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderText="Settore" DataField="Settore" HeaderStyle-Width="160px" ItemStyle-Width="160px"
                                                                       AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" 
                                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                                     FilterControlAltText="Filter Settore column">
                                                                  
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("Settore")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                            width: 160px; border: 0px solid red">
                                                                            <%# Eval("Settore")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>


                                                                 



                                                                 <telerik:GridTemplateColumn SortExpression="ContatoreGenerale" UniqueName="ContatoreGenerale" FooterStyle-HorizontalAlign="Right"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderText="N. Det" DataField="ContatoreGenerale"
                                                                    HeaderStyle-Width="55px" ItemStyle-Width="55px" ItemStyle-HorizontalAlign="Right"
                                                                       AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" 
                                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                                     FilterControlAltText="Filter ContatoreGenerale column">


                                                                  
                                                                      <FilterTemplate>

                                                                       
                                                                          <telerik:RadNumericTextBox ID="ContatoreGeneraleTextBox" runat="server" Width="100%" 
                                                                              DbValue='<%# CType(Container,GridItem).OwnerTableView.GetColumn("ContatoreGenerale").CurrentFilterValue %>'
                                                                              ClientEvents-OnLoad="OnContatoreGeneraleTextBoxLoad" ClientEvents-OnKeyPress="OnContatoreGeneraleTextBoxKeyPressed"
                                                                              Skin="Office2007">
                                                                              <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                          </telerik:RadNumericTextBox>


                                                           
                                                                     
                                                                        <telerik:RadScriptBlock ID="RadScriptBlock7" runat="server">
                                                                            <script type="text/javascript">
                                                                                function OnContatoreGeneraleTextBoxKeyPressed(sender, args) {
                                                                                    if (13 == args.get_keyCode()) {
                                                                                        var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                        var c = sender.get_textBoxValue();
                                                                                        tableView.filter("ContatoreGenerale", c, "EqualTo");
                                                                                        args.set_cancel(true);
                                                                                    }
                                                                                }

                                                                                // SOVRASCRIVO GLI STILI
                                                                                function OnContatoreGeneraleTextBoxLoad(sender, args) {
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
                                                                        <div title='<%# Eval("ContatoreGenerale")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                            width: 55px; border: 0px solid red">
                                                                            <%# Eval("ContatoreGenerale")%></div>
                                                                    </ItemTemplate>


                                                                </telerik:GridTemplateColumn>




                                                                 <telerik:GridTemplateColumn SortExpression="DataDocumento" UniqueName="DataDocumento"
                                                                            HeaderText="Data" DataField="DataDocumento" HeaderStyle-Width="70px"
                                                                            ItemStyle-Width="70px" AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" 
                                                                            FilterControlWidth="100%" ShowFilterIcon="false" FilterControlAltText="Filter DataDocumento column"
                                                                            AllowFiltering="true" >
                                                                               <FilterTemplate>

                                                                                <telerik:RadDatePicker ID="DataDocumentoTextBox" Skin="Office2007" ShowPopupOnFocus="true" DatePopupButton-Visible="false"
                                                                                    Width="100%" runat="server" MinDate="1753-01-01" ClientEvents-OnDateSelected="DateSelected"
                                                                                    DbSelectedDate='<%# CType(Container,GridItem).OwnerTableView.GetColumn("DataDocumento").CurrentFilterValue %>'
                                                                                    DateInput-ClientEvents-OnKeyPress="OnDataDocumentoTextBoxKeyPressed"
                                                                                      />

                                                                                   <telerik:RadScriptBlock ID="RadScriptBlock8" runat="server">
                                                                                      
                                                                                       <script type="text/javascript">

                                                                                           function OnDataDocumentoTextBoxKeyPressed(sender, args) {
                                                                                               if (13 == args.get_keyCode()) {
                                                                                                   var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                   var c = sender.get_textBoxValue();
                                                                                                   if (Date.parse(c)) {
                                                                                                       // tableView.filter("DataDocumento", c, "EqualTo");

                                                                                                       var idPicker = sender.get_element().parentNode.parentNode.children(0).id;
                                                                                                       var picker = $find(idPicker)
                                                                                                       var ddmmyyyy = c.split('/');
                                                                                                       var mmddyyyy = ddmmyyyy[1] + '/' + ddmmyyyy[0] + '/' + ddmmyyyy[2];

                                                                                                       picker.set_selectedDate(new Date(mmddyyyy));
                                                                                                       DateSelected(picker, "");
                                                                                                   } 

                                                                                                   args.set_cancel(true);
                                                                                               }
                                                                                           }


                                                                                           function DateSelected(sender, args) {
                                                                                               var tableView = $find("<%# ctype(Container,GridItem).OwnerTableView.ClientID %>");

                                                                                               var date = FormatSelectedDate(sender);

                                                                                               tableView.filter("DataDocumento", date, "EqualTo");
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
                                                                                <div title='<%# Eval("DataDocumento","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; text-overflow: ellipsis; width:70px; border: 0px solid red">
                                                                                    <%# Eval("DataDocumento", "{0:dd/MM/yyyy}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>





                                              


                                                                   <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderText="Oggetto" DataField="Oggetto" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                                                       AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" 
                                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                                     FilterControlAltText="Filter Oggetto column">
                                                                  
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                            width: 150px; border: 0px solid red">
                                                                            <%# Eval("Oggetto")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="20px"  Text="Visualizza Determina..."
                                                                        ItemStyle-Width="20px">
                                                                    </telerik:GridButtonColumn>

                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                    Text="Seleziona Impegno Spesa" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                    ImageUrl="~\images\checks.png" UniqueName="Select" />
                                                            </Columns>
                                                        </MasterTableView></telerik:RadGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                              <tr>
                                        <td align="center"  style="background-color: #BFDBFF;padding: 4px; border-bottom:0px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
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
