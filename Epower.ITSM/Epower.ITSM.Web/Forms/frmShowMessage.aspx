<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmShowMessage.aspx.cs" Inherits="Epower.ITSM.Web.frmPaneList" Title="����Ϣ" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
    function delete_confirmMessage()
	{
		event.returnValue =confirm("ȷ��Ҫɾ������Ϣ��?");
	}
	function Replay(obj)
	{	
	    var userid = document.getElementById(obj.id.replace("lnkReplay","hidUserID")).value;   //�ظ���USERID
		var retValue = window.showModalDialog("../Forms/FrmSMSSend.aspx?userid=" + userid ,window,"dialogWidth=400px; dialogHeight=310px;status=no; help=no;scroll=auto;resizable=no");
		if(retValue != null)
		{
			event.returnValue = true;
		}
		else
		{
		    event.returnValue = false;
		}
	}
	function showWin()
	{	
		var retValue = window.showModalDialog("../Forms/FrmSMSSend.aspx",window,"dialogWidth=400px; dialogHeight=310px;status=no; help=no;scroll=auto;resizable=no");
		if(retValue != null)
		{
			if(retValue.length>1)
			{
			}
		}
	}
</script>  
<table width='99%' class="listContent" align="center">
    <tr >
        <td width='100%' align='center' valign='middle' class="listTitle"><STRONG> ����δ������Ϣ </STRONG><asp:Label ID="lblcount" runat="server" Text=""></asp:Label><STRONG>��</STRONG>
            &nbsp;&nbsp;<a onclick='showWin();return false;' href='#'><font color=blue>д��Ϣ</font></a>
        </td>
    </tr>
</table>
<table cellpadding='1' cellspacing='1' width='100%' border='0'>
    <tr>
        <td>
            <asp:DataList ID="DataList1" runat="server" DataKeyField="smsid" OnItemCommand="DataList1_ItemCommand" ShowFooter="false" Width="100%">
                <AlternatingItemStyle BackColor="Azure" />
                <ItemStyle BackColor="White" CssClass="tablebody" />
                <HeaderStyle CssClass="listTitle" />
                <ItemTemplate>
                    <table width="100%" class="listContent">
                        <tr>
                            <td class="listTitle" width="100%">
                                <asp:Label ID="lbluserTitel" runat="server" Text='�����ˣ�'></asp:Label>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SenderName")%>'></asp:Label>&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkReplay" runat="server" ForeColor="blue" Text="�ظ�" OnClientClick="Replay(this);"></asp:LinkButton>&nbsp;&nbsp
                                <asp:LinkButton ID="lnkRead" runat="server" CommandName="read" ForeColor="blue" Text="�ر�" ></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="delete" ForeColor="blue" OnClientClick="delete_confirmMessage();" Text="ɾ��" ></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="listTitle" width="100%">
                                <asp:Label ID="Label5" runat="server" Text='����ʱ�䣺'></asp:Label>
                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("sendtime") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="list" colspan="5" style="word-break: break-all" width="100%">
                                <asp:Label ID="Label3" runat="server" Text='���ݣ�'></asp:Label>
                                <asp:Label ID="lblContent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"content")%>'></asp:Label>
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("smsid") %>' Visible="false"></asp:Label>
                                <input type="hidden" id="hidUserID" runat="server" value='<%# Eval("SenderID") %>' />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList></td>
    </tr>
</table>
</asp:Content>
