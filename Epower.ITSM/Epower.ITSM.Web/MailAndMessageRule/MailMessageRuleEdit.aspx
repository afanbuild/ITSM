<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="MailMessageRuleEdit.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.MailMessageRuleEdit"
    Title="模板信息编辑" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CustSchemeCtr.ascx" TagName="CustSchemeCtr" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc5" %>
<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc7" %>
<%@ Register Src="../Controls/UserPickerMult.ascx" TagName="UserPickerMult" TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        body:nth-of-type(1) .other_fix_width_for_chrome
        {
            width: 100px !important;
        }
        .fixed-grid-border2
        {
            border-right: 1px #A3C9E1 solid;
        }
        .fixed-grid-border2 td
        {
            border-left: solid 1px #CEE3F2;
            border-right: 0px;
        }
        .fixed-grid-border2 tr
        {
            border-bottom: solid 1px #CEE3F2;
            border-top: solid 1px #CEE3F2;
        }
    </style>

    <script type="Text/javascript" language="Javascript">
function ShowTable(imgCtrl)
{
      var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
      var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
      var TableID = imgCtrl.id.replace("Img","Table");
      var className;
      var objectFullName;
      var tableCtrl;
      objectFullName = <%=tr2.ClientID%>.id;
      className = objectFullName.substring(0,objectFullName.indexOf("tr2")-1);
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

            //由件短信模板
			function SelectTem(obj) 
			{
			    var cboApp=document.getElementById("<%=cboApp.ClientID %>");
			    var systemID = cboApp.options[cboApp.selectedIndex].value;
			    
			    if(systemID=="" || systemID =="-1")
			    {
			        alert("请选择应用名称!");
			        return;
			    }
			    var url="../MailAndMessageRule/MailMessageTemManager.aspx?IsSelect='1'&randomid="+GetRandom()+"&systemID="+systemID+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&typeID=0&objID="+obj.id;
			    window.open(url,"E8OpenWin","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50");
			    
			}
			
			function SelectTem1(obj,type) 
			{
			
			    var cboApp=document.getElementById("<%=cboApp.ClientID %>");
			    var systemID = cboApp.options[cboApp.selectedIndex].value;
			    
			    if(systemID=="" || systemID =="-1")
			    {
			        alert("请选择应用名称!");
			        return;
			    }

			    window.open("../MailAndMessageRule/MailMessageTemManager.aspx?IsSelect='1'&randomid="+GetRandom()+"&systemID="+systemID+"&objID="+obj.id+"&typeID="+type,"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no");
			   
			}
	function SubmitValidate()
	{   
	    if(document.all.<%=hidTNname.ClientID%>.value=="")
	    {
	        alert("邮件短信模板不能为空！");
	        return false;
	    }
	    return true;
	}
    </script>

    <table id="Table1" width="98%" align="center" runat="server" class="listNewContent"
        style="display: none">
        <tr id="tr2" runat="server">
            <td valign="top" class="listTitleNew">
                <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                    width="16" />短信邮件规则
                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
                    type="hidden" />
            </td>
        </tr>
    </table>
    <table style="width: 98%" class="listContent" align="center" runat="server" id="Table2">
        <tr id="ShowRefUser" runat="server">
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="LitRuleName" runat="server" Text="规则名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%" colspan="3">
                <uc4:CtrFlowFormText ID="CtrRuleName" runat="server" MustInput="true" TextToolTip="规则名称"
                    MaxLength="50" Width="80%" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " style="width: 12%">
                应用名称
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="cboApp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboApp_SelectedIndexChanged"
                    Width="152px">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight " style="width: 12%">
                流程模型名称
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="cboFlowModel" runat="server" Width="152px" AutoPostBack="True"
                    OnSelectedIndexChanged="cboFlowModel_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="litStatus" runat="server" Text="是否启用"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152px">
                    <asp:ListItem Text="" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                    <asp:ListItem Text="启用" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="listTitleRight ">
                <asp:Literal ID="Literal2" runat="server" Text="短信模板"></asp:Literal>
            </td>
            <td class="list">
                <input type="hidden" id="hidTNid" runat="server" />
                <input type="hidden" id="hidTNname" runat="server" />
                <asp:TextBox ID="txtTName" runat="server" MaxLength="80"></asp:TextBox>
                <input id="cmdTem" onclick="SelectTem(this)" type="button" value="..." runat="server"
                    name="cmdTem" class="btnClass2" />
                <asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td class="listTitleRight " style="width: 12%">
                短信接收类型
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddlReceiversType" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight " style="width: 12%">
                短信发送类型
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddlSenderType" runat="server" Width="152px">
                    <asp:ListItem Text="提交时" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="display: none;">
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="LitMailContent" runat="server" Text="过期小时数"></asp:Literal>
            </td>
            <td class="list" style="width: 35%" colspan="3">
                <uc7:CtrFlowNumeric ID="CtrFlowTimeCount" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="Literal1" runat="server" Text="其它接收人员列表"></asp:Literal>
            </td>
            <td class="list" style="width: 35%" colspan="3">
                <uc9:UserPickerMult ID="UserPickerMult1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="LitModelContent" runat="server" Text="邮件标题"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <uc4:CtrFlowFormText ID="CtrFlowMailTitle" runat="server" Width="85%" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight ">
                <asp:Literal ID="LitRemark1" runat="server" Text="备注"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtRemark" runat="server" Width="90%" Rows="3" TextMode="MultiLine"
                    onblur="MaxLength(this,200,'备注长度超出限定长度:');"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="98%">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img4" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                runat="server" width="16" align="absbottom" />
                            规则设置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table4" runat="server" width="98%" class="listContent" cellpadding="0"
        border="0">
        <tr>
            <td style="width: 100%">
                <asp:DataGrid ID="dgCondition" runat="server" Width="100%" OnItemDataBound="dgCondition_ItemDataBound"
                    CssClass="fixed-grid-border2" OnItemCommand="dgCondition_ItemCommand" ShowFooter="true"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundColumn DataField="id" Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="环节名称">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
                            <ItemTemplate>
                                <input type="hidden" id="hidNodeId" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeId") %>' />
                                <asp:DropDownList ID="drpNodeName" runat="server">
                                </asp:DropDownList>
                                <input type="hidden" id="hidNodeName" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeName") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <input id="HidAddNodeId" type="hidden" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeId") %>' />
                                <asp:DropDownList ID="drpAddNodeName" runat="server">
                                </asp:DropDownList>
                                <input id="HidAddNodeName" type="hidden" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeName") %>' />
                                <asp:Label ID="rnode" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="标题">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
                            <ItemTemplate>
                                <input id="HidNodeContent" type="hidden" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeContent") %>' />
                                <uc4:CtrFlowFormText ID="CtrFlowMailTitle" Value='<%#DataBinder.Eval(Container,"DataItem.NodeContent") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="140px" Wrap="True" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <input id="HidAddNodeContent" type="hidden" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeContent") %>' />
                                            <uc4:CtrFlowFormText ID="CtrFlowAddMailTitle" Value='<%#DataBinder.Eval(Container,"DataItem.NodeContent") %>'
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="环节关联模板">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hidTNid" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.FlowNameId") %>' />
                                            <input type="hidden" id="hidTNname" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.FlowName") %>' />
                                            <asp:TextBox ID="txtTName" runat="server" MaxLength="80" onfocus="this.blur(); "></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="cmdTem" type="button" Text="..." Width="20" Style="width: 20px;"
                                                runat="server" CommandName="cmdTemCommand" name="cmdTem" SkinID="btnClass2" OnClientClick="SelectTem1(this,'1')" />
                                            <%--onclick="SelectTem1(this,'1')"--%>
                                            <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
                                                type="hidden" />
                                            <asp:HiddenField ID="typeValue" runat="server" Value="1" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hidAddTNid" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.FlowNameId") %>' />
                                            <input type="hidden" id="hidAddTNname" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.FlowName") %>' />
                                            <asp:TextBox ID="txtAddTName" runat="server" MaxLength="80" onfocus="this.blur(); "></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="cmdAddTem" type="button" Text="..." CommandName="cmdTemCommand" runat="server"
                                                name="cmdAddTem" SkinID="btnClass2" OnClientClick="SelectTem1(this,'2')" /><%-- onclick="SelectTem1(this,'2')"--%>
                                            <asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
                                            <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
                                                type="hidden" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">                                            
                                            <asp:HiddenField ID="typeValue" runat="server" Value="2" />
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="接收类型">
                            <ItemTemplate>
                                <input id="HidReceiverType" type="hidden" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.ReceiverStypeName") %>' />
                                <asp:DropDownList ID="ddlReceiversType" runat="server" Width="100px">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <input id="HidAddReceiverType" type="hidden" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.ReceiverStypeName") %>' />
                                            <asp:DropDownList ID="ddlAddReceiversType" runat="server" Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="触发类型">
                            <ItemTemplate>
                                <input type="hidden" id="hiddropTriggerType" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.TRIGGER_TYPE") %>' />
                                <asp:DropDownList ID="dropTriggerType" runat="server" Width="100px">
                                    <asp:ListItem Value="Type_link" Text="环节"></asp:ListItem>
                                    <asp:ListItem Value="Type_resWar" Text="响应过期预警"></asp:ListItem>
                                    <asp:ListItem Value="Type_res" Text="响应过期"></asp:ListItem>
                                    <asp:ListItem Value="Type_hanWar" Text="处理过期预警"></asp:ListItem>
                                    <asp:ListItem Value="Type_han" Text="处理过期"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hiddropAddTriggerType" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.TRIGGER_TYPE") %>' />
                                            <asp:DropDownList ID="dropAddTriggerType" runat="server" Width="100px">
                                                <asp:ListItem Value="Type_link" Text="环节"></asp:ListItem>
                                                <asp:ListItem Value="Type_resWar" Text="响应过期预警"></asp:ListItem>
                                                <asp:ListItem Value="Type_res" Text="响应过期"></asp:ListItem>
                                                <asp:ListItem Value="Type_hanWar" Text="处理过期预警"></asp:ListItem>
                                                <asp:ListItem Value="Type_han" Text="处理过期"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="时间(分钟)">
                            <ItemTemplate>
                                <input type="hidden" id="hidtxt_Time" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.INTERVAL_TIME") %>' />
                                <input type="hidden" id="Hidtime" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.INTERVAL_TIME") %>' />
                                <uc7:CtrFlowNumeric ID="txt_Time" runat="server" Width="80px" />
                            </ItemTemplate>
                            <FooterStyle Width="80px"></FooterStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc7:CtrFlowNumeric ID="txt_TimeAdd" runat="server" Width="80px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="接收人">
                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
                            <ItemTemplate>
                                <input type="hidden" id="hiduc_User" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.Recipient_User") %>' />
                                <input type="hidden" id="HidUserID" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.Recipient_UserID") %>' />
                                <uc9:UserPickerMult ID="uc_user" runat="server" Width="80px" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc9:UserPickerMult ID="uc_userAdd" runat="server" Width="80px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="删除">
                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" OnClientClick="return confirm('确定删除吗？')"
                                                CausesValidation="False" SkinID="btnClass1"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnadd" CommandName="Add" runat="server" Text="新增" CausesValidation="False"
                                                SkinID="btnClass1"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="listTitle" />
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
