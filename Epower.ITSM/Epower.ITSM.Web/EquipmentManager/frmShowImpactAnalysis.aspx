<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmShowImpactAnalysis.aspx.cs"
    Inherits="Epower.ITSM.Web.EquipmentManager.frmShowImpactAnalysis" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>资产关联信息</title>
    <!--#include file="~/Js/tableSort.js" -->
    <script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
</head>
<body>

    <script type="text/javascript" language="javascript">
        function openwin(EquID)
        {
            window.open('../EquipmentManager/frmEqu_DeskEdit.aspx?newWin=true&FlowID=0&id='+EquID,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
            event.returnValue = false;
        }
    </script>
    <form id="form1" runat="server">
    
    <table id="Table4" style="width: 100%;" runat="server" class="listContent" cellpadding="0">
        <tr>
            <td align="right" class="list">
                <input id="cmdOpenRelChart" onclick="OpenEquRelChart();" class="btnClass" type="button"
                    value="资产影响图" />
            </td>
        </tr>
         <tr>
            <td class="list">
                <div id="ImplyDiv" runat="server"></div>
            </td>
        </tr>
        <tr style="display:none">
            <td align="center" class="listContent" colspan="6">
                <asp:DataGrid ID="dgPro_ProblemAnalyse" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCreated="dgPro_ProblemAnalyse_ItemCreated"
                    OnItemDataBound="dgPro_ProblemAnalyse_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <FooterStyle CssClass="listTitle" />
                    <Columns>
                        <asp:BoundColumn DataField='Equ_ID' HeaderText='EquID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RelID' HeaderText='RelID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='Name' Visible="false"></asp:BoundColumn>
                        <asp:HyperLinkColumn DataTextField="Name" HeaderText="关联资产" Target="_blank" DataNavigateUrlField="RelID"
                            DataNavigateUrlFormatString="frmEqu_DeskEdit.aspx?FlowID=0&IsRel=0&ID={0}"></asp:HyperLinkColumn>
                        <asp:BoundColumn DataField='RelDescription' HeaderText='关系描述'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Code' HeaderText='资产编号'></asp:BoundColumn>
                        <asp:BoundColumn DataField='CostomName' HeaderText='所属客户'></asp:BoundColumn>
                        <asp:BoundColumn DataField='partBankName' HeaderText='维护单位'></asp:BoundColumn>
                        <asp:BoundColumn DataField='partBranchName' HeaderText='维护部门'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table id="Table5" style="width: 100%;" visible="false" runat="server" class="listContent"
        cellpadding="0">
        <tr>
            <td align="right" class="list">
                <input id="Button1" onclick="OpenEquRelToChart();" class="btnClass" type="button" value="资产影响图" />
            </td>
        </tr>
        <tr>
            <td align="center" class="listContent" colspan="6">
                <asp:DataGrid ID="dg_EquRelIn" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCreated="dg_EquRelIn_ItemCreated"
                    OnItemDataBound="dg_EquRelIn_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <FooterStyle CssClass="listTitle" />
                    <Columns>
                        <asp:BoundColumn DataField='Equ_ID' HeaderText='EquID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RelID' HeaderText='RelID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='Name' Visible="false"></asp:BoundColumn>
                        <asp:HyperLinkColumn DataTextField="Name" HeaderText="关联资产" Target="_blank" DataNavigateUrlField="Equ_ID"
                            DataNavigateUrlFormatString="frmEqu_DeskEdit.aspx?FlowID=0&IsRel=0&ID={0}"></asp:HyperLinkColumn>
                        <asp:BoundColumn DataField='RelDescription' HeaderText='关系描述'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Code' HeaderText='资产编号'></asp:BoundColumn>
                        <asp:BoundColumn DataField='CostomName' HeaderText='所属客户'></asp:BoundColumn>
                        <asp:BoundColumn DataField='partBankName' HeaderText='维护单位'></asp:BoundColumn>
                        <asp:BoundColumn DataField='partBranchName' HeaderText='维护部门'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript" language="javascript">
    function OpenEquRelChart()
       {
     
            if ($.browser.msie)  {           
                    window.open("frm_Equ_RelChartView.aspx?&ReadOnly=true&newWin=true&id=" + <%=EquID%>,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");      
              }
              else{              
                     window.open("frm_Equ_RelChartView_SVG.aspx?ReadOnly=true&newWin=true&id=" + <%=EquID%>,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
              }
      } 
    
    function OpenEquRelToChart()
        {
            window.open("frm_Equ_RelChartView.aspx?ReadOnly=true&newWin=true&id=" + <%=EquID%>,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
        }
    </script>

</body>
</html>
