<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VerificaPasswordPage.aspx.vb" Inherits="VerificaPasswordPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link type="text/css" href="../../../../Styles/StylesA.css" rel="stylesheet" />
    <style type="text/css">
        .style1
        {
            width: 141px;
        }
        .style2
        {
            height: 27px;
            width: 142px;
        }
        .style3
        {
            width: 142px;
        }
    </style>
</head>
<script type="text/javascript">
//    function GetRadWindow() {
//        var oWindow = null;
//        if (window.radWindow) oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog    
//        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz az well)    
//        return oWindow;
//    }

//    function Close(codicePratica) {
//        var oWnd = GetRadWindow();
//        //Chiudo la finestra e passo il codice alla pagina chiamante
//        if (codicePratica) {
//            oWnd.close(codicePratica);
//        }
//        else {
//            alert("Errore durante l'inserimento della password.");
//        }
//    } 
</script>
<body>
    <form id="form1" runat="server">
    <div>
   <telerik:RadScriptManager ID="ScriptManager" runat="server" />


       <table style="width:100%; height:100%"  cellpadding="5" cellspacing="5" border="0">
        <tr>
            <td valign="top">
                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width:100%; height:100%">
                    <tr class="ContainerHeader">
                        <td>
                         <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Font-Size="12px" ForeColor="#00156E" Font-Bold="True" Text="Inserire la password:" />
                        </td>
                    </tr>
                    <tr>
                        <td  valign="top" class="ContainerMargin">
                            <table class="Container" cellpadding="0" cellspacing="4" style="width:100%; height:100%" border="0">
                                <tr>
                                  <td valign="top">
                                <div style="height:100%">

     <table style="width:100%;">
                    <tr>
                        <td colspan="3" style="text-align:center">
                            <telerik:RadTextBox ID="rtxtPassword" ToolTip="Password case sensitive" runat="server" TextMode="Password"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align:center">
                            <asp:PlaceHolder ID="ScriptHolder" runat="server"/>
                        </td>
                    </tr>
                </table>       
    
                                </div>
                                  </td>
                                </tr>
                               <tr class="GridFooter">
                                    <td  colspan="2" align="center">
                                        <telerik:RadButton ID="rbtnInserisciPass" runat="server" Skin="Office2007" Text="Ok">
                                        <Icon PrimaryIconUrl="~/images/action-check.png" />
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
    </div>
    </form>
</body>
</html>
