<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUserFlowModels.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmUserFlowModels" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" border="0" >
            <tr>
                <td style="width: 100%"  class="listContent">
                    <asp:DataGrid ID="grdFlowModel" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  BorderColor="White"
                        BorderWidth="0px" CellPadding="1" CellSpacing="2" PageSize="16" ShowHeader="False" OnItemDataBound="grdFlowModel_ItemDataBound"
                        Width="100%">
                        <AlternatingItemStyle BackColor="Azure" />
                        <ItemStyle BackColor="White" CssClass="tablebody" />
                        <HeaderStyle CssClass="listTitle" />
                        <Columns>
                            <asp:BoundColumn DataField="FlowName" ItemStyle-Width="100%" HeaderText="流程名称">
                                <HeaderStyle Width="100%"  />
                            </asp:BoundColumn>
                            <asp:TemplateColumn ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <input id="cmdStart" class="btnClass1" onclick='window.open("../Forms/OA_AddNew.aspx?flowmodelid=<%#DataBinder.Eval(Container.DataItem, "FlowModelID")%>","MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));'
                                        type="button" value="申 请" style="width: 48px">
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-Width="60px">
                                <ItemTemplate >
                                    <font face="宋体">
                                        <input id="cmdViewChart" class="btnClass1" onclick='window.open("../Forms/flow_View_ChartModel.aspx?flowmodelid=<%#DataBinder.Eval(Container.DataItem, "FlowModelID")%>","" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));'
                                            type="button" value="查看流程图"></font>
                                </ItemTemplate>
                                <HeaderStyle Width="60px" />
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
