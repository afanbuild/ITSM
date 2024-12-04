<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmInfSearch.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmInfSearch"
    Title="知识搜索" %>

<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc4" %>
<%@ Register Src="../Controls/CtrTextDropList.ascx" TagName="CtrTextDropList" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrKBCataDropList.ascx" TagName="ctrKBCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table width='98%' class="listContent" style="display:none;">
        <tr>
            <td class='listTitleNoAlign' colspan="2" align="center">
                <uc4:ctrtitle ID="Ctrtitle1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class='listNoAlign' align="center">
                <uc3:CtrTextDropList ID="txtPKey" runat="server" onkeydown="keyon();" Width="400px" />
                <asp:TextBox ID='txtPKey1' runat='server' Width="260px" onkeydown="keyon();" Visible="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleNoAlign' colspan="2" align="center">
                <asp:Button ID="btnSearch" runat="server" Text="搜  索" CssClass="btnClass" OnClick="btnSearch_Click"
                    UseSubmitBehavior="false" OnClientClick="CheckInput();" />
            </td>
        </tr>
    </table>
    <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click">高级搜索</asp:LinkButton>
    <table width="98%" id="table1" runat="server" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgInf_Information" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="dgInf_Information_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                    <Columns>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-Wrap="true">
                            <ItemTemplate>
                                <table cellpadding='1' cellspacing='2' width='100%' border='0'>
                                    <tr>
                                        <td align="left">
                                            <a target="_blank" href="frmKBShow.aspx?KBID=<%#DataBinder.Eval(Container.DataItem, "ID") %>">
                                                <%#DataBinder.Eval(Container.DataItem, "Title") %></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <br />
                                            <asp:Literal ID="Literal2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Content") %>'></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            关键字：<asp:Literal ID="Literal3" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Pkey") %>'></asp:Literal>
                                            知识类别:
                                            <asp:Literal ID="Literal4" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TypeName") %>'></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPage1" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
    <table width="98%" id="table2" runat="server" class="listContent">
        <tr>
            <td class="listTitleRight" style="width:12%">
                相关搜索
            </td>
            <td class="list">
                <asp:DataList ID="DataList1" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"
                    ShowFooter="False" ShowHeader="False" Width="60%">
                    <ItemTemplate>
                        <a href="#" onclick="ShowMore('<%# DataBinder.Eval(Container.DataItem, "Keyword")%>')">
                            <%# DataBinder.Eval(Container.DataItem, "Keyword")%></a>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>

    <script language="javascript">
//        document.all.<%=txtPKey.ClientID%>.focus();
//        function keyon()
//        {
//            if (event.keyCode==13) {document.all.<%=btnSearch.ClientID%>.click();} 
//        }
//        document.onkeydown=keyon;
//                
//        function CheckInput()          //
//	    {
//	        if(document.all.<%=txtPKey.ClientID%>.value.trim()== "")
//	        {
//	            alert("请输入搜索条件!");
//	            document.all.<%=txtPKey.ClientID%>.focus();
//	            event.returnValue = false;
//	        }
//		    else   //
//		    {
//		        event.returnValue = true;
//		    }
//	    }
//	    String.prototype.trim = function()  //去空格

//			{
//				// 用正则表达式将前后空格
//				// 用空字符串替代。
//				return this.replace(/(^\s*)|(\s*$)/g, "");
//			}
			
            
            function ShowMore(keyword)
            {
                var url = "frmInfSearch.aspx?KeyWord=" + escape(keyword);
                window.open(url,"_self","" );
            }

    </script>

</asp:Content>
