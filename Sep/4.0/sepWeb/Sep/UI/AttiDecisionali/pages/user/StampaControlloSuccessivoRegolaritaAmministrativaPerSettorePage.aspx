<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="StampaControlloSuccessivoRegolaritaAmministrativaPerSettorePage.aspx.vb" Inherits="StampaControlloSuccessivoRegolaritaAmministrativaPerSettorePage" %>



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
            EnableUI(true);
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


            <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center; top: 300px; z-index: 2000000">
                <table cellpadding="4" style="background-color: #4892FF; margin: 0 auto">
                    <tr>
                        <td>
                            <div id="loadingInner" style="width: 300px; text-align: center; background-color: #BFDBFF; height: 60px">
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





                    <table width="600px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">

                                    <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; height: 25px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>&nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                        Text="Stampa Controllo Regolarità Amministrativa" />
                                                    </td>

                                                    <td style="width: 30px; text-align: center">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Effettua la ricerca con i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>

                                                    <td style="width: 30px; text-align: center">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>

                                            </table>

                                        </td>

                                    </tr>

                                    <%-- CONTENT--%>
                                    <tr>
                                        <td class="ContainerMargin">

                                            <div id="ControlliRegolaritaAmministrativaPanel" runat="server" style="padding: 2px 0px 0px 0px;">

                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">


                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" width="100%" border="0"
                                                                style="background-color: #BFDBFF">
                                                                <tr>
                                                                    <td style="background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 130px">
                                                                                    <asp:Label ID="DataLabel" runat="server" CssClass="Etichetta" Text="Data Estrazione" ForeColor="#00156E"
                                                                                        Font-Bold="True" />
                                                                                </td>
                                                                                <td style="width: 30px">
                                                                                    <asp:Label ID="DataInizioLabel" runat="server" CssClass="Etichetta" Text="da" ForeColor="#00156E" />
                                                                                </td>
                                                                                <td style="width: 130px">
                                                                                    <telerik:RadDatePicker ID="DataInizioTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                                        MinDate="1753-01-01" />
                                                                                </td>
                                                                                <td style="width: 20px">
                                                                                    <asp:Label ID="DataAFineLabel" runat="server" CssClass="Etichetta" Text="a" ForeColor="#00156E" />
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadDatePicker ID="DataFineTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                                        MinDate="1753-01-01" />
                                                                                </td>
                                                                            </tr>

                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                    </tr>




                                                </table>

                                            </div>

                                            <div id="GrigliaControlliRegolaritaAmministrativaPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                    <tr>
                                                        <td style="height: 20px">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>&nbsp;<asp:Label ID="ControlliRegolaritaAmministrativaLabel" runat="server" Font-Bold="True" Style="width: 500px; color: #00156E; background-color: #BFDBFF"
                                                                        Text="Controlli Regolarità Amministrativa" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="background-color: #FFFFFF">
                                                        <td>
                                                            <div id="scrollPanelUtenti" runat="server" style="overflow: auto; height: 315px; border: 1px solid #5D8CC9">

                                                                <telerik:RadGrid ID="ControlliRegolaritaAmministrativaGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" PageSize="10"
                                                                    Culture="it-IT">

                                                                    <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                        <Columns>

                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />


                                                                            <telerik:GridTemplateColumn DataField="PeriodoRiferimento"
                                                                                FilterControlAltText="Filter PeriodoRiferimento column" HeaderText="Periodo Riferimento" SortExpression="PeriodoRiferimento"
                                                                                UniqueName="PeriodoRiferimento">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("PeriodoRiferimento")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                        <%# Eval("PeriodoRiferimento")%>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>



                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="30px"
                                                                                ItemStyle-Width="30px" Text="Stampa" FilterControlAltText="Filter Select column"
                                                                                ImageUrl="~\images\Printer16.png" UniqueName="Select" />



                                                                        </Columns>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>

                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 25px"></td>
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
