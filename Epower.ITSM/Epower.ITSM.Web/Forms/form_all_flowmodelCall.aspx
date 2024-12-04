<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Epower.ITSM.Web.Forms.form_all_flowmodelCall" Title="流程模型清单" CodeBehind="form_all_flowmodelCall.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
    function AddNewFlow(id)  //删除流程
    {
        var isnew = '<%=IsNewWin %>';
        var isstrExtPara = '<%=strExtPara %>';
        if(isnew == 'False')
        {
            if(isstrExtPara !='')
            {
                window.open("OA_AddNew.aspx?flowmodelid=" + id + "&ep=" + isstrExtPara,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
            }
            else
            {
                window.open("OA_AddNew.aspx?flowmodelid=" + id,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
            }
        }
        else
        {
            if(isstrExtPara !='')
            {
                window.open("OA_AddNew.aspx?NewWin=true&flowmodelid=" + id + "&ep=" + isstrExtPara,"_self" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
            }
            else
            {
                window.open("OA_AddNew.aspx?NewWin=true&flowmodelid=" + id,"_self" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
            }
        }
        window.close();
    }
    
    </script>

    <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
        <tr>
            <td align="center">
                <font face="宋体">
                    <p>
                        <uc1:CtrTitle ID="CtrTitle1" runat="server"></uc1:CtrTitle>
                    </p>
                    <p>
                        &nbsp;</p>
                </font>
            </td>
        </tr>
        <tr id="trCust" runat="server">
            <td>
                <table id="Table12" width="100%" align="center" runat="server" class="listNewContent">
                    <tr>
                        <td valign="top" align="left" class="listTitleNew" style="width: 92%">
                            客户信息
                        </td>
                    </tr>
                </table>
                <table class="listContent" width="100%" align="center" runat="server" id="Table1">
                    <tr>
                        <td style="width: 99px" class="listTitle" nowrap="nowrap">
                            <asp:Literal ID="LitCustName" runat="server" Text="用户名称"></asp:Literal>
                        </td>
                        <td class="list" style="width:35%">
                            <asp:Label ID="labCustAddr" runat="server"></asp:Label>
                        </td>
                        <td style="width: 99px" class="listTitle">
                            <asp:Literal ID="LitCustAddress" runat="server" Text="用户地址"></asp:Literal>
                        </td>
                        <td class="list" style="width: *">
                            <asp:Label ID="lblAddr" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap align="left" class="listTitle">
                            <asp:Literal ID="LitContact" runat="server" Text="联系人"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="labContact" runat="server"></asp:Label>
                        </td>
                        <td nowrap class="listTitle">
                            <asp:Literal ID="LitCTel" runat="server" Text="联系电话"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="labCTel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="listTitle" nowrap="nowrap">
                            <asp:Literal ID="LitCustDeptName" runat="server" Text="用户部门"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="lblCustDeptName" runat="server"></asp:Label>
                        </td>
                        <td class="listTitle" nowrap="nowrap">
                            <asp:Literal ID="litCustEmail" runat="server" Text="电子邮件"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="listTitle" nowrap="nowrap">
                            <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
                        </td>
                        <td class="list" style="height: 23px">
                            <asp:Label ID="lblMastCust" runat="server"></asp:Label>
                        </td>
                        <td align="left" class="listTitle" nowrap="nowrap">
                            职位
                        </td>
                        <td class="list" style="height: 23px">
                            <asp:Label ID="lbljob" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table cellpadding="0" cellspacing="0" width="98%" border="0">
                    <tr>
                        <td align="center" class="listContent">
                            <asp:DataGrid ID="grdFlowModel" runat="server" PageSize="16" AutoGenerateColumns="False"  CssClass="Gridtable" 
                                CellPadding="1" CellSpacing="2" BorderWidth="0px" BorderColor="White" 
                                Width="100%" onitemdatabound="grdFlowModel_ItemDataBound">
                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                <HeaderStyle CssClass="listTitle"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="FlowName" HeaderText="流程名称">
                                        <HeaderStyle Width="40%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Remark" ReadOnly="True" HeaderText="备注">
                                        <HeaderStyle Width="40%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="申请">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemTemplate>
                                            <input id="cmdStart" class="btnClass1" onclick='AddNewFlow(<%#DataBinder.Eval(Container.DataItem, "FlowModelID")%>)'
                                                type="button" value="申请" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="查看流程图">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemTemplate>
                                            <font face="宋体">
                                                <input id="cmdViewChart" class="btnClass1" onclick='window.open("flow_View_ChartModel.aspx?flowmodelid=<%#DataBinder.Eval(Container.DataItem, "FlowModelID")%>","" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));'
                                                    type="button" value="查看流程图"></font>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="FlowModelID" HeaderText="流程名称" Visible="false">
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitle" align="right" style="width: 100%">
                            <pagerstyle nextpagetext="下一页" prevpagetext="上一页" horizontalalign="Left" forecolor="#003399"
                                backcolor="#99CCCC" mode="NumericPages"></pagerstyle>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
