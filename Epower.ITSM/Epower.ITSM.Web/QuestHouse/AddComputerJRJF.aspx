<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="AddComputerJRJF.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.AddComputerJRJF" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        function AddExecUserTbl() {
        var opObj = window.parent.document.all.hidOpObjId;
        var chkSelObject=window.parent.document.all.chkSelObject;
        if ((opObj.value == ""||opObj.value == "0")&&chkSelObject.value =="true") {
            alert("请选择操作对象");
            window.location.reload();
        } else {
            var HouseID = document.getElementById("<%=HouseID.ClientID%>").value;
            var url="../QuestHouse/AddExecUserTbl.aspx?HouseID=" + HouseID + "&OpObjId=" + opObj.value + "&chk=" + chkSelObject.value + "&Random=" + GetRandom()+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
            window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=600,height=600,left=150,top=50");
        }
    }
    </script>

    <input type="hidden" runat="server" id="HouseID" />
    <table class="listContent" width="100%" align="center">
        <tr>
            <td class="list" style="width: *" colspan="4">
                <asp:DataGrid ID="ExecUserGrid" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemDataBound="ExecUserGrid_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.ItemIndex+1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="true" DataField="IP" HeaderText="IP">
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="" HeaderText="用户名"></asp:BoundColumn>
                        <asp:TemplateColumn Visible="true">
                            <HeaderTemplate>
                                <asp:Button ID="BtnAdd" runat="server" Text="编辑" OnClientClick="AddExecUserTbl();" />
                            </HeaderTemplate>
                            <HeaderStyle Width="4%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:Button runat="server" ID="BtnHidden" style="display:none;"  Text="BtnHidden" 
                    onclick="BtnHidden_Click" />
                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
            </td>
        </tr>
    </table>
</asp:Content>
