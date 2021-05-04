<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="ProcedimentiPage.aspx.vb" Inherits="ProcedimentiPage" %>

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
     
          var message = $get('<%= infoOperazioneHidden.ClientId %>').value;
          var showIcon = 1;

          if (message !== '') {

              count = 2;
              if (message != 'Operazione conclusa con successo!') {
                  count = 8;
                  showIcon = 0;
              }

             
              //VISUALIZZO IL MESSAGGIO

              ShowMessageBox(message, showIcon);

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


      function ShowMessageBox(message, showIcon) {


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
               if (showIcon==1) {

              style.textAlign = 'center';
              style.verticalAlign = 'middle';
          }
              innerHTML = message;
              style.color = '#00156E';

              if (showIcon==1) {

             
              style.backgroundImage = 'url(/sep/Images/success.png)';
              style.backgroundPosition = '5px center';
              style.backgroundRepeat = 'no-repeat';
          }

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

      function CheckUpload(e) {
          var msg = 'Per inserire un modulo, è necessario specificare:\n';
//          var upload = $find("<%= AllegatoUpload.ClientID %>");
//          var inputs = upload.getUploadedFiles();

          var nome = $find("<%= NomeModuloTextBox.ClientID %>").get_value();
          var descrizione = $find("<%= DescrizioneModuloTextBox.ClientID %>").get_value();

          var cancel = false;

          if (nome == '') {
              cancel = true;
              msg += 'il nome\n'
          }

          if (descrizione == '') {
              cancel = true;
              msg += 'la descrizione\n'
          }

//          if (inputs.length == 0) {
//              cancel = true;
//              msg += 'il relativo file'
//             
//          }

          if (cancel) {
            
              alert(msg);
              var evt = e ? e : window.event;
              if (evt.stopPropagation) evt.stopPropagation();
              if (evt.cancelBubble != null) evt.cancelBubble = true;
              return false;
          }





      }


      function OnClientFileUploaded(sender, args) {
//          var contentType = args.get_fileInfo().ContentType;
          //          alert(contentType);
          //var btn = $get("<%= AggiungiModuloImageButton.ClientID %>");
          //btn.click();
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

             <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID1" Skin="Web20"></telerik:RadFormDecorator>
                      <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator>

                      <telerik:RadFormDecorator ID="RadFormDecorator3" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID3" Skin="Web20"></telerik:RadFormDecorator>


                        <telerik:RadFormDecorator ID="RadFormDecorator4" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID4" Skin="Web20"></telerik:RadFormDecorator>

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
                                                    CommandName="Nuovo" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Searchb.png" Text="Trova"
                                                    CommandName="Trova" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Delete.png" Text="Annulla"
                                                    CommandName="Annulla" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/SaveB.png" Text="Salva"
                                                    CommandName="Salva" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Trashcanempty.png" Text="Elimina"
                                                    CommandName="Elimina" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                    CommandName="Stampa" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar">
                                                </telerik:RadToolBarButton>
                                            </Items>
                                        </telerik:RadToolBar>
                                    </td>
                                </tr>
                            </table>

                              <%--FINE TOOLBAR--%>

                           



                          


                            <telerik:RadTabStrip runat="server" ID="AttiTabStrip" SelectedIndex="0" MultiPageID="AttiMultiPage"
                                Skin="Office2007" Width="100%">
                                <Tabs>
                                    <telerik:RadTab Text="Generale" Selected="True" ToolTip="Dati generali procedimento" />
                                    <telerik:RadTab Text="Documentazione" ToolTip="Elenco documenti associati al procedimento" />
                                    <telerik:RadTab Text="Visibilità" ToolTip="Settori che hanno la visibilità al procedimento" />
                                </Tabs>
                            </telerik:RadTabStrip>
                            <!--no spaces between the tabstrip and multipage, in order to remove unnecessary whitespace-->
                            <telerik:RadMultiPage runat="server" ID="AttiMultiPage" SelectedIndex="0" Height="100%"
                                Width="100%" CssClass="multiPage" BorderColor="#3399FF">
                               
                                <telerik:RadPageView runat="server" ID="GeneralePageView" CssClass="corporatePageView"
                                    Height="435px">

                                    <div id="GeneralePanel" runat="server" style="padding: 2px 2px 2px 2px;">

                                        <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9; height:430px">

                                            <tr>
                                                <td style="width: 100%; vertical-align: top">
                                                    <table style="width: 100%">
                                                        <tr style="height: 20px">
                                                            <td style=" width:50%; padding-right:4px">
                                                                <asp:Label ID="NomeLabel" runat="server" CssClass="Etichetta" Text="Nome *"
                                                                    ForeColor="#FF8040" />
                                                            </td>
                                                         
                                                            <td style=" width:50%;padding-left:4px">
                                                                <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione"
                                                                     />
                                                            </td>
                                                          
                                                        </tr>
                                                        <tr>
                                                           <td style=" width:50%; padding-right:4px">
                                                                <telerik:RadTextBox ID="NomeTextBox" runat="server" Skin="Office2007" Width="430px"
                                                                    MaxLength="2000" ToolTip="Nome del procedimento" Rows="3" TextMode="MultiLine"
                                                                    Style="overflow-x: hidden" />
                                                            </td>
                                                         
                                                              <td style=" width:50%;padding-left:4px">
                                                                <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="430px"
                                                                    MaxLength="2000" ToolTip="Descrizione del procedimento" Rows="3" TextMode="MultiLine"
                                                                    Style="overflow-x: hidden" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            

                                            <tr>
                                                <td style="width: 100%; vertical-align: top">
                                                    <table style="width: 100%">
                                                        <tr style="height: 20px">
                                                             <td style=" width:50%; padding-right:4px">
                                                                <asp:Label ID="NormativaLabel" runat="server" CssClass="Etichetta" Text="Normativa di riferimento" />
                                                            </td>
                                                         <td style=" width:50%;padding-left:4px">
                                                                <asp:Label ID="IndicazioniLabel" runat="server" CssClass="Etichetta" Text="Indicazioni" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                             <td style=" width:50%; padding-right:4px">
                                                                <telerik:RadTextBox ID="NormativaTextBox" runat="server" Skin="Office2007" Width="430px"
                                                                    MaxLength="2000" ToolTip="Normativa di riferimento del procedimento" Rows="3"
                                                                    TextMode="MultiLine" Style="overflow-x: hidden" />
                                                            </td>
                                                             <td style=" width:50%;padding-left:4px">
                                                                <telerik:RadTextBox ID="IndicazioniTextBox" runat="server" Skin="Office2007" Width="430px"
                                                                    MaxLength="2000" ToolTip="" Rows="3"
                                                                    TextMode="MultiLine" Style="overflow-x: hidden" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                           

                                             <tr>
                                                <td style="width: 100%; vertical-align: top">
                                                    <table style="width: 100%">
                                                        <tr style="height: 20px">
                                                            <td>
                                                                <asp:Label ID="NoteLabel" runat="server" CssClass="Etichetta" Text="Note" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" 
                                                                    Width="875px" ToolTip="" Rows="3"
                                                                    TextMode="MultiLine" Style="overflow-x: hidden;"   />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 110px">
                                                                <asp:Label ID="ResponsabileLabel" runat="server" CssClass="Etichetta" Text="Responsabile *" />
                                                            </td>
                                                             <td style="width: 400px">
                                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 390px; height: 19px">
                                                                    <asp:Label ID="ResponsabileTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                        runat="server" Width="390px" ToolTip="Nominativo del responsabile del procedimento">&nbsp;</asp:Label>
                                                                </span>
                                                            </td>
                                                            <td style="width: 25px; text-align: center">
                                                                <asp:ImageButton ID="TrovaResponsabileImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona responsabile..." ImageAlign="AbsMiddle" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="EliminaResponsabileImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    ToolTip="Cancella responsabile" ImageAlign="AbsMiddle" />
                                                                <asp:ImageButton ID="AggiornaResponsabileImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none; width: 0px" />
                                                                <telerik:RadTextBox ID="IdResponsabileTextBox" runat="server" Skin="Office2007" Style="display: none;
                                                                    width: 0px" />
                                                                     <telerik:RadTextBox ID="CodiceResponsabileTextBox" runat="server" Skin="Office2007"
                                                                    Width="0px" Style="display: none" />
                                                            </td>


                                                           

                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                         <td style="width: 110px">
                                                                <asp:Label ID="ResponsabileInerziaLabel" runat="server" CssClass="Etichetta" Text="Resp. Inerzia *" />
                                                            </td>
                                                            <td style="width: 400px">
                                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 390px; height: 19px">
                                                                    <asp:Label ID="ResponsabileInerziaTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                        runat="server" Width="390px" ToolTip="Nominativo del soggetto con potere sostitutivo in caso di inerzia">&nbsp;</asp:Label>
                                                                </span>
                                                            </td>
                                                            <td style="width: 25px; text-align: center">
                                                                <asp:ImageButton ID="TrovaResponsabileInerziaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ToolTip="Seleziona responsabile..." ImageAlign="AbsMiddle" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="EliminaResponsabileInerziaImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    ToolTip="Cancella responsabile" ImageAlign="AbsMiddle" />
                                                                <asp:ImageButton ID="AggiornaResponsabileInerziaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none; width: 0px" />
                                                                <telerik:RadTextBox ID="IdResponsabileInerziaTextBox" runat="server" Skin="Office2007"
                                                                    Style="display: none; width: 0px" />
                                                                      <telerik:RadTextBox ID="CodiceResponsabileInerziaTextBox" runat="server" Skin="Office2007"
                                                                    Width="0px" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>


                                         
                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 110px">
                                                                <asp:Label ID="SettoreCompetenzaLabel" runat="server" CssClass="Etichetta" Text="Settore/Area *" />
                                                            </td>
                                                            <td style="width: 400px">
                                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 390px; height: 19px">
                                                                    <asp:Label ID="SettoreCompetenzaTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                        runat="server" Width="390px" ToolTip="Struttura competente ad emettere il provvedimento">&nbsp;</asp:Label>
                                                                </span>
                                                            </td>
                                                            <td style="width: 25px; text-align: center">
                                                                <asp:ImageButton ID="TrovaSettoreCompetenzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ImageAlign="AbsMiddle" ToolTip="Seleziona settore..." />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="EliminaSettoreCompetenzaImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    ImageAlign="AbsMiddle" ToolTip="Cancella settore" />
                                                                <asp:ImageButton ID="AggiornaSettoreCompetenzaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none" />
                                                                <telerik:RadTextBox ID="IdSettoreCompetenzaTextBox" runat="server" Skin="Office2007"
                                                                    Width="0px" Style="display: none" />
                                                                <telerik:RadTextBox ID="CodiceSettoreCompetenzaTextBox" runat="server" Skin="Office2007"
                                                                    Width="0px" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 110px">
                                                                <asp:Label ID="ClassificazioneLabel" runat="server" CssClass="Etichetta" Text="Classificazione *" />
                                                            </td>
                                                            <td style="width: 400px">
                                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 390px; height: 19px">
                                                                    <asp:Label ID="ClassificazioneTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                        runat="server" Width="390px" ToolTip="Indice di classificazione completo">&nbsp;</asp:Label>
                                                                </span>
                                                            </td>
                                                            <td style="width: 25px; text-align: center">
                                                                <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    ImageAlign="AbsMiddle" ToolTip="Seleziona classificazione..." />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="EliminaClassificazioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                    ImageAlign="AbsMiddle" ToolTip="Cancella classificazione" />
                                                                <asp:ImageButton ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                    Style="display: none" />
                                                                <telerik:RadTextBox ID="RadTextBox1" runat="server" Skin="Office2007"
                                                                    Width="0px" Style="display: none" />
                                                                <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                    Width="0px" Style="display: none" />
                                                                     <telerik:RadTextBox ID="CodiceClassificazioneTextBox" runat="server" Skin="Office2007"
                                                            Style="display: none; width: 0px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="IterLabel" runat="server" CssClass="Etichetta" Text="Iter *" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="IterComboBox" runat="server" AutoPostBack="false" EmptyMessage="- Seleziona Iter -"
                                                                    MaxHeight="150px" Skin="Office2007" Width="340px" />
                                                            </td>
                                                           <td style="width: 120px; text-align:center">
                                                               <asp:Label ID="IterIntegrazioneLabel" runat="server" CssClass="Etichetta" Text="Iter Integrazione" />
                                                            </td>
                                                            <td>
                                                             <telerik:RadComboBox ID="IterIntegrazioneComboBox" runat="server" AutoPostBack="false" EmptyMessage="- Seleziona Iter Integrazione -"
                                                                    MaxHeight="150px" Skin="Office2007" Width="340px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                          

                                            <tr>
                                                <td>
                                                    <div id="ZoneID1">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                 <td style=" width:100px">
                                                                    <asp:Label ID="TipoLabel" runat="server" CssClass="Etichetta"
                                                                         Text="Avviabile da"  />
                                                                </td>
                                                                <td style=" width:90px">
                                                                    <asp:CheckBox ID="ImpresaCheckBox" runat="server" Checked="True" CssClass="etichetta"
                                                                        Text="Imprese" ToolTip=""
                                                                        Width="90px" />
                                                                </td>
                                                                <td style=" width:120px">
                                                                    <asp:CheckBox ID="ProfessionistaCheckBox" runat="server" Checked="True" CssClass="etichetta"
                                                                        Text="Professionisti" ToolTip=""
                                                                        Width="120px" />
                                                                </td>
                                                               <td style=" width:90px">
                                                                    <asp:CheckBox ID="PrivatoCheckBox" runat="server" CssClass="etichetta" Text="Privati"
                                                                        ToolTip="" Width="90px"  Checked="True" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="PubblicoCheckBox" runat="server" CssClass="etichetta" Text="Pubblico"
                                                                        ToolTip="" Width="90px"  Checked="True" />
                                                                </td>
                                                               
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 50px">
                                                                <asp:Label ID="SettoreLabel" runat="server" CssClass="Etichetta" Text="Codice" />
                                                            </td>
                                                            <td style="width: 100px">
                                                                <telerik:RadTextBox ID="CodiceTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                    MaxLength="10" />
                                                            </td>
                                                            <td style="width: 100px; text-align: center">
                                                                <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="Classe Rischio" />
                                                            </td>
                                                            <td style="width: 100px">
                                                                <telerik:RadTextBox ID="ClasseTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                    MaxLength="10" />
                                                            </td>
                                                            <td style="width: 90px; text-align: center">
                                                                <asp:Label ID="TermineLabel" runat="server" CssClass="Etichetta" Text="Termine (gg)" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadNumericTextBox ID="TermineConclusioneTextBox" runat="server" 
                                                                    Skin="Office2007" Width="70px"
                                                                    DataType="System.Int32" MaxLength="4" MaxValue="9999" MinValue="1" ToolTip="Termine di conclusione (in giorni) del procedimento"
                                                                    ShowSpinButtons="True">
                                                                    <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                </telerik:RadNumericTextBox>
                                                            </td>
                                                            
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 130px;">
                                                                <asp:Label ID="GiorniSospensioneLabel" runat="server" CssClass="Etichetta" Text="Giorni Sospensione" />
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <telerik:RadNumericTextBox ID="GiorniSospensioneTextBox" runat="server" Skin="Office2007"
                                                                    Width="70px" DataType="System.Int32" MaxLength="4" MaxValue="9999" MinValue="1"
                                                                    ToolTip="Termine di conclusione (in giorni) della sospensione" ShowSpinButtons="True">
                                                                    <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                </telerik:RadNumericTextBox>
                                                            </td>

                                                             <td style="width: 140px;">
                                                                <div id="ZoneID3">
                                                                 <asp:Label ID="PubblicabileLabel" runat="server" CssClass="Etichetta" Text="Pubblicabile" />&nbsp;<asp:CheckBox
                                                                    ID="PubblicabileCheckBox" runat="server" Text="&nbsp;"  />
                                                                </div>
                                                            </td>

                                                            <td>
                                                              <div id="ZoneID2">

                                                               <asp:Label ID="AbilitatoLabel" runat="server" CssClass="Etichetta" Text="Abilitato" />&nbsp;<asp:CheckBox
                                                                    ID="AbilitatoCheckBox" runat="server" Text="&nbsp;"  />
                                                               
                                                            </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                       

                                            

                                        </table>
                                    </div>

                                </telerik:RadPageView>


                                <telerik:RadPageView runat="server" ID="ModuliPageView" CssClass="corporatePageView"
                                    Height="435px">

                                    <div id="ModuliPanel" runat="server" style="padding: 2px 2px 2px 2px;">
                                        <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">

                                            <tr>
                                                <td>

                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style=" width:50%">
                                                                <asp:Label ID="NomeModuloLabel" runat="server" CssClass="Etichetta" Text="Nome *" />
                                                            </td>
                                                            <td style=" width:50%">
                                                                <asp:Label ID="NomeFileModuloLabel" runat="server" CssClass="Etichetta" Text="Nome file" />
                                                            </td>
                                                        </tr>
                                                        <tr style="height:35px">
                                                            <td>
                                                                <telerik:RadTextBox ID="NomeModuloTextBox" runat="server" Skin="Office2007" MaxLength="50"
                                                                    Width="99%" />
                                                            </td>
                                                            <td style=" vertical-align:bottom">
                                                                <telerik:RadAsyncUpload ID="AllegatoUpload" runat="server" MaxFileInputsCount="1"
                                                                    Skin="Office2007" Width="400px" InputSize="56" EnableViewState="True" OnClientFileUploaded="OnClientFileUploaded">
                                                                    <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..."  />
                                                                </telerik:RadAsyncUpload>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table style="width: 100%">
                                                       <tr>
                                                            <td style=" width:50%">
                                                                <asp:Label ID="DescrizioneModuloLabel" runat="server" CssClass="Etichetta" Text="Descrizione *" />
                                                            </td>

                                                             <td style=" width:50%">
                                                                <asp:Label ID="NoteModuloLabel" runat="server" CssClass="Etichetta" Text="Note" />
                                                            </td>

                                                        </tr>
                                                         <tr>
                                                            <td>
                                                                <telerik:RadTextBox ID="DescrizioneModuloTextBox" runat="server" Skin="Office2007"
                                                                    Width="430px" MaxLength="250" Rows="2" TextMode="MultiLine" Style="overflow-x: hidden" />
                                                            </td>
                                                              <td>
                                                                <telerik:RadTextBox ID="NoteModuloTextBox" runat="server" Skin="Office2007" Width="430px"
                                                                    MaxLength="250" Rows="2" TextMode="MultiLine" Style="overflow-x: hidden" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                   
                                                    <div id="ZoneID4">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 150px">
                                                                    <asp:Label ID="ObbligatorioLabel" runat="server" CssClass="Etichetta" Text="Obbligatorio" />&nbsp;<asp:CheckBox
                                                                        ID="ObbligatorioCheckBox" runat="server" Text="&nbsp;" Checked="true" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="ObbligatorioFirmaDigitaleLabel" runat="server" CssClass="Etichetta"
                                                                        Text="Obbligo Firma Digitale" />&nbsp;<asp:CheckBox ID="ObbligatorioFirmaDigitaleCheckBox"
                                                                            runat="server" Text="&nbsp;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>

                                        </table>
                                    </div>

                                    <div  id="GrigliaModuliPanel" runat="server" style="padding: 2px 2px 2px 2px;">

                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 20px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="ModuliLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 700px; color: #00156E; background-color: #BFDBFF" 
                                                                    Text="Documentazione" />
                                                            </td>
                                                            <td align="right">
                                                               <asp:ImageButton ID="AggiungiModuloImageButton" runat="server" ImageUrl="~/images//add16.png"
                                                                  OnClientClick="return  CheckUpload(event);"       ToolTip="Allega modulo"  ImageAlign="AbsMiddle" BorderStyle="None" />
                                                                       
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div style="overflow: auto; height: 240px; border: 1px solid #5D8CC9">
                                                        <telerik:RadGrid ID="ModuliGridView" runat="server" ToolTip="Elenco allegati associati al documento"
                                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                             Culture="it-IT" AllowMultiRowSelection="true">
                                                            <MasterTableView DataKeyNames="Id, Nomefile">
                                                                <Columns>

                                                                 

                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                    </telerik:GridBoundColumn>
                                                                    
                                                                   
                                                                        <telerik:GridTemplateColumn SortExpression="Nome" UniqueName="Oggetto" HeaderText="Nome"
                                                                        DataField="Nome" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Nome")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 200px; border: 0px solid red">
                                                                             <%# Eval("Nome")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                      <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione" HeaderText="Descrizione"
                                                                        DataField="Descrizione" HeaderStyle-Width="260px" ItemStyle-Width="260px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 260px; border: 0px solid red">
                                                                           <%# Eval("Descrizione")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                   
                                                                    <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                        DataField="NomeFile" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 200px; border: 0px solid red">
                                                                           <%# Eval("NomeFile")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="30px" ItemStyle-Width="30px" DataField="Obbligatorio"
                                                                        FilterControlAltText="Filter Obbligatorio column" HeaderText="Obbl."
                                                                        SortExpression="Obbligatorio" UniqueName="Obbligatorio">
                                                                        <ItemTemplate>
                                                                            <div title='<%# if(Eval("Obbligatorio"),"SI","NO")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 30px; border: 0px solid red">
                                                                                <%# If(Eval("Obbligatorio"), "SI", "NO")%>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="30px" ItemStyle-Width="30px" DataField="ObbligoFirmaDigitale"
                                                                        FilterControlAltText="Filter ObbligoFirmaDigitale column" HeaderText="Firma"
                                                                        SortExpression="ObbligoFirmaDigitale" UniqueName="ObbligoFirmaDigitale">
                                                                        <ItemTemplate>
                                                                            <div title='<%# if(Eval("ObbligoFirmaDigitale"),"SI","NO")%>' style="white-space: nowrap;
                                                                                overflow: hidden; text-overflow: ellipsis; width: 30px; border: 0px solid red">
                                                                                <%# If(Eval("ObbligoFirmaDigitale"), "SI", "NO")%>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                 
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                        ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="10px" Text="Visualizza Documento..."
                                                                        ItemStyle-Width="10px">
                                                                    </telerik:GridButtonColumn>

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                        ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="10px" Text="Cancella Documento"
                                                                        ItemStyle-Width="10px">
                                                                    </telerik:GridButtonColumn>

                                                                </Columns>
                                                            </MasterTableView></telerik:RadGrid></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>



                                </telerik:RadPageView>



                                <telerik:RadPageView runat="server" ID="VisibilitaPageView" CssClass="corporatePageView"
                                    Height="435px">


                                

                                    <div  id="VisibilitaPanel" runat="server" style="padding: 2px 2px 2px 2px;">

                                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                            <tr>
                                                <td style="height: 25px;  vertical-align:top">
                                                    <table style="width: 100%">
                                                        <tr style="height:25px" >
                                                            <td style=" width:120px">
                                                                <asp:Label ID="StrutturaVisibilitaLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                    Style="width: 120px; color: #00156E; background-color: #BFDBFF" 
                                                                    Text="Settore/Area" />
                                                            </td>

                                                            <td style="width: 400px">
                                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 21px">
                                                                    <asp:Label ID="SettoreTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                        runat="server" Width="100%" ToolTip="" Text="&nbsp;" />
                                                                </span>
                                                            </td>

                                                             <td style="width: 25px; text-align: center">
                                                                        <asp:ImageButton ID="TrovaSettoreImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            ImageAlign="AbsMiddle" ToolTip="Seleziona settore/area..." />
                                                                    </td>

                                                                    <td style="width: 25px; text-align: center">
                                                                        <asp:ImageButton ID="addSettoreCommandButton" runat="server" BorderStyle="None" ImageAlign="AbsMiddle"
                                                                            ImageUrl="~/images/add16.png" ToolTip="Aggiungi settore/area selezionata" />
                                                                    </td>

                                                                    <td>
                                                                        <asp:ImageButton ID="AggiornaSettoreImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                            Style="display: none" />
                                                                        <telerik:RadTextBox ID="IdSettoreTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                            Style="display: none" />
                                                                        <telerik:RadTextBox ID="CodiceSettoreTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                            Style="display: none" />
                                                                    </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #FFFFFF">
                                                <td>
                                                    <div style="overflow: auto; height: 389px; border: 1px solid #5D8CC9">
                                                               <telerik:RadGrid ID="SettoriGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                        CellSpacing="0" GridLines="None" Skin="Office2007"  
                                                                        Culture="it-IT" >

                                                                    <MasterTableView DataKeyNames="id, CodiceStruttura">
                                                                        <Columns>

                                                                            <telerik:GridBoundColumn DataField="id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                HeaderText="id" ReadOnly="True" SortExpression="id" UniqueName="id" Visible="False" />

                                                                            <telerik:GridTemplateColumn SortExpression="descrizione" UniqueName="descrizione" 
                                                                                HeaderText="Settore/Area" DataField="descrizione" HeaderStyle-Width="760px" ItemStyle-Width="760px">
                                                                                <ItemTemplate>
                                                                                    <div title='<%# Eval("Struttura.descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                        text-overflow: ellipsis; width: 760px;">
                                                                                        <%# Eval("Struttura.descrizione")%>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                ImageUrl="~\images\Delete16.png" UniqueName="Delete" ConfirmText="Sei sicuro di voler eliminare l'elemento selezionato?"
                                                                                ItemStyle-Width="20px" HeaderStyle-Width="20px" Text="Cancella Settore/Area" />
                                                                            
                                                                     

                                                                        </Columns>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                     </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>


                                </telerik:RadPageView>


                            </telerik:RadMultiPage>


                            <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                         
                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                     <tr>
                                        <td>
                                            <table style="width: 100%; background-color: #BFDBFF">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="TitoloLabel" runat="server" Font-Bold="True" Style="width: 800px;
                                                            color: #00156E; background-color: #BFDBFF" Text="Elenco Procedimenti" CssClass="Etichetta" />
                                                    </td>
                                                <td align="center" style="width: 40px">
                                                   <asp:ImageButton ID="EsportaInExcelImageButton" 
                                                       Style="border: 0; width:20px; height:20px" runat="server" 
                                                       ImageUrl="~/images//excel32.png" 
                                                       ToolTip="Esporta procedimenti in un file formato excel" 
                                                       ImageAlign="AbsMiddle" />
                                               </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 210px; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <telerik:RadGrid ID="ProcedimentiGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" PageSize="6"
                                                    Culture="it-IT">
                                                    <MasterTableView DataKeyNames="Id">
                                                        <Columns>
                                                            
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="70px" ItemStyle-Width="70px" DataField="Codice"
                                                                FilterControlAltText="Filter Codice column" HeaderText="Codice" SortExpression="Codice"
                                                                UniqueName="Codice">
                                                                <ItemTemplate>
                                                                    <div title='<%# Replace(Eval("Codice"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                        <%# Eval("Codice")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderStyle-Width="650px" ItemStyle-Width="650px" DataField="Nome"
                                                                FilterControlAltText="Filter Nome column" HeaderText="Nome" SortExpression="Nome"
                                                                UniqueName="Nome">
                                                                <ItemTemplate>
                                                                    <div title='<%# Replace(Eval("Nome"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                        overflow: hidden; text-overflow: ellipsis; width: 650px; border: 0px solid red">
                                                                        <%# Eval("Nome")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn SortExpression="Tempo" UniqueName="Tempo" HeaderText="Termine"
                                                                DataField="Tempo" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                                <ItemTemplate>
                                                               <div title='<%# Eval("Tempo") %>' style="white-space: nowrap; overflow: hidden;
                                                                        text-overflow: ellipsis; width: 65px; border: 0px solid red">
                                                                        <%# Eval("Tempo")%></div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>


                                                           

                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                                ItemStyle-Width="20px" Text="Seleziona Procedimento" FilterControlAltText="Filter Select column"
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
                </table>

            </div>
              <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
