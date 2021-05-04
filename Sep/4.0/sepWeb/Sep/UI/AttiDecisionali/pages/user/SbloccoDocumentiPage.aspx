<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="SbloccoDocumentiPage.aspx.vb" Inherits="SbloccoDocumentiPage" %>
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
            var sbloccaButton = $find('<%= SbloccaButton.ClientId %>');
            var items = sender.get_items();
            var chk = $get('<%= SelectAllCheckBox.ClientId %>');
            chk.checked = (sender.get_checkedItems().length == items.get_count());
            var checked = (sender.get_checkedItems().length > 0);
            sbloccaButton.set_enabled(checked);
         }


         //***************************************************************************************************************************
         //SELEZIONA-DESELEZIONA TUTTE LE CHECKBOX DEL CONTROLLO RADLISTBOX QUANDO VIENE SELEZIONATA LA CHECKBOX 'SELEZIONA TUTTO'.
         //***************************************************************************************************************************
         function OnCheckBoxClick(checkBox) {
             var listbox = $find('<%= AttiListBox.ClientId %>');
             var sbloccaButton = $find('<%= SbloccaButton.ClientId %>');
             var items = listbox.get_items();
             var checked = checkBox.checked;
             items.forEach(function (itm) { itm.set_checked(checked); });
             sbloccaButton.set_enabled(checked);
          }



          function OnClientLoadHandler(sender) {
              var coll = sender.get_items();
              var count = coll.get_count();
              var i;
              for (i = 0; i < count; i++) {
                  var item = coll.getItem(i);
                  var actualText = item.get_text();
                  var span = item.get_textElement();
                  span.innerHTML = actualText;
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
                                                            background-color: #BFDBFF" Text="Sblocco Documenti" />
                                                    </td>
                                                   <%-- <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="FiltraImageButton" runat="server" ImageUrl="~/images//search.png"
                                                            ToolTip="Applica i filtri impostati" Style="border: 0" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="center" style="width: 30px">
                                                        <asp:ImageButton ID="AnnullaFiltroImageButton" Style="border: 0" runat="server" ImageUrl="~/images//cancelSearch.png"
                                                            ToolTip="Annulla i filtri impostati" ImageAlign="AbsMiddle" />
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>



                                   <%-- CONTENT--%>
                                    <tr>
                                        <td class="ContainerMargin">


                                          <%--  <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 100%; width: 100%; background-color: #DFE8F6;
                                                            border: 0px solid #5D8CC9;">


                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>--%>

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
                                                                                        color: #00156E; background-color: #BFDBFF" Text="Elenco Atti" />
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
                                                                                                SelectionMode="Multiple" onclientitemchecked="OnItemChecked" OnClientLoad="OnClientLoadHandler" >
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
                                           
                                            <telerik:RadButton ID="SbloccaButton" runat="server" Text="Sblocca" Width="100px"
                                                Skin="Office2007" ToolTip="Effettua lo sblocco degli atti selezionati">
                                                <Icon PrimaryIconUrl="../../../../images/save16.png" PrimaryIconLeft="5px" />
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

