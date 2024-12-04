<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrRight" Src="../Controls/CtrRight.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmRight" Codebehind="frmRight.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD id="HEAD1" runat="server">
		<TITLE>权限维护</TITLE>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" type="text/javascript" src="../Js/App_Base.js"> </script>
				<script src="../Js/App_Common.js" language="javascript"></script>
				<script type="text/javascript" src="../Js/Common.js"></script>
		<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
				<script language="javascript" type="text/javascript" src="../JS/jquery-ui-1.8.20.custom.min.js"></script>
        <link type="text/css" href="../JS/css/ui-lightness/jquery-ui-1.8.20.custom.css" rel="stylesheet" />
	
		<!--#include file="../Js/tableSort.js" -->
		
	</HEAD>
	<script language="javascript">
			function CanRead_Click(ctl)
			{
				reg=/chkCanRead/g;
				var idtag=ctl.id.replace(reg,"");
				if (ctl.checked==false) {
					document.all(idtag+"chkCanAdd").checked=false;
					document.all(idtag+"chkCanModify").checked=false;
					document.all(idtag+"chkCanDelete").checked=false;
				}
			}
			
			function Delete_confirm()
			{
				if(!confirm("删除将不可恢复,确定要删除吗?"))
				{
					return false;
				}
			}
			
			function Show_ModifyWindow(obj)
			{
				reg=/cmdModify/g;
				var idtag=obj.id.replace(reg,"");
				var rid=document.all(idtag+"labRightID").innerText;
				var value=OpenNoBarWindow("frmeditright.aspx?rightid="+rid+"&TypeFrm=frmright",500,300);
				
			}
			
			//全选复选框
		function checkAll(checkAll)
		{			  
			var len = document.forms[0].elements.length;
			var cbCount = 0;
			for (i=0;i < len;i++)
			{
				if (document.forms[0].elements[i].type == "checkbox")
				{
					if (document.forms[0].elements[i].name.indexOf("chkSelect") != -1 && 
						document.forms[0].elements[i].name.indexOf("dgRight") != -1 &&
						document.forms[0].elements[i].disabled == false)
					{
						document.forms[0].elements[i].checked = checkAll.checked;

						cbCount += 1;
					}
				}
			}		
		} 
		//选择操作项
		function SelectOperates()
		{
		    //=====zxl==
		    var url="frmPopOperates.aspx?OperateID="+document.all.txtOpID.value+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmright";
		    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=504,height=520,left=150,top=50");
		}
		//选择部门，用户组，条件人员
		function PopSelectDialog()
		{
			switch(document.all.dpdObjectType.value)
			{
				case "10":
					SelectPDept();
					break;
					
				case "20":
					SelectStaff();
					break;
				
				case "30":
					SelectActor();
					break;
				default:
				    alert("请选择授权对象类型！");
				    break;
			}
		}
		function SelectPDept()
		{
		    var url="frmpopdept.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmright";
		    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300,left=150,top=100");
		}
		
		function SelectStaff()
		{
		    var url="frmSelectStaffRight.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmright";
		    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=462,height=560,left=150,top=100");
		}
		
		function SelectActor()
		{
		    var url="frmPopActor.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmright";
		    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=512,height=580,left=150,top=100");
		}
        var selectobj = document.getElementById("name0");
    
        function chang(obj,img){
                if(obj!=selectobj)	    
		            obj.background="../Images/"+img;
        }
		function chang_Class(name,num,my)
        {
	        for(i=0;i<num;i++){
		        if(i!=my){
			        document.getElementById(name+i).className="STYLE4";
			        //document.getElementById(name+i).background="../Images/lm-a.gif";
			        $("#" + name + i).css("background-image", "url(../Images/lm-a.gif)");
		        }
	        }
	        document.getElementById(name+my).className="td_3";
	        //document.getElementById(name+my).background="../Images/lm-b.gif";
	        $("#" + name + my).css("background-image", "url(../Images/lm-b.gif)");
	        document.getElementById("hidSelect").value=my;
	     }
		</script>
	<body bgcolor="#ffffff">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="tbMain" width="100%" class="listContent" align="center">
				<TR vAlign="top" height="30">
					<TD class="list">
						<uc1:ctrtitle id="uTitle" runat="server"></uc1:ctrtitle></TD>
				</TR>
				<TR>
					<TD class="list">
						<table align="left" border="0" cellpadding="0" cellspacing="0" height="29">
                            <tr style="CURSOR: hand">
                                <td width="7" valign="top"><img src="../Images/lm-left.gif" width="7" height="29" /></td>
                                <td width="95" id="name0"  height="29" align="center"
                                   style="cursor: hand;" background="../Images/lm-a.gif" class="td_3" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                                   <asp:LinkButton ID="lnkbtndept" OnClientClick="chang_Class('name',2,0)" runat="server" OnClick="lnkbtndept_Click">管理员权限</asp:LinkButton>
                                </td>
                                <td width="95" id="name1" height="29" align="center"
                                    style="cursor: hand;" background="../Images/lm-b.gif" class="STYLE4" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                                    <asp:LinkButton ID="lnkbtnall" OnClientClick="chang_Class('name',2,1)" runat="server" OnClick="lnkbtnall_Click">全部权限</asp:LinkButton>
                                </td>
                                <td width="7" valign="top"><img src="../Images/lm-right.gif" width="7" height="29" /></td>
                            </tr>
                        </table>
						</TD>
				</TR>
				<TR height="30">
					<TD class="listTitle">
						<INPUT onclick="OpenNoBarWindow('frmeditrightBatch.aspx?rightid=',600,500);" type="button"
							value="批量设置" class="btnClass">&nbsp;
						<asp:button id="cmdDelete"  TabIndex="20" runat="server" Text="删除" onclick="cmdDelete_Click" UseSubmitBehavior="False" CssClass="btnClass"></asp:button>
                        </TD>
				</TR>
				<TR>
					<td class="listTitle">操作项ID:
						<asp:textbox id="txtOpID" runat="server" Width="88px" style="ime-mode:Disabled" onblur="CheckIsnum(this,'操作项ID必须为数值！');" onkeydown="NumberInput('');"></asp:textbox>
						
						操作项名称:
						<asp:textbox id="txtOpName" runat="server" Width="88px"></asp:textbox>
						
                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
                
						<INPUT id="cmdPopOperates" onclick="SelectOperates()" type="button" value="..." name="Button1" class="btnClass2">
						&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="cmdFind1" runat="server" Text="查询" onclick="cmdFind1_Click" CssClass="btnClass"></asp:button></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 17px" class="listTitle">授权对象类型:
						<asp:dropdownlist id="dpdObjectType" runat="server"></asp:dropdownlist>对象名称:
						<asp:textbox ID="txtObjectName" runat="server" Width="88px" style="ime-mode:Disabled;" onblur="CheckIsnum(this,'对象ID必须为数值！');" onkeydown="NumberInput('');"></asp:textbox>
						<asp:textbox id="txtObjectID" runat="server" Width="88px" style="ime-mode:Disabled; display:none;" >
						</asp:textbox>

						<INPUT id="cmdPop" onclick="PopSelectDialog()" type="button" value="..." class="btnClass2">
						&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="cmdFind" runat="server" Text="查询" onclick="cmdFind_Click" CssClass="btnClass"></asp:button></TD>
				</TR>
</table>
<TABLE id="TABLE1" width="100%" class="listContent" cellpadding=0>
				<TR>
					<TD class="listContent">
						<asp:datagrid  id="dgRight" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  AllowPaging="True"
							PageSize="20">
							<Columns>
								<asp:TemplateColumn>
								    <HeaderTemplate>
						                <asp:CheckBox id="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
						            </HeaderTemplate>
									<ItemTemplate>
										<asp:CheckBox id="chkSelect" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<HeaderStyle Width="6%"></HeaderStyle>
									<ItemTemplate>
										<a href="#" id="cmdModify" runat="server" onclick="Show_ModifyWindow(this)">编辑</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="权限ID">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label ID="labRightID" Runat=server Text='<%#DataBinder.Eval(Container.DataItem, "RightID")%>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="OpNameAndID" HeaderText="操作项">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ObjectType" HeaderText="授权对象类型">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ObjectName" HeaderText="授权对象">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RightRange" HeaderText="权限范围">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="权限值">
									<HeaderStyle Width="100%"></HeaderStyle>
									<ItemTemplate>
										<uc1:CtrRight id=uRight runat="server" RightValue='<%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "RightValue"))%>' Enabled="false">
										</uc1:CtrRight>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="ExtDeptList"></asp:BoundColumn>
							</Columns>
							<PagerStyle Visible="False"></PagerStyle>
						</asp:datagrid>
						</TD>
				</TR>
				<tr>
				    <td class="listTitle" align="right"><uc1:controlpage id="ControlPageRight" runat="server"></uc1:controlpage></td>
				</tr>
			</TABLE>
			<input id="hidSelect" type="hidden" runat="server" value="0" />
			<script type="text/javascript" language="javascript" >
			    var iselect = document.getElementById("hidSelect").value;
			    selectobj = document.getElementById("name"+iselect);
			    chang_Class('name',2,iselect)
			</script>
		</FORM>
	</body>
</HTML>
