<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RicercaGruppoPage.aspx.vb"
    Inherits="RicercaGruppoPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ricerca Gruppo</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />

    <script type="text/javascript">
        window.focus();
    </script> 


</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />

    <center>


      <table style="width: 800px; height: 500px" cellpadding="5" cellspacing="5" border="0">
                <tr>
                     <td>
                        <table style="width: 100%; height: 100%"  class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0">
                            <%--  HEADER--%>
                            <tr>
                                <td  style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                    &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                        Text="Elenco Gruppi" CssClass="Etichetta" />
                                </td>
                            </tr>
                            <%-- BODY--%>
                            <tr>
                                <td class="ContainerMargin" valign="top">
                                    <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                        <tr>
                                           <td valign="top">
                                              
                                                <div id="scrollPanel" runat="server" style="overflow: auto; height:500px; width: 100%; background-color: #DFE8F6;
                                                    border: 0px solid #5D8CC9;">

                                                 

                                                    <telerik:RadGrid ID="GruppiGridView" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                                        GridLines="None" Skin="Office2007" Width="99.8%" AllowSorting="True" AllowMultiRowSelection="True"
                                                        AllowFilteringByColumn="True">
                                                        <MasterTableView DataKeyNames="Id">
                                                            <Columns>


                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderTooltip="Seleziona tutto"
                                                                    AllowFiltering="False" ItemStyle-Width="20px"  HeaderStyle-Width="20px">
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
                                                                    HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" AllowFiltering="False"
                                                                    ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                                                </telerik:GridBoundColumn>


                                                                <telerik:GridBoundColumn DataField="Descrizione" FilterControlAltText="Filter Descrizione column"
                                                                    HeaderText="Descrizione" SortExpression="Descrizione" UniqueName="Descrizione"
                                                                    AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" FilterControlWidth="100%"
                                                                    ShowFilterIcon="False">
                                                                 </telerik:GridBoundColumn>


                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" FilterControlAltText="Filter Select column"
                                                                    ImageUrl="~\images\checks.png" UniqueName="Select" 
                                                                    ItemStyle-Width="20px"  HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                 </telerik:GridButtonColumn>

                                                            </Columns>
                                                        </MasterTableView>
                                                          <GroupingSettings CaseSensitive="False" />
                                                    <ClientSettings EnableRowHoverStyle="true" />
                                                    </telerik:RadGrid>
                                                 
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%-- FOOTER--%>
                            <tr>
                                <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                    border-top: 1px solid  #9ABBE8; height: 25px">
                                      <telerik:RadButton ID="ConfermaButton" runat="server" Text="Conferma" Width="100px"
                                            Skin="Office2007">
                                            <Icon PrimaryIconUrl="../../../../images/checks.png" PrimaryIconLeft="5px" />
                                        </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>


    </center>

    <asp:HiddenField ID="scrollPosHidden" runat="server" Value="0" />
    </form>
</body>
</html>
