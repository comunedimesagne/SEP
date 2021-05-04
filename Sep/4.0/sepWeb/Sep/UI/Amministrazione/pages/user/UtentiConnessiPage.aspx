<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="UtentiConnessiPage.aspx.vb" Inherits="UtentiConnessiPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Pannello" runat="server">
        <ContentTemplate>
            <center>

                <table width="700px" cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td>
                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                               
                                <%-- HEADER--%>
                                <tr>
                                    <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    &nbsp;<asp:Label ID="TitoloLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                        Text="Utenti Connessi" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="TimeLabel" Style="text-align: right; width: 45%;color: #00156E" runat="server" CssClass="Etichetta"  Font-Bold="True"
                                                        Text="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <%--BODY--%>
                                <tr>
                                    <td class="ContainerMargin">
                                        <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; height: 330px">
                                                        <telerik:RadGrid ID="UtentiConessiGridView" runat="server" AutoGenerateColumns="False"
                                                            CellSpacing="0" GridLines="None" Skin="Office2007" Width="700px" AllowSorting="True">
                                                            <MasterTableView DataKeyNames="Id">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                        HeaderText="Id" ReadOnly="True" SortExpression="Id" UniqueName="Id" Visible="False">
                                                                        <HeaderStyle Width="70px" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="70px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="Nome" FilterControlAltText="Filter Nome column"
                                                                        HeaderText="Nominativo" SortExpression="Nome" UniqueName="Nome">
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="Username" FilterControlAltText="Filter Username column"
                                                                        HeaderText="Username" SortExpression="Username" UniqueName="Username">
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" FilterControlAltText="Filter Delete column"
                                                                        UniqueName="Delete" ImageUrl="~\images\user_delete.png" CommandName="Delete">
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
                                    </td>
                                </tr>

                                <%-- FOOTER--%>
                                <tr>
                                    <td align="center" style="background-color: #BFDBFF; padding: 4px; border-bottom: 0px solid  #9ABBE8;
                                        border-top: 1px solid  #9ABBE8; height: 25px">
                                        
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>

                <asp:Timer ID="Timer1" runat="server" Interval="600">
                </asp:Timer>

            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
