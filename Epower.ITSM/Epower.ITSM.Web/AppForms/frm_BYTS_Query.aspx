<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frm_BYTS_Query.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_BYTS_Query" Title="无标题页" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc5" %>

<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc4" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/ControlPageFoot.ascx" tagname="ControlPageFoot" tagprefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="../Controls/Calendar/Popup.js"></script>
<script language="javascript" type="text/javascript">
    function OpenDeleteFlow(obj)  //删除流程
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete","hidDelete")).value;
         var	value=window.showModalDialog("../Common/frmFlowDelete.aspx?FlowID=" + FlowID,window,"dialogHeight:230px;dialogWidth:320px");
        if(value!=null)
        {
            if(value[0]=="0") //成功
                event.returnValue = true;
            else
                event.returnValue = false;
        }
        else
        {
            event.returnValue = false;
        }
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
                
                document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id;
              }
              else
              {
                tableCtrl.style.display ="none";
                imgCtrl.src = ImgPlusScr ;	
                var temp = document.all.<%=hidTable.ClientID%>.value;
                document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
              }
        }
</script>

        <style>
        #tooltip
        {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #eee;
            padding: 5px;
            opacity: 0.85;
        }
        #tooltip h3, #tooltip div
        {
            margin: 0;
        }
    </style>
<script src="../Js/jquery.tooltip.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $.ajaxSetup({ cache: false });
        function ShowDetailsInfo(obj, id) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?V_BYTS=" + id }).responseText; } });
        }
    
    </script>
    
<input id="hidTable" value="" runat="server" type="hidden" /> 
	    <table class="listContent" width="100%" id="Table2">
        <tr>
            <TD noWrap align="left" class="listTitle" style="width:99px;">
               投诉人</TD>
            <TD align="left" class="list" style="width:*">
                <asp:TextBox id="txtBYPersonName" runat="server"></asp:TextBox></TD>
            <TD noWrap align="left" class="listTitle" style="width:99px;">
                被投诉人</TD>
            <TD align="left" class="list"  style="width:*">
                <uc5:UserPicker ID="UserPicker1" runat="server"  />
            </TD>
            <TD noWrap align="left" class="listTitle" style="width:99px;">
                流程状态</TD>
            <TD align="left" class="list"  style="width:*">
                    <asp:DropDownList id="cboStatus" runat="server"></asp:DropDownList></TD>
        </TR>
           <tr>
            <TD noWrap align="left" class="listTitle">登记日期</TD>
            <TD colspan="5" noWrap align="left" class="list">
                <asp:TextBox id="txtMsgDateBegin" runat="server" Width="108px"></asp:TextBox>
                <IMG id="imgSBegin" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand">~
                <asp:TextBox id="txtMsgDateEnd" runat="server" Width="108px"></asp:TextBox>
                <IMG id="imgEEnd" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand">
                <asp:RegularExpressionValidator id="Regularexpressionvalidator2" runat="server" ErrorMessage="格式错误" Display="None"
                    ControlToValidate="txtMsgDateBegin" ValidationExpression="^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$">*</asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator Display="None" ID="Regularexpressionvalidator3" runat="server" ControlToValidate="txtMsgDateEnd"
                    ErrorMessage="格式错误" ValidationExpression="^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$">*</asp:RegularExpressionValidator>
            </TD>
        </TR>
        </table>	
        <table id="Table12" width="100%" align="center"  runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td vAlign="top" align="left"  class="listTitleNew">
                  <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_expandall.gif" width="16"/>高级条件
            </td>
        </tr>
        </table>
       <table class="listContent"  width="100%"  id="Table3" style="display:none">
       <TR>
            <TD noWrap align="left" class="listTitle" style="width:99px;">
                投诉来源</TD>
            <TD align="left" class="list"  style="width:*">
                <uc3:ctrFlowCataDropList ID="CataSource" runat="server"  RootID="1009" />
            </TD>
            <TD noWrap align="left" class="listTitle" style="width:99px;">
               投诉类型</TD>
            <TD align="left" class="list"  style="width:*">
                <uc3:ctrFlowCataDropList ID="CataType" runat="server" RootID="1010" />
            </TD>
            <TD noWrap align="left" class="listTitle" style="width:99px;">
                投诉性质</TD>
            <TD align="left" class="list"  style="width:*">
                <uc3:ctrFlowCataDropList ID="CataKind" runat="server" RootID="1011" />
            </td>
        </TR>
        <TR>
            <TD noWrap align="left" class="listTitle">
                接收时间</TD>
            <TD align="left" class="list" colspan="5">
                <asp:TextBox id="Ctr_ReceiveTime" runat="server" Width="108px"></asp:TextBox>
                <IMG id="Img1" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand">~
                <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator4" runat="server" ControlToValidate="Ctr_ReceiveTime"
                    ErrorMessage="格式错误" ValidationExpression="^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$">*</asp:RegularExpressionValidator>
             <asp:TextBox id="Ctr_ReceiveTimeEnd" runat="server" Width="108px"></asp:TextBox>
                <IMG id="Img2" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand">
                <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator1" runat="server" ControlToValidate="Ctr_ReceiveTimeEnd"
                    ErrorMessage="格式错误" ValidationExpression="^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$">*</asp:RegularExpressionValidator>
            </TD>  
           </TR>
       </table>            
<br/>
<TABLE cellPadding="0" width="100%" align="center" class="listContent">
<TR>
	<TD colspan=4 class="listContent"><asp:datagrid id="gridUndoMsg" runat="server" Width="100%"  AutoGenerateColumns="False"  CssClass="Gridtable" 
			EnableViewState="True" OnItemDataBound="gridUndoMsg_ItemDataBound" OnItemCreated="gridUndoMsg_ItemCreated" OnDeleteCommand="gridUndoMsg_DeleteCommand">
			<Columns>
			    <asp:TemplateColumn HeaderText="投诉人">                        
                    <ItemTemplate>
                        <asp:Label runat="server" id="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.BY_PersonName")%>'></asp:Label>              
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateColumn>                          
				<asp:BoundColumn DataField="BY_ProjectName" HeaderText="被投诉人">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_SoureName" HeaderText="投诉来源">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_TypeName" HeaderText="投诉类型">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_KindName" HeaderText="投诉性质">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_ReceiveTime" HeaderText="接收时间" DataFormatString="{0:d}">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="RegTime" HeaderText="登记日期" DataFormatString="{0:g}">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="status" HeaderText="流程状态">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:TemplateColumn HeaderText="处理" ItemStyle-HorizontalAlign="center">
					<ItemTemplate>
						<INPUT id="CmdDeal" class="btnClass" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>' type="button" value='<%#GetButtonValue((int)DataBinder.Eval(Container.DataItem, "status"))%>' runat="server">
					</ItemTemplate>
					<HeaderStyle HorizontalAlign="center" Width="5%" />
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
				<asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
		            <HeaderStyle Width="5%"></HeaderStyle>
		            <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" CssClass="btnClass" OnClientClick="OpenDeleteFlow(this);" />
                        <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
		            </ItemTemplate>
	            </asp:TemplateColumn>
			</Columns>
		</asp:datagrid>
	</TD>
</TR>
<TR>
	<TD colspan="4" align="right" class="listTitle">
        <uc6:ControlPageFoot ID="cpByts" runat="server" />
    </TD>
</TR>
</TABLE>	
<script language="javascript">	
    var temp = document.all.<%=hidTable.ClientID%>.value;
    var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
    var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus -
    if(temp!="")
    {
        var arr=temp.split(",");
        for(i=1;i<arr.length;i++)
        {
            var tableid=arr[i];
            var tableCtrl = document.all.item(tableid);
            tableCtrl.style.display ="";
            var ImgID = tableid.replace("Table","Img");
            var imgCtrl = document.all.item(ImgID)
            imgCtrl.src = ImgMinusScr ;	
        }
    }
    else
    { 
        var tableid="Table3";
        var tableCtrl = document.all.item(tableid);
        var ImgID = "Img3";
        var imgCtrl = document.all.item(ImgID)
        tableCtrl.style.display ="none";
        imgCtrl.src = ImgPlusScr ;	
    }
</script> 
</asp:Content>
