<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_Equ_PatrolBaseQuery.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_Equ_PatrolBaseQuery"
    Title="巡检维保查询" %>

<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc7" %>
<%@ Register Src="../Controls/CtrDateAndTime.ascx" TagName="ctrdateandtime" TagPrefix="uc5" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/controlpage.ascx" TagName="controlpage" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc2" %>
<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Begin: 引入基础脚本库-->
    <script type="text/javascript" language="javascript" src="../js/epower.base.js"></script>
    <!--End: 引入基础脚本库-->
    <script language="javascript" type="text/javascript">
    function OpenDeleteFlow(obj)  //删除流程
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete","hidDelete")).value;
        
        var url = "../Common/frmFlowDelete.aspx?FlowID="+ FlowID +"&Opener_hiddenDelete=<%=hidd_btnDelete.ClientID %>" +"&TypeFrm=frm_KBBaseQuery";;
             
        var height=($(document).height() - 230)/2 + +$(window).scrollTop();
        var width=($(document).width() - 320)/2 + +$(window).scrollLeft();              
        
        
        var _xy = epower.tools.computeXY('c', window, 320, 230);                
        width = _xy.x;
        height = _xy.y;
                
        
		open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=320px,height=230px,left=' + width +',top=' + height );     
    }
    
    function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              //var className;
              //var objectFullName;
              var tableCtrl;
              //objectFullName = <%=tr2.ClientID%>.id;
              //className = objectFullName.substring(0,objectFullName.indexOf("tr2")-1);
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

    <table width='98%' cellpadding="2" cellspacing="0" class="listContent GridTable">
        <tr>
            <td class='listTitleRight' width="12%">
                流程状态
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="cboStatus" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                标题
            </td>
            <td class='list'>
                <uc4:CtrFlowFormText ID="CtrFlowTitle" runat="server" TextToolTip="标题" MustInput="false" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' width="12%">
                登记日期
            </td>
            <td class="list" nowrap colspan="3">
                <uc7:ctrDateSelectTime ID="ctrDateTime" runat="server" />
            </td>
        </tr>
    </table>
    <table id="Table12" width="98%" align="center" runat="server">
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
    <table class="listContent" width="98%" cellpadding="2" cellspacing="0" id="Table2"
        style="display: none">
        <tr>
            <td class='listTitleRight' width="12%">
                登记人
            </td>
            <td class="list" width="35%">
                <asp:TextBox ID='txtRegName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' width="12%">
                登记人部门
            </td>
            <td class="list">
                <asp:TextBox ID='txtRegDeptName' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                资产名称
            </td>
            <td class="list">
                <asp:TextBox ID='txtEquName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight'>
                巡检项
            </td>
            <td class="list">
                <asp:TextBox ID='txtItemName' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                巡检人
            </td>
            <td class='list' colspan="3">
                <asp:TextBox ID='txtPatrolName' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="grd" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="grd_ItemCreated"
                    OnDeleteCommand="gridUndoMsg_DeleteCommand" OnItemDataBound="grd_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="endtime"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Title' HeaderText='标题' ItemStyle-Width="60%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='RegUserName' HeaderText='登记人' ItemStyle-Width="5%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='RegDeptName' HeaderText='登记人部门' ItemStyle-Width="10%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="登记日期">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "RegTime", "{0:d}")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="流程状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle Width="60" HorizontalAlign="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='详情' runat="server">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"
                            HeaderText="删除">
                            <HeaderStyle Width="60"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" SkinID="btnClass1"
                                    OnClientClick="OpenDeleteFlow(this);" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                
                <asp:Button ID="hidd_btnDelete" runat="server" style="display:none;"  Text="删除时重新调用数据" 
                    onclick="hidd_btnDelete_Click" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc6:ControlPageFoot ID="cpfPatrolBase" runat="server" />
            </td>
        </tr>
    </table>

    <script language="javascript">	
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
    </script>

</asp:Content>
