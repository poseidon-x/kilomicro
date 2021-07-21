<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="writeOff.aspx.cs" Inherits="coreERP.ln.loans.writeOff" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Write of Interest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Write Off Interest</h3>
    <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
            DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="200px" HighlightTemplatedItems="true"
        MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox> 
    <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px" AllowMultiRowSelection="true" >
        <MasterTableView ShowFooter="true" DataKeyNames="repaymentScheduleID" >
            <Columns>
                <telerik:GridClientSelectColumn HeaderText="Write Off?"></telerik:GridClientSelectColumn>
                <telerik:GridBoundColumn DataField="repaymentScheduleID" HeaderText="accountNumber" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn> 
                <telerik:GridTemplateColumn DataField="repaymentDate" >
                    <ItemTemplate>
                        <asp:Label ID="txtDate" runat="server"></asp:Label>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Date</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridNumericColumn DataField="principalPayment" HeaderText="Principal Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="interestPayment" HeaderText="Interest Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridCalculatedColumn Expression="principalPayment+interestPayment" HeaderText="Total Payment" DataFormatString="{0:#,###.#0}" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
                <telerik:GridTemplateColumn>
                    <ItemTemplate>
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server" Value='<%# Bind("proposedInterestWriteOff") %>'></telerik:RadNumericTextBox>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Amount  to Write Off</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn> 
                <telerik:GridNumericColumn DataField="interestWritenOff" HeaderText="Interest Written-Off" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
            </Columns> 
        </MasterTableView>
        <ClientSettings><Selecting AllowRowSelect="true"  /></ClientSettings>
    </telerik:RadGrid>
    <br />
    <telerik:RadButton runat="server" Text="Write off Selected" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Selected" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
