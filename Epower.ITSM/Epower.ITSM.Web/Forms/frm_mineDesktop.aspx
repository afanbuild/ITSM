<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_mineDesktop.aspx.cs"
    Inherits="Epower.ITSM.Web.frm_mineDesktop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>服 务 台</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</head>

<script language="javascript" type="text/javascript">
        function showurl(strurl,stype)
        {
            if(stype=='1' && '<%=ServiceRights%>'=="true")
            {
                top.MainFrame.location= strurl;
                return;
            }
            else if(stype=='2' && '<%=BYTSApplyRights%>'=="true")
            {
                top.MainFrame.location= strurl;
                return;
            }
            else if(stype=='3' &&  '<%=CustomerServiceRights%>'=="true")
            {
                top.MainFrame.location= strurl;
                return;
            }
            else if(stype=='4')
            {
                top.MainFrame.location= strurl;
                return;
            }
            else
            {
                alert("你没有权限操作，请联系系统管理员！");
                return;
            }
        }
        
        function showurl2(strurl)
        {
            top.MainFrame.location= strurl;
            return;
        }
</script>

<body topmargin="0" leftmargin="0" rightmargin="0" bottommargin="0">
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td height="4">
            </td>
        </tr>
        <tr>
            <td  align="left" valign="top" class="list">
                <table width="95%" border="0" cellspacing="0" cellpadding="0" align="center">
                    <tr>
                        <td colspan="2" align="center">
                            <table width="100%" >
                                <tr>
                                    <td width="15%" class="listTdService">
                                        <span style="cursor: hand" onclick="showurl('../Forms/form_all_flowmodel.aspx?appid=1026','1');">
                                            <img src="../Images/dengjishijian.jpg" alt="事件单登记" align="absMiddle" border="0" /></span>
                                    </td>
                                    <td width="15%" class="listTdService">
                                        <span style="cursor: hand" onclick="showurl('../AppForms/Cst_Issue_List.aspx','3');">
                                            <img src="../Images/shijiangenzong.jpg" alt="事件单跟踪" align="absMiddle" border="0" /></span>
                                    </td>
                                     <td width="15%" class="listTdService">
                                        <span style="cursor: hand" onclick="top.MainFrame.location='../Forms/frmWaittingContent.aspx?TypeContent=MyReg';">
                                            <img src="../Images/shijiandengji.jpg" alt="我登记事件" align="absMiddle" border="0" /></span>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td style="font-size: 10pt" class="listTdService">
                                        <span style="cursor: hand" onclick="showurl('../Forms/form_all_flowmodel.aspx?appid=1026','1');">
                                            事件单登记 </span>
                                    </td>
                                    
                                    <td style="font-size: 10pt" class="listTdService">
                                        <span style="cursor: hand" onclick="showurl('../AppForms/Cst_Issue_List.aspx','3');">
                                            事件单跟踪 </span>
                                    </td>
                                     <td style="font-size: 10pt" class="listTdService">
                                        <span style="cursor: hand" onclick="top.MainFrame.location='../Forms/frmWaittingContent.aspx?TypeContent=MyReg';">
                                            我登记事件 </span>
                                    </td>
                                 </tr>
                                 <tr>
                                <td width="15%" class="listTdService">
                                        <span style="cursor: hand" onclick="showurl2('../Forms/frmcontent.aspx');">
                                            <img src="../Images/daijie.jpg" alt="待接收事项" align="absMiddle" border="0" /></span>
                                    </td>
                                    
                                    <td width="15%" class="listTdService">
                                        <span style="cursor: hand" onclick="showurl2('../Forms/frmcontent.aspx');">
                                            <img src="../Images/daiban.jpg" alt="待办事项" align="absMiddle" border="0" /></span>
                                    </td>
                                    <td width="15%" class="listTdService">
                                        <span style="cursor: hand" onclick="showurl('frm_Hastodoitems.aspx','4');">
                                            <img src="../Images/yiban.jpg" alt="我已办事件" align="absMiddle" border="0" /></span>
                                    </td>
                                    
                                </tr>
                                 <tr>
                                <td style="font-size: 10pt" class="listTdService">
                                     <span style="cursor:hand" onclick="showurl2('../Forms/frmcontent.aspx');">
                                            <asp:Label ID="Lab_a" runat="server" Font-Size="10pt"  Text="没有待接收事项" Width="100%"></asp:Label>
                                     </span>
                                 </td>
                                
                                 <td style="font-size: 10pt" class="listTdService">
                                    <span style="cursor:hand" onclick="showurl2('../Forms/frmcontent.aspx');">
                                            <asp:Label ID="Lab_b" runat="server" Font-Size="10pt" Text="没有待办事项" Width="100%"></asp:Label>
                                    </span>
                                 </td>
                                     <td style="font-size: 10pt" class="listTdService">
                                        <span style="cursor: hand" onclick="showurl('frm_Hastodoitems.aspx','4');">
                                            我已办事件 </span>
                                    </td>
                                </tr>
                                 <tr>
                                 
                                    
                                     <td width="15%" class="listTdService">
                                        <span style="cursor: hand" onclick="top.MainFrame.location='../InformationManager/frmInf_MainShow.htm';">
                                            <img src="../Images/sousuo.jpg" alt="知识搜索" align="absMiddle" border="0" /></span>
                                    </td>
                                     <td width="15%" class="listTdService">
                                     </td>
                                    <td width="15%" class="listTdService">
                                        <span style="cursor: hand" onclick="top.MainFrame.location='../Forms/frmAgentSet.aspx';">
                                            <img src="../Images/chucai.jpg" alt="出差授权" align="absMiddle" border="0" /></span>
                                    </td>
                                </tr>
                                 <tr>
                                 
                                     <td style="font-size: 10pt" class="listTdService">
                                        <span style="cursor: hand" onclick="top.MainFrame.location='../InformationManager/frmInf_MainShow.htm';">
                                            知识搜索 </span>
                                    </td>
                                     <td style="font-size: 10pt" class="listTdService">
                                 </td>
                                    <td style="font-size: 10pt" class="listTdService">
                                        <span style="cursor: hand" onclick="top.MainFrame.location='../Forms/frmAgentSet.aspx';">
                                            出差授权 </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
