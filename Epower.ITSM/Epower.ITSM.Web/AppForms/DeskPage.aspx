<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="DeskPage.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.DeskPage" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        ul
        {
            text-align: left;
            padding: 5px;
            letter-spacing: 8px;
            margin-left: 10px;
            letter-spacing: normal;
        }
    </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>

    <script>

    </script>

    <script type="text/javascript" language="javascript">

    
    
	function E8MenuConfigGoToUrl(url)
	{
		//E8MenuConfighideAllWindows(0);
        if(url != "")
        {
           window.top.MainFrame.location=url;
        }

    }
    
    var __pageName;
    var __pageUrl;
    var __strVal;
    var __resourceKey;
    
    function onmuseover(obj, PageName, pageUrl, strVlues,resourceKey) {
    
        __pageName = PageName;
        __pageUrl = pageUrl;
        __strVal = strVlues;
        __resourceKey = resourceKey;   
        
        //obj.style.backgroundColor = "green";        
        document.getElementById("<%=PageName.ClientID%>").value = PageName;
        document.getElementById("<%=pageUrl.ClientID%>").value = pageUrl;
        document.getElementById("<%=power.ClientID%>").value=strVlues;
        document.getElementById("<%=resourceKey.ClientID%>").value=resourceKey;
        var strManager = document.getElementById("<%=Manager.ClientID%>").value;
        if(pageUrl!="")
        {
            obj.style.color = "red";
        }

	 }
	 function onmuseoverOut(obj) {
	     //obj.style.backgroundColor = "";
	     obj.style.color = "";
	     document.getElementById("<%=PageName.ClientID%>").value = "";
         document.getElementById("<%=pageUrl.ClientID%>").value = "";
         document.getElementById("<%=power.ClientID%>").value="";
         document.getElementById("<%=resourceKey.ClientID%>").value="";
	 }
	 
	 
window.onload = function(){

document.getElementById("tdItem").oncontextmenu=ContextMenu;  

}
 
 
function ContextMenu()
{  

    var strPageName=document.getElementById("<%=PageName.ClientID%>").value;
    var strpageUrl=document.getElementById("<%=pageUrl.ClientID%>").value;
    var strManager = document.getElementById("<%=Manager.ClientID%>").value;
    var powerValue=document.getElementById("<%=power.ClientID%>").value;    
    var resourceKey=document.getElementById("<%=resourceKey.ClientID%>").value;    
    if(strPageName!=""&&strpageUrl!="")    
    {              
         var url="DeskPageSet.aspx?Randomid="+ GetRandom()+"&PageName="+escape(strPageName)+"&PageUrl="+escape(strpageUrl)
         +"&Manager="+escape(strManager)+"&Power="+escape(powerValue)+"&ResourceKey="+escape(resourceKey);         

         open(url,"","scrollbars=no,status=yes ,resizable=yes,width=400,height=200");                       
    }    
   return false;
}


function ContextMenu22()
{  

    __pageName;
    __pageUrl;
    __strVal;
    __resourceKey;

    var strPageName=__pageName;
    var strpageUrl=__pageUrl;
    var strManager = document.getElementById("<%=Manager.ClientID%>").value;
    var powerValue=__strVal;    
    var resourceKey=__resourceKey;   
    
    if(strPageName!=""&&strpageUrl!="")    
    {    

       
         var url="DeskPageSet.aspx?Randomid="+ GetRandom()+"&PageName="+escape(strPageName)+"&PageUrl="+escape(strpageUrl)
         +"&Manager="+escape(strManager)+"&Power="+escape(powerValue)+"&ResourceKey="+escape(resourceKey);         
//         Window.open ('http://www.baidu.com');
         
//         open("http://www.baidu.com");
         open(url,"","scrollbars=no,status=yes ,resizable=yes,width=400,height=200");         

    }    
}
    </script>

    <input type="hidden" id="power" runat="server" />
    <input type="hidden" id="Manager" runat="server" />
    <input type="hidden" id="PageName" runat="server" />
    <input type="hidden" id="pageType" runat="server" />
    <input type="hidden" id="pageUrl" runat="server" />
    <input type="hidden" id="UserId" runat="server" />
    <input type="hidden" id="resourceKey" runat="server" />
    <table style="width: 100%; vertical-align: top" id="tdItem">
        <tr>
            <td align="left">
                <font color='green' size='2' face='宋体'>请对列表中想设置成为常用功能的项目点击右键，在弹出的对话框中点击【设置为个人】按钮。如此，该功能就会出现在主页的常用功能栏目中。相应的，如果要取消，就在列表中右击设置好的项目，在弹出的对话框中点击【取消设置】按钮。</font><br />
                <hr />
                <div id="DeskPageId" runat="server">
                </div>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
$(document).ready(function(event){    
    $('#<%=DeskPageId.ClientID %> ul li').mouseover(function(){
        var len = $(this).find('a').size();        
        if (len >= 2) {
            len = $(this).find('li').size();
            if (len <= 0){
                $(this).find('a:last').show();
            } else {
                //$(this).find('a:eq(1)').show();
            }             
        }else {
            len = $(this).find('li').size();
            if (len <= 0){
                $(this).append('&nbsp;&nbsp;&nbsp;&nbsp;<a href="###" onclick="ContextMenu22()">打开</a>');
            } else {
                //$(this).prependTo('&nbsp;&nbsp;&nbsp;&nbsp;<a href="###" onclick="ContextMenu22()">打开</a>');
            }
        }        
          
    }).mouseout(function(event){        
        var len = $(this).find('a').size();        
        if (len >= 2){          
            len = $(this).find('li').size();            
            if (len <= 0){
                $(this).find('a:last').hide();
            } else {
                //$(this).find('a:eq(1)').hide();
            }                           
        }
    });
});

    </script>

</asp:Content>
