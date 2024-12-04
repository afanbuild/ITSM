<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowWorkIssuesOfDay.ascx.cs" Inherits="Epower.ITSM.Web.AppSchedules.ShowWorkIssuesOfDay" %>

<br />
<asp:Label ID="lblRemark" runat="server"  ForeColor ="Red" style=" margin-top:10px;" ></asp:Label>排班表
<br />

<asp:DataGrid ID="gridIssuesOfDay" runat="server" Width="90%" AutoGenerateColumns="False"
                    CssClass="Gridtable" 
    onitemdatabound="gridIssuesOfDay_ItemDataBound" 
    onitemcommand="gridIssuesOfDay_ItemCommand" >
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                 <%# Container.ItemIndex + 1%>
                            </ItemTemplate>
                            <HeaderStyle Width="4%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="DeptName" HeaderText="部门" >
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EngineerName" HeaderText="姓名" >
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        
                        <asp:TemplateColumn HeaderText="班次">
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                               <asp:DropDownList ID="ddlSchedule" runat="server"  Width="50"></asp:DropDownList>
                                <asp:Label ID="lblSchedule" runat ="server" Text ='<%#Eval("ScheduleName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="">
                            <HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
                     
                            <ItemTemplate>
                                <span style=" margin-left:10px;"></span>
                                <asp:LinkButton ID="lnkChange" runat="server" Text="调整" 
                                    CommandArgument='<%#Container.ItemIndex %>'
                                    CommandName="Change"></asp:LinkButton>
                                                   
                               <asp:HiddenField ID="hidESID" runat="server"  Value ='<%#Eval("ESID") %>' />
                               <asp:HiddenField ID="hidScheduleName" runat="server"  Value ='<%#Eval("ScheduleName") %>' />
                
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>               