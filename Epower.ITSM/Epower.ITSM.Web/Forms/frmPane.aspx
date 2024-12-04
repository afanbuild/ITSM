<%@ Register TagPrefix="uc1" TagName="CtrNewsInfolist" Src="../Controls/CtrNewsInfolist.ascx" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmPane.aspx.cs" Inherits="Epower.ITSM.Web.frmPane" Title="ÎÞ±êÌâÒ³" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CtrNewsInfolist id="CtrNewsInfolist1" runat="server"></uc1:CtrNewsInfolist>
    <br />
	<TABLE id="Table1" width="98%" class="listContent">
		<TR>
			<TD class="listTitle">
				<asp:label id="lblOrgName" runat="server" Font-Bold="True"></asp:label>
		    </TD>
		</TR>
	</TABLE>
	<uc1:CtrNewsInfolist id="CtrNewsInfolist2" runat="server"></uc1:CtrNewsInfolist>
</asp:Content>
