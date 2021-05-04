<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaStoricoDocumentiFascicoloPage.aspx.vb" Inherits="RicercaStoricoDocumentiFascicoloPage" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ricerca Storico Documenti Fascicolo</title>
       <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />


</head>

<body>

    <form id="form1" runat="server">

    <telerik:RadScriptManager ID="ScriptManager" runat="server" />


     <table style="width:100%; height:100%"  cellpadding="5" cellspacing="5" border="0">
        <tr>
            <td valign="top">
                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width:100%; height:100%">
                    <tr class="ContainerHeader">
                        <td>
                          <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="Elenco Storico Documenti" />
                        </td>
                    </tr>
                    <tr>
                        <td  valign="top" class="ContainerMargin">
                            <table class="Container" cellpadding="0" cellspacing="4" style="width:100%; height:100%" border="0">
                                <tr>
                                  <td valign="top">
                                <div style=" overflow:auto; height:100%">



                                 <telerik:RadGrid ID="StoricoDocumentiFascicoloGridView" runat="server"  ToolTip="Elenco storico dei documenti associati al fascicolo" AutoGenerateColumns="False" CellSpacing="0"  GridLines="None" Skin="Office2007" Width="100%">
                    <MasterTableView DataKeyNames="Id">
                           
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                           
                            <Columns>
                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" 
                                   FilterControlAltText="Filter Id column" HeaderText="Id" 
                                   ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                 <HeaderStyle Width="30px" />
                                 <ItemStyle Width="30px" />
                                 </telerik:GridBoundColumn>

                           <telerik:GridImageColumn FilterControlAltText="Filter DocumentType column" 
                                    ImageHeight="" ImageWidth="" UniqueName="DocumentType"  ImageUrl="../../../../images/Information.png">
                                    <HeaderStyle Width="20px" />
                                 <ItemStyle Width="20px" />
                                </telerik:GridImageColumn>

                                <telerik:GridBoundColumn DataField="NomeDocumento" 
                                    FilterControlAltText="Filter NomeDocumento column" HeaderText="Documento" 
                                    SortExpression="NomeDocumento" UniqueName="NomeDocumento">
                                </telerik:GridBoundColumn>


                              


                            </Columns>
                           

                           
                          </MasterTableView>
                       

                       
                    </telerik:RadGrid>



                                
                                </div>
                                  </td>
                                </tr>
                               
                               <tr class="GridFooter">
                                  <td  colspan="2" align="center">
                                     <telerik:RadButton  ID="ChiudiButton"  runat="server" Text="Chiudi" 
                                           Width="100px" Skin="Office2007">
                                   <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>

                                 
                                   

                                  </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
      
    </table>




  
    </form>
</body>
</html>
