<%@ Page language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmpopdeptMult" Codebehind="frmpopdeptMult.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../DeptControls/CtrDeptTree.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>ѡ��ಿ��</title>
		 <base target="_self" />

		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
        <script language="javascript" type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
	</HEAD>
	<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
	<script language="javascript" type="text/jscript">
			function Click_OK()
			{
				
				var value=document.all.<%=hidDeptID.ClientID %>.value + "@" + document.all.<%=hidDeptName.ClientID %>.value;			
				window.close();
			}
			function Click_Cancel()
			{
				window.returnValue="@";
				window.close();
			}
			
			//�����ڵ��µ��ӽڵ�
		    function QueryAllChillNode(treeNode)
		    {
			    var NodeArray=treeNode.getChildren();
			    Choice_DeptMult(treeNode);//�ýڵ�û���ӽڵ�	
			    if(NodeArray.length!=0)
                {
				    for(var i=0;i<NodeArray.length;i++)
				    {
					    QueryAllChillNode(NodeArray[i]);//�ݹ��ӽڵ�
					}
			    }
		    }
		    
		    
		    
		</script>
		<form id="Form1" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
			
			  </ContentTemplate>
        </asp:UpdatePanel>
        
        <table width="100%" height="100%"  class="listContent">
				<tr height="100%">
					<td class="list" width="40%" vAlign="top">
					

<%--	                <asp:TreeView ID="tvDept"  runat="server" 
                            onselectednodechanged="tvDept_SelectedNodeChanged"   > 
	                <SelectedNodeStyle CssClass="TreeViewSelectedNode"  /> 
                    </asp:TreeView>--%>
					<uc1:CtrDeptTree ID="CtrDeptTree" runat="server" />
					
					</td>
					
					<td vAlign="middle" align="center" width="5%" class="listTitle">
					    <asp:Button ID="btnAdd" style="WIDTH: 35px; HEIGHT: 24px" Text="ѡ��"  
                            runat="server" onclick="btnAdd_Click" />
			            <br />
						<br />
						    <asp:Button runat="server" ID="btnRemove" style="WIDTH: 35px;" Text="�Ƴ�" 
                            onclick="btnRemove_Click" />						
					            
					      <br />
					    <br />

				            <asp:Button class="FLOWBUTTON" id="btnClear" style="WIDTH: 35px" size="30" 
                            Text="���" runat="server" onclick="btnClear_Click" />
		            </td>
		            <td width="40%" vAlign="top" class="list">
		                <asp:listbox id="lsbDeptTo" Rows="6" Width="100%" runat="server" Height="100%" ></asp:listbox>
		            </td> <%--ondblclick="Removeone();"--%>
				</tr>
				<tr>
					<td class="list" align="center" colspan="3">
					        <asp:Button ID="cmdOK" Text="ȷ��" runat="server" class="btnClass" 
                                onclick="cmdOK_Click"/>
						<%--<INPUT id="cmdOK" type="button" value="ȷ��" onclick="Click_OK()" class="btnClass">--%> 
						<INPUT id="cmdCancel" type="button" value="ȡ��" onclick="Click_Cancel()" class="btnClass">
					</td>
				</tr>
			</table>
        
			<INPUT type="hidden" id="hidDeptID" runat="server">
			<INPUT type="hidden" id="hidDeptName" runat="server">
		</form>
		<script language="javascript" type="text/jscript">
		    function Choice_Dept()
		    {	
		            //=========zxl
		            var selectedNodeID = Form1.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
		            if(selectedNodeID=="")
		            {
		                alert("��ѡ����Ҫ�Ĳ��ţ�");
		                return;
		            }
		              
			    if (selectedNodeID != "") {
			        var selectedNode = document.getElementById(selectedNodeID);
			        var sValue = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
			        sValue = sValue.substring(sValue.lastIndexOf('\\') + 1);
			        var sText = selectedNode.innerHTML;
			        var lngID=sValue;
			        }
			        
			    var oOption = window.document.createElement("OPTION");
			    oOption.text= sText;
			    oOption.value= lngID;
				var j;
				var iID = "<%=lsbDeptTo.ClientID %>";	
				var objDeptTo = document.getElementById(iID);
			    for (j=0; j<objDeptTo.options.length; j++) 
			    {
				    if (objDeptTo.options(j).value==lngID)
				    {
					    alert("���棬ѡ�����ظ���");
					    return;
				    }			        
			    }
			    objDeptTo.add(oOption);
			    Get_DeptIDs(objDeptTo);			
		    }
		    
		    function Choice_DeptMult(treeNode)
		    {	
		        var lngID =treeNode.getAttribute("ID");
			    var sText =treeNode.getAttribute("Text");
			    var oOption = window.document.createElement("OPTION");
			    oOption.text= sText;
			    oOption.value= lngID;
				var j;
				var iID = "<%=lsbDeptTo.ClientID %>";	
				var objDeptTo = document.getElementById(iID);
			    for (j=0; j<objDeptTo.options.length; j++) 
			    {
				    if (objDeptTo.options(j).value==lngID)
				    {
					    return;
				    }			        
			    }
			    objDeptTo.add(oOption);
			    Get_DeptIDs(objDeptTo);			
		    }
		    
		    //ɾ���Ѿ�ѡ��
		    function Removeone()
		    {
		        var i;
		        var j;
		        var k = 0;
		        var iID = "<%=lsbDeptTo.ClientID %>";	
				var objDeptTo = document.getElementById(iID);
				i=objDeptTo.selectedIndex;
				if(i==-1)
				{
				    alert("��ѡ����Ҫɾ���Ĳ��ţ�");
				}
				else
				{
				    oOption=objDeptTo.options(i);
				    objDeptTo.remove(i);
				    Get_DeptIDs(objDeptTo);
				}
		    }
		    
            function Removeall()
            {
            
                if(confirm("�Ƿ�Ҫ���������"))
                {
               
                   var lsbDeptTosize= $('#<%=lsbDeptTo.ClientID %> option').length;
                   var iID="<%=lsbDeptTo.ClientID %>";
                   var objDeptTo=document.getElementById("<%=lsbDeptTo.ClientID %>");
                   //alert(objDeptTo);
                 
                   if(lsbDeptTosize>0)
                   {
                   
                         for(var i=0;i<lsbDeptTosize;i++)
                         {
                            objDeptTo.options.remove(i);   
                         }
                        
                   }
                   else
                   {
                    alert("û��Ҫ�����ѡ��");
                   }
                    
                }
            }
		    //��ȡ��ز���ID��
		    function Get_DeptIDs(obj)
		    {
		    
			    var i;
			    var DeptIDs="";
			    var DeptNames="";			
			    
			    for (i=0; i<obj.options.length; i++) 
			    {
				    DeptIDs=DeptIDs + obj.options(i).value + ",";
				    DeptNames=DeptNames +obj.options(i).text + "," ;
    				
			    }
    			
			    if(DeptIDs.length==1)	DeptIDs="";
    						
			    document.all.<%=hidDeptID.ClientID %>.value=DeptIDs;
			    document.all.<%=hidDeptName.ClientID %>.value=DeptNames;
		    }
		</script>
	</body>
</HTML>
