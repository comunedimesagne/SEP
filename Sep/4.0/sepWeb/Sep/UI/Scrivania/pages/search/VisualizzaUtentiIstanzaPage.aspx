<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VisualizzaUtentiIstanzaPage.aspx.vb" Inherits="VisualizzaUtentiIstanzaPage" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Visualizza Utenti Istanza</title>
    <link type="text/css" href="../../../../Styles/Theme.css" rel="stylesheet" />
    <link href="../../../../Styles/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
     <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <asp:UpdatePanel  ID="Panello" runat="server">
            <ContentTemplate>                                                        
                <div id="scrollPanel" runat="server" style="overflow: auto; height: 100%; border: 1 solid #5D8CC9">
                     <table style="color: #00156E; background-color: #BFDBFF">
                        <tr>
                            <td><asp:Label ID="Lbl" runat="server" Font-Bold="true" Width="540px" ToolTip="Riferimento dell'istanza"/> </td>
                            <td align="right"><asp:ImageButton ID="imbEsportaPDF" runat="server" ImageUrl="~/images//pdf32.png" TabIndex="11" Height="32px" Width="32px" ToolTip="Esporta l'elenco in PDF" /></td>
                     </tr></table>                                                                                    					 
                     <telerik:RadGrid ID="UtentiIstanzaGridView" runat="server" AllowPaging="True" 
                         AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0"
                         ToolTip="Lista degli utenti che hanno visualizzato l'istanza" GGridLines="None" Skin="Office2007" 
                         Width="580px" TabIndex="1" Culture="it-IT">                                                                  
                         <mastertableview datakeynames="Id">
                         <CommandItemSettings ExportToPdfText="Export to PDF" />   
                         <rowindicatorcolumn filtercontrolalttext="Filter RowIndicator column"><HeaderStyle Width="20px" /></rowindicatorcolumn>
                         <expandcollapsecolumn filtercontrolalttext="Filter ExpandColumn column"><HeaderStyle Width="20px" /></expandcollapsecolumn>                                                                                                                                                                                                                       
                            <columns>
                                <telerik:GridBoundColumn DataField="Id" UniqueName="Id" DataType="System.Int32" SortExpression="Id" FilterControlAltText="Filter Id column" Visible="False"/>
                                <telerik:GridBoundColumn DataField="Utente" HeaderText="Utente" UniqueName="Utente" SortExpression="Utente" FilterControlAltText="Filter Utente column" ItemStyle-Width="400px" HeaderStyle-HorizontalAlign="Center"/>  
                                <telerik:GridBoundColumn DataField="Data" HeaderText="Data" UniqueName="Data" SortExpression="Data" FilterControlAltText="Filter Data column" HeaderStyle-HorizontalAlign="Center"  DataFormatString="{0:dd/MM/yyyy HH.mm.ss}" ItemStyle-Width="120px"/>                                                                                                                                                          
                            </Columns>                                                                     
                         </MasterTableView>
                         <PagerStyle PagerTextFormat="Cambia : {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; di &lt;strong&gt;{1}&lt;/strong&gt;, elementi da &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; di &lt;strong&gt;{5}&lt;/strong&gt;." 
                            PageSizeLabelText="Dimensione Pagina :" AlwaysVisible="True" />
                         <filtermenu enableimagesprites="False"></filtermenu>
                         <headercontextmenu cssclass="GridContextMenu GridContextMenu_Default"></headercontextmenu>
                     </telerik:RadGrid>
</div></ContentTemplate></asp:UpdatePanel></form></body></html>    