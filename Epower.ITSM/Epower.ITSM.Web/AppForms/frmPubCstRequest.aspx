<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeBehind="frmPubCstRequest.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmPubCstRequest"
    Title="服务请求处理" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript" src="../Js/jquery-1.3.2.js"> </script>

    <script language="javascript" type="text/javascript" src="../Js/jquery-ui-1.7.2.custom.min.js"> </script>

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

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

        function GetFlowShotInfo(obj, id) {

            if (blnHasShow == true) {
                return;
            }

            blnHasShow = true;
            //异步获取
            if (xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();

            if (xmlhttp != null) {
                try {

                    xmlhttp.open("GET", "../Common/frmGetPubRequestShow.aspx?id=" + id, true);
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
            event.returnValue = confirm("确定要删除吗?");
        }

        function deal_confirm(obj) {
            var ddlFlow = document.getElementById(obj.id.replace("lnkdeal", "ddlIssueTemplates"));
            if (ddlFlow.length == 0) {
                alert("请先建立事件请求模板！");
                event.returnValue = false;
            }
        }
        function CheckNewItems()
        {
         
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();
                  
            if(xmlhttp != null)
            {
                try
                {
					xmlhttp.open("GET", "frmReqCheck.aspx?LastID=" + document.all.hidLastMessageID.value, true); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText=="1")
														{
														    
														    window.location = "frmPubCstRequest.aspx"; 
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
        $.ajaxSetup({ cache: false });
        function ShowDetailsInfo(obj, id) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "../Common/frmGetPubRequestShow.aspx?id=" + id }).responseText; } });
        }
    
    </script>

    <table cellpadding="0" width="98%" cellspacing="0" border="0">
        <tr>
            <td valign="top">
                <asp:DataGrid ID="gridReceiveMsg" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="gridReceiveMsg_ItemCommand" OnItemDataBound="gridReceiveMsg_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn HeaderText="请求来源">
                             <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "contract") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="inDate" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd H:mm}">
                            <HeaderStyle Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="事件模板">
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlIssueTemplates" runat="server" Width="141px">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn  HeaderText="处理">
                            <ItemTemplate>
                                <asp:Button ID="lnkdeal" SkinID="btnClass1" runat="server" Text="处理" CommandName="deal"
                                    OnClientClick="deal_confirm(this);" />
                            </ItemTemplate>
                            <HeaderStyle Width="44px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn  HeaderText="删除">
                            <ItemTemplate>
                                <asp:Button ID="lnkdele" Width="44" SkinID="btnClass1" runat="server" Text="删除"
                                    CommandName="dele" OnClientClick="delete_confirm();" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Content" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CTel" Visible="False"></asp:BoundColumn>
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
</asp:Content>

