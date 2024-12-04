<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.frmEditRightBatch" CodeBehind="frmEditRightBatch.aspx.cs" %>

<%@ Register Src="../Controls/DeptPickerMult.ascx" TagName="DeptPickerMult" TagPrefix="uc2" %>
<%@ Register Src="../Controls/UserPickerMult.ascx" TagName="UserPickerMult" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrOperates" Src="../Controls/CtrOperates.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrRight" Src="../Controls/CtrRight.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>权限批量设置</title>
     <base target="_self" />

    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

    

</head>
<script language="javascript">
        function CanRead_Click(ctl) {
            reg = /chkCanRead/g;
            var idtag = ctl.id.replace(reg, "");
            if (ctl.checked == false) {
                document.all(idtag + "chkCanAdd").checked = false;
                document.all(idtag + "chkCanModify").checked = false;
                document.all(idtag + "chkCanDelete").checked = false;
            }
        }


        function SelectOperates() {
            var value = window.showModalDialog("frmPopOperates.aspx?OperateID=" + document.all.txtOperateId.value);
            if (typeof (value) != "undefined" && value.length > 1) {
                arr = value.split("@");
                document.all.txtOperateName.value = arr[1] + "(" + arr[0] + ")";
                document.all.txtOperateId.value = arr[0];
            }

        }



        function PopSelectDialog() {
           // debugger;
            switch (document.all.dpdObjectType.value) {
                case "10":
                    SelectPDept();
                    break;

                case "20":
                    SelectStaff();
                    break;

                case "30":
                    SelectActor();
                    break;
            }
        }


        function AddExtDeptList() {
            var value = window.showModalDialog("frmpopdept.aspx");
            if (typeof (value) != "undefined" && value.length > 1) {
                arr = value.split("@");
                if (document.all.txtDeptNames.value == "") {
                    document.all.txtDeptNames.value = arr[1];
                }
                else {
                    document.all.txtDeptNames.value = document.all.txtDeptNames.value + ";" + arr[1];
                }
                if (document.all.hidDeptList.value == "") {
                    document.all.hidDeptList.value = arr[0];
                }
                else {
                    document.all.hidDeptList.value = document.all.hidDeptList.value + "," + arr[0];
                }
            }
        }

        function SelectPDept() {
        
        var  url="frmpopdept.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmeditrightbatch";
        window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300,left=150,top=100");       
        }

        function SelectStaff() {
        
            var value = window.showModalDialog("frmpopstaff.aspx");
            if (typeof (value) != "undefined" && value.length > 1) {
                arr = value.split("@");
                document.all.txtObjectName.value = arr[1] + "(" + arr[0] + ")";
                document.all.txtObjectId.value = arr[0];

                document.all.hidObjectName.value = arr[1] + "(" + arr[0] + ")";
            }
            else {
                document.all.hidObjectName.value = "";
            }
        }

        function SelectActor() {
            //========zxl
              var url="frmPopActor.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmeditrightbatch";
		    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100");
        }

        function clearDeptList() {
            document.all.txtDeptNames.value = "";
            document.all.hidDeptList.value = "";
        }

        function Selectchange() {
            switch (document.all.dpdObjectType.value) {
                case "10":   //部门
                    document.all.divdept.style.display = "";
                    document.all.divuser.style.display = "none";
                    document.all.divactor.style.display = "none";
                    //SelectPDept();
                    break;

                case "20":  //用户
                    document.all.divdept.style.display = "none";
                    document.all.divuser.style.display = "";
                    document.all.divactor.style.display = "none";
                    //SelectStaff();
                    break;

                case "30":  //用户组
                    document.all.divdept.style.display = "none";
                    document.all.divuser.style.display = "none";
                    document.all.divactor.style.display = "";
                    //SelectActor();
                    break;
            }
        }

    </script>
<body>
    <form id="Form1" method="post" runat="server">
    <input id="hidClientId_ForOpenerPage" type="hidden" value="0" runat="server" />
    <input id="hidObjectName" runat="server" type="hidden" />
    <table id="tbMain" class="listContent">
        <tr>
            <td class="listTitle" nowrap>
                权限对象类别:
            </td>
            <td class="list">
                <asp:DropDownList ID="dpdObjectType" runat="server" onchange="Selectchange();">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="ObjectTypeRequired" runat="server" ControlToValidate="dpdObjectType"
                    ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="listTitle">
                对象ID:
            </td>
            <td class="list">
                <div id="divactor">
                    <asp:TextBox ID="txtObjectId" runat="server" Width="0px"></asp:TextBox><asp:TextBox
                        ID="txtObjectName" runat="server"  ReadOnly="True"></asp:TextBox><input
                            id="cmdPop" onclick="PopSelectDialog()" type="button" value="..." class="btnClass2"></div>
                <div id="divdept">
                    <uc2:DeptPickerMult ID="CtrDeptMult" runat="server" IsLimit="true" />
                </div>
                <div id="divuser">
                    <uc3:UserPickerMult ID="CtrUserMult" runat="server" IsLimit="true" />
                </div>
            </td>
        </tr>
        <tr runat="server" id="tr_RightType">
            <td class="listTitle">
                权限类别:
            </td>
            <td class="list">
                <asp:DropDownList ID="ddltRightType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltRightType_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" id="tr_Operates">
            <td class="listTitle">
                操作项:
            </td>
            <td class="list">
                <uc1:CtrOperates ID="CtrOperates1" runat="server"></uc1:CtrOperates>
            </td>
        </tr>
        <tr>
            <td style="height: 1px" class="listTitle">
                权限范围:
            </td>
            <td style="height: 1px" class="list">
                <asp:DropDownList ID="dpdRightRange" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="display: none;">
            <td class="listTitle">
                <font face="宋体">扩展范围</font>
            </td>
            <td class="list">
                <asp:TextBox ID="txtDeptNames" runat="server" Width="268px" ReadOnly="True"></asp:TextBox><input
                    id="Button1" onclick="AddExtDeptList()" type="button" value="添加" name="Button1"
                    class="btnClass"><input id="Button2" onclick="clearDeptList()" type="button" value="清除"
                        name="Button2" class="btnClass">
            </td>
        </tr>
        <tr>
            <td class="listTitle">
                权限值:
            </td>
            <td class="list">
                <uc1:CtrRight ID="uRight" runat="server"></uc1:CtrRight>
            </td>
        </tr>
        <tr height="20">
            <td colspan="2" class="listTitle">
                <font color="#006600">!注:对分析类操作,在权限设定中,只对"可读"选项有效,"可读"=允许操作,反之不允许操作</font>
            </td>
        </tr>
        <tr>
            <td class="list" align="center" colspan="2">
                <asp:Button ID="cmdSave" runat="server" Text="保存" OnClick="cmdSave_Click" CssClass="btnClass">
                </asp:Button>
                <input id="cmdExit" onclick="javascript:window.close()" type="button" value="取消"
                    class="btnClass">
            </td>
        </tr>
    </table>
    <input id="hidDeptList" runat="server" type="hidden" name="hidDeptList">
    </form>

    <script type="text/javascript" language="javascript">
        Selectchange();
    </script>

</body>
</html>
