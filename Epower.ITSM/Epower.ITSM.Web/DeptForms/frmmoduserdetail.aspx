<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.frmModUserDetail" CodeBehind="frmModUserDetail.aspx.cs" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="HEAD1" runat="server">
    <title>�޸��û�</title>
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
					alert('��½������Ϊ��');
				}
			}

			
			//ȥ���ִ���ո�
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
				{ return "" ;}    //��ʾsString�е������ַ����ǿո�,�򷵻ؿմ�
				else
				{ return sString.substring(iStart) ;}
			}
			

			//ȥ���ִ��ҿո�
			function RTrim(sString)
			{ 
				var sStr,i,sResult = "",sTemp = "" ;

				sStr = sString.split("");
				for (i = sStr.length - 1 ; i >= 0 ; i --)  // ���ַ������е���
				{ 
					sResult = sResult + sStr[i]; 
				}
				sTemp = JHshLTrim(sResult) ; // �����ַ���ǰ�ո�س�

				if (sTemp == "") { return "" ; }

				sStr = sTemp.split("");
				sResult = "" ;
				for (i = sStr.length - 1 ; i >= 0 ; i--) // �����������ַ����ٽ��е���
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
               <asp:Literal ID="litLoginName" runat="server" Text="��¼�û����ƣ�"></asp:Literal></div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtLoginName" runat="server" Width="159px" MaxLength="20"></asp:TextBox>
                
                <input
                    title="����½�ʻ��Ƿ���Ч" type="button" value="���" onclick="CheckUser(document.all.txtLoginName.value);"
                    id="btnCheckUser" class="btnClass">
                    <label style="color:Red;">*</label> 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                        runat="server" ErrorMessage="����Ϊ��" ControlToValidate="txtLoginName"></asp:RequiredFieldValidator>
                       
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 16px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                 <asp:Literal ID="litname" runat="server" Text="�û����ƣ�"></asp:Literal>   </div>
            </td>
            <td style="height: 16px" class="list">
                <asp:TextBox ID="txtName" runat="server" Width="159px" MaxLength="10"></asp:TextBox>
                <label style="color:Red;">*</label> 
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="����Ϊ��"
                    ControlToValidate="txtName"></asp:RequiredFieldValidator>
                       
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                 <asp:Literal ID="litPassword1" runat="server"  Text="���룺"></asp:Literal>     </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtFistPwd" runat="server" Width="159px" MaxLength="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                 <asp:Literal ID="litPassword2" runat="server"  Text="ȷ�����룺"></asp:Literal>    </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtLastPwd" runat="server" Width="159px" MaxLength="25"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="�������벻һ��"
                    ControlToValidate="txtLastPwd" Operator="Equal" ControlToCompare="txtFistPwd"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 4px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="Literal1" runat="server"  Text="�Ա�"></asp:Literal>     </div>
            </td>
            <td style="height: 4px" class="list">
                <asp:DropDownList ID="dropSex" runat="server" Width="159px">
                    <asp:ListItem Value="1">��</asp:ListItem>
                    <asp:ListItem Value="2">Ů</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="Literal2" runat="server"  Text=" ְλ��"></asp:Literal>    </div>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="CtrFlowCataDropListjob" runat="server" RootID="1014" />
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                <asp:Literal ID="Literal3" runat="server"  Text="�绰��"></asp:Literal>    </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtTelNo" runat="server" Width="159px" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="Literal4" runat="server"  Text="�ֻ����룺"></asp:Literal>     </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtMobile" runat="server" Width="159px" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 21px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
                 <asp:Literal ID="Literal5" runat="server"  Text="�����ʼ���"></asp:Literal>   </div>
            </td>
            <td style="height: 21px" class="list">
                <asp:TextBox ID="txtEmail" runat="server" Width="159px" MaxLength="50"></asp:TextBox><asp:RegularExpressionValidator
                    ID="RegularExpressionValidator2" runat="server" ErrorMessage="�����ʼ�����" ControlToValidate="txtEmail"
                    ValidationExpression="^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr style="display:none;">
            <td style="width: 145px; height: 21px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
               <asp:Literal ID="Literal6" runat="server"  Text="QQ���룺"></asp:Literal>     </div>
            </td>
            <td style="height: 21px" class="list">
                <asp:TextBox ID="txtQQ" runat="server" Width="159px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 16px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right">
              <asp:Literal ID="Literal7" runat="server"  Text="ѧ����"></asp:Literal>      </div>
            </td>
            <td style="height: 16px" class="list">
                <asp:DropDownList ID="dropEdu" runat="server" Width="159px">
                    <asp:ListItem Value="" Selected="True">��ѡ��...</asp:ListItem>
                    <asp:ListItem Value="��ʿ">��ʿ</asp:ListItem>
                    <asp:ListItem Value="˶ʿ">˶ʿ</asp:ListItem>
                    <asp:ListItem Value="��ѧ">��ѧ</asp:ListItem>
                    <asp:ListItem Value="��ר">��ר</asp:ListItem>
                    <asp:ListItem Value="��ר">��ר</asp:ListItem>
                    <asp:ListItem Value="����">����</asp:ListItem>
                    <asp:ListItem Value="����">����</asp:ListItem>
                    <asp:ListItem Value="Сѧ">Сѧ</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 145px; height: 21px" class="listTitle" align="right">
                <div style="display: inline; width: 136px; height: 18px" align="right" align="right">
               <asp:Literal ID="Literal8" runat="server"  Text="��ɫ��"></asp:Literal>     </div>
            </td>
            <td style="height: 21px" class="list">
                <asp:TextBox ID="txtRole" runat="server" Width="159px" MaxLength="20"></asp:TextBox>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152" Visible="False">
                    <asp:ListItem Value="0" Selected="True">����</asp:ListItem>
                    <asp:ListItem Value="2">����</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trdept" runat="server">
            <td style="width: 145px" class="listTitle" align="right">
                <div id="lblDept" style="display: inline; width: 136px; height: 18px" align="right"
                    runat="server">
              <asp:Literal ID="Literal9" runat="server"  Text="�������ţ�"></asp:Literal>      </div>
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
               <asp:Literal ID="Literal10" runat="server"  Text="����������ID��"></asp:Literal>     </div>
            </td>
            <td class="list">
                <asp:TextBox ID="txtSortID" runat="server" Width="160px" MaxLength="9">-1</asp:TextBox>
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtSortID"
                    ErrorMessage="RangeValidator" MaximumValue="999999999" MinimumValue="-1" Type="Integer"> ��������Чֵ</asp:RangeValidator>
            </td>
        </tr>
        <tr style="display:none;">
            <td style="width: 145px" class="listTitle" align="right">
                <div id="Div2" style="display: inline; width: 136px; height: 18px" align="right"
                    runat="server">
               <asp:Literal ID="Literal11" runat="server"  Text="�Ƿ�������"></asp:Literal>     </div>
            </td>
            <td nowrap class="listTitle">
                <asp:DropDownList ID="ddlLock" runat="server" Width="159px">
                    <asp:ListItem Text="��" Value="0"></asp:ListItem>
                    <asp:ListItem Text="��" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 145px" class="listTitle">
            </td>
            <td nowrap class="listTitle">
                *ע:����IDΪ-1ʱ,��ϵͳָ��������ID,��Чֵ��Χ(-1 ~ 99999)
            </td>
        </tr>
    </table>
    <table id="Table1" width="95%" class="listContent" align="center">
        <tr>
            <td align="center" class="listTitle">
                <asp:Button ID="cmdSave" runat="server" Text="ȷ��" OnClick="cmdSave_Click" CssClass="btnClass">
                </asp:Button>&nbsp;
                <input onclick="javascript:window.close()" type="button" value="ȡ��" class="btnClass">
            </td>
        </tr>
    </table>
    <input id="hidPassWord" type="hidden" runat="server">
    <input id="hidUserID" type="hidden" runat="server">
    <input id="hidprentDeptID" type="hidden" runat="server" />
    </form>
</body>
</html>
