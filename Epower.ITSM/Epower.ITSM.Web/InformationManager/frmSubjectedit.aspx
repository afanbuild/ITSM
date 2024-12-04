<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>

<%@ Page Language="c#" Inherits="Epower.ITSM.Web.InformationManager.frmSubjectedit"
    CodeBehind="frmSubjectedit.aspx.cs" EnableEventValidation="false" %>

<%@ Register Src="../Controls/ctrSetUserOtherRight.ascx" TagName="ctrSetUserOtherRight"
    TagPrefix="uc3" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>知识库类别管理</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
</head>
<script language="javascript" src="../Controls/Calendar/Popup.js"></script>
<script language="javascript" type="text/javascript" src="../Js/App_Base.js"></script>
<script language="javascript">
    function SelectPCatalog() {
        if (document.all.hidCatalogID.value == 1) {
            alert("已经是最顶层!");
            return;
        }
        //===========zxl==========
            window.open("frmpopSubject.aspx?CurrSubjectID=" + document.all.hidCatalogID.value+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>","","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=700,height=550px,left=150,top=50");
            // window.open(strUrl,'',"resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=700,height=550px,left=150,top=50");
        //================zxl==
    }

    function delete_confirm() {

        if (document.getElementById("cmdDelete").disabled == false) {
            if (event.srcElement.value == "删除")
                event.returnValue = confirm("确认要删除吗?");
        }
    }
    document.onclick = delete_confirm;
		
</script>

</head>
<body>
    <form id="Form1" method="post" runat="server">
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <table style="width: 100%" class="listContent">
        <tr>
            <td colspan="2" class="list">
                <uc1:CtrTitle ID="CtrTitle" runat="server"></uc1:CtrTitle>
            </td>
        </tr>
        <tr height="40" class="listTitle">
            <td colspan="2">
                <asp:Button ID="cmdAdd" runat="server" Text="新增" CssClass="btnClass" OnClick="cmdAdd_Click"
                    CausesValidation="false"></asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdSave" runat="server" Text="保存" CssClass="btnClass" OnClick="cmdSave_Click">
                </asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdDelete" runat="server" Text="删除" CssClass="btnClass" OnClick="cmdDelete_Click"
                    CausesValidation="false"></asp:Button>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                名称:
            </td>
            <td class="list">
                <asp:TextBox ID="txtSubjectName" runat="server" Width="273px"></asp:TextBox>
                <asp:Label ID="labMsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblSubjectName" runat="server" Text="*"  Font-Bold="True" Font-Size="Small"
                                ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                描述:
            </td>
            <td class="list">
                <asp:TextBox ID="txtDesc" runat="server" Width="273" Height="56px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                上级:
            </td>
            <td class="list">
                <asp:TextBox ID="txtPCatalogName" runat="server" Width="258" ReadOnly="True"></asp:TextBox><input
                    id="hidPCatalogID" style="width: 56px" type="hidden" runat="server" name="hidPCatalogID"><input
                        id="cmdPopParentCatalog" onclick="SelectPCatalog()" type="button" value="..."
                        class="btnClass2">
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                排序:
            </td>
            <td class="list">
                <font face="宋体">
                    <asp:TextBox ID="txtSortID" runat="server" Width="258px" Style="ime-mode: Disabled"
                        onblur="CheckIsnum(this,'排序必须为数值！');" onkeydown="NumberInput('');" MaxLength="9">-1</asp:TextBox></font>
            </td>
        </tr>
        <tr>
            <td class="listTitle" colspan="2">
                <font face="宋体">*注:排序为-1时,由系统指定其排序ID,有效值范围(-1 ~ 99999)</font>
            </td>
        </tr>
        <tr id="trRight" runat="server" visible="false">
            <td class="listTitleRight" style="height: 25px">
                访问权限
            </td>
            <td class="list" align="left" style="height: 25px">
                <uc3:ctrSetUserOtherRight ID="CtrSetUserOtherRight1" runat="server" OperateType="20"
                    OperateID="0" />
            </td>
        </tr>
    </table>
    
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    <input id="hidOrgID" type="hidden" runat="server" name="hidOrgID">
    <input id="hidCatalogID" type="hidden" runat="server" name="hidCatalogID">
    </form>
</body>
</html>
