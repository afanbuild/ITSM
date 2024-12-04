<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    CodeBehind="frm_ChangeBase.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_ChangeBase"
    Title="变更登记" %>

<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc11" %>
 <%@ Register Src="~/Controls/Extension_DaySchemeCtrList.ascx" TagName="Extension_DayCtrList" 
  TagPrefix="uc13"%>
<%@ Register src="../Controls/UserPicker.ascx" tagname="UserPicker" tagprefix="uc3" %>
<%@ Register Src="../Controls/CtrCataSelectShowInpText.ascx" TagName="CtrCataSelectShowInpText"
    TagPrefix="uc5" %>
<%@ Register src="../Controls/TrueOrFalseShowInpText2.ascx" tagname="TrueOrFalseShowInpText2" tagprefix="uc8" %>

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
    
    <script type="text/javascript" src="../Js/jquery.autocomplete.js"></script>

    <script language="javascript">
 function TransferValue()
{
    //拼凑待办事项标题
   if (typeof(document.all.<%=CtrFlowFTSubject.ClientID%>)!="undefined" )
	  parent.header.flowInfo.Subject.value = document.all.<%=CtrFlowFTSubject.ClientID%>.value ;
}
 function DoUserValidate(lngActionID,strActionName)
{
    TransferValue();
    if(IsEquEmpty()){
        return ValiDateCataText();
    }else{
        return false;
    }
}

function ValiDateCataText()
    {
        var flag = true;
        var str = "";
        var CtrChangeWindow = document.getElementById("ctl00_ContentPlaceHolder1_CtrChangeWindow_txtContent");
        var CtrStopServer = document.getElementById("ctl00_ContentPlaceHolder1_CtrStopServer_txtContent");
        var CtrBusEffect = document.getElementById("ctl00_ContentPlaceHolder1_CtrBusEffect_txtContent");
        var CtrDataEffect = document.getElementById("ctl00_ContentPlaceHolder1_CtrDataEffect_txtContent");
        
        var TrChangeWindow = document.getElementById("ctl00_ContentPlaceHolder1_CtrChangeWindow_trContent");
        var TrStopServer = document.getElementById("ctl00_ContentPlaceHolder1_CtrStopServer_trContent");
        var TrBusEffect = document.getElementById("ctl00_ContentPlaceHolder1_CtrBusEffect_trContent");
        var TrDataEffect = document.getElementById("ctl00_ContentPlaceHolder1_CtrDataEffect_trContent");
   
        
        var chk = document.getElementById("<%=chkChangePlace.ClientID%>");
        if($("#<%=chkChangePlace.ClientID%> input:checked").size()==0&&$("#<%=labChangePlace.ClientID%>").text()=="" && chk !="undefine" && chk!=null)
        {
            if(str != "")
                str += ",";
            str += "变更场所";
            flag = false;
        }
        if(CtrChangeWindow != null && TrChangeWindow.style.display !="none")
        {
            if(CtrChangeWindow.value=="" || CtrChangeWindow.value=="变更窗口说明")
            {
                str += "变更窗口说明";
                flag = false;
            }
        }
        if(CtrStopServer != null && TrStopServer.style.display !="none")
        {
            if(CtrStopServer.value=="" || CtrStopServer.value=="停用服务说明")
            {
                if(str != "")
                    str += ",";
                str += "停用服务说明";
                flag = false;
            }
        }
        if(CtrBusEffect != null && TrBusEffect.style.display !="none")
        {
            if(CtrBusEffect.value=="" || CtrBusEffect.value=="业务影响说明")
            {
                if(str != "")
                    str += ",";
                str += "业务影响说明";
                flag = false;
            }
        }
        if(CtrDataEffect != null && TrDataEffect.style.display !="none")
        {
            if(CtrDataEffect.value=="" || CtrDataEffect.value=="数据影响说明")
            {
                if(str != "")
                    str += ",";
                str += "数据影响说明";
                flag = false;
            }
        }
        if($("#<%=ddlIsplan.ClientID%>").val()=="-1")
        {
            if(str != "")
                str += ",";
            str += "应急/回退方案";
            flag = false;
        }
        if($("#<%=ddlIsplan.ClientID%>").val()=="1"&&$("#ctl00_ContentPlaceHolder1_ctrIsplan_txtText").val()=="")
        {
            if(str != "")
                str += ",";
            str += "应急/回退方案说明";
            flag = false;
        }
        if(flag == false)
        {
            str +=";不能为空！"
            alert(str);
        }
        return flag;
    }

  //知识归档
    function DoKmAdd(lngMessageID,lngAppID,lngFlowID)
    {
         window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=400&randomid="+GetRandom()+"&ep=" + lngMessageID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
    }
//判断资产是否为空
    function IsEquEmpty()
    {
        var varRet = false;
        var dgChange = document.getElementById('<%=gvBillItem.ClientID %>');
        
        if(dgChange)
        {
            //获取Footer模板的值

            var m = dgChange.rows.length;
            if(m < 10)
                m = "0" + m;
            
            if(document.getElementById("ctl00_ContentPlaceHolder1_gvBillItem_ctl" + m + "_txtAddListName") != undefined && document.getElementById("ctl00_ContentPlaceHolder1_gvBillItem_ctl" + m + "_txtAddEquName") != undefined)
            {
                var EquList2 = document.getElementById("ctl00_ContentPlaceHolder1_gvBillItem_ctl" + m + "_txtAddListName").value;       //资产目录
                var EquName2 = document.getElementById("ctl00_ContentPlaceHolder1_gvBillItem_ctl" + m + "_txtAddEquName").value;           //资产名称
                if(m == 2)
                {
                    if(EquList2 == "" && EquName2 == "")
                    {
                        alert('资产信息不能为空');                     
                        varRet = false;  
                    }
                    else if(EquList2 == "" || EquName2 == "")
                    {
                        alert('资产目录和资产名称必须同时不为空');
                        varRet = false;        
                    }
                    else
                    {
                        varRet = true;
                    }
                }
                else 
                {
                    if(EquList2 == "" && EquName2 == "")
                    {
                        varRet = true;
                    }
                    else if(EquList2 == "" || EquName2 == "")
                    {
                        alert('资产目录和资产名称必须同时不为空');
                        varRet = false;        
                    }
                }
            }
            else
            {
                varRet = true;
            }
            
            for(var j=0;j<dgChange.rows.length - 2;j++)
            {
                var idSum = parseInt(j) + parseInt(2);
                if(idSum < 10)
                idSum = "0" + idSum;
                
                if(document.getElementById("ctl00_ContentPlaceHolder1_gvBillItem_ctl" + idSum + "_txtListName") != undefined && document.getElementById("ctl00_ContentPlaceHolder1_gvBillItem_ctl" + idSum + "_txtEquName") != undefined)
                {
                    var EquList = document.getElementById("ctl00_ContentPlaceHolder1_gvBillItem_ctl" + idSum + "_txtListName").value;       //资产目录
                    var EquName = document.getElementById("ctl00_ContentPlaceHolder1_gvBillItem_ctl" + idSum + "_txtEquName").value;           //资产名称
                    if(j == 0 && (EquList == "" && EquName == ""))
                    {
                         alert('资产信息不能为空');                     
                         varRet = false;                         
                         break;                    
                    } 
                    else if(j == 0 && (EquList == "" || EquName == ""))
                    {
                        alert('资产目录和资产名称必须同时不为空');
                        varRet = false;
                        break;
                    }
                    else if(j > 0)
                    {
                        if(EquList == "" || EquName == "")
                        {
                             alert('资产目录和资产名称必须同时不为空');
                             varRet = false;                         
                             break;
                        }
                    }
                    else
                    {
                        varRet = true;
                    }
                }
                else
                {
                    varRet = true;
                }
            }
        }
        else
        {
            varRet = true;
        }
        return varRet;
    }


String.prototype.trim = function()  //去空格

{
	return this.replace(/(^\s*)|(\s*$)/g, "");
}


 //打开服务服务级别
function OpenServiceProtocol()
{

    if((document.all.<%=hidCustID.ClientID%>.value=="-1" || document.all.<%=hidCustID.ClientID%>.value=='0'))
    {
        alert("没有选择客户或资产！");
        event.returnValue = false;
    }
    else
    {
        window.open("../AppForms/frmServiceProtocolShow.aspx?ID=" + document.all.<%=hidCustID.ClientID%>.value,"","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
    }

    event.returnValue = false;
}


//客户选择
function CustomSelect(obj) 
{
    var CustName = document.all.<%=txtCustAddr.ClientID%>.value;
    var  url ="../Common/frmDRMUserSelectajax.aspx?IsSelect=true&CustID=" + document.getElementById(obj.id.replace('cmdCust','hidCustID')).value 
                + "&randomid=" + GetRandom() + "&FlowID="+ '<%=FlowID%>' 
                + "&CustName=" + escape(CustName)
                + "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>" 
                + "&PageType=3";
    open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50');
}

function SelectSomeCust(obj)   //选择多个客户
{
    var newDateObj = new Date()
    var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
    var name = obj.value.trim();
    if(name.trim()=="")
    {
        return;
    }
	var	value=window.showModalDialog("../mydestop/frmQuickLocateCustAjax.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name),"","dialogHeight:500px;dialogWidth:900px");
	if(value != null)
	{
		            var json = value;
				    var record=json.record;
				    
					for(var i=0; i < record.length; i++)
					{ 
			            document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = record[i].shortname;   //客户
			            document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = record[i].shortname;
			            document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = record[i].address;   //地址
			            document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = record[i].address;   //地址
			            document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = record[i].linkman1;    //联系人



			            document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = record[i].linkman1;   
			            document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = record[i].tel1;   //联系人电话



			            document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = record[i].tel1;
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = record[i].id;    //客户ID号


			           
			            document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = record[i].custdeptname;   //所属部门

		                document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = record[i].email;  //电子邮件
		                document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = record[i].mastcustname;   //服务单位
		                document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = record[i].custdeptname;   //所属部门

		                document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = record[i].email;  //电子邮件
		                document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = record[i].mastcustname;   //服务单位
		                document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = record[i].job;   //职位
		                document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = record[i].job;   //职位			            			           
					}
	}
	else
	{
				     document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = "0";    //客户ID号	
			         document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr") ).value="";//客户名称	
			                                                
			         document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = "";
			         document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = "";   //地址
			         document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = "";   //地址
			         document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = "";    //联系人

			         document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = "";   
			         document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = "";   //联系人电话

			         document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = "";			                                                            
			         document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = "";   //所属部门

		             document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = "";  //电子邮件
		             document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = "";   //服务单位
		             document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = "";   //所属部门

		             document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = "";  //电子邮件
		             document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = "";   //服务单位
		             document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = "";   //职位
		             document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = "";   //职位
	}
}

var xmlhttp = null;
function CreateXmlHttpObject()
{
try  
{  
	xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");  
}  
catch(e)  
{  
	try  
	{  
		xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");  
	}  
	catch(e2){}  
}
return xmlhttp;
}

function GetCustID(obj)
{
if(obj.value.trim()=="")
    {
	                document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="0";
	                document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr") ).value="";//客户名称	
			        document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = "";
			        document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = "";   //地址
			        document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = "";   //地址
			        document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = "";    //联系人

			        document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = "";   
			        document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = "";   //联系人电话

			        document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = "";			                                                            
			        document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = "";   //所属部门

		            document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = "";  //电子邮件
		            document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = "";   //服务单位
		            document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = "";   //所属部门

		            document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = "";  //电子邮件
		            document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = "";   //服务单位
		            document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = "";   //职位
		            document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = "";   //职位
        return;
    }
if(obj.value.trim()==document.getElementById(obj.id.replace("txtCustAddr","hidCust")).value.trim() && (document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="" || document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="0"))
    {
        return;
    }
    //==zxl==
     $.ajax({
        url:"../MyDestop/frmXmlHttpAjax.aspx?Cust=" + escape(obj.value),
        datatype:"json",
        type:'GET',
        success:function(data)
        {
            if(data =="-1")
            {
                alert("此客户不存在！");
                document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = "0";    //客户ID号	
			    document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr") ).value="";//客户名称	
			                                                
			    document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = "";
			    document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = "";   //地址
			    document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = "";   //地址
			    document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = "";    //联系人

                document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = "";   
                document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = "";   //联系人电话

                document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = "";			                                                            
                document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = "";   //所属部门

                document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = "";  //电子邮件
                document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = "";   //服务单位
                document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = "";   //所属部门

                document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = "";  //电子邮件
                document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = "";   //服务单位
                document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = "";   //职位
                document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = "";   //职位
                
            }
           else if(data=="0") //找到多个
            {
                    if($.browser.safari)
		            {
		                alert("多个客户名称相同，请到旁边选项中选择！");
		                return;
		            }
                    SelectSomeCust(obj);
                
                
            }
            else{
                  var json =eval("("+data+")");
                  var record=json.record; 
                  
                  
                 for(var i=0; i < record.length; i++)
                  {
                        document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = record[i].shortname;   //客户
                        document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = record[i].shortname;
                        document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = record[i].address;   //地址
                        document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = record[i].address;   //地址
                        document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = record[i].linkman1;    //联系人



                        document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = record[i].linkman1;   
                        document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = record[i].tel1;   //联系人电话



                        document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = record[i].tel1;
                        document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = record[i].id;    //客户ID号


			            
                        document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = record[i].custdeptname;   //所属部门

                        document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = record[i].email;  //电子邮件
                        document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = record[i].mastcustname;   //服务单位
                        document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = record[i].custdeptname;   //所属部门

                        document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = record[i].email;  //电子邮件
                        document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = record[i].mastcustname;   //服务单位
                        document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = record[i].job;   //职位
                        document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = record[i].job;   //职位                                                			            			                                                                
                  }
               }
        },
        error:function()
        {
            alert("失败！");  
        }
        
        
     });
}       

function ShowTable(imgCtrl)
{
      var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
      var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
      var TableID = imgCtrl.id.replace("Img","Table");
      var className;
      var objectFullName;
      var tableCtrl;
      objectFullName = <%=ValidationSummary1.ClientID%>.id;
      className = objectFullName.substring(0,objectFullName.indexOf("ValidationSummary1")-1);
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

//资产变更
function Change_confirm(obj)
{
    var equid = document.getElementById(obj.id.replace("btnChange","hidID") ).value;
    var changeid=document.all.<%=hidChangeId.ClientID %>.value;//变更ID
    if((equid=="" || equid=="0"))
    {
        alert("请先选择资产！");
        return false;
    }
    
    /*     
     * Date: 2012-8-6 14:26
     * summary: 在 URL 中取消息ID.
     * modified: sunshaozong@gmail.com     
     */        
    try{
        var msg_id = epower.tools.qstr.get('MessageID');                    
    }catch(e){ alert(e.message); }
    
  //========================
   $.ajax({
                url:"../MyDestop/frmXmlHttp.aspx?AssetLockId=" + escape(equid)+"&ChangeId="+escape(changeid)+"&Random=" + GetRandom(),
                datatype: "json",
                type: 'GET', 
                 success: function (data)
                  {
                   
                    var flag=true;
                     if(data =="当前变更锁定" || data =="没有锁定")
                     {
                    //===============
                                if(data=="没有锁定")
                                {
                                    if(confirm("是否锁定资产？"))
                                    {
                                        flag=true;
                                    }else
                                    {
                                        flag=false;
                                    }
                                }
                               if(flag==true)
                                {
                                
                                    $.ajax({
                                        url:"../MyDestop/frmXmlHttp.aspx?EquId=" + escape(equid)+"&ChangeId="+escape(changeid)+"&Random=" + GetRandom(),
                                        datatype: "json",
                                        type: 'GET', 
                                         success: function (data)
                                          {
                                             if(data =="1")
                                             {
                                           //  alert("aaa");
                                             var url="frmEqu_DeskEdit.aspx?ChangeBillFlowID=" + <%=FlowID %> + "&id=" + equid + "&subjectid=1&IsChange=True&IsChEdit=1&randomid=" + GetRandom()+"&TypeFrm=frm_ChangeBase&FlowModelIDChangeBase=<%=FlowModelID %>";
                                             
                                            /*     
                                             * Date: 2012-8-6 14:26
                                             * summary: 在跳转时, 为 url 追加消息ID的参数

                                             * modified: sunshaozong@gmail.com     
                                             */           
                                             if (msg_id) {                                          
                                                url = url + '&MessageID=' + msg_id;
                                             }
                                             
                                            /*     
                                             * Date: 2013-07-04 09:50
                                             * summary: 在跳转时, 为 url 追加from_change_base的参数                                                                                          from_change_base: 表示是从 变更单中 > 资产信息 > 变更配置项 按钮跳转的.                                             from_change_base 的合法值: yes

                                             * modified: sunshaozong@gmail.com     
                                             */                                                        
                                             url = url + '&from_change_base=yes';
                                             
                                              window.location=url;
                                              
                                             }
                                          }
                                         
                                    });
                                }
                     
                      //=======
                       
                     }
                      else
                     {
                        alert(data);
                     }
                  }
                 
            });
}

//资产是否锁定
function CheckAssetLock(equid,changeid)
{
    var result="";
    
    window.open("frmEqu_DeskEdit.aspx?ChangeBillFlowID=" + <%=FlowID %> + "&id=" + equid + "&subjectid=1&IsChange=True&IsChEdit=1&randomid=" + GetRandom(),"","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
}

function FareSelect(obj,type) 
{
    var typeid = "",typename="";

    if(type=="1")//类型为1表示修改hidAddTypeID
    {
    
        if(typeof(document.getElementById(obj.id.replace("cmdEqu","hidListID")))!="undefined")
        {
            //typeid = document.getElementById(obj.id.replace("cmdEqu","hidListID")).value;
        }
    }
    else
    {
        if(typeof(document.getElementById(obj.id.replace("cmdAddEqu","hidAddListID")))!="undefined")
        {
            //typeid = document.getElementById(obj.id.replace("cmdAddEqu","hidAddListID")).value;
        }				        
    } 
    
}
function FareSelect1(obj,type) 
{
	var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskCateListSel.aspx?random=" + GetRandom(),"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
	if(typeof(value)!="undefined"){
	    if(value != null)
	    {
	        if(type=="1")   //类型为1表示修改
	        {
	            document.getElementById(obj.id.replace("cmdListName","hidListID")).value = value[0];  //资产目录ID
                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = value[1];   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListName") ).value = value[1];   //资产目录名称	
            }
            else
            {
	            document.getElementById(obj.id.replace("cmdAddListName","hidAddListID")).value = value[0];  //资产目录ID
                document.getElementById(obj.id.replace("cmdAddListName","txtAddListName")).value = value[1];   //资产目录名称
                document.getElementById(obj.id.replace("cmdAddListName","hidAddListName") ).value = value[1];   //资产目录名称
            }
	    }
	    else
	    {
	        if(type=="1")   //类型为1表示修改
	        {
	            document.getElementById(obj.id.replace("cmdListName","hidListID")).value = "0";   //资产目录ID
                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListName") ).value = "";  //资产目录名称	
            }
            else
            {
	            document.getElementById(obj.id.replace("cmdAddListName","hidAddListID")).value = "0";   //资产目录ID
                document.getElementById(obj.id.replace("cmdAddListName","txtAddListName")).value = "";   //资产目录名称
                document.getElementById(obj.id.replace("cmdAddListName","hidAddListName") ).value = "";  //资产目录名称
            }
	    }
	}
	else
	{
	    if(type=="1")   //类型为1表示修改
	        {
	            document.getElementById(obj.id.replace("cmdListName","hidListID")).value = "0";   //资产目录ID
                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";   //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListName") ).value = "";  //资产目录名称	
            }
            else
            {
	            document.getElementById(obj.id.replace("cmdAddListName","hidAddListID")).value = "0";   //资产目录ID
                document.getElementById(obj.id.replace("cmdAddListName","txtAddListName")).value = "";   //资产目录名称
                document.getElementById(obj.id.replace("cmdAddListName","hidAddListName") ).value = "";  //资产目录名称
            }
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

//影响度分析

function ImpactAnalysis(equId)
{
     window.open("frmEqu_ImpactAnalysis.aspx?EquId=" + equId + "&ChangeBillFlowID=" + <%=FlowID %>+"&randomid=" + GetRandom(),"","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
    return true; //return false;
}
var selectobj = document.getElementById("name0");

function chang(obj,img){
   if(obj!=selectobj)	    
	    obj.background="../Images/"+img;
}
        
function chang_Class(name,num,my)
{
    for(i = 0;i < num; i++)
    {
        if(my != i)
        {
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
        case 0: //基本信息
        document.getElementById("Tables0").style.display = "";
        document.getElementById("Tables1").style.display = "none";
        document.getElementById("Tables2").style.display = "none";
        document.getElementById("Tables3").style.display = "none";
        document.getElementById("Tables4").style.display = "none";
        document.getElementById("Tables5").style.display = "none";
        break;
        case 1: //处理信息
        document.getElementById("Tables0").style.display = "none";
        document.getElementById("Tables1").style.display = "";
        document.getElementById("Tables2").style.display = "none";
        document.getElementById("Tables3").style.display = "none";
        document.getElementById("Tables4").style.display = "none";
        document.getElementById("Tables5").style.display = "none";               
        break;
        case 2: //相关事件
        document.getElementById("Tables0").style.display = "none";
        document.getElementById("Tables1").style.display = "none";
        document.getElementById("Tables2").style.display = "";
        document.getElementById("Tables3").style.display = "none";
        document.getElementById("Tables4").style.display = "none";
        document.getElementById("Tables5").style.display = "none";
        break;
        case 3: //相关问题
        document.getElementById("Tables0").style.display = "none";
        document.getElementById("Tables1").style.display = "none";
        document.getElementById("Tables2").style.display = "none";
        document.getElementById("Tables3").style.display = "";
        document.getElementById("Tables4").style.display = "none";
        document.getElementById("Tables5").style.display = "none";
        break;
        case 4: //知识参考

        document.getElementById("Tables0").style.display = "none";
        document.getElementById("Tables1").style.display = "none";
        document.getElementById("Tables2").style.display = "none";
        document.getElementById("Tables3").style.display = "none";
        document.getElementById("Tables4").style.display = "";
        document.getElementById("Tables5").style.display = "none";
        Iframe4.location="../InformationManager/frmInf_InformationMain.aspx?IsSelect=1";
        break;
        case 5: //配置信息

        document.getElementById("Tables0").style.display = "none";
        document.getElementById("Tables1").style.display = "none";
        document.getElementById("Tables2").style.display = "none";
        document.getElementById("Tables3").style.display = "none";
        document.getElementById("Tables4").style.display = "none";
        document.getElementById("Tables5").style.display = "";
        var CustID = document.all.<%=hidCustID.ClientID%>.value;
        if(CustID=="")
        {
            CustID = 0;
        }
        var EquID="";
        if(EquID=="")
        {
            EquID = 0;
        }
        Iframe5.location="../AppForms/frmServiceProtocolShow.aspx?ID=" + CustID + "&EquID=" + EquID;
        break;
    }
}
//获取资产信息信息
function LookEquDetail(equid) {
    window.open("frmEqu_DeskEdit.aspx?ChangeBillFlowID=" + <%=FlowID %> + "&id=" + equid + "&subjectid=1&IsChange=True&randomid=" + GetRandom(),"","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
}

  function selectIsplan()
        {
        if($("#<%=ddlIsplan.ClientID%>").val()=="1")
        {
        $("#div_isplan").slideDown();
        }
        else
        {
         $("#div_isplan").slideUp();
         $("#<%=ctrIsplan.ClientID%>").val("");
        }
        }
    </script>

    <asp:HiddenField ID="hidClientId_ForOpenerPage" runat="server" />
    <%--<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />--%>
    <input type="hidden" runat="server" id="hidEquIsHid" />
    <asp:HiddenField ID="hidDeskChange" runat="server" Value="" />
    <asp:HiddenField ID="hidChangeId" runat="server" Value="0" />
    <input type="hidden" runat="server" id="hidAppID" value="0" />
    <%--<input type="hidden" runat="server" id="hidFlowID" value="0" />--%>
    <table id="Table12" width="100%" align="center" runat="server" class="listNewContent">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            客户信息
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%" align="center" runat="server" id="Table2"
        border="0" cellspacing="1" cellpadding="1">
        <tr>
            <td style="width: 12%" class="listTitleRight">
                <asp:Literal ID="Change_CustName" runat="server" Text="客户名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="labCustAddr" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCustAddr" runat="server" onblur="GetCustID(this)" MaxLength="200"></asp:TextBox>
                <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                    class="btnClass2" />
                    <asp:Label ID="lblMustIn" runat="server" Text="*" Style="margin-left:7px;" ForeColor="Red"></asp:Label>
                <input id="hidCust" runat="server" type="hidden" />
                <input id="hidCustID" runat="server" type="hidden" value="-1" />
            </td>
            <td style="width: 12%" class="listTitleRight">
                <asp:Literal ID="Change_CustAddress" runat="server" Text="地址"></asp:Literal>
            </td>
            <td class="list">
                <div style=" width:300px; word-wrap: break-word">
                    <asp:Label ID="lblAddr" runat="server" Visible="False"></asp:Label>
                </div>               
                <asp:TextBox ID="txtAddr" runat="server" MaxLength="200"></asp:TextBox>
                <input id="hidAddr" runat="server" type="hidden" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="Change_CustContract" runat="server" Text="联系人"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labContact" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtContact" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                <input id="hidContact" runat="server" type="hidden" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="Change_CustPhone" runat="server" Text="联系人电话"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labCTel" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCTel" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                <input id="hidTel" runat="server" type="hidden" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="Change_CustDeptName" runat="server" Text="所属部门"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustDeptName" runat="server"></asp:Label>
                <input id="hidCustDeptName" type="hidden" runat="server" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="Change_CustEmail" runat="server" Text="电子邮件"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                <input id="hidCustEmail" type="hidden" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="Change_CustMastName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblMastCust" runat="server"></asp:Label>
                <input id="hidMastCust" type="hidden" runat="server" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="Change_CustJob" runat="server" Text="职位"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lbljob" runat="server"></asp:Label>
                <input id="hidjob" type="hidden" runat="server" />
            </td>
        </tr>
    </table>
    <table id="Table15" width="100%" align="center" runat="server" class="listNewContent">
        <tr>
            <td valign="top" align="left" class="listTitleNew" style="width: 92%">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            资产信息
                        </td>
                    </tr>
                </table>
            </td>
            <td id="trChange" runat="server" valign="middle" align="center" class="listTitleNew"
                style="width: 120">
                <asp:Button ID="btnChange" runat="server" Text="资产变更" OnClientClick="javascript:Change_confirm(this);"
                    CssClass="btnClass" Visible="false" />
                <input id="hidEquEdit" type="hidden" runat="server" value="1" />
            </td>
        </tr>
    </table>
    <table width="100%" runat="server" id="Table3" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="gvBillItem" runat="server" Width="100%" OnItemCommand="gvBillItem_ItemCommand"
                    ShowFooter="True" AutoGenerateColumns="False" CssClass="gridTable fixed-grid-border2" border="0"
                    CellSpacing="1" CellPadding="1" OnItemDataBound="gvBillItem_ItemDataBound">
                    <ItemStyle BackColor="White" CssClass="tablebody" />
                    <HeaderStyle CssClass="listTitle" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="资产目录" Visible="false">
                            <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblListName" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.LISTNAME")%>'></asp:Label>
                                <asp:TextBox ID="txtListName" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.LISTNAME")%>'
                                    Width="60%" MaxLength="50"></asp:TextBox>
                                <input id="cmdListName" onclick="FareSelect1(this,'1')" type="button" value="..."
                                    name="cmdSubject" runat="server" class="btnClass2" />
                                <input id="hidListName" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.LISTNAME")%>' />
                                <input id="hidListID" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.LISTID")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblAddListName" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.LISTNAME")%>'></asp:Label>
                                <asp:TextBox ID="txtAddListName" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.LISTNAME")%>'
                                    Width="60%" MaxLength="50"></asp:TextBox>
                                <input id="cmdAddListName" onclick="FareSelect1(this,'0')" type="button" value="..."
                                    name="cmdSubject" runat="server" class="btnClass2" />
                                <input id="hidAddListName" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.LISTNAME")%>' />
                                <input id="hidAddListID" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.LISTID")%>' />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="资产名称">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" Wrap="true"></HeaderStyle>
                            <ItemTemplate>
                                <input type="hidden" id="hidTypeID" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.LISTID")%>' />
                                <a href="#" title="单击查看资产详细信息" onclick='LookEquDetail(<%# DataBinder.Eval(Container, "DataItem.EQUID")%>)'>
                                    <asp:Label ID="lblEquName" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNAME")%>'></asp:Label>
                                </a>
                                <input type="hidden" id="hidID" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.EQUID")%>' />
                                <asp:TextBox ID="txtEquName" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNAME")%>'
                                    Width="60%" MaxLength="50"></asp:TextBox>
                                <input id="cmdEqu" onclick="FareSelect(this,'1')" type="button" value="..." name="cmdSubject"
                                    runat="server" class="btnClass2" />
                                <input id="hidEquName" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.EQUNAME")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <input type="hidden" id="hidAddTypeID" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.LISTID")%>' />
                                <asp:Label ID="lblAddEquName" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.EQUID")%>'></asp:Label>
                                <input type="hidden" id="hidAddID" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.EQUID")%>' />
                                <asp:TextBox ID="txtAddEquName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNAME")%>'
                                    Width="60%" MaxLength="50"></asp:TextBox>
                                <input id="cmdAddEqu" onclick="FareSelect(this,'2')" type="button" value="..." runat="server"
                                    class="btnClass2" />
                                <asp:HiddenField ID="hidClientId_ForOpenerPage" runat="server" />
                                <input id="hidAddEquName" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.EQUNAME")%>' />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="维护部门">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" Wrap="true"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDept" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DEPT")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblDept" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DEPT")%>'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="资产编号">
                            <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUCODE")%>'></asp:Label>
                                <input id="hidCode" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.EQUCODE")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblAddCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Code")%>'></asp:Label>
                                <input id="hidAddCode" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.EQUCODE")%>' />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="变更内容">
                            <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <uc4:CtrFlowFormText ID="CtrChangeContent" runat="server" TextToolTip="变更内容" MustInput="false"
                                    MaxLength="100" TextMode="MultiLine" Width="90%" Value='<%# DataBinder.Eval(Container, "DataItem.CHANGECONTENT")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <uc4:CtrFlowFormText ID="CtrAddChangeContent" runat="server" TextToolTip="变更内容" MustInput="false"
                                    MaxLength="100" TextMode="MultiLine" Width="90%" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="变更原值" Visible="false">
                            <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <uc4:CtrFlowFormText ID="CtrOldValue" runat="server" TextToolTip="变更原值" MustInput="false"
                                    MaxLength="100" TextMode="MultiLine" Width="100%" Value='<%# DataBinder.Eval(Container, "DataItem.OLDVALUE")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <uc4:CtrFlowFormText ID="CtrAddOldValue" runat="server" TextToolTip="变更原值" MustInput="false"
                                    MaxLength="100" TextMode="MultiLine" Width="100%" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="变更后值" Visible="false">
                            <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <uc4:CtrFlowFormText ID="CtrNewValue" runat="server" TextToolTip="变更后值" MustInput="false"
                                    MaxLength="100" TextMode="MultiLine" Width="100%" Value='<%# DataBinder.Eval(Container, "DataItem.NEWVALUE")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <uc4:CtrFlowFormText ID="CtrAddNewValue" runat="server" TextToolTip="变更后值" MustInput="false"
                                    MaxLength="100" TextMode="MultiLine" Width="100%" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="影响分析">
                            <HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>

                                    <asp:Button ID="btnImpactAnalysis" OnClientClick='<%#GetUrlEquId((decimal)DataBinder.Eval(Container.DataItem, "EQUID"))%>'  runat="server"  Text="影响分析" SkinID="btnClass1" Visible="true" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle Width="3%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" OnClientClick="return confirm('确定删除吗？')"
                                    CausesValidation="False" SkinID="btnClass1"></asp:Button>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Button ID="btnadd" CommandName="Add" runat="server" Text="新增" CausesValidation="False"
                                    SkinID="btnClass1" Font-Size="14px"></asp:Button>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="变更">
                            <HeaderStyle Width="8%" HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnChange" runat="server" Width="98%" Text="变更配置项" CausesValidation="False"
                                    OnClientClick="return Change_confirm(this);" SkinID="btnClass1" UseSubmitBehavior="false"
                                    Visible="true"></asp:Button>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table height="29" border="0" cellpadding="0" cellspacing="0">
        <tr style="cursor: hand">
            <td width="7">
                <img src="../Images/lm-left.gif" width="7" height="29" />
            </td>
            <td width="95" height="29" id="name0" valign="middle" class="td_3" onclick="chang_Class('name',6,0)"
                align="center" background="../Images/lm-b.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')">
                <span id="a0" class="td_3">基本信息</span>
            </td>
            <td width="95" height="29" id="name1" valign="middle" onclick="chang_Class('name',6,1)"
                align="center" class="STYLE4" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')">
                <span id="a1" class="STYLE4">处理信息</span>
            </td>
            <td width="95" height="29" id="name2" onclick="chang_Class('name',6,2)" align="center"
                valign="middle" class="STYLE4" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')" style="display: none">
                <span id="a2" class="STYLE4">相关事件</span>
            </td>
            <td width="95" height="29" id="name3" onclick="chang_Class('name',6,3)" align="center"
                valign="middle" class="STYLE4" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')" style="display: none">
                <span id="a3" class="STYLE4">相关问题</span>
            </td>
            <td width="95" height="29" id="name4" onclick="chang_Class('name',6,4)" align="center"
                valign="middle" class="STYLE4" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')">
                <span id="a4" class="STYLE4">知识参考</span>
            </td>
            <td width="95" height="29" id="name5" onclick="chang_Class('name',6,5)" align="center"
                valign="middle" class="STYLE4" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')" style="display: none">
                <span id="a5" class="STYLE4">配置信息</span>
            </td>
            <td width="7">
                <img src="../Images/lm-right.gif" width="7" height="29" />
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <table id="Tables0" align="center" width="100%" class="listContent" border="0" cellspacing="1"
                    cellpadding="1">
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_ChangeNo" Text="变更单号" runat="server"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="labServiceNo" runat="server"></asp:Label>
                        </td>
                       
                        
                         <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_RegTime" runat="server" Text="登记时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTRegTime" runat="server" ContralState="eReadOnly" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_RegUserName" runat="server" Text="登&nbsp;单&nbsp;人"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">                            
                            <uc3:UserPicker ID="UserPicker1" runat="server" ContralState="eReadOnly" />                            
                        </td>
                          <td class="listTitleRight" style="width: 12%">
                              变更需求人
                        </td>
                        <td class="list">                            
                            <uc3:UserPicker ID="ctrChangeNeedPeople" runat="server" MustInput="true" TextToolTip="变更需求人" />                            
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_ChangeTypeName" runat="server" Text="变更类别"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                            <uc11:ctrFlowCataDropListNew ID="CtrChangeType" runat="server" RootID="1033" MustInput="true"
                                TextToolTip="变更类别" />
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_ChangeTime" runat="server" Text="变更时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTCustTime" runat="server" MustInput="true" TextToolTip="变更时间" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                           主题
                        </td>
                        <td class="list" colspan="3">
                            <uc4:CtrFlowFormText ID="CtrFlowFTSubject" Width="80%" runat="server" TextToolTip="主题"
                                MustInput="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            描述
                        </td>
                        <td colspan="3" class="list">
                            <uc2:CtrFlowRemark ID="CtrFlowReContent" runat="server" MustInput="true" TextToolTip="描述" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="Change_EffectName" runat="server" Text="影响度"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrFCDEffect" runat="server" RootID="1026" MustInput="true"
                                TextToolTip="影响度" />
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="Change_InstancyName" runat="server" Text="紧急度"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrFCDInstancy" runat="server" RootID="1027" MustInput="true"
                                TextToolTip="紧急度" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="Change_LevelName" runat="server" Text="变更级别"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrFCDlevel" runat="server" RootID="1025" MustInput="true"
                                TextToolTip="变更级别" />
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="Change_DealStatus" runat="server" Text="变更状态"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrFCDDealStatus" runat="server" RootID="1022" MustInput="true"
                                TextToolTip="变更状态" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_IsPlan" runat="server" Text="是否计划性变更"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                            <%--<uc11:ctrFlowCataDropListNew ID="CtrIsPlanChange" ShowType="2" RootID="1059" runat="server"
                                TextToolTip="是否计划性变更"  ContralState="eReadOnly"/>--%>
                            <asp:Label ID="labIsPlanChange" runat="server" Text="" ></asp:Label>
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_Place" runat="server" Text="变更场所"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                      <%--      <uc11:ctrFlowCataDropListNew ID="CtrChangePlace" ShowType="2" RootID="1058" runat="server"
                                TextToolTip="变更场所" />--%>
                            <asp:CheckBoxList ID="chkChangePlace" RepeatLayout="Flow" runat="server" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                            <span style="color:Red;margin-left: 7px; font-size: small; font-weight: normal;" runat="server" id="spPlace">*</span>
                            <asp:Label ID="labChangePlace" runat="server" Text="" Visible="false"></asp:Label>
                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_ChangeWindow" runat="server" Text="变更窗口"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                            <uc5:ctrcataselectshowinptext ID="CtrChangeWindow" CatalogId="1061" 
                                SelToolTip="变更窗口"  ToolTipString="变更窗口说明"
                                TextShowCatalogId="10313" MustInput="true" runat="server" />
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_IsStopServer" runat="server" Text="是否停用服务"></asp:Literal>
                        </td>
                        <td class="list">
                            <%--<uc7:TrueOrFalseShowInpText ID="CtrStopServer" TextMustInput="true" ToolTipString="停用服务说明"
                                runat="server" />--%>

                            <uc8:trueorfalseshowinptext2 ID="CtrStopServer"  MustInput="true" 
                                runat="server" SelToolTip="是否停用服务" TextMustInput="true" 
                                ToolTipString="停用服务说明" />
                                
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_BusEffect" runat="server" Text="是否业务影响"></asp:Literal>
                        </td>
                        <td class="list">
                            <%--<uc7:TrueOrFalseShowInpText ID="CtrBusEffect" TextMustInput="true" ToolTipString="业务影响说明"
                                runat="server" />--%>
                                <uc8:trueorfalseshowinptext2 ID="CtrBusEffect"  MustInput="true" 
                                runat="server" SelToolTip="是否业务影响"  TextMustInput="true" 
                                ToolTipString="业务影响说明" />
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_DataEffect" runat="server" Text="是否数据影响"></asp:Literal>
                        </td>
                        <td class="list">
                           <%-- <uc7:TrueOrFalseShowInpText ID="CtrDataEffect" TextMustInput="true" ToolTipString="数据影响说明"
                                runat="server" />--%>
                                <uc8:trueorfalseshowinptext2 ID="CtrDataEffect" runat="server"  
                                MustInput="true" SelToolTip="是否数据影响"  TextMustInput="true" 
                                ToolTipString="数据影响说明" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_PlanStartTime" runat="server" Text="计划开始时间"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                            <uc1:ctrdateandtime ID="CtrPlanStartTime" runat="server" TextToolTip="计划开始时间" MustInput="true" />
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_PlanEndTime" runat="server" Text="计划完成时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrPlanEndTime" runat="server" TextToolTip="计划完成时间" MustInput="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            应急/回退方案
                        </td>
                        <td colspan="3" class="list">
                            <asp:DropDownList ID="ddlIsplan" runat="server" Width="152px" onclick="selectIsplan()">
                            <asp:ListItem Value="-1" Text=""></asp:ListItem>
                            <asp:ListItem Value="0">无</asp:ListItem>
                            <asp:ListItem  Value="1">有</asp:ListItem>
                            </asp:DropDownList>
                            <span style="color:Red;margin-left: 7px; font-size: small; font-weight: normal;" runat="server" id="spPlan" >*</span>
                            <div id="div_isplan" style=" display:none;"> 
                            <uc2:CtrFlowRemark ID="ctrIsplan" runat="server" MustInput="false" TextToolTip="应急/回退方案" />
                            </div>
                        </td>
                    </tr>  
                    <tr>
                        <td colspan="4" class="list">
                            <uc13:Extension_DayCtrList ID="Extension_DayCtrList1" runat="server" />
                        </td>
                    </tr>
                </table>
                <table id="Tables1" align="center" width="100%" class="listContent" style="display: none"
                    border="0" cellspacing="1" cellpadding="1">
                        <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_RealStartTime" runat="server" Text="实际开始时间"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                            <uc1:ctrdateandtime ID="CtrRealStartTime" runat="server" TextToolTip="实际开始时间" />
                            
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_RealEndTime" runat="server" Text="实际结束时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrRealEndTime" runat="server" TextToolTip="实际结束时间"  />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_ChangeAnalyses" runat="server" Text="变更分析"></asp:Literal>
                        </td>
                        <td colspan="3" class="list">
                            <uc2:CtrFlowRemark ID="CtrFlowReChangeAnalyses" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Change_ChangeAnalysesResult" runat="server" Text="分析结果"></asp:Literal>
                        </td>
                        <td colspan="3" class="list">
                            <uc2:CtrFlowRemark ID="CtrFlowReChangeAnalysesResult" runat="server" />
                        </td>
                    </tr>
                </table>
                <table id="Tables2" align="center" width="100%" height="100px" class="listContent"
                    style="display: none">
                    <tr id="trAIssu" runat="server">
                        <td class="list" colspan="3" valign="top">
                            <asp:DataGrid ID="gridAttention" runat="server" AutoGenerateColumns="False" CssClass="Gridtable fixed-grid-border2"
                                BorderColor="White" Width="100%" border="0" CellSpacing="1" CellPadding="1">
                                <AlternatingItemStyle BackColor="Azure" />
                                <ItemStyle BackColor="White" CssClass="tablebody" />
                                <HeaderStyle CssClass="listTitle" />
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
                                    <asp:BoundColumn DataField="Subject" HeaderText="摘要"></asp:BoundColumn>
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
                <table id="Tables3" align="center" width="100%" height="100px" class="listContent"
                    style="display: none">
                    <tr id="tr1" runat="server">
                        <td class="list" colspan="3" valign="top">
                            <asp:DataGrid ID="dgRelProblem" runat="server" AutoGenerateColumns="False" CssClass="Gridtable fixed-grid-border2"
                                BorderColor="White" Width="100%" border="0" CellSpacing="1" CellPadding="1">
                                <AlternatingItemStyle BackColor="Azure" />
                                <ItemStyle BackColor="White" CssClass="tablebody" />
                                <HeaderStyle CssClass="listTitle" />
                                <Columns>
                                    <asp:BoundColumn DataField="Status" HeaderText="Status" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="flowid" HeaderText="flowid" ReadOnly="True" Visible="False">
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ServiceNo" HeaderText="问题单号">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Problem_TypeName" HeaderText="问题类别">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Problem_Title" HeaderText="摘要"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="StateName" HeaderText="问题状态">
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
                                                type="button" value="详情" class="btnClass1" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" Mode="NumericPages" />
                            </asp:DataGrid>
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                            &nbsp; &nbsp;<asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                            &nbsp;
                            <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="Tables4" align="center" width="100%" class="listContent" style="display: none"
                    border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td align="right" class="listTitleRight">
                            <iframe id="Iframe4" name="Iframe4" src="" width="100%" height='300' scrolling="auto"
                                frameborder="0"></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables5" align="center" width="100%" class="listContent" style="display: none"
                    border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td align="right" class="listTitleRight">
                            <iframe id="Iframe5" name="Iframe5" src="" width="100%" height='300' scrolling="auto"
                                frameborder="0"></iframe>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        var temp = 0;
        for (i = 0; i < 6; i++) {
            if (i != temp) {
                document.getElementById("name" + i).className = "STYLE4";
                document.getElementById("name" + i).background = "../Images/lm-a.gif"; ;
                document.getElementById("a" + i).className = "STYLE4";
                document.getElementById("name" + i).style.display = "";
            }
        }
        if ('<%=ChangeFlowID%>' == "0") {
            document.getElementById("name2").style.display = "none";
        }
        if ('<%=FromProblemFlowID%>' == "0") {
            document.getElementById("name3").style.display = "none";
        }
        selectobj = document.getElementById("name" + temp);
        document.getElementById("name" + temp).className = "td_3";
        document.getElementById("name" + temp).background = "../Images/lm-b.gif";
        document.getElementById("a" + temp).className = "td_3";

        document.getElementById("name5").style.display = "none";
    </script>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="744px" ShowMessageBox="True"
        ShowSummary="False" HeaderText="对不起,您输入的数据不完整,请正确输入以下数据:"></asp:ValidationSummary>
    <input id="hidFlowID" runat="server" type="hidden" value=" 0" />
    
<!--Begin: 引入基础脚本-->
<script src="../js/epower.base.js" type="text/javascript" language="javascript"></script>    
<!--End: 引入基础脚本-->
</asp:Content>
