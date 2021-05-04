<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneFirmePage.aspx.vb" Inherits="GestioneFirmePage" %>

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
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveAndExit.png" Text="Salva e Chiudi"
                                                CommandName="SalvaChiudi" Owner="RadToolBar" />
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
                        <br />

                        <table style="width: 100%">
                            <tr>
                                <td style="width: 95px">
                                    <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *"
                                        ForeColor="#FF8040" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ToolTip="Descrizione della firma" ID="DescrizioneTextBox" runat="server"
                                        Skin="Office2007" Width="340px" />
                                </td>
                                <td style="width: 95px">
                                    <asp:Label ID="FirmatarioLabel" runat="server" CssClass="Etichetta" 
                                        Text="Firmatario" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ToolTip="Firmatario di default" ID="FirmatarioTextBox" runat="server"
                                        Skin="Office2007" Width="340px"  />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 95px">
                                    <asp:Label ID="InfoLabel" runat="server" CssClass="Etichetta" Text="Info" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ToolTip="Informazioni sulla firma" ID="InfoTextBox" runat="server" Skin="Office2007"
                                        Width="340px"  />
                                </td>
                                <td style="width: 95px">
                                    <asp:Label ID="EsitoLabel" runat="server" CssClass="Etichetta" 
                                        Text="Esito" />
                                </td>
                                <td>
                                      <telerik:RadTextBox ToolTip="Esito di default" ID="EsitoTextBox" runat="server" Skin="Office2007"
                                        Width="340px"  />
                                </td>
                            </tr>

                              <tr>
                                <td style="width: 95px">
                                    <asp:Label ID="TipiEsitoLabel" runat="server" CssClass="Etichetta" Text="Tipi Esito" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ToolTip="Tipi di esito" ID="TipiEsitoTextBox" runat="server" Skin="Office2007"
                                        Width="340px"  />
                                </td>
                                <td style="width: 95px">
                                    <asp:Label ID="PrefissoLabel" runat="server" CssClass="Etichetta" 
                                        Text="Prefisso" />
                                </td>
                                <td>
                                      <telerik:RadTextBox ToolTip="Prefisso di default" ID="PrefissoTextBox" runat="server" Skin="Office2007"
                                        Width="340px"  />
                                </td>
                            </tr>


                             <tr>
                                <td style="width: 95px">
                                    <asp:Label ID="TipoPrefissoLabel" runat="server" CssClass="Etichetta" Text="Tipo Prefisso" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ToolTip="Tipo di prefisso" ID="TipoPrefissoTextBox" runat="server" Skin="Office2007"
                                        Width="340px" />
                                </td>
                                <td style="width: 95px">
                                    <asp:Label ID="QualificaLabel" runat="server" CssClass="Etichetta" 
                                        Text="Qualifica M *" />
                                </td>
                                <td>
                                      <telerik:RadTextBox ToolTip="Qualifica di default al maschile" ID="QualificaTextBox" runat="server" Skin="Office2007"
                                        Width="340px"  />
                                </td>
                            </tr>

                            
                             <tr>
                                <td style="width: 95px">
                                    <asp:Label ID="FirmaSempliceLabel" runat="server" CssClass="Etichetta" Text="Visto" />
                                </td>
                                <td>
                                       <asp:CheckBox ID="FirmaSempliceCheckBox" runat="server" />
                                </td>
                                <td style="width: 95px">
                                     <asp:Label ID="QualificaFemminileLabel" runat="server" CssClass="Etichetta" 
                                        Text="Qualifica F. *" />
                                </td>
                                <td>
                                         <telerik:RadTextBox ToolTip="Qualifica di default al femminile" ID="QualificaFemminileTextBox" runat="server" Skin="Office2007"
                                        Width="340px"  />
                                </td>
                            </tr>

                          

                        </table>

                       <table style="width: 100%">
                        <tr>

                        <td style="width: 95px">
                          <asp:Label ID="UtenteLabel" runat="server" CssClass="Etichetta" Text="Utente" />
                        </td>
                        <td>
                        <telerik:RadTextBox ID="UtenteTextBox" Runat="server" Skin="Office2007" Width="300px" ReadOnly="true" ToolTip="Utente" />
                       <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona utente..."  ImageAlign="AbsMiddle" />
                       <asp:ImageButton ID="EliminaUtenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella utente"  ImageAlign="AbsMiddle" />
                       <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" Style="display: none" />
                       <asp:TextBox ID="IdUtenteTextBox" Runat="server" style=" display:none"/>
                        
                        </td>
                        <td style="width: 95px">
                         <asp:Label ID="DataFirmaLabel" runat="server" CssClass="Etichetta"  Text="Data Firma" />
                        </td>
                        <td>
                            <asp:CheckBox ID="DataFirmaCheckBox" runat="server" />       
                        </td>
                        </tr>

                           <tr>
                               <td style="width: 95px">
                                   <asp:Label ID="TipologiaFirmaLabel" runat="server" CssClass="Etichetta"
                                       Text="Tipo Firma *" />
                               </td>
                               <td>
                                   <telerik:RadComboBox ID="TipologieFirmaComboBox" runat="server" EmptyMessage="- Seleziona Tipologia Firma -"
                                       MaxHeight="150px" Skin="Office2007"
                                       Width="330px" />
                               </td>

                               <td style="width: 95px">
                                   <asp:Label ID="RuoloLabel" runat="server" CssClass="Etichetta" Text="Ruolo" />
                               </td>
                               <td>
                                 <telerik:RadTextBox ID="RuoloTextBox" Runat="server" Skin="Office2007" Width="300px" ReadOnly="true" ToolTip="Ruolo" />

                                  <asp:ImageButton ID="TrovaRuoloImageButton" runat="server" ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona ruolo..."  ImageAlign="AbsMiddle" />
                       <asp:ImageButton ID="EliminaRuoloImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella ruolo"  ImageAlign="AbsMiddle" />
                       <asp:ImageButton ID="AggiornaRuoloImageButton" runat="server" Style="display: none" />
                       <asp:TextBox ID="IdRuoloTextBox" Runat="server" style=" display:none"/>
                               </td>

                           </tr>

                           <tr>
                               <td style="width: 95px">
                                   <asp:Label ID="GerarchiaLabel" runat="server" CssClass="Etichetta" Text="Gerarchia" />
                               </td>
                               <td>
                                   <telerik:RadComboBox ID="LivelliOrganigrammaComboBox" runat="server" EmptyMessage="- Seleziona Livello Gerarchia -"
                                       MaxHeight="150px" Skin="Office2007"
                                       Width="330px" />
                               </td>
                             <td style="width: 95px">
                                   <asp:Label ID="TipologiaQualificaLabel" runat="server" CssClass="Etichetta" Text="Tipo Qualifica" />
                               </td>
                               <td>
                                   <telerik:RadComboBox ID="TipologieQualificaComboBox" runat="server" EmptyMessage="- Seleziona Tipologia Qualifica-"
                                       MaxHeight="150px" Skin="Office2007"
                                       Width="330px" />
                               </td>
                           </tr>

                          </table>
                       

                        

                        <br />

                        <asp:Panel ID="FirmePanel" runat="server">
                        
                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9" >
                            <tr>
                                <td>
                                    <table style="width: 100%; background-color: #BFDBFF">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitoloElencoFirmeLabel" runat="server" Font-Bold="True"
                                                    Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Firme" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                        border: 0px solid #5D8CC9;">
                                        <telerik:RadGrid ID="FirmeGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                                               

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="True" SortExpression="Id" UniqueName="Id" />

                                                    <telerik:GridTemplateColumn HeaderStyle-Width="280px" ItemStyle-Width="280px" DataField="Descrizione"
                                                        FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                        UniqueName="Descrizione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 280px; border: 0px solid red">
                                                                <%# Eval("Descrizione")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                       <telerik:GridTemplateColumn HeaderStyle-Width="250px" ItemStyle-Width="250px" DataField="DefaultStruttura"
                                                        FilterControlAltText="Filter DefaultStruttura column" HeaderText="Firmatario" SortExpression="DefaultStruttura"
                                                        UniqueName="DefaultStruttura">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DefaultStruttura")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 250px;border: 0px solid red">
                                                                <%# Eval("DefaultStruttura")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn HeaderStyle-Width="230px" ItemStyle-Width="230px" DataField="Utente"
                                                        FilterControlAltText="Filter Utente column" HeaderText="Utente" SortExpression="Utente"
                                                        UniqueName="Utente">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Utente")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 230px;border: 0px solid red">
                                                                <%# Eval("Utente")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                               


                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px" Text="Seleziona Firma" ItemStyle-Width="20px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png" UniqueName="Select" />
                                                   <telerik:GridButtonColumn Text="Seleziona firma e chiudi" FilterControlAltText="Filter ConfirmSelectAndClose column"  ImageUrl="~/images/accept.png" UniqueName="ConfirmSelectAndClose"  ButtonType="ImageButton" CommandName="ConfirmSelectAndClose" HeaderStyle-Width="20px" ItemStyle-Width="20px" />

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
