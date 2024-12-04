<%@ Page Language="C#" MasterPageFile="~/MasterPageSingle.master" AutoEventWireup="True"
    Inherits="Epower.ITSM.Web.FrmContent" Title="Untitled Page" CodeBehind="FrmContent.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPageSingle.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .hand
        {
        	cursor:pointer;
        }
    </style>
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
        
        function CheckNewItems()
        {
         
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();
                  
            if(xmlhttp != null)
            {
                try
                {
					
					xmlhttp.open("GET", "frmContentCheck.aspx?LastMessageID=" + document.all.hidLastMessageID.value, true); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText=="1")
														{
														    
														    window.location = "frmContent.aspx"; 
													    }
													} 
												} 
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
        
        
      
		function HanderClick(strID)
		{
			igpnl_getPanelById(strID).setExpanded(!igpnl_getPanelById(strID).getExpanded());
		}
		function Cancel_confirm()
		{
				event.returnValue =confirm("确定要取消此项关注吗?");
			
		}
		function GetRandom() {
            return Math.floor(Math.random() * 1000 + 1);
        }
		//自动检测
		if("true" == '<%=sCheckEnable%>')
		{
		    var lngTimeVal = <%=lngCheckTime%>;
		    setInterval("CheckNewItems()",lngTimeVal);
		}
		
		//1分钟测试
		//setInterval("CheckNewItems()",60000);

        function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              var className;
              var objectFullName;
              var tableCtrl;
              objectFullName = <%=Table8.ClientID%>.id;
              className = objectFullName.substring(0,objectFullName.indexOf("Table8")-1);
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
		
//-->
    </script>

    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top" align="center">
                <table align="center" id="Table8" runat="server" width="98%" cellspacing="0" cellpadding="0"
                    border="0">
                    <tr id="trTitle1" runat="server" visible="false">
                        <td style="height: 41px;" valign="top" nowrap align="center" class="list">
                            <div style="margin-top:20px;">                            
                            <p>
                                <uc1:CtrTitle ID="CtrTitle1" runat="server"></uc1:CtrTitle>
                            </p>
                            </div>
                        </td>
                    </tr>
                    <tr id="trTitle" visible="false" runat="server">
                        <td style="height: 41px;" valign="top" nowrap align="center">
                            <uc1:CtrTitle ID="CtrTitle2" runat="server" Title="我登记事件"></uc1:CtrTitle>
                        </td>
                    </tr>
                    <tr id="trReceiveMsg1" runat="server">
                        <td valign="top" align="left" class="listTitleNew">
                            <table width="150" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="24" class="bt_di">
                                        <img class="icon" id="Img1" onclick="ShowTable(this);" style="cursor: hand" height="16"
                                            src="../Images/icon_collapseall.gif" width="16" align="absbottom" />
                                        待接收事项
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trReceiveMsg2" runat="server">
                        <td valign="top" align="center">
                            <table id="Table1" width="100%" cellpadding="0" runat="server" class="listContent">
                                <tr>
                                    <td align="right" class="listTitle" height="18">
                                        <a href="OA_MessageQuery.aspx" target="_self">
                                            <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="listContent">
                                        <asp:DataGrid ID="gridReceiveMsg" runat="server" Width="100%" Visible="False" AutoGenerateColumns="False"  CssClass="Gridtable" 
                                            OnItemCreated="gridReceiveMsg_ItemCreated">
                                            <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                            <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                            <HeaderStyle CssClass="listTitle"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_status.gif">
                                                    <HeaderStyle Width="16px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image2" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                                        </asp:Image>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_important.gif">
                                                    <HeaderStyle Width="12px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image5" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_important.gif"
                                                            Visible='<%#GetImportanceVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Important")))%>'>
                                                        </asp:Image>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_hasattachment.gif">
                                                    <HeaderStyle Width="12px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image9" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_hasattachment.gif"
                                                            Visible='<%#GetVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Attachment")))%>'>
                                                        </asp:Image>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="主题">
                                                    <ItemTemplate>
                                                        <div style=" text-align:left; margin-left:10px;"> 
                                                        <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                            onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                            Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                                            </div>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="FActors" HeaderText="发送人">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm:ss}">
                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AppName" HeaderText="所属应用">
                                                    <HeaderStyle Width="25%"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="actortype" HeaderText="类别">
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="接收" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderStyle Width="44"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                                            type="button" value="查看" runat="server" name="CmdDeal">
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                                            </PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trUndoMsg1" runat="server">
                        <td valign="top" align="left" class="listTitleNew">
                            <table width="150" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="24" class="bt_di">
                                        <img class="icon" id="Img2" onclick="ShowTable(this);" style="cursor: hand" height="16"
                                            src="../Images/icon_collapseall.gif" width="16" align="absbottom" />
                                        待办事项
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trUndoMsg2" runat="server">
                        <td valign="top" align="center">
                            <table id="Table2" width="100%" runat="server" cellpadding="0" class="listContent">
                                <tr>
                                    <td align="right" class="listTitle" height="18">
                                        <a href="OA_MessageQuery.aspx" target="_self">
                                            <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="listContent">
                                        <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" Visible="False" BorderWidth="0px"
                                            CellSpacing="2" CellPadding="1" BorderColor="White" AutoGenerateColumns="False"  CssClass="Gridtable" 
                                            OnItemCreated="gridUndoMsg_ItemCreated">
                                            <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                            <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                            <HeaderStyle CssClass="listTitle"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_status.gif">
                                                    <HeaderStyle Width="16px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                                        </asp:Image>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_important.gif">
                                                    <HeaderStyle Width="12px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image6" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_important.gif"
                                                            Visible='<%#GetImportanceVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Important")))%>'>
                                                        </asp:Image>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_hasattachment.gif">
                                                    <HeaderStyle Width="12px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image10" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_hasattachment.gif"
                                                            Visible='<%#GetVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Attachment")))%>'>
                                                        </asp:Image>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="主题">
                                                    <ItemTemplate>
                                                    <div style=" text-align:left; margin-left:10px;"> 
                                                        <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                            onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                            Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                                            </div>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="FActors" HeaderText="发送人">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm:ss}">
                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AppName" HeaderText="所属应用">
                                                    <HeaderStyle Width="25%"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="actortype" HeaderText="类别">
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                                            </PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trReadMsg1" runat="server">
                        <td valign="top" align="left" class="listTitleNew">
                            <table width="150" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="24" class="bt_di">
                                        <img class="icon" id="Img3" onclick="ShowTable(this);" style="cursor: hand" height="16"
                                            src="../Images/icon_collapseall.gif" width="16" align="absbottom" />
                                        阅知事项
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trReadMsg2" runat="server">
                        <td valign="top" align="center">
                            <font face="宋体">
                                <table id="Table3" cellpadding="0" width="100%" runat="server" class="listContent">
                                    <tr>
                                        <td align="right" class="listTitle" height="18">
                                            <a href="OA_MessageQuery.aspx" target="_self">
                                                <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="listContent">
                                            <asp:DataGrid ID="gridReadMsg" runat="server" Width="100%" Visible="False" AutoGenerateColumns="False"  CssClass="Gridtable" 
                                                BorderWidth="0px" OnItemCreated="gridReadMsg_ItemCreated">
                                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                                <HeaderStyle CssClass="listTitle"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_status.gif">
                                                        <HeaderStyle Width="16px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Image ID="Image3" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                                            </asp:Image>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_important.gif">
                                                        <HeaderStyle Width="12px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Image ID="Image7" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_important.gif"
                                                                Visible='<%#GetImportanceVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Important")))%>'>
                                                            </asp:Image>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_hasattachment.gif">
                                                        <HeaderStyle Width="12px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Image ID="Image11" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_hasattachment.gif"
                                                                Visible='<%#GetVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Attachment")))%>'>
                                                            </asp:Image>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="主题">
                                                        <ItemTemplate>
                                                            <div style=" text-align:left; margin-left:10px;"> 
                                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                                                </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="FActors" HeaderText="发送人">
                                                        <HeaderStyle Width="10%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm:ss}">
                                                        <HeaderStyle Width="15%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="AppName" HeaderText="所属应用">
                                                        <HeaderStyle Width="30%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="actortype"></asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="查阅" HeaderStyle-VerticalAlign="Middle">
                                                        <HeaderStyle Width="44"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <input id="Button2" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                                                type="button" value="查阅" runat="server" name="CmdDeal">
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                                                </PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                            </font>
                        </td>
                    </tr>
                    <tr id="trWaiting1" runat="server">
                        <td valign="top" align="left" class="listTitleNew">
                            <table width="150" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="24" class="bt_di">
                                        <img class="icon" id="Img4" onclick="ShowTable(this);" style="cursor: hand" height="16"
                                            src="../Images/icon_collapseall.gif" width="16" align="absbottom" />
                                        挂起的事项
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trWaiting2" runat="server">
                        <td valign="top" align="left">
                            <font face="宋体">
                                <table id="Table4" cellpadding="0" width="100%" runat="server" class="listContent">
                                    <tr>
                                        <td align="right" class="listTitle" height="18">
                                            <a href="OA_MessageQuery.aspx" target="_self">
                                                <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="listContent">
                                            <asp:DataGrid ID="gridWaitingMsg" runat="server" Width="100%" Visible="False" BorderWidth="0px"
                                                AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="gridWaitingMsg_ItemCreated">
                                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                                <HeaderStyle CssClass="listTitle"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_status.gif">
                                                        <HeaderStyle Width="16px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Image ID="Image4" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                                            </asp:Image>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_important.gif">
                                                        <HeaderStyle Width="12px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Image ID="Image8" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_important.gif"
                                                                Visible='<%#GetImportanceVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Important")))%>'>
                                                            </asp:Image>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderImageUrl="..\Images\page\heading_hasattachment.gif">
                                                        <HeaderStyle Width="12px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Image ID="Image12" runat="server" Width="14px" Height="14px" ImageUrl="..\Images\page\heading_hasattachment.gif"
                                                                Visible='<%#GetVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Attachment")))%>'>
                                                            </asp:Image>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="主题" >
                                                        <ItemTemplate   >
                                                            <div style=" text-align:left; margin-left:10px;"> 
                                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="FActors" HeaderText="发送人">
                                                        <HeaderStyle Width="10%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm:ss}">
                                                        <HeaderStyle Width="15%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="AppName" HeaderText="所属应用">
                                                        <HeaderStyle Width="25%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="actortype" HeaderText="类别">
                                                        <HeaderStyle Width="5%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="查看" HeaderStyle-VerticalAlign="Middle">
                                                        <HeaderStyle Width="44"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <input id="Button3" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                                                type="button" value="查看" runat="server" name="CmdDeal">
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                                                </PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                            </font>
                        </td>
                    </tr>
                    <tr id="trAttention1" runat="server">
                        <td valign="top" align="left" class="listTitleNew">
                            <table width="150" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="24" class="bt_di">
                                        <img class="icon" id="Img5" onclick="ShowTable(this);" style="cursor: hand" height="16"
                                            src="../Images/icon_collapseall.gif" width="16" align="absbottom" />
                                        关注事项
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trAttention2" runat="server">
                        <td valign="top" align="left">
                            <font face="宋体">
                                <table id="Table5" cellpadding="0" width="100%" runat="server" class="listContent">
                                    <tr>
                                        <td align="right" class="listTitle" height="18">
                                            <a href="OA_MessageQuery.aspx" target="_self">
                                                <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="listContent">
                                            <asp:DataGrid ID="gridAttention" runat="server" Width="100%" Visible="False" BorderWidth="0px"
                                                AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="gridAttention_ItemCommand" OnItemCreated="gridAttention_ItemCreated">
                                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                                <HeaderStyle CssClass="listTitle"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn Visible="False" DataField="id" HeaderText="id"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Status" HeaderText="Status"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="messageid" HeaderText="messageid">
                                                        <HeaderStyle Width="15%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="flowid" ReadOnly="True" HeaderText="flowid">
                                                        <HeaderStyle Wrap="False" Width="30%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="receiverid" HeaderText="receiverid">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="主题">
                                                        <ItemTemplate>
                                                            <div style=" text-align:left; margin-left:10px;"> 
                                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                                                </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="name" HeaderText="处理人员">
                                                        <HeaderStyle Width="35%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="nodename" ReadOnly="True" HeaderText="状态">
                                                        <HeaderStyle Width="20%"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="" ItemStyle-HorizontalAlign="center">
                                                        <HeaderStyle Width="5%"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <input id="Button4" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                                                type="button" value="查看" runat="server" name="CmdDeal">
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="" ItemStyle-HorizontalAlign="center">
                                                        <HeaderStyle Width="44"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button5" runat="server" Text="取消" SkinID="btnClass1" CommandName="Del"
                                                                OnClientClick="Cancel_confirm();" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="AppID" Visible="False"></asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                                                </PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                            </font>
                        </td>
                    </tr>
                    <tr id="trMyReg1" runat="server">
                        <td valign="top" align="left" class="listTitleNew">
                            <table width="150" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="24" class="bt_di">
                                        <img class="icon" id="Img6" onclick="ShowTable(this);" style="cursor: hand" height="16"
                                            src="../Images/icon_collapseall.gif" width="16" align="absbottom" />
                                        我登记事件
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trMyReg2" runat="server" visible="false">
                        <td valign="top" align="left" width="100%">
                            <table id="Table6" cellpadding="0" width="100%" runat="server" class="listContent">
                                <tr>
                                    <td align="right" class="listTitle" height="18">
                                        <a href="OA_MessageQuery.aspx" target="_self">
                                            <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="100%" class="listContent" align="center">
                                        <asp:DataGrid ID="gridMyReg" runat="server" Visible="False" Width="100%" BorderWidth="0px"
                                            AutoGenerateColumns="False"  CssClass="Gridtable"  BorderColor="White" CellPadding="1" CellSpacing="2">
                                            <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                            <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                            <HeaderStyle CssClass="listTitle"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="FlowID"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="主题">
                                                    <ItemTemplate>
                                                        <div style=" text-align:left; margin-left:10px;"> 
                                                        <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                            onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                            Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                                            </div>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="starttime" DataFormatString="{0:yyyy-MM-dd H:mm}" HeaderText="登记时间">
                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="查看" HeaderStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <input id="Button3" class="btnClass1" onclick='<%#GetMyRegUrl((decimal)DataBinder.Eval(Container.DataItem, "Flowid"))%>'
                                                            type="button" value="详情" runat="server" name="CmdDeal">
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="44" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="AppID" Visible="False"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" width="100%" class="list">
                <font color="red">
                    <asp:Literal ID="litlShowNone" Text="没有我的事项！" runat="server" Visible="False"></asp:Literal></font>
            </td>
        </tr>
        <tr id="trNoneData" runat="server" visible="false">
            <td align="center" style="width: 100%;">
                <font color="red">没有我登记事件！</font>
            </td>
        </tr>
    </table>
    <input id="hidLastMessageID" runat="server" type="hidden" value="0" />
    <div id='divShowMessageDetail' style='display: none; position: absolute; left: 120;
        top: 90; z-index: 2'>
    </div>
</asp:Content>
