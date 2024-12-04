<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    EnableEventValidation="false" ValidateRequest="false" CodeBehind="frmProblemMain.aspx.cs"
    Inherits="Epower.ITSM.Web.ProbleForms.frmProblemMain" Title="问题管理" %>

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
        
        function ChangeAssociate(obj)  //关联变更单

        {
            var lngFlowID = document.getElementById(obj.id.replace("btnAssociate","hidAssociate")).value;
            window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=420&epProblem=" + lngFlowID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
            event.returnValue = false;
        }
        
        //高级查询
        function Show_SQLWhere()
	    {
	        var varSQLName = document.getElementById('<%=hidSQLName.ClientID%>').value;
	        //====zxl== 
	        //SQLName=&randomid=308&viewName=%u95EE%u9898%u5355
	        var url="../ProbleForms/frmProblem_AdvancedCondition.aspx?SQLName="+varSQLName+"&randomid="+GetRandom()+"&&viewName="+ encodeURIComponent('问题单') +"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmProbleMain";
	        window.open(url,"",'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50');
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
           if(type=='ProblemA')
           {
                type=type+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
           }
           else if(type=='ProblemB')
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
    
        //问题登单
        function showServiceConfig2(divId){
            
           var type='ProblemCountA'+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
            
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
                        result = get_all_list('ProblemA');
                        
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
         
         //问题合并
        function showServiceConfig3(divId){
            
           var type='ProblemCountA'+'|'+document.getElementById('<%=hidUserID.ClientID%>').value
            
           $.get("../AppForms/CST_Issue_List.ashx", 
                { Type: type }, 
                function(data) 
                { 
                    if(data != '')
                    {
                        document.getElementById(divId).style.display="none";
                        var flowidlist = "problemmerge" +  document.getElementById('<%=hidFlowIDList.ClientID%>').value;
                        if(document.getElementById('<%=hidFlowIDList.ClientID%>').value.trim()=="")
                        {
                            alert("请选择要合并的问题单！");
                            return;
                        }
                        if(confirm("确认合并问题单？"))
                        {
                            window.open("../Forms/OA_AddNew.aspx?flowmodelid=" + data + "&ep=" + flowidlist,"MainFrame" ,"scrollbars=yes,resizable=yes,top=0,left=0,width="+(window.availWidth-12)+",height="+(window.availHeight-35));
                        }
                    }
                    else
                    {
                        var result= false;
                        result = get_all_list('ProblemB');
                        
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
        function AddNewFlowMerge(id)  //问题单合并打开登单界面

        {
            if(document.getElementById('<%=hidFlowIDList.ClientID%>').value=="")
            {
                alert("请选择要合并的问题单！");
                return;
            }
            var isnew = 'False';
            var flowidlist = "problemmerge" +  document.getElementById('<%=hidFlowIDList.ClientID%>').value;
            var isstrExtPara = flowidlist;
            if(isnew == 'False')
            {
                if(confirm("确认合并问题单？"))
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
        function SetUrl()
        {
              
             var fromurl = '<%=FromBackUrl%>';
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
        //合并问题单，组成FLOW编号 
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
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "../AppForms/frmBr_CustomXmlHttp.aspx?Problem_ID=" + id }).responseText; } });
        }
    
    </script>

    <input type="hidden" runat="server" id="hidIsGaoji" value="0" />
    <input id="hidTable" value="" runat="server" type="hidden" />
    <input id="hidSQLName" value="" runat="server" type="hidden" />
    <input id="hidUserID" value="" runat="server" type="hidden" />
    <input type="hidden" runat="server" id="hidFlowIDList" value="" />
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
    <input id="HiddenColumn" value="" runat="server" type="hidden" />
    <table id="tbTest" width="98%" align="center" runat="server" class="listTitleNew">
        <tr id="trShowCondi" valign="top">
            <td align="left">
                <div style="display:none;">
                    <asp:DropDownList ID="DropSQLwSave" runat="server" Width="160px" AutoPostBack="True"
                        OnSelectedIndexChanged="DropSQLwSave_SelectedIndexChanged">
                    </asp:DropDownList>
                    <img class="icon" style="cursor: hand" id="Img2" onclick="Show_SQLWhere();" height="16"
                        src="../Images/ss_01.gif" width="16" title="高级条件查询" />
                </div>
                <uc1:ctrCondition ID="ctrCondition" runat="server" Visible="true" />
            </td>
        </tr>
    </table>
    <table cellpadding="0" width="98%" cellspacing="0" border="0">
        <tr id="trShowControlPre" runat="server" style="height: 10px;">
            <td height="20" colspan="2" align="right">
                <div style="width: 300px; height: 30px; text-align: right; position: relative">
                    <input id="cmdFlowList" type="button" value="创建问题" onclick="javascript:showServiceConfig2('servicClass3')"
                        class="btnClass">
                    <input id="btnMerge" type="button" value="合并问题" onclick="javascript:showServiceConfig3('servicClass3')"
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
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid EnableViewState="false" ID="dgProblem" runat="server" Width="100%"
                            AutoGenerateColumns="False" CssClass="Gridtable" OnItemCreated="dgProblem_ItemCreated"
                            OnItemDataBound="dgProblem_ItemDataBound" OnDeleteCommand="dgProblem_DeleteCommand"
                            OnItemCommand="dgProblem_ItemCommand">
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
                                <asp:BoundColumn Visible="False" DataField="Problem_ID" HeaderText="ID"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="State" HeaderText="状态"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Problem_Type" HeaderText="问题类别"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Problem_Level" HeaderText="问题级别"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="RegUserID" HeaderText="登记人"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="问题单号">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Lb_ProblemNo" Text='<%#DataBinder.Eval(Container, "DataItem.ProblemNo")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="Problem_Title" HeaderText="摘要">
                                    <HeaderStyle Width="15%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Problem_TypeName" HeaderText="问题类别">
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Problem_LevelName" HeaderText="问题级别">
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="StateName" HeaderText="状态">
                                    <HeaderStyle Width="8%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>                                
                                <asp:TemplateColumn HeaderText="处理">
                                    <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>                       
                                    <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <input id="CmdDeal" type="button" class="btnClass1" value="详情" onclick="javascript:window.location.href='../Forms/frmIssueView.aspx?FlowID=<%#DataBinder.Eval(Container.DataItem, "flowid") %>';" />
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
                                <asp:TemplateColumn HeaderText="变更">
                                    <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>                       
                                    <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Button ID="btnAssociate" runat="server" Text="变更" CommandName="Associate" SkinID="btnClass1"
                                            OnClientClick="ChangeAssociate(this);" />
                                        <asp:Label ID="lblAssociate" runat="server" Text="已变更" ForeColor="red"></asp:Label>
                                        <input id="hidAssociate" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                                    </ItemTemplate>                                    
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                <asp:Button ID="hidd_btnDelete" runat="server" Style="display: none;" Text="删除时重新调用数据"
                    OnClick="hidd_btnDelete_Click" />
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
                <asp:Button ID="btnOk" runat="server" OnClick="HidButton_Click" Width="0px" Style="display: none;"
                    Height="0px" />
                <%--zxl设置隐藏的--%>
            </td>
        </tr>
    </table>
</asp:Content>
