<%@ Page Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="RicercaDatiContabiliPage.aspx.vb" Inherits="RicercaDatiContabiliPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">


    <script type="text/javascript">


        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");

       
        Sys.Application.add_init(function () {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        });


        function pageLoad() {
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

             <telerik:RadFormDecorator ID="RadFormDecorator3" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID3" Skin="Web20"></telerik:RadFormDecorator>

                     <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID4" Skin="Web20"></telerik:RadFormDecorator>

                     <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID5" Skin="Web20"></telerik:RadFormDecorator>
               
                <center>

                   <table width="100%" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">

                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                                        &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                            Text="Filtro Determine" />
                                                    </td>

                                                    <td style=" width:250px;background-color: #BFDBFF; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8;">
                                                    <table style="width: 100%">
                                                            <tr>
                                                                <td style=" width:140px">
                                                                    <asp:Label ID="AnnoDeterminaLabel" runat="server" Font-Bold="True" CssClass="Etichetta" Style="width: 140px;
                                                                        color: #00156E" Text="Anno Determina" />
                                                                </td>
                                                                <td>
                                                                    <telerik:RadComboBox ID="AnnoDeterminaComboBox" runat="server" MaxHeight="150" ItemsPerRequest="5"
                                                                        Skin="Office2007" Width="70px" ToolTip="Seleziona l'anno della determina" EmptyMessage="Tutti" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center" style="width: 30px; background-color: #BFDBFF; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8;">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Effettua la ricerca con i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px; background-color: #BFDBFF; border-bottom: 1px solid  #9ABBE8;
                                                        border-top: 1px solid  #9ABBE8;">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td align="center" style="height: 50px">

                                                        

                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>

                                                                   

                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                       <td style="width: 30px">

                                                                           <div id="ZoneID3">
                                                                               <asp:CheckBox ID="ImpegnoSpesaCheckBox" runat="server" Checked="True" CssClass="etichetta"
                                                                                   Text="&nbsp;" ToolTip="Se selezionato permette di applicare il filtro solo alle determine di impegno di spesa"
                                                                                   Width="25px" />
                                                                           </div>
                                                                        </td>

                                                                             <td style="width: 100px">
                                                                                <asp:Label ID="ImpegnoSpesaLabel" runat="server" Font-Bold="True" CssClass="Etichetta" Style="color: #00156E"
                                                                                    Text="Impegno" Width="100px" />
                                                                            </td>

                                                                       
                                                                             <td style="width: 50px; text-align:center">
                                                                                <asp:Label ID="AnnoImpegnoSpesaLabel" runat="server" CssClass="Etichetta" Text="Anno" Width="50px" />
                                                                            </td>

                                                                            <td style="width: 70px">
                                                                                <telerik:RadComboBox ID="AnnoImpegnoSpesaComboBox" runat="server" MaxHeight="100px" Skin="Office2007"
                                                                                    Width="70px" ToolTip="Seleziona l'anno di impegno" EmptyMessage="Tutti" />
                                                                            </td>

                                                                            <td style="width: 85px; text-align: right">
                                                                                <asp:Label ID="CapitoloImpegnoSpesaLabel" runat="server" CssClass="Etichetta" Text="Capitolo" Width="80" />
                                                                            </td>

                                                                            <td style="width: 100px">
                                                                                <telerik:RadNumericTextBox ID="CapitoloImpegnoSpesaTextBox" runat="server" Skin="Office2007" Width="100px"
                                                                                    DataType="System.Int32" MaxLength="6">
                                                                                    <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </td>

                                                                                <td style="width: 70px; text-align: right">
                                                                                    <asp:Label ID="ArticoloImpegnoSpesaLabel" runat="server" CssClass="Etichetta" Text="Articolo" Width="70px" />
                                                                                </td>

                                                                                <td style="width: 110px">
                                                                                    <telerik:RadNumericTextBox ID="ArticoloImpegnoSpesaTextBox" runat="server" Skin="Office2007" Width="110px"
                                                                                        DataType="System.Int32" MaxLength="6" ToolTip="Articolo">
                                                                                        <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                    </telerik:RadNumericTextBox>
                                                                                </td>

                                                                                <td style="width: 95px; text-align: right">
                                                                                    <asp:Label ID="NumeroImpegnoSpesaLabel" runat="server" CssClass="Etichetta" Text="Impegno" Width="90px" />
                                                                                </td>

                                                                                <td style="width: 200px">
                                                                                  


                                                                                       <telerik:RadTextBox ID="NumeroImpegnoSpesaTextBox" runat="server" Skin="Office2007" ToolTip="Numero dell'impegno"
                                                                                    Width="200px" MaxLength="50"  />

                                                                                </td>

                                                                                <td style="width: 95px; text-align: right">
                                                                                    <asp:Label ID="NumeroSubImpegnoSpesaLabel" runat="server" CssClass="Etichetta" Text="Sub-Impegno" Width="90px" />
                                                                                </td>

                                                                                <td>
                                                                                  

                                                                                        <telerik:RadTextBox ID="NumeroSubImpegnoSpesaTextBox" runat="server" Skin="Office2007" ToolTip="Numero del sub-impegno"
                                                                                    Width="200px" MaxLength="50"  />

                                                                                </td>

                                                                           
                                                                        
                                                                            

                                                                        </tr>
                                                                    </table>

                                                                    <table style="width: 100%">
                                                                        <tr>

                                                                        <td style="width: 30px">

                                                                         <div id="ZoneID4">

                                                                         <asp:CheckBox ID="LiquidazioneCheckBox" runat="server" Checked="True" CssClass="etichetta"
                                                                                        Text="&nbsp;" ToolTip="Se selezionato permette di applicare il filtro solo alle determine di liquidazione"
                                                                                        Width="25px" />
                                                                           </div>
                                                                        </td>

                                                                             <td style="width: 100px">
                                                                                <asp:Label ID="LiquidazioneLabel" runat="server" Font-Bold="True" CssClass="Etichetta" Style="color: #00156E"
                                                                                    Text="Liquidazione" Width="100px" />
                                                                            </td>
                                                                       
                                                                             <td style="width: 50px; text-align:center">
                                                                                <asp:Label ID="AnnoLiquidazioneLabel" runat="server" CssClass="Etichetta" Text="Anno" Width="50px" />
                                                                            </td>

                                                                             <td style="width: 70px">
                                                                                <telerik:RadComboBox ID="AnnoLiquidazioneComboBox" runat="server" MaxHeight="100px" Skin="Office2007"
                                                                                    ToolTip="Seleziona l'anno di Liquidazione" Width="70px" EmptyMessage="Tutti" />
                                                                            </td>

                                                                            <td style="width: 85px; text-align: right">
                                                                                <asp:Label ID="NumeroLiquidazioneLabel" runat="server" CssClass="Etichetta" Text="Liquidazione" Width="80px" />
                                                                            </td>

                                                                             <td style="width: 100px">
                                                                                <telerik:RadNumericTextBox ID="NumeroLiquidazioneTextBox" runat="server" Skin="Office2007"
                                                                                    Width="100px" DataType="System.Int32" MaxLength="7" ToolTip="Numero della liquidazione">
                                                                                    <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </td>
                                                                            <td style="width: 70px; text-align: right">
                                                                                <asp:Label ID="MandatoLiquidazioneLabel" runat="server" CssClass="Etichetta" Text="Mandato" Width="70px" />
                                                                            </td>

                                                                             <td style="width: 110px">
                                                                                <telerik:RadTextBox ID="MandatoLiquidazioneTextBox" runat="server" Skin="Office2007" ToolTip="Mandato della liquidazione"
                                                                                    Width="110px" MaxLength="100"  />
                                                                            </td>

                                                                            <td style="width: 95px; text-align: right">
                                                                                <asp:Label ID="DatoFiscaleLiquidazioneLabel" runat="server" CssClass="Etichetta" Text="C.F. - P. IVA" Width="90px" />
                                                                            </td>

                                                                            <td style="width: 200px">
                                                                                <telerik:RadTextBox ToolTip="Codice fiscale o partita IVA del beneficiario" ID="DatoFiscaleLiquidazioneTextBox"
                                                                                    runat="server" Skin="Office2007" Width="200px" MaxLength="16"  />
                                                                            </td>
                                                                       
                                                                           <td style="width: 95px; text-align: right">
                                                                                <asp:Label ID="BeneficiarioLiquidazioneLabel" runat="server" CssClass="Etichetta" Text="Beneficiario" Width="90px" />
                                                                            </td>

                                                                           <td>
                                                                                <telerik:RadTextBox ToolTip="Beneficiario della liquidazione" ID="BeneficiarioLiquidazioneTextBox"
                                                                                    runat="server" Skin="Office2007" Width="300px" />
                                                                            </td>

                                                                            
                                                                       
                                                                    

                                                                        </tr>
                                                                    </table>

                                                                    <table style="width: 100%">

                                                                        <tr>

                                                                         <td style="width: 30px">

                                                                          <div id="ZoneID5">
                                                                         <asp:CheckBox ID="AccertamentoCheckBox" runat="server" Checked="True" CssClass="etichetta"
                                                                                        Text="&nbsp;" ToolTip="Se selezionato permette di applicare il filtro solo alle determine di accertamento"
                                                                                        Width="25px" />
                                                                            </div>
                                                                        </td>

                                                                            <td style="width: 100px">
                                                                                <asp:Label ID="AccertamentoLabel" runat="server" Font-Bold="True" CssClass="Etichetta"
                                                                                    Style="color: #00156E" Text="Accertamento" Width="100px" />
                                                                            </td>

                                                                            <td style="width: 50px; text-align:center">
                                                                                <asp:Label ID="AnnoAccertamentoLabel" runat="server" CssClass="Etichetta" Text="Anno" Width="50px" />
                                                                            </td>

                                                                            <td style="width: 70px">
                                                                                <telerik:RadComboBox ID="AnnoAccertamentoComboBox" runat="server" MaxHeight="100px" Skin="Office2007"
                                                                                    ToolTip="Seleziona l'anno di accertamento" Width="70px" EmptyMessage="Tutti" />
                                                                            </td>

                                                                            <td style="width: 85px; text-align: right">
                                                                                <asp:Label ID="CapitoloAccertamentoLabel" runat="server" CssClass="Etichetta" Text="Capitolo" Width="80px" />
                                                                            </td>

                                                                            <td style="width: 100px">
                                                                                <telerik:RadNumericTextBox ID="CapitoloAccertamentoTextBox" runat="server" DataType="System.Int32"
                                                                                    MaxLength="6" Skin="Office2007" ToolTip="Capitolo" Width="100px">
                                                                                    <NumberFormat AllowRounding="False" DecimalDigits="0" GroupSeparator="" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </td>

                                                                            <td style="width: 70px; text-align: right">
                                                                                <asp:Label ID="ArticoloAccertamentoLabel" runat="server" CssClass="Etichetta" Text="Articolo" Width="70px" />
                                                                            </td>
                                                                            <td style="width: 110px">
                                                                                <telerik:RadNumericTextBox ID="ArticoloAccertamentoTextBox" runat="server" Skin="Office2007" Width="110px"
                                                                                    DataType="System.Int32" MaxLength="6" ToolTip="Articolo dell'accertamento">
                                                                                    <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </td>

                                                                            <td style="width: 95px; text-align: right">
                                                                                <asp:Label ID="NumeroAccertamentoLabel" runat="server" CssClass="Etichetta" Text="Accertamento" Width="90px" />
                                                                            </td>

                                                                             <td>

                                                                                <telerik:RadNumericTextBox ID="NumeroAccertamentoTextBox" runat="server" Skin="Office2007" Width="140px"
                                                                                    DataType="System.Int32" MaxLength="7"  ToolTip="Numero dell'accertamento">
                                                                                    <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </td>
                                                                            
                                                                        </tr>
                                                                    </table>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;<asp:Label ID="TitoloElencoLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Elenco Determine" />
                                                                </td>

                                                                <td align="center" style="width: 40px">
                                                                    <asp:ImageButton ID="EsportaInExcelImageButton" Style="border: 0" runat="server"
                                                                        ImageUrl="~/images//excel32.png" ToolTip="Esporta le determine visualizzate in un file formato excel"
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

                                                                <MasterTableView  DataKeyNames="Id" TableLayout="Fixed">
                                                                
                                                                   
                                                                    <NoRecordsTemplate>
                                                                        <div>
                                                                            Nessuna Determina Trovata</div>
                                                                    </NoRecordsTemplate>
                                                                    <Columns>

                                                                 
                                                                  
                                                                        <telerik:GridTemplateColumn DataField="IdStato" UniqueName="IdStato" ItemStyle-Width="30px"
                                                                            HeaderStyle-Width="30px" AllowFiltering="False" HeaderTooltip="Dato Contabile">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="IdStato" runat="server" /></ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                      
                                                                  

                                                                       
                                                                        <telerik:GridBoundColumn  AutoPostBackOnFilter="True" FilterControlWidth="100%" DataField="ContatoreGenerale" FilterControlAltText="Filter ContatoreGenerale column"
                                                                            HeaderText="N." SortExpression="ContatoreGenerale" UniqueName="ContatoreGenerale"
                                                                            ShowFilterIcon="False"  ItemStyle-Width="60px" HeaderStyle-Width="60px" AndCurrentFilterFunction="EqualTo"
                                                                            HeaderTooltip="Numero Contatore Generale">
                                                                        </telerik:GridBoundColumn>


                                                                        <telerik:GridTemplateColumn  AndCurrentFilterFunction="Contains" ItemStyle-Width="170px"
                                                                            HeaderStyle-Width="170px" AutoPostBackOnFilter="True" DataField="DescrizioneTipologia"
                                                                            FilterControlAltText="Filter DescrizioneTipologia column" FilterControlWidth="100%"
                                                                            HeaderText="Tipologia" ShowFilterIcon="False" SortExpression="DescrizioneTipologia"
                                                                            UniqueName="DescrizioneTipologia" HeaderTooltip="Tipologia">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width:100%;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("DescrizioneTipologia"), "'", "&#039;")%>'>
                                                                                    <%# Eval("DescrizioneTipologia")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>


                                                                         <telerik:GridBoundColumn AllowFiltering="False" DataField="DataDocumento"
                                                                            DataType="System.DateTime" FilterControlAltText="Filter DataDocumento column"
                                                                            HeaderText="Data" ShowFilterIcon="False" SortExpression="DataDocumento"
                                                                            UniqueName="DataDocumento" DataFormatString="{0:dd/MM/yyyy}"
                                                                            HeaderTooltip="Data della Determina" ItemStyle-Width="85px" HeaderStyle-Width="85px">
                                                                        </telerik:GridBoundColumn>


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
                                                                            HeaderStyle-Width="200px" AutoPostBackOnFilter="True" DataField="DescrizioneUfficio"
                                                                            FilterControlAltText="Filter DescrizioneUfficio column" FilterControlWidth="100%"
                                                                            HeaderText="Ufficio" ShowFilterIcon="False" SortExpression="DescrizioneUfficio"
                                                                            UniqueName="DescrizioneUfficio" HeaderTooltip="Ufficio">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100%;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("DescrizioneUfficio"), "'", "&#039;")%>'>
                                                                                    <%# Eval("DescrizioneUfficio")%>
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
                                                                            UniqueName="Anteprima" Text="Anteprima Determina" HeaderTooltip="Anteprima Determina">
                                                                        </telerik:GridButtonColumn>

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
                        <tr>
                            <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                border-top: 1px solid  #9ABBE8; height: 25px">
                            </td>
                        </tr>
                    </table>


                </center>

                

               

               

                <table style="width: 100%; border: 1px solid #9ABBE8; text-align: center">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 80px">
                                        <asp:Label ID="LegendaLabel" runat="server" CssClass="Etichetta" Text="Legenda :"
                                            Font-Bold="True" ForeColor="#FF9900" />
                                    </td>
                                    
                                    <td>
                                        <img alt="" src="../../../../Images/pArancio16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaProtocollata" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Impegno" Width="70px" />
                                    </td>
                                    <td>
                                        <img alt="" src="../../../../Images/pVerde16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaAccettata" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Accertamento" Width="100px" />
                                    </td>
                                    <td>
                                        <img alt="" src="../../../../Images/pRosso16.png" style="vertical-align: middle" />
                                        <asp:Label ID="LegendaRifiutata" runat="server" CssClass="Etichetta" Style="text-align: center"
                                            Text="Liquidazione" Width="90px" />
                                    </td>
                                    
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                
            
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
