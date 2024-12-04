<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="MailMessageTemEdit.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.MailMessageTemEdit"
    Title="模板信息编辑" %>

<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc8" %>
<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc6" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CustSchemeCtr.ascx" TagName="CustSchemeCtr" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="Text/javascript" language="Javascript">
function ShowTable(imgCtrl)
{
      var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
      var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
      var TableID = imgCtrl.id.replace("Img","Table");
      var className;
      var objectFullName;
      var tableCtrl;
      objectFullName = <%=tr2.ClientID%>.id;
      className = objectFullName.substring(0,objectFullName.indexOf("tr2")-1);
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
    </script>

    <table width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top"  class="listTitleNew">
                            <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                    width="16" align="absbottom"/> 模板信息
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" class="listContent GridTable" cellpadding="2" cellspacing="0" runat="server" id="Table1">
        <tr id="ShowRefUser" runat="server">
            <td class="listTitleRight"  style="width: 12%">
                <asp:Literal ID="LitTemplateName" runat="server" Text="模板名称"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <uc4:CtrFlowFormText ID="CtrTemplateName" runat="server" MustInput="true" TextToolTip="模板名称" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitSystemName" runat="server" Text="应用名称"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <asp:DropDownList ID="cboApp" runat="server" AutoPostBack="True" Width="152px" OnSelectedIndexChanged="cboApp_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trFlowModelID" runat="server" visible="false">
            <td class="listTitleRight"> 
                <asp:Literal ID="Literal2" runat="server" Text="流程模型"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="cboFlowModel" runat="server" AutoPostBack="True" Width="152px"  OnSelectedIndexChanged="cboFlowModel_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server">
            <td class="listTitleRight">
                配置说明
            </td>
            <td class="list" colspan="3">
                <div id="divDesc" runat="server">
                </div>
            </td>
        </tr>
         <tr>
            <td  class="listTitleRight">
                <asp:Literal ID="Literal1" runat="server" Text="领导模板"></asp:Literal>
            </td>
            <td  class="list" colspan="3">
                <ftb:FreeTextBox ID="leaderMailContent" runat="server" Width="85%" ButtonPath="../Forms/images/epower/officexp/"
                    ImageGalleryPath="Attfiles\\Photos" Height="400px">
                </ftb:FreeTextBox>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight">
                <asp:Literal ID="LitMailContent" runat="server" Text="上报人模板内容"></asp:Literal>
            </td>
            <td  class="list" colspan="3">
                <ftb:FreeTextBox ID="FTBMailContent" runat="server" Width="85%" ButtonPath="../Forms/images/epower/officexp/"
                    ImageGalleryPath="Attfiles\\Photos" Height="400px">
                </ftb:FreeTextBox>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight">
                <asp:Literal ID="LitMailContent2" runat="server" Text="处理人模板内容"></asp:Literal>
            </td>
            <td  class="list" colspan="3">
                <ftb:FreeTextBox ID="FTBMailContent2" runat="server" Width="85%" ButtonPath="../Forms/images/epower/officexp/"
                    ImageGalleryPath="Attfiles\\Photos" Height="280px">
                </ftb:FreeTextBox>
            </td>
        </tr> 
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitModelContent" runat="server" Text="短信模板内容"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <asp:TextBox ID="txtModelContent" Width="85%" Rows="3" TextMode="MultiLine" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="litStatus" runat="server" Text="是否启用"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152px">
                    <asp:ListItem Text="" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                    <asp:ListItem Text="启用" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" >
                <asp:Literal ID="LitRemark" runat="server" Text="备注"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <asp:TextBox ID="txtRemark" runat="server" Width="85%" Rows="3" TextMode="MultiLine"
                    onblur="MaxLength(this,200,'备注长度超出限定长度:');"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitle"  colspan="4">
                &nbsp;&nbsp;动态内容是指相关表单录入的项，以规定格式放入模板内容中以便将我们输入项加在指定的邮件短信中。
            </td>
        </tr>
    </table>
</asp:Content>
