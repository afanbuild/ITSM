<%@ Page Language="C#" Title="问题单高级查询" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="frmProblem_AdvancedCondition.aspx.cs"
 Inherits="Epower.ITSM.Web.ProbleForms.frmProblem_AdvancedCondition" %>

<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
   <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">
        function GetRandom() {
            return Math.floor(Math.random() * 1000 + 1);
        }
    </script>
        <table>
            <tbody>
            </tbody>
            <tr>
                <td><table class="listContent" id="Table2" border="0" cellpadding="1" cellspacing="1">
            <tr>
                <td colspan="4" align="center" class="list">
                    <strong style=" font-weight:bold; font-size:20px; color:#08699E;"> 问题单高级查询</strong> 
                </td>
            </tr>
            <tr>
                <td nowrap align="right" class="listTitle">
                    选择收藏查询条件
                </td>
                <td align="left" class="list" colspan="1">
                        <asp:DropDownList ID="DropSQLwSave" runat="server"  Width="152px"
                        AutoPostBack="True" onselectedindexchanged="DropSQLwSave_SelectedIndexChanged"></asp:DropDownList>                                           
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
                <td class="listTitleRight" style="width: 12%">
                    <asp:Literal ID="litProblemNo" runat="server" Text="问题单号"></asp:Literal>
                </td>
                <td class="list" style="width:35%">
                    <asp:TextBox ID="txtProblemNo" runat="server"></asp:TextBox>
                </td>
                <td class="listTitleRight" style="width: 12%">
                    <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称"></asp:Literal>
                </td>
                <td class="list">
                    <asp:TextBox ID="txtEquName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="listTitleRight">
                    流程状态
                </td>
                <td class="list">
                    <asp:DropDownList ID="cboStatus" runat="server" Width="152px">
                    </asp:DropDownList>
                </td>
                <td class="listTitleRight">
                    <asp:Literal ID="litProbleState" runat="server" Text="问题状态"></asp:Literal>
                </td>
                <td class="list">
                    <uc2:ctrFlowCataDropListNew ID="CtrDealStatus" runat="server" RootID="1021"/>
                </td>
            </tr>
            <tr>
                <td class="listTitleRight">
                    <asp:Literal ID="litProbleRegTime" runat="server" Text="登记时间"></asp:Literal>
                </td>
                <td class="list">
                    <uc4:ctrDateSelectTime ID="ctrDateReSetTime" runat="server" />
                </td>
                <td class="listTitleRight">
                    <asp:Literal ID="litProblemTitle" runat="server" Text="摘要"></asp:Literal>
                </td>
                <td class="list">
                    <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                </td>                
            </tr>
            <tr>
                <td class="listTitleRight">
                    <asp:Literal ID="litProbleType" runat="server" Text="问题类别"></asp:Literal>
                </td>
                <td class="list">
                    <uc2:ctrFlowCataDropListNew ID="CataProblemType" runat="server" RootID="1006" />
                </td>
                <td class="listTitleRight">
                    <asp:Literal ID="litProbleLevel" runat="server" Text="问题级别"></asp:Literal>
                </td>
                <td class="list">
                    <uc2:ctrFlowCataDropListNew ID="CataProblemLevel" runat="server" RootID="1007" />
                </td>
            </tr>
            <tr>
                <td class="listTitleRight">
                    <asp:Literal ID="litProbleEffect" runat="server" Text="影响度"></asp:Literal>
                </td>
                <td class="list">
                    <uc2:ctrFlowCataDropListNew ID="CtrFCDEffect" runat="server" RootID="1028"  />
                </td>
                <td class="listTitleRight">
                    <asp:Literal ID="litProbleInstancy" runat="server" Text="紧急度"></asp:Literal>
                </td>
                <td class="list">
                    <uc2:ctrFlowCataDropListNew ID="CtrFCDInstancy" runat="server" RootID="1029"  />
                </td>
            </tr>
            <tr>
                <td class="listTitleRight">
                    <asp:Literal ID="litProbleRegUserName" runat="server" Text="登记人"></asp:Literal>
                </td>
                <td class="list">
                    <asp:TextBox ID="txtRegUser" runat="server"></asp:TextBox>
                </td>
                <td align="right" class="listTitle">
                    <asp:Literal ID="Literal3" runat="server" Text="查询条件名称"></asp:Literal>
                </td>
                <td align="left" class="list">
                    <asp:TextBox ID="txtSQLName" runat="server"></asp:TextBox>
                    <asp:Button ID="chkSave" runat="server" Text="保存" CssClass="btnClass" 
                        onclick="chkSave_Click"  />
                    <asp:Button ID="btn_delete" runat="server" Text="删除" CssClass="btnClass"  
                        onclick="btn_delete_Click"   />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" class="list">
                    <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="btnClass" 
                        onclick="btnOK_Click"  />
                    <asp:Button ID="btnClose" runat="server" Text="关闭" CssClass="btnClass" 
                        onclick="btnClose_Click"   />
                </td>
            </tr>
        </table></td>
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
            </tr>
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