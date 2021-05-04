<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OrdineGiornoPage.aspx.vb"
    Inherits="OrdineGiornoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>


<body>
    <form id="form2" runat="server">
 

       <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
      <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
      </telerik:RadWindowManager>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Sunset" />
    <center>

     <div id="pageContent">
    
    <center>

        <table width="800px" cellpadding="5" cellspacing="5" border="0">
            <tr>
                <td>
                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                        <%--  HEADER--%>

                        <tr>

                         <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 25px">
                        <table style=" width:100%">
                        <tr>
    <td>
                                &nbsp;<asp:Label ID="TitleLabel" 
                                    runat="server" Style="color: #00156E" Font-Bold="True"
                                    Text="Nuovo Argomento Ordine del Giorno" />
                                  
                        
                           
                                   
                            </td>
                            <td align="right">
                               <asp:Label ID="InfoPropostaLabel"  runat="server" Style="color: #00156E" Font-Bold="True" Text=""   />
                            </td>
                        </tr>
                        </table>
                        </td>

                        
                        </tr>


                       <%-- BODY--%>

                        <tr>
                            <td class="ContainerMargin">
                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF; border: 0px solid #5D8CC9;">

                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 80px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="NumeroLabel" runat="server" CssClass="Etichetta" Text="Numero" />
                                                        </td>
                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                            <telerik:RadTextBox ID="NumeroTextBox" runat="server" Skin="Office2007" Width="60px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 80px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="UfficioLabel" runat="server" CssClass="Etichetta" Text="Ufficio" />
                                                        </td>
                                                        <td>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <telerik:RadTextBox ID="UfficioTextBox" runat="server" Skin="Office2007" Width="100%" />
                                                                       
                                                                     </td>
                                                                    <td align="right" style=" width:24px">
                                                                        <asp:ImageButton ID="TrovaUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            ToolTip="Seleziona ufficio..." ImageAlign="AbsMiddle" />
                                                                    </td>
                                                                     <td align="right" style=" width:24px">
                                                                        <asp:ImageButton ID="EliminaUfficioImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                            ToolTip="Cancella utente" ImageAlign="AbsMiddle" />
                                                                        <asp:ImageButton ID="AggiornaUfficioImageButton" runat="server" Style="display: none" />
                                                                        <asp:TextBox ID="IdUfficioTextBox" runat="server" Style="display: none" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                          
                                                           
                                                         
                                                          
                                                         
                                                         
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 80px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="SettoreLabel" runat="server" CssClass="Etichetta" Text="Settore" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="SettoreTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                ReadOnly="True" />
                                                            <asp:TextBox ID="IdSettoreTextBox" runat="server" Style="display: none" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 80px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                Rows="3" TextMode="MultiLine" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 80px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="NoteLabel" runat="server" CssClass="Etichetta" Text="Note" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                Rows="3" TextMode="MultiLine" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 80px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="StatoLabel" runat="server" CssClass="Etichetta" Text="Stato" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="StatoDiscussioneComboBox" AutoPostBack="false" runat="server"
                                                                EmptyMessage="- Seleziona Stato -" MaxHeight="150px" Skin="Office2007" Width="300px" />
                                                        </td>
                                                    </tr>
                                                </table>

                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                       <%-- FOOTER--%>

                        <tr>
                            <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                border-top: 1px solid  #9ABBE8; height: 25px">
                                <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="90px" Skin="Office2007"
                                    ToolTip="Salva Seduta">
                                    <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                                &nbsp;
                                <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="90px"
                                    Skin="Office2007" ToolTip="Cancella i dati immessi">
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

 </center>

    </form>
</body>
</html>

