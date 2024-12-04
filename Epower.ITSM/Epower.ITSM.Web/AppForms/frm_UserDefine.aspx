<%@ Page Language="C#" MasterPageFile="~/FlowFormsUserDefine.Master" AutoEventWireup="true" CodeBehind="frm_UserDefine.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_UserDefine" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript">
<!--
            function TransferValue()
            {
                if (typeof(parent.header.flowInfo.Subject)!="undefined" )
                { 
	                parent.header.flowInfo.Subject.value = document.all.<%=hidUser.ClientID%>.value + "申请单";
	            }
            }

            function DoUserValidate(lngActionID,strActionName)   //提交时执行
	        {
	            window.ifflow.topic.document.all.btnsubmit.click();  // 提交表单前提交自定义表单 
	            
	            TransferValue();
			    return true;
		    }
		    function DoUserSaveValidate()   //暂存时执行
		    {
		        //alert(window.ifflow.topic.document.all.btnsubmit.id);
		        window.ifflow.topic.document.all.btnsubmit.click();  // 提交表单前提交自定义表单 
		        return true;
		    }
		    function DoMasterPublicDeleteValidate()   //删除时执行
		    {
		        window.ifflow.topic.document.all.btndelete.click();  // 删除前删除自定义表单数据
		        return true;
		    }
			
			function test()
			{
			    //var id =document.all.iftest.document.forms[0].getElementById("Button3").id;
			    //var id =window.iftest.topic.form1.document.getElementById("button3").id;
			    window.ifflow.topic.document.all.btnsubmit.click();
			    //var id = window.iftest.topic.document.all.button3.id;
			    //alert(id);
			    event.returnValue = false;
			}
  //-->
</script>
<input type="hidden" id="hidUser" runat="server" />
<iframe name="ifflow" id="ifflow" src="" width="100%" frameBorder=0 scrolling="no"></iframe>

<script language="javascript" type="text/javascript">
    var surl = "../ebsys/fceform/common/djframe.htm?djsn=" + '<%=sDjsn%>' + "&djtype=" + '<%=sDjlx%>' + "&id=" + '<%=sID%>' + "&control=" + '<%=sInfControl%>';
  ifflow.location=surl;
  
  document.all.ifflow.style.height= '<%=sDjHeight%>';
</script>
</asp:Content>
