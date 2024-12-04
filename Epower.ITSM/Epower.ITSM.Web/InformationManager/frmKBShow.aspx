<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmKBShow.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmKBShow"
    Title="知识浏览" %>

<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
    function Infdelete_confirm()
	{
		event.returnValue =confirm("确认要删除此评论吗?");
	}
	function CheckValid(obj)
	{
	    var obj = document.getElementById(obj.id.replace("btnAdd","txtContent"));
	    if(obj.value =="")
	    {
	        alert("评论内容不能为空！");
		    event.returnValue = false;
		}
		else
		{
		    event.returnValue = true;
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
    function SelectScore() 
	{
	    var newDateObj = new Date()
	    var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
		var	value=window.showModalDialog("frmInf_Score.aspx?pDate="+sparamvalue + "&KBID=" + '<%= sKBID%>',"","dialogWidth=300px; dialogHeight=200px;status=no; help=no;scroll=auto;resizable=no") ;
		//event.returnValue = false;
	}
    </script>

    <table width="90%" align="center" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table width="100%" align="center" class="listContent_1">
                    <tr>
                        <td align="center" class="listNoAlign" colspan="1" rowspan="1" height="50">
                            <asp:Label ID="LblTitle" runat="server" Font-Size="12" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="listTitleNoAlign" colspan="1" rowspan="1">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left">
                                        关键字：<asp:Label ID="labKey" runat="server" Font-Size="9"></asp:Label>
                                        &nbsp; &nbsp;提供者：
                                        <asp:Label ID="LblWriter" runat="server" Font-Size="9"></asp:Label>
                                        （发布时间：
                                        <asp:Label ID="LblPubDate" runat="server" Font-Size="9"></asp:Label>）
                                    </td>
                                    <td align="right">
                                        平均分：
                                        <asp:Label ID="lblScore" runat="server" Font-Size="9pt"></asp:Label>
                                        <asp:LinkButton ID="lnkScore" runat="server" OnClientClick="SelectScore();" OnClick="lnkScore_Click"><font color="blue">(评分)</font></asp:LinkButton>
                                        &nbsp; 等级：<asp:Label ID="lbllevel" runat="server" Font-Size="9pt"></asp:Label>
                                    </td>
                                    <td align="right">
                                        访问次数：<asp:Label ID="labReadCount" runat="server" Font-Size="9pt"></asp:Label>&nbsp;
                                        知识来源：<asp:Literal ID="labSource" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="word-break: break-all; height: 85px;" class="listNoAlign">
                            <div>
                                &nbsp;<asp:Label ID="LblContent" runat="server"></asp:Label></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 100%" class="listNoAlign">
                            <br />
                            <uc1:ctrattachment ID="Ctrattachment1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <table width="90%" align="center" id="TableImg" runat="server" class="listNewContent">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            知识评论
                        </td>
                    </tr>
                </table>
            </td>
            <td align="center" class="listTitleNew" style="width: 80px" nowrap>
                评论次数：<asp:Label ID="lblcount" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table1" cellpadding="0" width="90%" align="center" runat="server">
        <tr>
            <td>
                <asp:DataList ID="DataList1" runat="server" DataKeyField="ID" Width="100%" ShowFooter="true"
                    OnItemCommand="DataList1_ItemCommand" OnItemDataBound="DataList1_ItemDataBound">
                    <ItemTemplate>
                        <table width="100%" class="listContent">
                            <tr>
                                <td class="listTitle" width="10%">
                                    <asp:Label ID="lbluserTitel" runat="server" Text='评论人：'></asp:Label>
                                </td>
                                <td width="40%" class="list">
                                    <asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserName")%>'></asp:Label>
                                </td>
                                <td class="listTitle" width="10%">
                                    <asp:Label ID="Label1" runat="server" Text='评论时间：'></asp:Label>
                                </td>
                                <td width="40%" class="list">
                                    <asp:Label ID="RegTimeLabel" runat="server" Text='<%# Eval("RegTime") %>'></asp:Label>
                                </td>
                                <td nowrap class="list">
                                    <asp:LinkButton ID="btnDelete" ForeColor="blue" runat="server" Text="删除评论" CommandName="delete"
                                        Visible="false" OnClientClick="Infdelete_confirm();" />
                                </td>
                            </tr>
                            <tr>
                                <td class="list" colspan="5" width="100%" style="word-break: break-all">
                                    <asp:Label ID="lblContent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Content")%>'></asp:Label>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblKBID" runat="server" Text='<%# Eval("KBID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID") %>' Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <table width="100%" class="listContent">
                            <tr>
                                <td align="right" class="listTitle">
                                    <asp:LinkButton ID="btnAdd" ForeColor="blue" runat="server" Text="新增评论" CommandName="add"
                                        OnClientClick="CheckValid(this);" />
                                </td>
                            </tr>
                            <tr>
                                <td class="list">
                                    <asp:TextBox ID="txtContent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Content")%>'
                                        Width="90%" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </FooterTemplate>
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <br />
    <table width="90%" align="center" id="Table3" runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            相关知识
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table2" width="90%" align="center" runat="server" cellpadding="0" cellspacing0="0"
        border="0">
        <tr>
            <td class="listContent">
                <asp:DataGrid ID="dgPro_ProblemAnalyse" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    ShowHeader="false" OnItemCreated="dgPro_ProblemAnalyse_ItemCreated">
                    <Columns>
                        <asp:BoundColumn DataField='KBID' HeaderText='KBID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RelID' HeaderText='RelID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Title' HeaderText='Title' Visible="false"></asp:BoundColumn>
                        <asp:HyperLinkColumn DataTextField="Title" HeaderText="关联知识主题" Target="_blank" DataNavigateUrlField="RelID"
                            DataNavigateUrlFormatString="frmKBShow.aspx?KBID={0}"></asp:HyperLinkColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
