<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FirmaPage.aspx.vb"
    Inherits="FirmaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Modifica Firma</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />


</head>

<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />

     <div id="pageContent">
    
    <center>


      <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server" DecorationZoneID="ZoneID" Skin="Web20"></telerik:RadFormDecorator> 


                  <table width="600px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                   
                                    <%--  HEADER--%>

                                       <tr>
                                        <td  style="background-color: #BFDBFF;padding: 0px; border-bottom:1px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                            &nbsp;<asp:Label 
                                                ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"  
                                                Text="Firma" />
                                        </td>
                                    </tr>

                                      <%-- BODY--%>

                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 230px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <table style="width: 100%">
                                                           
                                                                <tr style="height: 25px">
                                                                    <td style="width: 90px">
                                                                        <asp:Label ID="FirmatarioLabel" runat="server" CssClass="Etichetta" Text="Firmatario" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <telerik:RadComboBox ID="FirmatariComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"
                                                                            MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td style="width: 90px">
                                                                        <asp:Label ID="DataFirmaLabel" runat="server" CssClass="Etichetta" Text="Data Firma" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <telerik:RadDatePicker ID="DataFirmaTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                            MinDate="1753-01-01" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td style="width: 90px">
                                                                        <asp:Label ID="QualificaLabel" runat="server" CssClass="Etichetta" 
                                                                            Text="Qualifica *" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <telerik:RadTextBox ID="QualificaTextBox" runat="server" Skin="Office2007" Width="100%" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td style="width: 90px">
                                                                        <asp:Label ID="ParereLabel" runat="server" CssClass="Etichetta" Text="Parere" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <telerik:RadTextBox ID="ParereTextBox" runat="server" Skin="Office2007" Width="100%"  />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td style="width: 90px">
                                                                        <asp:Label ID="DelegaLabel" runat="server" CssClass="Etichetta" Text="Delega" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                      <div id="ZoneID">
                                                                           <asp:CheckBox ID="DelegaCheckBox" runat="server" AutoPostBack="true" />
                                                                      </div>
                                                                    
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td style="width: 90px">
                                                                        <asp:Label ID="DelegatoLabel" runat="server" CssClass="Etichetta" Text="Delegato" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <telerik:RadComboBox ID="DelegatiComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"
                                                                            MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                                    </td>
                                                                </tr>

                                                                 <tr style="height: 25px">
                                                                    <td style="width: 90px">
                                                                        <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="Qualifica Del." />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <telerik:RadTextBox ID="QualificaDelegatoTextBox" runat="server" Skin="Office2007" Width="100%" />
                                                                    </td>
                                                                </tr>

                                                                <tr style="display: none; height: 25px">
                                                                    <td style="width: 90px">
                                                                        <asp:Label ID="FileFirmatoLabel" runat="server" CssClass="Etichetta" Text="File Firmato" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <asp:Label ID="NomeFileFirmatoLabel" runat="server" CssClass="Etichetta" Text="" />
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
                                    ToolTip="Salva Firma">
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
    </form>
</body>
</html>
