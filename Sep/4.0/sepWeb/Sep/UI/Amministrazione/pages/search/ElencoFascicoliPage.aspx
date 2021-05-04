<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ElencoFascicoliPage.aspx.vb"
    Inherits="ElencoFascicoliPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ricerca Fascicoli</title>
   <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />


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

              var h = document.getElementById("CorpoPagina").offsetHeight;
              var w = document.getElementById("CorpoPagina").offsetWidth;


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

<body runat="server"  id="CorpoPagina">

   <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loadingOuter" style="position: absolute; width: 100%; text-align: center;
                top: 300px">
                <table cellpadding="4" style="background-color: #4892FF">
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
    
      <div id="pageContent">

        <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" runat="server"
                    DecorationZoneID="ZoneID2" Skin="Web20"></telerik:RadFormDecorator>
    
        <center>

            <asp:Panel ID="FiltroPanel" runat="server">
                <table width="900px" cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td>
                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                        &nbsp;<asp:Label ID="TitoloFiltroLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                            Text="Filtro Fascicolo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ContainerMargin">
                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; height: 515px; width: 100%; background-color: #DFE8F6;
                                                        border: 0px solid #5D8CC9;">
                                                     

                                                         <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="TipoFascicoloFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                Text="Tipo Fascicolo" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="TipologiaFascicoloFiltroComboBox" runat="server" EmptyMessage="Seleziona Tipo Fascicolo"
                                                                                Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                                Width="270px" Enabled="true" ToolTip="Tipo di fascicolo" />
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%">
                                                                 
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="NumeroDataProtocolloFiltroLabel" runat="server" 
                                                                                CssClass="Etichetta" Text="Numero Registro" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="NumeroProtocolloInizioFiltroLabel" runat="server" 
                                                                                            CssClass="Etichetta" Text="da" />
                                                                                    </td>
                                                                                    <td style="width: 70px">
                                                                                        <telerik:RadNumericTextBox ID="NumeroRegistroInizioTextBox" runat="server" Skin="Office2007"
                                                                                            Width="60px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                            ShowSpinButtons="True" ToolTip="Numero del registro  (inizio)">
                                                                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                        </telerik:RadNumericTextBox>
                                                                                    </td>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="NumeroProtocolloFineFiltroLabel" runat="server" 
                                                                                            CssClass="Etichetta" Text="a" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadNumericTextBox ID="NumeroRegistroFineTextBox" runat="server" Skin="Office2007"
                                                                                            Width="60px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                                                            ShowSpinButtons="True" ToolTip="Numero del registro (fine)">
                                                                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                        </telerik:RadNumericTextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%">
                                                                    <tr style="height: 25px">
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="DataAperturaFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                Text="Data Apertura" />
                                                                        </td>
                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="DataInizioAperturaFiltroLabel" runat="server" 
                                                                                            CssClass="Etichetta" Text="da" />
                                                                                    </td>
                                                                                    <td style="width: 80px">
                                                                                        <telerik:RadDatePicker ID="DataInizioAperturaTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" 
                                                                                            ToolTip="Data apertura del fascicolo (inizio)">
                                                                                            <Calendar ID="Calendar1" runat="server">
                                                                                                <SpecialDays>
                                                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                </SpecialDays>
                                                                                            </Calendar>
                                                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                                                        </telerik:RadDatePicker>
                                                                                    </td>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="DataFineAperturaFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                            Text="a" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadDatePicker ID="DataFineAperturaTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" 
                                                                                            ToolTip="Data apertura del fascicolo (fine)">
                                                                                            <Calendar ID="Calendar2" runat="server">
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
                                                                </table>

                                                                <table style="width: 100%">
                                                                    <tr style="height: 25px">
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="DataChiusuraFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                Text="Data Chiusura" />
                                                                        </td>
                                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="DataInizioChiusuraFiltroLabel" runat="server" 
                                                                                            CssClass="Etichetta" Text="da" />
                                                                                    </td>
                                                                                    <td style="width: 80px">
                                                                                        <telerik:RadDatePicker ID="DataInizioChiusuraTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" 
                                                                                            ToolTip="Data chiusura del fascicolo (inizio)">
                                                                                            <Calendar runat="server">
                                                                                                <SpecialDays>
                                                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                                </SpecialDays>
                                                                                            </Calendar>
                                                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                                                        </telerik:RadDatePicker>
                                                                                    </td>
                                                                                    <td align="center" style="width: 40px">
                                                                                        <asp:Label ID="DataFineChiusuraFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                            Text="a" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadDatePicker ID="DataFineChiusuraTextBox" Skin="Office2007" Width="110px"
                                                                                            runat="server" MinDate="1753-01-01" 
                                                                                            ToolTip="Data chiusura del fascicolo (fine)">
                                                                                            <Calendar runat="server">
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
                                                                </table>

                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="CodiceFascicoloFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                Text="Cod. Fascicolo" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                  
                                                                                    <td style="width: 320px">
                                                                                        <telerik:RadTextBox ID="CodiceFascicoloCompletoTextBox" runat="server" Skin="Office2007"
                                                                                            Width="310px" ToolTip="Codice completo del fascicolo"  />
                                                                                    </td>
                                                                                    <td style="width: 60px; text-align: center">
                                                                                        <asp:Label ID="StatoFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                            Text="Stato" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <telerik:RadComboBox ID="StatoFascicoloFiltroComboBox" runat="server" Skin="Office2007"
                                                                                            Width="130px" EmptyMessage="- Selezionare Stato-" ItemsPerRequest="10" Filter="StartsWith"
                                                                                            MaxHeight="300px" ToolTip="Stato del fascicolo" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="TipoProcedimentoFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                Text="Tipo Procedimento" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadComboBox ID="ProcedimentoFiltroComboBox" runat="server" EmptyMessage="Seleziona Tipo Procedimento"
                                                                                            Filter="StartsWith" ItemsPerRequest="10" MaxHeight="150px" Skin="Office2007"
                                                                                            Width="100%" Enabled="true" ToolTip="Tipo di procedimento" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="TitolareFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                Text="Titolare" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadComboBox ID="TitololariFiltroComboBox" runat="server" Width="100%" Height="150"
                                                                                            EmptyMessage="Seleziona Titolare" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" 
                                                                                            Filter="StartsWith" Skin="Office2007"
                                                                                            LoadingMessage="Caricamento in corso..." 
                                                                                            ToolTip="Titolare del procedimento">
                                                                                            <WebServiceSettings Method="GetElementiRubrica" Path="RicercaFascicoloPage.aspx" />
                                                                                        </telerik:RadComboBox>
                                                                                    </td>
                                                                                    <td style="width: 25px; text-align: center">
                                                                                        <asp:ImageButton ID="TrovaBeneficiarioFiltroImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                            ToolTip="Seleziona Titolare ..." ImageAlign="AbsMiddle" />
                                                                                        <asp:ImageButton ID="AggiornaBeneficiarioFiltroImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                            Style="display: none" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="OggettoFiltroLabel" runat="server" CssClass="Etichetta" 
                                                                                Text="Oggetto" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="OggettoFiltroTextBox" runat="server" Skin="Office2007" 
                                                                                            Width="100%" ToolTip="Oggetto del procedimento" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="ClassificazioneFiltroLabel" runat="server" CssClass="Etichetta" Text="Classificazione"
                                                                                AssociatedControlID="TrovaClassificazioneImageButton" AccessKey="T" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="ClassificazioneFiltroTextBox" runat="server" Skin="Office2007"
                                                                                            Width="100%" Enabled="False" 
                                                                                            ToolTip="Indice di classificazione completo" />
                                                                                    </td>
                                                                                    <td style="width: 1px; text-align: center">
                                                                                        <asp:ImageButton ID="AggiornaClassificazioneFiltroImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                            Style="display: none" />
                                                                                        <telerik:RadTextBox ID="IdClassificazioneFiltroTextBox" runat="server" Skin="Office2007"
                                                                                            Width="1px" Style="display: none" />
                                                                                    </td>
                                                                                    <td style="width: 30px; text-align: center">
                                                                                        <asp:ImageButton ID="TrovaClassificazioneFiltroImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                            ToolTip="Seleziona titolario classificazione (ALT + T) ..." />&nbsp;
                                                                                    </td>
                                                                                    <td style="width: 25px; text-align: center">
                                                                                        <asp:ImageButton ID="EliminaClassificazioneFiltroImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                            ToolTip="Cancella titolario classificazione " />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 130px">
                                                                            <asp:Label ID="ResponsabileFiltroLabel" runat="server" CssClass="Etichetta" Text="Responsabile"
                                                                                AssociatedControlID="TrovaResponsabileFiltroImageButton" AccessKey="S" />
                                                                        </td>
                                                                        <td>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadTextBox ID="ResponsabileFiltroTextBox" runat="server" 
                                                                                            Skin="Office2007" Width="100%"
                                                                                            Enabled="False" ToolTip="Nominativo del responsabile del procedimento" />
                                                                                    </td>
                                                                                    <td style="width: 1px; text-align: center">
                                                                                      <asp:ImageButton ID="AggiornaResponsabileFiltroImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                            Style="display: none" />
                                                                                        <telerik:RadTextBox ID="IdResponsabileFiltroTextBox" runat="server" Skin="Office2007" Width="0px"
                                                                                            Style="display: none" />
                                                                                    </td>
                                                                                    <td style="width: 30px; text-align: center">
                                                                                        <asp:ImageButton ID="TrovaResponsabileFiltroImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                            ToolTip="Seleziona responsabile (ALT + S)..." />
                                                                                    </td>
                                                                                    <td style="width: 25px; text-align: center">
                                                                                        <asp:ImageButton ID="EliminaResponsabileFiltroImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                            ToolTip="Cancella responsabile" />
                                                                                      
                                                                                    </td>
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
                                <tr>
                                    <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                        <telerik:RadButton ID="AvantiImageButton" runat="server" Text="Avanti" Width="100px"
                                            Skin="Office2007" ToolTip="Vai ai risultati">
                                            <Icon PrimaryIconUrl="../../../../images/forward.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>
                                        &nbsp;&nbsp;
                                        <telerik:RadButton ID="CercaButton" runat="server" Text="Cerca" Width="100px" Skin="Office2007"
                                            ToolTip="Effettua la ricerca con i filtri impostati">
                                            <Icon PrimaryIconUrl="../../../../images/Ricerca.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>
                                        &nbsp;&nbsp;
                                        <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="100px"
                                            Skin="Office2007" ToolTip="Annulla i filtri impostati">
                                            <Icon PrimaryIconUrl="../../../../images/Annulla.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>


            <asp:Panel ID="RisultatiPanel" runat="server">

                <table width="900px" cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td>
                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                        &nbsp;<asp:Label ID="ElencoRegistrazioniLabel" runat="server" 
                                            Style="color: #00156E" Font-Bold="True"
                                            Text="Elenco Fascicoli" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ContainerMargin">
                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                           <tr style="background-color: #DFE8F6">
                                                <td>
                                                    <div style="overflow: auto; height:515px; width: 100%; background-color: #FFFFFF;
                                                        border: 0px solid #5D8CC9;">


                                                             <telerik:RadGrid ID="FascicoliGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" PageSize ="5"
                                            Culture="it-IT">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column" HeaderText="Id" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                   

                                                   <%--<telerik:GridTemplateColumn SortExpression="NumeroRegistro" UniqueName="NumeroRegistro"
                                                        HeaderText="N." DataField="NumeroRegistro" HeaderStyle-Width="45px" ItemStyle-Width="45px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("NumeroRegistro")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 45px; border: 0px solid red">
                                                                <%# Eval("NumeroRegistro")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>--%>


                                                    <telerik:GridTemplateColumn SortExpression="IdentificativoFascicolo" UniqueName="IdentificativoFascicolo"
                                                        HeaderText="Cod. Fascicolo" DataField="CodiceFascicolo" HeaderStyle-Width="145px" ItemStyle-Width="145px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("IdentificativoFascicolo")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 145px; border: 0px solid red">
                                                                <%# Eval("IdentificativoFascicolo")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn SortExpression="DescrizioneProcedimento" UniqueName="DescrizioneProcedimento" HeaderText="Tipo Procedimento"
                                                        DataField="DescrizioneProcedimento" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DescrizioneProcedimento")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 180px; border: 0px solid red">
                                                                <%# Eval("DescrizioneProcedimento")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                     <telerik:GridTemplateColumn SortExpression="Oggetto" UniqueName="Oggetto"
                                                        HeaderText="Oggetto" DataField="Oggetto" >
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("Oggetto")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 230px; border: 0px solid red">
                                                                <%# Eval("Oggetto")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn SortExpression="DataApertura" UniqueName="DataApertura"
                                                        HeaderText="Apertura" DataField="DataApertura" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DataApertura","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                <%# Eval("DataApertura", "{0:dd/MM/yyyy}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                     <telerik:GridTemplateColumn SortExpression="DataChiusura" UniqueName="DataChiusura"
                                                        HeaderText="Chiusura" DataField="DataChiusura" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DataChiusura","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                overflow: hidden; text-overflow: ellipsis; width: 70px;">
                                                                <%# Eval("DataChiusura", "{0:dd/MM/yyyy}")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                   
                                                

                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                        ImageUrl="~\images\checks.png" UniqueName="Select" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                     
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>

                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                        
                                         <telerik:RadButton ID="IndietroImageButton" runat="server" Text="Indietro" Width="100px"
                                                        Skin="Office2007" ToolTip="Torna al filtro">
                                                        <Icon PrimaryIconUrl="../../../../images/back.png" PrimaryIconLeft="5px" />
                                                    </telerik:RadButton>

                                                    &nbsp;&nbsp;
                                                    <telerik:RadButton ID="DettaglioImageButton" runat="server" Text="Dettagli" Width="100px"
                                                        Skin="Office2007" ToolTip="Visualizza registrazione selezionata">
                                                        <Icon PrimaryIconUrl="../../../../images/text.png" PrimaryIconLeft="5px" />
                                                    </telerik:RadButton>
                                                    &nbsp;&nbsp;

                                                    <telerik:RadButton ID="ConfermaButton" runat="server" Text="Conferma" Width="100px"   Skin="Office2007">
                                                        <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px" />
                                                    </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>



           
            </asp:Panel>

            <asp:Panel ID="DettaglioPanel" runat="server">
                <table width="900px" cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td>
                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                        &nbsp;<asp:Label ID="TitoloDettaglioRegistrazione" runat="server" Style="color: #00156E"
                                            Font-Bold="True" Text="Dettaglio Fascicolo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ContainerMargin">
                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                            <tr style="background-color: #DFE8F6">
                                                <td valign="top">
                                                    <div style="overflow: auto; height: 515px; width: 100%; background-color: #FFFFFF;
                                                        border: 0px solid #5D8CC9;">
                                                     
                                                                   
                                                                     <table style="width: 100%; border: 1px solid #5D8CC9">
                                                                        <tr>
                                                                            <td>
                                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                                    <tr>
                                                                                        <td style=" width:600px">
                                                                                            &nbsp; &nbsp;<asp:Label ID="AreaInfoLabel" runat="server" Font-Bold="True" Style="width: 570px;
                                                                                                color: #00156E; background-color: #BFDBFF" Text="" CssClass="Etichetta" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <%-- INIZIO  NAVIGAZIONE--%>
                                                                                            <table style=" width:100%">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="PrimoImageButton" runat="server" ImageUrl="~/images//first.png"
                                                                                                            ToolTip="Sposta in prima posizione" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="PrecedenteImageButton" runat="server" ImageUrl="~/images//Previous.png"
                                                                                                            ToolTip="Sposta indietro" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Image ID="NavigatorSeparator1" runat="server" ImageUrl="~/images//NavigatorSeparator.png" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <telerik:RadTextBox ID="PositionItemTextBox" runat="server" Skin="Office2007" Width="50px"
                                                                                                            ToolTip="Posizione corrente" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="CountItemLabel" runat="server" CssClass="Etichetta" Text="di {0}"
                                                                                                            Width="60px" ToolTip="Numero totale di elementi" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="VaiImageButton" runat="server" ImageUrl="~/images//Goto.png"
                                                                                                            ToolTip="Vai a" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Image ID="NavigatorSeparator2" runat="server" ImageUrl="~/images//NavigatorSeparator.png" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="SuccessivoImageButton" runat="server" ImageUrl="~/images//Next.png"
                                                                                                            ToolTip="Sposta avanti" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="UltimoImageButton" runat="server" ImageUrl="~/images//Last.png"
                                                                                                            ToolTip="Sposta in ultima posizione" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                            <%-- FINE  NAVIGAZIONE--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>


                                                                             <%--DETTAGLIO--%>

                                                                                <div id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td style="width: 100px">
                                                                                                <%-- <asp:Label ID="NumeroLabel" runat="server" CssClass="Etichetta" Text="N. Registro *"
                                                ForeColor="#FF8040" ToolTip="Numero di Registro" />--%>
                                                                                                <asp:Label ID="TipoFascicoloLabel" runat="server" CssClass="Etichetta" Text="Tipo Fascicolo *"
                                                                                                    ForeColor="#FF8040" ToolTip="Tipo Fascicolo" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 200px;">
                                                                                                            <telerik:RadComboBox ID="TipologiaFascicoloComboBox" runat="server" EmptyMessage="Seleziona Tipo Fascicolo"
                                                                                                                Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                                                                Width="200px" Enabled="true" ToolTip="Tipo di fascicolo" />
                                                                                                        </td>
                                                                                                        <%-- <td style="width: 80px;">

                                                        <telerik:RadNumericTextBox ID="NumeroRegistroTextBox" runat="server" Skin="Office2007"
                                                            Width="60px" DataType="System.Int32" MaxLength="7" MaxValue="9999999" MinValue="1"
                                                            ShowSpinButtons="True" ToolTip="Numero Registro">
                                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                        </telerik:RadNumericTextBox>

                                                    </td>
                                                    <td style="width: 30px">
                                                   
                                                    <div id="ZoneID1">
                                                        <asp:CheckBox ID="GeneraRegistroCheckBox" runat="server" Text="&nbsp;" ToolTip="Selezionando questa casella il numero di registro verrà calcolato dal sistema"  />
                                                       
                                                        </div>

                                                    </td>
                                                    <td style="width: 110px; text-align:left">
                                                         <asp:Label ID="GeneraRegistroLabel" runat="server" CssClass="Etichetta" Text="Genera Registro" />
                                                    </td>--%>
                                                                                                        <td style="width: 120px; text-align: center">
                                                                                                            <asp:Label ID="CodiceFascicoloLabel" runat="server" CssClass="Etichetta" Text="Cod. Fascicolo *"
                                                                                                                ForeColor="#FF8040" ToolTip="Codice del Fascicolo" />
                                                                                                        </td>
                                                                                                        <td style="width: 150px">
                                                                                                            <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                                                border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 19px">
                                                                                                                <asp:Label ID="CodiceFascicoloSistemaTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                                                    runat="server" Width="140px" ToolTip="Parte del codice del fascicolo impostato automaticamente dal sistema">&nbsp;</asp:Label>
                                                                                                            </span>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <telerik:RadTextBox ID="CodiceFascicoloUtenteTextBox" runat="server" Skin="Office2007"
                                                                                                                Width="280px" 
                                                                                                                ToolTip="Parte del codice del fascicolo che può essere inserito dall'utente">
                                                                                                            </telerik:RadTextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td style="width: 100px">
                                                                                                <asp:Label ID="DataLabel" runat="server" CssClass="Etichetta" Text="Apertura *" ForeColor="#FF8040" />
                                                                                            </td>
                                                                                            <td style="width: 110px">
                                                                                                <telerik:RadDatePicker ID="DataTextBox" runat="server" MinDate="1753-01-01" Skin="Office2007"
                                                                                                    Width="100px" ToolTip="Data di apertura del fascicolo">
                                                                                                    <Calendar ID="Calendar3" runat="server">
                                                                                                        <SpecialDays>
                                                                                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday">
                                                                                                            </telerik:RadCalendarDay>
                                                                                                        </SpecialDays>
                                                                                                    </Calendar>
                                                                                                    <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                </telerik:RadDatePicker>
                                                                                                <td style="width: 70px; text-align: center">
                                                                                                    <asp:Label ID="Label4" runat="server" CssClass="Etichetta" Text="Chiusura" ForeColor="#FF8040" />
                                                                                                </td>
                                                                                                <td style="width: 110px">
                                                                                                    <telerik:RadDatePicker ID="DataChiusuraTextBox" runat="server" MinDate="1753-01-01"
                                                                                                        Skin="Office2007" Width="100px" ToolTip="Data di chiusura del fascicolo">
                                                                                                        <Calendar ID="Calendar4" runat="server">
                                                                                                            <SpecialDays>
                                                                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday">
                                                                                                                </telerik:RadCalendarDay>
                                                                                                            </SpecialDays>
                                                                                                        </Calendar>
                                                                                                        <DatePopupButton ToolTip="Apri il calendario." />
                                                                                                    </telerik:RadDatePicker>
                                                                                                </td>
                                                                                                <td style="width: 110px">
                                                                                                    <asp:Label ID="ClassificazioneLabel" runat="server" CssClass="Etichetta" Text="Classificazione *"
                                                                                                        ForeColor="#FF8040" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 320px">
                                                                                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 19px">
                                                                                                                    <asp:Label ID="ClassificazioneTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                                                        runat="server" Width="100%" ToolTip="Indice di classificazione completo">&nbsp;</asp:Label>
                                                                                                                </span>
                                                                                                            </td>
                                                                                                            <td style="width: 25px; text-align: center">
                                                                                                                <asp:ImageButton ID="TrovaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                    ToolTip="Seleziona classificazione..." ImageAlign="AbsMiddle" />
                                                                                                            </td>
                                                                                                            <td style="width: 25px; text-align: center">
                                                                                                                <asp:ImageButton ID="EliminaClassificazioneImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                                                    ToolTip="Cancella classificazione" ImageAlign="AbsMiddle" />
                                                                                                                <asp:ImageButton ID="AggiornaClassificazioneImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                                    Style="display: none; width: 0px" />
                                                                                                                <telerik:RadTextBox ID="IdClassificazioneTextBox" runat="server" Skin="Office2007"
                                                                                                                    Style="display: none; width: 0px" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td style="width: 100px">
                                                                                                <asp:Label ID="TipoProcedimentoLabel" runat="server" CssClass="Etichetta" Text="Tipo Proced. *"
                                                                                                    ToolTip="Tipo Procedimento" ForeColor="#FF8040" />
                                                                                            </td>
                                                                                            <td style="width: 110px">
                                                                                                <telerik:RadComboBox ID="ProcedimentoComboBox" runat="server" EmptyMessage="Seleziona Tipo Procedimento"
                                                                                                    Filter="StartsWith" ItemsPerRequest="10" MaxHeight="300px" Skin="Office2007"
                                                                                                    Width="300px" Enabled="true" ToolTip="Tipo di procedimento" />
                                                                                            </td>
                                                                                            <td style="width: 110px; text-align: center">
                                                                                                <asp:Label ID="ResponsabileLabel" runat="server" CssClass="Etichetta" Text="Responsabile *"
                                                                                                    ForeColor="#FF8040" />
                                                                                            </td>
                                                                                            <td style="width: 295px">
                                                                                                <span style="border-color: #abc1de; background: #fff; color: #000; font: 12px 'segoe ui',arial,sans-serif;
                                                                                                    border-width: 1px; border-style: solid; padding: 2px 1px 0px; width: 100%; height: 19px">
                                                                                                    <asp:Label ID="ResponsabileTextBox" Style="color: #000; font: 12px 'segoe ui',arial,sans-serif;"
                                                                                                        runat="server" Width="100%" ToolTip="Nominativo del responsabile del procedimento">&nbsp;</asp:Label>
                                                                                                </span>
                                                                                            </td>
                                                                                            <td style="width: 25px; text-align: center">
                                                                                                <asp:ImageButton ID="TrovaResponsabileImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                    ToolTip="Seleziona responsabile..." ImageAlign="AbsMiddle" />
                                                                                            </td>
                                                                                            <td style="width: 25px; text-align: center">
                                                                                                <asp:ImageButton ID="EliminaResponsabileImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                                    ToolTip="Cancella responsabile" ImageAlign="AbsMiddle" />
                                                                                                <asp:ImageButton ID="AggiornaResponsabileImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                                    Style="display: none; width: 0px" />
                                                                                                <telerik:RadTextBox ID="IdResponsabileTextBox" runat="server" Skin="Office2007" Style="display: none;
                                                                                                    width: 0px" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td style="width: 100px">
                                                                                                <asp:Label ID="OggettoLabel" runat="server" CssClass="Etichetta" Text="Oggetto *"
                                                                                                    ForeColor="#FF8040" />
                                                                                            </td>
                                                                                            <td style="width: 300px">
                                                                                                <telerik:RadTextBox ID="OggettoTextBox" runat="server" Skin="Office2007" ToolTip="Oggetto del procedimento"
                                                                                                    Width="350px" Rows="3" TextMode="MultiLine" MaxLength="1000" 
                                                                                                    Style="overflow-x: hidden" />
                                                                                            </td>
                                                                                            <td style="width: 50px; text-align: center">
                                                                                                <asp:Label ID="NoteLabel" runat="server" CssClass="Etichetta" Text="Note" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <telerik:RadTextBox ID="NoteTextBox" runat="server" Skin="Office2007" Width="100%"
                                                                                                    Rows="3" TextMode="MultiLine" MaxLength="1000" Style="overflow-x: hidden" 
                                                                                                    ToolTip="Note del fascicolo" />
                                                                                            </td>
                                                                                          
                                                                                        </tr>
                                                                                    </table>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td style="width: 100px">
                                                                                                <asp:Label ID="Label10" runat="server" CssClass="Etichetta" Text="Provv. Finale" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <telerik:RadTextBox ID="numeroProvvedimentoChiusuraTextbox" runat="server" Skin="Office2007"
                                                                                                    Width="400px" MaxLength="100" ToolTip="Provvedimento finale" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>


                                                                          
                                                                                             <telerik:RadTabStrip runat="server" ID="DatiFascicoloStrip" SelectedIndex="0" 
                                                                                    MultiPageID="DatiMultiPage" Skin="Office2007" Width="100%">
                                <Tabs>
                                    <telerik:RadTab Text="Titolari"  Selected="True" />
                                    <telerik:RadTab Text="Documenti"/>
                                    <telerik:RadTab Text="Visibilità" />
                                  
                                </Tabs>
                            </telerik:RadTabStrip>

                                                                                 <telerik:RadMultiPage runat="server" ID="DatiMultiPage" SelectedIndex="0" 
                                                                                    Height="100%" Width="100%" CssClass="multiPage" BorderStyle="Solid" 
                                                                                    BorderWidth="1px" BorderColor="#5D8CC9">
                                
                                <telerik:RadPageView runat="server" ID="TitolariPageView" CssClass="corporatePageView" Width="100%" >
                                  
                                   


                                       <div id="TitolariPanel" runat="server" style="padding: 2px 2px 2px 2px; width: 99.8%;  height: 250px">
                                       <table style="width: 100%">
                                           <tr>
                                               <td  style=" vertical-align:top">
                                                   <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                       <tr style="height: 30px; background-color: #BFDBFF">
                                                           <td>
                                                               <table style="width: 100%; display:none">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="TitolareLabel" runat="server" CssClass="Etichetta" Text="Titolare" ForeColor="#FF8040" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="BeneficiarioComboBox" runat="server" Width="290px" Height="150"
                                                                                EmptyMessage="Seleziona Titolare" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007"
                                                                                LoadingMessage="Caricamento in corso...">
                                                                              <%--  <WebServiceSettings Method="GetElementiRubrica" Path="FascicoliPage.aspx" />--%>
                                                                            </telerik:RadComboBox>
                                                                            &nbsp;&nbsp;<asp:ImageButton ID="TrovaBeneficiarioImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                ToolTip="Seleziona Titolare ..." ImageAlign="AbsMiddle" />&nbsp;&nbsp;<asp:ImageButton
                                                                                    ID="AggiornaBeneficiarioImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                    Style="display: none" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label3" runat="server" CssClass="Etichetta" Text="R. Legale" ToolTip="Rappresentante Legale" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="LegaleComboBox" runat="server" Width="285px" Height="150"
                                                                                EmptyMessage="Seleziona Rappresentante" EnableAutomaticLoadOnDemand="True" ItemsPerRequest="10"
                                                                                ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" Skin="Office2007"
                                                                                LoadingMessage="Caricamento in corso...">
                                                                                <WebServiceSettings Method="GetElementiRubrica" Path="FascicoliPage.aspx" />
                                                                            </telerik:RadComboBox>
                                                                            &nbsp;&nbsp;<asp:ImageButton ID="TrovaLegaleImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                ToolTip="Seleziona Rappresentante Legale ..." ImageAlign="AbsMiddle" />&nbsp;&nbsp;<asp:ImageButton
                                                                                    ID="AggiornaLegaleImageButton" runat="server" ImageUrl="~/images/knob-search16.png"
                                                                                    Style="display: none" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="addTitolariCommandButton" runat="server" BorderStyle="None"
                                                                                ImageAlign="AbsMiddle" ImageUrl="~/images/add16.png" TabIndex="43" ToolTip="Aggiungi Titolare" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                           </td>
                                                       </tr>
                                                       <tr>
                                                           <td>
                                                               <div id="DivTitolari" runat="server" style="overflow: auto; height: 205px; border: 1px solid #5D8CC9;
                                                                   background-color: White">
                                                                 <telerik:RadGrid ID="TitolariGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                        CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" PageSize="6"
                                                                        Culture="it-IT">
                                                                        <MasterTableView DataKeyNames="idTitolareFascicolo,idStrutturaEsternaTitolare">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="idTitolareFascicolo" DataType="System.Int32"
                                                                                    FilterControlAltText="Filter Id column" HeaderText="idTitolareFascicolo" ReadOnly="True"
                                                                                    SortExpression="idTitolareFascicolo" UniqueName="idTitolareFascicolo" Visible="False" />
                                                                               
                                                                                <telerik:GridTemplateColumn SortExpression="denominazioneTitolare" UniqueName="denominazioneTitolare"
                                                                                    HeaderText="Titolare" DataField="denominazioneTitolare">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("denominazioneTitolare")%>' style="white-space: nowrap; overflow: hidden;
                                                                                            text-overflow: ellipsis; width: 405px; border:0px solid red">
                                                                                            <%# Eval("denominazioneTitolare")%></div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn SortExpression="denominazioneRappresentanteLegale" UniqueName="denominazioneRappresentanteLegale"
                                                                                    HeaderText="Rappresentante Legale" DataField="denominazioneRappresentanteLegale"
                                                                                    HeaderStyle-Width="400px" ItemStyle-Width="400px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("denominazioneRappresentanteLegale")%>' style="white-space: nowrap;
                                                                                            overflow: hidden; text-overflow: ellipsis; width: 400px;">
                                                                                            <%# Eval("denominazioneRappresentanteLegale")%></div>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                    HeaderStyle-Width="20px" ItemStyle-Width="20px" ImageUrl="~\images\Delete16.png"
                                                                                    UniqueName="Delete" ConfirmText="Sei sicuro di voler eliminare il Titolare selezionato?" />
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
                                   


                                                                                        
                                   </telerik:RadPageView>

                                <telerik:RadPageView runat="server" ID="DocumentiPageView" CssClass="corporatePageView" Width="100%" >


                                    <div id="DocumentiPanel" runat="server" style="padding: 2px 2px 2px 2px; width: 99.8%; height: 250px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style=" vertical-align:top">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">

                                                        <tr style="height: 30px; background-color: #BFDBFF">
                                                            <td>
                                                                <table style="width: 100%; display:none">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label Font-Bold="True" ID="Label5" runat="server" Style="color: #00156E; background-color: #BFDBFF;
                                                                                width: 50px" CssClass="Etichetta" Text="Fase" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="FaseDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                EmptyMessage="" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="200px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label Font-Bold="True" ID="Label6" runat="server" Style="color: #00156E; background-color: #BFDBFF;
                                                                                width: 50px" CssClass="Etichetta" Text="Tipo" />
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="TipoDocumentoComboBox" runat="server" Skin="Office2007"
                                                                                Width="400px" EmptyMessage="- Selezionare -" ItemsPerRequest="10" Filter="StartsWith"
                                                                                MaxHeight="400px" ToolTip="Tipologie di documenti da associare al fascicolo" />
                                                                        </td>
                                                                        <td style="width: 20px">
                                                                            <asp:ImageButton ID="TrovaDocumentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                ToolTip="Seleziona documento..." ImageAlign="AbsMiddle" 
                                                                                style="width: 16px" />
                                                                        </td>

                                                                        <td style="width: 20px">
                                                                            <asp:ImageButton ID="EliminaDocumentiImageButton" runat="server" ImageUrl="~/images//RecycleEmpty.png"
                                                                                ToolTip="Cancella documenti" ImageAlign="AbsMiddle" />
                                                                            <asp:ImageButton ID="AggiornaDocumentoImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                Style="display: none; width: 0px" />
                                                                        </td>
                                                                       
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <div id="documentiScrollPanel" runat="server" style="overflow: auto; height: 205px;
                                                                    border: 1px solid #5D8CC9; background-color: White">
                                                                  
                                                                    <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco documenti associati al fascicolo"
                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                        PageSize="6" AllowPaging="true">
                                                                        <MasterTableView DataKeyNames="Id">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                              
                                                                              
                                                                                <telerik:GridTemplateColumn SortExpression="DescrizioneTipoDocumento" UniqueName="DescrizioneTipoDocumento"
                                                                                        HeaderText="Tipo" DataField="DescrizioneTipoDocumento" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DescrizioneTipoDocumento")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 80px;">
                                                                                                <%# Eval("DescrizioneTipoDocumento")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
																					
                                                                                    <telerik:GridTemplateColumn SortExpression="DescrizioneFase" UniqueName="DescrizioneFase"
                                                                                        HeaderText="Fase" DataField="DescrizioneFase" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DescrizioneFase")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 80px;">
                                                                                                <%# Eval("DescrizioneFase")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn SortExpression="NomeDocumentoOriginale" UniqueName="NomeDocumentoOriginale"
                                                                                        HeaderText="Documento" DataField="NomeDocumentoOriginale">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("NomeDocumentoOriginale")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis;">
                                                                                                <%# Eval("NomeDocumentoOriginale")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn SortExpression="DataDocumento" UniqueName="DataDocumento"
                                                                                        HeaderText="Inserito il" DataField="DataDocumento" HeaderStyle-Width="75px"
                                                                                        ItemStyle-Width="75px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("DataDocumento","{0:dd/MM/yyyy}")%>' style="white-space: nowrap;
                                                                                                overflow: hidden; text-overflow: ellipsis; width: 75px;">
                                                                                                <%# Eval("DataDocumento", "{0:dd/MM/yyyy}")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Stato" FilterControlAltText="Filter Stato column"
                                                                                    ImageUrl="~\images\vuoto.png" UniqueName="Stato" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                                    ImageUrl="~\images\knob-search16.png" UniqueName="Preview" HeaderStyle-Width="20px"
                                                                                    ItemStyle-Width="20px" />
                                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Filter Delete column"
                                                                                    ImageUrl="~\images\Delete16.png" UniqueName="Delete" HeaderStyle-Width="20px"
                                                                                    ItemStyle-Width="20px" />
                                                                            </Columns>
                                                                        </MasterTableView></telerik:RadGrid></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>


                                </telerik:RadPageView>


                               <telerik:RadPageView runat="server" ID="VisibilitaPageView" CssClass="corporatePageView"  Width="100%">

                                   <div id="VisibilitaPanel" runat="server" style="padding: 2px 2px 2px 2px; width: 99.8%;  height: 250px">
                                       <table style="width: 100%">
                                           <tr>
                                               <td valign="top" style="width: 50%">
                                                   <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                       <tr style="height: 30px; background-color: #BFDBFF">
                                                           <td>
                                                               <table style="width: 100%">
                                                                   <tr>
                                                                       <td>
                                                                           <asp:Label ID="VisibilitaLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                               Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Visibilità" />
                                                                       </td>
                                                                       <td align="right">
                                                                           <asp:ImageButton ID="TrovaUtenteVisibilitaImageButton" runat="server" ImageUrl="~/images//user_add.png"
                                                                               ImageAlign="AbsMiddle" BorderStyle="None" ToolTip="Aggiungi Utente..." />&nbsp;<asp:ImageButton
                                                                                   ID="TrovaGruppoVisibilitaImageButton" runat="server" ImageUrl="~/images//group_add.png"
                                                                                   ToolTip="Aggiungi Gruppo..." ImageAlign="AbsMiddle" BorderStyle="None" />&nbsp;&nbsp;<asp:ImageButton
                                                                                       ID="AggiornaGruppoVisibilitaImageButton" runat="server" ImageUrl="~/images//knob-search16.png"
                                                                                       Style="display: none" /><asp:ImageButton ID="AggiornaUtenteVisibilitaImageButton"
                                                                                           runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none" />
                                                                       </td>
                                                                   </tr>
                                                               </table>
                                                           </td>
                                                       </tr>
                                                       <tr>
                                                           <td>
                                                               <div id="Div1" runat="server" style="overflow: auto; height: 205px; border: 1px solid #5D8CC9;
                                                                   background-color: White">
                                                                   <telerik:RadGrid ID="VisibilitaGridView" runat="server" ToolTip="Elenco utenti o gruppi associati al protocollo"
                                                                       AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                       AllowPaging="true" PageSize="6" Culture="it-IT">
                                                                       <MasterTableView DataKeyNames="IdEntita, TipoEntita">
                                                                           <Columns>

                                                                               <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                   HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False" />
                                                                              
                                                                               <telerik:GridTemplateColumn SortExpression="TipoEntita" UniqueName="" HeaderText="Tipologia"
                                                                                        DataField="TipoEntita" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# IIf(Eval("TipoEntita")=1, "GRUPPO", "UTENTE")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 70px; border:0px solid red">
                                                                                                <%# IIf(Eval("TipoEntita") = 1, "GRUPPO", "UTENTE")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                  

                                                                                    <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione" HeaderText="Descrizione"
                                                                                        DataField="Descrizione" >
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 710px; border:0px solid red">
                                                                                                <%# Eval("Descrizione")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                               <telerik:GridButtonColumn FilterControlAltText="Filter Delete column" ImageUrl="~/images/Delete16.png"
                                                                                   ItemStyle-Width="10px" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center"
                                                                                   ItemStyle-VerticalAlign="Middle" UniqueName="Delete" ButtonType="ImageButton"
                                                                                   CommandName="Delete" />

                                                                           </Columns>
                                                                       </MasterTableView></telerik:RadGrid></div>
                                                           </td>
                                                       </tr>
                                                   </table>
                                               </td>
                                           </tr>
                                       </table>
                                   </div>
                               </telerik:RadPageView>

                                

                                </telerik:RadMultiPage>     



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
                                    <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                        <telerik:RadButton ID="IndietroRisultatiButton" runat="server" Text="Indietro" Width="100px"
                                            Skin="Office2007" ToolTip="Torna ai risultati">
                                            <Icon PrimaryIconUrl="../../../../images/back.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>
                                        &nbsp;&nbsp;
                                        <telerik:RadButton ID="ConfermaButton2" runat="server" Text="Conferma" Width="100px"
                                            Skin="Office2007">
                                            <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

          
            </asp:Panel>
          
          
                <telerik:RadTextBox ID="CodiceClassificazioneTextBox" runat="server" Skin="Office2007"
                                                        Style="display: none; width: 0px" />
                                                          
    </center>
    
      </div>
         </ContentTemplate>
 </asp:UpdatePanel>
    </form>
</body>
</html>
