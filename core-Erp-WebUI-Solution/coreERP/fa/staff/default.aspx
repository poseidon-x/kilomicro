<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.ln.staff._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Search For Staff
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Search For Staff</h3>
   <table>
       <tr>
           <td rowspan="20">
               <telerik:RadTreeView ID="tree" runat="server" OnNodeExpand="tree_NodeExpand"></telerik:RadTreeView>
           </td>
           <td style="height:30px">Surname</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" Width="150"></telerik:RadTextBox></td>
           <td style="height:30px">Other Names</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" Width="150"></telerik:RadTextBox></td> 
           <td style="height:30px">Staff No.</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccNo" runat="server" Width="100"></telerik:RadTextBox></td> 
           <td style="height:30px"><telerik:RadButton ID="btnFind" runat="server" Text="Find Staff" OnClick="btnFind_Click"></telerik:RadButton></td>
       </tr>
       <tr>
           <td colspan="7">
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false">
                   <MasterTableView>
                       <Columns>
                           <telerik:GridHyperLinkColumn DataTextField="staffNo"  HeaderText="Staff No."
                               DataNavigateUrlFields="staffID" DataNavigateUrlFormatString="/fa/staff/staff.aspx?id={0}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn> 
                       </Columns>
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
   </table>    
</asp:Content>
