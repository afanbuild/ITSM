<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.frmPopDept" CodeBehind="frmPopDept.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../DeptControls/CtrDeptTree.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>部门</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

   		<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
		

</head>
 <script language="javascript">
 
            function CheckSelectState() {
                if (window.isSelected != undefined && window.isSelected) {
                    window.isSelected = false;
                    Click_OK();                    
                }
            }
            
            setInterval('CheckSelectState()',300);
            
			function Click_OK()
			{
			   
			   //=====zxl
		       var type  ="<%=TypeFrm %>";
		       var value=document.all.hidDeptID.value;


		         if(type=="frmeditrightbatch")
		         {    	
		                  		    
			        //window.returnValue=document.all.hidDeptID.value;    		          				
			         if (typeof (value) != "undefined" && value.length > 1)
			          {
			          
                        arr = value.split("@");
                         window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
			             window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectID").value=arr[0];
			             window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectName").value=arr[1] + "(" + arr[0] + ")";
                    }
                    else {
                       // document.all.hidObjectName.value = "";
                         window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectName").value="";
                    }
                }
                else if(type=="frmuserquery")
                {
                    if(typeof(value) !="undefined" && value.length>1)
                    {
                        arr=value.split("@");
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtDeptName").value =arr[1];
                        window.opener.document.getElementById("<%=Opener_ClientId %>hidDeptName").value=arr[1];
                        window.opener.document.getElementById("<%=Opener_ClientId %>hidQueryDeptID").value=arr[0];
                    }                    
                    
                }
                else if(type=="frmactormemberedit")
                {         
                    if (typeof (value) != "undefined" && value.length > 1)
			        {
                         arr = value.split("@");
                         window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1];
			             window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectID").value=arr[0];
                    }
                    else {
                         window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value="";
			             window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectID").value="0";
                    }
                }
               else if(type=="frmrightSeach")
               {                    
                    if(typeof(value)!="undefined" && value.length>1)
                    {                                            
                        arr=value.split("@");
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value=arr[1]+"(" +arr[0] + ")";
                    }
                    else
                    {
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value="";
                    }
			
               }
               else if(type=="frmdeptedit")
               {
                    if(value != "")
				    {
					    if(value.length>1)
					    {
						    arr=value.split("@");
						    window.opener.document.getElementById("<%=Opener_ClientId %>txtPDeptName").value =arr[1];
						    window.opener.document.getElementById("<%=Opener_ClientId %>hidPDeptID").value =arr[0];
						   // document.all.txtPDeptName.value=arr[1];
						   // document.all.hidPDeptID.value=arr[0];
					    }
				    }
               }
               else if(type=="frmactorcondedit")
               {
                    frmactorcondedit(value);
               }
               else if(type=="frmmoduserdetail")
               {
                 frmmoduserdetail(value);
               }
               else if(type=="frmeditright")
               {
                frmiditright(value)
               }
               else if(type=="frmright")
               {
                frmright(value);
               }
				
				window.close();
			}
			function Click_Cancel()
			{
				//window.returnValue="@";
				window.close();
			}
			function frmright(value)
			{
			    if(typeof(value) != "undefined" && value.length>1)
			    {			    
				    arr=value.split("@");
                    /*     
                     * Date: 2012-8-6 16:41
                     * summary: 在 frmright.aspx 页面中增加一个控件名为: txtObjectID. 用于显示友好文本.
                     * 控件 txtObjectID 隐藏, 存储具体值.
                     * modified: sunshaozong@gmail.com     
                     */        
	                window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
                    window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value=arr[0];
			    }
			    else
			    {
			        //document.all.txtObjectID.value ="";
			        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value="";
			    }
			
			}
			function frmactorcondedit(value)
			{
		            if (typeof(value)!="undefined")
					{
					    if(value.length>1)
					    {
					    
						    arr=value.split("@");
						    var objID="<%=ObjID %>";
						    var hiValue=objID.replace("cmdPop","hidValue");
						    var txtValue=objID.replace("cmdPop","txtValue");
						    window.opener.document.getElementById(hiValue).value=arr[0];
						    window.opener.document.getElementById(txtValue).value=arr[1];
					    }
					}
					else
					{
					    window.opener.document.getElementById("<%=ObjID %>.replace('cmdPop','hidValue');").value="";
					    window.opener.document.getElementById("<%=ObjID %>.replace('cmdPop','txtValue')").value="";

					}
			} 
			
			function SetSelectUrl()
			{
			   document.all.Myiframe.location = "frmSelectDept.aspx?LimitCurr=true&CurrDeptID=" + document.all.currDeptID.value;
			}
			function frmmoduserdetail(value)
			{
			    if(value != "")
				{
					if(value.length>1)
					{
						var arr=value.split("@");
						window.opener.document.getElementById("<%=Opener_ClientId %>txtDeptName").value =arr[1];
						window.opener.document.getElementById("<%=Opener_ClientId %>hidDeptID").value=arr[0];
					}
				}
			}
			function frmiditright(value)
			{
			    if(typeof(value) != "undefined" && value.length>1)
			    {
				    arr=value.split("@");
				    window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
				    window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectId").value=arr[0];

			    }
			
			}
			
			
    </script>
    
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
<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
    <form id="Form1" method="post" runat="server">
    <table>
        <tr>
            <td>
            <div style="width:240px;height:320px; overflow:scroll; border:1px solid; color:Silver;">
   <%--         <asp:TreeView ID="tvDept" runat="server" 
                            onselectednodechanged="tvDept_SelectedNodeChanged">     
                             <SelectedNodeStyle CssClass="TreeViewSelectedNode"  />                       
                            </asp:TreeView>--%>
                            
                            <uc1:CtrDeptTree ID="CtrDeptTree" runat="server" />
                            
                            
            </div>
            </td>
            <td class="style2"> &nbsp;</td>
            <td align="left" valign="top" style="width:200px;" >
             <div style="width:200px;">
                <span id="selectDeptTips"></span>
                <asp:Label ID="deptname" runat="server" Text="" Width="100%"></asp:Label>
                <asp:HiddenField ID="hidDeptID" runat="server" />
            </div>
            
            </td>
        </tr>
    </table>
    
   
    <div style="text-align: center;padding-top:30px;">
        <input id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass">
        <input id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
    </div>

    <input id="currDeptID" type="hidden" value="<%=lngCurrDeptID%>" name="currDeptID"></input>

    </form>
</body>
</html>
