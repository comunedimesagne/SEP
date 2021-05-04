<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="DownloadManualiPage.aspx.vb" Inherits="DownloadManualiPage" %>

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
              //Gestione messaggio 
              //            var dataItems = args.get_dataItems();
              //            if (dataItems['MainContent_messaggio'] != null) {
              //               alert(dataItems['MainContent_messaggio']);
              //               dataItems['MainContent_messaggio'] = null;
              //            } 
          }

          function EnableUI(state) {
              if (!state) {
                  _backgroundElement.style.display = '';
                  _backgroundElement.style.position = 'absolute';
                  _backgroundElement.style.left = '0px';
                  _backgroundElement.style.top = '0px';

                  var h = document.getElementById("lyrCorpoPagina").offsetHeight;
                  var w = document.getElementById("lyrCorpoPagina").offsetWidth;


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

   <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="200">
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 300px">
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
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
   

<asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>

          <div id="pageContent">

            <center>

              <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />

                   <div style="padding:  20px 2px 2px 2px; width: 100%">
                    <table cellpadding="0" style="width: 900px; background-color: #BFDBFF; border: 1px solid #5D8CC9;">
                        <tr>
                            <td>
                                <table style="width: 100%; background-color: #BFDBFF">
                                    <tr>
                                        <td>
                                            <asp:Label ID="TitoloLabel" Style="width: 755px; color: #00156E;" runat="server"
                                                CssClass="Etichetta" Font-Bold="True" Text="Manuali Utente" />
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr style=" background-color:#FFFFFF">
                            <td>
                                <div id="scrollPanel" runat="server" style="overflow: auto; width: auto; height:680px; background-color: #FFFFFF;  border-right: 3px solid #BFDBFF; border-left:3px solid #BFDBFF; border-bottom:3px solid #BFDBFF" >
                                    <telerik:RadGrid ID="ManualiGridView" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                            GridLines="None" Skin="Office2007" Width="99.8%" AllowMultiRowSelection="false" 
                                            AllowPaging="true" AllowFilteringByColumn="True" PageSize="20" Culture="it-IT">

                                            <MasterTableView DataKeyNames="Id,NomeFile" Width="99.8%" TableLayout="Auto">
                                                     
                                                <Columns>

                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column" HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" AllowFiltering="False" Visible="False" />
                                                                    
                                                    <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="400px"  HeaderStyle-Width="400px" 
                                                        AutoPostBackOnFilter="True" DataField="Descrizione" 
                                                        FilterControlAltText="Filter Descrizione column" 
                                                        FilterControlWidth="100%" HeaderText="Descrizione" ShowFilterIcon="False" 
                                                        SortExpression="Descrizione" UniqueName="Descrizione">
                                                        <ItemTemplate>
                                                            <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;width:400px" 
                                                                title='<%# Eval("Descrizione")%>'>
                                                                <%# Eval("Descrizione")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn AndCurrentFilterFunction="Contains" ItemStyle-Width="400px"  HeaderStyle-Width="400px" 
                                                        AutoPostBackOnFilter="True" DataField="NomeFile" 
                                                        FilterControlAltText="Filter NomeFile column" 
                                                        FilterControlWidth="100%" HeaderText="Nome File" ShowFilterIcon="False" 
                                                        SortExpression="NomeFile" UniqueName="NomeFile">
                                                        <ItemTemplate>
                                                            <div style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;width:400px" 
                                                                title='<%# Eval("NomeFile")%>'>
                                                                <%# Eval("NomeFile") %>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                                 
                                                    <telerik:GridButtonColumn Text="Visualizza il Documento"  HeaderStyle-Width="20px" ItemStyle-Width="20px" ButtonType="ImageButton" CommandName="PreviewDocumento"  FilterControlAltText="Filter Preview column" ImageUrl="~\images\knob-search16.png" UniqueName="Preview" />  
                                                                   
                                                </Columns>
                                                               
                                            </MasterTableView>
                                                          
                                        </telerik:RadGrid>
                                 
                                </div>
                            </td>
                        </tr>
                    </table>

                </div>

            </center>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>


</asp:Content>
