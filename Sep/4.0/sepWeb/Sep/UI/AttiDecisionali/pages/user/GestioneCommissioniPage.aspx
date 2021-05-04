<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneCommissioniPage.aspx.vb" Inherits="GestioneCommissioniPage" %>

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
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Nome *"
                                                        ForeColor="#FF8040" />
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ToolTip="Nome della commissione" ID="DescrizioneTextBox" runat="server"
                                                        Skin="Office2007" Width="340px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="PresidenteLabel" runat="server" CssClass="Etichetta" Text="Presidente" />
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="PresidenteTextBox" runat="server" Skin="Office2007" Width="300px"
                                                        ReadOnly="true" ToolTip="Presidente" />
                                                    <asp:ImageButton ID="TrovaPresidenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                        ToolTip="Seleziona presidente..." ImageAlign="AbsMiddle" />
                                                    <asp:ImageButton ID="EliminaPresidenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                        ToolTip="Cancella presidente" ImageAlign="AbsMiddle" />
                                                    <asp:ImageButton ID="AggiornaPresidenteImageButton" runat="server" Style="display: none" />
                                                    <asp:TextBox ID="IdPresidenteTextBox" runat="server" Style="display: none" />
                                                </td>
                                                <td style="width: 90px">
                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="ViceLabel" runat="server" CssClass="Etichetta" Text="Vice" />
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="ViceTextBox" runat="server" Skin="Office2007" Width="310px"
                                                        ReadOnly="true" ToolTip="Vice" />
                                                    <asp:ImageButton ID="TrovaViceImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                        ToolTip="Seleziona vice..." ImageAlign="AbsMiddle" />
                                                    <asp:ImageButton ID="EliminaViceImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                        ToolTip="Cancella vice" ImageAlign="AbsMiddle" />
                                                    <asp:ImageButton ID="AggiornaViceImageButton" runat="server" Style="display: none" />
                                                    <asp:TextBox ID="IdViceTextBox" runat="server" Style="display: none" />
                                                </td>
                                            </tr>
                                        </table>


                                   
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr style="height: 20px; background-color: #BFDBFF">
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label Font-Bold="True" ID="TitoloElencoConsiglieriLabel" runat="server"
                                                                                Style="color: #00156E; background-color: #BFDBFF; width: 95%" CssClass="Etichetta"
                                                                                Text="Elenco Consiglieri" />
                                                                        </td>
                                                                        <td align="right" style="width: 20px;">
                                                                            <asp:ImageButton ID="AggiornaConsigliereImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none; width: 0" />
                                                                        </td>
                                                                        <td align="right" style="width: 20px;">
                                                                            <asp:ImageButton ID="TrovaConsigliereImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona consigliere..." ImageAlign="AbsMiddle" />
                                                                        </td>
                                                                        <td align="right" style="width: 20px;">
                                                                            <asp:ImageButton ID="EliminaConsiglieriSelezionatiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                Style="width: 16px" ToolTip="Cancella consigliere selezionati" ImageAlign="AbsMiddle" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="scrollConsiglieriPanel" runat="server" style="overflow: auto; height: 150px;
                                                                    border: 1px solid #5D8CC9; background-color: White">
                                                                    <telerik:RadGrid ID="ConsiglieriGridView" ToolTip="Elenco consiglieri associati alla commissione"
                                                                        runat="server" AllowPaging="False" AutoGenerateColumns="False" CellSpacing="0"
                                                                        GridLines="None" Skin="Office2007"  AllowSorting="True" AllowMultiRowSelection="True"
                                                                        Culture="it-IT">
                                                                        <MasterTableView DataKeyNames="IdCommissione, IdConsigliere">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="IdCommissione" DataType="System.Int32" FilterControlAltText="Filter IdCommissione column"
                                                                                    ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderText="IdCommissione" ReadOnly="True"
                                                                                    SortExpression="IdCommissione" UniqueName="IdCommissione" Visible="False" />
                                                                                <telerik:GridBoundColumn DataField="IdConsigliere" DataType="System.Int32" FilterControlAltText="Filter IdConsigliere column"
                                                                                    ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderText="IdConsigliere" ReadOnly="True"
                                                                                    SortExpression="IdConsigliere" UniqueName="IdConsigliere" Visible="False" />
                                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                                    AllowFiltering="False" ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center">
                                                                                    <HeaderTemplate>
                                                                                        <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ConsiglieriToggleSelectedState"
                                                                                            AutoPostBack="True" runat="server"></asp:CheckBox>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ConsiglieriToggleRowSelection"
                                                                                            AutoPostBack="True" runat="server"></asp:CheckBox>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn SortExpression="Nominativo" UniqueName="Nominativo" HeaderText="Nominativo"
                                                                                    DataField="Nominativo" HeaderStyle-Width="335px" ItemStyle-Width="335px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 335px; border: 0px solid red">
                                                                                            <%# Eval("Nominativo")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" ItemStyle-Width="20px" HeaderStyle-Width="20px" />
                                                                            </Columns>
                                                                        </MasterTableView></telerik:RadGrid>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 50%">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr style="height: 20px; background-color: #BFDBFF">
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label Font-Bold="True" ID="TitoloElencoUtentiLabel" runat="server" Style="color: #00156E;
                                                                                background-color: #BFDBFF; width: 95%" CssClass="Etichetta" Text="Elenco Utenti" />
                                                                        </td>
                                                                        <td align="right" style="width: 20px;">
                                                                            <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none; width: 0" />
                                                                        </td>
                                                                        <td align="right" style="width: 20px;">
                                                                            <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona utente..." ImageAlign="AbsMiddle" />
                                                                        </td>
                                                                        <td align="right" style="width: 20px;">
                                                                            <asp:ImageButton ID="EliminaUtentiSelezionatiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                Style="width: 16px" ToolTip="Cancella utenti selezionati" ImageAlign="AbsMiddle" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="scrollUtentiPanel" runat="server" style="overflow: auto; height: 150px;
                                                                    border: 1px solid #5D8CC9; background-color: White">
                                                                    <telerik:RadGrid ID="UtentiGridView" ToolTip="Elenco utenti associati alla commissione"
                                                                        runat="server" AllowPaging="False" AutoGenerateColumns="False" CellSpacing="0"
                                                                        GridLines="None" Skin="Office2007"  AllowSorting="True" AllowMultiRowSelection="True"
                                                                        Culture="it-IT">
                                                                        <MasterTableView DataKeyNames="IdCommissione, IdUtente">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="IdCommissione" DataType="System.Int32" FilterControlAltText="Filter IdCommissione column"
                                                                                    ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderText="IdCommissione" ReadOnly="True"
                                                                                    SortExpression="IdCommissione" UniqueName="IdCommissione" Visible="False" />
                                                                                <telerik:GridBoundColumn DataField="IdUtente" DataType="System.Int32" FilterControlAltText="Filter IdUtente column"
                                                                                    ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderText="IdUtente" ReadOnly="True"
                                                                                    SortExpression="IdUtente" UniqueName="IdUtente" Visible="False" />
                                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                                    AllowFiltering="False" ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center">
                                                                                    <HeaderTemplate>
                                                                                        <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="UtentiToggleSelectedState"
                                                                                            AutoPostBack="True" runat="server"></asp:CheckBox>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="UtentiToggleRowSelection" AutoPostBack="True"
                                                                                            runat="server"></asp:CheckBox>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn SortExpression="Nominativo" UniqueName="Nominativo" HeaderText="Nominativo"
                                                                                    DataField="Nominativo" >
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 325px; border: 0px solid red">
                                                                                            <%# Eval("Nominativo")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" ItemStyle-Width="20px" HeaderStyle-Width="20px" />
                                                                            </Columns>
                                                                        </MasterTableView></telerik:RadGrid></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>

                      <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                        
                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9" >
                            <tr>
                                <td>
                                    <table style="width: 100%; background-color: #BFDBFF">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitoloElencoCommissioniLabel" runat="server" Font-Bold="True"
                                                    Style="width: 800px; color: #00156E; background-color: #BFDBFF" 
                                                    Text="Elenco Commissioni" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                        border: 0px solid #5D8CC9;">
                                        <telerik:RadGrid ID="CommissioniGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True" Culture="it-IT">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                                               

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />

                                                    <telerik:GridTemplateColumn HeaderStyle-Width="830px" ItemStyle-Width="830px" DataField="Descrizione"
                                                        FilterControlAltText="Filter Descrizione column" HeaderText="Nome" SortExpression="Descrizione"
                                                        UniqueName="Descrizione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 830px; border: 0px solid red">
                                                                <%# Eval("Descrizione")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                      


                                               


                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px" Text="Seleziona Commissione" ItemStyle-Width="20px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png" UniqueName="Select" />
                                                  

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

             <asp:HiddenField ID="scrollPosConsiglieriHidden" runat="server" Value="0" />
              <asp:HiddenField ID="scrollPosUtentiHidden" runat="server" Value="0" />
              <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
    

       </ContentTemplate>
       
   

           

    </asp:UpdatePanel>

   





         


</asp:Content>
