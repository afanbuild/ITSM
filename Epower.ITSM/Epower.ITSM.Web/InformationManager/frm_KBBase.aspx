<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frm_KBBase.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frm_KBBase"
    Title="知识登单" %>

<%@ Register Src="../Controls/ctrKBCataDropList.ascx" TagName="ctrKBCataDropList"
    TagPrefix="uc1" %>
<%@ Register src="../Controls/UEditor.ascx" tagname="UEditor" tagprefix="uc2" %>
<%@ Register src="../Controls/CtrFlowRemark.ascx" tagname="CtrFlowRemark" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
function TransferValue()
{
//拼凑待办事项标题
   if (typeof(parent.header.flowInfo.Subject)!="undefined" && typeof(document.all.<%=txtTitle.ClientID%>)!="undefined" )
	     parent.header.flowInfo.Subject.value = "[" + document.all.<%=txtTitle.ClientID%>.value + "]知识单";
}

function DoUserValidate(lngActionID,strActionName)
{
    //拼标题

    TransferValue();
    return CheckCustAndType();
}	

//检查是否必填

function CheckCustAndType()
{
    if (typeof(document.all.<%=txtTitle.ClientID%>)!="undefined" )
    {
        if (document.all.<%=txtTitle.ClientID%>.value.trim()=="")//主题

        {
            document.all.<%=txtTitle.ClientID%>.focus();
	        alert("主题不能为空！");
	        return false;
        }
    }
    
    if (typeof(document.all.<%=CtrKBType.ClientID%>)!="undefined" )
    {
        if(document.all.<%=CtrKBType.ClientID%>.value.trim()=="1")
	    {
	         alert("知识类别中根目不能新建，请选择根目录的下级目录！");
	         return false;
	    }
	}       
    return true;
}

//选择资产目录
function SelectListName(obj)
{
        var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskCateListSel.aspx?random=" + GetRandom(),"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
        if(value != null)
        {
            if(value.length>1)
            {			                
                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = value[1];   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListName")).value = value[1];   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = value[0];  //资产目录ID
            }
            else
            {			                
                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";  //资产目录ID
            }
        }
        else
        {			                
                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";  //资产目录ID
        }
}
//设备
function SelectEqu(obj) 
{
    var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value.trim();
        			        
    var EquName = document.all.<%=txtEqu.ClientID%>.value.trim();
	var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&randomid="+GetRandom()+"&FlowID="+ '<%=FlowID%>' + "&EquName=" + escape(EquName) +"&EquipmentCatalogID="+EquipmentCatalogID,window,"dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
	if(value != null)
	{
		var json = value;
	    var record=json.record;
	    
		for(var i=0; i < record.length; i++)
		{					
            document.getElementById(obj.id.replace("cmdEqu","txtEqu")).value = record[i].name;   //设备名称
            document.getElementById(obj.id.replace("cmdEqu","hidEquName")).value = record[i].name;   //设备名称
            document.getElementById(obj.id.replace("cmdEqu","hidEqu") ).value = record[i].id;  //设备ID
		}
	}
	else
	{
	    document.getElementById(obj.id.replace("cmdEqu","txtEqu")).value = "";   //设备名称
        document.getElementById(obj.id.replace("cmdEqu","hidEquName")).value = "";   //设备名称
        document.getElementById(obj.id.replace("cmdEqu","hidEqu") ).value = 0;  //设备ID
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
    var flowid="<%=FlowID%>";
    var AppID="<%=AppID%>";
    var FlowMoldelID="<%=FlowModelID%>";
    window.open("../Print/printRule.aspx?FlowId="+flowid+"&AppID="+AppID+"&FlowMoldelID="+FlowMoldelID,'','toolbar=no,menubar=no,status=yes,resizable=yes,tilebar=yes,scrollbars=yes');
    return false;
}

    </script>

    <table class="listContent" width="100%" align="center" border="0" cellSpacing="1" cellPadding="1">
        <tr>
            <td style="width: 12%" align="right" class="listTitle">
                <asp:Literal ID="info_Title" runat="server" Text="主题"></asp:Literal>
            </td>
            <td style="width: 88%" align="left" class="list" colspan="3">
                <asp:Label ID="labTitle" runat="server" Visible="False"></asp:Label><asp:TextBox
                    ID="txtTitle" runat="server" MaxLength="50" Width="319px"></asp:TextBox><asp:Label
                        ID="rTitle" runat="server" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <asp:Literal ID="info_PKey" runat="server" Text="关键字"></asp:Literal>
            </td>
            <td class="list" align="left" colspan="3">
                <asp:TextBox ID="txtPKey" runat="server" Width="319px" MaxLength="50"></asp:TextBox><asp:Label
                    ID="labPKey" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align="right">
                <asp:Literal ID="info_Tags" runat="server" Text="摘要"></asp:Literal>
            </td>
            <td class='list' align="left" colspan="3">
                <uc3:CtrFlowRemark ID="txtTags" Width="80%" MaxLength="200" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <asp:Literal ID="info_TypeName" runat="server" Text="知识类别"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <uc1:ctrKBCataDropList ID="CtrKBType" runat="server" Width="70%" RootID="1" />
            </td>
        </tr>
        <tr runat="server" id="trEqu" style="display:none">
            <td style="width: 12%; display:none;" align="right" class="listTitle">
                <asp:Literal ID="Literal1" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list" style="width: 35%; display:none;">
                <asp:Label ID="lblListName" runat="server" Visible="false" Width="120px"></asp:Label>
                <asp:TextBox ID="txtListName" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                <input id="cmdListName" onclick="SelectListName(this)" type="button" value="..."
                    runat="server" name="cmdListName" class="btnClass2" />
                <input id="hidListName" value="" type="hidden" runat="server" />
                <input id="hidListID" value="0" type="hidden" runat="server" />
            </td>
            <td style="width: 12%" align="right" class="listTitle">
                <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblEqu" runat="server" Visible="False" Width="120px"></asp:Label>
                <asp:TextBox ID="txtEqu" runat="server" Width="120px" MaxLength="80" onblur="GetEquID(this)"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
                <input id="hidEqu" type="hidden" runat="server" value="-1" />
                <input id="hidEquName" type="hidden" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <asp:Literal ID="info_Content" runat="server" Text="知识内容"></asp:Literal>
            </td>
            <td class="list ueditor_tbl" style="word-break: break-all" colspan="3">
                <asp:Label ID="lblContent" runat="server" Text="" Visible="false"></asp:Label>
                <uc2:UEditor ID="UEditor1" runat="server" UEditorFrameWidth="850" />
                <asp:Label ID="rWarning" runat="server" Style="margin-left: 7px;" Font-Bold="False"
        Font-Size="Small" ForeColor="Red">*</asp:Label>                                               
            </td>
        </tr>
        <tr>
            <td class="listTitle" align="right">
                <asp:Literal ID="info_IsInKB" runat="server" Text="同意入库"></asp:Literal>
            </td>
            <td class="list" align="left" colspan="3">
                <asp:CheckBox ID="chkIsInKB" runat="server" Checked="true" />
            </td>
        </tr>
    </table>
</asp:Content>
