<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmWaittingContent.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmWaittingContent" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<!--#include file="../Js/tableSort.js" -->
<head runat="server">
    <title>待办事项</title>  
    <script src="../Js/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="../Js/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <style type="text/css">
        #tooltip
        {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #eee;
            padding: 5px;
            opacity: 0.85;
        }
        #tooltip h3, #tooltip div
        {
            margin: 0;
        }  
        

.gridTable
{
	border: solid 1px #EEEEE0;
}
.gridTable th
{
	border-bottom: solid 1px #EEEEE0;
}
.gridTable td
{
	border-bottom: solid 1px #EEEEE0;
}
.gridTable th
{
	border-right: solid 0px #EEEEE0;
}
.gridTable td
{
	border-right: solid 0px #EEEEE0;
}
.gridTable th
{
	border-left: solid 1px #EEEEE0;
}
.gridTable td
{
	border-left: solid 1px #EEEEE0;
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
    </script>
    
    <script type="text/javascript" language="javascript">
        function ShowDetailsInfo(obj, flowid, appid) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "../Common/frmGetFlowShot.aspx?flowid=" + flowid + "&appid=" + appid }).responseText; } });
        }
    </script>
    <script language="javascript" type="text/javascript">
    function CheckNewItems()
            {
             
                if(xmlhttp == null)
                    xmlhttp = CreateXmlHttpObject();
                if(xmlhttp != null)
                {
                    try
                    {
    					
					    xmlhttp.open("GET", "../Forms/frmContentCheck.aspx?LastMessageID=" + document.all.hidLastMessageID.value, true); 
					    xmlhttp.onreadystatechange = function() 
												    { 
													    if ( xmlhttp.readyState==4 ) 
													    { 
														    if(xmlhttp.responseText=="1")
														    {
    														    
														        window.location = "../Forms/frmWaittingContent.aspx?TypeContent=" + '<%=TypeContent%>'; 
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


    <form id="form1" runat="server" style="margin-left: 0px;margin-top: 0px;">
    <table cellpadding="0" cellspacing="0" width="100%" border="0">
        <tr id="trTitle" visible="false" runat="server">
            <td style="height: 41px;" valign="top" nowrap align="center">
                <uc1:CtrTitle ID="CtrTitle1" runat="server" Title="我登记事件"></uc1:CtrTitle>
            </td>
        </tr>
        <tr id="trNoneData" runat="server" visible="false">
            <td align="center" style="width: 100%;">
                <font color="red">没有我登记事件！</font>
            </td>
        </tr>
        <tr id="trWarnMsg2" runat="server" visible="false">
            <td valign="top" align="center" width="100%">
                <table cellpadding="0" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td valign="top" class="listContent gridTable">
                            <asp:DataGrid ID="gridWarnMsg" runat="server" Width="100%" Visible="False" AutoGenerateColumns="False" 
                                CellPadding="1" CellSpacing="2" >
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                                <Columns>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/Page/heading_status.gif">
                                        <HeaderStyle Width="16px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/page/heading_important.gif">
                                        <HeaderStyle Width="12px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image6" runat="server" Width="14px" Height="14px" ImageUrl="~\Images\page\heading_important.gif"
                                                Visible='<%#GetImportanceVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Important")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/page/heading_hasattachment.gif">
                                        <HeaderStyle Width="12px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image10" runat="server" Width="14px" Height="14px" ImageUrl="~\Images\page\heading_hasattachment.gif"
                                                Visible='<%#GetVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Attachment")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="主题">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"                                                
                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FActors" HeaderText="发送人" Visible="False">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppName" HeaderText="所属应用">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="actortype" HeaderText="类别">
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
        <tr id="trUndoMsg2" runat="server" visible="false">
            <td valign="top" align="center" width="100%">
                <table cellpadding="0" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td valign="top" class="listContent gridTable">
                            <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" Visible="False">
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                                <Columns>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/Page/heading_status.gif">
                                        <HeaderStyle Width="16px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/page/heading_important.gif">
                                        <HeaderStyle Width="12px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image6" runat="server" Width="14px" Height="14px" ImageUrl="~\Images\page\heading_important.gif"
                                                Visible='<%#GetImportanceVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Important")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/page/heading_hasattachment.gif">
                                        <HeaderStyle Width="12px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image10" runat="server" Width="14px" Height="14px" ImageUrl="~\Images\page\heading_hasattachment.gif"
                                                Visible='<%#GetVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Attachment")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="主题">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                    onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>' Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FActors" HeaderText="发送人" Visible="False">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppName" HeaderText="所属应用">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="actortype" HeaderText="类别">
                                        <HeaderStyle Width="5%"></HeaderStyle>
                                    </asp:BoundColumn>
                               <%-- zxl--%>
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
        <tr id="trDealMsg2" runat="server" visible="false">
            <td valign="top" align="center" width="100%">
                <table cellpadding="0" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td valign="top" class="listContent gridTable">
                            <asp:DataGrid ID="dgDealMsg2" runat="server" Width="100%" Visible="False" >
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                                <Columns>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/Page/heading_status.gif">
                                        <HeaderStyle Width="16px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl='<%#GetStatusImage(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsRead")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/page/heading_important.gif">
                                        <HeaderStyle Width="12px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image6" runat="server" Width="14px" Height="14px" ImageUrl="~\Images\page\heading_important.gif"
                                                Visible='<%#GetImportanceVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Important")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderImageUrl="~/Images/page/heading_hasattachment.gif">
                                        <HeaderStyle Width="12px"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="Image10" runat="server" Width="14px" Height="14px" ImageUrl="~\Images\page\heading_hasattachment.gif"
                                                Visible='<%#GetVisible(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Attachment")))%>'>
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="主题">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FActors" HeaderText="发送人" Visible="False">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppName" HeaderText="所属应用">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="actortype" HeaderText="类别">
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
        <tr id="trReceiveMsg2" runat="server" visible="false">
            <td valign="top" align="center" width="100%">
                <table cellpadding="0" width="100%">
                    <tr>
                        <td valign="top" class="listContent gridTable">
                            <asp:DataGrid ID="gridReceiveMsg" runat="server" Width="100%" Visible="False" AutoGenerateColumns="False"  >
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                                <Columns>
                                    <asp:TemplateColumn HeaderImageUrl="~\Images\page\heading_status.gif">
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
                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "FlowID"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FActors" HeaderText="发送人" Visible="False">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AppName" Visible="False" HeaderText="所属应用">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="actortype" HeaderText="类别">
                                        <HeaderStyle Width="5%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="接收" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="44"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                                type="button" value="查看" runat="server" name="CmdDeal">
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
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
        <tr id="trReadMsg2" runat="server" visible="false">
            <td valign="top" align="center" width="100%">
                <table cellpadding="0" width="100%">
                    <tr>
                        <td valign="top" class="listContent gridTable">
                            <asp:DataGrid ID="gridReadMsg" runat="server" Width="100%" Visible="False" CellSpacing="2"
                                CellPadding="1" BorderColor="White" AutoGenerateColumns="False"   BorderWidth="0px">
                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
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
                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FActors" HeaderText="发送人" Visible="False">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppName" HeaderText="所属应用">
                                        <HeaderStyle Width="30%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="actortype"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="查阅" ItemStyle-HorizontalAlign="Center">
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
            </td>
        </tr>
        <tr id="trWaiting2" runat="server" visible="false">
            <td valign="top" align="left" width="100%">
                <table cellpadding="0" width="100%">
                    <tr>
                        <td valign="top" class="listContent gridTable">
                            <asp:DataGrid ID="gridWaitingMsg" runat="server" Width="100%" Visible="False" BorderWidth="0px"
                                AutoGenerateColumns="False"   BorderColor="White" CellPadding="1" CellSpacing="2">
                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
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
                                    <asp:TemplateColumn HeaderText="主题">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FActors" HeaderText="发送人" Visible="False">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppName" HeaderText="所属应用">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="MessageID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DiffMinute"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="AppID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FlowName"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="actortype" HeaderText="类别">
                                        <HeaderStyle Width="5%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="查看" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="44%"></HeaderStyle>
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
            </td>
        </tr>        
        <tr id="trAttention2" runat="server" visible="false">
            <td valign="top" align="left" width="100%">
                <tablewidth="100%">
                    <tr>
                        <td valign="top" class="listContent gridTable">
                            <asp:DataGrid ID="gridAttention" runat="server" Width="100%" Visible="False" BorderWidth="0px"
                                AutoGenerateColumns="False"   BorderColor="White" CellPadding="1" CellSpacing="2"
                                OnItemCommand="gridAttention_ItemCommand">    
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
                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:BoundColumn DataField="nodename" ReadOnly="True" HeaderText="状态">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle Width="5%"></HeaderStyle>
                                        <ItemTemplate>
                                            <input id="Button4" class="btnClass" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                                type="button" value="查看" runat="server" name="CmdDeal">
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle Width="44"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Button ID="Button5" runat="server" Text="取消" SkinID="btnClass1" CommandName="Del"
                                               OnClientClick="delete_confirm();"  />
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
            </td>
        </tr>        
        <tr id="trMyReg2" runat="server" visible="false">
            <td valign="top" align="left" width="100%">
                <table cellpadding="0" width="100%">
                    <tr>
                        <td valign="top" width="100%" class="listContent gridTable" align="center">
                            <asp:DataGrid ID="gridMyReg" runat="server" Visible="False" Width="100%" BorderWidth="0px"
                                AutoGenerateColumns="False"   BorderColor="White" CellPadding="1" CellSpacing="2">
                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="FlowID"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="主题">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" onmouseout="javascript:hideMe('divShowMessageDetail','none');"
                                                onmouseover='<%#GetFlowShotInfo((decimal)DataBinder.Eval(Container.DataItem, "flowid"),(decimal)DataBinder.Eval(Container.DataItem, "appid")) %>'
                                                Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="starttime" DataFormatString="{0:yyyy-MM-dd H:mm}" HeaderText="登记时间">
                                        <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="查看" ItemStyle-HorizontalAlign="Center">
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
        <tr id="tr_ClientJJ" runat="server" visible="false">
            <td valign="top" align="left" width="100%">
                <table cellpadding="0" width="100%">
                    <tr>
                        <td valign="top" class="listContent gridTable">
                            <asp:DataGrid ID="Grid_handOver" runat="server" Width="100%" Visible="False" BorderWidth="0px"
                                AutoGenerateColumns="False"  BorderColor="White" CellPadding="1" 
                                CellSpacing="2" onitemdatabound="Grid_handOver_ItemDataBound" >
                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                <ItemStyle CssClass="tablebody" BackColor="White" HorizontalAlign="Center"></ItemStyle>
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" HorizontalAlign="Center" />
                                <Columns>
                                                                                                        
                                    <asp:BoundColumn Visible="False" DataField="id" HeaderText="id"></asp:BoundColumn>                                                                         
                                      <asp:TemplateColumn HeaderText="序号">
                                        <ItemTemplate>
                                            <%# Container.ItemIndex+1%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn  DataField="IntentClientName" HeaderText="客户名称"></asp:BoundColumn>   
                                    <asp:BoundColumn  DataField="ChanceFromName" HeaderText="客户来源"></asp:BoundColumn>                                                                          
                                    <asp:BoundColumn  DataField="ContactPerson" HeaderText="客户联系人"></asp:BoundColumn>

                                </Columns>
                                  
                                <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                                </PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input id="hidLastMessageID" runat="server" type="hidden" value="0" style="width: 0;
        height: 0; display: none;" />
    <div id='divShowMessageDetail' style='position: absolute; display: none; width: 0;
        height: 0;'>
    </div>
    </form>
</body>
</html>
