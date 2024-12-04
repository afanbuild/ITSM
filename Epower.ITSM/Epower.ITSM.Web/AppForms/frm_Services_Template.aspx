<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_Services_Template.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_Services_Template"
    Title="无标题页" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc8" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc7" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/common/DictionaryPicker.ascx" TagName="DictionaryPicker"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Register TagPrefix="uc11" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<%@ Register Src="../Controls/browsepic.ascx" TagName="browsepic" TagPrefix="uc3" %>
<%@ Register src="../Controls/ctrattachment.ascx" tagname="ctrattachment" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" src="../Js/jquery-1.7.2.min.js"></script>

    <script language="javascript" src="../Js/jShowDiv.js"></script>

    <script language="javascript" src="../Js/jUtility.js"></script>

    <script language="javascript">
//页面加载执行获取图片
 $(document).ready(function () {
        var slngid = GetQueryString("subjectid");		                            
        showPic(slngid);     // 读取图片信息     
 });
 
// 读取图片信息
function showPic(lngid) {
   $.ajax({
       type: 'POST',
       url: '../Forms/Handler.ashx',
       data: 'act=easerviceimg&lngid=' + lngid,
       timeout: '10000',
       error: function () {
           alert("读取图片信息失败！");
       }, success: function (json) {
           piclistCallBack(json);
       }
   });
}
                                 
//服务级别选择
function ServiceLevelSelect(obj) 
{
    var ServiceTypeID = "0";
    var ServiceKindID = "0";
    var ServiceEffID = "0";
    var ServiceInsID = "0";  
    var CustID = "";
    var EquID = ""; 
         
	var	value=window.showModalDialog("frmCst_ServicLevelSelect.aspx?IsSelect=true&randomid="+GetRandom()+"&CustID=" + CustID + "&EquID="+ EquID + "&TypeID=" + ServiceTypeID + "&KindID=" + ServiceKindID + "&EffID=" + ServiceEffID + "&InsID=" + ServiceInsID,window,"dialogHeight:600px;dialogWidth:800px");
	if(value != null)
	{
		if(value.length>1)
		{                         
            document.getElementById(obj.id.replace("cmdPopServiceLevel","txtServiceLevel")).value = value[2];   //级别名称        
            document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevel")).value = value[2];   //级别名称
            document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevelID")).value = value[1];  //级别ID                            
		}
	}
}        

//保存事件
function SubmitValidate()
{
    var objFlag=document.getElementById("<%=radioParent.ClientID %>");//标志
    var objParentList=document.getElementById("<%=drParentList.ClientID %>");//服务目录
    var objFlowModel=document.getElementById("<%=ddlFlowModel.ClientID %>");//事件模板
    
    var arr = objFlag.getElementsByTagName("input");
    
    var objvalue = 0;
    if(objFlag!=null){
        var i;
        for(i=0;i<arr.length;i++){
            if(arr[i].checked){
                objvalue= arr[i].value; 
            }
        }
    }
    
    if(objvalue=="0"){
        var pIndex=objParentList.selectedIndex;//服务目录
        var fIndex=objFlowModel.selectedIndex;//事件模板
        if(pIndex=="0")
        {
            objParentList.focus();
            alert("服务目录不能为空!");
            return false;
        }
        if(fIndex=="0")
        {
            objFlowModel.focus();
            alert("事件模板不能为空!");
            return false;
        }
    }
    return true;
}

//查询模板
function ShowIssTemp()
{
    //获取当前选择的事件模板ID
    var obj = document.getElementById("<%=ddlFlowModel.ClientID %>");
    if(obj.options.length<=0)
        return;
    
    if(obj.options[obj.selectedIndex].value=="")
    {
        return;
    }
    
    var tid = obj.options[obj.selectedIndex].value;
    window.open("frm_Issue_Template.aspx?id="+tid+"&IsShow=true","","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
    event.returnValue = false;
}

//单选按钮单击事件


function radioClick() {
    var radio=document.all.<%=radioParent.ClientID %>;//获取单选对象


    var childName=radio.firstChild.firstChild.firstChild.firstChild.name;
    var childNodes=document.getElementsByName(childName);
    for(var i=0;i<childNodes.length;i++)
    {
        var radValue=childNodes[i].value;
        if(childNodes[i].checked)
        {
            if(radValue=="0"){
               var objFlowModel = document.getElementById("<%=ddlFlowModel.ClientID %>");//事件模板
               var objGuide=document.getElementById("<%=txtGuide.ClientID %>");//指引
               var objHGuide=document.getElementById("<%=hfGuide.ClientID %>");//指引
               var objParentList=document.getElementById("<%=drParentList.ClientID %>");//服务目录
               var objFlag=document.getElementById("<%=hfFlag.ClientID %>");//标志
               objGuide.value=objHGuide.value;//指引
               objFlowModel.disabled=false;
               objGuide.disabled=false;
               objParentList.disabled=false;
               objFlag.value="1";
            }else{
               var objFlowModel = document.getElementById("<%=ddlFlowModel.ClientID %>");//事件模板
               var objGuide=document.getElementById("<%=txtGuide.ClientID %>");//指引
               var objParentList=document.getElementById("<%=drParentList.ClientID %>");//服务目录
               var objFlag=document.getElementById("<%=hfFlag.ClientID %>");//标志
               objGuide.value="";
               objFlowModel.selectedIndex=0;
               objParentList.selectedIndex=0;
               objFlag.value="0";
               objFlowModel.disabled=true;
               objGuide.disabled=true;
               objParentList.disabled=true;
            }
        }
    }
}   

		function delete_confirm()
			{
				if (event.srcElement.value =="删除" )
					event.returnValue =confirm("确认要删除吗?");
			}
		function SelectPCatalog()
			{ 
				if(	document.getElementById('<%=hidCatalogID1.ClientID %>').value== 1)//
				{
					alert("已经是最顶层分类!");
					return;
				}
				var newDateObj = new Date();
				var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString() + newDateObj.getMinutes().toString() + newDateObj.getMilliseconds().toString();
				//=========zxl======
				var url="frmpopSubject.aspx?CurrSubjectID=" + document.getElementById('<%=hidCatalogID1.ClientID %>').value + "&paramvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
				window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
			}
    </script>
    <asp:HiddenField ID="hfRadio" runat="server" Value="0" />
    <asp:HiddenField ID="hfFlag" runat="server" Value="0" />
    <asp:HiddenField ID="hfGuide" runat="server" Value="" />
    <asp:HiddenField ID="hidModified" runat="server" Value="0" />
    <asp:HiddenField ID="hidCatalogID1" runat="server" Value="0" />
        <table style="width: 98%" align="center" class="listContent">
        <tr>
            <td colspan="2" class="list">
                <uc11:CtrTitle ID="CtrTitle" runat="server" Title="服务目录管理 "></uc11:CtrTitle>
            </td>
        </tr>
        <tr height="40">
            <td colspan="2" class="listTitle">
                <asp:Button ID="cmdAdd" runat="server" Text="新增" CssClass="btnClass" OnClick="cmdAdd_Click"
                    CausesValidation="false"></asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdSave" runat="server" Text="保存" CssClass="btnClass" OnClick="cmdSave_Click">
                </asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdDelete" runat="server" Text="删除" CssClass="btnClass" OnClick="cmdDelete_Click"
                    OnClientClick="delete_confirm();" CausesValidation="false"></asp:Button>&nbsp;
            </td>
        </tr>
    </table>
    <table class="listContent" width="98%" align="center" runat="server" id="Table2">
        <tr>
          <td class="listTitleRight " nowrap="nowrap" style="width: 12%;display:none" >
                是否一级目录


            </td>
            <td class="list" style="width: 35%;display:none">
                <asp:RadioButtonList ID="radioParent"  runat="server" 
                    RepeatColumns="2">
                    <asp:ListItem Value="1">是</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
                服务项名称
            </td>
            <td class="list">
                <uc1:CtrFlowFormText ID="CtrFTTemplateName" runat="server" MaxLength="50" MustInput="true" TextToolTip="服务项名称" />
            </td>
               <td class="listTitleRight " nowrap="nowrap">
                <asp:Literal ID="Literal1" runat="server" Text="上级目录"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="drParentList" runat="server" Width="152px"  Visible=false>
                </asp:DropDownList>
                    <%--===zxl==--%>
                    
        <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
                     <asp:TextBox ID="txtPCatalogName" runat="server" Width="258" ></asp:TextBox>
                         <asp:HiddenField ID="hidPCatalogID" runat="server" Value="0" />      
                            <asp:HiddenField ID="hidPCatalogID1" runat="server" Value="0" />                          
                         <asp:HiddenField ID="hidPCatalogName" runat="server" Value="" />
                    
                <input
                        id="cmdPopParentCatalog" onclick="SelectPCatalog()" type="button" value="..."
                        class="btnClass2">
                 <span style="color:Red; font-size:Small">*</span>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                所属应用
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="ddlApp" runat="server"  Width="306px" 
                    onselectedindexchanged="ddlApp_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">--请选择--</asp:ListItem>
                    <asp:ListItem Value="1026">事件管理</asp:ListItem>
                    <asp:ListItem Value="1062">需求管理</asp:ListItem>
                </asp:DropDownList><asp:Label ID="rWarning" runat="server" Style="margin-left:7px;" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                事件模板
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="ddlFlowModel" runat="server"  Width="306px">
                </asp:DropDownList><asp:Label ID="Label1" runat="server" Style="margin-left:7px;" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
<%--                <a href="#" id="isstemp" onclick="ShowIssTemp();">
                    模板详情</a>--%>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
                <asp:Literal ID="LitContent" runat="server" Text="详细描述"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <asp:TextBox ID="txtContent" runat="server" Width="95%" MaxLength="500" Rows="3"
                    TextMode="MultiLine"></asp:TextBox><asp:Label ID="labContent" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
                <asp:Literal ID="LitGuide" runat="server" Text="指引"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <asp:TextBox ID="txtGuide" runat="server" Width="95%" MaxLength="500" Rows="3" TextMode="MultiLine"></asp:TextBox><asp:Label
                    ID="labGuide" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
                <asp:Literal ID="Literal2" runat="server" Text="图片LOGO"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <uc3:browsepic ID="browsepic1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
               附&nbsp;件

            </td>
            <td colspan="3" class="list" width="88%">                
                <uc4:ctrattachment ID="ctrattachment1" runat="server"  width="98%" />                
            </td>
        </tr>
    </table>      
</asp:Content>
