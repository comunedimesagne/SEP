<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="OrganigrammaPage.aspx.vb" Inherits="OrganigrammaPage" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" Runat="Server">


    <asp:UpdatePanel ID="Pannello" runat="server" UpdateMode="conditional">

   

        <ContentTemplate>


          <table style="width: 900px; border: 1px solid #5D8CC9">
          <tr>
          <td>
          
       
        


            <table style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                            <Items>
                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                    CommandName="Nuovo" Owner="RadToolBar">
                                </telerik:RadToolBarButton>
                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                    CommandName="Trova" Owner="RadToolBar">
                                </telerik:RadToolBarButton>
                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                    CommandName="Annulla" Owner="RadToolBar">
                                </telerik:RadToolBarButton>
                                <telerik:RadToolBarButton  runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                    CommandName="Salva" Owner="RadToolBar">
                                </telerik:RadToolBarButton>
                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                    CommandName="Elimina" Owner="RadToolBar">
                                </telerik:RadToolBarButton>
                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                    CommandName="Stampa" Owner="RadToolBar">
                                </telerik:RadToolBarButton>
                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar">
                                </telerik:RadToolBarButton>
                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                    CommandName="Home" Owner="RadToolBar">
                                </telerik:RadToolBarButton>
                            </Items>
                        </telerik:RadToolBar>
                    </td>
                </tr>
            </table>

            <br />


              <table style="width:100%; height:10px">
          <tr>
             <td valign="top">

               <telerik:RadButton  ID="SpostaSuButton"  runat="server" Text="Sposta sù"  Width="100px" Skin="Office2007">
                                   <Icon PrimaryIconUrl="../../../../images/UpArrow.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>

                    <telerik:RadButton  ID="SpostaGiuButton"  runat="server" Text="Sposta giù"  Width="100px" Skin="Office2007">
                                   <Icon PrimaryIconUrl="../../../../images/DownArrow.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>
            
              </td>
           
            </tr>
           </table>


            <table style="width: 100%; height:720px">

            <tr>

         

            <td style="border:1px solid #abc1de ; width:300px">
             <telerik:RadTreeView ID="StruttureTreeView" runat="server" Skin="Office2007"  
                    style=" overflow:auto; height:100%; width:300px" EnableDragAndDrop="True" 
                    EnableDragAndDropBetweenNodes="True" 
                    
                 />

            </td>
            <td style="width:15px"></td>


          
            <td valign="top">
            <table style="width:100%">
             <tr>
                    <td>
                        <asp:Label ID="TipoStrutturaLabel" runat="server" CssClass="Etichetta"  Text="Tipo struttura" />
                    </td>
                    <td>
                         <telerik:RadComboBox ID="TipoStruttureComboBox" Runat="server" Skin="Office2007" 
                   Width="230px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" 
                         Filter="StartsWith" MaxHeight="400px" 
                          ToolTip="Tipologie di strutture" AutoPostBack="True" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="StrutturaAppartenenzaLabel" runat="server" CssClass="Etichetta"  Text="Struttura appart." />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="StrutturaAppartenenzaTextBox" runat="server" Skin="Office2007" 
                            Width="240px"  ToolTip="Struttura di appartenenza" ReadOnly="True"  />

                            &nbsp;<asp:ImageButton ID="TrovaStrutturaImageButton" runat="server" ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona struttura..." />
                            &nbsp;<asp:ImageButton ID="AggiornaStrutturaImageButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                          <telerik:RadTextBox ID="IdStrutturaTextBox" Runat="server" Skin="Office2007" Width="1px"  style=" display:none"/> 
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="UtenteLabel" runat="server" CssClass="Etichetta"  Text="Utente" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="UtenteTextBox" runat="server" Skin="Office2007" 
                            Width="240px" ToolTip="Utente" ReadOnly="True"  />
                            &nbsp;<asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona utente..." />
                            &nbsp;<asp:ImageButton ID="EliminaUtenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella utente" />
                           &nbsp;<asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                           <telerik:RadTextBox ID="IdUtenteTextBox" Runat="server" Skin="Office2007" Width="1px"  style=" display:none"/> 
                    </td>
                </tr>

                  <tr>
                    <td>
                        <asp:Label ID="ResponsabileLabel" runat="server" CssClass="Etichetta"  Text="Responsabile" />
                    </td>
                    <td>
                        <telerik:RadButton ID="ResponsabileCheckBox" runat="server" ButtonType="ToggleButton"  Skin="Office2007" Text="" ToggleType="CheckBox" />
                    </td>
                </tr>

                  <tr>
                   
                    <td>
                        <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta"  Text="Descr./ Cognome" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" 
                            Width="300px" ToolTip="Descrizione" MaxLength="200" />
                    </td>

                </tr>

            

                  <tr>
                    <td>
                        <asp:Label ID="QualificaLabel" runat="server" CssClass="Etichetta"  Text="Qualifica" />
                    </td>
                    <td>
                            <telerik:RadComboBox ID="QualificheComboBox" Runat="server" Skin="Office2007" 
                     Width="230px" EmptyMessage="Seleziona Qualifica" ItemsPerRequest="10" 
                         Filter="StartsWith" MaxHeight="400px" 
                          ToolTip="Qualifiche" />
                    </td>
                </tr>

          

           

        

                 <tr>
                    <td>
                        <asp:Label ID="IndirizzoLabel" runat="server" CssClass="Etichetta"  Text="Indirizzo" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="IndirizzoTextBox" runat="server" Skin="Office2007" 
                            Width="300px" ToolTip="Indirizzo"  MaxLength="50" />
                    </td>
                </tr>


                 <tr>
                    <td>
                        <asp:Label ID="CapLabel" runat="server" CssClass="Etichetta"  Text="Cap" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="CapTextBox" runat="server" Skin="Office2007" 
                            Width="60px" ToolTip="Cap" MaxLength="5" />
                    </td>
                </tr>

                 <tr>
                    <td>
                        <asp:Label ID="LocalitaLabel" runat="server" CssClass="Etichetta"  Text="Località" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="LocalitaTextBox" runat="server" Skin="Office2007" 
                            Width="300px" ToolTip="Località"  MaxLength="50" />
                    </td>
                </tr>


                 <tr>
                    <td>
                        <asp:Label ID="ProvinciaLabel" runat="server" CssClass="Etichetta"  Text="Provincia" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="ProvinciaTextBox" runat="server" Skin="Office2007" 
                            Width="25px" ToolTip="Sigla della provincia" MaxLength="2"  />
                    </td>
                </tr>

                 <tr>
                    <td>
                        <asp:Label ID="TelefonoLabel" runat="server" CssClass="Etichetta"  Text="Telefono" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="TelefonoTextBox" runat="server" Skin="Office2007" 
                            Width="160px" ToolTip="Telefono"  MaxLength="15" />
                    </td>
                </tr>

                 <tr>
                    <td>
                        <asp:Label ID="FaxLabel" runat="server" CssClass="Etichetta"  Text="Fax" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="FaxTextBox" runat="server" Skin="Office2007" 
                            Width="160px" ToolTip="Fax" MaxLength="15" />
                    </td>
                </tr>

                 <tr>
                    <td>
                        <asp:Label ID="EmailLabel" runat="server" CssClass="Etichetta"  Text="E-mail" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="EmailTextBox" runat="server" Skin="Office2007" 
                            Width="440px" ToolTip="E-mail"   />
                    </td>
                </tr>


                <tr>
                    <td>
                        <asp:Label ID="GruppoLabel" runat="server" CssClass="Etichetta"  Text="Gruppo" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="GruppoTextBox" runat="server" Skin="Office2007" 
                            Width="240px" ToolTip="Gruppo" ReadOnly="True"  />
                            &nbsp;<asp:ImageButton ID="TrovaGruppoImageButton" runat="server" ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona gruppo..." />
                            &nbsp;<asp:ImageButton ID="EliminaGruppoImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella gruppo" />
                           &nbsp;<asp:ImageButton ID="AggiornaGruppoImageButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                           <telerik:RadTextBox ID="idGruppoTextBox" Runat="server" Skin="Office2007" Width="1px"  style=" display:none"/> 
                    </td>
                </tr>

              

               <%-- <tr>
                    <td>
                        <asp:Label ID="CodiceIpaLabel" runat="server" CssClass="Etichetta" Text="Codice IPA" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="CodiceIpaTextBox" runat="server" Skin="Office2007" Width="160px"
                            ToolTip="Codice assegnato dall'IPA (Codice Univoco Ufficio)" MaxLength="16" />
                    </td>
                </tr>--%>

                  <tr>
                    <td>
                        <asp:Label ID="CodiceIpaLabel" runat="server" CssClass="Etichetta" Text="Codice IPA" />
                    </td>
                    <td>
                         <telerik:RadComboBox ID="CodiceUnivocoUfficioComboBox" Runat="server" Skin="Office2007" 
                     Width="440px" EmptyMessage="Seleziona Codice Univoco Ufficio" ItemsPerRequest="10" 
                         Filter="StartsWith" MaxHeight="400px" 
                          ToolTip="Codice Univoco Ufficio" />
                    </td>
                </tr>

                 <tr>
                    <td>
                        <asp:Label ID="CodiceUfficioBilancioLabel" runat="server" CssClass="Etichetta" Text="Ufficio bilancio" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="CodiceUfficioBilancioTextBox" runat="server" Skin="Office2007" Width="160px"
                            ToolTip="Codice ufficio bilancio" MaxLength="16" />
                    </td>
                </tr>

                <tr>
                <td>
                  <asp:Label ID="DescrizioneDettagliataLabel" runat="server" CssClass="Etichetta"  Text="Descrizione" />
                </td>
                 <td>
                                                                                 <telerik:RadTextBox ID="DescrizioneDettagliataTextBox" runat="server" Skin="Office2007" Width="440px"
                                                                    MaxLength="5000" ToolTip="Descrizione dettagliata" Rows="7"
                                                                    TextMode="MultiLine" Style="overflow-x: hidden" />
                </td>
                </tr>


                  <tr>
                    <td>
                        
                    </td>
                    <td>
                       <telerik:RadButton ID="EsportaButton" runat="server" Text="Esporta Procedimenti" Width="200px"
                                                Skin="Office2007" ToolTip="Effettua l'esportazione dei procedimenti associati alla struttura selezionata">
                                                <Icon PrimaryIconUrl="../../../../images/export.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                    </td>
                </tr>


            </table>

            </td>
            </tr>
               
               
            </table>

             


           
         



            <br />
 
    </td>
          </tr>
          </table>









   </ContentTemplate>
   </asp:UpdatePanel>
</asp:Content>

