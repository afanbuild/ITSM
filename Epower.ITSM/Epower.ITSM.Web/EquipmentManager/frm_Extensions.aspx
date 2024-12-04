<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_Extensions.aspx.cs"  ValidateRequest="false"
    Inherits="Epower.ITSM.Web.EquipmentManager.frm_Extensions"
    Title="扩展项管理" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrSetUserOtherRight.ascx" TagName="ctrSetUserOtherRight"
    TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc5" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
    function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              //var className;
              //var objectFullName;
              var tableCtrl;
             
              //className = objectFullName.substring(0,objectFullName.indexOf("tr2")-1);
              tableCtrl = document.all.item(TableID);
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
        function AddNewItem()
	    {
	        var newDateObj = new Date();
	        var sparamvalue =  newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
    	    
	        window.open("../EquipmentManager/frm_ExtensionsEdit.aspx?IsNew=true&sparamvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>","","dialogWidth=600px; dialogHeight=480px;status=no; help=no;scroll=auto;resizable=no");
    	    
	    }
	    function doAddNewItem()
	    {
    	   
//	        var subjectedId=document.getElementById("hidID").value;
	        var newDateObj = new Date();	    
	        var sparamvalue =  newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
	        window.open("../EquipmentManager/frm_ExtensionsMain.aspx?IsChecked=True&IsSelect='1'"+"&sparamvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>","","dialogWidth=600px; dialogHeight=480px;status=no; help=no;scroll=auto;resizable=no") ;
    	    
	    }
</script>
 
 
 <input id="hidTempID" runat="server" type="hidden" />
<input id="hidExtendsID" runat="server" type="hidden" />
 

  
 <input id="hidTable" value="" runat="server" type="hidden" />
 
 <input id="hidSchemaXml" type="hidden" runat="server" name="hidSchemaXml" />
 
 <input id="hidModified" value="false" type="hidden" runat="server" name="hidModified" />
 <input id="hidPCatalogID" style="width: 56px" type="hidden" runat="server" name="hidPCatalogID"/>
 
 <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />



<table style="width: 98%" align="center" class="listContent">
       
        <tr height="40">
            <td colspan="2" class="listTitleRight">
                <asp:Button ID="cmdQuery" runat="server" Text="查询" CssClass="btnClass" OnClick="cmdQuery_Click">
                </asp:Button>&nbsp;
                <asp:Button ID="cmdSave" runat="server" Text="保存" CssClass="btnClass" OnClick="cmdSave_Click">
                </asp:Button>&nbsp;&nbsp;
               
            </td>
        </tr>
</table>

 <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent GridTable'>
        <tr>
            <td class='listTitleRight' style="width: 8%">
                配置项名称
            </td>
            <td class='list' style="width: 15%">
                <asp:TextBox ID='txtCHName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' style="width: 8%">
                所属分组
            </td>
            <td class='list' style="width: 35%">
                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" onselectedindexchanged="ddlType_SelectedIndexChanged1" 
                    >
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="1026">事件单</asp:ListItem>
                    <asp:ListItem Value="210">问题单</asp:ListItem>
                    <asp:ListItem Value="420">变更单</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    
    </table>
    
 <table id="Table12" width="98%" align="center" runat="server">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />配置项设置
                        </td>
                    </tr>
                </table>
            </td>
            <td class="listTitleNew" align="right" style="width: 160px">
     
                <asp:Button ID="btnAddNewItem" runat="server" Text="添加配置项" style="display:none;" CssClass="btnClass" OnClick="btnAddNewItem_Click"
                     CausesValidation="false" />
                <input id="cbtnAdd" class="btnClass" onclick="doAddNewItem();" runat="server" type="button"
                    value="批量添加" causesvalidation="false" />
                <input id="cbtnNew" class="btnClass" onclick="AddNewItem();" runat="server" type="button"
                    value="新配置项" causesvalidation="false" />
            </td>
        </tr>
    </table>
   
    
 <table style="width: 98%" align="center" runat="server" id="Table2">
        <tr>
            <td>
                <asp:DataGrid ID="dgSchema" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgPro_ProvideManage_ItemCommand"
                    OnItemDataBound="dgPro_ProvideManage_ItemDataBound" OnItemCreated="dgPro_ProvideManage_ItemCreated" >
                   
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtID" Text='<%# DataBinder.Eval(Container, "DataItem.ID")%>' onblur="CheckDoubleID(this,'txtID');"
                                    Width="85%" runat="server" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="类别">
                            <ItemTemplate>
                                &nbsp;<asp:DropDownList ID="ddlTypeName" runat="server" onchange="CheckDefaultControlStatus(this);"
                                    Width="95%" SelectedValue='<%# DataBinder.Eval(Container, "DataItem.TypeName") %>'
                                    Enabled="False">
                                    <asp:ListItem>基础信息</asp:ListItem>
                                    <asp:ListItem>关联配置</asp:ListItem>
                                    <asp:ListItem>备注信息</asp:ListItem>
                                    <asp:ListItem>下拉选择</asp:ListItem>
                                    <asp:ListItem>部门信息</asp:ListItem>
                                    <asp:ListItem>用户信息</asp:ListItem>
                                    <asp:ListItem>日期类型</asp:ListItem>
                                    <asp:ListItem>数值类型</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="配置项名称">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHName" Text='<%# DataBinder.Eval(Container, "DataItem.CHName")%>'
                                    Width="90%" runat="server" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="25%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="初值">
                            <ItemTemplate>
                                <asp:Panel ID="PanDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"0")%>'
                                    runat="server" Height="16px" Width="100px">
                                    <asp:CheckBox ID="chkDefault" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" /></asp:Panel>
                                <asp:Panel ID="PantxtDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"1")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:TextBox ID="txtDefault" Text='<%# DataBinder.Eval(Container, "DataItem.Default")%>'
                                        Width="100%" runat="server"></asp:TextBox></asp:Panel>
                                <asp:Panel ID="PantxtMDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"2")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:TextBox ID="txtMDefault" Text='<%# DataBinder.Eval(Container, "DataItem.Default")%>'
                                        Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox></asp:Panel>
                                <asp:Panel ID="panDropDownList" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"3")%>'
                                    runat="server" Height="24px" Width="100px">
                                    
                                    <uc1:ctrFlowCataDropList ID="ctrFlowCataDropDefault" runat="server" RootID="1" />
                                </asp:Panel>
                                <asp:Panel ID="panDept" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"4")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckDept" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" />所在部门</asp:Panel>
                                <asp:Panel ID="panUser" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"5")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckUser" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" />登录人</asp:Panel>
                                <asp:Panel ID="PanTime" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"6")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckTime" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" />当前日期
                                    <asp:CheckBox ID="CheckIsTime" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.isChack"))%>'
                                        runat="server" />是否时间</asp:Panel>
                                <asp:Panel ID="PanNumber" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"7")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <uc3:CtrFlowNumeric ID="TextNumber" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.Default")%>' />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="必填">
                            <HeaderStyle Width="65px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsMust" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.IsMust"))%>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="查询">
                            <HeaderStyle Width="65px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsSelect" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.IsSelect"))%>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="组">
                            <ItemTemplate>
                                <asp:TextBox ID="txtGroup" Text='<%# DataBinder.Eval(Container, "DataItem.Group")%>'
                                    Width="95%" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="排序">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtOrderBy" Text='<%# DataBinder.Eval(Container, "DataItem.OrderBy")%>'
                                    Width="95%" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="所属分组">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtGroupName" Text='<%#DataBinder.Eval(Container,"DataItem.GroupName") %>' 
                                 Width="95%" runat="server" Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="TxtGroupID" Visible="false"  runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.GroupID") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="删除">
                            <ItemTemplate>
                                <asp:Button ID="lnkdelete"  SkinID="btnClass1" runat="server" Text="删除" CommandName="delete"
                                    CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Default" HeaderText="初值" Visible="false"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
 

 
</asp:Content>
