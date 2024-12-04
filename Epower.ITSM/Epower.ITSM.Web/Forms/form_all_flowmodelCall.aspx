<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Epower.ITSM.Web.Forms.form_all_flowmodelCall" Title="����ģ���嵥" CodeBehind="form_all_flowmodelCall.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
    function AddNewFlow(id)  //ɾ������
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
                <font face="����">
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
                            �ͻ���Ϣ
                        </td>
                    </tr>
                </table>
                <table class="listContent" width="100%" align="center" runat="server" id="Table1">
                    <tr>
                        <td style="width: 99px" class="listTitle" nowrap="nowrap">
                            <asp:Literal ID="LitCustName" runat="server" Text="�û�����"></asp:Literal>
                        </td>
                        <td class="list" style="width:35%">
                            <asp:Label ID="labCustAddr" runat="server"></asp:Label>
                        </td>
                        <td style="width: 99px" class="listTitle">
                            <asp:Literal ID="LitCustAddress" runat="server" Text="�û���ַ"></asp:Literal>
                        </td>
                        <td class="list" style="width: *">
                            <asp:Label ID="lblAddr" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap align="left" class="listTitle">
                            <asp:Literal ID="LitContact" runat="server" Text="��ϵ��"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="labContact" runat="server"></asp:Label>
                        </td>
                        <td nowrap class="listTitle">
                            <asp:Literal ID="LitCTel" runat="server" Text="��ϵ�绰"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="labCTel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="listTitle" nowrap="nowrap">
                            <asp:Literal ID="LitCustDeptName" runat="server" Text="�û�����"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="lblCustDeptName" runat="server"></asp:Label>
                        </td>
                        <td class="listTitle" nowrap="nowrap">
                            <asp:Literal ID="litCustEmail" runat="server" Text="�����ʼ�"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="listTitle" nowrap="nowrap">
                            <asp:Literal ID="LitMastShortName" runat="server" Text="����λ"></asp:Literal>
                        </td>
                        <td class="list" style="height: 23px">
                            <asp:Label ID="lblMastCust" runat="server"></asp:Label>
                        </td>
                        <td align="left" class="listTitle" nowrap="nowrap">
                            ְλ
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
                                    <asp:BoundColumn DataField="FlowName" HeaderText="��������">
                                        <HeaderStyle Width="40%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Remark" ReadOnly="True" HeaderText="��ע">
                                        <HeaderStyle Width="40%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="����">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemTemplate>
                                            <input id="cmdStart" class="btnClass1" onclick='AddNewFlow(<%#DataBinder.Eval(Container.DataItem, "FlowModelID")%>)'
                                                type="button" value="����" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="�鿴����ͼ">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemTemplate>
                                            <font face="����">
                                                <input id="cmdViewChart" class="btnClass1" onclick='window.open("flow_View_ChartModel.aspx?flowmodelid=<%#DataBinder.Eval(Container.DataItem, "FlowModelID")%>","" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));'
                                                    type="button" value="�鿴����ͼ"></font>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="FlowModelID" HeaderText="��������" Visible="false">
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitle" align="right" style="width: 100%">
                            <pagerstyle nextpagetext="��һҳ" prevpagetext="��һҳ" horizontalalign="Left" forecolor="#003399"
                                backcolor="#99CCCC" mode="NumericPages"></pagerstyle>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
