<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneDocumentiPage.aspx.vb" Inherits="GestioneDocumentiPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">




    <script type="text/javascript">

        var _backgroundElement = document.createElement("div");
        var messageBox = document.createElement('div');
        var messageBoxPanel = document.createElement('div');

        var count = 2;

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



        function OnFileSelected() {
            var p = document.getElementById('AddAllegatoPanel');
            p.style.visibility = 'visible'
        }

        function OnFileRemove() {
            var p = document.getElementById('AddAllegatoPanel');
            p.style.visibility = 'hidden'
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
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/new.png" Text="Nuovo"
                                                CommandName="Nuovo" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                CommandName="Trova" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                CommandName="Annulla" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                CommandName="Salva" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                CommandName="Elimina" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                CommandName="Stampa" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                            <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                CommandName="Home" Owner="RadToolBar" />
                                        </Items>
                                    </telerik:RadToolBar>
                                </td>
                            </tr>
                        </table>

                        <%--FINE TOOLBAR--%>

  
  
                       <%--INIZIO DATI--%>
                        <div id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">
                            <telerik:RadTabStrip runat="server" ID="DocumentiGenericiTabStrip" SelectedIndex="0"
                                MultiPageID="DocumentiGenericiMultiPage" Skin="Office2007" Width="100%">
                                <Tabs>
                                    <telerik:RadTab Text="Dati Documento" Selected="True" />
                                    <telerik:RadTab Text="Allega Documento" />
                                </Tabs>
                            </telerik:RadTabStrip>

                            <telerik:RadMultiPage runat="server" ID="DocumentiGenericiMultiPage" SelectedIndex="0"
                                Height="100%" Width="100%" CssClass="multiPage" BorderStyle="Solid" BorderWidth="1px"
                                BorderColor="#5D8CC9">

                                <telerik:RadPageView runat="server" ID="DatiPageView" CssClass="corporatePageView"
                                    Width="100%">

                                    <div id="DatiPanel" runat="server" style="padding: 2px 2px 2px 2px; height: 135px">
                                        <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9;  height:100%">
                                            <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="DataDocumentoLabel" runat="server" CssClass="Etichetta" Text="Data Doc. *" />
                                                </td>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 170px">
                                                                <telerik:RadDatePicker ID="DataDocumentoTextBox" Skin="Office2007" Width="110px"
                                                                    runat="server" MinDate="1753-01-01" />
                                                            </td>
                                                            <td style="width: 120px">
                                                                <asp:Label ID="GeneraDaModelloLabel" runat="server" CssClass="Etichetta" Text="Genera da modello" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="GeneraDaModelloCheckBox" runat="server" Text="&nbsp;" />
                                                            </td>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="ObbligoDestinatariLabel" runat="server" CssClass="Etichetta" Text="Obbligo destinatari" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="ObbligoDestinatariCheckBox" runat="server" Text="&nbsp;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="ModelloLabel" runat="server" CssClass="Etichetta" Text="Modello *"
                                                        ForeColor="#FF8040" />
                                                </td>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 330px">
                                                                <div style="display: none">
                                                                    <telerik:RadTextBox ID="ModelloTextBox" runat="server" Skin="Office2007" Width="350px"
                                                                        Enabled="False" />&nbsp;<asp:ImageButton ID="TrovaModelloImageButton" runat="server"
                                                                            ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona modello..." />&nbsp;<asp:ImageButton
                                                                                ID="EliminaModelloImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella modello" />&nbsp;<asp:ImageButton ID="AggiornaModelloImageButton"
                                                                                    runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none; width: 0px" /><telerik:RadTextBox
                                                                                        ID="IdModelloTextBox" runat="server" Skin="Office2007" Width="0px" Style="display: none;
                                                                                        width: 0px" /></div>
                                                                <telerik:RadComboBox ID="ModelliComboBox" runat="server" Width="320px" Height="150"
                                                                    EmptyMessage="Seleziona Modello" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007"
                                                                    LoadingMessage="Caricamento in corso...">
                                                                    <WebServiceSettings Method="GetModelliDocumento" Path="GestioneDocumentiPage.aspx" />
                                                                </telerik:RadComboBox>
                                                            </td>
                                                            <td style="width: 70px; text-align: center">
                                                                <asp:Label ID="UfficioLabel" runat="server" CssClass="Etichetta" Text="Ufficio *"
                                                                    ForeColor="#FF8040" />
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 325px">
                                                                            <telerik:RadTextBox ID="UfficioTextBox" runat="server" Skin="Office2007" Width="325px"
                                                                                Enabled="False" />
                                                                        </td>
                                                                        <td style="width: 25px; text-align: center; vertical-align: middle">
                                                                            <asp:ImageButton ID="TrovaUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona utente..." ImageAlign="AbsMiddle" />
                                                                        </td>
                                                                        <td style="width: 25px; text-align: center; vertical-align: middle">
                                                                            <asp:ImageButton ID="EliminaUfficioImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella utente" ImageAlign="AbsMiddle" /><asp:ImageButton ID="AggiornaUfficioImageButton"
                                                                                    runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none; width: 0px" /><telerik:RadTextBox
                                                                                        ID="IdUfficioTextBox" runat="server" Skin="Office2007" Width="0px" Style="display: none;
                                                                                        width: 0px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto *"
                                                        ForeColor="#FF8040" />
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" Width="790px"
                                                        Rows="3" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="AllegatoPageView" CssClass="corporatePageView"
                                    Width="100%">

                                    <div id="AllegatoPanel" runat="server" style="padding: 2px 2px 2px 2px; height: 135px">
                                    
                                        <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9;
                                            height: 100%">

                                            <tr>
                                            <td>
                                            <table style="width: 100%">

                                              <tr>
                                                <td style="width: 90px">
                                                    <asp:Label ID="DocumentoLabel" runat="server" CssClass="Etichetta"
                                                        Text="Nome File" />
                                                </td>
                                                <td>
                                                    <div id="AddDocumentoPanel" runat="server" visible="true">

                                                     <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                      <%--  <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">--%>
                                                            <tr style="height: 30px">
                                                                <td style="padding-top: 6px">
                                                                    <telerik:RadAsyncUpload ID="DocumentoUpload" runat="server"
                                                                        MaxFileInputsCount="1" OnClientFileSelected="OnFileSelected"
                                                                        OnClientFileUploadRemoved="OnFileRemove" Skin="Office2007"
                                                                        Width="100%" InputSize="34" EnableViewState="True">
                                                                        <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                                    </telerik:RadAsyncUpload>
                                                                </td>
                                                                <td style="padding-top: 6px; width: 30px; text-align: center">
                                                                    <div id="AddAllegatoPanel" style="visibility: hidden;">
                                                                        <asp:ImageButton ID="AggiungiDocumentoImageButton" runat="server"
                                                                            ImageUrl="~/images//add16.png" ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="RemoveDocumentoPanel" runat="server" visible="false">
                                                     <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                       <%-- <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #ABC1DE">--%>
                                                            <tr style="height: 34px">
                                                                <td>
                                                                    <asp:LinkButton ID="NomeFileDocumentoLinkButton" ForeColor="Blue"
                                                                        CssClass="Etichetta" runat="server" />
                                                                </td>
                                                                <td style="width: 30px; text-align: center">
                                                                    <asp:Label ID="NomeFileDocumentoLabel" runat="server"
                                                                        Visible="false" />
                                                                    <asp:ImageButton ID="RimuoviDocumentoImageButton" runat="server"
                                                                        ImageUrl="~/images//Delete16.png" ToolTip="Rimuovi Documento" ImageAlign="AbsMiddle"
                                                                        BorderStyle="None" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>

                                               

                                            </tr>
                                            </table>
                                            </td>
                                            </tr>

                                            <tr>
                                            <td style=" text-align:center">
                                             <telerik:RadButton ID="SalvaButton" runat="server" Text="Salva" Width="100px"
                                    Skin="Office2007" ToolTip="Salva Allegato">
                                    <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                                            </td>
                                            </tr>

                                          
                                        </table>

                                        

                                    </div>

                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </div>

                      

                       <div  id="UtentiPanel" runat="server" style="padding: 2px 2px 2px 2px;">

                       <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr style="height: 20px; background-color: #BFDBFF">
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style=" width:180px">
                                                                &nbsp;<asp:Label Font-Bold="True" ID="TitoloElencoDestinatariLabel" runat="server"
                                                                    Style="color: #00156E; background-color: #BFDBFF; width: 170px" CssClass="Etichetta"
                                                                    Text="Elenco Destinatari" />
                                                            </td>
                                                            <td style=" text-align:center; width:610px">
                                                             <div style="display: none">
                                                                <telerik:RadTextBox ID="FiltroDenominazioneTextBox" runat="server" Skin="Office2007"
                                                                    Width="320px" ToolTip="Digitare parola chiave (INVIO)"  />
                                                                </div>

                                                                       <telerik:RadComboBox ID="RubricaComboBox" runat="server" Width="610" Height="150" EmptyMessage="Seleziona Destinatario" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                               ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007" LoadingMessage="Caricamento in corso...">
                                <WebServiceSettings Method="GetElementiRubrica" Path="GestioneDocumentiPage.aspx"  />
                              </telerik:RadComboBox>
                                                            </td>
                                                            <td style=" width:25px; text-align:center">
                                                              <asp:ImageButton ID="AggiungiDestinatarioImageButton" runat="server" ImageUrl="~/images//ok16.png"
                                                                            ToolTip="Aggiungi destinatario selezionato" ImageAlign="AbsMiddle" />
                                                            </td>
                                                           
                                                             <td style=" width:25px; text-align:center">
                                                                <asp:ImageButton ID="TrovaDestinatarioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona destinatario..." ImageAlign="AbsMiddle" />
                                                            </td>
                                                             <td style=" width:25px; text-align:center">
                                                                <asp:ImageButton ID="EliminaDestinatariSelezionatiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    Style="width: 16px" ToolTip="Cancella destinatari selezionati" ImageAlign="AbsMiddle" />
                                                           
                                                             <asp:ImageButton ID="AggiornaDestinatarioImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none; width: 0px" />
                                                            </td>
                                                          
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="scrollPanel" runat="server" style="overflow: auto; height: 150px; border: 1px solid #5D8CC9;
                                                        background-color: White">
                                                        <telerik:RadGrid ID="DestinatariGridView" ToolTip="Elenco destinatari" runat="server"
                                                            AllowPaging="False" AutoGenerateColumns="False" CellSpacing="0" GridLines="None"
                                                            Skin="Office2007"   AllowSorting="True" AllowMultiRowSelection="True"
                                                            Culture="it-IT">
                                                            <MasterTableView DataKeyNames="Id" TableLayout="fixed">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                        AllowFiltering="False" HeaderStyle-Width="35px" ItemStyle-Width="35px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="SelectAllCheckBox" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True"
                                                                                runat="server"></asp:CheckBox>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="SelectCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                                runat="server"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False"
                                                                        HeaderStyle-Width="30px" ItemStyle-Width="30px" />

                                                                    <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                        HeaderText="Descrizione" DataField="Descrizione" >
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 100%;">
                                                                                <%# Eval("Descrizione")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="30px"
                                                                        ItemStyle-Width="30px" />
                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>

                        </div>

                       <%--FINE DATI--%>

                         
                       
                       
                        <%--INIZIO GRIDVIEW--%>


                           <div style="padding: 2px 2px 2px 2px;" id="DocumentiPanel" runat="server" >

                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label CssClass="Etichetta" ID="TitoloElencoDocumentiLabel" runat="server"
                                                            Font-Bold="True" Style="width: 800px; color: #00156E; background-color: #BFDBFF"
                                                            Text="Elenco Documenti" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 305px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="DocumentiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="True"
                                                    Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                        <telerik:GridButtonColumn Text="Visualizza documento" ButtonType="ImageButton" CommandName="Preview"
                                                            FilterControlAltText="Filter Preview column" ImageUrl="~\images\knob-search16.png"
                                                            UniqueName="Preview" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                        <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                            HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />


                                                      


                                                        <telerik:GridTemplateColumn SortExpression="NomeModello" UniqueName="NomeModello"
                                                            HeaderText="Modello" DataField="NomeModello" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("NomeModello")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 150px;">
                                                                    <%# Eval("NomeModello")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn SortExpression="DescrizioneUfficio" UniqueName="DescrizioneUfficio"
                                                            HeaderText="Ufficio" DataField="DescrizioneUfficio" HeaderStyle-Width="150px"
                                                            ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DescrizioneUfficio")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 150px;">
                                                                    <%# Eval("DescrizioneUfficio")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto" HeaderText="Oggetto"
                                                            DataField="Oggetto" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 150px;">
                                                                    <%# Eval("Oggetto")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                          <telerik:GridTemplateColumn SortExpression="NomeUtente" UniqueName="NomeUtente" HeaderText="Utente"
                                                            DataField="NomeUtente" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("NomeUtente")%>' style="white-space: nowrap; overflow: hidden;
                                                                    text-overflow: ellipsis; width: 150px;">
                                                                    <%# Eval("NomeUtente")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn SortExpression="DataCreazione" UniqueName="DataCreazione"
                                                            HeaderText="Creato" DataField="DataCreazione" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DataCreazione","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 70px;border: 0px solid red">
                                                                    <%# Eval("DataCreazione", "{0:dd/MM/yyyy}")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn SortExpression="DataModifica" UniqueName="DataModifica"
                                                            HeaderText="Modific." DataField="DataModifica" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <div title='<%# Eval("DataModifica","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                    overflow: hidden; text-overflow: ellipsis; width: 70px;border: 0px solid red">
                                                                    <%# Eval("DataModifica", "{0:dd/MM/yyyy}")%></div>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridButtonColumn Text="Seleziona documento" ButtonType="ImageButton" CommandName="Select"
                                                            FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png" UniqueName="Select"
                                                            HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                    </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>





          

                         <%--FINE GRIDVIEW--%>

                      
                           <asp:Panel runat="server" ID="Panel1" Style="background-color: #BFDBFF">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadButton Visible="False" ID="ChiudiButton" runat="server" Text="Chiudi"
                                                Width="100px" Skin="Office2007" ToolTip="Chiudi la finestra">
                                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>


                       

                   </td>
                </tr>

            

            </table>

            </div>


            <div style="display: none">
                <asp:Button runat="server" ID="salvaContenutoButton" Style="width: 0px; height: 0px;
                    left: -1000px; position: absolute" />


                


                <asp:Button runat="server" ID="DisabilitaPulsantePredefinito" Style="width: 0px;
                    height: 0px; left: -1000px; position: absolute" />
            </div>


             <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
            <asp:HiddenField ID="documentContentHidden" runat="server" />


         

             <asp:HiddenField ID="infoOperazioneHidden" runat="server" />


        </ContentTemplate>
       
    </asp:UpdatePanel>
</asp:Content>
