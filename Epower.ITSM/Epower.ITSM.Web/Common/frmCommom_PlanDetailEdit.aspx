<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="frmCommom_PlanDetailEdit.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmCommom_PlanDetailEdit"
    Title="无标题页" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
   function SubmitValidate()         //保存前数据检验
   {
        if(document.all.<%=ddltFlow.ClientID%>.value.trim() == "")
        {
            alert("执行流程不能为空！");
            return false;
        }
        if(document.all.<%=txtPlanName.ClientID%>.value.trim() == "")
        {
            alert("计划名称不能为空！");
            return false;
        }
        if(document.all.ctl00_ContentPlaceHolder1_UserPicker1_hidUserName.value == "")
        {
            alert("计划执行人不能为空！");
            return false;
        }
        return true;
   }
   String.prototype.trim = function()  //去空格

			{
				// 用正则表达式将前后空格

				// 用空字符串替代。

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
   function SelectEquItem()
			{
			    var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
		        var features =
		        'dialogWidth:800px;' +
		        'dialogHeight:500px;' +
		        'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';

		        var	value=window.showModalDialog('../EquipmentManager/frmEqu_SelectItem.aspx?pDate='+sparamvalue,"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
		        if(value != null)
                {
	                if(value.length>1)
	                {
                        document.all.<%=hidItemArrID.ClientID%>.value = value[0];   //项目ID数组
                        event.returnValue = true; 
	                }
	                else
	                {
	                    event.returnValue = false;
	                }
                }
                else
                {
                    event.returnValue = false;
                }
			}
	function SelectTimeSet()
			{
			    var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
	            
		        var	value=window.showModalDialog('../AppForms/frmPlanDetail.aspx?pDate='+sparamvalue +'&planset=' +'<%=PlanXml%>',"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=no;resizable=no") ;
		        if(value != null)
                {
	                if(value.length>1)
	                {
                        document.all.<%=hidPlanXml.ClientID%>.value = value;   //资产时间值
	                }
                }
                event.returnValue = false;
			}
	function ShowTable(imgCtrl)
    {
          var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
          var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
          var TableID = imgCtrl.id.replace("Img","Table");
          var className;
          var objectFullName;
          var tableCtrl;
          objectFullName = '<%=Table12.ClientID%>';
          className = objectFullName.substring(0,objectFullName.indexOf("Table12")-1);
          tableCtrl = document.all.item(className.substr(0,className.length)+"_"+TableID);
          if(imgCtrl.src.indexOf("icon_expandall") != -1)
          {
            tableCtrl.style.display ="";
            imgCtrl.src = ImgMinusScr ;
            var temp = document.all.<%=hidTable.ClientID%>.value;
            document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
          }
          else
          {
            tableCtrl.style.display ="none";
            imgCtrl.src = ImgPlusScr ;		
            document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id; 
          }
    }
    </script>
<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <input id="hidItemArrID" runat="server" type="hidden" />
    <input id="hidPlanXml" runat="server" type="hidden" />
    <table id="Table12" width="98%" align="center" runat="server">
        <tr>
            <td align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            基本信息设置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table2" style="width: 98%;" cellpadding="2" cellspacing="0" runat="server" class="listContent">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                执行流程<font color="#ff6666">*</font>
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddltFlow" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                计划名称<font color="#ff6666">*</font>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPlanName' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td class='listTitleRight'>
                计划有效性<font color="#ff6666">*</font>
            </td>
            <td class='list' style="display: none;">
                <asp:DropDownList ID="ddltPlanState" runat="server" Width="152px">
                    <asp:ListItem Value="0">有效</asp:ListItem>
                    <asp:ListItem Value="1" Selected="True">无效</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                计划执行人<font color="#ff6666">*</font>
            </td>
            <td class='list'>
                <uc1:UserPicker ID="UserPicker1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                计划描述
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPlanDesc' runat='server' Rows="4" TextMode="MultiLine" Width="85%"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td class='list' colspan="2" align="center">
                <asp:Button ID="btnSetTime" runat="server" Text="计划时间设置" CausesValidation="False"
                    OnClientClick="SelectTimeSet();" SkinID="btnClass3"></asp:Button>
            </td>
        </tr>
    </table>
    <table id="Table13" width="98%" align="center" runat="server">
        <tr>
            <td align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            计划时间设置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table3" style="width: 98%;" cellpadding="2" cellspacing="0" runat="server" class="listContent">
        <tr>
            <td class='listTitleRight' style="width: 12%">
                计划类别:
            </td>
            <td class="list">
                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                    OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                    <asp:ListItem Value="10">持续运行</asp:ListItem>
                    <asp:ListItem Value="20" Selected="True">每日执行</asp:ListItem>
                    <asp:ListItem Value="30">每周执行</asp:ListItem>
                    <asp:ListItem Value="40">每月执行</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr runat="server" id="trPlanTime">
            <td align="right" class='listTitleRight'>
                指定执行时间:
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlHours" runat="server" Width="48px">
                </asp:DropDownList>
                时<asp:DropDownList ID="ddlMinutes" runat="server" Width="48px">
                </asp:DropDownList>
                分
            </td>
        </tr>
        <tr runat="server" id="trPlanDateMonth">
            <td align="right" class='listTitleRight'>
                指定日期(每月执行):
            </td>
            <td class="list">
                第<asp:DropDownList ID="ddlDays" runat="server" Width="48px">
                </asp:DropDownList>
                天
            </td>
        </tr>
        <tr runat="server" id="trPlanDateWeek">
            <td align="right" class='listTitleRight'>
                指定日期(每周执行):
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlWeeks" runat="server" Width="64px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" id="trTime">
            <td align="right" class='listTitleRight'>
                运行时间间隔:
            </td>
            <td class="list">
                <asp:TextBox ID="txtInterval" runat="server" MaxLength="9" Style="ime-mode: Disabled"
                    onblur="CheckIsnum(this,'运行时间间隔必须为数值！');" onkeydown="NumberInput('');"></asp:TextBox>小时
            </td>
        </tr>
        <tr runat="server" id="trBetween">
            <td align="right" class='listTitleRight'>
                运行时间范围:
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlHoursBeg" runat="server" Width="48px">
                </asp:DropDownList>
                时<asp:DropDownList ID="ddlMinutesBeg" runat="server" Width="48px">
                </asp:DropDownList>
                分&nbsp; ---- &nbsp;
                <asp:DropDownList ID="ddlHoursEnd" runat="server" Width="48px">
                </asp:DropDownList>
                时<asp:DropDownList ID="ddlMinutesEnd" runat="server" Width="48px">
                </asp:DropDownList>
                分
            </td>
        </tr>
        <tr>
            <td align="right" class='listTitleRight'>
                星期运行设置:
            </td>
            <td class="list">
                <asp:CheckBoxList ID="cblWeeks" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">周日</asp:ListItem>
                    <asp:ListItem Selected="True" Value="1">周一</asp:ListItem>
                    <asp:ListItem Selected="True" Value="2">周二</asp:ListItem>
                    <asp:ListItem Selected="True" Value="3">周三</asp:ListItem>
                    <asp:ListItem Selected="True" Value="4">周四</asp:ListItem>
                    <asp:ListItem Selected="True" Value="5">周五</asp:ListItem>
                    <asp:ListItem Value="6">周六</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr runat="server" id="trWeek">
            <td>
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />
</asp:Content>
