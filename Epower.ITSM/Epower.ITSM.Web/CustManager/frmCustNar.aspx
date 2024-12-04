<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCustNar.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmCustNar" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../Js/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="../Js/jquery-ui-1.7.2.custom.js" type="text/javascript"></script>

</head>
<body style=" margin:0px;">
    <form id="form1" runat="server">

    <script type="text/javascript">
        $(function() {
            //鼠标移动时切换TABS
            $("#tabs").tabs({ event: 'mouseover',
                effect: 'ajax', cache: true,
                show: function(event, ui) {
                    $("#" + ui.panel.id).fadeIn("slow");
                },
                select: function(event, ui) {
                    $("#" + ui.panel.id).hide();
                }

            });
        });

        var xmlhttp = null;
        function CreateXmlHttpObject() {
            try {
                xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");
            }
            catch (e) {
                try {
                    xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e2) { }
            }
            return xmlhttp;
        }

        function GetCustID(obj) {
            if (obj.value.trim() == "") {
                document.getElementById(obj.id.replace("txtCustAddr", "hidCustID")).value.trim() != "0";
                return;
            }
            if (obj.value.trim() == document.getElementById(obj.id.replace("txtCustAddr", "hidCust")).value.trim() && (document.getElementById(obj.id.replace("txtCustAddr", "hidCustID")).value.trim() != "" || document.getElementById(obj.id.replace("txtCustAddr", "hidCustID")).value.trim() != "0")) {
                return;
            }
            if (xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();
            if (xmlhttp != null) {
                try {
                    xmlhttp.open("GET", "../MyDestop/frmXmlHttp.aspx?Cust=" + escape(obj.value), true);
                    xmlhttp.setRequestHeader("CONTENT-TYPE ", "application/x-www-form-urlencoded ");
                    xmlhttp.onreadystatechange = function() {
                        if (xmlhttp.readyState == 4) {
                            if (xmlhttp.responseText == "-1")  //没有
                            {
                                document.getElementById(obj.id.replace("txtCustAddr", "hidCustID")).value.trim() != "0"
                                alert("此客户不存在！");
                            }
                            else if (xmlhttp.responseText == "0") //找到多个
                            {
                                SelectSomeCust(obj);
                            }
                            else  //找到唯一
                            {

                                var sreturn = xmlhttp.responseText;

                                arr = sreturn.split(",");
                                //alert(arr[0]);
                                document.getElementById(obj.id.replace("txtCustAddr", "txtCustAddr")).value = arr[1];   //名称	 
                                document.getElementById(obj.id.replace("txtCustAddr", "hidCust")).value = arr[1];   //名称                                               
                                document.getElementById(obj.id.replace("txtCustAddr", "hidCustID")).value = arr[0];    //客户ID号

                                document.getElementById(obj.id.replace("txtCustAddr", "txtEqu")).value = arr[9];   //设备名称
                                document.getElementById(obj.id.replace("txtCustAddr", "hidEquName")).value = arr[9];   //设备名称
                                document.getElementById(obj.id.replace("txtCustAddr", "hidEqu")).value = arr[8];  //设备IDontact")).id);
                            }
                        }
                    }
                    xmlhttp.send(null);
                } catch (e3) { }
            }
        }
        
        String.prototype.trim = function()  //去空格
			{
				// 用正则表达式将前后空格
				// 用空字符串替代。
				return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;","");
			}
//客户选择
			function CustomSelect(obj) 
			{
			    var CustName = document.all.<%=txtCustAddr.ClientID%>.value;
				var	value=window.showModalDialog("../Common/frmDRMUserSelect.aspx?IsSelect=true&CustID=" + document.getElementById(obj.id.replace('cmdCust','hidCustID')).value + "&FlowID=0&CustName=" + escape(CustName) ,window,"dialogHeight:600px;dialogWidth:800px");
				if(value != null)
				{
					if(value.length>1)
					{
			            document.getElementById(obj.id.replace("cmdCust","txtCustAddr")).value = value[1];   //客户
			            document.getElementById(obj.id.replace("cmdCust","hidCust") ).value = value[1];
			            document.getElementById(obj.id.replace("cmdCust","hidCustID")).value = value[4];    //客户ID号
			            
	                    document.getElementById(obj.id.replace("cmdCust","txtEqu")).value = value[9];   //资产名称
                        document.getElementById(obj.id.replace("cmdCust","hidEquName")).value = value[9];   //资产名称
                        document.getElementById(obj.id.replace("cmdCust","hidEqu") ).value = value[8];  //资产IDontact")).id);
					}
				}
				else
				{
				    document.getElementById(obj.id.replace("cmdCust","txtCustAddr")).value = "";   //客户
		            document.getElementById(obj.id.replace("cmdCust","hidCust") ).value = "";
		            document.getElementById(obj.id.replace("cmdCust","hidCustID")).value = 0;    //客户ID号

				}
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
				var	value=window.showModalDialog("../mydestop/frmQuickLocateCust.aspx?Name=" + escape(name),"","dialogHeight:500px;dialogWidth:700px");
				if(value != null)
				{
					if(value.length>1)
					{
				        document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = value[1];   //客户
			            document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = value[1];
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = value[4];    //客户ID号
			            
	                    document.getElementById(obj.id.replace("txtCustAddr","txtEqu")).value = value[7];   //设备名称
                        document.getElementById(obj.id.replace("txtCustAddr","hidEquName")).value = value[7];   //设备名称
                        document.getElementById(obj.id.replace("txtCustAddr","hidEqu") ).value = value[6];  //设备IDontact")).id);
					}
				}
			}
			
			//设备
			function SelectEqu(obj) 
			{
			    var EquName = document.all.<%=txtEqu.ClientID%>.value.trim();
			    var CustName = document.all.<%=hidCust.ClientID%>.value.trim();
				var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&FlowID=0&EquName=" + escape(EquName) + "&Cust=" + escape(CustName),window,"dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
				if(value != null)
				{
					if(value.length>1)
					{
			            document.getElementById(obj.id.replace("cmdEqu","txtEqu")).value = value[1];   //设备名称
			            document.getElementById(obj.id.replace("cmdEqu","hidEquName")).value = value[1];   //设备名称
			            document.getElementById(obj.id.replace("cmdEqu","hidEqu") ).value = value[0];  //设备ID
					}
				}
				else
				{
				    document.getElementById(obj.id.replace("cmdEqu","txtEqu")).value = "";   //设备名称
		            document.getElementById(obj.id.replace("cmdEqu","hidEquName")).value = "";   //设备名称
		            document.getElementById(obj.id.replace("cmdEqu","hidEqu") ).value = 0;  //设备ID
				}
			}
			
			function next() { if (event.keyCode==13) event.keyCode=9;} 
			
			function DoShortCutIssueReq() {
           var temp = document.all.<%=ddlTemplaties.ClientID%>.value;
           var flowmodelid= '0';
           var templateid = '0';
           flowmodelid = temp.substring(temp.indexOf("|")+1,10);
           templateid = temp.substring(0,temp.indexOf("|"));
           if(flowmodelid!="0")
           {
                window.open("../Forms/oa_AddNew.aspx?flowmodelid=" + flowmodelid  + "&IsFirst=true&ep=" + templateid,"MainFrame","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
                event.returnValue = true;
           }
           else
           {
                event.returnValue = false;
           }
        }
    </script>

    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">事件跟踪</a></li>
            <li><a href="#tabs-2">新事件</a></li>
        </ul>
        <div id="tabs-1">
            <iewc:TreeView ID="tvCatalog" runat="server" Height="405"></iewc:TreeView>
        </div>
        <div id="tabs-2">
            <table width="140px" border="0" cellspacing="0" cellpadding="0" bgcolor="#EEF5FB">
                <tr>
                    <td align="left" valign="top" class="bian">
                        <table width="100%">
                            <tr>
                                <td valign="top">
                                    事件模版:
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlTemplaties" runat="server" Width="130px" CssClass="bian">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal ID="LitCustName" runat="server" Text="客户名称:"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCustAddr" runat="server" CssClass="bian" MaxLength="200" onblur="GetCustID(this)"
                                        Width="100px" onkeydown="if (event.keyCode==13){event.keyCode=9}"></asp:TextBox>
                                    <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                                        class="btnClass" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称:"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtEqu" CssClass="bian" runat="server" Width="100px" MaxLength="80" onkeydown="if (event.keyCode==13){event.keyCode=9}"></asp:TextBox>
                                    <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                                        name="cmdEqu" class="btnClass" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal ID="LitSubject" runat="server" Text="事件标题:"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtSubject" CssClass="bian" runat="server" Width="110px" onkeydown="if (event.keyCode==13){event.keyCode=9}"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="LitContent" runat="server" Text="详细描述:"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtContext" CssClass="bian" runat="server" Height="56px" TextMode="MultiLine"
                                        Width="130px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="cmdOK" runat="server" OnClick="cmdOK_Click" Text="确定" CssClass="btnClass" />
                                </td>
                            </tr>
                        </table>
                        <input id="hidCust" runat="server" type="hidden" /><input id="hidCustID" runat="server"
                            type="hidden" value="0" />
                        <input id="hidEqu" type="hidden" runat="server" value="0" /><input id="hidEquName"
                            type="hidden" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
