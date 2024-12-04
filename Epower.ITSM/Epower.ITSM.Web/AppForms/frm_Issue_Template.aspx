<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frm_Issue_Template.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_Issue_Template"
    Title="事件请求模板编辑" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc8" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc7" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/common/DictionaryPicker.ascx" TagName="DictionaryPicker"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript">


function next() { if (event.keyCode==13) event.keyCode=9;} 

function stringtrim(str){
		while (str.charAt(0)==' ')
			str=str.substr(1);
		while (str.charAt(str.length-1)==' ')
			str=str.substr(0,str.length-1);
		return str;
		
	}


function NumberInputlocal()
{
    if(((event.keyCode<48)||(event.keyCode>57))) 
    if ((event.keyCode !=37) & (event.keyCode !=39) & (event.keyCode !=8) 
        & (event.keyCode !=190) & (event.keyCode !=9) & (event.keyCode !=189) & (event.keyCode !=109) & ((event.keyCode<96) || (event.keyCode >110)))
        event.returnValue=false;    
} 
            
            
			
String.prototype.trim = function()  //去空格
{
	// 用正则表达式将前后空格
	// 用空字符串替代。
	return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;","");
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
            
//服务级别选择
function ServiceLevelSelect(obj) 
{
    var idtype= document.all.<%=ctrFlowServiceType.ClientID%>.id;
    var ddltype=document.getElementById(idtype);      
    var ServiceTypeID ;
    
    
    if(ddltype.selectedIndex!=null&&ddltype.selectedIndex!="undefined")
    {
        ///dropDownlist
        ServiceTypeID= ddltype.options[ddltype.selectedIndex].value.trim();
    }
    else
    {
        //RadionbuttonList
        var oRadioButtonLists=ddltype.getElementsByTagName('input');
        for(var i=0;i<oRadioButtonLists.length;i++)
        {
             if(oRadioButtonLists[i].checked) 
             {
                  ServiceTypeID = oRadioButtonLists[i].value;
             }
        }
    }
    
    var ideff= document.all.<%=CtrFlowEffect.ClientID%>.id;
    var ddleff=document.getElementById(ideff);      
    var ServiceEffID;
    
    if(ddleff.selectedIndex!=null&&ddleff.selectedIndex!="undefined")
    {
        ServiceEffID= ddleff.options[ddleff.selectedIndex].value.trim();
    }
    else
    {
        var oRadioButtonLists=ddleff.getElementsByTagName('input');
        for(var i=0;i<oRadioButtonLists.length;i++)
        {
             if(oRadioButtonLists[i].checked) 
             {
                  ServiceEffID = oRadioButtonLists[i].value;
             }
        }
    }
    
    var idins= document.all.<%=CtrFlowInstancy.ClientID%>.id;
    var ddlins=document.getElementById(idins);      
    var ServiceInsID ;//= ddlins.options[ddlins.selectedIndex].value.trim();
    
    if(ddlins.selectedIndex!=null&&ddlins.selectedIndex!="undefined")
    {
        ServiceInsID= ddlins.options[ddlins.selectedIndex].value.trim();
    }
    else
    {
        var oRadioButtonLists=ddlins.getElementsByTagName('input');
        for(var i=0;i<oRadioButtonLists.length;i++)
        {
             if(oRadioButtonLists[i].checked) 
             {
                  ServiceInsID = oRadioButtonLists[i].value;
             }
        }
    }
    //==zxl==
    var url="frmCst_ServicLevelSelect.aspx?IsSelect=true&TypeID=" + ServiceTypeID + "&EffID=" + ServiceEffID + "&InsID=" + ServiceInsID+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frm_Issue_Template";
    window.open( url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=100");
}

//选择资产目录
function SelectEquCatalog(obj)
{
        var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskCateListSel.aspx?random=" + GetRandom(),window,"dialogWidth=800px; dialogHeight=600px;") ;

        if(value != null)
        {
            if(value.length>1)
            {			
                if(document.getElementById(obj.id.replace("cmdListName","hidListID") ).value == value[0])
                {}
                else
                {
                    document.getElementById(obj.id.replace("cmdListName","txtListName")).value = value[1];   //资产目录名称
                    document.getElementById(obj.id.replace("cmdListName","hidListName")).value = value[1];   //资产目录名称
                    document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = value[0];  //资产目录ID
                }
            }
            else
            {			                
                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";         //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";         //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";         //资产目录ID
            }
        }
        else
        {			                
                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";    //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";    //资产目录名称
                document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";    //资产目录ID
        }
}
//模版性质改变
function ownerClick(){        
    
    var radio=document.all.<%=rblTempOwner.ClientID %>;//获取单选对象               
    var hidOldOwner=document.getElementById("hidOldOwner");//模版性质原值       
    var childNodes=document.getElementsByName("ctl00$ContentPlaceHolder1$rblTempOwner");//获取子项集合
    var ddl=document.all.<%=ddlFlowModel.ClientID %>;//流程模型
    
    for(var i=0;i<childNodes.length;i++)
    {        
        var radValue=childNodes[i].value;
        if(childNodes[i].checked)
        {
            if(radValue==hidOldOwner.value)//进行过滤
                return;
            hidOldOwner.value=radValue;
            
            var pars = "randomid="+GetRandom()+"&Owner=" + escape(radValue)+"&AppID=1026";
            var surl = "<%=sApplicationUrl %>/MyDestop/frmXmlHttp.aspx";
            
            $.ajax({
                    type: "get",
                    data:pars,
                    async:false,
                    url: surl,
                    success: function(data, textStatus){                         
                            var json= eval("("+data+")");
				            var record=json.record;
				            ddl.options.length=0;
				            var newOption=document.createElement("Option");
				            newOption.text="";
				            newOption.value="";
				            ddl.add(newOption);
				            for(var i=0; i < record.length; i++)
			                {   
			                    newOption = document.createElement("Option");
                                newOption.text = record[i].flowname;
                                newOption.value = record[i].oflowmodelid;
                                ddl.options.add(newOption);
			                }                                                                    
		            }
                 });                
        }
    }
}

//流程模型改变
function dllChange(dom)
{
    var objFlowModel=document.getElementById("<%=hidFlowModel.ClientID %>");//流程模型
    objFlowModel.value=dom.options[dom.selectedIndex].value;//流程模型ID
}
</script>
    <input type="hidden" id="hidOldOwner" value="-1" /><!--模版性质原值-->
       <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <asp:HiddenField ID="hidFlowModel" runat="server" Value="0" />
    <table id="Table12" width="98%" align="center" runat="server" class="listNewContent">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            模版信息
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="listContent Gridtable" width="98%" cellpadding="2" cellspacing="0" runat="server"
        id="Table2">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                模版名称
            </td>
            <td class="list" style="width: 35%">
                <uc1:CtrFlowFormText ID="CtrFTTemplateName" runat="server" MustInput="true" TextToolTip="模板名称" />
            </td>
            <td class="listTitleRight" style="width: 12%">
                模版性质
            </td>
            <td class="list">
                <asp:RadioButtonList ID="rblTempOwner" onclick="ownerClick();" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="0">公共</asp:ListItem>                    
                    <asp:ListItem Value="3">自助服务</asp:ListItem>                     
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                流程模型
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="ddlFlowModel" runat="server" Width="152px" onchange="dllChange(this);">
                </asp:DropDownList>                
                <span style="color:Red; font-size:Small">*</span>
            </td>
        </tr>
    </table>
    <table id="Table15" width="98%" align="center" runat="server" class="listNewContent" visible="false">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            请求对象
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="listContent" width="98%" cellpadding="2" cellspacing="0" runat="server"
        id="Table3" visible="false">
        <tr runat="server" id="trEqu">
            <td style="width: 12%" class="listTitleRight">
                <asp:Literal ID="LitListName" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblListName" runat="server" Visible="false"></asp:Label>
                <asp:TextBox ID="txtListName" runat="server" MaxLength="20" ReadOnly="true"></asp:TextBox>
                <input id="cmdListName" onclick="SelectEquCatalog(this)" type="button" value="..."
                    runat="server" name="cmdListName" class="btnClass2" />
                <input id="hidListName" value="" type="hidden" runat="server" />
                <input id="hidListID" value="0" type="hidden" runat="server" />
            </td>
        </tr>
    </table>
    <table id="Table11" class="listNewContent" width="98%" align="center" runat="server">
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
    <table class="listContent Gridtable" width="98%" cellpadding="2" cellspacing="0" runat="server"
        id="Table1">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitEffectName" runat="server" Text="影响度(范围/初值)"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlRootEffect" OnSelectedIndexChanged="ddlRootEffect_SelectedIndexChanged"
                                AutoPostBack="true" Width="152px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <uc3:ctrFlowCataDropListNew ID="CtrFlowEffect" runat="server" ShowType="2" Width="152px"
                                RootID="1023" />
                        </td>
                    </tr>
                </table>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitInstancyName" runat="server" Text="紧急度(范围/初值)"></asp:Literal>
            </td>
            <td class="list">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlInstancy" OnSelectedIndexChanged="ddlInstancy_SelectedIndexChanged"
                                AutoPostBack="true" Width="152px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <uc3:ctrFlowCataDropListNew ID="CtrFlowInstancy" runat="server" ShowType="2" Width="152px"
                                RootID="1024" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="Literal1" runat="server" Text="事件来源(范围/初值)"></asp:Literal>
            </td>
            <td class="list">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlFrom" OnSelectedIndexChanged="ddlRootFrom_SelectedIndexChanged"
                                AutoPostBack="true" Width="152px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <uc3:ctrFlowCataDropListNew ID="CtrFlowFrom" runat="server" ShowType="2" Width="152px"
                                RootID="1041" />
                        </td>
                    </tr>
                </table>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="Literal2" runat="server" Text="关闭理由(范围/初值)"></asp:Literal>
            </td>
            <td class="list">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlRootReason" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged"
                                AutoPostBack="true" Width="152px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <uc3:ctrFlowCataDropListNew ID="CtrFlowReason" runat="server" ShowType="2" Width="152px"
                                RootID="1040" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trShowServiceLevel" runat="server">
            <td class="listTitleRight">
                <asp:Literal ID="LitServiceType" runat="server" Text="事件类别(范围/初值)"></asp:Literal>
            </td>
            <td class="list">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlServRootType" OnSelectedIndexChanged="ddlServRootType_SelectedIndexChanged"
                                AutoPostBack="true" Width="152px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <uc3:ctrFlowCataDropListNew ID="ctrFlowServiceType" runat="server" ShowType="2" Width="152px"
                                RootID="1001" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left" class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="LitServiceLevel" runat="server" Text="服务级别"></asp:Literal>
            </td>
            <td class="list" nowrap="nowrap" colspan="3">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 100%">
                            <asp:TextBox ID="txtServiceLevel" runat="server" ReadOnly="True"></asp:TextBox>
                            <input id="cmdPopServiceLevel" onclick="ServiceLevelSelect(this);" runat="server"
                                class="btnClass2" type="button" value="..." />
                            <input id="hidServiceLevelID" runat="server" type="hidden" /><input id="hidServiceLevel"
                                runat="server" type="hidden" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="744px" ShowMessageBox="True"
        ShowSummary="False" HeaderText=""></asp:ValidationSummary>
</asp:Content>
