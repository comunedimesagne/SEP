<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="FascicoliPage.aspx.vb" Inherits="FascicoliPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');
        var overlay = document.createElement("div");

        var hide = true;
        var hideUpload = true;

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest); 
            manager.add_endRequest(OnEndRequest);

           $get("pageContent").appendChild(overlay);
           $get("pageContent").appendChild(_backgroundElement);

            if (hide) {
                HidePanel();

            } else {
                ShowPanel();
            }


            if (hideUpload) {
                HideUploadPanel();

            } else {
                ShowUploadPanel();
            }

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
                style.width = '300px';
                style.height = '40px';
                style.backgroundColor = '#BFDBFF';
                style.border = 'solid #4892FF 2px';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.zIndex = 10000;
                innerHTML = message;
                style.color = '#00156E';
                style.backgroundImage = 'url(/sep/Images/success.png)';
                style.backgroundPosition = '10px center';
                style.backgroundRepeat = 'no-repeat';
                style.padding = '15px 10px 15px 50px';
                style.margin = '15px 0px';
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

        }

                
        function OnClientButtonClicking(sender, args) {
            var button = args.get_item();
            var commandName = button.get_commandName();
            if (commandName == "Stampa") {
                args.set_cancel(true);
            }
        }





        function HideUploadPanel() {
            var panel = document.getElementById("UploadPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
            _backgroundElement.style.display = 'none';
        }

        function ShowUploadPanel() {
            overlay.style.display = '';
            var panel = document.getElementById("UploadPanel");
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

            var shadow = document.getElementById("UploadShadowPanel");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }

        }


        StopPropagation = function (e) {
            e.cancelBubble = true;
            if (e.stopPropagation) {
                e.stopPropagation();
            }
        }


       

        function removeLastComma(str) {
            return str.replace(/,$/, "");
        }



        function OnCheckBoxClick(chk) {
            var combo = $find('<%= TipologiaFascicoloComboBox.ClientId %>');
           
            var chkall = $get(combo.get_id() + '_Header_SelectAll');
            chkall.checked = AllSelected();

            var text = '';
            var values = '';
            var items = combo.get_items();

            for (var i = 0; i < items.get_count(); i++) {
                
                var chk1 = $get(combo.get_id() + '_i' + i + '_chk1');

                if (chk1.checked) {
                    var item = items.getItem(i);
                    text += item.get_text() + ',';
                    values += item.get_value() + ',';
                }
            }

            text = removeLastComma(text);
            values = removeLastComma(values);

            if (text.length > 0) {
                combo.set_text(text);
            } else {
                combo.set_text('');
            }
        }

        function AnyOneSelected() {
            var combo = $find('<%= TipologiaFascicoloComboBox.ClientId %>');
            var items = combo.get_items();
            for (var i = 0; i < items.get_count(); i++) {
                var chk1 = $get(combo.get_id() + '_i' + i + '_chk1');
                if (chk1.checked) { return true; }
            }
            return false;
        }


        function AllSelected() {
            var combo = $find('<%= TipologiaFascicoloComboBox.ClientId %>');
            var items = combo.get_items();
            for (var i = 0; i < items.get_count(); i++) {
                var chk1 = $get(combo.get_id() + '_i' + i + '_chk1');
                if (chk1.checked == false) { return false; }
            }
            return true;
        }


        function OnSelectAllClick(chk) {
            var selectAll = true;
            if (AnyOneSelected() == true) selectAll = true;
            if (AllSelected() == true) selectAll = false;
            var text = '';
            var values = '';
            var combo = $find('<%= TipologiaFascicoloComboBox.ClientId %>');
            var items = combo.get_items();
            for (var i = 0; i < items.get_count(); i++) {
              
                var chk1 = $get(combo.get_id() + '_i' + i + '_chk1');
                if (selectAll) {
                    chk1.checked = true;
                } else {
                    chk1.checked = false;
                }

                if (chk1.checked) {
                    var item = items.getItem(i);
                    text += item.get_text() + ',';
                    values += item.get_value() + ',';
                }
            }
            text = removeLastComma(text);
            values = removeLastComma(values);
            if (text.length > 0) combo.set_text(text); else combo.set_text('');
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

            <div id="pageContent" >

               <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>

                <table style="width: 900px; border: 1px solid #5D8CC9">

                    <tr>
                        <td>

                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" OnClientButtonClicking="OnClientButtonClicking" Width="100%">
                                            <Items>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                                    CommandName="Nuovo" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                    CommandName="Trova" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>

                                                 <telerik:RadToolBarDropDown runat="server" ImageUrl="~/images/Printer.png" Text="Stampa">
                                                    <Buttons>
                                                        <telerik:RadToolBarButton runat="server" CommandName="StampaRegistro"
                                                            Text="Stampa registro..." Width="290px" />

                                                         <telerik:RadToolBarButton runat="server" CommandName="StampaElencoDocumenti"
                                                            Text="Stampa elenco documenti" Width="290px" />
                                                    </Buttons>
                                                </telerik:RadToolBarDropDown>



                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>

                                                 <telerik:RadToolBarButton runat="server" ImageUrl="~/images/AdvancedSearch32.png" Text="Ricerca Avanzata"
                                                    CommandName="RicercaAvanzata" Owner="RadToolBar" />

                                                 <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore2" Owner="RadToolBar" />

                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>


                         
                            <div id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">
                             
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100px">

                                                   <asp:Label ID="TipoFascicoloLabel" runat="server" CssClass="Etichetta" Text="Tipo Fascicolo *"
                                                ForeColor="#FF8040" ToolTip="Tipo Fascicolo" />
                                        </td>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>

                                                  <td style="width: 270px;">

                                                         <telerik:RadComboBox ID="TipologiaFascicoloComboBox" runat="server" EmptyMessage="Seleziona Tipo Fascicolo"
                                                Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007" AutoPostBack="true"
                                                Width="270px" Enabled="true" ToolTip="Tipo di fascicolo" />

                                                </td>
                                                   
                                                    <td style="width: 120px; text-align:center">
                                                        <asp:Label ID="CodiceFascicoloLabel" runat="server" CssClass="Etichetta" 
                                                            Text="Cod. Fascicolo *" ForeColor="#FF8040"
                                                            ToolTip="Codice del Fascicolo" />
                                                    </td>
                                                    <td style=" width:150px">

                                                        <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                            border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 19px">
                                                            <asp:Label ID="CodiceFascicoloSistemaTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                runat="server" Width="140px" ToolTip="Parte del codice del fascicolo impostato automaticamente dal sistema">&nbsp;</asp:Label>
                                                        </span>
                                                    </td>

                                                    <td>
                                                        <telerik:RadTextBox ID="CodiceFascicoloUtenteTextBox" runat="server" Skin="Office2007"
                                                            Width="230px" 
                                                            ToolTip="Parte del codice del fascicolo che può essere inserito dall'utente">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                                <table style="width: 100%">
                                    <tr>
                                       <td style="width: 100px">
                                            <asp:Label ID="DataLabel" runat="server" CssClass="Etichetta" Text="Apertura *" ForeColor="#FF8040" />
                                        </td>
                                          <td style="width: 100px">
                                            <telerik:RadDatePicker ID="DataTextBox" runat="server" MinDate="1753-01-01" Skin="Office2007"
                                                Width="100px" ToolTip="Data di apertura del fascicolo">
                                                <Calendar ID="Calendar1" runat="server">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                                  <DatePopupButton ToolTip="Apri il calendario." />
                                            </telerik:RadDatePicker>



                                         
                                        <td style="width: 60px; text-align:center">
                                            <asp:Label ID="Label4" runat="server" CssClass="Etichetta" Text="Chiusura" ForeColor="#FF8040" />
                                        </td>
                                         <td style="width: 100px">
                                            <telerik:RadDatePicker ID="DataChiusuraTextBox" runat="server" MinDate="1753-01-01"
                                                Skin="Office2007" Width="100px" ToolTip="Data di chiusura del fascicolo">
                                                <Calendar ID="Calendar2" runat="server">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                                 <DatePopupButton ToolTip="Apri il calendario." />
                                            </telerik:RadDatePicker>
                                        </td>

                                           <td style="width: 105px">
                                            <asp:Label ID="ClassificazioneLabel" runat="server" CssClass="Etichetta" 
                                                Text="Classificazione *" ForeColor="#FF8040" />
                                        </td>
                                        <td >
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 320px">
                                                        

                                                               <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                            border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 340px; height: 19px">
                                                            <asp:Label ID="ClassificazioneTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                runat="server" Width="350px" ToolTip="Indice di classificazione completo">&nbsp;</asp:Label>
                                                        </span>
                                                    </td>
                                                    <td style="width: 25px; text-align: center">
                                                        <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                            ToolTip="Seleziona classificazione..." ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td style="width: 25px; text-align: center">
                                                        <asp:ImageButton ID="EliminaClassificazioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                            ToolTip="Cancella classificazione" ImageAlign="AbsMiddle" />
                                                        <asp:ImageButton ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                            Style="display: none; width: 0px" />
                                                        <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                            Style="display: none; width: 0px" />
                                                            
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                     
                                       
                                    </tr>
                                </table>


                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100px">
                                            <asp:Label ID="TipoProcedimentoLabel" runat="server" CssClass="Etichetta" Text="Tipo Proced. *"
                                                ToolTip="Tipo Procedimento" ForeColor="#FF8040" />
                                        </td>
                                        <td style="width: 110px">
                                            <telerik:RadComboBox ID="ProcedimentoComboBox" runat="server" EmptyMessage="Seleziona Tipo Procedimento"
                                                Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                Width="300px" Enabled="true" ToolTip="Tipo di procedimento" />
                                        </td>
                                        <td style="width: 110px; text-align:center">
                                            <asp:Label ID="ResponsabileLabel" runat="server" CssClass="Etichetta" Text="Responsabile *"
                                                ForeColor="#FF8040" />
                                        </td>
                                        <td style="width: 295px">

                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 290px; height: 19px">
                                                <asp:Label ID="ResponsabileTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                    runat="server" Width="305px" ToolTip="Nominativo del responsabile del procedimento">&nbsp;</asp:Label>
                                            </span>

                                        </td>
                                        <td style="width: 25px; text-align: center">
                                            <asp:ImageButton ID="TrovaResponsabileImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                ToolTip="Seleziona responsabile..." ImageAlign="AbsMiddle" />
                                        </td>
                                        <td style="width: 25px; text-align: center">
                                            <asp:ImageButton ID="EliminaResponsabileImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                ToolTip="Cancella responsabile" ImageAlign="AbsMiddle" />
                                            <asp:ImageButton ID="AggiornaResponsabileImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                Style="display: none; width: 0px" />
                                            <telerik:RadTextBox ID="IdResponsabileTextBox" runat="server" Skin="Office2007" Style="display: none;
                                                width: 0px" />

                                               
                                        </td>
                                       
                                    </tr>
                                </table>

                              
                                  <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100px">
                                            <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" 
                                                Text="Oggetto *" ForeColor="#FF8040" />
                                        </td>
                                        <td style="width: 360px">
                                            <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" ToolTip="Oggetto del procedimento" Width="350px"
                                                Rows="3" TextMode="MultiLine" MaxLength="1000" Style="overflow-x: hidden"  />
                                        </td>
                                        <td style="text-align:center">
                                            <asp:Label ID="NoteLabel" runat="server" CssClass="Etichetta" Text="Note" />
                                        </td>
                                        <td style="width: 360px">
                                            <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="350px"
                                                Rows="3" TextMode="MultiLine" MaxLength="1000" 
                                                Style="overflow-x: hidden" ToolTip="Note del fascicolo" />
                                        </td>
                                    </tr>
                                </table>

                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100px">
                                            <asp:Label ID="Label10" runat="server" CssClass="Etichetta" Text="Provv. Finale" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="numeroProvvedimentoChiusuraTextbox" runat="server" Skin="Office2007"
                                                Width="400px" MaxLength="100" ToolTip="Provvedimento finale" />
                                        </td>
                                    </tr>
                                </table>

                            </div>


                           <telerik:RadTabStrip runat="server" ID="DatiFascicoloStrip" SelectedIndex="0" MultiPageID="DatiMultiPage" Skin="Office2007" Width="100%">
                                <Tabs>
                                    <telerik:RadTab Text="Amministrazioni" Selected="True"/>
                                    <telerik:RadTab Text="Documenti"/>
                                    <telerik:RadTab Text="Visibilità"/>
                                  
                                </Tabs>
                            </telerik:RadTabStrip>
                            <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                       <telerik:RadMultiPage runat="server" ID="DatiMultiPage" SelectedIndex="0"   Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                

                                  <telerik:RadPageView runat="server" ID="AmministrazioniPageView" CssClass="corporatePageView"   Height="250px" >
                                    <div id="AmministrazioniPanel" runat="server" style="padding: 2px 2px 2px 2px; width: 99.8%;  height: 250px">

                                    <table style="width: 100%">
                                           <tr>
                                              <td  style=" vertical-align:top">
                                                   <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                       <tr style="height: 30px; background-color: #BFDBFF">
                                                           <td>
                                                               <table style="width: 100%">
                                                                   <tr>
                                                                       <td style="width: 80px">
                                                                           <asp:Label ID="CategorieAmministrazioniLabel" runat="server" Font-Bold="True" CssClass="Etichetta"
                                                                               Text="Categorie" ForeColor="#00156E" />
                                                                       </td>

                                                                       <td style="width: 350px">
                                                                           <telerik:RadComboBox ID="CategorieAmministrazioneComboBox" runat="server" Width="350px"
                                                                               Height="150" EmptyMessage="Seleziona Categoria Amministrazione" EnableAutomaticLoadOnDemand="True"
                                                                               ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                                               Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                                                               <WebServiceSettings Method="GetCategorieAmministrazioni" Path="FascicoliPage.aspx" />
                                                                           </telerik:RadComboBox>
                                                                       </td>

                                                                     

                                                                       <td style="width: 120px; text-align: center">
                                                                           <asp:Label ID="ChiaveRicercaLabel" runat="server" ForeColor="#00156E"  Font-Bold="True" CssClass="Etichetta" Text="Chiave ricerca" />
                                                                       </td>

                                                                       <td>
                                                                           <telerik:RadTextBox ID="ChiaveRicercaTextBox" runat="server" Skin="Office2007" Width="270px" />
                                                                       </td>

                                                                       <td style=" width:0px">
                                                                           <asp:ImageButton ID="AggiornaAmministrazioneImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                               Style="display: none" />
                                                                       </td>

                                                                       <td style="width: 25px; text-align: center">
                                                                           <asp:ImageButton ID="TrovaAmministrazioneImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                               ToolTip="Seleziona Amministrazione Partecipante ..." ImageAlign="AbsMiddle" />
                                                                       </td>

                                                                       
                                                                   </tr>
                                                               </table>
                                                           </td>
                                                       </tr>
                                                       <tr>
                                                           <td>
                                                               <div id="DivAmministrazioni" runat="server" style="overflow: auto; height: 205px; border: 1px solid #5D8CC9;
                                                                   background-color: White">
                                                                 <telerik:RadGrid ID="AmministrazioniGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                        CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" PageSize="6"
                                                                        Culture="it-IT">
                                                                        <MasterTableView DataKeyNames="Id">
                                                                            <Columns>

                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32"
                                                                                    FilterControlAltText="Filter Id column" HeaderText="Id" ReadOnly="True"
                                                                                    SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                               
                                                                                <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                    HeaderText="Denominazione" DataField="Descrizione">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 405px; border:0px solid red">
                                                                                            <%# Eval("Descrizione")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                               
                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                    HeaderStyle-Width="20px" ItemStyle-Width="20px" ImageUrl="~\images\Delete16.png"
                                                                                    UniqueName="Delete" ConfirmText="Sei sicuro di voler eliminare l'Amministrazione selezionata?" Text="Elimina Amministrazione" />
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

                                    </div>
                                  </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="DocumentiPageView" CssClass="corporatePageView"  Height="250px" >


                                    <div id="DocumentiPanel" runat="server" style="padding: 2px 2px 2px 2px; width: 99.8%; height: 250px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style=" vertical-align:top">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr style="height: 30px; background-color: #BFDBFF">
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width:40px">
                                                                            <asp:Label Font-Bold="True" ID="Label5" runat="server" Style="color: #00156E; background-color: #BFDBFF;
                                                                                width: 40px" CssClass="Etichetta" Text="Fase" />
                                                                        </td>
                                                                        <td style="width:150px">
                                                                            <telerik:RadComboBox ID="FaseDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                EmptyMessage="Seleziona Fase" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="200px" Width="150px" />
                                                                        </td>

                                                                       <td style="width:80px;text-align:center">
                                                                            <asp:Label Font-Bold="True" ID="Label1" runat="server" Style="color: #00156E; background-color: #BFDBFF;
                                                                                width: 80px" CssClass="Etichetta" Text="Categoria" />
                                                                        </td>
                                                                        <td style="width:230px">
                                                                            <telerik:RadComboBox ID="CategoriaComboBox" runat="server" Skin="Office2007"
                                                                                EmptyMessage="Seleziona Categoria" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="200px"  Width="230px" />
                                                                        </td>



                                                                        <td style="width:40px; text-align:center">
                                                                            <asp:Label Font-Bold="True" ID="Label6" runat="server" Style="color: #00156E; background-color: #BFDBFF;
                                                                                width: 50px" CssClass="Etichetta" Text="Tipo" />
                                                                        </td>
                                                                        <td style="width:260px">
                                                                            <telerik:RadComboBox ID="TipoDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                Width="260px" EmptyMessage="Seleziona Tipologia Documento" ItemsPerRequest="10" Filter="StartsWith"
                                                                                MaxHeight="400px" ToolTip="Tipologie di documenti da associare al fascicolo" />
                                                                        </td>
                                                                        <td style="width: 20px">
                                                                            <asp:ImageButton ID="TrovaDocumentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona documento..." ImageAlign="AbsMiddle" 
                                                                                style="width: 16px" />
                                                                        </td>

                                                                        <td>
                                                                            <asp:ImageButton ID="EliminaDocumentiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella documenti" ImageAlign="AbsMiddle" />
                                                                            <asp:ImageButton ID="AggiornaDocumentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none; width: 0px" />
                                                                        </td>
                                                                       
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="documentiScrollPanel" runat="server" style="overflow: auto; height: 205px;
                                                                    border: 1px solid #5D8CC9; background-color: White">
                                                                  
                                                                    <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco documenti associati al fascicolo"
                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007" Culture="it-IT"
                                                                        PageSize="6" AllowPaging="true" AllowSorting="true" AllowFilteringByColumn="true">
                                                                        <GroupingSettings CaseSensitive="false" /> 
                                                                        <MasterTableView DataKeyNames="Id">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />

                                                                                  <telerik:GridBoundColumn DataField="IdDocumento" DataType="System.Int32" FilterControlAltText="Filter IdDocumento column"
                                                                                    HeaderText="IdDocumento" ReadOnly="True" SortExpression="IdDocumento" UniqueName="IdDocumento" Visible="False" />







                                                                                <telerik:GridTemplateColumn SortExpression="DescrizioneTipoDocumento" UniqueName="DescrizioneTipoDocumento"
                                                                                    AutoPostBackOnFilter="True"
                                                                                    CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                    FilterControlAltText="Filter DescrizioneTipoDocumento column" AllowFiltering="true" HeaderText="Tipo" DataField="DescrizioneTipoDocumento" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("DescrizioneTipoDocumento")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 80px;">
                                                                                            <%# Eval("DescrizioneTipoDocumento")%>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn SortExpression="DescrizioneFase" UniqueName="DescrizioneFase"
                                                                                    AutoPostBackOnFilter="True"
                                                                                    CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                    FilterControlAltText="Filter DescrizioneFase column" AllowFiltering="true" HeaderText="Fase" DataField="DescrizioneFase" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("DescrizioneFase")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 80px;">
                                                                                            <%# Eval("DescrizioneFase")%>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>


                                                                                <telerik:GridTemplateColumn SortExpression="DescrizioneCategoria" UniqueName="DescrizioneCategoria"
                                                                                    AutoPostBackOnFilter="True"
                                                                                    CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                    FilterControlAltText="Filter DescrizioneCategoria column" AllowFiltering="true" HeaderText="Categoria" DataField="DescrizioneCategoria" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("DescrizioneCategoria")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 80px;">
                                                                                            <%# Eval("DescrizioneCategoria")%>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn SortExpression="NomeDocumentoOriginale" UniqueName="NomeDocumentoOriginale"
                                                                                    AutoPostBackOnFilter="True"
                                                                                    CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                    FilterControlAltText="Filter NomeDocumentoOriginale column" AllowFiltering="true"
                                                                                    HeaderText="Documento" DataField="NomeDocumentoOriginale">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Replace(DescrizioneDocumentoTooltip(Container.DataItem), "'", "&#039;") %>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                                                                            <%# Eval("NomeDocumentoOriginale")%>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                               <%-- <telerik:GridTemplateColumn SortExpression="DataDocumento" UniqueName="DataDocumento"
                                                                                    AutoPostBackOnFilter="True"
                                                                                    CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                                    FilterControlAltText="Filter DataDocumento column" AllowFiltering="true"
                                                                                    HeaderText="Inserito il" DataField="DataDocumento" HeaderStyle-Width="75px"
                                                                                    ItemStyle-Width="75px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("DataDocumento","{0:dd/MM/yyyy}")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 75px;">
                                                                                            <%# Eval("DataDocumento", "{0:dd/MM/yyyy}")%>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>--%>

                                                                            <telerik:GridTemplateColumn SortExpression="DataDocumentoSenzaOrario" UniqueName="DataDocumentoSenzaOrario"
                                                                            HeaderText="Inserito il" DataField="DataDocumentoSenzaOrario" HeaderStyle-Width="80px"
                                                                            ItemStyle-Width="80px" AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" 
                                                                            FilterControlWidth="100%" ShowFilterIcon="false" FilterControlAltText="Filter DataDocumentoSenzaOrario column"
                                                                            AllowFiltering="true" >
                                                                               <FilterTemplate>
                                                                                <telerik:RadDatePicker  ID="DataInserimentoTextBox" Skin="Office2007" ShowPopupOnFocus="true" DatePopupButton-Visible="false"
                                                                                    Width="100%" runat="server" MinDate="1753-01-01" ClientEvents-OnDateSelected="DateSelected" 
                                                                                    DbSelectedDate='<%#DataDocumentoSenzaOrario(Container) %>' 
                                                                                    DateInput-ClientEvents-OnKeyPress="OnDataInserimentoTextBoxKeyPressed"/>
                                                                                   <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">                                                                                      
                                                                                       <script type="text/javascript">                                                                                         
                                                                                           function OnDataInserimentoTextBoxKeyPressed(sender, args) {                                                                                             
                                                                                               if (13 == args.get_keyCode()) {
                                                                                                 
                                                                                                   var tableView = $find("<%# CType(Container,GridItem).OwnerTableView.ClientID %>");
                                                                                                   var c = sender.get_textBoxValue();                                                                                                 
                                                                                                   if (Date.parse(c)) {                                                                                                       
                                                                                                       //tableView.filter("DataInserimento", c, "EqualTo");
                                                                                                       var idPicker = sender.get_element().parentNode.parentNode.children(0).id;
                                                                                                       var picker = $find(idPicker)
                                                                                                       var ddmmyyyy = c.split('/');
                                                                                                       var mmddyyyy = ddmmyyyy[1] + '/' + ddmmyyyy[0] + '/' + ddmmyyyy[2];

                                                                                                       picker.set_selectedDate(new Date(mmddyyyy));
                                                                                                       DateSelected(picker, "");
                                                                                                   }                                                                                                                                                                                            
                                                                                                   args.set_cancel(true);                                                                                                                                                                                                   
                                                                                               }
                                                                                           }

                                                                                           function DateSelected(sender, args) {
                                                                                             
                                                                                               var tableView = $find("<%# CType(Container, GridItem).OwnerTableView.ClientID %>");
                                                                                               var date = FormatSelectedDate(sender);                                                                                             
                                                                                               tableView.filter("DataDocumentoSenzaOrario", date, "EqualTo");
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
                                                                                <div title='<%# Eval("DataDocumentoSenzaOrario", "{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; text-overflow: ellipsis; width:80px; border: 0px solid red">
                                                                                    <%# Eval("DataDocumentoSenzaOrario", "{0:dd/MM/yyyy}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn> 






                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Stato" FilterControlAltText="Filter Stato column"
                                                                                    ImageUrl="~\images\vuoto.png" UniqueName="Stato" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                    ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="20px"
                                                                                    ItemStyle-Width="20px" />
                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="20px"
                                                                                    ItemStyle-Width="20px" />
                                                                            </Columns>
                                                                        </MasterTableView></telerik:RadGrid></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>


                                </telerik:RadPageView>


                               <telerik:RadPageView runat="server" ID="VisibilitaPageView" CssClass="corporatePageView"   Height="250px">

                                   <div id="VisibilitaPanel" runat="server" style="padding: 2px 2px 2px 2px; width: 99.8%;  height: 250px">
                                       <table style="width: 100%">
                                           <tr>
                                               <td valign="top" style="width: 50%">
                                                   <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                       <tr style="height: 30px; background-color: #BFDBFF">
                                                           <td>
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
                                                                                   ToolTip="Aggiungi Gruppo..." ImageAlign="AbsMiddle" BorderStyle="None" />&nbsp;&nbsp;<asp:ImageButton
                                                                                       ID="AggiornaGruppoVisibilitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                       Style="display: none" /><asp:ImageButton ID="AggiornaUtenteVisibilitaImageButton"
                                                                                           runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                                                                       </td>
                                                                   </tr>
                                                               </table>
                                                           </td>
                                                       </tr>
                                                       <tr>
                                                           <td>
                                                               <div id="Div1" runat="server" style="overflow: auto; height: 205px; border: 1px solid #5D8CC9;
                                                                   background-color: White">
                                                                   <telerik:RadGrid ID="VisibilitaGridView" runat="server" ToolTip="Elenco utenti o gruppi associati al protocollo"
                                                                       AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                       AllowPaging="true" PageSize="6" Culture="it-IT">
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
                                                                                   ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                                   ItemStyle-VerticalAlign="Middle" UniqueName="Delete" ButtonType="ImageButton"
                                                                                   CommandName="Delete" />

                                                                           </Columns>
                                                                       </MasterTableView></telerik:RadGrid></div>
                                                           </td>
                                                       </tr>
                                                   </table>
                                               </td>
                                           </tr>
                                       </table>
                                   </div>
                               </telerik:RadPageView>

                                

                                </telerik:RadMultiPage>                       
                                
                                                      
                       

                           <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                            <%--  Tabella Griglia--%>
                            <table style="width:100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">

                                    <tr>
                                        <td>
                                             <asp:ImageButton ID="AggiornaFascicoloImageButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />

                                            <table style="background-color: #BFDBFF; width: 100%" >
                                            <tr>

                                                <td>
                                                    &nbsp;<asp:Label ID="TitoloElencoFascicoliLabel" runat="server" Font-Bold="True" Style="color: #00156E;" Text="Elenco Fascicoli" />
                                                </td>
                                                <td  align="center" style="width:30px">
                                                    <asp:ImageButton ID="FiltraImageButton" runat="server" 
                                                        ImageUrl="~/images//search.png" ToolTip="Filtra Fascicoli" 
                                
                                                        style=" border-style: none; border-color: inherit; border-width: 0; width: 16px;" 
                                                        ImageAlign="AbsMiddle" />
                                                 </td>
                                                 <td align="center" style="width:30px">
                                                    <asp:ImageButton ID="RipristinaFiltroInizialeImageButton" style="border:0"  
                                                            runat="server" ImageUrl="~/images//cancelSearch.png" 
                                                            ToolTip="Ripristina filtro iniziale" ImageAlign="AbsMiddle" />
                                                      </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                <tr>
                                    <td>
                                      <div style="overflow: auto; height: 180px; width: 100%; background-color: #FFFFFF; border: 0px solid #5D8CC9;">
                                        <telerik:RadGrid ID="FascicoliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" PageSize ="5"
                                            Culture="it-IT">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column" HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                   

                                                    <telerik:GridTemplateColumn SortExpression="IdTipologiaFascicolo" UniqueName="" HeaderText="Tipo"
                                                                                        DataField="IdTipologiaFascicolo" HeaderStyle-Width="45px" ItemStyle-Width="45px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# IIf(Eval("IdTipologiaFascicolo")=1, "P = Procedimento", "A = Affare")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 45px; border:0px solid red">
                                                                                                <%# IIf(Eval("IdTipologiaFascicolo") = 1, "P", "A")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>



                                                 <%--  <telerik:GridTemplateColumn SortExpression="NumeroRegistro" UniqueName="NumeroRegistro"
                                                        HeaderText="N." DataField="NumeroRegistro" HeaderStyle-Width="45px" ItemStyle-Width="45px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("NumeroRegistro")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 45px; border: 0px solid red">
                                                                <%# Eval("NumeroRegistro")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>--%>


                                                    <telerik:GridTemplateColumn SortExpression="IdentificativoFascicolo" UniqueName="IdentificativoFascicolo"
                                                        HeaderText="Cod. Fascicolo" DataField="CodiceFascicolo" HeaderStyle-Width="145px" ItemStyle-Width="145px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("IdentificativoFascicolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 145px; border: 0px solid red">
                                                                <%# Eval("IdentificativoFascicolo")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn SortExpression="DescrizioneProcedimento" UniqueName="DescrizioneProcedimento" HeaderText="Tipo Procedimento"
                                                        DataField="DescrizioneProcedimento" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DescrizioneProcedimento")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 180px; border: 0px solid red">
                                                                <%# Eval("DescrizioneProcedimento")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                     <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto"
                                                        HeaderText="Oggetto" DataField="Oggetto" >
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 240px; border: 0px solid red">
                                                                <%# Eval("Oggetto")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn SortExpression="DataApertura" UniqueName="DataApertura"
                                                        HeaderText="Apertura" DataField="DataApertura" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DataApertura","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                <%# Eval("DataApertura", "{0:dd/MM/yyyy}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                     <telerik:GridTemplateColumn SortExpression="DataChiusura" UniqueName="DataChiusura"
                                                        HeaderText="Chiusura" DataField="DataChiusura" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DataChiusura","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                <%# Eval("DataChiusura", "{0:dd/MM/yyyy}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                   
                                                   <%--<telerik:GridButtonColumn Text="Seleziona referente e chiudi" FilterControlAltText="Filter ConfirmSelectAndClose column"
                                                                ImageUrl="~/images/accept.png" UniqueName="ConfirmSelectAndClose" ButtonType="ImageButton"
                                                                CommandName="ConfirmSelectAndClose" HeaderStyle-Width="20px" ItemStyle-Width="20px" />--%>

                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                        ImageUrl="~\images\checks.png" UniqueName="Select" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                     
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                    </td>
                                </tr>
                            </table>

                            </div>

                                
                           <asp:Panel runat="server" ID="PannelloChiusura" Style="background-color: #BFDBFF" Visible="false" >
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="center">
                                        </td> 
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

                        </td>
                    </tr>

                </table>

              <asp:HiddenField ID="infoOperazioneHidden" runat="server" />

               <asp:HiddenField ID="infoScansioneHidden" runat="server" />

            </div>


             

            <div id="printPanel" style="position: absolute; width: 100%; text-align: center; z-index:2000000; display:none; top:300px">

                <div id="containerPanel" style="width: 460px; text-align: center; background-color: #BFDBFF;margin: 0 auto">
                     <table width="460px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                            <%--  HEADER--%>
                            <tr>
                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 25px">
                            <table style="width:100%">
                            <tr>
                            <td>
                              <asp:Label ID="Label7" runat="server" Style="color: #00156E" Font-Bold="True" Text="Stampa Registri" CssClass="Etichetta" />
                            </td>
                             
                              <td align="right">
                                <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HidePanel();document.getElementById('<%= Me.ChiudiStampaImageButton.ClientID %>').click();" />
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
                                          
                                                <div style="overflow: auto; height: 120px; width: 100%; background-color: #DFE8F6; border: 0px solid #5D8CC9;">
                                                 
                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="TipoFascicoloFiltroLabel" runat="server" CssClass="Etichetta" Text="Tipo Fascicolo" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="TipologiaFascicoloFiltroComboBox" runat="server" EmptyMessage="Seleziona Tipo Fascicolo"
                                                                    Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                    Width="300px" Enabled="true" ToolTip="Tipo di fascicolo" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="NumeroRegistroFiltroLabel" runat="server" CssClass="Etichetta" Text="N. Registro" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="ContatoreGeneraleInizioFiltroLabel" runat="server" CssClass="Etichetta"
                                                                                Text="da" />
                                                                        </td>
                                                                        <td style="width: 80px">
                                                                            <telerik:RadNumericTextBox ID="ContatoreGeneraleInizioTextBox" runat="server" Skin="Office2007"
                                                                                MaxValue="9999999" MinValue="1" Width="90px" DataType="System.Int32" MaxLength="7">
                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </td>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="ContatoreGeneraleFineFiltroLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadNumericTextBox ID="ContatoreGeneraleFineTextBox" runat="server" Skin="Office2007"
                                                                                MaxValue="9999999" MinValue="1" Width="90px" DataType="System.Int32" MaxLength="7">
                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="AnnoFiltroLabel" runat="server" CssClass="Etichetta" Text="Anno" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="AnnoInizioFiltroLabel" runat="server" CssClass="Etichetta"
                                                                                Text="da" />
                                                                        </td>
                                                                        <td style="width: 80px">
                                                                            <telerik:RadNumericTextBox ID="AnnoInizioFiltroTextBox" runat="server" Skin="Office2007"
                                                                                MaxValue="9999999" MinValue="1" Width="90px" DataType="System.Int32" MaxLength="7">
                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </td>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="AnnoFineFiltroLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadNumericTextBox ID="AnnoFineFiltroTextBox" runat="server" Skin="Office2007"
                                                                                MaxValue="9999999" MinValue="1" Width="90px" DataType="System.Int32" MaxLength="7">
                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
<%--                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="BeneficiarioFiltroLabel" runat="server" CssClass="Etichetta" Text="Titolare" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="BeneficiarioFiltroComboBox" runat="server" Width="290px" Height="150" EmptyMessage="Seleziona Titolare" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                     ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                                                    <WebServiceSettings Method="GetElementiRubrica" Path="FascicoliPage.aspx"  />
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>--%>
                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="ProcedimentoFiltroLabel" runat="server" CssClass="Etichetta" Text="Tipo Procedimento" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="ProcedimentoFiltroComboBox" runat="server" 
                                                                    EmptyMessage="Seleziona Tipo Procedimento" Filter="StartsWith" ItemsPerRequest="10" 
                                                                    MaxHeight="300px" Skin="Office2007" Width="300px" Enabled ="true"/>
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
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 25px">

                                    <telerik:RadButton ID="StampaImageButton" runat="server" Skin="Office2007" Text="Stampa"
                                        Width="100px" ToolTip="Stampa l'elenco dei registri" >
                                        <Icon PrimaryIconUrl="../../../../images/Printer16.png" />
                                    </telerik:RadButton>

                                    <telerik:RadButton ID="EsportaImageButton" runat="server" Text="Esporta" Width="100px"
                                        Skin="Office2007" ToolTip="Effettua l'esportazione dei registri">
                                        <Icon PrimaryIconUrl="../../../../images/export.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>

                                    <telerik:RadButton ID="ChiudiStampaImageButton" runat="server" Text="Chiudi" Width="100px" Skin="Office2007"
                                        ToolTip="Chiudi la finestra">
                                        <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
                </div>

                 <telerik:RadTextBox ID="CodiceClassificazioneTextBox" runat="server" Skin="Office2007"
                                                            Style="display: none; width: 0px" />
            </div>
            <div id="UploadPanel" style="position: absolute; width: 100%; text-align: center;
                z-index: 2000000; display: none; top: 100px">
                <div id="UploadShadowPanel" style="width: 660px; text-align: center; background-color: #BFDBFF;
                    margin: 0 auto">
                    <table width="660px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 0px; height: 25px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloPannelloUploadLabel" runat="server" CssClass="Etichetta"
                                                            Font-Bold="True" Style="width: 500px; color: #00156E; background-color: #BFDBFF"
                                                            Text="Allega Documento Generico" />
                                                    </td>
                                                    <td align="right">
                                                        <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="HideUploadPanel();document.getElementById('<%= Me.ChiudiUploadButton.ClientID %>').click();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%-- BODY--%>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <div id="ZoneID1">
                                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="TipoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Allega da" />
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton Text="File" Checked="True" AutoPostBack="False" ID="UploadDaFileRadioButton"
                                                                GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                            <asp:RadioButton Text="Scanner" AutoPostBack="False" ID="UploadDaScannerRadioButton" 
                                                                GroupName="TipoDocumento" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="NomeFileDocumentoLabel" runat="server" CssClass="Etichetta" Text="Nome file" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadAsyncUpload ID="AllegatoUpload" runat="server" MaxFileInputsCount="1"
                                                                Skin="Office2007" Width="300px" InputSize="69">
                                                                <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                            </telerik:RadAsyncUpload>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="DescrizioneAllegatoTextBox" runat="server" Skin="Office2007"
                                                                Width="480px" MaxLength="100" ToolTip="Descrizione Allegato" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="OpzioniScannerLabel" runat="server" CssClass="Etichetta" Text="Opzioni scanner" />
                                                        </td>
                                                        <td>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="FronteRetroLabel" runat="server" CssClass="Etichetta" Text="Fronte retro" />&nbsp;<asp:CheckBox
                                                                            ID="FronteRetroCheckBox" runat="server" Text="&nbsp;" Checked="true" />&nbsp;&nbsp;<asp:Label
                                                                                ID="VisualizzaUILabel" runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                                                    ID="VisualizzaUICheckBox" runat="server" Text="&nbsp;" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="NomeFileDocumentoScansionatoLabel" runat="server" CssClass="Etichetta"
                                                                Text="File scansionato" />
                                                        </td>
                                                        <td>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                            border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 420px; height: 19px">
                                                                            <asp:Label ID="NomeFileDocumentoScansionatoTextbox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                runat="server" Width="420px">&nbsp;</asp:Label>
                                                                        </span>
                                                                    </td>
                                                                    <td style="width: 25; text-align: center">
                                                                        <asp:ImageButton ID="ScansionaImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                            ToolTip="Allega documento digitalizzato" TabIndex="44" BorderStyle="None" ImageAlign="AbsMiddle" /><asp:ImageButton
                                                                                ID="ScanUploadButton" Style="display: none" runat="server" ImageUrl="~/images//RecycleEmpty.png" />
                                                                    </td>
                                                                    <td style="width: 25; text-align: center">
                                                                        <asp:ImageButton ID="EliminaDocumentoScansionatoImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                            ToolTip="Elimina documento scansionato" ImageAlign="AbsMiddle" />
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
                            <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                border-top: 1px solid  #9ABBE8; height: 25px">
                                &nbsp;
                                <telerik:RadButton ID="ChiudiUploadButton" runat="server" Text="Chiudi" Width="90px"
                                    Skin="Office2007" ToolTip="Chiudi">
                                    <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                                &nbsp; &nbsp; &nbsp;
                                <telerik:RadButton ID="ConfermaUploadButton" runat="server" Text="Conferma" Width="100px"
                                    Skin="Office2007">
                                    <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
