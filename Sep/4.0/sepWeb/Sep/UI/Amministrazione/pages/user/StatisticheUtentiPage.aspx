<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="StatisticheUtentiPage.aspx.vb" Inherits="StatisticheUtentiPage" %>

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
     

             <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center; top: 300px;z-index:2000000">
                <table cellpadding="4" style="background-color: #4892FF;margin: 0 auto">
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
                                                                    &nbsp;<asp:Label ID="LogEventiLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Elenco Log" />
                                                                </td>
                                                            
                                                               <td align="center" style="width: 40px">
                                                   <asp:ImageButton ID="EsportaInExcelImageButton" 
                                                       Style="border: 0" runat="server"
                                                       ImageUrl="~/images//excel32.png" 
                                                       ToolTip="Esporta i log visualizzati in un file formato excel" 
                                                       ImageAlign="AbsMiddle" />
                                               </td>
                                                          
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 150px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <telerik:RadGrid ID="LogEventiGridView" runat="server" AllowPaging="False"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="99.8%" AllowSorting="True" Culture="it-IT">
                                                                <MasterTableView DataKeyNames="Descrizione">
                                                                    <Columns>


                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="110px" ItemStyle-Width="110px" DataField="Data"
                                                                            FilterControlAltText="Filter Data column" HeaderText="Data" SortExpression="Data"
                                                                            UniqueName="Data">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Data")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 110px; border: 0px solid red">
                                                                                    <%# Eval("Data")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>


                                                                       

                                                                         <telerik:GridTemplateColumn HeaderStyle-Width="590px" ItemStyle-Width="590px" DataField="Descrizione"
                                                                            FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                                            UniqueName="Descrizione">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 590px; border: 0px solid red">
                                                                                    <%# Eval("Descrizione")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" DataField="IndirizzoIP"
                                                                            FilterControlAltText="Filter IndirizzoIP column" HeaderText="Indirizzo IP" SortExpression="IndirizzoIP"
                                                                            UniqueName="IndirizzoIP">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("IndirizzoIP")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 100px; border: 0px solid red">
                                                                                    <%# Eval("IndirizzoIP")%></div>
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
                                                        <div style="overflow: auto; height: 325px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <telerik:RadGrid ID="StatisticheGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                Culture="it-IT">
                                                                <MasterTableView DataKeyNames="Id">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />



                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="430px" ItemStyle-Width="430px" DataField="Utente"
                                                                            FilterControlAltText="Filter Utente column" HeaderText="Utente" SortExpression="Utente"
                                                                            UniqueName="Utente">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Utente")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 430px; border: 0px solid red">
                                                                                    <%# Eval("Utente")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>


                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" ItemStyle-Width="130px" DataField="DataUltimoAccesso"
                                                                            FilterControlAltText="Filter DataUltimoAccesso column" HeaderText="Ultimo Accesso" SortExpression="DataUltimoAccesso"
                                                                            UniqueName="DataUltimoAccesso">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DataUltimoAccesso")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 130px; border: 0px solid red">
                                                                                    <%# Eval("DataUltimoAccesso")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>



                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" ItemStyle-Width="130px" DataField="DataUltimoSettaggioPassword"
                                                                            FilterControlAltText="Filter DataUltimoSettaggioPassword column" HeaderText="Ultimo Cambio Pass."
                                                                            SortExpression="DataUltimoSettaggioPassword" UniqueName="DataUltimoSettaggioPassword">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DataUltimoSettaggioPassword")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 130px; border: 0px solid red">
                                                                                    <%# Eval("DataUltimoSettaggioPassword")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>


                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" ItemStyle-Width="80px" DataField="NumeroAccessi"
                                                                            FilterControlAltText="Filter NumeroAccessi column" HeaderText="N. Accessi" SortExpression="NumeroAccessi"
                                                                            UniqueName="NumeroAccessi">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroAccessi")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                                    <%# Eval("NumeroAccessi")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                          <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                     ImageUrl="~\images\checks.png" UniqueName="Select">
                                                     <HeaderStyle Width="10px" />
                                                     <ItemStyle Width="10px" />
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
