<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frm_Demand_Template.aspx.cs" Inherits="Epower.ITSM.Web.Demand.frm_Demand_Template"
    Title="需求请求模板编辑" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
	        
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
              }
              else
              {
                tableCtrl.style.display ="none";
                imgCtrl.src = ImgPlusScr ;	                
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
                
                var pars = "randomid="+GetRandom()+"&Owner=" + escape(radValue)+"&AppID=1062";
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
    <table class="listContent" width="98%" cellpadding="2" cellspacing="0" id="Table2">
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
    <table class="listContent" width="98%" cellpadding="2" cellspacing="0" id="Table1">        
        <tr>
            <td class="listTitleRight" style="width:12%;">
                <asp:Literal ID="LitServiceType" runat="server" Text="需求类别(范围/初值)"></asp:Literal>
            </td>
            <td class="list">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlDemandRootType" OnSelectedIndexChanged="ddlDemandRootType_SelectedIndexChanged"
                                AutoPostBack="true" Width="152px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <uc3:ctrFlowCataDropListNew ID="ctrFlowDemandType" runat="server" ShowType="2" Width="152px"
                                RootID="1003" />
                        </td>
                    </tr>
                </table>
            </td>           
        </tr>
    </table>
</asp:Content>
