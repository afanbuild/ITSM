<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false" CodeBehind="frm_Req_DemandMain.aspx.cs"
    Inherits="Epower.ITSM.Web.Demand.frm_Req_DemandMain" Title="需求查询" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrDateSelectTime.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrCondition.ascx" TagName="ctrCondition" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" src="../Js/Common.js"></script>
    
    <script type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <!--Begin: 引入基础脚本库-->

    <script type="text/javascript" language="javascript" src="../js/epower.base.js"></script>

    <!--End: 引入基础脚本库-->

    <script language="javascript" type="text/javascript">
    function OpenDeleteFlow(obj)  //删除流程
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete","hidDelete")).value;
        //var	value=window.showModalDialog(,window,"dialogHeight:230px;dialogWidth:320px");
        var url = "../Common/frmFlowDelete.aspx?FlowID=" + FlowID +"&Opener_hiddenDelete=<%=hidd_btnDelete.ClientID %>&TypeFrm=cst_Issue_list";
        
        var height=($(document).height() - 230)/2 + +$(window).scrollTop();
        var width=($(document).width() - 320)/2 + +$(window).scrollLeft();              
        
        var _xy = epower.tools.computeXY('c', window, 320, 230);                
        width = _xy.x;
        height = _xy.y;
        
		open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=320px,height=230px,left=' + width +',top=' + height );
    }
    
    function ShowFind(Name)
	{
		OpenNoBarWindow2("../Demand/frm_REQ_DEMAND_Fast.aspx?CondID="+escape(Name),500,300);
	}	
	
    String.prototype.trim = function()  //去空格
	{
		// 用正则表达式将前后空格
		// 用空字符串替代。
		return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;","");
	}
    function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              //var className;
              //var objectFullName;
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
           var stype = type;
           
           var result =true;    
           if(type=='ReqDemandA')
           {
                type=type+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
           }
           else if(type=='ReqDemandB')
           {
                type=type+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
           } 

             $.ajax({
                type: "post",
                data:"type="+type,
                async:false,
                url: "../AppForms/CST_Issue_List.ashx",
                success: function(data){
                     
                    //debugger;
                                       
                    if(stype=='ReqDemandA')
                    {
                       $("#tlist1").html(data);                     
                    }
                    else if(stype=='ReqDemandB')
                    {
                       $("#tlist2").html(data); 
                    } 
                    
                    if(data=="")
                    {   
                       result =false;
                    }
		        }

             });
             
           return result;
        }
    
        //需求登单
        function showServiceConfig2(divId){
            
           var type="";
           if(divId=="servicClass3")
           {
               type='ReqDemandCountA'+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
           }
           else if(divId=="servicClass4")
           {
               type='ReqDemandCountB'+'|'+document.getElementById('<%=hidUserID.ClientID%>').value;
           } 
            
            
           $.get("../AppForms/CST_Issue_List.ashx", 
                { Type: type }, 
                function(data) 
                { 
                    if(data != '')
                    {
                        document.getElementById(divId).style.display="none";
                                                 
                       if(divId=="servicClass3")
                       {
                           window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + data,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
                       }
                       else if(divId=="servicClass4")
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
		                         
		                    var url="../Demand/frm_REQ_DEMAND_Fast.aspx?CondID="+escape(data);
		        
	                        window.location=url;
                       }                          
                         
                    }
                    else
                    {
                        var result= false;
                                                
                        if(divId=='servicClass3')
                        {
                            result = get_all_list('ReqDemandA');
                        }
                        else if(divId=='servicClass4')
                        {
                            result = get_all_list('ReqDemandB');
                        }     
                        
                        if(result==false)
                        {
                                                        
                            if(divId=='servicClass3')
                            {
                                alert("请先确保已经存在需求流程且您有启动权限!");
                            }
                            else if(divId=='servicClass4')
                            {
                                alert("请先设置相关需求请求模板!");
                            } 
                            else
                            {
                                alert("请先确保已经存在需求流程且您有启动权限！");
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
         
         function AddNewFlow(id)  //打开相应的流程登单界面
        {
            var isnew = 'False';
            var isstrExtPara = "";
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
        
        function SetUrl()
        {
              
             var fromurl = 'FromBackUrl%>';
             $.ajax({ type: "GET", async: false, url: "../AppForms/frmBr_CustomXmlHttp.aspx?formurl="+escape(fromurl) });
        }
        
        function checkAll(checkAll) {
        var len = document.forms[0].elements.length;
        var cbCount = 0;
        for (i = 0; i < len; i++) {
            if (document.forms[0].elements[i].type == "checkbox") {
                if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgProblem") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                    document.forms[0].elements[i].checked = checkAll.checked;
            
                    cbCount += 1;
                    checkFlowList(document.forms[0].elements[i]);
                }
                }
            }
        } 
        
        function checkFlowList(checkobj)
        {
            var lngFlowID = document.getElementById(checkobj.id.replace("chkDel","hidFlowID")).value;
            if(checkobj.checked)
            {
                document.getElementById('ctl00_hidID').value = document.getElementById('ctl00_hidID').value +  "," + lngFlowID;
            }
            else
            {
                document.getElementById('ctl00_hidID').value = document.getElementById('ctl00_hidID').value.replace(","+lngFlowID,"");
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
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "../AppForms/frmBr_CustomXmlHttp.aspx?DemandID=" + id }).responseText; } });
        }
    
    </script>

    <input type="hidden" runat="server" id="hidIsGaoji" value="0" />
    <input id="hidTable" value="" runat="server" type="hidden" />
    <input id="hidSQLName" value="" runat="server" type="hidden" />
    <input id="hidUserID" value="" runat="server" type="hidden" />
    <input type="hidden" runat="server" id="hidFlowIDList" value="" />
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
    <table id="tbTest" width="98%" align="center" runat="server" class="listTitleNew">
        <tr id="trShowCondi" valign="top">
            <td align="left">
                <uc1:ctrCondition ID="ctrCondition" runat="server" />
            </td>
        </tr>
    </table>
    <table cellpadding="0" width="98%" cellpadding="0" cellspacing="0" border="0">
        <tr id="trShowControlPre" runat="server" style="height: 10px;">
            <td height="20" colspan="2" align="right">
                <div style="width: 300px; height: 30px; text-align: right; position: relative">
                    <input id="cmdFlowList" type="button" value="创建需求" onclick="javascript:showServiceConfig2('servicClass3')"
                        class="btnClass">
                    <input id="cmdFastList" type="button" value="快速登单" onclick="javascript:showServiceConfig2('servicClass4')"
                        class="btnClass">
                    <div id="servicClass3" style="width: 140px; text-align: center; position: absolute;
                        z-index: 100; left: 160px; top: 20px; display: none; border: 1px solid #111;
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
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid EnableViewState="false" ID="dgProblem" runat="server" Width="100%"
                    AutoGenerateColumns="False" CssClass="Gridtable" OnDeleteCommand="dgProblem_DeleteCommand"
                    OnItemCommand="dgProblem_ItemCommand" OnItemDataBound="dgProblem_ItemDataBound"
                    OnItemCreated="dgProblem_ItemCreated">
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server" onclick="checkFlowList(this);"></asp:CheckBox>
                                <input id="hidFlowID" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DemandStatusID" HeaderText="状态"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DemandTypeID" HeaderText="需求类别"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="RegUserID" HeaderText="登记人"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="需求单号">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_ProblemNo" Text='<%#DataBinder.Eval(Container, "DataItem.DemandNo")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="DemandSubject" HeaderText="需求主题">
                            <HeaderStyle Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DemandTypeName" HeaderText="需求类别">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DemandStatus" HeaderText="需求状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" type="button" class="btnClass1" value="详情" onclick="javascript:window.location.href='../Forms/frmIssueView.aspx?FlowID=<%#DataBinder.Eval(Container.DataItem, "flowid") %>';" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="删除">
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" SkinID="btnClass1"
                                    OnClientClick="OpenDeleteFlow(this);" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <%--OnClick="hidd_btnDelete_Click"--%>
                <asp:Button ID="hidd_btnDelete" runat="server" Style="display: none;" Text="删除时重新调用数据" />
                <!--存储 TypeId, 原因: DataGrid 禁用了视图状态.-->
                <asp:HiddenField ID="hfTypeId" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc3:ControlPageFoot ID="cpProblem" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <%--OnClick="HidButton_Click"--%>
                <asp:Button ID="btnOk" runat="server" Width="0px" Style="display: none;" Height="0px" />
            </td>
        </tr>
    </table>
</asp:Content>
