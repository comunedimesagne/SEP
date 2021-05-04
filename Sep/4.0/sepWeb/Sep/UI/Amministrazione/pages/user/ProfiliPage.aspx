<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="ProfiliPage.aspx.vb" Inherits="ProfiliPage" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" Runat="Server">


 <script type="text/javascript">

     var _backgroundElement = document.createElement("div");
     var messageBox = document.createElement('div');
     var messageBoxPanel = document.createElement('div');

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
             // _backgroundElement.className = "modalBackground";
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

                 <div style="padding:3px; width: 100%">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 90px">
                            <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *" />
                        </td>
                        <td>
                            <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="300px" />
                        </td>
                        <td style="width: 90px">
                            <asp:Label ID="ModuloLabel" runat="server" CssClass="Etichetta" Text="Modulo *" ForeColor="#FF8040" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ModuloComboBox" runat="server" Skin="Office2007" Width="300"
                                EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 90px">
                            <asp:Label ID="BloccatoLabel" runat="server" CssClass="Etichetta" Text="Bloccato" />
                        </td>
                        <td>
                            <asp:CheckBox ID="BloccatoCheckBox" runat="server" />
                        </td>
                        <td style="width: 90px">
                            <asp:Label ID="RiservatoLabel" runat="server" CssClass="Etichetta" Text="Riservato" />
                        </td>
                        <td>
                            <asp:CheckBox ID="RiservatoCheckBox" runat="server" />
                        </td>
                    </tr>
                </table>

                </div>

    <telerik:RadTabStrip runat="server" ID="DatiUtenteTabStrip" SelectedIndex="0"  MultiPageID="DatiUtenteMultiPage" Skin="Office2007" Width="100%">
     <Tabs>
     <telerik:RadTab Text="Procedure" Selected="True"  />
     <telerik:RadTab Text="Utenti" />
      </Tabs>
     </telerik:RadTabStrip>
     <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
     <telerik:RadMultiPage runat="server" ID="DatiUtenteMultiPage" SelectedIndex="0"  Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">


     <telerik:RadPageView runat="server" ID="ProcedurePageView" CssClass="corporatePageView" Height="200px">
   
          <table style="width: 600px">
                 <tr>
                     <td>
                         <table style="width: 600px">
                             <tr>
                                 <td>
                                     <asp:Label ID="ProcedureLabel" runat="server" CssClass="Etichetta" Text="Procedure" />
                                 </td>
                                 <td align="right" style="width: 20px">
                                     <asp:ImageButton ID="AggiornaProceduraImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                         Style="display: none" />
                                 </td>
                                 <td align="right" style="width: 20px">
                                     <asp:ImageButton ID="TrovaProceduraImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                         ToolTip="Seleziona procedure..." />
                                 </td>
                                 <td align="right" style="width: 20px">
                                     <asp:ImageButton ID="EliminaProceduraImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                         Style="width: 16px" ToolTip="Cancella procedure selezionate" />
                                 </td>
                             </tr>
                         </table>
                     </td>
                 </tr>
                 <tr>
                     <td>
                         <telerik:RadListBox ID="ProcedureListBox" runat="server" Skin="Office2007" Style="width:600px;
                             height: 160px" Height="160px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True">
                         </telerik:RadListBox>
                     </td>
                 </tr>
             </table>
            
              </telerik:RadPageView>

         <telerik:RadPageView runat="server" ID="UtentiPageView" CssClass="corporatePageView" Height="200px">
             <table style="width: 600px">
                 <tr>
                     <td>
                         <table style="width: 600px">
                             <tr>
                                 <td>
                                     <asp:Label ID="UtentiLabel" runat="server" CssClass="Etichetta" Text="Utenti" />
                                 </td>
                                 <td align="right" style="width: 20px">
                                     <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                         Style="display: none" />
                                 </td>
                                 <td align="right" style="width: 20px">
                                     <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                         ToolTip="Seleziona utenti..." />
                                 </td>
                                 <td align="right" style="width: 20px">
                                     <asp:ImageButton ID="EliminaUtenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                         Style="width: 16px" ToolTip="Cancella utenti selezionati" />
                                 </td>
                             </tr>
                         </table>
                     </td>
                 </tr>
                 <tr>
                     <td>
                         <telerik:RadListBox ID="UtentiListBox" runat="server" Skin="Office2007" Style="width:600px;
                             height: 160px" Height="160px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True">
                         </telerik:RadListBox>
                     </td>
                 </tr>
             </table>
         </telerik:RadPageView>

        
     </telerik:RadMultiPage>



         <asp:Panel runat="server" ID="ProfiliPanel">
                                <table style="width: 100%; background-color: #BFDBFF">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloElencoProfiliLabel" runat="server" Font-Bold="True" CssClass="Etichetta"
                                                            Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Profili" />
                                                    </td>
                                                    <%--  <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Filtra registrazioni" Style="border-style: none; border-color: inherit;
                                                            border-width: 0; width: 16px;" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="RipristinaFiltroInizialeImageButton" Style="border: 0" runat="server"
                                                            ImageUrl="~/images//cancelSearch.png" ToolTip="Ripristina filtro iniziale" ImageAlign="AbsMiddle" />
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 330px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="ProfiliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                    Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />

                                                            <telerik:GridBoundColumn DataField="Descrizione" ItemStyle-Width="400px" HeaderStyle-Width="400px"
                                                                FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                                UniqueName="Descrizione" />

                                                            <telerik:GridBoundColumn DataField="DescrizioneModulo" ItemStyle-Width="400px" HeaderStyle-Width="400px"
                                                                FilterControlAltText="Filter DescrizioneModulo column" HeaderText="Modulo" UniqueName="DescrizioneModulo" />

                                                            <telerik:GridTemplateColumn DataField="Disabilitato" FilterControlAltText="Filter Disabilitato column"
                                                                SortExpression="Disabilitato" UniqueName="Disabilitato" ItemStyle-Width="20px"
                                                                HeaderStyle-Width="20px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <img alt='<%# IIF(Eval("Disabilitato") ,"Profilo bloccato", "Profilo sbloccato") %>'
                                                                        src='<%# IIF(Eval("Disabilitato") ,ResolveClientUrl("~/images/Lock_16.png"), ResolveClientUrl("~/images/Unlock_16.png")) %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridButtonColumn ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-VerticalAlign="Middle"
                                                                ItemStyle-HorizontalAlign="Center" ButtonType="ImageButton" CommandName="Select"
                                                                FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png" UniqueName="Select" />

                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>




   
          

 </td>
            </tr>
</table>

          </div>
            <asp:HiddenField ID="infoOperazioneHidden" runat="server" />

   </ContentTemplate>
   </asp:UpdatePanel>
</asp:Content>

