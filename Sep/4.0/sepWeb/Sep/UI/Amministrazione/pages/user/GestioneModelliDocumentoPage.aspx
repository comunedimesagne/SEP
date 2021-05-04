<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneModelliDocumentoPage.aspx.vb" Inherits="GestioneModelliDocumentoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">

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


         

            <table style="width: 900px; border: 1px solid #5D8CC9">
                <tr>
                    <td>

                        <%--INIZIO TOOLBAR--%>

                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <telerik:RadToolBar ID="RadToolBar" runat="server" Skin="Office2007" Width="100%">
                                        <Items>
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo" CommandName="Nuovo" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova" CommandName="Trova" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla" CommandName="Annulla" Owner="RadToolBar" />
                                           <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva" CommandName="Salva" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina" CommandName="Elimina" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa" CommandName="Stampa" Owner="RadToolBar" />
                                           <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                          <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"  CommandName="Home" Owner="RadToolBar" />
                                      </Items>
                                    </telerik:RadToolBar>
                                </td>
                            </tr>
                        </table>

                        <%--FINE TOOLBAR--%>



                 
  <div  id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">

                        <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                            <tr>
                                <td>
                                    <asp:Label ID="NomeLabel" runat="server" CssClass="Etichetta" Text="Nome *" ForeColor="#FF8040" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="NomeTextBox" runat="server" Skin="Office2007" Width="500px"
                                        ToolTip="Nome del modello" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ValidoLabel" runat="server" CssClass="Etichetta" Text="Valido" />
                                </td>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 30px">
                                                <asp:Label ID="DataInizioValiditaLabel" runat="server" CssClass="Etichetta" Text="Da*" />
                                            </td>
                                            <td style="width: 90px">
                                                <telerik:RadDatePicker ID="DataInizioValiditaTextBox" Skin="Office2007" Width="110px"
                                                    runat="server" MinDate="1753-01-01" ToolTip="Data inizio validità">
                                                    <Calendar>
                                                        <SpecialDays>
                                                            <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                        </SpecialDays>
                                                    </Calendar>
                                                    <DatePopupButton ToolTip="Apri il calendario." />
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td style="width: 30px">
                                                <asp:Label ID="DataFineValiditaLabel" runat="server" CssClass="Etichetta" Text="A" />
                                            </td>
                                            <td style="width: 390px">
                                                <telerik:RadDatePicker ID="DataFineValiditaTextBox" Skin="Office2007" Width="110px"
                                                    runat="server" MinDate="1753-01-01" ToolTip="Data fine validità">
                                                    <Calendar>
                                                        <SpecialDays>
                                                            <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                        </SpecialDays>
                                                    </Calendar>
                                                    <DatePopupButton ToolTip="Apri il calendario." />
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="VisibilitaLabel" runat="server" CssClass="Etichetta" Text="Visibile a tutti" />
                                </td>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 90px">
                                                <asp:CheckBox ID="VisibilitaPubblicaCheckBox" runat="server"  Text="&nbsp"/>
                                            </td>
                                            <td style="width: 60px">
                                                <asp:Label ID="AbilitatoLabel" runat="server" CssClass="Etichetta" Text="Abilitato" />
                                            </td>
                                            <td style="width: 390px">
                                                <asp:CheckBox ID="AbilitatoCheckBox" runat="server"   Text="&nbsp"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        </div>
                      
                        <%--INIZIO GRIDVIEW--%>


  <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">

                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                           <asp:Label CssClass="Etichetta" ID="TitoloElencoModelliDocumentoLabel" runat="server" Font-Bold="True" Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Modelli Documento" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="PannelloContenitoreGriglia" runat="server" style="overflow: auto; background-color: #FFFFFF;  border: 0px solid #5D8CC9;">

                                                <telerik:RadGrid ID="ModelliDocumentoGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007"  AllowSorting="True"
                                                    Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>

                                                            <telerik:GridButtonColumn Text="Visualizza modello di documento" HeaderStyle-Width="20px"
                                                                ItemStyle-Width="20px" ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                ImageUrl="~\images\knob-search16.png" UniqueName="Preview" />

                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                            <telerik:GridTemplateColumn SortExpression="Nome" UniqueName="Nome" HeaderText="Nome"
                                                                DataField="Nome" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Nome")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                        width: 200px;">
                                                                        <%# Eval("Nome")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn SortExpression="Creatore" UniqueName="Creatore" HeaderText="Utente"
                                                                DataField="Creatore" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Creatore")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 200px;">
                                                                        <%# Eval("Creatore")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn SortExpression="DataInizioValidita" UniqueName="DataInizioValidita"
                                                                HeaderText="Valido dal" DataField="DataInizioValidita" HeaderStyle-Width="70px"
                                                                ItemStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("DataInizioValidita","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                        <%# Eval("DataInizioValidita", "{0:dd/MM/yyyy}")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn SortExpression="DataFineValidita" UniqueName="DataFineValidita"
                                                                HeaderText="Valido al" DataField="DataFineValidita" HeaderStyle-Width="70px"
                                                                ItemStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("DataFineValidita","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                        <%# Eval("DataFineValidita", "{0:dd/MM/yyyy}")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridButtonColumn Text="Seleziona modello di documento" HeaderStyle-Width="20px"
                                                                ItemStyle-Width="20px" ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                                ImageUrl="~\images\checks.png" UniqueName="Select" />

                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                       

                         <div style="display:none">
                              <asp:Button runat="server" ID= "DisabilitaPulsantePredefinito" style=" width:0px; height:0px; display:none" />
                        </div>
                   


                        <%--FINE GRIDVIEW--%>
                    </td>
                </tr>
            </table>

            </div>

            <asp:HiddenField ID="infoOperazioneHidden" runat="server" />

             <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
            <asp:HiddenField ID="documentContentHidden" runat="server" />
          <asp:ImageButton ID="salvaContenutoButton" runat="server" ImageUrl="~/images//knob-search16.png"  style="display: none; width:0px" />

        </ContentTemplate>
       
    </asp:UpdatePanel>




</asp:Content>
