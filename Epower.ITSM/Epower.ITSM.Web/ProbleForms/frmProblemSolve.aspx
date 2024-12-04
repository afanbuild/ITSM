<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmProblemSolve.aspx.cs" Inherits="Epower.ITSM.Web.ProbleForms.frmProblemSolve"
    Title="问题单登记" %>

<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/FlowForms.Master" %>
<%@ Register Src="~/Controls/Extension_DaySchemeCtrList.ascx" TagName="Extension_DayCtrList" TagPrefix="uc13" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
            body:nth-of-type(1) .other_fix_width_for_chrome
            {
            	width:100px!important;
            }                        
            
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
    

    <script type="text/javascript" language="javascript">
function TransferValue()
{
	if (typeof(document.all.<%=txtProblem_Title.ClientID%>)!="undefined" )
    { 
        parent.header.flowInfo.Subject.value = document.all.<%= txtProblem_Title.ClientID%>.value + "问题管理";
    }
}

function DoUserValidate(lngActionID,strActionName)
{
    //拼标题
    TransferValue();           
    
//    if (window.FTB_API && window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox2']){
//        var dealContentDisplayName = $('#<%=PRO_DealContent.ClientID %>').text();
//        var dealContent = window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox2'];
//       
//        dealContent.
//        
//    }
//    
//    return false;
    
    return true;//CheckCustAndType();
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
      tableCtrl = document.getElementById(className.substr(0,className.length)+"_"+TableID);
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


//------------- 
//去掉字串左边的空格 
function lTrim(str) 
{ 
    if (str.charAt(0) == " ") 
    { 
        //如果字串左边第一个字符为空格 
        str = str.slice(1);//将空格从字串中去掉 
        //这一句也可改成 str = str.substring(1, str.length); 
        str = lTrim(str); //递归调用 
    } 
    return str; 
} 

//去掉字串右边的空格 
function rTrim(str) 
{ 
    var iLength; 

    iLength = str.length; 
    if (str.charAt(iLength - 1) == " ") 
    { 
        //如果字串右边第一个字符为空格 
        str = str.slice(0, iLength - 1);//将空格从字串中去掉 
        //这一句也可改成 str = str.substring(0, iLength - 1); 
        str = rTrim(str); //递归调用 
    } 
    return str; 
} 

//去掉字串两边的空格 
function trim(str) 
{ 
    return lTrim(rTrim(str)); 
} 




function SelectSomeEqu(obj)   //选择多个设备
{
    var newDateObj = new Date();
    var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
    var name = trim(obj.value);
    if(trim(name)=="")
    {
        return;
    }
    var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value;
    var CustName = "";
    var	value=window.showModalDialog("../mydestop/frmQuickLocateEqu.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name) +"&EquipmentCatalogID="+EquipmentCatalogID,"","dialogHeight:500px;dialogWidth:600px");
    if(value != null)
    {
        if(value.length>1)
        {
            document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = value[2];   //设备名称
            document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = value[2];   //设备名称
            document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = value[1];  //设备IDontact")).id);
            document.getElementById(obj.id.replace("txtEqu","txtListName")).value = value[3];   //资产目录名称
			document.getElementById(obj.id.replace("txtEqu","hidListName")).value = value[3];   //资产目录名称
			document.getElementById(obj.id.replace("txtEqu","hidListID") ).value = value[4];  //资产目录ID                                                                         
        }
        else
        {
            document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = "";   //设备名称
            document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = "";   //设备名称
            document.getElementById(obj.id.replace("txtEqu","hidEqu")).value = "0";  //设备IDontact")).id);                                                                   
        }
    }
    else
    {
        document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = document.getElementById(obj.id.replace("txtEqu","hidEquName")).value;   //设备名称
    }
}
function GetEquID(obj)
{
    
    var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value;
    if(trim(obj.value)=="")
    {
        document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = "0";  //设备IDontact")).id);
        return;
    }
    if(trim(obj.value)==trim(document.getElementById(obj.id.replace("txtEqu","hidEquName")).value) && (trim(document.getElementById(obj.id.replace("txtEqu","hidEqu")).value)!="" || trim(document.getElementById(obj.id.replace("txtEqu","hidEqu")).value)!="0"))
    {
        return;
    }
    var CustName = "";
    if(xmlhttp == null)
    xmlhttp = CreateXmlHttpObject();       
    if(xmlhttp != null)
    {
        try
        {	
            xmlhttp.open("GET", "../MyDestop/frmXmlHttpAjax.aspx?Equ=" + escape(obj.value) +  "&randomid="+GetRandom()+"&EquipmentCatalogID="+EquipmentCatalogID, true); 
            xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
            xmlhttp.onreadystatechange = function() 
							            { 
								            if ( xmlhttp.readyState==4 ) 
								            { 								               
									            if(xmlhttp.responseText=="-1")  //没有
									            {
									              //  document.getElementById(obj.id.replace("txtEqu","hidEqu")).value.trim()!="0"
									                alert("此资产不存在，请重新查找！"); 
									                document.getElementById(obj.id.replace("txtEqu","hidEqu")).value=0;
									                obj.value="";
								                }
								                else if(xmlhttp.responseText=="0") //找到多个
								                {
								                    SelectSomeEqu(obj);
								                }
								                else  //找到唯一
								                {
        									        
								                    var json= eval("("+xmlhttp.responseText+")");
                                                    var record=json.record;
                                                    
                                                    for(var i=0; i < record.length; i++)
                                                    {
								                        document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = record[i].name;   //设备名称
                                                        document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = record[i].name;   //设备名称
                                                        document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = record[i].id;  //设备IDontact")).id);			                                                    			                                                    			                                                   
                                                        document.getElementById(obj.id.replace("txtEqu","txtListName")).value = record[i].listname;   //资产目录名称
			                                            document.getElementById(obj.id.replace("txtEqu","hidListName")).value = record[i].listname;   //资产目录名称
			                                            document.getElementById(obj.id.replace("txtEqu","hidListID") ).value = record[i].listid;  //资产目录ID
                                                    }
								                }
								            } 
							            } 
            xmlhttp.send(null); 
        }catch(e3){}
    }
}

//设备
function SelectEqu(obj) 
{
    var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value; 			        
    var EquName = document.all.<%=txtEqu.ClientID%>.value;
    var CustName = "";
    var MastCust = "";
    //===============zxl==
    var url="../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&randomid="+GetRandom()+"&FlowID="+ '<%=FlowID%>' + "&EquName=" + escape(EquName) + "&Cust=" + escape(CustName) + "&MastCust=" + escape(MastCust)+"&EquipmentCatalogID="+EquipmentCatalogID+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmProblemSolve";
    window.open(url,"","toolbar=no,menubar=no,status=yes,resizable=yes,tilebar=yes,scrollbars=yes,width=800,height=600px,left=150,top=50");
}


//选择资产目录
function SelectListName(obj)
{
    var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskCateListSel.aspx?random=" + GetRandom(),"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
    if(value != null)
    {
        if(value.length>1)
        {			                
            document.getElementById(obj.id.replace("cmdListName","txtListName")).value = value[1];   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListName")).value = value[1];   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = value[0];  //资产目录ID
        }
        else
        {			                
            document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";  //资产目录ID
        }
    }
    else
    {			                
            document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";   //资产目录名称
            document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";  //资产目录ID
    }
}


//案例分析
function DoItemQuestionAnalysis(lngAppID,lngFlowID)
{
    window.open("../ProbleForms/frmPro_ProblemAnalyseMain.aspx?AppID=" + lngAppID + "&FlowID=" + lngFlowID,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=500");
}
//知识归档
function DoKmAdd(lngMessageID,lngAppID,lngFlowID)
{
    window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=400&ep=" + lngMessageID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
}

var selectobj = document.getElementById("name0");
    
function chang(obj,img){
   if(obj!=selectobj)	    
		 obj.background="../Images/"+img;
}

function chang_Class(name,num,my)
{
    for(i=0;i<num;i++){
        if(i!=my){
            document.getElementById(name+i).className="STYLE4";
            //document.getElementById(name+i).background="../Images/lm-a.gif";
            $("#" + name + i).css("background-image", "url(../Images/lm-a.gif)");
            document.getElementById("a"+i).className="STYLE4";
        }
    }
	selectobj = document.getElementById(name+my);
	document.getElementById(name+my).className="td_3";	
	//document.getElementById(name+my).background="../Images/lm-b.gif";
	$("#" + name + my).css("background-image", "url(../Images/lm-b.gif)");
	document.getElementById("a"+my).className="td_3";

    switch(my)
    {
        case 0:     //处理信息
            document.getElementById("Tables0").style.display ="";
            document.getElementById("Tables1").style.display ="none";
            document.getElementById("Tables2").style.display ="none";
            document.getElementById("Tables3").style.display ="none";
            document.getElementById("Tables4").style.display ="none";
            document.getElementById("Tables5").style.display ="none";
             if (window.FTB_API && window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox1']){
                window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox1'].GoToDesignMode();
            }
             if (window.FTB_API && window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox2']){
                window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox2'].GoToDesignMode();
            }
            break;
            
        case 1:    //相关事件
            document.getElementById("Tables0").style.display ="none";
            document.getElementById("Tables1").style.display ="";
            document.getElementById("Tables2").style.display ="none";
            document.getElementById("Tables3").style.display ="none";
            document.getElementById("Tables4").style.display ="none";
            document.getElementById("Tables5").style.display ="none";
        break;
        case 2:    //知识参考
            document.getElementById("Tables0").style.display ="none";
            document.getElementById("Tables1").style.display ="none";
            document.getElementById("Tables2").style.display ="";
            document.getElementById("Tables3").style.display ="none";
            document.getElementById("Tables4").style.display ="none";
            document.getElementById("Tables5").style.display ="none";
            Iframe2.location="../InformationManager/frmInf_InformationMain.aspx?IsSelect=1"
        break;        
        case 3:    //相关变更

            document.getElementById("Tables0").style.display ="none";
            document.getElementById("Tables1").style.display ="none";
            document.getElementById("Tables2").style.display ="none";
            document.getElementById("Tables3").style.display ="";
            document.getElementById("Tables4").style.display ="none";
            document.getElementById("Tables5").style.display ="none";
            if(Iframe3.location =="about:blank")
	        {
                Iframe3.location="../EquipmentManager/frmShowEquRel.aspx?EquID=" + '<%=ChangeFlowID%>' + "&randomid="+GetRandom()+"&Type=7";   
            }
            break;
         case 4:    //相关问题
            document.getElementById("Tables0").style.display ="none";
            document.getElementById("Tables1").style.display ="none";
            document.getElementById("Tables2").style.display ="none";
            document.getElementById("Tables3").style.display ="none";
            document.getElementById("Tables4").style.display ="";
            document.getElementById("Tables5").style.display ="none";
        break;
        case 5:    //相关问题于
            document.getElementById("Tables0").style.display ="none";
            document.getElementById("Tables1").style.display ="none";
            document.getElementById("Tables2").style.display ="none";
            document.getElementById("Tables3").style.display ="none";
            document.getElementById("Tables4").style.display ="none";
            document.getElementById("Tables5").style.display ="";
        break;
     }
}

function checkAll(checkAll) {
        var len = document.forms[0].elements.length;
        var cbCount = 0;
        for (i = 0; i < len; i++) {
            if (document.forms[0].elements[i].type == "checkbox") {
                if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgProblemSub") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                    document.forms[0].elements[i].checked = checkAll.checked;
            
                    cbCount += 1;
                    checkFlowList(document.forms[0].elements[i]);
                }
                }
            }
        } 
    </script>
     <input id="hidAppID" type="hidden" runat="server" value="0" />
    
    
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <table id="Table11" width="100%" align="center" runat="server" class="listNewContent">
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
    <table class="listContent" width="100%" align="center" runat="server" id="Table1"
         border="0" cellSpacing="1" cellPadding="1">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="PRO_ProblemNo" runat="server" Text="问题单号"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="lblBuildCode" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblProblemNo" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="PRO_RegUserName" runat="server" Text="登单人"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labRegUserName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_RegDeptName" runat="server" Text="登单人部门"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labRegDeptName" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_RegTime" runat="server" Text="登单时间"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labRegTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr style="display: none;">
            <td class="listTitleRight">
                <asp:Literal ID="PRO_EquList" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblListName" runat="server" Visible="false"></asp:Label>
                <asp:TextBox ID="txtListName" runat="server" ReadOnly="true"></asp:TextBox>
                <input id="cmdListName" onclick="SelectListName(this)" type="button" value="..."
                    runat="server" name="cmdListName" class="btnClass2" />
                <input id="hidListName" value="" type="hidden" runat="server" />
                <input id="hidListID" value="0" type="hidden" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_ProblemTypeName" runat="server" Text="问题类别"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropListNew ID="CataProblemType" runat="server" RootID="1006" MustInput="true"
                    TextToolTip="问题类别" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_ProblemLevelName" runat="server" Text="问题级别"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropListNew ID="CataProblemLevel" runat="server" RootID="1007" TextToolTip="问题级别" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_EffectName" runat="server" Text="影响度"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropListNew ID="CtrFCDEffect" runat="server" RootID="1028" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_InstancyName" runat="server" Text="紧急度"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropListNew ID="CtrFCDInstancy" runat="server" RootID="1029" />
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight">
                <asp:Literal ID="PRO_EquName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblEqu" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtEqu" runat="server" onblur="GetEquID(this)"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
                <input id="hidEquName" type="hidden" runat="server" />
                <input id="hidEqu" type="hidden" runat="server" value="-1" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_StateName" runat="server" Text="问题状态"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropListNew ID="CtrDealState" runat="server" RootID="1021" TextToolTip="问题状态"
                    MustInput="true" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_Title" runat="server" Text="摘要"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <uc1:CtrFlowFormText ID="txtProblem_Title" runat="server" TextToolTip="摘要" MustInput="true"
                    Width="80%" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="PRO_Subject" runat="server" Text="问题描述"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <uc1:CtrFlowFormText ID="txtProblem_Subject" runat="server" Row="4" TextMode="MultiLine"
                    TextToolTip="问题描述" MustInput="true" Width="90%" />
            </td>
        </tr>
    </table>
    <table height="29" border="0" cellpadding="0" cellspacing="0">
        <tr style="cursor: hand">
            <td width="7"><img src="../Images/lm-left.gif" width="7" height="29" /></td>
            <td width="95" height="29" id="name0" onclick="chang_Class('name',6,0)" align="center" valign="middle" class="td_3" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')" background="../Images/lm-b.gif"><span id="a0" class="td_3">处理信息</span></td>
            <td width="95" height="29" id="name2" onclick="chang_Class('name',6,2)" align="center" class="STYLE4" valign="middle" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')"><span id="a2" class="STYLE4">知识参考</span></td>
            <td width="95" height="29" id="name1" onclick="chang_Class('name',6,1)" align="center" class="STYLE4" valign="middle" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a1" class="STYLE4">相关事件</span></td>
            <td width="95" height="29" id="name3" onclick="chang_Class('name',6,3)" align="center" class="STYLE4" valign="middle" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a3" class="STYLE4">相关变更</span></td>
            <td width="95" height="29" id="name4" onclick="chang_Class('name',6,4)" align="center" class="STYLE4" valign="middle" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a4" class="STYLE4">合并问题</span></td>
            <td width="95" height="29" id="name5" onclick="chang_Class('name',6,5)" align="center" class="STYLE4" valign="middle" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a5" class="STYLE4">合并问题于</span></td>
            <td width="7"><img src="../Images/lm-right.gif" width="7" height="29" /></td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <table id="Tables0" class="listContent" width="100%" align="center"  border="0" cellSpacing="1" cellPadding="1">
                    <tr>
                        <td align="left" class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="PRO_Remark" runat="server" Text="解决方案"></asp:Literal>
                        </td>
                        <td class="list" colspan="3" style="width:896px;">
                            <ftb:FreeTextBox ID="freeTextBox1" StartMode="HtmlMode" runat="server" Width="100%" Height="100px" ButtonPath="../Forms/images/epower/officexp/"
                                ImageGalleryPath="Attfiles\\Photos">
                            </ftb:FreeTextBox>
                            <asp:Label ID="labRemark" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="PRO_DealContent" runat="server" Text="原因分析"></asp:Literal>
                        </td>
                        <td class="list">
                            <ftb:FreeTextBox ID="freeTextBox2" StartMode="HtmlMode" runat="server" Width="100%" Height="100px" ButtonPath="../Forms/images/epower/officexp/"
                                ImageGalleryPath="Attfiles\\Photos">
                            </ftb:FreeTextBox>                            
                            <asp:Label ID="LabDealContent" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="list">
                            <uc13:Extension_DayCtrList ID="Extension_DayCtrList1" runat="server" />
                        </td>
                    </tr>
                </table>
                <table id="Tables2" class="listContent" width="100%" align="center" style="display: none" border="0" cellSpacing="1" cellPadding="1">
                    <tr>
                        <td align="right" class="listTitleRight">
                            <iframe id='Iframe2' name="Iframe2" src="" width='100%' height='auto' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables1" class="listContent" width="100%" align="center" height="100px"
                    style="display: none">
                    <tr>
                        <td align="left" class="list" colspan="3" width="*" valign="top">
                            <asp:DataGrid ID="gridAttention" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable fixed-grid-border2"  EnableViewState="True"
                                Width="100%" OnItemDataBound="gridAttention_ItemDataBound"  border="0" cellSpacing="1" cellPadding="1">
                                <Columns>
                                    <asp:BoundColumn DataField="Status" HeaderText="Status" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="flowid" HeaderText="flowid" ReadOnly="True" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ServiceNo" HeaderText="事件单号">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ServiceType" HeaderText="事件类别">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="scale" HeaderText="关联权重" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Effect" HeaderText="关联影响度" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="stress" HeaderText="关联紧迫性" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="subject" HeaderText="摘要"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DealStatus" HeaderText="事件状态">
                                        <HeaderStyle Width="70px" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="流程状态">
                                        <HeaderStyle Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                            <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="详情">
                                        <HeaderStyle Width="5%" />
                                        <ItemTemplate>
                                            <input id="Button4" runat="server" name="CmdDeal" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                                type="button" value="详情" class="btnClass1">
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" Mode="NumericPages" />
                            </asp:DataGrid>
                            <asp:Label ID="labScale" runat="server" ForeColor="Red"></asp:Label>
                            &nbsp; &nbsp;<asp:Label ID="labEffect" runat="server" ForeColor="Red"></asp:Label>
                            &nbsp;
                            <asp:Label ID="labStress" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="Tables3" width="100%" align="center" class="listContent" style="display: none;" border="0" cellSpacing="1" cellPadding="1">
                    <tr>
                        <td align="right" class="listTitleRight">
                            <iframe id='Iframe3' name="Iframe3" src="" width='100%' height="100px" scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables4" class="listContent" width="100%" align="center" height="100px"
                    style="display: none">
                    <tr>
                        <td align="left" class="list" colspan="3" width="*" valign="top">
                            <asp:DataGrid ID="dgProblemSub" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable fixed-grid-border2" 
                                OnItemDataBound="dgProblem_ItemDataBound"  border="0" cellSpacing="1" cellPadding="1">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="结束问题单">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ServiceNo" HeaderText="问题单号">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Problem_TypeName" HeaderText="问题类别">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Problem_Title" HeaderText="摘要"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="StateName" HeaderText="问题状态"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="流程状态">
                                        <HeaderStyle Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                             <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="详情">
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                                type="button" value='详情' runat="server">
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FlowDealState"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <table id="Tables5" class="listContent" width="100%" align="center" height="100px"
                    style="display: none">
                    <tr>
                        <td align="left" class="list" colspan="3" width="*" valign="top">
                            <asp:DataGrid ID="dgProblemSubRel" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable fixed-grid-border2" 
                                OnItemDataBound="dgProblemSubRel_ItemDataBound"  border="0" cellSpacing="1" cellPadding="1">
                                <Columns>
                                    <asp:BoundColumn DataField="ServiceNo" HeaderText="问题单号">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Problem_TypeName" HeaderText="问题类别">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Problem_Title" HeaderText="摘要"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="StateName" HeaderText="问题状态"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="流程状态">
                                        <HeaderStyle Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                             <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="详情">
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                                type="button" value='详情' runat="server">
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input id="hidServiceTitle" runat="server" type="hidden" />
    <input id="hidFlowID" runat="server" type="hidden" value=" 0" />

    <script type="Text/javascript" language="javascript">
        var temp = 0;
        for (i = 0; i < 6; i++) {
            if (i != temp) {
                document.getElementById("name" + i).className = "STYLE4";
                document.getElementById("name" + i).background = "../Images/lm-a.gif";
                document.getElementById("a" + i).className = "STYLE4";
                document.getElementById("name" + i).style.display = "";
            }
        }
        document.getElementById("name" + temp).className = "";
        document.getElementById("name" + temp).background = "../Images/lm-b.gif";

        if ('<%=IssuesForProblem%>' == "") {
            document.getElementById("name1").style.display = "none";
        }
        if ('<%=ChangeFlowID%>' == "") {
            document.getElementById("name3").style.display = "none";
        }
        if ('<%=ProblemSub%>' == "") {
            document.getElementById("name4").style.display = "none";
        }
        if ('<%=ProblemSubRel%>' == "") {
            document.getElementById("name5").style.display = "none";
        }

    $(document).ready(function(){       
       setTimeout("chang_Class('name',6,0);", 1000);
              
    });

    </script>

</asp:Content>
