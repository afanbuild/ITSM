<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportDe.aspx.cs" Inherits="Epower.ITSM.Web.Common.ReportDe" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
   <form id="form1" runat="server">
 <table id="Table1" runat="server" style="width:100%;">
        <tr runat="server" id="_DetailTR" >
			<td align="center" colspan="3" >
			 <asp:DataGrid ID="gridIssue" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="1" BorderWidth="1px" 
                    onitemdatabound="gridIssue_ItemDataBound" >
                    <HeaderStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />
                    <ItemStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />
                    <FooterStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />
                    <Columns>
                   
                        <asp:BoundColumn DataField="NumberNo" HeaderText="事件单号">
                            <HeaderStyle Width="120px"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                              <asp:BoundColumn DataField="content" HeaderText="详细信息">
                            <HeaderStyle Width="330px"></HeaderStyle>
                             <ItemStyle HorizontalAlign="Left"/>
                        </asp:BoundColumn>
               
                        <asp:BoundColumn DataField="regsysdate" HeaderText="登记时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                            <HeaderStyle Width="150px"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="servicelevel" HeaderText="服务级别" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="处理时效" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceType"  HeaderText="事件类别" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                             <ItemStyle HorizontalAlign="Left"/>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="reUserName" HeaderText="当前处理人" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid></td>
		</tr>
		 <tr runat="server" id="_CpIssueTR" >
            <td align="right">
                <uc3:controlpagefoot ID="cpIssue" runat="server" />
            </td>
        </tr>
 </table>
    </form>
</body>
</html>
