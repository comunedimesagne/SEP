<%@ Page Title="" Language="VB" MasterPageFile="~/BasePage.master" AutoEventWireup="false"
    CodeFile="StoricoAttiAmministrativiPage.aspx.vb" Inherits="StoricoAttiAmministrativiPage" %>

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




        var popupDiv;


        function ShowTooltip(e, message, w, h) {

            HideTooltip();

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

                style.zIndex = 10000;

                e = window.event || e;

                if (window.event) {

                    style.left = (e.clientX - w - 20) + 'px';
                    style.top = (e.clientY) + 'px';

                }
                else {
                    style.left = (e.x - w - 24 + 280) + 'px';
                    style.top = (e.y + 60) + 'px';
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

        function CurriculumSelezionato() {
            var divAggCurriculum = document.getElementById('divAggCurriculum');
            divAggCurriculum.style.display = 'block'
        }

        function CurriculumRimosso() {
            var divAggCurriculum = document.getElementById('divAggCurriculum');
            divAggCurriculum.style.display = 'none'
        }


        function InconsistenzaSelezionato() {
            var divAggProgetto = document.getElementById('divAggInconsistenza');
            divAggProgetto.style.display = 'block'
        }

        function InconsistenzaRimosso() {
            var divAggProgetto = document.getElementById('divAggInconsistenza');
            divAggProgetto.style.display = 'none'
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
                    <asp:Panel ID="RisultatiPanel" runat="server">
                        <table width="900px" cellpadding="2" cellspacing="2" border="0">
                            <tr>
                                <td>
                                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                                border-top: 1px solid  #9ABBE8; height: 25px">
                                                &nbsp;<asp:Label ID="ElencoDocumetiLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                    CssClass="Etichetta" Text="Elenco Storico Atti Amministrativi" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ContainerMargin">
                                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                    <tr>
                                                        <td>
                                                            <div id="scrollPanelDocumenti" style="overflow: auto; height: 538px; width: 100%;
                                                                background-color: #FFFFFF; border: 0px solid #5D8CC9;">
                                                                <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco firme associate al documento"
                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                    Width="99.8%" Culture="it-IT">
                                                                    <MasterTableView DataKeyNames="Id">
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                            <telerik:GridTemplateColumn SortExpression="NumVersione" UniqueName="NumVersione"
                                                                                HeaderText="Versione" DataField="NumVersione" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("NumVersione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 50px; border: 0px solid red;">
                                                                                        <%# Eval("NumVersione")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="LogDataRegistrazione" UniqueName="LogDataRegistrazione"
                                                                                HeaderText="Data" DataField="LogDataRegistrazione" HeaderStyle-Width="110px"
                                                                                ItemStyle-Width="110px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("LogDataRegistrazione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 110px; border: 0px solid red;">
                                                                                        <%# Eval("LogDataRegistrazione")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="LogUtente" UniqueName="LogUtente" HeaderText="Utente"
                                                                                DataField="LogUtente" HeaderStyle-Width="160px" ItemStyle-Width="160px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("LogUtente")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 160px; border: 0px solid red;">
                                                                                        <%# Eval("LogUtente")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                                DataField="Oggetto" HeaderStyle-Width="450px" ItemStyle-Width="450px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 450px; border: 0px solid red;">
                                                                                        <%# Eval("Oggetto")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridButtonColumn HeaderStyle-Width="20px" ItemStyle-Width="20px" ButtonType="ImageButton"
                                                                                CommandName="Select" FilterControlAltText="Filter Select column" ImageUrl="~/images/Checks.png"
                                                                                UniqueName="Select" />
                                                                        </Columns>
                                                                    </MasterTableView></telerik:RadGrid>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                                border-top: 1px solid  #9ABBE8; height: 25px">
                                                <telerik:RadButton ID="DettaglioImageButton" runat="server" Text="Dettagli" Width="100px"
                                                    Skin="Office2007" ToolTip="Visualizza registrazione selezionata">
                                                    <Icon PrimaryIconUrl="../../../../images/text.png" PrimaryIconLeft="5px" />
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="DettaglioPanel" runat="server">
                        <table width="900px" cellpadding="2" cellspacing="2" border="0">
                            <tr>
                                <td>
                                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                                border-top: 1px solid  #9ABBE8; height: 25px">
                                                &nbsp;<asp:Label ID="TitoloDettaglioAttoAmministrativo" runat="server" Style="color: #00156E"
                                                    Font-Bold="True" CssClass="Etichetta" Text="Dettaglio Atto Amministrativo" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ContainerMargin">
                                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                    <tr>
                                                        <td>

                                                            <div id="mainAreaPanel" runat="server"   style="overflow: auto; background-color: #FFFFFF;
                                                                border: 0px solid #5D8CC9;">


                                                                <table style="width: 100%; border: 0 solid #5D8CC9" cellpadding="0" cellspacing="0"
                                                                    border="0">
                                                                    <tr>
                                                                        <td style="border-bottom: 0px solid #DFE8F6">
                                                                            <table style="width: 100%; background-color: #BFDBFF" cellpadding="0" cellspacing="0"
                                                                                border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <%--    &nbsp; &nbsp;<asp:Label ID="AreaInfoLabel2" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                        color: #00156E; background-color: #BFDBFF" Text="" />--%>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <%-- INIZIO  NAVIGAZIONE--%>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="PrimoImageButton" runat="server" ImageUrl="~/images//first.png"
                                                                                                        ToolTip="Sposta in prima posizione" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="PrecedenteImageButton" runat="server" ImageUrl="~/images//Previous.png"
                                                                                                        ToolTip="Sposta indietro" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Image ID="NavigatorSeparator1" runat="server" ImageUrl="~/images//NavigatorSeparator.png" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <telerik:RadTextBox ID="PositionItemTextBox" runat="server" Skin="Office2007" Width="50px"
                                                                                                        ToolTip="Posizione corrente" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="CountItemLabel" runat="server" CssClass="Etichetta" Text="di {0}"
                                                                                                        Width="50px" ToolTip="Numero totale di elementi" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="VaiImageButton" runat="server" ImageUrl="~/images//Goto.png"
                                                                                                        ToolTip="Vai a" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Image ID="NavigatorSeparator2" runat="server" ImageUrl="~/images//NavigatorSeparator.png" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="SuccessivoImageButton" runat="server" ImageUrl="~/images//Next.png"
                                                                                                        ToolTip="Sposta avanti" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="UltimoImageButton" runat="server" ImageUrl="~/images//Last.png"
                                                                                                        ToolTip="Sposta in ultima posizione" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <%-- FINE  NAVIGAZIONE--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table id="TabellaNotifica" style="width: 100%; background-color: #BFDBFF" cellpadding="0"
                                                                                cellspacing="0" border="0">
                                                                                <tr style="height: 24px">
                                                                                    <td>
                                                                                        <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    &nbsp;&nbsp;<asp:Label ID="AreaInfoLabel" runat="server" Font-Bold="True" Style="width: 550px;
                                                                                                        color: #00156E; background-color: #BFDBFF" Text="" CssClass="Etichetta" />
                                                                                                </td>
                                                                                                <td align="right">
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
                                                                                                </td>
                                                                                                <td align="center" style="width: 40px">
                                                                                                    <img id="InfoUtenteImageButton" runat="server" src="~/images/userInfo.png" style="cursor: pointer;
                                                                                                        border: 0px" alt="Informazioni sull'utente" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="height: 24px">
                                                                                    <td>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 200px; border: 0px solid red; vertical-align: middle" align="left">
                                                                                                    &nbsp;&nbsp;<asp:Label ID="InfoDocumentoCollegatoLabel" runat="server" Font-Bold="True"
                                                                                                        Style="width: 200px; color: #00156E; background-color: #BFDBFF; font-family: Verdana;
                                                                                                        font-size: 10px" Text="" />
                                                                                                </td>
                                                                                                <td align="left" style="border: 0px solid green">
                                                                                                    <asp:ImageButton ID="VisualizzaDocumentoCollegatoImageButton" runat="server" ImageUrl="~/images//Documento16.gif"
                                                                                                        Style="border: 0px" ToolTip="Visualizza il documento collegato" ImageAlign="AbsMiddle"
                                                                                                        Visible="false" />
                                                                                                    &nbsp;
                                                                                                    <asp:ImageButton ID="VisualizzaCopiaDocumentoCollegatoImageButton" runat="server"
                                                                                                        ImageUrl="~/images//DocumentoCopia16.gif" Style="border: 0px" ToolTip="Visualizza la copia conforme del documento collegato"
                                                                                                        ImageAlign="AbsMiddle" Visible="false" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <telerik:RadTabStrip runat="server" ID="AttiTabStrip" SelectedIndex="0" MultiPageID="AttiMultiPage"
                                                                                Skin="Office2007" style=" width:100%" >
                                                                                <Tabs>
                                                                                    <telerik:RadTab Text="Generale" Selected="True" />
                                                                                    <telerik:RadTab Text="Presenze" />
                                                                                    <telerik:RadTab Text="Contabilità" />
                                                                                    <telerik:RadTab Text="Allegati" />
                                                                                    <telerik:RadTab Text="Classificazioni" />
                                                                                    <telerik:RadTab Text="Visibilità" />
                                                                                     <%--<telerik:RadTab Text="Trasparenza" />--%>
                                                                                     <telerik:RadTab Text="Fascicoli" />
                                                                                </Tabs>
                                                                            </telerik:RadTabStrip>
                                                                            <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                                                                            <telerik:RadMultiPage runat="server" ID="AttiMultiPage" SelectedIndex="0" Height="100%"
                                                                                style="width:auto" CssClass="multiPage" BorderColor="#3399FF">


                                                                                <telerik:RadPageView runat="server" ID="GeneralePageView" CssClass="corporatePageView"
                                                                                    Height="425px">

                                                                                         <div  id="GeneralePanel" runat="server" style="padding: 2px 2px 2px 2px;">

                                        <table style="width: 100%">
                                            <tr style="height: 35px">
                                                <td style="width: 160px">
                                                    <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipologia Documento *"
                                                        ForeColor="#FF8040" />
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="TipologieDocumentoComboBox" AutoPostBack="true" runat="server"
                                                        EmptyMessage="- Seleziona Tipologia -" Filter="StartsWith" ItemsPerRequest="10"
                                                        MaxHeight="400px" Skin="Office2007" Width="340px" />
                                                </td>
                                                <td style="width: 340px;">
                                                 <table style="width: 100%;" id="NumeroSettoreTable" runat="server">
                                                        <tr>
                                                            <td style="width: 120px">
                                                                <asp:Label ID="NumeroSettoreLabel" runat="server" CssClass="Etichetta" Text="N. Reg. Settore" />
                                                            </td>
                                                              <td style="border: 0px solid red">
                                                                <telerik:RadTextBox ID="NumeroSettoreTextBox" runat="server" Skin="Office2007" Width="70px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>

                                      


                                        <table style="width: 100%">
                                            <tr>
                                         <td style="width: 70px; border: 0px solid red">
                                                    <asp:Label ID="NumeroDeliberaLabel" runat="server" CssClass="Etichetta" Text="N. Atto"
                                                        ForeColor="#FF8040" />
                                                </td>
                                                <td style="width: 80px; border: 0px solid red">



                                                   

                                                     <telerik:RadNumericTextBox ID="NumeroAttoTextBox" runat="server" Skin="Office2007"
                                                             Width="70px" DataType="System.Int32" MaxLength="4" MaxValue="9999" MinValue="1"
                                                             ShowSpinButtons="True" ToolTip="Numero Contatatore Atto">
                                                             <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                         </telerik:RadNumericTextBox>




                                                </td>
                                                <td style="width: 70px">
                                                    <asp:Label ID="DataLabel" runat="server" CssClass="Etichetta" Text="Data *" ForeColor="#FF8040" />
                                                </td>
                                                <td style="width: 140px">
                                                    <telerik:RadDatePicker ID="DataTextBox" Skin="Office2007" Width="110px" runat="server"
                                                        MinDate="1753-01-01" />
                                                </td>
                                                <td style="width: 70px">
                                                    <asp:Label ID="ModelloLabel" runat="server" CssClass="Etichetta" Text="Modello *"
                                                        ForeColor="#FF8040" />
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="ModelliComboBox" AutoPostBack="true" runat="server" EmptyMessage="- Seleziona Modello -"
                                                        Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                                        Width="400px" />
                                                </td>
                                            </tr>
                                        </table>


                                        <table style="width: 100%; display: none">
                                            <tr>
                                                <td style="width: 100px">
                                                    <asp:Label ID="PubblicatoLabel" runat="server" CssClass="Etichetta" Text="Da Pubblicare" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="PubblicatoCheckBox" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 70px">
                                                    <asp:Label ID="UfficioLabel" runat="server" CssClass="Etichetta" Text="Ufficio *"
                                                        ForeColor="#FF8040" />
                                                </td>
                                                <td style="width: 380px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <telerik:RadTextBox ID="UfficioTextBox" runat="server" Skin="Office2007" Width="100%" />
                                                            </td>
                                                            <td align="center" style="width: 25px">
                                                                <asp:ImageButton ID="TrovaUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona ufficio..." ImageAlign="AbsMiddle" 
                                                                    style="height: 16px" />
                                                            </td>
                                                            <td style="width: 25px">
                                                                <asp:ImageButton ID="EliminaUfficioImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    ToolTip="Cancella ufficio" ImageAlign="AbsMiddle"  />
                                                                <asp:ImageButton ID="AggiornaUfficioImageButton"
                                                                        runat="server" Style="display: none" />
                                                                <asp:TextBox ID="IdUfficioTextBox" runat="server"
                                                                            Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center" style="width: 70px">
                                                    <asp:Label ID="SettoreLabel" runat="server" CssClass="Etichetta" Text="Settore" />
                                                </td>
                                                <td style="width: 380px">
                                                    <telerik:RadTextBox ID="SettoreTextBox" runat="server" Skin="Office2007" Width="100%" /><asp:TextBox
                                                        ID="IdSettoreTextBox" runat="server" Style="display: none" />
                                                </td>
                                            </tr>
                                        </table>

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 70px">
                                                                <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto *"
                                                                    ForeColor="#FF8040" />
                                                            </td>
                                                            <td>
                                                                <span id="Span1" class="RadInput RadInput_Office2007" style="white-space: nowrap;">
                                                                    <asp:TextBox ID="OggettoTextBox" runat="server" CssClass="riTextBox riEnabled" Width="330px"
                                                                        Rows="3" TextMode="MultiLine" />
                                                                </span>
                                                            </td>
                                                            <td style="width: 35px; text-align: center">
                                                                <asp:Image ID="DelimitaTestoImageButton" ImageAlign="AbsMiddle" Style="cursor: pointer;
                                                                    border: 0px" runat="server" ImageUrl="~/images//ok16.png" ToolTip="Delimita testo selezionato come omissis" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td style="width: 50%">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td align="center" style="width: 70px">
                                                                <asp:Label ID="NoteLabel" runat="server" CssClass="Etichetta" Text="Note" />
                                                            </td>
                                                            <td>
                                                              
                                                                <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="370px"
                                                                    Rows="3" TextMode="MultiLine" />
                                                              
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>


                                         <div style="height: 55px; width: 100%; border: 0px solid red">

                                          <asp:Panel ID="AffissionePanel" runat="server">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="DataAffissioneLabel" runat="server" CssClass="Etichetta" Text="Data di Affissione" />
                                                        </td>
                                                        <td style="width: 140px">
                                                            <telerik:RadDatePicker ID="DataAffissioneTextBox" Skin="Office2007" Width="110px"
                                                                runat="server" MinDate="1753-01-01" />
                                                        </td>
                                                        <td style="width: 50px">
                                                            <asp:Label ID="GiorniAffissioneLabel" runat="server" CssClass="Etichetta" Text="Giorni" />



                                                        </td>
                                                        <td style="width: 110px">
                                                         

                                                                

                                                        <telerik:RadNumericTextBox ID="GiorniAffissioneTextBox" runat="server" Skin="Office2007"
                                                             Width="90px" DataType="System.Int32" MaxLength="3" MaxValue="999" MinValue="1"
                                                             ShowSpinButtons="True">
                                                             <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                         </telerik:RadNumericTextBox>

                                                        </td>
                                                        <td style="width: 150px">
                                                            <asp:Label ID="NumeroRegistroPubblicazioneLabel" runat="server" CssClass="Etichetta"
                                                                Text="N° Reg. Pubblicazione" />
                                                        </td>
                                                        <td>
                                                           

                                                                   <telerik:RadNumericTextBox ID="NumeroRegistroPubblicazioneTextBox" 
                                                                       runat="server" Skin="Office2007"
                                                                                                Width="90px" DataType="System.Int32" 
                                                                       MaxLength="5" Enabled="False">
                                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>


                                            <asp:Panel ID="ProtocolloPanel" runat="server">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100px">
                                                            <asp:Label ID="NumeroProtocolloLabel" runat="server" CssClass="Etichetta" Text="Protocollo n." />
                                                        </td>
                                                        <td style="width: 90px">
                                                            <telerik:RadTextBox ID="NumeroProtocolloTextBox" runat="server" Skin="Office2007"
                                                                Width="90px" />
                                                        </td>
                                                        <td style="width: 10px">
                                                            <asp:Label ID="DataProtocolloLabel" runat="server" CssClass="Etichetta" Text="/" />
                                                        </td>
                                                        <td style="width: 95px">
                                                            <telerik:RadTextBox ID="DataProtocolloTextBox" runat="server" Skin="Office2007" Width="90px" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="TrovaProtocolloImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                ToolTip="Protocolla documento..." ImageAlign="AbsMiddle" />
                                                            <asp:ImageButton ID="EliminaProtocolloImageButton"
                                                                    runat="server" ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella protocollo"
                                                                    ImageAlign="AbsMiddle" />
                                                            <asp:ImageButton ID="AggiornaProtocolloImageButton" runat="server"
                                                                        Style="display: none" />&#160;&#160;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>

                                          
                                             <asp:Panel ID="BozzaPanel" runat="server" style=" vertical-align:bottom" >
                                                <div style="border-top:1px solid #9ABBE8;border-bottom:0px solid #9ABBE8;padding-top:4px;padding-bottom:4px;">
                                                 </div>
                                                <center>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 155px">
                                                                <asp:Label ID="BozzaLabel" runat="server" CssClass="Etichetta" Text="Carica corpo da bozza" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="BozzaTextBox" runat="server" Skin="Office2007" Width="450px" /><asp:ImageButton
                                                                    ID="TrovaBozzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona bozza..." ImageAlign="AbsMiddle" />
                                                                <asp:ImageButton ID="EliminaBozzaImageButton"
                                                                        runat="server" ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella bozza"
                                                                        ImageAlign="AbsMiddle" />
                                                                <asp:ImageButton ID="AggiornaBozzaImageButton" runat="server"
                                                                            Style="display: none" />
                                                                <asp:TextBox ID="IdBozzaTextBox" runat="server" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                            </asp:Panel>
                                         


                                        </div>

                                  
                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>
                                                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                                                border-top: 1px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 250px">
                                                                            &nbsp;<asp:Label ID="ElencoFirmeLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                CssClass="Etichetta" Text="Visti e Pareri" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ContainerMargin">
                                                             
                                                                <table class="Container" cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div id="scrollPanelFirme" style="overflow: auto; height:160px; width: 100%; background-color: #FFFFFF;
                                                                                border: 0px solid #5D8CC9;">
                                                                                <telerik:RadGrid ID="FirmeGridView" runat="server" ToolTip="Elenco firme associate al documento"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Width="99.8%" Culture="it-IT">
                                                                                    <MasterTableView DataKeyNames="Id">
                                                                                        <Columns>
                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="170px" ItemStyle-Width="170px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 170px; border: 0px solid red">
                                                                                                    <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="DefaultStruttura" UniqueName="DefaultStruttura"
                                                                                                HeaderText="Firmatario" DataField="DefaultStruttura" HeaderStyle-Width="170px"
                                                                                                ItemStyle-Width="170px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("DefaultStruttura")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 170px; border: 0px solid red">
                                                                                                      <%# Eval("DefaultStruttura")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="DataFirma" UniqueName="DataFirma" HeaderText="Data"
                                                                                                DataField="DataFirma" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("DataFirma","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                        overflow: hidden; text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                                   <%# Eval("DataFirma", "{0:dd/MM/yyyy}")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="DefaultQualifica" UniqueName="DefaultQualifica"
                                                                                                HeaderText="Qualifica" DataField="DefaultQualifica" HeaderStyle-Width="160px"
                                                                                                ItemStyle-Width="160px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("DefaultQualifica")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 160px; border: 0px solid red">
                                                                                                       <%# Eval("DefaultQualifica")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="DefaultEsito" UniqueName="DefaultEsito"
                                                                                                HeaderText="Parere" DataField="DefaultEsito" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("DefaultEsito")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 90px; border: 0px solid red">
                                                                                                      <%# Eval("DefaultEsito")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="DefaultStrutturaDelegato" UniqueName="DefaultStrutturaDelegato"
                                                                                                HeaderText="Delega" DataField="DefaultStrutturaDelegato" HeaderStyle-Width="20px"
                                                                                                ItemStyle-Width="20px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# If ( String.IsNullOrEmpty(Eval("DefaultStrutturaDelegato")),"NO","SI") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 20px;
                                                                                                        border: 0px solid red">
                                                                                                     <%# If(String.IsNullOrEmpty(Eval("DefaultStrutturaDelegato")), "NO", "SI")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" HeaderStyle-Width="20px"
                                                                                             ItemStyle-Width="20px" FilterControlAltText="Filter Preview column"
                                                                                                ImageUrl="~\images\edit16.png" UniqueName="Preview" />
                                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                                                Text="Modifica Firma..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                                                ImageUrl="~\images\edit16.png" UniqueName="Select" />
                                                                                        </Columns>
                                                                                    </MasterTableView></telerik:RadGrid></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                                                                </telerik:RadPageView>

                                                                                <telerik:RadPageView runat="server" ID="PresenzePageView" CssClass="corporatePageView"
                                                                                    Height="425px">

                                                                                       <div id="PresenzePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; background-color: #DFE8F6">
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <asp:Label ID="EsecutivitaLabel" runat="server" CssClass="Etichetta" Text="Esecutività" />
                                                            </td>
                                                            <td style="width: 70px">
                                                                <asp:CheckBox ID="EsecutivitaImmediataCheckBox" runat="server" Text="&nbsp;" />
                                                            </td>
                                                            <td style="width: 70px">
                                                                <asp:Label ID="GiorniEsecutivitaLabel" runat="server" CssClass="Etichetta" Text="Giorni" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadNumericTextBox ID="GiorniEsecutivitaTextBox" runat="server" Skin="Office2007"
                                                                    Width="90px" DataType="System.Int32" MaxLength="3">
                                                                    <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                </telerik:RadNumericTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="width: 100%; background-color: #DFE8F6">
                                                        <tr>
                                                            <td style="width: 140px">
                                                                <asp:Label ID="TipoApprovazioneLabel" runat="server" CssClass="Etichetta" Text="Tipo approvazione" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="TipiApprovazioneComboBox" AutoPostBack="false" runat="server"
                                                                    EmptyMessage="- Seleziona Tipo -" Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px"
                                                                    Skin="Office2007" Width="340px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                      <div  id="GrigliaPresenzePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                 
                                        <table  style="width: 100%; background-color: #BFDBFF;  border: 1px solid #5D8CC9">
                                          <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 500px">
                                                                <asp:Label ID="TitoloPresenzeLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 400px; color: #00156E; background-color: #BFDBFF" Text="Elenco Presenze" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 60px">
                                                                            <asp:Label ID="SedutaLabel" runat="server" CssClass="Etichetta" Style="color: #00156E;
                                                                                background-color: #BFDBFF" Text="Seduta" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="SedutaTextBox" runat="server" Skin="Office2007" Width="100%" /><asp:TextBox
                                                                                ID="IdSedutaTextBox" runat="server" Style="display: none" />
                                                                        </td>
                                                                        <td align="right" style="width: 30px">
                                                                            <asp:ImageButton ID="TrovaSedutaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona Seduta..." ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                                    ID="AggiornaSedutaImageButton" runat="server" Style="display: none" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div style="overflow: auto; height: 330px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="PresenzeGridView" runat="server" ToolTip="Elenco presenze associate al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            Width="99.8%" Culture="it-IT">
                                                            <MasterTableView DataKeyNames="IdStruttura">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="IdStruttura" DataType="System.Int32" FilterControlAltText="Filter IdStruttura column"
                                                                        HeaderText="IdStruttura" ReadOnly="True" SortExpression="IdStruttura" UniqueName="IdStruttura"
                                                                        Visible="False" />
                                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" AllowFiltering="False">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="PresenteCheckBox"  AutoPostBack="True"
                                                                                runat="server"></asp:CheckBox></ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                                                        <ItemStyle Width="20px" />
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Convocato" UniqueName="Convocato" HeaderText="Convocato"
                                                                        DataField="Convocato" HeaderStyle-Width="700px" ItemStyle-Width="700px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Convocato")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 700px; border: 0px solid red">
                                                                             <%# Eval("Convocato")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                                                               
                                                                                </telerik:RadPageView>

                                                                                <telerik:RadPageView runat="server" ID="ContabilitàPageView" CssClass="corporatePageView"
                                                                                    Height="425px">

                                                                                        <div  id="ContabilitaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                 

                                        <table runat="server" id="ImpegniSpesaTable" style="width: 100%; background-color: #BFDBFF;
                                            border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="TitoloImpegniSpesaLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 400px; color: #00156E; background-color: #BFDBFF" Text="Elenco Impegni di Spesa" />
                                                            </td>
                                                            <td align="right" style="width: 30px">
                                                                <asp:ImageButton ID="AggiungiImpegnoSpesaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Aggiungi impegno di spesa" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                        ID="AggiornaImpegnoSpesaImageButton" runat="server" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div class="CustomFooter" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="ImpegniSpesaGridView" runat="server" ToolTip="Elenco impegni di spesa associati al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            ShowFooter="true" Width="99.8%" Culture="it-IT">
                                                            <MasterTableView DataKeyNames="Id, Guid">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                    <telerik:GridTemplateColumn SortExpression="AnnoEsercizio" UniqueName="AnnoEsercizio"
                                                                        HeaderText="Anno" DataField="AnnoEsercizio" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("AnnoEsercizio")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                            <%# Eval("AnnoEsercizio")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Capitolo" UniqueName="Capitolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Capitolo" DataField="Capitolo" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Capitolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                            <%# Eval("Capitolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Articolo" UniqueName="Articolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Articolo" DataField="Articolo" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Articolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                            <%# Eval("Articolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Descrizione" DataField="Note" HeaderStyle-Width="320px" ItemStyle-Width="320px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Note")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                width: 320px; border: 0px solid red">
                                                                            <%# Eval("Note")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Importo" UniqueName="Importo" FooterStyle-HorizontalAlign="Right"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Importo" DataField="Importo"
                                                                        HeaderStyle-Width="110px" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Importo","{0:N2}")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 110px; border: 0px solid red">
                                                                             <%# Eval("Importo", "{0:N2}")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroImpegno" UniqueName="NumeroImpegno"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Impegno" DataField="NumeroImpegno"
                                                                        HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                            <%# Eval("NumeroImpegno")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroSubImpegno" UniqueName="NumeroSubImpegno"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Sub Impegno" DataField="NumeroSubImpegno"
                                                                        HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroSubImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                            <%# Eval("NumeroSubImpegno")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                                                 <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                                            UniqueName="Copy" ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy" Text="Copia Impegno di Spesa..." />

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                        Text="Modifica Impegno di Spesa..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                        ImageUrl="~\images\edit16.png" UniqueName="Select" />
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        Text="Elimina Impegno di Spesa" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                                        ImageUrl="~\images\Delete16.png" UniqueName="Delete" />
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>

                                        <table runat="server" id="LiquidazioniTable" style="width: 100%; background-color: #BFDBFF;
                                            border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="TitoloLiquidazioneLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 400px; color: #00156E; background-color: #BFDBFF" Text="Elenco Liquidazioni" />
                                                            </td>
                                                            <td align="right" style="width: 30px">
                                                                <asp:ImageButton ID="AggiungiLiquidazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Aggiungi liquidazione" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                        ID="AggiornaLiquidazioneImageButton" runat="server" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div class="CustomFooter" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="LiquidazioniGridView" runat="server" ToolTip="Elenco liquidazioni associate al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            ShowFooter="true" Width="99.8%" Culture="it-IT">
                                                            <MasterTableView DataKeyNames="Id, Guid">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                    <telerik:GridTemplateColumn SortExpression="Capitolo" UniqueName="Capitolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Cap." DataField="Capitolo" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Capitolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 30px; border: 0px solid red">
                                                                           <%# Eval("Capitolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Articolo" UniqueName="Articolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Art." DataField="Articolo" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Articolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 30px; border: 0px solid red">
                                                                            <%# Eval("Articolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="AnnoImpegno" UniqueName="AnnoImpegno"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Anno I." DataField="AnnoImpegno"
                                                                        HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("AnnoImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                             <%# Eval("AnnoImpegno")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroImpegno" UniqueName="NumeroImpegno"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="N. Imp." DataField="NumeroImpegno"
                                                                        HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                              <%# Eval("NumeroImpegno")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="AnnoEsercizio" UniqueName="AnnoEsercizio"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Anno L." DataField="AnnoEsercizio"
                                                                        HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("AnnoEsercizio")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: AnnoEsercizio; width: 40px; border: 0px solid red">
                                                                             <%# Eval("AnnoEsercizio")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Numero" UniqueName="Numero" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="N. Liq." DataField="Numero" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Numero")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                width: 40px; border: 0x solid red">
                                                                           <%# Eval("Numero")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Mandato" UniqueName="Mandato" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Mandato" DataField="Mandato" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Mandato")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 100px; border: 0px solid red">
                                                                           <%# Eval("Mandato")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="ImportoLiquidato" UniqueName="ImportoLiquidato"
                                                                        FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderText="Importo"
                                                                        DataField="ImportoLiquidato" HeaderStyle-Width="110px" ItemStyle-Width="110px"
                                                                        ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("ImportoLiquidato","{0:N2}")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 110px; border: 0px solid red">
                                                                             <%# Eval("ImportoLiquidato", "{0:N2}")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Nominativo" UniqueName="Nominativo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Beneficiario" DataField="Nominativo" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 200px; border: 0px solid red">
                                                                           <%# Eval("Nominativo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                      <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                                            UniqueName="Copy" ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy" Text="Copia Liquidazione..." />

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                        Text="Modifica Liquidazione..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                        ImageUrl="~\images\edit16.png" UniqueName="Select" />


                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        Text="Elimina Liquidazione" ItemStyle-Width="20px" HeaderStyle-Width="20px" ImageUrl="~\images\Delete16.png"
                                                                        UniqueName="Delete" />

                                                                     <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="10px"
                                                                        ItemStyle-Width="10px">
                                                                    </telerik:GridButtonColumn>

                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>

                                        <table runat="server" id="AccertamentiTable" style="width: 100%; background-color: #BFDBFF;
                                            border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="TitoloAccertamentoLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 400px; color: #00156E; background-color: #BFDBFF" Text="Elenco Accertamenti" />
                                                            </td>
                                                            <td align="right" style="width: 30px">
                                                                <asp:ImageButton ID="AggiungiAccertamentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Aggiungi accertamento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                        ID="AggiornaAccertamentoImageButton" runat="server" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div class="CustomFooter" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="AccertamentiGridView" runat="server" ToolTip="Elenco accertamenti associati al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            ShowFooter="true" Width="99.8%" Culture="it-IT">
                                                            <FooterStyle ForeColor="#00156E" />
                                                            <MasterTableView DataKeyNames="Id, Guid">
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
                                                                        HeaderText="Capitolo" DataField="Capitolo" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Capitolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                            <%# Eval("Capitolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Articolo" UniqueName="Articolo" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Articolo" DataField="Articolo" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Articolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 40px; border: 0px solid red">
                                                                             <%# Eval("Articolo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Descrizione" DataField="Note" HeaderStyle-Width="320px" ItemStyle-Width="320px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Note")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                width: 320px; border: 0px solid red">
                                                                           <%# Eval("Note")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="Importo" UniqueName="Importo" FooterStyle-HorizontalAlign="Right"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Importo" DataField="Importo"
                                                                        HeaderStyle-Width="110px" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Importo","{0:N2}")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 110px; border: 0px solid red">
                                                                              <%# Eval("Importo", "{0:N2}")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroAccertamento" UniqueName="NumeroAccertamento"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Accert." DataField="NumeroAccertamento"
                                                                        HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroAccertamento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                              <%# Eval("NumeroAccertamento")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroSubAccertamento" UniqueName="NumeroSubAccertamento"
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderText="Sub. Accert." DataField="NumeroSubAccertamento"
                                                                        HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroSubAccertamento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                            <%# Eval("NumeroSubAccertamento")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                                            UniqueName="Copy" ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy" Text="Copia Accertamento..." />

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                        Text="Modifica Accertamento..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                        ImageUrl="~\images\edit16.png" UniqueName="Select" />
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        Text="Elimina Accertamento" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                                        ImageUrl="~\images\Delete16.png" UniqueName="Delete" />
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                                                                
                                                                                </telerik:RadPageView>

                                                                                <telerik:RadPageView runat="server" ID="AllegatiPageView" CssClass="corporatePageView"
                                                                                    Height="425px">
                                                                                         <div  id="AllegatiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                      
                                         <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                         <tr>
                                         <td>

                                             <table style="width: 100%">
                                            <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="TipoAllegatoLabel" runat="server" CssClass="Etichetta" Text="Tipo" />
                                                </td>
                                                <td>
                                              <telerik:RadComboBox ID="TipologiaAllegatoComboBox" runat="server"
                                                            EmptyMessage="- Seleziona Tipologia -" Filter="StartsWith" ItemsPerRequest="10"
                                                            MaxHeight="400px" Skin="Office2007" Width="200px"  
                                                                 ToolTip="Tipologia Allegato" />
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

                                        <table style="width: 100%">
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
                                         </td>
                                         </tr>
                                         </table>
                                   
                                   </div>
                                     <div  id="GrigliaAllegatiPanel" runat="server" style="padding: 2px 2px 2px 2px;">

                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati" />
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
                                                            Width="99.8%" Culture="it-IT" AllowMultiRowSelection="true">
                                                            <MasterTableView DataKeyNames="Id, Nomefile, NomeFileFirmato">
                                                                <Columns>

                                                                 <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Pubblicazione allegati albo pretorio on-line"
                                                                            AllowFiltering="False" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="20px" ItemStyle-Width="20px">

                                                                            <HeaderTemplate>
                                                                                <div style="width: 20px; height: 20px">
                                                                                    <asp:CheckBox ID="SelectAllCheckBox"  AutoPostBack="True"
                                                                                        runat="server" ToolTip="Seleziona tutto"></asp:CheckBox>
                                                                                </div>
                                                                            </HeaderTemplate>

                                                                           <ItemTemplate>
                                                                                <asp:CheckBox ID="SelectCheckBox" AutoPostBack="True"
                                                                                    runat="server" ToolTip="Pubblicazione dell'allegato all'albo pretorio on-line"></asp:CheckBox>

                                                                                      
                                                                            </ItemTemplate>
                                                                           </telerik:GridTemplateColumn>

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

                                                                       <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="SignedPreview" CommandName="SignedPreview"
                                                                                                                        ImageUrl="~/images/signedDocument16.png" ItemStyle-Width="16px" HeaderStyle-Width="16px" />

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

                                                                                <telerik:RadPageView runat="server" ID="ClassificazioniPageView" CssClass="corporatePageView"
                                                                                    Height="425px">

                                                                                       <div id="ClassificazioniPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <div id="DettaglioClassificazionePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                            <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td style="width: 80px">
                                                        <asp:Label ID="AnnotazioniLabel" runat="server" CssClass="Etichetta" Text="Note" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="AnnotazioniTextBox" runat="server" Skin="Office2007" Width="790px"
                                                            Rows="3" TextMode="MultiLine" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="GrigliaClassificazioniPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr style="height: 20px; background-color: #BFDBFF">
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 170px">
                                                                    &nbsp;<asp:Label Font-Bold="True" ID="ClassificazioneLabel" runat="server" Style="color: #00156E;
                                                                        background-color: #BFDBFF; width: 130px" CssClass="Etichetta" Text="Classificazioni" />
                                                                </td>
                                                                <td style="width: 460px">
                                                                    <telerik:RadTextBox ID="ClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                        Width="450px" />
                                                                </td>
                                                                <td style="width: 30px; text-align: center">
                                                                    <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                        ToolTip="Seleziona titolario classificazione..." ImageAlign="AbsMiddle" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="AggiungiClassificazioneImageButton" runat="server" ImageUrl="~/images//Add16.png"
                                                                        ToolTip="Aggiungi titolario classificazione" ImageAlign="AbsMiddle" />
                                                                    <asp:ImageButton ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                        Style="display: none; width: 0px" />
                                                                    <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                        Width="0px" Style="display: none" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="Div3" runat="server" style="overflow: auto; height: 310px; border: 1px solid #5D8CC9;
                                                            background-color: White">
                                                            <telerik:RadGrid ID="ClassificazioniGridView" runat="server" ToolTip="Elenco classificazioni associate al documento"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="99.8%" Culture="it-IT">
                                                                <MasterTableView DataKeyNames="IdClassificazione">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="IdClassificazione" DataType="System.Int32" FilterControlAltText="Filter IdClassificazione column"
                                                                            HeaderText="IdClassificazione" ReadOnly="True" SortExpression="IdClassificazione"
                                                                            UniqueName="IdClassificazione" Visible="False" />
                                                                        <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                            HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="400px" ItemStyle-Width="400px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 400px; border: 0px solid red">
                                                                                    <%# Eval("Descrizione")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Note" UniqueName="Note" HeaderText="Note"
                                                                            DataField="Note" HeaderStyle-Width="400px" ItemStyle-Width="400px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Note")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 400px; border: 0px solid red">
                                                                                    <%# Eval("Note")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                            Text="Elimina Classificazione" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                                            ImageUrl="~\images\Delete16.png" UniqueName="Delete" />
                                                                    </Columns>
                                                                </MasterTableView></telerik:RadGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                                                                
                                                                                </telerik:RadPageView>

                                                                                <telerik:RadPageView runat="server" ID="VisibilitaPageView" CssClass="corporatePageView"
                                                                                    Height="425px">

                                                                                    <div id="VisibilitaPanel" runat="server" style="padding: 2px 2px 2px 2px; width: 100%">
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="VisibilitaLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Visibilità" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:ImageButton ID="TrovaUtenteVisibilitaImageButton" runat="server" 
                                                                    ImageUrl="~/images//user_add.png" ImageAlign="AbsMiddle" BorderStyle="None"
                                                                    ToolTip="Aggiungi Utente..." />
                                                                    &nbsp;<asp:ImageButton ID="TrovaGruppoVisibilitaImageButton"
                                                                        runat="server" ImageUrl="~/images//group_add.png" ToolTip="Aggiungi Gruppo..." ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                      &nbsp;<asp:ImageButton
                                                                            ID="AggiornaGruppoVisibilitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            Style="display: none" />
                                                                            <asp:ImageButton
                                                                            ID="AggiornaUtenteVisibilitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div style="overflow: auto; height: 320px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="VisibilitaGridView" runat="server" ToolTip="Elenco utenti o gruppi associati al protocollo"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            Width="99.8%" Culture="it-IT">
                                                            <MasterTableView DataKeyNames="IdEntita, TipoEntita">
                                                                <Columns>

                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                    

                                                                     <telerik:GridTemplateColumn SortExpression="TipoEntita" UniqueName="" HeaderText="Tipologia"
                                                                        DataField="TipoEntita" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# IIf(Eval("TipoEntita")=1, "GRUPPO", "UTENTE")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 70px; border:0px solid red">
                                                                                <%# IIf(Eval("TipoEntita") = 1, "GRUPPO", "UTENTE")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                  

                                                                    <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione" HeaderText="Descrizione"
                                                                        DataField="Oggetto" HeaderStyle-Width="720px" ItemStyle-Width="720px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 720px; border:0px solid red">
                                                                                <%# Eval("Descrizione")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                     
                                    



                                                                       <telerik:GridButtonColumn FilterControlAltText="Filter Delete column" ImageUrl="~/images/Delete16.png"
                                                         ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"     
                                                         UniqueName="Delete" ButtonType="ImageButton" CommandName="Delete" />
                                                          


                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>

                                
                                
                                
                                
                                
                                
                                
                                <%--<telerik:RadPageView runat="server" ID="TrasparenzaPageView" CssClass="corporatePageView" Height="425px">


                                    <div id="TrasparenzaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 70px">
                                                    &nbsp;<asp:Label ID="SezioneLabel" runat="server" CssClass="Etichetta" Text="Sezione"
                                                        ToolTip="Digitare parola chiave (INVIO)" />
                                                </td>
                                                <td style="width: 360px">
                                                    <telerik:RadTextBox ID="SezioneTrasparenzaTextBox" runat="server" Skin="Office2007"
                                                        Width="350px" ToolTip="Digitare parola chiave (INVIO)" />
                                                </td>
                                                <td style="width: 20px">
                                                    <asp:ImageButton ID="TrovaSezioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                        ToolTip="Seleziona sezione trasparenza..." ImageAlign="AbsMiddle" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="EliminaSezioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                        ToolTip="Cancella sezione trasparenza" ImageAlign="AbsMiddle" />
                                                    <asp:ImageButton ID="AggiornaSezioneImageButton" runat="server" Style="display: none" />
                                                    <asp:TextBox ID="IdSezioneTrasparenzaTextBox" runat="server" Style="display: none" />
                                                </td>
                                                <td style="text-align: center">
                                                    &nbsp;<asp:Label ID="Label4" runat="server" CssClass="Etichetta" Text="Tipologia" />
                                                </td>
                                                <td style="width: 330px">
                                                    <telerik:RadComboBox ID="comboboxSottoSezione" AutoPostBack="true" runat="server"
                                                        Enabled="false" EmptyMessage="- Seleziona -" Filter="StartsWith" ItemsPerRequest="10"
                                                        MaxHeight="220px" Skin="Office2007" Width="325px" />
                                                </td>
                                            </tr>
                                        </table>
                                    
                                     
                                       

                                       

                                     

                                     
                                        <asp:Panel Width="100%" runat="server" ID="PubblicazionePanel" Visible="true">

                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                <tr>
                                                    <td>
                                                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="width: 250px">
                                                                                &nbsp;<asp:Label ID="DatiPubblicazioneLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                    CssClass="Etichetta" Text="Dati Pubblicazione" />
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
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 50px; text-align: center">
                                                                                            <asp:Label ID="DataInizioPubblicazioneLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="Da *" />
                                                                                        </td>
                                                                                        <td style="width: 120px">
                                                                                            <telerik:RadDatePicker ID="DataInizioPubblicazioneTextBox" Skin="Office2007" Width="110px"
                                                                                                runat="server" MinDate="1753-01-01" ToolTip="Data inizio pubblicazione">
                                                                                                <Calendar>
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                            </telerik:RadDatePicker>
                                                                                        </td>
                                                                                        <td style="width: 40px; text-align: center">
                                                                                            <asp:Label ID="DataFinePubblicazioneLabel" runat="server" CssClass="Etichetta" 
                                                                                                Text="A *" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadDatePicker ID="DataFinePubblicazioneTextBox" Skin="Office2007" Width="110px"
                                                                                                runat="server" ToolTip="Data fine pubblicazione" MinDate="1753-01-01">
                                                                                                <Calendar>
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                            </telerik:RadDatePicker>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                	      
          	              
                     

                                        <asp:Panel Width="100%" runat="server" ID="AttiConcessionePanel" Visible="false">
                                      
                                            <div id="AttiConsessionePanel2" runat="server" style="padding: 2px 2px 2px 2px;">

                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #9ABBE8">
                                                    <tr style="height: 20px; background-color: #BFDBFF">
                                                        <td>
                                                            <table style="width: 100%; background-color: #BFDBFF">
                                                                <tr>
                                                                    <td style="width: 110px;">
                                                                        &nbsp;<asp:Label ID="TitoloBeneficiariLabel" runat="server" Font-Bold="True" CssClass="Etichetta"
                                                                            Style="color: #00156E; background-color: #BFDBFF" Text="Beneficiari" ToolTip="Beneficiari atto di concessione" />
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <telerik:RadComboBox ID="RubricaComboBox" runat="server" Width="730px" Height="150"
                                                                            EmptyMessage="Seleziona Beneficiario" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007"
                                                                            LoadingMessage="Caricamento in corso...">
                                                                            <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx" />
                                                                        </telerik:RadComboBox>
                                                                    </td>
                                                                    <td align="center" style="width: 30px">
                                                                        <asp:ImageButton ID="AggiungiBeneficiarioImageButton" runat="server" ImageUrl="~/images//ok16.png"
                                                                            ToolTip="Aggiungi beneficiario vantaggio economico." ImageAlign="AbsMiddle" BorderStyle="None"
                                                                            Style="height: 16px" />
                                                                        <asp:ImageButton ID="InserisciBeneficiarioImageButton" runat="server" Style="display: none" />
                                                                        <asp:ImageButton ID="ModificaBeneficiarioImageButton" runat="server" Style="display: none" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="Div1" runat="server" style="overflow: auto; height: 280px; border: 1px solid #5D8CC9;
                                                                width: 100%; background-color: White">
                                                                <telerik:RadGrid ID="BeneficiariGridView" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                    PageSize="5" Culture="it-IT">
                                                                    <MasterTableView DataKeyNames="Id, IdBeneficiario">
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="false" />
                                                                            <telerik:GridBoundColumn DataField="IdBeneficiario" DataType="System.Int32" FilterControlAltText="Filter IdBeneficiario column"
                                                                                HeaderText="IdBeneficiario" ReadOnly="True" SortExpression="IdBeneficiario" UniqueName="IdBeneficiario"
                                                                                Visible="false" />
                                                                            <telerik:GridTemplateColumn SortExpression="Beneficiario" UniqueName="Beneficiario"
                                                                                HeaderText="Beneficiario" DataField="Beneficiario" HeaderStyle-Width="240px"
                                                                                ItemStyle-Width="240px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("Beneficiario")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 240px; border: 0px solid red">
                                                                                        <%# Eval("Beneficiario")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="DatoFiscaleBeneficiario" UniqueName="DatoFiscaleBeneficiario"
                                                                                HeaderText="C.F./P.IVA" DataField="DatoFiscaleBeneficiario" HeaderStyle-Width="120px"
                                                                                ItemStyle-Width="120px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("DatoFiscaleBeneficiario")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 120px; border: 0px solid red">
                                                                                        <%# Eval("DatoFiscaleBeneficiario")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="TitoloNorma" UniqueName="TitoloNorma"
                                                                                HeaderText="Norma/Titolo" DataField="TitoloNorma" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("TitoloNorma")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 140px; border: 0px solid red">
                                                                                        <%# Eval("TitoloNorma")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="Modalita" UniqueName="Modalita" HeaderText="Modalità"
                                                                                DataField="Modalita" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("Modalita")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 140px; border: 0px solid red">
                                                                                        <%# Eval("Modalita")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn SortExpression="Importo" UniqueName="Importo" HeaderText="Importo"
                                                                                DataField="Importo" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("Importo","{0:N2}")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                        <%# Eval("Importo", "{0:N2}")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="20px"
                                                                                Text="Elimina l'atto di Concessione" ItemStyle-Width="20px">
                                                                            </telerik:GridButtonColumn>
                                                                            <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                                                                UniqueName="Copy" ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy"
                                                                                Text="Copia l'atto di concessione" />
                                                                            <telerik:GridButtonColumn FilterControlAltText="Filter Select column" ImageUrl="~/images/edit16.png"
                                                                                ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                                ItemStyle-VerticalAlign="Middle" UniqueName="Select" ButtonType="ImageButton"
                                                                                CommandName="Select" Text="Modifica l'atto di Concessione" />
                                                                            <telerik:GridButtonColumn FilterControlAltText="Filter Preview column" ImageUrl="~/images/knob-search16.png"
                                                                                Visible="false" ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                                ItemStyle-VerticalAlign="Middle" UniqueName="Preview" ButtonType="ImageButton"
                                                                                CommandName="Preview" Text="Visualizza l'atto di concessione" />
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </div>
                                        </asp:Panel>
                                
                                
                               
                                  <asp:Panel Width="100%" runat="server" ID="BandiGareContrattiPanel" Visible="false">
                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                <tr>
                                                    <td>

                                                 
                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                            <tr>
                                                                <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                    border-top: 0px solid  #9ABBE8; height: 25px">
                                                                    <table style="width: 100%" border="0" cellpadding="0" cellspacing="0" >
                                                                        <tr>
                                                                            <td>
                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 300px">
                                                                                            &nbsp;<asp:Label ID="DatiBandoGaraLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                                CssClass="Etichetta" Text="Dati Bando Gara/Contratto" />
                                                                                        </td>
                                                                                        <td style="width: 40px">
                                                                                            <asp:Label ID="FiltroBandoGaraLabel" runat="server" CssClass="Etichetta" Style="color: #00156E;
                                                                                                background-color: #BFDBFF" Text="Filtro" Font-Bold="true" ToolTip="Digitare parola chiave (INVIO)" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="FiltroBandoGaraTextBox" runat="server" Skin="Office2007"
                                                                                                Width="100%" ToolTip="Digitare parola chiave (INVIO)" />
                                                                                        </td>
                                                                                        <td style="text-align: right; width: 25px">
                                                                                            <asp:ImageButton ID="TrovaBandoGaraImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                ToolTip="Seleziona bando gara..." ImageAlign="AbsMiddle" />
                                                                                            <asp:ImageButton ID="AggiornaBandoGaraImageButton" runat="server" Style="display: none" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>



                                                        <telerik:RadTabStrip runat="server" ID="BandoGaraTabStrip" SelectedIndex="0" MultiPageID="BandoGaraMultiPage"
                                                            Skin="Office2007" Width="100%">
                                                            <Tabs>
                                                                <telerik:RadTab Text="Generale" Selected="True" />
                                                                <telerik:RadTab Text="Documenti" />
                                                            </Tabs>
                                                        </telerik:RadTabStrip>
                                               
                                                        <telerik:RadMultiPage runat="server" ID="BandoGaraMultiPage" SelectedIndex="0" Height="100%"
                                                            Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                                           
                                                           
                                                            <telerik:RadPageView runat="server" ID="GeneraleBandoGaraPageView" CssClass="corporatePageView"
                                                                Height="235px">
                                                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td class="ContainerMargin">
                                                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">

                                                                                <tr>
                                                                                    <td>
                                                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 100px">
                                                                                                    <asp:Label ID="ProponenteLabel" runat="server" CssClass="Etichetta" Text="Proponente *"
                                                                                                        ToolTip="Denominazione della Stazione Appaltante responsabile del procedimento di scelta del contraente" />
                                                                                                </td>
                                                                                                <td style="width: 505px">
                                                                                                    <telerik:RadTextBox ID="DenominazioneStrutturaProponenteTextBox" runat="server" Skin="Office2007"
                                                                                                        ToolTip="Denominazione della Stazione Appaltante responsabile del procedimento di scelta del contraente"
                                                                                                        Width="500px" MaxLength="250" />
                                                                                                </td>
                                                                                                <td style="width: 125px; text-align: center">
                                                                                                    <asp:Label ID="CodiceFiscalePropontenteLabel" runat="server" CssClass="Etichetta"
                                                                                                        Text="CF Proponente" ToolTip="Codice fiscale della Stazione Appaltante responsabile del procedimento di scelta del contraente (lunghezza 11 caratteri)!" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <telerik:RadTextBox ID="CodiceFiscaleProponenteTextBox" runat="server" Skin="Office2007"
                                                                                                        ToolTip="Codice fiscale della Stazione Appaltante responsabile del procedimento di scelta del contraente (lunghezza 11 caratteri)!"
                                                                                                        Width="120px" MaxLength="11" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr>
                                                                                    <td>
                                                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 100px">
                                                                                                    <asp:Label ID="OggettoBandoGaraLabel" runat="server" CssClass="Etichetta" Text="Oggetto *" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <telerik:RadTextBox ID="OggettoBandoGaraTextBox" runat="server" Skin="Office2007"
                                                                                                        Width="745px" Rows="2" TextMode="MultiLine" ToolTip="Oggetto dell'appalto" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table border="0" cellpadding="0" cellspacing="4" style="width: 100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 85px">
                                                                                                                <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="Partecipante" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadComboBox ID="PartecipanteComboBox" runat="server" Width="230px" Height="150"
                                                                                                                    EmptyMessage="Seleziona Partecipante" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                                                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007"
                                                                                                                    LoadingMessage="Caricamento in corso...">
                                                                                                                </telerik:RadComboBox>
                                                                                                            </td>
                                                                                                            <td style="width: 25px; text-align: center">
                                                                                                                <asp:ImageButton ID="TrovaPartecipanteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                    ToolTip="Seleziona Beneficiario..." ImageAlign="AbsMiddle" />
                                                                                                            </td>
                                                                                                            <td style="width: 25px; text-align: center">
                                                                                                                <asp:ImageButton ID="AggiornaPartecipanteGaraImageButton" ImageAlign="AbsMiddle"
                                                                                                                    runat="server" Style="display: none" />
                                                                                                                <asp:ImageButton ID="AggiungiPartecipanteImageButton" ImageAlign="AbsMiddle" runat="server"
                                                                                                                    ImageUrl="~/images/add16.png" />
                                                                                                            </td>
                                                                                                            <td style="width: 25px; text-align: center">
                                                                                                                <asp:ImageButton ID="TrovaRaggruppamentoImageButton" runat="server" ImageUrl="~/images//group_add.png"
                                                                                                                    ToolTip="Aggiungi Raggruppamento..." ImageAlign="AbsMiddle" BorderStyle="None"
                                                                                                                    Style="width: 18px; height: 18px" />
                                                                                                                <asp:ImageButton ID="AggiornaRaggruppamentoImageButton" runat="server" Style="display: none" />
                                                                                                            </td>
                                                                                                            <td style="width: 25px; text-align: right">
                                                                                                                <asp:ImageButton ID="EliminaPartecipanteImageButton" runat="server" ImageUrl="~/images/RecycleEmpty.png"
                                                                                                                    ToolTip="Cancella Partecipanti selezionati" ImageAlign="AbsMiddle" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                                <td>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="text-align: right">
                                                                                                                <asp:Label ID="ListaAggiudicatariLabel" runat="server" CssClass="Etichetta" Text="Lista Aggiudicatari" />
                                                                                                            </td>
                                                                                                            <td style="width: 25px; text-align: right">
                                                                                                                <asp:ImageButton ID="EliminaAggiudicatarioImageButton" runat="server" ImageUrl="~/images/RecycleEmpty.png"
                                                                                                                    ToolTip="Cancella Aggiudicatari selezionati" ImageAlign="AbsMiddle" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 48%">
                                                                                                    <telerik:RadListBox ID="PartecipantiListBox" runat="server" Skin="Office2007" Style="width: 100%;
                                                                                                        height: 80px" Height="80px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True">
                                                                                                    </telerik:RadListBox>
                                                                                                </td>
                                                                                                <td align="center">
                                                                                                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                                                                                        <tr>
                                                                                                            <td align="center" style="height: 26px">
                                                                                                                <asp:ImageButton ID="AggiungiTuttoImageButton" runat="server" ImageUrl="~/images/Forwardd24.png"
                                                                                                                    ToolTip="Aggiungi tutto" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:ImageButton ID="AggiungiImageButton" runat="server" ImageUrl="~/images/FrecciaDx24.png"
                                                                                                                    ToolTip="Aggiungi elementi selezionati" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                                <td style="width: 48%">
                                                                                                    <telerik:RadListBox ID="AggiudicatariListBox" runat="server" Skin="Office2007" Style="width: 100%;
                                                                                                        height: 80px" Height="80px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True">
                                                                                                    </telerik:RadListBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50px">
                                                                                                    <asp:Label ID="CigBandoGaraLabel" runat="server" CssClass="Etichetta" Text="CIG *" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 100px">
                                                                                                                <telerik:RadTextBox ID="CigBandoGaraTextBox" MaxLength="10" runat="server" Skin="Office2007"
                                                                                                                    Width="80px" />
                                                                                                            </td>
                                                                                                            <td style="width: 130px; text-align: center;">
                                                                                                                <asp:Label ID="ImportoAggiudicazioneLabel" runat="server" CssClass="Etichetta" Text="Importo Aggiud. *" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadNumericTextBox ID="ImportoAggiudicazioneTextBox" runat="server" Skin="Office2007"
                                                                                                                    Width="120px" MaxLength="10" />
                                                                                                            </td>
                                                                                                            <td style="width: 130px; text-align: center;">
                                                                                                                <asp:Label ID="ImportoLiquidatoLabel" runat="server" CssClass="Etichetta" Text="Importo Liquid. *" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadNumericTextBox ID="ImportoLiquidatoTextBox" runat="server" Skin="Office2007"
                                                                                                                    Width="120px" MaxLength="10" />
                                                                                                            </td>
                                                                                                            <td style="width: 100px; text-align: center;">
                                                                                                                <asp:Label ID="NumeroOfferentiLabel" runat="server" CssClass="Etichetta" Text="N. Offerenti *" />
                                                                                                            </td>
                                                                                                            <td style="width: 80px">
                                                                                                                <telerik:RadNumericTextBox ID="NumeroOfferentiTextBox" runat="server" Skin="Office2007"
                                                                                                                    Width="70px" DataType="System.Int32" MaxLength="4" MaxValue="9999" MinValue="1"
                                                                                                                    ShowSpinButtons="True" ToolTip="Numero offerenti">
                                                                                                                    <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                                </telerik:RadNumericTextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 100px">
                                                                                                    <asp:Label ID="DataLavoroLabel" runat="server" CssClass="Etichetta" Text="Durata Lavoro" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 40px; text-align: center;">
                                                                                                                <asp:Label ID="DataInizioLavoroLabel" runat="server" CssClass="Etichetta" Text="Da" />
                                                                                                            </td>
                                                                                                            <td style="width: 105px">
                                                                                                                <telerik:RadDatePicker ID="DataInizioLavoroTextBox" Skin="Office2007" Width="95px"
                                                                                                                    runat="server" MinDate="1753-01-01">
                                                                                                                    <Calendar>
                                                                                                                        <SpecialDays>
                                                                                                                            <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                                        </SpecialDays>
                                                                                                                    </Calendar>
                                                                                                                    <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                                </telerik:RadDatePicker>
                                                                                                            </td>
                                                                                                            <td style="width: 40px; text-align: center">
                                                                                                                <asp:Label ID="DataFineLavoroLabel" runat="server" CssClass="Etichetta" Text="A" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadDatePicker ID="DataFineLavoroTextBox" Skin="Office2007" Width="95px"
                                                                                                                    runat="server" MinDate="1753-01-01">
                                                                                                                    <Calendar>
                                                                                                                        <SpecialDays>
                                                                                                                            <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                                        </SpecialDays>
                                                                                                                    </Calendar>
                                                                                                                    <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                                </telerik:RadDatePicker>
                                                                                                            </td>
                                                                                                            <td style="width: 140px; text-align: center">
                                                                                                                <asp:Label ID="TipologiaSceltaLabel" runat="server" CssClass="Etichetta" Text="Procedura Scelta *" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadComboBox ID="TipologiaSceltaComboBox" AutoPostBack="true" runat="server"
                                                                                                                    EmptyMessage="- Seleziona Tipologia -" Filter="StartsWith" ItemsPerRequest="10"
                                                                                                                    MaxHeight="400px" Skin="Office2007" Width="330px" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </telerik:RadPageView>


                                                            <telerik:RadPageView runat="server" ID="AllegatiBandoGaraPageView" CssClass="corporatePageView"
                                                                Height="235px">

                                                                <div id="AllegatiBandoGaraPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                    <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                        <tr>
                                                                            <td>

                                                                                <table style="width: 100%">
                                                                                    <tr style="height: 30px">
                                                                                        <td style="width: 90px">
                                                                                            <asp:Label ID="DescrizioneDocumentoBandoGaraLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="Descrizione" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="DescrizioneDocumentoBandoGaraTextBox" runat="server" Skin="Office2007"
                                                                                                Width="250px" />
                                                                                        </td>
                                                                                        <td style="width: 90px">
                                                                                            <asp:Label ID="NomeFileDocumentoBandoGaraLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="Nome file" />
                                                                                        </td>
                                                                                        <td style=" vertical-align:bottom">
                                                                                            <telerik:RadAsyncUpload ID="AllegatoBandoGaraUpload" runat="server" MaxFileInputsCount="1"
                                                                                                Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                                <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                            </telerik:RadAsyncUpload>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>

                                                                                <table style="width: 100%">

                                                                                   

                                                                                    <tr>
                                                                                        <td style="width: 90px">
                                                                                            <asp:Label ID="TipoAllegatoBandoGaraLabel" runat="server" CssClass="Etichetta" Text="Tipo" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioBandoGaraRadioButton"
                                                                                                GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                            <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoBandoGaraRadioButton"
                                                                                                GroupName="TipoDocumento" runat="server" />
                                                                                        </td>
                                                                                  
                                                                                        <td style="width: 140px">
                                                                                            <asp:Label ID="OpzioniScannerBandoGaraLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="Opzioni scanner" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="FronteRetroBandoGaraLabel" runat="server" CssClass="Etichetta" Text="Fronte retro" />&nbsp;<asp:CheckBox
                                                                                                ID="FronteRetroBandoGaraCheckBox" runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label
                                                                                                    ID="VisualizzaUIBandoGaraLabel" runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                                        ID="VisualizzaUIBandoGaraCheckBox" runat="server" Text="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>

                                     <div  id="GrigliaAllegatiBandoGaraPanel" runat="server" style="padding: 2px 2px 2px 2px;">

                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="DocumentiBandoGaraLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:ImageButton ID="ScansionaBandoGaraImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                    ToolTip="Allega documento digitalizzato"  BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                        ID="AggiungiDocumentoBandoGaraImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                        ToolTip="Allega documento"  ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                            ID="ScanUploadBandoGaraButton" Style="display: none" runat="server" ImageUrl="~/images//RecycleEmpty.png" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="AllegatiBandoGaraGridView" runat="server" ToolTip="Elenco allegati associati al bando di gara"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                             Culture="it-IT" AllowMultiRowSelection="true">
                                                            <MasterTableView DataKeyNames="Id, Nomefile">
                                                                <Columns>

                                                                

                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                    </telerik:GridBoundColumn>



                                                                    <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                        DataField="NomeFile" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 300px; border: 0px solid red">
                                                                           <%# Eval("NomeFile")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                 

                                                                    <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione" HeaderText="Descrizione"
                                                                        DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 350px;border: 0px solid red">
                                                                             <%# Eval("Descrizione")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                      <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                        HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                        <ItemTemplate>
                                                                            <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;border: 0px solid red">
                                                                                  <%# IIf(Eval("IdTipologiaAllegato") = 1, "Primario", "Allegato")%></div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="16px"
                                                                        ItemStyle-Width="16px" />
                                                                  

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="16px"
                                                                        ItemStyle-Width="16px" />
                                                                 

                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>


                                                            </telerik:RadPageView>


                                                        </telerik:RadMultiPage>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                     

                                       <asp:Panel Width="100%" runat="server" ID="CompensoConsulenzaPanel" Visible="false">

                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                <tr>
                                                    <td>
                                                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;<asp:Label ID="DatiIncaricoConsulenzaLabel" runat="server" Style="color: #00156E"
                                                                                    Font-Bold="True" CssClass="Etichetta" Text="Dati Incarico Consulenza/Collaborazione" />
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
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style=" width:110px">
                                                                                            <asp:Label ID="Label10" runat="server" CssClass="Etichetta" Text="Titolare *"  />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadComboBox ID="BeneficiarioConsulenzaComboBox" runat="server" Width="700px"
                                                                                                Height="150" EmptyMessage="Seleziona Titolare" EnableAutomaticLoadOnDemand="True"
                                                                                                ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                                                                Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                                                                                <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx" />
                                                                                            </telerik:RadComboBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="AggiornaBeneficiarioConsulenzaImageButton" runat="server" Style="display: none" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="TrovaBeneficiarioConsulenzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                ToolTip="Seleziona Beneficiario..." ImageAlign="AbsMiddle" Style="height: 16px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                                <table style=" width:100%">
                                                                                    <tr>
                                                                                        <td style="width: 110px">
                                                                                            <asp:Label ID="Label5" runat="server" CssClass="Etichetta" Text="Denominazione *" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="DenominazioneConsulenzaTextBox" runat="server" Skin="Office2007"
                                                                                                Width="700px" Rows="2" TextMode="MultiLine" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 110px">
                                                                                            <asp:Label ID="RagioneIncaricoLabel" runat="server" CssClass="Etichetta" Text="Ragione *" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="RagioneIncaricoConsulenzaTextBox" runat="server" Rows="2"
                                                                                                Skin="Office2007" TextMode="MultiLine" ToolTip="Ragione dell'incarico" Width="700px"
                                                                                                MaxLength="1500" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                                <table style=" width:100%">
                                                                                    <tr>
                                                                                        <td style="width: 110px">
                                                                                            <asp:Label ID="CompensoConsulenzaLabel" runat="server" CssClass="Etichetta" Text="Compensi *" />
                                                                                        </td>
                                                                                          <td style="width: 340px">
                                                                                            <telerik:RadTextBox ID="CompensoConsulenzaTextBox" runat="server" MaxLength="1500"
                                                                                                Skin="Office2007" ToolTip="Importo previsto da corrispondere" Width="330px" />
                                                                                        </td>
                                                                                        <td style="width: 80px; text-align:center">
                                                                                            <asp:Label ID="VariabileCompensoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="Variabili" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="VariabileCompensoConsulenzaTextBox" runat="server" MaxLength="1500"
                                                                                                Skin="Office2007" ToolTip="Compensi variabili da corrispondere" Width="300px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                               <table style=" width:100%">
                                                                                    <tr>
                                                                                       <td style="width: 110px">
                                                                                            <asp:Label ID="Label7" runat="server" CssClass="Etichetta" Text="Cariche"  />
                                                                                        </td>
                                                                                         <td style="width: 340px">
                                                                                            <telerik:RadTextBox ID="altreCaricheTextBox" ToolTip="Altre Cariche" runat="server"
                                                                                                MaxLength="1500" Skin="Office2007" Width="330px" />
                                                                                        </td>
                                                                                        <td style="width: 80px; text-align:center">
                                                                                            <asp:Label ID="Label8" runat="server" CssClass="Etichetta" Text="Incarichi " />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ToolTip="Altri Incarichi" ID="altriIncarichiTextBox" runat="server"
                                                                                                MaxLength="1500" Skin="Office2007" Width="300px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                               <table style=" width:100%">
                                                                                    <tr>
                                                                                       <td style="width: 110px">
                                                                                            <asp:Label ID="Label9" runat="server" CssClass="Etichetta" Text="Altre Attività"
                                                                                                />
                                                                                        </td>
                                                                                        <td style="width: 340px">
                                                                                            <telerik:RadTextBox ID="altreAttivitaProfessionaliTextBox" runat="server" MaxLength="1500"
                                                                                                Skin="Office2007" Width="330px" />
                                                                                        </td>
                                                                                        <td style=" width:70px; text-align:center">
                                                                                            <asp:Label ID="DataIncaricoConsulenzaLabel" runat="server" CssClass="Etichetta" Text="Durata" />
                                                                                        </td>
                                                                                        <td style=" width:30px; text-align:center">
                                                                                            <asp:Label ID="DataInizioIncaricoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="Da *" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadDatePicker ID="DataInizioIncaricoConsulenzaTextBox" runat="server" MinDate="1753-01-01"
                                                                                                Skin="Office2007" ToolTip="Data inizio incarico consulenza" Width="110px">
                                                                                                <Calendar ID="Calendar1" runat="server">
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                            </telerik:RadDatePicker>
                                                                                        </td>
                                                                                        <td style="width: 30px; text-align: center;">
                                                                                            <asp:Label ID="DataFineIncaricoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="A *" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadDatePicker ID="DataFineIncaricoConsulenzaTextBox" runat="server" MinDate="1753-01-01"
                                                                                                Skin="Office2007" ToolTip="Data fine incarico consulenza" Width="110px">
                                                                                                <Calendar ID="Calendar2" runat="server">
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                            </telerik:RadDatePicker>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td style=" vertical-align:middle">
                                                                               <table style=" width:100%">
                                                                                    <tr>


                                                                                        <td style="width: 109px;">
                                                                                            <asp:Label ID="CurriculumLabel" runat="server" CssClass="Etichetta" Text="Curriculum"
                                                                                                 />
                                                                                        </td>

                                                                                        <td style="width: 300px; border:0px solid red;">
                                                                                            <div id="curriculumUpload1" runat="server" visible="true">
                                                                                                 <table style=" width:100%;" >
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <telerik:RadAsyncUpload ID="CurriculumUpload" runat="server" MaxFileInputsCount="1" 
                                                                                                            Width="200px"   OnClientFileSelected="CurriculumSelezionato" OnClientFileUploadRemoved="CurriculumRimosso"
                                                                                                                Skin="Office2007"  InputSize="40" EnableViewState="True">
                                                                                                                <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                            </telerik:RadAsyncUpload>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <div id="divAggCurriculum" style="display: none;">
                                                                                                                <asp:ImageButton ID="AggiungiCurriculumImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                                    ToolTip="Allega Curriculum" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                            <div id="curriculumUpload2" runat="server" style="width: 300px" visible="false">
                                                                                                <asp:LinkButton ID="CurriculumAllegatoLinkButton" ForeColor="Blue" CssClass="Etichetta"
                                                                                                    runat="server" />
                                                                                                <asp:ImageButton ID="RimuoviCurriculumImageButton" runat="server" ImageUrl="~/images//Delete16.png"
                                                                                                    ToolTip="Rimuovi Curriculum" ImageAlign="AbsMiddle" BorderStyle="None" Width="16px" />
                                                                                                <asp:Label ID="NomeFileCurriculumLabel" runat="server" Visible="false" />
                                                                                            </div>
                                                                                        </td>

                                                                                        <td style=" text-align:center">
                                                                                            <asp:Label ID="InconsistenzaLabel" runat="server" CssClass="Etichetta" Text="Insussistenza"
                                                                                                ToolTip="Dichiarazione Insussistenza"  />
                                                                                        </td>


                                                                                      <td style="width: 300px;">
                                                                                            <div id="InconsistenzaUpload1" runat="server"  visible="true">
                                                                                                   <table style=" width:100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <telerik:RadAsyncUpload ID="InconsistenzaUpload" runat="server" MaxFileInputsCount="1"  Width="200px" 
                                                                                                                OnClientFileSelected="InconsistenzaSelezionato" OnClientFileUploadRemoved="InconsistenzaRimosso"
                                                                                                                Skin="Office2007"  InputSize="40" EnableViewState="True">
                                                                                                                <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                            </telerik:RadAsyncUpload>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <div id="divAggInconsistenza" style="display: none;">
                                                                                                                <asp:ImageButton ID="AggiungiInconsistenzaImageButton" runat="server" ImageUrl="~/images/add16.png"
                                                                                                                    ToolTip="Allega Dichiarazione" ImageAlign="AbsMiddle" BorderStyle="None" /></div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                            <div id="InconsistenzaUpload2" runat="server" style="width: 300px" visible="false">
                                                                                                <asp:LinkButton ID="InconsistenzaAllegatoLinkButton" ForeColor="Blue" CssClass="Etichetta"
                                                                                                    runat="server" />
                                                                                                <asp:ImageButton ID="RimuoviInconsistenzaImageButton" runat="server" ImageUrl="~/images/Delete16.png"
                                                                                                    ToolTip="Rimuovi Dichiarazione" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                <asp:Label ID="NomeFileInsussistenzaLabel" runat="server" Visible="false" />
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
                                                    </td>
                                                </tr>
                                            </table>

                                        </asp:Panel>
                              
                                    
                                    
                                
                                         <asp:Panel Width="100%" runat="server" ID="IncaricoPanel" Visible="false">

                                             <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                 <tr>
                                                     <td>
                                                         <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                             <tr>
                                                                 <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                                                     border-top: 1px solid  #9ABBE8; height: 25px">
                                                                     <table style="width: 100%">
                                                                         <tr>
                                                                             <td>
                                                                                 &nbsp;<asp:Label ID="DatiIncaricoLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                     CssClass="Etichetta" Text="Dati Incarico Dipendente" />

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
                                                                                <table  border="0" cellpadding="0" cellspacing="0" style="width:100%">
                   
                                                                                    <tr>
                                                                                        <td >
                                                                                            <asp:Label ID="Label15" runat="server" CssClass="Etichetta" Text="Dipendente *" Width ="100px"  />
                                                                                        </td>
                                                                                        <td style=" width:99%">
                                                                                                <telerik:RadComboBox ID="BeneficiarioIncaricoComboBox" runat="server" Width="730px" Height="150" EmptyMessage="Seleziona Dipendente" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                                                                                    <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx"  />
                                                                                                </telerik:RadComboBox>

                                                                                                 &nbsp;
                                                                                                <asp:ImageButton ID="TrovaBeneficiarioIncaricoDipendenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                    ToolTip="Seleziona Beneficiario..." ImageAlign="AbsMiddle" style="height: 16px" />
                                                                                                <asp:ImageButton ID="AggiornaBeneficiarioIncaricoDipendenteImageButton"
                                                                                                    runat="server" Style="display: none" />
                                                                                                

                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                                <table  border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                                                    <tr>
                                                                                        <td >
                                                                                            <asp:Label Width ="100px" ID="Label16" runat="server" CssClass="Etichetta" Text="Oggetto *"  />
                                                                                        </td>
                                                                                        <td style="width:99%;">
                                                                                            <telerik:RadTextBox ID="oggettoIncaricoDipendenteTextBox" runat="server" Skin="Office2007"
                                                                                                Width="99%" Rows="3" TextMode="MultiLine" 
                                                                                                ToolTip="Oggetto" MaxLength="1500"/>
                                                                                        </td>
                                                                   
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                                <table  border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                                                    <tr>
                                                                                        <td >
                                                                                            <asp:Label Width ="100px" ID="Label14" runat="server" CssClass="Etichetta" Text="Ragione"  />
                                                                                        </td>
                                                                                        <td style="width:99%;">
                                                                                            <telerik:RadTextBox ID="ragioneIncaricoDipendenteTextBox" runat="server" Skin="Office2007"
                                                                                                Width="99%" Rows="3" TextMode="MultiLine" ToolTip="ragione" MaxLength="1500"/>
                                                                                        </td>
                                                                   
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                                                    <tr>
                                                                                        <td style="width: 100px" >
                                                                                            <asp:Label ID="Label20" Width ="90px" runat="server" CssClass="Etichetta" Text="Compenso *" />
                                                                                        </td>
                                                                                        <td>
                                                                                                <telerik:RadNumericTextBox  ID="compensoIncaricoDipendenteTextBox" runat="server" Skin="Office2007" Width="70px" MaxLength="10" />
                                                                                        </td>

                                                                                        <td style="width:100px">
                                                                                                    <asp:Label ID="Label23" runat="server" CssClass="Etichetta" Text="Durata Incarico" />
                                                                                                </td>
                                                                                                <td>
                                                                                                <table style="width: 100%">
                                                                                                <tr>
                                                                                                    <td style="width:40px">
                                                                                                    <asp:Label ID="Label24" runat="server" CssClass="Etichetta" 
                                                                                                            Text="Da *" />
                                                                                                </td>
                                                                                                    <td style="width:120px">
                                                                                                    <telerik:RadDatePicker ID="DataInizioIncaricoDipendenteTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                                                        MinDate="1753-01-01">
                                                                                                        <Calendar ID="Calendar4" runat = "server">
                                                                                                            <SpecialDays>
                                                                                                                <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                            </SpecialDays>
                                                                                                        </Calendar>
                                                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                    </telerik:RadDatePicker>
                                                                                                </td>

                                                                                                <td style="width:40px; text-align:center">
                                                                                                    <asp:Label ID="Label25" runat="server" CssClass="Etichetta" 
                                                                                                        Text="A *" />
                                                                                                </td>

                                                                                                    <td>
                                                                                                    <telerik:RadDatePicker ID="DataFineIncaricoDipendenteTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                                                        MinDate="1753-01-01">
                                                                                                        <Calendar ID="Calendar5" runat ="server">
                                                                                                            <SpecialDays>
                                                                                                                <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                            </SpecialDays>
                                                                                                        </Calendar>
                                                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                    </telerik:RadDatePicker>
                                                                                                </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>    
                                                                        </tr>    
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                 </td>
                                                             </tr>
                                                         </table>
                                                     </td>
                                                 </tr>
                                             </table>

                                         </asp:Panel>
                                  
                                
                                    
                                    </div>
                                    
                                 </telerik:RadPageView>--%>

                                 
                                 
                                 
                                 
                                 
                                 
                                 
                                 
                                 
                                 
                                 
                                 
                                 
                                 <telerik:RadPageView runat="server" ID="FascicoliPageView" CssClass="corporatePageView"
                                                                                    Height="425px">
                                                                                    <div id="FascicoliPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                                        <div id="GrigliaFascicoliPanel" runat="server" style="padding: 0px 0px 0px 0px;">
                                                                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                <tr>
                                                                                                    <td style="height: 20px">
                                                                                                        <table style="width: 100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="ElencoFascicoliLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                                                        CssClass="Etichetta" Text="Elenco Fascicoli" />
                                                                                                                </td>
                                                                                                                <td style="width: 40px">
                                                                                                                    <asp:Label ID="FaseDocumentoFascicoloLabel" runat="server" CssClass="Etichetta" Text="Fase" />
                                                                                                                </td>
                                                                                                                <td style="width: 120px">
                                                                                                                    <telerik:RadComboBox ID="FaseDocumentoFascicoloComboBox" runat="server" Skin="Office2007"
                                                                                                                        EmptyMessage="Seleziona Fase" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="200px"
                                                                                                                        Width="105px" />
                                                                                                                </td>
                                                                                                                <td style="width: 25px; text-align: center">
                                                                                                                    <asp:ImageButton ID="NuovoFascicoloImageButton" runat="server" ImageUrl="~/images/add16.png"
                                                                                                                        ToolTip="Nuovo Fascicolo ..." ImageAlign="AbsMiddle" />
                                                                                                                    <asp:ImageButton ID="InserisciFascicoloImageButton" runat="server" Style="display: none" />
                                                                                                                    <asp:ImageButton ID="ModificaFascicoloImageButton" runat="server" Style="display: none" />
                                                                                                                </td>
                                                                                                                <td style="width: 25px; text-align: center">
                                                                                                                    <asp:ImageButton ID="TrovaFascicoloImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                                                        ToolTip="Seleziona Fascicolo ..." ImageAlign="AbsMiddle" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="background-color: #FFFFFF">
                                                                                                    <td>
                                                                                                        <div id="scrollPanelFascicoli" style="overflow: auto; height: 320px; border: 1px solid #5D8CC9">
                                                                                                            <telerik:RadGrid ID="FascicoliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                                                                PageSize="5" Culture="it-IT">
                                                                                                                <MasterTableView DataKeyNames="Id">
                                                                                                                    <Columns>
                                                                                                                        <telerik:GridBoundColumn DataField="Id" Visible="False" />
                                                                                                                        <telerik:GridBoundColumn DataField="idDocumento" Visible="False" />
                                                                                                                        <%-- <telerik:GridTemplateColumn SortExpression="NumeroRegistro" UniqueName="NumeroRegistro"
                                                                            ItemStyle-HorizontalAlign="Right" HeaderText="N." DataField="NumeroRegistro"
                                                                            HeaderStyle-Width="25px" ItemStyle-Width="25px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroRegistro")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 25px;">
                                                                                    <%# Eval("NumeroRegistro")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>--%>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="IdentificativoFascicolo" UniqueName="IdentificativoFascicolo"
                                                                                                                            HeaderText="Cod. Fascicolo" DataField="CodiceFascicolo" HeaderStyle-Width="145px"
                                                                                                                            ItemStyle-Width="145px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("IdentificativoFascicolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 145px; border: 0px solid red">
                                                                                                                                    <%# Eval("IdentificativoFascicolo")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="Fase" UniqueName="Fase" HeaderText="Fase"
                                                                                                                            DataField="Fase" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center"
                                                                                                                            ItemStyle-Width="45px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='I: Iniziale; F:Finale' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 45px; border: 0px solid red">
                                                                                                                                    <%# Eval("Fase")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneProcedimento" UniqueName="DescrizioneProcedimento"
                                                                                                                            HeaderText="Tipo Procedimento" DataField="DescrizioneProcedimento" HeaderStyle-Width="180px"
                                                                                                                            ItemStyle-Width="180px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("DescrizioneProcedimento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 180px; border: 0px solid red">
                                                                                                                                    <%# Eval("DescrizioneProcedimento")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                                                                            DataField="Oggetto">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 190px; border: 0px solid red">
                                                                                                                                    <%# Eval("Oggetto")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="DataApertura" UniqueName="DataApertura"
                                                                                                                            HeaderText="Apertura" DataField="DataApertura" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("DataApertura","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                                                    overflow: hidden; text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                                                                    <%# Eval("DataApertura", "{0:dd/MM/yyyy}")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="DataChiusura" UniqueName="DataChiusura"
                                                                                                                            HeaderText="Chiusura" DataField="DataChiusura" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("DataChiusura","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                                                    overflow: hidden; text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                                                                                    <%# Eval("DataChiusura", "{0:dd/MM/yyyy}")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridButtonColumn FilterControlAltText="Filter Select column" ImageUrl="~/images/edit16.png"
                                                                                                                            ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                                                                            ItemStyle-VerticalAlign="Middle" UniqueName="Select" ButtonType="ImageButton"
                                                                                                                            CommandName="Select" Text="Modifica Fascicolo" />
                                                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                            ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="20px"
                                                                                                                            Text="Elimina Fascicolo" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                                                                            ItemStyle-VerticalAlign="Middle" />
                                                                                                                    </Columns>
                                                                                                                </MasterTableView>
                                                                                                            </telerik:RadGrid>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </div>
                                                                                </telerik:RadPageView>

                                                                            </telerik:RadMultiPage>
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
                                                <telerik:RadButton ID="IndietroRisultatiButton" runat="server" Text="Indietro" Width="100px"
                                                    Skin="Office2007" ToolTip="Torna ai risultati">
                                                    <Icon PrimaryIconUrl="../../../../images/back.png" PrimaryIconLeft="5px" />
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
