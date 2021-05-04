<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="DelegatiPage.aspx.vb" Inherits="DelegatiPage" %>

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

        function ShowMessageBox(message) {

            this.document.body.appendChild(messageBox);
            this.document.body.appendChild(messageBoxPanel);

           // HideMessageBox();

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

                <table style="width: 900px; border: 1px solid #5D8CC9">
                    <tr>
                        <td>

                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                                            <Items>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                                    CommandName="Nuovo" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                    CommandName="Trova" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                    CommandName="Stampa" Owner="RadToolBar" />
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
                                    <tr style="height: 30px">
                                        <td style="width: 90px">
                                            <asp:Label ID="DelegatoLabel" runat="server" CssClass="Etichetta" ForeColor="#FF6600"
                                                Text="Delegato *" />
                                        </td>
                                        <td style="width: 500px">
                                          <%--  <telerik:RadTextBox ID="DelegatoTextBox" runat="server" Skin="Office2007" Width="500px"
                                                Enabled="False" ToolTip="Utente Delegato" />--%>

                                                   <telerik:RadComboBox ID="UtentiComboBox" runat="server" Width="600" Height="150" EmptyMessage="Seleziona" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                               ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" ToolTip="Utente Delegante" Skin="Office2007"  AutoPostBack="true"  LoadingMessage="Caricamento in corso...">
                                <WebServiceSettings Method="GetUtenti" Path="DeleghePage.aspx"   />
                              </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 25px; text-align: center">
                                            <asp:ImageButton ID="TrovaDelegatoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                ToolTip="Seleziona delegato..." ImageAlign="AbsMiddle" Visible="false" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="EliminaDelegatoImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                ToolTip="Cancella delegato" ImageAlign="AbsMiddle" Visible="false" />
                                            <asp:ImageButton ID="AggiornaDelegatoImageButton" runat="server" Style="display: none" />
                                            <asp:TextBox ID="IdDelegatoTextBox" runat="server" Style="display: none" />
                                        </td>
                                    </tr>
                               
                                </table>

                            </div>

                            <table style=" width:100%">
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
                                                                            &nbsp;<asp:Label ID="ElencoUtentiLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                                                Text="Utenti" />
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
                                                                                <telerik:RadGrid ID="UtentiGridView" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                                                                    GridLines="None" Skin="Office2007" AllowSorting="False" AllowMultiRowSelection="true"
                                                                                    Culture="it-IT">
                                                                                    <ClientSettings>
                                                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                    </ClientSettings>
                                                                                    <MasterTableView DataKeyNames="Id">
                                                                                        <Columns>
                                                                                            <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                                                                            </telerik:GridClientSelectColumn>

                                                                                               <telerik:GridBoundColumn DataField="Nominativo" DataType="System.String" FilterControlAltText="Filter NominativoHidden column"
                                                                                                HeaderText="NominativoHidden" SortExpression="Nominativo" UniqueName="NominativoHidden" Visible="False" />

                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="340px" ItemStyle-Width="340px" DataField="Nominativo"
                                                                                                FilterControlAltText="Filter Nominativo column" HeaderText="Nominativo" SortExpression="Nominativo"
                                                                                                UniqueName="Nominativo">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 340px; border: 0px solid red">
                                                                                                        <%# Eval("Nominativo")%></div>
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
                                                        ToolTip="Rimuovi tutto" />
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
                                                                        <td style=" width:120px">
                                                                            &nbsp;<asp:Label ID="ElencoDelegantiLabel" runat="server" Style="color: #00156E"
                                                                                Font-Bold="True" Text="Deleganti" />
                                                                        </td>
                                                                         <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 35px">
                                                                                        <asp:Label ID="DataValiditaInizioLabel" runat="server" CssClass="Etichetta" Text="Da *" />
                                                                                    </td>
                                                                                    <td style="width: 80px">
                                                                                        <telerik:RadDatePicker ID="DataValiditaInizioTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" ToolTip="Data inizio validità">
                                                                                            <Calendar>
                                                                                                <SpecialDays>
                                                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                </SpecialDays>
                                                                                            </Calendar>
                                                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                                                        </telerik:RadDatePicker>
                                                                                    </td>
                                                                                    <td align="center" style="width: 35px">
                                                                                        <asp:Label ID="DataValiditaFineLabel" runat="server" CssClass="Etichetta" Text="A *" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadDatePicker ID="DataValiditaFineTextBox" Skin="Office2007" Width="110px" ToolTip="Data fine validità"
                                                                                            runat="server" MinDate="1753-01-01">
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
                                                        <tr>
                                                            <td class="ContainerMargin">
                                                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div id="scrollPanelDeleganti" style="overflow: auto; height: 200px; width: 100%;
                                                                                background-color: #FFFFFF; border-top: 1px solid #5D8CC9;">
                                                                                <telerik:RadGrid ID="DelegantiGridView" runat="server" AutoGenerateColumns="False"
                                                                                    CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="False" AllowMultiRowSelection="true"
                                                                                    Culture="it-IT">
                                                                                    <ClientSettings>
                                                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                                    </ClientSettings>
                                                                                    <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                        <Columns>

                                                                                            <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px" ItemStyle-Width="35px">
                                                                                            </telerik:GridClientSelectColumn>

                                                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />

                                                                                            <telerik:GridTemplateColumn  DataField="Nominativo"
                                                                                                FilterControlAltText="Filter Nominativo column" HeaderText="Nominativo" SortExpression="Nominativo"
                                                                                                UniqueName="Nominativo">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                        <%# Eval("Nominativo")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn SortExpression="ValidaDa" UniqueName="ValidaDa" HeaderText="Valida da"
                                                                                                DataField="ValidaDa" HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("ValidaDa","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                        overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                        <%# Eval("ValidaDa", "{0:dd/MM/yyyy}")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn SortExpression="ValidaA" UniqueName="ValidaA" HeaderText="Valida a"
                                                                                                DataField="ValidaDa" HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("ValidaA","{0:dd/MM/yyyy}")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                        <%# Eval("ValidaA", "{0:dd/MM/yyyy}")%></div>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn UniqueName="CheckBoxIterTemplateColumn" HeaderText="Disabil."
                                                                                                HeaderStyle-Width="55px" ItemStyle-Width="55px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderTooltip="Seleziona per disabilitare l'accesso sulla scrivania del delegante">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="DisabilitaAccessoScrivaniaCheckBox" runat="server" AutoPostBack="False" ToolTip="Seleziona per disabilitare l'accesso sulla scrivania del delegante" /></ItemTemplate>
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
                                </tr>
                            </table>

                            <div id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                             
                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloElencoDelegatiLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Elenco Delegati" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="DelegatiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                    Culture="it-IT" AllowFilteringByColumn="false">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="825px" ItemStyle-Width="825px" DataField="Delegato"
                                                                FilterControlAltText="Filter Delegato column" HeaderText="Nominativo" SortExpression="Delegato"
                                                                AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                ShowFilterIcon="False" UniqueName="Delegato">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Delegato")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 825px; border: 0px solid red">
                                                                        <%# Eval("Delegato")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                Text="Seleziona Delegato" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                ImageUrl="~\images\checks.png" UniqueName="Select">
                                                                <HeaderStyle Width="20px" />
                                                                <ItemStyle Width="20px" />
                                                            </telerik:GridButtonColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>

                            </div>

                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
