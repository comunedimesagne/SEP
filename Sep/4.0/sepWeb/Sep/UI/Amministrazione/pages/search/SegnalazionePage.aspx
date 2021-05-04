<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SegnalazionePage.aspx.vb"
    Inherits="SegnalazionePage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">


<head id="Head1" runat="server">
    <title>Segnalazione</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />

    
     <style type="text/css">
                .RadUpload {
                    text-align: left !important;
                }
            </style>

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


       function OnClientFileAllegatoSegnalatoreSelected() {
           var divAggIncompatibilita = document.getElementById('AggiungiAllegatoSegnalatorePanel');
           divAggIncompatibilita.style.visibility = 'visible'
       }

       function OnClientFileUploadAllegatoSegnalatoreRemoved() {
           var divAggIncompatibilita = document.getElementById('AggiungiAllegatoSegnalatorePanel');
           divAggIncompatibilita.style.visibility = 'hidden'
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
                                 <table style="width: 100%">
                                     <tr>
                                         <td>
                                             &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                 Text="Dettaglio Segnalazione" />
                                         </td>
                                       
                                         <td style=" width:400px; text-align:right">
                                             <asp:Label ID="StatoTextBox" runat="server" CssClass="Etichetta" Text="" Style="color: #00156E"
                                                 Font-Bold="True" />
                                         </td>
                                     </tr>
                                 </table>
                            </td>
                           
                              
                            </tr>

                              <%-- BODY--%>

                            <tr>
                                <td class="ContainerMargin">

                                  <%--DETTAGLIO SEGNALAZIONE--%>
                                    <table style="width: 100%; border: 0px solid #5D8CC9; height: 100%; margin-left:3px">
                                        <tr style="height: 22px">
                                            <td style="width: 95px">
                                                <asp:Label ID="NumeroSequenzaLabel" runat="server" CssClass="Etichetta" Text="N. Sequenza" />
                                            </td>
                                            <td style="width: 110px">
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                    <asp:Label ID="NumeroSequenzaTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="85px" Text="&nbsp;" />
                                                </span>
                                            </td>


                                            <%-- <td style="width: 70px; text-align: center">
                                                <asp:Label ID="ContestoLabel" runat="server" CssClass="Etichetta" Text="Contesto" />
                                            </td>
                                            <td>
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100px; height: 21px">
                                                    <asp:Label ID="ContestoTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="185px" Text="&nbsp;" />
                                                </span>
                                            </td>--%>

                                            <td style="width: 110px; text-align: center">
                                                <asp:Label ID="DataCreazioneLabel" runat="server" CssClass="Etichetta" Text="Data Creazione" />
                                            </td>
                                             <td style="width:105px">
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100px; height: 21px">
                                                    <asp:Label ID="DataCreazioneTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="100px" Text="&nbsp;" />
                                                </span>
                                            </td>

                                            

                                             <td style="width: 110px; text-align: center">
                                                <asp:Label ID="DataScadenzaLabel" runat="server" CssClass="Etichetta" Text="Data Scadenza" />
                                            </td>
                                            <td>
                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100px; height: 21px">
                                                    <asp:Label ID="DataScadenzaTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                        runat="server" Width="100px" Text="&nbsp;" />
                                                </span>
                                            </td>



                                        </tr>
                                    </table>


                                    <div id="GrigliaDocumentiPanel" runat="server" style="padding: 2px 0px 0px 0px;">

                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">


                                                        <tr>
                                                            <td>
                                                                <table  cellpadding="0" cellspacing="0" width="100%" border="0"
                                                                    style="background-color: #BFDBFF">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTabStrip ID="SegnalazioneTabStrip" runat="server" MultiPageID="SegnalazioneMultiPage"
                                                                                SelectedIndex="0" Skin="Office2007" Width="100%">
                                                                                <Tabs>
                                                                                    <telerik:RadTab Selected="True" Text="Risposte Questionario" Style="text-align: center" />
                                                                                    <telerik:RadTab Text="Messaggi" Style="text-align: center;"  />
                                                                                     <telerik:RadTab Text="Commenti" Style="text-align: center" />
                                                                                     <telerik:RadTab Text="Allegati Ricevente" Style="text-align: center" />
                                                                                     <telerik:RadTab Text="Allegati Segnalante" Style="text-align: center" />
                                                                                </Tabs>
                                                                            </telerik:RadTabStrip>

                                                                            <telerik:RadMultiPage ID="SegnalazioneMultiPage" runat="server" BorderColor="#3399FF"
                                                                                CssClass="multiPage" Height="100%" SelectedIndex="0" Width="100%">

                                                                                <telerik:RadPageView ID="RisposteQuestionarioPageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="420px" Width="100%">

                                                                                    

                                                                                    <div id="RisposteQuestionarioPanel" runat="server" style="padding: 2px 2px 2px 2px;overflow: auto; height: 410px;">

                                                                                     
                                                                                    </div>
                                                                                </telerik:RadPageView>


                                                                                <telerik:RadPageView ID="MessaggiPageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="420px" Width="100%">

                                                                                    <div id="MessaggiPanel" runat="server" style="padding: 2px 2px 2px 2px;">

                                                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">

                                                                                            <tr>
                                                                                                <td style="height: 20px">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="MessaggiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Messaggi" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr style="background-color: #FFFFFF">
                                                                                                <td>
                                                                                                    <div style="overflow: auto; height: 325px; border: 1px solid #5D8CC9">
                                                                                                        <telerik:RadGrid ID="MessaggiGridView" runat="server" ToolTip="Elenco Messaggi"
                                                                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                            Width="99.8%" Culture="it-IT">
                                                                                                            <MasterTableView DataKeyNames="id" TableLayout="Fixed">
                                                                                                                <Columns>

                                                                                                                    <telerik:GridTemplateColumn SortExpression="author" UniqueName="author" HeaderText="Autore"
                                                                                                                        DataField="author" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("author")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("author")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                    <telerik:GridTemplateColumn SortExpression="content" UniqueName="content" HeaderText="Contenuto"
                                                                                                                        DataField="content" >
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("content")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("content")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                    <telerik:GridTemplateColumn SortExpression="creation_date" UniqueName="creation_date" HeaderText="Data Creazione"
                                                                                                                        DataField="creation_date" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("creation_date")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("creation_date")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                

                                                                                                                </Columns>
                                                                                                            </MasterTableView></telerik:RadGrid>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr>
                                                                                                <td style="height: 55px">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width:80px">
                                                                                                                <asp:Label ID="MessaggioLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                    Style="color: #00156E; background-color: #BFDBFF" Text="Messaggio" />
                                                                                                            </td>

                                                                                                              <td>
                                                                                                               <telerik:RadTextBox ID="MessaggioTextBox" runat="server" MaxLength="1500" Rows="3"
                                                                                                                Skin="Office2007" TextMode="MultiLine" Width="100%" ToolTip="Scrivi Messaggio" />
                                                                                                            </td>

                                                                                                            <td style="width: 90px">
                                                                                                                <telerik:RadButton ID="InviaMessaggioButton" runat="server" Text="Invia" Width="90px"  Skin="Office2007" ToolTip="Invia Messaggio">
                                                                                                                </telerik:RadButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>


                                                                                        </table>

                                                                                   
                                                                                    </div>
                                                                                </telerik:RadPageView>


                                                                                  <telerik:RadPageView ID="CommentiPageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="420px" Width="100%">

                                                                                    <div id="CommentiPanel" runat="server" style="padding: 2px 2px 2px 2px;">

                                                                                      <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">

                                                                                            <tr>
                                                                                                <td style="height: 20px">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="CommentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Commenti" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr style="background-color: #FFFFFF">
                                                                                                <td>
                                                                                                    <div style="overflow: auto; height: 325px; border: 1px solid #5D8CC9">
                                                                                                        <telerik:RadGrid ID="CommentiGridView" runat="server" ToolTip="Elenco Commenti"
                                                                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                            Width="99.8%" Culture="it-IT">
                                                                                                            <MasterTableView DataKeyNames="id" TableLayout="Fixed">
                                                                                                                <Columns>

                                                                                                                    

                                                                                                                    <telerik:GridTemplateColumn SortExpression="content" UniqueName="content" HeaderText="Contenuto"
                                                                                                                        DataField="content">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("content")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red;">
                                                                                                                                <div style="width: 15px; height: 15px; border: 1px solid <%# If(Eval("type") = "receiver","#D6E9C6","#BCE8F1") %>;
                                                                                                                                    float: left; background-color: <%# If(Eval("type") = "receiver","#DFF0D8","#D9EDF7") %>;
                                                                                                                                    margin-right: 10px">
                                                                                                                                </div>
                                                                                                                                <%# Eval("content")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                     <telerik:GridTemplateColumn SortExpression="author" UniqueName="author" HeaderText="Autore"
                                                                                                                        DataField="author" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# If(Eval("type") = "receiver","Ricevente","Segnalante") %>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# If(Eval("type") = "receiver", "Ricevente", "Segnalante")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                    <telerik:GridTemplateColumn SortExpression="creation_date" UniqueName="creation_date" HeaderText="Data Creazione"
                                                                                                                        DataField="creation_date" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("creation_date")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("creation_date")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                

                                                                                                                </Columns>
                                                                                                            </MasterTableView></telerik:RadGrid>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>


                                                                                            <tr>
                                                                                                <td style="height: 55px">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width:80px">
                                                                                                                <asp:Label ID="CommentoLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                    Style="color: #00156E; background-color: #BFDBFF" Text="Commento" />
                                                                                                            </td>

                                                                                                              <td>
                                                                                                               <telerik:RadTextBox ID="CommentoTextBox" runat="server" MaxLength="1500" Rows="3"
                                                                                                                Skin="Office2007" TextMode="MultiLine" Width="100%" ToolTip="Scrivi Commento" />
                                                                                                            </td>

                                                                                                            <td style="width: 90px">
                                                                                                                <telerik:RadButton ID="InviaCommentoButton" runat="server" Text="Invia" Width="90px"  Skin="Office2007" ToolTip="Invia Commento">
                                                                                                                </telerik:RadButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>

                                                                                        </table>


                                                                                   
                                                                                    </div>
                                                                                </telerik:RadPageView>

                                                                                 <telerik:RadPageView ID="AllegatiRiceventePageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="420px" Width="100%">
                                                                                    <div id="AllegatiRiceventePanel" runat="server" style="padding: 2px 2px 2px 2px;">


                                                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">

                                                                                            <tr>
                                                                                                <td style="height: 20px">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="AllegatiRiceventeLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati Ricevente" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr style="background-color: #FFFFFF">
                                                                                                <td>
                                                                                                    <div style="overflow: auto; height: 325px; border: 1px solid #5D8CC9">
                                                                                                        <telerik:RadGrid ID="AllegatiRiceventeGridView" runat="server" ToolTip="Elenco Allegati Ricevente"
                                                                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                            Width="99.8%" Culture="it-IT">
                                                                                                            <MasterTableView DataKeyNames="id, name" TableLayout="Fixed">
                                                                                                                <Columns>

                                                                                                                    <telerik:GridTemplateColumn SortExpression="name" UniqueName="name" HeaderText="Nome del file"
                                                                                                                        DataField="name">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("name")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("name")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                     <telerik:GridTemplateColumn SortExpression="creation_date" UniqueName="creation_date" HeaderText="Data Caricamento"
                                                                                                                        DataField="creation_date" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("creation_date")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("creation_date")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>


                                                                                                                    <telerik:GridTemplateColumn SortExpression="content_type" UniqueName="content_type" HeaderText="Tipo"
                                                                                                                        DataField="content_type" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("content_type")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("content_type")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>


                                                                                                                      <telerik:GridTemplateColumn SortExpression="size" UniqueName="size" HeaderText="Dimensione (KB)"
                                                                                                                        DataField="size" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%#  String.Format("{0:N2}", Eval("size") / 1024)  %>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# String.Format("{0:N2}", Eval("size") / 1024)%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>


                                                                                                                     <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column" Text="Visualizza Allegato"
                                                                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="30px" ItemStyle-Width="30px"  />
                                                                                                                    
                                                                                                                   
                                                                                                                   
                                                                                                                

                                                                                                                </Columns>
                                                                                                            </MasterTableView></telerik:RadGrid>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>


                                                                                            <tr>
                                                                                                <td style="height: 55px">
                                                                                                  
                                                                                                </td>
                                                                                            </tr>

                                                                                        </table>


                                                                                   
                                                                                    </div>
                                                                                </telerik:RadPageView>

                                                                                 <telerik:RadPageView ID="AllegatiSegnalatorePageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="420px" Width="100%">
                                                                                    <div id="AllegatiSegnalatorePanel" runat="server" style="padding: 2px 2px 2px 2px;">



                                                                                     <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">

                                                                                            <tr>
                                                                                                <td style="height: 20px">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="AllegatiSegnalatoreLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati Segnalante" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr style="background-color: #FFFFFF">
                                                                                                <td>
                                                                                                    <div style="overflow: auto; height: 305px; border: 1px solid #5D8CC9">
                                                                                                        <telerik:RadGrid ID="AllegatiSegnalatoreGridView" runat="server" ToolTip="Elenco Allegati Segnalante"
                                                                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                                                            Width="99.8%" Culture="it-IT">
                                                                                                            <MasterTableView DataKeyNames="id, name" TableLayout="Fixed">
                                                                                                                <Columns>

                                                                                                                    <telerik:GridTemplateColumn SortExpression="name" UniqueName="name" HeaderText="Nome del file"
                                                                                                                        DataField="name">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("name")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("name")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                     <telerik:GridTemplateColumn SortExpression="creation_date" UniqueName="creation_date" HeaderText="Data Caricamento"
                                                                                                                        DataField="creation_date" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("creation_date")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("creation_date")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>


                                                                                                                    <telerik:GridTemplateColumn SortExpression="content_type" UniqueName="content_type" HeaderText="Tipo"
                                                                                                                        DataField="content_type" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%# Eval("content_type")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# Eval("content_type")%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>


                                                                                                                      <telerik:GridTemplateColumn SortExpression="size" UniqueName="size" HeaderText="Dimensione (KB)"
                                                                                                                        DataField="size" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                                                                        <ItemTemplate>
                                                                                                                            <div title='<%#  String.Format("{0:N2}", Eval("size") / 1024)  %>' style="white-space: nowrap; overflow: hidden;
                                                                                                                                text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                                                <%# String.Format("{0:N2}", Eval("size") / 1024)%></div>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                     <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column" Text="Visualizza Allegato"
                                                                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="30px" ItemStyle-Width="30px"  />

                                                                                                                     <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column" Text="Cancella Allegato"
                                                                                                                        ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="30px" ItemStyle-Width="30px" />

                                                                                                                    
                                                                                                                   
                                                                                                                   
                                                                                                                

                                                                                                                </Columns>
                                                                                                            </MasterTableView></telerik:RadGrid>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>


                                                                                            <tr>
                                                                                                <td style="height: 55px">
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div id="UploadAllegatoSegnalatorePanel" runat="server" visible="true">
                                                                                                                    <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <table style="width: 100%">
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 80px;">
                                                                                                                                            <asp:Label ID="DescrizioneAllegatoSegnalatoreLabel" runat="server" CssClass="Etichetta"
                                                                                                                                                Text="Descrizione" />
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <telerik:RadTextBox ID="DescrizioneAllegatoSegnalatoreTextBox" runat="server" Skin="Office2007"
                                                                                                                                                Width="100%" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr style="height: 30px">
                                                                                                                            <td>
                                                                                                                                <table style="width: 100%">
                                                                                                                                    <tr>
                                                                                                                                    <td style="width: 80px;">
                                                                                                                                            <asp:Label ID="NomefileLabel" runat="server" CssClass="Etichetta"
                                                                                                                                                Text="Nomefile" />
                                                                                                                                        </td>

                                                                                                                                        <td style="padding-top: 6px; width:400px;">
                                                                                                                                            <telerik:RadAsyncUpload ID="AllegatoSegnalatoreUpload" runat="server" MaxFileInputsCount="1"
                                                                                                                                                OnClientFileSelected="OnClientFileAllegatoSegnalatoreSelected" OnClientFileUploadRemoved="OnClientFileUploadAllegatoSegnalatoreRemoved"
                                                                                                                                                Skin="Office2007" Width="100%" InputSize="32" EnableViewState="True">
                                                                                                                                                <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                                                                                            </telerik:RadAsyncUpload>
                                                                                                                                        </td>
                                                                                                                                        <td style="padding-top: 3px; width: 30px; text-align: center">
                                                                                                                                            <div id="AggiungiAllegatoSegnalatorePanel" style="visibility: hidden;">
                                                                                                                                                <asp:ImageButton ID="AggiungiAllegatoSegnalatoreImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                                                                                                    ToolTip="Aggiungi file" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                        &nbsp;
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
                                                                                     </table>





                                                                                   
                                                                                    </div>
                                                                                </telerik:RadPageView>


                                                                            </telerik:RadMultiPage>
                                                                        </td>
                                                                    </tr>
                                                        </tr>

                                                  


                                                    </table>
                                                </div>

                                                  

                             

                       


                                   




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


           <%-- <div style="display: none">
                <asp:Button runat="server" ID="notificaOperazioneButton" Style="width: 0px; height: 0px;
                    left: -1000px; position: absolute" />
            </div>--%>
         
           <asp:HiddenField ID="infoOperazioneHidden" runat="server" />

        </ContentTemplate>
 </asp:UpdatePanel>


    </form>
</body>
</html>
