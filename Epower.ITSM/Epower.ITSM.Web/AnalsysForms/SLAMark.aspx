<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="SLAMark.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.SLAMark"
    Title="SLA达标率" %>

<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <script type="text/javascript" language="javascript">
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

    <script language="javascript" type="text/javascript">
        function btnClick() {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>

    <div style="display: none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>
    <table id="Table1" width="98%" class="listContent" cellpadding="2">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                时间范围:
            </td>
            <td class="list">
                <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()" />
            </td>
            <td class="listTitleRight" style="width: 12%">
                事件状态:
            </td>
            <td class="list">
                <uc6:ctrFlowCataDropListNew ID="CtrFCDDealStatus" runat="server" RootID="1017" ShowType="2" />
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="dpdMastShortName" runat="server" AutoPostBack="True" Width="140px"
                    OnSelectedIndexChanged="dpdMastShortName_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="98%">
        <tr>
            <td valign="top" align="left" width="100%" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            展示数据
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" cellpadding="0" border="0" id="tblResult" runat="server">
        <tr id="tr1" runat="server">
            <td align="left" class="list">
                <asp:DataGrid ID="dgMaterialStat" runat="server" Width="100%" AllowCustomPaging="True"
                    PageSize="100" ShowFooter="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="服务级别" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblLevelName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.servicelevel")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblLevelNameFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="GuidName" HeaderText="标准指标">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="事件数量">
                            <ItemTemplate>
                                <asp:Label ID="lblicount" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.icount")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblicountFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="TimeLimit" HeaderText="时限(小时)" DataFormatString="{0:0.00}">
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="达标数量">
                            <ItemTemplate>
                                <asp:Label ID="lbliRespond" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.iRespond")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lbliRespondFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="实际达标率(%)">
                            <ItemTemplate>
                                <asp:Label ID="lblirate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.irate")%>'></asp:Label>
                                <asp:Label ID="lbl" runat="server" Text='%'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblirateFoot" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblFoot" runat="server" Text='%'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Target" HeaderText="达标率(%)"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table width="98%" cellpadding="0" border="0" id="Table2" runat="server">
        <tr id="tr2" runat="server">
            <td align="left" class="list">
                <asp:DataGrid ID="dgMaterialStat2" runat="server" Width="100%" AllowCustomPaging="True"
                    PageSize="100" ShowFooter="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="服务级别" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblLevelName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.servicelevel")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblLevelNameFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="GuidName" HeaderText="标准指标">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="事件数量">
                            <ItemTemplate>
                                <asp:Label ID="lblicount" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.icount")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblicountFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="TimeLimit" HeaderText="时限(小时)" DataFormatString="{0:0.00}">
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="达标数量">
                            <ItemTemplate>
                                <asp:Label ID="lbliRespond" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.iRespond")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lbliRespondFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="实际达标率(%)">
                            <ItemTemplate>
                                <asp:Label ID="lblirate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.irate")%>'></asp:Label>
                                <asp:Label ID="lbl" runat="server" Text='%'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblirateFoot" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblFoot" runat="server" Text='%'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Target" HeaderText="达标率(%)"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
