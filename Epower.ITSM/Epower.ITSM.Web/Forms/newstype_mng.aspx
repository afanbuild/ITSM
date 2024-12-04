<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.NewsType_mng" CodeBehind="NewsType_mng.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script language="javascript" type="text/javascript">
            function delete_confirm(obj)     //ɾ��ǰִ�нű�
	        {
	            var TypeID = document.getElementById(obj.id.replace("Button5","hidDelete")).value;
                var pars = "act=OANewTypeMsg&TypeID="+TypeID;
                 $.ajax({
                            type: "post",
                            data:pars,
                            async:false,
                            url: "../Forms/Handler.ashx",
                            success: function(data, textStatus){
                                //alert(data);
                                var json = eval("(" + data + ")");
                                var result = json.record;
                                if(result==false)
                                {
                                    event.returnValue =confirm("ȷ��Ҫɾ����?");
                                }
                                else
                                {
                                    event.returnValue =confirm("������»��й�����Ϣ,ȷ��Ҫɾ����?");
                                }
		                    }
                         });
	            
		       //event.returnValue =confirm("ȷ��Ҫɾ����?");
	        }
    </script>

    <table id="Table1" align="center" style="width: 98%;" cellpadding="0" class="listContent">
        <tr>
            <td valign="top" align="center" colspan="5" class="listContent">
                <asp:DataGrid ID="DgNewsType" runat="server" Width="100%" GridLines="Horizontal"
                    AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="DgNewsType_ItemCommand" OnItemCreated="DgNews_ItemCreated">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="TypeId"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="IsInner" HeaderText="�ڲ�"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="IsOuter" HeaderText="�ⲿ"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TypeName" HeaderText="��Ϣ���">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Description" HeaderText="˵��">
                            <HeaderStyle Width="260px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="�༭">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" Text="�༭" SkinID="btnClass1" CommandName="Edit" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="ɾ��">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="Button5" runat="server" Text="ɾ��" SkinID="btnClass1" CommandName="Delete" OnClientClick="delete_confirm(this);" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "TypeId")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle" width="100px">
                �������
            </td>
            <td class="list">
                <asp:TextBox ID="TxtTypeName" runat="server" Width="158px" MaxLength="200"></asp:TextBox>
            </td>
            <td align="right" height="22" class="listTitle" width="100px">
                ˵��
            </td>
            <td class="list">
                <asp:TextBox ID="txtDesc" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
            </td>
            <td nowrap align="left" class="listTitle">
                <asp:Button ID="BtAddType" runat="server" Text="������" OnClick="Add_NewsType" CssClass="btnClass">
                </asp:Button>&nbsp;
                <asp:Button ID="btnSave" runat="server" Text="����" Enabled="False" OnClick="btnSave_Click"
                    CssClass="btnClass"></asp:Button>
                <asp:Button ID="btnCancel" runat="server" Text="ȡ��" Enabled="False" OnClick="btnCancel_Click"
                    CssClass="btnClass"></asp:Button>
            </td>
        </tr>
    </table>
    <asp:TextBox ID="txtID" runat="server" Visible="False">0</asp:TextBox>
</asp:Content>
