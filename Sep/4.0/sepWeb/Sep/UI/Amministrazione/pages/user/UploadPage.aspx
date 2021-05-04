<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UploadPage.aspx.vb" Inherits="UploadPage" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seleziona File</title>
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
                         <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="Upload File" />
                        </td>
                    </tr>
                    <tr>
                        <td  valign="top" class="ContainerMargin">
                            <table class="Container" cellpadding="0" cellspacing="4" style="width:100%; height:100%" border="0">
                                <tr>
                                  <td valign="top" align="center">
                              
                               <telerik:RadAsyncUpload ID="AllegatoUpload" runat="server" MaxFileInputsCount="1"
                                        Skin="Office2007" Width="500px" InputSize="70">
                                        <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                    </telerik:RadAsyncUpload>
                              
                               
                                  </td>
                                </tr>
                               
                               <tr class="GridFooter">
                                  <td  colspan="2" align="center">
                                     <telerik:RadButton  ID="ConfermaButton"  runat="server" Text="Conferma" 
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
