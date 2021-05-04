<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="RegistrazioneArrivoPage.aspx.vb" Inherits="RegistrazioneArrivoPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UI/Protocollo/pages/user/VisualizzaEmailUserControl.ascx" TagName="VisualizzaEmailControl" TagPrefix="parsec" %>

<%--SPDX-License-Identifier: GPL-3.0-only--%>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">

<asp:PlaceHolder ID="bloccoCodice" runat="server">

    <script type="text/javascript">

     function OnClientSelectedIndexChanged(sender, args) {
         var itemText;
         itemText = args.get_item().get_text();
         if (itemText == 'PEC') {
             var combo = $find("<%= TipologiaDocumentoComboBox.ClientID %>");
             var item = combo.findItemByText('ELETTRONICO');
             if (item) {
                 item.select();
             }
         }
     }

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');
        var overlay = document.createElement("div");

        

        //var hide = true;
        var hideFullSizePanel = true;
        var showUI = true;
        var hideEmailPanel = true;

       

        Sys.Application.add_init(function () {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        });


        function pageLoad() {

            $get("pageContent").appendChild(_backgroundElement);
            $get("pageContent").appendChild(overlay);

            if (hideEmailPanel) {
                HideEmailPanel();
            } else {
                ShowEmailPanel();
            }

            if (hideFullSizePanel) {
                HideFullSizeGridPanel();
            } else {
                ShowFullSizeGridPanel();
            }

           
            //SE CI SONO APPLET 
            //ATTENZIONE C'E' SEMPRE UN APPLET REGISTRATO PER IL CONTROLLO UPLOAD DELLA TELERIK CHE UTILIZZA SILVERLIGHT
            if (document.applets.length > 1) {
                CheckAppletIsActive();

            }
         }



        function OnBeginRequest(sender, args) {
            EnableUI(false);
        }


        var count = 2;

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

                         <% If Not Page.Request("Mode") Is Nothing AndAlso Me.TipologiaProceduraApertura = TipoProcedura.Annullamento Then %>
                             $get('<%= chiudiImageButton.ClientId %>').click();
                        <% End If %>

                    }
                                }, 1000);

//                var intervallo = setInterval(NascondiMessaggio, 1000);
//                function NascondiMessaggio() {
//                   count = count - 1;
//                   if (count <= 0) {
//                      HideMessageBox();
//                      showUI = true;
//                      EnableUI(true);
//                      clearInterval(intervallo);

//                      <% If Not Page.Request("Mode") Is Nothing Then %>
//                         $get('<%= chiudiImageButton.ClientId %>').click();
//                     <% End If %>

//                   }
//                 
//                }

                $get('<%= infoOperazioneHidden.ClientId %>').value = '';

            } else { 

                 if (showUI){
                      EnableUI(true);
                 }
           
             }
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


        //***********************************************************************
        //INIZIO GESTIONE CONTROLLO CARICAMENTO APPLET
        //***********************************************************************
        var notificato = true;

        function CheckAppletIsActive() {


            var isActive = false;
            try {

                for (i = 0; i < document.applets.length; i++) {
                    isActive = document.applets[i].isActive();
                    var code = document.applets[i].code;
                    if (isActive) {
                        if (code == 'ParsecComunicator.class') { 
                            break;
                        }
                       
                     }  
                 }


                
             }
            catch (e) { }


          
            if (!isActive) {

                if (!notificato) {
                    alert("ATTENZIONE!!!\nAlcune funzioni sono state disabilitate.\nContattare l\'assistenza.");
                    notificato = true;
                }

                //DISABILITO I PULSANTI CHE UTILIZZANO L'APPLET
                DisableButtons();
            }
        }


        function DisableButtons() {

            DisableGridButtonColumns($find("<%=ProtocolliGridView.ClientID %>"), 'Stamp');
            DisableGridButtonColumns($find("<%=DocumentiGridView.ClientID %>"), 'Firma');

            DisableButton(document.getElementById('<%= ScansionaImageButton.ClientID %>'));
        }


        function DisableGridButtonColumns(grid, buttonUniqueName) {

            window.$ = $telerik.$;
            var tableView = grid.get_masterTableView();
            var dataRows = tableView.get_dataItems();

            var count = dataRows.length;
            for (var i = 0; i < count; i++) {
                var row = dataRows[i];
                var cell = tableView.getCellByColumnUniqueName(row, buttonUniqueName);
                $(cell).children('input')[0].disabled = true;
                $(cell).children('input')[0].title = 'Funzione disabilitata.\nContattare l\'assistenza.';

            }
        }


        function DisableButton(button) {
            if (button) {
                button.disabled = true;
                button.title = 'Funzione disabilitata.\nContattare l\'assistenza.';
            }
        }

        //***********************************************************************
        //FINE GESTIONE CONTROLLO CARICAMENTO APPLET
        //***********************************************************************


        function HideEmailPanel() {
            //panelIndex = -1;
            var panel = document.getElementById("EmailPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
        }

        function ShowEmailPanel() {

            //panelIndex = 0;

            var panel = document.getElementById("EmailPanel");

            panel.style.display = '';
            panel.style.position = 'absolute';

            overlay.style.display = '';
            overlay.style.position = 'absolute';
            overlay.style.left = '0px';
            overlay.style.top = '0px';
            overlay.style.width = '100%';
            overlay.style.height = '100%';
            overlay.style.zIndex = 10000;
            overlay.style.backgroundColor = '#09718F';
            overlay.style.filter = "alpha(opacity=20)";
            overlay.style.opacity = "0.2";


            var shadow = document.getElementById("ShadowEmailPanel");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }

        }



        function ShowFullSizeGridPanel() {
            var panel = document.getElementById("FullSizeGridPanel");
            panel.style.display = '';
            hideFullSizePanel = false;
         }

        function HideFullSizeGridPanel() {
            var panel = document.getElementById("FullSizeGridPanel");
            panel.style.display = 'none';
            hideFullSizePanel = true;
        }

    </script>


    </asp:PlaceHolder>

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

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="AggiungiReferenteEsternoRadWindow" runat="server" Modal="true"
                Animation="Fade" AnimationDuration="200" Behaviors="Close" Height="270" Skin="Office2007"
                Width="616" VisibleTitlebar="True" VisibleStatusbar="False" ReloadOnShow="true"
                Title="Inserisci referente esterno">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

   
       <asp:UpdatePanel ID="Pannello" runat="server">
           <ContentTemplate>

        <div ID="AjaxPanel" runat="server">


              <div id="pageContent">


                 <asp:Button runat="server" ID= "DisabilitaPulsantePredefinito" style=" width:0px; height:0px; left:-1000px; position:absolute" />

               <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>

                      <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator>

                   <telerik:RadFormDecorator ID="RadFormDecorator3" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID3" Skin="Web20"></telerik:RadFormDecorator>

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
                                                    CommandName="Trova" Owner="RadToolBar" Visible="False" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveAndExit.png" Text="Salva e Chiudi"
                                                    CommandName="SalvaChiudi" Owner="RadToolBar" Visible="False" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/send_email.png" Text="Invia E-mail"
                                                    CommandName="InviaEmail" Owner="RadToolBar" />                                                                                       
                                                   <telerik:RadToolBarDropDown runat="server" ImageUrl="~/images/Printer.png" Text="Stampa">
                                                    <Buttons>
                                                       <telerik:RadToolBarButton runat="server" CommandName="StampaRicevuta" Text="Ricevuta"
                                                            Width="240px" />

                                                        <telerik:RadToolBarButton runat="server" CommandName="StampaEtichetta" Text="Etichetta"
                                                            Width="240px">
                
                                                        </telerik:RadToolBarButton>

                                                             <telerik:RadToolBarButton runat="server" CommandName="StampaRegistroGenerale" Text="Registro Generale"
                                                            Width="240px" />

                                                               <telerik:RadToolBarButton runat="server" CommandName="StampaElencoRegistrazioni" Text="Elenco Registrazioni"
                                                            Width="240px" />
                                                    </Buttons>
                                                </telerik:RadToolBarDropDown>
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
                                        &nbsp;&nbsp;<asp:Label ID="AreaInfoLabel" runat="server" Font-Bold="True" Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="" CssClass="Etichetta" />
                                    </td>
                                    <td align="center" style="width: 40px">
                                        <asp:ImageButton ID="VisualizzaStoricoRegistrazioneImageButton" runat="server" ImageUrl="~/images//FolderHistory.png"
                                            Style="border: 0px" ToolTip="Visualizza storico registrazione selezionata" ImageAlign="Top"
                                            Visible="false" />
                                    </td>
                                    <td align="center" style="width: 40px">
                                        <img id="InfoUtenteImageButton" runat="server" src="~/images/userInfo.png" style="cursor: pointer;
                                            border: 0px" alt="Informazioni sull'utente" />
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadTabStrip runat="server" ID="DatiProtocolloTabStrip" SelectedIndex="0"
                                MultiPageID="DatiProtocolloMultiPage" Skin="Office2007" Width="100%">
                                <Tabs>
                                    <telerik:RadTab Text="Generale" Selected="True"/>
                                    <telerik:RadTab Text="Avanzate" />
                                    <telerik:RadTab Text="Documenti" />
                                    <telerik:RadTab Text="Collegamenti" />
                                    <telerik:RadTab Text="Fascicoli"/>
                                    <telerik:RadTab Text="Visibilità" />
                                </Tabs>
                            </telerik:RadTabStrip>
                            <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                            <telerik:RadMultiPage runat="server" ID="DatiProtocolloMultiPage" 
                                SelectedIndex="0" Height="100%"
                                 CssClass="multiPage" BorderColor="#3399FF">
                                <telerik:RadPageView runat="server" ID="GeneralePageView" CssClass="corporatePageView"
                                    Height="400px">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 150px">
                                                <asp:Label ID="TipologiaRegistrazioneLabel" runat="server" CssClass="Etichetta" Text="Tipologia registrazione" />
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="TipoRegistrazioneRadioList" runat="server" RepeatDirection="Horizontal"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Text="Arrivo" Value="0" Selected="True" />
                                                    <asp:ListItem Text="Partenza" Value="1" />
                                                    <asp:ListItem Text="Interna" Value="2" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="PannelloSessioneEmergenza" runat="server">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="SessioneLabel" runat="server" CssClass="Etichetta" Text="Sessione" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="SessioniEmergenzaComboBox" runat="server" Skin="Office2007"
                                                                    Width="220" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                    MaxHeight="300px" />
                                                                &#160;&#160;
                                                            </td>
                                                            <td style="width: 50px">
                                                                <asp:Label ID="NumeroEmergenzaLabel" runat="server" CssClass="Etichetta" Text="Numero" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="NumeroEmergenzaTextBox" runat="server" Skin="Office2007"
                                                                    Width="70px" MaxLength="7" />&nbsp;
                                                            </td>
                                                            <td style="width: 25px">
                                                                <asp:Label ID="DataImmissioneLabel" runat="server" CssClass="Etichetta" Text="Del" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadDatePicker ID="DataImmissioneTextBox" Skin="Office2007" Width="110px"
                                                                    runat="server" MinDate="1753-01-01" />
                                                                &#160;&#160;
                                                            </td>
                                                            <td style="width: 50px">
                                                                <asp:Label ID="UtenteLabel" runat="server" CssClass="Etichetta" Text="Utente" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="UtenteEmergenzaTextBox" runat="server" Skin="Office2007"
                                                                    Width="200px" Enabled="False" />&nbsp;&nbsp;
                                                                <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona utente (ALT + U) ..." />&nbsp;
                                                                <asp:ImageButton ID="EliminaUtenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    ToolTip="Cancella utente" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none" /><telerik:RadTextBox ID="IdUtenteEmergenzaTextBox" runat="server"
                                                                        Skin="Office2007" Style="display: none" Width="0px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="height: 25px">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 120px;">
                                                                        <asp:Label ID="ReferenteEsternoLabel" runat="server" CssClass="Etichetta" Text="Mittenti"
                                                                            AccessKey="R" AssociatedControlID="TrovaReferenteEsternoImageButton" /><asp:Label
                                                                                ID="Label2" runat="server" Style="display: none" CssClass="Etichetta" Text="Mittenti"
                                                                                AccessKey="1" AssociatedControlID="TrovaPrimoReferenteInternoImageButton" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <telerik:RadComboBox ID="RubricaComboBox" runat="server" Width="600" Height="150"
                                                                            EmptyMessage="Seleziona" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="Contains" Skin="Office2007"
                                                                            LoadingMessage="Caricamento in corso...">
                                                                            <WebServiceSettings Method="GetElementiRubrica" Path="RegistrazioneArrivoPage.aspx" />
                                                                        </telerik:RadComboBox>
                                                                        &nbsp;&nbsp;<asp:ImageButton ID="AggiungiReferenteEsternoImageButton" runat="server"
                                                                            ImageUrl="~/images//ok16.png" ToolTip="Aggiungi referente selezionato" ImageAlign="AbsMiddle" /><telerik:RadTextBox
                                                                                ID="FiltroDenominazioneTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                ToolTip="Digitare parola chiave (INVIO)" Visible="false" Style="display: none" />
                                                                    </td>
                                                                    <td align="right" style="width: 120px; vertical-align: top">
                                                                        <asp:ImageButton ID="TrovaReferenteEsternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            ToolTip="Seleziona referente esterno (ALT+R) ..." ImageAlign="AbsMiddle" />&nbsp;
                                                                        <asp:ImageButton ID="AggiungiNuovoReferenteEsternoImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                            ToolTip="Aggiungi nuovo referente esterno..." ImageAlign="AbsMiddle" />&nbsp;
                                                                        <asp:ImageButton ID="TrovaReferenteEsternoIpaImageButton" runat="server" ImageUrl="~/images//ipasearch.png"
                                                                            ToolTip="Seleziona referente esterno IPA..." ImageAlign="AbsMiddle" />&nbsp;
                                                                        <asp:ImageButton ID="TrovaPrimoReferenteInternoImageButton" runat="server" ImageUrl="~/images//uffici.png"
                                                                            ToolTip="Seleziona referente interno (ALT+1) ..." ImageAlign="AbsMiddle" />&nbsp;
                                                                        <asp:ImageButton ID="AggiornaNuovoReferenteEsternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            Style="display: none" /><asp:ImageButton ID="AggiornaReferenteEsternoImageButton"
                                                                                runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" /><asp:ImageButton
                                                                                    ID="AggiornaPrimoReferenteInternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                    Style="display: none" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="ZoneID3" style="overflow: auto; height: 120px; border: 1px solid #5D8CC9">
                                                                <telerik:RadGrid ID="ReferentiEsterniGridView" runat="server" ToolTip="Elenco referenti associati al protocollo"
                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                    Width="99.8%" Culture="it-IT">
                                                                    <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                        <Columns>

                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                            </telerik:GridBoundColumn>

                                                                            <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderText="P.C."
                                                                                HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="PerConoscenzaCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn UniqueName="CheckBoxIterTemplateColumn" HeaderText="Iter"
                                                                                HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="IterCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                             <telerik:GridTemplateColumn UniqueName="CheckBoxInviaEmailTemplateColumn" HeaderText="Email"
                                                                                HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="InviaEmailCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                HeaderText="Descrizione" DataField="Descrizione">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                        overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red;">
                                                                                       <%# Eval("Descrizione")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn SortExpression="Interno" UniqueName="Interno" HeaderText="Tipologia"
                                                                                DataField="Interno" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# IIf(CBool(Eval("Interno")), "Interno", "Esterno")%>' style="white-space: nowrap;
                                                                                        overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                       <%# IIf(CBool(Eval("Interno")), "Interno", "Esterno")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Modifica" FilterControlAltText="Filter Modifica column"
                                                                                ImageUrl="~\images\Edit16.png" UniqueName="Modifica" HeaderStyle-Width="30px" ItemStyle-Width="30px"
                                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                            </telerik:GridButtonColumn>

                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="30px" ItemStyle-Width="30px"
                                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                            </telerik:GridButtonColumn>

                                                                        </Columns>
                                                                    </MasterTableView></telerik:RadGrid></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="vertical-align: top; width: 70%">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="height: 25px">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 120px">
                                                                        <asp:Label ID="SecondoReferenteInternoLabel" runat="server" CssClass="Etichetta"
                                                                            Text="Destinatari" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <telerik:RadTextBox ID="FiltroSecondoReferenteInternoTextBox" runat="server" Skin="Office2007"
                                                                            Width="350px" ToolTip="Digitare parola chiave (INVIO)" />
                                                                    </td>
                                                                    <td align="right" style="width: 60px">
                                                                        <asp:ImageButton ID="AggiornaSecondoReferenteInternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            Style="display: none" /><asp:ImageButton ID="secondoReferenteInternoChechedButton"
                                                                                runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none; width: 0px" /><asp:ImageButton
                                                                                    ID="secondoReferenteInternoIterChechedButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                    Style="display: none; width: 0px" /><asp:ImageButton ID="secondoReferenteInternoInviaEmailChechedButton"
                                                                                        runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none; width: 0px" /><telerik:RadTextBox
                                                                                            ID="IdReferenteInternoTextBox" runat="server" Skin="Office2007" Style="display: none"
                                                                                            Width="0px" /><asp:ImageButton ID="TrovaSecondoReferenteInternoImageButton" runat="server"
                                                                                                ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona referente interno ..."
                                                                                                Style="width: 16px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="ZoneID1" style="overflow: auto; height: 125px; border: 1px solid #5D8CC9">
                                                                <telerik:RadGrid ID="SecondoReferentiInterniGridView" runat="server" ToolTip="Elenco referenti interni associati al protocollo"
                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                    Width="99.7%" Culture="it-IT">
                                                                    <MasterTableView DataKeyNames="Id">
                                                                        <Columns>

                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                <HeaderStyle Width="10px" />
                                                                                <ItemStyle Width="10px" />
                                                                            </telerik:GridBoundColumn>

                                                                            <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderText="P.C."
                                                                                HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="PerConoscenzaCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn UniqueName="CheckBoxIterTemplateColumn" HeaderText="Iter"
                                                                                HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="IterCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn UniqueName="CheckBoxInviaEmailTemplateColumn" HeaderText="Email"
                                                                                HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="InviaEmailCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                HeaderText="Descrizione" DataField="Descrizione">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                        overflow: hidden; text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                      <%# Eval("Descrizione")%></div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="20px"
                                                                                ItemStyle-Width="20px">
                                                                            </telerik:GridButtonColumn>
                                                                        </Columns>
                                                                    </MasterTableView></telerik:RadGrid></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="vertical-align: top; width: 30%;">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%">
                                                                <tr valign="top">
                                                                    <td style="width: 65px; vertical-align: middle">
                                                                        <asp:Label ID="OggettoLabel" runat="server" AccessKey="O" AssociatedControlID="TrovaOggettoImageButton"
                                                                            CssClass="Etichetta" Text="Oggetto" />
                                                                    </td>
                                                                    <td style="vertical-align: middle">
                                                                        <telerik:RadComboBox ID="OggettiComboBox" runat="server" Width="230px" Height="150px"
                                                                            EmptyMessage="Seleziona" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="Contains" Skin="Office2007"
                                                                            LoadingMessage="Caricamento in corso...">
                                                                            <WebServiceSettings Method="GetOggetti" Path="RegistrazioneArrivoPage.aspx" />
                                                                        </telerik:RadComboBox>
                                                                    </td>
                                                                    <td style="width: 20px; text-align: center; vertical-align: middle">
                                                                        <asp:ImageButton ID="AggiungiOggettoImageButton" runat="server" ImageUrl="~/images//ok16.png"
                                                                            ToolTip="Aggiungi oggetto selezionato" ImageAlign="AbsMiddle" />
                                                                    </td>
                                                                    <td style="width: 20px; text-align: right; vertical-align: middle">
                                                                        <asp:ImageButton ID="TrovaOggettoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            ToolTip="Seleziona oggetto (ALT+O) ..." ImageAlign="AbsMiddle" /><asp:ImageButton
                                                                                ID="AggiornaOggettoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span1" class="RadInput RadInput_Office2007" style="white-space: nowrap;">
                                                                <asp:TextBox ID="OggettoTextBox" runat="server" 
                                                                CssClass="riTextBox riEnabled" Width="350px"
                                                                    Rows="7" TextMode="MultiLine" ToolTip="Digitare parola chiave (ALT+O)" 
                                                                 /></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="TipologiaIterLabel" runat="server" CssClass="Etichetta" Text="Iter" />&nbsp;&nbsp;
                                                            <telerik:RadComboBox ID="TipologiaIterComboBox" runat="server" Skin="Office2007"
                                                                Width="300px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                MaxHeight="300px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </telerik:RadPageView>



                                <telerik:RadPageView runat="server" ID="AvanzatePageView" CssClass="corporatePageView"
                                    Height="400px">
                                    <div id="DatiDocumentoPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                &nbsp;<asp:Label ID="DatiDocumentoLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Dati Documento" />
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div style="overflow: auto; height: 70px; border: 1px solid #5D8CC9">
                                                        <table cellpadding="0" cellspacing="4" width="100%" border="0">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>

                                                                            <td style="width: 130px">
                                                                                <asp:Label ID="DataOraRicezioneInvioLabel" runat="server" CssClass="Etichetta" Text="Data/Ora ricezione" />
                                                                            </td>

                                                                            <td>


                                                                                <telerik:RadDatePicker ID="DataRicezioneInvioTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                                    MinDate="1753-01-01" ToolTip="Data ricezione/invio">
                                                                                    <Calendar runat="server">
                                                                                        <SpecialDays>
                                                                                            <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                        </SpecialDays>
                                                                                    </Calendar>
                                                                                    <DatePopupButton ToolTip="Apri il calendario." />
                                                                                </telerik:RadDatePicker>
                                                                            </td>
                                                                            <td style="width: 80px">

                                                                                <telerik:RadTimePicker ID="OrarioRicezioneInvioTextBox" Skin="Office2007" Width="70px"
                                                                                    runat="server" />
                                                                            </td>

                                                                           <td style="width:80px;text-align:center">
                                                                                <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipologia" />
                                                                            </td>
                                                                             <td style="width:190px">
                                                                                <telerik:RadComboBox ID="TipologiaDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                    Width="190" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                    MaxHeight="300px" />
                                                                            </td>
                                                                            <td style="width:110px;text-align:center">
                                                                                <asp:Label ID="TipoRicezioneInvioLabel" runat="server" CssClass="Etichetta" Text="Tipo ricezione" />
                                                                            </td>
                                                                             <td style="width:170px">
                                                                                <telerik:RadComboBox ID="TipoRicezioneInvioComboBox" runat="server" EmptyMessage="- Selezionare -"
                                                                                    Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                                    Width="170" NoWrap="True" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" />
                                                                            </td>
                                                                            <%--  INIZIO CONTENUTO--%></tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="width: 100px">
                                                                                            <asp:Label ID="ProtocolloMittenteLabel" runat="server" CssClass="Etichetta" Text="N. Protocollo" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="ProtocolloMittenteTextBox" runat="server" Skin="Office2007"
                                                                                                Width="100px" />
                                                                                        </td>
                                                                                        <td style="width: 70px; text-align: center">
                                                                                            <asp:Label ID="DataDocumentoLabel" runat="server" CssClass="Etichetta" Text="Data" />
                                                                                        </td>
                                                                                        <td style="width: 120px">
                                                                                           


                                                                                            <telerik:RadDatePicker ID="DataDocumentoTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                                                MinDate="1753-01-01" ToolTip="Data documento">
                                                                                                <Calendar>
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                            </telerik:RadDatePicker>



                                                                                        </td>
                                                                                        <td style="width: 130px; text-align: center">
                                                                                            <asp:Label ID="AnticipatoViaFaxLabel" runat="server" CssClass="Etichetta" Text="Anticipato via fax" />
                                                                                        </td>
                                                                                        <td style="width: 60px">
                                                                                            <asp:CheckBox ID="AnticipatoViaFaxCheckBox" runat="server" CssClass="Etichetta" Text="&nbsp;" />
                                                                                        </td>
                                                                                        <td style="width: 60px; text-align: center">
                                                                                            <asp:Label ID="StatoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Stato" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadComboBox ID="StatoDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                                Width="190" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                                MaxHeight="300px" />
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
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="ClassificazionePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                &nbsp;<asp:Label ID="ClassificazioneLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Classificazione" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="Label3" AccessKey="T" runat="server" Style="display: none" AssociatedControlID="TrovaClassificazioneImageButton" /><asp:Label
                                                                    ID="Label1" AccessKey="V" runat="server" Style="display: none" AssociatedControlID="FiltraClassificazioneImageButton" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <asp:Panel runat="server" ID="FiltroClassificazionePanel" Style="border: 1px solid #5D8CC9">
                                                        <table cellpadding="0" cellspacing="4" width="100%" border="0">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="width: 35px">
                                                                                <telerik:RadTextBox ID="FiltroCategoriaTextBox" runat="server" Skin="Office2007"
                                                                                    Width="30px" MaxLength="5" ToolTip="Imposta criterio di ricerca (Categoria)" />
                                                                            </td>
                                                                            <td style="width: 35px">
                                                                                <telerik:RadTextBox ID="FiltroClasseTextBox" runat="server" Skin="Office2007" Width="30px"
                                                                                    MaxLength="5" ToolTip="Imposta criterio di ricerca (Classe)" />
                                                                            </td>
                                                                            <td style="width: 35px">
                                                                                <telerik:RadTextBox ID="FiltroSottoClasseTextBox" runat="server" Skin="Office2007"
                                                                                    Width="30px" MaxLength="5" ToolTip="Imposta criterio di ricerca (Sotto-classe)" />
                                                                            </td>
                                                                            <td style="width: 45px">
                                                                                <asp:ImageButton ID="FiltraClassificazioneImageButton" runat="server" ImageUrl="~/images//refresh16.png"
                                                                                    ToolTip="Filtra classificazione (ALT+V) ..." ImageAlign="AbsMiddle" /><asp:ImageButton
                                                                                        ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                        Style="display: none" />
                                                                            </td>
                                                                            <td style="width: 310px">
                                                                                <telerik:RadTextBox ID="FiltroDescrizioneClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                    Width="300px" MaxLength="50" ToolTip="Imposta criterio di ricerca (Descrizione)" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                    ToolTip="Seleziona titolario di classificazione (ALT+T) ..." ImageAlign="AbsMiddle" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="width: 610px">
                                                                                <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                    Width="0px" Style="display: none" /><telerik:RadTextBox ID="ClassificazioneTextBox"
                                                                                        runat="server" Skin="Office2007" Width="600px" MaxLength="50" ToolTip="Classificazione"
                                                                                        Enabled="False" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="EliminaClassificazioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                    ToolTip="Cancella classificazione" ImageAlign="AbsMiddle" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="NotePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="NoteLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                Style="width: 120px; color: #00156E; background-color: #BFDBFF" Text="Note" />
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:ImageButton ID="EliminaNoteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella note" ImageAlign="AbsMiddle" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="background-color: #FFFFFF; border: 1px solid #5D8CC9">
                                                            <td style="padding: 2px 2px 2px 2px">
                                                                <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                    Rows="4" TextMode="MultiLine" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 50%">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="NoteInterneLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                Style="width: 120px; color: #00156E; background-color: #BFDBFF" Text="Note Interne" />
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:ImageButton ID="EliminaNoteInterneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella note interne" ImageAlign="AbsMiddle" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="background-color: #FFFFFF; border: 1px solid #5D8CC9">
                                                            <td style="padding: 2px 2px 2px 2px">
                                                                <telerik:RadTextBox ID="NoteInterneTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                    Rows="4" TextMode="MultiLine" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>


                                <telerik:RadPageView runat="server" ID="DocumentiPageView" CssClass="corporatePageView"
                                    Height="400px">
                                    <div id="AllegatiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%;">
                                                        <tr style="display: none">
                                                            <td style="width: 90px">
                                                                <asp:Label ID="NumeroDocumentiLabel" runat="server" CssClass="Etichetta" Text="N. documenti" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="NumeroDocumentiTextBox" runat="server" Skin="Office2007"
                                                                    Width="250px" />
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
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="NomeFileDocumentoLabel" runat="server" CssClass="Etichetta" Text="Nome file" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadAsyncUpload ID="AllegatoUpload" runat="server" MaxFileInputsCount="1"
                                                                    Skin="Office2007" Width="250px" InputSize="40">
                                                                    <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                </telerik:RadAsyncUpload>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="TipoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipo" />
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioRadioButton"
                                                                    GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoRadioButton"
                                                                    GroupName="TipoDocumento" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="width: 100%;">
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
                                    <div id="GrigliaAllegatiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
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
                                                        <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco documenti associati al protocollo"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            Width="99.8%" Culture="it-IT">
                                                            <MasterTableView DataKeyNames="Id, Nomefile,NomeFileFirmato">
                                                                <Columns>

                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                        <HeaderStyle Width="10px" />
                                                                        <ItemStyle Width="10px" />
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="N.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="NumeratoreLabel" runat="server" Width="10px" /></ItemTemplate>
                                                                        <HeaderStyle Width="10px" />
                                                                        <ItemStyle Width="10px" />
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                        DataField="NomeFile" HeaderStyle-Width="220px" ItemStyle-Width="220px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 220px; border: 0px solid red">
                                                                            <%# Eval("NomeFile")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="ImprontaEsadecimale" UniqueName="ImprontaEsadecimale"
                                                                        HeaderText="Impronta" DataField="ImprontaEsadecimale" HeaderStyle-Width="260px"
                                                                        ItemStyle-Width="260px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("ImprontaEsadecimale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 260px; border: 0px solid red">
                                                                            <%# Eval("ImprontaEsadecimale")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                        DataField="Oggetto" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Oggetto"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 140px;">
                                                                           <%# Eval("Oggetto")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="DescrizioneTipologiaDocumento" UniqueName="DescrizioneTipologiaDocumento"
                                                                        HeaderText="Tipo" DataField="DescrizioneTipologiaDocumento" HeaderStyle-Width="50px"
                                                                        ItemStyle-Width="50px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("DescrizioneTipologiaDocumento")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 50px;">
                                                                            <%# Eval("DescrizioneTipologiaDocumento")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                     <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="Firma" CommandName="Firma"
                                                                        ImageUrl="~/images/firmaDocumento16.png" ItemStyle-Width="16px" HeaderStyle-Width="16px" />

                                                                         <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="SignedPreview" CommandName="SignedPreview"
                                                                        ImageUrl="~/images/signedDocument16.png" ItemStyle-Width="16px" HeaderStyle-Width="16px" />

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" ItemStyle-Width="16px" HeaderStyle-Width="16px" />
                                                                     
                                                                   

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        ImageUrl="~\images\Delete16.png" UniqueName="Delete" ItemStyle-Width="16px" HeaderStyle-Width="16px" />
                                                                    

                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:HiddenField ID="infoScansioneHidden" runat="server" />
                                    <asp:HiddenField ID="documentContentHidden" runat="server" />
                                </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="CollegamentiPageView" CssClass="corporatePageView"
                                    Height="400px">
                                    <div id="CollegamentiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CollegamentiDirettiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Collegamenti diretti" /><telerik:RadTextBox
                                                                        ID="RiscontroNumeroProtocolloTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                        Style="display: none" /><telerik:RadTextBox ID="RiscontroDataImmissioneProtocolloTextBox"
                                                                            runat="server" Skin="Office2007" Width="0px" Style="display: none" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:ImageButton ID="TrovaCollegamentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona protocollo..." />&nbsp;
                                                                <asp:ImageButton ID="AggiornaCollegamentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div id="CollegamentiDirettiPanel" runat="server" style="overflow: auto; height: 145px;
                                                        border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="CollegamentiDirettiGridView" runat="server" ToolTip="Elenco collegamenti diretti associati al protocollo"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            Culture="it-IT">
                                                            <MasterTableView DataKeyNames="Id,NumeroProtocollo,AnnoProtocollo">
                                                                <Columns>

                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                   
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                                        HeaderText="N. prot." DataField="NumeroProtocollo" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 65px;">
                                                                            <%# Eval("NumeroProtocollo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn SortExpression="AnnoProtocollo" UniqueName="AnnoProtocollo"
                                                                        HeaderText="Anno" DataField="AnnoProtocollo" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("AnnoProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 30px;">
                                                                             <%# Eval("AnnoProtocollo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                        DataField="Oggetto" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Oggetto"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 200px;">
                                                                           <%# Eval("Oggetto")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="Uffici" UniqueName="Uffici" HeaderText="Uffici"
                                                                        DataField="Uffici" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Uffici"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 200px;">
                                                                              <%# Eval("Uffici")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="Referenti" UniqueName="Referenti" HeaderText="Referenti"
                                                                        DataField="Referenti" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Referenti"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 200px;">
                                                                              <%# Eval("Referenti")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Detail" FilterControlAltText="Filter Detail column"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\text_preview.png" UniqueName="Detail" />
                                                                  
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\Delete16.png" UniqueName="Delete" />

                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="GrigliaCollegamentiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CollegamentiIndirettiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Collegamenti indiretti" />
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div id="CollegamentiIndirettiPanel" runat="server" style="overflow: auto; height: 145px;
                                                        border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="CollegamentiIndirettiGridView" runat="server" ToolTip="Elenco collegamenti indiretti associati al protocollo"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            Culture="it-IT">
                                                            <MasterTableView DataKeyNames="Id,NumeroProtocollo,AnnoProtocollo">
                                                                <Columns>

                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                    
                                                                    <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                                        HeaderText="N. prot." DataField="NumeroProtocollo" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NumeroProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 65px;">
                                                                            <%# Eval("NumeroProtocollo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="AnnoProtocollo" UniqueName="AnnoProtocollo"
                                                                        HeaderText="Anno" DataField="AnnoProtocollo" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("AnnoProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 30px;">
                                                                           <%# Eval("AnnoProtocollo")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                        DataField="Oggetto" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Oggetto"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 300px;">
                                                                             <%# Eval("Oggetto")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="Uffici" UniqueName="Uffici" HeaderText="Uffici"
                                                                        DataField="Uffici" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Uffici"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 200px;">
                                                                            <%# Eval("Uffici")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="Referenti" UniqueName="Referenti" HeaderText="Referenti"
                                                                        DataField="Referenti" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Referenti"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 200px;">
                                                                          <%# Eval("Referenti")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Detail" FilterControlAltText="Filter Detail column"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\text_preview.png" UniqueName="Detail" />

                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>


                                <telerik:RadPageView runat="server" ID="FascicoliPageView" CssClass="corporatePageView"
                                    Height="400px">
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
                                                                        ToolTip="Nuovo Fascicolo ..." ImageAlign="AbsMiddle" /><asp:ImageButton ID="InserisciFascicoloImageButton"
                                                                            runat="server" Style="display: none" /><asp:ImageButton ID="ModificaFascicoloImageButton"
                                                                                runat="server" Style="display: none" />
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
                                                                        
                                                                        <telerik:GridTemplateColumn SortExpression="IdTipologiaFascicolo" UniqueName="" HeaderText="Tipo"
                                                                            DataField="IdTipologiaFascicolo" HeaderStyle-Width="45px" ItemStyle-Width="45px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# IIf(Eval("IdTipologiaFascicolo")=1, "P = Procedimento", "A = Affare")%>'
                                                                                    style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 45px;
                                                                                    border: 0px solid red">
                                                                              <%# IIf(Eval("IdTipologiaFascicolo") = 1, "P", "A")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>


                                                                        <telerik:GridTemplateColumn SortExpression="IdentificativoFascicolo" UniqueName="IdentificativoFascicolo"
                                                                            HeaderText="Cod. Fascicolo" DataField="CodiceFascicolo" HeaderStyle-Width="125px"
                                                                            ItemStyle-Width="125px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("IdentificativoFascicolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 125px; border: 0px solid red">
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
                                                                            HeaderText="Tipo Procedimento" DataField="DescrizioneProcedimento" HeaderStyle-Width="170px"
                                                                            ItemStyle-Width="170px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DescrizioneProcedimento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 170px; border: 0px solid red">
                                                                                 <%# Eval("DescrizioneProcedimento")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                            DataField="Oggetto">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 170px; border: 0px solid red">
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
                                                                </MasterTableView></telerik:RadGrid></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </telerik:RadPageView>




                                <telerik:RadPageView runat="server" ID="VisibilitaPageView" CssClass="corporatePageView"
                                    Height="400px">
                                    <div id="VisibilitaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
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
                                                                <asp:ImageButton ID="TrovaUtenteVisibilitaImageButton" runat="server" ImageUrl="~/images//user_add.png"
                                                                    ImageAlign="AbsMiddle" BorderStyle="None" ToolTip="Aggiungi Utente..." />&nbsp;<asp:ImageButton
                                                                        ID="TrovaGruppoVisibilitaImageButton" runat="server" ImageUrl="~/images//group_add.png"
                                                                        ToolTip="Aggiungi Gruppo..." ImageAlign="AbsMiddle" BorderStyle="None" />&nbsp;<asp:ImageButton
                                                                            ID="AggiornaGruppoVisibilitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            Style="display: none" /><asp:ImageButton ID="AggiornaUtenteVisibilitaImageButton"
                                                                                runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
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
                                                                            <div title='<%# IIf(Eval("TipoEntita")=1, "GRUPPO", "UTENTE")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                            <%# IIf(Eval("TipoEntita") = 1, "GRUPPO", "UTENTE")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                        HeaderText="Descrizione" DataField="Oggetto" HeaderStyle-Width="720px" ItemStyle-Width="720px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 720px; border: 0px solid red">
                                                                            <%# Eval("Descrizione")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridButtonColumn FilterControlAltText="Filter Delete column" ImageUrl="~/images/Delete16.png"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle" UniqueName="Delete" ButtonType="ImageButton"
                                                                        CommandName="Delete" />
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="ZoneID2">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 100px">
                                                    <asp:Label ID="RiservatoLabel" runat="server" CssClass="Etichetta" Text="Riservato" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="RiservatoCheckBox" runat="server" CssClass="etichetta" Text=""
                                                        Width="90px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </telerik:RadPageView>

                            </telerik:RadMultiPage>
                            <%--INIZIO GRIDVIEW--%>
                            <asp:ImageButton ID="AggiornaProtocolliImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                Style="display: none" />
                            <asp:Panel runat="server" ID="ProtocolliPanel">
                                <table style="width: 100%; background-color: #BFDBFF">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloElencoProtocolliLabel" runat="server" Font-Bold="True" CssClass="Etichetta"
                                                            Style="width: 570px; color: #00156E; background-color: #BFDBFF" Text="Elenco Registrazioni"
                                                            ToolTip="Ultime cinque registrazioni effettuate dall'utente corrente" />
                                                    </td>

                                                     <td align="center" style="width:30;">
                                                        <asp:ImageButton ID="EsportaInExcelImageButton" runat="server" ImageUrl="~/images/excel16.png"
                                                            ToolTip="Esporta in Excel i protocolli visualizzati" Style="border: 0" ImageAlign="AbsMiddle" />    
                                                    </td> 

                                                    <td align="center" style="width: 125; border-left: 0 solid #5D8CC9;">
                                                        <telerik:RadButton ID="NoPaging" runat="server" Text="Non Paginare" Skin="Office2007"
                                                            ImageAlign="AbsMiddle" Width="115px" ToolTip="Disattiva/Attiva Paginazione">
                                                            <Icon PrimaryIconUrl="~/images/Next.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td align="center" style="width: 40px">
                                                        <asp:ImageButton ID="VisualizzaSchermoInteroImageButton" Style="border: 0" runat="server"
                                                            ImageUrl="~/images//full_screen_icon.png" ToolTip="Visualizza griglia a schermo intero"
                                                            ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Filtra registrazioni" Style="border-style: none; border-color: inherit;
                                                            border-width: 0; width: 16px;" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="RipristinaFiltroInizialeImageButton" Style="border: 0" runat="server"
                                                            ImageUrl="~/images//cancelSearch.png" ToolTip="Ripristina filtro iniziale" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                          <div style="overflow: auto; height: 185px; width: 100%; background-color: #FFFFFF;
                                                border: 1px solid #5D8CC9;">
                                            <telerik:RadGrid ID="ProtocolliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" PageSize="5"  
                                                Culture="it-IT">
                                                <MasterTableView DataKeyNames="Id">
                                                    <Columns>

                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                         
                                                    
                                                        <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                            HeaderText="N." DataField="NumeroProtocollo" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <div title='<%#  Eval("NumeroProtocollo","{0:0000000}") %>' 
                                                                style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 50px; border: 0px solid red">
                                                                    <%# Eval("NumeroProtocollo", "{0:0000000}")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneTipologiaRegistristrazione"
                                                            UniqueName="DescrizioneTipologiaRegistristrazione" HeaderText="Tipo" DataField="DescrizioneTipologiaRegistristrazione"
                                                            HeaderStyle-Width="45px" ItemStyle-Width="45px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DescrizioneTipologiaRegistristrazione") %>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 45px;border: 0px solid red">
                                                                    <%# Eval("DescrizioneTipologiaRegistristrazione").Chars(0)%></div>
                                                            </ItemTemplate>

                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn SortExpression="DataImmissione" UniqueName="DataImmissione"
                                                            HeaderText="Data" DataField="DataImmissione" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DataImmissione","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 65px;border: 0px solid red">
                                                                    <%# Eval("DataImmissione", "{0:dd/MM/yyyy}")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                            DataField="Oggetto" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                                            <ItemTemplate>
                                                                <div id="Oggetto"  runat="server"  title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 180px;border: 0px solid red">
                                                                    <%# Eval("Oggetto")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn SortExpression="ElencoReferentiEsterni" UniqueName="ElencoReferentiEsterni"
                                                            HeaderText="Mittente/Destinatario" DataField="ElencoReferentiEsterni" HeaderStyle-Width="180px"
                                                            ItemStyle-Width="180px">
                                                            <ItemTemplate>
                                                                <div  title='<%# Replace(Eval("ElencoReferentiEsterni"), "'", "&#039;")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 180px;border: 0px solid red">
                                                                    <%# Eval("ElencoReferentiEsterni")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn SortExpression="ElencoReferentiInterni" UniqueName="ElencoReferentiInterni"
                                                            HeaderText="Ufficio" DataField="ElencoReferentiInterni" HeaderStyle-Width="180px"
                                                            ItemStyle-Width="180px">
                                                            <ItemTemplate>
                                                                <div  title='<%# Replace(Eval("ElencoReferentiInterni"), "'", "&#039;")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 180px;border: 0px solid red">
                                                                    <%# Eval("ElencoReferentiInterni")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>


                                                        <telerik:GridButtonColumn  ImageUrl="~/images/Checks.png"
                                                          ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"      
                                                        UniqueName="Select" ButtonType="ImageButton" CommandName="Select" />
                                                         

                                                        <telerik:GridButtonColumn  ImageUrl="~/images/copy16.png"
                                                          ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"      
                                                        UniqueName="Copy" ButtonType="ImageButton" CommandName="Copy" />
                                                       

                                                        <telerik:GridButtonColumn  ImageUrl="~/images/stamp.png"
                                                         ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"     
                                                         UniqueName="Stamp" ButtonType="ImageButton" CommandName="Stamp" />
                                                          
                                                           

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
                           
                            <%--FINE GRIDVIEW--%>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="EmailPanel" style="position: absolute; width: 100%; text-align: center;
                z-index: 2000000; display: none; top: 40px">
                <div id="ShadowEmailPanel" style="width: 800px; text-align: center; background-color: #BFDBFF;
                    margin: 0 auto">
                    <parsec:VisualizzaEmailControl runat="server" ID="VisualizzaEmailControl" />
                </div>
            </div>

               <div id="FullSizeGridPanel" style="position: absolute; width: 100%; height:100%;text-align: center; z-index : 999; display: none; top: 0px; left:0px; background-color:White">
                  
                  
                   <table style="width: 100%; height:100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                     <tr>
                           <td style=" vertical-align:top; height:30px">
                             <table style="width: 100%; background-color: #BFDBFF">
                                 <tr>
                                     <td>
                                         &nbsp;<asp:Label ID="TitoloElencoFullSizeProtocolliLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                             color: #00156E; background-color: #BFDBFF" Text="Elenco Registrazioni" />
                                     </td>
                                      <td align="center" style="width:125; border-left:0 solid #5D8CC9;">
                                                    <telerik:RadButton ID="FullSizeNoPaging" runat="server" Text="Non Paginare"   Skin="Office2007"
                                                      ImageAlign="AbsMiddle"  Width="115px" 
                                                      ToolTip="Disattiva/Attiva Paginazione">  
                                                      <Icon PrimaryIconUrl="~/images/Next.png" PrimaryIconLeft="5px"/>
                                                    </telerik:RadButton>
                                                </td>

                                     <td align="center" style="width: 40px">
                                         <img alt="Nascondi griglia a schermo intero" src="../../../../images/original_size_icon.png"
                                             style="border: 0px" onclick="HideFullSizeGridPanel();" />
                                     </td>
                                 </tr>
                             </table>
                         </td>
                     </tr>

                     <tr>
                    <td style=" vertical-align:top; padding-bottom:2px; position:relative">

                    <div  id="fullSizeScrollPanel" runat="server" style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF; 
                                    border-top:1px solid #5D8CC9; position:absolute;">

                                         

                      <telerik:RadGrid ID="FullSizeProtocolliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False" Culture="it-IT"
                                            CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True" AllowFilteringByColumn="True" EnableLinqExpressions ="false">

                          <GroupingSettings CaseSensitive="False" />
                                            
                                            <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                <Columns>

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />


                                                    <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                        HeaderText="N." DataField="NumeroProtocollo" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                                                        AutoPostBackOnFilter="True" FilterControlWidth="100%" ShowFilterIcon="False" CurrentFilterFunction="EqualTo">

                                                         <FilterTemplate>
                                                                        <telerik:RadNumericTextBox Width="100%" ID="FiltroNumeroProtocolloTextBox" runat="server"
                                                                            ClientEvents-OnLoad="OnFiltroNumeroProtocolloTextBoxLoad" DbValue='<%# TryCast(Container,GridItem).OwnerTableView.GetColumn("NumeroProtocollo").CurrentFilterValue %>'
                                                                            ClientEvents-OnKeyPress="OnFiltroNumeroProtocolloTextBoxKeyPressed" Skin="Office2007">
                                                                            <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                        </telerik:RadNumericTextBox>
                                                                        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                                            <script type="text/javascript">
                                                                                function OnFiltroNumeroProtocolloTextBoxKeyPressed(sender, args) {
                                                                                    if (13 == args.get_keyCode()) {
                                                                                        var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                        var c = sender.get_textBoxValue();
                                                                                        tableView.filter("NumeroProtocollo", c, "EqualTo");
                                                                                        args.set_cancel(true);
                                                                                    }
                                                                                    var text = sender.get_value() + args.get_keyCharacter();
                                                                                    if (!text.match('^[0-9]+$'))
                                                                                        args.set_cancel(true);
                                                                                }

                                                                                // SOVRASCRIVO GLI STILI
                                                                                function OnFiltroNumeroProtocolloTextBoxLoad(sender, args) {
                                                                                    sender.get_styles().HoveredStyle[0] = "";
                                                                                    sender.get_styles().HoveredStyle[1] = "";
                                                                                    sender.get_styles().FocusedStyle[0] = "";
                                                                                    sender.get_styles().FocusedStyle[1] = "";
                                                                                    sender.updateCssClass();
                                                                                }
                                                                            </script>
                                                                        </telerik:RadScriptBlock>
                                                                    </FilterTemplate>

                                                            <ItemTemplate>
                                                                <div title='<%#  Eval("NumeroProtocollo","{0:0000000}") %>' 
                                                                style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                    <%# Eval("NumeroProtocollo", "{0:0000000}")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                             

                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneTipologiaRegistristrazione"
                                                            UniqueName="DescrizioneTipologiaRegistristrazione" HeaderText="Tipo" DataField="DescrizioneTipologiaRegistristrazione"
                                                            HeaderStyle-Width="65px" ItemStyle-Width="65px"
                                                            AutoPostBackOnFilter="True" FilterControlWidth="100%"  
                                                                    ShowFilterIcon="False" CurrentFilterFunction="StartsWith"
                                                            >

                                                            <FilterTemplate>
                                                                <telerik:RadComboBox ID="FiltroTipologiaRegistrazioneComboBox" runat="server" DataSourceID="XmlDataSource1"
                                                                    DataTextField="Text" DataValueField="Value"  Skin="Office2007"
                                                                    SelectedValue='<%# DescrizioneTipologiaSelezionata(Container) %>'
                                                                    Width="100%" AppendDataBoundItems="true" OnClientSelectedIndexChanged="OnFiltroTipologiaRegistrazioneComboBoxIndexChanged">
                                                                    <Items>
                                                                      <telerik:RadComboBoxItem Text="" Value="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>

                                                                <telerik:RadScriptBlock ID="RadScriptBlock4" runat="server">
                                                                    <script type="text/javascript">

                                                                        function OnFiltroTipologiaRegistrazioneComboBoxIndexChanged(sender, args) {
                                                                            var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");

                                                                            if (args.get_item().get_value() == "") {
                                                                                tableView.filter("DescrizioneTipologiaRegistristrazione", args.get_item().get_value(), "NoFilter");
                                                                            }
                                                                            else {
                                                                                tableView.filter("DescrizioneTipologiaRegistristrazione", args.get_item().get_value(), "EqualTo");
                                                                            }
                                                                        } 

                                                                    </script>
                                                                </telerik:RadScriptBlock>

                                                            </FilterTemplate>


                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DescrizioneTipologiaRegistristrazione") %>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 65px;border: 0px solid red">
                                                                    <%# Eval("DescrizioneTipologiaRegistristrazione").Chars(0)%></div>
                                                            </ItemTemplate>

                                                        </telerik:GridTemplateColumn>

                                                    

                                                    <telerik:GridTemplateColumn SortExpression="DataImmissione" UniqueName="DataImmissione"
                                                            HeaderText="Data" DataField="DataImmissione" HeaderStyle-Width="85px" ItemStyle-Width="85px"
                                                            AutoPostBackOnFilter="True" FilterControlWidth="100%" 
                                                                    ShowFilterIcon="False">


                                                                      <FilterTemplate>
                                                                          <telerik:RadDatePicker ID="DataImmissioneTextBox" Skin="Office2007" ShowPopupOnFocus="true"
                                                                              DatePopupButton-Visible="false" Width="100%" runat="server" MinDate="1753-01-01" 
                                                                              ClientEvents-OnDateSelected="DateSelected" DbSelectedDate='<%# DataImmissione(Container) %>'
                                                                              DateInput-ClientEvents-OnKeyPress="OnDataImmissioneTextBoxKeyPressed" >
                                                                                <Calendar ID="Calendar8" runat="server">
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" 
                                                                                                            Repeatable="Today" />
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                              </telerik:RadDatePicker>

                                                                                   <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                                                                      
                                                                                       <script type="text/javascript">

                                                                                           function OnDataImmissioneTextBoxKeyPressed(sender, args) {
                                                                                               if (13 == args.get_keyCode()) {
                                                                                                   var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                   var c = sender.get_textBoxValue();

                                                                                                   var idPicker = sender.get_element().parentNode.parentNode.children(0).id;
                                                                                                   var picker = $find(idPicker)

                                                                                                   if (Date.parse(c)) {

                                                                                                       var ddmmyyyy = c.split('/');
                                                                                                       var mmddyyyy = ddmmyyyy[1] + '/' + ddmmyyyy[0] + '/' + ddmmyyyy[2];

                                                                                                       picker.set_selectedDate(new Date(mmddyyyy));
                                                                                                       // OnDataDocumentoTextBoxDateSelected(picker, "");
                                                                                                       picker.hidePopup();
                                                                                                   } else {
                                                                                                       picker.set_selectedDate(null);
                                                                                                       picker.hidePopup();
                                                                                                   }
                                                                                                   args.set_cancel(true);

                                                                                               }
                                                                                           }


                                                                                           function DateSelected(sender, args) {
                                                                                               var tableView = $find("<%# ctype(Container,GridItem).OwnerTableView.ClientID %>");

                                                                                               var date = FormatSelectedDate(sender);
                                                                                               var toDate = '';

                                                                                               try {
                                                                                                   var dateInput = sender.get_dateInput();
                                                                                                   var d = sender.get_selectedDate();
                                                                                                   d.setDate(d.getDate() + 1);
                                                                                               }
                                                                                               catch (e) {
                                                                                               }


                                                                                               toDate = dateInput.get_dateFormatInfo().FormatDate(d, dateInput.get_displayDateFormat());

                                                                                               tableView.filter("DataImmissione", date + " " + toDate, "Between");

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
                                                                <div title='<%# Eval("DataImmissione","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 75px;border: 0px solid red">
                                                                    <%# Eval("DataImmissione", "{0:dd/MM/yyyy}")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                    <telerik:GridBoundColumn DataField="Oggetto" FilterControlAltText="Filter Oggetto column"
                                                         HeaderText="Oggetto" SortExpression="Oggetto"
                                                        UniqueName="Oggetto"
                                                        AutoPostBackOnFilter="True" FilterControlWidth="100%"
                                                                    ShowFilterIcon="False"
                                                        >
                                                    </telerik:GridBoundColumn>

                                                         <telerik:GridBoundColumn DataField="ElencoReferentiEsterni" FilterControlAltText="Filter ElencoReferentiEsterni column"
                                                        HeaderStyle-Width="220px" ItemStyle-Width="220px" HeaderText="Mittente/Destinatario" SortExpression="ElencoReferentiEsterni"
                                                        UniqueName="ElencoReferentiEsterni"
                                                        AutoPostBackOnFilter="True" FilterControlWidth="100%"
                                                                    ShowFilterIcon="False"
                                                        >
                                                    </telerik:GridBoundColumn>

                                                 

                                                    
                                                     <telerik:GridBoundColumn DataField="ElencoReferentiInterni" FilterControlAltText="Filter ElencoReferentiInterni column" 
                                                         HeaderText="Ufficio" SortExpression="ElencoReferentiInterni"  HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                                        UniqueName="ElencoReferentiInterni"
                                                        AutoPostBackOnFilter="True" FilterControlWidth="100%"
                                                                    ShowFilterIcon="False">
                                                    </telerik:GridBoundColumn>
                                                  
                                                    <telerik:GridButtonColumn ButtonType="ImageButton"   CommandName="Select" HeaderStyle-Width="30px"
                                                        ItemStyle-Width="30px" FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png"
                                                        UniqueName="Select" Text="Seleziona Registrazione"  />
                                                </Columns>
                                                
                                            </MasterTableView>

                                             
                                        </telerik:RadGrid>
                      
                        <asp:XmlDataSource ID="XmlDataSource1" runat="server">
                            <Data>
                <Items>
                  <Item Value="Arrivo" Text="A"></Item>
                    <Item Value="Partenza" Text="P"></Item>
                    <Item Value="Interno" Text="I"></Item>
                </Items>
                            </Data>
                        </asp:XmlDataSource>

                                        </div>
                    </td>

                     </tr>
                 </table>
             


                    


                </div>



                

            
              <asp:HiddenField ID="infoOperazioneHidden" runat="server" />

                <asp:ImageButton ID="InviaEmailButton" runat="server" ImageUrl="~/images//knob-search16.png"
                        Style="display: none" />

         
             <asp:ImageButton ID="AggiornaFirmaDigitaleImageButton" runat="server" Style="display: none" />
             <asp:HiddenField ID="signerOutputHidden" runat="server" />

              <asp:ImageButton ID="chiudiImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                Style="display: none; width: 0px" />
   
       </div>

        </ContentTemplate>



       <%-- <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TipoRegistrazioneRadioList" EventName="SelectedIndexChanged" />          
        </Triggers>--%>




    </asp:UpdatePanel>







   
   
      
          




</asp:Content>