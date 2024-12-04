<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectEquNature.aspx.cs" Inherits="Epower.ITSM.Web.Controls.EquNature.selectEquNature" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script type="text/javascript" defer="defer">    
    function onconfrim()
    {   
        var value="";
        var text="";     
         if(document.getElementById("checkEquNature")!=null)
         {   
               var chk=document.getElementById("checkEquNature").getElementsByTagName("input");//               
               
               for(var i=0;i<chk.length;i++)
               {
                    if(chk[i].checked==true)
                    {
                        if(value=="")
                        {
                            value = chk[i].parentNode.getAttributeNode("selectValue").value;
                        }
                        else
                        {
                            value +=","+ chk[i].parentNode.getAttributeNode("selectValue").value;
                        }
                        
                        if(text=="")
                        {
                            text = chk[i].parentNode.getAttributeNode("selectText").value;
                            //text=chk[i].parentNode.selectText;
                        }else
                        {
                            //text+=","+chk[i].parentNode.selectText;
                            text+="," + chk[i].parentNode.getAttributeNode("selectText").value;
                        }
                    }
               }             
          }
          parent.onClickValue(value,text);
   }
   
   function onClear()
   {
       parent.onClickValue("","");
   }
       
   </script>
       <style type="text/css"> 
        .WdateDiv *{font-size:9pt;white-space:nowrap;}
        .dpButton{ 
	        height:18px;
	        width:45px;
	        border:0px;
	        padding-top:2px;
	        background:url(../TimeSelect/images/btnbg.jpg);
	        color:#FFF;
	        cursor:pointer;
        }
               
    BODY {
	    margin: 0;

    }
    
    
  
</style>
    
</head>
<body>
    <form id="form1" runat="server">    
          <table class="WdateDiv" id="tableId" >          
            <tr>
                <td style="white-space:nowrap;"><asp:CheckBoxList ID="checkEquNature" runat="server"  RepeatColumns="4" ></asp:CheckBoxList></td>
            </tr>
            <tr><td align="center"><input id="btnconfrim" type="button" value="确定" onclick="onconfrim()"  class="dpButton"/>&nbsp;&nbsp;<input id="Button1" type="button" value="清空" onclick="onClear()"  class="dpButton"/></td></tr>            
          </table>   
    </form>
</body>
</html>
<script language="javascript">
if(document.getElementById("checkEquNature")!=null)
    {
         var Equchk=document.getElementById("checkEquNature").getElementsByTagName("input");//
         var EquNatureId=parent.HidEuqNature.value;
         if(EquNatureId!="")
         {
               for(var i=0;i<Equchk.length;i++)
               {
                   var sValue=EquNatureId.split(',');
                   for(var j=0;j<=sValue.length;j++)
                   {
                        if(sValue[j]==Equchk[i].parentNode.getAttributeNode("selectValue").value)
                        {
                            Equchk[i].checked=true;
                        }
                   }
               }     
           }
      }
</script>
