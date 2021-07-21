<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="employer.aspx.cs" Inherits="coreERP.ln.setup.employer" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Employers
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script type="text/javascript">
   var popUp;
   function PopUpShowing(sender, eventArgs)
   {
       popUp = eventArgs.get_popUp();
       var gridWidth = sender.get_element().offsetWidth;
       var gridHeight = sender.get_element().offsetHeight;
       var popUpWidth = popUp.style.width.substr(0,popUp.style.width.indexOf("px"));
       var popUpHeight = popUp.style.height.substr(0,popUp.style.height.indexOf("px"));
       popUp.style.left = ((gridWidth - popUpWidth)/2 + sender.get_element().offsetLeft).toString() + "px";
       popUp.style.top = ((gridHeight - popUpHeight)/2 + sender.get_element().offsetTop).toString() + "px";
   }
 
            function mngRequestStarted(ajaxManager, eventArgs)
            {
                if (eventArgs.EventTarget == "btnExcel" || eventArgs.EventTarget == "btnWord" || eventArgs.EventTarget == "btnPDF")
              {
                 eventArgs.EnableAjax = false;
              }
            }
            function pnlRequestStarted(ajaxPanel, eventArgs)
            {
              if(eventArgs.EventTarget == "pnlBtnExcel" || eventArgs.EventTarget == "pnlBtnWord")
              {
                 eventArgs.EnableAjax = false;
              }
            }
            function gridRequestStart(grid, eventArgs)
            {
              if((eventArgs.EventTarget.indexOf("gridBtnExcel") != -1) || (eventArgs.EventTarget.indexOf("gridBtnWord") != -1))
              {
                 eventArgs.EnableAjax = false;
              }
            }
      </script>
</telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Manage Employers</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true" AllowPaging="true" 
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnItemCreated="RadGrid1_ItemCreated"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="employerID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="employerID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px">
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource3" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="employerDepartmentID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Department" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="employerID" MasterKeyField="employerID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="employerDepartmentID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="employerDepartmentID" ReadOnly="True" 
                    SortExpression="employerDepartmentID" UniqueName="employerDepartmentID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="departmentName" DefaultInsertValue="" 
                    HeaderText="Department Name" SortExpression="departmentName" UniqueName="departmentName" 
                    ItemStyle-Width="400px">
                </telerik:GridBoundColumn>   
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected department?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="employerDirectorID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Director" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="employerID" MasterKeyField="employerID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="employerDirectorID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="employerDirectorID" ReadOnly="True" 
                    SortExpression="employerDirectorID" UniqueName="employerDirectorID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="surName" DefaultInsertValue="" 
                    HeaderText="Surname" SortExpression="surName" UniqueName="surName" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>  
                <telerik:GridBoundColumn DataField="otherNames" DefaultInsertValue="" 
                    HeaderText="Other Names" SortExpression="otherNames" UniqueName="otherNames" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>   
                <telerik:GridBoundColumn DataField="phone.phoneNo" DefaultInsertValue="" 
                    HeaderText="Phone" SortExpression="phone.phoneNo" UniqueName="phone.phoneNo" 
                    ItemStyle-Width="150px">
                </telerik:GridBoundColumn>   
                <telerik:GridBoundColumn DataField="email.emailAddress" DefaultInsertValue="" 
                    HeaderText="Email" SortExpression="email.emailAddress" UniqueName="email.emailAddress" 
                    ItemStyle-Width="150px"> 
                </telerik:GridBoundColumn>
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected director?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="employerID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="employer ID" ReadOnly="True" 
            SortExpression="employerID" UniqueName="employerID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="employerName" DefaultInsertValue="" 
            HeaderText="Employer Name" SortExpression="employerName" UniqueName="employerName"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>  
        <telerik:GridBoundColumn DataField="address.addressLine1" DefaultInsertValue="" 
            HeaderText="Address" SortExpression="address.addressLine1" UniqueName="address.addressLine1"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>  
        <telerik:GridBoundColumn DataField="address.cityTown" DefaultInsertValue="" 
            HeaderText="Location" SortExpression="address.cityTown" UniqueName="address.cityTown"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>  
        <telerik:GridBoundColumn DataField="officeNumber" DefaultInsertValue="" 
            HeaderText="Office Number" SortExpression="officeNumber" UniqueName="officeNumber" 
            ItemStyle-Width="150px">
        </telerik:GridBoundColumn> 
        <telerik:GridTemplateColumn DataField="employmentTypeID" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Employment Type" 
            SortExpression="employmentTypeID" UniqueName="employmentTypeID" ItemStyle-Width="80px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# EmploymentTypeName(DataBinder.Eval(Container.DataItem, "employmentTypeID"))%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                 <telerik:RadComboBox ID="employmentTypeID" runat="server" Height="150px" Width="200px"
                    DropDownWidth=" 355px" EmptyMessage="Select Employment Type" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""    
                    AutoPostBack="true" SelectedValue='<%# DataBinder.Eval(Container.DataItem,"employmentTypeID") %>'>                    
                </telerik:RadComboBox>
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn> 
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
        ConfirmText="Are you sure you want to delete the selected employer?">
        </telerik:GridButtonColumn>
    </Columns> 
</MasterTableView>
       <ClientSettings>
           <ClientEvents OnPopUpShowing="PopUpShowing"/>
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
    </telerik:RadGrid>
    <asp:ImageButton ID="btnExcel" Text="Export to Excel" OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" Text="Export to PDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" Text="Export to Word" OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="employers" Include="address">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="employerDirectors" Where="it.employer.employerID == @employerID"
        Include="phone,email">
        <WhereParameters>
            <asp:SessionParameter Name="employerID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="employerDepartments" Where="it.employer.employerID == @employerID"
        >
        <WhereParameters>
            <asp:SessionParameter Name="employerID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    </asp:Content>
