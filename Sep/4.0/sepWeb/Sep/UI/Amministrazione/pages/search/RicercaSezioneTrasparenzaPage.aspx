<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaSezioneTrasparenzaPage.aspx.vb" Inherits="RicercaSezioneTrasparenzaPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ricerca Sezione</title>
      <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server">



    <telerik:RadScriptManager ID="ScriptManager" runat="server" />






     <center>

                  <table cellpadding="5" cellspacing="5" border="0" style=" width:900px; height:600px">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                   


                                       <tr>
                                        <td  style="background-color: #BFDBFF;padding: 4px; border-bottom:1px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                            &nbsp;<asp:Label ID="TitleLabel" 
                                                runat="server" Style="color: #00156E" Font-Bold="True"  Text="Elenco Sezioni Trasparenza" />
                                        </td>



                                    </tr>
                                    <tr>
                                    <td>
                                      <table style="width:100%">
                            <tr>
                              <td style="width:40px"><asp:Label ID="FiltroLabel" runat="server" CssClass="Etichetta" Text="Filtro" /></td>
                              <td style="width:270px"><telerik:RadTextBox ID="FiltroTextBox" Runat="server" Skin="Office2007" Width="250px" /></td>
                              <td><asp:ImageButton ID="FiltroImageButton" runat="server" ImageUrl="~/images//filter16.png" ToolTip="Applica filtro" /></td>
                            </tr>
                             </table>
                             </td>
                                    </tr>

                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                         <td>
                                                                    <div style="overflow: auto; height:550px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">


                                                                        <telerik:RadTreeView ID="SezioniTrasparenzaTreeView" runat="server" Skin="Office2007" OnClientNodeClicking="OnClientNodeClicking"  />
                                                                       
                                                                        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                                            <script type="text/javascript" language="javascript">
                                                                                function OnClientNodeClicking(sender, args) {
                                                                                    var node = args.get_node();
                                                                                    var attr = node.get_attributes().getAttribute("nonselezionabile");

                                                                                    if (attr == '') {
                                                                                        args.set_cancel(true);
                                                                                        return false;
                                                                                    }
                                                                                }

                                                                            </script>
                                                                        </telerik:RadCodeBlock>
                                                                          
                                                                    </div>
                                                                </td>
                                                </tr>
                                               
                                              
                                            </table>
                                        </td>
                                    </tr>

                                 

                                          <tr>
                                                        <td align="center"  style="background-color: #BFDBFF;padding: 4px; border-bottom:0px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                           <telerik:RadButton ID="ConfermaButton" runat="server" Text="Conferma" Width="100px" Skin="Office2007"
                                                            ToolTip="Conferma selezione">
                                                            <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>

                                                     &nbsp;
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
