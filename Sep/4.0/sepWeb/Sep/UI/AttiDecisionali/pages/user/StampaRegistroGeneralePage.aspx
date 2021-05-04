<%@ Page Title="Stampa Registro Generale" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="StampaRegistroGeneralePage.aspx.vb" Inherits="StampaRegistroGeneralePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
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
                    <center>
            <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                DecorationZoneID="ZoneID" Skin="Web20"></telerik:RadFormDecorator>


            <table width="600px" cellpadding="5" cellspacing="5" border="0">

                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                            <%--  HEADER--%>
                            <tr>
                                <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    &nbsp;<asp:Label ID="TitoloLabel" 
                                        runat="server" Style="color: #00156E" Font-Bold="True"
                                        Text="Stampa Registro Generale" CssClass="Etichetta" />
                                </td>
                            </tr>
                            <%-- BODY--%>
                            <tr>
                                <td class="ContainerMargin">
                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <%--<div style="overflow: auto; height: 230px; width: 100%; background-color: #FFFFFF; border: 0px solid #5D8CC9;">--%>
                                                <div style="overflow: auto; height: 380px; width: 100%; background-color: #DFE8F6;
                                                    border: 0px solid #5D8CC9;">

                                                 
                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="ContatoreGeneraleLabel" runat="server" CssClass="Etichetta" Text="Registro Generale" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="ContatoreGeneraleInizioLabel" runat="server" CssClass="Etichetta"
                                                                                Text="da" />
                                                                        </td>
                                                                        <td style="width: 80px">
                                                                            <telerik:RadNumericTextBox ID="ContatoreGeneraleInizioTextBox" runat="server" Skin="Office2007" 
                                                                                     Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1" 
                                                                       ShowSpinButtons="True" ToolTip="" >
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                       </telerik:RadNumericTextBox>
                                                                        </td>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="ContatoreGeneraleFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadNumericTextBox ID="ContatoreGeneraleFineTextBox" runat="server" Skin="Office2007" 
                                                                                     Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1" 
                                                                       ShowSpinButtons="True" ToolTip="" >
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                       </telerik:RadNumericTextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <asp:Panel ID="ContatoreSettorePanel" runat="server">
                                                  
                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="ContatoreSettoreLabel" runat="server" CssClass="Etichetta" Text="Registro Settore" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="ContatoreSettoreInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                        </td>
                                                                        <td style="width: 80px">
                                                                             <telerik:RadNumericTextBox ID="ContatoreSettoreInizioTextBox" runat="server" Skin="Office2007" 
                                                                                     Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1" 
                                                                       ShowSpinButtons="True" ToolTip="" >
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                       </telerik:RadNumericTextBox>
                                                                        </td>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="ContatoreSettoreFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadNumericTextBox ID="ContatoreSettoreFineTextBox" runat="server" Skin="Office2007" 
                                                                                     Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1" 
                                                                       ShowSpinButtons="True" ToolTip="" >
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                       </telerik:RadNumericTextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
  </asp:Panel>

                                                    <asp:Panel ID="TipologiaRegistroPanel" runat="server">
                                                        <table style="width: 100%">
                                                            <tr style="height: 25px">
                                                                <td style="width: 120px">
                                                                    <asp:Label ID="TipologiaRegistroLabel" runat="server" CssClass="Etichetta" Text="Tipologia Registro" />
                                                                </td>
                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                     <telerik:RadComboBox ID="TipologieRegistroComboBox" AutoPostBack="true" runat="server" EmptyMessage="- Seleziona  -"
                                                                    MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="DataDocumentoLabel" runat="server" CssClass="Etichetta" Text="Data Documento" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="DataDocumentoInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                        </td>
                                                                        <td style="width: 80px">
                                                                            <telerik:RadDatePicker ID="DataDocumentoInizioTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                        <td align="center" style="width: 40px">
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
                                                    </table>


                                                        <asp:Panel ID="TipologiaSedutaPanel" runat="server">

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="TipologiaSedutaLabel" runat="server" CssClass="Etichetta" Text="Tipo Seduta" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="TipologieSedutaComboBox" AutoPostBack="true" runat="server"
                                                                    EmptyMessage="- Seleziona Tipologia-" MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel>


                                                      <asp:Panel ID="DataSedutaPanel" runat="server">

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="DataSedutaLabel" runat="server" CssClass="Etichetta" Text="Data Seduta" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="DataSedutaInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                        </td>
                                                                        <td style="width: 80px">
                                                                            <telerik:RadDatePicker ID="DataSedutaInizioTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="DataSedutaFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="DataSedutaFineTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    </asp:Panel>

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="SettoreLabel" runat="server" CssClass="Etichetta" Text="Settore" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="SettoreTextBox" runat="server" Skin="Office2007" 
                                                                                Width="100%" />
                                                                        </td>
                                                                        <td align="right" style="width: 60px">
                                                                            <asp:ImageButton ID="TrovaSettoreImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona ufficio (ALT + S)..." />&nbsp;
                                                                            <asp:ImageButton ID="EliminaSettoreImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella settore" />&nbsp;
                                                                            <asp:ImageButton ID="AggiornaSettoreImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none" />
                                                                            <telerik:RadTextBox ID="IdSettoreTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                Style="display: none" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="UfficioLabel" runat="server" CssClass="Etichetta" Text="Ufficio" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="UfficioTextBox" runat="server" Skin="Office2007" 
                                                                                Width="100%" />
                                                                        </td>
                                                                        <td align="right" style="width: 60px">
                                                                            <asp:ImageButton ID="TrovaUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona ufficio (ALT + S)..." />&nbsp;
                                                                            <asp:ImageButton ID="EliminaUfficioImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella settore" />&nbsp;
                                                                            <asp:ImageButton ID="AggiornaUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none" />
                                                                            <telerik:RadTextBox ID="IdUfficioTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                Style="display: none" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <%-- <asp:Panel ID="ApprovazionePanel" runat="server">
                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="StatoApprovazioneLabel" runat="server" CssClass="Etichetta" Text="Approvazione" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="StatiApprovazioneComboBox" AutoPostBack="false" runat="server"
                                                                    EmptyMessage="- Seleziona Stato -" MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    </asp:Panel>

                                                      <asp:Panel ID="AdottatePanel" runat="server">

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="AdottateLabel" runat="server" CssClass="Etichetta" Text="Adottate" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="AdottateComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona  -"
                                                                    MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    </asp:Panel>--%>

                                                     <asp:Panel ID="PubblicazioneAlboPanel" runat="server">

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="PubblicazioneAlboLabel" runat="server" CssClass="Etichetta" Text="Pubblicazione Albo" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="PubblicazioneAlboComboBox" AutoPostBack="false" runat="server"
                                                                    EmptyMessage="- Seleziona  -" MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    </asp:Panel>

                                                      <asp:Panel ID="DataPubblicazionePanel" runat="server">

                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="DataPubblicazioneLabel" runat="server" CssClass="Etichetta" Text="Data Pubblicazione" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="DataPubblicazioneInizioLabel" runat="server" CssClass="Etichetta"
                                                                                Text="da" />
                                                                        </td>
                                                                        <td style="width: 80px">
                                                                            <telerik:RadDatePicker ID="DataPubblicazioneInizioTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="DataPubblicazioneFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="DataPubblicazioneFineTextBox" Skin="Office2007" Width="110px"
                                                                                runat="server" MinDate="1753-01-01" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    </asp:Panel>


                                                      <asp:Panel ID="PubblicazioneWebPanel" runat="server">
                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="PubblicazioneWebLabel" runat="server" CssClass="Etichetta" Text="Pubblicazione WEB" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="PubblicazioneWebComboBox" AutoPostBack="false" runat="server"
                                                                    EmptyMessage="- Seleziona  -" MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel>

                                                     <asp:Panel ID="ModelloPanel" runat="server">
                                                    <table style="width: 100%">
                                                        <tr style="height: 25px">
                                                            <td style="width: 120px">
                                                                <asp:Label ID="ModelloLabel" runat="server" CssClass="Etichetta" Text="Modello" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="ModelliComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona  -"
                                                                    MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    </asp:Panel>

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
                        <telerik:RadButton ID="StampaButton" runat="server" Text="Stampa" Width="90px" Skin="Office2007"
                            ToolTip="Effettua la stampa con i filtri impostati">
                            <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                        </telerik:RadButton>
                        &nbsp;
                        <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="90px"
                            Skin="Office2007" ToolTip="Annulla i filtri impostati">
                            <Icon PrimaryIconUrl="../../../../images/Annulla.png" PrimaryIconLeft="5px" />
                        </telerik:RadButton>
                    </td>
                  </tr>
            </table>
            </td> 
               </tr> 

            </table>
        </center>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
