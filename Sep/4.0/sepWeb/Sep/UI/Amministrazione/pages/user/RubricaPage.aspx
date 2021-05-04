<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="RubricaPage.aspx.vb" Inherits="RubricaPage" %>

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

  

    <telerik:RadTabStrip runat="server" ID="DatiRubricaTabStrip" SelectedIndex="1" MultiPageID="DatiRubricaMultiPage"
        Skin="Office2007" Width="100%">
        <Tabs>
           <telerik:RadTab Text="Anagrafica" Selected="True" />
            <telerik:RadTab Text="Dati aggiuntivi"  />
        </Tabs>
    </telerik:RadTabStrip>
    <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
             <telerik:RadMultiPage runat="server" ID="DatiRubricaMultiPage" SelectedIndex="1"
                 Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">


                 <telerik:RadPageView runat="server" ID="AnagraficaPageView" CssClass="corporatePageView"
                     Height="320px">

                     <asp:Button runat="server" ID= "DisabilitaPulsantePredefinito" style=" width:0px; height:0px; left:-1000px; position:absolute" />

                       <div  id="PannelloAnagrafica" runat="server" style="padding: 2px 2px 2px 2px;">

                           <table style="width: 100%">
                               <tr>
                                   <td style="width: 80px">
                                       <asp:Label ID="TipoPersonaLabel" runat="server" CssClass="Etichetta" Text="Tipo Pers. *"
                                           ForeColor="#FF8040" />
                                   </td>
                                   <td style="width: 90px">
                                       <telerik:RadComboBox ID="TipoPersonaComboBox" runat="server" EmptyMessage="- Seleziona -"
                                           Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                           Width="90px" />
                                   </td>
                                   <td style="width: 125px; text-align: center">
                                       <asp:Label ID="DenominazioneLabel" runat="server" CssClass="Etichetta" Text="Denom./Cognome *"
                                           ForeColor="#FF8040" />
                                   </td>
                                   <td style="width: 240px">
                                       <telerik:RadTextBox ID="DenominazioneTextBox" runat="server" Skin="Office2007" Width="250px"
                                           MaxLength="200" />
                                   </td>
                                   <td style="width: 50px; text-align: center">
                                       <asp:Label ID="NomeLabel" runat="server" CssClass="Etichetta" Text="Nome" ForeColor="#FF8040" />
                                   </td>
                                   <td style="width: 230px">
                                       <telerik:RadTextBox ID="NomeTextBox" runat="server" Skin="Office2007" Width="240px"
                                           MaxLength="100" />
                                   </td>
                               </tr>
                           </table>

                           <table style="width: 100%">
                               <tr>
                                   <td style="width: 150px">
                                       <asp:Label ID="ComuneNascitaLabel" runat="server" CssClass="Etichetta" Text="Comune/Stato Nascita" />
                                   </td>
                                   <td style="width: 250px">
                                       <telerik:RadTextBox ID="ComuneNascitaTextBox" runat="server" Skin="Office2007" Width="170px"
                                           MaxLength="40" ToolTip="Digitare parola chiave (INVIO)" />
                                       <telerik:RadTextBox ID="CapNascitaTextBox" runat="server" Skin="Office2007" Width="45px"
                                           ToolTip="C.A.P. del comune di nascita" MaxLength="5" Style="display: none" />
                                       <telerik:RadTextBox ID="ProvinciaNascitaTextBox" runat="server" Skin="Office2007"
                                           ToolTip="Provincia del comune di nascita" Width="30px" MaxLength="2" /><telerik:RadTextBox
                                               ID="CodiceIstatTextBox" runat="server" Skin="Office2007" Width="215px" MaxLength="40"
                                               Style="display: none" />
                                       <asp:ImageButton ID="TrovaComuneNascitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                           ToolTip="Seleziona comune di nascita..." ImageAlign="AbsMiddle" />
                                       <asp:ImageButton ID="EliminaComuneNascitaImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                           ToolTip="Cancella comune di nascita" ImageAlign="AbsMiddle" /><asp:ImageButton ID="AggiornaComuneNascitaImageButton"
                                               runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                                   </td>
                                   <td style="width: 90px; text-align: center">
                                       <asp:Label ID="DataNascitaLabel" runat="server" CssClass="Etichetta" Text="Data Nascita" />
                                   </td>
                                   <td style="width: 120px">
                                       <telerik:RadDatePicker ID="DataNascitaTextBox" Skin="Office2007" Width="110px" runat="server"
                                           MinDate="1753-01-01" ToolTip="Data di nascita">
                                           <Calendar runat="server">
                                               <SpecialDays>
                                                   <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                               </SpecialDays>
                                           </Calendar>
                                           <DatePopupButton ToolTip="Apri il calendario." />
                                       </telerik:RadDatePicker>
                                   </td>
                                   <td style="width: 50px">
                                       <asp:Label ID="SessoLabel" runat="server" CssClass="Etichetta" Text="Sesso" />
                                   </td>
                                   <td>
                                       <telerik:RadComboBox ID="SessoComboBox" runat="server" Skin="Office2007" Width="50px"
                                           EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" />
                                   </td>
                               </tr>
                           </table>

                           <table style="width: 100%">
                               <tr>
                                   <td style="width: 120px">
                                       <asp:Label ID="LocalitaEsteraNascitaLabel" runat="server" CssClass="Etichetta" Text="Località Est. Nasc." />
                                   </td>
                                   <td style="width: 330px">
                                       <telerik:RadTextBox ID="LocalitaEsteraNascitaTextBox" runat="server" Skin="Office2007"
                                           Width="330px" MaxLength="100" ToolTip="Località estera di nascita" />
                                   </td>
                                   <td style="width: 100px; text-align: center">
                                       <asp:Label ID="CodiceFiscaleLabel" runat="server" CssClass="Etichetta" Text="Codice Fiscale" />
                                   </td>
                                   <td>
                                       <telerik:RadTextBox ID="CodiceFiscaleTextBox" runat="server" Skin="Office2007" Width="230px"
                                           MaxLength="16" />&nbsp;<telerik:RadButton ID="CalcolaCodiceFiscaleButton" runat="server"
                                               Text="Calcola" Width="60px" Skin="Office2007" />
                                   </td>
                               </tr>
                           </table>

                           <table style="width: 100%">
                               <tr>
                                   <td style="width: 45px">
                                       <asp:Label ID="EmailLabel" runat="server" CssClass="Etichetta" Text="E-mail" ForeColor="#FF8040" />
                                   </td>
                                   <td style="width: 320px">
                                       <telerik:RadTextBox ID="EmailTextBox" runat="server" Skin="Office2007" Width="320px"
                                           MaxLength="250" />
                                   </td>
                                   <td style="width: 70px; text-align: center">
                                       <asp:Label ID="TelefonoLabel" runat="server" CssClass="Etichetta" Text="Telefono"
                                           ForeColor="#FF8040" />
                                   </td>
                                   <td style="width: 200px">
                                       <telerik:RadTextBox ID="TelefonoTextBox" runat="server" Skin="Office2007" Width="200px"
                                           MaxLength="50" />
                                   </td>
                                   <td style="width: 40px; text-align: center">
                                       <asp:Label ID="FaxLabel" runat="server" CssClass="Etichetta" Text="Fax" />
                                   </td>
                                   <td>
                                       <telerik:RadTextBox ID="FaxTextBox" runat="server" Skin="Office2007" Width="200px"
                                           MaxLength="25" />
                                   </td>
                               </tr>
                           </table>


                           <table style="width: 100%">
                               <tr>
                                   <td style="width: 130px">
                                       <asp:Label ID="DocumentoIdentitaLabel" runat="server" CssClass="Etichetta" Text="Documento" />&nbsp;
                                       &nbsp;&nbsp;
                                       <asp:Label ID="TipoDocumentoIdentitaLabel" runat="server" CssClass="Etichetta" Text="Tipo" />
                                   </td>
                                   <td style="width: 100px">
                                       <telerik:RadComboBox ID="TipoDocumentoIdentitaComboBox" runat="server" EmptyMessage="- Seleziona Tipo -"
                                           Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                           Width="150px" />
                                   </td>
                                   <td style="width: 60px">
                                       <asp:Label ID="NumeroDocumentoIdentitaLabel" runat="server" CssClass="Etichetta"
                                           Text="Numero" />
                                   </td>
                                   <td style="width: 205px">
                                       <telerik:RadTextBox ID="NumeroDocumentoIdentitaTextBox" runat="server" Skin="Office2007"
                                           Width="210px" MaxLength="20" />
                                   </td>
                                   <td style="width: 90px">
                                       <asp:Label ID="DocumentoIdentitaEnteRilascioLabel" runat="server" CssClass="Etichetta"
                                           Text="Rilasciato Da" />
                                   </td>
                                   <td style="width: 190px">
                                       <telerik:RadTextBox ID="DocumentoIdentitaEnteRilascioTextBox" runat="server" Skin="Office2007"
                                           Width="210px" MaxLength="50" />
                                   </td>
                               </tr>
                           </table>
                           <table style="width: 100%">
                               <tr>
                                   <td style="width: 125px">
                                       <asp:Label ID="IndirizzoResidenzaLabel" runat="server" CssClass="Etichetta" Text="Indirizzo Residenza"
                                           ForeColor="#FF8040" />
                                   </td>
                                   <td style="width: 260px">
                                       <telerik:RadTextBox ID="IndirizzoResidenzaTextBox" runat="server" Skin="Office2007"
                                           Width="240px" MaxLength="200" />
                                   </td>
                                   <td style="width: 130px">
                                       <asp:Label ID="ComuneResidenzaLabel" runat="server" CssClass="Etichetta" Text="Comune Residenza"
                                           ForeColor="#FF8040" />
                                   </td>
                                   <td style="width: 170px">
                                       <telerik:RadTextBox ID="ComuneResidenzaTextBox" runat="server" Skin="Office2007"
                                           Width="170px" MaxLength="40" ToolTip="Digitare parola chiave (INVIO)" />
                                   </td>
                                   <td style="width: 65px">
                                       <telerik:RadTextBox ID="CapResidenzaTextBox" runat="server" ToolTip="C.A.P. del comune di residenza"
                                           Skin="Office2007" Width="65px" MaxLength="10" />
                                   </td>
                                   <td style="width: 30px">
                                       <telerik:RadTextBox ID="ProvinciaResidenzaTextBox" ToolTip="Provincia del comune di residenza"
                                           runat="server" Skin="Office2007" Width="30px" MaxLength="2" />
                                   </td>
                                   <td style="width: 20px">
                                       <asp:ImageButton ID="TrovaComuneResidenzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                           ToolTip="Seleziona comune di residenza..." ImageAlign="AbsMiddle" />
                                   </td>
                                   <td>
                                       <asp:ImageButton ID="EliminaComuneResidenzaImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                           ToolTip="Cancella comune di residenza" ImageAlign="AbsMiddle" />
                                       <asp:ImageButton ID="AggiornaComuneResidenzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                           Style="display: none" />
                                   </td>
                               </tr>
                           </table>

                             <table style="width: 100%">

                                 <tr>
                                     <td style="width: 125px">
                                         <asp:Label ID="LocalitaEsteraResidenzaLabel" runat="server" 
                                             CssClass="Etichetta" Text="Località Est. Resid." />
                                     </td>
                                     <td>
                                         <telerik:RadTextBox ID="LocalitaEsteraResidenzaTextBox" runat="server" 
                                             MaxLength="100" Skin="Office2007" Width="330px" 
                                             ToolTip="Località estera di residenza" />
                                     </td>
                                 </tr>

                             </table>

                           <table style="width: 100%">
                               <tr>
                                   <td style="width: 125px">
                                       <asp:Label ID="IndirizzoUfficioLabel" runat="server" CssClass="Etichetta" Text="Indirizzo Ufficio" />
                                   </td>
                                   <td style="width: 260px">
                                       <telerik:RadTextBox ID="IndirizzoUfficioTextBox" runat="server" Skin="Office2007"
                                           Width="240px" MaxLength="200" />
                                   </td>
                                   <td style="width: 130px">
                                       <asp:Label ID="ComuneUfficoLabel" runat="server" CssClass="Etichetta" Text="Comune Ufficio" />
                                   </td>
                                   <td style="width: 170px">
                                       <telerik:RadTextBox ID="ComuneUfficioTextBox" runat="server" Skin="Office2007" Width="170px"
                                           MaxLength="40" ToolTip="Digitare parola chiave (INVIO)" />
                                   </td>
                                   <td style="width: 65px">
                                       <telerik:RadTextBox ID="CapUfficioTextBox" ToolTip="C.A.P. del comune dell'ufficio"
                                           runat="server" Skin="Office2007" Width="65px" MaxLength="10" />
                                   </td>
                                   <td style="width: 30px">
                                       <telerik:RadTextBox ID="ProvinciaUfficioTextBox" ToolTip="Provincia del comune dell'ufficio"
                                           runat="server" Skin="Office2007" Width="30px" MaxLength="2" />
                                   </td>
                                   <td style="width: 20px">
                                       <asp:ImageButton ID="TrovaComuneUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                           ToolTip="Seleziona comune ufficio..." ImageAlign="AbsMiddle" />
                                   </td>
                                   <td>
                                       <asp:ImageButton ID="EliminaComuneUfficioImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                           ToolTip="Cancella comune ufficio" ImageAlign="AbsMiddle" />
                                       <asp:ImageButton ID="AggiornaComuneUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                           Style="display: none" />
                                   </td>
                               </tr>
                           </table>


                     <table style="width: 100%">
                         <tr style="height: 100px">
                             <td style="width: 125px">
                                 <asp:Label ID="TipoReferenteLabel" runat="server" CssClass="Etichetta" Text="Tipo Referente"
                                     ForeColor="#FF8040" />
                             </td>
                             <td style="width: 260px">
                                 <telerik:RadListBox ID="TipoReferenteListBox" runat="server" Skin="Office2007" Style="width: 240px;
                                     height: 100px" Height="100px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True"
                                      />
                             </td>
                             <td valign="top">
                                 <table style="width: 100%">
                                   
                                   
                                    <%-- <tr>
                                         <td style="width: 130px">
                                             <asp:Label ID="AziendaLabel" runat="server" CssClass="Etichetta" Text="Ragione Sociale" ForeColor="#FF8040" />
                                         </td>
                                         <td>
                                             <telerik:RadTextBox ID="AziendaTextBox" runat="server" Skin="Office2007" Width="280px"
                                                 MaxLength="200" />
                                         </td>
                                     </tr>--%>
                                     <tr>
                                         <td style="width: 130px">
                                             <asp:Label ID="PartitaIvaLabel" runat="server" CssClass="Etichetta" 
                                                 Text="Partita IVA" ForeColor="#FF8040" />
                                         </td>
                                         <td>
                                             <telerik:RadTextBox ID="PartitaIvaTextBox" runat="server" Skin="Office2007" Width="280px"
                                                 MaxLength="11" />
                                         </td>
                                     </tr>
                                     <tr>
                                         <td style="width: 130px">
                                             <asp:Label ID="IbanLabel" runat="server" CssClass="Etichetta" Text="IBAN" />
                                         </td>
                                         <td>
                                             <telerik:RadTextBox ID="IbanTextBox" runat="server" Skin="Office2007" Width="280px"
                                                 MaxLength="27" />
                                         </td>
                                     </tr>
                                 </table>
                             </td>
                         </tr>
                     </table>

                     </div>

                 </telerik:RadPageView>


                 <telerik:RadPageView runat="server" ID="AziendaPageView" CssClass="corporatePageView"
                     Height="320px">

                        <div  id="PannelloAzienda" runat="server" style="padding: 2px 2px 2px 2px;">
                   
                     <table style="width: 100%">
                         <tr>
                             <td>
                                 <asp:Label ID="CellulareLabel" runat="server" CssClass="Etichetta" Text="Cellulare" />
                             </td>
                             <td>
                                 <telerik:RadTextBox ID="CellulareTextBox" runat="server" Skin="Office2007" Width="280px"
                                     MaxLength="25" />
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="Titolo" />
                             </td>
                             <td>
                                 <telerik:RadTextBox ID="TitoloTextBox" runat="server" Skin="Office2007" Width="280px"
                                     MaxLength="50" />
                             </td>
                             <td>
                                 <asp:Label ID="SitoWebLabel" runat="server" CssClass="Etichetta" Text="Sito WEB" />
                             </td>
                             <td>
                                 <telerik:RadTextBox ID="SitoWebTextBox" runat="server" MaxLength="50" Skin="Office2007"
                                     Width="280px" />
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:Label ID="NumeroIscrizioneAlboLabel" runat="server" CssClass="Etichetta" 
                                     Text="Num. Iscr. Albo" />
                             </td>
                             <td>
                                 <telerik:RadTextBox ID="NumeroIscrizioneAlboTextBox" runat="server" MaxLength="6"
                                     Skin="Office2007" Width="60px" />&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                 <asp:Label ID="ProvinciaAlboLabel" runat="server" CssClass="Etichetta" 
                                     Text="Provincia Albo" />&nbsp;
                                 &nbsp;
                                 <telerik:RadTextBox ID="ProvinciaAlboTextBox" runat="server" MaxLength="2" Skin="Office2007"
                                     Width="50px" />
                             </td>
                             <td>
                                 <asp:Label ID="AlboProfessionaleLabel" runat="server" CssClass="Etichetta" 
                                     Text="Albo Professionale" />
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="AlboProfessionaleComboBox" runat="server" EmptyMessage="- Selezionare -"
                                     Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                     Width="280px" />
                             </td>
                         </tr>
                     </table>

                     </div>

                 </telerik:RadPageView>
             </telerik:RadMultiPage>
   
   
 

     

      

       <table style="width: 900px; background-color: #BFDBFF; border: 0 solid #00156E">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="height: 20px">
                                    &nbsp;<asp:Label ID="TitoloElencoReferentiLabel" runat="server" Font-Bold="True"
                                        Style="width: 390px; color: #00156E; background-color: #BFDBFF" Text="Elenco referenti esterni" />
                                </td>
                               <%-- <td>
                                    <div id="ZoneID1">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 20px">
                                                    <asp:RadioButton Text="&nbsp;" Checked="true" AutoPostBack="false" ID="VisualizzaColonnaCognomeDenominazioneRadioButton"
                                                        GroupName="Tipologia" runat="server" CssClass="Etichetta" />
                                                </td>
                                                <td style="width: 240px">
                                                    <asp:Label ID="VisualizzaColonnaCognomeDenominazioneLabel" runat="server" Text="Visualizza Cognome/Denominazione"
                                                        CssClass="Etichetta" Style="color: #00156E" />
                                                </td>
                                                <td style="width: 20px">
                                                    <asp:RadioButton Text="&nbsp;" AutoPostBack="false" ID="VisualizzaColonnaRagioneSocialeRadioButton"
                                                        GroupName="Tipologia" runat="server" Style="color: #00156E" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="VisualizzaColonnaRagioneSocialeLabel" runat="server" Text="Visualizza Ragione Sociale"
                                                        CssClass="Etichetta" Style="color: #00156E" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>--%>
                            </tr>
                        </table>
                    </td>
                   

                </tr>
                <tr>
                    <td>

                    <div style="width:100%; height:300px; background-color:#FFFFFF">
                 
                     <telerik:RadGrid ID="RubricaGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007" Width="895px" AllowSorting="True"  Culture="it-IT">
                            <MasterTableView DataKeyNames="Id">
                               
                                <Columns>

                                   
                                      <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" 
                                           FilterControlAltText="Filter Id column" HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False">
                                      </telerik:GridBoundColumn>


                                 

                                          <telerik:GridTemplateColumn  SortExpression="Denominazione" UniqueName="Denominazione" HeaderText="Cognome/Denominazione" DataField="Denominazione" HeaderStyle-Width="190px" ItemStyle-Width="190px">    
                                           <ItemTemplate >   
                                             <div title='<%# Eval("Denominazione")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:190px; border:0px solid red;" ><%# Eval("Denominazione")%></div>
                                           </ItemTemplate>    
                                      </telerik:GridTemplateColumn> 


                                          <telerik:GridTemplateColumn  SortExpression="Azienda" UniqueName="Azienda" HeaderText="Ragione Sociale" DataField="Azienda" HeaderStyle-Width="190px" ItemStyle-Width="190px" Visible="false">    
                                           <ItemTemplate >   
                                             <div title='<%# Eval("Azienda")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:190px; border:0px solid red;" ><%# Eval("Azienda")%></div>
                                           </ItemTemplate>    
                                      </telerik:GridTemplateColumn> 


                                           <telerik:GridTemplateColumn  SortExpression="Nome" UniqueName="Nome" HeaderText="Nome" DataField="Nome" HeaderStyle-Width="120px" ItemStyle-Width="120px">    
                                           <ItemTemplate >   
                                             <div title='<%# Eval("Nome")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:120px; border:0px solid red;" ><%# Eval("Nome")%></div>
                                           </ItemTemplate>    
                                      </telerik:GridTemplateColumn> 


                                       <telerik:GridTemplateColumn  SortExpression="Email" UniqueName="Email" HeaderText="Email" DataField="Email" HeaderStyle-Width="145px" ItemStyle-Width="145px">    
                                           <ItemTemplate >   
                                             <div title='<%# Eval("Email")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:145px;border:0px solid red;" ><%# Eval("Email")%></div>
                                           </ItemTemplate>    
                                      </telerik:GridTemplateColumn> 

                                      <telerik:GridTemplateColumn  SortExpression="Comune" UniqueName="Comune" HeaderText="Città" DataField="Comune" HeaderStyle-Width="100px" ItemStyle-Width="100px">    
                                           <ItemTemplate >   
                                             <div title='<%# Eval("Comune")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:100px;border:0px solid red;" ><%# Eval("Comune")%></div>
                                           </ItemTemplate>    
                                      </telerik:GridTemplateColumn> 
                                  
                                      <telerik:GridTemplateColumn  SortExpression="Indirizzo" UniqueName="Indirizzo" HeaderText="Indirizzo" DataField="Indirizzo" HeaderStyle-Width="100px" ItemStyle-Width="100px">    
                                           <ItemTemplate >   
                                             <div title='<%# Eval("Indirizzo")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:100px;border:0px solid red;" ><%# Eval("Indirizzo")%></div>
                                           </ItemTemplate>    
                                      </telerik:GridTemplateColumn> 


                                        <telerik:GridTemplateColumn  SortExpression="PartitaIva" UniqueName="PartitaIva" HeaderText="P. IVA" DataField="PartitaIva" HeaderStyle-Width="70px" ItemStyle-Width="70px">    
                                           <ItemTemplate >   
                                             <div title='<%# Eval("PartitaIva")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:70px;border:0px solid red;" ><%# Eval("PartitaIva")%></div>
                                           </ItemTemplate>    
                                      </telerik:GridTemplateColumn>

                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                        ImageUrl="~\images\checks.png" UniqueName="Select" HeaderStyle-Width="20px" ItemStyle-Width="20px"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle"
                                        ItemStyle-HorizontalAlign="Center" />

                                    <telerik:GridButtonColumn Text="Seleziona referente e chiudi" FilterControlAltText="Filter ConfirmSelectAndClose column"
                                        ImageUrl="~/images/accept.png" UniqueName="ConfirmSelectAndClose" ButtonType="ImageButton"
                                        CommandName="ConfirmSelectAndClose" HeaderStyle-Width="20px" ItemStyle-Width="20px"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle"
                                        ItemStyle-HorizontalAlign="Center" />

                                    <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                        UniqueName="Copy" ButtonType="ImageButton" CommandName="Copy" HeaderStyle-Width="20px"
                                        ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                        ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" />

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

            </div>
              <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
