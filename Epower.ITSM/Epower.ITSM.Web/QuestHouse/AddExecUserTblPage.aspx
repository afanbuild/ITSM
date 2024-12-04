<%@ Page Title="行外人员" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="AddExecUserTblPage.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.AddExecUserTblPage" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language=javascript>
    function AddComp() {
        var opObj = window.parent.document.all.hidOpObjId;
        var chkSelObject=window.parent.document.all.chkSelObject;
        if ((opObj.value == ""||opObj.value == "0")&&chkSelObject.value =="true") {
            alert("请选择操作对象");
            window.location.reload();
        } else {
            var HouseID = document.getElementById("<%=HouseID.ClientID%>").value;
            //======zxl==
            var url="../QuestHouse/EiitcomeINJFpeople.aspx?HouseID=" + HouseID + "&OpObjId=" + opObj.value + "&chk=" + chkSelObject.value + "&Random=" + GetRandom()+"&TypeFrm=AddExecUserTblPage&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
            window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=597,height=597,left=150,top=50");
        }
    }
</script>
  <input type="hidden" runat="server" id="HouseID" />  
 <table class="listContent" width="100%" align="center">
    <tr>
        <td class="list" style="width: *" colspan="4">  
              <asp:datagrid id="comperGrid" runat="server" Width="100%" 
		        AutoGenerateColumns="False"  CssClass="Gridtable" >
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>
				     <asp:TemplateColumn HeaderText="序号"  >
                            <ItemTemplate>
                                <%# Container.ItemIndex+1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"  Width="30px"/>
                            <HeaderStyle HorizontalAlign="Center"  Width="30px"/>
                        </asp:TemplateColumn>
					<asp:BoundColumn Visible="true" DataField="peopleName" HeaderText="姓名"></asp:BoundColumn>
                    <asp:BoundColumn DataField="CardNO" HeaderText="证件号码"></asp:BoundColumn>
					<asp:BoundColumn DataField="PeoplePhone" HeaderText="电话"></asp:BoundColumn>
					<asp:BoundColumn DataField="computeName" HeaderText="公司名称"></asp:BoundColumn>
                    <asp:BoundColumn DataField="COMEID" Visible=false></asp:BoundColumn>
					<asp:TemplateColumn Visible="true">
					    <HeaderTemplate>
					         <asp:Button ID="ADDComp" runat="server" CommandName="Delete" Text="编辑" OnClientClick="AddComp()" />
					    </HeaderTemplate>
						
						<HeaderStyle Width="4%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
					</asp:TemplateColumn>
				</Columns>
				</asp:datagrid>
				
<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
				<asp:Button ID="hiddButton" runat="server" Text="hiddButton" 
                  onclick="hiddButton_Click"  style="display:none;" />
        </td>
	</tr>
</table> 
</asp:Content>
