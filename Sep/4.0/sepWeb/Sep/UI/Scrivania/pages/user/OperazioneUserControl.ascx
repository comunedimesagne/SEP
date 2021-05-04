<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OperazioneUserControl.ascx.vb"
    Inherits="OperazioneUserControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


 <script type="text/javascript">


     var panelIsVisible;
     var panelFatturaIsVisible;

     




     function HideControlPanel() {
         var screen = $get("screen");
         screen.style.display = 'none';
         panelIsVisible = false;
     }

     function ShowControlPanel() {
         var controlContent = $get("controlContent");
         var screen = $get("screen");
         screen.style.display = '';
         screen.style.position = 'absolute';
         screen.style.left = '0px';
         screen.style.top = '0px';
         screen.style.width = '100%';
         screen.style.height = '100%';
         screen.style.zIndex = 10000;
         screen.style.backgroundColor = '#09718F';
         screen.style.filter = "alpha(opacity=20)";
         screen.style.opacity = "0.2";

         var progress = document.createElement("div");
         controlContent.appendChild(progress);

         with (progress) {
             style.width = '200px';
             style.height = '40px';
             style.backgroundColor = '#BFDBFF';
             style.border = 'solid #4892FF 2px';
             style.position = 'absolute';
             style.left = '230px';
             style.top = '160px';
             style.zIndex = 20000;
             innerHTML = 'Attendere prego...';
             style.color = '#00156E';
             style.backgroundImage = 'url(/sep/Images/loading.gif)';
             style.backgroundPosition = '10px center';
             style.backgroundRepeat = 'no-repeat';
             style.padding = '15px 10px 15px 50px';
             style.margin = '15px 0px';
         }


         panelIsVisible = true;
     }



     function HideRecipientsPanel() {
         var panel = document.getElementById("RecipientsPanel");
         panel.style.display = "none";
         //overlay.style.display = 'none';
         //_backgroundElement.style.display = 'none';
         //HideControlPanel();
     }

     function ShowRecipientsPanel() {
         overlay.style.display = '';
         var panel = document.getElementById("RecipientsPanel");
         panel.style.display = '';
         panel.style.position = 'absolute';
         //overlay.style.position = 'absolute';
        // overlay.style.left = '0px';
        // overlay.style.top = '0px';
       //  overlay.style.width = '100%';
        // overlay.style.height = '100%';
        // overlay.style.zIndex = 10000;
        // overlay.style.backgroundColor = '#09718F';
        // overlay.style.filter = "alpha(opacity=20)";
        // overlay.style.opacity = "0.2";

         var shadow = document.getElementById("RecipientsShadowPanel");

         with (shadow) {
             style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
             style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
             style.boxShadow = "3px 3px 4px #333";
             style.mozBoxShadow = "3px 3px 4px #333";
             style.webkitBoxShadow = "3px 3px 4px #333";
         }

     }


     


 </script>
 

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">


        function OnClientClicked(sender, args) {
            if (args.IsSplitButtonClick() || !sender.get_commandName()) {
                var currentLocation = $telerik.getLocation(sender.get_element());
                var id = sender.get_element().id;
                var t = $find(id).get_element().attributes['menu'].value;
                var contextMenu = $find(t);
                contextMenu.showAt(currentLocation.x, currentLocation.y + 22);
            }
        }

    </script>

</telerik:RadCodeBlock>



   

  
  <div id="controlContent" style=" height:100%; position:relative ; background-color:#DFE8F6">


     

<table width="650px" cellpadding="5" cellspacing="5" border="0">
    <tr>
        <td>
            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                        border-top: 1px solid  #9ABBE8; height: 25px">
                        <table style="width: 100%">
                            <tr>
                           
                            <td style=" width:30px; vertical-align:middle">
                                  <img alt=""  src="../../../../Images/process.png" style=" text-align:center; border:0px" />
                            </td>
                                <td>
                                    <asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                        Text="Operazione" CssClass="Etichetta" />
                                </td>
                                <td align="right" style="vertical-align:middle; width:20px">
                                    <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="currentPanel=0;HidePanel(currentPanel);document.getElementById('<%= Me.ChiudiButton.ClientID %>').click();" />
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
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="VisualizzaDocumentoImageButton" runat="server" ImageUrl="~/images//Documento.gif"
                                                                    Style="border: 0px;" ToolTip="Visualizza documento" ImageAlign="Top" Visible="false" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="RiferimentoDocumentoLabel" runat="server" Font-Bold="True" Style="height: 100%;
                                                                    color: #00156E; background-color: #BFDBFF" Text="" CssClass="Etichetta" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:ImageButton ID="VisualizzaStoricoPraticheImageButton" runat="server" ImageUrl="~/images//Folder.png"
                                                                    Style="border: 0px" ToolTip="Visualizza storico pratiche" ImageAlign="Top" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; height: 100%; width: 100%; background-color: #DFE8F6;
                                                        border: 0px solid #5D8CC9;">
                                                        <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%; border: 0px">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;
                                                                                    height: 100%">
                                                                                    <tr>
                                                                                        <td valign="top" style="width: 60%">
                                                                                            <asp:Panel ID="NotePanel" runat="server">
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="NoteLabel" runat="server" CssClass="Etichetta" Text="Note Destinatario" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <telerik:RadTextBox ID="NoteInterneTextBox" runat="server" MaxLength="1500" Rows="7"
                                                                                                                Skin="Office2007" TextMode="MultiLine" Width="350px" ToolTip="Note da scrivere sull'operazione" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <br />
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="NoteInterneLabel" runat="server" CssClass="Etichetta" Text="Note Mittente" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <telerik:RadTextBox ID="NotePresentiTextBox" runat="server" MaxLength="1500" 
                                                                                                                Rows="7" Skin="Office2007" TextMode="MultiLine" Width="350px" 
                                                                                                                ToolTip="Note presenti dalla precedente operazione" ReadOnly="True" />
                                                                                                            <asp:HiddenField ID="EnableUiHidden" runat="server" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                        <td valign="top" style="width: 40%">
                                                                                            <asp:Panel ID="ToolbarPanel" runat="server" HorizontalAlign="Center">
                                                                                                <asp:DataList runat="server" ID="ToolbarButtonList" RepeatLayout="Table"  DataKeyField="Name"  HeaderStyle-VerticalAlign="Middle">
                                                                                                  <ItemStyle  Height="40px" />
                                                                                                    <ItemTemplate>

                                                                                                   
                                                                                                
                                                                                                 
                                                                                                        <telerik:RadButton Width="220px" ID="ExecuteTaskButton" runat="server" Text='<%# Eval("Description") %>'
                                                                                                            EnableSplitButton="true" AutoPostBack="false" OnClientClicked="OnClientClicked"
                                                                                                            ToolTip='<%# Eval("Description") %>' CommandName='<%# Eval("Name") %>' Skin="Office2007"
                                                                                                            CommandArgument='<%# Eval("Type") %>' />

                                                                                                 
                                                                                                     
                                                                                                      
                                                                                                        <telerik:RadContextMenu ID="ExecuteContextMenu" runat="server" OnItemClick="OnCtx_ItemClick">
                                                                                                            <Items>
                                                                                                            </Items>
                                                                                                        </telerik:RadContextMenu>
                                                                                                   
                                                                                                    </ItemTemplate>
                                                                                                </asp:DataList>
                                                                                            </asp:Panel>
                                                                                        </td>


                                                                                    </tr>
                                                                                   
                                                                                   
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                       <tr>
                                                                                    <td>
                                                                                      <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">

                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9" >
                            <tr>
                                <td>
                                    <table style="width: 100%; background-color: #BFDBFF">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitoloElencoNoteLabel" runat="server" Font-Bold="True"
                                                    Style="width: 100%; color: #00156E; background-color: #BFDBFF" Text="Storico Note" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: auto; height: 120px; width: 100%; background-color: #FFFFFF;
                                        border: 1px solid #5D8CC9;">
                                        <telerik:RadGrid ID="NoteGridView" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True" Culture="it-IT">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                 
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="160px" ItemStyle-Width="160px" DataField="Utente"
                                                        FilterControlAltText="Filter Utente column" HeaderText="Utente" SortExpression="Utente"
                                                        UniqueName="Utente">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Utente")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 160px; border: 0px solid red">
                                                                <%# Eval("Utente")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn SortExpression="Data" UniqueName="Data" HeaderText="Data"
                                                        DataField="DataInizio" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Data","{0:dd/MM/yyyy HH:mm}")%>' style="white-space: nowrap;
                                                                overflow: hidden; text-overflow: ellipsis; width: 90px; border: 0px solid red">
                                                                <%# Eval("Data", "{0:dd/MM/yyyy HH:mm}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn DataField="Note" FilterControlAltText="Filter Note column"
                                                        HeaderText="Note" SortExpression="Note" UniqueName="Note">
                                                        <ItemTemplate>
                                                            <div title='<%# Replace(Eval("Note"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                overflow: hidden; text-overflow: ellipsis; width: 300px; border: 0px solid red">
                                                                <%# Eval("Note")%></div>
                                                            <asp:Label ID="NoteLabel" runat="server" Text='<%# Eval("Note") %>' Visible="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                                        UniqueName="Copy" ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy"
                                                        Text="Copia note negli Appunti" />
                                                  
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
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
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
                        <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="100px" Skin="Office2007"
                            ToolTip="Chiudi la finestra" >
                            <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>



<%--PANNELLO DESTINATARI--%>


<div id="RecipientsPanel" style="position: absolute; width: 100%;  z-index: 2000000; display: none; top: 90px; left:0">
                <div id="RecipientsShadowPanel" style="width: 660px;  background-color: #BFDBFF;margin: 0 auto">
                    <table width="660px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 0px; height: 25px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloRecipientsPanelLabel" runat="server" CssClass="Etichetta"
                                                            Font-Bold="True" Style="width: 500px; color: #00156E; background-color: #BFDBFF"
                                                            Text="Elenco Destinatari" />
                                                    </td>
                                                    <td align="right">
                                                        <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HideRecipientsPanel();HideControlPanel();document.getElementById('<%= Me.ChiudiRecipientsButton.ClientID %>').click();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%-- BODY--%>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <div style="height: 200px; background-color: White">
                                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <telerik:RadListBox ID="UtentiListBox" runat="server" Skin="Office2007" Style="width: 600px;
                                                                height: 200px" Height="200px" SortCaseSensitive="False" CheckBoxes="true" Sort="Ascending">
                                                            </telerik:RadListBox>
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
                                &nbsp;
                                <telerik:RadButton ID="ConfermaRecipientsButton" runat="server" Text="Invia" Width="100px"
                                    Skin="Office2007">
                                    <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                                &nbsp; &nbsp; &nbsp;
                                <telerik:RadButton ID="ChiudiRecipientsButton" runat="server" Text="Chiudi" Width="90px"
                                    Skin="Office2007" ToolTip="Chiudi">
                                    <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>


            



<%--INIZIO CONTROLLI NASCOSTI--%>


      <asp:ImageButton ID="PubblicaOnlineButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none" />

      <asp:ImageButton ID="AggiornaIterButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none" />

      <asp:ImageButton ID="AggiornaTaskButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none" />

      <asp:ImageButton ID="AggiornaProtocolloButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none" />

      <asp:ImageButton ID="AggiornaProtocolloInserisciDocumentoFirmatoButton" runat="server"
          ImageUrl="~/images//knob-search16.png" Style="display: none" />


      <asp:ImageButton ID="firmaDocumentoButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none; width: 0px" />

      <asp:ImageButton ID="numeraDocumentoButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none; width: 0px" />

      <asp:ImageButton ID="AnnullaFirmaDocumentoButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none; width: 0px" />

      <asp:ImageButton ID="salvaDocumentoButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none; width: 0px" />

      <asp:ImageButton ID="aggiornaTaskMassiviButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none; width: 0px" />

      <asp:ImageButton ID="firmaMassivaDocumentiButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none; width: 0px" />

      <asp:ImageButton ID="PubblicaOnlineMassivaButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none" />


      <asp:HiddenField ID="documentContentHidden" runat="server" />

      <asp:HiddenField ID="signerOutputHidden" runat="server" />

      <asp:HiddenField ID="checkContentHidden" runat="server" />
     
      <asp:ImageButton ID="verificaCorpoDocumentoButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none; width: 0px" />

      <asp:ImageButton ID="verificaMassivaCorpoDocumentiButton" runat="server" ImageUrl="~/images//knob-search16.png"
          Style="display: none; width: 0px" />


<%--FINE CONTROLLI NASCOSTI--%>

      <div id="screen">
      </div>

      <asp:PlaceHolder ID="scriptHolder" runat="server"></asp:PlaceHolder>

  </div>


