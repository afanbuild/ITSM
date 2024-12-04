<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmServiceProtocolShow.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmServiceProtocolShow"
    Title="配置信息" %>

<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc12" %>
<%@ Register Src="../Controls/CustSchemeCtr.ascx" TagName="CustSchemeCtr" TagPrefix="uc3" %>
<%@ Register Src="../Controls/DymSchemeCtrList.ascx" TagName="DymSchemeCtrList" TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc1" %>
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
      objectFullName = '<%=hidProvideID.ClientID%>';
      className = objectFullName.substring(0,objectFullName.indexOf("hidProvideID")-1);
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


function OpenProviderInfo(obj)
    {
        var ObjID = obj.id.replace("lblProvideName","hidProvideID");
        var ID = document.getElementById(ObjID).value;
        
        window.open("../Provide/frmPro_ProvideManageEdit.aspx?newWin=true&ReadyOnly=1&id=" + ID + "&FlowID=0","","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
	           
	    event.returnValue = false;
    }

function OpenEquRelChart()
   {
        window.open("../EquipmentManager/frm_Equ_RelChartView.aspx?newWin=true&id=" + <%=EquID%>,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
   } 
   
    function OpenEquHistoryChart()
   {
        window.open("../EquipmentManager/frm_Equ_HistoryChartView.aspx?newWin=true&id=" + <%=EquID%>,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
   } 

    </script>

    <table id="TableImg1" width="98%" align="center" runat="server" class="listNewContent"
        border="0" cellspacing="2" cellpadding="0">
        <tr id="tr2" runat="server">
            <td valign="bottom" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" width="16" src="../Images/icon_collapseall.gif"
                                align="absbottom" />
                            客户配置信息
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" border="0" cellspacing="1" cellpadding="1" class="listContent" runat="server"
        id="Table1">
        <tr>
            <td align="center" class="listTitleNoAlign" colspan="4" width="100%" style="height: 19px">
                <asp:Label ID="LblTitle" runat="server" Font-Size="12" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="lblMastShortName" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitFullName" runat="server" Text="英文名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblFullName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustomCode" runat="server" Text="客户代码"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustomCode" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustomerType" runat="server" Text="客户类型"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustomerType" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustDeptName" runat="server" Text="部门"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustDeptName" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitContact" runat="server" Text="联系人"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblContact" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCTel" runat="server" Text="联系电话"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCTel" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustAddress" runat="server" Text="联系地址"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustAddress" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustEmail" runat="server" Text="电子邮件"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:HyperLink ID="lblEmail" runat="server">[lblEmail]</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitRights" runat="server" Text="权限"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblRights" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitkhRemark" runat="server" Text="备注"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" style="word-break: break-all;" colspan="4">
                <div align="center" class="list">
                    <asp:Literal ID="LblContent" runat="server" Mode="PassThrough"></asp:Literal>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4" class="list">
                <uc3:CustSchemeCtr ID="CustSchemeCtr1" runat="server" ReadOnly="true" />
            </td>
        </tr>        
    </table>
    <br />
    <table id="TableImg2" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" width="16" src="../Images/icon_collapseall.gif"
                                align="absbottom" />
                            资产配置信息
                        </td>
                    </tr>
                </table>
            </td>
            <td class="listTitleNew" align="right">
                <input id="cmdOpenHistory" class="btnClass3" onclick="OpenEquHistoryChart();" type="button"
                    value="资产配置基线" />
                &nbsp;
                <input id="cmdOpenRelChart" class="btnClass3" onclick="OpenEquRelChart();" type="button"
                    value="资产关联图" />
            </td>
        </tr>
    </table>
    <table style="width: 98%" border="0" cellspacing="1" cellpadding="1" class="listContent" runat="server"
        id="Table2">
        <tr>
            <td align="center" class="listTitleNoAlign" colspan="4">
                <asp:Label ID="lblEqu" runat="server" Font-Size="12" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr style="display: none;">
            <td class="listTitleRight">
                <asp:Literal ID="LitListName" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblListName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%;">
                <asp:Literal ID="LitEquDeskName" runat="server" Text="名称"></asp:Literal>
            </td>
            <td class='list' style="width: 35%;">
                <asp:Label ID="lblEquDeskName" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight" style="width: 12%;">
                <asp:Literal ID="LitEquDeskCode" runat="server" Text="资产编号"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblCode" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustom" runat="server" Text="所属客户"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustom" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitEquDeskServiceTime" runat="server" Text="保修期"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblServiceBeginTime" runat="server" Text=""></asp:Label>~
                <asp:Label ID="lblServiceEndTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitPartBankName" runat="server" Text="维护机构"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblPartBankName" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitPartBranchName" Text="维护部门" runat="server"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblPartBranchName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitEquDeskEquStatus" runat="server" Text="资产状态"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblEquStatus" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight">
                扩展配置信息
            </td>
            <td class="list">
                <asp:Label ID="lblConfigureInfo" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table width="98%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td colspan="4" align="left">
                <div>
                    <uc2:DymSchemeCtrList ID="DymSchemeCtr1" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4" class="list">
                <uc12:ctrattachment ID="ctrattachment1" runat="server" />
            </td>
        </tr>
    </table>
    <table runat="server" id="tbMessage">
        <tr>
            <td>
                <asp:Label ID="lbMessage" runat="server" Text="没有相关数据！"></asp:Label>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidProvideID" runat="server" />
</asp:Content>
