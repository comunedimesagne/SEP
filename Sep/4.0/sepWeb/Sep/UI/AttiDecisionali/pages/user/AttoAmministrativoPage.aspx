<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="AttoAmministrativoPage.aspx.vb" Inherits="AttoAmministrativoPage" %>

<%@ Register Src="~/UI/Protocollo/pages/user/VisualizzaFatturaUserControl.ascx" TagName="VisualizzaFatturaControl"
    TagPrefix="parsec" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">


    <asp:PlaceHolder ID="bloccoCodice" runat="server">
        <script type="text/javascript">


        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');
         var overlay = document.createElement("div");

        var count = 1;
        var hideFullSizePanel = true;
        var showUI = true;
        var hideFatturaPanel = true;

        Sys.Application.add_init(function () {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        });

         function pageLoad() {
            
             $get("pageContent").appendChild(_backgroundElement);
               $get("pageContent").appendChild(overlay);

              if (hideFatturaPanel) {
                HideFatturaElettronicaPanel();
            } else {
                ShowFatturaElettronicaPanel();
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


    function HideUpload(id){
          var upload = $find(id);

          if(upload != null){
              var uploadDiv = upload.get_element();
              var inputs = uploadDiv.getElementsByTagName("input");
              inputs[1].style.display = 'none';
           }

    }



        function OnBeginRequest(sender, args) {
            EnableUI(false);
         }

     

        function OnEndRequest(sender, args) {
            count = 1;
            var message = $get('<%= infoOperazioneHidden.ClientId %>').value;

            if (message !== '') {

                //VISUALIZZO IL MESSAGGIO

                ShowMessageBox(message);

                var intervallo = setInterval(function () {
                    count = count - 1;
                    if (count <= 0) {
                        HideMessageBox();
                        showUI = true;
                        EnableUI(true);
                        clearInterval(intervallo);
                        <% If Not Page.Request("Mode") Is Nothing Then %>

                         if (message !== 'Firma eseguita correttamente!') {
                             $get('<%= chiudiImageButton.ClientId %>').click();
                          }

                        <% End If %>
                       

                        if ($get('<%= verificaDocumentoHidden.ClientId %>').value == "SI") {
                            $get('<%= verificaRigenerazioneDocumento.ClientId %>').click();
                            $get('<%= verificaDocumentoHidden.ClientId %>').value = '';
                        }

                    }
                }, 1000);



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



         function HideFatturaElettronicaPanel() {
            //panelIndex = -1;
            var panel = document.getElementById("FatturaPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
        }

        

        function ShowFatturaElettronicaPanel() {

            //panelIndex = 1;
            var panel = document.getElementById("FatturaPanel");

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


            var shadow = document.getElementById("ShadowFatturaPanel");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }

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


        function OnClientButtonClicked(sender, args) {
            var button = args.get_item();
            if (button.get_text() == "Elimina") {
                if (button.get_enabled()) {
                       Confirm();
                   }
              }
           }



           //***********************************************************************
           //INIZIO GESTIONE CONTROLLO CARICAMENTO APPLET
           //***********************************************************************


          var notificato = false;

           



        function CheckAppletIsActive() {

            var isActive = true;

            try {

                for (i = 0; i < document.applets.length; i++) {
                 
                    var code = document.applets[i].code
                    if (code == 'ParsecComunicator.class') {
                      isActive = document.applets[i].isActive();
                      break;
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




           //***********************************************************************
           //FINE GESTIONE CONTROLLO CARICAMENTO APPLET
           //***********************************************************************


           //DISABILITO I PULSANTI CHE UTILIZZANO L'APPLET

           function DisableButtons() {
               
               DisableButtonToolbar(3);

               DisableButton(document.getElementById('<%= ScansionaImageButton.ClientID %>'))
               DisableButton(document.getElementById('<%= VisualizzaDocumentoImageButton.ClientID %>'))

               DisableButton(document.getElementById('<%= VisualizzaCopiaDocumentoImageButton.ClientID %>'))
               DisableButton(document.getElementById('<%= VisualizzaDocumentoFirmatoImageButton.ClientID %>'))

               DisableButton(document.getElementById('<%= VisualizzaDocumentoCollegatoImageButton.ClientID %>'))
               DisableButton(document.getElementById('<%= VisualizzaCopiaDocumentoCollegatoImageButton.ClientID %>'))
           }


           function DisableButton(button) {
               if (button) {
                   button.disabled = true;
                   button.title = 'Funzione disabilitata.\nContattare l\'assistenza.';
               }
           }

           function DisableButtonToolbar(index) {
               var toolBar = $find('<%= RadToolBar.ClientID %>');
               if (toolBar) {
                   var toolbarItems = toolBar.get_allItems();
                   // toolbarItems[index].set_enabled(false);
                   toolbarItems[index].disable();
                   toolbarItems[index].set_toolTip('Funzione disabilitata.\nContattare l\'assistenza.');
                   toolbarItems[index].get_element().title = 'Funzione disabilitata.\nContattare l\'assistenza.';
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


        //TODO FUNZIONE CHE NASCONDE O VISUALIZZA IL DIV


        function OnClientFileSelected(sender, args) {
           var currentFileName = args.get_fileName();
           alert(currentFileName);
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


        function CurriculumIncaricoAmministrativoSelezionato() {
            var divAggCurriculum = document.getElementById('divAggIncaricoAmministrativoCurriculum');
            divAggCurriculum.style.visibility = 'visible'
        }

        function CurriculumIncaricoAmministrativoRimosso() {
            var divAggCurriculum = document.getElementById('divAggIncaricoAmministrativoCurriculum');
            divAggCurriculum.style.visibility = 'hidden'
        }

        function InconferibilitaIncaricoAmministrativoSelezionato() {
            var divAggInconferibilita = document.getElementById('divAggIncaricoAmministrativoInconferibilita');
            divAggInconferibilita.style.visibility = 'visible'
        }

        function InconferibilitaIncaricoAmministrativoRimosso() {
            var divAggInconferibilita = document.getElementById('divAggIncaricoAmministrativoInconferibilita');
            divAggInconferibilita.style.visibility = 'hidden'
        }

        function IncompatibilitaIncaricoAmministrativoSelezionato() {
            var divAggInconferibilita = document.getElementById('divAggIncaricoAmministrativoIncompatibilita');
            divAggInconferibilita.style.visibility = 'visible'
        }

        function IncompatibilitaIncaricoAmministrativoRimosso() {
            var divAggIncompatibilita = document.getElementById('divAggIncaricoAmministrativoIncompatibilita');
            divAggIncompatibilita.style.visibility = 'hidden'
        }



         function InconferibilitaSelezionato() {
            var divAggInconferibilita = document.getElementById('divAggInconferibilita');
            divAggInconferibilita.style.visibility = 'visible'
        }

        function InconferibilitaRimosso() {
            var divAggInconferibilita = document.getElementById('divAggInconferibilita');
            divAggInconferibilita.style.visibility = 'hidden'
        }

        function IncompatibilitaSelezionato() {
            var divAggIncompatibilita = document.getElementById('divAggIncompatibilita');
            divAggIncompatibilita.style.visibility = 'visible'
        }

        function IncompatibilitaRimosso() {
            var divAggIncompatibilita = document.getElementById('divAggIncompatibilita');
            divAggIncompatibilita.style.visibility = 'hidden'
        }




        </script>
    </asp:PlaceHolder>


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


    <%--<asp:UpdatePanel ID="Pannello" runat="server">--%>
    <%--  <ContentTemplate>--%>

    <div id="AjaxPanel" runat="server">
        <div id="pageContent">

           


            <table style="width: 900px; border: 1px solid #5D8CC9">
                <tr>
                    <td>
                        <%--INIZIO TOOLBAR--%>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%"
                                        OnClientButtonClicked="OnClientButtonClicked">
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
                                            <telerik:RadToolBarDropDown runat="server" ImageUrl="~/images/Printer.png" Text="Stampa">
                                                <Buttons>
                                                    <telerik:RadToolBarButton runat="server" CommandName="RegistroGeneraleDelibere" Text="Registro Generale Delibere..."
                                                        Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="RegistroGeneraleDetermine"
                                                        Text="Registro Generale Determine..." Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="RegistroGeneraleDecreti" Text="Registro Generale Decreti..."
                                                        Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="RegistroGeneraleOrdinanze"
                                                        Text="Registro Generale Ordinanze..." Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="RegistroSettoreDetermine" Text="Registro Settore Determine..."
                                                        Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="ElencoDetermineImpegnoSpesa"
                                                        Text="Elenco Determine (Spesa)..." Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="ElencoDeterminePubblicazione"
                                                        Text="Elenco Determine (Pubblicazione)..." Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="ElencoDetermineLiquidazione"
                                                        Text="Elenco Determine (Liquidazione)..." Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="ElencoDelibereImpegnoSpesa"
                                                        Text="Elenco Delibere (Spesa)..." Width="260px" />
                                                    <telerik:RadToolBarButton runat="server" CommandName="ElencoDeliberePubblicazione"
                                                        Text="Elenco Delibere (Pubblicazione)..." Width="260px" />
                                                </Buttons>
                                            </telerik:RadToolBarDropDown>
                                            <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/AdvancedSearch32.png"
                                                Text="Ricerca Avanzata" CommandName="RicercaAvanzata" Owner="RadToolBar" />

                                           

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
                                            <td align="right">
                                                <asp:ImageButton ID="VisualizzaStoricoDocumentoImageButton" runat="server" ImageUrl="~/images//FolderHistory.png"
                                                    Style="border: 0px" ToolTip="Visualizza storico documento selezionato" ImageAlign="Top"
                                                    Visible="false" />
                                                 &nbsp;
                                                <asp:ImageButton ID="VisualizzaDocumentoFirmatoPubblicazioneImageButton" runat="server" ImageUrl="~/images//DocumentoFirmatoAlbo.gif"
                                                    Style="border: 0px" ToolTip="Visualizza documento firmato per la pubblicazione" ImageAlign="Top" Visible="false" />
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
                                            <td align="right">
                                                <asp:Label ID="InfoSettoreLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                    color: #00156E; background-color: #BFDBFF; font-family: Verdana; font-size: 10px"
                                                    Text="" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <telerik:RadTabStrip runat="server" ID="AttiTabStrip" SelectedIndex="0" MultiPageID="AttiMultiPage"
                            Skin="Office2007" Width="100%">
                            <Tabs>
                                <telerik:RadTab Text="Generale" Selected="True" />
                                <telerik:RadTab Text="Presenze" ToolTip="Elenco Presenze Assessori\Consiglieri">
                                    <TabTemplate>
                                        <asp:Label ID="TabLabel" runat="server" Text="" />
                                    </TabTemplate>
                                </telerik:RadTab>
                                <telerik:RadTab Text="Contabilità" ToolTip="Dettaglio dei dati contabili">
                                    <TabTemplate>
                                        <asp:Label ID="TabLabel" runat="server" Text="" />
                                    </TabTemplate>
                                </telerik:RadTab>
                                <telerik:RadTab Text="Allegati" ToolTip="Elenco allegati">
                                </telerik:RadTab>
                                <telerik:RadTab Text="Classificazioni" ToolTip="Elenco classificazioni">
                                    <TabTemplate>
                                        <asp:Label ID="TabLabel" runat="server" Text="" />
                                    </TabTemplate>
                                </telerik:RadTab>
                                <telerik:RadTab Text="Visibilità" ToolTip="Elenco elementi della visibilità" />

                                <%--<telerik:RadTab Text="Trasparenza" ToolTip="Sezione amministrazione trasparente">
                                    <TabTemplate>
                                        <asp:Label ID="TabLabel" runat="server" Text="" />
                                    </TabTemplate>
                                </telerik:RadTab>--%>

                                <telerik:RadTab Text="Fascicoli" ToolTip="Sezione fascicoli procedimenti">
                                </telerik:RadTab>
                            </Tabs>
                        </telerik:RadTabStrip>
                        <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                        <telerik:RadMultiPage runat="server" ID="AttiMultiPage" SelectedIndex="2" Height="100%"
                            Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                            <telerik:RadPageView runat="server" ID="GeneralePageView" CssClass="corporatePageView"
                                Height="425px">
                                <div id="GeneralePanel" runat="server" style="padding: 2px 2px 2px 2px;">
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
                                                            <telerik:RadNumericTextBox ID="NumeroSettoreTextBox" runat="server" Skin="Office2007"
                                                                Width="70px" DataType="System.Int32" MaxLength="4" MaxValue="9999" MinValue="1"
                                                                ShowSpinButtons="True" ToolTip="Numero Settore Atto">
                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                            </telerik:RadNumericTextBox>
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
                                            <td style="width: 60px; border: 0px solid red; text-align: center">
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
                                                            <telerik:RadTextBox ID="UfficioTextBox" runat="server" Skin="Office2007" Width="330px" />
                                                        </td>
                                                        <td align="center" style="width: 25px">
                                                            <asp:ImageButton ID="TrovaUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                ToolTip="Seleziona ufficio..." ImageAlign="AbsMiddle" Style="height: 16px" />
                                                        </td>
                                                        <td style="width: 25px">
                                                            <asp:ImageButton ID="EliminaUfficioImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                ToolTip="Cancella ufficio" ImageAlign="AbsMiddle" />
                                                            <asp:ImageButton ID="AggiornaUfficioImageButton" runat="server" Style="display: none" />
                                                            <asp:TextBox ID="IdUfficioTextBox" runat="server" Style="display: none" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="center" style="width: 70px">
                                                <asp:Label ID="SettoreLabel" runat="server" CssClass="Etichetta" Text="Settore" />
                                            </td>
                                            <td style="width: 380px">
                                                <telerik:RadTextBox ID="SettoreTextBox" runat="server" Skin="Office2007" Width="370px" /><asp:TextBox
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
                                                           
                                                             <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="330px"
                                                                            Rows="3" TextMode="MultiLine" />
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <telerik:RadButton ID="SalvaNoteButton" runat="server" Text="" Width="28px" Skin="Office2007" Visible="False"
                                                                            ToolTip="Salva note">
                                                                            <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                                                        </telerik:RadButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                                                        <telerik:RadNumericTextBox ID="NumeroRegistroPubblicazioneTextBox" MinValue="1" runat="server"
                                                            Skin="Office2007" Width="90px" DataType="System.Int32" ShowSpinButtons="True"
                                                            MaxLength="5" Enabled="False">
                                                            <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                        </telerik:RadNumericTextBox>
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="ProtocolloPanel" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="NumeroProtocolloLabel" runat="server" CssClass="Etichetta" Text="Protocollo n." />
                                                    </td>
                                                    <td style="width: 90px">
                                                        <telerik:RadTextBox ID="NumeroProtocolloTextBox" runat="server" Skin="Office2007"
                                                            Width="90px" />
                                                    </td>
                                                    <td style="width: 10px; text-align: center">
                                                        <asp:Label ID="DataProtocolloLabel" runat="server" CssClass="Etichetta" Text="/" />
                                                    </td>
                                                    <td style="width: 95px">
                                                        <telerik:RadTextBox ID="DataProtocolloTextBox" runat="server" Skin="Office2007" Width="90px" />
                                                    </td>
                                                    <td style="width: 30px; text-align: center">
                                                        <asp:ImageButton ID="TrovaProtocolloImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                            ToolTip="Protocolla documento..." ImageAlign="AbsMiddle" Width="16px" />
                                                    </td>
                                                    <td style="text-align: left; width: 600px">
                                                        <asp:ImageButton ID="EliminaProtocolloImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                            ToolTip="Cancella protocollo" ImageAlign="AbsMiddle" />
                                                        <asp:ImageButton ID="AggiornaProtocolloImageButton" runat="server" Style="display: none" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="BozzaPanel" runat="server" Style="vertical-align: bottom">
                                            <div style="border-top: 1px solid #9ABBE8; border-bottom: 0px solid #9ABBE8; padding-top: 4px;
                                                padding-bottom: 4px;">
                                            </div>
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td style="width: 90px">
                                                            <asp:Label ID="BozzaLabel" runat="server" CssClass="Etichetta" Text="Carica Corpo" />
                                                        </td>
                                                        <td style="width: 450px">
                                                            <telerik:RadTextBox ID="BozzaTextBox" runat="server" Skin="Office2007" Width="450px" />
                                                        </td>
                                                        <td style="width: 30px; text-align: center">
                                                            <asp:ImageButton
                                                                ID="TrovaBozzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                ToolTip="Seleziona Bozza/Determina..." ImageAlign="AbsMiddle" />
                                                        </td>
                                                        <td style="width: 30px;text-align:left">
                                                            <asp:ImageButton ID="EliminaBozzaImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                ToolTip="Cancella Bozza/Determina" ImageAlign="AbsMiddle" />

                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="AggiornaBozzaImageButton" runat="server" Style="display: none" />
                                                            <asp:TextBox ID="PercorsoRemotoCorpoDocumentoTextBox" runat="server" Style="display: none" />
                                                            
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
                                                                        <asp:Label ID="ElencoFirmeLabel" runat="server" Style="color: #00156E" Font-Bold="True"
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
                                                                        <div id="scrollPanelFirme" style="overflow: auto; height: 160px; width: 100%; background-color: #FFFFFF;
                                                                            border: 0px solid #5D8CC9;">
                                                                            <telerik:RadGrid ID="FirmeGridView" runat="server" ToolTip="Elenco firme associate al documento"
                                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                Culture="it-IT">
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
                                                                                            ItemStyle-Width="20px" FilterControlAltText="Filter Preview column" ImageUrl="~\images\edit16.png"
                                                                                            UniqueName="Preview" />
                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                                            ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
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
                                                                Width="90px" DataType="System.Int32" MaxLength="3" ShowSpinButtons="True" MaxValue="999"
                                                                MinValue="1">
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
                                <div id="GrigliaPresenzePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
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
                                                <div style="overflow: auto; height: 310px; border: 1px solid #5D8CC9">
                                                    <telerik:RadGrid ID="PresenzeGridView" runat="server" ToolTip="Elenco presenze associate al documento"
                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                        Culture="it-IT">
                                                        <MasterTableView DataKeyNames="IdStruttura">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="IdStruttura" DataType="System.Int32" FilterControlAltText="Filter IdStruttura column"
                                                                    HeaderText="IdStruttura" ReadOnly="True" SortExpression="IdStruttura" UniqueName="IdStruttura"
                                                                    Visible="False" />
                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" AllowFiltering="False">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="PresenteCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
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
                                <div id="ContabilitaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                    <table runat="server" id="ImpegniSpesaTable" style="width: 100%; background-color: #BFDBFF;
                                        border: 1px solid #5D8CC9;">
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
                                                        ShowFooter="true" Culture="it-IT">
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
                                                                    HeaderText="Descrizione" DataField="Note" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("Note")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                            width: 300px; border: 0px solid red">
                                                                            <%# Eval("Note")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn SortExpression="Importo" UniqueName="Importo" FooterStyle-HorizontalAlign="Right"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderText="Importo" DataField="Importo"
                                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("Importo","{0:N2}")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 100px; border: 0px solid red">
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
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderText="Sub Imp." DataField="NumeroSubImpegno"
                                                                    HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("NumeroSubImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                            <%# Eval("NumeroSubImpegno")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                                                    UniqueName="Copy" ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy"
                                                                    Text="Copia Impegno di Spesa..." />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                    Text="Modifica Impegno di Spesa..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                    ImageUrl="~\images\edit16.png" UniqueName="Select" />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                    Text="Elimina Impegno di Spesa" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                    ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="16px"
                                                                    Text="Visualizza Impegno di Spesa..." ItemStyle-Width="16px" />
                                                            </Columns>
                                                        </MasterTableView></telerik:RadGrid></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="SeparatorePanel1" runat="server" style="padding: 0px 2px 2px 2px;">
                                    </div>
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
                                                        ShowFooter="true" Culture="it-IT">
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
                                                                    HeaderStyle-Width="45px" ItemStyle-Width="45px">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("NumeroImpegno")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 45px; border: 0px solid red">
                                                                            <%# Eval("NumeroImpegno")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn SortExpression="AnnoEsercizio" UniqueName="AnnoEsercizio"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderText="Anno L." DataField="AnnoEsercizio"
                                                                    HeaderStyle-Width="45px" ItemStyle-Width="45px">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("AnnoEsercizio")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: AnnoEsercizio; width: 45px; border: 0px solid red">
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
                                                                    DataField="ImportoLiquidato" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                                                    ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("ImportoLiquidato","{0:N2}")%>' style="white-space: nowrap;
                                                                            overflow: hidden; text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                            <%# Eval("ImportoLiquidato", "{0:N2}")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn SortExpression="Nominativo" UniqueName="Nominativo" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderText="Beneficiario" DataField="Nominativo" HeaderStyle-Width="190px" ItemStyle-Width="190px">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: <%= LarghezzaContenitore %>; border: 0px solid red">
                                                                            <%# Eval("Nominativo")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                                                    UniqueName="Copy" ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy"
                                                                    Text="Copia Liquidazione..." />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                    Text="Modifica Liquidazione..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                    ImageUrl="~\images\edit16.png" UniqueName="Select" />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                    Text="Elimina Liquidazione" ItemStyle-Width="20px" HeaderStyle-Width="20px" ImageUrl="~\images\Delete16.png"
                                                                    UniqueName="Delete" />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                    ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="10px"
                                                                    Text="Visualizza Liquidazione..." ItemStyle-Width="10px">
                                                                </telerik:GridButtonColumn>
                                                            </Columns>
                                                        </MasterTableView></telerik:RadGrid></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="SeparatorePanel2" runat="server" style="padding: 0px 2px 2px 2px;">
                                    </div>
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
                                                        ShowFooter="true" Culture="it-IT">
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
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy"
                                                                    Text="Copia Accertamento..." />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                    Text="Modifica Accertamento..." ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                    ImageUrl="~\images\edit16.png" UniqueName="Select" />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                    Text="Elimina Accertamento" ItemStyle-Width="20px" HeaderStyle-Width="20px" ImageUrl="~\images\Delete16.png"
                                                                    UniqueName="Delete" />
                                                            </Columns>
                                                        </MasterTableView></telerik:RadGrid></div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="AllegatiPageView" CssClass="corporatePageView"
                                Height="425px">
                                <div id="AllegatiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                    <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 90px">
                                                            <asp:Label ID="TipoAllegatoLabel" runat="server" CssClass="Etichetta" Text="Tipo" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="TipologiaAllegatoComboBox" runat="server" EmptyMessage="- Seleziona Tipologia -"
                                                                Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                                                Width="200px" ToolTip="Tipologia Allegato" />
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
                                <div id="GrigliaAllegatiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                        <tr>
                                            <td style="height: 20px">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati" />
                                                        </td>
                                                        <td style="width: 30px; text-align: center">
                                                            <asp:ImageButton ID="TrovaFatturaImageButton" runat="server" ImageUrl="~/images//AddFattura.png"
                                                                ToolTip="Allega fattura elettronica" TabIndex="44" BorderStyle="None" ImageAlign="AbsMiddle" />
                                                            <asp:ImageButton ID="AllegaFatturaImageButton" Style="display: none" runat="server"
                                                                ImageUrl="~/images//RecycleEmpty.png" />
                                                        </td>
                                                        <td style="width: 30px; text-align: center">
                                                            <asp:ImageButton ID="ScansionaImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                ToolTip="Allega documento digitalizzato" TabIndex="44" BorderStyle="None" ImageAlign="AbsMiddle" />
                                                        </td>
                                                        <td style="width: 30px; text-align: center">
                                                            <asp:ImageButton ID="AggiungiDocumentoImageButton" runat="server" ImageUrl="~/images//add16.png"
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
                                                        Culture="it-IT" AllowMultiRowSelection="true">
                                                        <MasterTableView DataKeyNames="Id, Nomefile, NomeFileFirmato">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Pubblicazione allegati albo pretorio on-line"
                                                                    AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20px"
                                                                    ItemStyle-Width="20px">
                                                                    <HeaderTemplate>
                                                                        <div style="width: 20px; height: 20px">
                                                                            <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                                runat="server" ToolTip="Seleziona tutto"></asp:CheckBox>
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="AllegatiToggleRowSelection" AutoPostBack="True"
                                                                            runat="server" ToolTip="Pubblicazione dell'allegato all'albo pretorio on-line e nella sotto-sezione Provvedimenti della sezione Amministrazione Trasparente">
                                                                        </asp:CheckBox>
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
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="Firma" CommandName="Firma"
                                                                    ImageUrl="~/images/firmaDocumento16.png" ItemStyle-Width="16px" HeaderStyle-Width="16px" />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="SignedPreview" CommandName="SignedPreview"
                                                                    ImageUrl="~/images/signedDocument16.png" ItemStyle-Width="16px" HeaderStyle-Width="16px" />
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
                                                    <div id="Div1" runat="server" style="overflow: auto; height: 310px; border: 1px solid #5D8CC9;
                                                        background-color: White">
                                                        <telerik:RadGrid ID="ClassificazioniGridView" runat="server" ToolTip="Elenco classificazioni associate al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                            Culture="it-IT">
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
                                                            <asp:ImageButton ID="TrovaUtenteVisibilitaImageButton" runat="server" ImageUrl="~/images//user_add.png"
                                                                ImageAlign="AbsMiddle" BorderStyle="None" ToolTip="Aggiungi Utente..." />
                                                            &nbsp;<asp:ImageButton ID="TrovaGruppoVisibilitaImageButton" runat="server" ImageUrl="~/images//group_add.png"
                                                                ToolTip="Aggiungi Gruppo..." ImageAlign="AbsMiddle" BorderStyle="None" />
                                                            &nbsp;<asp:ImageButton ID="AggiornaGruppoVisibilitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                Style="display: none" />
                                                            <asp:ImageButton ID="AggiornaUtenteVisibilitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
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
                                                        Culture="it-IT">
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
                                                                        <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 720px; border: 0px solid red">
                                                                            <%# Eval("Descrizione")%></div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridButtonColumn FilterControlAltText="Filter Delete column" ImageUrl="~/images/Delete16.png"
                                                                    ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-VerticalAlign="Middle" UniqueName="Delete" ButtonType="ImageButton"
                                                                    CommandName="Delete" />
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>


                               
                               

                                <div id="ZoneID1">


                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 80px">
                                                &nbsp;<asp:Label ID="RiservatoLabel" runat="server" CssClass="Etichetta" Text="Riservato" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="RiservatoCheckBox" runat="server" CssClass="etichetta" Text=""
                                                    Width="90px"  />
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
                                                    Enabled="true" EmptyMessage="- Seleziona -" Filter="StartsWith" ItemsPerRequest="10"
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
                                                                            &nbsp;<asp:Label ID="DatiPubblicazioneLabel" runat="server" Style="color: #00156E"
                                                                                Font-Bold="True" CssClass="Etichetta" Text="Dati Pubblicazione" />
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
                                                                                            <Calendar runat="server">
                                                                                                <SpecialDays>
                                                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                </SpecialDays>
                                                                                            </Calendar>
                                                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                                                        </telerik:RadDatePicker>
                                                                                    </td>
                                                                                    <td style="width: 40px; text-align: center">
                                                                                        <asp:Label ID="DataFinePubblicazioneLabel" runat="server" CssClass="Etichetta" Text="A *" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadDatePicker ID="DataFinePubblicazioneTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" ToolTip="Data fine pubblicazione" MinDate="1753-01-01">
                                                                                             <Calendar runat="server">
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
                                      
                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                border-top: 0px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 100%">
                                                                                        &nbsp;<asp:Label ID="DatiAttiConcessioneLabel" runat="server" Style="color: #00156E"
                                                                                            Font-Bold="True" CssClass="Etichetta" Text="Dati Atti Concessione" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <telerik:RadTabStrip runat="server" ID="AttiConcessioneTabStrip" SelectedIndex="0"
                                                        MultiPageID="AttiConcessioneMultiPage" Skin="Office2007" Width="100%">
                                                        <Tabs>
                                                            <telerik:RadTab Text="Generale" Selected="True" />
                                                            <telerik:RadTab Text="Documenti" />
                                                        </Tabs>
                                                    </telerik:RadTabStrip>
                                                    
                                                    <telerik:RadMultiPage runat="server" ID="AttiConcessioneMultiPage" SelectedIndex="0"
                                                        Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                                        <telerik:RadPageView runat="server" ID="AttiConcessionePageView" CssClass="corporatePageView"
                                                            Height="260px">
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
                                                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="Contains" Skin="Office2007"
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
                                                                            <div id="Div2" runat="server" style="overflow: auto; height: 200px; border: 1px solid #5D8CC9;
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
                                                        </telerik:RadPageView>
                                                        <telerik:RadPageView runat="server" ID="AllegatiAttiConcessionePageView" CssClass="corporatePageView"
                                                            Height="260px">
                                                            <div id="AllegatiAttiConcessionePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr style="height: 30px">
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="DescrizioneDocumentoAttiConcessioneLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Descrizione" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="DescrizioneDocumentoAttiConcessioneTextBox" runat="server"
                                                                                            Skin="Office2007" Width="250px" />
                                                                                    </td>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="NomeFileDocumentoAttiConcessioneLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Nome file" />
                                                                                    </td>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <telerik:RadAsyncUpload ID="AllegatoAttiConcessioneUpload" runat="server" MaxFileInputsCount="1"
                                                                                            Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                        </telerik:RadAsyncUpload>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="TipoAllegatoAttiConcessioneLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Tipo" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioAttiConcessioneRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                        <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoAttiConcessioneRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />
                                                                                    </td>
                                                                                    <td style="width: 140px">
                                                                                        <asp:Label ID="OpzioniScannerAttiConcessioneLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Opzioni scanner" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="FronteRetroAttiConcessioneLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Fronte retro" />&nbsp;<asp:CheckBox ID="FronteRetroAttiConcessioneCheckBox"
                                                                                                runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label ID="VisualizzaUIAttiConcessioneLabel"
                                                                                                    runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                                        ID="VisualizzaUIAttiConcessioneCheckBox" runat="server" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="GrigliaAllegatiAttiConcessionePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td style="height: 20px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="DocumentiAttiConcessioneLabel" runat="server" CssClass="Etichetta"
                                                                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                                                                            Text="Allegati" />
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:ImageButton ID="ScansionaAttiConcessioneImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                                            ToolTip="Allega documento digitalizzato" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                                                ID="AggiungiDocumentoAttiConcessioneImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                ToolTip="Allega documento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                                                    ID="ScanUploadAttiConcessioneButton" Style="display: none" runat="server" ImageUrl="~/images//RecycleEmpty.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: #FFFFFF">
                                                                        <td>
                                                                            <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                <telerik:RadGrid ID="AllegatiAttiConcessioneGridView" runat="server" ToolTip="Elenco allegati associati alla pubblicazione"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Culture="it-IT" AllowMultiRowSelection="true">
                                                                                    <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                        <Columns>
                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                DataField="NomeFile" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("NomeFile")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                                                HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red">
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


                                    <asp:Panel Width="100%" runat="server" ID="BandiGareContrattiPanel" Visible="false">
                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                border-top: 0px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
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
                                                            Height="260px">

                                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td class="ContainerMargin">
                                                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                        <tr>
                                                                                            <td style="width: 90px">
                                                                                                <asp:Label ID="ProponenteLabel" runat="server" CssClass="Etichetta" Text="Proponente *"
                                                                                                    ToolTip="Denominazione della Stazione Appaltante responsabile del procedimento di scelta del contraente" />
                                                                                            </td>
                                                                                            <td style="width: 530px">
                                                                                                <telerik:RadTextBox ID="DenominazioneStrutturaProponenteTextBox" runat="server" Skin="Office2007"
                                                                                                    ToolTip="Denominazione della Stazione Appaltante responsabile del procedimento di scelta del contraente"
                                                                                                    Width="525px" MaxLength="250" />
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
                                                                                            <td style="width: 90px">
                                                                                                <asp:Label ID="OggettoBandoGaraLabel" runat="server" CssClass="Etichetta" Text="Oggetto *" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <telerik:RadTextBox ID="OggettoBandoGaraTextBox" runat="server" Skin="Office2007"
                                                                                                    Width="775px" Rows="2" TextMode="MultiLine" ToolTip="Oggetto dell'appalto" />
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
                                                                                                                <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx" />
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
                                                                                                <telerik:RadListBox ID="PartecipantiListBox" runat="server" Skin="Office2007" Height="80px" CheckBoxes="True" style=" width:100%" />
                                                                                               
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
                                                                                                <telerik:RadListBox ID="AggiudicatariListBox" runat="server" Skin="Office2007" Height="80px"  CheckBoxes="True" style=" width:100%" />
                                                                                              
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
                                                                                                             ToolTip ="Importo di aggiudicazione al lordo degli oneri di sicurezza ed al netto dell’IVA"   Width="120px" MaxLength="10" />
                                                                                                        </td>
                                                                                                        <td style="width: 130px; text-align: center;">
                                                                                                            <asp:Label ID="ImportoLiquidatoLabel" runat="server" CssClass="Etichetta" Text="Importo Liquid. *" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadNumericTextBox ID="ImportoLiquidatoTextBox" runat="server" Skin="Office2007"
                                                                                                              ToolTip = "Importo liquidato al netto dell’IVA"  Width="120px" MaxLength="10" />
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
                                                                                                                 <Calendar runat="server">
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
                                                                                                                <Calendar  runat="server">
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
                                                            Height="260px">
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
                                                                                    <td style="vertical-align: bottom">
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
                                                            <div id="GrigliaAllegatiBandoGaraPanel" runat="server" style="padding: 2px 2px 2px 2px;">
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
                                                                                            ToolTip="Allega documento digitalizzato" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                                                ID="AggiungiDocumentoBandoGaraImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                ToolTip="Allega documento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
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
                                                                                                DataField="NomeFile" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("NomeFile")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                                                HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red">
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


                                    <asp:Panel Width="100%" runat="server" ID="CollaborazioneConsulenzaPanel" Visible="false">

                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>

                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                border-top: 0px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 100%">
                                                                                        &nbsp;<asp:Label ID="DatiIncaricoConsulenzaLabel" runat="server" Style="color: #00156E"
                                                                                            Font-Bold="True" CssClass="Etichetta" Text="Dati Incarico Consulenza/Collaborazione" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <telerik:RadTabStrip runat="server" ID="CompensoConsulenzaTabStrip" SelectedIndex="0"
                                                        MultiPageID="CompensoConsulenzaMultiPage" Skin="Office2007" Width="100%">
                                                        <Tabs>
                                                            <telerik:RadTab Text="Generale" Selected="True" />
                                                            <telerik:RadTab Text="Dichiarazioni" />
                                                            <telerik:RadTab Text="Documenti" />
                                                        </Tabs>
                                                    </telerik:RadTabStrip>
                                                    
                                                    <telerik:RadMultiPage runat="server" ID="CompensoConsulenzaMultiPage" SelectedIndex="0"
                                                        Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                                      
                                                        <telerik:RadPageView runat="server" ID="CompensoConsulenzaPageView" CssClass="corporatePageView"
                                                            Height="260px">

                                                            
                                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td class="ContainerMargin">
                                                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                                            style="height: 250px">
                                                                            <tr style="vertical-align: top">
                                                                                <td>
                                                                                    <table style="width: 100%">

                                                                                        <tr>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 90px">
                                                                                                            <asp:Label ID="BeneficiarioConsulenzaLabel" runat="server" CssClass="Etichetta" Text="Titolare *" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadComboBox ID="BeneficiarioConsulenzaComboBox" runat="server" Width="100%"
                                                                                                                Height="150" EmptyMessage="Seleziona Titolare" EnableAutomaticLoadOnDemand="True"
                                                                                                                ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                                                                                Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                                                                                                <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx" />
                                                                                                            </telerik:RadComboBox>
                                                                                                        </td>
                                                                                                        <td style="width: 30px; text-align: center">
                                                                                                            <asp:ImageButton ID="TrovaBeneficiarioConsulenzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                ToolTip="Seleziona Beneficiario..." ImageAlign="AbsMiddle" Style="height: 16px" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="AggiornaBeneficiarioConsulenzaImageButton" runat="server" Style="display: none" />
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
                                                                                                            <asp:Label ID="DenominazioneConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Denominaz. *" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadTextBox ID="DenominazioneConsulenzaTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="100%" Rows="2" TextMode="MultiLine" ToolTip="Denominazione dell'incarico" />
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
                                                                                                            <asp:Label ID="RagioneIncaricoLabel" runat="server" CssClass="Etichetta" Text="Ragione *" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadTextBox ID="RagioneIncaricoConsulenzaTextBox" runat="server" Rows="2"
                                                                                                                Skin="Office2007" TextMode="MultiLine" ToolTip="Ragione dell'incarico" Width="100%"
                                                                                                                MaxLength="1500" />
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
                                                                                                         <asp:Label ID="DataIncaricoConsulenzaLabel" runat="server" CssClass="Etichetta" Text="Durata Da *" />
                                                                                                        </td>
                                                                                                        <td style="width: 105px">
                                                                                                         <telerik:RadDatePicker ID="DataInizioIncaricoConsulenzaTextBox" runat="server" MinDate="1753-01-01"
                                                                                                                Skin="Office2007" ToolTip="Data inizio incarico consulenza" Width="100px">
                                                                                                                <Calendar   runat="server">
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
                                                                                                        <td style="width: 105px">
                                                                                                         <telerik:RadDatePicker ID="DataFineIncaricoConsulenzaTextBox" runat="server" MinDate="1753-01-01"
                                                                                                                Skin="Office2007" ToolTip="Data fine incarico consulenza" Width="100px">
                                                                                                                <Calendar runat="server">
                                                                                                                    <SpecialDays>
                                                                                                                        <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                                                                    </SpecialDays>
                                                                                                                </Calendar>
                                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                            </telerik:RadDatePicker>
                                                                                                        </td>
                                                                                                        <td style="width: 90px; text-align: center">
                                                                                                          <asp:Label ID="CompensoConsulenzaLabel" runat="server" CssClass="Etichetta" Text="Compensi *" />
                                                                                                        </td>
                                                                                                        <td style="width: 180px">
                                                                                                         <telerik:RadTextBox ID="CompensoConsulenzaTextBox" runat="server" MaxLength="1500"
                                                                                                                Skin="Office2007" ToolTip="Importo previsto da corrispondere" Width="180px" />
                                                                                                        </td>
                                                                                                        <td style="width: 65px; text-align: center">
                                                                                                           <asp:Label ID="VariabileCompensoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Variabili" />
                                                                                                        </td>
                                                                                                        <td style="text-align: right">
                                                                                                          <telerik:RadTextBox ID="VariabileCompensoConsulenzaTextBox" runat="server" MaxLength="1500"
                                                                                                                Skin="Office2007" ToolTip="Compensi variabili da corrispondere" Width="170px" />
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
                                                                                                          <asp:Label ID="AltreCaricheLabel" runat="server" CssClass="Etichetta" Text="Cariche" />
                                                                                                        </td>
                                                                                                        <td style="width: 345px">
                                                                                                          <telerik:RadTextBox ID="altreCaricheTextBox" ToolTip="Altre Cariche" runat="server"
                                                                                                                MaxLength="1500" Skin="Office2007" Width="340px" />
                                                                                                        </td>
                                                                                                        <td style="width: 65px; text-align: center">
                                                                                                          <asp:Label ID="AltriIncarichiLabel" runat="server" CssClass="Etichetta" Text="Incarichi " />
                                                                                                        </td>
                                                                                                        <td style="text-align: right">
                                                                                                          <telerik:RadTextBox ToolTip="Altri Incarichi" ID="altriIncarichiTextBox" runat="server"
                                                                                                                MaxLength="1500" Skin="Office2007" Width="340px" />
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
                                                                                                          <asp:Label ID="AltreAttivitaProfessionaliLabel" runat="server" CssClass="Etichetta" Text="Altre Attività" />
                                                                                                        </td>
                                                                                                        <td style="width: 390px">
                                                                                                          <telerik:RadTextBox ID="altreAttivitaProfessionaliTextBox" runat="server" MaxLength="1500"
                                                                                                                Skin="Office2007" Width="390px" />
                                                                                                        </td>
                                                                                                        <td style="width: 65px; text-align: center">
                                                                                                            <asp:Label ID="NumeroAttoIncaricoConsulenzaCollaborazioneLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="N. Atto" ToolTip="Numero Atto" />
                                                                                                        </td>
                                                                                                        <td style="width: 120px">
                                                                                                            <telerik:RadNumericTextBox ID="NumeroAttoIncaricoConsulenzaCollaborazioneTextBox" runat="server"
                                                                                                                Skin="Office2007" Width="100px" DataType="System.Int32" MaxLength="8" MaxValue="99999999"
                                                                                                                MinValue="0" ShowSpinButtons="True">
                                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                            </telerik:RadNumericTextBox>
                                                                                                        </td>
                                                                                                        <td style="width: 60px; text-align: center;">
                                                                                                            <asp:Label ID="DataAttoIncaricoConsulenzaCollaborazioneLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Data Atto" />
                                                                                                        </td>
                                                                                                        <td style="text-align: right">
                                                                                                            <telerik:RadDatePicker ID="DataAttoIncaricoConsulenzaCollaborazioneTextBox" runat="server"
                                                                                                                MinDate="1753-01-01" Skin="Office2007" ToolTip="Data atto" Width="100px">
                                                                                                                <Calendar  runat="server">
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

                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                                     

                                                        </telerik:RadPageView>

                                                          <telerik:RadPageView runat="server" ID="DichiarazioniConsulenzaCollaborazionePageView" CssClass="corporatePageView" Height="260px">

                                                              <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                  <tr>
                                                                      <td class="ContainerMargin">
                                                                          <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                                              style="height: 250px">
                                                                              <tr style="vertical-align: top">
                                                                                  <td>
                                                                                   <table style="width: 100%">
                                                                                     <tr>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>

                                                                                                        <td style="width: 50%">
                                                                                                            <table style="width: 100%">
                                                                                                                <tr>
                                                                                                                    <td style="width: 109px;">
                                                                                                                        <asp:Label ID="CurriculumLabel" runat="server" CssClass="Etichetta" Text="Curriculum" />
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <div id="curriculumUpload1" runat="server" visible="true">
                                                                                                                            <table style="width: 100%; background-color:#DFE8F6;border: 1px solid #ABC1DE">
                                                                                                                                 <tr style="height:30px">
                                                                                                                                    <td style="padding-top:6px">
                                                                                                                                        <telerik:RadAsyncUpload ID="CurriculumUpload" runat="server" MaxFileInputsCount="1"
                                                                                                                                            Width="100%" OnClientFileSelected="CurriculumSelezionato" OnClientFileUploadRemoved="CurriculumRimosso"
                                                                                                                                            Skin="Office2007" InputSize="35" EnableViewState="True">
                                                                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                                                        </telerik:RadAsyncUpload>
                                                                                                                                    </td>
                                                                                                                                     <td style= "padding-top:6px;width: 30px; text-align: center">
                                                                                                                                        <div id="divAggCurriculum" style="display: none;">
                                                                                                                                            <asp:ImageButton ID="AggiungiCurriculumImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                                                                ToolTip="Allega Curriculum" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                        <div id="curriculumUpload2" runat="server" style="width: 100%" visible="false">
                                                                                                                            <table style="width: 100%;background-color:#DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                                <tr style="height:34px">
                                                                                                                                    <td>
                                                                                                                                        <asp:LinkButton ID="CurriculumAllegatoLinkButton" ForeColor="Blue" CssClass="Etichetta"
                                                                                                                                            runat="server"  />
                                                                                                                                    </td>
                                                                                                                                    <td style="width: 30px; text-align: center">
                                                                                                                                        <asp:ImageButton ID="RimuoviCurriculumImageButton" runat="server" ImageUrl="~/images//Delete16.png"
                                                                                                                                            ToolTip="Rimuovi Curriculum" ImageAlign="AbsMiddle" BorderStyle="None" Width="16px" />
                                                                                                                                        <asp:Label ID="NomeFileCurriculumLabel" runat="server" Visible="false"  />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>

                                                                                                        <td style="width: 50%">
                                                                                                            <table style="width: 100%">
                                                                                                                <tr>
                                                                                                                    <td style="width: 110px; text-align:center">
                                                                                                                        <asp:Label ID="InconsistenzaLabel" runat="server" CssClass="Etichetta" Text="Insussistenza"
                                                                                                                            ToolTip="Dichiarazione Insussistenza" />
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <div id="InconsistenzaUpload1" runat="server" visible="true">
                                                                                                                             <table style="width: 100%; background-color:#DFE8F6;border: 1px solid #ABC1DE">
                                                                                                                               <tr style="height:30px">
                                                                                                                                   <td style= "padding-top:6px">
                                                                                                                                        <telerik:RadAsyncUpload ID="InconsistenzaUpload" runat="server" MaxFileInputsCount="1"
                                                                                                                                            Width="100%" OnClientFileSelected="InconsistenzaSelezionato" OnClientFileUploadRemoved="InconsistenzaRimosso"
                                                                                                                                            Skin="Office2007" InputSize="35" EnableViewState="True">
                                                                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                                                        </telerik:RadAsyncUpload>
                                                                                                                                    </td>
                                                                                                                                    <td style= "padding-top:6px;width: 30px; text-align: center">
                                                                                                                                        <div id="divAggInconsistenza" style="display: none;">
                                                                                                                                            <asp:ImageButton ID="AggiungiInconsistenzaImageButton" runat="server" ImageUrl="~/images/add16.png"
                                                                                                                                                ToolTip="Allega Dichiarazione" ImageAlign="AbsMiddle" BorderStyle="None" /></div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>

                                                                                                                        <div id="InconsistenzaUpload2" runat="server" style="width: 100%" visible="false">
                                                                                                                            <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                                <tr style="height: 34px">
                                                                                                                                    <td>
                                                                                                                                        <asp:LinkButton ID="InconsistenzaAllegatoLinkButton" ForeColor="Blue" CssClass="Etichetta"
                                                                                                                                            runat="server" />
                                                                                                                                    </td>
                                                                                                                                    <td style="width: 30px; text-align: center">
                                                                                                                                        <asp:ImageButton ID="RimuoviInconsistenzaImageButton" runat="server" ImageUrl="~/images/Delete16.png"
                                                                                                                                            ToolTip="Rimuovi Dichiarazione" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                                        <asp:Label ID="NomeFileInsussistenzaLabel" runat="server" Visible="false" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
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
                                                                      </td>
                                                                  </tr>
                                                              </table>


                                                        </telerik:RadPageView>

                                                        <telerik:RadPageView runat="server" ID="AllegatiCompensoConsulenzaPageView" CssClass="corporatePageView"
                                                            Height="260px">
                                                            <div id="AllegatiCompensoConsulenzaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr style="height: 30px">
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="DescrizioneDocumentoCompensoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Descrizione" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="DescrizioneDocumentoCompensoConsulenzaTextBox" runat="server"
                                                                                            Skin="Office2007" Width="250px" />
                                                                                    </td>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="NomeFileDocumentoCompensoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Nome file" />
                                                                                    </td>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <telerik:RadAsyncUpload ID="AllegatoCompensoConsulenzaUpload" runat="server" MaxFileInputsCount="1"
                                                                                            Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                        </telerik:RadAsyncUpload>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="TipoAllegatoCompensoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Tipo" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioCompensoConsulenzaRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                        <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoCompensoConsulenzaRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />
                                                                                    </td>
                                                                                    <td style="width: 140px">
                                                                                        <asp:Label ID="OpzioniScannerCompensoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Opzioni scanner" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="FronteRetroCompensoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Fronte retro" />&nbsp;<asp:CheckBox ID="FronteRetroCompensoConsulenzaCheckBox"
                                                                                                runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label ID="VisualizzaUICompensoConsulenzaLabel"
                                                                                                    runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                                        ID="VisualizzaUICompensoConsulenzaCheckBox" runat="server" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="GrigliaAllegatiCompensoConsulenzaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td style="height: 20px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="DocumentiCompensoConsulenzaLabel" runat="server" CssClass="Etichetta"
                                                                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                                                                            Text="Allegati" />
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:ImageButton ID="ScansionaCompensoConsulenzaImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                                            ToolTip="Allega documento digitalizzato" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                                                ID="AggiungiDocumentoCompensoConsulenzaImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                ToolTip="Allega documento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                                                    ID="ScanUploadCompensoConsulenzaButton" Style="display: none" runat="server"
                                                                                                    ImageUrl="~/images//RecycleEmpty.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: #FFFFFF">
                                                                        <td>
                                                                            <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                <telerik:RadGrid ID="AllegatiCompensoConsulenzaGridView" runat="server" ToolTip="Elenco allegati associati alla pubblicazione"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Culture="it-IT" AllowMultiRowSelection="true">
                                                                                    <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                        <Columns>
                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                DataField="NomeFile" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("NomeFile")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                                                HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red">
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


                                    <asp:Panel Width="100%" runat="server" ID="IncaricoPanel" Visible="false">

                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>

                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                border-top: 0px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 100%">
                                                                                        &nbsp;<asp:Label ID="DatiIncaricoLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                            CssClass="Etichetta" Text="Dati Incarico Dipendente" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <telerik:RadTabStrip runat="server" ID="IncaricoDipendenteTabStrip" SelectedIndex="0"
                                                        MultiPageID="IncaricoDipendenteMultiPage" Skin="Office2007" Width="100%">
                                                        <Tabs>
                                                            <telerik:RadTab Text="Generale" Selected="True" />
                                                            <telerik:RadTab Text="Documenti" />
                                                        </Tabs>
                                                    </telerik:RadTabStrip>
                                                    
                                                    <telerik:RadMultiPage runat="server" ID="IncaricoDipendenteMultiPage" SelectedIndex="0"
                                                        Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                                       
                                                        <telerik:RadPageView runat="server" ID="IncaricoDipendentePageView" CssClass="corporatePageView"
                                                            Height="260px">

                                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                               
                                                                <tr>
                                                                    <td class="ContainerMargin">
                                                                     
                                                                   
                                                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0" style=" height:250px">
                                                                            
                                                                            
                                                                             <tr style="vertical-align: top">
                                                                                <td>
                                                                                    <table style=" width:100%">

                                                                                        <tr>
                                                                                            <td>
                                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 100px">
                                                                                                            <asp:Label ID="BeneficiarioIncaricoLabel" runat="server" CssClass="Etichetta" Text="Dipendente *" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadComboBox ID="BeneficiarioIncaricoComboBox" runat="server" Width="100%"
                                                                                                                Height="150" EmptyMessage="Seleziona Dipendente" EnableAutomaticLoadOnDemand="True"
                                                                                                                ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                                                                                Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                                                                                                <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx" />
                                                                                                            </telerik:RadComboBox>
                                                                                                        </td>
                                                                                                        <td style="width: 30px; text-align: center">
                                                                                                            <asp:ImageButton ID="TrovaBeneficiarioIncaricoDipendenteImageButton" runat="server"
                                                                                                                ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona Beneficiario..." ImageAlign="AbsMiddle"
                                                                                                                Style="height: 16px" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="AggiornaBeneficiarioIncaricoDipendenteImageButton" runat="server"
                                                                                                                Style="display: none" />
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
                                                                                                            <asp:Label ID="OggettoIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Oggetto *" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadTextBox ID="OggettoIncaricoDipendenteTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="100%" Rows="3" TextMode="MultiLine" ToolTip="Oggetto" MaxLength="1500" />
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
                                                                                                            <asp:Label ID="RagioneIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Ragione" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadTextBox ID="RagioneIncaricoDipendenteTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="100%" Rows="3" TextMode="MultiLine" ToolTip="ragione" MaxLength="1500" />
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
                                                                                                            <asp:Label ID="CompensoIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Compenso *" />
                                                                                                        </td>
                                                                                                        <td style="width: 100px">
                                                                                                            <telerik:RadNumericTextBox ID="CompensoIncaricoDipendenteTextBox" runat="server"
                                                                                                                Skin="Office2007" Width="70px" MaxLength="10" />
                                                                                                        </td>
                                                                                                        <td style="width: 115px">
                                                                                                            <asp:Label ID="DurataIncaricoLabel" runat="server" CssClass="Etichetta" Text="Durata Incarico" />
                                                                                                        </td>
                                                                                                        <td style="width: 40px">
                                                                                                            <asp:Label ID="DataInizioIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Da *" />
                                                                                                        </td>
                                                                                                        <td style="width: 130px">
                                                                                                            <telerik:RadDatePicker ID="DataInizioIncaricoDipendenteTextBox" Skin="Office2007"
                                                                                                                Width="110px" runat="server" MinDate="1753-01-01">
                                                                                                                <Calendar  runat="server">
                                                                                                                    <SpecialDays>
                                                                                                                        <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                                    </SpecialDays>
                                                                                                                </Calendar>
                                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                            </telerik:RadDatePicker>
                                                                                                        </td>
                                                                                                        <td style="width: 35px; text-align: center">
                                                                                                            <asp:Label ID="DataFineIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="A *" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadDatePicker ID="DataFineIncaricoDipendenteTextBox" Skin="Office2007" Width="110px"
                                                                                                                runat="server" MinDate="1753-01-01">
                                                                                                                <Calendar  runat="server">
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
                                                        </telerik:RadPageView>

                                                        <telerik:RadPageView runat="server" ID="AllegatiIncaricoDipendentePageView" CssClass="corporatePageView"
                                                            Height="260px">

                                                            <div id="AllegatiIncaricoDipendentePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr style="height: 30px">
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="DescrizioneDocumentoIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Descrizione" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="DescrizioneDocumentoIncaricoDipendenteTextBox" runat="server"
                                                                                            Skin="Office2007" Width="250px" />
                                                                                    </td>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="NomeFileDocumentoIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Nome file" />
                                                                                    </td>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <telerik:RadAsyncUpload ID="AllegatoIncaricoDipendenteUpload" runat="server" MaxFileInputsCount="1"
                                                                                            Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                        </telerik:RadAsyncUpload>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="TipoAllegatoIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Tipo" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioIncaricoDipendenteRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                        <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoIncaricoDipendenteRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />
                                                                                    </td>
                                                                                    <td style="width: 140px">
                                                                                        <asp:Label ID="OpzioniScannerIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Opzioni scanner" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="FronteRetroIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Fronte retro" />&nbsp;<asp:CheckBox ID="FronteRetroIncaricoDipendenteCheckBox"
                                                                                                runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label ID="VisualizzaUIIncaricoDipendenteLabel"
                                                                                                    runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                                        ID="VisualizzaUIIncaricoDipendenteCheckBox" runat="server" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="GrigliaAllegatiIncaricoDipendentePanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td style="height: 20px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="DocumentiIncaricoDipendenteLabel" runat="server" CssClass="Etichetta"
                                                                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                                                                            Text="Allegati" />
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:ImageButton ID="ScansionaIncaricoDipendenteImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                                            ToolTip="Allega documento digitalizzato" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                                                ID="AggiungiDocumentoIncaricoDipendenteImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                ToolTip="Allega documento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                                                    ID="ScanUploadIncaricoDipendenteButton" Style="display: none" runat="server"
                                                                                                    ImageUrl="~/images//RecycleEmpty.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: #FFFFFF">
                                                                        <td>
                                                                            <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                <telerik:RadGrid ID="AllegatiIncaricoDipendenteGridView" runat="server" ToolTip="Elenco allegati associati alla pubblicazione"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Culture="it-IT" AllowMultiRowSelection="true">
                                                                                    <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                        <Columns>
                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                DataField="NomeFile" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("NomeFile")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                                                HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red">
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

                                  
                                    <asp:Panel Width="100%" runat="server" ID="PubblicazioneGenericaPanel" Visible="false">

                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                border-top: 0px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 100%">
                                                                                        &nbsp;<asp:Label ID="DatiPubblicazioneGenericaLabel" runat="server" Style="color: #00156E"
                                                                                            Font-Bold="True" CssClass="Etichetta" Text="Dati Pubblicazione Generica" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <telerik:RadTabStrip runat="server" ID="PubblicazioneGenericaTabStrip" SelectedIndex="0"
                                                        MultiPageID="PubblicazioneGenericaMultiPage" Skin="Office2007" Width="100%">
                                                        <Tabs>
                                                            <telerik:RadTab Text="Generale" Selected="True" />
                                                            <telerik:RadTab Text="Documenti" />
                                                        </Tabs>
                                                    </telerik:RadTabStrip>
                                                    
                                                    <telerik:RadMultiPage runat="server" ID="PubblicazioneGenericaMultiPage" SelectedIndex="0"
                                                        Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                                        <telerik:RadPageView runat="server" ID="PubblicazioneGenericaPageView" CssClass="corporatePageView"
                                                            Height="260px">

                                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td class="ContainerMargin">
                                                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                                            style="height: 250px">
                                                                            <tr style="vertical-align: top">
                                                                                <td>
                                                                                    <table style="width: 100%">

                                                                                        <tr>
                                                                                            <td style="width: 100px">
                                                                                                <asp:Label ID="TitoloPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                                    Text="Titolo *" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <telerik:RadTextBox ID="PubblicazioneTitoloTextBox" runat="server" Skin="Office2007"
                                                                                                    Width="760px" ToolTip="Titolo" MaxLength="3000" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td style="width: 100px">
                                                                                                <asp:Label ID="ContenutoPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                                    Text="Contenuto *" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <telerik:RadTextBox ID="PubblicazioneContenutoTextBox" runat="server" Skin="Office2007"
                                                                                                    Width="760px" Rows="4" TextMode="MultiLine" ToolTip="Contenuto" MaxLength="1500"
                                                                                                    Style="overflow-x: hidden" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td style="width: 100px">
                                                                                                <asp:Label ID="NumeroPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                                    Text="Numero" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 100px">
                                                                                                            <telerik:RadNumericTextBox ID="PubblicazioneNumeroTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="90px" DataType="System.Int32" MaxLength="6" MaxValue="9999" MinValue="0"
                                                                                                                ShowSpinButtons="True" ToolTip="Numero">
                                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                            </telerik:RadNumericTextBox>
                                                                                                        </td>
                                                                                                        <td style="width: 120px; text-align: center">
                                                                                                            <asp:Label ID="PubblicazioneInizioRiferimentoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Riferimento Dal" />
                                                                                                        </td>
                                                                                                        <td style="width: 110px">
                                                                                                            <telerik:RadDatePicker ID="PubblicazioneInizioRiferimentoTextBox" runat="server"
                                                                                                                MinDate="1753-01-01" Skin="Office2007" ToolTip="Data inizio riferimento" Width="110px">
                                                                                                                <Calendar  runat="server">
                                                                                                                    <SpecialDays>
                                                                                                                        <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                                                                    </SpecialDays>
                                                                                                                </Calendar>
                                                                                                                <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                            </telerik:RadDatePicker>
                                                                                                        </td>
                                                                                                        <td style="width: 40px; text-align: center">
                                                                                                            <asp:Label ID="PubblicazioneFineRiferimentoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Al" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadDatePicker ID="PubblicazioneFineRiferimentoTextBox" runat="server" MinDate="1753-01-01"
                                                                                                                Skin="Office2007" ToolTip="Data fine riferimento" Width="110px">
                                                                                                                <Calendar  runat="server">
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

                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>


                                                        </telerik:RadPageView>

                                                        <telerik:RadPageView runat="server" ID="AllegatiPubblicazioneGenericaPageView" CssClass="corporatePageView"
                                                            Height="260px">
                                                            <div id="AllegatiPubblicazioneGenericaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr style="height: 30px">
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="DescrizioneDocumentoPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Descrizione" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="DescrizioneDocumentoPubblicazioneGenericaTextBox" runat="server"
                                                                                            Skin="Office2007" Width="250px" />
                                                                                    </td>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="NomeFileDocumentoPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Nome file" />
                                                                                    </td>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <telerik:RadAsyncUpload ID="AllegatoPubblicazioneGenericaUpload" runat="server" MaxFileInputsCount="1"
                                                                                            Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                        </telerik:RadAsyncUpload>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="TipoAllegatoPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Tipo" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioPubblicazioneGenericaRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                        <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoPubblicazioneGenericaRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />
                                                                                    </td>
                                                                                    <td style="width: 140px">
                                                                                        <asp:Label ID="OpzioniScannerPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Opzioni scanner" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="FronteRetroPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Fronte retro" />&nbsp;<asp:CheckBox ID="FronteRetroPubblicazioneGenericaCheckBox"
                                                                                                runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label ID="VisualizzaUIPubblicazioneGenericaLabel"
                                                                                                    runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                                        ID="VisualizzaUIPubblicazioneGenericaCheckBox" runat="server" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="GrigliaAllegatiPubblicazioneGenericaPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td style="height: 20px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="DocumentiPubblicazioneGenericaLabel" runat="server" CssClass="Etichetta"
                                                                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                                                                            Text="Allegati" />
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:ImageButton ID="ScansionaPubblicazioneGenericaImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                                            ToolTip="Allega documento digitalizzato" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                                                ID="AggiungiDocumentoPubblicazioneGenericaImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                ToolTip="Allega documento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                                                    ID="ScanUploadPubblicazioneGenericaButton" Style="display: none" runat="server"
                                                                                                    ImageUrl="~/images//RecycleEmpty.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: #FFFFFF">
                                                                        <td>
                                                                            <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                <telerik:RadGrid ID="AllegatiPubblicazioneGenericaGridView" runat="server" ToolTip="Elenco allegati associati alla pubblicazione"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Culture="it-IT" AllowMultiRowSelection="true">
                                                                                    <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                        <Columns>
                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                DataField="NomeFile" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("NomeFile")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                                                HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red">
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


                                     <asp:Panel Width="100%" runat="server" ID="IncarichiAmministrativiDirigenzialiPanel" Visible="false">

                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>

                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                border-top: 0px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <tr>

                                                                                    <td style="width: 100%">
                                                                                        &nbsp;<asp:Label ID="DatiIncarichiAmministrativiDirigenzialiLabel" runat="server" Style="color: #00156E"
                                                                                            Font-Bold="True" CssClass="Etichetta" Text="Dati Incarico" />
                                                                                    </td>


                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <telerik:RadTabStrip runat="server" ID="IncarichiAmministrativiDirigenzialiTabStrip" SelectedIndex="0"
                                                        MultiPageID="IncarichiAmministrativiDirigenzialiMultiPage" Skin="Office2007" Width="100%">
                                                        <Tabs>
                                                            <telerik:RadTab Text="Generale" Selected="True" />
                                                            <telerik:RadTab Text="Dichiarazioni" />
                                                            <telerik:RadTab Text="Documenti" />
                                                        </Tabs>
                                                    </telerik:RadTabStrip>
                                                   
                                                    <telerik:RadMultiPage runat="server" ID="IncarichiAmministrativiDirigenzialiMultiPage" SelectedIndex="0"
                                                        Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                                        
                                                       <telerik:RadPageView runat="server" ID="IncarichiAmministrativiDirigenzialiPageView" CssClass="corporatePageView"
                                                            Height="260px">


                                                           <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                               <tr>
                                                                   <td class="ContainerMargin">
                                                                       <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                                           style="height: 250px">
                                                                           <tr style="vertical-align: top">
                                                                               <td>
                                                                                   <table style="width: 100%">

                                                                                       <tr>
                                                                                           <td>
                                                                                               <table style="width: 100%">
                                                                                                   <tr>
                                                                                                       <td style="width: 90px">
                                                                                                           <asp:Label ID="TitolareIncaricoLabel" runat="server" CssClass="Etichetta" Text="Titolare *" />
                                                                                                       </td>
                                                                                                       <td>
                                                                                                           <telerik:RadComboBox ID="TitolareIncaricoComboBox" runat="server" Width="100%" Height="150"
                                                                                                               EmptyMessage="Seleziona Titolare" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                                               ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007"
                                                                                                               LoadingMessage="Caricamento in corso...">
                                                                                                               <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx" />
                                                                                                           </telerik:RadComboBox>
                                                                                                       </td>
                                                                                                       <td style="width: 30px; text-align: center">
                                                                                                           <asp:ImageButton ID="TrovaTitolareIncaricoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                               ToolTip="Seleziona Titolare..." ImageAlign="AbsMiddle" />
                                                                                                       </td>
                                                                                                       <td>
                                                                                                           <asp:ImageButton ID="AggiornaTitolareIncaricoImageButton" runat="server" Style="display: none" />
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
                                                                                                           <asp:Label ID="DenominazioneIncaricoLabel" runat="server" CssClass="Etichetta" Text="Denominaz. *" />
                                                                                                       </td>
                                                                                                       <td>
                                                                                                           <telerik:RadTextBox ID="DenominazioneIncaricoAmministrativoTextBox" runat="server"
                                                                                                               Skin="Office2007" Width="100%" Rows="2" TextMode="MultiLine" Style="overflow-x: hidden"
                                                                                                               MaxLength="1500" />
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
                                                                                                           <asp:Label ID="RagioneIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Ragione *" />
                                                                                                       </td>
                                                                                                       <td>
                                                                                                           <telerik:RadTextBox ID="RagioneIncaricoAmministrativoTextBox" runat="server" Rows="2"
                                                                                                               Skin="Office2007" TextMode="MultiLine" ToolTip="Ragione dell'incarico" Width="100%"
                                                                                                               MaxLength="1500" Style="overflow-x: hidden" />
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
                                                                                                          <asp:Label ID="DataInizioIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Durata Da *" ToolTip="Data inizio incarico" />
                                                                                                       </td>

                                                                                                       <td style="width: 105px">
                                                                                                        <telerik:RadDatePicker ID="DataInizioIncaricoAmministrativoTextBox" runat="server"
                                                                                                               MinDate="1753-01-01" Skin="Office2007" ToolTip="Data inizio incarico" Width="100px">
                                                                                                               <Calendar  runat="server">
                                                                                                                   <SpecialDays>
                                                                                                                       <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                                                                   </SpecialDays>
                                                                                                               </Calendar>
                                                                                                               <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                           </telerik:RadDatePicker>
                                                                                                          
                                                                                                       </td>

                                                                                                       <td style="width: 30px; text-align: center;">
                                                                                                           <asp:Label ID="DataFineIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Al *" />
                                                                                                       </td>
                                                                                                        <td style="width: 105px">
                                                                                                           <telerik:RadDatePicker ID="DataFineIncaricoAmministrativoTextBox" runat="server"
                                                                                                               MinDate="1753-01-01" Skin="Office2007" ToolTip="Data fine incarico" Width="100px">
                                                                                                               <Calendar  runat="server">
                                                                                                                   <SpecialDays>
                                                                                                                       <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                                                                   </SpecialDays>
                                                                                                               </Calendar>
                                                                                                               <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                           </telerik:RadDatePicker>
                                                                                                       </td>


                                                                                                       <td style="width: 90px; text-align: center">
                                                                                                            <asp:Label ID="CompensoIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Compensi * " />
                                                                                                       </td>
                                                                                                       <td style="width: 180px">
                                                                                                           <telerik:RadTextBox ID="CompensoIncaricoAmministrativoTextBox" runat="server" MaxLength="1500"
                                                                                                               Skin="Office2007" ToolTip="Importo previsto da corrispondere" Width="180px" />
                                                                                                       </td>

                                                                                                      <td style="width: 65px; text-align: center">
                                                                                                         <asp:Label ID="CompensiVariabiliIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Variabili" />
                                                                                                       </td>
                                                                                                       <td style=" text-align:right"> 
                                                                                                         <telerik:RadTextBox ID="CompensiVariabiliIncaricoAmministrativoTextBox" runat="server"
                                                                                                               MaxLength="1500" Skin="Office2007" ToolTip="Compensi variabili da corrispondere"
                                                                                                               Width="170px" />
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
                                                                                                           <asp:Label ID="CaricheIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Cariche" />
                                                                                                       </td>
                                                                                                       <td style="width: 345px">
                                                                                                           <telerik:RadTextBox ID="CaricheIncaricoAmministrativoTextBox" ToolTip="Altre Cariche"
                                                                                                               runat="server" MaxLength="1500" Skin="Office2007" Width="340px" />
                                                                                                       </td>
                                                                                                       <td style="width: 65px; text-align: center">
                                                                                                           <asp:Label ID="AltriIncarichiIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Incarichi" />
                                                                                                       </td>
                                                                                                       <td style=" text-align:right">
                                                                                                           <telerik:RadTextBox ToolTip="Altri Incarichi" ID="AltriIncarichiIncaricoAmministrativoTextBox"
                                                                                                               runat="server" MaxLength="1500" Skin="Office2007" Width="340px" />
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
                                                                                                           <asp:Label ID="AltreAttivitaIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Altre Attività" />
                                                                                                       </td>
                                                                                                       <td style="width: 390px">
                                                                                                           <telerik:RadTextBox ID="AltreAttivitaIncaricoAmministrativoTextBox" runat="server"
                                                                                                               MaxLength="1500" Skin="Office2007" Width="390px" />
                                                                                                       </td>
                                                                                                       <td style="width: 65px; text-align: center">
                                                                                                           <asp:Label ID="NumeroAttoIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="N. Atto" ToolTip="Numero Atto" />
                                                                                                       </td>
                                                                                                        <td style="width: 120px">
                                                                                                           <telerik:RadNumericTextBox ID="NumeroAttoIncaricoAmministrativoTextBox" runat="server"
                                                                                                               Skin="Office2007" Width="100px" DataType="System.Int32" MaxLength="8" MaxValue="99999999"
                                                                                                               MinValue="0" ShowSpinButtons="True">
                                                                                                               <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                           </telerik:RadNumericTextBox>
                                                                                                       </td>
                                                                                                       <td style="width: 60px; text-align: center;">
                                                                                                            <asp:Label ID="DataAttoIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                               Text="Data Atto" />
                                                                                                       </td>
                                                                                                       <td style=" text-align:right">
                                                                                                           <telerik:RadDatePicker ID="DataAttoIncaricoAmministrativoTextBox" runat="server"
                                                                                                               MinDate="1753-01-01" Skin="Office2007" ToolTip="Data atto" Width="100px">
                                                                                                               <Calendar runat="server">
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

                                                                               

                                                                                    

                                                                                   </table>
                                                                               </td>
                                                                           </tr>
                                                                       </table>
                                                                   </td>
                                                               </tr>
                                                           </table>


                                                          
                                                        </telerik:RadPageView>


                                                        <telerik:RadPageView runat="server" ID="DichiarazioniIncarichiAmministrativiDirigenzialiPageView"
                                                            CssClass="corporatePageView" Height="260px">
                                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td class="ContainerMargin">
                                                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                                            style="height: 250px">
                                                                            <tr style="vertical-align: top">
                                                                                <td>
                                                                                    <table style="width: 100%">

                                                                                            <tr>
                                                                                           <td>
                                                                                               <table style="width: 100%">
                                                                                                   <tr>
                                                                                                       <td style="width: 50%">
                                                                                                         
                                                                                                               <table style="width: 100%">
                                                                                                                   <tr>
                                                                                                                       <td style="width: 90px">
                                                                                                                           <asp:Label ID="CurriculumIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta"
                                                                                                                               Text="Curriculum" />
                                                                                                                       </td>
                                                                                                                       <td>

                                                                                                                           <div id="curriculumincaricoAmministrativoUpload1" runat="server" visible="true">
                                                                                                                               <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                                   <tr style="height: 30px">
                                                                                                                                       <td style="padding-top: 6px">
                                                                                                                                           <telerik:RadAsyncUpload ID="CurriculumIncaricoAmministrativoUpload" runat="server"
                                                                                                                                               MaxFileInputsCount="1" OnClientFileSelected="CurriculumIncaricoAmministrativoSelezionato"
                                                                                                                                               OnClientFileUploadRemoved="CurriculumIncaricoAmministrativoRimosso" Skin="Office2007"
                                                                                                                                               Width="100%" InputSize="34" EnableViewState="True">
                                                                                                                                               <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                                                           </telerik:RadAsyncUpload>
                                                                                                                                       </td>
                                                                                                                                       <td style="padding-top: 6px; width: 30px; text-align: center">
                                                                                                                                           <div id="divAggIncaricoAmministrativoCurriculum" style="visibility: hidden;">
                                                                                                                                               <asp:ImageButton ID="AggiungiCurriculumIncaricoAmministrativoImageButton" runat="server"
                                                                                                                                                   ImageUrl="~/images//add16.png" ToolTip="Allega Curriculum" ImageAlign="AbsMiddle"
                                                                                                                                                   BorderStyle="None" />
                                                                                                                                           </div>
                                                                                                                                       </td>
                                                                                                                                   </tr>
                                                                                                                               </table>
                                                                                                                           </div>

                                                                                                                           <div id="curriculumincaricoAmministrativoUpload2" runat="server" visible="false">
                                                                                                                               <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                                   <tr style="height: 34px">
                                                                                                                                       <td>
                                                                                                                                           <asp:LinkButton ID="CurriculumIncaricoAmministrativoLinkButton" ForeColor="Blue"
                                                                                                                                               CssClass="Etichetta" runat="server" />
                                                                                                                                       </td>
                                                                                                                                       <td style="width: 30px; text-align: center">
                                                                                                                                           <asp:Label ID="NomeFileCurriculumIncaricoAmministrativoLabel" runat="server" Visible="false" />
                                                                                                                                           <asp:ImageButton ID="RimuoviCurriculumIncaricoAmministrativoImageButton" runat="server"
                                                                                                                                               ImageUrl="~/images//Delete16.png" ToolTip="Rimuovi Curriculum" ImageAlign="AbsMiddle"
                                                                                                                                               BorderStyle="None" />
                                                                                                                                       </td>
                                                                                                                                   </tr>
                                                                                                                               </table>
                                                                                                                           </div>

                                                                                                                       </td>

                                                                                                                       

                                                                                                                   </tr>
                                                                                                               </table>
                                                                                                          
                                                                                                           
                                                                                                       </td>

                                                                                                       <td style="width: 50%">

                                                                                                               <table style="width: 100%">
                                                                                                                   <tr>
                                                                                                                       <td style="width: 90px; text-align:center">
                                                                                                                           <asp:Label ID="InconferibilitaIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta" Text="Inconferibilità" />
                                                                                                                       </td>
                                                                                                                       <td>

                                                                                                                           <div id="inconferibilitaIncaricoAmministrativoUpload1" runat="server" visible="true">
                                                                                                                               <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                                   <tr style="height: 30px">
                                                                                                                                       <td style="padding-top: 6px">
                                                                                                                                           <telerik:RadAsyncUpload ID="InconferibilitaIncaricoAmministrativoUpload" runat="server"
                                                                                                                                               MaxFileInputsCount="1" OnClientFileSelected="InconferibilitaIncaricoAmministrativoSelezionato"
                                                                                                                                               OnClientFileUploadRemoved="InconferibilitaIncaricoAmministrativoRimosso" Skin="Office2007"
                                                                                                                                               Width="100%" InputSize="34" EnableViewState="True">
                                                                                                                                               <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                                                           </telerik:RadAsyncUpload>
                                                                                                                                       </td>
                                                                                                                                       <td style="padding-top: 6px; width: 30px; text-align: center">
                                                                                                                                           <div id="divAggIncaricoAmministrativoInconferibilita" style="visibility: hidden;">
                                                                                                                                               <asp:ImageButton ID="AggiungiInconferibilitaIncaricoAmministrativoImageButton" runat="server"
                                                                                                                                                   ImageUrl="~/images//add16.png" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                                           </div>
                                                                                                                                       </td>
                                                                                                                                   </tr>
                                                                                                                               </table>
                                                                                                                           </div>

                                                                                                                           <div id="InconferibilitaIncaricoAmministrativoUpload2" runat="server" visible="false">
                                                                                                                               <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                                   <tr style="height: 34px">
                                                                                                                                       <td>
                                                                                                                                           <asp:LinkButton ID="InconferibilitaIncaricoAmministrativoLinkButton" ForeColor="Blue"
                                                                                                                                               CssClass="Etichetta" runat="server" />
                                                                                                                                       </td>
                                                                                                                                       <td style="width: 30px; text-align: center">
                                                                                                                                           <asp:Label ID="NomeFileInconferibilitaIncaricoAmministrativoLabel" runat="server"
                                                                                                                                               Visible="false" />
                                                                                                                                           <asp:ImageButton ID="RimuoviInconferibilitaIncaricoAmministrativoImageButton" runat="server"
                                                                                                                                               ImageUrl="~/images//Delete16.png" ToolTip="Rimuovi Inconferibilita" ImageAlign="AbsMiddle"
                                                                                                                                               BorderStyle="None" />
                                                                                                                                       </td>
                                                                                                                                   </tr>
                                                                                                                               </table>
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
                                                                                           <td>
                                                                                               <table style="width: 100%">
                                                                                                   <tr>
                                                                                                       <td style="width: 50%">

                                                                                                          
                                                                                                               <table style="width: 100%">
                                                                                                                   <tr>
                                                                                                                       <td style="width: 90px">
                                                                                                                           <asp:Label ID="IncompatibilitaIncaricoAmministrativoLabel" runat="server" CssClass="Etichetta" Text="Incompatibilità" />
                                                                                                                       </td>
                                                                                                                       <td>

                                                                                                                           <div id="incompatibilitaIncaricoAmministrativoUpload1" runat="server" visible="true">
                                                                                                                               <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                                   <tr style="height: 30px">
                                                                                                                                       <td style="padding-top: 6px">
                                                                                                                                           <telerik:RadAsyncUpload ID="incompatibilitaIncaricoAmministrativoUpload" runat="server"
                                                                                                                                               MaxFileInputsCount="1" OnClientFileSelected="IncompatibilitaIncaricoAmministrativoSelezionato"
                                                                                                                                               OnClientFileUploadRemoved="IncompatibilitaIncaricoAmministrativoRimosso" Skin="Office2007"
                                                                                                                                               Width="100%" InputSize="34" EnableViewState="True">
                                                                                                                                               <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                                                           </telerik:RadAsyncUpload>
                                                                                                                                       </td>
                                                                                                                                       <td style="padding-top: 6px; width: 30px; text-align: center">
                                                                                                                                           <div id="divAggIncaricoAmministrativoIncompatibilita" style="visibility: hidden;">
                                                                                                                                               <asp:ImageButton ID="AggiungiIncompatibilitaIncaricoAmministrativoImageButton" runat="server"
                                                                                                                                                   ImageUrl="~/images//add16.png" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                                           </div>
                                                                                                                                       </td>
                                                                                                                                   </tr>
                                                                                                                               </table>
                                                                                                                           </div>
                                                                                                                           <div id="incompatibilitaIncaricoAmministrativoUpload2" runat="server" visible="false">
                                                                                                                               <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                                   <tr style="height: 34px">
                                                                                                                                       <td>
                                                                                                                                           <asp:LinkButton ID="IncompatibilitaIncaricoAmministrativoLinkButton" ForeColor="Blue"
                                                                                                                                               CssClass="Etichetta" runat="server" />
                                                                                                                                       </td>
                                                                                                                                       <td style="width: 30px; text-align: center">
                                                                                                                                           <asp:Label ID="NomeFileIncompatibilitaIncaricoAmministrativoLabel" runat="server"
                                                                                                                                               Visible="false" />
                                                                                                                                           <asp:ImageButton ID="RimuoviIncompatibilitaIncaricoAmministrativoImageButton" runat="server"
                                                                                                                                               ImageUrl="~/images//Delete16.png" ToolTip="Rimuovi Incompatibilità" ImageAlign="AbsMiddle"
                                                                                                                                               BorderStyle="None" />
                                                                                                                                       </td>
                                                                                                                                   </tr>
                                                                                                                               </table>
                                                                                                                           </div>

                                                                                                                           
                                                                                                                       </td>
                                                                                                                     
                                                                                                                   </tr>
                                                                                                               </table>
                                                                                                          
                                                                                                           
                                                                                                       </td>

                                                                                                        <td style="width: 50%">
                                                                                                           &nbsp;
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

                                                        <telerik:RadPageView runat="server" ID="AllegatiIncarichiAmministrativiDirigenzialiPageView" CssClass="corporatePageView"
                                                            Height="260px">
                                                            <div id="AllegatiIncarichiAmministrativiDirigenzialiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr style="height: 30px">
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="DescrizioneDocumentoIncarichiAmministrativiDirigenzialiLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Descrizione" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="DescrizioneDocumentoIncarichiAmministrativiDirigenzialiTextBox" runat="server"
                                                                                            Skin="Office2007" Width="250px" />
                                                                                    </td>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="NomeFileDocumentoIncarichiAmministrativiDirigenzialiLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Nome file" />
                                                                                    </td>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <telerik:RadAsyncUpload ID="AllegatoIncarichiAmministrativiDirigenzialiUpload" runat="server" MaxFileInputsCount="1"
                                                                                            Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                        </telerik:RadAsyncUpload>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="TipoAllegatoIncarichiAmministrativiDirigenzialiLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Tipo" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioIncarichiAmministrativiDirigenzialiRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                        <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoIncarichiAmministrativiDirigenzialiRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />
                                                                                    </td>
                                                                                    <td style="width: 140px">
                                                                                        <asp:Label ID="OpzioniScannerIncarichiAmministrativiDirigenzialiLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Opzioni scanner" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="FronteRetroIncarichiAmministrativiDirigenzialiLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Fronte retro" />&nbsp;<asp:CheckBox ID="FronteRetroIncarichiAmministrativiDirigenzialiCheckBox"
                                                                                                runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label ID="VisualizzaUIIncarichiAmministrativiDirigenzialiLabel"
                                                                                                    runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                                        ID="VisualizzaUIIncarichiAmministrativiDirigenzialiCheckBox" runat="server" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="GrigliaAllegatiIncarichiAmministrativiDirigenzialiPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td style="height: 20px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="DocumentiIncarichiAmministrativiDirigenzialiLabel" runat="server" CssClass="Etichetta"
                                                                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                                                                            Text="Allegati" />
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:ImageButton ID="ScansionaIncarichiAmministrativiDirigenzialiImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                                            ToolTip="Allega documento digitalizzato" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                                                ID="AggiungiDocumentoIncarichiAmministrativiDirigenzialiImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                ToolTip="Allega documento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                                                    ID="ScanUploadIncarichiAmministrativiDirigenzialiButton" Style="display: none" runat="server"
                                                                                                    ImageUrl="~/images//RecycleEmpty.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: #FFFFFF">
                                                                        <td>
                                                                            <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                <telerik:RadGrid ID="AllegatiIncarichiAmministrativiDirigenzialiGridView" runat="server" ToolTip="Elenco allegati associati alla pubblicazione"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Culture="it-IT" AllowMultiRowSelection="true">
                                                                                    <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                        <Columns>
                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                DataField="NomeFile" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("NomeFile")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                                                HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red">
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



                                     <asp:Panel Width="100%" runat="server" ID="BandoConcorsoPanel" Visible="false">

                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td>

                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                border-top: 0px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <tr>

                                                                                    <td style="width: 100%">
                                                                                        &nbsp;<asp:Label ID="DatiBandoConcorsoLabel" runat="server" Style="color: #00156E"
                                                                                            Font-Bold="True" CssClass="Etichetta" Text="Dati Bando Concorso"/>
                                                                                    </td>


                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <telerik:RadTabStrip runat="server" ID="BandoConcorsoTabStrip" SelectedIndex="0"
                                                        MultiPageID="BandoConcorsoMultiPage" Skin="Office2007" Width="100%">
                                                        <Tabs>
                                                            <telerik:RadTab Text="Generale" Selected="True" />
                                                            <telerik:RadTab Text="Documenti" />
                                                        </Tabs>
                                                    </telerik:RadTabStrip>
                                                    
                                                    <telerik:RadMultiPage runat="server" ID="BandoConcorsoMultiPage" SelectedIndex="0"
                                                        Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                                        
                                                       <telerik:RadPageView runat="server" ID="BandoConcorsoPageView" CssClass="corporatePageView"
                                                            Height="260px">

                                                           <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                               <tr>
                                                                   <td class="ContainerMargin">
                                                                       <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                          
                                                                           <tr>
                                                                               <td>
                                                                                   <table style="width: 100%">
                                                                                       <tr>
                                                                                           <td style=" width:70px">
                                                                                               <asp:Label  ID="BandoConcorsoOggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto *" />
                                                                                           </td>
                                                                                           <td>
                                                                                               <telerik:RadTextBox ID="BandoConcorsoOggettoTextBox" runat="server" Skin="Office2007"
                                                                                                   Width="100%" Rows="3" TextMode="MultiLine" ToolTip="Oggetto" MaxLength="1500"
                                                                                                   Style="overflow-x: hidden" />
                                                                                           </td>
                                                                                       </tr>
                                                                                   </table>
                                                                               </td>
                                                                           </tr>

                                                                           <tr>
                                                                               <td>
                                                                                  <table style="width: 100%">
                                                                                       <tr>
                                                                                          <td style=" width:70px">
                                                                                               <asp:Label  ID="BandoConcorsoProfiloLabel" runat="server" CssClass="Etichetta" Text="Profilo" />
                                                                                           </td>

                                                                                           <td style=" width:200px">
                                                                                               <telerik:RadTextBox ID="BandoConcorsoProfiloTextBox" runat="server" Skin="Office2007"
                                                                                                   Width="200px" ToolTip="Profilo" MaxLength="75" />
                                                                                           </td>

                                                                                          <td style=" width:120px; text-align:center">
                                                                                               <asp:Label ID="BandoConcorsoTipoAssunzioneLabel" runat="server" CssClass="Etichetta" Text="Tipo Assunzione" />
                                                                                           </td>

                                                                                         <td style=" width:200px">
                                                                                               <telerik:RadTextBox ID="BandoConcorsoTipoAssunzioneTextBox" runat="server" Skin="Office2007"
                                                                                                   Width="200px" ToolTip="tipo Assunzione" MaxLength="75" />
                                                                                           </td>

                                                                                           <td style=" width:80px; text-align:center">
                                                                                               <asp:Label ID="BandoConcorsoCategoriaLabel" runat="server" CssClass="Etichetta" Text="Categoria" />
                                                                                           </td>

                                                                                           <td style="text-align:right">
                                                                                               <telerik:RadTextBox ID="BandoConcorsoCategoriaTextBox" runat="server" Skin="Office2007"
                                                                                                   Width="205px" ToolTip="Categoria" MaxLength="75" />
                                                                                           </td>
                                                                                       </tr>
                                                                                   </table>
                                                                               </td>
                                                                           </tr>

                                                                           <tr>
                                                                               <td>
                                                                                    <table style="width: 100%">
                                                                                       <tr>
                                                                                           <td style=" width:70px">
                                                                                               <asp:Label  ID="BandoConcorsoSpesaLabel" runat="server" CssClass="Etichetta" Text="Spesa" />
                                                                                           </td>

                                                                                          <td style=" width:100px">
                                                                                               <telerik:RadNumericTextBox ID="BandoConcorsoSpesaTextBox" runat="server" Skin="Office2007"
                                                                                                   Width="100px" MaxLength="12" />
                                                                                           </td>

                                                                                           <td style=" width:170px; text-align:center">
                                                                                               <asp:Label ID="BandoConcorsoNumeroDipendentiAssuntiLabel" runat="server" CssClass="Etichetta" Text="Num. Dipendenti Assunti" />
                                                                                           </td>

                                                                                          <td style=" width:90px">
                                                                                               <telerik:RadNumericTextBox ID="BandoConcorsoNumeroDipendentiAssuntiTextBox" runat="server"
                                                                                                   Skin="Office2007" Width="90px" DataType="System.Int32" MaxLength="4" MaxValue="9999"
                                                                                                   MinValue="0" ShowSpinButtons="True" ToolTip="Numero Dipendenti">
                                                                                                   <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                               </telerik:RadNumericTextBox>
                                                                                           </td>

                                                                                          <td style=" width:130px; text-align:center">
                                                                                               <asp:Label ID="BandoConcorsoEstremiDocumentiLabel" runat="server" CssClass="Etichetta" Text="Estremi Documenti"
                                                                                                   ToolTip="Estremi Principali Documenti" />
                                                                                           </td>

                                                                                          <td style="text-align:right">
                                                                                               <telerik:RadTextBox ID="BandoConcorsoEstremiDocumentiTextBox" runat="server" Skin="Office2007"
                                                                                                   Width="315px" MaxLength="75" />
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

                                                        <telerik:RadPageView runat="server" ID="AllegatiBandoConcorsoPageView" CssClass="corporatePageView"
                                                            Height="260px">
                                                            <div id="AllegatiBandoConcorsoPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr style="height: 30px">
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="DescrizioneDocumentoBandoConcorsoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Descrizione" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="DescrizioneDocumentoBandoConcorsoTextBox" runat="server"
                                                                                            Skin="Office2007" Width="250px" />
                                                                                    </td>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="NomeFileDocumentoBandoConcorsoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Nome file" />
                                                                                    </td>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <telerik:RadAsyncUpload ID="AllegatoBandoConcorsoUpload" runat="server" MaxFileInputsCount="1"
                                                                                            Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                        </telerik:RadAsyncUpload>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="TipoAllegatoBandoConcorsoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Tipo" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioBandoConcorsoRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                        <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoBandoConcorsoRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />
                                                                                    </td>
                                                                                    <td style="width: 140px">
                                                                                        <asp:Label ID="OpzioniScannerBandoConcorsoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Opzioni scanner" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="FronteRetroBandoConcorsoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Fronte retro" />&nbsp;<asp:CheckBox ID="FronteRetroBandoConcorsoCheckBox"
                                                                                                runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label ID="VisualizzaUIBandoConcorsoLabel"
                                                                                                    runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                                        ID="VisualizzaUIBandoConcorsoCheckBox" runat="server" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="GrigliaAllegatiBandoConcorsoPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td style="height: 20px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="DocumentiBandoConcorsoLabel" runat="server" CssClass="Etichetta"
                                                                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                                                                            Text="Allegati" />
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:ImageButton ID="ScansionaBandoConcorsoImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                                            ToolTip="Allega documento digitalizzato" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                                                ID="AggiungiDocumentoBandoConcorsoImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                ToolTip="Allega documento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                                                    ID="ScanUploadBandoConcorsoButton" Style="display: none" runat="server"
                                                                                                    ImageUrl="~/images//RecycleEmpty.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: #FFFFFF">
                                                                        <td>
                                                                            <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                <telerik:RadGrid ID="AllegatiBandoConcorsoGridView" runat="server" ToolTip="Elenco allegati associati alla pubblicazione"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Culture="it-IT" AllowMultiRowSelection="true">
                                                                                    <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                        <Columns>
                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                DataField="NomeFile" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("NomeFile")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                                                HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red">
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


                                    <asp:Panel Width="100%" runat="server" ID="EnteControllatoPanel" Visible="false">
                                      
                                        <table width="100%" cellpadding="2" cellspacing="2" border="0">

                                            <tr>
                                                <td>

                                                    <table style="width: 100%; background-color: #BFDBFF">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                                                                border-top: 0px solid  #9ABBE8; height: 25px">
                                                                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 100%">
                                                                                        &nbsp;<asp:Label ID="DatiEnteControllatoLabel" runat="server" Style="color: #00156E"
                                                                                            Font-Bold="True" CssClass="Etichetta" Text="Dati Ente Controllato" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <telerik:RadTabStrip runat="server" ID="EnteControllatoTabStrip" SelectedIndex="0"
                                                        MultiPageID="EnteControllatoMultiPage" Skin="Office2007" Width="100%">
                                                        <Tabs>
                                                            <telerik:RadTab Text="Generale" Selected="True" />
                                                             <telerik:RadTab Text="Dichiarazioni" />
                                                            <telerik:RadTab Text="Varie Ente" />
                                                            <telerik:RadTab Text="Bilancio Ente" />
                                                            <telerik:RadTab Text="Documenti" />
                                                        </Tabs>
                                                    </telerik:RadTabStrip>
                                                    
                                                    <telerik:RadMultiPage runat="server" ID="EnteControllatoMultiPage" SelectedIndex="0"
                                                        Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">

                                                        <telerik:RadPageView runat="server" ID="EnteControllatoPageView" CssClass="corporatePageView"
                                                            Height="260px">

                                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td class="ContainerMargin">
                                                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                                            style="height: 250px">
                                                                            <tr style="vertical-align: top">
                                                                                <td>
                                                                                    <table style="width: 100%">

                                                                                        <tr>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 120px">
                                                                                                            <asp:Label ID="RagioneSocialeEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Ragione Sociale *" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadComboBox ID="RagioneSocialeEnteControllatoCombobox" runat="server" Width="100%"
                                                                                                                Height="150" EmptyMessage="Seleziona Ente" EnableAutomaticLoadOnDemand="True"
                                                                                                                ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                                                                                Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                                                                                                <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx" />
                                                                                                            </telerik:RadComboBox>
                                                                                                        </td>
                                                                                                        <td style="width: 30px; text-align: center">
                                                                                                            <asp:ImageButton ID="TrovaEnteControllatoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                ToolTip="Seleziona Ente..." ImageAlign="AbsMiddle" Style="height: 16px" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="AggiornaEnteControllatoImageButton" runat="server" Style="display: none" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 120px">
                                                                                                            <asp:Label ID="FunzioniAttribuiteEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Funzioni Attribuite" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadTextBox ID="FunzioniAttribuiteEnteControllatoTextBox" Rows="2" TextMode="MultiLine"
                                                                                                                runat="server" Skin="Office2007" Width="100%" ToolTip="Funzioni Attibuite" MaxLength="1500"
                                                                                                                Style="overflow-x: hidden" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 120px">
                                                                                                            <asp:Label ID="AttivitaFavoreAmministrazioneEnteControllatoLabel" runat="server"
                                                                                                                CssClass="Etichetta" Text="Attività Amministr." ToolTip="Attività a favore dell'Amministrazione" />
                                                                                                        </td>
                                                                                                        <td style="width: 305px">
                                                                                                            <telerik:RadTextBox ID="AttivitaFavoreAmministrazioneEnteControllatoTextBox" Rows="2"
                                                                                                                TextMode="MultiLine" runat="server" Skin="Office2007" Width="305px" MaxLength="1500"
                                                                                                                ToolTip="Attività a favore dell'Amministrazione" Style="overflow-x: hidden" />
                                                                                                        </td>
                                                                                                        <td style="width: 110px; text-align: center">
                                                                                                            <asp:Label ID="AttivitaServizioPubblicoEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Attività Servizio" ToolTip="Attività Servizio pubblico" />
                                                                                                        </td>
                                                                                                        <td style="text-align: right">
                                                                                                            <telerik:RadTextBox ID="AttivitaServizioPubblicoEnteControllatoTextBox" Rows="2"
                                                                                                                TextMode="MultiLine" runat="server" Skin="Office2007" Width="305px" MaxLength="1500"
                                                                                                                ToolTip="Attività Servizio Pubblico" Style="overflow-x: hidden" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 120px">
                                                                                                            <asp:Label ID="MisuraPartecipazioneEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Misura Partecipaz." ToolTip="Misura partecipazione" />
                                                                                                        </td>
                                                                                                        <td style="width: 305px">
                                                                                                            <telerik:RadTextBox ID="MisuraPartecipazioneEnteControllatoTextBox" runat="server"
                                                                                                                Skin="Office2007" Width="305px" ToolTip="Misura partecipazione" MaxLength="255" />
                                                                                                        </td>
                                                                                                        <td style="width: 110px; text-align: center">
                                                                                                            <asp:Label ID="DurataImpegnoEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Durata Impegno" ToolTip="Durata Impegno" />
                                                                                                        </td>
                                                                                                        <td style="text-align: right">
                                                                                                            <telerik:RadTextBox ID="DurataImpegnoEnteControllatoTextbox" runat="server" Skin="Office2007"
                                                                                                                Width="305px" MaxLength="75" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 120px">
                                                                                                            <asp:Label ID="UrlSitoIstituzionaleEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Url" ToolTip="Url Sito Istituzionale" />
                                                                                                        </td>
                                                                                                        <td style="width: 500px">
                                                                                                            <telerik:RadTextBox ID="UrlSitoIstituzionaleEnteControllatoTextBox" runat="server"
                                                                                                                Skin="Office2007" Width="500px" MaxLength="250" ToolTip="La url deve iniziare con http:// oppure https://" />
                                                                                                        </td>
                                                                                                        <td style="width: 50px; text-align: center">
                                                                                                            <asp:Label ID="OnereComplessivoEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                                                Text="Onere" ToolTip="Onere Complessivo" />
                                                                                                        </td>
                                                                                                        <td style="text-align: right">
                                                                                                            <telerik:RadNumericTextBox ID="OnereComplessivoEnteControllatoTextBox" runat="server"
                                                                                                                Skin="Office2007" Width="170px" MaxLength="12" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td>
                                                                                                
                                                                                                <asp:Panel ID="PannelloSocietaPartecipate" runat="server" Visible="false">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 120px">
                                                                                                                <asp:Label ID="EntitaSocietaPartecipateLabel" runat="server" CssClass="Etichetta"
                                                                                                                    Text="Entità" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadNumericTextBox ID="EntitaSocietaPartecipateTextBox" runat="server" Skin="Office2007"
                                                                                                                    Width="80px" MaxLength="12" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </asp:Panel>
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

                                                          <telerik:RadPageView runat="server" ID="DichiarazioniEnteControllatoPageView" CssClass="corporatePageView"
                                                            Height="260px">

                                                              <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                  <tr>
                                                                      <td class="ContainerMargin">
                                                                          <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                                              style="height: 250px">
                                                                              <tr style="vertical-align: top">
                                                                                  <td>
                                                                                      <table style="width: 100%">

                                                                                          <tr>

                                                                                              <td style="width: 50%">
                                                                                                  <table style="width: 100%">
                                                                                                      <tr>
                                                                                                          <td style="width: 120px">
                                                                                                              <asp:Label ID="InconferibilitaLabel" runat="server" CssClass="Etichetta" Text="Inconferibilità" />
                                                                                                          </td>
                                                                                                          <td>
                                                                                                              <div id="inconferibilitaUpload1" runat="server" visible="true" style="width: 100%;">
                                                                                                                  <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                      <tr style="height: 30px">
                                                                                                                          <td style="padding-top: 6px">
                                                                                                                              <telerik:RadAsyncUpload ID="InconferibilitaUpload" runat="server" MaxFileInputsCount="1"
                                                                                                                                  OnClientFileSelected="InconferibilitaSelezionato" OnClientFileUploadRemoved="InconferibilitaRimosso"
                                                                                                                                  Skin="Office2007" Width="100%" InputSize="31" EnableViewState="True">
                                                                                                                                  <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                                              </telerik:RadAsyncUpload>
                                                                                                                          </td>
                                                                                                                          <td style="padding-top: 3px; width: 30px; text-align: center;">
                                                                                                                              <div id="divAggInconferibilita" style="visibility: hidden;">
                                                                                                                                  <asp:ImageButton ID="AggiungiInconferibilitaImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                                                      ToolTip="Allega Curriculum" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                              </div>
                                                                                                                          </td>
                                                                                                                      </tr>
                                                                                                                  </table>
                                                                                                              </div>
                                                                                                              <div id="InconferibilitaUpload2" runat="server" style="width: 100%;" visible="false">
                                                                                                                  <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                      <tr style="height: 34px">
                                                                                                                          <td>
                                                                                                                              <asp:LinkButton ID="InconferibilitaAllegatoLinkButton" ForeColor="Blue" CssClass="Etichetta"
                                                                                                                                  runat="server" />
                                                                                                                          </td>
                                                                                                                          <td style="width: 30px; text-align: center">
                                                                                                                              <asp:Label ID="NomeFileInconferibilitaLabel" runat="server" Visible="false" />
                                                                                                                              <asp:ImageButton ID="RimuoviInconferibilitaImageButton" runat="server" ImageUrl="~/images//Delete16.png"
                                                                                                                                  ToolTip="Rimuovi Inconferibilita" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                          </td>
                                                                                                                      </tr>
                                                                                                                  </table>
                                                                                                              </div>
                                                                                                          </td>
                                                                                                      </tr>
                                                                                                  </table>
                                                                                              </td>

                                                                                              <td style="width: 50%">
                                                                                                  <table style="width: 100%">
                                                                                                      <tr>
                                                                                                          <td style="width: 100px; text-align: center">
                                                                                                              <asp:Label ID="incompatibilitaLabel" runat="server" CssClass="Etichetta" Text="Incompatibilità" />
                                                                                                          </td>
                                                                                                          <td>
                                                                                                              <div id="incompatibilitaUpload1" runat="server" visible="true">
                                                                                                                  <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                      <tr style="height: 30px">
                                                                                                                          <td style="padding-top: 6px">
                                                                                                                              <telerik:RadAsyncUpload ID="IncompatibilitaUpload" runat="server" MaxFileInputsCount="1"
                                                                                                                                  OnClientFileSelected="IncompatibilitaSelezionato" OnClientFileUploadRemoved="IncompatibilitaRimosso"
                                                                                                                                  Skin="Office2007" Width="100%" InputSize="32" EnableViewState="True">
                                                                                                                                  <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                                              </telerik:RadAsyncUpload>
                                                                                                                          </td>
                                                                                                                          <td style="padding-top: 3px; width: 30px; text-align: center">
                                                                                                                              <div id="divAggIncompatibilita" style="visibility: hidden;">
                                                                                                                                  <asp:ImageButton ID="AggiungiIncompatibilitaImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                                                      ToolTip="Allega incompatibilita" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                              </div>
                                                                                                                          </td>
                                                                                                                      </tr>
                                                                                                                  </table>
                                                                                                              </div>
                                                                                                              <div id="incompatibilitaUpload2" runat="server" style="width: 100%;" visible="false">
                                                                                                                  <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                      <tr style="height: 34px">
                                                                                                                          <td>
                                                                                                                              <asp:LinkButton ID="IncompatibilitaAllegatoLinkButton" ForeColor="Blue" CssClass="Etichetta"
                                                                                                                                  runat="server" />
                                                                                                                          </td>
                                                                                                                          <td style="width: 30px; text-align: center">
                                                                                                                              <asp:Label ID="NomeFileIncompatibilitaLabel" runat="server" Visible="false" />
                                                                                                                              <asp:ImageButton ID="RimuoviIncompatibilitaImageButton" runat="server" ImageUrl="~/images//Delete16.png"
                                                                                                                                  ToolTip="Rimuovi incompatibilita" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                          </td>
                                                                                                                      </tr>
                                                                                                                  </table>
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

                                                        </telerik:RadPageView>

                                                          <telerik:RadPageView runat="server" ID="VarieEnteControllatoPageView" CssClass="corporatePageView" Height="260px">
                                                           
                                                              <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                  <tr>
                                                                      <td class="ContainerMargin">
                                                                          <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0" style="height:250px">
                                                                           
                                                                              <tr>
                                                                                  <td>
                                                                                      <table style=" width:100%">
                                                                                          <tr style="vertical-align: top">
                                                                                              <td style="width: 120px">
                                                                                                  <asp:Label ID="NumeroRappresentantiEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                                      Text="N. Rappresentanti" ToolTip="Numero di rappresentanti all'interno dell'Ente" />
                                                                                              </td>
                                                                                              <td>
                                                                                                  <telerik:RadNumericTextBox ID="NumeroRappresentantiEnteControllatoTextBox" runat="server"
                                                                                                      Skin="Office2007" Width="100px" DataType="System.Int32" MaxLength="10" MaxValue="9999999999"
                                                                                                      MinValue="0" ShowSpinButtons="True" ToolTip="Numero di rappresentanti all'interno dell'Ente">
                                                                                                      <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                                  </telerik:RadNumericTextBox>
                                                                                              </td>
                                                                                          </tr>
                                                                                      </table>
                                                                                  </td>
                                                                              </tr>

                                                                              <tr>
                                                                                  <td>
                                                                                      <table style="width:100%">
                                                                                          <tr>
                                                                                              <td style="width: 50%">

                                                                                                  <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                      <tr style="height: 20px; background-color: #BFDBFF">
                                                                                                          <td>
                                                                                                              <table style="width: 100%">
                                                                                                                  <tr>
                                                                                                                      <td>
                                                                                                                          &nbsp;<asp:Label Font-Bold="True" ID="TrattamentoEconomicoRappresentanteEnteControllatoLabel" runat="server"
                                                                                                                              Style="color: #00156E; background-color: #BFDBFF;" CssClass="Etichetta"
                                                                                                                              Text="Trattamento Economico" />
                                                                                                                      </td>
                                                                                                                      <td style=" width:195px">
                                                                                                                          <telerik:RadTextBox ID="TrattamentoEconomicoRappresentanteEnteControllatoTextbox" runat="server"
                                                                                                                              Skin="Office2007" Width="195px" />
                                                                                                                      </td>
                                                                                                                      <td style="width: 25px; text-align: center">
                                                                                                                          <asp:ImageButton ID="AggiungiTrattamentoEconomicoRappresentanteEnteControllatoImageButton" runat="server"
                                                                                                                              ImageUrl="~/images//add16.png" ToolTip="Aggiungi trattamento economico" ImageAlign="AbsMiddle" />
                                                                                                                      </td>
                                                                                                                      <td style="width: 25px; text-align: center">
                                                                                                                          <asp:ImageButton ID="DeleteTrattamentoEconomicoRappresentanteEnteControllatoImageButton" runat="server"
                                                                                                                              ImageUrl="~/images//RecycleEmpty.png" Style="width: 16px" ToolTip="Cancella trattamenti economici selezionati"
                                                                                                                              ImageAlign="AbsMiddle" />
                                                                                                                      </td>
                                                                                                                  </tr>
                                                                                                              </table>
                                                                                                          </td>
                                                                                                      </tr>
                                                                                                      <tr>
                                                                                                          <td>
                                                                                                              <div id="Div3">

                                                                                                                  <div id="TrattamentiEconomiciRappresentantiEnteControllatoPanel" runat="server" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9; background-color: White">

                                                                                                                   



                                                                                                                      <telerik:RadGrid ID="TrattamentiEconomiciRappresentantiEnteControllatoGridView" ToolTip="Elenco trattamenti economici"
                                                                                                                          runat="server" AllowPaging="False" AutoGenerateColumns="False" CellSpacing="0"
                                                                                                                          GridLines="None" Skin="Office2007" AllowSorting="True" AllowMultiRowSelection="True"
                                                                                                                          Culture="it-IT">
                                                                                                                          <ClientSettings>
                                                                                                                              <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                                                      
                                                                                                                          </ClientSettings>
                                                                                                                          <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                                                              <Columns>

                                                                                                                                  <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                                                      ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                                                                                  </telerik:GridClientSelectColumn>

                                                                                                                                  <telerik:GridBoundColumn DataField="Descrizione" DataType="System.String" FilterControlAltText="Filter DescrizioneHidden column"
                                                                                                                                      HeaderText="Descrizione" UniqueName="DescrizioneHidden" Visible="False" />
                                                                                                                                 
                                                                                                                                  <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                                                      HeaderText="Trattamento" DataField="Descrizione">
                                                                                                                                      <ItemTemplate>
                                                                                                                                          <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                              text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                              <%# Eval("Descrizione")%></div>
                                                                                                                                      </ItemTemplate>
                                                                                                                                  </telerik:GridTemplateColumn>

                                                                                                                                  <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                                      ItemStyle-Width="30px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center"
                                                                                                                                      ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\Delete16.png" UniqueName="Delete"
                                                                                                                                       Text="Elimina trattamento">
                                                                                                                                  </telerik:GridButtonColumn>

                                                                                                                              </Columns>
                                                                                                                          </MasterTableView></telerik:RadGrid></div>
                                                                                                              </div>
                                                                                                          </td>
                                                                                                      </tr>
                                                                                                  </table>

                                                                                              </td>

                                                                                              <td style="width: 50%">

                                                                                              <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                      <tr style="height: 20px; background-color: #BFDBFF">
                                                                                                          <td>
                                                                                                              <table style="width: 100%">
                                                                                                                  <tr>
                                                                                                                      <td>
                                                                                                                          &nbsp;<asp:Label Font-Bold="True" ID="IncaricoAmministratoreEnteControllatoLabel" runat="server"
                                                                                                                              Style="color: #00156E; background-color: #BFDBFF;" CssClass="Etichetta"
                                                                                                                              Text="Incarico" />
                                                                                                                      </td>
                                                                                                                      <td style=" width:150px">
                                                                                                                          <telerik:RadTextBox ID="IncaricoAmministratoreEnteControllatoTextBox" runat="server"
                                                                                                                              Skin="Office2007" Width="150px" />
                                                                                                                      </td>

                                                                                                                      <td style="width: 60px; text-align: center">
                                                                                                                          <asp:Label Font-Bold="True" ID="TrattamentoEconomicoAmministratoreEnteControllatoLabel" runat="server"
                                                                                                                              Style="color: #00156E; background-color: #BFDBFF;" CssClass="Etichetta"
                                                                                                                              Text="Tratt." />
                                                                                                                      </td>
                                                                                                                      <td style=" width:100px">
                                                                                                                          <telerik:RadTextBox ID="TrattamentoEconomicoAmministratoreEnteControllatoTextBox" runat="server"
                                                                                                                              Skin="Office2007" Width="100px" />
                                                                                                                      </td>




                                                                                                                      <td style="width: 25px; text-align: center">
                                                                                                                          <asp:ImageButton ID="AggiungiTrattamentoEconomicoIncaricoAmministratoreEnteControllatoImageButton" runat="server"
                                                                                                                              ImageUrl="~/images//add16.png" ToolTip="Aggiungi trattamento" ImageAlign="AbsMiddle" />
                                                                                                                      </td>
                                                                                                                      <td style="width: 25px; text-align: center">
                                                                                                                          <asp:ImageButton ID="DeleteTrattamentoEconomicoIncaricoAmministratoreEnteControllatoImageButton" runat="server"
                                                                                                                              ImageUrl="~/images//RecycleEmpty.png" Style="width: 16px" ToolTip="Cancella trattamenti selezionati"
                                                                                                                              ImageAlign="AbsMiddle" />
                                                                                                                      </td>
                                                                                                                  </tr>
                                                                                                              </table>
                                                                                                          </td>
                                                                                                      </tr>
                                                                                                      <tr>
                                                                                                          <td>
                                                                                                              <div id="Div4">

                                                                                                                  <div id="TrattamentiEconomiciIncarichiAmministratoreEnteControllatoPanel" runat="server" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9; background-color: White">

                                                                                                                      <telerik:RadGrid ID="TrattamentiEconomiciIncarichiAmministratoreEnteControllatoGridView" ToolTip="Elenco trattamenti economici"
                                                                                                                          runat="server" AllowPaging="False" AutoGenerateColumns="False" CellSpacing="0"
                                                                                                                          GridLines="None" Skin="Office2007" AllowSorting="True" AllowMultiRowSelection="True"
                                                                                                                          Culture="it-IT">
                                                                                                                          <ClientSettings>
                                                                                                                              <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                                                     
                                                                                                                          </ClientSettings>
                                                                                                                          <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                                                              <Columns>

                                                                                                                                  <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                                                      ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                                                                                  </telerik:GridClientSelectColumn>

                                                                                                                                   <telerik:GridBoundColumn DataField="Incarico" DataType="System.String" FilterControlAltText="Filter IncaricoHidden column"
                                                                                                                                      HeaderText="Incarico" UniqueName="IncaricoHidden" Visible="False" />

                                                                                                                                   <telerik:GridTemplateColumn SortExpression="Incarico" UniqueName="Incarico"
                                                                                                                                      HeaderText="Incarico" DataField="Incarico">
                                                                                                                                      <ItemTemplate>
                                                                                                                                          <div title='<%# Eval("Incarico")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                              text-overflow: ellipsis; width: 150px; border: 0px solid red">
                                                                                                                                              <%# Eval("Incarico")%></div>
                                                                                                                                      </ItemTemplate>
                                                                                                                                  </telerik:GridTemplateColumn>

                                                                                                                                  <telerik:GridBoundColumn DataField="Descrizione" DataType="System.String" FilterControlAltText="Filter DescrizioneHidden column"
                                                                                                                                      HeaderText="Descrizione" UniqueName="DescrizioneHidden" Visible="False" />
                                                                                                                                 
                                                                                                                                  <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                                                      HeaderText="Trattamento" DataField="Descrizione">
                                                                                                                                      <ItemTemplate>
                                                                                                                                          <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                              text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                              <%# Eval("Descrizione")%></div>
                                                                                                                                      </ItemTemplate>
                                                                                                                                  </telerik:GridTemplateColumn>


                                                                                                                                  <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                                      ItemStyle-Width="30px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center"
                                                                                                                                      ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\Delete16.png" UniqueName="Delete"
                                                                                                                                       Text="Elimina trattamento">
                                                                                                                                  </telerik:GridButtonColumn>

                                                                                                                              </Columns>
                                                                                                                          </MasterTableView></telerik:RadGrid></div>
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

                                                          </telerik:RadPageView>

                                                          <telerik:RadPageView runat="server" ID="BilancioEnteControllatoPageView" CssClass="corporatePageView" Height="260px">

                                                             <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                  <tr>
                                                                      <td class="ContainerMargin">
                                                                          <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                                              style="height: 250px">


                                                                              <tr style="vertical-align: top">
                                                                                  <td>

                                                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                        <tr style="height: 20px; background-color: #BFDBFF">
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style=" text-align:right">
                                                                                                            &nbsp;<asp:Label Font-Bold="True" ID="AnnoBilancioEnteControllatoLabel" runat="server"
                                                                                                                Style="color: #00156E; background-color: #BFDBFF;" CssClass="Etichetta" Text="Anno"
                                                                                                                ToolTip="Anno esercizio finanziario" />
                                                                                                        </td>
                                                                                                        <td style="width: 75px; text-align:right">
                                                                                                            <telerik:RadNumericTextBox ToolTip="Anno esercizio finanziario" ID="AnnoBilancioEnteControllatoTextBox"
                                                                                                                runat="server" Skin="Office2007" Width="70px" MaxLength="4">
                                                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                                            </telerik:RadNumericTextBox>
                                                                                                        </td>
                                                                                                        <td style="width: 70px; text-align: center">
                                                                                                            <asp:Label Font-Bold="True" ID="BilancioEnteControllatoLabel" runat="server" Style="color: #00156E;
                                                                                                                background-color: #BFDBFF;" CssClass="Etichetta" Text="Bilancio" ToolTip="Risultato di bilancio" />
                                                                                                        </td>
                                                                                                        <td style="width: 110px">
                                                                                                            <telerik:RadNumericTextBox ToolTip="Risultato di bilancio" ID="BilancioEnteControllatoTextBox"
                                                                                                                runat="server" Skin="Office2007" Width="110px" MaxLength="12" />
                                                                                                        </td>
                                                                                                        <td style="width: 25px; text-align: center">
                                                                                                            <asp:ImageButton ID="AggiungiBilancioEnteControllatoImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                                ToolTip="Aggiungi bilancio" ImageAlign="AbsMiddle" />
                                                                                                        </td>
                                                                                                        <td style="width: 25px; text-align: center">
                                                                                                            <asp:ImageButton ID="DeleteBilancioEnteControllatoImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                                                Style="width: 16px" ToolTip="Cancella bilanci selezionati" ImageAlign="AbsMiddle" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div id="Div5">
                                                                                                    <div id="BilanciEnteControllatoPanel" runat="server" style="overflow: auto; height: 150px;
                                                                                                        border: 1px solid #5D8CC9; background-color: White">
                                                                                                       
                                                                                                        <telerik:RadGrid ID="BilanciEnteControllatoGridView" ToolTip="Elenco bilanci" runat="server"
                                                                                                            AllowPaging="False" AutoGenerateColumns="False" CellSpacing="0" GridLines="None"
                                                                                                            Skin="Office2007" AllowSorting="True" AllowMultiRowSelection="True" Culture="it-IT">
                                                                                                            <ClientSettings>
                                                                                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                                  
                                                                                                            </ClientSettings>
                                                                                                            <MasterTableView DataKeyNames="Id, Anno" TableLayout="Fixed">
                                                                                                                <Columns>
                                                                                                                    <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                                                                                    </telerik:GridClientSelectColumn>
                                                                                                                    <telerik:GridTemplateColumn SortExpression="Anno" UniqueName="Anno" HeaderText="Anno"
                                                                                                                        DataField="Anno" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("Anno")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                                                width: 150px; border: 0px solid red">
                                                                                                                                <%# Eval("Anno")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn SortExpression="Risultato" UniqueName="Risultato" HeaderText="Bilancio"
                                                                                                                        DataField="Risultato">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("Risultato")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("Risultato")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                        ItemStyle-Width="30px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center"
                                                                                                                        ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\Delete16.png" UniqueName="Delete"
                                                                                                                        Text="Elimina bilancio">
                                                                                                                    </telerik:GridButtonColumn>
                                                                                                                </Columns>
                                                                                                            </MasterTableView></telerik:RadGrid></div>
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

                                                          </telerik:RadPageView>

                                                        <telerik:RadPageView runat="server" ID="AllegatiEnteControllatoPageView" CssClass="corporatePageView"
                                                            Height="260px">

                                                            <div id="AllegatiEnteControllatoPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr style="height: 30px">
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="DescrizioneDocumentoEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Descrizione" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="DescrizioneDocumentoEnteControllatoTextBox" runat="server"
                                                                                            Skin="Office2007" Width="250px" />
                                                                                    </td>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="NomeFileDocumentoEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Nome file" />
                                                                                    </td>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <telerik:RadAsyncUpload ID="AllegatoEnteControllatoUpload" runat="server"
                                                                                            MaxFileInputsCount="1" Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                            <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                        </telerik:RadAsyncUpload>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 90px">
                                                                                        <asp:Label ID="TipoAllegatoEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Tipo" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioEnteControllatoRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                        <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoEnteControllatoRadioButton"
                                                                                            GroupName="TipoDocumento" runat="server" />
                                                                                    </td>
                                                                                    <td style="width: 140px">
                                                                                        <asp:Label ID="OpzioniScannerEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Opzioni scanner" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="FronteRetroEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                            Text="Fronte retro" />&nbsp;<asp:CheckBox ID="FronteRetroEnteControllatoCheckBox"
                                                                                                runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label ID="VisualizzaUIEnteControllatoLabel"
                                                                                                    runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                                        ID="VisualizzaUIEnteControllatoCheckBox" runat="server" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <div id="GrigliaAllegatiEnteControllatoPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td style="height: 20px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="DocumentiEnteControllatoLabel" runat="server" CssClass="Etichetta"
                                                                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                                                                            Text="Allegati" />
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:ImageButton ID="ScansionaEnteControllatoImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                                            ToolTip="Allega documento digitalizzato" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                                                ID="AggiungiDocumentoEnteControllatoImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                ToolTip="Allega documento" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                                                    ID="ScanUploadEnteControllatoButton" Style="display: none" runat="server"
                                                                                                    ImageUrl="~/images//RecycleEmpty.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: #FFFFFF">
                                                                        <td>
                                                                            <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                <telerik:RadGrid ID="AllegatiEnteControllatoGridView" runat="server" ToolTip="Elenco allegati associati alla pubblicazione"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Culture="it-IT" AllowMultiRowSelection="true">
                                                                                    <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                        <Columns>
                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                DataField="NomeFile" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("NomeFile")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                HeaderText="Descrizione" DataField="Descrizione" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 350px; border: 0px solid red">
                                                                                                        <%# Eval("Descrizione")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn SortExpression="IdTipologiaAllegato" UniqueName="IdTipologiaAllegato"
                                                                                                HeaderText="Tipologia" DataField="IdTipologiaAllegato">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# IIF( Eval("IdTipologiaAllegato") =1 , "Primario","Allegato") %>'
                                                                                                        style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; border: 0px solid red">
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
                                                                    <telerik:GridTemplateColumn SortExpression="IdTipologiaFascicolo" UniqueName="" HeaderText="Tipo"
                                                                        DataField="IdTipologiaFascicolo" HeaderStyle-Width="45px" ItemStyle-Width="45px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# IIf(Eval("IdTipologiaFascicolo")=1, "P = Procedimento", "A = Affare")%>'
                                                                                style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 45px;
                                                                                border: 0px solid red">
                                                                                <%# IIf(Eval("IdTipologiaFascicolo") = 1, "P", "A")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
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
                                                    <asp:Label ID="TitoloElencoAttiLabel" runat="server" Font-Bold="True" CssClass="Etichetta"
                                                        Style="width: 600px; color: #00156E; background-color: #BFDBFF" Text="Elenco Atti Amministrativi"
                                                        ToolTip="Ultimi cinque atti amministrativi registrati dall'utente corrente" />
                                                </td>
                                                <td align="center" style="width: 30px;">
                                                    <asp:ImageButton ID="EspRicXls" runat="server" ImageUrl="~/images/excel16.png" ToolTip="Esporta in Excel gli atti visualizzati"
                                                        Style="border: 0" ImageAlign="AbsMiddle" />
                                                </td>
                                                <td align="center" style="width: 125px; border-left: 0 solid #5D8CC9;">
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
                                                <td align="center" style="width: 30px;">
                                                    <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                        ToolTip="Filtra atti amministrativi" Style="border: 0" ImageAlign="AbsMiddle" />
                                                </td>
                                                <td align="center" style="width: 30px;">
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
                                            <telerik:RadGrid ID="DocumentiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="True" PageSize="5"
                                                Culture="it-IT">
                                                <MasterTableView DataKeyNames="Id">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="false" />

                                                        

                                                        <%--     <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                            HeaderText="Numero" DataField="NumeroProtocollo" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("NumeroProtocollo","{0:0000000}")%>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 65px;">
                                                                    <%# Eval("NumeroProtocollo", "{0:0000000}")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>--%>
                                                        <telerik:GridTemplateColumn SortExpression="ContatoreGenerale" UniqueName="ContatoreGenerale"
                                                            HeaderText="N." DataField="ContatoreGenerale" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("ContatoreGenerale")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 40px;">
                                                                    <%# Eval("ContatoreGenerale")%>
                                                                </div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneTipologia" UniqueName="DescrizioneTipologia"
                                                            HeaderText="Tipo" DataField="DescrizioneTipologia" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DescrizioneTipologia")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 60px;">
                                                                    <%# Eval("DescrizioneTipologia")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn SortExpression="DataDocumento" UniqueName="DataDocumento"
                                                            HeaderText="Data" DataField="DataDocumento" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DataDocumento","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 65px;">
                                                                    <%# Eval("DataDocumento", "{0:dd/MM/yyyy}")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                            DataField="Oggetto" HeaderStyle-Width="170px" ItemStyle-Width="170px">
                                                            <ItemTemplate>
                                                                <div title='<%# Replace(Eval("Oggetto"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 170px;">
                                                                    <%# Eval("Oggetto")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneUfficio" UniqueName="DescrizioneUfficio"
                                                            HeaderText="Ufficio" DataField="DescrizioneUfficio" HeaderStyle-Width="170px"
                                                            ItemStyle-Width="170px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DescrizioneUfficio")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 170px;">
                                                                    <%# Eval("DescrizioneUfficio")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneSettore" UniqueName="DescrizioneSettore"
                                                            HeaderText="Settore" DataField="DescrizioneSettore" HeaderStyle-Width="200px"
                                                            ItemStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DescrizioneSettore")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 200px;">
                                                                    <%# Eval("DescrizioneSettore")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Stato" FilterControlAltText="Filter Stato column"
                                                            ImageUrl="~\images\vuoto.png" UniqueName="Stato" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                        <telerik:GridButtonColumn FilterControlAltText="Filter Unlock column" ImageUrl="~/images/LockDelete16.png"
                                                            ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-VerticalAlign="Middle" UniqueName="Unlock" ButtonType="ImageButton"
                                                            CommandName="Unlock" />
                                                        <telerik:GridButtonColumn FilterControlAltText="Filter Copy column" ImageUrl="~/images/copy16.png"
                                                            UniqueName="Copy" ButtonType="ImageButton" ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CommandName="Copy"
                                                            Text="Copia Documento" />
                                                        <telerik:GridButtonColumn FilterControlAltText="Filter Select column" ImageUrl="~/images/Checks.png"
                                                            ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-VerticalAlign="Middle" UniqueName="Select" ButtonType="ImageButton"
                                                            CommandName="Select" Text="Seleziona Documento" />
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

            <%-- INIZIO GRIDVIEW FULL SIZE--%>
            <div id="FullSizeGridPanel" style="position: absolute; width: 100%; height: 100%;
                text-align: center; z-index: 999; display: none; top: 0px; left: 0px; background-color: White">
                <table style="width: 100%; height: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                    <tr>
                        <td style="vertical-align: top; height: 30px">
                            <table style="width: 100%; background-color: #BFDBFF">
                                <tr>
                                    <td>
                                        &nbsp;<asp:Label ID="TitoloElencoFullSizeAttiLabel" runat="server" Font-Bold="True"
                                            Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Atti Amministrativi" />
                                    </td>
                                    <td align="center" style="width: 125; border-left: 0 solid #5D8CC9;">
                                        <telerik:RadButton ID="FullSizeNoPaging" runat="server" Text="Non Paginare" Skin="Office2007"
                                            ImageAlign="AbsMiddle" Width="115px" ToolTip="Disattiva/Attiva Paginazione">
                                            <Icon PrimaryIconUrl="~/images/Next.png" PrimaryIconLeft="5px" />
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
                        <td style="vertical-align: top; padding-bottom: 2px; position: relative">
                            <div id="fullSizeScrollPanel" runat="server" style="overflow: auto; height: 100%;
                                width: 100%; background-color: #FFFFFF; border-top: 1px solid #5D8CC9; position: absolute;">
                                <telerik:RadGrid ID="FullSizeDocumentiGridView" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                    AllowSorting="True" AllowFilteringByColumn="True" EnableLinqExpressions="false"
                                    Culture="it-IT">
                                    <GroupingSettings CaseSensitive="False" />
                                    <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="false" />
                                            <telerik:GridTemplateColumn SortExpression="ContatoreGenerale" UniqueName="ContatoreGenerale"
                                                HeaderText="N." DataField="ContatoreGenerale" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                                                FilterControlWidth="100%">
                                                <FilterTemplate>
                                                    <telerik:RadNumericTextBox Width="100%" ID="FiltroContatoreGeneraleTextBox" runat="server"
                                                        MaxLength="9" ClientEvents-OnLoad="OnFiltroContatoreGeneraleTextBoxLoad" DbValue='<%# TryCast(Container,GridItem).OwnerTableView.GetColumn("ContatoreGenerale").CurrentFilterValue %>'
                                                        ClientEvents-OnKeyPress="OnFiltroContatoreGeneraleTextBoxKeyPressed" Skin="Office2007">
                                                        <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                    </telerik:RadNumericTextBox>
                                                    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                        <script type="text/javascript">
                                                            function OnFiltroContatoreGeneraleTextBoxKeyPressed(sender, args) {
                                                                if (13 == args.get_keyCode()) {
                                                                    var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                    var c = sender.get_textBoxValue();
                                                                    tableView.filter("ContatoreGenerale", c, "EqualTo");
                                                                    args.set_cancel(true);
                                                                }
                                                                var text = sender.get_value() + args.get_keyCharacter();
                                                                if (!text.match('^[0-9\b]+$'))
                                                                    args.set_cancel(true);
                                                            }

                                                            // SOVRASCRIVO GLI STILI
                                                            function OnFiltroContatoreGeneraleTextBoxLoad(sender, args) {
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
                                                    <div title='<%# Eval("ContatoreGenerale")%>' style="white-space: nowrap; overflow: hidden;
                                                        text-overflow: ellipsis; width: 55px;">
                                                        <%# Eval("ContatoreGenerale")%>
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="DescrizioneTipologia" UniqueName="DescrizioneTipologia"
                                                AllowFiltering="false" HeaderText="Tipo" DataField="DescrizioneTipologia" HeaderStyle-Width="100px"
                                                ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <div title='<%# Eval("DescrizioneTipologia")%>' style="white-space: nowrap; overflow: hidden;
                                                        text-overflow: ellipsis; width: 95px;">
                                                        <%# Eval("DescrizioneTipologia")%></div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="DataDocumento" UniqueName="DataDocumento"
                                                HeaderText="Data" DataField="DataDocumento" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                <FilterTemplate>
                                                    <telerik:RadDatePicker ID="DataDocumentoTextBox" Skin="Office2007" ShowPopupOnFocus="true"
                                                        DatePopupButton-Visible="false" Width="100%" runat="server" MinDate="1753-01-01"
                                                        ClientEvents-OnDateSelected="OnDataDocumentoTextBoxDateSelected" DbSelectedDate='<%# DataDocumento(Container) %>'
                                                        DateInput-ClientEvents-OnKeyPress="OnDataDocumentoTextBoxKeyPressed">
                                                        <Calendar  runat="server">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                                        <script type="text/javascript">

                                                            function OnDataDocumentoTextBoxKeyPressed(sender, args) {
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
                                                                        // picker.hidePopup();
                                                                    } else {
                                                                        picker.set_selectedDate(null);
                                                                        //picker.hidePopup();
                                                                    }
                                                                    args.set_cancel(true);

                                                                }
                                                            }


                                                            function OnDataDocumentoTextBoxDateSelected(sender, args) {
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

                                                                tableView.filter("DataDocumento", date + " " + toDate, "Between");

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
                                                    <div title='<%# Eval("DataDocumento","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                        overflow: hidden; text-overflow: ellipsis; width: 85px;">
                                                        <%# Eval("DataDocumento", "{0:dd/MM/yyyy}")%></div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="Oggetto" FilterControlAltText="Filter Oggetto column"
                                                HeaderText="Oggetto" SortExpression="Oggetto" UniqueName="Oggetto" AutoPostBackOnFilter="True"
                                                FilterControlWidth="100%" ShowFilterIcon="False">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DescrizioneUfficio" FilterControlAltText="Filter DescrizioneUfficio column"
                                                HeaderStyle-Width="220px" ItemStyle-Width="220px" HeaderText="Ufficio" SortExpression="DescrizioneUfficio"
                                                UniqueName="DescrizioneUfficio" AutoPostBackOnFilter="True" FilterControlWidth="100%"
                                                ShowFilterIcon="False">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DescrizioneSettore" FilterControlAltText="Filter DescrizioneSettore column"
                                                HeaderStyle-Width="220px" ItemStyle-Width="220px" HeaderText="Settore" SortExpression="DescrizioneSettore"
                                                UniqueName="DescrizioneSettore" AutoPostBackOnFilter="True" FilterControlWidth="100%"
                                                ShowFilterIcon="False">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="VisualizzaDocumento"
                                                FilterControlAltText="Filter VisualizzaDocumento column" ImageUrl="~\images\Documento16.gif"
                                                UniqueName="VisualizzaDocumento" HeaderStyle-Width="30px" Text="Visualizza Documento..."
                                                ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="VisualizzaCopiaDocumento"
                                                FilterControlAltText="Filter VisualizzaCopiaDocumento column" ImageUrl="~\images\DocumentoCopia16.gif"
                                                UniqueName="VisualizzaCopiaDocumento" HeaderStyle-Width="30px" Text="Visualizza Copia Documento..."
                                                ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                                            <telerik:GridButtonColumn FilterControlAltText="Filter Select column" ImageUrl="~/images/Checks.png"
                                                ItemStyle-Width="30px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" UniqueName="Select" ButtonType="ImageButton"
                                                CommandName="Select" Text="Seleziona Documento" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <%-- FINE GRIDVIEW FULL SIZE--%>



             <div id="FatturaPanel" style="position: absolute; width: 100%; text-align: center;
            z-index: 2000000; display: none; top: 0px">
            <div id="ShadowFatturaPanel" style="width: 800px; text-align: center; background-color: #BFDBFF;
                margin: 0 auto">
                <parsec:VisualizzaFatturaControl runat="server" ID="VisualizzaFatturaControl" />
            </div>
        </div>



        </div>

       

        <asp:ImageButton ID="salvaContenutoButton" runat="server" ImageUrl="~/images//knob-search16.png"
            Style="display: none; width: 0px" />
        <asp:HiddenField ID="documentContentHidden" runat="server" />
        <asp:HiddenField ID="infoScansioneHidden" runat="server" />
        <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
        <asp:ImageButton ID="AggiornaFirmaImageButton" runat="server" Style="display: none" />
        <asp:ImageButton ID="EliminaLockDocumdentoImageButton" runat="server" Style="display: none" />
        <asp:ImageButton ID="verificaRigenerazioneDocumento" runat="server" ImageUrl="~/images//knob-search16.png"
            Style="display: none; width: 0px" />
        <asp:ImageButton ID="rigeneraDocumento" runat="server" ImageUrl="~/images//knob-search16.png"
            Style="display: none; width: 0px" />
        <asp:HiddenField ID="verificaDocumentoHidden" runat="server" />
        <asp:ImageButton ID="chiudiImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
            Style="display: none; width: 0px" />
        <%--<asp:ImageButton ID="eseguiPubblicazioneOnline" runat="server" Style="display: none" />--%>
        <asp:HiddenField ID="infoSessioneHidden" runat="server" />
        <%--<asp:HiddenField ID="IdBandoGaraHidden" runat="server" />--%>
        <asp:ImageButton ID="AggiornaFirmaDigitaleImageButton" runat="server" Style="display: none" />
        <asp:HiddenField ID="signerOutputHidden" runat="server" />

         <asp:HiddenField ID="IdFirmaModificabileHidden" runat="server" />

          <asp:HiddenField ID="ElencoIdAllegatiBloccatiHidden" runat="server" />

    </div>

    <%--  </ContentTemplate>--%>
    <%--  </asp:UpdatePanel>--%>

   

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="AjaxPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="AjaxPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

</asp:Content>
