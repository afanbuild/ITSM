<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_SelectItem.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_SelectItem"
    Title="资产项目选择" %>

<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">


function ShowTable(imgCtrl)
    {
          var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
          var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
          var TableID = imgCtrl.id.replace("Img","Table");
          var className;
          var objectFullName;
          var tableCtrl;
          objectFullName = '<%=txtName.ClientID%>';
          className = objectFullName.substring(0,objectFullName.indexOf("txtName")-1);
          tableCtrl = document.all.item(className.substr(0,className.length)+"_"+TableID);
          if(imgCtrl.src.indexOf("icon_expandall") != -1)
          {
            tableCtrl.style.display ="";
            imgCtrl.src = ImgMinusScr ;
          }
          else
          {
            tableCtrl.style.display ="none";
            imgCtrl.src = ImgPlusScr ;		 
          }
    }
 function delete_confirm()
	{
	    event.returnValue =confirm("确认要删除吗?");
	}
function Cancel_confirm()
    {
        top.close();
    }
 function SaveItem_confirm()
	{
	    if(document.all.<%=txtItemName.ClientID%>.value.trim()=="")
		{
		    alert("巡检项名称必须输入！");
		    event.returnValue = false;
		}
		else
		   event.returnValue = true; 
	}
String.prototype.trim = function()  //去空格

			{
				// 用正则表达式将前后空格

				// 用空字符串替代。

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
//设备
function SelectEqu(obj) 
{   
    var url="../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&randomid="+GetRandom()+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmEqu_SelectItem";
     window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=100");
}

        function GetEqu() {
            var equId = "<%=this.Master.MainID %>";
            var tb = document.getElementById("<%=this.dtlstProject.ClientID %>");
            var cbs = $(tb).find("input[type='checkbox']");
            if (cbs.length == 0) {
                alert("请选择巡检项");
            }
            else {
                var content = new Array();
                for (var i = 0; i < cbs.length; i++) {
                    var cb = cbs[i];
                    if (cb.checked)
                        content.push(cb.value);
                }
                if (window.opener.SetEqu) {
                    window.opener.SetEqu(equId, content.join(","));
                    window.close();
                }
            }
            window.opener
            return false;
        }
    </script>
    
    <table style="width: 98%" class="listContent" cellpadding="2" cellspacing="0">
        <tr>
            <td class='listTitleRight'' ' style='width: 12%;'>
                资产名称
            </td>
            <td class='list'>
                <asp:TextBox ID='txtName' runat='server' ReadOnly="true"></asp:TextBox>
                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
                <asp:Label ID="lblName" runat="server" Text="" Visible="false"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="资产名称不能为空！"
                    ControlToValidate="txtName" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:Button ID="cmdEqu" runat="server" Text="..." OnClientClick="SelectEqu(this);"
                     CausesValidation="false" SkinID="btnClass2" /> <%--OnClick="btnLoadItem_Click"--%>
                   <asp:Button runat="server" Text="子窗体要调用的" style="display:none;" ID="hiddeCmdEqu" OnClick="btnLoadItem_Click" />
                <font color="#ff6666">*</font>
                <input id="hidEqu" style="width: 56px; height: 19px" type="hidden" runat="server" />
                <input id="hidEquName" style="width: 56px; height: 19px" type="hidden" runat="server" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                资产编号
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCode' runat='server'></asp:TextBox>
                <asp:Label ID="lblCode" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr style="display: none;">
            <td class='listTitleRight'>
                资产目录
            </td>
            <td class='list'>
                <asp:TextBox ID='txtMulu' runat='server'></asp:TextBox>
                <asp:Label ID="lblMulu" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                维护机构
            </td>
            <td class='list'>
                <asp:TextBox ID='txtOpionBank' runat='server'></asp:TextBox>
                <asp:Label ID="lblOpionBank" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                维护部门
            </td>
            <td class='list'>
                <asp:TextBox ID='txtDept' runat='server'></asp:TextBox>
                <asp:Label ID="lbltDept" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                类别
            </td>
            <td class='list'>
                <uc2:ctrEquCataDropList ID="CtrEquCataDropList1" runat="server" RootID="1" />
            </td>
        </tr>
    </table>
    <br />
    <table style="width: 98%" id="TableImg2" runat="server">
        <tr id="tr2" runat="server">
            <td align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            巡检项定义
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table2" style="width: 98%;" runat="server" class="listContent">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                巡检项名称
            </td>
            <td class='list' style="width: 23%;">
                <asp:TextBox ID='txtItemName' runat='server' MaxLength="25"></asp:TextBox>
                <input id="hidItemNameID" type="hidden" runat="server" />
            </td>
            <td style="width: 65%" class="listTitle" align="left">
                <asp:Button ID="btnAdd" runat="server" Text="保存项" OnClick="btnAdd_Click" CausesValidation="False"
                    OnClientClick="SaveItem_confirm()" CssClass="btnClass" />
                &nbsp;<asp:Button ID="btnDelete" runat="server" Text="删除项" OnClick="btnDelete_Click"
                    OnClientClick="delete_confirm()" CausesValidation="False" CssClass="btnClass" />&nbsp;
                <asp:Button ID="btnSelect" runat="server" Text="选  择" CausesValidation="False" OnClick="btnSelect_Click"
                    CssClass="btnClass" />
                <asp:Button ID="btnCancel" runat="server" Text="取  消" OnClientClick="Cancel_confirm()"
                    CausesValidation="False" CssClass="btnClass" />
            </td>
        </tr>
        <tr>
            <td class="list" colspan="3">
                <asp:DataList ID="dtlstProject" runat="server" RepeatColumns="5" Width="100%" RepeatLayout="Table"
                    OnItemCommand="dtlstProject_ItemCommand">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDel" runat="server" Checked="true"></asp:CheckBox>                        
                        <asp:LinkButton ID="lblItemName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ItemName")%>'
                            CommandName="select"></asp:LinkButton>
                        <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID")%>'
                            Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
</asp:Content>
