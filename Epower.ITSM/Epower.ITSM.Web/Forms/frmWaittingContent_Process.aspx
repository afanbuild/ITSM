<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmWaittingContent_Process.aspx.cs"
    Inherits="Epower.ITSM.Web.Forms.frmWaittingContent_Process" Title="待办事项-按流程分类" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<!--#include file="../Js/tableSort.js" -->
<head runat="server">
    <title>待办事项</title>
    <style type="text/css">
        .hand
        {
            cursor: pointer;
        }
        .GridTable
        {
            border: solid 1px #EEEEE0;
        }
        .GridTable th
        {
            border-bottom: solid 1px #EEEEE0;
        }
        .GridTable td
        {
            border-bottom: solid 1px #EEEEE0;
        }
    </style>
</head>
<body>

    <script language="javascript" type="text/javascript">
    var blnHasShow;
    var xmlhttp = null;

    function CreateXmlHttpObject() {
        try {
            xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");
        }
        catch (e) {
            try {
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e2) { }
        }
        return xmlhttp;
    }


    function absoluteLocation(element, offset) {
        var c = 0; while (element) { c += element[offset]; element = element.offsetParent; } return c;
    }

    function GetFlowShotInfo(obj, flowid, appid) {

            if (blnHasShow == true) {
                return;
            }

            blnHasShow = true;
            //异步获取
            if (xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();

            if (xmlhttp != null) {
                try {

                    xmlhttp.open("GET", "../Common/frmGetFlowShot.aspx?flowid=" + flowid + "&appid=" + appid, true);
                    xmlhttp.setRequestHeader("CONTENT-TYPE ", "application/x-www-form-urlencoded ");
                    xmlhttp.onreadystatechange = function() {
             
                        if (xmlhttp.readyState == 4) {
                            
                            var sXml = xmlhttp.responseText;
                            var object = document.getElementById("divShowMessageDetail");
                            if (object != null) {
                                object.style.display = "";
                            }
                            object.innerHTML = sXml;
                                
                            //alert(object.offsetHeight);				       
                            object.style.left = absoluteLocation(obj, 'offsetLeft') + obj.offsetWidth / 2 + "px";
                            if (absoluteLocation(obj, 'offsetTop') - object.offsetHeight < 20) {
                                object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight / 2 + "px";
                            }
                            else {
                                object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight / 2 - object.offsetHeight + "px";
                            }
                        }

                    }
                    xmlhttp.send(null);
                }
                catch (e3) {
                }
            }

        }

    function hideMe(id, status) {

        var object = document.getElementById(id);

        if (object != null) {
            object.style.display = status;

            if (status == "none")
                blnHasShow = false;
        }
        //alert(object.style.display);
    }

    function HanderClick(strID) {
        igpnl_getPanelById(strID).setExpanded(!igpnl_getPanelById(strID).getExpanded());
    }
    function delete_confirm() {
        event.returnValue = confirm("确定要取消此项关注吗?");
    }
    function GetRandom() {
       return Math.floor(Math.random() * 1000 + 1);
    }
    //-->
    function CheckNewItems()
            {
             
                if(xmlhttp == null)
                    xmlhttp = CreateXmlHttpObject();
                      
                if(xmlhttp != null)
                {
                    try
                    {		
					    xmlhttp.open("GET", "../Forms/frmContentCheck.aspx?LastMessageID=" + document.all.<%=hidLastMessageID.ClientID %>.value, true); 
					    xmlhttp.onreadystatechange = function() 
												    { 
													    if ( xmlhttp.readyState==4 ) 
													    { 
														    if(xmlhttp.responseText=="1")
														    {
    														    
														        window.location = "frmWaittingContent_Process.aspx?TypeContent=" + '<%=TypeContent%>'; 
													        }
													    } 
												    } 
					    xmlhttp.send(null); 
				    }catch(e3){}
                }
            }
		    //自动检测
		    if("true" == '<%=sCheckEnable%>')
		    {
		        var lngTimeVal = <%=lngCheckTime%>;
		        setInterval("CheckNewItems()",lngTimeVal);
		    }
		    
    </script>

    <script type="text/javascript" language="javascript">
        function ShowDetailsInfo(obj, flowid, appid) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "../Common/frmGetFlowShot.aspx?flowid=" + flowid + "&appid=" + appid }).responseText; } });
        }
    </script>

    <table cellpadding="0" cellspacing="0" width="100%" border="0">
        <tr id="trIssue" runat="server" visible="false">
            <td valign="top" align="center" width="100%" class="listContent">            
                <asp:DataGrid ID="gridIssue" runat="server" Width="100%" Visible="false" CssClass="GridTable fixed-grid-border"
                    CellPadding="1" CellSpacing="1" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateColumn HeaderImageUrl="~/Images/Page/heading_status.gif">
                            <HeaderStyle Width="16px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                </asp:Image>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="NumberNo" HeaderText="事件单号">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="摘要" ItemStyle-Width="450px">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Font-Underline="true" Style="word-wrap: break-word;"
                                    onmouseout="javascript:hideMe('divShowMessageDetail','none');" onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                    Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ReceiveTime" HeaderText="登记时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceLevel" HeaderText="服务级别" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceType" HeaderText="事件类别" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>

                        <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>        
        <tr id="trChange" runat="server" visible="false">
            <td valign="top" align="center" width="100%" class="listContent">
                <asp:DataGrid ID="gridChange" runat="server" Width="100%" Visible="False" CssClass="GridTable fixed-grid-border"
                    CellPadding="1" CellSpacing="1" BorderWidth="1px">
                    <Columns> 
                        <asp:TemplateColumn HeaderImageUrl="~/Images/Page/heading_status.gif">
                            <HeaderStyle Width="16px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                </asp:Image>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="NumberNo" HeaderText="变更单号">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="摘要" ItemStyle-Width="450px">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Font-Underline="true" Style="word-wrap: break-word"
                                    onmouseout="javascript:hideMe('divShowMessageDetail','none');" onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                    Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ReceiveTime" HeaderText="登记时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LevelName" HeaderText="变更级别" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <input id="Button1" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                    type="button" value="处理" runat="server" name="CmdDeal">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr id="trByts" runat="server" visible="false">
            <td valign="top" align="center" width="100%" class="listContent">
                <asp:DataGrid ID="gridByts" runat="server" Width="100%" Visible="False" CssClass="GridTable fixed-grid-border"
                    CellPadding="1" CellSpacing="1" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateColumn HeaderImageUrl="~/Images/Page/heading_status.gif">
                            <HeaderStyle Width="16px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                </asp:Image>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="NumberNo" HeaderText="问题单号" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="摘要" ItemStyle-Width="300px">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Font-Underline="true" Style="word-wrap: break-word"
                                    onmouseout="javascript:hideMe('divShowMessageDetail','none');" onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                    Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ReceiveTime" HeaderText="登记时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Problem_LevelName" HeaderText="问题级别" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <input id="Button1" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                    type="button" value="处理" runat="server" name="CmdDeal">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr id="trRelease" runat="server" visible="false">
            <td valign="top" align="center" width="100%" class="listContent">
                <asp:DataGrid ID="gridRelease" runat="server" Width="100%" Visible="False" CssClass="GridTable fixed-grid-border"
                    CellPadding="1" CellSpacing="1" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateColumn HeaderImageUrl="~/Images/Page/heading_status.gif">
                            <HeaderStyle Width="16px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                </asp:Image>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="摘要" ItemStyle-Width="450px">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Font-Underline="true" Style="word-wrap: break-word"
                                    onmouseout="javascript:hideMe('divShowMessageDetail','none');" onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                    Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ReceiveTime" HeaderText="登记时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <input id="Button1" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                    type="button" value="处理" runat="server" name="CmdDeal">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr id="trOther" runat="server" visible="false">
            <td valign="top" align="center" width="100%" class="listContent">
                <asp:DataGrid ID="gridOther" runat="server" Width="100%" Visible="False" CssClass="GridTable fixed-grid-border"
                    CellPadding="1" CellSpacing="1" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateColumn HeaderImageUrl="~/Images/Page/heading_status.gif">
                            <HeaderStyle Width="16px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                </asp:Image>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Name" HeaderText="流程名称" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="True" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="摘要" ItemStyle-Width="400px">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Font-Underline="true" Style="word-wrap: break-word"
                                    onmouseout="javascript:hideMe('divShowMessageDetail','none');" onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                    Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ReceiveTime" HeaderText="登记时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <input id="Button1" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                    type="button" value="处理" runat="server" name="CmdDeal">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <input id="hidLastMessageID" runat="server" type="hidden" value="0" style="width: 0;
        height: 0; display: none;" />
    <div id='divShowMessageDetail' style='position: absolute; display: none; width: 0;
        height: 0;'>
    </div>
</body>
</html>
