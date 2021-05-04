<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GenerazioneImprontaPage.aspx.vb" Inherits="GenerazioneImprontaPage" %>

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

                var h = document.getElementById("lyrCorpoPagina").offsetHeight;
                var w = document.getElementById("lyrCorpoPagina").offsetWidth;


                _backgroundElement.style.width = w + 'px';
                _backgroundElement.style.height = h + 'px';

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


    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="200">
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
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <div id="pageContent">
                <center>
                    <table width="600px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text=" Genera Impronta" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ContainerMargin">
                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td style=" width:230px">
                                                       <asp:Label ID="NomeFileDocumentoLabel" runat="server" CssClass="Etichetta" Text="Nome File" />
                                                    </td>
                                                    <td>
                                                    <telerik:RadAsyncUpload ID="AllegatoUpload" runat="server" MaxFileInputsCount="1"
                                                                      Skin="Office2007" Width="250px" InputSize="60">
                                                                      <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                  </telerik:RadAsyncUpload>
                                                    </td>
                                                </tr>
                                                <tr>
                                                 <td style=" width:230px">
                                                       <asp:Label ID="TipologiaHashLabel" runat="server" CssClass="Etichetta" Text="Tipo Hash" />
                                                    </td>
                                                    <td>

                                                     <telerik:RadComboBox ID="TipologiaHashComboBox" runat="server" Skin="Office2007" Width="300px"
                                                       EmptyMessage="- Seleziona Tipologia Hash -" ItemsPerRequest="10" Filter="StartsWith"
                                                       MaxHeight="300px" />
                                                    
                                                    </td>
                                                </tr>
                                                <tr>
                                                   <td style=" width:230px">
                                                        <asp:Label ID="ImprontaLabel" runat="server" Text="Impronta"  CssClass="Etichetta" />
                                                    </td>
                                                    <td>
                                                     <telerik:RadTextBox ID="ImprontaTextBox" runat="server" Skin="Office2007" Width="450px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            <telerik:RadButton ID="SalvaButton" runat="server" Text="Genera" Width="90px" Skin="Office2007" 
                                                ToolTip="Genera Impronta">
                                                <Icon PrimaryIconUrl="../../../../images/Process.png" PrimaryIconLeft="4px" PrimaryIconTop="2px" PrimaryIconHeight="19px" PrimaryIconWidth="19px"  />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
