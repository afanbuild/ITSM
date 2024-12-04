<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEquRelMain.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEquRelMain"
    Title="�ʲ�����" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/CtrEquNature.ascx" TagName="CtrEquNature" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style type="text/css">
        .jqmWindow {
    display: none;
    
    position: fixed;
    top: 17%;
    left: 50%;
    
    margin-left: -300px;
    width: 600px;
    
    background-color: #EEE;
    color: #333;
    border: 1px solid black;
    padding: 12px;
}


    </style>

    <script language="javascript" type="text/javascript">	
    //ѡ������ʲ�
    function SelectProblems()
	{
	    var newDateObj = new Date()
	    var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
	    var url = 'frmEqu_RelSelect.aspx?pDate='+sparamvalue 
		+ "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&EquID=" 
		+ '<%=EquID%>' + "&subjectid=" 
		+ '<%=CatalogID%>';
		
		var val = $('#<%=ddlRelName.ClientID %>').val();
		if (val > 0) {		    
		    url = url + '&ddlrelkeyindex=' + val;
		}
		
		window.open(url,"","dialogWidth=820px; dialogHeight=620px;status=no; help=no;scroll=auto;resizable=no") ;
    }
    
    
    
    
    
    </script>

    <input id="hidCustArrID" runat="server" type="hidden" />
    <input id="hidCustArrIDold" runat="server" type="hidden" />
    <input id="hidFlag" runat="server" type="hidden" />
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
        
    <table cellpadding='2' cellspacing='2' width='98%' border='0' class='listContent'>
        <tr style="display: none;">
            <td class="listTitleRight" style="width: 12%;">
                <asp:Literal ID="LitListName" runat="server" Text="�ʲ�Ŀ¼"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblListName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style='width: 12%;'>
                <asp:Literal ID="LitEquDeskName" runat="server" Text="����"></asp:Literal>
            </td>
            <td class='list' style='width: 35%;'>
                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight" style="width: 12%;">
                <asp:Literal ID="LitEquDeskCode" runat="server" Text="�ʲ����"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCode" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitMastCust" runat="server" Text="����λ"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblMastCust" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustom" runat="server" Text="�����ͻ�"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustom" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitEquDeskServiceTime" runat="server" Text="������"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblServiceBeginTime" runat="server" Text=""></asp:Label>
                ~
                <asp:Label ID="lblServiceEndTime" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitEquStatus" runat="server" Text="�ʲ�״̬"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblEquStatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitPartBankName" runat="server" Text="ά������"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblPartBankName" runat="server" Text=""></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitPartBranchName" Text="ά������" runat="server"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblPartBranchName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding='2' cellspacing='2' width='98%' border='0' class='listContent'>
        <tr id="trNewSave" runat="server">            
            <td align="center" class="listTitleNoAlign">
                <div style="display:none;">
                ѡ���������:<asp:DropDownList ID="ddlRelName"
                 AutoPostBack="true"
                  OnSelectedIndexChanged="ddlRelName_SelectedIndexChanged"
                 runat="server"></asp:DropDownList>
                <input type="button" id="btnAddRelName" value="�½�����" class="btnClass" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <asp:Button ID="btnAdd" runat="server" Text="����" OnClientClick="SelectProblems();"
                    OnClick="btnAdd_Click" />
                <asp:Button ID="btnAddHid" runat="server" Text="����" OnClick="btnAdd_Click" style="display:none;" />
                &nbsp;
                <asp:Button ID="btnSave" runat="server" Text="����" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
            <table width="98%" cellpadding="0" cellspacing="0" border="0" id="TablesTitle">
        <tr style="text-align: left">
            <td>
                <table border="0" height="29" cellpadding="0" cellspacing="0">
                    <tr style="cursor: hand">
                        <td width="7" valign="top"><img src="../Images/lm-left.gif" width="7" height="29" /></td>
                        <td width="115" height="29" align="center" valign="middle" id="name0" 
                        class="td_3 switchTab" 
                        background="../Images/lm-2b.gif"><span id="default" class="td_3">Ĭ���ӽ�</span></td>
                        <asp:Literal ID="literalRelKeyList" runat="server">
                            <td width="115" height="29" align="center" valign="middle" 
                            id="Td1" class="STYLE4 switchTab" 
                            background="../Images/lm-a.gif" 
                            style="display: block;"><span id="{0}" class="STYLE4">{1}</span></td>
                        </asp:Literal>                                                
                        <td width="7" valign="top"><img src="../Images/lm-right.gif" width="7" height="29" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgPro_ProblemAnalyse" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="dgPro_ProblemAnalyse_ItemCommand" OnItemCreated="dgPro_ProblemAnalyse_ItemCreated"
                    OnItemDataBound="dgPro_ProblemAnalyse_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <FooterStyle CssClass="listTitle" />
                    <Columns>
                        <asp:BoundColumn DataField='Equ_ID' HeaderText='EquID' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RelID' HeaderText='RelID' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='Name' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Code' HeaderText='�ʲ����' HeaderStyle-Width="12%"></asp:BoundColumn>

                        <asp:TemplateColumn HeaderText="�����ʲ�">
                            <ItemTemplate>                                
                            </ItemTemplate>
                            <HeaderStyle Width="20%" HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="�����ʲ�����" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="20%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <uc1:CtrEquNature ID="CtrEquNature1" runat="server" EquId='<%# DataBinder.Eval(Container, "DataItem.Equ_ID") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="��ϵ����">
                            <ItemTemplate>
                                <uc6:ctrFlowCataDropList ID="txtRelDescription_RelDescription" runat="server" RootID="1052" />
                            </ItemTemplate>
                            <HeaderStyle Width="15%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>                        
                        <asp:TemplateColumn HeaderText="ɾ��">
                            <HeaderStyle Width="8%" VerticalAlign='Top'></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="ɾ��" CausesValidation="False"
                                    SkinID="btnClass1"></asp:Button>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    
    <!--Begin: �������HTML�ṹ-->
    <div id="ex3a" class="jqmWindow">
        <div class="jqmdTL"><div class="jqmdTR"><div class="jqmdTC jqDrag">
        <h3>�½���������</h3>
        </div></div></div>
        <div class="jqmdBL"><div class="jqmdBR"><div class="jqmdBC">

        <div class="jqmdMSG">
            <div style="font-size:14px;">                
                <input style="padding:5px;" type="text" id="txtSearchKey"/>
            </div>

        <input type="button" id="btnConfirm" value="���!" class="btnClass" />
        <input type="button" id="btnCancel" value="ȡ��"  class="btnClass" />
        </div>

        </div></div></div>
    </div>
    <!--End: �������HTML�ṹ-->
    
    <!--Begin: �ⲿjs�ļ�����-->      
    <script language="javascript" type="text/javascript" src="../Js/jqModal.js"> </script>
    <script type="text/javascript" language="javascript" src="../js/epower.equ.frmEquRelMain.js"></script>  
    <!--End: �ⲿjs�ļ�����-->    
</asp:Content>
