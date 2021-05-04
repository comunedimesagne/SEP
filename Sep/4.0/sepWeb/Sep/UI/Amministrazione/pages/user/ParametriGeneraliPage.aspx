<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="ParametriGeneraliPage.aspx.vb" Inherits="ParametriGeneraliPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">

 <script type="text/javascript">

     var _backgroundElement = document.createElement("div");
     var messageBox = document.createElement('div');
     var messageBoxPanel = document.createElement('div');

     var hideFullSizePanel = true;

     function pageLoad() {
         var manager = Sys.WebForms.PageRequestManager.getInstance();
         manager.add_beginRequest(OnBeginRequest);
         manager.add_endRequest(OnEndRequest);
         $get("pageContent").appendChild(_backgroundElement);

         if (hideFullSizePanel) {
             HideFullSizeGridPanel();
         } else {
             ShowFullSizeGridPanel();
         }

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


     //JavaScript Object Literal
     //Alterno lo stato della visibilità
     var toggle = {
              show: function (obj) {
             obj.style.display = '';
             },
              hide: function (obj) {
             obj.style.display = 'none';
            }
     };


     function ShowFullSizeGridPanel() {
         var panel = document.getElementById("FullSizeGridPanel");
         panel.style.display = '';

         //var h = panel.offsetHeight - 55;
         //var div = $get('<%= fullSizeScrollPanel.ClientId %>');
         //div.style.height = h + "px";

         hideFullSizePanel = false;
         // toggle.show(panel);
     }

     function HideFullSizeGridPanel() {
         var panel = document.getElementById("FullSizeGridPanel");
         panel.style.display = 'none';
         hideFullSizePanel = true;
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
                        <table style="width: 100%">
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

                         <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 90px">
                                                <asp:Label ID="ModuloLabel" runat="server" CssClass="Etichetta" Text="Modulo *" ForeColor="#FF8040" />
                                            </td>
                                            <td><telerik:RadComboBox ID="ModuloComboBox" runat="server" Skin="Office2007" 
                                        Width="249px" EmptyMessage="- Tutti -" ItemsPerRequest="10" 
                                        Filter="StartsWith" MaxHeight="300px" /></td>
                                        </tr>
                                       
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 90px">
                                                <asp:Label ID="NomeLabel" runat="server" CssClass="Etichetta" Text="Nome *" ForeColor="#FF8040" />
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ToolTip="Nome del parametro" ID="NomeTextBox" runat="server"
                                                    Skin="Office2007" Width="600px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 90px">
                                                <asp:Label ID="ValoreLabel" runat="server" CssClass="Etichetta" Text="Valore" />
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ToolTip="Valore del parametro" ID="ValoreTextBox" runat="server"
                                                    Skin="Office2007" Width="600px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione" /><br />
                                    <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="900px"
                                        MaxLength="255" ToolTip="Descrizione del parametro" Height="70px" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 90px">
                                                <asp:Label ID="RiservatoLabel" runat="server" CssClass="Etichetta" Text="Riservato" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="RiservatoCheckBox" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                      </div>

                         <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">

                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9" >
                            <tr>
                                <td>
                                    <table style="width: 100%; background-color: #BFDBFF">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitoloElencoParametriLabel" runat="server" Font-Bold="True"
                                                    Style="width: 600px; color: #00156E; background-color: #BFDBFF" Text="Elenco Parametri" />
                                            </td>

                                            <td align="center" style="width:125; border-left:0 solid #5D8CC9; display:none">
                                                    <telerik:RadButton ID="NoPaging" runat="server" Text="Non Paginare"   Skin="Office2007"
                                                      ImageAlign="AbsMiddle"  Width="115px" 
                                                      ToolTip="Disattiva/Attiva Paginazione">  
                                                      <Icon PrimaryIconUrl="~/images/Next.png" PrimaryIconLeft="5px"/>
                                                    </telerik:RadButton>
                                                </td>
                                            <td align="center" style="width: 40px; display:none">
                                                                    <asp:ImageButton ID="VisualizzaSchermoInteroImageButton" Style="border: 0" runat="server"
                                                                        ImageUrl="~/images//full_screen_icon.png" ToolTip="Visualizza griglia a schermo intero"
                                                                        ImageAlign="AbsMiddle" />
                                                                </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>

                             
                                    <div id="scrollPanel" runat="server" style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF; 
                                        border-top:1px solid #5D8CC9;">
                                        <telerik:RadGrid ID="ParametriGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False" Culture="it-IT"
                                            CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True">
                                            
                                            <MasterTableView DataKeyNames="Id" TableLayout="Auto">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />


                                                           <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="Modulo"
                                                        FilterControlAltText="Filter Modulo column" HeaderText="Mod." SortExpression="Modulo"
                                                        UniqueName="Modulo" HeaderTooltip="Modulo di appartenenza del parametro">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Modulo")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 50px;border: 0px solid red">
                                                                <%# Eval("Modulo")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn HeaderStyle-Width="220px" ItemStyle-Width="220px" DataField="Nome"
                                                        FilterControlAltText="Filter Nome column" HeaderText="Nome" SortExpression="Nome"
                                                        UniqueName="Nome">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Nome")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 200px;border: 0px solid red">
                                                                <%# Eval("Nome")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn HeaderStyle-Width="220px" ItemStyle-Width="220px" DataField="Valore"
                                                        FilterControlAltText="Filter Valore column" HeaderText="Valore" SortExpression="Valore"
                                                        UniqueName="Valore">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Valore")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 200px;border: 0px solid red">
                                                                <%# Eval("Valore")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn  DataField="Descrizione"
                                                        FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                        UniqueName="Descrizione">
                                                        <ItemTemplate>

                                                            <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 340px; border: 0px solid red">
                                                                <%# Eval("Descrizione")%></div>

                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="30px"
                                                        ItemStyle-Width="30px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png"
                                                        UniqueName="Select" />
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

            </div>


             <div id="FullSizeGridPanel" style="position: absolute; width: 100%; height:100%;text-align: center; z-index : 2000000; display: none; top: 0px; left:0px; background-color:White">
                   

                 <table style="width: 100%; height:100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                     <tr>
                           <td style=" vertical-align:top; height:30px">
                             <table style="width: 100%; background-color: #BFDBFF">
                                 <tr>
                                     <td>
                                         &nbsp;<asp:Label ID="TitoloElencoFullSizeParametriLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                             color: #00156E; background-color: #BFDBFF" Text="Elenco Parametri" />
                                     </td>
                                      <td align="center" style="width:125; border-left:0 solid #5D8CC9;">
                                                    <telerik:RadButton ID="FullSizeNoPaging" runat="server" Text="Non Paginare"   Skin="Office2007"
                                                      ImageAlign="AbsMiddle"  Width="115px" 
                                                      ToolTip="Disattiva/Attiva Paginazione">  
                                                      <Icon PrimaryIconUrl="~/images/Next.png" PrimaryIconLeft="5px"/>
                                                    </telerik:RadButton>
                                                </td>

                                     <td align="center" style="width: 40px">
                                         <img alt="Nascondi griglia a schermo intero" src="../../../../images/original_size_icon.png"
                                             style="border: 0px" onclick="HideFullSizeGridPanel();" />
                                     </td>
                                 </tr>
                             </table>
                         </td>
                     </tr>

                     <tr>
                    <td style=" vertical-align:top; padding-bottom:2px">

                    <div id="fullSizeScrollPanel" runat="server" style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF; 
                                        border-top:1px solid #5D8CC9; position:relative">
                      <telerik:RadGrid ID="FullSizeParametriGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False" Culture="it-IT"
                                            CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True">

                          <ClientSettings>
                              <%--<Resizing AllowColumnResize="true" EnableRealTimeResize="true" ResizeGridOnColumnResize="true" />--%>
                             
                          </ClientSettings>
                                            
                                            <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />

                                                    <telerik:GridBoundColumn DataField="Modulo" FilterControlAltText="Filter Modulo column"
                                                        HeaderStyle-Width="70px" ItemStyle-Width="70px" HeaderText="Mod." SortExpression="Modulo"
                                                        UniqueName="Modulo">
                                                    </telerik:GridBoundColumn>


                                                    <telerik:GridBoundColumn DataField="Nome" FilterControlAltText="Filter Nome column"
                                                        HeaderStyle-Width="220px" ItemStyle-Width="220px" HeaderText="Nome" SortExpression="Nome"
                                                        UniqueName="Nome">
                                                    </telerik:GridBoundColumn>


                                                     <telerik:GridBoundColumn DataField="Valore" FilterControlAltText="Filter Valore column"
                                                        HeaderStyle-Width="220px" ItemStyle-Width="220px" HeaderText="Valore" SortExpression="Valore"
                                                        UniqueName="Valore">
                                                    </telerik:GridBoundColumn>

                                                    
                                                     <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column" 
                                                         HeaderText="Descrizione" SortExpression="Descrizione"
                                                        UniqueName="Descrizione">
                                                    </telerik:GridBoundColumn>
                                                  
                                                    <telerik:GridButtonColumn ButtonType="ImageButton"   CommandName="Select" HeaderStyle-Width="30px"
                                                        ItemStyle-Width="30px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png"
                                                        UniqueName="Select" Text="Seleziona Parametro"  />
                                                </Columns>
                                                
                                            </MasterTableView>
                                        </telerik:RadGrid>

                                        </div>
                    </td>

                     </tr>
                 </table>


                    


                </div>

             <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
               <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
