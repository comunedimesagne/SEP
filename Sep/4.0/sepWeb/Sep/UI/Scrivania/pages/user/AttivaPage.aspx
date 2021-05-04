<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="AttivaPage.aspx.vb" Inherits="AttivaPage" %>

    <%@ Register Src="~/UI/Scrivania/pages/user/OperazioneUserControl.ascx" TagName="OperazioneControl" TagPrefix="parsec" %>
    <%--<%@ Register Src="~/UI/Scrivania/pages/user/OperazioneUserControlSuape.ascx" TagName="OperazioneControlSUAPE" TagPrefix="parsec" %>--%>
    <%@ Register Src="~/UI/Protocollo/pages/user/VisualizzaEmailUserControl.ascx" TagName="VisualizzaEmailControl" TagPrefix="parsec" %>
    <%@ Register Src="~/UI/Protocollo/pages/user/VisualizzaFatturaUserControl.ascx" TagName="VisualizzaFatturaControl" TagPrefix="parsec" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">





    <script type="text/javascript">

        var panelIndex = -1;   //-1 Nessuno ; 0 = Email ; 1 = Fattura

        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");
        var overlaySearch = document.createElement("div");

        var hide = true;
        var updateGrid = false;
        var hideSearch = true;
        var enableUiHidden = '';
        var hideEmailPanel = true;
        var hideFatturaPanel = true;

        var hideRecipients = true;

        var currentPanel;   // 0 = OperazioneUserControl  1= OperazioneUserControlSuape 


        function HideMainScreen() {
            var screen = $get("mainscreen");
            screen.style.display = 'none';
        }

        function ShowMainScreen() {
            var screen = $get("mainscreen");
            screen.style.display = '';
            screen.style.position = 'absolute';
            screen.style.left = '0px';
            screen.style.top = '0px';
            screen.style.width = '100%';
            screen.style.height = '100%';
            screen.style.zIndex = 20000;
            screen.style.backgroundColor = '#09718F';
            screen.style.filter = "alpha(opacity=20)";
            screen.style.opacity = "0.2";
        }

      

        // ASP.NET AJAX's pageLoad() Method
        function pageLoad() {

           
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);

           

            //SE CI SONO APPLET 
            //ATTENZIONE C'E' SEMPRE UN APPLET REGISTRATO PER IL CONTROLLO UPLOAD DELLA TELERIK CHE UTILIZZA SILVERLIGHT
            if (document.applets.length > 1) {
                CheckAppletIsActive();
             }

            $get("pageContent").appendChild(overlaySearch);
            $get("pageContent").appendChild(overlay);
            $get("pageContent").appendChild(_backgroundElement);

            //document.body.appendChild(overlaySearch);
            //document.body.appendChild(overlay);
            //document.body.appendChild(_backgroundElement);


            if (enableUiHidden != '') {

                var hidden = document.getElementById(enableUiHidden);
                if (hidden) {
                    var stato = hidden.value;
                    if (stato == 'Abilita') {
                        panelIsVisible = false;
                        hidden.value = '';
                    }
                }
            }


            //VISUALIZZO E NASCONDO IL  POPUP (TASK)
            if (hide) {
                HidePanel(currentPanel);
            } else {
                ShowPanel(currentPanel);
            }


            //            //VISUALIZZO E NASCONDO IL  POPUP (EMAIL)
            //            if (hideEmailPanel) {
            //                HideEmailPanel();
            //            } else {
            //                ShowEmailPanel();
            //            }


            //            //VISUALIZZO E NASCONDO IL  POPUP (FATTURA)
            //            if (hideFatturaPanel) {
            //                HideFatturaElettronicaPanel();
            //            } else {
            //                ShowFatturaElettronicaPanel();
            //            }

            if (panelIndex != -1) {

                //VISUALIZZO E NASCONDO IL  POPUP (FATTURA)

                if (panelIndex == 1) {
                    if (hideFatturaPanel) {
                        HideFatturaElettronicaPanel();
                    } else {
                        ShowFatturaElettronicaPanel();
                    }

                }

                //VISUALIZZO E NASCONDO IL  POPUP (EMAIL)
                if (panelIndex == 0) {

                    if (hideEmailPanel) {
                        HideEmailPanel();
                    } else {
                        ShowEmailPanel();
                    }

                }
            }



            //VISUALIZZO E NASCONDO IL PANNELLO OVERLAY DEL POPUP (TASK)
            if (panelIsVisible) {

                switch (currentPanel) {
                    case 0:
                        ShowControlPanel();
                        //ShowMainScreen();
                        break;
                    case 1:
                        ShowControlPanelSUAP();
                        break;
                    default:
                        break;
                }

            } else {
                switch (currentPanel) {
                    case 0:
                        HideControlPanel();
                        // HideMainScreen();
                        break;
                    case 1:
                        HideControlPanelSUAP();
                        break;
                    default:
                        break;
                }

            }

            //VISUALIZZO E NASCONDO IL  POPUP (SEARCH)
            if (hideSearch) {
                HideSearchPanel();
            } else {
                ShowSearchPanel();
            }

            //VISUALIZZO E NASCONDO IL  POPUP (VISUALIZZAZIONE FATTURA) DEL PANNELLO DELLE OPERAZIONI
//            if (panelFatturaIsVisible) {
//                ShowFatturaPanelOperazione();
//            } else {
//                HideFatturaPanelOperazione();
            //            }


            //VISUALIZZO E NASCONDO IL  POPUP (DESTINATARI) DEL PANNELLO DELLE OPERAZIONI - OperazioneUserControl
            if (currentPanel == 0) {
                if (hideRecipients) {
                    // DEFINITO NEL PANNELLO DELLE OPERAZIONI - OperazioneUserControl
                    HideRecipientsPanel();
                } else {
                    ShowRecipientsPanel();
                }
            }
           


            document.getElementById("valore").value = 'Nascosto' + hide;
        }


        function UpdateTask() {
            document.getElementById("<%= AggiornaTaskButton.ClientID %>").click();
        }

        function HidePanel(index) {
           
         
            if (updateGrid) {
                document.getElementById("<%= AggiornaTaskButton.ClientID %>").click();
                updateGrid = false;
                panelIsVisible = false;
            }


            var panel;

            switch (index) {
                case 0:
                    panel = document.getElementById("taskPanel");
                    break;
                case 1:
                    panel = document.getElementById("taskPanelSUAPE");
                    break;
                default:
                    break;
            }

            if (panel) {
                panel.style.display = "none";
            }
           
            overlay.style.display = 'none';
            // _backgroundElement.style.display = 'none';
        }

        function ShowPanel(index) {
           
          
           
            with (overlay) {
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


            var panel;
            var shadow;

            switch (index) {
                case 0:
                    panel = document.getElementById("taskPanel");
                    shadow = document.getElementById("containerPanel");
                    break;
                case 1:
                     panel = document.getElementById("taskPanelSUAPE");
                    shadow = document.getElementById("containerPanelSUAPE");
                    break;
                default:
                    break;
            }


            with (panel) {
                style.display = '';
                style.position = 'absolute';
                style.top = 80 + "px";
            }


            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }


        }


        function ShowOverlay() {

//            var p = document.createElement("div");
//            $get("pageContent").appendChild(p);
//            with (p) {
//                style.display = '';
//                style.position = 'absolute';
//                style.left = '0px';
//                style.top = '0px';
//                style.width = '100%';
//                style.height = '100%';
//                style.zIndex = 10000;
//                style.backgroundColor = '#09718F';
//                style.filter = "alpha(opacity=20)";
//                style.opacity = "0.2";

//            }
        }


        function HideSearchPanel() {
            var panel = document.getElementById("searchPanel");
            panel.style.display = "none";
            overlaySearch.style.display = 'none';
            //_backgroundElement.style.display = 'none';
        }


        function ShowSearchPanel() {

            with (overlaySearch) {
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

            var panel = document.getElementById("searchPanel");

            with (panel) {
                style.display = '';
                style.position = 'absolute';
                style.zIndex = 2000000;
            }


            var shadow = document.getElementById("searchContainerPanel");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }
        }



        function OnBeginRequest(sender, args) {
            EnableUI(false);


        }

        function OnEndRequest(sender, args) {
            //SE  NASCONDO IL PANNELLO DI CONTROLLO DEI TASK
               EnableUI(true);
            if (hide) {
           
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


//        function OnClientBeforeClose(oWnd, args) {

//            if (args.get_argument() == null) {
//                document.getElementById("<%= SbloccaTaskButton.ClientID %>").click();

//            }

        //        }


        //***********************************************************************
        //INIZIO GESTIONE CONTROLLO CARICAMENTO APPLET
        //***********************************************************************

        var notificato = false;

        
        function CheckAppletIsActive() {


            var isActive = false;
            try {

                for (i = 0; i < document.applets.length; i++) {
                    isActive = document.applets[i].isActive();
                    var code = document.applets[i].code
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

            DisableGridButtonColumns($find("<%=TaskGridView.ClientID %>"), 'Execute')

          
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

        //***********************************************************************
        //FINE GESTIONE CONTROLLO CARICAMENTO APPLET
        //***********************************************************************



        function HideEmailPanel() {
            panelIndex = -1;
            var panel = document.getElementById("EmailPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
        }

        function ShowEmailPanel() {

            panelIndex = 0;

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




        function HideFatturaElettronicaPanel() {
            panelIndex = -1;
            var panel = document.getElementById("FatturaPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
        }

        

        function ShowFatturaElettronicaPanel() {

            panelIndex = 1;
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


        function copy(text) {
           var textArea = document.createElement('textarea');
            textArea.setAttribute('style', 'width:1px;border:0;opacity:0;');
            document.body.appendChild(textArea);
            textArea.value = text;
            textArea.select();
            document.execCommand('copy');
            document.body.removeChild(textArea);
        }


        //COPIO IL TESTO NEGLI APPUNTI
        function SetClipboard(text) {
            var id = "clipboard-textarea-hidden-id";
            var existsTextarea = document.getElementById(id);

            if (!existsTextarea) {

                var textarea = document.createElement("textarea");
                textarea.id = id;
                textarea.style.position = 'fixed';
                textarea.style.top = -1000;
                textarea.style.left = -1000;
                textarea.style.width = '1px';
                textarea.style.height = '1px';
                textarea.style.background = 'transparent';
                document.body.appendChild(textarea);

                existsTextarea = document.getElementById(id);
            }

            //window.clipboardData.setData('Text',text);

            existsTextarea.value = text;
            existsTextarea.select();
          

            try { var status = document.execCommand('copy'); }
            catch (err) { }
        }





    </script>


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


    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>

            <div id="pageContent">
                <center>


                    <table style="width: 100%; height: 600px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td valign="top">

                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" style="width: 100%;
                                    height: 100%;border-top: 1px solid #9ABBE8">
                                   <%-- <tr style="height: 20px">
                                        <td valign="top">

                                            <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="FiltroTaskLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Filtro" />
                                                    </td>
                                                  
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>--%>

                                    <tr style="height: 30px">
                                        <td valign="top">
                                            <%-- INIZIO FILTRO--%>

                                            <table style="width: 100%">
                                                <tr>
                                                    <td style=" width:85px">
                                                        &nbsp;<asp:Label ID="AttoriScrivaniaLabel" runat="server" CssClass="Etichetta" Text="Scrivania di"
                                                            />
                                                    </td>
                                                    <td>
                                                     <telerik:RadComboBox AutoPostBack="True" ToolTip="Titolare della scrivania" ID="DelegheScrivaniaComboBox"
                                                                        runat="server" Skin="Office2007" Width="500px" EmptyMessage="- Selezionare -"
                                                                        ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" NoWrap="True" />

                                                                        <input type="text" id="valore" style=" width:100%; display:none"  />
                                                    </td>
                                                    
                                                </tr>
                                                 </table>

                                           <table style="width: 100%; display:none">
                                                  
                                                <tr style="display: none">
                                                    <td>
                                                        &nbsp;<asp:Label ID="StatoLabel" runat="server" CssClass="Etichetta" Text="Stato"
                                                            Width="80px" />
                                                    </td>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 40px">
                                                                   <%-- <telerik:RadComboBox ToolTip="Stato dell'iter" ID="StatoComboBox" runat="server"
                                                                        Skin="Office2007" Width="230px" EmptyMessage="- Selezionare -" ItemsPerRequest="10"
                                                                        Filter="StartsWith" MaxHeight="400px" NoWrap="True" />--%>
                                                                </td>
                                                                <td>
                                                                    &nbsp;&nbsp;<asp:Label ID="RiferimentoDocumentoLabel" runat="server" CssClass="Etichetta"
                                                                        Text="Riferimento" />&nbsp;&nbsp;
                                                                    <telerik:RadTextBox ID="RiferimentoDocumentoTextBox" runat="server" Skin="Office2007"
                                                                        Width="220px" />
                                                                   
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>

                                            <%-- FINE FILTRO--%>
                                        </td>
                                    </tr>

                                    <tr style="height: 20px">
                                        <td valign="top">


                                            <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                                <tr style="height: 26px">

                                                    <td style="width: 140px">
                                                        &nbsp;<asp:Label ID="ElencoTaskLabel" runat="server" Font-Bold="True" Style="color: #00156E;
                                                            background-color: #BFDBFF" Text="Attività" />
                                                    </td>

                                                    <td style="width: 50px">
                                                        &nbsp;<asp:Label ID="FiltriLabel" runat="server" Font-Bold="True" Style="color: #00156E;
                                                            background-color: #BFDBFF; text-align: center" Text="Filtri:" />
                                                    </td>

                                                    <td>
                                                        <span style=" width:2px; float:right"></span>
                                                        <asp:Repeater ID="ToolBarFiltri" runat="server">
                                                            <HeaderTemplate>
                                                                <table style="height: 100%;" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <td align="left">
                                                                    <table cellpadding="1" cellspacing="1" style="border: 1px solid #5A7892; height: 18px;
                                                                        -webkit-border-radius: 0.5em; -moz-border-radius: 0.5em; border-radius: 0.5em;
                                                                        background-color: #FFCB61">
                                                                        <tr>
                                                                            <td style="vertical-align: middle">
                                                                                <asp:Label ID="DescrizioneFiltroLabel" Style="color: #00156E; font-size: 10px; font-family: Verdana;
                                                                                    vertical-align: middle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Descrizione").Tostring %>'
                                                                                    ToolTip='<%# DataBinder.Eval(Container.DataItem, "Tooltip").Tostring %>' />
                                                                            </td>
                                                                            <td style="width: 30px" align="right">
                                                                                <asp:ImageButton ImageAlign="AbsMiddle" ID="CancellaFiltroImage" runat="server" ImageUrl="~/Images/filterClose2.png"
                                                                                    ToolTip="Annulla filtro" onmouseout="this.src='/sep/images/filterClose2.png';"
                                                                                    onmouseover="this.src='/sep/Images/filterCloseSelected.png';" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id").Tostring %>'
                                                                                    Style="border: 0px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </ItemTemplate>
                                                            <SeparatorTemplate>
                                                                <td style="width: 2px">
                                                                </td>
                                                            </SeparatorTemplate>
                                                            <FooterTemplate>
                                                                </tr> </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </td>


                                                    <td align="center" style="width: 35px">
                                                        <asp:ImageButton ID="EsportaInExcelImageButton" Style="border: 0; width: 20px; height: 20px"
                                                            runat="server" ImageUrl="~/images//excel32.png" ToolTip="Esporta  in un file formato excel"
                                                            ImageAlign="AbsMiddle" />
                                                    </td>

                                                    <td style="width: 10px">
                                                        <asp:Image ID="SeparatorImageButton" runat="server" ImageUrl="~/images//NavigatorSeparator.png"
                                                            Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>

                                                    <td align="center" style="width: 35px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Ricerca Avanzata..." Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>

                                                    <td align="center" style="width: 35">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>

                                                </tr>
                                            </table>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td valign="top" class="ContainerMargin">

                                            <table class="Container" cellpadding="0" cellspacing="4" style="width: 100%; height: 100%"
                                                border="0">
                                                <tr style="background-color: #DFE8F6">
                                                    <td valign="top">
                                                         <div id="scrollPanel" runat="server" style="overflow: auto; height: 600px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="TaskGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            GridLines="None" Skin="Office2007"  AllowFilteringByColumn="true" AllowMultiRowSelection="True"
                                                            AllowSorting="True" Culture="it-IT">
                                                              <GroupingSettings CaseSensitive="false" /> 
                                                            <MasterTableView DataKeyNames="Id,NomeFileIter,IdDocumento,IdModulo, IdIstanza">


                                                             <NestedViewTemplate>
                                                                        <telerik:RadGrid ID="IterGridView" runat="server" AutoGenerateColumns="False" ToolTip="Lista dell'attività collegate all'istanza dell'iter"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                            AllowPaging="True">

                                                                            <MasterTableView Width="100%" TableLayout="Fixed">
                                                                            
                                                                                <Columns>
                                                                                    <telerik:GridBoundColumn DataField="ID" Visible="False" />



                                                                                    <telerik:GridTemplateColumn SortExpression="AttoreMittente" UniqueName="AttoreMittente" HeaderText="Utente"
                                                                                        DataField="AttoreMittente" HeaderStyle-Width="190px" ItemStyle-Width="190px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("AttoreMittente")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 190px; border: 0px solid red">
                                                                                                <%# Eval("AttoreMittente")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <%--<telerik:GridTemplateColumn SortExpression="DataInizio" UniqueName="DataInizio" HeaderText="Inizio"
                                                                                        DataField="DataInizio" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DataInizio","{0:dd/MM/yyyy HH.mm}")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 100px; border: 1px solid red">
                                                                                                <%# Eval("DataInizio", "{0:dd/MM/yyyy HH.mm}")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>--%>
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
                                                                                   <%-- <telerik:GridTemplateColumn SortExpression="Stato" UniqueName="Stato" HeaderText="Stato"
                                                                                        DataField="Stato" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Stato")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 80px; border: 0 solid red">
                                                                                                <%# Eval("Stato")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>--%>

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



                                                                <NoRecordsTemplate>
                                                                    <div>
                                                                        Nessuna Attività Presente</div>
                                                                </NoRecordsTemplate>
                                                                <Columns>

                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" UniqueName="Id" Visible="False" />
                                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                        AllowFiltering="False" ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                                runat="server"></asp:CheckBox>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                                runat="server"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                  <%--  <telerik:GridTemplateColumn HeaderStyle-Width="20px" ItemStyle-Width="20px" AllowFiltering="false">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="ModuloImage" Style="display: inline-block" Width="15px" runat="Server"
                                                                                ImageUrl='<%# Eval("LogoModulo") %>' ToolTip='<%# "Istanza ID: " & Eval("Id")%>' />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>--%>

                                                                    <telerik:GridTemplateColumn SortExpression="DescrizioneDocumento" UniqueName="DescrizioneDocumento"
                                                                        HeaderText="Documento" DataField="DescrizioneDocumento" 
                                                                         AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                        FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="NoteImage" Style="text-align: left" ImageAlign="Left" Width="15px"
                                                                                runat="Server" ImageUrl="~\images\Note.png" ToolTip='<%# Eval("Note")%>' Visible='<%# CBool(Eval("Note") <> "").ToString %>' />
                                                                            <div title='<%# Replace(Eval("DescrizioneDocumento"), "'", "&#039;")%>' style="border: 0px solid red; margin-left: 30px">
                                                                                <%# GetDescrizioneDocumento(Container.DataItem)%>
                                                                               
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridButtonColumn Text="Esegui operazione corrente" ButtonType="ImageButton"
                                                                        ItemStyle-HorizontalAlign="Center" ImageUrl="~/images/new/attivita.gif" CommandName="Execute"
                                                                        UniqueName="Execute" ItemStyle-Width="45px" HeaderStyle-Width="45px">
                                                                    </telerik:GridButtonColumn>

                                                                   


                                                                    <telerik:GridTemplateColumn SortExpression="Proponente" UniqueName="Proponente"
                                                                        HeaderText="Proponente/Mittente" DataField="Proponente" HeaderStyle-Width="240px"
                                                                        ItemStyle-Width="240px" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                        FilterControlWidth="100%" ShowFilterIcon="False" AllowFiltering="true">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Replace(Eval("Interlocutore"), "'", "&#039;")%>' style="width: 240px;
                                                                                border: 0px solid red">
                                                                                <%# Eval("Proponente")%>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="TaskCorrente" UniqueName="TaskCorrente"
                                                                        HeaderText="Stato" DataField="TaskCorrente" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                                                                        AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                        ShowFilterIcon="False" AllowFiltering="true">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("TaskCorrente")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 100px; border: 0px solid red">
                                                                                <%# Eval("TaskCorrente")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                                        AllowFiltering="false">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="DirezioneImage" Style="display: inline-block" Width="15px" runat="Server"
                                                                                ImageUrl='<%# Eval("PathDirezione") %>' ToolTip='<%# String.Concat(Eval("TooltipDirezione"), " Istanza ID: ", Eval("Id"))%>'  />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    

                                                                     <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Attachment" ImageUrl="~\images\attachment.png"
                                                                            UniqueName="Attachment" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-VerticalAlign="Middle"
                                                                            ItemStyle-HorizontalAlign="Center" />




                                                                   <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="PreviewEmail" ImageUrl="~\images\email20.png"
                                                                            UniqueName="PreviewEmail" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-VerticalAlign="Middle"
                                                                            ItemStyle-HorizontalAlign="Center" />
                                                                     

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                        Text="Visualizza documento" ImageUrl="~\images\knob-search16.png" UniqueName="Preview"
                                                                        ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                    </telerik:GridButtonColumn>

                                                                    <telerik:GridBoundColumn DataField="DataInizio" DataFormatString="{0:dd/MM/yyyy}"
                                                                        Visible="false" />

                                                                    <telerik:GridBoundColumn DataField="TaskSuccessivo" HeaderText="Successivo" Visible="false" />
                                                                    <telerik:GridBoundColumn DataField="TaskPrecedente" Visible="False" />
                                                                    <telerik:GridBoundColumn DataField="IdModello" Visible="false" />
                                                                    <telerik:GridBoundColumn DataField="NomeFileIter" Visible="false" UniqueName="NomeFileIter" />
                                                                    <telerik:GridBoundColumn DataField="IdDocumento" Visible="false" UniqueName="IdDocumento" />
                                                                    <telerik:GridBoundColumn DataField="IdModulo" Visible="false" UniqueName="IdModulo" />
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
                    </table>

          

                

                    <asp:ImageButton ID="AggiornaTaskButton" runat="server" ImageUrl="~/images//knob-search16.png"
                        Style="display: none" />
                    <asp:ImageButton ID="SbloccaTaskButton" runat="server" ImageUrl="~/images//knob-search16.png"
                        Style="display: none" />

                      <asp:ImageButton ID="ResettaFiltroButton" runat="server" ImageUrl="~/images//knob-search16.png"  Style="display: none" />

                         <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />

                            <asp:ImageButton ID="continuaSenzaIntegrazioneButton" runat="server" ImageUrl="~/images//knob-search16.png"
                Style="display: none; width: 0px" />

                </center>


                
                <div id="taskPanel" style="position: absolute; width: 100%; text-align: center; z-index: 2000000;
                    display: none; top: 0px">
                    <div id="containerPanel" style="width: 650px; text-align: center; background-color: #BFDBFF;
                        margin: 0 auto">
                        <parsec:OperazioneControl runat="server" ID="OperazioneControl" />
                    </div>
                </div>

                <%--<div id="taskPanelSUAPE" style="position: absolute; width: 100%; text-align: center;
                    z-index: 2000000; display: none; top: 0px">
                    <div id="containerPanelSUAPE" style="width: 650px; text-align: center; background-color: #BFDBFF;
                        margin: 0 auto">
                        <parsec:OperazioneControlSUAPE runat="server" ID="OperazioneControlSUAPE" />
                    </div>
                </div>--%>


                <div id="EmailPanel" style="position: absolute; width: 100%; text-align: center; z-index: 2000000; display: none; top: 0px">
                    <div id="ShadowEmailPanel" style="width: 800px; text-align: center; background-color: #BFDBFF; margin: 0 auto">
                        <parsec:VisualizzaEmailControl runat="server" ID="VisualizzaEmailControl" />
                    </div>
                </div>


              
                  <div id="FatturaPanel" style="position: absolute; width: 100%; text-align: center; z-index: 2000000; display: none; top: 0px">
                    <div id="ShadowFatturaPanel" style="width: 800px; text-align: center; background-color: #BFDBFF; margin: 0 auto">
                        <parsec:VisualizzaFatturaControl runat="server" ID="VisualizzaFatturaControl" />
                    </div>
                </div>


              <div id="searchPanel" style="position: absolute; width: 100%; text-align: center; z-index:2000000; display:none; top:200px">
                <div id="searchContainerPanel" style="width:430px; text-align: center; background-color: #BFDBFF;margin: 0 auto">
                     <table width="430px" cellpadding="5" cellspacing="5" border="0">
                       <tr>
                           <td>
                               <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                 <%--  HEADER--%>

                                   <tr>
                                       <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                           border-top: 1px solid  #9ABBE8; height: 25px">
                                           <table style="width: 100%">
                                               <tr>
                                                   <td>
                                                       <asp:Label ID="TitoloRicercaLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                           Text="Ricerca Attività" CssClass="Etichetta" />
                                                   </td>
                                                   <td align="right">
                                                       <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HideSearchPanel();hideSearch=true;document.getElementById('<%= Me.ResettaFiltroButton.ClientID %>').click();" />
                                                   </td>
                                               </tr>
                                           </table>
                                       </td>
                                   </tr>



                                 <%-- BODY--%>
                                 <tr>
                                     <td class="ContainerMargin">
                                         <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                            <tr>
                                                <td>
                                                <div style="overflow: auto; height: 120px; width: 100%; background-color: #DFE8F6;
                                                    border: 0px solid #5D8CC9;">
                                                  
                                                  <%--CONTENUTO--%>

                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                &nbsp;<asp:Label ID="ModuloLabel" runat="server" CssClass="Etichetta" Text="Modulo" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ToolTip="Modulo dell'iter" ID="ModuliComboBox" runat="server"
                                                                    Skin="Office2007" Width="270px" EmptyMessage="- Seleziona Modulo -" ItemsPerRequest="10"
                                                                    AutoPostBack="true" Filter="StartsWith" MaxHeight="400px" NoWrap="True" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipologia" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ToolTip="Tipologia di documento" ID="TipologiaDocumentoComboBox"
                                                                    runat="server" Skin="Office2007" Width="270px" EmptyMessage="- Selezionare -"
                                                                    ItemsPerRequest="10" Filter="StartsWith" MaxHeight="400px" NoWrap="True" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Stato" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ToolTip="Stato dell'iter" ID="StatoComboBox" runat="server"
                                                                    Skin="Office2007" Width="270px" EmptyMessage="- Selezionare -" ItemsPerRequest="10"
                                                                    Filter="StartsWith" MaxHeight="400px" NoWrap="True" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 90px">
                                                                <asp:Label ID="DataInizioFiltroLabel" runat="server" CssClass="Etichetta" Text="Data Inizio" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td align="center" style="width: 30px">
                                                                            <asp:Label ID="DataInizioIstanzaFiltroLabel" runat="server" CssClass="Etichetta"
                                                                                Text="da *" />
                                                                        </td>
                                                                        <td style="width: 100px">
                                                                            <telerik:RadDatePicker ID="DataInizioIstanzaFiltroTextBox" runat="server" MinDate="1753-01-01"
                                                                                Skin="Office2007" ToolTip="Data Inizio Istanza" Width="100px">
                                                                                <Calendar ID="Calendar1" runat="server">
                                                                                    <SpecialDays>
                                                                                        <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                                    </SpecialDays>
                                                                                </Calendar>
                                                                                <DatePopupButton ToolTip="Apri il calendario per selezionare Data Inizio Istanza." />
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                        <td align="center" style="width: 30px">
                                                                            <asp:Label ID="DataFineIstanzaFiltroLabel" runat="server" CssClass="Etichetta" Text="a *" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="DataFineIstanzaFiltroTextBox" runat="server" MinDate="1753-01-01"
                                                                                Skin="Office2007" ToolTip="Data Fine Istanza" Width="100px">
                                                                                <Calendar ID="Calendar2" runat="server">
                                                                                    <SpecialDays>
                                                                                        <telerik:RadCalendarDay Date="" ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                                    </SpecialDays>
                                                                                </Calendar>
                                                                                <DatePopupButton ToolTip="Apri il calendario per selezionare Data Fine Istanza." />
                                                                            </telerik:RadDatePicker>
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
                                </td>
                            </tr>
                            <%-- FOOTER--%>

                                   <tr>
                                       <%--PULSANTI--%>
                                       <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                           border-top: 1px solid  #9ABBE8; height: 25px">
                                           <telerik:RadButton ID="ApplicaFiltroButton" runat="server" Text="Ok" Width="90px"
                                               Skin="Office2007" ToolTip="Effettua la ricerca con i filtri impostati">
                                               <Icon PrimaryIconUrl="../../../../images/check16.png" PrimaryIconLeft="5px" />
                                           </telerik:RadButton>
                                           &nbsp;
                                           <telerik:RadButton ID="SalvaFiltroButton" runat="server" Text="   Salva Filtro" Width="90px"
                                               Skin="Office2007" ToolTip="Salva i filtri impostati">
                                               <Icon PrimaryIconUrl="../../../../images/save16.png" PrimaryIconLeft="5px" />
                                           </telerik:RadButton>
                                           &nbsp;
                                           <telerik:RadButton ID="AnnullaFiltroButton" runat="server" Text="Annulla" Width="90px"
                                               Skin="Office2007" ToolTip="Annulla i filtri impostati">
                                               <Icon PrimaryIconUrl="../../../../images/Annulla.png" PrimaryIconLeft="5px" />
                                           </telerik:RadButton>
                                       </td>
                                   </tr>
                        </table>
                    </td>
                </tr>
            </table>
                </div>
            </div>




               
            </div>

             

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
