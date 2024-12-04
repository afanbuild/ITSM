<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.flow_view_exitem_chart" Codebehind="flow_view_exitem_chart.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head runat=server>
		<title>流程视图</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!--<script language="javascript" src="../Js/NoRigthMouse.js"></script>  -->
		
		<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
		
	</head>
	<body>	
		<form method="post" runat="server">
			<FONT face="宋体"></FONT>
		</form>
	</body>
</HTML>

<script type="text/javascript" src="../js/epower.base.js"></script>

<script type="text/javascript">
			
    var ignore_node = [1,4];

    function open_view_displayway(nodemodelid) {
                       
        if ($.inArray(nodemodelid, ignore_node) >= 0 ) {
            return;
        }

        var appid = $('#hidAppID').val();			    
        var flowmodelid = $('#hidFlowModelID').val();
        
        var url = '../EquipmentManager/frm_ExtensionDisplayWayEdit.aspx?appid='+ appid +'&flowmodelid='+ flowmodelid +'&nodemodelid=' + nodemodelid;
        
        var pos = epower.tools.computeXY('center', window, 600,300);
        //epower.tools.open(url,null,{width:200, height:100, left:pos.x, top:pos.y});
        window.open(url, "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top="+ pos.y +",left="+ pos.x +",width=600,height=300");
    }
			
</script>