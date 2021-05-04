<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="AttivaRagionierePage.aspx.vb" Inherits="AttivaRagionierePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:content id="Content1" contentplaceholderid="MainContent" runat="Server">
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
            //Gestione messaggio 
            //            var dataItems = args.get_dataItems();
            //            if (dataItems['MainContent_messaggio'] != null) {
            //               alert(dataItems['MainContent_messaggio']);
            //               dataItems['MainContent_messaggio'] = null;
            //            } 
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
                                                     
          <table style="width: 950px; height:600px" cellpadding="5" cellspacing="5" border="0">
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
                                     <td style="width: 90px">
                                         <asp:Label ID="AttoriScrivaniaLabel" runat="server" CssClass="Etichetta" Text="Scrivania di" />
                                     </td>
                                     <td>
                                         <telerik:RadComboBox AutoPostBack="True" ToolTip="Titolare della scrivania" ID="DelegheScrivaniaComboBox"
                                             runat="server" Skin="Office2007" Width="350px" EmptyMessage="- Selezionare -"
                                             ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" NoWrap="True" TabIndex="2" />
                                     </td>
                                     <td style="width: 90px">
                                         
                                          <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipologia" />
                                     </td>
                                     <td>
                                       <telerik:RadComboBox ToolTip="Tipologia modello del documento" ID="TipologiaDocumentoComboBox"
                                             runat="server" Skin="Office2007" Width="370px" EmptyMessage="- Selezionare -"
                                             ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" NoWrap="True" TabIndex="5" />
                                        
                                     </td>
                                 </tr>
                                 <tr style="  display:none">
                                     <td style="width: 90px">
                                         <asp:Label ID="StatoLabel" runat="server" CssClass="Etichetta" Text="Stato" />
                                     </td>
                                     <td>
                                         <telerik:RadComboBox ToolTip="Stato dell'iter" ID="StatoComboBox" runat="server"
                                             Skin="Office2007" Width="350px" EmptyMessage="- Selezionare -" ItemsPerRequest="10"
                                             Filter="StartsWith" MaxHeight="400px" NoWrap="True" TabIndex="4" />
                                     </td>
                                     <td style="width: 90px">
                                        <asp:Label ID="RiferimentoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Riferimento" />
                                     </td>
                                     <td>
                                        <telerik:RadTextBox ID="RiferimentoDocumentoTextBox" runat="server" Skin="Office2007"
                                             Width="370px" TabIndex="3" />
                                     </td>
                                 </tr>
                           </table>


                        <%-- FINE FILTRO--%>                        
                       </td></tr>
                       <tr style="height:20px">
                           <td valign="top" style=" border-bottom:1px solid #9ABBE8">
                               <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;" >
                                 <tr>
                                     <td>
                                         <asp:Label ID="ElTask" runat="server" Font-Bold="True" Style="width: 120px;color: #00156E; background-color: #BFDBFF" Text="Attività"/>                                                                                                                 
                                     </td>  
                                     <td style="width: 200px; text-align: center; background-color: #BFDBFF;">
                                      <telerik:RadButton id="Btn_Invia" tabIndex="6" onclick="Btn_Invia_Click" runat="server" 
                                                          ToolTip="Inoltra l'atto all'attore successivo dell'iter" Width="150px" 
                                                          ForeColor="Green" UseSubmitBehavior="False" onClientClick="this.disabled=true;" 
                                                          Font-Size="X-Small" Font-Names="Verdana" 
                                             Text="Inoltra Avanti"/>
                                     </td>
                                      <td style="width: 200px; text-align: center; background-color: #BFDBFF;">
                                     <telerik:RadButton id="Btn_Indietro" tabIndex="7" onclick="Btn_Indietro_Click" 
                                                          runat="server" ToolTip="Invio indietro all'attore precedente dell'iter" 
                                                          Width="150px" UseSubmitBehavior="False" 
                                                          onClientClick="this.disabled=true;" Font-Size="X-Small" 
                                                          Font-Names="Verdana" Text="Ritorna Indietro"/> 
                                     </td>
                                     <td style="width: 200px; text-align: center; background-color: #BFDBFF;">
                                     <telerik:RadButton Width="150px" ID="ExecuteTaskButton" 
                                                          onClientClick="this.disabled=true;"  OnClientClicked="OnClientClicked" 
                                                          runat="server" Text="Smista Addetto" EnableSplitButton="true"  
                                                           ToolTip="Selezione dell'addetto a cui smistare l'attività" 
                                                          Skin="Office2007" Autopostback="False" Forecolor="Green" Font-Size="X-Small" 
                                                          Font-Names="Verdana" TabIndex="8"/>
                                                      <telerik:RadContextMenu ID="ExecuteContextMenu" runat="server" OnItemClick="OnCtx_ItemClick">
                                                            <Items/> 
                                                      </telerik:RadContextMenu> 
                                     </td>
                                     <td>
                                     <telerik:RadButton id="Btn_Firma" tabIndex="9" onclick="Btn_Firma_Click" 
                                                          runat="server" ToolTip="Firma del documento selezionato - FUNZIONALITA' DA IMPLEMENTARE" 
                                                          Width="150px" 
                                                          ForeColor="Blue" UseSubmitBehavior="False" onClientClick="this.disabled=true;" 
                                                          Font-Size="X-Small" Font-Names="Verdana" Text="Firma" 
                                             Enabled="false"/>
                                     </td>    
                                     <td>
                                      <asp:ImageButton ID="EsportaXlsBtn" runat="server" ImageUrl="~/images/excel32.png"
                                                          TabIndex="10"  ToolTip="Esporta in Excel i risultati visualizzati"
                                                          ImageAlign="AbsMiddle" />    
                                     </td>                                                                   
                       </tr></table></td></tr>                    
                        <tr>
                            <td valign="top" class="ContainerMargin">                                  
                          <div id="scrollPanel" runat="server" style="overflow: auto; width: 100%; height:490px; background-color: #FFFFFF;  border-right: 1px solid #BFDBFF; border-left:1px solid #BFDBFF; border-bottom:1px solid #BFDBFF" >                                                                                                                                            
                              <telerik:RadGrid ID="TaskGridView" runat="server" 
                                                   AutoGenerateColumns="False"  GridLines="None" Skin="Office2007" Width="99.8%" 
                                                          AllowSorting="True" Culture="it-IT" AllowMultiRowSelection="True" AllowFilteringByColumn="true" 
                                                   TabIndex="1"> 
                                                   <GroupingSettings CaseSensitive="false" />                            
                                                <MasterTableView DataKeyNames="Id,TaskCorrente,NomeFileIter,IdDocumento,IdIstanza" >  
                                                       <NestedViewTemplate>

                                                             <table runat="server" id="ImpTable" style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                <tr><td><asp:Label ID="TitImpLbl" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 150px; color: #00156E; background-color: #BFDBFF" Text="Impegni di Spesa" /></td></tr>
                                                             <tr style="background-color: #FFFFFF">
                                                                 <td>
                                                                    <div class="CustomFooter" style="overflow: auto; height: 100%; border: 1px solid #5D8CC9">
                                                                            <telerik:RadGrid ID="ImpGridView" runat="server" ToolTip="Elenco impegni di spesa associati al documento"
                                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                            Width="99.8%" Culture="it-IT" ShowFooter="true">
                                                                            <MasterTableView DataKeyNames="Id">
                                                                                <Columns>
                                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" UniqueName="Id" Visible="False" />
                                                                                    <telerik:GridTemplateColumn SortExpression="AnnoEsercizio" UniqueName="AnnoEsercizio"
                                                                                        HeaderText="Anno" DataField="AnnoEsercizio" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("AnnoEsercizio")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis;border: 0px solid red">
                                                                                            <%# Eval("AnnoEsercizio")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="Capitolo" UniqueName="Capitolo" ItemStyle-HorizontalAlign="Right"
                                                                                        HeaderText="Capitolo" DataField="Capitolo" ItemStyle-Width="40px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Capitolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; border: 0px solid red">
                                                                                            <%# Eval("Capitolo")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="Articolo" UniqueName="Articolo" ItemStyle-HorizontalAlign="Right"
                                                                                        HeaderText="Articolo" DataField="Articolo" ItemStyle-Width="40px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Articolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                                            <%# Eval("Articolo")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="NumeroImpegno" UniqueName="NumeroImpegno"
                                                                                        HeaderText="Impegno" DataField="NumeroImpegno"
                                                                                        ItemStyle-HorizontalAlign="Right">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("NumeroImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width:50px; border: 0px solid red">
                                                                                            <%# Eval("NumeroImpegno")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="NumeroSubImpegno" UniqueName="NumeroSubImpegno"
                                                                                        ItemStyle-HorizontalAlign="Left" HeaderText="Sub Impegno" DataField="NumeroSubImpegno">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("NumeroSubImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width:80px; border: 0px solid red">
                                                                                            <%# Eval("NumeroSubImpegno")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="Importo" UniqueName="Importo" HeaderText="Importo" DataField="Importo"
                                                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" 
                                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Importo","{0:N2}")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; border: 0px solid red">
                                                                                             <%# Eval("Importo", "{0:N2}")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn> 
                                                                                    <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" ItemStyle-HorizontalAlign="Left"
                                                                                        HeaderText="Descrizione" DataField="Note" HeaderStyle-Width="320px" ItemStyle-Width="320px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Note")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 320px; border: 0px solid red">
                                                                                            <%# Eval("Note")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>                                                                                                                                                                                                                                           
                                                                                </Columns>
                                                             </MasterTableView> </telerik:RadGrid></div></td></tr></table>
                                                             <table runat="server" id="LiqTable" style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                <tr><td><asp:Label ID="TitLiqLbl" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 150px; color: #00156E; background-color: #BFDBFF" Text="Liquidazioni" /></td></tr>
                                                             <tr style="background-color: #FFFFFF">
                                                                 <td>
                                                                    <div class="CustomFooter" style="overflow: auto; height: 100%; border: 1px solid #5D8CC9">
                                                            <telerik:RadGrid ID="LiqGridView" runat="server" ToolTip="Elenco liquidazioni associate al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            Width="99.8%" Culture="it-IT" ShowFooter="True">
                                                            <MasterTableView DataKeyNames="Id, Guid">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                    <telerik:GridTemplateColumn SortExpression="AnnoImpegno" UniqueName="AnnoImpegno"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Anno I." DataField="AnnoImpegno"
                                                                        HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("AnnoImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                             <%# Eval("AnnoImpegno")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Capitolo" UniqueName="Capitolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Cap. I." DataField="Capitolo" ItemStyle-HorizontalAlign="Right" >
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Capitolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 50px; border: 0px solid red">
                                                                           <%# Eval("Capitolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Articolo" UniqueName="Articolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Art. I." DataField="Articolo" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Articolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                            <%# Eval("Articolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>                                                                    
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroImpegno" UniqueName="NumeroImpegno"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Num.Imp." DataField="NumeroImpegno" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                              <%# Eval("NumeroImpegno")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroSubImpegno" UniqueName="NumeroSubImpegno"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Sub Imp." DataField="NumeroSubImpegno" ItemStyle-HorizontalAlign="left">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroSubImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                              <%# Eval("NumeroImpegno")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="AnnoEsercizio" UniqueName="AnnoEsercizio"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Anno L." DataField="AnnoEsercizio" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("AnnoEsercizio")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: AnnoEsercizio; width: 50px; border: 0px solid red">
                                                                             <%# Eval("AnnoEsercizio")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Numero" UniqueName="Numero" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Num. Liq." DataField="Numero" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Numero")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                width: 60px; border: 0x solid red">
                                                                           <%# Eval("Numero")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Mandato" UniqueName="Mandato" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Mandato" DataField="Mandato" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Mandato")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                           <%# Eval("Mandato")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="ImportoLiquidato" UniqueName="ImportoLiquidato"
                                                                        FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderText="Importo"
                                                                        DataField="ImportoLiquidato" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("ImportoLiquidato","{0:N2}")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 90px; border: 0px solid red">
                                                                             <%# Eval("ImportoLiquidato", "{0:N2}")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Descrizione" DataField="Note" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Note")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 150px; border: 0px solid red">
                                                                           <%# Eval("Nominativo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Nominativo" UniqueName="Nominativo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Beneficiario" DataField="Nominativo" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 150px; border: 0px solid red">
                                                                           <%# Eval("Nominativo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>                                                                    
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div></td></tr></table>
                                                             <table runat="server" id="AccTable" style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                <tr><td><asp:Label ID="TitAccLbl" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 150px; color: #00156E; background-color: #BFDBFF" Text="Accertamenti" /></td></tr>
                                                             <tr style="background-color: #FFFFFF">
                                                                 <td>
                                                                    <div class="CustomFooter" style="overflow: auto; height: 100%; border: 1px solid #5D8CC9">
                                                             <telerik:RadGrid ID="AccGridView" runat="server" ToolTip="Elenco accertamenti associati al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007" 
                                                           Width="99.8%" Culture="it-IT" ShowFooter="True">                                                            
                                                            <MasterTableView DataKeyNames="Id">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                    <telerik:GridTemplateColumn SortExpression="AnnoEsercizio" UniqueName="AnnoEsercizio"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Anno" DataField="AnnoEsercizio"
                                                                        HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("AnnoEsercizio")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                              <%# Eval("AnnoEsercizio")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Capitolo" UniqueName="Capitolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Capitolo" DataField="Capitolo" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Capitolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                            <%# Eval("Capitolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Articolo" UniqueName="Articolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Articolo" DataField="Articolo" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Articolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                             <%# Eval("Articolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroAccertamento" UniqueName="NumeroAccertamento"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Numero" DataField="NumeroAccertamento"
                                                                        ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroAccertamento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                              <%# Eval("NumeroAccertamento")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroSubAccertamento" UniqueName="NumeroSubAccertamento"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Sub. Accertamento" DataField="NumeroSubAccertamento" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroSubAccertamento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                            <%# Eval("NumeroSubAccertamento")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn> 
                                                                     <telerik:GridTemplateColumn SortExpression="Importo" UniqueName="Importo" FooterStyle-HorizontalAlign="Right"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Importo" DataField="Importo"
                                                                        ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Importo","{0:N2}")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 110px; border: 0px solid red">
                                                                              <%# Eval("Importo", "{0:N2}")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>  
                                                                    <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Descrizione" DataField="Note" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Note")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                width: 320px; border: 0px solid red">
                                                                           <%# Eval("Note")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>                                                                                                                                                                                                        
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div></td></tr></table>

                                                        </NestedViewTemplate> 
                                                                                 
                                                    <NoRecordsTemplate><div>Nessun Atto Contabile Presente</div></NoRecordsTemplate>                                                                                          
                                                  
                                                    <Columns>
                                                       
                                                        <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto" AllowFiltering="False" ItemStyle-Width="25px" HeaderStyle-Width="25px">
                                                            <HeaderTemplate>
                                                                 <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True" runat="server"/>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                 <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True" runat="server" Tooltip="Seleziona"/>
                                                            </ItemTemplate>                                                           
                                                         
                                                        </telerik:GridTemplateColumn> 
                                                                                                                                                                                                                                                                                                                                          
                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" UniqueName="Id" Visible="False"/>  


                                                        <telerik:GridButtonColumn ItemStyle-Width="20px" HeaderStyle-Width="20px" ButtonType="ImageButton" CommandName="Preview" Text="Visualizza documento" ImageUrl="~\images\knob-search16.png" UniqueName="Preview"  />                              
                                                       
                                                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" Headertext="Tipo" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px" HeaderStyle-Width="70px" AllowFiltering="false">
					                                            <ItemTemplate>
						                                             <asp:Label ID="LblTipo" runat="server" Width="50px"/>
					                                            </ItemTemplate>
				                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn  SortExpression="Documento" UniqueName="Documento" HeaderText="Riferimento Documento" DataField="Documento" HeaderStyle-Width="350px"
                                                                        ItemStyle-Width="350px" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                        FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true" >    
                                                                   <ItemTemplate >   
                                                                        <div title="ISTRUITO DA -<%# Replace(Eval("Proponente"), "'", "&#039;")%> - SCADE IL -<%# Format(Eval("DataScadenza"),"dd/MM/yyyy") %>-" style="width:350px; border: 0px solid red" ><%# Eval("Documento")%></div>
                                                                     </ItemTemplate>    
                                                        </telerik:GridTemplateColumn> 

                                                        <telerik:GridButtonColumn ItemStyle-Width="45px" HeaderStyle-Width="45px" Text="Esegui operazione corrente" ButtonType="ImageButton" ItemStyle-HorizontalAlign="Center" ImageUrl="~/images/new/attivita.gif" CommandName="Execute" UniqueName="Execute"  />
                                                      
                                                      
                                                        <telerik:GridTemplateColumn SortExpression="TaskCorrente" UniqueName="TaskCorrente" HeaderText="Stato" DataField="TaskCorrente" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="110px" HeaderStyle-Width="110px"
                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                        FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true">    
                                                             <ItemTemplate>   
                                                                 <div title="PRECEDENTE <%# Eval("TaskPrecedente")%>  <-  -> SUCCESSIVO <%# Eval("TaskSuccessivo")%>" style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:110px; border: 0 solid red" ><%# Eval("TaskCorrente")%></div>
                                                             </ItemTemplate>    
                                                        </telerik:GridTemplateColumn>   
                                                                                                             
                                                        <telerik:GridTemplateColumn ItemStyle-Width="20px" HeaderStyle-Width="20px" AllowFiltering="false">
					                                            <ItemTemplate>
						                                             <asp:Image ID="Col_Imp" runat="server"/>
					                                            </ItemTemplate>
				                                        </telerik:GridTemplateColumn>   

                                                        <telerik:GridTemplateColumn SortExpression="TotaleImporto" UniqueName="TotaleImporto" HeaderText="Importo" DataField="TotaleImporto" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataType="System.Double" ItemStyle-Width="100px" HeaderStyle-Width="100px" AllowFiltering="false">    
                                                             <ItemTemplate>   
                                                                 <div title="Totale Importo -> ( <%# Eval("TotaleImportoTooltip") %> ) <-  Accertati/Impegnati/Liquidati  -> ( <%# Eval("TotaleImportoI") %> )" style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:100px; border: 0px solid red" ><%# Eval("TotaleImporto", "{0:N2}")%></div>
                                                             </ItemTemplate>    
                                                        </telerik:GridTemplateColumn>   
                                                                                                             
                                                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" HeaderStyle-Width="20px" AllowFiltering="false">    
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
     </telerik:RadGrid>
   </div>
     
     </td></tr></table></td></tr></table>

          
     <asp:ImageButton ID="AggiornaTaskButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
</center></div></contenttemplate>
    </asp:updatepanel>
</asp:content>