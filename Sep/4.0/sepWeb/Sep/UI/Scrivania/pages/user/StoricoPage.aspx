<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="StoricoPage.aspx.vb" Inherits="StoricoPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");

        var hide = true;

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
            $get("pageContent").appendChild(_backgroundElement);
            $get("pageContent").appendChild(overlay);

            if (hide) {
                HidePanel();

            } else {
                ShowPanel();
            }
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
                // _backgroundElement.className = "modalBackground";
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
            _backgroundElement.style.display = 'none';
         }

         function ShowPanel() {
            overlay.style.display = '';
            var panel = document.getElementById("printPanel");
            panel.style.display = '';
            panel.style.position = 'absolute';
            overlay.style.position = 'absolute';
            overlay.style.left = '0px';
            overlay.style.top = '0px';
            overlay.style.width = '100%';
            overlay.style.height = '100%';
             overlay.style.zIndex = 10000;
            overlay.style.backgroundColor = '#09718F';
            overlay.style.filter = "alpha(opacity=20)";
            overlay.style.opacity = "0.2";

            var shadow = document.getElementById("containerPanel");

            shadow.style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
            shadow.style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
            shadow.style.boxShadow = "3px 3px 4px #333";
            shadow.style.mozBoxShadow = "3px 3px 4px #333";
            shadow.style.webkitBoxShadow = "3px 3px 4px #333";

        }

        function onItemChecked(sender, e) {
            var item = e.get_item();
            var items = sender.get_items();
            var checked = item.get_checked();
            var firstItem = sender.getItem(0);
            if (item.get_text() == "Seleziona Tutto") {
                items.forEach(function (itm) { itm.set_checked(checked); });
            }
            else {
                if (sender.get_checkedItems().length == items.get_count() - 1) {
                    firstItem.set_checked(!firstItem.get_checked());
                }
            }
        }

            
    </script>
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>

             <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px;z-index:2000000">
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
        <asp:UpdatePanel ID="Panello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <center>
                    <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
                    <table style="width: 100%; height: 600px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td valign="top">
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                    height: 100%">                                                                
                                    <tr style="height: 20px">                                      
                                              <td valign="top"  style=" border-bottom:1px solid #9ABBE8">
                                            <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                                <tr>
                                                    <td style="width:140px">
                                                        <asp:Label ID="ElencoIstanzaLabel" runat="server" Font-Bold="True" Style="width: 140px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Istanze" />
                                                    </td>
                                                    <td style="width:40px">
                                                        <asp:Label ID="FiltriLabel" runat="server" Font-Bold="True" Style="width: 40px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Filtri" /> 
                                                    </td>
                                                    <td>
                                                        <table id="FilterContainerTable" runat="server" cellpadding="1" cellspacing="1" style= "height:20px; border : 0px solid red; width:100%" >
                                                            <tr>                
                                                               <td align="left">
                                                                    <asp:Repeater ID="ToolBarFiltri" runat="server">
                                                            <HeaderTemplate>

                                                                <table style="height: 100%;" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <td align="left">
                                                                    <table cellpadding="1" cellspacing="1" style="border: 1px solid #5A7892; height: 16px;
                                                                        -webkit-border-radius: 0.5em; -moz-border-radius: 0.5em; border-radius: 0.5em; background-color:#FFCB61">
                                                                        <tr>
                                                                            <td style="width: 16px" style="vertical-align:middle">
                                                                                <asp:Label ID="DescrizioneFiltroLabel" style="color:#00156E; font-size:10px; font-family:Verdana; vertical-align:middle; width:70px" runat="server" 
                                                                                    Text='<%# DataBinder.Eval(Container.DataItem, "Descrizione").Tostring %>' 
                                                                                    Tooltip='<%# DataBinder.Eval(Container.DataItem, "Tooltip").Tostring %>'/>
                                                                            </td>
                                                                            <td style="width: 16px" align="right" >
                                                                                <asp:ImageButton ImageAlign="AbsMiddle" ID="CancellaFiltroImage" runat="server" ImageUrl="~/Images/filterClose2.png"
                                                                                    ToolTip="Annulla filtro" onmouseout="this.src='/sep/images/filterClose2.png';" onmouseover="this.src='/sep/Images/filterCloseSelected.png';"
                                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id").Tostring %>' Style="border: 0px"
                                                                                    Visible='<%# DataBinder.Eval(Container.DataItem, "Cancellabile") %>'  />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </ItemTemplate>
                                                            <SeparatorTemplate>
                                                                <td style="width: 2px">
                                                                </td>
                                                            </SeparatorTemplate>
                                                            <FooterTemplate>
                                                                </tr> </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                                </td>
                                                            </tr>
                                                        </table>     
                                                    </td>                                                    
                                                    <td style=" display:none;width:35px">
                                                        <asp:ImageButton ID="EsportaPdfButton" runat="server" ImageUrl="~/images/pdf24.png"
                                                             ToolTip="Esporta in PDF" />
                                                    </td>

                                                     <td align="center" style="width: 35px">
                                                        <asp:ImageButton ID="EsportaInExcelImageButton" Style="border: 0; width: 20px; height: 20px"
                                                            runat="server" ImageUrl="~/images//excel32.png" ToolTip="Esporta  in un file formato excel"
                                                            ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td style="width: 10px">
                                                        <asp:Image ID="SeparatorImageButton" runat="server" ImageUrl="~/images//NavigatorSeparator.png"
                                                            Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td style="width: 35px">                                                              
                                                        <asp:Image ID="FiltraImageButton" ImageAlign="AbsMiddle" 
                                                             style= " border-style: none; border-color: inherit; border-width: 0px; cursor:pointer; height: 16px;" 
                                                             runat="server" ImageUrl="~/images//search.png" ToolTip="Ricerca Avanzata..." />
                                                    </td>
                                                    <td style="width: 35px">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images//cancelSearch.png" 
                                                            Style="border-style: none; border-color: inherit; border-width: 0px; width: 18px; height: 16px;" 
                                                            ToolTip="Annulla i filtri impostati, ritorno al filtro iniziale" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" style="width: 100%; height: 100%"
                                                border="0">
                                                <tr style="background-color: #DFE8F6">
                                                    <td valign="top">                                                         
                                                         <div id="scrollPanel" runat="server" style="overflow: auto; height: 680px; border: 1px solid #5D8CC9">
                                                            <telerik:RadGrid ID="IstanzaGridView" runat="server" AutoGenerateColumns="False"
                                                                CellSpacing="0" ToolTip="Lista delle istanze" GridLines="None" Skin="Office2007" AllowSorting= "true" TabIndex="1"
                                                                Width="99.8%" AllowPaging="true" PageSize="24" Culture="it-IT" AllowFilteringByColumn="true" EnableLinqExpressions ="false">
                                                                <GroupingSettings CaseSensitive="false"/> 
                                                                <MasterTableView DataKeyNames="ID,IdDocumento" Width="100%" TableLayout="Auto">
                                                                    <NestedViewTemplate>
                                                                        <telerik:RadGrid ID="IterGridView" runat="server" AutoGenerateColumns="False" ToolTip="Lista dell'attività collegate all'istanza dell'iter"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                            AllowPaging="True">
                                                                            <MasterTableView Width="100%" TableLayout="Fixed">
                                                                                <Columns>
                                                                                    <telerik:GridBoundColumn DataField="ID" Visible="False" />
                                                                                    <telerik:GridTemplateColumn SortExpression="AttoreMittente" UniqueName="AttoreMittente" HeaderText="Utente"
                                                                                        DataField="AttoreMittente" HeaderStyle-Width="160px" ItemStyle-Width="160px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("AttoreMittente")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 160px; border: 0px solid red">
                                                                                                <%# Eval("AttoreMittente")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                   <%-- <telerik:GridTemplateColumn SortExpression="DataInizio" UniqueName="DataInizio" HeaderText="Inizio"
                                                                                        DataField="DataInizio" HeaderStyle-Width="95px" ItemStyle-Width="95px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DataInizio","{0:dd/MM/yyyy HH.mm}")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 95px; border: 1px solid red">
                                                                                                <%# Eval("DataInizio", "{0:dd/MM/yyyy HH.mm}")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>--%>
                                                                                    <telerik:GridTemplateColumn SortExpression="DataEsecuzione" UniqueName="DataEsecuzione"
                                                                                        HeaderText="Data Esecuzione" DataField="DataEsecuzione" HeaderStyle-Width="115px" ItemStyle-Width="115px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DataEsecuzione","{0:dd/MM/yyyy HH.mm}")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 115px; border: 0px solid red">
                                                                                                <%# Eval("DataEsecuzione", "{0:dd/MM/yyyy HH.mm}")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="Operazione" UniqueName="Operazione" HeaderText="Operazione"
                                                                                        DataField="Operazione" HeaderStyle-Width="220px" ItemStyle-Width="220px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Operazione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 220px; border: 0px solid red">
                                                                                                <%# Eval("Operazione")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" HeaderText="Note"
                                                                                        DataField="Note" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Replace(Eval("Note"), "'", "&#039;")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 250px; border: 0px solid red">
                                                                                                <%# Eval("Note")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                   <%-- <telerik:GridTemplateColumn SortExpression="Stato" UniqueName="Stato" HeaderText="Stato"
                                                                                        DataField="Stato" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Stato")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 80px; border: 0 solid red">
                                                                                                <%# Eval("Stato")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>--%>
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="OperatoreImage"  runat="Server"
                                                                                                ImageUrl="~/images/UserInfo16.png" ToolTip='<%# Eval("Operatore")%>' />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </NestedViewTemplate>
                                                                    <NoRecordsTemplate><div>- Nessuna Istanza Trovata -</div></NoRecordsTemplate>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="ID" UniqueName="Id" Visible="False"/>
                                                                        <%--<telerik:GridTemplateColumn HeaderStyle-Width="15px" ItemStyle-Width="15px"  AllowFiltering="false">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="ModuloImage" Style="display: inline-block" Width="15px" runat="Server"
                                                                                    ImageUrl='<%# Eval("Im") %>' ToolTip='<%# Eval("TooltipMod")%>' />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>--%>
                                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneRiferimento" UniqueName="DescrizioneRiferimento"
                                                                            HeaderText="Documento" DataField="DescrizioneRiferimento"
                                                                            AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                            ShowFilterIcon="False" FilterControlAltText="Filter DescrizioneRiferimento column" AllowFiltering="true">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Replace(Eval("DescrizioneRiferimento"), "'", "&#039;")%>' style="border: 0px solid red">
                                                                                    <%# Eval("DescrizioneRiferimento")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn SortExpression="Proponente" UniqueName="Proponente"
                                                                            HeaderText="Proponente/Destinatario" DataField="Proponente" HeaderStyle-Width="150px"
                                                                            AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                            FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true">
                                                                            <ItemTemplate>
                                                                               <%-- <div  title='<%# Replace(Eval("Interlocutore"), "'", "&#039;")%>' style="width: 150px;
                                                                                     border: 0px solid red">
                                                                                     <%# Eval("Proponente")%>
                                                                                </div>--%>

                                                                                <asp:Label  ID="InterlocutoreLabel" runat="server" style="width: 150px; border: 0px solid red">
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>


                                                                        <telerik:GridTemplateColumn SortExpression="DataInserimento" UniqueName="DataInserimento"
                                                                            HeaderText="Data Inizio" DataField="DataInserimento" HeaderStyle-Width="80px"
                                                                            ItemStyle-Width="80px" AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" 
                                                                            FilterControlWidth="100%" ShowFilterIcon="false" FilterControlAltText="Filter DataInserimento column"
                                                                            AllowFiltering="true" >
                                                                               <FilterTemplate>
                                                                                <telerik:RadDatePicker  ID="DataInserimentoTextBox" Skin="Office2007" ShowPopupOnFocus="true" DatePopupButton-Visible="false"
                                                                                    Width="100%" runat="server" MinDate="1753-01-01" ClientEvents-OnDateSelected="DateSelected" 
                                                                                    DbSelectedDate='<%# DataInserimento(Container) %>' 
                                                                                    DateInput-ClientEvents-OnKeyPress="OnDataInserimentoTextBoxKeyPressed"/>
                                                                                   <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">                                                                                      
                                                                                       <script type="text/javascript">                                                                                         
                                                                                           function OnDataInserimentoTextBoxKeyPressed(sender, args) {                                                                                             
                                                                                               if (13 == args.get_keyCode()) {
                                                                                                   var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                   var c = sender.get_textBoxValue();                                                                                                 
                                                                                                   if (Date.parse(c)) {                                                                                                       
                                                                                                       //tableView.filter("DataInserimento", c, "EqualTo");
                                                                                                       var idPicker = sender.get_element().parentNode.parentNode.children(0).id;
                                                                                                       var picker = $find(idPicker)
                                                                                                       var ddmmyyyy = c.split('/');
                                                                                                       var mmddyyyy = ddmmyyyy[1] + '/' + ddmmyyyy[0] + '/' + ddmmyyyy[2];

                                                                                                       picker.set_selectedDate(new Date(mmddyyyy));
                                                                                                       DateSelected(picker, "");
                                                                                                   }                                                                                                                                                                                            
                                                                                                   args.set_cancel(true);                                                                                                                                                                                                   
                                                                                               }
                                                                                           }

                                                                                           function DateSelected(sender, args) {
                                                                                               var tableView = $find("<%# ctype(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                               var date = FormatSelectedDate(sender);                                                                                             
                                                                                               tableView.filter("DataInserimento", date, "EqualTo");
                                                                                           }

                                                                                           function FormatSelectedDate(picker) {
                                                                                               var date = picker.get_selectedDate();
                                                                                               var dateInput = picker.get_dateInput();
                                                                                               var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
                                                                                               return formattedDate;
                                                                                           }

                                                                                       </script>
                                                                                   </telerik:RadScriptBlock>
                                                                               </FilterTemplate>
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DataInserimento","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; text-overflow: ellipsis; width:80px; border: 0px solid red">
                                                                                    <%# Eval("DataInserimento", "{0:dd/MM/yyyy}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>                            
                                                                        <telerik:GridTemplateColumn SortExpression="Stato" UniqueName="Stato" HeaderText="Stato"
                                                                            DataField="Stato" HeaderStyle-Width="150px" ItemStyle-Width="150px" AutoPostBackOnFilter="True"
                                                                            CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                            FilterControlAltText="Filter Stato column" AllowFiltering="true">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Stato")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 150px; border: 0px solid red">
                                                                                    <%# Eval("Stato")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <%-- <telerik:GridTemplateColumn SortExpression="Giorni" UniqueName="Giorni" HeaderText="Giorni"
                                                                            DataField="Giorni" HeaderStyle-Width="40px" ItemStyle-Width="40px"  AllowFiltering="false">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Giorni")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 40px; border: 0 solid red">
                                                                                    <%# Eval("Giorni")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>--%>
                                                                        <telerik:GridBoundColumn DataField="IdDocumento" Visible="false" />
                                                                        <telerik:GridBoundColumn DataField="ContatoreGenerale" Visible="False" />
                                                                        <telerik:GridBoundColumn DataField="IdModulo" Visible="false" />
                                                                        <telerik:GridButtonColumn Text="Visualizza anteprima documento..." ButtonType="ImageButton"
                                                                            ImageUrl="~/images/knob-search16.png" CommandName="Preview" UniqueName="Preview"
                                                                            HeaderStyle-Width="20px"/>                                                                       
                                                                       <%-- <telerik:GridButtonColumn Text="Stampa lo storico dell'iter" ButtonType="ImageButton"
                                                                            ImageUrl="~/images/Printer16.png" CommandName="PrintIter" UniqueName="PrintIter"
                                                                            ItemStyle-Width="20px" />--%>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/Delete16.png"
                                                                            CommandName="Delete" UniqueName="Delete" HeaderStyle-Width="20px"/>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="20px" AllowFiltering="false">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="OperatoreImage" runat="Server" ImageAlign="AbsMiddle"
                                                                                    ImageUrl="~/images/UserInfo16.png" ToolTip='<%# Eval("AttoreCorrente")%>' />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                     
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
                            </td>
                        </tr>
                    </table>
                </center>
                
                                 
                <div id="printPanel" style="position: absolute; width: 100%; text-align: center; z-index:2000000; display:none; top:200px">
                <div id="containerPanel" style="width:600px; text-align: center; background-color: #BFDBFF;margin: 0 auto">
                     <table width="600px" cellpadding="5" cellspacing="5" border="0">
                       <tr>
                           <td>
                               <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                 <%--  HEADER--%>
                                 <tr>
                                     <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 25px">
                                         <table style="width:100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Style="color: #00156E" Font-Bold="True" Text="Ricerca Avanzata Istanza" CssClass="Etichetta" />
                                                </td>
                                                <td align="right">
                                                    <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HidePanel();document.getElementById('<%= Me.ResettaFiltroButton.ClientID %>').click();"  />
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
                                                <div style="overflow: auto; height: 275px; width: 100%; background-color: #DFE8F6;
                                                    border: 0px solid #5D8CC9;">
                                                    <table style="width: 100%">
                                                       <tr>
                                                           <td style="width: 90px">
                                                               <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="Moduli" />
                                                           </td>
                                                           <td style="padding-left: 1px; padding-right: 1px">
                                                               <telerik:RadComboBox ToolTip="Moduli di SEP" ID="ModuliComboBox"
                                                                  runat="server" Skin="Office2007" Width="100%" AutoPostBack="true"
                                                                  Filter="StartsWith" MaxHeight="250px" NoWrap="True" TabIndex="1"/>
                                                           </td>
                                                       </tr>
                                                    </table>
                                                    <table style="width: 100%">
                                                       <tr>
                                                           <td style="width: 90px">
                                                               <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipologia" />
                                                           </td>
                                                           <td style="padding-left: 1px; padding-right: 1px">
                                                               <telerik:RadComboBox ToolTip="Tipologia di documento" ID="TipologiaDocumentoComboBox"
                                                                  runat="server" Skin="Office2007" Width="100%" TabIndex="2" AutoPostBack="true"
                                                                  Filter="StartsWith" MaxHeight="250px" NoWrap="True" />
                                                           </td>
                                                       </tr>
                                                    </table>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="RiferimentoFiltroLabel" runat="server" CssClass="Etichetta" Text="Riferimento" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadTextBox ID="RiferimentoFiltroTextBox" runat="server" Skin="Office2007" TabIndex="3"
                                                                    Width="100%" tooltip="Riferimento del documento" MaxLength="100"/>
                                                            </td>
                                                        </tr>
                                                    </table>                                                    
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="DestinatarioFiltroLabel" runat="server" CssClass="Etichetta" Text="Assegnato a" />                                                               
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                               <telerik:RadComboBox ID="UtentiComboBox" runat="server" Skin="Office2007" Width="100%" Tooltip="Elenco utenti che svolgono attività" TabIndex="4"
                                                                  Filter="StartsWith" MaxHeight="250px" NoWrap="True" />

                                                                 
                                                            </td>                                                            
                                                        </tr>
                                                    </table>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="DataInizioFiltroLabel" runat="server" CssClass="Etichetta" Text="Data Inizio" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 30px">
                                                                            <asp:Label ID="DataInizioIstanzaFiltroLabel" runat="server" CssClass="Etichetta" Text="da *" />
                                                                        </td>
                                                                        <td style="width: 100px">
                                                                            <telerik:RadDatePicker ID="DataInizioIstanzaFiltroTextBox" Skin="Office2007" Width="100px"
                                                                                runat="server" MinDate="1753-01-01" TabIndex="5" ToolTip="Data Inizio Istanza" AutoPostBack="true">
                                                                                <Calendar runat="server">
                                                                                    <SpecialDays>
                                                                                        <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                    </SpecialDays>
                                                                                </Calendar>
                                                                                <DatePopupButton ToolTip="Apri il calendario per selezionare Data Inizio Istanza." />
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                        <td align="center" style="width: 30px">
                                                                            <asp:Label ID="DataFineIstanzaFiltroLabel" runat="server" CssClass="Etichetta" Text="a *" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="DataFineIstanzaFiltroTextBox" Skin="Office2007" Width="100px"
                                                                                runat="server" MinDate="1753-01-01" TabIndex="6" ToolTip="Data Fine Istanza" AutoPostBack="true">
                                                                                <Calendar runat="server">
                                                                                    <SpecialDays>
                                                                                        <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                    </SpecialDays>
                                                                                </Calendar>
                                                                                <DatePopupButton ToolTip="Apri il calendario per selezionare Data Fine Istanza." />
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="StatoTakLabel" runat="server" CssClass="Etichetta" Text="Stato Task" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ToolTip="Elenco degli Stati dei Task" ID="StatoTaskComboBox"
                                                                    runat="server" Skin="Office2007" Width="100%" TabIndex="7" Filter="StartsWith"
                                                                    MaxHeight="250px" NoWrap="True" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="StatoIterLabel" runat="server" CssClass="Etichetta" Text="Stato Iter"
                                                                    />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadListBox ID="StatoIterListBox" runat="server" Skin="Office2007" Width="170px"
                                                                    TabIndex="8" ToolTip="Stato dell'iter" Height="110px" SortCaseSensitive="False"
                                                                    Sort="Ascending" CheckBoxes="True" OnClientItemChecked="onItemChecked" />
                                                            
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
                                        ToolTip="Effettua la ricerca con i filtri impostati" TabIndex="9">
                                        <Icon PrimaryIconUrl="../../../../images/check16.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>&nbsp;
                                    <telerik:RadButton ID="SalvaFiltroButton" runat="server" Text="    Salva Filtro" Width="90px"
                                        Skin="Office2007" ToolTip="Salva i filtri impostati" TabIndex="10">
                                        <Icon PrimaryIconUrl="../../../../images/save16.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>&nbsp;
                                    <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="90px"
                                        Skin="Office2007" ToolTip="Annulla i filtri impostati" TabIndex="11">
                                        <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />                                
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="ResettaFiltroButton" runat="server" Text="Resetta" Width="90px"
                                        Skin="Office2007" ToolTip="Resetta i filtri impostati"  style="display:none">
                                        <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px"  />                                
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