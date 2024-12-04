<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Epower.ITSM.Web.Forms.OA_MessageQuery" Title="流程跟踪" Codebehind="OA_MessageQuery.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrMessageQuery" Src="../Controls/CtrMessageQuery.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/ControlPageFoot.ascx" tagname="ControlPageFoot" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript">	
        var blnHasShow;
		var xmlhttp = null;
        function CreateXmlHttpObject()
        {
			try  
			{  
				xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");  
			}  
			catch(e)  
			{  
				try  
				{  
					xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");  
				}  
				catch(e2){}  
			}
			return xmlhttp;
        }
        
         function absoluteLocation(element, offset) 
        { var c = 0; while (element) {  c += element[offset];  element = element.offsetParent; } return c; 
        } 
        
        function GetFlowShotInfo(obj,flowid,appid)
        {
            
             if(blnHasShow == true)
              {
                  return;
              }
      
            blnHasShow = true;
         //异步获取
            if(xmlhttp == null)
                 xmlhttp = CreateXmlHttpObject();   
                
            if(xmlhttp != null)
            {
                try
                {	
                
			        xmlhttp.open("GET", "../Common/frmGetFlowShot.aspx?flowid=" + flowid + "&appid=" + appid, true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
			        xmlhttp.onreadystatechange = function() 
										        { 
    										        
											        if ( xmlhttp.readyState==4 ) 
											        {
             
                                                         var sXml = xmlhttp.responseText;
                                                         var object = document.getElementById("divShowMessageDetail");
                                                         if(object != null)
                                                         {
                                                              object.style.display = "";
                                                         }
                                                         object.innerHTML = sXml;
                                            								       
                                            			//alert(object.offsetHeight);				       
                                                        object.style.left = absoluteLocation(obj, 'offsetLeft') +  obj.offsetWidth / 2 + "px";  
                                                        if(absoluteLocation(obj, 'offsetTop') - object.offsetHeight < 20)
                                                        {
                                                            object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight / 2 + "px"; 
                                                        }
                                                        else
                                                        {
                                                            object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight / 2 - object.offsetHeight + "px";
                                                        }
                                                     }
                                                     
                                                }   
                      xmlhttp.send(null);   
                 }
                 catch(e3)
                 {
                 }
             }    
              
        }
        
        function hideMe(id,status)
        {
            
            var object = document.getElementById(id);
            
            if(object != null)
            {
                object.style.display = status;
                
                if(status == "none")
                  blnHasShow = false;
            }
            //alert(object.style.display);
        }

      
		
//-->
		</script>
		
<table width="80%" align="center" class="listContent">
	<TR>
		<TD class="listTitle" align="center">
			<uc1:CtrTitle id="CtrTitle1" runat="server"></uc1:CtrTitle>
		</TD>
	</TR>
	<TR>
		<TD align="center" class="list">
			<asp:LinkButton id="lkbOverTime" runat="server" ForeColor="Blue" onclick="lkbOverTime_Click">月内超时完成</asp:LinkButton><FONT face="宋体">&nbsp;&nbsp;&nbsp;
				<asp:LinkButton id="lkbInTime" runat="server" ForeColor="Blue" onclick="lkbInTime_Click">月内按时完成</asp:LinkButton>&nbsp;&nbsp;
			</FONT>
			<asp:LinkButton id="lkbMaster" runat="server" ForeColor="Blue" onclick="lkbMaster_Click">月内主办</asp:LinkButton><FONT face="宋体">&nbsp;&nbsp;
			</FONT>
			<asp:LinkButton id="lkbAssist" runat="server" ForeColor="Blue" onclick="lkbAssist_Click">月内协办</asp:LinkButton><FONT face="宋体">&nbsp;&nbsp;
			</FONT>
			<asp:LinkButton id="lkbReader" runat="server" ForeColor="Blue" onclick="lkbReader_Click">月内阅知</asp:LinkButton><FONT face="宋体">&nbsp;&nbsp;
				<asp:LinkButton id="lkbInFlux" runat="server" ForeColor="Blue" onclick="lkbInFlux_Click">月内会签</asp:LinkButton></FONT></TD>
	</TR>
	<TR>
		<TD align="center" class="list">
		    <uc1:CtrMessageQuery id="CtrMessageQuery1" runat="server"></uc1:CtrMessageQuery>
		</TD>
	</TR>
	<TR height="30">
		<TD align="center" class="listTitle"><asp:button id="cmdQuery" CssClass="btnClass" runat="server"  Text="查询" onclick="cmdQuery_Click"></asp:button>&nbsp;
			<asp:Button id="cmdAttention" runat="server" Text="加为关注" CssClass="btnClass" onclick="cmdAttention_Click"></asp:Button></TD>
	</TR>
</table>
<br />




<TABLE width="98%" align="center" cellpadding=0 border=0 cellspacing=0>
<TR>
<TD class="list">
<asp:datagrid id="gridUndoMsg" runat="server" Width="100%"  AutoGenerateColumns="False"  CssClass="Gridtable" >
	<Columns>
		<asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_status.gif">
			<HeaderStyle Width="16px"></HeaderStyle>
			<ItemTemplate>
				<asp:Image id="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>' >
				</asp:Image>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_important.gif">
			<HeaderStyle Width="12px"></HeaderStyle>
			<ItemTemplate>
				<asp:Image id="Image6" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_important.gif" Visible='<%#GetImportanceVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Important")))%>'>
				</asp:Image>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_hasattachment.gif">
			<HeaderStyle Width="12px"></HeaderStyle>
			<ItemTemplate>
				<asp:Image id="Image10" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_hasattachment.gif" Visible='<%#GetVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Attachment")))%>'>
				</asp:Image>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="主题">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Font-Underline="true"  onmouseout="javascript:hideMe('divShowMessageDetail','none');"  onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>' Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="40%"></HeaderStyle>
                        </asp:TemplateColumn>
		<asp:BoundColumn DataField="FActors" HeaderText="发送人">
			<HeaderStyle Width="10%"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:d}">
			<HeaderStyle Width="10%"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="AppName" HeaderText="所属应用">
			<HeaderStyle Width="25%"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
		<asp:BoundColumn DataField="actortype" HeaderText="类别">
			<HeaderStyle Width="5%"></HeaderStyle>
		</asp:BoundColumn>
		<asp:TemplateColumn HeaderText="处理">
			<HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
			<ItemTemplate>
				<INPUT id=CmdDeal class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"),Convert.ToInt32(DataBinder.Eval(Container.DataItem, "status")))%>' type=button value=详情 name=CmdDeal runat="server">
			</ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="关注">
			<HeaderStyle Wrap Width="5%" HorizontalAlign="Center"></HeaderStyle>
			<ItemStyle HorizontalAlign="Center"></ItemStyle>
			<ItemTemplate>
				<asp:CheckBox id="chkAttention" runat="server"></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>
        <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
	</Columns>
	<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
</asp:datagrid>
    </TD>
</TR>
<tr>
    <td class="list" align="right">
            <uc2:ControlPageFoot ID="cpFlow" runat="server" />
    </td>
</tr>
</TABLE>

<div  id='divShowMessageDetail' style='display: none; position:absolute;  left: 120; top: 90; z-index:2'></div>
</asp:Content>