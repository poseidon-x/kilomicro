<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="supplier.aspx.cs" Inherits="coreERP.ln.setup.supplier" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage suppliers
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Manage suppliers</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="supplierID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="supplierID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px">
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="supplierContactID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Contact" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="supplierID" MasterKeyField="supplierID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="supplierContactID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="supplierContactID" ReadOnly="True" 
                    SortExpression="supplierContactID" UniqueName="supplierContactID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="contactName" DefaultInsertValue="" 
                    HeaderText="Contact Name" SortExpression="contactName" UniqueName="contactName" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>   
                <telerik:GridBoundColumn DataField="workPhone" DefaultInsertValue="" 
                    HeaderText="Work Phone" SortExpression="workPhone" UniqueName="workPhone" 
                    ItemStyle-Width="150px">
                </telerik:GridBoundColumn>   
                <telerik:GridBoundColumn DataField="mobilePhone" DefaultInsertValue="" 
                    HeaderText="Mobile Phone" SortExpression="mobilePhone" UniqueName="mobilePhone" 
                    ItemStyle-Width="150px">
                </telerik:GridBoundColumn>  
                <telerik:GridBoundColumn DataField="email" DefaultInsertValue="" 
                    HeaderText="Email" SortExpression="email" UniqueName="email" 
                    ItemStyle-Width="150px">
                </telerik:GridBoundColumn>   
                <telerik:GridBoundColumn DataField="department" DefaultInsertValue="" 
                    HeaderText="Department" SortExpression="department" UniqueName="department" 
                    ItemStyle-Width="150px">
                </telerik:GridBoundColumn>  
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected supplier?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="supplierID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="supplier ID" ReadOnly="True" 
            SortExpression="supplierID" UniqueName="supplierID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="supplierName" DefaultInsertValue="" 
            HeaderText="supplier Name" SortExpression="supplierName" UniqueName="supplierName"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>         
        <telerik:GridNumericColumn DataField="maximumExposure" DefaultInsertValue="" 
            HeaderText="Max. Exposure" SortExpression="maximumExposure" UniqueName="maximumExposure" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>    
        <telerik:GridBoundColumn DataField="directions" DefaultInsertValue="" 
            HeaderText="Directions" SortExpression="directions" UniqueName="directions"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>  
        <telerik:GridBoundColumn DataField="comments" DefaultInsertValue="" 
            HeaderText="Comments" SortExpression="comments" UniqueName="comments"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>  
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
        ConfirmText="Are you sure you want to delete the selected supplier?">
        </telerik:GridButtonColumn>
    </Columns> 
</MasterTableView>
       <ClientSettings>
           <ClientEvents />
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
    </telerik:RadGrid>
    <asp:ImageButton ID="btnExcel" OnClick="btnExcel_Click" runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click" runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" OnClick="btnWord_Click" runat="server" ImageUrl="~/images/word.jpg" />
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="suppliers">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="supplierContacts" Where="it.supplier.supplierID == @supplierID"
        >
        <WhereParameters>
            <asp:SessionParameter Name="supplierID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    </asp:Content>
