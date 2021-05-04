<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaBeneficiarioPage.aspx.vb"
    Inherits="RicercaBeneficiarioPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Beneficiari</title>
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
                                        <td  style="background-color: #BFDBFF;padding: 4px; border-bottom:1px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" 
                                                runat="server" Style="color: #00156E" Font-Bold="True"  
                                                Text="Elenco Beneficiari" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                         <td>
                                                                    <div style="overflow: auto; height: 360px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">
                                                                              <telerik:RadGrid ID="BeneficiariGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT"  AllowFilteringByColumn="True">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                           

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />


                                                        




                                                    <telerik:GridTemplateColumn HeaderStyle-Width="380px" ItemStyle-Width="380px" DataField="Nominativo"
                                                        FilterControlAltText="Filter Nominativo column" HeaderText="Nominativo" SortExpression="Nominativo"
                                                         AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" 
                                                         FilterControlWidth="100%" ShowFilterIcon="False"
                                                        UniqueName="Nominativo">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 380px; border: 1px solid red">
                                                                <%# Eval("Nominativo")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                     <telerik:GridTemplateColumn HeaderStyle-Width="150px" ItemStyle-Width="150px" DataField="CFPIVA"
                                                        FilterControlAltText="Filter CFPIVA column" HeaderText="Cod. Fisc./Partita Iva" SortExpression="CFPIVA"
                                                         AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" 
                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                        UniqueName="CFPIVA">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("CFPIVA")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 150px;border: 1px solid red">
                                                                <%# Eval("CFPIVA")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn HeaderStyle-Width="250px" ItemStyle-Width="250px" DataField="IBAN"
                                                        FilterControlAltText="Filter IBAN column" HeaderText="IBAN" SortExpression="IBAN"
                                                         AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" 
                                                    FilterControlWidth="100%" ShowFilterIcon="False"
                                                        UniqueName="IBAN">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("IBAN")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 250px;border: 1px solid red">
                                                                <%# Eval("IBAN")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                


                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px" Text="Seleziona Bozza"
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
