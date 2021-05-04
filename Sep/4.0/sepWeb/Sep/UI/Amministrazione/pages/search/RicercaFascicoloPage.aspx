<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaFascicoloPage.aspx.vb"
    Inherits="RicercaFascicoloPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Fascicoli</title>
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
            // _backgroundElement.className = "modalBackground";
            _backgroundElement.style.backgroundColor = '#09718F';
            _backgroundElement.style.filter = "alpha(opacity=20)";
            _backgroundElement.style.opacity = "0.2";
        }
        else {
            _backgroundElement.style.display = 'none';

        }
    }


</script>


<body runat="server" id="CorpoPagina">
    
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 150px; z-index: 2000000">
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
            <div id="pageContent">

          

                <center>

                    <asp:Panel ID="FiltroPanel" runat="server">

                        <table width="700px" cellpadding="5" cellspacing="5" border="0">
                            <tr>
                                <td>
                                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <%--  HEADER--%>
                                        <tr>
                                            <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                                border-top: 1px solid  #9ABBE8; height: 25px">
                                                &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                    Text="Filtro Fascicolo" CssClass="Etichetta" />
                                            </td>
                                        </tr>
                                        <%-- BODY--%>
                                        <tr>
                                            <td class="ContainerMargin">
                                                <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0"
                                                    style="background-color: #DFE8F6">
                                                    <tr>
                                                        <td>
                                                            <div style="overflow: auto; height: 300px; width: 100%; background-color: #DFE8F6;
                                                                border: 0px solid #5D8CC9;">

                                                                   <%-- CONTENT--%>

                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="TipoFascicoloLabel" runat="server" CssClass="Etichetta" Text="Tipo Fascicolo" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="TipologiaFascicoloComboBox" runat="server" EmptyMessage="Seleziona Tipo Fascicolo"
                                                                                Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                                Width="270px" Enabled="true" ToolTip="Tipo di fascicolo" />
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%">
                                                                 
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="NumeroDataProtocolloLabel" runat="server" CssClass="Etichetta" Text="Numero Registro" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="NumeroProtocolloInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                                    </td>
                                                                                    <td style="width: 70px">
                                                                                        <telerik:RadNumericTextBox ID="NumeroRegistroInizioTextBox" runat="server" Skin="Office2007"
                                                                                            Width="60px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                            ShowSpinButtons="True" ToolTip="Numero del registro  (inizio)">
                                                                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                        </telerik:RadNumericTextBox>
                                                                                    </td>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="NumeroProtocolloFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadNumericTextBox ID="NumeroRegistroFineTextBox" runat="server" Skin="Office2007"
                                                                                            Width="60px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                            ShowSpinButtons="True" ToolTip="Numero del registro (fine)">
                                                                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                        </telerik:RadNumericTextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%">
                                                                    <tr style="height: 25px">
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="DataAperturaLabel" runat="server" CssClass="Etichetta" Text="Data Apertura" />
                                                                        </td>
                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="DataInizioAperturaLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                                    </td>
                                                                                    <td style="width: 80px">
                                                                                        <telerik:RadDatePicker ID="DataInizioAperturaTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" 
                                                                                            ToolTip="Data apertura del fascicolo (inizio)">
                                                                                            <Calendar runat="server">
                                                                                                <SpecialDays>
                                                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                </SpecialDays>
                                                                                            </Calendar>
                                                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                                                        </telerik:RadDatePicker>
                                                                                    </td>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="DataFineAperturaLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadDatePicker ID="DataFineAperturaTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" 
                                                                                            ToolTip="Data apertura del fascicolo (fine)">
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
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%">
                                                                    <tr style="height: 25px">
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="DataChiusuraLabel" runat="server" CssClass="Etichetta" Text="Data Chiusura" />
                                                                        </td>
                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="DataInizioChiusuraLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                                    </td>
                                                                                    <td style="width: 80px">
                                                                                        <telerik:RadDatePicker ID="DataInizioChiusuraTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" 
                                                                                            ToolTip="Data chiusura del fascicolo (inizio)">
                                                                                            <Calendar>
                                                                                                <SpecialDays>
                                                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                </SpecialDays>
                                                                                            </Calendar>
                                                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                                                        </telerik:RadDatePicker>
                                                                                    </td>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="DataFineChiusuraLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadDatePicker ID="DataFineChiusuraTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" 
                                                                                            ToolTip="Data chiusura del fascicolo (fine)">
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

                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="CodiceFascicoloLabel" runat="server" CssClass="Etichetta" Text="Cod. Fascicolo" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                  
                                                                                    <td style="width: 320px">
                                                                                        <telerik:RadTextBox ID="CodiceFascicoloCompletoTextBox" runat="server" Skin="Office2007"
                                                                                            Width="310px" ToolTip="Codice completo del fascicolo"  />
                                                                                    </td>
                                                                                    <td style="width: 60px; text-align: center">
                                                                                        <asp:Label ID="StatoLabel" runat="server" CssClass="Etichetta" Text="Stato" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadComboBox ID="StatoFascicoloComboBox" runat="server" Skin="Office2007"
                                                                                            Width="130px" EmptyMessage="- Selezionare Stato-" ItemsPerRequest="10" Filter="StartsWith"
                                                                                            MaxHeight="300px" ToolTip="Stato del fascicolo" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="Label3" runat="server" CssClass="Etichetta" Text="Tipo Procedimento" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadComboBox ID="ProcedimentoComboBox" runat="server" EmptyMessage="Seleziona Tipo Procedimento"
                                                                                            Filter="StartsWith" ItemsPerRequest="10" MaxHeight="150px" Skin="Office2007"
                                                                                            Width="100%" Enabled="true" ToolTip="Tipo di procedimento" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="Label4" runat="server" CssClass="Etichetta" Text="Titolare" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadComboBox ID="TitololariComboBox" runat="server" Width="100%" Height="150"
                                                                                            EmptyMessage="Seleziona Titolare" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" 
                                                                                            Filter="StartsWith" Skin="Office2007"
                                                                                            LoadingMessage="Caricamento in corso..." 
                                                                                            ToolTip="Titolare del procedimento">
                                                                                            <WebServiceSettings Method="GetElementiRubrica" Path="RicercaFascicoloPage.aspx" />
                                                                                        </telerik:RadComboBox>
                                                                                    </td>
                                                                                    <td style="width: 25px; text-align: center">
                                                                                        <asp:ImageButton ID="TrovaBeneficiarioImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                            ToolTip="Seleziona Titolare ..." ImageAlign="AbsMiddle" />
                                                                                        <asp:ImageButton ID="AggiornaBeneficiarioImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                            Style="display: none" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" 
                                                                                            Width="100%" ToolTip="Oggetto del procedimento" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="ClassificazioneLabel" runat="server" CssClass="Etichetta" Text="Classificazione"
                                                                                AssociatedControlID="TrovaClassificazioneImageButton" AccessKey="T" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="ClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                            Width="100%" Enabled="False" 
                                                                                            ToolTip="Indice di classificazione completo" />
                                                                                    </td>
                                                                                    <td style="width: 1px; text-align: center">
                                                                                        <asp:ImageButton ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                            Style="display: none" />
                                                                                        <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                            Width="1px" Style="display: none" />
                                                                                    </td>
                                                                                    <td style="width: 30px; text-align: center">
                                                                                        <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                            ToolTip="Seleziona titolario classificazione (ALT + T) ..." />&nbsp;
                                                                                    </td>
                                                                                    <td style="width: 25px; text-align: center">
                                                                                        <asp:ImageButton ID="EliminaClassificazioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                            ToolTip="Cancella titolario classificazione " />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="ResponsabileLabel" runat="server" CssClass="Etichetta" Text="Responsabile"
                                                                                AssociatedControlID="TrovaResponsabileImageButton" AccessKey="S" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="ResponsabileTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                            Enabled="False" ToolTip="Nominativo del responsabile del procedimento" />
                                                                                    </td>
                                                                                    <td style="width: 1px; text-align: center">
                                                                                      <asp:ImageButton ID="AggiornaResponsabileImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                            Style="display: none" />
                                                                                        <telerik:RadTextBox ID="IdResponsabileTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                            Style="display: none" />
                                                                                    </td>
                                                                                    <td style="width: 30px; text-align: center">
                                                                                        <asp:ImageButton ID="TrovaResponsabileImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                            ToolTip="Seleziona responsabile (ALT + S)..." />
                                                                                    </td>
                                                                                    <td style="width: 25px; text-align: center">
                                                                                        <asp:ImageButton ID="EliminaResponsabileImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                            ToolTip="Cancella responsabile" />
                                                                                      
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
                                                <telerik:RadButton ID="CercaButton" runat="server" Text="Cerca" Width="100px" Skin="Office2007"
                                                    ToolTip="Effettua la ricerca con i filtri impostati">
                                                    <Icon PrimaryIconUrl="../../../../images/Ricerca.png" PrimaryIconLeft="5px" />
                                                </telerik:RadButton>
                                                &nbsp;&nbsp;
                                                <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="100px"
                                                    Skin="Office2007" ToolTip="Annulla i filtri impostati">
                                                    <Icon PrimaryIconUrl="../../../../images/Annulla.png" PrimaryIconLeft="5px" />
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>

                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
