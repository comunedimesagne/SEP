<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SedutaPage.aspx.vb"
    Inherits="SedutaPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
 

       <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
      <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
      </telerik:RadWindowManager>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Sunset" />
    <center>

     <div id="pageContent">
    
    <center>

        <table width="800px" cellpadding="5" cellspacing="5" border="0">
            <tr>
                <td>
                    <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="100%">
                        <%--  HEADER--%>

                        <tr>

                         <td style="background-color: #BFDBFF; padding: 4px; border-bottom: 1px solid  #9ABBE8; border-top: 1px solid  #9ABBE8; height: 25px">
                        <table style=" width:100%">
                        <tr>
    <td>
                                &nbsp;<asp:Label ID="TitleLabel" 
                                    runat="server" Style="color: #00156E" Font-Bold="True"
                                    Text="Nuova Seduta" />
                                  
                        
                           
                                   
                            </td>
                            <td align="right">
                               <asp:Label ID="DataConvocazioneLabel"  runat="server" Style="color: #00156E" Font-Bold="True" Text=""   />
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
                                            <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                border: 0px solid #5D8CC9;">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 125px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="OrganoCollegioLabel" runat="server" CssClass="Etichetta" Text="Organo *" />
                                                        </td>
                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                            <telerik:RadComboBox ID="TipologieSedutaComboBox" AutoPostBack="true" runat="server"
                                                                EmptyMessage="- Seleziona Organo -" MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                        </td>
                                                        <td style="width: 125px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="SessioneLabel" runat="server" CssClass="Etichetta" Text="Sessione" />
                                                        </td>
                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                            <telerik:RadComboBox ID="TipologieConvocazioneComboBox" AutoPostBack="false" runat="server"
                                                                EmptyMessage="- Seleziona Convocazione -" MaxHeight="150px" Skin="Office2007" Width="100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="Label1" runat="server" CssClass="Etichetta" Text="1^ Convocazione *" />
                                                        </td>
                                                        <td style="padding-left: 1px; padding-right: 1px">

                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width:80px">
                                                                        <telerik:RadDatePicker ID="DataPrimaConvocazioneTextBox" Skin="Office2007" Width="120px"
                                                                            runat="server" MinDate="1753-01-01" />
                                                                    </td>
                                                                    <td>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style=" width:80px">
                                                                                    <telerik:RadTimePicker ID="OrarioPrimaConvocazioneTextBox" Skin="Office2007" Width="70px"
                                                                                        runat="server" />
                                                                                </td>
                                                                                <td style=" text-align:center">
                                                                                    <asp:RadioButton ID="DataPrimaConvocazioneRadioButton" runat="server" GroupName="Convocazione"
                                                                                        ToolTip="Seleziona la Data di 1^ convocazione" Text="&nbsp" Checked="true" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                        
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                         
                                                        
                                                         
                                                        </td>
                                                        <td style="width: 125px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="Label2" runat="server" CssClass="Etichetta" Text="2^ Convocazione" />
                                                        </td>
                                                        <td style="padding-left: 1px; padding-right: 1px">

                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width:85px">
                                                                        <telerik:RadDatePicker ID="DataSecondaConvocazioneTextBox" Skin="Office2007" Width="120px"
                                                                            runat="server" MinDate="1753-01-01" />
                                                                    </td>
                                                                    <td>
                                                                       

                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 80px">
                                                                                    <telerik:RadTimePicker ID="OrarioSecondaConvocazioneTextBox" Skin="Office2007" Width="70px"
                                                                                        runat="server" />
                                                                                </td>
                                                                                <td style="text-align: center">
                                                                                    <asp:RadioButton ID="DataSecondaConvocazioneRadioButton" runat="server" GroupName="Convocazione"
                                                                                        ToolTip="Seleziona la Data di 2^ convocazione" Text="&nbsp" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                           
                                                        
                                                          
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="CapigruppoLabel" runat="server" CssClass="Etichetta" Text="Lett. ai Capigruppo" />
                                                        </td>
                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                            <telerik:RadDatePicker ID="DataTrasmissioneCapigruppoTextBox" Skin="Office2007" Width="120px"
                                                                runat="server" MinDate="1753-01-01" />

                                                                   
                                                        </td>
                                                        <td style="width: 125px; padding-left: 1px; padding-right: 1px">
                                                            <asp:Label ID="RevisoriLabel" runat="server" CssClass="Etichetta" Text="Lett. ai Revisori" />
                                                        </td>
                                                        <td style="padding-left: 1px; padding-right: 1px">
                                                            <telerik:RadDatePicker ID="DataTrasmissioneRevisoriTextBox" Skin="Office2007" Width="120px"
                                                                runat="server" MinDate="1753-01-01" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td>
                                                <div style="overflow: auto; height: 100%; width: 100%; background-color: #FFFFFF;
                                                    border: 0px solid #5D8CC9;">

                                                    <table style="width: 100%; background-color: #BFDBFF; border: 1px solid #5D8CC9">
                                                        <tr>
                                                            <td>
                                                                <table style="width: 100%; background-color: #BFDBFF">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;<asp:Label ID="ElencoPresenzeLabel" runat="server" Font-Bold="True" Style="width: 500px;
                                                                                color: #00156E; background-color: #BFDBFF" Text="Elenco Presenze" />
                                                                        </td>
                                                                        <td align="right" style="width: 30px">
                                                                            <asp:ImageButton ID="AggiornaPresenzeImageButton" runat="server" ImageUrl="~/images//arr_recycle.png"
                                                                                ToolTip="Ricarica Presenze da Organigramma" ImageAlign="AbsMiddle" BorderStyle="None" style=" width:20px; height:20px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div style="overflow: auto; height: 350px; width: 100%; background-color: #FFFFFF; border: 0px solid #5D8CC9;">
                                                                    <table style="width: 100%; border: 1px solid #5D8CC9; height: 100%">
                                                                        <tr>
                                                                             <td valign="top">
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                       <td>
                                                    <div  style="overflow: auto; height: 340px; border: 1px solid #5D8CC9">

                                                        <telerik:RadGrid ID="PresenzeGridView" runat="server" ToolTip="Elenco presenze associate alla seduta"  AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2007" Width="99.8%" Culture="it-IT">
                                                            <MasterTableView DataKeyNames="CodiceStruttura">
                                                                <Columns>

                                                                    <telerik:GridBoundColumn DataField="CodiceStruttura" DataType="System.Int32" FilterControlAltText="Filter CodiceStruttura column"
                                                                        HeaderText="CodiceStruttura" ReadOnly="True" SortExpression="CodiceStruttura" UniqueName="CodiceStruttura"
                                                                        Visible="False" />

                                                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderText="Presente" AllowFiltering="False" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                                                        <ItemTemplate>

                                                                        <div  style="white-space: nowrap; overflow: hidden;  text-overflow: ellipsis; width: 70px; border: 0px solid red">
                                                                            <asp:CheckBox ID="PresenteCheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True"
                                                                                runat="server"></asp:CheckBox>
                                                                         </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn SortExpression="Convocato" UniqueName="Convocato"
                                                                        HeaderText="Convocato" DataField="Convocato" HeaderStyle-Width="650px"
                                                                        ItemStyle-Width="650px">
                                                                        <ItemTemplate>
                                                                            <div title='<%# Eval("Convocato")%>' style="white-space: nowrap; overflow: hidden;
                                                                                text-overflow: ellipsis; width: 650px; border: 0px solid red">
                                                                                <%# Eval("Convocato")%></div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
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
                                <telerik:RadButton ID="SalvaButton" runat="server" Text="Ok" Width="90px" Skin="Office2007"
                                    ToolTip="Salva Seduta">
                                    <Icon PrimaryIconUrl="../../../../images/Save16.png" PrimaryIconLeft="5px" />
                                </telerik:RadButton>
                                &nbsp;
                                <telerik:RadButton ID="AnnullaButton" runat="server" Text="Annulla" Width="90px"
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

 </center>
    </form>
</body>
</html>

