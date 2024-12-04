<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    ValidateRequest="false" CodeBehind="frmPro_ProvideManageEdit.aspx.cs" Inherits="Epower.ITSM.Web.Provide.frmPro_ProvideManageEdit"
    Title="供应商管理" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
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

    <table id="Table3" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr3" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            供应商基本资料
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" cellpadding="2" cellspacing="0" class="listContent" runat="server"
        id="Table1">
        <tr>
            <td class="listTitleRight " style="width: 12%;">
                供应商名称
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtName" runat="server" MaxLength="50"></asp:TextBox><asp:Label
                    ID="lblWarning" runat="server" ForeColor="#FF6666" Text="*" Width="1px"></asp:Label><font
                        color="#ff6666"></font><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ErrorMessage="供应商名称不能为空！" ControlToValidate="txtName" SetFocusOnError="True">*</asp:RequiredFieldValidator><asp:Label
                                ID="lblName" runat="server" Visible="False"></asp:Label>
            </td>
            <td class="listTitleRight " style="width: 12%;">
                供应商代码
            </td>
            <td class="list">
                <asp:TextBox ID="txtCode" runat="server" MaxLength="50"></asp:TextBox>
                <asp:Label ID="lblCode" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                联系人
            </td>
            <td class="list">
                <asp:TextBox ID="txtContract" runat="server" MaxLength="50"></asp:TextBox>
                <asp:Label ID="lblContract" runat="server" Visible="False"></asp:Label>
            </td>
            <td class="listTitleRight ">
                联系人电话
            </td>
            <td class="list">
                <asp:TextBox ID="txtContractTel" runat="server" MaxLength="50"></asp:TextBox>
                <asp:Label ID="lblContractTel" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                地址
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtAddress" runat="server" Width="90%" MaxLength="200"></asp:TextBox>
                <asp:Label ID="lblAddress" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                备注
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtRemark" runat="server" Rows="3" TextMode="MultiLine" Width="90%"
                    onblur="MaxLength(this,200,'备注长度超出限定长度:');"></asp:TextBox>
                <asp:Label ID="lblRemark" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table4" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />服务协议
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table2" width="98%" align="center" runat="server" class="listContent">
        <tr>
            <td class='listTitleRight' style="width: 12%;">
                服务协议
            </td>
            <td class='list'>
                <ftb:FreeTextBox ID="txtContent" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                    Height="500px" ImageGalleryPath="Attfiles\\Photos" Width="98%">
                </ftb:FreeTextBox>
                <asp:Label ID="lblContent" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
</asp:Content>
