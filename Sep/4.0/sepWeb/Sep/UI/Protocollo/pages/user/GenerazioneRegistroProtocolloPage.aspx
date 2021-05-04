<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="GenerazioneRegistroProtocolloPage.aspx.vb" Inherits="GenerazioneRegistroProtocolloPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%--SPDX-License-Identifier: GPL-3.0-only--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");


        function OnClientClicking(sender, args) {

            var risposta = confirm("Saranno generati i registri di Protocollo Giornalieri nell\'arco temporale indicato; inoltre saranno generati eventuali registri aggiuntivi relativi a registrazioni di protocollo annullate o modificate nello stesso arco temporale.\nSi vuole procedere?");
            if (risposta == true) {
                args.set_cancel(false);
                return true;
            } else {
                args.set_cancel(true);
                return false;
            }
        }

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
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



        function ShowMessageBox(message) {


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

            var messageBoxPanel = document.getElementById("messageBoxPanel");


            with (messageBoxPanel) {
                style.display = '';
            }

            var messageBox = document.getElementById("messageBox");

            with (messageBox) {
                style.width = '300px';
                style.height = '40px';
                style.backgroundColor = '#BFDBFF';
                style.border = 'solid #4892FF 2px';
                innerHTML = message;
                style.color = '#00156E';
                style.backgroundImage = 'url(/sep/Images/success.png)';
                style.backgroundPosition = '10px center';
                style.backgroundRepeat = 'no-repeat';
                style.padding = '15px 10px 15px 50px';
                style.margin = '15px 0px';

            }

        }


        function HideMessageBox() {
            try {
                var messageBoxPanel = document.getElementById("messageBoxPanel");
                messageBoxPanel.style.display = 'none';
                overlay.style.display = 'none';
            }
            catch (e) { }
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


        function updating(sender, args) {
            var d = $get("progressBar");
            d.style.display = '';
            if (args.get_progressData() && args.get_progressData().OperationComplete == 'true') {
                args.set_cancel(true);
                d.style.display = 'none';
            }
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
            <div id="progressBar" style="position: absolute; width: 100%; text-align: center;
                top: 300px; z-index: 2000000">
                <div id="Div3" style="width: 310px; text-align: center; background-color: #BFDBFF;
                    margin: 0 auto">
                    <telerik:RadProgressArea OnClientProgressUpdating="updating" Skin="Office2007" ID="RadProgressArea1"
                        runat="server" ProgressIndicators="RequestSize,TotalProgressPercent,TotalProgressBar"
                        HeaderText="Invio in corso ...">
                        <Localization Total="Totale:" Uploaded="Completato:" />
                    </telerik:RadProgressArea>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hflVerificaElimina" runat="server" />
            <div id="pageContent">
                <telerik:RadProgressManager ID="Radprogressmanager1" runat="server" Skin="Office2007" />
                <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>
                <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator>
                <center>
                    <table style="width: 900px; height: 690px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td valign="top">
                                <table class="ContainerWrapper" cellpadding="0" cellspacing="0" style="width: 100%;
                                    height: 100%" border="0">
                                    <tr style="height: 30px">
                                        <td valign="top">
                                            <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="FiltroEmailLabel" runat="server" Font-Bold="True" Style="width: 100%;
                                                            color: #00156E; background-color: #BFDBFF" Text="Generazione Registro Protocollo" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Effettua la ricerca con i filtri impostati" Style="border-style: none;
                                                            border-color: inherit; border-width: 0; width: 16px;" ImageAlign="AbsMiddle"
                                                            TabIndex="9" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="RipristinaFiltroInizialeImageButton" Style="border: 0" runat="server"
                                                            ImageUrl="~/images/cancelSearch.png" ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle"
                                                            TabIndex="10" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td valign="middle">
                                            <table>
                                                <tr>
                                                    <td style="width: 30px">
                                                        <asp:Label ID="DataProtocolloInizioLabel" runat="server" CssClass="Etichetta" Text="Dal" />
                                                    </td>
                                                    <td style="width: 120px">
                                                        <telerik:RadDatePicker ID="DataProtocolloInizioTextBox" Skin="Office2007" runat="server"
                                                            MinDate="1753-01-01" Width="110px">
                                                            <Calendar ID="Calendar2" runat="server" MinDate="1753-01-01">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td style="width: 30px">
                                                        <asp:Label ID="DataProtocolloFineLabel" runat="server" CssClass="Etichetta" Text="Al" />
                                                    </td>
                                                    <td style="width: 120px">
                                                        <telerik:RadDatePicker ID="DataProtocolloFineTextBox" Skin="Office2007" runat="server"
                                                            MinDate="1753-01-01" Width="110px">
                                                            <Calendar ID="Calendar1" runat="server" MinDate="1753-01-01">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td style="width: 120px">
                                                        <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Tipologia Registro" />
                                                    </td>
                                                    <td style="width: 110px">
                                                        <telerik:RadComboBox ID="comboTipologiaRegistro" runat="server" EmptyMessage="Selezionare la tipologia"
                                                            Filter="StartsWith" ItemsPerRequest="5" MaxHeight="300px" Skin="Office2007" Width="130px" />
                                                    </td>
    
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px">
                                        <td valign="top">
                                            <table style="width: 100%; background-color: #BFDBFF; border-top: 1px solid #9ABBE8;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRisultatiRicerca" runat="server" Font-Bold="True" Style="color: #00156E;"
                                                            Text="Elenco Registrazioni" />
                                                    </td>
                                                    <td align="right" valign="middle">
                                                        <telerik:RadButton ID="cmbGeneraRegistroGiornaliero" runat="server" Text="Genera Registri"
                                                            OnClientClicking="OnClientClicking" Skin="Office2007" ToolTip="Genera i registri">
                                                            <Icon PrimaryIconUrl="~/images/Processa.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td align="center" style="width: 125; border-left: 0 solid #5D8CC9;">
                                                        <telerik:RadButton ID="NoPaging" runat="server" Text="Non Paginare" Skin="Office2007"
                                                            ImageAlign="AbsMiddle" Width="115px" ToolTip="Disattiva/Attiva Paginazione">
                                                            <Icon PrimaryIconUrl="~/images/Next.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>
                                                    </td>
                                                    
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="5" style="width: 900px; height: 680px"
                                                border="0">
                                                <tr style="background-color: #DFE8F6">
                                                    <td valign="top">
                                                        <div id="scrollPanel" runat="server" style="overflow: auto; height: 685px; border: 1 solid #5D8CC9">
                                                            <asp:ImageButton ID="AggiornaRegistroFirmato" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                Style="display: none" />
                                                            <telerik:RadGrid ID="grigliaRegistroProtocollo" runat="server" AllowPaging="True"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="99.8%" AllowMultiRowSelection="true" AllowSorting="true" PageSize="26"
                                                                Culture="it-IT">
                                                                <MasterTableView DataKeyNames="id,dataRegistro,FileNameOriginale,FileNameFirmato,path,conservato,tipologia,daNumero,aNumero,daData,aData,idUtente,idPacchettoWs">
                                                                    <Columns>

                                                                        <telerik:GridTemplateColumn SortExpression="dataRegistro" UniqueName="Data" HeaderText="Data"
                                                                            DataField="dataRegistro" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("dataRegistro","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; text-overflow: ellipsis; width: 80px;">
                                                                                    <%# Eval("dataRegistro", "{0:dd/MM/yyyy}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="FileNameOriginale" UniqueName="fileNameOriginale"
                                                                            HeaderText="Nome Registro" DataField="fileNameOriginale" HeaderStyle-Width="250px"
                                                                            ItemStyle-Width="250px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("FileNameOriginale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 250px;">
                                                                                    <%# Eval("FileNameOriginale", "")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="daNumero" UniqueName="daNumero" HeaderText="Da Numero"
                                                                            DataField="daNumero" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("daNumero","{0:0000000}") %>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 100px;">
                                                                                    <%# Eval("daNumero", "{0:0000000}")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="aNumero" UniqueName="aNumero" HeaderText="A Numero"
                                                                            DataField="aNumero" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                                            <ItemTemplate>
                                                                                <div title='<%#  Eval("aNumero","{0:0000000}") %>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 100px;">
                                                                                    <%# Eval("aNumero", "{0:0000000}")%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="tipologia" UniqueName="tipologia" HeaderText="Tipologia"
                                                                            DataField="tipologia" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# IIf ( Eval("tipologia") = 0,"Giornaliero",IIf(Eval("tipologia") = 1,"Annullati","Modificati") )%>'
                                                                                    style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 100px;">
                                                                                    <%# IIf(Eval("tipologia") = 0, "Giornaliero", IIf(Eval("tipologia") = 1, "Annullati", "Modificati"))%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn Text="Visualizza il Registro Originale" HeaderStyle-Width="20px"
                                                                            ItemStyle-Width="20px" ButtonType="ImageButton" CommandName="PreviewDocumento"
                                                                            FilterControlAltText="Filter Preview column" ImageUrl="~\images\knob-search16.png"
                                                                            UniqueName="Preview" />
                                                                        <telerik:GridButtonColumn Text="Firma il Registro Giornaliero" HeaderStyle-Width="20px"
                                                                            ItemStyle-Width="20px" ButtonType="ImageButton" CommandName="FirmaRegistro" FilterControlAltText="Filter Preview column"
                                                                            ImageUrl="~\images\firmaDocumento16.png" UniqueName="FirmaRegistro" />

                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                            ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="10px"
                                                                            ItemStyle-Width="10px" ConfirmText="Sei sicuro che vuoi cancellare il registro?">
                                                                        </telerik:GridButtonColumn>

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
                            <td>
                                <div id="messageBoxPanel" style="position: absolute; width: 100%; text-align: center;
                                    z-index: 2000000; display: none; top: 300px">
                                    <div id="messageBox" style="width: 300px; height: 40; text-align: center; background-color: #BFDBFF;
                                        margin: 0 auto;">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </center>
                <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
