<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmEqu_DeskMainImply.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_DeskMainImply" Title="被影响资产选择" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/BussinessControls/CustomCtr.ascx" TagName="CustomCtr"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <script type="text/javascript" language="javascript">
       var openobj = window;

       if (typeof (window.dialogArguments) == "object") {

           openobj = window.dialogArguments;

       }

       //关闭窗口时，将资产的名称传给父界面         
       function ServerOndblclick(arr)
       {
         if(arr != null)
		{
			var json = arr;
		    var record=json.record;
		    
			for(var i=0; i < record.length; i++)
			{	
		        window.opener.document.getElementById("<%=Opener_ClientId %>txtEqu").value=record[i].name; //设备名称
		        window.opener.document.getElementById("<%=Opener_ClientId %>hidEquName").value=record[i].name;//设备名称
		        window.opener.document.getElementById("<%=Opener_ClientId %>hidEqu").value=record[i].id;//设备id
		        window.opener.document.getElementById("<%=Opener_ClientId %>deskPropCh").click();
		        				
	        		 
			}
		}
		else
		{
		
		        window.opener.document.getElementById("<%=Opener_ClientId %>txtEqu").value="";  //设备名称
		        window.opener.document.getElementById("<%=Opener_ClientId %>hidEquName").value=""; //设备名称
		        window.opener.document.getElementById("<%=Opener_ClientId %>hidEqu").value=0; //设备id
		        window.opener.document.getElementById("<%=Opener_ClientId %>deskPropCh").click();
		}
         
         
         
           top.close();
       }

       function OpenDetail(obj) {
           var lngID = document.getElementById(obj.id.replace("lnkEquName", "hidID")).value;
           var surl = "frmEqu_DeskEdit.aspx?IsTanChu='true'&IsSelect=1&id=" + lngID;
           openobj.open(surl, "", "dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no");
       }
    </script>
    
<input id="hidIsTip" runat="server" type="hidden" />
    <input id="hidstrID" runat="server" type="hidden" />
    <input id="hidEquipmentCatalogID" type="hidden" value="0" runat="server" />
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class="listContent">
        <tr>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class='list' style="width: 35%">
                <asp:TextBox ID='txtName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskCode" runat="server" Text="资产编号"></asp:Literal>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCode' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgEqu_Desk" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgEqu_Desk_ItemCommand"
                    OnItemDataBound="dgEqu_Desk_ItemDataBound" OnItemCreated="dgEqu_Desk_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="资产名称">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEquName" runat="server" OnClientClick='OpenDetail(this)' Text='<%#DataBinder.Eval(Container,"DataItem.Name") %>'></asp:LinkButton>
                                <input type="hidden" runat="server" value='<%#DataBinder.Eval(Container, "DataItem.ID")%>' id="hidID" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='Code' HeaderText='资产编号'></asp:BoundColumn>
                        <asp:BoundColumn DataField='costomname' HeaderText='所属客户'></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Button ID="btnSel" SkinID="btnClass1" runat="server" Text="选择" CommandName="Sel" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc4:ControlPageFoot ID="cpfECustomerInfo" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
