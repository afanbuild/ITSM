<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_KBBaseQuery.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frm_KBBaseQuery"
    Title="֪ʶ������ѯ" %>

<%@ Register Src="../Controls/controlpage.ascx" TagName="controlpage" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc2" %>
<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register src="../Controls/ctrDateSelectTimeV2.ascx" tagname="ctrDateSelectTime" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Begin: ��������ű���-->
    <script type="text/javascript" language="javascript" src="../js/epower.base.js"></script>
    <!--End: ��������ű���-->
    
    <script language="javascript" type="text/javascript">    
    function OpenDeleteFlow(obj)  //ɾ������
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete","hidDelete")).value;
        //zxl -2012.8.6.
        var str="../Common/frmFlowDelete.aspx?FlowID=" + FlowID+"&Opener_hiddenDelete=<%=hidd_btnDelete.ClientID %>";
        var url=str+"&TypeFrm=frm_KBBaseQuery";
        
        var _xy = epower.tools.computeXY('c', window, 320, 230);                        
        
        window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=320,height=230,left=" + _xy.x + ",top=" + _xy.y);
        
    }
    </script>

    <table width="98%" align="center" style="display: none">
        <tr>
            <td class="list" align="center" colspan="2">
                <uc1:CtrTitle ID="CtrTitle1" runat="server" Title="֪ʶ��ѯ"></uc1:CtrTitle>
            </td>
        </tr>
    </table>
    <table width='98%' cellpadding="2" cellspacing="0" border='0' class="listContent GridTable">
        <tr>
            <td nowrap align="right" class="listTitleRight" width="12%">
                ����״̬
            </td>
            <td align="left" class="list" width="35%">
                <asp:DropDownList ID="cboStatus" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight"  style='width: 12%;'>
                <asp:Literal ID="info_PKey" runat="server" Text="�ؼ���"></asp:Literal>  
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPKey' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                �Ǽ�����
            </td>
            <td align="left" class="list" nowrap>
                <uc5:ctrDateSelectTime ID="ctrDateSelectTime1" runat="server" />
            </td>
            <td class="listTitleRight" align='right'>
               <asp:Literal ID="info_Title" runat="server" Text="����"></asp:Literal>    
            </td>
            <td class='list'>
                <asp:TextBox ID='txtTitle' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" align='right'>
               <asp:Literal ID="info_Tags" runat="server" Text="ժҪ"></asp:Literal>     
            </td>
            <td class='list'>
                <asp:TextBox ID='txtTags' runat='server'></asp:TextBox>
            </td>
            <td class="listTitleRight" align='right'>
                <asp:Literal ID="info_Content" runat="server" Text="����"></asp:Literal>      
            </td>
            <td class='list' style="word-break: break-all">
                <asp:TextBox ID='txtContent' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>

    <br />
    <table cellpadding="0" width="98%" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="grd" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="grd_ItemCreated"
                    OnDeleteCommand="gridUndoMsg_DeleteCommand" OnItemDataBound="grd_ItemDataBound">
                    <Columns>
                        <asp:BoundColumn DataField='Title' HeaderText='����' ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='PKey' ItemStyle-Width="20%" HeaderText='�ؼ���' ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='TypeName' HeaderText='����' ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="�Ǽ�����" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "RegTime", "{0:d}")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="����״̬">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                   <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>���ڴ���</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>������ͣ</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>������ֹ</font>" : "<font color='green'>��������</font>"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="����" ItemStyle-HorizontalAlign="center">
                            <HeaderStyle Width="5%" HorizontalAlign="center"></HeaderStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='����' runat="server">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="endtime"></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="ɾ��" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="ɾ��" CommandName="Delete" SkinID="btnClass1"
                                    OnClientClick="OpenDeleteFlow(this);" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:Button ID="hidd_btnDelete" runat="server" style="display:none;"  Text="ɾ��ʱ���µ�������" 
                    onclick="hidd_btnDelete_Click"  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc4:ControlPageFoot ID="cpKBBase" runat="server" />
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />

</asp:Content>
