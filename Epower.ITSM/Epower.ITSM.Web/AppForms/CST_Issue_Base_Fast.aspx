<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>

<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.CST_Issue_Base_Fast" CodeBehind="CST_Issue_Base_Fast.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>���ٵǵ�</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

    <script language="javascript" type="text/javascript" src="../Js/jquery-1.3.2.js"> </script>

    <script language="javascript" type="text/javascript" src="../Js/jquery-ui-1.7.2.custom.min.js"> </script>

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script language="javascript" src="../Js/Common.js"></script>

    <style type="text/css">
        .listContent
        {
            width: 99%;
        }
    </style>
</head>

<script type="text/javascript">
        $(function() {
            //����ƶ�ʱ�л�TABS
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
                            if (xmlhttp.responseText == "-1")  //û��
                            {
                                document.getElementById(obj.id.replace("txtCustAddr", "hidCustID")).value= "0";//�ͻ�ID
                                document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr") ).value="";//�ͻ�����
                                alert("�˿ͻ������ڣ�");
                            }
                            else if (xmlhttp.responseText == "0") //�ҵ����
                            {
                                SelectSomeCust(obj);
                            }
                            else  //�ҵ�Ψһ
                            {

                                var sreturn = xmlhttp.responseText;

                                arr = sreturn.split(",");                                
                                document.getElementById(obj.id.replace("txtCustAddr", "txtCustAddr")).value = arr[1];   //����	 
                                document.getElementById(obj.id.replace("txtCustAddr", "hidCust")).value = arr[1];   //����                                               
                                document.getElementById(obj.id.replace("txtCustAddr", "hidCustID")).value = arr[0];    //�ͻ�ID��


                                document.getElementById(obj.id.replace("txtCustAddr", "txtEqu")).value = arr[9];   //�豸����
                                document.getElementById(obj.id.replace("txtCustAddr", "hidEquName")).value = arr[9];   //�豸����
                                document.getElementById(obj.id.replace("txtCustAddr", "hidEqu")).value = arr[8];  //�豸IDontact")).id);
                            }
                        }
                    }
                    xmlhttp.send(null);
                } catch (e3) { }
            }
        }
        
        String.prototype.trim = function()  //ȥ�ո�

			{
				// ��������ʽ��ǰ��ո�

				// �ÿ��ַ��������

				return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;","");
			}
//�ͻ�ѡ��
			function CustomSelect(obj) 
			{
			    var CustName = document.all.<%=txtCustAddr.ClientID%>.value;			    
			    var url="../Common/frmDRMUserSelect.aspx?IsSelect=true&CustID=" + document.getElementById(obj.id.replace('cmdCust','hidCustID')).value + "&FlowID=0&CustName=" + escape(CustName)+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
			    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50");			    
			}
			
			function SelectSomeCust(obj)   //ѡ�����ͻ�
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
				        document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = value[1];   //�ͻ�
			            document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = value[1];
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = value[4];    //�ͻ�ID��

			            
	                    document.getElementById(obj.id.replace("txtCustAddr","txtEqu")).value = value[7];   //�豸����
                        document.getElementById(obj.id.replace("txtCustAddr","hidEquName")).value = value[7];   //�豸����
                        document.getElementById(obj.id.replace("txtCustAddr","hidEqu") ).value = value[6];  //�豸IDontact")).id);
					}
				}
			}
			
			
			    
            function SelectSomeEqu(obj)   //ѡ�����豸
			{
			    var newDateObj = new Date();
	            var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
	            var name = obj.value.trim();
	            if(name.trim()=="")
	            {
	                return;
	            }
	            var EquipmentCatalogID = 0;
	            var CustName ="";
				var	value=window.showModalDialog("../mydestop/frmQuickLocateEqu.aspx?IsSelect=true&Name=" + escape(name) + "&EquCust=" + escape(CustName)+"&EquipmentCatalogID="+EquipmentCatalogID,"","dialogHeight:500px;dialogWidth:600px");
				if(value != null)
				{
					if(value.length>1)
					{
				        document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = value[2];   //�豸����
                        document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = value[2];   //�豸����
                        document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = value[1];  //�豸IDontact")).id);                                                
					}
					else
					{
					    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = "";   //�豸����
                        document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = "";   //�豸����
                        document.getElementById(obj.id.replace("txtEqu","hidEqu")).value = "0";  //�豸IDontact")).id);                                                                   
					}
				}
				else
				{
				    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = document.getElementById(obj.id.replace("txtEqu","hidEquName")).value;   //�豸����
				}
			}
			
        function GetEquID(obj)
        {
            var EquipmentCatalogID = 0;
            if(obj.value.trim()=="")
	            {
	                document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = "0";  //�豸IDontact")).id);
	                return;
	            }
	        if(obj.value.trim()==document.getElementById(obj.id.replace("txtEqu","hidEquName")).value.trim() && (document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value.trim()!="" || document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value.trim()!="0"))
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
					xmlhttp.open("GET", "../MyDestop/frmXmlHttpAjax.aspx?Equ=" + escape(obj.value), true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText=="-1")  //û��
														{
														    alert("���ʲ������ڣ������²��ң�"); 
														    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value="";
														    document.getElementById(obj.id.replace("txtEqu","hidEqu")).value=0;
														    obj.focus();
														    obj.select();
													    }
													    else if(xmlhttp.responseText=="0") //�ҵ����
													    {
													        SelectSomeEqu(obj);
													    }
													    else  //�ҵ�Ψһ
													    {
													        
													        var json= eval("("+xmlhttp.responseText+")");
				                                            var record=json.record;
				                                            
				                                            for(var i=0; i < record.length; i++)
					                                        {
													        
													            document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = record[i].name;   //�豸����
			                                                    document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = record[i].name;   //�豸����
			                                                    document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = record[i].id;  //�豸IDontact")).id);			                                                    
			                                                }
													    }
													} 
												} 
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
			
			//�豸
			function SelectEqu(obj) 
			{
			    var EquName = document.all.<%=txtEqu.ClientID%>.value.trim();
			    var CustName = "";
			    var url="../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&FlowID=0&EquName=" + escape(EquName) + "&Cust=" + escape(CustName)
			             + "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=CST_Issue_Base_Fast" ;
			    open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50');				
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

<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hidClientId_ForOpenerPage" runat="server" />
    <table class="listContent">
        <tr>
            <td align="right" class="listTitle">
                �¼�ģ��:
            </td>
            <td align="left" class="list">
                <asp:Label ID="labModelName" runat="server" Text="Label"></asp:Label>
                <asp:TextBox ID="txtModelName" CssClass="bian" runat="server" Height="24px" Width="178px"
                    ReadOnly="True" Visible="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <asp:Literal ID="LitCustName" runat="server" Text="�ͻ�����:"></asp:Literal>
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtCustAddr" runat="server" CssClass="bian" MaxLength="200" onblur="GetCustID(this)"
                    Width="152px" onkeydown="if (event.keyCode==13){event.keyCode=9}"></asp:TextBox>
                <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                    class="btnClass2" />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <asp:Literal ID="LitEquipmentName" runat="server" Text="�ʲ�����:"></asp:Literal>
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtEqu" CssClass="bian" runat="server" Width="151px" MaxLength="80"  onblur="GetEquID(this)"
                    onkeydown="if (event.keyCode==13){event.keyCode=9}"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <asp:Literal ID="LitContent" runat="server" Text="��ϸ����:"></asp:Literal>
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtContext" CssClass="bian" runat="server" Height="170px" TextMode="MultiLine"
                    Width="393px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle" colspan="2">
                <asp:Button ID="cmdOK" runat="server" OnClick="cmdOK_Click" Text="ȷ��" CssClass="btnClass" />
                <br />
                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    <input id="hidCust" runat="server" type="hidden" /><input id="hidCustID" runat="server"
        type="hidden" value="0" />
    <input id="hidEqu" type="hidden" runat="server" value="0" /><input id="hidEquName"
        type="hidden" runat="server" />
    <asp:DropDownList ID="ddlTemplaties" runat="server" Width="130px" CssClass="bian"
        Visible="false">
    </asp:DropDownList>
    </form>
</body>
</html>
