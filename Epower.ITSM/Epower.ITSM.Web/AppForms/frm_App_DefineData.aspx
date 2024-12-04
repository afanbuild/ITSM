<%@ Page Title="" Language="C#" MasterPageFile="~/FlowForms.Master" validateRequest="false" AutoEventWireup="true" CodeBehind="frm_App_DefineData.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_App_DefineData" %>
<%@ Register TagPrefix="uc1" TagName="UserPicker" Src="../Controls/UserPicker.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ctrdateandtime" Src="../Controls/CtrDateAndTimeV2.ascx" %>
<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ctrFlowCataDropList" Src="../Controls/ctrFlowCataDropList.ascx" %>
<%@ Register TagPrefix="uc5" TagName="CtrFlowNumeric" Src="../Controls/CtrFlowNumeric.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="../Js/calendar.js"></script>
<script type="text/javascript" language="javascript">
<!--
            function TransferValue()
            {
				//Title
				if (typeof(document.all.<%=CtrFlowFlowName.ClientID%>)!="undefined" )
                {
				    parent.header.flowInfo.Subject.value = document.all.<%=CtrFlowFlowName.ClientID%>.value;
				}
            }

             function DoUserValidate(lngActionID,strActionName)  //流程提交时执行
	        {
				TransferValue();
				
				Load_Do();
				
			    return CheckValue();
		    }
		    
		    function DoUserSaveValidate()   //暂存时执行
		    {
		        Load_Do();
		    }
		    
            //
			function CheckValue()
			{
			    return true;
			}
			//
			String.prototype.trim = function()  
			{
				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
  //-->
</script>

<table width='100%' class='listContent' align='center'>
<tr>
	<td noWrap align="left" width="9%" colSpan="1" rowSpan="1" class="listTitle">标题</td>
	<td colSpan="3" class="list">
	    <uc3:CtrFlowFormText ID="CtrFlowFlowName" runat="server" Width="90%" TextToolTip="标题" MustInput="true" />
	</td>
</tr>
<tr>
	<td noWrap align="left" class="listTitle" style="width: 10%">申请人</td>
	<td class="list" style="width: 40%"><asp:label id="labApplyName" runat="server"></asp:label></td>
	<td noWrap align="left" class="listTitle" style="width: 10%">所属部门</td>
	<td class="list" style="width: 40%"><asp:label id="labDeptName" runat="server"></asp:label></td>
</tr>
<tr>
	<td noWrap align="left" class="listTitle">开始时间</td>
	<td class="list"><asp:label id="labStartDate" runat="server"></asp:label></td>
	<td noWrap align="left" class="listTitle">结束时间</td>
	<td class="list"><asp:label id="labEndDate" runat="server"></asp:label></td>
</tr>
<tr>
    <td class="list" colspan="4">
        <asp:label id="lblContent" runat="server"></asp:label>
        <input type="hidden" id="hidContent" runat="server" />
    </td>
</tr>
</table>
<script type="text/javascript">
//加载数据
function Load_Do() {
     var content = document.all.<%=lblContent.ClientID%>.innerHTML;
     document.all.<%=hidContent.ClientID%>.value = content;
 }
</script>
</asp:Content>