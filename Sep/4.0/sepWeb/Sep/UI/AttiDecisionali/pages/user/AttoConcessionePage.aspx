<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AttoConcessionePage.aspx.vb" Inherits="AttoConcessionePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Atto di Concessione</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 16px;
        }
    </style>
    <script type="text/javascript">

        function ProgettoSelezionato() {
            var divAggProgetto = document.getElementById('divAggProgetto');
            divAggProgetto.style.visibility = 'visible'
        }

        function ProgettoRimosso() {
            var divAggProgetto = document.getElementById('divAggProgetto');
            divAggProgetto.style.visibility = 'hidden'
        }

        function CurriculumSelezionato() {
            var divAggCurriculum = document.getElementById('divAggCurriculum');
            divAggCurriculum.style.visibility = 'visible'
        }

        function CurriculumRimosso() {
            var divAggCurriculum = document.getElementById('divAggCurriculum');
            divAggCurriculum.style.visibility = 'hidden'
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <div id="pageContent">
        <center>
            <table width="600px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    &nbsp;<asp:Label ID="TitleLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                        Text="Atto di Concessione" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ContainerMargin">
                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                    border: 0px solid #5D8CC9;">
                                                 
                                                     <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td>
                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="GeneraleLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                color: #00156E; background-color: #BFDBFF" Text="Dati Beneficiario" />
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
                                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                                <asp:Label ID="BeneficiarioLabel" runat="server" CssClass="Etichetta" 
                                                                                    Text="Beneficiario *" />
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                                <td style="padding-left: 1px; padding-right: 1px">
                                                                                        <telerik:RadComboBox ID="RubricaComboBox" runat="server" Width="470px" Height="150" EmptyMessage="Seleziona Destinatario" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                                                                            <WebServiceSettings Method="GetElementiRubrica" Path="AttoAmministrativoPage.aspx"  />
                                                                                        </telerik:RadComboBox>

                                                                                &nbsp;
                                                                                <asp:ImageButton ID="TrovaBeneficiarioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                    ToolTip="Seleziona Beneficiario..." ImageAlign="AbsMiddle" BorderStyle="None"
                                                                                    Style="height: 16px" /><asp:ImageButton ID="AggiornaBeneficiarioImageButton" runat="server" Style="display: none" />
                                                                            </td>
                                
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:Label ID="CodiceFiscalePartitaIvaLabel" runat="server" 
                                                                                    CssClass="Etichetta" Text="C.F - P. IVA *" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="width: 120px;">
                                                                                <telerik:RadTextBox ID="CodiceFiscalePartitaIvaTextBox" 
	                                                                                                style="text-transform:uppercase" MaxLength="16" runat="server" 
	                                                                                                Skin="Office2007" Width="120px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="CurriculumLabel" runat="server" CssClass="Etichetta" Text="Curriculum" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="curriculumUpload1" runat="server" style="width:100%;" visible="true">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <telerik:RadAsyncUpload ID="CurriculumUpload" runat="server" MaxFileInputsCount="1" OnClientFileSelected="CurriculumSelezionato"
                                                                                                    OnClientFileUploadRemoved="CurriculumRimosso" Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                                    <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                </telerik:RadAsyncUpload>
                                                                                            </td>
                                                                                            <td>
                                                                                                <div id="divAggCurriculum" runat="server" style="visibility:hidden;">
                                                                                                <asp:ImageButton ID="AggiungiCurriculumImageButton" runat="server" ImageUrl="~/images//add16.png" ToolTip="Allega Curriculum"
                                                                                                                 ImageAlign="AbsMiddle" BorderStyle="None" /></div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <div id="curriculumUpload2" runat="server" style="width:100%;" visible="false">
                                                                                    <asp:LinkButton ID="CurriculumAllegatoLinkButton" ForeColor="Red" CssClass="Etichetta" runat="server"/>
                                                                                    <asp:Label ID="NomeFileCurriculumLabel" runat="server" Visible="false" />
                                                                                    <asp:ImageButton ID="RimuoviCurriculumImageButton" runat="server" ImageUrl="~/images//Delete16.png" ToolTip="Rimuovi Curriculum"
                                                                                                         ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table>
                                                     <tr style=" height:3px">
                                                     <td>
                                                     </td>
                                                     </tr>
                                                     </table>


                                                

                                                       <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td>
                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="DatiAttoConcessioneLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                color: #00156E; background-color: #BFDBFF" Text="Dati Atto di Concessione" />
                                                                                <asp:Button runat="server" ID= "DisabilitaPulsantePredefinito" style=" width:0px; height:0px; left:-1000px; position:absolute" />
                                                                        </td>
                                                                        <td style="text-align: right">
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
                                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                                <asp:Label ID="ImportoLiquidazioneLabel" runat="server" CssClass="Etichetta" Text="Importo *" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-left: 1px; padding-right: 1px; width: 90px;">
                                                                                <telerik:RadNumericTextBox ID="ImportoAttoConcessioneTextBox" runat="server" Skin="Office2007"
                                                                                    Width="100%" MaxLength="10" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-left: 1px; padding-right: 1px">
                                                                                <asp:Label ID="NormaLabel" runat="server" CssClass="Etichetta" 
                                                                                    Text="Norma/Titolo *" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-left: 1px; padding-right: 1px; width: 500px;">
                                                                                <telerik:RadTextBox ID="NormaTextBox" runat="server" Skin="Office2007" Width="500px"
                                                                                    MaxLength="500" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-left: 1px; padding-right: 1px;">
                                                                                <asp:Label ID="ModalitaLabel" runat="server" CssClass="Etichetta" Text="Modalità *" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-left: 1px; padding-right: 1px; width: 500px;">
                                                                                <telerik:RadTextBox ID="ModalitaTextBox" runat="server" Skin="Office2007" Width="500px" MaxLength="1500" />
                                                                                <%--<telerik:RadComboBox ID="ModalitaComboBox" AutoPostBack="true" runat="server"
                                                                                    EmptyMessage="- Seleziona Modalità -" Filter="StartsWith" ItemsPerRequest="10"
                                                                                    MaxHeight="600px" Skin="Office2007" Width="90%" ToolTip="Modalità di scelta del contraente" />--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ProgettoLabel" runat="server" CssClass="Etichetta" Text="Progetto" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="progettoUpload1" runat="server" style="width:100%;" visible="true">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <telerik:RadAsyncUpload ID="ProgettoUpload" runat="server" MaxFileInputsCount="1" OnClientFileSelected="ProgettoSelezionato"
                                                                                                    OnClientFileUploadRemoved="ProgettoRimosso" Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                                                                    <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                </telerik:RadAsyncUpload>
                                                                                            </td>
                                                                                            <td>
                                                                                                <div id="divAggProgetto" runat="server" style="visibility:hidden;">
                                                                                                <asp:ImageButton ID="AggiungiProgettoImageButton" runat="server" ImageUrl="~/images//add16.png" ToolTip="Allega progetto"
                                                                                                                 ImageAlign="AbsMiddle" BorderStyle="None" /></div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <div id="progettoUpload2" runat="server" style="width:100%;" visible="false">
                                                                                    <asp:LinkButton ID="ProgettoAllegatoLinkButton" ForeColor="Red" CssClass="Etichetta" runat="server"/>
                                                                                    <asp:Label ID="NomeFileProgettoLabel" runat="server" Visible="false" />
                                                                                    <asp:ImageButton ID="RimuoviProgettoImageButton" runat="server" ImageUrl="~/images//Delete16.png" ToolTip="Rimuovi Progetto"
                                                                                                         ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                                    <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="100px" Skin="Office2007"
                                        ToolTip="Salva l'accertamento">
                                        <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                    </telerik:RadButton>
                                    &nbsp;
                                    <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="100px"
                                        Skin="Office2007" ToolTip="Cancella i dati immessi">
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
