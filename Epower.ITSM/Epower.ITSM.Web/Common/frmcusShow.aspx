<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmcusShow.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmcusShow"
    Title="�ͻ�����" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/CustSchemeCtr.ascx" TagName="CustSchemeCtr" TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              var className;
              var objectFullName;
              var tableCtrl;
              objectFullName = <%=tr1.ClientID%>.id;
              className = objectFullName.substring(0,objectFullName.indexOf("tr1")-1);
              tableCtrl = document.all.item(className.substr(0,className.length)+"_"+TableID);
              if(imgCtrl.src.indexOf("icon_expandall") != -1)
              {
                tableCtrl.style.display ="";
                imgCtrl.src = ImgMinusScr ;
              }
              else
              {
                tableCtrl.style.display ="none";
                imgCtrl.src = ImgPlusScr ;		 
              }
        }
    </script>

    <table style="width: 98%" cellpadding="2" cellspacing="0" class="listContent" align="center">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="Literal1" runat="server" Text="��Ӧ�û�"></asp:Literal>
            </td>
            <td class="list" style="width:35%">
                <asp:Label ID="lblRefUser" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitMastShortName" runat="server" Text="����λ"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblMastCust" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="LitCustDeptName" runat="server" Text="����"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustDeptName" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustName" runat="server" Text="�ͻ�����"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblShortName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight"">
                <asp:Literal ID="LitCustomCode" runat="server" Text="�ͻ�����"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustomCode" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitFullName" runat="server" Text="Ӣ������"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblfullname" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustEmail" runat="server" Text="�����ʼ�"></asp:Literal>
            </td>
            <td class="list">
                <asp:HyperLink ID="lblEmail" runat="server">[lblEmail]</asp:HyperLink>
            </td>        
            <td class="listTitleRight">
                <asp:Literal ID="LitCustomerType" runat="server" Text="�ͻ�����"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustomerTypeName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitContact" runat="server" Text="��ϵ��"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblLinkMan1" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight ">
                <asp:Literal ID="LitJob" runat="server" Text="ְλ"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblJob" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCTel" runat="server" Text="��ϵ�绰"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblTel1" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustAddress" runat="server" Text="��ϵ��ַ"></asp:Literal>
            </td>
            <td class="list" align="left">
                <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitRights" runat="server" Text="Ȩ��"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblRights" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitkhRemark" runat="server" Text="��ע"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4" class="list">
                <uc3:CustSchemeCtr ID="CustSchemeCtr1" runat="server" ReadOnly="true" />
            </td>
        </tr>
    </table>
    <br />
    <table style="width: 98%" class="listContent" id="TableImg" runat="server">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNoAlign">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" />��ʷ�����¼
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table1" style="width: 98%" cellpadding="0" cellspacing="0" border="0" runat="server">
        <tr>
            <td>
                <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" CellSpacing="2" CellPadding="1"
                    BorderWidth="0px" BorderColor="White" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="gridUndoMsg_ItemCreated"
                    OnItemDataBound="gridUndoMsg_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="subject" HeaderText="����">
                            <HeaderStyle Width="40%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="RegUserName" HeaderText="������">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustTime" HeaderText="����ʱ��" DataFormatString="{0:g}">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="custName" HeaderText="�ͻ�">
                            <HeaderStyle Width="15%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="contact" HeaderText="��ϵ��">
                            <HeaderStyle Width="12%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="dealstatus" HeaderText="����״̬">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="����">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='����' runat="server" causesvalidation="false" class="btnClass1">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="dealstatus"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="endtime"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPageIssues" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
</asp:Content>
