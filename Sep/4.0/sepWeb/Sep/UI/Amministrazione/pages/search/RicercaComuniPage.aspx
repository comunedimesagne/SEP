<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaComuniPage.aspx.vb" Inherits="RicercaComuniPage" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ricerca Comuni</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <!-- custom head section -->
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
      
            function RowSelected(sender, args) {
                //alert(args.getDataKeyValue("Descrizione"));
                window.opener.document.getElementById("ctl00_MainContent_rtxtcmtComuneResidenza").value = args.getDataKeyValue("Descrizione");
                window.opener.document.getElementById("ctl00_MainContent_rtxtcmtProvinciaResidenza").value = args.getDataKeyValue("Provincia");
                window.opener.document.getElementById("ctl00_MainContent_rtxtcmtCAP").value = args.getDataKeyValue("Cap");
                self.close();
                //ctl00_MainContent
            }

            function refreshParent() {
                window.opener.location.href = window.opener.location.href;
                if (window.opener.progressWindow)
                    window.opener.progressWindow.close();
                window.close();
            }

            function resizeWin() {
                var myWidth = rgrdComuni.clientWidth + 55;
                var myHeight = rgrdComuni.clientHeight + 160;

                window.resizeTo(myWidth, myHeight);
            }
        </script>

        </telerik:RadCodeBlock>
    <!-- end of custom head section -->
    
</head>

<body>
    <form id="formComuni" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
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
                                        <div id="scrollPanel" runat="server" style="overflow: auto; height: 100%; width: 100%;
                                            background-color: #FFFFFF; border: 0px solid #5D8CC9;">
                                            <telerik:RadGrid ID="rgrdComuni" runat="server" AllowFilteringByColumn="True" AllowPaging="True"
                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                Width="99.8%" Culture="it-IT">
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="True" />
                                                    <ClientEvents OnGridCreated="resizeWin" />
                                                </ClientSettings>
                                                <MasterTableView ClientDataKeyNames="Codice,Provincia,Descrizione,Toponimo,CAP" DataKeyNames="Codice">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="Codice" DataType="System.Int32" FilterControlAltText="Filter Codice column"
                                                            HeaderText="Codice" ReadOnly="True" SortExpression="Codice" UniqueName="Codice"
                                                            Visible="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                            FilterControlWidth="100%" DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                            HeaderText="Comune" ShowFilterIcon="False" SortExpression="Descrizione" UniqueName="Descrizione">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo"
                                                            FilterControlWidth="100%" HeaderStyle-Width="40px" ItemStyle-Width="40px" DataField="Provincia"
                                                            FilterControlAltText="Filter Provincia column" HeaderText="Prov." ShowFilterIcon="False"
                                                            SortExpression="Provincia" UniqueName="Provincia">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Descrizione2" FilterControlAltText="Filter Descrizione2 column"
                                                            HeaderText="Descrizione2" SortExpression="Descrizione2" UniqueName="Descrizione2"
                                                            Visible="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn AutoPostBackOnFilter="True" DataField="DenominazioneFrazione"
                                                            FilterControlAltText="Filter DenominazioneFrazione column" HeaderText="Frazione"
                                                            ShowFilterIcon="False" SortExpression="DenominazioneFrazione" FilterControlWidth="100%"
                                                            UniqueName="DenominazioneFrazione">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="DenominazioneFrazione2" FilterControlAltText="Filter DenominazioneFrazione2 column"
                                                            HeaderText="DenominazioneFrazione2" SortExpression="DenominazioneFrazione2" UniqueName="DenominazioneFrazione2"
                                                            Visible="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn AllowFiltering="False" DataField="DenUrbGenerica" FilterControlAltText="Filter DenUrbGenerica column"
                                                            HeaderText="Den. Urb." SortExpression="DenUrbGenerica" UniqueName="DenUrbGenerica">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                            FilterControlWidth="100%" DataField="Toponimo" FilterControlAltText="Filter Toponimo column"
                                                            HeaderText="Toponimo" ShowFilterIcon="False" SortExpression="Toponimo" UniqueName="Toponimo">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Toponimo2" FilterControlAltText="Filter Toponimo2 column"
                                                            HeaderText="Toponimo2" SortExpression="Toponimo2" UniqueName="Toponimo2" Visible="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn AutoPostBackOnFilter="True" DataField="NumeriCivici" FilterControlAltText="Filter NumeriCivici column"
                                                            FilterControlWidth="100%" HeaderStyle-Width="60px" ItemStyle-Width="60px" HeaderText="Numeri"
                                                            ShowFilterIcon="False" SortExpression="NumeriCivici" UniqueName="NumeriCivici">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo"
                                                            FilterControlWidth="100%" HeaderStyle-Width="50px" ItemStyle-Width="50px" DataField="CAP"
                                                            FilterControlAltText="Filter CAP column" HeaderText="CAP" ShowFilterIcon="False"
                                                            SortExpression="CAP" UniqueName="CAP">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                            ImageUrl="~/images/action-check.png" UniqueName="Select" FilterControlWidth="20px">
                                                            <ItemStyle Width="20px" />
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
    </form>
</body>
</html>
