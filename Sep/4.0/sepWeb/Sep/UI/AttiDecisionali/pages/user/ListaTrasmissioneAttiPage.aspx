<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="ListaTrasmissioneAttiPage.aspx.vb" Inherits="ListaTrasmissioneAttiPage" %>
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



        //***************************************************************************************************************************
        //SELEZIONA LA CHECKBOX 'SELEZIONA TUTTO' QUANDO LE CHECKBOX DEL CONTROLLO RADLISTBOX SONO TUTTE SELEZIONATE.
        //***************************************************************************************************************************
        function OnItemChecked(sender, e) {
            var stampaButton = $find('<%= StampaButton.ClientId %>');
            var items = sender.get_items();
            var chk = $get('<%= SelectAllCheckBox.ClientId %>');
            chk.checked = (sender.get_checkedItems().length == items.get_count());
            var checked = (sender.get_checkedItems().length > 0);
            stampaButton.set_enabled(checked);
         }


         //***************************************************************************************************************************
         //SELEZIONA-DESELEZIONA TUTTE LE CHECKBOX DEL CONTROLLO RADLISTBOX QUANDO VIENE SELEZIONATA LA CHECKBOX 'SELEZIONA TUTTO'.
         //***************************************************************************************************************************
         function OnCheckBoxClick(checkBox) {
             var listbox = $find('<%= AttiListBox.ClientId %>');
             var stampaButton = $find('<%= StampaButton.ClientId %>');
             var items = listbox.get_items();
             var checked = checkBox.checked;
             items.forEach(function (itm) { itm.set_checked(checked); });
             stampaButton.set_enabled(checked);

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

             <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server" DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator> 
             <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server" DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator> 
               <telerik:RadFormDecorator ID="RadFormDecorator3" DecoratedControls="all" runat="server" DecorationZoneID="ZoneID3" Skin="Web20"></telerik:RadFormDecorator> 

                <center>

                 
            
                   

                       <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                              <%--  HEADER--%>
                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 2px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="PannelloFiltroLabel" runat="server" Font-Bold="True" Style="color: #00156E;
                                                            background-color: #BFDBFF" Text="Filtro Proposte Determina" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Applica i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>



                                   <%-- CONTENT--%>
                                    <tr>
                                        <td class="ContainerMargin">


                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 100%; width: 100%; background-color: #DFE8F6;
                                                            border: 0px solid #5D8CC9;">

                                                            <table style="width: 100%">
                                                                <tr style="height: 25px">
                                                                    <td style="width: 120px">
                                                                       <asp:Label ID="ContatoreGeneraleLabel" runat="server" CssClass="Etichetta" Text="Registro Generale" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px; width:270px">
                                                                          <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center" style="width: 40px">
                                                                                    <asp:Label ID="ContatoreGeneraleInizioLabel" runat="server" CssClass="Etichetta"
                                                                                        Text="da" />
                                                                                </td>
                                                                                <td style="width: 80px">
                                                                                    <telerik:RadNumericTextBox ID="ContatoreGeneraleInizioTextBox" runat="server" Skin="Office2007"
                                                                                        Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                        ShowSpinButtons="True" ToolTip="">
                                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                    </telerik:RadNumericTextBox>
                                                                                </td>
                                                                                <td align="center" style="width: 40px">
                                                                                    <asp:Label ID="ContatoreGeneraleFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                                </td> 
                                                                                <td>
                                                                                    <telerik:RadNumericTextBox ID="ContatoreGeneraleFineTextBox" runat="server" Skin="Office2007"
                                                                                        Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                        ShowSpinButtons="True" ToolTip="">
                                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                    </telerik:RadNumericTextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="width: 90px; text-align: center">
                                                                        <asp:Label ID="TipologiaRegistroLabel" runat="server" CssClass="Etichetta" Text="Tipo Registro" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <asp:Panel ID="TipologiaRegistroPanel" runat="server">
                                                                            <telerik:RadComboBox ID="TipologieRegistroComboBox" AutoPostBack="true" runat="server"
                                                                                EmptyMessage="- Seleziona Tipologia Registro -" Filter="StartsWith" ItemsPerRequest="10"
                                                                                MaxHeight="200px" Skin="Office2007" Width="100%" />
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <table style="width: 100%">
                                                                <tr style="height: 25px">
                                                                    <td style="width: 120px">
                                                                        <asp:Label ID="ContatoreSettoreLabel" runat="server" CssClass="Etichetta" Text="Registro Settore" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                           <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="ContatoreSettoreInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                        </td>
                                                                        <td style="width: 80px">
                                                                         

                                                                                  <telerik:RadNumericTextBox ID="ContatoreSettoreInizioTextBox" runat="server" Skin="Office2007" 
                                                                                     Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1" 
                                                                       ShowSpinButtons="True" ToolTip="" >
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                       </telerik:RadNumericTextBox>

                                                                        </td>
                                                                        <td align="center" style="width: 40px">
                                                                            <asp:Label ID="ContatoreSettoreFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                        </td>
                                                                        <td>
                                                                          

                                                                                   <telerik:RadNumericTextBox ID="ContatoreSettoreFineTextBox" runat="server" Skin="Office2007" 
                                                                                     Width="75px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1" 
                                                                       ShowSpinButtons="True" ToolTip="" >
                                                                        <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                       </telerik:RadNumericTextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                    </td>
                                                                    <td style="width: 130px">
                                                                        <asp:Label ID="DataDocumentoLabel" runat="server" CssClass="Etichetta" Text="Data Documento" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center" style="width: 40px">
                                                                                    <asp:Label ID="DataDocumentoInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                                </td>
                                                                                <td style="width: 80px">
                                                                                    <telerik:RadDatePicker ID="DataDocumentoInizioTextBox" Skin="Office2007" Width="110px"
                                                                                        runat="server" MinDate="1753-01-01" />
                                                                                </td>
                                                                                <td align="center" style="width: 40px">
                                                                                    <asp:Label ID="DataDocumentoFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadDatePicker ID="DataDocumentoFineTextBox" Skin="Office2007" Width="110px"
                                                                                        runat="server" MinDate="1753-01-01" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <table style="width: 100%">
                                                                <tr style="height: 25px">
                                                                    <td style="width: 120px">
                                                                      <asp:Label ID="SettoreLabel" runat="server" CssClass="Etichetta" Text="Settore" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px; width: 330px">
                                                                         <table style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadTextBox ID="SettoreTextBox" runat="server" Skin="Office2007" 
                                                                                        Width="100%" />
                                                                                </td>
                                                                                <td align="right" style="width: 60px">
                                                                                    <asp:ImageButton ID="TrovaSettoreImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                        ToolTip="Seleziona ufficio (ALT + S)..." />&nbsp;
                                                                                    <asp:ImageButton ID="EliminaSettoreImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                        ToolTip="Cancella settore" />&nbsp;
                                                                                    <asp:ImageButton ID="AggiornaSettoreImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                        Style="display: none" />
                                                                                    <telerik:RadTextBox ID="IdSettoreTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                        Style="display: none" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="width: 70px; text-align: center">
                                                                        <asp:Label ID="UfficioLabel" runat="server" CssClass="Etichetta" Text="Ufficio" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                        <table style="width: 100%">
                                                                    <tr>
                                                                         <td style="width: 280px">
                                                                            <telerik:RadTextBox ID="UfficioTextBox" runat="server" Skin="Office2007" 
                                                                                 Width="100%" />
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:ImageButton ID="TrovaUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona ufficio (ALT + S)..." />&nbsp;
                                                                            <asp:ImageButton ID="EliminaUfficioImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella settore" />&nbsp;
                                                                            <asp:ImageButton ID="AggiornaUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none" />
                                                                            <telerik:RadTextBox ID="IdUfficioTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                Style="display: none" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <table style="width: 100%">
                                                                <tr style="height: 25px">
                                                                    <td style="width: 120px">
                                                                          <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto" />
                                                                    </td>
                                                                    <td style="padding-left: 1px; padding-right: 1px">
                                                                      <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="100%" />
                                                                    </td>
                                                                   
                                                                  
                                                                </tr>
                                                            </table>

                                                            

                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>

                                            <%-- INIZIO TABELLA RISULTATI--%>

                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                            <%-- HEADER--%>
                                                            <tr>
                                                                <td valign="top">
                                                                    <div id="ZoneID3">
                                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;<asp:Label ID="PannelloRisultatiLabel" runat="server" Font-Bold="True" Style="width: 400px;
                                                                                        color: #00156E; background-color: #BFDBFF" 
                                                                                        Text="Elenco Proposte Determina" />
                                                                                </td>
                                                                                <td style="width: 30px">
                                                                                    <asp:CheckBox CssClass="Etichetta" ID="SelectAllCheckBox" runat="server" Text="&nbsp;"
                                                                                        AutoPostBack="false" onclick="OnCheckBoxClick(this);" />
                                                                                </td>
                                                                                <td style="width: 110px">
                                                                                    <asp:Label ID="SelezionaTuttoLabel" runat="server" Text="Seleziona Tutto" CssClass="Etichetta"
                                                                                        Font-Bold="True" Style="color: #00156E" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <%--  CONTENT--%>
                                                            <tr style="background-color: #DFE8F6">
                                                                <td valign="top">
                                                                    <div style="overflow: auto; height: 100%; width: 100%; background-color: #DFE8F6;
                                                                        border: 0px solid #5D8CC9;">
                                                                        <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <telerik:RadListBox ID="AttiListBox" runat="server" Skin="Office2007" Style="width: 867px; 
                                                                                                height: 300px" Height="300px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True"
                                                                                                SelectionMode="Multiple" onclientitemchecked="OnItemChecked" >
                                                                                            </telerik:RadListBox>
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

                                            <%-- FINE TABELLA RISULTATI--%>

                                         


                                        </td>
                                    </tr>

                                    <%--FOOTER--%>

                                    <tr>
                                       <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">

                                     <telerik:RadButton ID="StampaButton" runat="server" Text="Stampa" Width="100px"
                                                Skin="Office2007" ToolTip="Effettua la stampa delle proposte di determina selezionate">
                                                <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                            
                                          
                                        </td>
                                    </tr>


                                </table>
                            </td>
                        </tr>
                    </table>

                </center>

            </div>


            <div style="display: none">
                <asp:Button runat="server" ID="notificaOperazioneButton" Style="width: 0px; height: 0px;
                    left: -1000px; position: absolute" />
            </div>
            <asp:HiddenField ID="infoOperazioneHidden" runat="server" />


        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

