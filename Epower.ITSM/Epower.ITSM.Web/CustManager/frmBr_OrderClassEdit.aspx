<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmBr_OrderClassEdit.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmBr_OrderClassEdit"
    Title="排班表编辑" %>

<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc6" %>
<%@ Register TagPrefix="uc2" TagName="ctrdateandtime" Src="../Controls/CtrDateAndTimeV2.ascx" %>
<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ctrFlowCataDropList" Src="../Controls/ctrFlowCataDropList.ascx" %>
<%@ Register TagPrefix="uc5" TagName="CtrFlowNumeric" Src="../Controls/CtrFlowNumeric.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/UserPickerMult.ascx" tagname="UserPickerMult" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript">
            //工程师选择
			function ClassTypeSelect(obj) 
			{
			      window.open("../CustManager/frmBr_OrderClassTypeSel.aspx?IsSelect=true&randomid="+GetRandom(),"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=100");
			}
</script>

    <table style='width: 98%' cellpadding="2" cellspacing="0" class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>                
                值班人
            </td>
            <td class='list'  style='width: 35%;'>                
                <uc1:UserPickerMult ID="UserPickerMult1" runat="server" MustInput="true" TextToolTip="值班人" />                
            </td>
            <td class='listTitleRight' align='right' style='width: 12%;'>
                班次
            </td>
            <td class='list'>
                <asp:TextBox ID='txtClassTypeName' runat='server'></asp:TextBox>
                <input id="cmdClassType" onclick="ClassTypeSelect(this)" type="button" value="..." runat="server"
                    class="btnClass2" />
                <input id="HidClassTypeID" runat="server" type="hidden" value="0" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' align='right' style='width: 12%;'>
                值班日期
            </td>
            <td class='list' colspan="3">                
                    <uc2:ctrdateandtime ID="ctrDutyTime" runat="server" MustInput="true" ShowTime="false" TextToolTip="值班日期" />
                <div id="divTime" runat="server">
                    <uc2:ctrdateandtime ID="ctrBeginDutyTime" runat="server" MustInput="true" ShowTime="false" TextToolTip="值班开始日期" />
                    ~
                    <uc2:ctrdateandtime ID="ctrEndDutyTime" runat="server" MustInput="true" ShowTime="false" TextToolTip="值班结束日期" />             
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
