<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="true"
    ValidateRequest="false" AutoEventWireup="true" CodeBehind="frmCst_ServiceLevelEdit.aspx.cs"
    Inherits="Epower.ITSM.Web.AppForms.frmCst_ServiceLevelEdit" Title="服务级别编辑" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<%@ Register src="../Controls/CtrFlowNumeric.ascx" tagname="CtrFlowNumeric" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">                        
.fixed-grid-border2 
{
	border-right: 1px #A3C9E1 solid;
}

.fixed-grid-border2 td {
	border-left: solid 1px #CEE3F2;
	border-right: 0px;
}

.fixed-grid-border2 tr {
	border-bottom: solid 1px #CEE3F2;
	border-top: solid 1px #CEE3F2;
}            
    </style>
    <script language="javascript">	
			function PopCondValue(obj)
			{
				reg=/cmdPop/g;
				var idtag=obj.id.replace(reg,"");
				//debugger;
				var v=document.all(idtag+"cboItems").value;
				var itemType=v.split(',')[1];
				var itemID = v.split(',')[0];
				switch(itemType)
				{
					case 'CATA' ://分类情况
				        
					   var url="frmServiceLevelCata.aspx?id=" + itemID+"&objID="+obj.id;
					   window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=220,height=160,left=400,top=300");					  
						break;
					
				}
				
			}
			
					//根据字段的改变更改比较符
		function ChangeFieldName(obj)
		{
		    reg=/cboItems/g;
			var idtag=obj.id.replace(reg,"");
			var ddlOperator = document.getElementById(obj.id.replace("cboItems","cboOperate"));			
			while(ddlOperator.options.length>0) {ddlOperator.remove(0);}//清空比较符


			
			var SelectField = obj.options[obj.selectedIndex].value;
			var FieldType=SelectField.split(',')[1];
			var newOption;
			//alert(FieldType);
			switch(FieldType)
			{
				case "CHAR":
					newOption = document.createElement("OPTION");					
					newOption.text  = "等于";
					newOption.value = "0";
					newOption.selected=true;
					ddlOperator.options.add(newOption);
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "不等于";
					newOption.value = "1";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "以..开头";   //大于等于
					newOption.value = "3";
					ddlOperator.options.add(newOption);	

					newOption = document.createElement("OPTION");					
					newOption.text  = "包含";
					newOption.value = "6";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "不包含";
					newOption.value = "7";
					ddlOperator.options.add(newOption);	
					
					document.all(idtag+"cmdPop").style.visibility="Hidden";
					
					document.all(idtag+"txtValue").disabled=false;
					
					break;
				case "CATA":
					newOption = document.createElement("OPTION");					
					newOption.text  = "等于";
					newOption.value = "0";
					newOption.selected=true;
					ddlOperator.options.add(newOption);
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "不等于";
					newOption.value = "1";
					ddlOperator.options.add(newOption);	
					
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "属于";
					newOption.value = "6";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "不属于";
					newOption.value = "7";
					ddlOperator.options.add(newOption);	
					
					document.all(idtag+"cmdPop").style.visibility="Visible";
					
					//改变以后 把相关分类值清空


					
					document.all(idtag+"txtValue").disabled=true;
					
					document.all(idtag+"txtValue").value="";
					document.all(idtag+"hidValue").value="";
					document.all(idtag+"hidTag").value="";
					
					break;
			}			
			ddlOperator.selectedIndex=0;
		}
		
		function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              var className;
              var objectFullName;
              var tableCtrl;
              objectFullName = <%=tr2.ClientID%>.id;
              className = objectFullName.substring(0,objectFullName.indexOf("tr2")-1);
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

    <table id="Table11" width="98%" align="center" runat="server" class="listNewContent ">
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
    <table style="width: 98%;border: 1px solid #A3C9E1;" class="listContent " runat="server" id="Table1" cellpadding="2"
        cellspacing="0">
        <tr>
            <td class='listTitleRight' style='width: 12%;border-right: 1px solid #A3C9E1;border-bottom: 1px solid #A3C9E1;'>
                级别名称
            </td>
            <td class='list' style='width: 35%'>
                <uc1:CtrFlowFormText ID="CtrFTLevelName" runat="server" MustInput="true" TextToolTip="级别名称" MaxLength="50" />
            </td>
            <td class='listTitleRight' style='width: 12%;border-left: 1px solid #A3C9E1;border-right: 1px solid #A3C9E1;border-bottom: 1px solid #A3C9E1;'>
                是否有效
            </td>
            <td class='list'>
                <asp:RadioButtonList ID="rdbtIsAvail" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0" Selected="True">有效</asp:ListItem>
                    <asp:ListItem Value="1">无效</asp:ListItem>
                </asp:RadioButtonList>                
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style="border-right: 1px solid #A3C9E1;border-bottom: 1px solid #A3C9E1;">
                级别定义
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID='txtDefinition' runat='server' Rows="3" MaxLength="200" TextMode="MultiLine"
                    Width="98%" onblur="MaxLength(this,200,'级别定义长度超出限定长度:');"></asp:TextBox>
                <asp:Label ID="labDefinition" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style="border-right: 1px solid #A3C9E1;border-bottom: 1px solid #A3C9E1;">
                服务包括
            </td>
            <td class='list'>
                <asp:TextBox ID='txtBaseLevel' runat='server' Rows="3" MaxLength="200" TextMode="MultiLine"
                    Width="95%" onblur="MaxLength(this,200,'服务包括长度超出限定长度:');"></asp:TextBox>
                <asp:Label ID="labBaseLevel" runat="server" Visible="False"></asp:Label>
            </td>
            <td class='listTitleRight' style="border-top: 1px solid #A3C9E1;border-left: 1px solid #A3C9E1;border-right: 1px solid #A3C9E1;border-bottom: 1px solid #A3C9E1;">
                服务不包括

            </td>
            <td class='list'>
                <asp:TextBox ID='txtNotInclude' runat='server' Rows="3" TextMode="MultiLine" Width="95%"
                    onblur="MaxLength(this,200,'服务不包括长度超出限定长度:');"></asp:TextBox>
                <asp:Label ID="labNotInclude" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style="border-right: 1px solid #A3C9E1;">
                服务有效性
            </td>
            <td class='list'>
                <asp:TextBox ID='txtAvailability' runat='server' Rows="3" TextMode="MultiLine" Width="95%"
                    onblur="MaxLength(this,200,'服务有效性长度超出限定长度:');"></asp:TextBox>
                <asp:Label ID="labAvailability" runat="server" Visible="False"></asp:Label>
            </td>
            <td class='listTitleRight' style="border-left: 1px solid #A3C9E1;border-right: 1px solid #A3C9E1;">
                费用说明
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCharge' runat='server' Rows="3" TextMode="MultiLine" Width="95%"
                    onblur="MaxLength(this,200,'费用说明长度超出限定长度:');"></asp:TextBox>
                <asp:Label ID="labCharge" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="Table12" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            规则设置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" align="center" runat="server" id="Table2" cellpadding="0"
        cellspacing="0" border="0">
        <tr>
            <td style="width: 100%">
                <asp:DataGrid ID="dgCondition" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="fixed-grid-border2" Style="border: 1px solid #A3C9E1;"
                    OnItemDataBound="dgCondition_ItemDataBound" OnItemCommand="dgCondition_ItemCommand">
                    <Columns>
                        <asp:BoundColumn DataField="id" Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="逻辑关系">
                            <ItemTemplate>
                                <asp:DropDownList ID="cboRelation" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Relation") %>'
                                    Width="60">
                                    <asp:ListItem Selected="True" Value="0">并且</asp:ListItem>
                                    <asp:ListItem Value="1">或者</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="组">
                            <ItemTemplate>
                                <uc2:CtrFlowNumeric ID="CtrFlowGroupValue" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "GroupValue") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="140px" Wrap="True" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="内容项">
                            <ItemTemplate>
                                <asp:DropDownList ID="cboItems" runat="server" onchange="ChangeFieldName(this);"
                                    Width="100">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="比较关系">
                            <ItemTemplate>
                                <asp:DropDownList ID="cboOperate" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Operate") %>'
                                    Width="80">
                                    <asp:ListItem Selected="True" Value="0">等于</asp:ListItem>
                                    <asp:ListItem Value="1">不等于</asp:ListItem>
                                    <asp:ListItem Value="2">大于</asp:ListItem>
                                    <asp:ListItem Value="3">大于等于</asp:ListItem>
                                    <asp:ListItem Value="4">小于</asp:ListItem>
                                    <asp:ListItem Value="5">小于等于</asp:ListItem>
                                    <asp:ListItem Value="6">包含</asp:ListItem>
                                    <asp:ListItem Value="7">不包含</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="比较值">
                            <ItemTemplate>
                                <asp:TextBox ID="txtValue" runat="server" Width="85" Text='<%# DataBinder.Eval(Container.DataItem, "Expression") %>'>  <%--Width="100"--%>
                                </asp:TextBox>
                                   <%-- <asp:Button ID="cmdPop" runat="server" CssClass="btnClass2" style="visibility: hidden; width:25px; height:22px;" Text="..." CommandName="commandCmdPop" />--%>
                                <input id="cmdPop" runat="server" class="btnClass2"  onclick="PopCondValue(this)" type="button" style="visibility: hidden" value="..."><%--&nbsp;--%> <%--onclick="PopCondValue(this)"--%>
                                <input id="hidValue" runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Expression") %>'>
                                <input id="HidTag" runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Tag") %>'>
                                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
                            </ItemTemplate>
                            <ItemStyle Width="140px" Wrap="True" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:Button ID="cmdOK" runat="server" SkinID="btnClass1" OnClick="cmdOK_Click" Text="添加"
                                    CausesValidation="False" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False"
                                    SkinID="btnClass1"></asp:Button>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="44" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CondType" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <HeaderStyle CssClass="listTitle" />
                </asp:DataGrid>
            </td>
           
        </tr>
    </table>
    <br />
    <table id="Table13" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr3" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            标准设置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" align="center" runat="server" id="Table3" cellpadding="0"
        cellspacing="0" border="0">
        <tr>
            <td style="width: 100%">
                <asp:DataGrid ID="dgSLTime" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="fixed-grid-border2"  Style="border: 1px solid #A3C9E1;"
                    OnItemDataBound="dgSLTime_ItemDataBound" OnItemCommand="dgSLTime_ItemCommand">
                    <Columns>
                        <asp:BoundColumn DataField="ID" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="GuidID" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Saved" Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="标准选择">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddltGuidID" runat="server" Width="120">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="时限">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTime" Style="ime-mode: Disabled" Text='<%# DataBinder.Eval(Container, "DataItem.TimeLimit")%>'
                                    onblur="CheckIsnum(this,'时间必须为数值！');" onkeydown="NumberInput('');" runat="server"
                                    MaxLength="10" Width="120px"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="时限单位">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddltUnit" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "TimeUnit") %>'
                                    Width="120">
                                    <asp:ListItem Value="0" Selected="True">分钟</asp:ListItem>
                                    <asp:ListItem Value="1">小时</asp:ListItem>
                                    <asp:ListItem Value="2" >天</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="达标率(%)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTarget" Style="ime-mode: Disabled" Text='<%# DataBinder.Eval(Container, "DataItem.Target")%>'
                                    runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="其它说明">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRemark" Text='<%# DataBinder.Eval(Container, "DataItem.Remark")%>'
                                    runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:Button ID="cmdTimeAdd" runat="server" SkinID="btnClass1" OnClick="cmdTimeAdd_Click"
                                    Text="添加" CausesValidation="False" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False"
                                    SkinID="btnClass1"></asp:Button>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="44" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="listTitle" />
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />

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
            var ImgID = tableid.replace("ctl00_ContentPlaceHolder1_Table","Img");
            var imgCtrl = document.all.item(ImgID)
            imgCtrl.src = ImgPlusScr ;	
        }
    }
    </script>

</asp:Content>
