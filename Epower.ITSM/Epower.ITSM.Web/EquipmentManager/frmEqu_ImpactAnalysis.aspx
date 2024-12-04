<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmEqu_ImpactAnalysis.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_ImpactAnalysis"
    Title="变更影响度分析" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/DymSchemeCtr.ascx" TagName="DymSchemeCtr" TagPrefix="uc4" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register Src="../Controls/BussinessControls/CustomCtr.ascx" TagName="CustomCtr"
    TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc7" %>
<%@ Register Src="../Controls/DymSchemeCtrList.ascx" TagName="DymSchemeCtrList" TagPrefix="uc8" %>
<%@ Register Src="../Controls/DeptPickerbank.ascx" TagName="DeptPickerbank" TagPrefix="uc9" %>
<%@ Register Src="../Controls/DeptPickerBranch.ascx" TagName="DeptPickerBranch" TagPrefix="uc10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
function SubmitValidate()//保存前数据检验{
    return true;
}
function OpenProviderInfo(obj)
{
    var ObjID = obj.id.replace("lblProvideName","hidProvideID");
    var ID = document.getElementById(ObjID).value;
    
    window.open("../Provide/frmPro_ProvideManageEdit.aspx?newWin=true&ReadyOnly=1&id=" + ID + "&FlowID=0","","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
           
    event.returnValue = false;
}

function OpenCustInfo(obj)
{
    var CustObjID = obj.id.replace("lblCustomCtr1","hidCustID");
    var custID = document.getElementById(CustObjID).value;
    
    window.open("../Common/frmcusShow.aspx?newWin=true&ID=" + custID + "&FlowID=0","","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
           
    event.returnValue = false;
}



function OpenEquHistoryChart()
{
    window.open("frm_Equ_HistoryChartView.aspx?newWin=true&id=" + <%=EquID%>,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
    event.returnValue = false;
} 
 
String.prototype.trim = function()  //去空格{
	return this.replace(/(^\s*)|(\s*$)/g, "");
}
    </script>

    <script type="text/javascript" language="javascript">
        var selectobj = document.getElementById("name0");
    
        function chang(obj,img){
                if(obj!=selectobj)	    
		            obj.background="../Images/"+img;
        }
    
        function chang_Class(name, num, my) {
            for (i = 0; i < num; i++) {
                if (i != my) {
			        document.getElementById(name+i).className="STYLE4";
			        //document.getElementById(name+i).background="../Images/lm-a.gif";
			        $("#" + name + i).css("background-image", "url(../Images/lm-a.gif)");
			        document.getElementById("a"+i).className="STYLE4";
                }
            }
	        selectobj = document.getElementById(name+my);
	        document.getElementById(name+my).className="td_3";
	        //document.getElementById(name + my).background = "../Images/lm-b.gif";
	        $("#" + name + my).css("background-image", "url(../Images/lm-b.gif)");
	        document.getElementById("a"+my).className="td_3";
	
            switch (my) {
                case 0:   //影响资产
                    document.getElementById("Tables0").style.display = "";
                    document.getElementById("Tables1").style.display = "none";
                    document.getElementById("Tables2").style.display = "none";
                    if (Iframe0.location == "about:blank") {
                        Iframe0.location = 'frmShowImpactAnalysis.aspx?EquID=<%=EquID %>&Type=4';
                    }
                    break;
                case 1:  //关联于资产
                    document.getElementById("Tables0").style.display = "none";
                    document.getElementById("Tables1").style.display = "";
                    document.getElementById("Tables2").style.display = "none";
                    if (Iframe1.location == "about:blank") {
                        Iframe1.location = 'frmShowImpactAnalysis.aspx?EquID=<%=EquID %>&Type=5';
                    }
                    break;

                case 2:  //知识参考
                    document.getElementById("Tables0").style.display = "none";
                    document.getElementById("Tables1").style.display = "none";
                    document.getElementById("Tables2").style.display = "";
                    
                    var PKey = document.getElementById("<%=lblEquDeskName.ClientID %>").innerText;
                    
                    if (Iframe2.location == "about:blank") {
                        Iframe2.location = "../InformationManager/frmInf_InformationMain.aspx?IsSelect=1&PKey="+escape(PKey);
                    }
                    break;

            }
        }        

    </script>

    <table style="width: 98%" cellpadding="2" class="listContent" runat="server" id="Table3">
        <tr>
            <td class="listTitleRight" style='width: 12%;'>
                <asp:Literal ID="LitEquDeskName" runat="server" Text="名称"></asp:Literal>
            </td>
            <td class='list' colspan="3">
                <asp:Label ID="lblEquDeskName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr style="display:none;">
            <td class="listTitleRight" style='width: 12%;'>
                <asp:Literal ID="LitListName" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblListName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight"  style='width: 12%;'>
                <asp:Literal ID="LitEquDeskCode" runat="server" Text="资产编号"></asp:Literal>
            </td>
            <td class='list'  style="width: 35%">
                <asp:Label ID="lblEquDeskCode" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight" style='width: 12%;'>
                <asp:Literal ID="LitCustName" runat="server" Text="所属客户"></asp:Literal>
            </td>
            <td class="list" >
                <asp:Label ID="lblCustName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitPartBank" runat="server" Text="维护机构"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblPartBank" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitPartBranch" runat="server" Text="维护部门"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblPartBranch" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br /> 
    <table width="98%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
               <table height="29" border="0" cellpadding="0" cellspacing="0">
                    <tr style="CURSOR: hand">
                        <td width="7" valign="top"><img src="../Images/lm-left.gif" width="7" height="29" /></td>
                        <td width="95" height="29" align="center" valign="middle" id="name0" class="td_3" onclick="chang_Class('name',3,0)" background="../Images/lm-b.gif" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')"><span id="a0" class="td_3">影响资产</span></td>
                        <td width="95" height="29" align="center" valign="middle" id="name1" class="STYLE4" onclick="chang_Class('name',3,1)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')"><span id="a1" class="STYLE4">影响于资产</span></td>
                        <td width="95" height="29" align="center" valign="middle" id="name2" class="STYLE4" onclick="chang_Class('name',3,2)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')"><span id="a2" class="STYLE4">知识参考</span></td>
                        <td width="7" valign="top"><img src="../Images/lm-right.gif" width="7" height="29" /></td>
                    </tr>
                </table> 
            </td>
        </tr>
        <tr>
            <td class="list">
                <table id="Tables0" width="100%" align="center" class="listContent">
                    <tr>
                        <td style="word-break: break-all;" class="list">
                            <iframe id='Iframe0' name="Iframe0" src="" width='100%' height='400' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables1" width="100%" align="center" class="listContent" style="display: none;">
                    <tr>
                        <td align="right" class="listTitle">
                            <iframe id='Iframe1' name="Iframe1" src="" width='100%' height='400' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables2" width="100%" align="center" class="listContent" style="display: none;">
                    <tr>
                        <td align="right" class="listTitle">
                            <iframe id='Iframe2' name="Iframe2" src="" width='100%' height='400' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    <input id="hidTableExp" value="" runat="server" type="hidden" />

    <script type="Text/javascript" language="javascript">
        var temp = 0;
        for (i = 0; i < 3; i++) {
            if (i != temp) {
                document.getElementById("name" + i).className = "";
                document.getElementById("name" + i).background = "../Images/lm-a.gif";
                document.getElementById("name" + i).style.display = "";
                
                if ('<%=EquID%>' == "0") {
                    document.getElementById("name" + i).style.display = "none";
                }
                else {
                    document.getElementById("name" + i).style.display = "";
                }
            }
        }
        selectobj = document.getElementById("name" + temp);
        document.getElementById("name" + temp).className = "";
        document.getElementById("name" + temp).background = "../Images/lm-b.gif";
        
        //默认显示影响资产
        if (typeof(Iframe0) != undefined && Iframe0.location == "about:blank") {
            Iframe0.location = 'frmShowImpactAnalysis.aspx?EquID=<%=EquID %>&Type=4';
        }
        
        document.getElementById("name1").style.display = "none";
    </script>

</asp:Content>
