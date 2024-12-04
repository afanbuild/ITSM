<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDefineDeskMain.aspx.cs" Inherits="Epower.ITSM.Web.mydestop.frmDefineDeskMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>自定义桌面</title>
    <script language="javascript" type="text/javascript">
        var menu_id=0;

        function setPointer(theRow, thePointerColor,menu_id_over)
        {
          if(menu_id!=menu_id_over)
             theRow.bgColor = thePointerColor;
        }

        function view_menu1()
        {
          if(menu_id!=0)
           menu_main.location="frmDefineDesk.aspx?POS=MYTABLE_LEFT";
           menu_id=1;
           menu_1.bgColor='#E7F0F9';
           menu_2.bgColor='#ffffff';
        }

        function view_menu2()
        {
           menu_main.location="frmDefineDesk.aspx?POS=MYTABLE_RIGHT";
           menu_id=2;
           menu_1.bgColor='#ffffff';
           menu_2.bgColor='#E7F0F9';
        }
        </script>
</head>
<body onload="view_menu1()" >
    <form id="form1" runat="server">
    <TABLE class="listContent" height="100%" width="100%">
         <TR>
           <TD width="150" title="桌面左侧" id="menu_1" onclick="view_menu1()" onmouseover="setPointer(this, '#B3D1FF',1)" onmouseout="setPointer(this, '#ffffff',1)" style="cursor:hand">
               <img src="../Images/mytable.gif" WIDTH="18" HEIGHT="18" align="absmiddle"><b><font color="#000000"> 桌面左侧</font></b>
           </TD>

           <TD width="150" title="桌面右侧" id="menu_2" onclick="view_menu2()" onmouseover="setPointer(this, '#B3D1FF',2)" onmouseout="setPointer(this, '#ffffff',2)" style="cursor:hand">
               <img src="../Images/mytable.gif" WIDTH="18" HEIGHT="18" align="absmiddle"><b><font color="#000000"> 桌面右侧</font></b>
           </TD>
           <td  class="list"></td>
           </TR>
           <tr>
                <td colspan="3" class="list">
                    <iframe id='menu_main' src="frmDefineDesk.aspx?POS=MYTABLE_LEFT" width='100%' height='450' scrolling='no'  frameborder='no'></iframe>
                </td>
           </tr>
        </TABLE>
    </form>
</body>
</html>
