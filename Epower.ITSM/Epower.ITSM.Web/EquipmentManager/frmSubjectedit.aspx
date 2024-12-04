<%@ Page Language="c#" Inherits="Epower.ITSM.Web.EquipmentManager.frmSubjectedit"
    ValidateRequest="false" EnableViewState="true" CodeBehind="frmSubjectedit.aspx.cs"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrSetUserOtherRight.ascx" TagName="ctrSetUserOtherRight"
    TagPrefix="uc4" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>资产类别管理</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../App_Themes/NewOldMainPage/css.css" type="text/css" />

    <script language="javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Base.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Common.js"></script>
    <script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>

    <!--#include file="~/Js/tableSort.js" -->
</head>

<script language="javascript">
        function changeSelectImage() {   
                        $("#<%=Image1.ClientID %>")[0].src=$("#<%=hidImage.ClientID %>").val();  
                        return false;
                    }                   
		function SelectPCatalog()
			{
				if(document.all.hidCatalogID.value == 1)
				{
					alert("已经是最顶层分类!");
					return;
				}
				var newDateObj = new Date();
				var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString() + newDateObj.getMinutes().toString() + newDateObj.getMilliseconds().toString();
			    var url="frmpopSubject.aspx?CurrSubjectID=" + document.all.hidCatalogID.value + "&paramvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
			    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=100");
			}
			
			function SelectImage()
			{
			    //=========zxl===
                window.open("frmpopImage.aspx","",'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=500,height=400px,left=150,top=50'); 
			}
			
			function rdoSchema_onchange(obj)
			{
			    //更改继承关系后,进入配置编辑状态时 初始状态  因为只有 选中 继承关系才会有这个事件,因此是正确的,因为继承情况下不存在编辑
			    document.getElementById('<%=hidModified.ClientID %>').value = "true";
			}
			
			function SetCatalogSchema()
			{
			    var now = new Date().getTime();
			    var sPara = null;
			    var hasChangeParent = document.getElementById('<%=hidModified.ClientID %>').value;
			    

                //还原未更改状态
                document.getElementById('<%=hidModified.ClientID %>').value = "false";
			    
			    //防止showModalDialog 缓存 加了一个时间参数
			    var	value=window.showModalDialog("frmEquSchemaMain.aspx?ChangeParent=" + hasChangeParent + "&PSubjectID=" + document.all.hidPCatalogID.value + 
			                            "&time"  + now + 
			                           "&CurrSubjectID=" + document.all.hidCatalogID.value + 
			                           "&jcFather=" + document.getElementById("rdoSchema_0").checked 
			                            ,null,"dialogHeight:560px; dialogWidth:780px;");
				if(value != null)
				{
                    document.getElementById('<%= hidSchemaXml.ClientID %>').value = value;
                   // document.getElementById('<%=hidModified.ClientID %>').value = "true";
                    //alert( document.getElementById('<%= hidSchemaXml.ClientID %>').value);
				}
			}
			
			function delete_confirm()
			{
				if (event.srcElement.value =="删除" )
					event.returnValue =confirm("确认要删除吗?");
			}
		
		function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              //var className;
              //var objectFullName;
              var tableCtrl;
              //objectFullName = <%=tr2.ClientID%>.id;
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
        
    function CheckDefaultControlStatus(obj)
	{
	    //alert(obj.id);
	    
		var otxtobj = document.getElementById(obj.id.replace("ddlTypeName","PantxtDefault"));
		var ochkobj = document.getElementById(obj.id.replace("ddlTypeName","PanDefault"));
		var otxtMobj = document.getElementById(obj.id.replace("ddlTypeName","PantxtMDefault"));
		var otxtDropDownList = document.getElementById(obj.id.replace("ddlTypeName","panDropDownList"));
		var otxtpanUser = document.getElementById(obj.id.replace("ddlTypeName","panUser"));
		var otxtpanDept = document.getElementById(obj.id.replace("ddlTypeName","panDept"));
		
		var otxtpanTime = document.getElementById(obj.id.replace("ddlTypeName","PanTime"));
		var otxtpanNumber = document.getElementById(obj.id.replace("ddlTypeName","PanNumber"));
		
		
		
		if(obj.value == "基础信息")
		{
		    otxtobj.style.display="";
		    ochkobj.style.display="none";
		    otxtMobj.style.display="none";
		    otxtDropDownList.style.display="none";
		    otxtpanDept.style.display="none";
		    otxtpanUser.style.display="none";
		    otxtpanTime.style.display="none";
		    otxtpanNumber.style.display="none";
		}
		else if(obj.valueOf == "备注信息")
		{
	
		    otxtobj.style.display="none";
		    ochkobj.style.display="none";
		    otxtMobj.style.display="";
		    otxtDropDownList.style.display="none";
		    otxtpanDept.style.display="none";
		    otxtpanUser.style.display="none";
		    otxtpanTime.style.display="none";
		    otxtpanNumber.style.display="none";
		}
		else if(obj.value=="下拉选择")
		{
		      otxtobj.style.display="none";
		    ochkobj.style.display="none";
		    otxtMobj.style.display="none";
		    otxtpanDept.style.display="none";
		    otxtpanUser.style.display="none";
		    otxtDropDownList.style.display="";
		    otxtpanTime.style.display="none";
		    otxtpanNumber.style.display="none";
		}
		else if(obj.value=="部门信息")
		{
		      otxtobj.style.display="none";
		    ochkobj.style.display="none";
		    otxtMobj.style.display="none";
		    otxtpanDept.style.display="";
		    otxtpanUser.style.display="none";
		    otxtDropDownList.style.display="none";
		    otxtpanTime.style.display="none";
		    otxtpanNumber.style.display="none";
		}
		else if(obj.value=="用户信息")
		{
		      otxtobj.style.display="none";
		    ochkobj.style.display="none";
		    otxtMobj.style.display="none";
		    otxtpanDept.style.display="none";
		    otxtpanUser.style.display="";
		    otxtDropDownList.style.display="none";
		    otxtpanTime.style.display="none";
		    otxtpanNumber.style.display="none";
		}
		else if(obj.valueOf=="日期类型")
		{
		    otxtobj.style.display="none";
		    ochkobj.style.display="none";
		    otxtMobj.style.display="none";
		    otxtpanDept.style.display="none";
		    otxtpanUser.style.display="none";
		    otxtDropDownList.style.display="none";
		    otxtpanTime.style.display="";
		    otxtpanNumber.style.display="none";
		}		
		else if(obj.valueOf=="数值类型")
		{
		    otxtobj.style.display="none";
		    ochkobj.style.display="none";
		    otxtMobj.style.display="none";
		    otxtpanDept.style.display="none";
		    otxtpanUser.style.display="none";
		    otxtDropDownList.style.display="none";
		    otxtpanTime.style.display="none";
		    otxtpanNumber.style.display="";
		}
		else
		{
		    otxtobj.style.display="none";
		    ochkobj.style.display="";
		    otxtMobj.style.display="none";
		    otxtDropDownList.style.display="none";
		    otxtpanDept.style.display="none";
		    otxtpanUser.style.display="none";
		    otxtpanTime.style.display="none";
		    otxtpanNumber.style.display="none";
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
	    
	    var subjectedId=document.getElementById("hidCatalogID").value;
	    var newDateObj = new Date();	    
	    var sparamvalue =  newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
	    window.open("../EquipmentManager/frmEqu_SchemaItemsMain.aspx?IsChecked=True&IsSelect='1'&subjectedId="+subjectedId+"&sparamvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>","","dialogWidth=600px; dialogHeight=480px;status=no; help=no;scroll=auto;resizable=no") ;
	}
	
	function AddNewItem()
	{
	    var newDateObj = new Date();
	    var sparamvalue =  newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
	    
	    window.open("../EquipmentManager/frmEqu_SchemaItemsEdit.aspx?IsNew=true&sparamvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>","","dialogWidth=600px; dialogHeight=480px;status=no; help=no;scroll=auto;resizable=no");

	}
	
	String.prototype.trim = function()  //去空格
			{
				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
//-->
</script>

<body>
    <form id="Form1" method="post" runat="server">
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <table style="width: 98%" align="center" class="listContent">
        <tr>
            <td colspan="2" class="list">
                <uc1:CtrTitle ID="CtrTitle" runat="server" Title="资产类别管理 "></uc1:CtrTitle>
            </td>
        </tr>
        <tr height="40">
            <td colspan="2" class="listTitle">
                <asp:Button ID="cmdAdd" runat="server" Text="新增" CssClass="btnClass" OnClick="cmdAdd_Click"
                    CausesValidation="false"></asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdSave" runat="server" Text="保存" CssClass="btnClass" OnClick="cmdSave_Click">
                </asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdDelete" runat="server" Text="删除" CssClass="btnClass" OnClick="cmdDelete_Click"
                    OnClientClick="delete_confirm();" CausesValidation="false"></asp:Button>&nbsp;
            </td>
        </tr>
    </table>
    <table id="Table11" width="98%" align="center" runat="server">
        <tr id="tr2" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            基本信息
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" class="listContent" align="center" runat="server" id="Table1">
        <tr>
            <td nowrap class="listTitleRight" width="12%">
                名称:
            </td>
            <td class="list">
                <asp:TextBox ID="txtSubjectName" runat="server" Width="273px" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSubjectName"
                    ErrorMessage="分类名称不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
            <td nowrap class="list" rowspan="6" align="center" valign="top">
                <asp:Button ID="btnSelectImage" runat="server" Text="修改分类图片" OnClientClick="SelectImage();"
                    SkinID="btnClass3" OnClick="btnSelectImage_Click" />
                <br />
                <br />
                <asp:Image ID="Image1" runat="server" />
                <input id="hidImage" runat="server" type="hidden" />
                <div style=" visibility:hidden;">
                <input type="button" id="DispImage" onclick="javascript:changeSelectImage();return false;" />
                </div>
                
           
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight" width="12%">
                描述:
            </td>
            <td class="list">
                <asp:TextBox ID="txtDesc" runat="server" Width="273px" TextMode="MultiLine" onblur="MaxLength(this,50,'描述长度超出限定长度:');"
                    Rows="3"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight" width="12%">
                上级:
            </td>
            <td class="list">
                <asp:TextBox ID="txtPCatalogName" runat="server" Width="258" ReadOnly="True"></asp:TextBox><input
                    id="hidPCatalogID" style="width: 56px" type="hidden" runat="server" name="hidPCatalogID"><input
                        id="cmdPopParentCatalog" onclick="SelectPCatalog()" type="button" value="..."
                        class="btnClass2">
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" nowrap="nowrap" width="12%">
                配置:
            </td>
            <td class="list">
                <asp:RadioButtonList ID="rdoSchema" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                    OnSelectedIndexChanged="rdoSchema_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="0">继承父配置模板</asp:ListItem>
                    <asp:ListItem Value="1">独立配置模板</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight" width="12%">
                <font face="宋体">排序:</font>
            </td>
            <td nowrap class="list">
                <font face="宋体">
                    <asp:TextBox ID="txtSortID" runat="server" Width="258px" Style="ime-mode: Disabled"
                        onblur="CheckIsnum(this,'排序必须为数值！');" onkeydown="NumberInput('');" MaxLength="9">-1</asp:TextBox></font>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
            </td>
            <td nowrap class="list">
                <font face="宋体">*注:排序为-1时,由系统指定其排序,有效值范围(-1 ~ 99999)</font>
            </td>
        </tr>
        <tr id="trRight" runat="server">
            <td class="listTitleRight" width="12%" style="height: 25px">
                访问权限
            </td>
            <td class="list" align="left" style="height: 25px" colspan="3">
                <uc4:ctrSetUserOtherRight ID="ctrSetUserOtherRight2" runat="server" OperateType="30"
                    OperateID="0" />
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
       <%--     zxl 这里把它隐藏了--%>
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
                    OnItemDataBound="dgPro_ProvideManage_ItemDataBound" OnItemCreated="dgPro_ProvideManage_ItemCreated">
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
                                    <uc2:ctrFlowCataDropList ID="ctrFlowCataDropDefault" runat="server" RootID="0" />
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
                                    runat="server" Height="24px" Width="150px">
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
                        <asp:TemplateColumn Visible="False" HeaderText="删除">
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
    <input id="hidTempID" runat="server" type="hidden" />
    <input id="hidTable" value="" runat="server" type="hidden" />
    <input id="hidOrgID" type="hidden" runat="server" name="hidOrgID">
    <input id="hidCatalogID" type="hidden" runat="server" name="hidCatalogID">
    <input id="hidSchemaXml" type="hidden" runat="server" name="hidSchemaXml" />
    <input id="hidModified" value="false" type="hidden" runat="server" name="hidModified" />

    <script language="javascript">	
    var temp = document.all.<%=hidTable.ClientID%>.value;
    if(temp!="")
    {
        var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
        var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
        var arr=temp.split(",");
        for(i=1;i<arr.length;i++)
        {   
            var tableid=arr[i];
            var tableCtrl = document.all.item(tableid);
            tableCtrl.style.display ="none";
            var ImgID = tableid.replace("Table","Img");
            var imgCtrl = document.all.item(ImgID)
            imgCtrl.src = ImgPlusScr ;	
        }
    }
    </script>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    </form>
</body>
</html>
