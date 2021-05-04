<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneOrdineGiornoPage.aspx.vb" Inherits="GestioneOrdineGiornoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');
        var count = 2;

        //https://stackoverflow.com/questions/20065344/javascript-function-to-select-deselect-check-all-check-box-in-radgrid-header-tem?rq=1

        function GridCreated(sender, args) {

            var numeroRigheDisabilitate = 0;
            var grid = sender;
            var masterTable = grid.get_masterTableView();

         
            var column = masterTable.getColumnByUniqueName("SelectCheckBox");
            var cell = column.get_element();
            //var checkBoxSelectAll = $telerik.$(cell).find('input')[0];
            var checkBoxSelectAll = $telerik.$(cell).find(':checkbox').get(0);



            $telerik.$(checkBoxSelectAll).removeAttr("onclick");

            $telerik.$(checkBoxSelectAll).click(function () {
                SelectAll(checkBoxSelectAll, sender);
            });
            
            //var checkBoxSelectAll = $get($get('<%= CheckBoxSelectAll.ClientID %>').value)

            var rows = masterTable.get_dataItems();



            var numeroRighe = rows.length;
            
            var numeroRigheSelezionate = 0; 


            var disabled = false;

            //DISABILITO LA CHECKBOX SE NON CI SONO RIGHE O SE SONO TUTTE DISABILITATE
            if (numeroRighe == 0) {
                disabled = true;
            } else {
                for (var i = 0; i < numeroRighe; i++) {
                    var item = rows[i];
                    var chk = item.findElement("SelectCheckBox");
                    if (chk.disabled) {
                        numeroRigheDisabilitate += 1;
                    } else {
                        if (chk.checked) {
                            numeroRigheSelezionate += 1
                        }
                    }
                }

                if (numeroRigheDisabilitate == numeroRighe) {
                    disabled = true;
                } else {

                    if (numeroRigheSelezionate > 0) {
                        if (numeroRigheSelezionate == (numeroRighe - numeroRigheDisabilitate)) {
                            checkBoxSelectAll.checked = true;
                        }
                    }

                }
            }

            checkBoxSelectAll.disabled = disabled;
        }


        function SelectAll(chkAll,sender) {
            var grid = sender;
            var masterTable = grid.get_masterTableView();
            var rows = masterTable.get_dataItems();
            var numeroRighe = rows.length;

            if (numeroRighe > 0) {

                for (var i = 0; i < numeroRighe; i++) {
                    var item = rows[i];
                    var chk = item.findElement("SelectCheckBox");
                    if (!chk.disabled) {
                        chk.cheched = chkAll.checked;
                        item.set_selected(chkAll.checked);
                    }
                }
            }
        }


        function RowSelecting(sender, args) {
            //args E' UN OGGETTO Telerik.Web.UI.GridDataItemCancelEventArgs
            

            //TROVO LA CHECKBOX DELLA RIGA SELEZIONATA

            //var id = args.get_id();
            //var inputCheckBox = $get(id).getElementsByTagName("input")[0];

            //var inputCheckBox = $telerik.$($get(args.get_id())).find('input')[0];

            //var dataItem = sender.get_masterTableView().get_dataItems()[args.get_itemIndexHierarchical()];

            var dataItem = args.get_gridDataItem();

            var inputCheckBox = dataItem.findElement("SelectCheckBox");

            if (!inputCheckBox || inputCheckBox.disabled) {
                //ANNULLO LA SELEZIONE PER DISABILITARE LA RIGA
                 args.set_cancel(true);
             }

            var numeroRigheSelezionate = 0;  // masterTable.get_selectedItems().length;
            var numeroRigheDisabilitate = 0;
            var grid = sender;
            var masterTable = grid.get_masterTableView();

            var checkBoxSelectAll = $get($get('<%= CheckBoxSelectAll.ClientID %>').value)

            var rows = masterTable.get_dataItems();
            var numeroRighe = rows.length;

            for (var i = 0; i < numeroRighe; i++) {
                var item = rows[i]
                var chk = item.findElement("SelectCheckBox");
                if (chk.disabled) {
                    numeroRigheDisabilitate += 1;

                } else {
                    // SE LA RIGA CORRENTE E' QUELLA SELEZIONATA
                    if (item._itemIndexHierarchical == args.get_itemIndexHierarchical()) {
                        numeroRigheSelezionate += 1

                    } else {
                        if (chk.checked) {
                            numeroRigheSelezionate += 1
                        }
                    }


                }
                // masterTable.get_dataItems()[i].set_selected(false);
            }

            //SE CI SONO RIGHE SELEZIONATE E SE TUTTE LE RIGHE SELEZIONABILI SONO SELEZIONATE
            if (numeroRigheSelezionate > 0) {
                if (numeroRigheSelezionate == (numeroRighe - numeroRigheDisabilitate)) {
                     checkBoxSelectAll.checked = true;
                }
            }
        }

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


      <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="OrdineGiornoRadWindow" runat="server" Modal="True" Animation="Fade"
                AnimationDuration="200" Behaviors="Close" Height="350" Skin="Office2007" Width="850"
                VisibleTitlebar="True" VisibleStatusbar="False" ReloadOnShow="true" Title="Ordine del Giorno">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>



    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>

            <div id="pageContent">

            

                <table style="width: 900px; border: 1px solid #5D8CC9">
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                                            <Items>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                                    CommandName="Nuovo" Owner="RadToolBar" Enabled="False" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                    CommandName="Trova" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar" />
                                                <telerik:RadToolBarDropDown runat="server" ImageUrl="~/images/Printer.png" Text="Stampa">
                                                    <Buttons>
                                                        <telerik:RadToolBarButton runat="server" CommandName="StampaOrdineGiorno" Text="Ordine del Giorno"
                                                            Width="240px" />
                                                        <telerik:RadToolBarButton runat="server" CommandName="StampaAvvisoConvocazione" Text="Avviso di Convocazione"
                                                            Width="240px" />
                                                    </Buttons>
                                                </telerik:RadToolBarDropDown>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/send_email.png" Text="Invia E-mail"
                                                    CommandName="InviaEmail" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Export32.png" Text="Esporta"
                                                    CommandName="Export" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar" />
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>

                            <div id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">
                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                    <tr style="height: 36px">
                                        <td style="width: 65px">
                                            <asp:Label ID="PeriodoLabel" runat="server" CssClass="Etichetta" ForeColor="#FF6600"
                                                Text="Periodo" Width="60px" />
                                        </td>
                                         <td style="width: 170px">
                                            <telerik:RadMonthYearPicker ID="PeriodoTextBox" runat="server" Width="140px" Culture="it-IT"
                                                Skin="Office2007">
                                                <DateInput DateFormat="MMMM yyyy" DisplayDateFormat="MMMM yyyy" runat="server">
                                                </DateInput>
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" ToolTip="Apri il visualizzatore mese/anno" />
                                            </telerik:RadMonthYearPicker>
                                        </td>
                                        <td style="width: 120px">
                                          <asp:Label ID="TipologiaSedutaLabel" runat="server" CssClass="Etichetta" ForeColor="#FF6600"
                                                Text="Tipologia Seduta" />
                                        </td>
                                        <td>
                                          <telerik:RadComboBox ID="TipologieSedutaComboBox" AutoPostBack="true" runat="server"
                                                                EmptyMessage="- Seleziona Organo -" MaxHeight="150px" Skin="Office2007" Width="400px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloElencoSeduteLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Elenco Sedute" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="SeduteGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                    Culture="it-IT" AllowFilteringByColumn="false">
                                                    <MasterTableView DataKeyNames="Id, IdTipologiaSeduta">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="270px" ItemStyle-Width="270px" DataField="DescrizioneTipologiaSeduta"
                                                                FilterControlAltText="Filter DescrizioneTipologiaSeduta column" HeaderText="Tipologia"
                                                                SortExpression="DescrizioneTipologiaSeduta" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains"
                                                                FilterControlWidth="100%" ShowFilterIcon="False" UniqueName="DescrizioneTipologiaSeduta">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("DescrizioneTipologiaSeduta")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 270px; border: 0px solid red">
                                                                        <%# Eval("DescrizioneTipologiaSeduta")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="200px" ItemStyle-Width="200px" DataField="DescrizioneTipologiaConvocazione"
                                                                FilterControlAltText="Filter DescrizioneTipologiaConvocazione column" HeaderText="Sessione"
                                                                SortExpression="DescrizioneTipologiaConvocazione" AutoPostBackOnFilter="True"
                                                                CurrentFilterFunction="Contains" FilterControlWidth="100%" ShowFilterIcon="False"
                                                                UniqueName="DescrizioneTipologiaConvocazione">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("DescrizioneTipologiaConvocazione")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 200px; border: 0px solid red">
                                                                        <%# Eval("DescrizioneTipologiaConvocazione")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="120px" ItemStyle-Width="120px" DataField="DataConvocazione"
                                                                HeaderText="Data" SortExpression="DataConvocazione" AllowFiltering="false" UniqueName="DataConvocazione">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("DataConvocazione", "{0:dd/MM/yyyy HH:mm:ss}")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 120px; border: 0px solid red">
                                                                        <%# Eval("DataConvocazione", "{0:dd/MM/yyyy HH:mm:ss}")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="95px" ItemStyle-Width="95px" DataField="DataPrimaConvocazione"
                                                                HeaderText="Data 1&#170; Conv." SortExpression="DataPrimaConvocazione" AllowFiltering="false"
                                                                UniqueName="DataPrimaConvocazione">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("DataPrimaConvocazione", "{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 95px; border: 0px solid red">
                                                                        <%# Eval("DataPrimaConvocazione", "{0:dd/MM/yyyy}")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="95px" ItemStyle-Width="95px" DataField="DataSecondaConvocazione"
                                                                HeaderText="Data 2&#170; Conv." SortExpression="DataSecondaConvocazione" AllowFiltering="false"
                                                                UniqueName="DataSecondaConvocazione">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("DataSecondaConvocazione", "{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 95px; border: 0px solid red">
                                                                        <%# Eval("DataSecondaConvocazione", "{0:dd/MM/yyyy}")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                Text="Seleziona Seduta" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                ImageUrl="~\images\checks.png" UniqueName="Select" />
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 49%">
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                                                border-top: 1px solid  #9ABBE8; height: 40px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 250px">
                                                                            &nbsp;<asp:Label ID="ElencoProposteLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                Text="Proposte" />
                                                                        </td>
                                                                        <td align="right">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="AnnoLabel" runat="server" CssClass="Etichetta" ForeColor="#FF6600"
                                                                                            Text="Anno" Width="40px" Font-Bold="True" />
                                                                                    </td>
                                                                                    <td align="right" style="width: 110px">
                                                                                        <telerik:RadComboBox ID="AnniComboBox" AutoPostBack="true" runat="server" EmptyMessage="- Seleziona -"
                                                                                            MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
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
                                                                      
                                                                            <div id="scrollPanelProposte" style="overflow: auto; height: 200px; width: auto;
                                                                                background-color: #FFFFFF; border-top: 1px solid #5D8CC9;">
                                                                                <telerik:RadGrid ID="ProposteGridView" runat="server" AutoGenerateColumns="False"
                                                                                    CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="False" AllowMultiRowSelection="true"
                                                                                    Culture="it-IT">

                                                                                    <ClientSettings>
                                                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                    </ClientSettings>

                                                                                    <MasterTableView DataKeyNames="Id, Codice">
                                                                                        
                                                                                        <Columns>

                                                                                            <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                                                                            </telerik:GridClientSelectColumn>

                                                                                            <%--<telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                                                AllowFiltering="False" HeaderStyle-Width="10px" ItemStyle-Width="10px" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                              <ItemTemplate>
                                                                                                    <asp:CheckBox ID="SelectCheckBox"  AutoPostBack="false"
                                                                                                        runat="server"></asp:CheckBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>--%>

                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />

                                                                                            <telerik:GridBoundColumn DataField="Codice" DataType="System.Int32" FilterControlAltText="Filter Codice column"
                                                                                                HeaderText="Codice" SortExpression="Codice" UniqueName="Codice" Visible="False" />

                                                                                            <telerik:GridTemplateColumn SortExpression="ContatoreGenerale" UniqueName="ContatoreGenerale"
                                                                                                HeaderText="N." DataField="ContatoreGenerale" HeaderStyle-Width="25px" ItemStyle-Width="25px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("ContatoreGenerale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 25px; border: 0px solid red">
                                                                                                        <%# Eval("ContatoreGenerale")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn SortExpression="DataProposta" UniqueName="DataProposta"
                                                                                                HeaderText="Data" DataField="DataProposta" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("DataProposta","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                        overflow: hidden; text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                                                        <%# Eval("DataProposta", "{0:dd/MM/yyyy}")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="230px" ItemStyle-Width="230px" DataField="Oggetto"
                                                                                                FilterControlAltText="Filter Oggetto column" HeaderText="Oggetto" SortExpression="Oggetto"
                                                                                                UniqueName="Oggetto">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 230px; border: 0px solid red">
                                                                                                        <%# Eval("Oggetto")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

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
                                    </td>
                                    <td align="center" width="2%">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%; height: 161px;">
                                            <tr>
                                                <td align="center" style="height: 26px">
                                                    <asp:ImageButton ID="AggiungiTuttoImageButton" runat="server" ImageUrl="~/images/Forwardd24.png"
                                                        ToolTip="Aggiungi tutto" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:ImageButton ID="EliminaTuttoImageButton" runat="server" ImageUrl="~/images/Forwards24.png"
                                                        TabIndex="13" ToolTip="Rimuovi tutto" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px;">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px">
                                                    <asp:ImageButton ID="AggiungiImageButton" runat="server" ImageUrl="~/images/FrecciaDx24.png"
                                                        ToolTip="Aggiungi elementi selezionati" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px">
                                                    <asp:ImageButton ID="EliminaImageButton" runat="server" ImageUrl="~/images/FrecciaSx24.png"
                                                        ToolTip="Rimuovi elementi selezionati" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 21px">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="49%">
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                                                border-top: 1px solid  #9ABBE8; height: 40px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="ElencoOrdiniGiornoLabel" runat="server" Style="color: #00156E"
                                                                                Font-Bold="True" Text="Ordine del Giorno" />
                                                                        </td>
                                                                        <td align="right" style="width: 30px">
                                                                            <asp:ImageButton ImageAlign="AbsMiddle" ID="AggiungiArgomentoImageButton" runat="server"
                                                                                ImageUrl="~/images//knob_add20.png" ToolTip="Aggiungi argomento..." />
                                                                        </td>
                                                                        <td align="center" style="width: 20px">
                                                                            <asp:Image ImageAlign="AbsMiddle" ID="NavigatorSeparator1" runat="server" ImageUrl="~/images//NavigatorSeparator.png" />
                                                                        </td>
                                                                        <td align="right" style="width: 30px">
                                                                            <asp:ImageButton ImageAlign="AbsMiddle" ID="SpostaSuImageButton" runat="server" ImageUrl="~/images/Up.png"
                                                                                ToolTip="Sposta sù elemento selezionato" />
                                                                        </td>
                                                                        <td align="right" style="width: 30px">
                                                                            <asp:ImageButton ImageAlign="AbsMiddle" ID="SpostaGiuImageButton" runat="server"
                                                                                ImageUrl="~/images/Down.png" ToolTip="Sposta giù elemento selezionato" />
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
                                                                        
                                                                            <div id="scrollPanelElementiOrdineGiorno" style="overflow: auto; height: 200px; width: 100%;
                                                                                background-color: #FFFFFF; border-top: 1px solid #5D8CC9;">
                                                                                <telerik:RadGrid ID="ElementiOrdineGiornoGridView" runat="server" AutoGenerateColumns="False"
                                                                                    CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="False" AllowMultiRowSelection="true"
                                                                                    Culture="it-IT">
                                                                                    <ClientSettings>
                                                                                        <Selecting AllowRowSelect="True" EnableDragToSelectRows="false" />
                                                                                         <ClientEvents OnRowSelecting="RowSelecting" OnGridCreated="GridCreated"  />
                                                                                      
                                                                                    </ClientSettings>
                                                                                    <MasterTableView DataKeyNames="Id, CodiceDocumento,Guid">
                                                                                        <Columns>

                                                                                            <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                                                                            </telerik:GridClientSelectColumn>

                                                                                            <%-- <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                                                AllowFiltering="False" HeaderStyle-Width="10px" ItemStyle-Width="10px" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                               
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="SelectCheckBox"  AutoPostBack="false"
                                                                                                        runat="server"></asp:CheckBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>--%>

                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                                                            
                                                                                            <telerik:GridBoundColumn DataField="CodiceDocumento" DataType="System.Int32" FilterControlAltText="Filter CodiceDocumento column"
                                                                                                HeaderText="Codice" SortExpression="CodiceDocumento" UniqueName="CodiceDocumento"
                                                                                                Visible="False" />
                                                                                            
                                                                                            <telerik:GridBoundColumn DataField="Ordinale" DataType="System.Int32" FilterControlAltText="Filter Ordinale column"
                                                                                                HeaderText="#" SortExpression="Ordinale" AllowSorting="false" UniqueName="Ordinale"
                                                                                                Visible="True" />
                                                                                           
                                                                                            <telerik:GridTemplateColumn SortExpression="ContatoreGenerale" UniqueName="ContatoreGenerale"
                                                                                                HeaderText="N." DataField="ContatoreGenerale" HeaderStyle-Width="25px" ItemStyle-Width="25px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("ContatoreGenerale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 25px; border: 0px solid red">
                                                                                                        <%# Eval("ContatoreGenerale")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                           
                                                                                            <telerik:GridTemplateColumn SortExpression="DataProposta" UniqueName="DataProposta"
                                                                                                HeaderText="Data" DataField="DataProposta" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("DataProposta","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                        overflow: hidden; text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                                                        <%# Eval("DataProposta", "{0:dd/MM/yyyy}")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            
                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="125px" ItemStyle-Width="125px" DataField="Oggetto"
                                                                                                FilterControlAltText="Filter Oggetto column" HeaderText="Oggetto" SortExpression="Oggetto"
                                                                                                UniqueName="Oggetto">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 125px; border: 0px solid red">
                                                                                                        <%# Eval("Oggetto")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Modifica" HeaderStyle-Width="20px"
                                                                                                 ItemStyle-Width="20px" FilterControlAltText="Filter Modifica column"
                                                                                                ImageUrl="~\images\Edit16.png" UniqueName="Modifica" />

                                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                ItemStyle-Width="20px" HeaderStyle-Width="20px"
                                                                                                ImageUrl="~\images\Delete16.png" UniqueName="Delete" />

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
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                </table>

                <asp:ImageButton ID="AggiornaElementoOrdineGiornoButton" runat="server" ImageUrl="~/images//knob-search16.png"
                    Style="display: none; width: 0px" />
                <asp:HiddenField ID="documentContentHidden" runat="server" />
                <asp:ImageButton ID="AggiornaPaginaButton" runat="server" ImageUrl="~/images//knob-search16.png"
                    Style="display: none; width: 0px" />
                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />

               
                <asp:HiddenField ID="CheckBoxSelectAll" runat="server" />

                  

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
