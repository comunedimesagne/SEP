<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VisualizzaIstanzaPraticaPage.aspx.vb"
    Inherits="VisualizzaIstanzaPraticaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">


<head id="Head1" runat="server">
    <title>Visualizza Istanza Pratica</title>
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
               // _backgroundElement.className = "modalBackground";
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


    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />


      <asp:UpdatePanel ID="Pannello" runat="server">
       <ContentTemplate>



    <div id="pageContent" style="overflow:auto; border: 0px solid red; height:100%">
        <center>

            <table width="900px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                           <%-- HEADER--%>
                            <tr>
                           
                                <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                        Text="Dettaglio Istanza Pratica" />
                                </td>
                            </tr>

                              <%-- BODY--%>

                            <tr>
                                <td class="ContainerMargin">

                                    <telerik:RadTabStrip runat="server" ID="IstanzeTabStrip" SelectedIndex="1" MultiPageID="IstanzeMultiPage"
                                        Skin="Office2007" Width="100%">
                                        <Tabs>
                                            <telerik:RadTab runat="server" Text="Dati Generali" Selected="True" />
                                            <telerik:RadTab runat="server" Text="Documenti" />
                                        </Tabs>
                                    </telerik:RadTabStrip>
                        <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                        <telerik:RadMultiPage runat="server" ID="IstanzeMultiPage" SelectedIndex="0" Height="100%"
                             CssClass="multiPage" BorderColor="#3399FF">

                            <telerik:RadPageView runat="server" ID="DatiGeneraliPageView" CssClass="corporatePageView"
                                Height="525px">

                                <div id="DatiGeneraliPanel" runat="server" style="padding: 2px 0px 0px 0px;">

                                  <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%; background-color: #BFDBFF">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;<asp:Label ID="DatiNotificaLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                            color: #00156E; background-color: #BFDBFF" Text="Dati Generali" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="overflow: auto;  width: 100%; background-color: #FFFFFF;
                                                                border: 0px solid #5D8CC9;">

                                                                <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr style="height:22px">
                                                                        <td style="width: 70px">
                                                                            <asp:Label ID="NumeroPraticaLabel" runat="server" CssClass="Etichetta" Text="N. Pratica" />
                                                                        </td>
                                                                        <td style="width: 140px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="NumeroPraticaTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="135px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td style="width: 150px; text-align:center">
                                                                            <asp:Label ID="DataPresentazioneLabel" runat="server" CssClass="Etichetta" Text="Data Presentazione" />
                                                                        </td>
                                                                         <td style="width: 70px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="DataPresentazioneTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="65px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>

                                                                         <td style="width: 130px; text-align:center">
                                                                            <asp:Label ID="TerminePrevistoLabel" runat="server" CssClass="Etichetta" Text="Termine Previsto" />
                                                                        </td>
                                                                         <td style="width: 70px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="TerminePrevistoTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="65px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>

                                                                       <td style="width: 100px; text-align:center">
                                                                            <asp:Label ID="StatoPraticaLabel" runat="server" CssClass="Etichetta" Text="Stato Pratica" />
                                                                        </td>
                                                                        <td style="width: 130px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="StatoPraticaTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="125px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td>
                                                                        &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                
                                                                <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr style="height:22px">
                                                                        <td style="width: 100px">
                                                                            <asp:Label ID="ProcedimentoLabel" runat="server" CssClass="Etichetta" Text="Procedimento" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="ProcedimentoTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="740px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        
                                                                      
                                                                    </tr>
                                                                </table>


                                                                <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr style="height:22px">
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="ResponsabileProcedimentoLabel" runat="server" CssClass="Etichetta" Text="Responsabile Proc." />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="ResponsabileProcedimentoTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="440px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td>
                                                                        &nbsp;
                                                                        </td>
                                                                      
                                                                    </tr>
                                                                    
                                                                </table>

                                                                <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr style="height: 22px">
                                                                        <td>
                                                                            <asp:Label ID="ProtocolloLabel" runat="server" CssClass="Etichetta" Text="" Font-Bold="true" />
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>

                                               <div id="RichiedentePanel" runat="server" style="padding: 2px 0px 0px 0px;">

                                                   <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%; background-color: #BFDBFF">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;<asp:Label ID="DatiRichiedenteLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                            color: #00156E; background-color: #BFDBFF" Text="Dati Richiedente" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="overflow: auto;  width: 100%; background-color: #FFFFFF;
                                                                border: 0px solid #5D8CC9;">

                                                                <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                   <tr style="height:22px">
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="DenominazioneCognomeRichiedenteLabel" runat="server" CssClass="Etichetta" Text="Denom.\Cognome" />
                                                                        </td>
                                                                        <td style="width: 320px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="DenominazioneCognomeRichiedenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="315px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td style="width: 70px; text-align:center">
                                                                            <asp:Label ID="NomeRichiedenteLabel" runat="server" CssClass="Etichetta" Text="Nome" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="NomeRichiedenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="320px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                 <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr style="height:22px">
                                                                        <td style="width: 100px">
                                                                            <asp:Label ID="CodiceFiscaleRichiedenteLabel" runat="server" CssClass="Etichetta" Text="Codice fiscale" />
                                                                        </td>
                                                                        <td style="width: 160px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="CodiceFiscaleRichiedenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="155px" ToolTip=""  Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td style="width: 90px; text-align:center">
                                                                            <asp:Label ID="IndirizzoResidenzaRichiedenteLabel" runat="server" CssClass="Etichetta" Text="Indirizzo" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="IndirizzoResidenzaRichiedenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="490px" ToolTip=""  Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                </table>



                                                                <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr style="height:22px">

                                                                    <td style="width: 90px">
                                                                            <asp:Label ID="ComuneResidenzaRichiedenteLabel" runat="server" CssClass="Etichetta" Text="Comune" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="ComuneResidenzaRichiedenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="490px" ToolTip=""  Text="&nbsp;" />
                                                                            </span>
                                                                        </td>

                                                                        <td style="width: 50px;text-align:center">
                                                                            <asp:Label ID="CapResidenzaRichiedenteLabel" runat="server" CssClass="Etichetta" Text="C.A.P." />
                                                                        </td>
                                                                        <td style="width: 60px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="CapResidenzaRichiedenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="55px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td style="width: 90px; text-align:center">
                                                                            <asp:Label ID="ProvinciaResidenzaRichiedenteLabel" runat="server" CssClass="Etichetta" Text="Provincia" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="ProvinciaResidenzaRichiedenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="40px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                 <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                  <tr style=" height:22px">
                                                                  <td style="width: 100px">
                                                                            <asp:Label ID="IndirizzoPecRichiedenteLabel" runat="server" CssClass="Etichetta" Text="Indirizzo PEC" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="IndirizzoPecRichiedenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="400px" ToolTip=""  Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                         
                                                                        <td style=" text-align:right" >
                                                                        <asp:Label ID="MessaggioLabel" runat="server" CssClass="Etichetta" Text="" Font-Bold="true" />
                                                                        </td>
                                                                 </tr>
                                                                 </table>


                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>

                                                </div>

                                                      <div id="CommittentePanel" runat="server" style="padding: 2px 0px 0px 0px;">

                                                   <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%; background-color: #BFDBFF">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" Style="width: 500px;
                                                                            color: #00156E; background-color: #BFDBFF" Text="Dati Committente" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="overflow: auto;  width: 100%; background-color: #FFFFFF;
                                                                border: 0px solid #5D8CC9;">

                                                                <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="DenominazioneCognomeCommittenteLabel" runat="server" CssClass="Etichetta" Text="Denom.\Cognome" />
                                                                        </td>
                                                                        <td style="width: 320px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="DenominazioneCognomeCommittenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="320px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td style="width: 70px; text-align:center">
                                                                            <asp:Label ID="NomeCommittenteLabel" runat="server" CssClass="Etichetta" Text="Nome" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="NomeCommittenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="320px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                 <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                            <asp:Label ID="CodiceFiscaleCommittenteLabel" runat="server" CssClass="Etichetta" Text="Codice fiscale" />
                                                                        </td>
                                                                        <td style="width: 160px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="CodiceFiscaleCommittenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="160px" ToolTip=""  Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td style="width: 90px; text-align:center">
                                                                            <asp:Label ID="IndirizzoResidenzaCommittenteLabel" runat="server" CssClass="Etichetta" Text="Indirizzo" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="IndirizzoResidenzaCommittenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="490px" ToolTip=""  Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                </table>



                                                                <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                    <tr>

                                                                    <td style="width: 90px">
                                                                            <asp:Label ID="ComuneResidenzaCommittenteLabel" runat="server" CssClass="Etichetta" Text="Comune" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="ComuneResidenzaCommittenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="490px" ToolTip=""  Text="&nbsp;" />
                                                                            </span>
                                                                        </td>

                                                                        <td style="width: 50px;text-align:center">
                                                                            <asp:Label ID="CapResidenzaCommittenteLabel" runat="server" CssClass="Etichetta" Text="C.A.P." />
                                                                        </td>
                                                                        <td style="width: 60px">
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="CapResidenzaCommittenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="60px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                        <td style="width: 90px; text-align:center">
                                                                            <asp:Label ID="ProvinciaResidenzaCommittenteLabel" runat="server" CssClass="Etichetta" Text="Provincia" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="ProvinciaResidenzaCommittenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="40px" ToolTip="" Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                 <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                 <tr>
                                                                  <td style="width: 100px">
                                                                            <asp:Label ID="IndirizzoPecCommittenteLabel" runat="server" CssClass="Etichetta" Text="Indirizzo PEC" />
                                                                        </td>
                                                                        <td>
                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                                <asp:Label ID="IndirizzoPecCommittenteTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                    runat="server" Width="400px" ToolTip=""  Text="&nbsp;" />
                                                                            </span>
                                                                        </td>
                                                                         
                                                                        <td style=" text-align:right" >
                                                                        <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="" Font-Bold="true" />
                                                                        </td>
                                                                 </tr>
                                                                 </table>


                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>

                                                </div>
                              </div>

                            </telerik:RadPageView>

                            <telerik:RadPageView runat="server" ID="DocumentiPageView" CssClass="corporatePageView"
                                Height="525px">

                                    <div id="GrigliaAllegatiPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="AllegatiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="background-color: #FFFFFF">
                                                            <td>
                                                                <div style="overflow: auto; height: 220px; border: 1px solid #5D8CC9">
                                                                    <telerik:RadGrid ID="AllegatiGridView" runat="server" ToolTip="Elenco allegati associati all'istanza"
                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                        Width="99.8%" Culture="it-IT">
                                                                        <MasterTableView DataKeyNames="IdAllegato, Nomefile">
                                                                            <Columns>

                                                                                <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                                    DataField="NomeFile" HeaderStyle-Width="810px" ItemStyle-Width="810px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 810px; border: 0px solid red">
                                                                                            <%# Eval("NomeFile")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                    Text="Visualizza Allegato..." ImageUrl="~\images\knob-search16.png" UniqueName="Preview"
                                                                                    HeaderStyle-Width="20px" ItemStyle-Width="20px" />

                                                                            </Columns>
                                                                            
                                                                        </MasterTableView>
                                                                      
                                                                    </telerik:RadGrid>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>

                                                     <div id="GrigliaDocumentiPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Documenti" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="background-color: #FFFFFF">
                                                            <td>
                                                                <div style="overflow: auto; height: 220px; border: 1px solid #5D8CC9">

                                                                    <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco documenti associati all'istanza"
                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                        Width="99.8%" Culture="it-IT">

                                                                        <MasterTableView DataKeyNames="Id">
                                                                          
                                                                            <Columns>

                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />

                                                                                <telerik:GridTemplateColumn SortExpression="DescrizioneTipoDocumento" UniqueName="DescrizioneTipoDocumento"
                                                                                    HeaderText="Tipo" DataField="DescrizioneTipoDocumento" HeaderStyle-Width="80px"
                                                                                    ItemStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("DescrizioneTipoDocumento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 80px;">
                                                                                            <%# Eval("DescrizioneTipoDocumento")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                               <%-- <telerik:GridTemplateColumn SortExpression="DescrizioneFase" UniqueName="DescrizioneFase"
                                                                                    HeaderText="Fase" DataField="DescrizioneFase" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("DescrizioneFase")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 80px;">
                                                                                            <%# Eval("DescrizioneFase")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>--%>

                                                                                    <telerik:GridTemplateColumn SortExpression="NomeDocumentoOriginale" UniqueName="NomeDocumentoOriginale"
                                                                                        HeaderText="Documento" DataField="NomeDocumentoOriginale">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("NomeDocumentoOriginale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis;">
                                                                                                <%# Eval("NomeDocumentoOriginale")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                               <%-- <telerik:GridTemplateColumn SortExpression="DataDocumento" UniqueName="DataDocumento"
                                                                                    HeaderText="Inserito il" DataField="DataDocumento" HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("DataDocumento","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                            overflow: hidden; text-overflow: ellipsis; width: 75px;">
                                                                                            <%# Eval("DataDocumento", "{0:dd/MM/yyyy}")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>--%>

                                                                               <%-- <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Stato" FilterControlAltText="Filter Stato column"
                                                                                    ImageUrl="~\images\vuoto.png" UniqueName="Stato" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                                                </telerik:GridButtonColumn>--%>

                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                    ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="20px"
                                                                                    ItemStyle-Width="20px" />
                                                                               
                                                                                
                                                                            </Columns>
                                                                          
                                                                        </MasterTableView>
                                                                           
                                                                        </telerik:RadGrid>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>

                               <%-- <div runat="server" id="PannelloFirme" style="padding: 2px 2px 2px 2px;">
                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                       <tr>
                                       <td></td>
                                       </tr>
                                    </table>
                                </div>--%>

                            </telerik:RadPageView>
                        </telerik:RadMultiPage>


                                   <%-- <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                       
                                        <tr>
                                            <td>


                                              PPPPPPPPPPPPPP

                                            
                                            </td>
                                        </tr>
                                    </table>--%>




                                </td>
                            </tr>


                              <%-- FOOTER--%>
                            <tr>
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: right">
                                                <telerik:RadButton ID="ChiudiFinestraButton" runat="server" Text="Chiudi" Width="130px"
                                                    Skin="Office2007">
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
