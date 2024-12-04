<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceStaffMastCust.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.ServiceStaffMastCust" %>

<script>
    //维修人员选择
    function SelectServiceStaffMastCust(obj) {
  
        var url = "../CustManager/frmCst_ServiceStaffMain.aspx?IsSelect=true&flowid=" 
                + document.getElementById(obj.id.replace("cmdPopUser", "hidFlowID")).value 
                + "&CustID=" + document.getElementById(obj.id.replace("cmdPopUser", "hidCustID")).value 
                + "&EquID=" + document.getElementById(obj.id.replace("cmdPopUser", "hidEquID")).value 
                + "&ServiceTypeID=" + document.getElementById(obj.id.replace("cmdPopUser", "hidServiceTypeID")).value 
                + "&ServiceLevelID=" + document.getElementById(obj.id.replace("cmdPopUser", "hidServiceLevelID")).value 
                + "&MastCustName=" + document.getElementById(obj.id.replace("cmdPopUser", "hidMastCustName")).value 
                + "&EquName=" + document.getElementById(obj.id.replace("cmdPopUser", "hidEquName")).value
                + "&CustName=" + document.getElementById(obj.id.replace("cmdPopUser", "hidCustName")).value
                + "&TypeName=" + document.getElementById(obj.id.replace("cmdPopUser", "hidTypeName")).value
                + "&LevelName=" + document.getElementById(obj.id.replace("cmdPopUser", "hidLevelName")).value
                + "&randomid=" + GetRandom()
                + "&RequestType=<%= GetRequestType()  %>" 
                +"&TypeFrm=ServiceStaffMastCust"
                + "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
        open(encodeURI(url), 'SelectEngineer', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=600,height=500px,left=150,top=50');
    }
</script>

<script type="text/javascript">



    var engineerJson=<%=EngineerJsonString %>;
	$(function() {

	    $("#<%=txtUser.ClientID %>").autocomplete(engineerJson, options);

	    $("#<%=txtUser.ClientID %>").result(function(event, data, formatted) {
	        $("#<%=hidUser.ClientID %>").val(data.id);
	        $("#<%=hidUserName.ClientID %>").val(data.name);
	    });
	    
	     $("#ctl00$cmdMsgProcess").click(function() {
	     if (!checkSelectEngineer())
	     {
	        return false;
	     }	       
	    });
	    
	    $("#ctl00$cmdHiddenSave").click(function() {
	         if (!checkSelectEngineer())
	         {
	            return false;
	         }
	    });
	    
	    $("#ctl00$cmdPauseFlow").click(function() {
	         if (!checkSelectEngineer())
	         {
	            return false;
	         }     
	    });
	    

	    
	    $("#ctl00_cmdHidden").click(function() {
	         if (!checkSelectEngineer())
	         {	           
	            return false;
	         }     
	    });
	
	});

    function checkSelectEngineer()
    {
        if ($("#<%=txtUser.ClientID %>").val() =="")
        {
            $("#<%=hidUser.ClientID %>").val("");
	        $("#<%=hidUserName.ClientID %>").val("");
	        return true;
        }
       // debugger;
        if ($("#<%=txtUser.ClientID %>").val() ==$("#<%=hid_txtUser.ClientID %>").val() || ($("#<%=hid_txtUser.ClientID %>").val() ==""))
        {
        return true;
        }
        else
        {
         alert("您未正确输入处理的工程师。" );
        return false ;
        }
    }

	var options =
        {
            minChars: 1,
            matchCase: false, //不区分大小写
            autoFill: false,
            max: 10,
            formatItem: function(row, i, max, term) {
                return row.name;
            },
            formatMatch: function(row, i, max) {
                return row.name;
            },
            formatResult: function(row) {
                return row.name;
            },
            reasultSearch: function(row, v)//本场数据自定义查询语法 注意这是我自己新加的事件
            {
                //自定义在code或firstword中匹配
                if (row.data.firstword.indexOf(v) == 0) {
                    return row;
                }
                else {
                    return false;
                }
            }
        };
        
        function clearSelectedEngineer()
        {
            if ($("#<%=txtUser.ClientID %>").val() =="")
            {
                $("#<%=hidUser.ClientID %>").val("");
	            $("#<%=hidUserName.ClientID %>").val("");
	            return true;
            }
        }


</script>

<asp:Label ID="labUser" runat="server" Visible="False"></asp:Label>
<asp:TextBox ID="txtUser"  runat="server" MaxLength="80" onblur="javascript:clearSelectedEngineer();" ></asp:TextBox>&nbsp;

<input id="cmdPopUser" runat="server" onclick="SelectServiceStaffMastCust(this)" type="button"  value="..." class="btnClass2" />

<input id="hid_txtUser" runat="server" name="hidUser" type="hidden" />   
<input id="hidUser" runat="server" name="hidUser" type="hidden" />        
<input id="hidUserName" runat="server" name="hidUser"  type="hidden" />

<input id="hidFlowID" runat="server" name="hidFlowID"   type="hidden" />
<input id="hidCustID" runat="server" name="hidCustID"   type="hidden" />
<input id="hidEquID" runat="server" name="hidEquID" type="hidden" />
<input id="hidServiceTypeID" runat="server" name="hidServiceTypeID" type="hidden" />
<input id="hidServiceLevelID" runat="server" name="hidServiceLevelID" type="hidden" />
<input id="hidMastCustName" runat="server" name="hidMastCustName" type="hidden" />
<input id="hidEquName" runat="server" name="hidEquName" type="hidden" />
<input id="hidCustName" runat="server" name="hidCustName" type="hidden" />
<input id="hidLevelName" runat="server" name="hidLevelName" type="hidden" />
<input id="hidTypeName" runat="server" name="hidTypeName" type="hidden" />

<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />