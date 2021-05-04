<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaModelloDocumentoPage.aspx.vb"
    Inherits="RicercaModelloDocumentoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Modello Documento</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <center>


       



          <table cellpadding="5" cellspacing="5" border="0" style="width: 800px; height: 500px">
            <tr>
                <td>
                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                border-top: 1px solid  #9ABBE8; height: 25px">
                                &nbsp;<asp:Label ID="ElencoModelliDocumentoLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                    Text="Elenco Modelli Documento" />

                              
                            </td>
                        </tr>
                        <tr>
                            <td class="ContainerMargin">
                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <div id="Div1" runat="server" style="overflow: auto; height: 500px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                  <telerik:RadGrid ID="ModelliDocumentoGridView" runat="server" AllowPaging="True"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="99.8%" AllowSorting="True" Culture="it-IT" >
                                                                <MasterTableView DataKeyNames="Id">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Nome" UniqueName="Nome" HeaderText="Nome"
                                                                            DataField="Nome" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Nome")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 200px;">
                                                                                    <%# Eval("Nome")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Creatore" UniqueName="Creatore" HeaderText="Utente"
                                                                            DataField="Creatore" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Creatore")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 200px;">
                                                                                    <%# Eval("Creatore")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="DataInizioValidita" UniqueName="DataInizioValidita"
                                                                            HeaderText="Valido dal" DataField="DataInizioValidita" HeaderStyle-Width="70px"
                                                                            ItemStyle-Width="70px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DataInizioValidita","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                                    <%# Eval("DataInizioValidita", "{0:dd/MM/yyyy}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="DataFineValidita" UniqueName="DataFineValidita"
                                                                            HeaderText="Valido al" DataField="DataFineValidita" HeaderStyle-Width="70px"
                                                                            ItemStyle-Width="70px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DataFineValidita","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                                    <%# Eval("DataFineValidita", "{0:dd/MM/yyyy}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                                            ImageUrl="~\images\checks.png" UniqueName="Select">
                                                                            <HeaderStyle Width="10px" />
                                                                            <ItemStyle Width="10px" />
                                                                        </telerik:GridButtonColumn>
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
                            <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                border-top: 1px solid  #9ABBE8; height: 25px">
                                
                                <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="100px" Skin="Office2007"
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
    </form>
</body>
</html>
