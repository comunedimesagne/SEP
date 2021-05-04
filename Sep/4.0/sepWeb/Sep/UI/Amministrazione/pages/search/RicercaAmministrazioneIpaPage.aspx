<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaAmministrazioneIpaPage.aspx.vb"
    Inherits="RicercaAmministrazioneIpaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Amministrazione IPA</title>
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
                                                Text="Elenco Amministrazioni" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                         <td>
                                                                    <div style="overflow: auto; height: 360px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">

                                                                            <telerik:RadGrid ID="AmministrazioniGridView" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                        CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" 
                                                        AllowSorting="True" Culture="it-IT" AllowFilteringByColumn="true">
                                                     
                                                       <GroupingSettings CaseSensitive="false" /> 

                                                        <MasterTableView DataKeyNames="Codice">
                                                           
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="Codice" FilterControlAltText="Filter Codice column"
                                                                    HeaderText="Codice" ReadOnly="True" SortExpression="Codice" UniqueName="Codice"
                                                                    Visible="false" />


                                                                <telerik:GridTemplateColumn SortExpression="Nome" UniqueName="Nome" HeaderText="Descrizione"
                                                                    DataField="Nome" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                    CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                    FilterControlAltText="Filter Nome column" AllowFiltering="true">
                                                                    <ItemTemplate>
                                                                        <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 200px;"
                                                                            title='<%# Eval("Nome")%>'>
                                                                            <%# Eval("Nome")%>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="Sede" UniqueName="Sede" HeaderText="Sede"
                                                                    DataField="Sede" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                    CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                    FilterControlAltText="Filter Sede column" AllowFiltering="true">
                                                                    <ItemTemplate>
                                                                        <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 200px;"
                                                                            title='<%# Eval("Sede")%>'>
                                                                            <%# Eval("Sede")%>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>


                                                                <telerik:GridTemplateColumn SortExpression="Email" UniqueName="Email" HeaderText="E-mail"
                                                                    DataField="Email" HeaderStyle-Width="200px" ItemStyle-Width="200px" AutoPostBackOnFilter="True"
                                                                    CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                    FilterControlAltText="Filter Email column" AllowFiltering="true">
                                                                    <ItemTemplate>
                                                                        <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 200px;"
                                                                            title='<%# Eval("Email")%>'>
                                                                            <%# Eval("Email")%>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                               

                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                                    ImageUrl="~/images/Checks.png" UniqueName="Select" HeaderStyle-Width="20px" ItemStyle-Width="20px"
                                                                    ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                </telerik:GridButtonColumn>

                                                             <%--    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px" Text="Seleziona Bozza"
                                                        ItemStyle-Width="20px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png"
                                                        UniqueName="Select" />--%>

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
