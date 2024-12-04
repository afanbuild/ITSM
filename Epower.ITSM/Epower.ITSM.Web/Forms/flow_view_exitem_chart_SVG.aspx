<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="flow_view_exitem_chart_SVG.aspx.cs"
    Inherits="Epower.ITSM.Web.Forms.flow_view_exitem_chart_SVG" %>

<!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">
<html>
<head id="Head1" runat="server">
    <title>流程视图</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <!--<script language="javascript" src="../Js/NoRigthMouse.js"></script>  -->

    <script type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>

</head>
<body>
    <form id="Form1" method="post" runat="server">
    <font face="宋体"></font>
    </form>
</body>
</html>

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
        epower.tools.open(url,null,{width:600, height:300, left:pos.x, top:pos.y});
    }
			
</script>

