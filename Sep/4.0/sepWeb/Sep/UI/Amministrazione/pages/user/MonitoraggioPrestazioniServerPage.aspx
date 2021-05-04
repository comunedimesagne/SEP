<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="MonitoraggioPrestazioniServerPage.aspx.vb" Inherits="MonitoraggioPrestazioniServerPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

   
   
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
            $get("pageContent").appendChild(_backgroundElement);

        }


        function OnBeginRequest(sender, args) {
            EnableUI(false);
        }



        function OnEndRequest(sender, args) {
            EnableUI(true)
        }

        function EnableUI(state) {
            if (!state) {
                _backgroundElement.style.display = '';
                _backgroundElement.style.position = 'absolute';
                _backgroundElement.style.left = '0px';
                _backgroundElement.style.top = '0px';

                _backgroundElement.style.width = '100%';
                _backgroundElement.style.height = '100%';

                _backgroundElement.style.zIndex = 10000;
                // _backgroundElement.className = "modalBackground";
                _backgroundElement.style.backgroundColor = '#09718F';
                _backgroundElement.style.filter = "alpha(opacity=20)";
                _backgroundElement.style.opacity = "0.2";
            }
            else {
                _backgroundElement.style.display = 'none';

            }
        }


    </script>
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 300px; z-index: 2000000">
                <table cellpadding="4" style="background-color: #4892FF; margin: 0 auto">
                    <tr>
                        <td>
                            <div id="loadingInner" style="width: 300px; text-align: center; background-color: #BFDBFF;
                                height: 60px">
                                <span style="color: #00156E">Attendere prego ... </span>
                                <br />
                                <br />
                                <img alt="" src="../../../../images/loading.gif" border="0">
                                
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>

            <div id="pageContent">
                <center>
                    <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                 
                                    <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 2px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloLabel" Text="Monitoraggio Prestazioni Server" runat="server"
                                                            Font-Bold="True" Style="color: #00156E; background-color: #BFDBFF" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <%-- CONTENT--%>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr style="height: 25px">
                                                                <td style="width: 130px">
                                                                    <telerik:RadButton ID="WriteTimeButton" runat="server" Skin="Office2007" Text="Scrivi"
                                                                        Width="100px">
                                                                        <Icon PrimaryIconLeft="5px" />
                                                                    </telerik:RadButton>
                                                                </td>
                                                                <td style="width: 150px; text-align: right">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Tempo Scrittura:" />&nbsp;&nbsp;
                                                                </td>
                                                                <td style="width: 100px; text-align: right">
                                                                    <asp:Label ID="TempoScritturaLabel" runat="server" CssClass="Etichetta" Text="&nbsp;" />
                                                                </td>
                                                                <td style="width: 90px; text-align: center">
                                                                    <asp:Label ID="NumeroLabel" runat="server" CssClass="Etichetta" Text="Numero" />
                                                                </td>
                                                                <td>
                                                                    <telerik:RadNumericTextBox ID="NumeroTextBox" runat="server" LabelCssClass="" MinValue="1"
                                                                        NumberFormat-DecimalDigits="0" Skin="Office2007" Width="70px">
                                                                    </telerik:RadNumericTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table style="width: 100%">
                                                            <tr style="height: 25px">
                                                                <td style="width: 130px">
                                                                    <telerik:RadButton ID="ReadTimeButton" runat="server" Skin="Office2007" Text="Leggi"
                                                                        Width="100px">
                                                                        <Icon PrimaryIconLeft="5px" />
                                                                    </telerik:RadButton>
                                                                </td>
                                                                <td style="width: 150px; text-align: right">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="Tempo Lettura:" />&nbsp;&nbsp;
                                                                </td>
                                                                <td style="width: 100px; text-align: right">
                                                                    <asp:Label ID="TempoLetturaLabel" runat="server" CssClass="Etichetta" Text="&nbsp;" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table style="width: 100%">
                                                            <tr style="height: 25px">
                                                                <td style="width: 130px">
                                                                    <telerik:RadButton ID="LoadTimeButton" runat="server" Skin="Office2007" Text="Carica"
                                                                        Width="100px">
                                                                        <Icon PrimaryIconLeft="5px" />
                                                                    </telerik:RadButton>
                                                                </td>
                                                                <td style="width: 150px; text-align: right">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="Etichetta" Text="Tempo Caricamento:" />&nbsp;&nbsp;
                                                                </td>
                                                                <td style="width: 100px; text-align: right">
                                                                    <asp:Label ID="TempoCaricamentoLabel" runat="server" CssClass="Etichetta" Text="&nbsp;" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table style="width: 100%">
                                                            <tr style="height: 25px">
                                                                <td style="width: 130px">
                                                                    <telerik:RadButton ID="CancellaButton" runat="server" Skin="Office2007" Text="Cancella"
                                                                        Width="100px">
                                                                        <Icon PrimaryIconLeft="5px" />
                                                                    </telerik:RadButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- INIZIO TABELLA RISULTATI--%>
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                            <%-- HEADER--%>
                                                            <tr>
                                                                <td valign="top">
                                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;<asp:Label ID="PannelloRisultatiLabel" runat="server" Font-Bold="True" Style="width: 400px;
                                                                                    color: #00156E; background-color: #BFDBFF" Text="Risultati" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--  CONTENT--%>
                                                            <tr style="background-color: #DFE8F6">
                                                                <td valign="top">
                                                                    <div style="overflow: auto; height: 100%; width: 100%; background-color: #DFE8F6;
                                                                        border: 0px solid #5D8CC9;">
                                                                        <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <telerik:RadGrid ID="TestGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                                                    Culture="it-IT" PageSize="5">
                                                                                                    <MasterTableView DataKeyNames="IdTest">
                                                                                                        <Columns>
                                                                                                            <telerik:GridBoundColumn DataField="IdTest" DataType="System.Int32" FilterControlAltText="Filter IdTest column"
                                                                                                                HeaderText="IdTest" ReadOnly="True" SortExpression="IdTest" UniqueName="IdTest"
                                                                                                                Visible="False">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoStringa1" FilterControlAltText="Filter CampoStringa1 column"
                                                                                                                HeaderText="CampoStringa1" SortExpression="CampoStringa1" UniqueName="CampoStringa1">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoStringa2" FilterControlAltText="Filter CampoStringa2 column"
                                                                                                                HeaderText="CampoStringa2" SortExpression="CampoStringa2" UniqueName="CampoStringa2">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoStringa3" FilterControlAltText="Filter CampoStringa3 column"
                                                                                                                HeaderText="CampoStringa3" SortExpression="CampoStringa3" UniqueName="CampoStringa3">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoStringa4" FilterControlAltText="Filter CampoStringa4 column"
                                                                                                                HeaderText="CampoStringa4" SortExpression="CampoStringa4" UniqueName="CampoStringa4">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoStringa5" FilterControlAltText="Filter CampoStringa5 column"
                                                                                                                HeaderText="CampoStringa5" SortExpression="CampoStringa5" UniqueName="CampoStringa5">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoInt1" DataType="System.Int32" FilterControlAltText="Filter CampoInt1 column"
                                                                                                                HeaderText="CampoInt1" SortExpression="CampoInt1" UniqueName="CampoInt1">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoInt2" DataType="System.Int32" FilterControlAltText="Filter CampoInt2 column"
                                                                                                                HeaderText="CampoInt2" SortExpression="CampoInt2" UniqueName="CampoInt2">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoInt3" DataType="System.Int32" FilterControlAltText="Filter CampoInt3 column"
                                                                                                                HeaderText="CampoInt3" SortExpression="CampoInt3" UniqueName="CampoInt3">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoInt4" DataType="System.Int32" FilterControlAltText="Filter CampoInt4 column"
                                                                                                                HeaderText="CampoInt4" SortExpression="CampoInt4" UniqueName="CampoInt4">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoInt5" DataType="System.Int32" FilterControlAltText="Filter CampoInt5 column"
                                                                                                                HeaderText="CampoInt5" SortExpression="CampoInt5" UniqueName="CampoInt5">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoData1" DataType="System.DateTime" FilterControlAltText="Filter CampoData1 column"
                                                                                                                HeaderText="CampoData1" SortExpression="CampoData1" UniqueName="CampoData1">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoData2" DataType="System.DateTime" FilterControlAltText="Filter CampoData2 column"
                                                                                                                HeaderText="CampoData2" SortExpression="CampoData2" UniqueName="CampoData2">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CampoData3" DataType="System.DateTime" FilterControlAltText="Filter CampoData3 column"
                                                                                                                HeaderText="CampoData3" SortExpression="CampoData3" UniqueName="CampoData3">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                        </Columns>
                                                                                                    </MasterTableView>
                                                                                                </telerik:RadGrid>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- FINE TABELLA RISULTATI--%>
                                        </td>
                                    </tr>

                                    <%--FOOTER--%>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 20px">
                                        </td>
                                    </tr>


                                   
                                    


                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>


           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
