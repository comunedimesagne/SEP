<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="AttivaModularePage.aspx.vb" Inherits="AttivaModularePage" %>
<%@ Register Src="~/UI/Scrivania/pages/user/OperazioneUserControl.ascx" TagName="OperazioneControl" TagPrefix="parsec" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");
        var hide = true;
        var updateGrid = false;

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);

            $get("pageContent").appendChild(overlay);
            $get("pageContent").appendChild(_backgroundElement);


            var hidden = $get('MainContent_OperazioneControl_EnableUiHidden')
            if (hidden) {
                var stato = $get('MainContent_OperazioneControl_EnableUiHidden').value;
                if (stato == 'Abilita') {
                    panelIsVisible = false;
                    $get('MainContent_OperazioneControl_EnableUiHidden').value = '';
                }
            }
            if (hide) {
                HidePanel();
            } else {
                ShowPanel();
            }
            if (panelIsVisible) {
                ShowControlPanel();
            } else {
                HideControlPanel();
            }
        }

        function UpdateTask() {
            document.getElementById("<%= AggiornaTaskButton.ClientID %>").click();
        }

        function HidePanel() {
           if (updateGrid) {
                document.getElementById("<%= AggiornaTaskButton.ClientID %>").click();
                updateGrid = false;
                panelIsVisible = false;
            }

            var panel = document.getElementById("taskPanel");
            panel.style.display = "none";
           overlay.style.display = 'none';
           _backgroundElement.style.display = 'none';                       
        }

        function ShowPanel() {
            overlay.style.display = '';
            var panel = document.getElementById("taskPanel");
            panel.style.display = '';
            panel.style.position = 'absolute';
            panel.style.top = 150 + "px";
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
    </script>
<asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
    <ProgressTemplate>
        <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center; top: 300px;z-index:2000000">
             <table cellpadding="4" style="background-color: #4892FF;margin: 0 auto">
                <tr>
                    <td>
                        <div id="loadingInner" style="width: 300px; text-align: center; background-color: #BFDBFF;height: 60px">
                             <span style="color: #00156E">Attendere prego ... </span><br /><br />
                             <img alt="" src="../../../../images/loading.gif" border="0">
</div></td></tr></table></div></ProgressTemplate></asp:UpdateProgress>
<asp:UpdatePanel ID="Pannello" runat="server">
    <ContentTemplate>
        <div id="pageContent">
             <center>
                 <table style="width: 900px; height: 600px" cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td valign="top">
                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;height: 100%">                                    
                               <tr style="height: 30px">
                                   <td valign="top">                                       
                                       <table style="width: 100%;border-top:1px solid #9ABBE8;">
                                          <tr>
                                              <td><asp:Label ID="AttoriScrivaniaLabel" runat="server" CssClass="Etichetta" Text="Scrivania di" Width="80"/></td>
                                              <td>
                                                  <table style="width: 100%">
                                                     <tr>
                                                         <td style="width: 420px">
                                                             <telerik:RadComboBox AutoPostBack="True" ToolTip="Titolare della scrivania" ID="DelegheScrivaniaComboBox"
                                                                  runat="server" Skin="Office2007" Width="500px" EmptyMessage="- Selezionare -"
                                                                  ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" NoWrap="True" TabIndex="1"/>
                                          </td></tr></table></td></tr>
                                          <tr style="display: none">
                                              <td><asp:Label ID="StatoLabel" runat="server" CssClass="Etichetta" Text="Stato" Width="70"/></td>
                                              <td>
                                                  <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 40px">
                                                                    <telerik:RadComboBox ToolTip="Stato dell'iter" ID="StatoComboBox" runat="server"
                                                                        Skin="Office2007" Width="230px" EmptyMessage="- Selezionare -" ItemsPerRequest="10"
                                                                        Filter="StartsWith" MaxHeight="400px" NoWrap="True" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="RiferimentoDocumentoLabel" runat="server" CssClass="Etichetta"
                                                                        Text="Riferimento" />
                                                                    <telerik:RadTextBox ID="RiferimentoDocumentoTextBox" runat="server" Skin="Office2007"
                                                                        Width="220px" />
                                                                    <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta"
                                                                        Text="Tipologia" />
                                                                    <telerik:RadComboBox ToolTip="Tipologia di documento" ID="TipologiaDocumentoComboBox"
                                                                        runat="server" Skin="Office2007" Width="220px" EmptyMessage="- Selezionare -"
                                                                        ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" NoWrap="True" />
                                                                </td>
                                                            </tr>
                                                        </table>
                               </td></tr></table></td></tr>
                               <tr style="height: 20px">
                                   <td valign="top">
                                       <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                         <tr>
                                             <td>
                                                 <asp:Label ID="ElencoTaskLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                     color: #00156E; background-color: #BFDBFF" Text="Elenco attività per moduli" />
                                             </td>
                                             <td align="center" style="width: 30;border-left:1 solid #5D8CC9; display:none" >
                                                 <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                     ToolTip="Effettua la ricerca con i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" TabIndex="3"/>
                                             </td>
                                             <td align="center" style="width: 30;border-left:1 solid #5D8CC9;display:none">
                                                 <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                     ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" TabIndex="4"/>
                                             </td>
                                             <td align="center" style="width: 30;border-left:1 solid #5D8CC9;">
                                                 <asp:ImageButton ID="EsportaXlsBtn" runat="server" ImageUrl="~/images/excel32.png"
                                                     TabIndex="5" ToolTip="Esporta in Excel i dati visualizzati" Width="20" Height="20"
                                                     ImageAlign="AbsMiddle" /> 
                                             </td>
                               </tr></table></td></tr>
                               <tr>
                                   <td valign="top" class="ContainerMargin">
                                       <telerik:RadTabStrip runat="server" ID="ScrivaniaTabStrip" MultiPageID="ScrivaniaMultiPage" Width="100%" SelectedIndex="0" Skin="Office2007" >
                                           <Tabs>
                                                 <telerik:RadTab Text="Atti decisionali" Selected="True" ToolTip="Elenco attività Modulo Atti Decisionali" Width="400px"/>
                                                 <telerik:RadTab Text="Protocolli" ToolTip="Elenco attività Modulo Protocollo" Width="400px"/>                                                 
                                           </Tabs>
                                       </telerik:RadTabStrip>
                                       <telerik:RadMultiPage runat="server" ID="ScrivaniaMultiPage" Height="600" Width="100%" CssClass="multiPage" BorderColor="#3399FF" SelectedIndex="0">

                                           <telerik:RadPageView runat="server" ID="AttiPageView" CssClass="corporatePageView" Height="600px">
                                              <table class="Container" cellpadding="0" cellspacing="4" style="width: 100%; height: 100%" border="0">
                                                 <tr style="background-color: #DFE8F6">
                                                     <td valign="top">
                                                         <div id="scrollPanelAtti" runat="server" style="overflow: auto; height:590px; border: 1px solid #5D8CC9">
                                                              <telerik:RadGrid ID="TaskAttiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                  GridLines="None" Skin="Office2007" Width="950px" AllowFilteringByColumn="true" TabIndex="2"
                                                                  AllowSorting="True" Culture="it-IT">
                                                                  <GroupingSettings CaseSensitive="false" /> 
                                                                  <MasterTableView DataKeyNames="Id,NomeFileIter,IdDocumento,IdModulo, IdIstanza">
                                                                     <NestedViewTemplate>

                                                                        <telerik:RadGrid ID="IterAttiGridView" runat="server" AutoGenerateColumns="False" ToolTip="Lista dell'attività collegate all'istanza dell'iter"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                            AllowPaging="True">

                                                                            <MasterTableView Width="100%" TableLayout="Fixed">
                                                                            
                                                                                <Columns>
                                                                                    <telerik:GridBoundColumn DataField="ID" Visible="False"/>
                                                                                    <telerik:GridTemplateColumn SortExpression="AttoreMittente" UniqueName="AttoreMittente" HeaderText="Utente"
                                                                                        DataField="AttoreMittente" HeaderStyle-Width="190px" ItemStyle-Width="190px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("AttoreMittente")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 190px; border: 0px solid red">
                                                                                                <%# Eval("AttoreMittente")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>                                                                                    
                                                                                    <telerik:GridTemplateColumn SortExpression="DataEsecuzione" UniqueName="DataEsecuzione"
                                                                                        HeaderText="Data Esecuzione" DataField="DataEsecuzione" HeaderStyle-Width="115px" ItemStyle-Width="115px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DataEsecuzione","{0:dd/MM/yyyy HH.mm}")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 115px; border: 0px solid red">
                                                                                                <%# Eval("DataEsecuzione", "{0:dd/MM/yyyy HH.mm}")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn SortExpression="Operazione" UniqueName="Operazione" HeaderText="Operazione"
                                                                                        DataField="Operazione" HeaderStyle-Width="260px" ItemStyle-Width="260px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Operazione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 260px; border: 0px solid red">
                                                                                                <%# Eval("Operazione")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" HeaderText="Note"
                                                                                        DataField="Note" HeaderStyle-Width="290px" ItemStyle-Width="290px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Replace(Eval("Note"), "'", "&#039;")%>'  style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 290px; border: 0px solid red">
                                                                                                <%# Eval("Note")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>                                                                                   
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
                                                                     <NoRecordsTemplate><div>Nessun Atto Presente</div></NoRecordsTemplate>
                                                                     <Columns>
                                                                        <telerik:GridBoundColumn DataField="Id" UniqueName="Id" Visible="False" />                                                                        
                                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneDocumento" UniqueName="DescrizioneDocumento"
                                                                        HeaderText="Atto" DataField="DescrizioneDocumento" HeaderStyle-Width="600px"
                                                                        ItemStyle-Width="600px" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                        FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="NoteImage" Style="text-align: left" ImageAlign="Left" Width="15px"
                                                                                runat="Server" ImageUrl="~\images\Note.png" ToolTip='<%# Eval("Note")%>' Visible='<%# CBool(Eval("Note") <> "").ToString %>' />
                                                                            <div title='<%# Replace(Eval("DescrizioneDocumento"), "'", "&#039;")%>' style="width: 600px;
                                                                                border: 0px solid red; margin-left: 30px">
                                                                                <%# Eval("DescrizioneDocumento")%>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn Text="Esegui operazione corrente" ButtonType="ImageButton"
                                                                        ItemStyle-HorizontalAlign="Center" ImageUrl="~/images/new/attivita.gif" CommandName="Execute"
                                                                        UniqueName="Execute" ItemStyle-Width="45px" HeaderStyle-Width="45px">
                                                                    </telerik:GridButtonColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Proponente" UniqueName="Proponente"
                                                                        HeaderText="Settore Proponente" DataField="Proponente" HeaderStyle-Width="150px"
                                                                        ItemStyle-Width="150px" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                        FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Interlocutore"), "'", "&#039;")%>' style="width: 150px;
                                                                                border: 0px solid red">
                                                                                <%# Eval("Proponente")%>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        AllowFiltering="false">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="DirezioneImage" Style="display: inline-block" Width="15px" runat="Server"
                                                                                ImageUrl='<%# Eval("PathDirezione") %>' ToolTip='<%# Eval("TooltipDirezione")%>' />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                        Text="Visualizza documento" ImageUrl="~\images\knob-search16.png" UniqueName="Preview"
                                                                        ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                    </telerik:GridButtonColumn>
                                                                        <telerik:GridBoundColumn DataField="DataInizio" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                                                        <telerik:GridBoundColumn DataField="TaskSuccessivo" HeaderText="Successivo" Visible="false" />
                                                                        <telerik:GridBoundColumn DataField="TaskPrecedente" Visible="False" />
                                                                        <telerik:GridBoundColumn DataField="IdModello" Visible="false" />
                                                                        <telerik:GridBoundColumn DataField="NomeFileIter" Visible="false" UniqueName="NomeFileIter" />
                                                                        <telerik:GridBoundColumn DataField="IdDocumento" Visible="false" UniqueName="IdDocumento" />
                                                                        <telerik:GridBoundColumn DataField="IdModulo" Visible="false" UniqueName="IdModulo" />
                                                                     </Columns>
                                                                  </MasterTableView>
                                                              </telerik:RadGrid>
                                              </div></td></tr></table>
                                           </telerik:RadPageView>

                                           <telerik:RadPageView runat="server" ID="ProtocolliPageView" CssClass="corporatePageView" Height="600px">
                                              <table class="Container" cellpadding="0" cellspacing="4" style="width: 100%; height: 100%" border="0">
                                                 <tr style="background-color: #DFE8F6">
                                                     <td valign="top">
                                                         <div id="scrollPanelProtocolli" runat="server" style="overflow: auto; height: 590px; border: 1px solid #5D8CC9">           
                                                              <telerik:RadGrid ID="TaskProcolliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                  GridLines="None" Skin="Office2007" Width="950px" AllowFilteringByColumn="true" TabIndex="2"
                                                                  AllowSorting="True" Culture="it-IT">
                                                                  <GroupingSettings CaseSensitive="false" /> 
                                                                  <MasterTableView DataKeyNames="Id,NomeFileIter,IdDocumento,IdModulo, IdIstanza">
                                                                      <NestedViewTemplate>
                                                                        <telerik:RadGrid ID="IterProtocolliGridView" runat="server" AutoGenerateColumns="False" ToolTip="Lista dell'attività collegate all'istanza dell'iter"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                            AllowPaging="True">

                                                                            <MasterTableView Width="100%" TableLayout="Fixed">
                                                                            
                                                                                <Columns>
                                                                                    <telerik:GridBoundColumn DataField="ID" Visible="False"/>
                                                                                    <telerik:GridTemplateColumn SortExpression="AttoreMittente" UniqueName="AttoreMittente" HeaderText="Utente"
                                                                                        DataField="AttoreMittente" HeaderStyle-Width="190px" ItemStyle-Width="190px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("AttoreMittente")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 190px; border: 0px solid red">
                                                                                                <%# Eval("AttoreMittente")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>                                                                                    
                                                                                    <telerik:GridTemplateColumn SortExpression="DataEsecuzione" UniqueName="DataEsecuzione"
                                                                                        HeaderText="Data Esecuzione" DataField="DataEsecuzione" HeaderStyle-Width="115px" ItemStyle-Width="115px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DataEsecuzione","{0:dd/MM/yyyy HH.mm}")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 115px; border: 0px solid red">
                                                                                                <%# Eval("DataEsecuzione", "{0:dd/MM/yyyy HH.mm}")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn SortExpression="Operazione" UniqueName="Operazione" HeaderText="Operazione"
                                                                                        DataField="Operazione" HeaderStyle-Width="260px" ItemStyle-Width="260px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Operazione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 260px; border: 0px solid red">
                                                                                                <%# Eval("Operazione")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" HeaderText="Note"
                                                                                        DataField="Note" HeaderStyle-Width="290px" ItemStyle-Width="290px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Replace(Eval("Note"), "'", "&#039;")%>'  style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 290px; border: 0px solid red">
                                                                                                <%# Eval("Note")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>                                                                                   
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
                                                                      <NoRecordsTemplate><div>Nessun Protocollo Presente</div></NoRecordsTemplate>
                                                                      <Columns>
                                                                          <telerik:GridBoundColumn DataField="Id" UniqueName="Id" Visible="False" />                                                                   
                                                                          <telerik:GridTemplateColumn SortExpression="DescrizioneDocumento" UniqueName="DescrizioneDocumento"
                                                                            HeaderText="Protocollo" DataField="DescrizioneDocumento" HeaderStyle-Width="600px"
                                                                            ItemStyle-Width="600px" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                            FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="NoteImage" Style="text-align: left" ImageAlign="Left" Width="15px"
                                                                                    runat="Server" ImageUrl="~\images\Note.png" ToolTip='<%# Eval("Note")%>' Visible='<%# CBool(Eval("Note") <> "").ToString %>' />
                                                                                <div title='<%# Replace(Eval("DescrizioneDocumento"), "'", "&#039;")%>' style="width: 600px;
                                                                                    border: 0px solid red; margin-left: 30px">
                                                                                    <%# Eval("DescrizioneDocumento")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                          <telerik:GridButtonColumn Text="Esegui operazione corrente" ButtonType="ImageButton"
                                                                            ItemStyle-HorizontalAlign="Center" ImageUrl="~/images/new/attivita.gif" CommandName="Execute"
                                                                            UniqueName="Execute" ItemStyle-Width="45px" HeaderStyle-Width="45px">
                                                                          </telerik:GridButtonColumn>
                                                                          <telerik:GridTemplateColumn SortExpression="Proponente" UniqueName="Proponente"
                                                                            HeaderText="Destinatario" DataField="Proponente" HeaderStyle-Width="150px"
                                                                            ItemStyle-Width="150px" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                            FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Replace(Eval("Interlocutore"), "'", "&#039;")%>' style="width: 150px;
                                                                                    border: 0px solid red">
                                                                                    <%# Eval("Proponente")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                          <telerik:GridTemplateColumn HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                                AllowFiltering="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="DirezioneImage" Style="display: inline-block" Width="15px" runat="Server"
                                                                                        ImageUrl='<%# Eval("PathDirezione") %>' ToolTip='<%# Eval("TooltipDirezione")%>' />
                                                                                </ItemTemplate>
                                                                          </telerik:GridTemplateColumn>
                                                                          <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                            Text="Visualizza documento" ImageUrl="~\images\knob-search16.png" UniqueName="Preview"
                                                                            ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                          </telerik:GridButtonColumn>
                                                                          <telerik:GridBoundColumn DataField="DataInizio" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                                                          <telerik:GridBoundColumn DataField="TaskSuccessivo" HeaderText="Successivo" Visible="false" />
                                                                          <telerik:GridBoundColumn DataField="TaskPrecedente" Visible="False" />
                                                                          <telerik:GridBoundColumn DataField="IdModello" Visible="false" />
                                                                          <telerik:GridBoundColumn DataField="NomeFileIter" Visible="false" UniqueName="NomeFileIter" />
                                                                          <telerik:GridBoundColumn DataField="IdDocumento" Visible="false" UniqueName="IdDocumento" />
                                                                          <telerik:GridBoundColumn DataField="IdModulo" Visible="false" UniqueName="IdModulo" />
                                                                     </Columns>
                                                                </MasterTableView>
                                                              </telerik:RadGrid>  
                                              </div></td></tr></table>          
                                           </telerik:RadPageView>

                                       </telerik:RadMultiPage> 
                    </td></tr></table></td></tr></table>          
                    <asp:ImageButton ID="AggiornaTaskButton" runat="server" Style="display: none" />
                    <asp:ImageButton ID="SbloccaTaskButton" runat="server" Style="display: none" />
                    <asp:HiddenField ID="scrollPosHiddenAtti" runat="server" Value="0" />
                    <asp:HiddenField ID="scrollPosHiddenProtocolli" runat="server" Value="0" />
                </center>
                <div id="taskPanel" style="position: absolute; width: 100%; text-align: center; z-index: 2000000; display: none; top:0px">
                    <div id="containerPanel" style="width: 650px; text-align: center; background-color: #BFDBFF;   margin: 0 auto">
                         <parsec:OperazioneControl runat="server" ID="OperazioneControl" />                                     
                </div></div>               
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>