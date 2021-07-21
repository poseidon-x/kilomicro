<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="regularSusuAccountSchedule.aspx.cs" Inherits="coreERP.ln.regularSusu.analysis.regularSusuAccountSchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Normal Susu Account Analysis
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Normal Susu Account Analysis</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Report as At
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker  runat="server" CssClass="inputControl" ID="dtDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
        </div>
    </div>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <asp:Button runat="server" ID="btnShow" Text="Show Report Data" OnClick="btnShow_Click" />
        </div>
    </div> 
    <telerik:RadScheduler runat="server" ID="RadScheduler1"  
            FirstDayOfWeek="Monday" LastDayOfWeek="Saturday" Reminders-Enabled="false" SelectedView="DayView"
            RowHeight="30px" AppointmentStyleMode="Default" OnAppointmentDataBound="RadScheduler1_AppointmentDataBound"
            OverflowBehavior="Auto" DataStartField="Start" DataEndField="End" DataSubjectField="Subject" DataKeyField="ID"
          Visible="false" AllowDelete="false" AllowEdit="false" AllowInsert="false"
         CustomAttributeNames="Body" Font-Names="Calibri">
            <AdvancedForm Modal="true"></AdvancedForm>
            <AppointmentTemplate>
                <div>
                    <asp:Panel ID="RecurrencePanel" CssClass="rsAptRecurrence" runat="server" Visible="false">
                    </asp:Panel>
                    <asp:Panel ID="RecurrenceExceptionPanel" CssClass="rsAptRecurrenceException" runat="server"
                        Visible="false">
                    </asp:Panel>
                    <asp:Panel ID="ReminderPanel" CssClass="rsAptReminder" runat="server" Visible="false">
                    </asp:Panel>
                    <%# Eval("Subject") %>
                </div>
                <hr />
                <div>
                    <%# Eval("Body") %>
                </div>
            </AppointmentTemplate>
            <ResourceStyles>
                <telerik:ResourceStyleMapping Type="User" Key="1" BackColor="YellowGreen"></telerik:ResourceStyleMapping>
                <telerik:ResourceStyleMapping Type="User" Key="2" BackColor="Pink"></telerik:ResourceStyleMapping>
                <telerik:ResourceStyleMapping Type="User" Key="3" BackColor="Azure"></telerik:ResourceStyleMapping>
            </ResourceStyles>
        </telerik:RadScheduler>
</asp:Content>
