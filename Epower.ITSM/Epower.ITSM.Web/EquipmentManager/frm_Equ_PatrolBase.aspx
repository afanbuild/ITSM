<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    CodeBehind="frm_Equ_PatrolBase.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_Equ_PatrolBase"
    Title="新增资产巡检" %>

<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/CtrMonitor.ascx" TagName="CtrMonitor" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
<!--
            function TransferValue()
            {
				if (typeof(document.all.<%=CtrFlowTitle.ClientID%>)!="undefined" )
				{
				    parent.header.flowInfo.Subject.value = document.all.<%=CtrFlowTitle.ClientID%>.value + "[巡检单]";
				}
            }

             function DoUserValidate(lngActionID,strActionName)
	        {
	            TransferValue()
			    return CheckValue();
		    }
		    
		
            //
			function CheckValue()
			{
			    return true;
			}
			//
			String.prototype.trim = function()  
			{
				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
			
			function SelectEquItem()
			{
			    var newDateObj = new Date();
	            var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
		        var features =
		        'dialogWidth:800px;' +
		        'dialogHeight:500px;' +
		        'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';
		        //=====zxl==
		           var url='frmEqu_SelectItem.aspx?pDate='+sparamvalue + '&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>';
		            window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=100");
			}
		function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              var className;
              var objectFullName;
              var tableCtrl;
              objectFullName = <%=Table11.ClientID%>.id;
              className = objectFullName.substring(0,objectFullName.indexOf("Table11")-1);
              tableCtrl = document.all.item(className.substr(0,className.length)+"_"+TableID);
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
        //打印
        function printdiv()
        {
            var flowid="<%=FlowID%>";
            var AppID="<%=AppID%>";
            var FlowMoldelID="<%=FlowModelID%>";
            window.open("../Print/printRule.aspx?FlowId="+flowid+"&AppID="+AppID+"&FlowMoldelID="+FlowMoldelID);
            return false;
        }
        //知识参考
        function FormDoKmRef()
        {
            window.open("../InformationManager/frmInf_InformationMain.aspx?IsSelect=1","","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
        }
  //-->
    </script>
    

    <asp:Button ID="hidbtnAddItem" runat="server" Width="0px" Style="display:none;" Text="Button" OnClick="btnadd_Click" />
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <input id="hidItemArrID" runat="server" type="hidden" />
    <table id="Table11" width="100%" align="center" runat="server">
        <tr>
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
    <table class="listContent" width="100%" border="0" cellSpacing="1" cellPadding="1" runat="server" id="Table1">
        <tr>
            <td class='listTitleRight' style="width: 12%">
                登记人
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="lblRegUserName" runat="server"></asp:Label>
            </td>
            <td class='listTitleRight' style="width: 12%">
                登记部门
            </td>
            <td class="list">
                <asp:Label ID="lblRegDeptName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 12%" class='listTitleRight'>
                标题
            </td>
            <td colspan="3" class="list">
                <uc4:CtrFlowFormText ID="CtrFlowTitle" runat="server" TextToolTip="标题" MustInput="true"
                    MaxLength="100" Width="85%" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                备注
            </td>
            <td class="list" colspan="3">
                <uc2:CtrFlowRemark ID="CtrFlowRemark1" runat="server" OnBlurScript="MaxLength(this,100,'备注长度超出限定长度:');" />
            </td>
        </tr>
        <tr id="trShowMonitor" width="100%" runat="server">
            <td nowrap align="left" class='listTitleRight' style="width: 12%">
                督办内容
            </td>
            <td colspan="3" class="list">
                <uc5:CtrMonitor ID="CtrMonitor1" runat="server" />
            </td>
        </tr>
    </table>
    <table id="Table12" width="100%" align="center" runat="server">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            巡检信息
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%" runat="server" id="Table2" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td colspan="4">
                <asp:DataGrid ID="gdPatrol" runat="server" Width="100%" BorderColor="White" BorderWidth="0px"
                    CellPadding="1" CellSpacing="2" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="gdPatrol_ItemCommand"
                    OnItemDataBound="gdPatrol_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField='EquID' HeaderText='资产编号' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='ItemID' HeaderText='项编号' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='EquName' HeaderText='资产名称' HeaderStyle-Width="20%"></asp:BoundColumn>
                        <asp:BoundColumn DataField='ItemName' HeaderText='巡检项' HeaderStyle-Width="10%"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="正常" ItemStyle-Wrap="true">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkEffect" runat="server" Checked="true" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="巡检时间" ItemStyle-Wrap="false">
                            <HeaderStyle Width="35%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblPatrolTime" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.PatrolTime")%>'></asp:Label>
                                <uc1:ctrdateandtime ID="CtrPatrolTime" runat="server" dateTime='<%# DataBinder.Eval(Container, "DataItem.PatrolTime")%>'
                                    Disparity="True" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="巡检人">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblPatrolName" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.PatrolName")%>'></asp:Label>
                                <asp:TextBox ID="txtPatrolName" Text='<%# DataBinder.Eval(Container, "DataItem.PatrolName")%>'
                                    runat="server" Width="95%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="备注">
                            <HeaderStyle Width="15%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblRemark" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Remark")%>'></asp:Label>
                                <asp:TextBox ID="txtRemark" Text='<%# DataBinder.Eval(Container, "DataItem.Remark")%>'
                                    runat="server" Width="95%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="5%" VerticalAlign="Top"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Button ID="btnadd" runat="server" Text="新  增" CausesValidation="False" OnClientClick="SelectEquItem();"
                                    OnClick="btnadd_Click" CssClass="btnClass"></asp:Button>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删  除" CausesValidation="False"
                                    CssClass="btnClass"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ItemData' HeaderText='是否正常' Visible="false"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
