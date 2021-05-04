<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaProtocolliPage.aspx.vb"
    Inherits="RicercaProtocolliPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Protocolli</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <div id="pageContent">
        <center>
            <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                DecorationZoneID="ZoneID" Skin="Web20"></telerik:RadFormDecorator>
            <table width="850px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                            <%--  HEADER--%>
                            <tr>
                                <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                        Text="Ricerca Registrazione" CssClass="Etichetta" />
                                </td>
                            </tr>
                            <%-- BODY--%>
                            <tr>
                                <td class="ContainerMargin">
                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <div style="overflow: auto; height: 460px; width: 100%; background-color: #DFE8F6;
                                                    border: 0px solid #5D8CC9;">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="TipoRegistrazioneLabel" runat="server" CssClass="Etichetta" Text="Tipo registrazione" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox Text="Arrivo" Checked="true" AutoPostBack="False" CssClass="Etichetta"
                                                                                ID="RegistrazioneArrivoCheckBox" GroupName="TipoRegistrazione" runat="server" />
                                                                            &nbsp;&nbsp;
                                                                            <asp:CheckBox Text="Partenza" AutoPostBack="False" CssClass="Etichetta" ID="RegistrazionePartenzaCheckBox"
                                                                                GroupName="TipoRegistrazione" runat="server" Checked="True" />
                                                                            &nbsp;&nbsp;
                                                                            <asp:CheckBox Text="Interna" AutoPostBack="False" CssClass="Etichetta" ID="RegistrazioneInternaCheckBox"
                                                                                GroupName="TipoRegistrazione" runat="server" Checked="True" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <telerik:RadComboBox ID="FiltroRegistrazioniAnnullateComboBox" runat="server" Skin="Office2007"
                                                                                Width="170px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                MaxHeight="400px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="NumeroDataProtocolloLabel" runat="server" CssClass="Etichetta" Text="Numero/Data prot."
                                                                    Width="150px" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 50px">
                                                                            <asp:Label ID="NumeroProtocolloInizioLabel" runat="server" CssClass="Etichetta" Text="N° da" />
                                                                        </td>
                                                                        <td style="width: 100px">
                                                                            <telerik:RadNumericTextBox ID="NumeroProtocolloInizioTextBox" runat="server" Skin="Office2007"
                                                                                Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                ShowSpinButtons="True" ToolTip="">
                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </td>
                                                                        <td style="width: 30px">
                                                                            <asp:Label ID="NumeroProtocolloFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td style="width: 130px">
                                                                            <telerik:RadNumericTextBox ID="NumeroProtocolloFineTextBox" runat="server" Skin="Office2007"
                                                                                Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                ShowSpinButtons="True" ToolTip="">
                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                            </telerik:RadNumericTextBox>
                                                                        </td>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="DataProtocolloInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadDatePicker ID="DataProtocolloInizioTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="DataProtocolloFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadDatePicker ID="DataProtocolloFineTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="MittenteDestinatarioLabel" runat="server" CssClass="Etichetta" Text="Mittente/Destinatario" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="DenominazioneCognomeLabel" runat="server" CssClass="Etichetta" Text="Denom./Cognome" />&nbsp;&nbsp;
                                                                            <telerik:RadTextBox ID="DenominazioneCognomeTextBox" runat="server" Skin="Office2007"
                                                                                Width="200px" />&nbsp;&nbsp;
                                                                            <asp:Label ID="NomeLabel" runat="server" CssClass="Etichetta" Text="Nome" />&nbsp;&nbsp;
                                                                            <telerik:RadTextBox ID="NomeTextBox" runat="server" Skin="Office2007" Width="240px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="CittaLabel" runat="server" CssClass="Etichetta" Text="Città" />&nbsp;&nbsp;
                                                                            <telerik:RadTextBox ID="CittaTextBox" runat="server" Skin="Office2007" Width="250px" />&nbsp;&nbsp;
                                                                            <asp:Label ID="EmailLabel" runat="server" CssClass="Etichetta" Text="E-mail" />&nbsp;&nbsp;
                                                                            <telerik:RadTextBox ID="EmailTextBox" runat="server" Skin="Office2007" Width="270px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="UfficioLabel" runat="server" CssClass="Etichetta" Text="Struttura" />
                                                                <asp:Label ID="AccessKeySLabel" Style="display: none; width: 0px" runat="server"
                                                                    CssClass="Etichetta" Text="" AssociatedControlID="TrovaStrutturaImageButton"
                                                                    AccessKey="S" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="UfficioTextBox" runat="server" Skin="Office2007" Width="370px" />&nbsp;
                                                                            <asp:ImageButton ID="TrovaStrutturaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona ufficio (ALT + S)..." />&nbsp;
                                                                            <asp:ImageButton ID="EliminaStrutturaImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella settore" />&nbsp;
                                                                            <asp:ImageButton ID="AggiornaStrutturaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none" />
                                                                            <telerik:RadTextBox ID="IdUfficioTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                Style="display: none" />&nbsp;
                                                                            <asp:CheckBox Text="Includi strutture dipendenti" ID="IncludiStruttureDipendentiCheckBox"
                                                                                runat="server" CssClass="Etichetta" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="FiltroOggettoTextBox" runat="server" Skin="Office2007" Width="500px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="ClassificazioneLabel" runat="server" CssClass="Etichetta" Text="Classificazione" />
                                                                <asp:Label ID="AccessKeyTLabel" Style="display: none; width: 0px" runat="server"
                                                                    CssClass="Etichetta" Text="" AssociatedControlID="TrovaClassificazioneImageButton"
                                                                    AccessKey="T" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="ClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                Width="360px" Enabled="False" />&nbsp;
                                                                            <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona titolario classificazione (ALT + T) ..." />&nbsp;
                                                                            <asp:ImageButton ID="EliminaClassificazioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella settore" />&nbsp;
                                                                            <asp:ImageButton ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none" />
                                                                            <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                Width="0px" Style="display: none" />&nbsp;
                                                                            <asp:CheckBox Text="Includi classifiche dipendenti" ID="IncludiClassificheDipendentiCheckBox"
                                                                                runat="server" CssClass="Etichetta" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="RiscontroLabel" runat="server" CssClass="Etichetta" Text="Riscontro" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 60px">
                                                                            <asp:Label ID="NumeroRiscontroLabel" runat="server" CssClass="Etichetta" Text="Numero" />
                                                                        </td>
                                                                        <td style="width: 120px">
                                                                            <telerik:RadTextBox ID="NumeroRiscontroTextBox" runat="server" Skin="Office2007"
                                                                                Width="100px" />
                                                                        </td>
                                                                        <td style="width: 50px">
                                                                            <asp:Label ID="AnnoRiscontroLabel" runat="server" CssClass="Etichetta" Text="Anno" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="AnnoRiscontroTextBox" runat="server" Skin="Office2007" Width="100px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="ProtocolloMittenteLabel" runat="server" CssClass="Etichetta" Text="Protocollo mittente" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="ProtocolloMittenteTextBox" runat="server" Skin="Office2007"
                                                                                Width="200px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="NoteLabel" runat="server" CssClass="Etichetta" Text="Note" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="630px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="NoteInterneLabel" runat="server" CssClass="Etichetta" Text="Note interne" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="NoteInterneTextBox" runat="server" Skin="Office2007" Width="630px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="TipoDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipo documento" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="TipologiaDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                Width="210px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                MaxHeight="400px" />
                                                                            &nbsp;&nbsp;
                                                                            <asp:Label ID="TipoRicezioneInvioLabel" runat="server" CssClass="Etichetta" Text="Tipo ricezione/Invio" />&nbsp;&nbsp;
                                                                            <telerik:RadComboBox ID="TipoRicezioneInvioComboBox" runat="server" Skin="Office2007"
                                                                                Width="210px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                MaxHeight="400px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="DataDocumentoLabel" runat="server" CssClass="Etichetta" Text="Data documento" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 30px">
                                                                            <asp:Label ID="DataDocumentoInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                        </td>
                                                                        <td style="width: 150px">
                                                                            <telerik:RadDatePicker ID="DataDocumentoInizioTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                        <td style="width: 30px">
                                                                            <asp:Label ID="DataDocumentoFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="DataDocumentoFineTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="DataRicezioneInvioLabel" runat="server" CssClass="Etichetta" Text="Data ricezione/invio" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 30px">
                                                                            <asp:Label ID="DataRicezioneInvioInizioLabel" runat="server" CssClass="Etichetta"
                                                                                Text="da" />
                                                                        </td>
                                                                        <td style="width: 150px">
                                                                            <telerik:RadDatePicker ID="DataRicezioneInvioInizioTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                        <td style="width: 30px">
                                                                            <asp:Label ID="DataRicezioneInvioFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="DataRicezioneInvioFineTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="OperatoreInserimentoLabel" runat="server" CssClass="Etichetta" Text="Operatore inserimento" />
                                                                <asp:Label ID="AccessKeyOLabel" Style="display: none; width: 0px" runat="server"
                                                                    CssClass="Etichetta" Text="" AssociatedControlID="TrovaUtenteImageButton" AccessKey="O" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="UtenteInserimentoTextBox" runat="server" Skin="Office2007"
                                                                                Width="450px" Enabled="False" />&nbsp;
                                                                            <asp:ImageButton ID="TrovaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona utente (ALT + O)..." />&nbsp;
                                                                            <asp:ImageButton ID="EliminaUtenteImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella utente" />&nbsp;
                                                                            <asp:ImageButton ID="AggiornaUtenteImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none" />
                                                                            <telerik:RadTextBox ID="IdUtenteInserimentoTextBox" runat="server" Skin="Office2007"
                                                                                Width="0px" Style="display: none" />
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
                                    <telerik:RadButton ID="CercaButton" runat="server" Text="Ok" Width="90px" Skin="Office2007"
                                        ToolTip="Effettua la ricerca con i filtri impostati">
                                        <Icon PrimaryIconUrl="../../../../images/check16.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                    &nbsp;
                                    <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="90px"
                                        Skin="Office2007" ToolTip="Annulla i filtri impostati">
                                        <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
