<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SchedaControlloSuccessivoRegolaritaAmministrativaPage.aspx.vb"
    Inherits="SchedaControlloSuccessivoRegolaritaAmministrativaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">


<head id="Head1" runat="server">
    <title>Visualizza Istanza Pratica</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
  </head>


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
                       window.close();
                    
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

           messageBox = document.createElement('DIV');


           this.document.body.appendChild(messageBox);

           with (messageBox) {
               style.width = '300px';
               style.height = '40px';
               style.backgroundColor = '#BFDBFF';
               style.border = 'solid #4892FF 2px';
               style.position = 'absolute';
               style.left = '0px';
               style.top = '0px';
               style.zIndex = 10000;
               innerHTML = message;
               style.color = '#00156E';
               style.backgroundImage = 'url(/sep/Images/success.png)';
               style.backgroundPosition = '10px center';
               style.backgroundRepeat = 'no-repeat';
               style.padding = '15px 10px 15px 50px';
               style.margin = '15px 0px';
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
           }
           catch (e) { }
       }

      

    </script>

<body>

 <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
     

             <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center; top: 300px;z-index:2000000; left: 0px;">
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


    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />


      <asp:UpdatePanel ID="Pannello" runat="server">
       <ContentTemplate>



    <div id="pageContent" style="overflow:auto; border: 0px solid red; height:100%">
        <center>

            <table width="900px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                           <%-- HEADER--%>
                            <tr>

                             <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                 <table style= "  width:100%">
                                     <tr>
                                         <td>
                                             &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                 Text="Dettaglio Scheda Controllo Regolarità Ammistrativa" />
                                         </td>

                                         <td>
                                           <asp:ImageButton ID="VisualizzaDocumentoFirmatoImageButton" runat="server" ImageUrl="~/images//DocumentoFirmato.gif"
                                                    
                                                 Style="border-style: none; border-color: inherit; border-width: 0px; height: 24px;" 
                                                 ToolTip="Visualizza documento firmato" ImageAlign="Top" Visible="false" />
                                         </td>
                                         <td>
                                          <asp:ImageButton ID="VisualizzaDocumentoImageButton" runat="server" ImageUrl="~/images//Documento.gif"
                                                    Style="border: 0px;" ToolTip="Visualizza documento" ImageAlign="Top" Visible="false" />
                                         </td>
                                      <td style="background-color: #4892FF; width: 90px">
                                             <asp:Table ID="componentPlaceHolder" runat="server" CellPadding="0" CellSpacing="0"
                                                ToolTip="Componenti installati"  Style="width: 100%">
                                                 <asp:TableRow>
                                                 </asp:TableRow>
                                             </asp:Table>
                                           
                                         </td>
                                     </tr>
                                 </table>
                            </td>
                           
                              
                            </tr>

                              <%-- BODY--%>

                            <tr>
                                <td class="ContainerMargin">


                                    <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                        <tr style="height: 22px">
                                            <td style="width: 90px">
                                                <asp:Label ID="NumeroPraticaLabel" runat="server" CssClass="Etichetta" Text="N. Atto" />
                                            </td>
                                            <td style="width: 100px">
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                    <asp:Label ID="NumeroTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="95px" Text="&nbsp;" />
                                                </span>
                                            </td>
                                            <td style="width: 70px; text-align: center">
                                                <asp:Label ID="DataPresentazioneLabel" runat="server" CssClass="Etichetta" Text="Data" />
                                            </td>
                                            <td>
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100px; height: 21px">
                                                    <asp:Label ID="DataTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="100px" Text="&nbsp;" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>


                                    <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                        <tr style="height: 22px">
                                            <td style="width: 90px;">
                                                <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                            </td>
                                            <td>
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                    <asp:Label ID="OggettoTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="100%" Text="&nbsp;" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>


                                  

                                    <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                        <tr style="height: 22px">
                                            <td style="width: 90px;">
                                                <asp:Label ID="SettoreLabel" runat="server" CssClass="Etichetta" Text="Settore" />
                                            </td>
                                            <td>
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                    <asp:Label ID="SettoreTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="100%" Text="&nbsp;" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>

                                    
                                      <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                        <tr style="height: 22px">
                                            <td style="width: 90px;">
                                                <asp:Label ID="ResponsabileLabel" runat="server" CssClass="Etichetta" Text="Responsabile" />
                                            </td>
                                            <td>
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                    <asp:Label ID="ResponsabileTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="100%" Text="&nbsp;" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>

                                    <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                        <tr style="height: 22px">
                                            <td style="width: 90px;">
                                                <asp:Label ID="OsservazioniLabel" runat="server" CssClass="Etichetta" Text="Osservazioni" />
                                            </td>
                                            <td>
                                               <telerik:RadTextBox ID="OsservazioniTextBox" runat="server" Skin="Office2007"
                                                    Width="775px" Rows="2" TextMode="MultiLine" ToolTip="Osservazioni" />
                                            </td>
                                        </tr>
                                    </table>

                                    

                                     <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                        <tr style="height: 22px">
                                            <td style="width: 90px;">
                                                <asp:Label ID="EsitoLabel" runat="server" CssClass="Etichetta" Text="Esito" />
                                            </td>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 30px">
                                                            <asp:RadioButton Text="&nbsp;" Checked="true" AutoPostBack="false" ID="EsitoPositivoRadioButton"
                                                                GroupName="Esito" runat="server" CssClass="Etichetta" />
                                                        </td>
                                                        <td style="width: 100px">
                                                            <asp:Label ID="EsitoPositivoLabel" runat="server" Text="Positivo" CssClass="Etichetta" 
                                                                Style="color: #00156E" />
                                                        </td>
                                                        <td style="width: 30px">
                                                            <asp:RadioButton Text="&nbsp;" AutoPostBack="false" ID="EsitoNegativoRadioButton" GroupName="Esito"
                                                                runat="server" Style="color: #00156E" />
                                                        </td>
                                                       <td>
                                                            <asp:Label ID="EsitoNegativoLabel" runat="server" Text="Negativo" CssClass="Etichetta"
                                                                Style="color: #00156E" />
                                                        </td>

                                                        
                                                    </tr>
                                                </table>
                                              

                                            </td>
                                        </tr>
                                    </table>
                       

                                    <div id="GrigliaIndicatoriPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                       <td style="width: 220px;">
                                                                            <asp:Label ID="IndicatoriLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                Style="width: 220px; color: #00156E; background-color: #BFDBFF" Text="Indicatori Conformità" />
                                                                        </td>

                                                                         <td style="width: 40px;">
                                                <asp:Label ID="AnnotazioniLabel" runat="server" CssClass="Etichetta" Text="Note" style="color:#00156E" Font-Bold="True" />
                                            </td>
                                            <td>
                                               <telerik:RadTextBox ID="AnnotazioniTextBox" runat="server" Skin="Office2007"
                                                    Width="580px" ToolTip="Note" MaxLength="2500" />

                                            </td>
                                              <td style="width: 30px; text-align:center">
                                                 <asp:ImageButton ID="AggiornaNoteImageButton" runat="server" ImageUrl="~/images//ok16.png"
                                                                            ToolTip="Aggiorna note indicatore conformità selezionato" ImageAlign="AbsMiddle" />
                                            </td>
                                           


                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>

                                                      
                                                        



                                                        <tr style="background-color: #FFFFFF">
                                                            <td>
                                                                <div style="overflow: auto; height: 220px; border: 1px solid #5D8CC9">
                                                                    <telerik:RadGrid ID="IndicatoriGridView" runat="server" ToolTip="Elenco indicatori associati alla scheda"
                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                        Width="99.8%" Culture="it-IT">
                                                                        <MasterTableView DataKeyNames="Id, Descrizione, IdIndicatore">
                                                                            <Columns>

                                                                                <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione" HeaderText="Descrizione"
                                                                                    DataField="Descrizione" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 300px; border: 0px solid red">
                                                                                            <%# Eval("Descrizione")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>


                                                                                 <telerik:GridTemplateColumn SortExpression="Conforme" UniqueName="Conforme" HeaderText="Conforme"
                                                                                    DataField="Conforme" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:RadioButton AutoPostBack="false" ID="SiRadioButton" Checked='<%# Eval("EsitoConformita") = 0 %>'
                                                                                              GroupName="Tipologia" runat="server" CssClass="Etichetta" />

                                                                                         
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                 <telerik:GridTemplateColumn SortExpression="NonConforme" UniqueName="NonConforme" HeaderText="Non Conforme"
                                                                                    DataField="NonConforme" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:RadioButton  AutoPostBack="false" ID="NoRadioButton" Checked='<%#  Eval("EsitoConformita") = 1 %>'
                                                                                              GroupName="Tipologia" runat="server" CssClass="Etichetta" />

                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                  <telerik:GridTemplateColumn SortExpression="NonApplicabile" UniqueName="NonApplicabile" HeaderText="Non Applicabile"
                                                                                    DataField="NonApplicabile" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:RadioButton  AutoPostBack="false" ID="NoApplicabileRadioButton" Checked='<%#  Eval("EsitoConformita") = 2 %>'
                                                                                              GroupName="Tipologia" runat="server" CssClass="Etichetta" />

                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>


                                                                                  <telerik:GridTemplateColumn SortExpression="Annotazioni" UniqueName="Annotazioni" HeaderText="Note"
                                                                                    DataField="Annotazioni" HeaderStyle-Width="200px" ItemStyle-Width="200px" >
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="NoteTextBox" Text='<%#  Eval("Annotazioni") %>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 200px; border: 0px solid red; display:none"
                                                                                               runat="server" CssClass="Etichetta" />

                                                                                          <div title='<%# Eval("Annotazioni")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 200px; border: 0px solid red">
                                                                                            <%# Eval("Annotazioni")%></div>

                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                  <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                            ItemStyle-Width="20px" Text="Seleziona Via" FilterControlAltText="Filter Select column"
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


                              <%-- FOOTER--%>
                            <tr>
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    <table style="width: 100%">
                                        <tr>

                                            <td style="text-align: right">

                                             <telerik:RadButton ID="SalvaButton" runat="server" Text="Salva" Width="130px"
                                                    Skin="Office2007">
                                                </telerik:RadButton>

                                                <telerik:RadButton ID="ChiudiFinestraButton" runat="server" Text="Chiudi" Width="130px"
                                                    Skin="Office2007">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>


              




        </center>
    </div>


            <div style="display: none">
                <asp:Button runat="server" ID="notificaOperazioneButton" Style="width: 0px; height: 0px;
                    left: -1000px; position: absolute" />
            </div>
         
           <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
           <asp:HiddenField ID="IdIndicatoreHidden" runat="server" />

        </ContentTemplate>
 </asp:UpdatePanel>


    </form>
</body>
</html>
