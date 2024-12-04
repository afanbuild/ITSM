<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="CST_Issue_List.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.CST_Issue_List"
    Title="事件单查询" %>

<%@ Register Src="../Controls/ServiceStaffMastCust.ascx" TagName="ServiceStaffMastCust"
    TagPrefix="uc4" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc3" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrCondition.ascx" TagName="ctrCondition" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .hand
        {
            cursor: pointer;
        }
    </style>

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script language="javascript" src="../Js/Common.js"></script>

    <script language="javascript" type="text/javascript">
    $.ajaxSetup ({cache: false});
    
     function DeleteSelectedFlow(flowId)  //删除流程
     {
        var url="../Common/frmFlowDelete.aspx?FlowID=" + flowId+"&Opener_hiddenDelete=<%=hidd_btnDelete.ClientID %>&TypeFrm=cst_Issue_list";
		open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=320px,height=230px,left=300,top=200');
     }

    function OpenDeleteFlow(obj)  //删除流程
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete","hidDelete")).value;
        var url="../Common/frmFlowDelete.aspx?FlowID=" + FlowID;
		open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=320px,height=230px,left=150,top=50');		
    }
    
    function ShowFind(Name)
	{
		OpenNoBarWindow2("../AppForms/CST_Issue_Base_Fast.aspx?CondID="+escape(Name),500,300);
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
       var userid=document.getElementById('<%=hidUserID.ClientID%>').value;
       if(type=='a')
       {
            type=type+'|'+userid;
       }
        else if(type=='ProblemA')
       {
            type=type+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
       }
       else if(type=='IssueB')
       {
            type=type+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
       }

             $.ajax({
                type: "post",
                data:"type="+type+"&userid="+userid+"&appid=1026",
                async:false,
                url: "../AppForms/CST_Issue_List.ashx",
                success: function(data, textStatus){
                        if(type=='b')
                        {
                            $("#tlist2").html(data);   
                        }
                        else 
                        {
                            $("#tlist1").html(data); 
                            $("#tlist3").html(data);        
                        }      
                        
                        if(data=="")
                        {   
                           result =false;
                        }
		        }

             });
       
       return result;
    }
    
    function showServiceConfig2(divId){
        
        var type = '';
        var userid=document.getElementById('<%=hidUserID.ClientID%>').value;
        if(divId=='servicClass3')
        {
            type='IssueCountA'+'|'+userid;
            document.getElementById('servicClass4').style.display="none";
        }
        else if(divId=='servicClass4')
        {
            type='IssueCountB'+"|1026";
            document.getElementById('servicClass3').style.display="none";
        }else{
            type='IssueCountC'+'|'+userid;
            document.getElementById('servicClass4').style.display="none";
        }
        $.get("../AppForms/CST_Issue_List.ashx", 
                { Type: type,Userid:userid }, 
                function(data) 
                { 
                    if(data != '')
                    {
                        document.getElementById(divId).style.display="none";
                        
                        if(divId=='servicClass3')
                        {
                            //基本流程
                             window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + data,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
                        }
                        else if(divId=='servicClass4')
                        {
                        
                            //快速登单       
	                        var wWidth = 500;
	                        var wHeight = 300;
	                        var wLeft = (window.screen.width - wWidth) / 2;
	                        var wTop = (window.screen.height - wHeight-30) / 2;
	                        if( wLeft<200)
		                        wLeft=0;
	                        if(wTop<200)
		                        wTop=0;
		                         var url="../AppForms/CST_Issue_Base_Fast.aspx?CondID="+escape(data);
		        
	                   window.location=url;
	                
                        }else{
                            window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + data,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
                        }
                    }
                    else
                    {
                        var result= false;
                        if(divId=='servicClass3')
                        {
                            result = get_all_list('a');
                        }
                        else if(divId=='servicClass4')
                        {
                            result = get_all_list('b');
                        }                                                                      
                        
                        if(result==false)
                        {
                            if(divId=='servicClass3')
                            {
                                alert("请先确保已经存在事件流程且您有启动权限!");
                            }
                            else if(divId=='servicClass4')
                            {
                                alert("请先设置相关事件请求模板!");
                            } 
                            else
                            {
                                alert("请先确保已经存在事件流程且您有启动权限！");
                            }
                        
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
      //合并重复事件
        function showServiceConfig3(divId)
        {
        
           var type='IssueCountA'+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
            
           $.get("../AppForms/CST_Issue_List.ashx", 
                { Type: type }, 
                function(data) 
                { 
                    if(data != '')
                    {
                        document.getElementById(divId).style.display="none";
                       
                        var flowidlist = "issuemerge" +  document.getElementById('<%=hidFlowIDList.ClientID%>').value;
                        if(document.getElementById('<%=hidFlowIDList.ClientID%>').value=="")
                        {
                            alert("请选择要合并的事件单！");
                            return;
                        }
                        if(confirm("确认合并事件单？"))
                        {
                           
                            window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + data + "&ep=" + flowidlist,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
                            
                        }
                    }
                    else
                    {
                  
                        var result= false;                       
                       result = get_all_list('IssueB');
                        
                        if(result==false)
                        {
                            alert("请先确保已经存在问题流程且您有启动权限!");
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
    
    function DoKmAdd(lngMessageID)
	{
	     window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=400&ep=" + lngMessageID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
	}
    function ChnageProblem(obj)  //转问题单
    {
        var lngFlowID = document.getElementById(obj.id.replace("btnChange","hidChange")).value;
        window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=210&ep=" + lngFlowID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
        event.returnValue = false;
    }
    
    function ChangeAssociate(obj)  //关联变更单

    {
        var lngFlowID = document.getElementById(obj.id.replace("btnAssociate","hidAssociate")).value;
        window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=420&ep=" + lngFlowID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
        event.returnValue = false;
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
        
    function SendMail()
    {
        window.open('../Common/frmSendMailFeedBack.aspx','','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
    }
    
    function checkAll(objectCheck) 
    {

        var demo = document.getElementById('<%=gridUndoMsg.ClientID%>');
        var gg = demo.getElementsByTagName('INPUT');
        for (i = 0; i < gg.length; i++) 
        {
            if (gg[i].type == "checkbox") {
                gg[i].checked = objectCheck.checked;
                checkFlowList(document.forms[0].elements[i]);
            }
        }
    }
    //合并事件单，组成FLOW编号 
        function checkFlowList(checkobj)
        {
        
            var lngFlowID = document.getElementById(checkobj.id.replace("chkDel","hidFlowID")).value;
            if(checkobj.checked)
            {
                document.getElementById('<%=hidFlowIDList.ClientID%>').value = document.getElementById('<%=hidFlowIDList.ClientID%>').value +  "," + lngFlowID;
            }
            else
            {
                document.getElementById('<%=hidFlowIDList.ClientID%>').value = document.getElementById('<%=hidFlowIDList.ClientID%>').value.replace(","+lngFlowID,"");
            }
        }
    
    function ShowDetailsInfo(obj, id) 
    {
   
        $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?ZHServiceDP=" + id }).responseText; } });
    }
    
    function SetUrl()
    {
          
         var fromurl = '<%=FromBackUrl%>';
         $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?formurl="+escape(fromurl) });
    }
    
    function AddNewFlowMerge(id)  //问题单合并打开登单界面
    
        {
       
            if(document.getElementById('<%=hidFlowIDList.ClientID%>').value=="")
            {
                alert("请选择重复的事件单！");
                return;
            }
            var isnew = 'False';
            var flowidlist = "issuemerge" +  document.getElementById('<%=hidFlowIDList.ClientID%>').value;
            var isstrExtPara = flowidlist;
            if(isnew == 'False')
            {
                if(confirm("确认重复事件单？"))
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
        }
        
    function AddNewFlow(id)  //删除流程
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
    
    function Show_SQLWhere()
	{
	    var varSQLName = document.getElementById('<%=hidSQLName.ClientID%>').value;	    
    	var url="../AppForms/CST_Issue_AdvancedConditionNew.aspx?SQLName="+varSQLName+"&randomid="+GetRandom()+"&viewName="+escape('事件单')+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
    	window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600,left=150,top=50");    	
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
    <input type="hidden" runat="server" id="hidFlowIDList" value="" />
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
    <input type="hidden" runat="server" id="hidIsGaoji" value="0" />
    <table class="listTitleNew" width="98%" align="center" id="tbTest" runat="server">
        <tr id="trShowCondi" runat="server">
            <td class="list" align="left" valign="top">
                <table width="100%" id="Table1">
                    <tr>
                        <td>
                            <uc1:ctrcondition id="ctrCondition" runat="server" visible="true" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td align="left" style="border-right-style: none;">
                            <asp:LinkButton ID="lkbMy" runat="server" OnClick="lkbMy_Click">由我登记</asp:LinkButton>
                            <asp:LinkButton ID="lkbProccessing" runat="server" OnClick="lkbProccessing_Click">正在处理</asp:LinkButton>
                            <asp:LinkButton ID="lkbProcessed" runat="server" OnClick="lkbProcessed_Click">正常结束</asp:LinkButton>
                            <asp:LinkButton ID="lkbOverTimeF" runat="server" OnClick="lkbOverTimeF_Click">超时完成</asp:LinkButton>
                            <asp:LinkButton ID="lkbOverTimeU" runat="server" OnClick="lkbOverTimeU_Click">超时未完成</asp:LinkButton>
                            <asp:DropDownList ID="ddlPeriod" runat="server" Width="84px">
                                <asp:ListItem Value="0">一个月内</asp:ListItem>
                                <asp:ListItem Value="1">三个月内</asp:ListItem>
                                <asp:ListItem Value="2">六个月内</asp:ListItem>
                                <asp:ListItem Value="3">一年内</asp:ListItem>
                                <asp:ListItem Value="4">全部</asp:ListItem>
                            </asp:DropDownList>
                            <div style="display:none;" >  <asp:DropDownList ID="DropSQLwSave" runat="server" Width="160px" AutoPostBack="True"
                                OnSelectedIndexChanged="DropSQLwSave_SelectedIndexChanged">
                            </asp:DropDownList>
                            <img class="icon" style="cursor: hand" id="Img2" onclick="Show_SQLWhere();" height="16"
                                src="../Images/ss_01.gif" width="16" title="高级条件查询" /></div>
                        </td>
                    </tr>
                </table>
                <input id="hidTable" value="" runat="server" type="hidden" />
                <input id="hidUserID" value="" runat="server" type="hidden" />
                <input id="hidSQLName" value="" runat="server" type="hidden" />
                <input id="HiddenColumn" value="" runat="server" type="hidden" />
            </td>
        </tr>
    </table>
    <table width="98%" align="center" id="TableSelect" runat="server" visible="false"
        class="listContent">
        <tr>
            <td style="color: #08699E; background-color: #E3F2FB" align="center" width="100%">
                <asp:Button ID="btnConfirm" runat="server" Text="确  定" Width="60" OnClick="btnConfirm_Click"
                    CssClass="btnClass" />
                &nbsp;
                <asp:Button ID="btnClose" runat="server" Text="取  消" Width="60" OnClick="btnClose_Click"
                    CssClass="btnClass" />
            </td>
        </tr>
    </table>
    <table width="98%" align="center" id="tblEmail" runat="server" class="listContent"
        visible="false">
        <tr>
            <td class="list" align="left">
                <asp:DropDownList ID="ddltEmail" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddltEmail_SelectedIndexChanged"
                    Width="100px">
                    <asp:ListItem Value="0">--全部--</asp:ListItem>
                    <asp:ListItem Value="1">有邮件</asp:ListItem>
                    <asp:ListItem Value="2">无邮件</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="list" align="right">
                <asp:Button ID="btnSendMail" runat="server" Text="批量邮件回访" SkinID="btnClass3" OnClick="btnSendMail_Click1" />
            </td>
        </tr>
    </table>
    <table width="98%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr id="trShowControlPre" runat="server" style="height: 10px;">
            <td height="20" colspan="2" align="right">
                <div style="width: 300px; height: 30px; text-align: right; position: relative">
                    <input id="cmdFlowList" type="button" value="创建事件" onclick="javascript:showServiceConfig2('servicClass3')"
                        class="btnClass">
                    <input id="cmdFastList" type="button" value="快速登单" onclick="javascript:showServiceConfig2('servicClass4')"
                        class="btnClass">
                    <input id="cmdMerge" type="button" value="重复事件" onclick="javascript:showServiceConfig3('servicClass5')"
                        class="btnClass" />
                    <div id="servicClass3" style="width: 140px; text-align: center; position: absolute;
                        z-index: 9999; left: 89px; top: 20px; display: none; border: 1px solid #111;
                        background-color: #eee; padding: 5px; opacity: 0.85;" onmouseout="mouseOut('servicClass3')"
                        onmouseover="mouseOver('servicClass3')">
                        <table class="listContent">
                            <tr>
                                <td class="list" id="tlist1" width="135px" align="left">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="servicClass4" style="width: 140px; text-align: center; position: absolute;
                        z-index: 9999; left: 160px; top: 20px; display: none; border: 1px solid #111;
                        background-color: #eee; padding: 5px; opacity: 0.85;" onmouseout="mouseOut('servicClass4')"
                        onmouseover="mouseOver('servicClass4')">
                        <table class="listContent">
                            <tr>
                                <td class="list" id="tlist2" width="135px" align="left">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="servicClass5" style="width: 140px; text-align: center; position: absolute;
                        z-index: 100; left: 160px; top: 20px; display: none; border: 1px solid #111;
                        background-color: #eee; padding: 5px; opacity: 0.85;" onmouseout="mouseOut('servicClass5')"
                        onmouseover="mouseOver('servicClass5')">
                        <table class="listContent">
                            <tr>
                                <td class="list" id="tlist3" width="135px" align="left">
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable" OnDeleteCommand="gridUndoMsg_DeleteCommand" OnItemCreated="gridUndoMsg_ItemCreated"
                    EnableViewState="false" OnItemDataBound="gridUndoMsg_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server" onclick="checkFlowList(this);"></asp:CheckBox>
                                <input id="hidFlowID" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="事件单号">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.ServiceNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="8%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CustName" HeaderText="客户名称">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ctel" HeaderText="联系电话">
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceLevel" HeaderText="服务级别">
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustTime" HeaderText="发生时间" DataFormatString="{0:g}">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EquipmentName" HeaderText="资产名称">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
			<asp:BoundColumn DataField="SubJect" HeaderText="标题">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                       
                        <asp:BoundColumn DataField="dealstatus" HeaderText="事件状态">
                            <HeaderStyle Width="5%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="响应时效">
                            <ItemTemplate>
                                <asp:Label ID="txtNew2" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="8%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="处理时效">
                            <ItemTemplate>
                                <asp:Label ID="txtNew1" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="8%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="当前处理人">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrName" runat="server" Text='<% #GetCurrName(Eval("FlowID")) %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="6%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle Width="5%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FlowID")))%>'
                                    type="button" value='<%#GetButtonValue(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "status")))%>'
                                    name="CmdDeal" runat="server" onmouseover="SetUrl(1);" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="删除">
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <input type="button" class="btnClass1" onmouseover="SetUrl(1);" onclick="javascript:DeleteSelectedFlow('<%#DataBinder.Eval(Container.DataItem, "flowid")%>');"
                                    value="删除" />
                                <asp:HiddenField ID="hidClientId_ForOpenerPage" runat="server" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="问题">
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnChange" runat="server" Text="问题" CommandName="Change" SkinID="btnClass1"
                                    OnClientClick="ChnageProblem(this);" />
                                <asp:Label ID="lblChange" runat="server" Text="已升级" ForeColor="red"></asp:Label>
                                <input id="hidChange" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="变更">
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnAssociate" runat="server" Text="变更" CommandName="Associate" SkinID="btnClass1"
                                    OnClientClick="ChangeAssociate(this);" />
                                <asp:Label ID="lblAssociate" runat="server" Text="已变更" Visible="false" ForeColor="red"></asp:Label>
                                <input id="hidAssociate" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="dealstatus"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="endtime"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:Button ID="hidd_btnDelete" runat="server" Style="display: none;" Text="删除时重新调用数据"
                    OnClick="hidd_btnDelete_Click" />
            </td>
        </tr>
        <tr id="trShowControlPage" runat="server">
            <td width="500" height="30">
            </td>
            <td align="right">
                <uc5:controlpagefoot id="cpCST_Issue" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnOk" runat="server" OnClick="HidButton_Click" Style="display: none;"
                    Width="0px" Height="0px" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
        ShowMessageBox="true" ShowSummary="False" />
</asp:Content>
