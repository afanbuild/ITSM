<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmSelectStaffRight" Codebehind="frmSelectStaffRight.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>执行人员选择</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
		
		<style type="text/css">
            .TreeViewSelectedNode{
         background-color:Yellow;
         color:Red;	}
            .style1
            {
                width: 229px;
                border: 1px solid #eeeee00;
                
            }
            .style2
            {
                width: 43px;
            }
            .style3
            {
                width: 116px;
            }
        </style>
        

		
	</HEAD>
	<script language="javascript">
			function chkSelect_Click(obj)
			{
				reg=/chkSelect/g;
				var idtag=obj.id.replace(reg,"");
				if (obj.checked==true) {
				     // var userid=document.getElementById("hidUserID").value;
				    //  var username=document.getElementById("hidUserName").value;
					var userid=document.all(idtag+"hidUserID").value;
					var username=document.all(idtag+"hidUserName").value;
					
					window.parent.parent.document.all.hidUserID_Name.value=userid+"@"+username;
				}
				
				OnlySelectOne(obj);
			}
			function OnlySelectOne(obj)
			{
				for(i=0;i<document.all.length;i++)
				{
					var sid=document.all(i).id;
					if(sid!="")
					{
						if(sid.substr(sid.length-9,9)=="chkSelect")
						{
							document.all(i).checked=false;
						}
					}
				}
				obj.checked=true;				
			}
			function Click_Cancel()
			{
				//window.returnValue="@";
				window.close();
			}
			
			//==================
			function Click_OK()
			{
			    var type  ="<%=TypeFrm %>";
			    
			    var value=document.all.hidUserID_Name.value;
	
                if(type =="frmrightSeach")
                {
                    if(typeof(value) !="undefined" && value.length>1)
                    {
                        arr=value.split("@");
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value=arr[0];
                    }
                    else
                    {
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value="";
                    }                    
                       
                }
                if(type =="frmactormemberedit")
                {
                     if (value != null) {
	                    if (value.length > 1) {
	                        arr = value.split("@");
	                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1];
	                        window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectID").value=arr[0];
	                        
	                    }
	                }
                }
                if(type=="frmdeptedit")
                {   
                    if(value != "")
				    {
					    if(value.length>1)
					    {	
						    arr=value.split("@");
						      window.opener.document.getElementById("<%=Opener_ClientId %>txtLeaderName").value=arr[1];
						      window.opener.document.getElementById("<%=Opener_ClientId %>hidLeaderID").value =arr[0];
					    }
					    else
					    {
					          window.opener.document.getElementById("<%=Opener_ClientId %>txtLeaderName").value="超级管理员";
					          window.opener.document.getElementById("<%=Opener_ClientId %>hidLeaderID").value="1";
					    }
				    }      
                }
                if(type=="frmdeptedit_Manager")
                {
                    frmdeptedit_Manager(value);
                }
			    if(type=="frmeditright")
			    {
			        frmeditright(value);
			    }
			    if(type =="frmright")
			    {
			        frmright(value);
			    }
				window.close();
			
			}
			
			
			//==================
		    function frmdeptedit_Manager(value)
		    {
		        if(value != ""){
					if(value.length>1)
					{
						arr=value.split("@");
						window.opener.document.getElementById("<%=Opener_ClientId %>txtManagerName").value=arr[1];
						window.opener.document.getElementById("<%=Opener_ClientId %>hidMangerName").value=arr[1];
						window.opener.document.getElementById("<%=Opener_ClientId %>hidManagerID").value=arr[0];
					
					}
					else
					{
					    window.opener.document.getElementById("<%=Opener_ClientId %>txtManagerName").value="超级管理员";
						window.opener.document.getElementById("<%=Opener_ClientId %>hidMangerName").value= "超级管理员";
						window.opener.document.getElementById("<%=Opener_ClientId %>hidManagerID")=1;
						
					}
				}
				return true;
		    }
		    function frmeditright(value)
		    {
		        if(typeof(value) != "undefined" && value.length>1)
			    {
				    arr=value.split("@");
				    window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
				    window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectId").value =arr[0];
			    }
		    }
		    //zxl
		    function frmright(value)
		    {
		    
		    if(typeof(value) != "undefined" && value.length>1)
			{			
				arr=value.split("@");
//				window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value =arr[0];
//	
                /*     
                 * Date: 2012-8-6 16:34
                 * summary: 在 frmright.aspx 页面中增加一个控件名为: txtObjectID. 用于显示友好文本.
                 * 控件 txtObjectID 隐藏, 存储具体值.
                 * modified: sunshaozong@gmail.com     
                 */        
	            window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
                window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value=arr[0];
			}
			else
		    {
		      
		       window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value="";
		    }
		        
		    }
		   
			
		</script>
		
		 
	<body>
		<form id="Form1" method="post" runat="server">
		<%--这个要综合到一个页面里 zxl--%>
		
		  <div style=" margin: 0px 0px 0px 0px;">
<asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
          <table  style=" margin: 0px 0px 0px 0px;" >
				<tr>
					<td align="left" valign="top" class="style3" >
						<div style="width:240px;height:320px; overflow:scroll; border:1px solid; color:Silver;">
						<asp:TreeView ID="tvDept" runat="server" 
                            onselectednodechanged="tvDept_SelectedNodeChanged">  
                             <SelectedNodeStyle CssClass="TreeViewSelectedNode"  />                              
                            </asp:TreeView>
                           </div> 
					</td>
					<td class="style2"> &nbsp;</td>
					<td align="left" valign="top" class="style1" >
				          <asp:DataList id="dlUsers" runat="server" RepeatColumns="2" BorderColor="#999999" 
				            BackColor="White" BorderWidth="1px" RepeatDirection="Horizontal">
				            <ItemTemplate>
				                <asp:RadioButton ID="chkSelect" onclick="chkSelect_Click(this)"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
	                            <INPUT id="hidUserID" type=hidden value='<%# DataBinder.Eval(Container.DataItem, "UserID")%>' runat="server">
					            <INPUT type="hidden" id="hidUserName" value='<%# DataBinder.Eval(Container.DataItem, "Name")%>' runat="server">
				            </ItemTemplate>
			            </asp:DataList> 
			            <INPUT type="hidden" id="hidUserID_Name"> 				           					    
					</td>
				</tr>
				<tr>
				    <td align="center" colspan="3">
				    <div style="margin-top:30px;">
		            <INPUT id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass"> 
				    <INPUT id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
				
		            </div>
		         </td>
				</tr>
			</table>		
        </ContentTemplate>
    </asp:UpdatePanel>
       </div>
		    
            </form>
	</body>
</HTML>
