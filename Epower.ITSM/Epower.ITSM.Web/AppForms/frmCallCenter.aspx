<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCallCenter.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCallCenter" %>

<object classid="clsid:E94560D6-0ADA-4303-83C3-1FC615519DC3" name="jtdrv1" width="300"
    height="45">
</object>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>呼叫中心</title>
    
    <script language="JavaScript">    
    jtdrv1.attachEvent("OnConnect", OnConnect);
    jtdrv1.attachEvent("OnDisConnect", OnDisConnect);
    jtdrv1.attachEvent("OnRing", OnRing);
    jtdrv1.attachEvent("OnCallIn", OnCallIn);
    //////////////////////////////////////////////////
    //函数与方法
    //////////////////////////////////////////////////
    function ec_Connect() {
        jtdrv1.EC_Connect(document.getElementById('<%=txtHost.ClientID %>').value);
    }

    function ec_DisConnect() {
        jtdrv1.EC_DisConnect();
    }

    function OnConnect(sHost) {
        

    }

    function OnDisConnect(sHost) {
    }

    function OnRing(s1, s2) {       
    }

    function OnCallIn(s1, s2) {
        var blockRoom = document.getElementById('<%=hidBlockRoom.ClientID %>').value; //座室
        var appid = document.getElementById('<%=hidFlowModelId.ClientID %>').value; //应用ID
        var strExtPara = "t" + s2; //呼叫客户电话 加#是为了区分         
        if (blockRoom.indexOf(s1) > -1) {
            window.open("../Forms/form_all_flowmodelCall.aspx?AppID=" + appid + "&ep=" + strExtPara, "", "scrollbars=yes,resizable=yes,top=0,left=0,width=600,height=500");
        }
    }
   
</script>
</head>
<body>
    <input id="txtHost" type="text" value="192.168.8.97" runat="server" />
    <input id="hidBlockRoom" runat="server" type="hidden" value="-1" />
    <input id="hidFlowModelId" runat="server" type="hidden" value="" />
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>

<script language="JavaScript">
    ec_Connect();
</script>
</html>
