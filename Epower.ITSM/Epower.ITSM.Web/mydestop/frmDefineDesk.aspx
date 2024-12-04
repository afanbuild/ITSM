<%@ Page language="c#" Inherits="Epower.ITSM.Web.mydestop.frmDefineDesk" Codebehind="frmDefineDesk.aspx.cs" AutoEventWireup="true" validateRequest="false" enableEventValidation="false"  %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>自定义桌面</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
        <script language="javascript" type="text/javascript">
        function func_insert()
        {
         var j=0;
         for (i=0; i<document.all.select2.options.length; i++)
         {
           if(document.all.select2.options(i).selected)
           {
             option_text=document.all.select2.options(i).text;
             option_value=document.all.select2.options(i).value;
             option_style_color=document.all.select2.options(i).style.color;

             var my_option = document.createElement("OPTION");
             my_option.text=option_text;
             my_option.value=option_value;
             my_option.style.color=option_style_color;

             pos=document.all.select2.options.length;
             document.all.select1.add(my_option,pos);
             document.all.select2.remove(i);
             i--;
             j--;
          }
          else
          {
            j++;
          }
          if(j==document.all.select2.options.length)
          {
             alert("调整桌面模块的项时，请选择其中一项！");
          }
         }//for
        }

        function func_delete()
        {
         var j=0;
         for (i=0;i<document.all.select1.options.length;i++)
         {
           if(document.all.select1.options(i).selected)
           {
             option_text=document.all.select1.options(i).text;
             option_value=document.all.select1.options(i).value;

             var my_option = document.createElement("OPTION");
             my_option.text=option_text;
             my_option.value=option_value;

             if(option_text.indexOf("[必选]")>0)
                return;
             pos=document.all.select2.options.length;
             document.all.select2.add(my_option,pos);
             document.all.select1.remove(i);
             i--;
             j--;
          }
          else
          {
            j++;
          }
          if(j==document.all.select1.options.length)
          {
             alert("调整桌面模块的项时，请选择其中一项！");
          }
         }//for
        }

        function func_select_all1()
        {
         for (i=document.all.select1.options.length-1; i>=0; i--)
           document.all.select1.options(i).selected=true;
        }

        function func_select_all2()
        {
         for (i=document.all.select2.options.length-1; i>=0; i--)
           document.all.select2.options(i).selected=true;
        }

        function func_up()
        {                  
          sel_count=0;
          for (i=document.all.select1.options.length-1; i>=0; i--)
          {                      
            if(document.all.select1.options(i).selected)
               sel_count++;
          }
          
          
          
          if(sel_count==0)
          {
             alert("调整桌面模块的顺序时，请选择其中一项！");
             return;
          }
          else if(sel_count>1)
          {
             alert("调整桌面模块的顺序时，只能选择其中一项！");
             return;
          }
            
          i=document.all.select1.selectedIndex;

          if(i!=0)
          {
            var my_option = document.createElement("OPTION");
            my_option.text=document.all.select1.options(i-1).text;
            my_option.value=document.all.select1.options(i-1).value;           

            document.all.select1.options(i-1).text = document.all.select1.options(i).text;
            document.all.select1.options(i-1).value = document.all.select1.options(i).value;
            document.all.select1.options(i).text = my_option.text;
            document.all.select1.options(i).value = my_option.value;
            
            
            //document.all.select1.remove(i);
            
            //document.all.select1.add(my_option,i-1);          
            
            
            //先取消原来的选择
            for (k=document.all.select1.options.length-1; k>=0; k--)
                document.all.select1.options(k).selected=false;
            
            document.all.select1.options(i-1).selected=true;
            
          }
        }

        function func_down()
        {
          sel_count=0;
          for (i=document.all.select1.options.length-1; i>=0; i--)
          {
            if(document.all.select1.options(i).selected)
               sel_count++;
          }

          if(sel_count==0)
          {
             alert("调整桌面模块的顺序时，请选择其中一项！");
             return;
          }
          else if(sel_count>1)
          {
             alert("调整桌面模块的顺序时，只能选择其中一项！");
             return;
          }

          i=document.all.select1.selectedIndex;

          if(i!=document.all.select1.options.length-1)
          {
            var my_option = document.createElement("OPTION");
            my_option.text=document.all.select1.options(i+1).text;
            my_option.value=document.all.select1.options(i+1).value;

            document.all.select1.options(i+1).text = document.all.select1.options(i).text;
            document.all.select1.options(i+1).value = document.all.select1.options(i).value;
            document.all.select1.options(i).text = my_option.text;
            document.all.select1.options(i).value = my_option.value;
            
            //document.all.select1.add(my_option,i+2);
            //document.all.select1.remove(i);
            
            //先取消原来的选择
            for (k=document.all.select1.options.length-1; k>=0; k--)
                document.all.select1.options(k).selected=false;
                
            document.all.select1.options(i+1).selected=true;
          }
        }

        function mysubmit()
        {
           fld_str="";
           for (i=0; i< document.all.select1.options.length; i++)
           {
              options_value=document.all.select1.options(i).value;
              fld_str+=options_value+",";
           }
           document.all.hidValue.value = fld_str;
       }
        
        function confirmReset()     //确定恢复吗
	    {
	        if (confirm("确认恢复默认设置吗?")) {
	            event.returnValue = true;
	        }
	        else {
	            event.returnValue = false;
	        }
	    }
        </script>
	</HEAD>
	<body>
	<form id="Form1" method="post" runat="server">
        <table width="500" align="center" class="listContent">
          <tr>
            <td align="center" nowrap class="listTitle">排序</td>
            <td align="center" valign="middle" class="listTitle"><b>显示以下桌面模块</b></td>
            <td align="center" class="listTitle">选择</td>
            <td align="center" valign="middle" class="listTitle"><b>备选桌面模块</b></td>
          </tr>
          <tr>
            <td align="center" class="listTitle">
              <input type="button" class="SmallInput" value=" ↑ " onclick="func_up();">
              <br><br>
              <input type="button" class="SmallInput" value=" ↓ " onclick="func_down();">
            </td>
            <td valign="top" align="center" class="listTitle">
            <asp:ListBox ID="select1" runat="server" style="width:200;height:280" ondblclick="func_delete();" SelectionMode="Multiple">
                <asp:ListItem Value="3">公告通知</asp:ListItem>
                <asp:ListItem Value="4">内部邮件</asp:ListItem>
            </asp:ListBox>
            <input type="button" value=" 全 选 " onclick="func_select_all1();" class="btnClass">
            </td>

            <td align="center" class="listTitle">
              <input type="button" class="SmallInput" value=" ← " onclick="func_insert();">
              <br><br>
              <input type="button" class="SmallInput" value=" → " onclick="func_delete();">
            </td>

            <td align="center" valign="top" class="listTitle">
            <asp:ListBox ID="select2" runat="server" style="width:200;height:280" ondblclick="func_insert();" SelectionMode="Multiple">
                <asp:ListItem Value="1">便签</asp:ListItem>
                <asp:ListItem Value="2">常用网址</asp:ListItem>
            </asp:ListBox>
            <input type="button" value=" 全 选 " onclick="func_select_all2();" class="btnClass">
            </td>
          </tr>

          <tr>
            <td align="center" valign="top" colspan="4" class="listTitle">
            点击条目时，可以组合CTRL或SHIFT键进行多选<br>
                <asp:Button ID="btnConfirm" runat="server" CssClass="btnClass" Text=" 保 存 " OnClientClick="mysubmit();" OnClick="btnConfirm_Click" />
                <input type="hidden" runat="server" id="hidValue" />
&nbsp;
                <asp:Button ID="btnreset" runat="server" CssClass="btnClass" Text="恢复默认"  OnClientClick="confirmReset();"
                    OnClick="btnreset_Click" />
            </td>
          </tr>
        </table>
    </form>
	</body>
</HTML>
