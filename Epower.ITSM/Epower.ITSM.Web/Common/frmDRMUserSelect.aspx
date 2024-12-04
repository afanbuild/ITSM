<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmDRMUserSelect.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmDRMUserSelect"
    Title="客户资料" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

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

    <script language="javascript" type="text/javascript">
$.ajaxSetup({ cache: false });
function ShowDetailsInfo(obj,id) 
	{
    	$("#"+obj.id).tooltip({showURL: false,bodyHandler: function() {return $.ajax({ type: "GET", async: false, url:  "../AppForms/frmBr_CustomXmlHttp.aspx?id="+id }).responseText;}});
    }

var openobj = window;
if(typeof(window.dialogArguments) == "object")
{
    openobj =  window.dialogArguments;
}
//资产维修记录
function SelectService(obj) 
{
    var lngID = document.getElementById(obj.id.replace("CmdService","hidID")).value;
	openobj.open("../AppForms/frmIssueList.aspx?NewWin=true&ID=" + lngID ,'','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');
	event.returnValue = false;
}
//抱怨投诉
function SelectByts(obj) 
{
    var lngID = document.getElementById(obj.id.replace("CmdByts","hidID")).value;
	openobj.open("../AppForms/frmBYTSList.aspx?NewWin=true&?ID=" + lngID ,'','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');
	event.returnValue = false;
}

function ServerOndblclick(value1,value2,value3,value4,value5,value6,value7,value8,value9,value10,value11,value12,value13,value14,value15,value16,value17)
{
        var arr = new Array();        
        arr[1]=value1;
        arr[2]=value2;
        arr[3]=value3;
        arr[4]=value4;
        arr[5]=value5;
        arr[6]=value6;
        arr[7]=value7;
        arr[13]=value8;
        arr[14]=value9;
        arr[15]=value10;
        arr[8]=value11;
        arr[9]=value12;
        arr[10]=value13;
        arr[11]=value14;
        arr[12]=value15;
        arr[16]=value16;
        arr[17] = value17;

        window.parent.returnValue = arr;
        top.close();
    }
    
    function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              var tableCtrl;
              tableCtrl = document.all.item(TableID);
              if(imgCtrl.src.indexOf("icon_expandall") != -1)
              {
                tableCtrl.style.display ="";
                imgCtrl.src = ImgMinusScr ;
                document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id;
              }
              else
              {
                tableCtrl.style.display ="none";
                imgCtrl.src = ImgPlusScr ;	
                var temp = document.all.<%=hidTable.ClientID%>.value;
                document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
              }
        }
    </script>

    <table width='98%' class="listContent" cellpadding="2" cellspacing="0">
        <tr>
            <td class="listTitleRight" align="right" style="width: 12%;">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddltMastCustID" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" tyle="width:12%">
                <asp:Literal ID="LitCustName" runat="server" Text="客户名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table id="Table12" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_expandall.gif"
                                width="16" align="absbottom" />
                            高级条件
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width='98%' class="listContent" cellpadding="2" cellspacing="0" style="display: none;"
        id="Table2">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitCustAddress" runat="server" Text="联系地址"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtaddress" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitCustomerType" runat="server" Text="客户类型"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server" ContralState="eNormal"
                    RootID="1019" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitContact" runat="server" Text="联系人"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtcontract" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitCTel" runat="server" Text="联系电话"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txttel" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustomCode" runat="server" Text="客户代码"></asp:Literal>
            </td>
            <td class="list" align="left" colspan="3">
                <asp:TextBox ID="txtCustomCode" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <input id="hidTable" value="" runat="server" type="hidden" />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgdrmuser" runat="server" Width="100%" DataKeyField="client_id"
                    AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgProduct_ItemCommand" OnItemDataBound="dgdrmuser_ItemDataBound"
                    OnItemCreated="dgdrmuser_ItemCreated">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="client_id" HeaderText="client_id"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MName" HeaderText="服务单位">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustomerTypeName" HeaderText="客户类型">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="客户名称">
                            <ItemTemplate>
                                <asp:Label ID="client_name" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.client_name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="client_contact" HeaderText="联系人">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="client_phone" HeaderText="联系人电话">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Email" HeaderText="电子邮件">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustomCode" HeaderText="客户代码">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="client_address" HeaderText="地址">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="lnkselect" CommandName="Select" Text="选择" SkinID="btnClass1" />
                            </ItemTemplate>
                            <HeaderStyle Width="44" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr> <%--class="listTitleRight"--%>
            <td align="right">
                <uc1:ControlPageFoot ID="cpfECustomerInfo" runat="server" PageSize="10" />
            </td>
        </tr>
    </table>

    <script language="javascript">	
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
            var tableid="Table2";
            var tableCtrl = document.all.item(tableid);
            var ImgID = "Img2";
            var imgCtrl = document.all.item(ImgID)
            tableCtrl.style.display ="none";
            imgCtrl.src = ImgPlusScr ;	
        }
    }
    </script>

</asp:Content>
