<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="RuoliPage.aspx.vb" Inherits="RuoliPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');
        var count;

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


    <asp:UpdatePanel ID="Pannello" runat="server" DisplayAfter="0">
        <ContentTemplate>
            <div id="pageContent">
                <table style="width: 700px; border: 1px solid #5D8CC9">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
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
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                    CommandName="Stampa" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>


                            <div id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">
                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                    <tr style="height: 30px">
                                        <td style="width: 70px">
                                            <asp:Label ID="RuoloLabel" runat="server" CssClass="Etichetta" Text="Ruolo *" ForeColor="#FF8040"
                                                Width="60px" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="RuoloTextBox" runat="server" Skin="Office2007" Width="100%"
                                                ToolTip="Descrizione del ruolo" />
                                        </td>

                                    </tr>
                                    <tr style="height: 30px">
                                        <td style="width: 70px">
                                            <asp:Label ID="TipologiaRuoloLabel" runat="server" CssClass="Etichetta" Text="Tipologia"
                                                Width="60px" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="TipologiaRuoloComboBox" AutoPostBack="false" runat="server" EmptyMessage="Seleziona Tipologia"
                                                MaxHeight="150px" Skin="Office2007" Filter="Contains" ItemsPerRequest="10" Width="400px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div  id="PannelloGrigliaUtenti" runat="server" style="padding: 2px 2px 2px 2px;">
                          
                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                <tr>
                                    <td style="height: 20px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    &nbsp;<asp:Label ID="UtentiRuoloLabel" runat="server" Font-Bold="True" Style="width: 300px;
                                                        color: #00156E; background-color: #BFDBFF" Text="Elenco Utenti" />
                                                </td>
                                                <td align="right">
                                                   <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                        ToolTip="Seleziona utente ..." ImageAlign="AbsMiddle" />
                                                    <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" Style="display: none" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="scrollPanel" runat="server" style="overflow: auto; height: 200px; border: 1px solid #5D8CC9;
                                            background-color: White; width: 100%;">
                                            <telerik:RadGrid ID="UtentiRuoloGridView" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True"
                                                ToolTip="Elenco Utenti legati al Ruolo" Culture="it-IT">
                                                <MasterTableView DataKeyNames="Id, IdUtente">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="Id" UniqueName="Id" Visible="False" />
                                                        <telerik:GridBoundColumn DataField="IdUtente" Visible="False" />
                                                        <telerik:GridBoundColumn DataField="IdRuolo" UniqueName="IdRuolo" Visible="False" />
                                                        <telerik:GridBoundColumn DataField="Utente" HeaderText="Utente" SortExpression="Utente"
                                                            UniqueName="Utente" />
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ImageUrl="~\images\Delete16.png"
                                                            UniqueName="Delete" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </td>
                                </tr>
                            </table>

                            </div>
                           
                         <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">

                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                <tr>
                                    <td style="height: 20px">
                                        &nbsp;<asp:Label ID="RuoliLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                            color: #00156E; background-color: #BFDBFF" Text="Elenco ruoli" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                     <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                        border: 0px solid #5D8CC9;">
                                        <telerik:RadGrid ID="RuoliGridView" runat="server" AllowPaging="True" CellPadding="0"
                                            CellSpacing="0" AutoGenerateColumns="False" GridLines="None" Skin="Office2007"
                                            AllowSorting="True" Culture="it-IT">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Id" Visible="False" />

                                                    
                                                    <telerik:GridBoundColumn DataField="Descrizione" UniqueName="Descrizione" SortExpression="Descrizione"
                                                        HeaderText="Ruolo" HeaderStyle-Width="560px" ItemStyle-Width="560px" />
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" ImageUrl="~\images\checks.png"
                                                        UniqueName="Select" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
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
            </div>
            <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
              <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
