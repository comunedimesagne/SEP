<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InserimentoReferenteEsterno.aspx.vb"
    Inherits="InserimentoReferenteEsterno" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <center>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;
            background-color: #DFE8F6">
            <tr>
                <td>
                    <div style="display: none;">
                        <asp:Button runat="server" ID="DisabilitaPulsantePredefinito" OnClientClick="return false;"
                            Style="width: 0px; height: 0px;" />
                    </div>
                    <table style="width: 600px">
                        <tr>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 125px">
                                            <asp:Label ID="TipoPersonaLabel" runat="server" CssClass="Etichetta" Text="Tipo Persona" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="TipoPersonaComboBox" runat="server" EmptyMessage="- Seleziona -"
                                                Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                                Width="90px" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 125px">
                                            <asp:Label ID="DenominazioneLabel" runat="server" CssClass="Etichetta" Text="Denom./Cognome" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="DenominazioneTextBox" runat="server" Skin="Office2007" Width="455px"
                                                MaxLength="200" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 125px">
                                            <asp:Label ID="NomeLabel" runat="server" CssClass="Etichetta" Text="Nome" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="NomeTextBox" runat="server" Skin="Office2007" Width="455px"
                                                MaxLength="100" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 125px">
                                            <asp:Label ID="IndirizzoLabel" runat="server" CssClass="Etichetta" Text="Indirizzo" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="IndirizzoTextBox" runat="server" Skin="Office2007" Width="455px"
                                                MaxLength="200" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 125px">
                                            <asp:Label ID="CittaLabel" runat="server" CssClass="Etichetta" Text="Comune/Stato Est." />
                                        </td>
                                        <td style="width: 230px">
                                            <telerik:RadTextBox ID="CittaTextBox" runat="server" Skin="Office2007" Width="230px"
                                                MaxLength="40" />
                                        </td>
                                        <td style="width: 40px; text-align: center">
                                            <asp:Label ID="CapLabel" runat="server" CssClass="Etichetta" Text="Cap" Width="40px" />
                                        </td>
                                        <td style="width: 65px">
                                            <telerik:RadTextBox ID="CapTextBox" runat="server" Skin="Office2007" Width="65px"
                                                MaxLength="10" />
                                        </td>
                                        <td style="width: 40px; text-align: center">
                                            <asp:Label ID="ProvinciaLabel" runat="server" CssClass="Etichetta" Text="Prov." Width="40px" />
                                        </td>
                                        <td style="width: 25px">
                                            <telerik:RadTextBox ID="ProvinciaTextBox" runat="server" Skin="Office2007" Width="25px"
                                                MaxLength="2" />
                                        </td>
                                        <td style="width: 25px; text-align: center">
                                            <asp:ImageButton ID="TrovaComuneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                ToolTip="Seleziona comune di residenza..." ImageAlign="AbsMiddle" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="EliminaComuneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                ToolTip="Cancella comune di residenza" ImageAlign="AbsMiddle" />
                                            <asp:ImageButton ID="AggiornaComuneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                Style="display: none" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 125px">
                                            <asp:Label ID="LocalitaEsteraLabel" runat="server" CssClass="Etichetta" Text="Località Estera" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="LocalitaEsteraTextBox" runat="server" Skin="Office2007" Width="455px"
                                                MaxLength="100" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 125px">
                                            <asp:Label ID="EmailLabel" runat="server" CssClass="Etichetta" Text="E-mail" />
                                        </td>
                                        <td style="width: 390px">
                                            <telerik:RadTextBox ID="EmailTextBox" runat="server" Skin="Office2007" Width="390px"
                                                MaxLength="250" />
                                        </td>
                                        <td style="width: 45px; text-align: center">
                                            <asp:Label ID="RubricaLabel" runat="server" CssClass="Etichetta" Text="Rubr." Width="45px" />
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="RubricaCheckBox" runat="server" ButtonType="ToggleButton"
                                                Skin="Office2007" Text="" ToggleType="CheckBox" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: bottom">
                    <table style="width: 600px; background-color: #BFDBFF">
                        <tr style="height: 30px;">
                            <td align="center" valign="middle">
                                <telerik:RadButton ID="ConfermaButton" runat="server" Text="Conferma" Width="100px"
                                    Skin="Office2007">
                                    <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                                <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="100px"
                                    Skin="Office2007">
                                    <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="display: none;">
            <asp:Button runat="server" ID="SostituisciReferenteButton" Style="width: 0px; height: 0px;
                display: none" />
        </div>
    </center>
    </form>
</body>
</html>
