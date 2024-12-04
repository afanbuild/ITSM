<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_ChangeQuery.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_ChangeQuery"
    Title="变更管理" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>

<%@ Register Src="../Controls/ctrCondition.ascx" TagName="ctrCondition" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script src="../Js/Common.js" type="text/javascript"></script>
    
    <!--Begin: 引入基础脚本库-->
    <script type="text/javascript" language="javascript" src="../js/epower.base.js"></script>
    <!--End: 引入基础脚本库-->

    <script language="javascript" type="text/javascript">
$.ajaxSetup({ cache: false });
    function OpenDeleteFlow(obj)  //删除流程
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete","hidDelete")).value;                
        var url = "../Common/frmFlowDelete.aspx?FlowID=" + FlowID +"&Opener_hiddenDelete=<%=hidd_btnDelete.ClientID %>&TypeFrm=cst_Issue_list";
        
        var height=($(document).height() - 230)/2 + +$(window).scrollTop();
        var width=($(document).width() - 320)/2 + +$(window).scrollLeft();              
        
        var _xy = epower.tools.computeXY('c', window, 320, 230);                
        width = _xy.x;
        height = _xy.y;
        
		open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=320px,height=230px,left=' + width +',top=' + height );
    }
     
    function txtFastQueryClear()
    {
        if(document.getElementById('<%=txtFastQuery.ClientID%>').value=="请输入变更单号,客户信息")
        {
            document.getElementById('<%=txtFastQuery.ClientID%>').value="";
        }
    }  
    
   	function Show_SQLWhere()
	{
	    var varSQLName = document.getElementById('<%=hidSQLName.ClientID%>').value;
	   
	    //=========zxl====
	  
	   window.open("../EquipmentManager/frm_Change_AdvancedCondition.aspx?SQLName="+
	   varSQLName+"&randomid="+GetRandom()+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>"+"&viewName="+escape('变更单'),"",'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=750,height=350px,left=150,top=50'); 
	}
	     
    function ShowTable(imgCtrl)
       {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              var tableCtrl;
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
        
        
        // 鼠标移出  查看服务流程     查看服务说明  隐藏Div
        function mouseOut(divId,ifId) {
	        var divIdElement = document.getElementById(divId);
	        var ifIdElement  = document.getElementById(ifId);
	        if(divIdElement != null && divIdElement != "null") {
		        divIdElement.style.display="none";
	        }
	        if(ifIdElement != null && ifIdElement != "null") {
		        ifIdElement.style.display="none";
	        }
        }

        function mouseOver(divId,ifId){
	        var divIdElement = document.getElementById(divId);
	        var ifIdElement  = document.getElementById(ifId);
	        if(divIdElement != null && divIdElement != "null") {
		        divIdElement.style.display="";
	        }
	        if(ifIdElement != null && ifIdElement != "null") {
		        ifIdElement.style.display="";
	        }
        }
        
	    function get_all_list(type) 
        { 
           var result =true;     
           if(type=='ChangeA')
           {
                type=type+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
           }
 
            $.ajax({
                type: "post",
                data:"type="+type,
                async:false,
                url: "../AppForms/CST_Issue_List.ashx",
                success: function(data){
                    
                    $("#tlist1").html(data);
                    
                    if(data=="")
                    {   
                       result =false;
                    }
		        }

             });
             
           return result;

        }
    
        //变更登单
        function showServiceConfig2(divId){
        
            var type='ChangeCountA'+'|'+document.getElementById('<%=hidUserID.ClientID%>').value;
            
            $.get("../AppForms/CST_Issue_List.ashx", 
                { Type: type }, 
                function(data) 
                { 
                    if(data != '')
                    {
                        document.getElementById(divId).style.display="none";
                         window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + data,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
                    }
                    else
                    {
                        var result= false;
                        result = get_all_list('ChangeA');
            
                        if(result==false)
                        {
                            alert("请先确保已经存在变更流程且您有启动权限!");
                            
                            document.getElementById(divId).style.display="none";
                        }
                        else
                        {
                            var display=document.getElementById(divId).style.display;
                            if(display==""){
	                            document.getElementById(divId).style.display="none";
                            }
                            if(display=="none"){
	                            document.getElementById(divId).style.display="";
                            }
                        }
                    }
                });
         }
         
         function AddNewFlow(id)  //打开相应的流程登单界面
        {
            var isnew = 'False';
            var isstrExtPara = '';
            if(isnew == 'False')
            {
                if(isstrExtPara !='')
                {
                    window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + id + "&ep=" + isstrExtPara,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
                }
                else
                {
                    window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + id,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
                }
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
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "../AppForms/frmBr_CustomXmlHttp.aspx?V_EquChange=" + id }).responseText; } });
        }
    
    </script>
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    
    <input id="hidUserID" value="" runat="server" type="hidden" />
    <input id="HiddenColumn" value="" runat="server" type="hidden" />
    <table id="Table12" width="98%" align="center" runat="server" class="listTitleNew">
        <tr id="tr2" valign="top">
            <td align="left" style="display: none; height: 0px">
                <asp:TextBox ID="txtFastQuery" runat="server" Width="199px" onmouseover="this.style.backgroundColor='#FFFBE8'"
                    onmouseout="this.style.backgroundColor='#FFFFFF'">请输入变更单号,客户信息</asp:TextBox>
                <asp:Button ID="btn_query" runat="server" Text="查询" CssClass="btnClass" OnClick="btn_query_Click" />
                <asp:Button ID="btn_addnew" runat="server" Text="新建" CssClass="btnClass" OnClick="btn_addnew_Click" />
                <asp:Button ID="btn_excel" runat="server" Text="导出" CssClass="btnClass" OnClick="btn_excel_Click" />
                <asp:Button ID="btnOk" CausesValidation="false" runat="server" OnClick="HidButton_Click" Width="0px" Height="0px" />
            </td>
            <td align="left" valign="top" class="list">
                <div style="display:none;">
                           <asp:DropDownList ID="DropSQLwSave" runat="server" Width="152px" AutoPostBack="True"
                    OnSelectedIndexChanged="DropSQLwSave_SelectedIndexChanged">
                </asp:DropDownList>
                <img class="icon" style="cursor: hand" id="Img2" onclick="Show_SQLWhere();" title="高级条件查询"
                    height="16" src="../Images/ss_01.gif" width="16" />
                <input id="hidTable" runat="server" type="hidden" />
                <input id="hidSQLName" value="" runat="server" type="hidden" />
                </div>
                
                <uc1:ctrCondition ID="ctrCondition" runat="server" Visible="true" />
            </td>
        </tr>
    </table>
    <table class="listContent" width="98%" id="Table2" style="display: none" cellpadding="2"
        cellspacing="0">
        <tr>
            <td class="listTitleRight" width="12%">
                <asp:Literal ID="LitCustInfo" runat="server" Text="用户信息"></asp:Literal>
            </td>
            <td class="list" width="35%">
                <asp:TextBox ID="txtCustInfo" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                流程状态
            </td>
            <td class="list">
                <asp:DropDownList ID="cboStatus" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight">
                <asp:Literal ID="litChangeDealStatus" runat="server" Text="变更状态"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="CtrDealState" runat="server" RootID="1022" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="litChangeFTSubject" runat="server" Text="标题"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="litChangeCustTime" runat="server" Text="变更时间"></asp:Literal>
            </td>
            <td class="list" align="left" colspan="3">
                <asp:TextBox ID="txtRegTime" runat="server" Width="128px"></asp:TextBox>
                <img id="imgTBSJ" style="cursor: hand" src="../Controls/Calendar/calendar.gif" runat="server">
                <asp:RegularExpressionValidator ID="Regularexpressionvalidator2" runat="server" ErrorMessage="格式错误"
                    Display="None" ControlToValidate="txtRegTime" ValidationExpression="^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$">*</asp:RegularExpressionValidator>
                --
                <asp:TextBox ID="txtEndTime" runat="server" Width="128px"></asp:TextBox>
                <img id="imgEnd" style="cursor: hand" src="../Controls/Calendar/calendar.gif" runat="server">
                <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" ErrorMessage="格式错误"
                    Display="None" ControlToValidate="txtEndTime" ValidationExpression="^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtEquipmentName" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtEquipmentDir" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="litChangelevel" runat="server" Text="变更级别"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="CtrFCDlevel" runat="server" RootID="1025" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="litChangeEffect" runat="server" Text="影响度"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="CtrFCDEffect" runat="server" RootID="1026" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="litChangeInstancy" runat="server" Text="紧急度"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="CtrFCDInstancy" runat="server" RootID="1027" />
            </td>
        </tr>
         <tr>
            <td class="listTitleRight">
                <asp:Literal ID="Literal1" runat="server" Text="变更类别"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <uc2:ctrFlowCataDropList ID="CtrChangeType" runat="server" RootID="1033"/>
            </td>
        </tr>
    </table>
    <table cellpadding="0" width="98%" cellspacing="0" border="0">
        <tr id="trShowControlPre" runat="server" style="height: 10px;">
            <td height="20" colspan="2" align="right">
                <div style="width: 300px; height: 30px; text-align: right; position: relative">
                    <input id="cmdFlowList" type="button" value="创建变更" onclick="javascript:showServiceConfig2('servicClass3')"
                        class="btnClass">
                    <div id="servicClass3" style="width: 160px; text-align: center; position: absolute;
                        z-index: 100; left: 140px; top: 20px; display: none; border: 1px solid #111;
                        background-color: #eee; padding: 5px; opacity: 0.85;" onmouseout="mouseOut('servicClass3')"
                        onmouseover="mouseOver('servicClass3')">
                        <table class="listContent">
                            <tr>
                                <td class="list" id="tlist1" width="135px" align="left">
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgProblem"  runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCreated="dgProblem_ItemCreated" OnDeleteCommand="dgProblem_DeleteCommand"
                    OnItemDataBound="dgProblem_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle CssClass="listTitle" HorizontalAlign="Left"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="变更单号">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblChangeNo" Text='<%#DataBinder.Eval(Container, "DataItem.ChangeNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Custname" HeaderText="客户名称">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Subject" HeaderText="主题">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="ChangeTypeName" HeaderText="变更类别">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="InstancyName" HeaderText="紧急度">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EffectName" HeaderText="影响度">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <%--<asp:BoundColumn DataField="EquipmentName" HeaderText="资产名称" Visible="false"></asp:BoundColumn>--%>
                        <asp:BoundColumn DataField="ChangeTime" HeaderText="变更日期" ItemStyle-HorizontalAlign="Left"
                            DataFormatString="{0:g}">
                            <ItemStyle Width="8%" HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DealStatus" HeaderText="变更状态">
                            <ItemStyle Width="8%" HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="流程状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>                       
                            <ItemStyle  HorizontalAlign="Center"></ItemStyle> 
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='详情' runat="server">
                            </ItemTemplate>                           
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="删除">
                            <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>                       
                            <ItemStyle  HorizontalAlign="Center"></ItemStyle>                             
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" SkinID="btnClass1"
                                    OnClientClick="OpenDeleteFlow(this);" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                          <asp:Button ID="hidd_btnDelete" runat="server" style="display:none;"  Text="删除时重新调用数据" 
                    onclick="hidd_btnDelete_Click" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc3:ControlPageFoot ID="cpChange" runat="server" />
            </td>
        </tr>
    </table>

    <script language="javascript">	
    var temp = document.all.<%=hidTable.ClientID%>.value;
    var ImgPlusScr ="../Images/ss_01.gif"	;      	// pic Plus  +
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
        var tableid="Table2";
        var tableCtrl = document.all.item(tableid);
        var ImgID = "Img2";
        var imgCtrl = document.all.item(ImgID)
        tableCtrl.style.display ="none";
        imgCtrl.src = ImgPlusScr ;	
    }
    </script>

</asp:Content>
