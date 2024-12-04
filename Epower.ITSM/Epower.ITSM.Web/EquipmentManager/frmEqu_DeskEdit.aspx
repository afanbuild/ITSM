<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    ValidateRequest="false" CodeBehind="frmEqu_DeskEdit.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_DeskEdit"
    Title="资产资料编辑" %>

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
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc11" %>
<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc12" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
    <script language="javascript" type="text/javascript">
       
   function SubmitValidate()         //保存前数据检验   {
        if(document.all.<%=dtServiceBeginTime.ClientID%>_txtDate.value.trim()>=document.all.<%=dtServiceEndTime.ClientID%>_txtDate.value.trim())
        {
            alert("保修期截止日期必须大于起始日期！");
            return false;
        }
        return true;
   }
    function SelectProvide()
    {	
         var newDateObj = new Date()
	     var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
	    var	value=window.showModalDialog("../Provide/frmPro_ProvideManageMain.aspx?IsSelect='1'&pDate="+sparamvalue,"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
	    if(value != null)
	    {
		    if(value.length>1)
		    {
			    document.all.<%=txtProvideName.ClientID%>.value=value[1];
			    document.all.<%=hidProvideName.ClientID%>.value=value[1];
			    document.all.<%=hidProvideID.ClientID%>.value=value[0];
		    }
		    else
		    {
		        document.all.<%=txtProvideName.ClientID%>.value="";
		        document.all.<%=hidProvideName.ClientID%>.value="";
			    document.all.<%=hidProvideID.ClientID%>.value="0";
		    }
	    }
    }

    function OpenCateList()
    {
        var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskCateListSel.aspx?random=" + GetRandom() + "&subjectid="+<%=lngCatalogID%>,"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
	    if(value != null)
	    {
		    if(value.length>1)
		    {
			    document.all.<%=txtCateList .ClientID%>.value=value[1];
			    document.all.<%=hidCateListName.ClientID%>.value=value[1];
			    document.all.<%=hiCateListID.ClientID%>.value=value[0];
		    }
		    else
		    {
		        document.all.<%=txtCateList.ClientID%>.value="";
		        document.all.<%=hidCateListName.ClientID%>.value="";
			    document.all.<%=hiCateListID .ClientID%>.value="0";
		    }
	    }
	    else
	    {
	            document.all.<%=txtCateList.ClientID%>.value="";
		        document.all.<%=hidCateListName.ClientID%>.value="";
			    document.all.<%=hiCateListID .ClientID%>.value="0";
	    }
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
   
    function delete_confirm()
	{
	    event.returnValue =confirm("确认要删除吗?");
	}
    String.prototype.trim = function()  //去空格
	{
		// 用正则表达式将前后空格
		// 用空字符串替代。
		return this.replace(/(^\s*)|(\s*$)/g, "");
	}
	
	function GetMastCust()
	{
	   var ddlMastCust  =   document.getElementById("<%=ddlMastCust.ClientID %>");   
       var value = ddlMastCust.options[ddlMastCust.selectedIndex].value; 
       return value;
	}
	//客户改变函数
	function ChangeCust()
	{
	    var CustID = document.getElementById("<%=CustomCtr1.ClientID %>_hidCustom").value;        
        var pars = "act=custdetail&CustID="+CustID;
         $.ajax({
                    type: "post",
                    data:pars,
                    async:false,
                    url: "../Forms/Handler.ashx",
                    success: function(data, textStatus){
                        //alert(data);
                        var json = eval("(" + data + ")");
                                                
                        if (json.record != null && json.record.length > 0)
                        {
                            selectValue(document.getElementById("<%=ddlMastCust.ClientID %>"), json.record[0].mastcustid);
                        }
		            }
                 });
	}
	
	//服务单位改变函数
	function ChangeMastCust()
	{
	    document.getElementById("<%=CustomCtr1.ClientID %>_hidCustom").value ="0";
	    document.getElementById("<%=CustomCtr1.ClientID %>_hidCustomName").value ="";
	    document.getElementById("<%=CustomCtr1.ClientID %>_txtCustom").value ="";
	}
			
    </script>

    <script type="text/javascript" language="javascript">
        var selectobj = document.getElementById("name0");

        function chang(obj, img) {
            if (obj != selectobj)
                obj.background = "../Images/" + img;
        }

        function chang_Class(name, num, my) {
            for (i = 0; i < num; i++) {
                if (i != my) {
                    document.getElementById(name + i).className = "STYLE4";
                    //document.getElementById(name + i).background = "../Images/lm-a.gif";
                    $("#" + name + i).css("background-image", "url(../Images/lm-a.gif)");
                    document.getElementById("a" + i).className = "STYLE4";
                }
            }
            selectobj = document.getElementById(name + my);
            document.getElementById(name + my).className = "td_3";
            //document.getElementById(name + my).background = "../Images/lm-2b.gif";
            $("#" + name + my).css("background-image", "url(../Images/lm-2b.gif)");
            document.getElementById("a" + my).className = "td_3";

            switch (my) {
                case 0:
                    document.getElementById("Tables0").style.display = "";
                    document.getElementById("Tables1").style.display = "none";
                    document.getElementById("Tables2").style.display = "none";
                    document.getElementById("Tables3").style.display = "none";
                    document.getElementById("Tables4").style.display = "none";
                    document.getElementById("Tables5").style.display = "none";
                    document.getElementById("Tables6").style.display = "none";
                    if (window.FTB_API && window.FTB_API['ctl00_ContentPlaceHolder1_txtConfigureInfo']){
                    window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox1'].GoToDesignMode();
                 }
                    break;
                case 1:    //资产事件记录
                    document.getElementById("Tables0").style.display = "none";
                    document.getElementById("Tables1").style.display = "";
                    document.getElementById("Tables2").style.display = "none";
                    document.getElementById("Tables3").style.display = "none";
                    document.getElementById("Tables4").style.display = "none";
                    document.getElementById("Tables5").style.display = "none";
                    document.getElementById("Tables6").style.display = "none";
                    if (Iframe1.location == "about:blank") {
                        Iframe1.location = 'frmShowEquRel.aspx?EquID=<%=EquID %>&Type=1';
                    }
                    break;
                case 2:  //资产巡检记录
                    document.getElementById("Tables0").style.display = "none";
                    document.getElementById("Tables1").style.display = "none";
                    document.getElementById("Tables2").style.display = "";
                    document.getElementById("Tables3").style.display = "none";
                    document.getElementById("Tables4").style.display = "none";
                    document.getElementById("Tables5").style.display = "none";
                    document.getElementById("Tables6").style.display = "none";
                    if (Iframe2.location == "about:blank") {
                        Iframe2.location = 'frmShowEquRel.aspx?EquID=<%=EquID %>&Type=2';
                    }
                    break;
                case 3:   //资产变更记录
                    document.getElementById("Tables0").style.display = "none";
                    document.getElementById("Tables1").style.display = "none";
                    document.getElementById("Tables2").style.display = "none";
                    document.getElementById("Tables3").style.display = "";
                    document.getElementById("Tables4").style.display = "none";
                    document.getElementById("Tables5").style.display = "none";
                    document.getElementById("Tables6").style.display = "none";
                    if (Iframe3.location == "about:blank") {
                        Iframe3.location = 'frmShowEquRel.aspx?EquID=<%=EquID %>&Type=3';
                    }
                    break;
                case 4:   //关联资产
                    document.getElementById("Tables0").style.display = "none";
                    document.getElementById("Tables1").style.display = "none";
                    document.getElementById("Tables2").style.display = "none";
                    document.getElementById("Tables3").style.display = "none";
                    document.getElementById("Tables4").style.display = "";
                    document.getElementById("Tables5").style.display = "none";
                    document.getElementById("Tables6").style.display = "none";
                    if (Iframe4.location == "about:blank") {
                        Iframe4.location = 'frmShowEquRel.aspx?EquID=<%=EquID %>&Type=4';
                    }                                        
                    break;
                case 5:  //资产关联于


                    document.getElementById("Tables0").style.display = "none";
                    document.getElementById("Tables1").style.display = "none";
                    document.getElementById("Tables2").style.display = "none";
                    document.getElementById("Tables3").style.display = "none";
                    document.getElementById("Tables4").style.display = "none";
                    document.getElementById("Tables5").style.display = "";
                    document.getElementById("Tables6").style.display = "none";
                    if (Iframe5.location == "about:blank") {
                       
                        Iframe5.location = 'frmShowEquRel.aspx?EquID=<%=EquID %>&Type=5';
                    }
                    break;
                case 6:  //资产配置基线
                    document.getElementById("Tables0").style.display = "none";
                    document.getElementById("Tables1").style.display = "none";
                    document.getElementById("Tables2").style.display = "none";
                    document.getElementById("Tables3").style.display = "none";
                    document.getElementById("Tables4").style.display = "none";
                    document.getElementById("Tables5").style.display = "none";
                    document.getElementById("Tables6").style.display = "";
                    if (Iframe6.location == "about:blank") {
                        if ($.browser.msie &&( $.browser.version == "6.0" || $.browser.version == "7.0" || $.browser.version == "8.0" )) {
                            Iframe6.location = 'frm_Equ_HistoryChartView.aspx?newWin=true&id=<%=EquID%>&Type=6';
                        }
                        else {
                            Iframe6.location = 'frm_Equ_HistoryChartView_SVG.aspx?newWin=true&id=<%=EquID%>&Type=6';
                        }
                        
                    }
                    break;
            }
        }
    </script>

    <table style="width: 98%" class="listContent GridTable" cellpadding="2" cellspacing="0" runat="server"
        id="Table3">
        <tr id="trType" runat="server" style="width: 12%">
            <td class='listTitleRight'>
                <asp:Literal ID="Equ_CatalogName" runat="server" Text="资产类别"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <uc2:ctrEquCataDropList ID="CtrEquCataDropList1" runat="server" RootID="1" Width="152px" />
            </td>
        </tr>
        <tr style="display: none;">
            <td class='listTitleRight ' style="width: 12%">
                <asp:Literal ID="Equ_ListName" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCateList" runat="server" Visible="false"></asp:Label>
                <asp:TextBox ID='txtCateList' runat='server' ReadOnly="true" MaxLength="50"></asp:TextBox>
                <input id="Button1" runat="server" onclick="OpenCateList();" type="button" value="..."
                    class="btnClass2" />&nbsp;&nbsp;<asp:Label ID="MustCate" runat="server" Text="*"
                        ForeColor="Red" Font-Size="Small"></asp:Label>
                <input type="hidden" runat="server" id="hidCateListName" value="" />
                <input type="hidden" runat="server" id="hiCateListID" value="" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style="width: 12%">
                <asp:Literal ID="Equ_DeskName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class='list' style="width: 35%">
                <uc7:CtrFlowFormText ID="CtrFTName" runat="server" MustInput="true" TextToolTip="资产名称" />
            </td>
            <td class='listTitleRight ' style="width: 12%">
                <asp:Literal ID="Equ_Code" runat="server" Text="资产编号"></asp:Literal>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCode' runat='server' MaxLength="50"></asp:TextBox>&nbsp;<asp:Label
                    ID="MustCode" runat="server" Text="*" ForeColor="Red" Font-Size="Small"></asp:Label>
                <asp:Label ID="lblCode" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                <asp:Literal ID="Equ_MastCust" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblMastCust" runat="server" Visible="false"></asp:Label>
                <asp:DropDownList ID="ddlMastCust" Width="152px" runat="server" onchange="ChangeMastCust();">
                </asp:DropDownList>
            </td>
            <td class='listTitleRight '>
                <asp:Literal ID="Equ_CostomName" runat="server" Text="所属客户"></asp:Literal>
            </td>
            <td class="list">
                <uc5:CustomCtr ID="CustomCtr1" runat="server" MustInput="false" OnChangeScript="GetMastCust();"
                    OnChangeEndScript="ChangeCust();" />
                <asp:LinkButton ID="lblCustomCtr1" runat="server" ForeColor="#0000C0" OnClientClick="OpenCustInfo(this);"
                    Visible="False"></asp:LinkButton>
                <input id="hidCustID" type="hidden" runat="server" />
            </td>
        </tr>
        <tr style="display: none">
            <td class='listTitleRight '>
                <asp:Literal ID="Equ_SerialNumber" runat="server" Text="SN"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID='txtSerialNumber' runat='server' MaxLength="50"></asp:TextBox><asp:Label
                    ID="lblSerialNumber" runat="server" Visible="False"></asp:Label>
            </td>
            <td align="right" class="listTitleRight ">
                <asp:Literal ID="Equ_Breed" runat="server" Text="品牌"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtBreed" runat="server" MaxLength="50"></asp:TextBox><asp:Label
                    ID="lblBreed" runat="server" Text="" Visible="false"></asp:Label>
            </td>
            <td align="right" class="listTitleRight ">
                <asp:Literal ID="Equ_Model" runat="server" Text="型号"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtModel" runat="server" MaxLength="50"></asp:TextBox><asp:Label
                    ID="lblModel" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr style="display: none">
            <td class='listTitleRight '>
                <asp:Literal ID="Equ_ItemCode" runat="server" Text="条形码"></asp:Literal>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtItemCode' runat='server' MaxLength="50"></asp:TextBox>
                <asp:Label ID="lblItemCode" runat="server" Text="" Visible="false"></asp:Label>
            </td>
            <td class='listTitleRight '>
                <asp:Literal ID="Equ_Positions" runat="server" Text="位置"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID='txtPositions' runat='server' MaxLength="200"></asp:TextBox><asp:Label
                    ID="lblPositions" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr style="display: none">
            <td class='listTitleRight '>
                供应商
            </td>
            <td class='list'>
                <asp:TextBox ID='txtProvideName' runat='server' ReadOnly="true" MaxLength="50"></asp:TextBox>
                <input id="cmdPop" runat="server" onclick="SelectProvide();" type="button" value="..."
                    class="btnClass2" />
                <input id="hidProvideID" runat="server" type="hidden" />
                <input id="hidProvideName" runat="server" type="hidden" />
                <asp:LinkButton ID="lblProvideName" runat="server" ForeColor="#0000C0" OnClientClick="OpenProviderInfo(this);"
                    Visible="False"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight '>
              <asp:Literal ID="Equ_PartBankName" runat="server"  Text="维护机构"></asp:Literal>
            </td>
            <td class='list'>
                <uc9:DeptPickerbank ID="DeptPickerbank2" runat="server" TextToolTip="维护机构" MustInput="true" />
            </td>
            <td class='listTitleRight '>
                <asp:Literal ID="Equ_PartBranchName" runat="server"  Text="维护部门"></asp:Literal>
            </td>
            <td class='list'>
                <uc10:DeptPickerBranch ID="DeptBranch" runat="server" TextToolTip="维护部门" MustInput="true" />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitleRight ">
                <asp:Literal ID="Equ_EquStatusName" runat="server" Text="资产状态"></asp:Literal>
            </td>
            <td class="list">
                <uc11:ctrFlowCataDropListNew ID="ctrFlowCataEquStatus" runat="server" RootID="1018"
                    MustInput="true" TextToolTip="资产状态" />
            </td>
            <td class='listTitleRight'>
                <asp:Literal ID="Equ_ServiceTime" runat="server" Text="保修期"></asp:Literal>
            </td>
            <td class='list'>
                <uc3:ctrdateandtime ID="dtServiceBeginTime" runat="server" ShowTime="false" />
                <asp:Label ID="lblServiceBeginTime" runat="server" Text="" Visible="false"></asp:Label>
                ~
                <uc3:ctrdateandtime ID="dtServiceEndTime" runat="server" ShowTime="false" MustInput="true"
                    TextToolTip="保修期" />
                <asp:Label ID="lblServiceEndTime" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4" class="list">            
                <uc8:DymSchemeCtrList ID="DymSchemeCtrList1" runat="server" />                               
            </td>
        </tr>
        <tr>
            <td colspan="4" class="list">
                <uc12:ctrattachment ID="ctrattachment1" runat="server" />
            </td>
        </tr>
    </table>
    <table width="98%" cellpadding="0" cellspacing="0" border="0" id="TablesTitle">
        <tr style="text-align: left">
            <td>
                <table border="0" height="29" cellpadding="0" cellspacing="0">
                    <tr style="cursor: hand">
                        <td width="7" valign="top"><img src="../Images/lm-left.gif" width="7" height="29" /></td>
                        <td width="115" height="29" align="center" valign="middle" id="name0" class="td_3" onclick="chang_Class('name',7,0)" background="../Images/lm-2b.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')"><span id="a0" class="td_3">扩展配置</span></td>
                        <td width="115" height="29" align="center" valign="middle" id="name1" class="STYLE4" onclick="chang_Class('name',7,1)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a1" class="STYLE4">事件记录</span></td>
                        <td width="115" height="29" align="center" valign="middle" id="name2" class="STYLE4" onclick="chang_Class('name',7,2)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a2" class="STYLE4">巡检记录</span></td>
                        <td width="115" height="29" align="center" valign="middle" id="name3" class="STYLE4" onclick="chang_Class('name',7,3)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a3" class="STYLE4">变更记录</span></td>
                        <td width="115" height="29" align="center" valign="middle" id="name4" class="STYLE4" onclick="chang_Class('name',7,4)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a4" class="STYLE4">关联资产</span></td>
                        <td width="115" height="29" align="center" valign="middle" id="name5" class="STYLE4" onclick="chang_Class('name',7,5)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a5" class="STYLE4">资产关联于</span></td>
                        <td width="115" height="29" align="center" valign="middle" id="name6" class="STYLE4" onclick="chang_Class('name',7,6)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')" style="display: none;"><span id="a6" class="STYLE4">配置基线</span></td>
                        <td width="7" valign="top"><img src="../Images/lm-right.gif" width="7" height="29" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Tables0" width="100%" align="center" class="listContent">
                    <tr>
                        <td style="word-break: break-all;" class="list">
                            <ftb:FreeTextBox ID="txtConfigureInfo" StartMode="HtmlMode" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                                Height="300px" ImageGalleryPath="Attfiles\\Photos" Width="100%">
                            </ftb:FreeTextBox>
                            <asp:Label ID="lblConfigureInfo" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="Tables1" width="100%" align="center" class="listContent" style="display: none;">
                    <tr>
                        <td align="right" class="listTitleRight ">
                            <iframe id='Iframe1' name="Iframe1" src="" width='100%' height='400' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables2" width="100%" align="center" class="listContent" style="display: none;">
                    <tr>
                        <td align="right" class="listTitleRight ">
                            <iframe id='Iframe2' name="Iframe2" src="" width='100%' height='400' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables3" width="100%" align="center" class="listContent" style="display: none;">
                    <tr>
                        <td align="right" class="listTitleRight ">
                            <iframe id='Iframe3' name="Iframe3" src="" width='100%' height='400' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables4" width="100%" align="center" class="listContent" style="display: none;">
                    <tr>
                        <td align="right" class="listTitleRight ">
                            <iframe id='Iframe4' name="Iframe4" src="" width='100%' height='400' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables5" width="100%" align="center" class="listContent" style="display: none;">
                    <tr>
                        <td align="right" class="listTitleRight ">
                            <iframe id='Iframe5' name="Iframe5" src="" width='100%' height='400' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables6" width="100%" align="center" class="listContent" style="display: none;">
                    <tr>
                        <td align="right" class="listTitleRight ">
                            <iframe id='Iframe6' name="Iframe6" src="" width='100%' height='400' scrolling='auto'
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
        for (i = 0; i < 7; i++) {
            if (i != temp) {
                document.getElementById("name" + i).className = "STYLE4";
                document.getElementById("name" + i).background = "../Images/lm-a.gif";
                document.getElementById("a" + i).className = "STYLE4";
                if ('<%=EquID%>' == "0") {
                    document.getElementById("name" + i).style.display = "none";
                }
                else {
                    document.getElementById("name" + i).style.display = "";
                }
            }
        }
        selectobj = document.getElementById("name" + temp);
        document.getElementById("name" + temp).className = "td_3";
        document.getElementById("name" + temp).background = "../Images/lm-2b.gif";
        document.getElementById("a" + temp).className = "td_3";
        if ('<%=IsuueBase()%>' == "IssueBase") {
            document.getElementById("name" + temp).style.display = "none";
            document.getElementById("TablesTitle").style.display = "none";
        }
    </script>

</asp:Content>
