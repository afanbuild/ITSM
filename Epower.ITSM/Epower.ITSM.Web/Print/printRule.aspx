<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="printRule.aspx.cs" Inherits="Epower.ITSM.Web.Print.printRule" Title="打印" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="uc1" TagName="CtrlProcess" Src="~/Controls/CtrlProcess.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/Js/App_Common.js"> </script>

    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/Js/App_Base.js"> </script>

    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/JS/ows.js"></script>

    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/JS/OWSBROWS.JS"></script>

    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/Controls/Calendar/Popup.js"></script>

    <script>
function printDiv()
{
//    if(confirm("确认打印，取消放弃打印！"))
//    {
        var isProcess=document.all.<%=hidProcess.ClientID %>.value;
        var headstr = "<html><head><title></title></head><body>";
        var footstr = "</body>";
        //var newstr = document.all.<%=DivPrint.ClientID %>.innerHTML;
        var newstr = document.all.<%=Table1.ClientID %>.innerHTML;
        var oldstr = document.body.innerHTML;
        if(isProcess==1)
        {
            var processstr=document.all.<%=divProcess.ClientID %>.innerHTML;
            document.body.innerHTML = headstr+newstr+processstr+footstr;
        }
        else
        {
            document.body.innerHTML = headstr+newstr+footstr;
        }
        window.print();
        document.body.innerHTML = oldstr; 
//     }
}
    </script>

    <asp:HiddenField ID="hidProcess" runat="server" Value="-1" />
    <div id="divBtnPrint" runat="server">
        <input type="button" id="btnPrint" onclick="printDiv();" class="btnClass" value="确认打印" /><br />
        <br />
    </div>
     <table id="Table1" runat="server" class="listContent" width="80%" align="center"
        cellpadding="0" border="0">
        <tr>
            <td class="list">
                <div id="DivPrint" runat="server" style="width: 80%; margin-bottom: 10px;">
                </div>
            </td>
        </tr>
    </table>
    <table id="divProcess" runat="server" class="listContent" width="80%" align="center"
        cellpadding="0" border="0">
        <tr valign="top">
            <td class="listTitle">
                &nbsp;&nbsp;处理过程
            </td>
        </tr>
        <tr>
            <td class="list">
                <uc1:CtrlProcess ID="CtrlProcess1" runat="server"></uc1:CtrlProcess>
            </td>
        </tr>
    </table>
</asp:Content>
