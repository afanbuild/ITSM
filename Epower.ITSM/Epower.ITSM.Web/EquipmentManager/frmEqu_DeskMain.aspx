<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="frmEqu_DeskMain.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_DeskMain"
    Title="�ʲ�����" %>

<%@ Register Src="../Controls/DeptPickerbank.ascx" TagName="DeptPickerbank" TagPrefix="uc12" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/BussinessControls/CustomCtr.ascx" TagName="CustomCtr"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/DeptPickerMult.ascx" TagName="DeptPickerMult" TagPrefix="uc7" %>
<%@ Register Src="../Controls/UserPickerMult.ascx" TagName="UserPickerMult" TagPrefix="uc8" %>
<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc9" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc10" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
var openobj = window;
if(typeof(window.dialogArguments) == "object")
{
    openobj =  window.dialogArguments;
}
//�豸�����¼
function SelectService(obj) 
{
    var lngID = document.getElementById(obj.id.replace("CmdService","hidID")).value;
	openobj.open("../AppForms/frmIssueList.aspx?NewWin=true&ID=0&EquID=" + lngID ,'','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');
	event.returnValue = false;
}
//�豸Ѳ��
function SelectByts(obj) 
{
    var lngID = document.getElementById(obj.id.replace("CmdByts","hidID")).value;
	openobj.open("frm_Equ_PatrolList.aspx?EquID=" + lngID ,'','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');
	event.returnValue = false;
}
    function ServerOndblclick(jsonstr)
    {      
        if(jsonstr != null)
	    {	    
	        var json = jsonstr;
		    var record=json.record;
    				    
		    for(var i=0; i < record.length; i++)
		    {	

		        try{
		        	window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value=record[i].name;//�豸����
                    window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value=record[i].id; //�豸ID 	         
	    	        window.opener.document.getElementById("<%=Opener_ClientId%>txtName").value=record[i].name;//�豸����
    		        window.opener.document.getElementById("<%=Opener_ClientId%>hiddeCmdEqu").click();
		        }catch(e){
		            try{
		                window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value=record[i].name;//�豸����		            
		            }catch(ex){ 
		                window.parent.returnValue = jsonstr;
		            }
		        }
		        
		    }
	    }
	    else
	    {
	        event.returnValue = false;
	    }
	    top.close();
    }    
    
   function ServerCst_Issue_Serivce(jsonstr)
   {
   //
   if(jsonstr != null)
		{
			var json = jsonstr;
		    var record=jsonstr.record;
		    
			for(var i=0; i < record.length; i++)
			{
			
			      window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value=record[i].name;//�豸����
			      window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value=record[i].name;//�豸����
			      window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value=record[i].id;//�豸ID
			      window.opener.document.getElementById("<%=Opener_ClientId%>txtListName").value=record[i].listname;//�ʲ�Ŀ¼����
			      window.opener.document.getElementById("<%=Opener_ClientId%>hidListName").value=record[i].listname;//�ʲ�Ŀ¼����
			      window.opener.document.getElementById("<%=Opener_ClientId%>hidListID").value=record[i].listid;//�ʲ�Ŀ¼ID
			      					
			}
		}
		else
		{
		     window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value="";//�豸����
			 window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value="";//�豸����
			 window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value="-1";//�豸ID
	
		}
		top.close();
    
   }
    //===========zxl==
    function ServerAdvancedCondition(jsonstr)
    {
         if(jsonstr != null)
        {
	        var json = jsonstr;
            var record=json.record;
            
	        for(var i=0; i < record.length; i++)
	        {					
                window.opener.document.getElementById("<%=Opener_ClientId%>txtEquipmentName").value = record[i].name;   //�豸����
                window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value = record[i].name;   //�豸����
                window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value = record[i].id;  //�豸ID
	        }
        }
        else
        {
            window.opener.document.getElementById("<%=Opener_ClientId%>txtEquipmentName").value = "";   //�豸����
            window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value = "";   //�豸����
            window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu" ).value = 0;  //�豸ID		          
        }
        
    }
    //============zxl===
     function ServerOndblclick_CST_Issue_Base(jsonstr)
    {
    //=========zxl====
        if(jsonstr != null)
	{
	    
		var record=jsonstr.record;
				    
		for(var i=0; i < record.length; i++)
		{
		    window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtEqu").value=record[i].name;//�豸����
		    window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidEquName").value=record[i].name; //�豸����
		    window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidEqu").value=record[i].id;  //�豸ID
		    window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtListName").value=record[i].listname;   //�ʲ�Ŀ¼����
		    window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidListName").value=record[i].listname;   //�ʲ�Ŀ¼����
		    window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidListID").value=record[i].listid;  //�ʲ�Ŀ¼ID
		  
		}
	}
        
    //=========zxl===
      //  window.parent.returnValue = jsonstr;
        top.close();
    }
    
    
    function ServerOndblclickChangeBase(jsonstr)
    {
    //=========zxl====
        if(jsonstr != null)
	{
	    
		var record=jsonstr.record;
				    
		for(var i=0; i < record.length; i++)
		{
		    window.opener.document.getElementById("<%=Opener_ClientId%>hidAddID").value=record[i].id;
		    window.opener.document.getElementById("<%=Opener_ClientId %>txtAddEquName").value=record[i].name;
		    window.opener.document.getElementById("<%=Opener_ClientId %>hidAddEquName").value=record[i].name;
		    window.opener.document.getElementById("<%=Opener_ClientId %>lblAddCode").innerHTML=record[i].code;
		    window.opener.document.getElementById("<%=Opener_ClientId %>hidAddCode").value=record[i].code;
		    window.opener.document.getElementById("<%=Opener_ClientId %>lblDept").innerHTML=record[i].partbranchname;
		   
		}
	}
        
    //=========zxl===
      //  window.parent.returnValue = jsonstr;
        top.close();
    }
    function cst_issue_Base_fastServe(jsonstr)
    {
        if(jsonstr !=null)
        {
             var record=jsonstr.record;
    
			if(jsonstr.record.length>0)
			{
			     window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value=record[0].name; //�豸����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value=record[0].name;//�豸����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value=record[0].id; //�豸ID			     
			}
        }
		else
		{
		    window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value=""; //�豸����
		    window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value=""; //�豸����
		    window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value=0; //�豸ID
		    
		}
        top.close();
    }
    //����Ӧ�ĸ����帳ֵ zxl==2012.8.7
    function CTS_Issue_Base_Self(jsonstr)
    {
        //=============
        if(jsonstr != null)
		{
			
		    var record=jsonstr.record;
		    
			for(var i=0; i < record.length; i++)
			{	
			     window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value=record[0].name;	//�豸����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value=	record[i].name;//�豸����
			      window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value=record[i].id;//�豸����
			       window.opener.document.getElementById("<%=Opener_ClientId%>txtListName").value=	record[i].listname;//�豸����
			        window.opener.document.getElementById("<%=Opener_ClientId%>hidListName").value=	record[i].listname;//�豸����
			        window.opener.document.getElementById("<%=Opener_ClientId%>hidListID").value=	record[i].listid;//�豸����		
			}
		}
		else
		{
		     window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value="";//�豸����
		     window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value=""; //�豸����
		     window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value="-1"; //�豸ID
		     
		}
        //=============
        top.close();
    }
    
    function ServerOndblclickfrmProblemSolve(jsonstr)
    {
    
         if(jsonstr != null)
        {
            var record=jsonstr.record;
            
	        for(var i=0; i < record.length; i++)
	        {
	            window.opener.document.getElementById("<%=Opener_ClientId %>txtEqu").value=record[i].name; //�豸����
	            window.opener.document.getElementById("<%=Opener_ClientId %>hidEquName").value=record[i].name;
	            window.opener.document.getElementById("<%=Opener_ClientId %>hidEqu").value=record[i].id;
	            window.opener.document.getElementById("<%=Opener_ClientId %>txtListName").value=record[i].listname;//�ʲ�Ŀ¼����
	            window.opener.document.getElementById("<%=Opener_ClientId %>hidListName").value=record[i].listname;//�ʲ�Ŀ¼����
	            window.opener.document.getElementById("<%=Opener_ClientId %>hidListID").value=record[i].listid;//�ʲ�Ŀ¼id
    	        					
	        }
        }
        else
        {
            window.opener.document.getElementById("<%=Opener_ClientId %>txtEqu").value="";//�豸ID
            window.opener.document.getElementById("<%=Opener_ClientId %>hidEquName").value="";//�豸����
            window.opener.document.getElementById("<%=Opener_ClientId %>hidEqu").value=0;//�豸ID
            	          
        }
        top.close();

    }
    
     //����Ӧ�ĸ����帳ֵ ����ǰ 2013-04-27
    function DemandBase(jsonstr)
    {
        //=============
        if(jsonstr != null)
		{
			
		    var record=jsonstr.record;
		    
			for(var i=0; i < record.length; i++)
			{	
			     window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value=record[0].name;	//�ʲ�����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value=	record[i].name;//�ʲ�����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value=record[i].id;//�ʲ�ID			       
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidListName").value=	record[i].listname;//�ʲ�Ŀ¼����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidListID").value=	record[i].listid;//�ʲ�Ŀ¼ID
			}
		}
		else
		{
			     window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value="";	//�ʲ�����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value="";//�ʲ�����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value="0";//�ʲ�ID			       
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidListName").value="";//�ʲ�Ŀ¼����
			     window.opener.document.getElementById("<%=Opener_ClientId%>hidListID").value="0";//�ʲ�Ŀ¼ID
		     
		}
        //=============
        top.close();
    }
    
    
    
function checkAll(checkAll) {
        var len = document.forms[0].elements.length;
        var cbCount = 0;
        for (i = 0; i < len; i++) {
            if (document.forms[0].elements[i].type == "checkbox") {
                if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgEqu_Desk") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                    document.forms[0].elements[i].checked = checkAll.checked;

                    cbCount += 1;
                }
            }
        }
    } 
 function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              //var className;
              //var objectFullName;
              var tableCtrl;
              //objectFullName = <%=tr2.ClientID%>.id;
              //className = objectFullName.substring(0,objectFullName.indexOf("tr2")-1);
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
        
        function ShowTable2(imgCtrl)
        {
            var ImgPlusScr = "../Images/icon_expandall.gif";      	// pic Plus  +
            var ImgMinusScr = "../Images/icon_collapseall.gif";     // pic Minus - 
            var TableID = imgCtrl.id.replace("Img", "Table");

            var tableCtrl;
            tableCtrl = document.all.item(TableID);

            if (imgCtrl.src.indexOf("icon_expandall") != -1) {
                tableCtrl.style.display = "";
                imgCtrl.src = ImgMinusScr;
                document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id;
            }
            else {
                tableCtrl.style.display = "none";
                imgCtrl.src = ImgPlusScr;
                document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
            }
        }
        
        function DeleteValidate()
        {
            var DgView = document.getElementById("<%=dgEqu_Desk.ClientID %>");
            var strEuqID = "";
            if(DgView)  
            {
                for(var j=1;j<DgView.rows.length;j++)
                {
                    chkdel = DgView.rows[j].childNodes[0].childNodes[0];
                    if(chkdel.checked){
                        for(i=0;i<DgView.rows[j].childNodes[0].childNodes.length;i++)
                        {
                            node=DgView.rows[j].childNodes[0].childNodes[i];
                            if(node.id=="hidID"&&node.type=="hidden")
                            {
                                strEuqID +=node.defaultValue + ",";
                            }
                        }   
                        
                    }  
                }
            }
            
            var result =false;
             
            $.ajax({
                 type: 'POST', //URL��ʽΪPOST
                 url: '../Ajax/HttpAjax.ashx',
                 async:false,
                 data: 'Type=Equ&strEquIDs=' + strEuqID,
                 dataType: 'text', //��������ΪJSON��ʽ����֤ 
                 //�ڷ�������֮ǰҪ���еĺ��� 
                 beforeSend: function() {
                     //����   
                 },
                 success: function(data) {
                 if (data != "") {        
                         //sessionֵ����ɹ�
                        if(confirm(data))
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                 }else{
                    result = true;
                 }
                }});
                return result;
        }
        
        function OpenEquHistoryChart(obj)
           {
                
                var hidEuqID = document.getElementById(obj.id.replace("btnConfig","hidConfigID")).value;
                
                if($.browser.safari) { 
                    //alert('����Safari�İ�ȫ���ƣ��������ݲ�֧��Safari�����!'); 
                    openobj.open("frm_Equ_HistoryChartView_SVG.aspx?newWin=true&id=" + hidEuqID,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
                    event.returnValue = false;
                    return;
                 }
                
                openobj.open("frm_Equ_HistoryChartView.aspx?newWin=true&id=" + hidEuqID,'',"scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
                event.returnValue = false;
           } 
    //Ӱ��ȷ���

    function ImpactAnalysis(obj)
    {
        
        var lngID = document.getElementById(obj.id.replace("btnImpactAnalysis","hidAnalyID")).value;
        openobj.open("frmEqu_ImpactAnalysis.aspx?EquId=" + lngID +"&randomid=" + GetRandom(),"","scrollbars=yes,status=yes ,resizable=yes,width=800,height=560");
        event.returnValue = false;
    }
    </script>

    <input id="hidIsTip" runat="server" type="hidden" />
    <input id="hidstrID" runat="server" type="hidden" />
    <input id="hidEquipmentCatalogID" type="hidden" value="0" runat="server" />
    <table cellpadding='2' cellspacing='0' width='98%' class="listContent GridTable">
        <tr style="display: none">
            <td>
                <asp:Literal ID="Equ_ListName" runat="server" Text="�ʲ�Ŀ¼"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlListName" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="Equ_CatalogName" runat="server" Text="�ʲ����"></asp:Literal>&nbsp;
            </td>
            <td class='list' colspan="3">
                <uc2:ctrEquCataDropList ID="CtrEquCataDropList1" Width="152px" runat="server" RootID="1" />
                <asp:CheckBox ID="chkIncludeSub" Text="��������" Checked="true" runat="server" />
            </td>
        </tr>
        </tr>
        <tr>
            <td class="listTitleRight" style='width: 12%;'>
                <asp:Literal ID="Equ_Code" runat="server" Text="�ʲ����"></asp:Literal>
            </td>
            <td class='list' style="width: 35%">
                <asp:TextBox ID='txtCode' runat='server'></asp:TextBox>
            </td>
            <td class="listTitleRight" style='width: 12%;'>
                <asp:Literal ID="Equ_DeskName" runat="server" Text="����"></asp:Literal>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtName' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style='width: 12%;'>
                <asp:Literal ID="Equ_CostomName" runat="server" Text="�ͻ�����"></asp:Literal>
            </td>
            <td class='list' style="width: 35%">
                <asp:TextBox ID='txtCust' runat='server'></asp:TextBox>
            </td>
            <td class="listTitleRight">
              <asp:Literal ID="Equ_PartBankName" runat="server"  Text="ά������"></asp:Literal>
            </td>
            <td class="list">
                <uc12:DeptPickerbank ID="DeptPickerbank2" runat="server" TextToolTip="ά������" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="Equ_EquStatusName" runat="server" Text="�ʲ�״̬"></asp:Literal>
            </td>
            <td class='list'>
                <uc6:ctrFlowCataDropList ID="ctrCataEquStatus" runat="server" RootID="1018" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="Equ_MastCust" runat="server" Text="����λ"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddltMastCustID" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table id="Table13" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img3" onclick="ShowTable2(this);" height="16" src="../Images/icon_expandall.gif"
                                width="16" align="absbottom" runat="server" />
                            ��������
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table cellpadding='2' cellspacing='0' width='98%' class="listContent" runat="server"
        id="Table3" style="display: none">
    </table>
    <table id="Table12" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable2(this);" height="16" src="../Images/icon_expandall.gif"
                                width="16" align="absbottom" runat="server" />
                            �߼�����
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table cellpadding='2' cellspacing='0' width='98%' class="listContent" id="Table2"
        style="display: none" runat="server">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                ����������
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddlSchemaItemJB" runat="server" Width="150px">
                </asp:DropDownList>
                <asp:TextBox ID="txtItemValue" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight" style="width: 12%">
                ����������
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlSchemaItemGL" runat="server" Width="152px">
                </asp:DropDownList>
                <asp:CheckBox ID="chkItemValue" Text="�Ƿ�����" Checked="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                ����������
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlPulldownItem" runat="server" Width="150px" OnSelectedIndexChanged="ddlPulldownItem_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="ddlPulldownValue" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight">
                ����������
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlSchemaItemDT" runat="server" Width="152px">
                </asp:DropDownList>
                <uc3:ctrdateandtime ID="dtServiceTime" runat="server" ShowTime="false" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                ����������
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlSchemaItemDP" runat="server" Width="150px">
                </asp:DropDownList>
                <uc9:DeptPicker ID="DeptPicker1" runat="server" />
            </td>
            <td class="listTitleRight">
                ��Ա������
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlSchemaItemUS" runat="server" Width="152px">
                </asp:DropDownList>
                <uc10:UserPicker ID="UserPicker1" runat="server" />
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgEqu_Desk" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable" OnItemCommand="dgEqu_Desk_ItemCommand" OnItemDataBound="dgEqu_Desk_ItemDataBound"
                    OnItemCreated="dgEqu_Desk_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox><input type="hidden" id="hidID"
                                    value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEquName" runat="server" OnClientClick='<%#GetEquDetail((decimal)DataBinder.Eval(Container,"DataItem.ID")) %>'
                                    Text='<%#DataBinder.Eval(Container,"DataItem.Name") %>' Visible="true"></asp:LinkButton>
                                <asp:Label ID="lblEquName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.Name") %>'
                                    Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='Code' HeaderText='����' ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='costomname' HeaderText='�����ͻ�' ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='CatalogName' HeaderText='�ʲ�����' ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='SerialNumber' HeaderText='SN' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='CatalogName' HeaderText='�ʲ�����' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Breed" HeaderText="Ʒ��" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Model" HeaderText="�ͺ�" Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="�޸�">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="�޸�" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="60"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="���û���">
                            <ItemTemplate>
                                <asp:Button ID="btnConfig" SkinID="btnClass1" Width="60" runat="server" Text="���û���"
                                    OnClientClick="OpenEquHistoryChart(this);" />
                                <input type="hidden" runat="server" id="hidConfigID" value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="60"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="false" HeaderText="����">
                            <ItemTemplate>
                                <asp:Button ID="lnkLook" SkinID="btnClass1" runat="server" Text="����" CommandName="look" />
                            </ItemTemplate>
                            <HeaderStyle Width="60"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="�����ʲ�">
                            <ItemTemplate>
                                <asp:Button ID="lnklink" Width="60" SkinID="btnClass1" runat="server" Text="�����ʲ�"
                                    CommandName="link" />
                            </ItemTemplate>
                            <HeaderStyle Width="60"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="�¼���¼" Visible="false">
                            <ItemTemplate>
                                <input type="hidden" runat="server" id="hidID" value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                                <asp:Button ID="CmdService" Width="60" runat="server" Text="�¼���¼" OnClientClick="SelectService(this);"
                                    SkinID="btnClass1" CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle Width="60" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Ӱ�����">
                            <ItemTemplate>
                                <asp:Button ID="btnImpactAnalysis" Width="60" runat="server" Text="Ӱ�����" OnClientClick="ImpactAnalysis(this);"
                                    SkinID="btnClass1" CausesValidation="false" />
                                <input type="hidden" runat="server" id="hidAnalyID" value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="60" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc4:ControlPageFoot ID="cpfECustomerInfo" runat="server" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">	
             
             
    if(typeof(document.all.<%=hidTable.ClientID%>) != "undefined")
    {
        var temp = document.all.<%=hidTable.ClientID%>.value;
        var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
        var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus -
        if(temp!="")
        {
            var arr=temp.split(",");
            for(i=1;i<arr.length;i++)
            {
                var tableid=arr[i];
                var tableCtrl = document.all.item(tableid);
                tableCtrl.style.display ="";
                var ImgID = tableid.replace("Table","Img");
                var imgCtrl = document.all.item(ImgID)
                if(typeof(imgCtrl) != "undefined"&&imgCtrl!=null)
                    imgCtrl.src = ImgMinusScr ;	
            }
        }
        else
        { 
            
            var tableid = '<%=Table2.ClientID%>';
            var tableCtrl = document.all.item(tableid);
            var ImgID =tableid.replace("Table","Img");
            var imgCtrl = document.all.item(ImgID)
            tableCtrl.style.display ="none";
            if(typeof(imgCtrl) != "undefined"&&imgCtrl!=null)
                imgCtrl.src = ImgPlusScr ;	
        }
    }
    </script>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="744px"></asp:ValidationSummary>
</asp:Content>
