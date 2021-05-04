<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LiquidazionePage.aspx.vb"
    Inherits="LiquidazionePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Liquidazione</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <div id="pageContent">
        <center>
            <table width="900px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    &nbsp;<asp:Label ID="TitleLabel" 
                                        runat="server" Style="color: #00156E" Font-Bold="True"
                                        Text="Liquidazione" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ContainerMargin">
                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                    border: 0px solid #5D8CC9;">



                                                     <div style="padding:  0px 0px  2px  0px;">

                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td>
                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="ImpegnoSpesaLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                color: #00156E; background-color: #BFDBFF" Text="Dati Impegno di Spesa" />
                                                                        </td>
                                                                         <td style="text-align: right">
                                                <asp:ImageButton ID="TrovaImpegnoSpesaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                    ToolTip="Seleziona Impegno Spesa..." ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                        ID="AggiornaImpegnoSpesaImageButton" runat="server" Style="display: none" />
                                            </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div style="overflow: auto; height: 90px; width: 100%; background-color: #FFFFFF;
                                                                    border: 0px solid #5D8CC9;">

                                                                    <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                        <tr>
                                                                            <td style="vertical-align: top">
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 30px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="MissioneLabel" runat="server" CssClass="Etichetta" Text="M." ToolTip="Missione" />
                                                                                        </td>
                                                                                        <td style="width: 30px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="ProgrammaLabel" runat="server" CssClass="Etichetta" Text="P." ToolTip="Programma" />
                                                                                        </td>
                                                                                        <td style="width: 30px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="T." ToolTip="Titolo" />
                                                                                        </td>
                                                                                        <td style="width: 60px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="MacroAggregatoLabel" runat="server" CssClass="Etichetta" ToolTip="Macroaggregato"
                                                                                                Text="M. A." />
                                                                                        </td>
                                                                                        <td style="width: 95px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="AnnoLabel" runat="server" CssClass="Etichetta" Text="Anno" />
                                                                                        </td>
                                                                                        <td style="width: 70px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="CapitoloLabel" runat="server" CssClass="Etichetta" Text="Capitolo *" />
                                                                                        </td>
                                                                                        <td style="width: 70px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="ArticoloLabel" runat="server" CssClass="Etichetta" Text="Articolo" />
                                                                                        </td>
                                                                                        <td style="width: 150px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="ImpegnoLabel" runat="server" CssClass="Etichetta" Text="Impegno" />
                                                                                        </td>
                                                                                        <td style="width: 100px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="SubImpegnoLabel" runat="server" CssClass="Etichetta" Text="Sub Impegno" />
                                                                                        </td>
                                                                                        <td style="width: 200px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="ImportoLabel" runat="server" CssClass="Etichetta" Text="Importo *" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadMaskedTextBox Mask="##" PromptChar="_" ID="MissioneTextBox" runat="server"
                                                                                                SelectionOnFocus="CaretToBeginning" ToolTip="Missione" Width="20px">
                                                                                            </telerik:RadMaskedTextBox>
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadMaskedTextBox Mask="##" PromptChar="_" ID="ProgrammaTextBox" runat="server"
                                                                                                SelectionOnFocus="CaretToBeginning" ToolTip="Programma" Width="20px">
                                                                                            </telerik:RadMaskedTextBox>
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadMaskedTextBox Mask="#" PromptChar="_" ID="TitoloTextBox" runat="server"
                                                                                                SelectionOnFocus="CaretToBeginning" ToolTip="Titolo" Width="20px">
                                                                                            </telerik:RadMaskedTextBox>
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadMaskedTextBox Mask="##.##.##.###" PromptChar="_" ID="MacroAggregatoTextBox"
                                                                                                runat="server" SelectionOnFocus="CaretToBeginning" ToolTip="Macroaggregato" Width="70px">
                                                                                            </telerik:RadMaskedTextBox>
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadComboBox ID="AnniComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"
                                                                                                MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadNumericTextBox ID="CapitoloTextBox" runat="server" Skin="Office2007"
                                                                                                Width="100%" DataType="System.Int32" MaxLength="6">
                                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadNumericTextBox ID="ArticoloTextBox" runat="server" Skin="Office2007"
                                                                                                Width="100%" DataType="System.Int32" MaxLength="6">
                                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                           <telerik:RadTextBox ID="ImpegnoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                MaxLength="50" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <%--   <telerik:RadNumericTextBox ID="SubImpegnoTextBox" runat="server" Skin="Office2007"
                                                                                                Width="100%" DataType="System.Int32" MaxLength="6">
                                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>--%>
                                                                                            <telerik:RadTextBox ID="SubImpegnoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                MaxLength="50" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadNumericTextBox ID="ImportoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                MaxLength="10" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="DescrizionePianoPrecedenteLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="" />
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

                                                    </div>

                                                   <div style="padding:  2px 0px  0px  0px;">
                                                   

                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td>
                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="GeneraleLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                color: #00156E; background-color: #BFDBFF" Text="Dati Liquidazione" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                                    border: 0px solid #5D8CC9;">
                                                                    <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 90px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="AnnoLiquidazioneLabel" runat="server" CssClass="Etichetta" Text="Anno" />
                                                                                        </td>
                                                                                        <td style="width: 90px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="LiquidazioneLabel" runat="server" CssClass="Etichetta" Text="Liquidazione" />
                                                                                        </td>
                                                                                        <td style="width: 238px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="MandatoLabel" runat="server" CssClass="Etichetta" Text="Mandato" />
                                                                                        </td>
                                                                                        <td style="width: 238px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="ImportoLiquidazioneLabel" runat="server" CssClass="Etichetta" 
                                                                                                Text="Importo *" />
                                                                                        </td>
                                                                                        <td style="width: 210px; padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="DisponibilitaLabel" runat="server" CssClass="Etichetta" Text="Disponibilità residua" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                           
                                                                                            <telerik:RadComboBox ID="AnniLiquidazioneComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"
                                                                                                MaxHeight="150px" Skin="Office2007" Width="100%" />

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadNumericTextBox ID="LiquidazioneTextBox" runat="server" Skin="Office2007"
                                                                                                Width="100%" DataType="System.Int32" MaxLength="7">
                                                                                                <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                          

                                                                                             <telerik:RadTextBox ID="MandatoTextBox" MaxLength="100" runat="server" Skin="Office2007" Width="100%" />
                                                                                             

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            


                                                                                             <telerik:RadNumericTextBox ID="ImportoLiquidazioneTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                MaxLength="10" />

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                           

                                                                                             <telerik:RadNumericTextBox ID="DisponibilitaTextBox" runat="server" 
                                                                                                 Skin="Office2007" Width="100%"
                                                                                                MaxLength="19" ReadOnly="True" />


                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="ModalitaPagamentoLabel" runat="server" CssClass="Etichetta" 
                                                                                                Text="Modalità pagamento" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="CodiceFiscalePartitaIvaLabel" runat="server" CssClass="Etichetta"
                                                                                                Text="C.F - P. IVA" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="IbanLabel" runat="server" CssClass="Etichetta" Text="IBAN" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="CigLabel" runat="server" CssClass="Etichetta" Text="CIG" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="CupLabel" runat="server" CssClass="Etichetta" Text="CUP" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="DurcLabel" runat="server" CssClass="Etichetta" Text="DURC" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                          


                                                                                             <telerik:RadTextBox ID="ModalitaPagamentoTextBox" MaxLength="100" runat="server" Skin="Office2007" Width="100%" />

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px; width:120px">
                                                                                           

                                                                                             <telerik:RadTextBox ID="CodiceFiscalePartitaIvaTextBox" 
                                                                                                 style="text-transform:uppercase" MaxLength="16" runat="server" 
                                                                                                 Skin="Office2007" Width="100%" />

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px; width:170px">
                                                                                        

                                                                                              <telerik:RadTextBox ID="IbanTextBox" MaxLength="27" runat="server" 
                                                                                                  Skin="Office2007" Width="100%" />

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px; width:70px">
                                                                                          
                                                                                              <telerik:RadTextBox ID="CigTextBox" MaxLength="10" runat="server" 
                                                                                                  Skin="Office2007" Width="100%" />

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px; width:100">
                                                                                           

                                                                                             <telerik:RadTextBox ID="CupTextBox" MaxLength="15" runat="server" 
                                                                                                 Skin="Office2007" Width="100%" />

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px;width:100">
                                                                                         
                                                                                            <telerik:RadTextBox ID="DurcTextBox" MaxLength="14" runat="server" Skin="Office2007" Width="100%" />

                                                                                        </td>
                                                                                    </tr>
                                                                                </table>

                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="BeneficiarioLabel" runat="server" CssClass="Etichetta" Text="Beneficiario" />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="NumeroDeterminaLabel" runat="server" CssClass="Etichetta" Text="N. Det." />
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="AnnoDeterminaLabel" runat="server" CssClass="Etichetta" Text="Anno Det." />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <table style="width: 100%">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <telerik:RadTextBox ID="BeneficiarioTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                            MaxLength="500" />
                                                                                                    </td>
                                                                                                    <td style="width: 20px; text-align: right">
                                                                                                        <asp:ImageButton ID="TrovaBeneficiarioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                            ToolTip="Seleziona Beneficiario..." ImageAlign="AbsMiddle" BorderStyle="None"
                                                                                                            Style="height: 16px" /><asp:ImageButton ID="AggiornaBeneficiarioImageButton" runat="server"
                                                                                                                Style="display: none" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px; width: 100px">
                                                                                          

                                                                                            <telerik:RadNumericTextBox ID="NumeroDeterminaTextBox" runat="server" Skin="Office2007"
                                                                                                Width="100%" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                                ShowSpinButtons="True" ToolTip="Contatore generale della determina">
                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>

                                                                                        </td>
                                                                                        <td style="padding-left: 1px; padding-right: 1px; width: 80px">
                                                                                           

                                                                                                 <telerik:RadNumericTextBox ID="AnnoDeterminaTextBox" runat="server" Skin="Office2007"
                                                                                                Width="100%" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                                ShowSpinButtons="True" ToolTip="Anno della determina">
                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>

                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                Rows="4" TextMode="MultiLine" MaxLength="4000" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="QuietanzanteLabel" runat="server" CssClass="Etichetta" Text="Quietanzante" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadTextBox ID="QuietanzanteTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                MaxLength="100" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="NormaLabel" runat="server" CssClass="Etichetta" Text="Norma/Titolo" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadTextBox ID="NormaTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                MaxLength="500" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <asp:Label ID="ModalitaLabel" runat="server" CssClass="Etichetta" Text="Modalità" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                                            <telerik:RadTextBox ID="ModalitaTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                MaxLength="500" />
                                                                                        </td>
                                                                                    </tr>
                                                                               

                                                                                  <table style="width: 100%">

                                                                                  </table>

                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    </div>

                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="100px" Skin="Office2007"
                                        ToolTip="Salva l'accertamento">
                                        <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                    &nbsp;
                                    <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="100px"
                                        Skin="Office2007" ToolTip="Cancella i dati immessi">
                                        <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>

                                     <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="100px" AutoPostBack="false"
                                        Skin="Office2007" ToolTip="Chiudi">
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
