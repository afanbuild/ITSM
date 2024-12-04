<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="True"
    ValidateRequest="false" CodeBehind="app_pub_normal_form_New.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.app_pub_normal_form_New"
    Title="无标题页" %>

<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc5" %>
<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc6" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ MasterType VirtualPath="~/FlowForms.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" src="../Js/App_Common.js"> </script>

    <script language="javascript" src="../Js/App_Base.js"> </script>

    <script language="javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" src="../JS/ows.js"></script>

    <script language="javascript" src="../JS/OWSBROWS.JS"></script>

    <script language="javascript">		
		function TransferValue()
		{
		    if (typeof(document.all.<%=txtFlowName.ClientID%>)!="undefined" )
			    parent.header.flowInfo.Subject.value = document.all.<%=txtFlowName.ClientID%>.value;
		}
		
		function DoUserValidate(lngActionID,strActionName)
		{
		    TransferValue();
		    if (typeof(document.all.<%=txtFlowName.ClientID%>)!="undefined" )
		    {
	            if (document.all.<%=txtFlowName.ClientID%>.value.trim()=="")         //标题

		        {
		            document.all.<%=txtFlowName.ClientID%>.focus();
			        alert("标题不能为空！");
			        return false;
		        }
		    }
		}
		
		String.prototype.trim = function()  //去空格

			{
				// 用正则表达式将前后空格

				// 用空字符串替代。

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}	
			
			
            
             //打印
            function printdiv()
            {
                window.open("frmPrintNormal.aspx?lngFlowModelID=" + <%=lngFlowModelID%> + "&lngMessageID=" + <%=lngMessageID%>,"","scrollbars=yes,resizable=yes,top=0,left=0;");
                return false;
            }
            
            
            function ShowTable(imgCtrl)
            {
                  var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
                  var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
                  var TableID = imgCtrl.id.replace("Img","Table");
                  var tableCtrl;
                  tableCtrl = document.all.item(TableID);
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

    <div id="PrintArea" runat="server">
        <table id="head_00001"
            width="100%" align="center" class="listNewContent">
            <tbody>
                <tr class="listTitleNew">
                    <td align="left" valign="top" class="bt_di">
                        <img src="../Images/icon_collapseall.gif"
                            class="icon" onclick="display('head_00001');"
                            height="16" width="16" align="absbottom">
                            <span>基础信息</span>
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="listContent" id="tabMain" width="100%" align="center">
            <tr>
                <td nowrap align="left" width="12%" colspan="1" rowspan="1" class="listTitleRight">
                    标题
                </td>
                <td colspan="3" class="list" >
                    <asp:Label ID="labFlowName" runat="server" Visible="False"></asp:Label><asp:TextBox
                        ID="txtFlowName" runat="server" Width="70%" MaxLength="50"></asp:TextBox>
                    <asp:Label ID="lblScript" runat="server"></asp:Label>
                    <asp:Label ID="rFlowName" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
                </td>
            </tr>
            <tr>
                <td nowrap align="left" class="listTitleRight" width="12%">
                    申请人
                </td>
                <td class="list" width="38%">
                    <asp:Label ID="labApplyName" runat="server"></asp:Label>
                </td>
                <td nowrap align="left" class="listTitleRight" width="12%">
                    所属部门
                </td>
                <td class="list">
                    <asp:Label ID="labDeptName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="12%" nowrap align="left" class="listTitleRight">
                    开始时间
                </td>
                <td class="list" width="35%">
                    <asp:Label ID="labStartDate" runat="server"></asp:Label>
                </td>
                <td width="12%" nowrap align="left" class="listTitleRight">
                    
                </td>
                <td class="list">
                    <asp:Label ID="labEndDate" Visible="false" runat="server"></asp:Label>
                </td>
            </tr>

        </table>
        
        <table class="listContent" id="tabList" width="100%" align="center">
            <tbody></tbody>
        </table>
        
                
        
        <div id="field_container" style="display: none;">
            <div id="ShowDate1" runat="server">
                <div width="12%" align="left" class="listTitleRight">
                    <asp:Label ID="subDate1" runat="server">日期1</asp:Label>
                </div>
                <div class="list">
                    <uc3:ctrdateandtime ID="Ctrdateandtime1" runat="server" />
                </div>
            </div>
            <div id="ShowDate2" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subDate2" runat="server">日期2</asp:Label>
                </div>
                <div class="list">
                    <uc3:ctrdateandtime ID="Ctrdateandtime2" runat="server" />
                    
                    
                </div>
            </div>
            <div id="ShowDate3" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subDate3" runat="server">日期3</asp:Label>
                </div>
                <div class="list">
                    <uc3:ctrdateandtime ID="Ctrdateandtime3" runat="server" />
                </div>
            </div>
            <div id="ShowDate4" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subDate4" runat="server">日期4</asp:Label>
                </div>
                <div class="list">
                    <uc3:ctrdateandtime ID="Ctrdateandtime4" runat="server" />
                </div>
            </div>
            <div id="ShowDate5" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subDate5" runat="server">日期5</asp:Label></div>
                <div class="list">
                    <uc3:ctrdateandtime ID="Ctrdateandtime5" runat="server" />
                </div>
            </div>
            <div id="ShowDate6" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subDate6" runat="server">日期6</asp:Label></div>
                <div class="list">
                    <uc3:ctrdateandtime ID="Ctrdateandtime6" runat="server" />
                </div>
            </div>
            <div id="ShowDate7" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subDate7" runat="server">日期7</asp:Label></div>
                <div class="list">
                    <uc3:ctrdateandtime ID="Ctrdateandtime7" runat="server" />
                </div>
            </div>
            <div id="ShowDate8" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subDate8" runat="server">日期8</asp:Label></div>
                <div class="list">
                    <uc3:ctrdateandtime ID="Ctrdateandtime8" runat="server" />
                </div>
            </div>
            <div id="ShowCate1" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subCate1" runat="server">分类1</asp:Label></div>
                <div class="list">
                    <uc2:ctrFlowCataDropList ID="CtrFCate1" runat="server" />
                </div>
            </div>
            <div id="ShowCate2" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subCate2" runat="server">分类1</asp:Label></div>
                <div class="list">
                    <uc2:ctrFlowCataDropList ID="CtrFCate2" runat="server" />
                </div>
            </div>
            <div id="ShowCate3" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subCate3" runat="server">分类1</asp:Label></div>
                <div class="list">
                    <uc2:ctrFlowCataDropList ID="CtrFCate3" runat="server" />
                </div>
            </div>
            <div id="ShowCate4" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subCate4" runat="server">分类1</asp:Label></div>
                <div class="list">
                    <uc2:ctrFlowCataDropList ID="CtrFCate4" runat="server" />
                </div>
            </div>
            <div id="ShowCate5" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subCate5" runat="server">分类1</asp:Label></div>
                <div class="list">
                    <uc2:ctrFlowCataDropList ID="CtrFCate5" runat="server" />
                </div>
            </div>
            <div id="ShowString1" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subString1" runat="server">字符1</asp:Label></div>
                <div class="list">
                    <uc4:CtrFlowFormText ID="CtrFlowFormText1" runat="server" MaxLength="50" />
                </div>
            </div>
            <div id="ShowString2" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subString2" runat="server">字符2</asp:Label></div>
                <div class="list">
                    <uc4:CtrFlowFormText ID="CtrFlowFormText2" runat="server" MaxLength="50" />
                </div>
            </div>
            <div id="ShowString3" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subString3" runat="server">字符3</asp:Label></div>
                <div class="list">
                    <uc4:CtrFlowFormText ID="CtrFlowFormText3" runat="server" MaxLength="50" EnableViewState="false" />
                </div>
            </div>
            <div id="ShowString4" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subString4" runat="server">字符4</asp:Label></div>
                <div class="list">
                    <uc4:CtrFlowFormText ID="CtrFlowFormText4" runat="server" MaxLength="50" />
                </div>
            </div>
            <div id="ShowString5" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subString5" runat="server">字符5</asp:Label></div>
                <div class="list">
                    <uc4:CtrFlowFormText ID="CtrFlowFormText5" runat="server" MaxLength="50" />
                </div>
            </div>
            <div id="ShowString6" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subString6" runat="server">字符6</asp:Label></div>
                <div class="list">
                    <uc4:CtrFlowFormText ID="CtrFlowFormText6" runat="server" MaxLength="50" />
                </div>
            </div>
            <div id="ShowString7" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subString7" runat="server">字符7</asp:Label></div>
                <div class="list">
                    <uc4:CtrFlowFormText ID="CtrFlowFormText7" runat="server" MaxLength="50" />
                </div>
            </div>
            <div id="ShowString8" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subString8" runat="server">字符8</asp:Label></div>
                <div class="list">
                    <uc4:CtrFlowFormText ID="CtrFlowFormText8" runat="server" MaxLength="50" />
                </div>
            </div>
            <div id="ShowNumber1" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subNumber1" runat="server">数值1</asp:Label></div>
                <div class="list">
                    <uc5:CtrFlowNumeric ID="CtrFlowNumeric1" runat="server" />
                </div>
            </div>
            <div id="ShowNumber2" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subNumber2" runat="server">数值1</asp:Label></div>
                <div class="list">
                    <uc5:CtrFlowNumeric ID="CtrFlowNumeric2" runat="server" />
                </div>
            </div>
            <div id="ShowNumber3" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subNumber3" runat="server">数值1</asp:Label></div>
                <div class="list">
                    <uc5:CtrFlowNumeric ID="CtrFlowNumeric3" runat="server" />
                </div>
            </div>
            <div id="ShowNumber4" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subNumber4" runat="server">数值1</asp:Label></div>
                <div class="list">
                    <uc5:CtrFlowNumeric ID="CtrFlowNumeric4" runat="server" />
                </div>
            </div>
            <div id="ShowNumber5" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subNumber5" runat="server">数值5</asp:Label></div>
                <div class="list">
                    <uc5:CtrFlowNumeric ID="CtrFlowNumeric5" runat="server" />
                </div>
            </div>
            <div id="ShowBool1" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subBool1" runat="server">判断1</asp:Label></div>
                <div class="list">
                    <asp:CheckBox ID="chkBool1" runat="server"></asp:CheckBox></div>
            </div>
            <div id="ShowBool2" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subBool2" runat="server">判断2</asp:Label></div>
                <div class="list">
                    <asp:CheckBox ID="chkBool2" runat="server"></asp:CheckBox></div>
            </div>
            <div id="ShowBool3" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subBool3" runat="server">判断3</asp:Label></div>
                <div class="list">
                    <asp:CheckBox ID="chkBool3" runat="server"></asp:CheckBox></div>
            </div>
            <div id="ShowBool4" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subBool4" runat="server">判断4</asp:Label></div>
                <div class="list">
                    <asp:CheckBox ID="chkBool4" runat="server"></asp:CheckBox></div>
            </div>
            <div id="ShowRemark1" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subRemark1" runat="server">备注1</asp:Label></div>
                <div class="list">
                    <uc6:CtrFlowRemark ID="CtrFlowRemark1" runat="server" MaxLength="250" />                    
                </div>
            </div>
            <div id="ShowRemark2" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subRemark2" runat="server">备注2</asp:Label></div>
                <div class="list">
                    <uc6:CtrFlowRemark ID="CtrFlowRemark2" runat="server" MaxLength="250" />
                </div>
            </div>
            <div id="ShowRemark3" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subRemark3" runat="server">备注3</asp:Label></div>
                <div class="list">
                    <uc6:CtrFlowRemark ID="CtrFlowRemark3" runat="server" MaxLength="250" />
                </div>
            </div>
            <div id="ShowRemark4" runat="server">
                <div align="left" class="listTitleRight">
                    <asp:Label ID="subRemark4" runat="server">备注4</asp:Label></div>
                <div class="list">
                    <uc6:CtrFlowRemark ID="CtrFlowRemark4" runat="server" MaxLength="250" />
                </div>
            </div>
        </div>
        
        <table class="listContent" id="tabDesc" width="100%" align="center">
                    
            <tr id="ShowDesc" runat="server">
                <td colspan="4" class="list">
                    <table id="Table12" width="100%" runat="server" class="listNewContent">
                        <tr id="tr2" runat="server">
                            <td valign="top" align="left" class="listTitleNew">
                                <img class="icon" alt="" id="Img2" onclick="ShowTable(this);" style="cursor: hand"
                                    height="16" src="../Images/icon_collapseall.gif" width="16" /><asp:Label ID="subFbox"
                                        runat="server">复杂表单</asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table class="listContent" width="100%" id="Table2">
                        <tr>
                            <td colspan="2" class="list">
                                <asp:Label ID="lblDesc" runat="server" Visible="false"></asp:Label>
                                <ftb:FreeTextBox ID="ftxtDesc" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                                    Height="300px" ImageGalleryPath="Attfiles\\Photos" Width="100%">
                                </ftb:FreeTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>          
              
        </table>
        
    </div>
               
    <script type="text/javascript" language="javascript">
        window.flowModelID = <%=lngFlowModelID%>;
        window.ctrl_prefix = '<%=ShowDate1.ClientID %>'.replace('ShowDate1','');        
    </script>
    <script src="../Js/epower.appforms.normalapp.js" type="text/javascript"></script>    


    
</asp:Content>
