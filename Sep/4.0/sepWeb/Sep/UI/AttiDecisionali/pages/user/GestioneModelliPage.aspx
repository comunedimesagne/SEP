<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneModelliPage.aspx.vb" Inherits="GestioneModelliPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">

 <script type="text/javascript">

     var _backgroundElement = document.createElement("div");
     var messageBox = document.createElement('div');
     var messageBoxPanel = document.createElement('div');
     var overlay = document.createElement("div");

     var count = 2;

     var hideSearchPanel = true;

     function pageLoad() {
         var manager = Sys.WebForms.PageRequestManager.getInstance();
         manager.add_beginRequest(OnBeginRequest);
         manager.add_endRequest(OnEndRequest);
         $get("pageContent").appendChild(_backgroundElement);
         $get("pageContent").appendChild(overlay);

         if (hideSearchPanel) {
             HideSearchPanel();
         } else {
             ShowSearchPanel();
         }


      }


     function OnBeginRequest(sender, args) {
       EnableUI(false);
     }

   


   function OnEndRequest(sender, args) {
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



     function HideSearchPanel() {
         var panel = document.getElementById("SearchPanel");
         panel.style.display = "none";
         overlay.style.display = 'none';
         //_backgroundElement.style.display = 'none';

     }

     function ShowSearchPanel() {
      
         var panel = document.getElementById("SearchPanel");
         panel.style.display = '';
         panel.style.position = 'absolute';

         overlay.style.display = '';
         overlay.style.position = 'absolute';
         overlay.style.left = '0px';
         overlay.style.top = '0px';
         overlay.style.width = '100%';
         overlay.style.height = '100%';
         overlay.style.zIndex = 10000;
         overlay.style.backgroundColor = '#09718F';
         overlay.style.filter = "alpha(opacity=20)";
         overlay.style.opacity = "0.2";

         var shadow = document.getElementById("ShadowSearchPanel");

         with (shadow) {
             style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
             style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
             style.boxShadow = "3px 3px 4px #333";
             style.mozBoxShadow = "3px 3px 4px #333";
             style.webkitBoxShadow = "3px 3px 4px #333";
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

           <div style=" display:none">
                 <asp:Button runat="server" ID="DisabilitaPulsantePredefinito" Style="width: 0px;
                                    height: 0px; display:none" />
           </div>
      


            <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server" DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator> 


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
                                                CommandName="Stampa" Owner="RadToolBar"  />
                                            <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                           <%-- <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                CommandName="Home" Owner="RadToolBar" />--%>

                                          
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/AdvancedSearch32.png" PostBack="false"
                                                    Text="Ricerca Avanzata" CommandName="RicercaAvanzata" Owner="RadToolBar"  />
                                        </Items>
                                    </telerik:RadToolBar>
                                </td>
                            </tr>
                        </table>
                       
                           <div  id="PannelloDettaglio" runat="server" style="padding: 0px 2px 2px 2px;">

                               <table id="TabellaNotifica" style="width: 100%; background-color: #BFDBFF">
                                   <tr style="height: 2px">
                                       <td>
                                       </td>
                                   </tr>
                               </table>


                        <telerik:RadTabStrip runat="server" ID="ModelliTabStrip" SelectedIndex="1" MultiPageID="ModelliMultiPage"
                            Skin="Office2007" Width="100%">
                            <Tabs>
                                <telerik:RadTab runat="server" Text="Dati Generali" Selected="True" />
                                <telerik:RadTab runat="server" Text="Firme" />
                            </Tabs>
                        </telerik:RadTabStrip>
                        <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                               <telerik:RadMultiPage runat="server" ID="ModelliMultiPage" SelectedIndex="0" Height="100%"
                                   CssClass="multiPage" BorderColor="#3399FF">
                                   <telerik:RadPageView runat="server" ID="DatiGeneraliPageView" CssClass="corporatePageView"
                                       Height="225px">
                                       <table style="width: 100%">
                                           <tr>
                                               <td style="width: 90px">
                                                   <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta" Text="Documento *"
                                                       ForeColor="#FF8040" />
                                               </td>
                                               <td>
                                                   <telerik:RadComboBox ID="TipologieDocumentoComboBox" runat="server" AutoPostBack="true"
                                                       EmptyMessage="- Seleziona Tipologia -" MaxHeight="150px" Skin="Office2007" Width="340px" />
                                               </td>
                                               <td style="width: 90px">
                                                   <asp:Label ID="SezioneTrasparenzaLabel" runat="server" CssClass="Etichetta" Text="Sezione" />
                                               </td>
                                               <td>
                                                   <telerik:RadComboBox ID="SezioneTrasparenzaComboBox" runat="server" AutoPostBack="false"
                                                       EmptyMessage="- Seleziona Sezione Trasparenza -" MaxHeight="150px" Skin="Office2007"
                                                       Width="340px" />
                                               </td>
                                           </tr>
                                           <tr>
                                               <td style="width: 90px">
                                                   <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *"
                                                       ForeColor="#FF8040" />
                                               </td>
                                               <td>
                                                   <telerik:RadTextBox ToolTip="Descrizione del modello" ID="DescrizioneTextBox" runat="server"
                                                       Skin="Office2007" Width="340px" />
                                               </td>
                                               <td style="width: 90px">
                                                   <asp:Label ID="TipologiaRegistroLabel" runat="server" CssClass="Etichetta" Text="Registro" />
                                               </td>
                                               <td>
                                                   <telerik:RadComboBox ID="TipologieRegistroComboBox" runat="server" EmptyMessage="- Seleziona Registro -"
                                                       MaxHeight="150px" Skin="Office2007" Width="340px" />
                                               </td>
                                           </tr>
                                           <tr>
                                               <td style="width: 90px">
                                                   <asp:Label ID="MetaModelloLabel" runat="server" CssClass="Etichetta" Text="Meta mod. *" />
                                               </td>
                                               <td>
                                                   <telerik:RadComboBox ID="MetaModelliComboBox" runat="server" EmptyMessage="- Seleziona Metamodello -"
                                                       MaxHeight="150px" Skin="Office2007" Width="340px" />
                                               </td>
                                               <td style="width: 90px">
                                                   <asp:Label ID="IterLabel" runat="server" CssClass="Etichetta" Text="Iter" />
                                               </td>
                                               <td>
                                                   <telerik:RadComboBox ID="IterComboBox" runat="server" EmptyMessage="- Seleziona Iter -"
                                                       MaxHeight="150px" Skin="Office2007" Width="340px" />
                                               </td>
                                           </tr>
                                           <tr>
                                               <td style="width: 90px">
                                                   <asp:Label ID="PrefissoLabel" runat="server" CssClass="Etichetta" Text="Prefisso *" />
                                               </td>
                                               <td>
                                                   <table style="width: 100%">
                                                       <tr>
                                                           <td>
                                                               <telerik:RadTextBox ToolTip="Prefisso del modello collegato" ID="PrefissoTextBox"
                                                                   runat="server" Skin="Office2007" Width="70px" />
                                                           </td>
                                                           <td>
                                                               <asp:Label ID="ModelloAdottatoLabel" runat="server" CssClass="Etichetta" Text="Adottato" />
                                                           </td>
                                                           <td>
                                                               <telerik:RadComboBox ID="ModelliAdottatiComboBox" runat="server" EmptyMessage="- Seleziona Modello -"
                                                                   MaxHeight="150px" Skin="Office2007" Width="200px" />
                                                           </td>
                                                       </tr>
                                                   </table>
                                               </td>
                                               <td style="width: 90px">
                                                   <asp:Label ID="TipologiaOrganoLabel" runat="server" CssClass="Etichetta" Text="Organo" />
                                               </td>
                                               <td>
                                                   <table style="width: 100%">
                                                       <tr>
                                                           <td>
                                                               <telerik:RadComboBox ID="TipologieOrganoComboBox" runat="server" EmptyMessage="- Seleziona Organo -"
                                                                   MaxHeight="150px" Skin="Office2007" Width="190px" />
                                                           </td>
                                                           <td>
                                                               <asp:Label ID="DataInizioValiditaLabel" runat="server" CssClass="Etichetta" Text="Valido dal" />
                                                           </td>
                                                           <td>
                                                               <telerik:RadTextBox ToolTip="Data inizio validità" ID="DataInizioValiditaTextBox"
                                                                   runat="server" Skin="Office2007" Width="65px" ReadOnly="True" />
                                                           </td>
                                                       </tr>
                                                   </table>
                                               </td>
                                           </tr>
                                       </table>
                                       <div id="ZoneID2">
                                           <table style="width: 100%">
                                               <tr style="height: 30px">
                                                   <td>
                                                       <div id="DisabilitatoPanel" runat="server">
                                                           <asp:CheckBox ID="DisabilitatoCheckBox" runat="server" Text="Disabilitato" />&nbsp;&nbsp;&nbsp;
                                                       </div>
                                                   </td>
                                                   <td>
                                                       <div id="PropostaPanel" runat="server">
                                                           &nbsp;&nbsp;
                                                           <asp:CheckBox ID="PropostaCheckBox" runat="server" Text="Proposta" />&nbsp;&nbsp;&nbsp;
                                                       </div>
                                                   </td>
                                                   <td>
                                                       <div id="ObbligoAllegatiPanel" runat="server">
                                                           &nbsp;&nbsp;
                                                           <asp:CheckBox ID="ObbligoAllegatiCheckBox" runat="server" Text="Obbligo Allegati" />&nbsp;&nbsp;&nbsp;
                                                       </div>
                                                       <td>
                                                           <div id="PubblicazioneAlboPanel" runat="server">
                                                               &nbsp;&nbsp;
                                                               <asp:CheckBox ID="PubblicazioneAlboCheckBox" runat="server" Text="Pubblicazione Albo (Art.32 L.69/2009)" /></div>
                                                       </td>
                                               </tr>
                                               <tr style="height: 30px">
                                                   <td>
                                                       <div id="AccertamentoPanel" runat="server">
                                                           <asp:CheckBox ID="AccertamentoCheckBox" runat="server" Text="Accertamento" />&nbsp;&nbsp;&nbsp;
                                                       </div>
                                                   </td>
                                                   <td>
                                                       <div id="ImpegnoSpesaPanel" runat="server">
                                                           &nbsp;&nbsp;
                                                           <asp:CheckBox ID="ImpegnoSpesaCheckBox" runat="server" Text="Impegno di Spesa" />&nbsp;&nbsp;&nbsp;
                                                       </div>
                                                   </td>
                                                   <td>
                                                       <div id="LiquidazionePanel" runat="server">
                                                           &nbsp;&nbsp;
                                                           <asp:CheckBox ID="LiquidazioneCheckBox" runat="server" Text="Liquidazione" />&nbsp;&nbsp;&nbsp;
                                                       </div>
                                                   </td>
                                                   <td>
                                                       <div id="PubblicazioneLiquidazionePanel" runat="server">
                                                           &nbsp;&nbsp;
                                                           <asp:CheckBox ID="PubblicazioneLiquidazioneCheckBox" runat="server" Text="Pubblicazione Liquidazione (Art.18 L.134/2012)" /></div>
                                                   </td>
                                               </tr>
                                           </table>
                                       </div>
                                   </telerik:RadPageView>
                                   <telerik:RadPageView runat="server" ID="FirmePageView" CssClass="corporatePageView"
                                       Height="225px">
                                       <div runat="server" id="PannelloFirme" style="padding: 2px 2px 2px 2px;">
                                           <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                               <tr>
                                                   <td style="height: 20px">
                                                       <table style="width: 100%">
                                                           <tr>
                                                               <td>
                                                                   <asp:Label ID="FirmeLabel" runat="server" CssClass="Etichetta" Font-Bold="True" Style="width: 400px;
                                                                       color: #00156E; background-color: #BFDBFF" Text="Elenco Firme" />
                                                               </td>
                                                               <td style="width: 305px">
                                                                   <telerik:RadTextBox ToolTip="Digitare parola chiave (INVIO)" ID="FiltroTextBox" runat="server"
                                                                       Skin="Office2007" Width="300px" />
                                                               </td>
                                                               <td align="right" style="width: 30px">
                                                                   <asp:ImageButton ID="AggiungiFirmaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                       ToolTip="Aggiungi firma" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                               </td>
                                                           </tr>
                                                       </table>
                                                   </td>
                                               </tr>
                                               <tr style="background-color: #FFFFFF">
                                                   <td>
                                                       <div style="overflow: auto; height: 180px; border: 1px solid #5D8CC9">

                                                           <telerik:RadGrid ID="FirmeGridView" runat="server" ToolTip="Elenco firme associati al modello"
                                                               AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                               Culture="it-IT">
                                                               <MasterTableView DataKeyNames="IdFirma">
                                                                   <Columns>
                                                                       <telerik:GridBoundColumn DataField="IdFirma" DataType="System.Int32" FilterControlAltText="Filter IdFirma column"
                                                                           HeaderText="Id" ReadOnly="True" SortExpression="IdFirma" UniqueName="IdFirma"
                                                                           Visible="true" />
                                                                       <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                           HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="680px" ItemStyle-Width="680px">
                                                                           <ItemTemplate>
                                                                               <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                   text-overflow: ellipsis; width: 680px; border: 0px solid red">
                                                                                   <%# Eval("Descrizione")%>
                                                                               </div>
                                                                           </ItemTemplate>
                                                                       </telerik:GridTemplateColumn>
                                                                       <telerik:GridTemplateColumn SortExpression="Iter" UniqueName="Iter" HeaderText="Iter"
                                                                           DataField="Iter" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                           <ItemTemplate>
                                                                               <div title='<%# if(Eval("Iter"),"SI","NO")%>' style="white-space: nowrap; overflow: hidden;
                                                                                   text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                                   <%# If(Eval("Iter"), "SI", "NO")%>
                                                                               </div>
                                                                           </ItemTemplate>
                                                                       </telerik:GridTemplateColumn>
                                                                       <telerik:GridTemplateColumn SortExpression="Ordinale" UniqueName="Ordinale" HeaderText="Ordinale"
                                                                           DataField="Ordinale" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                           <ItemTemplate>
                                                                               <div title='<%# Eval("Ordinale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                   text-overflow: ellipsis; width: 50px; border: 0px solid red">
                                                                                   <%# Eval("Ordinale")%>
                                                                               </div>
                                                                           </ItemTemplate>
                                                                       </telerik:GridTemplateColumn>
                                                                       <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                           ItemStyle-Width="10px" HeaderStyle-Width="10px" ImageUrl="~\images\Delete16.png"
                                                                           UniqueName="Delete" />
                                                                   </Columns>
                                                               </MasterTableView></telerik:RadGrid>

                                                       </div>
                                                   </td>
                                               </tr>
                                           </table>
                                       </div>
                                   </telerik:RadPageView>
                               </telerik:RadMultiPage>

                        </div>

                           <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">

                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9" >
                            <tr>
                                <td>
                                    <table style="width: 100%; background-color: #BFDBFF">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitoloElencoModelliLabel" runat="server" Font-Bold="True"
                                                    Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Firme" />
                                            </td>
                                            
                                             <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Filtra registrazioni" Style="border-style: none; border-color: inherit;
                                                            border-width: 0; width: 16px;" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="RipristinaFiltroInizialeImageButton" Style="border: 0" runat="server"
                                                            ImageUrl="~/images//cancelSearch.png" ToolTip="Ripristina filtro iniziale" ImageAlign="AbsMiddle" />
                                                    </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                        border: 0px solid #5D8CC9;">
                                        <telerik:RadGrid ID="ModelliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True" Culture="it-IT">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                                                 <telerik:GridButtonColumn Text="Visualizza Modello..."  HeaderStyle-Width="20px" ItemStyle-Width="20px" ButtonType="ImageButton" CommandName="Preview"  FilterControlAltText="Filter Preview column" ImageUrl="~\images\knob-search16.png" UniqueName="Preview" />

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />

                                                    <telerik:GridTemplateColumn HeaderStyle-Width="460px" ItemStyle-Width="460px" DataField="Descrizione"
                                                        FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                        UniqueName="Descrizione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 460px; border: 0px solid red">
                                                                <%# Eval("Descrizione")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    
                                                    <telerik:GridTemplateColumn SortExpression="DataInizio" UniqueName="DataInizio"
                                                                            HeaderText="Data" DataField="DataInizio" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DataInizio","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; text-overflow: ellipsis; width: 65px;border: 0px solid red">
                                                                                    <%# Eval("DataInizio", "{0:dd/MM/yyyy}")%></div>
                                                                            </ItemTemplate>
                                                                           
                                                                        </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn HeaderStyle-Width="250px" ItemStyle-Width="250px" DataField="DescrizioneTipologiaDocumento"
                                                        FilterControlAltText="Filter DescrizioneTipologiaDocumento column" HeaderText="Tipologia" SortExpression="DescrizioneTipologiaDocumento"
                                                        UniqueName="DescrizioneTipologiaDocumento">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DescrizioneTipologiaDocumento")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 250px;border: 0px solid red">
                                                                <%# Eval("DescrizioneTipologiaDocumento")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                               


                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px" Text="Seleziona Modello"
                                                        ItemStyle-Width="20px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png"
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


            <asp:ImageButton ID="AggiornaFirmaImageButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
         <asp:HiddenField ID="infoOperazioneHidden" runat="server" />

            </div>



             <div id="SearchPanel" style="position: absolute; width: 100%; text-align: center;
                z-index: 2000000; display: block; top: 300px">
                <div id="ShadowSearchPanel" style="width: 600px; text-align: center; background-color: #BFDBFF;
                    margin: 0 auto">


                    <table width="600px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="TitoloRicercaLabel" runat="server" Style="color: #00156E" Font-Bold="True" Text="Ricerca Modello"
                                                            CssClass="Etichetta" />
                                                    </td>
                                                    <td align="right">
                                                        <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="document.getElementById('<%= Me.ChiudiRicercaImageButton.ClientID %>').click();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%-- BODY--%>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 100px; width: 100%; background-color: #DFE8F6;
                                                            border: 0px solid #5D8CC9;">
                                                            <table style="width: 100%">
                                                                <tr style="height: 25px">
                                                                    <td style="width: 120px">
                                                                        <asp:Label ID="TipologiaDocumentoFiltroLabel" runat="server" CssClass="Etichetta"
                                                                            Text="Documento" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <telerik:RadComboBox ID="TipologieDocumentoFiltroComboBox" runat="server" 
                                                                            EmptyMessage="- Seleziona Tipologia -" MaxHeight="150px" Skin="Office2007" Width="340px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 120px">
                                                                        <asp:Label ID="DescrizioneFiltroLabel" runat="server" CssClass="Etichetta" Text="Descrizione" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <telerik:RadTextBox ToolTip="Descrizione del modello" ID="DescrizioneFiltroTextBox" runat="server"
                                                                            Skin="Office2007" Width="340px" />
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td style="width: 120px">
                                                                        <asp:Label ID="StatoLabel" runat="server" CssClass="Etichetta" Text="Stato" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                     <telerik:RadComboBox ID="StatoFiltroComboBox" runat="server" AutoPostBack="true"
                                                                            EmptyMessage="- Seleziona Stato -" MaxHeight="150px" Skin="Office2007" Width="340px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%-- FOOTER--%>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                           

                                            <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="90px" Skin="Office2007"
                                        ToolTip="Effettua la ricerca con i filtri impostati">
                                        <Icon PrimaryIconUrl="../../../../images/check16.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                    &nbsp;
                                   <%-- <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="90px"
                                        Skin="Office2007" ToolTip="Annulla i filtri impostati">
                                        <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                     &nbsp; --%>
                                     <telerik:RadButton ID="ChiudiRicercaImageButton" runat="server" Text="Chiudi" Width="100px" 
                                                Skin="Office2007" ToolTip="Chiudi la finestra">
                                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            
      
    

       </ContentTemplate>
       
   

           

    </asp:UpdatePanel>

   





         


</asp:Content>
