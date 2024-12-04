<%@ Page language="c#" Codebehind="frmLeft.aspx.cs" AutoEventWireup="True" Inherits="Epower.ITSM.Web.Config.frmLeft" %>
<HTML xmlns="http://www.w3.org/1999/xhtml">
	<HEAD runat="server">
<script type="text/javascript" src="../Js/App_Base.js"> </script>

<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
<script  type="text/javascript" src="../Js/jquery-ui-1.8.20.custom.min.js"> </script>
<style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
.STYLE8 {
	color: #336666;
	font-size: 16px;
	font-style: italic;
}
.STYLE14 {
	color: #3192BE;
	font-size: 20px;
	font-family: "ºÚÌå";
}

-->
</style>
	</HEAD>
<script language="javascript">
var selectObj;

//selectObj=;

function chang(obj,img){
	if(obj!=selectObj){
		obj.background="images/"+img;
	}
}
function myMouseDown(obj,img){
	//alert(obj.className);
	if(obj!=selectObj){
		obj.className="STYLE6";
		obj.background="images/"+img;
		selectObj.background="images/lmt-1-b.gif";
		selectObj.className="td_0";
		selectObj=obj;
	}
	
}
function chang_Class(name,my){

   var icount = <%=iCount %>;
   //debugger;
	for(i=0;i<icount;i++){
		if(i!=my){
			document.getElementById(name+i).className="STYLE2";
			document.getElementById(name+i).background="images/lanmu-closed.gif";
			$("#" + name + i).css("background-image", "url(Images/lanmu-closed.gif)");
		}
	}
	document.getElementById(name+my).className="STYLE5";
	//document.getElementById(name+my).background="images/lanmu-open.gif";
	$("#" + name + my).css("background-image", "url(images/lanmu-open.gif)");
	
}
function changImg(obj){
	if(obj.src.indexOf("images/shensuo-b.gif")>-1){
		obj.src="images/shensuo-a.gif";
		document.getElementById("left_").style.display="none";
	}else{
		obj.src="images/shensuo-b.gif";
		document.getElementById("left_").style.display="";
	}
}
function ShowMain(url)
{
    parent.frmSystemMain.location = url;
}

</script>
	<body>
	<form runat="server">
	    <table height="5px" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td height="5px" align="center"></td>
          </tr>   
        </table>
        <table width="98%" height="98%" border="0" cellpadding="0" cellspacing="0" class="bian1" align="center">
          <tr>
            <td align="center" valign="top">
                 <asp:Literal ID="LitData" runat="server"></asp:Literal>
            </td>
          </tr>
        </table>
    </form>
    </body>
    
<SCRIPT LANGUAGE="JavaScript" type="text/jscript">

	selectObj=document.getElementById("home");
	chang_Class('Td',0);
</SCRIPT>
</HTML>
