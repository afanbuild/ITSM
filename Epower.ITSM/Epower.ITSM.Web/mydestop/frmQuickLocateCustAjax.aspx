<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmQuickLocateCustAjax.aspx.cs" Inherits="Epower.ITSM.Web.MyDestop.frmQuickLocateCustAjax"
    Title="���ٲ�ѯ�û�" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        function ServerOndblclick(jsonstr) {
            //==zxl==
           // window.parent.returnValue = jsonstr;
           if(jsonstr != null)
				{
				    var json = jsonstr;
				    var record=json.record;
				    
					for(var i=0; i < record.length; i++)
					{
			            window.opener.document.getElementById("<%=Opener_ClientId%>txtCustAddr").value = record[i].shortname;   //�ͻ�
			            window.opener.document.getElementById("<%=Opener_ClientId%>hidCust").value = record[i].shortname;
			            window.opener.document.getElementById( "<%=Opener_ClientId%>txtAddr").value = record[i].address;   //��ַ
			            window.opener.document.getElementById("<%=Opener_ClientId%>hidAddr").value = record[i].address;   //��ַ
			            window.opener.document.getElementById("<%=Opener_ClientId%>txtContact").value = record[i].linkman1;    //��ϵ��


			            window.opener.document.getElementById("<%=Opener_ClientId%>hidContact").value = record[i].linkman1;   
			            window.opener.document.getElementById("<%=Opener_ClientId%>txtCTel").value = record[i].tel1;   //��ϵ�˵绰


			            window.opener.document.getElementById("<%=Opener_ClientId%>hidTel").value = record[i].tel1;
			            window.opener.document.getElementById("<%=Opener_ClientId%>hidCustID").value = record[i].id;    //�ͻ�ID��

			           
			            window.opener.document.getElementById("<%=Opener_ClientId%>lblCustDeptName").innerHTML = record[i].custdeptname;   //��������
		                window.opener.document.getElementById("<%=Opener_ClientId%>lblEmail").innerHTML = record[i].email;  //�����ʼ�
		                window.opener.document.getElementById("<%=Opener_ClientId%>lblMastCust").innerHTML = record[i].mastcustname;   //����λ
		                window.opener.document.getElementById("<%=Opener_ClientId%>hidCustDeptName").value = record[i].custdeptname;   //��������
		                window.opener.document.getElementById("<%=Opener_ClientId%>hidCustEmail").value = record[i].email;  //�����ʼ�
		                window.opener.document.getElementById("<%=Opener_ClientId%>hidMastCust").value = record[i].mastcustname;   //����λ
		                window.opener.document.getElementById("<%=Opener_ClientId%>lbljob").innerHTML = record[i].job;   //ְλ
		                window.opener.document.getElementById("<%=Opener_ClientId%>hidjob").value = record[i].job;   //ְλ
			            
			            if (typeof(document.getElementById("<%=Opener_ClientId%>Table3"))!="undefined")
			            {
			                window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value = record[i].equname;   //�ʲ�����
                            window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value = record[i].equname;   //�ʲ�����
                            window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value = record[i].equid;  //�ʲ�IDontact")).id);                            
                            
                            window.opener.document.getElementById("<%=Opener_ClientId%>txtListName").value = record[i].listname;   //�ʲ�Ŀ¼
                            window.opener.document.getElementById("<%=Opener_ClientId%>hidListName").value = record[i].listname;   //�ʲ�Ŀ¼
                            window.opener.document.getElementById("<%=Opener_ClientId%>hidListID").value = record[i].listid;  //�ʲ�Ŀ¼ID
                            
                        }               
					}
				}
				else
				{
				     window.opener.document.getElementById("<%=Opener_ClientId%>hidCustID").value = "";    //�ͻ�ID��	
			         window.opener.document.getElementById("<%=Opener_ClientId%>txtCustAddr").value="";//�ͻ�����	
			                                                
			         window.opener.document.getElementById("<%=Opener_ClientId%>hidCust").value = "";
			         window.opener.document.getElementById("<%=Opener_ClientId%>txtAddr").value = "";   //��ַ
			         window.opener.document.getElementById("<%=Opener_ClientId%>hidAddr").value = "";   //��ַ
			         window.opener.document.getElementById("<%=Opener_ClientId%>txtContact").value = "";    //��ϵ��
			         window.opener.document.getElementById("<%=Opener_ClientId%>hidContact").value = "";   
			         window.opener.document.getElementById("<%=Opener_ClientId%>txtCTel").value = "";   //��ϵ�˵绰
			         window.opener.document.getElementById("<%=Opener_ClientId%>hidTel").value = "";			                                                            
			         window.opener.document.getElementById("<%=Opener_ClientId%>lblCustDeptName").innerHTML = "";   //��������
		             window.opener.document.getElementById("<%=Opener_ClientId%>lblEmail").innerHTML = "";  //�����ʼ�
		             window.opener.document.getElementById("<%=Opener_ClientId%>lblMastCust").innerHTML = "";   //����λ
		             window.opener.document.getElementById("<%=Opener_ClientId%>hidCustDeptName").value = "";   //��������
		             window.opener.document.getElementById("<%=Opener_ClientId%>hidCustEmail").value = "";  //�����ʼ�
		             window.opener.document.getElementById("<%=Opener_ClientId%>hidMastCust").value = "";   //����λ
		             window.opener.document.getElementById("<%=Opener_ClientId%>lbljob").innerHTML = "";   //ְλ
		             window.opener.document.getElementById("<%=Opener_ClientId%>hidjob").value = "";   //ְλ
				}           
            top.close();
        }
        function OnClientClick(selectcommandId)
        {            
            $("#" + selectcommandId ).click();
        }
    </script>

    <table id="Table1" bordercolor="#000000" cellspacing="1" bordercolordark="#ffffff"
        cellpadding="1" width="98%" bordercolorlight="#000000" border="0">
        <tr>
            <td valign="top" align="center" colspan="2" class="listContent">
                <asp:DataGrid ID="dgUserInfo" runat="server" Width="100%" PageSize="50" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    GridLines="Horizontal" CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCommand="dgUserInfo_ItemCommand"
                    OnItemDataBound="dgUserInfo_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="id" HeaderText="id"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ShortName" HeaderText="�ͻ�����">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Address" HeaderText="�ͻ���ַ">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LinkMan1" HeaderText="��ϵ��">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Tel1" HeaderText="��ϵ�˵绰">
                            <HeaderStyle />
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustomCode" HeaderText="�ͻ�����">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Email" HeaderText="�����ʼ�">
                            <HeaderStyle />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="MastCust" HeaderText="����λ">
                            <HeaderStyle />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="ѡ��">
                            <ItemTemplate>
                                <asp:Button runat="server" ID="lnkselect" CommandName="Select" Text="ѡ��" SkinID="btnClass1" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
                        Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td class="listTitle" align="right">
                <input id="hidQueryDeptID" style="width: 72px; height: 19px" type="hidden" size="6"
                    runat="server">
                <input id="hidDeptID" style="width: 72px; height: 19px" type="hidden" size="6" runat="server">
            </td>
        </tr>
    </table>
</asp:Content>
