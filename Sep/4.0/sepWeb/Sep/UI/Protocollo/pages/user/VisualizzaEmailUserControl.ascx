<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VisualizzaEmailUserControl.ascx.vb"
    Inherits="VisualizzaEmailUserControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--SPDX-License-Identifier: GPL-3.0-only--%>
<script type="text/javascript">

</script>
<div id="controlContent" style="height: 100%; position: relative; background-color: #DFE8F6">
    <table width="800px" cellpadding="5" cellspacing="5" border="0">
        <tr>
            <td>
                <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                    <%--  HEADER--%>
                    <tr>
                        <td style="background-color: #BFDBFF; padding: 0px; border-bottom: 0px solid  #9ABBE8;
                            border-top: 0px solid  #9ABBE8; height: 25px">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        &nbsp;<asp:Label ID="TitoloPannelloEmailLabel" runat="server" CssClass="Etichetta"
                                            Font-Bold="True" Style="width: 700px; color: #00156E; background-color: #BFDBFF"
                                            Text="Anteprima Email" />
                                    </td>
                                    <td align="right">
                                        <div id="buttonPanel" runat="server">
                                            <img alt="Chiudi" src="../../../../images/Close.png" style="border: 0px" onclick="document.getElementById('<%= Me.ChiudiAnteprimaEmailButton.ClientID %>').click();" />
                                        </div>
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
                                        <div id="pannelloHeaderEmail" runat="server" style="padding: 2px 2px 2px 2px; overflow: auto;
                                            height: 150px; width: 100%;">
                                            <table style="width: 100%; background-color: #DFE8F6; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="OggettoEmailLabel" runat="server" CssClass="Etichetta" Style="color: #00156E;
                                                            background-color: #DFE8F6" Text="" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="DataEmailLabel" runat="server" CssClass="Etichetta" Style="color: #00156E;
                                                            background-color: #DFE8F6" Text="" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="MittenteEmailLabel" runat="server" CssClass="Etichetta" Style="color: #00156E;
                                                            background-color: #DFE8F6" Text="" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;<asp:Label ID="DestinatarioEmailLabel" runat="server" CssClass="Etichetta"
                                                            Style="color: #00156E; background-color: #DFE8F6" Text="" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="pannelloEmail" runat="server" style="overflow: auto; height: 300px; width: 770px;
                                            border: 0px solid #5D8CC9;">
                                            <asp:Literal ID="contenutoEmail" runat="server"> </asp:Literal>
                                        </div>
                                        <div id="AllegatiEmailPanel" runat="server" style="padding: 2px 0px 0px 0px;">
                                            <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                <tr>
                                                    <td style="height: 20px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 420px">
                                                                    <asp:Label ID="AllegatiEmailLabel" runat="server" CssClass="Etichetta" Font-Bold="True"
                                                                        Style="color: #00156E; background-color: #BFDBFF" Text="Allegati" />
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="width: 110px; text-align: right">
                                                                                <asp:Label ID="PosizioneTimbroLabel" runat="server" CssClass="Etichetta" Text="Posizione Timbro" />
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                                <telerik:RadComboBox ID="PosizioneTimbroComboBox" runat="server" Skin="Office2007"
                                                                                    EmptyMessage="Nessuno" ItemsPerRequest="10" Filter="StartsWith" MaxHeight="200px"
                                                                                    Width="200px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #FFFFFF">
                                                    <td>
                                                        <div style="overflow: auto; height: 115px; border: 1px solid #5D8CC9">
                                                            <telerik:RadGrid ID="AllegatiEmailGridView" runat="server" ToolTip="Elenco allegati associati all'email"
                                                                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007"
                                                                Width="99.8%" Culture="it-IT">
                                                                <MasterTableView DataKeyNames="Id, Nomefile">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn SortExpression="NomeFile" UniqueName="NomeFile" HeaderText="Nome file"
                                                                            DataField="NomeFile">
                                                                            <ItemTemplate>
                                                                                <div title='<%# Eval("NomeFile")%>' style="white-space: nowrap; overflow: hidden;
                                                                                    text-overflow: ellipsis; width: auto; border: 0px solid red">
                                                                                    <%# Eval("NomeFile")%></div>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" FilterControlAltText="Filter Preview column"
                                                                            Text="Visualizza Allegato..." ImageUrl="~\images\knob-search16.png" UniqueName="Preview"
                                                                            HeaderStyle-Width="20px" ItemStyle-Width="20px">
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
                            <telerik:RadButton ID="VisualizzaFatturaButton" runat="server" Text="Apri Fattura"
                                Width="110px" Skin="Office2007" ToolTip="Visualizza Fattura">
                                <Icon PrimaryIconUrl="../../../../images/xml_16.png" PrimaryIconLeft="3px" />
                            </telerik:RadButton>
                            &nbsp;
                            <telerik:RadButton ID="VisualizzaEmailButton" runat="server" Text="Apri/Salva" Width="100px"
                                Skin="Office2007" ToolTip="Apri/Salva Email">
                                <Icon PrimaryIconUrl="../../../../images/attachment.png" PrimaryIconLeft="5px" />
                            </telerik:RadButton>
                            &nbsp;
                            <telerik:RadButton ID="StampaEmailButton" runat="server" Text="Stampa" Width="100px"
                                Skin="Office2007" ToolTip="Stampa Email">
                                <Icon PrimaryIconUrl="../../../../images/Printer16.png" PrimaryIconLeft="5px" />
                            </telerik:RadButton>
                            &nbsp;
                            <telerik:RadButton ID="ChiudiAnteprimaEmailButton" runat="server" Text="Chiudi" Width="100px"
                                Skin="Office2007" ToolTip="Chiudi">
                                <Icon PrimaryIconUrl="../../../../images/cancel.png" PrimaryIconLeft="5px" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="IdEmailHidden" runat="server" />
    <asp:HiddenField ID="WatermarkHidden" runat="server" />
    <asp:HiddenField ID="FullNameHidden" runat="server" />
    <asp:HiddenField ID="NumeroProtocolloHidden" runat="server" />
    <asp:HiddenField ID="AnnoProtocolloHidden" runat="server" />
    <asp:HiddenField ID="infoSessioneHidden" runat="server" />
</div>
