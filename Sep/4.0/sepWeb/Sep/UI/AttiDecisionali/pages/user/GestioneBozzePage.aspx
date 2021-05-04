<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneBozzePage.aspx.vb" Inherits="GestioneBozzePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">

 <script type="text/javascript">

     var _backgroundElement = document.createElement("div");
     var overlay = document.createElement("div");

     
     function pageLoad() {
         var manager = Sys.WebForms.PageRequestManager.getInstance();
         manager.add_beginRequest(OnBeginRequest);
         manager.add_endRequest(OnEndRequest);
         $get("pageContent").appendChild(_backgroundElement);
         $get("pageContent").appendChild(overlay);

     }

     function OnBeginRequest(sender, args) {
       EnableUI(false);
     }

     function OnEndRequest(sender, args) {
       EnableUI(true);
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

     function HidePanel() {
         var panel = document.getElementById("printPanel");
         panel.style.display = "none";
         overlay.style.display = 'none';
     }

     function ShowPanel() {
         overlay.style.display = '';
         var panel = document.getElementById("printPanel");

      
         panel.style.display = 'block';
         panel.style.zIndex = 60001;
         overlay.style.position = 'absolute';
         overlay.style.left = '0px';
         overlay.style.top = '0px';

         overlay.style.width = '100%';
         overlay.style.height = '100%';

         overlay.style.zIndex = 10000;
         overlay.style.backgroundColor = '#09718F';
         overlay.style.filter = "alpha(opacity=20)";
         overlay.style.opacity = "0.2";
     }


      function OnClientButtonClicking(sender, args) {
          var button = args.get_item();
          var commandName = button.get_commandName();
          if (commandName == "Stampa") {
             args.set_cancel(true);
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


      <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="StampaBozzeRadWindow" runat="server" Modal="True"
                Animation="Fade" AnimationDuration="200" Behaviors="Close" Height="170" Skin="Office2007"
                Width="470" VisibleTitlebar="True" VisibleStatusbar="False" ReloadOnShow="true"
                Title="Stampa Bozze">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>


    <asp:UpdatePanel ID="Pannello" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

           <div id="pageContent">

            <table style="width: 900px; border: 1px solid #5D8CC9">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%" OnClientButtonClicking="OnClientButtonClicking">
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
                                <td style="width: 90px">
                                    <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *"
                                        ForeColor="#FF8040" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ToolTip="Descrizione della bozza" ID="DescrizioneTextBox" runat="server"
                                        Skin="Office2007" Width="340px" />
                                </td>
                                <td style="width: 90px">
                                    <asp:Label ID="UtenteLabel" runat="server" CssClass="Etichetta" Text="Utente" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ToolTip="Utente che ha creato la bozza" ID="UtenteTextBox" runat="server"
                                        Skin="Office2007" Width="340px" ReadOnly="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 90px">
                                    <asp:Label ID="DataLabel" runat="server" CssClass="Etichetta" Text="Data" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ToolTip="Data della bozza" ID="DataTextBox" runat="server" Skin="Office2007"
                                        Width="70px" ReadOnly="True" />
                                </td>
                                <td style="width: 90px">
                                    <asp:Label ID="VisibilitaLabel" runat="server" CssClass="Etichetta" Text="Visibile a tutti" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="VisibilitaCheckBox" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />

                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9" >
                            <tr>
                                <td>
                                    <table style="width: 100%; background-color: #BFDBFF">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitoloElencoBozzeLabel" runat="server" Font-Bold="True"
                                                    Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Bozze" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                        border: 0px solid #5D8CC9;">
                                        <telerik:RadGrid ID="BozzeGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                                                  <telerik:GridButtonColumn Text="Visualizza Bozza..."  HeaderStyle-Width="20px" ItemStyle-Width="20px" ButtonType="ImageButton" CommandName="Preview"  FilterControlAltText="Filter Preview column" ImageUrl="~\images\knob-search16.png" UniqueName="Preview" />

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />

                                                    <telerik:GridTemplateColumn HeaderStyle-Width="380px" ItemStyle-Width="380px" DataField="Descrizione"
                                                        FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                        UniqueName="Descrizione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 380px; border: 0px solid red">
                                                                <%# Eval("Descrizione")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn HeaderStyle-Width="250px" ItemStyle-Width="250px" DataField="Utente"
                                                        FilterControlAltText="Filter Utente column" HeaderText="Utente" SortExpression="Utente"
                                                        UniqueName="Utente">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Utente")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                width: 250px;border: 0px solid red">
                                                                <%# Eval("Utente")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150px" ItemStyle-Width="150px" DataField="Nomefile"
                                                        FilterControlAltText="Filter Nomefile column" HeaderText="Nomefile" SortExpression="Nomefile"
                                                        UniqueName="Nomefile">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Nomefile")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 150px;border: 0px solid red">
                                                                <%# Eval("Nomefile")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>



                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px" Text="Seleziona Bozza"
                                                        ItemStyle-Width="20px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png"
                                                        UniqueName="Select" />
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





         <div id="printPanel" style="position: absolute; width: 100%;  text-align: center; display:none; top:300px">
          <div id="containerPanel" style="width: 400px; text-align: center; background-color: #BFDBFF;margin: 0 auto; ">

           <table width="100%" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                            <%--  HEADER--%>
                            <tr>
                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 25px">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="TitoloStampaLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text="Stampa Elenco Bozze" CssClass="Etichetta" />
                                        </td>
                                        <td align="right">
                                            <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HidePanel();" />
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
                                          
                                                <div style="overflow: auto; height: 90px; width: 100%; background-color: #DFE8F6;
                                                    border: 0px solid #5D8CC9;">
                                                       <%-- CONTENT--%>
                                                 <table style="width: 100%">

                                                   <tr>
                                        <td>
                                            <asp:Label ID="NumeroLabel" runat="server" CssClass="Etichetta" Text="Numero" ForeColor="#00156E"
                                                Font-Bold="true" />
                                        </td>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 30px; text-align: center;">
                                                        <asp:Label ID="NumeroInizioLabel" runat="server" CssClass="Etichetta" Text="da" ForeColor="#00156E" />
                                                    </td>
                                                    <td style="width: 120px">
                                                        <telerik:RadTextBox ID="NumeroInizioTextBox" runat="server" Skin="Office2007" Width="80px" />
                                                    </td>
                                                    <td style="width: 30px;  text-align:center">
                                                        <asp:Label ID="NumeroFineLabel" runat="server" CssClass="Etichetta" Text="a" ForeColor="#00156E" />
                                                    </td>
                                                    <td style="width:120px">
                                                        <telerik:RadTextBox ID="NumeroFineTextBox" runat="server" Skin="Office2007" Width="80px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="DataStampaLabel" runat="server" CssClass="Etichetta" Text="Data" ForeColor="#00156E"
                                                Font-Bold="true" />
                                        </td>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 30px; text-align: center;">
                                                        <asp:Label ID="DataInizioLabel" runat="server" CssClass="Etichetta" Text="da" ForeColor="#00156E" />
                                                    </td>
                                                    <td style="width: 120px">
                                                        <telerik:RadDatePicker ID="DataInizioTextBox" Skin="Office2007" Width="110px" ZIndex="70000"
                                                            runat="server" MinDate="1753-01-01">
                                                            <Calendar>
                                                                <SpecialDays>
                                                                  <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                </SpecialDays>
                                                            </Calendar>

                                                            <DatePopupButton  ToolTip="Apri il calendario." />
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                        <td style="width: 30px;  text-align:center">
                                                        <asp:Label ID="DataAFineLabel" runat="server" CssClass="Etichetta" Text="a" ForeColor="#00156E" />
                                                    </td>
                                                      <td style="width:120px">
                                                        <telerik:RadDatePicker ID="DataFineTextBox" Skin="Office2007" Width="110px" ZIndex="70000"
                                                            runat="server" MinDate="1753-01-01">
                                                              <Calendar>
                                                                <SpecialDays>
                                                                  <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                </SpecialDays>
                                                            </Calendar>
                                                            <DatePopupButton  ToolTip="Apri il calendario." />
                                                        </telerik:RadDatePicker>
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
                                </td>
                            </tr>
                            <%-- FOOTER--%>
                            <tr>
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    <telerik:RadButton ID="StampaImageButton" runat="server" Skin="Office2007" Text="Stampa"
                                                    Width="100px" ToolTip="Stampa elenco bozze">
                                                    <Icon PrimaryIconUrl="~/images/Printer16.png" />
                                                </telerik:RadButton>
                                    &nbsp;
                                      <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="100px" Skin="Office2007"
                                                    ToolTip="Chiudi la finestra">
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

      <asp:ImageButton ID="FirmaButton" runat="server" ImageUrl="~/images//knob-search16.png"
                        Style="display: none" />

       </ContentTemplate>
       
      

           

    </asp:UpdatePanel>

   







         


</asp:Content>
