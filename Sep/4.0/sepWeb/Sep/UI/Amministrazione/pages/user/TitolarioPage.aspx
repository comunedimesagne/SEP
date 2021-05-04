<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="TitolarioPage.aspx.vb" Inherits="TitolarioPage" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:UpdatePanel ID="Pannello" runat="server" UpdateMode="conditional">

   

        <ContentTemplate>

          <table style="width: 900px; height:100%; border: 1px solid #5D8CC9">
          <tr>
          <td valign="top">
       

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

           



            <table style="width: 100%;  height:720px">


         

            <tr>

         

            <td style="border:1px solid #abc1de ; width:300px">
             <telerik:RadTreeView ID="ClassificazioniTreeView" runat="server" Skin="Office2007" EnableDragAndDrop="True"  EnableDragAndDropBetweenNodes="true"  style=" overflow:auto; height:720px; width:300px"   />
            </td>
            <td style="width:15px"></td>


          
            <td valign="top">
            <table style="width:100%">
             <tr>
                    <td>
                        <asp:Label ID="LivelloLabel" runat="server" CssClass="Etichetta"  Text="Livello" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="DescrizioneLivelloTextBox" runat="server" Skin="Office2007" 
                            Width="100px" ToolTip="Descrizione del livello della voce  di titolario" 
                            ReadOnly="True"  />
                           <telerik:RadTextBox ID="IdLivelloTextBox" runat="server" Skin="Office2007" Width="100px"  style="display:none"    />
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="CodificaLabel" runat="server" CssClass="Etichetta"  
                            Text="Codifica" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="CodificaTextBox" runat="server" Skin="Office2007" 
                            Width="200px"  ToolTip="Codifica della voce di titolario" 
                            MaxLength="50"  />
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta"  
                            Text="Descrizione *" ForeColor="#FF8040" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" 
                            Width="450px" ToolTip="Descrizione della voce di titolario" 
                            MaxLength="200"  />
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

