<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="VisualizzazioneSegnalazioniPage.aspx.vb" Inherits="VisualizzazioneSegnalazioniPage" %>

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


                  <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                  
                                   <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                            Text="Dettaglio Segnalazione" />
                                        </td>
                                    </tr>

                                     <%-- CONTENT--%>
                                    <tr>
                                        <td class="ContainerMargin">

                                            <div id="TabSegnalazioniPanel" runat="server" style="padding: 2px 0px 0px 0px;">

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
                                                                                    <telerik:RadTab Text="Commenti" Style="text-align: center" />
                                                                                     <telerik:RadTab Text="Allegati Ricevente" Style="text-align: center" />
                                                                                     <telerik:RadTab Text="Allegati Segnalante" Style="text-align: center" />
                                                                                </Tabs>
                                                                            </telerik:RadTabStrip>

                                                                            <telerik:RadMultiPage ID="SegnalazioneMultiPage" runat="server" BorderColor="#3399FF"
                                                                                CssClass="multiPage" Height="100%" SelectedIndex="0" Width="100%">

                                                                                <telerik:RadPageView ID="RisposteQuestionarioPageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="320px" Width="100%">
                                                                                    <div id="RisposteQuestionarioPanel" runat="server" style="padding: 2px 2px 2px 2px;
                                                                                        overflow: auto; height: 310px;">
                                                                                    </div>
                                                                                </telerik:RadPageView>
                                                                                
                                                                                <telerik:RadPageView ID="CommentiPageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="320px" Width="100%">
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
                                                                                                    <div style="overflow: auto; height: 280px; border: 1px solid #5D8CC9">
                                                                                                        <telerik:RadGrid ID="CommentiGridView" runat="server" ToolTip="Elenco Commenti" AutoGenerateColumns="False"
                                                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" Culture="it-IT">
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
                                                                                                            </MasterTableView></telerik:RadGrid></div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </telerik:RadPageView>

                                                                                <telerik:RadPageView ID="AllegatiRiceventePageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="320px" Width="100%">
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
                                                                                                    <div style="overflow: auto; height: 280px; border: 1px solid #5D8CC9">
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

                                                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="30px" Text="Visualizza Allegato"
                                                                                                                        ItemStyle-Width="30px" />

                                                                                                                </Columns>
                                                                                                            </MasterTableView></telerik:RadGrid></div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </telerik:RadPageView>

                                                                                <telerik:RadPageView ID="AllegatiSegnalatorePageView" runat="server" CssClass="corporatePageView"
                                                                                    Height="320px" Width="100%">
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
                                                                                                    <div style="overflow: auto; height: 280px; border: 1px solid #5D8CC9">
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

                                                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="30px" Text="Visualizza Allegato"
                                                                                                                        ItemStyle-Width="30px" />

                                                                                                                </Columns>
                                                                                                            </MasterTableView></telerik:RadGrid></div>
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

                                            <div id="GrigliaSegnalazioniPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="SegnalazioniLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                        color: #00156E; background-color: #BFDBFF" Text="Segnalazioni" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="background-color: #FFFFFF">
                                                            <td>
                                                                <div id="scrollPanelUtenti" runat="server" style="overflow: auto; height: 300px; border: 1px solid #5D8CC9">
                                                                  
                                                                      <telerik:RadGrid ID="SegnalazioniGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True"
                                                                Culture="it-IT">

                                                                <MasterTableView DataKeyNames="Id, GuidSegnalazione" TableLayout="Fixed">
                                                                    <Columns>

                                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                            HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />


                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" ItemStyle-Width="120px" DataField="NumeroSeriale"
                                                                            FilterControlAltText="Filter NumeroSeriale column" HeaderText="N. Seriale" SortExpression="NumeroSeriale"
                                                                            UniqueName="NumeroSeriale">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NumeroSeriale")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                    width: 100%; border: 0px solid red">
                                                                                    <%# Eval("NumeroSeriale")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn  DataField="Stato"
                                                                            FilterControlAltText="Filter Stato column" HeaderText="Stato" SortExpression="Stato"
                                                                            UniqueName="Stato">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Stato")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                    <%# Eval("Stato")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>



                                                                       



                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" ItemStyle-Width="120px" DataField="DataCreazione"
                                                                            FilterControlAltText="Filter DataCreazione column" HeaderText="Data Creazione"
                                                                            SortExpression="DataCreazione" UniqueName="DataCreazione">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("DataCreazione","{0:dd/MM/yyyy HH.mm}")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                <%# Eval("DataCreazione", "{0:dd/MM/yyyy HH.mm}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" ItemStyle-Width="120px" DataField="DataScadenza"
                                                                            FilterControlAltText="Filter DataScadenza column" HeaderText="Data Scadenza" SortExpression="DataScadenza"
                                                                            UniqueName="DataScadenza">
                                                                            <ItemTemplate>
                                                                               <div title='<%# Eval("DataScadenza","{0:dd/MM/yyyy HH.mm}")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                                <%# Eval("DataScadenza", "{0:dd/MM/yyyy HH.mm}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="30px"
                                                                            ItemStyle-Width="30px" Text="Seleziona Segnalazione" FilterControlAltText="Filter Select column"
                                                                            ImageUrl="~\images\checks.png" UniqueName="Select" />

                                                                      

                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                               
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="ScaricaSegnalazioniButton" runat="server" Text="Scarica" Width="120px"
                                                Skin="Office2007" ToolTip="Scarica Segnalazioni">
                                                <Icon PrimaryIconUrl="../../../../images/process.png" PrimaryIconLeft="5px" PrimaryIconHeight="20px"
                                                    PrimaryIconWidth="20px" PrimaryIconTop="2px" />
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
