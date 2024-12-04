<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frm_BR_MEETINGSCHEDULED_List.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_BR_MEETINGSCHEDULED_List" Title="无标题页" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %> 


<%@ Register src="../Controls/ctrDateSelectTime.ascx" tagname="ctrDateSelectTime" tagprefix="uc1" %>
<%@ Register src="../Controls/controlpage.ascx" tagname="controlpage" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
            body:nth-of-type(1) .other_fix_width_for_chrome
            {
            	width:100px!important;
            }                        
            
.fixed-grid-border2 
{
	border-right: 1px #A3C9E1 solid;
}

.fixed-grid-border2 td {
	border-left: solid 1px #CEE3F2;
	border-right: 0px;
}

.fixed-grid-border2 tr {
	border-bottom: solid 1px #CEE3F2;
	border-top: solid 1px #CEE3F2;
}            
    </style>
    

<script language="javascript" type="text/javascript">
function OpenDeleteFlow(obj)  //删除流程
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete","hidDelete")).value;
        var	value=window.showModalDialog("../Common/frmFlowDelete.aspx?FlowID=" + FlowID,window,"dialogHeight:230px;dialogWidth:320px");
        if(value!=null)
        {
            if(value[0]=="0") //成功
                event.returnValue = true;
            else
                event.returnValue = false;
        }
        else
        {
            event.returnValue = false;
        }
    }
    
    function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","tr");
              
              var className;
              var objectFullName;
              var tableCtrl;
              objectFullName = <%=tr1.ClientID%>.id;
              className = objectFullName.substring(0,objectFullName.indexOf("tr1")-1);
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
</script>	

<table cellpadding='1' cellspacing='2' width='98%' border='0' class='listContent' visible="false">
<tr>
    
    <TD align="left"  visible="false" style="border-color:White">    
        <uc1:ctrDateSelectTime ID="ctrmeetingseletime" runat="server" Visible="false" />
    </TD>
   
   
    <td align="right" class="list" style="border-color:White">
    <asp:Label id="labtitle" runat="server" Font-Bold="True" Font-Size="Small" ></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

    <asp:Button ID="btnupofday" runat="server" Text="上周会议" onclick="btnupofday_Click" />&nbsp;&nbsp;&nbsp;<asp:Button 
            ID="btnnexofday" runat="server" Text="下周会议" onclick="btnnexofday_Click" />
    </td>
</tr>
</table>

<br />

<table width="98%" border="0">
    <tr>
         <td valign="top" align="left" class="listTitleNew">
              <table width="150" border="0" cellspacing="0" cellpadding="0">
                 <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_expandall.gif"
                                width="16" align="absbottom" />
                            详细列表
                        </td>
                    </tr>
                </table>
         </td>
     </tr>
</table>

<table cellpadding="0"  cellspacing="0" width="98%" align="center" border="0"  >
	<tr id="tr1" runat="server" style="display: none;">
		<td align="center"  class="listContent">
			<asp:datagrid id="datagrid_Meeting" runat="server" Width="100%" 
                AutoGenerateColumns="False" cellpadding="1" cellspacing="2" BorderWidth="0px" 
                ondeletecommand="datagrid_Meeting_DeleteCommand"  CssClass="fixed-grid-border2"
                onitemdatabound="datagrid_Meeting_ItemDataBound" EnableTheming="True"
			    >
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle" ForeColor="Black"></HeaderStyle>
				<Columns>
				    <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
	                <asp:BoundColumn Visible="False" DataField="flowmodelid"></asp:BoundColumn>
				    <asp:BoundColumn DataField='MeetingName' HeaderText='会议名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='Title' HeaderText='议题'></asp:BoundColumn>
					<asp:BoundColumn DataField='MeetingRoom' HeaderText='会议室'></asp:BoundColumn>
					<asp:BoundColumn DataField='datetime2' HeaderText='预定日期'></asp:BoundColumn>
					
					<asp:TemplateColumn HeaderText="处理" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
		            <HeaderStyle Width="5%"></HeaderStyle>
		            <ItemTemplate>
			            <INPUT id=CmdDeal class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "flowid"))%>' type=button value='详情' name=CmdDeal runat="server">
		            </ItemTemplate>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
	                </asp:TemplateColumn>
	                <asp:TemplateColumn HeaderText="删除" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
		                <HeaderStyle Width="5%"></HeaderStyle>
		                <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" SkinID="btnClass1" OnClientClick="OpenDeleteFlow(this);" />
                            <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
		                </ItemTemplate>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
	                </asp:TemplateColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
</table>


<table width="98%">
        <tr>
            <td valign="top" align="left" width="100%" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            每周会议安排
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
</table>
    
<table cellpadding="0"  cellspacing="0" width="100%" align="center" border="0">
    <tr id="tr2" runat="server">
        <td align="center">
        
          
            <table cellpadding="0"  cellspacing="0" width="98%" align="center" border="0">
            <tr >
		        <td align="center"  class="listContent">
			        <asp:datagrid id="datagrid_show" runat="server" Width="100%" 
                        AutoGenerateColumns="False" cellpadding="1" cellspacing="2" BorderWidth="0px" 
                         onitemdatabound="datagrid_show_ItemDataBound"  CssClass="fixed-grid-border2"
                         EnableTheming="True"
			            >
				        <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                        <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			            <HeaderStyle CssClass="listTitle" ForeColor="Black" BorderColor="White" 
                            BorderStyle="None" Height="0px"></HeaderStyle>
				        <Columns>
				            <asp:BoundColumn DataField='MeetingRoom' HeaderText='' ItemStyle-Width="10%"></asp:BoundColumn>
					        <asp:BoundColumn DataField='Monday' HeaderText='' ItemStyle-Width="10%"></asp:BoundColumn>
				            <asp:BoundColumn DataField='Tuesday' HeaderText='' ItemStyle-Width="10%"></asp:BoundColumn>
					        <asp:BoundColumn DataField='Wednesday' HeaderText='' ItemStyle-Width="10%"></asp:BoundColumn>
					        <asp:BoundColumn DataField='Thursday' HeaderText='' ItemStyle-Width="10%"></asp:BoundColumn>
					        <asp:BoundColumn DataField='Friday' HeaderText='' ItemStyle-Width="10%"></asp:BoundColumn>
					        <asp:BoundColumn DataField='Saturday' HeaderText='' ItemStyle-Width="10%"></asp:BoundColumn>
					        <asp:BoundColumn DataField='Sunday' HeaderText='' ItemStyle-Width="10%"></asp:BoundColumn>
				        </Columns>
				        </asp:datagrid>
			     </td>
	        </tr>
            </table>
        </td>
    </tr>
	
</table>
</asp:Content>
