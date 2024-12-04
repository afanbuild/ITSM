<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    ValidateRequest="false" CodeBehind="frmInf_InformationEdit.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmInf_InformationEdit"
    Title="资产编辑" %>

<%@ Register Src="../Controls/ctrKBCataDropList.ascx" TagName="ctrKBCataDropList"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/UEditor.ascx" tagname="UEditor" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" charset="utf-8" src="../ueditor2/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../ueditor2/ueditor.all.min.js"> </script>
    <!--建议手动加在语言，避免在ie下有时因为加载语言失败导致编辑器加载失败-->
    <!--这里加载的语言文件会覆盖你在配置项目里添加的语言类型，比如你在配置项目里配置的是英文，这里加载的中文，那最后就是中文-->
    <script type="text/javascript" charset="utf-8" src="../ueditor2/lang/zh-cn/zh-cn.js"></script>

    <script language="javascript" type="text/javascript">
	function SubmitValidate()          //保存前执行脚本

	{		
	    if(typeof(document.all.<%=txtTitle.ClientID%>) != "undefined")
	    {
	        if(document.all.<%=txtTitle.ClientID%>.value.trim()=="")
	        {
	            alert("主题不能为空！");
	            return false;
	        }
	        if(document.all.<%=CtrKBCata1.ClientID%>.value.trim()=="1")
	        {
	            alert("知识类别中根目不能新建，请选择根目录的下级目录！");
	            return false;
	        }
	    }
	    
        /*     
         * Date: 2013-08-05 14:56
         * summary: 取UEditor无格式的内容, 存入隐藏域 UEditorPlainContent 中.
         *                  
         * modified: sunshaozong@gmail.com     
         */	    
	    var plainContent = UE.getEditor(window.UEditorName).getContentTxt();
	    $('#<%=UEditorPlainContent.ClientID %>').val(plainContent);
	    
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
    if(EquipmentCatalogID=="0"||EquipmentCatalogID=="")
    {
          alert("请先选择资产目录!");
          return;
    }
        			        
    var EquName = document.all.<%=txtEqu.ClientID%>.value.trim();
	var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&randomid="+GetRandom()+"&EquName=" + escape(EquName) +"&EquipmentCatalogID="+EquipmentCatalogID,window,"dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
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
    </script>

    <table style="width: 98%" class="listContent Gridtable" cellpadding="2" cellspacing="0" border="0">
        <tr>
            <td class='listTitleRight' style='width: 12%'>
             <asp:Literal ID="info_Title" runat="server" Text="主题" ></asp:Literal>   
             
            </td>
            <td class='list' colspan="3">
                <asp:TextBox ID='txtTitle' runat='server' Width="416px" MaxLength="25"></asp:TextBox><font
                    color="#ff6666">*</font>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
              <asp:Literal ID="info_PKey" runat="server" Text="关键字" ></asp:Literal>     

            </td>
            <td class='list' colspan="3">
                <asp:TextBox ID='txtPKey' runat='server' Width="416px" MaxLength="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
               <asp:Literal ID="info_Tags" runat="server" Text="摘要" ></asp:Literal>      
            </td>
            <td class='list' colspan="3" style="width:800px;">
                <asp:TextBox ID='txtTags' runat='server' Width="100%" Rows="4" TextMode="MultiLine"
                    onblur="MaxLength(this,200,'摘要长度超出限定长度:');"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
             <asp:Literal ID="info_TypeName" runat="server" Text="知识类别" ></asp:Literal>          
            </td>
            <td class='list' colspan="3">
                <uc2:ctrKBCataDropList ID="CtrKBCata1" runat="server"></uc2:ctrKBCataDropList>
            </td>
        </tr>
        <tr style="display: none;">
            <td style="width: 12%" class="listTitleRight">
                <asp:Literal ID="Literal1" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="lblListName" runat="server" Visible="false" Width="120px"></asp:Label>
                <asp:TextBox ID="txtListName" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                <input id="cmdListName" onclick="SelectListName(this)" type="button" value="..."
                    runat="server" name="cmdListName" class="btnClass2" />
                <input id="hidListName" value="" type="hidden" runat="server" />
                <input id="hidListID" value="0" type="hidden" runat="server" />
            </td>
            <td style="width: 12%" class="listTitleRight">
                <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblEqu" runat="server" Visible="False" Width="120px"></asp:Label>
                <asp:TextBox ID="txtEqu" runat="server" Width="120px" MaxLength="80" onblur="GetEquID(this)"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
                <input id="hidEqu" type="hidden" runat="server" value="-1" />
                <input id="hidEquName" type="hidden" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitleRight">
                附件
            </td>
            <td class="list" colspan="3" style="width:800px;">
                <uc1:ctrattachment ID="Ctrattachment1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
               <asp:Literal ID="info_Content" runat="server" Text="知识内容"></asp:Literal>  
            </td>
            <td class='list' colspan="3">            
                <uc3:UEditor Visible="true" ID="UEditor1" runat="server" UEditorFrameWidth="800" />
                                
                <%--<script id="editor" visible="false" type="text/plain" style="width:1024px;height:500px;"></script>--%>
                <!--Begin: 2013-08-05 隐藏域 UEditorPlainContent 用于存储无格式的UEditor内容-->
                <asp:HiddenField ID="UEditorPlainContent" runat="server" />
                <!--End:-->
                
                <asp:Label ID="rWarning" runat="server" Style="margin-left: 7px;" Font-Bold="False"
        Font-Size="Small" ForeColor="Red">*</asp:Label>            
            </td>
        </tr>
    </table>
    
    <script type="text/javascript">
//    UE.getEditor('editor');
    </script>
</asp:Content>
