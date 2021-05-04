<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="LivelliOrganigrammaPage.aspx.vb" Inherits="LivelliTitolarioPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Pannello" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <table style="width: 900px; border: 1px solid #5D8CC9">
                <tr>
                    <td>

                        <table style="width: 900px">
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

                        <br />

                        <table style="width: 900px; height: 747px">
                            <tr>
                                <td style="border: 1px solid #abc1de; width: 300px">
                                    <telerik:RadTreeView ID="LivelliOrganigrammaTreeView" runat="server" Skin="Office2007"
                                        Style="overflow: auto; height: 100%; width: 300px" />
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="LivelloLabel" runat="server" CssClass="Etichetta" Text="Livello" />
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="LivelloTextBox" runat="server" Skin="Office2007" Width="90px"
                                                    ToolTip="Livello del titolario" ReadOnly="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="DescrizioneLabel" runat="server" CssClass="Etichetta" Text="Descrizione" />
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="DescrizioneTextBox" runat="server" Skin="Office2007" Width="430px"
                                                    ToolTip="Descrizione del livello" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="IconaLabel" runat="server" CssClass="Etichetta" Text="Icona" />
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="IconaTextBox" runat="server" Skin="Office2007" Width="430px"
                                                    ToolTip="Icona del livello" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                        <br />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
