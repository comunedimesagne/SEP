<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GenerazioneCertificatiPage.aspx.vb" Inherits="GenerazioneCertificatiPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');

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
            var count = 2;
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
                _backgroundElement.style.backgroundColor = '#09718F';
                _backgroundElement.style.filter = "alpha(opacity=20)";
                _backgroundElement.style.opacity = "0.2";
            }
            else {
                _backgroundElement.style.display = 'none';

            }
        }


        function ShowMessageBox(message) {


            this.document.body.appendChild(messageBox);
            this.document.body.appendChild(messageBoxPanel);

            with (messageBoxPanel) {
                style.display = '';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.width = '100%';
                style.height = '100%';
                style.zIndex = 10000;
                style.backgroundColor = '#09718F';
                style.filter = "alpha(opacity=20)";
                style.opacity = "0.2";
            }

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
                messageBoxPanel.style.display = 'none';
            }
            catch (e) { }
        }


        function updating(sender, args) {
            var d = $get("progressBar");
            d.style.display = '';
            if (args.get_progressData() && args.get_progressData().OperationComplete == 'true') {
                args.set_cancel(true);
                d.style.display = 'none';
            }
        }

      
    </script>


    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
       
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px">
                <table cellpadding="4" style="background-color: #4892FF">
                    <tr>
                        <td>
                            <div id="loadingContainer" style="width: 300px; text-align: center; background-color: #BFDBFF;
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

         <div id="progressBar" style="position: absolute; width: 100%; text-align: center; top: 300px;z-index:2000000">
                <div id="innerProgressBar" style="width: 310px; text-align: center; background-color: #BFDBFF;margin: 0 auto">
                    <telerik:RadProgressArea OnClientProgressUpdating="updating" Skin="Office2007" ID="RadProgressArea1"
                        runat="server" ProgressIndicators="TotalProgress,RequestSize,TotalProgressPercent,TotalProgressBar"
                        HeaderText="Elaborazione in corso ...">
                        <Localization Total="Totale:" Uploaded="Completato:" />
                    </telerik:RadProgressArea>                    
                </div>
            </div>

        </ProgressTemplate>

        



    </asp:UpdateProgress>




    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <center>

                   <telerik:RadProgressManager ID="Radprogressmanager1" runat="server" Skin="Office2007" />

                    <table width="700px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                  
                                   <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text=" Genera Certificati" />
                                        </td>
                                    </tr>

                                     <%-- CONTENT--%>
                                    <tr>
                                        <td class="ContainerMargin">

                                          
                                               <div id="GrigliaUtentiPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td style="height: 20px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="UtentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                Style="width: 600px; color: #00156E; background-color: #BFDBFF" Text="Utenti" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="background-color: #FFFFFF">
                                                            <td>
                                                                <div id="scrollPanelUtenti" runat="server" style="overflow: auto; height: 300px; border: 1px solid #5D8CC9">
                                                                    <telerik:RadGrid ID="UtentiGridView" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                                                        GridLines="None" Skin="Office2007" Width="700px" AllowSorting="True" ToolTip="Elenco Utenti">
                                                                        <MasterTableView DataKeyNames="Id">
                                                                         
                                                                            <Columns>

                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                                    <HeaderStyle Width="70px" />
                                                                                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                                                                                </telerik:GridBoundColumn>

                                                                                <telerik:GridBoundColumn DataField="Nominativo" FilterControlAltText="Filter Nominativo column"
                                                                                    HeaderText="Nominativo" SortExpression="Nominativo" UniqueName="Nominativo">
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </telerik:GridBoundColumn>

                                                                                <telerik:GridBoundColumn DataField="Username" FilterControlAltText="Filter Username column"
                                                                                    HeaderText="Username" SortExpression="Username" UniqueName="Username">
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </telerik:GridBoundColumn>

                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="StatoElaborazione"
                                                                                    ImageUrl="~\images\knob-search16.png" UniqueName="StatoElaborazione" HeaderTooltip="Stato Elaborazione"
                                                                                    HeaderStyle-Width="20px" ItemStyle-Width="20px" />

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
                                            <telerik:RadButton ID="SalvaButton" runat="server" Text="Genera" Width="120px" Skin="Office2007" 
                                                ToolTip="Genera Certificati">
                                                <Icon PrimaryIconUrl="../../../../images/Process.png" PrimaryIconLeft="4px" PrimaryIconTop="2px" PrimaryIconHeight="19px" PrimaryIconWidth="19px"  />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>

                   <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
