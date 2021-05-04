<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="DisabilitaPasswordUtentePage.aspx.vb" Inherits="DisabilitaPasswordUtentePage" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" Runat="Server">

 <asp:UpdatePanel ID="Pannello" runat="server">
   <ContentTemplate>
       <center>

         <table width="600px" cellpadding="5" cellspacing="5" border="0">
         <tr>
         <td>
          <b><br />ATTENZIONE:</b> dopo la pressione del tasto "OK" <b>le password "applicative"</b> di tutti gli utenti (se non è specificato nessun utente..., altrimenti di quelli
                                        'specificati'), compreso quello correntemente
		                                connesso, <b>saranno DISABILITATE</b>: pertanto, tutti gli utenti saranno obbligati,
		                                al successivo login, a modificare
		                                la propria
		                                password applicativa personale tramite l'apposita procedura!!!<br/>
                                        &nbsp;<br />
         </td>
         </tr>
         </table>
      

           <table width="600px" cellpadding="5" cellspacing="5" border="0">
               <tr>
                   <td>
                       <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                           <tr class="ContainerHeader">
                               <td>
                                   Operazioni sulle password utente
                               </td>
                           </tr>
                           <tr>
                               <td class="ContainerMargin">
                                   <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                       <tr>
                                           <td>
                                               <table>
                                                   <tr>
                                                       <td style="width: 510px">
                                                           <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="Utenti" />
                                                       </td>
                                                       <td>
                                                           <table>
                                                               <tr>
                                                                   <td align="right" style="width: 20px">
                                                                       <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                           Style="display: none" />
                                                                   </td>
                                                                   <td align="right" style="width: 20px">
                                                                       <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                           ToolTip="Seleziona utente..." />
                                                                   </td>
                                                                   <td align="right" style="width: 20px">
                                                                       <asp:ImageButton ID="EliminaUtenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                           Style="width: 16px" ToolTip="Cancella utenti selezionati" />
                                                                   </td>
                                                               </tr>
                                                           </table>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                               <telerik:RadListBox ID="UtentiListBox" runat="server" Skin="Office2007" Style="width: 600px;
                                                   height: 200px" Height="200px" SortCaseSensitive="False" CheckBoxes="true" Sort="Ascending">
                                               </telerik:RadListBox>
                                           </td>
                                       </tr>
                                       
                                       <tr class="GridFooter">
                                           <td colspan="2" align="center">
                                               <telerik:RadButton ID="SalvaButton" runat="server" Text="Salva" Width="80px" 
                                                   Skin="Office2007" ToolTip="Salva impostazioni password">
                                                   <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                               </telerik:RadButton>
                                           </td>
                                       </tr>
                                   </table>
                           </tr>
                       </table>
                   </td>
               </tr>
           </table>
       </center>
   </ContentTemplate>
   </asp:UpdatePanel>

</asp:Content>

