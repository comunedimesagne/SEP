<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="AddettiPage.aspx.vb" Inherits="AddettiPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">

    

    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');


        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
            $get("pageContent").appendChild(_backgroundElement);

            //onDataBound();
        }


        function OnBeginRequest(sender, args) {
            EnableUI(false);
        }

        function OnEndRequest(sender, args) {
            count = 2;
            var message = $get('<%= infoOperazioneHidden.ClientId %>').value;

            if (message !== '') {

                //VISUALIZZO IL MESSAGGIO

                ShowMessageBox(message);

                var intervallo = setInterval(function () {
                    count = count - 1;
                    if (count <= 0) {
                        HideMessageBox();
                        EnableUI(true);
                        clearInterval(intervallo);

                    }
                }, 1000);


                $get('<%= infoOperazioneHidden.ClientId %>').value = '';

            } else { EnableUI(true); }
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
                _backgroundElement.style.backgroundColor = '#09718F';
                _backgroundElement.style.filter = "alpha(opacity=20)";
                _backgroundElement.style.opacity = "0.2";
            }
            else {
                _backgroundElement.style.display = 'none';
            }
        }


        function ShowMessageBox(message) {


            this.document.body.appendChild(messageBox);
            this.document.body.appendChild(messageBoxPanel);

            with (messageBoxPanel) {
                style.display = '';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.width = '100%';
                style.height = '100%';
                style.zIndex = 10000;
                style.backgroundColor = '#09718F';
                style.filter = "alpha(opacity=20)";
                style.opacity = "0.2";
            }

            with (messageBox) {
                style.width = '305px';
                style.height = '40px';
                style.backgroundColor = '#BFDBFF';
                style.border = 'solid #4892FF 2px';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.zIndex = 10000;
                style.textAlign = 'center';
                style.verticalAlign = 'middle';
                innerHTML = message;
                style.color = '#00156E';
                style.backgroundImage = 'url(/sep/Images/success.png)';
                style.backgroundPosition = '5px center';
                style.backgroundRepeat = 'no-repeat';

                style.lineHeight = '40px';
            }


            xc = Math.round((document.body.clientWidth / 2) - (300 / 2));
            yc = Math.round((document.body.clientHeight / 2) - (40 / 2));


            messageBox.style.left = xc + "px";
            messageBox.style.top = yc + "px";
            messageBox.style.display = 'block';



        }



        function HideMessageBox() {
            try {
                messageBox.style.display = 'none';
                messageBoxPanel.style.display = 'none';
            }
            catch (e) { }
        }




        function OnScrollUtenti(obj) {
         
            var grid = $find("<%=UtentiGridView.ClientID %>");
            var tableView = grid.get_masterTableView();
            var top = obj.scrollTop;
            var columnCount = tableView.get_columns().length;
           
            for (var i = 0; i < columnCount; i++) {

               var c = tableView.get_columns()[i].get_element();

                var w = c.parentNode.offsetWidth;
                var h = c.parentNode.offsetHeight;

                c.parentNode.style.position = 'absolute';

                c.parentNode.style.top = c.parentNode.offsetParent.scrollTop -2 + 'px';

                c.parentNode.style.width = w + 'px';
                c.parentNode.style.height = h + 'px';

               // document.title = c.parentNode.offsetTop + '  -  ' + top;
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

       

                <table style="width: 900px; border: 1px solid #5D8CC9">

                  <tr>
                <td>

                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                                    <Items>
                                        <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                            CommandName="Nuovo" Owner="RadToolBar" />
                                        <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                            CommandName="Trova" Owner="RadToolBar" />
                                        <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                            CommandName="Annulla" Owner="RadToolBar" />
                                        <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                            CommandName="Salva" Owner="RadToolBar" />
                                        <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                            CommandName="Elimina" Owner="RadToolBar" />
                                        <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                            CommandName="Stampa" Owner="RadToolBar" />
                                        <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                        <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                            CommandName="Home" Owner="RadToolBar" />
                                    </Items>
                                </telerik:RadToolBar>
                            </td>
                        </tr>
                    </table>

                      <div  id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">

                   <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                          <tr style="height: 30px">
                              <td style="width: 130px">
                                  <asp:Label ID="ResponsabileLabel" runat="server" CssClass="Etichetta" Text="Responsabile *"
                                      ForeColor="#FF8040" />
                              </td>
                              <td style="width: 370px">
                                  <telerik:RadTextBox ID="UtenteTextBox" runat="server" Skin="Office2007" Width="350px"
                                      Enabled="False" ToolTip="Utente Responsabile" />
                              </td>
                              <td style="width: 25px; text-align: center">
                                  <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                      ToolTip="Seleziona Responsabile..." ImageAlign="AbsMiddle" />
                              </td>
                              <td>
                                  <asp:ImageButton ID="EliminaUtenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                      ToolTip="Cancella Responsabile" ImageAlign="AbsMiddle" />
                                  <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" Style="display: none" />
                                  <asp:TextBox ID="IdUtenteTextBox" runat="server" Style="display: none" />
                              </td>
                          </tr>
                      </table>
                  </div>

                
                 <table style="width: 100%;">
                                <tr>

                                    <td style="width: 49%">
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 20px">
                                                                
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 250px">
                                                                            &nbsp;<asp:Label ID="ElencoUtentiLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                Text="Utenti" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                             
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ContainerMargin">
                                                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                      
                                                                            <div id="scrollPanelUtenti" runat="server"  style="overflow: auto; height: 200px; width: auto; background-color: #FFFFFF;
                                                                                border-top: 1px solid #5D8CC9">

                                                                                <telerik:RadGrid ID="UtentiGridView" runat="server" AutoGenerateColumns="False"
                                                                                    CellSpacing="0" GridLines="None" Skin="Office2007"  ToolTip="Elenco Utenti"
                                                                                    AllowSorting="False" AllowMultiRowSelection="true"
                                                                                    Culture="it-IT">
                                                                                   
                                                                                    <ClientSettings>
                                                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                  
                                                                                     
                                                                                    </ClientSettings>

                                                                                     


                                                                                    <MasterTableView DataKeyNames="Id">
                                                                                        <Columns>


                                                                                            <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                                                            </telerik:GridClientSelectColumn>

                                                                                           

                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                                          
                                                                                          
                                                                                            <telerik:GridTemplateColumn  DataField="Nominativo"
                                                                                                FilterControlAltText="Filter Nominativo column" HeaderText="Nominativo" SortExpression="Nominativo"
                                                                                                UniqueName="Nominativo">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 340px; border: 0px solid red">
                                                                                                        <%# Eval("Nominativo")%></div>
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
                                        </table>
                                    </td>

                                    <td align="center" width="2%">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%; height: 161px;">
                                            <tr>
                                                <td align="center" style="height: 26px">
                                                    <asp:ImageButton ID="AggiungiTuttoImageButton" runat="server" ImageUrl="~/images/Forwardd24.png"  ToolTip="Aggiungi tutto" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:ImageButton ID="EliminaTuttoImageButton" runat="server" ImageUrl="~/images/Forwards24.png"
                                                         ToolTip="Rimuovi tutto" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px;">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px">
                                                    <asp:ImageButton ID="AggiungiImageButton" runat="server" ImageUrl="~/images/FrecciaDx24.png" ToolTip="Aggiungi elementi selezionati" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px">
                                                    <asp:ImageButton ID="EliminaImageButton" runat="server" ImageUrl="~/images/FrecciaSx24.png"  ToolTip="Rimuovi elementi selezionati" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                    <td width="49%">
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 20px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="ElencoAddettiLabel" runat="server" Style="color: #00156E"
                                                                                Font-Bold="True" Text="Addetti" />

                                                                               
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ContainerMargin">
                                                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div id="scrollPanelAddetti" style="overflow: auto; height: 200px; width: 100%; background-color: #FFFFFF;
                                                                                border-top:  1px solid #5D8CC9;">
                                                                                <telerik:RadGrid ID="AddettiGridView" runat="server" AutoGenerateColumns="False"
                                                                                    CellSpacing="0" GridLines="None" Skin="Office2007"  ToolTip="Elenco Addetti"
                                                                                    AllowSorting="False" AllowMultiRowSelection="true"
                                                                                    Culture="it-IT">

                                                                                      <ClientSettings>
                                                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                    </ClientSettings>

                                                                                    <MasterTableView DataKeyNames="IdAddetto">
                                                                                       
                                                                                        <Columns>

                                                                                            <telerik:GridClientSelectColumn UniqueName="SelectCheckBox"
                                                                                            HeaderStyle-HorizontalAlign="Center"
                                                                                            ItemStyle-HorizontalAlign="Center"
                                                                                             HeaderStyle-Width="20px" ItemStyle-Width="20px" >
                                                                                               
                                                                                            </telerik:GridClientSelectColumn>



                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                                                            <telerik:GridTemplateColumn DataField="Addetto"
                                                                                                FilterControlAltText="Filter Addetto column" HeaderText="Nominativo" SortExpression="Addetto"
                                                                                                UniqueName="Addetto">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Addetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 340px; border: 0px solid red">
                                                                                                        <%# Eval("Addetto")%></div>
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
                                        </table>
                                    </td>

                                </tr>

                            </table>
                        

                             <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                                  <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                <tr>
                                    <td>
                                        <table style="width: 100%; background-color: #BFDBFF">
                                            <tr>
                                                <td>
                                                    &nbsp;<asp:Label ID="TitoloElencoResponsabiliLabel" runat="server" Font-Bold="True"
                                                        Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Responsabili"
                                                        CssClass="Etichetta" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                               <div id="PannelloContenitoreGriglia" runat="server" style="overflow: auto; background-color: #FFFFFF;  border: 0px solid #5D8CC9;">

                                            <telerik:RadGrid ID="ResponsabiliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True"
                                                ToolTip="Elenco Responsabili" Culture="it-IT" AllowFilteringByColumn="false">
                                                <MasterTableView DataKeyNames="Id">
                                                    <Columns>

                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                            HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False">
                                                        </telerik:GridBoundColumn>

                                                       

                                                        <telerik:GridTemplateColumn HeaderStyle-Width="825px" ItemStyle-Width="825px" DataField="Responsabile"
                                                            FilterControlAltText="Filter Responsabile column" HeaderText="Responsabile" SortExpression="Responsabile"
                                                            AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                            ShowFilterIcon="False" UniqueName="Responsabile">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("Responsabile")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 825px; border: 0px solid red">
                                                                    <%# Eval("Responsabile")%></div>
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
                            </div>
                </td>
                </tr>

            


                


                  
                </table>


  

     
               <asp:HiddenField ID="infoOperazioneHidden" runat="server" />


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
