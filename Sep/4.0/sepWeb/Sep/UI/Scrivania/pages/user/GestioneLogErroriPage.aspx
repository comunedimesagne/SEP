<%@ Page Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneLogErroriPage.aspx.vb" Inherits="GestioneLogErroriPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UI/Protocollo/pages/user/VisualizzaFatturaUserControl.ascx" TagName="VisualizzaFatturaControl" TagPrefix="parsec" %>

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
                // _backgroundElement.className = "modalBackground";
                _backgroundElement.style.backgroundColor = '#09718F';
                _backgroundElement.style.filter = "alpha(opacity=20)";
                _backgroundElement.style.opacity = "0.2";
            }
            else {
                _backgroundElement.style.display = 'none';

            }
        }


        function ShowMessageBox(message) {

            messageBox = document.createElement('DIV');


            this.document.body.appendChild(messageBox);

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
                                                            Text="Filtro Log Errori" />
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
                                                                <td style="width: 80px">
                                                                    <asp:Label ID="DataInizioLabel" runat="server" CssClass="Etichetta" Text="Data da" />
                                                                </td>
                                                                <td style="width: 125px">
                                                                    <telerik:RadDatePicker ID="DataInizioTextBox" Skin="Office2007" Width="110px"
                                                                        runat="server" MinDate="1753-01-01" ToolTip="Data inizio log errore">
                                                                        <Calendar runat="server">
                                                                            <SpecialDays>
                                                                                <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                            </SpecialDays>
                                                                        </Calendar>
                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td style="width: 20px">
                                                                    <asp:Label ID="DataFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="DataFineTextBox" Skin="Office2007" Width="110px"
                                                                        runat="server" MinDate="1753-01-01" ToolTip="Data fine log errore">
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
                                                                    &nbsp;<asp:Label ID="TitoloElencoLogLabel" runat="server" Font-Bold="True" Style="
                                                                        color: #00156E; background-color: #BFDBFF" Text="Elenco Log Errori" />
                                                                </td>


                                                               


                                                                   <td align="center" style="width: 40px">



                                                                    <asp:ImageButton ID="EsportaInExcelImageButton" Style="border: 0" runat="server"
                                                                         ImageUrl="~/images//excel32.png" ToolTip="Esporta i log visualizzati in un file formato excel"
                                                                        ImageAlign="AbsMiddle" />
                                                                </td>

                                                               
                                                                <td align="center" style="width: 40px">



                                                                    <asp:ImageButton ID="ElimnaLogSelezionatiImageButton" Style="border: 0" runat="server"
                                                                         ImageUrl="~/images//Delete.png" ToolTip="Elimina tutti i log selezionati"
                                                                        ImageAlign="AbsMiddle" />
                                                                </td>
                                                              
                                                               

                                                                
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div runat="server" id="ZoneID2" style="overflow: auto; height: 610px; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <telerik:RadGrid ID="LogErroriGridView" runat="server" AllowPaging="False"
                                                                AutoGenerateColumns="False" AllowFilteringByColumn="True" CellSpacing="0" GridLines="None"
                                                                Skin="Office2007" Width="99.8%" AllowSorting="True" Culture="it-IT" 
                                                                AllowMultiRowSelection="True">

                                                                <ClientSettings>
                                                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                                                                </ClientSettings>

                                                                <MasterTableView DataKeyNames="Id" TableLayout="Fixed">


                                                                 
                                                                    <Columns>

                                                                        <telerik:GridClientSelectColumn UniqueName="SelectCheckBox" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                        </telerik:GridClientSelectColumn>

                                                                        

                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                      
                                                                  

                                                                           <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" 
                                                                            AutoPostBackOnFilter="True" DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                            FilterControlWidth="100%" HeaderText="Descrizione" ShowFilterIcon="False"
                                                                            SortExpression="Descrizione" UniqueName="Descrizione" HeaderTooltip="Descrizione">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width:100% ;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>'>
                                                                                    <%# Eval("Descrizione")%>
                                                                                </div>

                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>


                                                                       
                                                                          <telerik:GridTemplateColumn  AndCurrentFilterFunction="Contains" ItemStyle-Width="250px"
                                                                            HeaderStyle-Width="250px" AutoPostBackOnFilter="True" DataField="RiferimentoEntita"
                                                                            FilterControlAltText="Filter RiferimentoEntita column" FilterControlWidth="100%"
                                                                            HeaderText="Riferimento" ShowFilterIcon="False" SortExpression="RiferimentoEntita"
                                                                            UniqueName="RiferimentoEntita" HeaderTooltip="Riferimento">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 250px;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("RiferimentoEntita"), "'", "&#039;")%>'>
                                                                                    <%# Eval("RiferimentoEntita")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                          <telerik:GridTemplateColumn  AndCurrentFilterFunction="Contains" ItemStyle-Width="140px"
                                                                            HeaderStyle-Width="140px" AutoPostBackOnFilter="True" DataField="NomeModulo"
                                                                            FilterControlAltText="Filter NomeModulo column" FilterControlWidth="100%"
                                                                            HeaderText="Modulo" ShowFilterIcon="False" SortExpression="NomeModulo"
                                                                            UniqueName="NomeModulo" HeaderTooltip="Modulo">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 140px;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("NomeModulo"), "'", "&#039;")%>'>
                                                                                    <%# Eval("NomeModulo")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>



                                                                          <telerik:GridTemplateColumn  AndCurrentFilterFunction="Contains" ItemStyle-Width="190px"
                                                                            HeaderStyle-Width="190px" AutoPostBackOnFilter="True" DataField="Utente"
                                                                            FilterControlAltText="Filter Utente column" FilterControlWidth="100%"
                                                                            HeaderText="Utente" ShowFilterIcon="False" SortExpression="Utente"
                                                                            UniqueName="Utente" HeaderTooltip="Utente">
                                                                            <ItemTemplate>
                                                                                <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 190px;
                                                                                    border: 0px solid red" title='<%# Replace(Eval("Utente"), "'", "&#039;")%>'>
                                                                                    <%# Eval("Utente")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                      

                                                                     
                                                                        <telerik:GridBoundColumn AllowFiltering="False" DataField="Data"
                                                                            DataType="System.DateTime" FilterControlAltText="Filter Data column"
                                                                            HeaderText="Data" ShowFilterIcon="False" SortExpression="Data"
                                                                            UniqueName="Data" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                                                            HeaderTooltip="Data" ItemStyle-Width="140px" HeaderStyle-Width="140px">
                                                                        </telerik:GridBoundColumn>

                                                                        

                                                                       <%-- <telerik:GridButtonColumn ItemStyle-Width="30px" HeaderStyle-Width="30px" ButtonType="ImageButton"
                                                                            CommandName="Anteprima" FilterControlAltText="Filter Anteprima column" ImageUrl="~/images/knob-search16.png"
                                                                            UniqueName="Anteprima" Text="Anteprima Fattura" HeaderTooltip="Anteprima Fattura">
                                                                        </telerik:GridButtonColumn>--%>

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
                                <%--<telerik:RadButton ID="SalvaButton" runat="server" Text="Esporta" Width="90px" Skin="Office2007"
                                    ToolTip="Esporta in formato CSV">
                                    <Icon PrimaryIconUrl="../../../../images/excel16.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>--%>
                            </td>
                        </tr>
                    </table>


                </center>

                


                <table style="width: 100%; border: 1px solid #9ABBE8; text-align: center">
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
             
                
                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
               
            </div>



           


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
