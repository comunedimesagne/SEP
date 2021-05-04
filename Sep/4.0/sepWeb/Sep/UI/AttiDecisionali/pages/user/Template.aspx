<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="Template.aspx.vb" Inherits="UI_AttiDecisionali_pages_user_Template" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">


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

                var clientBounds = this.GetClientBounds();
                var clientWidth = clientBounds.width;
                var clientHeight = clientBounds.height;
                _backgroundElement.style.width = Math.max(Math.max(document.documentElement.scrollWidth, document.body.scrollWidth), clientWidth) + 'px';
                _backgroundElement.style.height = Math.max(Math.max(document.documentElement.scrollHeight, document.body.scrollHeight), clientHeight) + 'px';

                //                _backgroundElement.style.width = '100%';
                //                _backgroundElement.style.height = '100%';

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

        function GetClientBounds() {
            var clientWidth;
            var clientHeight;
            switch (Sys.Browser.agent) {
                case Sys.Browser.InternetExplorer:
                    clientWidth = document.documentElement.clientWidth;
                    clientHeight = document.documentElement.clientHeight;
                    break;
                case Sys.Browser.Safari:
                    clientWidth = window.innerWidth;
                    clientHeight = window.innerHeight;
                    break;
                case Sys.Browser.Opera:
                    clientWidth = Math.min(window.innerWidth, document.body.clientWidth);
                    clientHeight = Math.min(window.innerHeight, document.body.clientHeight);
                    break;
                default:  // Sys.Browser.Firefox, etc.
                    clientWidth = Math.min(window.innerWidth, document.documentElement.clientWidth);
                    clientHeight = Math.min(window.innerHeight, document.documentElement.clientHeight);
                    break;
            }
            return new Sys.UI.Bounds(0, 0, clientWidth, clientHeight);
        }


        var popupDiv;

        function ShowTooltip(e, message, w, h) {

            popupDiv = document.createElement('DIV');

            with (popupDiv) {
                style.fontFamily = 'Arial';
                style.fontWeight = 500;
                style.fontStyle = 'normal';
                style.fontSize = '10pt';
                style.color = '#00156E';
                style.backgroundColor = '#FFCB61';
                style.padding = '5px';
                // style.filter = "alpha(opacity=20)";
                // style.opacity = "0.2";
                //style.backgroundColor = 'lightyellow';
                style.border = 'solid #FF9B35 1px';

                style.filter = "-ms-filter: progid:DXImageTransform.Microsoft.gradient(GradientType=0,startColorstr='#FFDB9B', endColorstr='#FFCB61')";
                //style.filter = "filter: progid:DXImageTransform.Microsoft.gradient(GradientType=1,startColorstr='" + g1.value + "', endColorstr='" + g2.value + "')";
                style.width = w;
                style.height = h;
                id = 'myPopupDiv';
                style.position = 'absolute';
                style.display = 'block';
                innerHTML = message;
                style.borderRadius = '3px';
                style.MozBorderRadius = '3px';

                e = window.event || e;

                if (window.event) {
                    style.left = (e.x - w - 24) + 'px';
                    style.top = e.y + 'px';
                }
                else {
                    style.left = (e.x - w - 24 + 280) + 'px';
                    style.top = e.y + 80 + 'px';
                }

            }

            this.document.body.appendChild(popupDiv);

        }


        function HideTooltip() {
            try {
                popupDiv.style.display = 'none';
            }
            catch (e) { }
        }

        
    </script>


    <asp:UpdateProgress runat="server" ID="UpdateProgress1">
        <ProgressTemplate>
          <%--  <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 300px">
                <table cellpadding="4" style="background-color: #4892FF">
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
            </div>--%>
        </ProgressTemplate>
    </asp:UpdateProgress>


   
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>

            <div id="pageContent">

                <table style="width: 900px; border: 1px solid #5D8CC9">
                    <tr>
                        <td>

                            <%--INIZIO TOOLBAR--%>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                                            <Items>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                                    CommandName="Nuovo" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                    CommandName="Trova" Owner="RadToolBar" Visible="true" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveAndExit.png" Text="Salva e Chiudi"
                                                    CommandName="SalvaChiudi" Owner="RadToolBar" Visible="False" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar" />
                                              <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                    CommandName="Stampa" Owner="RadToolBar" />

                                                   
                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                                   
                                                     <telerik:RadToolBarButton runat="server" ImageUrl="~/images/AdvancedSearch32.png" Text="Ricerca Avanzata"
                                                    CommandName="RicercaAvanzata" Owner="RadToolBar" />

                                                    <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore2" Owner="RadToolBar" />

                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar" />
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>
                            <%--FINE TOOLBAR--%>


                            <table id="TabellaNotifica" style="width: 100%; background-color: #BFDBFF">

                                <tr style="height: 24px">
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    &nbsp;&nbsp;<asp:Label ID="AreaInfoLabel" runat="server" Font-Bold="True" Style="width: 550px;
                                                        color: #00156E; background-color: #BFDBFF" Text="" CssClass="Etichetta" />
                                                </td>
                                               <%-- <td align="right">
                                                    <asp:ImageButton ID="VisualizzaStoricoDocumentoImageButton" runat="server" ImageUrl="~/images//FolderHistory.png"
                                                        Style="border: 0px" ToolTip="Visualizza storico documento selezionato" ImageAlign="Top"
                                                        Visible="false" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="VisualizzaDocumentoFirmatoImageButton" runat="server" ImageUrl="~/images//DocumentoFirmato.gif"
                                                        Style="border: 0px" ToolTip="Visualizza documento firmato" ImageAlign="Top" Visible="false" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="VisualizzaCopiaDocumentoImageButton" runat="server" ImageUrl="~/images//DocumentoCopia.gif"
                                                        Style="border: 0px" ToolTip="Visualizza la copia conforme del documento" ImageAlign="Top"
                                                        Visible="false" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="VisualizzaDocumentoImageButton" runat="server" ImageUrl="~/images//Documento.gif"
                                                        Style="border: 0px;" ToolTip="Visualizza documento" ImageAlign="Top" Visible="false" />
                                                    &nbsp;
                                                </td>--%>
                                                <td align="center" style="width: 40px">
                                                    <img id="InfoUtenteImageButton" runat="server" src="~/images/userInfo.png" style="cursor: pointer;
                                                        border: 0px" alt="Informazioni sull'utente" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                               

                            </table>

                          


                            <telerik:RadTabStrip runat="server" ID="AttiTabStrip" SelectedIndex="0"  
                                MultiPageID="AttiMultiPage" Skin="Office2007" Width="100%">
                                <Tabs>
                                    <telerik:RadTab Text="Generale" Selected="True" />
                                   <telerik:RadTab Text="Documenti" />
                                  
                                </Tabs>
                            </telerik:RadTabStrip>
                            <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                            <telerik:RadMultiPage runat="server" ID="AttiMultiPage" SelectedIndex="0" Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                             
                             
                                <telerik:RadPageView runat="server" ID="GeneralePageView" CssClass="corporatePageView"
                                    Height="425px">

                                    <div style="padding: 2px 2px 2px 2px; width: 100%">

                                     
                                    
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 70px">
                                            <asp:Label ID="NumeroRegistroLabel" runat="server" CssClass="Etichetta" Text="Numero" Font-Italic="True"
                                                Width="90px" Style="text-align: right" />
                                        </td>
                                        <td style="width: 70px">
                                            <telerik:RadTextBox ID="NumeroRegistroTextBox" runat="server" Skin="Office2007" Width="70px" Enabled="false"
                                                BorderColor="Blue" />
                                        </td>
                                        <td style="width: 50px">
                                            <asp:Label ID="DataRegistrazioneLabel" runat="server" CssClass="Etichetta" Text="Data" Style="text-align: right"
                                                Width="50px" Font-Italic="True" />
                                        </td>
                                        <td style="width: 110px">
                                            <telerik:RadTextBox ID="DataRegistrazioneTextBox" Skin="Office2007" Width="110px" runat="server"
                                                Enabled="false" BorderColor="Blue" />
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Label ID="NumeroAttoLabel" runat="server" CssClass="Etichetta" Font-Italic="True"
                                                Style="text-align: right" Text="Numero Atto" Width="90px" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="NumeroAttoTextBox" runat="server" BorderColor="Blue" Enabled="false"
                                                Skin="Office2007" Width="40px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="TipiDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipologia *" Width="100px"
                                                Style="text-align: right" ForeColor="#FF8040" Font-Bold="True" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="TipiDocumentoComboBox" runat="server" Skin="Office2007"
                                                Width="300px" Height="140px" EmptyMessage="- Seleziona Tipologia -" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="AggiornaRicPro" runat="server" ImageUrl="~/images//knob-search16.png"
                                                Style="display: none" />
                                            <asp:ImageButton ID="RicPro" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/WF_PRO.gif"
                                                ToolTip="Seleziona il protocollo ..." />
                                        </td>
                                    </tr>
                                </table>

                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 90px">
                                                        <asp:Label ID="StrutturaLabel" runat="server" CssClass="Etichetta" Text="Struttura *"
                                                            Width="90px" Style="text-align: right" ForeColor="#FF8040" Font-Bold="True" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="StrutturaTextBox" runat="server" Skin="Office2007" Width="700px"
                                                            ToolTip="Struttura" />
                                                    </td>
                                                    <td align="right" style="width: 120px; vertical-align: top">
                                                        <asp:ImageButton ID="TrovaPrimoReferenteInternoImageButton" runat="server" ImageUrl="~/images//uffici.png"
                                                            ToolTip="Seleziona referente interno (ALT+1) ..." ImageAlign="AbsMiddle" /><asp:ImageButton
                                                                ID="TrovaReferenteEsternoIpaImageButton" runat="server" ImageUrl="~/images//ipasearch.png"
                                                                ToolTip="Seleziona referente esterno IPA..." ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 90px">
                                                        <asp:Label ID="EmailLabel" runat="server" CssClass="Etichetta" Text="E-Mails" Width="90px"
                                                            Style="text-align: right" />
                                                    </td>
                                                    <td colspan="2">
                                                        <telerik:RadTextBox ID="EmailTextBox" runat="server" Skin="Office2007" Width="730px"
                                                            ToolTip="Emails" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                                <table style="width: 100%">
                                    <tr>
                                        <td style="height: 25px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 90px">
                                                        <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto *" Width="90px"
                                                            Style="text-align: right" ForeColor="#FF8040" Font-Bold="True" />
                                                    </td>
                                                    <td colspan="2">
                                                        <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="700px"
                                                            ToolTip="Oggetto" TextMode="MultiLine" Rows="3" Height="60px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                                <table style="width: 100%; border: 0px solid lightBlue" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="PubblicazioneLabel" runat="server" CssClass="Etichetta" Text="Pubblicazione"
                                                Width="90px" />
                                        </td>
                                        <td style=" width:100px">
                                            <asp:Label ID="DataInizioPubblicazioneLabel" runat="server" CssClass="Etichetta"
                                                Text="Data Inizio *" Width="90px" ForeColor="#FF8040" Font-Bold="True" />
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="DataInizioPubblicazioneTextBox" Skin="Office2007" Width="100px"
                                                runat="server" MinDate="1753-01-01" />
                                        </td>
                                          <td style=" width:40px">
                                            <asp:RadioButton ID="UsaGiorniRadioButton" runat="server" GroupName="Modo" 
                                                Text="&nbsp;" />
                                        </td>
                                         <td style=" width:60px">
                                            <asp:Label ID="GiorniLabel" runat="server" CssClass="Etichetta" Text="Giorni" 
                                                Width="50px" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="GiorniTextBox" runat="server" Skin="Office2007" Width="40px"
                                                MaxLength="3" />
                                        </td>

                                        <td style=" width:40px">
                                            <asp:RadioButton ID="UsaDataFineRadioButton" runat="server" GroupName="Modo" 
                                                Text="&nbsp;" />
                                        </td>
                                         <td style=" width:100px">
                                            <asp:Label ID="DataFinePubblicazioneLabel" runat="server" CssClass="Etichetta" 
                                                ForeColor="#FF8040" Text="Data Fine *" Width="90px" Font-Bold="True" />
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="DataFinePubblicazioneTextBox" runat="server" MinDate="1753-01-01"
                                                Skin="Office2007" Width="100px" />
                                        </td>
                                    </tr>
                                </table>


                                    </div>
                                </telerik:RadPageView>
                             
                           
                                <telerik:RadPageView runat="server" ID="DocumentiPageView" CssClass="corporatePageView"
                                    Height="425px">
                                    <div style="padding: 2px 2px 2px 2px; width: 100%">
                                        <table style="width: 100%; background-color: #DFE8F6">
                                            <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="TipoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipo" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="DescrizioneDocumentoLabel" runat="server" CssClass="Etichetta" Text="Descrizione" />
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="DescrizioneDocumentoTextBox" runat="server" Skin="Office2007"
                                                        Width="250px" />
                                                </td>
                                            </tr>
                                            <tr style="height: 30px">
                                                <td style="width: 90px">
                                                    <asp:Label ID="NomeFileDocumentoLabel" runat="server" CssClass="Etichetta" Text="Nome file" />
                                                </td>
                                                <td>
                                                    <telerik:RadAsyncUpload ID="AllegatoUpload" runat="server" MaxFileInputsCount="1"
                                                        Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                        <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                    </telerik:RadAsyncUpload>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%; background-color: #DFE8F6">
                                            <tr>
                                                <td style="width: 140px">
                                                    <asp:Label ID="OpzioniScannerLabel" runat="server" CssClass="Etichetta" Text="Opzioni scanner" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="FronteRetroLabel" runat="server" CssClass="Etichetta" Text="Fronte retro" />&nbsp;<asp:CheckBox
                                                        ID="FronteRetroCheckBox" runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label
                                                            ID="VisualizzaUILabel" runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                ID="VisualizzaUICheckBox" runat="server" Text="" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Documenti" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:ImageButton ID="ScansionaImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                    ToolTip="Allega documento digitalizzato" TabIndex="44" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                        ID="AggiungiDocumentoImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                        ToolTip="Allega documento" TabIndex="43" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                            ID="ScanUploadButton" Style="display: none" runat="server" ImageUrl="~/images//RecycleEmpty.png" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div style="overflow: auto; height: 240px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="AllegatiGridView" runat="server" ToolTip="Elenco allegati associati al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            Width="99.8%" Culture="it-IT">
                                                            <MasterTableView DataKeyNames="Id, Nomefile">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="N." HeaderStyle-Width="10px"
                                                                        ItemStyle-Width="10px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="NumeratoreLabel" runat="server" Width="10px" /></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                        DataField="NomeFile" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 250px">
                                                                           <%# Eval("NomeFile")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Impronta" UniqueName="Impronta" HeaderText="Impronta"
                                                                        DataField="Impronta" HeaderStyle-Width="260px" ItemStyle-Width="260px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Impronta")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 260px;">
                                                                             <%# Eval("Impronta")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                        DataField="Oggetto" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 150px;">
                                                                             <%# Eval("Oggetto")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="10px"
                                                                        ItemStyle-Width="10px">
                                                                    </telerik:GridButtonColumn>
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="10px"
                                                                        ItemStyle-Width="10px">
                                                                    </telerik:GridButtonColumn>
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>

                              

                             

                                </telerik:RadMultiPage>

                            <%--INIZIO GRIDVIEW--%>


                            <asp:ImageButton ID="AggiornaAttiImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                Style="display: none" />

                            


                        

                            <asp:Panel runat="server" ID="DocumentiPanel">
                                <table style="width: 100%; background-color: #BFDBFF">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloElencoAttiLabel" runat="server" Font-Bold="True" CssClass="Etichetta"
                                                            Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Pubblicazioni"
                                                            
                                                            ToolTip="Ultimi cinque atti amministrativi registrati dall'utente corrente" />
                                                    </td>
                                                  <%--  <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Filtra registrazioni" Style="border-style: none; border-color: inherit;
                                                            border-width: 0; width: 16px;" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="RipristinaFiltroInizialeImageButton" Style="border: 0" runat="server"
                                                            ImageUrl="~/images//cancelSearch.png" ToolTip="Ripristina filtro iniziale" ImageAlign="AbsMiddle" />
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                           <div style="overflow: auto; height: 185px; width: 100%; background-color: #FFFFFF;
                                                border: 1px solid #5D8CC9;">
                                            <telerik:RadGrid ID="DocumentiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" PageSize="5"
                                                Culture="it-IT">
                                                <MasterTableView DataKeyNames="Id">
                                                      <Columns>    
                                                                                     
                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" Visible="False"/>

                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="16px" ItemStyle-Width="16px">
                                                    <ItemTemplate><asp:CheckBox ID="SelChk" runat="server" /></ItemTemplate>
                                                    <HeaderTemplate><asp:CheckBox ID="SelTChk" runat="server" /></HeaderTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridBoundColumn SortExpression="NumeroRegistro" UniqueName="NumeroRegistro" HeaderText="N. Reg." DataField="NumeroRegistro" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center"/>                                                                                          
                                                <telerik:GridTemplateColumn SortExpression="Struttura" UniqueName="Struttura" HeaderText="Struttura" DataField="Struttura">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Struttura")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 140px;"><%# Eval("Struttura")%></div>                                                      
                                                </ItemTemplate></telerik:GridTemplateColumn>
                                                
                                                <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Atto" DataField="Oggetto">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 140px;"><%# Eval("Oggetto")%></div>                                                      
                                                </ItemTemplate></telerik:GridTemplateColumn>
                                               
                                                <telerik:GridTemplateColumn SortExpression="DescrizioneTipologia" UniqueName="DescrizioneTipologia" HeaderText="Tipo" DataField="DescrizioneTipologia">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DescrizioneTipologia")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100px;"><%# Eval("DescrizioneTipologia")%></div>                                                      
                                                </ItemTemplate></telerik:GridTemplateColumn>
                                               
                                                <telerik:GridBoundColumn SortExpression="ContatoreDocumento" UniqueName="ContatoreDocumento" HeaderText="N. Atto" DataField="ContatoreDocumento" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center"/>                                                                                          
                                                
                                                <telerik:GridTemplateColumn SortExpression="DataInizioPubblicazione" UniqueName="DataInizioPubblicazione" HeaderText="Data Inizio" DataField="DataInizioPubblicazione" HeaderStyle-Width="90px">
                                                       <ItemTemplate>
                                                            <div title='<%# Eval("DataInizioPubblicazione","{0:dd/MM/yyyy}")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 90px;"><%# Eval("DataInizioPubblicazione", "{0:dd/MM/yyyy}")%></div>                                                        
                                                </ItemTemplate></telerik:GridTemplateColumn>
                                                
                                                <telerik:GridTemplateColumn SortExpression="DataFinePubblicazione" UniqueName="DataFinePubblicazione" HeaderText="Data Fine" DataField="DataFinePubblicazione" HeaderStyle-Width="90px">
                                                       <ItemTemplate>
                                                            <div title='<%# Eval("DataFinePubblicazione","{0:dd/MM/yyyy}")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 90px;"><%# Eval("DataFinePubblicazione", "{0:dd/MM/yyyy}")%></div>                                                        
                                                </ItemTemplate></telerik:GridTemplateColumn>
                                                
                                                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="Select" CommandName="Select" ImageUrl="~/images/Checks.png" Text="Selezione"/>
                                                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="LetTrasm" CommandName="LetTrasm" ImageUrl="~/images/text.png" Text="Stampa della lettera di trasmissione"/>
                                                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="Firma" CommandName="Firma" ImageUrl="~/images/signedDocument16.png" Text="Firma digitale del documento da pubblicare"/>  
                                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="Pubblica" CommandName="Pubblica" ImageUrl="~/images/internet.gif" Text="Pubblica On-Line il documento"/>     --%>                                          
                                          
                                            </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel runat="server" ID="Panel1" Style="background-color: #BFDBFF">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadButton Visible="False" ID="ChiudiButton" runat="server" Text="Chiudi"
                                                Width="100px" Skin="Office2007" ToolTip="Chiudi la finestra">
                                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <div style="display: none">
                                <asp:Button runat="server" ID="DisabilitaPulsantePredefinito" Style="width: 0px;
                                    height: 0px; display: none" />
                            </div>
                            <%--FINE GRIDVIEW--%>
                        </td>
                    </tr>
                </table>
            </div>


               <asp:ImageButton ID="salvaContenutoButton" runat="server" ImageUrl="~/images//knob-search16.png"  style="display: none; width:0px" />
               <asp:HiddenField ID="documentContentHidden" runat="server" />
               <asp:HiddenField ID="infoScansioneHidden" runat="server" />

              <asp:ImageButton  ID="AggiornaFirmaImageButton" runat="server" style="display: none" />
                <asp:ImageButton  ID="EliminaLockDocumdentoImageButton" runat="server" style="display: none"  />

        </ContentTemplate>
     
    </asp:UpdatePanel>








    
</asp:Content>

