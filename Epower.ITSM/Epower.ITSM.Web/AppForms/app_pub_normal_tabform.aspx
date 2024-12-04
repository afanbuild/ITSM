<%@ Page Language="C#" MasterPageFile="~/FlowFormsTab.Master" AutoEventWireup="True" validateRequest="false" CodeBehind="app_pub_normal_tabform.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.app_pub_normal_tabform" Title="�ޱ���ҳ" %>

<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc5" %>
<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc6" %>

<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"  TagPrefix="uc2" %>

<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<%@ MasterType VirtualPath="~/FlowFormsTab.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
		<script language="javascript" src="../Js/App_Common.js"> </script>
		<script language="javascript" src="../Js/App_Base.js"> </script>
		<script language="javascript" src="../Controls/Calendar/Popup.js"></script>
		<SCRIPT language="javascript" src="../JS/ows.js"></SCRIPT>
		<SCRIPT language="javascript" src="../JS/OWSBROWS.JS"></SCRIPT>
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
	            if (document.all.<%=txtFlowName.ClientID%>.value.trim()=="")         //����

		        {
		            //document.all.<%=txtFlowName.ClientID%>.focus();
			        alert("���ⲻ��Ϊ�գ�");
			        return false;
		        }
		    }
		}
		
		String.prototype.trim = function()  //ȥ�ո�

			{
				// ��������ʽ��ǰ��ո�

				// �ÿ��ַ��������

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}	
			
			
            
             //��ӡ
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
	<table class="listContent" id="tabMain"  width="100%" align="center">
	    <tr>
			<td noWrap align="left" width="12%" colSpan="1" rowSpan="1" class="listTitleRight">����</td>
			<td colSpan="3" class="list"><asp:label id="labFlowName" runat="server" Visible="False"></asp:label><asp:textbox id="txtFlowName" runat="server" Width="70%" MaxLength="50"></asp:textbox>
				<asp:label id="lblScript" runat="server"></asp:label>
				<asp:Label ID="rFlowName" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
			</td>
		</tr>
		<tr>
			<td noWrap align="left" class="listTitleRight"  width="12%">������</td>
			<td class="list"><asp:label id="labApplyName" runat="server"></asp:label></td>
			<td noWrap align="left" class="listTitleRight"   width="12%">��������</td>
			<td class="list"><asp:label id="labDeptName" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td width="12%" noWrap align="left" class="listTitleRight">��ʼʱ��</td>
			<td class="list"><asp:label id="labStartDate" runat="server"></asp:label></td>
			<td width="12%"" noWrap align="left" class="listTitleRight">����ʱ��</td>
			<td class="list"><asp:label id="labEndDate" runat="server"></asp:label></td>
		</tr>
		<TR id="ShowDate1" runat="server">
			<TD  width="12%" noWrap align="left" class="listTitleRight"><asp:label id="subDate1" runat="server" >����1</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc3:ctrdateandtime ID="Ctrdateandtime1" runat="server"/>
            </TD>
		</TR>
		<TR id="ShowDate2" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subDate2" runat="server">����2</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc3:ctrdateandtime ID="Ctrdateandtime2" runat="server" />
            </TD>
		</TR>
		<TR id="ShowDate3" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subDate3" runat="server">����3</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc3:ctrdateandtime ID="Ctrdateandtime3" runat="server" />
            </TD>
		</TR>
		<TR id="ShowDate4" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subDate4" runat="server">����4</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc3:ctrdateandtime ID="Ctrdateandtime4" runat="server" />
            </TD>
		</TR>
		<TR id="ShowCate1" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subCate1" runat="server">����1</asp:label></TD>
			<TD colSpan="3" class="list"><uc2:ctrFlowCataDropList ID="CtrFCate1" runat="server" /></TD>
		</TR>
		<TR id="ShowCate2" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subCate2" runat="server">����1</asp:label></TD>
			<TD colSpan="3" class="list"><uc2:ctrFlowCataDropList ID="CtrFCate2" runat="server" /></TD>
		</TR>
		<TR id="ShowCate3" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subCate3" runat="server">����1</asp:label></TD>
			<TD colSpan="3" class="list"><uc2:ctrFlowCataDropList ID="CtrFCate3" runat="server"  /></TD>
		</TR>
		<TR id="ShowCate4" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subCate4" runat="server">����1</asp:label></TD>
			<TD colSpan="3" class="list"><uc2:ctrFlowCataDropList ID="CtrFCate4" runat="server"/></TD>
		</TR>
		<TR id="ShowCate5" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subCate5" runat="server">����1</asp:label></TD>
			<TD colSpan="3" class="list"><uc2:ctrFlowCataDropList ID="CtrFCate5" runat="server" /></TD>
		</TR>
		<TR id="ShowString1" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subString1" runat="server">�ַ�1</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowFormText1" runat="server" MaxLength="50" />
            </TD>
		</TR>
		<TR id="ShowString2" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subString2" runat="server">�ַ�2</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowFormText2" runat="server" MaxLength="50" />
            </TD>
		</TR>
		<TR id="ShowString3" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subString3" runat="server">�ַ�3</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowFormText3" runat="server"  MaxLength="50" />
            </TD>
		</TR>
		<TR id="ShowString4" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subString4" runat="server">�ַ�4</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowFormText4" runat="server" MaxLength="50" />
            </TD>
		</TR>
		<TR id="ShowString5" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subString5" runat="server">�ַ�5</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowFormText5" runat="server" MaxLength="50" />
            </TD>
		</TR>
		<TR id="ShowString6" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subString6" runat="server">�ַ�6</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowFormText6" runat="server" MaxLength="50" />
            </TD>
		</TR>
		<TR id="ShowString7" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subString7" runat="server">�ַ�7</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowFormText7" runat="server" MaxLength="50" />
            </TD>
		</TR>
		<TR id="ShowString8" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subString8" runat="server">�ַ�8</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowFormText8" runat="server" MaxLength="50" />
            </TD>
		</TR>
		<TR id="ShowNumber1" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subNumber1" runat="server">��ֵ1</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc5:CtrFlowNumeric ID="CtrFlowNumeric1" runat="server" />
            </TD>
		</TR>
		<TR id="ShowNumber2" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subNumber2" runat="server">��ֵ1</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc5:CtrFlowNumeric ID="CtrFlowNumeric2" runat="server" />
            </TD>
		</TR>
		<TR id="ShowNumber3" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subNumber3" runat="server">��ֵ1</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc5:CtrFlowNumeric ID="CtrFlowNumeric3" runat="server" />
            </TD>
		</TR>
		<TR id="ShowNumber4" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subNumber4" runat="server">��ֵ1</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc5:CtrFlowNumeric ID="CtrFlowNumeric4" runat="server" />
            </TD>
		</TR>
		<TR id="ShowNumber5" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subNumber5" runat="server">��ֵ5</asp:label></TD>
			<TD colSpan="3" class="list">
                <uc5:CtrFlowNumeric ID="CtrFlowNumeric5" runat="server" />
            </TD>
		</TR>
		<TR id="ShowBool1" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subBool1" runat="server">�ж�1</asp:label></TD>
			<TD colSpan="3" class="list"><asp:checkbox id="chkBool1" runat="server"></asp:checkbox></TD>
		</TR>
		<TR id="ShowBool2" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subBool2" runat="server">�ж�2</asp:label></TD>
			<TD colSpan="3" class="list"><asp:checkbox id="chkBool2" runat="server"></asp:checkbox></TD>
		</TR>
		<TR id="ShowBool3" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subBool3" runat="server">�ж�3</asp:label></TD>
			<TD colSpan="3" class="list"><asp:checkbox id="chkBool3" runat="server"></asp:checkbox></TD>
		</TR>
		<TR id="ShowBool4" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subBool4" runat="server">�ж�4</asp:label></TD>
			<TD colSpan="3" class="list"><asp:checkbox id="chkBool4" runat="server"></asp:checkbox></TD>
		</TR>
		<TR id="ShowRemark1" runat="server">
			<TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitleRight"><asp:label id="subRemark1" runat="server">��ע1</asp:label></TD>
			<TD style="HEIGHT: 17px;word-break:break-all" colSpan="3" class="list">
                <uc6:CtrFlowRemark ID="CtrFlowRemark1" runat="server" MaxLength="250" />
            </TD>
		</TR>
		<TR id="ShowDesc" runat="server">
		        <TD colspan="4" class="list">
		            <table id="Table12" width="100%" runat="server" class="listNewContent">
                        <tr id="tr2" runat="server">
                            <td  valign="top" align="left" class="listTitleNew" >
                                  <img class="icon" alt="" id="Img2" onclick="ShowTable(this);" style="cursor:hand" height="16" src="../Images/icon_collapseall.gif" width="16"/><asp:label id="subFbox" runat="server">���ӱ�</asp:label>
                            </td>
                        </tr>
                    </table>
                    <table class="listContent"  width="100%"  id="Table2" >
                    <TR>
		                        <TD colspan="2" class="list">
		                        <asp:label id="lblDesc" runat="server" Visible="false" ></asp:label>
		                        <ftb:FreeTextBox ID="ftxtDesc" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                                                    Height="300px"  ImageGalleryPath="Attfiles\\Photos" Width="100%" >
                                                </ftb:FreeTextBox>
		                        </TD>
	                        </TR>
                   </table>
		        </TD>
	        </TR>
	</table>
	
</div>
</asp:Content>