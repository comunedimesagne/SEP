<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VisualizzaAttoAmministrativoPage.aspx.vb"
    Inherits="VisualizzaAttoAmministrativoPage"  MasterPageFile="~/PopupPage.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">

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

               var clientBounds = this.GetClientBounds();
               var clientWidth = clientBounds.width;
               var clientHeight = clientBounds.height;
               _backgroundElement.style.width = Math.max(Math.max(document.documentElement.scrollWidth, document.body.scrollWidth), clientWidth) + 'px';
               _backgroundElement.style.height = Math.max(Math.max(document.documentElement.scrollHeight, document.body.scrollHeight), clientHeight) + 'px';
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

       function GetClientBounds() {
           var clientWidth;
           var clientHeight;
           switch (Sys.Browser.agent) {
               case Sys.Browser.InternetExplorer:
                   clientWidth = document.documentElement.clientWidth;
                   clientHeight = document.documentElement.clientHeight;
                   break;
               case Sys.Browser.Safari:
                   clientWidth = window.innerWidth;
                   clientHeight = window.innerHeight;
                   break;
               case Sys.Browser.Opera:
                   clientWidth = Math.min(window.innerWidth, document.body.clientWidth);
                   clientHeight = Math.min(window.innerHeight, document.body.clientHeight);
                   break;
               default:  // Sys.Browser.Firefox, etc.
                   clientWidth = Math.min(window.innerWidth, document.documentElement.clientWidth);
                   clientHeight = Math.min(window.innerHeight, document.documentElement.clientHeight);
                   break;
           }
           return new Sys.UI.Bounds(0, 0, clientWidth, clientHeight);
       }



    

         
    </script>

    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="200">
        <ProgressTemplate>
            <%--<div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px">
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
            </div>--%>
        </ProgressTemplate>
    </asp:UpdateProgress>


       <asp:UpdatePanel ID="Pannello" runat="server">
       <ContentTemplate>

         <div id="pageContent">


  
   

                                        <%--INIZIO CONTENUTO--%>

                                            <table style="width: 100%;">

                                                <tr style="height: 32px">
                                                    <td>
                                                        <asp:Label ID="lbldocNumeroDocumento" runat="server" CssClass="Etichetta" ForeColor="#FF6600"
                                                            Text="N° Pratica" Width="90px" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="rtxtdocNumeroDocPratica" runat="server" Skin="Office2007"
                                                            TabIndex="1" Width="80px" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="label10" runat="server" CssClass="Etichetta" Font-Bold="True" Text="/" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="rtxtdocAnnoDocPratica" runat="server" Skin="Office2007" TabIndex="2"
                                                            Width="40px" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldocDataDocumento" runat="server" CssClass="Etichetta"
                                                            Style="text-align: center" Text="Del" Width="40px" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="rdtpdocDataAperturaDocPratica" runat="server" MinDate="1900-01-01"
                                                            Skin="Office2007" TabIndex="3" Width="100px">
                                                            <Calendar Skin="Office2007" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                                ViewSelectorText="x">
                                                            </Calendar>
                                                            <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" TabIndex="3">
                                                            </DateInput>
                                                            <DatePopupButton HoverImageUrl="" ImageUrl="" TabIndex="3" />
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldocNumeroProtocollo" runat="server" CssClass="Etichetta" Text="N° Protocollo"
                                                            Width="100px" Style="text-align: center" />
                                                    </td>
                                                    <td style="height: 24px">
                                                        <telerik:RadTextBox ID="rtxtdocNumeroProtocollo" runat="server" Enabled="False" Skin="Office2007"
                                                            Width="80px" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldocDataProtocollo" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                            Text="Del" Width="35px" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="rdtpdocDataProtocollo" runat="server" Culture="it-IT"
                                                            EnableTyping="False" MinDate="1900-01-01" Skin="Office2007" Width="75px">
                                                            <Calendar Skin="Office2007" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                                ViewSelectorText="x">
                                                            </Calendar>
                                                            <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" ReadOnly="True"
                                                                Width="75px">
                                                            </DateInput>
                                                            <DatePopupButton HoverImageUrl="" ImageUrl="" Visible="False" />
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldocAmmissibile" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                            Style="text-align: center" Text="Ammissibilità *" Width="110px" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="rcbodocAmmissibile" runat="server" AutoPostBack="True" EmptyMessage="- Selezionare -"
                                                            Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                                            TabIndex="4" Width="120px">
                                                            <Items>
                                                                <telerik:RadComboBoxItem runat="server" Text="- Seleziona -" Value="-1" />
                                                                <telerik:RadComboBoxItem runat="server" BackColor="#00C000" ForeColor="White" Text="Ammissibile"
                                                                    Value="1" />
                                                                <telerik:RadComboBoxItem runat="server" BackColor="Red" ForeColor="White" Text="Non Ammissibile"
                                                                    Value="0" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>


                                            </table>

                                            
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <telerik:RadTabStrip runat="server" ID="rtbsFormulario" SelectedIndex="0" MultiPageID="DatiPraticaMultiPage"
                                                            Skin="Office2007" Width="100%">
                                                            <Tabs>
                                                                <telerik:RadTab Text="Dati delle Parti" Selected="True" />
                                                                <telerik:RadTab Text="Contratto" />
                                                                <telerik:RadTab runat="server" Text="Improcedibilità" />
                                                                <telerik:RadTab runat="server" Text="Documenti" />
                                                                <telerik:RadTab runat="server" Text="Sedute di Conciliazione" />
                                                            </Tabs>
                                                        </telerik:RadTabStrip>
                                                        <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace -->
                                                        <telerik:RadMultiPage runat="server" ID="DatiPraticaMultiPage" SelectedIndex="0"
                                                            Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">

                                                            <telerik:RadPageView runat="server" ID="DatiGeneraliPageView" CssClass="corporatePageView"
                                                                Height="425px">

                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbllblTestoA" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                ForeColor="#666666" Text="Istanza presentata da :" Width="180px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblutzCognomeRagSoc" runat="server" CssClass="Etichetta" ForeColor="#FF6600"
                                                                                Text="Intestatario Utenza *" Width="160px"></asp:Label>
                                                                            <telerik:RadTextBox ID="rtxtutzCognomeRagSoc" runat="server" Skin="Office2007" Style="text-transform: uppercase;"
                                                                                TabIndex="5" Width="350px">
                                                                            </telerik:RadTextBox>&nbsp;<asp:ImageButton ID="imbTrovaUtenza" runat="server" ImageUrl="~/images//knob-search16.png" />&nbsp;<asp:ImageButton
                                                                                ID="imbSvuotaUtenza" runat="server" ImageUrl="~/images/RecycleEmpty.png" OnClientClick="SvuotaUtenza()" />&nbsp;<asp:ImageButton
                                                                                    ID="imbAggiornaDatiUtenza" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                    Style="display: none" /><telerik:RadNumericTextBox ID="rtxndocIdUtenza" runat="server"
                                                                                        Skin="Office2007" TabIndex="2" Width="0px">
                                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                    </telerik:RadNumericTextBox>&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%" border="0" cellpadding="2" cellspacing="0" class="ContainerWrapper">
                                                                                <tr class="ContainerHeader">
                                                                                    <td>
                                                                                        <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Font-Bold="True" Style="color: #00156E;"
                                                                                            Text="Recapito principale" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="ContainerMargin" valign="top" style="height: 46px">
                                                                                        <table border="0" cellpadding="0" cellspacing="4" class="Container" style="width: 100%">
                                                                                            <tr style="background-color: #DFE8F6">
                                                                                                <td valign="top">
                                                                                                    <table bgcolor="White" style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 67px">
                                                                                                                <asp:Label ID="lblutzIndirizzo" runat="server" CssClass="Etichetta" Text="Indirizzo"
                                                                                                                    Width="75px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzIndirizzo" runat="server" Skin="Office2007" TabIndex="6"
                                                                                                                    Width="280px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblutzComune" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                                                    Text="Comune" Width="70px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzComune" runat="server" Height="23px" Skin="Office2007"
                                                                                                                    TabIndex="7" Width="213px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="imbTrovaComune" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                    ImageAlign="AbsMiddle" TabIndex="13" Visible="False" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="imbSvuotaComune" runat="server" ImageUrl="~/images/RecycleEmpty.png"
                                                                                                                    ImageAlign="AbsMiddle" OnClientClick="SvuotaComuni()" TabIndex="14" Visible="False" />&nbsp;
                                                                                                                <asp:ImageButton ID="imbAggiornaComune1Utenza" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                    Style="display: none" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblutzProvincia" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                                                    Text="Prov." Width="45px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzProvincia" runat="server" Skin="Office2007" TabIndex="8"
                                                                                                                    Width="30px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblutzCAP" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                                                    Text="CAP" Width="30px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzCAP" runat="server" Skin="Office2007" TabIndex="9"
                                                                                                                    Width="60px" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                    <table bgcolor="White" style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 67px">
                                                                                                                <asp:Label ID="lblutzTelefono" runat="server" CssClass="Etichetta" Text="Telefono"
                                                                                                                    Width="75px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzTelefono" runat="server" Skin="Office2007" TabIndex="10"
                                                                                                                    Width="200px" />
                                                                                                            </td>

                                                                                                             <td>
                                                                                                                <asp:Label ID="lblutzFax" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                                                    Text="Fax" Width="45px" />
                                                                                                             </td>

                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzFax" runat="server" Skin="Office2007" TabIndex="11"
                                                                                                                    Width="200px" />

                                                                                                            </td>
                                                                                                            <td style="width:55px">
                                                                                                                <asp:Label ID="lblutzEmail" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                                                    Text="Email" Width="50px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzEmail" runat="server" Skin="Office2007" TabIndex="12"
                                                                                                                    Width="260px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="btnCancellaDopoSblocco" runat="server" Style="display: none;" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>

                                                                                                    <table bgcolor="White" style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 140px">
                                                                                                                <asp:Label ID="lbldocIdTipoDocumentoIdentita" runat="server" CssClass="Etichetta"
                                                                                                                    Text="Documento" Width="90px" />
                                                                                                                <asp:Label ID="Label7" runat="server" CssClass="Etichetta" Text="Tipo" Width="40px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadComboBox ID="rcbodocIdTipoDocumentoIdentita" runat="server" EmptyMessage="- Selezionare -"
                                                                                                                    Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                                                                                                    TabIndex="23" Width="150px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lbldocNumeroDocumentoIdentita" runat="server" CssClass="Etichetta"
                                                                                                                    Style="text-align: center" Text="Numero" Width="60px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtdocNumeroDocumentoIdentita" runat="server" Height="23px"
                                                                                                                    Skin="Office2007" TabIndex="24" Width="150px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lbldocRilasciataDa" runat="server" CssClass="Etichetta" Text="Rilasciato Da"
                                                                                                                    Width="80px" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtdocRilasciataDa" runat="server" Skin="Office2007" TabIndex="25"
                                                                                                                    Width="250px" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>

                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="2" cellspacing="0" class="ContainerWrapper">
                                                                                <tr class="ContainerHeader">
                                                                                    <td>
                                                                                        <asp:Label ID="FiltroLabel0" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                            Style="color: #00156E;" Text="Recapiti per le comunicazioni" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="ContainerMargin" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="4" class="Container" style="width: 100%">
                                                                                            <tr style="background-color: #DFE8F6">
                                                                                                <td valign="top">
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td colspan="10">
                                                                                                                <asp:Label ID="lbldocRappresentatoDa" runat="server" CssClass="Etichetta" Text="Rappresentante"
                                                                                                                    Width="114px"></asp:Label>
                                                                                                                <telerik:RadTextBox ID="rtxtdocRappresentatoDa" runat="server" Skin="Office2007"
                                                                                                                    Style="text-transform: uppercase;" TabIndex="13" Width="530px">
                                                                                                                </telerik:RadTextBox>
                                                                                                                <asp:CheckBox ID="chkdocDomicilioRapprPredef" runat="server" CssClass="etichetta"
                                                                                                                    Text="Domicilio Predefinito" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="width: 67px">
                                                                                                                <asp:Label ID="lblutzIndirizzoDomicilio" runat="server" CssClass="Etichetta" Text="Indirizzo"
                                                                                                                    Width="75px"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzIndirizzoDomicilio" runat="server" Skin="Office2007"
                                                                                                                    Style="text-transform: uppercase;" TabIndex="14" Width="280px">
                                                                                                                </telerik:RadTextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblutzComuneDomicilio" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                                                    Text="Comune" Width="70px"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzComuneDomicilio" runat="server" Height="23px" Skin="Office2007"
                                                                                                                    TabIndex="15" Width="213px">
                                                                                                                </telerik:RadTextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="imbTrovaComuneD" runat="server" ImageUrl="~/images//knob-search16.png" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="imbSvuotaComuneD" runat="server" ImageUrl="~/images/RecycleEmpty.png"
                                                                                                                    OnClientClick="SvuotaComuniD()" />
                                                                                                                &nbsp;<asp:ImageButton ID="imbAggiornaComune2Utenza" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                    Style="display: none" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblutzProvinciaDomicilio" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                                                    Text="Prov." Width="45px"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzProvinciaDomicilio" runat="server" Skin="Office2007"
                                                                                                                    TabIndex="16" Width="30px">
                                                                                                                </telerik:RadTextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblutzCAPDomicilio" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                                                    Text="CAP" Width="30px"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <telerik:RadTextBox ID="rtxtutzCAPDomicilio" runat="server" Skin="Office2007" TabIndex="17"
                                                                                                                    Width="60px">
                                                                                                                </telerik:RadTextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbldocTipoUtenza" runat="server" CssClass="Etichetta" Text="Tipo Utenza"
                                                                                Width="85px" Font-Bold="True" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 50%;" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:RadioButton ID="rbtnPrivata" runat="server" AutoPostBack="True" CssClass="Etichetta"
                                                                                            GroupName="TipoUtenza" TabIndex="18" Text="Privata" Width="70px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton ID="rbtnBusiness" runat="server" AutoPostBack="True" CssClass="Etichetta"
                                                                                            GroupName="TipoUtenza" TabIndex="19" Text="Business" Width="100px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lbldocNumTeleUtenzaInteressato" runat="server" CssClass="Etichetta"
                                                                                            ForeColor="#FF6600" Text="N° Utenza Tel." Width="95px"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="rtxtdocNumTeleUtenzaInteressato" runat="server" Skin="Office2007"
                                                                                            TabIndex="20" Width="150px">
                                                                                        </telerik:RadTextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lbldocNumContrattoTelefonico" runat="server" CssClass="Etichetta"
                                                                                            Text="N° Contratto" Width="95px" Style="text-align: center"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="rtxtdocNumContrattoTelefonico" runat="server" Skin="Office2007"
                                                                                            TabIndex="21" Width="160px">
                                                                                        </telerik:RadTextBox>
                                                                                    </td>
                                                                                    <td style="width: 4px">
                                                                                        <asp:Label ID="lbldocCodiceCliente" runat="server" CssClass="Etichetta" Text="Cod. Cliente"
                                                                                            Width="90px" Style="text-align: center"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="rtxtdocCodiceCliente" runat="server" MaxLength="11" Skin="Office2007"
                                                                                            TabIndex="22" Width="120px">
                                                                                        </telerik:RadTextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 26px;">
                                                                            <asp:Label ID="lbllblTestoA1" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                                ForeColor="#666666" Text="Nei confronti dell'operatore :" Width="200px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblgteDenominazioneGestore" runat="server" CssClass="Etichetta" ForeColor="#FF6600"
                                                                                Text="Denominazione *" Width="160px"></asp:Label>
                                                                            <telerik:RadComboBox AutoPostBack="true" ID="DenominazioneGestoreComboBox" runat="server"
                                                                                EmptyMessage="- Seleziona Gestore -" Filter="StartsWith" ItemsPerRequest="10"
                                                                                MaxHeight="400px" Skin="Office2007" TabIndex="26" Width="380px" />
                                                                            &nbsp;<asp:ImageButton ID="imbTrovaGestoreTelefonico" runat="server" ImageUrl="~/images//knob-search16.png" />&nbsp;<asp:ImageButton
                                                                                ID="imbSvuotaGestoreTelefonico" runat="server" ImageUrl="~/images/RecycleEmpty.png"
                                                                                OnClientClick="SvuotaGestore()" />&nbsp;<asp:ImageButton ID="imbAggiornaGestore"
                                                                                    runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" /><telerik:RadNumericTextBox
                                                                                        ID="rtxndocIdGestore" runat="server" Skin="Office2007" TabIndex="2" Width="0px">
                                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                    </telerik:RadNumericTextBox>&nbsp;<asp:CheckBox ID="chkdocParsteIstante" runat="server"
                                                                                        CssClass="etichetta" Text="Parte Istante" TabIndex="27" />
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 67px">
                                                                            <asp:Label ID="lblgteIndirizzoGestore" runat="server" CssClass="Etichetta" Text="Indirizzo"
                                                                                Width="75px" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="rtxtgteIndirizzoGestore" runat="server" Skin="Office2007"
                                                                                Style="text-transform: uppercase;" TabIndex="28" Width="280px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblgteComuneGestore" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                Text="Comune" Width="70px" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="rtxtgteComuneGestore" runat="server" Height="23px" Skin="Office2007"
                                                                                TabIndex="29" Width="213px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imbTrovaComuneGestore" runat="server" ImageUrl="~/images//knob-search16.png" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imbSvuotaComuneGestore" runat="server" ImageUrl="~/images/RecycleEmpty.png"
                                                                                OnClientClick="SvuotaComuniGest()" />&nbsp;<asp:ImageButton ID="imbAggiornaComuneGestore"
                                                                                    runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblgteProvinciaGestore" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                Text="Prov." Width="45px" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="rtxtgteProvinciaGestore" runat="server" Skin="Office2007"
                                                                                TabIndex="30" Width="30px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblgteCAPGestore" runat="server" CssClass="Etichetta" Style="text-align: center"
                                                                                Text="CAP" Width="30px" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="rtxtgteCAPGestore" runat="server" Skin="Office2007" TabIndex="31"
                                                                                Width="60px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </telerik:RadPageView>


                                                            <telerik:RadPageView runat="server" ID="MotiviPageView" CssClass="productsPageView" Height="425px">

                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="2" cellspacing="0" class="ContainerWrapper" style="width: 100%">
                                                                                <tr class="ContainerHeader">
                                                                                    <td>
                                                                                        <asp:Label ID="Label6" runat="server" CssClass="Etichetta" Font-Bold="False" Style="color: #00156E;"
                                                                                            Text="Oggetto del Contratto" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="ContainerMargin" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="4" class="Container" style="width:100%">
                                                                                            <tr style="background-color: #DFE8F6">
                                                                                                <td valign="top">
                                                                                                    <telerik:RadListBox ID="rlsbTipologieServizi" runat="server" CheckBoxes="True" Height="150px"
                                                                                                        Skin="Office2007" Sort="Ascending" SortCaseSensitive="False" Width="100%" TabIndex="32" />
                                                                                                  
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <table border="0" cellpadding="2" cellspacing="0" class="ContainerWrapper" style="width: 100%">
                                                                                <tr class="ContainerHeader">
                                                                                    <td>
                                                                                        <asp:Label ID="Label5" runat="server" CssClass="Etichetta" Font-Bold="False" Style="color: #00156E;"
                                                                                            Text="Oggetto della Controversia" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="ContainerMargin" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="4" class="Container" style="width:100%">
                                                                                            <tr style="background-color: #DFE8F6">
                                                                                                <td valign="top">
                                                                                                    <telerik:RadListBox ID="rlsbOggettoControversia" runat="server" CheckBoxes="True"
                                                                                                        Height="150px" Skin="Office2007" Sort="Ascending" SortCaseSensitive="False" Width="100%"
                                                                                                        TabIndex="33" />
                                                                                                  
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table cellpadding="2" cellspacing="0" border="0" class="ContainerWrapper" style="width:100%">
                                                                                <tr class="ContainerHeader">
                                                                                    <td style="width: 882px">
                                                                                        <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Font-Bold="False" Style="color: #00156E;"
                                                                                            Text="Descrizione dei Fatti" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="ContainerMargin" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="4" class="Container" style="width:100%">
                                                                                            <tr style="background-color: #DFE8F6">
                                                                                                <td valign="top">
                                                                                                    <telerik:RadTextBox ID="rtxtdocDescrizione" runat="server" Height="30px" Skin="Office2007"
                                                                                                        TabIndex="34" TextMode="MultiLine" Width="100%" />
                                                                                                
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                  

                                                            <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="2" cellspacing="0" class="ContainerWrapper">
                                                                                <tr class="ContainerHeader">
                                                                                    <td>
                                                                                        <asp:Label ID="Label4" runat="server" CssClass="Etichetta" Font-Bold="False" Style="color: #00156E;"
                                                                                            Text="Precedenti tentativi di composizione della controversia" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="ContainerMargin" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="4" class="Container"  style="width: 100%">
                                                                                            <tr style="background-color: #DFE8F6">
                                                                                                <td  valign="top">
                                                                                                    <telerik:RadTextBox ID="rtxtdocNoteIstConcContr" runat="server" Height="30px" Skin="Office2007"
                                                                                                        TabIndex="35" TextMode="MultiLine" Width="100%" />
                                                                                                 
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="2" cellspacing="0" class="ContainerWrapper">
                                                                                <tr class="ContainerHeader">
                                                                                    <td>
                                                                                        <asp:Label ID="Label3" runat="server" CssClass="Etichetta" Font-Bold="False" Style="color: #00156E;"
                                                                                            Text="Richieste" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="ContainerMargin" valign="top">
                                                                                        <table border="0" cellpadding="0" cellspacing="4" class="Container" style="width: 100%">
                                                                                            <tr style="background-color: #DFE8F6">
                                                                                                <td valign="top">
                                                                                                    <telerik:RadTextBox ID="rtxtdocRichieste" runat="server" Height="30px" Skin="Office2007"
                                                                                                        TabIndex="36" TextMode="MultiLine" Width="100%" />
                                                                                                  
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </telerik:RadPageView>


                                                            <telerik:RadPageView ID="InammissibilitaPageView" runat="server" Height="425px">
                                                                <table border="0" cellpadding="2" cellspacing="0" class="ContainerWrapper">
                                                                    <tr class="ContainerHeader">
                                                                        <td>
                                                                            <asp:Label ID="Label11" runat="server" CssClass="Etichetta" Font-Bold="False" Style="color: #00156E;"
                                                                                Text="Motivazioni Improcedibilità" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="ContainerMargin" valign="top">
                                                                            <table border="0" cellpadding="0" cellspacing="4" class="Container">
                                                                                <tr style="background-color: #DFE8F6">
                                                                                    <td valign="top">
                                                                                        <table style="width: 100%;">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <telerik:RadListBox ID="rlsbMotivazioniImprocedibilita" runat="server" CheckBoxes="True"
                                                                                                        Height="300px" Skin="Office2007" Sort="Ascending" SortCaseSensitive="False" Width="860px"
                                                                                                        TabIndex="37">
                                                                                                    </telerik:RadListBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <telerik:RadButton ID="rbtnLetteraInammissibilità" runat="server" Skin="Office2007"
                                                                                                        Text="Lettera Inammissibilità" TabIndex="38">
                                                                                                        <Icon PrimaryIconUrl="~/images/mail16.png" PrimaryIconWidth="16px" PrimaryIconHeight="16px"
                                                                                                            PrimaryIconTop="2px" PrimaryIconLeft="4px" />
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
                                                            </telerik:RadPageView>

                                                            <telerik:RadPageView runat="server" ID="DocumentiPageView" CssClass="corporatePageView" Height="425px">
                                                          
                                                                 <div style=" padding:2px 2px 2px 2px;width:100%" >

                                       <table style="width: 100%;background-color: #DFE8F6">


                                        <tr style="display: none">
                                            <td style="width: 90px">
                                                <asp:Label ID="NumeroDocumentiLabel" runat="server" CssClass="Etichetta" Text="N. documenti" />
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="NumeroDocumentiTextBox" runat="server" Skin="Office2007"
                                                    Width="50px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 90px">
                                                <asp:Label ID="DescrizioneDocumentoLabel" runat="server" CssClass="Etichetta" Text="Descrizione" />
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="DescrizioneDocumentoTextBox" runat="server" Skin="Office2007"
                                                    Width="250px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 90px">
                                                <asp:Label ID="TipologiaDocumentoLabel" runat="server" CssClass="Etichetta" Text="Tipologia" />
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbodocIdTipologia" runat="server" EmptyMessage="- Selezionare -"
                                                    Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                                    TabIndex="41" Width="250px" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"  />
                                            </td>
                                        </tr>
                                        <tr style="height:30px;  vertical-align:middle">
                                            <td style="width: 90px">
                                                <asp:Label ID="NomeFileDocumentoLabel" runat="server" CssClass="Etichetta" Text="Nome file" />
                                            </td>
                                            <td>
                                                <telerik:RadAsyncUpload ID="AllegatoUpload" runat="server" MaxFileInputsCount="1"
                                                    Skin="Office2007" Width="250px" InputSize="40" EnableViewState="True">
                                                    <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                </telerik:RadAsyncUpload>
                                            </td>
                                        </tr>
                                    </table>

                                    <table style="width: 100%;background-color: #DFE8F6">
                                        <tr>
                                            <td style="width: 140px">
                                                <asp:Label ID="OpzioniScannerLabel" runat="server" CssClass="Etichetta" Text="Opzioni scanner" />
                                            </td>
                                            <td>
                                                <asp:Label ID="FronteRetroLabel" runat="server" CssClass="Etichetta" Text="Fronte retro" />&nbsp;<asp:CheckBox
                                                    ID="FronteRetroCheckBox" runat="server" Text="" Checked="true" />&nbsp;&nbsp;<asp:Label
                                                        ID="VisualizzaUILabel" runat="server" CssClass="Etichetta" Text="Mostra interfaccia" />&nbsp;<asp:CheckBox
                                                            ID="VisualizzaUICheckBox" runat="server" Text="" />
                                            </td>
                                        </tr>
                                    </table>

                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                        <tr>
                                            <td style="height: 20px">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Documenti" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="ScansionaImageButton" runat="server" ImageUrl="~/images//scanner.png"
                                                                ToolTip="Allega documento digitalizzato" TabIndex="44" BorderStyle="None" ImageAlign="AbsMiddle" />&nbsp;<asp:ImageButton
                                                                    ID="AggiungiDocumentoImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                    ToolTip="Allega documento" TabIndex="43" ImageAlign="AbsMiddle" BorderStyle="None" /><asp:ImageButton
                                                                        ID="ScanUploadButton" Style="display: none" runat="server" ImageUrl="~/images//RecycleEmpty.png" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #FFFFFF">
                                            <td>
                                                <div style="overflow: auto; height: 283px; border: 1px solid #5D8CC9">
                                                    <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco documenti associati alla Pratica"
                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                        Width="99.8%" Culture="it-IT">
                                                        <MasterTableView DataKeyNames="allIdAllegato, allIdDocumento,Nomefile,allOggetto">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="allIdAllegato" DataType="System.Int32" FilterControlAltText="Filter allIdAllegato column"
                                                                    HeaderText="Id" ReadOnly="True" SortExpression="allIdAllegato" UniqueName="allIdAllegato"
                                                                    Visible="False" />
                                                                <telerik:GridBoundColumn DataField="allIdDocumento" FilterControlAltText="Filter allIdDocumento column"
                                                                    HeaderText="allIdDocumento" SortExpression="allIdDocumento" UniqueName="allIdDocumento"
                                                                    Visible="False" />
                                                                <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="N." ItemStyle-Width="10px"
                                                                    HeaderStyle-Width="10px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="NumeratoreLabel" runat="server" Width="10px" /></ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                    DataField="NomeFile" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 300px;">
                                                                         <%# Eval("NomeFile")%>
                                                                         </div>

                                                                
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="ImprontaEsadecimale" UniqueName="ImprontaEsadecimale"
                                                                    HeaderText="Impronta" DataField="ImprontaEsadecimale" HeaderStyle-Width="260px"
                                                                    ItemStyle-Width="260px" Visible="False">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("ImprontaEsadecimale")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 260px;">
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn SortExpression="allOggetto" UniqueName="allOggetto" HeaderText="Oggetto"
                                                                    DataField="allOggetto" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("allOggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 350px;">
                                                                             <%# Eval("allOggetto")%>
                                                                        </div>
                                                                    </ItemTemplate>

                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn SortExpression="DescrizioneTipologiaDocumento" UniqueName="DescrizioneTipologiaDocumento"
                                                                    HeaderText="Tipo" DataField="DescrizioneTipologiaDocumento" HeaderStyle-Width="70px"
                                                                    ItemStyle-Width="70px" Visible="False">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("DescrizioneTipologiaDocumento")%>' style="white-space: nowrap;
                                                                            overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn SortExpression="PadProtocollo" UniqueName="PadProtocollo"
                                                                    HeaderText="Protocollo" DataField="PadProtocollo" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                    <ItemTemplate>
                                                                        <div title='<%# Eval("PadProtocollo")%>' style="white-space: nowrap; overflow: hidden;
                                                                            text-overflow: ellipsis; width: 70px;">
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                    ItemStyle-Width="10px" HeaderStyle-Width="10px" ImageUrl="~\images\knob-search16.png"
                                                                    UniqueName="Preview" />
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                    ItemStyle-Width="10px" HeaderStyle-Width="10px" ImageUrl="~\images\Delete16.png"
                                                                    UniqueName="Delete" />
                                                            </Columns>
                                                        </MasterTableView></telerik:RadGrid></div>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>

                                        <asp:HiddenField ID="infoScansioneHidden" runat="server" />
                                    <asp:HiddenField ID="documentContentHidden" runat="server" />

                                                            </telerik:RadPageView>

                                                             <telerik:RadPageView runat="server" ID="SeduteConciliazionePageView" CssClass="corporatePageView" Height="425px">
                                  <div style="padding: 20px 2px 2px 2px; width: 100%">
                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="SeduteLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Elenco Sedute di Conciliazione" />
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div style="overflow: auto; height: 370px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="rgrdElencoConvocazioni" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            CellSpacing="0" Culture="it-IT" GridLines="None" PageSize="5" Skin="Office2007"
                                                            TabIndex="21" Width="99.8%">
                                                            <MasterTableView DataKeyNames="Id,rptCodicePratica">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                    <telerik:GridBoundColumn DataField="rptCodicePratica" DataType="System.Int32" FilterControlAltText="Filter rptCodicePratica column"
                                                                        HeaderText="rptCodicePratica" SortExpression="rptCodicePratica" UniqueName="rptCodicePratica"
                                                                        Visible="False" />
                                                                    <telerik:GridBoundColumn DataField="rptIdTavolo" DataType="System.Int32" FilterControlAltText="Filter rptIdTavolo column"
                                                                        HeaderText="rptIdTavolo" SortExpression="rptIdTavolo" UniqueName="rptIdTavolo"
                                                                        Visible="False" />
                                                                    <telerik:GridBoundColumn DataField="rptOraPrevista" DataFormatString="{0:dd/MM/yyyy}"
                                                                        DataType="System.DateTime" FilterControlAltText="rptOraPrevista" HeaderText="rptOraPrevista"
                                                                        SortExpression="rptOraPrevista" UniqueName="rptOraPrevista" Visible="False" />
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="250px" ItemStyle-Width="250px" DataField="DataIncontroTesto"
                                                                        FilterControlAltText="Filter DataIncontroTesto column" HeaderText="Data Incontro"
                                                                        SortExpression="DataIncontroTesto" UniqueName="DataIncontroTesto" />
                                                                    <telerik:GridBoundColumn HeaderStyle-Width="150px" ItemStyle-Width="150px" DataField="StatoConvocazione"
                                                                        FilterControlAltText="Filter StatoConvocazione column" HeaderText="Stato Convocazione"
                                                                        SortExpression="StatoConvocazione" UniqueName="StatoConvocazione" />
                                                                    <telerik:GridBoundColumn DataField="Risultato" FilterControlAltText="Filter Risultato column"
                                                                        HeaderText="Risultato" SortExpression="Risultato" UniqueName="Risultato" />
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                                            </telerik:RadPageView>


                                                        </telerik:RadMultiPage>
                                                        <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                                                    </td>
                                                </tr>
                                            </table>

                                         <%--FINE CONTENUTO--%>

                                     <table cellpadding="0" cellspacing="0" style="width: 100%;">
                              
                                    <tr class="GridFooter">
                                          <td align="center">
                                            <telerik:RadButton ID="SalvaAllegatiButton" runat="server" Text="Salva Allegati" Width="120px" Skin="Office2007">
                                                <Icon PrimaryIconUrl="../../../../images/attach16.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                      &nbsp; &nbsp;
                                            <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="100px" Skin="Office2007">
                                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                    </td>
                                    </tr>

                            </table>

     </div>

             </ContentTemplate>
 </asp:UpdatePanel>

 
 </asp:Content>