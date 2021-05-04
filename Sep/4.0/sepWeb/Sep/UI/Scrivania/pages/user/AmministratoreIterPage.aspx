<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false" CodeFile="AmministratoreIterPage.aspx.vb" Inherits="AmministratoreIterPage" %>
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

         function ModelloSelezionato() {
             var divAggModello = document.getElementById('divAggModello');
             divAggModello.style.visibility = 'visible' 
         }

         function ModelloRimosso() {
             var divAggModello = document.getElementById('divAggModello');
             divAggModello.style.visibility = 'hidden'
         }  

 </script>
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
             <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px;z-index:2000000">
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
    <asp:UpdatePanel ID="Panello" runat="server">
        <ContentTemplate>
            <div id="pageContent">                
                    <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
                    <table style="width: 900px; border: 1px solid #5D8CC9">
                        <tr>
                            <td valign="top">
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
                                                    CommandName="Elimina" Owner="RadToolBar" Enabled="false"/>
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Printer.png" Text="Stampa"
                                                    CommandName="Stampa" Owner="RadToolBar" Enabled="false"  />
                                                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Separatore1" Owner="RadToolBar" />
                                                <telerik:RadToolBarButton runat="server" ImageUrl="~/images/Home.png" Text="Home"
                                                    CommandName="Home" Owner="RadToolBar" />
                                </Items></telerik:RadToolBar></td></tr></table>
                                <table style="width: 100%; border:1 solid #9ABBE8">                                    
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione *"
                                                Width="90px" ForeColor="#FF8040" />
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ToolTip="Descrizione del modello" ID="DescrizioneTextBox" runat="server"
                                                Skin="Office2007" Width="100%" />
                                        </td>                                      
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ModuloLabel" runat="server" CssClass="Etichetta" 
                                                Text="Modulo *" Width="90px"/>
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadComboBox ID="ModuloCB" runat="server" MaxHeight="170px" Skin="Office2007" Width="300px" ToolTip="Seleziona modulo SEP" Enabled="False" AutoPostBack="true"/>
                                            <asp:Label ID="ModelloLabel" runat="server" CssClass="Etichetta" 
                                                Text="Modello *"/>
                                            <telerik:RadComboBox ID="ModelliCB" runat="server" EmptyMessage="- Seleziona Modello -"
                                                MaxHeight="150px" Skin="Office2007"
                                                Width="485px" ToolTip="Seleziona modello collegato al modulo selezionato" Enabled="False"/>
                                            <asp:Label ID="DataLabel" runat="server" CssClass="Etichetta" Text="Data" />
                                            <telerik:RadTextBox ToolTip="Data inizio validità" ID="DataTB" runat="server" Skin="Office2007" Width="65px" ReadOnly="True"/>
                                        </td>                                                                                
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="NomeFileLabel" runat="server" CssClass="Etichetta" 
                                                Text="Nome File *" Width="90px"/>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="NuovoModelloIB" ImageUrl="~/Images/new-document.png" ToolTip="Apri il Designer di WorkFlow" Enabled="false"/>
                                        </td>
                                        <td colspan="2">
                                            <div id="modelloUpload1" runat="server" style="width:100%" visible="true">
                                                 <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadAsyncUpload ID="ModelloUpload" runat="server" MaxFileInputsCount="1" OnClientFileSelected="ModelloSelezionato" Enabled="false"
                                                                 OnClientFileUploadRemoved="ModelloRimosso" Skin="Office2007" Width="100%" EnableViewState="True" InputSize="130" ToolTip="Seleziona file del modello">
                                                                 <Localization Cancel="Annulla" Remove="Elimina" Select="Sfoglia..." />
                                                            </telerik:RadAsyncUpload>
                                                        </td>
                                                        <td>
                                                            <div id="divAggModello" style="visibility:hidden;">
                                                                 <asp:ImageButton ID="AggiungiModelloIB" runat="server" ImageUrl="~/images//add16.png" ToolTip="Collega modello"
                                                                     ImageAlign="AbsMiddle" BorderStyle="None" /></div>
                                            </td></tr></table></div>
                                            <div id="modelloUpload2" runat="server" style="width:100%" visible="false">
                                                 <asp:LinkButton ID="ModelloLnkB" ForeColor="Red" CssClass="Etichetta" runat="server" ToolTip="Visualizza modello collegato"/>
                                                 <asp:Label ID="NomeFileModelloLbl" runat="server" Visible="false" />
                                                 <asp:ImageButton ID="RimuoviModelloIB" runat="server" ImageUrl="~/images//Delete16.png" ToolTip="Scollega modello"
                                                     ImageAlign="AbsMiddle" BorderStyle="None" />
                                            </div>
                                        </td>                                                                                                                      
                                    </tr>
                                </table>
                                <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">                                                              
                                   <tr>                                      
                                       <td>
                                           <table style="width: 100%; background-color: #BFDBFF">
                                             <tr>
                                                 <td>
                                                     <asp:Label ID="ElencoModelliLabel" runat="server" Font-Bold="True" Style="width: 600px;
                                                         color: #00156E; background-color: #BFDBFF" Text="Modelli" />
                                                 </td>                                                 
                                                 <td align="center" style="width:20; border-left:1 solid #5D8CC9;">
                                                     <asp:ImageButton ID="EspModXls" runat="server" ImageUrl="~/images/excel16.png"
                                                          TabIndex="20" ToolTip="Esporta in Excel i modelli visualizzati"                                                          
                                                          ImageAlign="AbsMiddle" />    
                                                 </td>
                                    </tr></table></td></tr>
                                    <tr>                                        
                                        <td>                                                  
                                            <div id="scrollPanel" runat="server" style="overflow: auto; height: 325px; width: 100%; background-color: #FFFFFF;border: 1px solid #5D8CC9">
                                                 <telerik:RadGrid ID="ModelliGridView" runat="server" AutoGenerateColumns="False"
                                                      CellSpacing="0" ToolTip="Lista modelli d'iter" GridLines="None" Skin="Office2007" AllowSorting= "true"
                                                      Width="99.8%" AllowPaging="true" PageSize="10" Culture="it-IT" AllowFilteringByColumn="false" EnableLinqExpressions ="false">
                                                      <GroupingSettings CaseSensitive="false" />                                                                 
                                                      <MasterTableView DataKeyNames="ID" Width="100%" TableLayout="Auto">
                                                          <NestedViewTemplate>
                                                               <telerik:RadGrid ID="ModelliWkfGridView" runat="server" AutoGenerateColumns="False" ToolTip="Lista dei modelli collegati al modello di Workflow"
                                                                   CellSpacing="0" GridLines="None" Skin="Office2007" Width="85%" AllowSorting="True"
                                                                   AllowPaging="True" HeaderStyle-HorizontalAlign="Center">
                                                                            <MasterTableView Width="100%" TableLayout="Fixed">                                                                            
                                                                                <Columns>
                                                                                    <telerik:GridBoundColumn DataField="ID" Visible="False"/>                                                                                                                                                                                                                                                                                                                                               
                                                                                    <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione" HeaderText="Modello collegato"
                                                                                        DataField="Descrizione">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>'  style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 350px; border: 0px solid red">
                                                                                                <%# Eval("Descrizione")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn SortExpression="Tipo" UniqueName="Tipo" HeaderText="Tipo Documento"
                                                                                        DataField="Tipo">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Replace(Eval("Tipo"), "'", "&#039;")%>'  style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                                                                                                width: 200px; border: 0px solid red">
                                                                                                <%# Eval("Tipo")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </NestedViewTemplate>
                                                          <NoRecordsTemplate><div>Nessun Modello Trovato</div></NoRecordsTemplate>
                                                          <Columns>
                                                              <telerik:GridBoundColumn DataField="ID" UniqueName="Id" Visible="False"/> 
                                                              <telerik:GridTemplateColumn HeaderStyle-Width="15px" AllowFiltering="false">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="ModuloImage" Style="display: inline-block" Width="15px" runat="Server"
                                                                                    ImageUrl='<%# Eval("ImageMod") %>' ToolTip='<%# Eval("RifModello")%>' />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>                                                                       
                                                              <telerik:GridTemplateColumn SortExpression="Descrizione" UniqueName="Descrizione"
                                                                            HeaderText="Descrizione" DataField="Descrizione">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Replace(Eval("Descrizione"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; width: 415px">
                                                                                    <%# Eval("Descrizione")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                              <telerik:GridTemplateColumn SortExpression="DataInserimento" UniqueName="DataInserimento"
                                                                            HeaderText="Del" DataField="DataInserimento" HeaderStyle-HorizontalAlign="Center"                                                                           
                                                                            AllowFiltering="false" >                                                                               
                                                                            <ItemTemplate>
                                                                                <div title='<%# Replace(Eval("UtenteInserimento"),"'", "&#039;") %>' style="white-space: nowrap;
                                                                                    overflow: hidden; width:60">
                                                                                    <%# Eval("DataInserimento", "{0:dd/MM/yyyy}")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>  
                                                              <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile"
                                                                            HeaderText="Nome File" DataField="NomeFile" HeaderStyle-HorizontalAlign="Center"                                                                          
                                                                            AllowFiltering="false">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Replace(Eval("NomeFile"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; width: 310px">
                                                                                    <%# Eval("NomeFile")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>  
                                                              <%-- <telerik:GridTemplateColumn SortExpression="Nomefilegrafico" UniqueName="Nomefilegrafico"
                                                                            HeaderText="File XPDL" DataField="Nomefilegrafico" HeaderStyle-Width="200px"                                                                          
                                                                            ShowFilterIcon="False" AllowFiltering="false">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Replace(Eval("Nomefilegrafico"), "'", "&#039;")%>' style="white-space: nowrap;
                                                                                    overflow: hidden; width: 150px">
                                                                                    <%# Eval("Nomefilegrafico")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn> --%>                                                                                                                                                                                                                                             
                                                              <telerik:GridBoundColumn DataField="IdModulo" Visible="false" />
                                                              <telerik:GridButtonColumn ImageUrl="~/images/Checks.png" Text="Seleziona il modello"
                                                                   ItemStyle-Width="20px" UniqueName="Select" ButtonType="ImageButton" CommandName="Select"/>
                                                              <telerik:GridButtonColumn ImageUrl="~/images/copy16.png" Text="Copia il modello selezionato"
                                                                   ItemStyle-Width="20px" UniqueName="Copy" ButtonType="ImageButton" CommandName="Copy"/>
                                                              <telerik:GridButtonColumn Text="Visualizza il modello nel designer" ButtonType="ImageButton"
                                                                   ImageUrl="~/images/knob-search16.png" CommandName="Preview" UniqueName="Preview" ItemStyle-Width="20px"/>                                                                     
                                                              <telerik:GridButtonColumn Text="Cancella modello" ButtonType="ImageButton" ImageUrl="~/images/Delete16.png"
                                                                   CommandName="Delete" UniqueName="Delete" ItemStyle-Width="20px"/> 
                                                              <telerik:GridButtonColumn Text="Recupera modello" ButtonType="ImageButton" ImageUrl="~/images/refresh.png"
                                                                   CommandName="Recovery" UniqueName="Recovery" ItemStyle-Width="20px"/>                                                                           
                                                          </Columns>
                                                      </MasterTableView>
                                                 </telerik:RadGrid>
                                    </div></td></tr>                               
                                </table>
                            </td>
                        </tr>
                    </table>                                                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>