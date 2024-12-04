<%@ Page Title="公告信息维护" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="frmnews_list.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmnews_list" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript" src="../js/epower.base.js"></script>
<script language="javascript" type="text/javascript">
    function OpenDeleteFlow(obj)  //删除流程
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete", "hidDelete")).value;
         var url = "../Common/frmFlowDelete.aspx?FlowID=" + FlowID +"&Opener_hiddenDelete=<%=hidd_btnDelete.ClientID %>&TypeFrm=cst_Issue_list";
        
        var height=($(document).height() - 230)/2 + +$(window).scrollTop();
        var width=($(document).width() - 320)/2 + +$(window).scrollLeft();              
        
        var _xy = epower.tools.computeXY('c', window, 320, 230);                
        width = _xy.x;
        height = _xy.y;
        
		open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=320px,height=230px,left=' + width +',top=' + height );    
    }
</script>

    <table cellspacing="0" cellpadding="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgNews" runat="server" CssClass="Gridtable"
                    AutoGenerateColumns="False" Width="100%" 
                    OnItemCreated="dgNews_ItemCreated" OnItemDataBound="dgNews_ItemDataBound" 
                    ondeletecommand="dgNews_DeleteCommand" >
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="NewsId"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TypeName" HeaderText="信息类别">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="NewsId" DataNavigateUrlFormatString="ShowNews.aspx?NewsID={0}"
                            DataTextField="Title" HeaderText="信息题目">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField="Writer" HeaderText="单位/人员">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DispFlag" HeaderText="是否显示">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IsAlert" HeaderText="是否弹出">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PubDate" HeaderText="发布时间" DataFormatString="{0:yyyy-MM-dd}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="OutDate" HeaderText="截止时间" DataFormatString="{0:yyyy-MM-dd}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="name" HeaderText="创建人">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="处理">
                            <HeaderStyle Width="5%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='详情' runat="server">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="删除">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemTemplate>
                                 <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" SkinID="btnClass1"
                                    OnClientClick="OpenDeleteFlow(this);" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="TypeID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="OutDate" Visible="false" DataFormatString="{0:yyyy-MM-dd}">
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="InputUser"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#000066" BackColor="#E7E7FF"></PagerStyle>
                </asp:DataGrid>
                <asp:Button ID="hidd_btnDelete" runat="server" style="display:none;"  Text="删除时重新调用数据" 
                    onclick="hidd_btnDelete_Click" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc3:ControlPageFoot ID="cpNews" runat="server"></uc3:ControlPageFoot>
            </td>
        </tr>
    </table>
</asp:Content>

