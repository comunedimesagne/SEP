<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="ImpostaContatoriDeterminePage.aspx.vb" Inherits="ImpostaContatoriDeterminePage" %>

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

                    <table width="600px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">

                                    <tr>
                                        <td  style="background-color: #BFDBFF;padding: 4px; border-bottom:1px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"  Text=" Gestione Contatori Determine" />
                                        </td>
                                    </tr>

                                   
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td align="center" style="height: 50px">
                                                        <asp:Label ID="AnnoCorrenteLabel" runat="server" Font-Size="Medium" Style="color: #00156E"
                                                            Font-Bold="True" />
                                                           
                                                     
                                                    </td>
                                                    </td>
                                                </tr>
                                                <tr>
                                               <td align="center" style="height: 20px">
                                                       <asp:Label ID="MessaggioLabel" runat="server" Text=""  style="color: #00156E" />
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
                                                                                    color: #00156E; background-color: #BFDBFF" Text="Generale" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="overflow: auto; height: 35px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">
                                                                        <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                            <tr>
                                                                                <td style="width: 80px; vertical-align: middle">
                                                                                    <asp:Label ID="CorrenteLabel" runat="server" CssClass="Etichetta" Text="Corrente" />
                                                                                </td>
                                                                                  <td style="width: 120px">
                                                                                   <telerik:RadTextBox  ReadOnly="true" ToolTip="" MaxLength="5" ID="ContatoreGeneraleTextBox" runat="server" Skin="Office2007" Width="70px" />
                                                                                
                                                                                </td>
                                                                                <td style="width: 80px">
                                                                                    <asp:Label ID="NuovoLabel" runat="server" CssClass="Etichetta" Text="Nuovo" />
                                                                                </td>
                                                                                <td>
                                                                                  


                                                                                         <telerik:RadNumericTextBox ID="NuovoContatoreGeneraleTextBox" runat="server" Skin="Office2007" 
                                                                                     Width="70px" DataType="System.Int32" MaxLength="5" MaxValue="99999" MinValue="1" 
                                                                       ShowSpinButtons="True" ToolTip="" >
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                       </telerik:RadNumericTextBox>

   </td>
                                                                           <td>
 <telerik:RadTextBox ID="IdContatoreGeneraleTextBox" Runat="server" Skin="Office2007" Width="0px"  style=" display:none"/>
                                                                           </td>

                                                                                
                                                                                
                                                                                
                                                                             
                                                                            </tr>
                                                                        </table>
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
                                                                                    color: #00156E; background-color: #BFDBFF" Text="Settoriale" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">
                                                                        <telerik:RadGrid ID="ContatoriSettoreGridView" runat="server" AllowPaging="True"
                                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                            Width="99.8%" AllowSorting="True" Culture="it-IT">
                                                                            <MasterTableView DataKeyNames="Id">
                                                                                <Columns>
                                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="390px" ItemStyle-Width="390px" DataField="Settore"
                                                                                        FilterControlAltText="Filter Settore column" HeaderText="Settore" SortExpression="Settore"
                                                                                        UniqueName="Settore">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Settore")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 390px; border: 0px solid red">
                                                                                                <%# Eval("Settore")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="Valore"
                                                                                        FilterControlAltText="Filter Valore column" HeaderText="Corrente" SortExpression="Valore"
                                                                                        UniqueName="Valore">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Valore")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 70px; border: 0px solid red">
                                                                                                <%# Eval("Valore")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" HeaderText="Nuovo"
                                                                                        UniqueName="Nuovo">
                                                                                        <ItemTemplate>
                                                                                            <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 70px;
                                                                                                border: 0px solid red">

                                                                                                   <telerik:RadNumericTextBox ID="NuovoContatoreSettoreTextBox" runat="server" Skin="Office2007" 
                                                                                     Width="70px" DataType="System.Int32" MaxLength="5" MaxValue="99999" MinValue="1"  Text='<%# Eval("Valore")%>'
                                                                       ShowSpinButtons="True" ToolTip="" >
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                       </telerik:RadNumericTextBox>

                                                                                           
                                                                                            </div>
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
                                        <td align="center"  style="background-color: #BFDBFF;padding: 4px; border-bottom:0px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                           <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="80px" Skin="Office2007"
                                                            ToolTip="Esegui passaggio a nuovo anno">
                                                            <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
