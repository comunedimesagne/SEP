<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VisualizzaRegistrazionePage.aspx.vb"
    Inherits="VisualizzaRegistrazionePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dettaglio Registrazione</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .rgAltRow, .rgRow
        {
            cursor: pointer !important;
        }
        .style1
        {
            height: 25px;
        }
    </style>
    <script type="text/javascript">



        var hide = true;
        var _backgroundElement = document.createElement("div");
        var overlay = document.createElement("div");

        function pageLoad() {

            $get("pageContent").appendChild(_backgroundElement);
            $get("pageContent").appendChild(overlay);

            if (hide) {
                HidePanel();
            } else {
                ShowPanel();
            }
        }

        function OnBeginRequest(sender, args) {
            EnableUI(false);
        }

        function OnEndRequest(sender, args) {
            EnableUI(true);
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


        function HidePanel() {
            var panel = document.getElementById("VisualizzaEmailPanel");
            panel.style.display = "none";
            overlay.style.display = 'none';
        }

        function ShowPanel() {

            var panel = document.getElementById("VisualizzaEmailPanel");

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

            var shadow = document.getElementById("ShadowPanel");

            with (shadow) {
                style.msFilter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.filter = "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#333333')";
                style.boxShadow = "3px 3px 4px #333";
                style.mozBoxShadow = "3px 3px 4px #333";
                style.webkitBoxShadow = "3px 3px 4px #333";
            }

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

                style.border = 'solid #FF9B35 1px';

                style.filter = "-ms-filter: progid:DXImageTransform.Microsoft.gradient(GradientType=0,startColorstr='#FFDB9B', endColorstr='#FFCB61')";

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

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px;
                z-index: 2000000">
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
    <asp:UpdatePanel ID="Pannello" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="pageContent">
                <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator>
                <center>
                    <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text="Dettaglio Registrazione" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                <tr>
                                                    <td valign="top">
                                                        <%--INIZIO CONTENUTO--%>
                                                        <table class="Container" cellpadding="0" cellspacing="4" style="width: 100%; height: 100%"
                                                            border="0">
                                                            <tr style="background-color: #DFE8F6">
                                                                <td valign="top">
                                                                    <table style="width: 900px; border: 1px solid #5D8CC9">
                                                                        <tr>
                                                                            <td>
                                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                                    <tr>
                                                                                        <td>
                                                                                            &nbsp;&nbsp;<asp:Label ID="AreaInfoLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                                                                color: #00156E; background-color: #BFDBFF" Text="" CssClass="Etichetta" />
                                                                                        </td>
                                                                                        <td align="center" style="width: 40px">
                                                                                            <img id="InfoUtenteImageButton" runat="server" src="~/images/userInfo.png" style="cursor: pointer;
                                                                                                border: 0px" alt="Informazioni sull'utente" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <telerik:RadTabStrip runat="server" ID="DatiProtocolloTabStrip" SelectedIndex="2"
                                                                                    MultiPageID="RadMultiPage1" Skin="Office2007" Width="100%">
                                                                                    <Tabs>
                                                                                        <telerik:RadTab Text="Generale" Selected="True" />
                                                                                        <telerik:RadTab Text="Avanzate" />
                                                                                        <telerik:RadTab Text="Documenti" />
                                                                                        <telerik:RadTab Text="Collegamenti" />
                                                                                        <telerik:RadTab Text="Fascicoli" />
                                                                                        <telerik:RadTab Text="Visibilità" />
                                                                                    </Tabs>
                                                                                </telerik:RadTabStrip>
                                                                                <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                                                                                <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" Height="350px"
                                                                                    Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                                                                                    <telerik:RadPageView runat="server" ID="GeneralePageView" CssClass="corporatePageView"
                                                                                        Height="350px">
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 150px">
                                                                                                    <asp:Label ID="TipologiaRegistrazioneLabel" runat="server" CssClass="Etichetta" Text="Tipologia registrazione" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:RadioButtonList ID="TipoRegistrazioneRadioList" runat="server" RepeatDirection="Horizontal"
                                                                                                        AutoPostBack="True">
                                                                                                        <asp:ListItem Text="Arrivo" Value="0" Selected="True" />
                                                                                                        <asp:ListItem Text="Partenza" Value="1" />
                                                                                                        <asp:ListItem Text="Interna" Value="2" />
                                                                                                    </asp:RadioButtonList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="height: 25px">
                                                                                                                <table style="width: 100%">
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="ReferenteEsternoLabel" runat="server" CssClass="Etichetta" Text="Mittenti"
                                                                                                                                AccessKey="R" AssociatedControlID="TrovaReferenteEsternoImageButton" />
                                                                                                                        </td>
                                                                                                                        <td align="right">
                                                                                                                            <telerik:RadTextBox ID="FiltroDenominazioneTextBox" runat="server" Skin="Office2007"
                                                                                                                                Width="300px" />
                                                                                                                        </td>
                                                                                                                        <td align="right" style="width: 90px; vertical-align: top">
                                                                                                                            <asp:ImageButton ID="TrovaReferenteEsternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                ToolTip="Seleziona referente esterno (ALT+R) ..." ImageAlign="AbsMiddle" />&nbsp;
                                                                                                                            <asp:ImageButton ID="TrovaPrimoReferenteInternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                ToolTip="Seleziona referente interno (ALT+1) ..." ImageAlign="AbsMiddle" />&nbsp;
                                                                                                                            <asp:ImageButton ID="AggiungiNuovoReferenteEsternoImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                                                ToolTip="Aggiungi nuovo referente esterno..." ImageAlign="AbsMiddle" /><asp:ImageButton
                                                                                                                                    ID="AggiornaNuovoReferenteEsternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                    Style="display: none" /><asp:ImageButton ID="AggiornaReferenteEsternoImageButton"
                                                                                                                                        runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" /><asp:ImageButton
                                                                                                                                            ID="AggiornaPrimoReferenteInternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                            Style="display: none" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div style="overflow: auto; height: 120px; border: 1px solid #5D8CC9">
                                                                                                                    <telerik:RadGrid ID="ReferentiEsterniGridView" runat="server" ToolTip="Elenco referenti associati al protocollo"
                                                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                                        Width="100%" Culture="it-IT">
                                                                                                                        <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                                                            <Columns>
                                                                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                                                                </telerik:GridBoundColumn>
                                                                                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderText="P.C."
                                                                                                                                    ItemStyle-Width="40px" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                                                                                    ItemStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:CheckBox ID="PerConoscenzaCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxIterTemplateColumn" HeaderText="Iter"
                                                                                                                                    ItemStyle-Width="40px" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                                                                                    ItemStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:CheckBox ID="IterCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                                                    HeaderText="Descrizione" DataField="Descrizione">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                                                                            overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red;">
                                                                                                                                            <%# Eval("Descrizione")%></div>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn SortExpression="Interno" UniqueName="Interno" HeaderText="Tipologia"
                                                                                                                                    DataField="Interno" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <div title='<%# IIf(CBool(Eval("Interno")), "Interno", "Esterno")%>' style="white-space: nowrap;
                                                                                                                                            overflow: hidden; text-overflow: ellipsis; width: 80px; border: 0px solid red">
                                                                                                                                            <%# IIf(CBool(Eval("Interno")), "Interno", "Esterno")%>
                                                                                                                                        </div>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Modifica" FilterControlAltText="Filter Modifica column"
                                                                                                                                    ImageUrl="~\images\Edit16.png" UniqueName="Modifica" HeaderStyle-Width="20px"
                                                                                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                                                                </telerik:GridButtonColumn>
                                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="20px"
                                                                                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
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
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="vertical-align: top; width: 70%">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="height: 25px">
                                                                                                                <table style="width: 100%">
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="SecondoReferenteInternoLabel" runat="server" CssClass="Etichetta"
                                                                                                                                Text="Destinatari" AccessKey="2" AssociatedControlID="TrovaSecondoReferenteInternoImageButton" />
                                                                                                                        </td>
                                                                                                                        <td align="right">
                                                                                                                            <telerik:RadTextBox ID="FiltroSecondoReferenteInternoTextBox" runat="server" Skin="Office2007"
                                                                                                                                Width="300px" />
                                                                                                                        </td>
                                                                                                                        <td align="right" style="width: 60px">
                                                                                                                            <asp:ImageButton ID="AggiornaSecondoReferenteInternoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                Style="display: none" /><asp:ImageButton ID="secondoReferenteInternoChechedButton"
                                                                                                                                    runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" /><telerik:RadTextBox
                                                                                                                                        ID="IdReferenteInternoTextBox" runat="server" Skin="Office2007" Style="display: none"
                                                                                                                                        Width="10px" /><asp:ImageButton ID="TrovaSecondoReferenteInternoImageButton" runat="server"
                                                                                                                                            ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona referente interno (ALT+2) ..." />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div style="overflow: auto; height: 120px; border: 1px solid #5D8CC9">
                                                                                                                    <telerik:RadGrid ID="SecondoReferentiInterniGridView" runat="server" ToolTip="Elenco referenti interni associati al protocollo"
                                                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                                        Width="100%" Culture="it-IT">
                                                                                                                        <MasterTableView DataKeyNames="Id" TableLayout="Fixed">
                                                                                                                            <Columns>
                                                                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                                                                </telerik:GridBoundColumn>
                                                                                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderText="P.C."
                                                                                                                                    HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                                                                                    ItemStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:CheckBox ID="PerConoscenzaCheckBox" runat="server" AutoPostBack="False" />
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxIterTemplateColumn" HeaderText="Iter"
                                                                                                                                    HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                                                                                    ItemStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:CheckBox ID="IterCheckBox" runat="server" AutoPostBack="False" />
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxInviaEmailTemplateColumn" HeaderText="Email"
                                                                                                                                    HeaderStyle-Width="45px" ItemStyle-Width="45px" HeaderStyle-HorizontalAlign="Center"
                                                                                                                                    ItemStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:CheckBox ID="InviaEmailCheckBox" runat="server" AutoPostBack="False" /></ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                                                                                    HeaderText="Descrizione" DataField="Descrizione">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                                                                            overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                            <%# Eval("Descrizione")%></div>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="20px"
                                                                                                                                    ItemStyle-Width="20px">
                                                                                                                                </telerik:GridButtonColumn>
                                                                                                                            </Columns>
                                                                                                                        </MasterTableView>
                                                                                                                    </telerik:RadGrid>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                                <td style="vertical-align: top; width: 30%">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <table style="width: 100%">
                                                                                                                    <tr valign="top">
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="OggettoLabel" runat="server" AccessKey="O" AssociatedControlID="TrovaOggettoImageButton"
                                                                                                                                CssClass="Etichetta" Text="Oggetto" />
                                                                                                                        </td>
                                                                                                                        <td align="right" style="width: 20px">
                                                                                                                            <asp:ImageButton ID="AggiornaOggettoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                Style="display: none" />
                                                                                                                        </td>
                                                                                                                        <td align="right" style="width: 20px">
                                                                                                                            <asp:ImageButton ID="TrovaOggettoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                ToolTip="Seleziona referente interno (ALT+O) ..." />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="OggettoTextBox" runat="server" MaxLength="500" Rows="7" Skin="Office2007"
                                                                                                                    TextMode="MultiLine" Width="350px" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TipologiaIterLabel" runat="server" CssClass="Etichetta" Text="Iter" />&nbsp;&nbsp;
                                                                                                                <telerik:RadComboBox ID="TipologiaIterComboBox" runat="server" Skin="Office2007"
                                                                                                                    Width="300px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                                                    MaxHeight="300px" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </telerik:RadPageView>
                                                                                    <telerik:RadPageView runat="server" ID="AvanzatePageView" CssClass="corporatePageView"
                                                                                        Height="350px">
                                                                                        <div style="padding: 2px 2px 2px 2px; width: 100%">
                                                                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                <tr>
                                                                                                    <td style="height: 20px">
                                                                                                        <table style="width: 100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    &nbsp;<asp:Label ID="DatiDocumentoLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Dati Documento" />
                                                                                                                </td>
                                                                                                                <td align="right">
                                                                                                                    <%--  pulsanti--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="background-color: #FFFFFF">
                                                                                                    <td>
                                                                                                        <table cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <table style="width: 100%">
                                                                                                                        <tr>
                                                                                                                            <%--  INIZIO CONTENUTO--%>
                                                                                                                            <td>
                                                                                                                                <asp:Label ID="DataOraRicezioneInvioLabel" runat="server" CssClass="Etichetta" Text="Data/Ora ricezione" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <telerik:RadDatePicker ID="DataRicezioneInvioTextBox" Skin="Office2007" Width="120px"
                                                                                                                                    runat="server" MinDate="1753-01-01" />
                                                                                                                                &nbsp;
                                                                                                                                <telerik:RadTimePicker ID="OrarioRicezioneInvioTextBox" Skin="Office2007" Width="70px"
                                                                                                                                    runat="server" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipologia" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <telerik:RadComboBox ID="TipologiaDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                                                                    Width="190" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                                                                    MaxHeight="300px" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <asp:Label ID="TipoRicezioneInvioLabel" runat="server" CssClass="Etichetta" Text="Tipo ricezione" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <telerik:RadComboBox ID="TipoRicezioneInvioComboBox" runat="server" EmptyMessage="- Selezionare -"
                                                                                                                                    Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                                                                                    Width="170" NoWrap="True" />
                                                                                                                            </td>
                                                                                                                            <%--  FINE CONTENUTO --%>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <table style="width: 100%">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <table>
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 100px">
                                                                                                                                            <asp:Label ID="ProtocolloMittenteLabel" runat="server" CssClass="Etichetta" Text="N. Protocollo" />
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <telerik:RadTextBox ID="ProtocolloMittenteTextBox" runat="server" Skin="Office2007"
                                                                                                                                                Width="100px" />
                                                                                                                                        </td>
                                                                                                                                        <td style="width: 70px; text-align: center">
                                                                                                                                            <asp:Label ID="DataDocumentoLabel" runat="server" CssClass="Etichetta" Text="Data" />
                                                                                                                                        </td>
                                                                                                                                        <td style="width: 120px">
                                                                                                                                            <telerik:RadDatePicker ToolTip="Data documento" ID="DataDocumentoTextBox" Skin="Office2007"
                                                                                                                                                Width="100px" runat="server" MinDate="1753-01-01" />
                                                                                                                                        </td>
                                                                                                                                        <td style="width: 130px; text-align: center">
                                                                                                                                            <asp:Label ID="AnticipatoViaFaxLabel" runat="server" CssClass="Etichetta" Text="Anticipato via fax" />&nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <asp:CheckBox ID="AnticipatoViaFaxCheckBox" runat="server" Text="&nbsp;" />
                                                                                                                                        </td>
                                                                                                                                        <td style="width: 60px; text-align: center">
                                                                                                                                            <asp:Label ID="StatoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Stato" />
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <telerik:RadComboBox ID="StatoDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                                                                                Width="190" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                                                                                MaxHeight="300px" />
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
                                                                                        </div>
                                                                                        <div style="padding: 2px 2px 2px 2px; width: 100%">
                                                                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                <tr>
                                                                                                    <td style="height: 20px">
                                                                                                        <table style="width: 100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    &nbsp;<asp:Label ID="ClassificazioneLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Classificazione" />
                                                                                                                </td>
                                                                                                                <td align="right">
                                                                                                                    <asp:Label ID="Label3" AccessKey="T" runat="server" Style="display: none" AssociatedControlID="TrovaClassificazioneImageButton" />
                                                                                                                    <asp:Label ID="Label1" AccessKey="V" runat="server" Style="display: none" AssociatedControlID="FiltraClassificazioneImageButton" />
                                                                                                                    <%--  pulsanti--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="background-color: #FFFFFF">
                                                                                                    <td>
                                                                                                        <asp:Panel runat="server" ID="FiltroClassificazionePanel">
                                                                                                            <table cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <table style="width: 100%">
                                                                                                                            <tr>
                                                                                                                                <td style="width: 35px">
                                                                                                                                    <telerik:RadTextBox ID="FiltroCategoriaTextBox" runat="server" Skin="Office2007"
                                                                                                                                        Width="30px" MaxLength="5" ToolTip="Imposta criterio di ricerca (Categoria)" />
                                                                                                                                </td>
                                                                                                                                <td style="width: 35px">
                                                                                                                                    <telerik:RadTextBox ID="FiltroClasseTextBox" runat="server" Skin="Office2007" Width="30px"
                                                                                                                                        MaxLength="5" ToolTip="Imposta criterio di ricerca (Classe)" />
                                                                                                                                </td>
                                                                                                                                <td style="width: 35px">
                                                                                                                                    <telerik:RadTextBox ID="FiltroSottoClasseTextBox" runat="server" Skin="Office2007"
                                                                                                                                        Width="30px" MaxLength="5" ToolTip="Imposta criterio di ricerca (Sotto-classe)" />
                                                                                                                                </td>
                                                                                                                                <td style="width: 45px">
                                                                                                                                    <asp:ImageButton ID="FiltraClassificazioneImageButton" runat="server" ImageUrl="~/images//refresh16.png"
                                                                                                                                        ToolTip="Filtra classificazione (ALT+V) ..." ImageAlign="AbsMiddle" />
                                                                                                                                    <asp:ImageButton ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                        Style="display: none" />
                                                                                                                                </td>
                                                                                                                                <td style="width: 310px">
                                                                                                                                    <telerik:RadTextBox ID="FiltroDescrizioneClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                                                                        Width="300px" MaxLength="50" ToolTip="Imposta criterio di ricerca (Descrizione)" />
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                                        ToolTip="Seleziona titolario di classificazione (ALT+T) ..." ImageAlign="AbsMiddle" />
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <table style="width: 100%">
                                                                                                                            <tr>
                                                                                                                                <td style="width: 610px">
                                                                                                                                    <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                                                                        Width="0px" Style="display: none" />
                                                                                                                                    <telerik:RadTextBox ID="ClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                                                                        Width="600px" MaxLength="50" ToolTip="Classificazione" Enabled="False" />
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <asp:ImageButton ID="EliminaClassificazioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                                                                        ToolTip="Cancella classificazione" ImageAlign="AbsMiddle" />
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </asp:Panel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="padding: 2px 2px 2px 2px; width: 100%">
                                                                                            <table style="width: 100%">
                                                                                                <tr>
                                                                                                    <td style="width: 50%">
                                                                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                            <tr>
                                                                                                                <td style="height: 20px">
                                                                                                                    <table style="width: 100%">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                &nbsp;
                                                                                                                                <asp:Label ID="NoteLabel" Style="color: #00156E" Font-Bold="True" runat="server"
                                                                                                                                    CssClass="Etichetta" Text="Note" />
                                                                                                                            </td>
                                                                                                                            <td align="right">
                                                                                                                                <asp:ImageButton ID="EliminaNoteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                                                                    ToolTip="Cancella note" ImageAlign="AbsMiddle" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr style="background-color: #FFFFFF">
                                                                                                                <td>
                                                                                                                    <table cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                                                    Rows="4" TextMode="MultiLine" MaxLength="1000" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td style="width: 50%">
                                                                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                            <tr>
                                                                                                                <td style="height: 20px">
                                                                                                                    <table style="width: 100%">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                &nbsp;
                                                                                                                                <asp:Label ID="NoteInterneLabel" Style="color: #00156E" Font-Bold="True" runat="server"
                                                                                                                                    CssClass="Etichetta" Text="Note Interne" />
                                                                                                                            </td>
                                                                                                                            <td align="right">
                                                                                                                                <asp:ImageButton ID="EliminaNoteInterneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                                                                    ToolTip="Cancella note interne" ImageAlign="AbsMiddle" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr style="background-color: #FFFFFF">
                                                                                                                <td>
                                                                                                                    <table cellpadding="0" cellspacing="4" width="100%" border="0">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <telerik:RadTextBox ID="NoteInterneTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                                                    Rows="4" TextMode="MultiLine" MaxLength="1000" />
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
                                                                                    <telerik:RadPageView runat="server" ID="DocumentiPageView" CssClass="corporatePageView"
                                                                                        Height="350px">
                                                                                        <div style="padding: 2px 2px 2px 2px; width: 100%">
                                                                                            <table style="width: 100%; background-color: #DFE8F6; display: none">
                                                                                                <tr style="display: none">
                                                                                                    <td style="width: 90px">
                                                                                                        <asp:Label ID="NumeroDocumentiLabel" runat="server" CssClass="Etichetta" Text="N. documenti" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <telerik:RadTextBox ID="NumeroDocumentiTextBox" runat="server" Skin="Office2007"
                                                                                                            Width="250px" />
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
                                                                                                <tr>
                                                                                                    <td style="width: 90px">
                                                                                                        <asp:Label ID="TipoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipo" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:RadioButton Text="Documento primario" Checked="true" AutoPostBack="False" ID="DocumentoPrimarioRadioButton"
                                                                                                            GroupName="TipoDocumento" runat="server" />&nbsp;&nbsp;
                                                                                                        <asp:RadioButton Text="Documento allegato" AutoPostBack="False" ID="DocumentoAllegatoRadioButton"
                                                                                                            GroupName="TipoDocumento" runat="server" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                            <table style="width: 100%; background-color: #DFE8F6; display: none">
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
                                                                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                <tr>
                                                                                                    <td style="height: 20px">
                                                                                                        <table style="width: 100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Documenti" />
                                                                                                                </td>
                                                                                                                <td align="right">
                                                                                                                    <asp:ImageButton ID="ScansionaImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                                                                        ToolTip="Allega documento digitalizzato" TabIndex="44" BorderStyle="None" ImageAlign="AbsMiddle"
                                                                                                                        Style="display: none" />&nbsp;<asp:ImageButton ID="AggiungiDocumentoImageButton"
                                                                                                                            runat="server" ImageUrl="~/images//add16.png" ToolTip="Allega documento" TabIndex="43"
                                                                                                                            ImageAlign="AbsMiddle" BorderStyle="None" Style="display: none" /><asp:ImageButton
                                                                                                                                ID="ScanUploadButton" Style="display: none" runat="server" ImageUrl="~/images//RecycleEmpty.png" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="background-color: #FFFFFF">
                                                                                                    <td>
                                                                                                        <div style="overflow: auto; height: 300px; border: 1px solid #5D8CC9">
                                                                                                            <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco documenti associati al protocollo"
                                                                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                                Width="99.8%" Culture="it-IT">
                                                                                                                <MasterTableView DataKeyNames="Id, Nomefile, NomeFileFirmato">
                                                                                                                    <Columns>
                                                                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                                                            <HeaderStyle Width="10px" />
                                                                                                                            <ItemStyle Width="10px" />
                                                                                                                        </telerik:GridBoundColumn>
                                                                                                                        <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="N.">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="NumeratoreLabel" runat="server" Width="10px" />
                                                                                                                            </ItemTemplate>
                                                                                                                            <HeaderStyle Width="10px" />
                                                                                                                            <ItemStyle Width="10px" />
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                                            DataField="NomeFile" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 250px">
                                                                                                                                    <%# Eval("NomeFile")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="ImprontaEsadecimale" UniqueName="ImprontaEsadecimale"
                                                                                                                            HeaderText="Impronta" DataField="ImprontaEsadecimale" HeaderStyle-Width="260px"
                                                                                                                            ItemStyle-Width="260px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("ImprontaEsadecimale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 260px;">
                                                                                                                                    <%# Eval("ImprontaEsadecimale")%></div>
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
                                                                                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneTipologiaDocumento" UniqueName="DescrizioneTipologiaDocumento"
                                                                                                                            HeaderText="Tipo" DataField="DescrizioneTipologiaDocumento" HeaderStyle-Width="70px"
                                                                                                                            ItemStyle-Width="70px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("DescrizioneTipologiaDocumento")%>' style="white-space: nowrap;
                                                                                                                                    overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                                                                                    <%# Eval("DescrizioneTipologiaDocumento")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="SignedPreview" CommandName="SignedPreview"
                                                                                                                            ImageUrl="~/images/signedDocument16.png" ItemStyle-Width="16px" HeaderStyle-Width="16px" />
                                                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                                                            ImageUrl="~\images\knob-search16.png" UniqueName="Preview">
                                                                                                                            <HeaderStyle Width="10px" />
                                                                                                                            <ItemStyle Width="10px" />
                                                                                                                        </telerik:GridButtonColumn>
                                                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                            ImageUrl="~\images\Delete16.png" UniqueName="Delete">
                                                                                                                            <HeaderStyle Width="10px" />
                                                                                                                            <ItemStyle Width="10px" />
                                                                                                                        </telerik:GridButtonColumn>
                                                                                                                    </Columns>
                                                                                                                </MasterTableView></telerik:RadGrid>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </telerik:RadPageView>
                                                                                    <telerik:RadPageView runat="server" ID="CollegamentiPageView" CssClass="corporatePageView"
                                                                                        Height="350px">
                                                                                        <div style="padding: 2px 2px 2px 2px; width: 100%">
                                                                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                <tr>
                                                                                                    <td style="height: 20px">
                                                                                                        <table style="width: 100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="CollegamentiDirettiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Collegamenti diretti" /><telerik:RadTextBox
                                                                                                                            ID="RiscontroNumeroProtocolloTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                                                            Style="display: none" /><telerik:RadTextBox ID="RiscontroDataImmissioneProtocolloTextBox"
                                                                                                                                runat="server" Skin="Office2007" Width="0px" Style="display: none" />
                                                                                                                </td>
                                                                                                                <td align="right">
                                                                                                                    <asp:ImageButton ID="TrovaCollegamentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                        ToolTip="Seleziona protocollo..." />&nbsp;
                                                                                                                    <asp:ImageButton ID="AggiornaCollegamentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                        Style="display: none" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="background-color: #FFFFFF">
                                                                                                    <td>
                                                                                                        <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                                            <telerik:RadGrid ID="CollegamentiDirettiGridView" runat="server" ToolTip="Elenco collegamenti diretti associati al protocollo"
                                                                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                                Width="99.8%" Culture="it-IT">
                                                                                                                <MasterTableView DataKeyNames="Id,NumeroProtocollo,AnnoProtocollo">
                                                                                                                    <Columns>
                                                                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                                                                        <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                                                                                            HeaderText="N. prot." DataField="NumeroProtocollo" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("NumeroProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 65px;">
                                                                                                                                    <%# Eval("NumeroProtocollo")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="AnnoProtocollo" UniqueName="AnnoProtocollo"
                                                                                                                            HeaderText="Anno" DataField="AnnoProtocollo" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("AnnoProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 30px;">
                                                                                                                                    <%# Eval("AnnoProtocollo")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                                                                            DataField="Oggetto" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 300px;">
                                                                                                                                    <%# Eval("Oggetto")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="Uffici" UniqueName="Uffici" HeaderText="Uffici"
                                                                                                                            DataField="Uffici" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("Uffici")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                                                    width: 200px;">
                                                                                                                                    <%# Eval("Uffici")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="Referenti" UniqueName="Referenti" HeaderText="Referenti"
                                                                                                                            DataField="Referenti" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("Referenti")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 200px;">
                                                                                                                                    <%# Eval("Referenti")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Detail" FilterControlAltText="Filter Detail column"
                                                                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                                                                            ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\text_preview.png" UniqueName="Detail" />
                                                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                                                                            ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\Delete16.png" UniqueName="Delete" />
                                                                                                                    </Columns>
                                                                                                                </MasterTableView></telerik:RadGrid>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div style="padding: 2px 2px 2px 2px; width: 100%">
                                                                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                                                <tr>
                                                                                                    <td style="height: 20px">
                                                                                                        <table style="width: 100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="CollegamentiIndirettiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Collegamenti indiretti" />
                                                                                                                </td>
                                                                                                                <td align="right">
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="background-color: #FFFFFF">
                                                                                                    <td>
                                                                                                        <div style="overflow: auto; height: 135px; border: 1px solid #5D8CC9">
                                                                                                            <telerik:RadGrid ID="CollegamentiIndirettiGridView" runat="server" ToolTip="Elenco collegamenti indiretti associati al protocollo"
                                                                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                                Width="99.8%" Culture="it-IT">
                                                                                                                <MasterTableView DataKeyNames="Id,NumeroProtocollo,AnnoProtocollo">
                                                                                                                    <Columns>
                                                                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                                                            HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                                                                        <telerik:GridTemplateColumn SortExpression="NumeroProtocollo" UniqueName="NumeroProtocollo"
                                                                                                                            HeaderText="N. prot." DataField="NumeroProtocollo" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("NumeroProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 65px;">
                                                                                                                                    <%# Eval("NumeroProtocollo")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="AnnoProtocollo" UniqueName="AnnoProtocollo"
                                                                                                                            HeaderText="Anno" DataField="AnnoProtocollo" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("AnnoProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 30px;">
                                                                                                                                    <%# Eval("AnnoProtocollo")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                                                                            DataField="Oggetto" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 300px;">
                                                                                                                                    <%# Eval("Oggetto")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="Uffici" UniqueName="Uffici" HeaderText="Uffici"
                                                                                                                            DataField="Uffici" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("Uffici")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                                                    width: 200px;">
                                                                                                                                    <%# Eval("Uffici")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn SortExpression="Referenti" UniqueName="Referenti" HeaderText="Referenti"
                                                                                                                            DataField="Referenti" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div title='<%# Eval("Referenti")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                    text-overflow: ellipsis; width: 200px;">
                                                                                                                                    <%# Eval("Referenti")%></div>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Detail" FilterControlAltText="Filter Detail column"
                                                                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                                                                            ItemStyle-VerticalAlign="Middle" ImageUrl="~\images\text_preview.png" UniqueName="Detail" />
                                                                                                                    </Columns>
                                                                                                                </MasterTableView></telerik:RadGrid></div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </telerik:RadPageView>
                                                                                    <telerik:RadPageView runat="server" ID="FascicoliPageView" CssClass="corporatePageView"
                                                                                        Height="350px">
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
                                                                                                            <div id="scrollPanelFascicoli" style="overflow: auto; height: 310px; border: 1px solid #5D8CC9">
                                                                                                                <telerik:RadGrid ID="FascicoliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                                                                    PageSize="5" Culture="it-IT">
                                                                                                                    <MasterTableView DataKeyNames="Id">
                                                                                                                        <Columns>
                                                                                                                            <telerik:GridBoundColumn DataField="Id" Visible="False" />
                                                                                                                            <telerik:GridBoundColumn DataField="idDocumento" Visible="False" />
                                                                                                                            <telerik:GridTemplateColumn SortExpression="IdentificativoFascicolo" UniqueName="IdentificativoFascicolo"
                                                                                                                                HeaderText="Cod. Fascicolo" DataField="CodiceFascicolo" HeaderStyle-Width="145px"
                                                                                                                                ItemStyle-Width="145px">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <div title='<%# Eval("IdentificativoFascicolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                        text-overflow: ellipsis; width: 145px; border: 0px solid red">
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
                                                                                                                                HeaderText="Tipo Procedimento" DataField="DescrizioneProcedimento" HeaderStyle-Width="180px"
                                                                                                                                ItemStyle-Width="180px">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <div title='<%# Eval("DescrizioneProcedimento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                        text-overflow: ellipsis; width: 180px; border: 0px solid red">
                                                                                                                                        <%# Eval("DescrizioneProcedimento")%></div>
                                                                                                                                </ItemTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                                                                                                DataField="Oggetto">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                        text-overflow: ellipsis; width: 200px; border: 0px solid red">
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
                                                                                    <telerik:RadPageView runat="server" ID="VisibilitaPageView" CssClass="corporatePageView"
                                                                                        Height="350px">
                                                                                        <div style="padding: 2px 2px 2px 2px; width: 100%">
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
                                                                                                        <div style="overflow: auto; height: 280px; border: 1px solid #5D8CC9">
                                                                                                            <telerik:RadGrid ID="VisibilitaGridView" runat="server" ToolTip="Elenco utenti o gruppi associati al protocollo"
                                                                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                                Width="99.8%" Culture="it-IT">
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
                                                                                        <div id="ZoneID2" style="padding: 2px 2px 2px 2px; width: 100%">
                                                                                            <table style="width: 100%">
                                                                                                <tr>
                                                                                                    <td style="width: 90px">
                                                                                                        <asp:Label ID="RiservatoLabel" runat="server" CssClass="Etichetta" Text="Riservato" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <div disabled="true">
                                                                                                            <asp:CheckBox ID="RiservatoCheckBox" runat="server" CssClass="etichetta" Text=""
                                                                                                                Width="90px" />
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </telerik:RadPageView>
                                                                                    
                                                                                </telerik:RadMultiPage>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <%--FINE CONTENUTO--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="100px" Skin="Office2007">
                                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
                <div id="VisualizzaEmailPanel" style="position: absolute; width: 100%; text-align: center;
                    z-index: 10001; display: none; top: 10px">
                    <div id="ShadowPanel" style="width: 700px; text-align: center; background-color: #BFDBFF;
                        margin: 0 auto">
                        <table width="700px" cellpadding="5" cellspacing="5" border="0">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <%--  HEADER--%>
                                        <tr>
                                            <td style="background-color: #BFDBFF; padding: 0px; height: 25px">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            &nbsp;<asp:Label ID="TitoloPannelloEmailLabel" runat="server" CssClass="Etichetta"
                                                                Font-Bold="True" Style="width: 600px; color: #00156E; background-color: #BFDBFF"
                                                                Text="Anteprima Email" />
                                                        </td>
                                                        <td align="right">
                                                            <div id="buttonPanel" runat="server">
                                                                <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="document.getElementById('<%= Me.ChiudiAnteprimaEmailButton.ClientID %>').click();" />
                                                            </div>
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
                                                            <div id="Div2" runat="server" style="padding: 2px 2px 2px 2px; overflow: auto; height: 150px;
                                                                width: 100%;">
                                                                <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="OggettoEmailLabel" runat="server" CssClass="Etichetta" Style="color: #00156E;
                                                                                background-color: #DFE8F6" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="DataEmailLabel" runat="server" CssClass="Etichetta" Style="color: #00156E;
                                                                                background-color: #DFE8F6" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="MittenteEmailLabel" runat="server" CssClass="Etichetta" Style="color: #00156E;
                                                                                background-color: #DFE8F6" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="DestinatarioEmailLabel" runat="server" CssClass="Etichetta"
                                                                                Style="color: #00156E; background-color: #DFE8F6" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="pannelloEmail" runat="server" style="overflow: auto; height: 150px; width: 700px;
                                                                border: 0px solid #5D8CC9;">
                                                                <asp:Literal ID="contenutoEmail" runat="server"> </asp:Literal>
                                                            </div>
                                                            <div id="AllegatiEmailPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                    <tr>
                                                                        <td style="height: 20px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 320px">
                                                                                        <asp:Label ID="AllegatiEmailLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                            Style="color: #00156E; background-color: #BFDBFF" Text="Allegati" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 110px; text-align: right">
                                                                                                    <asp:Label ID="PosizioneTimbroLabel" runat="server" CssClass="Etichetta" Text="Posizione Timbro" />
                                                                                                </td>
                                                                                                <td style="text-align: right">
                                                                                                    <telerik:RadComboBox ID="PosizioneTimbroComboBox" runat="server" Skin="Office2007"
                                                                                                        EmptyMessage="Nessuno" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="200px"
                                                                                                        Width="200px" ZIndex="60000" />
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
                                                                            <div style="overflow: auto; height: 115px; border: 1px solid #5D8CC9">
                                                                                <telerik:RadGrid ID="AllegatiEmailGridView" runat="server" ToolTip="Elenco allegati associati all'email"
                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                    Width="99.8%" Culture="it-IT">
                                                                                    <MasterTableView DataKeyNames="Id, Nomefile">
                                                                                        <Columns>
                                                                                            <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                                DataField="NomeFile">
                                                                                                <ItemTemplate>
                                                                                                    <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                        text-overflow: ellipsis; width: auto; border: 0px solid red">
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
                                    </table>
                                </td>
                            </tr>
                            <%-- FOOTER--%>
                            <tr>
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    <telerik:RadButton ID="VisualizzaFatturaButton" runat="server" Text="Apri Fattura"
                                        Width="110px" Skin="Office2007" ToolTip="Visualizza Fattura">
                                        <Icon PrimaryIconUrl="../../../../images/xml_16.png" PrimaryIconLeft="3px" />
                                    </telerik:RadButton>
                                    &nbsp;
                                    <telerik:RadButton ID="VisualizzaEmailButton" runat="server" Text="Apri/Salva" Width="100px"
                                        Skin="Office2007" ToolTip="Apri/Salva Email">
                                        <Icon PrimaryIconUrl="../../../../images/attachment.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                    &nbsp;
                                    <telerik:RadButton ID="StampaEmailButton" runat="server" Text="Stampa" Width="100px"
                                        Skin="Office2007" ToolTip="Stampa Email">
                                        <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                    &nbsp;
                                    <telerik:RadButton ID="ChiudiAnteprimaEmailButton" runat="server" Text="Chiudi" Width="100px"
                                        Skin="Office2007" ToolTip="Chiudi">
                                        <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
