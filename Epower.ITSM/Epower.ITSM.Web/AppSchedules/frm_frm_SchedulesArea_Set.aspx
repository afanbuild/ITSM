<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_frm_SchedulesArea_Set.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.frm_frm_SchedulesArea_Set" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">
    function CheckDefaultControlStatus(obj)
	{
		var objTurnType = document.getElementById(obj.id.replace("ddlTypeName","PanTurnType"));
		var objSchedules = document.getElementById(obj.id.replace("ddlTypeName","PanSchedules"));
		var objTurn = document.getElementById(obj.id.replace("ddlTypeName","PanTurn"));
	
    	if(obj.value == "2")
    	{
	        objTurnType.style.display="";
	        objSchedules.style.display="none";
	        objTurn.style.display="";
		}
		else if(obj.value == "1")
		{
			objTurnType.style.display="none";
	        objSchedules.style.display="";
	        objTurn.style.display="none";
	    }
	}
	
    function CheckDefaultControlStatus2(obj)
	{
		var objTurnType = document.getElementById(obj.id.replace("ddlTypeNameNew","PanTurnTypeNew"));
		var objSchedules = document.getElementById(obj.id.replace("ddlTypeNameNew","PanSchedulesNew"));
		var objTurn = document.getElementById(obj.id.replace("ddlTypeNameNew","PanTurnNew"));
	
    	if(obj.value == "2")
    	{
	        objTurnType.style.display="";
	        objSchedules.style.display="none";
	        objTurn.style.display="";
		}
		else if(obj.value == "1")
		{
			objTurnType.style.display="none";
	        objSchedules.style.display="";
	        objTurn.style.display="none";
	    }
	}

	function CheckTurnType(obj) {
    
        //alert(obj.id);
        $.post("turnAjax.aspx", { Action: "post", TRID: obj.value },
        function (data){
            var dataObj=eval("("+data+")");//转换为json对象
            var objTurnType = document.getElementById(obj.id.replace("ddlTurnName","ddlTurnDetl"));
            //alert(objTurnType.id);
            objTurnType.options.length=0;//清空
         
            $.each(dataObj.result, function (k, v) { 
                var newOption = document.createElement("OPTION");
                newOption.text = v.FULLNAME;
                newOption.value = v.SCHEDULESID;
                objTurnType.options.add(newOption);//添加子项
            });  
        });
    }
    
    function CheckTurnType2(obj) {

        $.post("turnAjax.aspx", { Action: "post", TRID: obj.value },
        function (data){
            var dataObj=eval("("+data+")");//转换为json对象
            var objTurnTypeNew = document.getElementById(obj.id.replace("ddlTurnNameNew","ddlTurnDetlNew"));

            objTurnTypeNew.options.length=0;//清空
         
            $.each(dataObj.result, function (k, v) { 
                var newOption = document.createElement("OPTION");
                newOption.text = v.FULLNAME;
                newOption.value = v.SCHEDULESID;
                objTurnTypeNew.options.add(newOption);//添加子项
            });  
        });
    }
    
    </script>

    <table id="Table1" align="center" style="width: 98%;" cellpadding="0" class="listContent">
        <tr>
            <td align="right" class="listTitle" width="8%">
                时间周期：
            </td>
            <td class="list" align="Left">
                <asp:Label ID="lblTimeArea" runat="server"></asp:Label>
            </td>
            <td align="right" class="listTitle" colspan="2" width="30%">
                <asp:Button ID="btnView" runat="server" Text="查看排班表" CssClass="btnClass" OnClick="btnView_Click" />
                <asp:Button ID="btnGen" runat="server" Text="生成排班表" CssClass="btnClass" OnClientClick="javascript: return confirm('您确定生成排班表吗?');"
                    OnClick="btnGen_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="重新排班" CssClass="btnClass" OnClientClick="javascript: return confirm('您确定重新排班吗?');"
                    OnClick="btnDelete_Click" />
                <asp:Button ID="btnCancel2" Text="取  消" runat="server" CssClass="btnClass" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr id="tr2" runat="server">
            <td valign="top" height="30px" align="left" colspan="6" class="listContent" style="font-size: 15px; margin-top: 30px; font-weight: bold;">
                工程师已排班信息
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="5" class="listContent">
                <asp:DataGrid ID="gridSchedule" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable" OnItemDataBound="gridSchedule_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.ItemIndex + 1%>
                                <asp:HiddenField ID="hidEngineerID" Value='<%# DataBinder.Eval(Container, "DataItem.EngineerID")%>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="deptName" HeaderText="部门">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EngineerName" HeaderText="姓名">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="上班类型">
                            <ItemTemplate>
                                <%
                                    if (Status == 0 || Status == 1)
                                    { %>
                                &nbsp;<asp:DropDownList ID="ddlTypeName" runat="server" onchange="CheckDefaultControlStatus(this);"
                                    Width="95%" SelectedValue='<%# DataBinder.Eval(Container, "DataItem.CATEID") %>'>
                                    <asp:ListItem Text="固定班次" Value="1" />
                                    <asp:ListItem Text="轮班" Value="2" />
                                </asp:DropDownList>
                                <%}
                                    else
                                    { %>
                                <asp:Label ID="txtCATENAME" Text='<%# DataBinder.Eval(Container, "DataItem.CATENAME")%>'
                                    runat="server"></asp:Label>
                                <%} %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="8%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="轮班规则">
                            <ItemTemplate>
                                <%
                                    if (Status == 0 || Status == 1)
                                    { %>
                                <asp:Panel ID="PanTurnType" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.CATENAME")) %>'
                                    runat="server" Height="16px" Width="100px">
                                    &nbsp;
                                    
                                    <asp:DropDownList ID="ddlTurnName" runat="server" onchange="CheckTurnType(this);"
                                        Width="95%" />
                                </asp:Panel>
                                <%}
                                    else
                                    { %>
                                <asp:Label ID="txtTurnName" Text='<%# DataBinder.Eval(Container, "DataItem.turnname")%>'
                                    runat="server"></asp:Label>
                                <%} %>
                                <asp:HiddenField ID="hidTurnID" Value='<%# DataBinder.Eval(Container, "DataItem.trid")%>'
                                    runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="8%" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="RestName" HeaderText="休息">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                 
                        <asp:BoundColumn DataField="PreSchedulesName" HeaderText="上一个班次">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="预排班次">
                            <ItemTemplate>
                                <%
                                    if (Status == 0 || Status == 1)
                                    { %>
                                <asp:Panel ID="PanSchedules" Style='<%# GetDefaulControlState2((string)DataBinder.Eval(Container, "DataItem.CATENAME")) %>'
                                    runat="server" Height="16px" Width="100px">
                                    &nbsp;<asp:DropDownList ID="ddlCurSchedulesName" runat="server" Width="95%">
                                    </asp:DropDownList>
                                </asp:Panel>
                                <asp:Panel ID="PanTurn" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.CATENAME")) %>'
                                    runat="server" Height="16px" Width="100px">
                                    &nbsp;<asp:DropDownList ID="ddlTurnDetl" runat="server" Width="95%">
                                    </asp:DropDownList>
                                </asp:Panel>
                                <%}
                                    else
                                    { %>
                                     <asp:Label ID="Label1" Text='<%# DataBinder.Eval(Container, "DataItem.CurSchedulesName")%>'
                                    runat="server"></asp:Label>
                                <asp:Label ID="txtCurSchedulesName" Text='<%# DataBinder.Eval(Container, "DataItem.SchedulesRemark")%>'
                                    runat="server"></asp:Label>
                                <%} %>
                                
                                <asp:HiddenField ID="hidCurSchedulesID" Value='<%# DataBinder.Eval(Container, "DataItem.curSchedulesId")%>'
                                    runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="8%" />
                        </asp:TemplateColumn>

                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>

        <tr id="trNewTitle" runat="server">
            <td valign="top" align="left" colspan="6" class="listContent" style="font-size: 15px; border 5px;
                font-weight: bold;">
                初始工程师排班信息
            </td>
        </tr>
        <tr id="trNewGrid" runat="server">
            <td valign="top" align="center" colspan="5" class="listContent">
                <asp:DataGrid ID="gridScheduleNew" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable" OnItemDataBound="gridScheduleNew_ItemDataBound" OnItemCommand="gridScheduleNew_ItemCommand">
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.ItemIndex + 1%>
                                <asp:HiddenField ID="hidEngineerNewID" Value='<%# DataBinder.Eval(Container, "DataItem.EngineerID")%>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="deptName" HeaderText="部门">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EngineerName" HeaderText="姓名">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="上班类型">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlTypeNameNew" runat="server" onchange="CheckDefaultControlStatus2(this);"
                                    Width="95%">
                                    <asp:ListItem Text="固定班次" Value="1" Selected="True" />
                                    <asp:ListItem Text="轮班" Value="2" />
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="8%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="轮班规则">
                            <ItemTemplate>
                                <asp:Panel ID="PanTurnTypeNew" Style="display: none" runat="server" Height="16px"
                                    Width="100px">
                                    &nbsp;<asp:DropDownList ID="ddlTurnNameNew" runat="server" onchange="CheckTurnType2(this);"
                                        Width="95%" />
                                </asp:Panel>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="8%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="预排班次">
                            <ItemTemplate>
                                <asp:Panel ID="PanSchedulesNew" runat="server" Height="16px" Width="100px">
                                    &nbsp;<asp:DropDownList ID="ddlCurSchedulesNameNew" runat="server" Width="95%">
                                    </asp:DropDownList>
                                </asp:Panel>
                                <asp:Panel ID="PanTurnNew" Style="display: none" runat="server" Height="16px" Width="100px">
                                    &nbsp;<asp:DropDownList ID="ddlTurnDetlNew" runat="server" Width="95%">
                                    </asp:DropDownList>
                                </asp:Panel>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="8%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="">
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkChangeSchedules" CommandArgument='<%#Eval("EngineerID") %>'
                                    CommandName="Add" runat="server" Text="补班"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
