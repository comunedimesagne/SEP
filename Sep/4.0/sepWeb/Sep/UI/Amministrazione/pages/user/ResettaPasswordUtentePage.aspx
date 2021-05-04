<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="ResettaPasswordUtentePage.aspx.vb" Inherits="ResettaPasswordUtentePage" %>

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
         var count = 2;
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

         var panel = document.getElementById("pageContent");

         panel.appendChild(messageBox);
         panel.appendChild(messageBoxPanel);

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
             style.left = '50%';
             style.top = '25%';
             style.marginLeft = '-150px';
             style.marginTop = '-20px';
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

       <center>

           <table width="600px" cellpadding="5" cellspacing="5" border="0">
               <tr>
                   <td>
                       <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                           <tr>
                               <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                   border-top: 1px solid  #9ABBE8; height: 25px">
                                   <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                       <tr>
                                           <td>
                                               &nbsp;<asp:Label ID="TitoloFinestraLabel" runat="server" Style="color: #00156E; width: 300px"
                                                   Font-Bold="True" Text=" Resetta Password" />
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
                                               <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                   border: 0px solid #5D8CC9;">
                                                   <table style="width: 100%">
                                                       <tr>
                                                           <td>
                                                               <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="Utenti" />
                                                           </td>
                                                           <td style=" width:70px">
                                                               <table style=" width:100%">
                                                                   <tr>
                                                                       <td align="right" style="width: 20px">
                                                                           <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                               Style="display: none" />
                                                                       </td>
                                                                       <td align="right" style="width: 30px">
                                                                           <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                               ToolTip="Seleziona utente..." ImageAlign="AbsMiddle" />
                                                                       </td>
                                                                       <td align="right" style="width: 30px">
                                                                           <asp:ImageButton ID="EliminaUtenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                               ToolTip="Cancella utenti selezionati" ImageAlign="AbsMiddle" />
                                                                       </td>
                                                                   </tr>
                                                               </table>
                                                           </td>
                                                       </tr>
                                                   </table>
                                                   <table style="width: 100%">
                                                       <tr>
                                                           <td>
                                                               <telerik:RadListBox ID="UtentiListBox" runat="server" Skin="Office2007" Style="width: 600px;
                                                                   height: 200px" Height="200px" SortCaseSensitive="False" CheckBoxes="true" Sort="Ascending">
                                                               </telerik:RadListBox>
                                                           </td>
                                                       </tr>
                                                   </table>
                                                   <table style="width: 100%">
                                                       <tr>
                                                           <td style="width: 90px">
                                                               <asp:Label ID="PasswordLabel" runat="server" CssClass="Etichetta" Text="Password" />
                                                           </td>
                                                           <td>
                                                               <telerik:RadTextBox ID="PasswordTextBox" runat="server" Skin="Office2007" Width="250px" />
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
                               <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                   border-top: 1px solid  #9ABBE8; height: 25px">
                                   <telerik:RadButton ID="SalvaButton" runat="server" Text="Salva" Width="80px" Skin="Office2007"
                                       ToolTip="Salva impostazioni password">
                                       <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                   </telerik:RadButton>
                               </td>
                           </tr>
                       </table>
                   </td>
               </tr>
           </table>
            

       </center>

       </div>
        <asp:HiddenField ID="infoOperazioneHidden" runat="server" />


   </ContentTemplate>
   </asp:UpdatePanel>

</asp:Content>

