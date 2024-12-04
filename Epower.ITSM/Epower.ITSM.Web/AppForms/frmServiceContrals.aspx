<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmServiceContrals.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmServiceContrals" Title="服务监管" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
function ShowTable(imgCtrl)
{
      var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
      var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
      var TableID = imgCtrl.id.replace("Img","Table");
      var className;
      var objectFullName;
      var tableCtrl;
      objectFullName = <%=tableserver.ClientID%>.id;
      className = objectFullName.substring(0,objectFullName.indexOf("tableserver")-1);
      tableCtrl = document.all.item(className.substr(0,className.length)+"_"+TableID);
      if(imgCtrl.src.indexOf("icon_expandall") != -1)
      {
        tableCtrl.style.display ="";
        imgCtrl.src = ImgMinusScr ;
      }
      else
      {
        tableCtrl.style.display ="none";
        imgCtrl.src = ImgPlusScr ;		 
      }
} 
function ChangeDate()
{
   document.all.<%=btnOk.ClientID%>.click();
}
</script>
<table class="listContent" width="100%" align="center"  runat="server" id="tableserver">
<tr>
    <td style="width: 84px" class="listTitle">
        统计范围：</td>
    <td class="list">
        <uc1:ctrdateandtime ID="CtrdBegin" runat="server" ShowTime="false" />
        ~<uc1:ctrdateandtime ID="CtrdEnd" runat="server" ShowTime="false"/>
    </td>
</tr>
</table>
<table class="listContent" width="100%" align="center">
<tr>
    <td align="left" class="listTitle" style="width: 106px">
        <asp:label id="Label2" runat="server" Text="累计事件单数：" Width="113px"></asp:label>
        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:label id="lblServic" runat="server" Text="0" ForeColor="red" Font-Bold="true" Font-Size="Large"></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 106px">
        <asp:label id="Label1" runat="server" Text="累计投诉单数：" Width="113px"></asp:label>
        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:label id="lblByts" runat="server" Text="0" ForeColor="red" Font-Bold="true" Font-Size="Large"></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 106px">
        <asp:label id="Label4" runat="server" Text="累计问题单数：" Width="113px"></asp:label>
        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:label id="lblProblem" runat="server" Text="0" ForeColor="red" Font-Bold="true" Font-Size="Large"></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 106px">
        <asp:label id="Label6" runat="server" Text="累计变更单数：" Width="113px"></asp:label>
        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:label id="lblChange" runat="server" Text="0" ForeColor="red" Font-Bold="true" Font-Size="Large"></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 106px">
        <asp:label id="Label8" runat="server" Text="累计巡检单数：" Width="113px"></asp:label>
        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:label id="lblPatrol" runat="server" Text="0" ForeColor="red" Font-Bold="true" Font-Size="Large"></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 106px">
        <asp:label id="Label10" runat="server" Text="累计知识单数：" Width="113px"></asp:label>
        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:label id="lblInfor" runat="server" Text="0" ForeColor="red" Font-Bold="true" Font-Size="Large"></asp:label>
    </td>
</tr>
</table>
<table id="Table11" width="100%" align="center" runat="server"  >
<tr>
    <td vAlign="top" align="left" class="listTitleNew">
          <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>事件统计</td>
</tr>
</table>
<table width="100%" align="center" runat="server" id="Table1" class="listContent">
<tr>
    <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label12" runat="server" Text="已完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblServiceFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
     <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label14" runat="server" Text="未完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblServiceUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label16" runat="server" Text="超时完成："></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblServiceOverFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
<tr>
<td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label18" runat="server" Text="超时未完成：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblServiceOverUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label20" runat="server" Text="已回访："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblServicFeedBack" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
     <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label26" runat="server" Text="满意度：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list"">
        <asp:label id="lblServicePlease" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
<tr>
    <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label28" runat="server" Text="超时事件率：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list" colspan="5">
        <asp:label id="lblServiceOver" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
</table>
<br />
<table id="Table12" width="100%" align="center" runat="server" >
<tr>
    <td vAlign="top" align="left" class="listTitleNew">
          <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>投诉统计</td>
</tr>
</table>
<table width="100%" align="center" runat="server" id="Table2" class="listContent">
<tr>
    <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label30" runat="server" Text="已完成："></asp:label>
    </td>
    <td align="left" class="list"  width="20%">
        <asp:label id="lblBytsFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
     <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label32" runat="server" Text="未完成："></asp:label>
    </td>
    <td align="left" class="list"  width="20%">
        <asp:label id="lblBytsUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label34" runat="server" Text="超时完成："></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblBytsOverFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
<tr>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label36" runat="server" Text="超时未完成：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblBytsOverUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label40" runat="server" Text="已回访："></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblBytsFeedBack" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
     <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label46" runat="server" Text="满意度：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblBytsPlease" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
<tr>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label38" runat="server" Text="超时事件率：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list" colspan="5">
        <asp:label id="lblBytsOver" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
</table>
<br />
<table id="Table13"  width="100%" align="center" runat="server" >
<tr>
    <td vAlign="top" align="left" class="listTitleNew">
          <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>问题统计</td>
</tr>
</table>
<table  width="100%" align="center" runat="server" id="Table3" class="listContent">
<tr>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label48" runat="server" Text="已完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblProblemFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
     <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label50" runat="server" Text="未完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblProblemUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label52" runat="server" Text="超时完成："></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblProblemOverFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
<tr>
 <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label54" runat="server" Text="超时未完成：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblProblemOverUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label56" runat="server" Text="超时事件率：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list" colspan="3">
        <asp:label id="lblProblemOver" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
</table>
<br />
<table id="Table14" width="100%" align="center" runat="server"  >
<tr>
    <td vAlign="top" align="left" class="listTitleNew">
          <img class="icon" id="Img4" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>变更统计</td>
</tr>
</table>
<table  width="100%" align="center" runat="server" id="Table4" class="listContent">
<tr>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label58" runat="server" Text="已完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblChangeFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
     <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label60" runat="server" Text="未完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblChangeUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label62" runat="server" Text="超时完成："></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblChangeOverFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
<tr>
<td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label64" runat="server" Text="超时未完成：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblChangeOverUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label66" runat="server" Text="超时事件率：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list" colspan="3">
        <asp:label id="lblChangeOver" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
</table>
<br />
<table id="Table15"  width="100%" align="center" runat="server"  >
<tr>
    <td vAlign="top" align="left" class="listTitleNew">
          <img class="icon" id="Img5" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>巡检统计</td>
</tr>
</table>
<table width="100%" align="center" runat="server" id="Table5" class="listContent">
<tr>
    <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label68" runat="server" Text="已完成："></asp:label>
    </td>
    <td align="left" class="list"  width="20%">
        <asp:label id="lblPatrolFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
     <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label70" runat="server" Text="未完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblPatrolUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px;">
        <asp:label id="Label72" runat="server" Text="超时完成："></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblPatrolOverFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
<tr>
<td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label74" runat="server" Text="超时未完成：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblPatrolOverUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label76" runat="server" Text="超时事件率：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list" colspan="3">
        <asp:label id="lblPatrolOver" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
</tr>
</table>
<br />
<table id="Table16"  width="100%" align="center" runat="server"  >
<tr>
    <td vAlign="top" align="left" class="listTitleNew">
          <img class="icon" id="Img6" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>知识统计</td>
</tr>
</table>
<table width="100%" align="center" runat="server" id="Table6" class="listContent">
<tr>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label78" runat="server" Text="已完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblInforFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
     <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label80" runat="server" Text="未完成："></asp:label>
    </td>
    <td align="left" class="list" width="20%">
        <asp:label id="lblInforUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label82" runat="server" Text="超时完成："></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblInforOverFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true"></asp:label>
    </td>
</tr>
<tr>
<td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label84" runat="server" Text="超时未完成：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list">
        <asp:label id="lblInforOverUnFinish" runat="server" Text="0" ForeColor="red" Font-Bold="true" ></asp:label>
    </td>
    <td align="left" class="listTitle" style="width: 100px">
        <asp:label id="Label86" runat="server" Text="超时事件率：" Width="100px"></asp:label>
    </td>
    <td align="left" class="list" colspan="3">
        <asp:label id="lblInforOver" runat="server" Text="0" ForeColor="red" Font-Bold="true"></asp:label>
    </td>
</tr>
</table>
<asp:Button ID="btnOk" runat="server" Text="dfd" Width="0" Height="0" OnClick="btnOk_Click" />
</asp:Content>
