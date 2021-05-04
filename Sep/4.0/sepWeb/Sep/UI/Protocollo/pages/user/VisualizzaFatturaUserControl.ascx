<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VisualizzaFatturaUserControl.ascx.vb"
    Inherits="VisualizzaFatturaUserControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<script type="text/javascript">

</script>
<iframe id="ifmcontentstoprint" style="position: absolute; top: -1000px; left: -1000px;">
</iframe>
<div id="controlContent" style="height: 100%; position: relative; background-color: #DFE8F6">
    <table width="900px" cellpadding="5" cellspacing="5" border="0">
        <tr>
            <td>
                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                    <%--  HEADER--%>
                    <tr>
                        <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 1px solid  #9ABBE8;
                            border-top: 1px solid  #9ABBE8; height: 25px">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        &nbsp;<asp:Label ID="TitoloPannelloFatturaLabel" runat="server" CssClass="Etichetta"
                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                            Text="Anteprima Fattura" />
                                    </td>
                                    <td align="right">
                                        <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="document.getElementById('<%= Me.ChiudiButton.ClientID %>').click();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%-- BODY--%>
                    <tr>
                        <td class="ContainerMargin">
                            <table class="Container" cellpadding="0" cellspacing="4" width="100%" border="0">
                                <tr>
                                    <td>
                                        <div style="overflow: auto; height: 400px; width: 100%; border: 0px solid #5D8CC9;">
                                            <asp:Panel ID="pannelloFattura" runat="server" Height="400px" Style="overflow: auto;
                                                width: 870px">
                                            </asp:Panel>
                                        </div>
                                        <div id="GrigliaAllegatiPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td style="height: 20px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="DocumentiLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                        Style="width: 700px; color: #00156E; background-color: #BFDBFF" Text="Allegati" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #FFFFFF">
                                                    <td>
                                                        <div style="overflow: auto; height: 100px; border: 1px solid #5D8CC9">
                                                            <telerik:RadGrid ID="DocumentiGridView" runat="server" ToolTip="Elenco allegati associati alla fattura elettronica"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="99.8%" Culture="it-IT">
                                                                <MasterTableView DataKeyNames="Id, Nomefile" TableLayout="Fixed">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn SortExpression="Posizione" UniqueName="Posizione" HeaderText="Posizione"
                                                                            DataField="Posizione" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Posizione")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; border: 0px solid red; width: 100%">
                                                                                    <%# Eval("Posizione")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="Estremi" UniqueName="Estremi" HeaderText="Estremi"
                                                                            DataField="Estremi" HeaderStyle-Width="330px" ItemStyle-Width="330px">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("Estremi")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                    <%# Eval("Estremi")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                            DataField="NomeFile">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: 100%; border: 0px solid red">
                                                                                    <%# Eval("NomeFile")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                            Text="Visualizza Allegato..." ImageUrl="~\images\knob-search16.png" UniqueName="Preview"
                                                                            HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                        </telerik:GridButtonColumn>
                                                                    </Columns>
                                                                </MasterTableView></telerik:RadGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
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
                            <telerik:RadButton ID="VisualizzaFatturaButton" runat="server" Text="Tabellare" Width="100px"
                                Skin="Office2007" ToolTip="Imposta layout visualizzazione fattura">
                                <Icon PrimaryIconUrl="../../../../images/Table.png" PrimaryIconLeft="5px" />
                            </telerik:RadButton>
                            &nbsp; &nbsp; &nbsp;
                            <telerik:RadButton ID="SalvaFatturaButton" runat="server" Text="Apri/Salva" Width="100px"
                                Skin="Office2007" ToolTip="Apri/Salva Email">
                                <Icon PrimaryIconUrl="../../../../images/attachment.png" PrimaryIconLeft="5px" />
                            </telerik:RadButton>
                            &nbsp; &nbsp; &nbsp;
                            <telerik:RadButton ID="StampaFatturaButton" runat="server" Text="Stampa" Width="90px"
                                Skin="Office2007" ToolTip="Stampa Fattura" AutoPostBack="false">
                                <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                            </telerik:RadButton>
                            &nbsp; &nbsp; &nbsp;
                            <telerik:RadButton ID="ChiudiButton" runat="server" Text="Chiudi" Width="90px" Skin="Office2007"
                                ToolTip="Chiudi">
                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="infoSessioneHidden" runat="server" />
    <asp:HiddenField ID="FullNameHidden" runat="server" />
</div>
