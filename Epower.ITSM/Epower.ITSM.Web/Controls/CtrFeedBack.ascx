<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrFeedBack.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.CtrFeedBack" %>
<%@ Register Src="CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<table cellspacing="1" cellpadding="1" width="100%"  border='0'>
    <tr>
        <td width="100%">
            <asp:Literal ID="ltlHFList" runat="server"></asp:Literal>
        </td>
    </tr>
</table>
<br />
<table id="tbEdit" runat="server" cellspacing="1" cellpadding="1" width="100%" class="listContent">
    <tr>
        <td style="width: 12%" align="right" class="listTitle">
            满意程度
        </td>
        <td class="list" colspan="3" align="left">
            <asp:RadioButtonList ID="rblFeedBack" runat="server" Width="280px" RepeatDirection="Horizontal">
                <asp:ListItem Value="1" Selected="True">满意</asp:ListItem>
                <asp:ListItem Value="2">基本满意</asp:ListItem>
                <asp:ListItem Value="3">不满意</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="listTitle" style="width: 12%" align="right">
            回访人
        </td>
        <td class="list" align="left">
            <asp:TextBox ID="txtFeedPerson" runat="server"></asp:TextBox>
        </td>
        <td class="listTitle" style="width: 12%" align="right">
            被回访人
        </td>
        <td class="list" align="left">
            <asp:TextBox ID="txtCustName" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="listTitle" style="width: 12%" align="right">
            回访类别
        </td>
        <td class="list" align="left">
            <asp:RadioButtonList ID="rboFeedType" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="1">电话</asp:ListItem>
                <asp:ListItem Value="2">上门</asp:ListItem>
                <asp:ListItem Value="4">邮件</asp:ListItem>
                <asp:ListItem Value="3">其它</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td style="width: 12%" align="right" class="listTitle">
            回访时间
        </td>
        <td class="list" align="left">
            <uc1:ctrdateandtime ID="CtrDTFBTime" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 12%" align="right" class="listTitle">
            备&nbsp;&nbsp;&nbsp; 注
        </td>
        <td class="list" colspan="3" align="left">
             <asp:TextBox ID="txtFeedBack" runat="server" Height="50px" Width="98%" MaxLength="1500"
                TextMode="MultiLine"></asp:TextBox>
            <asp:Button ID="cmdFeedBack" runat="server" Text="确定" OnClick="cmdFeedBack_Click"
                CssClass="btnClass" Visible="false"></asp:Button>
        </td>
    </tr>
</table>
