<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true" CodeBehind="frm_BR_MEETINGSCHEDULED.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_BR_MEETINGSCHEDULED" Title="无标题页" %>
<%@ Register src="../Controls/CtrFlowFormText.ascx" tagname="CtrFlowFormText" tagprefix="uc1" %>

<%@ Register src="../Controls/ctrFlowCataDropListNew.ascx" tagname="ctrFlowCataDropListNew" tagprefix="uc2" %>

<%@ Register src="../Controls/CtrFlowRemark.ascx" tagname="CtrFlowRemark" tagprefix="uc3" %>
<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc4" %>

<%@ Register src="../Controls/UserPicker.ascx" tagname="UserPicker" tagprefix="uc5" %>

<%@ Register src="../Controls/ctrdepttree.ascx" tagname="ctrdepttree" tagprefix="uc7" %>
<%@ Register src="../Controls/ctrdepttreeBranch.ascx" tagname="ctrdepttreeBranch" tagprefix="uc8" %>
<%@ Register src="../Controls/ctrdepttreeRight.ascx" tagname="ctrdepttreeRight" tagprefix="uc9" %>
<%@ Register src="../Controls/CtrdepttreeSubBank.ascx" tagname="CtrdepttreeSubBank" tagprefix="uc10" %>

<%@ Register src="../Controls/DeptPicker.ascx" tagname="DeptPicker" tagprefix="uc11" %>

<%@ Register src="../Controls/CtrFlowNumeric.ascx" tagname="CtrFlowNumeric" tagprefix="uc12" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  

   <script language="javascript" type="text/javascript">
			
            function TransferValue()
            {
               if (typeof(document.all.<%=CtrMeetingName.ClientID%>)!="undefined" )
                {
                  if(document.all.<%=CtrMeetingName.ClientID%>.value.trim()!="")
                  {
	                    parent.header.flowInfo.Subject.value = document.all.<%=CtrMeetingName.ClientID%>.value.trim();
	              }
	              else
	              {
	                 parent.header.flowInfo.Subject.value ="无标题";
	              }
	            }
            }

             function DoUserValidate(lngActionID,strActionName)
	        {
	            TransferValue();
			    return true;
		    }
		    
		   
            

			
			function SelectPDept()
			{
				if(document.all.hidDeptID.value == 1)
				{
					alert("已经是最顶层机构");
					return;
				}
				var	value=window.showModalDialog("frmpopdept.aspx?CurrDeptID=" + document.all.hidDeptID.value);
				if(value != null)
				{
					if(value.length>1)
					{
						arr=value.split("@");
						document.all.txtPDeptName.value=arr[1];
						document.all.hidPDeptID.value=arr[0];
					}
				}
			}
    
    </script>
    
    <table class="listContent" width="100%" align="center" runat="server" id="Table2"
        border="0" cellspacing="1" cellpadding="1">
        <tr>
            <td style="width: 13%" class="listTitleRight" nowrap="nowrap">
                会议名称：

            </td>
            <td class="list" style="width: 35%">
            
                <uc1:CtrFlowFormText ID="CtrMeetingName"  runat="server" MaxLength=500  MustInput="true" TextToolTip="会议名称"/>

            </td>
            
             <td style="width: 13%" class="listTitleRight" nowrap="nowrap">
                会议议题：

            </td>
            <td class="list" style="width: 35%">
                <uc1:CtrFlowFormText ID="CtrTitle"  runat="server" MaxLength=500   MustInput="true" TextToolTip="会议议题"/>
            </td>
        </tr>
        
         <tr>
            <td class="listTitleRight" nowrap="nowrap">
                会议地点：

            </td>
            <td class="list" >
             <uc1:CtrFlowFormText ID="CtrAddress"  runat="server" MaxLength=500  MustInput="true" TextToolTip="会议地点"/>
            </td>
            <td class="listTitleRight" nowrap="nowrap">
                会议室：
            </td>
            <td class="list" >
            
                <uc2:ctrFlowCataDropListNew ID="ctrdropMeetingRoom" runat="server" RootID="10972" MustInput="true" TextToolTip="会议室" />
            
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
               主办部门：

            </td>
            <td class="list" >
      
                <uc11:DeptPicker ID="CtrDepartmentName" runat="server"  MustInput="true" TextToolTip="部门"/>

            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                主持人：
            </td>
            <td class="list">

            
                <uc5:UserPicker ID="CtrHostNames" runat="server" MustInput="true" TextToolTip="主持人" />
            
            </td>
        </tr>
        
         <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                联系电话：

            </td>
            <td class="list" colspan="3" >
                <%--<uc1:CtrFlowFormText ID="CtrPhone"  runat="server"  MustInput="true" TextToolTip="联系电话"/>--%>
                
                <uc12:CtrFlowNumeric ID="CtrPhone" runat="server"  MustInput="true" TextToolTip="联系电话"/>
                
            </td>
                    
         <td class="listTitleRight" nowrap="nowrap"  visible="false">
                预定日期：

            </td>
            <td class="list" visible="false">
            

                  <uc4:ctrdateandtime ID="Ctrdatetime" runat="server" visible="false" MustInput="true" TextToolTip="开始时间"/>
            
            </td>
        </tr>
        
        <tr>
            <td class="listTitleRight" nowrap="nowrap">
                开始时间：
            </td>
            <td class="list" >
            
               <uc4:ctrdateandtime ID="ctrStartTime"  runat="server"  MustInput="true" TextToolTip="开始时间"/>
            
            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                结束时间：

            </td>
            <td class="list" >
            
           
            <uc4:ctrdateandtime ID="ctrEndTime"  runat="server"  MustInput="true" TextToolTip="结束时间"/>
            
            </td>
        </tr>
       
        <tr>
            <td class="listTitleRight" nowrap="nowrap">
                    服务需要：
            </td>
            <td class="list" colspan="3">
                <asp:CheckBoxList ID="cheService" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">茶点</asp:ListItem>
                    <asp:ListItem Value="1">摄影</asp:ListItem>
                    <asp:ListItem Value="2">水果</asp:ListItem>
                    <asp:ListItem Value="3">投影仪</asp:ListItem>
                    <asp:ListItem Value="4">网络</asp:ListItem>
                    <asp:ListItem Value="5">鲜花</asp:ListItem>
                    <asp:ListItem Value="6">指引牌</asp:ListItem>
                </asp:CheckBoxList>
                
            </td>
        </tr>
        <tr>
        <td class="listTitleRight" nowrap="nowrap">
        备注：
        </td>
        <td colspan="3" class="list" >
        
            <uc3:CtrFlowRemark ID="CtrRemarKs" runat="server" />
        
        </td>
        </tr>
        </table>
        <input id="hidDeptID" type="hidden" runat="server" />
</asp:Content>
