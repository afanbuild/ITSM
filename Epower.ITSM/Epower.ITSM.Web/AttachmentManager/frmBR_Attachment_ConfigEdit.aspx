<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmBR_Attachment_ConfigEdit.aspx.cs" Inherits="Epower.ITSM.Web.AttachmentManager.frmBR_Attachment_ConfigEdit"
    Title="必填附件配置信息编辑" %>
    
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript">
    //改变应用名称下拉框
    function ddlAppChange(ddl)
    {   var sAppID = ddl.options[ddl.selectedIndex].value;      //应用ID
        var ddlModel=document.all.<%=ddlFlowModel.ClientID %>;  //流程模型
        var pars = "randomid="+GetRandom()+"&act=GetFlowModelByAppID&AppID=" + sAppID;
        var surl = "<%=sApplicationUrl %>/Forms/Handler.ashx";
        $.ajax({
                    type: "get",
                    data:pars,
                    async:false,
                    url: surl,
                    success: function(data, textStatus){                                                   
                            var json= eval("("+data+")");
				            var record=json.record;
				            ddlModel.options.length=0;
				            var newOption=document.createElement("Option");
				            newOption.text="";
				            newOption.value="0";
				            ddlModel.add(newOption);
				            for(var i=0; i < record.length; i++)
			                {   
			                    newOption = document.createElement("Option");
                                newOption.text = record[i].flowname;
                                newOption.value = record[i].oflowmodelid;
                                ddlModel.options.add(newOption);
			                }                                                                    
		            }
                 });            
    }
    
    //显示与隐藏table
    function ShowTable(imgCtrl)
    {              
          var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
          var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
          var TableID = imgCtrl.id.replace("Img","Table");
          var className;
          var objectFullName;
          var tableCtrl;
          objectFullName = document.all.<%=ddlApp.ClientID%>.id;
          className = objectFullName.substring(0,objectFullName.indexOf("ddlApp")-1);
          tableCtrl = document.getElementById(TableID);
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

    <table style="width: 98%" class="listContent" align="center" runat="server">
        <tr>
            <td class="listTitleRight" style='width: 12%;'>
                应用名称
            </td>
            <td class='list' style='width: 35%;'>
                <asp:DropDownList ID="ddlApp" runat="server" Width="152px"  AutoPostBack="true" OnSelectedIndexChanged="ddlApp_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" style='width: 12%;'>
                流程名称
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddlFlowModel" runat="server" Width="152px" AutoPostBack="true" OnSelectedIndexChanged="ddlFlowModel_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="98%">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                               runat="server"  width="16" align="absbottom" />
                            配置信息
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" align="center" runat="server" cellpadding="1" id="Table1"
    cellspacing="0" border="0">
    <tr>
        <td>
            <asp:DataGrid ID="dgBR_Attachment_Config" runat="server" AutoGenerateColumns="False" Width="100%"
                CssClass="fixed-grid-border2" Style="border: 1px solid #A3C9E1;" OnItemDataBound="dgBR_Attachment_Config_ItemDataBound"
                OnItemCommand="dgBR_Attachment_Config_ItemCommand">
                <Columns>
                    <asp:BoundColumn DataField="ID" Visible="False"></asp:BoundColumn>                    
                    <asp:TemplateColumn HeaderText="环节名称">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlNode" runat="server" Width="130px">                               
                            </asp:DropDownList>
                            <asp:Label ID="rNodeWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="160px" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="比较符">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlOperate" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Operators") %>'
                                Width="80" EnableViewState="false">
                                <asp:ListItem Selected="True" Value="0">等于</asp:ListItem>                                
                                <asp:ListItem Value="1">以..开头</asp:ListItem>
                                <asp:ListItem Value="2">包含</asp:ListItem>                                
                            </asp:DropDownList>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="必填附件名称">
                        <ItemTemplate>
                            <uc1:CtrFlowFormText ID="CtrFlowAttachmentName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AttachmentName") %>'
                                Width="300px" />
                            <asp:Label ID="rAttachmentWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="True" HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="必填附件类型">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlAttachmentType" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "AttachmentType") %>'
                                Width="80" EnableViewState="false">
                                <asp:ListItem Value=""></asp:ListItem>
                                <asp:ListItem Value="doc">doc</asp:ListItem>                                
                                <asp:ListItem Value="xls">xls</asp:ListItem>
                                <asp:ListItem Value="xlsx">xlsx</asp:ListItem>
                                <asp:ListItem Value="rar">rar</asp:ListItem>
                                <asp:ListItem Value="jpg">jpg</asp:ListItem>
                                <asp:ListItem Value="png">png</asp:ListItem>
                                <asp:ListItem Value="gif">gif</asp:ListItem>
                                <asp:ListItem Value="bmp">bmp</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                    </asp:TemplateColumn>                   
                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <asp:Button ID="cmdOK" runat="server" SkinID="btnClass1" OnClick="cmdOK_Click" Text="添加"
                                CausesValidation="False" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False" OnClientClick="return confirm('确定删除吗？')"
                                SkinID="btnClass1"></asp:Button>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="44px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>                    
                </Columns>
                <HeaderStyle CssClass="listTitle" />
            </asp:DataGrid>
        </td>
    </tr>   
</table>
</asp:Content>
