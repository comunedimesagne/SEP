<%@ Page Title="" Language="VB" MasterPageFile="~/MainPage.master" AutoEventWireup="false"
    CodeFile="GestioneSedutePage.aspx.vb" Inherits="GestioneSedutePage" %>

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


        function OnClientAppointmentInserting(sender, eventArgs) {
            // var wnd = window.radopen("SedutaPage.aspx", "SedutaRadWindow");
            // wnd.set_title("Inserisci Seduta");


            var start = eventArgs.get_startTime().format("dd/MM/yyyy hh:mm")
           
            document.getElementById("<%=dataAppuntamentoHidden.ClientID%>").value = start;

            document.getElementById("<%=IdSedutaHidden.ClientID%>").value = 0;
            document.getElementById("<%=VisualizzaSedutaButton.ClientID%>").click();
            //Annullo l'azione predefinita
            eventArgs.set_cancel(true);
        }

        function OnClientAppointmentEditing(sender, eventArgs) {

            //Ottengo l'appuntamento selezionato.
            var appuntamento = eventArgs.get_appointment();

            var idSeduta = appuntamento._id;
            //var wnd = window.radopen("SedutaPage.aspx?&IdSeduta=" + idSeduta, "SedutaRadWindow");
            // wnd.set_title("Modifica Seduta");
            document.getElementById("<%=IdSedutaHidden.ClientID%>").value = idSeduta;
            document.getElementById("<%=VisualizzaSedutaButton.ClientID%>").click();
        
            //Annullo l'azione predefinita
            eventArgs.set_cancel(true);
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


    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="SedutaRadWindow" runat="server" Modal="True" Animation="Fade"
                AnimationDuration="200" Behaviors="Close" Height="600" Skin="Office2007" Width="900"
                VisibleTitlebar="True" VisibleStatusbar="False" ReloadOnShow="true" Title="Seduta">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>


    <asp:UpdatePanel ID="Pannello" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

           <div id="pageContent">
               
            
         <table style="width:100%">
         <tr>
        <td>
     
             <telerik:RadScheduler ID="SeduteScheduler" runat="server" SelectedView="MonthView"  Skin="Office2007" Culture="it-IT" Height="100%" 
              FirstDayOfWeek="Monday"
              LastDayOfWeek="Sunday" 
              ShowViewTabs="true"  RowHeight="30px"
              MonthView-VisibleAppointmentsPerDay="5"
             OnClientAppointmentInserting="OnClientAppointmentInserting"
             OnClientAppointmentEditing="OnClientAppointmentEditing">
             <MonthView AdaptiveRowHeight="false" />
                 <Localization HeaderDay="Giorno" HeaderMonth="Mese" HeaderToday="oggi" HeaderWeek="Settimana"
                     ShowMore="altro..." ConfirmDeleteTitle="Conferma"
                     ConfirmDeleteText="Eliminare la seduta selezionata?" ConfirmCancel="Annulla"
                     AllDay="Tutto il Giorno" ContextMenuAddAppointment="Aggiungi" ContextMenuDelete="Cancella"
                     ContextMenuEdit="Modifica" ContextMenuGoToToday="Vai al giorno corrente" />
             </telerik:RadScheduler>
   </td>
 </tr>

         </table>


        

            </div>

             
                <asp:ImageButton ID="AggiornaPaginaButton" runat="server" ImageUrl="~/images//knob-search16.png"  style="display: none; width:0px" />
               <asp:ImageButton ID="VisualizzaSedutaButton" runat="server" ImageUrl="~/images//knob-search16.png"  style="display: none; width:0px" />
               <asp:HiddenField ID="IdSedutaHidden" runat="server" />
                <asp:HiddenField ID="dataAppuntamentoHidden" runat="server" />
         
         
         
        
          
         
      
   
       </ContentTemplate>
       
    

           

    </asp:UpdatePanel>

   






         


</asp:Content>
