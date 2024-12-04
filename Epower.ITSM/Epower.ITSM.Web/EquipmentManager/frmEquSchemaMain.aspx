<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" CodeBehind="frmEquSchemaMain.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEquSchemaMain" Title="配置项设置" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script language="javascript">
<!--           
	function CheckDefaultControlStatus(obj)
	{
	    //alert(obj.id);
	    
		var otxtobj = document.getElementById(obj.id.replace("ddlTypeName","PantxtDefault"));
		var ochkobj = document.getElementById(obj.id.replace("ddlTypeName","PanDefault"));
		
		if(obj.value == "基础信息")
		{
		    otxtobj.style.display="";
		    ochkobj.style.display="none";
		}
		else
		{
	
		    otxtobj.style.display="none";
		    ochkobj.style.display="";
		}
		
	}
	
	function CheckDoubleID(obj,name)
	{
	     var objectFullName = obj.id;
         var className = objectFullName.substring(0,objectFullName.indexOf(name)-1);
         var tmepName = className.substr(className.indexOf("dgSchema"),className.length-className.indexOf("dgSchema"));
         var i = tmepName.substr(tmepName.indexOf("ctl")+3,tmepName.length-tmepName.indexOf("ctl"));
         var curIDValue = obj.value.trim();
         var tmpobj;
         var tmpIDValue;

         var j=2;   
         while(true)
         {
                   if(j<10)
                   {  
				        tmpobj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_" + name);
				    }
				    else
				    {
				        tmpobj = document.all.item(className.substr(0,className.length-i.length)+j+"_" + name);
				    }
					if (tmpobj !=null)
					{
						tmpIDValue = tmpobj.value.trim();
						if(tmpobj.id != obj.id  && tmpIDValue == curIDValue)
				        {
				            alert("配置项ID 重复,请重新输入");
				            obj.value = '';
				            obj.focus();
				            break;
				        }
						
					}
					else
					{
						tmpIDValue = "";
						break;   //找不到控件时退出去
					}
				
				j++;
				
					

		}			
	}
	
	function doAddNewItem()
	{
	    var newDateObj = new Date();
	    var sparamvalue =  newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
	    var	value=window.showModalDialog("../EquipmentManager/frmEqu_SchemaItemsMain.aspx?IsSelect='1'&sparamvalue=" + sparamvalue,window,"dialogWidth=600px; dialogHeight=480px;status=no; help=no;scroll=auto;resizable=no") ;
	    var objTemp = document.getElementById('<%=hidTempID.ClientID %>');
	    var objBtn = document.getElementById('<%=btnAddNewItem.ClientID %>');
	    
	    if(value != null)
	    {
		    objTemp.value = value;  
           
		   
	    }
	    else
	    {
	        objTemp.value = "0";   
	    }
	    
	    if(objTemp.value != "0" && objTemp.value != "")
	    {
	        objBtn.click();
	    }
	    
	}
	
	
	
	String.prototype.trim = function()  //去空格

			{
				// 用正则表达式将前后空格

				// 用空字符串替代。

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
//-->
</script>

<table cellpadding='1' cellspacing='2' width='100%' border='0' class="listContent">
<tr>
	<td class='listTitle'  align='right' style='width:15%;'>
        配置标题&nbsp;</td>		
<td class='list'>
	<asp:TextBox ID='txtName' runat='server'></asp:TextBox>			
</td>	
<td class='list' align=right >
    <input id="cbtnAdd" class="btnClass" onclick="doAddNewItem();" runat=server type="button" value="添加配置项" />
    <input id="hidTempID" runat=server type="hidden" />
    &nbsp;<asp:Button ID="btnAddNewItem" runat="server" Text="填加配置项" CssClass="btnClass" OnClick="btnAddNewItem_Click" Width="0px" />
    <asp:Button ID="cmdOK" runat="server" CssClass="btnClass" OnClick="cmdOK_Click" Text="确定" />&nbsp;
</td>		
</tr>
</table>
<br />
<table cellpadding="0" cellspacing="0" width="100%" border="0" >
<tr>
    <td align="center"  class="listContent">
        <asp:datagrid id="dgSchema" runat="server" Width="100%" cellpadding="1" cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgPro_ProvideManage_ItemCommand" OnItemDataBound="dgPro_ProvideManage_ItemDataBound" OnItemCreated="dgPro_ProvideManage_ItemCreated">
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>

						<Columns>
                            <asp:TemplateColumn HeaderText="类别">
                                <ItemTemplate>
                                    &nbsp;<asp:DropDownList ID="ddlTypeName"  runat="server" onchange="CheckDefaultControlStatus(this);" Width="95%" SelectedValue='<%# DataBinder.Eval(Container, "DataItem.TypeName") %>' Enabled="False">
                                        <asp:ListItem>基础信息</asp:ListItem>
                                        <asp:ListItem>关联配置</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle Width="15%" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ID">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtID" Text='<%# DataBinder.Eval(Container, "DataItem.ID")%>' onblur="CheckDoubleID(this,'txtID');" Width="85%"  runat="server" Enabled="False"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="15%" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="配置项名称">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCHName" Text='<%# DataBinder.Eval(Container, "DataItem.CHName")%>' Width="90%"  runat="server" Enabled="False"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="25%" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="初值">
                                <ItemTemplate>
                                    <asp:Panel ID="PanDefault" style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"0")%>' runat="server" Height="16px" Width="125px">
                                    <asp:CheckBox ID="chkDefault"  Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'  runat="server" /></asp:Panel>
                                    <asp:Panel ID="PantxtDefault" style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"1")%>' runat="server" Height="24px" Width="95%">
                                    <asp:TextBox ID="txtDefault"  Text='<%# DataBinder.Eval(Container, "DataItem.Default")%>' Width="100%" runat="server"></asp:TextBox></asp:Panel>
                                </ItemTemplate>
                                <HeaderStyle Width="20%" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="组">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtGroup" Text='<%# DataBinder.Eval(Container, "DataItem.Group")%>' Width="95%" runat="server"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="15%" />
                            </asp:TemplateColumn>
							<asp:TemplateColumn>
						        <ItemTemplate>
							        <asp:Button ID="lnkdelete" CssClass="btnClass" runat="server" Text="删除" CommandName="delete" />
						        </ItemTemplate>
						        <HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" />
					        </asp:TemplateColumn>
						</Columns>
						<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </td>
</tr>
<tr>
	<td colspan="4" align="right" class='listTitle'>
        <input id="hidSavedSchema" type="hidden" runat="server" NAME="hidSavedSchema" /></td>
</tr>
</table>
</asp:Content>
