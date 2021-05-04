<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaSedutaPage.aspx.vb"
    Inherits="RicercaSedutaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Seduta</title>
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
                                        <td  style="background-color: #BFDBFF;padding: 4px; border-bottom:1px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                            &nbsp;<asp:Label 
                                                ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"  
                                                Text="Elenco Sedute" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                         <td>
                                                                    <div style="overflow: auto; height: 335px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">
                                                                              <telerik:RadGrid ID="SeduteGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT"  AllowFilteringByColumn="True">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                           

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />

                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200px" ItemStyle-Width="200px" DataField="DescrizioneTipologiaSeduta"
                                                        FilterControlAltText="Filter DescrizioneTipologiaSeduta column" HeaderText="Organo" SortExpression="DescrizioneTipologiaSeduta"
                                                         AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" 
                                                         FilterControlWidth="100%" ShowFilterIcon="False"
                                                        UniqueName="DescrizioneTipologiaSeduta">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DescrizioneTipologiaSeduta")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 200px; border: 0px solid red">
                                                                <%# Eval("DescrizioneTipologiaSeduta")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200px" ItemStyle-Width="200px" DataField="DescrizioneTipologiaConvocazione"
                                                        FilterControlAltText="Filter DescrizioneTipologiaConvocazione column" HeaderText="Sessione" SortExpression="DescrizioneTipologiaConvocazione"
                                                         AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" 
                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                        UniqueName="DescrizioneTipologiaConvocazione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DescrizioneTipologiaConvocazione")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 200px;border: 0px solid red">
                                                                <%# Eval("DescrizioneTipologiaConvocazione")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                       <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="DataConvocazione"
                                                       HeaderText="Data" SortExpression="DataConvocazione" AllowFiltering="false"
                                                         UniqueName="DataConvocazione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DataConvocazione", "{0:dd/MM/yyyy}") %>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 70px;border: 0px solid red">
                                                                <%# Eval("DataConvocazione", "{0:dd/MM/yyyy}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                     <telerik:GridTemplateColumn HeaderStyle-Width="95px" ItemStyle-Width="95px" DataField="DataPrimaConvocazione"
                                                       HeaderText="Data 1^ Conv." SortExpression="DataPrimaConvocazione" AllowFiltering="false"
                                                         UniqueName="DataPrimaConvocazione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DataPrimaConvocazione", "{0:dd/MM/yyyy}")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 95px;border: 0px solid red">
                                                                <%# Eval("DataPrimaConvocazione", "{0:dd/MM/yyyy}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    
                                                     <telerik:GridTemplateColumn HeaderStyle-Width="95px" ItemStyle-Width="95px" DataField="DataSecondaConvocazione"
                                                       HeaderText="Data 2^ Conv." SortExpression="DataSecondaConvocazione" AllowFiltering="false"
                                                         UniqueName="DataSecondaConvocazione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DataSecondaConvocazione", "{0:dd/MM/yyyy}")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 95px;border: 0px solid red">
                                                                <%# Eval("DataSecondaConvocazione", "{0:dd/MM/yyyy}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                  


                                                


                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px" Text="Seleziona Seduta"
                                                        ItemStyle-Width="20px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png"
                                                        UniqueName="Select" />
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                                                    </div>
                                                                </td>
                                                </tr>
                                              
                                              
                                            </table>
                                        </td>
                                    </tr>

                                       <%--   <tr>
                                        <td align="center"  style="background-color: #BFDBFF;padding: 4px; border-bottom:0px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                           <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="80px" Skin="Office2007"
                                                            ToolTip="Aggiorna contatori">
                                                            <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                        </td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>
   
              
    </center>

 </div>
    </form>
</body>
</html>
