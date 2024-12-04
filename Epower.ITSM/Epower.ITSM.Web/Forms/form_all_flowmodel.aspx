<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Epower.ITSM.Web.Forms.form_All_FlowModel" Title="流程模型清单" CodeBehind="form_All_FlowModel.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function AddNewFlow(id)  //删除流程
        {
            var isnew = '<%=IsNewWin %>';
            var isstrExtPara = '<%=strExtPara %>';
            if (isnew == 'False') {
                if (isstrExtPara != '') {
                    window.open("OA_AddNew.aspx?flowmodelid=" + id + "&ep=" + isstrExtPara, "MainFrame", "scrollbars=yes,resizable=yes,top=0,left=0,width=" + (window.availWidth - 12) + ",height=" + (window.availHeight - 35));
                }
                else {
                    window.open("OA_AddNew.aspx?flowmodelid=" + id, "MainFrame", "scrollbars=yes,resizable=yes,top=0,left=0,width=" + (window.availWidth - 12) + ",height=" + (window.availHeight - 35));
                }
            }
            else {
                if (isstrExtPara != '') {
                    window.open("OA_AddNew.aspx?NewWin=true&flowmodelid=" + id + "&ep=" + isstrExtPara, "_self", "scrollbars=yes,resizable=yes,top=0,left=0,width=" + (window.availWidth - 12) + ",height=" + (window.availHeight - 35));
                }
                else {
                    window.open("OA_AddNew.aspx?NewWin=true&flowmodelid=" + id, "_self", "scrollbars=yes,resizable=yes,top=0,left=0,width=" + (window.availWidth - 12) + ",height=" + (window.availHeight - 35));
                }
            }
        }
    
    </script>
    <br />
    <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">        
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
        <tr>
            <td align="center">
                <table cellpadding="0" cellspacing="0" width="98%" border="0">
                    <tr>
                        <td align="center">
                            <asp:DataGrid ID="grdFlowModel" runat="server" PageSize="16" AutoGenerateColumns="False"  CssClass="Gridtable"  Width="100%">
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
                                                <input id="cmdViewChart" class="btnClass1" onclick='window.open("flow_View_ChartModel.aspx?flowmodelid=<%#DataBinder.Eval(Container.DataItem, "FlowModelID")%>","" ,"scrollbars=yes,resizable=yes,top=0,left=0,fullscreen=1");'
                                                    type="button" value="查看流程图"></font>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 100%">
                            <pagerstyle nextpagetext="下一页" prevpagetext="上一页" horizontalalign="Left" forecolor="#003399"
                                backcolor="#99CCCC" mode="NumericPages"></pagerstyle>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
