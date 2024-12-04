<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="frm_BR_ProgressBar_Set.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frm_BR_ProgressBar_Set"
    Title="进度条配置" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
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
                var temp = document.all.<%=hidTable.ClientID%>.value;
                document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
              }
              else
              {
                tableCtrl.style.display ="none";
                imgCtrl.src = ImgPlusScr ;	
                document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id;
              }
        }
    </script>

    <input id="hidTable" value="" runat="server" type="hidden" />
    <table style='width: 98%' cellpadding="2" cellspacing="0" class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                应用名称
            </td>
            <td class='list' style='width: 35%;'>
                <asp:DropDownList ID="cboApp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboApp_SelectedIndexChanged"
                    Width="152px">
                </asp:DropDownList>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                流程模型
            </td>
            <td class='list'>
                <asp:DropDownList ID="cboFlowModel" runat="server" Width="152px" AutoPostBack="True"
                    OnSelectedIndexChanged="cboFlowModel_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table id="Table1" width="98%" align="center" runat="server" class="listNewContent">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            规则配置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" align="center" id="Table2">
        <tr>
            <td>
                <asp:DataGrid ID="dgCondition" runat="server" Width="100%" OnItemDataBound="dgCondition_ItemDataBound"
                    CssClass="fixed-grid-border2" OnItemCommand="dgCondition_ItemCommand" ShowFooter="true"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundColumn DataField="id" Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="环节名称">
                            <ItemTemplate>
                                <input type="hidden" id="hidNodeModelID" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeModelID") %>' />
                                <asp:DropDownList ID="drpNodeName" runat="server">
                                </asp:DropDownList>
                                <input type="hidden" id="hidNodeName" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeName") %>' />                                
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <input id="HidAddNodeModelID" type="hidden" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeModelID") %>' />
                                <asp:DropDownList ID="drpAddNodeName" runat="server">
                                </asp:DropDownList>
                                <input id="HidAddNodeName" type="hidden" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.NodeName") %>' />
                                <asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="进度图片">
                            <ItemTemplate>
                                <asp:FileUpload ID="fileLinedImage" runat="server" />
                                <a href="../Forms/frmShowProgressImage.aspx?AppID=<%#DataBinder.Eval(Container.DataItem,"AppID") %>&OFlowModelID=<%#DataBinder.Eval(Container.DataItem,"OFlowModelID") %>&NodeModelID=<%#DataBinder.Eval(Container.DataItem,"NodeModelID") %>" target="_blank">                                    
                                    <%#GetShow(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "IsChangeImg")))%>
                                </a>                                
                                <input type="hidden" id="hidiamgeName" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.ImgURL") %>' />
                                <input type="hidden" id="hidIsChangeImg" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.IsChangeImg") %>' />
                                <input type="hidden" id="hidFileName" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.FileName") %>' />
                                <input type="hidden" id="hidUpFile" runat="server" value='<%# DataBinder.Eval(Container,"DataItem.UpFile") %>' />                                
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:FileUpload ID="fileLinedImageFoot" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="删除">
                            <HeaderStyle Width="3%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" OnClientClick="return confirm('确定删除吗？')"
                                    CausesValidation="False" SkinID="btnClass1"></asp:Button>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Button ID="btnadd" CommandName="Add" runat="server" Text="新增" CausesValidation="False"
                                    SkinID="btnClass1"></asp:Button>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="listTitle" />
                </asp:DataGrid>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        var temp = document.all.<%=hidTable.ClientID%>.value;
        if(temp!="")
        {
            var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
            var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
            var arr=temp.split(",");
            for(i=1;i<arr.length;i++)
            {   
                var tableid=arr[i];
                var tableCtrl = document.all.item(tableid);
                tableCtrl.style.display ="none";
                var ImgID = tableid.replace("Table","Img");
                var imgCtrl = document.all.item(ImgID)
                imgCtrl.src = ImgPlusScr ;	
            }
        }
    </script>

</asp:Content>
