<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneConsiglieriPage.aspx.vb" Inherits="GestioneConsiglieriPage" %>

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

              

                var h = document.getElementsByTagName('body')[0].offsetHeight;
                var w = document.getElementsByTagName('body')[0].offsetWidth;


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
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"  Text=" Gestione Consiglieri" />
                                        </td>
                                    </tr>

                                   
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                               
                                                <tr>
                                               <td align="center" style="height: 20px">
                                                       
                                                   <telerik:RadButton ID="GeneraButton" runat="server" Skin="Office2007" 
                                                       Text="Genera" Width="100px" ToolTip="Aggiorna Consiglieri da Organigramma">
                                                       <Icon PrimaryIconLeft="5px" PrimaryIconUrl="../../../../images/processa.png" />
                                                   </telerik:RadButton>
                                                       
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
                                                                                &nbsp;<asp:Label ID="ConsiglieriLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                    color: #00156E; background-color: #BFDBFF" Text="Elenco Consiglieri" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="overflow: auto; height: 540px; width: 100%; background-color: #FFFFFF; border: 0px solid #5D8CC9;">


                                                                        <telerik:RadGrid ID="ConsiglieriGridView" runat="server" AllowPaging="False"
                                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                            Width="99.8%" AllowSorting="True" Culture="it-IT" AllowMultiRowSelection="True">
                                                                            <MasterTableView DataKeyNames="Id"   >
                                                                                <Columns>


                                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                                                  
                                                                                  <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                            AllowFiltering="False">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                                    runat="server"></asp:CheckBox>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                                    runat="server"></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                                                            <ItemStyle Width="20px" />
                                                                        </telerik:GridTemplateColumn>


                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="460px" ItemStyle-Width="460px" DataField="Nominativo"
                                                                                        FilterControlAltText="Filter Nominativo column" HeaderText="Nominativo" SortExpression="Nominativo"
                                                                                        UniqueName="Nominativo">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 460px; border: 0px solid red">
                                                                                                <%# Eval("Nominativo")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                  
                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                        ImageUrl="~\images\checks.png" UniqueName="Select">
                                        <HeaderStyle Width="20px" />
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
                                         
                                            </table>
                                        </td>
                                    </tr>

                                     <tr>
                                        <td align="center"  style="background-color: #BFDBFF;padding: 4px; border-bottom:0px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                         
                                            <telerik:RadButton ID="ConfermaButton" runat="server" Text="Conferma" Width="100px" Skin="Office2007" ToolTip="Conferma la selezione effettuata">
                                              <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px"  />
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
