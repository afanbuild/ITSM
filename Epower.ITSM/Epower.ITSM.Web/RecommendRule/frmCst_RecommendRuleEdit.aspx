<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false"
    ValidateRequest="false" AutoEventWireup="true" CodeBehind="frmCst_RecommendRuleEdit.aspx.cs"
    Inherits="Epower.ITSM.Web.RecommendRule.frmCst_RecommendRuleEdit" Title="推荐规则" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">	
			function PopCondValue(obj)
			{
				reg=/cmdPop/g;
				var idtag=obj.id.replace(reg,"");
				
				var v=document.all(idtag+"cboItems").value;
				var itemType=v.split(',')[1];
				var itemID = v.split(',')[0];
				switch(itemType)
				{
					case 'CATA' ://分类情况
					    var newDateObj = new Date()
	                    var sparamvalue =  newDateObj.getDate().toString() + newDateObj.getHours().toString() + newDateObj.getMinutes().toString() + newDateObj.getSeconds().toString();
	                    //===zxl==
	                    var url="frmRecommendRuleCata.aspx?id=" + itemID + "&param=" + sparamvalue+"&objID="+obj.id;
	                    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=220,height=160,left=150,top=50");

						break;
					
				}
				
			}
			 function UserSelect2(obj) 
			{
			
			    var url="../CustManager/frmCst_ServiceStaffMain.aspx?objID="+obj.id+"&IsSelect=true"+"&TypeFrm=frmCst_RecommonRuleEdit";
			    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600,left=150,top=50");
			    
			}
			//是否验证是否为数字
			function CheckInputValue(obj)
			{
			    var ddlFieldType = document.getElementById(obj.id.replace("txtValue","cboItems"));	
                var SelectField = ddlFieldType.options[ddlFieldType.selectedIndex].value;
			    var FieldType=SelectField.split(',')[1];
			    switch(FieldType)
			    {
				    case "CHAR":					
					    break;
				    case "INTER":
				        var svalue = obj.value;
                        if (isNaN(svalue))
                        {
                            alert("请输入数字！");
                            obj.focus(); 
                            obj.select(obj.value.length);
                        }
				        break;
				    case "CATA":				
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
				case "INTER":
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
					newOption.text  = "大于";   //大于等于
					newOption.value = "2";
					ddlOperator.options.add(newOption);	

					newOption = document.createElement("OPTION");					
					newOption.text  = "大于等于";
					newOption.value = "3";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "小于";
					newOption.value = "4";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "小于等于";
					newOption.value = "5";
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
		
		//根据字段的改变更改比较符
		function ChangeOp(obj)
		{
			//var ddlOperator = document.getElementById(obj.id.replace("cboItems","cboOperate"));	
			
			var objhidOperate = document.getElementById(obj.id.replace("cboOperate","hidOperate"));			
			//var SelectField = ddlOperator.options[ddlOperator.selectedIndex].value;
			objhidOperate.value = obj.options[obj.selectedIndex].value;
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
        function UserSelect(obj,type) 
			{
		
				var	value=window.showModalDialog("../CustManager/frmCst_ServiceStaffMain.aspx?IsSelect=true&randomid="+GetRandom(),"","dialogHeight:600px;dialogWidth:800px");
				if(value != null)
				{
					if(value.length>1)
					{
					    if(type=="2")   //类型为1表示修改
					    {
				            document.getElementById(obj.id.replace("cmdUserName","txtUserName") ).value = value[2].replace("&nbsp;", "");  //姓名
				            document.getElementById(obj.id.replace("cmdUserName","hidStaffID")).value = value[1].replace("&nbsp;", "");   //ID
				            document.getElementById(obj.id.replace("cmdUserName","hidBlongDeptName")).value = value[3].replace("&nbsp;", "");   //服务单位
				            document.getElementById(obj.id.replace("cmdUserName","txtUserName")).focus();
				        }
				        else
				        {
				            document.getElementById(obj.id.replace("cmdAddUserName","txtAddUserName") ).value = value[2].replace("&nbsp;", "");  //姓名
				            document.getElementById(obj.id.replace("cmdAddUserName","hidAddStaffID")).value = value[1].replace("&nbsp;", "");   //ID
				            document.getElementById(obj.id.replace("cmdAddUserName","hidAddBlongDeptName")).value = value[3].replace("&nbsp;", "");   //服务单位
				            document.getElementById(obj.id.replace("cmdAddUserName","lblAddBlongDeptName")).innerText = value[3].replace("&nbsp;", "");   //服务单位
				            document.getElementById(obj.id.replace("cmdAddUserName","txtAddUserName")).focus();
				            
				        }
					}
				}
				else
				{
				    if(type=="2")   //类型为1表示修改
					     if(type=="2")   //类型为1表示修改
					    {
				            document.getElementById(obj.id.replace("cmdUserName","txtUserName") ).value = "";  //姓名
				            document.getElementById(obj.id.replace("cmdUserName","hidStaffID")).value = "0";   //ID
				            document.getElementById(obj.id.replace("cmdUserName","hidBlongDeptName")).value = "";   //服务单位
				            document.getElementById(obj.id.replace("cmdUserName","txtUserName")).focus();
				        }
				        else
				        {
				            document.getElementById(obj.id.replace("cmdAddUserName","txtAddUserName") ).value = "";  //姓名
				            document.getElementById(obj.id.replace("cmdAddUserName","hidAddStaffID")).value = "0";   //ID
				            document.getElementById(obj.id.replace("cmdAddUserName","hidAddBlongDeptName")).value = "";   //服务单位
				            document.getElementById(obj.id.replace("cmdAddUserName","txtAddUserName")).focus();
				        }
				}
			}
			 
			
    </script>
    
    <table id="Table11" width="98%" align="center" runat="server" class="listNewContent">
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
    <table style="width: 98%" class="listContent Gridtable" runat="server" id="Table1" cellpadding="2"
        cellspacing="0">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                规则名称<asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
            <td class='list' style='width: 35%;'>
                <asp:TextBox ID='txtRuleName' runat='server' MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRuleName"
                    ErrorMessage="规则名称不能为空!" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:Label ID="labRuleName" runat="server" Visible="False"></asp:Label>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                是否有效
            </td>
            <td class='list' style='width: 35%;'>
                <asp:RadioButtonList ID="rdbtIsAvail" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0" Selected="True">有效</asp:ListItem>
                    <asp:ListItem Value="1">无效</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                规则说明
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID='txtDesc' runat='server' Rows="3" MaxLength="200" TextMode="MultiLine"
                    Width="98%" onblur="MaxLength(this,200,'规则说明长度超出限定长度:');"></asp:TextBox>
                <asp:Label ID="labDesc" runat="server" Visible="False"></asp:Label>
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
                <asp:DataGrid ID="dgCondition" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  Width="100%"
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
                        <asp:TemplateColumn HeaderText="内容项">
                            <ItemTemplate>
                                <asp:DropDownList ID="cboItems" runat="server" onchange="ChangeFieldName(this);"
                                    Width="200px">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="比较关系">
                            <ItemTemplate>
                                <asp:DropDownList ID="cboOperate" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Operate") %>'
                                    onchange="ChangeOp(this);" Width="80">
                                    <asp:ListItem Selected="True" Value="0">等于</asp:ListItem>
                                    <asp:ListItem Value="1">不等于</asp:ListItem>
                                    <asp:ListItem Value="2">大于</asp:ListItem>
                                    <asp:ListItem Value="3">大于等于</asp:ListItem>
                                    <asp:ListItem Value="4">小于</asp:ListItem>
                                    <asp:ListItem Value="5">小于等于</asp:ListItem>
                                    <asp:ListItem Value="6">包含</asp:ListItem>
                                    <asp:ListItem Value="7">不包含</asp:ListItem>
                                </asp:DropDownList>
                                <input id="hidOperate" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "Operate") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="比较值">
                            <ItemTemplate>
                                <asp:TextBox ID="txtValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Expression") %>'
                                    onblur="CheckInputValue(this);" Width="100">                                
                                </asp:TextBox>
                                <input id="cmdPop" runat="server" class="btnClass2" onclick="PopCondValue(this)" type="button"
                                    style="visibility: hidden" value="...">&nbsp;
                                <input id="hidValue" runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Expression") %>'>
                                <input id="HidTag" runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Tag") %>'>
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
                            相关工程师
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" align="center" runat="server" id="Table3" cellspacing="0"
        border="0" cellpadding="0">
        <tr>
            <td style="width: 100%">
                <asp:DataGrid ID="dgCst_ServiceStaff" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    Width="100%" ShowFooter="True" OnItemDataBound="dgCst_ServiceStaff_ItemDataBound"
                    OnItemCommand="dgCst_ServiceStaff_ItemCommand">
                    <Columns>
                        <asp:BoundColumn DataField="RuleID" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="StaffID" Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="工程师姓名">
                            <HeaderStyle Width="40%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Name")%>'></asp:Label>
                                <asp:TextBox ID="txtUserName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name")%>'
                                    Width="80%" MaxLength="50"></asp:TextBox>
                                <input id="cmdUserName" onclick="UserSelect(this,'2')" type="button" value="..."
                                    name="cmdSubject" runat="server" class="btnClass2" />
                                   
                                    
                                    
                                <input type="hidden" runat="server" id="hidStaffID" value='<%# DataBinder.Eval(Container, "DataItem.StaffID")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblAddUserName" runat="server" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtAddUserName" runat="server" Width="80%" MaxLength="50"></asp:TextBox>
                              <input id="cmdAddUserName" onclick="UserSelect2(this)" type="button" value="..."
                                    name="cmdAddSubject" runat="server" class="btnClass2" />
                                    <%-- <asp:Button ID="cmdUserName" runat="server" Text="..."
                                      style="width:20px; height:22px;" CssClass="btnClass2" 
                                      CommandName="command_cmdUserName"  />--%>
                                     <asp:HiddenField ID="hidClientId_ForOpenerPage" runat="server" />
                                    
                                <input type="hidden" runat="server" id="hidAddStaffID" value="0" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="服务单位">
                            <HeaderStyle Width="55%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblBlongDeptName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BlongDeptName")%>'></asp:Label>
                                <input type="hidden" runat="server" id="hidBlongDeptName" value='<%# DataBinder.Eval(Container, "DataItem.BlongDeptName")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblAddBlongDeptName" runat="server"></asp:Label>
                                <input type="hidden" runat="server" id="hidAddBlongDeptName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False"
                                    SkinID="btnClass1" UseSubmitBehavior="false"></asp:Button>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Button ID="btnadd" CommandName="Add" runat="server" Text="添加" CausesValidation="False"
                                    SkinID="btnClass1"></asp:Button>
                            </FooterTemplate>
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
