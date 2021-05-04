<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneCentriResponsabilitaPage.aspx.vb" Inherits="GestioneCentriResponsabilitaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">

 


    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');

        var rowSelectedHidden;

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
            $get("pageContent").appendChild(_backgroundElement);


             rowSelectedHidden = $get('<%= rowSelectedHidden.ClientID %>');

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


        



        function RowSelected(sender, eventArgs) {
            var grid = sender;
            var masterTable = grid.get_masterTableView();
            var numeroRigheSelezionate = masterTable.get_selectedItems().length;
            var numeroRighe = masterTable.get_dataItems().length;
            //alert(numeroRigheSelezionate);

            rowSelectedHidden.value = numeroRigheSelezionate;
            

        }




        function IsChecked(element) {
             
        //if (element.checked == true) { checkAll=1; }
         //else { checkAll = 0; }

         }


         function ConfirmDeleteUtenti(message) {
             var numeroRigheSelezionate = parseInt(rowSelectedHidden.value, 10);
             if (numeroRigheSelezionate > 0) {
                 return confirm(message);
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

                <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>

                  



                <table style="width: 900px; border: 1px solid #5D8CC9">
                    <tr>
                        <td>

                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                                            <Items>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                                    CommandName="Nuovo" Owner="RadToolBar" Enabled="false" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                    CommandName="Trova" Owner="RadToolBar" Enabled="false" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar" Enabled="false" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                    CommandName="Stampa" Owner="RadToolBar" Enabled="false" />
                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar" />
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>


                          <%-- <div  id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">

                                 <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td style="width: 90px">
                                            <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *"
                                                ForeColor="#FF8040" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ToolTip="Descrizione della tipologia di seduta" ID="DescrizioneTextBox"
                                                runat="server" Skin="Office2007" Width="99%" />
                                        </td>
                                    </tr>
                                  
                                </table>
                            </div>--%>

                             
                                 <div  id="PannelloGrigliaUtenti" runat="server" style="padding: 2px 2px 2px 2px;">

                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                <tr style="height: 20px; background-color: #BFDBFF">
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    &nbsp;<asp:Label Font-Bold="True" ID="TitoloElencoUtentiLabel" runat="server" Style="color: #00156E;
                                                        background-color: #BFDBFF; width: 600px" CssClass="Etichetta" Text="Elenco Utenti" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                        Style="display: none; width: 0" />
                                                </td>
                                                <td style="width:30px; text-align:center">
                                                    <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                        ToolTip="Seleziona utente..." ImageAlign="AbsMiddle" />
                                                </td>
                                               <td style="width:30px; text-align:center">
                                                    <asp:ImageButton ID="EliminaUtentiSelezionatiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                        Style="width: 16px" ToolTip="Cancella utenti selezionati" ImageAlign="AbsMiddle" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                      <div id="ZoneID1">
                                        <div id="scrollPanel" runat="server" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9;
                                            background-color: White">
                                            <telerik:RadGrid ID="UtentiGridView" ToolTip="Elenco utenti" runat="server" AllowPaging="False"
                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                 AllowSorting="True" AllowMultiRowSelection="True" Culture="it-IT">

                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false"  />
                                                      <ClientEvents  OnRowDeselected="RowSelected" OnRowSelected="RowSelected" /> 
                                                </ClientSettings>

                                                <MasterTableView DataKeyNames="IdUtente">
                                                    <Columns>


                                                        <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                        </telerik:GridClientSelectColumn>

                                                        <telerik:GridBoundColumn DataField="IdUtente" DataType="System.Int32" FilterControlAltText="Filter IdUtente column"
                                                            HeaderText="IdUtente" UniqueName="IdUtente" Visible="False" />


                                                         

                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneUtente" UniqueName="DescrizioneUtente"
                                                            HeaderText="Nominativo" DataField="DescrizioneUtente" HeaderStyle-Width="780px"
                                                            ItemStyle-Width="780px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DescrizioneUtente")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 780px; border: 0px solid red">
                                                                    <%# Eval("DescrizioneUtente")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                            ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\Delete16.png" UniqueName="Delete" ConfirmText="Eliminare l'elemento selezionato?"  Text="Elimina Utente" >
                                                        </telerik:GridButtonColumn>


                                                    </Columns>
                                                </MasterTableView></telerik:RadGrid></div>
                                    </div>
                                    </td>
                                </tr>
                            </table>

                            </div>


                             <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">

                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloCentriResponsabilitaLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                                             color: #00156E; background-color: #BFDBFF" 
                                                            Text="Elenco Centri Responsabilità" CssClass="Etichetta" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="CentriResponsabilitaGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True" AllowFilteringByColumn="True"
                                                    Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Codice">
                                                        <Columns>

                                                         <telerik:GridTemplateColumn SortExpression="Codice" UniqueName="Codice"
                                                            HeaderText="Codice" DataField="Codice" HeaderStyle-Width="70px"
                                                            ItemStyle-Width="70px" ShowFilterIcon="False" FilterControlWidth="100%"  AutoPostBackOnFilter="True" AndCurrentFilterFunction="Contains">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("Codice")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                    <%# Eval("Codice")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                          
                                                              <telerik:GridTemplateColumn HeaderStyle-Width="370px" ItemStyle-Width="370px" DataField="Descrizione"
                                                                FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                                 ShowFilterIcon="False"
                                                                AutoPostBackOnFilter="True" FilterControlWidth="100%" AndCurrentFilterFunction="Contains"
                                                                UniqueName="Descrizione">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 370px; border: 0px solid red">
                                                                        <%# Eval("Descrizione")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderStyle-Width="370px" ItemStyle-Width="370px" DataField="Responsabile"
                                                                FilterControlAltText="Filter Responsabile column" HeaderText="Responsabile" SortExpression="Responsabile"
                                                                UniqueName="Responsabile" ShowFilterIcon="False" AutoPostBackOnFilter="True"
                                                                FilterControlWidth="100%" AndCurrentFilterFunction="Contains">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Responsabile")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 370px; border: 0px solid red">
                                                                        <%# Eval("Responsabile")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                         

                                                       
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                Text="Seleziona Centro Responsabilità" ItemStyle-Width="20px" FilterControlAltText="Filter Select column" 
                                                                ImageUrl="~\images\checks.png" UniqueName="Select" />

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
                <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />

                <asp:HiddenField ID="rowSelectedHidden" runat="server" Value="0" />

                <asp:HiddenField ID="CodiceStrutturaHidden" runat="server" />

            </div>

                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
