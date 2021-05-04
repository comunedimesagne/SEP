<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="AggiornamentoSoftwarePage.aspx.vb" Inherits="AggiornamentoSoftwarePage" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>


 
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">


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


            this.document.body.appendChild(messageBoxPanel);
            this.document.body.appendChild(messageBox);

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
                style.width = '305px';
                style.height = '40px';
                style.backgroundColor = '#BFDBFF';
                style.border = 'solid #4892FF 2px';
                style.position = 'absolute';
                style.left = '0px';
                style.top = '0px';
                style.zIndex = 10000;
                style.textAlign = 'center';
                style.verticalAlign = 'middle';
                innerHTML = message;
                style.color = '#00156E';
                style.backgroundImage = 'url(/sep/Images/success.png)';
                style.backgroundPosition = '5px center';
                style.backgroundRepeat = 'no-repeat';

                style.lineHeight = '40px';
            }


            xc = Math.round((document.body.clientWidth / 2) - (300 / 2)) + 120;
            yc = Math.round((document.body.clientHeight / 2) - (40 / 2)) - 300 ;


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

           <asp:ImageButton ID="NotificaAggiornamentoButton" runat="server" ImageUrl="~/images//knob-search16.png"
                Style="display: none" />

            <div id="pageContent">
                <center>

                 



                      <table width="500px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td>
                        <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E; width: 300px"
                                                    Font-Bold="True" Text="Gestione Aggiornamento Software" />
                                            </td>
                                            <td style="text-align: right; width:25px">
                                                 <asp:Table ID="componentPlaceHolder" runat="server" CellPadding="0" CellSpacing="0"
                                                title="Componenti installati" Style="width: 100%">
                                                <asp:TableRow>
                                                </asp:TableRow>
                                            </asp:Table>
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
                                                <div  style="overflow: auto; height: 100px; width: 100%; background-color: #FFFFFF;
                                                    border: 0px solid #5D8CC9;">
                                                  
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                 <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="SalvaButton" runat="server" Text="Aggiorna" Width="120px" Skin="Office2007"
                                                ToolTip="Avvia Aggiornamento">
                                                <Icon PrimaryIconUrl="../../../../images/upload16.jpg" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
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

</asp:Content>

