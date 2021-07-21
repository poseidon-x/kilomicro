<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="writeOffLoan.aspx.cs" Inherits="coreERP.ln.loans.writeOffLoan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Write Off Remaining Loan Balance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Write Off Remaining Loan Balance</h3>
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number" ></telerik:RadComboBox>
    <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px" AllowMultiRowSelection="true" >
        <MasterTableView ShowFooter="true" DataKeyNames="loanId" >
            <Columns>
                <telerik:GridClientSelectColumn HeaderText="Write Off?"></telerik:GridClientSelectColumn>
                <telerik:GridBoundColumn DataField="loanId" HeaderText="loanId" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn> 
                <telerik:GridTemplateColumn DataField="writeOffDate" >
                    <ItemTemplate>
                        <asp:Label ID="txtDate" runat="server"></asp:Label>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Date</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Balance" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Balance" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="penaltyBalance" HeaderText="Add Int(Penalty) Balance" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridCalculatedColumn Expression="principalBalance+interestBalance" HeaderText="Total Balance" DataFormatString="{0:#,###.#0}" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
                <telerik:GridTemplateColumn>
                    <ItemTemplate>
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server" Value='<%# Bind("proposedWriteOff") %>'></telerik:RadNumericTextBox>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Amount  to Write Off</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn> 
            </Columns> 
        </MasterTableView>
        <ClientSettings><Selecting AllowRowSelect="true"  /></ClientSettings>
    </telerik:RadGrid>
    <br />
    <telerik:RadButton runat="server" Text="Write off Selected" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Selected" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
