<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NotificaFatturaPage.aspx.vb"
    Inherits="NotificaFatturaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seleziona Esito</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>
<script type="text/javascript">

    var _backgroundElement = document.createElement("div");

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
                    window.close();
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
<body>
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
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent" style="overflow: auto; border: 0px solid red; height: 100%">
                <center>
                    <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text="Gestione Esito Fattura" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pannelloFattura" runat="server" Height="300px" Width="100%" Style="overflow: auto;">
                                                        </asp:Panel>
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
                                                                                &nbsp;<asp:Label ID="DatiNotificaLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                    color: #00156E; background-color: #BFDBFF" Text="Dati Notifica" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="overflow: auto; height: 40px; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">
                                                                        <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                            <tr>
                                                                                <td style="width: 120px">
                                                                                    <asp:Label ID="MessaggioLabel" runat="server" CssClass="Etichetta" Text="Messaggio" />
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadTextBox ID="txtMessaggio" runat="server" MaxLength="255" Rows="2" Skin="Office2007"
                                                                                        TextMode="MultiLine" Width="100%" Enabled="true" Style="overflow-x: hidden" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div id="GrigliaAllegatiPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                <tr>
                                                                    <td style="height: 20px">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr style="background-color: #FFFFFF">
                                                                    <td>
                                                                        <div style="overflow: auto; height: 100px; border: 1px solid #5D8CC9">
                                                                            <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco allegati associati alla fattura elettronica"
                                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                Width="99.8%" Culture="it-IT">
                                                                                <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                    <Columns>
                                                                                        <telerik:GridTemplateColumn SortExpression="Posizione" UniqueName="Posizione" HeaderText="Posizione"
                                                                                            DataField="Posizione" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("Posizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                    text-overflow: ellipsis; width: 60px; border: 0px solid red">
                                                                                                    <%# Eval("Posizione")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn SortExpression="Estremi" UniqueName="Estremi" HeaderText="Estremi"
                                                                                            DataField="Estremi" HeaderStyle-Width="230px" ItemStyle-Width="230px">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("Estremi")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                    text-overflow: ellipsis; width: 230px; border: 0px solid red">
                                                                                                    <%# Eval("Estremi")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                            DataField="NomeFile" HeaderStyle-Width="480px" ItemStyle-Width="480px">
                                                                                            <ItemTemplate>
                                                                                                <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                    text-overflow: ellipsis; width: 480px; border: 0px solid red">
                                                                                                    <%# Eval("NomeFile")%></div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                            Text="Visualizza Allegato..." ImageUrl="~\images\knob-search16.png" UniqueName="Preview"
                                                                                            HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                                                        </telerik:GridButtonColumn>
                                                                                    </Columns>
                                                                                </MasterTableView></telerik:RadGrid>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: right">
                                                        <telerik:RadButton ID="cmdEsitoPositivo" runat="server" Text="Accettazione" Width="190px"
                                                            Skin="Office2007">
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td style="text-align: right; width: 220px">
                                                        <telerik:RadButton ID="cmdEsitoNegativo" runat="server" Text="Rifiuto" Width="190px"
                                                            Skin="Office2007">
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td style="text-align: right; width: 220px">
                                                        <telerik:RadButton ID="NotificataScadutaButton" runat="server" Text="Continua Senza Notifica"
                                                            Width="190px" Skin="Office2007">
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <telerik:RadButton ID="cmdChiudi" runat="server" Text="Chiudi" Width="130px" Skin="Office2007">
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
                </center>
            </div>
            <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
