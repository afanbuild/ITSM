<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="MailAndMessageRuleManager.aspx.cs" Inherits="Epower.ITSM.Web.MailAndMessageRule.MailAndMessageRuleManager" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc1" %>
<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script type="text/javascript">
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgMailMessageTem") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }
        function ShowTable(imgCtrl)//显示隐藏条件
        {
            var ImgPlusScr = "../Images/icon_expandall.gif";      	// pic Plus  +
            var ImgMinusScr = "../Images/icon_collapseall.gif";     // pic Minus - 
            var TableID = imgCtrl.id.replace("Img", "Table");

            var tableCtrl;
            tableCtrl = document.all.item(TableID);

            if (imgCtrl.src.indexOf("icon_expandall") != -1) {
                tableCtrl.style.display = "";
                imgCtrl.src = ImgMinusScr;
                document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id;
            }
            else {
                tableCtrl.style.display = "none";
                imgCtrl.src = ImgPlusScr;
                document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
            }
        }
        function ShowDetailsInfo(obj, id) {
            //$("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?ZHServiceDP=" + id }).responseText; } });
        }
    </script>

    <style>
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
    </style>
    <input id="hidTable" value="" runat="server" type="hidden" />
    <table width="98%" cellpadding="2" cellspacing="0" class="listContent GridTable">
        <tr>
            <td class="listTitleRight" style="width: 12%;"><asp:Literal ID="LitRuleName" runat="server" Text="规则名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%"><asp:TextBox ID="txtRuleName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight" style="width: 12%;">
                <asp:Literal ID="LitStatus" runat="server" Text="是否启用"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152px">
                    <asp:ListItem Text="" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                    <asp:ListItem Text="启用" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitSystemName" runat="server" Text="应用名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="cboApp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboApp_SelectedIndexChanged"
                    Width="152px">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitModelName" runat="server" Text="流程模型名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="cboFlowModel" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table id="Table12" width="98%" align="center" runat="server" class="listNewContent" style="display:none;">
        <tr id="tr2" runat="server">
            <td valign="top" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_expandall.gif"
                                runat="server" width="16" align="absbottom" />
                            高级条件
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table2" width="98%" cellpadding="2" cellspacing="0" class="listContent"
        runat="server" style="display: none">
        <tr>
            <td class="listTitleRight" style="width: 12%;">
                <asp:Literal ID="LitMailContent" runat="server" Text="邮件模板内容"></asp:Literal>
            </td>
            <td class="list" style="width: 35%;">
                <asp:TextBox ID="txtMailContent" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight" style="width: 12%;">
                <asp:Literal ID="LitModelContent" runat="server" Text="短信模板内容"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtModelContent" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgMailMessageTem" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgMailMessageTem_ItemCommand"
                    OnItemDataBound="dgMailMessageTem_ItemDataBound" OnItemCreated="dgMailMessageTem_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="规则名称">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbRuleName" Text='<%#DataBinder.Eval(Container, "DataItem.RuleName")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='SystemName' HeaderText='应用名称'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='ModelName' HeaderText='流程模型名称'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='ReceiversTypeName' HeaderText='接收类型名称'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='RUserNameList' HeaderText='接收人员姓名列表'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='SenderTypeName' HeaderText='发送类型名称'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='MailTitle' HeaderText='邮件标题'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="是否启用">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbStatus" Text='<%#DataBinder.Eval(Container, "DataItem.Status")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="5%" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Remark" HeaderText="备注">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='TemplateID' Visible="false" HeaderText='模板ID'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc4:ControlPageFoot ID="CtrcpfMailMessage" runat="server" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">	
             
    if(typeof(document.all.<%=hidTable.ClientID%>) != "undefined")
    {
        var temp = document.all.<%=hidTable.ClientID%>.value;
        var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
        var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus -
        if(temp!="")
        {
            var arr=temp.split(",");
            for(i=1;i<arr.length;i++)
            {
                var tableid=arr[i];
                var tableCtrl = document.all.item(tableid);
                tableCtrl.style.display ="";
                var ImgID = tableid.replace("Table","Img");
                var imgCtrl = document.all.item(ImgID)
                imgCtrl.src = ImgMinusScr ;	
            }
        }
        else
        { 
            
            var tableid = '<%=Table2.ClientID%>';
            var tableCtrl = document.all.item(tableid);
            var ImgID =tableid.replace("Table","Img");
            var imgCtrl = document.all.item(ImgID)
            tableCtrl.style.display ="none";
            imgCtrl.src = ImgPlusScr ;	
        }
    }
    </script>

</asp:Content>
