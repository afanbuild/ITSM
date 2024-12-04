<%@ Page Language="C#" CodeBehind="frmFlowDelete.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmFlowDelete" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>删除流程</title>
    <base target="_self" />
	<META HTTP-EQUIV="pragma" CONTENT="no-cache">
	<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache, must-revalidate">
	<META HTTP-EQUIV="expires" CONTENT="Mon, 23 Jan 1978 20:52:30 GMT">
	<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" Content="C#">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>	
	
</head>
	
</head>
<body>
    <form id="form1" runat="server">
        <TABLE id="Table1" cellSpacing="1"  cellPadding="1" width="100%"  align="center">
        <tr>
	        <td class='listTitle'  align='center' style='width:100%;height:40px;' colspan="2">
	        <FONT color="red" style="font-weight:bold;">不可恢复，确认吗？</FONT>		
        </td>	
        </tr>
        <tr>	
        <td width="15%" nowrap class="listTitle">
            原因</td>
        <td class="list" width="85%">
	        <asp:TextBox ID='txtRemark' runat='server' Width="95%" Rows="6" TextMode="MultiLine" onblur="MaxLength(this,200,'删除原因长度超出限定长度:');"></asp:TextBox>		
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRemark"
                ErrorMessage="删除原因不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator></td>		
        </tr>
        <tr>
	        <td class='listTitle'  align='center' style="width:100%; margin-top:10px;" colspan="2">
		        <asp:Button ID="btnConfirm" runat="server" Text="确认"  OnClick="btnConfirm_Click" CssClass="btnClass"  />
                <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" CssClass="btnClass" CausesValidation="false" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"  ShowSummary="False" />
            </td>	
        </tr>
        </table>
    
    </form>
</body>
</html>


<!--Begin: 引入基础脚本库-->
<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
<script type="text/javascript" language="javascript" src="../js/epower.base.js"></script>
<!--End: 引入基础脚本库-->