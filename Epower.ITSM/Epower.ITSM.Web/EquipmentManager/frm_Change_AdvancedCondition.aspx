<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="frm_Change_AdvancedCondition.aspx.cs"
 Inherits="Epower.ITSM.Web.EquipmentManager.frm_Change_AdvancedCondition" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc1" %>
    <%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc11" %>
     <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
//设备
function SelectEqu(obj) 
{
    var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value;
        			        
    var EquName = document.all.<%=txtEquipmentName.ClientID%>.value;
    var CustName = "";
    var MastCust = "";
   
    var url="../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&TypeFrm=frm_Change_AdvancedCondition&randomid="+GetRandom()+"&FlowID="+ '<%=FlowID%>' + "&EquName=" + escape(EquName) + "&Cust=" + escape(CustName) + "&MastCust=" + escape(MastCust)+"&EquipmentCatalogID="+EquipmentCatalogID+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
    window.open(url,"",'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50'); 
}


//选择资产目录
function SelectListName(obj)
{
    var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskCateListSel.aspx?random=" + GetRandom(),"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
    if(value != null)
    {
        if(value.length>1)
        {			                
            document.getElementById(obj.id.replace("cmdListName","txtEquipmentDir")).value = value[1];   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListName")).value = value[1];   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = value[0];  //资产目录ID
        }
        else
        {			                
            document.getElementById(obj.id.replace("cmdListName","txtEquipmentDir")).value = "";   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";  //资产目录ID
        }
    }
    else
    {			                
            document.getElementById(obj.id.replace("cmdListName","txtEquipmentDir")).value = "";   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";  //资产目录ID
    }
}

    function GetRandom() {
        if (confirm)
            return Math.floor(Math.random() * 1000 + 1);
    }
    //删除确认
    function ConfirmDel() { 
        var ddlSQL=document.all.<%=DropSQLwSave.ClientID %>;//获取条件名称
        var strValue=ddlSQL.options[ddlSQL.selectedIndex].value;//条件值

        if(strValue!="0"){
            if(confirm("确认删除？")){
                return true;
            }
        }else{
            alert("请选择查询条件名称！");
            ddlSQL.focus();
            return false;
        }
        return false;
    }
    
    //保存确认
    function ConfirmSave()
    {
        var txtSqlName=document.all.<%=txtSQLName.ClientID %>;//获取条件名称
        var strVale=txtSqlName.value;//获取值

        if(strVale.trim()!=""){
            return true;
        }else{
            alert("请输入查询条件名称！");
            txtSqlName.focus();
            return false;
        }
        return false;
    }
    
	String.prototype.trim = function()  //去空格

	{
		// 用正则表达式将前后空格

		// 用空字符串替代。

		return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;","");
	}
</script>

    <table>
        <tbody>
            <tr>
                <!--Begin: 高级查询表单-->
                <td style="width:700px;">
                    <table class="listContent" width="100%" id="Table2">
        <tr>
            <td colspan="6" align="center" class="list">
                <strong style="font-weight: bold; font-size: 20px; color: #08699E;">变更单高级查询</strong>
            </td>
        </tr>
        <tr>
            <td nowrap align="right" class="listTitle">
                选择收藏查询条件
            </td>
            <td align="left" class="list" colspan="0" align="justify" style="height:45px;">
                <asp:DropDownList ID="DropSQLwSave" runat="server" Width="152px" AutoPostBack="True" 
                    OnSelectedIndexChanged="DropSQLwSave_SelectedIndexChanged">
                    <asp:ListItem Value="0">==请选择收藏条件==</asp:ListItem>
                </asp:DropDownList>
            </td>
                            <td  nowrap align="right" class="listTitle">
                  自定义字段
                </td>
                <td  nowrap align="left" class="list">
                        
                    <input id="btn_columns" type="button" value="显示字段" class="btnClass" /> </td>
        </tr>
        <tr style="display:none;">
            <td align="right" class="listTitle" >
            选择字段
            </td>
            <td align="left" class="list"  colspan="3">
            
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle" nowrap="nowrap" style="width:12%">
                <asp:Literal ID="litChangeFTSubject" runat="server" Text="摘要"></asp:Literal>
            </td>
            <td class="list" nowrap="nowrap" style="width:35%">
                <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
            </td>
            <td align="right" class="listTitle" nowrap="nowrap" style="width:12%">
                流程状态
            </td>
            <td class="list" nowrap="nowrap">
                <asp:DropDownList ID="cboStatus" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle" nowrap="nowrap">
                <asp:Literal ID="LitbgCustName" runat="server" Text="客户名称"></asp:Literal>
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtCustInfo" runat="server"></asp:TextBox>
            </td>
            <td nowrap class="listTitle" align="right">
                <asp:Literal ID="litChangeDealStatus" runat="server" Text="变更状态"></asp:Literal>
            </td>
            <td class="list" align="left">
                <uc2:ctrFlowCataDropListNew ID="CtrDealStatus" runat="server" RootID="1022" />
            </td>            
        </tr>
        <tr>
            <td class="listTitle" align="right">
                <asp:Literal ID="litChangeCustTime" runat="server" Text="变更时间"></asp:Literal>
            </td>
            <td class="list" align="left">
                <uc1:ctrDateSelectTime ID="ctrDateSelectTime1" runat="server" />
            </td>
            <td nowrap align="right" class="listTitle">
                <asp:Literal ID="litChangelevel" runat="server" Text="变更级别"></asp:Literal>
            </td>
            <td align="left" class="list">
                <uc2:ctrFlowCataDropListNew ID="CtrFCDlevel" runat="server" RootID="1025" />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle" nowrap="nowrap">
                <asp:Literal ID="litChangeEffect" runat="server" Text="影响度"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropListNew ID="CtrFCDEffect" runat="server" RootID="1026" />
            </td>
            <td align="right" class="listTitle" nowrap="nowrap">
                <asp:Literal ID="litChangeInstancy" runat="server" Text="紧急度"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropListNew ID="CtrFCDInstancy" runat="server" RootID="1027" />
            </td>
        </tr>
        <tr style="display:none;">
            <td align="right" class="listTitle" nowrap="nowrap">
                <asp:Literal ID="Literal1" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtEquipmentDir" runat="server" Width="152px"></asp:TextBox>
                <input id="cmdListName" onclick="SelectListName(this)" type="button" value="..."
                    runat="server" name="cmdListName" class="btnClass2" />
                <input id="hidListName" value="" type="hidden" runat="server" />
                <input id="hidListID" value="0" type="hidden" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle" nowrap="nowrap">
                <asp:Literal ID="LitbgEquName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtEquipmentName" runat="server" Width="152px"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
                    <asp:HiddenField ID="hidClientId_ForOpenerPage" runat="server" />
                <input id="hidEquName" type="hidden" runat="server" />
                <input id="hidEqu" type="hidden" runat="server" value="-1" />
            </td>
             <td align="right" class="listTitle" >
                <asp:Literal ID="litChangeType" runat="server" Text="变更类别"></asp:Literal>
            </td>
            <td align="left" class="list" >
                <uc11:ctrFlowCataDropListNew ID="CtrChangeType" runat="server" RootID="1033"/>
            </td> 
                                 
        </tr>
        <tr>
             <td align="right" class="listTitle">
                <asp:Literal ID="Literal3" runat="server" Text="查询条件名称"></asp:Literal>
            </td>
            <td align="left" class="list" colspan="3">
                <asp:TextBox ID="txtSQLName" runat="server"></asp:TextBox>
                <asp:Label ID="rWarning" runat="server" Style="margin-left:7px;" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btnClass" OnClick="btnSave_Click" />
                <asp:Button ID="btn_delete" runat="server" Text="删除" CssClass="btnClass" OnClick="btn_delete_Click" />
            </td>  
        </tr>
        <tr>
            <td colspan="4" align="center" class="list">
                <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="btnClass"
                        OnClick="btnOK_Click" />
                <asp:Button ID="btnClose" runat="server" Text="关闭"  CssClass="btnClass" 
                    onclick="btnClose_Click"   />
            </td>
         </tr>
    </table>
                </td>
                <!--End: 高级查询表单-->
                
                <!--Begin: 显示字段-->                       
                <td>                
                    <div id="_DisplayColumnDiv" style="display: none; top: 0; left: 601px; width: 375px;
            height: 280px">
                        <table>
                            <tr style="height: 280px">
                                <td valign="top" style="height: 280px">
                                    <asp:ListBox ID="_TableColumnCheckBoxList" runat="server" Height="280px" Width="150px" SelectionMode="Multiple">
                                    </asp:ListBox>
                                </td>
                                <td valign="middle" align="center" class="listTitle">
                                    <input class="FLOWBUTTON" id="btnAdd" title="->" style="width: 44px; height: 24px"
                                        type="button" size="20" value="选择" name="btnAdd" runat="server"><br />
                                    <br />
                                    <input class="FLOWBUTTON" id="btnRemove" title="<-" style="width: 44px" type="button"
                                        size="20" value="移除" name="btnRemove" runat="server"><br />
                                    <br />
                                    <input class="FLOWBUTTON" id="btnClear" title="<<" style="width: 44px" type="button"
                                        size="30" value="清除" name="cmdSelect" runat="server">
                                </td>
                                <td style="width: 270px" valign="top">
                                    <asp:ListBox ID="lsbDeptTo" Width="150px" runat="server" Height="280px">
                                    </asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </div>                
                </td>
                <!--End: 显示字段-->
            </tr>
        </tbody>
    </table>
    
<input type="hidden" id="hidDeptID" runat="server" />
<input type="hidden" id="hidDeptName" runat="server" />   
    
<!--Begin: 初始化基础脚本-->
<script src="../js/epower.base.js" type="text/javascript"></script>
<script type="text/javascript">    
    epower.advance_search = {};
    epower.advance_search.deptIdCtrlID = '<%=hidDeptID.ClientID %>';
    epower.advance_search.deptNameCtrlID = '<%=hidDeptName.ClientID %>';     
</script>
<!--Begin: 初始化基础脚本-->

<!--Begin: 事件高级搜索脚本-->
<script src="../js/epower.appforms.advance_search.js" type="text/javascript"></script>
<!--End: 事件高级搜索脚本-->
</asp:Content>