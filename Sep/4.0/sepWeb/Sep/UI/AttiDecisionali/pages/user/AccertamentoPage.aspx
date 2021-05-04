<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AccertamentoPage.aspx.vb"
    Inherits="AccertamentoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Accertamento</title>
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
                                        <td  style="background-color: #BFDBFF;padding: 4px; border-bottom:1px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                           

                                                 <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                 <tr>
                                                   <td>
                                                    &nbsp;<asp:Label ID="TitleLabel" 
                                                runat="server" Style="color: #00156E" Font-Bold="True"  Text="Accertamento" />
                                                   </td>
                                                      <td style="text-align: right">
                                                <asp:ImageButton ID="TrovaCapitoloImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                    ToolTip="Seleziona Capitolo..." ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                        ID="AggiornaCapitoloImageButton" runat="server" Style="display: none" />
                                            </td>
                                                 </tr>
                                                 </table>


                                        </td>
                                     
                                    </tr>

                                    <tr>
                                        <td class="ContainerMargin">

                                            <table class="Container" cellpadding="0px" cellspacing="4px" width="100%" >
                                                <tr>
                                                         <td>
                                                                    <div style="overflow: auto; height:100%; width: 100%; background-color: #FFFFFF;
                                                                        border: 0px solid #5D8CC9;">


                                                                        <table  style="width:100%" >
                                                                            <tr>
                                                                                <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" 
                                                                                        Text="Titolo *" />
                                                                                </td>
                                                                                <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="CategoriaLabel" runat="server" CssClass="Etichetta" Text="Categoria" />
                                                                                </td>
                                                                               <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="RisorsaLabel" runat="server" CssClass="Etichetta" Text="Risorsa" />
                                                                                </td>
                                                                               <td style="width:70px;padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="AnnoLabel" runat="server" CssClass="Etichetta" Text="Anno" />
                                                                                </td>
                                                                              <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="CapitoloLabel" runat="server" CssClass="Etichetta" 
                                                                                        Text="Capitolo *" />
                                                                                </td>
                                                                              <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="ArticoloLabel" runat="server" CssClass="Etichetta" Text="Articolo" />
                                                                                </td>
                                                                              <td style="width:110px;padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="AccertamentoLabel" runat="server" CssClass="Etichetta" Text="Accertamento" />
                                                                                </td>
                                                                             <td style="width:110px;padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="SubAccertamentoLabel" runat="server" CssClass="Etichetta" 
                                                                                        Text="Sub Accertam." />
                                                                                </td>
                                                                            <td style="padding-left:1px; padding-right:1px">
                                                                                    <asp:Label ID="ImportoLabel" runat="server" CssClass="Etichetta" Text="Importo *" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>

                                                                                <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                    <telerik:RadComboBox ID="TitoliComboBox" AutoPostBack="false" runat="server" Skin="Office2007"
                                                                                        MaxHeight="150px" EmptyMessage="- Seleziona -" Width="100%" />
                                                                                </td>

                                                                                 <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                    <telerik:RadComboBox ID="CategorieComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"
                                                                                        MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                                                </td>

                                                                              <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                 <telerik:RadNumericTextBox ID="RisorsaTextBox" runat="server" Skin="Office2007" 
                                                                                       Width="100%" DataType="System.Int32" MaxLength="5">
                                                                                  <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator=""  /> 
                                                                                 </telerik:RadNumericTextBox>
                                                                                </td>

                                                                               <td style="width:70px;padding-left:1px; padding-right:1px">
                                                                               
                                                                                   <telerik:RadComboBox ID="AnniComboBox" AutoPostBack="false" runat="server" EmptyMessage="- Seleziona -"  MaxHeight="150px" Skin="Office2007"
                                                                             Width="100%" />
                                                                                </td>


                                                                                <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                 <telerik:RadNumericTextBox ID="CapitoloTextBox" runat="server" Skin="Office2007" 
                                                                                        Width="100%" DataType="System.Int32" MaxLength="6">
                                                                                   <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator=""  /> 
                                                                                 </telerik:RadNumericTextBox>
                                                                                </td>

                                                                                 <td style="width:90px;padding-left:1px; padding-right:1px">
                                                                                 <telerik:RadNumericTextBox ID="ArticoloTextBox" runat="server" Skin="Office2007" 
                                                                                         Width="100%" DataType="System.Int32" MaxLength="6">
                                                                                  <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator=""  /> 
                                                                                 </telerik:RadNumericTextBox>
                                                                                </td>

                                                                                     <td style="width:110px;padding-left:1px; padding-right:1px">
                                                                                 <telerik:RadNumericTextBox ID="AccertamentoTextBox" runat="server" 
                                                                                         Skin="Office2007" Width="100%" DataType="System.Int32" MaxLength="7">
                                                                                  <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator=""  /> 
                                                                                 </telerik:RadNumericTextBox>
                                                                                </td>

                                                                                   <td style="width:110px;padding-left:1px; padding-right:1px">
                                                                                 <telerik:RadNumericTextBox ID="SubAccertamentoTextBox" runat="server" 
                                                                                        Skin="Office2007" Width="100%" DataType="System.Int32" MaxLength="6">
                                                                                  <NumberFormat DecimalDigits="0" AllowRounding="False" GroupSeparator=""  /> 
                                                                                 </telerik:RadNumericTextBox>
                                                                                </td>

                                                                                 <td style="padding-left:1px; padding-right:1px">
                                                                                 <telerik:RadNumericTextBox  ID="ImportoTextBox" runat="server" Skin="Office2007" 
                                                                                         Width="100%" MaxLength="10" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                              <table style="width: 100%">
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


                                                        </tr>

                                                       
                                             
                                                        <tr>
                                                             <td style="width: 130px; padding-left: 1px; padding-right: 1px">
                                                              <telerik:RadNumericTextBox ID="StanziamentoInizialeTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                    MaxLength="10" />
                                                            </td>
                                                             <td style="width: 130px; padding-left: 1px; padding-right: 1px">
                                                              <telerik:RadNumericTextBox ID="StanziamentoAssestatoTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                    MaxLength="10" />
                                                            </td>

                                                              <td style="width: 150px; padding-left: 1px; padding-right: 1px">
                                                              <telerik:RadNumericTextBox ID="DisponibilitaResidualeTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                    MaxLength="10" />
                                                            </td>

                                                              <td style=" padding-left: 1px; padding-right: 1px">
                                                             <telerik:RadTextBox ID="NumeroAttoAssunzioneTextBox" runat="server" Skin="Office2007" Width="100%"
                                                               MaxLength="50" />
                                                            </td>

                                                        </tr>
                                                    </table>
                                                                         
                                                                        <table style=" width:100%">
                                                                        <tr>
                                                                       <td style="padding-left:1px; padding-right:1px">
                                                                          <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" 
                                                                               Text="Descrizione *" />
                                                                       </td>
                                                                        </tr>
                                                                         <tr>
                                                                       <td style="padding-left:1px; padding-right:1px">
                                                                        <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" 
                                                                               Width="100%" Rows="4" TextMode="MultiLine" MaxLength="4000" />
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
                                                        <td align="center"  style="background-color: #BFDBFF;padding: 4px; border-bottom:0px solid  #9ABBE8;border-top:1px solid  #9ABBE8; height:25px">
                                           <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="100px" Skin="Office2007"
                                                            ToolTip="Salva l'accertamento">
                                                            <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                                        </telerik:RadButton>

                                                     &nbsp;
                                           <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="100px" Skin="Office2007"
                                                            ToolTip="Cancella i dati immessi">
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
