<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="StatisticheUtentiAttiPage.aspx.vb" Inherits="StatisticheUtentiAttiPage" %>

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
            //Gestione messaggio 
            //            var dataItems = args.get_dataItems();
            //            if (dataItems['MainContent_messaggio'] != null) {
            //               alert(dataItems['MainContent_messaggio']);
            //               dataItems['MainContent_messaggio'] = null;
            //            } 
        }

        function EnableUI(state) {
            if (!state) {
                _backgroundElement.style.display = '';
                _backgroundElement.style.position = 'absolute';
                _backgroundElement.style.left = '0px';
                _backgroundElement.style.top = '0px';

                var h = document.getElementById("lyrCorpoPagina").offsetHeight;
                var w = document.getElementById("lyrCorpoPagina").offsetWidth;


                _backgroundElement.style.width = w + 'px';
                _backgroundElement.style.height = h + 'px';

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
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="200">
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px">
                <table cellpadding="4" style="background-color: #4892FF">
                    <tr>
                        <td>
                            <div id="loadingContainer" style="width: 300px; text-align: center; background-color: #BFDBFF;
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
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                                        &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                            Text="Statistiche" />
                                                    </td>
                                                    <td align="center" style="width: 30px; background-color: #BFDBFF; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8;">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Effettua la ricerca con i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px; background-color: #BFDBFF; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8;">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td align="center" style="height: 50px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 60px">
                                                                    <asp:Label ID="DataLabel" runat="server" CssClass="Etichetta" Text="Data" ForeColor="#00156E"
                                                                        Font-Bold="True" />
                                                                </td>
                                                                <td style="width: 30px">
                                                                    <asp:Label ID="DataInizioLabel" runat="server" CssClass="Etichetta" Text="da" ForeColor="#00156E" />
                                                                </td>
                                                                <td style="width: 150px">
                                                                    <telerik:RadDatePicker ID="DataInizioTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                        MinDate="1753-01-01" />
                                                                </td>
                                                                <td style="width: 30px">
                                                                    <asp:Label ID="DataAFineLabel" runat="server" CssClass="Etichetta" Text="a" ForeColor="#00156E" />
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="DataFineTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                        MinDate="1753-01-01" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;<asp:Label ID="GeneraleLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Statictiche Per Tipologia" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 150px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <telerik:RadGrid ID="StatisticheDocumentoGridView" runat="server" AllowPaging="True"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="99.8%" AllowSorting="True" Culture="it-IT">
                                                                <MasterTableView DataKeyNames="Descrizione">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                            HeaderText="Tipologia" SortExpression="Descrizione" UniqueName="Descrizione" />
                                                                        <telerik:GridBoundColumn DataField="NumeroProposte" FilterControlAltText="Filter NumeroProposte column"
                                                                            HeaderStyle-Width="70px" ItemStyle-Width="70px" HeaderText="Proposte" SortExpression="NumeroProposte"
                                                                            UniqueName="NumeroProposte" />
                                                                        <telerik:GridBoundColumn DataField="NumeroProposteAnnullate" FilterControlAltText="Filter NumeroProposteAnnullate column"
                                                                            HeaderStyle-Width="70px" ItemStyle-Width="70px" HeaderText="Annullate" SortExpression="NumeroProposteAnnullate"
                                                                            UniqueName="NumeroProposteAnnullate" />
                                                                        <telerik:GridBoundColumn DataField="NumeroAtti" FilterControlAltText="Filter NumeroAtti column"
                                                                            HeaderStyle-Width="70px" ItemStyle-Width="70px" HeaderText="Atti" SortExpression="NumeroAtti"
                                                                            UniqueName="NumeroAtti" />
                                                                        <telerik:GridBoundColumn DataField="NumeroAttiAnnullati" FilterControlAltText="Filter NumeroAttiAnnullati column"
                                                                            HeaderStyle-Width="70px" ItemStyle-Width="70px" HeaderText="Annullati" SortExpression="NumeroAttiAnnullati"
                                                                            UniqueName="NumeroAttiAnnullati" />
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
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;<asp:Label ID="SettorialeLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Statistiche Per Utente" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <telerik:RadGrid ID="StatisticheGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                Culture="it-IT">
                                                                <MasterTableView DataKeyNames="Id">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="390px" ItemStyle-Width="390px" DataField="Utente"
                                                                            FilterControlAltText="Filter Utente column" HeaderText="Utente" SortExpression="Utente"
                                                                            UniqueName="Utente">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Utente")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 390px; border: 0px solid red">
                                                                                    <%# Eval("Utente")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroDocumenti"
                                                                            FilterControlAltText="Filter NumeroDocumenti column" HeaderText="Atti" SortExpression="NumeroDocumenti"
                                                                            UniqueName="NumeroDocumenti">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroDocumenti")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                    <%# Eval("NumeroDocumenti")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroDocumentiAnnullati"
                                                                            FilterControlAltText="Filter NumeroDocumentiAnnullati column" HeaderText="Annullati"
                                                                            SortExpression="NumeroDocumentiAnnullati" UniqueName="NumeroDocumentiAnnullati">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroDocumentiAnnullati")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                    <%# Eval("NumeroDocumentiAnnullati")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroDetermine"
                                                                            FilterControlAltText="Filter NumeroDetermine column" HeaderText="Determine" SortExpression="NumeroDetermine"
                                                                            UniqueName="NumeroDetermine">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroDetermine")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                    <%# Eval("NumeroDetermine")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroDelibere"
                                                                            FilterControlAltText="Filter NumeroDelibere column" HeaderText="Delibere" SortExpression="NumeroDelibere"
                                                                            UniqueName="NumeroDelibere">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroDelibere")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                    <%# Eval("NumeroDelibere")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroDirettive"
                                                                            FilterControlAltText="Filter NumeroDirettive column" HeaderText="Direttive" SortExpression="NumeroDirettive"
                                                                            UniqueName="NumeroDirettive">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroDirettive")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                    <%# Eval("NumeroDirettive")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroDecreti"
                                                                            FilterControlAltText="Filter NumeroDecreti column" HeaderText="Decreti" SortExpression="NumeroDecreti"
                                                                            UniqueName="NumeroDecreti">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroDecreti")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                    <%# Eval("NumeroDecreti")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="NumeroOrdinanze"
                                                                            FilterControlAltText="Filter NumeroOrdinanze column" HeaderText="Ordinanze" SortExpression="NumeroOrdinanze"
                                                                            UniqueName="NumeroOrdinanze">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroOrdinanze")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                    <%# Eval("NumeroOrdinanze")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                border-top: 1px solid  #9ABBE8; height: 25px">
                                <telerik:RadButton ID="SalvaButton" runat="server" Text="Esporta" Width="90px" Skin="Office2007"
                                    ToolTip="Esporta in formato CSV">
                                    <Icon PrimaryIconUrl="../../../../images/excel16.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                 
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
