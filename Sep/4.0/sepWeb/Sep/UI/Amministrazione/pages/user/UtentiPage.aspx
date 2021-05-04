<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="UtentiPage.aspx.vb" Inherits="UtentiPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');

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
            count = 2;
            var message = $get('<%= infoOperazioneHidden.ClientId %>').value;

            if (message !== '') {

                //VISUALIZZO IL MESSAGGIO

                ShowMessageBox(message);

                var intervallo = setInterval(function () {
                    count = count - 1;
                    if (count <= 0) {
                        HideMessageBox();
                        EnableUI(true);
                        clearInterval(intervallo);

                    }
                }, 1000);



                $get('<%= infoOperazioneHidden.ClientId %>').value = '';

            } else { EnableUI(true); }
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
                style.border = 'solid #FF9B35 1px';
                style.filter = "-ms-filter: progid:DXImageTransform.Microsoft.gradient(GradientType=0,startColorstr='#FFDB9B', endColorstr='#FFCB61')";
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

        function ShowMessageBox(message) {


            this.document.body.appendChild(messageBox);
            this.document.body.appendChild(messageBoxPanel);

            with (messageBoxPanel) {
                style.display = '';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.width = '100%';
                style.height = '100%';
                style.zIndex = 10000;
                style.backgroundColor = '#09718F';
                style.filter = "alpha(opacity=20)";
                style.opacity = "0.2";
            }

            with (messageBox) {
                style.width = '305px';
                style.height = '40px';
                style.backgroundColor = '#BFDBFF';
                style.border = 'solid #4892FF 2px';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.zIndex = 10000;
                style.textAlign = 'center';
                style.verticalAlign = 'middle';
                innerHTML = message;
                style.color = '#00156E';
                style.backgroundImage = 'url(/sep/Images/success.png)';
                style.backgroundPosition = '5px center';
                style.backgroundRepeat = 'no-repeat';

                style.lineHeight = '40px';
            }


            xc = Math.round((document.body.clientWidth / 2) - (300 / 2));
            yc = Math.round((document.body.clientHeight / 2) - (40 / 2));


            messageBox.style.left = xc + "px";
            messageBox.style.top = yc + "px";
            messageBox.style.display = 'block';



        }




        function HideMessageBox() {
            try {
                messageBox.style.display = 'none';
                messageBoxPanel.style.display = 'none';
            }
            catch (e) { }
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
                <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>

                      <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator>

                       <telerik:RadFormDecorator ID="RadFormDecorator3" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID3" Skin="Web20"></telerik:RadFormDecorator>

                <table style="width: 900px; border: 1px solid #5D8CC9">
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                                            <Items>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                                    CommandName="Nuovo" Owner="RadToolBar" ToolTip="Inserisci un nuovo utente" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                    CommandName="Trova" Owner="RadToolBar" ToolTip="Ricerca utente" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar" ToolTip="Elimina i dati immessi" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar" ToolTip="Salva i dati immessi" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar" ToolTip="Elimina l'utente selezionato" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                    CommandName="Stampa" Owner="RadToolBar" ToolTip="Stampa l'elenco degli utenti" />
                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar" />
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" style="width: 100%; border: 1px solid #5D8CC9">
                                <tr>
                                    <td>
                                        <table id="TabellaNotifica" style="width: 100%; background-color: #BFDBFF; border-bottom: 1px solid #5D8CC9">
                                            <tr style="background-color: #BFDBFF; border-bottom: 1px solid #5D8CC9">
                                                <td>
                                                    <asp:Label ID="AreaInfoLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                                        color: #00156E" Text="" CssClass="Etichetta" />
                                                </td>
                                                <td align="center">
                                                    <img id="InfoUtenteImageButton" runat="server" src="~/images/userInfo.png" style="cursor: pointer;
                                                        border: 0px" alt="Informazioni sullo storico dell'utenza" width="16" height="16" />
                                                </td>
                                            </tr>
                                        </table>


                                      
                                       
                                     <%-- tabella inizio--%>

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%">
                                                 <table style="width: 100%">

                                                 <tr>
                                                 <td style="width: 150px">
                                                  <asp:Label ID="CognomeLabel" runat="server" CssClass="Etichetta" Text="Cognome *"
                                                        ForeColor="#FF8040" />
                                                 </td>
                                                 <td>
                                                 <telerik:RadTextBox ID="CognomeTextBox" runat="server" Skin="Office2007" Width="250px"
                                                        ToolTip="Cognome" TabIndex="1" MaxLength="50" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                  <asp:Label ID="CodiceFiscaleLabel" runat="server" CssClass="Etichetta" Text="Codice fiscale" />
                                                 </td>
                                                 <td>
                                                  <telerik:RadTextBox ID="CodiceFiscaleTextBox" runat="server" Skin="Office2007" Width="250px"
                                                        ToolTip="Codice fiscale" TabIndex="3" MaxLength="16" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                  <asp:Label ID="DataNascitaLabel" runat="server" CssClass="Etichetta" Text="Data nascita" />
                                                 </td>
                                                 <td>
                                                     <table style="width: 100%">
                                                         <tr>
                                                             <td style="width: 110px">
                                                                 <telerik:RadDatePicker ID="DataNascitaTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                     MinDate="1753-01-01" ToolTip="Data di nascita">
                                                                     <Calendar runat="server">
                                                                         <SpecialDays>
                                                                             <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                         </SpecialDays>
                                                                     </Calendar>
                                                                     <DatePopupButton ToolTip="Apri il calendario." />
                                                                 </telerik:RadDatePicker>
                                                             </td>
                                                             <td style="width: 50px; text-align: center">
                                                                 <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Sesso *" />
                                                             </td>
                                                             <td>
                                                                 <telerik:RadComboBox ID="SessoComboBox" runat="server" Skin="Office2007" Width="80px"
                                                                     EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" />
                                                             </td>
                                                         </tr>

                                                     </table>
                                                 
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                  <asp:Label ID="UsernameLabel" runat="server" CssClass="Etichetta" Text="Username *"
                                                        ForeColor="#FF8040" />
                                                 </td>
                                                 <td>
                                                 <telerik:RadTextBox ID="UsernameTextBox" runat="server" Skin="Office2007" Width="250px"
                                                        ToolTip="Username" TabIndex="7" MaxLength="20" />
                                                 </td>
                                                 </tr>

                                                     <tr>
                                                         <td style="width: 150px">
                                                             <table style="width: 100%; padding:0px; border-spacing: 0;border-collapse: collapse;" >
                                                                 <tr>
                                                                     <td style="width: 100px">
                                                                         <asp:Label ID="CellulareLabel" runat="server" CssClass="Etichetta" Text="Cellulare"/>

                                                                     </td>
                                                                     <td style="text-align:center">
                                                                         <asp:Label ID="PrefissoTelefonoLabel" runat="server" CssClass="Etichetta" Text="(+39)" />
                                                                     </td>
                                                                 </tr>
                                                             </table>
                                                         </td>

                                                         <td>
                                                             <telerik:RadTextBox ID="CellulareTextBox" runat="server" Skin="Office2007"
                                                                 Width="250px" MaxLength="25" />

                                                         </td>
                                                     </tr>

                                                 <tr>
                                                <td style="width: 150px">
                                                 <asp:Label ID="UltimoSettaggioLabel" runat="server" CssClass="Etichetta" Text="Ultimo settaggio" />
                                                 </td>
                                                 <td>
                                                  <telerik:RadTextBox ID="UltimoSettaggioTextBox" runat="server" Skin="Office2007"
                                                        Width="115px" ToolTip="Data ora ultimo settaggio password" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                <td style="width: 150px">
                                                 <asp:Label ID="StrutturaDefaultLabel" runat="server" CssClass="Etichetta" Text="Struttura Default" />
                                                 </td>
                                                 <td>
                                                     <table style="width: 100%">
                                                         <tr>
                                                             <td>
                                                                 <telerik:RadTextBox ID="StrutturaDefaultTextBox" runat="server" Skin="Office2007"
                                                                     Enabled="False" Width="235px" />
                                                             </td>
                                                             <td style="width: 20px; text-align:center">
                                                                 <asp:ImageButton ID="TrovaStrutturaDefaultImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                     ToolTip="Seleziona struttura di default ..." ImageAlign="AbsMiddle" />
                                                             </td>
                                                             <td style="width: 20px; text-align:center">
                                                                 <asp:ImageButton ID="EliminaStrutturaDefaultImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                     ToolTip="Cancella struttura di default" ImageAlign="AbsMiddle" />
                                                                 <asp:ImageButton ID="AggiornaStrutturaDefaultImageButton" runat="server" Style="display: none" />
                                                             </td>
                                                         </tr>
                                                     </table>
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                    <asp:Label ID="TecnologiaClientSideLabel" runat="server" CssClass="Etichetta" Text="Tecnologia Client" />
                                                 </td>
                                                 <td>
                                                 <telerik:RadComboBox ID="TecnologiaClientSideComboBox" runat="server" Skin="Office2007"
                                                        Width="250" EmptyMessage="- Seleziona Tecnologia -" ItemsPerRequest="10" Filter="StartsWith"
                                                        MaxHeight="400px" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                  <asp:Label ID="PasswordNonSettataLabel" runat="server" CssClass="Etichetta" Text="Password non settata" />
                                                 </td>
                                                 <td>
                                                     <table style="width: 100%">
                                                         <tr>
                                                             <td style="width: 50px">
                                                                 <telerik:RadButton ID="PasswordNonSettataCheckBox" runat="server" ButtonType="ToggleButton"
                                                                     Enabled="False" Skin="Office2007" ToggleType="CheckBox" Text="PN" />
                                                             </td>
                                                             <td style="width: 50px" align="right">
                                                                 <asp:Label ID="BloccatoLabel" runat="server" CssClass="Etichetta" Text="Bloccato" />
                                                             </td>
                                                             <td style="width: 40px">
                                                                 <asp:CheckBox ID="BloccatoCheckBox" runat="server" ToolTip="Bloccato" Text="&nbsp;" />
                                                             </td>
                                                             <td style="width: 80px" align="right">
                                                                 <asp:Label ID="SuperUserLabel" runat="server" CssClass="Etichetta" Text="Super user" />
                                                             </td>
                                                             <td>
                                                                 <asp:CheckBox ID="SuperUserCheckBox" runat="server" ToolTip="Super user" Text="&nbsp;" />
                                                             </td>
                                                         </tr>
                                                     </table>

                                                 </td>
                                                 </tr>

                                                 </table>

                                                </td>

                                                <td style="width: 50%">
                                                 <table style="width: 100%">

                                                 <tr>
                                                 <td style="width: 150px">
                                                  <asp:Label ID="NomeLabel" runat="server" CssClass="Etichetta" Text="Nome *" ForeColor="#FF8040" />
                                                 </td>
                                                 <td>
                                                  <telerik:RadTextBox ID="NomeTextBox" runat="server" Skin="Office2007" Width="250px"
                                                        ToolTip="Nome" TabIndex="2" MaxLength="50" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                  <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="Titolo" />
                                                 </td>
                                                 <td>
                                                   <telerik:RadTextBox ID="TitoloTextBox" runat="server" Skin="Office2007" Width="250px"
                                                        ToolTip="Titolo" TabIndex="4" MaxLength="50" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                <td style="width: 150px">
                                                 <asp:Label ID="EmailLabel" runat="server" CssClass="Etichetta" Text="E-mail" />
                                                 </td>
                                                 <td>
                                                   <telerik:RadTextBox ID="EmailTextBox" runat="server" Skin="Office2007" Width="250px"
                                                        ToolTip="E-mail" TabIndex="6" MaxLength="50" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                 <asp:Label ID="PasswordPrimoAccessoLabel" runat="server" CssClass="Etichetta" Text="Psw 1° accesso  *" />
                                                 </td>
                                                 <td>
                                                  <telerik:RadTextBox ID="PasswordPrimoAccessoTextBox" runat="server" Skin="Office2007"
                                                        Width="250px" ToolTip="Password di 1° accesso" TabIndex="8" MaxLength="20" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                  <asp:Label ID="ModuloDefaultLabel" runat="server" CssClass="Etichetta" Text="Modulo default" />
                                                 </td>
                                                 <td>
                                                    <telerik:RadComboBox ID="ModuloComboBox" runat="server" Skin="Office2007" Width="250"
                                                        EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="300px"
                                                        ToolTip="Modulo SEP di default" TabIndex="9" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                <td style="width: 150px">
                                                 <asp:Label ID="NumeroSerialeCertificatoLabel" runat="server" CssClass="Etichetta"
                                                        Text="Seriale certificato f.d." />
                                                 </td>
                                                 <td>
                                                   <telerik:RadTextBox ID="NumeroSerialeCertificatoTextBox" runat="server" Skin="Office2007"
                                                        Width="250px" ToolTip="Numero Seriale Certificato" TabIndex="10" MaxLength="50" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                   <asp:Label ID="ProviderFirmaLabel" runat="server" CssClass="Etichetta" Text="Provider Firma Digitale" />
                                                 </td>
                                                 <td>
                                                   <telerik:RadComboBox ID="ProviderFirmaComboBox" runat="server" Skin="Office2007"
                                                        Width="250" EmptyMessage="- Seleziona Provider -" ItemsPerRequest="10" Filter="StartsWith"
                                                        MaxHeight="400px" ToolTip="Provider Firma Digitale" TabIndex="11" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                 <td style="width: 150px">
                                                   <asp:Label ID="VersioneLabel" runat="server" CssClass="Etichetta" Text="Versione Java" />
                                                 </td>
                                                 <td>
                                                  <telerik:RadComboBox ID="VersioneJavaComboBox" runat="server" Skin="Office2007" Width="250"
                                                        EmptyMessage="- Seleziona Versione -" ItemsPerRequest="10" Filter="StartsWith"
                                                        MaxHeight="400px" />
                                                 </td>
                                                 </tr>

                                                 <tr>
                                                <td style="width: 150px">
                                                <asp:Label ID="UtenteWindowsLabel" runat="server" CssClass="Etichetta" 
                                                        Text="Guid Utente Windows"  />
                                                 </td>
                                                 <td>
                                                  <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <telerik:RadTextBox ID="UtenteWindowsTextBox" runat="server" Skin="Office2007"
                                                                    Enabled="False" Width="235px" />
                                                            </td>
                                                            <td style="width: 20px; text-align:center">
                                                                <asp:ImageButton ID="TrovaUtenteWindowsImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona utente windows ..." ImageAlign="AbsMiddle" />
                                                               
                                                               
                                                            </td>
                                                            <td style="width: 20px; text-align:center">
                                                             <asp:ImageButton ID="EliminaUtenteWindwsImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    ToolTip="Cancella utente windows" ImageAlign="AbsMiddle" />
                                                                <asp:ImageButton ID="AggiornaUtenteWindowsImageButton" runat="server" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                 </td>
                                                 </tr>

                                                 </table>
                                                </td>
                                            </tr>
                                        </table>

                                       <%-- tabella fine--%>

                                      
                                    </td>

                                   

                                </tr>

                               

                            </table>
                            <telerik:RadTabStrip runat="server" ID="DatiUtenteTabStrip" SelectedIndex="0" MultiPageID="DatiUtenteMultiPage"
                                Skin="Office2007" Width="100%">
                                <Tabs>
                                    <telerik:RadTab Text="Abilitazioni funzioni" Selected="True" ToolTip="Abilitazioni funzioni utente" />
                                    <telerik:RadTab Text="Profili" ToolTip="Profili associati all'utente" />
                                    <telerik:RadTab Text="Strutture abilitate" ToolTip="Strutture abilitate per modulo" />
                                    <telerik:RadTab Text="Abilitazioni moduli" ToolTip="Abilitazioni moduli" />
                                    <telerik:RadTab Text="Gruppi Visibilità" ToolTip="Gruppi Visibilità" />
                                    <telerik:RadTab Text="Note" ToolTip="Note" />
                                </Tabs>
                            </telerik:RadTabStrip>
                            <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                            <telerik:RadMultiPage runat="server" ID="DatiUtenteMultiPage" SelectedIndex="0" Height="100%"
                                Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                <telerik:RadPageView runat="server" ID="AbilitazioniFunzioniPageView" CssClass="corporatePageView"
                                    Height="175px">
                                    <div id="AbilitazioniFunzioniPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 600px;">
                                            <tr>
                                                <td>
                                                    <div id="scrollPanelAbilitazioniFunzioni" runat="server" style="overflow: auto; height: 165px;
                                                        border: 1px solid #5D8CC9;">
                                                       
                                                        <telerik:RadGrid ID="AbilitazioniFunzioniGridView" runat="server" AutoGenerateColumns="False"
                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" ToolTip="Elenco abilitazioni funzioni associate all'utente"
                                                            AllowMultiRowSelection="True" TabIndex="12">
                                                            <MasterTableView DataKeyNames="Id">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="AbilitazioniFunzioniToggleSelectedState"
                                                                                AutoPostBack="True" runat="server"></asp:CheckBox>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="abilitataCheckBox" OnCheckedChanged="AbilitazioniFunzioniToggleRowSelection"
                                                                                AutoPostBack="true" runat="server"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                        HeaderText="Descrizione" UniqueName="Descrizione" />
                                                                    <telerik:GridBoundColumn DataField="Modulo" FilterControlAltText="Filter Modulo column"
                                                                        HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderText="Modulo" UniqueName="Modulo" />
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid>
                                                    </div>
                                                </td>
                                        </table>
                                    </div>
                                </telerik:RadPageView>
                                <telerik:RadPageView runat="server" ID="ProfiliPageView" CssClass="corporatePageView"
                                    Height="175px">
                                    <div id="ProfiliPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 600px">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="ProfiliLabel" runat="server" CssClass="Etichetta" Text="Profili" />
                                                            </td>
                                                            <td align="right" style="width: 20px">
                                                                <asp:ImageButton ID="AggiornaProfiloImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none" />
                                                                <asp:ImageButton ID="TrovaProfiloImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona profilo..." ImageAlign="AbsMiddle" />
                                                            </td>
                                                            <td align="right" style="width: 20px">
                                                                <asp:ImageButton ID="EliminaProfiloImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    Style="width: 16px" ToolTip="Cancella profili selezionati" ImageAlign="AbsMiddle" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadListBox ID="ProfiliListBox" runat="server" Skin="Office2007" Style="width: 600px;
                                                        height: 135px" Height="135px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True"
                                                        TabIndex="13">
                                                    </telerik:RadListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>
                                <telerik:RadPageView runat="server" ID="StruttureAbilitatePageView" CssClass="corporatePageView"
                                    Height="175px">

                                     <div id="StruttureAbilitatePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                          <tr>
                                                            <td>
                                                             

                                                                <asp:Label ID="StruttureAbilitateLabel" runat="server" Font-Bold="True" Style="width: 200px;
                                                                    color: #00156E; background-color: #BFDBFF" Text="Strutture Abilitate" CssClass="Etichetta" />
                                                            </td>
                                                            <td style="width: 110px">
                                                             <asp:Label ID="AggiornaGruppoLabel" runat="server" CssClass="Etichetta" Text="Aggiorna Gruppo" />
                                                            </td>
                                                            <td>
                                                                <div id="ZoneID2">
                                                                    <asp:CheckBox ID="AggiornaGruppoVisibilitaCheckBox" Checked="true" runat="server"
                                                                        Text="&nbsp;" />
                                                                </div>
                                                            </td>
                                                            <td style="width: 50px">
                                                                <asp:Label ID="ModuloLabel" runat="server" CssClass="Etichetta" Text="Modulo" />
                                                            </td>
                                                            <td style="width: 210px">
                                                                <telerik:RadComboBox ID="ModuloAbilitatoComboBox" runat="server" Skin="Office2007"
                                                                    Width="200" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                    MaxHeight="400px" />
                                                            </td>
                                                            <td align="right" style="width: 20px">
                                                                <asp:ImageButton ID="AggiornaStrutturaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none" />
                                                                <asp:ImageButton ID="TrovaStrutturaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona struttura nell'organigramma..." ImageAlign="AbsMiddle" />
                                                            </td>
                                                            <td align="right" style="width: 20px">
                                                                <asp:ImageButton ID="EliminaStrutturaImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    Style="width: 16px" ToolTip="Cancella strutture selezionate" ImageAlign="AbsMiddle" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="ZoneID3">
                                                   
                                           <div id="scrollPanelStruttureAbilitate" runat="server" style="overflow: auto; height: 135px;
                                                        border: 1px solid #5D8CC9; background-color: #FFFFFF">
                                                        <telerik:RadGrid ID="StruttureAbilitateGridView" runat="server" AutoGenerateColumns="False"
                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="700px" AllowSorting="True"
                                                            AllowMultiRowSelection="True" TabIndex="14">
                                                            <MasterTableView DataKeyNames="Id, IdModulo">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                                runat="server"></asp:CheckBox>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                                runat="server"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn DataField="Modulo" FilterControlAltText="Filter Modulo column" ItemStyle-Width="150px" HeaderStyle-Width="150px"
                                                                        HeaderText="Modulo" UniqueName="Modulo" />

                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="false" />

                                                                    <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                        HeaderText="Descrizione" SortExpression="Descrizione" UniqueName="Descrizione" />

                                                                    <telerik:GridButtonColumn FilterControlAltText="Filter Delete column" ImageUrl="~/images/Delete16.png"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" UniqueName="Delete" ButtonType="ImageButton"
                                                                        CommandName="Delete" Text="Elimina struttura" />
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid>
                                                    </div>

                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>


                                    <asp:HiddenField ID="scrollPosHiddenStruttureAbilitate" runat="server" Value="0" />
                                </telerik:RadPageView>
                                <telerik:RadPageView runat="server" ID="AbilitazioneModuliPageView" CssClass="corporatePageView"
                                    Height="175px">
                                    <div id="AbilitazioneModuliPaqnel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 600px">
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; height: 165px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="AbilitazioniModuloGridView" runat="server" AutoGenerateColumns="False"
                                                            TabIndex="15" CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%"
                                                            ToolTip="Elenco abilitazioni per modulo">
                                                            <MasterTableView DataKeyNames="Id">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn HeaderText="Abilita a tutto" HeaderStyle-Width="80px"
                                                                        ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="abilitaTuttoCheckBox" runat="server" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Responsabile" HeaderStyle-Width="80px" ItemStyle-Width="80px"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="responsabileCheckBox" runat="server" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                        HeaderText="Modulo" UniqueName="Descrizione" />
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>
                                <telerik:RadPageView runat="server" ID="GruppiVisibilitaPageView" CssClass="corporatePageView"
                                    Height="175px">
                                    <div id="GruppiVisibilitaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="TitoloElencoGruppiLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                    color: #00156E; background-color: #BFDBFF" Text="Elenco Gruppi" CssClass="Etichetta" />
                                                            </td>
                                                            <td style="width: 25px">
                                                                <asp:ImageButton ID="TrovaGruppoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona gruppo..." ImageAlign="AbsMiddle" />
                                                            </td>
                                                            <td style="width: 25px">
                                                                <asp:ImageButton ID="EliminaGruppiSelezionatiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    Style="width: 16px" ToolTip="Cancella gruppi selezionati" ImageAlign="AbsMiddle" />
                                                                <asp:ImageButton ID="AggiornaGruppoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="ZoneID1" style="overflow: auto; height: 135px; width: 100%; background-color: #FFFFFF;
                                                        border-top: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="GruppiGridView" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                            CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="True" AllowMultiRowSelection="true"
                                                            Culture="it-IT">
                                                            <MasterTableView DataKeyNames="Id">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutti i gruppi"
                                                                        HeaderStyle-Width="16px" ItemStyle-Width="16px" AllowFiltering="False">
                                                                        <HeaderTemplate>
                                                                            <div style="width: 16px; height: 16px" align="center">
                                                                                <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="GruppiToggleSelectedState"
                                                                                    AutoPostBack="True" runat="server" />
                                                                            </div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="GruppiToggleRowSelection" AutoPostBack="True"
                                                                                runat="server" ToolTip="Seleziona gruppo da eliminare" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                        HeaderText="Descrizione" SortExpression="Descrizione" UniqueName="Descrizione">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 530px; border: 0px solid red">
                                                                                <%# Eval("Descrizione")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="DataInizioValidita" UniqueName="DataInizioValidita"
                                                                        HeaderText="Valido dal" DataField="DataInizioValidita" HeaderStyle-Width="70px"
                                                                        ItemStyle-Width="70px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("DataInizioValidita","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                                <%# Eval("DataInizioValidita", "{0:dd/MM/yyyy}")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn SortExpression="DataFineValidita" UniqueName="DataFineValidita"
                                                                        HeaderText="Valido al" DataField="DataFineValidita" HeaderStyle-Width="70px"
                                                                        ItemStyle-Width="70px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("DataFineValidita","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                                <%# Eval("DataFineValidita", "{0:dd/MM/yyyy}")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="55px" ItemStyle-Width="55px" DataField="Abilitato"
                                                                        FilterControlAltText="Filter Abilitato column" HeaderText="Abilitato" SortExpression="Abilitato"
                                                                        UniqueName="Abilitato">
                                                                        <ItemTemplate>
                                                                            <div title='<%# IIf(CBool(Eval("Abilitato")), "SI", "NO")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 55px; border: 0px solid red">
                                                                                <%# IIf(CBool(Eval("Abilitato")), "SI", "NO")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridButtonColumn FilterControlAltText="Filter Delete column" ImageUrl="~/images/Delete16.png"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" UniqueName="Delete" ButtonType="ImageButton"
                                                                        CommandName="Delete" Text="Elimina gruppo" />
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>
                                <telerik:RadPageView runat="server" ID="NotePageView" CssClass="corporatePageView"
                                    Height="175px">
                                    <div id="NotePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 770px;">
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="770px"
                                                        ToolTip="Note" TabIndex="16" MaxLength="1500" TextMode="MultiLine" Rows="7" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="NomeFileCertificatoLabel" runat="server" CssClass="Etichetta" ToolTip="Nome del file del certificato collegato all'utente"
                                                        Font-Bold="true" Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="UtentiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="True" PageSize="10"
                                            Culture="it-IT" TabIndex="17">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" UniqueName="Id" Visible="False" />
                                                    <telerik:GridTemplateColumn DataField="Superuser" SortExpression="Superuser" UniqueName="Superuser"
                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px"
                                                        HeaderStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <img alt='<%# IIF(Eval("Superuser"),"SUPER USER", "Utente") %>' src='<%# IIF(Eval("Superuser"),ResolveClientUrl("~/images/Super_User.gif"), ResolveClientUrl("~/images/UserInfo16.png")) %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="Nominativo" ItemStyle-Width="250px" HeaderStyle-Width="250px"
                                                        FilterControlAltText="Filter Nominativo column" HeaderText="Nominativo" SortExpression="Nominativo"
                                                        UniqueName="Nominativo" />
                                                    <telerik:GridBoundColumn DataField="Username" FilterControlAltText="Filter Username column"
                                                        HeaderText="Username" UniqueName="Username" />
                                                    <telerik:GridBoundColumn DataField="CodiceFiscale" FilterControlAltText="Filter CodiceFiscale column"
                                                        HeaderText="Codice fiscale" UniqueName="CodiceFiscale" />
                                                    <telerik:GridTemplateColumn DataField="Bloccato" FilterControlAltText="Filter Bloccato column"
                                                        SortExpression="Bloccato" UniqueName="Bloccato" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <img alt='<%# IIF(Eval("Bloccato").ToLower = "no" ,"Utente sbloccato", "Utente bloccato") %>'
                                                                src='<%# IIF(Eval("Bloccato").ToLower = "no" ,ResolveClientUrl("~/images/Unlock_16.png"), ResolveClientUrl("~/images/Lock_16.png")) %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" ItemStyle-Width="20px"
                                                        HeaderStyle-Width="20px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center"
                                                        FilterControlAltText="Filter Select column" ImageUrl="~/images/Checks.png" UniqueName="Select" />
                                                    <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ItemStyle-Width="20px"
                                                        HeaderStyle-Width="20px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center"
                                                        ImageUrl="~/images/copy16.png" UniqueName="Copy" ButtonType="ImageButton" CommandName="Copy" />
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
             <asp:HiddenField ID="scrollPosHiddenAbilitazioniFunzioni" runat="server" Value="0" />
              <telerik:RadTextBox ID="CodiceStrutturaDefaultTextBox" runat="server" Style="display: none; width:0px"   />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
