<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneGruppiPage.aspx.vb" Inherits="GestioneGruppiPage" %>

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

    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>
                <table style="width: 900px; border: 1px solid #5D8CC9">
                    <tr>
                        <td>

                            <table style="width: 100%">
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


                           <div  id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">

                                 <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td style="width: 90px">
                                            <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *"
                                                ForeColor="#FF8040" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ToolTip="Descrizione della tipologia di seduta" ID="DescrizioneTextBox"
                                                runat="server" Skin="Office2007" Width="99%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 90px">
                                            <asp:Label ID="DataValiditaLabel" runat="server" CssClass="Etichetta" Text="Valido" />
                                        </td>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td align="center" style="width: 40px">
                                                        <asp:Label ID="DataValiditaInizioLabel" runat="server" CssClass="Etichetta" Text="Da" />
                                                    </td>
                                                    <td style="width: 80px">
                                                        <telerik:RadDatePicker ID="DataValiditaInizioTextBox" Skin="Office2007" Width="110px"
                                                            runat="server" MinDate="1753-01-01">
                                                            <Calendar>
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                </SpecialDays>
                                                            </Calendar>
                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td align="center" style="width: 40px">
                                                        <asp:Label ID="DataValiditaFineLabel" runat="server" CssClass="Etichetta" Text="A" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="DataValiditaFineTextBox" Skin="Office2007" Width="110px"
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
                                    <tr>
                                        <td style="width: 80px">
                                            <asp:Label ID="AbilitatoLabel" runat="server" CssClass="Etichetta" Text="Abilitato" />
                                        </td>
                                        <td>
                                            <div id="ZoneID1">
                                                <asp:CheckBox ID="AbilitatoCheckBox" runat="server" Text="&nbsp;" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                             
                                 <div  id="PannelloGrigliaUtenti" runat="server" style="padding: 2px 2px 2px 2px;">

                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                <tr style="height: 20px; background-color: #BFDBFF">
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    &nbsp;<asp:Label Font-Bold="True" ID="TitoloElencoUtentiLabel" runat="server" Style="color: #00156E;
                                                        background-color: #BFDBFF; width: 600px" CssClass="Etichetta" Text="Elenco Utenti" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                        Style="display: none; width: 0" />
                                                </td>
                                                <td style="width:30px">
                                                    <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                        ToolTip="Seleziona utente..." ImageAlign="AbsMiddle" />
                                                </td>
                                               <td style="width:30px">
                                                    <asp:ImageButton ID="EliminaUtentiSelezionatiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                        Style="width: 16px" ToolTip="Cancella utenti selezionati" ImageAlign="AbsMiddle" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="scrollPanel" runat="server" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9;
                                            background-color: White">
                                            <telerik:RadGrid ID="UtentiGridView" ToolTip="Elenco utenti" runat="server" AllowPaging="False"
                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                 AllowSorting="True" AllowMultiRowSelection="True" Culture="it-IT">
                                                <MasterTableView DataKeyNames="IdUtente">
                                                    <Columns>

                                                        <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                            AllowFiltering="False" ItemStyle-Width="20px"  HeaderStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                    runat="server"></asp:CheckBox>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                    runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>


                                                        <telerik:GridBoundColumn DataField="IdUtente" DataType="System.Int32" FilterControlAltText="Filter IdUtente column"
                                                            HeaderText="IdUtente" UniqueName="IdUtente" Visible="False" />
                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneUtente" UniqueName="DescrizioneUtente"
                                                            HeaderText="Nominativo" DataField="DescrizioneUtente" HeaderStyle-Width="780px"
                                                            ItemStyle-Width="780px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DescrizioneUtente")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 780px; border: 0px solid red">
                                                                    <%# Eval("DescrizioneUtente")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                            ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\Delete16.png" UniqueName="Delete">
                                                        </telerik:GridButtonColumn>


                                                    </Columns>
                                                </MasterTableView></telerik:RadGrid></div>
                                    </td>
                                </tr>
                            </table>

                            </div>


                             <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">

                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloElencoGruppiLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                                             color: #00156E; background-color: #BFDBFF" Text="Elenco Gruppi" CssClass="Etichetta" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="GruppiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True"
                                                    Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="600px" ItemStyle-Width="600px" DataField="Descrizione"
                                                                FilterControlAltText="Filter Descrizione column" HeaderText="Descrizione" SortExpression="Descrizione"
                                                                UniqueName="Descrizione">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 600px; border: 0px solid red">
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
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                Text="Seleziona Gruppo" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                ImageUrl="~\images\checks.png" UniqueName="Select" />
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
                <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
            </div>

                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
