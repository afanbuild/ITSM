<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmShowMessageList.aspx.cs" Inherits="Epower.ITSM.Forms.Web.frmShowMessageList" Title="短信息" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="~/Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc1"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
    function delete_confirm()
	{
		event.returnValue =confirm("确认要删除此消息吗?");
	}
	function Replay(obj)
	{	
	    var userid = document.getElementById(obj.id.replace("lnkReplay","hidUserID")).value;   //回复人USERID
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
<table  width='100%' border='0' class="listContent">
    <tr>
         <TD noWrap align="right" class="listTitle" width="10%" >
            <asp:Label id="Label1" runat="server">消息状态</asp:Label></TD>
        <TD align="left" class="list"  width="30%">
                <asp:DropDownList id="ddltReadState" runat="server" Width="120px">
                    <asp:ListItem Selected="True" Value="-1">--全部--</asp:ListItem>
                    <asp:ListItem Value="0">未关闭</asp:ListItem>
                    <asp:ListItem Value="1">已关闭</asp:ListItem>
                </asp:DropDownList></TD>
        <TD noWrap  align="right" class="listTitle" width="10%" >
            <asp:Label id="Label2" runat="server">发送时间</asp:Label></TD>
        <TD align="left" class="list" width="50%">

                        <uc1:ctrDateSelectTime id="ctrDateSelectTime1" runat="server" />
                        
        </TD>
    </tr>
</table>
<font color="red">最大显示100条记录！</font>
<br />
<table cellpadding='1' cellspacing='1' width='100%' border='0' class="listContent">
    <tr >
        <td width='100%' align='center' valign='middle' class="listTitle"><STRONG> 您有短消息 </STRONG><asp:Label ID="lblcount" runat="server" Text=""></asp:Label><STRONG>条</STRONG>
            &nbsp;&nbsp;<a onclick='showWin();return false;' href='#'><font color=blue>写消息</font></a>
        </td>
    </tr>
</table>
<table  borderColor="#000000" cellSpacing="1" borderColorDark="#ffffff" cellPadding="1" width="100%" align="center" borderColorLight="#000000"  runat="server">
    <tr id="tr1" runat="server">
        <td vAlign="top" align="left" class="listTitle" >
              <img class="icon" id="Img1" onclick="ShowTable(this);" style="cursor:hand" height="16" src="../Images/icon_collapseall.gif" width="16"/>未关闭消息
        </td>
    </tr>
</table>
<table cellpadding='1' cellspacing='1' width='100%' border='0' id="Table1" runat="server">
    <tr>
        <td class="listContent">
            <asp:DataList ID="DataList1" runat="server" OnItemCommand="DataList1_ItemCommand" ShowFooter="false" Width="100%">
                <AlternatingItemStyle BackColor="Azure" />
                <ItemStyle BackColor="White" CssClass="tablebody" />
                <HeaderStyle CssClass="listTitle" />
                <ItemTemplate>
                    <table width="100%">
                        <tr>
                            <td class="listTitle" width="100%">
                                <asp:Label ID="lbluserTitel" runat="server" Text='发送人：'></asp:Label>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SenderName")%>'></asp:Label>&nbsp;&nbsp;  
                                <asp:LinkButton ID="lnkReplay" runat="server" ForeColor="blue" Text="回复" OnClientClick="Replay(this);"></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkRead" runat="server" CommandName="read" ForeColor="blue" Text="关闭" ></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="delete" ForeColor="blue" OnClientClick="delete_confirm();" Text="删除" ></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="listTitle" width="100%">
                                <asp:Label ID="Label5" runat="server" Text='发送时间：'></asp:Label>
                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("sendtime") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="list" colspan="5" style="word-break: break-all" width="100%">
                                <asp:Label ID="Label3" runat="server" Text='内容：'></asp:Label>
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
<br />
<table  borderColor="#000000" cellSpacing="1" borderColorDark="#ffffff" cellPadding="1" width="100%" align="center" borderColorLight="#000000"  runat="server">
    <tr>
        <td vAlign="top" align="left" class="listTitle" >
              <img class="icon" id="Img2" onclick="ShowTable(this);" style="cursor:hand" height="16" src="../Images/icon_collapseall.gif" width="16"/>已关闭消息
        </td>
    </tr>
</table>
<table cellpadding='1' cellspacing='1' width='100%' border='0' id="Table2" runat="server">
    <tr>
        <td class="listContent">
            <asp:DataList ID="DataList2" runat="server" OnItemCommand="DataList1_ItemCommand" ShowFooter="false" Width="100%">
                <AlternatingItemStyle BackColor="Azure" />
                <ItemStyle BackColor="White" CssClass="tablebody" />
                <HeaderStyle CssClass="listTitle" />
                <ItemTemplate>
                    <table width="100%">
                        <tr>
                            <td class="listTitle" width="100%">
                                <asp:Label ID="lbluserTitel" runat="server" Text='发送人：'></asp:Label>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SenderName")%>'></asp:Label>&nbsp;&nbsp;  
                                <asp:LinkButton ID="lnkReplay" runat="server" ForeColor="blue" Text="回复" OnClientClick="Replay(this);"></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkRead" runat="server" CommandName="read" ForeColor="blue" Text="关闭" Visible="false"></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="delete" ForeColor="blue" OnClientClick="delete_confirm();" Text="删除" ></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="listTitle" width="100%">
                                <asp:Label ID="Label5" runat="server" Text='发送时间：'></asp:Label>
                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("sendtime") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="list" colspan="5" style="word-break: break-all" width="100%">
                                <asp:Label ID="Label3" runat="server" Text='内容：'></asp:Label>
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

<script type="text/javascript" language="javascript">
    $(document).ready(function(){
        state_change($('#<%=ddltReadState.ClientID %>'));
        $('#<%=ddltReadState.ClientID %>').change(state_change);
    });
    
    function state_change(obj) {        
        var val;
        var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
        var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
        
        if (obj && obj.currentTarget == undefined) {            
            val = obj.val().trim();                    
        } else {
            
            val = $(this).val().trim();                
        }
        
        if (val == 0) {
            $('#<%=Table2.ClientID %>').hide();            
            $('#<%=Table1.ClientID %>').show();            
            $('#Img2').attr('src', ImgPlusScr);
            $('#Img1').attr('src', ImgMinusScr);
        } else if (val == 1) {
            $('#<%=Table1.ClientID %>').hide();            
            $('#<%=Table2.ClientID %>').show();            
            $('#Img2').attr('src', ImgMinusScr);
            $('#Img1').attr('src', ImgPlusScr);
        } else {
            $('#<%=Table2.ClientID %>').show();            
            $('#<%=Table1.ClientID %>').show();            
            
            $('#Img2').attr('src', ImgMinusScr);
            $('#Img1').attr('src', ImgMinusScr);
        }
    }
</script>
</asp:Content>

