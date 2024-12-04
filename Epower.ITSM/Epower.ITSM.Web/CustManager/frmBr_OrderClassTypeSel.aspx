<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmBr_OrderClassTypeSel.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmBr_OrderClassTypeSel"
    Title="班次选择" %>

<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function doubleSelect(jsonstr)
        {
        //====zxl==
        if(jsonstr != null)
				{	    
				   
				    var record=jsonstr.record;
				    
					for(var i=0; i < record.length; i++)
					{
					window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtClassTypeName").value=record[i].classtypename;//班次名称
					window.opener.document.getElementById("ctl00_ContentPlaceHolder1_HidClassTypeID").value=record[i].id; //班次ID
					
			           // document.getElementById(obj.id.replace("cmdClassType","txtClassTypeName")).value = record[i].classtypename;   //班次名称
			           // document.getElementById(obj.id.replace("cmdClassType","HidClassTypeID") ).value = record[i].id;	  //班次ID          			           
					}
				}
				else
				{
				        window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtClassTypeName").value=""; //班次名称
				        window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtClassTypeName").value=0; //班次ID
			           // document.getElementById(obj.id.replace("cmdClassType","txtClassTypeName")).value = "";   //班次名称
			           // document.getElementById(obj.id.replace("cmdClassType","HidClassTypeID") ).value = "0";	  //班次ID    
				}
        
            
            
            
        //=====
           // window.parent.returnValue = jsonstr;
           
            top.close();
        }        
    </script>

    <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                班次名称
            </td>
            <td class='list'>
                <asp:TextBox ID='txtClassTypeName' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgBr_OrderClassType" runat="server" Width="100%" OnItemDataBound="dgBr_OrderClassType_ItemDataBound"
                    OnItemCommand="dgBr_OrderClassType_ItemCommand" OnItemCreated="dgBr_OrderClassType_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='ClassTypeName' HeaderText='班次名称'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Remark' HeaderText='班次说明'></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="选择" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPageFoot ID="cpBr_OrderClassType" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
