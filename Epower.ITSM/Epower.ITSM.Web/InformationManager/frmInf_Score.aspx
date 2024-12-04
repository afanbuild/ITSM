<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInf_Score.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmInf_Score" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>知识评分</title>
    <base target="_self" />
    <script language="javascript" type="text/javascript" src="../Js/App_Base.js"></script>
    <script language="javascript" type="text/javascript">
	function CheckValid(obj)
	{
	    //var obj = document.getElementById(obj.id.replace("btnConfirm","txtScore"));
	    if(document.all.txtScore.value =="")
	    {
	        alert("分数不能为空！");
		    event.returnValue = false;
		}
		else if (document.all.txtScore.value<0 || document.all.txtScore.value >100)
		{
		    alert("分数必须为0至100的数值！");
		    event.returnValue = false;
		}
		else
		{
		    event.returnValue = true;
		}
	}
    </script>    
</head>
<body>
    <form id="form1" runat="server">
    <table style="width:100%" class="listContent" align="center" ">
    <tr>
	    <td class='list'  align='center' noWrap>知识评分
	    </td>
	</tr>
	</table>
	<br />
    <table style="width:100%" class="listContent" align="center">
	<tr>
	    <td class='listTitle'  align='right' noWrap style="width:30%">分数
	    </td>
	    <td class='list'  align='left' >
            <asp:TextBox ID="txtScore" runat="server" style="ime-mode:Disabled" onblur="CheckIsnum(this,'分数必须为数值！');"  onkeydown="NumberInput('');"></asp:TextBox>
	    </td>
	</tr>
	<tr>
	    <td class='listTitle'  align='right' noWrap style="width:30%">评分人
	    </td>
	    <td class='list'  align='left' >
            <asp:Label ID="lblUserName" runat="server" ></asp:Label>
	    </td>
	</tr>
	<tr>
	    <td class='listTitle'  align='right' noWrap style="width:30%">评分时间
	    </td>
	    <td class='list'  align='left' >
            <asp:Label ID="lblTime" runat="server" ></asp:Label>
	    </td>
	</tr>
	</table>
	<br />
	<br />
	<table style="width:100%" class="listContent">
	    <tr>
	    <td class='listTitle' align='center' style='width:100%;' noWrap colspan="2">
	        <asp:Button ID="btnConfirm" runat="server" Text="确定" OnClientClick="CheckValid();" OnClick="btnConfirm_Click" />
            &nbsp;
	        <asp:Button ID="btnClose" runat="server" Text="取消" OnClientClick="window.close();"/>
	    </td>
	</tr>
	</table>
    </form>
</body>
</html>
