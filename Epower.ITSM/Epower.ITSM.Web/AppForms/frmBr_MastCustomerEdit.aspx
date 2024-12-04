<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false" CodeBehind="frmBr_MastCustomerEdit.aspx.cs"
    Inherits="Epower.ITSM.Web.AppForms.frmBr_MastCustomerEdit" %>

<%@ Register Src="../Controls/ctrSetUserOtherRight.ascx" TagName="ctrSetUserOtherRight"
    TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
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
    
        <style type="text/css">
        #<%=Table4.ClientID%> td
        {
        	border: 1px solid #CEE3F2;
        }
    </style>


    <table width="98%" align="center" runat="server" class="listNewContent ">
        <tr id="tr3" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img4" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            单位基本资料
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" class="GridTable" cellpadding="2" cellspacing="0" align="center"
        id="Table4" runat="server">
        <tr>
            <td class="listTitleRight " style="width: 12%;">
                单位简称
            </td>
            <td class="list" style="width: 35%;">
                <uc4:CtrFlowFormText ID="CtrFTShortName" runat="server" MustInput="true" TextToolTip="单位简称" />
            </td>
            <td class="listTitleRight " style="width: 12%;">
                单位代码
            </td>
            <td class="list">
                <asp:TextBox ID="txtCustomCode" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                单位全称
            </td>
            <td class="list" colspan="3">
                <uc4:CtrFlowFormText ID="CtrFTFullName" runat="server" MustInput="true" TextToolTip="单位全称" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                单位类型
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="ctrFCDWTType" runat="server" ContralState="eNormal"
                    RootID="1015" Visible="true" />
            </td>
            <td class="listTitleRight ">
                单位性质
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server" ContralState="eNormal"
                    RootID="1016" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                联系电话
            </td>
            <td class="list">
                <uc4:CtrFlowFormText ID="CtrFTTel1" runat="server" MustInput="true" TextToolTip="联系电话" />
            </td>
            <td class="listTitleRight ">
                传 &nbsp;&nbsp; 真
            </td>
            <td class="list">
                <asp:TextBox ID="txtFax1" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                联系人
            </td>
            <td colspan="1" rowspan="1" class="list">
                <uc4:CtrFlowFormText ID="CtrFTLinkMan1" runat="server" MustInput="true" TextToolTip="联系人" />
            </td>
            <td class="listTitleRight ">
                URL
            </td>
            <td class="list">
                <asp:TextBox ID="txtWebSite" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                联系地址
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtAddress" runat="server" Width="72%" MaxLength="200"></asp:TextBox>
            </td>
        </tr>
        <tr id="trRight" runat="server" visible="false">
            <td class="listTitleRight ">
                访问权限
            </td>
            <td class="list" colspan="3">
                <uc3:ctrSetUserOtherRight ID="CtrSetUserOtherRight1" runat="server" />
            </td>
        </tr>
    </table>
    <table width="98%" align="center" runat="server" id="Table3" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_expandall.gif"
                                width="16" align="absbottom" />服务协议
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table2" width="98%" align="center" bordercolorlight="#000000" runat="server"
        style="display: none;" class="listContent">
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                服务协议
            </td>
            <td class='list' colspan="3">
                <ftb:FreeTextBox ID="txtContent" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                    Height="500px" ImageGalleryPath="Attfiles\\Photos" Width="100%">
                </ftb:FreeTextBox>
            </td>
        </tr>
    </table>
    <table width="98%" align="center" id="TableImg" runat="server" class="listNewContent">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            联系人
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table1" width="98%" align="center" bordercolorlight="#000000" runat="server"
        class="listContent" cellpadding="0">
        <tr>
            <td>
                <iframe id='Contract' name="Contract" src="frmBr_ContactMain.aspx?CustomID=<%=CustomID%>&CustomFlag=0"
                    width='100%' height='400' scrolling='no' frameborder='no'></iframe>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
</asp:Content>
