<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmBr_ECustomerEdit.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmBr_ECustomerEdit"
    Title="�ͻ����ϱ༭" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc8" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CustSchemeCtr.ascx" TagName="CustSchemeCtr" TagPrefix="uc3" %>
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

    <table width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            �ͻ�������Ϣ
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" cellpadding="2" cellspacing="0" class="listContent GridTable" runat="server"
        id="Table1">
        <tr id="ShowRefUser" runat="server">
            <td class="listTitleRight " style="width: 12%;" id="tdRefUserTitle" runat="server">
               <asp:Literal ID="Custom_User"  runat="server" Text="��Ӧ�û�"></asp:Literal>
            </td>
            <td class="list" style="width: 35%;" runat="server" id="tdRefUser">
                <uc8:UserPicker ID="RefUser" runat="server" MustInput="false" TextToolTip="�ǵ���">
                </uc8:UserPicker>
            </td>
            <td class="listTitleRight " style="width: 12%;">
                <asp:Literal ID="Custom_MastCustName" runat="server" Text="����λ"></asp:Literal>
            </td>
            <td class="list" id="tdMast" runat="server">
                <asp:DropDownList ID="ddltMastCustID" runat="server" Width="152px">
                </asp:DropDownList>
                <asp:Label ID="rWarning" runat="server"
                        Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_CustDeptName" runat="server" Text="����"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtCustDeptName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_CustName" runat="server" Text="�ͻ�����"></asp:Literal>
            </td>
            <td class="list">
                <uc4:CtrFlowFormText ID="CtrFTShortName" runat="server" MustInput="true" TextToolTip="�ͻ�����"
                    MaxLength="50" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_CustomCode" runat="server" Text="�ͻ�����"></asp:Literal>
            </td>
            <td class="list">
                <uc4:CtrFlowFormText ID="txtCustomCode" runat="server" MustInput="true" TextToolTip="�ͻ�����"
                    MaxLength="50" />
            </td>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_FullName" runat="server" Text="Ӣ������"></asp:Literal>
            </td>
            <td class="list">
                <uc4:CtrFlowFormText ID="CtrFTFullName" runat="server" TextToolTip="Ӣ������" MaxLength="50" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_CustEmail" runat="server" Text="�����ʼ�"></asp:Literal>
            </td>
            <td colspan="1" rowspan="1" class="list">
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="�����ʼ�����" ValidationExpression="^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$"></asp:RegularExpressionValidator>
            </td>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_CustomerType" runat="server" Text="�ͻ�����"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="ctrFCDWTType" runat="server" ContralState="eNormal"
                    RootID="1019" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_Contact" runat="server" Text="��ϵ��"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtLinkMan1" runat="server" MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_Job" runat="server" Text="ְλ"></asp:Literal>
            </td>
            <td class="list">
                <uc4:CtrFlowFormText ID="CtrFlowJob" runat="server" MaxLength="50" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_CTel" runat="server" Text="��ϵ�绰"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtTel1" runat="server" MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_CustAddress" runat="server" Text="��ϵ��ַ"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtAddress" runat="server" MaxLength="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_Rights" runat="server" Text="Ȩ��"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtRights" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="Custom_Remark" runat="server" Text="��ע"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtRemark" runat="server" Width="90%" Rows="3" TextMode="MultiLine"
                    onblur="MaxLength(this,200,'��ע���ȳ����޶�����:');"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4" class="list">
                <uc3:CustSchemeCtr ID="CustSchemeCtr1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
