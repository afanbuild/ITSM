<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCalenderEdit.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCalenderEdit"
    Title="" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
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
		
		function PopSelectDialog()
		{
			switch(document.all.<%=dpdObjectType.ClientID %>.value)
			{
				case "2":   //部门
					SelectPDept();
					break;
					
				case "3":  //人员
					SelectStaff();
					break;
					
			}
		}
		
		function SelectPDept()
		{
			var value=window.showModalDialog("../mydestop/frmpopdept.aspx");
			if(value==null)
			    return;
			if(value.length>1)
			{
				arr=value.split("@");
				document.all.<%=txtObjectName.ClientID%>.value=arr[1]+"(" +arr[0] + ")";
				document.all.<%=txtObjectId.ClientID%>.value=arr[0];
				document.all.<%=hidObjectName.ClientID%>.value = arr[1]+"(" +arr[0] + ")";
			}
			else
			{
				document.all.<%=hidObjectName.ClientID%>.value = "";
				document.all.<%=txtObjectName.ClientID%>.value = "";
				document.all.<%=txtObjectId.ClientID%>.value = "";
			}
		}
		
		function SelectStaff()
		{
			var value=window.showModalDialog("../mydestop/frmSelectPerson.htm");
			if(value==null)
			    return;	
			if(value.length>1)
			{
				arr=value.split("@");
				document.all.<%=txtObjectName.ClientID%>.value=arr[1]+"(" +arr[0] + ")";
				document.all.<%=txtObjectId.ClientID%>.value=arr[0];
				
				document.all.<%=hidObjectName.ClientID%>.value = arr[1]+"(" +arr[0] + ")";
			}
			else
			{
				document.all.<%=hidObjectName.ClientID%>.value = "";
				document.all.<%=txtObjectName.ClientID%>.value = "";
				document.all.<%=txtObjectId.ClientID%>.value = "";
			}
		}
		
		function selectChange()
		{
		    switch(document.all.<%=dpdObjectType.ClientID %>.value)
	        {
		        case "2":   //部门
			        document.getElementById("<%=choose.ClientID%>").style.display="";
		            document.getElementById("<%=choose1.ClientID%>").style.display="none";  
		            document.all.<%=hidObjectName.ClientID%>.value = "";
				    document.all.<%=txtObjectName.ClientID%>.value = "";
				    document.all.<%=txtObjectId.ClientID%>.value = "";
			        break;
		        case "3":  //人员
			        document.getElementById("<%=choose.ClientID%>").style.display="";
		            document.getElementById("<%=choose1.ClientID%>").style.display="none";
		            document.all.<%=hidObjectName.ClientID%>.value = "";
				    document.all.<%=txtObjectName.ClientID%>.value = "";
				    document.all.<%=txtObjectId.ClientID%>.value = "";  
			        break;
	            case "1":  //机构
	                document.getElementById("<%=choose.ClientID%>").style.display="none";
		            document.getElementById("<%=choose1.ClientID%>").style.display="";  
		            break;
		        case "0":  //全局
	                document.getElementById("<%=choose.ClientID%>").style.display="none";
		            document.getElementById("<%=choose1.ClientID%>").style.display="none";  
		            break;	
	        }
		}
        
        function SubmitValidate()
        {
             switch(document.all.<%=dpdObjectType.ClientID %>.value)
	         {
		        case "2":   //部门
			        if(document.all.<%=txtObjectName.ClientID%>.value=="") 
			        {
			            alert("请选择部门！");
			            return false;
			        }
			        break;
		        case "3":  //人员
			       if(document.all.<%=txtObjectName.ClientID%>.value=="") 
			        {
			            alert("请选择人员！");
			            return false;
			        } 
			        break;
	            case "1":  //机构
	                if(document.all.<%=dpdObjectName.ClientID%>.value=="请选择") 
			        {
			            alert("请选择机构！");
			            return false;
			        }
		            break;
		        case "0":  //全局
		            break;	
		        default:
		            alert("请选择对象类型！");
		            return false;
		            break;
	        }
        }
    </script>

    <input type="hidden" id="hidObjectName" value='' runat="server" />
    <table style='width: 98%' class='listContent'>
        <tr>
            <td class='listTitleRight' style="width: 12%">
                对象类型
            </td>
            <td class='list'>
                <asp:DropDownList ID="dpdObjectType" runat="server" Width="152px">
                    <asp:ListItem Value="0">全局</asp:ListItem>
                    <asp:ListItem Value="1">机构</asp:ListItem>
                    <asp:ListItem Value="2">部门</asp:ListItem>
                    <asp:ListItem Value="3">人员</asp:ListItem>
                </asp:DropDownList>
                <font color="#ff6666">*</font>
            </td>
        </tr>
        <tr id="choose" runat="server">
            <td class='listTitleRight' style="width: 12%">
                对象名称
            </td>
            <td class='list'>
                <asp:TextBox ID='txtObjectName' runat='server' MaxLength="9" onkeydown="NumberInput('');"
                    Style="ime-mode: Disabled" onblur="NumberInput('');" ReadOnly="True"></asp:TextBox><input
                        id="cmdPop" onclick="PopSelectDialog()" runat="server" type="button" value="..."
                        class="btnClass">
                <input id="txtObjectId" runat="server" type="hidden" />
            </td>
        </tr>
        <tr id="choose1" style="display: none" runat="server">
            <td class="listTitleRight" style="width: 12%">
                对象名称
            </td>
            <td class='list'>
                <asp:DropDownList ID="dpdObjectName" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style="width: 12%">
                假期时间
            </td>
            <td class='list' style="height: 26px">
                <uc2:ctrdateandtime ID="Ctrdateandtime1" runat="server" ContralState="eNormal" ShowTime="false" />
            </td>
        </tr>
    </table>

    <script type="text/ecmascript" language="javascript">
    switch(document.all.<%=dpdObjectType.ClientID %>.value)
	        {
		        case "2":   //部门
			        document.getElementById("<%=choose.ClientID%>").style.display="";
		            document.getElementById("<%=choose1.ClientID%>").style.display="none";  
			        break;
		        case "3":  //人员
			        document.getElementById("<%=choose.ClientID%>").style.display="";
		            document.getElementById("<%=choose1.ClientID%>").style.display="none";
			        break;
	            case "1":  //机构
	                document.getElementById("<%=choose.ClientID%>").style.display="none";
		            document.getElementById("<%=choose1.ClientID%>").style.display="";  
		            break;
		        case "0":  //全局
	                document.getElementById("<%=choose.ClientID%>").style.display="none";
		            document.getElementById("<%=choose1.ClientID%>").style.display="none";  
		            break;	
	        }
    </script>

</asp:Content>
