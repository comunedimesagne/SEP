<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="AttivaProtocolliPage.aspx.vb" Inherits="AttivaProtocolliPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:content id="Content" contentplaceholderid="MainContent" runat="Server">
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
            EnableUI(true);          
        }

        function EnableUI(state) {
            if (!state) {
                _backgroundElement.style.display = '';
                _backgroundElement.style.position = 'absolute';
                _backgroundElement.style.left = '0px';
                _backgroundElement.style.top = '0px';
                var h = document.getElementById("lyrCorpoPagina").offsetHeight;
                var w = document.getElementById("lyrCorpoPagina").offsetWidth;
                _backgroundElement.style.width = w + 'px';
                _backgroundElement.style.height = h + 'px';
                _backgroundElement.style.zIndex = 10000;              
                _backgroundElement.style.backgroundColor = '#09718F';
                _backgroundElement.style.filter = "alpha(opacity=20)";
                _backgroundElement.style.opacity = "0.2";
            }
            else {
                _backgroundElement.style.display = 'none';
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
    <asp:updateprogress runat="server" id="UpdateProgress1">
        <progresstemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px;">
                <table cellpadding="4" style="background-color: #4892FF">
                    <tr>
                        <td>
                            <div id="Div1" style="width: 300px; text-align: center; background-color: #BFDBFF;
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
        </progresstemplate>
    </asp:updateprogress>
    <telerik:radwindowmanager id="RadWindowManager1" runat="server" reloadonshow="true">
        <Windows>
            <telerik:RadWindow ID="EseguiOperazionieRadWindow" runat="server" Modal="True"
                Animation="Fade" AnimationDuration="200" Behaviors="Close" Height="510" Skin="Office2007"
                Width="640" VisibleTitlebar="True" VisibleStatusbar="False" ReloadOnShow="true" ShowContentDuringLoad="false"
                Title="Operazione">
            </telerik:RadWindow>
        </Windows>
    </telerik:radwindowmanager>
    <asp:updatepanel id="Pannello" runat="server">
        <contenttemplate>
     <div id="pageContent">
        <center>                                                     
          <table style="width: 900px; height:600px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td valign="top">
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%; height: 100%">
                        <tr style="height:20px">
                              <td valign="top">
                                 <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;" >                               
                                      <tr>
                                          <td>
                                              <asp:Label ID="FiltroTaskLabel" runat="server" Font-Bold="True" Style="width: 600px;color: #00156E; background-color: #BFDBFF" Text="Filtro" />                                                                                        
                                          </td>
                                          <td align="center" style="width: 30px">
                                              <asp:ImageButton ID="FiltraImageButton" runat="server" 
                                                  ImageUrl="~/images//search.png" 
                                                  ToolTip="Effettua la ricerca con i filtri impostati" 
                                                  Style="border-style: none; border-color: inherit; border-width: 0; width: 16px;" 
                                                  ImageAlign="AbsMiddle" />
                                          </td>
                                          <td align="center" style="width: 30">
                                              <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png" ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                          </td>                                                                                                                               
                                      </tr>
                                  </table>
                              </td>
                            </tr>

                     <tr style="height:30px">                    
                         <td valign="top">
                         <%-- INIZIO FILTRO--%>
                            <table style="width: 100%">
                               <tr>
                                   <td align="right">
                                       <asp:Label ID="AttoriScrivaniaLabel" runat="server" CssClass="Etichetta"   Text="Scrivania di" Width="75px"  />
                                   </td>
                                   <td>
                                       <telerik:RadComboBox AutoPostBack = "True" ToolTip="Titolare della scrivania" 
                                            ID="DelegheScrivaniaComboBox" runat="server" Skin="Office2007" Width="250px" 
                                            EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" 
                                            MaxHeight="200px" NoWrap="True" TabIndex="2"/>
                                       <asp:Label ID="RiferimentoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Riferimento" Width="75px" style="text-align:right" />
                                       <telerik:RadTextBox ID="RiferimentoDocumentoTextBox" runat="server" 
                                           Skin="Office2007" Width="220px" TabIndex="3" />
                                           <asp:Label ID="StatoLabel" runat="server" CssClass="Etichetta" Text="Stato"  Width="40px"/>
                                           <telerik:RadComboBox ToolTip="Stato dell'iter" ID="StatoComboBox" 
                                                       runat="server" Skin="Office2007" Width="200px" EmptyMessage="- Selezionare -" 
                                                       ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" NoWrap="True" 
                                                       TabIndex="4" />
                                   </td>                                                                                                                                                                                 
                               </tr>
                               <tr>
                                             <td align="right"></td>
                                             <td>
                                                 
                                             </td>
                               </tr></table>                                          
                        <%-- FINE FILTRO--%>                        
                       </td></tr>
                       <tr style="height:20px">
                           <td valign="top">
                               <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;" >
                                 <tr>
                                     <td>
                                         <asp:Label ID="ElTask" runat="server" Font-Bold="True" Style="width: 120px;color: #00156E; background-color: #BFDBFF" Text="Attività"/>                                                                                                                 
                                     </td>  
                                     <td>
                                      <telerik:RadButton id="Btn_Invia" tabIndex="6" onclick="Btn_Invia_Click" runat="server" 
                                                          ToolTip="Inoltra l'atto all'attore successivo dell'iter" Width="100px" 
                                                          ForeColor="Green" UseSubmitBehavior="False" onClientClick="this.disabled=true;" 
                                                          Font-Size="X-Small" Font-Names="Verdana" Text="Invia"/>
                                     <telerik:RadButton id="Btn_InCarico" tabIndex="7" onclick="Btn_InCarico_Click" 
                                                          runat="server" ToolTip="Presa in carico del protocollo" 
                                                          Width="100px" UseSubmitBehavior="False" 
                                                          onClientClick="this.disabled=true;" Font-Size="X-Small" 
                                                          Font-Names="Verdana" Text="In Carico"/> 
                                     <telerik:RadButton Width="200px" ID="ExecuteTaskButton" 
                                                          onClientClick="this.disabled=true;"  OnClientClicked="OnClientClicked" 
                                                          runat="server" Text="Smista" EnableSplitButton="true"  
                                                           ToolTip="Selezione dell'addetto a cui smistare" 
                                                          Skin="Office2007" Autopostback="False" Forecolor="Green" Font-Size="X-Small" 
                                                          Font-Names="Verdana" TabIndex="8"/>
                                                      <telerik:RadContextMenu ID="ExecuteContextMenu" runat="server" OnItemClick="OnCtx_ItemClick">
                                                            <Items/> 
                                                      </telerik:RadContextMenu> 
                                     <telerik:RadButton id="Btn_Notifica" tabIndex="9" onclick="Btn_Notifica_Click" 
                                                          runat="server" ToolTip="Notifica del protocollo" 
                                                          Width="100px" 
                                                          ForeColor="Green" UseSubmitBehavior="False" onClientClick="this.disabled=true;" 
                                                          Font-Size="X-Small" Font-Names="Verdana" Text="Notifica"/>
                                     <telerik:RadButton id="Btn_NoDestinatario" tabIndex="9" onclick="Btn_NoDestinatario_Click" 
                                                          runat="server" ToolTip="Non sono il destinatario del protocollo" 
                                                          Width="100px" 
                                                          ForeColor="Red" UseSubmitBehavior="False" onClientClick="this.disabled=true;" 
                                                          Font-Size="X-Small" Font-Names="Verdana" Text="No Destinatario"/>
                                     <telerik:RadButton id="Btn_Fine" tabIndex="9" onclick="Btn_Fine_Click" 
                                                          runat="server" ToolTip="Fine dell'iter" 
                                                          Width="100px" 
                                                          ForeColor="Red" UseSubmitBehavior="False" onClientClick="this.disabled=true;"  
                                                          Font-Size="X-Small" Font-Names="Verdana" Text="Fine"/>
                                     </td>
                                     <td>
                                     <asp:ImageButton ID="EsportaXlsBtn" runat="server" ImageUrl="~/images/excel32.png"
                                                          TabIndex="10"  ToolTip="Esporta in Excel i risultati visualizzati" Width="20px" Height="20px" 
                                                          ImageAlign="AbsMiddle" />    
                                     </td>                                                                   
                       </tr></table></td></tr>                    
                        <tr>
                            <td valign="top" class="ContainerMargin">                                  
                                <div id="scrollPanel" runat="server" style="overflow: auto; width: 100%; height:680px; background-color: #FFFFFF;  border-right: 1px solid #BFDBFF; border-left:1px solid #BFDBFF; border-bottom:1px solid #BFDBFF" >                                                                                                                                            
                                           <telerik:RadGrid ID="TaskGridView" runat="server" 
                                                   AutoGenerateColumns="False"  GridLines="None" Skin="Office2007" Width="100%" 
                                                          AllowSorting="True" Culture="it-IT" AllowMultiRowSelection="True" 
                                                   TabIndex="1">                            
                                                <MasterTableView DataKeyNames="Id,TaskCorrente,NomeFileIter,IdDocumento,IdIstanza">                                                                                   
                                                    <NoRecordsTemplate><div>Nessun Protocollo Presente</div></NoRecordsTemplate>                                                                                          
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto" AllowFiltering="False">
                                                            <HeaderTemplate>
                                                                 <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True" runat="server"/>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                 <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True" runat="server" Tooltip="Seleziona"/>
                                                            </ItemTemplate>                                                           
                                                            <ItemStyle Width="20px" />
                                                        </telerik:GridTemplateColumn>                                                                                                                                                                                                                                                                                   
                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" UniqueName="Id" Visible="False"/>  
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" Text="Visualizza il protocollo" ImageUrl="~\images\knob-search16.png" UniqueName="Preview"/>                                                                                      
                                                        <telerik:GridTemplateColumn  SortExpression="Documento" UniqueName="Documento" HeaderText="Riferimento Protocollo" DataField="Documento">    
                                                                   <ItemTemplate >   
                                                                        <div title="ATTIVITA' SCADE IL -<%# Format(Eval("DataScadenza"),"dd/MM/yyyy") %>-" style="width:430px; border: 0 solid red" ><%# Eval("Documento")%></div>
                                                                     </ItemTemplate>    
                                                        </telerik:GridTemplateColumn> 
                                                        <telerik:GridButtonColumn Text="Esegui operazione corrente" ButtonType="ImageButton" ItemStyle-HorizontalAlign="Center" ImageUrl="~/images/new/attivita.gif" CommandName="Execute" UniqueName="Execute"/>
                                                        <telerik:GridTemplateColumn SortExpression="TaskCorrente" UniqueName="TaskCorrente" HeaderText="Stato" DataField="TaskCorrente" HeaderStyle-HorizontalAlign="Center">    
                                                             <ItemTemplate>   
                                                                 <div title="PRECEDENTE <%# Eval("TaskPrecedente")%>  <-  -> SUCCESSIVO <%# Eval("TaskSuccessivo")%>" style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:110px; border: 0 solid red" ><%# Eval("TaskCorrente")%></div>
                                                             </ItemTemplate>    
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn SortExpression="Proponente" UniqueName="Proponente"
                                                             HeaderText="Destinatario" DataField="Proponente" HeaderStyle-Width="150px"
                                                             AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                             FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true" HeaderStyle-HorizontalAlign="Center">
                                                             <ItemTemplate>
                                                                  <div title='<%# Replace(Eval("Interlocutore"), "'", "&#039;")%>' style="width: 150px"><%# Eval("Proponente")%></div>
                                                             </ItemTemplate>
                                                        </telerik:GridTemplateColumn>                                                                                                                                                                     
                                                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center">    
                                                            <ItemTemplate>   
                                                                <asp:Image ID="DirezioneImage" style="display: inline-block" Width="15px" Runat="Server" ImageUrl='<%# Eval("PathDirezione") %>' ToolTip='<%# Eval("TooltipDirezione")%>' />  
                                                            </ItemTemplate>                                              
                                                        </telerik:GridTemplateColumn>                                                                                                            
                                                    <telerik:GridBoundColumn DataField="DataInizio" Visible="false"/>  
                                                    <telerik:GridBoundColumn DataField="DataScadenza" Visible="false"/> 
                                                    <telerik:GridBoundColumn DataField="TaskSuccessivo" Visible="false"/>  
                                                    <telerik:GridBoundColumn DataField="TaskPrecedente" Visible="False"/>   
                                                    <telerik:GridBoundColumn DataField="IdModello" Visible="false"/>
                                                    <telerik:GridBoundColumn DataField="NomeFileIter" Visible="false"/>
                                                    <telerik:GridBoundColumn DataField="IdDocumento" Visible="false"/> 
                                                    <telerik:GridBoundColumn DataField="IdIstanza" Visible="false"/>                                                                                                        
                                                </Columns>                                                                                                                                        
                          </MasterTableView>
                          <ClientSettings EnableRowHoverStyle="true">                                                    
                              <Selecting AllowRowSelect="true"/>
                          </ClientSettings>                                                                                                                               
     </telerik:RadGrid></div></td></tr></table></td></tr></table>
     <asp:ImageButton ID="AggiornaTaskButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
</center></div></contenttemplate>
    </asp:updatepanel>
</asp:content>