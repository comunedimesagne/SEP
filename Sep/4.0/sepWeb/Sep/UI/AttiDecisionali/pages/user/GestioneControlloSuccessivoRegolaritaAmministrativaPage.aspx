<%@ Page Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneControlloSuccessivoRegolaritaAmministrativaPage.aspx.vb" Inherits="GestioneControlloSuccessivoRegolaritaAmministrativaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">


    <script type="text/javascript">



        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");

        var hide = true;

       
        Sys.Application.add_init(function () {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        });


        function pageLoad() {
             $get("pageContent").appendChild(_backgroundElement);
             $get("pageContent").appendChild(overlay);


             var message = $get('<%= HideWindow.ClientId %>').value;

             if (message == 'SI') {
                 hide = true;
             }
             if (message == 'NO') {
                 hide = false;
             }

             $get('<%= HideWindow.ClientId %>').value = '';

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


        function SelectTabByIndex(index) {
            var tabStrip = $find("<%= ControlloSuccessivoTabStrip.ClientID %>");
            var tab = tabStrip.get_tabs().getTab(index);
            if (tab) {
                tab.select();
            }
        }

      

    </script>


    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 300px; z-index: 2000000">
                <table cellpadding="4" style="background-color: #4892FF; margin: 0 auto">
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

            
               
                <center>

                      <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td style=" width:300px">
                                                                    &nbsp;<asp:Label ID="TitoloElencoLabel" runat="server" Font-Bold="True" Style="width: 290px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Elenco Atti Soggetti a Controllo" />
                                                                </td>


                                                                 <td>
                                                                    <asp:Label ID="PeriodoRiferimentoLabel" runat="server" Font-Bold="True" Style=" width:100%;
                                                                        color: #00156E; background-color: #BFDBFF" Text="" />
                                                                </td>
                                                               
                                                                 <td style="width: 80px; text-align: center">
                                                               
                                                                     <asp:Label ID="TipologiaLabel" runat="server" Font-Bold="True" Style="width: 80px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Tipologia" />
                                                                </td>
                                                                <td style="width: 230px">
                                                                    <telerik:RadComboBox ID="TipologieDocumentoComboBox" runat="server" EmptyMessage="- Seleziona Tipologia -"
                                                                        Filter="StartsWith" ItemsPerRequest="10" MaxHeight="200px" Skin="Office2007"
                                                                        Width="170px" />
                                                                </td>

                                                                <td align="center" style="width: 40px">
                                                                    <asp:ImageButton ID="ModificaParametriControlloSuccessivo" Style="border: 0" runat="server"
                                                                        ImageUrl="~/images//Edit32.png" ToolTip="Modifica parametri"
                                                                        ImageAlign="AbsMiddle" />
                                                                </td>


                                                                <td align="center" style="width: 40px">
                                                                    <asp:ImageButton ID="EseguiCampionamentoImageButton" Style="border: 0" runat="server"
                                                                        ImageUrl="~/images//preferenze32.png" ToolTip="Esegui campionamento delle determine/ordinanze da sottoporre a controllo"
                                                                        ImageAlign="AbsMiddle" />
                                                                </td>

                                                                <td align="center" style="width: 40px">
                                                                    <asp:ImageButton ID="SalvaImageButton" Style="border: 0" runat="server"
                                                                        ImageUrl="~/images//Save.png" ToolTip="Avvia attività di controllo"
                                                                        ImageAlign="AbsMiddle" />
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="ZoneID2" style="overflow: auto; height: 600px; width: 100%; background-color: #FFFFFF;
                                                            border: 1px solid #5D8CC9;">
                                                          
                                                            <telerik:RadGrid ID="DocumentiGridView" runat="server" AllowPaging="True"
                                                                AutoGenerateColumns="False" AllowFilteringByColumn="True" CellSpacing="0" GridLines="None"
                                                                Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT" PageSize="20"
                                                                AllowMultiRowSelection="True">

                                                                <MasterTableView  DataKeyNames="IdAtto" TableLayout="Fixed">
                                                                
                                                                   
                                                                    <NoRecordsTemplate>
                                                                        <div>
                                                                            Nessuna Determina Trovata</div>
                                                                    </NoRecordsTemplate>
                                                                    <Columns>

                                                                 
                                                                  
                                                                   
                                                                        <telerik:GridBoundColumn DataField="IdAtto" DataType="System.Int32" FilterControlAltText="Filter IdAtto column"
                                                                            HeaderText="IdAtto" ReadOnly="True" SortExpression="IdAtto" UniqueName="IdAtto" Visible="False" />
                                                                      
                                                                  

                                                                       
                                                                        <telerik:GridBoundColumn  AutoPostBackOnFilter="True" FilterControlWidth="100%" DataField="ContatoreGenerale" FilterControlAltText="Filter ContatoreGenerale column"
                                                                            HeaderText="N." SortExpression="ContatoreGenerale" UniqueName="ContatoreGenerale"
                                                                            ShowFilterIcon="False"  ItemStyle-Width="60px" HeaderStyle-Width="60px" AndCurrentFilterFunction="EqualTo"
                                                                            HeaderTooltip="Numero Contatore Generale">
                                                                        </telerik:GridBoundColumn>


                                                                     

                                                                         <telerik:GridBoundColumn AllowFiltering="False" DataField="DataDocumento"
                                                                            DataType="System.DateTime" FilterControlAltText="Filter DataDocumento column"
                                                                            HeaderText="Data" ShowFilterIcon="False" SortExpression="DataDocumento"
                                                                            UniqueName="DataDocumento" DataFormatString="{0:dd/MM/yyyy}"
                                                                            HeaderTooltip="Data della Determina/Ordinanza" ItemStyle-Width="85px" HeaderStyle-Width="85px">
                                                                        </telerik:GridBoundColumn>

                                                                         

                                                                         <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains"  ItemStyle-Width="85px"
                                                                            HeaderStyle-Width="85px"
                                                                            AutoPostBackOnFilter="True" DataField="DescrizioneTipologiaDocumento" FilterControlAltText="Filter DescrizioneTipologiaDocumento column"
                                                                            FilterControlWidth="100%" HeaderText="Tipologia" ShowFilterIcon="False"
                                                                            SortExpression="DescrizioneTipologiaDocumento" UniqueName="DescrizioneTipologiaDocumento" HeaderTooltip="Tipologia">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width:100% ;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("DescrizioneTipologiaDocumento"), "'", "&#039;")%>'>
                                                                                    <%# Eval("DescrizioneTipologiaDocumento")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                       


                                                                        <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains"  ItemStyle-Width="85px"
                                                                            HeaderStyle-Width="85px"
                                                                            AutoPostBackOnFilter="True" DataField="NomeModulo" FilterControlAltText="Filter NomeModulo column"
                                                                            FilterControlWidth="100%" HeaderText="Modulo" ShowFilterIcon="False"
                                                                            SortExpression="NomeModulo" UniqueName="NomeModulo" HeaderTooltip="Nome del Modulo">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width:100% ;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("NomeModulo"), "'", "&#039;")%>'>
                                                                                    <%# Eval("NomeModulo")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>




                                                                         <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" 
                                                                            AutoPostBackOnFilter="True" DataField="Oggetto" FilterControlAltText="Filter Oggetto column"
                                                                            FilterControlWidth="100%" HeaderText="Oggetto" ShowFilterIcon="False"
                                                                            SortExpression="Oggetto" UniqueName="Oggetto" HeaderTooltip="Oggetto">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width:100% ;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("Oggetto"), "'", "&#039;")%>'>
                                                                                    <%# Eval("Oggetto")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                       
                                                                       

                                                                         <telerik:GridTemplateColumn  AndCurrentFilterFunction="Contains" ItemStyle-Width="200px"
                                                                            HeaderStyle-Width="200px" AutoPostBackOnFilter="True" DataField="DescrizioneSettore"
                                                                            FilterControlAltText="Filter DescrizioneSettore column" FilterControlWidth="100%"
                                                                            HeaderText="Settore" ShowFilterIcon="False" SortExpression="DescrizioneSettore"
                                                                            UniqueName="DescrizioneSettore" HeaderTooltip="Settore">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("DescrizioneSettore"), "'", "&#039;")%>'>
                                                                                    <%# Eval("DescrizioneSettore")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                       

                                                                       

                                                                      

                                                                        <telerik:GridButtonColumn ItemStyle-Width="30px" HeaderStyle-Width="30px" ButtonType="ImageButton"
                                                                            CommandName="Anteprima" FilterControlAltText="Filter Anteprima column" ImageUrl="~/images/knob-search16.png"
                                                                            UniqueName="Anteprima" Text="Anteprima Determina/Ordinanza" HeaderTooltip="Anteprima Determina/Ordinanza">
                                                                        </telerik:GridButtonColumn>

                                                                    </Columns>

                                                                </MasterTableView>

                                                            </telerik:RadGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>

                                            <%-- <asp:RadioButton ID="rb1" runat="server" GroupName="rbGroup" Text="RB1" />
 <asp:RadioButton ID="rb2" runat="server" GroupName="rbGroup" Text="RB2" />
 <asp:RadioButton ID="rb3" runat="server" GroupName="rbGroup" Text="RB3" />
 <asp:RadioButton ID="rb4" runat="server" GroupName="rbGroup" Text="RB4" />--%>


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
                                                    <asp:Label ID="TitoloControlloSuccessivoLabel" runat="server" 
                                                        Style="color: #00156E" Font-Bold="True" 
                                                        Text="Modifica Parametri Controllo Successivo Regolarità Amministrativa" 
                                                        CssClass="Etichetta" />
                                                </td>
                                                <td align="right">
                                                    <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HidePanel();hide=true;"  />
                                                </td>                           
                                            </tr>
                                         </table>
                                     </td>                                
                                 </tr>
                                 <%-- BODY--%>
                                 <tr>
                                     <td class="ContainerMargin">
                                         <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0" style="background-color: #BFDBFF">
                                            <tr>
                                                <td>
                                                 

                                                        <telerik:RadTabStrip ID="ControlloSuccessivoTabStrip" runat="server" MultiPageID="ControlloSuccessivoMultiPage"
                                                            SelectedIndex="0" Skin="Office2007" Width="100%">
                                                            <Tabs>
                                                                <telerik:RadTab Selected="True"  Text="Generale" Style="text-align: center" />
                                                                <telerik:RadTab  Text="Tipologie Atti Soggetti a Controllo" Style="text-align: center" />

                                                            </Tabs>
                                                        </telerik:RadTabStrip>

                                                        <telerik:RadMultiPage ID="ControlloSuccessivoMultiPage" runat="server" BorderColor="#3399FF"
                                                            CssClass="multiPage" Height="100%" SelectedIndex="0">
                                                             
                                                            <telerik:RadPageView ID="GeneralePageView" runat="server" CssClass="corporatePageView"
                                                                Height="225px">

                                                                   <table style="width: 100%">


                                                            <tr>
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="RuoloLabel" runat="server" CssClass="Etichetta" Text="Ruolo *" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                    <telerik:RadComboBox ToolTip="Ruolo" ID="RuoloComboBox" runat="server" Skin="Office2007"
                                                                        Width="100%" AutoPostBack="False" Filter="StartsWith" MaxHeight="250px" NoWrap="True" />
                                                                </td>
                                                            </tr>

                                                             <tr>
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="NomeFileIterLabel" runat="server" CssClass="Etichetta" Text="Iter *" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                    <telerik:RadTextBox ID="NomeFileIterLabelTextBox" runat="server" Skin="Office2007"
                                                                        Width="100%" ToolTip="Nome file Iter" MaxLength="250" />
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="SegretarioLabel" runat="server" CssClass="Etichetta" Text="Segretario *" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                    <telerik:RadComboBox ToolTip="Segretario" ID="UtentiComboBox" runat="server" Skin="Office2007"
                                                                        Width="100%" AutoPostBack="False" Filter="StartsWith" MaxHeight="250px" NoWrap="True" />
                                                                </td>
                                                            </tr>


                                                            

                                                           

                                                            <tr>
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="NomeFileTemplateLabel" runat="server" CssClass="Etichetta" Text="Template *" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                    <telerik:RadTextBox ID="NomeFileTemplateTextBox" runat="server" Skin="Office2007"
                                                                        Width="100%" ToolTip="Nome file Iter" MaxLength="250" />
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="PercentualeLabel" runat="server" CssClass="Etichetta" Text="Percentuale *" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                    <telerik:RadNumericTextBox ID="PercentualeTextBox" runat="server" Skin="Office2007"
                                                                        Width="75px" DataType="System.Int32" MaxLength="3" MaxValue="100" MinValue="1"
                                                                        ShowSpinButtons="True" ToolTip="Percentuale">
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                    </telerik:RadNumericTextBox>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="PeriodicitaLabel" runat="server" CssClass="Etichetta" Text="Periodicità *" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                    <telerik:RadNumericTextBox ID="PeriodicitaTextBox" runat="server" Skin="Office2007"
                                                                        Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="365" MinValue="1"
                                                                        ShowSpinButtons="True" ToolTip="Periodicità">
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                    </telerik:RadNumericTextBox>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="ControlloreLabel" runat="server" CssClass="Etichetta" Text="Controllore *" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                    <telerik:RadTextBox ID="ControlloreTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                        ToolTip="Controllore" MaxLength="250" />
                                                                </td>
                                                            </tr>

                                                             <tr>
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="CriterioSceltaLabel" runat="server" CssClass="Etichetta" Text="Criterio Scelta *" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                    <telerik:RadComboBox ToolTip="Criterio Scelta" ID="CriterioSceltaComboBox" runat="server" Skin="Office2007"
                                                                        Width="100%" AutoPostBack="False" Filter="StartsWith" MaxHeight="250px" NoWrap="True" />
                                                                </td>
                                                            </tr>

                                                        </table>

                                                            </telerik:RadPageView>

                                                             <telerik:RadPageView ID="TipologiaAttoPageView" runat="server" CssClass="corporatePageView"
                                                                Height="225px">


                                                                     <div id="TipologiaAttoPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                        <tr>
                                                                            <td>
                                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="TipologiaAttoLabel" runat="server" Font-Bold="True" Style="width: 300px;
                                                                                                color: #00156E; background-color: #BFDBFF" Text="Tipologie Atti Soggetti a Controllo" />
                                                                                        </td>
                                                                                        <td style="text-align: right">
                                                                                           <%-- <asp:ImageButton ID="AggiungiConvenzioneBancariaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                ToolTip="Aggiungi Convenzione Bancaria" ImageAlign="AbsMiddle" BorderStyle="None"
                                                                                                TabIndex="12" /><asp:ImageButton ID="AggiornaConvenzioneBancariaImageButton" runat="server"
                                                                                                    Style="display: none" />--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div style="overflow: auto; height: 185px; width: 100%; background-color: #FFFFFF;
                                                                                    border: 0px solid #5D8CC9;">
                                                                                    <telerik:RadGrid ID="TipologiaAttoGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        CellSpacing="0" Culture="it-IT" GridLines="None" PageSize="5" Skin="Office2007"
                                                                                        >
                                                                                        <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                            <Columns>

                                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter column"
                                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="false" />


                                                                                                <telerik:GridTemplateColumn DataField="Descrizione"  HeaderText="Descrizione"
                                                                                                    SortExpression="Descrizione" UniqueName="Descrizione" HeaderTooltip="Descrizione">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                            text-overflow: ellipsis; width: 100%;">
                                                                                                            <%# Eval("Descrizione")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>


                                                                                                <telerik:GridTemplateColumn DataField="NomeModulo" HeaderStyle-Width="140px" HeaderText="Modulo"
                                                                                                    ItemStyle-Width="140px" SortExpression="NomeModulo" UniqueName="NomeModulo" HeaderTooltip="Modulo">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# Eval("NomeModulo")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                            width: 100%;">
                                                                                                            <%# Eval("NomeModulo")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>

                                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="75px" ItemStyle-Width="75px" DataField="SoggettoControllo"
                                                                                                    FilterControlAltText="Filter SoggettoControllo column" HeaderText="Sog. Contr." SortExpression="SoggettoControllo"
                                                                                                    UniqueName="SoggettoControllo" HeaderTooltip="Soggetto a Controllo">
                                                                                                    <ItemTemplate>
                                                                                                        <div title='<%# IIf(CBool(Eval("SoggettoControllo")), "SI", "NO")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                            <%# IIf(CBool(Eval("SoggettoControllo")), "SI", "NO")%>
                                                                                                        </div>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>

                                                                                              

                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="30px"
                                                                                                    Text="Modifica..." ItemStyle-Width="30px" FilterControlAltText="Filter Select column"
                                                                                                    ImageUrl="~\images\edit16.png" UniqueName="Select" />

                                                                                    <%--            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" HeaderStyle-Width="30px"
                                                                                                    ItemStyle-Width="30px" FilterControlAltText="Filter Delete column" ImageUrl="~\images\Delete16.png"
                                                                                                    UniqueName="Delete" Text="Elimina Convenzione Bancaria" />--%>

                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>




                                                            </telerik:RadPageView>

                                                        </telerik:RadMultiPage>



                                                     

                                                
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%-- FOOTER--%>
                            <tr>
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">

                                    <telerik:RadButton ID="SalvaButton" runat="server" Text="Salva" Width="90px" Skin="Office2007"
                                        ToolTip="Salva" >
                                        <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>&nbsp;
                                  
                                    <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="90px"
                                        Skin="Office2007" ToolTip="Chiudi" >
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
               

               
              <asp:HiddenField runat="server" ID="HideWindow" />
          
                
            
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
