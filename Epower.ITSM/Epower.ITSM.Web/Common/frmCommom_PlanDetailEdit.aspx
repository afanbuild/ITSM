<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="frmCommom_PlanDetailEdit.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmCommom_PlanDetailEdit"
    Title="�ޱ���ҳ" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
   function SubmitValidate()         //����ǰ���ݼ���
   {
        if(document.all.<%=ddltFlow.ClientID%>.value.trim() == "")
        {
            alert("ִ�����̲���Ϊ�գ�");
            return false;
        }
        if(document.all.<%=txtPlanName.ClientID%>.value.trim() == "")
        {
            alert("�ƻ����Ʋ���Ϊ�գ�");
            return false;
        }
        if(document.all.ctl00_ContentPlaceHolder1_UserPicker1_hidUserName.value == "")
        {
            alert("�ƻ�ִ���˲���Ϊ�գ�");
            return false;
        }
        return true;
   }
   String.prototype.trim = function()  //ȥ�ո�

			{
				// ��������ʽ��ǰ��ո�

				// �ÿ��ַ��������

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
   function SelectEquItem()
			{
			    var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
		        var features =
		        'dialogWidth:800px;' +
		        'dialogHeight:500px;' +
		        'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';

		        var	value=window.showModalDialog('../EquipmentManager/frmEqu_SelectItem.aspx?pDate='+sparamvalue,"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
		        if(value != null)
                {
	                if(value.length>1)
	                {
                        document.all.<%=hidItemArrID.ClientID%>.value = value[0];   //��ĿID����
                        event.returnValue = true; 
	                }
	                else
	                {
	                    event.returnValue = false;
	                }
                }
                else
                {
                    event.returnValue = false;
                }
			}
	function SelectTimeSet()
			{
			    var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
	            
		        var	value=window.showModalDialog('../AppForms/frmPlanDetail.aspx?pDate='+sparamvalue +'&planset=' +'<%=PlanXml%>',"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=no;resizable=no") ;
		        if(value != null)
                {
	                if(value.length>1)
	                {
                        document.all.<%=hidPlanXml.ClientID%>.value = value;   //�ʲ�ʱ��ֵ
	                }
                }
                event.returnValue = false;
			}
	function ShowTable(imgCtrl)
    {
          var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
          var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
          var TableID = imgCtrl.id.replace("Img","Table");
          var className;
          var objectFullName;
          var tableCtrl;
          objectFullName = '<%=Table12.ClientID%>';
          className = objectFullName.substring(0,objectFullName.indexOf("Table12")-1);
          tableCtrl = document.all.item(className.substr(0,className.length)+"_"+TableID);
          if(imgCtrl.src.indexOf("icon_expandall") != -1)
          {
            tableCtrl.style.display ="";
            imgCtrl.src = ImgMinusScr ;
            var temp = document.all.<%=hidTable.ClientID%>.value;
            document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
          }
          else
          {
            tableCtrl.style.display ="none";
            imgCtrl.src = ImgPlusScr ;		
            document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id; 
          }
    }
    </script>
<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <input id="hidItemArrID" runat="server" type="hidden" />
    <input id="hidPlanXml" runat="server" type="hidden" />
    <table id="Table12" width="98%" align="center" runat="server">
        <tr>
            <td align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            ������Ϣ����
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table2" style="width: 98%;" cellpadding="2" cellspacing="0" runat="server" class="listContent">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                ִ������<font color="#ff6666">*</font>
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddltFlow" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                �ƻ�����<font color="#ff6666">*</font>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPlanName' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td class='listTitleRight'>
                �ƻ���Ч��<font color="#ff6666">*</font>
            </td>
            <td class='list' style="display: none;">
                <asp:DropDownList ID="ddltPlanState" runat="server" Width="152px">
                    <asp:ListItem Value="0">��Ч</asp:ListItem>
                    <asp:ListItem Value="1" Selected="True">��Ч</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                �ƻ�ִ����<font color="#ff6666">*</font>
            </td>
            <td class='list'>
                <uc1:UserPicker ID="UserPicker1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                �ƻ�����
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPlanDesc' runat='server' Rows="4" TextMode="MultiLine" Width="85%"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td class='list' colspan="2" align="center">
                <asp:Button ID="btnSetTime" runat="server" Text="�ƻ�ʱ������" CausesValidation="False"
                    OnClientClick="SelectTimeSet();" SkinID="btnClass3"></asp:Button>
            </td>
        </tr>
    </table>
    <table id="Table13" width="98%" align="center" runat="server">
        <tr>
            <td align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            �ƻ�ʱ������
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table3" style="width: 98%;" cellpadding="2" cellspacing="0" runat="server" class="listContent">
        <tr>
            <td class='listTitleRight' style="width: 12%">
                �ƻ����:
            </td>
            <td class="list">
                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                    OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                    <asp:ListItem Value="10">��������</asp:ListItem>
                    <asp:ListItem Value="20" Selected="True">ÿ��ִ��</asp:ListItem>
                    <asp:ListItem Value="30">ÿ��ִ��</asp:ListItem>
                    <asp:ListItem Value="40">ÿ��ִ��</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr runat="server" id="trPlanTime">
            <td align="right" class='listTitleRight'>
                ָ��ִ��ʱ��:
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlHours" runat="server" Width="48px">
                </asp:DropDownList>
                ʱ<asp:DropDownList ID="ddlMinutes" runat="server" Width="48px">
                </asp:DropDownList>
                ��
            </td>
        </tr>
        <tr runat="server" id="trPlanDateMonth">
            <td align="right" class='listTitleRight'>
                ָ������(ÿ��ִ��):
            </td>
            <td class="list">
                ��<asp:DropDownList ID="ddlDays" runat="server" Width="48px">
                </asp:DropDownList>
                ��
            </td>
        </tr>
        <tr runat="server" id="trPlanDateWeek">
            <td align="right" class='listTitleRight'>
                ָ������(ÿ��ִ��):
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlWeeks" runat="server" Width="64px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" id="trTime">
            <td align="right" class='listTitleRight'>
                ����ʱ����:
            </td>
            <td class="list">
                <asp:TextBox ID="txtInterval" runat="server" MaxLength="9" Style="ime-mode: Disabled"
                    onblur="CheckIsnum(this,'����ʱ��������Ϊ��ֵ��');" onkeydown="NumberInput('');"></asp:TextBox>Сʱ
            </td>
        </tr>
        <tr runat="server" id="trBetween">
            <td align="right" class='listTitleRight'>
                ����ʱ�䷶Χ:
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlHoursBeg" runat="server" Width="48px">
                </asp:DropDownList>
                ʱ<asp:DropDownList ID="ddlMinutesBeg" runat="server" Width="48px">
                </asp:DropDownList>
                ��&nbsp; ---- &nbsp;
                <asp:DropDownList ID="ddlHoursEnd" runat="server" Width="48px">
                </asp:DropDownList>
                ʱ<asp:DropDownList ID="ddlMinutesEnd" runat="server" Width="48px">
                </asp:DropDownList>
                ��
            </td>
        </tr>
        <tr>
            <td align="right" class='listTitleRight'>
                ������������:
            </td>
            <td class="list">
                <asp:CheckBoxList ID="cblWeeks" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">����</asp:ListItem>
                    <asp:ListItem Selected="True" Value="1">��һ</asp:ListItem>
                    <asp:ListItem Selected="True" Value="2">�ܶ�</asp:ListItem>
                    <asp:ListItem Selected="True" Value="3">����</asp:ListItem>
                    <asp:ListItem Selected="True" Value="4">����</asp:ListItem>
                    <asp:ListItem Selected="True" Value="5">����</asp:ListItem>
                    <asp:ListItem Value="6">����</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr runat="server" id="trWeek">
            <td>
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />
</asp:Content>
