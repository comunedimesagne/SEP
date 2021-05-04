<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaRubricaPage.aspx.vb"
    Inherits="RicercaRubricaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Rubrica</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link type="text/css" href="../../../../Styles/Styles.css" rel="stylesheet" />
    <style type="text/css">
        .rgAltRow, .rgRow
        {
            cursor: pointer !important;
        }
        .style1
        {
            height: 25px;
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

<body runat="server" id="CorpoPagina">


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
            <center>
                <div id="pageContent">
                    <table style="width: 900px; border: 1px solid #5D8CC9">
                        <tr>
                            <td>
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
                                                    <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveAndExit.png" Text="Salva e Chiudi"
                                                        CommandName="SalvaChiudi" Owner="RadToolBar" />
                                                    <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                        CommandName="Elimina" Owner="RadToolBar" />
                                                </Items>
                                            </telerik:RadToolBar>
                                        </td>
                                    </tr>
                                </table>
                                <telerik:RadTabStrip runat="server" ID="DatiRubricaTabStrip" SelectedIndex="0" MultiPageID="DatiRubricaMultiPage"
                                    Skin="Office2007" Width="100%">
                                    <Tabs>
                                        <telerik:RadTab Text="Anagrafica" Selected="True" />
                                        <telerik:RadTab Text="Dati aggiuntivi" />
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                                  <telerik:RadMultiPage runat="server" ID="DatiRubricaMultiPage" SelectedIndex="1"
        Height="100%" Width="100%" CssClass="multiPage" BorderColor="#3399FF">
        <telerik:RadPageView runat="server" ID="AnagraficaPageView" CssClass="corporatePageView" Height="320px">

        <br />


      
       
          <table style="width:100%">

                <tr>
                    <td style=" width:140px">
                        <asp:Label ID="DenominazioneLabel" runat="server" CssClass="Etichetta" Text="Denom./Cognome *" ForeColor="#FF8040" />
                     </td>
                      <td style="width: 260px">
                        <telerik:RadTextBox ID="DenominazioneTextBox" runat="server" Skin="Office2007" 
                              Width="240px" MaxLength="200" TabIndex="1" />
                     </td>
                      <td style="width: 50px">
                        <asp:Label ID="NomeLabel" runat="server" CssClass="Etichetta" Text="Nome"  ForeColor="#FF8040" />
                     </td>
                      <td style="width: 230px">
                        <telerik:RadTextBox ID="NomeTextBox" runat="server" Skin="Office2007" 
                              Width="220px" MaxLength="100" TabIndex="2" />
                     </td>
                      <td style="width: 50px">
                         <asp:Label ID="SessoLabel" runat="server" CssClass="Etichetta" Text="Sesso" />
                       </td>
                        <td>
                        <telerik:RadComboBox ID="SessoComboBox" runat="server" Skin="Office2007" 
                                Width="50px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" 
                                Filter="StartsWith" MaxHeight="400px" TabIndex="3" />
                   </td>
                  </tr>
               </table>

           <table style="width:100%">

                <tr>
                    <td style=" width:140px">
                       <asp:Label ID="IndirizzoResidenzaLabel" runat="server" CssClass="Etichetta" Text="Indirizzo residenza *" ForeColor="#FF8040" />
                  </td>
                   <td>
                       <telerik:RadTextBox ID="IndirizzoResidenzaTextBox" runat="server" 
                           Skin="Office2007" Width="240px" MaxLength="200" TabIndex="4" />
                 </td>

                    <td>
                      <asp:Label ID="ComuneResidenzaLabel" runat="server" CssClass="Etichetta" Text="Comune residenza *" ForeColor="#FF8040" />
                     </td>
                     <td>
                      <telerik:RadTextBox ID="ComuneResidenzaTextBox" runat="server" Skin="Office2007" 
                             Width="170px" MaxLength="40" TabIndex="5" />
                       <telerik:RadTextBox ID="CapResidenzaTextBox" runat="server" 
                             ToolTip="C.A.P. del comune di residenza" Skin="Office2007" Width="45px"  
                             MaxLength="5" TabIndex="6" />
                       <telerik:RadTextBox ID="ProvinciaResidenzaTextBox" 
                             ToolTip="Provincia del comune di residenza" runat="server" Skin="Office2007" 
                             Width="30px" MaxLength="2" TabIndex="7" />
                       <asp:ImageButton ID="TrovaComuneResidenzaImageButton" runat="server" 
                             ImageUrl="~/images//knob-search16.png" 
                             ToolTip="Seleziona comune di residenza..." TabIndex="8" />
                       <asp:ImageButton ID="EliminaComuneResidenzaImageButton" runat="server" 
                             ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella comune di residenza" 
                             TabIndex="9" />
                       <asp:ImageButton ID="AggiornaComuneResidenzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                   </td>
                  
                 </tr>


                <tr>
                     <td style=" width:140px">
                    <asp:Label ID="IndirizzoUfficioLabel" runat="server" CssClass="Etichetta" Text="Indirizzo ufficio" />
                     </td>
                    <td>
                    <telerik:RadTextBox ID="IndirizzoUfficioTextBox" runat="server" Skin="Office2007" 
                            Width="240px" MaxLength="200" TabIndex="10" />
                  </td>

                  <td>
                    <asp:Label ID="ComuneUfficoLabel" runat="server" CssClass="Etichetta" Text="Comune ufficio" />
                    </td>
                 <td>
                    <telerik:RadTextBox ID="ComuneUfficioTextBox" runat="server" Skin="Office2007" 
                         Width="170px" MaxLength="40" TabIndex="11" />
                    <telerik:RadTextBox ID="CapUfficioTextBox"  
                         ToolTip="C.A.P. del comune dell'ufficio" runat="server" Skin="Office2007" 
                         Width="45px" MaxLength="5" TabIndex="12" />
                    <telerik:RadTextBox ID="ProvinciaUfficioTextBox" 
                         ToolTip="Provincia del comune dell'ufficio" runat="server" Skin="Office2007" 
                         Width="30px" MaxLength="2" TabIndex="13" />
                    <asp:ImageButton ID="TrovaComuneUfficioImageButton" runat="server" 
                         ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona comune ufficio..." 
                         TabIndex="14" />
                    <asp:ImageButton ID="EliminaComuneUfficioImageButton" runat="server" 
                         ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella comune ufficio" 
                         TabIndex="15" />
                    <asp:ImageButton ID="AggiornaComuneUfficioImageButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                           
                  </td>
                  
                 
                   
                </tr>


                <tr>
                   <td style=" width:130px">
                  <asp:Label ID="ComuneNascitaLabel" runat="server" CssClass="Etichetta" Text="Comune nascita" />
                  </td>
                   <td>
                  <telerik:RadTextBox ID="ComuneNascitaTextBox" runat="server" Skin="Office2007" 
                           Width="170px" MaxLength="40" TabIndex="16" />
                  <telerik:RadTextBox ID="CapNascitaTextBox" runat="server" Skin="Office2007" 
                           Width="45px" ToolTip="C.A.P. del comune di nascita" MaxLength="5" 
                           TabIndex="17" />
                  <telerik:RadTextBox ID="ProvinciaNascitaTextBox" runat="server" Skin="Office2007" 
                           ToolTip="Provincia del comune di nascita"  Width="30px" MaxLength="2" 
                           TabIndex="18" />
                  <telerik:RadTextBox ID="CodiceIstatTextBox" runat="server" Skin="Office2007" Width="215px"  MaxLength="40" Style="display: none" />
                  <asp:ImageButton ID="TrovaComuneNascitaImageButton" runat="server" 
                           ImageUrl="~/images//knob-search16.png" ToolTip="Seleziona comune di nascita..." 
                           TabIndex="19" />
                  <asp:ImageButton ID="EliminaComuneNascitaImageButton" runat="server" 
                           ImageUrl="~/images//RecycleEmpty.png" ToolTip="Cancella comune di nascita" 
                           TabIndex="20" />
                  <asp:ImageButton ID="AggiornaComuneNascitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                  </td>
                 <td>
                  <asp:Label ID="DataNascitaLabel" runat="server" CssClass="Etichetta" Text="Data nascita" />
                   </td>
                    <td>
                  <telerik:RadDatePicker ID="DataNascitaTextBox" Skin="Office2007" Width="100px" 
                            runat="server" MinDate="1753-01-01" TabIndex="21" >
                      <Calendar Skin="Office2007" UseColumnHeadersAsSelectors="False" 
                          UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                      </Calendar>
                      <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" TabIndex="21">
                      </DateInput>
                      <DatePopupButton HoverImageUrl="" ImageUrl="" TabIndex="21" />
                        </telerik:RadDatePicker>
                </td>
                 
                   
                   
                </tr>

                </table>


            <table style="width:100%">
                <tr>
                    <td style="width: 140px">
                        <asp:Label ID="TelefonoLabel" runat="server" CssClass="Etichetta" Text="Telefono"
                            ForeColor="#FF8040" />
                    </td>
                    <td style="width: 210px">
                        <telerik:RadTextBox ID="TelefonoTextBox" runat="server" Skin="Office2007" Width="190px"
                            MaxLength="50" TabIndex="22" />
                    </td>
                    <td style="width: 30px">
                        <asp:Label ID="FaxLabel" runat="server" CssClass="Etichetta" Text="Fax" />
                    </td>
                    <td style="width: 205px">
                        <telerik:RadTextBox ID="FaxTextBox" runat="server" Skin="Office2007" Width="190px"
                            MaxLength="25" TabIndex="23" />
                    </td>
                    <td>
                        <asp:Label ID="EmailLabel" runat="server" CssClass="Etichetta" Text="E-mail" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="EmailTextBox" runat="server" Skin="Office2007" Width="240px"
                            MaxLength="250" />
                    </td>
                </tr>
            </table>

           <table style="width:100%">
                <tr>
                    <td style="width: 130px">
                        <asp:Label ID="DocumentoIdentitaLabel" runat="server" CssClass="Etichetta" 
                            Text="Documento" /> &nbsp; &nbsp;&nbsp; 
                           <asp:Label ID="TipoDocumentoIdentitaLabel" runat="server" CssClass="Etichetta" Text="Tipo" />
                    </td>
                    <td style="width: 100px">
                        <telerik:RadComboBox ID="TipoDocumentoIdentitaComboBox" Runat="server" EmptyMessage="- Seleziona Tipo -" Filter="StartsWith" ItemsPerRequest="10"  MaxHeight="400px" Skin="Office2007"  Width="150px" />
                    </td>

                    <td style="width: 60px">
                        <asp:Label ID="NumeroDocumentoIdentitaLabel" runat="server" CssClass="Etichetta" Text="Numero" />
                    </td>
                    <td style="width: 205px">
                        <telerik:RadTextBox ID="NumeroDocumentoIdentitaTextBox" runat="server" 
                            Skin="Office2007" Width="210px"  MaxLength="20" />
                    </td>
                    <td style="width: 90px">
                        <asp:Label ID="DocumentoIdentitaEnteRilascioLabel" runat="server" CssClass="Etichetta" Text="Rilasciato Da" />
                    </td>
                   <td style="width: 190px">
                        <telerik:RadTextBox ID="DocumentoIdentitaEnteRilascioTextBox" runat="server" 
                            Skin="Office2007" Width="210px"  MaxLength="50" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr style="height: 100px">
                    <td style="width: 140px">
                        <asp:Label ID="TipoReferenteLabel" runat="server" CssClass="Etichetta" Text="Tipo referente *"
                            ForeColor="#FF8040" />
                    </td>
                    <td style="width: 280px">
                        <telerik:RadListBox ID="TipoReferenteListBox" runat="server" Skin="Office2007" Style="width: 250px;
                            height: 100px" Height="100px" SortCaseSensitive="False" Sort="Ascending" CheckBoxes="True"
                            TabIndex="26" />
                    </td>
                    <td valign="top">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="CodiceFiscaleLabel" runat="server" CssClass="Etichetta" Text="Codice fiscale" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="CodiceFiscaleTextBox" runat="server" Skin="Office2007" Width="180px"
                                        MaxLength="16" TabIndex="24" />&nbsp;<telerik:RadButton ID="CalcolaCodiceFiscaleButton"
                                            runat="server" Text="Calcola" Width="60px" Skin="Office2007" TabIndex="25" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="TipoPersonaLabel" runat="server" CssClass="Etichetta" Text="Tipo persona *"
                                        ForeColor="#FF8040" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="TipoPersonaComboBox" runat="server" EmptyMessage="- Seleziona Tipo -"
                                        Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                                        Width="150px" />
                                </td>
                            </tr>

                                 <tr>
                                 <td style="width: 100px">
                                    <asp:Label ID="AziendaLabel" runat="server" CssClass="Etichetta" Text="Ragione sociale" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="AziendaTextBox" runat="server" Skin="Office2007" Width="280px"
                                        MaxLength="100" />
                                </td>
                            </tr>

                            <tr>
                                 <td style="width: 100px">
                                    <asp:Label ID="PartitaIvaLabel" runat="server" CssClass="Etichetta" Text="Partita IVA" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="PartitaIvaTextBox" runat="server" Skin="Office2007" Width="280px"
                                        MaxLength="11" />
                                </td>
                            </tr>

                            <tr>
                                 <td style="width: 100px">
                                    <asp:Label ID="IbanLabel" runat="server" CssClass="Etichetta" Text="IBAN" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="IbanTextBox" runat="server" Skin="Office2007" Width="280px"
                                        MaxLength="27" />
                                </td>
                            </tr>

                       
                        </table>
                    </td>
                </tr>
            </table>

        </telerik:RadPageView>

        <telerik:RadPageView runat="server" ID="AziendaPageView" CssClass="corporatePageView"
            Height="320px">
            <br />

            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="CellulareLabel" runat="server" CssClass="Etichetta" Text="Cellulare" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="CellulareTextBox" runat="server" Skin="Office2007" Width="280px"
                            MaxLength="25" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="TitoloLabel" runat="server" CssClass="Etichetta" Text="Titolo" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="TitoloTextBox" runat="server" Skin="Office2007" Width="280px"
                            MaxLength="50" />
                    </td>
                    <td>
                        <asp:Label ID="SitoWebLabel" runat="server" CssClass="Etichetta" Text="Sito WEB" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="SitoWebTextBox" runat="server" MaxLength="50" Skin="Office2007"
                            Width="280px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="NumeroIscrizioneAlboLabel" runat="server" CssClass="Etichetta" Text="Num. iscr. albo" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="NumeroIscrizioneAlboTextBox" runat="server" MaxLength="6"
                            Skin="Office2007" Width="60px" />
                        &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:Label ID="ProvinciaAlboLabel" runat="server" CssClass="Etichetta" Text="Provincia albo" />
                        &nbsp; &nbsp;
                        <telerik:RadTextBox ID="ProvinciaAlboTextBox" runat="server" MaxLength="2" Skin="Office2007"
                            Width="50px" />
                    </td>
                    <td>
                        <asp:Label ID="AlboProfessionaleLabel" runat="server" CssClass="Etichetta" Text="Albo professionale" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="AlboProfessionaleComboBox" runat="server" EmptyMessage="- Selezionare -"
                            Filter="StartsWith" ItemsPerRequest="10" MaxHeight="400px" Skin="Office2007"
                            Width="280px" />
                    </td>
                </tr>
            </table>

        </telerik:RadPageView>

    </telerik:RadMultiPage>


                                <div style="display: none">
                                    <asp:Button runat="server" ID="DisabilitaPulsantePredefinito" Style="width: 0px;
                                        height: 0px; display: none" />
                                </div>
                                <table style="width: 900px; background-color: #BFDBFF; border: 0 solid #00156E">
                                    <tr>
                                        <td style="height: 20px">
                                            &nbsp;<asp:Label CssClass="Etichetta" ID="TitoloElencoReferentiLabel" runat="server"
                                                Font-Bold="True" Style="width: 800px; color: #00156E; background-color: #BFDBFF"
                                                Text="Elenco Referenti" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%; height: 300px; background-color: #FFFFFF">
                                                <telerik:RadGrid ID="RubricaGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    GridLines="None" Skin="Office2007" Width="895px" AllowSorting="True" Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                            <%--  <telerik:GridTemplateColumn  SortExpression="Azienda" UniqueName="Azienda" HeaderText="Azienda" DataField="Azienda" HeaderStyle-Width="120px" ItemStyle-Width="120px" Visible="False">    
                                           <ItemTemplate >   
                                             <div title='<%# Eval("Azienda")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:120px;" ><%# Eval("Azienda")%></div>
                                           </ItemTemplate>    
                                      </telerik:GridTemplateColumn> --%>
                                                            <telerik:GridTemplateColumn SortExpression="Denominazione" UniqueName="Denominazione"
                                                                HeaderText="Cognome/Denominazione" DataField="Denominazione" HeaderStyle-Width="245px"
                                                                ItemStyle-Width="245px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Denominazione")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 245px; border: 0px solid red;">
                                                                        <%# Eval("Denominazione")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="Email" UniqueName="Email" HeaderText="Email"
                                                                DataField="Email" HeaderStyle-Width="245px" ItemStyle-Width="245px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Email")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                        width: 245px; border: 0px solid red;">
                                                                        <%# Eval("Email")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="Comune" UniqueName="Comune" HeaderText="Città"
                                                                DataField="Comune" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Comune")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                        width: 120px; border: 0px solid red;">
                                                                        <%# Eval("Comune")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn SortExpression="Indirizzo" UniqueName="Indirizzo" HeaderText="Indirizzo"
                                                                DataField="Indirizzo" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <div title='<%# Eval("Indirizzo")%>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 120px; border: 0px solid red;">
                                                                        <%# Eval("Indirizzo")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn Text="Seleziona referente" ButtonType="ImageButton" CommandName="Select"
                                                                FilterControlAltText="Filter Select column" ImageUrl="~\images\checks.png" UniqueName="Select"
                                                                HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                            <telerik:GridButtonColumn Text="Seleziona referente e chiudi" FilterControlAltText="Filter ConfirmSelectAndClose column"
                                                                ImageUrl="~/images/accept.png" UniqueName="ConfirmSelectAndClose" ButtonType="ImageButton"
                                                                CommandName="ConfirmSelectAndClose" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                            <telerik:GridButtonColumn Text="Copia referente" FilterControlAltText="Filter Copy column"
                                                                ImageUrl="~/images/copy16.png" UniqueName="Copy" ButtonType="ImageButton" CommandName="Copy"
                                                                HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </ContentTemplate>
        <%--   <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DatiRubricaTabStrip" EventName="TabClick" />
        </Triggers>--%>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hflVerificaElimina" runat="server" />
    </form>
</body>
</html>
