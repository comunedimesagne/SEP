<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ImpegnoSpesaPage.aspx.vb"
    Inherits="ImpegnoSpesaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Impegno di Spesa</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />


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
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitleLabel" runat="server" Style="color: #00156E; width: 400px"
                                                    Font-Bold="True" Text="Impegno" />
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="ContainerMargin">
                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                    border: 0px solid #5D8CC9;">

                                                     <table style="width:100%" id="FornitoriTable" runat="server">
                                                         <tr>
                                                             <td style="width:70px"><asp:Label ID="FornitoriLabel" runat="server" CssClass="Etichetta" Text="Fornitori"  /></td>
                                                              <td>


                                                           <%--       <telerik:RadComboBox ID="FornitoriComboBox" AutoPostBack="false" runat="server" EmptyMessage="Seleziona Fornitore"
                                                                    MaxHeight="150px" Skin="Office2007" Filter="Contains" ItemsPerRequest="10" Width="400px" />--%>

                                                                   <telerik:RadComboBox ID="FornitoriComboBox" runat="server" Width="400px" Height="150"
                                                                            EmptyMessage="Seleziona" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="Contains" Skin="Office2007"
                                                                            LoadingMessage="Caricamento in corso...">
                                                                            <WebServiceSettings Method="GetFornitori" Path="ImpegnoSpesaPage.aspx" />
                                                                        </telerik:RadComboBox>



                                                              </td>
                                                         </tr>
                                                     </table>

                                                    <table style="width: 100%">
                                                        <tr style="border 1px solid red">
                                                           <%-- <td style="width: 60px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="TitoloOldLabel" runat="server" CssClass="Etichetta" Text="Titolo" />
                                                            </td>
                                                            <td style="width: 60px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="FunzioneLabel" runat="server" CssClass="Etichetta" Text="Funzione" />
                                                            </td>
                                                            <td style="width: 60px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="ServizioLabel" runat="server" CssClass="Etichetta" Text="Servizio" />
                                                            </td>
                                                            <td style="width: 60px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="InterventoLabel" runat="server" CssClass="Etichetta" Text="Intervento" />
                                                            </td>--%>


                                                             <td style="width: 30px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="MissioneLabel" runat="server" CssClass="Etichetta" Text="M." ToolTip="Missione" />
                                                            </td>

                                                            <td style="width: 30px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="ProgrammaLabel" runat="server" CssClass="Etichetta" Text="P."  ToolTip="Programma" />
                                                            </td>

                                                            <td style="width: 30px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="T."  ToolTip="Titolo" />
                                                            </td>

                                                             <td style="width: 70px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="MacroAggregatoLabel" runat="server" CssClass="Etichetta"  ToolTip="Macroaggregato"
                                                                     Text="M. A." />
                                                            </td>



                                                            <td style="width: 95px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="AnnoLabel" runat="server" CssClass="Etichetta" Text="Anno" />
                                                            </td>
                                                            <td style="width: 70px; padding-left: 1px; padding-right: 1px">
                                                                
                                                                 <asp:Label ID="CapitoloLabel" runat="server" CssClass="Etichetta" Text="Capitolo*" />
                                                            </td>

                                                            <td>

                                                            </td>

                                                            <td style="width: 70px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="ArticoloLabel" runat="server" CssClass="Etichetta" Text="Articolo" />
                                                            </td>
                                                            <td style="width: 150px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="ImpegnoLabel" runat="server" CssClass="Etichetta" Text="Impegno" />
                                                            </td>

                                                            <td>

                                                            </td>

                                                            <td style="width: 100px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="SubImpegnoLabel" runat="server" CssClass="Etichetta" Text="Sub Impegno" />
                                                            </td>

                                                            <td style="width: 150px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="ImportoLabel" runat="server" CssClass="Etichetta" Text="Importo *" />
                                                            </td>

                                                        </tr>
                                                        <tr>

                                                             <td style="padding-left: 1px; padding-right: 1px;width: 30px">
                                                              <telerik:RadMaskedTextBox Mask="##" PromptChar="_" ID="MissioneTextBox" 
                                                                  runat="server" SelectionOnFocus="CaretToBeginning" 
                                                                  ToolTip="Missione" Width="25px">
                                                                </telerik:RadMaskedTextBox>
                                                            </td>

                                                             <td style="padding-left: 1px; padding-right: 1px;width: 30px">
                                                              
                                                                <telerik:RadMaskedTextBox Mask="##" PromptChar="_" ID="ProgrammaTextBox" 
                                                                     runat="server" SelectionOnFocus="CaretToBeginning" 
                                                                     ToolTip="Programma" Width="25px">
                                                                </telerik:RadMaskedTextBox>
                                                            </td>


                                                             <td style="padding-left: 1px; padding-right: 1px;width: 30px">
                                                                  <telerik:RadMaskedTextBox Mask="#" PromptChar="_" ID="TitoloTextBox" 
                                                                      runat="server" SelectionOnFocus="CaretToBeginning" 
                                                                      ToolTip="Titolo" Width="25px" >
                                                                </telerik:RadMaskedTextBox>
                                                            </td>

                                                             <td style="padding-left: 1px; padding-right: 1px;width: 70px">
                                                                  <telerik:RadMaskedTextBox Mask="##.##.##.###" PromptChar="_" 
                                                                      ID="MacroAggregatoTextBox" runat="server" SelectionOnFocus="CaretToBeginning" 
                                                                       ToolTip="Macroaggregato" Width="65px">
                                                                </telerik:RadMaskedTextBox>

                                                       

                                                            </td>

                                                            <%--<td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="TitoliComboBox" AutoPostBack="false" runat="server" Skin="Office2007"
                                                                    MaxHeight="150px" EmptyMessage="- Seleziona -" Width="100%" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="FunzioniComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"
                                                                    MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="ServiziComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"
                                                                    MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadComboBox ID="InterventiComboBox" AutoPostBack="false" runat="server"
                                                                    EmptyMessage="- Seleziona -" MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                            </td>--%>


                                                            <td style="padding-left: 1px; padding-right: 1px;width: 95px">
                                                                <telerik:RadComboBox ID="AnniComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"
                                                                    MaxHeight="150px" Skin="Office2007" Width="90px" />
                                                            </td>

                                                            <td style="padding-left: 1px; padding-right: 1px;width: 70px">
                                                                <telerik:RadNumericTextBox ID="CapitoloTextBox" runat="server" Skin="Office2007"
                                                                    Width="65px" DataType="System.Int32" MaxLength="6">
                                                                    <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                </telerik:RadNumericTextBox>
                                                            </td>

                                                            <td>
                                                <asp:ImageButton ID="TrovaCapitoloImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                    ToolTip="Seleziona Capitolo..." ImageAlign="AbsMiddle" BorderStyle="None" />
                                                    <asp:ImageButton ID="AggiornaCapitoloImageButton" runat="server" Style="display: none" />
                                                     <asp:ImageButton ID="AggiornaCapitoloJSibacImageButton" runat="server" Style="display: none" />
                                                      <asp:ImageButton ID="AggiornaCapitoloApkImageButton" runat="server" Style="display: none" />
                                                      <asp:ImageButton ID="AggiornaCapitoloDedaGroupImageButton" runat="server" Style="display: none" />
                                            </td>

                                                            <td style="padding-left: 1px; padding-right: 1px;width: 70px">
                                                                <telerik:RadNumericTextBox ID="ArticoloTextBox" runat="server" Skin="Office2007"
                                                                    Width="65px" DataType="System.Int32" MaxLength="6">
                                                                    <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator="" />
                                                                </telerik:RadNumericTextBox>
                                                            </td>

                                                            <td style="padding-left: 1px; padding-right: 1px;width: 150px">
                                                             

                                                                <telerik:RadTextBox ID="ImpegnoTextBox" runat="server" Skin="Office2007" Width="145px"  MaxLength="50" />

                                                            </td>

                                                            <td>
                                                             <asp:ImageButton ID="TrovaImpegnoJSibacImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                    ToolTip="Seleziona Impegno..." ImageAlign="AbsMiddle" BorderStyle="None" 
                                                                    style="height: 16px" />
                                                    <asp:ImageButton ID="AggiornaImpegnoJSibacImageButton" runat="server" Style="display: none" />
                                                    <asp:ImageButton ID="AggiornaImpegnoApkImageButton" runat="server" Style="display: none" />
                                                    <asp:ImageButton ID="AggiornaImpegnoDedaGroupImageButton" runat="server" Style="display: none" />
                                                   
                                                            </td>

                                                            <td style="padding-left: 1px; padding-right: 1px;width: 100px">
                                                              <telerik:RadTextBox ID="SubImpegnoTextBox" runat="server" Skin="Office2007" Width="95px"  MaxLength="50" />
                                                            </td>

                                                            <td style="padding-left: 1px; padding-right: 1px;width: 150px">
                                                                <telerik:RadNumericTextBox ID="ImportoTextBox" runat="server" Skin="Office2007" Width="145px"
                                                                    MaxLength="10" />
                                                            </td>

                                                        </tr>
                                                    </table>


                                                 

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 130px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="StanziamentoInizialeLabel" runat="server" CssClass="Etichetta" Text="Stanz. iniziale" />
                                                            </td>

                                                             <td style="width: 130px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="StanziamentoAssestatoLabel" runat="server" CssClass="Etichetta" Text="Stanz. assestato" />
                                                            </td>

                                                             <td style="width: 150px; padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="DisponibilitaResidualeLabel" runat="server" CssClass="Etichetta" Text="Disponibilità residuale" />
                                                            </td>

                                                             <td style=" padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="NumeroAttoAssunzioneLabel" runat="server" CssClass="Etichetta" Text="N° atto di assunzione" />
                                                            </td>

                                                            <td style="padding-left: 1px; padding-right: 1px; width:90px">
                                                                <asp:Label ID="CigLabel" runat="server" CssClass="Etichetta" Text="CIG" />
                                                            </td>


                                                        </tr>

                                                     
                                                       
                                             
                                                        <tr>
                                                             <td style="width: 130px; padding-left: 1px; padding-right: 1px;width: 130px">
                                                              <telerik:RadNumericTextBox ID="StanziamentoInizialeTextBox" runat="server" Skin="Office2007" Width="125px"
                                                                    MaxLength="10" />
                                                            </td>
                                                             <td style="width: 130px; padding-left: 1px; padding-right: 1px;width: 130px">
                                                              <telerik:RadNumericTextBox ID="StanziamentoAssestatoTextBox" runat="server" Skin="Office2007" Width="125px"
                                                                    MaxLength="10" />
                                                            </td>

                                                              <td style="width: 150px; padding-left: 1px; padding-right: 1px;width: 150px">
                                                              <telerik:RadNumericTextBox ID="DisponibilitaResidualeTextBox" runat="server" Skin="Office2007" Width="145px"
                                                                    MaxLength="10" />
                                                            </td>

                                                              <td style=" padding-left: 1px; padding-right: 1px">
                                                             <telerik:RadTextBox ID="NumeroAttoAssunzioneTextBox" runat="server" Skin="Office2007" Width="340px"
                                                               MaxLength="50" />
                                                            </td>

                                                            <td style="padding-left: 1px; padding-right: 1px; width:90px">
                                                                <telerik:RadTextBox ID="CigTextBox" MaxLength="10" runat="server" Skin="Office2007"
                                                                    Width="85px" />
                                                            </td>

                                                        </tr>
                                                    </table>



                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="860px"
                                                                    Rows="4" TextMode="MultiLine" MaxLength="4000" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="DescrizionePianoPrecedenteLabel" runat="server" CssClass="Etichetta" Text="" />
                                                               
                                                                
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
                                    <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="100px" Skin="Office2007"
                                        ToolTip="Salva l'impegno di spesa">
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

            <asp:HiddenField ID="IdImpegnoDedaGroup" runat="server" />
            <asp:HiddenField ID="IdCapitoloDedagroup" runat="server" />


        </div>
    </form>
</body>
</html>
