﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_Services_ListDtl.aspx.cs"
    Inherits="Epower.ITSM.Web.AppForms.frm_Services_ListDtl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>服务请求</title>
</head>

<script language="javascript" type="text/javascript">
    function AddNewFlow(id, templateid)  //删除流程
    {
        var isnew = 'False';
        var isstrExtPara = templateid;
        if (isnew == 'False') {
            if (isstrExtPara != '') {
                window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + id + "&ep=" + isstrExtPara, "MainFrame", "scrollbars=yes,resizable=yes,top=0,left=0,width=" + (window.availWidth - 12) + ",height=" + (window.availHeight - 35));
            }
            else {
                window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + id, "MainFrame", "scrollbars=yes,resizable=yes,top=0,left=0,width=" + (window.availWidth - 12) + ",height=" + (window.availHeight - 35));
            }
        }
        else {
            if (isstrExtPara != '') {
                window.open("../Forms/OA_AddNew.aspx?NewWin=true&flowmodelid=" + id + "&ep=" + isstrExtPara, "_self", "scrollbars=yes,resizable=yes,top=0,left=0,width=" + (window.availWidth - 12) + ",height=" + (window.availHeight - 35));
            }
            else {
                window.open("../Forms/OA_AddNew.aspx?NewWin=true&flowmodelid=" + id, "_self", "scrollbars=yes,resizable=yes,top=0,left=0,width=" + (window.availWidth - 12) + ",height=" + (window.availHeight - 35));
            }
        }
    } 
</script>

<body>
    <form id="form1" runat="server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>   
            <td rowspan="2" style="width: 70%" valign="top">
                <div>
                    <asp:Repeater runat="server" ID="tt" OnItemDataBound="tt_ItemDataBound">
                        <HeaderTemplate>
                            <div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <table width="95%" align="center" class="xuxian">
                                    <tr>
                                        <td width="9%" align="left">
                                            <img height="70" width="70" src="<%#GetImgSrc(DataBinder.Eval(Container.DataItem, "imglogo")) %>" onerror="this.src='../images/tb_001.gif'" />
                                        </td>
                                           <td width="74%" align="left">
                                            <a class="td_3" href="frm_Services_ListDtl.aspx?ServiceLevelID=<%#Eval("TemplateID") %>">
                                                <%#Eval("TemplateName")%></a><br />
                                            <%#Eval("Content")%>
                                        </td>
                                        <td id="tddown" runat="server" width="17%" align="center">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </td>
            <td valign="top">
                <table cellspacing='0' width='100%' cellpadding='1' border='0' class='bian'>
                    <tr class='sy_lmbg'>
                        <td id='module_@i_head' width='58%'>
                            公告通知
                        </td>
                        <td id='module_@i_more' align='right' width='42%'>
                            <div style='float: right;'>
                                <a href='../Forms/frmPane.aspx?Limit=false=ggtz'>
                                    <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td height='10' class='list' colspan="2">
                            <iframe id='iframe1' src="../Forms/frmPane.aspx?TypeContent=ggtz" width='100%' height='280'
                                frameborder='no' scrolling='no'></iframe>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#EEF5FB"
                    class="bian">
                    <tr>
                        <td height="5">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="95%" height="95%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                                class="bian">
                                <tr>
                                    <td height="25" colspan="2" align="center">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="81%">
                                                    &nbsp;<img height="20" src="../images/biaote_2.gif" width="20" align="absMiddle"><span
                                                        style="font-size: 11pt; width: 122px; color: firebrick; height: 24px"> 最新知识</span>
                                                </td>
                                                <td width="19%">
                                                    <a href="../InformationManager/frmInf_MainShow.htm">
                                                        <img src="../images/more-2.gif" border="0"></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td background="../images/xuxian.gif" colspan="2" align="center" height="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" align="right" width="50">
                                        <asp:Image ID="Image6" runat="server" ImageUrl="../Images/biaote_3.gif" Visible="False" />
                                    </td>
                                    <td width="84%" height="23">
                                        <asp:Label ID="lblInf1" runat="server" Font-Size="10pt" Text="" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" align="right">
                                        <asp:Image ID="Image7" runat="server" ImageUrl="../Images/biaote_3.gif" Visible="False" />
                                    </td>
                                    <td height="23">
                                        <asp:Label ID="lblInf2" runat="server" Font-Size="10pt" Text="" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" align="right">
                                        <asp:Image ID="Image8" runat="server" ImageUrl="../Images/biaote_3.gif" Visible="False" />
                                    </td>
                                    <td height="23">
                                        <asp:Label ID="lblInf3" runat="server" Font-Size="10pt" Text="" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" align="right">
                                        <asp:Image ID="Image9" runat="server" ImageUrl="../Images/biaote_3.gif" Visible="False" />
                                    </td>
                                    <td height="23">
                                        <asp:Label ID="lblInf4" runat="server" Font-Size="10pt" Text="" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" align="right">
                                        <asp:Image ID="Image10" runat="server" ImageUrl="../Images/biaote_3.gif" Visible="False" />
                                    </td>
                                    <td height="23">
                                        <asp:Label ID="lblInf5" runat="server" Font-Size="10pt" Text="" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="5">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
  
    </form>
</body>
</html>