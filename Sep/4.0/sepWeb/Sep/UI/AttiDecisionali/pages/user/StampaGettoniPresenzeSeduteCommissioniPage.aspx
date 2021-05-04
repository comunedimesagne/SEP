<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="StampaGettoniPresenzeSeduteCommissioniPage.aspx.vb" Inherits="StampaGettoniPresenzeSeduteCommissioniPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
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


    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
        <ProgressTemplate>
            <div id="loading" style="position: absolute; width: 100%; text-align: center; top: 180px">
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
                    <table width="900px" cellpadding="5" cellspacing="5" border="0">
                        <tr>
                            <td>
                                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">

                                    <tr>
                                        <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8;
                                            border-top: 1px solid  #9ABBE8; height: 25px">
                                            &nbsp;<asp:Label ID="TitleLabel" runat="server" Style="color: #00156E" Font-Bold="True"
                                                Text="Stampa Gettoni Presenze" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="ContainerMargin">

                                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">

                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                            border: 0px solid #5D8CC9;">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 120px">
                                                                        <asp:Label ID="DataSedutaLabel" runat="server" CssClass="Etichetta" Text="Data Seduta" />
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <asp:Label ID="DataInizioLabel" runat="server" CssClass="Etichetta" Text="da" />
                                                                    </td>
                                                                    <td style="width: 130px">
                                                                        <telerik:RadDatePicker ID="DataInizioTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                            MinDate="1753-01-01">
                                                                            <Calendar>
                                                                                <SpecialDays>
                                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                </SpecialDays>
                                                                            </Calendar>
                                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                                        </telerik:RadDatePicker>
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <asp:Label ID="DataFineLabel" runat="server" CssClass="Etichetta" Text="a" />
                                                                    </td>
                                                                    <td>
                                                                        <telerik:RadDatePicker ID="DataFineTextBox" Skin="Office2007" Width="110px" runat="server"
                                                                            MinDate="1753-01-01">
                                                                            <Calendar>
                                                                                <SpecialDays>
                                                                                    <telerik:RadCalendarDay Repeatable="Today" Date="" ItemStyle-CssClass="rcToday" />
                                                                                </SpecialDays>
                                                                            </Calendar>
                                                                            <DatePopupButton ToolTip="Apri il calendario." />
                                                                        </telerik:RadDatePicker>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <div id="PannelloGriglia" runat="server" style="padding:  3px 0px 0px 0px;">
                                                            <table style="width: 900px; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                                <tr>
                                                                    <td>
                                                                        <table style="width: 100%; background-color: #BFDBFF">
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;<asp:Label ID="TitoloElencoCommissioniLabel" runat="server" Font-Bold="True"
                                                                                        Style="width: 800px; color: #00156E; background-color: #BFDBFF" Text="Elenco Commissioni" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <%--<div style="overflow: auto; height: 230px; width: 100%; background-color: #FFFFFF; border: 0px solid #5D8CC9;">--%>
                                                                        <telerik:RadGrid ID="CommissioniGridView" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                            CellSpacing="0" GridLines="None" Skin="Office2007" AllowSorting="True" Culture="it-IT">
                                                                            <MasterTableView DataKeyNames="Id">
                                                                                <Columns>
                                                                                    <telerik:GridBoundColumn DataField="Id" DataType="System.Int32" FilterControlAltText="Filter Id column"
                                                                                        HeaderText="Id" Visible="false" SortExpression="Id" UniqueName="Id" />
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="800px" ItemStyle-Width="800px" DataField="Descrizione"
                                                                                        FilterControlAltText="Filter Descrizione column" HeaderText="Nome" SortExpression="Descrizione"
                                                                                        UniqueName="Descrizione">
                                                                                        <ItemTemplate>
                                                                                            <div title='<%# Eval("Descrizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                                text-overflow: ellipsis; width: 800px; border: 0px solid red">
                                                                                                <%# Eval("Descrizione")%></div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Stampa" HeaderStyle-Width="20px"
                                                                                        Text="Stampa" ItemStyle-Width="20px" FilterControlAltText="Filter Select column"
                                                                                        ImageUrl="~\images\Printer16.png" UniqueName="Select" />
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                        <%--</div>--%>
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
                                            <telerik:RadButton ID="StampaButton" runat="server" Text="Stampa" Width="100px" Skin="Office2007"
                                                ToolTip="Stampa gettoni presenze">
                                                <Icon PrimaryIconUrl="~/images/Printer16.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="100px"
                                                Skin="Office2007" ToolTip="Cancella i dati immessi">
                                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>

                

                </center>
            </div>

            <div style="display: none">
                <asp:Button runat="server" ID="notificaOperazioneButton" Style="width: 0px; height: 0px;
                    left: -1000px; position: absolute" />
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
