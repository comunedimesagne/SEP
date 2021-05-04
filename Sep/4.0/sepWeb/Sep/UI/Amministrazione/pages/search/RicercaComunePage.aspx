<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaComunePage.aspx.vb" Inherits="RicercaComunePage" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script type="text/javascript">

    function GridCreated(sender, args) {
        sender.get_element().style.height = sender.get_element().clientHeight + 1 + "px";
        sender.repaint();
        //alert(rgrdCommittenti.clientHeight);
    }


    function resizeWin() {
        var myWidth = rgrdComune.clientWidth + 55;
        var myHeight = rgrdComune.clientHeight + 160;

        window.resizeTo(myWidth, myHeight);
    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Ricerca Comuni</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
</head>


<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server" />
      


            <table cellpadding="5" cellspacing="5" border="0" style="width: 100%; height: 100%">
            <tr>
                <td>
                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                border-top: 1px solid  #9ABBE8; height: 20px">
                                &nbsp;
                                <asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                    Text="Elenco Comuni" />
                                    
                            </td>
                        </tr>
                        <tr>
                            <td class="ContainerMargin">
                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <div id="scrollPanel" runat="server" style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">


                                                <telerik:RadGrid ID="rgrdComune" runat="server" AllowFilteringByColumn="True" AllowPaging="True"
                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                    Width="99.8%" Culture="it-IT">

                                                    <GroupingSettings CaseSensitive="False" />

                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="True" />
                                                        <ClientEvents OnColumnCreated="resizeWin" />
                                                    </ClientSettings>

                                                    <MasterTableView ClientDataKeyNames="Id,Descrizione,Provincia,Cap" DataKeyNames="Id">
                                                      
                                                        <Columns>

                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn DataField="CodiceIstat" FilterControlAltText="Filter CodiceIstat column"
                                                                HeaderText="CodiceIstat" SortExpression="CodiceIstat" UniqueName="CodiceIstat"
                                                                Visible="False">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" 
                                                                DataField="Descrizione" FilterControlAltText="Filter Descrizione column" FilterControlWidth="100%"
                                                                HeaderText="Descrizione" ShowFilterIcon="False" SortExpression="Descrizione"
                                                                UniqueName="Descrizione">
                                                             </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn DataField="Provincia" FilterControlAltText="Filter Provincia column" AutoPostBackOnFilter="True" HeaderStyle-Width="40px" ItemStyle-Width="40px"
                                                                FilterControlWidth="100%" HeaderText="Prov." SortExpression="Provincia" UniqueName="Provincia"  ShowFilterIcon="False" >
                                                              </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn DataField="Cap" FilterControlAltText="Filter Cap column" AutoPostBackOnFilter="True" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                                                FilterControlWidth="100%" HeaderText="CAP" SortExpression="Cap" UniqueName="Cap"  ShowFilterIcon="False">
                                                             </telerik:GridBoundColumn>

                                                   

                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column" HeaderStyle-Width="20px" ItemStyle-Width="20px"
                                                                ImageUrl="~/images/action-check.png" UniqueName="Select">
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
                                border-top: 1px solid  #9ABBE8; height: 20px">

                         
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>


    </div>
    </form>
</body>
</html>
