<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.frmModUserDetail" CodeBehind="frmModUserDetail.aspx.cs" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="HEAD1" runat="server">
    <title>修改用户</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="javascript">
		<!--
			
			
			function CheckUser(LoginName)
			{		
				if(LoginName!='')
				{	
					var features =
					'dialogWidth:350px;' +
					'dialogHeight:300px;' +
					'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scrollbars:yes;resizable=yes';
					//======zxl==
					// window.showModalDialog('frmCheckUser.aspx?LoginName='+LoginName,'',features);
					window.open('frmCheckUser.aspx?LoginName='+LoginName,'','scrollbars=no,status=yes ,resizable=yes,top=100,left=250,width=300,height=300');
				}
				else
				{
					alert('登陆名不能为空');
				}
			}

			
			//去除字串左空格
			function LTrim(sString)
			{ 
				var sStr,i,iStart,sResult = "";

				sStr = sString.split("");
				iStart = -1 ;
				for (i = 0 ; i < sStr.length ; i++)
				{
					if (sStr[i] != " ") 
					{
						iStart = i;
						break;
					}
				}
				if (iStart == -1) 
				{ return "" ;}    //表示sString中的所有字符均是空格,则返回空串
				else
				{ return sString.substring(iStart) ;}
			}
			

			//去除字串右空格
			function RTrim(sString)
			{ 
				var sStr,i,sResult = "",sTemp = "" ;

				sStr = sString.split("");
				for (i = sStr.length - 1 ; i >= 0 ; i --)  // 将字符串进行倒序
				{ 
					sResult = sResult + sStr[i]; 
				}
				sTemp = JHshLTrim(sResult) ; // 进行字符串前空格截除

				if (sTemp == "") { return "" ; }

				sStr = sTemp.split("");
				sResult = "" ;
				for (i = sStr.length - 1 ; i >= 0 ; i--) // 将经处理后的字符串再进行倒序
				{
					sResult = sResult + sStr[i];
				}
				return sResult ;
			} 
				
		//-->
    </script>

</head>
<script language="javascript">
        function SelectDept()
		{
			//var	value=window.showModalDialog("frmpopdept.aspx?CurrDeptID=" + document.all.hidDeptID.value);
			var url="frmpopdept.aspx?&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmmoduserdetail";
			
			window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=400,height=400");
		}
			
</script>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table2" width="95%" class="listContent" align="center">
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="litLoginName" runat="server" Text="登录用户名称："></asp:Literal></div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtLoginName" runat="server" Width="159px" MaxLength="20"></asp:TextBox>
                
                <input
                    title="检测登陆帐户是否有效" type="button" value="检测" onclick="CheckUser(document.all.txtLoginName.value);"
                    id="btnCheckUser" class="btnClass">
                    <label style="color:Red;">*</label> 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                        runat="server" ErrorMessage="不能为空" ControlToValidate="txtLoginName"></asp:RequiredFieldValidator>
                       
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 16px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                 <asp:Literal ID="litname" runat="server" Text="用户名称："></asp:Literal>   </div>
            </td>
            <td style="height: 16px" class="list">
                <asp:TextBox ID="txtName" runat="server" Width="159px" MaxLength="10"></asp:TextBox>
                <label style="color:Red;">*</label> 
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="不能为空"
                    ControlToValidate="txtName"></asp:RequiredFieldValidator>
                       
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                 <asp:Literal ID="litPassword1" runat="server"  Text="密码："></asp:Literal>     </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtFistPwd" runat="server" Width="159px" MaxLength="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                 <asp:Literal ID="litPassword2" runat="server"  Text="确认密码："></asp:Literal>    </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtLastPwd" runat="server" Width="159px" MaxLength="25"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="两次输入不一致"
                    ControlToValidate="txtLastPwd" Operator="Equal" ControlToCompare="txtFistPwd"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 4px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="Literal1" runat="server"  Text="性别："></asp:Literal>     </div>
            </td>
            <td style="height: 4px" class="list">
                <asp:DropDownList ID="dropSex" runat="server" Width="159px">
                    <asp:ListItem Value="1">男</asp:ListItem>
                    <asp:ListItem Value="2">女</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="Literal2" runat="server"  Text=" 职位："></asp:Literal>    </div>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="CtrFlowCataDropListjob" runat="server" RootID="1014" />
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                <asp:Literal ID="Literal3" runat="server"  Text="电话："></asp:Literal>    </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtTelNo" runat="server" Width="159px" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="Literal4" runat="server"  Text="手机号码："></asp:Literal>     </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtMobile" runat="server" Width="159px" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 21px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                 <asp:Literal ID="Literal5" runat="server"  Text="电子邮件："></asp:Literal>   </div>
            </td>
            <td style="height: 21px" class="list">
                <asp:TextBox ID="txtEmail" runat="server" Width="159px" MaxLength="50"></asp:TextBox><asp:RegularExpressionValidator
                    ID="RegularExpressionValidator2" runat="server" ErrorMessage="电子邮件有误！" ControlToValidate="txtEmail"
                    ValidationExpression="^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr style="display:none;">
            <td style="width: 145px; height: 21px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="Literal6" runat="server"  Text="QQ号码："></asp:Literal>     </div>
            </td>
            <td style="height: 21px" class="list">
                <asp:TextBox ID="txtQQ" runat="server" Width="159px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 16px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
              <asp:Literal ID="Literal7" runat="server"  Text="学历："></asp:Literal>      </div>
            </td>
            <td style="height: 16px" class="list">
                <asp:DropDownList ID="dropEdu" runat="server" Width="159px">
                    <asp:ListItem Value="" Selected="True">请选择...</asp:ListItem>
                    <asp:ListItem Value="博士">博士</asp:ListItem>
                    <asp:ListItem Value="硕士">硕士</asp:ListItem>
                    <asp:ListItem Value="大学">大学</asp:ListItem>
                    <asp:ListItem Value="大专">大专</asp:ListItem>
                    <asp:ListItem Value="中专">中专</asp:ListItem>
                    <asp:ListItem Value="高中">高中</asp:ListItem>
                    <asp:ListItem Value="初中">初中</asp:ListItem>
                    <asp:ListItem Value="小学">小学</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 21px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right" align="right">
               <asp:Literal ID="Literal8" runat="server"  Text="角色："></asp:Literal>     </div>
            </td>
            <td style="height: 21px" class="list">
                <asp:TextBox ID="txtRole" runat="server" Width="159px" MaxLength="20"></asp:TextBox>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152" Visible="False">
                    <asp:ListItem Value="0" Selected="True">启用</asp:ListItem>
                    <asp:ListItem Value="2">禁用</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trdept" runat="server">
            <td style="width: 145px" class="listTitle" align="right">
                <div id="lblDept" style="display: inline; width: 136px; height: 18px" align="right"
                    runat="server">
              <asp:Literal ID="Literal9" runat="server"  Text="所属部门："></asp:Literal>      </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtDeptName" runat="server" Width="160" ReadOnly="True"></asp:TextBox>
                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"  type="hidden" />
                <input  id="hidDeptID" style="width: 56px" type="hidden" size="4" name="hidDeptID" runat="server"><input
                        id="cmdPopParentDept" onclick="SelectDept()" type="button" value="..." name="cmdPopParentDept"
                        runat="server" class="btnClass2"><input id="hidOldDeptID" style="width: 56px" type="hidden" size="4"
                            name="hidDeptID" runat="server">
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div id="Div1" style="display: inline; width: 136px; height: 18px" align="right"
                    runat="server">
               <asp:Literal ID="Literal10" runat="server"  Text="部门中排序ID："></asp:Literal>     </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtSortID" runat="server" Width="160px" MaxLength="9">-1</asp:TextBox>
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtSortID"
                    ErrorMessage="RangeValidator" MaximumValue="999999999" MinimumValue="-1" Type="Integer"> 请输入有效值</asp:RangeValidator>
            </td>
        </tr>
        <tr style="display:none;">
            <td style="width: 145px" class="listTitle" align="right">
                <div id="Div2" style="display: inline; width: 136px; height: 18px" align="right"
                    runat="server">
               <asp:Literal ID="Literal11" runat="server"  Text="是否锁定："></asp:Literal>     </div>
            </td>
            <td nowrap class="listTitle">
                <asp:DropDownList ID="ddlLock" runat="server" Width="159px">
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle">
            </td>
            <td nowrap class="listTitle">
                *注:排序ID为-1时,由系统指定其排序ID,有效值范围(-1 ~ 99999)
            </td>
        </tr>
    </table>
    <table id="Table1" width="95%" class="listContent" align="center">
        <tr>
            <td align="center" class="listTitle">
                <asp:Button ID="cmdSave" runat="server" Text="确认" OnClick="cmdSave_Click" CssClass="btnClass">
                </asp:Button>&nbsp;
                <input onclick="javascript:window.close()" type="button" value="取消" class="btnClass">
            </td>
        </tr>
    </table>
    <input id="hidPassWord" type="hidden" runat="server">
    <input id="hidUserID" type="hidden" runat="server">
    <input id="hidprentDeptID" type="hidden" runat="server" />
    </form>
</body>
</html>
