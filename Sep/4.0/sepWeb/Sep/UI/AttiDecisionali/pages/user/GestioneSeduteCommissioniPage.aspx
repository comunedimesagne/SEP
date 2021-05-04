<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="GestioneSeduteCommissioniPage.aspx.vb" Inherits="GestioneSeduteCommissioniPage" %>

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


                       <div  id="PannelloDettaglio" runat="server" style="padding: 2px 2px 2px 2px;">
                            <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 100px">
                                                    <asp:Label ID="CommissioneLabel" runat="server" CssClass="Etichetta" Text="Commissione *"
                                                        ForeColor="#FF8040" />
                                                </td>
                                                <td style="width: 360px">


                                                     <telerik:RadComboBox ToolTip="Commissioni" ID="CommissioniComboBox" runat="server" AutoPostBack="true"
                                                       Skin="Office2007" Width="350px" EmptyMessage="- Selezionare -" ItemsPerRequest="10"
                                                       Filter="StartsWith" MaxHeight="400px" NoWrap="True" />
                                                </td>

                                                  <td style="width: 100px; text-align:center">
                                                   <asp:Label ID="DataSedutaLabel" runat="server" CssClass="Etichetta" Text="Data Seduta *"
                                                        ForeColor="#FF8040" />
                                                </td>

                                                <td style="width: 130px">
                                                <telerik:RadDatePicker ID="DataSedutaTextBox" Skin="Office2007" Width="120px"
                                                                            runat="server" MinDate="1753-01-01" />
                                                </td>
                                                <td>
                                                   <telerik:RadTimePicker ID="OrarioSedutaTextBox" Skin="Office2007" Width="70px"
                                                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 100px">
                                                    <asp:Label ID="PresidenteLabel" runat="server" CssClass="Etichetta" Text="Presidente" />
                                                </td>
                                               <td style="width: 360px">
                                                    <telerik:RadTextBox ID="PresidenteTextBox" runat="server" Skin="Office2007" Width="350px"
                                                        ReadOnly="true" ToolTip="Presidente" />
                                                  
                                                </td>
                                                <td style="width: 60px; text-align:center">
                                                 <asp:Label ID="ViceLabel" runat="server" CssClass="Etichetta" Text="Vice" />
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="ViceTextBox" runat="server" Skin="Office2007" Width="350px"
                                                        ReadOnly="true" ToolTip="Vice" />
                                                   
                                                </td>
                                            </tr>
                                        </table>


                                   
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr style="height: 20px; background-color: #BFDBFF">
                                                            <td>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label Font-Bold="True" ID="TitoloElencoConsiglieriLabel" runat="server"
                                                                                Style="color: #00156E; background-color: #BFDBFF; width: 95%" CssClass="Etichetta"
                                                                                Text="Elenco Consiglieri" />
                                                                        </td>
                                                                       
                                                                        <asp:ImageButton ID="PresenzaSedutaButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none; width: 0px" />
                                                                        <asp:ImageButton ID="AssenzaSedutaButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none; width: 0px" />
                                                                        <asp:ImageButton ID="AssenzaIngiustificataSedutaButton" runat="server" ImageUrl="~/images//knob-search16.png" Style="display: none; width: 0px" />
                                                                        <telerik:RadTextBox  ID="IdConsigliereTextBox" runat="server" Skin="Office2007" Style="display: none" Width="0px" />

                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="scrollConsiglieriPanel" runat="server" style="overflow: auto; height: 250px; border: 1px solid #5D8CC9; background-color: White">
                                                                    <telerik:RadGrid ID="ConsiglieriGridView" ToolTip="Elenco consiglieri associati alla commissione"
                                                                        runat="server" AllowPaging="False" AutoGenerateColumns="False" CellSpacing="0"
                                                                        GridLines="None" Skin="Office2007"  AllowSorting="True" AllowMultiRowSelection="True"
                                                                        Culture="it-IT">
                                                                        <MasterTableView DataKeyNames="IdSeduta, IdConsigliere">
                                                                            <Columns>
                                                                               
                                                                                <%--<telerik:GridBoundColumn DataField="IdCommissione" DataType="System.Int32" FilterControlAltText="Filter IdCommissione column"
                                                                                    ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderText="IdCommissione" ReadOnly="True"
                                                                                    SortExpression="IdCommissione" UniqueName="IdCommissione" Visible="False" />
--%>

                                                                                <telerik:GridBoundColumn DataField="IdConsigliere" DataType="System.Int32" FilterControlAltText="Filter IdConsigliere column"
                                                                                    ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderText="IdConsigliere" ReadOnly="True"
                                                                                    SortExpression="IdConsigliere" UniqueName="IdConsigliere" Visible="False" />

                                                                               
                                                                                <telerik:GridTemplateColumn SortExpression="Nominativo" UniqueName="Nominativo" HeaderText="Nominativo" DataField="Nominativo" HeaderStyle-Width="700px" ItemStyle-Width="700px">
                                                                                    <ItemTemplate>
                                                                                        <div title='<%# Eval("Nominativo")%>' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; width: 700px; border: 0px solid red">
                                                                                            <%# Eval("Nominativo")%>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                               <telerik:GridTemplateColumn UniqueName="RadioButtonTemplateColumnAggiudicatario" HeaderText="Presenza/ Assenza" HeaderTooltip="Presenza/ Assenza" AllowFiltering="False" ReadOnly="true">
                                                                                    <ItemTemplate>
                                                                                            <table cellpadding="0" cellspacing="0" border="0" style="border:0; width:100%"  >
                                                                                                <tr>
                                                                                                    <td style="border:0px; padding:0px">
                                                                                                        <asp:RadioButton Checked='<%# Eval("Presenza")%>'  AutoPostBack="false" BorderWidth="0" ToolTip ="Presenza" BorderStyle ="None" id="Presenza" Text="PS"  GroupName="options" runat="server"/>
                                                                                                    </td>
                                                                                                    <td style="border:0px; padding =0px">
                                                                                                        <asp:RadioButton  Checked='<%# Eval("AssenzaGiustificata")%>'  AutoPostBack="false" BorderStyle ="None" ToolTip ="Assenza Giustificata" id="AssenzaGiustificata" Text="AG" GroupName="options" runat="server"/>
                                                                                                    </td>
                                                                                                    <td style="border:0px; padding =0px">
                                                                                                        <asp:RadioButton Checked='<%# Eval("AssenzaIngiustificata")%>' AutoPostBack="false"  BorderStyle ="None" ToolTip ="Assenza Ingiustificata" id="AssenzaIngiustificata" Text="AI" GroupName="options" runat="server"/>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center"/>
                    
                                                                                </telerik:GridTemplateColumn>

                                                                              
                                                                            </Columns>
                                                                        </MasterTableView></telerik:RadGrid>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                         
                                            </tr>
                                        </table>

                      <div  id="PannelloGriglia" runat="server" style="padding: 2px 2px 2px 2px;">
                        
                        <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9" >
                            <tr>
                                <td>
                                    <table style="width: 100%; background-color: #BFDBFF">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="TitoloElencoCommissioniLabel" runat="server" Font-Bold="True"
                                                    Style="width: 800px; color: #00156E; background-color: #BFDBFF" 
                                                    Text="Elenco Sedute Commissioni" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: auto; height: 180px; width: 100%; background-color: #FFFFFF;border: 0px solid #5D8CC9;">

                                        <telerik:RadGrid ID="SeduteCommissioniGridView" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                            AllowSorting="True" Culture="it-IT" PageSize ="5">
                                            <MasterTableView DataKeyNames="Id">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                   
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="750px" ItemStyle-Width="7500px" DataField="DescrizioneCommissione"
                                                        FilterControlAltText="Filter DescrizioneCommissione column" HeaderText="Nome"
                                                        SortExpression="DescrizioneCommissione" UniqueName="DescrizioneCommissione">
                                                        <ItemTemplate>
                                                            <div title='<%# Eval("DescrizioneCommissione")%>' style="white-space: nowrap; overflow: hidden;
                                                                text-overflow: ellipsis; width: 750px; border: 0px solid red">
                                                                <%# Eval("DescrizioneCommissione")%></div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn  SortExpression="Data" UniqueName="Data" HeaderText="Data" DataField="Data" HeaderStyle-Width="65px" ItemStyle-Width="65px">    
                                                        <ItemTemplate >   
                                                            <div  title='<%# Eval("Data","{0:dd/MM/yyyy}")%>' style=" white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:70px;" >
                                                            <%# Eval("Data", "{0:dd/MM/yyyy}")%></div>
                                                        </ItemTemplate>    
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-Width="20px"
                                                        Text="Seleziona Seduta Commissione" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                        ImageUrl="~\images\checks.png" UniqueName="Select">
                                                        <HeaderStyle Width="20px" />
                                                        <ItemStyle Width="20px" />
                                                    </telerik:GridButtonColumn>

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


            <table style="width:900px; text-align:left">
                <tr>
                    <td valign="middle">
                       <%-- <asp:Label ID="LegendaLabel" runat="server" CssClass="Etichetta" Text="Legenda :" Width="120px" Font-Bold="True" ForeColor="#FF0000" />--%>

                        <asp:Label ID="CostoOrario" runat="server" CssClass="Etichetta" Style="text-align: center" Text="Gettore Presenza (€)" Width="170px" />
                                
                        <telerik:RadNumericTextBox ID="txtCostoOrario" runat="server" ReadOnly ="true" Skin="Office2007" Width="50px" Value="0" NumberFormat-DecimalDigits="2" MinValue="0">
                        </telerik:RadNumericTextBox>

                        <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Style="text-align: center" Text="Costo Seduta (€)" Width="110px" />
                                
                        <telerik:RadNumericTextBox ID="txtCostoSeduta" BorderColor ="Red" runat="server" ReadOnly ="true" Skin="Office2007" Width="50px" Value="0" NumberFormat-DecimalDigits="2" MinValue="0">
                        </telerik:RadNumericTextBox>

                        <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Style="text-align: center" Text="Presenti" Width="70px" />
                                
                        <telerik:RadNumericTextBox ID="txtConsiglieriPresenti" BorderColor ="Blue" runat="server" ReadOnly ="true" Skin="Office2007" Width="50px" Value="0" NumberFormat-DecimalDigits="0" MinValue="0">
                        </telerik:RadNumericTextBox>

                        <asp:Label ID="Label3" runat="server" CssClass="Etichetta" Style="text-align: center" Text="Assenti Giustificati" Width="120px" />
                                
                        <telerik:RadNumericTextBox ID="txtConsiglieriAssentigiustificati" BorderColor ="orange"  runat="server" ReadOnly ="true" Skin="Office2007" Width="50px" Value="0" NumberFormat-DecimalDigits="0" MinValue="0">
                        </telerik:RadNumericTextBox>

                        <asp:Label ID="Label4" runat="server" CssClass="Etichetta" Style="text-align: center" Text="Assenti Ingiustificati" Width="130px" />
                                
                        <telerik:RadNumericTextBox ID="txtConsiglieriAssentiIngiustificati" BorderColor ="violet" runat="server" ReadOnly ="true" Skin="Office2007" Width="50px" Value="0" NumberFormat-DecimalDigits="0" MinValue="0">
                        </telerik:RadNumericTextBox>
      
                    </td>
                </tr>
            </table>
        

            </div>

             <asp:HiddenField ID="scrollPosConsiglieriHidden" runat="server" Value="0" />
              <asp:HiddenField ID="scrollPosUtentiHidden" runat="server" Value="0" />
              <asp:HiddenField ID="infoOperazioneHidden" runat="server" />
    

       </ContentTemplate>
       
   

           

    </asp:UpdatePanel>

   





         


</asp:Content>
