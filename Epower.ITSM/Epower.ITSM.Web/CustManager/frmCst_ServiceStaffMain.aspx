<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCst_ServiceStaffMain.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCst_ServiceStaffMain"
    Title="����ʦ�б�" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function CancelconfirmMast()     //ȡ��
        {
            top.close();
            event.returnValue = false;
        }
        //ȫѡ��ѡ��
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgCst_ServiceStaff") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }

        function doubleSelect(id, name, dept) {
            var arr = new Array();
            arr[1] = id;
            arr[2] = name;
            arr[3] = dept;
            var typeFrm="<%=TypeFrm%>";
            var type="<%=type%>";
            if(typeFrm =="frmCst_RecommendRuleEdit")
            {
             frmCst_RecommendRuleEdit(arr,type);
                   
            }
            else{
            
            
                //===================
               // window.parent.returnValue = arr;
               if (arr != null) {
                if (arr.length > 1) {
                    window.opener.document.getElementById("<%=Opener_ClientId %>txtUser").value = arr[2];
                    window.opener.document.getElementById("<%=Opener_ClientId %>hidUserName").value = arr[2];
                    window.opener.document.getElementById("<%=Opener_ClientId %>hidUser").value = arr[1];
                }
                else {
                     window.opener.document.getElementById("<%=Opener_ClientId %>txtUser").value = "";
                     window.opener.document.getElementById("<%=Opener_ClientId %>hidUserName").value = "";
                     window.opener.document.getElementById("<%=Opener_ClientId %>hidUser").value = 0;
                }
            }
            else {
                 window.opener.document.getElementById("<%=Opener_ClientId %>txtUser").value = "";
                 window.opener.document.getElementById("<%=Opener_ClientId %>hidUserName").value = "";
                 window.opener.document.getElementById("<%=Opener_ClientId %>hidUser").value = 0;
            }
            //=======
        }
           
           
            top.close();
        }
        //��������ʦ���
        function PrintValidate() {
            var breturn = false;
            var strUrl = "../mydestop/frmUsersMult.htm";

            return breturn;
        }
    </script>
    <script type="text/javascript">
        function frmCst_RecommendRuleEdit(value,type)
        {
            //====================
           
            if(value != null)
		    {
					if(value.length>1)
					{
					    if(type=="2")   //����Ϊ1��ʾ�޸�
					    {
					     
					       window.opener.document.getElementById("<%=Opener_ClientId %>txtUserName").value=value[2].replace("&nbsp;", "");
					       window.opener.document.getElementById("<%=Opener_ClientId %>hidStaffID").value=value[1].replace("&nbsp;", "");   //ID
					       window.opener.document.getElementById("<%=Opener_ClientId %>hidBlongDeptName").value=value[3].replace("&nbsp;", "");   //����λ
					       window.opener.document.getElementById("<%=Opener_ClientId %>txtUserName").focus();
					       
				        }
				        else
				        {
				         window.opener.document.getElementById("<%=Opener_ClientId %>txtAddUserName").value=value[2].replace("&nbsp;", "");  //����
				         window.opener.document.getElementById("<%=Opener_ClientId %>hidAddStaffID").value=value[1].replace("&nbsp;", "");   //ID
				         window.opener.document.getElementById("<%=Opener_ClientId %>hidAddBlongDeptName").value =value[3].replace("&nbsp;", "");   //����λ
				         window.opener.document.getElementById("<%=Opener_ClientId %>lblAddBlongDeptName").innerText =value[3].replace("&nbsp;", "");   //����λ
				         window.opener.document.getElementById("<%=Opener_ClientId %>txtAddUserName").focus();
				            
				        }
					}
				}
				else
				{
				    if(type=="2")   //����Ϊ1��ʾ�޸�
				    {
					     if(type=="2")   //����Ϊ1��ʾ�޸�
					    {
					        window.opener.document.getElementById("<%=Opener_ClientId %>txtUserName").value ="";
					        window.opener.document.getElementById("<%=Opener_ClientId %>hidStaffID").value="0";
					        window.opener.document.getElementById("<%=Opener_ClientId %>hidBlongDeptName").value="0";
					        window.opener.document.getElementById("<%=Opener_ClientId %>txtUserName").focus();
					        
					        
					        
				        }
				        else
				        {
				        
				             window.opener.document.getElementById("<%=Opener_ClientId %>txtAddUserName").value=""; //����
				             window.opener.document.getElementById("<%=Opener_ClientId %>hidAddStaffID")="0"; //ID
				             window.opener.document.getElementById("<%=Opener_ClientId %>hidAddBlongDeptName").value =""; //����λ
				             window.opener.document.getElementById("<%=Opener_ClientId %>txtAddUserName").focus();
				        }
				    }
			   }
            
            //====================
            
        }
        
    </script>

    <input type="hidden" id="hidUserName" runat="server" />
    <input type="hidden" id="hidUserID" runat="server" />
    <table runat="server" id="tblSelect" width='98%' class='listContent Gridtable' cellpadding="2"
        cellspacing="0">
        <tr>
            <td class='listTitle' align="right" style='width: 12%;'>
                ����λ
            </td>
            <td class='list' style="width: 35%;">
                <asp:Literal ID="ltlMastCust" runat="server"></asp:Literal>
            </td>
            <td class='listTitle' align="right" style='width: 12%;'>
                �¼����
            </td>
            <td class='list'>
                <asp:Literal ID="lblTypeName" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align="right">
                ���񼶱�
            </td>
            <td class='list'>
                <asp:Literal ID="lblLevelName" runat="server"></asp:Literal>
            </td>
            <td class='listTitle' align="right">
                �ʲ�����
            </td>
            <td class='list'>
                <asp:Literal ID="lblEquName" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align="right">
                �û�����
            </td>
            <td class='list' colspan="3">
                <asp:Literal ID="lblCustName" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <table runat="server" id="tabledept" width='98%' class='listContent Gridtable' cellpadding="2"
        cellspacing="0">
        <tr runat="server" id="trdept" align="center">
            <td class='listTitle' align="right" style='width: 12%;'>
                ����ʦ����
            </td>
            <td class='list' align="left" style="width: 35%;">
                <uc4:CtrFlowFormText ID="CtrFlowName" runat="server" />
            </td>
            <td class='listTitle' align="right" style='width: 12%;'>
                ����λ
            </td>
            <td class='list' align="left">
                <asp:DropDownList ID="ddltMastCustID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddltMastCustID_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                ��Ӧ�û�
            </td>
            <td class="list">
                <uc3:UserPicker ID="RefUser" runat="server" MustInput="false" TextToolTip="��Ӧ�û�">
                </uc3:UserPicker>
            </td>
            <td colspan="2" class="list">
                <asp:RadioButton ID="rbtnRU" runat="server" Text="�Ƽ�����ʦ" Checked="true" GroupName="MastCust"
                    AutoPostBack="True" OnCheckedChanged="rbtnRU_CheckedChanged" /><asp:RadioButton ID="rbtnAll"
                        runat="server" Text="����" GroupName="MastCust" AutoPostBack="True" OnCheckedChanged="rbtnAll_CheckedChanged" />
            </td>
        </tr>
        <tr runat="server" id="trselect" class='list'>
            <td colspan="4" align="center" class="listNoAlign">
                <asp:Button ID="btnSelect" runat="server" Text="ѡ  ��" OnClick="btnSelect_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="ȡ  ��" OnClientClick="CancelconfirmMast();" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td align="center">
                <asp:DataGrid ID="dgCst_ServiceStaff" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCommand="dgCst_ServiceStaff_ItemCommand"
                    OnItemCreated="dgCst_ServiceStaff_ItemCreated" OnItemDataBound="dgCst_ServiceStaff_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText=' ����ʦ����'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='BlongDeptName' HeaderText='����λ'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='UserName' HeaderText=' ��Ӧ�û�'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="JoinDate" HeaderText=" ��ְʱ��" DataFormatString="{0:d}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='OrderIndex' HeaderText='����' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='UserID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="�޸�">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="�޸�" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc2:ControlPageFoot ID="cpServiceStaff" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
