<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
   CodeBehind="FormRefreshPageMain.aspx.cs" Inherits="Epower.ITSM.Web.Common.FormRefreshPageMain"  Title="统一监控运维平台" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" >
    function SetUrl()
    {
          
         var fromurl = '<%=FromBackUrl%>';
         $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?formurl="+escape(fromurl) });
    }
    
</script>
    <br />
    

 <table>
 <tr>
 <td> </td>
 </tr>
 </table>
 
 <table width="100%" border="0" cellpadding="0" cellspacing="0" id="Table2" runat="server" >       
        <tr id="tr1" runat="server" style="width:100%;"  >
           <td >
             <iframe  id="iframe1"  src="Report.aspx"  width="100%" height="100%"  frameborder="0" style="border-color :#333399; border:0;" runat="server"></iframe>
           </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript" >
       
       var num = "<%=num%>";
        setInterval("timeview()", <%=lngCheckTime %>);


         function timeview() {        
      
               num++;
               if(num==3)
                 num=0;
                
              window.location = "FormRefreshPageMain.aspx?num="+num; 
               
             
        }

</script>
</asp:Content>

