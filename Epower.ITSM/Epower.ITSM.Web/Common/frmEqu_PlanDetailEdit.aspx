<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="frmEqu_PlanDetailEdit.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmEqu_PlanDetailEdit"
    Title="资产巡检计划编辑" %>

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
		        //==zxl==
		        var url='../EquipmentManager/frmEqu_SelectItem.aspx?pDate='+sparamvalue+'&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>';
		        window.open(url,"",'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600,left=150,top=50');
			}
	function SelectTimeSet()
			{
			    var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
	            
		        var	value=window.showModalDialog('../AppForms/frmPlanDetail.aspx?pDate='+sparamvalue +'&planset=' + document.all.<%=hidPlanXml.ClientID%>.value ,"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=no;resizable=no") ;
		        if(value != null)
                {
	                if(value.length>1)
	                {
                        document.all.<%=hidPlanXml.ClientID%>.value = value;   //设备时间值
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
          objectFullName = '<%=TableImg1.ClientID%>';
          className = objectFullName.substring(0,objectFullName.indexOf("TableImg1")-1);
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
    
    <div style="display:none;">
        <asp:Button ID="hidbtnAddItem" runat="server" Text="aa" style="display:none;" OnClick="hiddBtnAdd_Click" />
    </div>
    
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
    <table id="Table2" style="width: 98%;" border="0" cellSpacing="1" cellPadding="1" runat="server"
        class="listContent">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                执行流程
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddltFlow" runat="server" Width="152px">
                </asp:DropDownList>
                <font color="#ff6666">*</font>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                计划名称
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPlanName' runat='server' MaxLength="50"></asp:TextBox><font color="#ff6666">*</font>
            </td>
        </tr>
        <tr style="display: none;">
            <td class='listTitleRight'>
                计划有效性
            </td>
            <td class='list' style="display: none;">
                <asp:DropDownList ID="ddltPlanState" runat="server">
                    <asp:ListItem Value="0">有效</asp:ListItem>
                    <asp:ListItem Value="1" Selected="True">无效</asp:ListItem>
                </asp:DropDownList>
                <font color="#ff6666">*</font>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                计划执行人
            </td>
            <td class='list'>
                <uc1:UserPicker ID="UserPicker1" runat="server" />
                <font color="#ff6666">*</font>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                计划描述
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPlanDesc' runat='server' Rows="3" TextMode="MultiLine" Width="85%"
                    onblur="MaxLength(this,100,'计划描述长度超出限定长度:');"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='list' colspan="2" align="center" style="display: none;">
                <asp:Button ID="btnSetTime" runat="server" Text="计划时间设置" CausesValidation="False"
                    OnClientClick="SelectTimeSet();" CssClass="btnClass3"></asp:Button>
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
    <table id="Table3" style="width: 98%;" border="0" cellSpacing="1" cellPadding="1" runat="server"
        class="listContent">
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
            <td class='listTitleRight'>
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
            <td class='listTitleRight'>
                指定日期(每月执行):
            </td>
            <td class="list">
                第<asp:DropDownList ID="ddlDays" runat="server" Width="48px">
                </asp:DropDownList>
                天
            </td>
        </tr>
        <tr runat="server" id="trPlanDateWeek">
            <td class='listTitleRight'>
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
            <td class='listTitleRight'>
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
            <td class='listTitleRight'>
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
    <table id="TableImg1" width="98%" align="center" runat="server">
        <tr>
            <td align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            巡检项设置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table1" style="width: 98%;" cellpadding="0" border="0" cellspacing="0"
        runat="server">
        <tr>
            <td>
                <asp:DataGrid ID="gdPatrol" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="gdPatrol_ItemCommand">
                    <Columns>
                        <asp:BoundColumn DataField='EquID' HeaderText='资产编号' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='ItemID' HeaderText='巡检项编号' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='EquName' HeaderText='资产名称'></asp:BoundColumn>
                        <asp:BoundColumn DataField='ItemName' HeaderText='巡检项名称'></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="44" VerticalAlign="Top"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Button ID="btnadd" runat="server" Text="新增" CausesValidation="False" OnClientClick="SelectEquItem();"
                                     SkinID="btnClass1"></asp:Button>  <%--OnClick="btnadd_Click"--%>
                                    
                                    <asp:Button ID="hiddBtnAdd" runat="server" Text="aa" style="display:none;" OnClick="hiddBtnAdd_Click" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False"
                                     SkinID="btnClass1"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />

    <script language="javascript">	
    var temp = document.all.<%=hidTable.ClientID%>.value;
    if(temp!="")
    {
        var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
        var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
        var arr=temp.split(",");
        for(i=1;i<arr.length;i++)
        {
            var tableid=arr[i];
            var tableCtrl = document.all.item(tableid);
            tableCtrl.style.display ="none";
            var ImgID = tableid.replace("ctl00_ContentPlaceHolder1_Table","Img");
            var imgCtrl = document.all.item(ImgID)
            imgCtrl.src = ImgPlusScr ;	
        }
    }
    </script>

</asp:Content>
